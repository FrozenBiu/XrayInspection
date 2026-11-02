using System;
using System.Collections;

namespace GUI_XRS
{
	// Token: 0x0200013D RID: 317
	public class ChkPointComparer : IComparer
	{
		// Token: 0x06001E81 RID: 7809 RVA: 0x000C88A8 File Offset: 0x000C6AA8
		public int Compare(object item1, object item2)
		{
			ChkPointInfo chkPointInfo = item1 as ChkPointInfo;
			ChkPointInfo chkPointInfo2 = item2 as ChkPointInfo;
			bool flag = chkPointInfo != null && chkPointInfo2 != null;
			int result;
			if (flag)
			{
				result = chkPointInfo.Name.CompareTo(chkPointInfo2.Name);
			}
			else
			{
				result = 0;
			}
			return result;
		}
	}
}
