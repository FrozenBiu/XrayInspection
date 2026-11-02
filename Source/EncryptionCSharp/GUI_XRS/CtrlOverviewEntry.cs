using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
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
	// Token: 0x020000C7 RID: 199
	public class CtrlOverviewEntry : UserControl, IComponentConnector
	{
		// Token: 0x1700061F RID: 1567
		// (get) Token: 0x060013F7 RID: 5111 RVA: 0x000763A9 File Offset: 0x000745A9
		// (set) Token: 0x060013F8 RID: 5112 RVA: 0x000763B1 File Offset: 0x000745B1
		public bool ErrorOutputVisible { get; set; }

		// Token: 0x17000620 RID: 1568
		// (get) Token: 0x060013F9 RID: 5113 RVA: 0x000763BA File Offset: 0x000745BA
		// (set) Token: 0x060013FA RID: 5114 RVA: 0x000763C2 File Offset: 0x000745C2
		public bool RejectButtonVisible { get; set; }

		// Token: 0x17000621 RID: 1569
		// (get) Token: 0x060013FB RID: 5115 RVA: 0x000763CB File Offset: 0x000745CB
		// (set) Token: 0x060013FC RID: 5116 RVA: 0x000763D3 File Offset: 0x000745D3
		public bool OldWagoConfiguration { get; set; }

		// Token: 0x17000622 RID: 1570
		// (get) Token: 0x060013FD RID: 5117 RVA: 0x000763DC File Offset: 0x000745DC
		// (set) Token: 0x060013FE RID: 5118 RVA: 0x000763E4 File Offset: 0x000745E4
		public bool BuzzerAvailable { get; set; }

		// Token: 0x17000623 RID: 1571
		// (get) Token: 0x060013FF RID: 5119 RVA: 0x000763ED File Offset: 0x000745ED
		// (set) Token: 0x06001400 RID: 5120 RVA: 0x000763F5 File Offset: 0x000745F5
		public MachineState MachineState { get; set; }

		// Token: 0x17000624 RID: 1572
		// (get) Token: 0x06001401 RID: 5121 RVA: 0x000763FE File Offset: 0x000745FE
		// (set) Token: 0x06001402 RID: 5122 RVA: 0x00076406 File Offset: 0x00074606
		public bool DemoMode { get; set; }

		// Token: 0x17000625 RID: 1573
		// (get) Token: 0x06001403 RID: 5123 RVA: 0x0007640F File Offset: 0x0007460F
		// (set) Token: 0x06001404 RID: 5124 RVA: 0x00076417 File Offset: 0x00074617
		public bool CommunicationsAvailable { get; set; }

		// Token: 0x06001405 RID: 5125 RVA: 0x00076420 File Offset: 0x00074620
		public CtrlOverviewEntry(DBConnection dbConnection, ParameterHandling parHandling, Logger logger, AccessRightsManager accessManager, Conveyor conveyor, ErrorHandling errorHandling, ProductManager productManager, MachineState machineState, MessageSystem messageSystem, List<object> communicationsList, DBDataLogs dbDatalogs)
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
			this.communicationsList = communicationsList;
			this.dbDatalogs = dbDatalogs;
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
			foreach (object obj in communicationsList)
			{
				bool flag4 = obj.GetType().FullName == "GUI_XRS.VideojetPrinter";
				if (flag4)
				{
					VideojetPrinter videojetPrinter = (VideojetPrinter)obj;
					bool available = videojetPrinter.Available;
					if (available)
					{
						this.CommunicationsAvailable = true;
					}
					else
					{
						this.CommunicationsAvailable = false;
					}
				}
			}
			base.DataContext = this;
			bool flag5 = productManager.CurrentLoadedProduct.ProductNumber.Equals("-1");
			if (flag5)
			{
				this.btnConveyor.IsEnabled = false;
			}
		}

		// Token: 0x06001406 RID: 5126 RVA: 0x00076678 File Offset: 0x00074878
		private void btnDetector_Click(object sender, RoutedEventArgs e)
		{
			DetectorType detectorType = DetectorType.DT;
			this.dbConnection.GetDetectorType(ref detectorType);
			OverviewCommon overviewCommon = new OverviewCommon(this.parHandling, this.logger);
			overviewCommon.ShowDetectorWindow(detectorType);
		}

		// Token: 0x06001407 RID: 5127 RVA: 0x000766B0 File Offset: 0x000748B0
		private void btnSource_Click(object sender, RoutedEventArgs e)
		{
			OverviewCommon overviewCommon = new OverviewCommon(this.parHandling, this.logger);
			overviewCommon.ShowXRaySourceWindow();
		}

		// Token: 0x06001408 RID: 5128 RVA: 0x000766D8 File Offset: 0x000748D8
		private void btnSafetySwitch_Click(object sender, RoutedEventArgs e)
		{
			OverviewCommon overviewCommon = new OverviewCommon(this.parHandling, this.logger);
			overviewCommon.ShowSafetySwitchWindow(this.ioVersion, false);
		}

		// Token: 0x06001409 RID: 5129 RVA: 0x00076708 File Offset: 0x00074908
		private void btnPusher_Click(object sender, RoutedEventArgs e)
		{
			CtrlRejecter usrControl = new CtrlRejecter(this.parHandling, this.dbConnection, this.accessManager, this.MachineState, this.messageSystem);
			Subwindow.Show(usrControl, SubwindowButtons.BackAndHome);
		}

		// Token: 0x0600140A RID: 5130 RVA: 0x00076744 File Offset: 0x00074944
		private void btnTemperatures_Click(object sender, RoutedEventArgs e)
		{
			OverviewCommon overviewCommon = new OverviewCommon(this.parHandling, this.logger);
			overviewCommon.ShowTemperatureWindow(this.ioVersion);
		}

		// Token: 0x0600140B RID: 5131 RVA: 0x00076774 File Offset: 0x00074974
		private void btnConveyor_Click(object sender, RoutedEventArgs e)
		{
			CtrlConveyor usrControl = new CtrlConveyor(this.conveyor, this.accessManager, this.dbConnection, this.MachineState, this.dbDatalogs);
			Subwindow.Show(usrControl);
		}

		// Token: 0x0600140C RID: 5132 RVA: 0x000767B0 File Offset: 0x000749B0
		private void btnErrorOutput_Click(object sender, RoutedEventArgs e)
		{
			CtrlErrorContact usrControl = new CtrlErrorContact(this.errorHandling, this.parHandling, this.dbConnection, this.accessManager);
			Subwindow.Show(usrControl, SubwindowButtons.BackAndHome);
		}

		// Token: 0x0600140D RID: 5133 RVA: 0x000767E4 File Offset: 0x000749E4
		private void btnMachineParameter_Click(object sender, RoutedEventArgs e)
		{
			CtrlMachineParameter usrControl = new CtrlMachineParameter(this.dbConnection, this.accessManager, this.parHandling);
			Subwindow.Show(usrControl, SubwindowButtons.BackAndHome);
		}

		// Token: 0x0600140E RID: 5134 RVA: 0x00076814 File Offset: 0x00074A14
		private void btnBuzzer_Click(object sender, RoutedEventArgs e)
		{
			CtrlBuzzer usrControl = new CtrlBuzzer(this.parHandling, this.dbConnection);
			Subwindow.Show(usrControl, SubwindowButtons.BackAndHome);
		}

		// Token: 0x0600140F RID: 5135 RVA: 0x0007683C File Offset: 0x00074A3C
		private void btnCommunications_Click(object sender, RoutedEventArgs e)
		{
			SubMenu usrControl = new SubMenu(this.dbConnection, this.accessManager, this.MachineState, this.communicationsList);
			Subwindow.Show(usrControl, SubwindowButtons.BackAndHome);
		}

		// Token: 0x06001410 RID: 5136 RVA: 0x00076870 File Offset: 0x00074A70
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/overviews/ctrloverviewentry.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x06001411 RID: 5137 RVA: 0x000768A8 File Offset: 0x00074AA8
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
				this.btnConveyor = (Button)target;
				this.btnConveyor.Click += this.btnConveyor_Click;
				break;
			case 8:
				this.btnErrorOutput = (Button)target;
				this.btnErrorOutput.Click += this.btnErrorOutput_Click;
				break;
			case 9:
				this.btnMachineParameter = (Button)target;
				this.btnMachineParameter.Click += this.btnMachineParameter_Click;
				break;
			case 10:
				((Button)target).Click += this.btnBuzzer_Click;
				break;
			case 11:
				this.btnConnectioins = (Button)target;
				this.btnConnectioins.Click += this.btnCommunications_Click;
				break;
			default:
				this._contentLoaded = true;
				break;
			}
		}

		// Token: 0x04000D1C RID: 3356
		private Logger logger;

		// Token: 0x04000D1D RID: 3357
		private AccessRightsManager accessManager;

		// Token: 0x04000D1E RID: 3358
		private ParameterHandling parHandling;

		// Token: 0x04000D1F RID: 3359
		private DBConnection dbConnection;

		// Token: 0x04000D20 RID: 3360
		private DBDataLogs dbDatalogs;

		// Token: 0x04000D21 RID: 3361
		private Conveyor conveyor;

		// Token: 0x04000D22 RID: 3362
		private ErrorHandling errorHandling;

		// Token: 0x04000D23 RID: 3363
		private ProductManager productManager;

		// Token: 0x04000D24 RID: 3364
		private MessageSystem messageSystem;

		// Token: 0x04000D25 RID: 3365
		private List<object> communicationsList;

		// Token: 0x04000D26 RID: 3366
		private int ioVersion = 0;

		// Token: 0x04000D2E RID: 3374
		internal Image overviewImage;

		// Token: 0x04000D2F RID: 3375
		internal Button btnDetector;

		// Token: 0x04000D30 RID: 3376
		internal Button btnPusher;

		// Token: 0x04000D31 RID: 3377
		internal Button btnSafetySwitch;

		// Token: 0x04000D32 RID: 3378
		internal Button btnSource;

		// Token: 0x04000D33 RID: 3379
		internal Button btnTemperatures;

		// Token: 0x04000D34 RID: 3380
		internal Button btnConveyor;

		// Token: 0x04000D35 RID: 3381
		internal Button btnErrorOutput;

		// Token: 0x04000D36 RID: 3382
		internal Button btnMachineParameter;

		// Token: 0x04000D37 RID: 3383
		internal Button btnConnectioins;

		// Token: 0x04000D38 RID: 3384
		private bool _contentLoaded;
	}
}
