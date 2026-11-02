using System;
using System.Globalization;
using System.Windows.Data;

namespace GUI_XRS
{
	// Token: 0x020001AA RID: 426
	[ValueConversion(typeof(double), typeof(string))]
	public class DoubleStringConverterPrec1 : IValueConverter
	{
		// Token: 0x06002735 RID: 10037 RVA: 0x00109740 File Offset: 0x00107940
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			bool flag = value != null && value.GetType() == typeof(double);
			object result;
			if (flag)
			{
				double value2 = (double)value;
				result = Math.Round(value2, 1).ToString();
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06002736 RID: 10038 RVA: 0x0010978C File Offset: 0x0010798C
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
