using System;
using System.Globalization;
using System.Windows.Data;

namespace GUI_XRS
{
	// Token: 0x020001D5 RID: 469
	[ValueConversion(typeof(int), typeof(string))]
	public class ImageGroupStringConverter : IValueConverter
	{
		// Token: 0x060027B6 RID: 10166 RVA: 0x0010B378 File Offset: 0x00109578
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			bool flag = value is int;
			object result;
			if (flag)
			{
				int num = (int)value;
				result = ((num > 0) ? num.ToString() : LocalizationManager.Localize("txtAll"));
			}
			else
			{
				result = "";
			}
			return result;
		}

		// Token: 0x060027B7 RID: 10167 RVA: 0x0010B3C0 File Offset: 0x001095C0
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			int num = 0;
			bool flag = value is string;
			if (flag)
			{
				string text = (string)value;
				bool flag2 = text == LocalizationManager.Localize("txtAll");
				if (flag2)
				{
					num = 0;
				}
				else
				{
					int.TryParse(text, out num);
				}
			}
			return num;
		}
	}
}
