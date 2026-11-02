using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x020001E6 RID: 486
	public class RegionTypeRectangleVisibleConverter : IValueConverter
	{
		// Token: 0x060027E9 RID: 10217 RVA: 0x0010BE14 File Offset: 0x0010A014
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			bool flag = value is XRSRegionType;
			object result;
			if (flag)
			{
				result = (((XRSRegionType)value == XRSRegionType.Rectangle) ? Visibility.Visible : Visibility.Collapsed);
			}
			else
			{
				result = Visibility.Collapsed;
			}
			return result;
		}

		// Token: 0x060027EA RID: 10218 RVA: 0x001091FB File Offset: 0x001073FB
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
