using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace GUI_XRS
{
	// Token: 0x020001CC RID: 460
	public class BarItemActiveBrushConverter : IValueConverter
	{
		// Token: 0x0600279B RID: 10139 RVA: 0x0010AF2C File Offset: 0x0010912C
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			Brush brush = new LinearGradientBrush();
			bool flag = targetType != typeof(Brush);
			object result;
			if (flag)
			{
				result = null;
			}
			else
			{
				bool flag2 = value is bool;
				if (flag2)
				{
					bool flag3 = (bool)value;
					bool flag4 = flag3;
					if (flag4)
					{
						brush = new LinearGradientBrush(Color.FromArgb(byte.MaxValue, 128, 128, 128), Color.FromArgb(128, 128, 128, 128), new Point(0.0, 0.5), new Point(1.0, 0.5));
					}
					else
					{
						brush = Brushes.Transparent;
					}
				}
				result = brush;
			}
			return result;
		}

		// Token: 0x0600279C RID: 10140 RVA: 0x001091FB File Offset: 0x001073FB
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
