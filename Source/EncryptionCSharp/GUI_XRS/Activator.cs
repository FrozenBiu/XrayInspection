using System;
using System.ComponentModel;
using System.Diagnostics;
using DatabaseConnection;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x02000040 RID: 64
	public class Activator : INotifyPropertyChanged
	{
		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x06000695 RID: 1685 RVA: 0x0002BC78 File Offset: 0x00029E78
		// (set) Token: 0x06000696 RID: 1686 RVA: 0x0002BC90 File Offset: 0x00029E90
		public int OnTimeMinutes
		{
			get
			{
				return this.onTimeMinutes;
			}
			set
			{
				bool flag = this.onTimeMinutes != value && value * 60 + this.onTimeSeconds > 0;
				if (flag)
				{
					this.onTimeMinutes = value;
					bool flag2 = this.mode > ActivatorMode.Disabled;
					if (flag2)
					{
						this.parHandling.WriteValue(Definition.cleaningVibratorOnTimePath, (this.onTimeMinutes * 60 + this.onTimeSeconds).ToString(), false);
					}
					this.dbConnection.UpdateCurrentProductTable(Definition.dbCleaningVibratorOnTimePath, (this.onTimeMinutes * 60 + this.onTimeSeconds).ToString());
					this.OnPropertyChanged("OnTimeMinutes");
				}
			}
		}

		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x06000697 RID: 1687 RVA: 0x0002BD34 File Offset: 0x00029F34
		// (set) Token: 0x06000698 RID: 1688 RVA: 0x0002BD4C File Offset: 0x00029F4C
		public int OnTimeSeconds
		{
			get
			{
				return this.onTimeSeconds;
			}
			set
			{
				bool flag = this.onTimeSeconds != value && this.onTimeMinutes * 60 + value > 0;
				if (flag)
				{
					bool flag2 = value >= 0 && value < 60;
					if (flag2)
					{
						this.onTimeSeconds = value;
						bool flag3 = this.mode > ActivatorMode.Disabled;
						if (flag3)
						{
							this.parHandling.WriteValue(Definition.cleaningVibratorOnTimePath, (this.onTimeMinutes * 60 + this.onTimeSeconds).ToString(), false);
						}
						this.dbConnection.UpdateCurrentProductTable(Definition.dbCleaningVibratorOnTimePath, (this.onTimeMinutes * 60 + this.onTimeSeconds).ToString());
						this.OnPropertyChanged("OnTimeSeconds");
					}
				}
			}
		}

		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x06000699 RID: 1689 RVA: 0x0002BE04 File Offset: 0x0002A004
		// (set) Token: 0x0600069A RID: 1690 RVA: 0x0002BE1C File Offset: 0x0002A01C
		public int OffTimeMinutes
		{
			get
			{
				return this.offTimeMinutes;
			}
			set
			{
				bool flag = this.offTimeMinutes != value && value * 60 + this.offTimeSeconds > 0;
				if (flag)
				{
					this.offTimeMinutes = value;
					bool flag2 = this.mode > ActivatorMode.Disabled;
					if (flag2)
					{
						this.parHandling.WriteValue(Definition.cleaningVibratorOffTimePath, (this.offTimeMinutes * 60 + this.offTimeSeconds).ToString(), false);
					}
					this.dbConnection.UpdateCurrentProductTable(Definition.dbCleaningVibratorOffTimePath, (this.offTimeMinutes * 60 + this.offTimeSeconds).ToString());
					this.OnPropertyChanged("OffTimeMinutes");
				}
			}
		}

		// Token: 0x170001F3 RID: 499
		// (get) Token: 0x0600069B RID: 1691 RVA: 0x0002BEC0 File Offset: 0x0002A0C0
		// (set) Token: 0x0600069C RID: 1692 RVA: 0x0002BED8 File Offset: 0x0002A0D8
		public int OffTimeSeconds
		{
			get
			{
				return this.offTimeSeconds;
			}
			set
			{
				bool flag = this.offTimeSeconds != value && this.offTimeMinutes * 60 + value > 0;
				if (flag)
				{
					bool flag2 = value >= 0 && value < 60;
					if (flag2)
					{
						this.offTimeSeconds = value;
						bool flag3 = this.mode > ActivatorMode.Disabled;
						if (flag3)
						{
							this.parHandling.WriteValue(Definition.cleaningVibratorOffTimePath, (this.offTimeMinutes * 60 + this.offTimeSeconds).ToString(), false);
						}
						this.dbConnection.UpdateCurrentProductTable(Definition.dbCleaningVibratorOffTimePath, (this.offTimeMinutes * 60 + this.offTimeSeconds).ToString());
						this.OnPropertyChanged("OffTimeSeconds");
					}
				}
			}
		}

		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x0600069D RID: 1693 RVA: 0x0002BF90 File Offset: 0x0002A190
		// (set) Token: 0x0600069E RID: 1694 RVA: 0x0002BFAC File Offset: 0x0002A1AC
		public bool AlwaysEnabled
		{
			get
			{
				return this.mode == ActivatorMode.AlwaysEnabled;
			}
			set
			{
				if (value)
				{
					this.Mode = ActivatorMode.AlwaysEnabled;
				}
			}
		}

		// Token: 0x170001F5 RID: 501
		// (get) Token: 0x0600069F RID: 1695 RVA: 0x0002BFC8 File Offset: 0x0002A1C8
		// (set) Token: 0x060006A0 RID: 1696 RVA: 0x0002BFE4 File Offset: 0x0002A1E4
		public bool Disabled
		{
			get
			{
				return this.mode == ActivatorMode.Disabled;
			}
			set
			{
				if (value)
				{
					this.Mode = ActivatorMode.Disabled;
				}
			}
		}

		// Token: 0x170001F6 RID: 502
		// (get) Token: 0x060006A1 RID: 1697 RVA: 0x0002C000 File Offset: 0x0002A200
		// (set) Token: 0x060006A2 RID: 1698 RVA: 0x0002C01C File Offset: 0x0002A21C
		public bool IntervalMode
		{
			get
			{
				return this.mode == ActivatorMode.IntervalMode;
			}
			set
			{
				if (value)
				{
					this.Mode = ActivatorMode.IntervalMode;
				}
			}
		}

		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x060006A3 RID: 1699 RVA: 0x0002C038 File Offset: 0x0002A238
		// (set) Token: 0x060006A4 RID: 1700 RVA: 0x0002C050 File Offset: 0x0002A250
		public ActivatorMode Mode
		{
			get
			{
				return this.mode;
			}
			set
			{
				bool flag = this.mode != value;
				if (flag)
				{
					this.mode = value;
					bool flag2 = this.mode == ActivatorMode.Disabled;
					if (flag2)
					{
						this.parHandling.WriteValue(Definition.cleaningVibratorOnTimePath, "0", false);
						this.parHandling.WriteValue(Definition.cleaningVibratorOffTimePath, "0", false);
					}
					bool flag3 = this.mode == ActivatorMode.IntervalMode;
					if (flag3)
					{
						string value2 = null;
						this.dbConnection.GetParameterValue(Definition.dbCleaningVibratorOnTimePath, ref value2);
						this.parHandling.WriteValue(Definition.cleaningVibratorOnTimePath, value2, false);
						this.dbConnection.GetParameterValue(Definition.dbCleaningVibratorOffTimePath, ref value2);
						this.parHandling.WriteValue(Definition.cleaningVibratorOffTimePath, value2, false);
					}
					this.parHandling.WriteValue(Definition.cleaningVibratorAlwaysOnPath, (this.mode == ActivatorMode.AlwaysEnabled) ? "1" : "0", false);
					DBConnection dbconnection = this.dbConnection;
					string dbCleaningVibratorModePath = Definition.dbCleaningVibratorModePath;
					int num = (int)this.mode;
					dbconnection.UpdateCurrentProductTable(dbCleaningVibratorModePath, num.ToString());
					this.OnPropertyChanged("Mode");
				}
			}
		}

		// Token: 0x14000027 RID: 39
		// (add) Token: 0x060006A5 RID: 1701 RVA: 0x0002C168 File Offset: 0x0002A368
		// (remove) Token: 0x060006A6 RID: 1702 RVA: 0x0002C1A0 File Offset: 0x0002A3A0
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x060006A7 RID: 1703 RVA: 0x0002C1D5 File Offset: 0x0002A3D5
		public Activator(DBConnection dbConnection, ParameterHandling parHandling)
		{
			this.dbConnection = dbConnection;
			this.parHandling = parHandling;
			this.Initialize();
		}

		// Token: 0x060006A8 RID: 1704 RVA: 0x0002C1F4 File Offset: 0x0002A3F4
		private void Initialize()
		{
			string s = null;
			int num = 0;
			this.dbConnection.GetParameterValue(Definition.dbCleaningVibratorOnTimePath, ref s);
			bool flag = int.TryParse(s, out num);
			if (flag)
			{
				this.OnTimeMinutes = num / 60;
				this.OnTimeSeconds = num % 60;
			}
			this.dbConnection.GetParameterValue(Definition.dbCleaningVibratorOffTimePath, ref s);
			bool flag2 = int.TryParse(s, out num);
			if (flag2)
			{
				this.OffTimeMinutes = num / 60;
				this.OffTimeSeconds = num % 60;
			}
			this.dbConnection.GetParameterValue(Definition.dbCleaningVibratorModePath, ref s);
			bool flag3 = int.TryParse(s, out num);
			if (flag3)
			{
				bool flag4 = num >= 0 && num <= 2;
				if (flag4)
				{
					this.Mode = (ActivatorMode)num;
				}
			}
		}

		// Token: 0x060006A9 RID: 1705 RVA: 0x0002C2B4 File Offset: 0x0002A4B4
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x040004A8 RID: 1192
		private int onTimeMinutes;

		// Token: 0x040004A9 RID: 1193
		private int onTimeSeconds;

		// Token: 0x040004AA RID: 1194
		private int offTimeMinutes;

		// Token: 0x040004AB RID: 1195
		private int offTimeSeconds;

		// Token: 0x040004AC RID: 1196
		private DBConnection dbConnection;

		// Token: 0x040004AD RID: 1197
		private ParameterHandling parHandling;

		// Token: 0x040004AE RID: 1198
		private ActivatorMode mode;
	}
}
