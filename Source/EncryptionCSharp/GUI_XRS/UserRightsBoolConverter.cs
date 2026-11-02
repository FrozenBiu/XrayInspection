using System;
using System.Globalization;
using System.Windows.Data;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x020001DF RID: 479
	[ValueConversion(typeof(BuiltInPriviledges), typeof(bool))]
	public class UserRightsBoolConverter : IValueConverter
	{
		// Token: 0x060027D4 RID: 10196 RVA: 0x0010BAE0 File Offset: 0x00109CE0
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			bool flag = targetType != typeof(bool);
			object result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = value is BuiltInPriviledges;
				if (flag2)
				{
					result = ((BuiltInPriviledges)value > BuiltInPriviledges.NoUser);
				}
				else
				{
					result = false;
				}
			}
			return result;
		}

		// Token: 0x060027D5 RID: 10197 RVA: 0x001091FB File Offset: 0x001073FB
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
