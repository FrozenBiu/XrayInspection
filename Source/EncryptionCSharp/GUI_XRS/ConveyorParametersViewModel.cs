using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Threading;
using Common;
using DatabaseConnection;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x02000082 RID: 130
	internal class ConveyorParametersViewModel : MonitoringViewModel, INotifyPropertyChanged
	{
		// Token: 0x14000045 RID: 69
		// (add) Token: 0x06000CE0 RID: 3296 RVA: 0x0004BED8 File Offset: 0x0004A0D8
		// (remove) Token: 0x06000CE1 RID: 3297 RVA: 0x0004BF10 File Offset: 0x0004A110
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x170003E9 RID: 1001
		// (get) Token: 0x06000CE2 RID: 3298 RVA: 0x0004BF45 File Offset: 0x0004A145
		// (set) Token: 0x06000CE3 RID: 3299 RVA: 0x0004BF4D File Offset: 0x0004A14D
		public RelayCommand CleanUpCommand { get; set; }

		// Token: 0x170003EA RID: 1002
		// (get) Token: 0x06000CE4 RID: 3300 RVA: 0x0004BF58 File Offset: 0x0004A158
		// (set) Token: 0x06000CE5 RID: 3301 RVA: 0x0004BF70 File Offset: 0x0004A170
		public List<MonitoringItem> Items
		{
			get
			{
				return this.items;
			}
			set
			{
				this.items = value;
				this.OnPropertyChanged("Items");
			}
		}

		// Token: 0x170003EB RID: 1003
		// (get) Token: 0x06000CE6 RID: 3302 RVA: 0x0004BF86 File Offset: 0x0004A186
		// (set) Token: 0x06000CE7 RID: 3303 RVA: 0x0004BF8E File Offset: 0x0004A18E
		public BoundLocalizedText StrTitle { get; set; }

		// Token: 0x170003EC RID: 1004
		// (get) Token: 0x06000CE8 RID: 3304 RVA: 0x0004BF97 File Offset: 0x0004A197
		// (set) Token: 0x06000CE9 RID: 3305 RVA: 0x0004BF9F File Offset: 0x0004A19F
		public BoundLocalizedText StrCurrentVal { get; set; }

		// Token: 0x170003ED RID: 1005
		// (get) Token: 0x06000CEA RID: 3306 RVA: 0x0004BFA8 File Offset: 0x0004A1A8
		// (set) Token: 0x06000CEB RID: 3307 RVA: 0x0004BFB0 File Offset: 0x0004A1B0
		public BoundLocalizedText StrLimits { get; set; }

		// Token: 0x170003EE RID: 1006
		// (get) Token: 0x06000CEC RID: 3308 RVA: 0x0004BFB9 File Offset: 0x0004A1B9
		// (set) Token: 0x06000CED RID: 3309 RVA: 0x0004BFC1 File Offset: 0x0004A1C1
		public BoundLocalizedText StrOperatingHours { get; set; }

		// Token: 0x170003EF RID: 1007
		// (get) Token: 0x06000CEE RID: 3310 RVA: 0x0004BFCA File Offset: 0x0004A1CA
		// (set) Token: 0x06000CEF RID: 3311 RVA: 0x0004BFD2 File Offset: 0x0004A1D2
		public BoundLocalizedText StrConveyorDistance { get; set; }

		// Token: 0x170003F0 RID: 1008
		// (get) Token: 0x06000CF0 RID: 3312 RVA: 0x0004BFDC File Offset: 0x0004A1DC
		// (set) Token: 0x06000CF1 RID: 3313 RVA: 0x0004BFF4 File Offset: 0x0004A1F4
		public bool EnableLimits
		{
			get
			{
				return this.enableLimits;
			}
			set
			{
				this.enableLimits = value;
				this.OnPropertyChanged("EnableLimits");
			}
		}

		// Token: 0x06000CF2 RID: 3314 RVA: 0x0004C00C File Offset: 0x0004A20C
		public ConveyorParametersViewModel(DBDataLogs dBDataLogs, DBConnection dBConnection, ParameterHandling parameterHandling, bool EnableLimits, ErrorSurvey errorSurvey)
		{
			this.RegisterLocalizedText();
			this.dBDataLogs = dBDataLogs;
			this.dBConnection = dBConnection;
			this.parameterHandling = parameterHandling;
			this.EnableLimits = EnableLimits;
			this.errorSurvey = errorSurvey;
			this.Initialize();
			this.CleanUpCommand = new RelayCommand(new Action<object>(this.CleanUp));
			this.timerDataLogsSeconds = new DispatcherTimer(new TimeSpan(0, 0, 2), DispatcherPriority.Normal, new EventHandler(this.UpdateValues), Application.Current.Dispatcher);
			this.timerDataLogsSeconds.Start();
		}

		// Token: 0x06000CF3 RID: 3315 RVA: 0x0004C0C0 File Offset: 0x0004A2C0
		private void UpdateValues(object sender, EventArgs e)
		{
			bool flag = this.parameterHandling.ReadPar(Definition.conveyorIsRunning) == "1";
			bool flag2 = flag;
			if (flag2)
			{
				this.minutes += 0.03333333333333333;
				this.items[0].CurrentValue = Math.Round(this.minutes / 60.0, 2).ToString();
				double num = 0.0;
				double.TryParse(this.parameterHandling.ReadPar(Definition.conveyingSpeedPath), out num);
				this.meters += 2.0 * (num / 1000.0);
				this.items[1].CurrentValue = Math.Round(this.meters / 1000.0, 2).ToString();
			}
		}

		// Token: 0x06000CF4 RID: 3316 RVA: 0x0004C1B4 File Offset: 0x0004A3B4
		private void Initialize()
		{
			uint num = 0U;
			double num2 = 0.0;
			List<string> list = new List<string>();
			List<string> list2 = new List<string>();
			List<string> list3 = new List<string>();
			List<string> list4 = new List<string>();
			List<string> list5 = new List<string>();
			List<string> list6 = new List<string>();
			List<string> list7 = new List<string>();
			List<string> list8 = new List<string>();
			List<string> list9 = new List<string>();
			string text = "";
			this.dBDataLogs.GetDataLogsSettingsEntry("MonitoringMessagesOnOff", ref text);
			bool flag = text.Equals("1");
			if (flag)
			{
				this.showMessages = true;
			}
			text = "";
			this.dBDataLogs.GetLimitsEntry("OperatingHoursConveyor", ref text);
			this.dBDataLogs.OperatingCounterGetDoubleEntry("ConveyorOperatingMinutes", ref num2);
			this.items.Add(new MonitoringItem(this.StrOperatingHours.LocalizedText + " [h]", Math.Round(num2 / 60.0, 2).ToString(), text, "", this.EnableLimits, "txtOperatingHours", "txtConveyor"));
			this.dBDataLogs.OperatingCounterGetIntEntry("ConveyorOperatingHours", ref num);
			this.minutes = num2;
			this.dBDataLogs.GetLimitsEntry("OperatingDistance", ref text);
			this.dBDataLogs.OperatingCounterGetIntEntry("ConveyorDistanceMeter", ref num);
			this.items.Add(new MonitoringItem(this.StrConveyorDistance.LocalizedText + " [km]", Math.Round(num / 1000.0, 2).ToString(), text, "", this.EnableLimits, "txtConveyorDistance", ""));
			this.meters = num;
		}

		// Token: 0x06000CF5 RID: 3317 RVA: 0x0004C364 File Offset: 0x0004A564
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x06000CF6 RID: 3318 RVA: 0x0004C394 File Offset: 0x0004A594
		private void RegisterLocalizedText()
		{
			this.StrLimits = new BoundLocalizedText("txtLimits");
			this.StrCurrentVal = new BoundLocalizedText("txtCurrentValue");
			this.StrTitle = new BoundLocalizedText("txtConveyor");
			this.StrOperatingHours = new BoundLocalizedText("txtOperatingHours");
			this.StrConveyorDistance = new BoundLocalizedText("txtConveyorDistance");
		}

		// Token: 0x06000CF7 RID: 3319 RVA: 0x0004C3F8 File Offset: 0x0004A5F8
		private void DeregisterLocalizedText()
		{
			MainWindow.localizationManager.Deregister(this.StrLimits);
			MainWindow.localizationManager.Deregister(this.StrCurrentVal);
			MainWindow.localizationManager.Deregister(this.StrTitle);
			MainWindow.localizationManager.Deregister(this.StrConveyorDistance);
			MainWindow.localizationManager.Deregister(this.StrOperatingHours);
		}

		// Token: 0x06000CF8 RID: 3320 RVA: 0x0004C45C File Offset: 0x0004A65C
		public override void getNumKeyboardNumber(MonitoringItem mItem, int limitNumb)
		{
			this.limitNumber = limitNumb;
			this.selectedItem = mItem;
			bool flag = this.limitNumber == 1;
			string oldValue;
			if (flag)
			{
				oldValue = mItem.Limit;
			}
			else
			{
				oldValue = mItem.Limit2;
			}
			this.numKeyboard = new NumKeyboard(mItem.Name, oldValue, NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.keyboard_TextInputKeyboardNumber;
			this.numKeyboard.Show();
		}

		// Token: 0x06000CF9 RID: 3321 RVA: 0x0004C4D8 File Offset: 0x0004A6D8
		private void keyboard_TextInputKeyboardNumber(object sender, KeyboardInputEventArgs e)
		{
			bool flag = e.Text != null;
			if (flag)
			{
				bool flag2 = this.limitNumber == 1;
				if (flag2)
				{
					bool flag3 = e.Text != "" && e.Text != "0";
					if (flag3)
					{
						this.selectedItem.Limit = e.Text;
					}
					else
					{
						this.selectedItem.Limit = "-";
					}
					this.SetNewLimit(this.selectedItem);
				}
				else
				{
					this.selectedItem.Limit2 = e.Text;
				}
			}
			this.numKeyboard.TextInputKeyboard -= this.keyboard_TextInputKeyboardNumber;
		}

		// Token: 0x06000CFA RID: 3322 RVA: 0x0004C590 File Offset: 0x0004A790
		public void SetNewLimit(MonitoringItem item)
		{
			bool flag = item.Name.Equals(this.StrConveyorDistance.LocalizedText + " [km]");
			if (flag)
			{
				this.dBDataLogs.LimitsAddEntry("DateTimeOperatingDistance", "OperatingDistance", item.Limit, DateTime.Now.ToString(new CultureInfo("de-DE")));
			}
			else
			{
				bool flag2 = item.Name.Equals(this.StrOperatingHours.LocalizedText + " [h]");
				if (flag2)
				{
					this.dBDataLogs.LimitsAddEntry("DateTimeOperatingHoursConveyor", "OperatingHoursConveyor", item.Limit, DateTime.Now.ToString(new CultureInfo("de-DE")));
				}
			}
			bool flag3 = this.showMessages;
			if (flag3)
			{
				this.CheckLimit(item);
			}
		}

		// Token: 0x06000CFB RID: 3323 RVA: 0x0004C664 File Offset: 0x0004A864
		public void CheckLimit(MonitoringItem item)
		{
			bool flag = !item.Limit.Equals("") && !item.Limit.Equals("-");
			if (flag)
			{
				string text = "";
				this.dBDataLogs.GetDataLogsSettingsEntry("MessageLevel", ref text);
				bool flag2 = text.Equals("Info");
				SSTEntryCode code;
				if (flag2)
				{
					code = SSTEntryCode.LimitsInfo;
				}
				else
				{
					bool flag3 = text.Equals("Warning");
					if (flag3)
					{
						code = SSTEntryCode.LimitsWarning;
					}
					else
					{
						code = SSTEntryCode.LimitsError;
					}
				}
				text = "";
				this.dBDataLogs.GetDataLogsSettingsEntry("MessageLevel90Perc", ref text);
				bool flag4 = text.Equals("Info");
				SSTEntryCode code2;
				if (flag4)
				{
					code2 = SSTEntryCode.Limits90PerInfo;
				}
				else
				{
					code2 = SSTEntryCode.Limits90PerWarning;
				}
				double num;
				double.TryParse(item.Limit.Replace(',', '.'), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out num);
				double num2;
				double.TryParse(item.CurrentValue.Replace(',', '.'), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out num2);
				bool flag5 = num2 > num;
				if (flag5)
				{
					this.errorSurvey.PostMessageMonitoring(code, 0, item.ressourceText, item.ressourceText2, num.ToString(), num2.ToString());
				}
				else
				{
					bool flag6 = num2 >= 0.9 * num;
					if (flag6)
					{
						this.errorSurvey.PostMessageMonitoring(code2, 0, item.ressourceText, item.ressourceText2, num.ToString(), num2.ToString());
					}
				}
			}
		}

		// Token: 0x06000CFC RID: 3324 RVA: 0x0004C7E0 File Offset: 0x0004A9E0
		public override void ShowDiagramm()
		{
			CtrlDataPlots usrControl = new CtrlDataPlots(this.parameterHandling, this.dBConnection, this.dBDataLogs, DataModesMonitoring.conveyor);
			Subwindow.Show(usrControl, SubwindowButtons.BackAndHome);
		}

		// Token: 0x06000CFD RID: 3325 RVA: 0x0004C80F File Offset: 0x0004AA0F
		private void CleanUp(object obj)
		{
			this.timerDataLogsSeconds.Stop();
			this.DeregisterLocalizedText();
			this.Items.Clear();
			this.Items = null;
		}

		// Token: 0x0400086F RID: 2159
		private NumKeyboard numKeyboard;

		// Token: 0x04000870 RID: 2160
		private MonitoringItem selectedItem;

		// Token: 0x04000871 RID: 2161
		private int limitNumber;

		// Token: 0x04000872 RID: 2162
		private List<MonitoringItem> items = new List<MonitoringItem>();

		// Token: 0x04000878 RID: 2168
		private DBDataLogs dBDataLogs;

		// Token: 0x04000879 RID: 2169
		private ParameterHandling parameterHandling;

		// Token: 0x0400087A RID: 2170
		private DBConnection dBConnection;

		// Token: 0x0400087B RID: 2171
		private bool enableLimits = false;

		// Token: 0x0400087C RID: 2172
		private ErrorSurvey errorSurvey;

		// Token: 0x0400087D RID: 2173
		private double minutes;

		// Token: 0x0400087E RID: 2174
		private double meters;

		// Token: 0x0400087F RID: 2175
		private DispatcherTimer timerDataLogsSeconds;

		// Token: 0x04000880 RID: 2176
		private bool showMessages = false;
	}
}
