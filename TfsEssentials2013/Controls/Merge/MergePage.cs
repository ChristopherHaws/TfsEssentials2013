using System.ComponentModel.Design;
using Microsoft.TeamFoundation.Common.Internal;
using Microsoft.TeamFoundation.Controls;
using Spiral.TfsEssentials.WPF.TeamExplorer;

namespace Spiral.TfsEssentials.Controls.Merge
{
	[TeamExplorerPage(PageId)]
	internal class MergePage : TfsTeamExplorerPageBase
	{
		public const string PageId = "6EF9B9F7-71EE-4B9A-ACCF-9447536A9765";

		public MergePage()
        {
			this.Title = "Unsynced Merge";
        }

		protected override object CreateModel(PageInitializeEventArgs e)
		{
			return new MergeModel(e.ServiceProvider, this.TaskFactory);
		}

		protected override object CreateView(PageInitializeEventArgs e)
		{
			var view = new MergePageView();
			return view;
		}

		protected override ITeamExplorerPage CreateViewModel(PageInitializeEventArgs e)
		{
			var viewModel = new MergePageViewModel(this.Model as MergeModel);
			return viewModel;
		}

		public override void Dispose()
		{
			base.Dispose();

			var service = this.ServiceProvider.GetService<IServiceContainer>();

			if (service == null)
			{
				return;
			}

			service.RemoveService(typeof(MergeModel));
		}
	}
}
