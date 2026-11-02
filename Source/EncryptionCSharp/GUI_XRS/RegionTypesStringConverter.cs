using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x020001E5 RID: 485
	[ValueConversion(typeof(XRSRegionTypes), typeof(string[]))]
	public class RegionTypesStringConverter : IValueConverter
	{
		// Token: 0x060027E6 RID: 10214 RVA: 0x0010BD74 File Offset: 0x00109F74
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			List<string> list = new List<string>();
			bool flag = value is XRSRegionTypes;
			if (flag)
			{
				foreach (XRSRegionType xrsregionType in ((XRSRegionTypes)value))
				{
					XRSRegionType xrsregionType2 = xrsregionType;
					XRSRegionType xrsregionType3 = xrsregionType2;
					if (xrsregionType3 != XRSRegionType.Rectangle)
					{
						if (xrsregionType3 == XRSRegionType.Polygon)
						{
							list.Add("Polygon");
						}
					}
					else
					{
						list.Add("Rechteck");
					}
				}
			}
			return list;
		}

		// Token: 0x060027E7 RID: 10215 RVA: 0x001091FB File Offset: 0x001073FB
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
