using System;
using System.Globalization;
using System.Windows.Data;

namespace GUI_XRS
{
	// Token: 0x020001FA RID: 506
	public class VerifyTestMachineStatesBoolConverter : IMultiValueConverter
	{
		// Token: 0x06002825 RID: 10277 RVA: 0x0010C804 File Offset: 0x0010AA04
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			bool flag = values.Length == 3 && values[0] is MachineStates && values[1] is bool && values[2] is bool;
			if (flag)
			{
				bool flag2 = (bool)values[2];
				object result;
				if (flag2)
				{
					bool flag3 = !(bool)values[1];
					if (flag3)
					{
						MachineStates machineStates = (MachineStates)values[0];
						bool flag4 = machineStates == MachineStates.ConveyorOff || machineStates == MachineStates.ConveyorOnKeyOpened || machineStates == MachineStates.ConveyorOnKeyClosedXrayOff || machineStates == MachineStates.Reseasoning || machineStates == MachineStates.XrayOnCalibrationNotDone;
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
			throw new Exception("Invalid Arguments");
		}

		// Token: 0x06002826 RID: 10278 RVA: 0x001091FB File Offset: 0x001073FB
		public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
