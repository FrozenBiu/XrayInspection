using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace GUI_XRS
{
	// Token: 0x0200019F RID: 415
	[ValueConversion(typeof(bool), typeof(Visibility))]
	public class BoolToVisibilityCollapsedConverter : IValueConverter
	{
		// Token: 0x06002714 RID: 10004 RVA: 0x00109248 File Offset: 0x00107448
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
				result = ((!flag2) ? Visibility.Collapsed : Visibility.Visible);
			}
			return result;
		}

		// Token: 0x06002715 RID: 10005 RVA: 0x001091FB File Offset: 0x001073FB
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
