using System;
using System.Globalization;
using System.Windows.Data;
using Microsoft.TeamFoundation.Common.Internal;

namespace Spiral.TfsEssentials.Controls
{
	public class DateTimeFormatConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (!(value is DateTimeOffset))
			{
				return null;
			}

			var flag = false;

			var s = parameter as string;
			if (s != null)
			{
				flag = string.Equals(s, "friendly", StringComparison.OrdinalIgnoreCase);
			}

			var dateTimeOffset = (DateTimeOffset)value;

			return flag ? dateTimeOffset.LocalDateTime.Friendly() : dateTimeOffset.LocalDateTime.ToString();
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return null;
		}
	}
}
