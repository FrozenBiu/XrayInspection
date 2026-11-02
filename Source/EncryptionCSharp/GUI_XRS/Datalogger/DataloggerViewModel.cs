using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;
using Common;
using DatabaseConnection;

namespace GUI_XRS.Datalogger
{
	// Token: 0x0200026A RID: 618
	public class DataloggerViewModel : INotifyPropertyChanged
	{
		// Token: 0x17000CBE RID: 3262
		// (get) Token: 0x06002F8B RID: 12171 RVA: 0x0013AC05 File Offset: 0x00138E05
		// (set) Token: 0x06002F8C RID: 12172 RVA: 0x0013AC0D File Offset: 0x00138E0D
		public Logger Datalogger { get; set; }

		// Token: 0x17000CBF RID: 3263
		// (get) Token: 0x06002F8D RID: 12173 RVA: 0x0013AC16 File Offset: 0x00138E16
		// (set) Token: 0x06002F8E RID: 12174 RVA: 0x0013AC1E File Offset: 0x00138E1E
		public RelayCommand ChangeLogPeriodCommand { get; set; }

		// Token: 0x1400010F RID: 271
		// (add) Token: 0x06002F8F RID: 12175 RVA: 0x0013AC28 File Offset: 0x00138E28
		// (remove) Token: 0x06002F90 RID: 12176 RVA: 0x0013AC60 File Offset: 0x00138E60
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x17000CC0 RID: 3264
		// (get) Token: 0x06002F91 RID: 12177 RVA: 0x0013AC95 File Offset: 0x00138E95
		// (set) Token: 0x06002F92 RID: 12178 RVA: 0x0013AC9D File Offset: 0x00138E9D
		public RelayCommand CleanUpCommand { get; set; }

		// Token: 0x17000CC1 RID: 3265
		// (get) Token: 0x06002F93 RID: 12179 RVA: 0x0013ACA8 File Offset: 0x00138EA8
		// (set) Token: 0x06002F94 RID: 12180 RVA: 0x0013ACC0 File Offset: 0x00138EC0
		public bool ShowUSBConnected
		{
			get
			{
				return this.showUSBConnected;
			}
			set
			{
				this.showUSBConnected = value;
				this.OnPropertyChanged("ShowUSBConnected");
			}
		}

		// Token: 0x17000CC2 RID: 3266
		// (get) Token: 0x06002F95 RID: 12181 RVA: 0x0013ACD8 File Offset: 0x00138ED8
		// (set) Token: 0x06002F96 RID: 12182 RVA: 0x0013ACF0 File Offset: 0x00138EF0
		public bool DeactivateParamTree
		{
			get
			{
				return this.deactivateParamTree;
			}
			set
			{
				this.deactivateParamTree = value;
				this.OnPropertyChanged("DeactivateParamTree");
			}
		}

		// Token: 0x06002F97 RID: 12183 RVA: 0x0013AD08 File Offset: 0x00138F08
		public DataloggerViewModel(Logger logger, DBConnection dbConnection)
		{
			this.dbConnection = dbConnection;
			this.Datalogger = logger;
			this.ChangeLogPeriodCommand = new RelayCommand(new Action<object>(this.ChangeLogPeriod));
			this.ShowUSBConnected = false;
			this.showLogFileMessage = true;
			dbConnection.GetUsbDriveLetter(ref this.usbPath);
			this.timerCheckUSB = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Background, new EventHandler(this.SurveyUSB), Application.Current.Dispatcher);
			this.CleanUpCommand = new RelayCommand(new Action<object>(this.CleanUp));
		}

		// Token: 0x06002F98 RID: 12184 RVA: 0x0013ADB8 File Offset: 0x00138FB8
		private void SurveyUSB(object sender, EventArgs e)
		{
			bool flag = Directory.Exists(this.usbPath);
			if (flag)
			{
				bool flag2 = File.Exists(this.usbPath + "\\Log_Datei_Dauertest.txt");
				if (flag2)
				{
					this.ShowUSBConnected = true;
					this.DeactivateParamTree = true;
					this.readLogParametersFromUSB();
					this.writeLogParameters();
				}
				else
				{
					bool flag3 = this.showLogFileMessage;
					if (flag3)
					{
						Dialog.Show(LocalizationManager.Localize("txtWarning"), this.messageNoLogFile, DialogStyle.Warning, DialogButtons.OK);
						this.showLogFileMessage = false;
					}
				}
			}
			else
			{
				this.ShowUSBConnected = false;
				this.showLogFileMessage = true;
				bool flag4 = !this.Datalogger.IsRunning && this.DeactivateParamTree;
				if (flag4)
				{
					this.deleteLogParameters();
					this.DeactivateParamTree = false;
				}
			}
		}

		// Token: 0x06002F99 RID: 12185 RVA: 0x0013AE80 File Offset: 0x00139080
		public void ChangeLogPeriod(object obj)
		{
			this.numKeyboard = new NumKeyboard("LogPeriod", this.Datalogger.LogPeriod.ToString(), NumKeyboardType.TypeDouble);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_InputLogPeriod;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x06002F9A RID: 12186 RVA: 0x0013AED6 File Offset: 0x001390D6
		private void numKeyboard_InputLogPeriod(object sender, KeyboardInputEventArgs e)
		{
			this.Datalogger.LogPeriod = int.Parse(e.Text, CultureInfo.InvariantCulture);
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_InputLogPeriod;
		}

		// Token: 0x06002F9B RID: 12187 RVA: 0x0013AF10 File Offset: 0x00139110
		private void readLogParametersFromUSB()
		{
			try
			{
				using (StreamReader streamReader = new StreamReader(this.usbPath + "\\Log_Datei_Dauertest.txt"))
				{
					while (!streamReader.EndOfStream)
					{
						string text = streamReader.ReadLine();
						string[] array = text.Split(new char[]
						{
							';'
						});
						this.standardLogList.Add(array[1]);
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x06002F9C RID: 12188 RVA: 0x0013AFA4 File Offset: 0x001391A4
		private void writeLogParameters()
		{
			bool flag = this.standardLogList != null && this.Datalogger != null && this.Datalogger.Paths.Count != this.standardLogList.Count;
			if (flag)
			{
				this.Datalogger.LogPeriod = 10000;
				for (int i = 0; i < this.standardLogList.Count; i++)
				{
					this.Datalogger.Paths.Add(this.standardLogList[i] + "/Value");
				}
			}
			this.standardLogList = null;
		}

		// Token: 0x06002F9D RID: 12189 RVA: 0x0013B048 File Offset: 0x00139248
		private void deleteLogParameters()
		{
			this.Datalogger.LogPeriod = 60000;
			bool flag = this.Datalogger != null;
			if (flag)
			{
				int count = this.Datalogger.Paths.Count;
				for (int i = 0; i < count; i++)
				{
					this.Datalogger.Paths.RemoveAt(0);
				}
			}
		}

		// Token: 0x06002F9E RID: 12190 RVA: 0x0013B0AC File Offset: 0x001392AC
		public void SaveLogsToUsb()
		{
			string directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			string[] separator = new string[]
			{
				"\\"
			};
			string text = this.usbPath + "//LogData Raycon";
			bool flag = false;
			try
			{
				bool flag2 = Directory.Exists(this.usbPath);
				if (flag2)
				{
					Directory.CreateDirectory(text);
					foreach (string text2 in Directory.GetFiles(directoryName, "*.csv"))
					{
						string[] array = text2.Split(separator, 100, StringSplitOptions.None);
						string str = array[array.Length - 1];
						string destFileName = text + "//" + str;
						File.Copy(text2, destFileName, true);
						flag = true;
					}
					bool flag3 = flag;
					if (flag3)
					{
						Dialog.Show(LocalizationManager.Localize("txtSuccess"), LocalizationManager.Localize("txtSaveSuccessful"), DialogStyle.Ok, DialogButtons.OK);
					}
					else
					{
						Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("msgLogDataAvailable"), DialogStyle.Warning, DialogButtons.OK);
					}
				}
				else
				{
					Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtNoUSB"), DialogStyle.Warning, DialogButtons.OK);
				}
			}
			catch (UnauthorizedAccessException)
			{
				Dialog.Show(LocalizationManager.Localize("txtError"), string.Concat(new string[]
				{
					LocalizationManager.Localize("msgPathAccess"),
					" \"",
					Path.GetDirectoryName(this.usbPath),
					"\" ",
					LocalizationManager.Localize("msgForbidden")
				}), DialogStyle.Error);
			}
			catch
			{
				Dialog.Show(LocalizationManager.Localize("txtError"), string.Concat(new string[]
				{
					LocalizationManager.Localize("msgPathAccess"),
					" \"",
					Path.GetDirectoryName(this.usbPath),
					"\" ",
					LocalizationManager.Localize("msgForbidden")
				}), DialogStyle.Error);
			}
		}

		// Token: 0x06002F9F RID: 12191 RVA: 0x0013B2AC File Offset: 0x001394AC
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x06002FA0 RID: 12192 RVA: 0x0013B2D9 File Offset: 0x001394D9
		private void CleanUp(object obj)
		{
			this.timerCheckUSB.Stop();
		}

		// Token: 0x04001DDE RID: 7646
		private NumKeyboard numKeyboard;

		// Token: 0x04001DDF RID: 7647
		private string messageNoLogFile = "Auf dem USB Stick ist keine Log-Datei vorhanden.";

		// Token: 0x04001DE2 RID: 7650
		private DispatcherTimer timerCheckUSB;

		// Token: 0x04001DE3 RID: 7651
		private DBConnection dbConnection;

		// Token: 0x04001DE5 RID: 7653
		private string usbPath;

		// Token: 0x04001DE6 RID: 7654
		private List<string> standardLogList = new List<string>();

		// Token: 0x04001DE7 RID: 7655
		private const string nameOfLogParameterfile = "Log_Datei_Dauertest.txt";

		// Token: 0x04001DE8 RID: 7656
		private bool showLogFileMessage;

		// Token: 0x04001DE9 RID: 7657
		private bool showUSBConnected;

		// Token: 0x04001DEB RID: 7659
		private bool deactivateParamTree;
	}
}
