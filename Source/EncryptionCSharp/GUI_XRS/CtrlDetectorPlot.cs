using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;
using System.Windows.Media;
using DatabaseConnection;
using Definitions;
using SesoPlot;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x02000116 RID: 278
	public class CtrlDetectorPlot : UserControl, IComponentConnector
	{
		// Token: 0x06001A99 RID: 6809 RVA: 0x0009EEC0 File Offset: 0x0009D0C0
		public CtrlDetectorPlot(ParameterHandling parHandling, ImageDataTransfer imageTransfer, AccessRightsManager accessManager, DBConnection dbConnection, MachineState machineState, XrayState xrayState)
		{
			this.InitializeComponent();
			this.viewModel = new DetectorPlotViewModel(parHandling, accessManager, dbConnection, machineState, imageTransfer, xrayState);
			this.InitializeButtons(accessManager, dbConnection);
			bool flag = accessManager.CurrentUser.Rights >= this.rightPlotEdit;
			if (flag)
			{
				this.parameterControl.Initialize(parHandling, false, null, false, null, null);
				this.parameterControl.StartCreation();
				this.parameterControl.DockNumKeyboard = true;
			}
			for (int i = 0; i < this.plot.Graphs.Count; i++)
			{
				this.plot.Graphs[i].DataContext = this.viewModel;
			}
			base.DataContext = this.viewModel;
		}

		// Token: 0x06001A9A RID: 6810 RVA: 0x0009EFA4 File Offset: 0x0009D1A4
		private void InitializeButtons(AccessRightsManager accessManager, DBConnection dbConnection)
		{
			MachineMode machineMode = MachineMode.Standard;
			this.PanelSubButtons = new DockPanel();
			this.PanelSubButtons.HorizontalAlignment = HorizontalAlignment.Stretch;
			this.PanelSubButtons.VerticalAlignment = VerticalAlignment.Stretch;
			Button button = new Button();
			button.Content = "Autoscale";
			button.Command = this.viewModel.RescaleCommand;
			button.Margin = new Thickness(0.0, 2.0, 10.0, 2.0);
			this.PanelSubButtons.Children.Add(button);
			DockPanel.SetDock(button, Dock.Left);
			Button button2 = new Button();
			button2.Content = ImageConverter.ToImage("/img/savePNG.png");
			button2.Click += this.btnSavePlot_Click;
			button2.Style = (base.FindResource("defaultButtonStyle") as Style);
			button2.Margin = new Thickness(0.0, 0.0, 10.0, 0.0);
			this.PanelSubButtons.Children.Add(button2);
			DockPanel.SetDock(button2, Dock.Left);
			bool flag = accessManager.CurrentUser.Rights >= this.rightSaveReferencePlot;
			if (flag)
			{
				Button button3 = new Button();
				button3.Content = ImageConverter.ToImage("/img/referencePlot.png");
				button3.Command = this.viewModel.SaveReferencePlotCommand;
				button3.Style = (base.FindResource("defaultButtonStyle") as Style);
				button3.Margin = new Thickness(0.0, 0.0, 10.0, 0.0);
				this.PanelSubButtons.Children.Add(button3);
				DockPanel.SetDock(button3, Dock.Left);
			}
			dbConnection.GetMachineMode(ref machineMode);
			bool flag2 = machineMode == MachineMode.Bulk;
			if (flag2)
			{
				Button button4 = new Button();
				button4.Content = ImageConverter.ToImage("/img/zoomIn.png");
				button4.Command = this.viewModel.ZoomInXCommand;
				button4.Style = (base.FindResource("defaultButtonStyle") as Style);
				button4.Margin = new Thickness(0.0, 0.0, 10.0, 0.0);
				this.PanelSubButtons.Children.Add(button4);
				DockPanel.SetDock(button4, Dock.Left);
				Button button5 = new Button();
				button5.Content = ImageConverter.ToImage("/img/zoomOut.png");
				button5.Command = this.viewModel.ZoomOutXCommand;
				button5.Style = (base.FindResource("defaultButtonStyle") as Style);
				button5.Margin = new Thickness(0.0, 0.0, 10.0, 0.0);
				this.PanelSubButtons.Children.Add(button5);
				DockPanel.SetDock(button5, Dock.Left);
			}
			bool isDualEnergyDetector = this.viewModel.IsDualEnergyDetector;
			if (isDualEnergyDetector)
			{
				ComboBox comboBox = new ComboBox();
				comboBox.FontSize = 22.0;
				comboBox.Margin = new Thickness(5.0, 0.0, 10.0, 0.0);
				comboBox.Items.Add(EnergyLevelMode.AllEnergies);
				comboBox.Items.Add(EnergyLevelMode.LowEnergy);
				comboBox.Items.Add(EnergyLevelMode.HighEnergy);
				comboBox.SelectedIndex = 0;
				comboBox.SelectionChanged += this.viewModel.EnergyModeChanged;
				this.PanelSubButtons.Children.Add(comboBox);
				DockPanel.SetDock(comboBox, Dock.Left);
			}
			bool isTdiDetector = this.viewModel.IsTdiDetector;
			if (isTdiDetector)
			{
				this.InitTdiButtons();
			}
			Button button6 = new Button();
			button6.Content = ImageConverter.ToImage("/img/snrPlot.png");
			button6.Command = this.viewModel.ShowSnrCommand;
			button6.Style = (base.FindResource("defaultButtonStyle") as Style);
			button6.Margin = new Thickness(0.0, 0.0, 10.0, 0.0);
			this.PanelSubButtons.Children.Add(button6);
			DockPanel.SetDock(button6, Dock.Left);
			bool isTdiDetector2 = this.viewModel.IsTdiDetector;
			if (isTdiDetector2)
			{
				Button button7 = new Button();
				button7.Content = "Defect Pixel";
				bool flag3 = MeanPixelLogger.noTdiDefectPixels.Count > 0;
				if (flag3)
				{
					Button button8 = button7;
					object content = button8.Content;
					button8.Content = ((content != null) ? content.ToString() : null) + " " + MeanPixelLogger.noTdiDefectPixels.Count.ToString();
				}
				button7.Margin = new Thickness(0.0, 2.0, 10.0, 2.0);
				button7.ContextMenu = new ContextMenu();
				button7.Click += this.BtnDefectPixel_Click;
				TextBox textBox = new TextBox();
				textBox.FontSize = 14.0;
				textBox.Width = 600.0;
				textBox.Height = 200.0;
				textBox.TextAlignment = TextAlignment.Left;
				textBox.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
				string text = "";
				foreach (DTNetNoTdiDefectPixel dtnetNoTdiDefectPixel in MeanPixelLogger.noTdiDefectPixels)
				{
					text = text + dtnetNoTdiDefectPixel.ToStringAsLine() + "\n";
				}
				text += "\n";
				text += MeanPixelLogger.tdiDefectPixelList.ToString();
				bool flag4 = text == "\n";
				if (flag4)
				{
					text = "- No defect pixels -";
				}
				textBox.Text = text;
				button7.ContextMenu.Items.Add(textBox);
				this.PanelSubButtons.Children.Add(button7);
				DockPanel.SetDock(button7, Dock.Left);
			}
			this.UiElementRightAligned = new Label
			{
				Content = "Signal",
				FontSize = 32.0,
				FontWeight = FontWeights.Bold,
				FontFamily = new FontFamily("Verdana"),
				Foreground = Brushes.DarkGray,
				VerticalAlignment = VerticalAlignment.Center,
				HorizontalAlignment = HorizontalAlignment.Right,
				Margin = new Thickness(0.0, 0.0, 50.0, 0.0)
			};
		}

		// Token: 0x06001A9B RID: 6811 RVA: 0x0009F6B8 File Offset: 0x0009D8B8
		private void InitTdiButtons()
		{
			Grid grid = new Grid();
			grid.Margin = new Thickness(0.0, 5.0, 10.0, 0.0);
			RowDefinition rowDefinition = new RowDefinition();
			rowDefinition.Height = new GridLength(0.0, GridUnitType.Auto);
			RowDefinition rowDefinition2 = new RowDefinition();
			rowDefinition2.Height = new GridLength(0.0, GridUnitType.Auto);
			ColumnDefinition value = new ColumnDefinition();
			ColumnDefinition value2 = new ColumnDefinition();
			grid.RowDefinitions.Add(rowDefinition);
			grid.RowDefinitions.Add(rowDefinition2);
			grid.ColumnDefinitions.Add(value);
			grid.ColumnDefinitions.Add(value2);
			CheckBox checkBox = new CheckBox();
			checkBox.Height = 45.0;
			checkBox.Width = 45.0;
			checkBox.Margin = new Thickness(2.0);
			checkBox.IsChecked = new bool?(true);
			checkBox.DataContext = this.viewModel;
			checkBox.SetBinding(ToggleButton.IsCheckedProperty, "IsTdiScanMode");
			CheckBox checkBox2 = new CheckBox();
			checkBox2.Height = 45.0;
			checkBox2.Width = 45.0;
			checkBox2.Margin = new Thickness(2.0);
			checkBox2.IsChecked = new bool?(false);
			checkBox2.DataContext = this.viewModel;
			checkBox2.SetBinding(ToggleButton.IsCheckedProperty, "IsNoTdiScanMode");
			Grid.SetColumn(checkBox2, 1);
			Label label = new Label();
			label.Content = "TDI";
			label.FontSize = 12.0;
			label.FontWeight = FontWeights.Bold;
			label.FontFamily = new FontFamily("Verdana");
			label.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF555555");
			label.VerticalAlignment = VerticalAlignment.Top;
			label.HorizontalAlignment = HorizontalAlignment.Center;
			label.Margin = new Thickness(0.0, -5.0, 0.0, 0.0);
			Grid.SetRow(label, 1);
			Grid.SetColumn(label, 0);
			Label label2 = new Label();
			label2.Content = "NoTDI";
			label2.FontSize = 12.0;
			label2.FontWeight = FontWeights.Bold;
			label2.FontFamily = new FontFamily("Verdana");
			label2.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF555555");
			label2.VerticalAlignment = VerticalAlignment.Top;
			label2.HorizontalAlignment = HorizontalAlignment.Center;
			label2.Margin = new Thickness(0.0, -5.0, 0.0, 0.0);
			Grid.SetRow(label2, 1);
			Grid.SetColumn(label2, 1);
			grid.Children.Add(checkBox);
			grid.Children.Add(checkBox2);
			grid.Children.Add(label);
			grid.Children.Add(label2);
			this.PanelSubButtons.Children.Add(grid);
			DockPanel.SetDock(grid, Dock.Left);
		}

		// Token: 0x06001A9C RID: 6812 RVA: 0x0009FA18 File Offset: 0x0009DC18
		private void btnSavePlot_Click(object sender, RoutedEventArgs e)
		{
			Rect descendantBounds = VisualTreeHelper.GetDescendantBounds(this.chartGrid);
			this.viewModel.SavePlot(this.chartGrid, descendantBounds);
		}

		// Token: 0x06001A9D RID: 6813 RVA: 0x0009FA45 File Offset: 0x0009DC45
		private void BtnDefectPixel_Click(object sender, RoutedEventArgs e)
		{
			((Button)sender).ContextMenu.IsOpen = true;
		}

		// Token: 0x06001A9E RID: 6814 RVA: 0x0009FA5C File Offset: 0x0009DC5C
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/graph/ctrldetectorplot.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x06001A9F RID: 6815 RVA: 0x0009FA94 File Offset: 0x0009DC94
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		internal Delegate _CreateDelegate(Type delegateType, string handler)
		{
			return Delegate.CreateDelegate(delegateType, this, handler);
		}

		// Token: 0x06001AA0 RID: 6816 RVA: 0x0009FAB0 File Offset: 0x0009DCB0
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				this.parameterControl = (CtrlParameter)target;
				break;
			case 2:
				this.chartGrid = (Grid)target;
				break;
			case 3:
				this.plot = (SesoPlot)target;
				break;
			case 4:
				this.gridDtParameters = (Grid)target;
				break;
			default:
				this._contentLoaded = true;
				break;
			}
		}

		// Token: 0x040011B3 RID: 4531
		private BuiltInPriviledges rightPlotEdit = BuiltInPriviledges.Service;

		// Token: 0x040011B4 RID: 4532
		private BuiltInPriviledges rightSaveReferencePlot = BuiltInPriviledges.Service;

		// Token: 0x040011B5 RID: 4533
		private DetectorPlotViewModel viewModel;

		// Token: 0x040011B6 RID: 4534
		public DockPanel PanelSubButtons;

		// Token: 0x040011B7 RID: 4535
		public UIElement UiElementRightAligned;

		// Token: 0x040011B8 RID: 4536
		internal CtrlParameter parameterControl;

		// Token: 0x040011B9 RID: 4537
		internal Grid chartGrid;

		// Token: 0x040011BA RID: 4538
		internal SesoPlot plot;

		// Token: 0x040011BB RID: 4539
		internal Grid gridDtParameters;

		// Token: 0x040011BC RID: 4540
		private bool _contentLoaded;
	}
}
