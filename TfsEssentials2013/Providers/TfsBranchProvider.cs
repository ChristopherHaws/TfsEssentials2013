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
using Microsoft.VisualStudio.Shell;
using Spiral.TfsEssentials.Extentions;

namespace Spiral.TfsEssentials.Providers
{
	[Export]
	internal class TfsBranchProvider
	{
		private readonly IServiceProvider serviceProvider;

		protected ITeamFoundationContext TfsContext
		{
			get
			{
				var tfContextManager = this.serviceProvider.GetService<ITeamFoundationContextManager>();
				if (tfContextManager == null)
				{
					return null;
				}

				return tfContextManager.CurrentContext;
			}
		}

		[ImportingConstructor]
		public TfsBranchProvider([Import(typeof(SVsServiceProvider))] IServiceProvider serviceProvider)
		{
			this.serviceProvider = serviceProvider;
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
			var context = this.TfsContext;

			if (context == null)
			{
				Debug.WriteLine("Could not find TFS context.");
				return new List<string>();
			}

			if (!context.HasTeamProject)
			{
				Debug.WriteLine("No team project found.");
				return new List<string>();
			}

			var server = context.TeamProjectCollection.GetService<VersionControlServer>();
			var teamProject = server.GetTeamProject(context.TeamProjectName);

			var branches = server.QueryRootBranchObjects(RecursionType.Full);

			return branches
				.Where(s => s.Properties.RootItem.Item.StartsWith(teamProject.ServerItem) && !s.Properties.RootItem.IsDeleted)
				.Select(s => s.Properties.RootItem.Item)
				.OrderBy(s => s)
				.ToList();
		}
	}
}
