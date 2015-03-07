using System.ComponentModel.Design;
using Microsoft.TeamFoundation.Common.Internal;
using Microsoft.TeamFoundation.Controls;
using Spiral.TfsEssentials.Models;
using Spiral.TfsEssentials.ViewModels;

namespace Spiral.TfsEssentials.Components
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
			var view = new Views.MergePageView();
			return view;
		}

		protected override ITeamExplorerPage CreateViewModel(PageInitializeEventArgs e)
		{
			var viewModel = new MergePageViewModel(this.Model as MergeModel);
			return viewModel;
		}

		protected override void InitializeModel(PageInitializeEventArgs e)
		{
			base.InitializeModel(e);

			var model = this.Model as MergeModel;
			if (model == null)
			{
				return;
			}

			var service = e.ServiceProvider.GetService<IServiceContainer>();
			if (service == null)
			{
				return;
			}

			service.AddService(typeof(MergeModel), model);
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
