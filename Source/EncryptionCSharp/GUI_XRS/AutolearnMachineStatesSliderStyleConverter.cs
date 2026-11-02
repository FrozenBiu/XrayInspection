using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace GUI_XRS
{
	// Token: 0x020001F6 RID: 502
	public class AutolearnMachineStatesSliderStyleConverter : IMultiValueConverter
	{
		// Token: 0x06002819 RID: 10265 RVA: 0x0010C4C4 File Offset: 0x0010A6C4
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			FrameworkElement frameworkElement = values[0] as FrameworkElement;
			bool flag = values.Length == 3 && values[1] is MachineStates && values[2] is bool;
			if (flag)
			{
				bool flag2 = !(bool)values[2];
				Style result;
				if (flag2)
				{
					MachineStates machineStates = (MachineStates)values[1];
					bool flag3 = machineStates == MachineStates.ConveyorOff || machineStates == MachineStates.ConveyorOnKeyOpened || machineStates == MachineStates.ConveyorOnKeyClosedXrayOff || machineStates == MachineStates.Reseasoning || machineStates == MachineStates.XrayOnCalibrationNotDone || machineStates == MachineStates.NoConnectToSourceOrDetector;
					if (flag3)
					{
						result = (Style)frameworkElement.TryFindResource("deactivatedSliderStyle");
					}
					else
					{
						result = (Style)frameworkElement.TryFindResource("activatedSliderStyle");
					}
				}
				else
				{
					result = (Style)frameworkElement.TryFindResource("activatedSliderStyle");
				}
				return result;
			}
			throw new Exception("Invalid Arguments");
		}

		// Token: 0x0600281A RID: 10266 RVA: 0x001091FB File Offset: 0x001073FB
		public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
