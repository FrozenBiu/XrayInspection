using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Threading;
using Definitions;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x0200021E RID: 542
	public class CtrlChangeEdgePixel : UserControl, IComponentConnector
	{
		// Token: 0x17000B28 RID: 2856
		// (get) Token: 0x060028D9 RID: 10457 RVA: 0x0010FE5B File Offset: 0x0010E05B
		// (set) Token: 0x060028DA RID: 10458 RVA: 0x0010FE63 File Offset: 0x0010E063
		public UIElement ControlUIElement { get; set; }

		// Token: 0x060028DB RID: 10459 RVA: 0x0010FE6C File Offset: 0x0010E06C
		public CtrlChangeEdgePixel(ParameterHandling parHandling, ImageDataTransfer imageTransfer, Filter filter, int imageGroup, ProductManager productManager, MachineState machineState, StoreImageData imageArchiver)
		{
			this.InitializeComponent();
			this.parHandling = parHandling;
			this.filter = filter;
			this.imageTransfer = imageTransfer;
			this.imageGroup = imageGroup;
			this.productManager = productManager;
			this.machineState = machineState;
			this.imageArchiver = imageArchiver;
			base.DataContext = filter;
			imageTransfer.FrameArrived += this.imageTransfer_FrameArrived;
			filter.ChangePixelCommandExecuted += this.filter_ChangePixelCommandExecuted;
			machineState.PropertyChanged += this.machineState_PropertyChanged;
			this.timer = new Timer(new TimerCallback(this.LoadImages));
			this.timer.Change(100, -1);
			parHandling.EdgeFilterMode = true;
			this.AddControlButtons();
			this.InitializeEdgeParameters();
			this.SetGridsVisibility();
		}

		// Token: 0x060028DC RID: 10460 RVA: 0x0010FF44 File Offset: 0x0010E144
		private void InitializeEdgeParameters()
		{
			string str = (this.imageGroup > 0) ? this.imageGroup.ToString() : "";
			int.TryParse(this.parHandling.ReadPar(Definition.filterMainPath + str + Filter.leftPixelSubPath), out this.nOldLeftPixel);
			int.TryParse(this.parHandling.ReadPar(Definition.filterMainPath + str + Filter.rightPixelSubPath), out this.nOldRightPixel);
			int.TryParse(this.parHandling.ReadPar(Definition.filterMainPath + str + Filter.topPixelSubPath), out this.nOldTopPixel);
			int.TryParse(this.parHandling.ReadPar(Definition.filterMainPath + str + Filter.bottomPixelSubPath), out this.nOldBottomPixel);
			int.TryParse(this.parHandling.ReadPar(Definition.filterMainPath + str + Filter.edgeThresholdSubPath), out this.oldEdgeThreshold);
		}

		// Token: 0x060028DD RID: 10461 RVA: 0x00110034 File Offset: 0x0010E234
		private void AddControlButtons()
		{
			this.ctrlEdgeSubButtons = new CtrlEdgeSubButtons(this.parHandling, this.filter.AccessManager, this.imageGroup);
			this.ctrlEdgeSubButtons.EdgePropertyChanged += this.ctrlEdgeSubButtons_EdgePropertyChanged;
			this.ControlUIElement = this.ctrlEdgeSubButtons;
		}

		// Token: 0x060028DE RID: 10462 RVA: 0x0011008C File Offset: 0x0010E28C
		private void ctrlEdgeSubButtons_EdgePropertyChanged(string propertyName)
		{
			bool flag = this.filter.EdgeCurrentImage != null && this.filter.EdgeCurrentImage.Height > 0 && this.filter.EdgeCurrentImage.Width > 0;
			if (flag)
			{
				bool flag2 = this.machineState.State != MachineStates.XrayOnCalibrationNotDone && this.machineState.State != MachineStates.Reseasoning;
				if (flag2)
				{
					this.imageTransfer.SendImage(this.filter.EdgeCurrentImage);
				}
				else
				{
					Dialog.Show(LocalizationManager.Localize("txtInfo"), LocalizationManager.Localize("txtWaitingForCalibration"), DialogStyle.Info, DialogButtons.OK);
					this.sendNeeded = true;
				}
			}
			bool flag3 = propertyName == "SelectedEdgeMode";
			if (flag3)
			{
				this.SetGridsVisibility();
			}
		}

		// Token: 0x060028DF RID: 10463 RVA: 0x00110154 File Offset: 0x0010E354
		private void filter_ChangePixelCommandExecuted()
		{
			bool flag = this.filter.EdgeCurrentImage != null && this.filter.EdgeCurrentImage.Height > 0 && this.filter.EdgeCurrentImage.Width > 0;
			if (flag)
			{
				bool flag2 = this.machineState.State != MachineStates.XrayOnCalibrationNotDone && this.machineState.State != MachineStates.Reseasoning;
				if (flag2)
				{
					this.imageTransfer.SendImage(this.filter.EdgeCurrentImage);
				}
				else
				{
					Dialog.Show(LocalizationManager.Localize("txtInfo"), LocalizationManager.Localize("txtWaitingForCalibration"), DialogStyle.Info, DialogButtons.OK);
					this.sendNeeded = true;
				}
			}
		}

		// Token: 0x060028E0 RID: 10464 RVA: 0x00110204 File Offset: 0x0010E404
		private void LoadImages(object obj)
		{
			this.filter.EdgeCurrentImage = HistoryHelper.GetLatestImage((!this.productManager.CurrentLoadedProduct.IsRegionProduct) ? this.imageGroup : 0, this.imageArchiver);
			bool flag = this.filter.EdgeCurrentImage != null;
			if (flag)
			{
				string str = (this.imageGroup != 0) ? this.imageGroup.ToString() : "";
				this.filter.InitializeNPixels();
				this.parHandling.WriteValue(Definition.filterMainPath + str + Filter.edgeDebugSubPath, "1", false);
				this.imageTransfer.SendImage(this.filter.EdgeCurrentImage);
			}
			else
			{
				base.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
				{
					Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtNoValidImage"), DialogStyle.Error);
				}));
			}
		}

		// Token: 0x060028E1 RID: 10465 RVA: 0x001102EC File Offset: 0x0010E4EC
		private void imageTransfer_FrameArrived(object sender, ReceiveEventArgs e)
		{
			bool flag = this.machineState.State != MachineStates.Reseasoning && this.machineState.State != MachineStates.XrayOnCalibrationNotDone && e.Image.Type != ImageType.TestcardSimulation;
			if (flag)
			{
				bool flag2 = e.Image.Type == ImageType.Debug;
				if (flag2)
				{
					XRSImage xrsimage = e.Image.DeepCopy();
					xrsimage.Weight = 0f;
					xrsimage.ObjectCount = 0;
					xrsimage.ContaminationCount = 0;
					this.imagebox.ViewXRSImage(xrsimage);
				}
				else
				{
					bool flag3 = e.Image.Type == ImageType.Normal;
					if (flag3)
					{
						this.filter.EdgeCurrentImage = e.Image;
					}
					else
					{
						bool flag4 = e.Image.Type == ImageType.Demo && (int)e.Image.Group != this.imageGroup;
						if (flag4)
						{
							bool flag5 = !this.productManager.CurrentLoadedProduct.IsRegionProduct || e.Image.Group != 0;
							if (flag5)
							{
								base.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(delegate()
								{
									Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtNoValidImage"), DialogStyle.Error);
								}));
							}
						}
					}
				}
			}
		}

		// Token: 0x060028E2 RID: 10466 RVA: 0x00110434 File Offset: 0x0010E634
		private void CtrlChangeEdgePixel_Unloaded(object sender, RoutedEventArgs e)
		{
			string str = (this.imageGroup > 0) ? this.imageGroup.ToString() : "";
			this.parHandling.WriteValue(Definition.filterMainPath + str + Filter.edgeDebugSubPath, "0", false);
			this.imageTransfer.FrameArrived -= this.imageTransfer_FrameArrived;
			this.filter.ChangePixelCommandExecuted -= this.filter_ChangePixelCommandExecuted;
			this.machineState.PropertyChanged -= this.machineState_PropertyChanged;
			this.parHandling.EdgeFilterMode = false;
			bool flag = this.ctrlEdgeSubButtons != null;
			if (flag)
			{
				this.ctrlEdgeSubButtons.EdgePropertyChanged -= this.ctrlEdgeSubButtons_EdgePropertyChanged;
			}
			this.LogNPixelChange();
			bool flag2 = this.timer != null;
			if (flag2)
			{
				this.timer.Dispose();
			}
			this.filter.EdgeCurrentImage = null;
			BindingOperations.ClearAllBindings(this);
			base.DataContext = null;
		}

		// Token: 0x060028E3 RID: 10467 RVA: 0x00110538 File Offset: 0x0010E738
		private void Slider_PreviewMouseUp(object sender, MouseButtonEventArgs e)
		{
			bool flag = this.filter.EdgeCurrentImage != null && this.filter.EdgeCurrentImage.Height > 0 && this.filter.EdgeCurrentImage.Width > 0;
			if (flag)
			{
				bool flag2 = this.machineState.State != MachineStates.XrayOnCalibrationNotDone && this.machineState.State != MachineStates.Reseasoning;
				if (flag2)
				{
					this.imageTransfer.SendImage(this.filter.EdgeCurrentImage);
				}
				else
				{
					Dialog.Show(LocalizationManager.Localize("txtInfo"), LocalizationManager.Localize("txtWaitingForCalibration"), DialogStyle.Info, DialogButtons.OK);
					this.sendNeeded = true;
				}
			}
		}

		// Token: 0x060028E4 RID: 10468 RVA: 0x001105E8 File Offset: 0x0010E7E8
		private void LogNPixelChange()
		{
			string str = (this.imageGroup > 0) ? this.imageGroup.ToString() : "";
			int num;
			int.TryParse(this.parHandling.ReadPar(Definition.filterMainPath + str + Filter.leftPixelSubPath), out num);
			int num2;
			int.TryParse(this.parHandling.ReadPar(Definition.filterMainPath + str + Filter.rightPixelSubPath), out num2);
			int num3;
			int.TryParse(this.parHandling.ReadPar(Definition.filterMainPath + str + Filter.topPixelSubPath), out num3);
			int num4;
			int.TryParse(this.parHandling.ReadPar(Definition.filterMainPath + str + Filter.bottomPixelSubPath), out num4);
			int num5;
			int.TryParse(this.parHandling.ReadPar(Definition.filterMainPath + str + Filter.edgeThresholdSubPath), out num5);
			bool flag = this.imageGroup <= 0;
			if (flag)
			{
				string text = ": P" + this.productManager.CurrentLoadedProduct.ProductNumber + " " + this.productManager.CurrentLoadedProduct.ProductName;
			}
			else
			{
				ProductData[] array = (from product in this.productManager.CurrentLoadedProduct.SubProducts
				where product.TableName.EndsWith(this.imageGroup.ToString())
				select product).ToArray<ProductData>();
				bool flag2 = array.Length != 0;
				if (flag2)
				{
					string text = string.Concat(new string[]
					{
						": P",
						this.productManager.CurrentLoadedProduct.ProductNumber,
						"_",
						this.imageGroup.ToString(),
						" ",
						array[0].ProductName
					});
				}
			}
			bool flag3 = this.nOldLeftPixel != num;
			if (flag3)
			{
				LogbookManager.AddEntry(SSTEntryCode.EdgeParameterChanged, 0, this.nOldLeftPixel.ToString(), num.ToString());
			}
			bool flag4 = this.nOldRightPixel != num2;
			if (flag4)
			{
				LogbookManager.AddEntry(SSTEntryCode.EdgeParameterChanged, 1, this.nOldRightPixel.ToString(), num2.ToString());
			}
			bool flag5 = this.nOldTopPixel != num3;
			if (flag5)
			{
				LogbookManager.AddEntry(SSTEntryCode.EdgeParameterChanged, 2, this.nOldTopPixel.ToString(), num3.ToString());
			}
			bool flag6 = this.nOldBottomPixel != num4;
			if (flag6)
			{
				LogbookManager.AddEntry(SSTEntryCode.EdgeParameterChanged, 3, this.nOldBottomPixel.ToString(), num4.ToString());
			}
			bool flag7 = this.oldEdgeThreshold != num5;
			if (flag7)
			{
				LogbookManager.AddEntry(SSTEntryCode.EdgeParameterChanged, 4, this.oldEdgeThreshold.ToString(), num5.ToString());
			}
		}

		// Token: 0x060028E5 RID: 10469 RVA: 0x00110894 File Offset: 0x0010EA94
		private void machineState_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			bool flag = e.PropertyName == "State" && this.machineState.State != MachineStates.Reseasoning && this.machineState.State != MachineStates.XrayOnCalibrationNotDone;
			if (flag)
			{
				bool flag2 = this.sendNeeded && this.filter.EdgeCurrentImage != null && this.filter.EdgeCurrentImage.Height > 0 && this.filter.EdgeCurrentImage.Width > 0;
				if (flag2)
				{
					this.imageTransfer.SendImage(this.filter.EdgeCurrentImage);
				}
			}
		}

		// Token: 0x060028E6 RID: 10470 RVA: 0x00110938 File Offset: 0x0010EB38
		private void SetGridsVisibility()
		{
			bool flag = this.ctrlEdgeSubButtons != null;
			if (flag)
			{
				bool flag2 = this.ctrlEdgeSubButtons.EdgeMode != EdgeFilterMode.RemoveElliptic;
				if (flag2)
				{
					this.grdElliptic.Visibility = Visibility.Collapsed;
					this.grdNoElliptic.Visibility = Visibility.Visible;
				}
				else
				{
					this.grdNoElliptic.Visibility = Visibility.Collapsed;
					this.grdElliptic.Visibility = Visibility.Visible;
				}
			}
		}

		// Token: 0x060028E7 RID: 10471 RVA: 0x001109A4 File Offset: 0x0010EBA4
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/filter/ctrlchangeedgepixel.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x060028E8 RID: 10472 RVA: 0x001109DC File Offset: 0x0010EBDC
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				((CtrlChangeEdgePixel)target).Unloaded += this.CtrlChangeEdgePixel_Unloaded;
				break;
			case 2:
				this.imagebox = (XRSImageBox)target;
				break;
			case 3:
				this.grdNoElliptic = (Grid)target;
				break;
			case 4:
				((Slider)target).PreviewMouseUp += this.Slider_PreviewMouseUp;
				break;
			case 5:
				((Slider)target).PreviewMouseUp += this.Slider_PreviewMouseUp;
				break;
			case 6:
				((Slider)target).PreviewMouseUp += this.Slider_PreviewMouseUp;
				break;
			case 7:
				((Slider)target).PreviewMouseUp += this.Slider_PreviewMouseUp;
				break;
			case 8:
				this.grdElliptic = (Grid)target;
				break;
			case 9:
				((Slider)target).PreviewMouseUp += this.Slider_PreviewMouseUp;
				break;
			case 10:
				((Slider)target).PreviewMouseUp += this.Slider_PreviewMouseUp;
				break;
			default:
				this._contentLoaded = true;
				break;
			}
		}

		// Token: 0x0400196C RID: 6508
		private ParameterHandling parHandling;

		// Token: 0x0400196D RID: 6509
		private Filter filter;

		// Token: 0x0400196E RID: 6510
		private ImageDataTransfer imageTransfer;

		// Token: 0x0400196F RID: 6511
		private Timer timer;

		// Token: 0x04001970 RID: 6512
		private int nOldLeftPixel;

		// Token: 0x04001971 RID: 6513
		private int nOldRightPixel;

		// Token: 0x04001972 RID: 6514
		private int nOldTopPixel;

		// Token: 0x04001973 RID: 6515
		private int nOldBottomPixel;

		// Token: 0x04001974 RID: 6516
		private int oldEdgeThreshold;

		// Token: 0x04001975 RID: 6517
		private CtrlEdgeSubButtons ctrlEdgeSubButtons;

		// Token: 0x04001976 RID: 6518
		private int imageGroup;

		// Token: 0x04001977 RID: 6519
		private ProductManager productManager;

		// Token: 0x04001978 RID: 6520
		private MachineState machineState;

		// Token: 0x04001979 RID: 6521
		private bool sendNeeded;

		// Token: 0x0400197A RID: 6522
		private StoreImageData imageArchiver;

		// Token: 0x0400197C RID: 6524
		internal XRSImageBox imagebox;

		// Token: 0x0400197D RID: 6525
		internal Grid grdNoElliptic;

		// Token: 0x0400197E RID: 6526
		internal Grid grdElliptic;

		// Token: 0x0400197F RID: 6527
		private bool _contentLoaded;
	}
}
