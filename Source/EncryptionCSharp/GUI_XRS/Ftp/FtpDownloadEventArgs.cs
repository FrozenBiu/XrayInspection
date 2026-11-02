using System;

namespace GUI_XRS.Ftp
{
	// Token: 0x0200025E RID: 606
	public class FtpDownloadEventArgs : EventArgs
	{
		// Token: 0x06002EB0 RID: 11952 RVA: 0x00132B70 File Offset: 0x00130D70
		public FtpDownloadEventArgs(long totalBytes, long downloadedBytes)
		{
			this.totalBytes = totalBytes;
			this.downloadedBytes = downloadedBytes;
		}

		// Token: 0x17000C70 RID: 3184
		// (get) Token: 0x06002EB1 RID: 11953 RVA: 0x00132B88 File Offset: 0x00130D88
		public long TotalBytes
		{
			get
			{
				return this.totalBytes;
			}
		}

		// Token: 0x17000C71 RID: 3185
		// (get) Token: 0x06002EB2 RID: 11954 RVA: 0x00132BA0 File Offset: 0x00130DA0
		public long DownloadedBytes
		{
			get
			{
				return this.downloadedBytes;
			}
		}

		// Token: 0x04001D51 RID: 7505
		private readonly long totalBytes;

		// Token: 0x04001D52 RID: 7506
		private readonly long downloadedBytes;
	}
}
