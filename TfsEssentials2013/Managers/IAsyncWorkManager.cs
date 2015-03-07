using System;
using System.Threading.Tasks;

namespace Spiral.TfsEssentials.Managers
{
	internal interface IAsyncWorkManager
	{
		Task QueueWorkAsync(Action action);
	}
}
