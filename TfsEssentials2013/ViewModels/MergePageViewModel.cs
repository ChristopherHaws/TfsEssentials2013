using System;
using System.ComponentModel;
using System.Windows.Input;
using Microsoft.TeamFoundation.Controls;
using Microsoft.TeamFoundation.Controls.WPF.TeamExplorer;
using Microsoft.TeamFoundation.MVVM;
using Spiral.TfsEssentials.Models;
using Spiral.TfsEssentials.Providers;

namespace Spiral.TfsEssentials.ViewModels
{
	internal class MergePageViewModel : TeamExplorerPageViewModelBase
	{
		private MergeModel model;

		public MergeModel Model
		{
			get
			{
				return model;
			}
			set
			{
				if (model != null)
				{
					model.PropertyChanged -= Model_PropertyChanged;
				}

				model = value;
				if (model == null)
				{
					return;
				}

				model.PropertyChanged += Model_PropertyChanged;
			}
		}

		public BranchDropDownViewModel BranchDropDownViewModel { get; private set; }

		public ICommand ViewPendingChangesCommand { get; private set; }

		public MergePageViewModel(MergeModel model, TfsBranchProvider tfsBranchProvider)
		{
			this.Model = model;
			this.BranchDropDownViewModel = new BranchDropDownViewModel(this, tfsBranchProvider);

			ViewPendingChangesCommand = new RelayCommand(NavigateToPendingChangesPage);
		}

		private void NavigateToPendingChangesPage()
		{
			try
			{
				TeamExplorerUtils.Instance.NavigateToPage(TeamExplorerPageIds.PendingChanges, this.ServiceProvider, null);
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
