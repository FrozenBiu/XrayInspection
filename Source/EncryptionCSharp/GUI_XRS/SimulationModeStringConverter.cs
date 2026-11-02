using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace GUI_XRS
{
	// Token: 0x02000205 RID: 517
	[ValueConversion(typeof(SimulationModes), typeof(string[]))]
	public class SimulationModeStringConverter : IValueConverter
	{
		// Token: 0x06002846 RID: 10310 RVA: 0x0010CE14 File Offset: 0x0010B014
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			List<string> list = new List<string>();
			bool flag = value is SimulationModes;
			if (flag)
			{
				using (List<SimMode>.Enumerator enumerator = ((SimulationModes)value).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						switch (enumerator.Current)
						{
						case SimMode.AllCards:
							list.Add(LocalizationManager.Localize("txtAllTestcards"));
							break;
						case SimMode.UpTo100:
							list.Add(LocalizationManager.Localize("txtUptToHundered"));
							break;
						case SimMode.SingleCard:
							list.Add(LocalizationManager.Localize("txtSingleTestcard"));
							break;
						case SimMode.Autotrain:
							list.Add(LocalizationManager.Localize("txtAutotrain"));
							break;
						}
					}
				}
			}
			return list;
		}

		// Token: 0x06002847 RID: 10311 RVA: 0x001091FB File Offset: 0x001073FB
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
