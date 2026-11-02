using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Timers;
using DatabaseConnection;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x0200018D RID: 397
	public class AuditRequired : INotifyPropertyChanged
	{
		// Token: 0x17000A89 RID: 2697
		// (get) Token: 0x06002541 RID: 9537 RVA: 0x00100449 File Offset: 0x000FE649
		// (set) Token: 0x06002542 RID: 9538 RVA: 0x00100451 File Offset: 0x000FE651
		public bool newAuditCheck { get; set; }

		// Token: 0x17000A8A RID: 2698
		// (get) Token: 0x06002543 RID: 9539 RVA: 0x0010045A File Offset: 0x000FE65A
		// (set) Token: 0x06002544 RID: 9540 RVA: 0x00100462 File Offset: 0x000FE662
		public bool Initialized { get; set; }

		// Token: 0x140000C6 RID: 198
		// (add) Token: 0x06002545 RID: 9541 RVA: 0x0010046C File Offset: 0x000FE66C
		// (remove) Token: 0x06002546 RID: 9542 RVA: 0x001004A4 File Offset: 0x000FE6A4
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x17000A8B RID: 2699
		// (get) Token: 0x06002547 RID: 9543 RVA: 0x001004D9 File Offset: 0x000FE6D9
		// (set) Token: 0x06002548 RID: 9544 RVA: 0x001004E1 File Offset: 0x000FE6E1
		public DBConnection DbConnection { get; set; }

		// Token: 0x17000A8C RID: 2700
		// (get) Token: 0x06002549 RID: 9545 RVA: 0x001004EA File Offset: 0x000FE6EA
		// (set) Token: 0x0600254A RID: 9546 RVA: 0x001004F2 File Offset: 0x000FE6F2
		public BoundLocalizedText StrAuditNeeded { get; set; }

		// Token: 0x140000C7 RID: 199
		// (add) Token: 0x0600254B RID: 9547 RVA: 0x001004FC File Offset: 0x000FE6FC
		// (remove) Token: 0x0600254C RID: 9548 RVA: 0x00100534 File Offset: 0x000FE734
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action TimeToStartAuditElapsed;

		// Token: 0x140000C8 RID: 200
		// (add) Token: 0x0600254D RID: 9549 RVA: 0x0010056C File Offset: 0x000FE76C
		// (remove) Token: 0x0600254E RID: 9550 RVA: 0x001005A4 File Offset: 0x000FE7A4
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action NextCheckElapsed;

		// Token: 0x17000A8D RID: 2701
		// (get) Token: 0x0600254F RID: 9551 RVA: 0x001005DC File Offset: 0x000FE7DC
		// (set) Token: 0x06002550 RID: 9552 RVA: 0x001005F4 File Offset: 0x000FE7F4
		public int TimeToStartAudit
		{
			get
			{
				return this.timeToStartAudit;
			}
			set
			{
				bool flag = this.timeToStartAudit != value && value >= 0;
				if (flag)
				{
					bool initialized = this.Initialized;
					if (initialized)
					{
						LogbookManager.AddEntry(SSTEntryCode.AuditParameterChanged, 6, this.timeToStartAudit.ToString(), value.ToString());
					}
					this.timeToStartAudit = value;
					this.OnPropertyChanged("TimeToStartAudit");
					this.DbConnection.SetTimeToStartAudit(value.ToString());
				}
			}
		}

		// Token: 0x17000A8E RID: 2702
		// (get) Token: 0x06002551 RID: 9553 RVA: 0x00100669 File Offset: 0x000FE869
		// (set) Token: 0x06002552 RID: 9554 RVA: 0x00100671 File Offset: 0x000FE871
		public bool InTestContaminationMode { get; set; }

		// Token: 0x17000A8F RID: 2703
		// (get) Token: 0x06002553 RID: 9555 RVA: 0x0010067A File Offset: 0x000FE87A
		// (set) Token: 0x06002554 RID: 9556 RVA: 0x00100682 File Offset: 0x000FE882
		public bool InTestWeightMode { get; set; }

		// Token: 0x17000A90 RID: 2704
		// (get) Token: 0x06002555 RID: 9557 RVA: 0x0010068B File Offset: 0x000FE88B
		// (set) Token: 0x06002556 RID: 9558 RVA: 0x00100693 File Offset: 0x000FE893
		public bool InTestCountMode { get; set; }

		// Token: 0x17000A91 RID: 2705
		// (get) Token: 0x06002557 RID: 9559 RVA: 0x0010069C File Offset: 0x000FE89C
		// (set) Token: 0x06002558 RID: 9560 RVA: 0x001006B4 File Offset: 0x000FE8B4
		public int RemainingTime
		{
			get
			{
				return this.remainingTime;
			}
			set
			{
				this.remainingTime = value;
				this.OnPropertyChanged("RemainingTime");
			}
		}

		// Token: 0x17000A92 RID: 2706
		// (get) Token: 0x06002559 RID: 9561 RVA: 0x001006CC File Offset: 0x000FE8CC
		// (set) Token: 0x0600255A RID: 9562 RVA: 0x001006E4 File Offset: 0x000FE8E4
		public int NContaminations
		{
			get
			{
				return this.nContaminations;
			}
			set
			{
				bool flag = this.nContaminations != value && value >= 0;
				if (flag)
				{
					bool initialized = this.Initialized;
					if (initialized)
					{
						LogbookManager.AddEntry(SSTEntryCode.AuditParameterChanged, 7, this.nContaminations.ToString(), value.ToString());
					}
					this.nContaminations = value;
					this.OnPropertyChanged("NContaminations");
					this.DbConnection.UpdateCurrentProductTable(AuditRequired.nContaminationsPath, value.ToString());
				}
			}
		}

		// Token: 0x17000A93 RID: 2707
		// (get) Token: 0x0600255B RID: 9563 RVA: 0x00100760 File Offset: 0x000FE960
		// (set) Token: 0x0600255C RID: 9564 RVA: 0x00100778 File Offset: 0x000FE978
		public int NProducts
		{
			get
			{
				return this.nProducts;
			}
			set
			{
				bool flag = this.nProducts != value && value > 0 && value <= 50;
				if (flag)
				{
					bool initialized = this.Initialized;
					if (initialized)
					{
						LogbookManager.AddEntry(SSTEntryCode.AuditParameterChanged, 8, this.nProducts.ToString(), value.ToString());
					}
					this.nProducts = value;
					this.OnPropertyChanged("NProducts");
					this.DbConnection.UpdateCurrentProductTable(AuditRequired.nProductsPath, value.ToString());
				}
			}
		}

		// Token: 0x17000A94 RID: 2708
		// (get) Token: 0x0600255D RID: 9565 RVA: 0x001007F8 File Offset: 0x000FE9F8
		// (set) Token: 0x0600255E RID: 9566 RVA: 0x00100810 File Offset: 0x000FEA10
		public int TimeToNextAudit
		{
			get
			{
				return this.timeToNextAudit;
			}
			set
			{
				this.timeToNextAudit = value;
				this.OnPropertyChanged("TimeToNextAudit");
			}
		}

		// Token: 0x17000A95 RID: 2709
		// (get) Token: 0x0600255F RID: 9567 RVA: 0x00100828 File Offset: 0x000FEA28
		// (set) Token: 0x06002560 RID: 9568 RVA: 0x00100840 File Offset: 0x000FEA40
		public bool ContaminationEnabled
		{
			get
			{
				return this.contaminationEnabled;
			}
			set
			{
				this.contaminationEnabled = value;
				this.OnPropertyChanged("ContaminationEnabled");
			}
		}

		// Token: 0x17000A96 RID: 2710
		// (get) Token: 0x06002561 RID: 9569 RVA: 0x00100858 File Offset: 0x000FEA58
		// (set) Token: 0x06002562 RID: 9570 RVA: 0x00100870 File Offset: 0x000FEA70
		public bool WeightEnabled
		{
			get
			{
				return this.weightEnabled;
			}
			set
			{
				this.weightEnabled = value;
				this.OnPropertyChanged("WeightEnabled");
			}
		}

		// Token: 0x17000A97 RID: 2711
		// (get) Token: 0x06002563 RID: 9571 RVA: 0x00100888 File Offset: 0x000FEA88
		// (set) Token: 0x06002564 RID: 9572 RVA: 0x001008A0 File Offset: 0x000FEAA0
		public bool CountEnabled
		{
			get
			{
				return this.countEnabled;
			}
			set
			{
				this.countEnabled = value;
				this.OnPropertyChanged("CountEnabled");
			}
		}

		// Token: 0x17000A98 RID: 2712
		// (get) Token: 0x06002565 RID: 9573 RVA: 0x001008B8 File Offset: 0x000FEAB8
		// (set) Token: 0x06002566 RID: 9574 RVA: 0x001008D0 File Offset: 0x000FEAD0
		public bool AuditFailedError
		{
			get
			{
				return this.auditFailedError;
			}
			set
			{
				this.auditFailedError = value;
				this.OnPropertyChanged("AuditFailedError");
				if (value)
				{
					this.DbConnection.SetAuditFailedError("1");
				}
				else
				{
					this.DbConnection.SetAuditFailedError("0");
				}
			}
		}

		// Token: 0x17000A99 RID: 2713
		// (get) Token: 0x06002567 RID: 9575 RVA: 0x0010091A File Offset: 0x000FEB1A
		// (set) Token: 0x06002568 RID: 9576 RVA: 0x00100922 File Offset: 0x000FEB22
		public bool LastAuditFailed { get; set; }

		// Token: 0x17000A9A RID: 2714
		// (get) Token: 0x06002569 RID: 9577 RVA: 0x0010092B File Offset: 0x000FEB2B
		// (set) Token: 0x0600256A RID: 9578 RVA: 0x00100933 File Offset: 0x000FEB33
		public bool LastAuditFailedSaved { get; set; }

		// Token: 0x0600256B RID: 9579 RVA: 0x0010093C File Offset: 0x000FEB3C
		public AuditRequired(DBConnection dbConnection, ParameterHandling parHandling)
		{
			this.DbConnection = dbConnection;
			this.parHandling = parHandling;
			string s = "";
			int num = 0;
			dbConnection.GetAuditCheckType(ref num);
			bool flag = num == 1;
			if (flag)
			{
				this.newAuditCheck = true;
			}
			else
			{
				this.newAuditCheck = false;
			}
			this.timerNextCheck = new System.Timers.Timer();
			this.timerNextCheck.Elapsed += this.timerNextCheck_Elapsed;
			this.DbConnection.GetTimeToStartAudit(ref s);
			try
			{
				this.TimeToStartAudit = int.Parse(s);
			}
			catch
			{
			}
			this.StrAuditNeeded = new BoundLocalizedText("txtAuditNeeded");
			System.Timers.Timer timer = new System.Timers.Timer();
			timer.Elapsed += this.updateTimeToNextCheck_Elapsed;
			timer.Interval = 1000.0;
			timer.Start();
			this.LastAuditFailedSaved = false;
		}

		// Token: 0x0600256C RID: 9580 RVA: 0x00100A3C File Offset: 0x000FEC3C
		private void timerLeftToCheck_Elapsed(object sender, ElapsedEventArgs e)
		{
			this.OnTimeToStartAuditElapsed();
		}

		// Token: 0x0600256D RID: 9581 RVA: 0x00100A46 File Offset: 0x000FEC46
		private void timerNextCheck_Elapsed(object sender, ElapsedEventArgs e)
		{
			this.timerNextCheck.Stop();
			this.OnNextCheckElapsed();
		}

		// Token: 0x0600256E RID: 9582 RVA: 0x00100A5C File Offset: 0x000FEC5C
		private void updateTimeToNextCheck_Elapsed(object sender, ElapsedEventArgs e)
		{
			bool flag = this.realTimeToNextAudit > 0;
			if (flag)
			{
				this.realTimeToNextAudit--;
			}
			bool flag2 = this.ContaminationEnabled || this.CountEnabled || this.WeightEnabled;
			if (flag2)
			{
				this.TimeToNextAudit = this.realTimeToNextAudit;
			}
			else
			{
				this.TimeToNextAudit = 0;
			}
		}

		// Token: 0x0600256F RID: 9583 RVA: 0x00100ABC File Offset: 0x000FECBC
		public void UpdateRemainingTime()
		{
			bool flag = this.TimeToStartAudit > 0;
			if (flag)
			{
				this.stopRemainingTimeUpdate = false;
				new Thread(new ThreadStart(this.DecrementRemainingTime))
				{
					Name = "DecrementRemainingAuditTime"
				}.Start();
			}
		}

		// Token: 0x06002570 RID: 9584 RVA: 0x00100B08 File Offset: 0x000FED08
		protected virtual void OnTimeToStartAuditElapsed()
		{
			Action timeToStartAuditElapsed = this.TimeToStartAuditElapsed;
			bool flag = timeToStartAuditElapsed != null;
			if (flag)
			{
				timeToStartAuditElapsed();
			}
		}

		// Token: 0x06002571 RID: 9585 RVA: 0x00100B2C File Offset: 0x000FED2C
		protected virtual void OnNextCheckElapsed()
		{
			Action nextCheckElapsed = this.NextCheckElapsed;
			bool flag = nextCheckElapsed != null;
			if (flag)
			{
				nextCheckElapsed();
			}
		}

		// Token: 0x06002572 RID: 9586 RVA: 0x00100B50 File Offset: 0x000FED50
		public void SetHourlyInterval()
		{
			DateTime now = DateTime.Now;
			this.timerNextCheck.Stop();
			bool flag = now.Second == 0 && now.Minute == 0;
			DateTime dateTime;
			if (flag)
			{
				dateTime = now.AddHours(1.0);
			}
			else
			{
				dateTime = now.AddMinutes((double)(now.Minute * -1)).AddSeconds((double)(now.Second * -1)).AddHours(1.0);
			}
			this.timerNextCheck.Interval = (double)((dateTime.Ticks - now.Ticks) / 10000L);
			this.timerNextCheck.Start();
			this.realTimeToNextAudit = (int)this.timerNextCheck.Interval / 1000;
		}

		// Token: 0x06002573 RID: 9587 RVA: 0x00100C1C File Offset: 0x000FEE1C
		public void SetDailyInterval(int hour, int minute)
		{
			DateTime now = DateTime.Now;
			DateTime dateTime = new DateTime(now.Year, now.Month, now.Day, hour, minute, 0);
			this.timerNextCheck.Stop();
			bool flag = dateTime.Ticks - now.Ticks < 10000000L;
			if (flag)
			{
				dateTime = dateTime.AddDays(1.0);
			}
			this.timerNextCheck.Interval = (double)((dateTime.Ticks - now.Ticks) / 10000L);
			this.timerNextCheck.Start();
			this.realTimeToNextAudit = (int)this.timerNextCheck.Interval / 1000;
		}

		// Token: 0x06002574 RID: 9588 RVA: 0x00100CD0 File Offset: 0x000FEED0
		public void SetWeeklyInterval(DayOfWeek day, int hour, int minute)
		{
			DateTime now = DateTime.Now;
			DateTime dateTime = new DateTime(now.Year, now.Month, now.Day, hour, minute, 0);
			this.timerNextCheck.Stop();
			int num = day - now.DayOfWeek;
			bool flag = num < 0;
			if (flag)
			{
				num = 7 + num;
			}
			dateTime = dateTime.AddDays((double)num);
			bool flag2 = dateTime.Ticks - now.Ticks < 10000000L;
			if (flag2)
			{
				dateTime = dateTime.AddDays(7.0);
			}
			this.timerNextCheck.Interval = (double)((dateTime.Ticks - now.Ticks) / 10000L);
			this.timerNextCheck.Start();
			this.realTimeToNextAudit = (int)this.timerNextCheck.Interval / 1000;
		}

		// Token: 0x06002575 RID: 9589 RVA: 0x00100DA4 File Offset: 0x000FEFA4
		public void SetCustomInterval(int startHour, int startMinute, int intervalHour, int intervalMinute)
		{
			DateTime now = DateTime.Now;
			this.timerNextCheck.Stop();
			double num = (double)((intervalHour * 3600 + intervalMinute * 60) * 1000);
			int num2 = (startHour * 3600 + startMinute * 60) * 1000;
			int num3 = (now.Hour * 3600 + now.Minute * 60 + now.Second) * 1000;
			bool flag = num > 0.0;
			if (flag)
			{
				bool flag2 = num3 >= num2;
				if (flag2)
				{
					double num4 = (double)(num3 - num2) % num;
					num4 /= num;
					num = (1.0 - num4) * num;
				}
				else
				{
					num = (double)(num2 - num3);
				}
				this.timerNextCheck.Interval = num;
				this.timerNextCheck.Start();
				this.realTimeToNextAudit = (int)this.timerNextCheck.Interval / 1000;
			}
			else
			{
				this.realTimeToNextAudit = 0;
			}
		}

		// Token: 0x06002576 RID: 9590 RVA: 0x00100E94 File Offset: 0x000FF094
		public void SetInterval(int intervalHour, int intervalMinute)
		{
			this.timerNextCheck.Stop();
			double num = (double)((intervalHour * 3600 + intervalMinute * 60) * 1000);
			bool flag = num > 1.0;
			if (flag)
			{
				this.timerNextCheck.Interval = num;
				this.timerNextCheck.Start();
				this.realTimeToNextAudit = (int)this.timerNextCheck.Interval / 1000;
			}
			else
			{
				this.realTimeToNextAudit = 0;
			}
		}

		// Token: 0x06002577 RID: 9591 RVA: 0x00100F10 File Offset: 0x000FF110
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x06002578 RID: 9592 RVA: 0x00100F40 File Offset: 0x000FF140
		private void DecrementRemainingTime()
		{
			this.RemainingTime = this.TimeToStartAudit * 60;
			while (!this.stopRemainingTimeUpdate && this.RemainingTime > 0)
			{
				this.RemainingTime--;
				Thread.Sleep(1000);
			}
			bool flag = this.RemainingTime == 0;
			if (flag)
			{
				this.OnTimeToStartAuditElapsed();
			}
			this.RemainingTime = 0;
		}

		// Token: 0x06002579 RID: 9593 RVA: 0x00100FB0 File Offset: 0x000FF1B0
		public void StopRemainingTimeUpdate()
		{
			this.stopRemainingTimeUpdate = true;
			Thread.Sleep(1100);
		}

		// Token: 0x0600257A RID: 9594 RVA: 0x00100FC5 File Offset: 0x000FF1C5
		public void TriggerAuditCheck()
		{
			this.timerNextCheck.Stop();
			this.timerNextCheck_Elapsed(null, null);
		}

		// Token: 0x0600257B RID: 9595 RVA: 0x00100FE0 File Offset: 0x000FF1E0
		public void ResetProperties()
		{
			this.InTestContaminationMode = false;
			this.InTestWeightMode = false;
			this.InTestCountMode = false;
			this.StrAuditNeeded.ResourceKey = "txtAuditNeeded";
			bool flag = this.parHandling.ReadPar(Definition.contaminationsCounterEnablePath) == "1";
			if (flag)
			{
				this.contaminationCounterWasActive = true;
			}
			else
			{
				this.parHandling.WriteValue(Definition.contaminationsCounterEnablePath, "1");
				this.contaminationCounterWasActive = false;
			}
		}

		// Token: 0x0600257C RID: 9596 RVA: 0x00101060 File Offset: 0x000FF260
		public void Disable()
		{
			bool flag = this.timerNextCheck != null;
			if (flag)
			{
				this.timerNextCheck.Stop();
			}
		}

		// Token: 0x0600257D RID: 9597 RVA: 0x00101088 File Offset: 0x000FF288
		public void DisableContaminationsCounter()
		{
			bool flag = !this.contaminationCounterWasActive;
			if (flag)
			{
				this.parHandling.WriteValue(Definition.contaminationsCounterEnablePath, "0");
			}
		}

		// Token: 0x040017F5 RID: 6133
		public static readonly string nContaminationsPath = "AuditCheckNContaminations";

		// Token: 0x040017F6 RID: 6134
		public static readonly string nProductsPath = "AuditCheckNProducts";

		// Token: 0x040017F7 RID: 6135
		private int timeToStartAudit;

		// Token: 0x040017F8 RID: 6136
		private System.Timers.Timer timerNextCheck = new System.Timers.Timer();

		// Token: 0x040017F9 RID: 6137
		private int remainingTime;

		// Token: 0x040017FA RID: 6138
		private bool stopRemainingTimeUpdate;

		// Token: 0x040017FB RID: 6139
		private ParameterHandling parHandling;

		// Token: 0x040017FC RID: 6140
		private int nContaminations;

		// Token: 0x040017FD RID: 6141
		private bool contaminationCounterWasActive;

		// Token: 0x040017FE RID: 6142
		private int timeToNextAudit;

		// Token: 0x040017FF RID: 6143
		private bool contaminationEnabled;

		// Token: 0x04001800 RID: 6144
		private bool weightEnabled;

		// Token: 0x04001801 RID: 6145
		private bool countEnabled;

		// Token: 0x04001802 RID: 6146
		private int realTimeToNextAudit;

		// Token: 0x04001803 RID: 6147
		private bool auditFailedError;

		// Token: 0x04001804 RID: 6148
		private int nProducts;
	}
}
