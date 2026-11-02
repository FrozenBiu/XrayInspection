using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x020001FC RID: 508
	public class MachineStateXrayStarterVisibilityConverter : IMultiValueConverter
	{
		// Token: 0x0600282B RID: 10283 RVA: 0x0010C994 File Offset: 0x0010AB94
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			Visibility visibility = Visibility.Collapsed;
			bool flag = values.Length == 4 && values[0] is MachineStates && values[1] is IList<Message> && values[2] is bool && values[3] is BuiltInPriviledges;
			if (flag)
			{
				MachineStates machineStates = (MachineStates)values[0];
				IList<Message> source = (IList<Message>)values[1];
				bool flag2 = (bool)values[2];
				BuiltInPriviledges builtInPriviledges = (BuiltInPriviledges)values[3];
				int num = (from message in source
				where message.LogbookEntry.EntryCode == SSTEntryCode.SafetySwitchOpenInfo
				select message).Count<Message>();
				visibility = ((machineStates == MachineStates.ConveyorOnKeyClosedXrayOff && num == 0 && (flag2 || builtInPriviledges != BuiltInPriviledges.NoUser)) ? Visibility.Visible : Visibility.Collapsed);
			}
			return visibility;
		}

		// Token: 0x0600282C RID: 10284 RVA: 0x001091FB File Offset: 0x001073FB
		public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
