using System;
using System.Globalization;
using System.Windows.Data;

namespace GUI_XRS
{
	// Token: 0x020001BB RID: 443
	[ValueConversion(typeof(RejectPriority), typeof(string))]
	public class RejectPriorityStringConverter : IValueConverter
	{
		// Token: 0x06002768 RID: 10088 RVA: 0x0010A430 File Offset: 0x00108630
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string result = "";
			bool flag = value is RejectPriority;
			if (flag)
			{
				switch ((RejectPriority)value)
				{
				case RejectPriority.Highest:
					result = LocalizationManager.Localize("txtHighest");
					break;
				case RejectPriority.High:
					result = LocalizationManager.Localize("txtHigh");
					break;
				case RejectPriority.Low:
					result = LocalizationManager.Localize("txtLow");
					break;
				case RejectPriority.Lowest:
					result = LocalizationManager.Localize("txtLowest");
					break;
				}
			}
			return result;
		}

		// Token: 0x06002769 RID: 10089 RVA: 0x0010A4B4 File Offset: 0x001086B4
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			RejectPriority rejectPriority = RejectPriority.Lowest;
			bool flag = value is string;
			if (flag)
			{
				string a = (string)value;
				bool flag2 = a == LocalizationManager.Localize("txtHighest");
				if (flag2)
				{
					rejectPriority = RejectPriority.Highest;
				}
				else
				{
					bool flag3 = a == LocalizationManager.Localize("txtHigh");
					if (flag3)
					{
						rejectPriority = RejectPriority.High;
					}
					else
					{
						bool flag4 = a == LocalizationManager.Localize("txtLow");
						if (flag4)
						{
							rejectPriority = RejectPriority.Low;
						}
						else
						{
							bool flag5 = a == LocalizationManager.Localize("txtLowest");
							if (flag5)
							{
								rejectPriority = RejectPriority.Lowest;
							}
						}
					}
				}
			}
			return rejectPriority;
		}
	}
}
