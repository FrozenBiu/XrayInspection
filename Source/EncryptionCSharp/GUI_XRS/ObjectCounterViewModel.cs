using System;
using System.Windows;
using Common;
using DatabaseConnection;
using Definitions;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x02000236 RID: 566
	public class ObjectCounterViewModel
	{
		// Token: 0x17000BC0 RID: 3008
		// (get) Token: 0x06002B06 RID: 11014 RVA: 0x0011E0E7 File Offset: 0x0011C2E7
		// (set) Token: 0x06002B07 RID: 11015 RVA: 0x0011E0EF File Offset: 0x0011C2EF
		public ObjectCounter ObjCounter { get; set; }

		// Token: 0x17000BC1 RID: 3009
		// (get) Token: 0x06002B08 RID: 11016 RVA: 0x0011E0F8 File Offset: 0x0011C2F8
		// (set) Token: 0x06002B09 RID: 11017 RVA: 0x0011E100 File Offset: 0x0011C300
		public RelayCommand ChangeMinCountCommand { get; set; }

		// Token: 0x17000BC2 RID: 3010
		// (get) Token: 0x06002B0A RID: 11018 RVA: 0x0011E109 File Offset: 0x0011C309
		// (set) Token: 0x06002B0B RID: 11019 RVA: 0x0011E111 File Offset: 0x0011C311
		public RelayCommand ChangeMaxCountCommand { get; set; }

		// Token: 0x17000BC3 RID: 3011
		// (get) Token: 0x06002B0C RID: 11020 RVA: 0x0011E11A File Offset: 0x0011C31A
		// (set) Token: 0x06002B0D RID: 11021 RVA: 0x0011E122 File Offset: 0x0011C322
		public RelayCommand ChangeLowThresholdCommand { get; set; }

		// Token: 0x17000BC4 RID: 3012
		// (get) Token: 0x06002B0E RID: 11022 RVA: 0x0011E12B File Offset: 0x0011C32B
		// (set) Token: 0x06002B0F RID: 11023 RVA: 0x0011E133 File Offset: 0x0011C333
		public RelayCommand ChangeHighThresholdCommand { get; set; }

		// Token: 0x17000BC5 RID: 3013
		// (get) Token: 0x06002B10 RID: 11024 RVA: 0x0011E13C File Offset: 0x0011C33C
		// (set) Token: 0x06002B11 RID: 11025 RVA: 0x0011E144 File Offset: 0x0011C344
		public RelayCommand ChangeMinSizeCommand { get; set; }

		// Token: 0x17000BC6 RID: 3014
		// (get) Token: 0x06002B12 RID: 11026 RVA: 0x0011E14D File Offset: 0x0011C34D
		// (set) Token: 0x06002B13 RID: 11027 RVA: 0x0011E155 File Offset: 0x0011C355
		public RelayCommand ChangeMaxSizeCommand { get; set; }

		// Token: 0x17000BC7 RID: 3015
		// (get) Token: 0x06002B14 RID: 11028 RVA: 0x0011E15E File Offset: 0x0011C35E
		// (set) Token: 0x06002B15 RID: 11029 RVA: 0x0011E166 File Offset: 0x0011C366
		public RelayCommand TestSettingsCommand { get; set; }

		// Token: 0x17000BC8 RID: 3016
		// (get) Token: 0x06002B16 RID: 11030 RVA: 0x0011E16F File Offset: 0x0011C36F
		// (set) Token: 0x06002B17 RID: 11031 RVA: 0x0011E177 File Offset: 0x0011C377
		public RelayCommand ChangeFilterSizeCommand { get; set; }

		// Token: 0x17000BC9 RID: 3017
		// (get) Token: 0x06002B18 RID: 11032 RVA: 0x0011E180 File Offset: 0x0011C380
		// (set) Token: 0x06002B19 RID: 11033 RVA: 0x0011E188 File Offset: 0x0011C388
		public BoundLocalizedText StrCurrentCount { get; set; }

		// Token: 0x17000BCA RID: 3018
		// (get) Token: 0x06002B1A RID: 11034 RVA: 0x0011E191 File Offset: 0x0011C391
		// (set) Token: 0x06002B1B RID: 11035 RVA: 0x0011E199 File Offset: 0x0011C399
		public BoundLocalizedText StrMinCount { get; set; }

		// Token: 0x17000BCB RID: 3019
		// (get) Token: 0x06002B1C RID: 11036 RVA: 0x0011E1A2 File Offset: 0x0011C3A2
		// (set) Token: 0x06002B1D RID: 11037 RVA: 0x0011E1AA File Offset: 0x0011C3AA
		public BoundLocalizedText StrMaxCount { get; set; }

		// Token: 0x17000BCC RID: 3020
		// (get) Token: 0x06002B1E RID: 11038 RVA: 0x0011E1B3 File Offset: 0x0011C3B3
		// (set) Token: 0x06002B1F RID: 11039 RVA: 0x0011E1BB File Offset: 0x0011C3BB
		public BoundLocalizedText StrLowThreshold { get; set; }

		// Token: 0x17000BCD RID: 3021
		// (get) Token: 0x06002B20 RID: 11040 RVA: 0x0011E1C4 File Offset: 0x0011C3C4
		// (set) Token: 0x06002B21 RID: 11041 RVA: 0x0011E1CC File Offset: 0x0011C3CC
		public BoundLocalizedText StrHighThreshold { get; set; }

		// Token: 0x17000BCE RID: 3022
		// (get) Token: 0x06002B22 RID: 11042 RVA: 0x0011E1D5 File Offset: 0x0011C3D5
		// (set) Token: 0x06002B23 RID: 11043 RVA: 0x0011E1DD File Offset: 0x0011C3DD
		public BoundLocalizedText StrMinSize { get; set; }

		// Token: 0x17000BCF RID: 3023
		// (get) Token: 0x06002B24 RID: 11044 RVA: 0x0011E1E6 File Offset: 0x0011C3E6
		// (set) Token: 0x06002B25 RID: 11045 RVA: 0x0011E1EE File Offset: 0x0011C3EE
		public BoundLocalizedText StrMaxSize { get; set; }

		// Token: 0x17000BD0 RID: 3024
		// (get) Token: 0x06002B26 RID: 11046 RVA: 0x0011E1F7 File Offset: 0x0011C3F7
		// (set) Token: 0x06002B27 RID: 11047 RVA: 0x0011E1FF File Offset: 0x0011C3FF
		public BoundLocalizedText StrUnmaskForContEnabled { get; set; }

		// Token: 0x17000BD1 RID: 3025
		// (get) Token: 0x06002B28 RID: 11048 RVA: 0x0011E208 File Offset: 0x0011C408
		// (set) Token: 0x06002B29 RID: 11049 RVA: 0x0011E210 File Offset: 0x0011C410
		public BoundLocalizedText StrFilterSize { get; set; }

		// Token: 0x17000BD2 RID: 3026
		// (get) Token: 0x06002B2A RID: 11050 RVA: 0x0011E219 File Offset: 0x0011C419
		// (set) Token: 0x06002B2B RID: 11051 RVA: 0x0011E221 File Offset: 0x0011C421
		public MachineState MachineState { get; set; }

		// Token: 0x17000BD3 RID: 3027
		// (get) Token: 0x06002B2C RID: 11052 RVA: 0x0011E22A File Offset: 0x0011C42A
		// (set) Token: 0x06002B2D RID: 11053 RVA: 0x0011E232 File Offset: 0x0011C432
		public bool DemoMode { get; set; }

		// Token: 0x06002B2E RID: 11054 RVA: 0x0011E23C File Offset: 0x0011C43C
		public ObjectCounterViewModel(ParameterHandling parHandling, ImageDataTransfer imageTransfer, DBConnection dbConnection, int group, ProductManager productManager, StoreImageData imageArchiver, MachineState machineState)
		{
			this.parHandling = parHandling;
			this.imageTransfer = imageTransfer;
			this.dbConnection = dbConnection;
			this.productManager = productManager;
			this.imageGroup = group;
			this.imageArchiver = imageArchiver;
			this.MachineState = machineState;
			bool demoMode = false;
			dbConnection.GetDemoMode(ref demoMode);
			this.DemoMode = demoMode;
			this.ObjCounter = new ObjectCounter(parHandling, group, productManager, dbConnection);
			this.ChangeMinCountCommand = new RelayCommand(new Action<object>(this.ChangeMinCount));
			this.ChangeMaxCountCommand = new RelayCommand(new Action<object>(this.ChangeMaxCount));
			this.ChangeLowThresholdCommand = new RelayCommand(new Action<object>(this.ChangeLowThreshold));
			this.ChangeHighThresholdCommand = new RelayCommand(new Action<object>(this.ChangeHighThreshold));
			this.ChangeMinSizeCommand = new RelayCommand(new Action<object>(this.ChangeMinSize));
			this.ChangeMaxSizeCommand = new RelayCommand(new Action<object>(this.ChangeMaxSize));
			this.TestSettingsCommand = new RelayCommand(new Action<object>(this.TestSettings));
			this.ChangeFilterSizeCommand = new RelayCommand(new Action<object>(this.ChangeFilterSize));
			this.RegisterLocalizedText();
		}

		// Token: 0x06002B2F RID: 11055 RVA: 0x0011E374 File Offset: 0x0011C574
		private void RegisterLocalizedText()
		{
			this.StrCurrentCount = new BoundLocalizedText("txtCurrentCount");
			this.StrMinCount = new BoundLocalizedText("txtMinCount");
			this.StrMaxCount = new BoundLocalizedText("txtMaxCount");
			this.StrLowThreshold = new BoundLocalizedText("txtLowThreshold");
			this.StrHighThreshold = new BoundLocalizedText("txtHighThreshold");
			this.StrMinSize = new BoundLocalizedText("txtMinSize");
			this.StrMaxSize = new BoundLocalizedText("txtMaxSize");
			this.StrUnmaskForContEnabled = new BoundLocalizedText("txtObjectUnmaskForConEnabled");
			this.StrFilterSize = new BoundLocalizedText("txtFilterSize");
		}

		// Token: 0x06002B30 RID: 11056 RVA: 0x0011E41C File Offset: 0x0011C61C
		public void DeregisterLocalizedText()
		{
			MainWindow.localizationManager.Deregister(this.StrCurrentCount);
			MainWindow.localizationManager.Deregister(this.StrMinCount);
			MainWindow.localizationManager.Deregister(this.StrMaxCount);
			MainWindow.localizationManager.Deregister(this.StrLowThreshold);
			MainWindow.localizationManager.Deregister(this.StrHighThreshold);
			MainWindow.localizationManager.Deregister(this.StrMinSize);
			MainWindow.localizationManager.Deregister(this.StrMaxSize);
			MainWindow.localizationManager.Deregister(this.StrUnmaskForContEnabled);
			MainWindow.localizationManager.Deregister(this.StrFilterSize);
			this.ChangeMinCountCommand = null;
			this.ChangeMaxCountCommand = null;
			this.ChangeLowThresholdCommand = null;
			this.ChangeHighThresholdCommand = null;
			this.ChangeMinSizeCommand = null;
			this.ChangeMaxSizeCommand = null;
			this.TestSettingsCommand = null;
			this.ChangeFilterSizeCommand = null;
		}

		// Token: 0x06002B31 RID: 11057 RVA: 0x0011E504 File Offset: 0x0011C704
		public void ChangeMinCount(object obj)
		{
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtMinCount"), this.ObjCounter.MinCount.ToString(), NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_InputMinCount;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x06002B32 RID: 11058 RVA: 0x0011E55F File Offset: 0x0011C75F
		private void numKeyboard_InputMinCount(object sender, KeyboardInputEventArgs e)
		{
			this.ObjCounter.MinCount = int.Parse(e.Text);
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_InputMinCount;
		}

		// Token: 0x06002B33 RID: 11059 RVA: 0x0011E594 File Offset: 0x0011C794
		public void ChangeMaxCount(object obj)
		{
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtMaxCount"), this.ObjCounter.MaxCount.ToString(), NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_InputMaxCount;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x06002B34 RID: 11060 RVA: 0x0011E5EF File Offset: 0x0011C7EF
		private void numKeyboard_InputMaxCount(object sender, KeyboardInputEventArgs e)
		{
			this.ObjCounter.MaxCount = int.Parse(e.Text);
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_InputMaxCount;
		}

		// Token: 0x06002B35 RID: 11061 RVA: 0x0011E624 File Offset: 0x0011C824
		public void ChangeMaxSize(object obj)
		{
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtMaxSize"), this.ObjCounter.MaxSize.ToString(), NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_InputMaxSize;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x06002B36 RID: 11062 RVA: 0x0011E67F File Offset: 0x0011C87F
		private void numKeyboard_InputMaxSize(object sender, KeyboardInputEventArgs e)
		{
			this.ObjCounter.MaxSize = int.Parse(e.Text);
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_InputMaxSize;
		}

		// Token: 0x06002B37 RID: 11063 RVA: 0x0011E6B4 File Offset: 0x0011C8B4
		public void ChangeMinSize(object obj)
		{
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtMinSize"), this.ObjCounter.MinSize.ToString(), NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_InputMinSize;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x06002B38 RID: 11064 RVA: 0x0011E70F File Offset: 0x0011C90F
		private void numKeyboard_InputMinSize(object sender, KeyboardInputEventArgs e)
		{
			this.ObjCounter.MinSize = int.Parse(e.Text);
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_InputMinSize;
		}

		// Token: 0x06002B39 RID: 11065 RVA: 0x0011E744 File Offset: 0x0011C944
		public void ChangeHighThreshold(object obj)
		{
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtHighThreshold"), this.ObjCounter.HighThreshold.ToString(), NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_InputHighThreshold;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x06002B3A RID: 11066 RVA: 0x0011E79F File Offset: 0x0011C99F
		private void numKeyboard_InputHighThreshold(object sender, KeyboardInputEventArgs e)
		{
			this.ObjCounter.HighThreshold = int.Parse(e.Text);
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_InputHighThreshold;
		}

		// Token: 0x06002B3B RID: 11067 RVA: 0x0011E7D4 File Offset: 0x0011C9D4
		public void ChangeLowThreshold(object obj)
		{
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtLowThreshold"), this.ObjCounter.LowThreshold.ToString(), NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_InputLowThreshold;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x06002B3C RID: 11068 RVA: 0x0011E82F File Offset: 0x0011CA2F
		private void numKeyboard_InputLowThreshold(object sender, KeyboardInputEventArgs e)
		{
			this.ObjCounter.LowThreshold = int.Parse(e.Text);
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_InputLowThreshold;
		}

		// Token: 0x06002B3D RID: 11069 RVA: 0x0011E864 File Offset: 0x0011CA64
		public void TestSettings(object obj)
		{
			string str = (this.imageGroup > 0) ? this.imageGroup.ToString() : "";
			bool flag = this.parHandling.ReadPar(Definition.filterMainPath + str + ObjectCounter.enabledSubPath) == "1";
			if (flag)
			{
				this.ctrlCounterAdaption = new CtrlCounterAdaption(this.parHandling, this.imageTransfer, this.imageGroup, this.productManager, this.imageArchiver, this.dbConnection, this.MachineState);
				Subwindow.Show(this.ctrlCounterAdaption, SubwindowButtons.Back);
			}
			else
			{
				bool flag2 = Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("msgObjectCounterDisabled"), DialogStyle.Warning, DialogButtons.OKCancel) == Dialog.DialogResults.Ok;
				if (flag2)
				{
					this.parHandling.WriteValue(Definition.filterMainPath + str + ObjectCounter.enabledSubPath, "1");
					this.ctrlCounterAdaption = new CtrlCounterAdaption(this.parHandling, this.imageTransfer, this.imageGroup, this.productManager, this.imageArchiver, this.dbConnection, this.MachineState);
					Subwindow.Show(this.ctrlCounterAdaption, SubwindowButtons.Back);
				}
			}
		}

		// Token: 0x06002B3E RID: 11070 RVA: 0x0011E989 File Offset: 0x0011CB89
		public void TestSettingsRoutedEvent(object sender, RoutedEventArgs e)
		{
			this.TestSettings(null);
		}

		// Token: 0x06002B3F RID: 11071 RVA: 0x0011E994 File Offset: 0x0011CB94
		public void ChangeFilterSize(object obj)
		{
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtFilterSize"), this.ObjCounter.FilterSize.ToString(), NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_InputFilterSize;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x06002B40 RID: 11072 RVA: 0x0011E9EF File Offset: 0x0011CBEF
		private void numKeyboard_InputFilterSize(object sender, KeyboardInputEventArgs e)
		{
			this.ObjCounter.FilterSize = int.Parse(e.Text);
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_InputMaxSize;
		}

		// Token: 0x17000BD4 RID: 3028
		// (get) Token: 0x06002B41 RID: 11073 RVA: 0x0011EA21 File Offset: 0x0011CC21
		// (set) Token: 0x06002B42 RID: 11074 RVA: 0x0011EA2E File Offset: 0x0011CC2E
		public bool ChangeUnmaskForCont
		{
			get
			{
				return this.ObjCounter.UnmaskForContEnabled;
			}
			set
			{
				this.ObjCounter.UnmaskForContEnabled = value;
			}
		}

		// Token: 0x04001AF7 RID: 6903
		private ParameterHandling parHandling;

		// Token: 0x04001AF8 RID: 6904
		private ImageDataTransfer imageTransfer;

		// Token: 0x04001AF9 RID: 6905
		private DBConnection dbConnection;

		// Token: 0x04001AFA RID: 6906
		private NumKeyboard numKeyboard;

		// Token: 0x04001AFB RID: 6907
		private CtrlCounterAdaption ctrlCounterAdaption;

		// Token: 0x04001AFC RID: 6908
		private int imageGroup;

		// Token: 0x04001AFD RID: 6909
		private ProductManager productManager;

		// Token: 0x04001AFE RID: 6910
		private StoreImageData imageArchiver;
	}
}
