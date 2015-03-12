using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Windows.Input;
using Microsoft.TeamFoundation.Controls;
using Microsoft.TeamFoundation.Controls.WPF.TeamExplorer;
using Microsoft.TeamFoundation.MVVM;
using Spiral.TfsEssentials.Models;
using Spiral.TfsEssentials.Providers;
using Spiral.TfsEssentials.WPF.TeamExplorer;

namespace Spiral.TfsEssentials.ViewModels
{
	internal class MergePageViewModel : TeamExplorerAsyncPageViewModelBase<MergePageViewModelRefreshArgs>
	{
		private readonly ChangesetProvider changesetProvider;

		public MergeModel Model { get; set; }

		public BranchDropDownViewModel BranchDropDownViewModel { get; private set; }

		public ICommand ViewPendingChangesCommand { get; private set; }

		public ICommand PullCommand { get; private set; }

		public ICommand PushCommand { get; private set; }

		public ICommand SyncCommand { get; private set; }

		public MergePageViewModel(
			MergeModel model,
			TfsBranchProvider tfsBranchProvider,
			ChangesetProvider changesetProvider)
		{
			this.changesetProvider = changesetProvider;

			this.Title = "Merge";
			this.Model = model;
			this.BranchDropDownViewModel = new BranchDropDownViewModel(this, tfsBranchProvider);

			ViewPendingChangesCommand = new RelayCommand(NavigateToPendingChangesPage);
			PullCommand = new RelayCommand(Pull);
			PushCommand = new RelayCommand(Push);
			SyncCommand = new RelayCommand(Sync);
		}

		private void NavigateToPendingChangesPage()
		{
			try
			{
				TeamExplorerUtils.Instance.NavigateToPage(TeamExplorerPageIds.PendingChanges, this.ServiceProvider, null);
			}
			catch (Exception ex)
			{
				this.ShowException(ex);
			}
		}

		private void Pull()
		{
		}
		private void Push()
		{
		}
		private void Sync()
		{
			this.Model.HasUpstreamInfo = !this.Model.HasUpstreamInfo;
		}

		public override MergePageViewModelRefreshArgs BeginRefresh(RefreshReason reason, CancelEventArgs e)
		{
			var branchArgs = this.BranchDropDownViewModel.BeginRefresh(reason, e);
			var incomingChangsets = changesetProvider.GetIncomingChangesets(branchArgs.SelectedBranch);
			var outgoingChangsets = changesetProvider.GetOutgoingChangesets(branchArgs.SelectedBranch);

			return new MergePageViewModelRefreshArgs
			{
				BranchDropDownViewModelRefreshArgs = branchArgs,
				IncomingChangesets = incomingChangsets,
				OutgoingChangesets = outgoingChangsets
			};
		}

		public override void RefreshCompleted(MergePageViewModelRefreshArgs result, RefreshReason reason, AsyncCompletedEventArgs e)
		{
			if (result == null)
			{
				return;
			}

			this.BranchDropDownViewModel.RefreshCompleted(result.BranchDropDownViewModelRefreshArgs, reason, e);
			this.Model.IncomingChangesets = result.IncomingChangesets;
			this.Model.OutgoingChangesets = result.OutgoingChangesets;
		}
	}

	internal class MergePageViewModelRefreshArgs
	{
		public MergePageViewModelRefreshArgs()
		{
			this.IncomingChangesets = new List<ChangesetModel>();
			this.OutgoingChangesets = new List<ChangesetModel>();
		}

		public BranchDropDownViewModelRefreshArgs BranchDropDownViewModelRefreshArgs { get; set; }

		public List<ChangesetModel> IncomingChangesets { get; set; }

		public List<ChangesetModel> OutgoingChangesets { get; set; }
	}
}
