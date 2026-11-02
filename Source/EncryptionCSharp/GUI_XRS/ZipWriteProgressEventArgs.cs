using System;

namespace GUI_XRS
{
	// Token: 0x02000037 RID: 55
	public class ZipWriteProgressEventArgs : EventArgs
	{
		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x06000632 RID: 1586 RVA: 0x0002A1ED File Offset: 0x000283ED
		// (set) Token: 0x06000633 RID: 1587 RVA: 0x0002A1F5 File Offset: 0x000283F5
		public string FileName { get; set; }

		// Token: 0x170001DA RID: 474
		// (get) Token: 0x06000634 RID: 1588 RVA: 0x0002A1FE File Offset: 0x000283FE
		// (set) Token: 0x06000635 RID: 1589 RVA: 0x0002A206 File Offset: 0x00028406
		public long BytesWritten { get; set; }
	}
}
