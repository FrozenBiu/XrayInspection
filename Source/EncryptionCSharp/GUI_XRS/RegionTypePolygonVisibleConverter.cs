using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x020001E7 RID: 487
	public class RegionTypePolygonVisibleConverter : IValueConverter
	{
		// Token: 0x060027EC RID: 10220 RVA: 0x0010BE50 File Offset: 0x0010A050
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			bool flag = value is XRSRegionType;
			object result;
			if (flag)
			{
				result = (((XRSRegionType)value == XRSRegionType.Polygon) ? Visibility.Visible : Visibility.Collapsed);
			}
			else
			{
				result = Visibility.Collapsed;
			}
			return result;
		}

		// Token: 0x060027ED RID: 10221 RVA: 0x001091FB File Offset: 0x001073FB
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
