using System;
using System.Collections.Generic;

namespace GUI_XRS
{
	// Token: 0x02000079 RID: 121
	public class DirectoriesComparer : IComparer<string>
	{
		// Token: 0x06000BDA RID: 3034 RVA: 0x0004695C File Offset: 0x00044B5C
		public int Compare(string item1, string item2)
		{
			string strB = item1.Replace("Full_", "").Replace("Parameter_", "").Replace("Cert_", "").Replace("Rep_", "").Replace("RP_", "");
			string text = item2.Replace("Full_", "").Replace("Parameter_", "").Replace("Cert_", "").Replace("Rep_", "").Replace("RP_", "");
			return text.CompareTo(strB);
		}
	}
}
