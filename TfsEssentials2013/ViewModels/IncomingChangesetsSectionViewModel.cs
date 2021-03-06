﻿using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using Microsoft.TeamFoundation.MVVM;

namespace Spiral.TfsEssentials.ViewModels
{
	internal class IncomingChangesetsSectionViewModel : ChangesetsSectionViewModelBase
	{
		public IncomingChangesetsSectionViewModel()
		{
			this.PullCommand = new RelayCommand(p => this.Pull(), p => this.CanPull());
		}

		public RelayCommand PullCommand { get; private set; }

		private bool CanPull()
		{
			try
			{
				return this.Model != null
					&& !this.Model.IsRepositoryOperationInProgress
					&& !this.Model.IsTfsOperationRunning
					&& this.Model.HasUpstreamInfo
					&& this.Model.IncomingChangesets.Any();
			}
			catch (Exception ex)
			{
				this.ShowException(ex, true);
			}

			return false;
		}

		private void Pull()
		{
			throw new NotImplementedException();
		}

		protected override void UpdateItemsSource()
		{
			if (this.Model != null && this.Model.IncomingChangesets != null)
			{
				this.ChangesetsItemsSource.Reset(this.Model.IncomingChangesets);
			}
			else
			{
				this.ChangesetsItemsSource.Clear();
			}
		}

		protected override void UpdateTitle()
		{
			if (this.ChangesetsItemsSource.Any())
			{
				this.Title = String.Format("Incoming Changesets ({0})", this.ChangesetsItemsSource.Count);
				this.NoChangesetsMessage = String.Empty;
			}
			else
			{
				this.Title = "Incoming Commits";

				if (this.Model != null && !this.Model.HasUpstreamInfo)
				{
					this.NoChangesetsMessage = "Current branch does not have an upstream branch configured. There are no incoming changesets for non-tracking branches.";
				}
				else
				{
					this.NoChangesetsMessage = "There are no incoming changesets.";
				}
			}
		}

		private void UpdateActionLinks()
		{
			this.PullCommand.RaiseCanExecuteChanged();
		}

		protected override void SubscribeToModelEvents()
		{
			if (this.Model == null)
			{
				return;
			}

			this.Model.PropertyChanged += ModelPropertyChanged;
		}

		protected override void UnsubcribeToModelEvents()
		{
			if (this.Model == null)
			{
				return;
			}

			this.Model.PropertyChanged -= ModelPropertyChanged;
		}

		private void ModelPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			switch (e.PropertyName)
			{
				case "IsRepositoryOperationInProgress":
				case "IsTfsOperationRunning":
					this.UpdateActionLinks();
					break;
				case "IncomingChangesets":
				case "OutgoingChangesets":
				case "Branch":
					this.UpdateItemsSource();
					this.UpdateActionLinks();
					this.UpdateTitle();
					break;
			}
		}
	}
}