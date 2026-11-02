using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace GUI_XRS
{
	// Token: 0x020001A2 RID: 418
	[ValueConversion(typeof(int), typeof(Visibility))]
	public class IntToVisibilityConverter1 : IValueConverter
	{
		// Token: 0x0600271D RID: 10013 RVA: 0x001093BC File Offset: 0x001075BC
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
				int num = int.Parse(value.ToString());
				result = ((num <= 1) ? Visibility.Hidden : Visibility.Visible);
			}
			return result;
		}

		// Token: 0x0600271E RID: 10014 RVA: 0x001091FB File Offset: 0x001073FB
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
