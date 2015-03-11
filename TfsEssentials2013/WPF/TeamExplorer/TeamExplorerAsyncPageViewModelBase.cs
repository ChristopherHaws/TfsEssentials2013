using System.ComponentModel;
using Microsoft.TeamFoundation.Controls.WPF.TeamExplorer;

namespace Spiral.TfsEssentials.WPF.TeamExplorer
{
	internal class TeamExplorerAsyncPageViewModelBase<T> : TeamExplorerPageViewModelBase, IAsyncViewModelRefresh<T>
	{
		public virtual T BeginRefresh(RefreshReason reason, CancelEventArgs e)
		{
			return default(T);
		}

		public virtual void RefreshCompleted(T result, RefreshReason reason, AsyncCompletedEventArgs e)
		{
		}
	}
}