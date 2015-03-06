using System.Collections.Generic;

namespace Spiral.TfsEssentials.Controls
{
	internal class TfsBranch
	{
		public TfsBranch()
		{
			this.IncomingChangesets = new List<TfsChangeset>()
			{
				new TfsChangeset(),
				new TfsChangeset(),
				new TfsChangeset()
			};

			this.OutgoingChangesets = new List<TfsChangeset>()
			{
				new TfsChangeset(),
				new TfsChangeset(),
				new TfsChangeset()
			};

			this.HasUpstreamInfo = true;
		}

		public List<TfsChangeset> IncomingChangesets { get; set; }
		public List<TfsChangeset> OutgoingChangesets { get; set; }
		public bool HasUpstreamInfo { get; set; }
	}
}