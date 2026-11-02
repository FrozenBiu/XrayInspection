using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace GUI_XRS
{
	// Token: 0x02000214 RID: 532
	[ValueConversion(typeof(bool), typeof(Style))]
	public class BoolToTextBoxStyleConverter3 : IMultiValueConverter
	{
		// Token: 0x06002873 RID: 10355 RVA: 0x0010E050 File Offset: 0x0010C250
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			FrameworkElement frameworkElement = values[0] as FrameworkElement;
			bool flag;
			bool.TryParse(values[1].ToString(), out flag);
			bool flag2 = flag;
			Style result;
			if (flag2)
			{
				bool flag3 = !(bool)values[2];
				if (flag3)
				{
					result = (Style)frameworkElement.TryFindResource("TextBoxStyle");
				}
				else
				{
					result = (Style)frameworkElement.TryFindResource("disabledTextBox");
				}
			}
			else
			{
				result = (Style)frameworkElement.TryFindResource("disabledTextBox");
			}
			return result;
		}

		// Token: 0x06002874 RID: 10356 RVA: 0x001091FB File Offset: 0x001073FB
		public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
