using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows.Threading;
using DatabaseConnection;
using Definitions;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x0200019B RID: 411
	public class Counters : INotifyPropertyChanged
	{
		// Token: 0x140000D3 RID: 211
		// (add) Token: 0x060026E3 RID: 9955 RVA: 0x00108488 File Offset: 0x00106688
		// (remove) Token: 0x060026E4 RID: 9956 RVA: 0x001084C0 File Offset: 0x001066C0
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x17000B06 RID: 2822
		// (get) Token: 0x060026E5 RID: 9957 RVA: 0x001084F8 File Offset: 0x001066F8
		// (set) Token: 0x060026E6 RID: 9958 RVA: 0x00108510 File Offset: 0x00106710
		public ObservableCollection<CounterItem> ThroughputItems
		{
			get
			{
				return this.throughputItems;
			}
			set
			{
				this.throughputItems = value;
				this.OnPropertyChanged("ThroughputItems");
			}
		}

		// Token: 0x17000B07 RID: 2823
		// (get) Token: 0x060026E7 RID: 9959 RVA: 0x00108528 File Offset: 0x00106728
		// (set) Token: 0x060026E8 RID: 9960 RVA: 0x00108540 File Offset: 0x00106740
		public ObservableCollection<CounterItem> RejectItems
		{
			get
			{
				return this.rejectItems;
			}
			set
			{
				this.rejectItems = value;
				this.OnPropertyChanged("RejectItems");
			}
		}

		// Token: 0x17000B08 RID: 2824
		// (get) Token: 0x060026E9 RID: 9961 RVA: 0x00108558 File Offset: 0x00106758
		// (set) Token: 0x060026EA RID: 9962 RVA: 0x00108570 File Offset: 0x00106770
		public uint ThroughputCounter
		{
			get
			{
				return this.throughputCounter;
			}
			set
			{
				this.throughputCounter = value;
				this.OnPropertyChanged("ThroughputCounter");
			}
		}

		// Token: 0x17000B09 RID: 2825
		// (get) Token: 0x060026EB RID: 9963 RVA: 0x00108588 File Offset: 0x00106788
		// (set) Token: 0x060026EC RID: 9964 RVA: 0x001085A0 File Offset: 0x001067A0
		public uint RejectCounter
		{
			get
			{
				return this.rejectCounter;
			}
			set
			{
				this.rejectCounter = value;
				this.OnPropertyChanged("RejectCounter");
			}
		}

		// Token: 0x17000B0A RID: 2826
		// (get) Token: 0x060026ED RID: 9965 RVA: 0x001085B6 File Offset: 0x001067B6
		// (set) Token: 0x060026EE RID: 9966 RVA: 0x001085BE File Offset: 0x001067BE
		public Lane SelectedLane { get; set; }

		// Token: 0x17000B0B RID: 2827
		// (get) Token: 0x060026EF RID: 9967 RVA: 0x001085C7 File Offset: 0x001067C7
		// (set) Token: 0x060026F0 RID: 9968 RVA: 0x001085CF File Offset: 0x001067CF
		public int ImageGroup { get; set; }

		// Token: 0x17000B0C RID: 2828
		// (get) Token: 0x060026F1 RID: 9969 RVA: 0x001085D8 File Offset: 0x001067D8
		// (set) Token: 0x060026F2 RID: 9970 RVA: 0x001085F0 File Offset: 0x001067F0
		public ObservableCollection<CounterItem> GoodSinceRetrainItems
		{
			get
			{
				return this.goodSinceRetrainItems;
			}
			set
			{
				this.goodSinceRetrainItems = value;
				this.OnPropertyChanged("GoodSinceRetrainItems");
			}
		}

		// Token: 0x17000B0D RID: 2829
		// (get) Token: 0x060026F3 RID: 9971 RVA: 0x00108608 File Offset: 0x00106808
		// (set) Token: 0x060026F4 RID: 9972 RVA: 0x00108620 File Offset: 0x00106820
		public uint GoodSinceRetrainCounter
		{
			get
			{
				return this.goodSinceRetrainCounter;
			}
			set
			{
				this.goodSinceRetrainCounter = value;
				this.OnPropertyChanged("GoodSinceRetrainCounter");
			}
		}

		// Token: 0x060026F5 RID: 9973 RVA: 0x00108638 File Offset: 0x00106838
		public Counters(ImageDataTransfer imageTransfer, ParameterHandling parHandling, DBConnection dbConnection, AccessRightsManager accessManager, ProductManager productManager, MeanPixelLogger meanPixelLogger, MachineState machineState, Lane lane, int group, StoreImageData imageArchiver, TestcardSimService testcardSimService, SetFooterMainWindow setFooterMainWindow, Sensitivity bulksensitivity)
		{
			this.imageTransfer = imageTransfer;
			this.parHandling = parHandling;
			this.dbConnection = dbConnection;
			this.accessManager = accessManager;
			this.productManager = productManager;
			this.meanPixelLogger = meanPixelLogger;
			this.machineState = machineState;
			this.imageArchiver = imageArchiver;
			this.testcardSimService = testcardSimService;
			this.setFooterMainWindow = setFooterMainWindow;
			this.bulksensitivity = bulksensitivity;
			this.SelectedLane = lane;
			this.ImageGroup = group;
			accessManager.CurrentUser.PropertyChanged += this.CurrentUser_PropertyChanged;
			parHandling.ParameterChanged += this.parHandling_ParameterChanged;
			imageTransfer.FrameArrived += this.imageTransfer_FrameArrived;
			this.throughputTimer.Tick += this.ShowThroughput;
			this.throughputTimer.Interval = new TimeSpan(0, 0, 0, 0, 100);
			this.rejectTimer.Tick += this.ShowReject;
			this.rejectTimer.Interval = new TimeSpan(0, 0, 0, 0, 100);
			this.goodSinceLastRetrainTimer.Tick += this.ShowGoodSinceLastRetrainProduct;
			this.goodSinceLastRetrainTimer.Interval = new TimeSpan(0, 0, 0, 0, 100);
			this.cItemRejectHistory = new CounterItem("msgShowHistory", new Action<object>(this.CloseCtMenuAndStartRejectTimer));
			this.cItemRejectReset = new CounterItem("msgResetCounters", delegate(object obj)
			{
				bool flag = Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("msgResetRejectCounter"), DialogStyle.Warning, DialogButtons.OKCancel) == Dialog.DialogResults.Ok;
				if (flag)
				{
					this.RejectCounter = 0U;
				}
			});
			this.cItemThroughputHistory = new CounterItem("msgShowHistory", new Action<object>(this.CloseCtMenuAndStartThroughputTimer));
			this.cItemThroughputReset = new CounterItem("msgResetCounters", delegate(object obj)
			{
				bool flag = Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("msgResetThroughputCounter"), DialogStyle.Warning, DialogButtons.OKCancel) == Dialog.DialogResults.Ok;
				if (flag)
				{
					this.ThroughputCounter = 0U;
					bool flag2 = this.AvgThroughputPerMin != null;
					if (flag2)
					{
						this.AvgThroughputPerMin = new int[60];
					}
				}
			});
			this.cItemGoodSinceRetrainHistory = new CounterItem("msgShowHistory", new Action<object>(this.CloseCtMenuAndStartGoodSinceLastRetrainTimer));
			this.cItemGoodSinceRetrainReset = new CounterItem("msgResetCounters", delegate(object obj)
			{
				bool flag = Dialog.Show(LocalizationManager.Localize("txtWarning"), "Retrain Demo-Zähler zurücksetzen?", DialogStyle.Warning, DialogButtons.OKCancel) == Dialog.DialogResults.Ok;
				if (flag)
				{
					this.GoodSinceRetrainCounter = 0U;
				}
			});
			MainWindow.localizationManager.Register(this.cItemRejectHistory);
			MainWindow.localizationManager.Register(this.cItemRejectReset);
			MainWindow.localizationManager.Register(this.cItemThroughputHistory);
			MainWindow.localizationManager.Register(this.cItemThroughputReset);
			MainWindow.localizationManager.Register(this.cItemGoodSinceRetrainHistory);
			MainWindow.localizationManager.Register(this.cItemGoodSinceRetrainReset);
			this.CreateItems();
		}

		// Token: 0x060026F6 RID: 9974 RVA: 0x001088C8 File Offset: 0x00106AC8
		private void parHandling_ParameterChanged(object sender, ParameterChangeEventArgs e)
		{
			bool flag = e.Path == Definition.batchesEnabledPath || e.Path == Definition.machineModePath;
			if (flag)
			{
				this.CreateItems();
			}
		}

		// Token: 0x060026F7 RID: 9975 RVA: 0x00108906 File Offset: 0x00106B06
		private void CurrentUser_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			this.CreateItems();
		}

		// Token: 0x060026F8 RID: 9976 RVA: 0x00108910 File Offset: 0x00106B10
		private void imageTransfer_FrameArrived(object sender, ReceiveEventArgs e)
		{
			bool flag = e.Image.IsImageToIncreaseProductCounter(this.machineState, this.meanPixelLogger, this.parHandling) && e.Image.Type != ImageType.TestcardSimulation;
			if (flag)
			{
				DateTime now = DateTime.Now;
				bool flag2 = this.productManager.CurrentLoadedProduct.IsRegionProduct || ((this.SelectedLane == Lane.All || this.SelectedLane == e.Image.Lane) && (this.ImageGroup == 0 || this.ImageGroup == (int)e.Image.Group));
				if (flag2)
				{
					bool flag3 = this.parHandling.ReadPar(Definition.throughputCountPerMin) == "1";
					if (flag3)
					{
						this.throughputCounterPerMin += 1U;
					}
					else
					{
						uint num = this.ThroughputCounter;
						this.ThroughputCounter = num + 1U;
						num = this.GoodSinceRetrainCounter;
						this.GoodSinceRetrainCounter = num + 1U;
					}
				}
				bool flag4 = e.Image.HasErrors();
				if (flag4)
				{
					bool flag5 = this.productManager.CurrentLoadedProduct.IsRegionProduct || ((this.SelectedLane == Lane.All || this.SelectedLane == e.Image.Lane) && (this.ImageGroup == 0 || this.ImageGroup == (int)e.Image.Group));
					if (flag5)
					{
						uint num = this.RejectCounter;
						this.RejectCounter = num + 1U;
						this.GoodSinceRetrainCounter = 0U;
					}
				}
			}
		}

		// Token: 0x060026F9 RID: 9977 RVA: 0x00108A94 File Offset: 0x00106C94
		public void StartUpdateThroughputPerMin()
		{
			bool flag = !this.updateThreadRunning;
			if (flag)
			{
				this.updateThreadRunning = true;
				new Thread(new ThreadStart(this.UpdateThroughputPerMin))
				{
					Name = "UpdateThroughputPerMin"
				}.Start();
			}
		}

		// Token: 0x060026FA RID: 9978 RVA: 0x00108ADD File Offset: 0x00106CDD
		public void StopUpdateThroughputPerMin()
		{
			this.updateThreadRunning = false;
		}

		// Token: 0x060026FB RID: 9979 RVA: 0x00108AE8 File Offset: 0x00106CE8
		private void UpdateThroughputPerMin()
		{
			int second = DateTime.Now.Second;
			int num = 60;
			this.AvgThroughputPerMin = new int[num];
			int num2 = 0;
			int num3 = 0;
			bool flag = true;
			double num4 = 0.0;
			int num5 = 60;
			while (this.updateThreadRunning)
			{
				bool flag2 = this.machineState.State == MachineStates.Inspecting;
				if (flag2)
				{
					int num6 = Convert.ToInt32(this.throughputCounterPerMin);
					bool flag3 = num6 - num3 > 0;
					if (flag3)
					{
						this.AvgThroughputPerMin[num2] = num6 - num3;
					}
					else
					{
						this.AvgThroughputPerMin[num2] = 0;
					}
					bool flag4 = this.AvgThroughputPerMin.Sum() == 0;
					if (flag4)
					{
						flag = true;
					}
					else
					{
						num2++;
					}
					bool flag5 = num2 < num;
					if (flag5)
					{
						bool flag6 = flag;
						if (flag6)
						{
							num4 = (double)num5 / Convert.ToDouble(num2 + 1);
						}
						else
						{
							num4 = 1.0;
						}
					}
					else
					{
						flag = false;
						num2 = 0;
					}
					this.ThroughputCounter = Convert.ToUInt32(Convert.ToDouble(this.AvgThroughputPerMin.Sum()) * num4);
					num3 = num6;
				}
				else
				{
					this.AvgThroughputPerMin = new int[num];
					this.ThroughputCounter = 0U;
					this.throughputCounterPerMin = 0U;
					num2 = 0;
					flag = true;
				}
				Thread.Sleep(1000);
			}
		}

		// Token: 0x060026FC RID: 9980 RVA: 0x00108C38 File Offset: 0x00106E38
		private void CreateItems()
		{
			this.RejectItems = new ObservableCollection<CounterItem>();
			this.ThroughputItems = new ObservableCollection<CounterItem>();
			this.GoodSinceRetrainItems = new ObservableCollection<CounterItem>();
			bool flag = false;
			this.dbConnection.GetBatchesEnabled(ref flag);
			MachineMode machineMode = MachineMode.Standard;
			this.dbConnection.GetMachineMode(ref machineMode);
			bool flag2 = this.accessManager.CurrentUser.Rights >= this.rightHistory;
			if (flag2)
			{
				this.RejectItems.Add(this.cItemRejectHistory);
			}
			bool flag3 = !flag && this.accessManager.CurrentUser.Rights >= this.rightResetConters && machineMode != MachineMode.Bulk;
			if (flag3)
			{
				this.RejectItems.Add(this.cItemRejectReset);
			}
			bool flag4 = this.accessManager.CurrentUser.Rights >= this.rightHistory;
			if (flag4)
			{
				this.ThroughputItems.Add(this.cItemThroughputHistory);
			}
			bool flag5 = !flag && this.accessManager.CurrentUser.Rights >= this.rightResetConters;
			if (flag5)
			{
				this.ThroughputItems.Add(this.cItemThroughputReset);
			}
			bool flag6 = this.accessManager.CurrentUser.Rights >= this.rightHistory;
			if (flag6)
			{
				this.GoodSinceRetrainItems.Add(this.cItemGoodSinceRetrainHistory);
			}
			bool flag7 = !flag && this.accessManager.CurrentUser.Rights >= this.rightResetConters;
			if (flag7)
			{
				this.GoodSinceRetrainItems.Add(this.cItemGoodSinceRetrainReset);
			}
		}

		// Token: 0x060026FD RID: 9981 RVA: 0x00108DCE File Offset: 0x00106FCE
		private void CloseCtMenuAndStartRejectTimer(object obj)
		{
			this.rejectTimer.Start();
		}

		// Token: 0x060026FE RID: 9982 RVA: 0x00108DE0 File Offset: 0x00106FE0
		private void ShowReject(object sender, EventArgs e)
		{
			this.rejectTimer.Stop();
			int imageGroup = 0;
			bool flag = this.productManager != null && this.productManager.CurrentLoadedProduct != null;
			if (flag)
			{
				imageGroup = ((!this.productManager.CurrentLoadedProduct.IsRegionProduct) ? this.ImageGroup : 0);
			}
			HistoryControl usrControl = new HistoryControl(this.imageTransfer, this.parHandling, this.dbConnection, false, this.accessManager, this.productManager, this.SelectedLane, imageGroup, this.imageArchiver, this.testcardSimService, this.setFooterMainWindow, this.bulksensitivity, -1);
			Subwindow.Show(usrControl, SubwindowButtons.Back, true);
		}

		// Token: 0x060026FF RID: 9983 RVA: 0x00108E83 File Offset: 0x00107083
		private void CloseCtMenuAndStartThroughputTimer(object obj)
		{
			this.throughputTimer.Start();
		}

		// Token: 0x06002700 RID: 9984 RVA: 0x00108E94 File Offset: 0x00107094
		private void ShowThroughput(object sender, EventArgs e)
		{
			this.throughputTimer.Stop();
			int imageGroup = 0;
			bool flag = this.productManager != null && this.productManager.CurrentLoadedProduct != null;
			if (flag)
			{
				imageGroup = ((!this.productManager.CurrentLoadedProduct.IsRegionProduct) ? this.ImageGroup : 0);
			}
			HistoryControl usrControl = new HistoryControl(this.imageTransfer, this.parHandling, this.dbConnection, true, this.accessManager, this.productManager, this.SelectedLane, imageGroup, this.imageArchiver, this.testcardSimService, this.setFooterMainWindow, this.bulksensitivity, -1);
			Subwindow.Show(usrControl, SubwindowButtons.Back, true);
		}

		// Token: 0x06002701 RID: 9985 RVA: 0x00108F38 File Offset: 0x00107138
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x06002702 RID: 9986 RVA: 0x00108F65 File Offset: 0x00107165
		public void StopCounting()
		{
			this.imageTransfer.FrameArrived -= this.imageTransfer_FrameArrived;
		}

		// Token: 0x06002703 RID: 9987 RVA: 0x00108F80 File Offset: 0x00107180
		public void StartCounting()
		{
			this.imageTransfer.FrameArrived -= this.imageTransfer_FrameArrived;
			this.imageTransfer.FrameArrived += this.imageTransfer_FrameArrived;
		}

		// Token: 0x06002704 RID: 9988 RVA: 0x00108FB3 File Offset: 0x001071B3
		public void ResetCounters()
		{
			this.ThroughputCounter = 0U;
			this.RejectCounter = 0U;
			this.GoodSinceRetrainCounter = 0U;
		}

		// Token: 0x06002705 RID: 9989 RVA: 0x00108FD0 File Offset: 0x001071D0
		private void ShowGoodSinceLastRetrainProduct(object sender, EventArgs e)
		{
			this.goodSinceLastRetrainTimer.Stop();
			int imageGroup = 0;
			bool flag = this.productManager != null && this.productManager.CurrentLoadedProduct != null;
			if (flag)
			{
				imageGroup = ((!this.productManager.CurrentLoadedProduct.IsRegionProduct) ? this.ImageGroup : 0);
			}
			HistoryControl usrControl = new HistoryControl(this.imageTransfer, this.parHandling, this.dbConnection, false, this.accessManager, this.productManager, this.SelectedLane, imageGroup, this.imageArchiver, this.testcardSimService, this.setFooterMainWindow, this.bulksensitivity, (int)this.GoodSinceRetrainCounter);
			Subwindow.Show(usrControl, SubwindowButtons.Back, true);
		}

		// Token: 0x06002706 RID: 9990 RVA: 0x00109078 File Offset: 0x00107278
		private void CloseCtMenuAndStartGoodSinceLastRetrainTimer(object obj)
		{
			this.goodSinceLastRetrainTimer.Start();
		}

		// Token: 0x040018FC RID: 6396
		private BuiltInPriviledges rightHistory = BuiltInPriviledges.NoUser;

		// Token: 0x040018FD RID: 6397
		private BuiltInPriviledges rightResetConters = BuiltInPriviledges.Operator2;

		// Token: 0x040018FE RID: 6398
		public static string timeUpdateProductIndexPath = "/application/TimeUpdateProductIndex/Value";

		// Token: 0x040018FF RID: 6399
		private uint rejectCounter;

		// Token: 0x04001900 RID: 6400
		private uint throughputCounter;

		// Token: 0x04001901 RID: 6401
		private ImageDataTransfer imageTransfer;

		// Token: 0x04001902 RID: 6402
		private ParameterHandling parHandling;

		// Token: 0x04001903 RID: 6403
		private DBConnection dbConnection;

		// Token: 0x04001904 RID: 6404
		private DispatcherTimer rejectTimer = new DispatcherTimer();

		// Token: 0x04001905 RID: 6405
		private DispatcherTimer throughputTimer = new DispatcherTimer();

		// Token: 0x04001906 RID: 6406
		private AccessRightsManager accessManager;

		// Token: 0x04001907 RID: 6407
		private CounterItem cItemRejectHistory;

		// Token: 0x04001908 RID: 6408
		private CounterItem cItemRejectReset;

		// Token: 0x04001909 RID: 6409
		private CounterItem cItemThroughputHistory;

		// Token: 0x0400190A RID: 6410
		private CounterItem cItemThroughputReset;

		// Token: 0x0400190B RID: 6411
		private ObservableCollection<CounterItem> throughputItems;

		// Token: 0x0400190C RID: 6412
		private ObservableCollection<CounterItem> rejectItems;

		// Token: 0x0400190D RID: 6413
		private ProductManager productManager;

		// Token: 0x0400190E RID: 6414
		private MeanPixelLogger meanPixelLogger;

		// Token: 0x0400190F RID: 6415
		private MachineState machineState;

		// Token: 0x04001910 RID: 6416
		private StoreImageData imageArchiver;

		// Token: 0x04001911 RID: 6417
		private TestcardSimService testcardSimService;

		// Token: 0x04001912 RID: 6418
		private const int updateInterval = 1000;

		// Token: 0x04001913 RID: 6419
		private bool updateThreadRunning;

		// Token: 0x04001915 RID: 6421
		private uint throughputCounterPerMin;

		// Token: 0x04001916 RID: 6422
		private int[] AvgThroughputPerMin;

		// Token: 0x04001919 RID: 6425
		private SetFooterMainWindow setFooterMainWindow;

		// Token: 0x0400191A RID: 6426
		private Sensitivity bulksensitivity;

		// Token: 0x0400191B RID: 6427
		private uint goodSinceRetrainCounter;

		// Token: 0x0400191C RID: 6428
		private CounterItem cItemGoodSinceRetrainReset;

		// Token: 0x0400191D RID: 6429
		private ObservableCollection<CounterItem> goodSinceRetrainItems;

		// Token: 0x0400191E RID: 6430
		private CounterItem cItemGoodSinceRetrainHistory;

		// Token: 0x0400191F RID: 6431
		private DispatcherTimer goodSinceLastRetrainTimer = new DispatcherTimer();
	}
}
