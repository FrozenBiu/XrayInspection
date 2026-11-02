using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x020001C2 RID: 450
	[ValueConversion(typeof(MessageType), typeof(Style))]
	public class MessageTypeToButtonStyleConverter : IMultiValueConverter
	{
		// Token: 0x0600277D RID: 10109 RVA: 0x0010AA48 File Offset: 0x00108C48
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			FrameworkElement frameworkElement = values[0] as FrameworkElement;
			bool flag = targetType != typeof(Style) || frameworkElement == null || values.Length < 2;
			object result;
			if (flag)
			{
				result = null;
			}
			else
			{
				Style style = (Style)frameworkElement.TryFindResource("infoButton");
				bool flag2 = values[1] is MessageType && (MessageType)values[1] == MessageType.Error;
				if (flag2)
				{
					style = (Style)frameworkElement.TryFindResource("errorButton");
				}
				bool flag3 = values[1] is MessageType && (MessageType)values[1] == MessageType.Warning;
				if (flag3)
				{
					style = (Style)frameworkElement.TryFindResource("warningButton");
				}
				result = style;
			}
			return result;
		}

		// Token: 0x0600277E RID: 10110 RVA: 0x001091FB File Offset: 0x001073FB
		public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
