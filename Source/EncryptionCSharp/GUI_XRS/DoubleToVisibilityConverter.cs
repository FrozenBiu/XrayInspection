using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace GUI_XRS
{
	// Token: 0x020001BA RID: 442
	[ValueConversion(typeof(double), typeof(Visibility))]
	public class DoubleToVisibilityConverter : IValueConverter
	{
		// Token: 0x06002765 RID: 10085 RVA: 0x0010A3C4 File Offset: 0x001085C4
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
				double num = 0.0;
				double.TryParse(value.ToString().Replace(',', '.'), NumberStyles.None, new CultureInfo("en-US"), out num);
				result = ((num == 0.0) ? Visibility.Hidden : Visibility.Visible);
			}
			return result;
		}

		// Token: 0x06002766 RID: 10086 RVA: 0x001091FB File Offset: 0x001073FB
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
