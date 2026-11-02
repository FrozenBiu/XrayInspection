using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using Common;
using DatabaseConnection;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x0200008D RID: 141
	internal class OperationalParametersViewModel : MonitoringViewModel, INotifyPropertyChanged
	{
		// Token: 0x1400004C RID: 76
		// (add) Token: 0x06000E74 RID: 3700 RVA: 0x00057290 File Offset: 0x00055490
		// (remove) Token: 0x06000E75 RID: 3701 RVA: 0x000572C8 File Offset: 0x000554C8
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x1700046E RID: 1134
		// (get) Token: 0x06000E76 RID: 3702 RVA: 0x000572FD File Offset: 0x000554FD
		// (set) Token: 0x06000E77 RID: 3703 RVA: 0x00057305 File Offset: 0x00055505
		public RelayCommand CleanUpCommand { get; set; }

		// Token: 0x1700046F RID: 1135
		// (get) Token: 0x06000E78 RID: 3704 RVA: 0x00057310 File Offset: 0x00055510
		// (set) Token: 0x06000E79 RID: 3705 RVA: 0x00057328 File Offset: 0x00055528
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

		// Token: 0x17000470 RID: 1136
		// (get) Token: 0x06000E7A RID: 3706 RVA: 0x0005733E File Offset: 0x0005553E
		// (set) Token: 0x06000E7B RID: 3707 RVA: 0x00057346 File Offset: 0x00055546
		public BoundLocalizedText StrTitle { get; set; }

		// Token: 0x17000471 RID: 1137
		// (get) Token: 0x06000E7C RID: 3708 RVA: 0x0005734F File Offset: 0x0005554F
		// (set) Token: 0x06000E7D RID: 3709 RVA: 0x00057357 File Offset: 0x00055557
		public BoundLocalizedText StrCurrentVal { get; set; }

		// Token: 0x17000472 RID: 1138
		// (get) Token: 0x06000E7E RID: 3710 RVA: 0x00057360 File Offset: 0x00055560
		// (set) Token: 0x06000E7F RID: 3711 RVA: 0x00057368 File Offset: 0x00055568
		public BoundLocalizedText StrLimits { get; set; }

		// Token: 0x17000473 RID: 1139
		// (get) Token: 0x06000E80 RID: 3712 RVA: 0x00057371 File Offset: 0x00055571
		// (set) Token: 0x06000E81 RID: 3713 RVA: 0x00057379 File Offset: 0x00055579
		public BoundLocalizedText StrTemperature { get; set; }

		// Token: 0x17000474 RID: 1140
		// (get) Token: 0x06000E82 RID: 3714 RVA: 0x00057382 File Offset: 0x00055582
		// (set) Token: 0x06000E83 RID: 3715 RVA: 0x0005738A File Offset: 0x0005558A
		public BoundLocalizedText StrOperatingHours { get; set; }

		// Token: 0x17000475 RID: 1141
		// (get) Token: 0x06000E84 RID: 3716 RVA: 0x00057393 File Offset: 0x00055593
		// (set) Token: 0x06000E85 RID: 3717 RVA: 0x0005739B File Offset: 0x0005559B
		public BoundLocalizedText StrProductCounterGood { get; set; }

		// Token: 0x17000476 RID: 1142
		// (get) Token: 0x06000E86 RID: 3718 RVA: 0x000573A4 File Offset: 0x000555A4
		// (set) Token: 0x06000E87 RID: 3719 RVA: 0x000573AC File Offset: 0x000555AC
		public BoundLocalizedText StrProductCounterBad { get; set; }

		// Token: 0x17000477 RID: 1143
		// (get) Token: 0x06000E88 RID: 3720 RVA: 0x000573B5 File Offset: 0x000555B5
		// (set) Token: 0x06000E89 RID: 3721 RVA: 0x000573BD File Offset: 0x000555BD
		public BoundLocalizedText StrMachine { get; set; }

		// Token: 0x17000478 RID: 1144
		// (get) Token: 0x06000E8A RID: 3722 RVA: 0x000573C6 File Offset: 0x000555C6
		// (set) Token: 0x06000E8B RID: 3723 RVA: 0x000573CE File Offset: 0x000555CE
		public BoundLocalizedText StrPerDay { get; set; }

		// Token: 0x17000479 RID: 1145
		// (get) Token: 0x06000E8C RID: 3724 RVA: 0x000573D7 File Offset: 0x000555D7
		// (set) Token: 0x06000E8D RID: 3725 RVA: 0x000573DF File Offset: 0x000555DF
		public BoundLocalizedText StrProduct { get; set; }

		// Token: 0x1700047A RID: 1146
		// (get) Token: 0x06000E8E RID: 3726 RVA: 0x000573E8 File Offset: 0x000555E8
		// (set) Token: 0x06000E8F RID: 3727 RVA: 0x00057400 File Offset: 0x00055600
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

		// Token: 0x06000E90 RID: 3728 RVA: 0x00057418 File Offset: 0x00055618
		public OperationalParametersViewModel(DBDataLogs dBDataLogs, DBConnection dBConnection, ParameterHandling parameterHandling, bool EnableLimits, ErrorSurvey errorSurvey, MainWindowViewModel mainWindow)
		{
			this.RegisterLocalizedText();
			this.dBDataLogs = dBDataLogs;
			this.parameterHandling = parameterHandling;
			this.dBConnection = dBConnection;
			this.EnableLimits = EnableLimits;
			this.errorSurvey = errorSurvey;
			this.mainWindow = mainWindow;
			MachineType machineType = MachineType.RayconDPlus;
			dBConnection.GetMachineType(ref machineType);
			bool flag = machineType != MachineType.RayconEX2A;
			if (flag)
			{
				this.isEX2 = false;
			}
			else
			{
				this.isEX2 = true;
			}
			this.Initialize();
			this.CleanUpCommand = new RelayCommand(new Action<object>(this.CleanUp));
			this.timerDataLogsSeconds = new DispatcherTimer(new TimeSpan(0, 0, 2), DispatcherPriority.Normal, new EventHandler(this.UpdateValues), Application.Current.Dispatcher);
			this.timerDataLogsSeconds.Start();
		}

		// Token: 0x06000E91 RID: 3729 RVA: 0x00057534 File Offset: 0x00055734
		private void IncreaseReject()
		{
			this.reject += 1U;
			this.rejectPerDay += 1U;
			this.items[2].CurrentValue = this.reject.ToString();
			this.items[3].CurrentValue = this.rejectPerDay.ToString();
		}

		// Token: 0x06000E92 RID: 3730 RVA: 0x00057598 File Offset: 0x00055798
		private void IncreaseThroughput()
		{
			this.throughput += 1U;
			this.throughputPerDay += 1U;
			this.items[4].CurrentValue = this.throughput.ToString();
			this.items[5].CurrentValue = this.throughputPerDay.ToString();
		}

		// Token: 0x06000E93 RID: 3731 RVA: 0x000575FC File Offset: 0x000557FC
		private void UpdateValues(object sender, EventArgs e)
		{
			bool flag = !this.isEX2;
			if (flag)
			{
				this.items[1].CurrentValue = this.parameterHandling.ReadPar("/hardware/ios/Temp_00/Value");
			}
			this.minutes += 0.03333333333333333;
			this.items[0].CurrentValue = Math.Round(this.minutes / 60.0, 2).ToString();
		}

		// Token: 0x06000E94 RID: 3732 RVA: 0x00057680 File Offset: 0x00055880
		private void Initialize()
		{
			uint num = 0U;
			List<string> list = new List<string>();
			List<string> list2 = new List<string>();
			List<string> list3 = new List<string>();
			List<string> list4 = new List<string>();
			string text = "";
			double num2 = 0.0;
			this.dBDataLogs.GetDataLogsSettingsEntry("MonitoringMessagesOnOff", ref text);
			bool flag = text.Equals("1");
			if (flag)
			{
				this.showMessages = true;
			}
			text = "";
			this.dBDataLogs.OperatingCounterGetDoubleEntry("MachineOperatingHours", ref num2);
			this.dBDataLogs.GetLimitsEntry("OperatingHours", ref text);
			this.items.Add(new MonitoringItem(this.StrOperatingHours.LocalizedText + " " + this.StrMachine.LocalizedText + " [h]", num2.ToString(), text, "", this.EnableLimits, "txtOperatingHours", "txtMachine"));
			this.dBDataLogs.OperatingCounterGetDoubleEntry("MachineOperatingMinutes", ref this.minutes);
			bool flag2 = !this.isEX2;
			if (flag2)
			{
				this.dBDataLogs.TemperaturesGetEntries("Temperatures", ref list, ref list2, ref list3, ref list4);
				this.dBDataLogs.GetLimitsEntry("Temperature", ref text);
				bool flag3 = list2.Count > 0;
				if (flag3)
				{
					this.items.Add(new MonitoringItem(this.StrTemperature.LocalizedText + " " + this.StrMachine.LocalizedText + " [°C]", list2.Last<string>(), text, "", this.EnableLimits, "txtTemperature", "txtMachine"));
				}
				else
				{
					this.items.Add(new MonitoringItem(this.StrTemperature.LocalizedText + " " + this.StrMachine.LocalizedText + " [°C]", "-", text, "", this.EnableLimits));
				}
				text = "";
			}
			MachineMode machineMode = MachineMode.Standard;
			this.dBConnection.GetMachineMode(ref machineMode);
			bool flag4 = machineMode != MachineMode.Bulk;
			if (flag4)
			{
				this.dBDataLogs.OperatingCounterGetIntEntry("RejectIndex", ref num);
				this.dBDataLogs.GetLimitsEntry("Reject", ref text);
				bool flag5 = machineMode == MachineMode.BulkObject;
				if (flag5)
				{
					this.items.Add(new MonitoringItem(this.StrProductCounterBad.LocalizedText + " (Bulk Object " + this.StrProduct.LocalizedText + " )", num.ToString(), text, "", this.EnableLimits, "txtProductCounterBad", ""));
				}
				else
				{
					this.items.Add(new MonitoringItem(this.StrProductCounterBad.LocalizedText, num.ToString(), text, "", this.EnableLimits, "txtProductCounterBad", ""));
				}
				this.reject = num;
				this.dBDataLogs.OnOffTimesGetEntryOfDate(DateTime.Now.ToString("d", new CultureInfo("de-DE")), ref text, "Reject");
				double.TryParse(text, out num2);
				this.rejectPerDay = (uint)num2;
				this.dBDataLogs.GetLimitsEntry("RejectPerDay", ref text);
				bool flag6 = machineMode == MachineMode.BulkObject;
				if (flag6)
				{
					this.items.Add(new MonitoringItem(string.Concat(new string[]
					{
						this.StrProductCounterBad.LocalizedText,
						" ",
						this.StrPerDay.LocalizedText,
						" (Bulk Object ",
						this.StrProduct.LocalizedText,
						" )"
					}), this.rejectPerDay.ToString(), text, "", this.EnableLimits, "txtProductCounterBad", "txtPerDay"));
				}
				else
				{
					this.items.Add(new MonitoringItem(this.StrProductCounterBad.LocalizedText + " " + this.StrPerDay.LocalizedText, this.rejectPerDay.ToString(), text, "", this.EnableLimits, "txtProductCounterBad", "txtPerDay"));
				}
				this.dBDataLogs.OperatingCounterGetIntEntry("ThroughputIndex", ref num);
				this.dBDataLogs.GetLimitsEntry("Throughput", ref text);
				bool flag7 = machineMode == MachineMode.BulkObject;
				if (flag7)
				{
					this.items.Add(new MonitoringItem(this.StrProductCounterGood.LocalizedText + " (Bulk Object " + this.StrProduct.LocalizedText + " )", (num - this.reject).ToString(), text, "", this.EnableLimits, "txtProductCounterGood", ""));
				}
				else
				{
					this.items.Add(new MonitoringItem(this.StrProductCounterGood.LocalizedText, (num - this.reject).ToString(), text, "", this.EnableLimits, "txtProductCounterGood", ""));
				}
				this.throughput = num - this.reject;
				this.dBDataLogs.OnOffTimesGetEntryOfDate(DateTime.Now.ToString("d", new CultureInfo("de-DE")), ref text, "Throughput");
				double.TryParse(text, out num2);
				this.throughputPerDay = (uint)num2;
				this.dBDataLogs.GetLimitsEntry("ThroughputPerDay", ref text);
				bool flag8 = machineMode == MachineMode.BulkObject;
				if (flag8)
				{
					this.items.Add(new MonitoringItem(string.Concat(new string[]
					{
						this.StrProductCounterGood.LocalizedText,
						" ",
						this.StrPerDay.LocalizedText,
						" (Bulk Object ",
						this.StrProduct.LocalizedText,
						" )"
					}), this.throughputPerDay.ToString(), text, "", this.EnableLimits, "txtProductCounterGood", "txtPerDay"));
				}
				else
				{
					this.items.Add(new MonitoringItem(this.StrProductCounterGood.LocalizedText + " " + this.StrPerDay.LocalizedText, this.throughputPerDay.ToString(), text, "", this.EnableLimits, "txtProductCounterGood", "txtPerDay"));
				}
				this.mainWindow.ThroughputIncreased += this.IncreaseThroughput;
				this.mainWindow.RejectIncreased += this.IncreaseReject;
			}
			text = "";
			this.dBDataLogs.GetDefaultLimit("Temperature", ref text);
			double.TryParse(text, out this.limitDefaultTemp);
		}

		// Token: 0x06000E95 RID: 3733 RVA: 0x00057D0C File Offset: 0x00055F0C
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x06000E96 RID: 3734 RVA: 0x00057D3C File Offset: 0x00055F3C
		private void RegisterLocalizedText()
		{
			this.StrTemperature = new BoundLocalizedText("txtTemperature");
			this.StrLimits = new BoundLocalizedText("txtLimits");
			this.StrCurrentVal = new BoundLocalizedText("txtCurrentValue");
			this.StrTitle = new BoundLocalizedText("txtDeviceData");
			this.StrOperatingHours = new BoundLocalizedText("txtOperatingHours");
			this.StrProductCounterGood = new BoundLocalizedText("txtProductCounterGood");
			this.StrProductCounterBad = new BoundLocalizedText("txtProductCounterBad");
			this.StrMachine = new BoundLocalizedText("txtMachine");
			this.StrPerDay = new BoundLocalizedText("txtPerDay");
			this.StrProduct = new BoundLocalizedText("txtApplication");
		}

		// Token: 0x06000E97 RID: 3735 RVA: 0x00057DF4 File Offset: 0x00055FF4
		private void DeregisterLocalizedText()
		{
			MainWindow.localizationManager.Deregister(this.StrTemperature);
			MainWindow.localizationManager.Deregister(this.StrLimits);
			MainWindow.localizationManager.Deregister(this.StrCurrentVal);
			MainWindow.localizationManager.Deregister(this.StrTitle);
			MainWindow.localizationManager.Deregister(this.StrProductCounterGood);
			MainWindow.localizationManager.Deregister(this.StrProductCounterBad);
			MainWindow.localizationManager.Deregister(this.StrMachine);
			MainWindow.localizationManager.Deregister(this.StrPerDay);
			MainWindow.localizationManager.Deregister(this.StrProduct);
			MainWindow.localizationManager.Deregister(this.StrOperatingHours);
		}

		// Token: 0x06000E98 RID: 3736 RVA: 0x00057EAC File Offset: 0x000560AC
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

		// Token: 0x06000E99 RID: 3737 RVA: 0x00057F28 File Offset: 0x00056128
		private void keyboard_TextInputKeyboardNumber(object sender, KeyboardInputEventArgs e)
		{
			bool flag = e.Text != null;
			if (flag)
			{
				bool flag2 = this.limitNumber == 1;
				if (flag2)
				{
					bool flag3 = e.Text.Equals("") || e.Text.Equals("0");
					if (flag3)
					{
						this.selectedItem.Limit = "-";
						this.SetNewLimit(this.selectedItem);
					}
					else
					{
						bool flag4 = true;
						bool flag5 = this.selectedItem.Name.Equals(this.StrTemperature.LocalizedText + " " + this.StrMachine.LocalizedText + " [°C]") && !e.Text.Equals("") && !e.Text.Equals("-") && this.limitDefaultTemp > 0.0;
						if (flag5)
						{
							double num = 0.0;
							double.TryParse(e.Text, out num);
							bool flag6 = num > this.limitDefaultTemp;
							if (flag6)
							{
								flag4 = false;
							}
						}
						bool flag7 = flag4;
						if (flag7)
						{
							this.selectedItem.Limit = e.Text;
							this.SetNewLimit(this.selectedItem);
						}
					}
				}
				else
				{
					this.selectedItem.Limit2 = e.Text;
				}
			}
			this.numKeyboard.TextInputKeyboard -= this.keyboard_TextInputKeyboardNumber;
		}

		// Token: 0x06000E9A RID: 3738 RVA: 0x000580A4 File Offset: 0x000562A4
		public void SetNewLimit(MonitoringItem item)
		{
			bool flag = item.Name.Equals(this.StrTemperature.LocalizedText + " " + this.StrMachine.LocalizedText + " [°C]");
			if (flag)
			{
				this.dBDataLogs.LimitsAddEntry("DateTimeTemperature", "Temperature", item.Limit, DateTime.Now.ToString(new CultureInfo("de-DE")));
			}
			else
			{
				bool flag2 = item.Name.Equals(this.StrOperatingHours.LocalizedText + " " + this.StrMachine.LocalizedText + " [h]");
				if (flag2)
				{
					this.dBDataLogs.LimitsAddEntry("DateTimeOperatingHours", "OperatingHours", item.Limit, DateTime.Now.ToString(new CultureInfo("de-DE")));
				}
				else
				{
					bool flag3 = item.Name.Equals(this.StrProductCounterBad.LocalizedText) || item.Name.Equals(this.StrProductCounterBad.LocalizedText + " (Bulk Object " + this.StrProduct.LocalizedText + " )");
					if (flag3)
					{
						this.dBDataLogs.LimitsAddEntry("DateTimeReject", "Reject", item.Limit, DateTime.Now.ToString(new CultureInfo("de-DE")));
					}
					else
					{
						bool flag4 = item.Name.Equals(this.StrProductCounterGood.LocalizedText) || item.Name.Equals(this.StrProductCounterGood.LocalizedText + " (Bulk Object " + this.StrProduct.LocalizedText + " )");
						if (flag4)
						{
							this.dBDataLogs.LimitsAddEntry("DateTimeThroughput", "Throughput", item.Limit, DateTime.Now.ToString(new CultureInfo("de-DE")));
						}
						else
						{
							bool flag5 = item.Name.Equals(this.StrProductCounterBad.LocalizedText + " " + this.StrPerDay.LocalizedText) || item.Name.Equals(string.Concat(new string[]
							{
								this.StrProductCounterBad.LocalizedText,
								" ",
								this.StrPerDay.LocalizedText,
								" (Bulk Object ",
								this.StrProduct.LocalizedText,
								" )"
							}));
							if (flag5)
							{
								this.dBDataLogs.LimitsAddEntry("DateTimeRejectPerDay", "RejectPerDay", item.Limit, DateTime.Now.ToString(new CultureInfo("de-DE")));
							}
							else
							{
								bool flag6 = item.Name.Equals(this.StrProductCounterGood.LocalizedText + " " + this.StrPerDay.LocalizedText) || item.Name.Equals(string.Concat(new string[]
								{
									this.StrProductCounterGood.LocalizedText,
									" ",
									this.StrPerDay.LocalizedText,
									" (Bulk Object ",
									this.StrProduct.LocalizedText,
									" )"
								}));
								if (flag6)
								{
									this.dBDataLogs.LimitsAddEntry("DateTimeThroughputPerDay", "ThroughputPerDay", item.Limit, DateTime.Now.ToString(new CultureInfo("de-DE")));
								}
							}
						}
					}
				}
			}
			bool flag7 = this.showMessages;
			if (flag7)
			{
				this.CheckLimit(item);
			}
		}

		// Token: 0x06000E9B RID: 3739 RVA: 0x00058444 File Offset: 0x00056644
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

		// Token: 0x06000E9C RID: 3740 RVA: 0x000585C0 File Offset: 0x000567C0
		public override void ShowDiagramm()
		{
			CtrlDataPlots usrControl = new CtrlDataPlots(this.parameterHandling, this.dBConnection, this.dBDataLogs, DataModesMonitoring.operational);
			Subwindow.Show(usrControl, SubwindowButtons.BackAndHome);
		}

		// Token: 0x06000E9D RID: 3741 RVA: 0x000585F0 File Offset: 0x000567F0
		private void CleanUp(object obj)
		{
			this.timerDataLogsSeconds.Stop();
			this.DeregisterLocalizedText();
			this.Items.Clear();
			this.Items = null;
			this.mainWindow.ThroughputIncreased -= this.IncreaseThroughput;
			this.mainWindow.RejectIncreased -= this.IncreaseReject;
		}

		// Token: 0x0400097F RID: 2431
		private NumKeyboard numKeyboard;

		// Token: 0x04000980 RID: 2432
		private MonitoringItem selectedItem;

		// Token: 0x04000981 RID: 2433
		private int limitNumber;

		// Token: 0x04000982 RID: 2434
		private List<MonitoringItem> items = new List<MonitoringItem>();

		// Token: 0x0400098D RID: 2445
		private DBDataLogs dBDataLogs;

		// Token: 0x0400098E RID: 2446
		private ParameterHandling parameterHandling;

		// Token: 0x0400098F RID: 2447
		private DBConnection dBConnection;

		// Token: 0x04000990 RID: 2448
		private bool enableLimits = false;

		// Token: 0x04000991 RID: 2449
		private double limitDefaultTemp = 0.0;

		// Token: 0x04000992 RID: 2450
		private ErrorSurvey errorSurvey;

		// Token: 0x04000993 RID: 2451
		private DispatcherTimer timerDataLogsSeconds;

		// Token: 0x04000994 RID: 2452
		private double minutes = 0.0;

		// Token: 0x04000995 RID: 2453
		private uint reject = 0U;

		// Token: 0x04000996 RID: 2454
		private uint throughput = 0U;

		// Token: 0x04000997 RID: 2455
		private uint rejectPerDay = 0U;

		// Token: 0x04000998 RID: 2456
		private uint throughputPerDay = 0U;

		// Token: 0x04000999 RID: 2457
		private MainWindowViewModel mainWindow;

		// Token: 0x0400099A RID: 2458
		private bool showMessages = false;

		// Token: 0x0400099B RID: 2459
		private bool isEX2;
	}
}
