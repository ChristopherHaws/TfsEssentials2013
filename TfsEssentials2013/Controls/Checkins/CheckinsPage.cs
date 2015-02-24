using Microsoft.TeamFoundation.Controls;
using Microsoft.TeamFoundation.Controls.WPF.TeamExplorer;
using Microsoft.TeamFoundation.VersionControl.Client;
using Spiral.TfsEssentials.WPF.TeamExplorer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spiral.TfsEssentials.Controls.Checkins
{
	[TeamExplorerPageAttribute(CheckinsPage.PageId)]
	internal class CheckinsPage : TfsTeamExplorerPageBase
	{
		public const string PageId = "6EF9B9F7-71EE-4B9A-ACCF-9447536A9765";

		public CheckinsPage()
        {
			this.Title = "Unsynced Checkins";
			this.PageContent = new CheckinsPageView();
            View.SetViewModel(this);
        }

		protected CheckinsPageView View
		{
			get
			{
				return (CheckinsPageView)PageContent;
			}
		}

		protected override object CreateModel(PageInitializeEventArgs e)
		{
			return (object)new CheckinsModel(e.ServiceProvider, this.TaskFactory);
		}

		List<BranchObject> _branches = new List<BranchObject>();
		public List<BranchObject> Branches
		{
			get
			{
				return _branches;
			}
		}

		public bool CanChooseBranch
		{
			get { return Branches.Any(); }
		}
	}
}
