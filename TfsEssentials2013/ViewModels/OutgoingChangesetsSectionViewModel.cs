using System;
using System.Linq;
using Microsoft.TeamFoundation.MVVM;

namespace Spiral.TfsEssentials.ViewModels
{
	internal class OutgoingChangesetsSectionViewModel : ChangesetsSectionViewModelBase
	{
		public OutgoingChangesetsSectionViewModel()
		{
			this.PushCommand = new RelayCommand(p => this.Push(), p => this.CanPush());
		}

		public RelayCommand PushCommand { get; set; }

		private bool CanPush()
		{
			try
			{
				return this.Model != null
					&& !this.Model.IsRepositoryOperationInProgress
					&& !this.Model.IsTfsOperationRunning
					&& this.Model.HasUpstreamInfo
					&& this.Model.OutgoingChangesets.Any();
			}
			catch (Exception ex)
			{
				this.ShowException(ex);
			}

			return false;
		}

		private void Push()
		{
			throw new NotImplementedException();
		}

		protected override void UpdateItemsSource()
		{
			if (this.Model != null && this.Model.OutgoingChangesets != null)
			{
				this.ChangesetsItemsSource.Reset(this.Model.OutgoingChangesets);
			}
			else
			{
				this.ChangesetsItemsSource.Clear();
			}
		}

		protected override void UpdateTitle()
		{
			if (this.ChangesetsItemsSource != null && this.ChangesetsItemsSource.Any())
			{
				this.Title = String.Format("Outgoing Changesets ({0})", this.ChangesetsItemsSource.Count);
				this.NoChangesetsMessage = String.Empty;
			}
			else
			{
				this.Title = "Outgoing Commits";
				this.NoChangesetsMessage = "There are no outgoing changesets.";
			}
		}

		protected override void SubscribeToModelEvents()
		{
			if (this.Model == null)
			{
				return;
			}

			this.Model.PropertyChanged += Model_PropertyChanged;
		}

		protected override void UnsubcribeToModelEvents()
		{
			if (this.Model == null)
			{
				return;
			}

			this.Model.PropertyChanged -= Model_PropertyChanged;
		}

		private void Model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			switch (e.PropertyName)
			{
				case "IsRepositoryOperationInProgress":
				case "IsTfsOperationRunning":
					this.UpdateActionLinks();
					break;
				case "Branch":
					this.UpdateItemsSource();
					this.UpdateActionLinks();
					this.UpdateTitle();
					break;
				case "IncomingChangesets":
				case "OutgoingChangesets":
					this.UpdateItemsSource();
					break;
			}
		}

		private void UpdateActionLinks()
		{
			this.PushCommand.RaiseCanExecuteChanged();
		}
	}
}