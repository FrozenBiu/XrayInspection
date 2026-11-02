using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace GUI_XRS
{
	// Token: 0x020001BF RID: 447
	[ValueConversion(typeof(bool), typeof(Brush))]
	public class BoolToBrushConverter : IValueConverter
	{
		// Token: 0x06002774 RID: 10100 RVA: 0x0010A8EC File Offset: 0x00108AEC
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			bool flag = targetType != typeof(Brush);
			object result;
			if (flag)
			{
				result = null;
			}
			else
			{
				result = (bool.Parse(value.ToString()) ? Brushes.Black : Brushes.Gray);
			}
			return result;
		}

		// Token: 0x06002775 RID: 10101 RVA: 0x001091FB File Offset: 0x001073FB
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
