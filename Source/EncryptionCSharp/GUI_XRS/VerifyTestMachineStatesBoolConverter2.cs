using System;
using System.Globalization;
using System.Windows.Data;

namespace GUI_XRS
{
	// Token: 0x020001FB RID: 507
	public class VerifyTestMachineStatesBoolConverter2 : IMultiValueConverter
	{
		// Token: 0x06002828 RID: 10280 RVA: 0x0010C8B8 File Offset: 0x0010AAB8
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			bool flag = values.Length == 4 && values[0] is MachineStates && values[1] is bool && values[2] is bool && values[3] is bool;
			if (flag)
			{
				bool flag2 = (bool)values[2];
				object result;
				if (flag2)
				{
					bool flag3 = !(bool)values[1];
					if (flag3)
					{
						bool flag4 = !(bool)values[3];
						if (flag4)
						{
							result = false;
						}
						else
						{
							MachineStates machineStates = (MachineStates)values[0];
							bool flag5 = machineStates == MachineStates.ConveyorOff || machineStates == MachineStates.ConveyorOnKeyOpened || machineStates == MachineStates.ConveyorOnKeyClosedXrayOff || machineStates == MachineStates.Reseasoning || machineStates == MachineStates.XrayOnCalibrationNotDone;
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
				}
				else
				{
					result = false;
				}
				return result;
			}
			throw new Exception("Invalid Arguments");
		}

		// Token: 0x06002829 RID: 10281 RVA: 0x001091FB File Offset: 0x001073FB
		public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
