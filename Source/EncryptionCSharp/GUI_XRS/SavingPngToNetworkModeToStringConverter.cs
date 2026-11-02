using System;
using System.Globalization;
using System.Windows.Data;

namespace GUI_XRS
{
	// Token: 0x02000217 RID: 535
	[ValueConversion(typeof(SavingPngMode), typeof(string))]
	public class SavingPngToNetworkModeToStringConverter : IValueConverter
	{
		// Token: 0x0600287C RID: 10364 RVA: 0x0010E23C File Offset: 0x0010C43C
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string result = "";
			bool flag = value is SavingPngMode;
			if (flag)
			{
				switch ((SavingPngMode)value)
				{
				case SavingPngMode.BadImages:
					result = LocalizationManager.Localize("txtBadImages");
					break;
				case SavingPngMode.GoodImages:
					result = LocalizationManager.Localize("txtGoodImages");
					break;
				case SavingPngMode.GoodandBadImages:
					result = LocalizationManager.Localize("txtGoodAndBadImages");
					break;
				}
			}
			return result;
		}

		// Token: 0x0600287D RID: 10365 RVA: 0x0010E2B0 File Offset: 0x0010C4B0
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			SavingPngMode savingPngMode = SavingPngMode.BadImages;
			bool flag = value is string;
			if (flag)
			{
				string a = (string)value;
				bool flag2 = a == LocalizationManager.Localize("txtBadImages");
				if (flag2)
				{
					savingPngMode = SavingPngMode.BadImages;
				}
				else
				{
					bool flag3 = a == LocalizationManager.Localize("txtGoodImages");
					if (flag3)
					{
						savingPngMode = SavingPngMode.GoodImages;
					}
					else
					{
						bool flag4 = a == LocalizationManager.Localize("txtGoodAndBadImages");
						if (flag4)
						{
							savingPngMode = SavingPngMode.GoodandBadImages;
						}
					}
				}
			}
			return savingPngMode;
		}
	}
}
