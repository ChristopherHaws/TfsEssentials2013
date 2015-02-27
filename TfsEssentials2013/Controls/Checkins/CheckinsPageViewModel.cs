using System;
using System.ComponentModel;
using System.Windows.Input;
using Microsoft.TeamFoundation.Controls.WPF.TeamExplorer;
using Microsoft.TeamFoundation.MVVM;

namespace Spiral.TfsEssentials.Controls.Checkins
{
	internal class CheckinsPageViewModel : TeamExplorerPageViewModelBase
	{
		private CheckinsModel model;

		public CheckinsModel Model
		{
			get
			{
				return model;
			}
			set
			{
				if (model != null)
				{
					model.PropertyChanged -= new PropertyChangedEventHandler(Model_PropertyChanged);
				}

				model = value;
				if (model == null)
				{
					return;
				}

				model.PropertyChanged += new PropertyChangedEventHandler(Model_PropertyChanged);
			}
		}

		public BranchDropDownViewModel BranchDropDownViewModel { get; private set; }

		public ICommand SelectBranchCommand { get; private set; }

		public ICommand ViewPendingChangesCommand { get; private set; }

		public CheckinsPageViewModel(CheckinsModel model)
		{
			this.Model = model;
			this.BranchDropDownViewModel = new BranchDropDownViewModel(this);

			ViewPendingChangesCommand = new RelayCommand(NavigateToPendingChangesPage);
		}

		private void NavigateToPendingChangesPage()
		{
			try
			{
				TeamExplorerUtils.Instance.NavigateToPage(GuidList.TfsPendingChangesPageId, this.ServiceProvider, null);
			}
			catch (Exception ex)
			{
				this.ShowException(ex);
			}
		}

		private void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			throw new NotImplementedException();
		}
	}
}
