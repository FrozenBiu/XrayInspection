using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace GUI_XRS
{
	// Token: 0x020001C8 RID: 456
	[ValueConversion(typeof(ObservableCollection<Point>), typeof(PointCollection))]
	public class PointCollectionConverter : IValueConverter
	{
		// Token: 0x0600278F RID: 10127 RVA: 0x0010ACBC File Offset: 0x00108EBC
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			bool flag = value.GetType() == typeof(ObservableCollection<Point>) && targetType == typeof(PointCollection);
			object result;
			if (flag)
			{
				PointCollection pointCollection = new PointCollection();
				foreach (Point value2 in (value as ObservableCollection<Point>))
				{
					pointCollection.Add(value2);
				}
				result = pointCollection;
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06002790 RID: 10128 RVA: 0x0010AD50 File Offset: 0x00108F50
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return null;
		}
	}
}
