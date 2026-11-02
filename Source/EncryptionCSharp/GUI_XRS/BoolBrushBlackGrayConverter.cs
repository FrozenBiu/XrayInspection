using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace GUI_XRS
{
	// Token: 0x020001ED RID: 493
	[ValueConversion(typeof(bool), typeof(Brush))]
	public class BoolBrushBlackGrayConverter : IValueConverter
	{
		// Token: 0x060027FE RID: 10238 RVA: 0x0010C078 File Offset: 0x0010A278
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
				bool flag2 = value is bool;
				if (flag2)
				{
					result = (((bool)value) ? new SolidColorBrush(Color.FromRgb(39, 39, 39)) : Brushes.Gray);
				}
				else
				{
					result = Brushes.Black;
				}
			}
			return result;
		}

		// Token: 0x060027FF RID: 10239 RVA: 0x001091FB File Offset: 0x001073FB
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
