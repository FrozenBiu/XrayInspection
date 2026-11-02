using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using DatabaseConnection;
using Definitions;
using SSTEthernet;

namespace GUI_XRS
{
	// Token: 0x0200011E RID: 286
	public class Logbook
	{
		// Token: 0x17000825 RID: 2085
		// (get) Token: 0x06001B89 RID: 7049 RVA: 0x000A7C93 File Offset: 0x000A5E93
		// (set) Token: 0x06001B8A RID: 7050 RVA: 0x000A7C9B File Offset: 0x000A5E9B
		public ObservableCollection<SSTLogbookEntry> ShownLogbookEntries { get; set; }

		// Token: 0x06001B8B RID: 7051 RVA: 0x000A7CA4 File Offset: 0x000A5EA4
		public Logbook(DBConnection dbConnection)
		{
			this.dbConnection = dbConnection;
			this.allLogbookEntries = new List<SSTLogbookEntry>();
			this.ShownLogbookEntries = new ObservableCollection<SSTLogbookEntry>();
			this.contaminationsEnabled = true;
			this.errorsEnabled = true;
			this.warningsEnabled = true;
			this.infosEnabled = true;
			this.logbookLocalPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\" + this.logbookFolderName;
			dbConnection.GetUsbDriveLetter(ref this.usbLetter);
			this.usbLetter += "\\";
		}

		// Token: 0x06001B8C RID: 7052 RVA: 0x000A7D74 File Offset: 0x000A5F74
		public void CreateLogbook()
		{
			this.stopLogbookCreation = false;
			this.entries.Clear();
			this.dbConnection.GetLogbookEntries(this.entries);
			this.entries.Reverse();
			bool flag = this.entries.Count > 5000;
			if (flag)
			{
				this.nShownEntries = 5000;
			}
			else
			{
				this.nShownEntries = this.entries.Count;
			}
			int i;
			int j;
			for (i = 0; i < this.nShownEntries; i = j + 1)
			{
				bool flag2 = this.stopLogbookCreation;
				if (flag2)
				{
					break;
				}
				while (this.suspendCreation)
				{
					Thread.Sleep(500);
				}
				byte b = 0;
				byte.TryParse(this.entries[i][2], NumberStyles.HexNumber, null, out b);
				bool flag3 = Application.Current != null;
				if (flag3)
				{
					Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate()
					{
						SSTLogbookEntry sstlogbookEntry = new SSTLogbookEntry(this.entries[i].ToList<string>());
						sstlogbookEntry.CreateLogbookText(this.dbConnection);
						this.allLogbookEntries.Add(sstlogbookEntry);
						bool flag4 = this.IsEnabledType(sstlogbookEntry.Type);
						if (flag4)
						{
							this.ShownLogbookEntries.Add(sstlogbookEntry);
						}
					}));
				}
				j = i;
			}
		}

		// Token: 0x06001B8D RID: 7053 RVA: 0x000A7EA4 File Offset: 0x000A60A4
		public void StopLogbookCreation()
		{
			this.stopLogbookCreation = true;
		}

		// Token: 0x06001B8E RID: 7054 RVA: 0x000A7EAE File Offset: 0x000A60AE
		public void DeleteLogbook()
		{
			this.DeleteLogbook(false);
		}

		// Token: 0x06001B8F RID: 7055 RVA: 0x000A7EBC File Offset: 0x000A60BC
		public void DeleteLogbook(bool noPrompt)
		{
			this.stopLogbookCreation = true;
			bool flag = noPrompt || Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtConfirmLogbookDelete"), DialogStyle.Warning, DialogButtons.OKCancel) == Dialog.DialogResults.Ok;
			if (flag)
			{
				bool flag2 = this.dbConnection.DeleteLogbook();
				if (flag2)
				{
					this.ShownLogbookEntries.Clear();
					this.allLogbookEntries.Clear();
				}
			}
		}

		// Token: 0x06001B90 RID: 7056 RVA: 0x000A7F24 File Offset: 0x000A6124
		public SSTLogbookEntry AddEntry(DateTime time, string username, SSTEntryCode code, byte param1, string param2, string param3, string param4, string param5, string param6)
		{
			SSTLogbookEntry lbEntry = null;
			int num = 1500;
			try
			{
				int num2 = 0;
				uint num3 = 0U;
				lock (this)
				{
					this.dbConnection.GetLBIndex(ref num3);
					num3 += 1U;
					this.dbConnection.AddLogbookEntry(num3, time.ToString(new CultureInfo("de-DE")), username, (byte)code, param1, param2, param3, param4, param5, param6);
					bool flag2 = (byte)code < 64 && (byte)code > 1 && code != SSTEntryCode.RejectVerify;
					if (flag2)
					{
						bool flag3 = false;
						this.dbConnection.GetSaveUncommittedErrors(ref flag3);
						bool flag4 = flag3;
						if (flag4)
						{
							this.dbConnection.AddUncommittedError(num3, time.ToString(new CultureInfo("de-DE")), username, (byte)code, param1, param2, param3, param4, param5, param6);
						}
					}
					this.dbConnection.GetMaxLogbookEntries(ref num);
					bool flag5 = num <= 0;
					if (flag5)
					{
						num = 1500;
					}
					this.dbConnection.GetNumLogbookEntries(ref num2);
					bool flag6 = num2 > num;
					if (flag6)
					{
						this.dbConnection.DeleteOldestLogbookEntries(num2 - num);
					}
					lbEntry = new SSTLogbookEntry(num3, time, username, code, param1, param2, param3, param4, param5, param6);
					lbEntry.CreateLogbookText(this.dbConnection);
					this.allLogbookEntries.Insert(0, lbEntry);
					Action action = delegate()
					{
						this.ShownLogbookEntries.Add(lbEntry);
					};
					bool flag7 = this.IsEnabledType(lbEntry.Type);
					if (flag7)
					{
						bool flag8 = Application.Current != null && !Application.Current.Dispatcher.CheckAccess();
						if (flag8)
						{
							Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Send, action);
						}
						else
						{
							action();
						}
					}
				}
			}
			catch (Exception ex)
			{
				Dialog.Show(LocalizationManager.Localize("txtError"), ex.Message, DialogStyle.Error);
			}
			return lbEntry;
		}

		// Token: 0x06001B91 RID: 7057 RVA: 0x000A816C File Offset: 0x000A636C
		public bool SaveLogbook()
		{
			bool result = false;
			DateTime now = DateTime.Now;
			bool flag = !Directory.Exists(this.logbookLocalPath);
			if (flag)
			{
				Directory.CreateDirectory(this.logbookLocalPath);
			}
			string fileName = this.CreateLogbookFilename(this.logbookLocalPath, now);
			List<string> list = Directory.GetFiles(this.logbookLocalPath).ToList<string>();
			list.Sort(new BackupNamesComparer());
			while (list.Count >= 10)
			{
				bool flag2 = list.Count > 0;
				if (flag2)
				{
					File.Delete(list[0]);
					list.RemoveAt(0);
				}
			}
			this.SaveToFile(fileName, now);
			bool flag3 = Directory.Exists(this.usbLetter) && this.usbLetter != "\\";
			if (flag3)
			{
				result = true;
				string text = this.usbLetter + "\\" + this.logbookFolderName;
				bool flag4 = !Directory.Exists(text);
				if (flag4)
				{
					Directory.CreateDirectory(text);
				}
				fileName = this.CreateLogbookFilename(text, now);
				this.SaveToFile(fileName, now);
			}
			return result;
		}

		// Token: 0x06001B92 RID: 7058 RVA: 0x000A8288 File Offset: 0x000A6488
		private void SaveToFile(string fileName, DateTime now)
		{
			StreamWriter streamWriter = new StreamWriter(fileName);
			streamWriter.Write(this.CreateLogFileHeader(now));
			foreach (string[] source in this.entries)
			{
				SSTLogbookEntry sstlogbookEntry = new SSTLogbookEntry(source.ToList<string>());
				sstlogbookEntry.CreateLogbookText(this.dbConnection);
				streamWriter.WriteLine(sstlogbookEntry.EntryTime.ToString(LocalizationManager.CurrentCulture));
				streamWriter.WriteLine(sstlogbookEntry.LogbookText);
				streamWriter.WriteLine();
			}
			streamWriter.Close();
		}

		// Token: 0x06001B93 RID: 7059 RVA: 0x000A8340 File Offset: 0x000A6540
		public string GetDump(SSTEthernetFrame.LogbookType type)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			List<string[]> list = new List<string[]>();
			list.Clear();
			this.dbConnection.GetLogbookEntries(list);
			list.Reverse();
			stringBuilder.Append(this.CreateLogFileHeader(DateTime.Now, true, type));
			foreach (string[] source in list)
			{
				SSTLogbookEntry sstlogbookEntry = new SSTLogbookEntry(source.ToList<string>());
				if (type - SSTEthernetFrame.LogbookType.Intermediate > 1)
				{
					if (type == SSTEthernetFrame.LogbookType.Audit)
					{
						bool flag = sstlogbookEntry.EntryCode.ToString().StartsWith("Audit");
						if (flag)
						{
							sstlogbookEntry.CreateLogbookText(this.dbConnection);
							stringBuilder.Append(sstlogbookEntry.EntryTime.ToString(LocalizationManager.CurrentCulture) + Environment.NewLine);
							stringBuilder.Append(sstlogbookEntry.LogbookText + Environment.NewLine + Environment.NewLine);
						}
					}
				}
				else
				{
					sstlogbookEntry.CreateLogbookText(this.dbConnection);
					stringBuilder.Append(sstlogbookEntry.EntryTime.ToString(LocalizationManager.CurrentCulture) + Environment.NewLine);
					stringBuilder.Append(sstlogbookEntry.LogbookText + Environment.NewLine + Environment.NewLine);
				}
				num++;
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06001B94 RID: 7060 RVA: 0x000A84E0 File Offset: 0x000A66E0
		private string CreateLogFileHeader(DateTime now)
		{
			return this.CreateLogFileHeader(now, false, SSTEthernetFrame.LogbookType.Intermediate);
		}

		// Token: 0x06001B95 RID: 7061 RVA: 0x000A84FC File Offset: 0x000A66FC
		private string CreateLogFileHeader(DateTime now, bool logbookDump, SSTEthernetFrame.LogbookType type)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(SSTEthernetFrame.logbookDumpEndMarker);
			if (logbookDump)
			{
				stringBuilder.AppendLine("Report:        " + type.ToString());
			}
			stringBuilder.AppendLine("Date:          " + now.ToString("d", LocalizationManager.CurrentCulture));
			stringBuilder.AppendLine("Time:          " + now.ToString("HH:mm:ss", LocalizationManager.CurrentCulture));
			stringBuilder.AppendLine("------------------------------------------");
			stringBuilder.AppendLine("");
			return stringBuilder.ToString();
		}

		// Token: 0x06001B96 RID: 7062 RVA: 0x000A85A8 File Offset: 0x000A67A8
		private string CreateLogbookFilename(string logbookPath, DateTime time)
		{
			string text = "";
			StringBuilder stringBuilder = new StringBuilder(logbookPath);
			this.dbConnection.GetSerialNumber(ref text);
			bool flag = text != "";
			if (flag)
			{
				text = "_" + text;
			}
			stringBuilder.Append("\\Logbook_" + time.ToString(this.timeFormatString) + text + Definition.txtFileEnding);
			return stringBuilder.ToString();
		}

		// Token: 0x06001B97 RID: 7063 RVA: 0x000A861C File Offset: 0x000A681C
		public bool FilterContaminations()
		{
			this.contaminationsEnabled = !this.contaminationsEnabled;
			this.FilterEntries();
			return this.contaminationsEnabled;
		}

		// Token: 0x06001B98 RID: 7064 RVA: 0x000A864C File Offset: 0x000A684C
		public bool FilterErrors()
		{
			this.errorsEnabled = !this.errorsEnabled;
			this.FilterEntries();
			return this.errorsEnabled;
		}

		// Token: 0x06001B99 RID: 7065 RVA: 0x000A867C File Offset: 0x000A687C
		public bool FilterWarnings()
		{
			this.warningsEnabled = !this.warningsEnabled;
			this.FilterEntries();
			return this.warningsEnabled;
		}

		// Token: 0x06001B9A RID: 7066 RVA: 0x000A86AC File Offset: 0x000A68AC
		public bool FilterInfos()
		{
			this.infosEnabled = !this.infosEnabled;
			this.FilterEntries();
			return this.infosEnabled;
		}

		// Token: 0x06001B9B RID: 7067 RVA: 0x000A86DC File Offset: 0x000A68DC
		private void FilterEntries()
		{
			this.suspendCreation = true;
			new Thread(new ThreadStart(this.FilterEntriesWithProgress))
			{
				Name = "FilterEntries"
			}.Start();
		}

		// Token: 0x06001B9C RID: 7068 RVA: 0x000A8718 File Offset: 0x000A6918
		private void FilterEntriesWithProgress()
		{
			Logbook.<>c__DisplayClass38_0 CS$<>8__locals1 = new Logbook.<>c__DisplayClass38_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.prog = null;
			bool flag = this.allLogbookEntries.Count > 50;
			if (flag)
			{
				bool flag2 = Application.Current != null;
				if (flag2)
				{
					Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate()
					{
						CS$<>8__locals1.prog = new Progress("Filtering entries...", false);
						CS$<>8__locals1.prog.Show();
					}));
				}
			}
			int i;
			int j;
			for (i = 0; i < this.allLogbookEntries.Count; i = j + 1)
			{
				bool flag3 = this.allLogbookEntries[i].Type == MessageType.Contamination;
				if (flag3)
				{
					bool flag4 = this.contaminationsEnabled && !this.ShownLogbookEntries.Contains(this.allLogbookEntries[i]);
					if (flag4)
					{
						this.AddDispatched(this.ShownLogbookEntries, this.allLogbookEntries[i]);
					}
					bool flag5 = !this.contaminationsEnabled && this.ShownLogbookEntries.Contains(this.allLogbookEntries[i]);
					if (flag5)
					{
						this.RemoveDispatched(this.ShownLogbookEntries, this.allLogbookEntries[i]);
					}
				}
				else
				{
					bool flag6 = this.allLogbookEntries[i].Type == MessageType.Error;
					if (flag6)
					{
						bool flag7 = this.errorsEnabled && !this.ShownLogbookEntries.Contains(this.allLogbookEntries[i]);
						if (flag7)
						{
							this.AddDispatched(this.ShownLogbookEntries, this.allLogbookEntries[i]);
						}
						bool flag8 = !this.errorsEnabled && this.ShownLogbookEntries.Contains(this.allLogbookEntries[i]);
						if (flag8)
						{
							this.RemoveDispatched(this.ShownLogbookEntries, this.allLogbookEntries[i]);
						}
					}
					else
					{
						bool flag9 = this.allLogbookEntries[i].Type == MessageType.Warning;
						if (flag9)
						{
							bool flag10 = this.warningsEnabled && !this.ShownLogbookEntries.Contains(this.allLogbookEntries[i]);
							if (flag10)
							{
								this.AddDispatched(this.ShownLogbookEntries, this.allLogbookEntries[i]);
							}
							bool flag11 = !this.warningsEnabled && this.ShownLogbookEntries.Contains(this.allLogbookEntries[i]);
							if (flag11)
							{
								this.RemoveDispatched(this.ShownLogbookEntries, this.allLogbookEntries[i]);
							}
						}
						else
						{
							bool flag12 = this.infosEnabled && !this.ShownLogbookEntries.Contains(this.allLogbookEntries[i]);
							if (flag12)
							{
								this.AddDispatched(this.ShownLogbookEntries, this.allLogbookEntries[i]);
							}
							bool flag13 = !this.infosEnabled && this.ShownLogbookEntries.Contains(this.allLogbookEntries[i]);
							if (flag13)
							{
								this.RemoveDispatched(this.ShownLogbookEntries, this.allLogbookEntries[i]);
							}
						}
					}
				}
				bool flag14 = i % 20 == 0;
				if (flag14)
				{
					bool flag15 = Application.Current != null;
					if (flag15)
					{
						Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate()
						{
							bool flag17 = CS$<>8__locals1.prog != null;
							if (flag17)
							{
								CS$<>8__locals1.prog.progBar.Value = (double)(i * 100 / CS$<>8__locals1.<>4__this.allLogbookEntries.Count);
							}
						}));
					}
				}
				j = i;
			}
			bool flag16 = Application.Current != null;
			if (flag16)
			{
				Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate()
				{
					bool flag17 = CS$<>8__locals1.prog != null;
					if (flag17)
					{
						CS$<>8__locals1.prog.Close();
					}
				}));
			}
			this.suspendCreation = false;
		}

		// Token: 0x06001B9D RID: 7069 RVA: 0x000A8B1C File Offset: 0x000A6D1C
		private bool IsEnabledType(MessageType type)
		{
			bool flag = type == MessageType.Contamination;
			bool result;
			if (flag)
			{
				result = this.contaminationsEnabled;
			}
			else
			{
				bool flag2 = type == MessageType.Error;
				if (flag2)
				{
					result = this.errorsEnabled;
				}
				else
				{
					bool flag3 = type == MessageType.Warning;
					if (flag3)
					{
						result = this.warningsEnabled;
					}
					else
					{
						result = this.infosEnabled;
					}
				}
			}
			return result;
		}

		// Token: 0x06001B9E RID: 7070 RVA: 0x000A8B6D File Offset: 0x000A6D6D
		public void ClearEntries()
		{
			this.ShownLogbookEntries.Clear();
			this.allLogbookEntries.Clear();
			this.contaminationsEnabled = true;
			this.errorsEnabled = true;
			this.warningsEnabled = true;
			this.infosEnabled = true;
		}

		// Token: 0x06001B9F RID: 7071 RVA: 0x000A8BA4 File Offset: 0x000A6DA4
		private void AddDispatched(IList<SSTLogbookEntry> list, SSTLogbookEntry entry)
		{
			bool flag = Application.Current != null;
			if (flag)
			{
				Application.Current.Dispatcher.Invoke(DispatcherPriority.Send, new Action(delegate()
				{
					list.Add(entry);
				}));
			}
		}

		// Token: 0x06001BA0 RID: 7072 RVA: 0x000A8BF4 File Offset: 0x000A6DF4
		private void RemoveDispatched(IList<SSTLogbookEntry> list, SSTLogbookEntry entry)
		{
			bool flag = Application.Current != null;
			if (flag)
			{
				Application.Current.Dispatcher.Invoke(DispatcherPriority.Send, new Action(delegate()
				{
					list.Remove(entry);
				}));
			}
		}

		// Token: 0x06001BA1 RID: 7073 RVA: 0x000A8C44 File Offset: 0x000A6E44
		public void CreateSampleLogbook()
		{
			bool flag = Application.Current != null;
			if (flag)
			{
				Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate()
				{
					List<SSTLogbookEntry> list = new List<SSTLogbookEntry>();
					SSTLogbookEntry item = new SSTLogbookEntry(1U, DateTime.Now, "Service", SSTEntryCode.Metal, 0, "15", "2", "", "", "");
					list.Add(item);
					item = new SSTLogbookEntry(2U, DateTime.Now, "", SSTEntryCode.Metal, 1, "1054", "7", "197660", "200000", "");
					list.Add(item);
					item = new SSTLogbookEntry(3U, DateTime.Now, "NoUser", SSTEntryCode.Metal, 2, "1554", "22", "12", "", "");
					list.Add(item);
					this.CreateSampleErrors(list);
					this.CreateSampleWarnings(list);
					this.CreateSampleInfos(list);
					this.allLogbookEntries.AddRange(list);
					for (int i = 0; i < list.Count; i++)
					{
						this.ShownLogbookEntries.Add(list[i]);
						list[i].CreateLogbookText(this.dbConnection);
					}
				}));
			}
		}

		// Token: 0x06001BA2 RID: 7074 RVA: 0x000A8C80 File Offset: 0x000A6E80
		private void CreateSampleErrors(List<SSTLogbookEntry> list)
		{
			SSTLogbookEntry item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.Metal, 0, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.DetectorComm, 0, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.CalibrationFault, 0, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.XRayComm, 0, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.XRayFault, 1, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.XRayFault, 2, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.XRayFault, 3, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.XRayFault, 4, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.XRayFault, 5, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.XRayFault, 6, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.XRayFault, 7, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.XRayFault, 8, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.XRayFault, 9, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.XRayFault, 10, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.XRayFault, 11, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.XRayFault, 12, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.XRayFault, 13, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.XRayFault, 14, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.XRayFault, 15, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.XRayFault, 16, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.ProcessingDelay, 0, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.MemoryUsage, 0, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.MotorError, 0, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.LowAirPressure, 0, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.CoolingError, 0, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.BinFull, 1, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.BinFull, 2, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.BinFull, 3, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.BinFull, 4, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.ParamComm, 0, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.ImageComm, 0, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.IOComm, 0, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.IOController, 0, "3", "2", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.RejectVerify, 1, "1", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.RejectVerify, 2, "5", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.RejectVerify, 3, "7", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.RejectVerify, 4, "2", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.XRayLamp, 0, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.Temperature, 0, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.Temperature, 1, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.Voltage, 0, "0", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.Voltage, 0, "1", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.Voltage, 1, "0", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.Voltage, 1, "1", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.ImageProcPC, 0, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.LightBarrier, 1, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.LightBarrier, 2, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.LightBarrier, 3, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.LightBarrier, 4, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.SVTComm, 0, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.SVTError, 1, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.SVTError, 2, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.SVTError, 3, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.SVTError, 4, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.SVTError, 5, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.SVTError, 6, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.SVTError, 7, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.XRayMotor, 0, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.XRayMotor, 1, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.AuditFailedError, 0, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.XrayArcs, 0, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.ProductsMissing, 1, "26", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.SensorBlockedPhotogating, 0, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.SensorBlockedRejectCheck, 1, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.ErrorConsistency, 0, "3202", "3170", "34", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.BinMissing, 1, "", "", "", "", "");
			list.Add(item);
		}

		// Token: 0x06001BA3 RID: 7075 RVA: 0x000A9BF8 File Offset: 0x000A7DF8
		private void CreateSampleWarnings(List<SSTLogbookEntry> list)
		{
			SSTLogbookEntry item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.DatabaseVersion, 0, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.WagoOutOfDate, 0, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.WagoVersionWrong, 0, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.VibroInManualMode, 0, "", "", "", "", "");
			list.Add(item);
		}

		// Token: 0x06001BA4 RID: 7076 RVA: 0x000A9CF8 File Offset: 0x000A7EF8
		private void CreateSampleInfos(List<SSTLogbookEntry> list)
		{
			SSTLogbookEntry item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.MachineTurnedOn, 0, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.MachineTurnedOff, 0, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.ProductChanged, 0, "2", "altes Produkt", "12", "neues Produkt", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.ProductEnded, 0, "2", "altes Produkt", "1522", "14", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.ProductCreated, 0, "3", "Bohnen", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.ProductCopied, 0, "3", "Bohnen", "4", "Fisch", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.ProductDeleted, 0, "4", "Fisch", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.ProductRenamed, 0, "2", "altes Produkt", "Ready meal", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.ProductNumberChanged, 0, "2", "Ready meal", "273", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.SafetySwitchOpenInfo, 0, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.SafetySwitchOpenInfo, 1, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.SafetySwitchOpenInfo, 2, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.SafetySwitchOpenInfo, 3, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.SafetySwitchOpenInfo, 4, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.SafetySwitchOpenInfo, 5, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.SafetySwitchOpenInfo, 6, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.SafetySwitchOpenInfo, 7, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.SafetySwitchOpenInfo, 8, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.SafetySwitchOpenInfo, 9, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.SafetySwitchOpenInfo, 10, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.SafetySwitchOpenInfo, 11, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.SafetySwitchOpenInfo, 12, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.SafetySwitchOpenInfo, 13, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.SafetySwitchOpenInfo, 14, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.SafetySwitchOpenInfo, 15, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.SafetySwitchOpenInfo, 16, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.EmergencyStopPressedInfo, 0, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.AutotrainStarted, 0, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.AutotrainStarted, 1, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.AutotrainEnded, 0, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.AutotrainAborted, 0, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.Retrain, 0, "7", "Test", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.Retrain, 1, "5", "Dose", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.ConveyorSpeed, 0, "40", "52", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.RatedConveyorSpeed, 0, "50", "80", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.FilterActivated, 0, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.FilterActivated, 1, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.FilterActivated, 2, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.FilterActivated, 3, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.FilterDeactivated, 0, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.FilterDeactivated, 1, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.FilterDeactivated, 2, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.FilterDeactivated, 3, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.EdgeParameterChanged, 0, "5", "8", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.EdgeParameterChanged, 1, "5", "3", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.EdgeParameterChanged, 2, "10", "13", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.EdgeParameterChanged, 3, "8", "6", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.EdgeParameterChanged, 4, "8500", "6000", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.SystemTimeChanged, 0, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.CounterParameterChanged, 0, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.CounterParameterChanged, 1, "10", "9", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.CounterParameterChanged, 2, "9", "11", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.CounterParameterChanged, 3, "0", "1200", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.CounterParameterChanged, 4, "6300", "4000", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.CounterParameterChanged, 5, "1", "356", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.CounterParameterChanged, 6, "10000", "97555", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.MetalDuringAudit, 0, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.XrayTurnedOn, 0, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.XrayTurnedOff, 0, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.WeigherParameterChanged, 0, "0", "200000", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.WeigherParameterChanged, 1, "150000", "190000", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.WeigherParameterChanged, 2, "230000", "210000", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.ParameterChanged, 0, "/filter/acquisition/BackgroundNoise/Value", "15", "25", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.UserLoggedIn, 0, "Herbert", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.UserLoggedOut, 0, "Stefan", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.UserChanged, 0, "Herbert", "Stefan", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.UserCreated, 0, "3224821", "Haribo", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.UserParameterChanged, 0, "7", "Markus", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.UserDeleted, 0, "3224821", "Haribo", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.RetrainGroup, 0, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.RetrainGroupFailed, 0, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.SensitivityChanged, 0, "90", "65", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.AuditParameterChanged, 0, "0", "3", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.AuditParameterChanged, 1, "18", "9", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.AuditParameterChanged, 2, "0", "30", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.AuditParameterChanged, 3, "0", "2", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.AuditParameterChanged, 4, "0", "10", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.AuditParameterChanged, 5, "30", "45", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.AuditParameterChanged, 6, "15", "10", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.AuditParameterChanged, 7, "5", "8", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.AuditActivated, 0, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.AuditActivated, 1, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.AuditActivated, 2, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.AuditDeactivated, 0, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.AuditDeactivated, 1, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.AuditDeactivated, 2, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.AuditNeeded, 0, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.AuditOK, 0, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.AuditFailedInfo, 0, "", "", "", "", "");
			list.Add(item);
			item = new SSTLogbookEntry((uint)(list.Count + 1), DateTime.Now, "Admin", SSTEntryCode.AuditStart, 0, "", "", "", "", "");
			list.Add(item);
		}

		// Token: 0x0400125F RID: 4703
		private const int maxLogFiles = 10;

		// Token: 0x04001260 RID: 4704
		private const int maxLogbookEntriesDefault = 1500;

		// Token: 0x04001261 RID: 4705
		private const int shownEntriesInterval = 5000;

		// Token: 0x04001262 RID: 4706
		private readonly string logbookFolderName = "LogbookReports";

		// Token: 0x04001263 RID: 4707
		public readonly string timeFormatString = "yyyy_MM_dd_HH_mm_ss_ff";

		// Token: 0x04001264 RID: 4708
		private string logbookLocalPath;

		// Token: 0x04001265 RID: 4709
		private DBConnection dbConnection;

		// Token: 0x04001266 RID: 4710
		private bool stopLogbookCreation;

		// Token: 0x04001267 RID: 4711
		private string usbLetter = null;

		// Token: 0x04001268 RID: 4712
		private List<string[]> entries = new List<string[]>();

		// Token: 0x04001269 RID: 4713
		private int nShownEntries = 0;

		// Token: 0x0400126A RID: 4714
		private bool contaminationsEnabled;

		// Token: 0x0400126B RID: 4715
		private bool errorsEnabled;

		// Token: 0x0400126C RID: 4716
		private bool warningsEnabled;

		// Token: 0x0400126D RID: 4717
		private bool infosEnabled;

		// Token: 0x0400126E RID: 4718
		private bool suspendCreation = false;

		// Token: 0x0400126F RID: 4719
		private List<SSTLogbookEntry> allLogbookEntries;
	}
}
