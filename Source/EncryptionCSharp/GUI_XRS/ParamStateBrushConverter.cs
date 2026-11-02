using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace GUI_XRS
{
	// Token: 0x020001DB RID: 475
	public class ParamStateBrushConverter : IValueConverter
	{
		// Token: 0x060027C8 RID: 10184 RVA: 0x0010B688 File Offset: 0x00109888
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
				bool flag2 = value is ParamState;
				if (flag2)
				{
					switch ((ParamState)value)
					{
					case ParamState.Neutral:
						brush = Brushes.Transparent;
						break;
					case ParamState.Equal:
					{
						Color color = (Color)ColorConverter.ConvertFromString("#8ee28e");
						Color color2 = (Color)ColorConverter.ConvertFromString("#1ae01a");
						linearGradientBrush.GradientStops.Add(new GradientStop(color, 0.0));
						linearGradientBrush.GradientStops.Add(new GradientStop(color2, 1.0));
						brush = linearGradientBrush;
						break;
					}
					case ParamState.Changed:
					{
						Color color = (Color)ColorConverter.ConvertFromString("#e28e8e");
						Color color2 = (Color)ColorConverter.ConvertFromString("#e01a1a");
						linearGradientBrush.GradientStops.Add(new GradientStop(color, 0.0));
						linearGradientBrush.GradientStops.Add(new GradientStop(color2, 1.0));
						brush = linearGradientBrush;
						break;
					}
					default:
						brush = Brushes.Transparent;
						break;
					}
				}
				result = brush;
			}
			return result;
		}

		// Token: 0x060027C9 RID: 10185 RVA: 0x001091FB File Offset: 0x001073FB
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
