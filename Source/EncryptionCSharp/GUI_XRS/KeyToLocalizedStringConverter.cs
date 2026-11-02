using System;
using System.Globalization;
using System.Windows.Data;

namespace GUI_XRS
{
	// Token: 0x020001B7 RID: 439
	public class KeyToLocalizedStringConverter : IValueConverter
	{
		// Token: 0x0600275C RID: 10076 RVA: 0x0010A300 File Offset: 0x00108500
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			bool flag = targetType != typeof(string);
			object result;
			if (flag)
			{
				result = null;
			}
			else
			{
				bool flag2 = value is string;
				if (flag2)
				{
					string text = LocalizationManager.Localize((string)value);
					result = text;
				}
				else
				{
					result = null;
				}
			}
			return result;
		}

		// Token: 0x0600275D RID: 10077 RVA: 0x001091FB File Offset: 0x001073FB
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
