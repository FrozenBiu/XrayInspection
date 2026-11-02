using System;
using System.Globalization;
using System.Windows.Data;

namespace GUI_XRS
{
	// Token: 0x0200020C RID: 524
	[ValueConversion(typeof(TestCardSize), typeof(string))]
	public class IVPSingleTestCardSizeStringConverter : IValueConverter
	{
		// Token: 0x0600285B RID: 10331 RVA: 0x0010D96C File Offset: 0x0010BB6C
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string result = "";
			bool flag = value is TestCardSize;
			if (flag)
			{
				if (value is TestCardSize)
				{
					switch ((TestCardSize)value)
					{
					case TestCardSize.SingleBall:
						result = LocalizationManager.Localize("txtSingleBall");
						break;
					case TestCardSize.Testcard5:
						result = LocalizationManager.Localize("txtTestCard5");
						break;
					case TestCardSize.Testcard100:
						result = LocalizationManager.Localize("txtTestCard100");
						break;
					case TestCardSize.Other:
						result = LocalizationManager.Localize("txtOther");
						break;
					case TestCardSize.Testcard5Same:
						result = LocalizationManager.Localize("txtTestCard5Same");
						break;
					}
				}
			}
			return result;
		}

		// Token: 0x0600285C RID: 10332 RVA: 0x0010DA08 File Offset: 0x0010BC08
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			TestCardSize testCardSize = TestCardSize.Other;
			bool flag = value is string;
			if (flag)
			{
				string a = (string)value;
				bool flag2 = a == LocalizationManager.Localize("txtSingleBall");
				if (flag2)
				{
					testCardSize = TestCardSize.SingleBall;
				}
				else
				{
					bool flag3 = a == LocalizationManager.Localize("txtTestCard5");
					if (flag3)
					{
						testCardSize = TestCardSize.Testcard5;
					}
					else
					{
						bool flag4 = a == LocalizationManager.Localize("txtTestCard100");
						if (flag4)
						{
							testCardSize = TestCardSize.Testcard100;
						}
						else
						{
							bool flag5 = a == LocalizationManager.Localize("txtTestCard5Same");
							if (flag5)
							{
								testCardSize = TestCardSize.Testcard5Same;
							}
							else
							{
								bool flag6 = a == LocalizationManager.Localize("txtOther");
								if (flag6)
								{
									testCardSize = TestCardSize.Other;
								}
							}
						}
					}
				}
			}
			return testCardSize;
		}
	}
}
