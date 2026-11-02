using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;
using DatabaseConnection;
using Definitions;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x02000257 RID: 599
	public class HistoryControl : UserControl, IComponentConnector
	{
		// Token: 0x17000C55 RID: 3157
		// (get) Token: 0x06002DE1 RID: 11745 RVA: 0x0012AF1D File Offset: 0x0012911D
		// (set) Token: 0x06002DE2 RID: 11746 RVA: 0x0012AF25 File Offset: 0x00129125
		public BoundLocalizedText StrSavedOnUSB { get; set; }

		// Token: 0x06002DE3 RID: 11747 RVA: 0x0012AF30 File Offset: 0x00129130
		public HistoryControl(ImageDataTransfer imgTransfer, ParameterHandling parHandling, DBConnection dbConnection, bool throughput, AccessRightsManager accessManager, ProductManager productManager, Lane lane, int imageGroup, StoreImageData imageArchiver, TestcardSimService testcardSimService, SetFooterMainWindow setFooterMainWindow, Sensitivity sensitivity, int number)
		{
			this.InitializeComponent();
			this.accessManager = accessManager;
			this.testcardSimService = testcardSimService;
			this.setFooterMainWindow = setFooterMainWindow;
			this.dbConnection = dbConnection;
			this.Initialize(imgTransfer, parHandling, dbConnection, throughput, accessManager, productManager, lane, imageGroup, imageArchiver, sensitivity, number);
			setFooterMainWindow.SetFooterDeactivation();
			this.InitializeLocalizedText();
		}

		// Token: 0x06002DE4 RID: 11748 RVA: 0x0012AFCB File Offset: 0x001291CB
		private void InitializeLocalizedText()
		{
			this.StrSavedOnUSB = new BoundLocalizedText("txtSavedOnUSB");
		}

		// Token: 0x06002DE5 RID: 11749 RVA: 0x0012AFDF File Offset: 0x001291DF
		private void DeregisterLocalizedText()
		{
			MainWindow.localizationManager.Deregister(this.StrSavedOnUSB);
		}

		// Token: 0x06002DE6 RID: 11750 RVA: 0x0012AFF4 File Offset: 0x001291F4
		private void Initialize(ImageDataTransfer imgTransfer, ParameterHandling parHandling, DBConnection dbConnection, bool throughput, AccessRightsManager accessManager, ProductManager productManager, Lane lane, int imageGroup, StoreImageData imageArchiver, Sensitivity sensitivity, int number)
		{
			List<string> list = new List<string>();
			bool flag = number < 0;
			if (flag)
			{
				list.Add(StoreImageData.archiveDirectoryReject);
			}
			bool flag2 = throughput || number >= 0;
			if (flag2)
			{
				list.Add(StoreImageData.archiveDirectoryThroughput);
			}
			this.viewModel = new HistoryViewModel(this.imagebox, list.ToArray(), 300, parHandling, imgTransfer, dbConnection, productManager, lane, imageGroup, imageArchiver, accessManager.CurrentUser, this.testcardSimService, sensitivity, number);
			this.viewModel.PropertyChanged += this.viewModel_PropertyChanged;
			base.DataContext = this.viewModel;
			this.detectortype = DetectorType.Sesotec;
			dbConnection.GetDetectorType(ref this.detectortype);
			this.InitializeButtons(dbConnection, productManager);
			this.SetControlRights();
			this.imagebox.ButtonPressed += this.Imagebox_ButtonPressed;
			this.imagebox.MouseDown += this.imagebox_MouseDown;
			bool flag3 = this.testcardSimService != null;
			if (flag3)
			{
				this.testcardSimService.MonitoringSensIsActive = false;
			}
		}

		// Token: 0x06002DE7 RID: 11751 RVA: 0x0012B108 File Offset: 0x00129308
		private void InitializeButtons(DBConnection dbConnection, ProductManager productManager)
		{
			dbConnection.GetUsbDriveLetter(ref this.usbPath);
			this.dockpanel = new DockPanel();
			this.dockpanel.HorizontalAlignment = HorizontalAlignment.Stretch;
			this.dockpanel.VerticalAlignment = VerticalAlignment.Stretch;
			Grid.SetColumn(this.dockpanel, 4);
			this.btnRetrain = new Button();
			this.btnRetrain.Content = ImageConverter.ToImage("/img/retrain.png");
			this.btnRetrain.Click += this.Retrain;
			this.btnRetrain.Style = (base.FindResource("defaultButtonStyle") as Style);
			this.btnRetrain.Margin = new Thickness(0.0, 0.0, 10.0, 0.0);
			this.btnRetrain.Visibility = Visibility.Collapsed;
			this.dockpanel.Children.Add(this.btnRetrain);
			DockPanel.SetDock(this.btnRetrain, Dock.Left);
			bool flag = this.detectortype != DetectorType.DTNetDE && !productManager.CurrentLoadedProduct.IsRegionProduct;
			if (flag)
			{
				this.btnUndoRetrain = new Button();
				this.setRetrainUndoButton(false);
				this.btnUndoRetrain.Click += this.UndoRetrain;
				this.btnUndoRetrain.Style = (base.FindResource("defaultButtonStyle") as Style);
				this.btnUndoRetrain.Margin = new Thickness(0.0, 0.0, 10.0, 0.0);
				this.btnUndoRetrain.Visibility = Visibility.Collapsed;
				this.dockpanel.Children.Add(this.btnUndoRetrain);
				DockPanel.SetDock(this.btnUndoRetrain, Dock.Left);
			}
			this.btnRetrainRegion = new Button();
			Image image = ImageConverter.ToImage("/img/retrainregion.png");
			image.Height = 40.0;
			image.Width = 40.0;
			image.Stretch = Stretch.Uniform;
			this.btnRetrainRegion.Content = image;
			this.btnRetrainRegion.Click += this.ShowRetrainRegion;
			this.btnRetrainRegion.Style = (base.FindResource("defaultButtonStyle") as Style);
			this.btnRetrainRegion.Margin = new Thickness(0.0, 0.0, 10.0, 0.0);
			this.btnRetrainRegion.Visibility = Visibility.Collapsed;
			this.dockpanel.Children.Add(this.btnRetrainRegion);
			DockPanel.SetDock(this.btnRetrainRegion, Dock.Left);
			bool isMultiProduct = productManager.CurrentLoadedProduct.IsMultiProduct;
			if (isMultiProduct)
			{
				this.btnRetrainGroup = new Button();
				Image image2 = ImageConverter.ToImage("/img/retrainGroup.png");
				image2.Height = 40.0;
				image2.Width = 40.0;
				image2.Stretch = Stretch.Uniform;
				this.btnRetrainGroup.Content = image2;
				this.btnRetrainGroup.Click += this.RetrainGroup;
				this.btnRetrainGroup.Style = (base.FindResource("defaultButtonStyle") as Style);
				this.btnRetrainGroup.Margin = new Thickness(0.0, 0.0, 10.0, 0.0);
				this.btnRetrainGroup.Visibility = Visibility.Collapsed;
				this.dockpanel.Children.Add(this.btnRetrainGroup);
				DockPanel.SetDock(this.btnRetrainGroup, Dock.Left);
			}
			this.btnSaveSelectedToUsb = new ButtonUSB(this.usbPath, "/img/saveonusb_small.png", "/img/saveonusbsmall_nousb.png", new RoutedEventHandler(this.SaveSelectedToUSB), true);
			this.dockpanel.Children.Add(this.btnSaveSelectedToUsb);
			DockPanel.SetDock(this.btnSaveSelectedToUsb, Dock.Left);
			this.btnDeleteSelected = new Button();
			Image image3 = ImageConverter.ToImage("/img/deletexriimagelist_small.png");
			image3.Height = 40.0;
			image3.Width = 40.0;
			image3.Stretch = Stretch.Uniform;
			this.btnDeleteSelected.Content = image3;
			this.btnDeleteSelected.Click += this.DeleteMarkedImages;
			this.btnDeleteSelected.Style = (base.FindResource("defaultButtonStyle") as Style);
			this.btnDeleteSelected.Margin = new Thickness(0.0, 0.0, 10.0, 0.0);
			this.dockpanel.Children.Add(this.btnDeleteSelected);
			DockPanel.SetDock(this.btnDeleteSelected, Dock.Left);
			this.btnMarkAll = new Button();
			Image image4 = ImageConverter.ToImage("/img/mark_all.png");
			image4.Height = 40.0;
			image4.Width = 40.0;
			image4.Stretch = Stretch.Uniform;
			this.btnMarkAll.Content = image4;
			this.btnMarkAll.Click += this.MarkAllImages;
			this.btnMarkAll.Style = (base.FindResource("defaultButtonStyle") as Style);
			this.btnMarkAll.Margin = new Thickness(0.0, 0.0, 10.0, 0.0);
			this.dockpanel.Children.Add(this.btnMarkAll);
			DockPanel.SetDock(this.btnMarkAll, Dock.Left);
			this.btnShowList = new ButtonUSB(this.usbPath, "/img/usblist.png", "/img/usblist_disabled.png", new RoutedEventHandler(this.showList));
			this.dockpanel.Children.Add(this.btnShowList);
			DockPanel.SetDock(this.btnShowList, Dock.Left);
			bool widescreen = this.viewModel.Widescreen;
			if (widescreen)
			{
				this.btnSaveWithDirectoryname = new ButtonUSB(this.usbPath, "/img/saving_directory.png", "/img/saving_directory_disabled.png", new RoutedEventHandler(this.saveWithDirectoryname));
				this.dockpanel.Children.Add(this.btnSaveWithDirectoryname);
				DockPanel.SetDock(this.btnSaveWithDirectoryname, Dock.Left);
			}
			string text = null;
			dbConnection.GetPrinterName(ref text);
			bool flag2 = text != null && text != "";
			if (flag2)
			{
				this.btnPrint = new Button();
				Image image5 = ImageConverter.ToImage("/img/printer.png");
				image5.Height = 40.0;
				image5.Width = 40.0;
				this.btnPrint.Content = image5;
				this.btnPrint.Click += this.PrintReport;
				this.btnPrint.Style = (base.FindResource("defaultButtonStyle") as Style);
				this.btnPrint.Margin = new Thickness(0.0, 0.0, 10.0, 0.0);
				this.dockpanel.Children.Add(this.btnPrint);
				DockPanel.SetDock(this.btnPrint, Dock.Left);
			}
			this.txtSelectedProduct = new TextBlock();
			this.txtSelectedProduct.Style = (base.FindResource("txtBlockStyle") as Style);
			DockPanel.SetDock(this.txtSelectedProduct, Dock.Right);
			this.dockpanel.Children.Add(this.txtSelectedProduct);
		}

		// Token: 0x06002DE8 RID: 11752 RVA: 0x0012B8B4 File Offset: 0x00129AB4
		private void showList(object sender, RoutedEventArgs e)
		{
			bool fileListVisible = this.viewModel.FileListVisible;
			if (fileListVisible)
			{
				this.viewModel.FileListVisible = false;
			}
			else
			{
				this.getData();
				this.viewModel.FileListVisible = true;
			}
		}

		// Token: 0x06002DE9 RID: 11753 RVA: 0x0012B8F8 File Offset: 0x00129AF8
		private void MarkAllImages(object sender, RoutedEventArgs e)
		{
			bool flag = this.viewModel.AllImagesAreMarked || this.viewModel.SelectedImages.Count == this.viewModel.Preview.PreviewItems.Count;
			if (flag)
			{
				this.lstImages.SelectedItems.Clear();
				this.viewModel.AllImagesAreMarked = false;
				this.viewModel.Number = Convert.ToString(this.viewModel.SelectedImages.Count);
			}
			else
			{
				this.lstImages.SelectAll();
				this.viewModel.AllImagesAreMarked = true;
				this.viewModel.Number = Convert.ToString(this.viewModel.SelectedImages.Count);
			}
		}

		// Token: 0x06002DEA RID: 11754 RVA: 0x0012B9C0 File Offset: 0x00129BC0
		private void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
			HwndSource hwndSource = PresentationSource.FromVisual(this) as HwndSource;
			HwndTarget compositionTarget = hwndSource.CompositionTarget;
			compositionTarget.RenderMode = RenderMode.SoftwareOnly;
			Subwindow subwindow = Window.GetWindow(this) as Subwindow;
			subwindow.buttonGrid.Children.Remove(subwindow.buttonPanel);
			subwindow.buttonGrid.Children.Add(this.dockpanel);
		}

		// Token: 0x06002DEB RID: 11755 RVA: 0x0012BA24 File Offset: 0x00129C24
		private void UserControl_Unloaded(object sender, RoutedEventArgs e)
		{
			this.viewModel.NotRetrained -= this.ViewModel_NotRetrained;
			this.viewModel.DeregisterLocalizedText();
			this.DeregisterLocalizedText();
			bool flag = this.testcardSimService != null;
			if (flag)
			{
				this.testcardSimService.MonitoringSensIsActive = true;
			}
			this.viewModel.SetContinuousModeBulk(true, true);
			this.viewModel.ClosePreview();
			this.viewModel.SelectedImages.Clear();
			this.viewModel.SelectedImages = null;
			BindingOperations.ClearAllBindings(this);
			base.DataContext = null;
			this.accessManager.DeregisterControl(this.btnShowList);
			this.accessManager.DeregisterControl(this.btnSaveSelectedToUsb);
			this.accessManager.DeregisterControl(this.btnMarkAll);
			this.accessManager.DeregisterControl(this.btnShowList);
			bool flag2 = this.btnSaveWithDirectoryname != null;
			if (flag2)
			{
				this.accessManager.DeregisterControl(this.btnSaveWithDirectoryname);
			}
			this.accessManager.DeregisterControl(this.btnDeleteSelected);
			bool flag3 = this.btnUndoRetrain != null;
			if (flag3)
			{
				this.accessManager.DeregisterControl(this.btnUndoRetrain);
			}
			bool flag4 = this.btnPrint != null;
			if (flag4)
			{
				this.accessManager.DeregisterControl(this.btnPrint);
			}
			this.dockpanel.Children.Clear();
			this.imagebox.ButtonPressed -= this.Imagebox_ButtonPressed;
			this.imagebox.MouseDown -= this.imagebox_MouseDown;
			this.viewModel.PropertyChanged -= this.viewModel_PropertyChanged;
			this.imagebox = null;
			GC.Collect();
			bool flag5 = this.setFooterMainWindow != null;
			if (flag5)
			{
				this.setFooterMainWindow.UnSetFooterDeactivation();
			}
		}

		// Token: 0x06002DEC RID: 11756 RVA: 0x0012BBFA File Offset: 0x00129DFA
		private void imagebox_MouseDown(object sender, MouseButtonEventArgs e)
		{
			this.ShowFullscreenImage();
		}

		// Token: 0x06002DED RID: 11757 RVA: 0x0012BC04 File Offset: 0x00129E04
		private void viewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			bool flag = Application.Current != null && Application.Current.Dispatcher.CheckAccess();
			if (flag)
			{
				bool flag2 = e.PropertyName == PreviewViewModel.StrSelectedImage && this.viewModel.SelectedImage != null && this.viewModel.SelectedImage.XrsImage != null;
				if (flag2)
				{
					bool flag3 = this.viewModel.SelectedImage.XrsImage.ProductNumber > 0U && this.viewModel.SelectedImage.XrsImage.ProductName != null;
					if (flag3)
					{
						bool widescreen = this.viewModel.Widescreen;
						if (widescreen)
						{
							this.txtSelectedProduct.Text = "P " + this.viewModel.SelectedImage.XrsImage.ProductNumber.ToString() + ": " + this.viewModel.SelectedImage.XrsImage.ProductName;
						}
						else
						{
							this.txtSelectedProduct.Text = "P " + this.viewModel.SelectedImage.XrsImage.ProductNumber.ToString();
						}
						bool flag4 = this.viewModel.SelectedImage.XrsImage.BatchID != null && this.viewModel.SelectedImage.XrsImage.BatchID.Length > 0;
						if (flag4)
						{
							TextBlock textBlock = this.txtSelectedProduct;
							textBlock.Text = string.Concat(new string[]
							{
								textBlock.Text,
								Environment.NewLine,
								"(",
								this.viewModel.SelectedImage.XrsImage.BatchID,
								")"
							});
						}
					}
					else
					{
						bool flag5 = this.viewModel.SelectedImage.XrsImage.ProductNumber == 0U && this.viewModel.SelectedImage.XrsImage.ProductName == "Reference Product";
						if (flag5)
						{
							bool widescreen2 = this.viewModel.Widescreen;
							if (widescreen2)
							{
								this.txtSelectedProduct.Text = "P -1: " + this.viewModel.SelectedImage.XrsImage.ProductName;
							}
							else
							{
								this.txtSelectedProduct.Text = "P -1";
							}
						}
						else
						{
							this.txtSelectedProduct.Text = "";
						}
					}
					bool flag6 = this.viewModel.SelectedImage.XrsImage != null;
					if (flag6)
					{
						bool flag7 = this.viewModel.CurrentLoadedProduct == null || !this.viewModel.CurrentLoadedProduct.IsMultiProduct;
						if (flag7)
						{
							this.btnRetrain.Visibility = ((this.accessManager.CurrentUser.Rights >= HistoryControl.rightRetrain) ? Visibility.Visible : Visibility.Collapsed);
							this.btnRetrainRegion.Visibility = ((this.accessManager.CurrentUser.Rights >= HistoryControl.rightRetrain) ? Visibility.Visible : Visibility.Collapsed);
							bool flag8 = this.btnUndoRetrain != null;
							if (flag8)
							{
								this.btnUndoRetrain.Visibility = ((this.accessManager.CurrentUser.Rights >= HistoryControl.rightRetrain) ? Visibility.Visible : Visibility.Collapsed);
							}
						}
						else
						{
							bool flag9 = this.viewModel.SelectedImage.XrsImage.Group <= 0;
							if (flag9)
							{
								this.btnRetrain.Visibility = Visibility.Collapsed;
								this.btnRetrainRegion.Visibility = Visibility.Collapsed;
								bool flag10 = this.btnUndoRetrain != null;
								if (flag10)
								{
									this.btnUndoRetrain.Visibility = Visibility.Collapsed;
								}
								bool flag11 = this.btnRetrainGroup != null;
								if (flag11)
								{
									this.btnRetrainGroup.Visibility = ((this.accessManager.CurrentUser.Rights >= HistoryControl.rightRetrain) ? Visibility.Visible : Visibility.Collapsed);
								}
							}
							else
							{
								this.btnRetrain.Visibility = ((this.accessManager.CurrentUser.Rights >= HistoryControl.rightRetrain) ? Visibility.Visible : Visibility.Collapsed);
								this.btnRetrainRegion.Visibility = ((this.accessManager.CurrentUser.Rights >= HistoryControl.rightRetrain) ? Visibility.Visible : Visibility.Collapsed);
								bool flag12 = this.btnUndoRetrain != null;
								if (flag12)
								{
									this.btnUndoRetrain.Visibility = ((this.accessManager.CurrentUser.Rights >= HistoryControl.rightRetrain) ? Visibility.Visible : Visibility.Collapsed);
								}
								bool flag13 = this.btnRetrainGroup != null;
								if (flag13)
								{
									this.btnRetrainGroup.Visibility = ((this.viewModel.SelectedImage.XrsImage.DefaultGroupAssigned && this.accessManager.CurrentUser.Rights >= HistoryControl.rightRetrain) ? Visibility.Visible : Visibility.Collapsed);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06002DEE RID: 11758 RVA: 0x0012BBFA File Offset: 0x00129DFA
		private void Imagebox_ButtonPressed()
		{
			this.ShowFullscreenImage();
		}

		// Token: 0x06002DEF RID: 11759 RVA: 0x0012C0BC File Offset: 0x0012A2BC
		public void SelectedItems(object sender, SelectionChangedEventArgs eventArgs)
		{
			IList selectedItems = ((ListBox)sender).SelectedItems;
			this.viewModel.SelectedImages = selectedItems.Cast<PreviewItem>().ToList<PreviewItem>();
			bool flag = this.viewModel.SelectedImages != null;
			if (flag)
			{
				bool flag2 = this.viewModel.SelectedImages.Count > 0;
				if (flag2)
				{
					this.btnDeleteSelected.IsEnabled = true;
					this.btnDeleteSelected.Content = ImageConverter.ToImage("/img/deletexriimagelist_small.png");
					string text = "";
					this.dbConnection.GetUsbDriveLetter(ref text);
					bool flag3 = text != "\\" && Directory.Exists(text);
					if (flag3)
					{
						this.btnSaveSelectedToUsb.IsActive = true;
						this.btnSaveSelectedToUsb.IsEnabled = true;
					}
					bool flag4 = this.viewModel.SelectedImages.Count == 1;
					if (flag4)
					{
						this.btnRetrainRegion.IsEnabled = true;
						this.btnRetrainRegion.Content = ImageConverter.ToImage("/img/retrainregion_small.png");
					}
					else
					{
						this.btnRetrainRegion.IsEnabled = false;
						this.btnRetrainRegion.Content = ImageConverter.ToImage("/img/retrainregion_disabled.png");
					}
					this.viewModel.SelectedImage = this.viewModel.SelectedImages.Last<PreviewItem>();
					bool flag5 = this.viewModel.SelectedImages.Count > this.viewModel.maximumNumber;
					if (flag5)
					{
						this.btnRetrain.IsEnabled = false;
						bool flag6 = this.btnUndoRetrain != null;
						if (flag6)
						{
							this.btnUndoRetrain.IsEnabled = false;
						}
						this.btnRetrain.Content = ImageConverter.ToImage("/img/retrain_disabled.png");
						this.setRetrainUndoButton(false);
						this.viewModel.RetrainMax = true;
					}
					else
					{
						this.btnRetrain.IsEnabled = true;
						this.btnRetrain.Content = ImageConverter.ToImage("/img/retrain.png");
						bool flag7 = this.undoRetrain;
						if (flag7)
						{
							this.setRetrainUndoButton(true);
						}
						else
						{
							this.setRetrainUndoButton(false);
						}
						this.viewModel.RetrainMax = false;
					}
				}
				else
				{
					this.viewModel.RetrainMax = false;
					this.btnRetrain.IsEnabled = false;
					this.btnDeleteSelected.IsEnabled = false;
					this.btnRetrainRegion.IsEnabled = false;
					bool flag8 = this.btnUndoRetrain != null;
					if (flag8)
					{
						this.btnUndoRetrain.IsEnabled = false;
						this.btnUndoRetrain.Content = ImageConverter.ToImage("/img/retrainUndo_disabled");
					}
					this.btnSaveSelectedToUsb.IsActive = false;
					this.btnSaveSelectedToUsb.IsEnabled = false;
					this.btnRetrain.Content = ImageConverter.ToImage("/img/retrain_disabled.png");
					this.btnDeleteSelected.Content = ImageConverter.ToImage("/img/deletexriimagelist_small_disabled.png");
					this.btnRetrainRegion.Content = ImageConverter.ToImage("/img/retrainregion_disabled.png");
					this.setRetrainUndoButton(false);
				}
			}
			else
			{
				this.viewModel.SelectedImage = null;
			}
			this.viewModel.AllImagesAreMarked = false;
			bool flag9 = this.viewModel.SelectedImages != null;
			if (flag9)
			{
				this.viewModel.Number = this.viewModel.SelectedImages.Count<PreviewItem>().ToString();
			}
			else
			{
				this.viewModel.Number = "0";
			}
			bool flag10 = this.viewModel.SelectedImages.Count > 1;
			if (flag10)
			{
				this.viewModel.SelectedImages[this.viewModel.SelectedImages.Count - 1].LoadXRSImage();
				this.imagebox.ViewXRSImage(this.viewModel.SelectedImages[this.viewModel.SelectedImages.Count - 1].XrsImage);
			}
		}

		// Token: 0x06002DF0 RID: 11760 RVA: 0x0012C498 File Offset: 0x0012A698
		private void ShowFullscreenImage()
		{
			bool flag = this.viewModel.SelectedImage != null && !this.bFullscreenImageShown;
			if (flag)
			{
				this.bFullscreenImageShown = true;
				this.ctrlFullscreenImage = new CtrlFullscreenImage(this.viewModel, this.btnRetrain != null && this.btnRetrain.Visibility == Visibility.Visible, this.btnRetrainGroup != null && this.btnRetrainGroup.Visibility == Visibility.Visible, this.usbPath, this.accessManager, null, null, null);
				this.ctrlFullscreenImage.Unloaded += this.ctrlFullscreenImage_Unloaded;
				Subwindow.Show(this.ctrlFullscreenImage, SubwindowButtons.Back);
			}
		}

		// Token: 0x06002DF1 RID: 11761 RVA: 0x0012C548 File Offset: 0x0012A748
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

		// Token: 0x06002DF2 RID: 11762 RVA: 0x0012C5A8 File Offset: 0x0012A7A8
		private void SetControlRights()
		{
			this.accessManager.RegisterControl(this.btnShowList, HistoryControl.rightSaveToUsb, HistoryControl.rightSaveToUsb);
			this.accessManager.RegisterControl(this.btnSaveSelectedToUsb, HistoryControl.rightSaveToUsb, HistoryControl.rightSaveToUsb);
			this.accessManager.RegisterControl(this.btnMarkAll, HistoryControl.rightSaveToUsb, HistoryControl.rightSaveToUsb);
			this.accessManager.RegisterControl(this.btnDeleteSelected, this.rightDeleteAll, this.rightDeleteAll);
			bool flag = this.btnSaveWithDirectoryname != null;
			if (flag)
			{
				this.accessManager.RegisterControl(this.btnSaveWithDirectoryname, HistoryControl.rightSaveWithDirectoryname, HistoryControl.rightSaveWithDirectoryname);
			}
			bool flag2 = this.btnPrint != null;
			if (flag2)
			{
				this.accessManager.RegisterControl(this.btnPrint, this.rightPrintReport, this.rightPrintReport);
			}
			this.accessManager.SetProperties();
		}

		// Token: 0x06002DF3 RID: 11763 RVA: 0x0012C68C File Offset: 0x0012A88C
		private void SetButtonVisibilities(bool visible)
		{
			bool flag = !visible;
			if (flag)
			{
				this.btnSaveSelectedToUsb.Visibility = Visibility.Collapsed;
				this.btnMarkAll.Visibility = Visibility.Collapsed;
				this.btnShowList.Visibility = Visibility.Collapsed;
				this.btnDeleteSelected.Visibility = Visibility.Collapsed;
				bool flag2 = this.btnPrint != null;
				if (flag2)
				{
					this.btnPrint.Visibility = Visibility.Collapsed;
				}
			}
			else
			{
				this.btnShowList.Visibility = ((this.accessManager.CurrentUser.Rights >= HistoryControl.rightSaveToUsb) ? Visibility.Visible : Visibility.Collapsed);
				this.btnSaveSelectedToUsb.Visibility = ((this.accessManager.CurrentUser.Rights >= HistoryControl.rightSaveToUsb) ? Visibility.Visible : Visibility.Collapsed);
				this.btnDeleteSelected.Visibility = ((this.accessManager.CurrentUser.Rights >= this.rightDeleteAll) ? Visibility.Visible : Visibility.Collapsed);
				this.btnMarkAll.Visibility = ((this.accessManager.CurrentUser.Rights >= HistoryControl.rightSaveToUsb) ? Visibility.Visible : Visibility.Collapsed);
				bool flag3 = this.btnPrint != null;
				if (flag3)
				{
					this.btnPrint.Visibility = ((this.accessManager.CurrentUser.Rights >= this.rightPrintReport) ? Visibility.Visible : Visibility.Collapsed);
				}
			}
		}

		// Token: 0x06002DF4 RID: 11764 RVA: 0x0012C7CC File Offset: 0x0012A9CC
		public void Retrain(object sender, RoutedEventArgs e)
		{
			this.viewModel.first = true;
			this.viewModel.numberItem = 0;
			bool flag = this.viewModel.Retrain();
			if (flag)
			{
				this.setRetrainUndoButton(true);
				this.undoRetrain = true;
				this.viewModel.NotRetrained += this.ViewModel_NotRetrained;
			}
		}

		// Token: 0x06002DF5 RID: 11765 RVA: 0x0012C82C File Offset: 0x0012AA2C
		private void ViewModel_NotRetrained()
		{
			bool flag = Application.Current != null;
			if (flag)
			{
				Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
				{
					this.setRetrainUndoButton(false);
					this.undoRetrain = false;
				}));
			}
			this.viewModel.NotRetrained -= this.ViewModel_NotRetrained;
		}

		// Token: 0x06002DF6 RID: 11766 RVA: 0x0012C87E File Offset: 0x0012AA7E
		public void UndoRetrain(object sender, RoutedEventArgs e)
		{
			this.viewModel.UndoRetrain();
			this.setRetrainUndoButton(false);
			this.undoRetrain = false;
		}

		// Token: 0x06002DF7 RID: 11767 RVA: 0x0012C89C File Offset: 0x0012AA9C
		public void SaveToUSB(object sender, RoutedEventArgs e)
		{
			this.viewModel.SaveToUSB();
		}

		// Token: 0x06002DF8 RID: 11768 RVA: 0x0012C8AB File Offset: 0x0012AAAB
		public void SaveAllToUSB(object sender, RoutedEventArgs e)
		{
			this.viewModel.SaveAllToUSB();
		}

		// Token: 0x06002DF9 RID: 11769 RVA: 0x0012C8BA File Offset: 0x0012AABA
		public void SaveSelectedToUSB(object sender, RoutedEventArgs e)
		{
			HistoryViewModel.cancelSaving = false;
			this.viewModel.SaveSelectedToUSB();
		}

		// Token: 0x06002DFA RID: 11770 RVA: 0x0012C8D0 File Offset: 0x0012AAD0
		public void PrintReport(object sender, RoutedEventArgs e)
		{
			this.viewModel.PrintReport();
		}

		// Token: 0x06002DFB RID: 11771 RVA: 0x0012C8DF File Offset: 0x0012AADF
		public void ShowRetrainRegion(object sender, RoutedEventArgs e)
		{
			this.viewModel.ShowRetrainRegion();
		}

		// Token: 0x06002DFC RID: 11772 RVA: 0x0012C8EE File Offset: 0x0012AAEE
		private void DeleteMarkedImages(object sender, RoutedEventArgs e)
		{
			this.viewModel.DeletMarkedImages();
		}

		// Token: 0x06002DFD RID: 11773 RVA: 0x0012C8FD File Offset: 0x0012AAFD
		public void RetrainGroup(object sender, RoutedEventArgs e)
		{
			this.viewModel.RetrainGroup();
		}

		// Token: 0x06002DFE RID: 11774 RVA: 0x0012C90C File Offset: 0x0012AB0C
		private void setRetrainUndoButton(bool enable)
		{
			Image image = ImageConverter.ToImage("/img/retrainUndo_disabled.png");
			if (enable)
			{
				image = ImageConverter.ToImage("/img/retrainUndo.png");
			}
			image.Height = 40.0;
			image.Width = 40.0;
			image.Stretch = Stretch.Uniform;
			bool flag = this.btnUndoRetrain != null;
			if (flag)
			{
				this.btnUndoRetrain.Content = image;
				this.btnUndoRetrain.IsEnabled = enable;
			}
		}

		// Token: 0x06002DFF RID: 11775 RVA: 0x0001092C File Offset: 0x0000EB2C
		private void imagebox_Loaded(object sender, RoutedEventArgs e)
		{
		}

		// Token: 0x06002E00 RID: 11776 RVA: 0x0012C988 File Offset: 0x0012AB88
		private void getData()
		{
			this.listTree.Items.Clear();
			HistoryControl.Data data = new HistoryControl.Data
			{
				name = this.StrSavedOnUSB.LocalizedText,
				topLevel = true
			};
			string str = "";
			this.dbConnection.GetUsbDriveLetter(ref str);
			bool flag = Directory.Exists(str + "\\");
			if (flag)
			{
				DriveInfo driveInfo = new DriveInfo(str + "\\");
				DirectoryInfo rootDirectory = driveInfo.RootDirectory;
				List<string> list = new List<string>();
				bool flag2 = rootDirectory != null;
				if (flag2)
				{
					DirectoryInfo[] directories = rootDirectory.GetDirectories();
					foreach (DirectoryInfo directoryInfo in directories)
					{
						bool flag3 = directoryInfo.Name.Equals("RayconImages");
						if (flag3)
						{
							DirectoryInfo[] directories2 = directoryInfo.GetDirectories();
							foreach (DirectoryInfo directoryInfo2 in directories2)
							{
								HistoryControl.Data data2 = new HistoryControl.Data
								{
									name = directoryInfo2.Name,
									topLevel = false,
									lowerLevel = true
								};
								list.Add(directoryInfo2.Name);
								FileInfo[] files = directoryInfo2.GetFiles();
								foreach (FileInfo fileInfo in files)
								{
									list.Add(fileInfo.Name);
									data2.Items.Add(new HistoryControl.Data
									{
										name = fileInfo.Name,
										topLevel = false,
										lowerLevel = true
									});
								}
								DirectoryInfo[] directories3 = directoryInfo2.GetDirectories();
								foreach (DirectoryInfo directoryInfo3 in directories3)
								{
									HistoryControl.Data data3 = new HistoryControl.Data
									{
										name = directoryInfo3.Name,
										topLevel = false,
										lowerLevel = true
									};
									data2.Items.Add(data3);
									FileInfo[] files2 = directoryInfo3.GetFiles();
									foreach (FileInfo fileInfo2 in files2)
									{
										data3.Items.Add(new HistoryControl.Data
										{
											name = fileInfo2.Name,
											topLevel = false,
											lowerLevel = true
										});
									}
								}
								data.Items.Add(data2);
							}
						}
					}
				}
			}
			this.listTree.Items.Add(data);
		}

		// Token: 0x06002E01 RID: 11777 RVA: 0x0012CC1B File Offset: 0x0012AE1B
		private void Button_ScrollFirst(object sender, RoutedEventArgs e)
		{
			this.lstImages.ScrollIntoView(this.viewModel.Preview.PreviewItems[0]);
		}

		// Token: 0x06002E02 RID: 11778 RVA: 0x0012CC40 File Offset: 0x0012AE40
		private void Button_ScrollLast(object sender, RoutedEventArgs e)
		{
			this.lstImages.ScrollIntoView(this.viewModel.Preview.PreviewItems[this.viewModel.Preview.PreviewItems.Count - 1]);
		}

		// Token: 0x06002E03 RID: 11779 RVA: 0x0012CC7B File Offset: 0x0012AE7B
		private void saveWithDirectoryname(object sender, RoutedEventArgs e)
		{
			this.viewModel.SaveWithDirectoryname();
		}

		// Token: 0x06002E04 RID: 11780 RVA: 0x0012CC8C File Offset: 0x0012AE8C
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/history/historycontrol.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x06002E05 RID: 11781 RVA: 0x0012CCC4 File Offset: 0x0012AEC4
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				((HistoryControl)target).Unloaded += this.UserControl_Unloaded;
				((HistoryControl)target).Loaded += this.UserControl_Loaded;
				break;
			case 2:
				this.mainGrid = (Grid)target;
				break;
			case 3:
				this.imagebox = (XRSImageBox)target;
				this.imagebox.Loaded += this.imagebox_Loaded;
				break;
			case 4:
				this.subGrid = (Grid)target;
				break;
			case 5:
				this.listTree = (TreeView)target;
				break;
			case 6:
				((Button)target).Click += this.Button_ScrollFirst;
				break;
			case 7:
				this.lstImages = (ListBox)target;
				this.lstImages.SelectionChanged += this.SelectedItems;
				break;
			case 8:
				((Button)target).Click += this.Button_ScrollLast;
				break;
			default:
				this._contentLoaded = true;
				break;
			}
		}

		// Token: 0x04001CA5 RID: 7333
		public static readonly BuiltInPriviledges rightSaveToUsb = BuiltInPriviledges.Operator1;

		// Token: 0x04001CA6 RID: 7334
		private BuiltInPriviledges rightPrintReport = BuiltInPriviledges.Operator1;

		// Token: 0x04001CA7 RID: 7335
		public static readonly BuiltInPriviledges rightRetrain = BuiltInPriviledges.Operator1;

		// Token: 0x04001CA8 RID: 7336
		private BuiltInPriviledges rightDeleteAll = BuiltInPriviledges.Operator1;

		// Token: 0x04001CA9 RID: 7337
		public HistoryViewModel viewModel;

		// Token: 0x04001CAA RID: 7338
		private AccessRightsManager accessManager;

		// Token: 0x04001CAB RID: 7339
		private Button btnRetrain;

		// Token: 0x04001CAC RID: 7340
		private Button btnUndoRetrain;

		// Token: 0x04001CAD RID: 7341
		private ButtonUSB btnSaveSelectedToUsb;

		// Token: 0x04001CAE RID: 7342
		private Button btnPrint;

		// Token: 0x04001CAF RID: 7343
		private Button btnRetrainRegion;

		// Token: 0x04001CB0 RID: 7344
		private Button btnMarkAll;

		// Token: 0x04001CB1 RID: 7345
		private Button btnDeleteSelected;

		// Token: 0x04001CB2 RID: 7346
		private Button btnRetrainGroup;

		// Token: 0x04001CB3 RID: 7347
		private TextBlock txtSelectedProduct;

		// Token: 0x04001CB4 RID: 7348
		private DockPanel dockpanel;

		// Token: 0x04001CB5 RID: 7349
		private CtrlFullscreenImage ctrlFullscreenImage;

		// Token: 0x04001CB6 RID: 7350
		private string usbPath = "";

		// Token: 0x04001CB7 RID: 7351
		private DetectorType detectortype;

		// Token: 0x04001CB8 RID: 7352
		private TestcardSimService testcardSimService;

		// Token: 0x04001CB9 RID: 7353
		private bool bFullscreenImageShown = false;

		// Token: 0x04001CBA RID: 7354
		private SetFooterMainWindow setFooterMainWindow;

		// Token: 0x04001CBB RID: 7355
		private ButtonUSB btnShowList;

		// Token: 0x04001CBC RID: 7356
		private DBConnection dbConnection;

		// Token: 0x04001CBD RID: 7357
		public int maximumNumber = 100;

		// Token: 0x04001CBF RID: 7359
		private bool undoRetrain = false;

		// Token: 0x04001CC0 RID: 7360
		private Button btnSaveWithDirectoryname;

		// Token: 0x04001CC1 RID: 7361
		public static readonly BuiltInPriviledges rightSaveWithDirectoryname = BuiltInPriviledges.Service;

		// Token: 0x04001CC2 RID: 7362
		internal Grid mainGrid;

		// Token: 0x04001CC3 RID: 7363
		internal XRSImageBox imagebox;

		// Token: 0x04001CC4 RID: 7364
		internal Grid subGrid;

		// Token: 0x04001CC5 RID: 7365
		internal TreeView listTree;

		// Token: 0x04001CC6 RID: 7366
		internal ListBox lstImages;

		// Token: 0x04001CC7 RID: 7367
		private bool _contentLoaded;

		// Token: 0x02000393 RID: 915
		public class Data
		{
			// Token: 0x060037C2 RID: 14274 RVA: 0x00157ABF File Offset: 0x00155CBF
			public Data()
			{
				this.Items = new ObservableCollection<HistoryControl.Data>();
			}

			// Token: 0x17001125 RID: 4389
			// (get) Token: 0x060037C3 RID: 14275 RVA: 0x00157AD5 File Offset: 0x00155CD5
			// (set) Token: 0x060037C4 RID: 14276 RVA: 0x00157ADD File Offset: 0x00155CDD
			public string name { get; set; }

			// Token: 0x17001126 RID: 4390
			// (get) Token: 0x060037C5 RID: 14277 RVA: 0x00157AE6 File Offset: 0x00155CE6
			// (set) Token: 0x060037C6 RID: 14278 RVA: 0x00157AEE File Offset: 0x00155CEE
			public ObservableCollection<HistoryControl.Data> Items { get; set; }

			// Token: 0x17001127 RID: 4391
			// (get) Token: 0x060037C7 RID: 14279 RVA: 0x00157AF7 File Offset: 0x00155CF7
			// (set) Token: 0x060037C8 RID: 14280 RVA: 0x00157AFF File Offset: 0x00155CFF
			public bool topLevel { get; set; }

			// Token: 0x17001128 RID: 4392
			// (get) Token: 0x060037C9 RID: 14281 RVA: 0x00157B08 File Offset: 0x00155D08
			// (set) Token: 0x060037CA RID: 14282 RVA: 0x00157B10 File Offset: 0x00155D10
			public bool lowerLevel { get; set; }
		}
	}
}
