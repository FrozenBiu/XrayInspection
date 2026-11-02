using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace GUI_XRS
{
	// Token: 0x020001C5 RID: 453
	[ValueConversion(typeof(bool), typeof(Brush))]
	public class ComplianceBoolToBackgroundColorConverter : IValueConverter
	{
		// Token: 0x06002786 RID: 10118 RVA: 0x0010ABE4 File Offset: 0x00108DE4
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			bool flag = targetType != typeof(Brush);
			object result;
			if (flag)
			{
				result = null;
			}
			else
			{
				result = (bool.Parse(value.ToString()) ? Brushes.Red : Brushes.Transparent);
			}
			return result;
		}

		// Token: 0x06002787 RID: 10119 RVA: 0x001091FB File Offset: 0x001073FB
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
