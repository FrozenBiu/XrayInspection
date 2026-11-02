using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace GUI_XRS
{
	// Token: 0x020001C0 RID: 448
	[ValueConversion(typeof(bool), typeof(Style))]
	public class StringToDialogStyleConverter : IMultiValueConverter
	{
		// Token: 0x06002777 RID: 10103 RVA: 0x0010A934 File Offset: 0x00108B34
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			FrameworkElement frameworkElement = values[0] as FrameworkElement;
			string a = null;
			bool flag = values[1] is string;
			if (flag)
			{
				a = (string)values[1];
			}
			bool flag2 = a == "ErrorStyle";
			Style result;
			if (flag2)
			{
				result = (Style)frameworkElement.TryFindResource("ErrorStyle");
			}
			else
			{
				bool flag3 = a == "WarningStyle";
				if (flag3)
				{
					result = (Style)frameworkElement.TryFindResource("WarningStyle");
				}
				else
				{
					bool flag4 = a == "OkStyle";
					if (flag4)
					{
						result = (Style)frameworkElement.TryFindResource("OkStyle");
					}
					else
					{
						result = (Style)frameworkElement.TryFindResource("DefaultStyle");
					}
				}
			}
			return result;
		}

		// Token: 0x06002778 RID: 10104 RVA: 0x001091FB File Offset: 0x001073FB
		public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
