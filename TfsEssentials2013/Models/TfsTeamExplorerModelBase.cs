using System;
using System.Threading.Tasks;
using Microsoft.TeamFoundation.MVVM;
using Spiral.TfsEssentials.Managers;

namespace Spiral.TfsEssentials.Models
{
	internal class TfsTeamExplorerModelBase : NotifyPropertyChangedDispatcherObject, IDisposable, IAsyncWorkManager
	{
		private bool isBusy;

		protected IServiceProvider ServiceProvider { get; set; }

		public bool IsBusy
		{
			get
			{
				return this.isBusy;
			}
			set
			{
				this.SetAndRaisePropertyChanged(ref this.isBusy, value, "IsBusy");
			}
		}

		protected TaskFactory TaskFactory { get; set; }

		public TfsTeamExplorerModelBase(IServiceProvider serviceProvider, TaskFactory taskFactory)
		{
			this.ServiceProvider = serviceProvider;
			this.TaskFactory = taskFactory;
		}

		public virtual void Dispose()
		{
		}

		public Task QueueWorkAsync(Action action)
		{
			return this.TaskFactory.StartNew(action);
		}
	}
}