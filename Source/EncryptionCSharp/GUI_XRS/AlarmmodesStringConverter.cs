using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace GUI_XRS
{
	// Token: 0x020001AF RID: 431
	[ValueConversion(typeof(AlarmModes), typeof(string[]))]
	public class AlarmmodesStringConverter : IValueConverter
	{
		// Token: 0x06002744 RID: 10052 RVA: 0x00109B64 File Offset: 0x00107D64
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			List<string> list = new List<string>();
			bool flag = value is AlarmModes;
			if (flag)
			{
				using (List<AlarmMode>.Enumerator enumerator = ((AlarmModes)value).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						switch (enumerator.Current)
						{
						case AlarmMode.PerHour:
							list.Add(LocalizationManager.Localize("txtHourly"));
							break;
						case AlarmMode.PerDay:
							list.Add(LocalizationManager.Localize("txtDaily"));
							break;
						case AlarmMode.PerWeek:
							list.Add(LocalizationManager.Localize("txtWeekly"));
							break;
						case AlarmMode.PerProductChange:
							list.Add(LocalizationManager.Localize("txtPerProductChange"));
							break;
						case AlarmMode.PerInterval:
							list.Add(LocalizationManager.Localize("txtPerInterval"));
							break;
						}
					}
				}
			}
			return list;
		}

		// Token: 0x06002745 RID: 10053 RVA: 0x001091FB File Offset: 0x001073FB
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
