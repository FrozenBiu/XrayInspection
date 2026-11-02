using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace GUI_XRS
{
	// Token: 0x020001D7 RID: 471
	public class BoolGridLengthFixedValueConverter : IMultiValueConverter
	{
		// Token: 0x060027BC RID: 10172 RVA: 0x0010B4AC File Offset: 0x001096AC
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			bool flag = false;
			bool flag2 = values[0] is bool;
			if (flag2)
			{
				flag = (bool)values[0];
			}
			bool flag3 = flag;
			if (flag3)
			{
				bool flag4 = values[1] is int;
				if (flag4)
				{
					int num = (int)values[1];
					return new GridLength((double)num);
				}
			}
			return new GridLength(0.0);
		}

		// Token: 0x060027BD RID: 10173 RVA: 0x001091FB File Offset: 0x001073FB
		public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
