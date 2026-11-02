using System;
using System.Globalization;
using System.Windows.Data;

namespace GUI_XRS
{
	// Token: 0x020001AE RID: 430
	[ValueConversion(typeof(AlarmMode), typeof(string))]
	public class AlarmmodeStringConverter : IValueConverter
	{
		// Token: 0x06002741 RID: 10049 RVA: 0x00109A1C File Offset: 0x00107C1C
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string result = "";
			bool flag = value is AlarmMode;
			if (flag)
			{
				switch ((AlarmMode)value)
				{
				case AlarmMode.PerHour:
					result = LocalizationManager.Localize("txtHourly");
					break;
				case AlarmMode.PerDay:
					result = LocalizationManager.Localize("txtDaily");
					break;
				case AlarmMode.PerWeek:
					result = LocalizationManager.Localize("txtWeekly");
					break;
				case AlarmMode.PerProductChange:
					result = LocalizationManager.Localize("txtPerProductChange");
					break;
				case AlarmMode.PerInterval:
					result = LocalizationManager.Localize("txtPerInterval");
					break;
				}
			}
			return result;
		}

		// Token: 0x06002742 RID: 10050 RVA: 0x00109AB0 File Offset: 0x00107CB0
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			AlarmMode alarmMode = AlarmMode.PerHour;
			bool flag = value is string;
			if (flag)
			{
				string a = (string)value;
				bool flag2 = a == LocalizationManager.Localize("txtDaily");
				if (flag2)
				{
					alarmMode = AlarmMode.PerDay;
				}
				else
				{
					bool flag3 = a == LocalizationManager.Localize("txtHourly");
					if (flag3)
					{
						alarmMode = AlarmMode.PerHour;
					}
					else
					{
						bool flag4 = a == LocalizationManager.Localize("txtWeekly");
						if (flag4)
						{
							alarmMode = AlarmMode.PerWeek;
						}
						else
						{
							bool flag5 = a == LocalizationManager.Localize("txtPerProductChange");
							if (flag5)
							{
								alarmMode = AlarmMode.PerProductChange;
							}
							else
							{
								bool flag6 = a == LocalizationManager.Localize("txtPerInterval");
								if (flag6)
								{
									alarmMode = AlarmMode.PerInterval;
								}
							}
						}
					}
				}
			}
			return alarmMode;
		}
	}
}
