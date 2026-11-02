using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace GUI_XRS
{
	// Token: 0x020001F2 RID: 498
	[ValueConversion(typeof(MachineState), typeof(Visibility))]
	public class ConveyorOffVisibilityConverter : IValueConverter
	{
		// Token: 0x0600280D RID: 10253 RVA: 0x0010C2F8 File Offset: 0x0010A4F8
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			Visibility visibility = Visibility.Collapsed;
			bool flag = value is MachineStates;
			if (flag)
			{
				visibility = (((MachineStates)value == MachineStates.ConveyorOff) ? Visibility.Visible : Visibility.Collapsed);
			}
			return visibility;
		}

		// Token: 0x0600280E RID: 10254 RVA: 0x001091FB File Offset: 0x001073FB
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
