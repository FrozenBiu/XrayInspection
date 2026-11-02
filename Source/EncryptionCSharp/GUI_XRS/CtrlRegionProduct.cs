using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using DatabaseConnection;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x0200012E RID: 302
	public class CtrlRegionProduct : UserControl, IComponentConnector
	{
		// Token: 0x06001D16 RID: 7446 RVA: 0x000BBE8D File Offset: 0x000BA08D
		public CtrlRegionProduct(ParameterHandling parHandling, ImageDataTransfer imageTransfer, DBConnection dbConnection, ProductManager productManager, StoreImageData imageArchiver)
		{
			this.InitializeComponent();
			this.viewModel = new RegionProductViewModel(parHandling, imageTransfer, dbConnection, productManager, this.imagebox, imageArchiver);
			this.InitializeButtons();
			base.DataContext = this.viewModel;
		}

		// Token: 0x06001D17 RID: 7447 RVA: 0x000BBECC File Offset: 0x000BA0CC
		private void InitializeButtons()
		{
			this.Buttons = new List<Button>();
			this.btnBack = new Button();
			this.btnBack.Content = ImageConverter.ToImage("/img/arrow-left.png");
			this.btnBack.Command = this.viewModel.RemoveLastCrossCommand;
			this.btnBack.Style = (base.FindResource("defaultButtonStyle") as Style);
			Binding binding = new Binding("BackCommandVisibility");
			binding.Source = this.viewModel;
			this.btnBack.SetBinding(UIElement.VisibilityProperty, binding);
			this.Buttons.Add(this.btnBack);
			this.btnMoveOrSelect = new Button();
			this.btnMoveOrSelect.Click += this.ChangeToMoveOrSelect;
			this.btnMoveOrSelect.Style = (base.FindResource("defaultButtonStyle") as Style);
			this.Buttons.Add(this.btnMoveOrSelect);
			this.imagebox.ToggleBoxSelectionModeAndButtonImage(this.btnMoveOrSelect, XRSImageBox.SelectionMode.DrawRegionRectangles);
		}

		// Token: 0x06001D18 RID: 7448 RVA: 0x000BBFDC File Offset: 0x000BA1DC
		private void ChangeToMoveOrSelect(object sender, RoutedEventArgs e)
		{
			XRSImageBox.SelectionMode otherModeThanPanAndZomm = XRSImageBox.SelectionMode.DrawRegionRectangles;
			bool flag = this.viewModel.ProductManager.SelectedSubProduct == null || this.viewModel.ProductManager.SelectedSubProduct.IsRectangleRegion;
			if (flag)
			{
				otherModeThanPanAndZomm = XRSImageBox.SelectionMode.DrawRegionRectangles;
			}
			else
			{
				bool isPolygonalRegion = this.viewModel.ProductManager.SelectedSubProduct.IsPolygonalRegion;
				if (isPolygonalRegion)
				{
					otherModeThanPanAndZomm = XRSImageBox.SelectionMode.DrawCrossesConnected;
				}
				else
				{
					bool isEllipticRegion = this.viewModel.ProductManager.SelectedSubProduct.IsEllipticRegion;
					if (isEllipticRegion)
					{
						otherModeThanPanAndZomm = XRSImageBox.SelectionMode.DrawEllipse;
					}
				}
			}
			this.imagebox.ToggleBoxSelectionModeAndButtonImage(this.btnMoveOrSelect, otherModeThanPanAndZomm);
		}

		// Token: 0x06001D19 RID: 7449 RVA: 0x000BC06C File Offset: 0x000BA26C
		private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			bool flag = sender is ListBox && ((ListBox)sender).SelectedItem != null && ((ListBox)sender).SelectedItem is ProductData;
			if (flag)
			{
				ProductData productData = (ProductData)((ListBox)sender).SelectedItem;
				bool flag2 = productData.ProductNumber.Length <= 0 || !productData.IsSingleProduct;
				if (flag2)
				{
					bool flag3 = e.RemovedItems.Count > 0 && e.RemovedItems[0] is ProductData;
					if (flag3)
					{
						((ListBox)sender).SelectedItem = e.RemovedItems[0];
					}
					else
					{
						((ListBox)sender).SelectedIndex = -1;
					}
				}
			}
		}

		// Token: 0x06001D1A RID: 7450 RVA: 0x000BC130 File Offset: 0x000BA330
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/productchange/ctrlregionproduct.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x06001D1B RID: 7451 RVA: 0x000BC168 File Offset: 0x000BA368
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				this.tbCurrentAppl = (TextBlock)target;
				break;
			case 2:
				this.lstSubProducts = (ListBox)target;
				this.lstSubProducts.SelectionChanged += this.ListBox_SelectionChanged;
				break;
			case 3:
				this.imagebox = (XRSImageBox)target;
				break;
			default:
				this._contentLoaded = true;
				break;
			}
		}

		// Token: 0x04001334 RID: 4916
		private RegionProductViewModel viewModel;

		// Token: 0x04001335 RID: 4917
		private Button btnMoveOrSelect;

		// Token: 0x04001336 RID: 4918
		private Button btnBack;

		// Token: 0x04001337 RID: 4919
		public List<Button> Buttons;

		// Token: 0x04001338 RID: 4920
		internal TextBlock tbCurrentAppl;

		// Token: 0x04001339 RID: 4921
		internal ListBox lstSubProducts;

		// Token: 0x0400133A RID: 4922
		internal XRSImageBox imagebox;

		// Token: 0x0400133B RID: 4923
		private bool _contentLoaded;
	}
}
