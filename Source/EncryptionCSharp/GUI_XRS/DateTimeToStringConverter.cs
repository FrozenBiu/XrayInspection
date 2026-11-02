using System;
using System.Globalization;
using System.Windows.Data;

namespace GUI_XRS
{
	// Token: 0x020001B5 RID: 437
	[ValueConversion(typeof(DateTime), typeof(string))]
	public class DateTimeToStringConverter : IValueConverter
	{
		// Token: 0x06002756 RID: 10070 RVA: 0x0010A0D4 File Offset: 0x001082D4
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			bool flag = targetType != typeof(string);
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
					result = ((DateTime)value).ToString(LocalizationManager.CurrentCulture);
				}
				else
				{
					result = null;
				}
			}
			return result;
		}

		// Token: 0x06002757 RID: 10071 RVA: 0x001091FB File Offset: 0x001073FB
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
