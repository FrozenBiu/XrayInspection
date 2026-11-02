using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using DatabaseConnection;
using Definitions;

namespace GUI_XRS.Datalogger
{
	// Token: 0x0200026C RID: 620
	public class Logger : INotifyPropertyChanged
	{
		// Token: 0x14000110 RID: 272
		// (add) Token: 0x06002FAE RID: 12206 RVA: 0x0013B56C File Offset: 0x0013976C
		// (remove) Token: 0x06002FAF RID: 12207 RVA: 0x0013B5A4 File Offset: 0x001397A4
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x17000CC5 RID: 3269
		// (get) Token: 0x06002FB0 RID: 12208 RVA: 0x0013B5D9 File Offset: 0x001397D9
		// (set) Token: 0x06002FB1 RID: 12209 RVA: 0x0013B5E1 File Offset: 0x001397E1
		public ObservableCollection<string> Paths { get; set; }

		// Token: 0x17000CC6 RID: 3270
		// (get) Token: 0x06002FB2 RID: 12210 RVA: 0x0013B5EC File Offset: 0x001397EC
		// (set) Token: 0x06002FB3 RID: 12211 RVA: 0x0013B604 File Offset: 0x00139804
		public int LogPeriod
		{
			get
			{
				return this.logPeriod;
			}
			set
			{
				this.logPeriod = value;
				this.OnPropertyChanged("LogPeriod");
				bool flag = this.IsRunning;
				if (flag)
				{
					this.logTimer.Change(0, this.LogPeriod);
				}
			}
		}

		// Token: 0x17000CC7 RID: 3271
		// (get) Token: 0x06002FB4 RID: 12212 RVA: 0x0013B644 File Offset: 0x00139844
		// (set) Token: 0x06002FB5 RID: 12213 RVA: 0x0013B65C File Offset: 0x0013985C
		public bool IsRunning
		{
			get
			{
				return this.isRunning;
			}
			set
			{
				this.isRunning = value;
				this.OnPropertyChanged("IsRunning");
			}
		}

		// Token: 0x17000CC8 RID: 3272
		// (get) Token: 0x06002FB6 RID: 12214 RVA: 0x0013B674 File Offset: 0x00139874
		// (set) Token: 0x06002FB7 RID: 12215 RVA: 0x0013B68C File Offset: 0x0013988C
		public bool ShowLoggerIcon
		{
			get
			{
				return this.showLoggerIcon;
			}
			set
			{
				this.showLoggerIcon = value;
				this.OnPropertyChanged("ShowLoggerIcon");
			}
		}

		// Token: 0x06002FB8 RID: 12216 RVA: 0x0013B6A2 File Offset: 0x001398A2
		public Logger(ParameterHandling parHandling, DBConnection dbConnection)
		{
			this.parHandling = parHandling;
			this.dbConnection = dbConnection;
			this.LogPeriod = 10000;
			this.Paths = new ObservableCollection<string>();
		}

		// Token: 0x06002FB9 RID: 12217 RVA: 0x0013B6E0 File Offset: 0x001398E0
		public void StartLog()
		{
			string text = null;
			this.dbConnection.GetSerialNumber(ref text);
			this.logTimer = new Timer(new TimerCallback(this.Log));
			DateTime now = DateTime.Now;
			string text2 = now.Year.ToString();
			text2 += "-";
			text2 += string.Format("{0:D2}", now.Month);
			text2 += "-";
			text2 += string.Format("{0:D2}", now.Day);
			text2 += "_";
			text2 += string.Format("{0:D2}", now.Hour);
			text2 += "-";
			text2 += string.Format("{0:D2}", now.Minute);
			text2 += "-";
			text2 += string.Format("{0:D2}", now.Second);
			bool flag = text != null & text.Length > 0;
			if (flag)
			{
				text2 = text2 + "_" + text;
			}
			this.fileName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Log_" + text2 + ".csv";
			this.IsRunning = true;
			bool flag2 = MainWindowViewModel.accessRightsManager != null;
			if (flag2)
			{
				MainWindowViewModel.accessRightsManager.CurrentUser.PropertyChanged -= this.CurrentUser_PropertyChanged;
				MainWindowViewModel.accessRightsManager.CurrentUser.PropertyChanged += this.CurrentUser_PropertyChanged;
				bool flag3 = MainWindowViewModel.accessRightsManager.CurrentUser.Rights == BuiltInPriviledges.Service;
				if (flag3)
				{
					this.ShowLoggerIcon = true;
				}
				else
				{
					this.ShowLoggerIcon = false;
				}
			}
			this.logTimer.Change(0, this.LogPeriod);
		}

		// Token: 0x06002FBA RID: 12218 RVA: 0x0013B8D0 File Offset: 0x00139AD0
		private void CurrentUser_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			bool flag = e.PropertyName == "Rights";
			if (flag)
			{
				bool flag2 = this.IsRunning && MainWindowViewModel.accessRightsManager.CurrentUser.Rights == BuiltInPriviledges.Service;
				if (flag2)
				{
					this.ShowLoggerIcon = true;
				}
				else
				{
					this.ShowLoggerIcon = false;
				}
			}
		}

		// Token: 0x06002FBB RID: 12219 RVA: 0x0013B92B File Offset: 0x00139B2B
		public void StopLog()
		{
			MainWindowViewModel.accessRightsManager.CurrentUser.PropertyChanged -= this.CurrentUser_PropertyChanged;
			this.logTimer.Dispose();
			this.IsRunning = false;
			this.ShowLoggerIcon = false;
		}

		// Token: 0x06002FBC RID: 12220 RVA: 0x0013B968 File Offset: 0x00139B68
		private void Log(object dummy)
		{
			try
			{
				bool flag = Thread.CurrentThread.Name == null;
				if (flag)
				{
					Thread.CurrentThread.Name = this.ToString();
				}
				this.CreateLogItems();
			}
			catch (Exception ex)
			{
				Exception ex3;
				Exception ex2 = ex3;
				Exception ex = ex2;
				bool flag2 = Application.Current != null && !Application.Current.Dispatcher.CheckAccess();
				if (flag2)
				{
					Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate()
					{
						Dialog.Show("Error", ex.Message);
					}));
				}
			}
		}

		// Token: 0x06002FBD RID: 12221 RVA: 0x0013BA04 File Offset: 0x00139C04
		private void CreateLogItems()
		{
			this.WriteToCSVFile();
		}

		// Token: 0x06002FBE RID: 12222 RVA: 0x0013BA10 File Offset: 0x00139C10
		private void WriteToCSVFile()
		{
			StreamWriter streamWriter = null;
			bool flag = this.Paths != null && this.Paths.Count > 0;
			if (flag)
			{
				try
				{
					bool flag2 = !File.Exists(this.fileName);
					if (flag2)
					{
						streamWriter = new StreamWriter(this.fileName, true);
						streamWriter.Write(this.CsvSeparator);
						for (int i = 0; i < this.Paths.Count - 1; i++)
						{
							streamWriter.Write(this.Paths[i] + this.CsvSeparator);
						}
						streamWriter.Write(this.Paths[this.Paths.Count - 1] + Environment.NewLine);
					}
					else
					{
						streamWriter = new StreamWriter(this.fileName, true);
					}
					streamWriter.Write(DateTime.Now.ToString() + this.CsvSeparator);
					string str;
					for (int j = 0; j < this.Paths.Count - 1; j++)
					{
						str = this.parHandling.ReadPar(this.Paths[j]);
						streamWriter.Write(str + this.CsvSeparator);
					}
					str = this.parHandling.ReadPar(this.Paths[this.Paths.Count - 1]);
					streamWriter.Write(str + Environment.NewLine);
					streamWriter.Close();
				}
				catch (IOException)
				{
					bool flag3 = streamWriter != null;
					if (flag3)
					{
						streamWriter.Close();
					}
				}
				finally
				{
					bool flag4 = streamWriter != null;
					if (flag4)
					{
						streamWriter.Close();
					}
				}
			}
		}

		// Token: 0x06002FBF RID: 12223 RVA: 0x0013BC04 File Offset: 0x00139E04
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x04001DF4 RID: 7668
		private string CsvSeparator = ";";

		// Token: 0x04001DF5 RID: 7669
		private bool isRunning;

		// Token: 0x04001DF6 RID: 7670
		private bool showLoggerIcon;

		// Token: 0x04001DF7 RID: 7671
		private int logPeriod;

		// Token: 0x04001DF8 RID: 7672
		private Timer logTimer;

		// Token: 0x04001DF9 RID: 7673
		private ParameterHandling parHandling;

		// Token: 0x04001DFA RID: 7674
		private string fileName;

		// Token: 0x04001DFB RID: 7675
		private DBConnection dbConnection;
	}
}
