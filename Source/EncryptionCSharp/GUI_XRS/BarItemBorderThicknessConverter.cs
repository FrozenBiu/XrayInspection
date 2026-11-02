using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace GUI_XRS
{
	// Token: 0x020001CB RID: 459
	public class BarItemBorderThicknessConverter : IValueConverter
	{
		// Token: 0x06002798 RID: 10136 RVA: 0x0010AE34 File Offset: 0x00109034
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			Thickness thickness = default(Thickness);
			bool flag = targetType != typeof(Thickness);
			object result;
			if (flag)
			{
				result = null;
			}
			else
			{
				bool flag2 = value is BarItem;
				if (flag2)
				{
					BarItem barItem = (BarItem)value;
					bool isFirstItem = barItem.IsFirstItem;
					if (isFirstItem)
					{
						thickness = new Thickness(1.0, 0.0, 1.0, 1.0);
					}
					else
					{
						bool isLastItem = barItem.IsLastItem;
						if (isLastItem)
						{
							thickness = new Thickness(1.0, 0.0, 1.0, 1.0);
						}
						else
						{
							thickness = new Thickness(1.0, 0.0, 1.0, 0.0);
						}
					}
				}
				result = thickness;
			}
			return result;
		}

		// Token: 0x06002799 RID: 10137 RVA: 0x001091FB File Offset: 0x001073FB
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
