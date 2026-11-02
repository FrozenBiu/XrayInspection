using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace GUI_XRS
{
	// Token: 0x020001C9 RID: 457
	[ValueConversion(typeof(bool), typeof(Style))]
	public class BoolFontWeightConverter : IValueConverter
	{
		// Token: 0x06002792 RID: 10130 RVA: 0x0010AD64 File Offset: 0x00108F64
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			bool flag = targetType != typeof(FontWeight);
			object result;
			if (flag)
			{
				result = null;
			}
			else
			{
				result = (bool.Parse(value.ToString()) ? FontWeights.Bold : FontWeights.Normal);
			}
			return result;
		}

		// Token: 0x06002793 RID: 10131 RVA: 0x001091FB File Offset: 0x001073FB
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
