using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x0200020E RID: 526
	[ValueConversion(typeof(MessageType), typeof(Brush))]
	public class AuditCheckEntryToBrushConverter : IValueConverter
	{
		// Token: 0x06002861 RID: 10337 RVA: 0x0010DB1C File Offset: 0x0010BD1C
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
				BoundLocalizedText boundLocalizedText = new BoundLocalizedText("txtFailed");
				bool flag2 = value is string;
				if (flag2)
				{
					bool flag3 = value.Equals(boundLocalizedText.LocalizedText);
					if (flag3)
					{
						Color color = (Color)ColorConverter.ConvertFromString("#e28e8e");
						Color color2 = (Color)ColorConverter.ConvertFromString("#e01a1a");
						linearGradientBrush.GradientStops.Add(new GradientStop(color, 0.0));
						linearGradientBrush.GradientStops.Add(new GradientStop(color2, 1.0));
						brush = linearGradientBrush;
					}
					else
					{
						BrushConverter brushConverter = new BrushConverter();
						brush = (SolidColorBrush)brushConverter.ConvertFrom("#e7efc5");
					}
				}
				MainWindow.localizationManager.Deregister(boundLocalizedText);
				result = brush;
			}
			return result;
		}

		// Token: 0x06002862 RID: 10338 RVA: 0x001091FB File Offset: 0x001073FB
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
