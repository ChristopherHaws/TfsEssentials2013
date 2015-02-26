using System.ComponentModel.Design;
using Microsoft.TeamFoundation.Common.Internal;
using Microsoft.TeamFoundation.Controls;
using Spiral.TfsEssentials.WPF.TeamExplorer;

namespace Spiral.TfsEssentials.Controls.Checkins
{
	[TeamExplorerPage(PageId)]
	internal class CheckinsPage : TfsTeamExplorerPageBase
	{
		public const string PageId = "6EF9B9F7-71EE-4B9A-ACCF-9447536A9765";

		public CheckinsPage()
        {
			this.Title = "Unsynced Checkins";
        }

		protected override object CreateModel(PageInitializeEventArgs e)
		{
			return (object)new CheckinsModel(e.ServiceProvider, this.TaskFactory);
		}

		protected override object CreateView(PageInitializeEventArgs e)
		{
			var view = new CheckinsPageView();
			return (object)view;
		}

		protected override ITeamExplorerPage CreateViewModel(PageInitializeEventArgs e)
		{
			var viewModel = new CheckinsPageViewModel(this.Model as CheckinsModel);
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

			service.RemoveService(typeof(CheckinsModel));
		}
	}
}
