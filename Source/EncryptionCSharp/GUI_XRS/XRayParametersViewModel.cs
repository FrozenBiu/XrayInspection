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
	// Token: 0x0200008C RID: 140
	internal class XRayParametersViewModel : MonitoringViewModel, INotifyPropertyChanged
	{
		// Token: 0x1400004B RID: 75
		// (add) Token: 0x06000E4B RID: 3659 RVA: 0x00056020 File Offset: 0x00054220
		// (remove) Token: 0x06000E4C RID: 3660 RVA: 0x00056058 File Offset: 0x00054258
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x17000461 RID: 1121
		// (get) Token: 0x06000E4D RID: 3661 RVA: 0x0005608D File Offset: 0x0005428D
		// (set) Token: 0x06000E4E RID: 3662 RVA: 0x00056095 File Offset: 0x00054295
		public RelayCommand CleanUpCommand { get; set; }

		// Token: 0x17000462 RID: 1122
		// (get) Token: 0x06000E4F RID: 3663 RVA: 0x000560A0 File Offset: 0x000542A0
		// (set) Token: 0x06000E50 RID: 3664 RVA: 0x000560B8 File Offset: 0x000542B8
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

		// Token: 0x17000463 RID: 1123
		// (get) Token: 0x06000E51 RID: 3665 RVA: 0x000560CE File Offset: 0x000542CE
		// (set) Token: 0x06000E52 RID: 3666 RVA: 0x000560D6 File Offset: 0x000542D6
		public BoundLocalizedText StrTitle { get; set; }

		// Token: 0x17000464 RID: 1124
		// (get) Token: 0x06000E53 RID: 3667 RVA: 0x000560DF File Offset: 0x000542DF
		// (set) Token: 0x06000E54 RID: 3668 RVA: 0x000560E7 File Offset: 0x000542E7
		public BoundLocalizedText StrCurrentVal { get; set; }

		// Token: 0x17000465 RID: 1125
		// (get) Token: 0x06000E55 RID: 3669 RVA: 0x000560F0 File Offset: 0x000542F0
		// (set) Token: 0x06000E56 RID: 3670 RVA: 0x000560F8 File Offset: 0x000542F8
		public BoundLocalizedText StrLimits { get; set; }

		// Token: 0x17000466 RID: 1126
		// (get) Token: 0x06000E57 RID: 3671 RVA: 0x00056101 File Offset: 0x00054301
		// (set) Token: 0x06000E58 RID: 3672 RVA: 0x00056109 File Offset: 0x00054309
		public BoundLocalizedText StrTemperature { get; set; }

		// Token: 0x17000467 RID: 1127
		// (get) Token: 0x06000E59 RID: 3673 RVA: 0x00056112 File Offset: 0x00054312
		// (set) Token: 0x06000E5A RID: 3674 RVA: 0x0005611A File Offset: 0x0005431A
		public BoundLocalizedText StrOperatingHours { get; set; }

		// Token: 0x17000468 RID: 1128
		// (get) Token: 0x06000E5B RID: 3675 RVA: 0x00056123 File Offset: 0x00054323
		// (set) Token: 0x06000E5C RID: 3676 RVA: 0x0005612B File Offset: 0x0005432B
		public BoundLocalizedText StrArcs { get; set; }

		// Token: 0x17000469 RID: 1129
		// (get) Token: 0x06000E5D RID: 3677 RVA: 0x00056134 File Offset: 0x00054334
		// (set) Token: 0x06000E5E RID: 3678 RVA: 0x0005613C File Offset: 0x0005433C
		public BoundLocalizedText StrFilamentCurrent { get; set; }

		// Token: 0x1700046A RID: 1130
		// (get) Token: 0x06000E5F RID: 3679 RVA: 0x00056145 File Offset: 0x00054345
		// (set) Token: 0x06000E60 RID: 3680 RVA: 0x0005614D File Offset: 0x0005434D
		public BoundLocalizedText StrOperatingHoursPerOperatingPoint { get; set; }

		// Token: 0x1700046B RID: 1131
		// (get) Token: 0x06000E61 RID: 3681 RVA: 0x00056156 File Offset: 0x00054356
		// (set) Token: 0x06000E62 RID: 3682 RVA: 0x0005615E File Offset: 0x0005435E
		public BoundLocalizedText StrSwitchOnCycles { get; set; }

		// Token: 0x1700046C RID: 1132
		// (get) Token: 0x06000E63 RID: 3683 RVA: 0x00056167 File Offset: 0x00054367
		// (set) Token: 0x06000E64 RID: 3684 RVA: 0x0005616F File Offset: 0x0005436F
		public BoundLocalizedText StrToday { get; set; }

		// Token: 0x1700046D RID: 1133
		// (get) Token: 0x06000E65 RID: 3685 RVA: 0x00056178 File Offset: 0x00054378
		// (set) Token: 0x06000E66 RID: 3686 RVA: 0x00056190 File Offset: 0x00054390
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

		// Token: 0x06000E67 RID: 3687 RVA: 0x000561A8 File Offset: 0x000543A8
		public XRayParametersViewModel(DBDataLogs dBDataLogs, ParameterHandling parameterHandling, DBConnection dBConnection, bool EnableLimits, ErrorSurvey errorSurvey)
		{
			this.RegisterLocalizedText();
			this.dBDataLogs = dBDataLogs;
			this.parameterHandling = parameterHandling;
			this.dBConnection = dBConnection;
			this.EnableLimits = EnableLimits;
			this.errorSurvey = errorSurvey;
			this.Initialize();
			this.CleanUpCommand = new RelayCommand(new Action<object>(this.CleanUp));
			this.timerDataLogsSeconds = new DispatcherTimer(new TimeSpan(0, 0, 2), DispatcherPriority.Normal, new EventHandler(this.UpdateValues), Application.Current.Dispatcher);
			this.timerDataLogsSeconds.Start();
		}

		// Token: 0x06000E68 RID: 3688 RVA: 0x00056268 File Offset: 0x00054468
		private void Initialize()
		{
			double num = 0.0;
			List<string> list = new List<string>();
			List<string> list2 = new List<string>();
			List<string> list3 = new List<string>();
			List<string> list4 = new List<string>();
			List<string> list5 = new List<string>();
			List<string> source = new List<string>();
			List<string> list6 = new List<string>();
			List<string> list7 = new List<string>();
			string text = "";
			string text2 = "";
			string text3 = "";
			this.dBDataLogs.GetDataLogsSettingsEntry("MonitoringMessagesOnOff", ref text3);
			bool flag = text3.Equals("1");
			if (flag)
			{
				this.showMessages = true;
			}
			text3 = "";
			this.dBDataLogs.MachineInfosGetSerialNumbers(ref text2, ref text);
			string text4 = "FilamentCurrent";
			string text5 = "SwitchOnCycles";
			string text6 = "Arcs";
			bool flag2 = text2.Length > 0;
			if (flag2)
			{
				text4 = text4 + "_" + text2.Replace("-", "");
				text5 = text5 + "_" + text2.Replace("-", "");
				text6 = text6 + "_" + text2.Replace("-", "");
			}
			this.dBDataLogs.GetLimitsEntry("OperatingHoursSource", ref text3);
			this.dBDataLogs.OperatingCounterGetDoubleEntry("SourceOperatingHoursSesotec", ref num);
			this.items.Add(new MonitoringItem(this.StrOperatingHours.LocalizedText + " [h]", num.ToString(), text3, "", this.EnableLimits, "txtOperatingHours", "parItemXRaySource"));
			this.timeXRay = num;
			this.dBDataLogs.GetLimitsEntry("TemperatureSource", ref text3);
			this.dBDataLogs.TemperaturesGetEntries("Temperatures", ref list, ref list2, ref list3, ref list4);
			bool flag3 = list2.Count > 0;
			if (flag3)
			{
				this.items.Add(new MonitoringItem(this.StrTemperature.LocalizedText + " [°C]", list3.Last<string>(), text3, "", this.EnableLimits, "txtTemperature", "parItemXRaySource"));
			}
			else
			{
				this.items.Add(new MonitoringItem(this.StrTemperature.LocalizedText + " [°C]", "-", text3, "", this.EnableLimits));
			}
			list = new List<string>();
			list2 = new List<string>();
			list3 = new List<string>();
			list4 = new List<string>();
			this.dBDataLogs.GetLimitsEntry("FilamentCurrent", ref text3);
			this.dBDataLogs.FilamentCurrentGetEntries(text4, ref list, ref list2, ref list3, ref list4, ref list5, ref source, ref list6);
			bool flag4 = list3.Count > 0;
			if (flag4)
			{
				this.items.Add(new MonitoringItem(this.StrFilamentCurrent.LocalizedText + " [A]", source.Last<string>(), text3, "", this.EnableLimits, "txtFilamentCurrent", ""));
			}
			else
			{
				this.items.Add(new MonitoringItem(this.StrFilamentCurrent.LocalizedText + " [A]", "-", text3, "", this.EnableLimits));
			}
			list = new List<string>();
			list2 = new List<string>();
			list3 = new List<string>();
			list4 = new List<string>();
			MachineMode machineMode = MachineMode.Standard;
			this.dBConnection.GetMachineMode(ref machineMode);
			bool flag5 = machineMode != MachineMode.Bulk && machineMode != MachineMode.BulkObject;
			if (flag5)
			{
				this.dBDataLogs.GetLimitsEntry("Arcs", ref text3);
				this.dBDataLogs.ArcsGetEntriesType0(text6, ref list, ref list2, ref list3);
				bool flag6 = list3.Count > 0;
				if (flag6)
				{
					this.items.Add(new MonitoringItem(this.StrArcs.LocalizedText, list3.Count<string>().ToString(), text3, "", this.EnableLimits, "txtArcs", ""));
				}
				else
				{
					this.items.Add(new MonitoringItem(this.StrArcs.LocalizedText, "0", text3, "", this.EnableLimits, "txtArcs", ""));
				}
				this.dBDataLogs.GetLimitsEntry("ArcsDaily", ref text3);
				int num2 = 0;
				this.dBDataLogs.ArcsGetNumberToday(text6, ref num2);
				this.items.Add(new MonitoringItem(this.StrArcs.LocalizedText + " (" + this.StrToday.LocalizedText + ")", num2.ToString(), text3, "", this.EnableLimits, "txtArcs", "txtDaily"));
			}
			list = new List<string>();
			list2 = new List<string>();
			list3 = new List<string>();
			list4 = new List<string>();
			this.dBDataLogs.GetLimitsEntry("SwitchOnCycles", ref text3);
			this.dBDataLogs.XRaySwitchOnGetEntries(text5, ref list);
			bool flag7 = list.Count > 0;
			if (flag7)
			{
				this.items.Add(new MonitoringItem(this.StrSwitchOnCycles.LocalizedText, list.Count<string>().ToString(), text3, "", this.EnableLimits, "txtSwitchOnCycles", ""));
			}
			else
			{
				this.items.Add(new MonitoringItem(this.StrSwitchOnCycles.LocalizedText, "0", text3, "", this.EnableLimits, "txtSwitchOnCycles", ""));
			}
			List<string> list8 = new List<string>();
			List<string> list9 = new List<string>();
			this.dBDataLogs.MachineInfosAndOperatingCounterGetEntries("MachineInfos", ref list8, ref list9);
			bool flag8 = list9.Count<string>() > 0;
			if (flag8)
			{
				int index = list8.IndexOf("/hardware/xraysource/ModelNumber");
				string value = list9[index];
				bool flag9 = XRayParametersViewModel.modelNr50kV60W.Contains(value);
				string str;
				if (flag9)
				{
					str = "50KV60W";
				}
				else
				{
					bool flag10 = XRayParametersViewModel.modelNr50kV100W.Contains(value);
					if (flag10)
					{
						str = "50KV100W";
					}
					else
					{
						bool flag11 = XRayParametersViewModel.modelNr60kV200W.Contains(value);
						if (flag11)
						{
							str = "60KV200W";
						}
						else
						{
							str = "100KV200W";
						}
					}
				}
				string path = str + "TemperatureSource";
				string text7 = "";
				this.dBDataLogs.GetDefaultLimit(path, ref text7);
				double.TryParse(text7, out this.limitDefaultTemp);
				text7 = "";
				path = str + "FilamentCurrent";
				this.dBDataLogs.GetDefaultLimit(path, ref text7);
				double.TryParse(text7.Replace(',', '.'), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out this.limitDefaultFilamentCur);
			}
			else
			{
				this.limitDefaultFilamentCur = 2.2;
				this.limitDefaultTemp = 57.0;
			}
		}

		// Token: 0x06000E69 RID: 3689 RVA: 0x00056918 File Offset: 0x00054B18
		private void UpdateValues(object sender, EventArgs e)
		{
			this.items[1].CurrentValue = this.parameterHandling.ReadPar("/hardware/xraysource/TankTemperature/Value");
			this.items[2].CurrentValue = this.parameterHandling.ReadPar("/hardware/xraysource/FilamentCurrent/Value");
		}

		// Token: 0x06000E6A RID: 3690 RVA: 0x0005696C File Offset: 0x00054B6C
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x06000E6B RID: 3691 RVA: 0x0005699C File Offset: 0x00054B9C
		private void RegisterLocalizedText()
		{
			this.StrTemperature = new BoundLocalizedText("txtTemperature");
			this.StrLimits = new BoundLocalizedText("txtLimits");
			this.StrCurrentVal = new BoundLocalizedText("txtCurrentValue");
			this.StrTitle = new BoundLocalizedText("parItemXRaySource");
			this.StrOperatingHours = new BoundLocalizedText("txtOperatingHours");
			this.StrArcs = new BoundLocalizedText("txtArcs");
			this.StrFilamentCurrent = new BoundLocalizedText("txtFilamentCurrent");
			this.StrOperatingHoursPerOperatingPoint = new BoundLocalizedText("txtOperatingHoursPerOperatingPoint");
			this.StrSwitchOnCycles = new BoundLocalizedText("txtSwitchOnCycles");
			this.StrToday = new BoundLocalizedText("txtToday");
		}

		// Token: 0x06000E6C RID: 3692 RVA: 0x00056A54 File Offset: 0x00054C54
		private void DeregisterLocalizedText()
		{
			MainWindow.localizationManager.Deregister(this.StrTemperature);
			MainWindow.localizationManager.Deregister(this.StrLimits);
			MainWindow.localizationManager.Deregister(this.StrCurrentVal);
			MainWindow.localizationManager.Deregister(this.StrTitle);
			MainWindow.localizationManager.Deregister(this.StrArcs);
			MainWindow.localizationManager.Deregister(this.StrFilamentCurrent);
			MainWindow.localizationManager.Deregister(this.StrOperatingHoursPerOperatingPoint);
			MainWindow.localizationManager.Deregister(this.StrSwitchOnCycles);
			MainWindow.localizationManager.Deregister(this.StrToday);
			MainWindow.localizationManager.Deregister(this.StrOperatingHours);
		}

		// Token: 0x06000E6D RID: 3693 RVA: 0x00056B0C File Offset: 0x00054D0C
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
			bool flag2 = mItem.Name.Equals(this.StrFilamentCurrent.LocalizedText + " [A]");
			if (flag2)
			{
				this.numKeyboard = new NumKeyboard(mItem.Name, oldValue, NumKeyboardType.TypeDouble);
			}
			else
			{
				this.numKeyboard = new NumKeyboard(mItem.Name, oldValue, NumKeyboardType.TypeUInt);
			}
			this.numKeyboard.TextInputKeyboard += this.keyboard_TextInputKeyboardNumber;
			this.numKeyboard.Show();
		}

		// Token: 0x06000E6E RID: 3694 RVA: 0x00056BC4 File Offset: 0x00054DC4
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
						double num = 0.0;
						bool flag4 = true;
						bool flag5 = this.selectedItem.Name.Equals(this.StrTemperature.LocalizedText + " [°C]") && !e.Text.Equals("") && !e.Text.Equals("-") && this.limitDefaultTemp > 0.0;
						if (flag5)
						{
							double.TryParse(e.Text, out num);
							bool flag6 = num > this.limitDefaultTemp;
							if (flag6)
							{
								flag4 = false;
							}
						}
						else
						{
							bool flag7 = this.selectedItem.Name.Equals(this.StrFilamentCurrent.LocalizedText + " [A]") && !e.Text.Equals("") && !e.Text.Equals("-") && this.limitDefaultFilamentCur > 0.0;
							if (flag7)
							{
								double.TryParse(e.Text.Replace(',', '.'), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out num);
								bool flag8 = num > this.limitDefaultFilamentCur;
								if (flag8)
								{
									flag4 = false;
								}
							}
						}
						bool flag9 = flag4;
						if (flag9)
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

		// Token: 0x06000E6F RID: 3695 RVA: 0x00056DCC File Offset: 0x00054FCC
		public void SetNewLimit(MonitoringItem item)
		{
			bool flag = item.Name.Equals(this.StrTemperature.LocalizedText + " [°C]");
			if (flag)
			{
				this.dBDataLogs.LimitsAddEntry("DateTimeTemperatureSource", "TemperatureSource", item.Limit, DateTime.Now.ToString(new CultureInfo("de-DE")));
			}
			else
			{
				bool flag2 = item.Name.Equals(this.StrOperatingHours.LocalizedText + " [h]");
				if (flag2)
				{
					this.dBDataLogs.LimitsAddEntry("DateTimeOperatingHoursSource", "OperatingHoursSource", item.Limit, DateTime.Now.ToString(new CultureInfo("de-DE")));
				}
				else
				{
					bool flag3 = item.Name.Equals(this.StrFilamentCurrent.LocalizedText + " [A]");
					if (flag3)
					{
						this.dBDataLogs.LimitsAddEntry("DateTimeFilamentCurrent", "FilamentCurrent", item.Limit, DateTime.Now.ToString(new CultureInfo("de-DE")));
					}
					else
					{
						bool flag4 = item.Name.Equals(this.StrArcs.LocalizedText + " (" + this.StrToday.LocalizedText + ")");
						if (flag4)
						{
							this.dBDataLogs.LimitsAddEntry("DateTimeArcsDaily", "ArcsDaily", item.Limit, DateTime.Now.ToString(new CultureInfo("de-DE")));
						}
						else
						{
							bool flag5 = item.Name.Equals(this.StrArcs.LocalizedText);
							if (flag5)
							{
								this.dBDataLogs.LimitsAddEntry("DateTimeArcs", "Arcs", item.Limit, DateTime.Now.ToString(new CultureInfo("de-DE")));
							}
							else
							{
								bool flag6 = item.Name.Equals(this.StrSwitchOnCycles.LocalizedText);
								if (flag6)
								{
									this.dBDataLogs.LimitsAddEntry("DateTimeSwitchOnCycles", "SwitchOnCycles", item.Limit, DateTime.Now.ToString(new CultureInfo("de-DE")));
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

		// Token: 0x06000E70 RID: 3696 RVA: 0x0005701C File Offset: 0x0005521C
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

		// Token: 0x06000E71 RID: 3697 RVA: 0x00057198 File Offset: 0x00055398
		public override void ShowDiagramm()
		{
			CtrlDataPlots usrControl = new CtrlDataPlots(this.parameterHandling, this.dBConnection, this.dBDataLogs, DataModesMonitoring.xray);
			Subwindow.Show(usrControl, SubwindowButtons.BackAndHome);
		}

		// Token: 0x06000E72 RID: 3698 RVA: 0x000571C7 File Offset: 0x000553C7
		private void CleanUp(object obj)
		{
			this.timerDataLogsSeconds.Stop();
			this.DeregisterLocalizedText();
			this.Items.Clear();
			this.Items = null;
		}

		// Token: 0x04000961 RID: 2401
		private NumKeyboard numKeyboard;

		// Token: 0x04000962 RID: 2402
		private MonitoringItem selectedItem;

		// Token: 0x04000963 RID: 2403
		private int limitNumber;

		// Token: 0x04000964 RID: 2404
		private List<MonitoringItem> items = new List<MonitoringItem>();

		// Token: 0x0400096F RID: 2415
		private DBDataLogs dBDataLogs;

		// Token: 0x04000970 RID: 2416
		private ParameterHandling parameterHandling;

		// Token: 0x04000971 RID: 2417
		private DBConnection dBConnection;

		// Token: 0x04000972 RID: 2418
		private bool enableLimits = false;

		// Token: 0x04000973 RID: 2419
		private ErrorSurvey errorSurvey;

		// Token: 0x04000974 RID: 2420
		private double limitDefaultTemp;

		// Token: 0x04000975 RID: 2421
		private double limitDefaultFilamentCur;

		// Token: 0x04000976 RID: 2422
		private double timeXRay = 0.0;

		// Token: 0x04000977 RID: 2423
		private static string[] modelNr50kV100W = new string[]
		{
			"XRB50N100",
			"X4209",
			"X4771",
			"IXS050SE100P214"
		};

		// Token: 0x04000978 RID: 2424
		private static string[] modelNr50kV60W = new string[]
		{
			"IXS050SE060P359",
			"X5884"
		};

		// Token: 0x04000979 RID: 2425
		private static string[] modelNr60kV200W = new string[]
		{
			"IXS060BP200P630",
			"IXS060BP200P760"
		};

		// Token: 0x0400097A RID: 2426
		private static string[] modelNr100kV200W = new string[]
		{
			"IXS100BP200P367",
			"IXS100BP200P395",
			"XRB100PN210HRX5885",
			"IXS100BP200P380"
		};

		// Token: 0x0400097B RID: 2427
		private DispatcherTimer timerDataLogsSeconds;

		// Token: 0x0400097C RID: 2428
		private bool showMessages = false;
	}
}
