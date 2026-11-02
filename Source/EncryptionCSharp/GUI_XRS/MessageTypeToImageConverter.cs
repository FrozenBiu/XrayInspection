using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Definitions;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x020001B2 RID: 434
	[ValueConversion(typeof(MessageType), typeof(BitmapImage))]
	public class MessageTypeToImageConverter : IValueConverter
	{
		// Token: 0x0600274D RID: 10061 RVA: 0x00109F38 File Offset: 0x00108138
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			ImageSource imageSource = null;
			bool flag = targetType != typeof(ImageSource);
			object result;
			if (flag)
			{
				result = null;
			}
			else
			{
				bool flag2 = value is MessageType;
				if (flag2)
				{
					switch ((MessageType)value)
					{
					case MessageType.Error:
						imageSource = ImageConverter.ToBitmap("/img/error.png");
						break;
					case MessageType.Warning:
						imageSource = ImageConverter.ToBitmap("/img/warningIcon.png");
						break;
					case MessageType.Info:
						imageSource = ImageConverter.ToBitmap("/img/info.png");
						break;
					}
				}
				result = imageSource;
			}
			return result;
		}

		// Token: 0x0600274E RID: 10062 RVA: 0x001091FB File Offset: 0x001073FB
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
