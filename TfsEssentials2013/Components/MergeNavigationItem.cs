using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.TeamFoundation.Controls;
using Microsoft.TeamFoundation.Controls.WPF.TeamExplorer;
using Microsoft.VisualStudio.Shell;

namespace Spiral.TfsEssentials.Components
{
	[TeamExplorerNavigationItem(MergeNavigationItem.LinkId, 1000)]
	public class MergeNavigationItem : TeamExplorerNavigationItemBase
	{
		public const string LinkId = "5E7F3922-32ED-4621-ACEB-D7C8D80CA3EE";

		private readonly IServiceProvider serviceProvider;

		private readonly UIContext tfsProviderContext;

		[ImportingConstructor]
		public MergeNavigationItem([Import(typeof(SVsServiceProvider))] IServiceProvider serviceProvider)
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
			var teamExplorer = GetService<ITeamExplorer>();
			teamExplorer.NavigateToPage(new Guid(MergePage.PageId), null);
		}

		/// <summary>
		/// Get the requested service from the service provider.
		/// </summary>
		public T GetService<T>()
		{
			Debug.Assert(this.serviceProvider != null, "GetService<T> called before service provider is set");
			if (this.serviceProvider != null)
			{
				return (T)this.serviceProvider.GetService(typeof(T));
			}

			return default(T);
		}
	}
}
