using System.Collections.Generic;

namespace Spiral.TfsEssentials.Models
{
	internal class TfsBranchModel
	{
		public TfsBranchModel()
		{
			this.IncomingChangesets = new List<TfsChangesetModel>()
			{
				new TfsChangesetModel(),
				new TfsChangesetModel(),
				new TfsChangesetModel()
			};

			this.OutgoingChangesets = new List<TfsChangesetModel>()
			{
				new TfsChangesetModel(),
				new TfsChangesetModel(),
				new TfsChangesetModel()
			};

			this.HasUpstreamInfo = true;
		}

		public List<TfsChangesetModel> IncomingChangesets { get; set; }
		public List<TfsChangesetModel> OutgoingChangesets { get; set; }
		public bool HasUpstreamInfo { get; set; }
	}
}