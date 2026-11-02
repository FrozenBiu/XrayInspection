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
	// Token: 0x02000108 RID: 264
	public class CtrlOverviewSsh : UserControl, IComponentConnector
	{
		// Token: 0x1700075A RID: 1882
		// (get) Token: 0x060018C7 RID: 6343 RVA: 0x00094104 File Offset: 0x00092304
		// (set) Token: 0x060018C8 RID: 6344 RVA: 0x0009410C File Offset: 0x0009230C
		public bool ErrorOutputVisible { get; set; }

		// Token: 0x1700075B RID: 1883
		// (get) Token: 0x060018C9 RID: 6345 RVA: 0x00094115 File Offset: 0x00092315
		// (set) Token: 0x060018CA RID: 6346 RVA: 0x0009411D File Offset: 0x0009231D
		public bool RejectButtonVisible { get; set; }

		// Token: 0x1700075C RID: 1884
		// (get) Token: 0x060018CB RID: 6347 RVA: 0x00094126 File Offset: 0x00092326
		// (set) Token: 0x060018CC RID: 6348 RVA: 0x0009412E File Offset: 0x0009232E
		public bool MoveXrayVisible { get; set; }

		// Token: 0x1700075D RID: 1885
		// (get) Token: 0x060018CD RID: 6349 RVA: 0x00094137 File Offset: 0x00092337
		// (set) Token: 0x060018CE RID: 6350 RVA: 0x0009413F File Offset: 0x0009233F
		public bool OldWagoConfiguration { get; set; }

		// Token: 0x1700075E RID: 1886
		// (get) Token: 0x060018CF RID: 6351 RVA: 0x00094148 File Offset: 0x00092348
		// (set) Token: 0x060018D0 RID: 6352 RVA: 0x00094150 File Offset: 0x00092350
		public bool BuzzerAvailable { get; set; }

		// Token: 0x060018D1 RID: 6353 RVA: 0x0009415C File Offset: 0x0009235C
		public CtrlOverviewSsh(DBConnection dbConnection, ParameterHandling parHandling, Logger logger, AccessRightsManager accessManager, Conveyor conveyor, ErrorHandling errorHandling, ProductManager productManager, MachineState machineState, MessageSystem messageSystem, DBDataLogs dBDataLogs)
		{
			bool flag = false;
			this.InitializeComponent();
			this.dbConnection = dbConnection;
			this.parHandling = parHandling;
			this.logger = logger;
			this.accessManager = accessManager;
			this.conveyor = conveyor;
			this.errorHandling = errorHandling;
			this.productManager = productManager;
			this.machineState = machineState;
			this.messageSystem = messageSystem;
			this.dBDataLogs = dBDataLogs;
			int.TryParse(parHandling.ReadPar(Definition.ioVersionPath), out this.ioVersion);
			this.OldWagoConfiguration = (this.ioVersion < Definition.minNewWagoConfigVersion);
			this.ErrorOutputVisible = (accessManager.CurrentUser.Rights >= this.changeErrorOutputsRight && (!this.OldWagoConfiguration || parHandling.ReadPar(Definition.ioSubversionPath) != "1"));
			this.RejectButtonVisible = (accessManager.CurrentUser.Rights >= this.rejecterMinOpenRight);
			this.MoveXrayVisible = (accessManager.CurrentUser.Rights >= this.moveSourceRight);
			dbConnection.GetBuzzerAvailable(ref flag);
			this.BuzzerAvailable = (flag && accessManager.CurrentUser.Rights >= this.rejecterMinOpenRight);
			base.DataContext = this;
			bool flag2 = productManager.CurrentLoadedProduct.ProductNumber.Equals("-1");
			if (flag2)
			{
				this.btnConveyor.IsEnabled = false;
			}
		}

		// Token: 0x060018D2 RID: 6354 RVA: 0x000942F0 File Offset: 0x000924F0
		private void btnDetector_Click(object sender, RoutedEventArgs e)
		{
			DetectorType detectorType = DetectorType.DT;
			this.dbConnection.GetDetectorType(ref detectorType);
			OverviewCommon overviewCommon = new OverviewCommon(this.parHandling, this.logger);
			overviewCommon.ShowDetectorWindow(detectorType);
		}

		// Token: 0x060018D3 RID: 6355 RVA: 0x00094328 File Offset: 0x00092528
		private void btnSource_Click(object sender, RoutedEventArgs e)
		{
			OverviewCommon overviewCommon = new OverviewCommon(this.parHandling, this.logger);
			overviewCommon.ShowXRaySourceWindow();
		}

		// Token: 0x060018D4 RID: 6356 RVA: 0x00094350 File Offset: 0x00092550
		private void btnSafetySwitch_Click(object sender, RoutedEventArgs e)
		{
			OverviewCommon overviewCommon = new OverviewCommon(this.parHandling, this.logger);
			overviewCommon.ShowSafetySwitchWindow(this.ioVersion, false);
		}

		// Token: 0x060018D5 RID: 6357 RVA: 0x00094380 File Offset: 0x00092580
		private void btnPusher_Click(object sender, RoutedEventArgs e)
		{
			CtrlRejecter usrControl = new CtrlRejecter(this.parHandling, this.dbConnection, this.accessManager, this.machineState, this.messageSystem);
			Subwindow.Show(usrControl, SubwindowButtons.BackAndHome);
		}

		// Token: 0x060018D6 RID: 6358 RVA: 0x000943BC File Offset: 0x000925BC
		private void btnTemperatures_Click(object sender, RoutedEventArgs e)
		{
			OverviewCommon overviewCommon = new OverviewCommon(this.parHandling, this.logger);
			overviewCommon.ShowTemperatureWindow(this.ioVersion);
		}

		// Token: 0x060018D7 RID: 6359 RVA: 0x000943EC File Offset: 0x000925EC
		private void btnVoltages_Click(object sender, RoutedEventArgs e)
		{
			OverviewCommon overviewCommon = new OverviewCommon(this.parHandling, this.logger);
			overviewCommon.ShowVoltageWindow();
		}

		// Token: 0x060018D8 RID: 6360 RVA: 0x00094414 File Offset: 0x00092614
		private void btnConveyor_Click(object sender, RoutedEventArgs e)
		{
			CtrlConveyor usrControl = new CtrlConveyor(this.conveyor, this.accessManager, this.dbConnection, this.machineState, this.dBDataLogs);
			Subwindow.Show(usrControl);
		}

		// Token: 0x060018D9 RID: 6361 RVA: 0x00094450 File Offset: 0x00092650
		private void btnErrorOutput_Click(object sender, RoutedEventArgs e)
		{
			CtrlErrorContact usrControl = new CtrlErrorContact(this.errorHandling, this.parHandling, this.dbConnection, this.accessManager);
			Subwindow.Show(usrControl, SubwindowButtons.BackAndHome);
		}

		// Token: 0x060018DA RID: 6362 RVA: 0x00094484 File Offset: 0x00092684
		private void btnMachineParameter_Click(object sender, RoutedEventArgs e)
		{
			CtrlMachineParameter usrControl = new CtrlMachineParameter(this.dbConnection, this.accessManager, this.parHandling);
			Subwindow.Show(usrControl, SubwindowButtons.BackAndHome);
		}

		// Token: 0x060018DB RID: 6363 RVA: 0x000944B4 File Offset: 0x000926B4
		private void btnMoveSource_Click(object sender, RoutedEventArgs e)
		{
			bool flag = this.machineState.State != MachineStates.Inspecting;
			if (flag)
			{
				Dialog.Show(LocalizationManager.Localize("txtInfo"), LocalizationManager.Localize("txtTurnXrayOn") + ".", DialogStyle.Info);
			}
			else
			{
				bool flag2 = Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtTurningOffProductInspection"), DialogStyle.Warning, DialogButtons.OKCancel) == Dialog.DialogResults.Ok;
				if (flag2)
				{
					CtrlMoveSource usrControl = new CtrlMoveSource(this.dbConnection, this.parHandling, this.machineState, this.accessManager);
					Subwindow.Show(usrControl, SubwindowButtons.BackAndHome);
				}
			}
		}

		// Token: 0x060018DC RID: 6364 RVA: 0x0009454C File Offset: 0x0009274C
		private void btnBuzzer_Click(object sender, RoutedEventArgs e)
		{
			CtrlBuzzer usrControl = new CtrlBuzzer(this.parHandling, this.dbConnection);
			Subwindow.Show(usrControl, SubwindowButtons.BackAndHome);
		}

		// Token: 0x060018DD RID: 6365 RVA: 0x00094574 File Offset: 0x00092774
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/overviews/ctrloverviewssh.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x060018DE RID: 6366 RVA: 0x000945AC File Offset: 0x000927AC
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
				this.btnMoveSource = (Button)target;
				this.btnMoveSource.Click += this.btnMoveSource_Click;
				break;
			case 7:
				this.btnTemperatures = (Button)target;
				this.btnTemperatures.Click += this.btnTemperatures_Click;
				break;
			case 8:
				this.btnVoltages = (Button)target;
				this.btnVoltages.Click += this.btnVoltages_Click;
				break;
			case 9:
				this.btnConveyor = (Button)target;
				this.btnConveyor.Click += this.btnConveyor_Click;
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

		// Token: 0x04001038 RID: 4152
		private BuiltInPriviledges rejecterMinOpenRight = BuiltInPriviledges.Operator1;

		// Token: 0x04001039 RID: 4153
		private BuiltInPriviledges changeErrorOutputsRight = BuiltInPriviledges.Operator1;

		// Token: 0x0400103A RID: 4154
		private BuiltInPriviledges moveSourceRight = BuiltInPriviledges.Operator1;

		// Token: 0x0400103B RID: 4155
		private Logger logger;

		// Token: 0x0400103C RID: 4156
		private AccessRightsManager accessManager;

		// Token: 0x0400103D RID: 4157
		private ParameterHandling parHandling;

		// Token: 0x0400103E RID: 4158
		private DBConnection dbConnection;

		// Token: 0x0400103F RID: 4159
		private DBDataLogs dBDataLogs;

		// Token: 0x04001040 RID: 4160
		private Conveyor conveyor;

		// Token: 0x04001041 RID: 4161
		private ErrorHandling errorHandling;

		// Token: 0x04001042 RID: 4162
		private ProductManager productManager;

		// Token: 0x04001043 RID: 4163
		private MachineState machineState;

		// Token: 0x04001044 RID: 4164
		private MessageSystem messageSystem;

		// Token: 0x04001045 RID: 4165
		private int ioVersion = 0;

		// Token: 0x0400104B RID: 4171
		internal Image overviewImage;

		// Token: 0x0400104C RID: 4172
		internal Button btnDetector;

		// Token: 0x0400104D RID: 4173
		internal Button btnPusher;

		// Token: 0x0400104E RID: 4174
		internal Button btnSafetySwitch;

		// Token: 0x0400104F RID: 4175
		internal Button btnSource;

		// Token: 0x04001050 RID: 4176
		internal Button btnMoveSource;

		// Token: 0x04001051 RID: 4177
		internal Button btnTemperatures;

		// Token: 0x04001052 RID: 4178
		internal Button btnVoltages;

		// Token: 0x04001053 RID: 4179
		internal Button btnConveyor;

		// Token: 0x04001054 RID: 4180
		internal Button btnErrorOutput;

		// Token: 0x04001055 RID: 4181
		internal Button btnMachineParameter;

		// Token: 0x04001056 RID: 4182
		private bool _contentLoaded;
	}
}
