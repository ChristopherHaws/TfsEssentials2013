using System;

namespace Spiral.TfsEssentials.Controls
{
	internal class TfsChangeset
	{
		public TfsChangeset()
		{
			this.Comments = "Changeset Comments";
			this.AuthorEmail = "test@test.com";
			this.AuthorName = "Christopher Haws";
		}

		public string Comments { get; set; }
		public string AuthorEmail { get; set; }
		public string AuthorName { get; set; }
		public DateTime AuthoredTime { get; set; }
	}
}