using Microsoft.TeamFoundation.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//TODO: Remove this in place of TeamExplorerNavigationLinkBase
namespace Spiral.TfsEssentials.WPF.TeamExplorer
{
	/// <summary>
	/// Team Explorer base navigation link class.
	/// </summary>
	public class TeamExplorerBaseNavigationLink : TeamExplorerBase, ITeamExplorerNavigationLink
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		public TeamExplorerBaseNavigationLink(IServiceProvider serviceProvider)
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
		/// Gets or sets the IsEnabled flag.
		/// </summary>
		public bool IsEnabled
		{
			get { return _isEnabled; }
			set { _isEnabled = value; RaisePropertyChanged("IsEnabled"); }
		}
		private bool _isEnabled = true;

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
		/// Invalidate the link state.
		/// </summary>
		public virtual void Invalidate()
		{
		}

		/// <summary>
		/// Execute the link action.
		/// </summary>
		public virtual void Execute()
		{
		}
	}
}
