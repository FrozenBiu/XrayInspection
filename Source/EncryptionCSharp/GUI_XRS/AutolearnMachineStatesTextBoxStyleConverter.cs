using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace GUI_XRS
{
	// Token: 0x020001F5 RID: 501
	public class AutolearnMachineStatesTextBoxStyleConverter : IMultiValueConverter
	{
		// Token: 0x06002816 RID: 10262 RVA: 0x0010C3FC File Offset: 0x0010A5FC
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
						result = (Style)frameworkElement.TryFindResource("disabledTextBox");
					}
					else
					{
						result = (Style)frameworkElement.TryFindResource("TextBoxStyle");
					}
				}
				else
				{
					result = (Style)frameworkElement.TryFindResource("TextBoxStyle");
				}
				return result;
			}
			throw new Exception("Invalid Arguments");
		}

		// Token: 0x06002817 RID: 10263 RVA: 0x001091FB File Offset: 0x001073FB
		public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
