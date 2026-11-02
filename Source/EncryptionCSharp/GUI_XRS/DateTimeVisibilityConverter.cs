using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace GUI_XRS
{
	// Token: 0x020001EC RID: 492
	[ValueConversion(typeof(DateTime), typeof(Visibility))]
	public class DateTimeVisibilityConverter : IValueConverter
	{
		// Token: 0x060027FB RID: 10235 RVA: 0x0010C00C File Offset: 0x0010A20C
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
				bool flag2 = value is DateTime;
				if (flag2)
				{
					DateTime d = (DateTime)value;
					bool flag3 = d == default(DateTime);
					if (flag3)
					{
						result = Visibility.Hidden;
					}
					else
					{
						result = Visibility.Visible;
					}
				}
				else
				{
					result = null;
				}
			}
			return result;
		}

		// Token: 0x060027FC RID: 10236 RVA: 0x001091FB File Offset: 0x001073FB
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
