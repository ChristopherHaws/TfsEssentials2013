using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;

namespace Spiral.TfsEssentials.Controls.Checkins
{
	/// <summary>
	/// Interaction logic for CheckinsView.xaml
	/// </summary>
	internal partial class CheckinsPageView : UserControl, IComponentConnector
	{
		public ICommand ShowBranchesDropDownCommand { get; private set; }

		public CheckinsPageView()
		{
			InitializeComponent();
		}

		public CheckinsPageViewModel ViewModel
		{
			get
			{
				return this.DataContext as CheckinsPageViewModel;
			}
		}
	}
}
