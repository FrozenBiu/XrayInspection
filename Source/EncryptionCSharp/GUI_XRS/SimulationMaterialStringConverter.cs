using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace GUI_XRS
{
	// Token: 0x02000207 RID: 519
	[ValueConversion(typeof(List<SimMaterial>), typeof(string[]))]
	public class SimulationMaterialStringConverter : IValueConverter
	{
		// Token: 0x0600284C RID: 10316 RVA: 0x0010D00C File Offset: 0x0010B20C
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			List<string> list = new List<string>();
			bool flag = value is List<SimMaterial>;
			if (flag)
			{
				using (List<SimMaterial>.Enumerator enumerator = ((List<SimMaterial>)value).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						switch (enumerator.Current)
						{
						case SimMaterial.VAandGlass:
							list.Add(LocalizationManager.Localize("txtVAGlass"));
							break;
						case SimMaterial.VA:
							list.Add(LocalizationManager.Localize("txtVA"));
							break;
						case SimMaterial.Glass:
							list.Add(LocalizationManager.Localize("txtGlass"));
							break;
						case SimMaterial.OtherMaterial:
							list.Add(LocalizationManager.Localize("txtOtherMaterial"));
							break;
						}
					}
				}
			}
			return list;
		}

		// Token: 0x0600284D RID: 10317 RVA: 0x001091FB File Offset: 0x001073FB
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
