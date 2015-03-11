using System;
using System.ComponentModel.Composition;
using System.Drawing;
using System.IO;
using System.Reflection;
using Microsoft.TeamFoundation.Controls;
using Microsoft.TeamFoundation.Controls.WPF.TeamExplorer;
using Microsoft.VisualStudio.Shell;
using Spiral.TfsEssentials.Extentions;

namespace Spiral.TfsEssentials.Components
{
	[TeamExplorerNavigationItem(LinkId, 1000)]
	internal class MergeNavigationItem : TeamExplorerNavigationItemBase
	{
		public const string LinkId = "5E7F3922-32ED-4621-ACEB-D7C8D80CA3EE";

		private readonly IServiceProvider serviceProvider;

		private readonly UIContext tfsProviderContext;

		[ImportingConstructor]
		public MergeNavigationItem(
			[Import(typeof(SVsServiceProvider))] IServiceProvider serviceProvider)
		{
			this.serviceProvider = serviceProvider;
			this.Text = "Merge";

			tfsProviderContext = UIContext.FromUIContextGuid(GuidList.TfsProviderGuid);

			if (tfsProviderContext != null)
			{
				tfsProviderContext.UIContextChanged += tfsProviderContext_UIContextChanged;
			}

			SetVisibility();
			this.Image = GetImage();
		}

		private void SetVisibility()
		{
			this.IsVisible = tfsProviderContext != null && tfsProviderContext.IsActive;
		}

		void tfsProviderContext_UIContextChanged(object sender, UIContextChangedEventArgs e)
		{
			SetVisibility();
		}

		private Image GetImage()
		{
			Image mergeIcon = null;

			var assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

			if (String.IsNullOrWhiteSpace(assemblyPath))
			{
				return null;
			}

			string path = Path.Combine(assemblyPath, "Resources\\Branches.ico");

			if (File.Exists(path))
			{
				mergeIcon = Image.FromFile(path);
			}

			return mergeIcon;
		}

		public override void Execute()
		{
			var teamExplorer = serviceProvider.GetService<ITeamExplorer>();
			teamExplorer.NavigateToPage(new Guid(MergePage.PageId), null);
		}
	}
}
