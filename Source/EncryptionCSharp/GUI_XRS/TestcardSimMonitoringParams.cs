using System;

namespace GUI_XRS
{
	// Token: 0x020000D7 RID: 215
	public class TestcardSimMonitoringParams
	{
		// Token: 0x1700064F RID: 1615
		// (get) Token: 0x060014F4 RID: 5364 RVA: 0x0007D88D File Offset: 0x0007BA8D
		// (set) Token: 0x060014F5 RID: 5365 RVA: 0x0007D895 File Offset: 0x0007BA95
		public DateTime DTTimeStamp { get; set; }

		// Token: 0x17000650 RID: 1616
		// (get) Token: 0x060014F6 RID: 5366 RVA: 0x0007D89E File Offset: 0x0007BA9E
		// (set) Token: 0x060014F7 RID: 5367 RVA: 0x0007D8A6 File Offset: 0x0007BAA6
		public string StrTimeStamp { get; set; }

		// Token: 0x17000651 RID: 1617
		// (get) Token: 0x060014F8 RID: 5368 RVA: 0x0007D8AF File Offset: 0x0007BAAF
		// (set) Token: 0x060014F9 RID: 5369 RVA: 0x0007D8B7 File Offset: 0x0007BAB7
		public string VAAct { get; set; }

		// Token: 0x17000652 RID: 1618
		// (get) Token: 0x060014FA RID: 5370 RVA: 0x0007D8C0 File Offset: 0x0007BAC0
		// (set) Token: 0x060014FB RID: 5371 RVA: 0x0007D8C8 File Offset: 0x0007BAC8
		public string GlAct { get; set; }

		// Token: 0x17000653 RID: 1619
		// (get) Token: 0x060014FC RID: 5372 RVA: 0x0007D8D1 File Offset: 0x0007BAD1
		// (set) Token: 0x060014FD RID: 5373 RVA: 0x0007D8D9 File Offset: 0x0007BAD9
		public string VACompl { get; set; }

		// Token: 0x17000654 RID: 1620
		// (get) Token: 0x060014FE RID: 5374 RVA: 0x0007D8E2 File Offset: 0x0007BAE2
		// (set) Token: 0x060014FF RID: 5375 RVA: 0x0007D8EA File Offset: 0x0007BAEA
		public string GlCompl { get; set; }

		// Token: 0x17000655 RID: 1621
		// (get) Token: 0x06001500 RID: 5376 RVA: 0x0007D8F3 File Offset: 0x0007BAF3
		// (set) Token: 0x06001501 RID: 5377 RVA: 0x0007D8FB File Offset: 0x0007BAFB
		public bool NoComplBackRed { get; set; }
	}
}
