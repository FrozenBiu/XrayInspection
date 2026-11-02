using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x020001C3 RID: 451
	[ValueConversion(typeof(MessageType), typeof(Brush))]
	public class WorstMessageTypeToBrushConverter : IValueConverter
	{
		// Token: 0x06002780 RID: 10112 RVA: 0x0010AAFC File Offset: 0x00108CFC
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			Brush brush = null;
			bool flag = targetType != typeof(Brush);
			object result;
			if (flag)
			{
				result = null;
			}
			else
			{
				bool flag2 = value is MessageType;
				if (flag2)
				{
					MessageType messageType = (MessageType)value;
					MessageType messageType2 = messageType;
					if (messageType2 != MessageType.Error)
					{
						if (messageType2 != MessageType.Warning)
						{
							brush = new SolidColorBrush(Color.FromRgb(128, 128, 128));
						}
						else
						{
							brush = new SolidColorBrush(Color.FromRgb(0, 0, 0));
						}
					}
					else
					{
						brush = new SolidColorBrush(Color.FromRgb(byte.MaxValue, byte.MaxValue, byte.MaxValue));
					}
				}
				result = brush;
			}
			return result;
		}

		// Token: 0x06002781 RID: 10113 RVA: 0x001091FB File Offset: 0x001073FB
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
