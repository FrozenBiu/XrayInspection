using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using DatabaseConnection;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x02000112 RID: 274
	public class ErrorHandling : INotifyPropertyChanged
	{
		// Token: 0x14000085 RID: 133
		// (add) Token: 0x060019C9 RID: 6601 RVA: 0x00097A08 File Offset: 0x00095C08
		// (remove) Token: 0x060019CA RID: 6602 RVA: 0x00097A40 File Offset: 0x00095C40
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x170007AA RID: 1962
		// (get) Token: 0x060019CB RID: 6603 RVA: 0x00097A78 File Offset: 0x00095C78
		// (set) Token: 0x060019CC RID: 6604 RVA: 0x00097A90 File Offset: 0x00095C90
		public bool DetectorErrorEnabled
		{
			get
			{
				return this.detectorErrorEnabled;
			}
			set
			{
				this.detectorErrorEnabled = value;
				this.dbConnection.SetErrorContact(Definition.ErrorContact.Detector, value);
				this.OnPropertyChanged("DetectorErrorEnabled");
			}
		}

		// Token: 0x170007AB RID: 1963
		// (get) Token: 0x060019CD RID: 6605 RVA: 0x00097AB4 File Offset: 0x00095CB4
		// (set) Token: 0x060019CE RID: 6606 RVA: 0x00097ACC File Offset: 0x00095CCC
		public bool XrayErrorEnabled
		{
			get
			{
				return this.xrayErrorEnabled;
			}
			set
			{
				this.xrayErrorEnabled = value;
				this.dbConnection.SetErrorContact(Definition.ErrorContact.XRaySource, value);
				this.OnPropertyChanged("XrayErrorEnabled");
			}
		}

		// Token: 0x170007AC RID: 1964
		// (get) Token: 0x060019CF RID: 6607 RVA: 0x00097AF0 File Offset: 0x00095CF0
		// (set) Token: 0x060019D0 RID: 6608 RVA: 0x00097B08 File Offset: 0x00095D08
		public bool BinFullEnabled
		{
			get
			{
				return this.binFullEnabled;
			}
			set
			{
				this.binFullEnabled = value;
				this.dbConnection.SetErrorContact(Definition.ErrorContact.BinFull, value);
				this.parHandling.WriteValue(Definition.beltStopOnBinFullErrorEnablePath, (value && this.stopConveyorIfError) ? "1" : "0", false);
				this.OnPropertyChanged("BinFullEnabled");
			}
		}

		// Token: 0x170007AD RID: 1965
		// (get) Token: 0x060019D1 RID: 6609 RVA: 0x00097B60 File Offset: 0x00095D60
		// (set) Token: 0x060019D2 RID: 6610 RVA: 0x00097B78 File Offset: 0x00095D78
		public bool MissingObjectsEnabled
		{
			get
			{
				return this.missingObjectsEnabled;
			}
			set
			{
				this.missingObjectsEnabled = value;
				this.dbConnection.SetErrorContact(Definition.ErrorContact.RejectVerify, value);
				this.parHandling.WriteValue(Definition.beltStopOnVerifyErrorEnablePath, (value && this.stopConveyorIfError) ? "1" : "0", false);
				this.OnPropertyChanged("MissingObjectsEnabled");
			}
		}

		// Token: 0x170007AE RID: 1966
		// (get) Token: 0x060019D3 RID: 6611 RVA: 0x00097BD0 File Offset: 0x00095DD0
		// (set) Token: 0x060019D4 RID: 6612 RVA: 0x00097BE8 File Offset: 0x00095DE8
		public bool VoltageErrorEnabled
		{
			get
			{
				return this.voltageErrorEnabled;
			}
			set
			{
				this.voltageErrorEnabled = value;
				this.dbConnection.SetErrorContact(Definition.ErrorContact.Voltage, value);
				this.OnPropertyChanged("VoltageErrorEnabled");
			}
		}

		// Token: 0x170007AF RID: 1967
		// (get) Token: 0x060019D5 RID: 6613 RVA: 0x00097C0C File Offset: 0x00095E0C
		// (set) Token: 0x060019D6 RID: 6614 RVA: 0x00097C24 File Offset: 0x00095E24
		public bool TemperatureErrorEnabled
		{
			get
			{
				return this.temperatureErrorEnabled;
			}
			set
			{
				this.temperatureErrorEnabled = value;
				this.dbConnection.SetErrorContact(Definition.ErrorContact.Temperature, value);
				this.OnPropertyChanged("TemperatureErrorEnabled");
			}
		}

		// Token: 0x170007B0 RID: 1968
		// (get) Token: 0x060019D7 RID: 6615 RVA: 0x00097C48 File Offset: 0x00095E48
		// (set) Token: 0x060019D8 RID: 6616 RVA: 0x00097C60 File Offset: 0x00095E60
		public bool EngineErrorEnabled
		{
			get
			{
				return this.engineErrorEnabled;
			}
			set
			{
				this.engineErrorEnabled = value;
				this.dbConnection.SetErrorContact(Definition.ErrorContact.Motor, value);
				this.OnPropertyChanged("EngineErrorEnabled");
			}
		}

		// Token: 0x170007B1 RID: 1969
		// (get) Token: 0x060019D9 RID: 6617 RVA: 0x00097C84 File Offset: 0x00095E84
		// (set) Token: 0x060019DA RID: 6618 RVA: 0x00097C9C File Offset: 0x00095E9C
		public bool LowAirPressureErrorEnabled
		{
			get
			{
				return this.lowAirPressureErrorEnabled;
			}
			set
			{
				this.lowAirPressureErrorEnabled = value;
				this.dbConnection.SetErrorContact(Definition.ErrorContact.AirPressure, value);
				this.OnPropertyChanged("LowAirPressureErrorEnabled");
			}
		}

		// Token: 0x170007B2 RID: 1970
		// (get) Token: 0x060019DB RID: 6619 RVA: 0x00097CC0 File Offset: 0x00095EC0
		// (set) Token: 0x060019DC RID: 6620 RVA: 0x00097CD8 File Offset: 0x00095ED8
		public bool CoolingErrorEnabled
		{
			get
			{
				return this.coolingErrorEnabled;
			}
			set
			{
				this.coolingErrorEnabled = value;
				this.dbConnection.SetErrorContact(Definition.ErrorContact.Cooling, value);
				this.OnPropertyChanged("CoolingErrorEnabled");
			}
		}

		// Token: 0x170007B3 RID: 1971
		// (get) Token: 0x060019DD RID: 6621 RVA: 0x00097CFC File Offset: 0x00095EFC
		// (set) Token: 0x060019DE RID: 6622 RVA: 0x00097D14 File Offset: 0x00095F14
		public bool XrayLampErrorEnabled
		{
			get
			{
				return this.xrayLampErrorEnabled;
			}
			set
			{
				this.xrayLampErrorEnabled = value;
				this.dbConnection.SetErrorContact(Definition.ErrorContact.XRayLamp, value);
				this.OnPropertyChanged("XrayLampErrorEnabled");
			}
		}

		// Token: 0x170007B4 RID: 1972
		// (get) Token: 0x060019DF RID: 6623 RVA: 0x00097D3C File Offset: 0x00095F3C
		// (set) Token: 0x060019E0 RID: 6624 RVA: 0x00097D54 File Offset: 0x00095F54
		public bool BinMissingEnabled
		{
			get
			{
				return this.binMissingEnabled;
			}
			set
			{
				this.binMissingEnabled = value;
				this.dbConnection.SetErrorContact(Definition.ErrorContact.BinMissing, value);
				this.parHandling.WriteValue(Definition.BeltStopOnBinMissingEnablePath, (value && this.stopConveyorIfError) ? "1" : "0", false);
				this.OnPropertyChanged("BinMissingEnabled");
			}
		}

		// Token: 0x170007B5 RID: 1973
		// (get) Token: 0x060019E1 RID: 6625 RVA: 0x00097DB0 File Offset: 0x00095FB0
		// (set) Token: 0x060019E2 RID: 6626 RVA: 0x00097DC8 File Offset: 0x00095FC8
		public bool HighLevelSensorsEnabled
		{
			get
			{
				return this.highLevelSensorsEnabled;
			}
			set
			{
				this.highLevelSensorsEnabled = value;
				this.dbConnection.SetErrorContact(Definition.ErrorContact.HighLevelSensors, value);
				this.parHandling.WriteValue(Definition.BeltStopOnInOutErrorEnablePath, (value && this.stopConveyorIfError) ? "1" : "0", false);
				this.OnPropertyChanged("HighLevelSensorsEnabled");
			}
		}

		// Token: 0x170007B6 RID: 1974
		// (get) Token: 0x060019E3 RID: 6627 RVA: 0x00097E24 File Offset: 0x00096024
		// (set) Token: 0x060019E4 RID: 6628 RVA: 0x00097E3C File Offset: 0x0009603C
		public bool TooManyRejectsEnabled
		{
			get
			{
				return this.tooManyRejectsEnabled;
			}
			set
			{
				this.tooManyRejectsEnabled = value;
				this.dbConnection.SetErrorContact(Definition.ErrorContact.TooManyRejects, value);
				this.OnPropertyChanged("TooManyRejectsEnabled");
			}
		}

		// Token: 0x170007B7 RID: 1975
		// (get) Token: 0x060019E5 RID: 6629 RVA: 0x00097E64 File Offset: 0x00096064
		// (set) Token: 0x060019E6 RID: 6630 RVA: 0x00097E7C File Offset: 0x0009607C
		public bool InsightNetConnectionErrorEnabled
		{
			get
			{
				return this.insightNetConnectionErrorEnabled;
			}
			set
			{
				this.insightNetConnectionErrorEnabled = value;
				this.dbConnection.SetErrorContact(Definition.ErrorContact.InsightNetConnection, value);
				this.OnPropertyChanged("InsightNetConnectionErrorEnabled");
			}
		}

		// Token: 0x170007B8 RID: 1976
		// (get) Token: 0x060019E7 RID: 6631 RVA: 0x00097EA4 File Offset: 0x000960A4
		// (set) Token: 0x060019E8 RID: 6632 RVA: 0x00097EBC File Offset: 0x000960BC
		public bool DiskSpaceLowErrorEnabled
		{
			get
			{
				return this.diskSpaceLowErrorEnabled;
			}
			set
			{
				this.diskSpaceLowErrorEnabled = value;
				this.dbConnection.SetErrorContact(Definition.ErrorContact.DiskSpaceLow, value);
				this.OnPropertyChanged("DiskSpaceLowErrorEnabled");
			}
		}

		// Token: 0x170007B9 RID: 1977
		// (get) Token: 0x060019E9 RID: 6633 RVA: 0x00097EE4 File Offset: 0x000960E4
		// (set) Token: 0x060019EA RID: 6634 RVA: 0x00097EFC File Offset: 0x000960FC
		public bool MessageLevelSafeSwiVisible
		{
			get
			{
				return this.messageLevelSafeSwiVisible;
			}
			set
			{
				bool flag = this.messageLevelSafeSwiVisible != value;
				if (flag)
				{
					this.messageLevelSafeSwiVisible = value;
					this.OnPropertyChanged("MessageLevelSafeSwiVisible");
				}
			}
		}

		// Token: 0x170007BA RID: 1978
		// (get) Token: 0x060019EB RID: 6635 RVA: 0x00097F2F File Offset: 0x0009612F
		// (set) Token: 0x060019EC RID: 6636 RVA: 0x00097F37 File Offset: 0x00096137
		public List<string> ListMessLevSafeSwitch { get; set; }

		// Token: 0x170007BB RID: 1979
		// (get) Token: 0x060019ED RID: 6637 RVA: 0x00097F40 File Offset: 0x00096140
		// (set) Token: 0x060019EE RID: 6638 RVA: 0x00097F58 File Offset: 0x00096158
		public string MessageLevelSafetySwitch
		{
			get
			{
				return this.messageLevelSafetySwitch;
			}
			set
			{
				bool flag = this.messageLevelSafetySwitch != value;
				if (flag)
				{
					this.messageLevelSafetySwitch = value;
					bool flag2 = value == this.StrInfoSafetySwitch;
					if (flag2)
					{
						this.dbConnection.SetSafetySwitchMessageAsError(false);
					}
					else
					{
						bool flag3 = value == this.StrErrorSafetySwitch;
						if (flag3)
						{
							this.dbConnection.SetSafetySwitchMessageAsError(true);
						}
					}
					this.OnPropertyChanged("MessageLevelSafetySwitch");
				}
			}
		}

		// Token: 0x170007BC RID: 1980
		// (get) Token: 0x060019EF RID: 6639 RVA: 0x00097FCC File Offset: 0x000961CC
		// (set) Token: 0x060019F0 RID: 6640 RVA: 0x00097FE4 File Offset: 0x000961E4
		public string MessageLevelTestcardSimMonitoring
		{
			get
			{
				return this.messageLevelTestcardSimMonitoring;
			}
			set
			{
				bool flag = this.messageLevelTestcardSimMonitoring != value;
				if (flag)
				{
					this.messageLevelTestcardSimMonitoring = value;
					bool flag2 = value == this.StrInfoTestcardSimMonitoring;
					if (flag2)
					{
						this.dbConnection.SetTestcardSimMonitoringMessLev(0);
					}
					else
					{
						bool flag3 = value == this.StrWarningTestcardSimMonitoring;
						if (flag3)
						{
							this.dbConnection.SetTestcardSimMonitoringMessLev(1);
						}
						else
						{
							bool flag4 = value == this.StrErrorTestcardSimMonitoring;
							if (flag4)
							{
								this.dbConnection.SetTestcardSimMonitoringMessLev(2);
							}
						}
					}
					this.OnPropertyChanged("MessageLevelTestcardSimMonitoring");
				}
			}
		}

		// Token: 0x170007BD RID: 1981
		// (get) Token: 0x060019F1 RID: 6641 RVA: 0x00098078 File Offset: 0x00096278
		// (set) Token: 0x060019F2 RID: 6642 RVA: 0x00098090 File Offset: 0x00096290
		public bool StopConveyorIfError
		{
			get
			{
				return this.stopConveyorIfError;
			}
			set
			{
				bool flag = this.stopConveyorIfError != value;
				if (flag)
				{
					this.stopConveyorIfError = value;
					this.OnPropertyChanged("StopConveyorIfError");
					this.parHandling.WriteValue(Definition.noBeltStartOnErrorPath, value ? "1" : "0");
					this.parHandling.WriteValue(Definition.beltStopOnErrorEnablePath, value ? "1" : "0");
					this.parHandling.WriteValue(Definition.beltStopOnVerifyErrorEnablePath, (value && this.missingObjectsEnabled) ? "1" : "0", false);
					this.parHandling.WriteValue(Definition.beltStopOnBinFullErrorEnablePath, (value && this.binFullEnabled) ? "1" : "0", false);
					this.parHandling.WriteValue(Definition.BeltStopOnInOutErrorEnablePath, (value && this.highLevelSensorsEnabled) ? "1" : "0", false);
					this.parHandling.WriteValue(Definition.BeltStopOnBinMissingEnablePath, (value && this.binMissingEnabled) ? "1" : "0", false);
					bool isService = this.IsService;
					if (isService)
					{
						bool flag2 = this.machinetype != MachineType.RayconEX2A;
						if (flag2)
						{
							this.MessageLevelSafeSwiVisible = this.stopConveyorIfError;
						}
						bool flag3 = !this.stopConveyorIfError;
						if (flag3)
						{
							this.MessageLevelSafetySwitch = this.StrInfoSafetySwitch;
						}
					}
				}
			}
		}

		// Token: 0x060019F3 RID: 6643 RVA: 0x000981ED File Offset: 0x000963ED
		public ErrorHandling(DBConnection dbConnection, ParameterHandling parHandling)
		{
			this.dbConnection = dbConnection;
			this.parHandling = parHandling;
			this.UpdateFromDatabase();
			dbConnection.GetMachineType(ref this.machinetype);
		}

		// Token: 0x060019F4 RID: 6644 RVA: 0x0009821C File Offset: 0x0009641C
		public void UpdateFromDatabase()
		{
			this.dbConnection.GetErrorContact(Definition.ErrorContact.AirPressure, ref this.lowAirPressureErrorEnabled);
			this.dbConnection.GetErrorContact(Definition.ErrorContact.Cooling, ref this.coolingErrorEnabled);
			this.dbConnection.GetErrorContact(Definition.ErrorContact.Motor, ref this.engineErrorEnabled);
			this.dbConnection.GetErrorContact(Definition.ErrorContact.Temperature, ref this.temperatureErrorEnabled);
			this.dbConnection.GetErrorContact(Definition.ErrorContact.Voltage, ref this.voltageErrorEnabled);
			this.dbConnection.GetErrorContact(Definition.ErrorContact.RejectVerify, ref this.missingObjectsEnabled);
			this.dbConnection.GetErrorContact(Definition.ErrorContact.BinFull, ref this.binFullEnabled);
			this.dbConnection.GetErrorContact(Definition.ErrorContact.XRaySource, ref this.xrayErrorEnabled);
			this.dbConnection.GetErrorContact(Definition.ErrorContact.Detector, ref this.detectorErrorEnabled);
			this.dbConnection.GetErrorContact(Definition.ErrorContact.XRayLamp, ref this.xrayLampErrorEnabled);
			this.dbConnection.GetErrorContact(Definition.ErrorContact.BinMissing, ref this.binMissingEnabled);
			this.dbConnection.GetErrorContact(Definition.ErrorContact.HighLevelSensors, ref this.highLevelSensorsEnabled);
			this.dbConnection.GetErrorContact(Definition.ErrorContact.TooManyRejects, ref this.tooManyRejectsEnabled);
			this.dbConnection.GetErrorContact(Definition.ErrorContact.InsightNetConnection, ref this.insightNetConnectionErrorEnabled);
			this.dbConnection.GetErrorContact(Definition.ErrorContact.DiskSpaceLow, ref this.diskSpaceLowErrorEnabled);
			this.dbConnection.GetStopConveyorIfError(ref this.stopConveyorIfError);
		}

		// Token: 0x060019F5 RID: 6645 RVA: 0x00098360 File Offset: 0x00096560
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x040010DD RID: 4317
		private bool coolingErrorEnabled;

		// Token: 0x040010DE RID: 4318
		private bool lowAirPressureErrorEnabled;

		// Token: 0x040010DF RID: 4319
		private bool engineErrorEnabled;

		// Token: 0x040010E0 RID: 4320
		private bool temperatureErrorEnabled;

		// Token: 0x040010E1 RID: 4321
		private bool voltageErrorEnabled;

		// Token: 0x040010E2 RID: 4322
		private bool missingObjectsEnabled;

		// Token: 0x040010E3 RID: 4323
		private bool binFullEnabled;

		// Token: 0x040010E4 RID: 4324
		private bool xrayErrorEnabled;

		// Token: 0x040010E5 RID: 4325
		private bool detectorErrorEnabled;

		// Token: 0x040010E6 RID: 4326
		private bool xrayLampErrorEnabled;

		// Token: 0x040010E7 RID: 4327
		private bool binMissingEnabled;

		// Token: 0x040010E8 RID: 4328
		private bool highLevelSensorsEnabled;

		// Token: 0x040010E9 RID: 4329
		private bool tooManyRejectsEnabled;

		// Token: 0x040010EA RID: 4330
		private bool insightNetConnectionErrorEnabled;

		// Token: 0x040010EB RID: 4331
		private bool diskSpaceLowErrorEnabled;

		// Token: 0x040010EC RID: 4332
		private DBConnection dbConnection;

		// Token: 0x040010ED RID: 4333
		private ParameterHandling parHandling;

		// Token: 0x040010EE RID: 4334
		private bool stopConveyorIfError;

		// Token: 0x040010EF RID: 4335
		private bool messageLevelSafeSwiVisible;

		// Token: 0x040010F0 RID: 4336
		public string StrInfoSafetySwitch;

		// Token: 0x040010F1 RID: 4337
		public bool IsService;

		// Token: 0x040010F2 RID: 4338
		public string StrErrorSafetySwitch;

		// Token: 0x040010F3 RID: 4339
		public string StrInfoTestcardSimMonitoring;

		// Token: 0x040010F4 RID: 4340
		public string StrWarningTestcardSimMonitoring;

		// Token: 0x040010F5 RID: 4341
		public string StrErrorTestcardSimMonitoring;

		// Token: 0x040010F6 RID: 4342
		private MachineType machinetype;

		// Token: 0x040010F9 RID: 4345
		private string messageLevelSafetySwitch;

		// Token: 0x040010FA RID: 4346
		private string messageLevelTestcardSimMonitoring;
	}
}
