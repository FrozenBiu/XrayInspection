using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace GUI_XRS
{
	// Token: 0x020001B8 RID: 440
	public class StringToVisibilityConverter : IValueConverter
	{
		// Token: 0x0600275F RID: 10079 RVA: 0x0010A34C File Offset: 0x0010854C
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string value2 = value as string;
			return (targetType != typeof(Visibility) || string.IsNullOrEmpty(value2)) ? Visibility.Collapsed : Visibility.Visible;
		}

		// Token: 0x06002760 RID: 10080 RVA: 0x001091FB File Offset: 0x001073FB
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
