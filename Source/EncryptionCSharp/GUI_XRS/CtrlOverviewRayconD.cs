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
	// Token: 0x020000C8 RID: 200
	public class CtrlOverviewRayconD : UserControl, IComponentConnector
	{
		// Token: 0x17000626 RID: 1574
		// (get) Token: 0x06001412 RID: 5138 RVA: 0x00076A8D File Offset: 0x00074C8D
		// (set) Token: 0x06001413 RID: 5139 RVA: 0x00076A95 File Offset: 0x00074C95
		public bool ErrorOutputVisible { get; set; }

		// Token: 0x17000627 RID: 1575
		// (get) Token: 0x06001414 RID: 5140 RVA: 0x00076A9E File Offset: 0x00074C9E
		// (set) Token: 0x06001415 RID: 5141 RVA: 0x00076AA6 File Offset: 0x00074CA6
		public bool RejectButtonVisible { get; set; }

		// Token: 0x17000628 RID: 1576
		// (get) Token: 0x06001416 RID: 5142 RVA: 0x00076AAF File Offset: 0x00074CAF
		// (set) Token: 0x06001417 RID: 5143 RVA: 0x00076AB7 File Offset: 0x00074CB7
		public bool OldWagoConfiguration { get; set; }

		// Token: 0x17000629 RID: 1577
		// (get) Token: 0x06001418 RID: 5144 RVA: 0x00076AC0 File Offset: 0x00074CC0
		// (set) Token: 0x06001419 RID: 5145 RVA: 0x00076AC8 File Offset: 0x00074CC8
		public bool BuzzerAvailable { get; set; }

		// Token: 0x1700062A RID: 1578
		// (get) Token: 0x0600141A RID: 5146 RVA: 0x00076AD1 File Offset: 0x00074CD1
		// (set) Token: 0x0600141B RID: 5147 RVA: 0x00076AD9 File Offset: 0x00074CD9
		public MachineState MachineState { get; set; }

		// Token: 0x1700062B RID: 1579
		// (get) Token: 0x0600141C RID: 5148 RVA: 0x00076AE2 File Offset: 0x00074CE2
		// (set) Token: 0x0600141D RID: 5149 RVA: 0x00076AEA File Offset: 0x00074CEA
		public bool DemoMode { get; set; }

		// Token: 0x1700062C RID: 1580
		// (get) Token: 0x0600141E RID: 5150 RVA: 0x00076AF3 File Offset: 0x00074CF3
		// (set) Token: 0x0600141F RID: 5151 RVA: 0x00076AFB File Offset: 0x00074CFB
		public bool CommunicationsAvailable { get; set; }

		// Token: 0x06001420 RID: 5152 RVA: 0x00076B04 File Offset: 0x00074D04
		public CtrlOverviewRayconD(DBConnection dbConnection, ParameterHandling parHandling, Logger logger, AccessRightsManager accessManager, Conveyor conveyor, ErrorHandling errorHandling, ProductManager productManager, MachineState machineState, MessageSystem messageSystem, List<object> communicationsList, DBDataLogs dBDataLogs)
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
			this.dbDatalogs = dBDataLogs;
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

		// Token: 0x06001421 RID: 5153 RVA: 0x00076D5C File Offset: 0x00074F5C
		private void btnDetector_Click(object sender, RoutedEventArgs e)
		{
			DetectorType detectorType = DetectorType.DT;
			this.dbConnection.GetDetectorType(ref detectorType);
			OverviewCommon overviewCommon = new OverviewCommon(this.parHandling, this.logger);
			overviewCommon.ShowDetectorWindow(detectorType);
		}

		// Token: 0x06001422 RID: 5154 RVA: 0x00076D94 File Offset: 0x00074F94
		private void btnSource_Click(object sender, RoutedEventArgs e)
		{
			OverviewCommon overviewCommon = new OverviewCommon(this.parHandling, this.logger);
			overviewCommon.ShowXRaySourceWindow();
		}

		// Token: 0x06001423 RID: 5155 RVA: 0x00076DBC File Offset: 0x00074FBC
		private void btnSafetySwitch_Click(object sender, RoutedEventArgs e)
		{
			OverviewCommon overviewCommon = new OverviewCommon(this.parHandling, this.logger);
			overviewCommon.ShowSafetySwitchWindow(this.ioVersion, false);
		}

		// Token: 0x06001424 RID: 5156 RVA: 0x00076DEC File Offset: 0x00074FEC
		private void btnPusher_Click(object sender, RoutedEventArgs e)
		{
			CtrlRejecter usrControl = new CtrlRejecter(this.parHandling, this.dbConnection, this.accessManager, this.MachineState, this.messageSystem);
			Subwindow.Show(usrControl, SubwindowButtons.BackAndHome);
		}

		// Token: 0x06001425 RID: 5157 RVA: 0x00076E28 File Offset: 0x00075028
		private void btnTemperatures_Click(object sender, RoutedEventArgs e)
		{
			OverviewCommon overviewCommon = new OverviewCommon(this.parHandling, this.logger);
			overviewCommon.ShowTemperatureWindow(this.ioVersion);
		}

		// Token: 0x06001426 RID: 5158 RVA: 0x00076E58 File Offset: 0x00075058
		private void btnConveyor_Click(object sender, RoutedEventArgs e)
		{
			CtrlConveyor usrControl = new CtrlConveyor(this.conveyor, this.accessManager, this.dbConnection, this.MachineState, this.dbDatalogs);
			Subwindow.Show(usrControl);
		}

		// Token: 0x06001427 RID: 5159 RVA: 0x00076E94 File Offset: 0x00075094
		private void btnErrorOutput_Click(object sender, RoutedEventArgs e)
		{
			CtrlErrorContact usrControl = new CtrlErrorContact(this.errorHandling, this.parHandling, this.dbConnection, this.accessManager);
			Subwindow.Show(usrControl, SubwindowButtons.BackAndHome);
		}

		// Token: 0x06001428 RID: 5160 RVA: 0x00076EC8 File Offset: 0x000750C8
		private void btnMachineParameter_Click(object sender, RoutedEventArgs e)
		{
			CtrlMachineParameter usrControl = new CtrlMachineParameter(this.dbConnection, this.accessManager, this.parHandling);
			Subwindow.Show(usrControl, SubwindowButtons.BackAndHome);
		}

		// Token: 0x06001429 RID: 5161 RVA: 0x00076EF8 File Offset: 0x000750F8
		private void btnBuzzer_Click(object sender, RoutedEventArgs e)
		{
			CtrlBuzzer usrControl = new CtrlBuzzer(this.parHandling, this.dbConnection);
			Subwindow.Show(usrControl, SubwindowButtons.BackAndHome);
		}

		// Token: 0x0600142A RID: 5162 RVA: 0x00076F20 File Offset: 0x00075120
		private void btnCommunications_Click(object sender, RoutedEventArgs e)
		{
			SubMenu usrControl = new SubMenu(this.dbConnection, this.accessManager, this.MachineState, this.communicationsList);
			Subwindow.Show(usrControl, SubwindowButtons.BackAndHome);
		}

		// Token: 0x0600142B RID: 5163 RVA: 0x00076F54 File Offset: 0x00075154
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/overviews/ctrloverviewraycond.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x0600142C RID: 5164 RVA: 0x00076F8C File Offset: 0x0007518C
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

		// Token: 0x04000D39 RID: 3385
		private Logger logger;

		// Token: 0x04000D3A RID: 3386
		private AccessRightsManager accessManager;

		// Token: 0x04000D3B RID: 3387
		private ParameterHandling parHandling;

		// Token: 0x04000D3C RID: 3388
		private DBConnection dbConnection;

		// Token: 0x04000D3D RID: 3389
		private DBDataLogs dbDatalogs;

		// Token: 0x04000D3E RID: 3390
		private Conveyor conveyor;

		// Token: 0x04000D3F RID: 3391
		private ErrorHandling errorHandling;

		// Token: 0x04000D40 RID: 3392
		private ProductManager productManager;

		// Token: 0x04000D41 RID: 3393
		private MessageSystem messageSystem;

		// Token: 0x04000D42 RID: 3394
		private List<object> communicationsList;

		// Token: 0x04000D43 RID: 3395
		private int ioVersion = 0;

		// Token: 0x04000D4B RID: 3403
		internal Image overviewImage;

		// Token: 0x04000D4C RID: 3404
		internal Button btnDetector;

		// Token: 0x04000D4D RID: 3405
		internal Button btnPusher;

		// Token: 0x04000D4E RID: 3406
		internal Button btnSafetySwitch;

		// Token: 0x04000D4F RID: 3407
		internal Button btnSource;

		// Token: 0x04000D50 RID: 3408
		internal Button btnTemperatures;

		// Token: 0x04000D51 RID: 3409
		internal Button btnConveyor;

		// Token: 0x04000D52 RID: 3410
		internal Button btnErrorOutput;

		// Token: 0x04000D53 RID: 3411
		internal Button btnMachineParameter;

		// Token: 0x04000D54 RID: 3412
		internal Button btnConnectioins;

		// Token: 0x04000D55 RID: 3413
		private bool _contentLoaded;
	}
}
