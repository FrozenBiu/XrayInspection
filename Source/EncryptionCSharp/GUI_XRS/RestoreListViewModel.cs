using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Common;
using DatabaseConnection;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x0200017C RID: 380
	public class RestoreListViewModel : INotifyPropertyChanged
	{
		// Token: 0x170009EC RID: 2540
		// (get) Token: 0x06002272 RID: 8818 RVA: 0x000DE57D File Offset: 0x000DC77D
		// (set) Token: 0x06002273 RID: 8819 RVA: 0x000DE585 File Offset: 0x000DC785
		public RelayCommand CreateBackupListCommand { get; set; }

		// Token: 0x170009ED RID: 2541
		// (get) Token: 0x06002274 RID: 8820 RVA: 0x000DE58E File Offset: 0x000DC78E
		// (set) Token: 0x06002275 RID: 8821 RVA: 0x000DE596 File Offset: 0x000DC796
		public RelayCommand ShowInfoCommand { get; set; }

		// Token: 0x170009EE RID: 2542
		// (get) Token: 0x06002276 RID: 8822 RVA: 0x000DE59F File Offset: 0x000DC79F
		// (set) Token: 0x06002277 RID: 8823 RVA: 0x000DE5A7 File Offset: 0x000DC7A7
		public RelayCommand DoRestoreCommand { get; set; }

		// Token: 0x170009EF RID: 2543
		// (get) Token: 0x06002278 RID: 8824 RVA: 0x000DE5B0 File Offset: 0x000DC7B0
		// (set) Token: 0x06002279 RID: 8825 RVA: 0x000DE5B8 File Offset: 0x000DC7B8
		public RelayCommand ShowIVPCommand { get; set; }

		// Token: 0x170009F0 RID: 2544
		// (get) Token: 0x0600227A RID: 8826 RVA: 0x000DE5C1 File Offset: 0x000DC7C1
		// (set) Token: 0x0600227B RID: 8827 RVA: 0x000DE5C9 File Offset: 0x000DC7C9
		public RelayCommand CleanUpCommand { get; set; }

		// Token: 0x140000B7 RID: 183
		// (add) Token: 0x0600227C RID: 8828 RVA: 0x000DE5D4 File Offset: 0x000DC7D4
		// (remove) Token: 0x0600227D RID: 8829 RVA: 0x000DE60C File Offset: 0x000DC80C
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x170009F1 RID: 2545
		// (get) Token: 0x0600227E RID: 8830 RVA: 0x000DE644 File Offset: 0x000DC844
		// (set) Token: 0x0600227F RID: 8831 RVA: 0x000DE65C File Offset: 0x000DC85C
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

		// Token: 0x170009F2 RID: 2546
		// (get) Token: 0x06002280 RID: 8832 RVA: 0x000DE690 File Offset: 0x000DC890
		// (set) Token: 0x06002281 RID: 8833 RVA: 0x000DE6A8 File Offset: 0x000DC8A8
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

		// Token: 0x170009F3 RID: 2547
		// (get) Token: 0x06002282 RID: 8834 RVA: 0x000DE6C0 File Offset: 0x000DC8C0
		// (set) Token: 0x06002283 RID: 8835 RVA: 0x000DE6D8 File Offset: 0x000DC8D8
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

		// Token: 0x170009F4 RID: 2548
		// (get) Token: 0x06002284 RID: 8836 RVA: 0x000DE6EE File Offset: 0x000DC8EE
		// (set) Token: 0x06002285 RID: 8837 RVA: 0x000DE6F6 File Offset: 0x000DC8F6
		public BoundLocalizedText StrShowOnlyValidBackups { get; set; }

		// Token: 0x06002286 RID: 8838 RVA: 0x000DE700 File Offset: 0x000DC900
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x06002287 RID: 8839 RVA: 0x000DE730 File Offset: 0x000DC930
		public RestoreListViewModel(DBConnection dbConnection, Restore restore, bool restoreUsers, ParameterHandling parHandling, AccessRightsManager accessManager, bool checkForOnlyValidBackups, bool searchIVP, bool checkforEX2A)
		{
			this.BackupList = new ObservableCollection<BackupData>();
			this.dbConnection = dbConnection;
			this.restore = restore;
			this.restoreUsers = restoreUsers;
			this.parHandling = parHandling;
			this.accessManager = accessManager;
			this.checkForOnlyValidBackups = checkForOnlyValidBackups;
			this.showOnlyValidBackups = false;
			this.searchIVP = searchIVP;
			this.checkRestoreDataBaseforEX2A = checkforEX2A;
			this.DoRestoreCommand = new RelayCommand(new Action<object>(this.DoRestore));
			this.CreateBackupListCommand = new RelayCommand(new Action<object>(this.CreateBackupList));
			this.ShowInfoCommand = new RelayCommand(new Action<object>(this.ShowInfo));
			this.CleanUpCommand = new RelayCommand(new Action<object>(this.CleanUp));
			this.ShowIVPCommand = new RelayCommand(new Action<object>(this.ShowIVP));
			this.RegisterLocalizedText();
		}

		// Token: 0x06002288 RID: 8840 RVA: 0x000DE828 File Offset: 0x000DCA28
		private void ShowIVP(object obj)
		{
			CtrlRestoreList ctrlRestoreList = new CtrlRestoreList(this.dbConnection, this.parHandling, this.restore, true, this.accessManager, this.checkRestoreDataBaseforEX2A);
			Subwindow.Show(ctrlRestoreList, SubwindowButtons.BackAndHome, ctrlRestoreList.Buttons);
		}

		// Token: 0x06002289 RID: 8841 RVA: 0x000DE86C File Offset: 0x000DCA6C
		private void CreateBackupList(object obj)
		{
			List<string> backupDirs = new List<string>();
			this.AddBackupDirs(backupDirs);
			List<BackupData> backupFiles = this.GetBackupFiles(backupDirs);
			backupFiles.Sort(new BackupDataComparer());
			bool flag = false;
			bool flag2 = this.parHandling != null;
			if (flag2)
			{
				flag = (this.parHandling.ReadPar(Definition.machineTypePath) == "4" && this.checkRestoreDataBaseforEX2A);
			}
			for (int i = 0; i < backupFiles.Count; i++)
			{
				bool flag3 = this.checkForOnlyValidBackups || flag;
				if (flag3)
				{
					bool flag4 = this.CheckVersionCompability(backupFiles[i]) && this.CheckBVImageVersionCompatibility(backupFiles[i]) && this.CheckMachineTypeEX2ACompatibility(backupFiles[i]);
					if (flag4)
					{
						this.BackupList.Add(backupFiles[i]);
					}
					else
					{
						this.ShowOnlyValidBackups = true;
					}
				}
				else
				{
					bool flag5 = this.parHandling != null && this.parHandling.ReadPar(Definition.imgprocarchitecture) == "1" && !this.CheckMachineSubTypeCompatibility(backupFiles[i]);
					if (flag5)
					{
						this.ShowOnlyValidBackups = true;
					}
					else
					{
						this.BackupList.Add(backupFiles[i]);
					}
				}
			}
		}

		// Token: 0x0600228A RID: 8842 RVA: 0x000DE9BC File Offset: 0x000DCBBC
		private void AddBackupDirs(List<string> backupDirs)
		{
			this.dbConnection.GetUsbDriveLetter(ref this.usbLetter);
			this.usbLetter += "\\";
			bool flag = this.searchIVP;
			if (flag)
			{
				bool flag2 = Directory.Exists(Backup.IVPBackupFolder);
				if (flag2)
				{
					string[] directories = Directory.GetDirectories(Backup.IVPBackupFolder);
					foreach (string item in directories)
					{
						backupDirs.Add(item);
					}
				}
				bool flag3 = Directory.Exists(this.usbLetter) && this.usbLetter != "\\";
				if (flag3)
				{
					bool flag4 = Directory.Exists(this.usbLetter + "IVPBackupsRaycon");
					if (flag4)
					{
						string[] directories2 = Directory.GetDirectories(this.usbLetter + "\\IVPBackupsRaycon");
						foreach (string item2 in directories2)
						{
							backupDirs.Add(item2);
						}
					}
				}
			}
			else
			{
				bool flag5 = Directory.Exists(Backup.localBackupFolder);
				if (flag5)
				{
					backupDirs.Add(Backup.localBackupFolder);
				}
				bool flag6 = Directory.Exists(Backup.autoBackupFolder);
				if (flag6)
				{
					backupDirs.Add(Backup.autoBackupFolder);
				}
				bool flag7 = Directory.Exists(this.usbLetter) && this.usbLetter != "\\";
				if (flag7)
				{
					backupDirs.Add(this.usbLetter);
				}
				bool flag8 = Directory.Exists(this.usbLetter + Backup.backupFolderName);
				if (flag8)
				{
					backupDirs.Add(this.usbLetter + Backup.backupFolderName);
				}
				bool flag9 = Directory.Exists(this.usbLetter + Backup.autoBackupFolderName);
				if (flag9)
				{
					backupDirs.Add(this.usbLetter + Backup.autoBackupFolderName);
				}
			}
		}

		// Token: 0x0600228B RID: 8843 RVA: 0x000DEBA0 File Offset: 0x000DCDA0
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

		// Token: 0x0600228C RID: 8844 RVA: 0x000DED08 File Offset: 0x000DCF08
		public void DoRestore(object dummy)
		{
			bool flag = !this.restoreUsers;
			if (flag)
			{
				bool flag2 = this.parHandling == null || this.parHandling.ReadPar(XrayState.xrayPowerOnPath) == "1" || this.parHandling.ReadPar(XrayState.xrayStarterPath) == "1";
				if (flag2)
				{
					Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtTurnXrayOff"), DialogStyle.Error, DialogButtons.OK);
				}
				else
				{
					bool flag3 = Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtOverwriteExisting"), DialogStyle.Warning, DialogButtons.OKCancel) == Dialog.DialogResults.Ok;
					if (flag3)
					{
						bool flag4 = this.SelectedBackup != null && this.SelectedBackup.BackupPath != null && this.SelectedBackup.BackupPath != "";
						if (flag4)
						{
							int num = 0;
							int num2 = 20;
							int.TryParse(MainWindowViewModel.GetWindowsImageVersion(), out num);
							bool flag5 = num < num2;
							if (flag5)
							{
								Dialog.Show(LocalizationManager.Localize("txtError"), "Image version too low.\nMin version: " + num2.ToString() + "\nCurrent version: " + num.ToString(), DialogStyle.Error);
							}
							else
							{
								this.restore.DoRestore(this.SelectedBackup.BackupPath, this.SelectedBackup.BackupName);
							}
						}
						else
						{
							Dialog.Show(LocalizationManager.Localize("txtError"), "No backup chosen!", DialogStyle.Error);
						}
					}
				}
			}
			else
			{
				bool flag6 = this.RestoreUsers() && this.accessManager != null;
				if (flag6)
				{
					this.accessManager.LogoutUser();
				}
			}
		}

		// Token: 0x0600228D RID: 8845 RVA: 0x000DEEA0 File Offset: 0x000DD0A0
		public bool IsValidBackup(string backupName)
		{
			bool result = false;
			bool flag = backupName.StartsWith(Backup.zipBackupPreName) && backupName.EndsWith(Definition.zipFileEnding);
			if (flag)
			{
				bool flag2 = this.restoreUsers;
				if (flag2)
				{
					result = true;
				}
				else
				{
					bool flag3 = this.restore.RestoreProducts && !this.restore.RestoreSystem && backupName.Contains(Backup.parameterPreString);
					if (flag3)
					{
						result = true;
					}
					else
					{
						bool flag4 = backupName.Contains(Backup.fullBackupPreString);
						if (flag4)
						{
							result = true;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x0600228E RID: 8846 RVA: 0x000DEF2C File Offset: 0x000DD12C
		public void ShowBackupInfo()
		{
			bool flag = this.SelectedBackup != null && this.SelectedBackup.BackupPath != null && this.SelectedBackup.BackupPath != "";
			if (flag)
			{
				Restore.ExtractInfoFile(this.SelectedBackup.BackupPath);
				CtrlBackupInfo usrControl = new CtrlBackupInfo(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\" + Backup.infoFileName);
				Subwindow.Show(usrControl, SubwindowButtons.BackAndHome);
			}
			else
			{
				Dialog.Show(LocalizationManager.Localize("txtError"), "No backup chosen!", DialogStyle.Error);
			}
		}

		// Token: 0x0600228F RID: 8847 RVA: 0x000DEFC4 File Offset: 0x000DD1C4
		public bool RestoreUsers()
		{
			bool flag = this.SelectedBackup != null && this.SelectedBackup.BackupPath != null && this.SelectedBackup.BackupPath != "";
			if (flag)
			{
				bool flag2 = Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtOverwriteExisting"), DialogStyle.Warning, DialogButtons.OKCancel) == Dialog.DialogResults.Ok;
				if (flag2)
				{
					return this.restore.RestoreUsers(this.SelectedBackup.BackupPath, this.dbConnection);
				}
			}
			else
			{
				Dialog.Show(LocalizationManager.Localize("txtError"), "No backup chosen!", DialogStyle.Error);
			}
			return false;
		}

		// Token: 0x06002290 RID: 8848 RVA: 0x000DF064 File Offset: 0x000DD264
		public void ShowUserBackupInfo()
		{
			bool flag = this.SelectedBackup != null && this.SelectedBackup.BackupPath != null && this.SelectedBackup.BackupPath != "";
			if (flag)
			{
				CtrlUserBackupInfo usrControl = new CtrlUserBackupInfo(this.restore, this.SelectedBackup.BackupPath);
				Subwindow.Show(usrControl, SubwindowButtons.BackAndHome);
			}
			else
			{
				Dialog.Show(LocalizationManager.Localize("txtError"), "No backup chosen!", DialogStyle.Error);
			}
		}

		// Token: 0x06002291 RID: 8849 RVA: 0x000DF0DC File Offset: 0x000DD2DC
		private void ShowInfo(object obj)
		{
			bool flag = !this.restoreUsers;
			if (flag)
			{
				this.ShowBackupInfo();
			}
			else
			{
				this.ShowUserBackupInfo();
			}
		}

		// Token: 0x06002292 RID: 8850 RVA: 0x000DF108 File Offset: 0x000DD308
		private bool CheckVersionCompability(BackupData backup)
		{
			int num = 0;
			int mainValue = 0;
			int subValue = 0;
			int subsubValue = 0;
			Restore.ExtractInfoFile(backup.BackupPath);
			BackupInfoFileParser backupInfoFileParser = new BackupInfoFileParser(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\info.txt");
			BackupInfo guiversion = backupInfoFileParser.GetGUIVersion();
			string infoValue = guiversion.InfoValue;
			int.TryParse(MainWindowViewModel.GetWindowsImageVersion(), out num);
			string windowsImageVersionHardware = MainWindowViewModel.GetWindowsImageVersionHardware();
			bool flag = num >= 32 && windowsImageVersionHardware == "YPC-ZD03";
			bool result;
			if (flag)
			{
				string[] array = infoValue.Split(new char[]
				{
					'.'
				});
				array[2] = array[2].Split(new char[]
				{
					'_'
				})[0];
				bool flag2 = array.Length < 3;
				if (flag2)
				{
					result = false;
				}
				else
				{
					int.TryParse(array[0], out mainValue);
					int.TryParse(array[1], out subValue);
					int.TryParse(array[2], out subsubValue);
					bool flag3 = this.IsVersionEqualAbove(1, 42, 0, mainValue, subValue, subsubValue);
					result = flag3;
				}
			}
			else
			{
				bool flag4 = num >= 32 && windowsImageVersionHardware == "PPC2200";
				if (flag4)
				{
					string[] array2 = infoValue.Split(new char[]
					{
						'.'
					});
					array2[2] = array2[2].Split(new char[]
					{
						'_'
					})[0];
					bool flag5 = array2.Length < 3;
					if (flag5)
					{
						result = false;
					}
					else
					{
						int.TryParse(array2[0], out mainValue);
						int.TryParse(array2[1], out subValue);
						int.TryParse(array2[2], out subsubValue);
						bool flag6 = this.IsVersionEqualAbove(1, 38, 0, mainValue, subValue, subsubValue);
						result = flag6;
					}
				}
				else
				{
					bool flag7 = num >= 32 && windowsImageVersionHardware == "I210_TPC-DRM";
					if (flag7)
					{
						string[] array3 = infoValue.Split(new char[]
						{
							'.'
						});
						array3[2] = array3[2].Split(new char[]
						{
							'_'
						})[0];
						bool flag8 = array3.Length < 3;
						if (flag8)
						{
							result = false;
						}
						else
						{
							int.TryParse(array3[0], out mainValue);
							int.TryParse(array3[1], out subValue);
							int.TryParse(array3[2], out subsubValue);
							bool flag9 = this.IsVersionEqualAbove(1, 37, 0, mainValue, subValue, subsubValue);
							result = flag9;
						}
					}
					else
					{
						bool flag10 = num >= 32 && windowsImageVersionHardware == "TPC-DRM";
						if (flag10)
						{
							string[] array4 = infoValue.Split(new char[]
							{
								'.'
							});
							array4[2] = array4[2].Split(new char[]
							{
								'_'
							})[0];
							bool flag11 = array4.Length < 3;
							if (flag11)
							{
								result = false;
							}
							else
							{
								int.TryParse(array4[0], out mainValue);
								int.TryParse(array4[1], out subValue);
								int.TryParse(array4[2], out subsubValue);
								bool flag12 = this.IsVersionEqualAbove(1, 35, 0, mainValue, subValue, subsubValue);
								result = flag12;
							}
						}
						else
						{
							bool flag13 = num >= 30;
							if (flag13)
							{
								string[] array5 = infoValue.Split(new char[]
								{
									'.'
								});
								array5[2] = array5[2].Split(new char[]
								{
									'_'
								})[0];
								bool flag14 = array5.Length < 3;
								if (flag14)
								{
									result = false;
								}
								else
								{
									int.TryParse(array5[0], out mainValue);
									int.TryParse(array5[1], out subValue);
									int.TryParse(array5[2], out subsubValue);
									bool flag15 = this.IsVersionEqualAbove(1, 27, 0, mainValue, subValue, subsubValue);
									result = flag15;
								}
							}
							else
							{
								result = true;
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06002293 RID: 8851 RVA: 0x000DF4B8 File Offset: 0x000DD6B8
		private bool IsVersionEqualAbove(int mainLimit, int subLimit, int subsubLimit, int mainValue, int subValue, int subsubValue)
		{
			bool flag = mainValue < mainLimit;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = mainValue == mainLimit;
				if (flag2)
				{
					bool flag3 = subValue < subLimit;
					if (flag3)
					{
						return false;
					}
					bool flag4 = subValue == subLimit;
					if (flag4)
					{
						bool flag5 = subsubValue < subsubLimit;
						if (flag5)
						{
							return false;
						}
						bool flag6 = subsubValue == subsubLimit;
						if (flag6)
						{
							return true;
						}
						bool flag7 = subsubValue > subsubLimit;
						if (flag7)
						{
							return true;
						}
					}
					else
					{
						bool flag8 = subValue > subLimit;
						if (flag8)
						{
							return true;
						}
					}
				}
				else
				{
					bool flag9 = mainValue > mainLimit;
					if (flag9)
					{
						return true;
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x06002294 RID: 8852 RVA: 0x000DF54D File Offset: 0x000DD74D
		private void RegisterLocalizedText()
		{
			this.StrShowOnlyValidBackups = new BoundLocalizedText("txtShowOnlyValidBackups");
		}

		// Token: 0x06002295 RID: 8853 RVA: 0x000DF561 File Offset: 0x000DD761
		public void DeregisterLocalizedText()
		{
			MainWindow.localizationManager.Deregister(this.StrShowOnlyValidBackups);
		}

		// Token: 0x06002296 RID: 8854 RVA: 0x000DF578 File Offset: 0x000DD778
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

		// Token: 0x06002297 RID: 8855 RVA: 0x000DF5B0 File Offset: 0x000DD7B0
		private bool CheckBVImageVersionCompatibility(BackupData backup)
		{
			bool result;
			try
			{
				bool flag = this.restoreUsers;
				if (flag)
				{
					result = true;
				}
				else
				{
					bool flag2 = false;
					bool flag3 = backup.BackupName.StartsWith(Backup.fullBackupPreString + "v");
					if (flag3)
					{
						List<string> zippedFileNames = new List<string>
						{
							Backup.imageProcName_64,
							Backup.imageProcName_a64
						};
						flag2 = ZipHelper.ZipContainsFiles(backup.BackupPath, zippedFileNames);
					}
					string text = this.parHandling.ReadPar(Definition.machineSubTypePath).Trim();
					string a = this.parHandling.ReadPar(Definition.imgprocarchitecture);
					Restore.ExtractInfoFile(backup.BackupPath);
					BackupInfoFileParser backupInfoFileParser = new BackupInfoFileParser(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\info.txt");
					List<string> paths = new List<string>
					{
						Definition.imageVersionElinosPath,
						Definition.machineSubTypePath
					};
					List<BackupInfo> valuesFromInfoTxt = backupInfoFileParser.GetValuesFromInfoTxt(paths);
					string text2 = valuesFromInfoTxt[0].InfoValue;
					string infoValue = valuesFromInfoTxt[1].InfoValue;
					bool flag4 = text2 == "";
					if (flag4)
					{
						text2 = "0";
					}
					bool flag5 = a == "0" && Convert.ToInt32(text2) == 40;
					if (flag5)
					{
						result = flag2;
					}
					else
					{
						bool flag6 = a == "1" && Convert.ToInt32(text2) != 40;
						if (flag6)
						{
							result = flag2;
						}
						else
						{
							bool flag7 = a == "1" && Convert.ToInt32(text2) == 40;
							if (flag7)
							{
								bool flag8 = infoValue == null;
								if (flag8)
								{
									return false;
								}
								bool flag9 = infoValue.Length >= 2 && text.Length >= 2;
								if (flag9)
								{
									return text.StartsWith(infoValue.Substring(0, 2));
								}
							}
							result = true;
						}
					}
				}
			}
			catch (FormatException)
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06002298 RID: 8856 RVA: 0x000DF7B8 File Offset: 0x000DD9B8
		private bool CheckMachineSubTypeCompatibility(BackupData backup)
		{
			bool result;
			try
			{
				bool flag = this.restoreUsers;
				if (flag)
				{
					result = true;
				}
				else
				{
					Restore.ExtractInfoFile(backup.BackupPath);
					BackupInfoFileParser backupInfoFileParser = new BackupInfoFileParser(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\info.txt");
					List<string> paths = new List<string>
					{
						Definition.machineSubTypePath
					};
					List<BackupInfo> valuesFromInfoTxt = backupInfoFileParser.GetValuesFromInfoTxt(paths);
					bool flag2 = valuesFromInfoTxt[0] == null;
					if (flag2)
					{
						result = false;
					}
					else
					{
						string infoValue = valuesFromInfoTxt[0].InfoValue;
						string text = this.parHandling.ReadPar(Definition.machineSubTypePath);
						bool flag3 = infoValue == null;
						if (flag3)
						{
							result = false;
						}
						else
						{
							bool flag4 = infoValue.Length >= 2 && text.Length >= 2;
							if (flag4)
							{
								result = text.StartsWith(infoValue.Substring(0, 2));
							}
							else
							{
								result = true;
							}
						}
					}
				}
			}
			catch (FormatException)
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06002299 RID: 8857 RVA: 0x000DF8B4 File Offset: 0x000DDAB4
		private bool CheckMachineTypeEX2ACompatibility(BackupData backup)
		{
			bool flag = this.parHandling.ReadPar(Definition.machineTypePath) == "4";
			bool result = true;
			bool flag2 = flag && this.checkRestoreDataBaseforEX2A;
			if (flag2)
			{
				try
				{
					Restore.ExtractInfoFile(backup.BackupPath);
					BackupInfoFileParser backupInfoFileParser = new BackupInfoFileParser(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\info.txt");
					List<string> paths = new List<string>
					{
						Definition.machineTypePath
					};
					List<BackupInfo> valuesFromInfoTxt = backupInfoFileParser.GetValuesFromInfoTxt(paths);
					bool flag3 = valuesFromInfoTxt[0] == null;
					if (flag3)
					{
						return false;
					}
					string infoValue = valuesFromInfoTxt[0].InfoValue;
					bool flag4 = infoValue == null;
					if (flag4)
					{
						return false;
					}
					result = (infoValue == "4");
				}
				catch (FormatException)
				{
					return false;
				}
			}
			return result;
		}

		// Token: 0x04001664 RID: 5732
		private string usbLetter = "";

		// Token: 0x04001665 RID: 5733
		private ObservableCollection<BackupData> backupList;

		// Token: 0x04001666 RID: 5734
		private BackupData selectedBackup;

		// Token: 0x04001667 RID: 5735
		private Restore restore;

		// Token: 0x04001668 RID: 5736
		private DBConnection dbConnection;

		// Token: 0x04001669 RID: 5737
		private bool restoreUsers;

		// Token: 0x0400166A RID: 5738
		private ParameterHandling parHandling;

		// Token: 0x0400166B RID: 5739
		private AccessRightsManager accessManager;

		// Token: 0x0400166C RID: 5740
		private bool checkForOnlyValidBackups;

		// Token: 0x0400166D RID: 5741
		private bool showOnlyValidBackups;

		// Token: 0x0400166E RID: 5742
		private bool checkRestoreDataBaseforEX2A;

		// Token: 0x04001676 RID: 5750
		private bool searchIVP = false;
	}
}
