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
	// Token: 0x0200005C RID: 92
	public class ComissioningMultilaneViewModel : INotifyPropertyChanged
	{
		// Token: 0x14000036 RID: 54
		// (add) Token: 0x06000923 RID: 2339 RVA: 0x00038B88 File Offset: 0x00036D88
		// (remove) Token: 0x06000924 RID: 2340 RVA: 0x00038BC0 File Offset: 0x00036DC0
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x170002AD RID: 685
		// (get) Token: 0x06000925 RID: 2341 RVA: 0x00038BF5 File Offset: 0x00036DF5
		// (set) Token: 0x06000926 RID: 2342 RVA: 0x00038BFD File Offset: 0x00036DFD
		public RelayCommand CleanUpCommand { get; set; }

		// Token: 0x170002AE RID: 686
		// (get) Token: 0x06000927 RID: 2343 RVA: 0x00038C06 File Offset: 0x00036E06
		// (set) Token: 0x06000928 RID: 2344 RVA: 0x00038C0E File Offset: 0x00036E0E
		public List<string> LaneCountList { get; set; }

		// Token: 0x170002AF RID: 687
		// (get) Token: 0x06000929 RID: 2345 RVA: 0x00038C18 File Offset: 0x00036E18
		// (set) Token: 0x0600092A RID: 2346 RVA: 0x00038C30 File Offset: 0x00036E30
		public string SelectedLane
		{
			get
			{
				return this.selectedLane;
			}
			set
			{
				bool flag = this.selectedLane != value;
				if (flag)
				{
					this.selectedLane = value;
					this.OnPropertyChanged("SelectedLane");
				}
			}
		}

		// Token: 0x170002B0 RID: 688
		// (get) Token: 0x0600092B RID: 2347 RVA: 0x00038C64 File Offset: 0x00036E64
		// (set) Token: 0x0600092C RID: 2348 RVA: 0x00038C7C File Offset: 0x00036E7C
		public string MinPixelLane1
		{
			get
			{
				return this.minPixelLane1;
			}
			set
			{
				bool flag = this.minPixelLane1 != value;
				if (flag)
				{
					this.minPixelLane1 = value;
					this.parHandling.WriteValue("/filter/acquisition/lane1/MinPixel/Value", this.minPixelLane1);
					this.parHandling.WriteValue("/defaultProduct/mainTable/filter/acquisition/lane1/MinPixel/Value", this.minPixelLane1);
					this.OnPropertyChanged("MinPixelLane1");
				}
			}
		}

		// Token: 0x170002B1 RID: 689
		// (get) Token: 0x0600092D RID: 2349 RVA: 0x00038CE0 File Offset: 0x00036EE0
		// (set) Token: 0x0600092E RID: 2350 RVA: 0x00038CF8 File Offset: 0x00036EF8
		public string MinPixelLane2
		{
			get
			{
				return this.minPixelLane2;
			}
			set
			{
				bool flag = this.minPixelLane2 != value;
				if (flag)
				{
					this.minPixelLane2 = value;
					this.parHandling.WriteValue("/filter/acquisition/lane2/MinPixel/Value", this.minPixelLane2);
					this.parHandling.WriteValue("/defaultProduct/mainTable/filter/acquisition/lane2/MinPixel/Value", this.minPixelLane2);
					this.OnPropertyChanged("MinPixelLane2");
				}
			}
		}

		// Token: 0x170002B2 RID: 690
		// (get) Token: 0x0600092F RID: 2351 RVA: 0x00038D5C File Offset: 0x00036F5C
		// (set) Token: 0x06000930 RID: 2352 RVA: 0x00038D74 File Offset: 0x00036F74
		public string MinPixelLane3
		{
			get
			{
				return this.minPixelLane3;
			}
			set
			{
				bool flag = this.minPixelLane3 != value;
				if (flag)
				{
					this.minPixelLane3 = value;
					this.parHandling.WriteValue("/filter/acquisition/lane3/MinPixel/Value", this.minPixelLane3);
					this.parHandling.WriteValue("/defaultProduct/mainTable/filter/acquisition/lane3/MinPixel/Value", this.minPixelLane3);
					this.OnPropertyChanged("MinPixelLane3");
				}
			}
		}

		// Token: 0x170002B3 RID: 691
		// (get) Token: 0x06000931 RID: 2353 RVA: 0x00038DD8 File Offset: 0x00036FD8
		// (set) Token: 0x06000932 RID: 2354 RVA: 0x00038DF0 File Offset: 0x00036FF0
		public string MinPixelLane4
		{
			get
			{
				return this.minPixelLane4;
			}
			set
			{
				bool flag = this.minPixelLane4 != value;
				if (flag)
				{
					this.minPixelLane4 = value;
					this.parHandling.WriteValue("/filter/acquisition/lane4/MinPixel/Value", this.minPixelLane4);
					this.parHandling.WriteValue("/defaultProduct/mainTable/filter/acquisition/lane4/MinPixel/Value", this.minPixelLane4);
					this.OnPropertyChanged("MinPixelLane4");
				}
			}
		}

		// Token: 0x170002B4 RID: 692
		// (get) Token: 0x06000933 RID: 2355 RVA: 0x00038E54 File Offset: 0x00037054
		// (set) Token: 0x06000934 RID: 2356 RVA: 0x00038E6C File Offset: 0x0003706C
		public string MaxPixelLane1
		{
			get
			{
				return this.maxPixelLane1;
			}
			set
			{
				bool flag = this.maxPixelLane1 != value;
				if (flag)
				{
					this.maxPixelLane1 = value;
					this.parHandling.WriteValue("/filter/acquisition/lane1/MaxPixel/Value", this.maxPixelLane1);
					this.parHandling.WriteValue("/defaultProduct/mainTable/filter/acquisition/lane1/MaxPixel/Value", this.maxPixelLane1);
					this.OnPropertyChanged("MaxPixelLane1");
				}
			}
		}

		// Token: 0x170002B5 RID: 693
		// (get) Token: 0x06000935 RID: 2357 RVA: 0x00038ED0 File Offset: 0x000370D0
		// (set) Token: 0x06000936 RID: 2358 RVA: 0x00038EE8 File Offset: 0x000370E8
		public string MaxPixelLane2
		{
			get
			{
				return this.maxPixelLane2;
			}
			set
			{
				bool flag = this.maxPixelLane2 != value;
				if (flag)
				{
					this.maxPixelLane2 = value;
					this.parHandling.WriteValue("/filter/acquisition/lane2/MaxPixel/Value", this.maxPixelLane2);
					this.parHandling.WriteValue("/defaultProduct/mainTable/filter/acquisition/lane2/MaxPixel/Value", this.maxPixelLane2);
					this.OnPropertyChanged("MaxPixelLane2");
				}
			}
		}

		// Token: 0x170002B6 RID: 694
		// (get) Token: 0x06000937 RID: 2359 RVA: 0x00038F4C File Offset: 0x0003714C
		// (set) Token: 0x06000938 RID: 2360 RVA: 0x00038F64 File Offset: 0x00037164
		public string MaxPixelLane3
		{
			get
			{
				return this.maxPixelLane3;
			}
			set
			{
				bool flag = this.maxPixelLane3 != value;
				if (flag)
				{
					this.maxPixelLane3 = value;
					this.parHandling.WriteValue("/filter/acquisition/lane3/MaxPixel/Value", this.maxPixelLane3);
					this.parHandling.WriteValue("/defaultProduct/mainTable/filter/acquisition/lane3/MaxPixel/Value", this.maxPixelLane3);
					this.OnPropertyChanged("MaxPixelLane3");
				}
			}
		}

		// Token: 0x170002B7 RID: 695
		// (get) Token: 0x06000939 RID: 2361 RVA: 0x00038FC8 File Offset: 0x000371C8
		// (set) Token: 0x0600093A RID: 2362 RVA: 0x00038FE0 File Offset: 0x000371E0
		public string MaxPixelLane4
		{
			get
			{
				return this.maxPixelLane4;
			}
			set
			{
				bool flag = this.maxPixelLane4 != value;
				if (flag)
				{
					this.maxPixelLane4 = value;
					this.parHandling.WriteValue("/filter/acquisition/lane4/MaxPixel/Value", this.maxPixelLane4);
					this.parHandling.WriteValue("/defaultProduct/mainTable/filter/acquisition/lane4/MaxPixel/Value", this.maxPixelLane4);
					this.OnPropertyChanged("MaxPixelLane4");
				}
			}
		}

		// Token: 0x170002B8 RID: 696
		// (get) Token: 0x0600093B RID: 2363 RVA: 0x00039044 File Offset: 0x00037244
		// (set) Token: 0x0600093C RID: 2364 RVA: 0x0003905C File Offset: 0x0003725C
		public string PixelPerLane
		{
			get
			{
				return this.pixelPerLane;
			}
			set
			{
				bool flag = this.pixelPerLane != value;
				if (flag)
				{
					this.pixelPerLane = value;
					this.OnPropertyChanged("PixelPerLane");
				}
			}
		}

		// Token: 0x170002B9 RID: 697
		// (get) Token: 0x0600093D RID: 2365 RVA: 0x0003908F File Offset: 0x0003728F
		// (set) Token: 0x0600093E RID: 2366 RVA: 0x00039097 File Offset: 0x00037297
		public string IgnoreLeft { get; set; }

		// Token: 0x170002BA RID: 698
		// (get) Token: 0x0600093F RID: 2367 RVA: 0x000390A0 File Offset: 0x000372A0
		// (set) Token: 0x06000940 RID: 2368 RVA: 0x000390A8 File Offset: 0x000372A8
		public string IgnoreRight { get; set; }

		// Token: 0x170002BB RID: 699
		// (get) Token: 0x06000941 RID: 2369 RVA: 0x000390B1 File Offset: 0x000372B1
		// (set) Token: 0x06000942 RID: 2370 RVA: 0x000390B9 File Offset: 0x000372B9
		public string PixelCount { get; set; }

		// Token: 0x06000943 RID: 2371 RVA: 0x000390C4 File Offset: 0x000372C4
		public ComissioningMultilaneViewModel(DBConnection dbConnection, ParameterHandling parHandling)
		{
			this.parHandling = parHandling;
			this.dbConnection = dbConnection;
			this.CleanUpCommand = new RelayCommand(new Action<object>(this.CleanUp));
			this.StartWritingLaneParamCommand = new RelayCommand(new Action<object>(this.StartWritingLaneParams));
			this.btnStartWritingLaneParams = new Button();
			this.Init();
		}

		// Token: 0x06000944 RID: 2372 RVA: 0x00039128 File Offset: 0x00037328
		private void Init()
		{
			int value = 0;
			int value2 = 0;
			string path = Definition.detectorSesotecNPixelPath;
			DetectorType detectorType = DetectorType.Sesotec;
			this.dbConnection.GetDetectorType(ref detectorType);
			bool flag = detectorType == DetectorType.DTNetDE || detectorType == DetectorType.DTNetTDI;
			if (flag)
			{
				path = Definition.detectorDTNPixelPath;
			}
			this.PixelCount = this.parHandling.ReadPar(path);
			bool flag2 = Convert.ToInt32(this.PixelCount) < 0;
			if (flag2)
			{
				this.PixelCount = 10239.ToString();
			}
			this.LaneCountList = new List<string>();
			this.LaneCountList.Add("1");
			this.LaneCountList.Add("2");
			this.LaneCountList.Add("3");
			this.LaneCountList.Add("4");
			this.dbConnection.GetIgnoredLeftPixel(ref value);
			this.dbConnection.GetIgnoredRightPixel(ref value2);
			this.IgnoreLeft = Convert.ToString(value);
			this.IgnoreRight = Convert.ToString(value2);
			this.SelectedLane = this.SetValueSelectedLanes();
			this.calculatePixelPerLane();
			this.minPixelLane1 = this.parHandling.ReadPar("/filter/acquisition/lane1/MinPixel/Value");
			this.minPixelLane2 = this.parHandling.ReadPar("/filter/acquisition/lane2/MinPixel/Value");
			this.minPixelLane3 = this.parHandling.ReadPar("/filter/acquisition/lane3/MinPixel/Value");
			this.minPixelLane4 = this.parHandling.ReadPar("/filter/acquisition/lane4/MinPixel/Value");
			this.maxPixelLane1 = this.parHandling.ReadPar("/filter/acquisition/lane1/MaxPixel/Value");
			this.maxPixelLane2 = this.parHandling.ReadPar("/filter/acquisition/lane2/MaxPixel/Value");
			this.maxPixelLane3 = this.parHandling.ReadPar("/filter/acquisition/lane3/MaxPixel/Value");
			this.maxPixelLane4 = this.parHandling.ReadPar("/filter/acquisition/lane4/MaxPixel/Value");
		}

		// Token: 0x06000945 RID: 2373 RVA: 0x000392F0 File Offset: 0x000374F0
		private void StartWritingLaneParams(object obj)
		{
			this.calculatePixelPerLane();
			bool flag = this.SelectedLane == "1";
			if (flag)
			{
				this.writeProductAndDefaultParam("1", "0", "0", "0");
			}
			else
			{
				bool flag2 = this.SelectedLane == "2";
				if (flag2)
				{
					this.writeProductAndDefaultParam("1", "1", "0", "0");
				}
				else
				{
					bool flag3 = this.SelectedLane == "3";
					if (flag3)
					{
						this.writeProductAndDefaultParam("1", "1", "1", "0");
					}
					else
					{
						bool flag4 = this.SelectedLane == "4";
						if (flag4)
						{
							this.writeProductAndDefaultParam("1", "1", "1", "1");
						}
					}
				}
			}
			this.calculateMinMaxPixel();
			string message = LocalizationManager.Localize("txtParameterLoadedSuccess");
			Dialog.Show(LocalizationManager.Localize("txtSuccess"), message, DialogStyle.Ok, DialogButtons.OK);
		}

		// Token: 0x06000946 RID: 2374 RVA: 0x000393FC File Offset: 0x000375FC
		private void calculatePixelPerLane()
		{
			bool flag = this.SelectedLane != "" && this.PixelCount != "" && this.IgnoreLeft != "" && this.IgnoreRight != "";
			if (flag)
			{
				double num = Convert.ToDouble(this.SelectedLane);
				double num2 = Convert.ToDouble(this.PixelCount);
				double num3 = Convert.ToDouble(this.IgnoreLeft);
				double num4 = Convert.ToDouble(this.IgnoreRight);
				double value = 0.0;
				bool flag2 = num > 0.0;
				if (flag2)
				{
					value = Math.Round((num2 - num3 - num4) / num, 0);
				}
				this.PixelPerLane = Convert.ToString(value);
			}
		}

		// Token: 0x06000947 RID: 2375 RVA: 0x000394C8 File Offset: 0x000376C8
		private void calculateMinMaxPixel()
		{
			double num = Convert.ToDouble(this.SelectedLane);
			double num2 = Convert.ToDouble(this.PixelCount);
			double num3 = Convert.ToDouble(this.IgnoreLeft);
			double num4 = Convert.ToDouble(this.PixelPerLane);
			bool flag = num > 0.0;
			if (flag)
			{
				bool flag2 = num == 1.0;
				if (flag2)
				{
					this.MaxPixelLane1 = Convert.ToString(num2 - 1.0);
					this.MinPixelLane1 = "0";
					this.MaxPixelLane2 = "0";
					this.MinPixelLane2 = "0";
					this.MaxPixelLane3 = "0";
					this.MinPixelLane3 = "0";
					this.MaxPixelLane4 = "0";
					this.MinPixelLane4 = "0";
				}
				else
				{
					bool flag3 = num == 2.0;
					if (flag3)
					{
						this.MaxPixelLane1 = Convert.ToString(num3 + num4 - 1.0);
						this.MinPixelLane1 = "0";
						this.MaxPixelLane2 = Convert.ToString(num2 - 1.0);
						this.MinPixelLane2 = Convert.ToString(num3 + num4);
						this.MaxPixelLane3 = "0";
						this.MinPixelLane3 = "0";
						this.MaxPixelLane4 = "0";
						this.MinPixelLane4 = "0";
					}
					else
					{
						bool flag4 = num == 3.0;
						if (flag4)
						{
							this.MaxPixelLane1 = Convert.ToString(num3 + num4 - 1.0);
							this.MinPixelLane1 = "0";
							this.MaxPixelLane2 = Convert.ToString(num3 + 2.0 * num4 - 1.0);
							this.MinPixelLane2 = Convert.ToString(num3 + num4);
							this.MaxPixelLane3 = Convert.ToString(num2 - 1.0);
							this.MinPixelLane3 = Convert.ToString(num3 + 2.0 * num4);
							this.MaxPixelLane4 = "0";
							this.MinPixelLane4 = "0";
						}
						else
						{
							bool flag5 = num == 4.0;
							if (flag5)
							{
								this.MaxPixelLane1 = Convert.ToString(num3 + num4 - 1.0);
								this.MinPixelLane1 = "0";
								this.MaxPixelLane2 = Convert.ToString(num3 + 2.0 * num4 - 1.0);
								this.MinPixelLane2 = Convert.ToString(num3 + num4);
								this.MaxPixelLane3 = Convert.ToString(num3 + 3.0 * num4 - 1.0);
								this.MinPixelLane3 = Convert.ToString(num3 + 2.0 * num4);
								this.MaxPixelLane4 = Convert.ToString(num2 - 1.0);
								this.MinPixelLane4 = Convert.ToString(num3 + 3.0 * num4);
							}
							else
							{
								this.MaxPixelLane1 = "0";
								this.MinPixelLane1 = "0";
								this.MaxPixelLane2 = "0";
								this.MinPixelLane2 = "0";
								this.MaxPixelLane3 = "0";
								this.MinPixelLane3 = "0";
								this.MaxPixelLane4 = "0";
								this.MinPixelLane4 = "0";
							}
						}
					}
				}
			}
		}

		// Token: 0x06000948 RID: 2376 RVA: 0x00039834 File Offset: 0x00037A34
		private string SetValueSelectedLanes()
		{
			string a = this.parHandling.ReadPar("/filter/acquisition/lane1/Enable/Value");
			string a2 = this.parHandling.ReadPar("/filter/acquisition/lane2/Enable/Value");
			string a3 = this.parHandling.ReadPar("/filter/acquisition/lane3/Enable/Value");
			string a4 = this.parHandling.ReadPar("/filter/acquisition/lane4/Enable/Value");
			bool flag = a == "1" && a2 != "" && a3 != "" && a4 != "";
			string result;
			if (flag)
			{
				bool flag2 = a2 == "0" && a3 == "0" && a4 == "0";
				if (flag2)
				{
					result = "1";
				}
				else
				{
					bool flag3 = a2 == "1" && a3 == "0" && a4 == "0";
					if (flag3)
					{
						result = "2";
					}
					else
					{
						bool flag4 = a2 == "1" && a3 == "1" && a4 == "0";
						if (flag4)
						{
							result = "3";
						}
						else
						{
							bool flag5 = a2 == "1" && a3 == "1" && a4 == "1";
							if (flag5)
							{
								result = "4";
							}
							else
							{
								result = "1";
							}
						}
					}
				}
			}
			else
			{
				result = "1";
			}
			return result;
		}

		// Token: 0x06000949 RID: 2377 RVA: 0x000399C0 File Offset: 0x00037BC0
		private void writeProductAndDefaultParam(string val1, string val2, string val3, string val4)
		{
			this.parHandling.WriteValue("/filter/acquisition/lane1/Enable/Value", val1);
			this.parHandling.WriteValue("/defaultProduct/mainTable/filter/acquisition/lane1/Enable/Value", val1);
			this.parHandling.WriteValue("/filter/acquisition/lane2/Enable/Value", val2);
			this.parHandling.WriteValue("/defaultProduct/mainTable/filter/acquisition/lane2/Enable/Value", val2);
			this.parHandling.WriteValue("/filter/acquisition/lane3/Enable/Value", val3);
			this.parHandling.WriteValue("/defaultProduct/mainTable/filter/acquisition/lane3/Enable/Value", val3);
			this.parHandling.WriteValue("/filter/acquisition/lane4/Enable/Value", val4);
			this.parHandling.WriteValue("/defaultProduct/mainTable/filter/acquisition/lane4/Enable/Value", val4);
		}

		// Token: 0x0600094A RID: 2378 RVA: 0x00039A60 File Offset: 0x00037C60
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x0600094B RID: 2379 RVA: 0x0001092C File Offset: 0x0000EB2C
		private void CleanUp(object obj)
		{
		}

		// Token: 0x0400063A RID: 1594
		private ParameterHandling parHandling;

		// Token: 0x0400063B RID: 1595
		private DBConnection dbConnection;

		// Token: 0x0400063E RID: 1598
		public Button btnStartWritingLaneParams;

		// Token: 0x0400063F RID: 1599
		public RelayCommand StartWritingLaneParamCommand;

		// Token: 0x04000641 RID: 1601
		private const string lane1Enable = "/filter/acquisition/lane1/Enable/Value";

		// Token: 0x04000642 RID: 1602
		private const string lane2Enable = "/filter/acquisition/lane2/Enable/Value";

		// Token: 0x04000643 RID: 1603
		private const string lane3Enable = "/filter/acquisition/lane3/Enable/Value";

		// Token: 0x04000644 RID: 1604
		private const string lane4Enable = "/filter/acquisition/lane4/Enable/Value";

		// Token: 0x04000645 RID: 1605
		private const string lane1Min = "/filter/acquisition/lane1/MinPixel/Value";

		// Token: 0x04000646 RID: 1606
		private const string lane2Min = "/filter/acquisition/lane2/MinPixel/Value";

		// Token: 0x04000647 RID: 1607
		private const string lane3Min = "/filter/acquisition/lane3/MinPixel/Value";

		// Token: 0x04000648 RID: 1608
		private const string lane4Min = "/filter/acquisition/lane4/MinPixel/Value";

		// Token: 0x04000649 RID: 1609
		private const string lane1Max = "/filter/acquisition/lane1/MaxPixel/Value";

		// Token: 0x0400064A RID: 1610
		private const string lane2Max = "/filter/acquisition/lane2/MaxPixel/Value";

		// Token: 0x0400064B RID: 1611
		private const string lane3Max = "/filter/acquisition/lane3/MaxPixel/Value";

		// Token: 0x0400064C RID: 1612
		private const string lane4Max = "/filter/acquisition/lane4/MaxPixel/Value";

		// Token: 0x0400064D RID: 1613
		private const string defaultMain = "/defaultProduct/mainTable";

		// Token: 0x0400064E RID: 1614
		private string selectedLane;

		// Token: 0x0400064F RID: 1615
		private string minPixelLane1;

		// Token: 0x04000650 RID: 1616
		private string minPixelLane2;

		// Token: 0x04000651 RID: 1617
		private string minPixelLane3;

		// Token: 0x04000652 RID: 1618
		private string minPixelLane4;

		// Token: 0x04000653 RID: 1619
		private string maxPixelLane1;

		// Token: 0x04000654 RID: 1620
		private string maxPixelLane2;

		// Token: 0x04000655 RID: 1621
		private string maxPixelLane3;

		// Token: 0x04000656 RID: 1622
		private string maxPixelLane4;

		// Token: 0x04000657 RID: 1623
		private string pixelPerLane;
	}
}
