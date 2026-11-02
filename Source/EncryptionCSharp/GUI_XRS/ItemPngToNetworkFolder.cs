using System;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x0200007F RID: 127
	public class ItemPngToNetworkFolder
	{
		// Token: 0x170003C9 RID: 969
		// (get) Token: 0x06000C83 RID: 3203 RVA: 0x0004AF36 File Offset: 0x00049136
		// (set) Token: 0x06000C84 RID: 3204 RVA: 0x0004AF3E File Offset: 0x0004913E
		public XRSImage Image { get; set; }

		// Token: 0x170003CA RID: 970
		// (get) Token: 0x06000C85 RID: 3205 RVA: 0x0004AF47 File Offset: 0x00049147
		// (set) Token: 0x06000C86 RID: 3206 RVA: 0x0004AF4F File Offset: 0x0004914F
		public XRSImage.RotateDirection RotateDirection { get; set; }

		// Token: 0x170003CB RID: 971
		// (get) Token: 0x06000C87 RID: 3207 RVA: 0x0004AF58 File Offset: 0x00049158
		// (set) Token: 0x06000C88 RID: 3208 RVA: 0x0004AF60 File Offset: 0x00049160
		public int MaxFiles { get; set; }

		// Token: 0x170003CC RID: 972
		// (get) Token: 0x06000C89 RID: 3209 RVA: 0x0004AF69 File Offset: 0x00049169
		// (set) Token: 0x06000C8A RID: 3210 RVA: 0x0004AF71 File Offset: 0x00049171
		public string ProductName { get; set; }

		// Token: 0x170003CD RID: 973
		// (get) Token: 0x06000C8B RID: 3211 RVA: 0x0004AF7A File Offset: 0x0004917A
		// (set) Token: 0x06000C8C RID: 3212 RVA: 0x0004AF82 File Offset: 0x00049182
		public string ProductNumber { get; set; }

		// Token: 0x170003CE RID: 974
		// (get) Token: 0x06000C8D RID: 3213 RVA: 0x0004AF8B File Offset: 0x0004918B
		// (set) Token: 0x06000C8E RID: 3214 RVA: 0x0004AF93 File Offset: 0x00049193
		public string NetworkPath { get; set; }

		// Token: 0x170003CF RID: 975
		// (get) Token: 0x06000C8F RID: 3215 RVA: 0x0004AF9C File Offset: 0x0004919C
		// (set) Token: 0x06000C90 RID: 3216 RVA: 0x0004AFA4 File Offset: 0x000491A4
		public string Mode { get; set; }

		// Token: 0x170003D0 RID: 976
		// (get) Token: 0x06000C91 RID: 3217 RVA: 0x0004AFAD File Offset: 0x000491AD
		// (set) Token: 0x06000C92 RID: 3218 RVA: 0x0004AFB5 File Offset: 0x000491B5
		public string SerialNumber { get; set; }

		// Token: 0x170003D1 RID: 977
		// (get) Token: 0x06000C93 RID: 3219 RVA: 0x0004AFBE File Offset: 0x000491BE
		// (set) Token: 0x06000C94 RID: 3220 RVA: 0x0004AFC6 File Offset: 0x000491C6
		public string Is16BitResolution { get; set; }

		// Token: 0x170003D2 RID: 978
		// (get) Token: 0x06000C95 RID: 3221 RVA: 0x0004AFCF File Offset: 0x000491CF
		// (set) Token: 0x06000C96 RID: 3222 RVA: 0x0004AFD7 File Offset: 0x000491D7
		public string BinningMode { get; set; }

		// Token: 0x170003D3 RID: 979
		// (get) Token: 0x06000C97 RID: 3223 RVA: 0x0004AFE0 File Offset: 0x000491E0
		// (set) Token: 0x06000C98 RID: 3224 RVA: 0x0004AFE8 File Offset: 0x000491E8
		public string LocationNetworkMode { get; set; }
	}
}
