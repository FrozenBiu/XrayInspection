using System;
using System.Collections.Generic;

namespace GUI_XRS
{
	// Token: 0x02000075 RID: 117
	internal class DocumentsComparer : IComparer<Document>
	{
		// Token: 0x06000AF6 RID: 2806 RVA: 0x00040BE4 File Offset: 0x0003EDE4
		public int Compare(Document item1, Document item2)
		{
			string strB = item1.DisplayedName.Replace("Full_", "").Replace("Parameter_", "").Replace("Cert_", "").Replace("Rep_", "");
			string text = item2.DisplayedName.Replace("Full_", "").Replace("Parameter_", "").Replace("Cert_", "").Replace("Rep_", "");
			return text.CompareTo(strB);
		}
	}
}
