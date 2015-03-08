using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Linq;
using EnvDTE;
using EnvDTE80;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.VersionControl.Client;
using Microsoft.VisualStudio.Shell;
using Spiral.TfsEssentials.Extentions;
using Spiral.TfsEssentials.Models;

namespace Spiral.TfsEssentials.Providers
{
	[Export]
	internal class TfsBranchProvider
	{
		private readonly TfsVersionControlProvider tfsVersionControlProvider;
		private readonly PackageConfigurationManager configurationManager;
		private Tuple<String, List<BranchObject>> branches;
		private DTE2 visualStudioEnvironmentProvider;

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
			[Import] PackageConfigurationManager configurationManager,
			[Import] SVsServiceProvider serviceProvider)
		{
			this.tfsVersionControlProvider = tfsVersionControlProvider;
			this.configurationManager = configurationManager;
			this.visualStudioEnvironmentProvider = serviceProvider.GetService<_DTE>() as DTE2;
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

			var currentBranch =
				GetBranchFromCurrentSolution(possibleBranches) ??
				GetBranchFromMostRecentBranch(possibleBranches, teamProject) ??
				GetRootBranchForCurrentTeamProject(possibleBranches, teamProject) ??
				GetEarliestBranchForCurrentTeamProject(possibleBranches, teamProject);

			if (currentBranch != null)
			{
				SetCurrentBranch(currentBranch, teamProject);
			}

			return currentBranch;
		}

		private BranchModel GetBranchFromMostRecentBranch(List<BranchModel> possibleBranches, TeamProject teamProject)
		{
			var currentBranchName = configurationManager.GetValue(teamProject, "MostRecentBranch");
			if (String.IsNullOrWhiteSpace(currentBranchName))
			{
				return null;
			}

			return possibleBranches.FirstOrDefault(x => String.Equals(x.Name, currentBranchName, StringComparison.InvariantCultureIgnoreCase));
		}

		private BranchModel GetRootBranchForCurrentTeamProject(List<BranchModel> possibleBranches, TeamProject teamProject)
		{
			return possibleBranches
				.OrderBy(x => x.CreatedDate)
				.FirstOrDefault(x => x.HasParent == false);
		}

		private BranchModel GetEarliestBranchForCurrentTeamProject(List<BranchModel> possibleBranches, TeamProject teamProject)
		{
			return possibleBranches
				.OrderBy(x => x.CreatedDate)
				.FirstOrDefault();
		}

		private BranchModel GetBranchFromCurrentSolution(List<BranchModel> possibleBranches)
		{
			if (visualStudioEnvironmentProvider == null)
			{
				return null;
			}

			var fileName = visualStudioEnvironmentProvider.Solution.FileName;
			if (String.IsNullOrWhiteSpace(fileName))
			{
				return null;
			}

			var server = tfsVersionControlProvider.GetCurrentServer();
			var workspace = server.TryGetWorkspace(fileName);
			if (workspace == null)
			{
				return null;
			}

			var serverItemForLocalItem = workspace.GetServerItemForLocalItem(fileName);

			return possibleBranches
				.Where(x => serverItemForLocalItem.StartsWith(x.Path))
				.OrderBy(x => x.CreatedDate)
				.FirstOrDefault();
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
