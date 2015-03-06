using System.ComponentModel.Composition;
using Microsoft.TeamFoundation.Controls;
using Microsoft.TeamFoundation.Controls.WPF.TeamExplorer;

namespace Spiral.TfsEssentials.Controls.Merge.Outgoing
{
	[PartMetadata("System.ComponentModel.Composition.Caching.PartIdentity", SectionId)]
	[TeamExplorerSection(SectionId, MergePage.PageId, 20)]
	internal class OutgoingChangesetsSection : TeamExplorerSectionBase
	{
		public const string SectionId = "4F820A0D-490F-4360-B857-F29F475B189A";

		protected override ITeamExplorerSection CreateViewModel(SectionInitializeEventArgs e)
		{
			return new OutgoingChangesetsSectionViewModel();
		}

		protected override object CreateView(SectionInitializeEventArgs e)
		{
			return new OutgoingChangesetsSectionView();
		}
	}
}
