using System;
using System.Globalization;
using System.Windows.Data;

namespace GUI_XRS
{
	// Token: 0x020001AD RID: 429
	[ValueConversion(typeof(bool), typeof(string))]
	public class BoolToStringConverter : IValueConverter
	{
		// Token: 0x0600273E RID: 10046 RVA: 0x001099C4 File Offset: 0x00107BC4
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			bool flag = targetType != typeof(string);
			object result;
			if (flag)
			{
				result = null;
			}
			else
			{
				bool flag2 = value is bool;
				if (flag2)
				{
					bool flag3 = (bool)value;
					if (flag3)
					{
						result = "1";
					}
					else
					{
						result = "0";
					}
				}
				else
				{
					result = "0";
				}
			}
			return result;
		}

		// Token: 0x0600273F RID: 10047 RVA: 0x001091FB File Offset: 0x001073FB
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
