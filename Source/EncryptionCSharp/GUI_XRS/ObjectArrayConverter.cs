using System;
using System.Globalization;
using System.Windows.Data;

namespace GUI_XRS
{
	// Token: 0x02000201 RID: 513
	public class ObjectArrayConverter : IMultiValueConverter
	{
		// Token: 0x0600283A RID: 10298 RVA: 0x0010CC40 File Offset: 0x0010AE40
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			object[] array = new object[values.Length];
			values.CopyTo(array, 0);
			return array;
		}

		// Token: 0x0600283B RID: 10299 RVA: 0x001091FB File Offset: 0x001073FB
		public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
