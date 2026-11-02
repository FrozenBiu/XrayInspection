using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace GUI_XRS
{
	// Token: 0x020001E3 RID: 483
	[ValueConversion(typeof(object), typeof(Visibility))]
	public class RefVisibilityCollapsedConverter : IValueConverter
	{
		// Token: 0x060027E0 RID: 10208 RVA: 0x0010BC70 File Offset: 0x00109E70
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			bool flag = targetType != typeof(Visibility);
			object result;
			if (flag)
			{
				result = null;
			}
			else
			{
				result = ((value == null) ? Visibility.Collapsed : Visibility.Visible);
			}
			return result;
		}

		// Token: 0x060027E1 RID: 10209 RVA: 0x001091FB File Offset: 0x001073FB
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
