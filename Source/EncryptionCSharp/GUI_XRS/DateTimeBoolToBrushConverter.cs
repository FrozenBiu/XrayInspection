using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace GUI_XRS
{
	// Token: 0x020001EA RID: 490
	public class DateTimeBoolToBrushConverter : IMultiValueConverter
	{
		// Token: 0x060027F5 RID: 10229 RVA: 0x0010BF24 File Offset: 0x0010A124
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			bool flag = targetType == typeof(Brush) && values != null && values.Length == 2;
			if (flag)
			{
				bool flag2 = values[0] is DateTime && values[1] is bool;
				if (flag2)
				{
					DateTime d = (DateTime)values[0];
					bool flag3 = (bool)values[1];
					bool flag4 = d == default(DateTime);
					if (flag4)
					{
						return flag3 ? Brushes.White : Brushes.Black;
					}
					return Brushes.Gray;
				}
			}
			return null;
		}

		// Token: 0x060027F6 RID: 10230 RVA: 0x001091FB File Offset: 0x001073FB
		public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
