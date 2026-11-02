using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace GUI_XRS
{
	// Token: 0x020001D1 RID: 465
	[ValueConversion(typeof(bool), typeof(GridLength))]
	public class BoolGridLengthAutoConverter : IValueConverter
	{
		// Token: 0x060027AA RID: 10154 RVA: 0x0010B198 File Offset: 0x00109398
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
				result = (flag ? GridLength.Auto : new GridLength(0.0));
			}
			return result;
		}

		// Token: 0x060027AB RID: 10155 RVA: 0x001091FB File Offset: 0x001073FB
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
