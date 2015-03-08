using System;
using System.Collections.Generic;
using System.Windows.Input;
using Microsoft.TeamFoundation.Controls.WPF.TeamExplorer;
using Microsoft.TeamFoundation.MVVM;
using Microsoft.VisualStudio.Shell;
using Spiral.TfsEssentials.Models;
using Spiral.TfsEssentials.Providers;
using Task = System.Threading.Tasks.Task;

namespace Spiral.TfsEssentials.ViewModels
{
	internal class BranchDropDownViewModel : ViewModelBase
	{
		private readonly TeamExplorerPageViewModelBase teamExplorerPageViewModelBase;
		private readonly TfsBranchProvider tfsBranchProvider;
		private BranchModel currentBranch;
		private List<BranchModel> branches;

		public BranchDropDownViewModel(TeamExplorerPageViewModelBase teamExplorerPageViewModelBase, TfsBranchProvider tfsBranchProvider)
			: base(teamExplorerPageViewModelBase)
		{
			this.teamExplorerPageViewModelBase = teamExplorerPageViewModelBase;
			this.tfsBranchProvider = tfsBranchProvider;

			SelectBranchCommand = new RelayCommand(SelectBranch);

			//Refresh();
		}

		private void SelectBranch(object obj)
		{
			var branch = obj as BranchModel;
			if (branch == null)
			{
				return;
			}

			//if (!this.Branches.Contains(branch))
			//{
			//	return;
			//}

			tfsBranchProvider.SetCurrentBranch(branch);
			this.CurrentBranch = branch;
		}

		public ICommand SelectBranchCommand { get; private set; }

		[ValueDependsOnProperty("CurrentBranch")]
		public string CurrentBranchName
		{
			get
			{
				return CurrentBranch == null ? "No Branches" : CurrentBranch.Name;
			}
		}

		public BranchModel CurrentBranch
		{
			get
			{
				return currentBranch;
			}
			set
			{
				SetAndRaisePropertyChanged(ref currentBranch, value, "CurrentBranch");
			}
		}

		public List<BranchModel> Branches
		{
			get
			{
				return branches;
			}
			set
			{
				SetAndRaisePropertyChanged(ref branches, value, "Branches");
			}
		}

		public void Refresh()
		{
			teamExplorerPageViewModelBase.IsBusy = true;

			Task.Run(async delegate
			{
				var currentBranches = tfsBranchProvider.GetBranches();
				var currentBranchName = tfsBranchProvider.GetCurrentBranch();

				await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

				Branches = currentBranches;
				this.CurrentBranch = currentBranchName;
				this.teamExplorerPageViewModelBase.IsBusy = false;
			});
		}
	}
}