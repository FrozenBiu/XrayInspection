using System;
using System.ComponentModel;
using System.Diagnostics;
using DatabaseConnection;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x020000A7 RID: 167
	public class HardwareFeaturesViewModel : INotifyPropertyChanged
	{
		// Token: 0x170004FD RID: 1277
		// (get) Token: 0x0600108A RID: 4234 RVA: 0x00066738 File Offset: 0x00064938
		// (set) Token: 0x0600108B RID: 4235 RVA: 0x00066750 File Offset: 0x00064950
		public bool RFIDLoginVisible
		{
			get
			{
				return this.isRFIDLoginVisible;
			}
			set
			{
				this.isRFIDLoginVisible = value;
				this.OnPropertyChanged("RFIDLoginVisible");
			}
		}

		// Token: 0x170004FE RID: 1278
		// (get) Token: 0x0600108C RID: 4236 RVA: 0x00066768 File Offset: 0x00064968
		// (set) Token: 0x0600108D RID: 4237 RVA: 0x00066780 File Offset: 0x00064980
		public bool IsHigherLevelAndLineJamVisible
		{
			get
			{
				return this.isHigherLevelAndLineJamVisible;
			}
			set
			{
				this.isHigherLevelAndLineJamVisible = value;
				this.OnPropertyChanged("IsHigherLevelAndLineJamVisible");
			}
		}

		// Token: 0x14000053 RID: 83
		// (add) Token: 0x0600108E RID: 4238 RVA: 0x00066798 File Offset: 0x00064998
		// (remove) Token: 0x0600108F RID: 4239 RVA: 0x000667D0 File Offset: 0x000649D0
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x170004FF RID: 1279
		// (get) Token: 0x06001090 RID: 4240 RVA: 0x00066808 File Offset: 0x00064A08
		// (set) Token: 0x06001091 RID: 4241 RVA: 0x00066820 File Offset: 0x00064A20
		public bool EnableRFIDLoginFeature
		{
			get
			{
				return this.enableRFIDLogin;
			}
			set
			{
				bool flag = this.enableRFIDLogin != value;
				if (flag)
				{
					this.enableRFIDLogin = value;
					this.dbConnection.SetRFIDLoginAvailable(value);
					this.parHandling.WriteValue(Definition.rfidLoginAvailablePath, value ? "1" : "0");
					this.OnPropertyChanged("EnableRFIDLoginFeature");
				}
			}
		}

		// Token: 0x17000500 RID: 1280
		// (get) Token: 0x06001092 RID: 4242 RVA: 0x00066880 File Offset: 0x00064A80
		// (set) Token: 0x06001093 RID: 4243 RVA: 0x00066898 File Offset: 0x00064A98
		public bool EnableHigherLevelFeatures
		{
			get
			{
				return this.enableHigherLevelFeatures;
			}
			set
			{
				bool flag = this.enableHigherLevelFeatures != value;
				if (flag)
				{
					this.enableHigherLevelFeatures = value;
					this.dbConnection.SetHigherLevelFeatures(value);
					this.parHandling.WriteValue(Definition.machineModePath, (value || this.enableLineJamSensor) ? 3.ToString() : 0.ToString());
					this.parHandling.WriteValue(Definition.ioSubversionPath, (value || this.enableLineJamSensor) ? "3" : "1", false);
					this.parHandling.WriteValue(Definition.doorLockAvailablePath, value ? "1" : "0");
					this.OnPropertyChanged("EnableHigherLevelFeatures");
				}
			}
		}

		// Token: 0x17000501 RID: 1281
		// (get) Token: 0x06001094 RID: 4244 RVA: 0x00066954 File Offset: 0x00064B54
		// (set) Token: 0x06001095 RID: 4245 RVA: 0x0006696C File Offset: 0x00064B6C
		public bool IsBulkMachine
		{
			get
			{
				return this.isBulkMachine;
			}
			set
			{
				bool flag = this.isBulkMachine != value;
				if (flag)
				{
					this.isBulkMachine = value;
					this.OnPropertyChanged("IsBulkMachine");
				}
			}
		}

		// Token: 0x17000502 RID: 1282
		// (get) Token: 0x06001096 RID: 4246 RVA: 0x000669A0 File Offset: 0x00064BA0
		// (set) Token: 0x06001097 RID: 4247 RVA: 0x000669B8 File Offset: 0x00064BB8
		public bool EnableBulkBinFullSensor
		{
			get
			{
				return this.enableBulkBinFullSensor;
			}
			set
			{
				bool flag = this.enableBulkBinFullSensor != value;
				if (flag)
				{
					this.enableBulkBinFullSensor = value;
					this.dbConnection.SetBulkBinFullSensor(value);
					this.parHandling.WriteValue(Definition.bulkBinFullSensorPath, value ? "1" : "0");
					this.OnPropertyChanged("EnableBulkBinFullSensor");
				}
			}
		}

		// Token: 0x17000503 RID: 1283
		// (get) Token: 0x06001098 RID: 4248 RVA: 0x00066A18 File Offset: 0x00064C18
		// (set) Token: 0x06001099 RID: 4249 RVA: 0x00066A30 File Offset: 0x00064C30
		public bool EnableBulkRejectVerifySensor
		{
			get
			{
				return this.enableBulkRejectVerifySensor;
			}
			set
			{
				bool flag = this.enableBulkRejectVerifySensor != value;
				if (flag)
				{
					this.enableBulkRejectVerifySensor = value;
					this.dbConnection.SetBulkRejectVerifySensor(value);
					this.parHandling.WriteValue(Definition.bulkRejectVerifySensorrPath, value ? "1" : "0");
					this.OnPropertyChanged("EnableBulkRejectVerifySensor");
				}
			}
		}

		// Token: 0x17000504 RID: 1284
		// (get) Token: 0x0600109A RID: 4250 RVA: 0x00066A90 File Offset: 0x00064C90
		// (set) Token: 0x0600109B RID: 4251 RVA: 0x00066AA8 File Offset: 0x00064CA8
		public bool EnableBulkSpeedBelow08
		{
			get
			{
				return this.enableBulkSpeedBelow08;
			}
			set
			{
				bool flag = this.enableBulkSpeedBelow08 != value;
				if (flag)
				{
					this.enableBulkSpeedBelow08 = value;
					this.dbConnection.SetBulkSpeedBelow08(value);
					this.conveyor.UpdateSpeed();
					this.conveyor.AdaptScanrate();
					this.parHandling.WriteValue(Definition.bulkSpeedBelow08Path, value ? "1" : "0");
					this.OnPropertyChanged("EnableBulkSpeedBelow08");
				}
			}
		}

		// Token: 0x17000505 RID: 1285
		// (get) Token: 0x0600109C RID: 4252 RVA: 0x00066B20 File Offset: 0x00064D20
		// (set) Token: 0x0600109D RID: 4253 RVA: 0x00066B38 File Offset: 0x00064D38
		public bool EnableLineJamSensor
		{
			get
			{
				return this.enableLineJamSensor;
			}
			set
			{
				bool flag = this.enableLineJamSensor != value;
				if (flag)
				{
					this.enableLineJamSensor = value;
					this.dbConnection.SetLineJamSensor(value);
					this.parHandling.WriteValue(Definition.machineModePath, (value || this.enableHigherLevelFeatures) ? 3.ToString() : 0.ToString());
					this.parHandling.WriteValue(Definition.ioSubversionPath, (value || this.enableHigherLevelFeatures) ? "3" : "1", false);
					this.OnPropertyChanged("EnableLineJamSensor");
				}
			}
		}

		// Token: 0x0600109E RID: 4254 RVA: 0x00066BD4 File Offset: 0x00064DD4
		public HardwareFeaturesViewModel(DBConnection dbConnection, ParameterHandling parHandling, Conveyor conveyor)
		{
			this.dbConnection = dbConnection;
			this.parHandling = parHandling;
			this.conveyor = conveyor;
			MachineType machineType = MachineType.MachineTypeUndefined;
			MachineMode machineMode = MachineMode.Standard;
			string value = parHandling.ReadPar(Definition.machineTypePath);
			dbConnection.GetMachineMode(ref machineMode);
			try
			{
				machineType = (MachineType)Enum.Parse(typeof(MachineType), value);
			}
			catch
			{
			}
			bool flag = machineType != MachineType.RayconXRS && machineType != MachineType.RayconD && machineType != MachineType.RayconEntry;
			if (flag)
			{
				this.isRFIDLoginVisible = true;
			}
			this.isHigherLevelAndLineJamVisible = (machineMode != MachineMode.Bulk && machineMode != MachineMode.BulkObject);
			dbConnection.GetMachineMode(ref machineMode);
			dbConnection.GetRFIDLoginAvailable(ref this.enableRFIDLogin);
			dbConnection.GetHigherLevelFeatures(ref this.enableHigherLevelFeatures);
			dbConnection.GetLineJamSensor(ref this.enableLineJamSensor);
			bool flag2 = machineMode == MachineMode.Bulk || machineMode == MachineMode.BulkObject;
			if (flag2)
			{
				this.IsBulkMachine = true;
				dbConnection.GetBulkBinFullSensor(ref this.enableBulkBinFullSensor);
				dbConnection.GetBulkRejectVerifySensor(ref this.enableBulkRejectVerifySensor);
				dbConnection.GetBulkSpeedBelow08(ref this.enableBulkSpeedBelow08);
				bool flag3 = !this.enableBulkSpeedBelow08;
				if (flag3)
				{
					conveyor.Speed = conveyor.Speed;
				}
			}
		}

		// Token: 0x0600109F RID: 4255 RVA: 0x00066D0C File Offset: 0x00064F0C
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x04000B0A RID: 2826
		private bool isRFIDLoginVisible;

		// Token: 0x04000B0B RID: 2827
		private bool enableRFIDLogin;

		// Token: 0x04000B0C RID: 2828
		private bool enableLineJamSensor;

		// Token: 0x04000B0D RID: 2829
		private bool enableHigherLevelFeatures;

		// Token: 0x04000B0E RID: 2830
		private bool enableBulkBinFullSensor;

		// Token: 0x04000B0F RID: 2831
		private bool enableBulkRejectVerifySensor;

		// Token: 0x04000B10 RID: 2832
		private bool enableBulkSpeedBelow08;

		// Token: 0x04000B11 RID: 2833
		private bool isHigherLevelAndLineJamVisible;

		// Token: 0x04000B12 RID: 2834
		private bool isBulkMachine;

		// Token: 0x04000B13 RID: 2835
		private Conveyor conveyor;

		// Token: 0x04000B14 RID: 2836
		private DBConnection dbConnection;

		// Token: 0x04000B15 RID: 2837
		private ParameterHandling parHandling;
	}
}
