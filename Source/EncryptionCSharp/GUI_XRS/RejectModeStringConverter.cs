using System;
using System.Globalization;
using System.Windows.Data;

namespace GUI_XRS
{
	// Token: 0x020001D8 RID: 472
	[ValueConversion(typeof(RejectMode), typeof(string))]
	public class RejectModeStringConverter : IValueConverter
	{
		// Token: 0x060027BF RID: 10175 RVA: 0x0010B524 File Offset: 0x00109724
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			bool flag = value is RejectMode;
			object result;
			if (flag)
			{
				RejectMode rejectMode = (RejectMode)value;
				result = ((rejectMode == RejectMode.Center) ? LocalizationManager.Localize("txtRejectModeCenter") : LocalizationManager.Localize("txtRejectModeLeadingEdge"));
			}
			else
			{
				result = "";
			}
			return result;
		}

		// Token: 0x060027C0 RID: 10176 RVA: 0x0010B56C File Offset: 0x0010976C
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			RejectMode rejectMode = RejectMode.Center;
			bool flag = value is string;
			if (flag)
			{
				string a = (string)value;
				bool flag2 = a == LocalizationManager.Localize("txtRejectModeLeadingEdge");
				if (flag2)
				{
					rejectMode = RejectMode.LeadingEdge;
				}
			}
			return rejectMode;
		}
	}
}
