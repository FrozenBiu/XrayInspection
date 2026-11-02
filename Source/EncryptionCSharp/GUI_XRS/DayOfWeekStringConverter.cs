using System;
using System.Globalization;
using System.Windows.Data;

namespace GUI_XRS
{
	// Token: 0x020001B0 RID: 432
	[ValueConversion(typeof(DayOfWeek), typeof(string))]
	public class DayOfWeekStringConverter : IValueConverter
	{
		// Token: 0x06002747 RID: 10055 RVA: 0x00109C60 File Offset: 0x00107E60
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string result = "";
			bool flag = value is DayOfWeek;
			if (flag)
			{
				switch ((DayOfWeek)value)
				{
				case DayOfWeek.Sunday:
					result = LocalizationManager.Localize("txtSunday");
					break;
				case DayOfWeek.Monday:
					result = LocalizationManager.Localize("txtMonday");
					break;
				case DayOfWeek.Tuesday:
					result = LocalizationManager.Localize("txtTuesday");
					break;
				case DayOfWeek.Wednesday:
					result = LocalizationManager.Localize("txtWednesday");
					break;
				case DayOfWeek.Thursday:
					result = LocalizationManager.Localize("txtThursday");
					break;
				case DayOfWeek.Friday:
					result = LocalizationManager.Localize("txtFriday");
					break;
				case DayOfWeek.Saturday:
					result = LocalizationManager.Localize("txtSaturday");
					break;
				}
			}
			return result;
		}

		// Token: 0x06002748 RID: 10056 RVA: 0x00109D1C File Offset: 0x00107F1C
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			DayOfWeek dayOfWeek = DayOfWeek.Monday;
			bool flag = value is string;
			if (flag)
			{
				string a = (string)value;
				bool flag2 = a == LocalizationManager.Localize("txtMonday");
				if (flag2)
				{
					dayOfWeek = DayOfWeek.Monday;
				}
				else
				{
					bool flag3 = a == LocalizationManager.Localize("txtTuesday");
					if (flag3)
					{
						dayOfWeek = DayOfWeek.Tuesday;
					}
					else
					{
						bool flag4 = a == LocalizationManager.Localize("txtWednesday");
						if (flag4)
						{
							dayOfWeek = DayOfWeek.Wednesday;
						}
						else
						{
							bool flag5 = a == LocalizationManager.Localize("txtThursday");
							if (flag5)
							{
								dayOfWeek = DayOfWeek.Thursday;
							}
							else
							{
								bool flag6 = a == LocalizationManager.Localize("txtFriday");
								if (flag6)
								{
									dayOfWeek = DayOfWeek.Friday;
								}
								else
								{
									bool flag7 = a == LocalizationManager.Localize("txtSaturday");
									if (flag7)
									{
										dayOfWeek = DayOfWeek.Saturday;
									}
									else
									{
										bool flag8 = a == LocalizationManager.Localize("txtSunday");
										if (flag8)
										{
											dayOfWeek = DayOfWeek.Sunday;
										}
									}
								}
							}
						}
					}
				}
			}
			return dayOfWeek;
		}
	}
}
