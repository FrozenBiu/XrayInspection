using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using DatabaseConnection;
using Definitions;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x020000C1 RID: 193
	public class CtrlComissioning : UserControl, IComponentConnector
	{
		// Token: 0x06001356 RID: 4950 RVA: 0x00071311 File Offset: 0x0006F511
		public CtrlComissioning(DBConnection dbConnection, ParameterHandling parHandling, DBDataLogs dBDataLogs, AuditCheckSettings auditSettings)
		{
			this.InitializeComponent();
			this.dbConnection = dbConnection;
			this.viewModel = new ComissioningViewModel(dbConnection, parHandling, dBDataLogs, auditSettings);
			base.DataContext = this.viewModel;
			this.InitializeButtons();
		}

		// Token: 0x06001357 RID: 4951 RVA: 0x00071350 File Offset: 0x0006F550
		private void InitializeButtons()
		{
			this.PanelSubButtons = new DockPanel();
			this.PanelSubButtons.HorizontalAlignment = HorizontalAlignment.Stretch;
			this.PanelSubButtons.VerticalAlignment = VerticalAlignment.Stretch;
			this.viewModel.btnStartWritingParameter.Content = ImageConverter.ToImage("/img/play.png");
			this.viewModel.btnStartWritingParameter.Style = (base.FindResource("defaultButtonStyle") as Style);
			this.viewModel.btnStartWritingParameter.Command = this.viewModel.StartWritingParameterCommand;
			this.PanelSubButtons.Children.Add(this.viewModel.btnStartWritingParameter);
			DockPanel.SetDock(this.viewModel.btnStartWritingParameter, Dock.Left);
			Button button = new Button();
			button.Content = "";
			button.Style = (base.FindResource("defaultButtonStyle") as Style);
			button.Visibility = Visibility.Hidden;
			button.Width = 10.0;
			this.PanelSubButtons.Children.Add(button);
			this.viewModel.btnOverview.Content = ImageConverter.ToImage("/img/table.png");
			this.viewModel.btnOverview.Command = this.viewModel.ShowOverviewParameterCommand;
			this.viewModel.btnOverview.Style = (base.FindResource("defaultButtonStyle") as Style);
			this.viewModel.btnOverview.Visibility = this.viewModel.getOverviewVisibility();
			this.PanelSubButtons.Children.Add(this.viewModel.btnOverview);
			DockPanel.SetDock(this.viewModel.btnOverview, Dock.Left);
			Button button2 = new Button();
			button2.Content = "";
			button2.Style = (base.FindResource("defaultButtonStyle") as Style);
			button2.Visibility = Visibility.Hidden;
			button2.Width = 10.0;
			this.PanelSubButtons.Children.Add(button2);
			this.viewModel.btnComissioningMultilane.Content = ImageConverter.ToImage("/img/multiLaneProductSettings.png");
			this.viewModel.btnComissioningMultilane.Style = (base.FindResource("defaultButtonStyle") as Style);
			this.viewModel.btnComissioningMultilane.Command = this.viewModel.ShowMultilaneAdjustmentsCommand;
			this.viewModel.btnComissioningMultilane.Visibility = this.viewModel.getMultilaneVisibility();
			this.PanelSubButtons.Children.Add(this.viewModel.btnComissioningMultilane);
			DockPanel.SetDock(this.viewModel.btnComissioningMultilane, Dock.Left);
			Button button3 = new Button();
			button3.Content = "";
			button3.Style = (base.FindResource("defaultButtonStyle") as Style);
			button3.Visibility = Visibility.Hidden;
			button3.Width = 10.0;
			this.PanelSubButtons.Children.Add(button3);
			MachineType machineType = MachineType.MachineTypeUndefined;
			this.dbConnection.GetMachineType(ref machineType);
			bool flag = machineType == MachineType.RayconDPlus || machineType == MachineType.RayconEntry || machineType == MachineType.RayconEX2A;
			this.viewModel.btnComissioningFeaturesLicense.Content = ImageConverter.ToImage("/img/producer_settings.png");
			this.viewModel.btnComissioningFeaturesLicense.Style = (base.FindResource("defaultButtonStyle") as Style);
			this.viewModel.btnComissioningFeaturesLicense.Command = this.viewModel.ShowFeaturesLicenseCommand;
			this.viewModel.btnComissioningFeaturesLicense.Visibility = (flag ? Visibility.Visible : Visibility.Collapsed);
			this.PanelSubButtons.Children.Add(this.viewModel.btnComissioningFeaturesLicense);
			DockPanel.SetDock(this.viewModel.btnComissioningFeaturesLicense, Dock.Left);
			Button button4 = new Button();
			button4.Content = "";
			button4.Style = (base.FindResource("defaultButtonStyle") as Style);
			button4.Visibility = Visibility.Hidden;
			button4.Width = 10.0;
			this.PanelSubButtons.Children.Add(button4);
			this.viewModel.btnAuditCheck.Content = ImageConverter.ToImage("/img/Auditcheck_reset.png");
			this.viewModel.btnAuditCheck.Style = (base.FindResource("defaultButtonStyle") as Style);
			this.viewModel.btnAuditCheck.Command = this.viewModel.ResetAuditCheckCommand;
			this.PanelSubButtons.Children.Add(this.viewModel.btnAuditCheck);
			DockPanel.SetDock(this.viewModel.btnAuditCheck, Dock.Left);
		}

		// Token: 0x06001358 RID: 4952 RVA: 0x000717E8 File Offset: 0x0006F9E8
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/comissioning/ctrlcomissioning.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x06001359 RID: 4953 RVA: 0x0007181E File Offset: 0x0006FA1E
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			this._contentLoaded = true;
		}

		// Token: 0x04000C9C RID: 3228
		private ComissioningViewModel viewModel;

		// Token: 0x04000C9D RID: 3229
		public DockPanel PanelSubButtons;

		// Token: 0x04000C9E RID: 3230
		private DBConnection dbConnection;

		// Token: 0x04000C9F RID: 3231
		private bool _contentLoaded;
	}
}
