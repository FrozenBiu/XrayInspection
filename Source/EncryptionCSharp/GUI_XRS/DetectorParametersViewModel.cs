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

namespace GUI_XRS
{
	// Token: 0x02000086 RID: 134
	internal class DetectorParametersViewModel : MonitoringViewModel, INotifyPropertyChanged
	{
		// Token: 0x14000046 RID: 70
		// (add) Token: 0x06000D25 RID: 3365 RVA: 0x00050C64 File Offset: 0x0004EE64
		// (remove) Token: 0x06000D26 RID: 3366 RVA: 0x00050C9C File Offset: 0x0004EE9C
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x170003F1 RID: 1009
		// (get) Token: 0x06000D27 RID: 3367 RVA: 0x00050CD1 File Offset: 0x0004EED1
		// (set) Token: 0x06000D28 RID: 3368 RVA: 0x00050CD9 File Offset: 0x0004EED9
		public RelayCommand CleanUpCommand { get; set; }

		// Token: 0x170003F2 RID: 1010
		// (get) Token: 0x06000D29 RID: 3369 RVA: 0x00050CE4 File Offset: 0x0004EEE4
		// (set) Token: 0x06000D2A RID: 3370 RVA: 0x00050CFC File Offset: 0x0004EEFC
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

		// Token: 0x170003F3 RID: 1011
		// (get) Token: 0x06000D2B RID: 3371 RVA: 0x00050D12 File Offset: 0x0004EF12
		// (set) Token: 0x06000D2C RID: 3372 RVA: 0x00050D1A File Offset: 0x0004EF1A
		public BoundLocalizedText StrTitle { get; set; }

		// Token: 0x170003F4 RID: 1012
		// (get) Token: 0x06000D2D RID: 3373 RVA: 0x00050D23 File Offset: 0x0004EF23
		// (set) Token: 0x06000D2E RID: 3374 RVA: 0x00050D2B File Offset: 0x0004EF2B
		public BoundLocalizedText StrCurrentVal { get; set; }

		// Token: 0x170003F5 RID: 1013
		// (get) Token: 0x06000D2F RID: 3375 RVA: 0x00050D34 File Offset: 0x0004EF34
		// (set) Token: 0x06000D30 RID: 3376 RVA: 0x00050D3C File Offset: 0x0004EF3C
		public BoundLocalizedText StrLimits { get; set; }

		// Token: 0x170003F6 RID: 1014
		// (get) Token: 0x06000D31 RID: 3377 RVA: 0x00050D45 File Offset: 0x0004EF45
		// (set) Token: 0x06000D32 RID: 3378 RVA: 0x00050D4D File Offset: 0x0004EF4D
		public BoundLocalizedText StrTemperature { get; set; }

		// Token: 0x170003F7 RID: 1015
		// (get) Token: 0x06000D33 RID: 3379 RVA: 0x00050D56 File Offset: 0x0004EF56
		// (set) Token: 0x06000D34 RID: 3380 RVA: 0x00050D5E File Offset: 0x0004EF5E
		public BoundLocalizedText StrOperatingHours { get; set; }

		// Token: 0x170003F8 RID: 1016
		// (get) Token: 0x06000D35 RID: 3381 RVA: 0x00050D67 File Offset: 0x0004EF67
		// (set) Token: 0x06000D36 RID: 3382 RVA: 0x00050D6F File Offset: 0x0004EF6F
		public BoundLocalizedText StrOperatingHoursPerOperatingPoint { get; set; }

		// Token: 0x170003F9 RID: 1017
		// (get) Token: 0x06000D37 RID: 3383 RVA: 0x00050D78 File Offset: 0x0004EF78
		// (set) Token: 0x06000D38 RID: 3384 RVA: 0x00050D80 File Offset: 0x0004EF80
		public BoundLocalizedText StrXRayOn { get; set; }

		// Token: 0x170003FA RID: 1018
		// (get) Token: 0x06000D39 RID: 3385 RVA: 0x00050D89 File Offset: 0x0004EF89
		// (set) Token: 0x06000D3A RID: 3386 RVA: 0x00050D91 File Offset: 0x0004EF91
		public BoundLocalizedText StrSignalMonitoring { get; set; }

		// Token: 0x170003FB RID: 1019
		// (get) Token: 0x06000D3B RID: 3387 RVA: 0x00050D9A File Offset: 0x0004EF9A
		// (set) Token: 0x06000D3C RID: 3388 RVA: 0x00050DA2 File Offset: 0x0004EFA2
		public BoundLocalizedText StrSignalFluctuation { get; set; }

		// Token: 0x170003FC RID: 1020
		// (get) Token: 0x06000D3D RID: 3389 RVA: 0x00050DAB File Offset: 0x0004EFAB
		// (set) Token: 0x06000D3E RID: 3390 RVA: 0x00050DB3 File Offset: 0x0004EFB3
		public BoundLocalizedText StrDrop { get; set; }

		// Token: 0x170003FD RID: 1021
		// (get) Token: 0x06000D3F RID: 3391 RVA: 0x00050DBC File Offset: 0x0004EFBC
		// (set) Token: 0x06000D40 RID: 3392 RVA: 0x00050DD4 File Offset: 0x0004EFD4
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

		// Token: 0x06000D41 RID: 3393 RVA: 0x00050DEC File Offset: 0x0004EFEC
		public DetectorParametersViewModel(DBDataLogs dBDataLogs, DBConnection dBConnection, ParameterHandling parameterHandling, bool LimitsEnabled, ErrorSurvey errorSurvey)
		{
			this.RegisterLocalizedText();
			this.dBDataLogs = dBDataLogs;
			this.parameterHandling = parameterHandling;
			this.dBConnection = dBConnection;
			this.EnableLimits = LimitsEnabled;
			this.errorSurvey = errorSurvey;
			this.Initialize();
			this.CleanUpCommand = new RelayCommand(new Action<object>(this.CleanUp));
			this.timerDataLogsSeconds = new DispatcherTimer(new TimeSpan(0, 0, 2), DispatcherPriority.Normal, new EventHandler(this.UpdateValues), Application.Current.Dispatcher);
			this.timerDataLogsSeconds.Start();
		}

		// Token: 0x06000D42 RID: 3394 RVA: 0x00050EBC File Offset: 0x0004F0BC
		private void UpdateValues(object sender, EventArgs e)
		{
			string a = this.parameterHandling.ReadPar("/hardware/detector/Type/Value");
			bool flag = a == "2" || a == "4";
			if (flag)
			{
				double num = 0.0;
				double.TryParse(this.parameterHandling.ReadPar("/hardware/detector/dt/MCUTemperature/Value"), out num);
				this.items[1].CurrentValue = (num / 10.0).ToString();
			}
			else
			{
				double num2 = 0.0;
				double.TryParse(this.parameterHandling.ReadPar("/hardware/detector/sesotec/Temperature/Value"), out num2);
				this.items[1].CurrentValue = (num2 / 10.0).ToString();
			}
			this.minutes += 0.03333333333333333;
			this.items[0].CurrentValue = Math.Round(this.minutes / 60.0, 2).ToString();
		}

		// Token: 0x06000D43 RID: 3395 RVA: 0x00050FD8 File Offset: 0x0004F1D8
		public void Initialize()
		{
			List<string> list = new List<string>();
			List<string> list2 = new List<string>();
			List<string> list3 = new List<string>();
			List<string> list4 = new List<string>();
			string text = "";
			double num = 0.0;
			string text2 = "";
			string text3 = "";
			this.dBDataLogs.GetDataLogsSettingsEntry("MonitoringMessagesOnOff", ref text);
			bool flag = text.Equals("1");
			if (flag)
			{
				this.showMessages = true;
			}
			text = "";
			this.dBDataLogs.MachineInfosGetSerialNumbers(ref text3, ref text2);
			string text4 = "DetectorOnTimes_" + text2.Replace(":", "");
			this.dBDataLogs.GetLimitsEntry("OperatingHoursDetector", ref text);
			this.dBDataLogs.OperatingCounterGetDoubleEntry("DetectorOperatingHoursWithXRay", ref num);
			this.items.Add(new MonitoringItem(this.StrOperatingHours.LocalizedText + " (" + this.StrXRayOn.LocalizedText + ") [h]", num.ToString(), text, "", this.EnableLimits, "txtOperatingHours", "parItemDetector"));
			this.dBDataLogs.OperatingCounterGetDoubleEntry("DetectorOperatingMinutesWithXRay", ref this.minutes);
			this.dBDataLogs.GetLimitsEntry("TemperatureDetector", ref text);
			this.dBDataLogs.TemperaturesGetEntries("Temperatures", ref list, ref list2, ref list3, ref list4);
			bool flag2 = list4.Count > 0;
			if (flag2)
			{
				this.items.Add(new MonitoringItem(this.StrTemperature.LocalizedText + " [°C]", list4.Last<string>(), text, "", this.EnableLimits, "txtTemperature", "parItemDetector"));
			}
			else
			{
				this.items.Add(new MonitoringItem(this.StrTemperature.LocalizedText + " [°C]", "-", text, "", this.EnableLimits, "txtTemperature", "parItemDetector"));
			}
			list.Clear();
			list2.Clear();
			list3.Clear();
			list4.Clear();
			List<string> list5 = new List<string>();
			List<string> list6 = new List<string>();
			this.dBDataLogs.MachineInfosAndOperatingCounterGetEntries("MachineInfos", ref list5, ref list6);
			int index = list5.IndexOf("/machine/MachineType");
			string text5 = list6[index];
			bool flag3 = !text5.Equals("Raycon XRS") && !text5.Equals("Raycon D");
			if (flag3)
			{
				this.dBDataLogs.GetLimitsEntry("SignalDifference", ref text);
				double num2 = 0.0;
				double num3 = 0.0;
				this.dBDataLogs.GetLastSignalDetectorQuality("DetectorQuality_" + text2.Replace(":", ""), ref num3, ref num2);
				this.items.Add(new MonitoringItem(this.StrSignalMonitoring.LocalizedText + " (" + this.StrDrop.LocalizedText + ") [%]", num2.ToString(), text, "", this.EnableLimits, "txtSignalMonitoring", ""));
				this.dBDataLogs.GetLimitsEntry("SignalFluctuation", ref text);
				num2 = 0.0;
				num3 = 0.0;
				this.dBDataLogs.GetLastSignalDetectorFlucutation("DetectorFluctuation_" + text2.Replace(":", ""), ref num3, ref num2);
				this.items.Add(new MonitoringItem(this.StrSignalFluctuation.LocalizedText + " [%]", num2.ToString(), text, "", this.EnableLimits, "txtSignalFluctuation", ""));
			}
			bool flag4 = list6.Count<string>() > 0;
			if (flag4)
			{
				index = list5.IndexOf("/hardware/detector/Type");
				string a = list6[index];
				bool flag5 = a == "4" || a == "DTNetTDI";
				if (flag5)
				{
					string path = "DTNetTDITemperatureDetector";
					text = "";
					this.dBDataLogs.GetDefaultLimit(path, ref text);
					double.TryParse(text, out this.limitDefaultTemp);
				}
				else
				{
					int num4 = list5.IndexOf("/hardware/detector/sesotec/PixelSize");
					bool flag6 = num4 != -1;
					if (flag6)
					{
						string text6 = list6[num4];
						bool flag7 = text6.Length == 0;
						if (flag7)
						{
							text6 = "0.8";
						}
						string path2 = text6 + "TemperatureDetector";
						text = "";
						this.dBDataLogs.GetDefaultLimit(path2, ref text);
						double.TryParse(text, out this.limitDefaultTemp);
					}
				}
			}
			else
			{
				this.limitDefaultTemp = 63.0;
			}
		}

		// Token: 0x06000D44 RID: 3396 RVA: 0x000514AC File Offset: 0x0004F6AC
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x06000D45 RID: 3397 RVA: 0x000514DC File Offset: 0x0004F6DC
		private void RegisterLocalizedText()
		{
			this.StrTemperature = new BoundLocalizedText("txtTemperature");
			this.StrLimits = new BoundLocalizedText("txtLimits");
			this.StrCurrentVal = new BoundLocalizedText("txtCurrentValue");
			this.StrTitle = new BoundLocalizedText("parItemDetector");
			this.StrOperatingHours = new BoundLocalizedText("txtOperatingHours");
			this.StrOperatingHoursPerOperatingPoint = new BoundLocalizedText("txtOperatingHoursPerOperatingPoint");
			this.StrSignalMonitoring = new BoundLocalizedText("txtSignalMonitoring");
			this.StrXRayOn = new BoundLocalizedText("lbXRayTurnedOn");
			this.StrDrop = new BoundLocalizedText("txtDropSignal");
			this.StrSignalMonitoring = new BoundLocalizedText("txtSignalMonitoring");
			this.StrSignalFluctuation = new BoundLocalizedText("txtSignalFluctuation");
		}

		// Token: 0x06000D46 RID: 3398 RVA: 0x000515A8 File Offset: 0x0004F7A8
		private void DeregisterLocalizedText()
		{
			MainWindow.localizationManager.Deregister(this.StrTemperature);
			MainWindow.localizationManager.Deregister(this.StrLimits);
			MainWindow.localizationManager.Deregister(this.StrCurrentVal);
			MainWindow.localizationManager.Deregister(this.StrTitle);
			MainWindow.localizationManager.Deregister(this.StrOperatingHoursPerOperatingPoint);
			MainWindow.localizationManager.Deregister(this.StrSignalMonitoring);
			MainWindow.localizationManager.Deregister(this.StrXRayOn);
			MainWindow.localizationManager.Deregister(this.StrDrop);
			MainWindow.localizationManager.Deregister(this.StrSignalFluctuation);
			MainWindow.localizationManager.Deregister(this.StrDrop);
			MainWindow.localizationManager.Deregister(this.StrOperatingHours);
		}

		// Token: 0x06000D47 RID: 3399 RVA: 0x00051674 File Offset: 0x0004F874
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

		// Token: 0x06000D48 RID: 3400 RVA: 0x000516F0 File Offset: 0x0004F8F0
		private void keyboard_TextInputKeyboardNumber(object sender, KeyboardInputEventArgs e)
		{
			bool flag = e.Text != null;
			if (flag)
			{
				bool flag2 = this.limitNumber == 1;
				if (flag2)
				{
					bool flag3 = e.Text == "" || e.Text == "0";
					if (flag3)
					{
						this.selectedItem.Limit = "-";
						this.SetNewLimit(this.selectedItem);
					}
					else
					{
						bool flag4 = true;
						bool flag5 = this.selectedItem.Name.Equals(this.StrTemperature.LocalizedText + " [°C]") && !e.Text.Equals("") && !e.Text.Equals("-") && this.limitDefaultTemp > 0.0;
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

		// Token: 0x06000D49 RID: 3401 RVA: 0x0005185C File Offset: 0x0004FA5C
		public void SetNewLimit(MonitoringItem item)
		{
			bool flag = item.Name.Equals(this.StrTemperature.LocalizedText + " [°C]");
			if (flag)
			{
				this.dBDataLogs.LimitsAddEntry("DateTimeTemperatureDetector", "TemperatureDetector", item.Limit, DateTime.Now.ToString(new CultureInfo("de-DE")));
			}
			else
			{
				bool flag2 = item.Name.Equals(this.StrOperatingHours.LocalizedText + " (" + this.StrXRayOn.LocalizedText + ") [h]");
				if (flag2)
				{
					this.dBDataLogs.LimitsAddEntry("DateTimeOperatingHoursDetector", "OperatingHoursDetector", item.Limit, DateTime.Now.ToString(new CultureInfo("de-DE")));
				}
				else
				{
					bool flag3 = item.Name.Equals(this.StrSignalMonitoring.LocalizedText + " (" + this.StrDrop.LocalizedText + ") [%]");
					if (flag3)
					{
						this.dBDataLogs.LimitsAddEntry("DateTimeSignalDifference", "SignalDifference", item.Limit, DateTime.Now.ToString(new CultureInfo("de-DE")));
					}
					else
					{
						bool flag4 = item.Name.Equals(this.StrSignalFluctuation.LocalizedText + " [%]");
						if (flag4)
						{
							this.dBDataLogs.LimitsAddEntry("DateTimeSignalFluctuation", "SignalFluctuation", item.Limit, DateTime.Now.ToString(new CultureInfo("de-DE")));
						}
					}
				}
			}
			bool flag5 = this.showMessages;
			if (flag5)
			{
				this.CheckLimit(item);
			}
		}

		// Token: 0x06000D4A RID: 3402 RVA: 0x00051A10 File Offset: 0x0004FC10
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

		// Token: 0x06000D4B RID: 3403 RVA: 0x00051B8C File Offset: 0x0004FD8C
		public override void ShowDiagramm()
		{
			CtrlDataPlots usrControl = new CtrlDataPlots(this.parameterHandling, this.dBConnection, this.dBDataLogs, DataModesMonitoring.detector);
			Subwindow.Show(usrControl, SubwindowButtons.BackAndHome);
		}

		// Token: 0x06000D4C RID: 3404 RVA: 0x00051BBB File Offset: 0x0004FDBB
		private void CleanUp(object obj)
		{
			this.timerDataLogsSeconds.Stop();
			this.DeregisterLocalizedText();
			this.Items.Clear();
			this.Items = null;
		}

		// Token: 0x040008A1 RID: 2209
		private NumKeyboard numKeyboard;

		// Token: 0x040008A2 RID: 2210
		private MonitoringItem selectedItem;

		// Token: 0x040008A3 RID: 2211
		private int limitNumber;

		// Token: 0x040008A4 RID: 2212
		private List<MonitoringItem> items = new List<MonitoringItem>();

		// Token: 0x040008AF RID: 2223
		private DBDataLogs dBDataLogs;

		// Token: 0x040008B0 RID: 2224
		private ParameterHandling parameterHandling;

		// Token: 0x040008B1 RID: 2225
		private DBConnection dBConnection;

		// Token: 0x040008B2 RID: 2226
		private bool enableLimits = false;

		// Token: 0x040008B3 RID: 2227
		private double limitDefaultTemp = 0.0;

		// Token: 0x040008B4 RID: 2228
		private ErrorSurvey errorSurvey;

		// Token: 0x040008B5 RID: 2229
		private DispatcherTimer timerDataLogsSeconds;

		// Token: 0x040008B6 RID: 2230
		private double minutes = 0.0;

		// Token: 0x040008B7 RID: 2231
		private bool showMessages = false;
	}
}
