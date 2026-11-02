using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace GUI_XRS
{
	// Token: 0x020001A6 RID: 422
	[ValueConversion(typeof(bool), typeof(Style))]
	public class BoolToCheckBoxStyleConverter : IMultiValueConverter
	{
		// Token: 0x06002729 RID: 10025 RVA: 0x001094F0 File Offset: 0x001076F0
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			FrameworkElement frameworkElement = values[0] as FrameworkElement;
			bool flag;
			bool.TryParse(values[1].ToString(), out flag);
			bool flag2 = flag;
			Style result;
			if (flag2)
			{
				result = (Style)frameworkElement.TryFindResource("styleCheckBox");
			}
			else
			{
				result = (Style)frameworkElement.TryFindResource("styleCheckBoxDisabled");
			}
			return result;
		}

		// Token: 0x0600272A RID: 10026 RVA: 0x001091FB File Offset: 0x001073FB
		public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
