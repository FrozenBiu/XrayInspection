using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using DatabaseConnection;
using Definitions;
using Microsoft.Windows.Controls;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x020000D5 RID: 213
	public class CtrlTestcardSimMonitoringOverview : UserControl, IComponentConnector
	{
		// Token: 0x060014EB RID: 5355 RVA: 0x0007D368 File Offset: 0x0007B568
		public CtrlTestcardSimMonitoringOverview(ParameterHandling parHandling, TestcardSimService testcardSimService, DBConnection dbConnection, ProductManager productManager, ErrorHandling errorHandling, AccessRightsManager accessManager)
		{
			this.productManager = productManager;
			this.dBConnection = dbConnection;
			this.InitializeComponent();
			this.viewModel = new TestcardSimMonitoringOverviewViewModel(parHandling, testcardSimService, dbConnection, productManager, errorHandling, accessManager);
			this.InitializeButtons();
			base.DataContext = this.viewModel;
			this.VaGlassTimeStpDataGrid.ItemsSource = TestcardSimMonitoringOverviewViewModel.SensitivityMonitoringParams;
		}

		// Token: 0x060014EC RID: 5356 RVA: 0x0007D3CC File Offset: 0x0007B5CC
		private void InitializeButtons()
		{
			this.PanelButtons = new DockPanel();
			this.PanelButtons.HorizontalAlignment = HorizontalAlignment.Stretch;
			this.PanelButtons.VerticalAlignment = VerticalAlignment.Stretch;
			this.viewModel.btnCsvExport.Content = ImageConverter.ToImage("/img/saveCSV.png");
			this.viewModel.btnCsvExport.Click += this.ExportinCSV;
			this.viewModel.btnCsvExport.Style = (base.FindResource("defaultButtonStyle") as Style);
			this.viewModel.btnCsvExport.Margin = new Thickness(0.0, 2.0, 10.0, 2.0);
			this.PanelButtons.Children.Add(this.viewModel.btnCsvExport);
			DockPanel.SetDock(this.viewModel.btnCsvExport, Dock.Left);
			Grid element = new Grid();
			this.PanelButtons.Children.Add(element);
			DockPanel.SetDock(element, Dock.Left);
			Label label = new Label();
			Definition.GUIDesign design = this.dBConnection.GetDesign();
			bool flag = design == Definition.GUIDesign.Bizerba;
			if (flag)
			{
				label.Content = this.viewModel.StrMonitoring.LocalizedText + " by Sesotec";
			}
			else
			{
				label.Content = this.viewModel.StrMonitoring.LocalizedText;
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

		// Token: 0x060014ED RID: 5357 RVA: 0x0007D5B8 File Offset: 0x0007B7B8
		private void ExportinCSV(object sender, RoutedEventArgs e)
		{
			this.viewModel.CreateCsvForICMSensitivities(this.productManager);
		}

		// Token: 0x060014EE RID: 5358 RVA: 0x0007D5D0 File Offset: 0x0007B7D0
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/testcardsimulation/ctrltestcardsimmonitoringoverview.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x060014EF RID: 5359 RVA: 0x0007D608 File Offset: 0x0007B808
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
				this.VaGlassTimeStpDataGrid = (Microsoft.Windows.Controls.DataGrid)target;
			}
		}

		// Token: 0x04000DE5 RID: 3557
		private TestcardSimMonitoringOverviewViewModel viewModel;

		// Token: 0x04000DE6 RID: 3558
		public DockPanel PanelButtons;

		// Token: 0x04000DE7 RID: 3559
		public UIElement UiElementRightAligned;

		// Token: 0x04000DE8 RID: 3560
		private ProductManager productManager;

		// Token: 0x04000DE9 RID: 3561
		private DBConnection dBConnection;

		// Token: 0x04000DEA RID: 3562
		internal Microsoft.Windows.Controls.DataGrid VaGlassTimeStpDataGrid;

		// Token: 0x04000DEB RID: 3563
		private bool _contentLoaded;
	}
}
