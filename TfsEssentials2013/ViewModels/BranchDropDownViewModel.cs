using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.TeamFoundation.Controls.WPF.TeamExplorer;
using Microsoft.TeamFoundation.MVVM;
using Spiral.TfsEssentials.Providers;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Threading;
using Task = System.Threading.Tasks.Task;

namespace Spiral.TfsEssentials.ViewModels
{
	internal class BranchDropDownViewModel : ViewModelBase
	{
		private readonly TeamExplorerPageViewModelBase teamExplorerPageViewModelBase;
		private readonly TfsBranchProvider tfsBranchProvider;
		private string currentBranch;
		private List<string> branches;

		public BranchDropDownViewModel(TeamExplorerPageViewModelBase teamExplorerPageViewModelBase, TfsBranchProvider tfsBranchProvider)
			: base(teamExplorerPageViewModelBase)
		{
			this.teamExplorerPageViewModelBase = teamExplorerPageViewModelBase;
			this.tfsBranchProvider = tfsBranchProvider;

			SelectBranchCommand = new RelayCommand(SelectBranch);

			Refresh();
		}

		private void SelectBranch(object obj)
		{
			var branch = obj as string;

			if (String.IsNullOrWhiteSpace(branch))
			{
				return;
			}

			this.CurrentBranch = branch;
		}

		public ICommand SelectBranchCommand { get; private set; }

		[ValueDependsOnProperty("CurrentBranch")]
		public string CurrentBranchName
		{
			get
			{
				return String.IsNullOrWhiteSpace(CurrentBranch) ? "No Branches" : CurrentBranch;
			}
		}

		public string CurrentBranch
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

		public List<string> Branches
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
				// Now you’re on a separate thread.
				var branchNames = tfsBranchProvider.GetBranchNames();

				await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

				Branches = branchNames;
				this.CurrentBranch = this.Branches.FirstOrDefault();
				this.teamExplorerPageViewModelBase.IsBusy = false;
			});
		}
	}
}