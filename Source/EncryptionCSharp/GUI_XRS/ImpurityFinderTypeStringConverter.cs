using System;
using System.Globalization;
using System.Windows.Data;

namespace GUI_XRS
{
	// Token: 0x020001EE RID: 494
	[ValueConversion(typeof(ImpurityFinderType), typeof(string))]
	public class ImpurityFinderTypeStringConverter : IValueConverter
	{
		// Token: 0x06002801 RID: 10241 RVA: 0x0010C0DC File Offset: 0x0010A2DC
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string result = "";
			bool flag = value is ImpurityFinderType;
			if (flag)
			{
				ImpurityFinderType impurityFinderType = (ImpurityFinderType)value;
				ImpurityFinderType impurityFinderType2 = impurityFinderType;
				ImpurityFinderType impurityFinderType3 = impurityFinderType2;
				if (impurityFinderType3 != ImpurityFinderType.Metal)
				{
					if (impurityFinderType3 == ImpurityFinderType.Glass)
					{
						result = LocalizationManager.Localize("txtGlass");
					}
				}
				else
				{
					result = LocalizationManager.Localize("txtMetal");
				}
			}
			return result;
		}

		// Token: 0x06002802 RID: 10242 RVA: 0x0010C138 File Offset: 0x0010A338
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			ImpurityFinderType impurityFinderType = ImpurityFinderType.Metal;
			bool flag = value is string;
			if (flag)
			{
				string a = (string)value;
				bool flag2 = a == LocalizationManager.Localize("txtMetal");
				if (flag2)
				{
					impurityFinderType = ImpurityFinderType.Metal;
				}
				else
				{
					bool flag3 = a == LocalizationManager.Localize("txtGlass");
					if (flag3)
					{
						impurityFinderType = ImpurityFinderType.Glass;
					}
				}
			}
			return impurityFinderType;
		}
	}
}
