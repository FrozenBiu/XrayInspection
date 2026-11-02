using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using Common;
using DatabaseConnection;
using Definitions;
using SesoPlot;
using SNRPlotLibrary;
using XrsImage;

namespace GUI_XRS.Graph
{
	// Token: 0x02000273 RID: 627
	public partial class CtrlSnr : UserControl
	{
		// Token: 0x17001115 RID: 4373
		// (get) Token: 0x06003482 RID: 13442 RVA: 0x00149977 File Offset: 0x00147B77
		// (set) Token: 0x06003483 RID: 13443 RVA: 0x0014997F File Offset: 0x00147B7F
		private Details details { get; set; }

		// Token: 0x17001116 RID: 4374
		// (get) Token: 0x06003484 RID: 13444 RVA: 0x00149988 File Offset: 0x00147B88
		// (set) Token: 0x06003485 RID: 13445 RVA: 0x00149990 File Offset: 0x00147B90
		public RelayCommand RescaleCommand { get; set; }

		// Token: 0x06003486 RID: 13446 RVA: 0x0014999C File Offset: 0x00147B9C
		public CtrlSnr(IImageArrived iImageArrived, IPlotDetailsArrived iPlotDetailsArrived, ParameterHandling parHandling, DBConnection dbConnection, Details details)
		{
			this.parHandling = parHandling;
			this.dbConnection = dbConnection;
			this.InitializeComponent();
			this.RescaleCommand = new RelayCommand(new Action<object>(this.Rescale));
			this.details = details;
			this.InitializeButtons();
			bool isTdiDetector = details.IsTdiDetector;
			if (isTdiDetector)
			{
				bool isTdiScanMode = details.IsTdiScanMode;
				if (isTdiScanMode)
				{
					this.lSNR.Content = "SNR TDI";
					this.labelSNR.Content = "SNR TDI";
					this.labelSNRLowEnergy.Content = "SNR Reference TDI";
					this.labelReferencePlotON.Content = "SNR Reference TDI";
					this.lLegendSNR.Content = "SNR TDI";
					this.lLegendSNRReference.Content = "SNR Reference TDI";
					this.snrPlot.TableName = "SNRPlot_TDI";
				}
				bool isNoTdiScanMode = details.IsNoTdiScanMode;
				if (isNoTdiScanMode)
				{
					this.lSNR.Content = "SNR NoTDI";
					this.labelSNR.Content = "SNR NoTDI";
					this.labelSNRLowEnergy.Content = "SNR Reference NoTDI";
					this.labelReferencePlotON.Content = "SNR Reference NoTDI";
					this.lLegendSNR.Content = "SNR NoTDI";
					this.lLegendSNRReference.Content = "SNR Reference NoTDI";
					this.snrPlot.TableName = "SNRPlot_NoTDI";
				}
			}
			this.iImageArrived = iImageArrived;
			this.iImageArrived.ImageArrived += this.iImageArrived_ImageArrived;
			this.iPlotDetailsArrived = iPlotDetailsArrived;
			this.iPlotDetailsArrived.PlotDetailsArrived += this.iPlotDetailsArrived_PlotDetailsArrived;
			this.InitializeGraphInfos();
			dbConnection.GetIgnoredLeftPixel(ref this.ignorePixelLeft);
			dbConnection.GetIgnoredRightPixel(ref this.ignorePixelRight);
			parHandling.WriteValue(ParameterHandling.ignoreLeftPixelPath, "0", false);
			parHandling.WriteValue(ParameterHandling.ignoreRightPixelPath, "0", false);
			this.detectorType = DetectorType.DT;
			dbConnection.GetDetectorType(ref this.detectorType);
			bool flag = this.detectorType == DetectorType.DTNetDE;
			if (flag)
			{
				details.IsDualDetector = true;
			}
			else
			{
				details.IsDualDetector = false;
			}
			this.InitializeDetectortypeLayout();
			this.initializeBlankoValues();
			base.DataContext = this;
			MachineType machineType = MachineType.MachineTypeUndefined;
			dbConnection.GetMachineType(ref machineType);
			this.MachineParametersFontSize = ((machineType != MachineType.RayconEntry) ? 12.0 : 10.0);
			parHandling.WriteValue(XRayOnProcessing.manualOffsetCalibrationPath, "1", false);
		}

		// Token: 0x17001117 RID: 4375
		// (get) Token: 0x06003487 RID: 13447 RVA: 0x00149D42 File Offset: 0x00147F42
		// (set) Token: 0x06003488 RID: 13448 RVA: 0x00149D4A File Offset: 0x00147F4A
		public double MachineParametersFontSize { get; set; }

		// Token: 0x06003489 RID: 13449 RVA: 0x00149D54 File Offset: 0x00147F54
		private void iImageArrived_ImageArrived(object sender, RcvImgEventArgs e)
		{
			this.imageWidth = e.Image.Width;
			this.imageHeight = e.Image.Height;
			this.imageData = e.Image.Data;
			bool flag = this.detectorType == DetectorType.DTNetTDI;
			if (flag)
			{
				this.parHandling.WriteValue("/hardware/detector/dt/StartStopScanning/Value", "0", false);
			}
			bool flag2 = this.imageWidth != 0 && this.imageHeight != 0 && !this.savingReferenceSNR && e.Image.SourceVoltage > 0f;
			if (flag2)
			{
				this.calcImage.SignalNoiseRatio(this.imageData, (int)this.imageWidth, (int)this.imageHeight, this.details.IsDualDetector, this.ignorePixelLeft, this.ignorePixelRight);
				Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate()
				{
					this.refdashstyle = new DoubleCollection(this.dc);
					bool isVisible = base.IsVisible;
					if (isVisible)
					{
						this.InitializeGraphs();
						this.Autoscale(false);
						bool flag4 = this.sesoplot.MaximumY < this.highestValueYAxis;
						if (flag4)
						{
							this.Autoscale(true);
						}
						this.sesoplot.MaximumY = this.sesoplot.MaximumY + 1E-05;
						bool flag5 = !this.isAutoscaleInitialized;
						if (flag5)
						{
							this.Autoscale(true);
							this.isAutoscaleInitialized = true;
						}
						this.checkSNRPlot((int)this.imageWidth);
					}
				}));
			}
			else
			{
				Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate()
				{
					this.refdashstyle = new DoubleCollection(this.dc);
					bool flag4 = base.IsVisible || this.calcImage.CalcImageList == null;
					if (flag4)
					{
						this.InitializeDetectortypeLayout();
						this.initializeBlankoValues();
					}
				}));
			}
			bool flag3 = this.detectorType == DetectorType.DTNetTDI;
			if (flag3)
			{
				this.parHandling.WriteValue("/hardware/detector/dt/StartStopScanning/Value", "1", false);
			}
		}

		// Token: 0x0600348A RID: 13450 RVA: 0x00149E84 File Offset: 0x00148084
		private void iPlotDetailsArrived_PlotDetailsArrived(object sender, Details e)
		{
			Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate()
			{
				this.details = e;
			}));
		}

		// Token: 0x0600348B RID: 13451 RVA: 0x00149EC4 File Offset: 0x001480C4
		private void InitializeButtons()
		{
			this.PanelButtons = new DockPanel();
			this.PanelButtons.HorizontalAlignment = HorizontalAlignment.Stretch;
			this.PanelButtons.VerticalAlignment = VerticalAlignment.Stretch;
			Button button = new Button();
			button.Content = "Autoscale";
			button.Command = this.RescaleCommand;
			button.Margin = new Thickness(0.0, 2.0, 10.0, 2.0);
			this.PanelButtons.Children.Add(button);
			DockPanel.SetDock(button, Dock.Left);
			Button button2 = new Button();
			button2.Content = XrsImage.ImageConverter.ToImage("/img/savePNG.png");
			button2.Click += this.btnSavePNG_Click;
			button2.Style = (base.FindResource("defaultButtonStyle") as Style);
			button2.Margin = new Thickness(0.0, 0.0, 10.0, 0.0);
			this.PanelButtons.Children.Add(button2);
			DockPanel.SetDock(button2, Dock.Left);
			Button button3 = new Button();
			button3.Content = XrsImage.ImageConverter.ToImage("/img/saveCSV.png");
			button3.Click += this.btnSaveCSV_Click;
			button3.Style = (base.FindResource("defaultButtonStyle") as Style);
			button3.Margin = new Thickness(0.0, 0.0, 10.0, 0.0);
			this.PanelButtons.Children.Add(button3);
			DockPanel.SetDock(button3, Dock.Left);
			Button button4 = new Button();
			button4.Content = XrsImage.ImageConverter.ToImage("/img/referencePlot.png");
			button4.Click += this.btnSaveDatabase_Click;
			button4.Style = (base.FindResource("defaultButtonStyle") as Style);
			button4.Margin = new Thickness(0.0, 0.0, 10.0, 0.0);
			this.PanelButtons.Children.Add(button4);
			DockPanel.SetDock(button4, Dock.Left);
			Grid element = new Grid();
			this.PanelButtons.Children.Add(element);
			DockPanel.SetDock(element, Dock.Left);
			Label label = new Label();
			bool isTdiDetector = this.details.IsTdiDetector;
			if (isTdiDetector)
			{
				bool isTdiScanMode = this.details.IsTdiScanMode;
				if (isTdiScanMode)
				{
					label.Content = "SNR TDI";
				}
				bool isNoTdiScanMode = this.details.IsNoTdiScanMode;
				if (isNoTdiScanMode)
				{
					label.Content = "SNR NoTDI";
				}
			}
			else
			{
				label.Content = "SNR";
			}
			label.FontSize = 32.0;
			label.FontWeight = FontWeights.Bold;
			label.FontFamily = new System.Windows.Media.FontFamily("Verdana");
			label.Foreground = System.Windows.Media.Brushes.DarkGray;
			label.VerticalAlignment = VerticalAlignment.Center;
			label.HorizontalAlignment = HorizontalAlignment.Right;
			label.Margin = new Thickness(0.0, 0.0, 50.0, 0.0);
			this.UiElementRightAligned = label;
		}

		// Token: 0x0600348C RID: 13452 RVA: 0x0014A218 File Offset: 0x00148418
		private void InitializeDetectortypeLayout()
		{
			bool isDualDetector = this.details.IsDualDetector;
			if (isDualDetector)
			{
				this.labelSNR.Content = "SNR Plot LE";
				this.cBSNRHighEnergy.Visibility = Visibility.Visible;
				this.labelSNRLowEnergy.Visibility = Visibility.Visible;
				this.lSNRLow.Visibility = Visibility.Visible;
				this.lSNRHigh.Visibility = Visibility.Visible;
				this.GLegendDualEnergy.Visibility = Visibility.Visible;
				this.lGainHighEnergy.Visibility = Visibility.Visible;
				this.tBlcGainHighEnergy.Visibility = Visibility.Visible;
				this.lGainLowEnergy.Visibility = Visibility.Visible;
				this.tBlcGainLowEnergy.Visibility = Visibility.Visible;
				this.lGain.Visibility = Visibility.Collapsed;
				this.tBlcGain.Visibility = Visibility.Collapsed;
				this.snrPlot.IsDualEnergy = true;
				this.ColumnLegendGreyvalueDualEnergy.Width = this.hideGridColumn;
				this.ColumnLegendReferenceSNRDualEnergy.Width = this.hideGridColumn;
				this.ColumnLegendStandarddevationDualEnergy.Width = this.hideGridColumn;
				this.GLegendDualEnergy.Visibility = Visibility.Visible;
				this.gridLenghtLegendDualEnergy = new List<GridLength>();
				this.gridLenghtLegendDualEnergy.Add(default(GridLength));
				Thickness margin = default(Thickness);
				margin.Bottom = 20.0;
				this.sesoplot.Margin = margin;
			}
			else
			{
				bool isTdiDetector = this.details.IsTdiDetector;
				if (isTdiDetector)
				{
					this.cBSNRHighEnergy.Visibility = Visibility.Collapsed;
					this.labelSNRLowEnergy.Visibility = Visibility.Collapsed;
					this.lSNRLow.Visibility = Visibility.Collapsed;
					this.lSNRHigh.Visibility = Visibility.Collapsed;
					this.GLegendDualEnergy.Visibility = Visibility.Collapsed;
					this.lGainHighEnergy.Visibility = Visibility.Visible;
					this.tBlcGainHighEnergy.Visibility = Visibility.Visible;
					this.lGainLowEnergy.Visibility = Visibility.Visible;
					this.tBlcGainLowEnergy.Visibility = Visibility.Visible;
					this.lGain.Visibility = Visibility.Collapsed;
					this.tBlcGain.Visibility = Visibility.Collapsed;
					this.snrPlot.IsDualEnergy = false;
					this.ColumnLegendGreyvalue.Width = this.hideGridColumn;
					this.ColumnLegendStandarddevation.Width = this.hideGridColumn;
					this.ColumnLegendReferenceSNR.Width = this.hideGridColumn;
					this.GLegendSingleEnergy.Visibility = Visibility.Visible;
					this.gridLenghtLegendSingleEnergy = new List<GridLength>();
					this.gridLenghtLegendSingleEnergy.Add(default(GridLength));
					Thickness margin2 = default(Thickness);
					margin2.Bottom = 5.0;
					this.sesoplot.Margin = margin2;
				}
				else
				{
					this.labelSNR.Content = "SNR Plot";
					this.cBSNRHighEnergy.Visibility = Visibility.Collapsed;
					this.labelSNRLowEnergy.Visibility = Visibility.Collapsed;
					this.lSNRLow.Visibility = Visibility.Collapsed;
					this.lSNRHigh.Visibility = Visibility.Collapsed;
					this.GLegendDualEnergy.Visibility = Visibility.Collapsed;
					this.tBlcAvaerageSNRLow.Visibility = Visibility.Collapsed;
					this.tBlcSNRMaxLow.Visibility = Visibility.Collapsed;
					this.tBlcSNRMinLow.Visibility = Visibility.Collapsed;
					this.lGainHighEnergy.Visibility = Visibility.Collapsed;
					this.tBlcGainHighEnergy.Visibility = Visibility.Collapsed;
					this.lGainLowEnergy.Visibility = Visibility.Collapsed;
					this.tBlcGainLowEnergy.Visibility = Visibility.Collapsed;
					this.lGain.Visibility = Visibility.Visible;
					this.tBlcGain.Visibility = Visibility.Visible;
					this.snrPlot.IsDualEnergy = false;
					this.ColumnLegendGreyvalue.Width = this.hideGridColumn;
					this.ColumnLegendStandarddevation.Width = this.hideGridColumn;
					this.ColumnLegendReferenceSNR.Width = this.hideGridColumn;
					this.GLegendSingleEnergy.Visibility = Visibility.Visible;
					this.gridLenghtLegendSingleEnergy = new List<GridLength>();
					this.gridLenghtLegendSingleEnergy.Add(default(GridLength));
					Thickness margin3 = default(Thickness);
					margin3.Bottom = 5.0;
					this.sesoplot.Margin = margin3;
				}
			}
		}

		// Token: 0x0600348D RID: 13453 RVA: 0x0014A614 File Offset: 0x00148814
		private void initializeBlankoValues()
		{
			CalculatedImage calculatedImage = new CalculatedImage();
			List<CalculatedImage> list = new List<CalculatedImage>();
			for (int i = 0; i < 16; i++)
			{
				bool flag = this.sesoplot.Graphs.Count < 16;
				if (flag)
				{
					Graph item = new Graph();
					this.sesoplot.Graphs.Add(item);
				}
			}
			calculatedImage.Average = 0.0;
			calculatedImage.DevationOfTotalAverage = 0.0;
			calculatedImage.MaximumHighEnergy = 0.0;
			calculatedImage.MaximumLowEnergy = 0.0;
			calculatedImage.MinimumLowEnergy = 0.0;
			calculatedImage.MinimumHighEnergy = 0.0;
			calculatedImage.Pixelnumber = 0;
			calculatedImage.Signalnoiseratio = 0.0;
			calculatedImage.Standarddevation = 0.0;
			calculatedImage.TotalAverageHighEnergy = 0.0;
			calculatedImage.TotalAverageLowEnergy = 0.0;
			for (int j = 0; j < 100; j++)
			{
				list.Add(calculatedImage);
			}
			this.calcImage.CalcImageList = list;
			this.sesoplot.MaximumY = 50.0;
			this.sesoplot.MinimumY = 0.0;
			this.sesoplot.MajorTicksY = 5.0;
			this.sesoplot.MinorTicksY = 1.0;
		}

		// Token: 0x0600348E RID: 13454 RVA: 0x0014A7A4 File Offset: 0x001489A4
		private void InitializeGraphInfos()
		{
			bool flag = this.details.SNDetector != null;
			if (flag)
			{
				this.serialnumberModified = this.details.SNDetector.Replace("-", "").Replace(":", "");
			}
			this.tBlcVoltage.Text = this.details.Voltage;
			this.tBlcCurrent.Text = this.details.Current;
			this.tBlcXrayEnable.Text = this.details.XrayEnable;
			this.tBlcSNXRaySource.Text = this.details.SNXRaySource;
			this.tBlcXRayOnTimeInternal.Text = this.details.XRayOnTimeInternal;
			bool lowHighGainAvailable = this.details.LowHighGainAvailable;
			if (lowHighGainAvailable)
			{
				this.tBlcGainHighEnergy.Text = this.details.HighGain;
				this.tBlcGainLowEnergy.Text = this.details.LowGain;
			}
			this.tBlcTemperature.Text = this.details.Temperature;
			this.tBlcScanrate.Text = this.details.Scanrate;
			this.tBlcGainEnable.Text = this.details.GainEnable;
			this.tBlcSNDetector.Text = this.details.SNDetector;
			this.tBlcOffsetEnable.Text = this.details.OffsetEnable;
			bool flag2 = !this.details.IsDualDetector;
			if (flag2)
			{
				this.tBlcGain.Text = this.details.Gain;
			}
			this.tBlcSNMachine.Text = this.details.SNMachine;
			this.tBlcIgnoreLeft.Text = Convert.ToString(this.ignorePixelLeft);
			this.tBlcIgnoreRight.Text = Convert.ToString(this.ignorePixelRight);
			this.tBlcDetectorType.Text = this.details.DetectorTypeString;
			this.tBlcPixelSize.Text = this.details.PixelSize;
		}

		// Token: 0x0600348F RID: 13455 RVA: 0x0014A9B8 File Offset: 0x00148BB8
		private void InitializeGraphs()
		{
			this.InitializeGraphInfos();
			bool isDualDetector = this.details.IsDualDetector;
			if (isDualDetector)
			{
				bool flag = this.sesoplot.MaximumX != (double)(this.imageWidth / 2 + 1);
				if (flag)
				{
					this.sesoplot.MaximumX = (double)Convert.ToInt16((int)(this.imageWidth / 2 + 1));
				}
				bool flag3;
				if ((int)(this.imageWidth / 2) < this.snrPlot.PixelCountReferencePlot)
				{
					bool? isChecked = this.cBReferencePlot.IsChecked;
					bool flag2 = true;
					flag3 = (isChecked.GetValueOrDefault() == flag2 & isChecked != null);
				}
				else
				{
					flag3 = false;
				}
				bool flag4 = flag3;
				if (flag4)
				{
					this.sesoplot.MaximumX = (double)(this.snrPlot.PixelCountReferencePlot + 1);
				}
				bool flag5 = this.imageWidth / 2 > 1025;
				if (flag5)
				{
					this.sesoplot.MajorTicksX = 128.0;
					this.sesoplot.MinorTicksX = 128.0;
				}
				else
				{
					this.sesoplot.MajorTicksX = 64.0;
					this.sesoplot.MinorTicksX = 64.0;
				}
			}
			else
			{
				bool flag6 = this.sesoplot.MaximumX != (double)(this.imageWidth + 1);
				if (flag6)
				{
					this.sesoplot.MaximumX = (double)Convert.ToInt16((int)(this.imageWidth + 1));
				}
				bool flag7;
				if ((int)this.imageWidth < this.snrPlot.PixelCountReferencePlot)
				{
					bool? isChecked = this.cBReferencePlot.IsChecked;
					bool flag2 = true;
					flag7 = (isChecked.GetValueOrDefault() == flag2 & isChecked != null);
				}
				else
				{
					flag7 = false;
				}
				bool flag8 = flag7;
				if (flag8)
				{
					this.sesoplot.MaximumX = (double)(this.snrPlot.PixelCountReferencePlot + 1);
				}
				this.sesoplot.MinorTicksX = (double)((this.sesoplot.MaximumX <= 512.0) ? 4 : 8);
				this.sesoplot.MajorTicksX = 64.0;
				bool flag9 = this.sesoplot.MaximumX > 2048.0;
				if (flag9)
				{
					this.sesoplot.MinorTicksX = 16.0;
					this.sesoplot.MajorTicksX = 160.0;
				}
				bool flag10 = this.sesoplot.MaximumX > 4096.0;
				if (flag10)
				{
					this.sesoplot.MinorTicksX = 32.0;
					this.sesoplot.MajorTicksX = 320.0;
				}
				bool flag11 = this.sesoplot.MaximumX > 8192.0;
				if (flag11)
				{
					this.sesoplot.MinorTicksX = 64.0;
					this.sesoplot.MajorTicksX = 640.0;
				}
			}
			bool flag12 = !this.isDetectorTypeLayoutInitialized || this.sesoplot.Graphs.Count == 0;
			if (flag12)
			{
				this.InitializeDetectortypeLayout();
				this.cBStandarddevation.IsCheckedReal = new bool?(false);
				this.cBImageAverage.IsCheckedReal = new bool?(false);
				this.cBSNRAverageMinMax.IsCheckedReal = new bool?(true);
				this.cBSNR.IsCheckedReal = new bool?(true);
				this.cBReferencePlot.IsCheckedReal = new bool?(false);
				this.sesoplot.Graphs = new ObservableCollection<Graph>();
				this.CreateNewGraphs(this.calcImage.CalcImageList);
				this.sesoplot.Graphs[1].Stroke = System.Windows.Media.Brushes.Transparent;
				this.sesoplot.Graphs[3].Stroke = System.Windows.Media.Brushes.Transparent;
				bool flag13 = !this.details.IsNoTdiScanMode;
				if (flag13)
				{
					this.sesoplot.Graphs[5].Stroke = System.Windows.Media.Brushes.Green;
				}
				else
				{
					this.sesoplot.Graphs[5].Stroke = System.Windows.Media.Brushes.CadetBlue;
					this.SingleEnergySNR.Stroke = System.Windows.Media.Brushes.CadetBlue;
				}
				this.sesoplot.Graphs[9].Stroke = System.Windows.Media.Brushes.DarkGray;
				this.sesoplot.Graphs[10].Stroke = System.Windows.Media.Brushes.DarkGray;
				this.sesoplot.Graphs[11].Stroke = System.Windows.Media.Brushes.DarkGray;
				bool flag14 = !this.details.IsNoTdiScanMode;
				if (flag14)
				{
					this.sesoplot.Graphs[12].Stroke = System.Windows.Media.Brushes.DarkRed;
				}
				else
				{
					this.sesoplot.Graphs[12].Stroke = System.Windows.Media.Brushes.Goldenrod;
					this.LineSNRReference.Stroke = System.Windows.Media.Brushes.Goldenrod;
				}
				this.sesoplot.Graphs[14].Stroke = System.Windows.Media.Brushes.DarkGray;
				this.sesoplot.Graphs[15].Stroke = System.Windows.Media.Brushes.DarkGray;
				this.sesoplot.Graphs[1].StrokeThickness = 2.0;
				this.sesoplot.Graphs[3].StrokeThickness = 2.0;
				this.sesoplot.Graphs[5].StrokeThickness = 2.0;
				this.sesoplot.Graphs[9].StrokeThickness = 2.0;
				this.sesoplot.Graphs[11].StrokeThickness = 2.0;
				this.sesoplot.Graphs[10].StrokeThickness = 2.0;
				this.sesoplot.Graphs[14].StrokeThickness = 3.0;
				this.sesoplot.Graphs[15].StrokeThickness = 3.0;
				this.sesoplot.Graphs[14].StrokeDashArray = this.refdashstyle;
				this.sesoplot.Graphs[15].StrokeDashArray = this.refdashstyle;
				bool isDualDetector2 = this.details.IsDualDetector;
				if (isDualDetector2)
				{
					this.sesoplot.Graphs[0].Stroke = System.Windows.Media.Brushes.Transparent;
					this.sesoplot.Graphs[2].Stroke = System.Windows.Media.Brushes.Transparent;
					this.sesoplot.Graphs[4].Stroke = System.Windows.Media.Brushes.Blue;
					this.sesoplot.Graphs[5].Stroke = System.Windows.Media.Brushes.Green;
					this.sesoplot.Graphs[6].Stroke = System.Windows.Media.Brushes.DarkGray;
					this.sesoplot.Graphs[7].Stroke = System.Windows.Media.Brushes.DarkGray;
					this.sesoplot.Graphs[8].Stroke = System.Windows.Media.Brushes.DarkGray;
					this.sesoplot.Graphs[0].StrokeThickness = 2.0;
					this.sesoplot.Graphs[2].StrokeThickness = 2.0;
					this.sesoplot.Graphs[4].StrokeThickness = 2.0;
					this.sesoplot.Graphs[6].StrokeThickness = 2.0;
					this.sesoplot.Graphs[7].StrokeThickness = 2.0;
					this.sesoplot.Graphs[8].StrokeThickness = 2.0;
				}
				this.snrPlot.CreateReferencePlots();
				this.cBReferencePlot.IsCheckedReal = new bool?(true);
				this.snrPlot.checkSNRPlot(this.serialnumberModified);
				this.snrPlot.LoadSNRPlot(this.serialnumberModified);
				this.sesoplot.Graphs[12] = this.snrPlot.SNRReferenceplots[1];
				this.sesoplot.Graphs[13] = this.snrPlot.SNRReferenceplots[0];
				bool flag15 = this.sesoplot.Graphs[12].Points != null;
				if (flag15)
				{
					this.sesoplot.Graphs[12].Points.RemoveRange(this.sesoplot.Graphs[12].Points.Count - this.ignorePixelRight, this.ignorePixelRight);
					this.sesoplot.Graphs[12].Points.RemoveRange(0, this.ignorePixelLeft);
					this.sesoplot.Graphs[12] = this.snrPlot.SNRReferenceplots[1];
					this.sesoplot.Graphs[12].StrokeThickness = 2.0;
					this.sesoplot.Graphs[12].StrokeDashArray = this.refdashstyle;
					bool flag16 = !this.details.IsNoTdiScanMode;
					if (flag16)
					{
						this.sesoplot.Graphs[12].Stroke = System.Windows.Media.Brushes.DarkRed;
					}
					else
					{
						this.sesoplot.Graphs[12].Stroke = System.Windows.Media.Brushes.Goldenrod;
						this.LineSNRReference.Stroke = System.Windows.Media.Brushes.Goldenrod;
					}
					bool isDualDetector3 = this.details.IsDualDetector;
					if (isDualDetector3)
					{
						this.ColumnLegendReferenceSNRDualEnergy.Width = this.gridLenghtLegendDualEnergy[0];
					}
					else
					{
						this.ColumnLegendReferenceSNR.Width = this.gridLenghtLegendSingleEnergy[0];
					}
				}
				bool isDualDetector4 = this.details.IsDualDetector;
				if (isDualDetector4)
				{
					bool flag17 = this.sesoplot.Graphs[13].Points != null;
					if (flag17)
					{
						this.sesoplot.Graphs[13].Points.RemoveRange(this.sesoplot.Graphs[13].Points.Count - this.ignorePixelRight, this.ignorePixelRight);
						this.sesoplot.Graphs[13].Points.RemoveRange(0, this.ignorePixelLeft);
						this.sesoplot.Graphs[13] = this.snrPlot.SNRReferenceplots[0];
						this.sesoplot.Graphs[13].Stroke = System.Windows.Media.Brushes.DarkTurquoise;
						this.sesoplot.Graphs[13].StrokeThickness = 2.0;
						this.sesoplot.Graphs[13].StrokeDashArray = this.refdashstyle;
						this.ColumnLegendReferenceSNRDualEnergy.Width = this.gridLenghtLegendDualEnergy[0];
					}
				}
				this.sesoplot.MaximumY = 50.0;
				this.sesoplot.MinimumY = 0.0;
			}
			else
			{
				bool flag18 = (int)this.imageWidth - (this.ignorePixelLeft + this.ignorePixelRight) != this.sesoplot.Graphs[3].Points.Count;
				if (flag18)
				{
					bool flag19 = this.sesoplot.Graphs[12].Points != null;
					if (flag19)
					{
						bool value = this.cBReferencePlot.IsChecked.Value;
						if (value)
						{
							bool flag20 = !this.details.IsNoTdiScanMode;
							if (flag20)
							{
								this.sesoplot.Graphs[12].Stroke = System.Windows.Media.Brushes.DarkRed;
							}
							else
							{
								this.sesoplot.Graphs[12].Stroke = System.Windows.Media.Brushes.Goldenrod;
							}
						}
						else
						{
							this.sesoplot.Graphs[12].Stroke = System.Windows.Media.Brushes.Transparent;
						}
						this.sesoplot.Graphs[12].StrokeThickness = 2.0;
						this.sesoplot.Graphs[12].StrokeDashArray = this.refdashstyle;
					}
					bool isDualDetector5 = this.details.IsDualDetector;
					if (isDualDetector5)
					{
						bool flag21 = this.sesoplot.Graphs[13].Points != null;
						if (flag21)
						{
							bool value2 = this.cBReferencePlot.IsChecked.Value;
							if (value2)
							{
								this.sesoplot.Graphs[13].Stroke = System.Windows.Media.Brushes.DarkTurquoise;
							}
							else
							{
								this.sesoplot.Graphs[13].Stroke = System.Windows.Media.Brushes.Transparent;
							}
							this.sesoplot.Graphs[13].StrokeThickness = 2.0;
							this.sesoplot.Graphs[13].StrokeDashArray = this.refdashstyle;
						}
					}
				}
				this.CreateNewGraphs(this.calcImage.CalcImageList);
			}
			this.tBlcAvaerageSNR.Text = Convert.ToString(Math.Round(this.calcImage.CalcImageList[this.ignorePixelLeft + 1].TotalAverageLowEnergy, 2));
			this.tBlcSNRMax.Text = Convert.ToString(Math.Round(this.calcImage.CalcImageList[0].MaximumLowEnergy, 2));
			this.tBlcSNRMin.Text = Convert.ToString(Math.Round(this.calcImage.CalcImageList[0].MinimumLowEnergy, 2));
			bool isDualDetector6 = this.details.IsDualDetector;
			if (isDualDetector6)
			{
				this.tBlcAvaerageSNRLow.Text = Convert.ToString(Math.Round(this.calcImage.CalcImageList[1 + this.ignorePixelLeft].TotalAverageHighEnergy, 2));
				this.tBlcSNRMaxLow.Text = Convert.ToString(Math.Round(this.calcImage.CalcImageList[0].MaximumHighEnergy, 2));
				this.tBlcSNRMinLow.Text = Convert.ToString(Math.Round(this.calcImage.CalcImageList[0].MinimumHighEnergy, 2));
			}
			this.isDetectorTypeLayoutInitialized = true;
		}

		// Token: 0x06003490 RID: 13456 RVA: 0x0014B890 File Offset: 0x00149A90
		private void CreateNewGraphs(List<CalculatedImage> clacImg)
		{
			this.sumofGreyvaluesHighEnergy = 0.0;
			this.sumofGreyvaluesLowEnergy = 0.0;
			double num = 0.0;
			bool flag = !this.isDetectorTypeLayoutInitialized || this.sesoplot.Graphs.Count != 16;
			if (flag)
			{
				for (int i = 0; i < 16; i++)
				{
					bool flag2 = this.sesoplot.Graphs.Count < 16;
					if (flag2)
					{
						Graph item = new Graph();
						this.sesoplot.Graphs.Add(item);
					}
				}
			}
			bool isDualDetector = this.details.IsDualDetector;
			int num2;
			if (isDualDetector)
			{
				num2 = this.calcImage.CalcImageList.Count / 2 - this.ignorePixelRight;
			}
			else
			{
				num2 = this.calcImage.CalcImageList.Count - this.ignorePixelRight;
			}
			bool isDualDetector2 = this.details.IsDualDetector;
			if (isDualDetector2)
			{
				for (int j = this.ignorePixelLeft; j < this.calcImage.CalcImageList.Count / 2 - this.ignorePixelRight; j++)
				{
					this.sumofGreyvaluesLowEnergy += this.calcImage.CalcImageList[j].Average;
				}
				for (int k = this.calcImage.CalcImageList.Count / 2 + this.ignorePixelLeft; k < this.calcImage.CalcImageList.Count - this.ignorePixelRight; k++)
				{
					this.sumofGreyvaluesHighEnergy += this.calcImage.CalcImageList[k].Average;
				}
				this.averageGreyvalueHighEnergy = this.sumofGreyvaluesHighEnergy / (double)(this.calcImage.CalcImageList.Count / 2 - this.ignorePixelLeft - this.ignorePixelRight);
				this.averageGreyvalueLowEnergy = this.sumofGreyvaluesLowEnergy / (double)(this.calcImage.CalcImageList.Count / 2 - this.ignorePixelLeft - this.ignorePixelRight);
				double num3 = Math.Max(this.averageGreyvalueHighEnergy, this.averageGreyvalueLowEnergy);
				this.ratioOfGreyvalueSNRHighEnergy = num3 / this.calcImage.CalcImageList[this.ignorePixelLeft].TotalAverageHighEnergy;
				this.ratioOfGreyvalueSNRLowEnergy = num3 / this.calcImage.CalcImageList[this.ignorePixelLeft].TotalAverageLowEnergy;
				num = Math.Max(this.ratioOfGreyvalueSNRHighEnergy, this.ratioOfGreyvalueSNRLowEnergy);
			}
			else
			{
				for (int l = this.ignorePixelLeft; l < this.calcImage.CalcImageList.Count - this.ignorePixelRight; l++)
				{
					this.sumofGreyvaluesLowEnergy += this.calcImage.CalcImageList[l].Average;
				}
				this.averageGreyvalueLowEnergy = this.sumofGreyvaluesLowEnergy / (double)(this.calcImage.CalcImageList.Count - this.ignorePixelLeft - this.ignorePixelRight);
				this.ratioOfGreyvalueSNRLowEnergy = this.averageGreyvalueLowEnergy / this.calcImage.CalcImageList[this.ignorePixelLeft].TotalAverageLowEnergy;
			}
			this.plotPoints = new List<System.Windows.Point>();
			this.plotPointsDualEnergy = new List<System.Windows.Point>();
			for (int m = this.ignorePixelLeft; m < num2; m++)
			{
				System.Windows.Point item2 = default(System.Windows.Point);
				bool isDualDetector3 = this.details.IsDualDetector;
				if (isDualDetector3)
				{
					item2.X = (double)m;
					item2.Y = clacImg[m + this.calcImage.CalcImageList.Count / 2].Average / (num * 1.3);
					this.plotPointsDualEnergy.Add(item2);
					this.sesoplot.Graphs[0].Points = this.plotPointsDualEnergy;
					item2.X = (double)clacImg[m].Pixelnumber;
					item2.Y = clacImg[m].Average / (num * 1.3);
					this.plotPoints.Add(item2);
					this.sesoplot.Graphs[1].Points = this.plotPoints;
				}
				else
				{
					item2.X = (double)clacImg[m].Pixelnumber;
					item2.Y = clacImg[m].Average / (this.ratioOfGreyvalueSNRLowEnergy * 1.3);
					this.plotPoints.Add(item2);
					this.sesoplot.Graphs[1].Points = this.plotPoints;
				}
			}
			this.plotPoints = new List<System.Windows.Point>();
			this.plotPointsDualEnergy = new List<System.Windows.Point>();
			for (int n = this.ignorePixelLeft; n < num2; n++)
			{
				System.Windows.Point item3 = default(System.Windows.Point);
				bool isDualDetector4 = this.details.IsDualDetector;
				if (isDualDetector4)
				{
					item3.X = (double)n;
					item3.Y = clacImg[n + this.calcImage.CalcImageList.Count / 2].Signalnoiseratio;
					this.plotPointsDualEnergy.Add(item3);
					this.sesoplot.Graphs[4].Points = this.plotPointsDualEnergy;
					item3.X = (double)clacImg[n].Pixelnumber;
					item3.Y = clacImg[n].Signalnoiseratio;
					this.plotPoints.Add(item3);
					this.sesoplot.Graphs[5].Points = this.plotPoints;
				}
				else
				{
					item3.X = (double)clacImg[n].Pixelnumber;
					item3.Y = clacImg[n].Signalnoiseratio;
					this.plotPoints.Add(item3);
					this.sesoplot.Graphs[5].Points = this.plotPoints;
				}
			}
			this.plotPoints = new List<System.Windows.Point>();
			this.plotPointsDualEnergy = new List<System.Windows.Point>();
			for (int num4 = this.ignorePixelLeft; num4 < num2; num4++)
			{
				System.Windows.Point item4 = default(System.Windows.Point);
				bool isDualDetector5 = this.details.IsDualDetector;
				if (isDualDetector5)
				{
					item4.X = (double)num4;
					item4.Y = clacImg[num4].TotalAverageHighEnergy;
					this.plotPointsDualEnergy.Add(item4);
					this.sesoplot.Graphs[8].Points = this.plotPointsDualEnergy;
					item4.X = (double)clacImg[num4].Pixelnumber;
					item4.Y = clacImg[num4].TotalAverageLowEnergy;
					this.plotPoints.Add(item4);
					this.sesoplot.Graphs[11].Points = this.plotPoints;
				}
				else
				{
					item4.X = (double)clacImg[num4].Pixelnumber;
					item4.Y = clacImg[num4].TotalAverageLowEnergy;
					this.plotPoints.Add(item4);
					this.sesoplot.Graphs[11].Points = this.plotPoints;
				}
			}
			this.plotPoints = new List<System.Windows.Point>();
			this.plotPointsDualEnergy = new List<System.Windows.Point>();
			for (int num5 = this.ignorePixelLeft; num5 < num2; num5++)
			{
				System.Windows.Point item5 = default(System.Windows.Point);
				bool isDualDetector6 = this.details.IsDualDetector;
				if (isDualDetector6)
				{
					item5.X = (double)num5;
					item5.Y = clacImg[num5].MaximumHighEnergy;
					this.plotPointsDualEnergy.Add(item5);
					this.sesoplot.Graphs[6].Points = this.plotPointsDualEnergy;
					item5.X = (double)clacImg[num5].Pixelnumber;
					item5.Y = clacImg[num5].MaximumLowEnergy;
					this.plotPoints.Add(item5);
					this.sesoplot.Graphs[9].Points = this.plotPoints;
				}
				else
				{
					item5.X = (double)clacImg[num5].Pixelnumber;
					item5.Y = clacImg[num5].MaximumLowEnergy;
					this.plotPoints.Add(item5);
					this.sesoplot.Graphs[9].Points = this.plotPoints;
				}
			}
			this.plotPoints = new List<System.Windows.Point>();
			this.plotPointsDualEnergy = new List<System.Windows.Point>();
			for (int num6 = this.ignorePixelLeft; num6 < num2; num6++)
			{
				System.Windows.Point item6 = default(System.Windows.Point);
				bool isDualDetector7 = this.details.IsDualDetector;
				if (isDualDetector7)
				{
					item6.X = (double)num6;
					item6.Y = clacImg[num6].MinimumHighEnergy;
					this.plotPointsDualEnergy.Add(item6);
					this.sesoplot.Graphs[7].Points = this.plotPointsDualEnergy;
					item6.X = (double)clacImg[num6].Pixelnumber;
					item6.Y = clacImg[num6].MinimumLowEnergy;
					this.plotPoints.Add(item6);
					this.sesoplot.Graphs[10].Points = this.plotPoints;
				}
				else
				{
					item6.X = (double)clacImg[num6].Pixelnumber;
					item6.Y = clacImg[num6].MinimumLowEnergy;
					this.plotPoints.Add(item6);
					this.sesoplot.Graphs[10].Points = this.plotPoints;
				}
			}
			this.plotPoints = new List<System.Windows.Point>();
			this.plotPointsDualEnergy = new List<System.Windows.Point>();
			for (int num7 = this.ignorePixelLeft; num7 < num2; num7++)
			{
				System.Windows.Point item7 = default(System.Windows.Point);
				bool isDualDetector8 = this.details.IsDualDetector;
				if (isDualDetector8)
				{
					item7.X = (double)num7;
					item7.Y = clacImg[num7 + this.calcImage.CalcImageList.Count / 2].Standarddevation;
					this.plotPointsDualEnergy.Add(item7);
					this.sesoplot.Graphs[2].Points = this.plotPointsDualEnergy;
					item7.X = (double)clacImg[num7].Pixelnumber;
					item7.Y = clacImg[num7].Standarddevation;
					this.plotPoints.Add(item7);
					this.sesoplot.Graphs[3].Points = this.plotPoints;
				}
				else
				{
					item7.X = (double)clacImg[num7].Pixelnumber;
					item7.Y = clacImg[num7].Standarddevation;
					this.plotPoints.Add(item7);
					this.sesoplot.Graphs[3].Points = this.plotPoints;
				}
			}
			bool flag3 = this.ignorePixelLeft != 0;
			if (flag3)
			{
				this.plotPoints = new List<System.Windows.Point>();
				for (int num8 = 0; num8 < this.calcImage.CalcImageList.Count; num8++)
				{
					System.Windows.Point item8 = default(System.Windows.Point);
					item8.X = (double)this.ignorePixelLeft;
					item8.Y = (double)(num8 + num8 * 25);
					this.plotPoints.Add(item8);
					this.sesoplot.Graphs[14].Points = this.plotPoints;
				}
			}
			else
			{
				this.plotPoints = new List<System.Windows.Point>();
				this.sesoplot.Graphs[14].Points = this.plotPoints;
			}
			bool flag4 = this.ignorePixelRight != 0;
			if (flag4)
			{
				this.plotPoints = new List<System.Windows.Point>();
				for (int num9 = 0; num9 < this.calcImage.CalcImageList.Count; num9++)
				{
					System.Windows.Point item9 = default(System.Windows.Point);
					item9.X = (double)num2;
					item9.Y = (double)(num9 + num9 * 25);
					this.plotPoints.Add(item9);
					this.sesoplot.Graphs[15].Points = this.plotPoints;
				}
			}
		}

		// Token: 0x06003491 RID: 13457 RVA: 0x0014C594 File Offset: 0x0014A794
		private void getMinMaxfromReferenceplots()
		{
			double num = 10000000.0;
			double num2 = 0.0;
			bool flag = this.sesoplot.Graphs[12].Points != null;
			if (flag)
			{
				bool? isChecked = this.cBReferencePlot.IsChecked;
				bool flag2 = true;
				bool flag3 = isChecked.GetValueOrDefault() == flag2 & isChecked != null;
				if (flag3)
				{
					for (int i = 0; i < this.sesoplot.Graphs[12].Points.Count; i++)
					{
						bool flag4 = num > this.sesoplot.Graphs[12].Points[i].Y;
						if (flag4)
						{
							num = this.sesoplot.Graphs[12].Points[i].Y;
						}
						bool flag5 = num2 < this.sesoplot.Graphs[12].Points[i].Y;
						if (flag5)
						{
							num2 = this.sesoplot.Graphs[12].Points[i].Y;
						}
					}
				}
			}
			bool flag6 = num != 10000000.0 || num2 != 0.0;
			if (flag6)
			{
				this.refPlotMaximum = num2;
				this.refPlotMinimum = num;
			}
		}

		// Token: 0x06003492 RID: 13458 RVA: 0x0014C728 File Offset: 0x0014A928
		private void Autoscale(bool updateSesoplot)
		{
			this.highestValueYAxis = 0.1;
			bool? isChecked = this.cBImageAverage.IsChecked;
			bool flag = true;
			bool flag2 = isChecked.GetValueOrDefault() == flag & isChecked != null;
			if (flag2)
			{
				for (int i = this.ignorePixelLeft; i < this.calcImage.CalcImageList.Count - this.ignorePixelRight; i++)
				{
					bool flag3 = this.calcImage.CalcImageList[i].Average / (this.ratioOfGreyvalueSNRLowEnergy * 1.3) > this.highestValueYAxis;
					if (flag3)
					{
						this.highestValueYAxis = this.calcImage.CalcImageList[i].Average / (this.ratioOfGreyvalueSNRLowEnergy * 1.3);
					}
				}
			}
			isChecked = this.cBStandarddevation.IsChecked;
			flag = true;
			bool flag4 = isChecked.GetValueOrDefault() == flag & isChecked != null;
			if (flag4)
			{
				for (int j = this.ignorePixelLeft; j < this.calcImage.CalcImageList.Count - this.ignorePixelRight; j++)
				{
					bool flag5 = this.highestValueYAxis < this.calcImage.CalcImageList[j].Standarddevation;
					if (flag5)
					{
						this.highestValueYAxis = this.calcImage.CalcImageList[j].Standarddevation;
					}
				}
			}
			bool flag6 = !this.details.IsDualDetector;
			if (flag6)
			{
				isChecked = this.cBSNR.IsChecked;
				flag = true;
				bool flag7 = isChecked.GetValueOrDefault() == flag & isChecked != null;
				if (flag7)
				{
					bool flag8 = this.highestValueYAxis < this.calcImage.CalcImageList[1].MaximumLowEnergy;
					if (flag8)
					{
						this.highestValueYAxis = this.calcImage.CalcImageList[1].MaximumLowEnergy;
					}
				}
			}
			else
			{
				isChecked = this.cBSNRHighEnergy.IsChecked;
				flag = true;
				bool flag9;
				if (!(isChecked.GetValueOrDefault() == flag & isChecked != null))
				{
					isChecked = this.cBSNR.IsChecked;
					flag = true;
					flag9 = (isChecked.GetValueOrDefault() == flag & isChecked != null);
				}
				else
				{
					flag9 = true;
				}
				bool flag10 = flag9;
				if (flag10)
				{
					bool flag11 = this.highestValueYAxis < this.calcImage.CalcImageList[1].MaximumLowEnergy;
					if (flag11)
					{
						this.highestValueYAxis = this.calcImage.CalcImageList[1].MaximumLowEnergy;
					}
				}
			}
			isChecked = this.cBReferencePlot.IsChecked;
			flag = true;
			bool flag12 = isChecked.GetValueOrDefault() == flag & isChecked != null;
			if (flag12)
			{
				this.getMinMaxfromReferenceplots();
				bool flag13 = this.highestValueYAxis < this.refPlotMaximum;
				if (flag13)
				{
					this.highestValueYAxis = this.refPlotMaximum;
				}
			}
			bool flag14 = updateSesoplot && this.highestValueYAxis != 0.1;
			if (flag14)
			{
				this.sesoplot.MajorTicksY = 5.0;
				this.sesoplot.MinorTicksY = 1.0;
				bool isDualDetector = this.details.IsDualDetector;
				if (isDualDetector)
				{
					this.sesoplot.MaximumY = this.highestValueYAxis * 1.5;
					this.sesoplot.MinimumY = 0.0;
				}
				else
				{
					this.sesoplot.MaximumY = this.highestValueYAxis * 1.5;
					this.sesoplot.MinimumY = 0.0;
				}
				bool flag15 = this.sesoplot.MaximumY > 150.0;
				if (flag15)
				{
					this.sesoplot.MajorTicksY = 10.0;
					this.sesoplot.MinorTicksY = 5.0;
				}
				bool flag16 = this.sesoplot.MaximumY > 500.0;
				if (flag16)
				{
					this.sesoplot.MajorTicksY = 50.0;
					this.sesoplot.MinorTicksY = 10.0;
				}
				bool flag17 = this.sesoplot.MaximumY > 1500.0;
				if (flag17)
				{
					this.sesoplot.MajorTicksY = 100.0;
					this.sesoplot.MinorTicksY = 50.0;
				}
				bool flag18 = this.sesoplot.MaximumY > 4000.0;
				if (flag18)
				{
					this.sesoplot.MajorTicksY = 1000.0;
					this.sesoplot.MinorTicksY = 500.0;
				}
				bool flag19 = this.sesoplot.MaximumY - this.sesoplot.MinimumY < 15.0;
				if (flag19)
				{
					this.sesoplot.MajorTicksY = 1.0;
					this.sesoplot.MinorTicksY = 1.0;
				}
				bool isDualDetector2 = this.details.IsDualDetector;
				if (isDualDetector2)
				{
					bool flag20;
					if (this.sesoplot.Graphs[5].Points.Count < this.snrPlot.PixelCountReferencePlot)
					{
						isChecked = this.cBReferencePlot.IsChecked;
						flag = true;
						flag20 = (isChecked.GetValueOrDefault() == flag & isChecked != null);
					}
					else
					{
						flag20 = false;
					}
					bool flag21 = flag20;
					if (flag21)
					{
						this.sesoplot.MaximumX = (double)(this.snrPlot.PixelCountReferencePlot + 1);
					}
				}
				else
				{
					this.sesoplot.MaximumX = (double)(this.imageWidth + 1);
				}
				bool flag22;
				if (this.sesoplot.Graphs[5].Points.Count < this.snrPlot.PixelCountReferencePlot)
				{
					isChecked = this.cBReferencePlot.IsChecked;
					flag = true;
					flag22 = (isChecked.GetValueOrDefault() == flag & isChecked != null);
				}
				else
				{
					flag22 = false;
				}
				bool flag23 = flag22;
				if (flag23)
				{
					this.sesoplot.MaximumX = (double)(this.snrPlot.PixelCountReferencePlot + 1);
				}
			}
		}

		// Token: 0x06003493 RID: 13459 RVA: 0x0014CD4C File Offset: 0x0014AF4C
		private void checkSNRPlot(int pixel)
		{
			this.snrPlot.checkSNRPlot(this.serialnumberModified);
			bool dbSnrEntryExist = this.snrPlot.dbSnrEntryExist;
			if (dbSnrEntryExist)
			{
				this.gReferencePlotON.Visibility = Visibility.Visible;
				this.imgXrayONexist.Visibility = Visibility.Visible;
				this.imgXrayONNoEntry.Visibility = Visibility.Hidden;
			}
			else
			{
				this.gReferencePlotON.Visibility = Visibility.Collapsed;
				this.imgXrayONexist.Visibility = Visibility.Hidden;
				this.imgXrayONNoEntry.Visibility = Visibility.Visible;
			}
		}

		// Token: 0x06003494 RID: 13460 RVA: 0x0014CDD0 File Offset: 0x0014AFD0
		private void cBSNRAverage_Click(object sender, RoutedEventArgs e)
		{
			bool isDualDetector = this.details.IsDualDetector;
			if (isDualDetector)
			{
				bool? isCheckedReal = this.cBSNRAverageMinMax.IsCheckedReal;
				bool flag = true;
				bool flag2 = !(isCheckedReal.GetValueOrDefault() == flag & isCheckedReal != null);
				if (flag2)
				{
					this.sesoplot.Graphs[6].Stroke = System.Windows.Media.Brushes.Transparent;
					this.sesoplot.Graphs[7].Stroke = System.Windows.Media.Brushes.Transparent;
					this.sesoplot.Graphs[8].Stroke = System.Windows.Media.Brushes.Transparent;
					this.sesoplot.Graphs[9].Stroke = System.Windows.Media.Brushes.Transparent;
					this.sesoplot.Graphs[10].Stroke = System.Windows.Media.Brushes.Transparent;
					this.sesoplot.Graphs[11].Stroke = System.Windows.Media.Brushes.Transparent;
					this.ColumnLegendMinMaxAverageDualEnergy.Width = this.hideGridColumn;
				}
				isCheckedReal = this.cBSNR.IsCheckedReal;
				flag = true;
				bool flag3;
				if (isCheckedReal.GetValueOrDefault() == flag & isCheckedReal != null)
				{
					isCheckedReal = this.cBSNRHighEnergy.IsCheckedReal;
					flag = true;
					if (isCheckedReal.GetValueOrDefault() == flag & isCheckedReal != null)
					{
						isCheckedReal = this.cBSNRAverageMinMax.IsCheckedReal;
						flag = true;
						flag3 = (isCheckedReal.GetValueOrDefault() == flag & isCheckedReal != null);
						goto IL_164;
					}
				}
				flag3 = false;
				IL_164:
				bool flag4 = flag3;
				if (flag4)
				{
					this.sesoplot.Graphs[6].Stroke = System.Windows.Media.Brushes.DarkGray;
					this.sesoplot.Graphs[7].Stroke = System.Windows.Media.Brushes.DarkGray;
					this.sesoplot.Graphs[8].Stroke = System.Windows.Media.Brushes.DarkGray;
					this.sesoplot.Graphs[9].Stroke = System.Windows.Media.Brushes.DarkGray;
					this.sesoplot.Graphs[10].Stroke = System.Windows.Media.Brushes.DarkGray;
					this.sesoplot.Graphs[11].Stroke = System.Windows.Media.Brushes.DarkGray;
					this.ColumnLegendMinMaxAverageDualEnergy.Width = this.gridLenghtLegendDualEnergy[0];
				}
				isCheckedReal = this.cBSNR.IsCheckedReal;
				flag = true;
				bool flag5;
				if (isCheckedReal.GetValueOrDefault() == flag & isCheckedReal != null)
				{
					isCheckedReal = this.cBSNRHighEnergy.IsCheckedReal;
					flag = false;
					if (isCheckedReal.GetValueOrDefault() == flag & isCheckedReal != null)
					{
						isCheckedReal = this.cBSNRAverageMinMax.IsCheckedReal;
						flag = true;
						flag5 = (isCheckedReal.GetValueOrDefault() == flag & isCheckedReal != null);
						goto IL_299;
					}
				}
				flag5 = false;
				IL_299:
				bool flag6 = flag5;
				if (flag6)
				{
					this.sesoplot.Graphs[6].Stroke = System.Windows.Media.Brushes.Transparent;
					this.sesoplot.Graphs[7].Stroke = System.Windows.Media.Brushes.Transparent;
					this.sesoplot.Graphs[8].Stroke = System.Windows.Media.Brushes.Transparent;
					this.sesoplot.Graphs[9].Stroke = System.Windows.Media.Brushes.DarkGray;
					this.sesoplot.Graphs[10].Stroke = System.Windows.Media.Brushes.DarkGray;
					this.sesoplot.Graphs[11].Stroke = System.Windows.Media.Brushes.DarkGray;
					this.ColumnLegendMinMaxAverageDualEnergy.Width = this.gridLenghtLegendDualEnergy[0];
				}
				isCheckedReal = this.cBSNR.IsCheckedReal;
				flag = false;
				bool flag7;
				if (isCheckedReal.GetValueOrDefault() == flag & isCheckedReal != null)
				{
					isCheckedReal = this.cBSNRHighEnergy.IsCheckedReal;
					flag = true;
					if (isCheckedReal.GetValueOrDefault() == flag & isCheckedReal != null)
					{
						isCheckedReal = this.cBSNRAverageMinMax.IsCheckedReal;
						flag = true;
						flag7 = (isCheckedReal.GetValueOrDefault() == flag & isCheckedReal != null);
						goto IL_3CE;
					}
				}
				flag7 = false;
				IL_3CE:
				bool flag8 = flag7;
				if (flag8)
				{
					this.sesoplot.Graphs[6].Stroke = System.Windows.Media.Brushes.DarkGray;
					this.sesoplot.Graphs[7].Stroke = System.Windows.Media.Brushes.DarkGray;
					this.sesoplot.Graphs[8].Stroke = System.Windows.Media.Brushes.DarkGray;
					this.sesoplot.Graphs[9].Stroke = System.Windows.Media.Brushes.Transparent;
					this.sesoplot.Graphs[10].Stroke = System.Windows.Media.Brushes.Transparent;
					this.sesoplot.Graphs[11].Stroke = System.Windows.Media.Brushes.Transparent;
					this.ColumnLegendMinMaxAverageDualEnergy.Width = this.gridLenghtLegendDualEnergy[0];
				}
			}
			else
			{
				bool? isCheckedReal = this.cBSNRAverageMinMax.IsCheckedReal;
				bool flag = true;
				bool flag9 = !(isCheckedReal.GetValueOrDefault() == flag & isCheckedReal != null);
				if (flag9)
				{
					this.sesoplot.Graphs[9].Stroke = System.Windows.Media.Brushes.Transparent;
					this.sesoplot.Graphs[10].Stroke = System.Windows.Media.Brushes.Transparent;
					this.sesoplot.Graphs[11].Stroke = System.Windows.Media.Brushes.Transparent;
					this.ColumnLegendMinMaxAverageSingleEnergy.Width = this.hideGridColumn;
				}
				else
				{
					this.sesoplot.Graphs[9].Stroke = System.Windows.Media.Brushes.DarkGray;
					this.sesoplot.Graphs[10].Stroke = System.Windows.Media.Brushes.DarkGray;
					this.sesoplot.Graphs[11].Stroke = System.Windows.Media.Brushes.DarkGray;
					this.ColumnLegendMinMaxAverageSingleEnergy.Width = this.gridLenghtLegendSingleEnergy[0];
				}
			}
			this.Autoscale(true);
		}

		// Token: 0x06003495 RID: 13461 RVA: 0x0014D390 File Offset: 0x0014B590
		private void cBStandarddevation_Click(object sender, RoutedEventArgs e)
		{
			bool isDualDetector = this.details.IsDualDetector;
			if (isDualDetector)
			{
				bool? isCheckedReal = this.cBStandarddevation.IsCheckedReal;
				bool flag = true;
				bool flag2 = !(isCheckedReal.GetValueOrDefault() == flag & isCheckedReal != null);
				if (flag2)
				{
					this.sesoplot.Graphs[2].Stroke = System.Windows.Media.Brushes.Transparent;
					this.sesoplot.Graphs[3].Stroke = System.Windows.Media.Brushes.Transparent;
					this.ColumnLegendStandarddevationDualEnergy.Width = this.hideGridColumn;
				}
				else
				{
					this.sesoplot.Graphs[2].Stroke = System.Windows.Media.Brushes.DeepSkyBlue;
					this.sesoplot.Graphs[3].Stroke = System.Windows.Media.Brushes.LightGreen;
					this.ColumnLegendStandarddevationDualEnergy.Width = this.gridLenghtLegendDualEnergy[0];
				}
			}
			else
			{
				bool? isCheckedReal = this.cBStandarddevation.IsCheckedReal;
				bool flag = true;
				bool flag3 = !(isCheckedReal.GetValueOrDefault() == flag & isCheckedReal != null);
				if (flag3)
				{
					this.sesoplot.Graphs[3].Stroke = System.Windows.Media.Brushes.Transparent;
					this.ColumnLegendStandarddevation.Width = this.hideGridColumn;
				}
				else
				{
					this.sesoplot.Graphs[3].Stroke = System.Windows.Media.Brushes.Blue;
					this.ColumnLegendStandarddevation.Width = this.gridLenghtLegendSingleEnergy[0];
				}
			}
			this.Autoscale(true);
		}

		// Token: 0x06003496 RID: 13462 RVA: 0x0014D51C File Offset: 0x0014B71C
		private void cBImageAverage_Click(object sender, RoutedEventArgs e)
		{
			bool isDualDetector = this.details.IsDualDetector;
			if (isDualDetector)
			{
				bool? isCheckedReal = this.cBImageAverage.IsCheckedReal;
				bool flag = true;
				bool flag2 = !(isCheckedReal.GetValueOrDefault() == flag & isCheckedReal != null);
				if (flag2)
				{
					this.sesoplot.Graphs[0].Stroke = System.Windows.Media.Brushes.Transparent;
					this.sesoplot.Graphs[1].Stroke = System.Windows.Media.Brushes.Transparent;
					this.ColumnLegendGreyvalueDualEnergy.Width = this.hideGridColumn;
				}
				else
				{
					this.sesoplot.Graphs[0].Stroke = System.Windows.Media.Brushes.Blue;
					this.sesoplot.Graphs[1].Stroke = System.Windows.Media.Brushes.Green;
					this.sesoplot.Graphs[0].StrokeDashArray = this.refdashstyle;
					this.sesoplot.Graphs[1].StrokeDashArray = this.refdashstyle;
					this.ColumnLegendGreyvalueDualEnergy.Width = this.gridLenghtLegendDualEnergy[0];
				}
			}
			else
			{
				bool? isCheckedReal = this.cBImageAverage.IsCheckedReal;
				bool flag = true;
				bool flag3 = !(isCheckedReal.GetValueOrDefault() == flag & isCheckedReal != null);
				if (flag3)
				{
					this.sesoplot.Graphs[1].Stroke = System.Windows.Media.Brushes.Transparent;
					this.ColumnLegendGreyvalue.Width = this.hideGridColumn;
				}
				else
				{
					this.sesoplot.Graphs[1].Stroke = System.Windows.Media.Brushes.Green;
					this.sesoplot.Graphs[1].StrokeDashArray = this.refdashstyle;
					this.ColumnLegendGreyvalue.Width = this.gridLenghtLegendSingleEnergy[0];
				}
			}
			this.Autoscale(true);
		}

		// Token: 0x06003497 RID: 13463 RVA: 0x0014D700 File Offset: 0x0014B900
		private void cBSNR_Click(object sender, RoutedEventArgs e)
		{
			bool isDualDetector = this.details.IsDualDetector;
			if (isDualDetector)
			{
				bool? isCheckedReal = this.cBSNR.IsCheckedReal;
				bool flag = true;
				bool flag2 = !(isCheckedReal.GetValueOrDefault() == flag & isCheckedReal != null);
				if (flag2)
				{
					bool flag3 = !this.details.IsNoTdiScanMode;
					if (flag3)
					{
						this.sesoplot.Graphs[12].Stroke = System.Windows.Media.Brushes.DarkRed;
					}
					else
					{
						this.sesoplot.Graphs[12].Stroke = System.Windows.Media.Brushes.Goldenrod;
					}
					this.sesoplot.Graphs[9].Stroke = System.Windows.Media.Brushes.Transparent;
					this.sesoplot.Graphs[10].Stroke = System.Windows.Media.Brushes.Transparent;
					this.sesoplot.Graphs[11].Stroke = System.Windows.Media.Brushes.Transparent;
					this.lLegendDualLowEnergy.Visibility = Visibility.Collapsed;
					this.LineLowORSingleEnergySNR.Visibility = Visibility.Collapsed;
				}
				else
				{
					bool flag4 = !this.details.IsNoTdiScanMode;
					if (flag4)
					{
						this.sesoplot.Graphs[12].Stroke = System.Windows.Media.Brushes.DarkRed;
					}
					else
					{
						this.sesoplot.Graphs[12].Stroke = System.Windows.Media.Brushes.Goldenrod;
					}
					this.ColumnLegendSNRDualEnergy.Width = this.gridLenghtLegendDualEnergy[0];
					this.lLegendDualLowEnergy.Visibility = Visibility.Visible;
					this.LineLowORSingleEnergySNR.Visibility = Visibility.Visible;
					isCheckedReal = this.cBSNRAverageMinMax.IsCheckedReal;
					flag = true;
					bool flag5 = isCheckedReal.GetValueOrDefault() == flag & isCheckedReal != null;
					if (flag5)
					{
						this.sesoplot.Graphs[9].Stroke = System.Windows.Media.Brushes.DarkGray;
						this.sesoplot.Graphs[10].Stroke = System.Windows.Media.Brushes.DarkGray;
						this.sesoplot.Graphs[11].Stroke = System.Windows.Media.Brushes.DarkGray;
					}
				}
			}
			else
			{
				bool? isCheckedReal = this.cBSNR.IsCheckedReal;
				bool flag = true;
				bool flag6 = !(isCheckedReal.GetValueOrDefault() == flag & isCheckedReal != null);
				if (flag6)
				{
					this.sesoplot.Graphs[5].Stroke = System.Windows.Media.Brushes.Transparent;
					this.ColumnLegendSNR.Width = this.hideGridColumn;
				}
				else
				{
					bool flag7 = !this.details.IsNoTdiScanMode;
					if (flag7)
					{
						this.sesoplot.Graphs[5].Stroke = System.Windows.Media.Brushes.Green;
						this.sesoplot.Graphs[12].Stroke = System.Windows.Media.Brushes.DarkRed;
					}
					else
					{
						this.sesoplot.Graphs[5].Stroke = System.Windows.Media.Brushes.CadetBlue;
						this.sesoplot.Graphs[12].Stroke = System.Windows.Media.Brushes.Goldenrod;
					}
					this.ColumnLegendSNR.Width = this.gridLenghtLegendSingleEnergy[0];
				}
			}
			this.Autoscale(true);
		}

		// Token: 0x06003498 RID: 13464 RVA: 0x0014DA2C File Offset: 0x0014BC2C
		private void cBSNRHighEnergy_Click(object sender, RoutedEventArgs e)
		{
			bool? isCheckedReal = this.cBSNRHighEnergy.IsCheckedReal;
			bool flag = true;
			bool flag2 = !(isCheckedReal.GetValueOrDefault() == flag & isCheckedReal != null);
			if (flag2)
			{
				this.sesoplot.Graphs[4].Stroke = System.Windows.Media.Brushes.Transparent;
				this.sesoplot.Graphs[6].Stroke = System.Windows.Media.Brushes.Transparent;
				this.sesoplot.Graphs[7].Stroke = System.Windows.Media.Brushes.Transparent;
				this.sesoplot.Graphs[8].Stroke = System.Windows.Media.Brushes.Transparent;
				this.lLegendDualHighEnergy.Visibility = Visibility.Collapsed;
				this.LineSNRHighEnergy.Visibility = Visibility.Collapsed;
				this.ColumnLegendSNRDualEnergy.Width = this.hideGridColumn;
				isCheckedReal = this.cBSNR.IsCheckedReal;
				flag = true;
				bool flag3 = isCheckedReal.GetValueOrDefault() == flag & isCheckedReal != null;
				if (flag3)
				{
					this.ColumnLegendSNRDualEnergy.Width = this.gridLenghtLegendDualEnergy[0];
				}
			}
			else
			{
				this.sesoplot.Graphs[4].Stroke = System.Windows.Media.Brushes.Blue;
				this.lLegendDualHighEnergy.Visibility = Visibility.Visible;
				this.LineSNRHighEnergy.Visibility = Visibility.Visible;
				this.ColumnLegendSNRDualEnergy.Width = this.gridLenghtLegendDualEnergy[0];
				isCheckedReal = this.cBSNRAverageMinMax.IsCheckedReal;
				flag = true;
				bool flag4 = isCheckedReal.GetValueOrDefault() == flag & isCheckedReal != null;
				if (flag4)
				{
					this.sesoplot.Graphs[6].Stroke = System.Windows.Media.Brushes.DarkGray;
					this.sesoplot.Graphs[7].Stroke = System.Windows.Media.Brushes.DarkGray;
					this.sesoplot.Graphs[8].Stroke = System.Windows.Media.Brushes.DarkGray;
					this.ColumnLegendSNRDualEnergy.Width = this.gridLenghtLegendDualEnergy[0];
					isCheckedReal = this.cBSNRAverageMinMax.IsCheckedReal;
					flag = true;
					bool flag5 = isCheckedReal.GetValueOrDefault() == flag & isCheckedReal != null;
					if (flag5)
					{
						this.sesoplot.Graphs[6].Stroke = System.Windows.Media.Brushes.DarkGray;
						this.sesoplot.Graphs[7].Stroke = System.Windows.Media.Brushes.DarkGray;
						this.sesoplot.Graphs[8].Stroke = System.Windows.Media.Brushes.DarkGray;
					}
				}
			}
			this.Autoscale(true);
		}

		// Token: 0x06003499 RID: 13465 RVA: 0x0014DCB0 File Offset: 0x0014BEB0
		private void cBReferencePlot_Click(object sender, RoutedEventArgs e)
		{
			bool? isCheckedReal = this.cBReferencePlot.IsCheckedReal;
			bool flag = true;
			bool flag2 = !(isCheckedReal.GetValueOrDefault() == flag & isCheckedReal != null);
			if (flag2)
			{
				this.sesoplot.Graphs[12].Stroke = System.Windows.Media.Brushes.Transparent;
				this.sesoplot.Graphs[13].Stroke = System.Windows.Media.Brushes.Transparent;
				this.ColumnLegendReferenceSNRDualEnergy.Width = this.hideGridColumn;
				this.ColumnLegendReferenceSNR.Width = this.hideGridColumn;
			}
			else
			{
				bool flag3 = !this.details.IsNoTdiScanMode;
				if (flag3)
				{
					this.sesoplot.Graphs[12].Stroke = System.Windows.Media.Brushes.DarkRed;
				}
				else
				{
					this.sesoplot.Graphs[12].Stroke = System.Windows.Media.Brushes.Goldenrod;
				}
				bool flag4 = this.sesoplot.Graphs[12].Points == null && !this.details.IsDualDetector;
				if (flag4)
				{
					this.snrPlot.LoadSNRPlot(this.serialnumberModified);
					bool flag5 = this.snrPlot.SNRReferenceplots[1].Points != null;
					if (flag5)
					{
						this.sesoplot.Graphs[12] = this.snrPlot.SNRReferenceplots[1];
						this.sesoplot.Graphs[12].Points.RemoveRange(this.sesoplot.Graphs[12].Points.Count - this.ignorePixelRight, this.ignorePixelRight);
						this.sesoplot.Graphs[12].Points.RemoveRange(0, this.ignorePixelLeft);
					}
				}
				bool flag6 = this.sesoplot.Graphs[13].Points == null && this.details.IsDualDetector;
				if (flag6)
				{
					this.snrPlot.LoadSNRPlot(this.serialnumberModified);
					this.sesoplot.Graphs[13] = this.snrPlot.SNRReferenceplots[0];
					this.sesoplot.Graphs[13].Points.RemoveRange(this.sesoplot.Graphs[13].Points.Count - this.ignorePixelRight, this.ignorePixelRight);
					this.sesoplot.Graphs[13].Points.RemoveRange(0, this.ignorePixelLeft);
				}
				bool flag7 = this.snrPlot.SNRReferenceplots[0].Points != null;
				if (flag7)
				{
					this.sesoplot.Graphs[13] = this.snrPlot.SNRReferenceplots[0];
					this.sesoplot.Graphs[13].Points.RemoveRange(this.sesoplot.Graphs[13].Points.Count - this.ignorePixelRight, this.ignorePixelRight);
					this.sesoplot.Graphs[13].Points.RemoveRange(0, this.ignorePixelLeft);
				}
				this.sesoplot.Graphs[12].StrokeThickness = 2.0;
				this.sesoplot.Graphs[12].StrokeDashArray = this.refdashstyle;
				this.sesoplot.Graphs[13].Stroke = System.Windows.Media.Brushes.DarkTurquoise;
				this.sesoplot.Graphs[13].StrokeThickness = 2.0;
				this.sesoplot.Graphs[13].StrokeDashArray = this.refdashstyle;
				bool isDualDetector = this.details.IsDualDetector;
				if (isDualDetector)
				{
					this.ColumnLegendReferenceSNRDualEnergy.Width = this.gridLenghtLegendDualEnergy[0];
				}
				else
				{
					this.ColumnLegendReferenceSNR.Width = this.gridLenghtLegendSingleEnergy[0];
				}
			}
			this.Autoscale(true);
		}

		// Token: 0x0600349A RID: 13466 RVA: 0x0014E104 File Offset: 0x0014C304
		private void toggleBtn_Click(object sender, RoutedEventArgs e)
		{
			bool? isChecked = this.toggleBtn.IsChecked;
			bool flag = true;
			bool flag2 = isChecked.GetValueOrDefault() == flag & isChecked != null;
			if (flag2)
			{
				this.GMain.ColumnDefinitions[0].Width = new GridLength(0.0, GridUnitType.Auto);
			}
			else
			{
				this.GMain.ColumnDefinitions[0].Width = new GridLength(60.0, GridUnitType.Pixel);
			}
		}

		// Token: 0x0600349B RID: 13467 RVA: 0x0014E188 File Offset: 0x0014C388
		private void btnSaveDatabase_Click(object sender, RoutedEventArgs e)
		{
			this.snrPlot.IsSnrPlotSaved = false;
			bool flag = this.calcImage.CalcImageList.Count != 100;
			if (flag)
			{
				bool flag2 = Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtOverwriteExisting"), DialogStyle.Warning, DialogButtons.OKCancel) == Dialog.DialogResults.Ok;
				if (flag2)
				{
					this.savingReferenceSNR = true;
					bool isDualDetector = this.details.IsDualDetector;
					if (isDualDetector)
					{
						this.snrPlot.SaveReferenceplotDualEnergy(this.calcImage.CalcImageList, this.serialnumberModified, this.details);
					}
					else
					{
						this.snrPlot.SaveReferenceplotSingleEnergy(this.calcImage.CalcImageList, this.serialnumberModified, this.details, this.dbConnection.Connection);
					}
					this.savingReferenceSNR = false;
					this.checkSNRPlot((int)this.imageWidth);
				}
			}
			else
			{
				Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtError"), DialogStyle.Error, DialogButtons.OK);
			}
			bool isSnrPlotSaved = this.snrPlot.IsSnrPlotSaved;
			if (isSnrPlotSaved)
			{
				Dialog.Show(LocalizationManager.Localize("txtSuccess"), LocalizationManager.Localize("txtSaveSuccessful"), DialogStyle.Ok, DialogButtons.None, true);
				this.sesoplot.Graphs[12].Points = this.sesoplot.Graphs[5].Points;
				bool flag3 = !this.details.IsNoTdiScanMode;
				if (flag3)
				{
					this.sesoplot.Graphs[12].Stroke = System.Windows.Media.Brushes.DarkRed;
				}
				else
				{
					this.sesoplot.Graphs[12].Stroke = System.Windows.Media.Brushes.Goldenrod;
				}
				this.sesoplot.Graphs[12].StrokeThickness = 2.0;
				this.sesoplot.Graphs[12].StrokeDashArray = this.refdashstyle;
				this.sesoplot.Graphs[13].Points = this.sesoplot.Graphs[4].Points;
				this.sesoplot.Graphs[13].Stroke = System.Windows.Media.Brushes.DarkTurquoise;
				this.sesoplot.Graphs[13].StrokeThickness = 2.0;
				this.sesoplot.Graphs[13].StrokeDashArray = this.refdashstyle;
				this.cBReferencePlot.IsCheckedReal = new bool?(true);
				this.Autoscale(true);
				bool isDualDetector2 = this.details.IsDualDetector;
				if (isDualDetector2)
				{
					this.ColumnLegendReferenceSNRDualEnergy.Width = this.gridLenghtLegendDualEnergy[0];
				}
				else
				{
					this.ColumnLegendReferenceSNR.Width = this.gridLenghtLegendSingleEnergy[0];
				}
			}
		}

		// Token: 0x0600349C RID: 13468 RVA: 0x0014E464 File Offset: 0x0014C664
		private void btnSaveCSV_Click(object sender, RoutedEventArgs e)
		{
			CsvFile csvFile = new CsvFile();
			csvFile.IsDualEnergy = this.details.IsDualDetector;
			bool flag = this.imgXrayONexist.Visibility == Visibility.Visible;
			if (flag)
			{
				this.snrPlot.LoadSNRPlot(this.serialnumberModified);
			}
			bool flag2 = this.calcImage.CalcImageList.Count != 100;
			if (flag2)
			{
				csvFile.CreateCsvFile(this.imageWidth, this.snrPlot.SNRReferenceplots[0], this.snrPlot.SNRReferenceplots[1], this.calcImage.CalcImageList, this.snrPlot.DetailsRefPlot, this.details);
				Dialog.Show(LocalizationManager.Localize("txtSuccess"), LocalizationManager.Localize("txtSaveSuccessful"), DialogStyle.Ok, DialogButtons.None, true);
			}
		}

		// Token: 0x0600349D RID: 13469 RVA: 0x0014E538 File Offset: 0x0014C738
		private void btnSavePNG_Click(object sender, RoutedEventArgs e)
		{
			string text = "";
			string text2 = "";
			text = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_ff");
			Rect descendantBounds = VisualTreeHelper.GetDescendantBounds(this.GMain);
			RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap((int)descendantBounds.Width, (int)descendantBounds.Height, 96.0, 96.0, PixelFormats.Pbgra32);
			DrawingVisual drawingVisual = new DrawingVisual();
			string text3 = "";
			bool isTdiDetector = this.details.IsTdiDetector;
			if (isTdiDetector)
			{
				bool isTdiScanMode = this.details.IsTdiScanMode;
				if (isTdiScanMode)
				{
					text3 = "TDI_";
				}
				bool isNoTdiScanMode = this.details.IsNoTdiScanMode;
				if (isNoTdiScanMode)
				{
					text3 = "NoTDI_";
				}
			}
			using (DrawingContext drawingContext = drawingVisual.RenderOpen())
			{
				VisualBrush brush = new VisualBrush(this.GMain);
				drawingContext.DrawRectangle(brush, null, new Rect(default(System.Windows.Point), descendantBounds.Size));
			}
			renderTargetBitmap.Render(drawingVisual);
			bool flag = !Directory.Exists(this.screenshotpath);
			if (flag)
			{
				Directory.CreateDirectory(this.screenshotpath);
			}
			string text4 = string.Concat(new string[]
			{
				"SNRPlot_",
				text3,
				text,
				"_",
				this.details.SNMachine,
				".png"
			});
			FileStream fileStream = new FileStream(this.screenshotpath + text4, FileMode.Create);
			new PngBitmapEncoder
			{
				Frames = 
				{
					BitmapFrame.Create(renderTargetBitmap)
				}
			}.Save(fileStream);
			fileStream.Close();
			fileStream.Dispose();
			DriveInfo[] drives = DriveInfo.GetDrives();
			for (int i = 0; i < drives.Length; i++)
			{
				bool flag2 = drives[i].DriveType == DriveType.Removable;
				if (flag2)
				{
					text2 = drives[i].Name;
				}
			}
			bool flag3 = Directory.Exists(text2);
			if (flag3)
			{
				Directory.CreateDirectory(text2 + "SNRPlot");
				FileStream fileStream2 = new FileStream(text2 + "SNRPlot\\" + text4, FileMode.Create);
				new PngBitmapEncoder
				{
					Frames = 
					{
						BitmapFrame.Create(renderTargetBitmap)
					}
				}.Save(fileStream2);
				fileStream2.Close();
				fileStream2.Dispose();
			}
			Dialog.Show(LocalizationManager.Localize("txtSuccess"), LocalizationManager.Localize("txtSaveSuccessful"), DialogStyle.Ok, DialogButtons.None, true);
			Bitmap image = new Bitmap((int)descendantBounds.Width, (int)descendantBounds.Height);
			Graphics graphics = Graphics.FromImage(image);
		}

		// Token: 0x0600349E RID: 13470 RVA: 0x0014E7E4 File Offset: 0x0014C9E4
		private void Rescale(object obj)
		{
			this.Autoscale(true);
		}

		// Token: 0x0600349F RID: 13471 RVA: 0x0014E7F0 File Offset: 0x0014C9F0
		private void UserControl_Unloaded(object sender, RoutedEventArgs e)
		{
			bool flag = this.iImageArrived != null;
			if (flag)
			{
				this.iImageArrived.ImageArrived -= this.iImageArrived_ImageArrived;
			}
			bool flag2 = this.iPlotDetailsArrived != null;
			if (flag2)
			{
				this.iPlotDetailsArrived.PlotDetailsArrived -= this.iPlotDetailsArrived_PlotDetailsArrived;
			}
			DetectorSubType detectorSubType = DetectorSubType.None;
			string value = "0";
			DetectorType detectorType = DetectorType.DT;
			this.dbConnection.GetDetectorType(ref detectorType);
			string a = this.parHandling.ReadPar("/hardware/detector/sesotec/SubType/Value");
			bool flag3 = a == "1";
			if (flag3)
			{
				detectorSubType = DetectorSubType.SesotecHS;
			}
			bool flag4 = detectorType == DetectorType.DTNetDE || (detectorType == DetectorType.Sesotec && detectorSubType == DetectorSubType.SesotecHS);
			if (flag4)
			{
				value = "1";
			}
			this.parHandling.WriteValue(XRayOnProcessing.manualOffsetCalibrationPath, value, false);
		}

		// Token: 0x04001E62 RID: 7778
		private List<System.Windows.Point> plotPoints = new List<System.Windows.Point>();

		// Token: 0x04001E63 RID: 7779
		private List<System.Windows.Point> plotPointsDualEnergy = new List<System.Windows.Point>();

		// Token: 0x04001E65 RID: 7781
		private IImageArrived iImageArrived;

		// Token: 0x04001E66 RID: 7782
		private IPlotDetailsArrived iPlotDetailsArrived;

		// Token: 0x04001E67 RID: 7783
		private CalculateImage calcImage = new CalculateImage();

		// Token: 0x04001E68 RID: 7784
		private SNRPlot snrPlot = new SNRPlot();

		// Token: 0x04001E69 RID: 7785
		private CsvFile csvFile = new CsvFile();

		// Token: 0x04001E6A RID: 7786
		private ParameterHandling parHandling;

		// Token: 0x04001E6B RID: 7787
		private DBConnection dbConnection;

		// Token: 0x04001E6C RID: 7788
		private short imageWidth;

		// Token: 0x04001E6D RID: 7789
		private short imageHeight;

		// Token: 0x04001E6E RID: 7790
		private short[] imageData;

		// Token: 0x04001E6F RID: 7791
		private double[] dc = new double[]
		{
			0.0,
			2.0
		};

		// Token: 0x04001E70 RID: 7792
		private double[] dcIgnoreLeftRight = new double[]
		{
			0.0,
			2.0
		};

		// Token: 0x04001E71 RID: 7793
		private DoubleCollection refdashstyle;

		// Token: 0x04001E72 RID: 7794
		private bool isDetectorTypeLayoutInitialized = false;

		// Token: 0x04001E73 RID: 7795
		public DockPanel PanelButtons;

		// Token: 0x04001E74 RID: 7796
		public UIElement UiElementRightAligned;

		// Token: 0x04001E75 RID: 7797
		private int ignorePixelLeft;

		// Token: 0x04001E76 RID: 7798
		private int ignorePixelRight;

		// Token: 0x04001E77 RID: 7799
		private string screenshotpath = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\SNRPlot\\";

		// Token: 0x04001E78 RID: 7800
		private const string usbFolderName = "SNRPlot";

		// Token: 0x04001E79 RID: 7801
		private double refPlotMinimum = 100000.0;

		// Token: 0x04001E7A RID: 7802
		private double refPlotMaximum = 0.0;

		// Token: 0x04001E7B RID: 7803
		private string serialnumberModified = "";

		// Token: 0x04001E7C RID: 7804
		private double sumofGreyvaluesHighEnergy = 0.0;

		// Token: 0x04001E7D RID: 7805
		private double sumofGreyvaluesLowEnergy = 0.0;

		// Token: 0x04001E7E RID: 7806
		private double ratioOfGreyvalueSNRHighEnergy = 0.0;

		// Token: 0x04001E7F RID: 7807
		private double ratioOfGreyvalueSNRLowEnergy = 0.0;

		// Token: 0x04001E80 RID: 7808
		private double averageGreyvalueHighEnergy = 0.0;

		// Token: 0x04001E81 RID: 7809
		private double averageGreyvalueLowEnergy = 0.0;

		// Token: 0x04001E82 RID: 7810
		private double highestValueYAxis;

		// Token: 0x04001E83 RID: 7811
		private bool isAutoscaleInitialized = false;

		// Token: 0x04001E85 RID: 7813
		private GridLength hideGridColumn = new GridLength(0.0);

		// Token: 0x04001E86 RID: 7814
		private List<GridLength> gridLenghtLegendDualEnergy;

		// Token: 0x04001E87 RID: 7815
		private List<GridLength> gridLenghtLegendSingleEnergy;

		// Token: 0x04001E88 RID: 7816
		private bool savingReferenceSNR;

		// Token: 0x04001E89 RID: 7817
		private DetectorType detectorType;
	}
}
