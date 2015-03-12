using System.Collections.ObjectModel;
using Microsoft.TeamFoundation.Common.Internal;
using Microsoft.TeamFoundation.Controls;
using Microsoft.TeamFoundation.Controls.WPF.TeamExplorer;
using Microsoft.TeamFoundation.MVVM;
using PropertyChanged;
using Spiral.TfsEssentials.Models;

namespace Spiral.TfsEssentials.ViewModels
{
	internal abstract class ChangesetsSectionViewModelBase : TeamExplorerSectionViewModelBase
	{
		private MergeModel model;

		protected ChangesetsSectionViewModelBase()
		{
			this.ChangesetsItemsSource = new BatchedObservableCollection<ChangesetModel>();
			this.SelectedItems = new ObservableCollection<ChangesetModel>();

			this.UpdateTitle();
			this.UpdateItemsSource();
		}

		/// <summary>
		/// Initialize this section.
		/// </summary>
		public override void Initialize(object sender, SectionInitializeEventArgs e)
		{
			base.Initialize(sender, e);
			this.Model = e.ServiceProvider.GetService<MergeModel>();
		}

		[DoNotNotify]
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

		public ObservableCollection<ChangesetModel> SelectedItems { get; private set; }

		public string NoChangesetsMessage { get; set; }

		public BatchedObservableCollection<ChangesetModel> ChangesetsItemsSource { get; set; }

		protected abstract void UpdateItemsSource();

		protected abstract void UpdateTitle();

		protected abstract void SubscribeToModelEvents();

		protected abstract void UnsubcribeToModelEvents();
	}
}
