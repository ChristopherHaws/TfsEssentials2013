using Microsoft.TeamFoundation.Common.Internal;
using Microsoft.TeamFoundation.Controls;
using Microsoft.TeamFoundation.Controls.WPF.TeamExplorer;
using Microsoft.TeamFoundation.Threading;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
			this.TaskFactory = new TaskFactory(this.CancellationTokenSource.Token, TaskCreationOptions.HideScheduler, TaskContinuationOptions.None, (System.Threading.Tasks.TaskScheduler)this.TaskScheduler);
			base.Initialize(sender, e);
			IServiceContainer service = IServiceProviderExtensions.GetService<IServiceContainer>(this.ServiceProvider);
			if (service == null)
				return;
			service.AddService(typeof(TaskFactory), (object)this.TaskFactory);
		}

		public override void Dispose()
		{
			base.Dispose();
			this.CancellationTokenSource.Cancel();
			IServiceContainer service = IServiceProviderExtensions.GetService<IServiceContainer>(this.ServiceProvider);
			if (service != null)
				service.RemoveService(typeof(TaskFactory));
			this.TaskFactory = (TaskFactory)null;
			this.TaskScheduler = (OrderedTaskScheduler)null;
		}

		public Task QueueWorkAsync(Action action)
		{
			return this.TaskFactory.StartNew(action);
		}

		public Task QueueWorkAsync(Action<CancellationToken> action)
		{
			return this.TaskFactory.StartNew((Action)(() => action(this.CancellationTokenSource.Token)));
		}
	}
}
