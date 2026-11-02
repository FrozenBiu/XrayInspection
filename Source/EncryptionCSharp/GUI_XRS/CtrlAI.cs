using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using DatabaseConnection;
using Definitions;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x020000D2 RID: 210
	public class CtrlAI : UserControl, IComponentConnector
	{
		// Token: 0x17000638 RID: 1592
		// (get) Token: 0x06001476 RID: 5238 RVA: 0x000786D5 File Offset: 0x000768D5
		// (set) Token: 0x06001477 RID: 5239 RVA: 0x000786DD File Offset: 0x000768DD
		public BoundLocalizedText StrSimulation { get; set; }

		// Token: 0x14000069 RID: 105
		// (add) Token: 0x06001478 RID: 5240 RVA: 0x000786E8 File Offset: 0x000768E8
		// (remove) Token: 0x06001479 RID: 5241 RVA: 0x00078720 File Offset: 0x00076920
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event EventHandler UpdateColourBtnSelectionMenu;

		// Token: 0x0600147A RID: 5242 RVA: 0x00078758 File Offset: 0x00076958
		public CtrlAI(ParameterHandling parHandling, StoreImageData imageArchiver, AccessRightsManager accessManager, ImageDataTransfer imageTransfer, DBConnection dbConnection, ProductManager productManager, AIService aiService, ErrorHandling errorHandling, TestcardSimService testcardSimService)
		{
			this.InitializeComponent();
			this.accessManager = accessManager;
			this.dbConnection = dbConnection;
			this.imageTransfer = imageTransfer;
			this.parHandling = parHandling;
			this.aiService = aiService;
			this.testcardSimService = testcardSimService;
			List<string> list = new List<string>();
			list.Add(StoreImageData.archiveDirectoryThroughput);
			list.Add(StoreImageData.archiveDirectoryReject);
			string tableName = "";
			dbConnection.GetCurrentProductTableNameAI(ref tableName);
			string archiveDirectoryAnomalyDetection = imageArchiver.GetArchiveDirectoryAnomalyDetection(tableName);
			list.Add(archiveDirectoryAnomalyDetection);
			this.viewModel = new AIViewModel(parHandling, this.imagebox, list.ToArray(), imageArchiver, imageTransfer, dbConnection, productManager, aiService, accessManager, errorHandling, testcardSimService);
			this.viewModel.SendTestcardSimImages += this.afterAIAcitvation_SendTestcardSimImg;
			base.DataContext = this.viewModel;
			this.RegisterLocalizedText();
			this.InitializeButtons();
			this.imagebox.ButtonPressed += this.Imagebox_ButtonPressed;
			this.imagebox.MouseDown += this.imagebox_MouseDown;
			bool flag = testcardSimService != null;
			if (flag)
			{
				testcardSimService.MonitoringSensIsActive = false;
			}
		}

		// Token: 0x0600147B RID: 5243 RVA: 0x00078884 File Offset: 0x00076A84
		private void InitializeButtons()
		{
			this.PanelButtons = new DockPanel();
			this.PanelButtons.HorizontalAlignment = HorizontalAlignment.Stretch;
			this.PanelButtons.VerticalAlignment = VerticalAlignment.Stretch;
			Button button = new Button();
			button.Content = "";
			button.Style = (base.FindResource("defaultButtonStyle") as Style);
			button.Visibility = Visibility.Hidden;
			button.Width = 40.0;
			this.PanelButtons.Children.Add(button);
			this.viewModel.btnMarkAll.Content = ImageConverter.ToImage("/img/mark_all.png");
			this.viewModel.btnMarkAll.Click += this.MarkAllImages;
			this.viewModel.btnMarkAll.Style = (base.FindResource("defaultButtonStyle") as Style);
			this.viewModel.btnMarkAll.Margin = new Thickness(0.0, 2.0, 10.0, 2.0);
			this.PanelButtons.Children.Add(this.viewModel.btnMarkAll);
			DockPanel.SetDock(this.viewModel.btnMarkAll, Dock.Left);
			this.viewModel.btnStartTrain.Content = ImageConverter.ToImage("/img/retrain_disabled.png");
			this.viewModel.btnStartTrain.Style = (base.FindResource("defaultButtonStyle") as Style);
			this.viewModel.btnStartTrain.Command = this.viewModel.StartTrainingCommand;
			this.viewModel.btnStartTrain.Margin = new Thickness(0.0, 2.0, 10.0, 2.0);
			this.viewModel.btnStartTrain.IsEnabled = false;
			this.PanelButtons.Children.Add(this.viewModel.btnStartTrain);
			DockPanel.SetDock(this.viewModel.btnStartTrain, Dock.Left);
			this.viewModel.btnDeleteMarkedImages.Content = ImageConverter.ToImage("/img/deletexriimagelist_small_disabled.png");
			this.viewModel.btnDeleteMarkedImages.Style = (base.FindResource("defaultButtonStyle") as Style);
			this.viewModel.btnDeleteMarkedImages.Command = this.viewModel.StartDeleteMarkedImagesCommand;
			this.viewModel.btnDeleteMarkedImages.Margin = new Thickness(0.0, 2.0, 10.0, 2.0);
			this.viewModel.btnDeleteMarkedImages.IsEnabled = false;
			this.PanelButtons.Children.Add(this.viewModel.btnDeleteMarkedImages);
			DockPanel.SetDock(this.viewModel.btnDeleteMarkedImages, Dock.Left);
			bool flag = this.viewModel.CheckForBackup() != "";
			if (flag)
			{
				this.viewModel.btnRestoreModel.Content = ImageConverter.ToImage("/img/restoreAI.png");
				this.viewModel.btnRestoreModel.IsEnabled = true;
			}
			else
			{
				this.viewModel.btnRestoreModel.Content = ImageConverter.ToImage("/img/restoreAI_disabled.png");
				this.viewModel.btnRestoreModel.IsEnabled = false;
			}
			this.viewModel.btnRestoreModel.Style = (base.FindResource("defaultButtonStyle") as Style);
			this.viewModel.btnRestoreModel.Command = this.viewModel.StartRestoreModelCommand;
			this.viewModel.btnRestoreModel.Margin = new Thickness(0.0, 2.0, 10.0, 2.0);
			this.PanelButtons.Children.Add(this.viewModel.btnRestoreModel);
			DockPanel.SetDock(this.viewModel.btnRestoreModel, Dock.Left);
			string a = this.parHandling.ReadPar(Definition.anomalyDetectionTensorRTEngineAvailable);
			string a2 = this.parHandling.ReadPar(Definition.anomalyDetectionWeightsInferenceAvailable);
			string a3 = this.parHandling.ReadPar(Definition.anomalyDetectionOnnxAvailable);
			string a4 = this.parHandling.ReadPar(Definition.anomalyDetectionResnetInferenceAvailable);
			bool flag2 = (a == "1" || a2 == "1" || a3 == "1" || a4 == "1") && this.accessManager.CurrentUser.Rights >= BuiltInPriviledges.Admin;
			if (flag2)
			{
				this.viewModel.btnDeleteModel.Content = ImageConverter.ToImage("/img/deleteAI.png");
				this.viewModel.btnDeleteModel.IsEnabled = true;
			}
			else
			{
				this.viewModel.btnDeleteModel.Content = ImageConverter.ToImage("/img/deleteAI_disabled.png");
				this.viewModel.btnDeleteModel.IsEnabled = false;
			}
			this.viewModel.btnDeleteModel.Style = (base.FindResource("defaultButtonStyle") as Style);
			this.viewModel.btnDeleteModel.Command = this.viewModel.StartDeleteModelCommand;
			this.viewModel.btnDeleteModel.Margin = new Thickness(0.0, 2.0, 10.0, 2.0);
			this.PanelButtons.Children.Add(this.viewModel.btnDeleteModel);
			DockPanel.SetDock(this.viewModel.btnDeleteModel, Dock.Left);
			this.viewModel.txtSelectedProduct.Style = (base.FindResource("txtBlockStyle") as Style);
			this.PanelButtons.Children.Add(this.viewModel.txtSelectedProduct);
			DockPanel.SetDock(this.viewModel.txtSelectedProduct, Dock.Right);
			string text = "";
			this.dbConnection.GetUsbDriveLetter(ref text);
			Grid element = new Grid();
			this.PanelButtons.Children.Add(element);
			DockPanel.SetDock(element, Dock.Left);
			this.UiElementRightAligned = new Label
			{
				Content = "THiNK XA",
				FontSize = 30.0,
				FontWeight = FontWeights.Bold,
				FontFamily = new FontFamily("Verdana"),
				Foreground = Brushes.DarkGray,
				VerticalAlignment = VerticalAlignment.Center,
				HorizontalAlignment = HorizontalAlignment.Right,
				Margin = new Thickness(0.0, 2.0, 20.0, 2.0)
			};
		}

		// Token: 0x0600147C RID: 5244 RVA: 0x00078F54 File Offset: 0x00077154
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

		// Token: 0x0600147D RID: 5245 RVA: 0x0007901C File Offset: 0x0007721C
		private void Imagebox_ButtonPressed()
		{
			this.ShowFullscreenImage();
		}

		// Token: 0x0600147E RID: 5246 RVA: 0x0007901C File Offset: 0x0007721C
		private void imagebox_MouseDown(object sender, MouseButtonEventArgs e)
		{
			this.ShowFullscreenImage();
		}

		// Token: 0x0600147F RID: 5247 RVA: 0x00079028 File Offset: 0x00077228
		private void ShowFullscreenImage()
		{
			bool flag = !this.bFullscreenImageShown;
			if (flag)
			{
				this.bFullscreenImageShown = true;
				this.ctrlFullscreenImage = new CtrlFullscreenImage(null, false, false, "0", this.accessManager, null, this.viewModel, null);
				this.ctrlFullscreenImage.Unloaded += this.ctrlFullscreenImage_Unloaded;
				Subwindow.Show(this.ctrlFullscreenImage, SubwindowButtons.Back);
			}
		}

		// Token: 0x06001480 RID: 5248 RVA: 0x00079094 File Offset: 0x00077294
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

		// Token: 0x06001481 RID: 5249 RVA: 0x000790F4 File Offset: 0x000772F4
		private void lstBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			IList selectedItems = ((ListBox)sender).SelectedItems;
			this.viewModel.SelectedPrievewItems = selectedItems.Cast<PreviewItem>().ToList<PreviewItem>();
			bool flag = this.viewModel.SelectedPrievewItems.Count<PreviewItem>() < this.viewModel.Preview.PreviewItems.Count<PreviewItem>();
			if (flag)
			{
				this.viewModel.AllImagesAreMarked = false;
			}
			bool flag2 = this.viewModel.SelectedPrievewItems.Count > 0;
			if (flag2)
			{
				for (int i = 0; i < this.viewModel.SelectedPrievewItems.Count; i++)
				{
					this.viewModel.SelectedPrievewItems[i].IsMarked = true;
					this.viewModel.SelectedPrievewItems[i].LoadXRSImage();
				}
				this.imagebox.ViewXRSImage(this.viewModel.SelectedPrievewItems[this.viewModel.SelectedPrievewItems.Count - 1].XrsImage);
				this.viewModel.btnDeleteMarkedImages.Content = ImageConverter.ToImage("/img/deletexriimagelist_small.png");
				this.viewModel.btnDeleteMarkedImages.IsEnabled = true;
			}
			else
			{
				this.viewModel.btnDeleteMarkedImages.Content = ImageConverter.ToImage("/img/deletexriimagelist_small_disabled.png");
				this.viewModel.btnDeleteMarkedImages.IsEnabled = false;
			}
			bool flag3 = this.viewModel.SelectedPrievewItems != null;
			if (flag3)
			{
				bool flag4 = this.viewModel.SelectedPrievewItems.Count > 3 && this.viewModel.SelectedPrievewItems[0].XrsImage.Height > 3000;
				if (flag4)
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
			bool flag5 = this.viewModel.SelectedPrievewItems.Count >= this.viewModel.minSelectedImages && this.parHandling.ReadPar(Definition.anomalyDetectionWeightsInferenceAvailable) == "1" && !this.viewModel.ShowDatasetImages;
			if (flag5)
			{
				this.viewModel.btnStartTrain.Content = ImageConverter.ToImage("/img/retrain.png");
				this.viewModel.btnStartTrain.IsEnabled = true;
				this.viewModel.ShowFineTuningMessage = false;
			}
			else
			{
				this.viewModel.btnStartTrain.Content = ImageConverter.ToImage("/img/retrain_disabled.png");
				this.viewModel.btnStartTrain.IsEnabled = false;
				this.viewModel.ShowFineTuningMessage = true;
			}
		}

		// Token: 0x06001482 RID: 5250 RVA: 0x000793E4 File Offset: 0x000775E4
		private void afterAIAcitvation_SendTestcardSimImg()
		{
			Thread thread = new Thread(delegate()
			{
				this.viewModel.initprogBarSendingTestcardSim();
				this.testcardSimService.TestcardSimAfterTrainingSendImg(false, null, true);
			});
			thread.Start();
		}

		// Token: 0x06001483 RID: 5251 RVA: 0x0007940B File Offset: 0x0007760B
		private void RegisterLocalizedText()
		{
			this.StrSimulation = new BoundLocalizedText("txtSimulation");
		}

		// Token: 0x06001484 RID: 5252 RVA: 0x0007941F File Offset: 0x0007761F
		public void DeregisterLocalizedText()
		{
			this.viewModel.DeregisterLocalizedText();
			MainWindow.localizationManager.Deregister(this.StrSimulation);
		}

		// Token: 0x06001485 RID: 5253 RVA: 0x00079440 File Offset: 0x00077640
		private void CtrlAI_Unloaded(object sender, RoutedEventArgs e)
		{
			this.viewModel.ClosePreview();
			BindingOperations.ClearAllBindings(this);
			base.DataContext = null;
			this.imagebox.ButtonPressed -= this.Imagebox_ButtonPressed;
			this.imagebox.MouseDown -= this.imagebox_MouseDown;
			this.imagebox = null;
			this.imageTransfer.FrameArrived -= this.viewModel.imageTestcardSimTransfer_FrameArrived;
			this.viewModel.Preview.FirstItemCreated -= this.viewModel.Preview_FirstItemCreated;
			GC.Collect();
			this.viewModel.timerCheckHistoryLoading.Dispose();
			bool flag = this.viewModel.timerCheckErrorTestcardSim != null;
			if (flag)
			{
				this.viewModel.timerCheckErrorTestcardSim.Stop();
				this.viewModel.timerCheckErrorTestcardSim = null;
			}
			bool flag2 = this.viewModel.SelectedPrievewItems != null;
			if (flag2)
			{
				this.viewModel.SelectedPrievewItems.Clear();
				this.viewModel.SelectedPrievewItems = null;
			}
			this.accessManager.DeregisterControl(this.viewModel.btnStartTrain);
			this.accessManager.DeregisterControl(this.viewModel.btnSimulateFromUsb);
			this.accessManager.DeregisterControl(this.viewModel.btnDeleteMarkedImages);
			this.accessManager.DeregisterControl(this.viewModel.btnMarkAll);
			this.accessManager.DeregisterControl(this.viewModel.btnRestoreModel);
			this.accessManager.DeregisterControl(this.viewModel.btnDeleteModel);
			this.accessManager.DeregisterControl(this.viewModel.txtSelectedProduct);
			this.DeregisterLocalizedText();
			bool flag3 = !this.viewModel.checkContCountIsEnable;
			if (flag3)
			{
				this.parHandling.WriteValue(Definition.contaminationsCounterEnablePath, "0");
			}
			bool flag4 = this.testcardSimService != null;
			if (flag4)
			{
				this.testcardSimService.MonitoringSensIsActive = true;
			}
			bool flag5 = this.parHandling.ReadPar(Definition.objectDetectionEnabled) == "1";
			if (flag5)
			{
				EventHandler updateColourBtnSelectionMenu = this.UpdateColourBtnSelectionMenu;
				if (updateColourBtnSelectionMenu != null)
				{
					updateColourBtnSelectionMenu(this, EventArgs.Empty);
				}
			}
		}

		// Token: 0x06001486 RID: 5254 RVA: 0x0007967C File Offset: 0x0007787C
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/ai/ctrlai.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x06001487 RID: 5255 RVA: 0x000796B4 File Offset: 0x000778B4
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				((CtrlAI)target).Unloaded += this.CtrlAI_Unloaded;
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

		// Token: 0x04000D85 RID: 3461
		private AIViewModel viewModel;

		// Token: 0x04000D86 RID: 3462
		public DockPanel PanelButtons;

		// Token: 0x04000D87 RID: 3463
		public UIElement UiElementRightAligned;

		// Token: 0x04000D88 RID: 3464
		private CtrlFullscreenImage ctrlFullscreenImage;

		// Token: 0x04000D89 RID: 3465
		private AccessRightsManager accessManager;

		// Token: 0x04000D8A RID: 3466
		private DBConnection dbConnection;

		// Token: 0x04000D8B RID: 3467
		private ImageDataTransfer imageTransfer;

		// Token: 0x04000D8C RID: 3468
		private ParameterHandling parHandling;

		// Token: 0x04000D8E RID: 3470
		private AIService aiService;

		// Token: 0x04000D8F RID: 3471
		private bool bFullscreenImageShown = false;

		// Token: 0x04000D90 RID: 3472
		private TestcardSimService testcardSimService;

		// Token: 0x04000D92 RID: 3474
		internal Grid mainGrid;

		// Token: 0x04000D93 RID: 3475
		internal XRSImageBox imagebox;

		// Token: 0x04000D94 RID: 3476
		internal ListBox listImages;

		// Token: 0x04000D95 RID: 3477
		private bool _contentLoaded;
	}
}
