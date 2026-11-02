using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using DatabaseConnection;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x02000048 RID: 72
	public class FlapsOverIOs : INotifyPropertyChanged
	{
		// Token: 0x14000029 RID: 41
		// (add) Token: 0x06000703 RID: 1795 RVA: 0x0002F608 File Offset: 0x0002D808
		// (remove) Token: 0x06000704 RID: 1796 RVA: 0x0002F640 File Offset: 0x0002D840
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x17000205 RID: 517
		// (get) Token: 0x06000705 RID: 1797 RVA: 0x0002F678 File Offset: 0x0002D878
		// (set) Token: 0x06000706 RID: 1798 RVA: 0x0002F690 File Offset: 0x0002D890
		public bool Enable
		{
			get
			{
				return this.enable;
			}
			set
			{
				bool flag = this.enable != value;
				if (flag)
				{
					this.enable = value;
					bool flag2 = this.initialized;
					if (flag2)
					{
						this.parHandling.WriteValue(FlapsOverIOs.bulkflapsEnableValvesPath, value ? "1" : "0");
						this.DisabledValves = !this.enable;
						this.OnPropertyChanged("Enable");
					}
				}
			}
		}

		// Token: 0x17000206 RID: 518
		// (get) Token: 0x06000707 RID: 1799 RVA: 0x0002F700 File Offset: 0x0002D900
		// (set) Token: 0x06000708 RID: 1800 RVA: 0x0002F718 File Offset: 0x0002D918
		public bool DisabledValves
		{
			get
			{
				return this.disabledValves;
			}
			set
			{
				bool flag = this.disabledValves != value;
				if (flag)
				{
					this.disabledValves = value;
					this.OnPropertyChanged("DisabledValves");
				}
			}
		}

		// Token: 0x17000207 RID: 519
		// (get) Token: 0x06000709 RID: 1801 RVA: 0x0002F74C File Offset: 0x0002D94C
		// (set) Token: 0x0600070A RID: 1802 RVA: 0x0002F764 File Offset: 0x0002D964
		public int Delay
		{
			get
			{
				return this.delay;
			}
			set
			{
				bool flag = value > 10000;
				if (flag)
				{
					value = 10000;
				}
				bool flag2 = this.delay != value;
				if (flag2)
				{
					this.delay = value;
					bool flag3 = this.initialized;
					if (flag3)
					{
						this.parHandling.WriteValue(FlapsOverIOs.bulkflapsDelayPath, value.ToString());
						this.OnPropertyChanged("Delay");
					}
				}
			}
		}

		// Token: 0x17000208 RID: 520
		// (get) Token: 0x0600070B RID: 1803 RVA: 0x0002F7D0 File Offset: 0x0002D9D0
		// (set) Token: 0x0600070C RID: 1804 RVA: 0x0002F7E8 File Offset: 0x0002D9E8
		public int InitEjectTime
		{
			get
			{
				return this.initEjectTime;
			}
			set
			{
				bool flag = this.initEjectTime != value;
				if (flag)
				{
					this.initEjectTime = value;
					this.OnPropertyChanged("InitEjectTime");
				}
			}
		}

		// Token: 0x17000209 RID: 521
		// (get) Token: 0x0600070D RID: 1805 RVA: 0x0002F81C File Offset: 0x0002DA1C
		// (set) Token: 0x0600070E RID: 1806 RVA: 0x0002F834 File Offset: 0x0002DA34
		public int EjectTime
		{
			get
			{
				return this.ejectTime;
			}
			set
			{
				bool flag = value > 10000;
				if (flag)
				{
					value = 10000;
				}
				bool flag2 = this.ejectTime != value;
				if (flag2)
				{
					this.ejectTime = value;
					bool flag3 = this.initialized;
					if (flag3)
					{
						this.parHandling.WriteValue(FlapsOverIOs.bulkflapsEjectTimePath, value.ToString());
						this.OnPropertyChanged("EjectTime");
					}
				}
			}
		}

		// Token: 0x1700020A RID: 522
		// (get) Token: 0x0600070F RID: 1807 RVA: 0x0002F8A0 File Offset: 0x0002DAA0
		// (set) Token: 0x06000710 RID: 1808 RVA: 0x0002F8B8 File Offset: 0x0002DAB8
		public int ImageSize
		{
			get
			{
				return this.imageSize;
			}
			set
			{
				double num = 0.0;
				double.TryParse(this.parHandling.ReadPar(Definition.conveyingSpeedPath), NumberStyles.Any, this.culture, out num);
				bool flag = value > 1000;
				if (flag)
				{
					value = 1000;
				}
				bool flag2 = this.parHandling.ReadPar(Definition.detectorTypePath) == "1" && this.parHandling.ReadPar(Definition.detectorSubtypePath) == "1";
				if (flag2)
				{
					bool flag3 = num > 900.0;
					if (flag3)
					{
						bool flag4 = value < 80;
						if (flag4)
						{
							value = 80;
						}
					}
					else
					{
						bool flag5 = num > 800.0;
						if (flag5)
						{
							bool flag6 = value < 70;
							if (flag6)
							{
								value = 70;
							}
						}
						else
						{
							bool flag7 = num > 700.0;
							if (flag7)
							{
								bool flag8 = value < 60;
								if (flag8)
								{
									value = 60;
								}
							}
							else
							{
								bool flag9 = value < 50;
								if (flag9)
								{
									value = 50;
								}
							}
						}
					}
				}
				else
				{
					bool flag10 = value < 50;
					if (flag10)
					{
						value = 50;
					}
				}
				bool flag11 = this.imageSize != value;
				if (flag11)
				{
					this.imageSize = value;
					this.parHandling.WriteValue(Acquisition.nMaxLinesPath, value.ToString());
					BulkMode.UpdateBulkOverlappingInspection(this.parHandling);
					this.calcInitEjectTime();
					this.OnPropertyChanged("ImageSize");
				}
			}
		}

		// Token: 0x1700020B RID: 523
		// (get) Token: 0x06000711 RID: 1809 RVA: 0x0002FA2C File Offset: 0x0002DC2C
		// (set) Token: 0x06000712 RID: 1810 RVA: 0x0002FA44 File Offset: 0x0002DC44
		public bool ValveTestStart
		{
			get
			{
				return this.valveTestStart;
			}
			set
			{
				bool flag = this.valveTestStart != value;
				if (flag)
				{
					this.valveTestStart = value;
					bool flag2 = this.initialized;
					if (flag2)
					{
						this.parHandling.WriteValue(FlapsOverIOs.bulkflapsValveTestStartPath, value ? "1" : "0");
						this.OnPropertyChanged("ValveTestStart");
					}
				}
			}
		}

		// Token: 0x1700020C RID: 524
		// (get) Token: 0x06000713 RID: 1811 RVA: 0x0002FAA4 File Offset: 0x0002DCA4
		// (set) Token: 0x06000714 RID: 1812 RVA: 0x0002FABC File Offset: 0x0002DCBC
		public int ValveTestInterval
		{
			get
			{
				return this.valvetestInterval;
			}
			set
			{
				bool flag = value > 10000;
				if (flag)
				{
					value = 10000;
				}
				bool flag2 = value < 200;
				if (flag2)
				{
					value = 200;
				}
				bool flag3 = this.valvetestInterval != value;
				if (flag3)
				{
					this.valvetestInterval = value;
					bool flag4 = this.initialized;
					if (flag4)
					{
						this.parHandling.WriteValue(FlapsOverIOs.bulkflapsValveTestIntervalPath, value.ToString());
						this.OnPropertyChanged("ValveTestInterval");
					}
				}
			}
		}

		// Token: 0x1700020D RID: 525
		// (get) Token: 0x06000715 RID: 1813 RVA: 0x0002FB3C File Offset: 0x0002DD3C
		// (set) Token: 0x06000716 RID: 1814 RVA: 0x0002FB54 File Offset: 0x0002DD54
		public bool ValveTestIsRunning
		{
			get
			{
				return this.valveTestIsRunning;
			}
			set
			{
				bool flag = this.valveTestIsRunning != value;
				if (flag)
				{
					this.valveTestIsRunning = value;
					bool flag2 = !this.valveTestIsRunning;
					if (flag2)
					{
						this.parHandling.WriteValue(FlapsOverIOs.bulkflapsValveTestIsRunningPath, "0");
					}
					this.OnPropertyChanged("ValveTestIsRunning");
				}
			}
		}

		// Token: 0x1700020E RID: 526
		// (get) Token: 0x06000717 RID: 1815 RVA: 0x0002FBAC File Offset: 0x0002DDAC
		// (set) Token: 0x06000718 RID: 1816 RVA: 0x0002FBC4 File Offset: 0x0002DDC4
		public bool RejectWithAllFlaps
		{
			get
			{
				return this.rejectWithAllFlaps;
			}
			set
			{
				bool flag = this.rejectWithAllFlaps != value;
				if (flag)
				{
					this.rejectWithAllFlaps = value;
					bool flag2 = this.initialized;
					if (flag2)
					{
						this.parHandling.WriteValue(FlapsOverIOs.bulkflapsRejectWithAllFlaps, value ? "1" : "0");
						this.OnPropertyChanged("RejectWithAllFlaps");
					}
				}
			}
		}

		// Token: 0x06000719 RID: 1817 RVA: 0x0002FC23 File Offset: 0x0002DE23
		public FlapsOverIOs(ParameterHandling parHandling, DBConnection dbConnection, MachineState machineState) : this(parHandling, dbConnection, machineState, true)
		{
		}

		// Token: 0x0600071A RID: 1818 RVA: 0x0002FC34 File Offset: 0x0002DE34
		public FlapsOverIOs(ParameterHandling parHandling, DBConnection dbConnection, MachineState machineState, bool showHazardMessage)
		{
			this.parHandling = parHandling;
			this.dbConnection = dbConnection;
			this.machineState = machineState;
			this.showHazardMessage = showHazardMessage;
			bool flag = this.parHandling != null && dbConnection.Connection != null;
			if (flag)
			{
				this.Enable = (parHandling.ReadPar(FlapsOverIOs.bulkflapsEnableValvesPath) == "1");
				this.DisabledValves = !this.enable;
				this.Delay = Convert.ToInt32(parHandling.ReadPar(FlapsOverIOs.bulkflapsDelayPath));
				this.EjectTime = Convert.ToInt32(parHandling.ReadPar(FlapsOverIOs.bulkflapsEjectTimePath));
				this.ValveTestStart = (parHandling.ReadPar(FlapsOverIOs.bulkflapsValveTestStartPath) == "1");
				this.ValveTestInterval = Convert.ToInt32(parHandling.ReadPar(FlapsOverIOs.bulkflapsValveTestIntervalPath));
				this.ImageSize = Convert.ToInt32(parHandling.ReadPar(Acquisition.nMaxLinesPath));
				this.RejectWithAllFlaps = (parHandling.ReadPar(FlapsOverIOs.bulkflapsRejectWithAllFlaps) == "1");
				this.calcInitEjectTime();
			}
			this.initialized = true;
		}

		// Token: 0x0600071B RID: 1819 RVA: 0x0002FD7C File Offset: 0x0002DF7C
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x0600071C RID: 1820 RVA: 0x0002FDAC File Offset: 0x0002DFAC
		private void calcInitEjectTime()
		{
			string s = "";
			double num = 0.0;
			double num2 = 0.0;
			double num3 = 0.0;
			double num4 = 0.0;
			double num5 = 1.0;
			this.dbConnection.GetGeometryFactor(ref s);
			double.TryParse(s, NumberStyles.Any, this.culture, out num);
			double.TryParse(this.parHandling.ReadPar(Acquisition.nMaxLinesPath), NumberStyles.Any, this.culture, out num2);
			double.TryParse(this.parHandling.ReadPar(Definition.conveyingSpeedPath), NumberStyles.Any, this.culture, out num3);
			double.TryParse(this.parHandling.ReadPar(Acquisition.binningPath), NumberStyles.Any, this.culture, out num4);
			bool flag = this.parHandling.ReadPar(Definition.detectorTypePath) == "1" && this.parHandling.ReadPar(Definition.detectorSesotecPixelSizePath) == "0.4";
			if (flag)
			{
				bool flag2 = this.parHandling.ReadPar(Definition.detectorSubtypePath) == "0";
				if (flag2)
				{
					bool flag3 = num3 > 650.0;
					if (flag3)
					{
						num3 = 650.0;
					}
				}
				else
				{
					bool flag4 = num3 > 1000.0;
					if (flag4)
					{
						num3 = 1000.0;
					}
				}
				num5 = 2.0;
			}
			this.InitEjectTime = 0;
			bool flag5 = num3 > 0.0;
			if (flag5)
			{
				this.InitEjectTime = (int)(num2 * num * 1000.0 / num3 * num4 / num5);
			}
		}

		// Token: 0x040004D7 RID: 1239
		private ParameterHandling parHandling;

		// Token: 0x040004D8 RID: 1240
		private DBConnection dbConnection;

		// Token: 0x040004D9 RID: 1241
		private MachineState machineState;

		// Token: 0x040004DB RID: 1243
		public static readonly string bulkflapsEnabledPath = "/hardware/rejecter/bulkflaps/Enabled/Value";

		// Token: 0x040004DC RID: 1244
		public static readonly string bulkflapsEnableValvesPath = "/hardware/rejecter/bulkflaps/EnableValves/Value";

		// Token: 0x040004DD RID: 1245
		public static readonly string bulkflapsDelayPath = "/hardware/rejecter/bulkflaps/DelayTime/Value";

		// Token: 0x040004DE RID: 1246
		public static readonly string bulkflapsEjectTimePath = "/hardware/rejecter/bulkflaps/EjectTime/Value";

		// Token: 0x040004DF RID: 1247
		public static readonly string bulkflapsBorderPix1Path = "/hardware/rejecter/bulkflaps/BorderPixel1/Value";

		// Token: 0x040004E0 RID: 1248
		public static readonly string bulkflapsBorderPix2Path = "/hardware/rejecter/bulkflaps/BorderPixel2/Value";

		// Token: 0x040004E1 RID: 1249
		public static readonly string bulkflapsBorderPix3Path = "/hardware/rejecter/bulkflaps/BorderPixel3/Value";

		// Token: 0x040004E2 RID: 1250
		public static readonly string bulkflapsFlapsCountPath = "/hardware/rejecter/bulkflaps/FlapsCount/Value";

		// Token: 0x040004E3 RID: 1251
		public static readonly string bulkflapsValveTestStartPath = "/hardware/rejecter/bulkflaps/ValveTestStart/Value";

		// Token: 0x040004E4 RID: 1252
		public static readonly string bulkflapsValveTestIntervalPath = "/hardware/rejecter/bulkflaps/ValveTestInterval/Value";

		// Token: 0x040004E5 RID: 1253
		public static readonly string bulkflapsValveTestIsRunningPath = "/hardware/rejecter/bulkflaps/ValveTestRun/Value";

		// Token: 0x040004E6 RID: 1254
		public static readonly string bulkflapsCalcFlapsMappingPath = "/hardware/rejecter/bulkflaps/CalcPixToFlapMapping/Value";

		// Token: 0x040004E7 RID: 1255
		public static readonly string bulkflapsValveActivityFlap1 = "/hardware/rejecter/bulkflaps/ValveActFlap1/Value";

		// Token: 0x040004E8 RID: 1256
		public static readonly string bulkflapsValveActivityFlap2 = "/hardware/rejecter/bulkflaps/ValveActFlap2/Value";

		// Token: 0x040004E9 RID: 1257
		public static readonly string bulkflapsValveActivityFlap3 = "/hardware/rejecter/bulkflaps/ValveActFlap3/Value";

		// Token: 0x040004EA RID: 1258
		public static readonly string bulkflapsValveActivityFlap4 = "/hardware/rejecter/bulkflaps/ValveActFlap4/Value";

		// Token: 0x040004EB RID: 1259
		public static readonly string bulkflapsValveActivityThroughput = "/hardware/rejecter/bulkflaps/ValveActThroughput/Value";

		// Token: 0x040004EC RID: 1260
		public static readonly string bulkflapsEnableValvesDOPath = "/hardware/ios/DO_31/Value";

		// Token: 0x040004ED RID: 1261
		public static readonly string bulkflapsManualSensitvity = "/hardware/rejecter/bulkflaps/ManualSensitivity/Value";

		// Token: 0x040004EE RID: 1262
		public static readonly string bulkflapsRejectWithAllFlaps = "/hardware/rejecter/bulkflaps/RejectWithAllFlaps/Value";

		// Token: 0x040004EF RID: 1263
		private const int maxEjectTime = 10000;

		// Token: 0x040004F0 RID: 1264
		private const int maxDelay = 10000;

		// Token: 0x040004F1 RID: 1265
		private const int minInterval = 200;

		// Token: 0x040004F2 RID: 1266
		private bool initialized = false;

		// Token: 0x040004F3 RID: 1267
		private bool showHazardMessage;

		// Token: 0x040004F4 RID: 1268
		private CultureInfo culture = new CultureInfo("en-US");

		// Token: 0x040004F5 RID: 1269
		private bool enable;

		// Token: 0x040004F6 RID: 1270
		private bool disabledValves;

		// Token: 0x040004F7 RID: 1271
		private int delay;

		// Token: 0x040004F8 RID: 1272
		private int initEjectTime;

		// Token: 0x040004F9 RID: 1273
		private int ejectTime;

		// Token: 0x040004FA RID: 1274
		private int imageSize;

		// Token: 0x040004FB RID: 1275
		private bool valveTestStart;

		// Token: 0x040004FC RID: 1276
		private int valvetestInterval;

		// Token: 0x040004FD RID: 1277
		private bool valveTestIsRunning;

		// Token: 0x040004FE RID: 1278
		private bool rejectWithAllFlaps;
	}
}
