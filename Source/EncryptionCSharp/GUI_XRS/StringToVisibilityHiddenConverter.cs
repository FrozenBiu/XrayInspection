using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace GUI_XRS
{
	// Token: 0x020001B9 RID: 441
	public class StringToVisibilityHiddenConverter : IValueConverter
	{
		// Token: 0x06002762 RID: 10082 RVA: 0x0010A388 File Offset: 0x00108588
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string value2 = value as string;
			return (targetType != typeof(Visibility) || string.IsNullOrEmpty(value2)) ? Visibility.Hidden : Visibility.Visible;
		}

		// Token: 0x06002763 RID: 10083 RVA: 0x001091FB File Offset: 0x001073FB
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
