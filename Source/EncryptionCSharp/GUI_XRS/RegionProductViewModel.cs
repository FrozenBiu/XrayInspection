using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Data;
using System.Windows.Threading;
using Common;
using DatabaseConnection;
using Definitions;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x0200012F RID: 303
	public class RegionProductViewModel : INotifyPropertyChanged
	{
		// Token: 0x17000883 RID: 2179
		// (get) Token: 0x06001D1C RID: 7452 RVA: 0x000BC1D9 File Offset: 0x000BA3D9
		// (set) Token: 0x06001D1D RID: 7453 RVA: 0x000BC1E1 File Offset: 0x000BA3E1
		public ProductManager ProductManager { get; set; }

		// Token: 0x17000884 RID: 2180
		// (get) Token: 0x06001D1E RID: 7454 RVA: 0x000BC1EC File Offset: 0x000BA3EC
		// (set) Token: 0x06001D1F RID: 7455 RVA: 0x000BC204 File Offset: 0x000BA404
		public XRSImage CurrentImage
		{
			get
			{
				return this.currentImage;
			}
			set
			{
				this.currentImage = value;
				this.OnPropertyChanged("CurrentImage");
			}
		}

		// Token: 0x17000885 RID: 2181
		// (get) Token: 0x06001D20 RID: 7456 RVA: 0x000BC21C File Offset: 0x000BA41C
		// (set) Token: 0x06001D21 RID: 7457 RVA: 0x000BC234 File Offset: 0x000BA434
		public bool RegionNotEditable
		{
			get
			{
				return this.regionNotEditable;
			}
			set
			{
				this.regionNotEditable = value;
				this.OnPropertyChanged("RegionNotEditable");
				this.OnPropertyChanged("RegionEditable");
				if (value)
				{
					this.imageBox.RemoveCrosses();
					this.BackCommandVisibility = Visibility.Collapsed;
				}
			}
		}

		// Token: 0x17000886 RID: 2182
		// (get) Token: 0x06001D22 RID: 7458 RVA: 0x000BC27C File Offset: 0x000BA47C
		public bool RegionEditable
		{
			get
			{
				return !this.regionNotEditable;
			}
		}

		// Token: 0x17000887 RID: 2183
		// (get) Token: 0x06001D23 RID: 7459 RVA: 0x000BC298 File Offset: 0x000BA498
		// (set) Token: 0x06001D24 RID: 7460 RVA: 0x000BC2B0 File Offset: 0x000BA4B0
		public Visibility BackCommandVisibility
		{
			get
			{
				return this.backCommandVisible;
			}
			set
			{
				this.backCommandVisible = value;
				this.OnPropertyChanged("BackCommandVisibility");
			}
		}

		// Token: 0x17000888 RID: 2184
		// (get) Token: 0x06001D25 RID: 7461 RVA: 0x000BC2C6 File Offset: 0x000BA4C6
		// (set) Token: 0x06001D26 RID: 7462 RVA: 0x000BC2CE File Offset: 0x000BA4CE
		public ObservableCollection<ProductData> RegionProduct { get; set; }

		// Token: 0x14000096 RID: 150
		// (add) Token: 0x06001D27 RID: 7463 RVA: 0x000BC2D8 File Offset: 0x000BA4D8
		// (remove) Token: 0x06001D28 RID: 7464 RVA: 0x000BC310 File Offset: 0x000BA510
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x17000889 RID: 2185
		// (get) Token: 0x06001D29 RID: 7465 RVA: 0x000BC345 File Offset: 0x000BA545
		// (set) Token: 0x06001D2A RID: 7466 RVA: 0x000BC34D File Offset: 0x000BA54D
		public RelayCommand AddRegionCommand { get; set; }

		// Token: 0x1700088A RID: 2186
		// (get) Token: 0x06001D2B RID: 7467 RVA: 0x000BC356 File Offset: 0x000BA556
		// (set) Token: 0x06001D2C RID: 7468 RVA: 0x000BC35E File Offset: 0x000BA55E
		public RelayCommand RemoveRegionCommand { get; set; }

		// Token: 0x1700088B RID: 2187
		// (get) Token: 0x06001D2D RID: 7469 RVA: 0x000BC367 File Offset: 0x000BA567
		// (set) Token: 0x06001D2E RID: 7470 RVA: 0x000BC36F File Offset: 0x000BA56F
		public RelayCommand RenameRegionCommand { get; set; }

		// Token: 0x1700088C RID: 2188
		// (get) Token: 0x06001D2F RID: 7471 RVA: 0x000BC378 File Offset: 0x000BA578
		// (set) Token: 0x06001D30 RID: 7472 RVA: 0x000BC380 File Offset: 0x000BA580
		public RelayCommand InitializeCommand { get; set; }

		// Token: 0x1700088D RID: 2189
		// (get) Token: 0x06001D31 RID: 7473 RVA: 0x000BC389 File Offset: 0x000BA589
		// (set) Token: 0x06001D32 RID: 7474 RVA: 0x000BC391 File Offset: 0x000BA591
		public RelayCommand CleanUpCommand { get; set; }

		// Token: 0x1700088E RID: 2190
		// (get) Token: 0x06001D33 RID: 7475 RVA: 0x000BC39A File Offset: 0x000BA59A
		// (set) Token: 0x06001D34 RID: 7476 RVA: 0x000BC3A2 File Offset: 0x000BA5A2
		public RelayCommand MakeRegionEditableCommand { get; set; }

		// Token: 0x1700088F RID: 2191
		// (get) Token: 0x06001D35 RID: 7477 RVA: 0x000BC3AB File Offset: 0x000BA5AB
		// (set) Token: 0x06001D36 RID: 7478 RVA: 0x000BC3B3 File Offset: 0x000BA5B3
		public RelayCommand CommitRegionSelectCommand { get; set; }

		// Token: 0x17000890 RID: 2192
		// (get) Token: 0x06001D37 RID: 7479 RVA: 0x000BC3BC File Offset: 0x000BA5BC
		// (set) Token: 0x06001D38 RID: 7480 RVA: 0x000BC3C4 File Offset: 0x000BA5C4
		public RelayCommand AbortRegionSelectCommand { get; set; }

		// Token: 0x17000891 RID: 2193
		// (get) Token: 0x06001D39 RID: 7481 RVA: 0x000BC3CD File Offset: 0x000BA5CD
		// (set) Token: 0x06001D3A RID: 7482 RVA: 0x000BC3D5 File Offset: 0x000BA5D5
		public RelayCommand RemoveLastCrossCommand { get; set; }

		// Token: 0x06001D3B RID: 7483 RVA: 0x000BC3E0 File Offset: 0x000BA5E0
		public RegionProductViewModel(ParameterHandling parHandling, ImageDataTransfer imageTransfer, DBConnection dbConnection, ProductManager productManager, XRSImageBox imageBox, StoreImageData imageArchiver)
		{
			this.parHandling = parHandling;
			this.imageTransfer = imageTransfer;
			this.dbConnection = dbConnection;
			this.ProductManager = productManager;
			this.imageBox = imageBox;
			this.imageArchiver = imageArchiver;
			this.regionNotEditable = true;
			this.AddRegionCommand = new RelayCommand(new Action<object>(this.AddRegion));
			this.RemoveRegionCommand = new RelayCommand(new Action<object>(this.RemoveRegion));
			this.RenameRegionCommand = new RelayCommand(new Action<object>(this.EditRegion));
			this.InitializeCommand = new RelayCommand(new Action<object>(this.Initialize));
			this.CleanUpCommand = new RelayCommand(new Action<object>(this.CleanUp));
			this.MakeRegionEditableCommand = new RelayCommand(new Action<object>(this.MakeRegionEditable));
			this.CommitRegionSelectCommand = new RelayCommand(new Action<object>(this.CommitRegionSelect));
			this.AbortRegionSelectCommand = new RelayCommand(new Action<object>(this.AbortRegionSelect));
			this.RemoveLastCrossCommand = new RelayCommand(new Action<object>(this.RemoveLastCross));
			imageBox.CrossDrawn += this.imagebox_CrossDrawn;
			imageBox.RegionSelectStarted += this.imageBox_RegionSelectStarted;
			imageBox.RegionSelectEnded += this.imageBox_RegionSelectEnded;
			imageBox.OverlaysVisible = false;
			imageBox.RegionRectanglesVisible = true;
			productManager.PropertyChanged += this.productManager_PropertyChanged;
			this.InitializeRegionProduct();
			this.viewProducts = (ListCollectionView)CollectionViewSource.GetDefaultView(this.RegionProduct);
			this.viewProducts.CustomSort = new ProductDataComparer();
			this.timer = new Timer(new TimerCallback(this.LoadLastImage));
			this.timer.Change(100, -1);
		}

		// Token: 0x06001D3C RID: 7484 RVA: 0x000BC5C0 File Offset: 0x000BA7C0
		private void InitializeRegionProduct()
		{
			this.RegionProduct = new ObservableCollection<ProductData>();
			foreach (ProductData item in this.ProductManager.SelectedProduct.SubProducts)
			{
				this.RegionProduct.Add(item);
			}
			this.dummyProduct = new ProductData("", "", "");
			this.RegionProduct.Add(this.dummyProduct);
		}

		// Token: 0x06001D3D RID: 7485 RVA: 0x000BC65C File Offset: 0x000BA85C
		private void InitializeRegionFigures(XRSImage image)
		{
			bool flag = this.ProductManager.SelectedProduct != null && image != null;
			if (flag)
			{
				int j;
				int i;
				for (i = 0; i < Definition.allowedRegions; i = j + 1)
				{
					List<ProductData> list = (from product in this.ProductManager.SelectedProduct.SubProducts
					where product.SubProductIndex == i
					select product).ToList<ProductData>();
					bool flag2 = list.Count > 0;
					if (flag2)
					{
						image.Regions[i].Type = list[0].Region.Type;
						image.Regions[i].Vertex1 = list[0].Region.Vertex1;
						image.Regions[i].Vertex2 = list[0].Region.Vertex2;
						image.Regions[i].Vertex3 = list[0].Region.Vertex3;
						image.Regions[i].Vertex4 = list[0].Region.Vertex4;
						image.Regions[i].Vertex5 = list[0].Region.Vertex5;
						image.Regions[i].Vertex6 = list[0].Region.Vertex6;
						image.Regions[i].Vertex7 = list[0].Region.Vertex7;
						image.Regions[i].Vertex8 = list[0].Region.Vertex8;
						image.Regions[i].Vertex9 = list[0].Region.Vertex9;
						image.Regions[i].Vertex10 = list[0].Region.Vertex10;
					}
					else
					{
						image.Regions[i].Vertex1 = new Point(-1.0, -1.0);
						image.Regions[i].Vertex2 = new Point(-1.0, -1.0);
						image.Regions[i].Vertex3 = new Point(-1.0, -1.0);
						image.Regions[i].Vertex4 = new Point(-1.0, -1.0);
						image.Regions[i].Vertex5 = new Point(-1.0, -1.0);
						image.Regions[i].Vertex6 = new Point(-1.0, -1.0);
						image.Regions[i].Vertex7 = new Point(-1.0, -1.0);
						image.Regions[i].Vertex8 = new Point(-1.0, -1.0);
						image.Regions[i].Vertex9 = new Point(-1.0, -1.0);
						image.Regions[i].Vertex10 = new Point(-1.0, -1.0);
					}
					j = i;
				}
			}
		}

		// Token: 0x06001D3E RID: 7486 RVA: 0x000BCA34 File Offset: 0x000BAC34
		private void LoadLastImage(object obj)
		{
			bool flag = this.timer != null;
			if (flag)
			{
				this.timer.Dispose();
			}
			XRSImage image = HistoryHelper.GetLatestImage(0, this.imageArchiver);
			Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(delegate()
			{
				bool flag2 = image != null;
				if (flag2)
				{
					this.InitializeRegionFigures(image);
					this.CurrentImage = image;
					this.imageBox.HideOverlayImage();
				}
				else
				{
					Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtNoValidImage"), DialogStyle.Error);
				}
			}));
		}

		// Token: 0x06001D3F RID: 7487 RVA: 0x000BCA98 File Offset: 0x000BAC98
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x06001D40 RID: 7488 RVA: 0x000BCAC8 File Offset: 0x000BACC8
		private void AddRegion(object obj)
		{
			bool flag = this.ProductManager.SelectedProduct.SubProducts.Count < Definition.allowedRegions;
			if (flag)
			{
				this.ctrlChoseRegionType = new CtrlChoseRegionType(this.ProductManager, this.RegionProduct, this.dummyProduct, true);
				Subwindow.Show(this.ctrlChoseRegionType, SubwindowButtons.BackAndHome, this.ctrlChoseRegionType.ButtonsAddRegion);
				this.ctrlChoseRegionType.Unloaded += this.ctrlChoseRegionType_Unloaded;
			}
			else
			{
				Dialog.Show(LocalizationManager.Localize("txtError"), "Max. regions: " + Definition.allowedRegions.ToString(), DialogStyle.Error);
			}
		}

		// Token: 0x06001D41 RID: 7489 RVA: 0x000BCB70 File Offset: 0x000BAD70
		private void ctrlChoseRegionType_Unloaded(object sender, RoutedEventArgs e)
		{
			this.ctrlChoseRegionType.Unloaded -= this.ctrlChoseRegionType_Unloaded;
			bool flag = this.ctrlChoseRegionType != null && (this.ctrlChoseRegionType.RegionAdded || this.ctrlChoseRegionType.RegionEdited);
			if (flag)
			{
				this.InitializeRegionFigures(this.currentImage);
				this.imageBox.UpdateFigures();
			}
		}

		// Token: 0x06001D42 RID: 7490 RVA: 0x000BCBDC File Offset: 0x000BADDC
		private void RemoveRegion(object obj)
		{
			bool flag = this.ProductManager.SelectedProduct.SubProducts.Count <= 1;
			if (flag)
			{
				Dialog.Show(LocalizationManager.Localize("txtError"), "Min. regions: 1", DialogStyle.Error);
			}
			else
			{
				bool flag2 = this.ProductManager.SelectedSubProduct != null;
				if (flag2)
				{
					bool flag3 = Dialog.Show(LocalizationManager.Localize("txtWarning"), string.Concat(new string[]
					{
						LocalizationManager.Localize("txtApplication"),
						" \"",
						this.ProductManager.SelectedSubProduct.ProductName,
						"\" ",
						LocalizationManager.Localize("msgGettingErased")
					}), DialogStyle.Warning, DialogButtons.OKCancel) == Dialog.DialogResults.Ok;
					if (flag3)
					{
						this.ProductManager.RemoveSelectedSubProduct(true);
						this.RegionProduct.Remove(this.ProductManager.SelectedSubProduct);
						this.InitializeRegionFigures(this.currentImage);
						this.imageBox.UpdateFigures();
					}
				}
			}
		}

		// Token: 0x06001D43 RID: 7491 RVA: 0x000BCCDC File Offset: 0x000BAEDC
		private void imageBox_RegionSelectEnded(Point endPoint)
		{
			bool flag = this.ProductManager.SelectedSubProduct != null;
			if (flag)
			{
				bool flag2 = this.startPoint.X > endPoint.X;
				if (flag2)
				{
					this.maxPointRectangleX = this.startPoint.X;
					this.minPointRectangleX = endPoint.X;
				}
				else
				{
					this.maxPointRectangleX = endPoint.X;
					this.minPointRectangleX = this.startPoint.X;
				}
				bool flag3 = this.startPoint.Y > endPoint.Y;
				if (flag3)
				{
					this.maxPointRectangleY = this.startPoint.Y;
					this.minPointRectangleY = endPoint.Y;
				}
				else
				{
					this.maxPointRectangleY = endPoint.Y;
					this.minPointRectangleY = this.startPoint.Y;
				}
			}
		}

		// Token: 0x06001D44 RID: 7492 RVA: 0x000BCDB5 File Offset: 0x000BAFB5
		private void imageBox_RegionSelectStarted(Point startPoint)
		{
			this.startPoint = startPoint;
		}

		// Token: 0x06001D45 RID: 7493 RVA: 0x000BCDBF File Offset: 0x000BAFBF
		private void Initialize(object obj)
		{
			this.ProductManager.SelectedSubProduct = null;
		}

		// Token: 0x06001D46 RID: 7494 RVA: 0x000BCDD0 File Offset: 0x000BAFD0
		private void EditRegion(object obj)
		{
			this.ProductManager.RenameSelectedSubProduct("msgRenameRegion");
			bool flag = this.ProductManager.SelectedSubProduct != null;
			if (flag)
			{
				this.ctrlChoseRegionType = new CtrlChoseRegionType(this.ProductManager, this.RegionProduct, this.dummyProduct, false);
				Subwindow.Show(this.ctrlChoseRegionType, SubwindowButtons.None);
				this.ctrlChoseRegionType.Unloaded += this.ctrlChoseRegionType_Unloaded;
			}
		}

		// Token: 0x06001D47 RID: 7495 RVA: 0x000BCE48 File Offset: 0x000BB048
		private void CleanUp(object obj)
		{
			this.imageBox.RegionSelectStarted -= this.imageBox_RegionSelectStarted;
			this.imageBox.RegionSelectEnded -= this.imageBox_RegionSelectEnded;
			this.ProductManager.PropertyChanged += this.productManager_PropertyChanged;
		}

		// Token: 0x06001D48 RID: 7496 RVA: 0x000BCEA0 File Offset: 0x000BB0A0
		private void productManager_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			bool flag = e.PropertyName == "SelectedSubProduct";
			if (flag)
			{
				bool flag2 = this.ProductManager.SelectedSubProduct != null;
				if (flag2)
				{
					this.imageBox.SetCurrentRegionStrokeThickness(4.0);
					this.RegionNotEditable = true;
				}
			}
		}

		// Token: 0x06001D49 RID: 7497 RVA: 0x000BCEF8 File Offset: 0x000BB0F8
		private void MakeRegionEditable(object obj)
		{
			this.RegionNotEditable = false;
			bool isRectangleRegion = this.ProductManager.SelectedSubProduct.IsRectangleRegion;
			if (isRectangleRegion)
			{
				bool flag = this.imageBox.BoxSelectionMode > XRSImageBox.SelectionMode.PanAndZoom;
				if (flag)
				{
					this.imageBox.BoxSelectionMode = XRSImageBox.SelectionMode.DrawRegionRectangles;
				}
			}
			bool isPolygonalRegion = this.ProductManager.SelectedSubProduct.IsPolygonalRegion;
			if (isPolygonalRegion)
			{
				bool flag2 = this.imageBox.BoxSelectionMode > XRSImageBox.SelectionMode.PanAndZoom;
				if (flag2)
				{
					this.imageBox.BoxSelectionMode = XRSImageBox.SelectionMode.DrawCrossesConnected;
				}
				this.imageBox.CrossColor = XRSImageDefs.GetRegionStroke(this.ProductManager.SelectedSubProduct.SubProductIndex);
				this.imageBox.CrossStrokeDash = XRSImageDefs.GetRegionDashArray(this.ProductManager.SelectedSubProduct.SubProductIndex);
				this.imageBox.CrossStrokeThickness = 4.0;
			}
			bool isEllipticRegion = this.ProductManager.SelectedSubProduct.IsEllipticRegion;
			if (isEllipticRegion)
			{
				bool flag3 = this.imageBox.BoxSelectionMode > XRSImageBox.SelectionMode.PanAndZoom;
				if (flag3)
				{
					this.imageBox.BoxSelectionMode = XRSImageBox.SelectionMode.DrawEllipse;
				}
				this.imageBox.CrossColor = XRSImageDefs.GetRegionStroke(this.ProductManager.SelectedSubProduct.SubProductIndex);
				this.imageBox.CrossStrokeDash = XRSImageDefs.GetRegionDashArray(this.ProductManager.SelectedSubProduct.SubProductIndex);
				this.imageBox.CrossStrokeThickness = 4.0;
			}
		}

		// Token: 0x06001D4A RID: 7498 RVA: 0x000BD060 File Offset: 0x000BB260
		private void CommitRegionSelect(object obj)
		{
			this.WriteNewRegionToParameterInterface();
			bool isRectangleRegion = this.ProductManager.SelectedSubProduct.IsRectangleRegion;
			if (isRectangleRegion)
			{
				this.CommitRectangleRegion();
			}
			else
			{
				bool isEllipticRegion = this.ProductManager.SelectedSubProduct.IsEllipticRegion;
				if (isEllipticRegion)
				{
					this.CommitEllipticRegion();
				}
				else
				{
					bool isPolygonalRegion = this.ProductManager.SelectedSubProduct.IsPolygonalRegion;
					if (isPolygonalRegion)
					{
						this.CommitPolygonalRegion();
					}
				}
			}
			this.imageBox.SetAutoZoomAndBrightness();
			this.imageBox.HideOverlayImage();
			this.InitializeRegionFigures(this.currentImage);
			this.imageBox.UpdateFigures();
			this.RegionNotEditable = true;
		}

		// Token: 0x06001D4B RID: 7499 RVA: 0x000BD104 File Offset: 0x000BB304
		private void WriteNewRegionToParameterInterface()
		{
			int group = (int)this.ProductManager.SelectedSubProduct.Group;
			bool flag = this.ProductManager.SelectedProduct == this.ProductManager.CurrentLoadedProduct;
			if (flag)
			{
				bool isRectangleRegion = this.ProductManager.SelectedSubProduct.IsRectangleRegion;
				if (isRectangleRegion)
				{
					this.parHandling.WriteValue(Definition.filterMainPath + group.ToString() + Definition.vertex1XSubPath, this.minPointRectangleX.ToString(), false);
					this.parHandling.WriteValue(Definition.filterMainPath + group.ToString() + Definition.vertex1YSubPath, this.minPointRectangleY.ToString(), false);
					this.parHandling.WriteValue(Definition.filterMainPath + group.ToString() + Definition.vertex2XSubPath, this.maxPointRectangleX.ToString(), false);
					this.parHandling.WriteValue(Definition.filterMainPath + group.ToString() + Definition.vertex2YSubPath, this.maxPointRectangleY.ToString(), false);
				}
				else
				{
					bool isEllipticRegion = this.ProductManager.SelectedSubProduct.IsEllipticRegion;
					if (isEllipticRegion)
					{
						Point point = new Point(this.minPointRectangleX + (this.maxPointRectangleX - this.minPointRectangleX) / 2.0, this.minPointRectangleY + (this.maxPointRectangleY - this.minPointRectangleY) / 2.0);
						Point point2 = new Point(this.maxPointRectangleX - this.minPointRectangleX, this.maxPointRectangleY - this.minPointRectangleY);
						this.parHandling.WriteValue(Definition.filterMainPath + group.ToString() + Definition.vertex1XSubPath, point.X.ToString(), false);
						this.parHandling.WriteValue(Definition.filterMainPath + group.ToString() + Definition.vertex1YSubPath, point.Y.ToString(), false);
						this.parHandling.WriteValue(Definition.filterMainPath + group.ToString() + Definition.vertex2XSubPath, point2.X.ToString(), false);
						this.parHandling.WriteValue(Definition.filterMainPath + group.ToString() + Definition.vertex2YSubPath, point2.Y.ToString(), false);
					}
					else
					{
						bool isPolygonalRegion = this.ProductManager.SelectedSubProduct.IsPolygonalRegion;
						if (isPolygonalRegion)
						{
							bool flag2 = this.imageBox.Crosses.Count > 0;
							if (flag2)
							{
								this.parHandling.WriteValue(Definition.filterMainPath + group.ToString() + Definition.vertex1XSubPath, ((int)this.imageBox.Crosses[0].CenterInImage.X).ToString(), false);
								this.parHandling.WriteValue(Definition.filterMainPath + group.ToString() + Definition.vertex1YSubPath, ((int)this.imageBox.Crosses[0].CenterInImage.Y).ToString(), false);
							}
							else
							{
								this.parHandling.WriteValue(Definition.filterMainPath + group.ToString() + Definition.vertex1XSubPath, "-1", false);
								this.parHandling.WriteValue(Definition.filterMainPath + group.ToString() + Definition.vertex1YSubPath, "-1", false);
							}
							bool flag3 = this.imageBox.Crosses.Count > 1;
							if (flag3)
							{
								this.parHandling.WriteValue(Definition.filterMainPath + group.ToString() + Definition.vertex2XSubPath, ((int)this.imageBox.Crosses[1].CenterInImage.X).ToString(), false);
								this.parHandling.WriteValue(Definition.filterMainPath + group.ToString() + Definition.vertex2YSubPath, ((int)this.imageBox.Crosses[1].CenterInImage.Y).ToString(), false);
							}
							else
							{
								this.parHandling.WriteValue(Definition.filterMainPath + group.ToString() + Definition.vertex2XSubPath, "-1", false);
								this.parHandling.WriteValue(Definition.filterMainPath + group.ToString() + Definition.vertex2YSubPath, "-1", false);
							}
							bool flag4 = this.imageBox.Crosses.Count > 2;
							if (flag4)
							{
								this.parHandling.WriteValue(Definition.filterMainPath + group.ToString() + Definition.vertex3XSubPath, ((int)this.imageBox.Crosses[2].CenterInImage.X).ToString(), false);
								this.parHandling.WriteValue(Definition.filterMainPath + group.ToString() + Definition.vertex3YSubPath, ((int)this.imageBox.Crosses[2].CenterInImage.Y).ToString(), false);
							}
							else
							{
								this.parHandling.WriteValue(Definition.filterMainPath + group.ToString() + Definition.vertex3XSubPath, "-1", false);
								this.parHandling.WriteValue(Definition.filterMainPath + group.ToString() + Definition.vertex3YSubPath, "-1", false);
							}
							bool flag5 = this.imageBox.Crosses.Count > 3;
							if (flag5)
							{
								this.parHandling.WriteValue(Definition.filterMainPath + group.ToString() + Definition.vertex4XSubPath, ((int)this.imageBox.Crosses[3].CenterInImage.X).ToString(), false);
								this.parHandling.WriteValue(Definition.filterMainPath + group.ToString() + Definition.vertex4YSubPath, ((int)this.imageBox.Crosses[3].CenterInImage.Y).ToString(), false);
							}
							else
							{
								this.parHandling.WriteValue(Definition.filterMainPath + group.ToString() + Definition.vertex4XSubPath, "-1", false);
								this.parHandling.WriteValue(Definition.filterMainPath + group.ToString() + Definition.vertex4YSubPath, "-1", false);
							}
							bool flag6 = this.imageBox.Crosses.Count > 4;
							if (flag6)
							{
								this.parHandling.WriteValue(Definition.filterMainPath + group.ToString() + Definition.vertex5XSubPath, ((int)this.imageBox.Crosses[4].CenterInImage.X).ToString(), false);
								this.parHandling.WriteValue(Definition.filterMainPath + group.ToString() + Definition.vertex5YSubPath, ((int)this.imageBox.Crosses[4].CenterInImage.Y).ToString(), false);
							}
							else
							{
								this.parHandling.WriteValue(Definition.filterMainPath + group.ToString() + Definition.vertex5XSubPath, "-1", false);
								this.parHandling.WriteValue(Definition.filterMainPath + group.ToString() + Definition.vertex5YSubPath, "-1", false);
							}
							bool flag7 = this.imageBox.Crosses.Count > 5;
							if (flag7)
							{
								this.parHandling.WriteValue(Definition.filterMainPath + group.ToString() + Definition.vertex6XSubPath, ((int)this.imageBox.Crosses[5].CenterInImage.X).ToString(), false);
								this.parHandling.WriteValue(Definition.filterMainPath + group.ToString() + Definition.vertex6YSubPath, ((int)this.imageBox.Crosses[5].CenterInImage.Y).ToString(), false);
							}
							else
							{
								this.parHandling.WriteValue(Definition.filterMainPath + group.ToString() + Definition.vertex6XSubPath, "-1", false);
								this.parHandling.WriteValue(Definition.filterMainPath + group.ToString() + Definition.vertex6YSubPath, "-1", false);
							}
							bool flag8 = this.imageBox.Crosses.Count > 6;
							if (flag8)
							{
								this.parHandling.WriteValue(Definition.filterMainPath + group.ToString() + Definition.vertex7XSubPath, ((int)this.imageBox.Crosses[6].CenterInImage.X).ToString(), false);
								this.parHandling.WriteValue(Definition.filterMainPath + group.ToString() + Definition.vertex7YSubPath, ((int)this.imageBox.Crosses[6].CenterInImage.Y).ToString(), false);
							}
							else
							{
								this.parHandling.WriteValue(Definition.filterMainPath + group.ToString() + Definition.vertex7XSubPath, "-1", false);
								this.parHandling.WriteValue(Definition.filterMainPath + group.ToString() + Definition.vertex7YSubPath, "-1", false);
							}
							bool flag9 = this.imageBox.Crosses.Count > 7;
							if (flag9)
							{
								this.parHandling.WriteValue(Definition.filterMainPath + group.ToString() + Definition.vertex8XSubPath, ((int)this.imageBox.Crosses[7].CenterInImage.X).ToString(), false);
								this.parHandling.WriteValue(Definition.filterMainPath + group.ToString() + Definition.vertex8YSubPath, ((int)this.imageBox.Crosses[7].CenterInImage.Y).ToString(), false);
							}
							else
							{
								this.parHandling.WriteValue(Definition.filterMainPath + group.ToString() + Definition.vertex8XSubPath, "-1", false);
								this.parHandling.WriteValue(Definition.filterMainPath + group.ToString() + Definition.vertex8YSubPath, "-1", false);
							}
							bool flag10 = this.imageBox.Crosses.Count > 8;
							if (flag10)
							{
								this.parHandling.WriteValue(Definition.filterMainPath + group.ToString() + Definition.vertex9XSubPath, ((int)this.imageBox.Crosses[8].CenterInImage.X).ToString(), false);
								this.parHandling.WriteValue(Definition.filterMainPath + group.ToString() + Definition.vertex9YSubPath, ((int)this.imageBox.Crosses[8].CenterInImage.Y).ToString(), false);
							}
							else
							{
								this.parHandling.WriteValue(Definition.filterMainPath + group.ToString() + Definition.vertex9XSubPath, "-1", false);
								this.parHandling.WriteValue(Definition.filterMainPath + group.ToString() + Definition.vertex9YSubPath, "-1", false);
							}
							bool flag11 = this.imageBox.Crosses.Count > 9;
							if (flag11)
							{
								this.parHandling.WriteValue(Definition.filterMainPath + group.ToString() + Definition.vertex10XSubPath, ((int)this.imageBox.Crosses[9].CenterInImage.X).ToString(), false);
								this.parHandling.WriteValue(Definition.filterMainPath + group.ToString() + Definition.vertex10YSubPath, ((int)this.imageBox.Crosses[9].CenterInImage.Y).ToString(), false);
							}
							else
							{
								this.parHandling.WriteValue(Definition.filterMainPath + group.ToString() + Definition.vertex10XSubPath, "-1", false);
								this.parHandling.WriteValue(Definition.filterMainPath + group.ToString() + Definition.vertex10YSubPath, "-1", false);
							}
						}
					}
				}
			}
		}

		// Token: 0x06001D4C RID: 7500 RVA: 0x000BDDEC File Offset: 0x000BBFEC
		private void CommitRectangleRegion()
		{
			int group = (int)this.ProductManager.SelectedSubProduct.Group;
			this.dbConnection.UpdateTable(this.ProductManager.SelectedSubProduct.TableName, Definition.filterMainPath + group.ToString() + Definition.vertex1XSubPath, this.minPointRectangleX.ToString());
			this.dbConnection.UpdateTable(this.ProductManager.SelectedSubProduct.TableName, Definition.filterMainPath + group.ToString() + Definition.vertex1YSubPath, this.minPointRectangleY.ToString());
			this.dbConnection.UpdateTable(this.ProductManager.SelectedSubProduct.TableName, Definition.filterMainPath + group.ToString() + Definition.vertex2XSubPath, this.maxPointRectangleX.ToString());
			this.dbConnection.UpdateTable(this.ProductManager.SelectedSubProduct.TableName, Definition.filterMainPath + group.ToString() + Definition.vertex2YSubPath, this.maxPointRectangleY.ToString());
			this.ProductManager.SelectedSubProduct.Region.Vertex1 = new Point(this.minPointRectangleX, this.minPointRectangleY);
			this.ProductManager.SelectedSubProduct.Region.Vertex2 = new Point(this.maxPointRectangleX, this.maxPointRectangleY);
		}

		// Token: 0x06001D4D RID: 7501 RVA: 0x000BDF50 File Offset: 0x000BC150
		private void CommitEllipticRegion()
		{
			int group = (int)this.ProductManager.SelectedSubProduct.Group;
			Point point = new Point(this.minPointRectangleX + (this.maxPointRectangleX - this.minPointRectangleX) / 2.0, this.minPointRectangleY + (this.maxPointRectangleY - this.minPointRectangleY) / 2.0);
			Point point2 = new Point(this.maxPointRectangleX - this.minPointRectangleX, this.maxPointRectangleY - this.minPointRectangleY);
			this.dbConnection.UpdateTable(this.ProductManager.SelectedSubProduct.TableName, Definition.filterMainPath + group.ToString() + Definition.vertex1XSubPath, point.X.ToString());
			this.dbConnection.UpdateTable(this.ProductManager.SelectedSubProduct.TableName, Definition.filterMainPath + group.ToString() + Definition.vertex1YSubPath, point.Y.ToString());
			this.dbConnection.UpdateTable(this.ProductManager.SelectedSubProduct.TableName, Definition.filterMainPath + group.ToString() + Definition.vertex2XSubPath, point2.X.ToString());
			this.dbConnection.UpdateTable(this.ProductManager.SelectedSubProduct.TableName, Definition.filterMainPath + group.ToString() + Definition.vertex2YSubPath, point2.Y.ToString());
			this.ProductManager.SelectedSubProduct.Region.Vertex1 = new Point(point.X, point.Y);
			this.ProductManager.SelectedSubProduct.Region.Vertex2 = new Point(point2.X, point2.Y);
		}

		// Token: 0x06001D4E RID: 7502 RVA: 0x000BE12C File Offset: 0x000BC32C
		private void CommitPolygonalRegion()
		{
			bool flag = this.imageBox.Crosses.Count > 2;
			if (flag)
			{
				int group = (int)this.ProductManager.SelectedSubProduct.Group;
				this.dbConnection.UpdateTable(this.ProductManager.SelectedSubProduct.TableName, Definition.filterMainPath + group.ToString() + Definition.vertex1XSubPath, ((int)this.imageBox.Crosses[0].CenterInImage.X).ToString());
				this.dbConnection.UpdateTable(this.ProductManager.SelectedSubProduct.TableName, Definition.filterMainPath + group.ToString() + Definition.vertex1YSubPath, ((int)this.imageBox.Crosses[0].CenterInImage.Y).ToString());
				this.ProductManager.SelectedSubProduct.Region.Vertex1 = new Point(this.imageBox.Crosses[0].CenterInImage.X, this.imageBox.Crosses[0].CenterInImage.Y);
				this.dbConnection.UpdateTable(this.ProductManager.SelectedSubProduct.TableName, Definition.filterMainPath + group.ToString() + Definition.vertex2XSubPath, ((int)this.imageBox.Crosses[1].CenterInImage.X).ToString());
				this.dbConnection.UpdateTable(this.ProductManager.SelectedSubProduct.TableName, Definition.filterMainPath + group.ToString() + Definition.vertex2YSubPath, ((int)this.imageBox.Crosses[1].CenterInImage.Y).ToString());
				this.ProductManager.SelectedSubProduct.Region.Vertex2 = new Point(this.imageBox.Crosses[1].CenterInImage.X, this.imageBox.Crosses[1].CenterInImage.Y);
				this.dbConnection.UpdateTable(this.ProductManager.SelectedSubProduct.TableName, Definition.filterMainPath + group.ToString() + Definition.vertex3XSubPath, ((int)this.imageBox.Crosses[2].CenterInImage.X).ToString());
				this.dbConnection.UpdateTable(this.ProductManager.SelectedSubProduct.TableName, Definition.filterMainPath + group.ToString() + Definition.vertex3YSubPath, ((int)this.imageBox.Crosses[2].CenterInImage.Y).ToString());
				this.ProductManager.SelectedSubProduct.Region.Vertex3 = new Point(this.imageBox.Crosses[2].CenterInImage.X, this.imageBox.Crosses[2].CenterInImage.Y);
				bool flag2 = this.imageBox.Crosses.Count > 3;
				if (flag2)
				{
					this.dbConnection.UpdateTable(this.ProductManager.SelectedSubProduct.TableName, Definition.filterMainPath + group.ToString() + Definition.vertex4XSubPath, ((int)this.imageBox.Crosses[3].CenterInImage.X).ToString());
					this.dbConnection.UpdateTable(this.ProductManager.SelectedSubProduct.TableName, Definition.filterMainPath + group.ToString() + Definition.vertex4YSubPath, ((int)this.imageBox.Crosses[3].CenterInImage.Y).ToString());
					this.ProductManager.SelectedSubProduct.Region.Vertex4 = new Point(this.imageBox.Crosses[3].CenterInImage.X, this.imageBox.Crosses[3].CenterInImage.Y);
				}
				else
				{
					this.dbConnection.UpdateTable(this.ProductManager.SelectedSubProduct.TableName, Definition.filterMainPath + group.ToString() + Definition.vertex4XSubPath, "-1");
					this.dbConnection.UpdateTable(this.ProductManager.SelectedSubProduct.TableName, Definition.filterMainPath + group.ToString() + Definition.vertex4YSubPath, "-1");
					this.ProductManager.SelectedSubProduct.Region.Vertex4 = new Point(-1.0, -1.0);
				}
				bool flag3 = this.imageBox.Crosses.Count > 4;
				if (flag3)
				{
					this.dbConnection.UpdateTable(this.ProductManager.SelectedSubProduct.TableName, Definition.filterMainPath + group.ToString() + Definition.vertex5XSubPath, ((int)this.imageBox.Crosses[4].CenterInImage.X).ToString());
					this.dbConnection.UpdateTable(this.ProductManager.SelectedSubProduct.TableName, Definition.filterMainPath + group.ToString() + Definition.vertex5YSubPath, ((int)this.imageBox.Crosses[4].CenterInImage.Y).ToString());
					this.ProductManager.SelectedSubProduct.Region.Vertex5 = new Point(this.imageBox.Crosses[4].CenterInImage.X, this.imageBox.Crosses[4].CenterInImage.Y);
				}
				else
				{
					this.dbConnection.UpdateTable(this.ProductManager.SelectedSubProduct.TableName, Definition.filterMainPath + group.ToString() + Definition.vertex5XSubPath, "-1");
					this.dbConnection.UpdateTable(this.ProductManager.SelectedSubProduct.TableName, Definition.filterMainPath + group.ToString() + Definition.vertex5YSubPath, "-1");
					this.ProductManager.SelectedSubProduct.Region.Vertex5 = new Point(-1.0, -1.0);
				}
				bool flag4 = this.imageBox.Crosses.Count > 5;
				if (flag4)
				{
					this.dbConnection.UpdateTable(this.ProductManager.SelectedSubProduct.TableName, Definition.filterMainPath + group.ToString() + Definition.vertex6XSubPath, ((int)this.imageBox.Crosses[5].CenterInImage.X).ToString());
					this.dbConnection.UpdateTable(this.ProductManager.SelectedSubProduct.TableName, Definition.filterMainPath + group.ToString() + Definition.vertex6YSubPath, ((int)this.imageBox.Crosses[5].CenterInImage.Y).ToString());
					this.ProductManager.SelectedSubProduct.Region.Vertex6 = new Point(this.imageBox.Crosses[5].CenterInImage.X, this.imageBox.Crosses[5].CenterInImage.Y);
				}
				else
				{
					this.dbConnection.UpdateTable(this.ProductManager.SelectedSubProduct.TableName, Definition.filterMainPath + group.ToString() + Definition.vertex6XSubPath, "-1");
					this.dbConnection.UpdateTable(this.ProductManager.SelectedSubProduct.TableName, Definition.filterMainPath + group.ToString() + Definition.vertex6YSubPath, "-1");
					this.ProductManager.SelectedSubProduct.Region.Vertex6 = new Point(-1.0, -1.0);
				}
				bool flag5 = this.imageBox.Crosses.Count > 6;
				if (flag5)
				{
					this.dbConnection.UpdateTable(this.ProductManager.SelectedSubProduct.TableName, Definition.filterMainPath + group.ToString() + Definition.vertex7XSubPath, ((int)this.imageBox.Crosses[6].CenterInImage.X).ToString());
					this.dbConnection.UpdateTable(this.ProductManager.SelectedSubProduct.TableName, Definition.filterMainPath + group.ToString() + Definition.vertex7YSubPath, ((int)this.imageBox.Crosses[6].CenterInImage.Y).ToString());
					this.ProductManager.SelectedSubProduct.Region.Vertex7 = new Point(this.imageBox.Crosses[6].CenterInImage.X, this.imageBox.Crosses[6].CenterInImage.Y);
				}
				else
				{
					this.dbConnection.UpdateTable(this.ProductManager.SelectedSubProduct.TableName, Definition.filterMainPath + group.ToString() + Definition.vertex7XSubPath, "-1");
					this.dbConnection.UpdateTable(this.ProductManager.SelectedSubProduct.TableName, Definition.filterMainPath + group.ToString() + Definition.vertex7YSubPath, "-1");
					this.ProductManager.SelectedSubProduct.Region.Vertex7 = new Point(-1.0, -1.0);
				}
				bool flag6 = this.imageBox.Crosses.Count > 7;
				if (flag6)
				{
					this.dbConnection.UpdateTable(this.ProductManager.SelectedSubProduct.TableName, Definition.filterMainPath + group.ToString() + Definition.vertex8XSubPath, ((int)this.imageBox.Crosses[7].CenterInImage.X).ToString());
					this.dbConnection.UpdateTable(this.ProductManager.SelectedSubProduct.TableName, Definition.filterMainPath + group.ToString() + Definition.vertex8YSubPath, ((int)this.imageBox.Crosses[7].CenterInImage.Y).ToString());
					this.ProductManager.SelectedSubProduct.Region.Vertex8 = new Point(this.imageBox.Crosses[7].CenterInImage.X, this.imageBox.Crosses[7].CenterInImage.Y);
				}
				else
				{
					this.dbConnection.UpdateTable(this.ProductManager.SelectedSubProduct.TableName, Definition.filterMainPath + group.ToString() + Definition.vertex8XSubPath, "-1");
					this.dbConnection.UpdateTable(this.ProductManager.SelectedSubProduct.TableName, Definition.filterMainPath + group.ToString() + Definition.vertex8YSubPath, "-1");
					this.ProductManager.SelectedSubProduct.Region.Vertex8 = new Point(-1.0, -1.0);
				}
				bool flag7 = this.imageBox.Crosses.Count > 8;
				if (flag7)
				{
					this.dbConnection.UpdateTable(this.ProductManager.SelectedSubProduct.TableName, Definition.filterMainPath + group.ToString() + Definition.vertex9XSubPath, ((int)this.imageBox.Crosses[8].CenterInImage.X).ToString());
					this.dbConnection.UpdateTable(this.ProductManager.SelectedSubProduct.TableName, Definition.filterMainPath + group.ToString() + Definition.vertex9YSubPath, ((int)this.imageBox.Crosses[8].CenterInImage.Y).ToString());
					this.ProductManager.SelectedSubProduct.Region.Vertex9 = new Point(this.imageBox.Crosses[8].CenterInImage.X, this.imageBox.Crosses[8].CenterInImage.Y);
				}
				else
				{
					this.dbConnection.UpdateTable(this.ProductManager.SelectedSubProduct.TableName, Definition.filterMainPath + group.ToString() + Definition.vertex9XSubPath, "-1");
					this.dbConnection.UpdateTable(this.ProductManager.SelectedSubProduct.TableName, Definition.filterMainPath + group.ToString() + Definition.vertex9YSubPath, "-1");
					this.ProductManager.SelectedSubProduct.Region.Vertex9 = new Point(-1.0, -1.0);
				}
				bool flag8 = this.imageBox.Crosses.Count > 9;
				if (flag8)
				{
					this.dbConnection.UpdateTable(this.ProductManager.SelectedSubProduct.TableName, Definition.filterMainPath + group.ToString() + Definition.vertex10XSubPath, ((int)this.imageBox.Crosses[9].CenterInImage.X).ToString());
					this.dbConnection.UpdateTable(this.ProductManager.SelectedSubProduct.TableName, Definition.filterMainPath + group.ToString() + Definition.vertex10YSubPath, ((int)this.imageBox.Crosses[9].CenterInImage.Y).ToString());
					this.ProductManager.SelectedSubProduct.Region.Vertex10 = new Point(this.imageBox.Crosses[9].CenterInImage.X, this.imageBox.Crosses[9].CenterInImage.Y);
				}
				else
				{
					this.dbConnection.UpdateTable(this.ProductManager.SelectedSubProduct.TableName, Definition.filterMainPath + group.ToString() + Definition.vertex10XSubPath, "-1");
					this.dbConnection.UpdateTable(this.ProductManager.SelectedSubProduct.TableName, Definition.filterMainPath + group.ToString() + Definition.vertex10YSubPath, "-1");
					this.ProductManager.SelectedSubProduct.Region.Vertex10 = new Point(-1.0, -1.0);
				}
			}
			else
			{
				Dialog.Show(LocalizationManager.Localize("txtError"), "Min. vertices: 3", DialogStyle.Error, DialogButtons.OK);
			}
		}

		// Token: 0x06001D4F RID: 7503 RVA: 0x000BF120 File Offset: 0x000BD320
		private void AbortRegionSelect(object obj)
		{
			XRSImage image = this.currentImage.DeepCopy();
			this.InitializeRegionFigures(image);
			this.CurrentImage = image;
			this.imageBox.SetAutoZoomAndBrightness();
			this.imageBox.HideOverlayImage();
			this.RegionNotEditable = true;
		}

		// Token: 0x06001D50 RID: 7504 RVA: 0x000BF16A File Offset: 0x000BD36A
		private void RemoveLastCross(object obj)
		{
			this.imageBox.RemoveLastDrawnCross();
			this.BackCommandVisibility = ((this.imageBox.Crosses.Count > 0) ? Visibility.Visible : Visibility.Collapsed);
		}

		// Token: 0x06001D51 RID: 7505 RVA: 0x000BF197 File Offset: 0x000BD397
		private void imagebox_CrossDrawn(Point obj)
		{
			this.BackCommandVisibility = ((this.imageBox.Crosses.Count > 0) ? Visibility.Visible : Visibility.Collapsed);
		}

		// Token: 0x0400133C RID: 4924
		private ParameterHandling parHandling;

		// Token: 0x0400133D RID: 4925
		private ImageDataTransfer imageTransfer;

		// Token: 0x0400133E RID: 4926
		private DBConnection dbConnection;

		// Token: 0x0400133F RID: 4927
		private Timer timer;

		// Token: 0x04001340 RID: 4928
		private XRSImage currentImage;

		// Token: 0x04001341 RID: 4929
		private XRSImageBox imageBox;

		// Token: 0x04001342 RID: 4930
		private Point startPoint;

		// Token: 0x04001343 RID: 4931
		private bool regionNotEditable;

		// Token: 0x04001344 RID: 4932
		private ProductData dummyProduct;

		// Token: 0x04001345 RID: 4933
		private ListCollectionView viewProducts;

		// Token: 0x04001346 RID: 4934
		private double minPointRectangleX;

		// Token: 0x04001347 RID: 4935
		private double maxPointRectangleX;

		// Token: 0x04001348 RID: 4936
		private double minPointRectangleY;

		// Token: 0x04001349 RID: 4937
		private double maxPointRectangleY;

		// Token: 0x0400134A RID: 4938
		private CtrlChoseRegionType ctrlChoseRegionType;

		// Token: 0x0400134B RID: 4939
		private Visibility backCommandVisible = Visibility.Collapsed;

		// Token: 0x0400134C RID: 4940
		private StoreImageData imageArchiver;
	}
}
