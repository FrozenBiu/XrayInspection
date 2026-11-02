using System;

namespace GUI_XRS
{
	// Token: 0x02000173 RID: 371
	public class BackupData
	{
		// Token: 0x170009D8 RID: 2520
		// (get) Token: 0x06002222 RID: 8738 RVA: 0x000DD590 File Offset: 0x000DB790
		// (set) Token: 0x06002223 RID: 8739 RVA: 0x000DD598 File Offset: 0x000DB798
		public string BackupName { get; set; }

		// Token: 0x170009D9 RID: 2521
		// (get) Token: 0x06002224 RID: 8740 RVA: 0x000DD5A1 File Offset: 0x000DB7A1
		// (set) Token: 0x06002225 RID: 8741 RVA: 0x000DD5A9 File Offset: 0x000DB7A9
		public string BackupPath { get; set; }

		// Token: 0x170009DA RID: 2522
		// (get) Token: 0x06002226 RID: 8742 RVA: 0x000DD5B2 File Offset: 0x000DB7B2
		// (set) Token: 0x06002227 RID: 8743 RVA: 0x000DD5BA File Offset: 0x000DB7BA
		public bool FromUSB { get; set; }

		// Token: 0x170009DB RID: 2523
		// (get) Token: 0x06002228 RID: 8744 RVA: 0x000DD5C3 File Offset: 0x000DB7C3
		// (set) Token: 0x06002229 RID: 8745 RVA: 0x000DD5CB File Offset: 0x000DB7CB
		public bool FromHarddisk { get; set; }

		// Token: 0x170009DC RID: 2524
		// (get) Token: 0x0600222A RID: 8746 RVA: 0x000DD5D4 File Offset: 0x000DB7D4
		// (set) Token: 0x0600222B RID: 8747 RVA: 0x000DD5DC File Offset: 0x000DB7DC
		public bool AutoBackup { get; set; }

		// Token: 0x0600222C RID: 8748 RVA: 0x000DD5E5 File Offset: 0x000DB7E5
		public BackupData() : this("", "")
		{
		}

		// Token: 0x0600222D RID: 8749 RVA: 0x000DD5F9 File Offset: 0x000DB7F9
		public BackupData(string backupName, string backupPath)
		{
			this.BackupName = backupName;
			this.BackupPath = backupPath;
		}
	}
}
