using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Common;
using DatabaseConnection;
using Definitions;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x020000D4 RID: 212
	public class AIViewModel : PreviewViewModel
	{
		// Token: 0x17000639 RID: 1593
		// (get) Token: 0x0600149B RID: 5275 RVA: 0x0007A920 File Offset: 0x00078B20
		// (set) Token: 0x0600149C RID: 5276 RVA: 0x0007A928 File Offset: 0x00078B28
		public RelayCommand ShowSettingsCommand { get; set; }

		// Token: 0x1700063A RID: 1594
		// (get) Token: 0x0600149D RID: 5277 RVA: 0x0007A931 File Offset: 0x00078B31
		// (set) Token: 0x0600149E RID: 5278 RVA: 0x0007A939 File Offset: 0x00078B39
		public RelayCommand StartTrainingCommand { get; set; }

		// Token: 0x1700063B RID: 1595
		// (get) Token: 0x0600149F RID: 5279 RVA: 0x0007A942 File Offset: 0x00078B42
		// (set) Token: 0x060014A0 RID: 5280 RVA: 0x0007A94A File Offset: 0x00078B4A
		public RelayCommand StartDeleteMarkedImagesCommand { get; set; }

		// Token: 0x1700063C RID: 1596
		// (get) Token: 0x060014A1 RID: 5281 RVA: 0x0007A953 File Offset: 0x00078B53
		// (set) Token: 0x060014A2 RID: 5282 RVA: 0x0007A95B File Offset: 0x00078B5B
		public RelayCommand StartRestoreModelCommand { get; set; }

		// Token: 0x1700063D RID: 1597
		// (get) Token: 0x060014A3 RID: 5283 RVA: 0x0007A964 File Offset: 0x00078B64
		// (set) Token: 0x060014A4 RID: 5284 RVA: 0x0007A96C File Offset: 0x00078B6C
		public RelayCommand StartDeleteModelCommand { get; set; }

		// Token: 0x1700063E RID: 1598
		// (get) Token: 0x060014A5 RID: 5285 RVA: 0x0007A975 File Offset: 0x00078B75
		// (set) Token: 0x060014A6 RID: 5286 RVA: 0x0007A97D File Offset: 0x00078B7D
		public List<PreviewItem> SelectedPrievewItems { get; set; }

		// Token: 0x1400006B RID: 107
		// (add) Token: 0x060014A7 RID: 5287 RVA: 0x0007A988 File Offset: 0x00078B88
		// (remove) Token: 0x060014A8 RID: 5288 RVA: 0x0007A9C0 File Offset: 0x00078BC0
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action SendTestcardSimImages;

		// Token: 0x1700063F RID: 1599
		// (get) Token: 0x060014A9 RID: 5289 RVA: 0x0007A9F8 File Offset: 0x00078BF8
		// (set) Token: 0x060014AA RID: 5290 RVA: 0x0007AA10 File Offset: 0x00078C10
		public bool SimImageArrived
		{
			get
			{
				return this.simImageArrived;
			}
			set
			{
				bool flag = this.simImageArrived != value;
				if (flag)
				{
					this.simImageArrived = value;
					this.OnPropertyChanged("SimImageArrived");
				}
			}
		}

		// Token: 0x17000640 RID: 1600
		// (get) Token: 0x060014AB RID: 5291 RVA: 0x0007AA44 File Offset: 0x00078C44
		// (set) Token: 0x060014AC RID: 5292 RVA: 0x0007AA5C File Offset: 0x00078C5C
		public int SimulatedImagesCounter
		{
			get
			{
				return this.simulatedImagesCounter;
			}
			set
			{
				bool flag = this.simulatedImagesCounter != value;
				if (flag)
				{
					this.simulatedImagesCounter = value;
					this.OnPropertyChanged("SimulatedImagesCounter");
				}
			}
		}

		// Token: 0x17000641 RID: 1601
		// (get) Token: 0x060014AD RID: 5293 RVA: 0x0007AA90 File Offset: 0x00078C90
		// (set) Token: 0x060014AE RID: 5294 RVA: 0x0007AAA8 File Offset: 0x00078CA8
		public string ButtonImageRejectsPath
		{
			get
			{
				return this.buttonImageRejectsPath;
			}
			set
			{
				bool flag = this.buttonImageRejectsPath != value;
				if (flag)
				{
					this.buttonImageRejectsPath = value;
					this.OnPropertyChanged("ButtonImageRejectsPath");
				}
			}
		}

		// Token: 0x17000642 RID: 1602
		// (get) Token: 0x060014AF RID: 5295 RVA: 0x0007AADC File Offset: 0x00078CDC
		// (set) Token: 0x060014B0 RID: 5296 RVA: 0x0007AAF4 File Offset: 0x00078CF4
		public string ButtonImageOkPath
		{
			get
			{
				return this.buttonImageOkPath;
			}
			set
			{
				bool flag = this.buttonImageOkPath != value;
				if (flag)
				{
					this.buttonImageOkPath = value;
					this.OnPropertyChanged("ButtonImageOkPath");
				}
			}
		}

		// Token: 0x17000643 RID: 1603
		// (get) Token: 0x060014B1 RID: 5297 RVA: 0x0007AB28 File Offset: 0x00078D28
		// (set) Token: 0x060014B2 RID: 5298 RVA: 0x0007AB40 File Offset: 0x00078D40
		public string ButtonImageAllPath
		{
			get
			{
				return this.buttonImageAllPath;
			}
			set
			{
				bool flag = this.buttonImageAllPath != value;
				if (flag)
				{
					this.buttonImageAllPath = value;
					this.OnPropertyChanged("ButtonImageAllPath");
				}
			}
		}

		// Token: 0x17000644 RID: 1604
		// (get) Token: 0x060014B3 RID: 5299 RVA: 0x0007AB74 File Offset: 0x00078D74
		// (set) Token: 0x060014B4 RID: 5300 RVA: 0x0007AB8C File Offset: 0x00078D8C
		public string ButtonImageDatasetPath
		{
			get
			{
				return this.buttonImageDatasetPath;
			}
			set
			{
				bool flag = this.buttonImageDatasetPath != value;
				if (flag)
				{
					this.buttonImageDatasetPath = value;
					this.OnPropertyChanged("buttonImageDatasetPath");
				}
			}
		}

		// Token: 0x17000645 RID: 1605
		// (get) Token: 0x060014B5 RID: 5301 RVA: 0x0007ABC0 File Offset: 0x00078DC0
		// (set) Token: 0x060014B6 RID: 5302 RVA: 0x0007ABD8 File Offset: 0x00078DD8
		public bool ShowRejectImages
		{
			get
			{
				return this.showRejectImages;
			}
			set
			{
				bool flag = this.showRejectImages != value;
				if (flag)
				{
					this.showRejectImages = value;
					bool flag2 = this.showRejectImages;
					if (flag2)
					{
						this.ButtonImageOkPath = "..\\img\\checkmark_enabled_darker_big_disabled.png";
						this.ButtonImageAllPath = "..\\img\\xriimagelist_small_disabled.png";
						this.ButtonImageDatasetPath = "..\\img\\ai_dataset_disabled.png";
						this.ShowOkImages = false;
						this.ShowAllImages = false;
						this.ShowDatasetImages = false;
						base.Preview.IsRejectViewAI = true;
						base.Preview.IsOkViewAI = false;
						base.Preview.IsAllViewAI = false;
						base.Preview.IsDatasetViewAI = false;
						bool flag3 = this.SelectedPrievewItems != null;
						if (flag3)
						{
							this.SelectedItemCount = Convert.ToString(this.SelectedPrievewItems.Count);
						}
						else
						{
							this.SelectedItemCount = "0";
						}
						base.Preview.PreviewItems.Clear();
						bool flag4 = this.notInitView;
						if (flag4)
						{
							base.Preview.CreatePreview();
							this.timerCheckHistoryLoading = new Timer(new TimerCallback(this.timer_CheckHistoryLoading), null, 0, 500);
						}
						this.notInitView = true;
					}
					this.UpdateFineTuningMsg();
					this.AllImagesAreMarked = false;
					this.OnPropertyChanged("ShowRejectImages");
				}
			}
		}

		// Token: 0x17000646 RID: 1606
		// (get) Token: 0x060014B7 RID: 5303 RVA: 0x0007AD18 File Offset: 0x00078F18
		// (set) Token: 0x060014B8 RID: 5304 RVA: 0x0007AD30 File Offset: 0x00078F30
		public bool ShowOkImages
		{
			get
			{
				return this.showOkImages;
			}
			set
			{
				bool flag = this.showOkImages != value;
				if (flag)
				{
					this.showOkImages = value;
					bool flag2 = this.showOkImages;
					if (flag2)
					{
						this.ButtonImageRejectsPath = "..\\img\\reject_orange_disabled.png";
						this.ButtonImageAllPath = "..\\img\\xriimagelist_small_disabled.png";
						this.ButtonImageDatasetPath = "..\\img\\ai_dataset_disabled.png";
						this.ShowAllImages = false;
						this.ShowRejectImages = false;
						this.ShowDatasetImages = false;
						base.Preview.IsRejectViewAI = false;
						base.Preview.IsOkViewAI = true;
						base.Preview.IsAllViewAI = false;
						base.Preview.IsDatasetViewAI = false;
						bool flag3 = this.SelectedPrievewItems != null;
						if (flag3)
						{
							this.SelectedItemCount = Convert.ToString(this.SelectedPrievewItems.Count);
						}
						else
						{
							this.SelectedItemCount = "0";
						}
						base.Preview.PreviewItems.Clear();
						bool flag4 = this.notInitView;
						if (flag4)
						{
							base.Preview.CreatePreview();
							this.timerCheckHistoryLoading = new Timer(new TimerCallback(this.timer_CheckHistoryLoading), null, 0, 500);
						}
						this.notInitView = true;
					}
					this.UpdateFineTuningMsg();
					this.AllImagesAreMarked = false;
					this.OnPropertyChanged("ShowOkImages");
				}
			}
		}

		// Token: 0x17000647 RID: 1607
		// (get) Token: 0x060014B9 RID: 5305 RVA: 0x0007AE70 File Offset: 0x00079070
		// (set) Token: 0x060014BA RID: 5306 RVA: 0x0007AE88 File Offset: 0x00079088
		public bool ShowAllImages
		{
			get
			{
				return this.showAllImages;
			}
			set
			{
				bool flag = this.showAllImages != value;
				if (flag)
				{
					this.showAllImages = value;
					bool flag2 = this.showAllImages;
					if (flag2)
					{
						this.ButtonImageRejectsPath = "..\\img\\reject_orange_disabled.png";
						this.ButtonImageOkPath = "..\\img\\checkmark_enabled_darker_big_disabled.png";
						this.ButtonImageDatasetPath = "..\\img\\ai_dataset_disabled.png";
						this.ShowRejectImages = false;
						this.ShowOkImages = false;
						this.ShowDatasetImages = false;
						base.Preview.IsRejectViewAI = false;
						base.Preview.IsOkViewAI = false;
						base.Preview.IsAllViewAI = true;
						base.Preview.IsDatasetViewAI = false;
						bool flag3 = this.SelectedPrievewItems != null;
						if (flag3)
						{
							this.SelectedItemCount = Convert.ToString(this.SelectedPrievewItems.Count);
						}
						else
						{
							this.SelectedItemCount = "0";
						}
						base.Preview.PreviewItems.Clear();
						bool flag4 = this.notInitView;
						if (flag4)
						{
							base.Preview.CreatePreview();
							this.timerCheckHistoryLoading = new Timer(new TimerCallback(this.timer_CheckHistoryLoading), null, 0, 500);
						}
						this.notInitView = true;
					}
					this.UpdateFineTuningMsg();
					this.AllImagesAreMarked = false;
					this.OnPropertyChanged("ShowAllImages");
				}
			}
		}

		// Token: 0x17000648 RID: 1608
		// (get) Token: 0x060014BB RID: 5307 RVA: 0x0007AFC8 File Offset: 0x000791C8
		// (set) Token: 0x060014BC RID: 5308 RVA: 0x0007AFE0 File Offset: 0x000791E0
		public bool ShowDatasetImages
		{
			get
			{
				return this.showDatasetImages;
			}
			set
			{
				bool flag = this.showDatasetImages != value;
				if (flag)
				{
					this.showDatasetImages = value;
					bool flag2 = this.showDatasetImages;
					if (flag2)
					{
						this.ButtonImageRejectsPath = "..\\img\\reject_orange_disabled.png";
						this.ButtonImageOkPath = "..\\img\\checkmark_enabled_darker_big_disabled.png";
						this.ButtonImageAllPath = "..\\img\\xriimagelist_small_disabled.png";
						this.ShowAllImages = false;
						this.ShowRejectImages = false;
						this.ShowOkImages = false;
						base.Preview.IsRejectViewAI = false;
						base.Preview.IsOkViewAI = false;
						base.Preview.IsAllViewAI = false;
						base.Preview.IsDatasetViewAI = true;
						bool flag3 = this.SelectedPrievewItems != null;
						if (flag3)
						{
							this.SelectedItemCount = Convert.ToString(this.SelectedPrievewItems.Count);
						}
						else
						{
							this.SelectedItemCount = "0";
						}
						base.Preview.PreviewItems.Clear();
						bool flag4 = this.notInitView;
						if (flag4)
						{
							base.Preview.CreatePreview();
							this.timerCheckHistoryLoading = new Timer(new TimerCallback(this.timer_CheckHistoryLoading), null, 0, 500);
						}
						this.notInitView = true;
					}
					this.UpdateFineTuningMsg();
					this.AllImagesAreMarked = false;
					this.OnPropertyChanged("ShowDatasetImages");
				}
			}
		}

		// Token: 0x17000649 RID: 1609
		// (get) Token: 0x060014BD RID: 5309 RVA: 0x0007B120 File Offset: 0x00079320
		// (set) Token: 0x060014BE RID: 5310 RVA: 0x0007B138 File Offset: 0x00079338
		public bool AllImagesAreMarked
		{
			get
			{
				return this.allImagesAreMarked;
			}
			set
			{
				bool flag = this.allImagesAreMarked != value;
				if (flag)
				{
					this.allImagesAreMarked = value;
					this.OnPropertyChanged("AllImagesAreMarked");
				}
			}
		}

		// Token: 0x1700064A RID: 1610
		// (get) Token: 0x060014BF RID: 5311 RVA: 0x0007B16C File Offset: 0x0007936C
		// (set) Token: 0x060014C0 RID: 5312 RVA: 0x0007B184 File Offset: 0x00079384
		public bool HistoryisLoading
		{
			get
			{
				return this.historyisLoading;
			}
			set
			{
				bool flag = this.historyisLoading != value;
				if (flag)
				{
					this.historyisLoading = value;
					this.OnPropertyChanged("HistoryisLoading");
				}
			}
		}

		// Token: 0x1700064B RID: 1611
		// (get) Token: 0x060014C1 RID: 5313 RVA: 0x0007B1B8 File Offset: 0x000793B8
		// (set) Token: 0x060014C2 RID: 5314 RVA: 0x0007B1D0 File Offset: 0x000793D0
		public string SelectedItemCount
		{
			get
			{
				return this.selectedItemCount;
			}
			set
			{
				bool flag = this.selectedItemCount != value;
				if (flag)
				{
					this.selectedItemCount = value;
					this.OnPropertyChanged("SelectedItemCount");
				}
			}
		}

		// Token: 0x1700064C RID: 1612
		// (get) Token: 0x060014C3 RID: 5315 RVA: 0x0007B203 File Offset: 0x00079403
		// (set) Token: 0x060014C4 RID: 5316 RVA: 0x0007B20B File Offset: 0x0007940B
		public PreviewItem FirstSelected { get; set; }

		// Token: 0x1700064D RID: 1613
		// (get) Token: 0x060014C5 RID: 5317 RVA: 0x0007B214 File Offset: 0x00079414
		// (set) Token: 0x060014C6 RID: 5318 RVA: 0x0007B22C File Offset: 0x0007942C
		public bool ShowFineTuningMessage
		{
			get
			{
				return this.showFineTuningMessage;
			}
			set
			{
				this.showFineTuningMessage = value;
				this.OnPropertyChanged("ShowFineTuningMessage");
			}
		}

		// Token: 0x1700064E RID: 1614
		// (get) Token: 0x060014C7 RID: 5319 RVA: 0x0007B242 File Offset: 0x00079442
		// (set) Token: 0x060014C8 RID: 5320 RVA: 0x0007B24A File Offset: 0x0007944A
		public BoundLocalizedText StrFineTuningMessage { get; set; }

		// Token: 0x060014C9 RID: 5321 RVA: 0x0007B254 File Offset: 0x00079454
		public AIViewModel(ParameterHandling parHandling, XRSImageBox imagebox, string[] imageFolders, StoreImageData imageArchiver, ImageDataTransfer imageTransfer, DBConnection dbConnection, ProductManager productManager, AIService aiService, AccessRightsManager accessManager, ErrorHandling errorHandling, TestcardSimService testcardSimService) : base(imagebox, imageFolders, 300, Lane.All, 0, imageArchiver, false, true, productManager.CurrentLoadedProduct.ProductName, -1)
		{
			this.parHandling = parHandling;
			this.imageArchiver = imageArchiver;
			this.imageTransfer = imageTransfer;
			this.dbConnection = dbConnection;
			this.productManager = productManager;
			this.aiService = aiService;
			this.accessManager = accessManager;
			this.errorHandling = errorHandling;
			this.testcardSimService = testcardSimService;
			this.StartTrainingCommand = new RelayCommand(new Action<object>(this.StartTrain));
			this.StartRestoreModelCommand = new RelayCommand(new Action<object>(this.StartRestoreModel));
			this.StartDeleteModelCommand = new RelayCommand(new Action<object>(this.StartDeleteModel));
			this.StartDeleteMarkedImagesCommand = new RelayCommand(new Action<object>(this.StartDeleteMarkedImages));
			this.btnStartTrain = new Button();
			this.btnDeleteMarkedImages = new Button();
			this.btnMarkAll = new Button();
			this.btnSimulateFromUsb = new Button();
			this.btnRestoreModel = new Button();
			this.btnDeleteModel = new Button();
			this.imageForComplianceMode = new XRSImage();
			this.txtSelectedProduct = new TextBlock();
			this.notInitView = false;
			this.AllImagesAreMarked = false;
			this.SelectedItemCount = "0";
			this.ShowRejectImages = true;
			this.ShowOkImages = false;
			this.ShowAllImages = false;
			this.checkContCountIsEnable = (parHandling.ReadPar(Definition.contaminationsCounterEnablePath) == "1");
			this.timerCheckHistoryLoading = new Timer(new TimerCallback(this.timer_CheckHistoryLoading), null, 0, 500);
			this.simulationmode = parHandling.ReadPar(Definition.simulationmodelPath);
			this.SelectedPrievewItems = new List<PreviewItem>();
			base.Preview.FirstItemCreated += this.Preview_FirstItemCreated;
			this.numImagesTrainDataset = this.GetNumImagesTrainDataset();
			bool flag = this.numImagesTrainDataset > 0;
			if (flag)
			{
				this.minSelectedImages = 10;
			}
			else
			{
				this.minSelectedImages = 25;
			}
			this.isTestcardSimActive = (parHandling.ReadPar(Definition.testcardSimulationPath) == "1" && parHandling.ReadPar(Definition.simulationAfterAutolearn) == "1");
			imageTransfer.FrameArrived += this.imageTestcardSimTransfer_FrameArrived;
			this.timerCheckErrorTestcardSim = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, new EventHandler(this.checkErrorsTestcardSim), Application.Current.Dispatcher);
			this.timerCheckErrorTestcardSim.Stop();
			this.timerCounterTestcardSim = 0;
			this.RegisterLocalizedText();
		}

		// Token: 0x060014CA RID: 5322 RVA: 0x0007B4FC File Offset: 0x000796FC
		private void RegisterLocalizedText()
		{
			this.aiService.UpdateAvailabilityModelFiles();
			bool flag = this.parHandling.ReadPar(Definition.anomalyDetectionWeightsInferenceAvailable) == "1";
			if (flag)
			{
				this.StrFineTuningMessage = new BoundLocalizedText("txtFineTuningMin");
			}
			else
			{
				this.StrFineTuningMessage = new BoundLocalizedText("txtFineTuningMissingFile");
			}
			bool flag2 = Definition.windowWidth >= 800.0;
			if (flag2)
			{
				this.txtSelectedProduct.Text = "P " + this.productManager.CurrentLoadedProduct.ProductNumber + ": " + this.productManager.CurrentLoadedProduct.ProductName;
			}
			else
			{
				this.txtSelectedProduct.Text = "P " + this.productManager.CurrentLoadedProduct.ProductNumber;
			}
		}

		// Token: 0x060014CB RID: 5323 RVA: 0x0007B5D2 File Offset: 0x000797D2
		public void DeregisterLocalizedText()
		{
			MainWindow.localizationManager.Deregister(this.StrFineTuningMessage);
		}

		// Token: 0x060014CC RID: 5324 RVA: 0x0007B5E8 File Offset: 0x000797E8
		private void UpdateFineTuningMsg()
		{
			bool flag = this.StrFineTuningMessage != null;
			if (flag)
			{
				bool flag2 = this.ShowDatasetImages || (!this.ShowAllImages && !this.ShowOkImages && !this.ShowRejectImages);
				if (flag2)
				{
					this.StrFineTuningMessage.ResourceKey = "txtFineTuningBaseDataset";
				}
				else
				{
					bool flag3 = this.parHandling.ReadPar(Definition.anomalyDetectionWeightsInferenceAvailable) == "1";
					if (flag3)
					{
						this.StrFineTuningMessage.ResourceKey = "txtFineTuningMin";
					}
					else
					{
						this.StrFineTuningMessage.ResourceKey = "txtFineTuningMissingFile";
					}
				}
				this.ShowFineTuningMessage = true;
			}
		}

		// Token: 0x060014CD RID: 5325 RVA: 0x0007B690 File Offset: 0x00079890
		public void Preview_FirstItemCreated()
		{
			bool flag = base.Preview.PreviewItems.Count > 0;
			if (flag)
			{
				this.SelectedPrievewItems.Add(base.Preview.PreviewItems[0]);
				this.FirstSelected = base.Preview.PreviewItems[0];
				this.OnPropertyChanged("FirstSelected");
			}
		}

		// Token: 0x060014CE RID: 5326 RVA: 0x0007B6F8 File Offset: 0x000798F8
		private void timer_CheckHistoryLoading(object obj)
		{
			bool isHistoryLoading = base.Preview.IsHistoryLoading;
			if (isHistoryLoading)
			{
				bool flag = this.showRejectImages;
				if (flag)
				{
					this.ButtonImageRejectsPath = "..\\img\\reject_orange_disabled.png";
				}
				else
				{
					bool flag2 = this.showOkImages;
					if (flag2)
					{
						this.ButtonImageOkPath = "..\\img\\checkmark_enabled_darker_big_disabled.png";
					}
					else
					{
						bool flag3 = this.showAllImages;
						if (flag3)
						{
							this.ButtonImageAllPath = "..\\img\\xriimagelist_small_disabled.png";
						}
						else
						{
							bool flag4 = this.showDatasetImages;
							if (flag4)
							{
								this.ButtonImageDatasetPath = "..\\img\\ai_dataset_disabled.png";
							}
						}
					}
				}
				this.HistoryisLoading = true;
			}
			else
			{
				bool flag5 = this.showRejectImages;
				if (flag5)
				{
					this.ButtonImageRejectsPath = "..\\img\\reject_orange.png";
				}
				else
				{
					bool flag6 = this.showOkImages;
					if (flag6)
					{
						this.ButtonImageOkPath = "..\\img\\checkmark_enabled_darker_big.png";
					}
					else
					{
						bool flag7 = this.showAllImages;
						if (flag7)
						{
							this.ButtonImageAllPath = "..\\img\\xriimagelist_small.png";
						}
						else
						{
							bool flag8 = this.showDatasetImages;
							if (flag8)
							{
								this.ButtonImageDatasetPath = "..\\img\\ai_dataset.png";
							}
						}
					}
				}
				this.HistoryisLoading = false;
				this.timerCheckHistoryLoading.Change(-1, -1);
			}
		}

		// Token: 0x060014CF RID: 5327 RVA: 0x0007B800 File Offset: 0x00079A00
		public void MarkAllImages()
		{
			bool flag = this.AllImagesAreMarked;
			if (flag)
			{
				this.AllImagesAreMarked = false;
				bool flag2 = this.SelectedPrievewItems != null;
				if (flag2)
				{
					this.SelectedItemCount = Convert.ToString(this.SelectedPrievewItems.Count);
				}
				else
				{
					this.SelectedItemCount = "0";
				}
			}
			else
			{
				this.AllImagesAreMarked = true;
				this.SelectedItemCount = Convert.ToString(base.Preview.PreviewItems.Count);
			}
		}

		// Token: 0x060014D0 RID: 5328 RVA: 0x0007B87C File Offset: 0x00079A7C
		private void bgTrainingAI_DoWork(object sender, DoWorkEventArgs e)
		{
			this.BackupModel();
			int num = Convert.ToInt32(this.SelectedItemCount);
			int value = num + this.numImagesTrainDataset;
			this.parHandling.WriteValue(Definition.anomalyDetectionNumberImagesFineTuning, Convert.ToString(value));
			this.parHandling.WriteValue(Definition.anomalyDetectionTrainEnabled, "1");
			for (int i = 0; i < num; i++)
			{
				bool flag = this.SelectedPrievewItems[i].XrsImage == null;
				if (flag)
				{
					this.SelectedPrievewItems[i].LoadXRSImage();
				}
				this.sendAITrainImages(this.SelectedPrievewItems[i].XrsImage);
			}
		}

		// Token: 0x060014D1 RID: 5329 RVA: 0x0007B92C File Offset: 0x00079B2C
		private void bgTrainingAI_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			this.progressBgTrain.FormFadeOut.Begin();
			this.bgTraining.DoWork -= this.bgTrainingAI_DoWork;
			this.bgTraining.ProgressChanged -= this.bgTrainingAI_ProgressChanged;
			this.bgTraining.RunWorkerCompleted -= this.bgTrainingAI_RunWorkerCompleted;
		}

		// Token: 0x060014D2 RID: 5330 RVA: 0x0007B993 File Offset: 0x00079B93
		private void bgTrainingAI_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			this.progressBgTrain.progBar.Value = (double)e.ProgressPercentage;
		}

		// Token: 0x060014D3 RID: 5331 RVA: 0x0007B9B0 File Offset: 0x00079BB0
		private void StartTrain(object obj)
		{
			bool flag = true;
			string text = "";
			bool flag2 = this.CheckForBackup() == "" && !this.aiService.CheckForFreeMemory();
			if (flag2)
			{
				flag = false;
			}
			else
			{
				bool flag3 = this.parHandling.ReadPar(XrayState.xrayPowerOnPath) == "1";
				if (flag3)
				{
					Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtTurnXrayOff"), DialogStyle.Error, DialogButtons.OK);
					flag = false;
				}
				else
				{
					bool flag4 = this.parHandling.ReadPar(Definition.anomalyDetectionTrainingInProgress) == "1";
					if (flag4)
					{
						string number = this.parHandling.ReadPar(Definition.anomalyDetectionTrainModelID);
						this.dbConnection.GetProductNameWithNumber(number, ref text);
						string message = string.Concat(new string[]
						{
							LocalizationManager.Localize("txtAITrainingAbort1"),
							" ",
							text,
							Environment.NewLine,
							" ",
							LocalizationManager.Localize("txtAITrainingAbort2")
						});
						flag = (Dialog.Show(LocalizationManager.Localize("txtWarning"), message, DialogStyle.Warning, DialogButtons.OKCancel) == Dialog.DialogResults.Ok);
					}
				}
			}
			bool flag5 = flag && Dialog.Show(LocalizationManager.Localize("txtInfo"), LocalizationManager.Localize("txtAIStartFinetuning"), DialogStyle.Info, DialogButtons.OKCancel) == Dialog.DialogResults.Ok;
			if (flag5)
			{
				this.parHandling.WriteValue(Definition.anomalyDetectionTrainEnabled, "0");
				this.dbConnection.SetProductTableNameTrainingAI("");
				this.parHandling.WriteValue(Definition.anomalyDetectionTrainingInProgress, "0");
				this.parHandling.WriteValue(Definition.anomalyDetectionTrainCurrentEpoch, "0");
				bool flag6 = this.parHandling.ReadPar(Definition.anomalyDetectionInferenceEnabled) == "1";
				this.parHandling.WriteValue(Definition.anomalyDetectionInferenceEnabled, "0");
				this.aiService.Comp19to30OnOff(false);
				this.btnStartTrain.IsEnabled = false;
				this.btnStartTrain.Content = ImageConverter.ToImage("/img/retrain_disabled.png");
				LogbookManager.AddEntry(SSTEntryCode.AIDetectionActivated, 1, this.productManager.CurrentLoadedProduct.ProductNumber, this.productManager.CurrentLoadedProduct.ProductName);
				bool flag7 = this.testcardSimService != null && this.isTestcardSimActive && !this.productManager.CurrentLoadedProduct.IsRegionProduct && this.parHandling.ReadPar(Definition.autoKVAdjustmentPath) == "1" && flag6;
				if (flag7)
				{
					this.OnSendSimTesctardImages();
					this.timerCheckErrorTestcardSim.Start();
				}
				else
				{
					this.sendImagesforFinetuning();
				}
			}
		}

		// Token: 0x060014D4 RID: 5332 RVA: 0x0007BC60 File Offset: 0x00079E60
		private void sendAITrainImages(XRSImage image)
		{
			XRSImage xrsimage = image.DeepCopy();
			xrsimage.Type = ImageType.TrainAI;
			this.imageTransfer.SendImage(xrsimage);
		}

		// Token: 0x060014D5 RID: 5333 RVA: 0x0007BC8C File Offset: 0x00079E8C
		private int GetNumImagesTrainDataset()
		{
			string tableName = "";
			this.dbConnection.GetCurrentProductTableNameAI(ref tableName);
			string archiveDirectoryAnomalyDetection = this.imageArchiver.GetArchiveDirectoryAnomalyDetection(tableName);
			bool flag = Directory.Exists(archiveDirectoryAnomalyDetection);
			int result;
			if (flag)
			{
				result = Directory.GetFiles(archiveDirectoryAnomalyDetection).Length;
			}
			else
			{
				result = 0;
			}
			return result;
		}

		// Token: 0x060014D6 RID: 5334 RVA: 0x0007BCD8 File Offset: 0x00079ED8
		private void BackupModel()
		{
			string str = "";
			this.dbConnection.GetCurrentProductTableNameAI(ref str);
			string text = Path.Combine(new string[]
			{
				ModelTransfer.archiveRoot + str
			});
			string[] directories = Directory.GetDirectories(text, "Backup_AI_*");
			foreach (string path in directories)
			{
				Directory.Delete(path, true);
			}
			string text2 = Path.Combine(text, "Backup_AI_" + DateTime.Now.ToString(AIViewModel.timeFormatString));
			Directory.CreateDirectory(text2);
			string[] files = Directory.GetFiles(text);
			foreach (string text3 in files)
			{
				string fileName = Path.GetFileName(text3);
				bool flag = AIViewModel.filesToIgnore.Contains(fileName);
				if (!flag)
				{
					string destFileName = Path.Combine(text2, fileName);
					File.Copy(text3, destFileName);
				}
			}
			List<string> list = new List<string>();
			for (int k = 0; k < Definition.nAIPostproFilters; k++)
			{
				string str2 = string.Format("{0}{1}{2}{3}{4}", new object[]
				{
					Definition.filterMainPath,
					Definition.anomalyDetectionMain,
					Definition.anomalyDetectionInference,
					Definition.aipostproFilterSubPath,
					k + 1
				});
				string text4 = this.parHandling.ReadPar(str2 + Definition.anomalyDetectionInferenceThresholdGroupContour + "/" + Definition.strValue);
				string text5 = this.parHandling.ReadPar(str2 + Definition.anomalyDetectionInferenceThresholdGroupInside + "/" + Definition.strValue);
				string text6 = this.parHandling.ReadPar(str2 + Definition.anomalyDetectionInferenceThresholdReconErrorContour + "/" + Definition.strValue);
				string text7 = this.parHandling.ReadPar(str2 + Definition.anomalyDetectionInferenceThresholdReconErrorInside + "/" + Definition.strValue);
				list.Add(string.Format("Filter {0}:\n{1}\n{2}\n{3}\n{4}\n", new object[]
				{
					k + 1,
					text4,
					text5,
					text6,
					text7
				}));
			}
			File.WriteAllText(Path.Combine(text2, "thresholds.txt"), string.Join("\n", list));
		}

		// Token: 0x060014D7 RID: 5335 RVA: 0x0007BF18 File Offset: 0x0007A118
		public string CheckForBackup()
		{
			string str = "";
			this.dbConnection.GetCurrentProductTableNameAI(ref str);
			string path = Path.Combine(new string[]
			{
				ModelTransfer.archiveRoot + str
			});
			bool flag = Directory.Exists(path);
			if (flag)
			{
				string[] directories = Directory.GetDirectories(path, "Backup_AI_*");
				foreach (string text in directories)
				{
					string path2 = "";
					string text2 = this.parHandling.ReadPar(Definition.machineSubTypePath);
					bool flag2 = text2.StartsWith("HX");
					if (flag2)
					{
						path2 = Path.Combine(text, "modelHX.weights");
					}
					else
					{
						bool flag3 = text2.StartsWith("TX");
						if (flag3)
						{
							path2 = Path.Combine(text, "modelTX.weights");
						}
					}
					string path3 = Path.Combine(text, "model.engine");
					bool flag4 = File.Exists(path2) && File.Exists(path3);
					if (flag4)
					{
						return text;
					}
				}
			}
			return "";
		}

		// Token: 0x060014D8 RID: 5336 RVA: 0x0007C02C File Offset: 0x0007A22C
		private void StartRestoreModel(object obj)
		{
			bool flag = this.parHandling.ReadPar(XrayState.xrayPowerOnPath) == "1" && this.parHandling.ReadPar(Definition.anomalyDetectionInferenceEnabled) == "1";
			string message;
			bool flag3;
			if (flag)
			{
				Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtTurnXrayOff"), DialogStyle.Error, DialogButtons.OK);
			}
			else
			{
				bool flag2 = this.parHandling.ReadPar(Definition.anomalyDetectionTrainModelID) == this.parHandling.ReadPar(Definition.anomalyDetectionInferenceModelID) && this.parHandling.ReadPar(Definition.anomalyDetectionTrainingInProgress) == "1";
				if (flag2)
				{
					message = LocalizationManager.Localize("txtAITrainingAbort3");
					flag3 = (Dialog.Show(LocalizationManager.Localize("txtWarning"), message, DialogStyle.Warning, DialogButtons.OKCancel) == Dialog.DialogResults.Ok);
					bool flag4 = !flag3;
					if (flag4)
					{
						return;
					}
					this.parHandling.WriteValue(Definition.anomalyDetectionTrainEnabled, "0");
				}
			}
			message = LocalizationManager.Localize("txtAITrainingRestore");
			flag3 = (Dialog.Show(LocalizationManager.Localize("txtWarning"), message, DialogStyle.Warning, DialogButtons.OKCancel) == Dialog.DialogResults.Ok);
			bool flag5 = flag3;
			if (flag5)
			{
				string description = LocalizationManager.Localize("txtAITrainingRestoreLoad") + "...";
				this.progressBgRestoreModel = new Progress(description, false);
				this.progressBgRestoreModel.Show();
				this.bgRestoreModel = new BackgroundWorker();
				this.bgRestoreModel.WorkerReportsProgress = true;
				this.bgRestoreModel.WorkerSupportsCancellation = true;
				this.bgRestoreModel.DoWork += this.bgRestoreModel_DoWork;
				this.bgRestoreModel.ProgressChanged += this.bgRestoreModel_ProgressChanged;
				this.bgRestoreModel.RunWorkerCompleted += this.bgRestoreModel_RunWorkerCompleted;
				this.bgRestoreModel.RunWorkerAsync();
			}
		}

		// Token: 0x060014D9 RID: 5337 RVA: 0x0007C210 File Offset: 0x0007A410
		private void bgRestoreModel_DoWork(object sender, DoWorkEventArgs e)
		{
			string text = this.CheckForBackup();
			bool flag = text != "";
			if (flag)
			{
				string value = this.parHandling.ReadPar(Definition.anomalyDetectionInferenceEnabled);
				this.parHandling.WriteValue(Definition.anomalyDetectionInferenceEnabled, "0");
				DirectoryInfo parent = Directory.GetParent(text);
				string path = (parent != null) ? parent.FullName : null;
				foreach (string text2 in Directory.GetFiles(text))
				{
					string fileName = Path.GetFileName(text2);
					string destFileName = Path.Combine(path, fileName);
					File.Copy(text2, destFileName, true);
				}
				string path2 = Path.Combine(path, "thresholds.txt");
				bool flag2 = File.Exists(path2);
				if (flag2)
				{
					string[] array = File.ReadAllLines(path2);
					int num = 0;
					for (int j = 0; j < Definition.nAIPostproFilters; j++)
					{
						string str = string.Format("{0}{1}{2}{3}{4}", new object[]
						{
							Definition.filterMainPath,
							Definition.anomalyDetectionMain,
							Definition.anomalyDetectionInference,
							Definition.aipostproFilterSubPath,
							j + 1
						});
						while (num < array.Length && (string.IsNullOrWhiteSpace(array[num]) || array[num].StartsWith("Filter")))
						{
							num++;
						}
						int num2;
						bool flag3 = num < array.Length && int.TryParse(array[num], out num2);
						if (flag3)
						{
							this.parHandling.WriteValue(str + Definition.anomalyDetectionInferenceThresholdGroupContour + "/" + Definition.strValue, array[num]);
						}
						num++;
						bool flag4 = num < array.Length && int.TryParse(array[num], out num2);
						if (flag4)
						{
							this.parHandling.WriteValue(str + Definition.anomalyDetectionInferenceThresholdGroupInside + "/" + Definition.strValue, array[num]);
						}
						num++;
						bool flag5 = num < array.Length && int.TryParse(array[num], out num2);
						if (flag5)
						{
							this.parHandling.WriteValue(str + Definition.anomalyDetectionInferenceThresholdReconErrorContour + "/" + Definition.strValue, array[num]);
						}
						num++;
						bool flag6 = num < array.Length && int.TryParse(array[num], out num2);
						if (flag6)
						{
							this.parHandling.WriteValue(str + Definition.anomalyDetectionInferenceThresholdReconErrorInside + "/" + Definition.strValue, array[num]);
						}
						num++;
						bool flag7 = num >= array.Length;
						if (flag7)
						{
							break;
						}
					}
					File.Delete(path2);
				}
				this.parHandling.WriteValue(Definition.anomalyDetectionInferenceEnabled, value);
				Directory.Delete(text, true);
				LogbookManager.AddEntry(SSTEntryCode.AIDetectionActivated, 4, this.productManager.CurrentLoadedProduct.ProductNumber, this.productManager.CurrentLoadedProduct.ProductName);
			}
		}

		// Token: 0x060014DA RID: 5338 RVA: 0x0007C504 File Offset: 0x0007A704
		private void bgRestoreModel_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			this.progressBgRestoreModel.FormFadeOut.Begin();
			this.bgRestoreModel.DoWork -= this.bgRestoreModel_DoWork;
			this.bgRestoreModel.ProgressChanged -= this.bgRestoreModel_ProgressChanged;
			this.bgRestoreModel.RunWorkerCompleted -= this.bgRestoreModel_RunWorkerCompleted;
			Dialog.Show(LocalizationManager.Localize("txtInfo"), LocalizationManager.Localize("txtAITrainingRestoreDone"), DialogStyle.Info);
			this.btnRestoreModel.IsEnabled = false;
			this.btnRestoreModel.Content = ImageConverter.ToImage("/img/restoreAI_disabled.png");
		}

		// Token: 0x060014DB RID: 5339 RVA: 0x0007C5A9 File Offset: 0x0007A7A9
		private void bgRestoreModel_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			this.progressBgRestoreModel.progBar.Value = (double)e.ProgressPercentage;
		}

		// Token: 0x060014DC RID: 5340 RVA: 0x0007C5C4 File Offset: 0x0007A7C4
		private void StartDeleteModel(object obj)
		{
			bool flag = this.parHandling.ReadPar(Definition.anomalyDetectionTrainModelID) == this.parHandling.ReadPar(Definition.anomalyDetectionInferenceModelID) && this.parHandling.ReadPar(Definition.anomalyDetectionTrainingInProgress) == "1";
			bool flag2;
			if (flag)
			{
				string message = LocalizationManager.Localize("txtAITrainingAbort4");
				flag2 = (Dialog.Show(LocalizationManager.Localize("txtWarning"), message, DialogStyle.Warning, DialogButtons.OKCancel) == Dialog.DialogResults.Ok);
				bool flag3 = !flag2;
				if (flag3)
				{
					return;
				}
				this.parHandling.WriteValue(Definition.anomalyDetectionTrainEnabled, "0");
			}
			else
			{
				string message = LocalizationManager.Localize("txtAITrainingDelete");
				flag2 = (Dialog.Show(LocalizationManager.Localize("txtWarning"), message, DialogStyle.Warning, DialogButtons.OKCancel) == Dialog.DialogResults.Ok);
			}
			bool flag4 = flag2;
			if (flag4)
			{
				string description = LocalizationManager.Localize("txtAITrainingDeleteActiv") + "...";
				this.progressBgDeleteModel = new Progress(description, false);
				this.progressBgDeleteModel.Show();
				this.bgDeleteModel = new BackgroundWorker();
				this.bgDeleteModel.WorkerReportsProgress = true;
				this.bgDeleteModel.WorkerSupportsCancellation = true;
				this.bgDeleteModel.DoWork += this.bgDeleteModel_DoWork;
				this.bgDeleteModel.ProgressChanged += this.bgDeleteModel_ProgressChanged;
				this.bgDeleteModel.RunWorkerCompleted += this.bgDeleteModel_RunWorkerCompleted;
				this.bgDeleteModel.RunWorkerAsync();
			}
		}

		// Token: 0x060014DD RID: 5341 RVA: 0x0007C748 File Offset: 0x0007A948
		private void bgDeleteModel_DoWork(object sender, DoWorkEventArgs e)
		{
			string text = "";
			this.dbConnection.GetCurrentProductTableNameAI(ref text);
			string path = Path.Combine(new string[]
			{
				ModelTransfer.archiveRoot + text
			});
			bool flag = Directory.Exists(path);
			if (flag)
			{
				string[] files = Directory.GetFiles(path);
				string[] array = files;
				for (int i = 0; i < array.Length; i++)
				{
					string file = array[i];
					bool flag2 = !AIViewModel.filesToIgnore.Any((string ignoreName) => file.EndsWith(ignoreName, StringComparison.OrdinalIgnoreCase));
					if (flag2)
					{
						File.Delete(file);
					}
				}
				string[] directories = Directory.GetDirectories(path, "Backup_AI_*", SearchOption.TopDirectoryOnly);
				foreach (string path2 in directories)
				{
					Directory.Delete(path2, true);
				}
			}
			path = this.imageArchiver.GetArchiveDirectoryAnomalyDetection(text);
			bool flag3 = Directory.Exists(path);
			if (flag3)
			{
				Directory.Delete(path, true);
			}
			LogbookManager.AddEntry(SSTEntryCode.AIDetectionDeactivated, 4, this.productManager.CurrentLoadedProduct.ProductNumber, this.productManager.CurrentLoadedProduct.ProductName);
			bool checkStart = true;
			string message = LocalizationManager.Localize("txtAITrainingDeleteDone");
			bool flag4 = Application.Current != null;
			if (flag4)
			{
				Application.Current.Dispatcher.Invoke(delegate()
				{
					checkStart = (Dialog.Show(LocalizationManager.Localize("txtInfo"), message, DialogStyle.Info, DialogButtons.OKCancel) == Dialog.DialogResults.Ok);
				});
			}
			bool flag5 = !checkStart;
			if (!flag5)
			{
				bool flag6 = this.parHandling.ReadPar(Definition.anomalyDetectionTrainingInProgress) == "1";
				if (flag6)
				{
					string text2 = "";
					string number = this.parHandling.ReadPar(Definition.anomalyDetectionTrainModelID);
					this.dbConnection.GetProductNameWithNumber(number, ref text2);
					message = string.Concat(new string[]
					{
						LocalizationManager.Localize("txtAITrainingAbort1"),
						" ",
						text2,
						Environment.NewLine,
						" ",
						LocalizationManager.Localize("txtAITrainingAbort2")
					});
					bool flag7 = Application.Current != null;
					if (flag7)
					{
						Application.Current.Dispatcher.Invoke(delegate()
						{
							checkStart = (Dialog.Show(LocalizationManager.Localize("txtWarning"), message, DialogStyle.Warning, DialogButtons.OKCancel) == Dialog.DialogResults.Ok);
						});
					}
					bool flag8 = !checkStart;
					if (flag8)
					{
						return;
					}
				}
				this.parHandling.WriteValue(Definition.anomalyDetectionTrainEnabled, "0");
				this.dbConnection.SetProductTableNameTrainingAI("");
				this.parHandling.WriteValue(Definition.anomalyDetectionTrainingInProgress, "0");
				this.parHandling.WriteValue(Definition.anomalyDetectionTrainCurrentEpoch, "0");
				Thread.Sleep(1000);
				this.parHandling.WriteValue(Definition.anomalyDetectionTrainEnabled, "1");
				LogbookManager.AddEntry(SSTEntryCode.AIDetectionActivated, 1, this.productManager.CurrentLoadedProduct.ProductNumber, this.productManager.CurrentLoadedProduct.ProductName);
			}
		}

		// Token: 0x060014DE RID: 5342 RVA: 0x0007CA54 File Offset: 0x0007AC54
		private void bgDeleteModel_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			this.progressBgDeleteModel.FormFadeOut.Begin();
			this.bgDeleteModel.DoWork -= this.bgDeleteModel_DoWork;
			this.bgDeleteModel.ProgressChanged -= this.bgDeleteModel_ProgressChanged;
			this.bgDeleteModel.RunWorkerCompleted -= this.bgDeleteModel_RunWorkerCompleted;
			this.btnDeleteModel.IsEnabled = false;
			this.btnDeleteModel.Content = ImageConverter.ToImage("/img/deleteAI_disabled.png");
			this.btnStartTrain.IsEnabled = false;
			this.btnStartTrain.Content = ImageConverter.ToImage("/img/retrain_disabled.png");
			this.btnRestoreModel.IsEnabled = false;
			this.btnRestoreModel.Content = ImageConverter.ToImage("/img/restoreAI_disabled.png");
			this.StrFineTuningMessage.ResourceKey = "txtFineTuningMissingFile";
			this.ShowFineTuningMessage = true;
			this.parHandling.WriteValue(Definition.anomalyDetectionInferenceEnabled, "0");
			this.aiService.Comp19to30OnOff(false);
		}

		// Token: 0x060014DF RID: 5343 RVA: 0x0007CB60 File Offset: 0x0007AD60
		private void bgDeleteModel_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			this.progressBgDeleteModel.progBar.Value = (double)e.ProgressPercentage;
		}

		// Token: 0x060014E0 RID: 5344 RVA: 0x0007CB7C File Offset: 0x0007AD7C
		public void imageTestcardSimTransfer_FrameArrived(object sender, ReceiveEventArgs e)
		{
			bool flag = e.Image.Type == ImageType.TestcardSimulation;
			if (flag)
			{
				bool flag2 = Application.Current != null;
				if (flag2)
				{
					Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
					{
						bool flag3 = this.testcardSimService.TestcardSimAfterTrainingGetImg(e.Image, true);
						if (flag3)
						{
							this.progresstestcardSim.progBar.Value = 100.0;
							this.timerCheckErrorTestcardSim.Stop();
							this.timerCounterTestcardSim = 0;
							this.progresstestcardSim.Close();
							this.testcardSimService.setParameterAfterTestcardSim(false);
							bool flag4 = Dialog.Show(this.testcardSimService.setRetrainTestcardSimMsg(true), this.testcardSimService.setRetrainTestcardSimMsg(false), DialogStyle.Info, DialogButtons.OK) == Dialog.DialogResults.Ok;
							if (flag4)
							{
								bool flag5 = this.testcardSimService.CheckWrongDetectedPixel(0) && this.parHandling.ReadPar(Definition.anomalyDetectionInferenceEnabled) != "1";
								if (flag5)
								{
									Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtWrongPixelTestcardSim") + ": " + this.parHandling.ReadPar(Definition.simulationWrongDetPix), DialogStyle.Warning, DialogButtons.OK);
								}
								bool checkSensitivityDecrease = this.testcardSimService.CheckSensitivityDecrease;
								if (checkSensitivityDecrease)
								{
									Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtDecreasesTestcardSim"), DialogStyle.Warning, DialogButtons.OK);
								}
								this.sendImagesforFinetuning();
							}
						}
						else
						{
							this.testcardSimProgressCounter++;
							this.progresstestcardSim.progBar.Value = (double)(100 * this.testcardSimProgressCounter / (this.testcardSimService.RealImgCountForSim * Definition.sumOfAllTestcards));
						}
					}));
				}
			}
		}

		// Token: 0x060014E1 RID: 5345 RVA: 0x0007CBE4 File Offset: 0x0007ADE4
		private void sendImagesforFinetuning()
		{
			string description = LocalizationManager.Localize("txtAISendImages") + "...";
			this.progressBgTrain = new Progress(description, false);
			this.progressBgTrain.Show();
			this.bgTraining = new BackgroundWorker();
			this.bgTraining.WorkerReportsProgress = true;
			this.bgTraining.WorkerSupportsCancellation = true;
			this.bgTraining.DoWork += this.bgTrainingAI_DoWork;
			this.bgTraining.ProgressChanged += this.bgTrainingAI_ProgressChanged;
			this.bgTraining.RunWorkerCompleted += this.bgTrainingAI_RunWorkerCompleted;
			this.bgTraining.RunWorkerAsync();
		}

		// Token: 0x060014E2 RID: 5346 RVA: 0x0007CC9C File Offset: 0x0007AE9C
		public void initprogBarSendingTestcardSim()
		{
			bool flag = Application.Current != null;
			if (flag)
			{
				Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
				{
					this.testcardSimProgressCounter = 0;
					this.progresstestcardSim = new Progress(LocalizationManager.Localize("txtCalcSensTestcardSim") + "...", false);
					this.progresstestcardSim.Show();
					this.progresstestcardSim.progBar.Value = 1.0;
				}));
			}
		}

		// Token: 0x060014E3 RID: 5347 RVA: 0x0007CCD8 File Offset: 0x0007AED8
		protected virtual void OnSendSimTesctardImages()
		{
			Action sendTestcardSimImages = this.SendTestcardSimImages;
			bool flag = sendTestcardSimImages != null;
			if (flag)
			{
				sendTestcardSimImages();
			}
		}

		// Token: 0x060014E4 RID: 5348 RVA: 0x0007CD00 File Offset: 0x0007AF00
		private void checkErrorsTestcardSim(object sender, EventArgs e)
		{
			bool flag = this.testcardSimService != null && this.testcardSimService.CheckVersionAndXRaySource;
			if (flag)
			{
				this.progresstestcardSim.Close();
				this.timerCheckErrorTestcardSim.Stop();
				Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtTestcardSimNotPossible"), DialogStyle.Error, DialogButtons.OK);
				this.sendImagesforFinetuning();
			}
			bool flag2 = this.testcardSimService != null && this.testcardSimService.CheckBallCounts;
			if (flag2)
			{
				this.progresstestcardSim.Close();
				this.timerCheckErrorTestcardSim.Stop();
				Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtErrorGeneralTestcardSim"), DialogStyle.Warning, DialogButtons.OK);
				this.sendImagesforFinetuning();
			}
			bool flag3 = this.testcardSimService != null && !this.testcardSimService.ImagesAvailable;
			if (flag3)
			{
				this.progresstestcardSim.Close();
				this.timerCheckErrorTestcardSim.Stop();
				Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtTestcardSimNoImages"), DialogStyle.Warning, DialogButtons.OK);
			}
			bool flag4 = this.timerCounterTestcardSim > 180;
			if (flag4)
			{
				this.progresstestcardSim.Close();
				this.timerCheckErrorTestcardSim.Stop();
				Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtAbortTestcardSim"), DialogStyle.Error, DialogButtons.OK);
				this.sendImagesforFinetuning();
			}
			this.timerCounterTestcardSim++;
		}

		// Token: 0x060014E5 RID: 5349 RVA: 0x0007CE6C File Offset: 0x0007B06C
		private void StartDeleteMarkedImages(object obj)
		{
			int num = Convert.ToInt32(this.SelectedItemCount);
			bool flag = num > 0 && this.SelectedPrievewItems != null;
			if (flag)
			{
				bool flag2 = this.parHandling.ReadPar(Definition.anomalyDetectionTrainEnabled) == "1" && this.parHandling.ReadPar(Definition.anomalyDetectionTrainModelID) == this.parHandling.ReadPar(Definition.anomalyDetectionInferenceModelID) && this.ShowDatasetImages;
				bool flag3 = flag2;
				string resourceKey;
				if (flag3)
				{
					resourceKey = "txtAIDeleteSelected";
				}
				else
				{
					resourceKey = "txtDeleteSelected";
				}
				bool flag4 = base.Preview.PreviewItems.Count > 0 && Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize(resourceKey) + "?", DialogStyle.Warning, DialogButtons.OKCancel) == Dialog.DialogResults.Ok;
				if (flag4)
				{
					bool flag5 = flag2;
					if (flag5)
					{
						this.parHandling.WriteValue(Definition.anomalyDetectionTrainEnabled, "0");
					}
					bool flag6 = this.AllImagesAreMarked || num == base.Preview.PreviewItems.Count;
					if (flag6)
					{
						for (int i = 0; i < base.Preview.PreviewItems.Count; i++)
						{
							bool flag7 = File.Exists(base.Preview.PreviewItems[i].FileName);
							if (flag7)
							{
								File.Delete(base.Preview.PreviewItems[i].FileName);
							}
						}
						base.Preview.PreviewItems.Clear();
						this.AllImagesAreMarked = false;
						this.SelectedItemCount = "0";
						base.SelectedImage = null;
						base.Preview.AbortCreation();
						this.imagebox.Reset();
					}
					else
					{
						bool flag8 = base.Preview.PreviewItems != null;
						if (flag8)
						{
							this.progressBgDeleteMarkedImages = new Progress(LocalizationManager.Localize("txtDeleteSelected"), false);
							this.progressBgDeleteMarkedImages.Show();
							this.bgDeleteMarkedImages = new BackgroundWorker();
							this.bgDeleteMarkedImages.WorkerReportsProgress = true;
							this.bgDeleteMarkedImages.DoWork += this.bgDeleteMarkedImages_DoWork;
							this.bgDeleteMarkedImages.WorkerSupportsCancellation = true;
							this.bgDeleteMarkedImages.RunWorkerCompleted += this.bgDeleteMarkedImages_RunWorkerCompleted;
							this.bgDeleteMarkedImages.ProgressChanged += this.bgDeleteMarkedImages_ProgressChanged;
							this.bgDeleteMarkedImages.RunWorkerAsync();
						}
					}
				}
			}
		}

		// Token: 0x060014E6 RID: 5350 RVA: 0x0007D0F8 File Offset: 0x0007B2F8
		private void bgDeleteMarkedImages_DoWork(object sender, DoWorkEventArgs e)
		{
			List<PreviewItem> selectedPrievewItems = this.SelectedPrievewItems;
			for (int i = 0; i < selectedPrievewItems.Count; i++)
			{
				int percentProgress = 100 * i / selectedPrievewItems.Count<PreviewItem>();
				this.bgDeleteMarkedImages.ReportProgress(percentProgress);
				int k;
				int j;
				for (j = 0; j < base.Preview.PreviewItems.Count; j = k + 1)
				{
					bool flag = selectedPrievewItems[i].Timestamp == base.Preview.PreviewItems[j].Timestamp;
					if (flag)
					{
						bool flag2 = File.Exists(base.Preview.PreviewItems[j].FileName);
						if (flag2)
						{
							File.Delete(base.Preview.PreviewItems[j].FileName);
						}
						bool flag3 = Application.Current != null;
						if (flag3)
						{
							Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
							{
								this.Preview.PreviewItems.RemoveAt(j);
								j = this.Preview.PreviewItems.Count;
							}));
						}
					}
					k = j;
				}
			}
			this.SelectedPrievewItems.Clear();
			bool flag4 = base.Preview.PreviewItems.Count > 0;
			if (flag4)
			{
				base.SelectedImage = base.Preview.PreviewItems[0];
				this.SelectedPrievewItems.Add(base.SelectedImage);
			}
			else
			{
				base.SelectedImage = null;
			}
		}

		// Token: 0x060014E7 RID: 5351 RVA: 0x0007D299 File Offset: 0x0007B499
		private void bgDeleteMarkedImages_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			this.progressBgDeleteMarkedImages.Close();
		}

		// Token: 0x060014E8 RID: 5352 RVA: 0x0007D2A8 File Offset: 0x0007B4A8
		private void bgDeleteMarkedImages_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			bool flag = this.progressBgDeleteMarkedImages != null;
			if (flag)
			{
				this.progressBgDeleteMarkedImages.progBar.Value = (double)e.ProgressPercentage;
			}
		}

		// Token: 0x04000DA2 RID: 3490
		private const string BackupFolder = "Backup_AI_";

		// Token: 0x04000DA3 RID: 3491
		public const string BackupFolderPattern = "Backup_AI_*";

		// Token: 0x04000DA4 RID: 3492
		public static string[] filesToIgnore = new string[]
		{
			"modelXO.engine",
			"modelXO.onnx",
			AIObjectViewModel.fileClassNames
		};

		// Token: 0x04000DAB RID: 3499
		private ParameterHandling parHandling;

		// Token: 0x04000DAC RID: 3500
		private bool allImagesAreMarked;

		// Token: 0x04000DAD RID: 3501
		private bool historyisLoading;

		// Token: 0x04000DAE RID: 3502
		private BackgroundWorker bgTraining;

		// Token: 0x04000DAF RID: 3503
		private BackgroundWorker bgRestoreModel;

		// Token: 0x04000DB0 RID: 3504
		private BackgroundWorker bgDeleteModel;

		// Token: 0x04000DB1 RID: 3505
		private BackgroundWorker bgDeleteMarkedImages;

		// Token: 0x04000DB2 RID: 3506
		private StoreImageData imageArchiver;

		// Token: 0x04000DB3 RID: 3507
		public Button btnStartTrain;

		// Token: 0x04000DB4 RID: 3508
		public Button btnSimulateFromUsb;

		// Token: 0x04000DB5 RID: 3509
		public Button btnDeleteMarkedImages;

		// Token: 0x04000DB6 RID: 3510
		public Button btnMarkAll;

		// Token: 0x04000DB7 RID: 3511
		public Button btnRestoreModel;

		// Token: 0x04000DB8 RID: 3512
		public Button btnDeleteModel;

		// Token: 0x04000DB9 RID: 3513
		public TextBlock txtSelectedProduct;

		// Token: 0x04000DBA RID: 3514
		private ImageDataTransfer imageTransfer;

		// Token: 0x04000DBB RID: 3515
		private DBConnection dbConnection;

		// Token: 0x04000DBC RID: 3516
		private string selectedItemCount;

		// Token: 0x04000DBD RID: 3517
		private bool notInitView;

		// Token: 0x04000DBE RID: 3518
		public Timer timerCheckHistoryLoading;

		// Token: 0x04000DBF RID: 3519
		private int simulatedImagesTotalCounts;

		// Token: 0x04000DC0 RID: 3520
		private int simulatedImagesCounter;

		// Token: 0x04000DC1 RID: 3521
		public bool checkContCountIsEnable;

		// Token: 0x04000DC2 RID: 3522
		private bool simImageArrived = false;

		// Token: 0x04000DC3 RID: 3523
		private ProductManager productManager;

		// Token: 0x04000DC4 RID: 3524
		private string simulationmode;

		// Token: 0x04000DC5 RID: 3525
		private ErrorHandling errorHandling;

		// Token: 0x04000DC6 RID: 3526
		private Progress progressBgTrain;

		// Token: 0x04000DC7 RID: 3527
		private Progress progressBgRestoreModel;

		// Token: 0x04000DC8 RID: 3528
		private Progress progressBgDeleteModel;

		// Token: 0x04000DC9 RID: 3529
		private Progress progressBgDeleteMarkedImages;

		// Token: 0x04000DCA RID: 3530
		private AIService aiService;

		// Token: 0x04000DCB RID: 3531
		private AccessRightsManager accessManager;

		// Token: 0x04000DCC RID: 3532
		public static readonly string timeFormatString = "yyyy_MM_dd_HH_mm_ss_ff";

		// Token: 0x04000DCD RID: 3533
		private bool isTestcardSimActive = false;

		// Token: 0x04000DCE RID: 3534
		private TestcardSimService testcardSimService;

		// Token: 0x04000DCF RID: 3535
		public DispatcherTimer timerCheckErrorTestcardSim;

		// Token: 0x04000DD0 RID: 3536
		private Progress progresstestcardSim;

		// Token: 0x04000DD1 RID: 3537
		private int testcardSimProgressCounter;

		// Token: 0x04000DD2 RID: 3538
		private int timerCounterTestcardSim;

		// Token: 0x04000DD4 RID: 3540
		private string buttonImageRejectsPath;

		// Token: 0x04000DD5 RID: 3541
		private string buttonImageOkPath;

		// Token: 0x04000DD6 RID: 3542
		private string buttonImageAllPath;

		// Token: 0x04000DD7 RID: 3543
		private string buttonImageDatasetPath;

		// Token: 0x04000DD8 RID: 3544
		private bool showRejectImages;

		// Token: 0x04000DD9 RID: 3545
		private bool showOkImages;

		// Token: 0x04000DDA RID: 3546
		private bool showAllImages;

		// Token: 0x04000DDB RID: 3547
		private bool showDatasetImages;

		// Token: 0x04000DDC RID: 3548
		public static List<int> VASensitivities;

		// Token: 0x04000DDD RID: 3549
		public static List<int> GlassSensitivities;

		// Token: 0x04000DDE RID: 3550
		public static List<string> ImgNamesForSensitivities;

		// Token: 0x04000DDF RID: 3551
		private XRSImage imageForComplianceMode;

		// Token: 0x04000DE1 RID: 3553
		private int numImagesTrainDataset;

		// Token: 0x04000DE2 RID: 3554
		public int minSelectedImages;

		// Token: 0x04000DE3 RID: 3555
		private bool showFineTuningMessage = true;
	}
}
