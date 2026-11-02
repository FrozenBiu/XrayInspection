using System;
using System.Collections;

namespace GUI_XRS
{
	// Token: 0x02000138 RID: 312
	public class ProductDataComparer : IComparer
	{
		// Token: 0x06001DED RID: 7661 RVA: 0x000C2AD0 File Offset: 0x000C0CD0
		public int Compare(object item1, object item2)
		{
			ProductData productData = item1 as ProductData;
			ProductData productData2 = item2 as ProductData;
			bool flag = productData != null && productData2 != null;
			int result;
			if (flag)
			{
				int num;
				int value;
				bool flag2 = int.TryParse(productData.ProductNumber, out num) && int.TryParse(productData2.ProductNumber, out value);
				if (flag2)
				{
					result = num.CompareTo(value);
				}
				else
				{
					result = this.CompareSubProductData(productData.ProductNumber, productData2.ProductNumber);
				}
			}
			else
			{
				result = 0;
			}
			return result;
		}

		// Token: 0x06001DEE RID: 7662 RVA: 0x000C2B4C File Offset: 0x000C0D4C
		private int CompareSubProductData(string strProductNumber1, string strProductNumber2)
		{
			int num = strProductNumber1.IndexOf('_');
			int num2 = strProductNumber2.IndexOf('_');
			bool flag = num > 0 && num2 > 0 && num < strProductNumber1.Length - 1 && num2 < strProductNumber2.Length - 1;
			if (flag)
			{
				int num3;
				int value;
				bool flag2 = int.TryParse(strProductNumber1.Remove(0, num + 1), out num3) && int.TryParse(strProductNumber2.Remove(0, num2 + 1), out value);
				if (flag2)
				{
					return num3.CompareTo(value);
				}
			}
			bool flag3 = strProductNumber1 == "" && strProductNumber2 != "";
			int result;
			if (flag3)
			{
				result = 1;
			}
			else
			{
				bool flag4 = strProductNumber2 == "" && strProductNumber1 != "";
				if (flag4)
				{
					result = -1;
				}
				else
				{
					result = strProductNumber1.CompareTo(strProductNumber2);
				}
			}
			return result;
		}
	}
}
