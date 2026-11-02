using System;
using System.Globalization;
using System.Windows.Data;

namespace GUI_XRS
{
	// Token: 0x02000213 RID: 531
	public class TwoBoolValuesToOneConverter : IMultiValueConverter
	{
		// Token: 0x06002870 RID: 10352 RVA: 0x0010DFC8 File Offset: 0x0010C1C8
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			bool flag = values[0] != null && values[1] != null;
			object result;
			if (flag)
			{
				bool flag2 = values.Length == 2 && values[0] is bool && values[1] is bool;
				if (!flag2)
				{
					throw new Exception("Invalid Arguments");
				}
				bool flag3 = !(bool)values[0] || (bool)values[1];
				if (flag3)
				{
					result = false;
				}
				else
				{
					result = true;
				}
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06002871 RID: 10353 RVA: 0x001091FB File Offset: 0x001073FB
		public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
