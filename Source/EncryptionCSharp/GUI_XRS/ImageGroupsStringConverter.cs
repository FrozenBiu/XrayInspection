using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace GUI_XRS
{
	// Token: 0x020001D6 RID: 470
	[ValueConversion(typeof(List<int>), typeof(string[]))]
	public class ImageGroupsStringConverter : IValueConverter
	{
		// Token: 0x060027B9 RID: 10169 RVA: 0x0010B414 File Offset: 0x00109614
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			List<string> list = new List<string>();
			bool flag = value is List<int>;
			if (flag)
			{
				foreach (int num in ((List<int>)value))
				{
					bool flag2 = num > 0;
					if (flag2)
					{
						list.Add(num.ToString());
					}
					else
					{
						list.Add(LocalizationManager.Localize("txtAll"));
					}
				}
			}
			return list;
		}

		// Token: 0x060027BA RID: 10170 RVA: 0x001091FB File Offset: 0x001073FB
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
