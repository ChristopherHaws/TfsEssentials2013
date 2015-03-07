using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.TeamFoundation.Controls;
using Microsoft.VisualStudio.Shell;
using Spiral.TfsEssentials.Extentions;

namespace Spiral.TfsEssentials.Providers
{
	[Export]
	internal class TeamExplorerProvider : ITeamExplorer
	{
		private readonly ITeamExplorer teamExplorer;

		[ImportingConstructor]
		public TeamExplorerProvider([Import(typeof(SVsServiceProvider))] IServiceProvider serviceProvider)
		{
			this.teamExplorer = serviceProvider.GetService<ITeamExplorer>();
		}

		/// <summary>
		/// Gets the service object of the specified type.
		/// </summary>
		/// <returns>
		/// A service object of type <paramref name="serviceType"/>.-or- null if there is no service object of type <paramref name="serviceType"/>.
		/// </returns>
		/// <param name="serviceType">An object that specifies the type of service object to get. </param>
		public object GetService(Type serviceType)
		{
			return teamExplorer.GetService(serviceType);
		}

		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// Navigate to the specified page.
		/// </summary>
		/// <returns>
		/// Returns <see cref="T:Microsoft.TeamFoundation.Controls.ITeamExplorerPage"/>.
		/// </returns>
		/// <param name="pageId">Page ID</param><param name="context">(Optional) Page context. This can be null if the page does not need context.</param>
		public ITeamExplorerPage NavigateToPage(Guid pageId, object context)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Close the specified page.
		/// </summary>
		/// <param name="page">Page instance</param>
		public void ClosePage(ITeamExplorerPage page)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Show a notification message in the Team Explorer tool window.
		/// </summary>
		/// <param name="message">The message text to display for the notification.</param><param name="type">This indicates what kind of icon the notification will get.</param><param name="flags">These flags indicate the behavior of the notification.</param><param name="command">This allows the owning page/object to be called back for all embedding links instead of allowing the default handler to attempt to handle them.</param><param name="id">Used to define a group of notifications. Only one notification shown with a particular id will be visible at a time.</param>
		public void ShowNotification(string message, NotificationType type, NotificationFlags flags, ICommand command, Guid id)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Removes the notification show using the specified id from the notification area.
		/// </summary>
		/// <returns>
		/// True if a notification was removed. False if a notification associated with id was not currently visible.
		/// </returns>
		/// <param name="id">The id of the notification that should be removed.</param>
		public bool HideNotification(Guid id)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Determine whether a notification associated with the specified id is currently visible in the Team Explorer.
		/// </summary>
		/// <returns>
		/// Returns <see cref="T:System.Boolean"/>.
		/// </returns>
		/// <param name="id">The id that was passed into ShowNotification for a particular notification.</param>
		public bool IsNotificationVisible(Guid id)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Clears all notifications that do not require the user's confirmation.
		/// </summary>
		public void ClearNotifications()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Get the current page. This can be null if there is no current page. The PropertyChanged event will be fired when this property changes.
		/// </summary>
		/// <returns>
		/// Returns <see cref="T:Microsoft.TeamFoundation.Controls.ITeamExplorerPage"/>.
		/// </returns>
		public ITeamExplorerPage CurrentPage
		{
			get { throw new NotImplementedException(); }
		}
	}
}
