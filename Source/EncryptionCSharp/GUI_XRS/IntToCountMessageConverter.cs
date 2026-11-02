using System;
using System.Globalization;
using System.Windows.Data;

namespace GUI_XRS
{
	// Token: 0x020001A3 RID: 419
	[ValueConversion(typeof(int), typeof(string))]
	public class IntToCountMessageConverter : IValueConverter
	{
		// Token: 0x06002720 RID: 10016 RVA: 0x00109400 File Offset: 0x00107600
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			int num = int.Parse(value.ToString());
			return (num <= 1) ? "" : value.ToString();
		}

		// Token: 0x06002721 RID: 10017 RVA: 0x001091FB File Offset: 0x001073FB
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
