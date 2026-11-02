using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x02000204 RID: 516
	public class ResetButtonVisibilityConverter : IMultiValueConverter
	{
		// Token: 0x06002843 RID: 10307 RVA: 0x0010CDA0 File Offset: 0x0010AFA0
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			Visibility visibility = Visibility.Collapsed;
			bool flag = values.Length == 2 && values[0] is int && values[1] is BuiltInPriviledges;
			if (flag)
			{
				int num = (int)values[0];
				BuiltInPriviledges builtInPriviledges = (BuiltInPriviledges)values[1];
				bool flag2 = num != 0 && builtInPriviledges >= BuiltInPriviledges.Admin;
				if (flag2)
				{
					visibility = Visibility.Visible;
				}
				else
				{
					visibility = Visibility.Collapsed;
				}
			}
			return visibility;
		}

		// Token: 0x06002844 RID: 10308 RVA: 0x001091FB File Offset: 0x001073FB
		public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
