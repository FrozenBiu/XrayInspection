using System;
using System.Globalization;
using System.Windows.Data;

namespace GUI_XRS
{
	// Token: 0x020001B4 RID: 436
	[ValueConversion(typeof(int), typeof(string))]
	public class IntSecondsToHoursMinutesSecondsStringeConverter : IValueConverter
	{
		// Token: 0x06002753 RID: 10067 RVA: 0x0010A030 File Offset: 0x00108230
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			bool flag = value != null && value.GetType() == typeof(int);
			object result;
			if (flag)
			{
				int num = (int)value;
				int num2 = num / 3600;
				num %= 3600;
				int num3 = num / 60;
				int num4 = num % 60;
				result = string.Concat(new string[]
				{
					num2.ToString("D2"),
					":",
					num3.ToString("D2"),
					":",
					num4.ToString("D2")
				});
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06002754 RID: 10068 RVA: 0x001091FB File Offset: 0x001073FB
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
