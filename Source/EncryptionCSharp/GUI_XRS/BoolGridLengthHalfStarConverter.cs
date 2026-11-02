using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace GUI_XRS
{
	// Token: 0x020001CD RID: 461
	[ValueConversion(typeof(bool), typeof(GridLength))]
	public class BoolGridLengthHalfStarConverter : IValueConverter
	{
		// Token: 0x0600279E RID: 10142 RVA: 0x0010AFF8 File Offset: 0x001091F8
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
				result = (flag ? new GridLength(0.5, GridUnitType.Star) : new GridLength(0.0));
			}
			return result;
		}

		// Token: 0x0600279F RID: 10143 RVA: 0x001091FB File Offset: 0x001073FB
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
