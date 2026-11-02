using System;
using System.Collections.Generic;

namespace GUI_XRS
{
	// Token: 0x02000225 RID: 549
	public class ParameterPathComparer : IComparer<string>
	{
		// Token: 0x0600291D RID: 10525 RVA: 0x001114C4 File Offset: 0x0010F6C4
		public int Compare(string item1, string item2)
		{
			string text = item1;
			string text2 = item2;
			bool flag = item2 == null;
			int result;
			if (flag)
			{
				result = item1.CompareTo(item2);
			}
			else
			{
				for (int i = 0; i < 3; i++)
				{
					int num = text.LastIndexOf('/');
					bool flag2 = num == -1;
					if (flag2)
					{
						break;
					}
					text = text.Substring(0, num);
				}
				for (int j = 0; j < 2; j++)
				{
					int num2 = text2.LastIndexOf('/');
					bool flag3 = num2 == -1;
					if (flag3)
					{
						break;
					}
					text2 = text2.Substring(0, num2);
				}
				bool flag4 = text == text2;
				if (flag4)
				{
					result = 1;
				}
				else
				{
					text = item1;
					text2 = item2;
					for (int k = 0; k < 2; k++)
					{
						int num3 = text.LastIndexOf('/');
						bool flag5 = num3 == -1;
						if (flag5)
						{
							break;
						}
						text = text.Substring(0, num3);
					}
					for (int l = 0; l < 3; l++)
					{
						int num4 = text2.LastIndexOf('/');
						bool flag6 = num4 == -1;
						if (flag6)
						{
							break;
						}
						text2 = text2.Substring(0, num4);
					}
					bool flag7 = text == text2;
					if (flag7)
					{
						result = -1;
					}
					else
					{
						result = item1.CompareTo(item2);
					}
				}
			}
			return result;
		}
	}
}
