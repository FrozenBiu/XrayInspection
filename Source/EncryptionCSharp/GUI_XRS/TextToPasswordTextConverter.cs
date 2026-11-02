using System;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace GUI_XRS
{
	// Token: 0x020001E9 RID: 489
	public class TextToPasswordTextConverter : IValueConverter
	{
		// Token: 0x060027F2 RID: 10226 RVA: 0x0010BEC8 File Offset: 0x0010A0C8
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = value is string;
			if (flag)
			{
				string text = (string)value;
				for (int i = 0; i < text.Length; i++)
				{
					stringBuilder.Append('*');
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060027F3 RID: 10227 RVA: 0x001091FB File Offset: 0x001073FB
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
