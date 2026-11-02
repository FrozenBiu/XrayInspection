using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Common;
using DatabaseConnection;
using Definitions;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x020000AC RID: 172
	public class CtrlAcquisitionSettingsViewModel : INotifyPropertyChanged
	{
		// Token: 0x17000529 RID: 1321
		// (get) Token: 0x06001110 RID: 4368 RVA: 0x00068660 File Offset: 0x00066860
		// (set) Token: 0x06001111 RID: 4369 RVA: 0x00068668 File Offset: 0x00066868
		public MachineState machineState { get; set; }

		// Token: 0x14000057 RID: 87
		// (add) Token: 0x06001112 RID: 4370 RVA: 0x00068674 File Offset: 0x00066874
		// (remove) Token: 0x06001113 RID: 4371 RVA: 0x000686AC File Offset: 0x000668AC
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x06001114 RID: 4372 RVA: 0x000686E4 File Offset: 0x000668E4
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x1700052A RID: 1322
		// (get) Token: 0x06001115 RID: 4373 RVA: 0x00068714 File Offset: 0x00066914
		// (set) Token: 0x06001116 RID: 4374 RVA: 0x0006872C File Offset: 0x0006692C
		public XRSImage CurrentImage
		{
			get
			{
				return this.currentImage;
			}
			set
			{
				this.currentImage = value;
				this.OnPropertyChanged("CurrentImage");
			}
		}

		// Token: 0x1700052B RID: 1323
		// (get) Token: 0x06001117 RID: 4375 RVA: 0x00068744 File Offset: 0x00066944
		// (set) Token: 0x06001118 RID: 4376 RVA: 0x00068768 File Offset: 0x00066968
		public bool ConveyingDirection
		{
			get
			{
				bool flag = this.conveyingDirection == Definitions.ConveyingDirection.LeftToRight;
				return !flag;
			}
			set
			{
				bool flag = this.parHandling != null;
				if (flag)
				{
					bool flag2 = !value;
					if (flag2)
					{
						this.conveyingDirection = Definitions.ConveyingDirection.LeftToRight;
					}
					else
					{
						this.conveyingDirection = Definitions.ConveyingDirection.RightToLeft;
					}
					ParameterHandling parameterHandling = this.parHandling;
					string path = CtrlAcquisitionSettingsViewModel.conveyingDirectionPath;
					int num = (int)this.conveyingDirection;
					parameterHandling.WriteValue(path, num.ToString());
					bool flag3 = this.parHandling.ReadPar(CtrlAcquisitionSettingsViewModel.conveyingDirectionPath) == "0";
					if (flag3)
					{
						this.conveyingDirection = Definitions.ConveyingDirection.LeftToRight;
					}
					else
					{
						this.conveyingDirection = Definitions.ConveyingDirection.RightToLeft;
					}
					this.OnPropertyChanged("ConveyingDirection");
					this.FeedNewProduct = true;
				}
			}
		}

		// Token: 0x1700052C RID: 1324
		// (get) Token: 0x06001119 RID: 4377 RVA: 0x00068800 File Offset: 0x00066A00
		// (set) Token: 0x0600111A RID: 4378 RVA: 0x00068818 File Offset: 0x00066A18
		public int NLeftPixel
		{
			get
			{
				return this.nLeftPixel;
			}
			set
			{
				bool flag = this.parHandling != null;
				if (flag)
				{
					this.nLeftPixel = value;
					bool flag2 = this.acquMode && value > Definition.objectModeAddLinesMax;
					if (flag2)
					{
						value = Definition.objectModeAddLinesMax;
					}
					bool flag3 = this.conveyingDirection == Definitions.ConveyingDirection.LeftToRight;
					if (flag3)
					{
						this.parHandling.WriteValue(CtrlAcquisitionSettingsViewModel.addedBackLinesPath, value.ToString());
						int.TryParse(this.parHandling.ReadPar(CtrlAcquisitionSettingsViewModel.addedBackLinesPath), out this.nLeftPixel);
					}
					else
					{
						this.parHandling.WriteValue(CtrlAcquisitionSettingsViewModel.addedFrontLinesPath, value.ToString());
						int.TryParse(this.parHandling.ReadPar(CtrlAcquisitionSettingsViewModel.addedFrontLinesPath), out this.nLeftPixel);
					}
					this.OnPropertyChanged("NLeftPixel");
					this.FeedNewProduct = true;
				}
			}
		}

		// Token: 0x1700052D RID: 1325
		// (get) Token: 0x0600111B RID: 4379 RVA: 0x000688EC File Offset: 0x00066AEC
		// (set) Token: 0x0600111C RID: 4380 RVA: 0x00068904 File Offset: 0x00066B04
		public int NRightPixel
		{
			get
			{
				return this.nRightPixel;
			}
			set
			{
				bool flag = this.parHandling != null;
				if (flag)
				{
					this.nRightPixel = value;
					bool flag2 = this.acquMode && value > Definition.objectModeAddLinesMax;
					if (flag2)
					{
						value = Definition.objectModeAddLinesMax;
					}
					bool flag3 = this.conveyingDirection == Definitions.ConveyingDirection.LeftToRight;
					if (flag3)
					{
						this.parHandling.WriteValue(CtrlAcquisitionSettingsViewModel.addedFrontLinesPath, value.ToString());
						int.TryParse(this.parHandling.ReadPar(CtrlAcquisitionSettingsViewModel.addedFrontLinesPath), out this.nRightPixel);
					}
					else
					{
						this.parHandling.WriteValue(CtrlAcquisitionSettingsViewModel.addedBackLinesPath, value.ToString());
						int.TryParse(this.parHandling.ReadPar(CtrlAcquisitionSettingsViewModel.addedBackLinesPath), out this.nRightPixel);
					}
					this.OnPropertyChanged("NRightPixel");
					this.FeedNewProduct = true;
				}
			}
		}

		// Token: 0x1700052E RID: 1326
		// (get) Token: 0x0600111D RID: 4381 RVA: 0x000689D8 File Offset: 0x00066BD8
		// (set) Token: 0x0600111E RID: 4382 RVA: 0x000689F0 File Offset: 0x00066BF0
		public int NTopPixel
		{
			get
			{
				return this.nTopPixel;
			}
			set
			{
				bool flag = this.parHandling != null;
				if (flag)
				{
					this.nTopPixel = value;
					string path = CtrlAcquisitionSettingsViewModel.addedLeftPixelPath;
					bool flag2 = this.acquMode;
					if (flag2)
					{
						path = CtrlAcquisitionSettingsViewModel.addedLeftPixelPathObjectMode;
						bool flag3 = value > Definition.objectModeAddLinesMax;
						if (flag3)
						{
							value = Definition.objectModeAddLinesMax;
						}
					}
					this.parHandling.WriteValue(path, value.ToString());
					int.TryParse(this.parHandling.ReadPar(path), out this.nTopPixel);
					this.OnPropertyChanged("NTopPixel");
					this.FeedNewProduct = true;
				}
			}
		}

		// Token: 0x1700052F RID: 1327
		// (get) Token: 0x0600111F RID: 4383 RVA: 0x00068A80 File Offset: 0x00066C80
		// (set) Token: 0x06001120 RID: 4384 RVA: 0x00068A98 File Offset: 0x00066C98
		public int NBottomPixel
		{
			get
			{
				return this.nBottomPixel;
			}
			set
			{
				bool flag = this.parHandling != null;
				if (flag)
				{
					this.nBottomPixel = value;
					string path = CtrlAcquisitionSettingsViewModel.addedRightPixelPath;
					bool flag2 = this.acquMode;
					if (flag2)
					{
						path = CtrlAcquisitionSettingsViewModel.addedRightPixelPathObjectMode;
						bool flag3 = value > Definition.objectModeAddLinesMax;
						if (flag3)
						{
							value = Definition.objectModeAddLinesMax;
						}
					}
					this.parHandling.WriteValue(path, value.ToString());
					int.TryParse(this.parHandling.ReadPar(path), out this.nBottomPixel);
					this.OnPropertyChanged("NBottomPixel");
					this.FeedNewProduct = true;
				}
			}
		}

		// Token: 0x17000530 RID: 1328
		// (get) Token: 0x06001121 RID: 4385 RVA: 0x00068B28 File Offset: 0x00066D28
		// (set) Token: 0x06001122 RID: 4386 RVA: 0x00068B40 File Offset: 0x00066D40
		public bool IsConnectImagesEnabled
		{
			get
			{
				return this.isConnectImagesEnabled;
			}
			set
			{
				bool flag = this.parHandling != null;
				if (flag)
				{
					this.isConnectImagesEnabled = value;
					this.parHandling.WriteValue(CtrlAcquisitionSettingsViewModel.connectImagesPath, Convert.ToInt32(value).ToString());
					bool flag2 = this.parHandling.ReadPar(CtrlAcquisitionSettingsViewModel.connectImagesPath) == "0";
					if (flag2)
					{
						this.isConnectImagesEnabled = false;
					}
					else
					{
						this.isConnectImagesEnabled = true;
					}
					this.OnPropertyChanged("IsConnectImagesEnabled");
					this.FeedNewProduct = true;
				}
			}
		}

		// Token: 0x17000531 RID: 1329
		// (get) Token: 0x06001123 RID: 4387 RVA: 0x00068BC8 File Offset: 0x00066DC8
		// (set) Token: 0x06001124 RID: 4388 RVA: 0x00068BE0 File Offset: 0x00066DE0
		public int NConnectedLines
		{
			get
			{
				return this.nConnectedLines;
			}
			set
			{
				bool flag = this.parHandling != null;
				if (flag)
				{
					this.nConnectedLines = value;
					this.parHandling.WriteValue(CtrlAcquisitionSettingsViewModel.nConnecteLinesPath, value.ToString());
					int.TryParse(this.parHandling.ReadPar(CtrlAcquisitionSettingsViewModel.nConnecteLinesPath), out this.nConnectedLines);
					this.OnPropertyChanged("NConnectedLines");
					this.FeedNewProduct = true;
				}
			}
		}

		// Token: 0x17000532 RID: 1330
		// (get) Token: 0x06001125 RID: 4389 RVA: 0x00068C4C File Offset: 0x00066E4C
		// (set) Token: 0x06001126 RID: 4390 RVA: 0x00068C64 File Offset: 0x00066E64
		public int MinLines
		{
			get
			{
				return this.minLines;
			}
			set
			{
				bool flag = this.parHandling != null;
				if (flag)
				{
					this.minLines = value;
					this.parHandling.WriteValue(CtrlAcquisitionSettingsViewModel.minLinesPath, value.ToString());
					int.TryParse(this.parHandling.ReadPar(CtrlAcquisitionSettingsViewModel.minLinesPath), out this.minLines);
					this.OnPropertyChanged("MinLines");
					this.FeedNewProduct = true;
				}
			}
		}

		// Token: 0x17000533 RID: 1331
		// (get) Token: 0x06001127 RID: 4391 RVA: 0x00068CD0 File Offset: 0x00066ED0
		// (set) Token: 0x06001128 RID: 4392 RVA: 0x00068CE8 File Offset: 0x00066EE8
		public int MaxLines
		{
			get
			{
				return this.maxLines;
			}
			set
			{
				bool flag = this.parHandling != null;
				if (flag)
				{
					this.maxLines = value;
					this.parHandling.WriteValue(CtrlAcquisitionSettingsViewModel.maxLinesPath, value.ToString());
					int.TryParse(this.parHandling.ReadPar(CtrlAcquisitionSettingsViewModel.maxLinesPath), out this.maxLines);
					this.OnPropertyChanged("MaxLines");
					this.FeedNewProduct = true;
				}
			}
		}

		// Token: 0x17000534 RID: 1332
		// (get) Token: 0x06001129 RID: 4393 RVA: 0x00068D54 File Offset: 0x00066F54
		// (set) Token: 0x0600112A RID: 4394 RVA: 0x00068D6C File Offset: 0x00066F6C
		public bool Binning2Enabled
		{
			get
			{
				return this.binning2Enabled;
			}
			set
			{
				bool flag = this.binning2Enabled != value;
				if (flag)
				{
					bool flag2 = Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtNewAutotrainRequired"), DialogStyle.Warning, DialogButtons.OKCancel) == Dialog.DialogResults.Ok;
					if (flag2)
					{
						this.parHandling.WriteValue(CtrlAcquisitionSettingsViewModel.binning2EnabledPath, value ? "2" : "1");
						this.binning2Enabled = value;
						this.OnPropertyChanged("Binning2Enabled");
						this.FeedNewProduct = true;
						this.parHandling.WriteValue(Definition.filterMainPath + Definition.clipFindingEnableSubPath, "0");
						DetectorType detectorType = DetectorType.Sesotec;
						this.dbConnection.GetDetectorType(ref detectorType);
						bool flag3 = false;
						this.dbConnection.GetDemoMode(ref flag3);
						MachineType machineType = MachineType.RayconXRS;
						this.dbConnection.GetMachineType(ref machineType);
						string a = this.parHandling.ReadPar(Definition.detectorSesotecPixelSizePath);
						bool flag4 = a == "0.4" && ((machineType == MachineType.RayconDPlus && detectorType == DetectorType.Sesotec) || flag3);
						if (flag4)
						{
							this.SwitchComparerAndLowDensityWithBinning(this.binning2Enabled);
						}
						bool flag5 = this.parHandling.ReadPar(Definition.testcardSimulationPath) == "1";
						if (flag5)
						{
							bool flag6 = this.binning2Enabled && (this.parHandling.ReadPar(Definition.simulationAfterTimeIntervall) == "1" || this.parHandling.ReadPar(Definition.simulationAfterAutolearn) == "1") && Dialog.Show(LocalizationManager.Localize("txtInfo"), LocalizationManager.Localize("txtDeactivateTestcardSim"), DialogStyle.Info, DialogButtons.OK) == Dialog.DialogResults.Ok;
							if (flag6)
							{
								this.parHandling.WriteValue(Definition.simulationAfterAutolearn, "0");
								this.parHandling.WriteValue(Definition.simulationAfterTimeIntervall, "0");
								this.parHandling.WriteValue(Autotrain.nAutolearnPath, "5");
								this.testcardSimService.SetProperties();
							}
						}
					}
				}
			}
		}

		// Token: 0x17000535 RID: 1333
		// (get) Token: 0x0600112B RID: 4395 RVA: 0x00068F64 File Offset: 0x00067164
		// (set) Token: 0x0600112C RID: 4396 RVA: 0x00068F7C File Offset: 0x0006717C
		public double DeformationFactor
		{
			get
			{
				return this.deformationFactor;
			}
			set
			{
				bool flag = this.parHandling != null && this.deformationFactor != value;
				if (flag)
				{
					this.parHandling.WriteValue(CtrlAcquisitionSettingsViewModel.deformationFactorPath, Convert.ToString(value));
					this.deformationFactor = value;
					this.OnPropertyChanged("DeformationFactor");
					this.FeedNewProduct = true;
					Conveyor.DeformationFactorChanged = true;
				}
			}
		}

		// Token: 0x17000536 RID: 1334
		// (get) Token: 0x0600112D RID: 4397 RVA: 0x00068FE0 File Offset: 0x000671E0
		// (set) Token: 0x0600112E RID: 4398 RVA: 0x00068FF8 File Offset: 0x000671F8
		public bool SetHEforSingleEval
		{
			get
			{
				return this.setHEforSingleEval;
			}
			set
			{
				bool flag = this.setHEforSingleEval != value;
				if (flag)
				{
					bool flag2 = Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtNewAutotrainRequired"), DialogStyle.Warning, DialogButtons.OKCancel) == Dialog.DialogResults.Ok;
					if (flag2)
					{
						this.parHandling.WriteValue(CtrlAcquisitionSettingsViewModel.setHEforSingleEvalPath, value ? "1" : "0");
						this.setHEforSingleEval = value;
						this.OnPropertyChanged("SetHEforSingleEval");
						XRSImageBox.ShowHighEnergyImage = this.setHEforSingleEval;
					}
				}
			}
		}

		// Token: 0x17000537 RID: 1335
		// (get) Token: 0x0600112F RID: 4399 RVA: 0x0006907C File Offset: 0x0006727C
		// (set) Token: 0x06001130 RID: 4400 RVA: 0x00069097 File Offset: 0x00067297
		public bool InversSetHEforSingleEval
		{
			get
			{
				return !this.setHEforSingleEval;
			}
			set
			{
			}
		}

		// Token: 0x17000538 RID: 1336
		// (get) Token: 0x06001131 RID: 4401 RVA: 0x0006909C File Offset: 0x0006729C
		// (set) Token: 0x06001132 RID: 4402 RVA: 0x000690B4 File Offset: 0x000672B4
		public bool AcquisitionMode
		{
			get
			{
				return this.acquMode;
			}
			set
			{
				bool flag = this.parHandling != null && this.acquMode != value;
				if (flag)
				{
					this.parHandling.WriteValue(CtrlAcquisitionSettingsViewModel.acquisitionModePath, value ? "1" : "0");
					this.acquMode = value;
					this.OnPropertyChanged("AcquisitionMode");
					this.FeedNewProduct = true;
					this.InvAcquisitionMode = !value;
					bool flag2 = this.parHandling.ReadPar(Definition.isLiveCalibrationActivePath) != "0" && this.parHandling.ReadPar(Definition.isLiveCalibrationActivePath) != "";
					if (flag2)
					{
						string value2 = value ? "2" : "1";
						this.parHandling.WriteValue(Definition.isLiveCalibrationActivePath, value2);
					}
					this.NTopPixel = 0;
					this.NBottomPixel = 0;
					this.NLeftPixel = 0;
					this.NRightPixel = 0;
				}
			}
		}

		// Token: 0x17000539 RID: 1337
		// (get) Token: 0x06001133 RID: 4403 RVA: 0x000691AC File Offset: 0x000673AC
		// (set) Token: 0x06001134 RID: 4404 RVA: 0x000691C4 File Offset: 0x000673C4
		public bool IsAcquisionModeVisible
		{
			get
			{
				return this.isAcquisionModeVisible;
			}
			set
			{
				this.isAcquisionModeVisible = value;
				this.OnPropertyChanged("IsAcquisionModeVisible");
			}
		}

		// Token: 0x1700053A RID: 1338
		// (get) Token: 0x06001135 RID: 4405 RVA: 0x000691DC File Offset: 0x000673DC
		// (set) Token: 0x06001136 RID: 4406 RVA: 0x00069202 File Offset: 0x00067402
		public bool InvAcquisitionMode
		{
			get
			{
				return !this.acquMode && !this.IsBulk;
			}
			set
			{
				this.OnPropertyChanged("InvAcquisitionMode");
			}
		}

		// Token: 0x1700053B RID: 1339
		// (get) Token: 0x06001137 RID: 4407 RVA: 0x00069214 File Offset: 0x00067414
		// (set) Token: 0x06001138 RID: 4408 RVA: 0x0006922C File Offset: 0x0006742C
		public bool FeedNewProduct
		{
			get
			{
				return this.feedNewProduct;
			}
			set
			{
				this.feedNewProduct = value;
				this.OnPropertyChanged("FeedNewProduct");
			}
		}

		// Token: 0x1700053C RID: 1340
		// (get) Token: 0x06001139 RID: 4409 RVA: 0x00069244 File Offset: 0x00067444
		// (set) Token: 0x0600113A RID: 4410 RVA: 0x0006925C File Offset: 0x0006745C
		public bool MachineCalibrating
		{
			get
			{
				return this.machineCalibrating;
			}
			set
			{
				bool flag = this.machineCalibrating != value;
				if (flag)
				{
					this.machineCalibrating = value;
					this.OnPropertyChanged("MachineCalibrating");
				}
			}
		}

		// Token: 0x1700053D RID: 1341
		// (get) Token: 0x0600113B RID: 4411 RVA: 0x0006928F File Offset: 0x0006748F
		// (set) Token: 0x0600113C RID: 4412 RVA: 0x00069297 File Offset: 0x00067497
		public RelayCommand ChangeNLeftPixelCommand { get; set; }

		// Token: 0x1700053E RID: 1342
		// (get) Token: 0x0600113D RID: 4413 RVA: 0x000692A0 File Offset: 0x000674A0
		// (set) Token: 0x0600113E RID: 4414 RVA: 0x000692A8 File Offset: 0x000674A8
		public RelayCommand ChangeNRightPixelCommand { get; set; }

		// Token: 0x1700053F RID: 1343
		// (get) Token: 0x0600113F RID: 4415 RVA: 0x000692B1 File Offset: 0x000674B1
		// (set) Token: 0x06001140 RID: 4416 RVA: 0x000692B9 File Offset: 0x000674B9
		public RelayCommand ChangeNTopPixelCommand { get; set; }

		// Token: 0x17000540 RID: 1344
		// (get) Token: 0x06001141 RID: 4417 RVA: 0x000692C2 File Offset: 0x000674C2
		// (set) Token: 0x06001142 RID: 4418 RVA: 0x000692CA File Offset: 0x000674CA
		public RelayCommand ChangeNBottomPixelCommand { get; set; }

		// Token: 0x17000541 RID: 1345
		// (get) Token: 0x06001143 RID: 4419 RVA: 0x000692D3 File Offset: 0x000674D3
		// (set) Token: 0x06001144 RID: 4420 RVA: 0x000692DB File Offset: 0x000674DB
		public RelayCommand ChangeConveyingDirectionCommand { get; set; }

		// Token: 0x17000542 RID: 1346
		// (get) Token: 0x06001145 RID: 4421 RVA: 0x000692E4 File Offset: 0x000674E4
		// (set) Token: 0x06001146 RID: 4422 RVA: 0x000692EC File Offset: 0x000674EC
		public RelayCommand ChangeNConnectedLinesCommand { get; set; }

		// Token: 0x17000543 RID: 1347
		// (get) Token: 0x06001147 RID: 4423 RVA: 0x000692F5 File Offset: 0x000674F5
		// (set) Token: 0x06001148 RID: 4424 RVA: 0x000692FD File Offset: 0x000674FD
		public RelayCommand ChangeMinLinesCommand { get; set; }

		// Token: 0x17000544 RID: 1348
		// (get) Token: 0x06001149 RID: 4425 RVA: 0x00069306 File Offset: 0x00067506
		// (set) Token: 0x0600114A RID: 4426 RVA: 0x0006930E File Offset: 0x0006750E
		public RelayCommand ChangeMaxLinesCommand { get; set; }

		// Token: 0x17000545 RID: 1349
		// (get) Token: 0x0600114B RID: 4427 RVA: 0x00069317 File Offset: 0x00067517
		// (set) Token: 0x0600114C RID: 4428 RVA: 0x0006931F File Offset: 0x0006751F
		public RelayCommand CleanUpCommand { get; set; }

		// Token: 0x17000546 RID: 1350
		// (get) Token: 0x0600114D RID: 4429 RVA: 0x00069328 File Offset: 0x00067528
		// (set) Token: 0x0600114E RID: 4430 RVA: 0x00069330 File Offset: 0x00067530
		public RelayCommand ChangeDeformationFactorCommand { get; set; }

		// Token: 0x17000547 RID: 1351
		// (get) Token: 0x0600114F RID: 4431 RVA: 0x00069339 File Offset: 0x00067539
		// (set) Token: 0x06001150 RID: 4432 RVA: 0x00069341 File Offset: 0x00067541
		public BoundLocalizedText StrConveyProduct { get; set; }

		// Token: 0x17000548 RID: 1352
		// (get) Token: 0x06001151 RID: 4433 RVA: 0x0006934A File Offset: 0x0006754A
		// (set) Token: 0x06001152 RID: 4434 RVA: 0x00069352 File Offset: 0x00067552
		public BoundLocalizedText StrAddedSidePixel { get; set; }

		// Token: 0x17000549 RID: 1353
		// (get) Token: 0x06001153 RID: 4435 RVA: 0x0006935B File Offset: 0x0006755B
		// (set) Token: 0x06001154 RID: 4436 RVA: 0x00069363 File Offset: 0x00067563
		public BoundLocalizedText StrConveyingDirection { get; set; }

		// Token: 0x1700054A RID: 1354
		// (get) Token: 0x06001155 RID: 4437 RVA: 0x0006936C File Offset: 0x0006756C
		// (set) Token: 0x06001156 RID: 4438 RVA: 0x00069374 File Offset: 0x00067574
		public BoundLocalizedText StrConnectImages { get; set; }

		// Token: 0x1700054B RID: 1355
		// (get) Token: 0x06001157 RID: 4439 RVA: 0x0006937D File Offset: 0x0006757D
		// (set) Token: 0x06001158 RID: 4440 RVA: 0x00069385 File Offset: 0x00067585
		public BoundLocalizedText StrDistanceConnectImages { get; set; }

		// Token: 0x1700054C RID: 1356
		// (get) Token: 0x06001159 RID: 4441 RVA: 0x0006938E File Offset: 0x0006758E
		// (set) Token: 0x0600115A RID: 4442 RVA: 0x00069396 File Offset: 0x00067596
		public BoundLocalizedText StrMinObjectLength { get; set; }

		// Token: 0x1700054D RID: 1357
		// (get) Token: 0x0600115B RID: 4443 RVA: 0x0006939F File Offset: 0x0006759F
		// (set) Token: 0x0600115C RID: 4444 RVA: 0x000693A7 File Offset: 0x000675A7
		public BoundLocalizedText StrMaxObjectLength { get; set; }

		// Token: 0x1700054E RID: 1358
		// (get) Token: 0x0600115D RID: 4445 RVA: 0x000693B0 File Offset: 0x000675B0
		// (set) Token: 0x0600115E RID: 4446 RVA: 0x000693B8 File Offset: 0x000675B8
		public BoundLocalizedText StrBinning2 { get; set; }

		// Token: 0x1700054F RID: 1359
		// (get) Token: 0x0600115F RID: 4447 RVA: 0x000693C1 File Offset: 0x000675C1
		// (set) Token: 0x06001160 RID: 4448 RVA: 0x000693C9 File Offset: 0x000675C9
		public BoundLocalizedText StrDeformationFactor { get; set; }

		// Token: 0x17000550 RID: 1360
		// (get) Token: 0x06001161 RID: 4449 RVA: 0x000693D2 File Offset: 0x000675D2
		// (set) Token: 0x06001162 RID: 4450 RVA: 0x000693DA File Offset: 0x000675DA
		public BoundLocalizedText StrImageForSingleEvaluation { get; set; }

		// Token: 0x17000551 RID: 1361
		// (get) Token: 0x06001163 RID: 4451 RVA: 0x000693E3 File Offset: 0x000675E3
		// (set) Token: 0x06001164 RID: 4452 RVA: 0x000693EB File Offset: 0x000675EB
		public BoundLocalizedText StrAcquisitionMode { get; set; }

		// Token: 0x17000552 RID: 1362
		// (get) Token: 0x06001165 RID: 4453 RVA: 0x000693F4 File Offset: 0x000675F4
		// (set) Token: 0x06001166 RID: 4454 RVA: 0x000693FC File Offset: 0x000675FC
		public bool IsServiceLoggedIn { get; set; }

		// Token: 0x17000553 RID: 1363
		// (get) Token: 0x06001167 RID: 4455 RVA: 0x00069405 File Offset: 0x00067605
		// (set) Token: 0x06001168 RID: 4456 RVA: 0x0006940D File Offset: 0x0006760D
		public bool IsDualSensorAvailable { get; set; }

		// Token: 0x17000554 RID: 1364
		// (get) Token: 0x06001169 RID: 4457 RVA: 0x00069418 File Offset: 0x00067618
		// (set) Token: 0x0600116A RID: 4458 RVA: 0x00069430 File Offset: 0x00067630
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

		// Token: 0x17000555 RID: 1365
		// (get) Token: 0x0600116B RID: 4459 RVA: 0x00069448 File Offset: 0x00067648
		// (set) Token: 0x0600116C RID: 4460 RVA: 0x00069460 File Offset: 0x00067660
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

		// Token: 0x17000556 RID: 1366
		// (get) Token: 0x0600116D RID: 4461 RVA: 0x00069476 File Offset: 0x00067676
		// (set) Token: 0x0600116E RID: 4462 RVA: 0x0006947E File Offset: 0x0006767E
		public bool DemoMode { get; set; }

		// Token: 0x17000557 RID: 1367
		// (get) Token: 0x0600116F RID: 4463 RVA: 0x00069487 File Offset: 0x00067687
		// (set) Token: 0x06001170 RID: 4464 RVA: 0x0006948F File Offset: 0x0006768F
		public RelayCommand HighSpeedImageCommand { get; set; }

		// Token: 0x17000558 RID: 1368
		// (get) Token: 0x06001171 RID: 4465 RVA: 0x00069498 File Offset: 0x00067698
		// (set) Token: 0x06001172 RID: 4466 RVA: 0x000694A0 File Offset: 0x000676A0
		public RelayCommand ChangeBackgroundNoiseCommand { get; set; }

		// Token: 0x17000559 RID: 1369
		// (get) Token: 0x06001173 RID: 4467 RVA: 0x000694A9 File Offset: 0x000676A9
		// (set) Token: 0x06001174 RID: 4468 RVA: 0x000694B1 File Offset: 0x000676B1
		public RelayCommand BulkXRIStartLiveModeCommand { get; set; }

		// Token: 0x1700055A RID: 1370
		// (get) Token: 0x06001175 RID: 4469 RVA: 0x000694BA File Offset: 0x000676BA
		// (set) Token: 0x06001176 RID: 4470 RVA: 0x000694C2 File Offset: 0x000676C2
		public RelayCommand BulkXRIStartObjectModeCommand { get; set; }

		// Token: 0x1700055B RID: 1371
		// (get) Token: 0x06001177 RID: 4471 RVA: 0x000694CB File Offset: 0x000676CB
		// (set) Token: 0x06001178 RID: 4472 RVA: 0x000694D3 File Offset: 0x000676D3
		public BoundLocalizedText StrBackgroundNoise { get; set; }

		// Token: 0x1700055C RID: 1372
		// (get) Token: 0x06001179 RID: 4473 RVA: 0x000694DC File Offset: 0x000676DC
		// (set) Token: 0x0600117A RID: 4474 RVA: 0x000694F4 File Offset: 0x000676F4
		public int BackgroundNoise
		{
			get
			{
				return this.backgroundNoise;
			}
			set
			{
				bool flag = this.parHandling != null && this.backgroundNoise != value;
				if (flag)
				{
					this.parHandling.WriteValue(CtrlAcquisitionSettingsViewModel.backgroundNoisePath, Convert.ToString(value));
					this.backgroundNoise = value;
					this.OnPropertyChanged("BackgroundNoise");
				}
			}
		}

		// Token: 0x1700055D RID: 1373
		// (get) Token: 0x0600117B RID: 4475 RVA: 0x0006954C File Offset: 0x0006774C
		// (set) Token: 0x0600117C RID: 4476 RVA: 0x00069564 File Offset: 0x00067764
		public bool IsBulk
		{
			get
			{
				return this.isBulk;
			}
			set
			{
				this.isBulk = value;
				this.IsNotBulk = !this.isBulk;
				this.OnPropertyChanged("IsBulk");
			}
		}

		// Token: 0x1700055E RID: 1374
		// (get) Token: 0x0600117D RID: 4477 RVA: 0x0006958C File Offset: 0x0006778C
		// (set) Token: 0x0600117E RID: 4478 RVA: 0x000695A4 File Offset: 0x000677A4
		public bool IsNotBulk
		{
			get
			{
				return this.isNotBulk;
			}
			set
			{
				this.isNotBulk = value;
				this.OnPropertyChanged("IsNotBulk");
			}
		}

		// Token: 0x1700055F RID: 1375
		// (get) Token: 0x0600117F RID: 4479 RVA: 0x000695BC File Offset: 0x000677BC
		// (set) Token: 0x06001180 RID: 4480 RVA: 0x000695D4 File Offset: 0x000677D4
		public string BulkLiveModeSourcePath
		{
			get
			{
				return this.bulkLiveModeSourcePath;
			}
			set
			{
				bool flag = this.bulkLiveModeSourcePath != value;
				if (flag)
				{
					this.bulkLiveModeSourcePath = value;
					this.OnPropertyChanged("BulkLiveModeSourcePath");
				}
			}
		}

		// Token: 0x17000560 RID: 1376
		// (get) Token: 0x06001181 RID: 4481 RVA: 0x00069608 File Offset: 0x00067808
		// (set) Token: 0x06001182 RID: 4482 RVA: 0x00069620 File Offset: 0x00067820
		public bool ShowNextBulk
		{
			get
			{
				return this.showNextBulk;
			}
			set
			{
				this.showNextBulk = value;
				this.OnPropertyChanged("ShowNextBulk");
			}
		}

		// Token: 0x17000561 RID: 1377
		// (get) Token: 0x06001183 RID: 4483 RVA: 0x00069638 File Offset: 0x00067838
		// (set) Token: 0x06001184 RID: 4484 RVA: 0x00069650 File Offset: 0x00067850
		public bool ShowNextBulkCont
		{
			get
			{
				return this.showNextBulkObject;
			}
			set
			{
				this.showNextBulkObject = value;
				this.OnPropertyChanged("ShowNextBulkObject");
			}
		}

		// Token: 0x17000562 RID: 1378
		// (get) Token: 0x06001185 RID: 4485 RVA: 0x00069668 File Offset: 0x00067868
		// (set) Token: 0x06001186 RID: 4486 RVA: 0x00069680 File Offset: 0x00067880
		public bool BulkContImagesShown
		{
			get
			{
				return this.bulkContImagesShown;
			}
			set
			{
				this.bulkContImagesShown = value;
				this.OnPropertyChanged("BulkContImagesShown");
			}
		}

		// Token: 0x17000563 RID: 1379
		// (get) Token: 0x06001187 RID: 4487 RVA: 0x00069698 File Offset: 0x00067898
		// (set) Token: 0x06001188 RID: 4488 RVA: 0x000696B0 File Offset: 0x000678B0
		public string BulkObjectModeSourcePath
		{
			get
			{
				return this.bulkObjectModeSourcePath;
			}
			set
			{
				bool flag = this.bulkObjectModeSourcePath != value;
				if (flag)
				{
					this.bulkObjectModeSourcePath = value;
					this.OnPropertyChanged("BulkObjectModeSourcePath");
				}
			}
		}

		// Token: 0x17000564 RID: 1380
		// (get) Token: 0x06001189 RID: 4489 RVA: 0x000696E4 File Offset: 0x000678E4
		// (set) Token: 0x0600118A RID: 4490 RVA: 0x000696FC File Offset: 0x000678FC
		public bool IsNotTX
		{
			get
			{
				return this.isNotTX;
			}
			set
			{
				this.isNotTX = value;
				this.OnPropertyChanged("IsNotTX");
			}
		}

		// Token: 0x17000565 RID: 1381
		// (get) Token: 0x0600118B RID: 4491 RVA: 0x00069714 File Offset: 0x00067914
		// (set) Token: 0x0600118C RID: 4492 RVA: 0x0006972C File Offset: 0x0006792C
		public bool IsDeformationVisible
		{
			get
			{
				return this.isDeformationVisible;
			}
			set
			{
				this.isDeformationVisible = value;
				this.OnPropertyChanged("IsDeformationVisible");
			}
		}

		// Token: 0x17000566 RID: 1382
		// (get) Token: 0x0600118D RID: 4493 RVA: 0x00069744 File Offset: 0x00067944
		// (set) Token: 0x0600118E RID: 4494 RVA: 0x0006975C File Offset: 0x0006795C
		public bool NotObjectBasedFlaps
		{
			get
			{
				return this.notObjectBasedFlaps;
			}
			set
			{
				this.notObjectBasedFlaps = value;
				this.OnPropertyChanged("NotObjectBasedFlaps");
			}
		}

		// Token: 0x0600118F RID: 4495 RVA: 0x00069774 File Offset: 0x00067974
		public CtrlAcquisitionSettingsViewModel(ParameterHandling parHandling, ImageDataTransfer imageTransfer, DBConnection dbConnection, ProductManager productManager, MachineState machineState, StoreImageData imageArchiver, XRSImageBox imagebox, AccessRightsManager accessmanager, TestcardSimService testcardSimService)
		{
			this.parHandling = parHandling;
			this.imageTransfer = imageTransfer;
			this.dbConnection = dbConnection;
			this.productManager = productManager;
			this.machineState = machineState;
			this.imageArchiver = imageArchiver;
			this.imagebox = imagebox;
			this.accessmanager = accessmanager;
			this.testcardSimService = testcardSimService;
			bool demoMode = false;
			dbConnection.GetDemoMode(ref demoMode);
			this.DemoMode = demoMode;
			this.ChangeNLeftPixelCommand = new RelayCommand(new Action<object>(this.ChangeNLeftPixel));
			this.ChangeNRightPixelCommand = new RelayCommand(new Action<object>(this.ChangeNRightPixel));
			this.ChangeNTopPixelCommand = new RelayCommand(new Action<object>(this.ChangeNTopPixel));
			this.ChangeNBottomPixelCommand = new RelayCommand(new Action<object>(this.ChangeNBottomPixel));
			this.ChangeConveyingDirectionCommand = new RelayCommand(new Action<object>(this.ChangeConveyingDirection));
			this.ChangeNConnectedLinesCommand = new RelayCommand(new Action<object>(this.ChangeNConnectedLines));
			this.ChangeMinLinesCommand = new RelayCommand(new Action<object>(this.ChangeMinLines));
			this.ChangeMaxLinesCommand = new RelayCommand(new Action<object>(this.ChangeMaxLines));
			this.ChangeDeformationFactorCommand = new RelayCommand(new Action<object>(this.ChangeDeformationFactor));
			this.HighSpeedImageCommand = new RelayCommand(new Action<object>(this.TakeHighSpeedImage));
			this.ChangeBackgroundNoiseCommand = new RelayCommand(new Action<object>(this.ChangeBackgroundNoise));
			this.BulkXRIStartLiveModeCommand = new RelayCommand(new Action<object>(this.StartBulkLiveXRIImage));
			this.BulkXRIStartObjectModeCommand = new RelayCommand(new Action<object>(this.StartBulkObjectXRIImage));
			this.InitializeValues();
			MachineMode machineMode = MachineMode.Standard;
			dbConnection.GetMachineMode(ref machineMode);
			bool flag = machineMode == MachineMode.Bulk;
			if (flag)
			{
				this.IsBulk = true;
				string a = "";
				dbConnection.GetParameterValue(Definition.bulkShowContImages, ref a);
				bool flag2 = a == "1";
				if (flag2)
				{
					this.BulkContImagesShown = true;
				}
				else
				{
					this.BulkContImagesShown = false;
				}
			}
			else
			{
				this.IsBulk = false;
				this.BulkContImagesShown = false;
			}
			this.timer = new System.Threading.Timer(new TimerCallback(this.LoadLastImage));
			this.timer.Change(100, -1);
			imageTransfer.FrameArrived += this.imageTransfer_FrameArrived;
			this.CleanUpCommand = new RelayCommand(new Action<object>(this.CleanUp));
			this.feedNewProduct = false;
			this.IsServiceLoggedIn = (accessmanager.CurrentUser.Rights == BuiltInPriviledges.Service);
			this.IsAcquisionModeVisible = false;
			bool flag3 = productManager.CurrentLoadedProduct.IsSingleProduct && this.IsServiceLoggedIn && !this.IsBulk;
			if (flag3)
			{
				this.IsAcquisionModeVisible = true;
			}
			DetectorType detectorType = DetectorType.Sesotec;
			dbConnection.GetDetectorType(ref detectorType);
			this.IsDualSensorAvailable = false;
			bool flag4 = this.IsServiceLoggedIn && detectorType == DetectorType.DTNetDE && !this.IsBulk;
			if (flag4)
			{
				this.IsDualSensorAvailable = true;
			}
			this.RegisterLocalizedText();
			bool flag5 = false;
			dbConnection.GetHighThroughputMachine(ref flag5);
			string text = "";
			dbConnection.GetParameterValue(Definition.highThroughputEnabled, ref text);
			bool flag6 = flag5 && text.Equals("1");
			if (flag6)
			{
				this.HighSpeedMachineActive = true;
			}
			else
			{
				this.HighSpeedMachineActive = false;
			}
			this.ShowNext = false;
			this.ShowNextBulk = false;
			this.BulkLiveModeSourcePath = "..\\img\\xriimageBulk.png";
			bool flag7 = this.BulkContImagesShown;
			if (flag7)
			{
				this.BulkObjectModeSourcePath = "..\\img\\xriimageBulkCont.png";
			}
			else
			{
				this.BulkObjectModeSourcePath = "..\\img\\xriimageBulkJustCont.png";
			}
			bool flag8 = detectorType == DetectorType.DTNetTDI;
			if (flag8)
			{
				this.IsNotTX = false;
				this.IsDeformationVisible = false;
			}
			else
			{
				this.IsNotTX = true;
				this.IsDeformationVisible = this.IsServiceLoggedIn;
			}
			bool flag9 = false;
			dbConnection.GetObjectBasedFlapsEnabled(ref flag9);
			this.NotObjectBasedFlaps = !flag9;
		}

		// Token: 0x06001190 RID: 4496 RVA: 0x00069BA0 File Offset: 0x00067DA0
		private void InitializeValues()
		{
			try
			{
				string a = "";
				string s = "";
				int num;
				int.TryParse(this.parHandling.ReadPar(CtrlAcquisitionSettingsViewModel.conveyingDirectionPath), out num);
				this.conveyingDirection = (ConveyingDirection)num;
				this.OnPropertyChanged("ConveyingDirection");
				this.dbConnection.GetParameterValue(Acquisition.acquisitionModePath, ref a);
				this.acquMode = (a == "1");
				this.UpdateAddedPixelOnAllSide();
				int value;
				int.TryParse(this.parHandling.ReadPar(CtrlAcquisitionSettingsViewModel.connectImagesPath), out value);
				this.IsConnectImagesEnabled = Convert.ToBoolean(value);
				int.TryParse(this.parHandling.ReadPar(CtrlAcquisitionSettingsViewModel.nConnecteLinesPath), out this.nConnectedLines);
				this.OnPropertyChanged("NConnectedLines");
				int.TryParse(this.parHandling.ReadPar(CtrlAcquisitionSettingsViewModel.minLinesPath), out this.minLines);
				this.OnPropertyChanged("MinLines");
				this.dbConnection.GetParameterValue(CtrlAcquisitionSettingsViewModel.maxLinesPath, ref s);
				int.TryParse(s, out this.maxLines);
				this.OnPropertyChanged("MaxLines");
				this.deformationFactor = Convert.ToDouble(this.parHandling.ReadPar(CtrlAcquisitionSettingsViewModel.deformationFactorPath));
				string a2 = this.parHandling.ReadPar(CtrlAcquisitionSettingsViewModel.binning2EnabledPath);
				bool flag = a2 == "1";
				if (flag)
				{
					this.binning2Enabled = false;
				}
				else
				{
					this.binning2Enabled = true;
				}
				string a3 = this.parHandling.ReadPar(CtrlAcquisitionSettingsViewModel.setHEforSingleEvalPath);
				this.setHEforSingleEval = (a3 == "1");
				this.timerIsMachineCalibrating = new System.Threading.Timer(new TimerCallback(this.CheckMachineState), null, 0, 500);
				string s2 = "";
				this.dbConnection.GetParameterValue(CtrlAcquisitionSettingsViewModel.backgroundNoisePath, ref s2);
				int.TryParse(s2, out this.backgroundNoise);
				this.OnPropertyChanged("BackgroundNoise");
			}
			catch
			{
			}
		}

		// Token: 0x06001191 RID: 4497 RVA: 0x00069DB4 File Offset: 0x00067FB4
		private void CheckMachineState(object obj)
		{
			this.MachineCalibrating = (this.machineState.State == MachineStates.XrayOnCalibrationNotDone || this.machineState.State == MachineStates.Reseasoning);
		}

		// Token: 0x06001192 RID: 4498 RVA: 0x00069DE0 File Offset: 0x00067FE0
		private void UpdateAddedPixelOnAllSide()
		{
			bool flag = this.conveyingDirection == Definitions.ConveyingDirection.LeftToRight;
			if (flag)
			{
				int.TryParse(this.parHandling.ReadPar(CtrlAcquisitionSettingsViewModel.addedBackLinesPath), out this.nLeftPixel);
				int.TryParse(this.parHandling.ReadPar(CtrlAcquisitionSettingsViewModel.addedFrontLinesPath), out this.nRightPixel);
			}
			else
			{
				int.TryParse(this.parHandling.ReadPar(CtrlAcquisitionSettingsViewModel.addedBackLinesPath), out this.nRightPixel);
				int.TryParse(this.parHandling.ReadPar(CtrlAcquisitionSettingsViewModel.addedFrontLinesPath), out this.nLeftPixel);
			}
			bool flag2 = this.acquMode;
			if (flag2)
			{
				int.TryParse(this.parHandling.ReadPar(CtrlAcquisitionSettingsViewModel.addedLeftPixelPathObjectMode), out this.nTopPixel);
				int.TryParse(this.parHandling.ReadPar(CtrlAcquisitionSettingsViewModel.addedRightPixelPathObjectMode), out this.nBottomPixel);
			}
			else
			{
				int.TryParse(this.parHandling.ReadPar(CtrlAcquisitionSettingsViewModel.addedLeftPixelPath), out this.nTopPixel);
				int.TryParse(this.parHandling.ReadPar(CtrlAcquisitionSettingsViewModel.addedRightPixelPath), out this.nBottomPixel);
			}
			this.OnPropertyChanged("NLeftPixel");
			this.OnPropertyChanged("NRightPixel");
			this.OnPropertyChanged("NTopPixel");
			this.OnPropertyChanged("NBottomPixel");
		}

		// Token: 0x06001193 RID: 4499 RVA: 0x00069F24 File Offset: 0x00068124
		private void LoadLastImage(object obj)
		{
			bool flag = this.timer != null;
			if (flag)
			{
				this.timer.Dispose();
			}
			this.currentImage = HistoryHelper.GetLatestImage(this.imageArchiver);
			Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(delegate()
			{
				bool flag2 = this.currentImage != null;
				if (flag2)
				{
					XRSImage image = this.currentImage.DeepCopy();
					this.imagebox.ViewXRSImage(image);
				}
				else
				{
					Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtNoValidImage"), DialogStyle.Error);
				}
			}));
		}

		// Token: 0x06001194 RID: 4500 RVA: 0x00069F7C File Offset: 0x0006817C
		private void imageTransfer_FrameArrived(object sender, ReceiveEventArgs e)
		{
			bool flag = (!this.HighSpeedMachineActive && this.IsNotBulk) || (this.HighSpeedMachineActive && this.ShowNext) || (this.IsBulk && this.ShowNextBulk) || (this.ShowNextBulkCont && this.IsBulk && e.Image.HasErrors()) || (this.ShowNextBulkCont && this.IsBulk && this.BulkContImagesShown);
			if (flag)
			{
				bool flag2 = this.machineState.State != MachineStates.Reseasoning && this.machineState.State != MachineStates.XrayOnCalibrationNotDone && e.Image.Type != ImageType.TestcardSimulation;
				if (flag2)
				{
					Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
					{
						bool flag5 = this.currentImage != null;
						if (flag5)
						{
							XRSImage image = e.Image.DeepCopy();
							this.imagebox.ViewXRSImage(image);
							this.FeedNewProduct = false;
						}
						else
						{
							Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtNoValidImage"), DialogStyle.Error);
						}
					}));
				}
				this.ShowNext = false;
				this.ShowNextBulk = false;
				this.ShowNextBulkCont = false;
				bool flag3 = this.IsBulk;
				if (flag3)
				{
					this.deactivateBulkLiveMode();
					this.BulkLiveModeSourcePath = "..\\img\\xriimageBulk.png";
					bool flag4 = this.BulkContImagesShown;
					if (flag4)
					{
						this.BulkObjectModeSourcePath = "..\\img\\xriimageBulkCont.png";
					}
					else
					{
						this.BulkObjectModeSourcePath = "..\\img\\xriimageBulkJustCont.png";
					}
				}
			}
		}

		// Token: 0x06001195 RID: 4501 RVA: 0x0006A0CC File Offset: 0x000682CC
		public void ChangeNLeftPixel(object obj)
		{
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtNumEdgePixel"), this.NLeftPixel.ToString(), NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_InputNLeftPixel;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x06001196 RID: 4502 RVA: 0x0006A124 File Offset: 0x00068324
		public void ChangeNRightPixel(object obj)
		{
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtNumEdgePixel"), this.NRightPixel.ToString(), NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_InputNRightPixel;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x06001197 RID: 4503 RVA: 0x0006A17C File Offset: 0x0006837C
		public void ChangeNTopPixel(object obj)
		{
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtNumEdgePixel"), this.NTopPixel.ToString(), NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_InputNTopPixel;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x06001198 RID: 4504 RVA: 0x0006A1D4 File Offset: 0x000683D4
		public void ChangeNBottomPixel(object obj)
		{
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtNumEdgePixel"), this.NBottomPixel.ToString(), NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_InputNBottomPixel;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x06001199 RID: 4505 RVA: 0x0006A22A File Offset: 0x0006842A
		public void ChangeConveyingDirection(object obj)
		{
			this.UpdateAddedPixelOnAllSide();
		}

		// Token: 0x0600119A RID: 4506 RVA: 0x0006A234 File Offset: 0x00068434
		public void ChangeNConnectedLines(object obj)
		{
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtDistanceConnectImages"), this.NConnectedLines.ToString(), NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_InputNConnectedLines;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x0600119B RID: 4507 RVA: 0x0006A28C File Offset: 0x0006848C
		public void ChangeMinLines(object obj)
		{
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtMinObjectLength"), this.MinLines.ToString(), NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_InputMinLines;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x0600119C RID: 4508 RVA: 0x0006A2E4 File Offset: 0x000684E4
		public void ChangeMaxLines(object obj)
		{
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtMaxObjectLength"), this.MaxLines.ToString(), NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_InputMaxLines;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x0600119D RID: 4509 RVA: 0x0006A33C File Offset: 0x0006853C
		public void ChangeDeformationFactor(object obj)
		{
			string oldValue = this.parHandling.ReadPar(CtrlAcquisitionSettingsViewModel.deformationFactorPath);
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtDeformationFactor"), oldValue, NumKeyboardType.TypeDouble);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_InputDeformationFactor;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x0600119E RID: 4510 RVA: 0x0006A39C File Offset: 0x0006859C
		private void numKeyboard_InputNLeftPixel(object sender, KeyboardInputEventArgs e)
		{
			try
			{
				this.NLeftPixel = int.Parse(e.Text);
			}
			catch
			{
			}
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_InputNLeftPixel;
		}

		// Token: 0x0600119F RID: 4511 RVA: 0x0006A3F0 File Offset: 0x000685F0
		private void numKeyboard_InputNRightPixel(object sender, KeyboardInputEventArgs e)
		{
			try
			{
				this.NRightPixel = int.Parse(e.Text);
			}
			catch
			{
			}
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_InputNRightPixel;
		}

		// Token: 0x060011A0 RID: 4512 RVA: 0x0006A444 File Offset: 0x00068644
		private void numKeyboard_InputNTopPixel(object sender, KeyboardInputEventArgs e)
		{
			try
			{
				this.NTopPixel = int.Parse(e.Text);
			}
			catch
			{
			}
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_InputNTopPixel;
		}

		// Token: 0x060011A1 RID: 4513 RVA: 0x0006A498 File Offset: 0x00068698
		private void numKeyboard_InputNBottomPixel(object sender, KeyboardInputEventArgs e)
		{
			try
			{
				this.NBottomPixel = int.Parse(e.Text);
			}
			catch
			{
			}
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_InputNBottomPixel;
		}

		// Token: 0x060011A2 RID: 4514 RVA: 0x0006A4EC File Offset: 0x000686EC
		private void numKeyboard_InputNConnectedLines(object sender, KeyboardInputEventArgs e)
		{
			try
			{
				this.NConnectedLines = int.Parse(e.Text);
			}
			catch
			{
			}
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_InputNConnectedLines;
		}

		// Token: 0x060011A3 RID: 4515 RVA: 0x0006A540 File Offset: 0x00068740
		private void numKeyboard_InputMinLines(object sender, KeyboardInputEventArgs e)
		{
			try
			{
				this.MinLines = int.Parse(e.Text);
				bool flag = this.MinLines > this.MaxLines;
				if (flag)
				{
					this.MinLines = this.MaxLines;
				}
			}
			catch
			{
			}
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_InputMinLines;
		}

		// Token: 0x060011A4 RID: 4516 RVA: 0x0006A5B0 File Offset: 0x000687B0
		private void numKeyboard_InputMaxLines(object sender, KeyboardInputEventArgs e)
		{
			try
			{
				this.MaxLines = int.Parse(e.Text);
				bool flag = this.MaxLines < this.MinLines;
				if (flag)
				{
					this.MaxLines = this.MinLines;
				}
			}
			catch
			{
			}
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_InputMaxLines;
		}

		// Token: 0x060011A5 RID: 4517 RVA: 0x0006A620 File Offset: 0x00068820
		private void numKeyboard_InputDeformationFactor(object sender, KeyboardInputEventArgs e)
		{
			try
			{
				double num = double.Parse(e.Text, new CultureInfo("de-DE"));
				this.DeformationFactor = num;
				this.numKeyboard.TextInputKeyboard -= this.numKeyboard_InputDeformationFactor;
			}
			catch
			{
			}
		}

		// Token: 0x060011A6 RID: 4518 RVA: 0x0006A680 File Offset: 0x00068880
		public void EnergyModeChanged(object sender, SelectionChangedEventArgs e)
		{
			bool flag = e.AddedItems.Count > 0 && e.AddedItems[0] is EnergyLevelMode;
			if (flag)
			{
				this.SetHEforSingleEval = true;
			}
		}

		// Token: 0x060011A7 RID: 4519 RVA: 0x0006A6C4 File Offset: 0x000688C4
		private void RegisterLocalizedText()
		{
			this.StrConveyProduct = new BoundLocalizedText("txtConveyProduct");
			this.StrAddedSidePixel = new BoundLocalizedText("txtAddedSidePixel");
			this.StrConveyingDirection = new BoundLocalizedText("txtConveyingDirection");
			this.StrConnectImages = new BoundLocalizedText("txtConnectImages");
			this.StrDistanceConnectImages = new BoundLocalizedText("txtDistanceConnectImages");
			this.StrMinObjectLength = new BoundLocalizedText("txtMinObjectLength");
			this.StrMaxObjectLength = new BoundLocalizedText("txtMaxObjectLength");
			this.StrBinning2 = new BoundLocalizedText("txtBinning2");
			this.StrDeformationFactor = new BoundLocalizedText("txtDeformationFactor");
			this.StrImageForSingleEvaluation = new BoundLocalizedText("txtImageForSingleEval");
			this.StrAcquisitionMode = new BoundLocalizedText("txtAcquisitionMode");
			this.StrBackgroundNoise = new BoundLocalizedText("txtBackgroundNoise");
		}

		// Token: 0x060011A8 RID: 4520 RVA: 0x0006A7A0 File Offset: 0x000689A0
		public void DeregisterLocalizedText()
		{
			MainWindow.localizationManager.Deregister(this.StrConveyProduct);
			MainWindow.localizationManager.Deregister(this.StrAddedSidePixel);
			MainWindow.localizationManager.Deregister(this.StrConveyingDirection);
			MainWindow.localizationManager.Deregister(this.StrConnectImages);
			MainWindow.localizationManager.Deregister(this.StrDistanceConnectImages);
			MainWindow.localizationManager.Deregister(this.StrMinObjectLength);
			MainWindow.localizationManager.Deregister(this.StrMaxObjectLength);
			MainWindow.localizationManager.Deregister(this.StrBinning2);
			MainWindow.localizationManager.Deregister(this.StrDeformationFactor);
			MainWindow.localizationManager.Deregister(this.StrImageForSingleEvaluation);
			MainWindow.localizationManager.Deregister(this.StrAcquisitionMode);
			MainWindow.localizationManager.Deregister(this.StrBackgroundNoise);
		}

		// Token: 0x060011A9 RID: 4521 RVA: 0x0006A87C File Offset: 0x00068A7C
		private void SwitchComparerAndLowDensityWithBinning(bool binactive)
		{
			bool isSingleProduct = this.productManager.CurrentLoadedProduct.IsSingleProduct;
			if (isSingleProduct)
			{
				Filter.EnableComparerFilter(this.parHandling, !binactive, 0);
				Filter.SwitchLowDensityParameter(this.parHandling, !binactive, 0);
			}
			else
			{
				for (int i = 0; i < this.productManager.CurrentLoadedProduct.SubProducts.Count; i++)
				{
					Filter.EnableComparerFilter(this.parHandling, !binactive, (int)this.productManager.CurrentLoadedProduct.SubProducts[i].Group);
					Filter.SwitchLowDensityParameter(this.parHandling, !binactive, (int)this.productManager.CurrentLoadedProduct.SubProducts[i].Group);
				}
			}
		}

		// Token: 0x060011AA RID: 4522 RVA: 0x0006A944 File Offset: 0x00068B44
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

		// Token: 0x060011AB RID: 4523 RVA: 0x0006A974 File Offset: 0x00068B74
		private void CleanUp(object obj)
		{
			this.imageTransfer.FrameArrived -= this.imageTransfer_FrameArrived;
			this.DeregisterLocalizedText();
			this.deactivateBulkLiveMode();
			bool flag = this.timerIsMachineCalibrating != null;
			if (flag)
			{
				this.timerIsMachineCalibrating.Dispose();
			}
		}

		// Token: 0x060011AC RID: 4524 RVA: 0x0006A9C3 File Offset: 0x00068BC3
		private void highSpeedTimer_Elapsed(object sender, ElapsedEventArgs e)
		{
			this.ShowNext = false;
		}

		// Token: 0x060011AD RID: 4525 RVA: 0x0006A9D0 File Offset: 0x00068BD0
		public void ChangeBackgroundNoise(object obj)
		{
			string oldValue = this.parHandling.ReadPar(CtrlAcquisitionSettingsViewModel.backgroundNoisePath);
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtBackgroundNoise"), oldValue, NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_InputBackgroundNoise;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x060011AE RID: 4526 RVA: 0x0006AA30 File Offset: 0x00068C30
		private void numKeyboard_InputBackgroundNoise(object sender, KeyboardInputEventArgs e)
		{
			try
			{
				this.BackgroundNoise = int.Parse(e.Text);
				bool flag = this.BackgroundNoise > this.maxBackgroundNoise;
				if (flag)
				{
					this.BackgroundNoise = this.maxBackgroundNoise;
				}
				bool flag2 = this.BackgroundNoise < this.minBackgroundNoise;
				if (flag2)
				{
					this.BackgroundNoise = this.minBackgroundNoise;
				}
			}
			catch
			{
			}
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_InputBackgroundNoise;
		}

		// Token: 0x060011AF RID: 4527 RVA: 0x0006AAC0 File Offset: 0x00068CC0
		private void StartBulkLiveXRIImage(object obj)
		{
			this.ShowNextBulk = true;
			this.ShowNextBulkCont = false;
			this.parHandling.WriteValue("/filter/acquisition/ContinuousModeObjImg/Value", "0");
			this.parHandling.WriteValue("/filter/acquisition/SendImage/Value", "1");
			this.parHandling.WriteValue("/filter/acquisition/SendOnlyIfContaminant/Value", "0");
			this.BulkLiveModeSourcePath = "..\\img\\xriimageBulk_pause.png";
		}

		// Token: 0x060011B0 RID: 4528 RVA: 0x0006AB2C File Offset: 0x00068D2C
		private void StartBulkObjectXRIImage(object obj)
		{
			this.ShowNextBulk = false;
			this.ShowNextBulkCont = true;
			bool flag = this.BulkContImagesShown;
			if (flag)
			{
				this.parHandling.WriteValue("/filter/acquisition/ContinuousModeObjImg/Value", "1");
				this.BulkObjectModeSourcePath = "..\\img\\xriimageBulkCont_pause.png";
			}
			else
			{
				this.BulkObjectModeSourcePath = "..\\img\\xriimageBulkJustCont_pause.png";
				this.parHandling.WriteValue("/filter/acquisition/ContinuousModeObjImg/Value", "0");
				this.parHandling.WriteValue("/filter/acquisition/SendImage/Value", "0");
				this.parHandling.WriteValue("/filter/acquisition/SendOnlyIfContaminant/Value", "1");
			}
		}

		// Token: 0x060011B1 RID: 4529 RVA: 0x0006ABCC File Offset: 0x00068DCC
		private void deactivateBulkLiveMode()
		{
			bool flag = this.parHandling != null && this.IsBulk && this.machineState.State == MachineStates.Inspecting;
			if (flag)
			{
				this.parHandling.WriteValue("/filter/acquisition/SendImage/Value", "0", false);
				this.parHandling.WriteValue("/filter/acquisition/ContinuousMode/Value", "1");
				bool flag2 = this.parHandling.ReadPar(Definition.bulkShowContImages) == "1";
				if (flag2)
				{
					this.parHandling.WriteValue("/filter/acquisition/SendOnlyIfContaminant/Value", "1");
				}
				else
				{
					this.parHandling.WriteValue("/filter/acquisition/SendOnlyIfContaminant/Value", "0");
				}
			}
		}

		// Token: 0x04000B58 RID: 2904
		private XRSImage currentImage;

		// Token: 0x04000B59 RID: 2905
		private ParameterHandling parHandling;

		// Token: 0x04000B5A RID: 2906
		private ImageDataTransfer imageTransfer;

		// Token: 0x04000B5B RID: 2907
		private DBConnection dbConnection;

		// Token: 0x04000B5C RID: 2908
		private ProductManager productManager;

		// Token: 0x04000B5E RID: 2910
		private StoreImageData imageArchiver;

		// Token: 0x04000B5F RID: 2911
		private System.Threading.Timer timer;

		// Token: 0x04000B60 RID: 2912
		private NumKeyboard numKeyboard;

		// Token: 0x04000B61 RID: 2913
		private XRSImageBox imagebox;

		// Token: 0x04000B62 RID: 2914
		private AccessRightsManager accessmanager;

		// Token: 0x04000B63 RID: 2915
		private CultureInfo culture = new CultureInfo("en-US");

		// Token: 0x04000B64 RID: 2916
		private System.Threading.Timer timerIsMachineCalibrating;

		// Token: 0x04000B65 RID: 2917
		public static readonly string acquisitionPath = "/acquisition";

		// Token: 0x04000B66 RID: 2918
		public static readonly string croppingPath = "/cropping";

		// Token: 0x04000B67 RID: 2919
		public static readonly string addedBackLinesPath = Definition.filterMainPath + CtrlAcquisitionSettingsViewModel.acquisitionPath + "/AddedBackLines/Value";

		// Token: 0x04000B68 RID: 2920
		public static readonly string addedFrontLinesPath = Definition.filterMainPath + CtrlAcquisitionSettingsViewModel.acquisitionPath + "/AddedFrontLines/Value";

		// Token: 0x04000B69 RID: 2921
		public static readonly string addedLeftPixelPath = Definition.filterMainPath + CtrlAcquisitionSettingsViewModel.croppingPath + "/AddedLeftPixel/Value";

		// Token: 0x04000B6A RID: 2922
		public static readonly string addedRightPixelPath = Definition.filterMainPath + CtrlAcquisitionSettingsViewModel.croppingPath + "/AddedRightPixel/Value";

		// Token: 0x04000B6B RID: 2923
		public static readonly string addedLeftPixelPathObjectMode = Definition.filterMainPath + CtrlAcquisitionSettingsViewModel.acquisitionPath + "/AddedLeftPixObjectImg/Value";

		// Token: 0x04000B6C RID: 2924
		public static readonly string addedRightPixelPathObjectMode = Definition.filterMainPath + CtrlAcquisitionSettingsViewModel.acquisitionPath + "/AddedRightPixObjectImg/Value";

		// Token: 0x04000B6D RID: 2925
		public static readonly string conveyingDirectionPath = "/application/ConveyingDirection/Value";

		// Token: 0x04000B6E RID: 2926
		public static readonly string connectImagesPath = Definition.filterMainPath + CtrlAcquisitionSettingsViewModel.acquisitionPath + "/ConnectImages/Value";

		// Token: 0x04000B6F RID: 2927
		public static readonly string nConnecteLinesPath = Definition.filterMainPath + CtrlAcquisitionSettingsViewModel.acquisitionPath + "/NConnectedLines/Value";

		// Token: 0x04000B70 RID: 2928
		public static readonly string minLinesPath = Definition.filterMainPath + CtrlAcquisitionSettingsViewModel.acquisitionPath + "/MinLines/Value";

		// Token: 0x04000B71 RID: 2929
		public static readonly string maxLinesPath = Definition.filterMainPath + CtrlAcquisitionSettingsViewModel.acquisitionPath + "/MaxLines/Value";

		// Token: 0x04000B72 RID: 2930
		public static readonly string acquisitionModePath = Definition.filterMainPath + CtrlAcquisitionSettingsViewModel.acquisitionPath + "/AcquisitionMode/Value";

		// Token: 0x04000B73 RID: 2931
		public static readonly string binning2EnabledPath = Definition.filterMainPath + CtrlAcquisitionSettingsViewModel.acquisitionPath + "/Binning/Value";

		// Token: 0x04000B74 RID: 2932
		public static readonly string deformationFactorPath = "/machine/DeformationFactor/Value";

		// Token: 0x04000B75 RID: 2933
		public static readonly string setHEforSingleEvalPath = Definition.filterMainPath + DualEnergy.pathSetHighforSingleEval;

		// Token: 0x04000B76 RID: 2934
		private int nLeftPixel;

		// Token: 0x04000B77 RID: 2935
		private int nRightPixel;

		// Token: 0x04000B78 RID: 2936
		private int nTopPixel;

		// Token: 0x04000B79 RID: 2937
		private int nBottomPixel;

		// Token: 0x04000B7A RID: 2938
		private bool isConnectImagesEnabled;

		// Token: 0x04000B7B RID: 2939
		private int nConnectedLines;

		// Token: 0x04000B7C RID: 2940
		private int minLines;

		// Token: 0x04000B7D RID: 2941
		private int maxLines;

		// Token: 0x04000B7E RID: 2942
		private bool acquMode;

		// Token: 0x04000B7F RID: 2943
		private bool isAcquisionModeVisible;

		// Token: 0x04000B80 RID: 2944
		private bool binning2Enabled;

		// Token: 0x04000B81 RID: 2945
		private bool setHEforSingleEval;

		// Token: 0x04000B82 RID: 2946
		private double deformationFactor;

		// Token: 0x04000B83 RID: 2947
		private bool feedNewProduct;

		// Token: 0x04000B85 RID: 2949
		private ConveyingDirection conveyingDirection;

		// Token: 0x04000B86 RID: 2950
		private bool machineCalibrating;

		// Token: 0x04000B9E RID: 2974
		private TestcardSimService testcardSimService;

		// Token: 0x04000B9F RID: 2975
		private bool highSpeedMachineActive = false;

		// Token: 0x04000BA0 RID: 2976
		private bool showNext = false;

		// Token: 0x04000BA3 RID: 2979
		public static readonly string backgroundNoisePath = "/filter/acquisition/BackgroundNoise/Value";

		// Token: 0x04000BA8 RID: 2984
		private int backgroundNoise;

		// Token: 0x04000BA9 RID: 2985
		private int minBackgroundNoise = 5;

		// Token: 0x04000BAA RID: 2986
		private int maxBackgroundNoise = 30;

		// Token: 0x04000BAB RID: 2987
		private bool isBulk;

		// Token: 0x04000BAC RID: 2988
		private bool isNotBulk;

		// Token: 0x04000BAD RID: 2989
		private string bulkLiveModeSourcePath;

		// Token: 0x04000BAE RID: 2990
		private bool showNextBulk = false;

		// Token: 0x04000BAF RID: 2991
		private bool showNextBulkObject = false;

		// Token: 0x04000BB0 RID: 2992
		private bool bulkContImagesShown = false;

		// Token: 0x04000BB1 RID: 2993
		private string bulkObjectModeSourcePath;

		// Token: 0x04000BB2 RID: 2994
		private bool isNotTX;

		// Token: 0x04000BB3 RID: 2995
		private bool isDeformationVisible;

		// Token: 0x04000BB4 RID: 2996
		private bool notObjectBasedFlaps;
	}
}
