using System;
using DatabaseConnection;

namespace GUI_XRS
{
	// Token: 0x02000143 RID: 323
	public class QualityCntrlDoc
	{
		// Token: 0x06001EDB RID: 7899 RVA: 0x000CE434 File Offset: 0x000CC634
		public QualityCntrlDoc(DBConnection dbConnection)
		{
			this.dbConnection = dbConnection;
		}

		// Token: 0x06001EDC RID: 7900 RVA: 0x0001092C File Offset: 0x0000EB2C
		public void CreateDoc(string docName)
		{
		}

		// Token: 0x0400142F RID: 5167
		private DBConnection dbConnection;
	}
}
