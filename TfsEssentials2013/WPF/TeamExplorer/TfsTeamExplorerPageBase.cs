using System;
using System.ComponentModel.Design;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.TeamFoundation.Common.Internal;
using Microsoft.TeamFoundation.Controls;
using Microsoft.TeamFoundation.Controls.WPF.TeamExplorer;
using Microsoft.TeamFoundation.Threading;

namespace Spiral.TfsEssentials.WPF.TeamExplorer
{
	internal class TfsTeamExplorerPageBase : TeamExplorerPageBase, IAsyncWorkManager
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
	}
}
