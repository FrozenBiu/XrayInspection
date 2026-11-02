using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace GUI_XRS
{
	// Token: 0x020001F0 RID: 496
	public class MultiBoolVisibilityCollapsedConverted : IMultiValueConverter
	{
		// Token: 0x06002807 RID: 10247 RVA: 0x0010C244 File Offset: 0x0010A444
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			Visibility visibility = Visibility.Visible;
			bool flag = targetType == typeof(Visibility) && values != null;
			if (flag)
			{
				for (int i = 0; i < values.Length; i++)
				{
					bool flag2 = !(values[i] is bool) || !(bool)values[i];
					if (flag2)
					{
						visibility = Visibility.Collapsed;
						break;
					}
				}
			}
			return visibility;
		}

		// Token: 0x06002808 RID: 10248 RVA: 0x001091FB File Offset: 0x001073FB
		public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
