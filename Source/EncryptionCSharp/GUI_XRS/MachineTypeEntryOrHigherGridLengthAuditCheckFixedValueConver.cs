using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x020001FE RID: 510
	public class MachineTypeEntryOrHigherGridLengthAuditCheckFixedValueConverter : IMultiValueConverter
	{
		// Token: 0x06002831 RID: 10289 RVA: 0x0010CAD8 File Offset: 0x0010ACD8
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			MachineType machineType = MachineType.MachineTypeUndefined;
			bool flag = true;
			bool flag2 = values[0] is MachineType;
			if (flag2)
			{
				machineType = (MachineType)values[0];
			}
			bool flag3 = values[2] is bool;
			if (flag3)
			{
				flag = (bool)values[2];
			}
			bool flag4 = machineType != MachineType.RayconXRS && machineType != MachineType.RayconD && flag;
			if (flag4)
			{
				bool flag5 = values[1] is double;
				if (flag5)
				{
					double pixels = (double)values[1];
					return new GridLength(pixels);
				}
			}
			return new GridLength(0.0);
		}

		// Token: 0x06002832 RID: 10290 RVA: 0x001091FB File Offset: 0x001073FB
		public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
