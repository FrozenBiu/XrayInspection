using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace GUI_XRS
{
	// Token: 0x020001BC RID: 444
	[ValueConversion(typeof(RejectPriorities), typeof(string[]))]
	public class RejectPrioritiesStringConverter : IValueConverter
	{
		// Token: 0x0600276B RID: 10091 RVA: 0x0010A54C File Offset: 0x0010874C
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			List<string> list = new List<string>();
			bool flag = value is RejectPriorities;
			if (flag)
			{
				using (List<RejectPriority>.Enumerator enumerator = ((RejectPriorities)value).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						switch (enumerator.Current)
						{
						case RejectPriority.Highest:
							list.Add(LocalizationManager.Localize("txtHighest"));
							break;
						case RejectPriority.High:
							list.Add(LocalizationManager.Localize("txtHigh"));
							break;
						case RejectPriority.Low:
							list.Add(LocalizationManager.Localize("txtLow"));
							break;
						case RejectPriority.Lowest:
							list.Add(LocalizationManager.Localize("txtLowest"));
							break;
						}
					}
				}
			}
			return list;
		}

		// Token: 0x0600276C RID: 10092 RVA: 0x001091FB File Offset: 0x001073FB
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
