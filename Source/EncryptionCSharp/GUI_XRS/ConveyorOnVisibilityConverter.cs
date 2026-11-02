using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace GUI_XRS
{
	// Token: 0x020001F3 RID: 499
	[ValueConversion(typeof(MachineState), typeof(Visibility))]
	public class ConveyorOnVisibilityConverter : IValueConverter
	{
		// Token: 0x06002810 RID: 10256 RVA: 0x0010C330 File Offset: 0x0010A530
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			Visibility visibility = Visibility.Collapsed;
			bool flag = value is MachineStates;
			if (flag)
			{
				visibility = (((MachineStates)value != MachineStates.ConveyorOff) ? Visibility.Visible : Visibility.Collapsed);
			}
			return visibility;
		}

		// Token: 0x06002811 RID: 10257 RVA: 0x001091FB File Offset: 0x001073FB
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
