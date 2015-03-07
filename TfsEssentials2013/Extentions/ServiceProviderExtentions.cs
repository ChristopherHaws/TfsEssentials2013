using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spiral.TfsEssentials.Extentions
{
	internal static class ServiceProviderExtentions
	{
		public static T GetService<T>(this IServiceProvider serviceProvider)
		{
			if (serviceProvider == null)
			{
				Debug.WriteLine("GetService<T> called before service provider is set");
				return default(T);
			}

			return (T)serviceProvider.GetService(typeof(T));
		}
	}
}
