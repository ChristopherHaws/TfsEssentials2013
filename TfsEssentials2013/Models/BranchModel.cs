using System;
using System.Diagnostics;

namespace Spiral.TfsEssentials.Models
{
	[DebuggerDisplay("{Path}")]
	internal class BranchModel
	{
		public String Name { get; set; }

		public String Path { get; set; }

		public String Description { get; set; }

		public String Owner { get; set; }

		public DateTime CreatedDate { get; set; }

		public Boolean HasParent { get; set; }

		public Boolean HasChildren { get; set; }
	}
}