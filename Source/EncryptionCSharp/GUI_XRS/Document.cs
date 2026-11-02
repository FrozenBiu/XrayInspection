using System;
using System.IO;

namespace GUI_XRS
{
	// Token: 0x0200009E RID: 158
	public class Document
	{
		// Token: 0x170004BD RID: 1213
		// (get) Token: 0x06000FC0 RID: 4032 RVA: 0x00061FBC File Offset: 0x000601BC
		// (set) Token: 0x06000FC1 RID: 4033 RVA: 0x00061FC4 File Offset: 0x000601C4
		public int Index { get; set; }

		// Token: 0x170004BE RID: 1214
		// (get) Token: 0x06000FC2 RID: 4034 RVA: 0x00061FCD File Offset: 0x000601CD
		// (set) Token: 0x06000FC3 RID: 4035 RVA: 0x00061FD5 File Offset: 0x000601D5
		public string DisplayedName { get; set; }

		// Token: 0x170004BF RID: 1215
		// (get) Token: 0x06000FC4 RID: 4036 RVA: 0x00061FE0 File Offset: 0x000601E0
		public string FilePath
		{
			get
			{
				return this.filePath;
			}
		}

		// Token: 0x06000FC5 RID: 4037 RVA: 0x00061FF8 File Offset: 0x000601F8
		public Document(string filePath, int index)
		{
			this.filePath = filePath;
			this.Index = index;
			this.DisplayedName = Path.GetFileNameWithoutExtension(filePath);
		}

		// Token: 0x04000A8F RID: 2703
		private string filePath;
	}
}
