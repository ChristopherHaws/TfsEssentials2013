using Microsoft.TeamFoundation;
using Spiral.TfsEssentials.WPF.TeamExplorer;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.TeamFoundation.MVVM;

namespace Spiral.TfsEssentials.Controls.Merge
{
	internal class MergeModel : TfsTeamExplorerModelBase
	{
		private bool _isRepositoryOperationInProgress;
		private TfsBranch branch;

		public bool IsRepositoryOperationInProgress
		{
			get
			{
				return _isRepositoryOperationInProgress;
			}
			set
			{
				this.SetAndRaisePropertyChanged(ref _isRepositoryOperationInProgress, value, "IsRepositoryOperationInProgress");
			}
		}

		public MergeModel(IServiceProvider serviceProvider, TaskFactory taskFactory)
			: base(serviceProvider, taskFactory)
		{
			TeamFoundationTrace.Verbose(TraceKeywordSets.TeamExplorer, "Entering ChangesetModel constructor");
			this.Branch = new TfsBranch();
		}

		[ValueDependsOnProperty("Branch")]
		public int IncomingOutgoingChangesetsMaxCount
		{
			get
			{
				var val1 = 0;
				var val2 = 0;

				if (this.Branch != null)
				{
					val1 = this.Branch.IncomingChangesets != null ? this.Branch.IncomingChangesets.Count : 0;
					val2 = this.Branch.OutgoingChangesets != null ? this.Branch.OutgoingChangesets.Count : 0;
				}

				return Math.Max(val1, val2);
			}
		}

		public TfsBranch Branch
		{
			get
			{
				return this.branch;
			}
			set
			{
				this.SetAndRaisePropertyChanged(ref this.branch, value, "Branch");
			}
		}

		public bool IsTfsOperationRunning { get; set; }
	}
}
