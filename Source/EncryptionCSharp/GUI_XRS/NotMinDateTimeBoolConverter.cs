using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace GUI_XRS
{
	// Token: 0x020001EB RID: 491
	[ValueConversion(typeof(DateTime), typeof(Visibility))]
	public class NotMinDateTimeBoolConverter : IValueConverter
	{
		// Token: 0x060027F8 RID: 10232 RVA: 0x0010BFC0 File Offset: 0x0010A1C0
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			bool flag = value is DateTime;
			object result;
			if (flag)
			{
				DateTime d = (DateTime)value;
				bool flag2 = d == DateTime.MinValue;
				if (flag2)
				{
					result = true;
				}
				else
				{
					result = false;
				}
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x060027F9 RID: 10233 RVA: 0x001091FB File Offset: 0x001073FB
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
