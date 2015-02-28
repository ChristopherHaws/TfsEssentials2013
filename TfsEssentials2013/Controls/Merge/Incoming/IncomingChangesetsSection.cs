﻿using Microsoft.TeamFoundation.Controls.WPF.TeamExplorer;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.TeamFoundation.Controls;

namespace Spiral.TfsEssentials.Controls.Merge.Incoming
{
	[PartMetadata("System.ComponentModel.Composition.Caching.PartIdentity", IncomingChangesetsSection.SectionId)]
	[TeamExplorerSection(IncomingChangesetsSection.SectionId, MergePage.PageId, 10)]
	internal class IncomingChangesetsSection : TeamExplorerSectionBase
	{
		public const string SectionId = "F74DABF9-19D4-45B9-A75F-E7F8B663625A";

		protected override ITeamExplorerSection CreateViewModel(SectionInitializeEventArgs e)
		{
			return new IncomingChangesetsSectionViewModel();
		}

		protected override object CreateView(SectionInitializeEventArgs e)
		{
			return new IncomingChangesetsSectionView();
		}
	}
}
