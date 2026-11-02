using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using DatabaseConnection;
using Definitions;
using GUI_XRS.Datalogger;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x02000251 RID: 593
	public class SubMenu : UserControl, IComponentConnector
	{
		// Token: 0x17000C2D RID: 3117
		// (get) Token: 0x06002D4D RID: 11597 RVA: 0x00129049 File Offset: 0x00127249
		// (set) Token: 0x06002D4E RID: 11598 RVA: 0x00129051 File Offset: 0x00127251
		public SubMenuNormalViewModel ViewModelNormal { get; set; }

		// Token: 0x06002D4F RID: 11599 RVA: 0x0012905C File Offset: 0x0012725C
		public SubMenu(ParameterHandling parHandling, ImageDataTransfer imageTransfer, DBConnection dbConnection, AccessRightsManager accessManager, MachineState machineState, ProductManager productManager, Vibro vibro, StoreImageData imageArchiver)
		{
			this.InitializeComponent();
			this.ViewModelBulk = new SubMenuLearnBulkViewModel(parHandling, imageTransfer, dbConnection, accessManager, machineState, productManager, vibro, imageArchiver);
			base.DataContext = this.ViewModelBulk;
			CursorsSettings.SetCursor(this);
		}

		// Token: 0x06002D50 RID: 11600 RVA: 0x001290A4 File Offset: 0x001272A4
		public SubMenu(ParameterHandling parHandling, DBConnection dbConnection, AccessRightsManager accessManager, MachineState machineState, ImageDataTransfer imageTransfer, Backup backup, ProductManager productManager, MessageSystem messageSystem)
		{
			this.InitializeComponent();
			this.dBConnection = dbConnection;
			this.ViewModelIVP = new SubMenuIVPViewModel(parHandling, dbConnection, accessManager, machineState, imageTransfer, backup, productManager, messageSystem);
			this.InitializeLabel();
			base.DataContext = this.ViewModelIVP;
			CursorsSettings.SetCursor(this);
		}

		// Token: 0x06002D51 RID: 11601 RVA: 0x001290FC File Offset: 0x001272FC
		public SubMenu(ParameterHandling parHandling, DBConnection dbConnection, AccessRightsManager accessManager, MachineState machineState, ImageDataTransfer imageTransfer, Backup backup, ProductManager productManager, MessageSystem messageSystem, AuditCheckSettings auditCheckSettings, Restore restore, int imageGroup)
		{
			this.InitializeComponent();
			this.ViewModelAuditcheck = new SubMenuAuditcheckViewModel(parHandling, dbConnection, accessManager, machineState, imageTransfer, backup, productManager, messageSystem, auditCheckSettings, restore, imageGroup);
			base.DataContext = this.ViewModelAuditcheck;
			CursorsSettings.SetCursor(this);
		}

		// Token: 0x06002D52 RID: 11602 RVA: 0x0012914C File Offset: 0x0012734C
		public SubMenu(ParameterHandling parHandling, ImageDataTransfer imageTransfer, Logger logger, DBConnection dbConnection, MachineState machineState, AccessRightsManager accessManager, Conveyor conveyor, AuditCheckSettings auditCheckSettings, Restore restore, Backup backup, Branding selectedBranding, ProductManager productManager, Logbook logbook, ErrorHandling errorHandling, StoreImageData imageArchiver, WifiClient wifiClient, Vibro vibro, MessageSystem messageSystem, List<object> communicationsList, TestcardSimService testcardSimService, SetFooterMainWindow setFooterMainWindow, DBDataLogs dbDatalogs, ErrorSurvey errorSurvey, MainWindowViewModel mainWindow, ImageDataTransfer imageDataTransfer, AIService aiService, XrayState xrayState)
		{
			this.InitializeComponent();
			this.ViewModelNormal = new SubMenuNormalViewModel(parHandling, imageTransfer, logger, dbConnection, machineState, accessManager, conveyor, auditCheckSettings, restore, backup, selectedBranding, productManager, logbook, errorHandling, imageArchiver, wifiClient, vibro, messageSystem, communicationsList, testcardSimService, setFooterMainWindow, dbDatalogs, errorSurvey, mainWindow, imageDataTransfer, aiService, xrayState);
			base.DataContext = this.ViewModelNormal;
			CursorsSettings.SetCursor(this);
			setFooterMainWindow.SetFooterDeactivation();
		}

		// Token: 0x06002D53 RID: 11603 RVA: 0x001291C3 File Offset: 0x001273C3
		public SubMenu(AccessRightsManager accessManager, WifiClient wifiClient, DBConnection dbConnection, MachineState machineState)
		{
			this.InitializeComponent();
			this.viewModelNetwork = new SubMenuNetworkViewModel(accessManager, wifiClient, dbConnection, machineState);
			base.DataContext = this.viewModelNetwork;
			CursorsSettings.SetCursor(this);
		}

		// Token: 0x06002D54 RID: 11604 RVA: 0x001291F8 File Offset: 0x001273F8
		public SubMenu(DBConnection dbConnection, AccessRightsManager accessManager, MachineState machineState, List<object> communicationsList)
		{
			this.InitializeComponent();
			this.viewModelCommunications = new SubMenuCommunicationsViewModel(dbConnection, accessManager, communicationsList, machineState);
			base.DataContext = this.viewModelCommunications;
			CursorsSettings.SetCursor(this);
		}

		// Token: 0x06002D55 RID: 11605 RVA: 0x0012922D File Offset: 0x0012742D
		private void lbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			this.lbox.SelectedIndex = -1;
		}

		// Token: 0x06002D56 RID: 11606 RVA: 0x0012923D File Offset: 0x0012743D
		public SubMenu(ParameterHandling parHandling, DBConnection dbConnection, AccessRightsManager accessManager, MachineState machineState, DBDataLogs dBDataLogs, ErrorSurvey errorSurvey, MainWindowViewModel mainWindow)
		{
			this.InitializeComponent();
			this.viewModelMonitoring = new SubMenuMonitoringViewModel(parHandling, dbConnection, accessManager, machineState, dBDataLogs, errorSurvey, mainWindow);
			base.DataContext = this.viewModelMonitoring;
			CursorsSettings.SetCursor(this);
		}

		// Token: 0x06002D57 RID: 11607 RVA: 0x00129278 File Offset: 0x00127478
		private void InitializeLabel()
		{
			Label label = new Label();
			Definition.GUIDesign design = this.dBConnection.GetDesign();
			bool flag = Definition.windowWidth >= 1300.0;
			if (flag)
			{
				bool flag2 = design == Definition.GUIDesign.Bizerba;
				if (flag2)
				{
					label.Content = this.ViewModelIVP.StrIVP.LocalizedText + " by Sesotec";
				}
				else
				{
					label.Content = this.ViewModelIVP.StrIVP.LocalizedText;
				}
			}
			else
			{
				bool flag3 = design == Definition.GUIDesign.Bizerba;
				if (flag3)
				{
					label.Content = "IVP by Sesotec";
				}
				else
				{
					label.Content = "IVP";
				}
			}
			label.FontSize = 30.0;
			label.FontWeight = FontWeights.Bold;
			label.FontFamily = new FontFamily("Verdana");
			label.Foreground = Brushes.DarkGray;
			label.VerticalAlignment = VerticalAlignment.Center;
			label.HorizontalAlignment = HorizontalAlignment.Right;
			label.Margin = new Thickness(0.0, 0.0, 20.0, 0.0);
			this.UiElementRightAligned = label;
		}

		// Token: 0x06002D58 RID: 11608 RVA: 0x001293A4 File Offset: 0x001275A4
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/submenu/submenu.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x06002D59 RID: 11609 RVA: 0x001293DC File Offset: 0x001275DC
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			if (connectionId != 1)
			{
				this._contentLoaded = true;
			}
			else
			{
				this.lbox = (ListBox)target;
				this.lbox.SelectionChanged += this.lbox_SelectionChanged;
			}
		}

		// Token: 0x04001C50 RID: 7248
		public SubMenuLearnBulkViewModel ViewModelBulk;

		// Token: 0x04001C51 RID: 7249
		private SubMenuNetworkViewModel viewModelNetwork;

		// Token: 0x04001C52 RID: 7250
		private SubMenuCommunicationsViewModel viewModelCommunications;

		// Token: 0x04001C53 RID: 7251
		private SubMenuIVPViewModel ViewModelIVP;

		// Token: 0x04001C54 RID: 7252
		private SubMenuAuditcheckViewModel ViewModelAuditcheck;

		// Token: 0x04001C55 RID: 7253
		private SubMenuMonitoringViewModel viewModelMonitoring;

		// Token: 0x04001C56 RID: 7254
		public UIElement UiElementRightAligned;

		// Token: 0x04001C57 RID: 7255
		private DBConnection dBConnection;

		// Token: 0x04001C59 RID: 7257
		internal ListBox lbox;

		// Token: 0x04001C5A RID: 7258
		private bool _contentLoaded;
	}
}
