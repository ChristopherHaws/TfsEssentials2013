using Microsoft.TeamFoundation.Controls;
using Microsoft.VisualStudio.Shell;
using Spiral.TfsEssentials.WPF.TeamExplorer;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Spiral.TfsEssentials.Controls.Merge
{
	[TeamExplorerNavigationItem(MergeNavigationItem.LinkId, 1000)]
	public class MergeNavigationItem : TeamExplorerBaseNavigationItem
	{
		public const string LinkId = "5E7F3922-32ED-4621-ACEB-D7C8D80CA3EE";

		private readonly UIContext tfsProviderContext;

		[ImportingConstructor]
		public MergeNavigationItem([Import(typeof(SVsServiceProvider))] IServiceProvider serviceProvider)
			: base(serviceProvider)
		{
			this.Text = "Merge";

			tfsProviderContext = UIContext.FromUIContextGuid(TeamExplorerBase.TfsProviderGuid);

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

			if(String.IsNullOrWhiteSpace(assemblyPath))
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
			var teamExplorer = GetService<ITeamExplorer>();
			teamExplorer.NavigateToPage(new Guid(MergePage.PageId), null);
		}
	}
}
