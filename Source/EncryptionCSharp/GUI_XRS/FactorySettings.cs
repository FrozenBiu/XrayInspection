using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using DatabaseConnection;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x02000115 RID: 277
	public class FactorySettings
	{
		// Token: 0x06001A93 RID: 6803 RVA: 0x0009EC0E File Offset: 0x0009CE0E
		public FactorySettings(DBConnection dbConnection, ProductManager productManager, Logbook logbook)
		{
			this.productManager = productManager;
			this.logbook = logbook;
			this.dbConnection = dbConnection;
		}

		// Token: 0x06001A94 RID: 6804 RVA: 0x0009EC30 File Offset: 0x0009CE30
		public void Reset(ParameterHandling parHandling)
		{
			bool flag = parHandling.ReadPar(XrayState.xrayPowerOnPath) == "1";
			if (flag)
			{
				Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtTurnXrayOff"), DialogStyle.Error, DialogButtons.OK);
			}
			else
			{
				this.prog = new Progress(LocalizationManager.Localize("txtErasingFiles"), false);
				this.prog.Show();
				this.bgWorker = new BackgroundWorker();
				this.bgWorker.WorkerReportsProgress = true;
				this.bgWorker.DoWork += this.bgWorker_DoWork;
				this.bgWorker.ProgressChanged += this.bgWorker_ProgressChanged;
				this.bgWorker.RunWorkerCompleted += this.bgWorker_RunWorkerCompleted;
				this.bgWorker.RunWorkerAsync();
			}
		}

		// Token: 0x06001A95 RID: 6805 RVA: 0x0009ED0C File Offset: 0x0009CF0C
		private void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			this.prog.FormFadeOut.Begin();
			this.productManager.ProductChanged += this.productManager_ProductChanged;
			this.productManager.ResetToFactorySettings();
			this.dbConnection.CreateDefaultUserTable();
			bool flag = Directory.Exists(StoreImageData.archiveRoot);
			if (flag)
			{
				string[] directories = Directory.GetDirectories(StoreImageData.archiveRoot);
				foreach (string path in directories)
				{
					Directory.Delete(path, true);
				}
			}
		}

		// Token: 0x06001A96 RID: 6806 RVA: 0x0009ED97 File Offset: 0x0009CF97
		private void productManager_ProductChanged(ProductData arg1, ProductData arg2)
		{
			this.productManager.ProductChanged -= this.productManager_ProductChanged;
			this.logbook.DeleteLogbook(true);
		}

		// Token: 0x06001A97 RID: 6807 RVA: 0x0009EDBF File Offset: 0x0009CFBF
		private void bgWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			this.prog.progBar.Value = (double)e.ProgressPercentage;
		}

		// Token: 0x06001A98 RID: 6808 RVA: 0x0009EDDC File Offset: 0x0009CFDC
		private void bgWorker_DoWork(object sender, DoWorkEventArgs e)
		{
			try
			{
				List<string> list = new List<string>();
				bool flag = Directory.Exists(StoreImageData.archiveRoot);
				if (flag)
				{
					list.AddRange(Directory.GetFiles(StoreImageData.archiveRoot, "*", SearchOption.AllDirectories));
				}
				bool flag2 = Directory.Exists(Backup.localBackupFolder);
				if (flag2)
				{
					list.AddRange(Directory.GetFiles(Backup.localBackupFolder, "*", SearchOption.AllDirectories));
				}
				bool flag3 = Directory.Exists(Backup.autoBackupFolder);
				if (flag3)
				{
					list.AddRange(Directory.GetFiles(Backup.autoBackupFolder, "*", SearchOption.AllDirectories));
				}
				for (int i = 0; i < list.Count; i++)
				{
					File.Delete(list[i]);
					this.bgWorker.ReportProgress(100 * (i + 1) / list.Count);
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x040011AE RID: 4526
		private Progress prog;

		// Token: 0x040011AF RID: 4527
		private BackgroundWorker bgWorker;

		// Token: 0x040011B0 RID: 4528
		private ProductManager productManager;

		// Token: 0x040011B1 RID: 4529
		private Logbook logbook;

		// Token: 0x040011B2 RID: 4530
		private DBConnection dbConnection;
	}
}
