using System;
using System.Collections.ObjectModel;
using Common;
using DatabaseConnection;
using Definitions;
using GUI_XRS.Datalogger;

namespace GUI_XRS
{
	// Token: 0x02000096 RID: 150
	public class SubMenuMonitoringViewModel
	{
		// Token: 0x170004A9 RID: 1193
		// (get) Token: 0x06000F48 RID: 3912 RVA: 0x0005B3FF File Offset: 0x000595FF
		// (set) Token: 0x06000F49 RID: 3913 RVA: 0x0005B407 File Offset: 0x00059607
		public ObservableCollection<SubMenuItem> SubMenuItems { get; set; }

		// Token: 0x170004AA RID: 1194
		// (get) Token: 0x06000F4A RID: 3914 RVA: 0x0005B410 File Offset: 0x00059610
		// (set) Token: 0x06000F4B RID: 3915 RVA: 0x0005B418 File Offset: 0x00059618
		public RelayCommand CloseCommand { get; set; }

		// Token: 0x170004AB RID: 1195
		// (get) Token: 0x06000F4C RID: 3916 RVA: 0x0005B421 File Offset: 0x00059621
		// (set) Token: 0x06000F4D RID: 3917 RVA: 0x0005B429 File Offset: 0x00059629
		public ParameterHandling parameterHandling { get; set; }

		// Token: 0x170004AC RID: 1196
		// (get) Token: 0x06000F4E RID: 3918 RVA: 0x0005B432 File Offset: 0x00059632
		// (set) Token: 0x06000F4F RID: 3919 RVA: 0x0005B43A File Offset: 0x0005963A
		public MachineState MachineState { get; set; }

		// Token: 0x170004AD RID: 1197
		// (get) Token: 0x06000F50 RID: 3920 RVA: 0x0005B443 File Offset: 0x00059643
		// (set) Token: 0x06000F51 RID: 3921 RVA: 0x0005B44B File Offset: 0x0005964B
		public bool DemoMode { get; set; }

		// Token: 0x06000F52 RID: 3922 RVA: 0x0005B454 File Offset: 0x00059654
		public SubMenuMonitoringViewModel(ParameterHandling parHandling, DBConnection dbConnection, AccessRightsManager accessManager, MachineState machineState, DBDataLogs dBDataLogs, ErrorSurvey errorSurvey, MainWindowViewModel mainWindow)
		{
			this.accessManager = accessManager;
			this.parameterHandling = parHandling;
			this.dBConnection = dbConnection;
			this.MachineState = machineState;
			this.dBDataLogs = dBDataLogs;
			this.errorSurvey = errorSurvey;
			this.mainWindow = mainWindow;
			bool demoMode = false;
			dbConnection.GetDemoMode(ref demoMode);
			this.DemoMode = demoMode;
			this.CloseCommand = new RelayCommand(new Action<object>(this.CloseSubMenu));
			this.RegisterLocalizedText();
			this.InitSubMenus();
		}

		// Token: 0x06000F53 RID: 3923 RVA: 0x0005B4DC File Offset: 0x000596DC
		private void InitSubMenus()
		{
			this.SubMenuItems = new ObservableCollection<SubMenuItem>();
			SubMenuItem item = new SubMenuItem("Data Logger", "txtLogData", "..\\..\\img\\dataplotter.png", "..\\..\\img\\settings.png", new Action<object>(this.btnDataloggerAutomatic_Click), this.accessManager, BuiltInPriviledges.Admin, BuiltInPriviledges.Admin);
			this.SubMenuItems.Add(item);
			SubMenuItem item2 = new SubMenuItem("Betriebsparameter", "txtDeviceData", "..\\..\\img\\operating_parameters_monitoring.png", "..\\..\\img\\operating_parameters_monitoring.png", new Action<object>(this.btnOperatingParameters_Click), this.accessManager, BuiltInPriviledges.Operator3, BuiltInPriviledges.Operator3);
			this.SubMenuItems.Add(item2);
			SubMenuItem item3 = new SubMenuItem("Röntgenquelle", "parItemXRaySource", "..\\..\\img\\xray_monitoring.png", "..\\..\\img\\xray_monitoring.png", new Action<object>(this.btnXRayParameters_Click), this.accessManager, BuiltInPriviledges.Operator3, BuiltInPriviledges.Operator3);
			this.SubMenuItems.Add(item3);
			SubMenuItem item4 = new SubMenuItem("Detektor", "parItemDetector", "..\\..\\img\\detector_monitoring.png", "..\\..\\img\\detector_monitoring.png", new Action<object>(this.btnDetectorParameters_Click), this.accessManager, BuiltInPriviledges.Operator3, BuiltInPriviledges.Operator3);
			this.SubMenuItems.Add(item4);
			SubMenuItem item5 = new SubMenuItem("Förderband", "txtConveyor", "..\\..\\img\\conveyor_monitoring.png", "..\\..\\img\\conveyor_monitoring.png", new Action<object>(this.btnConveyorParameters_Click), this.accessManager, BuiltInPriviledges.Operator3, BuiltInPriviledges.Operator3);
			this.SubMenuItems.Add(item5);
			SubMenuItem item6 = new SubMenuItem("Pusher", "txtRejecter", "..\\..\\img\\pusher_monitoring.png", "..\\..\\img\\pusher_monitoring.png", new Action<object>(this.btnRejecterParameters_Click), this.accessManager, BuiltInPriviledges.Operator3, BuiltInPriviledges.Operator3);
			this.SubMenuItems.Add(item6);
			SubMenuItem item7 = new SubMenuItem("Einstellungen", "txtSettings", "..\\..\\img\\settings.png", "..\\..\\img\\settings.png", new Action<object>(this.btnSettings_Click), this.accessManager, BuiltInPriviledges.Admin, BuiltInPriviledges.Admin);
			this.SubMenuItems.Add(item7);
		}

		// Token: 0x06000F54 RID: 3924 RVA: 0x0005B6D4 File Offset: 0x000598D4
		private void btnOperatingParameters_Click(object sender)
		{
			MonitoringView usrControl = new MonitoringView(this.accessManager, DataModesMonitoring.operational, this.dBDataLogs, this.dBConnection, this.parameterHandling, this.errorSurvey, this.mainWindow);
			Subwindow.Show(usrControl, SubwindowButtons.BackAndHome);
		}

		// Token: 0x06000F55 RID: 3925 RVA: 0x0005B718 File Offset: 0x00059918
		private void btnXRayParameters_Click(object sender)
		{
			MonitoringView usrControl = new MonitoringView(this.accessManager, DataModesMonitoring.xray, this.dBDataLogs, this.dBConnection, this.parameterHandling, this.errorSurvey, this.mainWindow);
			Subwindow.Show(usrControl, SubwindowButtons.BackAndHome);
		}

		// Token: 0x06000F56 RID: 3926 RVA: 0x0005B75C File Offset: 0x0005995C
		private void btnDetectorParameters_Click(object sender)
		{
			MonitoringView usrControl = new MonitoringView(this.accessManager, DataModesMonitoring.detector, this.dBDataLogs, this.dBConnection, this.parameterHandling, this.errorSurvey, this.mainWindow);
			Subwindow.Show(usrControl, SubwindowButtons.BackAndHome);
		}

		// Token: 0x06000F57 RID: 3927 RVA: 0x0005B7A0 File Offset: 0x000599A0
		private void btnConveyorParameters_Click(object sender)
		{
			MonitoringView usrControl = new MonitoringView(this.accessManager, DataModesMonitoring.conveyor, this.dBDataLogs, this.dBConnection, this.parameterHandling, this.errorSurvey, this.mainWindow);
			Subwindow.Show(usrControl, SubwindowButtons.BackAndHome);
		}

		// Token: 0x06000F58 RID: 3928 RVA: 0x0005B7E4 File Offset: 0x000599E4
		private void btnRejecterParameters_Click(object sender)
		{
			MonitoringView usrControl = new MonitoringView(this.accessManager, DataModesMonitoring.rejecter, this.dBDataLogs, this.dBConnection, this.parameterHandling, this.errorSurvey, this.mainWindow);
			Subwindow.Show(usrControl, SubwindowButtons.BackAndHome);
		}

		// Token: 0x06000F59 RID: 3929 RVA: 0x0005B828 File Offset: 0x00059A28
		private void btnSettings_Click(object sender)
		{
			CtrlSettingsMonitoring usrControl = new CtrlSettingsMonitoring(this.dBDataLogs, this.parameterHandling, this.dBConnection);
			Subwindow.Show(usrControl, SubwindowButtons.BackAndHome);
		}

		// Token: 0x06000F5A RID: 3930 RVA: 0x0005B858 File Offset: 0x00059A58
		private void btnDataloggerAutomatic_Click(object obj)
		{
			CtrlDatalogsPredMain ctrlDatalogsPredMain = new CtrlDatalogsPredMain(this.dBDataLogs, this.dBConnection, this.parameterHandling);
			Subwindow.Show(ctrlDatalogsPredMain, SubwindowButtons.BackAndHome, ctrlDatalogsPredMain.PanelButtons);
		}

		// Token: 0x06000F5B RID: 3931 RVA: 0x0001092C File Offset: 0x0000EB2C
		private void RegisterLocalizedText()
		{
		}

		// Token: 0x06000F5C RID: 3932 RVA: 0x0005B88C File Offset: 0x00059A8C
		private void CloseSubMenu(object obj)
		{
			foreach (SubMenuItem subMenuItem in this.SubMenuItems)
			{
				subMenuItem.DeregisterLocalizedText();
			}
		}

		// Token: 0x04000A3B RID: 2619
		private AccessRightsManager accessManager;

		// Token: 0x04000A3E RID: 2622
		public ProductManager productManager;

		// Token: 0x04000A3F RID: 2623
		private DBConnection dBConnection;

		// Token: 0x04000A42 RID: 2626
		private DBDataLogs dBDataLogs;

		// Token: 0x04000A43 RID: 2627
		private ErrorSurvey errorSurvey;

		// Token: 0x04000A44 RID: 2628
		private MainWindowViewModel mainWindow;
	}
}
