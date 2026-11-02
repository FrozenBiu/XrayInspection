using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using DatabaseConnection;
using Definitions;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x02000085 RID: 133
	public class MonitoringView : UserControl, IComponentConnector, IStyleConnector
	{
		// Token: 0x06000D18 RID: 3352 RVA: 0x00050558 File Offset: 0x0004E758
		public MonitoringView(AccessRightsManager accessManager, DataModesMonitoring mode, DBDataLogs dBDataLogs, DBConnection dbConnection, ParameterHandling parameterHandling, ErrorSurvey errorSurvey, MainWindowViewModel mainWindow)
		{
			this.accessManager = accessManager;
			this.dBDataLogs = dBDataLogs;
			this.dbConnection = dbConnection;
			this.parameterHandling = parameterHandling;
			this.mainWindow = mainWindow;
			this.InitializeComponent();
			bool flag = accessManager.CurrentUser.Rights >= BuiltInPriviledges.Admin;
			bool flag2 = flag;
			switch (mode)
			{
			case DataModesMonitoring.xray:
				this.viewModel = new XRayParametersViewModel(dBDataLogs, parameterHandling, dbConnection, flag2, errorSurvey);
				break;
			case DataModesMonitoring.detector:
				this.viewModel = new DetectorParametersViewModel(dBDataLogs, dbConnection, parameterHandling, flag2, errorSurvey);
				this.visibilityBackgroundMonBtn = true;
				break;
			case DataModesMonitoring.conveyor:
				this.viewModel = new ConveyorParametersViewModel(dBDataLogs, dbConnection, parameterHandling, flag2, errorSurvey);
				this.visibilityBeltContaminationBtn = true;
				break;
			case DataModesMonitoring.rejecter:
				this.viewModel = new RejectParametersViewModel(dBDataLogs, dbConnection, parameterHandling, flag2, errorSurvey);
				break;
			case DataModesMonitoring.operational:
				this.viewModel = new OperationalParametersViewModel(dBDataLogs, dbConnection, parameterHandling, flag2, errorSurvey, mainWindow);
				break;
			case DataModesMonitoring.background:
				this.viewModel = new BackgroundMonitoringViewModel(dBDataLogs, dbConnection, parameterHandling, flag2);
				this.visibilityDiagrammBtn = false;
				break;
			case DataModesMonitoring.livebackground:
				this.viewModel = new BackgroundLiveMonitoringViewModel(dBDataLogs, dbConnection, parameterHandling, flag2);
				this.visibilityDiagrammBtn = false;
				break;
			default:
				this.viewModel = new OperationalParametersViewModel(dBDataLogs, dbConnection, parameterHandling, flag2, errorSurvey, mainWindow);
				break;
			}
			base.DataContext = this.viewModel;
			this.InitializeButtons();
			this.SetControlRights();
		}

		// Token: 0x06000D19 RID: 3353 RVA: 0x000506F0 File Offset: 0x0004E8F0
		private void TextBox_Limit1(object sender, MouseButtonEventArgs e)
		{
			bool flag = e.Source is TextBox;
			if (flag)
			{
				MonitoringItem pItem = (MonitoringItem)((TextBox)e.Source).DataContext;
				this.viewModel.getNumKeyboardNumber(pItem, 1);
			}
		}

		// Token: 0x06000D1A RID: 3354 RVA: 0x00050738 File Offset: 0x0004E938
		private void TextBox_Limit2(object sender, MouseButtonEventArgs e)
		{
			bool flag = e.Source is TextBox;
			if (flag)
			{
				MonitoringItem pItem = (MonitoringItem)((TextBox)e.Source).DataContext;
				this.viewModel.getNumKeyboardNumber(pItem, 2);
			}
		}

		// Token: 0x06000D1B RID: 3355 RVA: 0x00050780 File Offset: 0x0004E980
		private void SetControlRights()
		{
			bool flag = this.visibilityDiagrammBtn;
			if (flag)
			{
				this.accessManager.RegisterControl(this.diagrammBtn, this.rightDiagramm, this.rightDiagramm);
				this.accessManager.SetProperties();
			}
		}

		// Token: 0x06000D1C RID: 3356 RVA: 0x000507C4 File Offset: 0x0004E9C4
		private void InitializeButtons()
		{
			this.dockpanel = new DockPanel();
			this.dockpanel.HorizontalAlignment = HorizontalAlignment.Stretch;
			this.dockpanel.VerticalAlignment = VerticalAlignment.Stretch;
			Grid.SetColumn(this.dockpanel, 4);
			bool flag = this.visibilityDiagrammBtn;
			if (flag)
			{
				this.diagrammBtn = new Button();
				this.diagrammBtn.Content = ImageConverter.ToImage("/img/graphplot_small.png");
				this.diagrammBtn.Click += this.ShowDiagramm;
				this.diagrammBtn.Style = (base.FindResource("defaultButtonStyle") as Style);
				this.diagrammBtn.Margin = new Thickness(0.0, 0.0, 10.0, 0.0);
				this.diagrammBtn.Visibility = Visibility.Visible;
				this.dockpanel.Children.Add(this.diagrammBtn);
				DockPanel.SetDock(this.diagrammBtn, Dock.Left);
			}
			bool flag2 = this.parameterHandling.ReadPar(Definition.detectorMonitoringActive) == "1";
			if (flag2)
			{
				bool flag3 = this.visibilityBackgroundMonBtn;
				if (flag3)
				{
					this.backgroundMonBtn = new Button();
					this.backgroundMonBtn.Content = ImageConverter.ToImage("/img/backgroundindex_menu.png");
					this.backgroundMonBtn.Click += this.ShowBackgroundMonitoring;
					this.backgroundMonBtn.Style = (base.FindResource("defaultButtonStyle") as Style);
					this.backgroundMonBtn.Margin = new Thickness(0.0, 0.0, 10.0, 0.0);
					this.backgroundMonBtn.Visibility = Visibility.Visible;
					this.dockpanel.Children.Add(this.backgroundMonBtn);
					DockPanel.SetDock(this.backgroundMonBtn, Dock.Left);
				}
				bool flag4 = this.visibilityBeltContaminationBtn;
				if (flag4)
				{
					this.backgroundLiveMonBtn = new Button();
					this.backgroundLiveMonBtn.Content = ImageConverter.ToImage("/img/belt_contaminant_monitoring.png");
					this.backgroundLiveMonBtn.Click += this.ShowLiveBackgroundMonitoring;
					this.backgroundLiveMonBtn.Style = (base.FindResource("defaultButtonStyle") as Style);
					this.backgroundLiveMonBtn.Margin = new Thickness(0.0, 0.0, 10.0, 0.0);
					this.backgroundLiveMonBtn.Visibility = Visibility.Visible;
					this.dockpanel.Children.Add(this.backgroundLiveMonBtn);
					DockPanel.SetDock(this.backgroundLiveMonBtn, Dock.Left);
				}
			}
		}

		// Token: 0x06000D1D RID: 3357 RVA: 0x00050A85 File Offset: 0x0004EC85
		private void ShowDiagramm(object sender, RoutedEventArgs e)
		{
			this.viewModel.ShowDiagramm();
		}

		// Token: 0x06000D1E RID: 3358 RVA: 0x00050A94 File Offset: 0x0004EC94
		private void ShowBackgroundMonitoring(object sender, RoutedEventArgs e)
		{
			MonitoringView usrControl = new MonitoringView(this.accessManager, DataModesMonitoring.background, this.dBDataLogs, this.dbConnection, this.parameterHandling, null, this.mainWindow);
			Subwindow.Show(usrControl, SubwindowButtons.BackAndHome);
		}

		// Token: 0x06000D1F RID: 3359 RVA: 0x00050AD0 File Offset: 0x0004ECD0
		private void ShowLiveBackgroundMonitoring(object sender, RoutedEventArgs e)
		{
			MonitoringView usrControl = new MonitoringView(this.accessManager, DataModesMonitoring.livebackground, this.dBDataLogs, this.dbConnection, this.parameterHandling, null, this.mainWindow);
			Subwindow.Show(usrControl, SubwindowButtons.BackAndHome);
		}

		// Token: 0x06000D20 RID: 3360 RVA: 0x00050B0C File Offset: 0x0004ED0C
		private void Monitoring_Unloaded(object sender, RoutedEventArgs e)
		{
			bool flag = this.visibilityDiagrammBtn;
			if (flag)
			{
				this.accessManager.DeregisterControl(this.diagrammBtn);
			}
			BindingOperations.ClearAllBindings(this);
			base.DataContext = null;
			GC.Collect();
		}

		// Token: 0x06000D21 RID: 3361 RVA: 0x00050B50 File Offset: 0x0004ED50
		private void Monitoring_Loaded(object sender, RoutedEventArgs e)
		{
			bool flag = this.visibilityDiagrammBtn;
			if (flag)
			{
				Subwindow subwindow = Window.GetWindow(this) as Subwindow;
				subwindow.buttonGrid.Children.Remove(subwindow.buttonPanel);
				subwindow.buttonGrid.Children.Add(this.dockpanel);
			}
		}

		// Token: 0x06000D22 RID: 3362 RVA: 0x00050BA4 File Offset: 0x0004EDA4
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/monitoring/monitoringview.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x06000D23 RID: 3363 RVA: 0x00050BDC File Offset: 0x0004EDDC
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
				((MonitoringView)target).Unloaded += this.Monitoring_Unloaded;
				((MonitoringView)target).Loaded += this.Monitoring_Loaded;
			}
		}

		// Token: 0x06000D24 RID: 3364 RVA: 0x00050C30 File Offset: 0x0004EE30
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IStyleConnector.Connect(int connectionId, object target)
		{
			if (connectionId == 2)
			{
				((TextBox)target).MouseUp += this.TextBox_Limit1;
			}
		}

		// Token: 0x04000890 RID: 2192
		private MonitoringViewModel viewModel;

		// Token: 0x04000891 RID: 2193
		private AccessRightsManager accessManager;

		// Token: 0x04000892 RID: 2194
		private DBDataLogs dBDataLogs;

		// Token: 0x04000893 RID: 2195
		private DBConnection dbConnection;

		// Token: 0x04000894 RID: 2196
		private ParameterHandling parameterHandling;

		// Token: 0x04000895 RID: 2197
		private DockPanel dockpanel;

		// Token: 0x04000896 RID: 2198
		private Button diagrammBtn;

		// Token: 0x04000897 RID: 2199
		private Button backgroundMonBtn;

		// Token: 0x04000898 RID: 2200
		private Button backgroundLiveMonBtn;

		// Token: 0x04000899 RID: 2201
		private bool visibilityDiagrammBtn = true;

		// Token: 0x0400089A RID: 2202
		private bool visibilityBackgroundMonBtn = false;

		// Token: 0x0400089B RID: 2203
		private bool visibilityBeltContaminationBtn = false;

		// Token: 0x0400089C RID: 2204
		private BuiltInPriviledges rightDiagramm = BuiltInPriviledges.Operator1;

		// Token: 0x0400089D RID: 2205
		private MainWindowViewModel mainWindow;

		// Token: 0x0400089E RID: 2206
		private bool _contentLoaded;
	}
}
