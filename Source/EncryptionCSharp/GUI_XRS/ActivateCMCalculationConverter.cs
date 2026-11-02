using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x0200020D RID: 525
	[ValueConversion(typeof(bool), typeof(BitmapImage))]
	public class ActivateCMCalculationConverter : IValueConverter
	{
		// Token: 0x0600285E RID: 10334 RVA: 0x0010DABC File Offset: 0x0010BCBC
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
				bool flag2 = value is bool;
				if (flag2)
				{
					imageSource = (((bool)value) ? ImageConverter.ToBitmap("/img/play.png") : ImageConverter.ToBitmap("/img/play_bright.png"));
				}
				result = imageSource;
			}
			return result;
		}

		// Token: 0x0600285F RID: 10335 RVA: 0x001091FB File Offset: 0x001073FB
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
