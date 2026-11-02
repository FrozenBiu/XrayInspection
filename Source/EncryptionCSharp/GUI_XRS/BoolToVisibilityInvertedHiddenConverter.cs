using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace GUI_XRS
{
	// Token: 0x0200019E RID: 414
	[ValueConversion(typeof(bool), typeof(Visibility))]
	public class BoolToVisibilityInvertedHiddenConverter : IValueConverter
	{
		// Token: 0x06002711 RID: 10001 RVA: 0x00109204 File Offset: 0x00107404
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
				result = (bool.Parse(value.ToString()) ? Visibility.Hidden : Visibility.Visible);
			}
			return result;
		}

		// Token: 0x06002712 RID: 10002 RVA: 0x001091FB File Offset: 0x001073FB
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
