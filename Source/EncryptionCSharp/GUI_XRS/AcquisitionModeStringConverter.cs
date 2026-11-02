using System;
using System.Globalization;
using System.Windows.Data;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x02000202 RID: 514
	[ValueConversion(typeof(AcquisitionMode), typeof(string))]
	public class AcquisitionModeStringConverter : IValueConverter
	{
		// Token: 0x0600283D RID: 10301 RVA: 0x0010CC68 File Offset: 0x0010AE68
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string result = "";
			bool flag = value is AcquisitionMode;
			if (flag)
			{
				AcquisitionMode acquisitionMode = (AcquisitionMode)value;
				bool flag2 = acquisitionMode == AcquisitionMode.LineBased;
				if (flag2)
				{
					result = LocalizationManager.Localize("txtAcquisitionModeLineBased");
				}
				else
				{
					result = LocalizationManager.Localize("txtAcquisitionModeObjectBased");
				}
			}
			return result;
		}

		// Token: 0x0600283E RID: 10302 RVA: 0x0010CCBC File Offset: 0x0010AEBC
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			AcquisitionMode acquisitionMode = AcquisitionMode.LineBased;
			bool flag = value is string;
			if (flag)
			{
				string a = (string)value;
				bool flag2 = a == LocalizationManager.Localize("txtAcquisitionModeObjectBased");
				if (flag2)
				{
					acquisitionMode = AcquisitionMode.ObjectBased;
				}
			}
			return acquisitionMode;
		}
	}
}
