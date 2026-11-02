using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace GUI_XRS
{
	// Token: 0x020001D2 RID: 466
	[ValueConversion(typeof(int), typeof(GridLength))]
	public class IntGridLengthAutoConverter : IValueConverter
	{
		// Token: 0x060027AD RID: 10157 RVA: 0x0010B1F8 File Offset: 0x001093F8
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			int num = 0;
			bool flag = targetType != typeof(GridLength);
			object result;
			if (flag)
			{
				result = null;
			}
			else
			{
				bool flag2 = value is int;
				if (flag2)
				{
					num = (int)value;
				}
				result = ((num > 0) ? GridLength.Auto : new GridLength(0.0));
			}
			return result;
		}

		// Token: 0x060027AE RID: 10158 RVA: 0x001091FB File Offset: 0x001073FB
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
