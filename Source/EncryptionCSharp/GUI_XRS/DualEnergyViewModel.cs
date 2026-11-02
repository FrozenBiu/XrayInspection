using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Common;
using DatabaseConnection;
using Definitions;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x020000A1 RID: 161
	public class DualEnergyViewModel : INotifyPropertyChanged
	{
		// Token: 0x14000050 RID: 80
		// (add) Token: 0x06000FD8 RID: 4056 RVA: 0x00062874 File Offset: 0x00060A74
		// (remove) Token: 0x06000FD9 RID: 4057 RVA: 0x000628AC File Offset: 0x00060AAC
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x170004C2 RID: 1218
		// (get) Token: 0x06000FDA RID: 4058 RVA: 0x000628E4 File Offset: 0x00060AE4
		// (set) Token: 0x06000FDB RID: 4059 RVA: 0x000628FC File Offset: 0x00060AFC
		public List<Point> PointsGood
		{
			get
			{
				return this.pointsGood;
			}
			set
			{
				this.pointsGood = value;
				this.OnPropertyChanged("PointsGood");
			}
		}

		// Token: 0x170004C3 RID: 1219
		// (get) Token: 0x06000FDC RID: 4060 RVA: 0x00062914 File Offset: 0x00060B14
		// (set) Token: 0x06000FDD RID: 4061 RVA: 0x0006292C File Offset: 0x00060B2C
		public List<Point> PointsBad
		{
			get
			{
				return this.pointsBad;
			}
			set
			{
				this.pointsBad = value;
				this.OnPropertyChanged("PointsBad");
			}
		}

		// Token: 0x170004C4 RID: 1220
		// (get) Token: 0x06000FDE RID: 4062 RVA: 0x00062944 File Offset: 0x00060B44
		// (set) Token: 0x06000FDF RID: 4063 RVA: 0x0006295C File Offset: 0x00060B5C
		public List<Point> PointsSelected
		{
			get
			{
				return this.pointsSelected;
			}
			set
			{
				this.pointsSelected = value;
				this.OnPropertyChanged("PointsSelected");
			}
		}

		// Token: 0x170004C5 RID: 1221
		// (get) Token: 0x06000FE0 RID: 4064 RVA: 0x00062974 File Offset: 0x00060B74
		// (set) Token: 0x06000FE1 RID: 4065 RVA: 0x0006298C File Offset: 0x00060B8C
		public List<Point> PointsMaterialCurveGraph
		{
			get
			{
				return this.pointsMaterialCurveGraph;
			}
			set
			{
				this.pointsMaterialCurveGraph = value;
				this.OnPropertyChanged("PointsMaterialCurveGraph");
			}
		}

		// Token: 0x170004C6 RID: 1222
		// (get) Token: 0x06000FE2 RID: 4066 RVA: 0x000629A4 File Offset: 0x00060BA4
		// (set) Token: 0x06000FE3 RID: 4067 RVA: 0x000629BC File Offset: 0x00060BBC
		public int MinX
		{
			get
			{
				return this.minX;
			}
			set
			{
				bool flag = this.minX != value;
				if (flag)
				{
					this.minX = value;
					this.OnPropertyChanged("MinX");
				}
			}
		}

		// Token: 0x170004C7 RID: 1223
		// (get) Token: 0x06000FE4 RID: 4068 RVA: 0x000629F0 File Offset: 0x00060BF0
		// (set) Token: 0x06000FE5 RID: 4069 RVA: 0x00062A08 File Offset: 0x00060C08
		public int MinY
		{
			get
			{
				return this.minY;
			}
			set
			{
				bool flag = this.minY != value;
				if (flag)
				{
					this.minY = value;
					this.OnPropertyChanged("MinY");
				}
			}
		}

		// Token: 0x170004C8 RID: 1224
		// (get) Token: 0x06000FE6 RID: 4070 RVA: 0x00062A3C File Offset: 0x00060C3C
		// (set) Token: 0x06000FE7 RID: 4071 RVA: 0x00062A54 File Offset: 0x00060C54
		public int MaxX
		{
			get
			{
				return this.maxX;
			}
			set
			{
				bool flag = this.maxX != value;
				if (flag)
				{
					this.maxX = value;
					this.OnPropertyChanged("MaxX");
				}
			}
		}

		// Token: 0x170004C9 RID: 1225
		// (get) Token: 0x06000FE8 RID: 4072 RVA: 0x00062A88 File Offset: 0x00060C88
		// (set) Token: 0x06000FE9 RID: 4073 RVA: 0x00062AA0 File Offset: 0x00060CA0
		public int MaxY
		{
			get
			{
				return this.maxY;
			}
			set
			{
				bool flag = this.maxY != value;
				if (flag)
				{
					this.maxY = value;
					this.OnPropertyChanged("MaxY");
				}
			}
		}

		// Token: 0x170004CA RID: 1226
		// (get) Token: 0x06000FEA RID: 4074 RVA: 0x00062AD4 File Offset: 0x00060CD4
		// (set) Token: 0x06000FEB RID: 4075 RVA: 0x00062AEC File Offset: 0x00060CEC
		public XRSImage Image
		{
			get
			{
				return this.image;
			}
			set
			{
				bool flag = this.image != value;
				if (flag)
				{
					this.image = value;
					this.OnPropertyChanged("Image");
				}
			}
		}

		// Token: 0x170004CB RID: 1227
		// (get) Token: 0x06000FEC RID: 4076 RVA: 0x00062B20 File Offset: 0x00060D20
		// (set) Token: 0x06000FED RID: 4077 RVA: 0x00062B38 File Offset: 0x00060D38
		public XRSImage DebugImage
		{
			get
			{
				return this.debugImage;
			}
			set
			{
				bool flag = this.debugImage != value;
				if (flag)
				{
					this.debugImage = value;
					this.OnPropertyChanged("DebugImage");
				}
			}
		}

		// Token: 0x170004CC RID: 1228
		// (get) Token: 0x06000FEE RID: 4078 RVA: 0x00062B6C File Offset: 0x00060D6C
		// (set) Token: 0x06000FEF RID: 4079 RVA: 0x00062B84 File Offset: 0x00060D84
		public int ToleranceMaxSlider
		{
			get
			{
				return this.toleranceMaxSlider;
			}
			set
			{
				bool flag = this.toleranceMaxSlider != value;
				if (flag)
				{
					bool flag2 = value > 50;
					if (flag2)
					{
						this.toleranceMaxSlider = 50;
					}
					else
					{
						bool flag3 = value < 0;
						if (flag3)
						{
							this.toleranceMaxSlider = 0;
						}
						else
						{
							this.toleranceMaxSlider = value;
						}
					}
					this.OnPropertyChanged("ToleranceMaxSlider");
					this.ShowMaterialCurve(this.GetActualMaterialCurveName());
					bool flag4 = this.DebugImage != null;
					if (flag4)
					{
						this.ShowImagePlot(this.DebugImage, this.GetActualMaterialCurveName());
					}
					this.allowReloadXRI = false;
				}
			}
		}

		// Token: 0x170004CD RID: 1229
		// (get) Token: 0x06000FF0 RID: 4080 RVA: 0x00062C10 File Offset: 0x00060E10
		// (set) Token: 0x06000FF1 RID: 4081 RVA: 0x00062C28 File Offset: 0x00060E28
		public int ToleranceMinSlider
		{
			get
			{
				return this.toleranceMinSlider;
			}
			set
			{
				bool flag = this.toleranceMinSlider != value;
				if (flag)
				{
					bool flag2 = value > 50;
					if (flag2)
					{
						this.toleranceMinSlider = 50;
					}
					else
					{
						bool flag3 = value < 0;
						if (flag3)
						{
							this.toleranceMinSlider = 0;
						}
						else
						{
							this.toleranceMinSlider = value;
						}
					}
					this.OnPropertyChanged("ToleranceMinSlider");
					this.ShowMaterialCurve(this.GetActualMaterialCurveName());
					bool flag4 = this.DebugImage != null;
					if (flag4)
					{
						this.ShowImagePlot(this.DebugImage, this.GetActualMaterialCurveName());
					}
					this.allowReloadXRI = false;
				}
			}
		}

		// Token: 0x170004CE RID: 1230
		// (get) Token: 0x06000FF2 RID: 4082 RVA: 0x00062CB4 File Offset: 0x00060EB4
		// (set) Token: 0x06000FF3 RID: 4083 RVA: 0x00062CCC File Offset: 0x00060ECC
		public bool AllowReloadXRI
		{
			get
			{
				return this.allowReloadXRI;
			}
			set
			{
				bool flag = this.allowReloadXRI != value;
				if (flag)
				{
					this.allowReloadXRI = value;
					this.OnPropertyChanged("AllowReloadXRI");
				}
			}
		}

		// Token: 0x170004CF RID: 1231
		// (get) Token: 0x06000FF4 RID: 4084 RVA: 0x00062D00 File Offset: 0x00060F00
		// (set) Token: 0x06000FF5 RID: 4085 RVA: 0x00062D18 File Offset: 0x00060F18
		public bool MultiLaneViewDual
		{
			get
			{
				return this.multiLaneViewDual;
			}
			set
			{
				this.multiLaneViewDual = value;
				this.OnPropertyChanged("MultiLaneViewDual");
			}
		}

		// Token: 0x170004D0 RID: 1232
		// (get) Token: 0x06000FF6 RID: 4086 RVA: 0x00062D30 File Offset: 0x00060F30
		// (set) Token: 0x06000FF7 RID: 4087 RVA: 0x00062D48 File Offset: 0x00060F48
		public int MultiLaneDualGridVisible
		{
			get
			{
				return this.multiLaneDualGridVisible;
			}
			set
			{
				this.multiLaneDualGridVisible = value;
				this.OnPropertyChanged("MultiLaneDualVisible");
			}
		}

		// Token: 0x170004D1 RID: 1233
		// (get) Token: 0x06000FF8 RID: 4088 RVA: 0x00062D60 File Offset: 0x00060F60
		// (set) Token: 0x06000FF9 RID: 4089 RVA: 0x00062D78 File Offset: 0x00060F78
		public Thickness TabBorderThickness
		{
			get
			{
				return this.tabBorderThickness;
			}
			set
			{
				this.tabBorderThickness = value;
				this.OnPropertyChanged("TabBorderThickness");
			}
		}

		// Token: 0x170004D2 RID: 1234
		// (get) Token: 0x06000FFA RID: 4090 RVA: 0x00062D8E File Offset: 0x00060F8E
		// (set) Token: 0x06000FFB RID: 4091 RVA: 0x00062D96 File Offset: 0x00060F96
		public List<LaneXri> AllLanesDual { get; set; }

		// Token: 0x170004D3 RID: 1235
		// (get) Token: 0x06000FFC RID: 4092 RVA: 0x00062D9F File Offset: 0x00060F9F
		// (set) Token: 0x06000FFD RID: 4093 RVA: 0x00062DA7 File Offset: 0x00060FA7
		public List<LaneXri> AllSubProductsDual { get; set; }

		// Token: 0x170004D4 RID: 1236
		// (get) Token: 0x06000FFE RID: 4094 RVA: 0x00062DB0 File Offset: 0x00060FB0
		// (set) Token: 0x06000FFF RID: 4095 RVA: 0x00062DC8 File Offset: 0x00060FC8
		public LaneXri SelectedXriLaneDual
		{
			get
			{
				return this.selectedXriLaneDual;
			}
			set
			{
				bool flag = this.selectedXriLaneDual != value;
				if (flag)
				{
					this.selectedXriLaneDual = value;
					bool flag2 = this.selectedXriLaneDual != null && this.productManager != null && this.productManager.CurrentLoadedProduct != null && this.parHandling != null;
					if (flag2)
					{
						string actualMaterialCurveName = this.GetActualMaterialCurveName();
						this.strGroup = "1";
						this.DebugImage = null;
						bool isLaneProduct = this.productManager.CurrentLoadedProduct.IsLaneProduct;
						if (isLaneProduct)
						{
							bool flag3 = this.selectedXriLaneDual.Lane == Lane.Lane2;
							if (flag3)
							{
								this.strGroup = "2";
							}
							bool flag4 = this.selectedXriLaneDual.Lane == Lane.Lane3;
							if (flag4)
							{
								this.strGroup = "3";
							}
							bool flag5 = this.selectedXriLaneDual.Lane == Lane.Lane4;
							if (flag5)
							{
								this.strGroup = "4";
							}
						}
						bool flag6 = this.productManager.CurrentLoadedProduct.IsMultiProduct && this.selectedXriLaneDual.Group > 0;
						if (flag6)
						{
							this.strGroup = Convert.ToString(this.selectedXriLaneDual.Group);
						}
						string text = this.parHandling.ReadPar(Definition.filterMainPath + this.strGroup + DualEnergy.pathToleranceMax);
						string text2 = this.parHandling.ReadPar(Definition.filterMainPath + this.strGroup + DualEnergy.pathToleranceMin);
						bool flag7 = text != "" && text2 != "";
						if (flag7)
						{
							this.ToleranceMaxSlider = Convert.ToInt32(text);
							this.ToleranceMinSlider = Convert.ToInt32(text2);
						}
						this.strSubproductName = this.ShownLanesDual[this.SortGroupToTabnumber(this.strGroup)].LaneNameCompleteMultiProduct;
						this.ShownLanesDual[this.SortGroupToTabnumber(this.strGroup)].Image = HistoryHelper.GetLatestImage((int)Convert.ToInt16(this.strGroup), this.imageStorer);
						this.Reset_DualGraph();
						bool flag8 = this.ShownLanesDual[this.SortGroupToTabnumber(this.strGroup)].Image != null && this.checkImageVisibility(this.ShownLanesDual[this.SortGroupToTabnumber(this.strGroup)].Image.SubProductName);
						if (flag8)
						{
							this.parHandling.WriteValue(Definition.filterMainPath + this.strGroup + DualEnergy.pathDebug, "1", false);
							this.SendLatestImage();
						}
						else
						{
							this.Image = null;
						}
						this.ShowMaterialCurve(actualMaterialCurveName);
					}
					this.OnPropertyChanged("SelectedXriLane");
				}
			}
		}

		// Token: 0x170004D5 RID: 1237
		// (get) Token: 0x06001000 RID: 4096 RVA: 0x00063070 File Offset: 0x00061270
		// (set) Token: 0x06001001 RID: 4097 RVA: 0x00063078 File Offset: 0x00061278
		public RelayCommand CleanUpCommand { get; set; }

		// Token: 0x170004D6 RID: 1238
		// (get) Token: 0x06001002 RID: 4098 RVA: 0x00063081 File Offset: 0x00061281
		// (set) Token: 0x06001003 RID: 4099 RVA: 0x00063089 File Offset: 0x00061289
		public RelayCommand DecTolMaxMaterialCurveCommand { get; set; }

		// Token: 0x170004D7 RID: 1239
		// (get) Token: 0x06001004 RID: 4100 RVA: 0x00063092 File Offset: 0x00061292
		// (set) Token: 0x06001005 RID: 4101 RVA: 0x0006309A File Offset: 0x0006129A
		public RelayCommand IncTolMaxMaterialCurveCommand { get; set; }

		// Token: 0x170004D8 RID: 1240
		// (get) Token: 0x06001006 RID: 4102 RVA: 0x000630A3 File Offset: 0x000612A3
		// (set) Token: 0x06001007 RID: 4103 RVA: 0x000630AB File Offset: 0x000612AB
		public RelayCommand DecTolMinMaterialCurveCommand { get; set; }

		// Token: 0x170004D9 RID: 1241
		// (get) Token: 0x06001008 RID: 4104 RVA: 0x000630B4 File Offset: 0x000612B4
		// (set) Token: 0x06001009 RID: 4105 RVA: 0x000630BC File Offset: 0x000612BC
		public RelayCommand IncTolMinMaterialCurveCommand { get; set; }

		// Token: 0x170004DA RID: 1242
		// (get) Token: 0x0600100A RID: 4106 RVA: 0x000630C5 File Offset: 0x000612C5
		// (set) Token: 0x0600100B RID: 4107 RVA: 0x000630CD File Offset: 0x000612CD
		public RelayCommand ChangeToleranceMaxCommand { get; set; }

		// Token: 0x170004DB RID: 1243
		// (get) Token: 0x0600100C RID: 4108 RVA: 0x000630D6 File Offset: 0x000612D6
		// (set) Token: 0x0600100D RID: 4109 RVA: 0x000630DE File Offset: 0x000612DE
		public RelayCommand ChangeToleranceMinCommand { get; set; }

		// Token: 0x170004DC RID: 1244
		// (get) Token: 0x0600100E RID: 4110 RVA: 0x000630E7 File Offset: 0x000612E7
		// (set) Token: 0x0600100F RID: 4111 RVA: 0x000630EF File Offset: 0x000612EF
		public RelayCommand SaveMCwithNewTolCommand { get; set; }

		// Token: 0x170004DD RID: 1245
		// (get) Token: 0x06001010 RID: 4112 RVA: 0x000630F8 File Offset: 0x000612F8
		// (set) Token: 0x06001011 RID: 4113 RVA: 0x00063100 File Offset: 0x00061300
		public BoundLocalizedText StrActualImage { get; set; }

		// Token: 0x170004DE RID: 1246
		// (get) Token: 0x06001012 RID: 4114 RVA: 0x00063109 File Offset: 0x00061309
		// (set) Token: 0x06001013 RID: 4115 RVA: 0x00063111 File Offset: 0x00061311
		public BoundLocalizedText StrMaterialCurve { get; set; }

		// Token: 0x170004DF RID: 1247
		// (get) Token: 0x06001014 RID: 4116 RVA: 0x0006311A File Offset: 0x0006131A
		// (set) Token: 0x06001015 RID: 4117 RVA: 0x00063122 File Offset: 0x00061322
		public BoundLocalizedText StrSensitivity { get; set; }

		// Token: 0x170004E0 RID: 1248
		// (get) Token: 0x06001016 RID: 4118 RVA: 0x0006312B File Offset: 0x0006132B
		// (set) Token: 0x06001017 RID: 4119 RVA: 0x00063133 File Offset: 0x00061333
		public BoundLocalizedText StrUpperMaterialCurve { get; set; }

		// Token: 0x170004E1 RID: 1249
		// (get) Token: 0x06001018 RID: 4120 RVA: 0x0006313C File Offset: 0x0006133C
		// (set) Token: 0x06001019 RID: 4121 RVA: 0x00063144 File Offset: 0x00061344
		public BoundLocalizedText StrLowerMaterialCurve { get; set; }

		// Token: 0x170004E2 RID: 1250
		// (get) Token: 0x0600101A RID: 4122 RVA: 0x0006314D File Offset: 0x0006134D
		// (set) Token: 0x0600101B RID: 4123 RVA: 0x00063155 File Offset: 0x00061355
		public ObservableCollection<LaneXri> ShownLanesDual { get; set; }

		// Token: 0x0600101C RID: 4124 RVA: 0x00063160 File Offset: 0x00061360
		public DualEnergyViewModel(StoreImageData imageStorer, ImageDataTransfer imageTransfer, ParameterHandling parHandling, ProductManager productManager, DBConnection dbConnection, MachineState machineState, XRSImageBox imagebox)
		{
			this.imageStorer = imageStorer;
			this.imageTransfer = imageTransfer;
			this.parHandling = parHandling;
			this.productManager = productManager;
			this.dbConnection = dbConnection;
			this.imagebox = imagebox;
			this.machineState = machineState;
			this.CleanUpCommand = new RelayCommand(new Action<object>(this.CleanUp));
			this.SaveMCwithNewTolCommand = new RelayCommand(new Action<object>(this.SaveMaterialCurveWithNewTolerances));
			this.DecTolMaxMaterialCurveCommand = new RelayCommand(new Action<object>(this.DecTolMaxMaterialCurve));
			this.IncTolMaxMaterialCurveCommand = new RelayCommand(new Action<object>(this.IncTolMaxMaterialCurve));
			this.DecTolMinMaterialCurveCommand = new RelayCommand(new Action<object>(this.DecTolMinMaterialCurve));
			this.IncTolMinMaterialCurveCommand = new RelayCommand(new Action<object>(this.IncTolMinMaterialCurve));
			this.ChangeToleranceMaxCommand = new RelayCommand(new Action<object>(this.ChangeToleranceMaxKeyboard));
			this.ChangeToleranceMinCommand = new RelayCommand(new Action<object>(this.ChangeToleranceMinKeyboard));
			imageTransfer.FrameArrived += this.imageTransfer_FrameArrived;
			imagebox.RegionSelectStarted += this.imagebox_RegionSelectStarted;
			imagebox.RegionSelectEnded += this.imagebox_RegionSelectEnded;
			XRSImageBox.IsDualEnergyMenu = true;
			bool flag = !productManager.CurrentLoadedProduct.IsSingleProduct;
			if (flag)
			{
				this.MultiLaneDualGridVisible = 40;
				this.InitializeTabs();
				parHandling.WriteValue(Definition.filterMainPath + DualEnergy.pathDebug, "0", false);
				bool flag2 = productManager.CurrentLoadedProduct.IsMultiProduct && this.ShownLanesDual[0].Image != null && this.checkImageVisibility(this.ShownLanesDual[0].Image.SubProductName);
				if (flag2)
				{
					this.strGroup = Convert.ToString(this.ShownLanesDual[0].Group);
					this.SendLatestImage();
				}
				bool isLaneProduct = productManager.CurrentLoadedProduct.IsLaneProduct;
				if (isLaneProduct)
				{
					this.strGroup = "1";
					this.SendLatestImage();
				}
			}
			else
			{
				this.MultiLaneDualGridVisible = 0;
				parHandling.WriteValue(Definition.filterMainPath + DualEnergy.pathDebug, "1", false);
				this.strGroup = "";
				bool flag3 = !dbConnection.CheckIfMaterialCurveExists(this.GetActualMaterialCurveName());
				if (flag3)
				{
					Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("msgNoMaterialCurveAvailable"), DialogStyle.Warning, DialogButtons.OK);
				}
				this.SendLatestImage();
			}
			string text = parHandling.ReadPar(Definition.filterMainPath + this.strGroup + DualEnergy.pathToleranceMax);
			string text2 = parHandling.ReadPar(Definition.filterMainPath + this.strGroup + DualEnergy.pathToleranceMin);
			bool flag4 = text != "" && text2 != "";
			if (flag4)
			{
				this.ToleranceMaxSlider = Convert.ToInt32(text);
				this.ToleranceMinSlider = Convert.ToInt32(text2);
			}
			this.checktoleranceMax = this.toleranceMaxSlider;
			this.checktoleranceMin = this.toleranceMinSlider;
			this.allowReloadXRI = false;
			parHandling.WriteValue(Definition.filterMainPath + this.strGroup + DualEnergy.requestCurveSubPath, "2", false);
			this.RegisterLocalizedText();
		}

		// Token: 0x0600101D RID: 4125 RVA: 0x0006350C File Offset: 0x0006170C
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x0600101E RID: 4126 RVA: 0x0006353C File Offset: 0x0006173C
		private void CleanUp(object obj)
		{
			this.imageTransfer.FrameArrived -= this.imageTransfer_FrameArrived;
			this.imagebox.RegionSelectStarted -= this.imagebox_RegionSelectStarted;
			this.imagebox.RegionSelectEnded -= this.imagebox_RegionSelectEnded;
			this.parHandling.WriteValue(Definition.filterMainPath + DualEnergy.pathDebug, "0", false);
			bool flag = !this.productManager.CurrentLoadedProduct.IsSingleProduct;
			if (flag)
			{
				for (int i = 0; i < this.productManager.CurrentLoadedProduct.SubProducts.Count; i++)
				{
					this.parHandling.WriteValue(Definition.filterMainPath + Convert.ToString(this.productManager.CurrentLoadedProduct.SubProducts[i].Group) + DualEnergy.pathDebug, "0", false);
				}
			}
			XRSImageBox.IsDualEnergyMenu = false;
			this.DeregisterLocalizedText();
		}

		// Token: 0x0600101F RID: 4127 RVA: 0x00063640 File Offset: 0x00061840
		public void SendLatestImage()
		{
			int imageGroup = 0;
			bool flag = this.strGroup != "";
			if (flag)
			{
				imageGroup = (int)Convert.ToInt16(this.strGroup);
			}
			XRSImage latestImage = HistoryHelper.GetLatestImage(imageGroup, this.imageStorer);
			bool flag2 = latestImage != null;
			if (flag2)
			{
				this.imageTransfer.SendImage(latestImage);
			}
		}

		// Token: 0x06001020 RID: 4128 RVA: 0x00063692 File Offset: 0x00061892
		public void RequestMaterialCurve()
		{
			this.parHandling.WriteValue(Definition.filterMainPath + DualEnergy.requestCurveSubPath, "2", false);
			this.allowReloadXRI = false;
		}

		// Token: 0x06001021 RID: 4129 RVA: 0x000636C0 File Offset: 0x000618C0
		private void ShowImagePlot(XRSImage image, string materialcurvename)
		{
			bool flag = image != null && image.EnergyLevels > 1 && image.Type != ImageType.MaterialCurve && image.Type != ImageType.Histogram && image.Width > 0 && image.Height > 0 && this.dbConnection.CheckIfMaterialCurveExists(materialcurvename);
			if (flag)
			{
				List<Point> pointsTempGood = new List<Point>();
				List<Point> pointsTempBad = new List<Point>();
				int num = (int)(image.Width / 2);
				int num2 = 0;
				int num3 = 0;
				int num4 = (int)(image.Width * image.Height);
				bool flag2 = num4 <= 200000;
				int num5;
				int num6;
				if (flag2)
				{
					num5 = 1;
					num6 = 1;
				}
				else
				{
					bool flag3 = num4 <= 400000;
					if (flag3)
					{
						num5 = 2;
						num6 = 1;
					}
					else
					{
						bool flag4 = num4 <= 800000;
						if (flag4)
						{
							num5 = 2;
							num6 = 2;
						}
						else
						{
							num5 = 4;
							num6 = 4;
						}
					}
				}
				this.GetToleranceValues(out num2, out num3);
				for (int i = 0; i < (int)image.Height; i += num5)
				{
					for (int j = 0; j < num; j += num6)
					{
						bool flag5 = (image.Mask[i * (int)image.Width + j] & XRSImageDefs.edgeFilterIndicator) != XRSImageDefs.edgeFilterIndicator;
						if (flag5)
						{
							short num7 = image[i, j];
							short num8 = image[i, j + num];
							bool flag6 = (image.Mask[i * (int)image.Width + j] & 32) == 32;
							if (flag6)
							{
								pointsTempBad.Add(new Point((double)num7, (double)num8));
							}
							else
							{
								pointsTempGood.Add(new Point((double)num7, (double)num8));
							}
						}
					}
				}
				Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
				{
					this.PointsGood = pointsTempGood;
					this.PointsBad = pointsTempBad;
				}));
			}
			else
			{
				this.PointsGood = new List<Point>();
				this.PointsBad = new List<Point>();
				this.PointsSelected = new List<Point>();
			}
		}

		// Token: 0x06001022 RID: 4130 RVA: 0x000638E4 File Offset: 0x00061AE4
		private void imageTransfer_FrameArrived(object sender, ReceiveEventArgs e)
		{
			bool flag = this.machineState.State != MachineStates.XrayOnCalibrationNotDone && e.Image.Type != ImageType.TestcardSimulation;
			if (flag)
			{
				bool flag2 = e.Image.Type == ImageType.Debug && e.Image.EnergyLevels > 1;
				if (flag2)
				{
					this.ShowImagePlot(e.Image, this.GetActualMaterialCurveName());
					this.DebugImage = e.Image;
					bool flag3 = !this.productManager.CurrentLoadedProduct.IsSingleProduct;
					if (flag3)
					{
						this.parHandling.WriteValue(Definition.filterMainPath + this.strGroup + DualEnergy.pathDebug, "0", false);
					}
				}
				bool flag4 = e.Image.Type == ImageType.MaterialCurve && e.Image.State == 1;
				if (flag4)
				{
					this.materialCurve = new List<MaterialCurveElement>();
					for (int i = 0; i < e.Image.Data.Length; i += 3)
					{
						this.materialCurve.Add(new MaterialCurveElement((int)e.Image.Data[i], (int)e.Image.Data[i + 1], (int)e.Image.Data[i + 2]));
					}
					this.ShowMaterialCurve(this.GetActualMaterialCurveName());
				}
				bool flag5 = e.Image.Type == ImageType.Demo;
				if (flag5)
				{
					this.Image = e.Image;
				}
				bool flag6 = e.Image.Type == ImageType.Normal;
				if (flag6)
				{
					bool flag7 = !this.productManager.CurrentLoadedProduct.IsSingleProduct;
					if (flag7)
					{
						bool flag8 = (this.productManager.CurrentLoadedProduct.IsMultiProduct && this.checkImageVisibility(e.Image.SubProductName)) || (this.productManager.CurrentLoadedProduct.IsLaneProduct && e.Image.Lane == this.selectedXriLaneDual.Lane);
						if (flag8)
						{
							this.parHandling.WriteValue(Definition.filterMainPath + this.strGroup + DualEnergy.pathDebug, "1", false);
							this.SendLatestImage();
						}
					}
					else
					{
						this.Image = e.Image;
					}
				}
			}
		}

		// Token: 0x06001023 RID: 4131 RVA: 0x00063B34 File Offset: 0x00061D34
		private bool checkImageVisibility(string name)
		{
			bool isMultiProduct = this.productManager.CurrentLoadedProduct.IsMultiProduct;
			bool result;
			if (isMultiProduct)
			{
				bool flag = name == this.strSubproductName;
				result = flag;
			}
			else
			{
				result = true;
			}
			return result;
		}

		// Token: 0x06001024 RID: 4132 RVA: 0x00063B74 File Offset: 0x00061D74
		private void ShowMaterialCurve(string materialcurvename)
		{
			bool flag = this.dbConnection.CheckIfMaterialCurveExists(materialcurvename);
			if (flag)
			{
				int num = 0;
				int num2 = 0;
				int num3 = 0;
				int num4 = 0;
				this.GraphPointsTempLower.Clear();
				this.GraphPointsTempUpper.Clear();
				List<MaterialCurveElement> list = new List<MaterialCurveElement>();
				this.dbConnection.GetMaterialCurve(materialcurvename, list);
				this.GetToleranceValues(out num, out num2);
				bool flag2 = this.toleranceMaxSlider >= num;
				int num5;
				bool flag3;
				if (flag2)
				{
					num5 = this.toleranceMaxSlider - num;
					flag3 = true;
				}
				else
				{
					num5 = num - this.toleranceMaxSlider;
					flag3 = false;
				}
				bool flag4 = this.toleranceMinSlider >= num2;
				int num6;
				bool flag5;
				if (flag4)
				{
					num6 = this.toleranceMinSlider - num2;
					flag5 = false;
				}
				else
				{
					num6 = num2 - this.toleranceMinSlider;
					flag5 = true;
				}
				for (int i = 0; i < list.Count; i++)
				{
					this.GraphPointsTempLower.Add(new Point((double)list[i].Low, (double)(flag5 ? (list[i].MinHigh + num6) : (list[i].MinHigh - num6))));
					this.GraphPointsTempUpper.Add(new Point((double)list[i].Low, (double)(flag3 ? (list[i].MaxHigh + num5) : (list[i].MaxHigh - num5))));
				}
				int num7 = Convert.ToInt32(this.GraphPointsTempUpper[0].Y);
				for (int j = 0; j < list.Count; j++)
				{
					bool flag6 = this.GraphPointsTempUpper[j].Y != (double)num7;
					if (flag6)
					{
						num4 = j;
						break;
					}
				}
				int num8 = Convert.ToInt32(this.GraphPointsTempUpper[list.Count - 1].Y);
				for (int k = list.Count - 1; k > 1000; k--)
				{
					bool flag7 = this.GraphPointsTempUpper[k].Y != (double)num8;
					if (flag7)
					{
						num3 = k;
						break;
					}
				}
				this.PointsMaterialCurveGraph.Clear();
				for (int l = num4; l < num3; l++)
				{
					this.PointsMaterialCurveGraph.Add(this.GraphPointsTempUpper[l]);
				}
				for (int m = 0; m < num3 - num4; m++)
				{
					this.PointsMaterialCurveGraph.Add(this.GraphPointsTempLower[num3 - m]);
				}
				this.PointsMaterialCurveGraph.Add(this.GraphPointsTempUpper[num4]);
				Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
				{
					bool flag8 = this.pointsMaterialCurveGraph != null;
					if (flag8)
					{
						this.MinX = (int)this.PointsMaterialCurveGraph.Min((Point point) => point.X - 100.0);
						bool flag9 = this.minX < 0;
						if (flag9)
						{
							this.MinX = 0;
						}
						this.MinY = (int)this.PointsMaterialCurveGraph.Min((Point point) => point.Y - 100.0);
						bool flag10 = this.minY < 0;
						if (flag10)
						{
							this.MinY = 0;
						}
						this.MaxX = (int)this.PointsMaterialCurveGraph.Max((Point point) => point.X + 100.0);
						bool flag11 = this.maxX < 0;
						if (flag11)
						{
							this.MaxX = 0;
						}
						this.MaxY = (int)this.PointsMaterialCurveGraph.Max((Point point) => point.Y + 100.0);
						bool flag12 = this.maxY < 0;
						if (flag12)
						{
							this.MaxY = 0;
						}
					}
				}));
			}
			else
			{
				this.PointsMaterialCurveGraph = new List<Point>();
			}
		}

		// Token: 0x06001025 RID: 4133 RVA: 0x00063E98 File Offset: 0x00062098
		private void GetToleranceValues(out int toleranceMax, out int toleranceMin)
		{
			string s = this.parHandling.ReadPar(Definition.filterMainPath + this.strGroup + DualEnergy.pathToleranceMax);
			int.TryParse(s, out toleranceMax);
			s = this.parHandling.ReadPar(Definition.filterMainPath + this.strGroup + DualEnergy.pathToleranceMin);
			int.TryParse(s, out toleranceMin);
		}

		// Token: 0x06001026 RID: 4134 RVA: 0x00063EF8 File Offset: 0x000620F8
		private void imagebox_RegionSelectEnded(Point endPoint)
		{
			bool flag = this.DebugImage != null && this.dbConnection.CheckIfMaterialCurveExists(this.GetActualMaterialCurveName());
			if (flag)
			{
				bool flag2 = this.startPoint.X > endPoint.X;
				int num;
				int num2;
				if (flag2)
				{
					num = (int)this.startPoint.X;
					num2 = (int)endPoint.X;
				}
				else
				{
					num = (int)endPoint.X;
					num2 = (int)this.startPoint.X;
				}
				bool flag3 = this.startPoint.Y > endPoint.Y;
				int num3;
				int num4;
				if (flag3)
				{
					num3 = (int)this.startPoint.Y;
					num4 = (int)endPoint.Y;
				}
				else
				{
					num3 = (int)endPoint.Y;
					num4 = (int)this.startPoint.Y;
				}
				List<Point> list = new List<Point>();
				int num5 = (int)(this.debugImage.Width / 2);
				int num6 = (num - num2) * (num3 - num4);
				bool flag4 = num6 <= 20000;
				int num7;
				int num8;
				if (flag4)
				{
					num7 = 1;
					num8 = 1;
				}
				else
				{
					bool flag5 = num6 <= 40000;
					if (flag5)
					{
						num7 = 2;
						num8 = 1;
					}
					else
					{
						bool flag6 = num6 <= 80000;
						if (flag6)
						{
							num7 = 2;
							num8 = 2;
						}
						else
						{
							bool flag7 = num6 <= 320000;
							if (flag7)
							{
								num7 = 4;
								num8 = 4;
							}
							else
							{
								num7 = 8;
								num8 = 8;
							}
						}
					}
				}
				for (int i = num4; i < num3; i += num7)
				{
					for (int j = num2; j < num; j += num8)
					{
						short num9 = this.DebugImage[i, j];
						short num10 = this.DebugImage[i, j + num5];
						bool flag8 = (this.image.Mask[i * (int)this.image.Width + j] & XRSImageDefs.edgeFilterIndicator) != XRSImageDefs.edgeFilterIndicator;
						if (flag8)
						{
							list.Add(new Point((double)num9, (double)num10));
						}
					}
				}
				this.PointsSelected = list;
			}
		}

		// Token: 0x06001027 RID: 4135 RVA: 0x00064110 File Offset: 0x00062310
		private void imagebox_RegionSelectStarted(Point startPoint)
		{
			List<Point> list = new List<Point>();
			this.PointsSelected = list;
			this.startPoint = startPoint;
		}

		// Token: 0x06001028 RID: 4136 RVA: 0x00064134 File Offset: 0x00062334
		private void Reset_DualGraph()
		{
			List<Point> list = new List<Point>();
			this.PointsSelected = list;
			this.PointsGood = list;
			this.PointsBad = list;
		}

		// Token: 0x06001029 RID: 4137 RVA: 0x00064160 File Offset: 0x00062360
		private void SendCorrectedMaterialCurve(List<MaterialCurveElement> materialcurve, int group)
		{
			Thread thread = new Thread(delegate()
			{
				DualEnergy dualEnergy = new DualEnergy(this.imageTransfer, this.parHandling);
				dualEnergy.SendMaterialCurveImage(materialcurve, group);
			});
			thread.Start();
		}

		// Token: 0x0600102A RID: 4138 RVA: 0x000641A4 File Offset: 0x000623A4
		private void SaveMaterialCurveWithNewTolerances(object obj)
		{
			List<MaterialCurveElement> list = new List<MaterialCurveElement>();
			bool flag = Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("msgSaveDualMaterialCurveDB"), DialogStyle.Warning, DialogButtons.OKCancel) == Dialog.DialogResults.Ok;
			if (flag)
			{
				bool flag2 = this.dbConnection != null && this.productManager.CurrentLoadedProduct != null && (this.checktoleranceMin != this.toleranceMinSlider || this.checktoleranceMax != this.toleranceMaxSlider) && this.GraphPointsTempUpper.Count != 0 && this.GraphPointsTempLower.Count != 0;
				if (flag2)
				{
					this.checktoleranceMin = this.toleranceMinSlider;
					this.checktoleranceMax = this.toleranceMaxSlider;
					this.dbConnection.GetMaterialCurve(this.GetActualMaterialCurveName(), list);
					for (int i = 0; i < list.Count; i++)
					{
						bool flag3 = list[i].MaxHigh != 0 && list[i].MinHigh != 0;
						if (flag3)
						{
							list[i].MaxHigh = Convert.ToInt32(this.GraphPointsTempUpper[i].Y);
							list[i].MinHigh = Convert.ToInt32(this.GraphPointsTempLower[i].Y);
						}
					}
					this.dbConnection.SetMaterialCurve(this.GetActualMaterialCurveName(), list);
					this.parHandling.WriteValue(Definition.filterMainPath + this.strGroup + DualEnergy.pathToleranceMax, Convert.ToString(this.toleranceMaxSlider));
					this.parHandling.WriteValue(Definition.filterMainPath + this.strGroup + DualEnergy.pathToleranceMin, Convert.ToString(this.toleranceMinSlider));
					int group = 0;
					bool flag4 = this.strGroup != "";
					if (flag4)
					{
						group = (int)Convert.ToInt16(this.strGroup);
					}
					this.SendCorrectedMaterialCurve(list, group);
					this.allowReloadXRI = true;
				}
			}
		}

		// Token: 0x0600102B RID: 4139 RVA: 0x000643A3 File Offset: 0x000625A3
		private void DecTolMaxMaterialCurve(object obj)
		{
			this.ToleranceMaxSlider--;
		}

		// Token: 0x0600102C RID: 4140 RVA: 0x000643B5 File Offset: 0x000625B5
		private void IncTolMaxMaterialCurve(object obj)
		{
			this.ToleranceMaxSlider++;
		}

		// Token: 0x0600102D RID: 4141 RVA: 0x000643C7 File Offset: 0x000625C7
		private void DecTolMinMaterialCurve(object obj)
		{
			this.ToleranceMinSlider--;
		}

		// Token: 0x0600102E RID: 4142 RVA: 0x000643D9 File Offset: 0x000625D9
		private void IncTolMinMaterialCurve(object obj)
		{
			this.ToleranceMinSlider++;
		}

		// Token: 0x0600102F RID: 4143 RVA: 0x000643EC File Offset: 0x000625EC
		private void ChangeToleranceMaxKeyboard(object j)
		{
			string oldValue = this.ToleranceMaxSlider.ToString();
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtUpperMaterialCurve"), oldValue, NumKeyboardType.TypeInt);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_InputToleranceMax;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x06001030 RID: 4144 RVA: 0x00064444 File Offset: 0x00062644
		private void ChangeToleranceMinKeyboard(object j)
		{
			string oldValue = this.ToleranceMinSlider.ToString();
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtLowerMaterialCurve"), oldValue, NumKeyboardType.TypeInt);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_InputToleranceMin;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x06001031 RID: 4145 RVA: 0x0006449C File Offset: 0x0006269C
		private void numKeyboard_InputToleranceMax(object sender, KeyboardInputEventArgs e)
		{
			int num = int.Parse(e.Text, new CultureInfo("de-DE"));
			this.ToleranceMaxSlider = num;
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_InputToleranceMax;
		}

		// Token: 0x06001032 RID: 4146 RVA: 0x000644E0 File Offset: 0x000626E0
		private void numKeyboard_InputToleranceMin(object sender, KeyboardInputEventArgs e)
		{
			int num = int.Parse(e.Text, new CultureInfo("de-DE"));
			this.ToleranceMinSlider = num;
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_InputToleranceMin;
		}

		// Token: 0x06001033 RID: 4147 RVA: 0x00064524 File Offset: 0x00062724
		public void SaveMaterialCurveAsPng(Grid chartGrid, Rect bounds)
		{
			string text = "";
			string text2 = "";
			string text3 = DualEnergyViewModel.materialCurveFolderName;
			string text4 = null;
			bool flag = this.dbConnection == null;
			if (flag)
			{
				Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtNoDatabase"), DialogStyle.Error);
			}
			else
			{
				bool flag2 = !Directory.Exists(text3);
				if (flag2)
				{
					Directory.CreateDirectory(text3);
				}
				this.dbConnection.GetUsbDriveLetter(ref text);
				this.dbConnection.GetSerialNumber(ref text4);
				try
				{
					List<string> list = Directory.GetFiles(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\MaterialCurveDualEnergy").ToList<string>();
					list.Sort(new NamesComparer());
					int count = list.Count;
					int datanumber = count / 2 + 1;
					string str = string.Concat(new string[]
					{
						datanumber.ToString(),
						"_MaterialCurve_",
						this.productManager.CurrentLoadedProduct.TableName,
						"_",
						DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_ff"),
						(text4 != null && text4.Length > 0) ? ("_" + text4) : "",
						".png"
					});
					text2 = text + "\\MaterialCurveDualEnergy";
					bool flag3 = Directory.Exists(text);
					if (flag3)
					{
						bool flag4 = !Directory.Exists(text2);
						if (flag4)
						{
							Directory.CreateDirectory(text2);
						}
					}
					this.CopyXRIFromHistoryToMaterialCurveFolder(datanumber, text3, text2);
					text3 = text3 + "\\" + str;
					text2 = text + "\\MaterialCurveDualEnergy\\" + str;
					FileInfo fileInfo = new FileInfo(text2);
					RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap((int)bounds.Width, (int)bounds.Height, 96.0, 96.0, PixelFormats.Pbgra32);
					DrawingVisual drawingVisual = new DrawingVisual();
					using (DrawingContext drawingContext = drawingVisual.RenderOpen())
					{
						VisualBrush brush = new VisualBrush(chartGrid);
						drawingContext.DrawRectangle(brush, null, new Rect(default(Point), bounds.Size));
					}
					renderTargetBitmap.Render(drawingVisual);
					FileStream fileStream = new FileStream(text3, FileMode.Create);
					new PngBitmapEncoder
					{
						Frames = 
						{
							BitmapFrame.Create(renderTargetBitmap)
						}
					}.Save(fileStream);
					fileStream.Close();
					bool flag5 = Directory.Exists(text + "\\");
					if (flag5)
					{
						fileStream = new FileStream(text2, FileMode.Create);
						new PngBitmapEncoder
						{
							Frames = 
							{
								BitmapFrame.Create(renderTargetBitmap)
							}
						}.Save(fileStream);
						fileStream.Close();
					}
					Dialog.Show(LocalizationManager.Localize("txtSuccess"), LocalizationManager.Localize("txtSaveSuccessful"), DialogStyle.Ok, DialogButtons.None, true);
				}
				catch (UnauthorizedAccessException)
				{
					Dialog.Show(LocalizationManager.Localize("txtError"), string.Concat(new string[]
					{
						LocalizationManager.Localize("msgPathAccess"),
						" \"",
						Path.GetDirectoryName(text2),
						"\" ",
						LocalizationManager.Localize("msgForbidden")
					}), DialogStyle.Error);
				}
				catch
				{
					Dialog.Show(LocalizationManager.Localize("txtError"), string.Concat(new string[]
					{
						LocalizationManager.Localize("msgPathAccess"),
						" \"",
						Path.GetDirectoryName(text2),
						"\" ",
						LocalizationManager.Localize("msgForbidden")
					}), DialogStyle.Error);
				}
			}
		}

		// Token: 0x06001034 RID: 4148 RVA: 0x000648EC File Offset: 0x00062AEC
		private void CopyXRIFromHistoryToMaterialCurveFolder(int datanumber, string destination1, string destination2)
		{
			bool flag = true;
			bool flag2 = Directory.Exists(StoreImageData.archiveDirectoryThroughput) || Directory.Exists(StoreImageData.archiveDirectoryReject);
			if (flag2)
			{
				string latestImageName = HistoryHelper.GetLatestImageName(ref flag);
				bool flag3 = flag;
				string sourceFileName;
				if (flag3)
				{
					sourceFileName = StoreImageData.archiveDirectoryThroughput + latestImageName;
				}
				else
				{
					sourceFileName = StoreImageData.archiveDirectoryReject + latestImageName;
				}
				string destFileName = string.Concat(new string[]
				{
					destination1,
					"\\",
					datanumber.ToString(),
					"_",
					latestImageName
				});
				string destFileName2 = string.Concat(new string[]
				{
					destination2,
					"\\",
					datanumber.ToString(),
					"_",
					latestImageName
				});
				try
				{
					File.Copy(sourceFileName, destFileName, true);
					File.Copy(sourceFileName, destFileName2, true);
				}
				catch
				{
				}
			}
		}

		// Token: 0x06001035 RID: 4149 RVA: 0x000649D8 File Offset: 0x00062BD8
		private void InitializeTabs()
		{
			this.ShownLanesDual = new ObservableCollection<LaneXri>();
			this.AllLanesDual = new List<LaneXri>();
			this.AllSubProductsDual = new List<LaneXri>();
			LaneXri laneXri = new LaneXri(Lane.Lane1, this.imageTransfer, this.parHandling, this.dbConnection, null, this.productManager, null, this.machineState, this.imageStorer, true, null, null, null);
			this.AllLanesDual.Add(laneXri);
			laneXri = new LaneXri(Lane.Lane2, this.imageTransfer, this.parHandling, this.dbConnection, null, this.productManager, null, this.machineState, this.imageStorer, true, null, null, null);
			this.AllLanesDual.Add(laneXri);
			laneXri = new LaneXri(Lane.Lane3, this.imageTransfer, this.parHandling, this.dbConnection, null, this.productManager, null, this.machineState, this.imageStorer, true, null, null, null);
			this.AllLanesDual.Add(laneXri);
			laneXri = new LaneXri(Lane.Lane4, this.imageTransfer, this.parHandling, this.dbConnection, null, this.productManager, null, this.machineState, this.imageStorer, true, null, null, null);
			this.AllLanesDual.Add(laneXri);
			for (int i = 0; i <= Definition.maxSubProducts; i++)
			{
				laneXri = new LaneXri(Lane.All, this.imageTransfer, this.parHandling, this.dbConnection, null, this.productManager, null, this.machineState, this.imageStorer, true, null, null, null);
				laneXri.Group = i;
				this.AllSubProductsDual.Add(laneXri);
			}
			this.SetActiveLanes();
		}

		// Token: 0x06001036 RID: 4150 RVA: 0x00064B68 File Offset: 0x00062D68
		private void SetActiveLanes()
		{
			bool flag = false;
			this.dbConnection.GetMultiLaneView(ref this.multiLaneViewDual);
			this.MultiLaneViewDual = this.multiLaneViewDual;
			XRSImageBox.multiLaneSupport = this.multiLaneViewDual;
			this.dbConnection.GetMultiProductView(ref flag);
			this.ShownLanesDual.Clear();
			bool flag2 = !this.multiLaneViewDual;
			if (flag2)
			{
				bool flag3 = !flag || this.productManager == null || this.productManager.CurrentLoadedProduct == null || (!this.productManager.CurrentLoadedProduct.IsMultiProduct && !this.productManager.CurrentLoadedProduct.IsRegionProduct);
				if (flag3)
				{
					this.TabBorderThickness = new Thickness(0.0);
					foreach (LaneXri laneXri in this.AllLanesDual)
					{
						laneXri.TabHeight = 0;
						bool flag4 = laneXri.Lane > Lane.All;
						if (flag4)
						{
							laneXri.Active = false;
						}
						else
						{
							laneXri.Active = true;
							this.ShownLanesDual.Add(laneXri);
							this.SelectedXriLaneDual = laneXri;
						}
					}
				}
				else
				{
					this.SetMultiProductViews();
				}
			}
			else
			{
				this.SetMultiLaneViews();
			}
			bool flag5 = this.ShownLanesDual.Count > 0 && (this.selectedXriLaneDual == null || !this.ShownLanesDual.Contains(this.selectedXriLaneDual));
			if (flag5)
			{
				List<LaneXri> list = (from lane in this.ShownLanesDual
				where lane.Lane == Lane.All
				select lane).ToList<LaneXri>();
				bool flag6 = list.Count > 0;
				if (flag6)
				{
					this.SelectedXriLaneDual = list[0];
				}
				else
				{
					this.SelectedXriLaneDual = this.ShownLanesDual[0];
				}
			}
		}

		// Token: 0x06001037 RID: 4151 RVA: 0x00064D68 File Offset: 0x00062F68
		private void SetMultiLaneViews()
		{
			string a = "";
			int num = 0;
			this.ShownLanesDual.Clear();
			this.TabBorderThickness = new Thickness(1.0, 1.0, 0.0, 0.0);
			this.dbConnection.GetParameterValue(Acquisition.lane1EnablePath, ref a);
			bool flag = a == "1";
			if (flag)
			{
				List<LaneXri> list = (from lane in this.AllLanesDual
				where lane.Lane == Lane.Lane1
				select lane).ToList<LaneXri>();
				bool flag2 = list.Count > 0;
				if (flag2)
				{
					list[0].Active = true;
					list[0].TabHeight = this.laneTabHeight;
					list[0].UpdateLaneName();
					this.ShownLanesDual.Add(list[0]);
					num++;
				}
			}
			this.dbConnection.GetParameterValue(Acquisition.lane2EnablePath, ref a);
			bool flag3 = a == "1";
			if (flag3)
			{
				List<LaneXri> list2 = (from lane in this.AllLanesDual
				where lane.Lane == Lane.Lane2
				select lane).ToList<LaneXri>();
				bool flag4 = list2.Count > 0;
				if (flag4)
				{
					list2[0].Active = true;
					list2[0].TabHeight = this.laneTabHeight;
					list2[0].UpdateLaneName();
					this.ShownLanesDual.Add(list2[0]);
					num++;
				}
			}
			this.dbConnection.GetParameterValue(Acquisition.lane3EnablePath, ref a);
			bool flag5 = a == "1";
			if (flag5)
			{
				List<LaneXri> list3 = (from lane in this.AllLanesDual
				where lane.Lane == Lane.Lane3
				select lane).ToList<LaneXri>();
				bool flag6 = list3.Count > 0;
				if (flag6)
				{
					list3[0].Active = true;
					list3[0].TabHeight = this.laneTabHeight;
					list3[0].UpdateLaneName();
					this.ShownLanesDual.Add(list3[0]);
					num++;
				}
			}
			this.dbConnection.GetParameterValue(Acquisition.lane4EnablePath, ref a);
			bool flag7 = a == "1";
			if (flag7)
			{
				List<LaneXri> list4 = (from lane in this.AllLanesDual
				where lane.Lane == Lane.Lane4
				select lane).ToList<LaneXri>();
				bool flag8 = list4.Count > 0;
				if (flag8)
				{
					list4[0].Active = true;
					list4[0].TabHeight = this.laneTabHeight;
					list4[0].UpdateLaneName();
					this.ShownLanesDual.Add(list4[0]);
					num++;
				}
			}
		}

		// Token: 0x06001038 RID: 4152 RVA: 0x00065088 File Offset: 0x00063288
		private void SetMultiProductViews()
		{
			List<LaneXri> list = new List<LaneXri>();
			this.TabBorderThickness = new Thickness(1.0, 1.0, 0.0, 0.0);
			int num = 15;
			this.ShownLanesDual.Clear();
			bool flag = this.productManager.CurrentLoadedProduct.SubProducts.Count > 0;
			if (flag)
			{
				num = 9;
			}
			bool flag2 = this.productManager.CurrentLoadedProduct.SubProducts.Count > 5;
			if (flag2)
			{
				this.laneTabHeight = 30;
				num = 5;
			}
			bool flag3 = this.productManager.CurrentLoadedProduct.SubProducts.Count > 10;
			if (flag3)
			{
				this.laneTabHeight = 20;
				num = 5;
			}
			List<ProductData> list2 = new List<ProductData>();
			list2.AddRange(this.productManager.CurrentLoadedProduct.SubProducts);
			list2.Sort(new ProductDataComparerGeneric());
			foreach (ProductData productData in list2)
			{
				int group = 0;
				string s = productData.TableName.Remove(0, productData.TableName.IndexOf(Definition.subTableIdentifier) + 1);
				bool flag4 = int.TryParse(s, out group);
				if (flag4)
				{
					List<LaneXri> list3 = (from subProdLane in this.AllSubProductsDual
					where subProdLane.Group == @group
					select subProdLane).ToList<LaneXri>();
					bool flag5 = list3.Count > 0;
					if (flag5)
					{
						list3[0].Active = true;
						list3[0].TabHeight = this.laneTabHeight;
						list3[0].LaneName = ((productData.ProductName.Length <= num) ? productData.ProductName : productData.ProductName.Substring(0, num));
						list3[0].LaneNameCompleteMultiProduct = productData.ProductName;
						this.ShownLanesDual.Add(list3[0]);
					}
				}
			}
		}

		// Token: 0x06001039 RID: 4153 RVA: 0x000652C8 File Offset: 0x000634C8
		private string GetActualMaterialCurveName()
		{
			string tableName = this.productManager.CurrentLoadedProduct.TableName;
			bool flag = this.productManager.CurrentLoadedProduct.IsLaneProduct && this.MultiLaneViewDual;
			string result;
			if (flag)
			{
				string str = "1";
				bool flag2 = this.selectedXriLaneDual.Lane == Lane.Lane2;
				if (flag2)
				{
					str = "2";
				}
				bool flag3 = this.selectedXriLaneDual.Lane == Lane.Lane3;
				if (flag3)
				{
					str = "3";
				}
				bool flag4 = this.selectedXriLaneDual.Lane == Lane.Lane4;
				if (flag4)
				{
					str = "4";
				}
				result = tableName + Definition.subTableIdentifier.ToString() + str;
			}
			else
			{
				bool isMultiProduct = this.productManager.CurrentLoadedProduct.IsMultiProduct;
				if (isMultiProduct)
				{
					result = tableName + Definition.subTableIdentifier.ToString() + this.selectedXriLaneDual.Group.ToString();
				}
				else
				{
					result = tableName;
				}
			}
			return result;
		}

		// Token: 0x0600103A RID: 4154 RVA: 0x000653BC File Offset: 0x000635BC
		private int SortGroupToTabnumber(string group)
		{
			int num = Convert.ToInt32(group);
			bool isLaneProduct = this.productManager.CurrentLoadedProduct.IsLaneProduct;
			if (isLaneProduct)
			{
				num--;
			}
			bool isMultiProduct = this.productManager.CurrentLoadedProduct.IsMultiProduct;
			if (isMultiProduct)
			{
				for (int i = 0; i < this.ShownLanesDual.Count; i++)
				{
					bool flag = num == this.ShownLanesDual[i].Group;
					if (flag)
					{
						num = i;
						break;
					}
				}
			}
			return num;
		}

		// Token: 0x0600103B RID: 4155 RVA: 0x00065444 File Offset: 0x00063644
		private void RegisterLocalizedText()
		{
			this.StrActualImage = new BoundLocalizedText("txtActualImage");
			this.StrMaterialCurve = new BoundLocalizedText("txtMaterialCurve");
			this.StrSensitivity = new BoundLocalizedText("txtSensitivity");
			this.StrUpperMaterialCurve = new BoundLocalizedText("txtUpperMaterialCurve");
			this.StrLowerMaterialCurve = new BoundLocalizedText("txtLowerMaterialCurve");
		}

		// Token: 0x0600103C RID: 4156 RVA: 0x000654A8 File Offset: 0x000636A8
		private void DeregisterLocalizedText()
		{
			MainWindow.localizationManager.Deregister(this.StrActualImage);
			MainWindow.localizationManager.Deregister(this.StrMaterialCurve);
			MainWindow.localizationManager.Deregister(this.StrSensitivity);
			MainWindow.localizationManager.Deregister(this.StrUpperMaterialCurve);
			MainWindow.localizationManager.Deregister(this.StrLowerMaterialCurve);
		}

		// Token: 0x04000AB0 RID: 2736
		private StoreImageData imageStorer;

		// Token: 0x04000AB1 RID: 2737
		private ImageDataTransfer imageTransfer;

		// Token: 0x04000AB2 RID: 2738
		private ParameterHandling parHandling;

		// Token: 0x04000AB3 RID: 2739
		private ProductManager productManager;

		// Token: 0x04000AB4 RID: 2740
		private DBConnection dbConnection;

		// Token: 0x04000AB5 RID: 2741
		private List<Point> pointsGood = new List<Point>();

		// Token: 0x04000AB6 RID: 2742
		private List<Point> pointsBad = new List<Point>();

		// Token: 0x04000AB7 RID: 2743
		private List<Point> pointsSelected = new List<Point>();

		// Token: 0x04000AB8 RID: 2744
		private List<Point> pointsMaterialCurveGraph = new List<Point>();

		// Token: 0x04000AB9 RID: 2745
		private List<MaterialCurveElement> materialCurve = new List<MaterialCurveElement>();

		// Token: 0x04000ABA RID: 2746
		private string strGroup;

		// Token: 0x04000ABB RID: 2747
		private string strSubproductName;

		// Token: 0x04000ABC RID: 2748
		private XRSImage image;

		// Token: 0x04000ABD RID: 2749
		private XRSImage debugImage;

		// Token: 0x04000ABE RID: 2750
		private XRSImageBox imagebox;

		// Token: 0x04000ABF RID: 2751
		private int toleranceMaxSlider;

		// Token: 0x04000AC0 RID: 2752
		private int toleranceMinSlider;

		// Token: 0x04000AC1 RID: 2753
		private int checktoleranceMin;

		// Token: 0x04000AC2 RID: 2754
		private int checktoleranceMax;

		// Token: 0x04000AC3 RID: 2755
		private Point startPoint;

		// Token: 0x04000AC4 RID: 2756
		private MachineState machineState;

		// Token: 0x04000AC5 RID: 2757
		private List<Point> GraphPointsTempLower = new List<Point>();

		// Token: 0x04000AC6 RID: 2758
		private List<Point> GraphPointsTempUpper = new List<Point>();

		// Token: 0x04000AC8 RID: 2760
		public static readonly string materialCurveFolderName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\MaterialCurveDualEnergy";

		// Token: 0x04000AC9 RID: 2761
		private bool allowReloadXRI;

		// Token: 0x04000ACA RID: 2762
		private bool multiLaneViewDual = false;

		// Token: 0x04000ACB RID: 2763
		private Thickness tabBorderThickness;

		// Token: 0x04000ACC RID: 2764
		private int multiLaneDualGridVisible = 0;

		// Token: 0x04000ACD RID: 2765
		private int laneTabHeight = 40;

		// Token: 0x04000ACE RID: 2766
		private LaneXri selectedXriLaneDual;

		// Token: 0x04000ACF RID: 2767
		private const int maxSubproductCount = 20;

		// Token: 0x04000AD0 RID: 2768
		private int minX;

		// Token: 0x04000AD1 RID: 2769
		private int minY;

		// Token: 0x04000AD2 RID: 2770
		private int maxX;

		// Token: 0x04000AD3 RID: 2771
		private int maxY;

		// Token: 0x04000ADD RID: 2781
		private NumKeyboard numKeyboard;
	}
}
