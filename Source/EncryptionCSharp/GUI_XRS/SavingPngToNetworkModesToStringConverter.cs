using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace GUI_XRS
{
	// Token: 0x02000216 RID: 534
	[ValueConversion(typeof(SavingPngToNetworkModes), typeof(string[]))]
	public class SavingPngToNetworkModesToStringConverter : IValueConverter
	{
		// Token: 0x06002879 RID: 10361 RVA: 0x0010E174 File Offset: 0x0010C374
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			List<string> list = new List<string>();
			bool flag = value is SavingPngToNetworkModes;
			if (flag)
			{
				using (List<SavingPngMode>.Enumerator enumerator = ((SavingPngToNetworkModes)value).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						switch (enumerator.Current)
						{
						case SavingPngMode.BadImages:
							list.Add(LocalizationManager.Localize("txtBadImages"));
							break;
						case SavingPngMode.GoodImages:
							list.Add(LocalizationManager.Localize("txtGoodImages"));
							break;
						case SavingPngMode.GoodandBadImages:
							list.Add(LocalizationManager.Localize("txtGoodAndBadImages"));
							break;
						}
					}
				}
			}
			return list;
		}

		// Token: 0x0600287A RID: 10362 RVA: 0x001091FB File Offset: 0x001073FB
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
