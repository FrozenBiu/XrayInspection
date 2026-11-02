using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace GUI_XRS
{
	// Token: 0x020001B1 RID: 433
	[ValueConversion(typeof(Days), typeof(string[]))]
	public class DaysStringConverter : IValueConverter
	{
		// Token: 0x0600274A RID: 10058 RVA: 0x00109E0C File Offset: 0x0010800C
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			List<string> list = new List<string>();
			bool flag = value is Days;
			if (flag)
			{
				using (List<DayOfWeek>.Enumerator enumerator = ((Days)value).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						switch (enumerator.Current)
						{
						case DayOfWeek.Sunday:
							list.Add(LocalizationManager.Localize("txtSunday"));
							break;
						case DayOfWeek.Monday:
							list.Add(LocalizationManager.Localize("txtMonday"));
							break;
						case DayOfWeek.Tuesday:
							list.Add(LocalizationManager.Localize("txtTuesday"));
							break;
						case DayOfWeek.Wednesday:
							list.Add(LocalizationManager.Localize("txtWednesday"));
							break;
						case DayOfWeek.Thursday:
							list.Add(LocalizationManager.Localize("txtThursday"));
							break;
						case DayOfWeek.Friday:
							list.Add(LocalizationManager.Localize("txtFriday"));
							break;
						case DayOfWeek.Saturday:
							list.Add(LocalizationManager.Localize("txtSaturday"));
							break;
						}
					}
				}
			}
			return list;
		}

		// Token: 0x0600274B RID: 10059 RVA: 0x001091FB File Offset: 0x001073FB
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
