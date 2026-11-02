using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace GUI_XRS
{
	// Token: 0x020001CE RID: 462
	[ValueConversion(typeof(bool), typeof(GridLength))]
	public class BoolGridLengthDobuleStarConverter : IValueConverter
	{
		// Token: 0x060027A1 RID: 10145 RVA: 0x0010B060 File Offset: 0x00109260
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
				result = (flag ? new GridLength(2.0, GridUnitType.Star) : new GridLength(0.0));
			}
			return result;
		}

		// Token: 0x060027A2 RID: 10146 RVA: 0x001091FB File Offset: 0x001073FB
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
