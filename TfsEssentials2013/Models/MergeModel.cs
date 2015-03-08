using System;
using System.Threading.Tasks;
using Microsoft.TeamFoundation;
using Microsoft.TeamFoundation.MVVM;

namespace Spiral.TfsEssentials.Models
{
	internal class MergeModel : TfsTeamExplorerModelBase
	{
		private bool _isRepositoryOperationInProgress;
		private TfsBranchModel _branchModel;

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
			this.BranchModel = new TfsBranchModel();
		}

		[ValueDependsOnProperty("Branch")]
		public int IncomingOutgoingChangesetsMaxCount
		{
			get
			{
				var val1 = 0;
				var val2 = 0;

				if (this.BranchModel != null)
				{
					val1 = this.BranchModel.IncomingChangesets != null ? this.BranchModel.IncomingChangesets.Count : 0;
					val2 = this.BranchModel.OutgoingChangesets != null ? this.BranchModel.OutgoingChangesets.Count : 0;
				}

				return Math.Max(val1, val2);
			}
		}

		public TfsBranchModel BranchModel
		{
			get
			{
				return this._branchModel;
			}
			set
			{
				this.SetAndRaisePropertyChanged(ref this._branchModel, value, "Branch");
			}
		}

		public bool IsTfsOperationRunning { get; set; }
	}
}