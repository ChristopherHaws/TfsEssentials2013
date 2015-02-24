using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Spiral.TfsEssentials.WPF.TeamExplorer
{
	/// <summary>
	/// Team Explorer plugin common base class.
	/// </summary>
	public class TeamExplorerBase : NotifyPropertyChangedBase, IDisposable
	{
		public static Guid TfsProviderGuid = new Guid("4CA58AB2-18FA-4F8D-95D4-32DDF27D184C");
		public static Guid GitProviderGuid = new Guid("11b8e6d7-c08b-4385-b321-321078cdd1f8");

		private bool _contextSubscribed = false;

		/// <summary>
		/// Gets or sets the service provider.
		/// </summary>
		public IServiceProvider ServiceProvider
		{
			get { return _serviceProvider; }
			set
			{
				// Unsubscribe from Team Foundation context changes
				if (_serviceProvider != null)
				{
					UnsubscribeContextChanges();
				}

				_serviceProvider = value;

				// Subscribe to Team Foundation context changes
				if (_serviceProvider != null)
				{
					SubscribeContextChanges();
				}
			}
		}
		private IServiceProvider _serviceProvider = null;

		/// <summary>
		/// Get the requested service from the service provider.
		/// </summary>
		public T GetService<T>()
		{
			Debug.Assert(this.ServiceProvider != null, "GetService<T> called before service provider is set");
			if (this.ServiceProvider != null)
			{
				return (T)this.ServiceProvider.GetService(typeof(T));
			}

			return default(T);
		}

		/// <summary>
		/// Clears all notifications in the Team Explorer window that don't require confirmation
		/// </summary>
		protected void ClearNotifications()
		{
			ITeamExplorer teamExplorer = GetService<ITeamExplorer>();
			if (teamExplorer != null)
			{
				teamExplorer.ClearNotifications();
			}
		}

		/// <summary>
		/// Show a notification in the Team Explorer window.
		/// </summary>
		protected Guid ShowNotification(string message, NotificationType type)
		{
			ITeamExplorer teamExplorer = GetService<ITeamExplorer>();
			if (teamExplorer != null)
			{
				Guid guid = Guid.NewGuid();
				teamExplorer.ShowNotification(message, type, NotificationFlags.None, null, guid);
				return guid;
			}

			return Guid.Empty;
		}

		/// <summary>
		/// Show a notification in the Team Explorer window with a hyperlink to execute
		/// </summary>
		protected Guid ShowNotification(string message, NotificationType type, ICommand command)
		{
			ITeamExplorer teamExplorer = GetService<ITeamExplorer>();
			if (teamExplorer != null)
			{
				Guid guid = Guid.NewGuid();
				teamExplorer.ShowNotification(message, type, NotificationFlags.None, command, guid);
				return guid;
			}

			return Guid.Empty;
		}

		/// <summary>
		/// Dispose.
		/// </summary>
		public virtual void Dispose()
		{
			UnsubscribeContextChanges();
		}

		/// <summary>
		/// Subscribe to context changes.
		/// </summary>
		protected void SubscribeContextChanges()
		{
			Debug.Assert(this.ServiceProvider != null, "ServiceProvider must be set before subscribing to context changes");
			if (this.ServiceProvider == null || _contextSubscribed)
			{
				return;
			}

			ITeamFoundationContextManager tfContextManager = GetService<ITeamFoundationContextManager>();
			if (tfContextManager != null)
			{
				tfContextManager.ContextChanged += ContextChanged;
				_contextSubscribed = true;
			}
		}

		/// <summary>
		/// Unsubscribe from context changes.
		/// </summary>
		protected void UnsubscribeContextChanges()
		{
			if (this.ServiceProvider == null || !_contextSubscribed)
			{
				return;
			}

			ITeamFoundationContextManager tfContextManager = GetService<ITeamFoundationContextManager>();
			if (tfContextManager != null)
			{
				tfContextManager.ContextChanged -= ContextChanged;
			}
		}

		/// <summary>
		/// ContextChanged event handler.
		/// </summary>
		protected virtual void ContextChanged(object sender, ContextChangedEventArgs e)
		{
		}

		/// <summary>
		/// Get the current Team Foundation context.
		/// </summary>
		protected ITeamFoundationContext CurrentContext
		{
			get
			{
				ITeamFoundationContextManager tfContextManager = GetService<ITeamFoundationContextManager>();
				if (tfContextManager != null)
				{
					return tfContextManager.CurrentContext;
				}

				return null;
			}
		}
	}
}
