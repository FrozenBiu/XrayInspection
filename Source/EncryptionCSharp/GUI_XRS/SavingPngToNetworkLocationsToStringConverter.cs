using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace GUI_XRS
{
	// Token: 0x02000218 RID: 536
	[ValueConversion(typeof(SavingPngLocations), typeof(string[]))]
	public class SavingPngToNetworkLocationsToStringConverter : IValueConverter
	{
		// Token: 0x0600287F RID: 10367 RVA: 0x0010E32C File Offset: 0x0010C52C
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			List<string> list = new List<string>();
			bool flag = value is SavingPngLocations;
			if (flag)
			{
				using (List<SavingPngLocation>.Enumerator enumerator = ((SavingPngLocations)value).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						switch (enumerator.Current)
						{
						case SavingPngLocation.Disabled:
							list.Add(LocalizationManager.Localize("txtNotActive"));
							break;
						case SavingPngLocation.NetworkFolder:
							list.Add(LocalizationManager.Localize("txtExternalDrive"));
							break;
						case SavingPngLocation.LocalFolderFTP:
							list.Add(LocalizationManager.Localize("txtFTP"));
							break;
						case SavingPngLocation.NetworkAndLocalFolderFTP:
							list.Add(LocalizationManager.Localize("txtExternalDriveAndFTP"));
							break;
						}
					}
				}
			}
			return list;
		}

		// Token: 0x06002880 RID: 10368 RVA: 0x001091FB File Offset: 0x001073FB
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
