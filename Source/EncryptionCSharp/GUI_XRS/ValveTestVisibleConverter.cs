using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x020001A0 RID: 416
	[ValueConversion(typeof(bool), typeof(Visibility))]
	public class ValveTestVisibleConverter : IMultiValueConverter
	{
		// Token: 0x06002717 RID: 10007 RVA: 0x0010928C File Offset: 0x0010748C
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			bool flag = values[0] is bool && values[1] is bool && values[2] is MachineMode;
			if (flag)
			{
				bool flag2 = targetType != typeof(Visibility);
				if (flag2)
				{
					return null;
				}
				bool flag3 = bool.Parse(values[0].ToString());
				bool flag4 = bool.Parse(values[1].ToString());
				MachineMode machineMode = (MachineMode)values[2];
				bool flag5 = machineMode == MachineMode.Bulk;
				if (flag5)
				{
					bool flag6 = !flag3;
					if (flag6)
					{
						return Visibility.Visible;
					}
					return Visibility.Collapsed;
				}
				else
				{
					bool flag7 = machineMode == MachineMode.BulkObject;
					if (flag7)
					{
						bool flag8 = !flag4;
						if (flag8)
						{
							return Visibility.Visible;
						}
						return Visibility.Collapsed;
					}
				}
			}
			return Visibility.Collapsed;
		}

		// Token: 0x06002718 RID: 10008 RVA: 0x001091FB File Offset: 0x001073FB
		public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
