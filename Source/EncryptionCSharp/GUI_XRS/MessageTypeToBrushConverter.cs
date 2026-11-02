using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x020001B6 RID: 438
	[ValueConversion(typeof(MessageType), typeof(Brush))]
	public class MessageTypeToBrushConverter : IValueConverter
	{
		// Token: 0x06002759 RID: 10073 RVA: 0x0010A124 File Offset: 0x00108324
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			Brush brush = null;
			LinearGradientBrush linearGradientBrush = new LinearGradientBrush();
			linearGradientBrush.StartPoint = new Point(0.0, 0.0);
			linearGradientBrush.EndPoint = new Point(0.0, 1.0);
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
					switch ((MessageType)value)
					{
					case MessageType.Contamination:
					{
						Color color = (Color)ColorConverter.ConvertFromString("#f4cb98");
						Color color2 = (Color)ColorConverter.ConvertFromString("#f7941d");
						linearGradientBrush.GradientStops.Add(new GradientStop(color, 0.0));
						linearGradientBrush.GradientStops.Add(new GradientStop(color2, 1.0));
						brush = linearGradientBrush;
						break;
					}
					case MessageType.Error:
					{
						Color color = (Color)ColorConverter.ConvertFromString("#e28e8e");
						Color color2 = (Color)ColorConverter.ConvertFromString("#e01a1a");
						linearGradientBrush.GradientStops.Add(new GradientStop(color, 0.0));
						linearGradientBrush.GradientStops.Add(new GradientStop(color2, 1.0));
						brush = linearGradientBrush;
						break;
					}
					case MessageType.Warning:
					{
						Color color = (Color)ColorConverter.ConvertFromString("#f2e998");
						Color color2 = (Color)ColorConverter.ConvertFromString("#f4e21d");
						linearGradientBrush.GradientStops.Add(new GradientStop(color, 0.0));
						linearGradientBrush.GradientStops.Add(new GradientStop(color2, 1.0));
						brush = linearGradientBrush;
						break;
					}
					default:
					{
						BrushConverter brushConverter = new BrushConverter();
						brush = (SolidColorBrush)brushConverter.ConvertFrom("#e7efc5");
						break;
					}
					}
				}
				result = brush;
			}
			return result;
		}

		// Token: 0x0600275A RID: 10074 RVA: 0x001091FB File Offset: 0x001073FB
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
