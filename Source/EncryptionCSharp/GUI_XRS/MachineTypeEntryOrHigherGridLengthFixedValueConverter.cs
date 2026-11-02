using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x020001FD RID: 509
	public class MachineTypeEntryOrHigherGridLengthFixedValueConverter : IMultiValueConverter
	{
		// Token: 0x0600282E RID: 10286 RVA: 0x0010CA50 File Offset: 0x0010AC50
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			MachineType machineType = MachineType.MachineTypeUndefined;
			bool flag = values[0] is MachineType;
			if (flag)
			{
				machineType = (MachineType)values[0];
			}
			bool flag2 = machineType != MachineType.RayconXRS && machineType != MachineType.RayconD;
			if (flag2)
			{
				bool flag3 = values[1] is double;
				if (flag3)
				{
					double pixels = (double)values[1];
					return new GridLength(pixels);
				}
			}
			return new GridLength(0.0);
		}

		// Token: 0x0600282F RID: 10287 RVA: 0x001091FB File Offset: 0x001073FB
		public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
