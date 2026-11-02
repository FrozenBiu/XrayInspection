using System;
using System.Globalization;
using System.Windows.Data;

namespace GUI_XRS
{
	// Token: 0x0200020F RID: 527
	public class AutolearnMachineStatesBoolConverter2 : IMultiValueConverter
	{
		// Token: 0x06002864 RID: 10340 RVA: 0x0010DC4C File Offset: 0x0010BE4C
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			bool flag = values[0] != null && values[1] != null;
			object result;
			if (flag)
			{
				bool flag2 = values.Length == 2 && values[0] is MachineStates && values[1] is bool;
				if (!flag2)
				{
					throw new Exception("Invalid Arguments");
				}
				bool flag3 = !(bool)values[1];
				if (flag3)
				{
					MachineStates machineStates = (MachineStates)values[0];
					bool flag4 = machineStates == MachineStates.ConveyorOff || machineStates == MachineStates.ConveyorOnKeyOpened || machineStates == MachineStates.ConveyorOnKeyClosedXrayOff || machineStates == MachineStates.Reseasoning || machineStates == MachineStates.XrayOnCalibrationNotDone || machineStates == MachineStates.NoConnectToSourceOrDetector;
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
					result = true;
				}
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06002865 RID: 10341 RVA: 0x001091FB File Offset: 0x001073FB
		public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
