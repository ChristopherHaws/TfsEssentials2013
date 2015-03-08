using System;
using System.ComponentModel.Composition;
using System.Diagnostics;
using EnvDTE;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.VersionControl.Client;
using Microsoft.VisualStudio.Shell;
using Spiral.TfsEssentials.Extentions;

namespace Spiral.TfsEssentials.Providers
{
	[Export]
	internal class TfsVersionControlProvider
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
		public TfsVersionControlProvider([Import(typeof(SVsServiceProvider))] IServiceProvider serviceProvider)
		{
			this.serviceProvider = serviceProvider;
		}

		public VersionControlServer GetCurrentServer()
		{
			var context = this.TfsContext;
			if (context == null)
			{
				Debug.WriteLine("Could not find TFS context.");
				return null;
			}

			return context.TeamProjectCollection.GetService<VersionControlServer>();
		}

		public TeamProject GetCurrentTeamProject()
		{
			var context = this.TfsContext;
			if (context == null)
			{
				Debug.WriteLine("Could not find TFS context.");
				return null;
			}

			if (!context.HasTeamProject)
			{
				Debug.WriteLine("Current TFS context does not contain a team project.");
				return null;
			}

			var server = context.TeamProjectCollection.GetService<VersionControlServer>();
			return server.GetTeamProject(context.TeamProjectName);
		}
	}
}