using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Microsoft.TeamFoundation.VersionControl.Client;
using Spiral.TfsEssentials.Models;

namespace Spiral.TfsEssentials.Providers
{
	[Export]
	internal class ChangesetProvider
	{
		private readonly TfsVersionControlProvider versionControlProvider;

		[ImportingConstructor]
		public ChangesetProvider([Import] TfsVersionControlProvider tfsVersionControlProvider)
		{
			this.versionControlProvider = tfsVersionControlProvider;
		}

		public List<ChangesetModel> GetIncomingChangesets(BranchModel branch)
		{
			return GetChangesets(branch.ParentPath, branch.Path);
		}

		public List<ChangesetModel> GetOutgoingChangesets(BranchModel branch)
		{
			return GetChangesets(branch.Path, branch.ParentPath);
		}

		private List<ChangesetModel> GetChangesets(string sourceBranch, string destinationBranch)
		{
			var server = versionControlProvider.GetCurrentServer();
			if (server == null || String.IsNullOrWhiteSpace(sourceBranch) || String.IsNullOrWhiteSpace(destinationBranch))
			{
				return new List<ChangesetModel>();
			}

			var mergeCandidates = server.GetMergeCandidates(sourceBranch, destinationBranch, RecursionType.Full);

			return mergeCandidates
				.Select(x => new ChangesetModel
				{
					Id = String.Format("C{0}", x.Changeset.ChangesetId),
					AuthoredTime = x.Changeset.CreationDate,
					AuthorEmail = "Test@test.com",
					AuthorName = x.Changeset.OwnerDisplayName,
					Comments = x.Changeset.Comment
				})
				.ToList();
		}
	}
}
