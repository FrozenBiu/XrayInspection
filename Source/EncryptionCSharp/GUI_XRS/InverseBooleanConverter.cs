using System;
using System.Globalization;
using System.Windows.Data;

namespace GUI_XRS
{
	// Token: 0x0200019C RID: 412
	public class InverseBooleanConverter : IValueConverter
	{
		// Token: 0x0600270B RID: 9995 RVA: 0x00109158 File Offset: 0x00107358
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			bool flag2;
			if (value is bool)
			{
				bool flag = (bool)value;
				flag2 = !flag;
			}
			else
			{
				flag2 = false;
			}
			return flag2;
		}

		// Token: 0x0600270C RID: 9996 RVA: 0x00109188 File Offset: 0x00107388
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			bool flag2;
			if (value is bool)
			{
				bool flag = (bool)value;
				flag2 = !flag;
			}
			else
			{
				flag2 = false;
			}
			return flag2;
		}
	}
}
