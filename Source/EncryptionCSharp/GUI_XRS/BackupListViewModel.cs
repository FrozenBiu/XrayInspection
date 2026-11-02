using System;
using Common;

namespace GUI_XRS
{
	// Token: 0x02000032 RID: 50
	internal class BackupListViewModel
	{
		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x06000608 RID: 1544 RVA: 0x000295F0 File Offset: 0x000277F0
		// (set) Token: 0x06000609 RID: 1545 RVA: 0x000295F8 File Offset: 0x000277F8
		public ProductManager ProductManager { get; set; }

		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x0600060A RID: 1546 RVA: 0x00029601 File Offset: 0x00027801
		// (set) Token: 0x0600060B RID: 1547 RVA: 0x00029609 File Offset: 0x00027809
		public RelayCommand CleanUpCommand { get; set; }

		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x0600060C RID: 1548 RVA: 0x00029612 File Offset: 0x00027812
		// (set) Token: 0x0600060D RID: 1549 RVA: 0x0002961A File Offset: 0x0002781A
		public Backup Backup { get; set; }

		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x0600060E RID: 1550 RVA: 0x00029623 File Offset: 0x00027823
		// (set) Token: 0x0600060F RID: 1551 RVA: 0x0002962B File Offset: 0x0002782B
		public ParameterHandling parHandling { get; set; }

		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x06000610 RID: 1552 RVA: 0x00029634 File Offset: 0x00027834
		// (set) Token: 0x06000611 RID: 1553 RVA: 0x0002964C File Offset: 0x0002784C
		public string AdditionalBackupPath
		{
			get
			{
				return this.additionalBackupPath;
			}
			set
			{
				this.additionalBackupPath = value;
			}
		}

		// Token: 0x06000612 RID: 1554 RVA: 0x00029658 File Offset: 0x00027858
		public BackupListViewModel(Backup backup, ProductManager productManager, ParameterHandling parHandling)
		{
			this.Backup = backup;
			this.ProductManager = productManager;
			this.parHandling = parHandling;
			productManager.SelectedProduct = productManager.CurrentLoadedProduct;
			productManager.ViewProducts.MoveCurrentTo(productManager.CurrentLoadedProduct);
			this.CleanUpCommand = new RelayCommand(new Action<object>(this.CleanUp));
		}

		// Token: 0x06000613 RID: 1555 RVA: 0x000296C8 File Offset: 0x000278C8
		public void DoBackup(string[] selectedModels)
		{
			bool flag = this.parHandling != null && (this.parHandling.ReadPar(XrayState.xrayPowerOnPath) == "1" || this.parHandling.ReadPar(XrayState.xrayStarterPath) == "1");
			if (flag)
			{
				Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtTurnXrayOff"), DialogStyle.Error, DialogButtons.OK);
			}
			else
			{
				bool flag2 = this.additionalBackupPath == "";
				if (flag2)
				{
					this.Backup.DoBackup(selectedModels);
				}
				else
				{
					this.Backup.DoBackup(this.additionalBackupPath, selectedModels);
				}
			}
		}

		// Token: 0x06000614 RID: 1556 RVA: 0x0001092C File Offset: 0x0000EB2C
		private void CleanUp(object obj)
		{
		}

		// Token: 0x0400046C RID: 1132
		private string additionalBackupPath = "";
	}
}
