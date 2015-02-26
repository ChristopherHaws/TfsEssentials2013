using System;
using System.ComponentModel;
using Microsoft.TeamFoundation.Controls.WPF.TeamExplorer;

namespace Spiral.TfsEssentials.Controls.Checkins
{
	internal class CheckinsPageViewModel : TeamExplorerPageViewModelBase
	{
		private CheckinsModel _model;

		public CheckinsModel Model
		{
			get
			{
				return _model;
			}
			set
			{
				if (_model != null)
				{
					_model.PropertyChanged -= new PropertyChangedEventHandler(Model_PropertyChanged);
				}

				_model = value;
				if (_model == null)
				{
					return;
				}

				_model.PropertyChanged += new PropertyChangedEventHandler(Model_PropertyChanged);
			}
		}

		public BranchDropDownViewModel BranchDropDownViewModel { get; private set; }

		public CheckinsPageViewModel(CheckinsModel model)
		{
			this.Model = model;
			this.BranchDropDownViewModel = new BranchDropDownViewModel((TeamExplorerPageViewModelBase)this);
		}

		private void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			throw new NotImplementedException();
		}
	}
}
