using System;
using System.Globalization;
using System.Windows.Data;

namespace GUI_XRS
{
	// Token: 0x020001F4 RID: 500
	public class AutolearnMachineStatesBoolConverter : IMultiValueConverter
	{
		// Token: 0x06002813 RID: 10259 RVA: 0x0010C368 File Offset: 0x0010A568
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			bool flag = values.Length == 2 && values[0] is MachineStates && values[1] is bool;
			if (flag)
			{
				bool flag2 = !(bool)values[1];
				object result;
				if (flag2)
				{
					MachineStates machineStates = (MachineStates)values[0];
					bool flag3 = machineStates == MachineStates.ConveyorOff || machineStates == MachineStates.ConveyorOnKeyOpened || machineStates == MachineStates.ConveyorOnKeyClosedXrayOff || machineStates == MachineStates.Reseasoning || machineStates == MachineStates.XrayOnCalibrationNotDone || machineStates == MachineStates.NoConnectToSourceOrDetector;
					if (flag3)
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
				return result;
			}
			throw new Exception("Invalid Arguments");
		}

		// Token: 0x06002814 RID: 10260 RVA: 0x001091FB File Offset: 0x001073FB
		public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
