using Microsoft.TeamFoundation;
using Spiral.TfsEssentials.WPF.TeamExplorer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spiral.TfsEssentials.Controls.Merge
{
	internal class MergeModel : TfsTeamExplorerModelBase
	{
		private bool _isRepositoryOperationInProgress;

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
			TeamFoundationTrace.Verbose((string[])TraceKeywordSets.TeamExplorer, "Entering CheckinModel constructor");
		}
	}
}
