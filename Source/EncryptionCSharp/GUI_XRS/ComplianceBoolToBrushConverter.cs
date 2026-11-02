using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace GUI_XRS
{
	// Token: 0x020001C4 RID: 452
	[ValueConversion(typeof(bool), typeof(Brush))]
	public class ComplianceBoolToBrushConverter : IValueConverter
	{
		// Token: 0x06002783 RID: 10115 RVA: 0x0010AB9C File Offset: 0x00108D9C
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
				result = (bool.Parse(value.ToString()) ? Brushes.Black : Brushes.Red);
			}
			return result;
		}

		// Token: 0x06002784 RID: 10116 RVA: 0x001091FB File Offset: 0x001073FB
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
