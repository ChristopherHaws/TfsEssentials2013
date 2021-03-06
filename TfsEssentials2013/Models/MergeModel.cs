using System;
using System.Collections.Generic;
using PropertyChanged;

namespace Spiral.TfsEssentials.Models
{
	internal class MergeModel : ModelBase
	{
		public bool IsRepositoryOperationInProgress { get; set; }

		public MergeModel()
		{
			this.IncomingChangesets = new List<ChangesetModel>();
			this.OutgoingChangesets = new List<ChangesetModel>();

			this.HasUpstreamInfo = true;
		}

		[DependsOn("Branch")]
		public int IncomingOutgoingChangesetsMaxCount
		{
			get
			{
				return Math.Max(
					this.IncomingChangesets != null ? this.IncomingChangesets.Count : 0,
					this.OutgoingChangesets != null ? this.OutgoingChangesets.Count : 0
				);
			}
		}

		public BranchModel Branch { get; set; }

		public List<BranchModel> Branches { get; set; }

		public bool IsTfsOperationRunning { get; set; }

		public List<ChangesetModel> IncomingChangesets { get; set; }

		public List<ChangesetModel> OutgoingChangesets { get; set; }

		public bool HasUpstreamInfo { get; set; }
	}
}
