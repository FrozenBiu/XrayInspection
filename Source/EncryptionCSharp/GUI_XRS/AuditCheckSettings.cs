using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using DatabaseConnection;

namespace GUI_XRS
{
	// Token: 0x02000190 RID: 400
	public class AuditCheckSettings : INotifyPropertyChanged
	{
		// Token: 0x17000AC1 RID: 2753
		// (get) Token: 0x060025E0 RID: 9696 RVA: 0x00102561 File Offset: 0x00100761
		// (set) Token: 0x060025E1 RID: 9697 RVA: 0x00102569 File Offset: 0x00100769
		public bool Initialized { get; set; }

		// Token: 0x17000AC2 RID: 2754
		// (get) Token: 0x060025E2 RID: 9698 RVA: 0x00102572 File Offset: 0x00100772
		// (set) Token: 0x060025E3 RID: 9699 RVA: 0x0010257A File Offset: 0x0010077A
		public AuditRequired AuditRequired { get; set; }

		// Token: 0x140000CB RID: 203
		// (add) Token: 0x060025E4 RID: 9700 RVA: 0x00102584 File Offset: 0x00100784
		// (remove) Token: 0x060025E5 RID: 9701 RVA: 0x001025BC File Offset: 0x001007BC
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x17000AC3 RID: 2755
		// (get) Token: 0x060025E6 RID: 9702 RVA: 0x001025F4 File Offset: 0x001007F4
		// (set) Token: 0x060025E7 RID: 9703 RVA: 0x0010260C File Offset: 0x0010080C
		public AlarmMode SelectedMode
		{
			get
			{
				return this.selectedMode;
			}
			set
			{
				bool flag = this.selectedMode != value;
				if (flag)
				{
					bool initialized = this.Initialized;
					if (initialized)
					{
						SSTEntryCode code = SSTEntryCode.AuditParameterChanged;
						byte param = 0;
						int num = (int)this.selectedMode;
						string param2 = num.ToString();
						num = (int)value;
						LogbookManager.AddEntry(code, param, param2, num.ToString());
					}
					this.selectedMode = value;
					this.OnPropertyChanged("SelectedMode");
					int num2 = 0;
					this.dbConnection.GetAuditCheckType(ref num2);
					bool flag2 = value == AlarmMode.PerWeek;
					if (flag2)
					{
						this.DayVisible = true;
					}
					else
					{
						this.DayVisible = false;
					}
					bool flag3 = value != AlarmMode.PerHour && value != AlarmMode.PerProductChange && (num2 != 1 || value != AlarmMode.PerInterval);
					if (flag3)
					{
						this.StartTimeVisible = true;
					}
					else
					{
						this.StartTimeVisible = false;
					}
					bool flag4 = value == AlarmMode.PerInterval || value == AlarmMode.PerProductChange;
					if (flag4)
					{
						this.IntervalTimeVisible = true;
					}
					else
					{
						this.IntervalTimeVisible = false;
					}
					bool flag5 = value == AlarmMode.PerInterval && num2 == 1;
					if (flag5)
					{
						this.StartTimeHour = 0;
						this.StartTimeMinute = 0;
					}
					this.dbConnection.UpdateCurrentProductTable(this.modePath, this.strModes[(int)value]);
					this.UpdateAuditInterval();
				}
			}
		}

		// Token: 0x17000AC4 RID: 2756
		// (get) Token: 0x060025E8 RID: 9704 RVA: 0x00102734 File Offset: 0x00100934
		// (set) Token: 0x060025E9 RID: 9705 RVA: 0x0010274C File Offset: 0x0010094C
		public int StartTimeHour
		{
			get
			{
				return this.startTimeHour;
			}
			set
			{
				bool flag = this.startTimeHour != value;
				if (flag)
				{
					bool initialized = this.Initialized;
					if (initialized)
					{
						LogbookManager.AddEntry(SSTEntryCode.AuditParameterChanged, 1, this.startTimeHour.ToString(), value.ToString());
					}
					bool flag2 = value >= 0 && value < 24;
					if (flag2)
					{
						this.startTimeHour = value;
						this.OnPropertyChanged("StartTimeHour");
						this.dbConnection.UpdateCurrentProductTable(this.hourPath, value.ToString());
						this.UpdateAuditInterval();
					}
				}
			}
		}

		// Token: 0x17000AC5 RID: 2757
		// (get) Token: 0x060025EA RID: 9706 RVA: 0x001027DC File Offset: 0x001009DC
		// (set) Token: 0x060025EB RID: 9707 RVA: 0x001027F4 File Offset: 0x001009F4
		public int StartTimeMinute
		{
			get
			{
				return this.startTimeMinute;
			}
			set
			{
				bool flag = this.startTimeMinute != value;
				if (flag)
				{
					bool initialized = this.Initialized;
					if (initialized)
					{
						LogbookManager.AddEntry(SSTEntryCode.AuditParameterChanged, 2, this.startTimeMinute.ToString(), value.ToString());
					}
					bool flag2 = value >= 0 && value < 60;
					if (flag2)
					{
						this.startTimeMinute = value;
						this.OnPropertyChanged("StartTimeMinute");
						this.dbConnection.UpdateCurrentProductTable(this.minutePath, value.ToString());
						this.UpdateAuditInterval();
					}
				}
			}
		}

		// Token: 0x17000AC6 RID: 2758
		// (get) Token: 0x060025EC RID: 9708 RVA: 0x00102884 File Offset: 0x00100A84
		// (set) Token: 0x060025ED RID: 9709 RVA: 0x0010289C File Offset: 0x00100A9C
		public DayOfWeek AlarmDay
		{
			get
			{
				return this.alarmDay;
			}
			set
			{
				bool flag = this.alarmDay != value;
				if (flag)
				{
					bool initialized = this.Initialized;
					if (initialized)
					{
						SSTEntryCode code = SSTEntryCode.AuditParameterChanged;
						byte param = 3;
						int num = (int)this.alarmDay;
						string param2 = num.ToString();
						num = (int)value;
						LogbookManager.AddEntry(code, param, param2, num.ToString());
					}
					this.alarmDay = value;
					this.OnPropertyChanged("AlarmDay");
					this.dbConnection.UpdateCurrentProductTable(this.dayPath, this.strDays[(int)value]);
					this.UpdateAuditInterval();
				}
			}
		}

		// Token: 0x17000AC7 RID: 2759
		// (get) Token: 0x060025EE RID: 9710 RVA: 0x00102920 File Offset: 0x00100B20
		// (set) Token: 0x060025EF RID: 9711 RVA: 0x00102938 File Offset: 0x00100B38
		public bool DayVisible
		{
			get
			{
				return this.dayVisible;
			}
			set
			{
				this.dayVisible = value;
				this.OnPropertyChanged("DayVisible");
			}
		}

		// Token: 0x17000AC8 RID: 2760
		// (get) Token: 0x060025F0 RID: 9712 RVA: 0x00102950 File Offset: 0x00100B50
		// (set) Token: 0x060025F1 RID: 9713 RVA: 0x00102968 File Offset: 0x00100B68
		public bool StartTimeVisible
		{
			get
			{
				return this.startTimeVisible;
			}
			set
			{
				this.startTimeVisible = value;
				this.OnPropertyChanged("StartTimeVisible");
			}
		}

		// Token: 0x17000AC9 RID: 2761
		// (get) Token: 0x060025F2 RID: 9714 RVA: 0x00102980 File Offset: 0x00100B80
		// (set) Token: 0x060025F3 RID: 9715 RVA: 0x00102998 File Offset: 0x00100B98
		public bool IntervalTimeVisible
		{
			get
			{
				return this.intervalTimeVisible;
			}
			set
			{
				this.intervalTimeVisible = value;
				this.OnPropertyChanged("IntervalTimeVisible");
			}
		}

		// Token: 0x17000ACA RID: 2762
		// (get) Token: 0x060025F4 RID: 9716 RVA: 0x001029B0 File Offset: 0x00100BB0
		// (set) Token: 0x060025F5 RID: 9717 RVA: 0x001029C8 File Offset: 0x00100BC8
		public int IntervalHour
		{
			get
			{
				return this.intervalHour;
			}
			set
			{
				int num = 0;
				this.dbConnection.GetAuditCheckType(ref num);
				bool flag = num == 1;
				if (flag)
				{
					bool flag2 = value == 0;
					if (flag2)
					{
						value = 1;
					}
				}
				bool flag3 = this.intervalHour != value;
				if (flag3)
				{
					bool flag4 = value >= 0 && value < 24;
					if (flag4)
					{
						bool initialized = this.Initialized;
						if (initialized)
						{
							LogbookManager.AddEntry(SSTEntryCode.AuditParameterChanged, 4, this.intervalHour.ToString(), value.ToString());
						}
						this.intervalHour = value;
						this.OnPropertyChanged("IntervalHour");
						this.dbConnection.UpdateCurrentProductTable(this.intervalHourPath, value.ToString());
						this.UpdateAuditInterval();
					}
				}
			}
		}

		// Token: 0x17000ACB RID: 2763
		// (get) Token: 0x060025F6 RID: 9718 RVA: 0x00102A80 File Offset: 0x00100C80
		// (set) Token: 0x060025F7 RID: 9719 RVA: 0x00102A98 File Offset: 0x00100C98
		public int IntervalMinute
		{
			get
			{
				return this.intervalMinute;
			}
			set
			{
				bool flag = this.intervalMinute != value;
				if (flag)
				{
					bool flag2 = value >= 0 && value < 60;
					if (flag2)
					{
						bool initialized = this.Initialized;
						if (initialized)
						{
							LogbookManager.AddEntry(SSTEntryCode.AuditParameterChanged, 5, this.intervalMinute.ToString(), value.ToString());
						}
						this.intervalMinute = value;
						this.OnPropertyChanged("IntervalMinute");
						this.dbConnection.UpdateCurrentProductTable(this.intervalMinutePath, value.ToString());
						this.UpdateAuditInterval();
					}
				}
			}
		}

		// Token: 0x17000ACC RID: 2764
		// (get) Token: 0x060025F8 RID: 9720 RVA: 0x00102B28 File Offset: 0x00100D28
		// (set) Token: 0x060025F9 RID: 9721 RVA: 0x00102B40 File Offset: 0x00100D40
		public bool ContaminationEnabled
		{
			get
			{
				return this.contaminationEnabled;
			}
			set
			{
				bool flag = this.contaminationEnabled != value;
				if (flag)
				{
					bool initialized = this.Initialized;
					if (initialized)
					{
						if (value)
						{
							LogbookManager.AddEntry(SSTEntryCode.AuditActivated, 0);
						}
						else
						{
							LogbookManager.AddEntry(SSTEntryCode.AuditDeactivated, 0);
						}
					}
					this.contaminationEnabled = value;
					this.AuditRequired.ContaminationEnabled = value;
					this.OnPropertyChanged("ContaminationEnabled");
					if (value)
					{
						this.dbConnection.UpdateCurrentProductTable(this.auditContaminationEnabledPath, "1");
						bool flag2 = this.AuditRequired != null;
						if (flag2)
						{
							this.UpdateAuditInterval();
						}
					}
					else
					{
						this.dbConnection.UpdateCurrentProductTable(this.auditContaminationEnabledPath, "0");
						bool flag3 = this.AuditRequired != null && !this.WeightEnabled && !this.CountEnabled;
						if (flag3)
						{
							this.AuditRequired.Disable();
						}
					}
				}
			}
		}

		// Token: 0x17000ACD RID: 2765
		// (get) Token: 0x060025FA RID: 9722 RVA: 0x00102C2C File Offset: 0x00100E2C
		// (set) Token: 0x060025FB RID: 9723 RVA: 0x00102C44 File Offset: 0x00100E44
		public bool WeightEnabled
		{
			get
			{
				return this.weightEnabled;
			}
			set
			{
				bool flag = this.weightEnabled != value;
				if (flag)
				{
					bool initialized = this.Initialized;
					if (initialized)
					{
						if (value)
						{
							LogbookManager.AddEntry(SSTEntryCode.AuditActivated, 1);
						}
						else
						{
							LogbookManager.AddEntry(SSTEntryCode.AuditDeactivated, 1);
						}
					}
					this.weightEnabled = value;
					this.AuditRequired.WeightEnabled = value;
					this.OnPropertyChanged("WeightEnabled");
					if (value)
					{
						this.dbConnection.UpdateCurrentProductTable(this.auditWeightEnabledPath, "1");
						bool flag2 = this.AuditRequired != null;
						if (flag2)
						{
							this.UpdateAuditInterval();
						}
					}
					else
					{
						this.dbConnection.UpdateCurrentProductTable(this.auditWeightEnabledPath, "0");
						bool flag3 = this.AuditRequired != null && !this.ContaminationEnabled && !this.CountEnabled;
						if (flag3)
						{
							this.AuditRequired.Disable();
						}
					}
				}
			}
		}

		// Token: 0x17000ACE RID: 2766
		// (get) Token: 0x060025FC RID: 9724 RVA: 0x00102D30 File Offset: 0x00100F30
		// (set) Token: 0x060025FD RID: 9725 RVA: 0x00102D48 File Offset: 0x00100F48
		public bool CountEnabled
		{
			get
			{
				return this.countEnabled;
			}
			set
			{
				bool flag = this.countEnabled != value;
				if (flag)
				{
					bool initialized = this.Initialized;
					if (initialized)
					{
						if (value)
						{
							LogbookManager.AddEntry(SSTEntryCode.AuditActivated, 2);
						}
						else
						{
							LogbookManager.AddEntry(SSTEntryCode.AuditDeactivated, 2);
						}
					}
					this.countEnabled = value;
					this.AuditRequired.CountEnabled = value;
					this.OnPropertyChanged("CountEnabled");
					if (value)
					{
						this.dbConnection.UpdateCurrentProductTable(this.auditCountEnabledPath, "1");
						bool flag2 = this.AuditRequired != null;
						if (flag2)
						{
							this.UpdateAuditInterval();
						}
					}
					else
					{
						this.dbConnection.UpdateCurrentProductTable(this.auditCountEnabledPath, "0");
						bool flag3 = this.AuditRequired != null && !this.ContaminationEnabled && !this.WeightEnabled;
						if (flag3)
						{
							this.AuditRequired.Disable();
						}
					}
				}
			}
		}

		// Token: 0x17000ACF RID: 2767
		// (get) Token: 0x060025FE RID: 9726 RVA: 0x00102E34 File Offset: 0x00101034
		// (set) Token: 0x060025FF RID: 9727 RVA: 0x00102E4C File Offset: 0x0010104C
		public bool FastAuditCheck
		{
			get
			{
				return this.fastAuditCheck;
			}
			set
			{
				bool flag = this.fastAuditCheck != value;
				if (flag)
				{
					bool initialized = this.Initialized;
					if (initialized)
					{
						if (value)
						{
							LogbookManager.AddEntry(SSTEntryCode.FastAuditCheck, 1);
						}
						else
						{
							LogbookManager.AddEntry(SSTEntryCode.FastAuditCheck, 0);
						}
					}
					this.fastAuditCheck = value;
					this.OnPropertyChanged("FastAuditCheck");
					if (value)
					{
						this.dbConnection.UpdateCurrentProductTable(this.fastAuditCheckEnabledPath, "1");
					}
					else
					{
						this.dbConnection.UpdateCurrentProductTable(this.fastAuditCheckEnabledPath, "0");
					}
				}
			}
		}

		// Token: 0x06002600 RID: 9728 RVA: 0x00102EE4 File Offset: 0x001010E4
		public AuditCheckSettings(DBConnection dbConnection, ParameterHandling parHandling)
		{
			this.dbConnection = dbConnection;
			this.AuditRequired = new AuditRequired(dbConnection, parHandling);
			this.GetCurrentValues();
		}

		// Token: 0x06002601 RID: 9729 RVA: 0x00102FFC File Offset: 0x001011FC
		public void UpdateAuditInterval()
		{
			bool flag = this.AuditRequired != null;
			if (flag)
			{
				switch (this.selectedMode)
				{
				case AlarmMode.PerHour:
					this.AuditRequired.SetHourlyInterval();
					break;
				case AlarmMode.PerDay:
					this.AuditRequired.SetDailyInterval(this.startTimeHour, this.startTimeMinute);
					break;
				case AlarmMode.PerWeek:
					this.AuditRequired.SetWeeklyInterval(this.alarmDay, this.startTimeHour, this.startTimeMinute);
					break;
				case AlarmMode.PerProductChange:
					this.AuditRequired.SetInterval(this.intervalHour, this.intervalMinute);
					break;
				case AlarmMode.PerInterval:
					this.AuditRequired.SetCustomInterval(this.startTimeHour, this.startTimeMinute, this.intervalHour, this.intervalMinute);
					break;
				}
			}
		}

		// Token: 0x06002602 RID: 9730 RVA: 0x001030CC File Offset: 0x001012CC
		public void GetCurrentValues()
		{
			bool flag = this.dbConnection != null;
			if (flag)
			{
				string b = "";
				string b2 = "";
				string s = "";
				string s2 = "";
				string s3 = "";
				string s4 = "";
				string a = "";
				string a2 = "";
				string a3 = "";
				string a4 = "";
				string s5 = "";
				string s6 = "";
				string a5 = "";
				this.dbConnection.GetParameterValue(this.auditContaminationEnabledPath, ref a2);
				bool flag2 = a2 == "1";
				if (flag2)
				{
					this.ContaminationEnabled = true;
				}
				else
				{
					this.ContaminationEnabled = false;
				}
				this.dbConnection.GetParameterValue(this.auditWeightEnabledPath, ref a3);
				bool flag3 = a3 == "1";
				if (flag3)
				{
					this.WeightEnabled = true;
				}
				else
				{
					this.WeightEnabled = false;
				}
				this.dbConnection.GetParameterValue(this.auditCountEnabledPath, ref a4);
				bool flag4 = a4 == "1";
				if (flag4)
				{
					this.CountEnabled = true;
				}
				else
				{
					this.CountEnabled = false;
				}
				this.dbConnection.GetParameterValue(this.modePath, ref b);
				for (int i = 0; i < this.strModes.Length; i++)
				{
					bool flag5 = this.strModes[i] == b;
					if (flag5)
					{
						this.SelectedMode = (AlarmMode)i;
						break;
					}
				}
				this.dbConnection.GetParameterValue(this.dayPath, ref b2);
				for (int j = 0; j < this.strDays.Length; j++)
				{
					bool flag6 = this.strDays[j] == b2;
					if (flag6)
					{
						this.AlarmDay = (DayOfWeek)j;
						break;
					}
				}
				int num = 0;
				this.dbConnection.GetAuditCheckType(ref num);
				bool flag7 = num == 1 && this.SelectedMode == AlarmMode.PerProductChange && (this.ContaminationEnabled || this.CountEnabled || this.WeightEnabled);
				if (flag7)
				{
					this.SelectedMode = AlarmMode.PerInterval;
					bool flag8 = Application.Current != null;
					if (flag8)
					{
						Application.Current.Dispatcher.BeginInvoke(new Action(delegate()
						{
							Dialog.Show(LocalizationManager.Localize("txtInfo"), "Einstellungen Audit Check bei Produktwechsel haben sich beim neuen Audit Check geändert. Bitte Einstellungen Audit Check überprüfen", DialogStyle.Info, DialogButtons.OK);
						}), new object[0]);
					}
				}
				bool flag9 = this.SelectedMode == AlarmMode.PerWeek;
				if (flag9)
				{
					this.DayVisible = true;
				}
				else
				{
					this.DayVisible = false;
				}
				bool flag10 = this.SelectedMode != AlarmMode.PerHour && this.SelectedMode != AlarmMode.PerProductChange && (num != 1 || this.SelectedMode != AlarmMode.PerInterval);
				if (flag10)
				{
					this.StartTimeVisible = true;
				}
				else
				{
					this.StartTimeVisible = false;
				}
				bool flag11 = this.SelectedMode == AlarmMode.PerInterval || this.SelectedMode == AlarmMode.PerProductChange;
				if (flag11)
				{
					this.IntervalTimeVisible = true;
				}
				else
				{
					this.IntervalTimeVisible = false;
				}
				this.dbConnection.GetParameterValue(this.hourPath, ref s);
				this.dbConnection.GetParameterValue(this.minutePath, ref s2);
				this.dbConnection.GetParameterValue(this.intervalHourPath, ref s3);
				this.dbConnection.GetParameterValue(this.intervalMinutePath, ref s4);
				this.dbConnection.GetParameterValue(AuditRequired.nContaminationsPath, ref s5);
				this.dbConnection.GetParameterValue(AuditRequired.nProductsPath, ref s6);
				try
				{
					this.StartTimeHour = int.Parse(s);
					this.StartTimeMinute = int.Parse(s2);
					this.IntervalHour = int.Parse(s3);
					this.IntervalMinute = int.Parse(s4);
					this.AuditRequired.NContaminations = int.Parse(s5);
					this.AuditRequired.NProducts = int.Parse(s6);
				}
				catch
				{
				}
				this.dbConnection.GetAuditFailedError(ref a);
				bool flag12 = a == "1";
				if (flag12)
				{
					this.AuditRequired.AuditFailedError = true;
				}
				else
				{
					this.AuditRequired.AuditFailedError = false;
				}
				this.dbConnection.GetParameterValue(this.fastAuditCheckEnabledPath, ref a5);
				bool flag13 = a5 == "1";
				if (flag13)
				{
					this.FastAuditCheck = true;
				}
				else
				{
					this.FastAuditCheck = false;
				}
			}
		}

		// Token: 0x06002603 RID: 9731 RVA: 0x00103514 File Offset: 0x00101714
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x06002604 RID: 9732 RVA: 0x00103544 File Offset: 0x00101744
		public void TriggerCheckIfNeeded()
		{
			bool flag = this.AuditRequired != null && (this.ContaminationEnabled || this.WeightEnabled || this.CountEnabled);
			if (flag)
			{
				DateTime now = DateTime.Now;
				string b = "";
				this.dbConnection.GetParameterValue("LastAuditCheckDay", ref b);
				switch (this.selectedMode)
				{
				case AlarmMode.PerDay:
				{
					bool flag2 = now.DayOfWeek.ToString() != b && now.Ticks > new DateTime(now.Year, now.Month, now.Day, this.StartTimeHour, this.StartTimeMinute, 0).Ticks;
					if (flag2)
					{
						new Thread(new ThreadStart(this.AuditRequired.TriggerAuditCheck))
						{
							Name = "TriggerAuditCheck"
						}.Start();
					}
					break;
				}
				case AlarmMode.PerWeek:
				{
					bool flag3 = now.DayOfWeek.ToString() != b && now.DayOfWeek == this.AlarmDay;
					if (flag3)
					{
						bool flag4 = now.Ticks > new DateTime(now.Year, now.Month, now.Day, this.StartTimeHour, this.StartTimeMinute, 0).Ticks;
						if (flag4)
						{
							new Thread(new ThreadStart(this.AuditRequired.TriggerAuditCheck))
							{
								Name = "TriggerAuditCheck"
							}.Start();
						}
					}
					break;
				}
				case AlarmMode.PerProductChange:
					this.AuditRequired.SetInterval(this.intervalHour, this.intervalMinute);
					new Thread(new ThreadStart(this.AuditRequired.TriggerAuditCheck))
					{
						Name = "TriggerAuditCheck"
					}.Start();
					break;
				}
			}
		}

		// Token: 0x04001843 RID: 6211
		private readonly string modePath = "AuditCheckAlarmMode";

		// Token: 0x04001844 RID: 6212
		private readonly string dayPath = "AuditCheckDay";

		// Token: 0x04001845 RID: 6213
		private readonly string intervalHourPath = "AuditCheckIntervalHour";

		// Token: 0x04001846 RID: 6214
		private readonly string intervalMinutePath = "AuditCheckIntervalMinute";

		// Token: 0x04001847 RID: 6215
		private readonly string hourPath = "AuditCheckHour";

		// Token: 0x04001848 RID: 6216
		private readonly string minutePath = "AuditCheckMinute";

		// Token: 0x04001849 RID: 6217
		private readonly string auditContaminationEnabledPath = "AuditContaminationEnabled";

		// Token: 0x0400184A RID: 6218
		private readonly string auditWeightEnabledPath = "AuditWeightEnabled";

		// Token: 0x0400184B RID: 6219
		private readonly string auditCountEnabledPath = "AuditCountEnabled";

		// Token: 0x0400184C RID: 6220
		private string[] strModes = new string[]
		{
			"PerHour",
			"PerDay",
			"PerWeek",
			"PerProductChange",
			"PerInterval"
		};

		// Token: 0x0400184D RID: 6221
		private string[] strDays = new string[]
		{
			"Sunday",
			"Monday",
			"Tuesday",
			"Wednesday",
			"Thursday",
			"Friday",
			"Saturday"
		};

		// Token: 0x0400184E RID: 6222
		private readonly string fastAuditCheckEnabledPath = "FastAuditCheck";

		// Token: 0x0400184F RID: 6223
		private int startTimeMinute;

		// Token: 0x04001850 RID: 6224
		private int startTimeHour;

		// Token: 0x04001851 RID: 6225
		private int intervalHour;

		// Token: 0x04001852 RID: 6226
		private int intervalMinute;

		// Token: 0x04001853 RID: 6227
		private AlarmMode selectedMode;

		// Token: 0x04001854 RID: 6228
		private DBConnection dbConnection;

		// Token: 0x04001855 RID: 6229
		private DayOfWeek alarmDay;

		// Token: 0x04001856 RID: 6230
		private bool dayVisible;

		// Token: 0x04001857 RID: 6231
		private bool startTimeVisible;

		// Token: 0x04001858 RID: 6232
		private bool intervalTimeVisible;

		// Token: 0x04001859 RID: 6233
		private bool contaminationEnabled;

		// Token: 0x0400185A RID: 6234
		private bool weightEnabled;

		// Token: 0x0400185B RID: 6235
		private bool countEnabled;

		// Token: 0x0400185F RID: 6239
		private bool fastAuditCheck;
	}
}
