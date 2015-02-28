using System.Collections.ObjectModel;
using Microsoft.TeamFoundation.Controls.WPF.TeamExplorer;
using Microsoft.TeamFoundation.MVVM;

namespace Spiral.TfsEssentials.Controls.Merge
{
	internal abstract class ChangesetsSectionViewModelBase : TeamExplorerSectionViewModelBase
	{
		private ObservableCollection<TfsBranchViewModel> selectedItems;
		private MergeModel model;
		private BatchedObservableCollection<TfsChangeset> changesetsItemsSource;
		private string noChangesetsMessage;

		protected ChangesetsSectionViewModelBase()
		{
			ChangesetsItemsSource = new BatchedObservableCollection<TfsChangeset>();
			this.UpdateTitle();
			this.UpdateItemsSource();
		}

		public virtual MergeModel Model
		{
			get
			{
				return this.model;
			}
			set
			{
				this.UnsubcribeToModelEvents();
				this.model = value;
				this.UpdateItemsSource();
				this.UpdateTitle();
				this.SubscribeToModelEvents();
			}
		}

		public ObservableCollection<TfsBranchViewModel> SelectedItems
		{
			get
			{
				return this.selectedItems;
			}
			private set
			{
				this.selectedItems = value;
				this.RaisePropertyChanged("SelectedItems");
			}
		}

		public string NoChangesetsMessage
		{
			get
			{
				return this.noChangesetsMessage;
			}
			set
			{
				this.SetAndRaisePropertyChanged(ref this.noChangesetsMessage, value, "NoChangesetsMessage");
			}
		}

		public BatchedObservableCollection<TfsChangeset> ChangesetsItemsSource
		{
			get
			{
				return this.changesetsItemsSource;
			}
			set
			{
				this.SetAndRaisePropertyChanged(ref this.changesetsItemsSource, value, "ChangesetsItemsSource");
			}
		}

		protected abstract void UpdateItemsSource();

		protected abstract void UpdateTitle();

		protected abstract void SubscribeToModelEvents();

		protected abstract void UnsubcribeToModelEvents();
	}
}
