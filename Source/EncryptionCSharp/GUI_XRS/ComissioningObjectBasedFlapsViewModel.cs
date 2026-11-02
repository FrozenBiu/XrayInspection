using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Controls;
using Common;
using DatabaseConnection;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x0200005B RID: 91
	public class ComissioningObjectBasedFlapsViewModel : INotifyPropertyChanged
	{
		// Token: 0x14000035 RID: 53
		// (add) Token: 0x060008FF RID: 2303 RVA: 0x00038358 File Offset: 0x00036558
		// (remove) Token: 0x06000900 RID: 2304 RVA: 0x00038390 File Offset: 0x00036590
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x1700029F RID: 671
		// (get) Token: 0x06000901 RID: 2305 RVA: 0x000383C5 File Offset: 0x000365C5
		// (set) Token: 0x06000902 RID: 2306 RVA: 0x000383CD File Offset: 0x000365CD
		public RelayCommand CleanUpCommand { get; set; }

		// Token: 0x170002A0 RID: 672
		// (get) Token: 0x06000903 RID: 2307 RVA: 0x000383D6 File Offset: 0x000365D6
		// (set) Token: 0x06000904 RID: 2308 RVA: 0x000383DE File Offset: 0x000365DE
		public List<string> FlapCountList { get; set; }

		// Token: 0x170002A1 RID: 673
		// (get) Token: 0x06000905 RID: 2309 RVA: 0x000383E8 File Offset: 0x000365E8
		// (set) Token: 0x06000906 RID: 2310 RVA: 0x00038400 File Offset: 0x00036600
		public string SelectedFlap
		{
			get
			{
				return this.selectedFlap;
			}
			set
			{
				bool flag = this.selectedFlap != value;
				if (flag)
				{
					this.selectedFlap = value;
					this.OnPropertyChanged("SelectedFlap");
				}
			}
		}

		// Token: 0x170002A2 RID: 674
		// (get) Token: 0x06000907 RID: 2311 RVA: 0x00038434 File Offset: 0x00036634
		// (set) Token: 0x06000908 RID: 2312 RVA: 0x00038464 File Offset: 0x00036664
		public string MinPixelFlap1
		{
			get
			{
				this.minPixelFlap1 = this.parHandling.ReadPar("/hardware/rejecter/rejecter1/ObjBasRejPixMin/Value");
				return this.minPixelFlap1;
			}
			set
			{
				bool flag = this.minPixelFlap1 != value;
				if (flag)
				{
					this.minPixelFlap1 = value;
					this.OnPropertyChanged("MinPixelFlap1");
				}
			}
		}

		// Token: 0x170002A3 RID: 675
		// (get) Token: 0x06000909 RID: 2313 RVA: 0x00038498 File Offset: 0x00036698
		// (set) Token: 0x0600090A RID: 2314 RVA: 0x000384B0 File Offset: 0x000366B0
		public string MinPixelFlap2
		{
			get
			{
				return this.minPixelFlap2;
			}
			set
			{
				bool flag = this.minPixelFlap2 != value;
				if (flag)
				{
					this.minPixelFlap2 = value;
					this.OnPropertyChanged("MinPixelFlap2");
				}
			}
		}

		// Token: 0x170002A4 RID: 676
		// (get) Token: 0x0600090B RID: 2315 RVA: 0x000384E4 File Offset: 0x000366E4
		// (set) Token: 0x0600090C RID: 2316 RVA: 0x000384FC File Offset: 0x000366FC
		public string MinPixelFlap3
		{
			get
			{
				return this.minPixelFlap3;
			}
			set
			{
				bool flag = this.minPixelFlap3 != value;
				if (flag)
				{
					this.minPixelFlap3 = value;
					this.OnPropertyChanged("MinPixelFlap3");
				}
			}
		}

		// Token: 0x170002A5 RID: 677
		// (get) Token: 0x0600090D RID: 2317 RVA: 0x00038530 File Offset: 0x00036730
		// (set) Token: 0x0600090E RID: 2318 RVA: 0x00038548 File Offset: 0x00036748
		public string MinPixelFlap4
		{
			get
			{
				return this.minPixelFlap4;
			}
			set
			{
				bool flag = this.minPixelFlap4 != value;
				if (flag)
				{
					this.minPixelFlap4 = value;
					this.OnPropertyChanged("MinPixelFlap4");
				}
			}
		}

		// Token: 0x170002A6 RID: 678
		// (get) Token: 0x0600090F RID: 2319 RVA: 0x0003857C File Offset: 0x0003677C
		// (set) Token: 0x06000910 RID: 2320 RVA: 0x00038594 File Offset: 0x00036794
		public string MaxPixelFlap1
		{
			get
			{
				return this.maxPixelFlap1;
			}
			set
			{
				bool flag = this.maxPixelFlap1 != value;
				if (flag)
				{
					this.maxPixelFlap1 = value;
					this.OnPropertyChanged("MaxPixelFlap1");
				}
			}
		}

		// Token: 0x170002A7 RID: 679
		// (get) Token: 0x06000911 RID: 2321 RVA: 0x000385C8 File Offset: 0x000367C8
		// (set) Token: 0x06000912 RID: 2322 RVA: 0x000385E0 File Offset: 0x000367E0
		public string MaxPixelFlap2
		{
			get
			{
				return this.maxPixelFlap2;
			}
			set
			{
				bool flag = this.maxPixelFlap2 != value;
				if (flag)
				{
					this.maxPixelFlap2 = value;
					this.OnPropertyChanged("MaxPixelFlap2");
				}
			}
		}

		// Token: 0x170002A8 RID: 680
		// (get) Token: 0x06000913 RID: 2323 RVA: 0x00038614 File Offset: 0x00036814
		// (set) Token: 0x06000914 RID: 2324 RVA: 0x0003862C File Offset: 0x0003682C
		public string MaxPixelFlap3
		{
			get
			{
				return this.maxPixelFlap3;
			}
			set
			{
				bool flag = this.maxPixelFlap3 != value;
				if (flag)
				{
					this.maxPixelFlap3 = value;
					this.OnPropertyChanged("MaxPixelFlap3");
				}
			}
		}

		// Token: 0x170002A9 RID: 681
		// (get) Token: 0x06000915 RID: 2325 RVA: 0x00038660 File Offset: 0x00036860
		// (set) Token: 0x06000916 RID: 2326 RVA: 0x00038678 File Offset: 0x00036878
		public string MaxPixelFlap4
		{
			get
			{
				return this.maxPixelFlap4;
			}
			set
			{
				bool flag = this.maxPixelFlap4 != value;
				if (flag)
				{
					this.maxPixelFlap4 = value;
					this.OnPropertyChanged("MaxPixelFlap4");
				}
			}
		}

		// Token: 0x170002AA RID: 682
		// (get) Token: 0x06000917 RID: 2327 RVA: 0x000386AB File Offset: 0x000368AB
		// (set) Token: 0x06000918 RID: 2328 RVA: 0x000386B3 File Offset: 0x000368B3
		public string IgnoreLeft { get; set; }

		// Token: 0x170002AB RID: 683
		// (get) Token: 0x06000919 RID: 2329 RVA: 0x000386BC File Offset: 0x000368BC
		// (set) Token: 0x0600091A RID: 2330 RVA: 0x000386C4 File Offset: 0x000368C4
		public string IgnoreRight { get; set; }

		// Token: 0x170002AC RID: 684
		// (get) Token: 0x0600091B RID: 2331 RVA: 0x000386CD File Offset: 0x000368CD
		// (set) Token: 0x0600091C RID: 2332 RVA: 0x000386D5 File Offset: 0x000368D5
		public string PixelCount { get; set; }

		// Token: 0x0600091D RID: 2333 RVA: 0x000386E0 File Offset: 0x000368E0
		public ComissioningObjectBasedFlapsViewModel(DBConnection dbConnection, ParameterHandling parHandling)
		{
			this.parHandling = parHandling;
			this.dbConnection = dbConnection;
			this.CleanUpCommand = new RelayCommand(new Action<object>(this.CleanUp));
			this.StartWritingFlapParamCommand = new RelayCommand(new Action<object>(this.StartWritingFlapParams));
			this.btnStartWritingFlapParams = new Button();
			this.Init();
		}

		// Token: 0x0600091E RID: 2334 RVA: 0x00038744 File Offset: 0x00036944
		private void Init()
		{
			int value = 0;
			int value2 = 0;
			string path = Definition.detectorSesotecNPixelPath;
			ConveyingDirection conveyingDirection = ConveyingDirection.LeftToRight;
			this.dbConnection.GetConveyingDirection(ref conveyingDirection);
			DetectorType detectorType = DetectorType.Sesotec;
			this.dbConnection.GetDetectorType(ref detectorType);
			bool flag = false;
			bool flag2 = detectorType == DetectorType.DTNetTDI;
			if (flag2)
			{
				this.parHandling.WriteValue("/hardware/detector/dt/StartStopScanning/Value", "0", false);
				bool flag3 = conveyingDirection == ConveyingDirection.LeftToRight;
				if (flag3)
				{
					this.parHandling.WriteValue("/hardware/detector/dt/TdiCorrectionMode/Value", 1.ToString(), false);
				}
				else
				{
					this.parHandling.WriteValue("/hardware/detector/dt/TdiCorrectionMode/Value", 2.ToString(), false);
				}
				this.parHandling.WriteValue("/hardware/detector/dt/StartStopScanning/Value", "1", false);
				this.parHandling.WriteValue("/hardware/rejecter/objectBasedFlaps/CalcPixToFlapMapping/Value", "1", false);
			}
			bool flag4 = detectorType == DetectorType.DTNetDE || detectorType == DetectorType.DTNetTDI;
			if (flag4)
			{
				path = Definition.detectorDTNPixelPath;
			}
			this.PixelCount = this.parHandling.ReadPar(path);
			bool flag5 = Convert.ToInt32(this.PixelCount) < 0;
			if (flag5)
			{
				this.PixelCount = 10239.ToString();
			}
			this.dbConnection.GetBulkObjectEnabled(ref flag);
			bool flag6 = flag;
			if (flag6)
			{
				int num = 1;
				this.dbConnection.GetFlapsCount(DBConnection.defaultApplication, ref num);
				this.FlapCountList = new List<string>();
				this.FlapCountList.Add(num.ToString());
				this.parHandling.WriteValue("/hardware/rejecter/objectBasedFlaps/FlapsCount/Value", num.ToString());
			}
			else
			{
				this.FlapCountList = new List<string>();
				this.FlapCountList.Add("1");
				this.FlapCountList.Add("2");
				bool flag7 = this.parHandling.ReadPar(Definition.ioSubversionPath) == "2";
				if (flag7)
				{
					this.FlapCountList.Add("3");
					this.FlapCountList.Add("4");
				}
			}
			this.dbConnection.GetIgnoredLeftPixel(ref value);
			this.dbConnection.GetIgnoredRightPixel(ref value2);
			this.IgnoreLeft = Convert.ToString(value);
			this.IgnoreRight = Convert.ToString(value2);
			this.SelectedFlap = this.parHandling.ReadPar("/hardware/rejecter/objectBasedFlaps/FlapsCount/Value");
			this.ReadMinMaxPixelFlap();
		}

		// Token: 0x0600091F RID: 2335 RVA: 0x000389A0 File Offset: 0x00036BA0
		private void ReadMinMaxPixelFlap()
		{
			this.MinPixelFlap1 = this.parHandling.ReadPar("/hardware/rejecter/rejecter1/ObjBasRejPixMin/Value");
			this.MinPixelFlap2 = this.parHandling.ReadPar("/hardware/rejecter/rejecter2/ObjBasRejPixMin/Value");
			this.MinPixelFlap3 = this.parHandling.ReadPar("/hardware/rejecter/rejecter3/ObjBasRejPixMin/Value");
			this.MinPixelFlap4 = this.parHandling.ReadPar("/hardware/rejecter/rejecter4/ObjBasRejPixMin/Value");
			this.MaxPixelFlap1 = this.parHandling.ReadPar("/hardware/rejecter/rejecter1/ObjBasRejPixMax/Value");
			this.MaxPixelFlap2 = this.parHandling.ReadPar("/hardware/rejecter/rejecter2/ObjBasRejPixMax/Value");
			this.MaxPixelFlap3 = this.parHandling.ReadPar("/hardware/rejecter/rejecter3/ObjBasRejPixMax/Value");
			this.MaxPixelFlap4 = this.parHandling.ReadPar("/hardware/rejecter/rejecter4/ObjBasRejPixMax/Value");
		}

		// Token: 0x06000920 RID: 2336 RVA: 0x00038A68 File Offset: 0x00036C68
		private void StartWritingFlapParams(object obj)
		{
			bool flag = this.SelectedFlap == "1";
			if (flag)
			{
				this.parHandling.WriteValue("/hardware/rejecter/objectBasedFlaps/FlapsCount/Value", "1");
			}
			else
			{
				bool flag2 = this.SelectedFlap == "2";
				if (flag2)
				{
					this.parHandling.WriteValue("/hardware/rejecter/objectBasedFlaps/FlapsCount/Value", "2");
				}
				else
				{
					bool flag3 = this.SelectedFlap == "3";
					if (flag3)
					{
						this.parHandling.WriteValue("/hardware/rejecter/objectBasedFlaps/FlapsCount/Value", "3");
					}
					else
					{
						bool flag4 = this.SelectedFlap == "4";
						if (flag4)
						{
							this.parHandling.WriteValue("/hardware/rejecter/objectBasedFlaps/FlapsCount/Value", "4");
						}
					}
				}
			}
			this.ReadMinMaxPixelFlap();
			string message = LocalizationManager.Localize("txtParameterLoadedSuccess");
			Dialog.Show(LocalizationManager.Localize("txtSuccess"), message, DialogStyle.Ok, DialogButtons.OK);
		}

		// Token: 0x06000921 RID: 2337 RVA: 0x00038B58 File Offset: 0x00036D58
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x06000922 RID: 2338 RVA: 0x0001092C File Offset: 0x0000EB2C
		private void CleanUp(object obj)
		{
		}

		// Token: 0x0400061A RID: 1562
		private ParameterHandling parHandling;

		// Token: 0x0400061B RID: 1563
		private DBConnection dbConnection;

		// Token: 0x0400061E RID: 1566
		public Button btnStartWritingFlapParams;

		// Token: 0x0400061F RID: 1567
		public RelayCommand StartWritingFlapParamCommand;

		// Token: 0x04000621 RID: 1569
		private const string flap1Enable = "/hardware/rejecter/rejecter1/Enabled/Value";

		// Token: 0x04000622 RID: 1570
		private const string flap2Enable = "/hardware/rejecter/rejecter2/Enabled/Value";

		// Token: 0x04000623 RID: 1571
		private const string flap3Enable = "/hardware/rejecter/rejecter3/Enabled/Value";

		// Token: 0x04000624 RID: 1572
		private const string flap4Enable = "/hardware/rejecter/rejecter4/Enabled/Value";

		// Token: 0x04000625 RID: 1573
		private const string flap1Min = "/hardware/rejecter/rejecter1/ObjBasRejPixMin/Value";

		// Token: 0x04000626 RID: 1574
		private const string flap2Min = "/hardware/rejecter/rejecter2/ObjBasRejPixMin/Value";

		// Token: 0x04000627 RID: 1575
		private const string flap3Min = "/hardware/rejecter/rejecter3/ObjBasRejPixMin/Value";

		// Token: 0x04000628 RID: 1576
		private const string flap4Min = "/hardware/rejecter/rejecter4/ObjBasRejPixMin/Value";

		// Token: 0x04000629 RID: 1577
		private const string flap1Max = "/hardware/rejecter/rejecter1/ObjBasRejPixMax/Value";

		// Token: 0x0400062A RID: 1578
		private const string flap2Max = "/hardware/rejecter/rejecter2/ObjBasRejPixMax/Value";

		// Token: 0x0400062B RID: 1579
		private const string flap3Max = "/hardware/rejecter/rejecter3/ObjBasRejPixMax/Value";

		// Token: 0x0400062C RID: 1580
		private const string flap4Max = "/hardware/rejecter/rejecter4/ObjBasRejPixMax/Value";

		// Token: 0x0400062D RID: 1581
		private const string defaultMain = "/defaultProduct/mainTable";

		// Token: 0x0400062E RID: 1582
		private string selectedFlap;

		// Token: 0x0400062F RID: 1583
		private string minPixelFlap1;

		// Token: 0x04000630 RID: 1584
		private string minPixelFlap2;

		// Token: 0x04000631 RID: 1585
		private string minPixelFlap3;

		// Token: 0x04000632 RID: 1586
		private string minPixelFlap4;

		// Token: 0x04000633 RID: 1587
		private string maxPixelFlap1;

		// Token: 0x04000634 RID: 1588
		private string maxPixelFlap2;

		// Token: 0x04000635 RID: 1589
		private string maxPixelFlap3;

		// Token: 0x04000636 RID: 1590
		private string maxPixelFlap4;
	}
}
