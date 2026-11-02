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
	// Token: 0x02000092 RID: 146
	public class CtrlOverviewRayconDPlusHyRework : UserControl, IComponentConnector
	{
		// Token: 0x17000485 RID: 1157
		// (get) Token: 0x06000ED5 RID: 3797 RVA: 0x00059802 File Offset: 0x00057A02
		// (set) Token: 0x06000ED6 RID: 3798 RVA: 0x0005980A File Offset: 0x00057A0A
		public bool ErrorOutputVisible { get; set; }

		// Token: 0x17000486 RID: 1158
		// (get) Token: 0x06000ED7 RID: 3799 RVA: 0x00059813 File Offset: 0x00057A13
		// (set) Token: 0x06000ED8 RID: 3800 RVA: 0x0005981B File Offset: 0x00057A1B
		public bool RejectButtonVisible { get; set; }

		// Token: 0x17000487 RID: 1159
		// (get) Token: 0x06000ED9 RID: 3801 RVA: 0x00059824 File Offset: 0x00057A24
		// (set) Token: 0x06000EDA RID: 3802 RVA: 0x0005982C File Offset: 0x00057A2C
		public bool OldWagoConfiguration { get; set; }

		// Token: 0x17000488 RID: 1160
		// (get) Token: 0x06000EDB RID: 3803 RVA: 0x00059835 File Offset: 0x00057A35
		// (set) Token: 0x06000EDC RID: 3804 RVA: 0x0005983D File Offset: 0x00057A3D
		public bool BuzzerAvailable { get; set; }

		// Token: 0x17000489 RID: 1161
		// (get) Token: 0x06000EDD RID: 3805 RVA: 0x00059846 File Offset: 0x00057A46
		// (set) Token: 0x06000EDE RID: 3806 RVA: 0x0005984E File Offset: 0x00057A4E
		public MachineState MachineState { get; set; }

		// Token: 0x1700048A RID: 1162
		// (get) Token: 0x06000EDF RID: 3807 RVA: 0x00059857 File Offset: 0x00057A57
		// (set) Token: 0x06000EE0 RID: 3808 RVA: 0x0005985F File Offset: 0x00057A5F
		public bool DemoMode { get; set; }

		// Token: 0x1700048B RID: 1163
		// (get) Token: 0x06000EE1 RID: 3809 RVA: 0x00059868 File Offset: 0x00057A68
		// (set) Token: 0x06000EE2 RID: 3810 RVA: 0x00059870 File Offset: 0x00057A70
		public bool CommunicationsAvailable { get; set; }

		// Token: 0x06000EE3 RID: 3811 RVA: 0x0005987C File Offset: 0x00057A7C
		public CtrlOverviewRayconDPlusHyRework(DBConnection dbConnection, ParameterHandling parHandling, Logger logger, AccessRightsManager accessManager, Conveyor conveyor, ErrorHandling errorHandling, ProductManager productManager, MachineState machineState, MessageSystem messageSystem, List<object> communicationsList, DBDataLogs dBDataLogs)
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

		// Token: 0x06000EE4 RID: 3812 RVA: 0x00059AD4 File Offset: 0x00057CD4
		private void btnDetector_Click(object sender, RoutedEventArgs e)
		{
			DetectorType detectorType = DetectorType.DT;
			this.dbConnection.GetDetectorType(ref detectorType);
			OverviewCommon overviewCommon = new OverviewCommon(this.parHandling, this.logger);
			overviewCommon.ShowDetectorWindow(detectorType);
		}

		// Token: 0x06000EE5 RID: 3813 RVA: 0x00059B0C File Offset: 0x00057D0C
		private void btnSource_Click(object sender, RoutedEventArgs e)
		{
			OverviewCommon overviewCommon = new OverviewCommon(this.parHandling, this.logger);
			overviewCommon.ShowXRaySourceWindow();
		}

		// Token: 0x06000EE6 RID: 3814 RVA: 0x00059B34 File Offset: 0x00057D34
		private void btnSafetySwitch_Click(object sender, RoutedEventArgs e)
		{
			OverviewCommon overviewCommon = new OverviewCommon(this.parHandling, this.logger);
			overviewCommon.ShowSafetySwitchWindow(this.ioVersion, false);
		}

		// Token: 0x06000EE7 RID: 3815 RVA: 0x00059B64 File Offset: 0x00057D64
		private void btnPusher_Click(object sender, RoutedEventArgs e)
		{
			CtrlRejecter usrControl = new CtrlRejecter(this.parHandling, this.dbConnection, this.accessManager, this.MachineState, this.messageSystem);
			Subwindow.Show(usrControl, SubwindowButtons.BackAndHome);
		}

		// Token: 0x06000EE8 RID: 3816 RVA: 0x00059BA0 File Offset: 0x00057DA0
		private void btnTemperatures_Click(object sender, RoutedEventArgs e)
		{
			OverviewCommon overviewCommon = new OverviewCommon(this.parHandling, this.logger);
			overviewCommon.ShowTemperatureWindow(this.ioVersion);
		}

		// Token: 0x06000EE9 RID: 3817 RVA: 0x00059BD0 File Offset: 0x00057DD0
		private void btnConveyor_Click(object sender, RoutedEventArgs e)
		{
			CtrlConveyor usrControl = new CtrlConveyor(this.conveyor, this.accessManager, this.dbConnection, this.MachineState, this.dbDatalogs);
			Subwindow.Show(usrControl);
		}

		// Token: 0x06000EEA RID: 3818 RVA: 0x00059C0C File Offset: 0x00057E0C
		private void btnErrorOutput_Click(object sender, RoutedEventArgs e)
		{
			CtrlErrorContact usrControl = new CtrlErrorContact(this.errorHandling, this.parHandling, this.dbConnection, this.accessManager);
			Subwindow.Show(usrControl, SubwindowButtons.BackAndHome);
		}

		// Token: 0x06000EEB RID: 3819 RVA: 0x00059C40 File Offset: 0x00057E40
		private void btnMachineParameter_Click(object sender, RoutedEventArgs e)
		{
			CtrlMachineParameter usrControl = new CtrlMachineParameter(this.dbConnection, this.accessManager, this.parHandling);
			Subwindow.Show(usrControl, SubwindowButtons.BackAndHome);
		}

		// Token: 0x06000EEC RID: 3820 RVA: 0x00059C70 File Offset: 0x00057E70
		private void btnBuzzer_Click(object sender, RoutedEventArgs e)
		{
			CtrlBuzzer usrControl = new CtrlBuzzer(this.parHandling, this.dbConnection);
			Subwindow.Show(usrControl, SubwindowButtons.BackAndHome);
		}

		// Token: 0x06000EED RID: 3821 RVA: 0x00059C98 File Offset: 0x00057E98
		private void btnCommunications_Click(object sender, RoutedEventArgs e)
		{
			SubMenu usrControl = new SubMenu(this.dbConnection, this.accessManager, this.MachineState, this.communicationsList);
			Subwindow.Show(usrControl, SubwindowButtons.BackAndHome);
		}

		// Token: 0x06000EEE RID: 3822 RVA: 0x0001092C File Offset: 0x0000EB2C
		private void Image_SizeChanged(object sender, SizeChangedEventArgs e)
		{
		}

		// Token: 0x06000EEF RID: 3823 RVA: 0x00059CCC File Offset: 0x00057ECC
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/overviews/ctrloverviewraycondplushyrework.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x06000EF0 RID: 3824 RVA: 0x00059D04 File Offset: 0x00057F04
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

		// Token: 0x040009D3 RID: 2515
		private Logger logger;

		// Token: 0x040009D4 RID: 2516
		private AccessRightsManager accessManager;

		// Token: 0x040009D5 RID: 2517
		private ParameterHandling parHandling;

		// Token: 0x040009D6 RID: 2518
		private DBConnection dbConnection;

		// Token: 0x040009D7 RID: 2519
		private DBDataLogs dbDatalogs;

		// Token: 0x040009D8 RID: 2520
		private Conveyor conveyor;

		// Token: 0x040009D9 RID: 2521
		private ErrorHandling errorHandling;

		// Token: 0x040009DA RID: 2522
		private ProductManager productManager;

		// Token: 0x040009DB RID: 2523
		private MessageSystem messageSystem;

		// Token: 0x040009DC RID: 2524
		private List<object> communicationsList;

		// Token: 0x040009DD RID: 2525
		private int ioVersion = 0;

		// Token: 0x040009E5 RID: 2533
		internal Image overviewImage;

		// Token: 0x040009E6 RID: 2534
		internal Button btnDetector;

		// Token: 0x040009E7 RID: 2535
		internal Button btnPusher;

		// Token: 0x040009E8 RID: 2536
		internal Button btnSafetySwitch;

		// Token: 0x040009E9 RID: 2537
		internal Button btnSource;

		// Token: 0x040009EA RID: 2538
		internal Button btnTemperatures;

		// Token: 0x040009EB RID: 2539
		internal Button btnConveyor;

		// Token: 0x040009EC RID: 2540
		internal Button btnErrorOutput;

		// Token: 0x040009ED RID: 2541
		internal Button btnMachineParameter;

		// Token: 0x040009EE RID: 2542
		internal Button btnConnectioins;

		// Token: 0x040009EF RID: 2543
		private bool _contentLoaded;
	}
}
