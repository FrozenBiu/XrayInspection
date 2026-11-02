using System;
using System.Globalization;
using System.Windows.Data;

namespace GUI_XRS
{
	// Token: 0x020001B3 RID: 435
	[ValueConversion(typeof(int), typeof(string))]
	public class IntSecondsToMinutesSecondsStringeConverter : IValueConverter
	{
		// Token: 0x06002750 RID: 10064 RVA: 0x00109FC4 File Offset: 0x001081C4
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			bool flag = value != null && value.GetType() == typeof(int);
			object result;
			if (flag)
			{
				int num = (int)value;
				int num2 = num / 60;
				int num3 = num % 60;
				result = num2.ToString("D2") + ":" + num3.ToString("D2");
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06002751 RID: 10065 RVA: 0x001091FB File Offset: 0x001073FB
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
