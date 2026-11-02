using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x020001E2 RID: 482
	[ValueConversion(typeof(int), typeof(DoubleCollection))]
	public class SubProductIndexToStrokeDashConverter : IValueConverter
	{
		// Token: 0x060027DD RID: 10205 RVA: 0x0010BC3C File Offset: 0x00109E3C
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			DoubleCollection result = null;
			bool flag = value is int;
			if (flag)
			{
				int index = (int)value;
				result = XRSImageDefs.GetRegionDashArray(index);
			}
			return result;
		}

		// Token: 0x060027DE RID: 10206 RVA: 0x001091FB File Offset: 0x001073FB
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
