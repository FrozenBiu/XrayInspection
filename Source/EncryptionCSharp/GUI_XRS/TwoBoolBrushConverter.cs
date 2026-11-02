using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace GUI_XRS
{
	// Token: 0x020001C1 RID: 449
	public class TwoBoolBrushConverter : IMultiValueConverter
	{
		// Token: 0x0600277A RID: 10106 RVA: 0x0010A9EC File Offset: 0x00108BEC
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			bool flag = false;
			bool flag2 = false;
			bool flag3 = values[0] is bool;
			if (flag3)
			{
				flag = (bool)values[0];
			}
			bool flag4 = values[1] is bool;
			if (flag4)
			{
				flag2 = (bool)values[1];
			}
			return (flag && flag2) ? Brushes.Black : Brushes.Gray;
		}

		// Token: 0x0600277B RID: 10107 RVA: 0x001091FB File Offset: 0x001073FB
		public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
