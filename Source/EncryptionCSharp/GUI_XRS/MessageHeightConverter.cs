using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using DatabaseConnection;

namespace GUI_XRS
{
	// Token: 0x020001A8 RID: 424
	public class MessageHeightConverter : IMultiValueConverter
	{
		// Token: 0x0600272F RID: 10031 RVA: 0x001095B0 File Offset: 0x001077B0
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			bool flag = values[0] is ProductType && values[1] is bool && values[2] is int;
			object result;
			if (flag)
			{
				ProductType productType = (ProductType)values[0];
				bool flag2 = (bool)values[1];
				int num = (int)values[2];
				bool flag3 = productType == ProductType.SingleProduct && !flag2;
				if (flag3)
				{
					result = new GridLength(90.0);
				}
				else
				{
					bool flag4 = productType == ProductType.MultiProduct;
					if (flag4)
					{
						bool flag5 = num > 14;
						if (flag5)
						{
							result = new GridLength(165.0);
						}
						else
						{
							result = new GridLength(125.0);
						}
					}
					else
					{
						result = new GridLength(125.0);
					}
				}
			}
			else
			{
				result = new GridLength(90.0);
			}
			return result;
		}

		// Token: 0x06002730 RID: 10032 RVA: 0x001091FB File Offset: 0x001073FB
		public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
