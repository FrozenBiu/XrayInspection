using System;
using System.Collections.Generic;

namespace GUI_XRS
{
	// Token: 0x02000176 RID: 374
	public class BackupNamesComparer : IComparer<string>
	{
		// Token: 0x0600223D RID: 8765 RVA: 0x000DD7FC File Offset: 0x000DB9FC
		public int Compare(string item1, string item2)
		{
			string strB = item1.Replace("Full_", "").Replace("Parameter_", "").Replace("Cert_", "").Replace("Rep_", "");
			string text = item2.Replace("Full_", "").Replace("Parameter_", "").Replace("Cert_", "").Replace("Rep_", "");
			return text.CompareTo(strB);
		}
	}
}
