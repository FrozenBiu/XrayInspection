using System;
using System.Globalization;
using System.Windows.Data;

namespace GUI_XRS
{
	// Token: 0x020001A9 RID: 425
	[ValueConversion(typeof(double), typeof(string))]
	public class DoubleStringConverter : IValueConverter
	{
		// Token: 0x06002732 RID: 10034 RVA: 0x001096A0 File Offset: 0x001078A0
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			bool flag = value != null && value.GetType() == typeof(double);
			object result;
			if (flag)
			{
				double value2 = (double)value;
				result = Math.Round(value2, 2).ToString();
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06002733 RID: 10035 RVA: 0x001096EC File Offset: 0x001078EC
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			bool flag = value != null && value.GetType() == typeof(string);
			if (flag)
			{
				string s = (string)value;
				double num;
				bool flag2 = double.TryParse(s, out num);
				if (flag2)
				{
					return num;
				}
			}
			return null;
		}
	}
}
