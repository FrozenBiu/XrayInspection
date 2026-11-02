using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x020001E8 RID: 488
	public class RegionTypeEllipseVisibleConverter : IValueConverter
	{
		// Token: 0x060027EF RID: 10223 RVA: 0x0010BE8C File Offset: 0x0010A08C
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			bool flag = value is XRSRegionType;
			object result;
			if (flag)
			{
				result = (((XRSRegionType)value == XRSRegionType.Ellipse) ? Visibility.Visible : Visibility.Collapsed);
			}
			else
			{
				result = Visibility.Collapsed;
			}
			return result;
		}

		// Token: 0x060027F0 RID: 10224 RVA: 0x001091FB File Offset: 0x001073FB
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
