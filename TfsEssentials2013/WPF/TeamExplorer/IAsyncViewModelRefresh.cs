using System.ComponentModel;
using Microsoft.TeamFoundation.Controls.WPF.TeamExplorer;

namespace Spiral.TfsEssentials.WPF.TeamExplorer
{
	internal interface IAsyncViewModelRefresh<T>
	{
		T BeginRefresh(RefreshReason reason, CancelEventArgs e);

		void RefreshCompleted(T result, RefreshReason reason, AsyncCompletedEventArgs e);
	}
}
