using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.TeamFoundation;

namespace Spiral.TfsEssentials.Converters
{
	[ValueConversion(typeof(byte[]), typeof(ImageSource))]
	internal class ImageConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			try
			{
				var bitmapImage = new BitmapImage();
				bitmapImage.BeginInit();

				if (value == null || value is byte[] && ((byte[])value).Length == 0)
				{
					bitmapImage.UriSource = new Uri("pack://application:,,,/Spiral.TfsEssentials2013;component/Resources/IdentityImage.png");
				}
				else
				{
					var buffer = (byte[])value;
					bitmapImage.StreamSource = new MemoryStream(buffer);
				}

				bitmapImage.EndInit();

				return bitmapImage;
			}
			catch (Exception ex)
			{
				TeamFoundationTrace.TraceException(TraceKeywordSets.TeamExplorer, "ImageConverter.Convert", ex);
			}

			return null;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return null;
		}
	}
}
