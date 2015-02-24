using Microsoft.TeamFoundation.Controls.WPF.TeamExplorer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

		private void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			throw new NotImplementedException();

			//switch (e.PropertyName)
			//{
			//	case "IsGitOperationRunning":
			//	case "Branch":
			//		this.UpdateActionLinks();
			//		break;
			//	case "IsRepositoryOperationInProgress":
			//		this.UpdateActionLinks();
			//		IServiceProvider serviceProvider = this.ServiceProvider;
			//		if (serviceProvider == null)
			//			break;
			//		SccNotificationService notificationService = (SccNotificationService)SccServiceHostExtensionMethods.GetSccService<SccNotificationService>(serviceProvider);
			//		if (notificationService == null || notificationService.IsConflictNotificationVisible((ITeamExplorerPage)null))
			//			break;
			//		this.UpdateOperationInProgressNotification();
			//		break;
			//}
		}
	}
}
