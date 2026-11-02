using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace GUI_XRS
{
	// Token: 0x0200019D RID: 413
	[ValueConversion(typeof(bool), typeof(Visibility))]
	public class BoolToVisibilityHiddenConverter : IValueConverter
	{
		// Token: 0x0600270E RID: 9998 RVA: 0x001091B8 File Offset: 0x001073B8
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			bool flag = targetType != typeof(Visibility);
			object result;
			if (flag)
			{
				result = null;
			}
			else
			{
				bool flag2 = bool.Parse(value.ToString());
				result = ((!flag2) ? Visibility.Hidden : Visibility.Visible);
			}
			return result;
		}

		// Token: 0x0600270F RID: 9999 RVA: 0x001091FB File Offset: 0x001073FB
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
