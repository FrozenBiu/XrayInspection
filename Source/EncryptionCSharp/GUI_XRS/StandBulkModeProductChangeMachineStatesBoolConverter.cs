using System;
using System.Globalization;
using System.Windows.Data;

namespace GUI_XRS
{
	// Token: 0x020001F8 RID: 504
	public class StandBulkModeProductChangeMachineStatesBoolConverter : IMultiValueConverter
	{
		// Token: 0x0600281F RID: 10271 RVA: 0x0010C610 File Offset: 0x0010A810
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			bool flag = values.Length == 4 && values[0] is MachineStates && values[1] is bool && values[2] is bool && values[3] is bool;
			if (flag)
			{
				bool flag2 = !(bool)values[1];
				object result;
				if (flag2)
				{
					bool flag3 = (bool)values[2] && !(bool)values[3];
					if (flag3)
					{
						MachineStates machineStates = (MachineStates)values[0];
						bool flag4 = machineStates == MachineStates.XrayOnCalibrationNotDone || machineStates == MachineStates.Reseasoning;
						if (flag4)
						{
							result = false;
						}
						else
						{
							result = true;
						}
					}
					else
					{
						MachineStates machineStates2 = (MachineStates)values[0];
						bool flag5 = machineStates2 == MachineStates.XrayOnCalibrationNotDone || machineStates2 == MachineStates.Reseasoning || machineStates2 == MachineStates.Inspecting;
						if (flag5)
						{
							result = false;
						}
						else
						{
							result = true;
						}
					}
				}
				else
				{
					result = true;
				}
				return result;
			}
			throw new Exception("Invalid Arguments");
		}

		// Token: 0x06002820 RID: 10272 RVA: 0x001091FB File Offset: 0x001073FB
		public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
