using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using DatabaseConnection;
using Definitions;
using GUI_XRS.Ftp;
using ICSharpCode.SharpZipLib.Zip;

namespace GUI_XRS
{
	// Token: 0x02000181 RID: 385
	public class Restore : INotifyPropertyChanged
	{
		// Token: 0x140000B9 RID: 185
		// (add) Token: 0x060022CA RID: 8906 RVA: 0x000E0174 File Offset: 0x000DE374
		// (remove) Token: 0x060022CB RID: 8907 RVA: 0x000E01AC File Offset: 0x000DE3AC
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x140000BA RID: 186
		// (add) Token: 0x060022CC RID: 8908 RVA: 0x000E01E4 File Offset: 0x000DE3E4
		// (remove) Token: 0x060022CD RID: 8909 RVA: 0x000E021C File Offset: 0x000DE41C
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action BeforeReboot;

		// Token: 0x140000BB RID: 187
		// (add) Token: 0x060022CE RID: 8910 RVA: 0x000E0254 File Offset: 0x000DE454
		// (remove) Token: 0x060022CF RID: 8911 RVA: 0x000E028C File Offset: 0x000DE48C
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action Rebooting;

		// Token: 0x140000BC RID: 188
		// (add) Token: 0x060022D0 RID: 8912 RVA: 0x000E02C4 File Offset: 0x000DE4C4
		// (remove) Token: 0x060022D1 RID: 8913 RVA: 0x000E02FC File Offset: 0x000DE4FC
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action DatabaseRestored;

		// Token: 0x140000BD RID: 189
		// (add) Token: 0x060022D2 RID: 8914 RVA: 0x000E0334 File Offset: 0x000DE534
		// (remove) Token: 0x060022D3 RID: 8915 RVA: 0x000E036C File Offset: 0x000DE56C
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<List<User>> UserInfoExtracted;

		// Token: 0x17000A01 RID: 2561
		// (get) Token: 0x060022D4 RID: 8916 RVA: 0x000E03A4 File Offset: 0x000DE5A4
		// (set) Token: 0x060022D5 RID: 8917 RVA: 0x000E03BC File Offset: 0x000DE5BC
		public bool RestoreProducts
		{
			get
			{
				return this.restoreProducts;
			}
			set
			{
				this.restoreProducts = value;
				this.OnPropertyChanged("RestoreProducts");
				bool flag = !value;
				if (flag)
				{
					this.RestoreDatabase = value;
				}
				bool flag2 = !value && !this.restoreSystem;
				if (flag2)
				{
					this.RestoreAnomalyDetectionImages = value;
				}
			}
		}

		// Token: 0x17000A02 RID: 2562
		// (get) Token: 0x060022D6 RID: 8918 RVA: 0x000E040C File Offset: 0x000DE60C
		// (set) Token: 0x060022D7 RID: 8919 RVA: 0x000E0424 File Offset: 0x000DE624
		public bool RestoreDatabase
		{
			get
			{
				return this.restoreDatabase;
			}
			set
			{
				this.restoreDatabase = value;
				if (value)
				{
					this.RestoreProducts = value;
				}
				this.OnPropertyChanged("RestoreDatabase");
			}
		}

		// Token: 0x17000A03 RID: 2563
		// (get) Token: 0x060022D8 RID: 8920 RVA: 0x000E0454 File Offset: 0x000DE654
		// (set) Token: 0x060022D9 RID: 8921 RVA: 0x000E046C File Offset: 0x000DE66C
		public bool RestoreSystem
		{
			get
			{
				return this.restoreSystem;
			}
			set
			{
				this.restoreSystem = value;
				bool flag = !value && !this.restoreProducts;
				if (flag)
				{
					this.RestoreAnomalyDetectionImages = false;
				}
			}
		}

		// Token: 0x17000A04 RID: 2564
		// (get) Token: 0x060022DA RID: 8922 RVA: 0x000E049C File Offset: 0x000DE69C
		public ParameterHandling ParHandling
		{
			get
			{
				return this.parHandling;
			}
		}

		// Token: 0x17000A05 RID: 2565
		// (get) Token: 0x060022DB RID: 8923 RVA: 0x000E04B4 File Offset: 0x000DE6B4
		// (set) Token: 0x060022DC RID: 8924 RVA: 0x000E04CC File Offset: 0x000DE6CC
		public bool RestoreAnomalyDetectionImages
		{
			get
			{
				return this.restoreAnomalyDetectionImages;
			}
			set
			{
				bool flag = value && (this.restoreDatabase || this.restoreProducts);
				if (flag)
				{
					this.restoreAnomalyDetectionImages = true;
				}
				else
				{
					this.restoreAnomalyDetectionImages = false;
				}
				this.OnPropertyChanged("RestoreAnomalyDetectionImages");
			}
		}

		// Token: 0x060022DD RID: 8925 RVA: 0x000E0514 File Offset: 0x000DE714
		public Restore(DBConnection dbConnection, ParameterHandling parHandling, ErrorSurvey errorSurvey, DBDataLogs dbDataLogs, AccessRightsManager accessManager)
		{
			this.dbConnection = dbConnection;
			this.parHandling = parHandling;
			this.errorSurvey = errorSurvey;
			this.dbDataLogs = dbDataLogs;
			this.accessManager = accessManager;
		}

		// Token: 0x060022DE RID: 8926 RVA: 0x000E0570 File Offset: 0x000DE770
		public static void ExtractInfoFile(string backupPath)
		{
			FastZip fastZip = new FastZip();
			string text = ";-\\.svn;-.*\\/.*";
			fastZip.Password = Backup.zipPassword;
			text = text + ";+" + Backup.infoFileName.Replace("\\", "\\\\");
			fastZip.ExtractZip(backupPath, Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), text);
		}

		// Token: 0x060022DF RID: 8927 RVA: 0x000E05D0 File Offset: 0x000DE7D0
		private void ExtractAutostart(string backupPath)
		{
			FastZip fastZip = new FastZip();
			string text = ";-\\.svn";
			fastZip.Password = Backup.zipPassword;
			text = text + ";+" + Restore.autostartScriptName.Replace("\\", "\\\\");
			fastZip.ExtractZip(backupPath, Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), text);
		}

		// Token: 0x060022E0 RID: 8928 RVA: 0x000E0630 File Offset: 0x000DE830
		public void DoRestore(string backupPath, string backupName)
		{
			bool databaseCompatible = true;
			this.dbConnection.AllowReconnection(false);
			this.dbConnection.CloseConnection();
			Restore.ExtractInfoFile(backupPath);
			new Thread(delegate()
			{
				try
				{
					bool flag = this.RestoreProducts;
					bool databaseCompatible;
					if (flag)
					{
						databaseCompatible = this.DoRestoreDatabase(backupPath, this.dbConnection);
						this.DoRestoreAnomalyDetection(backupPath);
					}
					databaseCompatible = databaseCompatible;
					if (databaseCompatible)
					{
						this.dbConnection.AllowReconnection(true);
						this.dbConnection.Connect(DBConnection.databasePath);
						bool flag2 = this.databaseRestored;
						if (flag2)
						{
							this.OnDatabaseRestored();
							this.databaseRestored = false;
						}
						bool flag3 = this.RestoreSystem && this.parHandling.ConnectionEstablished;
						if (flag3)
						{
							new Thread(new ParameterizedThreadStart(this.RestoreProcesses))
							{
								Name = "RestoreProcesses"
							}.Start(backupPath);
						}
						else
						{
							this.RestoreProcesses(backupPath);
						}
					}
					this.errorSurvey.StartSurvey();
					this.errorSurvey.StartCheckingXRayOperationgHours();
				}
				catch (Exception ex)
				{
					Exception ex3;
					Exception ex2 = ex3;
					Exception ex = ex2;
					Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(delegate()
					{
						this.dbConnection.AllowReconnection(true);
						Dialog.Show("Error", ex.Message, DialogStyle.Error, DialogButtons.OK);
					}));
				}
				this.dbConnection.AllowReconnection(true);
				this.dbDataLogs.BackupsAddEntry(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"), backupName, this.accessManager.CurrentUser.ID.ToString(), this.accessManager.CurrentUser.Username, this.accessManager.CurrentUser.Userlevel, this.RestoreProducts ? "1" : "0", this.RestoreDatabase ? "1" : "0", this.RestoreSystem ? "1" : "0");
				bool flag4 = Directory.Exists(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/img");
				if (flag4)
				{
					string[] files = Directory.GetFiles(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/img");
					foreach (string text in files)
					{
						bool flag5 = text.Contains("");
						if (flag5)
						{
						}
						File.Delete(text);
					}
					files = Directory.GetFiles(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/img");
					bool flag6 = files.Length == 0;
					if (flag6)
					{
						Directory.Delete(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/img");
					}
				}
			})
			{
				Name = "Restore"
			}.Start();
		}

		// Token: 0x060022E1 RID: 8929 RVA: 0x000E06AC File Offset: 0x000DE8AC
		private void RestoreProcesses(object arg)
		{
			string backupPath = "";
			bool flag = arg is string;
			if (flag)
			{
				backupPath = (string)arg;
			}
			bool flag2 = this.RestoreSystem;
			if (flag2)
			{
				bool connectionEstablished = this.parHandling.ConnectionEstablished;
				if (connectionEstablished)
				{
					this.DoRestoreImageProcessing(backupPath);
				}
				this.DoRestoreGUI(backupPath);
			}
			File.Delete(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\" + Backup.infoFileName);
			bool flag3 = this.RestoreDatabase;
			if (flag3)
			{
				this.resetAuditCheckDocumentation();
			}
		}

		// Token: 0x060022E2 RID: 8930 RVA: 0x000E0738 File Offset: 0x000DE938
		private bool DoRestoreDatabase(string backupPath, DBConnection dbConnection)
		{
			this.RestoreProductsOrDatabase(backupPath, dbConnection);
			return true;
		}

		// Token: 0x060022E3 RID: 8931 RVA: 0x000E0758 File Offset: 0x000DE958
		private void RestoreProductsOrDatabase(string backupPath, DBConnection dbConnection)
		{
			bool flag = ZipHelper.ZipContainsFile(backupPath, DBConnection.database);
			if (flag)
			{
				string text = ";-\\.svn";
				FastZip fastZip = new FastZip();
				fastZip.Password = Backup.zipPassword;
				text = text + ";+" + DBConnection.database.Replace("\\", "\\\\");
				bool flag2 = this.RestoreDatabase;
				if (flag2)
				{
					fastZip.ExtractZip(backupPath, Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), text);
				}
				bool flag3 = !this.RestoreDatabase && this.RestoreProducts;
				if (flag3)
				{
					bool flag4 = !Directory.Exists(this.tempBackupPath);
					if (flag4)
					{
						Directory.CreateDirectory(this.tempBackupPath);
					}
					fastZip.ExtractZip(backupPath, this.tempBackupPath, text);
					this.progressUpdateRunning = true;
					new Thread(new ParameterizedThreadStart(this.UpdateProgress))
					{
						Name = "UpdateProducts"
					}.Start("Updating products...");
					dbConnection.RestoreProductParameters(this.tempBackupPath + "\\" + DBConnection.database);
					this.progressUpdateRunning = false;
					File.Delete(this.tempBackupPath + "\\" + DBConnection.database);
				}
				bool flag5 = !this.RestoreSystem;
				if (flag5)
				{
					this.databaseRestored = true;
				}
			}
			else
			{
				Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(delegate()
				{
					Dialog.Show("Error", "No database found or backup is corrupted!", DialogStyle.Error, DialogButtons.OK);
				}));
			}
		}

		// Token: 0x060022E4 RID: 8932 RVA: 0x000E08E0 File Offset: 0x000DEAE0
		private void DoRestoreGUI(string backupPath)
		{
			FastZip fastZip = new FastZip();
			fastZip.Password = Backup.zipPassword;
			this.progressUpdateRunning = true;
			new Thread(new ParameterizedThreadStart(this.UpdateProgress))
			{
				Name = "UpdateDatabase"
			}.Start("Updating database...");
			this.UpdateDatabase(fastZip, backupPath);
			this.progressUpdateRunning = false;
			string fileFilter = ";+RestoreXRS.exe";
			fastZip.ExtractZip(backupPath, Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), fileFilter);
			Process.Start(Restore.restoreProcessPath, "\"" + backupPath + "\"");
			Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
			{
				Application.Current.Shutdown();
			}));
		}

		// Token: 0x060022E5 RID: 8933 RVA: 0x000E09AC File Offset: 0x000DEBAC
		private void DoRestoreImageProcessing(string backupPath)
		{
			string a = this.parHandling.ReadPar(Definition.imgprocarchitecture);
			bool flag = a == "1";
			string text;
			if (flag)
			{
				text = Backup.imageProcName_a64;
			}
			else
			{
				text = ((this.parHandling.ReadPar(Definition.bitnessPath) != "64") ? Backup.imageProcName_32 : Backup.imageProcName_64);
			}
			bool flag2 = ZipHelper.ZipContainsFile(backupPath, text);
			if (flag2)
			{
				bool flag3 = this.parHandling != null;
				if (flag3)
				{
					this.progressUpdateRunning = true;
					new Thread(new ThreadStart(this.UpdateProgress))
					{
						Name = "UpdateImgProcessing"
					}.Start();
					this.errorSurvey.StopSurvey();
					this.errorSurvey.StopCheckingXRayOperatingHours();
					Thread.Sleep(2500);
					string directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
					string text2 = ";-\\.svn";
					FastZip fastZip = new FastZip();
					fastZip.Password = Backup.zipPassword;
					text2 = text2 + ";+" + text;
					fastZip.ExtractZip(backupPath, directoryName, text2);
					this.parHandling.DisableWriteProtection();
					string remoteIP = Backup.remoteIP;
					this.dbConnection.GetIPElinos(ref remoteIP);
					FTPclient ftpclient = new FTPclient(remoteIP, Backup.remoteUsername, Backup.remotePassword);
					ftpclient.FtpDelete(Backup.imageProcAppPathForUpAndDownload + text);
					ftpclient.Upload(directoryName + "\\" + text, Backup.imageProcAppPathForUpAndDownload + text);
					ftpclient.SetFileAttributes(Backup.imageProcAppPath + text, "777");
					this.progressUpdateRunning = false;
					this.OnBeforeReboot();
					this.parHandling.Reboot();
					File.Delete(directoryName + "\\" + text);
					this.OnRebooting();
				}
			}
			else
			{
				Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(delegate()
				{
					Dialog.Show("Error", "Image processing file not found or backup is corrupted!", DialogStyle.Error, DialogButtons.OK);
				}));
			}
		}

		// Token: 0x060022E6 RID: 8934 RVA: 0x000E0BB5 File Offset: 0x000DEDB5
		private void UpdateProgress()
		{
			this.UpdateProgress(LocalizationManager.Localize("txtRestoreImageProcessing") + "...");
		}

		// Token: 0x060022E7 RID: 8935 RVA: 0x000E0BD4 File Offset: 0x000DEDD4
		private void UpdateProgress(object obj)
		{
			string progText = null;
			Progress prog = null;
			int progValue = 0;
			bool flag = obj != null;
			if (flag)
			{
				bool flag2 = obj is string;
				if (flag2)
				{
					progText = (string)obj;
				}
			}
			bool flag3 = Application.Current != null;
			if (flag3)
			{
				Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
				{
					prog = new Progress((progText != null) ? progText : "", false);
					prog.Show();
				}));
			}
			while (this.progressUpdateRunning)
			{
				bool flag4 = prog != null;
				if (flag4)
				{
					bool flag5 = Application.Current != null;
					if (flag5)
					{
						Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
						{
							prog.progBar.Value = (double)progValue;
						}));
					}
					progValue += 2;
					bool flag6 = progValue > 100;
					if (flag6)
					{
						progValue = 0;
					}
				}
				Thread.Sleep(50);
			}
			bool flag7 = Application.Current != null;
			if (flag7)
			{
				Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
				{
					prog.Close();
				}));
			}
		}

		// Token: 0x060022E8 RID: 8936 RVA: 0x000E0CF8 File Offset: 0x000DEEF8
		private void UpdateDatabase(FastZip zip, string backupPath)
		{
			string text = ";-\\.svn";
			zip.Password = Backup.zipPassword;
			text = text + ";+" + DBConnection.database.Replace("\\", "\\\\");
			bool flag = !Directory.Exists(this.tempBackupPath);
			if (flag)
			{
				Directory.CreateDirectory(this.tempBackupPath);
			}
			zip.ExtractZip(backupPath, this.tempBackupPath, text);
			this.dbConnection.UpdateDatabase(this.tempBackupPath + "\\" + DBConnection.database);
			File.Delete(this.tempBackupPath + "\\" + DBConnection.database);
		}

		// Token: 0x060022E9 RID: 8937 RVA: 0x000E0DA4 File Offset: 0x000DEFA4
		protected virtual void OnBeforeReboot()
		{
			Action beforeReboot = this.BeforeReboot;
			bool flag = beforeReboot != null;
			if (flag)
			{
				beforeReboot();
			}
		}

		// Token: 0x060022EA RID: 8938 RVA: 0x000E0DC8 File Offset: 0x000DEFC8
		protected virtual void OnRebooting()
		{
			Action rebooting = this.Rebooting;
			bool flag = rebooting != null;
			if (flag)
			{
				rebooting();
			}
		}

		// Token: 0x060022EB RID: 8939 RVA: 0x000E0DEC File Offset: 0x000DEFEC
		protected virtual void OnDatabaseRestored()
		{
			Action action = this.DatabaseRestored;
			bool flag = action != null;
			if (flag)
			{
				action();
			}
		}

		// Token: 0x060022EC RID: 8940 RVA: 0x000E0E10 File Offset: 0x000DF010
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x060022ED RID: 8941 RVA: 0x000E0E40 File Offset: 0x000DF040
		public bool RestoreUsers(string backupPath, DBConnection dbConnection)
		{
			bool result = false;
			bool flag = ZipHelper.ZipContainsFile(backupPath, DBConnection.database);
			if (flag)
			{
				string text = ";-\\.svn";
				FastZip fastZip = new FastZip();
				fastZip.Password = Backup.zipPassword;
				text = text + ";+" + DBConnection.database.Replace("\\", "\\\\");
				bool flag2 = !Directory.Exists(this.tempBackupPath);
				if (flag2)
				{
					Directory.CreateDirectory(this.tempBackupPath);
				}
				fastZip.ExtractZip(backupPath, this.tempBackupPath, text);
				result = dbConnection.RestoreUsers(this.tempBackupPath + "\\" + DBConnection.database);
				File.Delete(this.tempBackupPath + "\\" + DBConnection.database);
				Dialog.Show(LocalizationManager.Localize("txtSuccess"), LocalizationManager.Localize("txtSuccess"), DialogStyle.Ok, DialogButtons.None, true);
			}
			else
			{
				Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(delegate()
				{
					Dialog.Show("Error", "No database found or backup is corrupted!", DialogStyle.Error, DialogButtons.OK);
				}));
			}
			return result;
		}

		// Token: 0x060022EE RID: 8942 RVA: 0x000E0F5C File Offset: 0x000DF15C
		public void GetUsersFromBackup(string backupPath, DBConnection dbConnection)
		{
			Thread thread = new Thread(delegate()
			{
				bool flag = false;
				List<User> list = null;
				bool flag2 = ZipHelper.ZipContainsFile(backupPath, DBConnection.database);
				if (flag2)
				{
					string text = ";-\\.svn";
					FastZip fastZip = new FastZip();
					fastZip.Password = Backup.zipPassword;
					text = text + ";+" + DBConnection.database.Replace("\\", "\\\\");
					bool flag3 = !Directory.Exists(this.tempBackupPath);
					if (flag3)
					{
						Directory.CreateDirectory(this.tempBackupPath);
					}
					fastZip.ExtractZip(backupPath, this.tempBackupPath, text);
					DBConnection dbconnection = new DBConnection();
					dbconnection.Connect(this.tempBackupPath + "\\" + DBConnection.database);
					list = new List<User>();
					flag = dbconnection.GetUserList(list);
					dbconnection.CloseConnection();
					File.Delete(this.tempBackupPath + "\\" + DBConnection.database);
				}
				else
				{
					Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(delegate()
					{
						Dialog.Show("Error", "No database found!", DialogStyle.Error, DialogButtons.OK);
					}));
				}
				this.OnUserInfoExtracted(flag ? list : null);
			});
			thread.Start();
		}

		// Token: 0x060022EF RID: 8943 RVA: 0x000E0F98 File Offset: 0x000DF198
		protected virtual void OnUserInfoExtracted(List<User> users)
		{
			Action<List<User>> userInfoExtracted = this.UserInfoExtracted;
			bool flag = userInfoExtracted != null;
			if (flag)
			{
				userInfoExtracted(users);
			}
		}

		// Token: 0x060022F0 RID: 8944 RVA: 0x000E0FC0 File Offset: 0x000DF1C0
		private void resetAuditCheckDocumentation()
		{
			string str = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/AuditcheckDocumentation/CurrentDoc/";
			string text = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/AuditcheckDocumentation/OtherDocs/";
			bool flag = File.Exists(str + "AuditCheckCurrent.pdf");
			if (flag)
			{
				bool flag2 = !Directory.Exists(text);
				if (flag2)
				{
					Directory.CreateDirectory(text);
				}
				string text2 = "";
				this.dbConnection.GetSerialNumber(ref text2);
				File.Copy(str + "AuditCheckCurrent.pdf", string.Concat(new string[]
				{
					text,
					"AuditCheck_",
					DateTime.Now.ToString("yyyyMMdd_HHmmss").Replace(".", "_").Replace("/", "_").Replace(":", "_"),
					"_",
					text2,
					".pdf"
				}));
				File.Delete(str + "AuditCheckCurrent.pdf");
				this.dbConnection.SetYPositionAudit(0);
				DirectoryInfo directoryInfo = new DirectoryInfo(text);
				FileInfo[] files = directoryInfo.GetFiles();
				long num = files.Sum((FileInfo f) => f.Length) / 1024L;
				Array.Sort<FileInfo>(files, (FileInfo f1, FileInfo f2) => f1.Name.CompareTo(f2.Name));
				while (num > 15000L)
				{
					try
					{
						files.First<FileInfo>().Delete();
						files = directoryInfo.GetFiles();
						num = files.Sum((FileInfo f) => f.Length) / 1024L;
					}
					catch
					{
						break;
					}
				}
			}
			this.dbConnection.SetLastDateDocumentation(DateTime.Now.ToString("dd.MM.yyyy"));
			bool flag3 = false;
			this.dbConnection.GetDocIsManually(ref flag3);
			bool flag4 = flag3;
			if (flag4)
			{
				int num2 = 0;
				this.dbConnection.GetDocIntervallDays(ref num2);
				this.dbConnection.SetNextDateDocumentation(DateTime.Now.AddDays((double)num2).ToString("dd.MM.yyyy"));
			}
			else
			{
				this.dbConnection.GetDocIsDaily(ref flag3);
				bool flag5 = flag3;
				if (flag5)
				{
					this.dbConnection.SetNextDateDocumentation(DateTime.Now.AddDays(1.0).ToString("dd.MM.yyyy"));
				}
				else
				{
					this.dbConnection.SetNextDateDocumentation(DateTime.Now.AddDays(7.0).ToString("dd.MM.yyyy"));
				}
			}
		}

		// Token: 0x060022F1 RID: 8945 RVA: 0x000E12B4 File Offset: 0x000DF4B4
		private void DoRestoreAnomalyDetection(string backupPath)
		{
			this.ClearDirectory(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Archive", "Model"));
			this.ClearDirectory(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Archive", "Image", "TrainAnomalyDetection"));
			bool flag = ZipHelper.ZipContainsFolder(backupPath, "Archive/");
			if (flag)
			{
				FastZip fastZip = new FastZip();
				fastZip.Password = Backup.zipPassword;
				bool flag2 = this.RestoreAnomalyDetectionImages;
				string text;
				if (flag2)
				{
					text = "Archive/Model/*";
					text += ";+Archive/Image/TrainAnomalyDetection/*";
				}
				else
				{
					text = "Archive/Model/*";
				}
				this.progressUpdateRunning = true;
				new Thread(new ParameterizedThreadStart(this.UpdateProgress))
				{
					Name = "UpdateAIModels"
				}.Start("Updating AI models...");
				fastZip.ExtractZip(backupPath, Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), text);
				this.progressUpdateRunning = false;
			}
		}

		// Token: 0x060022F2 RID: 8946 RVA: 0x000E13B8 File Offset: 0x000DF5B8
		private void ClearDirectory(string dirPath)
		{
			bool flag = Directory.Exists(dirPath);
			if (flag)
			{
				foreach (string path in Directory.GetFiles(dirPath))
				{
					File.Delete(path);
				}
				foreach (string path2 in Directory.GetDirectories(dirPath))
				{
					Directory.Delete(path2, true);
				}
			}
		}

		// Token: 0x04001694 RID: 5780
		public static string guiKey = "/application/GUIVersion";

		// Token: 0x04001695 RID: 5781
		public static string elinosKey = "/application/ElinosVersion";

		// Token: 0x04001696 RID: 5782
		public static string autostartScriptName = "Autostart.bat";

		// Token: 0x04001697 RID: 5783
		private const string restoreProcessName = "RestoreXRS.exe";

		// Token: 0x04001698 RID: 5784
		private readonly string tempBackupPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\tempBackup";

		// Token: 0x04001699 RID: 5785
		public static string restoreProcessPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\RestoreXRS.exe";

		// Token: 0x0400169A RID: 5786
		private bool databaseRestored;

		// Token: 0x0400169B RID: 5787
		private ParameterHandling parHandling;

		// Token: 0x0400169C RID: 5788
		private bool progressUpdateRunning;

		// Token: 0x0400169D RID: 5789
		private bool restoreDatabase;

		// Token: 0x0400169E RID: 5790
		private bool restoreProducts;

		// Token: 0x0400169F RID: 5791
		private bool restoreSystem;

		// Token: 0x040016A0 RID: 5792
		private bool restoreAnomalyDetectionImages;

		// Token: 0x040016A1 RID: 5793
		private ErrorSurvey errorSurvey;

		// Token: 0x040016A2 RID: 5794
		private DBConnection dbConnection;

		// Token: 0x040016A8 RID: 5800
		private DBDataLogs dbDataLogs;

		// Token: 0x040016A9 RID: 5801
		private AccessRightsManager accessManager;

		// Token: 0x0200033B RID: 827
		private enum VersionCompatibility
		{
			// Token: 0x0400213B RID: 8507
			NotCompatible,
			// Token: 0x0400213C RID: 8508
			Compatible,
			// Token: 0x0400213D RID: 8509
			NoInfo
		}
	}
}
