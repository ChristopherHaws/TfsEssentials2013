using System.ComponentModel;
using Microsoft.TeamFoundation.Controls.WPF.TeamExplorer;

namespace Spiral.TfsEssentials.WPF.TeamExplorer
{
	internal class TeamExplorerAsyncSectionBase<T> : TeamExplorerSectionBase, IAsyncRefresh
	{
		public void OnPageRefreshStarted(RefreshReason reason)
		{
			var viewModel = this.ViewModel as TeamExplorerAsyncViewModelBase<T>;
			if (viewModel == null)
			{
				return;
			}

			viewModel.IsBusy = true;
		}

		public void OnPageRefreshCompleted(RefreshReason reason)
		{
			var viewModel = this.ViewModel as TeamExplorerAsyncViewModelBase<T>;
			if (viewModel == null)
			{
				return;
			}

			viewModel.IsBusy = false;
		}

		public object BeginRefresh(RefreshReason reason, CancelEventArgs e)
		{
			var viewModel = this.ViewModel as TeamExplorerAsyncViewModelBase<T>;
			if (viewModel == null)
			{
				return null;
			}

			return viewModel.BeginRefresh(reason, e);
		}

		public void RefreshCompleted(object result, RefreshReason reason, AsyncCompletedEventArgs e)
		{
			var viewModel = this.ViewModel as TeamExplorerAsyncViewModelBase<T>;
			if (viewModel != null)
			{
				viewModel.RefreshCompleted((T)result, reason, e);
			}

			this.ViewModel.Refresh();
		}
	}
}