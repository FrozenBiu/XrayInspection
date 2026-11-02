using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace GUI_XRS
{
	// Token: 0x020001A7 RID: 423
	[ValueConversion(typeof(bool), typeof(Visibility))]
	public class MachineStatesToVisibilityConverter : IValueConverter
	{
		// Token: 0x0600272C RID: 10028 RVA: 0x00109550 File Offset: 0x00107750
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			bool flag = targetType != typeof(Visibility);
			object result;
			if (flag)
			{
				result = null;
			}
			else
			{
				bool flag2 = value is MachineStates;
				if (flag2)
				{
					MachineStates machineStates = (MachineStates)value;
					result = ((machineStates == MachineStates.Inspecting || machineStates == MachineStates.ProductChange || machineStates == MachineStates.Autotrain || machineStates == MachineStates.AutoAdjustingXRay) ? Visibility.Collapsed : Visibility.Visible);
				}
				else
				{
					result = null;
				}
			}
			return result;
		}

		// Token: 0x0600272D RID: 10029 RVA: 0x001091FB File Offset: 0x001073FB
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
