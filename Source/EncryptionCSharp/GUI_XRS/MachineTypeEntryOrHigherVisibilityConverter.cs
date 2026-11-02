using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x020001F1 RID: 497
	[ValueConversion(typeof(MachineType), typeof(Visibility))]
	public class MachineTypeEntryOrHigherVisibilityConverter : IValueConverter
	{
		// Token: 0x0600280A RID: 10250 RVA: 0x0010C2B8 File Offset: 0x0010A4B8
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			Visibility visibility = Visibility.Collapsed;
			bool flag = value is MachineType;
			if (flag)
			{
				MachineType machineType = (MachineType)value;
				visibility = ((machineType != MachineType.RayconXRS && machineType != MachineType.RayconD) ? Visibility.Visible : Visibility.Collapsed);
			}
			return visibility;
		}

		// Token: 0x0600280B RID: 10251 RVA: 0x001091FB File Offset: 0x001073FB
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
