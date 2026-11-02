using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using DatabaseConnection;
using Definitions;
using Microsoft.Win32;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x020000DF RID: 223
	public class CtrlTestcardSim : UserControl, IComponentConnector
	{
		// Token: 0x17000695 RID: 1685
		// (get) Token: 0x060015E9 RID: 5609 RVA: 0x000845DB File Offset: 0x000827DB
		// (set) Token: 0x060015EA RID: 5610 RVA: 0x000845E3 File Offset: 0x000827E3
		public BoundLocalizedText StrSimulation { get; set; }

		// Token: 0x060015EB RID: 5611 RVA: 0x000845EC File Offset: 0x000827EC
		public CtrlTestcardSim(ParameterHandling parHandling, StoreImageData imageArchiver, AccessRightsManager accessManager, ImageDataTransfer imageTransfer, DBConnection dbConnection, ProductManager productManager, TestcardSimService testcardSimService, ErrorHandling errorHandling)
		{
			this.InitializeComponent();
			this.accessManager = accessManager;
			this.dbConnection = dbConnection;
			this.imageTransfer = imageTransfer;
			this.parHandling = parHandling;
			this.testcardSimService = testcardSimService;
			List<string> list = new List<string>();
			list.Add(StoreImageData.archiveDirectoryThroughput);
			list.Add(StoreImageData.archiveDirectoryTestcardSimulation);
			this.viewModel = new TestcardSimViewModel(parHandling, this.imagebox, list.ToArray(), imageArchiver, imageTransfer, dbConnection, productManager, testcardSimService, accessManager, errorHandling);
			base.DataContext = this.viewModel;
			this.RegisterLocalizedText();
			this.InitializeButtons();
			bool flag = testcardSimService != null;
			if (flag)
			{
				testcardSimService.MonitoringSensIsActive = false;
			}
			this.imagebox.ButtonPressed += this.Imagebox_ButtonPressed;
			this.imagebox.MouseDown += this.imagebox_MouseDown;
		}

		// Token: 0x060015EC RID: 5612 RVA: 0x000846D4 File Offset: 0x000828D4
		private void InitializeButtons()
		{
			this.PanelButtons = new DockPanel();
			this.PanelButtons.HorizontalAlignment = HorizontalAlignment.Stretch;
			this.PanelButtons.VerticalAlignment = VerticalAlignment.Stretch;
			this.btnSettings = new Button();
			this.btnSettings.Content = ImageConverter.ToImage("/img/settings_small.png");
			this.btnSettings.Style = (base.FindResource("defaultButtonStyle") as Style);
			this.btnSettings.Command = this.viewModel.ShowSettingsCommand;
			this.btnSettings.Margin = new Thickness(0.0, 2.0, 10.0, 2.0);
			this.PanelButtons.Children.Add(this.btnSettings);
			DockPanel.SetDock(this.btnSettings, Dock.Left);
			this.btnStatistics = new Button();
			this.btnStatistics.Content = ImageConverter.ToImage("/img/graphplot_small.png");
			this.btnStatistics.Style = (base.FindResource("defaultButtonStyle") as Style);
			this.btnStatistics.Command = this.viewModel.ShowStatisticsCommand;
			this.btnStatistics.Margin = new Thickness(0.0, 2.0, 10.0, 2.0);
			this.PanelButtons.Children.Add(this.btnStatistics);
			DockPanel.SetDock(this.btnStatistics, Dock.Left);
			this.btnIntegratedComplianceMonitoring = new Button();
			Image image = ImageConverter.ToImage("/img/monitoring.png");
			image.Height = 45.0;
			image.Width = 45.0;
			image.Stretch = Stretch.Uniform;
			image.Margin = new Thickness(0.0, 0.0, 0.0, 10.0);
			this.btnIntegratedComplianceMonitoring.Content = image;
			this.btnIntegratedComplianceMonitoring.Style = (base.FindResource("defaultButtonStyle") as Style);
			this.btnIntegratedComplianceMonitoring.Command = this.viewModel.ShowIntegratedComplianceMonitoring;
			this.btnIntegratedComplianceMonitoring.Margin = new Thickness(0.0, 2.0, 12.0, 0.0);
			this.PanelButtons.Children.Add(this.btnIntegratedComplianceMonitoring);
			DockPanel.SetDock(this.btnIntegratedComplianceMonitoring, Dock.Left);
			Image image2 = ImageConverter.ToImage("/img/retrainregionMinus.png");
			Button button = new Button();
			button.Content = "";
			button.Style = (base.FindResource("defaultButtonStyle") as Style);
			button.Visibility = Visibility.Hidden;
			button.Width = 40.0;
			this.PanelButtons.Children.Add(button);
			this.viewModel.btnStartSimulation.Content = ImageConverter.ToImage("/img/play_bright.png");
			this.viewModel.btnStartSimulation.Style = (base.FindResource("defaultButtonStyle") as Style);
			this.viewModel.btnStartSimulation.Command = this.viewModel.StartSimulationCommand;
			this.viewModel.btnStartSimulation.Margin = new Thickness(0.0, 2.0, 10.0, 2.0);
			this.viewModel.btnStartSimulation.IsEnabled = false;
			this.PanelButtons.Children.Add(this.viewModel.btnStartSimulation);
			DockPanel.SetDock(this.viewModel.btnStartSimulation, Dock.Left);
			string usbLetter = "";
			this.dbConnection.GetUsbDriveLetter(ref usbLetter);
			this.btnSaveToUsb = new ButtonUSB(usbLetter, "/img/saveonusb_small.png", "/img/saveonusbsmall_nousb.png", new RoutedEventHandler(this.SaveMarkedImagesToUSB), false);
			this.PanelButtons.Children.Add(this.btnSaveToUsb);
			DockPanel.SetDock(this.btnSaveToUsb, Dock.Left);
			this.viewModel.btnSimulateFromUsb = new ButtonUSB(usbLetter, "/img/play_usb.png", "/img/play_usb_disabled.png", new RoutedEventHandler(this.SimulateImagefromUSB));
			this.PanelButtons.Children.Add(this.viewModel.btnSimulateFromUsb);
			DockPanel.SetDock(this.viewModel.btnSimulateFromUsb, Dock.Left);
			this.viewModel.btnDelete.Content = ImageConverter.ToImage("/img/deletexriimagelist_small_disabled.png");
			this.viewModel.btnDelete.Click += this.DeleteMarkedImages;
			this.viewModel.btnDelete.Style = (base.FindResource("defaultButtonStyle") as Style);
			this.viewModel.btnDelete.Margin = new Thickness(0.0, 2.0, 10.0, 2.0);
			this.PanelButtons.Children.Add(this.viewModel.btnDelete);
			DockPanel.SetDock(this.viewModel.btnDelete, Dock.Left);
			this.viewModel.btnMarkAll.Content = ImageConverter.ToImage("/img/mark_all.png");
			this.viewModel.btnMarkAll.Click += this.MarkAllImages;
			this.viewModel.btnMarkAll.Style = (base.FindResource("defaultButtonStyle") as Style);
			this.viewModel.btnMarkAll.Margin = new Thickness(0.0, 2.0, 10.0, 2.0);
			this.PanelButtons.Children.Add(this.viewModel.btnMarkAll);
			DockPanel.SetDock(this.viewModel.btnMarkAll, Dock.Left);
			Grid element = new Grid();
			this.PanelButtons.Children.Add(element);
			DockPanel.SetDock(element, Dock.Left);
			this.UiElementRightAligned = new Label
			{
				Content = this.StrSimulation.LocalizedText,
				FontSize = 30.0,
				FontWeight = FontWeights.Bold,
				FontFamily = new FontFamily("Verdana"),
				Foreground = Brushes.DarkGray,
				VerticalAlignment = VerticalAlignment.Center,
				HorizontalAlignment = HorizontalAlignment.Right,
				Margin = new Thickness(0.0, 2.0, 20.0, 2.0)
			};
		}

		// Token: 0x060015ED RID: 5613 RVA: 0x00084D8F File Offset: 0x00082F8F
		private void DeleteMarkedImages(object sender, RoutedEventArgs e)
		{
			this.viewModel.DeleteMarkedImages();
		}

		// Token: 0x060015EE RID: 5614 RVA: 0x00084D9E File Offset: 0x00082F9E
		public void SaveMarkedImagesToUSB(object sender, RoutedEventArgs e)
		{
			this.viewModel.SaveMarkedImages();
		}

		// Token: 0x060015EF RID: 5615 RVA: 0x00084DB0 File Offset: 0x00082FB0
		public void SimulateImagefromUSB(object sender, RoutedEventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			bool value = openFileDialog.ShowDialog().Value;
			if (value)
			{
				bool flag = openFileDialog.FileName.EndsWith(XRSImage.fileEnding);
				if (flag)
				{
					this.viewModel.SimulationfromUSB(openFileDialog.FileName);
				}
			}
		}

		// Token: 0x060015F0 RID: 5616 RVA: 0x00084E00 File Offset: 0x00083000
		public void MarkAllImages(object sender, RoutedEventArgs e)
		{
			bool flag = this.viewModel.AllImagesAreMarked || this.viewModel.SelectedPrievewItems.Count == this.viewModel.Preview.PreviewItems.Count;
			if (flag)
			{
				this.listImages.SelectedItems.Clear();
				this.viewModel.AllImagesAreMarked = false;
				this.viewModel.SelectedItemCount = Convert.ToString(this.viewModel.SelectedPrievewItems.Count);
			}
			else
			{
				this.listImages.SelectAll();
				this.viewModel.AllImagesAreMarked = true;
				this.viewModel.SelectedItemCount = Convert.ToString(this.viewModel.SelectedPrievewItems.Count);
			}
		}

		// Token: 0x060015F1 RID: 5617 RVA: 0x00084EC8 File Offset: 0x000830C8
		private void Imagebox_ButtonPressed()
		{
			this.ShowFullscreenImage();
		}

		// Token: 0x060015F2 RID: 5618 RVA: 0x00084EC8 File Offset: 0x000830C8
		private void imagebox_MouseDown(object sender, MouseButtonEventArgs e)
		{
			this.ShowFullscreenImage();
		}

		// Token: 0x060015F3 RID: 5619 RVA: 0x00084ED4 File Offset: 0x000830D4
		private void ShowFullscreenImage()
		{
			bool flag = !this.bFullscreenImageShown;
			if (flag)
			{
				this.bFullscreenImageShown = true;
				this.ctrlFullscreenImage = new CtrlFullscreenImage(null, false, false, "0", this.accessManager, this.viewModel, null, null);
				this.ctrlFullscreenImage.Unloaded += this.ctrlFullscreenImage_Unloaded;
				Subwindow.Show(this.ctrlFullscreenImage, SubwindowButtons.Back);
			}
		}

		// Token: 0x060015F4 RID: 5620 RVA: 0x00084F40 File Offset: 0x00083140
		private void ctrlFullscreenImage_Unloaded(object sender, RoutedEventArgs e)
		{
			bool flag = this.ctrlFullscreenImage != null;
			if (flag)
			{
				this.ctrlFullscreenImage.Unloaded -= this.ctrlFullscreenImage_Unloaded;
			}
			((XRSImageBoxViewModel)this.imagebox.DataContext).IsXriInfoVisible = false;
			this.imagebox.SetAutoZoomAndBrightness();
			this.bFullscreenImageShown = false;
		}

		// Token: 0x060015F5 RID: 5621 RVA: 0x00084FA0 File Offset: 0x000831A0
		private void lstBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			IList selectedItems = ((ListBox)sender).SelectedItems;
			this.viewModel.SelectedPrievewItems = selectedItems.Cast<PreviewItem>().ToList<PreviewItem>();
			bool flag = this.viewModel.SelectedPrievewItems.Count<PreviewItem>() < this.viewModel.Preview.PreviewItems.Count<PreviewItem>();
			if (flag)
			{
				this.viewModel.AllImagesAreMarked = false;
			}
			bool flag2 = this.viewModel.SelectedPrievewItems != null;
			if (flag2)
			{
				bool flag3 = this.viewModel.SelectedPrievewItems.Count > 3 && this.viewModel.SelectedPrievewItems[0].XrsImage.Height > 3000;
				if (flag3)
				{
					this.viewModel.SelectedPrievewItems[0].IsMarked = false;
					((ListBox)sender).SelectedItems.RemoveAt(0);
				}
				this.viewModel.SelectedItemCount = Convert.ToString(this.viewModel.SelectedPrievewItems.Count);
			}
			else
			{
				this.viewModel.SelectedItemCount = "0";
			}
			bool flag4 = this.viewModel.SelectedPrievewItems.Count > 0;
			if (flag4)
			{
				for (int i = 0; i < this.viewModel.SelectedPrievewItems.Count; i++)
				{
					this.viewModel.SelectedPrievewItems[i].IsMarked = true;
				}
				this.viewModel.SelectedPrievewItems[this.viewModel.SelectedPrievewItems.Count - 1].LoadXRSImage();
				this.imagebox.ViewXRSImage(this.viewModel.SelectedPrievewItems[this.viewModel.SelectedPrievewItems.Count - 1].XrsImage);
				bool showHistoryOrigSim = this.viewModel.ShowHistoryOrigSim;
				if (showHistoryOrigSim)
				{
					this.viewModel.btnStartSimulation.Content = ImageConverter.ToImage("/img/play.png");
					this.viewModel.btnStartSimulation.IsEnabled = true;
				}
				string text = "";
				this.dbConnection.GetUsbDriveLetter(ref text);
				bool flag5 = text != "\\" && Directory.Exists(text);
				if (flag5)
				{
					this.btnSaveToUsb.IsActive = true;
					this.btnSaveToUsb.IsEnabled = true;
				}
				this.viewModel.btnDelete.Content = ImageConverter.ToImage("/img/deletexriimagelist_small.png");
				this.viewModel.btnDelete.IsEnabled = true;
			}
			else
			{
				this.viewModel.btnStartSimulation.Content = ImageConverter.ToImage("/img/play_bright.png");
				this.viewModel.btnStartSimulation.IsEnabled = false;
				this.btnSaveToUsb.IsActive = false;
				this.btnSaveToUsb.IsEnabled = false;
				this.viewModel.btnDelete.Content = ImageConverter.ToImage("/img/deletexriimagelist_small_disabled.png");
				this.viewModel.btnDelete.IsEnabled = false;
			}
		}

		// Token: 0x060015F6 RID: 5622 RVA: 0x000852AB File Offset: 0x000834AB
		private void RegisterLocalizedText()
		{
			this.StrSimulation = new BoundLocalizedText("txtSimulation");
		}

		// Token: 0x060015F7 RID: 5623 RVA: 0x000852BF File Offset: 0x000834BF
		public void DeregisterLocalizedText()
		{
			MainWindow.localizationManager.Deregister(this.StrSimulation);
		}

		// Token: 0x060015F8 RID: 5624 RVA: 0x000852D4 File Offset: 0x000834D4
		private void CtrlTestcardSim_Unloaded(object sender, RoutedEventArgs e)
		{
			this.viewModel.ClosePreview();
			BindingOperations.ClearAllBindings(this);
			base.DataContext = null;
			this.imagebox.ButtonPressed -= this.Imagebox_ButtonPressed;
			this.imagebox.MouseDown -= this.imagebox_MouseDown;
			this.imagebox = null;
			this.imageTransfer.FrameArrived -= this.viewModel.imageTransfer_SimImageArrived;
			this.viewModel.Preview.FirstItemCreated -= this.viewModel.Preview_FirstItemCreated;
			GC.Collect();
			this.viewModel.timerCheckHistoryLoading.Dispose();
			bool flag = this.viewModel.SelectedPrievewItems != null;
			if (flag)
			{
				this.viewModel.SelectedPrievewItems.Clear();
				this.viewModel.SelectedPrievewItems = null;
			}
			this.accessManager.DeregisterControl(this.btnSettings);
			this.accessManager.DeregisterControl(this.btnStatistics);
			this.accessManager.DeregisterControl(this.btnHistogramm);
			this.accessManager.DeregisterControl(this.viewModel.btnStartSimulation);
			this.accessManager.DeregisterControl(this.viewModel.btnSimulateFromUsb);
			this.accessManager.DeregisterControl(this.btnSaveToUsb);
			this.accessManager.DeregisterControl(this.viewModel.btnDelete);
			this.accessManager.DeregisterControl(this.viewModel.btnMarkAll);
			this.DeregisterLocalizedText();
			bool flag2 = !this.viewModel.checkContCountIsEnable;
			if (flag2)
			{
				this.parHandling.WriteValue(Definition.contaminationsCounterEnablePath, "0");
			}
			bool flag3 = this.testcardSimService != null;
			if (flag3)
			{
				this.testcardSimService.MonitoringSensIsActive = true;
			}
		}

		// Token: 0x060015F9 RID: 5625 RVA: 0x000854AC File Offset: 0x000836AC
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/testcardsimulation/ctrltestcardsim.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x060015FA RID: 5626 RVA: 0x000854E4 File Offset: 0x000836E4
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				((CtrlTestcardSim)target).Unloaded += this.CtrlTestcardSim_Unloaded;
				break;
			case 2:
				this.mainGrid = (Grid)target;
				break;
			case 3:
				this.imagebox = (XRSImageBox)target;
				break;
			case 4:
				this.listImages = (ListBox)target;
				this.listImages.SelectionChanged += this.lstBox_OnSelectionChanged;
				break;
			default:
				this._contentLoaded = true;
				break;
			}
		}

		// Token: 0x04000E84 RID: 3716
		private TestcardSimViewModel viewModel;

		// Token: 0x04000E85 RID: 3717
		public DockPanel PanelButtons;

		// Token: 0x04000E86 RID: 3718
		private Button btnSettings;

		// Token: 0x04000E87 RID: 3719
		private Button btnHistogramm;

		// Token: 0x04000E88 RID: 3720
		private Button btnIntegratedComplianceMonitoring;

		// Token: 0x04000E89 RID: 3721
		private Button btnStatistics;

		// Token: 0x04000E8A RID: 3722
		public UIElement UiElementRightAligned;

		// Token: 0x04000E8B RID: 3723
		private CtrlFullscreenImage ctrlFullscreenImage;

		// Token: 0x04000E8C RID: 3724
		private AccessRightsManager accessManager;

		// Token: 0x04000E8D RID: 3725
		private ButtonUSB btnSaveToUsb;

		// Token: 0x04000E8E RID: 3726
		private DBConnection dbConnection;

		// Token: 0x04000E8F RID: 3727
		private ImageDataTransfer imageTransfer;

		// Token: 0x04000E90 RID: 3728
		private ParameterHandling parHandling;

		// Token: 0x04000E92 RID: 3730
		private TestcardSimService testcardSimService;

		// Token: 0x04000E93 RID: 3731
		private bool bFullscreenImageShown = false;

		// Token: 0x04000E94 RID: 3732
		internal Grid mainGrid;

		// Token: 0x04000E95 RID: 3733
		internal XRSImageBox imagebox;

		// Token: 0x04000E96 RID: 3734
		internal ListBox listImages;

		// Token: 0x04000E97 RID: 3735
		private bool _contentLoaded;
	}
}
