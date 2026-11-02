using System;
using System.Collections.Generic;

namespace GUI_XRS
{
	// Token: 0x02000139 RID: 313
	public class ProductDataComparerGeneric : IComparer<ProductData>
	{
		// Token: 0x06001DF0 RID: 7664 RVA: 0x000C2C28 File Offset: 0x000C0E28
		public int Compare(ProductData item1, ProductData item2)
		{
			bool flag = item1 != null && item2 != null;
			int result;
			if (flag)
			{
				int num;
				int value;
				bool flag2 = int.TryParse(item1.ProductNumber, out num) && int.TryParse(item2.ProductNumber, out value);
				if (flag2)
				{
					result = num.CompareTo(value);
				}
				else
				{
					result = this.CompareSubProductData(item1.ProductNumber, item2.ProductNumber);
				}
			}
			else
			{
				result = 0;
			}
			return result;
		}

		// Token: 0x06001DF1 RID: 7665 RVA: 0x000C2C94 File Offset: 0x000C0E94
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
