using System;
using System.Globalization;
using Common;
using DatabaseConnection;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x02000050 RID: 80
	public class SVTViewModel
	{
		// Token: 0x17000249 RID: 585
		// (get) Token: 0x060007F2 RID: 2034 RVA: 0x000337EB File Offset: 0x000319EB
		// (set) Token: 0x060007F3 RID: 2035 RVA: 0x000337F3 File Offset: 0x000319F3
		public SVT svt { get; set; }

		// Token: 0x1700024A RID: 586
		// (get) Token: 0x060007F4 RID: 2036 RVA: 0x000337FC File Offset: 0x000319FC
		// (set) Token: 0x060007F5 RID: 2037 RVA: 0x00033804 File Offset: 0x00031A04
		public bool MinOP1LoggedIn { get; set; }

		// Token: 0x1700024B RID: 587
		// (get) Token: 0x060007F6 RID: 2038 RVA: 0x0003380D File Offset: 0x00031A0D
		// (set) Token: 0x060007F7 RID: 2039 RVA: 0x00033815 File Offset: 0x00031A15
		public bool MinOP3LoggedIn { get; set; }

		// Token: 0x1700024C RID: 588
		// (get) Token: 0x060007F8 RID: 2040 RVA: 0x0003381E File Offset: 0x00031A1E
		// (set) Token: 0x060007F9 RID: 2041 RVA: 0x00033826 File Offset: 0x00031A26
		public bool MinAdminLoggedIn { get; set; }

		// Token: 0x1700024D RID: 589
		// (get) Token: 0x060007FA RID: 2042 RVA: 0x0003382F File Offset: 0x00031A2F
		// (set) Token: 0x060007FB RID: 2043 RVA: 0x00033837 File Offset: 0x00031A37
		public bool MinServiceLoggedIn { get; set; }

		// Token: 0x1700024E RID: 590
		// (get) Token: 0x060007FC RID: 2044 RVA: 0x00033840 File Offset: 0x00031A40
		// (set) Token: 0x060007FD RID: 2045 RVA: 0x00033848 File Offset: 0x00031A48
		public RelayCommand SetDelayCommand { get; set; }

		// Token: 0x1700024F RID: 591
		// (get) Token: 0x060007FE RID: 2046 RVA: 0x00033851 File Offset: 0x00031A51
		// (set) Token: 0x060007FF RID: 2047 RVA: 0x00033859 File Offset: 0x00031A59
		public RelayCommand SetEjectTimeCommand { get; set; }

		// Token: 0x17000250 RID: 592
		// (get) Token: 0x06000800 RID: 2048 RVA: 0x00033862 File Offset: 0x00031A62
		// (set) Token: 0x06000801 RID: 2049 RVA: 0x0003386A File Offset: 0x00031A6A
		public RelayCommand SetStartPixelCommand { get; set; }

		// Token: 0x17000251 RID: 593
		// (get) Token: 0x06000802 RID: 2050 RVA: 0x00033873 File Offset: 0x00031A73
		// (set) Token: 0x06000803 RID: 2051 RVA: 0x0003387B File Offset: 0x00031A7B
		public RelayCommand SetValveWidthCommand { get; set; }

		// Token: 0x17000252 RID: 594
		// (get) Token: 0x06000804 RID: 2052 RVA: 0x00033884 File Offset: 0x00031A84
		// (set) Token: 0x06000805 RID: 2053 RVA: 0x0003388C File Offset: 0x00031A8C
		public RelayCommand SetOverlappingLeftCommand { get; set; }

		// Token: 0x17000253 RID: 595
		// (get) Token: 0x06000806 RID: 2054 RVA: 0x00033895 File Offset: 0x00031A95
		// (set) Token: 0x06000807 RID: 2055 RVA: 0x0003389D File Offset: 0x00031A9D
		public RelayCommand SetOverlappingRightCommand { get; set; }

		// Token: 0x17000254 RID: 596
		// (get) Token: 0x06000808 RID: 2056 RVA: 0x000338A6 File Offset: 0x00031AA6
		// (set) Token: 0x06000809 RID: 2057 RVA: 0x000338AE File Offset: 0x00031AAE
		public RelayCommand SetValveTestStartValveCommand { get; set; }

		// Token: 0x17000255 RID: 597
		// (get) Token: 0x0600080A RID: 2058 RVA: 0x000338B7 File Offset: 0x00031AB7
		// (set) Token: 0x0600080B RID: 2059 RVA: 0x000338BF File Offset: 0x00031ABF
		public RelayCommand SetValveTestEndValveCommand { get; set; }

		// Token: 0x17000256 RID: 598
		// (get) Token: 0x0600080C RID: 2060 RVA: 0x000338C8 File Offset: 0x00031AC8
		// (set) Token: 0x0600080D RID: 2061 RVA: 0x000338D0 File Offset: 0x00031AD0
		public RelayCommand SetTestIntervalCommand { get; set; }

		// Token: 0x17000257 RID: 599
		// (get) Token: 0x0600080E RID: 2062 RVA: 0x000338D9 File Offset: 0x00031AD9
		// (set) Token: 0x0600080F RID: 2063 RVA: 0x000338E1 File Offset: 0x00031AE1
		public RelayCommand CleanUpCommand { get; set; }

		// Token: 0x17000258 RID: 600
		// (get) Token: 0x06000810 RID: 2064 RVA: 0x000338EA File Offset: 0x00031AEA
		// (set) Token: 0x06000811 RID: 2065 RVA: 0x000338F2 File Offset: 0x00031AF2
		public RelayCommand SetNumValvesCommand { get; set; }

		// Token: 0x17000259 RID: 601
		// (get) Token: 0x06000812 RID: 2066 RVA: 0x000338FB File Offset: 0x00031AFB
		// (set) Token: 0x06000813 RID: 2067 RVA: 0x00033903 File Offset: 0x00031B03
		public RelayCommand SetFirstValveCommand { get; set; }

		// Token: 0x1700025A RID: 602
		// (get) Token: 0x06000814 RID: 2068 RVA: 0x0003390C File Offset: 0x00031B0C
		// (set) Token: 0x06000815 RID: 2069 RVA: 0x00033914 File Offset: 0x00031B14
		public RelayCommand SetStartPixelSecondValveCommand { get; set; }

		// Token: 0x1700025B RID: 603
		// (get) Token: 0x06000816 RID: 2070 RVA: 0x0003391D File Offset: 0x00031B1D
		// (set) Token: 0x06000817 RID: 2071 RVA: 0x00033925 File Offset: 0x00031B25
		public RelayCommand SetImageSizeCommand { get; set; }

		// Token: 0x1700025C RID: 604
		// (get) Token: 0x06000818 RID: 2072 RVA: 0x0003392E File Offset: 0x00031B2E
		// (set) Token: 0x06000819 RID: 2073 RVA: 0x00033936 File Offset: 0x00031B36
		public RelayCommand SetEndPixelNextToLastValveCommand { get; set; }

		// Token: 0x1700025D RID: 605
		// (get) Token: 0x0600081A RID: 2074 RVA: 0x0003393F File Offset: 0x00031B3F
		// (set) Token: 0x0600081B RID: 2075 RVA: 0x00033947 File Offset: 0x00031B47
		public BoundLocalizedText StrEjectTime { get; set; }

		// Token: 0x1700025E RID: 606
		// (get) Token: 0x0600081C RID: 2076 RVA: 0x00033950 File Offset: 0x00031B50
		// (set) Token: 0x0600081D RID: 2077 RVA: 0x00033958 File Offset: 0x00031B58
		public BoundLocalizedText StrDelay { get; set; }

		// Token: 0x1700025F RID: 607
		// (get) Token: 0x0600081E RID: 2078 RVA: 0x00033961 File Offset: 0x00031B61
		// (set) Token: 0x0600081F RID: 2079 RVA: 0x00033969 File Offset: 0x00031B69
		public BoundLocalizedText StrRejecterEnabled { get; set; }

		// Token: 0x17000260 RID: 608
		// (get) Token: 0x06000820 RID: 2080 RVA: 0x00033972 File Offset: 0x00031B72
		// (set) Token: 0x06000821 RID: 2081 RVA: 0x0003397A File Offset: 0x00031B7A
		public BoundLocalizedText StrValveStartPixel { get; set; }

		// Token: 0x17000261 RID: 609
		// (get) Token: 0x06000822 RID: 2082 RVA: 0x00033983 File Offset: 0x00031B83
		// (set) Token: 0x06000823 RID: 2083 RVA: 0x0003398B File Offset: 0x00031B8B
		public BoundLocalizedText StrValveWidth { get; set; }

		// Token: 0x17000262 RID: 610
		// (get) Token: 0x06000824 RID: 2084 RVA: 0x00033994 File Offset: 0x00031B94
		// (set) Token: 0x06000825 RID: 2085 RVA: 0x0003399C File Offset: 0x00031B9C
		public BoundLocalizedText StrOverlappingLeft { get; set; }

		// Token: 0x17000263 RID: 611
		// (get) Token: 0x06000826 RID: 2086 RVA: 0x000339A5 File Offset: 0x00031BA5
		// (set) Token: 0x06000827 RID: 2087 RVA: 0x000339AD File Offset: 0x00031BAD
		public BoundLocalizedText StrOverlappingRight { get; set; }

		// Token: 0x17000264 RID: 612
		// (get) Token: 0x06000828 RID: 2088 RVA: 0x000339B6 File Offset: 0x00031BB6
		// (set) Token: 0x06000829 RID: 2089 RVA: 0x000339BE File Offset: 0x00031BBE
		public BoundLocalizedText StrStartValveTest { get; set; }

		// Token: 0x17000265 RID: 613
		// (get) Token: 0x0600082A RID: 2090 RVA: 0x000339C7 File Offset: 0x00031BC7
		// (set) Token: 0x0600082B RID: 2091 RVA: 0x000339CF File Offset: 0x00031BCF
		public BoundLocalizedText StrFirstValve { get; set; }

		// Token: 0x17000266 RID: 614
		// (get) Token: 0x0600082C RID: 2092 RVA: 0x000339D8 File Offset: 0x00031BD8
		// (set) Token: 0x0600082D RID: 2093 RVA: 0x000339E0 File Offset: 0x00031BE0
		public BoundLocalizedText StrLastValve { get; set; }

		// Token: 0x17000267 RID: 615
		// (get) Token: 0x0600082E RID: 2094 RVA: 0x000339E9 File Offset: 0x00031BE9
		// (set) Token: 0x0600082F RID: 2095 RVA: 0x000339F1 File Offset: 0x00031BF1
		public BoundLocalizedText StrInterval { get; set; }

		// Token: 0x17000268 RID: 616
		// (get) Token: 0x06000830 RID: 2096 RVA: 0x000339FA File Offset: 0x00031BFA
		// (set) Token: 0x06000831 RID: 2097 RVA: 0x00033A02 File Offset: 0x00031C02
		public BoundLocalizedText StrNumValves { get; set; }

		// Token: 0x17000269 RID: 617
		// (get) Token: 0x06000832 RID: 2098 RVA: 0x00033A0B File Offset: 0x00031C0B
		// (set) Token: 0x06000833 RID: 2099 RVA: 0x00033A13 File Offset: 0x00031C13
		public BoundLocalizedText StrValvesInverted { get; set; }

		// Token: 0x1700026A RID: 618
		// (get) Token: 0x06000834 RID: 2100 RVA: 0x00033A1C File Offset: 0x00031C1C
		// (set) Token: 0x06000835 RID: 2101 RVA: 0x00033A24 File Offset: 0x00031C24
		public BoundLocalizedText StrFirstPixelSecondValve { get; set; }

		// Token: 0x1700026B RID: 619
		// (get) Token: 0x06000836 RID: 2102 RVA: 0x00033A2D File Offset: 0x00031C2D
		// (set) Token: 0x06000837 RID: 2103 RVA: 0x00033A35 File Offset: 0x00031C35
		public BoundLocalizedText StrEndPixelNextToLastValve { get; set; }

		// Token: 0x1700026C RID: 620
		// (get) Token: 0x06000838 RID: 2104 RVA: 0x00033A3E File Offset: 0x00031C3E
		// (set) Token: 0x06000839 RID: 2105 RVA: 0x00033A46 File Offset: 0x00031C46
		public RelayCommand StartValveTestCommand { get; set; }

		// Token: 0x1700026D RID: 621
		// (get) Token: 0x0600083A RID: 2106 RVA: 0x00033A4F File Offset: 0x00031C4F
		// (set) Token: 0x0600083B RID: 2107 RVA: 0x00033A57 File Offset: 0x00031C57
		public BoundLocalizedText StrImageSize { get; set; }

		// Token: 0x1700026E RID: 622
		// (get) Token: 0x0600083C RID: 2108 RVA: 0x00033A60 File Offset: 0x00031C60
		// (set) Token: 0x0600083D RID: 2109 RVA: 0x00033A68 File Offset: 0x00031C68
		public BoundLocalizedText StrEjectTimeInit { get; set; }

		// Token: 0x0600083E RID: 2110 RVA: 0x00033A71 File Offset: 0x00031C71
		public SVTViewModel(ParameterHandling parHandling, DBConnection dbConnection, MachineState machineState, AccessRightsManager accessManager, ImageDataTransfer imageDataTransfer, DBDataLogs dbDataLogs) : this(parHandling, dbConnection, machineState, accessManager, true, imageDataTransfer, dbDataLogs)
		{
		}

		// Token: 0x0600083F RID: 2111 RVA: 0x00033A88 File Offset: 0x00031C88
		public SVTViewModel(ParameterHandling parHandling, DBConnection dbConnection, MachineState machineState, AccessRightsManager accessManager, bool showHazardMessage, ImageDataTransfer imageTransfer, DBDataLogs dbDataLogs)
		{
			this.accessManager = accessManager;
			this.parHandling = parHandling;
			this.machineState = machineState;
			this.dbConnection = dbConnection;
			this.imageTransfer = imageTransfer;
			this.SetDelayCommand = new RelayCommand(new Action<object>(this.SetDelay));
			this.SetEjectTimeCommand = new RelayCommand(new Action<object>(this.SetEjectTime));
			this.SetStartPixelCommand = new RelayCommand(new Action<object>(this.SetStartPixel));
			this.SetValveWidthCommand = new RelayCommand(new Action<object>(this.SetValveWidth));
			this.SetOverlappingLeftCommand = new RelayCommand(new Action<object>(this.SetOverlappingLeft));
			this.SetOverlappingRightCommand = new RelayCommand(new Action<object>(this.SetOverlappingRight));
			this.SetValveTestStartValveCommand = new RelayCommand(new Action<object>(this.SetValveTestStartValve));
			this.SetValveTestEndValveCommand = new RelayCommand(new Action<object>(this.SetValveTestEndValve));
			this.SetTestIntervalCommand = new RelayCommand(new Action<object>(this.SetTestInterval));
			this.CleanUpCommand = new RelayCommand(new Action<object>(this.CleanUp));
			this.SetNumValvesCommand = new RelayCommand(new Action<object>(this.SetNumValves));
			this.SetFirstValveCommand = new RelayCommand(new Action<object>(this.SetFirstValve));
			this.SetEndPixelNextToLastValveCommand = new RelayCommand(new Action<object>(this.SetEndPixelNextToLastValve));
			this.SetStartPixelSecondValveCommand = new RelayCommand(new Action<object>(this.SetStartPixelSecondValve));
			this.SetImageSizeCommand = new RelayCommand(new Action<object>(this.SetImageSize));
			this.MinOP1LoggedIn = (accessManager.CurrentUser.Rights >= BuiltInPriviledges.Operator1);
			this.MinOP3LoggedIn = (accessManager.CurrentUser.Rights >= BuiltInPriviledges.Operator3);
			this.MinAdminLoggedIn = (accessManager.CurrentUser.Rights >= BuiltInPriviledges.Admin);
			this.MinServiceLoggedIn = (accessManager.CurrentUser.Rights >= BuiltInPriviledges.Service);
			this.svt = new SVT(parHandling, dbConnection, machineState, showHazardMessage, dbDataLogs);
			this.StartValveTestCommand = new RelayCommand(new Action<object>(this.StartValveTest));
			this.RegisterLocalizedText();
		}

		// Token: 0x06000840 RID: 2112 RVA: 0x00033CD0 File Offset: 0x00031ED0
		private void SVT_SVTTestEnded()
		{
			this.svt.EndValveTest();
		}

		// Token: 0x06000841 RID: 2113 RVA: 0x00033CDF File Offset: 0x00031EDF
		private void StartValveTest(object obj)
		{
			this.svt.StartOrStopValveTest();
		}

		// Token: 0x06000842 RID: 2114 RVA: 0x00033CF0 File Offset: 0x00031EF0
		private void RegisterLocalizedText()
		{
			this.StrEjectTime = new BoundLocalizedText("txtEjectTimeAdditional");
			this.StrDelay = new BoundLocalizedText("txtDelay");
			this.StrRejecterEnabled = new BoundLocalizedText("txtRejecterEnabled");
			this.StrValveStartPixel = new BoundLocalizedText("txtValveStartPixel");
			this.StrValveWidth = new BoundLocalizedText("txtValveWidth");
			this.StrOverlappingLeft = new BoundLocalizedText("txtOverlappingLeft");
			this.StrOverlappingRight = new BoundLocalizedText("txtOverlappingRight");
			bool flag = this.parHandling.ReadPar(SVT.svtPeriodicModePath) != "0";
			if (flag)
			{
				this.StrStartValveTest = new BoundLocalizedText("txtStopValveTest");
			}
			else
			{
				this.StrStartValveTest = new BoundLocalizedText("txtStartValveTest");
			}
			this.StrFirstValve = new BoundLocalizedText("txtFirstValve");
			this.StrLastValve = new BoundLocalizedText("txtLastValve");
			this.StrInterval = new BoundLocalizedText("txtInterval");
			this.StrNumValves = new BoundLocalizedText("txtNumValves");
			this.StrValvesInverted = new BoundLocalizedText("txtValvesInverted");
			this.StrFirstPixelSecondValve = new BoundLocalizedText("txtFirstPixelSecondValve");
			this.StrEndPixelNextToLastValve = new BoundLocalizedText("txtLastPixelNextToLastValve");
			this.StrImageSize = new BoundLocalizedText("txtImageSize");
			this.StrEjectTimeInit = new BoundLocalizedText("txtEjectTimeInit");
		}

		// Token: 0x06000843 RID: 2115 RVA: 0x00033E50 File Offset: 0x00032050
		private void CleanUp(object obj)
		{
			MainWindow.localizationManager.Deregister(this.StrEjectTime);
			MainWindow.localizationManager.Deregister(this.StrDelay);
			MainWindow.localizationManager.Deregister(this.StrRejecterEnabled);
			MainWindow.localizationManager.Deregister(this.StrValveStartPixel);
			MainWindow.localizationManager.Deregister(this.StrValveWidth);
			MainWindow.localizationManager.Deregister(this.StrOverlappingLeft);
			MainWindow.localizationManager.Deregister(this.StrOverlappingRight);
			MainWindow.localizationManager.Deregister(this.StrStartValveTest);
			MainWindow.localizationManager.Deregister(this.StrFirstValve);
			MainWindow.localizationManager.Deregister(this.StrLastValve);
			MainWindow.localizationManager.Deregister(this.StrInterval);
			MainWindow.localizationManager.Deregister(this.StrNumValves);
			MainWindow.localizationManager.Deregister(this.StrValvesInverted);
			MainWindow.localizationManager.Deregister(this.StrFirstPixelSecondValve);
			MainWindow.localizationManager.Deregister(this.StrEndPixelNextToLastValve);
			MainWindow.localizationManager.Deregister(this.StrImageSize);
			MainWindow.localizationManager.Deregister(this.StrEjectTimeInit);
			this.parHandling.WriteValue(SVT.valveTestEnablePath, "0", false);
			this.parHandling.WriteValue(SVT.svtPeriodicModePath, "0", false);
		}

		// Token: 0x06000844 RID: 2116 RVA: 0x00033FB0 File Offset: 0x000321B0
		private void SetDelay(object obj)
		{
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtDelay"), this.svt.Delay.ToString(), NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_SetDelay;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x06000845 RID: 2117 RVA: 0x0003400C File Offset: 0x0003220C
		private void numKeyboard_SetDelay(object sender, KeyboardInputEventArgs e)
		{
			try
			{
				this.svt.Delay = int.Parse(e.Text);
			}
			catch
			{
			}
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_SetDelay;
		}

		// Token: 0x06000846 RID: 2118 RVA: 0x00034064 File Offset: 0x00032264
		private void SetEjectTime(object obj)
		{
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtEjectTime"), this.svt.EjectTime.ToString(), NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_SetEjectTime;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x06000847 RID: 2119 RVA: 0x000340C0 File Offset: 0x000322C0
		private void numKeyboard_SetEjectTime(object sender, KeyboardInputEventArgs e)
		{
			try
			{
				this.svt.EjectTime = int.Parse(e.Text);
			}
			catch
			{
			}
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_SetEjectTime;
		}

		// Token: 0x06000848 RID: 2120 RVA: 0x00034118 File Offset: 0x00032318
		private void SetStartPixel(object obj)
		{
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtValveStartPixel"), this.svt.StartPixel.ToString(), NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_SetStartPixel;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x06000849 RID: 2121 RVA: 0x00034174 File Offset: 0x00032374
		private void numKeyboard_SetStartPixel(object sender, KeyboardInputEventArgs e)
		{
			try
			{
				this.svt.StartPixel = int.Parse(e.Text);
			}
			catch
			{
			}
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_SetStartPixel;
		}

		// Token: 0x0600084A RID: 2122 RVA: 0x000341CC File Offset: 0x000323CC
		private void SetValveWidth(object obj)
		{
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtValveWidth"), this.svt.ValveWidth.ToString(), NumKeyboardType.TypeDouble);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_SetValveWidth;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x0600084B RID: 2123 RVA: 0x00034228 File Offset: 0x00032428
		private void numKeyboard_SetValveWidth(object sender, KeyboardInputEventArgs e)
		{
			try
			{
				this.svt.ValveWidth = double.Parse(e.Text.Replace(',', '.'), new CultureInfo("en-US"));
			}
			catch
			{
			}
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_SetValveWidth;
		}

		// Token: 0x0600084C RID: 2124 RVA: 0x00034294 File Offset: 0x00032494
		private void SetOverlappingLeft(object obj)
		{
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtOverlappingLeft"), this.svt.OverlappingLeft.ToString(), NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_SetOverlappingLeft;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x0600084D RID: 2125 RVA: 0x000342F0 File Offset: 0x000324F0
		private void numKeyboard_SetOverlappingLeft(object sender, KeyboardInputEventArgs e)
		{
			try
			{
				this.svt.OverlappingLeft = int.Parse(e.Text);
			}
			catch
			{
			}
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_SetOverlappingLeft;
		}

		// Token: 0x0600084E RID: 2126 RVA: 0x00034348 File Offset: 0x00032548
		private void SetOverlappingRight(object obj)
		{
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtOverlappingRight"), this.svt.OverlappingRight.ToString(), NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_SetOverlappingRight;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x0600084F RID: 2127 RVA: 0x000343A4 File Offset: 0x000325A4
		private void numKeyboard_SetOverlappingRight(object sender, KeyboardInputEventArgs e)
		{
			try
			{
				this.svt.OverlappingRight = int.Parse(e.Text);
			}
			catch
			{
			}
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_SetOverlappingRight;
		}

		// Token: 0x06000850 RID: 2128 RVA: 0x000343FC File Offset: 0x000325FC
		private void SetValveTestStartValve(object obj)
		{
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtFirstValve"), this.svt.ValveTestStartValve.ToString(), NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_SetValveTestStartValve;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x06000851 RID: 2129 RVA: 0x00034458 File Offset: 0x00032658
		private void numKeyboard_SetValveTestStartValve(object sender, KeyboardInputEventArgs e)
		{
			try
			{
				this.svt.ValveTestStartValve = int.Parse(e.Text);
			}
			catch
			{
			}
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_SetValveTestStartValve;
		}

		// Token: 0x06000852 RID: 2130 RVA: 0x000344B0 File Offset: 0x000326B0
		private void SetValveTestEndValve(object obj)
		{
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtLastValve"), this.svt.ValveTestEndValve.ToString(), NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_SetValveTestEndValve;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x06000853 RID: 2131 RVA: 0x0003450C File Offset: 0x0003270C
		private void numKeyboard_SetValveTestEndValve(object sender, KeyboardInputEventArgs e)
		{
			try
			{
				this.svt.ValveTestEndValve = int.Parse(e.Text);
			}
			catch
			{
			}
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_SetValveTestEndValve;
		}

		// Token: 0x06000854 RID: 2132 RVA: 0x00034564 File Offset: 0x00032764
		private void SetTestInterval(object obj)
		{
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtInterval"), this.svt.TestInterval.ToString(), NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_SetTestInterval;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x06000855 RID: 2133 RVA: 0x000345C0 File Offset: 0x000327C0
		private void numKeyboard_SetTestInterval(object sender, KeyboardInputEventArgs e)
		{
			try
			{
				this.svt.TestInterval = int.Parse(e.Text);
			}
			catch
			{
			}
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_SetTestInterval;
		}

		// Token: 0x06000856 RID: 2134 RVA: 0x00034618 File Offset: 0x00032818
		private void SetNumValves(object obj)
		{
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtNumValves"), this.svt.NumValves.ToString(), NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_SetNumValves;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x06000857 RID: 2135 RVA: 0x00034674 File Offset: 0x00032874
		private void numKeyboard_SetNumValves(object sender, KeyboardInputEventArgs e)
		{
			try
			{
				this.svt.NumValves = int.Parse(e.Text);
			}
			catch
			{
			}
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_SetNumValves;
		}

		// Token: 0x06000858 RID: 2136 RVA: 0x000346CC File Offset: 0x000328CC
		private void SetFirstValve(object obj)
		{
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtFirstValve"), this.svt.FirstValve.ToString(), NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_SetFirstValve;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x06000859 RID: 2137 RVA: 0x00034728 File Offset: 0x00032928
		private void numKeyboard_SetFirstValve(object sender, KeyboardInputEventArgs e)
		{
			try
			{
				this.svt.FirstValve = int.Parse(e.Text);
			}
			catch
			{
			}
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_SetFirstValve;
		}

		// Token: 0x0600085A RID: 2138 RVA: 0x00034780 File Offset: 0x00032980
		private void SetStartPixelSecondValve(object obj)
		{
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtValveStartPixel"), this.svt.StartPixelSecondValve.ToString(), NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_SetStartPixelSecondValve;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x0600085B RID: 2139 RVA: 0x000347DC File Offset: 0x000329DC
		private void numKeyboard_SetStartPixelSecondValve(object sender, KeyboardInputEventArgs e)
		{
			try
			{
				this.svt.StartPixelSecondValve = int.Parse(e.Text);
			}
			catch
			{
			}
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_SetStartPixelSecondValve;
		}

		// Token: 0x0600085C RID: 2140 RVA: 0x00034834 File Offset: 0x00032A34
		private void SetEndPixelNextToLastValve(object obj)
		{
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtValveStartPixel"), this.svt.EndPixelNextToLastValve.ToString(), NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_SetEndPixelNextToLastValve;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x0600085D RID: 2141 RVA: 0x00034890 File Offset: 0x00032A90
		private void numKeyboard_SetEndPixelNextToLastValve(object sender, KeyboardInputEventArgs e)
		{
			try
			{
				this.svt.EndPixelNextToLastValve = int.Parse(e.Text);
			}
			catch
			{
			}
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_SetEndPixelNextToLastValve;
		}

		// Token: 0x0600085E RID: 2142 RVA: 0x000348E8 File Offset: 0x00032AE8
		private void SetImageSize(object obj)
		{
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtImageSize"), this.svt.ImageSize.ToString(), NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_SetImageSize;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x0600085F RID: 2143 RVA: 0x00034944 File Offset: 0x00032B44
		private void numKeyboard_SetImageSize(object sender, KeyboardInputEventArgs e)
		{
			try
			{
				this.svt.ImageSize = int.Parse(e.Text);
			}
			catch
			{
			}
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_SetImageSize;
		}

		// Token: 0x06000860 RID: 2144 RVA: 0x0003499C File Offset: 0x00032B9C
		public void Update()
		{
			this.svt.Update();
		}

		// Token: 0x04000583 RID: 1411
		private NumKeyboard numKeyboard;

		// Token: 0x04000584 RID: 1412
		private AccessRightsManager accessManager;

		// Token: 0x04000585 RID: 1413
		private ParameterHandling parHandling;

		// Token: 0x0400059A RID: 1434
		public RelayCommand ShowBulkCommand;

		// Token: 0x040005AD RID: 1453
		private DBConnection dbConnection;

		// Token: 0x040005AE RID: 1454
		private MachineState machineState;

		// Token: 0x040005AF RID: 1455
		private ImageDataTransfer imageTransfer;
	}
}
