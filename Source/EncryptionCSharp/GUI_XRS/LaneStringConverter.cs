using System;
using System.Globalization;
using System.Windows.Data;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x020001BD RID: 445
	[ValueConversion(typeof(Lane), typeof(string))]
	public class LaneStringConverter : IValueConverter
	{
		// Token: 0x0600276E RID: 10094 RVA: 0x0010A628 File Offset: 0x00108828
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string result = "";
			bool flag = value is Lane;
			if (flag)
			{
				switch ((Lane)value)
				{
				case Lane.All:
					result = LocalizationManager.Localize("txtAll");
					break;
				case Lane.Lane1:
					result = LocalizationManager.Localize("txtLane") + " 1";
					break;
				case Lane.Lane2:
					result = LocalizationManager.Localize("txtLane") + " 2";
					break;
				case Lane.Lane3:
					result = LocalizationManager.Localize("txtLane") + " 3";
					break;
				case Lane.Lane4:
					result = LocalizationManager.Localize("txtLane") + " 4";
					break;
				}
			}
			return result;
		}

		// Token: 0x0600276F RID: 10095 RVA: 0x0010A6E8 File Offset: 0x001088E8
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			Lane lane = Lane.All;
			bool flag = value is string;
			if (flag)
			{
				string a = (string)value;
				bool flag2 = a == LocalizationManager.Localize("txtAll");
				if (flag2)
				{
					lane = Lane.All;
				}
				else
				{
					bool flag3 = a == LocalizationManager.Localize("txtLane") + " 1";
					if (flag3)
					{
						lane = Lane.Lane1;
					}
					else
					{
						bool flag4 = a == LocalizationManager.Localize("txtLane") + " 2";
						if (flag4)
						{
							lane = Lane.Lane2;
						}
						else
						{
							bool flag5 = a == LocalizationManager.Localize("txtLane") + " 3";
							if (flag5)
							{
								lane = Lane.Lane3;
							}
							else
							{
								bool flag6 = a == LocalizationManager.Localize("txtLane") + " 4";
								if (flag6)
								{
									lane = Lane.Lane4;
								}
							}
						}
					}
				}
			}
			return lane;
		}
	}
}
