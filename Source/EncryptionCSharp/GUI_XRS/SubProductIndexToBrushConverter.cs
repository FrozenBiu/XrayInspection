using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x020001E1 RID: 481
	[ValueConversion(typeof(int), typeof(Brush))]
	public class SubProductIndexToBrushConverter : IValueConverter
	{
		// Token: 0x060027DA RID: 10202 RVA: 0x0010BC08 File Offset: 0x00109E08
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			Brush result = null;
			bool flag = value is int;
			if (flag)
			{
				int index = (int)value;
				result = XRSImageDefs.GetRegionStroke(index);
			}
			return result;
		}

		// Token: 0x060027DB RID: 10203 RVA: 0x001091FB File Offset: 0x001073FB
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
