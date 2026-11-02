using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace GUI_XRS
{
	// Token: 0x020001DD RID: 477
	public class ChangeToBrushConverter : IValueConverter
	{
		// Token: 0x060027CE RID: 10190 RVA: 0x0010B978 File Offset: 0x00109B78
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			Brush brush = Brushes.White;
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
					if (flag3)
					{
						brush = Brushes.Red;
					}
					else
					{
						brush = Brushes.White;
					}
				}
				result = brush;
			}
			return result;
		}

		// Token: 0x060027CF RID: 10191 RVA: 0x001091FB File Offset: 0x001073FB
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
