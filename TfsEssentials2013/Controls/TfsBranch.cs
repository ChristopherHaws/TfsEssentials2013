using System.Collections.Generic;

namespace Spiral.TfsEssentials.Controls
{
	internal class TfsBranch
	{
		public TfsBranch()
		{
			this.IncomingChangesets = new List<TfsChangeset>();
			this.OutgoingChangesets = new List<TfsChangeset>();
		}

		public List<TfsChangeset> IncomingChangesets { get; set; }
		public List<TfsChangeset> OutgoingChangesets { get; set; }
		public bool HasUpstreamInfo { get; set; }
	}
}