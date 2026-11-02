using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Threading;
using Common;
using DatabaseConnection;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x0200008A RID: 138
	internal class SettingsMonitoringViewModel : INotifyPropertyChanged
	{
		// Token: 0x14000048 RID: 72
		// (add) Token: 0x06000D71 RID: 3441 RVA: 0x000536D4 File Offset: 0x000518D4
		// (remove) Token: 0x06000D72 RID: 3442 RVA: 0x0005370C File Offset: 0x0005190C
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x17000407 RID: 1031
		// (get) Token: 0x06000D73 RID: 3443 RVA: 0x00053741 File Offset: 0x00051941
		// (set) Token: 0x06000D74 RID: 3444 RVA: 0x00053749 File Offset: 0x00051949
		public RelayCommand CleanUpCommand { get; set; }

		// Token: 0x17000408 RID: 1032
		// (get) Token: 0x06000D75 RID: 3445 RVA: 0x00053752 File Offset: 0x00051952
		// (set) Token: 0x06000D76 RID: 3446 RVA: 0x0005375A File Offset: 0x0005195A
		public BoundLocalizedText StrResetLimits { get; set; }

		// Token: 0x17000409 RID: 1033
		// (get) Token: 0x06000D77 RID: 3447 RVA: 0x00053763 File Offset: 0x00051963
		// (set) Token: 0x06000D78 RID: 3448 RVA: 0x0005376B File Offset: 0x0005196B
		public BoundLocalizedText StrError { get; set; }

		// Token: 0x1700040A RID: 1034
		// (get) Token: 0x06000D79 RID: 3449 RVA: 0x00053774 File Offset: 0x00051974
		// (set) Token: 0x06000D7A RID: 3450 RVA: 0x0005377C File Offset: 0x0005197C
		public BoundLocalizedText StrWarning { get; set; }

		// Token: 0x1700040B RID: 1035
		// (get) Token: 0x06000D7B RID: 3451 RVA: 0x00053785 File Offset: 0x00051985
		// (set) Token: 0x06000D7C RID: 3452 RVA: 0x0005378D File Offset: 0x0005198D
		public BoundLocalizedText StrInfo { get; set; }

		// Token: 0x1700040C RID: 1036
		// (get) Token: 0x06000D7D RID: 3453 RVA: 0x00053796 File Offset: 0x00051996
		// (set) Token: 0x06000D7E RID: 3454 RVA: 0x0005379E File Offset: 0x0005199E
		public BoundLocalizedText StrLevelMonitoring { get; set; }

		// Token: 0x1700040D RID: 1037
		// (get) Token: 0x06000D7F RID: 3455 RVA: 0x000537A7 File Offset: 0x000519A7
		// (set) Token: 0x06000D80 RID: 3456 RVA: 0x000537AF File Offset: 0x000519AF
		public BoundLocalizedText StrTimeSpanRepetitionMessage { get; set; }

		// Token: 0x1700040E RID: 1038
		// (get) Token: 0x06000D81 RID: 3457 RVA: 0x000537B8 File Offset: 0x000519B8
		// (set) Token: 0x06000D82 RID: 3458 RVA: 0x000537C0 File Offset: 0x000519C0
		public BoundLocalizedText StrLevelMonitoring90Per { get; set; }

		// Token: 0x1700040F RID: 1039
		// (get) Token: 0x06000D83 RID: 3459 RVA: 0x000537C9 File Offset: 0x000519C9
		// (set) Token: 0x06000D84 RID: 3460 RVA: 0x000537D1 File Offset: 0x000519D1
		public BoundLocalizedText StrMonitoringMessagesOnOff { get; set; }

		// Token: 0x17000410 RID: 1040
		// (get) Token: 0x06000D85 RID: 3461 RVA: 0x000537DA File Offset: 0x000519DA
		// (set) Token: 0x06000D86 RID: 3462 RVA: 0x000537E2 File Offset: 0x000519E2
		public BoundLocalizedText StrBackgroundMonitoring { get; set; }

		// Token: 0x17000411 RID: 1041
		// (get) Token: 0x06000D87 RID: 3463 RVA: 0x000537EB File Offset: 0x000519EB
		// (set) Token: 0x06000D88 RID: 3464 RVA: 0x000537F3 File Offset: 0x000519F3
		public BoundLocalizedText StrResetCounts { get; set; }

		// Token: 0x17000412 RID: 1042
		// (get) Token: 0x06000D89 RID: 3465 RVA: 0x000537FC File Offset: 0x000519FC
		// (set) Token: 0x06000D8A RID: 3466 RVA: 0x00053814 File Offset: 0x00051A14
		public bool IsInfo
		{
			get
			{
				return this.isInfo;
			}
			set
			{
				this.isInfo = value;
				if (value)
				{
					this.IsWarning = false;
					this.IsError = false;
					this.dBDataLogs.SetDataLogsSettings("MessageLevel", "Info");
				}
				this.OnPropertyChanged("IsInfo");
			}
		}

		// Token: 0x17000413 RID: 1043
		// (get) Token: 0x06000D8B RID: 3467 RVA: 0x00053864 File Offset: 0x00051A64
		// (set) Token: 0x06000D8C RID: 3468 RVA: 0x0005387C File Offset: 0x00051A7C
		public bool IsWarning
		{
			get
			{
				return this.isWarning;
			}
			set
			{
				this.isWarning = value;
				if (value)
				{
					this.IsError = false;
					this.IsInfo = false;
					this.dBDataLogs.SetDataLogsSettings("MessageLevel", "Warning");
				}
				this.OnPropertyChanged("IsWarning");
			}
		}

		// Token: 0x17000414 RID: 1044
		// (get) Token: 0x06000D8D RID: 3469 RVA: 0x000538CC File Offset: 0x00051ACC
		// (set) Token: 0x06000D8E RID: 3470 RVA: 0x000538E4 File Offset: 0x00051AE4
		public bool IsError
		{
			get
			{
				return this.isError;
			}
			set
			{
				this.isError = value;
				if (value)
				{
					this.IsWarning = false;
					this.IsInfo = false;
					this.dBDataLogs.SetDataLogsSettings("MessageLevel", "Error");
				}
				this.OnPropertyChanged("IsError");
			}
		}

		// Token: 0x17000415 RID: 1045
		// (get) Token: 0x06000D8F RID: 3471 RVA: 0x00053934 File Offset: 0x00051B34
		// (set) Token: 0x06000D90 RID: 3472 RVA: 0x0005394C File Offset: 0x00051B4C
		public bool MonitoringOnOff
		{
			get
			{
				return this.monitoringOnOff;
			}
			set
			{
				bool flag = value != this.monitoringOnOff;
				if (flag)
				{
					this.monitoringOnOff = value;
					this.dBDataLogs.SetDataLogsSettings("MonitoringMessagesOnOff", this.monitoringOnOff ? "1" : "0");
					bool flag2 = this.initialized;
					if (flag2)
					{
						if (value)
						{
							LogbookManager.AddEntry(SSTEntryCode.MonitoringActivated);
						}
						else
						{
							LogbookManager.AddEntry(SSTEntryCode.MonitoringDeactivated);
							bool flag3 = this.backgroundMonitoring;
							if (flag3)
							{
								this.BackgroundMonitoring = false;
							}
						}
					}
				}
				this.OnPropertyChanged("MonitoringOnOff");
			}
		}

		// Token: 0x17000416 RID: 1046
		// (get) Token: 0x06000D91 RID: 3473 RVA: 0x000539E4 File Offset: 0x00051BE4
		// (set) Token: 0x06000D92 RID: 3474 RVA: 0x000539FC File Offset: 0x00051BFC
		public bool BackgroundMonitoring
		{
			get
			{
				return this.backgroundMonitoring;
			}
			set
			{
				bool flag = value != this.backgroundMonitoring;
				if (flag)
				{
					this.backgroundMonitoring = value;
					this.parHandling.WriteValue(Definition.detectorMonitoringActive, value ? "1" : "0");
					this.initBordersforBgMonitoring();
					this.OnPropertyChanged("BackgroundMonitoring");
					bool flag2 = value && !this.monitoringOnOff;
					if (flag2)
					{
						this.MonitoringOnOff = true;
					}
				}
			}
		}

		// Token: 0x17000417 RID: 1047
		// (get) Token: 0x06000D93 RID: 3475 RVA: 0x00053A74 File Offset: 0x00051C74
		// (set) Token: 0x06000D94 RID: 3476 RVA: 0x00053A8C File Offset: 0x00051C8C
		public bool BackgroundMonitoringVisibility
		{
			get
			{
				return this.backgroundMonitoringVisibility;
			}
			set
			{
				this.backgroundMonitoringVisibility = value;
				this.OnPropertyChanged("BackgroundMonitoringVisibility");
			}
		}

		// Token: 0x17000418 RID: 1048
		// (get) Token: 0x06000D95 RID: 3477 RVA: 0x00053AA4 File Offset: 0x00051CA4
		// (set) Token: 0x06000D96 RID: 3478 RVA: 0x00053ABC File Offset: 0x00051CBC
		public bool IsInfo90Per
		{
			get
			{
				return this.isInfo90Per;
			}
			set
			{
				this.isInfo90Per = value;
				if (value)
				{
					this.IsWarning90Per = false;
					this.dBDataLogs.SetDataLogsSettings("MessageLevel90Perc", "Info");
				}
				this.OnPropertyChanged("IsInfo90Per");
			}
		}

		// Token: 0x17000419 RID: 1049
		// (get) Token: 0x06000D97 RID: 3479 RVA: 0x00053B04 File Offset: 0x00051D04
		// (set) Token: 0x06000D98 RID: 3480 RVA: 0x00053B1C File Offset: 0x00051D1C
		public bool IsWarning90Per
		{
			get
			{
				return this.isWarning90Per;
			}
			set
			{
				this.isWarning90Per = value;
				if (value)
				{
					this.IsInfo90Per = false;
					this.dBDataLogs.SetDataLogsSettings("MessageLevel90Perc", "Warning");
				}
				this.OnPropertyChanged("IsWarning90Per");
			}
		}

		// Token: 0x1700041A RID: 1050
		// (get) Token: 0x06000D99 RID: 3481 RVA: 0x00053B64 File Offset: 0x00051D64
		// (set) Token: 0x06000D9A RID: 3482 RVA: 0x00053B7C File Offset: 0x00051D7C
		public int TimeSpanRepetitionMessage
		{
			get
			{
				return this.timeSpanRepetitionMessage;
			}
			set
			{
				this.timeSpanRepetitionMessage = value;
				this.dBDataLogs.SetDataLogsSettings("TimeSpanRepetitionMessage", this.timeSpanRepetitionMessage.ToString());
				this.OnPropertyChanged("TimeSpanRepetitionMessage");
			}
		}

		// Token: 0x06000D9B RID: 3483 RVA: 0x00053BB0 File Offset: 0x00051DB0
		public SettingsMonitoringViewModel(DBDataLogs dBDataLogs, ParameterHandling parHandling)
		{
			this.dBDataLogs = dBDataLogs;
			this.parHandling = parHandling;
			this.RegisterLocalizedText();
			this.CleanUpCommand = new RelayCommand(new Action<object>(this.CleanUp));
			string text = "";
			dBDataLogs.GetDataLogsSettingsEntry("MessageLevel", ref text);
			bool flag = text.Equals("Info");
			if (flag)
			{
				this.IsInfo = true;
			}
			else
			{
				bool flag2 = text.Equals("Warning");
				if (flag2)
				{
					this.IsWarning = true;
				}
				else
				{
					this.IsError = true;
				}
			}
			dBDataLogs.GetDataLogsSettingsEntry("MessageLevel90Perc", ref text);
			bool flag3 = text.Equals("Info");
			if (flag3)
			{
				this.IsInfo90Per = true;
			}
			else
			{
				bool flag4 = text.Equals("Warning");
				if (flag4)
				{
					this.IsWarning90Per = true;
				}
			}
			dBDataLogs.GetDataLogsSettingsEntry("MonitoringMessagesOnOff", ref text);
			this.MonitoringOnOff = (text == "1");
			bool flag5 = (parHandling.ReadPar(Definition.machineModePath) == "0" || parHandling.ReadPar(Definition.machineModePath) == "3") && parHandling.ReadPar(Definition.filterMainPath + "/acquisition/AcquisitionMode/Value") == "0" && parHandling.ReadPar(Definition.filterMainPath + DualEnergy.enableSubPath) != "1";
			if (flag5)
			{
				this.BackgroundMonitoringVisibility = true;
				this.BackgroundMonitoring = (parHandling.ReadPar(Definition.detectorMonitoringActive) == "1");
			}
			else
			{
				this.BackgroundMonitoringVisibility = false;
			}
			int num = 0;
			dBDataLogs.GetDataLogsSettingsEntry("TimeSpanRepetitionMessage", ref text);
			int.TryParse(text, out num);
			this.TimeSpanRepetitionMessage = num;
			this.initialized = true;
		}

		// Token: 0x06000D9C RID: 3484 RVA: 0x00053D90 File Offset: 0x00051F90
		internal void resetLimits()
		{
			bool flag = this.dBDataLogs.setDefaultLimits();
			if (flag)
			{
				bool flag2 = Application.Current != null;
				if (flag2)
				{
					Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
					{
						Dialog.Show(LocalizationManager.Localize("txtInfo"), LocalizationManager.Localize("txtSuccess"), DialogStyle.Info, DialogButtons.None, true);
					}));
				}
			}
		}

		// Token: 0x06000D9D RID: 3485 RVA: 0x00053DF0 File Offset: 0x00051FF0
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x06000D9E RID: 3486 RVA: 0x00053E20 File Offset: 0x00052020
		private void RegisterLocalizedText()
		{
			this.StrLevelMonitoring = new BoundLocalizedText("txtMessageLevelMonitoring");
			this.StrLevelMonitoring90Per = new BoundLocalizedText("txtMessageLevelMonitoring90Per");
			this.StrResetLimits = new BoundLocalizedText("txtResetLimits");
			this.StrWarning = new BoundLocalizedText("txtWarning");
			this.StrInfo = new BoundLocalizedText("txtInfo");
			this.StrError = new BoundLocalizedText("txtError");
			this.StrMonitoringMessagesOnOff = new BoundLocalizedText("txtMonitoringMessagesOnOff");
			this.StrTimeSpanRepetitionMessage = new BoundLocalizedText("txtTimeSpanRepetitionMessage");
			this.StrBackgroundMonitoring = new BoundLocalizedText("txtBackgroundMonitoring");
			this.StrResetCounts = new BoundLocalizedText("txtResetSelectedCounter");
		}

		// Token: 0x06000D9F RID: 3487 RVA: 0x00053ED8 File Offset: 0x000520D8
		private void DeregisterLocalizedText()
		{
			MainWindow.localizationManager.Deregister(this.StrLevelMonitoring);
			MainWindow.localizationManager.Deregister(this.StrResetLimits);
			MainWindow.localizationManager.Deregister(this.StrWarning);
			MainWindow.localizationManager.Deregister(this.StrInfo);
			MainWindow.localizationManager.Deregister(this.StrError);
			MainWindow.localizationManager.Deregister(this.StrMonitoringMessagesOnOff);
			MainWindow.localizationManager.Deregister(this.StrLevelMonitoring90Per);
			MainWindow.localizationManager.Deregister(this.StrTimeSpanRepetitionMessage);
			MainWindow.localizationManager.Deregister(this.StrBackgroundMonitoring);
			MainWindow.localizationManager.Deregister(this.StrResetCounts);
		}

		// Token: 0x06000DA0 RID: 3488 RVA: 0x00053F90 File Offset: 0x00052190
		public void getNumKeyboardNumber()
		{
			string oldValue = this.TimeSpanRepetitionMessage.ToString();
			this.numKeyboard = new NumKeyboard("", oldValue, NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.keyboard_TextInputKeyboardNumber;
			this.numKeyboard.Show();
		}

		// Token: 0x06000DA1 RID: 3489 RVA: 0x00053FE4 File Offset: 0x000521E4
		private void keyboard_TextInputKeyboardNumber(object sender, KeyboardInputEventArgs e)
		{
			bool flag = e.Text != null;
			if (flag)
			{
				int num = 0;
				try
				{
					num = int.Parse(e.Text);
				}
				catch
				{
				}
				bool flag2 = num > 0 && num <= 168;
				if (flag2)
				{
					this.TimeSpanRepetitionMessage = num;
				}
			}
			this.numKeyboard.TextInputKeyboard -= this.keyboard_TextInputKeyboardNumber;
		}

		// Token: 0x06000DA2 RID: 3490 RVA: 0x00054060 File Offset: 0x00052260
		private void initBordersforBgMonitoring()
		{
			string value = "";
			bool flag = this.parHandling != null;
			if (flag)
			{
				bool limitsEntry = this.dBDataLogs.GetLimitsEntry("BgMonLiveErrorPixel", ref value);
				if (limitsEntry)
				{
					this.parHandling.WriteValue(Definition.detectorMonitoringPixelCountBorder, value, false);
				}
				value = "";
				bool limitsEntry2 = this.dBDataLogs.GetLimitsEntry("BgMonLiveImageLines", ref value);
				if (limitsEntry2)
				{
					this.parHandling.WriteValue(Definition.detectorMonitoringImageLines, value, false);
				}
				value = "";
				bool limitsEntry3 = this.dBDataLogs.GetLimitsEntry("BgMonLiveThreshold", ref value);
				if (limitsEntry3)
				{
					this.parHandling.WriteValue(Definition.detectorMonitoringThreshold, value, false);
				}
			}
		}

		// Token: 0x06000DA3 RID: 3491 RVA: 0x0005410F File Offset: 0x0005230F
		private void CleanUp(object obj)
		{
			this.DeregisterLocalizedText();
		}

		// Token: 0x040008F6 RID: 2294
		private bool isInfo;

		// Token: 0x040008F7 RID: 2295
		private bool isWarning;

		// Token: 0x040008F8 RID: 2296
		private bool isError;

		// Token: 0x040008F9 RID: 2297
		private bool monitoringOnOff;

		// Token: 0x040008FA RID: 2298
		private bool backgroundMonitoring;

		// Token: 0x040008FB RID: 2299
		private bool backgroundMonitoringVisibility;

		// Token: 0x040008FC RID: 2300
		private DBDataLogs dBDataLogs;

		// Token: 0x040008FD RID: 2301
		private ParameterHandling parHandling;

		// Token: 0x040008FE RID: 2302
		private bool isInfo90Per;

		// Token: 0x040008FF RID: 2303
		private bool isWarning90Per;

		// Token: 0x04000900 RID: 2304
		private int timeSpanRepetitionMessage;

		// Token: 0x04000901 RID: 2305
		private NumKeyboard numKeyboard;

		// Token: 0x04000902 RID: 2306
		private bool initialized = false;
	}
}
