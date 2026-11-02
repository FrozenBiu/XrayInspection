using System;
using System.Globalization;
using System.Windows.Data;

namespace GUI_XRS
{
	// Token: 0x0200021A RID: 538
	[ValueConversion(typeof(SavingPngStorageMode), typeof(string))]
	public class SavingPngToNetworkStorageModeToStringConverter : IValueConverter
	{
		// Token: 0x06002885 RID: 10373 RVA: 0x0010E524 File Offset: 0x0010C724
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string result = "";
			bool flag = value is SavingPngStorageMode;
			if (flag)
			{
				switch ((SavingPngStorageMode)value)
				{
				case SavingPngStorageMode.Disabled:
					result = LocalizationManager.Localize("txtNotActive");
					break;
				case SavingPngStorageMode.OperatingDays:
					result = LocalizationManager.Localize("txtOperatingDays");
					break;
				case SavingPngStorageMode.MaxCapacity:
					result = LocalizationManager.Localize("txtMaxStorageCapacity");
					break;
				}
			}
			return result;
		}

		// Token: 0x06002886 RID: 10374 RVA: 0x0010E598 File Offset: 0x0010C798
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			SavingPngStorageMode savingPngStorageMode = SavingPngStorageMode.MaxCapacity;
			bool flag = value is string;
			if (flag)
			{
				string a = (string)value;
				bool flag2 = a == LocalizationManager.Localize("txtNotActive");
				if (flag2)
				{
					savingPngStorageMode = SavingPngStorageMode.Disabled;
				}
				else
				{
					bool flag3 = a == LocalizationManager.Localize("txtOperatingDays");
					if (flag3)
					{
						savingPngStorageMode = SavingPngStorageMode.OperatingDays;
					}
					else
					{
						bool flag4 = a == LocalizationManager.Localize("txtMaxStorageCapacity");
						if (flag4)
						{
							savingPngStorageMode = SavingPngStorageMode.MaxCapacity;
						}
					}
				}
			}
			return savingPngStorageMode;
		}
	}
}
