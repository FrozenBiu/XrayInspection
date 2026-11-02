using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x020001BE RID: 446
	[ValueConversion(typeof(List<Lane>), typeof(string[]))]
	public class LanesStringConverter : IValueConverter
	{
		// Token: 0x06002771 RID: 10097 RVA: 0x0010A7C8 File Offset: 0x001089C8
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			List<string> list = new List<string>();
			bool flag = value is List<Lane>;
			if (flag)
			{
				using (List<Lane>.Enumerator enumerator = ((List<Lane>)value).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						switch (enumerator.Current)
						{
						case Lane.All:
							list.Add(LocalizationManager.Localize("txtAll"));
							break;
						case Lane.Lane1:
							list.Add(LocalizationManager.Localize("txtLane") + " 1");
							break;
						case Lane.Lane2:
							list.Add(LocalizationManager.Localize("txtLane") + " 2");
							break;
						case Lane.Lane3:
							list.Add(LocalizationManager.Localize("txtLane") + " 3");
							break;
						case Lane.Lane4:
							list.Add(LocalizationManager.Localize("txtLane") + " 4");
							break;
						}
					}
				}
			}
			return list;
		}

		// Token: 0x06002772 RID: 10098 RVA: 0x001091FB File Offset: 0x001073FB
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
