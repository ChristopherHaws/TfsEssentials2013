using Microsoft.TeamFoundation.Controls.WPF.TeamExplorer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Spiral.TfsEssentials.Controls.Checkins
{
	/// <summary>
	/// Interaction logic for CheckinsView.xaml
	/// </summary>
	internal partial class CheckinsPageView : UserControl
	{
		public ICommand ShowBranchesDropDownCommand { get; private set; }

		public CheckinsPageView()
		{
			InitializeComponent();
			this.ShowBranchesDropDownCommand = (ICommand)new DropDownLinkCommand((Action<object>)(p => this.ShowBranchesDropDown()), (Predicate<object>)(p => this.CanShowBranchesDropDown()));
		}

		public CheckinsPageViewModel ViewModel
		{
			get
			{
				return this.DataContext as CheckinsPageViewModel;
			}
		}

		private bool CanShowBranchesDropDown()
		{
			try
			{
				return this.ViewModel != null && this.ViewModel.Model != null && !this.ViewModel.Model.IsRepositoryOperationInProgress;
			}
			catch (Exception ex)
			{
				if (this.ViewModel != null)
				{
					this.ViewModel.ShowException(ex, true);
				}
			}
			return false;
		}

		private void ShowBranchesDropDown()
		{
			//try
			//{
			//	if (ViewHelper == null)
			//	{
			//		return;
			//	}

			//	this.ViewHelper.ShowBranchesContextMenu(currentBranchLink.PointToScreen(new Point(0.0, VisualTreeHelper.GetContentBounds((Visual)this.currentBranchLink).Bottom)));
			//}
			//catch (Exception ex)
			//{
			//	if (ViewModel == null)
			//	{
			//		return;
			//	}

			//	this.ViewModel.ShowException(ex, true);
			//}
		}

		public void SetViewModel(object viewModel)
		{
			this.DataContext = viewModel;
		}
	}
}
