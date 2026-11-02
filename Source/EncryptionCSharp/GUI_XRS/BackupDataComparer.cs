using System;
using System.Collections.Generic;

namespace GUI_XRS
{
	// Token: 0x02000177 RID: 375
	public class BackupDataComparer : IComparer<BackupData>
	{
		// Token: 0x0600223F RID: 8767 RVA: 0x000DD894 File Offset: 0x000DBA94
		public int Compare(BackupData item1, BackupData item2)
		{
			string strB = item1.BackupName.Replace("Full_", "").Replace("Parameter_", "").Replace("Cert_", "").Replace("Rep_", "");
			string text = item2.BackupName.Replace("Full_", "").Replace("Parameter_", "").Replace("Cert_", "").Replace("Rep_", "");
			return text.CompareTo(strB);
		}
	}
}
