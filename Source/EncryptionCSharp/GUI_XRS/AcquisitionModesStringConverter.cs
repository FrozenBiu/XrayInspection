using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x02000203 RID: 515
	[ValueConversion(typeof(List<AcquisitionMode>), typeof(string[]))]
	public class AcquisitionModesStringConverter : IValueConverter
	{
		// Token: 0x06002840 RID: 10304 RVA: 0x0010CD04 File Offset: 0x0010AF04
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			List<string> list = new List<string>();
			bool flag = value is List<AcquisitionMode>;
			if (flag)
			{
				foreach (AcquisitionMode acquisitionMode in ((List<AcquisitionMode>)value))
				{
					bool flag2 = acquisitionMode == AcquisitionMode.LineBased;
					if (flag2)
					{
						list.Add(LocalizationManager.Localize("txtAcquisitionModeLineBased"));
					}
					else
					{
						list.Add(LocalizationManager.Localize("txtAcquisitionModeObjectBased"));
					}
				}
			}
			return list;
		}

		// Token: 0x06002841 RID: 10305 RVA: 0x001091FB File Offset: 0x001073FB
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
