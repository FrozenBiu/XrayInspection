using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace GUI_XRS
{
	// Token: 0x020001EF RID: 495
	[ValueConversion(typeof(ImpurityFinderTypes), typeof(string[]))]
	public class ImpurityFinderTypesStringConverter : IValueConverter
	{
		// Token: 0x06002804 RID: 10244 RVA: 0x0010C19C File Offset: 0x0010A39C
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			List<string> list = new List<string>();
			bool flag = value is ImpurityFinderTypes;
			if (flag)
			{
				foreach (ImpurityFinderType impurityFinderType in ((ImpurityFinderTypes)value))
				{
					ImpurityFinderType impurityFinderType2 = impurityFinderType;
					ImpurityFinderType impurityFinderType3 = impurityFinderType2;
					if (impurityFinderType3 != ImpurityFinderType.Metal)
					{
						if (impurityFinderType3 == ImpurityFinderType.Glass)
						{
							list.Add(LocalizationManager.Localize("txtGlass"));
						}
					}
					else
					{
						list.Add(LocalizationManager.Localize("txtMetal"));
					}
				}
			}
			return list;
		}

		// Token: 0x06002805 RID: 10245 RVA: 0x001091FB File Offset: 0x001073FB
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
