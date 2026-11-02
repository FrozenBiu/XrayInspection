using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Timers;
using System.Windows;
using System.Windows.Threading;
using Common;
using DatabaseConnection;
using Definitions;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x0200012A RID: 298
	public class LearnCounterViewModel : INotifyPropertyChanged
	{
		// Token: 0x14000093 RID: 147
		// (add) Token: 0x06001CC9 RID: 7369 RVA: 0x000BA15C File Offset: 0x000B835C
		// (remove) Token: 0x06001CCA RID: 7370 RVA: 0x000BA194 File Offset: 0x000B8394
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x17000872 RID: 2162
		// (get) Token: 0x06001CCB RID: 7371 RVA: 0x000BA1CC File Offset: 0x000B83CC
		// (set) Token: 0x06001CCC RID: 7372 RVA: 0x000BA1E4 File Offset: 0x000B83E4
		public int MinCount
		{
			get
			{
				return this.minCount;
			}
			set
			{
				this.minCount = value;
				this.OnPropertyChanged("MinCount");
			}
		}

		// Token: 0x17000873 RID: 2163
		// (get) Token: 0x06001CCD RID: 7373 RVA: 0x000BA1FA File Offset: 0x000B83FA
		// (set) Token: 0x06001CCE RID: 7374 RVA: 0x000BA202 File Offset: 0x000B8402
		public RelayCommand ChangeCountCommand { get; set; }

		// Token: 0x17000874 RID: 2164
		// (get) Token: 0x06001CCF RID: 7375 RVA: 0x000BA20B File Offset: 0x000B840B
		// (set) Token: 0x06001CD0 RID: 7376 RVA: 0x000BA213 File Offset: 0x000B8413
		public BoundLocalizedText StrMinCount { get; set; }

		// Token: 0x17000875 RID: 2165
		// (get) Token: 0x06001CD1 RID: 7377 RVA: 0x000BA21C File Offset: 0x000B841C
		// (set) Token: 0x06001CD2 RID: 7378 RVA: 0x000BA224 File Offset: 0x000B8424
		public MachineState MachineState { get; set; }

		// Token: 0x17000876 RID: 2166
		// (get) Token: 0x06001CD3 RID: 7379 RVA: 0x000BA22D File Offset: 0x000B842D
		// (set) Token: 0x06001CD4 RID: 7380 RVA: 0x000BA235 File Offset: 0x000B8435
		public bool DemoMode { get; set; }

		// Token: 0x17000877 RID: 2167
		// (get) Token: 0x06001CD5 RID: 7381 RVA: 0x000BA23E File Offset: 0x000B843E
		// (set) Token: 0x06001CD6 RID: 7382 RVA: 0x000BA246 File Offset: 0x000B8446
		public bool HighSpeedMode { get; set; }

		// Token: 0x17000878 RID: 2168
		// (get) Token: 0x06001CD7 RID: 7383 RVA: 0x000BA250 File Offset: 0x000B8450
		// (set) Token: 0x06001CD8 RID: 7384 RVA: 0x000BA268 File Offset: 0x000B8468
		public bool ShowNext
		{
			get
			{
				return this.showNext;
			}
			set
			{
				this.showNext = value;
				this.OnPropertyChanged("ShowNext");
			}
		}

		// Token: 0x06001CD9 RID: 7385 RVA: 0x000BA280 File Offset: 0x000B8480
		public LearnCounterViewModel(ParameterHandling parHandling, ImageDataTransfer imageTransfer, DBConnection dbConnection, int imageGroup, XRSImageBox imagebox, ProductManager productManager, StoreImageData imageArchiver, MachineState machineSate)
		{
			this.dbConnection = dbConnection;
			this.parHandling = parHandling;
			this.imageTransfer = imageTransfer;
			this.imagebox = imagebox;
			this.imageGroup = imageGroup;
			this.productManager = productManager;
			this.imageArchiver = imageArchiver;
			this.MachineState = machineSate;
			bool demoMode = false;
			dbConnection.GetDemoMode(ref demoMode);
			this.DemoMode = demoMode;
			bool flag = imageGroup <= 0;
			if (flag)
			{
				this.additionalLogbookText = ": P" + productManager.CurrentLoadedProduct.ProductNumber + " " + productManager.CurrentLoadedProduct.ProductName;
			}
			else
			{
				ProductData[] array = (from product in productManager.CurrentLoadedProduct.SubProducts
				where product.TableName.EndsWith(imageGroup.ToString())
				select product).ToArray<ProductData>();
				bool flag2 = array.Length != 0;
				if (flag2)
				{
					this.additionalLogbookText = string.Concat(new string[]
					{
						": P",
						productManager.CurrentLoadedProduct.ProductNumber,
						"_",
						imageGroup.ToString(),
						" ",
						array[0].ProductName
					});
				}
			}
			this.RegisterLocalizedText();
			imagebox.RegionSelectStarted += this.imagebox_RegionSelectStarted;
			imagebox.RegionSelectEnded += this.imagebox_RegionSelectEnded;
			string str = (imageGroup > 0) ? imageGroup.ToString() : "";
			try
			{
				string s = parHandling.ReadPar(Definition.filterMainPath + str + ObjectCounter.minCountSubPath);
				this.MinCount = int.Parse(s);
			}
			catch
			{
			}
			this.ChangeCountCommand = new RelayCommand(new Action<object>(this.ChangeCount));
			imageTransfer.FrameArrived += this.imageTransfer_FrameArrived;
			this.timer = new System.Threading.Timer(new TimerCallback(this.LoadLastImage));
			this.timer.Change(100, -1);
			bool flag3 = false;
			dbConnection.GetHighThroughputMachine(ref flag3);
			string text = "";
			dbConnection.GetParameterValue(Definition.highThroughputEnabled, ref text);
			bool flag4 = flag3 && text.Equals("1");
			if (flag4)
			{
				this.HighSpeedMode = true;
			}
			else
			{
				this.HighSpeedMode = false;
			}
			this.ShowNext = false;
		}

		// Token: 0x06001CDA RID: 7386 RVA: 0x000BA50C File Offset: 0x000B870C
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x06001CDB RID: 7387 RVA: 0x000BA539 File Offset: 0x000B8739
		public void RegisterLocalizedText()
		{
			this.StrMinCount = new BoundLocalizedText("txtMinCount");
		}

		// Token: 0x06001CDC RID: 7388 RVA: 0x000BA54D File Offset: 0x000B874D
		public void DeregisterLocalizedText()
		{
			MainWindow.localizationManager.Deregister(this.StrMinCount);
		}

		// Token: 0x06001CDD RID: 7389 RVA: 0x000BA564 File Offset: 0x000B8764
		private void imageTransfer_FrameArrived(object sender, ReceiveEventArgs e)
		{
			bool flag = e.Image.Type != ImageType.Histogram && e.Image.Type != ImageType.MaterialCurve && e.Image.Type != ImageType.TestcardSimulation;
			if (flag)
			{
				bool flag2 = !this.HighSpeedMode || (this.HighSpeedMode && this.ShowNext);
				if (flag2)
				{
					bool flag3 = e.Image.Type == ImageType.Demo;
					if (flag3)
					{
						bool flag4 = this.inAutolearnMode;
						if (flag4)
						{
							Filter.UpdateImageProcessingFilterPaths(this.dbConnection, this.parHandling, this.imageGroup, this.productManager, true);
							this.inAutolearnMode = false;
							LogbookManager.AddEntry(SSTEntryCode.CounterParameterChanged, 0);
							Progress.CloseStaticProgress(false);
						}
					}
					this.imagebox.RemoveRetrainRectangle();
					XRSImage copiedImage = e.Image.DeepCopy();
					this.productManager.PrepareRegionImage(copiedImage, this.imageGroup);
					this.currentImage = copiedImage;
					this.imagebox.ViewXRSImage(this.currentImage);
					this.ShowNext = false;
				}
			}
		}

		// Token: 0x06001CDE RID: 7390 RVA: 0x000BA678 File Offset: 0x000B8878
		public void Learn()
		{
			bool connectionEstablished = this.parHandling.ConnectionEstablished;
			if (connectionEstablished)
			{
				bool flag = this.imagebox.RectanglePainted();
				if (flag)
				{
					bool flag2 = Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtOverwriteExisting"), DialogStyle.Warning, DialogButtons.OKCancel) == Dialog.DialogResults.Ok;
					if (flag2)
					{
						Progress.ShowStaticProgress(LocalizationManager.Localize("txtProcessing"), false);
						string str = (this.imageGroup > 0) ? this.imageGroup.ToString() : "";
						this.parHandling.WriteValue(Definition.filterMainPath + str + ObjectCounter.autolearnSubPath, "1", false);
						this.parHandling.WriteValue(Definition.filterMainPath + str + ObjectCounter.minXSubPath, this.minPointX.ToString(), false);
						this.parHandling.WriteValue(Definition.filterMainPath + str + ObjectCounter.minYSubPath, this.minPointY.ToString(), false);
						this.parHandling.WriteValue(Definition.filterMainPath + str + ObjectCounter.maxXSubPath, this.maxPointX.ToString(), false);
						this.parHandling.WriteValue(Definition.filterMainPath + str + ObjectCounter.maxYSubPath, this.maxPointY.ToString(), false);
						this.inAutolearnMode = true;
						this.imageTransfer.SendImage(this.currentImage);
					}
				}
				else
				{
					Dialog.Show(LocalizationManager.Localize("txtError"), "No region selected.", DialogStyle.Error, DialogButtons.OK);
				}
			}
			else
			{
				Dialog.Show(LocalizationManager.Localize("txtError"), "Parameter interface not connected.", DialogStyle.Error, DialogButtons.OK);
			}
		}

		// Token: 0x06001CDF RID: 7391 RVA: 0x000BA818 File Offset: 0x000B8A18
		public void ShowCounterControl()
		{
			CtrlCounter ctrlCounter = new CtrlCounter(this.parHandling, this.imageTransfer, this.dbConnection, this.imageGroup, this.productManager, this.imageArchiver, this.MachineState);
			Subwindow.Show(ctrlCounter, SubwindowButtons.BackAndHome, ctrlCounter.Buttons);
		}

		// Token: 0x06001CE0 RID: 7392 RVA: 0x000BA864 File Offset: 0x000B8A64
		private void LoadLastImage(object obj)
		{
			bool flag = this.timer != null;
			if (flag)
			{
				this.timer.Dispose();
			}
			this.currentImage = HistoryHelper.GetLatestImage((!this.productManager.CurrentLoadedProduct.IsRegionProduct) ? this.imageGroup : 0, this.imageArchiver);
			Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(delegate()
			{
				bool flag2 = this.currentImage != null;
				if (flag2)
				{
					XRSImage xrsimage = this.currentImage.DeepCopy();
					this.productManager.PrepareRegionImage(xrsimage, this.imageGroup);
					this.imagebox.ViewXRSImage(xrsimage);
				}
				else
				{
					Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtNoValidImage"), DialogStyle.Error);
				}
			}));
		}

		// Token: 0x06001CE1 RID: 7393 RVA: 0x000BA8D8 File Offset: 0x000B8AD8
		private void imagebox_RegionSelectEnded(Point endPoint)
		{
			bool flag = this.startPoint.X > endPoint.X;
			if (flag)
			{
				this.maxPointX = this.startPoint.X;
				this.minPointX = endPoint.X;
			}
			else
			{
				this.maxPointX = endPoint.X;
				this.minPointX = this.startPoint.X;
			}
			bool flag2 = this.startPoint.Y > endPoint.Y;
			if (flag2)
			{
				this.maxPointY = this.startPoint.Y;
				this.minPointY = endPoint.Y;
			}
			else
			{
				this.maxPointY = endPoint.Y;
				this.minPointY = this.startPoint.Y;
			}
		}

		// Token: 0x06001CE2 RID: 7394 RVA: 0x000BA99A File Offset: 0x000B8B9A
		private void imagebox_RegionSelectStarted(Point startPoint)
		{
			this.startPoint = startPoint;
		}

		// Token: 0x06001CE3 RID: 7395 RVA: 0x000BA9A4 File Offset: 0x000B8BA4
		public void ChangeCount(object obj)
		{
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtMinCount"), this.minCount.ToString(), NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_InputCount;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x06001CE4 RID: 7396 RVA: 0x000BA9F8 File Offset: 0x000B8BF8
		private void numKeyboard_InputCount(object sender, KeyboardInputEventArgs e)
		{
			try
			{
				this.numKeyboard.TextInputKeyboard -= this.numKeyboard_InputCount;
				string str = (this.imageGroup > 0) ? this.imageGroup.ToString() : "";
				this.MinCount = int.Parse(e.Text);
				this.parHandling.WriteValue(Definition.filterMainPath + str + ObjectCounter.minCountSubPath, this.MinCount.ToString());
				this.parHandling.WriteValue(Definition.filterMainPath + str + ObjectCounter.maxCountSubPath, this.MinCount.ToString());
			}
			catch
			{
			}
		}

		// Token: 0x06001CE5 RID: 7397 RVA: 0x000BAABC File Offset: 0x000B8CBC
		public void CleanUp()
		{
			this.imagebox.RegionSelectStarted -= this.imagebox_RegionSelectStarted;
			this.imagebox.RegionSelectEnded -= this.imagebox_RegionSelectEnded;
			this.imageTransfer.FrameArrived -= this.imageTransfer_FrameArrived;
			bool flag = this.timer != null;
			if (flag)
			{
				this.timer.Dispose();
			}
			this.ChangeCountCommand = null;
			this.DeregisterLocalizedText();
			this.currentImage = null;
		}

		// Token: 0x06001CE6 RID: 7398 RVA: 0x000BAB41 File Offset: 0x000B8D41
		private void highSpeedTimer_Elapsed(object sender, ElapsedEventArgs e)
		{
			this.ShowNext = false;
		}

		// Token: 0x04001306 RID: 4870
		private int minCount;

		// Token: 0x04001307 RID: 4871
		private NumKeyboard numKeyboard;

		// Token: 0x04001308 RID: 4872
		private ImageDataTransfer imageTransfer;

		// Token: 0x04001309 RID: 4873
		private ParameterHandling parHandling;

		// Token: 0x0400130A RID: 4874
		private DBConnection dbConnection;

		// Token: 0x0400130B RID: 4875
		private XRSImage currentImage;

		// Token: 0x0400130C RID: 4876
		private System.Threading.Timer timer;

		// Token: 0x0400130D RID: 4877
		private Point startPoint;

		// Token: 0x0400130E RID: 4878
		private double maxPointY;

		// Token: 0x0400130F RID: 4879
		private double minPointY;

		// Token: 0x04001310 RID: 4880
		private double maxPointX;

		// Token: 0x04001311 RID: 4881
		private double minPointX;

		// Token: 0x04001312 RID: 4882
		private XRSImageBox imagebox;

		// Token: 0x04001313 RID: 4883
		private bool inAutolearnMode = false;

		// Token: 0x04001314 RID: 4884
		private int imageGroup;

		// Token: 0x04001315 RID: 4885
		private ProductManager productManager;

		// Token: 0x04001316 RID: 4886
		private string additionalLogbookText = "";

		// Token: 0x04001317 RID: 4887
		private StoreImageData imageArchiver;

		// Token: 0x0400131E RID: 4894
		private bool showNext = false;
	}
}
