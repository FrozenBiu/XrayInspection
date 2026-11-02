using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x02000131 RID: 305
	public class CtrlSubProduct : UserControl, IComponentConnector, IStyleConnector
	{
		// Token: 0x17000892 RID: 2194
		// (get) Token: 0x06001D55 RID: 7509 RVA: 0x000BF24D File Offset: 0x000BD44D
		// (set) Token: 0x06001D56 RID: 7510 RVA: 0x000BF255 File Offset: 0x000BD455
		public List<Button> Buttons { get; set; }

		// Token: 0x06001D57 RID: 7511 RVA: 0x000BF25E File Offset: 0x000BD45E
		public CtrlSubProduct(ProductManager productManager, SubProductModes mode) : this(productManager, mode, null, null)
		{
		}

		// Token: 0x06001D58 RID: 7512 RVA: 0x000BF26C File Offset: 0x000BD46C
		public CtrlSubProduct(ProductManager productManager, SubProductModes mode, XRSImage image, SetFooterMainWindow setFooterMainWindow)
		{
			this.productManager = productManager;
			this.setFooterMainWindow = setFooterMainWindow;
			this.InitializeComponent();
			this.viewModel = new SubProductViewModel(productManager, image);
			this.InitializeButtons(mode);
			productManager.RetrainGroupSucceeded += this.productManager_RetrainGroupSucceeded;
			productManager.PropertyChanged += this.productManager_PropertyChanged;
			base.DataContext = this.viewModel;
		}

		// Token: 0x06001D59 RID: 7513 RVA: 0x000BF2E0 File Offset: 0x000BD4E0
		private void InitializeButtons(SubProductModes mode)
		{
			this.Buttons = new List<Button>();
			bool flag = mode == SubProductModes.AddOrRemove;
			if (flag)
			{
				this.btnAddSubProduct = new Button();
				this.btnAddSubProduct.Content = ImageConverter.ToImage("/img/addproduct.png");
				this.btnAddSubProduct.Click += this.btnAddSubProduct_Click;
				this.btnAddSubProduct.Style = (base.FindResource("defaultButtonStyle") as Style);
				this.Buttons.Add(this.btnAddSubProduct);
				this.btnCopySubProduct = new Button();
				this.btnCopySubProduct.Content = ImageConverter.ToImage("/img/copyproduct.png");
				this.btnCopySubProduct.Click += this.btnCopySubProduct_Click;
				this.btnCopySubProduct.Style = (base.FindResource("defaultButtonStyle") as Style);
				this.Buttons.Add(this.btnCopySubProduct);
				this.btnRemoveSubProduct = new Button();
				this.btnRemoveSubProduct.Content = ImageConverter.ToImage("/img/deleteproduct.png");
				this.btnRemoveSubProduct.Click += this.btnRemoveSubProduct_Click;
				this.btnRemoveSubProduct.Style = (base.FindResource("defaultButtonStyle") as Style);
				this.Buttons.Add(this.btnRemoveSubProduct);
				this.btnRename = new Button();
				this.btnRename.Content = ImageConverter.ToImage("/img/editproduct.png");
				this.btnRename.Click += this.btnRename_Click;
				this.btnRename.Style = (base.FindResource("defaultButtonStyle") as Style);
				this.Buttons.Add(this.btnRename);
				this.btnSetAsDefaultSubProduct = new Button();
				this.imageDefaultProduct = ImageConverter.ToImage("/img/defaultProduct.png");
				this.imageDefaultProduct.Height = 37.0;
				this.imageDefaultProduct.Width = 37.0;
				this.imageDefaultProduct.Stretch = Stretch.Uniform;
				this.imageDefaultProductClear = ImageConverter.ToImage("/img/defaultProductClear.png");
				this.imageDefaultProductClear.Height = 37.0;
				this.imageDefaultProductClear.Width = 37.0;
				this.imageDefaultProductClear.Stretch = Stretch.Uniform;
				this.btnSetAsDefaultSubProduct.Content = ((this.productManager.SelectedSubProduct == null || !this.productManager.SelectedSubProduct.IsDefaultSubProduct) ? this.imageDefaultProduct : this.imageDefaultProductClear);
				((Image)this.btnSetAsDefaultSubProduct.Content).Stretch = Stretch.Uniform;
				this.btnSetAsDefaultSubProduct.Click += this.btnSetAsDefaultSubProduct_Click;
				this.btnSetAsDefaultSubProduct.Style = (base.FindResource("defaultButtonStyle") as Style);
				this.Buttons.Add(this.btnSetAsDefaultSubProduct);
			}
			else
			{
				bool flag2 = mode == SubProductModes.Learn;
				if (flag2)
				{
					this.btnLearnSubProduct = new Button();
					Image image = ImageConverter.ToImage("/img/learn.png");
					image.Height = 40.0;
					image.Width = 40.0;
					image.Stretch = Stretch.Uniform;
					this.btnLearnSubProduct.Content = image;
					this.btnLearnSubProduct.Style = (base.FindResource("defaultButtonStyle") as Style);
					this.btnLearnSubProduct.Click += this.btnLearn_Click;
					this.Buttons.Add(this.btnLearnSubProduct);
				}
				else
				{
					bool flag3 = mode == SubProductModes.Filter;
					if (flag3)
					{
						this.btnFilter = new Button();
						Image image2 = ImageConverter.ToImage("/img/productparameter.png");
						image2.Height = 40.0;
						image2.Width = 40.0;
						image2.Stretch = Stretch.Uniform;
						this.btnFilter.Content = image2;
						this.btnFilter.Style = (base.FindResource("defaultButtonStyle") as Style);
						this.btnFilter.Click += this.btnFilter_Click;
						this.Buttons.Add(this.btnFilter);
					}
					else
					{
						bool flag4 = mode == SubProductModes.RetrainGroup;
						if (flag4)
						{
							this.btnRetrainGroup = new Button();
							Image image3 = ImageConverter.ToImage("/img/retrainGroup.png");
							image3.Height = 40.0;
							image3.Width = 40.0;
							image3.Stretch = Stretch.Uniform;
							this.btnRetrainGroup.Content = image3;
							this.btnRetrainGroup.Style = (base.FindResource("defaultButtonStyle") as Style);
							this.btnRetrainGroup.Click += this.btnRetrainGroup_Click;
							this.Buttons.Add(this.btnRetrainGroup);
						}
					}
				}
			}
		}

		// Token: 0x06001D5A RID: 7514 RVA: 0x000BF7C8 File Offset: 0x000BD9C8
		private void btnFilter_Click(object sender, RoutedEventArgs e)
		{
			this.viewModel.OpenFilter();
			Subwindow subwindow = Window.GetWindow(this) as Subwindow;
			bool flag = subwindow != null;
			if (flag)
			{
				subwindow.Close();
			}
		}

		// Token: 0x06001D5B RID: 7515 RVA: 0x000BF7FD File Offset: 0x000BD9FD
		private void btnAddSubProduct_Click(object sender, RoutedEventArgs e)
		{
			this.viewModel.AddSubProduct();
		}

		// Token: 0x06001D5C RID: 7516 RVA: 0x000BF80C File Offset: 0x000BDA0C
		private void btnCopySubProduct_Click(object sender, RoutedEventArgs e)
		{
			this.viewModel.CopySubProduct();
		}

		// Token: 0x06001D5D RID: 7517 RVA: 0x000BF81B File Offset: 0x000BDA1B
		private void btnRemoveSubProduct_Click(object sender, RoutedEventArgs e)
		{
			this.viewModel.RemoveSubProduct();
		}

		// Token: 0x06001D5E RID: 7518 RVA: 0x000BF82A File Offset: 0x000BDA2A
		private void btnRename_Click(object sender, RoutedEventArgs e)
		{
			this.viewModel.RenameSubProduct();
		}

		// Token: 0x06001D5F RID: 7519 RVA: 0x000BF83C File Offset: 0x000BDA3C
		private void btnLearn_Click(object sender, RoutedEventArgs e)
		{
			bool flag = this.viewModel.LearnSubProduct();
			if (flag)
			{
				Subwindow subwindow = Window.GetWindow(this) as Subwindow;
				bool flag2 = subwindow != null;
				if (flag2)
				{
					subwindow.Close();
				}
			}
		}

		// Token: 0x06001D60 RID: 7520 RVA: 0x000BF876 File Offset: 0x000BDA76
		private void btnRetrainGroup_Click(object sender, RoutedEventArgs e)
		{
			this.viewModel.RetrainGroup();
		}

		// Token: 0x06001D61 RID: 7521 RVA: 0x000BF885 File Offset: 0x000BDA85
		private void btnSetAsDefaultSubProduct_Click(object sender, RoutedEventArgs e)
		{
			this.viewModel.SetAsDefaultSubProduct();
		}

		// Token: 0x06001D62 RID: 7522 RVA: 0x000BF894 File Offset: 0x000BDA94
		private void listBoxItem_DoubleClick(object sender, RoutedEventArgs e)
		{
			bool flag = sender is ListBoxItem;
			if (flag)
			{
				bool flag2 = this.btnLearnSubProduct != null;
				if (flag2)
				{
					this.btnLearn_Click(null, new RoutedEventArgs());
				}
				else
				{
					bool flag3 = this.btnFilter != null;
					if (flag3)
					{
						this.btnFilter_Click(null, new RoutedEventArgs());
					}
				}
			}
		}

		// Token: 0x06001D63 RID: 7523 RVA: 0x000BF8E8 File Offset: 0x000BDAE8
		private void productManager_RetrainGroupSucceeded()
		{
			bool flag = base.Dispatcher != null;
			if (flag)
			{
				base.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(delegate()
				{
					Subwindow subwindow = Window.GetWindow(this) as Subwindow;
					bool flag2 = subwindow != null;
					if (flag2)
					{
						subwindow.Close();
					}
				}));
			}
		}

		// Token: 0x06001D64 RID: 7524 RVA: 0x000BF920 File Offset: 0x000BDB20
		private void CtrlSubProduct_Unloaded(object sender, RoutedEventArgs e)
		{
			this.productManager.RetrainGroupSucceeded -= this.productManager_RetrainGroupSucceeded;
			this.productManager.PropertyChanged -= this.productManager_PropertyChanged;
			bool flag = this.setFooterMainWindow != null;
			if (flag)
			{
				this.setFooterMainWindow.UnSetFooterDeactivation();
			}
		}

		// Token: 0x06001D65 RID: 7525 RVA: 0x000BF978 File Offset: 0x000BDB78
		private void productManager_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			bool flag = e.PropertyName == "SelectedSubProduct" && this.productManager.SelectedSubProduct != null;
			if (flag)
			{
				bool isDefaultSubProduct = this.productManager.SelectedSubProduct.IsDefaultSubProduct;
				if (isDefaultSubProduct)
				{
					bool flag2 = this.btnSetAsDefaultSubProduct != null && this.imageDefaultProductClear != null;
					if (flag2)
					{
						this.btnSetAsDefaultSubProduct.Content = this.imageDefaultProductClear;
					}
				}
				else
				{
					bool flag3 = this.btnSetAsDefaultSubProduct != null && this.imageDefaultProduct != null;
					if (flag3)
					{
						this.btnSetAsDefaultSubProduct.Content = this.imageDefaultProduct;
					}
				}
			}
		}

		// Token: 0x06001D66 RID: 7526 RVA: 0x000BFA20 File Offset: 0x000BDC20
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/productchange/ctrlsubproduct.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x06001D67 RID: 7527 RVA: 0x000BFA58 File Offset: 0x000BDC58
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				((CtrlSubProduct)target).Unloaded += this.CtrlSubProduct_Unloaded;
				break;
			case 2:
				this.tbCurrentAppl = (TextBlock)target;
				break;
			case 3:
				this.lstSubProducts = (ListBox)target;
				break;
			default:
				this._contentLoaded = true;
				break;
			}
		}

		// Token: 0x06001D68 RID: 7528 RVA: 0x000BFAC0 File Offset: 0x000BDCC0
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

		// Token: 0x0400135C RID: 4956
		private SubProductViewModel viewModel;

		// Token: 0x0400135D RID: 4957
		private Button btnRemoveSubProduct;

		// Token: 0x0400135E RID: 4958
		private Button btnAddSubProduct;

		// Token: 0x0400135F RID: 4959
		private Button btnCopySubProduct;

		// Token: 0x04001360 RID: 4960
		private Button btnLearnSubProduct;

		// Token: 0x04001361 RID: 4961
		private Button btnFilter;

		// Token: 0x04001362 RID: 4962
		private Button btnRetrainGroup;

		// Token: 0x04001363 RID: 4963
		private Button btnRename;

		// Token: 0x04001364 RID: 4964
		private Button btnSetAsDefaultSubProduct;

		// Token: 0x04001365 RID: 4965
		private Image imageDefaultProduct;

		// Token: 0x04001366 RID: 4966
		private Image imageDefaultProductClear;

		// Token: 0x04001367 RID: 4967
		private ProductManager productManager;

		// Token: 0x04001369 RID: 4969
		private SetFooterMainWindow setFooterMainWindow;

		// Token: 0x0400136A RID: 4970
		internal TextBlock tbCurrentAppl;

		// Token: 0x0400136B RID: 4971
		internal ListBox lstSubProducts;

		// Token: 0x0400136C RID: 4972
		private bool _contentLoaded;
	}
}
