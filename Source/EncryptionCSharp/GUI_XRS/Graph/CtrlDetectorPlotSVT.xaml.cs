using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using DatabaseConnection;
using Definitions;
using SesoPlot;
using XrsImage;

namespace GUI_XRS.Graph
{
	// Token: 0x02000271 RID: 625
	public partial class CtrlDetectorPlotSVT : UserControl
	{
		// Token: 0x06003402 RID: 13314 RVA: 0x00146758 File Offset: 0x00144958
		public CtrlDetectorPlotSVT(ParameterHandling parHandling, ImageDataTransfer imageTransfer, AccessRightsManager accessManager, DBConnection dbConnection, MachineState machineState, DBDataLogs dbDataLogs)
		{
			this.InitializeComponent();
			this.viewModel = new DetectorPlotSVTViewModel(parHandling, accessManager, dbConnection, machineState, imageTransfer, dbDataLogs);
			string[] currentPaths = new string[]
			{
				"/hardware/svt/valve01/MaxPixel/",
				"/hardware/svt/valve02/MaxPixel/",
				"/hardware/svt/valve03/MaxPixel/",
				"/hardware/svt/valve04/MaxPixel/",
				"/hardware/svt/valve05/MaxPixel/",
				"/hardware/svt/valve06/MaxPixel/",
				"/hardware/svt/valve07/MaxPixel/",
				"/hardware/svt/valve08/MaxPixel/",
				"/hardware/svt/valve09/MaxPixel/",
				"/hardware/svt/valve10/MaxPixel/",
				"/hardware/svt/valve11/MaxPixel/",
				"/hardware/svt/valve12/MaxPixel/",
				"/hardware/svt/valve13/MaxPixel/",
				"/hardware/svt/valve14/MaxPixel/",
				"/hardware/svt/valve15/MaxPixel/",
				"/hardware/svt/valve16/MaxPixel/",
				"/hardware/svt/valve17/MaxPixel/",
				"/hardware/svt/valve18/MaxPixel/",
				"/hardware/svt/valve19/MaxPixel/",
				"/hardware/svt/valve20/MaxPixel/",
				"/hardware/svt/valve01/MinPixel/",
				"/hardware/svt/valve02/MinPixel/",
				"/hardware/svt/valve03/MinPixel/",
				"/hardware/svt/valve04/MinPixel/",
				"/hardware/svt/valve05/MinPixel/",
				"/hardware/svt/valve06/MinPixel/",
				"/hardware/svt/valve07/MinPixel/",
				"/hardware/svt/valve08/MinPixel/",
				"/hardware/svt/valve09/MinPixel/",
				"/hardware/svt/valve10/MinPixel/",
				"/hardware/svt/valve11/MinPixel/",
				"/hardware/svt/valve12/MinPixel/",
				"/hardware/svt/valve13/MinPixel/",
				"/hardware/svt/valve14/MinPixel/",
				"/hardware/svt/valve15/MinPixel/",
				"/hardware/svt/valve16/MinPixel/",
				"/hardware/svt/valve17/MinPixel/",
				"/hardware/svt/valve18/MinPixel/",
				"/hardware/svt/valve19/MinPixel/",
				"/hardware/svt/valve20/MinPixel/"
			};
			this.InitializeButtons(accessManager, dbConnection);
			for (int i = 0; i < this.plotSVT.Graphs.Count; i++)
			{
				this.plotSVT.Graphs[i].DataContext = this.viewModel;
			}
			this.plotSVT.Graphs[1].GraphType = GraphType.Bar;
			this.plotSVT.Graphs[4].GraphType = GraphType.Bar;
			base.DataContext = this.viewModel;
			this.viewModel.InitializeValveLines();
			this.parameterControl.Initialize(parHandling, currentPaths, false, null, false, null, null, true);
			this.parameterControl.StartCreation();
		}

		// Token: 0x06003403 RID: 13315 RVA: 0x001469A0 File Offset: 0x00144BA0
		private void InitializeButtons(AccessRightsManager accessManager, DBConnection dbConnection)
		{
			MachineMode machineMode = MachineMode.Standard;
			this.PanelButtons = new DockPanel();
			this.PanelButtons.HorizontalAlignment = HorizontalAlignment.Stretch;
			this.PanelButtons.VerticalAlignment = VerticalAlignment.Stretch;
			Grid.SetColumn(this.PanelButtons, 4);
			Button button = new Button();
			button.Content = "Autoscale";
			button.Click += this.BtnAutoscale_Click;
			button.Margin = new Thickness(0.0, 2.0, 10.0, 2.0);
			this.PanelButtons.Children.Add(button);
			DockPanel.SetDock(button, Dock.Left);
			Button button2 = new Button();
			button2.Content = ImageConverter.ToImage("/img/savePNG.png");
			button2.Click += this.btnSavePlot_Click;
			button2.Style = (base.FindResource("defaultButtonStyle") as Style);
			button2.Margin = new Thickness(0.0, 0.0, 10.0, 0.0);
			this.PanelButtons.Children.Add(button2);
			DockPanel.SetDock(button2, Dock.Left);
			dbConnection.GetMachineMode(ref machineMode);
			bool flag = machineMode == MachineMode.Bulk;
			if (flag)
			{
				Button button3 = new Button();
				button3.Content = ImageConverter.ToImage("/img/zoomIn.png");
				button3.Command = this.viewModel.ZoomInXCommand;
				button3.Style = (base.FindResource("defaultButtonStyle") as Style);
				button3.Margin = new Thickness(0.0, 0.0, 10.0, 0.0);
				this.PanelButtons.Children.Add(button3);
				DockPanel.SetDock(button3, Dock.Left);
				Button button4 = new Button();
				button4.Content = ImageConverter.ToImage("/img/zoomOut.png");
				button4.Command = this.viewModel.ZoomOutXCommand;
				button4.Style = (base.FindResource("defaultButtonStyle") as Style);
				button4.Margin = new Thickness(0.0, 0.0, 10.0, 0.0);
				this.PanelButtons.Children.Add(button4);
				DockPanel.SetDock(button4, Dock.Left);
				Button button5 = new Button();
				button5.Content = ImageConverter.ToImage("/img/arrow-right.png");
				button5.Click += this.btnNavigateRight_Click;
				button5.Style = (base.FindResource("defaultButtonStyle") as Style);
				button5.Margin = new Thickness(0.0, 2.0, 10.0, 2.0);
				this.PanelButtons.Children.Add(button5);
				DockPanel.SetDock(button5, Dock.Left);
			}
		}

		// Token: 0x06003404 RID: 13316 RVA: 0x00146CA7 File Offset: 0x00144EA7
		private void btnNavigateRight_Click(object sender, RoutedEventArgs e)
		{
			this.plotSVT.GoToEndX();
		}

		// Token: 0x06003405 RID: 13317 RVA: 0x00146CB8 File Offset: 0x00144EB8
		private void BtnAutoscale_Click(object sender, RoutedEventArgs e)
		{
			this.viewModel.Rescale();
			this.plotSVT.MaximumX = (double)this.viewModel.NPixel;
			this.plotSVT.MinimumX = 0.0;
			this.plotSVT.MajorTicksX = 64.0;
		}

		// Token: 0x06003406 RID: 13318 RVA: 0x00146D14 File Offset: 0x00144F14
		private void btnSavePlot_Click(object sender, RoutedEventArgs e)
		{
			Rect descendantBounds = VisualTreeHelper.GetDescendantBounds(this.chartGridSVT);
			this.viewModel.SavePlot(this.chartGridSVT, descendantBounds);
		}

		// Token: 0x06003407 RID: 13319 RVA: 0x00146D41 File Offset: 0x00144F41
		private void NumberValves_MouseUp(object sender, MouseButtonEventArgs e)
		{
			this.viewModel.getNumKeyboardNumber("Number Valves");
		}

		// Token: 0x06003408 RID: 13320 RVA: 0x00146D55 File Offset: 0x00144F55
		private void OverlappingLeft_MouseUp(object sender, MouseButtonEventArgs e)
		{
			this.viewModel.getNumKeyboardNumber("Overlapping Left");
		}

		// Token: 0x06003409 RID: 13321 RVA: 0x00146D69 File Offset: 0x00144F69
		private void OverlappingRight_MouseUp(object sender, MouseButtonEventArgs e)
		{
			this.viewModel.getNumKeyboardNumber("Overlapping Right");
		}

		// Token: 0x0600340A RID: 13322 RVA: 0x00146D7D File Offset: 0x00144F7D
		private void EndPixelLastValve_MouseUp(object sender, MouseButtonEventArgs e)
		{
			this.viewModel.getNumKeyboardNumber("End Pixel First Valve");
		}

		// Token: 0x0600340B RID: 13323 RVA: 0x00146D91 File Offset: 0x00144F91
		private void StartPixelSecondValve_MouseUp(object sender, MouseButtonEventArgs e)
		{
			this.viewModel.getNumKeyboardNumber("Start Pixel Second Valve");
		}

		// Token: 0x0600340C RID: 13324 RVA: 0x00146DA8 File Offset: 0x00144FA8
		private void SVTPlot_Loaded(object sender, RoutedEventArgs e)
		{
			Subwindow subwindow = Window.GetWindow(this) as Subwindow;
			subwindow.buttonGrid.Children.Remove(subwindow.buttonPanel);
			subwindow.buttonGrid.Children.Add(this.PanelButtons);
		}

		// Token: 0x04001E16 RID: 7702
		private DetectorPlotSVTViewModel viewModel;

		// Token: 0x04001E17 RID: 7703
		public UIElement UiElementRightAligned;

		// Token: 0x04001E18 RID: 7704
		public DockPanel PanelButtons;
	}
}
