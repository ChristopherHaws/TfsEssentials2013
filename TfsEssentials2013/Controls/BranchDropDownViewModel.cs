using System;
using System.Collections.Generic;
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
		}

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
				SetAndRaisePropertyChanged<string>(ref currentBranch, value, "CurrentBranch");
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
				SetAndRaisePropertyChanged<List<string>>(ref branches, value, "Branches");
			}
		}
	}
}