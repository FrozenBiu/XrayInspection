using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using DatabaseConnection;
using Definitions;
using GUI_XRS.Datalogger;

namespace GUI_XRS
{
	// Token: 0x0200022B RID: 555
	public class CtrlOverviewXRS : UserControl, IComponentConnector
	{
		// Token: 0x17000B58 RID: 2904
		// (get) Token: 0x060029AC RID: 10668 RVA: 0x00114145 File Offset: 0x00112345
		// (set) Token: 0x060029AD RID: 10669 RVA: 0x0011414D File Offset: 0x0011234D
		public bool ErrorOutputVisible { get; set; }

		// Token: 0x17000B59 RID: 2905
		// (get) Token: 0x060029AE RID: 10670 RVA: 0x00114156 File Offset: 0x00112356
		// (set) Token: 0x060029AF RID: 10671 RVA: 0x0011415E File Offset: 0x0011235E
		public bool RejectButtonVisible { get; set; }

		// Token: 0x17000B5A RID: 2906
		// (get) Token: 0x060029B0 RID: 10672 RVA: 0x00114167 File Offset: 0x00112367
		// (set) Token: 0x060029B1 RID: 10673 RVA: 0x0011416F File Offset: 0x0011236F
		public bool OldWagoConfiguration { get; set; }

		// Token: 0x17000B5B RID: 2907
		// (get) Token: 0x060029B2 RID: 10674 RVA: 0x00114178 File Offset: 0x00112378
		// (set) Token: 0x060029B3 RID: 10675 RVA: 0x00114180 File Offset: 0x00112380
		public bool BuzzerAvailable { get; set; }

		// Token: 0x17000B5C RID: 2908
		// (get) Token: 0x060029B4 RID: 10676 RVA: 0x00114189 File Offset: 0x00112389
		// (set) Token: 0x060029B5 RID: 10677 RVA: 0x00114191 File Offset: 0x00112391
		public MachineState MachineState { get; set; }

		// Token: 0x17000B5D RID: 2909
		// (get) Token: 0x060029B6 RID: 10678 RVA: 0x0011419A File Offset: 0x0011239A
		// (set) Token: 0x060029B7 RID: 10679 RVA: 0x001141A2 File Offset: 0x001123A2
		public bool DemoMode { get; set; }

		// Token: 0x060029B8 RID: 10680 RVA: 0x001141AC File Offset: 0x001123AC
		public CtrlOverviewXRS(DBConnection dbConnection, ParameterHandling parHandling, Logger logger, AccessRightsManager accessManager, Conveyor conveyor, ErrorHandling errorHandling, ProductManager productManager, MachineState machineState, MessageSystem messageSystem, DBDataLogs dBDataLogs)
		{
			MachineMode machineMode = MachineMode.Standard;
			bool flag = false;
			bool flag2 = true;
			bool flag3 = false;
			this.InitializeComponent();
			this.dbConnection = dbConnection;
			this.parHandling = parHandling;
			this.logger = logger;
			this.accessManager = accessManager;
			this.conveyor = conveyor;
			this.errorHandling = errorHandling;
			this.productManager = productManager;
			this.MachineState = machineState;
			this.messageSystem = messageSystem;
			this.dBDataLogs = dBDataLogs;
			bool demoMode = false;
			dbConnection.GetDemoMode(ref demoMode);
			this.DemoMode = demoMode;
			dbConnection.GetMachineMode(ref machineMode);
			dbConnection.GetOp2HlExtendedRights(ref flag2);
			dbConnection.GetUKUserRightsHL(ref flag3);
			int.TryParse(parHandling.ReadPar(Definition.ioVersionPath), out this.ioVersion);
			this.OldWagoConfiguration = (this.ioVersion < Definition.minNewWagoConfigVersion);
			this.ErrorOutputVisible = (accessManager.CurrentUser.Rights >= BuiltInPriviledges.Operator1 && (!this.OldWagoConfiguration || parHandling.ReadPar(Definition.ioSubversionPath) != "1") && (!flag3 || accessManager.CurrentUser.Rights >= BuiltInPriviledges.Service));
			this.RejectButtonVisible = (accessManager.CurrentUser.Rights >= BuiltInPriviledges.Operator1 || (machineMode == MachineMode.MAndS && accessManager.CurrentUser.Rights >= BuiltInPriviledges.Operator2 && flag2));
			dbConnection.GetBuzzerAvailable(ref flag);
			this.BuzzerAvailable = (flag && accessManager.CurrentUser.Rights >= BuiltInPriviledges.Operator1);
			base.DataContext = this;
			bool flag4 = productManager.CurrentLoadedProduct.ProductNumber.Equals("-1");
			if (flag4)
			{
				this.btnConveyor.IsEnabled = false;
			}
		}

		// Token: 0x060029B9 RID: 10681 RVA: 0x00114374 File Offset: 0x00112574
		private void btnDetector_Click(object sender, RoutedEventArgs e)
		{
			DetectorType detectorType = DetectorType.DT;
			this.dbConnection.GetDetectorType(ref detectorType);
			OverviewCommon overviewCommon = new OverviewCommon(this.parHandling, this.logger);
			overviewCommon.ShowDetectorWindow(detectorType);
		}

		// Token: 0x060029BA RID: 10682 RVA: 0x001143AC File Offset: 0x001125AC
		private void btnSource_Click(object sender, RoutedEventArgs e)
		{
			OverviewCommon overviewCommon = new OverviewCommon(this.parHandling, this.logger);
			overviewCommon.ShowXRaySourceWindow();
		}

		// Token: 0x060029BB RID: 10683 RVA: 0x001143D4 File Offset: 0x001125D4
		private void btnSafetySwitch_Click(object sender, RoutedEventArgs e)
		{
			OverviewCommon overviewCommon = new OverviewCommon(this.parHandling, this.logger);
			overviewCommon.ShowSafetySwitchWindow(this.ioVersion, false);
		}

		// Token: 0x060029BC RID: 10684 RVA: 0x00114404 File Offset: 0x00112604
		private void btnPusher_Click(object sender, RoutedEventArgs e)
		{
			CtrlRejecter usrControl = new CtrlRejecter(this.parHandling, this.dbConnection, this.accessManager, this.MachineState, this.messageSystem);
			Subwindow.Show(usrControl, SubwindowButtons.BackAndHome);
		}

		// Token: 0x060029BD RID: 10685 RVA: 0x00114440 File Offset: 0x00112640
		private void btnTemperatures_Click(object sender, RoutedEventArgs e)
		{
			OverviewCommon overviewCommon = new OverviewCommon(this.parHandling, this.logger);
			overviewCommon.ShowTemperatureWindow(this.ioVersion);
		}

		// Token: 0x060029BE RID: 10686 RVA: 0x00114470 File Offset: 0x00112670
		private void btnVoltages_Click(object sender, RoutedEventArgs e)
		{
			OverviewCommon overviewCommon = new OverviewCommon(this.parHandling, this.logger);
			overviewCommon.ShowVoltageWindow();
		}

		// Token: 0x060029BF RID: 10687 RVA: 0x00114498 File Offset: 0x00112698
		private void btnConveyor_Click(object sender, RoutedEventArgs e)
		{
			CtrlConveyor usrControl = new CtrlConveyor(this.conveyor, this.accessManager, this.dbConnection, this.MachineState, this.dBDataLogs);
			Subwindow.Show(usrControl);
		}

		// Token: 0x060029C0 RID: 10688 RVA: 0x001144D4 File Offset: 0x001126D4
		private void btnErrorOutput_Click(object sender, RoutedEventArgs e)
		{
			CtrlErrorContact usrControl = new CtrlErrorContact(this.errorHandling, this.parHandling, this.dbConnection, this.accessManager);
			Subwindow.Show(usrControl, SubwindowButtons.BackAndHome);
		}

		// Token: 0x060029C1 RID: 10689 RVA: 0x00114508 File Offset: 0x00112708
		private void btnMachineParameter_Click(object sender, RoutedEventArgs e)
		{
			CtrlMachineParameter usrControl = new CtrlMachineParameter(this.dbConnection, this.accessManager, this.parHandling);
			Subwindow.Show(usrControl, SubwindowButtons.BackAndHome);
		}

		// Token: 0x060029C2 RID: 10690 RVA: 0x00114538 File Offset: 0x00112738
		private void btnBuzzer_Click(object sender, RoutedEventArgs e)
		{
			CtrlBuzzer usrControl = new CtrlBuzzer(this.parHandling, this.dbConnection);
			Subwindow.Show(usrControl, SubwindowButtons.BackAndHome);
		}

		// Token: 0x060029C3 RID: 10691 RVA: 0x00114560 File Offset: 0x00112760
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/overviews/ctrloverviewxrs.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x060029C4 RID: 10692 RVA: 0x00114598 File Offset: 0x00112798
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				this.overviewImage = (Image)target;
				break;
			case 2:
				this.btnDetector = (Button)target;
				this.btnDetector.Click += this.btnDetector_Click;
				break;
			case 3:
				this.btnPusher = (Button)target;
				this.btnPusher.Click += this.btnPusher_Click;
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
				this.btnErrorOutput = (Button)target;
				this.btnErrorOutput.Click += this.btnErrorOutput_Click;
				break;
			case 10:
				this.btnMachineParameter = (Button)target;
				this.btnMachineParameter.Click += this.btnMachineParameter_Click;
				break;
			case 11:
				((Button)target).Click += this.btnBuzzer_Click;
				break;
			default:
				this._contentLoaded = true;
				break;
			}
		}

		// Token: 0x040019D4 RID: 6612
		private Logger logger;

		// Token: 0x040019D5 RID: 6613
		private AccessRightsManager accessManager;

		// Token: 0x040019D6 RID: 6614
		private ParameterHandling parHandling;

		// Token: 0x040019D7 RID: 6615
		private DBConnection dbConnection;

		// Token: 0x040019D8 RID: 6616
		private DBDataLogs dBDataLogs;

		// Token: 0x040019D9 RID: 6617
		private Conveyor conveyor;

		// Token: 0x040019DA RID: 6618
		private ErrorHandling errorHandling;

		// Token: 0x040019DB RID: 6619
		private ProductManager productManager;

		// Token: 0x040019DC RID: 6620
		private MessageSystem messageSystem;

		// Token: 0x040019DD RID: 6621
		private int ioVersion = 0;

		// Token: 0x040019E4 RID: 6628
		internal Image overviewImage;

		// Token: 0x040019E5 RID: 6629
		internal Button btnDetector;

		// Token: 0x040019E6 RID: 6630
		internal Button btnPusher;

		// Token: 0x040019E7 RID: 6631
		internal Button btnSafetySwitch;

		// Token: 0x040019E8 RID: 6632
		internal Button btnSource;

		// Token: 0x040019E9 RID: 6633
		internal Button btnTemperatures;

		// Token: 0x040019EA RID: 6634
		internal Button btnVoltages;

		// Token: 0x040019EB RID: 6635
		internal Button btnConveyor;

		// Token: 0x040019EC RID: 6636
		internal Button btnErrorOutput;

		// Token: 0x040019ED RID: 6637
		internal Button btnMachineParameter;

		// Token: 0x040019EE RID: 6638
		private bool _contentLoaded;
	}
}
