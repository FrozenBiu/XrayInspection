using System;
using System.Globalization;
using System.Windows.Data;

namespace GUI_XRS
{
	// Token: 0x02000212 RID: 530
	public class MachineStatesBoolConverterColorMessage : IMultiValueConverter
	{
		// Token: 0x0600286D RID: 10349 RVA: 0x0010DF20 File Offset: 0x0010C120
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
				MachineStates machineStates = (MachineStates)values[0];
				bool flag3 = machineStates == MachineStates.Inspecting && (bool)values[1] && (bool)values[2];
				if (flag3)
				{
					result = true;
				}
				else
				{
					result = false;
				}
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x0600286E RID: 10350 RVA: 0x001091FB File Offset: 0x001073FB
		public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
