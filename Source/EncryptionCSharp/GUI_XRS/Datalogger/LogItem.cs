using System;

namespace GUI_XRS.Datalogger
{
	// Token: 0x0200026D RID: 621
	public class LogItem
	{
		// Token: 0x17000CC9 RID: 3273
		// (get) Token: 0x06002FC0 RID: 12224 RVA: 0x0013BC31 File Offset: 0x00139E31
		// (set) Token: 0x06002FC1 RID: 12225 RVA: 0x0013BC39 File Offset: 0x00139E39
		public DateTime Timestamp { get; set; }

		// Token: 0x17000CCA RID: 3274
		// (get) Token: 0x06002FC2 RID: 12226 RVA: 0x0013BC42 File Offset: 0x00139E42
		// (set) Token: 0x06002FC3 RID: 12227 RVA: 0x0013BC4A File Offset: 0x00139E4A
		public string Path { get; set; }

		// Token: 0x17000CCB RID: 3275
		// (get) Token: 0x06002FC4 RID: 12228 RVA: 0x0013BC53 File Offset: 0x00139E53
		// (set) Token: 0x06002FC5 RID: 12229 RVA: 0x0013BC5B File Offset: 0x00139E5B
		public string Value { get; set; }

		// Token: 0x06002FC6 RID: 12230 RVA: 0x0013BC64 File Offset: 0x00139E64
		public LogItem() : this(DateTime.Now, "", "")
		{
		}

		// Token: 0x06002FC7 RID: 12231 RVA: 0x0013BC7D File Offset: 0x00139E7D
		public LogItem(DateTime timestamp, string path, string value)
		{
			this.Timestamp = timestamp;
			this.Path = path;
			this.Value = value;
		}
	}
}
