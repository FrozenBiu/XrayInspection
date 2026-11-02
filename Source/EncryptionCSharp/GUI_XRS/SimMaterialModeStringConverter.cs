using System;
using System.Globalization;
using System.Windows.Data;

namespace GUI_XRS
{
	// Token: 0x02000208 RID: 520
	[ValueConversion(typeof(SimMaterial), typeof(string[]))]
	public class SimMaterialModeStringConverter : IValueConverter
	{
		// Token: 0x0600284F RID: 10319 RVA: 0x0010D0E8 File Offset: 0x0010B2E8
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string result = "";
			bool flag = value is SimMaterial;
			if (flag)
			{
				switch ((SimMaterial)value)
				{
				case SimMaterial.VAandGlass:
					result = LocalizationManager.Localize("txtVAGlass");
					break;
				case SimMaterial.VA:
					result = LocalizationManager.Localize("txtVA");
					break;
				case SimMaterial.Glass:
					result = LocalizationManager.Localize("txtGlass");
					break;
				case SimMaterial.OtherMaterial:
					result = LocalizationManager.Localize("txtOtherMaterial");
					break;
				}
			}
			return result;
		}

		// Token: 0x06002850 RID: 10320 RVA: 0x0010D16C File Offset: 0x0010B36C
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			SimMaterial simMaterial = SimMaterial.VAandGlass;
			bool flag = value is string;
			if (flag)
			{
				string a = (string)value;
				bool flag2 = a == LocalizationManager.Localize("txtVAGlass");
				if (flag2)
				{
					simMaterial = SimMaterial.VAandGlass;
				}
				else
				{
					bool flag3 = a == LocalizationManager.Localize("txtVA");
					if (flag3)
					{
						simMaterial = SimMaterial.VA;
					}
					else
					{
						bool flag4 = a == LocalizationManager.Localize("txtGlass");
						if (flag4)
						{
							simMaterial = SimMaterial.Glass;
						}
					}
				}
			}
			return simMaterial;
		}
	}
}
