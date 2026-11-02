using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace GUI_XRS
{
	// Token: 0x020001C6 RID: 454
	[ValueConversion(typeof(bool), typeof(Brush))]
	public class ImageSelectionBoolToBackgroundColorConverter : IValueConverter
	{
		// Token: 0x06002789 RID: 10121 RVA: 0x0010AC2C File Offset: 0x00108E2C
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			bool flag = targetType != typeof(Brush);
			object result;
			if (flag)
			{
				result = null;
			}
			else
			{
				bool flag2 = bool.Parse(value.ToString());
				SolidColorBrush solidColorBrush = new SolidColorBrush(Color.FromRgb(201, 219, 83));
				result = (flag2 ? solidColorBrush : Brushes.Transparent);
			}
			return result;
		}

		// Token: 0x0600278A RID: 10122 RVA: 0x001091FB File Offset: 0x001073FB
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
