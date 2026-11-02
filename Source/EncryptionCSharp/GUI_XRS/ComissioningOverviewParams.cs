using System;

namespace GUI_XRS
{
	// Token: 0x0200005D RID: 93
	public class ComissioningOverviewParams
	{
		// Token: 0x170002BC RID: 700
		// (get) Token: 0x0600094C RID: 2380 RVA: 0x00039A8D File Offset: 0x00037C8D
		// (set) Token: 0x0600094D RID: 2381 RVA: 0x00039A95 File Offset: 0x00037C95
		public string StrStatus { get; set; }

		// Token: 0x170002BD RID: 701
		// (get) Token: 0x0600094E RID: 2382 RVA: 0x00039A9E File Offset: 0x00037C9E
		// (set) Token: 0x0600094F RID: 2383 RVA: 0x00039AA6 File Offset: 0x00037CA6
		public string StrID { get; set; }

		// Token: 0x170002BE RID: 702
		// (get) Token: 0x06000950 RID: 2384 RVA: 0x00039AAF File Offset: 0x00037CAF
		// (set) Token: 0x06000951 RID: 2385 RVA: 0x00039AB7 File Offset: 0x00037CB7
		public string StrPathes { get; set; }

		// Token: 0x170002BF RID: 703
		// (get) Token: 0x06000952 RID: 2386 RVA: 0x00039AC0 File Offset: 0x00037CC0
		// (set) Token: 0x06000953 RID: 2387 RVA: 0x00039AC8 File Offset: 0x00037CC8
		public string StrValuesComissioning { get; set; }

		// Token: 0x170002C0 RID: 704
		// (get) Token: 0x06000954 RID: 2388 RVA: 0x00039AD1 File Offset: 0x00037CD1
		// (set) Token: 0x06000955 RID: 2389 RVA: 0x00039AD9 File Offset: 0x00037CD9
		public string StrValuesActual { get; set; }

		// Token: 0x170002C1 RID: 705
		// (get) Token: 0x06000956 RID: 2390 RVA: 0x00039AE2 File Offset: 0x00037CE2
		// (set) Token: 0x06000957 RID: 2391 RVA: 0x00039AEA File Offset: 0x00037CEA
		public bool DifferentCellRed { get; set; }

		// Token: 0x170002C2 RID: 706
		// (get) Token: 0x06000958 RID: 2392 RVA: 0x00039AF3 File Offset: 0x00037CF3
		// (set) Token: 0x06000959 RID: 2393 RVA: 0x00039AFB File Offset: 0x00037CFB
		public bool DefaultRedCell { get; set; }
	}
}
