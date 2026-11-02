using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace GUI_XRS
{
	// Token: 0x020001A1 RID: 417
	[ValueConversion(typeof(int), typeof(Visibility))]
	public class IntToVisibilityConverter : IValueConverter
	{
		// Token: 0x0600271A RID: 10010 RVA: 0x00109368 File Offset: 0x00107568
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			bool flag = targetType != typeof(Visibility) || value == null;
			object result;
			if (flag)
			{
				result = Visibility.Hidden;
			}
			else
			{
				int num = int.Parse(value.ToString());
				result = ((num == 0) ? Visibility.Hidden : Visibility.Visible);
			}
			return result;
		}

		// Token: 0x0600271B RID: 10011 RVA: 0x001091FB File Offset: 0x001073FB
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
