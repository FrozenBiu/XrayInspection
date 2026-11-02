using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace GUI_XRS
{
	// Token: 0x02000210 RID: 528
	[ValueConversion(typeof(bool), typeof(Visibility))]
	public class MachineStatesToVisibilityConverterWithBulk : IMultiValueConverter
	{
		// Token: 0x06002867 RID: 10343 RVA: 0x0010DD04 File Offset: 0x0010BF04
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			bool flag = targetType != typeof(Visibility);
			object result;
			if (flag)
			{
				result = null;
			}
			else
			{
				bool flag2 = values[0] != null && values[1] != null && values[2] != null && values[3] != null;
				if (flag2)
				{
					bool flag3 = values.Length == 4 && values[0] is MachineStates && values[1] is bool && values[2] is bool && values[2] is bool;
					if (flag3)
					{
						bool flag4 = !(bool)values[1];
						if (flag4)
						{
							MachineStates machineStates = (MachineStates)values[0];
							result = ((machineStates == MachineStates.Inspecting || machineStates == MachineStates.ProductChange || machineStates == MachineStates.Autotrain || machineStates == MachineStates.AutoAdjustingXRay) ? Visibility.Collapsed : Visibility.Visible);
						}
						else
						{
							bool flag5 = (MachineStates)values[0] == MachineStates.Inspecting;
							if (flag5)
							{
								bool flag6 = !(bool)values[2] && !(bool)values[3];
								if (flag6)
								{
									result = Visibility.Visible;
								}
								else
								{
									result = Visibility.Collapsed;
								}
							}
							else
							{
								MachineStates machineStates2 = (MachineStates)values[0];
								result = ((machineStates2 == MachineStates.ProductChange || machineStates2 == MachineStates.Autotrain || machineStates2 == MachineStates.AutoAdjustingXRay) ? Visibility.Collapsed : Visibility.Visible);
							}
						}
					}
					else
					{
						result = null;
					}
				}
				else
				{
					result = null;
				}
			}
			return result;
		}

		// Token: 0x06002868 RID: 10344 RVA: 0x001091FB File Offset: 0x001073FB
		public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
