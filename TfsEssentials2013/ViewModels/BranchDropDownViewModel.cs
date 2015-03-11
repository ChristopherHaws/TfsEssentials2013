using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using Microsoft.TeamFoundation.Controls.WPF.TeamExplorer;
using Microsoft.TeamFoundation.MVVM;
using Spiral.TfsEssentials.Models;
using Spiral.TfsEssentials.Providers;

namespace Spiral.TfsEssentials.ViewModels
{
	internal class BranchDropDownViewModel : ViewModelBase
	{
		private readonly TfsBranchProvider tfsBranchProvider;

		public BranchDropDownViewModel(TeamExplorerPageViewModelBase teamExplorerPageViewModelBase, TfsBranchProvider tfsBranchProvider)
			: base(teamExplorerPageViewModelBase)
		{
			this.tfsBranchProvider = tfsBranchProvider;

			SelectBranchCommand = new RelayCommand(SelectBranch);
		}

		private void SelectBranch(object obj)
		{
			var branch = obj as BranchModel;
			if (branch == null)
			{
				return;
			}

			tfsBranchProvider.SetCurrentBranch(branch);
			this.SelectedBranch = branch;
		}

		public ICommand SelectBranchCommand { get; private set; }

		[ValueDependsOnProperty("SelectedBranch")]
		public string SelectedBranchName
		{
			get
			{
				return SelectedBranch == null ? "No Branches" : SelectedBranch.Name;
			}
		}

		public BranchModel SelectedBranch { get; set; }

		public List<BranchModel> Branches { get; set; }

		public BranchDropDownViewModelRefreshArgs BeginRefresh(RefreshReason reason, CancelEventArgs e)
		{
			return new BranchDropDownViewModelRefreshArgs
			{
				Branches = tfsBranchProvider.GetBranches(),
				SelectedBranch = tfsBranchProvider.GetCurrentBranch()
			};
		}

		public void RefreshCompleted(BranchDropDownViewModelRefreshArgs result, RefreshReason reason, AsyncCompletedEventArgs e)
		{
			this.Branches = result.Branches;
			this.SelectedBranch = result.SelectedBranch;
		}
	}

	internal class BranchDropDownViewModelRefreshArgs
	{
		public BranchModel SelectedBranch { get; set; }

		public List<BranchModel> Branches { get; set; }
	}
}