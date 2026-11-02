using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace GUI_XRS
{
	// Token: 0x020001D9 RID: 473
	[ValueConversion(typeof(List<RejectMode>), typeof(string[]))]
	public class RejectModesStringConverter : IValueConverter
	{
		// Token: 0x060027C2 RID: 10178 RVA: 0x0010B5B4 File Offset: 0x001097B4
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			List<string> list = new List<string>();
			bool flag = value is List<RejectMode>;
			if (flag)
			{
				foreach (RejectMode rejectMode in ((List<RejectMode>)value))
				{
					bool flag2 = rejectMode == RejectMode.Center;
					if (flag2)
					{
						list.Add(LocalizationManager.Localize("txtRejectModeCenter"));
					}
					else
					{
						list.Add(LocalizationManager.Localize("txtRejectModeLeadingEdge"));
					}
				}
			}
			return list;
		}

		// Token: 0x060027C3 RID: 10179 RVA: 0x001091FB File Offset: 0x001073FB
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
