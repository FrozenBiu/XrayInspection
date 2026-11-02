using System;
using System.Globalization;
using System.Windows.Data;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x020001AB RID: 427
	[ValueConversion(typeof(double), typeof(string))]
	public class WeightInGrammWeightUnitConverter : IMultiValueConverter
	{
		// Token: 0x06002738 RID: 10040 RVA: 0x001097E0 File Offset: 0x001079E0
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			int digits = 1;
			bool flag = values != null && values.Length == 2 && values[0].GetType() == typeof(double) && values[1].GetType() == typeof(bool);
			object result;
			if (flag)
			{
				bool flag2 = (bool)values[1];
				double value;
				if (flag2)
				{
					value = (double)values[0];
				}
				else
				{
					value = (double)values[0] * XRSImageDefs.grammToOunceFactor;
					digits = 2;
				}
				result = Math.Round(value, digits).ToString("F" + digits.ToString());
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06002739 RID: 10041 RVA: 0x00109888 File Offset: 0x00107A88
		public object[] ConvertBack(object values, Type[] targetType, object parameter, CultureInfo culture)
		{
			return new object[]
			{
				new NotImplementedException()
			};
		}
	}
}
