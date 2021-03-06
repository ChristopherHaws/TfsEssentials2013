using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Common.Internal;
using Microsoft.TeamFoundation.Controls;
using Microsoft.TeamFoundation.Controls.WPF.TeamExplorer;
using Microsoft.TeamFoundation.Threading;
using Spiral.TfsEssentials.Managers;

namespace Spiral.TfsEssentials.WPF.TeamExplorer
{
	internal class TeamExplorerAsyncPageBase<T> : TeamExplorerAsyncPageBase, IAsyncWorkManager, IAsyncRefresh
	{
		protected OrderedTaskScheduler TaskScheduler { get; set; }

		protected TaskFactory TaskFactory { get; set; }

		protected CancellationTokenSource CancellationTokenSource { get; set; }

		public override void Initialize(object sender, PageInitializeEventArgs e)
		{
			this.TaskScheduler = new OrderedTaskScheduler();
			this.CancellationTokenSource = new CancellationTokenSource();
			this.TaskFactory = new TaskFactory(this.CancellationTokenSource.Token, TaskCreationOptions.HideScheduler, TaskContinuationOptions.None, this.TaskScheduler);

			base.Initialize(sender, e);

			var service = this.ServiceProvider.GetService<IServiceContainer>();
			if (service == null)
			{
				return;
			}

			service.AddService(typeof(TaskFactory), this.TaskFactory);
		}

		protected ITeamFoundationContext CurrentContext
		{
			get
			{
				var tfContextManager = GetService<ITeamFoundationContextManager>();
				if (tfContextManager != null)
				{
					return tfContextManager.CurrentContext;
				}

				return null;
			}
		}

		public override void Dispose()
		{
			base.Dispose();

			this.CancellationTokenSource.Cancel();

			var service = this.ServiceProvider.GetService<IServiceContainer>();
			if (service != null)
			{
				service.RemoveService(typeof(TaskFactory));
			}

			this.TaskFactory = null;
			this.TaskScheduler = null;
		}

		public Task QueueWorkAsync(Action action)
		{
			return this.TaskFactory.StartNew(action);
		}

		public Task QueueWorkAsync(Action<CancellationToken> action)
		{
			return this.TaskFactory.StartNew((() => action(this.CancellationTokenSource.Token)));
		}

		public TService GetService<TService>()
		{
			Debug.Assert(this.ServiceProvider != null, "GetService<T> called before service provider is set");
			if (this.ServiceProvider == null)
			{
				return default(TService);
			}

			return (TService)this.ServiceProvider.GetService(typeof(TService));
		}

		public void OnPageRefreshStarted(RefreshReason reason)
		{
			var viewModel = this.ViewModel as TeamExplorerAsyncPageViewModelBase<T>;
			if (viewModel == null)
			{
				return;
			}

			viewModel.IsBusy = true;
		}

		public void OnPageRefreshCompleted(RefreshReason reason)
		{
			var viewModel = this.ViewModel as TeamExplorerAsyncPageViewModelBase<T>;
			if (viewModel == null)
			{
				return;
			}

			viewModel.IsBusy = false;
		}

		public object BeginRefresh(RefreshReason reason, CancelEventArgs e)
		{
			var viewModel = this.ViewModel as TeamExplorerAsyncPageViewModelBase<T>;
			if (viewModel == null)
			{
				return null;
			}

			return viewModel.BeginRefresh(reason, e);
		}

		public void RefreshCompleted(object result, RefreshReason reason, AsyncCompletedEventArgs e)
		{
			var viewModel = this.ViewModel as TeamExplorerAsyncPageViewModelBase<T>;
			if (viewModel != null)
			{
				viewModel.RefreshCompleted((T)result, reason, e);
			}

			this.ViewModel.Refresh();
		}
	}
}
