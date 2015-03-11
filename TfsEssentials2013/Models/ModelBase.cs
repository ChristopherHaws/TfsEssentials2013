using System.ComponentModel;

namespace Spiral.TfsEssentials.Models
{
	public class ModelBase : INotifyPropertyChanged
	{
		public bool IsBusy { get; set; }

		public event PropertyChangedEventHandler PropertyChanged;
	}
}