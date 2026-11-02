using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace GUI_XRS
{
	// Token: 0x020001FF RID: 511
	[ValueConversion(typeof(int), typeof(Visibility))]
	public class IntStringToVisibilityConverter : IValueConverter
	{
		// Token: 0x06002834 RID: 10292 RVA: 0x0010CB80 File Offset: 0x0010AD80
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string text = value as string;
			bool flag = targetType != typeof(Visibility) || text == null;
			object result;
			if (flag)
			{
				result = Visibility.Collapsed;
			}
			else
			{
				int num = 0;
				int.TryParse(text, out num);
				result = ((num <= 0) ? Visibility.Collapsed : Visibility.Visible);
			}
			return result;
		}

		// Token: 0x06002835 RID: 10293 RVA: 0x001091FB File Offset: 0x001073FB
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
