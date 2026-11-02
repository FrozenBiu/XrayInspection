using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace GUI_XRS
{
	// Token: 0x020001D3 RID: 467
	public class ProductHeightLabelMarginConverter : IValueConverter
	{
		// Token: 0x060027B0 RID: 10160 RVA: 0x0010B258 File Offset: 0x00109458
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			int num = 77;
			Thickness thickness = default(Thickness);
			bool flag = targetType != typeof(Thickness);
			object result;
			if (flag)
			{
				result = null;
			}
			else
			{
				bool flag2 = value is int;
				if (flag2)
				{
					int num2 = (int)value;
					thickness.Bottom = (double)(num2 + num);
				}
				result = thickness;
			}
			return result;
		}

		// Token: 0x060027B1 RID: 10161 RVA: 0x001091FB File Offset: 0x001073FB
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
