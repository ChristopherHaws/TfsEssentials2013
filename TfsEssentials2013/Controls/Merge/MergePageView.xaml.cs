using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;

namespace Spiral.TfsEssentials.Controls.Merge
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
