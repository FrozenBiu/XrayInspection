using System;
using System.Globalization;
using System.Windows.Data;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x020001E4 RID: 484
	[ValueConversion(typeof(XRSRegionType), typeof(string))]
	public class RegionTypeStringConverter : IValueConverter
	{
		// Token: 0x060027E3 RID: 10211 RVA: 0x0010BCA8 File Offset: 0x00109EA8
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string result = "";
			bool flag = value is XRSRegionType;
			if (flag)
			{
				switch ((XRSRegionType)value)
				{
				case XRSRegionType.Rectangle:
					result = LocalizationManager.Localize("txtRectangle");
					break;
				case XRSRegionType.Polygon:
					result = LocalizationManager.Localize("txtPolygon");
					break;
				case XRSRegionType.Ellipse:
					result = LocalizationManager.Localize("txtEllipse");
					break;
				}
			}
			return result;
		}

		// Token: 0x060027E4 RID: 10212 RVA: 0x0010BD1C File Offset: 0x00109F1C
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			XRSRegionType xrsregionType = XRSRegionType.Rectangle;
			bool flag = value is string;
			if (flag)
			{
				string a = (string)value;
				bool flag2 = a == "Rechteck";
				if (flag2)
				{
					xrsregionType = XRSRegionType.Rectangle;
				}
				else
				{
					bool flag3 = a == "Polygon";
					if (flag3)
					{
						xrsregionType = XRSRegionType.Polygon;
					}
				}
			}
			return xrsregionType;
		}
	}
}
