using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using Common;
using DatabaseConnection;
using Definitions;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x02000095 RID: 149
	public class SizeFilterViewModel : INotifyPropertyChanged
	{
		// Token: 0x1400004F RID: 79
		// (add) Token: 0x06000F05 RID: 3845 RVA: 0x0005A6D8 File Offset: 0x000588D8
		// (remove) Token: 0x06000F06 RID: 3846 RVA: 0x0005A710 File Offset: 0x00058910
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x17000490 RID: 1168
		// (get) Token: 0x06000F07 RID: 3847 RVA: 0x0005A745 File Offset: 0x00058945
		// (set) Token: 0x06000F08 RID: 3848 RVA: 0x0005A74D File Offset: 0x0005894D
		public RelayCommand ChangeMinWidthCommand { get; set; }

		// Token: 0x17000491 RID: 1169
		// (get) Token: 0x06000F09 RID: 3849 RVA: 0x0005A756 File Offset: 0x00058956
		// (set) Token: 0x06000F0A RID: 3850 RVA: 0x0005A75E File Offset: 0x0005895E
		public RelayCommand ChangeMaxWidthCommand { get; set; }

		// Token: 0x17000492 RID: 1170
		// (get) Token: 0x06000F0B RID: 3851 RVA: 0x0005A767 File Offset: 0x00058967
		// (set) Token: 0x06000F0C RID: 3852 RVA: 0x0005A76F File Offset: 0x0005896F
		public RelayCommand ChangeMinHeightCommand { get; set; }

		// Token: 0x17000493 RID: 1171
		// (get) Token: 0x06000F0D RID: 3853 RVA: 0x0005A778 File Offset: 0x00058978
		// (set) Token: 0x06000F0E RID: 3854 RVA: 0x0005A780 File Offset: 0x00058980
		public RelayCommand ChangeMaxHeightCommand { get; set; }

		// Token: 0x17000494 RID: 1172
		// (get) Token: 0x06000F0F RID: 3855 RVA: 0x0005A789 File Offset: 0x00058989
		// (set) Token: 0x06000F10 RID: 3856 RVA: 0x0005A791 File Offset: 0x00058991
		public BoundLocalizedText StrCurrentWidth { get; set; }

		// Token: 0x17000495 RID: 1173
		// (get) Token: 0x06000F11 RID: 3857 RVA: 0x0005A79A File Offset: 0x0005899A
		// (set) Token: 0x06000F12 RID: 3858 RVA: 0x0005A7A2 File Offset: 0x000589A2
		public BoundLocalizedText StrMinWidth { get; set; }

		// Token: 0x17000496 RID: 1174
		// (get) Token: 0x06000F13 RID: 3859 RVA: 0x0005A7AB File Offset: 0x000589AB
		// (set) Token: 0x06000F14 RID: 3860 RVA: 0x0005A7B3 File Offset: 0x000589B3
		public BoundLocalizedText StrMaxWidth { get; set; }

		// Token: 0x17000497 RID: 1175
		// (get) Token: 0x06000F15 RID: 3861 RVA: 0x0005A7BC File Offset: 0x000589BC
		// (set) Token: 0x06000F16 RID: 3862 RVA: 0x0005A7C4 File Offset: 0x000589C4
		public BoundLocalizedText StrCurrentHeight { get; set; }

		// Token: 0x17000498 RID: 1176
		// (get) Token: 0x06000F17 RID: 3863 RVA: 0x0005A7CD File Offset: 0x000589CD
		// (set) Token: 0x06000F18 RID: 3864 RVA: 0x0005A7D5 File Offset: 0x000589D5
		public BoundLocalizedText StrMinHeight { get; set; }

		// Token: 0x17000499 RID: 1177
		// (get) Token: 0x06000F19 RID: 3865 RVA: 0x0005A7DE File Offset: 0x000589DE
		// (set) Token: 0x06000F1A RID: 3866 RVA: 0x0005A7E6 File Offset: 0x000589E6
		public BoundLocalizedText StrMaxHeight { get; set; }

		// Token: 0x1700049A RID: 1178
		// (get) Token: 0x06000F1B RID: 3867 RVA: 0x0005A7F0 File Offset: 0x000589F0
		// (set) Token: 0x06000F1C RID: 3868 RVA: 0x0005A808 File Offset: 0x00058A08
		public int CurrentWidth
		{
			get
			{
				return this.currentWidth;
			}
			set
			{
				this.currentWidth = value;
				this.OnPropertyChanged("CurrentWidth");
			}
		}

		// Token: 0x1700049B RID: 1179
		// (get) Token: 0x06000F1D RID: 3869 RVA: 0x0005A820 File Offset: 0x00058A20
		// (set) Token: 0x06000F1E RID: 3870 RVA: 0x0005A838 File Offset: 0x00058A38
		public int MinWidth
		{
			get
			{
				return this.minWidth;
			}
			set
			{
				bool flag = this.minWidth != value;
				if (flag)
				{
					bool flag2 = this.initialized;
					if (flag2)
					{
						LogbookManager.AddEntry(SSTEntryCode.ParameterChanged, 0, "SizeMinWidth", this.minWidth.ToString(), value.ToString());
					}
					this.minWidth = ((value >= 100000) ? 99999 : value);
					this.OnPropertyChanged("MinWidth");
					bool flag3 = this.initialized;
					if (flag3)
					{
						this.parHandling.WriteValue(Definition.filterMainPath + this.strImageGroup + SizeFilterViewModel.minWidthSubPath, this.minWidth.ToString());
					}
				}
			}
		}

		// Token: 0x1700049C RID: 1180
		// (get) Token: 0x06000F1F RID: 3871 RVA: 0x0005A8E0 File Offset: 0x00058AE0
		// (set) Token: 0x06000F20 RID: 3872 RVA: 0x0005A8F8 File Offset: 0x00058AF8
		public int MaxWidth
		{
			get
			{
				return this.maxWidth;
			}
			set
			{
				bool flag = this.maxWidth != value;
				if (flag)
				{
					bool flag2 = this.initialized;
					if (flag2)
					{
						LogbookManager.AddEntry(SSTEntryCode.ParameterChanged, 0, "SizeMaxWidth", this.maxWidth.ToString(), value.ToString());
					}
					this.maxWidth = ((value >= 100000) ? 99999 : value);
					this.OnPropertyChanged("MaxWidth");
					bool flag3 = this.initialized;
					if (flag3)
					{
						this.parHandling.WriteValue(Definition.filterMainPath + this.strImageGroup + SizeFilterViewModel.maxWidthSubPath, this.maxWidth.ToString());
					}
				}
			}
		}

		// Token: 0x1700049D RID: 1181
		// (get) Token: 0x06000F21 RID: 3873 RVA: 0x0005A9A0 File Offset: 0x00058BA0
		// (set) Token: 0x06000F22 RID: 3874 RVA: 0x0005A9B8 File Offset: 0x00058BB8
		public int CurrentHeight
		{
			get
			{
				return this.currentHeight;
			}
			set
			{
				this.currentHeight = value;
				this.OnPropertyChanged("CurrentHeight");
			}
		}

		// Token: 0x1700049E RID: 1182
		// (get) Token: 0x06000F23 RID: 3875 RVA: 0x0005A9D0 File Offset: 0x00058BD0
		// (set) Token: 0x06000F24 RID: 3876 RVA: 0x0005A9E8 File Offset: 0x00058BE8
		public int MinHeight
		{
			get
			{
				return this.minHeight;
			}
			set
			{
				bool flag = this.minHeight != value;
				if (flag)
				{
					bool flag2 = this.initialized;
					if (flag2)
					{
						LogbookManager.AddEntry(SSTEntryCode.ParameterChanged, 0, "SizeMinHeight", this.minHeight.ToString(), value.ToString());
					}
					this.minHeight = ((value >= 100000) ? 99999 : value);
					this.OnPropertyChanged("MinHeight");
					bool flag3 = this.initialized;
					if (flag3)
					{
						this.parHandling.WriteValue(Definition.filterMainPath + this.strImageGroup + SizeFilterViewModel.minHeightSubPath, this.minHeight.ToString());
					}
				}
			}
		}

		// Token: 0x1700049F RID: 1183
		// (get) Token: 0x06000F25 RID: 3877 RVA: 0x0005AA90 File Offset: 0x00058C90
		// (set) Token: 0x06000F26 RID: 3878 RVA: 0x0005AAA8 File Offset: 0x00058CA8
		public int MaxHeight
		{
			get
			{
				return this.maxHeight;
			}
			set
			{
				bool flag = this.maxHeight != value;
				if (flag)
				{
					bool flag2 = this.initialized;
					if (flag2)
					{
						LogbookManager.AddEntry(SSTEntryCode.ParameterChanged, 0, "SizeMaxHeight", this.maxHeight.ToString(), value.ToString());
					}
					this.maxHeight = ((value >= 100000) ? 99999 : value);
					this.OnPropertyChanged("MaxHeight");
					bool flag3 = this.initialized;
					if (flag3)
					{
						this.parHandling.WriteValue(Definition.filterMainPath + this.strImageGroup + SizeFilterViewModel.maxHeightSubPath, this.maxHeight.ToString());
					}
				}
			}
		}

		// Token: 0x170004A0 RID: 1184
		// (get) Token: 0x06000F27 RID: 3879 RVA: 0x0005AB50 File Offset: 0x00058D50
		// (set) Token: 0x06000F28 RID: 3880 RVA: 0x0005AB58 File Offset: 0x00058D58
		public XRSImage CurrentImage { get; set; }

		// Token: 0x170004A1 RID: 1185
		// (get) Token: 0x06000F29 RID: 3881 RVA: 0x0005AB64 File Offset: 0x00058D64
		// (set) Token: 0x06000F2A RID: 3882 RVA: 0x0005AB7C File Offset: 0x00058D7C
		public bool IsExpanded
		{
			get
			{
				return this.isExpanded;
			}
			set
			{
				this.isExpanded = value;
				bool flag = this.isExpanded;
				if (flag)
				{
					this.ImageBoxHeight = 400;
				}
				else
				{
					this.ImageBoxHeight = 160;
				}
				this.OnPropertyChanged("IsExpanded");
			}
		}

		// Token: 0x170004A2 RID: 1186
		// (get) Token: 0x06000F2B RID: 3883 RVA: 0x0005ABC8 File Offset: 0x00058DC8
		// (set) Token: 0x06000F2C RID: 3884 RVA: 0x0005ABE0 File Offset: 0x00058DE0
		public int ImageBoxHeight
		{
			get
			{
				return this.imageBoxHeight;
			}
			set
			{
				this.imageBoxHeight = value;
				this.OnPropertyChanged("ImageBoxHeight");
			}
		}

		// Token: 0x170004A3 RID: 1187
		// (get) Token: 0x06000F2D RID: 3885 RVA: 0x0005ABF8 File Offset: 0x00058DF8
		// (set) Token: 0x06000F2E RID: 3886 RVA: 0x0005AC10 File Offset: 0x00058E10
		public bool HighSpeedMachineActive
		{
			get
			{
				return this.highSpeedMachineActive;
			}
			set
			{
				this.highSpeedMachineActive = value;
				this.OnPropertyChanged("HighSpeedMachineActive");
			}
		}

		// Token: 0x170004A4 RID: 1188
		// (get) Token: 0x06000F2F RID: 3887 RVA: 0x0005AC28 File Offset: 0x00058E28
		// (set) Token: 0x06000F30 RID: 3888 RVA: 0x0005AC40 File Offset: 0x00058E40
		public bool ShowNext
		{
			get
			{
				return this.showNext;
			}
			set
			{
				this.showNext = value;
				this.OnPropertyChanged("ShowNext");
			}
		}

		// Token: 0x170004A5 RID: 1189
		// (get) Token: 0x06000F31 RID: 3889 RVA: 0x0005AC58 File Offset: 0x00058E58
		// (set) Token: 0x06000F32 RID: 3890 RVA: 0x0005AC70 File Offset: 0x00058E70
		public bool SendImage
		{
			get
			{
				return this.sendImage;
			}
			set
			{
				this.sendImage = value;
				bool flag = this.sendImage;
				if (flag)
				{
					this.OnPropertyChanged("SendImage");
				}
			}
		}

		// Token: 0x170004A6 RID: 1190
		// (get) Token: 0x06000F33 RID: 3891 RVA: 0x0005AC9B File Offset: 0x00058E9B
		// (set) Token: 0x06000F34 RID: 3892 RVA: 0x0005ACA3 File Offset: 0x00058EA3
		public MachineState machineState { get; set; }

		// Token: 0x170004A7 RID: 1191
		// (get) Token: 0x06000F35 RID: 3893 RVA: 0x0005ACAC File Offset: 0x00058EAC
		// (set) Token: 0x06000F36 RID: 3894 RVA: 0x0005ACB4 File Offset: 0x00058EB4
		public bool DemoMode { get; set; }

		// Token: 0x170004A8 RID: 1192
		// (get) Token: 0x06000F37 RID: 3895 RVA: 0x0005ACBD File Offset: 0x00058EBD
		// (set) Token: 0x06000F38 RID: 3896 RVA: 0x0005ACC5 File Offset: 0x00058EC5
		public RelayCommand HighSpeedImageCommand { get; set; }

		// Token: 0x06000F39 RID: 3897 RVA: 0x0005ACD0 File Offset: 0x00058ED0
		public SizeFilterViewModel(ParameterHandling parHandling, DBConnection dbConnection, int group, ProductManager productManager, StoreImageData imageArchiver, MachineState machineState)
		{
			this.parHandling = parHandling;
			this.productManager = productManager;
			this.imageGroup = group;
			this.imageArchiver = imageArchiver;
			this.machineState = machineState;
			bool demoMode = false;
			dbConnection.GetDemoMode(ref demoMode);
			this.DemoMode = demoMode;
			this.strImageGroup = "";
			this.ChangeMinWidthCommand = new RelayCommand(new Action<object>(this.ChangeMinWidth));
			this.ChangeMaxWidthCommand = new RelayCommand(new Action<object>(this.ChangeMaxWidth));
			this.ChangeMinHeightCommand = new RelayCommand(new Action<object>(this.ChangeMinHeight));
			this.ChangeMaxHeightCommand = new RelayCommand(new Action<object>(this.ChangeMaxHeight));
			this.HighSpeedImageCommand = new RelayCommand(new Action<object>(this.TakeHighSpeedImage));
			this.RegisterLocalizedText();
			this.InitializeValues(false);
			this.initialized = true;
			this.IsExpanded = false;
			bool flag = false;
			dbConnection.GetHighThroughputMachine(ref flag);
			string text = "";
			dbConnection.GetParameterValue(Definition.highThroughputEnabled, ref text);
			bool flag2 = flag && text.Equals("1");
			if (flag2)
			{
				this.HighSpeedMachineActive = true;
			}
			else
			{
				this.HighSpeedMachineActive = false;
			}
			this.ShowNext = false;
		}

		// Token: 0x06000F3A RID: 3898 RVA: 0x0005AE20 File Offset: 0x00059020
		private void RegisterLocalizedText()
		{
			this.StrCurrentWidth = new BoundLocalizedText("txtCurrentWidth");
			this.StrMinWidth = new BoundLocalizedText("txtMinWidth");
			this.StrMaxWidth = new BoundLocalizedText("txtMaxWidth");
			this.StrCurrentHeight = new BoundLocalizedText("txtCurrentHeight");
			this.StrMinHeight = new BoundLocalizedText("txtMinHeight");
			this.StrMaxHeight = new BoundLocalizedText("txtMaxHeight");
		}

		// Token: 0x06000F3B RID: 3899 RVA: 0x0005AE94 File Offset: 0x00059094
		public void Cleanup()
		{
			MainWindow.localizationManager.Deregister(this.StrCurrentWidth);
			MainWindow.localizationManager.Deregister(this.StrMinWidth);
			MainWindow.localizationManager.Deregister(this.StrMaxWidth);
			MainWindow.localizationManager.Deregister(this.StrCurrentHeight);
			MainWindow.localizationManager.Deregister(this.StrMinHeight);
			MainWindow.localizationManager.Deregister(this.StrMaxHeight);
			this.ChangeMinWidthCommand = null;
			this.ChangeMaxWidthCommand = null;
			this.ChangeMinHeightCommand = null;
			this.ChangeMaxHeightCommand = null;
		}

		// Token: 0x06000F3C RID: 3900 RVA: 0x0005AF28 File Offset: 0x00059128
		public void ChangeMinWidth(object obj)
		{
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtMinHeight"), this.MinWidth.ToString(), NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_InputMinWidth;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x06000F3D RID: 3901 RVA: 0x0005AF7E File Offset: 0x0005917E
		private void numKeyboard_InputMinWidth(object sender, KeyboardInputEventArgs e)
		{
			this.MinWidth = int.Parse(e.Text);
			this.SendImage = true;
			this.ShowNext = true;
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_InputMinWidth;
		}

		// Token: 0x06000F3E RID: 3902 RVA: 0x0005AFBC File Offset: 0x000591BC
		public void ChangeMaxWidth(object obj)
		{
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtMaxHeight"), this.MaxWidth.ToString(), NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_InputMaxWidth;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x06000F3F RID: 3903 RVA: 0x0005B012 File Offset: 0x00059212
		private void numKeyboard_InputMaxWidth(object sender, KeyboardInputEventArgs e)
		{
			this.MaxWidth = int.Parse(e.Text);
			this.SendImage = true;
			this.ShowNext = true;
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_InputMaxWidth;
		}

		// Token: 0x06000F40 RID: 3904 RVA: 0x0005B050 File Offset: 0x00059250
		public void ChangeMinHeight(object obj)
		{
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtMinWidth"), this.MinHeight.ToString(), NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_InputMinHeight;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x06000F41 RID: 3905 RVA: 0x0005B0A6 File Offset: 0x000592A6
		private void numKeyboard_InputMinHeight(object sender, KeyboardInputEventArgs e)
		{
			this.MinHeight = int.Parse(e.Text);
			this.SendImage = true;
			this.ShowNext = true;
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_InputMinHeight;
		}

		// Token: 0x06000F42 RID: 3906 RVA: 0x0005B0E4 File Offset: 0x000592E4
		public void ChangeMaxHeight(object obj)
		{
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtMaxWidth"), this.MaxHeight.ToString(), NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_InputMaxHeight;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x06000F43 RID: 3907 RVA: 0x0005B13A File Offset: 0x0005933A
		private void numKeyboard_InputMaxHeight(object sender, KeyboardInputEventArgs e)
		{
			this.MaxHeight = int.Parse(e.Text);
			this.SendImage = true;
			this.ShowNext = true;
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_InputMaxHeight;
		}

		// Token: 0x06000F44 RID: 3908 RVA: 0x0005B178 File Offset: 0x00059378
		public void InitializeValues(bool withCurentValues)
		{
			try
			{
				if (withCurentValues)
				{
					int.TryParse(this.parHandling.ReadPar(Definition.filterMainPath + this.strImageGroup + SizeFilterViewModel.currentWidthSubPath), out this.currentWidth);
					int.TryParse(this.parHandling.ReadPar(Definition.filterMainPath + this.strImageGroup + SizeFilterViewModel.currentHeightSubPath), out this.currentHeight);
				}
				else
				{
					this.currentWidth = 0;
					this.currentHeight = 0;
				}
				int.TryParse(this.parHandling.ReadPar(Definition.filterMainPath + this.strImageGroup + SizeFilterViewModel.minWidthSubPath), out this.minWidth);
				int.TryParse(this.parHandling.ReadPar(Definition.filterMainPath + this.strImageGroup + SizeFilterViewModel.maxWidthSubPath), out this.maxWidth);
				int.TryParse(this.parHandling.ReadPar(Definition.filterMainPath + this.strImageGroup + SizeFilterViewModel.minHeightSubPath), out this.minHeight);
				int.TryParse(this.parHandling.ReadPar(Definition.filterMainPath + this.strImageGroup + SizeFilterViewModel.maxHeightSubPath), out this.maxHeight);
				this.OnPropertyChanged("CurrentWidth");
				this.OnPropertyChanged("MinWidth");
				this.OnPropertyChanged("MaxWidth");
				this.OnPropertyChanged("CurrentHeight");
				this.OnPropertyChanged("MinHeight");
				this.OnPropertyChanged("MaxHeight");
			}
			catch
			{
			}
		}

		// Token: 0x06000F45 RID: 3909 RVA: 0x0005B314 File Offset: 0x00059514
		private void TakeHighSpeedImage(object ob)
		{
			bool flag = this.ShowNext;
			if (flag)
			{
				this.ShowNext = false;
			}
			else
			{
				this.ShowNext = true;
			}
		}

		// Token: 0x06000F46 RID: 3910 RVA: 0x0005B340 File Offset: 0x00059540
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x04000A11 RID: 2577
		public static readonly string sizeFilterSubPath = "/sizeFilter/";

		// Token: 0x04000A12 RID: 2578
		public static readonly string currentWidthSubPath = SizeFilterViewModel.sizeFilterSubPath + "CurrentWidth/Value";

		// Token: 0x04000A13 RID: 2579
		public static readonly string minWidthSubPath = SizeFilterViewModel.sizeFilterSubPath + "MinWidth/Value";

		// Token: 0x04000A14 RID: 2580
		public static readonly string maxWidthSubPath = SizeFilterViewModel.sizeFilterSubPath + "MaxWidth/Value";

		// Token: 0x04000A15 RID: 2581
		public static readonly string currentHeightSubPath = SizeFilterViewModel.sizeFilterSubPath + "CurrentHeight/Value";

		// Token: 0x04000A16 RID: 2582
		public static readonly string minHeightSubPath = SizeFilterViewModel.sizeFilterSubPath + "MinHeight/Value";

		// Token: 0x04000A17 RID: 2583
		public static readonly string maxHeightSubPath = SizeFilterViewModel.sizeFilterSubPath + "MaxHeight/Value";

		// Token: 0x04000A18 RID: 2584
		private ParameterHandling parHandling;

		// Token: 0x04000A19 RID: 2585
		private NumKeyboard numKeyboard;

		// Token: 0x04000A1A RID: 2586
		private int imageGroup;

		// Token: 0x04000A1B RID: 2587
		private ProductManager productManager;

		// Token: 0x04000A1C RID: 2588
		private StoreImageData imageArchiver;

		// Token: 0x04000A1D RID: 2589
		private Timer timer;

		// Token: 0x04000A1E RID: 2590
		private int currentWidth;

		// Token: 0x04000A1F RID: 2591
		private int minWidth;

		// Token: 0x04000A20 RID: 2592
		private int maxWidth;

		// Token: 0x04000A21 RID: 2593
		private int currentHeight;

		// Token: 0x04000A22 RID: 2594
		private int minHeight;

		// Token: 0x04000A23 RID: 2595
		private int maxHeight;

		// Token: 0x04000A24 RID: 2596
		private string strImageGroup;

		// Token: 0x04000A25 RID: 2597
		private bool initialized;

		// Token: 0x04000A32 RID: 2610
		private bool isExpanded;

		// Token: 0x04000A33 RID: 2611
		private int imageBoxHeight;

		// Token: 0x04000A34 RID: 2612
		private bool highSpeedMachineActive = false;

		// Token: 0x04000A35 RID: 2613
		private bool showNext = false;

		// Token: 0x04000A36 RID: 2614
		private bool sendImage;
	}
}
