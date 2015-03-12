using System;

namespace Spiral.TfsEssentials.Models
{
	internal class ChangesetModel
	{
		public string Id { get; set; }

		public string Comments { get; set; }

		public string AuthorEmail { get; set; }

		public string AuthorName { get; set; }

		public byte[] AuthorAvatar { get; set; }

		public DateTimeOffset AuthoredTime { get; set; }
	}
}