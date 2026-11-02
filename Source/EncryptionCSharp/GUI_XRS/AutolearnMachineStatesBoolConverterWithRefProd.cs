using System;
using System.Globalization;
using System.Windows.Data;

namespace GUI_XRS
{
	// Token: 0x02000211 RID: 529
	public class AutolearnMachineStatesBoolConverterWithRefProd : IMultiValueConverter
	{
		// Token: 0x0600286A RID: 10346 RVA: 0x0010DE38 File Offset: 0x0010C038
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			bool flag = values[0] != null && values[1] != null && values[2] != null;
			object result;
			if (flag)
			{
				bool flag2 = values.Length == 3 && values[0] is MachineStates && values[1] is bool && values[2] is bool;
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
						bool flag5 = machineStates == MachineStates.Inspecting && (bool)values[2];
						if (flag5)
						{
							result = true;
						}
						else
						{
							result = false;
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

		// Token: 0x0600286B RID: 10347 RVA: 0x001091FB File Offset: 0x001073FB
		public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
