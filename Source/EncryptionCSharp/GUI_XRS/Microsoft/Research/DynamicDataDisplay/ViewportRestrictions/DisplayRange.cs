using System;

namespace Microsoft.Research.DynamicDataDisplay.ViewportRestrictions
{
	// Token: 0x02000006 RID: 6
	public class DisplayRange
	{
		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000023 RID: 35 RVA: 0x0000237E File Offset: 0x0000057E
		// (set) Token: 0x06000024 RID: 36 RVA: 0x00002386 File Offset: 0x00000586
		public double Start { get; set; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000025 RID: 37 RVA: 0x0000238F File Offset: 0x0000058F
		// (set) Token: 0x06000026 RID: 38 RVA: 0x00002397 File Offset: 0x00000597
		public double End { get; set; }

		// Token: 0x06000027 RID: 39 RVA: 0x000023A0 File Offset: 0x000005A0
		public DisplayRange(double start, double end)
		{
			this.Start = start;
			this.End = end;
		}
	}
}
