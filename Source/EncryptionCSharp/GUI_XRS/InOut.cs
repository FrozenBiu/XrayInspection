using System;

namespace GUI_XRS
{
	// Token: 0x02000109 RID: 265
	public class InOut
	{
		// Token: 0x1700075F RID: 1887
		// (get) Token: 0x060018DF RID: 6367 RVA: 0x000947BE File Offset: 0x000929BE
		// (set) Token: 0x060018E0 RID: 6368 RVA: 0x000947C6 File Offset: 0x000929C6
		public string IOPath { get; set; }

		// Token: 0x17000760 RID: 1888
		// (get) Token: 0x060018E1 RID: 6369 RVA: 0x000947CF File Offset: 0x000929CF
		// (set) Token: 0x060018E2 RID: 6370 RVA: 0x000947D7 File Offset: 0x000929D7
		public string LocalizationString { get; set; }

		// Token: 0x17000761 RID: 1889
		// (get) Token: 0x060018E3 RID: 6371 RVA: 0x000947E0 File Offset: 0x000929E0
		// (set) Token: 0x060018E4 RID: 6372 RVA: 0x000947E8 File Offset: 0x000929E8
		public int SwitchID { get; set; }

		// Token: 0x060018E5 RID: 6373 RVA: 0x000947F1 File Offset: 0x000929F1
		public InOut(string ioPath, string localizationString) : this(ioPath, localizationString, -1)
		{
		}

		// Token: 0x060018E6 RID: 6374 RVA: 0x000947FE File Offset: 0x000929FE
		public InOut(string ioPath, string localizationString, int switchID)
		{
			this.IOPath = ioPath;
			this.LocalizationString = localizationString;
			this.SwitchID = switchID;
		}
	}
}
