using System;
using System.Globalization;
using System.Windows.Data;

namespace GUI_XRS
{
	// Token: 0x020001F7 RID: 503
	public class ProductChangeMachineStatesBoolConverter : IMultiValueConverter
	{
		// Token: 0x0600281C RID: 10268 RVA: 0x0010C58C File Offset: 0x0010A78C
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
				else
				{
					result = true;
				}
				return result;
			}
			throw new Exception("Invalid Arguments");
		}

		// Token: 0x0600281D RID: 10269 RVA: 0x001091FB File Offset: 0x001073FB
		public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
