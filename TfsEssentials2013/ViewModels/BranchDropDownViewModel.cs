using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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

		public BranchDropDownViewModel(TeamExplorerPageViewModelBase teamExplorerPageViewModelBase, TfsBranchProvider tfsBranchProvider)
			: base(teamExplorerPageViewModelBase)
		{
			this.teamExplorerPageViewModelBase = teamExplorerPageViewModelBase;
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

		public void Refresh()
		{
			teamExplorerPageViewModelBase.IsBusy = true;

			Task.Run(async delegate
			{
				var currentBranches = tfsBranchProvider.GetBranches();
				var currentBranchName = tfsBranchProvider.GetCurrentBranch();

				await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

				this.Branches = currentBranches;
				this.SelectedBranch = currentBranchName;
				this.teamExplorerPageViewModelBase.IsBusy = false;
			});
		}
	}
}