using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.VersionControl.Client;

namespace Spiral.TfsEssentials.Providers
{
	internal class TfsBranchProvider
	{
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

		public List<string> GetBranchNames(string teamProjectName)
		{
			throw new NotImplementedException();
		}
	}
}
