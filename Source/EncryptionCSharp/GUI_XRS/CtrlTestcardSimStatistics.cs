using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using DatabaseConnection;
using Definitions;
using SesoPlot;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x020000DE RID: 222
	public class CtrlTestcardSimStatistics : UserControl, IComponentConnector
	{
		// Token: 0x060015E0 RID: 5600 RVA: 0x00083AF4 File Offset: 0x00081CF4
		public CtrlTestcardSimStatistics(ParameterHandling parHandling, string selectedimages, int simulatedimages, ProductManager productManager, DBConnection dbConnection, string simmode, TestcardSimService testcardSimService)
		{
			this.parHandling = parHandling;
			this.productManager = productManager;
			this.dbConnection = dbConnection;
			this.simmode = simmode;
			this.testcardSimService = testcardSimService;
			this.InitializeComponent();
			this.viewModel = new TestcardSimStatisticsViewModel(parHandling, selectedimages, simulatedimages, dbConnection, productManager, simmode);
			base.DataContext = this.viewModel;
			this.checkIfCurvesAvailable = true;
			this.DrawSensitivityCurves(productManager.CurrentLoadedProduct.TableName, simmode);
			this.InitializeButtons();
		}

		// Token: 0x060015E1 RID: 5601 RVA: 0x00083B7C File Offset: 0x00081D7C
		private void InitializeButtons()
		{
			this.PanelButtons = new DockPanel();
			this.PanelButtons.HorizontalAlignment = HorizontalAlignment.Stretch;
			this.PanelButtons.VerticalAlignment = VerticalAlignment.Stretch;
			bool flag = (this.simmode == "0" || this.simmode == "2") && this.checkIfCurvesAvailable && !this.testcardSimService.ProductChangedForStatisticTable;
			if (flag)
			{
				this.viewModel.btnStatisticTable.Content = ImageConverter.ToImage("/img/table.png");
				this.viewModel.btnStatisticTable.Click += this.OpenStatisticTable;
				this.viewModel.btnStatisticTable.Style = (base.FindResource("defaultButtonStyle") as Style);
				this.viewModel.btnStatisticTable.Margin = new Thickness(0.0, 2.0, 10.0, 2.0);
				this.PanelButtons.Children.Add(this.viewModel.btnStatisticTable);
				DockPanel.SetDock(this.viewModel.btnStatisticTable, Dock.Left);
			}
			this.viewModel.btnCsvExport.Content = ImageConverter.ToImage("/img/saveCSV.png");
			this.viewModel.btnCsvExport.Click += this.ExportinCSV;
			this.viewModel.btnCsvExport.Style = (base.FindResource("defaultButtonStyle") as Style);
			this.viewModel.btnCsvExport.Margin = new Thickness(0.0, 2.0, 10.0, 2.0);
			this.PanelButtons.Children.Add(this.viewModel.btnCsvExport);
			DockPanel.SetDock(this.viewModel.btnCsvExport, Dock.Left);
			this.viewModel.btnPngExport.Content = ImageConverter.ToImage("/img/savePNG.png");
			this.viewModel.btnPngExport.Click += this.ExportinPNG;
			this.viewModel.btnPngExport.Style = (base.FindResource("defaultButtonStyle") as Style);
			this.viewModel.btnPngExport.Margin = new Thickness(0.0, 2.0, 10.0, 2.0);
			this.PanelButtons.Children.Add(this.viewModel.btnPngExport);
			DockPanel.SetDock(this.viewModel.btnPngExport, Dock.Left);
			Grid element = new Grid();
			this.PanelButtons.Children.Add(element);
			DockPanel.SetDock(element, Dock.Left);
			Label label = new Label();
			Definition.GUIDesign design = this.dbConnection.GetDesign();
			bool flag2 = design == Definition.GUIDesign.Bizerba;
			if (flag2)
			{
				label.Content = this.viewModel.StrStatistics.LocalizedText + " by Sesotec";
			}
			else
			{
				label.Content = this.viewModel.StrStatistics.LocalizedText;
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

		// Token: 0x060015E2 RID: 5602 RVA: 0x00083F34 File Offset: 0x00082134
		private void DrawSensitivityCurves(string tablename, string mode)
		{
			List<int> list = new List<int>();
			List<int> list2 = new List<int>();
			List<string> list3 = new List<string>();
			Graph item = new Graph();
			this.plotVA.Graphs = new ObservableCollection<Graph>();
			this.plotGL.Graphs = new ObservableCollection<Graph>();
			for (int i = 0; i < 6; i++)
			{
				bool flag = i < 3;
				if (flag)
				{
					this.plotVA.Graphs.Add(item);
				}
				else
				{
					this.plotGL.Graphs.Add(item);
				}
				item = new Graph();
			}
			this.plotPointsVA = new List<Point>();
			this.plotPointsGL = new List<Point>();
			this.sensitivityLine = new List<Point>();
			Point item2 = default(Point);
			Point item3 = default(Point);
			Point item4 = default(Point);
			for (int j = 0; j < Definition.sumOfGlassTestcards + 2; j++)
			{
				item4.X = (double)j;
				item4.Y = 95.0;
				this.sensitivityLine.Add(item4);
			}
			this.plotVA.Graphs[0].Points = this.sensitivityLine;
			this.plotVA.Graphs[0].Stroke = Brushes.Gray;
			this.plotVA.Graphs[0].StrokeThickness = 2.0;
			this.plotVA.MaximumX = 9.2;
			this.plotGL.Graphs[0].Points = this.sensitivityLine;
			this.plotGL.Graphs[0].Stroke = Brushes.Gray;
			this.plotGL.Graphs[0].StrokeThickness = 2.0;
			this.plotGL.MaximumX = 15.3;
			this.dbConnection.GetTestcardSimSensitivities(tablename, list3);
			bool flag2 = list3.Count != 0;
			if (flag2)
			{
				for (int k = 0; k < Definition.sumOfGlassTestcards; k++)
				{
					bool flag3 = k < Definition.sumOfVATestcards;
					if (flag3)
					{
						list.Add(Convert.ToInt32(list3[k]));
					}
					list2.Add(Convert.ToInt32(list3[k + Definition.sumOfVATestcards]));
				}
			}
			else
			{
				this.checkIfCurvesAvailable = false;
			}
			bool flag4 = list != null && list.Count != 0;
			if (flag4)
			{
				for (int l = 0; l < list.Count + 1; l++)
				{
					bool flag5 = l == 0;
					if (flag5)
					{
						item2.X = 0.0;
						item2.Y = 0.0;
					}
					else
					{
						item2.X = (double)l;
						item2.Y = (double)list[l - 1];
					}
					this.plotPointsVA.Add(item2);
				}
				this.plotVA.Graphs[1].GraphType = GraphType.Linear;
				this.plotVA.Graphs[1].Points = this.plotPointsVA;
				this.plotVA.Graphs[1].Stroke = Brushes.Green;
				this.plotVA.Graphs[1].StrokeThickness = 3.0;
				this.plotVA.Graphs[2].GraphType = GraphType.Scatter;
				this.plotVA.Graphs[2].Points = this.plotPointsVA;
				this.plotVA.Graphs[2].Stroke = Brushes.Green;
				this.plotVA.Graphs[2].StrokeThickness = 5.0;
			}
			bool flag6 = list2 != null && list2.Count != 0;
			if (flag6)
			{
				for (int m = 0; m < list2.Count + 1; m++)
				{
					bool flag7 = m == 0;
					if (flag7)
					{
						item3.X = 0.0;
						item3.Y = 0.0;
					}
					else
					{
						item3.X = (double)m;
						item3.Y = (double)list2[m - 1];
					}
					this.plotPointsGL.Add(item3);
				}
				this.plotGL.Graphs[1].GraphType = GraphType.Linear;
				this.plotGL.Graphs[1].Points = this.plotPointsGL;
				this.plotGL.Graphs[1].Stroke = Brushes.Green;
				this.plotGL.Graphs[1].StrokeThickness = 3.0;
				this.plotGL.Graphs[2].GraphType = GraphType.Scatter;
				this.plotGL.Graphs[2].Points = this.plotPointsGL;
				this.plotGL.Graphs[2].Stroke = Brushes.Green;
				this.plotGL.Graphs[2].StrokeThickness = 5.0;
			}
		}

		// Token: 0x060015E3 RID: 5603 RVA: 0x0008449D File Offset: 0x0008269D
		private void ExportinCSV(object sender, RoutedEventArgs e)
		{
			this.viewModel.CreateCsvForStatistics(this.productManager);
		}

		// Token: 0x060015E4 RID: 5604 RVA: 0x000844B4 File Offset: 0x000826B4
		private void ExportinPNG(object sender, RoutedEventArgs e)
		{
			Rect descendantBounds = VisualTreeHelper.GetDescendantBounds(this.graphesVAGlass);
			this.viewModel.CreatePNGForStatistics(this.graphesVAGlass, descendantBounds, this.productManager);
		}

		// Token: 0x060015E5 RID: 5605 RVA: 0x000844E8 File Offset: 0x000826E8
		private void OpenStatisticTable(object sender, RoutedEventArgs e)
		{
			CtrlTestcardSimStatisticsTable ctrlTestcardSimStatisticsTable = new CtrlTestcardSimStatisticsTable();
			Subwindow.Show(ctrlTestcardSimStatisticsTable, SubwindowButtons.BackAndHome, ctrlTestcardSimStatisticsTable.PanelButtons, ctrlTestcardSimStatisticsTable.UiElementRightAligned);
		}

		// Token: 0x060015E6 RID: 5606 RVA: 0x00084510 File Offset: 0x00082710
		private void UserControl_Unloaded(object sender, RoutedEventArgs e)
		{
			this.PanelButtons.Children.Clear();
			this.PanelButtons = null;
		}

		// Token: 0x060015E7 RID: 5607 RVA: 0x0008452C File Offset: 0x0008272C
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/testcardsimulation/ctrltestcardsimstatistics.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x060015E8 RID: 5608 RVA: 0x00084564 File Offset: 0x00082764
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				((CtrlTestcardSimStatistics)target).Unloaded += this.UserControl_Unloaded;
				break;
			case 2:
				this.graphesVAGlass = (Grid)target;
				break;
			case 3:
				this.plotVA = (SesoPlot)target;
				break;
			case 4:
				this.plotGL = (SesoPlot)target;
				break;
			default:
				this._contentLoaded = true;
				break;
			}
		}

		// Token: 0x04000E74 RID: 3700
		private TestcardSimStatisticsViewModel viewModel;

		// Token: 0x04000E75 RID: 3701
		private ParameterHandling parHandling;

		// Token: 0x04000E76 RID: 3702
		public DockPanel PanelButtons;

		// Token: 0x04000E77 RID: 3703
		public UIElement UiElementRightAligned;

		// Token: 0x04000E78 RID: 3704
		private List<Point> plotPointsVA;

		// Token: 0x04000E79 RID: 3705
		private List<Point> plotPointsGL;

		// Token: 0x04000E7A RID: 3706
		private List<Point> sensitivityLine;

		// Token: 0x04000E7B RID: 3707
		private ProductManager productManager;

		// Token: 0x04000E7C RID: 3708
		private DBConnection dbConnection;

		// Token: 0x04000E7D RID: 3709
		private string simmode;

		// Token: 0x04000E7E RID: 3710
		private bool checkIfCurvesAvailable;

		// Token: 0x04000E7F RID: 3711
		private TestcardSimService testcardSimService;

		// Token: 0x04000E80 RID: 3712
		internal Grid graphesVAGlass;

		// Token: 0x04000E81 RID: 3713
		internal SesoPlot plotVA;

		// Token: 0x04000E82 RID: 3714
		internal SesoPlot plotGL;

		// Token: 0x04000E83 RID: 3715
		private bool _contentLoaded;
	}
}
