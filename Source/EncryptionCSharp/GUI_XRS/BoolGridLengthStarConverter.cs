using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace GUI_XRS
{
	// Token: 0x020001CF RID: 463
	[ValueConversion(typeof(bool), typeof(GridLength))]
	public class BoolGridLengthStarConverter : IValueConverter
	{
		// Token: 0x060027A4 RID: 10148 RVA: 0x0010B0C8 File Offset: 0x001092C8
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			bool flag = false;
			bool flag2 = targetType != typeof(GridLength);
			object result;
			if (flag2)
			{
				result = null;
			}
			else
			{
				bool flag3 = value is bool;
				if (flag3)
				{
					flag = (bool)value;
				}
				result = (flag ? new GridLength(1.0, GridUnitType.Star) : new GridLength(0.0));
			}
			return result;
		}

		// Token: 0x060027A5 RID: 10149 RVA: 0x001091FB File Offset: 0x001073FB
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
