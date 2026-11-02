using System;
using System.Globalization;
using System.Windows.Data;

namespace GUI_XRS
{
	// Token: 0x02000219 RID: 537
	[ValueConversion(typeof(SavingPngLocation), typeof(string))]
	public class SavingPngToNetworkLocationToStringConverter : IValueConverter
	{
		// Token: 0x06002882 RID: 10370 RVA: 0x0010E408 File Offset: 0x0010C608
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string result = "";
			bool flag = value is SavingPngLocation;
			if (flag)
			{
				switch ((SavingPngLocation)value)
				{
				case SavingPngLocation.Disabled:
					result = LocalizationManager.Localize("txtNotActive");
					break;
				case SavingPngLocation.NetworkFolder:
					result = LocalizationManager.Localize("txtExternalDrive");
					break;
				case SavingPngLocation.LocalFolderFTP:
					result = LocalizationManager.Localize("txtFTP");
					break;
				case SavingPngLocation.NetworkAndLocalFolderFTP:
					result = LocalizationManager.Localize("txtExternalDriveAndFTP");
					break;
				}
			}
			return result;
		}

		// Token: 0x06002883 RID: 10371 RVA: 0x0010E48C File Offset: 0x0010C68C
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			SavingPngLocation savingPngLocation = SavingPngLocation.Disabled;
			bool flag = value is string;
			if (flag)
			{
				string a = (string)value;
				bool flag2 = a == LocalizationManager.Localize("txtNotActive");
				if (flag2)
				{
					savingPngLocation = SavingPngLocation.Disabled;
				}
				else
				{
					bool flag3 = a == LocalizationManager.Localize("txtExternalDrive");
					if (flag3)
					{
						savingPngLocation = SavingPngLocation.NetworkFolder;
					}
					else
					{
						bool flag4 = a == LocalizationManager.Localize("txtFTP");
						if (flag4)
						{
							savingPngLocation = SavingPngLocation.LocalFolderFTP;
						}
						else
						{
							bool flag5 = a == LocalizationManager.Localize("txtExternalDriveAndFTP");
							if (flag5)
							{
								savingPngLocation = SavingPngLocation.NetworkAndLocalFolderFTP;
							}
						}
					}
				}
			}
			return savingPngLocation;
		}
	}
}
