using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace GUI_XRS
{
	// Token: 0x020001A4 RID: 420
	[ValueConversion(typeof(bool), typeof(Style))]
	public class BoolToTextBoxStyleConverter : IMultiValueConverter
	{
		// Token: 0x06002723 RID: 10019 RVA: 0x00109430 File Offset: 0x00107630
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			FrameworkElement frameworkElement = values[0] as FrameworkElement;
			bool flag;
			bool.TryParse(values[1].ToString(), out flag);
			bool flag2 = flag;
			Style result;
			if (flag2)
			{
				result = (Style)frameworkElement.TryFindResource("TextBoxStyle");
			}
			else
			{
				result = (Style)frameworkElement.TryFindResource("TextBoxDisabledStyle");
			}
			return result;
		}

		// Token: 0x06002724 RID: 10020 RVA: 0x001091FB File Offset: 0x001073FB
		public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
