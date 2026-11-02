using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace GUI_XRS
{
	// Token: 0x0200021B RID: 539
	[ValueConversion(typeof(SavingPngStorageModes), typeof(string[]))]
	public class SavingPngToNetworkStorageModesToStringConverter : IValueConverter
	{
		// Token: 0x06002888 RID: 10376 RVA: 0x0010E614 File Offset: 0x0010C814
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			List<string> list = new List<string>();
			bool flag = value is SavingPngStorageModes;
			if (flag)
			{
				using (List<SavingPngStorageMode>.Enumerator enumerator = ((SavingPngStorageModes)value).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						switch (enumerator.Current)
						{
						case SavingPngStorageMode.Disabled:
							list.Add(LocalizationManager.Localize("txtNotActive"));
							break;
						case SavingPngStorageMode.OperatingDays:
							list.Add(LocalizationManager.Localize("txtOperatingDays"));
							break;
						case SavingPngStorageMode.MaxCapacity:
							list.Add(LocalizationManager.Localize("txtMaxStorageCapacity"));
							break;
						}
					}
				}
			}
			return list;
		}

		// Token: 0x06002889 RID: 10377 RVA: 0x001091FB File Offset: 0x001073FB
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
