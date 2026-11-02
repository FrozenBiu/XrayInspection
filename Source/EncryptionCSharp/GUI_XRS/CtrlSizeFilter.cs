using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;
using DatabaseConnection;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x02000094 RID: 148
	public class CtrlSizeFilter : UserControl, IComponentConnector
	{
		// Token: 0x1700048E RID: 1166
		// (get) Token: 0x06000EF9 RID: 3833 RVA: 0x0005A192 File Offset: 0x00058392
		// (set) Token: 0x06000EFA RID: 3834 RVA: 0x0005A19A File Offset: 0x0005839A
		public SizeFilterViewModel sizeFilterViewModel { get; set; }

		// Token: 0x1700048F RID: 1167
		// (get) Token: 0x06000EFB RID: 3835 RVA: 0x0005A1A3 File Offset: 0x000583A3
		// (set) Token: 0x06000EFC RID: 3836 RVA: 0x0005A1AB File Offset: 0x000583AB
		public UIElement ControlUIElement { get; set; }

		// Token: 0x06000EFD RID: 3837 RVA: 0x0005A1B4 File Offset: 0x000583B4
		public CtrlSizeFilter(ParameterHandling parHandling, ImageDataTransfer imageTransfer, DBConnection dbConnection, Filter filter, int imageGroup, ProductManager productManager, MachineState machineState, StoreImageData imageArchiver)
		{
			this.InitializeComponent();
			this.imageTransfer = imageTransfer;
			this.machineState = machineState;
			this.imageArchiver = imageArchiver;
			this.sizeFilterViewModel = new SizeFilterViewModel(parHandling, dbConnection, imageGroup, productManager, imageArchiver, machineState);
			base.DataContext = this.sizeFilterViewModel;
			this.timer = new Timer(new TimerCallback(this.LoadImages));
			this.timer.Change(100, -1);
			imageTransfer.FrameArrived += this.imageTransfer_FrameArrived;
			this.sizeFilterViewModel.PropertyChanged += this.SizeFilterViewModel_PropertyChanged;
			this.sizeFilterViewModel.ShowNext = true;
		}

		// Token: 0x06000EFE RID: 3838 RVA: 0x0005A268 File Offset: 0x00058468
		private void LoadImages(object obj)
		{
			this.sizeFilterViewModel.CurrentImage = HistoryHelper.GetLatestImage(0, this.imageArchiver);
			bool flag = this.sizeFilterViewModel.CurrentImage != null;
			if (flag)
			{
				this.imageTransfer.SendImage(this.sizeFilterViewModel.CurrentImage);
			}
			else
			{
				base.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
				{
					Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtNoValidImage"), DialogStyle.Error);
				}));
			}
		}

		// Token: 0x06000EFF RID: 3839 RVA: 0x0005A2EC File Offset: 0x000584EC
		private void imageTransfer_FrameArrived(object sender, ReceiveEventArgs e)
		{
			bool flag = !this.sizeFilterViewModel.HighSpeedMachineActive || (this.sizeFilterViewModel.HighSpeedMachineActive && (this.sizeFilterViewModel.ShowNext || e.Image.HasErrors()));
			if (flag)
			{
				bool flag2 = this.machineState.State != MachineStates.Reseasoning && this.machineState.State != MachineStates.XrayOnCalibrationNotDone && e.Image.Type != ImageType.TestcardSimulation;
				if (flag2)
				{
					XRSImage currentImage = e.Image.DeepCopy();
					this.sizeFilterViewModel.CurrentImage = currentImage;
					this.imagebox.ViewXRSImage(this.sizeFilterViewModel.CurrentImage);
					this.sizeFilterViewModel.InitializeValues(true);
					base.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
					{
						bool flag3 = this.sizeFilterViewModel.CurrentWidth < this.sizeFilterViewModel.MinWidth || this.sizeFilterViewModel.CurrentWidth > this.sizeFilterViewModel.MaxWidth;
						if (flag3)
						{
							this.tbCurrentWidth.Foreground = Brushes.Red;
							this.tbCurrentWidth.FontWeight = FontWeights.SemiBold;
						}
						else
						{
							this.tbCurrentWidth.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#5E5E5E");
							this.tbCurrentWidth.FontWeight = FontWeights.Normal;
						}
						bool flag4 = this.sizeFilterViewModel.CurrentHeight < this.sizeFilterViewModel.MinHeight || this.sizeFilterViewModel.CurrentHeight > this.sizeFilterViewModel.MaxHeight;
						if (flag4)
						{
							this.tbCurrentHeight.Foreground = Brushes.Red;
							this.tbCurrentHeight.FontWeight = FontWeights.SemiBold;
						}
						else
						{
							this.tbCurrentHeight.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#5E5E5E");
							this.tbCurrentHeight.FontWeight = FontWeights.Normal;
						}
					}));
					this.sizeFilterViewModel.ShowNext = false;
				}
			}
		}

		// Token: 0x06000F00 RID: 3840 RVA: 0x0005A3D8 File Offset: 0x000585D8
		private void SizeFilterViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			bool flag = e.PropertyName == "SendImage";
			if (flag)
			{
				this.imageTransfer.SendImage(this.sizeFilterViewModel.CurrentImage);
				this.sizeFilterViewModel.SendImage = false;
			}
			bool flag2 = e.PropertyName == "ImageBoxHeight";
			if (flag2)
			{
				this.imagebox.SetAutoZoomAndBrightness();
			}
		}

		// Token: 0x06000F01 RID: 3841 RVA: 0x0005A444 File Offset: 0x00058644
		private void CtrlSizeFilter_Unloaded(object sender, RoutedEventArgs e)
		{
			this.sizeFilterViewModel.Cleanup();
			bool flag = this.timer != null;
			if (flag)
			{
				this.timer.Dispose();
			}
			this.sizeFilterViewModel.CurrentImage = null;
			this.imageTransfer.FrameArrived -= this.imageTransfer_FrameArrived;
			this.sizeFilterViewModel.PropertyChanged -= this.SizeFilterViewModel_PropertyChanged;
			BindingOperations.ClearAllBindings(this);
			this.btnHighSpeedImage.ClearValue(UIElement.IsEnabledProperty);
			base.DataContext = null;
		}

		// Token: 0x06000F02 RID: 3842 RVA: 0x0005A4D4 File Offset: 0x000586D4
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/sizefilter/ctrlsizefilter.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x06000F03 RID: 3843 RVA: 0x0005A50C File Offset: 0x0005870C
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				((CtrlSizeFilter)target).Unloaded += this.CtrlSizeFilter_Unloaded;
				break;
			case 2:
				this.imagebox = (XRSImageBox)target;
				break;
			case 3:
				this.btnHighSpeedImage = (Button)target;
				break;
			case 4:
				this.toggleBtn = (ToggleButton)target;
				break;
			case 5:
				this.tbCurrentHeight = (TextBox)target;
				break;
			case 6:
				this.tbCurrentWidth = (TextBox)target;
				break;
			default:
				this._contentLoaded = true;
				break;
			}
		}

		// Token: 0x04000A06 RID: 2566
		private ImageDataTransfer imageTransfer;

		// Token: 0x04000A07 RID: 2567
		private Timer timer;

		// Token: 0x04000A08 RID: 2568
		private MachineState machineState;

		// Token: 0x04000A09 RID: 2569
		private StoreImageData imageArchiver;

		// Token: 0x04000A0B RID: 2571
		internal XRSImageBox imagebox;

		// Token: 0x04000A0C RID: 2572
		internal Button btnHighSpeedImage;

		// Token: 0x04000A0D RID: 2573
		internal ToggleButton toggleBtn;

		// Token: 0x04000A0E RID: 2574
		internal TextBox tbCurrentHeight;

		// Token: 0x04000A0F RID: 2575
		internal TextBox tbCurrentWidth;

		// Token: 0x04000A10 RID: 2576
		private bool _contentLoaded;
	}
}
