using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.VersionControl.Client;
using Spiral.TfsEssentials.Models;

namespace Spiral.TfsEssentials.Providers
{
	[Export]
	internal class TfsBranchProvider
	{
		private readonly TfsVersionControlProvider tfsVersionControlProvider;
		private readonly PackageConfigurationManager configurationManager;
		private Tuple<String, List<BranchObject>> branches;

		private IEnumerable<BranchObject> Branches
		{
			get
			{
				var server = tfsVersionControlProvider.GetCurrentServer();
				if (server == null)
				{
					Debug.WriteLine("Could not find current TFS server.");
					return new List<BranchObject>();
				}

				var teamProject = tfsVersionControlProvider.GetCurrentTeamProject();
				if (teamProject == null)
				{
					Debug.WriteLine("Could not find current team project.");
					return new List<BranchObject>();
				}

				if (branches != null && teamProject.ServerItem == branches.Item1)
				{
					return branches.Item2;
				}

				var teamProjectBranches = server.QueryRootBranchObjects(RecursionType.Full)
					.Where(s => s.Properties.RootItem.Item.StartsWith(teamProject.ServerItem))
					.OrderBy(s => s.Properties.RootItem.Item)
					.ToList();

				branches = new Tuple<String, List<BranchObject>>(teamProject.ServerItem, teamProjectBranches);

				return branches.Item2;
			}
		}

		[ImportingConstructor]
		public TfsBranchProvider(
			[Import] TfsVersionControlProvider tfsVersionControlProvider,
			[Import] PackageConfigurationManager configurationManager)
		{
			this.tfsVersionControlProvider = tfsVersionControlProvider;
			this.configurationManager = configurationManager;
		}

		public string GetBranchName(string localPath)
		{
			var localWorkspace = Workstation.Current.GetLocalWorkspaceInfo(localPath);
			if (localWorkspace == null)
			{
				Debug.WriteLine("Unable to find local workspace for path '{0}'.", localPath);
				return null;
			}

			var server = TfsTeamProjectCollectionFactory.GetTeamProjectCollection(localWorkspace.ServerUri);
			var service = server.GetService<VersionControlServer>();
			var workspace = service.TryGetWorkspace(localPath);
			if (workspace == null)
			{
				Debug.WriteLine("Unable to find workspace for path '{0}'.", localPath);
				return null;
			}

			var serverItemForLocalItem = workspace.GetServerItemForLocalItem(localPath);

			var branch = (
				from branchObject in service.QueryRootBranchObjects(RecursionType.Full)
				where serverItemForLocalItem.StartsWith(branchObject.Properties.RootItem.Item)
				select branchObject
			).FirstOrDefault();

			if (branch == null)
			{
				Debug.WriteLine("Unable to find branch for path '{0}'.", localPath);
				return null;
			}

			return Path.GetFileName(branch.Properties.RootItem.Item);
		}

		public List<string> GetBranchNames()
		{
			return this.Branches
				.Where(x => !x.Properties.RootItem.IsDeleted)
				.Select(x => x.Properties.RootItem.Item)
				.ToList();
		}

		public List<BranchModel> GetBranches()
		{
			return this.Branches
				.Where(x => !x.Properties.RootItem.IsDeleted)
				.Select(x => new BranchModel()
				{
					Name = Path.GetFileName(x.Properties.RootItem.Item),
					Path = x.Properties.RootItem.Item,
					Description = x.Properties.Description,
					Owner = x.Properties.OwnerDisplayName,
					CreatedDate = x.DateCreated,
					HasParent = x.Properties.ParentBranch != null,
					HasChildren = x.ChildBranches.Any()
				})
				.ToList();
		}

		public BranchModel GetCurrentBranch()
		{
			var possibleBranches = GetBranches();

			var teamProject = tfsVersionControlProvider.GetCurrentTeamProject();
			if (teamProject == null)
			{
				Debug.WriteLine("Could not find current team project.");
				return null;
			}

			var currentBranchName = configurationManager.GetValue(teamProject, "MostRecentBranch");

			if (!String.IsNullOrWhiteSpace(currentBranchName))
			{
				var possibleBranch = possibleBranches.FirstOrDefault(x => String.Equals(x.Name, currentBranchName, StringComparison.InvariantCultureIgnoreCase));
				if (possibleBranch != null)
				{
					return possibleBranch;
				}
			}

			var branch = possibleBranches
				.Where(x => x.HasParent = false)
				.OrderBy(x => x.CreatedDate)
				.FirstOrDefault();

			if (branch != null)
			{
				SetCurrentBranch(branch, teamProject);
				return branch;
			}

			branch = possibleBranches
				.OrderBy(x => x.CreatedDate)
				.FirstOrDefault();

			if (branch != null)
			{
				SetCurrentBranch(branch, teamProject);
			}

			return branch;
		}

		public void Refresh()
		{
			branches = null;
		}

		public void SetCurrentBranch(BranchModel currentBranch)
		{
			var teamProject = tfsVersionControlProvider.GetCurrentTeamProject();
			if (teamProject == null)
			{
				Debug.WriteLine("Could not find current team project.");
				return;
			}

			SetCurrentBranch(currentBranch, teamProject);
		}

		public void SetCurrentBranch(BranchModel currentBranch, TeamProject teamProject)
		{
			configurationManager.SetValue(teamProject, "MostRecentBranch", currentBranch.Name);
		}
	}
}
