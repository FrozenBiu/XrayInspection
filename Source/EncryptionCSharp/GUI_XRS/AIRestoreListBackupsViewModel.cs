using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Common;
using DatabaseConnection;
using Definitions;
using ICSharpCode.SharpZipLib.Zip;

namespace GUI_XRS
{
	// Token: 0x0200000D RID: 13
	internal class AIRestoreListBackupsViewModel : INotifyPropertyChanged
	{
		// Token: 0x17000023 RID: 35
		// (get) Token: 0x0600008C RID: 140 RVA: 0x0000489B File Offset: 0x00002A9B
		// (set) Token: 0x0600008D RID: 141 RVA: 0x000048A3 File Offset: 0x00002AA3
		public BoundLocalizedText StrShowOnlyValidBackups { get; set; }

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x0600008E RID: 142 RVA: 0x000048AC File Offset: 0x00002AAC
		// (set) Token: 0x0600008F RID: 143 RVA: 0x000048B4 File Offset: 0x00002AB4
		public RelayCommand CreateBackupListCommand { get; set; }

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000090 RID: 144 RVA: 0x000048BD File Offset: 0x00002ABD
		// (set) Token: 0x06000091 RID: 145 RVA: 0x000048C5 File Offset: 0x00002AC5
		public RelayCommand DoRestoreCommand { get; set; }

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000092 RID: 146 RVA: 0x000048CE File Offset: 0x00002ACE
		// (set) Token: 0x06000093 RID: 147 RVA: 0x000048D6 File Offset: 0x00002AD6
		public RelayCommand CleanUpCommand { get; set; }

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x06000094 RID: 148 RVA: 0x000048E0 File Offset: 0x00002AE0
		// (remove) Token: 0x06000095 RID: 149 RVA: 0x00004918 File Offset: 0x00002B18
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000096 RID: 150 RVA: 0x00004950 File Offset: 0x00002B50
		// (set) Token: 0x06000097 RID: 151 RVA: 0x00004968 File Offset: 0x00002B68
		public bool ShowOnlyValidBackups
		{
			get
			{
				return this.showOnlyValidBackups;
			}
			set
			{
				this.showOnlyValidBackups = value;
				this.OnPropertyChanged("ShowOnlyValidBackups");
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000098 RID: 152 RVA: 0x00004980 File Offset: 0x00002B80
		// (set) Token: 0x06000099 RID: 153 RVA: 0x00004998 File Offset: 0x00002B98
		public BackupData SelectedBackup
		{
			get
			{
				return this.selectedBackup;
			}
			set
			{
				bool flag = this.selectedBackup != value;
				if (flag)
				{
					this.selectedBackup = value;
					this.OnPropertyChanged("SelectedBackup");
				}
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x0600009A RID: 154 RVA: 0x000049CC File Offset: 0x00002BCC
		// (set) Token: 0x0600009B RID: 155 RVA: 0x000049E4 File Offset: 0x00002BE4
		public ObservableCollection<BackupData> BackupList
		{
			get
			{
				return this.backupList;
			}
			set
			{
				this.backupList = value;
				this.OnPropertyChanged("BackupList");
			}
		}

		// Token: 0x0600009C RID: 156 RVA: 0x000049FC File Offset: 0x00002BFC
		public AIRestoreListBackupsViewModel(DBConnection dbConnection, ParameterHandling parHandling, bool restoreThinkXA, bool restoreThinkXO, ProductManager productManager)
		{
			this.BackupList = new ObservableCollection<BackupData>();
			this.restoreThinkXA = restoreThinkXA;
			this.restoreThinkXO = restoreThinkXO;
			this.dbConnection = dbConnection;
			this.parHandling = parHandling;
			this.productManager = productManager;
			this.DoRestoreCommand = new RelayCommand(new Action<object>(this.DoRestore));
			this.CreateBackupListCommand = new RelayCommand(new Action<object>(this.CreateBackupList));
			this.CleanUpCommand = new RelayCommand(new Action<object>(this.CleanUp));
			this.RegisterLocalizedText();
			AIRestoreListBackupsViewModel.GetRequiredModelFiles(parHandling, restoreThinkXA, restoreThinkXO, out this.requiredFiles);
			AIRestoreListBackupsViewModel.BackupModelFolders = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00004ABC File Offset: 0x00002CBC
		public static void GetRequiredModelFiles(ParameterHandling parHandling, bool restoreThinkXA, bool restoreThinkXO, out HashSet<string> requiredFiles)
		{
			string text = parHandling.ReadPar(Definition.machineSubTypePath);
			bool flag = restoreThinkXA && restoreThinkXO;
			if (flag)
			{
				requiredFiles = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
				{
					"model.engine",
					"modelXO.engine"
				};
				bool flag2 = text.StartsWith("HX");
				if (flag2)
				{
					requiredFiles.Add("modelHX.weights");
					requiredFiles.Add("featureHX.weights");
				}
				else
				{
					requiredFiles.Add("modelTX.weights");
					requiredFiles.Add("featureTX.weights");
				}
			}
			else
			{
				bool flag3 = restoreThinkXA && !restoreThinkXO;
				if (flag3)
				{
					requiredFiles = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
					{
						"model.engine"
					};
					bool flag4 = text.StartsWith("HX");
					if (flag4)
					{
						requiredFiles.Add("modelHX.weights");
						requiredFiles.Add("featureHX.weights");
					}
					else
					{
						requiredFiles.Add("modelTX.weights");
						requiredFiles.Add("featureTX.weights");
					}
				}
				else
				{
					bool flag5 = !restoreThinkXA && restoreThinkXO;
					if (flag5)
					{
						requiredFiles = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
						{
							"modelXO.engine"
						};
					}
					else
					{
						requiredFiles = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
					}
				}
			}
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00004BF8 File Offset: 0x00002DF8
		private void CreateBackupList(object obj)
		{
			List<string> backupDirs = new List<string>();
			this.AddBackupDirs(backupDirs);
			List<BackupData> backupFiles = this.GetBackupFiles(backupDirs);
			backupFiles.Sort(new BackupDataComparer());
			for (int i = 0; i < backupFiles.Count; i++)
			{
				bool flag = !this.CheckAvailabilityModelFiles(backupFiles[i]);
				if (flag)
				{
					this.ShowOnlyValidBackups = true;
				}
				else
				{
					this.BackupList.Add(backupFiles[i]);
				}
			}
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00004C74 File Offset: 0x00002E74
		private void AddBackupDirs(List<string> backupDirs)
		{
			bool flag = Directory.Exists(Backup.localBackupFolder);
			if (flag)
			{
				backupDirs.Add(Backup.localBackupFolder);
			}
			this.dbConnection.GetUsbDriveLetter(ref this.usbLetter);
			this.usbLetter += "\\";
			bool flag2 = Directory.Exists(this.usbLetter) && this.usbLetter != "\\";
			if (flag2)
			{
				backupDirs.Add(this.usbLetter);
			}
			bool flag3 = Directory.Exists(this.usbLetter + Backup.backupFolderName);
			if (flag3)
			{
				backupDirs.Add(this.usbLetter + Backup.backupFolderName);
			}
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00004D28 File Offset: 0x00002F28
		private List<BackupData> GetBackupFiles(List<string> backupDirs)
		{
			List<BackupData> list = new List<BackupData>();
			for (int i = 0; i < backupDirs.Count; i++)
			{
				foreach (string text in Directory.GetFiles(backupDirs[i]))
				{
					bool flag = true;
					string fileName = Path.GetFileName(text);
					bool flag2 = this.IsValidBackup(fileName);
					if (flag2)
					{
						int k;
						for (k = 0; k < list.Count; k++)
						{
							bool flag3 = fileName.Remove(0, Backup.zipBackupPreName.Length) == list[k].BackupName;
							if (flag3)
							{
								flag = false;
								break;
							}
						}
						bool flag4 = flag;
						if (flag4)
						{
							BackupData item = new BackupData(fileName.Remove(0, Backup.zipBackupPreName.Length), text);
							list.Add(item);
						}
						bool flag5 = text.Contains(this.usbLetter) && this.usbLetter != "\\";
						if (flag5)
						{
							list[k].FromUSB = true;
						}
						else
						{
							list[k].FromHarddisk = true;
						}
						bool flag6 = text.Contains(Backup.autoBackupFolderName);
						if (flag6)
						{
							list[k].AutoBackup = true;
						}
					}
				}
			}
			return list;
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00004E90 File Offset: 0x00003090
		private bool IsValidBackup(string backupName)
		{
			return backupName.StartsWith(Backup.zipBackupPreName) && backupName.EndsWith(Definition.zipFileEnding);
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00004EC8 File Offset: 0x000030C8
		private bool CheckAvailabilityModelFiles(BackupData backup)
		{
			List<string> list = new List<string>();
			Dictionary<string, HashSet<string>> dictionary = new Dictionary<string, HashSet<string>>(StringComparer.OrdinalIgnoreCase);
			using (FileStream fileStream = File.OpenRead(backup.BackupPath))
			{
				using (ZipFile zipFile = new ZipFile(fileStream))
				{
					zipFile.Password = Backup.zipPassword;
					foreach (object obj in zipFile)
					{
						ZipEntry zipEntry = (ZipEntry)obj;
						bool flag = !zipEntry.IsFile || !zipEntry.Name.StartsWith(AIRestoreListBackupsViewModel.pathModelFiles, StringComparison.OrdinalIgnoreCase);
						if (!flag)
						{
							string text = zipEntry.Name.Substring(AIRestoreListBackupsViewModel.pathModelFiles.Length);
							string[] array = text.Split(new char[]
							{
								'/'
							}, 2);
							bool flag2 = array.Length != 2;
							if (!flag2)
							{
								string key = array[0];
								string fileName = Path.GetFileName(array[1]);
								bool flag3 = !dictionary.ContainsKey(key);
								if (flag3)
								{
									dictionary[key] = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
								}
								dictionary[key].Add(fileName);
							}
						}
					}
				}
			}
			bool result = false;
			foreach (KeyValuePair<string, HashSet<string>> keyValuePair in dictionary)
			{
				HashSet<string> filesInFolder = keyValuePair.Value;
				bool flag4 = this.requiredFiles.All((string req) => filesInFolder.Contains(req));
				if (flag4)
				{
					list.Add(keyValuePair.Key);
					result = true;
				}
			}
			bool flag5 = AIRestoreListBackupsViewModel.BackupModelFolders.ContainsKey(backup.BackupName);
			if (flag5)
			{
				AIRestoreListBackupsViewModel.BackupModelFolders[backup.BackupName] = list;
			}
			else
			{
				AIRestoreListBackupsViewModel.BackupModelFolders.Add(backup.BackupName, list);
			}
			return result;
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00005138 File Offset: 0x00003338
		private void DoRestore(object obj)
		{
			bool flag = this.SelectedBackup != null && this.SelectedBackup.BackupPath != null && this.SelectedBackup.BackupPath != "";
			if (flag)
			{
				CtrlAIRestoreListProducts ctrlAIRestoreListProducts = new CtrlAIRestoreListProducts(this.SelectedBackup.BackupPath, AIRestoreListBackupsViewModel.BackupModelFolders[this.SelectedBackup.BackupName], this.parHandling, this.dbConnection, this.restoreThinkXA, this.restoreThinkXO, this.productManager);
				Subwindow.Show(ctrlAIRestoreListProducts, SubwindowButtons.BackAndHome, ctrlAIRestoreListProducts.Buttons);
			}
			else
			{
				Dialog.Show(LocalizationManager.Localize("txtError"), "No backup chosen!", DialogStyle.Error);
			}
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x000051E4 File Offset: 0x000033E4
		private void CleanUp(object obj)
		{
			bool flag = this.BackupList != null;
			if (flag)
			{
				this.BackupList.Clear();
				this.BackupList = null;
			}
			this.DeregisterLocalizedText();
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x0000521C File Offset: 0x0000341C
		private void RegisterLocalizedText()
		{
			this.StrShowOnlyValidBackups = new BoundLocalizedText("txtShowOnlyValidBackups");
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x00005230 File Offset: 0x00003430
		private void DeregisterLocalizedText()
		{
			MainWindow.localizationManager.Deregister(this.StrShowOnlyValidBackups);
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x00005244 File Offset: 0x00003444
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x040000ED RID: 237
		public static readonly string pathModelFiles = "Archive/Model/";

		// Token: 0x040000EE RID: 238
		private bool restoreThinkXA;

		// Token: 0x040000EF RID: 239
		private bool restoreThinkXO;

		// Token: 0x040000F0 RID: 240
		private HashSet<string> requiredFiles;

		// Token: 0x040000F1 RID: 241
		private string usbLetter = "";

		// Token: 0x040000F2 RID: 242
		private DBConnection dbConnection;

		// Token: 0x040000F3 RID: 243
		private ParameterHandling parHandling;

		// Token: 0x040000F4 RID: 244
		private ProductManager productManager;

		// Token: 0x040000F5 RID: 245
		private ObservableCollection<BackupData> backupList;

		// Token: 0x040000F6 RID: 246
		private static Dictionary<string, List<string>> BackupModelFolders;

		// Token: 0x040000F7 RID: 247
		private BackupData selectedBackup;

		// Token: 0x040000FD RID: 253
		private bool showOnlyValidBackups;
	}
}
