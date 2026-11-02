using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace GUI_XRS
{
	// Token: 0x0200020B RID: 523
	[ValueConversion(typeof(List<TestCardSize>), typeof(string[]))]
	public class IVPTestCardSizeStringConverter : IValueConverter
	{
		// Token: 0x06002858 RID: 10328 RVA: 0x0010D7BC File Offset: 0x0010B9BC
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			List<string> list = new List<string>();
			bool flag = value is List<TestCardSize>;
			if (flag)
			{
				using (List<TestCardSize>.Enumerator enumerator = ((List<TestCardSize>)value).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						switch (enumerator.Current)
						{
						case TestCardSize.SingleBall:
							list.Add(LocalizationManager.Localize("txtSingleBall"));
							break;
						case TestCardSize.Testcard5:
							list.Add(LocalizationManager.Localize("txtTestCard5"));
							break;
						case TestCardSize.Testcard100:
							list.Add(LocalizationManager.Localize("txtTestCard100"));
							break;
						case TestCardSize.Other:
							list.Add(LocalizationManager.Localize("txtOther"));
							break;
						case TestCardSize.Testcard5Same:
							list.Add(LocalizationManager.Localize("txtTestCard5Same"));
							break;
						}
					}
				}
			}
			return list;
		}

		// Token: 0x06002859 RID: 10329 RVA: 0x0010D8B8 File Offset: 0x0010BAB8
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
