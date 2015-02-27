using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Microsoft.TeamFoundation.Controls.WPF.TeamExplorer;
using Microsoft.TeamFoundation.MVVM;

namespace Spiral.TfsEssentials.Controls
{
	internal class BranchDropDownViewModel : ViewModelBase
	{
		private string currentBranch;
		private List<string> branches;

		public BranchDropDownViewModel(TeamExplorerPageViewModelBase teamExplorerPageViewModelBase)
			: base(teamExplorerPageViewModelBase)
		{
			//TODO: Remove this mock logic
			Branches = new List<string>
			{
				"Main",
				"Branch 1",
				"Branch 2"
			};

			this.CurrentBranch = this.Branches.First();

			SelectBranchCommand = new RelayCommand(SelectBranch);
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
	}
}