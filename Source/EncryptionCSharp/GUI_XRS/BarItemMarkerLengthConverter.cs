using System;
using System.Globalization;
using System.Windows.Data;

namespace GUI_XRS
{
	// Token: 0x020001CA RID: 458
	public class BarItemMarkerLengthConverter : IValueConverter
	{
		// Token: 0x06002795 RID: 10133 RVA: 0x0010ADB0 File Offset: 0x00108FB0
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			double num = 0.0;
			bool flag = targetType != typeof(double);
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
					bool isMarker = barItem.IsMarker;
					if (isMarker)
					{
						bool isShortMarker = barItem.IsShortMarker;
						if (isShortMarker)
						{
							num = 2.0;
						}
						else
						{
							num = 5.0;
						}
					}
				}
				result = num;
			}
			return result;
		}

		// Token: 0x06002796 RID: 10134 RVA: 0x001091FB File Offset: 0x001073FB
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
