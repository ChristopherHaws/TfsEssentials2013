﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Microsoft.TeamFoundation;

namespace Spiral.TfsEssentials.Controls.Merge
{
	internal class ChangesetsSectionListViewMaxHeightConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			try
			{
				if (!(value is int))
				{
					return null;
				}

				return (int)value > 20 ? 300.0 : double.PositiveInfinity;
			}
			catch (Exception ex)
			{
				TeamFoundationTrace.TraceException(TraceKeywordSets.TeamExplorer, "ChangesetsSectionListViewMaxHeight.Convert", ex);
			}
			return null;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return null;
		}
	}
}
