using System;
using System.Globalization;
using System.Windows.Data;
using Microsoft.TeamFoundation;
using Spiral.TfsEssentials.Models;

namespace Spiral.TfsEssentials.Converters
{
	internal class ChangesetToolTipAuthorConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			try
			{
				var changeset = value as ChangesetModel;
				if (changeset == null)
				{
					return String.Empty;
				}

				return string.IsNullOrWhiteSpace(changeset.AuthorEmail)
					? String.Format("{0}", changeset.AuthorName)
					: String.Format("{0} <{1}>", changeset.AuthorName, changeset.AuthorEmail);
			}
			catch (Exception ex)
			{
				TeamFoundationTrace.TraceException(TraceKeywordSets.TeamExplorer, "ChangesetToolTipAuthor.Converter", ex);
			}
			return null;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}