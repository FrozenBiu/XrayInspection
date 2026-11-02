using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace GUI_XRS
{
	// Token: 0x020001E0 RID: 480
	[ValueConversion(typeof(bool[]), typeof(Visibility))]
	public class MultiBoolVisConverter : IMultiValueConverter
	{
		// Token: 0x060027D7 RID: 10199 RVA: 0x0010BB38 File Offset: 0x00109D38
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			List<bool> list = new List<bool>();
			foreach (object obj in values)
			{
				bool flag = obj is bool;
				if (flag)
				{
					list.Add((bool)obj);
				}
			}
			bool flag2 = list.Count == 0 || targetType != typeof(Visibility);
			object result;
			if (flag2)
			{
				result = null;
			}
			else
			{
				foreach (bool flag3 in list)
				{
					bool flag4 = !flag3;
					if (flag4)
					{
						return Visibility.Collapsed;
					}
				}
				result = Visibility.Visible;
			}
			return result;
		}

		// Token: 0x060027D8 RID: 10200 RVA: 0x001091FB File Offset: 0x001073FB
		public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
