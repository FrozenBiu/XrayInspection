using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using DatabaseConnection;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x02000199 RID: 409
	public class Conveyor : INotifyPropertyChanged
	{
		// Token: 0x140000D1 RID: 209
		// (add) Token: 0x060026C2 RID: 9922 RVA: 0x0010751C File Offset: 0x0010571C
		// (remove) Token: 0x060026C3 RID: 9923 RVA: 0x00107554 File Offset: 0x00105754
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x17000AFF RID: 2815
		// (get) Token: 0x060026C4 RID: 9924 RVA: 0x0010758C File Offset: 0x0010578C
		// (set) Token: 0x060026C5 RID: 9925 RVA: 0x001075A4 File Offset: 0x001057A4
		public double Speed
		{
			get
			{
				return this.speed;
			}
			set
			{
				bool flag = Math.Abs(value - this.speed) > 0.005 || this.updateSpeed;
				if (flag)
				{
					this.speedChanged = true;
					double num = this.GetMaxSpeed();
					double num2 = this.ratedSpeed * 0.4;
					MachineType machineType = MachineType.MachineTypeUndefined;
					this.dbConnection.GetMachineType(ref machineType);
					MachineMode machineMode = MachineMode.Standard;
					this.dbConnection.GetMachineMode(ref machineMode);
					bool flag2 = false;
					this.dbConnection.GetBulkSpeedBelow08(ref flag2);
					bool flag3 = (machineMode == MachineMode.Bulk || machineMode == MachineMode.BulkObject) && !flag2;
					if (flag3)
					{
						num2 = 0.8;
					}
					else
					{
						bool flag4 = machineType == MachineType.RayconEX2A;
						if (flag4)
						{
							num2 = 0.32;
							num = 1.4;
						}
					}
					double oldspeed = this.speed;
					bool flag5 = value < num2;
					if (flag5)
					{
						this.speed = num2;
					}
					else
					{
						bool flag6 = value > num;
						if (flag6)
						{
							this.speed = num;
						}
						else
						{
							this.speed = value;
						}
					}
					this.OnPropertyChanged("Speed");
					this.UpdateFCVoltage(oldspeed);
					this.dbConnection.UpdateCurrentProductTable("ConveyorSpeed", this.speed.ToString(this.culture));
					this.parHandling.WriteValue(Definition.conveyingSpeedPath, ((int)(this.speed * 1000.0)).ToString());
				}
			}
		}

		// Token: 0x060026C6 RID: 9926 RVA: 0x00107708 File Offset: 0x00105908
		public void UpdateSpeed()
		{
			this.updateSpeed = true;
			this.Speed = this.Speed;
			this.updateSpeed = false;
		}

		// Token: 0x17000B00 RID: 2816
		// (get) Token: 0x060026C7 RID: 9927 RVA: 0x00107728 File Offset: 0x00105928
		// (set) Token: 0x060026C8 RID: 9928 RVA: 0x00107740 File Offset: 0x00105940
		public double RatedSpeed
		{
			get
			{
				return this.ratedSpeed;
			}
			set
			{
				double oldspeed = this.ratedSpeed;
				this.ratedSpeed = value;
				this.OnPropertyChanged("RatedSpeed");
				string ratedConveyorSpeed = this.ratedSpeed.ToString(this.culture);
				this.dbConnection.SetRatedConveyorSpeed(ratedConveyorSpeed);
				this.UpdateFCVoltage(oldspeed);
				this.speedChanged = true;
			}
		}

		// Token: 0x17000B01 RID: 2817
		// (get) Token: 0x060026C9 RID: 9929 RVA: 0x00107798 File Offset: 0x00105998
		// (set) Token: 0x060026CA RID: 9930 RVA: 0x001077B0 File Offset: 0x001059B0
		public bool InManualMode
		{
			get
			{
				return this.inManualMode;
			}
			set
			{
				bool flag = this.inManualMode != value;
				if (flag)
				{
					this.parHandling.WriteValue(Definition.conveyorInManualModePath, value ? "1" : "0", false);
					this.inManualMode = value;
					this.OnPropertyChanged("InManualMode");
				}
			}
		}

		// Token: 0x17000B02 RID: 2818
		// (get) Token: 0x060026CB RID: 9931 RVA: 0x00107804 File Offset: 0x00105A04
		public int AdaptedIntegrationTime
		{
			get
			{
				return this.adaptedIntegrationTime;
			}
		}

		// Token: 0x060026CC RID: 9932 RVA: 0x0010781C File Offset: 0x00105A1C
		public Conveyor(DBConnection dbConnection, ParameterHandling parHandling, XrayState xrayState)
		{
			this.dbConnection = dbConnection;
			this.parHandling = parHandling;
			this.xrayState = xrayState;
			string s = "";
			dbConnection.GetRatedConveyorSpeed(ref s);
			double.TryParse(s, NumberStyles.AllowDecimalPoint, this.culture, out this.ratedSpeed);
			this.speedChanged = true;
		}

		// Token: 0x060026CD RID: 9933 RVA: 0x00107890 File Offset: 0x00105A90
		public bool AdaptScanrate()
		{
			return this.AdaptScanrate(true, "");
		}

		// Token: 0x060026CE RID: 9934 RVA: 0x001078B0 File Offset: 0x00105AB0
		public bool AdaptScanrate(bool onlyIfSpeedChanged, string speedexternal)
		{
			bool result = false;
			bool flag = this.speedChanged || !onlyIfSpeedChanged || Conveyor.DeformationFactorChanged;
			if (flag)
			{
				DetectorType detectorType = DetectorType.DT;
				int num = this.ComputeScanRate(speedexternal);
				bool flag2 = this.parHandling.ReadPar(Definition.detectorMonitoringActive) == "1";
				if (flag2)
				{
					this.parHandling.WriteValue(Definition.detectorMonitoringBackgroundEnable, "0");
				}
				this.dbConnection.GetDetectorType(ref detectorType);
				bool flag3 = detectorType == DetectorType.DT || detectorType == DetectorType.DTNetSE || detectorType == DetectorType.DTNetDE;
				if (flag3)
				{
					bool flag4 = num < 175;
					if (flag4)
					{
						this.parHandling.WriteValue(Conveyor.detectorDTAverageFilterSizePath, "2", false);
						num *= 4;
					}
					else
					{
						bool flag5 = num < 450;
						if (flag5)
						{
							this.parHandling.WriteValue(Conveyor.detectorDTAverageFilterSizePath, "1", false);
							num *= 2;
						}
						else
						{
							this.parHandling.WriteValue(Conveyor.detectorDTAverageFilterSizePath, "0", false);
						}
					}
					this.parHandling.WriteValue(XRayOnProcessing.dtGainEnablePath, "0", false);
				}
				else
				{
					bool flag6 = num < 175;
					if (flag6)
					{
						this.parHandling.WriteValue(Conveyor.detectorSesotecAveragedLinesSizePath, "4", false);
						num *= 4;
					}
					else
					{
						bool flag7 = num < 450;
						if (flag7)
						{
							this.parHandling.WriteValue(Conveyor.detectorSesotecAveragedLinesSizePath, "2", false);
							num *= 2;
						}
						else
						{
							this.parHandling.WriteValue(Conveyor.detectorSesotecAveragedLinesSizePath, "1", false);
						}
					}
				}
				string path = (detectorType == DetectorType.DT || detectorType == DetectorType.DTNetSE || detectorType == DetectorType.DTNetDE || detectorType == DetectorType.DTNetTDI) ? Definition.detectorDTScanratePath : Definition.detectorSesotecScanratePath;
				bool flag8 = detectorType == DetectorType.DT && num > 2000;
				if (flag8)
				{
					bool flag9 = false;
					this.dbConnection.GetDTScanrateLimitEnable(ref flag9);
					bool flag10 = flag9;
					if (flag10)
					{
						num = 2000;
					}
				}
				string text = this.parHandling.ReadPar(path);
				bool flag11 = text != num.ToString();
				if (flag11)
				{
					bool flag12 = detectorType == DetectorType.DTNetTDI;
					if (flag12)
					{
						int num2 = 0;
						int.TryParse(text, out num2);
						bool flag13 = Math.Abs(num2 - num) > 10;
						if (flag13)
						{
							XRayOnProcessing.DisableXray(this.parHandling, this.dbConnection, true);
							Thread.Sleep(200);
							this.adaptedIntegrationTime = (int)(1.0 / (double)num * 1000000.0);
							this.parHandling.WriteValue(path, num.ToString());
							result = true;
						}
					}
					else
					{
						XRayOnProcessing.DisableXray(this.parHandling, this.dbConnection, true);
						Thread.Sleep(200);
						this.parHandling.WriteValue(path, num.ToString());
						result = true;
					}
				}
			}
			this.speedChanged = false;
			Conveyor.DeformationFactorChanged = false;
			return result;
		}

		// Token: 0x060026CF RID: 9935 RVA: 0x00107BAC File Offset: 0x00105DAC
		private int ComputeScanRate(string speedexternal)
		{
			string s = null;
			double num = 0.0;
			string s2 = null;
			double num2 = 1.0;
			uint num3 = 0U;
			DetectorType detectorType = DetectorType.DT;
			DetectorSubType detectorSubType = DetectorSubType.None;
			MachineType machineType = MachineType.RayconEX2A;
			this.dbConnection.GetDetectorType(ref detectorType);
			string a = this.parHandling.ReadPar(Definition.detectorSubtypePath);
			bool flag = a == "1";
			if (flag)
			{
				detectorSubType = DetectorSubType.SesotecHS;
			}
			this.dbConnection.GetDetectorSesotecHSEnableMaxScanrate(ref num3);
			this.dbConnection.GetGeometryFactor(ref s);
			this.dbConnection.GetParameterValue(Conveyor.deformationFactorPath, ref s2);
			this.dbConnection.GetMachineType(ref machineType);
			double.TryParse(s2, NumberStyles.Any, this.culture, out num2);
			double.TryParse(s, NumberStyles.Any, this.culture, out num);
			bool flag2 = num2 == 0.0;
			if (flag2)
			{
				num2 = 1.0;
			}
			bool flag3 = speedexternal != "" && speedexternal != null;
			double num4;
			if (flag3)
			{
				num4 = Convert.ToDouble(speedexternal);
			}
			else
			{
				num4 = this.speed;
			}
			bool flag4 = num > 0.0;
			double num5;
			if (flag4)
			{
				num5 = num4 * 1000.0 / num * num2;
			}
			else
			{
				num5 = num4 * 1000.0 / 0.56 * num2;
			}
			bool flag5 = machineType == MachineType.RayconEX2A;
			if (flag5)
			{
				num5 *= Conveyor.scanfactorEX2A;
			}
			bool flag6 = detectorType == DetectorType.DTNetSE && this.parHandling.ReadPar(Conveyor.detectorDTPixelSizePath) == "0.2";
			if (flag6)
			{
				num5 *= 4.0;
			}
			bool flag7 = (detectorType == DetectorType.DT || detectorType == DetectorType.DTNetSE || detectorType == DetectorType.DTNetDE || detectorType == DetectorType.DTNetTDI) && this.parHandling.ReadPar(Conveyor.detectorDTPixelSizePath) == "0.4";
			if (flag7)
			{
				num5 *= 2.0;
			}
			bool flag8 = detectorType == DetectorType.Sesotec;
			if (flag8)
			{
				bool flag9 = this.parHandling.ReadPar(Conveyor.detectorSesotecPixelSizePath) == "0.4";
				if (flag9)
				{
					num5 *= 2.0;
				}
				bool flag10 = detectorSubType == DetectorSubType.SesotecHS;
				if (flag10)
				{
					bool flag11 = num3 == 1U;
					if (flag11)
					{
						bool flag12 = num5 > 3570.0;
						if (flag12)
						{
							num5 = 3570.0;
						}
					}
					else
					{
						bool flag13 = num3 == 0U;
						if (flag13)
						{
							bool flag14 = num5 > 2300.0;
							if (flag14)
							{
								num5 = 2300.0;
							}
						}
					}
				}
				else
				{
					bool flag15 = num5 > 2300.0;
					if (flag15)
					{
						num5 = 2300.0;
					}
				}
			}
			return (int)num5;
		}

		// Token: 0x060026D0 RID: 9936 RVA: 0x00107E60 File Offset: 0x00106060
		private double ComputeVoltage(double speedTemp)
		{
			double num = 0.0;
			this.dbConnection.GetFCVoltageFactor(ref num);
			return speedTemp * 10.0 / (this.ratedSpeed * (1.3 + num * this.ratedSpeed));
		}

		// Token: 0x060026D1 RID: 9937 RVA: 0x00107EBC File Offset: 0x001060BC
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x060026D2 RID: 9938 RVA: 0x00107EEC File Offset: 0x001060EC
		public void UpdateFCVoltage(double oldspeed)
		{
			MachineMode machineMode = MachineMode.Standard;
			this.dbConnection.GetMachineMode(ref machineMode);
			MachineType machineType = MachineType.MachineTypeUndefined;
			bool flag = this.dbConnection != null;
			if (flag)
			{
				this.dbConnection.GetMachineType(ref machineType);
			}
			double num = this.ComputeVoltage(this.speed);
			bool flag2 = machineType == MachineType.RayconEX2A && oldspeed > -1.0 && this.speed - oldspeed > 0.2;
			if (flag2)
			{
				double num2 = this.ComputeVoltage(oldspeed);
				num2 += (num - num2) / 2.0;
				bool flag3 = num2 > 0.0;
				if (flag3)
				{
					this.parHandling.WriteValue("/hardware/ios/AO_00/Value", num2.ToString(this.culture), false);
					Thread.Sleep(500);
				}
				this.parHandling.WriteValue("/hardware/ios/AO_00/Value", num.ToString(this.culture), false);
			}
			else
			{
				this.parHandling.WriteValue("/hardware/ios/AO_00/Value", num.ToString(this.culture), false);
			}
			bool flag4 = machineMode != MachineMode.Bulk;
			if (flag4)
			{
				int num3 = 0;
				this.parHandling.WriteValue("/hardware/ios/AO_01/Value", num.ToString(this.culture), false);
				string s = this.parHandling.ReadPar(Definition.ioVersionPath);
				bool flag5 = int.TryParse(s, out num3) && num3 >= Definition.minSesotecIOProgramVersion;
				if (flag5)
				{
					this.parHandling.WriteValue("/hardware/ios/AO_02/Value", num.ToString(this.culture), false);
					this.parHandling.WriteValue("/hardware/ios/AO_03/Value", num.ToString(this.culture), false);
				}
			}
		}

		// Token: 0x060026D3 RID: 9939 RVA: 0x001080AC File Offset: 0x001062AC
		public void StartConveyor()
		{
			MachineType machineType = MachineType.MachineTypeUndefined;
			bool flag = this.dbConnection != null;
			if (flag)
			{
				this.dbConnection.GetMachineType(ref machineType);
			}
			bool flag2 = machineType == MachineType.RayconEX2A;
			if (flag2)
			{
				this.startEx2ConveyorWithRamp();
			}
			else
			{
				this.parHandling.WriteValue(Conveyor.startPath, "1", false);
			}
		}

		// Token: 0x060026D4 RID: 9940 RVA: 0x00108104 File Offset: 0x00106304
		public void StopConveyor()
		{
			MachineMode machineMode = MachineMode.Standard;
			this.dbConnection.GetMachineMode(ref machineMode);
			bool flag = machineMode == MachineMode.Bulk;
			if (flag)
			{
				this.parHandling.WriteValue("/filter/acquisition/SendImage/Value", "0");
				this.parHandling.WriteValue("/filter/acquisition/SendOnlyIfContaminant/Value", "0");
			}
			this.parHandling.WriteValue(Conveyor.stopPath, "1", false);
		}

		// Token: 0x060026D5 RID: 9941 RVA: 0x00108170 File Offset: 0x00106370
		private double GetMaxSpeed()
		{
			MachineType machineType = MachineType.MachineTypeUndefined;
			bool flag = this.dbConnection != null;
			if (flag)
			{
				this.dbConnection.GetMachineType(ref machineType);
			}
			double num = 0.0;
			bool flag2 = !double.TryParse(this.parHandling.ReadPar(Definition.conveyingSpeedMaxDCPath), NumberStyles.Float, new CultureInfo("en-US"), out num);
			if (flag2)
			{
				num = 1.04;
			}
			return (machineType != MachineType.RayconEntry) ? (this.ratedSpeed * 1.3) : num;
		}

		// Token: 0x060026D6 RID: 9942 RVA: 0x001081FC File Offset: 0x001063FC
		private void startEx2ConveyorWithRamp()
		{
			double num = this.ComputeVoltage(this.speed);
			string a = this.parHandling.ReadPar(Definition.conveyorIsRunning);
			bool flag = num > 0.0 && a == "0";
			if (flag)
			{
				this.parHandling.WriteValue("/hardware/ios/AO_00/Value", (num / 2.0).ToString(this.culture), false);
				Thread.Sleep(100);
				this.parHandling.WriteValue(Conveyor.startPath, "1", false);
				Thread.Sleep(500);
			}
			this.parHandling.WriteValue("/hardware/ios/AO_00/Value", num.ToString(this.culture), false);
		}

		// Token: 0x040018D9 RID: 6361
		public static readonly string startPath = "/hardware/ios/ConveyorStartByGUI/Value";

		// Token: 0x040018DA RID: 6362
		public static readonly string stopPath = "/hardware/ios/ConveyorStopByGUI/Value";

		// Token: 0x040018DB RID: 6363
		private const string fuVoltageOutputPath = "/hardware/ios/AO_00/Value";

		// Token: 0x040018DC RID: 6364
		private const string fuVoltageSecondOutputPath = "/hardware/ios/AO_01/Value";

		// Token: 0x040018DD RID: 6365
		private const string fuVoltageThirdOutputPath = "/hardware/ios/AO_02/Value";

		// Token: 0x040018DE RID: 6366
		private const string fuVoltageFourthOutputPath = "/hardware/ios/AO_03/Value";

		// Token: 0x040018DF RID: 6367
		private const string vibroSpeedPath = "/hardware/ios/AO_01/Value";

		// Token: 0x040018E0 RID: 6368
		public static readonly string detectorDTAverageFilterSizePath = "/hardware/detector/dt/AverageFilterSize/Value";

		// Token: 0x040018E1 RID: 6369
		public static readonly string conveyingDirectionPath = "/application/ConveyingDirection/Value";

		// Token: 0x040018E2 RID: 6370
		public static readonly string geometryFactorPath = "/machine/GeometryFactor/Value";

		// Token: 0x040018E3 RID: 6371
		public static readonly string deformationFactorPath = "/machine/DeformationFactor/Value";

		// Token: 0x040018E4 RID: 6372
		public static readonly string detectorDTPixelSizePath = "/hardware/detector/dt/PixelSize/Value";

		// Token: 0x040018E5 RID: 6373
		public static readonly string detectorDT04Identifier = "40";

		// Token: 0x040018E6 RID: 6374
		public static readonly string detectorDT02Identifier = "20";

		// Token: 0x040018E7 RID: 6375
		private double ratedSpeed = 0.4;

		// Token: 0x040018E8 RID: 6376
		private CultureInfo culture = new CultureInfo("en-US");

		// Token: 0x040018E9 RID: 6377
		public static readonly string detectorSesotecAveragedLinesSizePath = "/hardware/detector/sesotec/AveragedLines/Value";

		// Token: 0x040018EA RID: 6378
		public static readonly string detectorSesotecPixelSizePath = "/hardware/detector/sesotec/PixelSize/Value";

		// Token: 0x040018EB RID: 6379
		private const int detectorSesotecMaxScanrate = 2300;

		// Token: 0x040018EC RID: 6380
		private const int detectorSesotecHSMaxScanrate = 3570;

		// Token: 0x040018ED RID: 6381
		private double speed;

		// Token: 0x040018EE RID: 6382
		private DBConnection dbConnection;

		// Token: 0x040018EF RID: 6383
		private ParameterHandling parHandling;

		// Token: 0x040018F0 RID: 6384
		private bool speedChanged;

		// Token: 0x040018F1 RID: 6385
		public static bool DeformationFactorChanged;

		// Token: 0x040018F2 RID: 6386
		private bool inManualMode;

		// Token: 0x040018F3 RID: 6387
		private XrayState xrayState;

		// Token: 0x040018F4 RID: 6388
		private int adaptedIntegrationTime;

		// Token: 0x040018F5 RID: 6389
		public static double scanfactorEX2A = 0.85;

		// Token: 0x040018F7 RID: 6391
		private bool updateSpeed;
	}
}
