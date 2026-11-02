using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using Common;
using DatabaseConnection;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x02000178 RID: 376
	public class BackupOptionsViewModel : INotifyPropertyChanged
	{
		// Token: 0x170009E1 RID: 2529
		// (get) Token: 0x06002241 RID: 8769 RVA: 0x000DD93F File Offset: 0x000DBB3F
		// (set) Token: 0x06002242 RID: 8770 RVA: 0x000DD947 File Offset: 0x000DBB47
		public Backup Backup { get; set; }

		// Token: 0x140000B6 RID: 182
		// (add) Token: 0x06002243 RID: 8771 RVA: 0x000DD950 File Offset: 0x000DBB50
		// (remove) Token: 0x06002244 RID: 8772 RVA: 0x000DD988 File Offset: 0x000DBB88
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x170009E2 RID: 2530
		// (get) Token: 0x06002245 RID: 8773 RVA: 0x000DD9BD File Offset: 0x000DBBBD
		// (set) Token: 0x06002246 RID: 8774 RVA: 0x000DD9C5 File Offset: 0x000DBBC5
		public BoundLocalizedText StrSaveProducts { get; set; }

		// Token: 0x170009E3 RID: 2531
		// (get) Token: 0x06002247 RID: 8775 RVA: 0x000DD9CE File Offset: 0x000DBBCE
		// (set) Token: 0x06002248 RID: 8776 RVA: 0x000DD9D6 File Offset: 0x000DBBD6
		public BoundLocalizedText StrSaveSystem { get; set; }

		// Token: 0x170009E4 RID: 2532
		// (get) Token: 0x06002249 RID: 8777 RVA: 0x000DD9DF File Offset: 0x000DBBDF
		// (set) Token: 0x0600224A RID: 8778 RVA: 0x000DD9E7 File Offset: 0x000DBBE7
		public BoundLocalizedText StrSaveAnomalyDetectionImages { get; set; }

		// Token: 0x170009E5 RID: 2533
		// (get) Token: 0x0600224B RID: 8779 RVA: 0x000DD9F0 File Offset: 0x000DBBF0
		// (set) Token: 0x0600224C RID: 8780 RVA: 0x000DD9F8 File Offset: 0x000DBBF8
		public BoundLocalizedText StrDoBackup { get; set; }

		// Token: 0x170009E6 RID: 2534
		// (get) Token: 0x0600224D RID: 8781 RVA: 0x000DDA01 File Offset: 0x000DBC01
		// (set) Token: 0x0600224E RID: 8782 RVA: 0x000DDA09 File Offset: 0x000DBC09
		public RelayCommand DoBackupCommand { get; set; }

		// Token: 0x170009E7 RID: 2535
		// (get) Token: 0x0600224F RID: 8783 RVA: 0x000DDA12 File Offset: 0x000DBC12
		// (set) Token: 0x06002250 RID: 8784 RVA: 0x000DDA1A File Offset: 0x000DBC1A
		public RelayCommand CleanUpCommand { get; set; }

		// Token: 0x06002251 RID: 8785 RVA: 0x000DDA24 File Offset: 0x000DBC24
		public BackupOptionsViewModel(DBConnection dbConnection, Backup backup, ParameterHandling parHandling, ProductManager productManager)
		{
			this.parHandling = parHandling;
			this.productManager = productManager;
			this.dbConnection = dbConnection;
			this.Backup = backup;
			this.Backup.BackupCompleted += this.backup_BackupCompleted;
			this.DoBackupCommand = new RelayCommand(new Action<object>(this.DoBackup));
			this.CleanUpCommand = new RelayCommand(new Action<object>(this.CleanUp));
			this.RegisterLocalizedText();
			dbConnection.GetUsbDriveLetter(ref this.usbLetter);
			this.usbLetter += "\\";
			bool flag = parHandling.ReadPar(Definition.anomalyDetectionEnabled) == "1" && parHandling.ReadPar(Definition.imgprocarchitecture) == "1";
			if (flag)
			{
				this.ShowSaveAnomalyDetectionImages = true;
			}
		}

		// Token: 0x06002252 RID: 8786 RVA: 0x000DDB2A File Offset: 0x000DBD2A
		public BackupOptionsViewModel(DBConnection dbConnection, Backup backup, ParameterHandling parHandling, ProductManager productManager, bool autoCleanup) : this(dbConnection, backup, parHandling, productManager)
		{
			this.autoCleanup = autoCleanup;
		}

		// Token: 0x06002253 RID: 8787 RVA: 0x000DDB44 File Offset: 0x000DBD44
		private void backup_BackupCompleted(bool success, string[] backupPaths)
		{
			this.dbConnection.Connect(DBConnection.databasePath);
			if (success)
			{
				string text = LocalizationManager.Localize("txtBackupSuccess") + Environment.NewLine + Environment.NewLine;
				bool flag = backupPaths != null && backupPaths.Length != 0;
				if (flag)
				{
					for (int i = 0; i < backupPaths.Length - 1; i++)
					{
						text = string.Concat(new string[]
						{
							text,
							LocalizationManager.Localize("txtBackupTo"),
							" ",
							backupPaths[i],
							Environment.NewLine,
							Environment.NewLine
						});
					}
					text = text + LocalizationManager.Localize("txtBackupTo") + " " + backupPaths[backupPaths.Length - 1];
				}
				bool flag2 = this.autoCleanup;
				if (flag2)
				{
					Dialog.Show(LocalizationManager.Localize("txtSuccess"), text, DialogStyle.Ok, DialogButtons.OK, true);
				}
				else
				{
					Dialog.Show(LocalizationManager.Localize("txtSuccess"), text, DialogStyle.Ok, DialogButtons.OK);
				}
			}
			else
			{
				Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtBackupFailed"), DialogStyle.Error, DialogButtons.OK);
			}
			bool flag3 = this.autoCleanup;
			if (flag3)
			{
				this.CleanUp(null);
			}
		}

		// Token: 0x06002254 RID: 8788 RVA: 0x000DDC74 File Offset: 0x000DBE74
		private void RegisterLocalizedText()
		{
			this.StrSaveProducts = new BoundLocalizedText("txtSaveDatabase");
			this.StrSaveSystem = new BoundLocalizedText("txtSaveSystem");
			this.StrDoBackup = new BoundLocalizedText("txtDoBackup");
			this.StrSaveAnomalyDetectionImages = new BoundLocalizedText("txtSaveAIImages");
		}

		// Token: 0x06002255 RID: 8789 RVA: 0x000DDCC8 File Offset: 0x000DBEC8
		private void CleanUp(object obj)
		{
			MainWindow.localizationManager.Deregister(this.StrSaveProducts);
			MainWindow.localizationManager.Deregister(this.StrSaveSystem);
			MainWindow.localizationManager.Deregister(this.StrDoBackup);
			MainWindow.localizationManager.Deregister(this.StrSaveAnomalyDetectionImages);
			this.Backup.BackupCompleted -= this.backup_BackupCompleted;
			this.StopUsbSurvey();
		}

		// Token: 0x06002256 RID: 8790 RVA: 0x000DDD3C File Offset: 0x000DBF3C
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x06002257 RID: 8791 RVA: 0x000DDD6C File Offset: 0x000DBF6C
		public void DoBackup(object dummy)
		{
			bool flag = !this.Backup.SaveProducts;
			if (flag)
			{
				Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtNoOptionChosen"), DialogStyle.Warning);
			}
			else
			{
				bool flag2 = Directory.Exists(this.usbLetter) && this.usbLetter != "\\";
				if (flag2)
				{
					string text = this.usbLetter + Backup.backupFolderName;
					bool flag3 = !Directory.Exists(text);
					if (flag3)
					{
						Directory.CreateDirectory(text);
					}
					bool flag4 = this.parHandling.ReadPar(Definition.anomalyDetectionEnabled) == "1" || this.parHandling.ReadPar(Definition.imgprocarchitecture) == "1" || this.parHandling.ReadPar(Definition.objectDetectionEnabled) == "1";
					if (flag4)
					{
						CtrlBackupList ctrlBackupList = new CtrlBackupList(this.productManager, this.Backup, this.parHandling);
						ctrlBackupList.AddBackupPath(text);
						Subwindow.Show(ctrlBackupList, SubwindowButtons.BackAndHome, ctrlBackupList.Buttons);
					}
					else
					{
						this.Backup.DoBackup(text, null);
					}
				}
				else
				{
					bool flag5 = this.parHandling.ReadPar(Definition.anomalyDetectionEnabled) == "1" || this.parHandling.ReadPar(Definition.imgprocarchitecture) == "1" || this.parHandling.ReadPar(Definition.objectDetectionEnabled) == "1";
					if (flag5)
					{
						CtrlBackupList ctrlBackupList2 = new CtrlBackupList(this.productManager, this.Backup, this.parHandling);
						Subwindow.Show(ctrlBackupList2, SubwindowButtons.BackAndHome, ctrlBackupList2.Buttons);
					}
					else
					{
						this.Backup.DoBackup(null);
					}
				}
			}
		}

		// Token: 0x06002258 RID: 8792 RVA: 0x000DDF38 File Offset: 0x000DC138
		public void StartUsbSurvey(FrameworkElement element)
		{
			this.usbSurveyRunning = true;
			this.usbElement = element;
			new Thread(new ThreadStart(this.UsbSurvey))
			{
				Name = "UsbSurveyBackup"
			}.Start();
		}

		// Token: 0x06002259 RID: 8793 RVA: 0x000DDF79 File Offset: 0x000DC179
		public void StopUsbSurvey()
		{
			this.usbSurveyRunning = false;
		}

		// Token: 0x0600225A RID: 8794 RVA: 0x000DDF84 File Offset: 0x000DC184
		private void UsbSurvey()
		{
			try
			{
				while (this.usbSurveyRunning)
				{
					this.SetUsbVisibility();
					Thread.Sleep(this.surveyPeriod);
				}
			}
			catch (Exception ex)
			{
				Exception ex3;
				Exception ex2 = ex3;
				Exception ex = ex2;
				bool flag = Application.Current != null;
				if (flag)
				{
					Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate()
					{
						Dialog.Show("Error", ex.Message, DialogStyle.Error, DialogButtons.OK);
					}));
				}
			}
		}

		// Token: 0x0600225B RID: 8795 RVA: 0x000DE004 File Offset: 0x000DC204
		private void SetUsbVisibility()
		{
			Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
			{
				bool flag = this.usbLetter != "\\" && Directory.Exists(this.usbLetter);
				if (flag)
				{
					this.usbElement.Visibility = Visibility.Visible;
				}
				else
				{
					this.usbElement.Visibility = Visibility.Hidden;
				}
			}));
		}

		// Token: 0x170009E8 RID: 2536
		// (get) Token: 0x0600225C RID: 8796 RVA: 0x000DE028 File Offset: 0x000DC228
		// (set) Token: 0x0600225D RID: 8797 RVA: 0x000DE040 File Offset: 0x000DC240
		public bool ShowSaveAnomalyDetectionImages
		{
			get
			{
				return this.showSaveAnomalyDetectionImages;
			}
			set
			{
				this.showSaveAnomalyDetectionImages = value;
				this.OnPropertyChanged("ShowSaveAnomalyDetectionImages");
			}
		}

		// Token: 0x04001643 RID: 5699
		private DBConnection dbConnection;

		// Token: 0x04001644 RID: 5700
		private string usbLetter = "";

		// Token: 0x04001645 RID: 5701
		private bool usbSurveyRunning;

		// Token: 0x04001646 RID: 5702
		private FrameworkElement usbElement;

		// Token: 0x04001647 RID: 5703
		private int surveyPeriod = 1000;

		// Token: 0x04001650 RID: 5712
		private bool showSaveAnomalyDetectionImages = false;

		// Token: 0x04001651 RID: 5713
		private ParameterHandling parHandling;

		// Token: 0x04001652 RID: 5714
		private ProductManager productManager;

		// Token: 0x04001653 RID: 5715
		private bool autoCleanup = false;
	}
}
