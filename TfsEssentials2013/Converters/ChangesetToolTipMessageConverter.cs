using System;
using System.Globalization;
using System.Windows.Data;
using Microsoft.TeamFoundation;
using Spiral.TfsEssentials.Models;

namespace Spiral.TfsEssentials.Converters
{
	internal class ChangesetToolTipMessageConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			try
			{
				var tfsChangeset = value as ChangesetModel;
				if (tfsChangeset == null)
				{
					return String.Empty;
				}

				var comments = tfsChangeset.Comments != null ? tfsChangeset.Comments.Trim() : String.Empty;
				if (comments.Length > 1024)
				{
					comments = comments.Substring(0, 1024) + "...";
				}

				return comments;
			}
			catch (Exception ex)
			{
				TeamFoundationTrace.TraceException(TraceKeywordSets.TeamExplorer, "ChangesetToolTipMessage.Convert", ex);
			}
			return null;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}