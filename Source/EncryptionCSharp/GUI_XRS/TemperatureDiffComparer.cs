using System;
using System.Collections.Generic;

namespace GUI_XRS
{
	// Token: 0x02000160 RID: 352
	public class TemperatureDiffComparer : IComparer<TemperatureDifference>
	{
		// Token: 0x06002118 RID: 8472 RVA: 0x000D7A40 File Offset: 0x000D5C40
		public int Compare(TemperatureDifference item1, TemperatureDifference item2)
		{
			return item1.TemperatureDiff.CompareTo(item2.TemperatureDiff);
		}
	}
}
