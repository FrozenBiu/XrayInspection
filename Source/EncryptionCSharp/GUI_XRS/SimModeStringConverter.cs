using System;
using System.Globalization;
using System.Windows.Data;

namespace GUI_XRS
{
	// Token: 0x02000206 RID: 518
	[ValueConversion(typeof(SimMode), typeof(string[]))]
	public class SimModeStringConverter : IValueConverter
	{
		// Token: 0x06002849 RID: 10313 RVA: 0x0010CEF0 File Offset: 0x0010B0F0
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string result = "";
			bool flag = value is SimMode;
			if (flag)
			{
				switch ((SimMode)value)
				{
				case SimMode.AllCards:
					result = LocalizationManager.Localize("txtAllTestcards");
					break;
				case SimMode.UpTo100:
					result = LocalizationManager.Localize("txtUptToHundered");
					break;
				case SimMode.SingleCard:
					result = LocalizationManager.Localize("txtSingleTestcard");
					break;
				case SimMode.Autotrain:
					result = LocalizationManager.Localize("txtAutotrain");
					break;
				}
			}
			return result;
		}

		// Token: 0x0600284A RID: 10314 RVA: 0x0010CF74 File Offset: 0x0010B174
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			SimMode simMode = SimMode.AllCards;
			bool flag = value is string;
			if (flag)
			{
				string a = (string)value;
				bool flag2 = a == LocalizationManager.Localize("txtAllTestcards");
				if (flag2)
				{
					simMode = SimMode.AllCards;
				}
				else
				{
					bool flag3 = a == LocalizationManager.Localize("txtUptToHundered");
					if (flag3)
					{
						simMode = SimMode.UpTo100;
					}
					else
					{
						bool flag4 = a == LocalizationManager.Localize("txtSingleTestcard");
						if (flag4)
						{
							simMode = SimMode.SingleCard;
						}
						else
						{
							bool flag5 = a == LocalizationManager.Localize("txtAutotrain");
							if (flag5)
							{
								simMode = SimMode.Autotrain;
							}
						}
					}
				}
			}
			return simMode;
		}
	}
}
