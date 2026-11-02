using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace GUI_XRS
{
	// Token: 0x02000215 RID: 533
	public class ReferenceProductConverter : IMultiValueConverter
	{
		// Token: 0x06002876 RID: 10358 RVA: 0x0010E0D8 File Offset: 0x0010C2D8
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			bool flag = values[0] != null && values[1] != null;
			object result;
			if (flag)
			{
				bool flag2 = values.Length == 2 && values[0] is bool && values[1] is string;
				if (flag2)
				{
					bool flag3 = ((string)values[1]).Equals("-1");
					if (flag3)
					{
						bool flag4 = (bool)values[0];
						if (flag4)
						{
							result = Visibility.Visible;
						}
						else
						{
							result = Visibility.Collapsed;
						}
					}
					else
					{
						result = Visibility.Visible;
					}
				}
				else
				{
					result = Visibility.Visible;
				}
			}
			else
			{
				result = Visibility.Collapsed;
			}
			return result;
		}

		// Token: 0x06002877 RID: 10359 RVA: 0x001091FB File Offset: 0x001073FB
		public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
