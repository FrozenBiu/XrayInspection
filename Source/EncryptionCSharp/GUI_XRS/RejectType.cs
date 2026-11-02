using System;

namespace GUI_XRS
{
	// Token: 0x0200014F RID: 335
	public enum RejectType
	{
		// Token: 0x040014D0 RID: 5328
		Contamination = 1,
		// Token: 0x040014D1 RID: 5329
		Overweight,
		// Token: 0x040014D2 RID: 5330
		Underweight = 4,
		// Token: 0x040014D3 RID: 5331
		Overcount = 8,
		// Token: 0x040014D4 RID: 5332
		Undercount = 16,
		// Token: 0x040014D5 RID: 5333
		Oversize = 32,
		// Token: 0x040014D6 RID: 5334
		Undersize = 64
	}
}
