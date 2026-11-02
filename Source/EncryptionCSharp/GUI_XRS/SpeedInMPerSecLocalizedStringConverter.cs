using System;
using System.Globalization;
using System.Windows.Data;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x020001AC RID: 428
	[ValueConversion(typeof(double), typeof(string))]
	public class SpeedInMPerSecLocalizedStringConverter : IMultiValueConverter
	{
		// Token: 0x0600273B RID: 10043 RVA: 0x001098A8 File Offset: 0x00107AA8
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			bool flag = values != null && values.Length == 2 && values[0].GetType() == typeof(double) && values[1].GetType() == typeof(int);
			if (flag)
			{
				bool flag2 = (int)values[1] == 0;
				if (flag2)
				{
					double num = (double)values[0] * Definition.meterToFeetFactor * 60.0;
					return string.Format("{0:0.0}", num);
				}
				bool flag3 = (int)values[1] == 1;
				if (flag3)
				{
					double num = (double)values[0];
					return string.Format("{0:0.00}", num);
				}
				bool flag4 = (int)values[1] == 2;
				if (flag4)
				{
					double num = (double)values[0] * 60.0;
					return string.Format("{0:0.00}", num);
				}
			}
			return null;
		}

		// Token: 0x0600273C RID: 10044 RVA: 0x001099A4 File Offset: 0x00107BA4
		public object[] ConvertBack(object values, Type[] targetType, object parameter, CultureInfo culture)
		{
			return new object[]
			{
				new NotImplementedException()
			};
		}
	}
}
