using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.TeamFoundation;
using PropertyChanged;

namespace Spiral.TfsEssentials.Models
{
	internal class MergeModel : ModelBase
	{
		public bool IsRepositoryOperationInProgress { get; set; }

		public MergeModel()
		{
			TeamFoundationTrace.Verbose(TraceKeywordSets.TeamExplorer, "Entering ChangesetModel constructor");

			this.IncomingChangesets = new List<ChangesetModel>()
			{
				new ChangesetModel(),
				new ChangesetModel(),
				new ChangesetModel()
			};

			this.OutgoingChangesets = new List<ChangesetModel>()
			{
				new ChangesetModel(),
				new ChangesetModel(),
				new ChangesetModel()
			};

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

		public bool IsTfsOperationRunning { get; set; }

		public List<ChangesetModel> IncomingChangesets { get; set; }

		public List<ChangesetModel> OutgoingChangesets { get; set; }

		public void OnOutgoingChangesetsChanged()
		{
			Debug.WriteLine("OutgoingChangesets Changed");
		}

		public bool HasUpstreamInfo { get; set; }
	}
}
