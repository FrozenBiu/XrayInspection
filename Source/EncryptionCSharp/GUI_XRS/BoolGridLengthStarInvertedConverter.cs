using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace GUI_XRS
{
	// Token: 0x020001D0 RID: 464
	[ValueConversion(typeof(bool), typeof(GridLength))]
	public class BoolGridLengthStarInvertedConverter : IValueConverter
	{
		// Token: 0x060027A7 RID: 10151 RVA: 0x0010B130 File Offset: 0x00109330
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
				result = (flag ? new GridLength(0.0) : new GridLength(1.0, GridUnitType.Star));
			}
			return result;
		}

		// Token: 0x060027A8 RID: 10152 RVA: 0x001091FB File Offset: 0x001073FB
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
