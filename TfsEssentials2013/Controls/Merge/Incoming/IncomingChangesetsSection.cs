using System.ComponentModel.Composition;
using Microsoft.TeamFoundation.Controls;
using Microsoft.TeamFoundation.Controls.WPF.TeamExplorer;

namespace Spiral.TfsEssentials.Controls.Merge.Incoming
{
	[PartMetadata("System.ComponentModel.Composition.Caching.PartIdentity", SectionId)]
	[TeamExplorerSection(SectionId, MergePage.PageId, 10)]
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
