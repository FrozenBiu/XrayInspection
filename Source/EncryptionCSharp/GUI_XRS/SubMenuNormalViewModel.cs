using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using Common;
using DatabaseConnection;
using Definitions;
using GUI_XRS.Datalogger;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x02000188 RID: 392
	public class SubMenuNormalViewModel
	{
		// Token: 0x140000C1 RID: 193
		// (add) Token: 0x060024E4 RID: 9444 RVA: 0x000FD670 File Offset: 0x000FB870
		// (remove) Token: 0x060024E5 RID: 9445 RVA: 0x000FD6A8 File Offset: 0x000FB8A8
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action GraphPlotInitialized;

		// Token: 0x140000C2 RID: 194
		// (add) Token: 0x060024E6 RID: 9446 RVA: 0x000FD6E0 File Offset: 0x000FB8E0
		// (remove) Token: 0x060024E7 RID: 9447 RVA: 0x000FD718 File Offset: 0x000FB918
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<MachineStates> GraphPlotUnloaded;

		// Token: 0x17000A83 RID: 2691
		// (get) Token: 0x060024E8 RID: 9448 RVA: 0x000FD74D File Offset: 0x000FB94D
		// (set) Token: 0x060024E9 RID: 9449 RVA: 0x000FD755 File Offset: 0x000FB955
		public ObservableCollection<SubMenuItem> SubMenuItems { get; set; }

		// Token: 0x17000A84 RID: 2692
		// (get) Token: 0x060024EA RID: 9450 RVA: 0x000FD75E File Offset: 0x000FB95E
		// (set) Token: 0x060024EB RID: 9451 RVA: 0x000FD766 File Offset: 0x000FB966
		public RelayCommand CloseCommand { get; set; }

		// Token: 0x17000A85 RID: 2693
		// (get) Token: 0x060024EC RID: 9452 RVA: 0x000FD76F File Offset: 0x000FB96F
		// (set) Token: 0x060024ED RID: 9453 RVA: 0x000FD777 File Offset: 0x000FB977
		public MachineState MachineState { get; set; }

		// Token: 0x17000A86 RID: 2694
		// (get) Token: 0x060024EE RID: 9454 RVA: 0x000FD780 File Offset: 0x000FB980
		// (set) Token: 0x060024EF RID: 9455 RVA: 0x000FD788 File Offset: 0x000FB988
		public bool DemoMode { get; set; }

		// Token: 0x060024F0 RID: 9456 RVA: 0x000FD794 File Offset: 0x000FB994
		public SubMenuNormalViewModel(ParameterHandling parHandling, ImageDataTransfer imageTransfer, Logger logger, DBConnection dbConnection, MachineState machineState, AccessRightsManager accessManager, Conveyor conveyor, AuditCheckSettings auditCheckSettings, Restore restore, Backup backup, Branding selectedBranding, ProductManager productManager, Logbook logbook, ErrorHandling errorHandling, StoreImageData imageArchiver, WifiClient wifiClient, Vibro vibro, MessageSystem messageSystem, List<object> communicationsList, TestcardSimService testcardSimService, SetFooterMainWindow setFooterMainWindow, DBDataLogs dbDatalogs, ErrorSurvey errorSurvey, MainWindowViewModel mainWindow, ImageDataTransfer imageDataTransfer, AIService aiService, XrayState xrayState)
		{
			this.ParameterHandling = parHandling;
			this.ImageTransfer = imageTransfer;
			this.logger = logger;
			this.dbConnection = dbConnection;
			this.MachineState = machineState;
			this.accessManager = accessManager;
			this.conveyor = conveyor;
			this.auditCheckSettings = auditCheckSettings;
			this.restore = restore;
			this.backup = backup;
			this.selectedBranding = selectedBranding;
			this.productManager = productManager;
			this.logbook = logbook;
			this.errorHandling = errorHandling;
			this.imageArchiver = imageArchiver;
			this.wifiClient = wifiClient;
			this.vibro = vibro;
			this.messageSystem = messageSystem;
			this.communicationsList = communicationsList;
			this.testcardSimService = testcardSimService;
			this.setFooterMainWindow = setFooterMainWindow;
			this.dbDatalogs = dbDatalogs;
			this.errorSurvey = errorSurvey;
			this.mainWindow = mainWindow;
			this.imageDataTransfer = imageDataTransfer;
			this.aiService = aiService;
			this.xrayState = xrayState;
			this.CloseCommand = new RelayCommand(new Action<object>(this.CloseSubMenu));
			this.InitializeSubMenuItems();
			parHandling.ParameterChanged += this.parHandling_ParameterChanged;
		}

		// Token: 0x060024F1 RID: 9457 RVA: 0x000FD9D0 File Offset: 0x000FBBD0
		private void InitializeSubMenuItems()
		{
			MachineMode machineMode = MachineMode.Standard;
			MachineType machineType = MachineType.MachineTypeUndefined;
			DetectorType detectorType = DetectorType.DT;
			bool demoMode = false;
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = false;
			this.dbConnection.GetMachineMode(ref machineMode);
			this.dbConnection.GetMachineType(ref machineType);
			this.dbConnection.GetDetectorType(ref detectorType);
			this.dbConnection.GetDemoMode(ref demoMode);
			this.dbConnection.GetTestcardSimulation(ref flag);
			this.dbConnection.GetIntegratedValidationProcess(ref flag2);
			this.dbConnection.GetAnomalyDetectionEnabled(ref flag3);
			this.dbConnection.GetObjectDetectionEnabled(ref flag4);
			string a = this.ParameterHandling.ReadPar(Definition.imgprocarchitecture);
			bool flag5 = a == "1";
			this.DemoMode = demoMode;
			bool flag6 = this.SubMenuItems == null;
			if (flag6)
			{
				this.SubMenuItems = new ObservableCollection<SubMenuItem>();
			}
			else
			{
				this.SubMenuItems.Clear();
			}
			SubMenuItem item = new SubMenuItem("Übersicht", "menuItemOverview", "..\\..\\img\\overview.png", new Action<object>(this.btnOverview_Click), this.accessManager, this.rightOverview, this.rightOverview);
			this.SubMenuItems.Add(item);
			bool flag7 = !this.productManager.CurrentLoadedProduct.ProductNumber.Equals("-1");
			if (flag7)
			{
				SubMenuItem item2 = new SubMenuItem("Filter", "menuItemFilter", "..\\..\\img\\productparameter.png", new Action<object>(this.btnFilter_Click), this.accessManager, this.rightFilter, this.rightFilter);
				this.SubMenuItems.Add(item2);
			}
			SubMenuItem item3 = new SubMenuItem("Acquisition", "menuItemAcquisition", "..\\..\\img\\objectxriimage.png", new Action<object>(this.btnAcquisition_Click), this.accessManager, this.rightAcquisition, this.rightAcquisition);
			this.SubMenuItems.Add(item3);
			bool flag8 = machineMode != MachineMode.Bulk;
			if (flag8)
			{
				int num = 0;
				this.dbConnection.GetAuditCheckType(ref num);
				bool flag9 = num == 0;
				if (flag9)
				{
					SubMenuItem item4 = new SubMenuItem("Audit Check", "menuItemAuditCheck", "..\\..\\img\\auditCheck.png", new Action<object>(this.btnAuditChange_Click), this.accessManager, this.rightAuditCheck, this.rightAuditCheck);
					this.SubMenuItems.Add(item4);
				}
				else
				{
					SubMenuItem item5 = new SubMenuItem("Audit Check", "menuItemAuditCheck", "..\\..\\img\\Auditcheck_new.png", new Action<object>(this.btnAuditChange_Click2), this.accessManager, this.rightAuditCheckNew, this.rightAuditCheckNew);
					this.SubMenuItems.Add(item5);
				}
				int num2 = 0;
				string text = this.ParameterHandling.ReadPar(Definition.ioVersionPath);
				int.TryParse(text, out num2);
				bool flag10 = num2 >= Definition.minWagoStopBeltIfSwitchOpenVersion || text == "";
				if (flag10)
				{
					bool flag11 = false;
					this.dbConnection.GetBeltStartIfSwitchOpen(ref flag11);
					bool flag12 = !flag11 && machineType != MachineType.RayconEX2A;
					if (flag12)
					{
						this.itSettingMode = new SubMenuItem("Setting mode", "menuItemSettingMode", "\\..\\..\\img\\settingMode.png", new Action<object>(this.btnSettingMode_Click), this.accessManager, this.rightFilter, this.rightFilter);
						this.SubMenuItems.Add(this.itSettingMode);
					}
				}
			}
			else
			{
				this.itSettingMode = new SubMenuItem("Setting mode", "menuItemSettingMode", "\\..\\..\\img\\settingMode.png", new Action<object>(this.btnSettingMode_Click), this.accessManager, this.rightFilter, this.rightFilter);
				this.SubMenuItems.Add(this.itSettingMode);
				int num3 = 0;
				this.dbConnection.GetAuditCheckType(ref num3);
				bool flag13 = num3 == 1;
				if (flag13)
				{
					SubMenuItem item6 = new SubMenuItem("Audit Check", "menuItemAuditCheck", "..\\..\\img\\Auditcheck_new.png", new Action<object>(this.btnAuditChange_Click2), this.accessManager, this.rightAuditCheckNew, this.rightAuditCheckNew);
					this.SubMenuItems.Add(item6);
				}
			}
			SubMenuItem item7 = new SubMenuItem("Parameter", "menuItemParameter", "..\\..\\img\\settings.png", new Action<object>(this.btnParameter_Click), this.accessManager, this.rightParameter, this.rightParameter);
			this.SubMenuItems.Add(item7);
			SubMenuItem item8 = new SubMenuItem("Plot", "menuItemPlot", "..\\..\\img\\graphplot.png", "..\\..\\img\\graphplot_disabled.png", new Action<object>(this.btnPlot_Click), this.accessManager, this.rightPlot, this.rightPlot);
			this.SubMenuItems.Add(item8);
			SubMenuItem item9 = new SubMenuItem("Simulieren", "menuItemSimulate", "..\\..\\img\\simulateimage.png", new Action<object>(this.btnSend_Click), this.accessManager, this.rightSimulate, this.rightSimulate);
			this.SubMenuItems.Add(item9);
			SubMenuItem item10 = new SubMenuItem("Anzeigen", "menuItemShowImage", "..\\..\\img\\viewimage.png", new Action<object>(this.btnShow_Click), this.accessManager, this.rightShow, this.rightShow);
			this.SubMenuItems.Add(item10);
			bool flag14 = detectorType == DetectorType.DTNetDE || this.DemoMode;
			if (flag14)
			{
				SubMenuItem item11 = new SubMenuItem("Dual", "menuItemDual", "..\\..\\img\\productparameter.png", "..\\..\\img\\productparameter_disabled.png", new Action<object>(this.btnDual_Click), this.accessManager, this.rightDualSensitivity, this.rightDualSensitivity);
				this.SubMenuItems.Add(item11);
			}
			bool flag15 = flag && this.testcardSimService.CheckAndSetActivationTestcardSim(this.productManager.CurrentLoadedProduct);
			if (flag15)
			{
				SubMenuItem item12 = new SubMenuItem("Empfindlichkeitsprognose", "menuItemTestcardSim", "..\\..\\img\\simulateTestcard.png", "..\\..\\img\\simulateTestcard.png", new Action<object>(this.btnTestcardSim_Click), this.accessManager, this.rightTestcardSim, this.rightTestcardSim);
				this.SubMenuItems.Add(item12);
				this.testcardSimService.SetProperties();
			}
			bool flag16 = (flag2 || this.accessManager.CurrentUser.ID < 0) && machineMode != MachineMode.Bulk;
			if (flag16)
			{
				SubMenuItem item13 = new SubMenuItem("Integrierter Validierungsprozess", "menuItemIVP", "..\\..\\img\\certificate_color2.png", "..\\..\\img\\certificate_color_disabled2.png", new Action<object>(this.btnIVP_Click), this.accessManager, this.rightIVP, this.rightIVP);
				this.SubMenuItems.Add(item13);
			}
			bool flag17 = machineMode != MachineMode.Bulk;
			if (flag17)
			{
				SubMenuItem item14 = new SubMenuItem("Histogramm", "menuItemHistogram", "..\\..\\img\\histogram.png", new Action<object>(this.btnHistogram_Click), this.accessManager, this.rightHistogram, this.rightHistogram);
				this.SubMenuItems.Add(item14);
			}
			SubMenuItem item15 = new SubMenuItem("Datalogger", "menuItemDatalogger", "..\\..\\img\\dataplotter.png", new Action<object>(this.btnDataloggerSelection_Click), this.accessManager, this.rightDatalogger, this.rightDatalogger);
			this.SubMenuItems.Add(item15);
			bool flag18 = false;
			bool flag19 = false;
			bool flag20 = false;
			bool flag21 = false;
			this.dbConnection.GetMultiProductsEnabled(ref flag18);
			this.dbConnection.GetMultiLaneProductsEnabled(ref flag19);
			this.dbConnection.GetMultiLaneView(ref flag21);
			this.dbConnection.GetMultiRegionEnabled(ref flag20);
			bool flag22 = !flag5 && !flag19 && !flag18 && !flag20 && !flag21;
			if (flag22)
			{
				SubMenuItem item16 = new SubMenuItem("Checkpoints", "txtCheckpoints", "..\\..\\img\\compareParams.png", new Action<object>(this.btnCompareParams_Click), this.accessManager, this.rightCompareParams, this.rightCompareParams);
				this.SubMenuItems.Add(item16);
			}
			SubMenuItem item17 = new SubMenuItem("Backup", "menuItemBackup", "..\\..\\img\\backup.png", "..\\..\\img\\backup_disabled.png", new Action<object>(this.btnBackup_Click), this.accessManager, this.rightBackup, this.rightBackup);
			this.SubMenuItems.Add(item17);
			string text2 = (this.wifiClient.WlanClient != null && this.wifiClient.WlanClient.Interfaces.Length != 0) ? "..\\..\\img\\wiredWireless.png" : "..\\..\\img\\network.png";
			SubMenuItem subMenuItem = new SubMenuItem("Netzwerk", "menuItemNetworkSettings", text2, new Action<object>(this.btnNetworkSettings_Click), this.accessManager, this.rightNetworkSettings, this.rightNetworkSettings);
			bool flag23 = text2 == "..\\..\\img\\wiredWireless.png";
			if (flag23)
			{
				subMenuItem.Stretch = Stretch.Uniform;
				subMenuItem.ImageHeight = 45.0;
			}
			this.SubMenuItems.Add(subMenuItem);
			SubMenuItem item18 = new SubMenuItem("Change language", "menuItemLanguage", "..\\..\\img\\changeLanguage.png", new Action<object>(this.btnChangeLanguage_Click), this.accessManager, this.rightLanguage, this.rightLanguage);
			this.SubMenuItems.Add(item18);
			SubMenuItem subMenuItem2 = new SubMenuItem("Branding", "menuItemBranding", "..\\..\\img\\SuSLogo.png", new Action<object>(this.btnBranding_Click), this.accessManager, this.rightBranding, this.rightBranding);
			subMenuItem2.Stretch = Stretch.Uniform;
			subMenuItem2.ImageHeight = 25.0;
			this.SubMenuItems.Add(subMenuItem2);
			string a2 = "";
			this.dbConnection.GetParameterValue("/hardware/svt/Enable/Value", DBConnection.defaultApplication, ref a2);
			bool flag24 = a2 == "0";
			if (flag24)
			{
				SubMenuItem subMenuItem3 = new SubMenuItem("Software features", "", "..\\..\\img\\features.png", new Action<object>(this.btnFeatures_Click), this.accessManager, this.rightFeatures, this.rightFeatures);
				subMenuItem3.Stretch = Stretch.Uniform;
				subMenuItem3.ImageHeight = 42.0;
				this.SubMenuItems.Add(subMenuItem3);
			}
			SubMenuItem item19 = new SubMenuItem("Hardware features", "", "..\\..\\img\\hardwareFeatures.png", new Action<object>(this.btnHardwareFeatures_Click), this.accessManager, this.rightFeatures, this.rightFeatures);
			this.SubMenuItems.Add(item19);
			SubMenuItem subMenuItem4 = new SubMenuItem("Remote", "menuItemRemote", "..\\..\\img\\remote2.png", new Action<object>(this.btnRemote_Click), this.accessManager, BuiltInPriviledges.Admin, BuiltInPriviledges.Admin);
			subMenuItem4.Stretch = Stretch.Uniform;
			subMenuItem4.ImageHeight = 50.0;
			this.SubMenuItems.Add(subMenuItem4);
			SubMenuItem item20 = new SubMenuItem("Monitoring", "txtMonitoringPred", "..\\..\\img\\monitoring_predictive_maint.png", new Action<object>(this.btnMonitoring_Click), this.accessManager, this.rightMonitoring, this.rightMonitoring);
			this.SubMenuItems.Add(item20);
			bool flag25 = flag3 || flag4;
			if (flag25)
			{
				SubMenuItem item21 = new SubMenuItem("THiNK X", "THiNK X", "..\\..\\img\\ai_brain_small.png", new Action<object>(this.btnAI_Click), this.accessManager, this.rightAI, this.rightAI);
				this.SubMenuItems.Add(item21);
				bool flag26 = flag3;
				if (flag26)
				{
					this.testcardSimService.SetProperties();
				}
			}
			SubMenuItem item22 = new SubMenuItem("Dokumente", "menuItemDocuments", "..\\..\\img\\help.png", new Action<object>(this.btnManual_Click), this.accessManager, this.rightManual, this.rightManual);
			this.SubMenuItems.Add(item22);
			SubMenuItem item23 = new SubMenuItem("Werkseinstellungen", "menuItemFactorySettings", "..\\..\\img\\settings.png", new Action<object>(this.btnFactorySettings_Click), this.accessManager, this.rightResetToFactorySettings, this.rightResetToFactorySettings);
			this.SubMenuItems.Add(item23);
			bool flag27 = this.ParameterHandling != null && machineType != MachineType.RayconD;
			if (flag27)
			{
				SubMenuItem item24 = new SubMenuItem("Inbetriebnahme", "menuItemComissioning", "..\\..\\img\\comissioning.png", "..\\..\\img\\comissioning_disabled.png", new Action<object>(this.btnComissioning_Click), this.accessManager, this.rightResetToFactorySettings, this.rightResetToFactorySettings);
				this.SubMenuItems.Add(item24);
			}
			SubMenuItem item25 = new SubMenuItem("Version", "menuItemVersion", "..\\..\\img\\info.png", new Action<object>(this.btnVersion_Click), this.accessManager, this.rightVersion, this.rightVersion);
			this.SubMenuItems.Add(item25);
			SubMenuItem item26 = new SubMenuItem("Close", "menuItemClose", "..\\..\\img\\cancel.png", "..\\..\\img\\cancel_disabled.png", new Action<object>(this.btnClose_Click), this.accessManager, this.rightClose, this.rightClose);
			this.SubMenuItems.Add(item26);
			SubMenuItem item27 = new SubMenuItem("Restart", "menuItemRestart", "..\\..\\img\\cancel.png", "..\\..\\img\\cancel_disabled.png", new Action<object>(this.btnRestart_Click), this.accessManager, this.rightClose, this.rightClose);
			this.SubMenuItems.Add(item27);
		}

		// Token: 0x060024F2 RID: 9458 RVA: 0x000FE650 File Offset: 0x000FC850
		private void btnFactorySettings_Click(object sender)
		{
			bool flag = Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtConfirmFactoryReset"), DialogStyle.Warning, DialogButtons.OKCancel) == Dialog.DialogResults.Ok;
			if (flag)
			{
				FactorySettings factorySettings = new FactorySettings(this.dbConnection, this.productManager, this.logbook);
				factorySettings.Reset(this.ParameterHandling);
			}
		}

		// Token: 0x060024F3 RID: 9459 RVA: 0x000FE6A8 File Offset: 0x000FC8A8
		private void btnComissioning_Click(object sender)
		{
			CtrlComissioning ctrlComissioning = new CtrlComissioning(this.dbConnection, this.ParameterHandling, this.dbDatalogs, this.auditCheckSettings);
			Subwindow.Show(ctrlComissioning, SubwindowButtons.Back, ctrlComissioning.PanelSubButtons);
		}

		// Token: 0x060024F4 RID: 9460 RVA: 0x000FE6E4 File Offset: 0x000FC8E4
		private void btnPlot_Click(object sender)
		{
			this.mainWindow.PlotShown = true;
			bool flag = this.MachineState.State == MachineStates.Reseasoning || this.MachineState.State == MachineStates.XrayOnCalibrationNotDone || this.MachineState.State == MachineStates.MovingSource;
			if (flag)
			{
				Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtWaitingForCalibration"), DialogStyle.Warning, DialogButtons.OK);
			}
			else
			{
				bool flag2 = this.MachineState.State != MachineStates.ViewingPlot;
				if (flag2)
				{
					bool flag3 = this.accessManager.CurrentUser.Rights == BuiltInPriviledges.Service || Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtTurningOffProductInspection"), DialogStyle.Warning, DialogButtons.OKCancel) == Dialog.DialogResults.Ok;
					if (flag3)
					{
						this.stateBeforePlot = this.MachineState.State;
						this.OnGraphPlotInitialized();
						this.ParameterHandling.WriteValue(Definition.checkArcsEnable, "0");
						CtrlDetectorPlot ctrlDetectorPlot = new CtrlDetectorPlot(this.ParameterHandling, this.ImageTransfer, this.accessManager, this.dbConnection, this.MachineState, this.xrayState);
						ctrlDetectorPlot.Unloaded += this.plot_Unloaded;
						this.StartActivityTimer();
						Subwindow.Show(ctrlDetectorPlot, SubwindowButtons.Back, ctrlDetectorPlot.PanelSubButtons, ctrlDetectorPlot.UiElementRightAligned);
					}
				}
			}
		}

		// Token: 0x060024F5 RID: 9461 RVA: 0x000FE838 File Offset: 0x000FCA38
		private void timerSignalActivity_Tick(object sender, EventArgs e)
		{
			this.accessManager.SignalActivity();
		}

		// Token: 0x060024F6 RID: 9462 RVA: 0x000FE847 File Offset: 0x000FCA47
		private void plot_Unloaded(object sender, RoutedEventArgs e)
		{
			this.mainWindow.PlotShown = false;
			this.StopActivityTimer();
			this.OnGraphPlotUnloaded(this.stateBeforePlot);
		}

		// Token: 0x060024F7 RID: 9463 RVA: 0x000FE86B File Offset: 0x000FCA6B
		private void settingMode_Unloaded(object sender, RoutedEventArgs e)
		{
			this.StopActivityTimer();
		}

		// Token: 0x060024F8 RID: 9464 RVA: 0x000FE878 File Offset: 0x000FCA78
		private void StartActivityTimer()
		{
			bool flag = this.timerSignalActivity == null;
			if (flag)
			{
				this.timerSignalActivity = new DispatcherTimer();
				this.timerSignalActivity.Interval = new TimeSpan(0, 0, 5);
				this.timerSignalActivity.Tick += this.timerSignalActivity_Tick;
				this.timerSignalActivity.Start();
			}
		}

		// Token: 0x060024F9 RID: 9465 RVA: 0x000FE8D8 File Offset: 0x000FCAD8
		private void StopActivityTimer()
		{
			bool flag = this.timerSignalActivity != null;
			if (flag)
			{
				this.timerSignalActivity.Stop();
				this.timerSignalActivity.Tick -= this.timerSignalActivity_Tick;
				this.timerSignalActivity = null;
			}
		}

		// Token: 0x060024FA RID: 9466 RVA: 0x000FE920 File Offset: 0x000FCB20
		private void btnParameter_Click(object sender)
		{
			List<string> paths = new List<string>();
			List<string> values = new List<string>();
			List<string> parameterPathBeginningsToIgnore = this.GetParameterPathBeginningsToIgnore();
			this.GetDefaultPathsForParamTree(paths, values);
			OverviewCommon.OperatingHourVisibility = false;
			this.ParameterHandling.WriteValue("/hardware/detector/dt/MissingLinesDisableCalc/Value", "1");
			CtrlParameter ctrlParameter = new CtrlParameter(this.ParameterHandling, null, false, false, this.logger, true, paths, values, parameterPathBeginningsToIgnore, this.MachineState, true);
			ctrlParameter.StartCreation();
			Subwindow.Show(ctrlParameter);
		}

		// Token: 0x060024FB RID: 9467 RVA: 0x000FE998 File Offset: 0x000FCB98
		private void GetDefaultPathsForParamTree(List<string> paths, List<string> values)
		{
			this.dbConnection.GetProductEntries(DBConnection.defaultApplication, paths, values);
			bool flag = TableOperations.TableExists(this.dbConnection.Connection, DBConnection.defaultApplicationAI);
			if (flag)
			{
				this.dbConnection.GetProductEntries(DBConnection.defaultApplicationAI, paths, values);
			}
			values.Clear();
			paths.Sort(new ParameterPathComparer());
			for (int i = 0; i < paths.Count; i++)
			{
				string item = null;
				bool flag2 = paths[i].Contains("anomalyDetection") || paths[i].Contains("objectDetection");
				if (flag2)
				{
					this.dbConnection.GetParameterValue(paths[i], DBConnection.defaultApplicationAI, ref item);
				}
				else
				{
					this.dbConnection.GetParameterValue(paths[i], DBConnection.defaultApplication, ref item);
				}
				values.Add(item);
			}
			for (int j = 0; j < paths.Count; j++)
			{
				bool flag3 = paths[j].StartsWith("/");
				if (flag3)
				{
					paths[j] = Definition.defaultProductParameterMainPath + Definition.defaultMainProductParameterSubPath + paths[j];
				}
				else
				{
					paths[j] = string.Concat(new string[]
					{
						Definition.defaultProductParameterMainPath,
						Definition.defaultMainProductParameterSubPath,
						"/",
						paths[j],
						"/Value"
					});
				}
			}
			List<string> list = new List<string>();
			List<string> list2 = new List<string>();
			this.dbConnection.GetProductEntries(DBConnection.defaultSubApplication, list, list2);
			list2.Clear();
			list.Sort(new ParameterPathComparer());
			for (int k = 0; k < list.Count; k++)
			{
				string item2 = null;
				this.dbConnection.GetParameterValue(list[k], DBConnection.defaultSubApplication, ref item2);
				list2.Add(item2);
			}
			for (int l = 0; l < list.Count; l++)
			{
				bool flag4 = list[l].StartsWith("/");
				if (flag4)
				{
					list[l] = Definition.defaultProductParameterMainPath + Definition.defaultSubProductParameterSubPath + list[l];
				}
				else
				{
					list[l] = string.Concat(new string[]
					{
						Definition.defaultProductParameterMainPath,
						Definition.defaultSubProductParameterSubPath,
						"/",
						list[l],
						"/Value"
					});
				}
			}
			paths.AddRange(list);
			values.AddRange(list2);
		}

		// Token: 0x060024FC RID: 9468 RVA: 0x000FEC44 File Offset: 0x000FCE44
		private void btnOverview_Click(object sender)
		{
			MachineMode machineMode = MachineMode.Standard;
			this.dbConnection.GetMachineMode(ref machineMode);
			bool flag = machineMode == MachineMode.Bulk || machineMode == MachineMode.BulkObject;
			if (flag)
			{
				this.ctrlOverview = new CtrlOverviewBulk(this.dbConnection, this.ParameterHandling, this.logger, this.accessManager, this.vibro, this.errorHandling, this.conveyor, this.MachineState, this.dbDatalogs, this.messageSystem, this.imageDataTransfer, this.productManager);
				this.ctrlOverview.BulkPlotUnloaded += this.CtrlOverview_BulkPlotUnloaded;
				this.ctrlOverview.BulkPlotInitialized += this.CtrlOverview_BulkPlotInitialized;
				Subwindow.Show(this.ctrlOverview);
			}
			else
			{
				bool flag2 = machineMode == MachineMode.Sideshooter;
				if (flag2)
				{
					CtrlOverviewSsh usrControl = new CtrlOverviewSsh(this.dbConnection, this.ParameterHandling, this.logger, this.accessManager, this.conveyor, this.errorHandling, this.productManager, this.MachineState, this.messageSystem, this.dbDatalogs);
					Subwindow.Show(usrControl);
				}
				else
				{
					MachineType machineType = MachineType.MachineTypeUndefined;
					this.dbConnection.GetMachineType(ref machineType);
					bool flag3 = machineType == MachineType.RayconXRS;
					if (flag3)
					{
						CtrlOverviewXRS usrControl2 = new CtrlOverviewXRS(this.dbConnection, this.ParameterHandling, this.logger, this.accessManager, this.conveyor, this.errorHandling, this.productManager, this.MachineState, this.messageSystem, this.dbDatalogs);
						Subwindow.Show(usrControl2);
					}
					else
					{
						bool flag4 = machineType == MachineType.RayconD;
						if (flag4)
						{
							CtrlOverviewRayconD usrControl3 = new CtrlOverviewRayconD(this.dbConnection, this.ParameterHandling, this.logger, this.accessManager, this.conveyor, this.errorHandling, this.productManager, this.MachineState, this.messageSystem, this.communicationsList, this.dbDatalogs);
							Subwindow.Show(usrControl3);
						}
						else
						{
							bool flag5 = machineType == MachineType.RayconEntry;
							if (flag5)
							{
								CtrlOverviewEntry usrControl4 = new CtrlOverviewEntry(this.dbConnection, this.ParameterHandling, this.logger, this.accessManager, this.conveyor, this.errorHandling, this.productManager, this.MachineState, this.messageSystem, this.communicationsList, this.dbDatalogs);
								Subwindow.Show(usrControl4);
							}
							else
							{
								bool flag6 = machineType == MachineType.RayconEX2A;
								if (flag6)
								{
									CtrlOverviewEX2A usrControl5 = new CtrlOverviewEX2A(this.dbConnection, this.ParameterHandling, this.logger, this.accessManager, this.conveyor, this.errorHandling, this.productManager, this.MachineState, this.messageSystem, this.communicationsList, this.dbDatalogs);
									Subwindow.Show(usrControl5);
								}
								else
								{
									bool flag7 = this.ParameterHandling.ReadPar(Definition.wagoMachineTypePath) == Definition.ioModuleIsRayconDPlus1ModuleHYRework;
									if (flag7)
									{
										CtrlOverviewRayconDPlusHyRework usrControl6 = new CtrlOverviewRayconDPlusHyRework(this.dbConnection, this.ParameterHandling, this.logger, this.accessManager, this.conveyor, this.errorHandling, this.productManager, this.MachineState, this.messageSystem, this.communicationsList, this.dbDatalogs);
										Subwindow.Show(usrControl6);
									}
									else
									{
										CtrlOverviewRayconD usrControl7 = new CtrlOverviewRayconD(this.dbConnection, this.ParameterHandling, this.logger, this.accessManager, this.conveyor, this.errorHandling, this.productManager, this.MachineState, this.messageSystem, this.communicationsList, this.dbDatalogs);
										Subwindow.Show(usrControl7);
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x060024FD RID: 9469 RVA: 0x000FEFC5 File Offset: 0x000FD1C5
		private void CtrlOverview_BulkPlotInitialized()
		{
			this.mainWindow.PlotShown = true;
			this.OnGraphPlotInitialized();
		}

		// Token: 0x060024FE RID: 9470 RVA: 0x000FEFDC File Offset: 0x000FD1DC
		private void CtrlOverview_BulkPlotUnloaded(MachineStates obj)
		{
			this.mainWindow.PlotShown = false;
			this.OnGraphPlotUnloaded(obj);
		}

		// Token: 0x060024FF RID: 9471 RVA: 0x000FEFF4 File Offset: 0x000FD1F4
		private void btnAcquisition_Click(object sender)
		{
			CtrlAcquisitionSettings usrControl = new CtrlAcquisitionSettings(this.ParameterHandling, this.ImageTransfer, this.dbConnection, this.productManager, this.MachineState, this.imageArchiver, this.accessManager, this.testcardSimService);
			Subwindow.Show(usrControl);
		}

		// Token: 0x06002500 RID: 9472 RVA: 0x000FF040 File Offset: 0x000FD240
		private void btnFilter_Click(object sender)
		{
			bool flag = this.productManager.CurrentLoadedProduct.IsSingleProduct || this.productManager.CurrentLoadedProduct.IsBulkObjectProduct;
			if (flag)
			{
				FilterControl usrControl = new FilterControl(this.ParameterHandling, this.ImageTransfer, this.dbConnection, this.accessManager, this.MachineState, this.productManager, 0, this.imageArchiver, this.testcardSimService, this.auditCheckSettings);
				Subwindow.Show(usrControl, SubwindowButtons.BackAndHome);
			}
			else
			{
				bool flag2;
				if (this.productManager.CurrentLoadedProduct.IsLaneProduct)
				{
					flag2 = ((from product in this.productManager.CurrentLoadedProduct.SubProducts
					where product.ProductAssigned
					select product).ToList<ProductData>().Count > 0);
				}
				else
				{
					flag2 = true;
				}
				bool flag3 = flag2;
				if (flag3)
				{
					this.productManager.SelectedProduct = this.productManager.CurrentLoadedProduct;
					CtrlSubProduct ctrlSubProduct = new CtrlSubProduct(this.productManager, SubProductModes.Filter, null, null);
					Subwindow.Show(ctrlSubProduct, SubwindowButtons.BackAndHome, ctrlSubProduct.Buttons);
				}
				else
				{
					Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtNoProductAssigned"), DialogStyle.Error, DialogButtons.OK);
				}
			}
		}

		// Token: 0x06002501 RID: 9473 RVA: 0x000FF174 File Offset: 0x000FD374
		private void btnSend_Click(object sender)
		{
			MachineMode machineMode = MachineMode.Standard;
			this.dbConnection.GetMachineMode(ref machineMode);
			bool flag = machineMode == MachineMode.Bulk && this.ParameterHandling.ReadPar(XrayState.xrayPowerOnPath) == "1";
			if (flag)
			{
				Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtTurnXrayOff"), DialogStyle.Warning, DialogButtons.OK);
			}
			else
			{
				CtrlSendImage ctrlSendImage = new CtrlSendImage(this.ImageTransfer, this.ParameterHandling, this.dbConnection, this.imageArchiver, this.productManager, this.testcardSimService);
				Subwindow.Show(ctrlSendImage, SubwindowButtons.BackAndHome, ctrlSendImage.Buttons);
			}
		}

		// Token: 0x06002502 RID: 9474 RVA: 0x000FF214 File Offset: 0x000FD414
		private void btnShow_Click(object sender)
		{
			CtrlShowImage ctrlShowImage = new CtrlShowImage(this.ParameterHandling, this.imageArchiver);
			Subwindow.Show(ctrlShowImage, SubwindowButtons.BackAndHome, ctrlShowImage.Buttons);
		}

		// Token: 0x06002503 RID: 9475 RVA: 0x000FF244 File Offset: 0x000FD444
		private void btnDataloggerSelection_Click(object sender)
		{
			CtrlDatalogger ctrlDatalogger = new CtrlDatalogger(this.ParameterHandling, this.logger, this.dbConnection);
			Subwindow.Show(ctrlDatalogger, SubwindowButtons.BackAndHome, ctrlDatalogger.Buttons);
		}

		// Token: 0x06002504 RID: 9476 RVA: 0x0001092C File Offset: 0x0000EB2C
		private void btnSitemap_Click(object sender)
		{
		}

		// Token: 0x06002505 RID: 9477 RVA: 0x000FF278 File Offset: 0x000FD478
		private void btnManual_Click(object sender)
		{
			string text = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\OtherDocuments";
			string usbPath = "";
			bool flag = !Directory.Exists(text);
			if (flag)
			{
				Directory.CreateDirectory(text);
			}
			this.dbConnection.GetUsbDriveLetter(ref usbPath);
			CtrlDocumentsManagement usrControl = new CtrlDocumentsManagement(text, usbPath, this.accessManager);
			Subwindow.Show(usrControl, SubwindowButtons.BackAndHome);
		}

		// Token: 0x06002506 RID: 9478 RVA: 0x000FF2E0 File Offset: 0x000FD4E0
		private void btnVersion_Click(object sender)
		{
			MachineMode machineMode = MachineMode.Standard;
			MachineType machineType = MachineType.MachineTypeUndefined;
			DetectorType detectorType = DetectorType.DT;
			this.dbConnection.GetMachineMode(ref machineMode);
			this.dbConnection.GetMachineType(ref machineType);
			this.dbConnection.GetDetectorType(ref detectorType);
			this.ParameterHandling.WriteValue("/machine/IPConfiguration/Value", IPInfo.GetIPConfigString(), false);
			List<string> list = Definition.versionPathes.ToList<string>();
			List<string> list2 = Definition.versionPathes.ToList<string>();
			bool flag = detectorType == DetectorType.DT || detectorType == DetectorType.DTNetSE;
			if (flag)
			{
				list.AddRange(Definition.detectorDTVersionPathes);
				list2.AddRange(Definition.detectorDTVersionPathes);
			}
			else
			{
				bool flag2 = detectorType == DetectorType.DTNetDE || detectorType == DetectorType.DTNetTDI;
				if (flag2)
				{
					list.AddRange(Definition.detectorDTNetVersionPathes);
					list2.AddRange(Definition.detectorDTNetVersionPathes);
				}
				else
				{
					list.AddRange(Definition.detectorSesotecVersionPathes);
					list2.AddRange(Definition.detectorSesotecVersionPathes);
				}
			}
			string[] ioversionPaths = this.GetIOVersionPaths(machineType, machineMode);
			list.AddRange(ioversionPaths);
			list2.AddRange(ioversionPaths);
			OverviewCommon.OperatingHourVisibility = false;
			CtrlParameter ctrlParameter = new CtrlParameter(this.ParameterHandling, list.ToArray(), true, false, this.logger, false, null, null, null, true);
			ctrlParameter.StartCreation();
			Subwindow.Show(ctrlParameter);
		}

		// Token: 0x06002507 RID: 9479 RVA: 0x000FF416 File Offset: 0x000FD616
		private void btnClose_Click(object sender)
		{
			Application.Current.Shutdown();
		}

		// Token: 0x06002508 RID: 9480 RVA: 0x000FF424 File Offset: 0x000FD624
		private void btnRestart_Click(object sender)
		{
			Process.Start(Restore.restoreProcessPath, "Restart");
			Application.Current.Shutdown();
		}

		// Token: 0x06002509 RID: 9481 RVA: 0x000FF442 File Offset: 0x000FD642
		private void btnChangeLanguage_Click(object sender)
		{
			this.ctrlLanguage = new CtrlLanguage(this.dbConnection);
			this.ctrlLanguage.Unloaded += this.ctrlLanguage_Unloaded;
			Subwindow.Show(this.ctrlLanguage, SubwindowButtons.BackAndHome);
		}

		// Token: 0x0600250A RID: 9482 RVA: 0x000FF47C File Offset: 0x000FD67C
		private void ctrlLanguage_Unloaded(object sender, RoutedEventArgs e)
		{
			bool flag = this.ctrlLanguage != null;
			if (flag)
			{
				this.ctrlLanguage.Unloaded -= this.ctrlLanguage_Unloaded;
			}
			this.InitializeSubMenuItems();
		}

		// Token: 0x0600250B RID: 9483 RVA: 0x000FF4B8 File Offset: 0x000FD6B8
		private void btnAuditChange_Click(object sender)
		{
			CtrlAuditCheck usrControl = new CtrlAuditCheck(this.auditCheckSettings, this.dbConnection, this.accessManager);
			Subwindow.Show(usrControl, SubwindowButtons.BackAndHome);
		}

		// Token: 0x0600250C RID: 9484 RVA: 0x000FF4E8 File Offset: 0x000FD6E8
		private void btnAuditChange_Click2(object sender)
		{
			int imageGroup = 0;
			SubMenu usrControl = new SubMenu(this.ParameterHandling, this.dbConnection, this.accessManager, this.MachineState, this.ImageTransfer, this.backup, this.productManager, this.messageSystem, this.auditCheckSettings, this.restore, imageGroup);
			Subwindow.Show(usrControl, SubwindowButtons.BackAndHome);
		}

		// Token: 0x0600250D RID: 9485 RVA: 0x000FF544 File Offset: 0x000FD744
		private void btnBackup_Click(object sender)
		{
			CtrlBackup usrControl = new CtrlBackup(this.dbConnection, this.ParameterHandling, this.restore, this.backup, this.accessManager, this.testcardSimService, this.productManager);
			Subwindow.Show(usrControl, SubwindowButtons.BackAndHome);
		}

		// Token: 0x0600250E RID: 9486 RVA: 0x000FF58C File Offset: 0x000FD78C
		protected virtual void OnGraphPlotInitialized()
		{
			Action graphPlotInitialized = this.GraphPlotInitialized;
			bool flag = graphPlotInitialized != null;
			if (flag)
			{
				graphPlotInitialized();
			}
		}

		// Token: 0x0600250F RID: 9487 RVA: 0x000FF5B0 File Offset: 0x000FD7B0
		protected virtual void OnGraphPlotUnloaded(MachineStates oldState)
		{
			Action<MachineStates> graphPlotUnloaded = this.GraphPlotUnloaded;
			bool flag = graphPlotUnloaded != null;
			if (flag)
			{
				graphPlotUnloaded(oldState);
			}
		}

		// Token: 0x06002510 RID: 9488 RVA: 0x000FF5D8 File Offset: 0x000FD7D8
		private void CloseSubMenu(object obj)
		{
			foreach (SubMenuItem subMenuItem in this.SubMenuItems)
			{
				subMenuItem.DeregisterLocalizedText();
			}
			this.ParameterHandling.ParameterChanged -= this.parHandling_ParameterChanged;
			bool flag = this.ctrlOverview != null;
			if (flag)
			{
				this.ctrlOverview.BulkPlotUnloaded -= this.CtrlOverview_BulkPlotUnloaded;
				this.ctrlOverview.BulkPlotInitialized -= this.CtrlOverview_BulkPlotInitialized;
			}
			this.setFooterMainWindow.UnSetFooterDeactivation();
		}

		// Token: 0x06002511 RID: 9489 RVA: 0x000FF68C File Offset: 0x000FD88C
		private void btnBranding_Click(object sender)
		{
			CtrlBranding usrControl = new CtrlBranding(this.dbConnection, this.selectedBranding, this.ParameterHandling);
			Subwindow.Show(usrControl, SubwindowButtons.BackAndHome);
		}

		// Token: 0x06002512 RID: 9490 RVA: 0x000FF6BC File Offset: 0x000FD8BC
		private void parHandling_ParameterChanged(object sender, ParameterChangeEventArgs e)
		{
			bool flag = e.Path == Definition.machineModePath || e.Path == Definition.conveyorInManualModePath || e.Path == Definition.machineTypePath || e.Path == FlapsOverIOs.bulkflapsEnabledPath || e.Path == "/hardware/svt/Enable/Value";
			if (flag)
			{
				this.InitializeSubMenuItems();
			}
		}

		// Token: 0x06002513 RID: 9491
		[DllImport("User32.dll", SetLastError = true)]
		public static extern bool SetForegroundWindow(IntPtr hObject);

		// Token: 0x06002514 RID: 9492 RVA: 0x000FF734 File Offset: 0x000FD934
		private void btnRemote_Click(object sender)
		{
			string text = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			bool flag = text.LastIndexOf("\\") > 0;
			if (flag)
			{
				text = text.Substring(0, text.LastIndexOf("\\"));
				text += "\\Teamviewer";
			}
			string text2 = text + "\\" + Definition.teamViewerEnglishExecutableName;
			bool flag2 = File.Exists(text2);
			if (flag2)
			{
				Process[] processesByName = Process.GetProcessesByName("TeamViewer");
				bool flag3 = processesByName.Length == 0;
				if (flag3)
				{
					this.progTeamViewer = new Progress("Starting...", false);
					this.progTeamViewer.Show();
					new Thread(new ThreadStart(this.UpdateProg))
					{
						Name = "UpdateProg"
					}.Start();
					new Process
					{
						StartInfo = 
						{
							FileName = text2
						}
					}.Start();
					this.timerBringTeamViewerToFront = new Timer(new TimerCallback(this.BringTeamViewerToFront));
					this.timerBringTeamViewerToFront.Change(10000, 5000);
				}
			}
			else
			{
				Dialog.Show("Error", "Teamviewer executable not found.", DialogStyle.Error);
			}
		}

		// Token: 0x06002515 RID: 9493 RVA: 0x000FF868 File Offset: 0x000FDA68
		private void BringTeamViewerToFront(object dummy)
		{
			Process[] processesByName = Process.GetProcessesByName("TeamViewer");
			bool flag = processesByName.Length != 0;
			if (flag)
			{
				SubMenuNormalViewModel.SetForegroundWindow(processesByName[0].MainWindowHandle);
			}
			else
			{
				this.timerBringTeamViewerToFront.Dispose();
			}
		}

		// Token: 0x06002516 RID: 9494 RVA: 0x000FF8A8 File Offset: 0x000FDAA8
		private void UpdateProg()
		{
			for (;;)
			{
				Process[] processesByName = Process.GetProcessesByName("TeamViewer");
				bool flag = processesByName.Length != 0;
				if (flag)
				{
					break;
				}
				Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
				{
					this.progTeamViewer.progBar.Value += 10.0;
					bool flag2 = this.progTeamViewer.progBar.Value > 100.0;
					if (flag2)
					{
						this.progTeamViewer.progBar.Value = 0.0;
					}
				}));
				Thread.Sleep(200);
			}
			Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
			{
				this.progTeamViewer.Close();
			}));
		}

		// Token: 0x06002517 RID: 9495 RVA: 0x000FF920 File Offset: 0x000FDB20
		private void btnHistogram_Click(object obj)
		{
			CtrlHistogram ctrlHistogram = new CtrlHistogram(this.ParameterHandling, this.ImageTransfer);
			Subwindow.Show(ctrlHistogram, SubwindowButtons.BackAndHome, ctrlHistogram.Buttons);
		}

		// Token: 0x06002518 RID: 9496 RVA: 0x000FF950 File Offset: 0x000FDB50
		private void btnCompareParams_Click(object obj)
		{
			CtrlQAParams ctrlQAParams = new CtrlQAParams(this.dbConnection, this.ParameterHandling, this.accessManager, true, this.conveyor, this.productManager, this.MachineState);
			Subwindow.Show(ctrlQAParams, SubwindowButtons.BackAndHome, ctrlQAParams.Buttons);
		}

		// Token: 0x06002519 RID: 9497 RVA: 0x000FF998 File Offset: 0x000FDB98
		private void btnSettingMode_Click(object obj)
		{
			MachineMode machineMode = MachineMode.Standard;
			this.dbConnection.GetMachineMode(ref machineMode);
			bool flag = machineMode == MachineMode.Bulk && this.ParameterHandling.ReadPar(XrayState.xrayPowerOnPath) == "1";
			if (flag)
			{
				Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtTurnXrayOff"), DialogStyle.Warning, DialogButtons.OK);
			}
			else
			{
				bool flag2 = Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtCrushingHazard"), DialogStyle.Warning, DialogButtons.OKCancel) == Dialog.DialogResults.Ok;
				if (flag2)
				{
					CtrlSettingMode ctrlSettingMode = new CtrlSettingMode(this.conveyor, new Vibro(this.dbConnection, this.ParameterHandling), this.accessManager, this.dbConnection, this.ParameterHandling, this.MachineState, this.dbDatalogs, this.messageSystem, this.imageDataTransfer);
					ctrlSettingMode.Unloaded += this.settingMode_Unloaded;
					this.StartActivityTimer();
					Subwindow.Show(ctrlSettingMode, SubwindowButtons.BackAndHome, null, true, true, null);
				}
			}
		}

		// Token: 0x0600251A RID: 9498 RVA: 0x000FFA94 File Offset: 0x000FDC94
		private void btnNetworkSettings_Click(object obj)
		{
			SubMenu usrControl = new SubMenu(this.accessManager, this.wifiClient, this.dbConnection, this.MachineState);
			Subwindow.Show(usrControl, SubwindowButtons.BackAndHome);
		}

		// Token: 0x0600251B RID: 9499 RVA: 0x000FFAC8 File Offset: 0x000FDCC8
		private void btnFeatures_Click(object obj)
		{
			CtrlFeatureSelection usrControl = new CtrlFeatureSelection(this.dbConnection, this.ParameterHandling, this.testcardSimService);
			Subwindow.Show(usrControl, SubwindowButtons.BackAndHome);
		}

		// Token: 0x0600251C RID: 9500 RVA: 0x000FFAF8 File Offset: 0x000FDCF8
		private void btnHardwareFeatures_Click(object obj)
		{
			CtrlHardwareFeatures usrControl = new CtrlHardwareFeatures(this.dbConnection, this.ParameterHandling, this.conveyor);
			Subwindow.Show(usrControl, SubwindowButtons.BackAndHome);
		}

		// Token: 0x0600251D RID: 9501 RVA: 0x000FFB28 File Offset: 0x000FDD28
		private string[] GetIOVersionPaths(MachineType machineType, MachineMode machineMode)
		{
			int num = 0;
			List<string> list = new List<string>();
			string s = this.ParameterHandling.ReadPar(Definition.ioVersionPath);
			int.TryParse(s, out num);
			bool flag = num < Definition.minSesotecIOProgramVersion;
			if (flag)
			{
				list.AddRange(Definition.versionPathsWago);
				bool flag2 = machineMode == MachineMode.Bulk;
				if (flag2)
				{
					list.Add(Definition.ioBulkVersionPath.Substring(0, Definition.ioBulkVersionPath.IndexOf("/Value") + 1));
				}
				bool flag3 = machineMode == MachineMode.MAndS;
				if (flag3)
				{
					list.Add(Definition.msVersion1Path.Substring(0, Definition.msVersion1Path.IndexOf("/Value") + 1));
				}
			}
			else
			{
				list.AddRange(Definition.versionPathsSesotecIO);
				bool flag4 = machineType == MachineType.RayconEntry;
				if (flag4)
				{
					List<string> list2 = (from path in list
					where path.EndsWith("_SM/")
					select path).ToList<string>();
					for (int i = 0; i < list2.Count; i++)
					{
						list.Remove(list2[i]);
					}
				}
			}
			return list.ToArray();
		}

		// Token: 0x0600251E RID: 9502 RVA: 0x000FFC50 File Offset: 0x000FDE50
		private List<string> GetParameterPathBeginningsToIgnore()
		{
			List<string> list = null;
			MachineType machineType = MachineType.MachineTypeUndefined;
			this.dbConnection.GetMachineType(ref machineType);
			bool flag = machineType == MachineType.RayconEntry;
			if (flag)
			{
				list = new List<string>();
				list.Add(Definition.multiProductsEnablePath);
				list.Add(Definition.multiProductViewPath);
				list.Add(Definition.multiLaneProductsPath);
				list.Add(Definition.multiLaneViewPath);
				list.Add(Definition.multiRegionPath);
			}
			return list;
		}

		// Token: 0x0600251F RID: 9503 RVA: 0x000FFCC4 File Offset: 0x000FDEC4
		private void btnDual_Click(object obj)
		{
			CtrlDualEnergy ctrlDualEnergy = new CtrlDualEnergy(this.imageArchiver, this.ImageTransfer, this.ParameterHandling, this.productManager, this.dbConnection, this.accessManager, this.MachineState);
			Subwindow.Show(ctrlDualEnergy, SubwindowButtons.BackAndHome, ctrlDualEnergy.Buttons);
		}

		// Token: 0x06002520 RID: 9504 RVA: 0x000FFD10 File Offset: 0x000FDF10
		private void btnTestcardSim_Click(object obj)
		{
			bool flag = this.ParameterHandling != null && this.ParameterHandling.ReadPar(Definition.autoKVAdjustmentPath) != "1";
			if (flag)
			{
				Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtTestCardSimAutokV"), DialogStyle.Warning, DialogButtons.OK);
			}
			else
			{
				CtrlTestcardSim ctrlTestcardSim = new CtrlTestcardSim(this.ParameterHandling, this.imageArchiver, this.accessManager, this.ImageTransfer, this.dbConnection, this.productManager, this.testcardSimService, this.errorHandling);
				Subwindow.Show(ctrlTestcardSim, SubwindowButtons.BackAndHome, ctrlTestcardSim.PanelButtons, ctrlTestcardSim.UiElementRightAligned);
			}
		}

		// Token: 0x06002521 RID: 9505 RVA: 0x000FFDB4 File Offset: 0x000FDFB4
		private void btnAI_Click(object obj)
		{
			CtrlAIMenuSelection ctrlAIMenuSelection = new CtrlAIMenuSelection(this.ParameterHandling, this.productManager, this.dbConnection, this.accessManager, this.imageDataTransfer, this.imageArchiver, this.errorHandling, this.testcardSimService, this.aiService, CtrlAIMenuSelection.SelectionMenuContext.MainMenu);
			Subwindow.Show(ctrlAIMenuSelection, SubwindowButtons.BackAndHome, ctrlAIMenuSelection.PanelButtons, ctrlAIMenuSelection.UiElementRightAligned);
		}

		// Token: 0x06002522 RID: 9506 RVA: 0x000FFE14 File Offset: 0x000FE014
		private void btnIVP_Click(object obj)
		{
			SubMenu subMenu = new SubMenu(this.ParameterHandling, this.dbConnection, this.accessManager, this.MachineState, this.ImageTransfer, this.backup, this.productManager, this.messageSystem);
			Subwindow.Show(subMenu, SubwindowButtons.BackAndHome, subMenu.UiElementRightAligned, 1);
		}

		// Token: 0x06002523 RID: 9507 RVA: 0x000FFE68 File Offset: 0x000FE068
		private void btnMonitoring_Click(object obj)
		{
			SubMenu usrControl = new SubMenu(this.ParameterHandling, this.dbConnection, this.accessManager, this.MachineState, this.dbDatalogs, this.errorSurvey, this.mainWindow);
			Subwindow.Show(usrControl, SubwindowButtons.BackAndHome);
		}

		// Token: 0x040017A4 RID: 6052
		private BuiltInPriviledges rightOverview = BuiltInPriviledges.Operator3;

		// Token: 0x040017A5 RID: 6053
		private BuiltInPriviledges rightAcquisition = BuiltInPriviledges.Operator1;

		// Token: 0x040017A6 RID: 6054
		private BuiltInPriviledges rightParameter = BuiltInPriviledges.Service;

		// Token: 0x040017A7 RID: 6055
		private BuiltInPriviledges rightPlot = BuiltInPriviledges.Operator1;

		// Token: 0x040017A8 RID: 6056
		private BuiltInPriviledges rightFilter = BuiltInPriviledges.Operator1;

		// Token: 0x040017A9 RID: 6057
		private BuiltInPriviledges rightSimulate = BuiltInPriviledges.Service;

		// Token: 0x040017AA RID: 6058
		private BuiltInPriviledges rightShow = BuiltInPriviledges.Service;

		// Token: 0x040017AB RID: 6059
		private BuiltInPriviledges rightHistogram = BuiltInPriviledges.Service;

		// Token: 0x040017AC RID: 6060
		private BuiltInPriviledges rightDatalogger = BuiltInPriviledges.Service;

		// Token: 0x040017AD RID: 6061
		private BuiltInPriviledges rightManual = BuiltInPriviledges.Operator3;

		// Token: 0x040017AE RID: 6062
		private BuiltInPriviledges rightVersion = BuiltInPriviledges.Operator3;

		// Token: 0x040017AF RID: 6063
		private BuiltInPriviledges rightClose = BuiltInPriviledges.Service;

		// Token: 0x040017B0 RID: 6064
		private BuiltInPriviledges rightLanguage = BuiltInPriviledges.Admin;

		// Token: 0x040017B1 RID: 6065
		private BuiltInPriviledges rightAuditCheck = BuiltInPriviledges.Operator1;

		// Token: 0x040017B2 RID: 6066
		private BuiltInPriviledges rightBackup = BuiltInPriviledges.Operator1;

		// Token: 0x040017B3 RID: 6067
		private BuiltInPriviledges rightBranding = BuiltInPriviledges.Service;

		// Token: 0x040017B4 RID: 6068
		private BuiltInPriviledges rightResetToFactorySettings = BuiltInPriviledges.Service;

		// Token: 0x040017B5 RID: 6069
		private BuiltInPriviledges rightCompareParams = BuiltInPriviledges.Admin;

		// Token: 0x040017B6 RID: 6070
		private BuiltInPriviledges rightNetworkSettings = BuiltInPriviledges.Admin;

		// Token: 0x040017B7 RID: 6071
		private BuiltInPriviledges rightFeatures = BuiltInPriviledges.Service;

		// Token: 0x040017B8 RID: 6072
		private BuiltInPriviledges rightDualSensitivity = BuiltInPriviledges.Operator1;

		// Token: 0x040017B9 RID: 6073
		private BuiltInPriviledges rightTestcardSim = BuiltInPriviledges.Operator1;

		// Token: 0x040017BA RID: 6074
		private BuiltInPriviledges rightAI = BuiltInPriviledges.Admin;

		// Token: 0x040017BB RID: 6075
		private BuiltInPriviledges rightIVP = BuiltInPriviledges.Admin;

		// Token: 0x040017BC RID: 6076
		private BuiltInPriviledges rightAuditCheckNew = BuiltInPriviledges.Operator3;

		// Token: 0x040017BD RID: 6077
		private BuiltInPriviledges rightMonitoring = BuiltInPriviledges.Operator3;

		// Token: 0x040017BE RID: 6078
		private ParameterHandling ParameterHandling;

		// Token: 0x040017BF RID: 6079
		private ImageDataTransfer ImageTransfer;

		// Token: 0x040017C0 RID: 6080
		private Logger logger;

		// Token: 0x040017C1 RID: 6081
		private DBConnection dbConnection;

		// Token: 0x040017C2 RID: 6082
		private DispatcherTimer timerSignalActivity;

		// Token: 0x040017C3 RID: 6083
		private AccessRightsManager accessManager;

		// Token: 0x040017C4 RID: 6084
		private Conveyor conveyor;

		// Token: 0x040017C5 RID: 6085
		private AuditCheckSettings auditCheckSettings;

		// Token: 0x040017C6 RID: 6086
		private Restore restore;

		// Token: 0x040017C7 RID: 6087
		private Backup backup;

		// Token: 0x040017C8 RID: 6088
		private CtrlLanguage ctrlLanguage;

		// Token: 0x040017C9 RID: 6089
		private Branding selectedBranding;

		// Token: 0x040017CA RID: 6090
		private ProductManager productManager;

		// Token: 0x040017CB RID: 6091
		private Logbook logbook;

		// Token: 0x040017CC RID: 6092
		private ErrorHandling errorHandling;

		// Token: 0x040017CD RID: 6093
		private Timer timerBringTeamViewerToFront;

		// Token: 0x040017CE RID: 6094
		private Progress progTeamViewer;

		// Token: 0x040017CF RID: 6095
		private MachineStates stateBeforePlot;

		// Token: 0x040017D0 RID: 6096
		private StoreImageData imageArchiver;

		// Token: 0x040017D1 RID: 6097
		private SubMenuItem itSettingMode;

		// Token: 0x040017D2 RID: 6098
		private WifiClient wifiClient;

		// Token: 0x040017D3 RID: 6099
		private Vibro vibro;

		// Token: 0x040017D4 RID: 6100
		private MessageSystem messageSystem;

		// Token: 0x040017D5 RID: 6101
		private List<object> communicationsList;

		// Token: 0x040017D6 RID: 6102
		private TestcardSimService testcardSimService;

		// Token: 0x040017D7 RID: 6103
		private AIService aiService;

		// Token: 0x040017D8 RID: 6104
		private SetFooterMainWindow setFooterMainWindow;

		// Token: 0x040017D9 RID: 6105
		private DBDataLogs dbDatalogs;

		// Token: 0x040017E0 RID: 6112
		private ErrorSurvey errorSurvey;

		// Token: 0x040017E1 RID: 6113
		private MainWindowViewModel mainWindow;

		// Token: 0x040017E2 RID: 6114
		private ImageDataTransfer imageDataTransfer;

		// Token: 0x040017E3 RID: 6115
		private CtrlOverviewBulk ctrlOverview;

		// Token: 0x040017E4 RID: 6116
		private XrayState xrayState;
	}
}
