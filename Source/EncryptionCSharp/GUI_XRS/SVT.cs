using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Threading;
using DatabaseConnection;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x0200004F RID: 79
	public class SVT : INotifyPropertyChanged
	{
		// Token: 0x1400002F RID: 47
		// (add) Token: 0x060007C1 RID: 1985 RVA: 0x00032388 File Offset: 0x00030588
		// (remove) Token: 0x060007C2 RID: 1986 RVA: 0x000323C0 File Offset: 0x000305C0
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x17000237 RID: 567
		// (get) Token: 0x060007C3 RID: 1987 RVA: 0x000323F8 File Offset: 0x000305F8
		// (set) Token: 0x060007C4 RID: 1988 RVA: 0x00032410 File Offset: 0x00030610
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
						this.parHandling.WriteValue(SVT.svtValvesEnablePath, value ? "1" : "0");
					}
					this.OnPropertyChanged("Enable");
				}
			}
		}

		// Token: 0x17000238 RID: 568
		// (get) Token: 0x060007C5 RID: 1989 RVA: 0x00032470 File Offset: 0x00030670
		// (set) Token: 0x060007C6 RID: 1990 RVA: 0x00032488 File Offset: 0x00030688
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
						this.parHandling.WriteValue(SVT.svtDelayPath, value.ToString());
					}
					this.OnPropertyChanged("Delay");
				}
			}
		}

		// Token: 0x17000239 RID: 569
		// (get) Token: 0x060007C7 RID: 1991 RVA: 0x000324F4 File Offset: 0x000306F4
		// (set) Token: 0x060007C8 RID: 1992 RVA: 0x0003250C File Offset: 0x0003070C
		public int EjectTime
		{
			get
			{
				return this.ejectTime;
			}
			set
			{
				bool flag = value > 127;
				if (flag)
				{
					value = 127;
				}
				bool flag2 = this.ejectTime != value;
				if (flag2)
				{
					this.ejectTime = value;
					bool flag3 = this.initialized;
					if (flag3)
					{
						this.parHandling.WriteValue(SVT.svtEjectTimePath, value.ToString());
					}
					this.OnPropertyChanged("EjectTime");
				}
			}
		}

		// Token: 0x1700023A RID: 570
		// (get) Token: 0x060007C9 RID: 1993 RVA: 0x00032570 File Offset: 0x00030770
		// (set) Token: 0x060007CA RID: 1994 RVA: 0x00032588 File Offset: 0x00030788
		public int StartPixel
		{
			get
			{
				return this.startPixel;
			}
			set
			{
				bool flag = this.startPixel != value;
				if (flag)
				{
					this.startPixel = value;
					bool flag2 = this.initialized;
					if (flag2)
					{
						this.dbConnection.SetValveStartPixel(value);
						this.SetAssignments();
					}
					this.OnPropertyChanged("StartPixel");
				}
			}
		}

		// Token: 0x1700023B RID: 571
		// (get) Token: 0x060007CB RID: 1995 RVA: 0x000325DC File Offset: 0x000307DC
		// (set) Token: 0x060007CC RID: 1996 RVA: 0x000325F4 File Offset: 0x000307F4
		public double ValveWidth
		{
			get
			{
				return this.valveWidth;
			}
			set
			{
				bool flag = this.valveWidth != value;
				if (flag)
				{
					bool flag2 = value < 0.0;
					if (flag2)
					{
						value = 0.0;
					}
					this.valveWidth = value;
					bool flag3 = this.initialized;
					if (flag3)
					{
						this.dbConnection.SetValveWidth(value);
						this.SetAssignments();
					}
					this.OnPropertyChanged("ValveWidth");
				}
			}
		}

		// Token: 0x1700023C RID: 572
		// (get) Token: 0x060007CD RID: 1997 RVA: 0x00032664 File Offset: 0x00030864
		// (set) Token: 0x060007CE RID: 1998 RVA: 0x0003267C File Offset: 0x0003087C
		public int OverlappingLeft
		{
			get
			{
				return this.overlappingLeft;
			}
			set
			{
				bool flag = this.overlappingLeft != value;
				if (flag)
				{
					bool flag2 = value > 100;
					if (flag2)
					{
						value = 100;
					}
					this.overlappingLeft = value;
					bool flag3 = this.initialized;
					if (flag3)
					{
						this.dbConnection.SetValveOverlappingLeft(value);
						this.SetAssignments();
					}
					this.OnPropertyChanged("OverlappingLeft");
				}
			}
		}

		// Token: 0x1700023D RID: 573
		// (get) Token: 0x060007CF RID: 1999 RVA: 0x000326DC File Offset: 0x000308DC
		// (set) Token: 0x060007D0 RID: 2000 RVA: 0x000326F4 File Offset: 0x000308F4
		public int OverlappingRight
		{
			get
			{
				return this.overlappingRight;
			}
			set
			{
				bool flag = this.overlappingRight != value;
				if (flag)
				{
					bool flag2 = value > 100;
					if (flag2)
					{
						value = 100;
					}
					this.overlappingRight = value;
					bool flag3 = this.initialized;
					if (flag3)
					{
						this.dbConnection.SetValveOverlappingRight(value);
						this.SetAssignments();
					}
					this.OnPropertyChanged("OverlappingRight");
				}
			}
		}

		// Token: 0x1700023E RID: 574
		// (get) Token: 0x060007D1 RID: 2001 RVA: 0x00032754 File Offset: 0x00030954
		// (set) Token: 0x060007D2 RID: 2002 RVA: 0x0003276C File Offset: 0x0003096C
		public int ValveTestStartValve
		{
			get
			{
				return this.valveTestStartValve;
			}
			set
			{
				bool flag = this.valveTestStartValve != value;
				if (flag)
				{
					bool flag2 = value > this.ValveTestEndValve;
					if (flag2)
					{
						value = this.ValveTestEndValve;
					}
					bool flag3 = value > this.NumValves;
					if (flag3)
					{
						value = this.NumValves;
					}
					bool flag4 = value < 1;
					if (flag4)
					{
						value = 1;
					}
					this.valveTestStartValve = value;
					bool flag5 = this.initialized;
					if (flag5)
					{
						this.parHandling.WriteValue(SVT.svtTestStartValvePath, value.ToString(), false);
						this.dbConnection.SetValveTestStartValve(value);
					}
					this.OnPropertyChanged("ValveTestStartValve");
				}
			}
		}

		// Token: 0x1700023F RID: 575
		// (get) Token: 0x060007D3 RID: 2003 RVA: 0x00032808 File Offset: 0x00030A08
		// (set) Token: 0x060007D4 RID: 2004 RVA: 0x00032820 File Offset: 0x00030A20
		public int ValveTestEndValve
		{
			get
			{
				return this.valveTestEndValve;
			}
			set
			{
				bool flag = this.valveTestEndValve != value;
				if (flag)
				{
					bool flag2 = value < this.ValveTestStartValve;
					if (flag2)
					{
						value = this.ValveTestStartValve;
					}
					bool flag3 = value > this.NumValves;
					if (flag3)
					{
						value = this.NumValves;
					}
					bool flag4 = value < 1;
					if (flag4)
					{
						value = 1;
					}
					this.valveTestEndValve = value;
					bool flag5 = this.initialized;
					if (flag5)
					{
						this.parHandling.WriteValue(SVT.svtTestEndValvePath, value.ToString(), false);
						this.dbConnection.SetValveTestEndValve(value);
					}
					this.OnPropertyChanged("ValveTestEndValve");
				}
			}
		}

		// Token: 0x17000240 RID: 576
		// (get) Token: 0x060007D5 RID: 2005 RVA: 0x000328BC File Offset: 0x00030ABC
		// (set) Token: 0x060007D6 RID: 2006 RVA: 0x000328D4 File Offset: 0x00030AD4
		public int TestInterval
		{
			get
			{
				return this.testInterval;
			}
			set
			{
				bool flag = this.testInterval != value;
				if (flag)
				{
					bool flag2 = value > 10000;
					if (flag2)
					{
						value = 10000;
					}
					this.testInterval = value;
					bool flag3 = this.initialized;
					if (flag3)
					{
						this.parHandling.WriteValue(SVT.svtTestIntervalPath, value.ToString(), false);
						this.dbConnection.SetValveTestInterval(value);
					}
					this.OnPropertyChanged("TestInterval");
				}
			}
		}

		// Token: 0x17000241 RID: 577
		// (get) Token: 0x060007D7 RID: 2007 RVA: 0x0003294C File Offset: 0x00030B4C
		// (set) Token: 0x060007D8 RID: 2008 RVA: 0x00032964 File Offset: 0x00030B64
		public int NumValves
		{
			get
			{
				return this.numValves;
			}
			set
			{
				bool flag = this.numValves != value;
				if (flag)
				{
					bool flag2 = value > 20;
					if (flag2)
					{
						value = 20;
					}
					bool flag3 = value < 1;
					if (flag3)
					{
						value = 1;
					}
					this.numValves = value;
					bool flag4 = this.initialized;
					if (flag4)
					{
						this.dbConnection.SetNumValves(value);
						this.dbDataLogs.AddRejectOnOffTimes();
						this.dbDataLogs.AddSVTValves(value);
						this.dbDataLogs.AddSVTValvesOperatingCounter(value);
						this.CalculateValues();
					}
					bool flag5 = this.NumValves < this.ValveTestEndValve;
					if (flag5)
					{
						this.ValveTestEndValve = this.NumValves;
					}
					this.OnPropertyChanged("NumValves");
				}
			}
		}

		// Token: 0x17000242 RID: 578
		// (get) Token: 0x060007D9 RID: 2009 RVA: 0x00032A1C File Offset: 0x00030C1C
		// (set) Token: 0x060007DA RID: 2010 RVA: 0x00032A34 File Offset: 0x00030C34
		public bool ValvesInverted
		{
			get
			{
				return this.valvesInverted;
			}
			set
			{
				bool flag = this.valvesInverted != value;
				if (flag)
				{
					this.valvesInverted = value;
					bool flag2 = this.initialized;
					if (flag2)
					{
						this.dbConnection.SetValvesInverted(value);
						this.OnPropertyChanged("ValvesInverted");
						this.SetAssignments();
					}
				}
			}
		}

		// Token: 0x17000243 RID: 579
		// (get) Token: 0x060007DB RID: 2011 RVA: 0x00032A88 File Offset: 0x00030C88
		// (set) Token: 0x060007DC RID: 2012 RVA: 0x00032AA0 File Offset: 0x00030CA0
		public int FirstValve
		{
			get
			{
				return this.firstValve;
			}
			set
			{
				bool flag = this.firstValve != value && value > 0;
				if (flag)
				{
					this.firstValve = value;
					bool flag2 = this.initialized;
					if (flag2)
					{
						this.dbConnection.SetFirstValve(value);
						this.OnPropertyChanged("FirstValve");
						this.SetAssignments();
					}
				}
			}
		}

		// Token: 0x17000244 RID: 580
		// (get) Token: 0x060007DE RID: 2014 RVA: 0x00032B10 File Offset: 0x00030D10
		// (set) Token: 0x060007DD RID: 2013 RVA: 0x00032AF7 File Offset: 0x00030CF7
		public bool SvtTestRunning
		{
			get
			{
				return this.svtTestRunning;
			}
			set
			{
				this.svtTestRunning = value;
				this.OnPropertyChanged("SvtTestRunning");
			}
		}

		// Token: 0x17000245 RID: 581
		// (get) Token: 0x060007DF RID: 2015 RVA: 0x00032B28 File Offset: 0x00030D28
		// (set) Token: 0x060007E0 RID: 2016 RVA: 0x00032B40 File Offset: 0x00030D40
		public int StartPixelSecondValve
		{
			get
			{
				return this.startPixelSecondValve;
			}
			set
			{
				bool flag = this.startPixelSecondValve != value && value > 0;
				if (flag)
				{
					this.startPixelSecondValve = value;
					bool flag2 = this.initialized;
					if (flag2)
					{
						this.dbConnection.SetSVTStartPixelSecondValve(value);
						this.CalculateValues();
					}
					this.OnPropertyChanged("StartPixelSecondValve");
				}
			}
		}

		// Token: 0x17000246 RID: 582
		// (get) Token: 0x060007E1 RID: 2017 RVA: 0x00032B98 File Offset: 0x00030D98
		// (set) Token: 0x060007E2 RID: 2018 RVA: 0x00032BB0 File Offset: 0x00030DB0
		public int EndPixelNextToLastValve
		{
			get
			{
				return this.endPixelNextToLast;
			}
			set
			{
				bool flag = this.endPixelNextToLast != value && value > 0;
				if (flag)
				{
					this.endPixelNextToLast = value;
					bool flag2 = this.initialized;
					if (flag2)
					{
						this.dbConnection.SetSVTEndPixelNextToLastValve(value);
						this.CalculateValues();
					}
					this.OnPropertyChanged("EndPixelNextToLastValve");
				}
			}
		}

		// Token: 0x17000247 RID: 583
		// (get) Token: 0x060007E3 RID: 2019 RVA: 0x00032C08 File Offset: 0x00030E08
		// (set) Token: 0x060007E4 RID: 2020 RVA: 0x00032C20 File Offset: 0x00030E20
		public int ImageSize
		{
			get
			{
				return this.imageSize;
			}
			set
			{
				bool flag = value > 300;
				if (flag)
				{
					value = 300;
				}
				bool flag2 = value < 50;
				if (flag2)
				{
					value = 50;
				}
				bool flag3 = this.imageSize != value;
				if (flag3)
				{
					this.imageSize = value;
					this.parHandling.WriteValue(Acquisition.nMaxLinesPath, value.ToString());
					this.calcInitEjectTime();
					this.OnPropertyChanged("ImageSize");
				}
			}
		}

		// Token: 0x17000248 RID: 584
		// (get) Token: 0x060007E5 RID: 2021 RVA: 0x00032C94 File Offset: 0x00030E94
		// (set) Token: 0x060007E6 RID: 2022 RVA: 0x00032CAC File Offset: 0x00030EAC
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

		// Token: 0x060007E7 RID: 2023 RVA: 0x00032CE0 File Offset: 0x00030EE0
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
				bool flag2 = num3 > 650.0;
				if (flag2)
				{
					num3 = 650.0;
				}
				num5 = 2.0;
			}
			this.InitEjectTime = 0;
			bool flag3 = num3 > 0.0;
			if (flag3)
			{
				this.InitEjectTime = (int)(num2 * num * 1000.0 / num3 * num4 / num5);
			}
		}

		// Token: 0x060007E8 RID: 2024 RVA: 0x00032E54 File Offset: 0x00031054
		private void CalculateValues()
		{
			int num = 0;
			int num2 = 0;
			this.dbConnection.GetIgnoredRightPixel(ref num2);
			this.dbConnection.GetIgnoredLeftPixel(ref num);
			int num3 = 0;
			string text = this.parHandling.ReadPar(Definition.detectorTypePath);
			bool flag = text.Equals("1");
			if (flag)
			{
				int.TryParse(this.parHandling.ReadPar(Definition.detectorSesotecNPixelPath), out num3);
			}
			else
			{
				int.TryParse(this.parHandling.ReadPar(Definition.detectorDTNPixelPath), out num3);
			}
			double num4 = Math.Round((double)(this.EndPixelNextToLastValve - this.StartPixelSecondValve) / ((double)this.NumValves - 2.0), 3);
			int num5 = (int)num4;
			bool flag2 = (double)(num5 - num5) >= 0.5;
			int num6;
			if (flag2)
			{
				num6 = (int)num4 - (this.StartPixelSecondValve - num) + 1;
			}
			else
			{
				num6 = (int)num4 - (this.StartPixelSecondValve - num) + 1;
			}
			int num7 = num3 - num2;
			this.ValveWidth = num4;
			this.StartPixel = num6;
			this.SetAssignments();
		}

		// Token: 0x060007E9 RID: 2025 RVA: 0x00032F66 File Offset: 0x00031166
		public SVT(ParameterHandling parHandling, DBConnection dbConnection, MachineState machineState, DBDataLogs dbDataLogs) : this(parHandling, dbConnection, machineState, true, dbDataLogs)
		{
		}

		// Token: 0x060007EA RID: 2026 RVA: 0x00032F78 File Offset: 0x00031178
		public SVT(ParameterHandling parHandling, DBConnection dbConnection, MachineState machineState, bool showHazardMessage, DBDataLogs dbDataLogs)
		{
			string s = null;
			int endPixelNextToLastValve = 0;
			double num = 0.0;
			bool flag = false;
			this.parHandling = parHandling;
			this.dbConnection = dbConnection;
			this.machineState = machineState;
			this.showHazardMessage = showHazardMessage;
			this.dbDataLogs = dbDataLogs;
			bool flag2 = this.parHandling != null && dbConnection.Connection != null;
			if (flag2)
			{
				this.Enable = (parHandling.ReadPar(SVT.svtValvesEnablePath) == "1");
				dbConnection.GetParameterValue(SVT.svtDelayPath, ref s);
				int.TryParse(s, out this.delay);
				dbConnection.GetParameterValue(SVT.svtEjectTimePath, ref s);
				int.TryParse(s, out this.ejectTime);
				dbConnection.GetValveStartPixel(ref endPixelNextToLastValve);
				this.StartPixel = endPixelNextToLastValve;
				dbConnection.GetValveWidth(ref num);
				this.ValveWidth = num;
				dbConnection.GetValveOverlappingLeft(ref endPixelNextToLastValve);
				this.OverlappingLeft = endPixelNextToLastValve;
				dbConnection.GetValveOverlappingRight(ref endPixelNextToLastValve);
				this.OverlappingRight = endPixelNextToLastValve;
				dbConnection.GetNumValves(ref endPixelNextToLastValve);
				this.NumValves = endPixelNextToLastValve;
				dbConnection.GetValveTestStartValve(ref endPixelNextToLastValve);
				this.ValveTestStartValve = endPixelNextToLastValve;
				dbConnection.GetValveTestEndValve(ref endPixelNextToLastValve);
				this.ValveTestEndValve = endPixelNextToLastValve;
				dbConnection.GetValveTestInterval(ref endPixelNextToLastValve);
				this.TestInterval = endPixelNextToLastValve;
				dbConnection.GetValvesInverted(ref flag);
				this.ValvesInverted = flag;
				dbConnection.GetFirstValve(ref endPixelNextToLastValve);
				this.FirstValve = endPixelNextToLastValve;
				dbConnection.GetSVTEndPixelNextToLastValve(ref endPixelNextToLastValve);
				this.EndPixelNextToLastValve = endPixelNextToLastValve;
				dbConnection.GetSVTStartPixelSecondValve(ref endPixelNextToLastValve);
				this.StartPixelSecondValve = endPixelNextToLastValve;
				this.valveTestStarted = (parHandling.ReadPar(SVT.svtPeriodicModePath) != "0");
				this.ImageSize = Convert.ToInt32(parHandling.ReadPar(Acquisition.nMaxLinesPath));
			}
			this.timerTestEnded = new DispatcherTimer(new TimeSpan(0, 0, 2), DispatcherPriority.Normal, new EventHandler(this.CheckTestEnded), Application.Current.Dispatcher);
			this.initialized = true;
		}

		// Token: 0x060007EB RID: 2027 RVA: 0x00033188 File Offset: 0x00031388
		private void CheckTestEnded(object sender, EventArgs e)
		{
			bool flag = this.parHandling.ReadPar(SVT.svtPeriodicModePath) == "0";
			if (flag)
			{
				this.SvtTestRunning = false;
				this.timerTestEnded.Stop();
			}
		}

		// Token: 0x060007EC RID: 2028 RVA: 0x000331CC File Offset: 0x000313CC
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x060007ED RID: 2029 RVA: 0x000331FC File Offset: 0x000313FC
		private void SetAssignments()
		{
			List<int> list = new List<int>();
			List<int> list2 = new List<int>();
			double num = 0.0;
			int num2 = 0;
			string text = this.parHandling.ReadPar(Definition.detectorTypePath);
			int num3 = (int)this.ValveWidth;
			for (int i = 1; i <= this.numValves; i++)
			{
				bool flag = i < this.firstValve;
				int num4;
				if (flag)
				{
					num4 = -1;
					num2 = -1;
				}
				else
				{
					bool flag2 = i == this.firstValve;
					if (flag2)
					{
						num4 = 0;
						num2 = num3 - this.startPixel + this.overlappingRight;
					}
					else
					{
						num4 = num2 - this.overlappingLeft - this.overlappingRight;
						num2 += num3;
					}
					bool flag3 = num4 < 0;
					if (flag3)
					{
						num4 = -1;
					}
					bool flag4 = num2 < 0;
					if (flag4)
					{
						num2 = -1;
					}
					num += this.ValveWidth - (double)num3;
					bool flag5 = num >= 0.5 || i == this.firstValve;
					if (flag5)
					{
						num2++;
						num -= 1.0;
					}
				}
				list.Add(num4);
				list2.Add(num2);
				bool flag6 = this.valvesInverted;
				string str;
				if (flag6)
				{
					str = Definition.SVTValvesAssignmentMainPath + (this.numValves - i + 1).ToString("D2");
				}
				else
				{
					str = Definition.SVTValvesAssignmentMainPath + i.ToString("D2");
				}
				this.parHandling.WriteValue(str + "/MinPixel/Value", num4.ToString(), false);
				this.parHandling.WriteValue(str + "/MaxPixel/Value", num2.ToString(), false);
			}
			for (int j = this.numValves + 1; j <= 80; j++)
			{
				bool flag7 = this.valvesInverted;
				string str;
				if (flag7)
				{
					str = Definition.SVTValvesAssignmentMainPath + (this.numValves - j + 1).ToString("D2");
				}
				else
				{
					str = Definition.SVTValvesAssignmentMainPath + j.ToString("D2");
				}
				this.parHandling.WriteValue(str + "/MinPixel/Value", "-1", false);
				this.parHandling.WriteValue(str + "/MaxPixel/Value", "-1", false);
				list.Add(-1);
				list2.Add(-1);
			}
			this.dbConnection.SetSVTMinPixels(0, list.ToArray(), this.valvesInverted);
			this.dbConnection.SetSVTMaxPixels(0, list2.ToArray(), this.valvesInverted);
		}

		// Token: 0x060007EE RID: 2030 RVA: 0x000334B0 File Offset: 0x000316B0
		public void StartOrStopValveTest()
		{
			bool flag = this.enable;
			if (flag)
			{
				bool flag2 = !this.SvtTestRunning;
				if (flag2)
				{
					int num = 0;
					string s = this.parHandling.ReadPar(Definition.ioBulkVersionPath);
					int.TryParse(s, out num);
					bool flag3 = !this.showHazardMessage || Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtCrushingHazard"), DialogStyle.Warning, DialogButtons.OKCancel) == Dialog.DialogResults.Ok;
					if (flag3)
					{
						this.parHandling.WriteValue(SVT.svtPeriodicModePath, 2.ToString(), false);
						this.parHandling.WriteValue(SVT.valveTestEnablePath, "1", false);
						this.valveTestStarted = true;
						this.OnPropertyChanged("ValveTestStarted");
						this.SvtTestRunning = true;
						this.timerTestEnded.Start();
					}
				}
				else
				{
					this.timerTestEnded.Stop();
					this.parHandling.WriteValue(SVT.svtPeriodicModePath, 0.ToString(), false);
					this.parHandling.WriteValue(SVT.valveTestEnablePath, "0", false);
					this.SvtTestRunning = false;
				}
			}
			else
			{
				Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtRejecterIsDisabled"), DialogStyle.Error, DialogButtons.OK);
			}
		}

		// Token: 0x060007EF RID: 2031 RVA: 0x000335F8 File Offset: 0x000317F8
		public void EndValveTest()
		{
			this.parHandling.WriteValue(SVT.svtPeriodicModePath, 0.ToString(), false);
			this.parHandling.WriteValue(SVT.valveTestEnablePath, "0", false);
			this.SvtTestRunning = false;
		}

		// Token: 0x060007F0 RID: 2032 RVA: 0x00033640 File Offset: 0x00031840
		public void Update()
		{
			int endPixelNextToLastValve = 0;
			double num = 0.0;
			bool flag = false;
			bool flag2 = this.parHandling != null && this.dbConnection.Connection != null;
			if (flag2)
			{
				this.initialized = false;
				this.dbConnection.GetValveWidth(ref num);
				this.ValveWidth = num;
				this.dbConnection.GetValveOverlappingLeft(ref endPixelNextToLastValve);
				this.OverlappingLeft = endPixelNextToLastValve;
				this.dbConnection.GetValveOverlappingRight(ref endPixelNextToLastValve);
				this.OverlappingRight = endPixelNextToLastValve;
				this.dbConnection.GetNumValves(ref endPixelNextToLastValve);
				this.NumValves = endPixelNextToLastValve;
				this.dbConnection.GetValvesInverted(ref flag);
				this.ValvesInverted = flag;
				this.dbConnection.GetFirstValve(ref endPixelNextToLastValve);
				this.FirstValve = endPixelNextToLastValve;
				this.dbConnection.GetSVTEndPixelNextToLastValve(ref endPixelNextToLastValve);
				this.EndPixelNextToLastValve = endPixelNextToLastValve;
				this.dbConnection.GetSVTStartPixelSecondValve(ref endPixelNextToLastValve);
				this.StartPixelSecondValve = endPixelNextToLastValve;
				this.initialized = true;
			}
		}

		// Token: 0x04000554 RID: 1364
		public static readonly string svtValvesEnablePath = "/hardware/svt/ValvesEnable/Value";

		// Token: 0x04000555 RID: 1365
		public static readonly string svtEnablePath = "/hardware/svt/Enable/Value";

		// Token: 0x04000556 RID: 1366
		public static readonly string svtDelayPath = "/hardware/svt/SoftwareDelay/Value";

		// Token: 0x04000557 RID: 1367
		public static readonly string svtEjectTimePath = "/hardware/svt/HardwareLength/Value";

		// Token: 0x04000558 RID: 1368
		public static readonly string svtCommErrorPath = "/hardware/svt/status/CommError/Value";

		// Token: 0x04000559 RID: 1369
		public static readonly string svtErrorStatusPath = "/hardware/svt/status/ErrorStatus/Value";

		// Token: 0x0400055A RID: 1370
		public static readonly string svtClearFaults = "/hardware/svt/ClearFaults/Value";

		// Token: 0x0400055B RID: 1371
		public static readonly string svtPeriodicModePath = "/hardware/svt/testing/PeriodicMode/Value";

		// Token: 0x0400055C RID: 1372
		public static readonly string svtTestStartValvePath = "/hardware/svt/testing/MinValve/Value";

		// Token: 0x0400055D RID: 1373
		public static readonly string svtTestEndValvePath = "/hardware/svt/testing/MaxValve/Value";

		// Token: 0x0400055E RID: 1374
		public static readonly string svtTestIntervalPath = "/hardware/svt/testing/PeriodicInterval/Value";

		// Token: 0x0400055F RID: 1375
		public static readonly string svtAirPressurePath = "/hardware/svt/AirPressure/Value";

		// Token: 0x04000560 RID: 1376
		public static readonly string svtMinAirPressurePath = "/hardware/svt/MinAirPressure/Value";

		// Token: 0x04000561 RID: 1377
		public static readonly string svtCOMPort = "/hardware/svt/COMPort/Value";

		// Token: 0x04000562 RID: 1378
		public static readonly string valveTestEnablePath = "/hardware/ios/ValveTestEnable/Value";

		// Token: 0x04000563 RID: 1379
		public static readonly int svtPeriodicModeIndicator = 2;

		// Token: 0x04000564 RID: 1380
		public static readonly int svtPeriodicModeOneShotIndicator = 3;

		// Token: 0x04000565 RID: 1381
		private const int maxEjectTime = 127;

		// Token: 0x04000566 RID: 1382
		private const int maxDelay = 10000;

		// Token: 0x04000567 RID: 1383
		private bool enable;

		// Token: 0x04000568 RID: 1384
		private int delay;

		// Token: 0x04000569 RID: 1385
		private int ejectTime;

		// Token: 0x0400056A RID: 1386
		private int startPixel;

		// Token: 0x0400056B RID: 1387
		private int firstValve;

		// Token: 0x0400056C RID: 1388
		private double valveWidth;

		// Token: 0x0400056D RID: 1389
		private int overlappingLeft;

		// Token: 0x0400056E RID: 1390
		private int overlappingRight;

		// Token: 0x0400056F RID: 1391
		private int numValves;

		// Token: 0x04000570 RID: 1392
		private bool valvesInverted;

		// Token: 0x04000571 RID: 1393
		private int startPixelSecondValve;

		// Token: 0x04000572 RID: 1394
		private int endPixelNextToLast;

		// Token: 0x04000573 RID: 1395
		private ParameterHandling parHandling;

		// Token: 0x04000574 RID: 1396
		private DBConnection dbConnection;

		// Token: 0x04000575 RID: 1397
		private MachineState machineState;

		// Token: 0x04000576 RID: 1398
		private bool initialized = false;

		// Token: 0x04000577 RID: 1399
		private int valveTestStartValve;

		// Token: 0x04000578 RID: 1400
		private int valveTestEndValve;

		// Token: 0x04000579 RID: 1401
		private int testInterval;

		// Token: 0x0400057A RID: 1402
		private bool valveTestStarted;

		// Token: 0x0400057B RID: 1403
		private bool showHazardMessage;

		// Token: 0x0400057D RID: 1405
		private bool svtTestRunning = false;

		// Token: 0x0400057E RID: 1406
		private DispatcherTimer timerTestEnded;

		// Token: 0x0400057F RID: 1407
		private CultureInfo culture = new CultureInfo("en-US");

		// Token: 0x04000580 RID: 1408
		private int imageSize;

		// Token: 0x04000581 RID: 1409
		private int initEjectTime;

		// Token: 0x04000582 RID: 1410
		private DBDataLogs dbDataLogs;
	}
}
