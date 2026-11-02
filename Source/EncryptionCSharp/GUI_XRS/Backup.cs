using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using DatabaseConnection;
using Definitions;
using GUI_XRS.Ftp;
using ICSharpCode.SharpZipLib.Zip;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x02000172 RID: 370
	public class Backup : INotifyPropertyChanged
	{
		// Token: 0x170009D3 RID: 2515
		// (get) Token: 0x060021FB RID: 8699 RVA: 0x000DB114 File Offset: 0x000D9314
		// (set) Token: 0x060021FC RID: 8700 RVA: 0x000DB12C File Offset: 0x000D932C
		public int MaxManualFiles
		{
			get
			{
				return this.maxManualFiles;
			}
			set
			{
				this.maxManualFiles = value;
			}
		}

		// Token: 0x140000B3 RID: 179
		// (add) Token: 0x060021FD RID: 8701 RVA: 0x000DB138 File Offset: 0x000D9338
		// (remove) Token: 0x060021FE RID: 8702 RVA: 0x000DB170 File Offset: 0x000D9370
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<bool, string[]> BackupCompleted;

		// Token: 0x170009D4 RID: 2516
		// (get) Token: 0x060021FF RID: 8703 RVA: 0x000DB1A8 File Offset: 0x000D93A8
		// (set) Token: 0x06002200 RID: 8704 RVA: 0x000DB1C0 File Offset: 0x000D93C0
		public bool SaveProducts
		{
			get
			{
				return this.saveProducts;
			}
			set
			{
				bool flag = !value && this.saveProducts;
				if (flag)
				{
					this.SaveSystem = false;
				}
				bool flag2 = !value && this.saveProducts && !this.saveSystem;
				if (flag2)
				{
					this.SaveAnomalyDetectionImages = false;
				}
				this.saveProducts = value;
				this.OnPropertyChanged("SaveProducts");
			}
		}

		// Token: 0x170009D5 RID: 2517
		// (get) Token: 0x06002201 RID: 8705 RVA: 0x000DB21C File Offset: 0x000D941C
		// (set) Token: 0x06002202 RID: 8706 RVA: 0x000DB234 File Offset: 0x000D9434
		public bool SaveSystem
		{
			get
			{
				return this.saveSystem;
			}
			set
			{
				bool flag = value && !this.saveSystem;
				if (flag)
				{
					this.SaveProducts = true;
				}
				this.saveSystem = value;
				this.OnPropertyChanged("SaveSystem");
			}
		}

		// Token: 0x170009D6 RID: 2518
		// (get) Token: 0x06002203 RID: 8707 RVA: 0x000DB270 File Offset: 0x000D9470
		// (set) Token: 0x06002204 RID: 8708 RVA: 0x000DB288 File Offset: 0x000D9488
		public bool SaveAnomalyDetectionImages
		{
			get
			{
				return this.saveAnomalyDetectionImages;
			}
			set
			{
				bool flag = value && (this.saveSystem || this.saveProducts);
				if (flag)
				{
					this.saveAnomalyDetectionImages = true;
				}
				else
				{
					this.saveAnomalyDetectionImages = false;
				}
				this.OnPropertyChanged("SaveAnomalyDetectionImages");
			}
		}

		// Token: 0x170009D7 RID: 2519
		// (get) Token: 0x06002205 RID: 8709 RVA: 0x000DB2CD File Offset: 0x000D94CD
		// (set) Token: 0x06002206 RID: 8710 RVA: 0x000DB2D5 File Offset: 0x000D94D5
		public bool isIVP { get; set; }

		// Token: 0x06002207 RID: 8711 RVA: 0x000DB2E0 File Offset: 0x000D94E0
		public Backup(DBConnection dbConnection, ParameterHandling parHandling, DBDataLogs dbDatalogs)
		{
			this.dbConnection = dbConnection;
			this.dbDatalogs = dbDatalogs;
			this.parHandling = parHandling;
			this.isIVP = false;
			this.bgWorker = new BackgroundWorker();
			this.bgWorker.WorkerReportsProgress = true;
			this.bgWorker.DoWork += this.bgWorker_DoWork;
			this.bgWorker.ProgressChanged += this.bgWorker_ProgressChanged;
			this.bgWorker.RunWorkerCompleted += this.bgWorker_RunWorkerCompleted;
			this.timerAutoBackup = new Timer(new TimerCallback(this.AutoBackup));
		}

		// Token: 0x06002208 RID: 8712 RVA: 0x000DB400 File Offset: 0x000D9600
		private void bgWorker_DoWork(object sender, DoWorkEventArgs e)
		{
			string text = "";
			string text2 = "";
			string text3 = "";
			string text4 = "";
			string text5 = this.parHandling.ReadPar(Definition.bitnessPath);
			string text6 = this.parHandling.ReadPar(Definition.imgprocarchitecture);
			bool flag = text6 == "1";
			string text7;
			if (flag)
			{
				text7 = Backup.imageProcName_a64;
			}
			else
			{
				text7 = ((text5 != "64") ? Backup.imageProcName_32 : Backup.imageProcName_64);
			}
			bool flag2 = false;
			this.dbConnection.GetDBDatalogsIsActive(ref flag2);
			this.isZipping = true;
			this.success = true;
			string[] selectedModels = e.Argument as string[];
			try
			{
				DateTime now = DateTime.Now;
				Predicate<string> <>9__1;
				int i2;
				int i;
				for (i = 0; i < this.backupPaths.Length; i = i2 + 1)
				{
					Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
					{
						bool flag37 = this.prog == null;
						if (flag37)
						{
							this.prog = new Progress(LocalizationManager.Localize("txtBackupTo") + " " + this.backupPaths[i], false);
							this.prog.Show();
						}
					}));
					bool flag3 = i == 0;
					if (flag3)
					{
						this.CreateInfoFile();
						bool flag4 = this.isIVP && this.folderNameIVP.Length != 0;
						if (flag4)
						{
							text = this.backupPaths[i] + "\\Backup_Parameter_" + this.folderNameIVP + Definition.zipFileEnding;
						}
						else
						{
							text = this.CreateZipFilename(this.backupPaths[i], now, text5, text6);
						}
						text3 = ";-\\.svn;-" + Backup.localBackupFolder.Replace("\\", "\\\\") + ";-" + Backup.autoBackupFolder.Replace("\\", "\\\\");
						text4 = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
						text3 = text3 + ";+" + (text4 + "\\" + Backup.infoFileName).Replace("\\", "\\\\");
						bool flag5 = this.SaveSystem && this.parHandling.ConnectionEstablished;
						if (flag5)
						{
							string hostname = Backup.remoteIP;
							this.dbConnection.GetIPElinos(ref hostname);
							FTPclient ftpclient = new FTPclient(hostname, Backup.remoteUsername, Backup.remotePassword);
							ftpclient.Download(Backup.imageProcAppPathForUpAndDownload + text7, text4 + "\\" + text7, true);
							text3 = text3 + ";+" + (text4 + "\\" + text7).Replace("\\", "\\\\");
						}
						text3 = text3 + ";+" + DBConnection.databasePath.Replace("\\", "\\\\");
						bool flag6 = flag2 && File.Exists(DBDataLogs.databasePath);
						if (flag6)
						{
							text3 = text3 + ";+" + DBDataLogs.databasePath.Replace("\\", "\\\\");
						}
						bool flag7 = false;
						this.dbConnection.GetTestAtCustomerSite(ref flag7);
						bool flag8 = selectedModels != null;
						if (flag8)
						{
							foreach (string text8 in selectedModels)
							{
								text3 = string.Concat(new string[]
								{
									text3,
									";+",
									this.modelData.Replace("\\", "\\\\"),
									"\\\\",
									text8
								});
								bool flag9 = this.SaveAnomalyDetectionImages;
								if (flag9)
								{
									text3 = text3 + ";+" + StoreImageData.archiveDirectoryTrainAnomalyDetectionMain.Replace("\\", "\\\\") + text8;
								}
							}
							string str = ".*\\Backup_AI_*";
							text3 = text3 + ";-" + str;
							bool flag10 = this.SaveAnomalyDetectionImages;
							if (flag10)
							{
								text3 = text3 + ";-" + StoreImageData.archiveDirectoryThroughput.Replace("\\", "\\\\");
								text3 = text3 + ";-" + StoreImageData.archiveDirectoryReject.Replace("\\", "\\\\");
								text3 = text3 + ";-" + StoreImageData.archiveDirectoryTestcardSimulation.Replace("\\", "\\\\");
								bool flag11 = flag7;
								if (flag11)
								{
									text3 = text3 + ";+" + StoreImageData.archiveDirectoryTestAtCustomerSite.Replace("\\", "\\\\");
								}
								else
								{
									text3 = text3 + ";-" + StoreImageData.archiveDirectoryTestAtCustomerSite.Replace("\\", "\\\\");
								}
							}
							else
							{
								text3 = text3 + ";-" + Path.Combine(StoreImageData.archiveRoot, "Image").Replace("\\", "\\\\");
							}
						}
						else
						{
							text3 = text3 + ";-" + StoreImageData.archiveRoot.Replace("\\", "\\\\");
						}
						bool flag12 = this.SaveSystem;
						if (flag12)
						{
							text3 = text3 + ";+" + text4.Replace("\\", "\\\\");
							foreach (string str2 in Backup.filesToIgnore)
							{
								text3 = text3 + ";-" + (text4 + "\\" + str2).Replace("\\", "\\\\");
							}
							foreach (string str3 in Backup.fileEndingsToIgnore)
							{
								text3 = text3 + ";-" + (str3 + "$").Replace("\\", "\\\\");
							}
							bool flag13 = selectedModels != null;
							if (flag13)
							{
								List<string> list = new List<string>();
								this.dbConnection.GetAllAIProductTableNames(list);
								List<string> list2 = list;
								Predicate<string> match;
								if ((match = <>9__1) == null)
								{
									match = (<>9__1 = ((string name) => selectedModels.Contains(name)));
								}
								list2.RemoveAll(match);
								foreach (string text9 in list)
								{
									text3 = string.Concat(new string[]
									{
										text3,
										";-",
										this.modelData.Replace("\\", "\\\\"),
										"\\\\",
										text9
									});
									bool flag14 = this.SaveAnomalyDetectionImages;
									if (flag14)
									{
										text3 = text3 + ";-" + StoreImageData.archiveDirectoryTrainAnomalyDetectionMain.Replace("\\", "\\\\") + text9;
									}
								}
								bool flag15 = this.SaveAnomalyDetectionImages && !flag7;
								if (flag15)
								{
									text3 = text3 + ";-" + StoreImageData.archiveDirectoryTestAtCustomerSite.Replace("\\", "\\\\");
								}
							}
							this.CreateAutostart();
						}
						List<string> filePaths = new List<string>();
						this.CalculateTotalFilesAndBytes(text4, text3, ref filePaths);
						for (int l = 0; l < this.backupPaths.Length; l++)
						{
							string pathRoot = Path.GetPathRoot(this.backupPaths[l]);
							this.success = this.CheckFreeMemory(this.backupPaths[l]);
							bool flag16 = !this.success;
							if (flag16)
							{
								break;
							}
						}
						bool flag17 = !this.success;
						if (flag17)
						{
							break;
						}
						bool flag18 = selectedModels != null;
						if (flag18)
						{
							bool flag19 = !Directory.Exists(Backup.tempBackupPath);
							if (flag19)
							{
								Directory.CreateDirectory(Backup.tempBackupPath);
							}
							this.success = this.dbConnection.CopyDatabaseToPath(Path.Combine(Backup.tempBackupPath, DBConnection.database));
							bool flag20 = !this.success;
							if (flag20)
							{
								break;
							}
							List<string> list3 = new List<string>();
							this.dbConnection.GetAllProductTableNames(list3);
							this.RemoveTablesInSelected(ref list3, selectedModels);
							foreach (string text10 in list3)
							{
								this.dbConnection.DeleteTable(text10);
								this.dbConnection.DeleteMaterialCurve(text10);
								this.dbConnection.DeleteTestcardSimSensitivities(text10);
								this.dbConnection.DeleteTableAI(text10);
							}
							this.dbConnection.SetCurrentProduct(selectedModels[0].Replace("AI", ""));
						}
						this.dbConnection.AllowReconnection(false);
						bool flag21 = flag2 && File.Exists(DBDataLogs.databasePath);
						if (flag21)
						{
							this.dbDatalogs.AllowReconnection(false);
						}
						bool flag22 = this.dbConnection.Connection.State > ConnectionState.Closed;
						if (flag22)
						{
							this.dbConnection.CloseConnection();
						}
						bool flag23 = flag2 && File.Exists(DBDataLogs.databasePath) && this.dbDatalogs.Connection.State > ConnectionState.Closed;
						if (flag23)
						{
							this.dbDatalogs.CloseConnection();
						}
						ZipBuilder.FileWritten += this.ProcessFileMethod;
						Backup.stopwatch.Restart();
						ZipBuilder.CreateZipFromList(text, text4, filePaths, Backup.zipPassword);
						Backup.stopwatch.Stop();
						ZipBuilder.FileWritten -= this.ProcessFileMethod;
						string text11 = Path.Combine(Backup.tempBackupPath, DBConnection.database);
						bool flag24 = File.Exists(text11) && selectedModels != null;
						if (flag24)
						{
							bool flag25 = this.dbConnection.RestoreProductParameters(text11);
							bool flag26 = !flag25;
							if (flag26)
							{
								File.Copy(Path.Combine(Backup.tempBackupPath, DBConnection.database), DBConnection.databasePath, true);
							}
							Directory.Delete(Backup.tempBackupPath, true);
						}
						this.dbConnection.AllowReconnection(true);
						bool flag27 = flag2 && File.Exists(DBDataLogs.databasePath);
						if (flag27)
						{
							this.dbDatalogs.AllowReconnection(true);
						}
						bool flag28 = this.SaveSystem;
						if (flag28)
						{
							File.Delete(text4 + "\\" + text7);
							File.Delete(Restore.autostartScriptName);
						}
						File.Delete(text4 + "\\" + Backup.infoFileName);
						bool flag29 = !ZipHelper.ZipContainsFile(text, DBConnection.database);
						if (flag29)
						{
							File.Delete(text);
							this.success = false;
							break;
						}
					}
					else
					{
						bool flag30 = this.isIVP && this.folderNameIVP.Length != 0;
						if (flag30)
						{
							text2 = this.backupPaths[i] + "\\Backup_Parameter_" + this.folderNameIVP + Definition.zipFileEnding;
						}
						else
						{
							text2 = this.CreateZipFilename(this.backupPaths[i], now, text5, text6);
						}
						this.success = ZipBuilder.WaitForZipCompletedAsync(5000).GetAwaiter().GetResult();
						bool flag31 = this.success;
						if (!flag31)
						{
							break;
						}
						this.isZipping = false;
						FileCopyHelper.CopyWithProgress(text, text2, this.bgWorker);
					}
					i2 = i;
				}
			}
			catch
			{
				string text12 = Path.Combine(Backup.tempBackupPath, DBConnection.database);
				bool flag32 = File.Exists(text12) && selectedModels != null;
				if (flag32)
				{
					bool flag33 = this.dbConnection.RestoreProductParameters(text12);
					bool flag34 = !flag33;
					if (flag34)
					{
						File.Copy(Path.Combine(Backup.tempBackupPath, DBConnection.database), DBConnection.databasePath, true);
					}
					Directory.Delete(Backup.tempBackupPath, true);
				}
				this.dbConnection.AllowReconnection(true);
				bool flag35 = File.Exists(text2);
				if (flag35)
				{
					File.Delete(text2);
				}
				bool flag36 = File.Exists(text);
				if (flag36)
				{
					File.Delete(text);
				}
				foreach (string path in Directory.EnumerateFiles(Backup.localBackupFolder, "*.tmp"))
				{
					File.Delete(path);
				}
				ZipBuilder.FileWritten -= this.ProcessFileMethod;
				Backup.stopwatch.Stop();
				this.success = false;
			}
		}

		// Token: 0x06002209 RID: 8713 RVA: 0x000DC0C0 File Offset: 0x000DA2C0
		private void CalculateTotalFilesAndBytes(string sourceDirectory, string fileFilter, ref List<string> filesToBackup)
		{
			this.currentFileCount = 0;
			this.totalFileCount = 0;
			this.currentByteCount = 0L;
			this.totalByteCount = 0L;
			this.remainingTime = 0;
			BackupSizeHelper backupSizeHelper = new BackupSizeHelper();
			backupSizeHelper.CalculateFileFilterSize(sourceDirectory, fileFilter, ref this.totalByteCount, ref this.totalFileCount, ref filesToBackup);
		}

		// Token: 0x0600220A RID: 8714 RVA: 0x000DC110 File Offset: 0x000DA310
		private void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			bool flag = this.prog != null;
			if (flag)
			{
				this.prog.FormFadeOut.Begin();
			}
			this.OnBackupCompleted(this.success, this.backupPaths);
			this.prog.Close();
			this.prog = null;
		}

		// Token: 0x0600220B RID: 8715 RVA: 0x000DC164 File Offset: 0x000DA364
		private void bgWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			bool flag = this.prog != null;
			if (flag)
			{
				int num = this.totalFileCount;
				bool flag2 = this.backupPaths.Length == 1;
				if (flag2)
				{
					this.prog.progBar.Value = (double)e.ProgressPercentage;
				}
				else
				{
					bool flag3 = this.isZipping;
					if (flag3)
					{
						this.prog.progBar.Value = (double)(e.ProgressPercentage * 70 / 100);
						num++;
					}
					else
					{
						this.prog.progBar.Value = (double)(70 + e.ProgressPercentage * 30 / 100);
						this.remainingTime = (int)(Backup.stopwatch.Elapsed.TotalSeconds * (1.0 - (double)e.ProgressPercentage / 100.0));
						num++;
					}
				}
				int num2 = (int)e.UserState;
				string newText = string.Concat(new string[]
				{
					LocalizationManager.Localize("txtBackupTo"),
					" ",
					this.backupPaths[num2],
					Environment.NewLine,
					"(",
					LocalizationManager.Localize("txtBackupCopied"),
					": ",
					string.Format("{0}/{1} - {2}: {3}s)", new object[]
					{
						this.currentFileCount,
						num,
						LocalizationManager.Localize("txtBackupTime"),
						this.remainingTime
					})
				});
				this.prog.ChangeText(newText);
			}
		}

		// Token: 0x0600220C RID: 8716 RVA: 0x000DC2F7 File Offset: 0x000DA4F7
		public void DoBackup(string[] selectedModels = null)
		{
			this.DoBackup(new string[]
			{
				Backup.localBackupFolder
			}, false, selectedModels);
		}

		// Token: 0x0600220D RID: 8717 RVA: 0x000DC311 File Offset: 0x000DA511
		public void DoBackup(string backupPath, string[] selectedModels = null)
		{
			this.DoBackup(new string[]
			{
				Backup.localBackupFolder,
				backupPath
			}, false, selectedModels);
		}

		// Token: 0x0600220E RID: 8718 RVA: 0x000DC32F File Offset: 0x000DA52F
		public void DoBackupIVP(string certificateName, string[] selectedModels = null)
		{
			this.folderNameIVP = certificateName;
			this.DoBackup(new string[]
			{
				Backup.IVPBackupFolder + "\\" + certificateName
			}, true, selectedModels);
		}

		// Token: 0x0600220F RID: 8719 RVA: 0x000DC35B File Offset: 0x000DA55B
		public void DoBackupIVP(string certificateName, string backupPath, string[] selectedModels = null)
		{
			this.folderNameIVP = certificateName;
			this.DoBackup(new string[]
			{
				Backup.IVPBackupFolder + "\\" + certificateName,
				backupPath
			}, true, selectedModels);
		}

		// Token: 0x06002210 RID: 8720 RVA: 0x000DC38C File Offset: 0x000DA58C
		public void DoBackup(string[] backupPaths, bool IVP, string[] selectedModels = null)
		{
			List<string> list = new List<string>();
			this.isIVP = IVP;
			bool flag = !this.isIVP;
			if (flag)
			{
				bool flag2 = !Directory.Exists(Backup.localBackupFolder);
				if (flag2)
				{
					Directory.CreateDirectory(Backup.localBackupFolder);
				}
			}
			for (int i = 0; i < backupPaths.Length; i++)
			{
				bool flag3 = i == 0;
				if (flag3)
				{
					list = Directory.GetFiles(backupPaths[i]).ToList<string>();
					list.Sort(new BackupNamesComparer());
					while (list.Count >= this.maxManualFiles)
					{
						bool flag4 = list.Count > 0;
						if (flag4)
						{
							File.Delete(list[list.Count - 1]);
							list.RemoveAt(list.Count - 1);
						}
					}
					string pathRoot = Path.GetPathRoot(Backup.localBackupFolder);
					long num = Disk.GetTotalFreeSpace(pathRoot) - 1073741824L;
					long directorySize = BackupSizeHelper.GetDirectorySize(Backup.localBackupFolder);
					while (directorySize >= num && num > 0L)
					{
						File.Delete(list[list.Count - 1]);
						list.RemoveAt(list.Count - 1);
						directorySize = BackupSizeHelper.GetDirectorySize(Backup.localBackupFolder);
					}
				}
			}
			this.backupPaths = backupPaths;
			bool flag5 = !this.bgWorker.IsBusy;
			if (flag5)
			{
				this.bgWorker.RunWorkerAsync(selectedModels);
			}
		}

		// Token: 0x06002211 RID: 8721 RVA: 0x000DC500 File Offset: 0x000DA700
		private string CreateZipFilename(string backupPath, DateTime time, string bitness, string architecture)
		{
			return this.CreateZipFilename(backupPath, time, bitness, architecture, false);
		}

		// Token: 0x06002212 RID: 8722 RVA: 0x000DC520 File Offset: 0x000DA720
		private string CreateZipFilename(string backupPath, DateTime time, string bitness, string architecture, bool autoBackup)
		{
			StringBuilder stringBuilder = new StringBuilder(backupPath);
			stringBuilder.Append(Backup.zipBackupPreName);
			if (autoBackup)
			{
				stringBuilder.Append(Backup.parameterPreString);
			}
			else
			{
				bool flag = this.SaveSystem;
				if (flag)
				{
					stringBuilder.Append(Backup.fullBackupPreString);
				}
				else
				{
					stringBuilder.Append(Backup.parameterPreString);
				}
			}
			stringBuilder.Append(time.ToString(Backup.timeFormatString));
			bool flag2 = this.serialNumber != null && this.serialNumber != "";
			if (flag2)
			{
				stringBuilder.Append("_");
				stringBuilder.Append(this.serialNumber);
			}
			bool flag3 = bitness == "64" && architecture == "0";
			if (flag3)
			{
				stringBuilder.Append("-64Bit");
			}
			else
			{
				bool flag4 = bitness == "64" && architecture == "1";
				if (flag4)
				{
					stringBuilder.Append(Backup.zipBackupAIName + "64Bit");
				}
				else
				{
					bool flag5 = bitness != "64" && architecture == "1";
					if (flag5)
					{
						stringBuilder.Append(Backup.zipBackupAIName);
					}
				}
			}
			stringBuilder.Append(Definition.zipFileEnding);
			return stringBuilder.ToString();
		}

		// Token: 0x06002213 RID: 8723 RVA: 0x000DC67E File Offset: 0x000DA87E
		private void ftpClient_DownloadProgressChanged(object sender, FtpDownloadEventArgs e)
		{
			this.bgWorker.ReportProgress((int)(e.DownloadedBytes * 100L / e.TotalBytes));
		}

		// Token: 0x06002214 RID: 8724 RVA: 0x000DC6A0 File Offset: 0x000DA8A0
		private void ProcessFileMethod(object sender, ZipWriteProgressEventArgs args)
		{
			bool flag = this.totalFileCount != 0;
			if (flag)
			{
				this.currentByteCount += args.BytesWritten;
				double num = (double)this.currentByteCount / Backup.stopwatch.Elapsed.TotalSeconds;
				long num2 = this.totalByteCount - this.currentByteCount;
				this.remainingTime = (int)((double)num2 / num);
				bool flag2 = this.backupPaths.Length != 1;
				if (flag2)
				{
					this.remainingTime += (int)((double)this.totalByteCount / num);
				}
				this.currentFileCount++;
				int percentProgress = this.currentFileCount * 100 / this.totalFileCount;
				this.bgWorker.ReportProgress(percentProgress, 0);
			}
		}

		// Token: 0x06002215 RID: 8725 RVA: 0x000DC768 File Offset: 0x000DA968
		protected virtual void OnBackupCompleted(bool success, string[] backupPaths)
		{
			Action<bool, string[]> backupCompleted = this.BackupCompleted;
			bool flag = backupCompleted != null;
			if (flag)
			{
				backupCompleted(success, backupPaths);
			}
		}

		// Token: 0x140000B4 RID: 180
		// (add) Token: 0x06002216 RID: 8726 RVA: 0x000DC790 File Offset: 0x000DA990
		// (remove) Token: 0x06002217 RID: 8727 RVA: 0x000DC7C8 File Offset: 0x000DA9C8
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x06002218 RID: 8728 RVA: 0x000DC800 File Offset: 0x000DAA00
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x06002219 RID: 8729 RVA: 0x000DC82D File Offset: 0x000DAA2D
		public void StartAutoBackupTimer()
		{
			this.timerAutoBackup.Change(600000, -1);
		}

		// Token: 0x0600221A RID: 8730 RVA: 0x000DC844 File Offset: 0x000DAA44
		private void AutoBackup(object dummy)
		{
			try
			{
				List<string> list = new List<string>();
				List<string> list2 = new List<string>();
				string text = "";
				string bitness = this.parHandling.ReadPar(Definition.bitnessPath);
				string architecture = this.parHandling.ReadPar(Definition.imgprocarchitecture);
				string text2 = DBConnection.databasePath + "_copy";
				list2.Add(Backup.autoBackupFolder);
				this.dbConnection.GetUsbDriveLetter(ref text);
				text += "\\";
				bool flag = Directory.Exists(text) && text != "\\";
				if (flag)
				{
					list2.Add(text + Backup.autoBackupFolderName);
				}
				DateTime now = DateTime.Now;
				foreach (string text3 in list2)
				{
					bool flag2 = !Directory.Exists(text3);
					if (flag2)
					{
						Directory.CreateDirectory(text3);
					}
					list = Directory.GetFiles(text3).ToList<string>();
					bool flag3 = list.Count > 9;
					if (flag3)
					{
						list.Sort(new BackupNamesComparer());
						for (int i = 0; i < list.Count - 10 + 1; i++)
						{
							File.Delete(list[i]);
						}
					}
					string directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
					string text4 = this.CreateZipFilename(text3, now, bitness, architecture, true);
					this.CreateInfoFile();
					File.Copy(DBConnection.databasePath, text2, true);
					ZipOutputStream zipOutputStream = null;
					try
					{
						zipOutputStream = new ZipOutputStream(File.Create(text4));
						zipOutputStream.Password = Backup.zipPassword;
						this.WriteZipEntry(zipOutputStream, text2, DBConnection.database);
						this.WriteZipEntry(zipOutputStream, directoryName + "\\" + Backup.infoFileName, Backup.infoFileName);
						zipOutputStream.IsStreamOwner = true;
						zipOutputStream.Close();
					}
					catch
					{
						bool flag4 = zipOutputStream != null;
						if (flag4)
						{
							zipOutputStream.Close();
						}
					}
					bool flag5 = !ZipHelper.ZipContainsFile(text4, DBConnection.database);
					if (flag5)
					{
						File.Delete(text4);
					}
					File.Delete(text2);
					File.Delete(directoryName + "\\" + Backup.infoFileName);
				}
			}
			catch
			{
			}
		}

		// Token: 0x0600221B RID: 8731 RVA: 0x000DCAE0 File Offset: 0x000DACE0
		private void WriteZipEntry(ZipOutputStream zs, string fileName, string nameInZipFile)
		{
			ZipEntry zipEntry = new ZipEntry(nameInZipFile);
			zipEntry.DateTime = DateTime.Now;
			zs.UseZip64 = UseZip64.Off;
			zs.PutNextEntry(zipEntry);
			FileStream fileStream = File.OpenRead(fileName);
			byte[] array = new byte[4096];
			for (int i = fileStream.Read(array, 0, array.Length); i > 0; i = fileStream.Read(array, 0, array.Length))
			{
				zs.Write(array, 0, i);
			}
			fileStream.Close();
			zs.CloseEntry();
		}

		// Token: 0x0600221C RID: 8732 RVA: 0x000DCB64 File Offset: 0x000DAD64
		private void CreateInfoFile()
		{
			string text = null;
			string text2 = null;
			MachineMode machineMode = MachineMode.Standard;
			DetectorType detectorType = DetectorType.DT;
			List<string> list = new List<string>();
			string directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			File.Create(directoryName + "\\" + Backup.infoFileName).Close();
			StreamWriter streamWriter = new StreamWriter(directoryName + "\\" + Backup.infoFileName);
			this.dbConnection.GetDetectorType(ref detectorType);
			bool flag = detectorType == DetectorType.DT;
			if (flag)
			{
				list.AddRange(Definition.detectorDTVersionPathes);
			}
			else
			{
				bool flag2 = detectorType == DetectorType.DTNetDE || detectorType == DetectorType.DTNetTDI;
				if (flag2)
				{
					list.AddRange(Definition.detectorDTNetVersionPathes);
				}
				else
				{
					list.AddRange(Definition.detectorSesotecVersionPathes);
				}
			}
			list.AddRange(Definition.versionPathes);
			list.AddRange(this.GetIOVersionPaths());
			list.AddRange(this.additionalInfoPaths);
			this.dbConnection.GetMachineMode(ref machineMode);
			bool flag3 = machineMode == MachineMode.Bulk;
			if (flag3)
			{
				list.Add(Definition.ioBulkVersionPath.Substring(0, Definition.ioBulkVersionPath.IndexOf("/Value") + 1));
			}
			bool flag4 = machineMode == MachineMode.MAndS;
			if (flag4)
			{
				list.Add(Definition.msVersion1Path.Substring(0, Definition.msVersion1Path.IndexOf("/Value") + 1));
			}
			foreach (string text3 in list)
			{
				string text4 = this.parHandling.ReadPar(text3 + "Value");
				bool flag5 = text3 == Definition.xRayOnTimePath || text3 == Definition.lastXRayOnTimePath;
				if (flag5)
				{
					double num = 0.0;
					bool flag6 = double.TryParse(text4, out num);
					if (flag6)
					{
						num = Math.Round(num / 3600.0, 2);
						text4 = num.ToString();
					}
				}
				bool flag7 = text3 == MainWindowViewModel.guiVersionPath.Substring(0, MainWindowViewModel.guiVersionPath.LastIndexOf("/") + 1);
				if (flag7)
				{
					text4 = Definition.guiVersion;
				}
				bool flag8 = text3 == MainWindowViewModel.guiVersionBuildPath.Substring(0, MainWindowViewModel.guiVersionBuildPath.LastIndexOf("/") + 1);
				if (flag8)
				{
					text4 = Definition.guiVersionBuild;
				}
				string[] array = text4.Split(new string[]
				{
					Environment.NewLine
				}, StringSplitOptions.RemoveEmptyEntries);
				bool flag9 = array.Length > 1;
				if (flag9)
				{
					streamWriter.WriteLine(text3 + " = " + array[0]);
					for (int i = 1; i < array.Length; i++)
					{
						StringBuilder stringBuilder = new StringBuilder(array[i]);
						for (int j = 0; j < (text3 + " = ").Length; j++)
						{
							stringBuilder.Insert(0, " ");
						}
						streamWriter.WriteLine(stringBuilder.ToString());
					}
				}
				else
				{
					streamWriter.WriteLine(text3 + " = " + text4);
				}
				streamWriter.WriteLine();
			}
			this.dbConnection.GetSerialNumber(ref this.serialNumber);
			bool flag10 = this.serialNumber != null;
			if (flag10)
			{
				streamWriter.WriteLine("SerialNumber = " + this.serialNumber);
				streamWriter.WriteLine();
			}
			this.dbConnection.GetDeviceName(ref text2);
			bool flag11 = text2 != null;
			if (flag11)
			{
				streamWriter.WriteLine("DeviceName = " + text2);
				streamWriter.WriteLine();
			}
			this.dbConnection.GetLineName(ref text);
			bool flag12 = text != null;
			if (flag12)
			{
				streamWriter.WriteLine("LineName = " + text);
				streamWriter.WriteLine();
			}
			streamWriter.Close();
		}

		// Token: 0x0600221D RID: 8733 RVA: 0x000DCF60 File Offset: 0x000DB160
		private void CreateAutostart()
		{
			StreamReader streamReader = new StreamReader(Definition.imageVersionFilename);
			string[] array = streamReader.ReadToEnd().Split(new char[0]);
			int num = 0;
			int num2 = 20;
			bool flag = array.Length > 1;
			if (flag)
			{
				int.TryParse(array[1], out num);
			}
			StreamWriter streamWriter = new StreamWriter(Restore.autostartScriptName);
			streamWriter.WriteLine("@echo off");
			streamWriter.WriteLine("for /f \"delims=\" %%i in ('type " + Definition.imageVersionFilename + "') do set WholeLine=%%i");
			streamWriter.WriteLine("set ImageVersion=%WholeLine:~-2%");
			streamWriter.WriteLine("if %ImageVersion% LSS " + num2.ToString() + " (");
			streamWriter.WriteLine("taskkill /f /im GUI_XRS.exe >nul 2>&1");
			streamWriter.WriteLine("echo Image version too low.");
			streamWriter.WriteLine("echo Min version: " + num2.ToString());
			streamWriter.WriteLine("echo Current version: %ImageVersion%");
			streamWriter.WriteLine("echo Application will restart...");
			streamWriter.WriteLine("timeout 5");
			streamWriter.WriteLine("del \"" + Restore.autostartScriptName + "\"");
			streamWriter.WriteLine("start \"\" \"GUI_XRS.exe\"");
			streamWriter.WriteLine(")");
			streamWriter.Close();
		}

		// Token: 0x0600221E RID: 8734 RVA: 0x000DD0A4 File Offset: 0x000DB2A4
		private string[] GetIOVersionPaths()
		{
			int num = 0;
			List<string> list = new List<string>();
			MachineType machineType = MachineType.MachineTypeUndefined;
			this.dbConnection.GetMachineType(ref machineType);
			string s = this.parHandling.ReadPar(Definition.ioVersionPath);
			int.TryParse(s, out num);
			bool flag = num < Definition.minSesotecIOProgramVersion;
			if (flag)
			{
				list.AddRange(Definition.versionPathsWago);
			}
			else
			{
				list.AddRange(Definition.versionPathsSesotecIO);
				bool flag2 = machineType == MachineType.RayconEntry;
				if (flag2)
				{
					List<string> list2 = (from path in list
					where path.EndsWith("_SM/")
					select path).ToList<string>();
					for (int i = 0; i < list2.Count; i++)
					{
						list.Remove(list2[i]);
					}
				}
			}
			return list.ToArray();
		}

		// Token: 0x0600221F RID: 8735 RVA: 0x000DD184 File Offset: 0x000DB384
		private void RemoveTablesInSelected(ref List<string> tableNames, string[] selectedModels)
		{
			tableNames = tableNames.Where(delegate(string tableName)
			{
				string text = "";
				this.dbConnection.GetProductName(tableName, ref text);
				bool flag = string.IsNullOrEmpty(text) || text == "Reference Product";
				return !selectedModels.Any((string selectedTable) => tableName.Substring(tableName.Length - 3) == selectedTable.Substring(selectedTable.Length - 3)) && !flag;
			}).ToList<string>();
		}

		// Token: 0x06002220 RID: 8736 RVA: 0x000DD1C0 File Offset: 0x000DB3C0
		private bool CheckFreeMemory(string backupPath)
		{
			this.success = true;
			string pathRoot = Path.GetPathRoot(backupPath);
			long totalFreeSpace = Disk.GetTotalFreeSpace(pathRoot);
			long num = this.totalByteCount;
			bool flag = pathRoot.Equals("C:\\", StringComparison.OrdinalIgnoreCase) || pathRoot.Equals("D:\\", StringComparison.OrdinalIgnoreCase);
			if (flag)
			{
				num += 1073741824L;
			}
			bool flag2 = totalFreeSpace < num;
			bool result;
			if (flag2)
			{
				string message = LocalizationManager.Localize("lbMemoryError") + " " + pathRoot;
				Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
				{
					Dialog.Show(LocalizationManager.Localize("txtError"), message, DialogStyle.Error, DialogButtons.OK);
				}));
				this.success = false;
				result = this.success;
			}
			else
			{
				DriveInfo driveInfo = new DriveInfo(pathRoot);
				bool isReady = driveInfo.IsReady;
				if (isReady)
				{
					string a = driveInfo.DriveFormat.ToUpperInvariant();
					bool flag3 = (a == "FAT16" && this.totalByteCount > (long)((ulong)int.MinValue)) || (a == "FAT32" && this.totalByteCount > 4294967296L);
					if (flag3)
					{
						string message = LocalizationManager.Localize("lbMemoryFileSystemError") + " " + pathRoot;
						Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
						{
							Dialog.Show(LocalizationManager.Localize("txtError"), message, DialogStyle.Error, DialogButtons.OK);
						}));
						this.success = false;
						return this.success;
					}
				}
				result = this.success;
			}
			return result;
		}

		// Token: 0x04001601 RID: 5633
		private static readonly string imageProcFolderName = "apps";

		// Token: 0x04001602 RID: 5634
		private const long diskSpaceReserve = 1073741824L;

		// Token: 0x04001603 RID: 5635
		private const int timeToBackupParameter = 10;

		// Token: 0x04001604 RID: 5636
		private const int maxAutoFiles = 10;

		// Token: 0x04001605 RID: 5637
		private int maxManualFiles = 8;

		// Token: 0x04001606 RID: 5638
		private string serialNumber = null;

		// Token: 0x04001607 RID: 5639
		public static string infoFileName = "info.txt";

		// Token: 0x04001608 RID: 5640
		public static readonly string backupFolderName = "BackupsRaycon\\";

		// Token: 0x04001609 RID: 5641
		public static readonly string autoBackupFolderName = "AutoBackupsRaycon\\";

		// Token: 0x0400160A RID: 5642
		public static readonly string localBackupFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\" + Backup.backupFolderName;

		// Token: 0x0400160B RID: 5643
		public static readonly string autoBackupFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\" + Backup.autoBackupFolderName;

		// Token: 0x0400160C RID: 5644
		public static readonly string IVPBackupFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\IVPCertificates";

		// Token: 0x0400160D RID: 5645
		public static readonly string tempBackupPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\tempBackup";

		// Token: 0x0400160E RID: 5646
		public static readonly string zipPassword = "sunds";

		// Token: 0x0400160F RID: 5647
		public static readonly string remoteIP = "192.168.0.60";

		// Token: 0x04001610 RID: 5648
		public static readonly string remoteUsername = "root";

		// Token: 0x04001611 RID: 5649
		public static readonly string remotePassword = "xrs_bv";

		// Token: 0x04001612 RID: 5650
		public static readonly string imageProcAppPath = "/" + Backup.imageProcFolderName + "/";

		// Token: 0x04001613 RID: 5651
		public static readonly string imageProcAppPathForUpAndDownload = "/%2f" + Backup.imageProcFolderName + "/";

		// Token: 0x04001614 RID: 5652
		public static readonly string imageProcName_32 = "XRSMain";

		// Token: 0x04001615 RID: 5653
		public static readonly string imageProcName_64 = "XRSMain_64";

		// Token: 0x04001616 RID: 5654
		public static readonly string imageProcName_a64 = "XRSMain_a64";

		// Token: 0x04001617 RID: 5655
		public static readonly string zipBackupPreName = "Backup_";

		// Token: 0x04001618 RID: 5656
		public static readonly string parameterPreString = "Parameter_";

		// Token: 0x04001619 RID: 5657
		public static readonly string zipBackupAIName = "-a";

		// Token: 0x0400161A RID: 5658
		public static readonly string guiPreString = "Application_";

		// Token: 0x0400161B RID: 5659
		public static readonly string elinosPreString = "Elinos_";

		// Token: 0x0400161C RID: 5660
		public static readonly string fullBackupPreString = "Full_";

		// Token: 0x0400161D RID: 5661
		public static readonly string timeFormatString = "yyyy_MM_dd_HH_mm_ss_ff";

		// Token: 0x0400161E RID: 5662
		public static string[] filesToIgnore = new string[]
		{
			"GUI_XRS.vshost.exe",
			"RestoreXRS.vshost.exe",
			"OtherDocuments",
			"Manuals",
			"TestcardSim",
			"IVPCertificates",
			"AuditcheckDocumentation",
			"DataPlots",
			"DataPlotsMonitoring",
			"DataLogs",
			Definition.tdiCalibFolder,
			"tempBackup",
			"DotNetZip.xml",
			"PdfSharp.Charting.xml",
			"PdfSharp.xml",
			"Log_",
			"Screenshots",
			"LogbookReports",
			"Backup_"
		};

		// Token: 0x0400161F RID: 5663
		public static string[] fileEndingsToIgnore = new string[]
		{
			".pdb",
			".zip"
		};

		// Token: 0x04001620 RID: 5664
		public string[] additionalInfoPaths = new string[]
		{
			Definition.xRayOnTimeInternalPath,
			Definition.xRayOnTimePath,
			Definition.lastXRayOnTimePath,
			Definition.xRayOnCyclesPath
		};

		// Token: 0x04001621 RID: 5665
		private BackgroundWorker bgWorker;

		// Token: 0x04001622 RID: 5666
		private string[] backupPaths;

		// Token: 0x04001623 RID: 5667
		private int totalFileCount = 0;

		// Token: 0x04001624 RID: 5668
		private int currentFileCount = 0;

		// Token: 0x04001625 RID: 5669
		private long totalByteCount = 0L;

		// Token: 0x04001626 RID: 5670
		private long currentByteCount = 0L;

		// Token: 0x04001627 RID: 5671
		private static Stopwatch stopwatch = new Stopwatch();

		// Token: 0x04001628 RID: 5672
		private int remainingTime = 0;

		// Token: 0x04001629 RID: 5673
		private Progress prog;

		// Token: 0x0400162A RID: 5674
		private bool saveProducts;

		// Token: 0x0400162B RID: 5675
		private bool saveSystem;

		// Token: 0x0400162C RID: 5676
		private bool saveAnomalyDetectionImages;

		// Token: 0x0400162D RID: 5677
		private Timer timerAutoBackup;

		// Token: 0x0400162E RID: 5678
		private DBConnection dbConnection;

		// Token: 0x0400162F RID: 5679
		private DBDataLogs dbDatalogs;

		// Token: 0x04001630 RID: 5680
		private ParameterHandling parHandling;

		// Token: 0x04001631 RID: 5681
		private bool success;

		// Token: 0x04001632 RID: 5682
		private string folderNameIVP;

		// Token: 0x04001633 RID: 5683
		private string modelData = StoreImageData.archiveRoot + "\\Model";

		// Token: 0x04001635 RID: 5685
		private bool isZipping;
	}
}
