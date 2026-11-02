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
	// Token: 0x02000091 RID: 145
	public class CtrlOverviewEX2A : UserControl, IComponentConnector
	{
		// Token: 0x1700047E RID: 1150
		// (get) Token: 0x06000EBA RID: 3770 RVA: 0x0005918C File Offset: 0x0005738C
		// (set) Token: 0x06000EBB RID: 3771 RVA: 0x00059194 File Offset: 0x00057394
		public bool ErrorOutputVisible { get; set; }

		// Token: 0x1700047F RID: 1151
		// (get) Token: 0x06000EBC RID: 3772 RVA: 0x0005919D File Offset: 0x0005739D
		// (set) Token: 0x06000EBD RID: 3773 RVA: 0x000591A5 File Offset: 0x000573A5
		public bool RejectButtonVisible { get; set; }

		// Token: 0x17000480 RID: 1152
		// (get) Token: 0x06000EBE RID: 3774 RVA: 0x000591AE File Offset: 0x000573AE
		// (set) Token: 0x06000EBF RID: 3775 RVA: 0x000591B6 File Offset: 0x000573B6
		public bool OldWagoConfiguration { get; set; }

		// Token: 0x17000481 RID: 1153
		// (get) Token: 0x06000EC0 RID: 3776 RVA: 0x000591BF File Offset: 0x000573BF
		// (set) Token: 0x06000EC1 RID: 3777 RVA: 0x000591C7 File Offset: 0x000573C7
		public bool BuzzerAvailable { get; set; }

		// Token: 0x17000482 RID: 1154
		// (get) Token: 0x06000EC2 RID: 3778 RVA: 0x000591D0 File Offset: 0x000573D0
		// (set) Token: 0x06000EC3 RID: 3779 RVA: 0x000591D8 File Offset: 0x000573D8
		public MachineState MachineState { get; set; }

		// Token: 0x17000483 RID: 1155
		// (get) Token: 0x06000EC4 RID: 3780 RVA: 0x000591E1 File Offset: 0x000573E1
		// (set) Token: 0x06000EC5 RID: 3781 RVA: 0x000591E9 File Offset: 0x000573E9
		public bool DemoMode { get; set; }

		// Token: 0x17000484 RID: 1156
		// (get) Token: 0x06000EC6 RID: 3782 RVA: 0x000591F2 File Offset: 0x000573F2
		// (set) Token: 0x06000EC7 RID: 3783 RVA: 0x000591FA File Offset: 0x000573FA
		public bool CommunicationsAvailable { get; set; }

		// Token: 0x06000EC8 RID: 3784 RVA: 0x00059204 File Offset: 0x00057404
		public CtrlOverviewEX2A(DBConnection dbConnection, ParameterHandling parHandling, Logger logger, AccessRightsManager accessManager, Conveyor conveyor, ErrorHandling errorHandling, ProductManager productManager, MachineState machineState, MessageSystem messageSystem, List<object> communicationsList, DBDataLogs dbDatalogs)
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

		// Token: 0x06000EC9 RID: 3785 RVA: 0x0005945C File Offset: 0x0005765C
		private void btnDetector_Click(object sender, RoutedEventArgs e)
		{
			DetectorType detectorType = DetectorType.DT;
			this.dbConnection.GetDetectorType(ref detectorType);
			OverviewCommon overviewCommon = new OverviewCommon(this.parHandling, this.logger);
			overviewCommon.ShowDetectorWindow(detectorType);
		}

		// Token: 0x06000ECA RID: 3786 RVA: 0x00059494 File Offset: 0x00057694
		private void btnSource_Click(object sender, RoutedEventArgs e)
		{
			OverviewCommon overviewCommon = new OverviewCommon(this.parHandling, this.logger);
			overviewCommon.ShowXRaySourceWindow();
		}

		// Token: 0x06000ECB RID: 3787 RVA: 0x000594BC File Offset: 0x000576BC
		private void btnSafetySwitch_Click(object sender, RoutedEventArgs e)
		{
			OverviewCommon overviewCommon = new OverviewCommon(this.parHandling, this.logger);
			overviewCommon.ShowSafetySwitchWindow(this.ioVersion, false);
		}

		// Token: 0x06000ECC RID: 3788 RVA: 0x000594EC File Offset: 0x000576EC
		private void btnPusher_Click(object sender, RoutedEventArgs e)
		{
			CtrlRejecter usrControl = new CtrlRejecter(this.parHandling, this.dbConnection, this.accessManager, this.MachineState, this.messageSystem);
			Subwindow.Show(usrControl, SubwindowButtons.BackAndHome);
		}

		// Token: 0x06000ECD RID: 3789 RVA: 0x00059528 File Offset: 0x00057728
		private void btnTemperatures_Click(object sender, RoutedEventArgs e)
		{
			OverviewCommon overviewCommon = new OverviewCommon(this.parHandling, this.logger);
			overviewCommon.ShowTemperatureWindow(this.ioVersion);
		}

		// Token: 0x06000ECE RID: 3790 RVA: 0x00059558 File Offset: 0x00057758
		private void btnConveyor_Click(object sender, RoutedEventArgs e)
		{
			CtrlConveyor usrControl = new CtrlConveyor(this.conveyor, this.accessManager, this.dbConnection, this.MachineState, this.dbDatalogs);
			Subwindow.Show(usrControl);
		}

		// Token: 0x06000ECF RID: 3791 RVA: 0x00059594 File Offset: 0x00057794
		private void btnErrorOutput_Click(object sender, RoutedEventArgs e)
		{
			CtrlErrorContact usrControl = new CtrlErrorContact(this.errorHandling, this.parHandling, this.dbConnection, this.accessManager);
			Subwindow.Show(usrControl, SubwindowButtons.BackAndHome);
		}

		// Token: 0x06000ED0 RID: 3792 RVA: 0x000595C8 File Offset: 0x000577C8
		private void btnMachineParameter_Click(object sender, RoutedEventArgs e)
		{
			CtrlMachineParameter usrControl = new CtrlMachineParameter(this.dbConnection, this.accessManager, this.parHandling);
			Subwindow.Show(usrControl, SubwindowButtons.BackAndHome);
		}

		// Token: 0x06000ED1 RID: 3793 RVA: 0x000595F8 File Offset: 0x000577F8
		private void btnBuzzer_Click(object sender, RoutedEventArgs e)
		{
			bool flag = false;
			bool buzzerAvailable = this.dbConnection.GetBuzzerAvailable(ref flag);
			if (buzzerAvailable)
			{
				CtrlBuzzer usrControl = new CtrlBuzzer(this.parHandling, this.dbConnection);
				Subwindow.Show(usrControl, SubwindowButtons.BackAndHome);
			}
		}

		// Token: 0x06000ED2 RID: 3794 RVA: 0x00059638 File Offset: 0x00057838
		private void btnCommunications_Click(object sender, RoutedEventArgs e)
		{
			SubMenu usrControl = new SubMenu(this.dbConnection, this.accessManager, this.MachineState, this.communicationsList);
			Subwindow.Show(usrControl, SubwindowButtons.BackAndHome);
		}

		// Token: 0x06000ED3 RID: 3795 RVA: 0x0005966C File Offset: 0x0005786C
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/overviews/ctrloverviewex2a.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x06000ED4 RID: 3796 RVA: 0x000596A4 File Offset: 0x000578A4
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
				this.btnSource = (Button)target;
				this.btnSource.Click += this.btnSource_Click;
				break;
			case 5:
				this.btnConveyor = (Button)target;
				this.btnConveyor.Click += this.btnConveyor_Click;
				break;
			case 6:
				this.btnErrorOutput = (Button)target;
				this.btnErrorOutput.Click += this.btnErrorOutput_Click;
				break;
			case 7:
				this.btnMachineParameter = (Button)target;
				this.btnMachineParameter.Click += this.btnMachineParameter_Click;
				break;
			case 8:
				((Button)target).Click += this.btnBuzzer_Click;
				break;
			default:
				this._contentLoaded = true;
				break;
			}
		}

		// Token: 0x040009B9 RID: 2489
		private Logger logger;

		// Token: 0x040009BA RID: 2490
		private AccessRightsManager accessManager;

		// Token: 0x040009BB RID: 2491
		private ParameterHandling parHandling;

		// Token: 0x040009BC RID: 2492
		private DBConnection dbConnection;

		// Token: 0x040009BD RID: 2493
		private DBDataLogs dbDatalogs;

		// Token: 0x040009BE RID: 2494
		private Conveyor conveyor;

		// Token: 0x040009BF RID: 2495
		private ErrorHandling errorHandling;

		// Token: 0x040009C0 RID: 2496
		private ProductManager productManager;

		// Token: 0x040009C1 RID: 2497
		private MessageSystem messageSystem;

		// Token: 0x040009C2 RID: 2498
		private List<object> communicationsList;

		// Token: 0x040009C3 RID: 2499
		private int ioVersion = 0;

		// Token: 0x040009CB RID: 2507
		internal Image overviewImage;

		// Token: 0x040009CC RID: 2508
		internal Button btnDetector;

		// Token: 0x040009CD RID: 2509
		internal Button btnPusher;

		// Token: 0x040009CE RID: 2510
		internal Button btnSource;

		// Token: 0x040009CF RID: 2511
		internal Button btnConveyor;

		// Token: 0x040009D0 RID: 2512
		internal Button btnErrorOutput;

		// Token: 0x040009D1 RID: 2513
		internal Button btnMachineParameter;

		// Token: 0x040009D2 RID: 2514
		private bool _contentLoaded;
	}
}
