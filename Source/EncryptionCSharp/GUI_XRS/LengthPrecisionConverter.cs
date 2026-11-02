using System;
using System.Globalization;
using System.Windows.Data;

namespace GUI_XRS
{
	// Token: 0x020001D4 RID: 468
	public class LengthPrecisionConverter : IMultiValueConverter
	{
		// Token: 0x060027B3 RID: 10163 RVA: 0x0010B2BC File Offset: 0x001094BC
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			bool flag = values != null && values.Length == 2 && values[0].GetType() == typeof(double) && values[1].GetType() == typeof(bool);
			object result;
			if (flag)
			{
				bool flag2 = (bool)values[1];
				if (flag2)
				{
					double num = (double)values[0];
					result = string.Format("{0:0}", num);
				}
				else
				{
					double num = (double)values[0];
					result = string.Format("{0:0.00}", num);
				}
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x060027B4 RID: 10164 RVA: 0x0010B358 File Offset: 0x00109558
		public object[] ConvertBack(object values, Type[] targetType, object parameter, CultureInfo culture)
		{
			return new object[]
			{
				new NotImplementedException()
			};
		}
	}
}
