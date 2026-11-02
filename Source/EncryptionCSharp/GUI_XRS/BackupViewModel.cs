using System;
using Common;
using DatabaseConnection;

namespace GUI_XRS
{
	// Token: 0x02000179 RID: 377
	public class BackupViewModel
	{
		// Token: 0x170009E9 RID: 2537
		// (get) Token: 0x0600225F RID: 8799 RVA: 0x000DE0A6 File Offset: 0x000DC2A6
		// (set) Token: 0x06002260 RID: 8800 RVA: 0x000DE0AE File Offset: 0x000DC2AE
		public RelayCommand ShowBackupOptionsCommand { get; set; }

		// Token: 0x170009EA RID: 2538
		// (get) Token: 0x06002261 RID: 8801 RVA: 0x000DE0B7 File Offset: 0x000DC2B7
		// (set) Token: 0x06002262 RID: 8802 RVA: 0x000DE0BF File Offset: 0x000DC2BF
		public RelayCommand ShowRestoreOptionsCommand { get; set; }

		// Token: 0x06002263 RID: 8803 RVA: 0x000DE0C8 File Offset: 0x000DC2C8
		public BackupViewModel(DBConnection dbConnection, ParameterHandling parHandling, Restore restore, Backup backup, AccessRightsManager accessManager, TestcardSimService testcardSimService, ProductManager productManager)
		{
			this.dbConnection = dbConnection;
			this.restore = restore;
			this.backup = backup;
			this.accessManager = accessManager;
			this.parHandling = parHandling;
			this.testcardSimService = testcardSimService;
			this.productManager = productManager;
			this.ShowBackupOptionsCommand = new RelayCommand(new Action<object>(this.ShowBackupOptions));
			this.ShowRestoreOptionsCommand = new RelayCommand(new Action<object>(this.ShowRestoreOptions));
		}

		// Token: 0x06002264 RID: 8804 RVA: 0x000DE144 File Offset: 0x000DC344
		public void ShowBackupOptions(object dummy)
		{
			CtrlBackupOptions ctrlBackupOptions = new CtrlBackupOptions(this.dbConnection, this.backup, this.parHandling, this.productManager);
			Subwindow.Show(ctrlBackupOptions, SubwindowButtons.BackAndHome, ctrlBackupOptions.Buttons);
		}

		// Token: 0x06002265 RID: 8805 RVA: 0x000DE180 File Offset: 0x000DC380
		public void ShowRestoreOptions(object dummy)
		{
			CtrlRestoreOptions ctrlRestoreOptions = new CtrlRestoreOptions(this.dbConnection, this.parHandling, this.restore, this.accessManager, this.testcardSimService);
			Subwindow.Show(ctrlRestoreOptions, SubwindowButtons.BackAndHome, ctrlRestoreOptions.Buttons);
		}

		// Token: 0x04001654 RID: 5716
		private DBConnection dbConnection;

		// Token: 0x04001655 RID: 5717
		private Restore restore;

		// Token: 0x04001656 RID: 5718
		private Backup backup;

		// Token: 0x04001657 RID: 5719
		private AccessRightsManager accessManager;

		// Token: 0x04001658 RID: 5720
		private ParameterHandling parHandling;

		// Token: 0x04001659 RID: 5721
		private TestcardSimService testcardSimService;

		// Token: 0x0400165A RID: 5722
		private ProductManager productManager;
	}
}
