using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;
using DatabaseConnection;
using Definitions;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x0200024B RID: 587
	public class ProductChangeControl : UserControl, IComponentConnector, IStyleConnector
	{
		// Token: 0x17000C02 RID: 3074
		// (get) Token: 0x06002C7E RID: 11390 RVA: 0x00124AE2 File Offset: 0x00122CE2
		// (set) Token: 0x06002C7F RID: 11391 RVA: 0x00124AEA File Offset: 0x00122CEA
		public List<Button> Buttons { get; set; }

		// Token: 0x06002C80 RID: 11392 RVA: 0x00124AF4 File Offset: 0x00122CF4
		public ProductChangeControl(DBConnection dbConnection, ParameterHandling parHandling, ImageDataTransfer imageTransfer, ProductManager productManager, AccessRightsManager accessManager, bool multiProductsEnabled, bool multiLaneProductsEnabled, bool multiRegionEnabled, bool bulkObjectEnabled, StoreImageData imageArchiver, SetFooterMainWindow setFooterMainWindow)
		{
			this.accessManager = accessManager;
			this.productManager = productManager;
			this.dbConnection = dbConnection;
			this.parHandling = parHandling;
			this.imageTransfer = imageTransfer;
			this.imageArchiver = imageArchiver;
			this.setFooterMainWindow = setFooterMainWindow;
			this.InitializeComponent();
			this.InitializeButtons(multiProductsEnabled, multiLaneProductsEnabled, multiRegionEnabled, bulkObjectEnabled);
			this.SetControlRights();
			bool flag = productManager != null;
			if (flag)
			{
				productManager.ProductChanged += this.productManager_ProductChanged;
				productManager.PropertyChanged += this.productManager_PropertyChanged;
			}
			this.productChangeViewModel = new ProductChangeViewModel(productManager, dbConnection);
			this.SetSubProductButtonsVisibility();
			setFooterMainWindow.SetFooterDeactivation();
			this.lstProducts.Loaded += delegate(object s, RoutedEventArgs e)
			{
				base.Dispatcher.Invoke(delegate()
				{
					this.lstProducts.ScrollIntoView(this.lstProducts.SelectedItem);
				});
			};
		}

		// Token: 0x06002C81 RID: 11393 RVA: 0x00031641 File Offset: 0x0002F841
		private void productManager_ProductChanged(ProductData oldProduct, ProductData newProduct)
		{
			(Window.GetWindow(this) as Subwindow).Close();
		}

		// Token: 0x06002C82 RID: 11394 RVA: 0x00124BFC File Offset: 0x00122DFC
		private void SetControlRights()
		{
			this.accessManager.RegisterControl(this.btnChange, this.rightProductChange, this.rightProductChange);
			this.accessManager.RegisterControl(this.btnNew, this.rightNew, this.rightNew);
			this.accessManager.RegisterControl(this.btnCopy, this.rightCopy, this.rightCopy);
			this.accessManager.RegisterControl(this.btnDelete, this.rightDelete, this.rightDelete);
			this.accessManager.RegisterControl(this.btnRename, this.rightRename, this.rightRename);
			bool flag = this.btnNewMulti != null;
			if (flag)
			{
				this.accessManager.RegisterControl(this.btnNewMulti, this.rightNew, this.rightNew);
			}
			bool flag2 = this.btnCreateLaneProduct != null;
			if (flag2)
			{
				this.accessManager.RegisterControl(this.btnCreateLaneProduct, this.rightNew, this.rightNew);
			}
			bool flag3 = this.btnCreateRegionProduct != null;
			if (flag3)
			{
				this.accessManager.RegisterControl(this.btnCreateRegionProduct, this.rightNew, this.rightNew);
			}
			bool flag4 = this.btnBatchSettings != null;
			if (flag4)
			{
				this.accessManager.RegisterControl(this.btnBatchSettings, this.rightNew, this.rightNew);
			}
			bool flag5 = this.btnCreateBulkObjectProduct != null;
			if (flag5)
			{
				this.accessManager.RegisterControl(this.btnCreateBulkObjectProduct, this.rightNew, this.rightNew);
			}
			this.accessManager.SetProperties();
		}

		// Token: 0x06002C83 RID: 11395 RVA: 0x00124D88 File Offset: 0x00122F88
		private void InitializeButtons(bool multiProductsEnabled, bool multiLaneProductsEnabled, bool multiRegionEnabled, bool bulkObjectEnabled)
		{
			this.Buttons = new List<Button>();
			bool flag = false;
			MachineMode machineMode = MachineMode.Standard;
			this.dbConnection.GetMachineMode(ref machineMode);
			this.dbConnection.GetBatchesEnabled(ref flag);
			this.btnChange = new Button();
			this.btnChange.Content = ImageConverter.ToImage("/img/changeproduct_small.png");
			this.btnChange.Click += this.btnChange_Click;
			this.btnChange.Style = (base.FindResource("defaultButtonStyle") as Style);
			this.Buttons.Add(this.btnChange);
			this.btnNew = new Button();
			this.btnNew.Content = ImageConverter.ToImage("/img/addproduct.png");
			this.btnNew.Click += this.btnNew_Click;
			this.btnNew.Style = (base.FindResource("defaultButtonStyle") as Style);
			this.Buttons.Add(this.btnNew);
			if (multiProductsEnabled)
			{
				this.btnNewMulti = new Button();
				this.btnNewMulti.Content = ImageConverter.ToImage("/img/addmultiproduct.png");
				this.btnNewMulti.Click += this.btnNewMulti_Click;
				this.btnNewMulti.Style = (base.FindResource("defaultButtonStyle") as Style);
				this.Buttons.Add(this.btnNewMulti);
			}
			if (multiLaneProductsEnabled)
			{
				this.btnCreateLaneProduct = new Button();
				this.btnCreateLaneProduct.Content = ImageConverter.ToImage("/img/addMultiLaneProduct.png");
				this.btnCreateLaneProduct.Click += this.btnCreateLaneProduct_Click;
				this.btnCreateLaneProduct.Style = (base.FindResource("defaultButtonStyle") as Style);
				this.Buttons.Add(this.btnCreateLaneProduct);
			}
			if (multiRegionEnabled)
			{
				this.btnCreateRegionProduct = new Button();
				Image image = ImageConverter.ToImage("/img/addMultiRegionProduct.png");
				image.Stretch = Stretch.Uniform;
				image.Width = 40.0;
				image.Height = 40.0;
				this.btnCreateRegionProduct.Content = image;
				this.btnCreateRegionProduct.Click += this.btnCreateRegionProduct_Click;
				this.btnCreateRegionProduct.Style = (base.FindResource("defaultButtonStyle") as Style);
				this.Buttons.Add(this.btnCreateRegionProduct);
			}
			bool flag2 = bulkObjectEnabled && (machineMode == MachineMode.Bulk || machineMode == MachineMode.BulkObject);
			if (flag2)
			{
				this.btnCreateBulkObjectProduct = new Button();
				Image image2 = ImageConverter.ToImage("/img/addBulkObjectProducts.png");
				image2.Stretch = Stretch.Uniform;
				image2.Width = 40.0;
				image2.Height = 40.0;
				this.btnCreateBulkObjectProduct.Content = image2;
				this.btnCreateBulkObjectProduct.Click += this.btnCreateBulkObjectProduct_Click;
				this.btnCreateBulkObjectProduct.Style = (base.FindResource("defaultButtonStyle") as Style);
				this.Buttons.Add(this.btnCreateBulkObjectProduct);
			}
			this.btnCopy = new Button();
			this.btnCopy.Content = ImageConverter.ToImage("/img/copyproduct.png");
			this.btnCopy.Click += this.btnCopy_Click;
			this.btnCopy.Style = (base.FindResource("defaultButtonStyle") as Style);
			this.Buttons.Add(this.btnCopy);
			this.btnDelete = new Button();
			this.btnDelete.Content = ImageConverter.ToImage("/img/deleteproduct.png");
			this.btnDelete.Click += this.btnDelete_Click;
			this.btnDelete.Style = (base.FindResource("defaultButtonStyle") as Style);
			this.Buttons.Add(this.btnDelete);
			this.btnRename = new Button();
			this.btnRename.Content = ImageConverter.ToImage("/img/editproduct.png");
			this.btnRename.Click += this.btnRename_Click;
			this.btnRename.Style = (base.FindResource("defaultButtonStyle") as Style);
			this.Buttons.Add(this.btnRename);
			this.btnMarkAll = new Button();
			Image image3 = ImageConverter.ToImage("/img/mark_all.png");
			image3.Height = 40.0;
			image3.Width = 40.0;
			image3.Stretch = Stretch.Uniform;
			this.btnMarkAll.Content = image3;
			this.btnMarkAll.Click += this.MarkAllImages;
			this.btnMarkAll.Style = (base.FindResource("defaultButtonStyle") as Style);
			this.btnMarkAll.Margin = new Thickness(0.0, 0.0, 10.0, 0.0);
			this.Buttons.Add(this.btnMarkAll);
			this.btnShowSubProducts = new Button();
			this.btnShowSubProducts.Content = ImageConverter.ToImage("/img/subProductSettings.png");
			this.btnShowSubProducts.Click += this.btnShowSubProducts_Click;
			this.btnShowSubProducts.Style = (base.FindResource("defaultButtonStyle") as Style);
			this.Buttons.Add(this.btnShowSubProducts);
			this.btnProductsToLane = new Button();
			this.btnProductsToLane.Content = ImageConverter.ToImage("/img/multiLaneProductSettings.png");
			this.btnProductsToLane.Click += this.btnProductsToLane_Click;
			this.btnProductsToLane.Style = (base.FindResource("defaultButtonStyle") as Style);
			this.Buttons.Add(this.btnProductsToLane);
			this.btnRegionSubProducts = new Button();
			Image image4 = ImageConverter.ToImage("/img/multiRegionProductSettings.png");
			image4.Stretch = Stretch.Uniform;
			image4.Width = 40.0;
			image4.Height = 40.0;
			this.btnRegionSubProducts.Content = image4;
			this.btnRegionSubProducts.Click += this.btnRegionSubProducts_Click;
			this.btnRegionSubProducts.Style = (base.FindResource("defaultButtonStyle") as Style);
			this.Buttons.Add(this.btnRegionSubProducts);
			bool flag3 = flag;
			if (flag3)
			{
				this.btnBatchSettings = new Button();
				this.btnBatchSettings.Content = ImageConverter.ToImage("/img/batchSettings.png");
				this.btnBatchSettings.Click += this.btnBatchSettings_Click;
				this.btnBatchSettings.Style = (base.FindResource("defaultButtonStyle") as Style);
				this.Buttons.Add(this.btnBatchSettings);
			}
		}

		// Token: 0x06002C84 RID: 11396 RVA: 0x0012548F File Offset: 0x0012368F
		private void btnDelete_Click(object sender, RoutedEventArgs e)
		{
			this.productChangeViewModel.DeleteProduct();
		}

		// Token: 0x06002C85 RID: 11397 RVA: 0x0012549E File Offset: 0x0012369E
		private void btnCopy_Click(object sender, RoutedEventArgs e)
		{
			this.productChangeViewModel.CopyProduct();
		}

		// Token: 0x06002C86 RID: 11398 RVA: 0x001254AD File Offset: 0x001236AD
		private void btnNew_Click(object sender, RoutedEventArgs e)
		{
			this.productChangeViewModel.CreateNewProduct();
		}

		// Token: 0x06002C87 RID: 11399 RVA: 0x001254BC File Offset: 0x001236BC
		private void btnNewMulti_Click(object sender, RoutedEventArgs e)
		{
			this.productChangeViewModel.CreateNewMultiProduct();
		}

		// Token: 0x06002C88 RID: 11400 RVA: 0x001254CC File Offset: 0x001236CC
		private void btnChange_Click(object sender, RoutedEventArgs e)
		{
			bool flag = this.productChangeViewModel.ChangeProduct();
			if (flag)
			{
				this.ShowBatchesWindowOrDialog();
			}
		}

		// Token: 0x06002C89 RID: 11401 RVA: 0x001254F2 File Offset: 0x001236F2
		private void btnRename_Click(object sender, RoutedEventArgs e)
		{
			this.productChangeViewModel.RenameProduct();
		}

		// Token: 0x06002C8A RID: 11402 RVA: 0x00125504 File Offset: 0x00123704
		private void listBoxItem_DoubleClick(object sender, MouseEventArgs e)
		{
			bool flag = sender is ListBoxItem && e.OriginalSource.GetType() != typeof(Image) && e.OriginalSource.GetType() != typeof(Ellipse);
			if (flag)
			{
				ProductData selectedProduct = (ProductData)(sender as ListBoxItem).DataContext;
				this.productManager.SelectedProduct = selectedProduct;
				bool flag2 = this.productChangeViewModel.ChangeProduct();
				if (flag2)
				{
					this.ShowBatchesWindowOrDialog();
				}
			}
		}

		// Token: 0x06002C8B RID: 11403 RVA: 0x00125590 File Offset: 0x00123790
		private void ProductChange_Unloaded(object sender, RoutedEventArgs e)
		{
			this.accessManager.DeregisterControl(this.btnChange);
			this.accessManager.DeregisterControl(this.btnNew);
			this.accessManager.DeregisterControl(this.btnCopy);
			this.accessManager.DeregisterControl(this.btnDelete);
			this.accessManager.DeregisterControl(this.btnRename);
			this.accessManager.DeregisterControl(this.btnMarkAll);
			bool flag = this.btnNewMulti != null;
			if (flag)
			{
				this.accessManager.DeregisterControl(this.btnNewMulti);
			}
			bool flag2 = this.btnCreateLaneProduct != null;
			if (flag2)
			{
				this.accessManager.DeregisterControl(this.btnCreateLaneProduct);
			}
			bool flag3 = this.btnCreateRegionProduct != null;
			if (flag3)
			{
				this.accessManager.DeregisterControl(this.btnCreateRegionProduct);
			}
			bool flag4 = this.btnBatchSettings != null;
			if (flag4)
			{
				this.accessManager.DeregisterControl(this.btnBatchSettings);
			}
			bool flag5 = this.btnCreateBulkObjectProduct != null;
			if (flag5)
			{
				this.accessManager.DeregisterControl(this.btnCreateBulkObjectProduct);
			}
			this.productManager.ProductChanged -= this.productManager_ProductChanged;
			this.productManager.PropertyChanged -= this.productManager_PropertyChanged;
			this.productChangeViewModel.ResetProductSearch();
			MachineMode machineMode = MachineMode.Standard;
			this.dbConnection.GetMachineMode(ref machineMode);
			this.setFooterMainWindow.UnSetFooterDeactivation();
		}

		// Token: 0x06002C8C RID: 11404 RVA: 0x00125704 File Offset: 0x00123904
		private void productManager_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			bool flag = e.PropertyName == "SelectedProduct";
			if (flag)
			{
				this.SetSubProductButtonsVisibility();
			}
		}

		// Token: 0x06002C8D RID: 11405 RVA: 0x00125730 File Offset: 0x00123930
		private void SetSubProductButtonsVisibility()
		{
			bool flag = this.productManager.SelectedProduct != null;
			if (flag)
			{
				bool isMultiProduct = this.productManager.SelectedProduct.IsMultiProduct;
				if (isMultiProduct)
				{
					this.btnShowSubProducts.Visibility = ((this.accessManager.CurrentUser.Rights >= BuiltInPriviledges.Operator1) ? Visibility.Visible : Visibility.Collapsed);
					this.btnProductsToLane.Visibility = Visibility.Collapsed;
					this.btnRegionSubProducts.Visibility = Visibility.Collapsed;
				}
				else
				{
					bool isLaneProduct = this.productManager.SelectedProduct.IsLaneProduct;
					if (isLaneProduct)
					{
						this.btnShowSubProducts.Visibility = Visibility.Collapsed;
						this.btnProductsToLane.Visibility = ((this.accessManager.CurrentUser.Rights >= BuiltInPriviledges.Operator1) ? Visibility.Visible : Visibility.Collapsed);
						this.btnRegionSubProducts.Visibility = Visibility.Collapsed;
					}
					else
					{
						bool isRegionProduct = this.productManager.SelectedProduct.IsRegionProduct;
						if (isRegionProduct)
						{
							this.btnShowSubProducts.Visibility = Visibility.Collapsed;
							this.btnProductsToLane.Visibility = Visibility.Collapsed;
							this.btnRegionSubProducts.Visibility = ((this.accessManager.CurrentUser.Rights >= BuiltInPriviledges.Operator1) ? Visibility.Visible : Visibility.Collapsed);
						}
						else
						{
							this.btnShowSubProducts.Visibility = Visibility.Collapsed;
							this.btnProductsToLane.Visibility = Visibility.Collapsed;
							this.btnRegionSubProducts.Visibility = Visibility.Collapsed;
						}
					}
				}
			}
			else
			{
				this.btnShowSubProducts.Visibility = Visibility.Collapsed;
				this.btnProductsToLane.Visibility = Visibility.Collapsed;
				this.btnRegionSubProducts.Visibility = Visibility.Collapsed;
			}
		}

		// Token: 0x06002C8E RID: 11406 RVA: 0x001258BC File Offset: 0x00123ABC
		private void btnShowSubProducts_Click(object sender, RoutedEventArgs e)
		{
			CtrlSubProduct ctrlSubProduct = new CtrlSubProduct(this.productManager, SubProductModes.AddOrRemove);
			Subwindow.Show(ctrlSubProduct, SubwindowButtons.BackAndHome, ctrlSubProduct.Buttons);
		}

		// Token: 0x06002C8F RID: 11407 RVA: 0x001258E5 File Offset: 0x00123AE5
		private void btnCreateLaneProduct_Click(object sender, RoutedEventArgs e)
		{
			this.productChangeViewModel.CreateNewLaneProduct();
		}

		// Token: 0x06002C90 RID: 11408 RVA: 0x001258F4 File Offset: 0x00123AF4
		private void btnProductsToLane_Click(object sender, RoutedEventArgs e)
		{
			CtrlProductsToLane usrControl = new CtrlProductsToLane(this.productManager, this.dbConnection, this.parHandling, this.imageTransfer);
			Subwindow.Show(usrControl, SubwindowButtons.BackAndHome);
		}

		// Token: 0x06002C91 RID: 11409 RVA: 0x00125928 File Offset: 0x00123B28
		private void btnCreateRegionProduct_Click(object sender, RoutedEventArgs e)
		{
			this.productChangeViewModel.CreateNewRegionProduct();
		}

		// Token: 0x06002C92 RID: 11410 RVA: 0x00125937 File Offset: 0x00123B37
		private void btnCreateBulkObjectProduct_Click(object sender, RoutedEventArgs e)
		{
			this.productChangeViewModel.CreateNewBulkObjectProduct();
		}

		// Token: 0x06002C93 RID: 11411 RVA: 0x00125948 File Offset: 0x00123B48
		private void btnRegionSubProducts_Click(object sender, RoutedEventArgs e)
		{
			CtrlRegionProduct ctrlRegionProduct = new CtrlRegionProduct(this.parHandling, this.imageTransfer, this.dbConnection, this.productManager, this.imageArchiver);
			Subwindow.Show(ctrlRegionProduct, SubwindowButtons.BackAndHome, ctrlRegionProduct.Buttons);
		}

		// Token: 0x06002C94 RID: 11412 RVA: 0x00125988 File Offset: 0x00123B88
		private void ShowBatchesWindowOrDialog()
		{
			Dialog.Show(LocalizationManager.Localize("txtWarning"), string.Concat(new string[]
			{
				LocalizationManager.Localize("txtApplication"),
				" \"",
				this.productChangeViewModel.ProductManager.SelectedProduct.ProductName,
				"\" ",
				LocalizationManager.Localize("msgAlreadyLoaded")
			}), DialogStyle.Warning, DialogButtons.OK);
		}

		// Token: 0x06002C95 RID: 11413 RVA: 0x001259F5 File Offset: 0x00123BF5
		private void btnBatchSettings_Click(object sender, RoutedEventArgs e)
		{
			this.productManager.ShowBatchWindow(false);
		}

		// Token: 0x06002C96 RID: 11414 RVA: 0x00125A05 File Offset: 0x00123C05
		private void ProductChange_Loaded(object sender, RoutedEventArgs e)
		{
			base.DataContext = this.productChangeViewModel;
		}

		// Token: 0x06002C97 RID: 11415 RVA: 0x00125A18 File Offset: 0x00123C18
		public void SelectedItems(object sender, SelectionChangedEventArgs eventArgs)
		{
			IList selectedItems = ((ListBox)sender).SelectedItems;
			this.productManager.SelectedProducts = selectedItems.Cast<ProductData>().ToList<ProductData>();
			bool flag = this.productManager.SelectedProducts != null;
			if (flag)
			{
				bool flag2 = this.productManager.SelectedProducts.Count > 0;
				if (flag2)
				{
					this.productManager.SelectedProduct = this.productManager.SelectedProducts.Last<ProductData>();
				}
			}
			else
			{
				this.productManager.SelectedProduct = null;
			}
			bool flag3 = this.productManager.SelectedProducts.Count == 1;
			if (flag3)
			{
				this.enableButtons(true);
			}
			else
			{
				this.enableButtons(false);
			}
		}

		// Token: 0x06002C98 RID: 11416 RVA: 0x00125AD0 File Offset: 0x00123CD0
		public void enableButtons(bool enabled)
		{
			this.btnChange.IsEnabled = enabled;
			this.btnCopy.IsEnabled = enabled;
			this.btnRename.IsEnabled = enabled;
			if (enabled)
			{
				this.btnChange.Content = ImageConverter.ToImage("/img/changeproduct_small.png");
				this.btnCopy.Content = ImageConverter.ToImage("/img/copyproduct.png");
				this.btnRename.Content = ImageConverter.ToImage("/img/editproduct.png");
			}
			else
			{
				this.btnChange.Content = ImageConverter.ToImage("/img/changeproduct_small_disabled.png");
				this.btnCopy.Content = ImageConverter.ToImage("/img/copyproduct_disabled.png");
				this.btnRename.Content = ImageConverter.ToImage("/img/editproduct_disabled.png");
			}
		}

		// Token: 0x06002C99 RID: 11417 RVA: 0x00125B94 File Offset: 0x00123D94
		private void MarkAllImages(object sender, RoutedEventArgs e)
		{
			bool flag = this.productManager.SelectedProducts.Count == this.productManager.ProductList.Count;
			if (flag)
			{
				this.lstProducts.SelectedItems.Clear();
			}
			else
			{
				this.lstProducts.SelectAll();
			}
		}

		// Token: 0x06002C9A RID: 11418 RVA: 0x00125BEC File Offset: 0x00123DEC
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/productchange/productchangecontrol.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x06002C9B RID: 11419 RVA: 0x00125C24 File Offset: 0x00123E24
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				((ProductChangeControl)target).Loaded += this.ProductChange_Loaded;
				((ProductChangeControl)target).Unloaded += this.ProductChange_Unloaded;
				break;
			case 2:
				this.tbCurrentAppl = (TextBlock)target;
				break;
			case 3:
				this.lstProducts = (ListBox)target;
				this.lstProducts.SelectionChanged += this.SelectedItems;
				break;
			default:
				this._contentLoaded = true;
				break;
			}
		}

		// Token: 0x06002C9C RID: 11420 RVA: 0x00125CBC File Offset: 0x00123EBC
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IStyleConnector.Connect(int connectionId, object target)
		{
			if (connectionId == 4)
			{
				EventSetter eventSetter = new EventSetter();
				eventSetter.Event = Control.MouseDoubleClickEvent;
				eventSetter.Handler = new MouseButtonEventHandler(this.listBoxItem_DoubleClick);
				((Style)target).Setters.Add(eventSetter);
			}
		}

		// Token: 0x04001BD6 RID: 7126
		private BuiltInPriviledges rightProductChange = BuiltInPriviledges.Operator2;

		// Token: 0x04001BD7 RID: 7127
		private BuiltInPriviledges rightNew = BuiltInPriviledges.Operator1;

		// Token: 0x04001BD8 RID: 7128
		private BuiltInPriviledges rightCopy = BuiltInPriviledges.Operator1;

		// Token: 0x04001BD9 RID: 7129
		private BuiltInPriviledges rightDelete = BuiltInPriviledges.Operator1;

		// Token: 0x04001BDA RID: 7130
		private BuiltInPriviledges rightRename = BuiltInPriviledges.Operator1;

		// Token: 0x04001BDB RID: 7131
		private Button btnRename;

		// Token: 0x04001BDC RID: 7132
		private Button btnChange;

		// Token: 0x04001BDD RID: 7133
		private Button btnNew;

		// Token: 0x04001BDE RID: 7134
		private Button btnNewMulti;

		// Token: 0x04001BDF RID: 7135
		private Button btnDelete;

		// Token: 0x04001BE0 RID: 7136
		private Button btnCopy;

		// Token: 0x04001BE1 RID: 7137
		private Button btnMarkAll;

		// Token: 0x04001BE2 RID: 7138
		private Button btnShowSubProducts;

		// Token: 0x04001BE3 RID: 7139
		private Button btnProductsToLane;

		// Token: 0x04001BE4 RID: 7140
		private Button btnRegionSubProducts;

		// Token: 0x04001BE5 RID: 7141
		private Button btnBatchSettings;

		// Token: 0x04001BE6 RID: 7142
		private AccessRightsManager accessManager;

		// Token: 0x04001BE7 RID: 7143
		private ProductChangeViewModel productChangeViewModel;

		// Token: 0x04001BE8 RID: 7144
		private ProductManager productManager;

		// Token: 0x04001BE9 RID: 7145
		private DBConnection dbConnection;

		// Token: 0x04001BEA RID: 7146
		private ParameterHandling parHandling;

		// Token: 0x04001BEB RID: 7147
		private ImageDataTransfer imageTransfer;

		// Token: 0x04001BEC RID: 7148
		private StoreImageData imageArchiver;

		// Token: 0x04001BED RID: 7149
		private Button btnCreateLaneProduct;

		// Token: 0x04001BEE RID: 7150
		private Button btnCreateRegionProduct;

		// Token: 0x04001BEF RID: 7151
		private SetFooterMainWindow setFooterMainWindow;

		// Token: 0x04001BF0 RID: 7152
		private Button btnCreateBulkObjectProduct;

		// Token: 0x04001BF2 RID: 7154
		internal TextBlock tbCurrentAppl;

		// Token: 0x04001BF3 RID: 7155
		internal ListBox lstProducts;

		// Token: 0x04001BF4 RID: 7156
		private bool _contentLoaded;
	}
}
