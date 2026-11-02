using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media.Imaging;
using DatabaseConnection;
using Definitions;
using GUI_XRS.Datalogger;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x02000110 RID: 272
	public class CtrlOverviewBulk : UserControl, INotifyPropertyChanged, IComponentConnector
	{
		// Token: 0x1700079F RID: 1951
		// (get) Token: 0x0600198F RID: 6543 RVA: 0x00096DBA File Offset: 0x00094FBA
		// (set) Token: 0x06001990 RID: 6544 RVA: 0x00096DC2 File Offset: 0x00094FC2
		public BitmapImage BmpSensorBottomWhite { get; set; }

		// Token: 0x170007A0 RID: 1952
		// (get) Token: 0x06001991 RID: 6545 RVA: 0x00096DCB File Offset: 0x00094FCB
		// (set) Token: 0x06001992 RID: 6546 RVA: 0x00096DD3 File Offset: 0x00094FD3
		public BitmapImage BmpSensorBottomRed { get; set; }

		// Token: 0x170007A1 RID: 1953
		// (get) Token: 0x06001993 RID: 6547 RVA: 0x00096DDC File Offset: 0x00094FDC
		// (set) Token: 0x06001994 RID: 6548 RVA: 0x00096DE4 File Offset: 0x00094FE4
		public BitmapImage BmpSensorTopWhite { get; set; }

		// Token: 0x170007A2 RID: 1954
		// (get) Token: 0x06001995 RID: 6549 RVA: 0x00096DED File Offset: 0x00094FED
		// (set) Token: 0x06001996 RID: 6550 RVA: 0x00096DF5 File Offset: 0x00094FF5
		public BitmapImage BmpSensorTopRed { get; set; }

		// Token: 0x170007A3 RID: 1955
		// (get) Token: 0x06001997 RID: 6551 RVA: 0x00096E00 File Offset: 0x00095000
		// (set) Token: 0x06001998 RID: 6552 RVA: 0x00096E18 File Offset: 0x00095018
		public BitmapImage BmpSensorTop
		{
			get
			{
				return this.bmpSensorTop;
			}
			set
			{
				bool flag = this.bmpSensorTop != value;
				if (flag)
				{
					this.bmpSensorTop = value;
					this.OnPropertyChanged("BmpSensorTop");
				}
			}
		}

		// Token: 0x170007A4 RID: 1956
		// (get) Token: 0x06001999 RID: 6553 RVA: 0x00096E4C File Offset: 0x0009504C
		// (set) Token: 0x0600199A RID: 6554 RVA: 0x00096E64 File Offset: 0x00095064
		public BitmapImage BmpSensorBottom
		{
			get
			{
				return this.bmpSensorBottom;
			}
			set
			{
				bool flag = this.bmpSensorBottom != value;
				if (flag)
				{
					this.bmpSensorBottom = value;
					this.OnPropertyChanged("BmpSensorBottom");
				}
			}
		}

		// Token: 0x170007A5 RID: 1957
		// (get) Token: 0x0600199B RID: 6555 RVA: 0x00096E97 File Offset: 0x00095097
		// (set) Token: 0x0600199C RID: 6556 RVA: 0x00096E9F File Offset: 0x0009509F
		public bool CleaningVibratorEnabled { get; set; }

		// Token: 0x170007A6 RID: 1958
		// (get) Token: 0x0600199D RID: 6557 RVA: 0x00096EA8 File Offset: 0x000950A8
		// (set) Token: 0x0600199E RID: 6558 RVA: 0x00096EB0 File Offset: 0x000950B0
		public bool BuzzerAvailable { get; set; }

		// Token: 0x170007A7 RID: 1959
		// (get) Token: 0x0600199F RID: 6559 RVA: 0x00096EB9 File Offset: 0x000950B9
		// (set) Token: 0x060019A0 RID: 6560 RVA: 0x00096EC1 File Offset: 0x000950C1
		public MachineState MachineState { get; set; }

		// Token: 0x14000082 RID: 130
		// (add) Token: 0x060019A1 RID: 6561 RVA: 0x00096ECC File Offset: 0x000950CC
		// (remove) Token: 0x060019A2 RID: 6562 RVA: 0x00096F04 File Offset: 0x00095104
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x170007A8 RID: 1960
		// (get) Token: 0x060019A3 RID: 6563 RVA: 0x00096F39 File Offset: 0x00095139
		// (set) Token: 0x060019A4 RID: 6564 RVA: 0x00096F41 File Offset: 0x00095141
		public bool ErrorOutputVisible { get; set; }

		// Token: 0x170007A9 RID: 1961
		// (get) Token: 0x060019A5 RID: 6565 RVA: 0x00096F4A File Offset: 0x0009514A
		// (set) Token: 0x060019A6 RID: 6566 RVA: 0x00096F52 File Offset: 0x00095152
		public bool OldWagoConfiguration { get; set; }

		// Token: 0x060019A7 RID: 6567 RVA: 0x00096F5C File Offset: 0x0009515C
		public CtrlOverviewBulk(DBConnection dbConnection, ParameterHandling parHandling, Logger logger, AccessRightsManager accessManager, Vibro vibro, ErrorHandling errorHandling, Conveyor conveyor, MachineState machineState, DBDataLogs dBDatalogs, MessageSystem messageSystem, ImageDataTransfer imageDataTransfer, ProductManager productManager)
		{
			int num = 0;
			bool cleaningVibratorEnabled = false;
			bool flag = false;
			this.InitializeComponent();
			this.dbConnection = dbConnection;
			this.parHandling = parHandling;
			this.logger = logger;
			this.accessManager = accessManager;
			this.vibro = vibro;
			this.errorHandling = errorHandling;
			this.conveyor = conveyor;
			this.machineState = machineState;
			this.dbDatalogs = dBDatalogs;
			this.messageSystem = messageSystem;
			this.imageDataTransfer = imageDataTransfer;
			this.productManager = productManager;
			int.TryParse(parHandling.ReadPar(Definition.ioVersionPath), out this.ioVersion);
			this.OldWagoConfiguration = (this.ioVersion < Definition.minNewWagoConfigVersion);
			this.ErrorOutputVisible = (accessManager.CurrentUser.Rights >= BuiltInPriviledges.Operator1 && (!this.OldWagoConfiguration || parHandling.ReadPar(Definition.ioSubversionPath) != "1"));
			int.TryParse(parHandling.ReadPar(Definition.ioBulkVersionPath), out num);
			this.newBulkConfig = true;
			this.BmpSensorTopRed = ImageConverter.ToBitmap("/img/sensorRed_top.png");
			this.BmpSensorTopWhite = ImageConverter.ToBitmap("/img/sensorWhite_top.png");
			this.BmpSensorBottomRed = ImageConverter.ToBitmap("/img/sensorRed_bottom.png");
			this.BmpSensorBottomWhite = ImageConverter.ToBitmap("/img/sensorWhite_bottom.png");
			this.SetFeedingSensorsBitmap();
			this.StartFeedingBinsSurvey();
			dbConnection.GetBulkCleaningVibrator(ref cleaningVibratorEnabled);
			this.CleaningVibratorEnabled = cleaningVibratorEnabled;
			dbConnection.GetBuzzerAvailable(ref flag);
			this.BuzzerAvailable = (flag && accessManager.CurrentUser.Rights >= BuiltInPriviledges.Operator1);
			base.DataContext = this;
			bool flag2 = productManager.CurrentLoadedProduct.ProductNumber.Equals("-1");
			if (flag2)
			{
				this.btnConveyor.IsEnabled = false;
			}
		}

		// Token: 0x060019A8 RID: 6568 RVA: 0x00097124 File Offset: 0x00095324
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x060019A9 RID: 6569 RVA: 0x00097154 File Offset: 0x00095354
		private void btnVibro_Click(object sender, RoutedEventArgs e)
		{
			CtrlVibro usrControl = new CtrlVibro(this.vibro, this.accessManager, this.parHandling);
			Subwindow.Show(usrControl);
		}

		// Token: 0x060019AA RID: 6570 RVA: 0x00097184 File Offset: 0x00095384
		private void btnDetector_Click(object sender, RoutedEventArgs e)
		{
			DetectorType detectorType = DetectorType.DT;
			this.dbConnection.GetDetectorType(ref detectorType);
			OverviewCommon overviewCommon = new OverviewCommon(this.parHandling, this.logger);
			overviewCommon.ShowDetectorWindow(detectorType);
		}

		// Token: 0x060019AB RID: 6571 RVA: 0x000971BC File Offset: 0x000953BC
		private void btnSource_Click(object sender, RoutedEventArgs e)
		{
			OverviewCommon overviewCommon = new OverviewCommon(this.parHandling, this.logger);
			overviewCommon.ShowXRaySourceWindow();
		}

		// Token: 0x060019AC RID: 6572 RVA: 0x000971E4 File Offset: 0x000953E4
		private void btnSafetySwitch_Click(object sender, RoutedEventArgs e)
		{
			OverviewCommon overviewCommon = new OverviewCommon(this.parHandling, this.logger);
			overviewCommon.ShowSafetySwitchWindow(this.ioVersion, this.newBulkConfig);
		}

		// Token: 0x060019AD RID: 6573 RVA: 0x00097218 File Offset: 0x00095418
		private void btnTemperatures_Click(object sender, RoutedEventArgs e)
		{
			OverviewCommon overviewCommon = new OverviewCommon(this.parHandling, this.logger);
			overviewCommon.ShowTemperatureWindow(this.ioVersion);
		}

		// Token: 0x060019AE RID: 6574 RVA: 0x00097248 File Offset: 0x00095448
		private void btnVoltages_Click(object sender, RoutedEventArgs e)
		{
			OverviewCommon overviewCommon = new OverviewCommon(this.parHandling, this.logger);
			overviewCommon.ShowVoltageWindow();
		}

		// Token: 0x060019AF RID: 6575 RVA: 0x00097270 File Offset: 0x00095470
		private void btnConveyor_Click(object sender, RoutedEventArgs e)
		{
			CtrlConveyor usrControl = new CtrlConveyor(this.conveyor, this.accessManager, this.dbConnection, this.machineState, this.dbDatalogs, this.parHandling);
			Subwindow.Show(usrControl);
		}

		// Token: 0x060019B0 RID: 6576 RVA: 0x000972B0 File Offset: 0x000954B0
		private void btnErrorOutput_Click(object sender, RoutedEventArgs e)
		{
			CtrlErrorContact usrControl = new CtrlErrorContact(this.errorHandling, this.parHandling, this.dbConnection, this.accessManager);
			Subwindow.Show(usrControl, SubwindowButtons.BackAndHome);
		}

		// Token: 0x060019B1 RID: 6577 RVA: 0x000972E4 File Offset: 0x000954E4
		private void btnSVT_Click(object sender, RoutedEventArgs e)
		{
			MachineMode machineMode = MachineMode.Standard;
			this.dbConnection.GetMachineMode(ref machineMode);
			bool flag = this.parHandling.ReadPar(FlapsOverIOs.bulkflapsEnabledPath) == "1" && !this.productManager.CurrentLoadedProduct.IsBulkObjectProduct;
			if (flag)
			{
				CtrlFlapsOverIOs usrControl = new CtrlFlapsOverIOs(this.parHandling, this.dbConnection, this.machineState, this.accessManager, this.messageSystem, machineMode);
				Subwindow.Show(usrControl, SubwindowButtons.BackAndHome);
			}
			else
			{
				bool flag2 = machineMode == MachineMode.BulkObject;
				if (flag2)
				{
					CtrlRejecter usrControl2 = new CtrlRejecter(this.parHandling, this.dbConnection, this.accessManager, this.machineState, this.messageSystem);
					Subwindow.Show(usrControl2, SubwindowButtons.BackAndHome);
				}
				else
				{
					this.ctrlSVT = new CtrlSVT(this.parHandling, this.dbConnection, this.machineState, this.accessManager, this.imageDataTransfer, this.dbDatalogs);
					this.ctrlSVT.GraphPlotUnloaded += this.bulkPlotUnloaded;
					this.ctrlSVT.GraphPlotInitialized += this.ViewModel_GraphPlotInitialized;
					Subwindow.Show(this.ctrlSVT, SubwindowButtons.BackAndHome);
				}
			}
		}

		// Token: 0x060019B2 RID: 6578 RVA: 0x00097414 File Offset: 0x00095614
		private void ViewModel_GraphPlotInitialized()
		{
			this.OnBulkPlotInitialized();
		}

		// Token: 0x060019B3 RID: 6579 RVA: 0x0009741E File Offset: 0x0009561E
		private void bulkPlotUnloaded(MachineStates obj)
		{
			this.OnBulkPlotUnloaded(obj);
		}

		// Token: 0x14000083 RID: 131
		// (add) Token: 0x060019B4 RID: 6580 RVA: 0x0009742C File Offset: 0x0009562C
		// (remove) Token: 0x060019B5 RID: 6581 RVA: 0x00097464 File Offset: 0x00095664
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<MachineStates> BulkPlotUnloaded;

		// Token: 0x060019B6 RID: 6582 RVA: 0x0009749C File Offset: 0x0009569C
		protected virtual void OnBulkPlotUnloaded(MachineStates oldState)
		{
			Action<MachineStates> bulkPlotUnloaded = this.BulkPlotUnloaded;
			bool flag = bulkPlotUnloaded != null;
			if (flag)
			{
				bulkPlotUnloaded(oldState);
			}
		}

		// Token: 0x14000084 RID: 132
		// (add) Token: 0x060019B7 RID: 6583 RVA: 0x000974C4 File Offset: 0x000956C4
		// (remove) Token: 0x060019B8 RID: 6584 RVA: 0x000974FC File Offset: 0x000956FC
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action BulkPlotInitialized;

		// Token: 0x060019B9 RID: 6585 RVA: 0x00097534 File Offset: 0x00095734
		protected virtual void OnBulkPlotInitialized()
		{
			Action bulkPlotInitialized = this.BulkPlotInitialized;
			bool flag = bulkPlotInitialized != null;
			if (flag)
			{
				bulkPlotInitialized();
			}
		}

		// Token: 0x060019BA RID: 6586 RVA: 0x00097558 File Offset: 0x00095758
		private void btnActivator_Click(object sender, RoutedEventArgs e)
		{
			CtrlActivator usrControl = new CtrlActivator(this.dbConnection, this.parHandling);
			Subwindow.Show(usrControl, SubwindowButtons.BackAndHome);
		}

		// Token: 0x060019BB RID: 6587 RVA: 0x00097580 File Offset: 0x00095780
		private void StartFeedingBinsSurvey()
		{
			this.feedingSurveyRunning = true;
			new Thread(new ThreadStart(this.SurveyFeedingBins))
			{
				Name = "SurveyFeedingBins"
			}.Start();
		}

		// Token: 0x060019BC RID: 6588 RVA: 0x000975BA File Offset: 0x000957BA
		private void StopFeedingBinsSurvey()
		{
			this.feedingSurveyRunning = false;
		}

		// Token: 0x060019BD RID: 6589 RVA: 0x000975C4 File Offset: 0x000957C4
		private bool FeddingBinHighIsOn()
		{
			return this.parHandling.ReadPar(Definition.feedingBinHighPath) == "1";
		}

		// Token: 0x060019BE RID: 6590 RVA: 0x000975F0 File Offset: 0x000957F0
		private bool FeddingBinLowIsOn()
		{
			return this.parHandling.ReadPar(Definition.feedingBinLowPath) == "1";
		}

		// Token: 0x060019BF RID: 6591 RVA: 0x0009761C File Offset: 0x0009581C
		private void SurveyFeedingBins()
		{
			while (this.feedingSurveyRunning)
			{
				Thread.Sleep(1000);
				this.SetFeedingSensorsBitmap();
			}
		}

		// Token: 0x060019C0 RID: 6592 RVA: 0x0009764C File Offset: 0x0009584C
		private void SetFeedingSensorsBitmap()
		{
			bool flag = this.FeddingBinHighIsOn();
			if (flag)
			{
				this.BmpSensorTop = this.BmpSensorTopRed;
			}
			else
			{
				this.BmpSensorTop = this.BmpSensorTopWhite;
			}
			bool flag2 = this.FeddingBinLowIsOn();
			if (flag2)
			{
				this.BmpSensorBottom = this.BmpSensorBottomRed;
			}
			else
			{
				this.BmpSensorBottom = this.BmpSensorBottomWhite;
			}
		}

		// Token: 0x060019C1 RID: 6593 RVA: 0x000976A8 File Offset: 0x000958A8
		private void CtrlOverviewBulk_Unloaded(object sender, RoutedEventArgs e)
		{
			bool flag = this.ctrlSVT != null;
			if (flag)
			{
				this.ctrlSVT.GraphPlotUnloaded -= this.bulkPlotUnloaded;
				this.ctrlSVT.GraphPlotInitialized -= this.ViewModel_GraphPlotInitialized;
			}
			this.StopFeedingBinsSurvey();
		}

		// Token: 0x060019C2 RID: 6594 RVA: 0x000976FC File Offset: 0x000958FC
		private void btnMachineParameter_Click(object sender, RoutedEventArgs e)
		{
			CtrlMachineParameter usrControl = new CtrlMachineParameter(this.dbConnection, this.accessManager, this.parHandling);
			Subwindow.Show(usrControl, SubwindowButtons.BackAndHome);
		}

		// Token: 0x060019C3 RID: 6595 RVA: 0x0009772C File Offset: 0x0009592C
		private void btnBuzzer_Click(object sender, RoutedEventArgs e)
		{
			CtrlBuzzer usrControl = new CtrlBuzzer(this.parHandling, this.dbConnection);
			Subwindow.Show(usrControl, SubwindowButtons.BackAndHome);
		}

		// Token: 0x060019C4 RID: 6596 RVA: 0x00097754 File Offset: 0x00095954
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/overviews/ctrloverviewbulk.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x060019C5 RID: 6597 RVA: 0x0009778C File Offset: 0x0009598C
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				((CtrlOverviewBulk)target).Unloaded += this.CtrlOverviewBulk_Unloaded;
				break;
			case 2:
				this.overviewImage = (Image)target;
				break;
			case 3:
				this.btnDetector = (Button)target;
				this.btnDetector.Click += this.btnDetector_Click;
				break;
			case 4:
				this.btnSafetySwitch = (Button)target;
				this.btnSafetySwitch.Click += this.btnSafetySwitch_Click;
				break;
			case 5:
				this.btnSource = (Button)target;
				this.btnSource.Click += this.btnSource_Click;
				break;
			case 6:
				this.btnTemperatures = (Button)target;
				this.btnTemperatures.Click += this.btnTemperatures_Click;
				break;
			case 7:
				this.btnVoltages = (Button)target;
				this.btnVoltages.Click += this.btnVoltages_Click;
				break;
			case 8:
				this.btnConveyor = (Button)target;
				this.btnConveyor.Click += this.btnConveyor_Click;
				break;
			case 9:
				this.btnSVT = (Button)target;
				this.btnSVT.Click += this.btnSVT_Click;
				break;
			case 10:
				this.btnErrorOutput = (Button)target;
				this.btnErrorOutput.Click += this.btnErrorOutput_Click;
				break;
			case 11:
				this.btnMachineParameter = (Button)target;
				this.btnMachineParameter.Click += this.btnMachineParameter_Click;
				break;
			case 12:
				((Button)target).Click += this.btnBuzzer_Click;
				break;
			default:
				this._contentLoaded = true;
				break;
			}
		}

		// Token: 0x040010B0 RID: 4272
		private const int feedingBinsSurveyInterval = 1000;

		// Token: 0x040010B1 RID: 4273
		private Logger logger;

		// Token: 0x040010B2 RID: 4274
		private AccessRightsManager accessManager;

		// Token: 0x040010B3 RID: 4275
		private ParameterHandling parHandling;

		// Token: 0x040010B4 RID: 4276
		private DBConnection dbConnection;

		// Token: 0x040010B5 RID: 4277
		private DBDataLogs dbDatalogs;

		// Token: 0x040010B6 RID: 4278
		private Vibro vibro;

		// Token: 0x040010B7 RID: 4279
		private ErrorHandling errorHandling;

		// Token: 0x040010B8 RID: 4280
		private Conveyor conveyor;

		// Token: 0x040010B9 RID: 4281
		private MachineState machineState;

		// Token: 0x040010BA RID: 4282
		private BitmapImage bmpSensorBottom;

		// Token: 0x040010BB RID: 4283
		private BitmapImage bmpSensorTop;

		// Token: 0x040010BC RID: 4284
		private bool feedingSurveyRunning;

		// Token: 0x040010BD RID: 4285
		private bool newBulkConfig;

		// Token: 0x040010BE RID: 4286
		private int ioVersion = 0;

		// Token: 0x040010BF RID: 4287
		private MessageSystem messageSystem;

		// Token: 0x040010CA RID: 4298
		private ImageDataTransfer imageDataTransfer;

		// Token: 0x040010CB RID: 4299
		private ProductManager productManager;

		// Token: 0x040010CC RID: 4300
		private CtrlSVT ctrlSVT;

		// Token: 0x040010CF RID: 4303
		internal Image overviewImage;

		// Token: 0x040010D0 RID: 4304
		internal Button btnDetector;

		// Token: 0x040010D1 RID: 4305
		internal Button btnSafetySwitch;

		// Token: 0x040010D2 RID: 4306
		internal Button btnSource;

		// Token: 0x040010D3 RID: 4307
		internal Button btnTemperatures;

		// Token: 0x040010D4 RID: 4308
		internal Button btnVoltages;

		// Token: 0x040010D5 RID: 4309
		internal Button btnConveyor;

		// Token: 0x040010D6 RID: 4310
		internal Button btnSVT;

		// Token: 0x040010D7 RID: 4311
		internal Button btnErrorOutput;

		// Token: 0x040010D8 RID: 4312
		internal Button btnMachineParameter;

		// Token: 0x040010D9 RID: 4313
		private bool _contentLoaded;
	}
}
