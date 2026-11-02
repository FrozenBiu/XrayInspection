using System;
using System.Globalization;
using System.Windows.Data;

namespace GUI_XRS
{
	// Token: 0x020001F9 RID: 505
	public class SubmenuMachineStatesBoolConverter : IMultiValueConverter
	{
		// Token: 0x06002822 RID: 10274 RVA: 0x0010C704 File Offset: 0x0010A904
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			bool flag = values.Length == 3 && values[0] is string && values[1] is MachineStates && values[2] is bool;
			if (flag)
			{
				bool flag2 = !(bool)values[2];
				object result;
				if (flag2)
				{
					string text = (string)values[0];
					string a = text;
					if (!(a == "menuItemPlot") && !(a == "menuItemBackup") && !(a == "menuItemDual") && !(a == "menuItemClose") && !(a == "menuItemRestart") && !(a == "menuItemComissioning"))
					{
						result = true;
					}
					else
					{
						MachineStates machineStates = (MachineStates)values[1];
						bool flag3 = machineStates == MachineStates.XrayOnCalibrationNotDone || machineStates == MachineStates.Reseasoning;
						if (flag3)
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

		// Token: 0x06002823 RID: 10275 RVA: 0x001091FB File Offset: 0x001073FB
		public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
