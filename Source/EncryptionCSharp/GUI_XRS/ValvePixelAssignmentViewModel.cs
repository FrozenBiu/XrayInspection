using System;
using System.Collections.Generic;
using DatabaseConnection;

namespace GUI_XRS
{
	// Token: 0x02000056 RID: 86
	public class ValvePixelAssignmentViewModel
	{
		// Token: 0x1700028A RID: 650
		// (get) Token: 0x060008B7 RID: 2231 RVA: 0x000376B2 File Offset: 0x000358B2
		// (set) Token: 0x060008B8 RID: 2232 RVA: 0x000376BA File Offset: 0x000358BA
		public int ValveWidth { get; set; }

		// Token: 0x1700028B RID: 651
		// (get) Token: 0x060008B9 RID: 2233 RVA: 0x000376C3 File Offset: 0x000358C3
		// (set) Token: 0x060008BA RID: 2234 RVA: 0x000376CB File Offset: 0x000358CB
		public List<ValvePixelAssignment> Assignments { get; set; }

		// Token: 0x060008BB RID: 2235 RVA: 0x000376D4 File Offset: 0x000358D4
		public ValvePixelAssignmentViewModel(DBConnection dbConnection, ParameterHandling parHandling, ImageDataTransfer imageTransfer)
		{
			this.dbConnection = dbConnection;
			this.parHandling = parHandling;
			this.imageTransfer = imageTransfer;
			this.Assignments = new List<ValvePixelAssignment>();
		}

		// Token: 0x040005ED RID: 1517
		private DBConnection dbConnection;

		// Token: 0x040005EE RID: 1518
		private ParameterHandling parHandling;

		// Token: 0x040005EF RID: 1519
		private ImageDataTransfer imageTransfer;
	}
}
