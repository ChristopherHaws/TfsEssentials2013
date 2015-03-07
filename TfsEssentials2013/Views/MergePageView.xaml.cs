using System.Windows.Controls;
using System.Windows.Markup;
using Spiral.TfsEssentials.ViewModels;

namespace Spiral.TfsEssentials.Views
{
	/// <summary>
	/// Interaction logic for MergeView.xaml
	/// </summary>
	internal partial class MergePageView : UserControl, IComponentConnector
	{
		public MergePageView()
		{
			InitializeComponent();
		}

		public MergePageViewModel ViewModel
		{
			get
			{
				return this.DataContext as MergePageViewModel;
			}
		}
	}
}
