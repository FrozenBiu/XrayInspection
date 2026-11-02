using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace GUI_XRS
{
	// Token: 0x020001DA RID: 474
	[ValueConversion(typeof(int), typeof(WindowState))]
	public class IntWindowStateConverter : IValueConverter
	{
		// Token: 0x060027C5 RID: 10181 RVA: 0x0010B650 File Offset: 0x00109850
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			WindowState windowState = WindowState.Maximized;
			bool flag = value is int;
			if (flag)
			{
				bool flag2 = (int)value > 0;
				if (flag2)
				{
					windowState = WindowState.Normal;
				}
			}
			return windowState;
		}

		// Token: 0x060027C6 RID: 10182 RVA: 0x001091FB File Offset: 0x001073FB
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
