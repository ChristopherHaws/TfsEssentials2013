using Microsoft.TeamFoundation.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//TODO: Remove this in place of TeamExplorerNavigationItemBase
namespace Spiral.TfsEssentials.WPF.TeamExplorer
{
	/// <summary>
	/// Team Explorer base navigation item class.
	/// </summary>
	public class TeamExplorerBaseNavigationItem : TeamExplorerBase, ITeamExplorerNavigationItem
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		public TeamExplorerBaseNavigationItem(IServiceProvider serviceProvider)
		{
			this.ServiceProvider = serviceProvider;
		}

		/// <summary>
		/// Gets or sets the item text.
		/// </summary>
		public string Text
		{
			get { return _text; }
			set { _text = value; RaisePropertyChanged("Text"); }
		}
		private string _text;

		/// <summary>
		/// Gets or sets the item image.
		/// </summary>
		public System.Drawing.Image Image
		{
			get { return _image; }
			set { _image = value; RaisePropertyChanged("Image"); }
		}
		private System.Drawing.Image _image;

		/// <summary>
		/// Gets or sets the IsVisible flag.
		/// </summary>
		public bool IsVisible
		{
			get { return _isVisible; }
			set { _isVisible = value; RaisePropertyChanged("IsVisible"); }
		}
		private bool _isVisible = true;

		/// <summary>
		/// Invalidate the item state.
		/// </summary>
		public virtual void Invalidate()
		{
		}

		/// <summary>
		/// Execute the item action.
		/// </summary>
		public virtual void Execute()
		{
		}
	}
}
