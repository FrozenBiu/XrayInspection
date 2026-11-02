using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace GUI_XRS
{
	// Token: 0x02000200 RID: 512
	public class KeyboardTypeBoxConverter : IMultiValueConverter
	{
		// Token: 0x06002837 RID: 10295 RVA: 0x0010CBD8 File Offset: 0x0010ADD8
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			bool flag = values[0] is KeyboardType && values[1] is IInputElement && values[2] is IInputElement;
			object result;
			if (flag)
			{
				KeyboardType keyboardType = (KeyboardType)values[0];
				IInputElement inputElement = (IInputElement)values[1];
				IInputElement inputElement2 = (IInputElement)values[2];
				result = ((keyboardType != KeyboardType.Password) ? inputElement : inputElement2);
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06002838 RID: 10296 RVA: 0x001091FB File Offset: 0x001073FB
		public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
