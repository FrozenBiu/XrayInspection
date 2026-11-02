using System;
using System.Globalization;
using System.Windows.Data;

namespace GUI_XRS
{
	// Token: 0x020001C7 RID: 455
	[ValueConversion(typeof(int), typeof(int))]
	public class HalfOfIntConverter : IValueConverter
	{
		// Token: 0x0600278C RID: 10124 RVA: 0x0010AC88 File Offset: 0x00108E88
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			int num = 0;
			bool flag = value is int;
			if (flag)
			{
				num = (int)value;
				num /= 2;
			}
			return num;
		}

		// Token: 0x0600278D RID: 10125 RVA: 0x001091FB File Offset: 0x001073FB
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
