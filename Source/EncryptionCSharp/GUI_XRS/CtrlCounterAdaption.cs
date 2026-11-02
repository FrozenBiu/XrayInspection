using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;
using DatabaseConnection;
using Definitions;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x02000234 RID: 564
	public class CtrlCounterAdaption : UserControl, IComponentConnector
	{
		// Token: 0x17000BB5 RID: 2997
		// (get) Token: 0x06002AE1 RID: 10977 RVA: 0x0011CE55 File Offset: 0x0011B055
		// (set) Token: 0x06002AE2 RID: 10978 RVA: 0x0011CE5D File Offset: 0x0011B05D
		public Grid PanelSubButtons { get; set; }

		// Token: 0x17000BB6 RID: 2998
		// (get) Token: 0x06002AE3 RID: 10979 RVA: 0x0011CE66 File Offset: 0x0011B066
		// (set) Token: 0x06002AE4 RID: 10980 RVA: 0x0011CE6E File Offset: 0x0011B06E
		public bool HighSpeedMode { get; set; }

		// Token: 0x06002AE5 RID: 10981 RVA: 0x0011CE78 File Offset: 0x0011B078
		public CtrlCounterAdaption(ParameterHandling parHandling, ImageDataTransfer imageTransfer, int imageGroup, ProductManager productManager, StoreImageData imageArchiver, DBConnection dbConnection, MachineState machineState)
		{
			this.InitializeComponent();
			this.parHandling = parHandling;
			this.imageTransfer = imageTransfer;
			this.imageGroup = imageGroup;
			this.productManager = productManager;
			this.imageArchiver = imageArchiver;
			this.viewModel = new CounterAdaptionViewModel(machineState, dbConnection);
			base.DataContext = this.viewModel;
			imageTransfer.FrameArrived += this.imageTransfer_FrameArrived;
			bool flag = false;
			dbConnection.GetHighThroughputMachine(ref flag);
			string text = "";
			dbConnection.GetParameterValue(Definition.highThroughputEnabled, ref text);
			bool flag2 = flag && text.Equals("1");
			if (flag2)
			{
				this.HighSpeedMode = true;
			}
			else
			{
				this.HighSpeedMode = false;
			}
			this.viewModel.ShowNext = false;
			this.timer = new Timer(new TimerCallback(this.LoadImages));
			this.timer.Change(100, -1);
			this.InitializeButtonsPanel();
		}

		// Token: 0x06002AE6 RID: 10982 RVA: 0x0011CF70 File Offset: 0x0011B170
		private void InitializeButtonsPanel()
		{
			this.dockpanel = new DockPanel();
			this.dockpanel.HorizontalAlignment = HorizontalAlignment.Stretch;
			this.dockpanel.VerticalAlignment = VerticalAlignment.Stretch;
			Grid.SetColumn(this.dockpanel, 4);
			bool highSpeedMode = this.HighSpeedMode;
			if (highSpeedMode)
			{
				Binding item = new Binding("machineState.State");
				Binding item2 = new Binding("DemoMode");
				Binding binding = new Binding();
				binding.Path = new PropertyPath("IsEnabled", new object[0]);
				binding.Converter = new BoolToVisibilityHiddenConverter();
				Binding binding2 = new Binding();
				binding2.Path = new PropertyPath("IsEnabled", new object[0]);
				binding2.Converter = new BoolToVisibilityInvertedHiddenConverter();
				this.btnHighSpeedMode = new Button();
				Image image = ImageConverter.ToImage("../img/objectxriimage.png");
				image.SetBinding(UIElement.VisibilityProperty, binding);
				image.Height = 40.0;
				image.Width = 40.0;
				image.Stretch = Stretch.Uniform;
				Image image2 = ImageConverter.ToImage("/img/objectxriimage_disabled.png");
				image2.Stretch = Stretch.Uniform;
				image2.Height = 40.0;
				image2.Width = 40.0;
				image2.SetBinding(UIElement.VisibilityProperty, binding2);
				Grid grid = new Grid();
				grid.Children.Add(image);
				grid.Children.Add(image2);
				this.btnHighSpeedMode.Content = grid;
				this.btnHighSpeedMode.Click += this.HighSpeedModeCommand;
				this.btnHighSpeedMode.Style = (base.FindResource("defaultButtonStyleActive") as Style);
				Grid.SetColumn(this.btnHighSpeedMode, 3);
				this.btnHighSpeedMode.Margin = new Thickness(0.0, 0.0, 25.0, 0.0);
				MultiBinding multiBinding = new MultiBinding();
				multiBinding.Converter = new AutolearnMachineStatesBoolConverter();
				multiBinding.Bindings.Add(item);
				multiBinding.Bindings.Add(item2);
				multiBinding.NotifyOnSourceUpdated = true;
				this.btnHighSpeedMode.SetBinding(UIElement.IsEnabledProperty, multiBinding);
				this.btnHighSpeedMode.DataContext = base.DataContext;
				image.DataContext = this.btnHighSpeedMode;
				image2.DataContext = this.btnHighSpeedMode;
				this.dockpanel.Children.Add(this.btnHighSpeedMode);
				DockPanel.SetDock(this.btnHighSpeedMode, Dock.Left);
			}
		}

		// Token: 0x06002AE7 RID: 10983 RVA: 0x0011D208 File Offset: 0x0011B408
		private void HighSpeedModeCommand(object sender, RoutedEventArgs e)
		{
			bool showNext = this.viewModel.ShowNext;
			if (showNext)
			{
				this.viewModel.ShowNext = false;
			}
			else
			{
				this.viewModel.ShowNext = true;
			}
		}

		// Token: 0x06002AE8 RID: 10984 RVA: 0x0011D248 File Offset: 0x0011B448
		private void LoadImages(object obj)
		{
			string str = (this.imageGroup > 0) ? this.imageGroup.ToString() : "";
			this.currentImage = HistoryHelper.GetLatestImage((!this.productManager.CurrentLoadedProduct.IsRegionProduct) ? this.imageGroup : 0, this.imageArchiver);
			bool flag = this.currentImage != null;
			if (flag)
			{
				this.parHandling.WriteValue(Definition.filterMainPath + str + ObjectCounter.debugSubPath, "1", false);
				this.imageTransfer.SendImage(this.currentImage);
			}
			else
			{
				base.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
				{
					Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtNoValidImage"), DialogStyle.Error);
				}));
			}
		}

		// Token: 0x06002AE9 RID: 10985 RVA: 0x0011D314 File Offset: 0x0011B514
		private void imageTransfer_FrameArrived(object sender, ReceiveEventArgs e)
		{
			bool flag = !this.HighSpeedMode || (this.HighSpeedMode && this.viewModel.ShowNext);
			if (flag)
			{
				bool flag2 = e.Image.Type == ImageType.Normal || e.Image.Type == ImageType.Demo;
				if (flag2)
				{
					bool flag3 = (!this.productManager.CurrentLoadedProduct.IsRegionProduct && this.imageGroup == (int)e.Image.Group) || (this.productManager.CurrentLoadedProduct.IsRegionProduct && e.Image.Group == 0);
					if (flag3)
					{
						XRSImage xrsimage = e.Image.DeepCopy();
						this.productManager.PrepareRegionImage(xrsimage, this.imageGroup);
						this.imageboxOriginal.ViewXRSImage(xrsimage);
						this.viewModel.ShowNext = false;
					}
				}
				bool flag4 = e.Image.Type == ImageType.Debug;
				if (flag4)
				{
					this.imageboxDebug.ViewXRSImage(e.Image);
				}
			}
		}

		// Token: 0x06002AEA RID: 10986 RVA: 0x0011D428 File Offset: 0x0011B628
		private void CtrlCounterAdaption_Unloaded(object sender, RoutedEventArgs e)
		{
			string str = (this.imageGroup > 0) ? this.imageGroup.ToString() : "";
			bool flag = this.timer != null;
			if (flag)
			{
				this.timer.Dispose();
			}
			this.viewModel.CleanUp();
			this.parHandling.WriteValue(Definition.filterMainPath + str + ObjectCounter.debugSubPath, "0", false);
			this.imageTransfer.FrameArrived -= this.imageTransfer_FrameArrived;
			bool flag2 = this.btnHighSpeedMode != null;
			if (flag2)
			{
				this.btnHighSpeedMode.Click -= this.HighSpeedModeCommand;
			}
			BindingOperations.ClearAllBindings(this);
			base.DataContext = null;
			this.currentImage = null;
		}

		// Token: 0x06002AEB RID: 10987 RVA: 0x0011D4EC File Offset: 0x0011B6EC
		private void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
			Subwindow subwindow = Window.GetWindow(this) as Subwindow;
			subwindow.buttonGrid.Children.Remove(subwindow.buttonPanel);
			subwindow.buttonGrid.Children.Add(this.dockpanel);
		}

		// Token: 0x06002AEC RID: 10988 RVA: 0x0011D534 File Offset: 0x0011B734
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/objectcounter/ctrlcounteradaption.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x06002AED RID: 10989 RVA: 0x0011D56C File Offset: 0x0011B76C
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				((CtrlCounterAdaption)target).Unloaded += this.CtrlCounterAdaption_Unloaded;
				((CtrlCounterAdaption)target).Loaded += this.UserControl_Loaded;
				break;
			case 2:
				this.imageboxOriginal = (XRSImageBox)target;
				break;
			case 3:
				this.imageboxDebug = (XRSImageBox)target;
				break;
			default:
				this._contentLoaded = true;
				break;
			}
		}

		// Token: 0x04001AC7 RID: 6855
		private ParameterHandling parHandling;

		// Token: 0x04001AC8 RID: 6856
		private ImageDataTransfer imageTransfer;

		// Token: 0x04001AC9 RID: 6857
		private XRSImage currentImage;

		// Token: 0x04001ACA RID: 6858
		private Timer timer;

		// Token: 0x04001ACB RID: 6859
		private int imageGroup;

		// Token: 0x04001ACC RID: 6860
		private ProductManager productManager;

		// Token: 0x04001ACD RID: 6861
		private StoreImageData imageArchiver;

		// Token: 0x04001ACE RID: 6862
		private Button btnHighSpeedMode;

		// Token: 0x04001AD1 RID: 6865
		private CounterAdaptionViewModel viewModel;

		// Token: 0x04001AD2 RID: 6866
		private DockPanel dockpanel;

		// Token: 0x04001AD3 RID: 6867
		internal XRSImageBox imageboxOriginal;

		// Token: 0x04001AD4 RID: 6868
		internal XRSImageBox imageboxDebug;

		// Token: 0x04001AD5 RID: 6869
		private bool _contentLoaded;
	}
}
