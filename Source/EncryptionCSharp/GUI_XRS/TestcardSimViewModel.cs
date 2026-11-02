using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using Common;
using DatabaseConnection;
using Definitions;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x020000EB RID: 235
	public class TestcardSimViewModel : PreviewViewModel
	{
		// Token: 0x170006FF RID: 1791
		// (get) Token: 0x06001723 RID: 5923 RVA: 0x0008C01E File Offset: 0x0008A21E
		// (set) Token: 0x06001724 RID: 5924 RVA: 0x0008C026 File Offset: 0x0008A226
		public RelayCommand ShowSettingsCommand { get; set; }

		// Token: 0x17000700 RID: 1792
		// (get) Token: 0x06001725 RID: 5925 RVA: 0x0008C02F File Offset: 0x0008A22F
		// (set) Token: 0x06001726 RID: 5926 RVA: 0x0008C037 File Offset: 0x0008A237
		public RelayCommand ShowStatisticsCommand { get; set; }

		// Token: 0x17000701 RID: 1793
		// (get) Token: 0x06001727 RID: 5927 RVA: 0x0008C040 File Offset: 0x0008A240
		// (set) Token: 0x06001728 RID: 5928 RVA: 0x0008C048 File Offset: 0x0008A248
		public RelayCommand ShowIntegratedComplianceMonitoring { get; set; }

		// Token: 0x17000702 RID: 1794
		// (get) Token: 0x06001729 RID: 5929 RVA: 0x0008C051 File Offset: 0x0008A251
		// (set) Token: 0x0600172A RID: 5930 RVA: 0x0008C059 File Offset: 0x0008A259
		public RelayCommand ShowHistogramCommand { get; set; }

		// Token: 0x17000703 RID: 1795
		// (get) Token: 0x0600172B RID: 5931 RVA: 0x0008C062 File Offset: 0x0008A262
		// (set) Token: 0x0600172C RID: 5932 RVA: 0x0008C06A File Offset: 0x0008A26A
		public RelayCommand StartSimulationCommand { get; set; }

		// Token: 0x17000704 RID: 1796
		// (get) Token: 0x0600172D RID: 5933 RVA: 0x0008C073 File Offset: 0x0008A273
		// (set) Token: 0x0600172E RID: 5934 RVA: 0x0008C07B File Offset: 0x0008A27B
		public List<PreviewItem> SelectedPrievewItems { get; set; }

		// Token: 0x17000705 RID: 1797
		// (get) Token: 0x0600172F RID: 5935 RVA: 0x0008C084 File Offset: 0x0008A284
		// (set) Token: 0x06001730 RID: 5936 RVA: 0x0008C09C File Offset: 0x0008A29C
		public bool StopVASimulation
		{
			get
			{
				return this.stopVASimulation;
			}
			set
			{
				bool flag = this.stopVASimulation != value;
				if (flag)
				{
					this.stopVASimulation = value;
					this.OnPropertyChanged("StopVASimulation");
				}
			}
		}

		// Token: 0x17000706 RID: 1798
		// (get) Token: 0x06001731 RID: 5937 RVA: 0x0008C0D0 File Offset: 0x0008A2D0
		// (set) Token: 0x06001732 RID: 5938 RVA: 0x0008C0E8 File Offset: 0x0008A2E8
		public bool StopGlassSimulation
		{
			get
			{
				return this.stopGlassSimulation;
			}
			set
			{
				bool flag = this.stopGlassSimulation != value;
				if (flag)
				{
					this.stopGlassSimulation = value;
					this.OnPropertyChanged("StopGlassSimulation");
				}
			}
		}

		// Token: 0x17000707 RID: 1799
		// (get) Token: 0x06001733 RID: 5939 RVA: 0x0008C11C File Offset: 0x0008A31C
		// (set) Token: 0x06001734 RID: 5940 RVA: 0x0008C134 File Offset: 0x0008A334
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

		// Token: 0x17000708 RID: 1800
		// (get) Token: 0x06001735 RID: 5941 RVA: 0x0008C168 File Offset: 0x0008A368
		// (set) Token: 0x06001736 RID: 5942 RVA: 0x0008C180 File Offset: 0x0008A380
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

		// Token: 0x17000709 RID: 1801
		// (get) Token: 0x06001737 RID: 5943 RVA: 0x0008C1B4 File Offset: 0x0008A3B4
		// (set) Token: 0x06001738 RID: 5944 RVA: 0x0008C1CC File Offset: 0x0008A3CC
		public string ButtonImageHistSourcePath
		{
			get
			{
				return this.buttonImageHistSourcePath;
			}
			set
			{
				bool flag = this.buttonImageHistSourcePath != value;
				if (flag)
				{
					this.buttonImageHistSourcePath = value;
					this.OnPropertyChanged("ButtonImageHistSourcePath");
				}
			}
		}

		// Token: 0x1700070A RID: 1802
		// (get) Token: 0x06001739 RID: 5945 RVA: 0x0008C200 File Offset: 0x0008A400
		// (set) Token: 0x0600173A RID: 5946 RVA: 0x0008C218 File Offset: 0x0008A418
		public bool ShowHistoryOrigSim
		{
			get
			{
				return this.showHistoryOrigSim;
			}
			set
			{
				bool flag = this.showHistoryOrigSim != value;
				if (flag)
				{
					this.showHistoryOrigSim = value;
					bool flag2 = this.showHistoryOrigSim;
					if (flag2)
					{
						this.ButtonImageHistSourcePath = "..\\img\\overview.png";
						base.Preview.IsOriginalHistTestcardSim = true;
						this.btnStartSimulation.Visibility = Visibility.Visible;
						this.btnSimulateFromUsb.Visibility = Visibility.Visible;
						this.btnDelete.Visibility = Visibility.Collapsed;
						this.btnMarkAll.Visibility = Visibility.Collapsed;
					}
					else
					{
						this.ButtonImageHistSourcePath = "..\\img\\simulateTestcard.png";
						base.Preview.IsOriginalHistTestcardSim = false;
						this.btnStartSimulation.Visibility = Visibility.Collapsed;
						this.btnSimulateFromUsb.Visibility = Visibility.Collapsed;
						this.btnDelete.Visibility = Visibility.Visible;
						this.btnMarkAll.Visibility = Visibility.Visible;
					}
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
					this.AllImagesAreMarked = false;
					this.OnPropertyChanged("ShowHistoryOrigSim");
				}
			}
		}

		// Token: 0x1700070B RID: 1803
		// (get) Token: 0x0600173B RID: 5947 RVA: 0x0008C380 File Offset: 0x0008A580
		// (set) Token: 0x0600173C RID: 5948 RVA: 0x0008C398 File Offset: 0x0008A598
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

		// Token: 0x1700070C RID: 1804
		// (get) Token: 0x0600173D RID: 5949 RVA: 0x0008C3CC File Offset: 0x0008A5CC
		// (set) Token: 0x0600173E RID: 5950 RVA: 0x0008C3E4 File Offset: 0x0008A5E4
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

		// Token: 0x1700070D RID: 1805
		// (get) Token: 0x0600173F RID: 5951 RVA: 0x0008C418 File Offset: 0x0008A618
		// (set) Token: 0x06001740 RID: 5952 RVA: 0x0008C430 File Offset: 0x0008A630
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

		// Token: 0x1700070E RID: 1806
		// (get) Token: 0x06001741 RID: 5953 RVA: 0x0008C463 File Offset: 0x0008A663
		// (set) Token: 0x06001742 RID: 5954 RVA: 0x0008C46B File Offset: 0x0008A66B
		public PreviewItem FirstSelected { get; set; }

		// Token: 0x06001743 RID: 5955 RVA: 0x0008C474 File Offset: 0x0008A674
		public TestcardSimViewModel(ParameterHandling parHandling, XRSImageBox imagebox, string[] imageFolders, StoreImageData imageArchiver, ImageDataTransfer imageTransfer, DBConnection dbConnection, ProductManager productManager, TestcardSimService testcardSimService, AccessRightsManager accessManager, ErrorHandling errorHandling) : base(imagebox, imageFolders, 300, Lane.All, 0, imageArchiver, true, false, "", -1)
		{
			this.parHandling = parHandling;
			this.imageArchiver = imageArchiver;
			this.imageTransfer = imageTransfer;
			this.dbConnection = dbConnection;
			this.productManager = productManager;
			this.testcardSimService = testcardSimService;
			this.accessManager = accessManager;
			this.errorHandling = errorHandling;
			this.ShowSettingsCommand = new RelayCommand(new Action<object>(this.ShowSettings));
			this.ShowStatisticsCommand = new RelayCommand(new Action<object>(this.ShowStatistics));
			this.ShowHistogramCommand = new RelayCommand(new Action<object>(this.ShowHistogram));
			this.StartSimulationCommand = new RelayCommand(new Action<object>(this.StartSimulation));
			this.ShowIntegratedComplianceMonitoring = new RelayCommand(new Action<object>(this.ShowICM));
			this.btnStartSimulation = new Button();
			this.btnDelete = new Button();
			this.btnMarkAll = new Button();
			this.btnSimulateFromUsb = new Button();
			this.imageForComplianceMode = new XRSImage();
			this.ButtonImageHistSourcePath = "..\\img\\overview.png";
			this.notInitView = false;
			this.ShowHistoryOrigSim = true;
			this.AllImagesAreMarked = false;
			this.SelectedItemCount = "0";
			this.StopVASimulation = false;
			this.StopGlassSimulation = false;
			this.progressCounter = 0;
			this.selectedImgCountforStatistics = "0";
			this.checkContCountIsEnable = (parHandling.ReadPar(Definition.contaminationsCounterEnablePath) == "1");
			this.timerCheckHistoryLoading = new Timer(new TimerCallback(this.timer_CheckHistoryLoading), null, 0, 500);
			this.simulationmode = parHandling.ReadPar(Definition.simulationmodelPath);
			this.SelectedPrievewItems = new List<PreviewItem>();
			base.Preview.FirstItemCreated += this.Preview_FirstItemCreated;
		}

		// Token: 0x06001744 RID: 5956 RVA: 0x0008C664 File Offset: 0x0008A864
		private void ShowSettings(object obj)
		{
			bool flag = this.testcardSimService.CheckBallCountsBecauseOfNewVersion();
			if (flag)
			{
				CtrlTestcardSimSettings ctrlTestcardSimSettings = new CtrlTestcardSimSettings(this.parHandling, this.testcardSimService, this.dbConnection, this.productManager, this.accessManager, this.errorHandling, this.imageForComplianceMode, this.imageTransfer);
				Subwindow.Show(ctrlTestcardSimSettings, SubwindowButtons.BackAndHome, ctrlTestcardSimSettings.PanelButtons, ctrlTestcardSimSettings.UiElementRightAligned);
			}
			else
			{
				Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtVersionCheckSensitivityPrediction"), DialogStyle.Warning);
			}
		}

		// Token: 0x06001745 RID: 5957 RVA: 0x0008C6EC File Offset: 0x0008A8EC
		private void ShowStatistics(object obj)
		{
			bool flag = this.testcardSimService.CheckBallCountsBecauseOfNewVersion();
			if (flag)
			{
				bool flag2 = this.testcardSimService.CompareActSensWithDB();
				if (flag2)
				{
					Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtCompareActSensWithDBValues"), DialogStyle.Warning);
				}
				else
				{
					CtrlTestcardSimStatistics ctrlTestcardSimStatistics = new CtrlTestcardSimStatistics(this.parHandling, this.selectedImgCountforStatistics, this.simulatedImagesTotalCounts, this.productManager, this.dbConnection, this.simulationmode, this.testcardSimService);
					Subwindow.Show(ctrlTestcardSimStatistics, SubwindowButtons.BackAndHome, ctrlTestcardSimStatistics.PanelButtons, ctrlTestcardSimStatistics.UiElementRightAligned);
				}
			}
			else
			{
				Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtVersionCheckSensitivityPrediction"), DialogStyle.Warning);
			}
		}

		// Token: 0x06001746 RID: 5958 RVA: 0x0008C7A0 File Offset: 0x0008A9A0
		private void ShowICM(object obj)
		{
			bool flag = this.testcardSimService.CheckBallCountsBecauseOfNewVersion();
			if (flag)
			{
				this.imageForComplianceMode = null;
				this.imageForComplianceMode = this.testcardSimService.GetLastNSavedImages(0);
				bool flag2 = this.imageForComplianceMode != null;
				if (flag2)
				{
					CtrlTestcardSimMonitoring ctrlTestcardSimMonitoring = new CtrlTestcardSimMonitoring(this.parHandling, this.testcardSimService, this.dbConnection, this.productManager, this.errorHandling, this.accessManager, this.imageForComplianceMode, this.imageTransfer, this.imageArchiver);
					Subwindow.Show(ctrlTestcardSimMonitoring, SubwindowButtons.BackAndHome, ctrlTestcardSimMonitoring.PanelButtons, ctrlTestcardSimMonitoring.UiElementRightAligned);
				}
				else
				{
					Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtNoValidImage"), DialogStyle.Warning);
				}
			}
			else
			{
				Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtVersionCheckSensitivityPrediction"), DialogStyle.Warning);
			}
		}

		// Token: 0x06001747 RID: 5959 RVA: 0x0008C878 File Offset: 0x0008AA78
		private void ShowHistogram(object obj)
		{
			bool flag = this.SelectedPrievewItems != null && this.SelectedPrievewItems.Count != 0;
			if (flag)
			{
				CtrlTestcardSimHistogram ctrlTestcardSimHistogram = new CtrlTestcardSimHistogram(this.SelectedPrievewItems, this.parHandling);
				Subwindow.Show(ctrlTestcardSimHistogram, SubwindowButtons.BackAndHome, ctrlTestcardSimHistogram.PanelButtons, ctrlTestcardSimHistogram.UiElementRightAligned);
			}
			else
			{
				Dialog.Show(LocalizationManager.Localize("txtInfo"), LocalizationManager.Localize("txtNoImageSelected"), DialogStyle.Info);
			}
		}

		// Token: 0x06001748 RID: 5960 RVA: 0x0008C8E8 File Offset: 0x0008AAE8
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

		// Token: 0x06001749 RID: 5961 RVA: 0x0008C950 File Offset: 0x0008AB50
		private void timer_CheckHistoryLoading(object obj)
		{
			bool isHistoryLoading = base.Preview.IsHistoryLoading;
			if (isHistoryLoading)
			{
				bool flag = this.showHistoryOrigSim;
				if (flag)
				{
					this.ButtonImageHistSourcePath = "..\\img\\overview_disabled.png";
				}
				else
				{
					this.ButtonImageHistSourcePath = "..\\img\\simulateTestcard_disabled.png";
				}
				this.HistoryisLoading = true;
			}
			else
			{
				bool flag2 = this.showHistoryOrigSim;
				if (flag2)
				{
					this.ButtonImageHistSourcePath = "..\\img\\overview.png";
				}
				else
				{
					this.ButtonImageHistSourcePath = "..\\img\\simulateTestcard.png";
				}
				this.HistoryisLoading = false;
				this.timerCheckHistoryLoading.Change(-1, -1);
			}
		}

		// Token: 0x0600174A RID: 5962 RVA: 0x0008C9DC File Offset: 0x0008ABDC
		public void DeleteMarkedImages()
		{
			bool flag = this.AllImagesAreMarked;
			if (flag)
			{
				for (int i = 0; i < base.Preview.PreviewItems.Count; i++)
				{
					bool flag2 = File.Exists(base.Preview.PreviewItems[i].FileName);
					if (flag2)
					{
						File.Delete(base.Preview.PreviewItems[i].FileName);
					}
				}
				base.Preview.PreviewItems.Clear();
				this.AllImagesAreMarked = false;
				this.SelectedItemCount = "0";
			}
			else
			{
				bool flag3 = base.Preview.PreviewItems != null && this.SelectedPrievewItems != null;
				if (flag3)
				{
					List<PreviewItem> selectedPrievewItems = this.SelectedPrievewItems;
					for (int j = 0; j < selectedPrievewItems.Count; j++)
					{
						for (int k = 0; k < base.Preview.PreviewItems.Count; k++)
						{
							bool flag4 = selectedPrievewItems[j].Timestamp == base.Preview.PreviewItems[k].Timestamp;
							if (flag4)
							{
								bool flag5 = File.Exists(base.Preview.PreviewItems[k].FileName);
								if (flag5)
								{
									File.Delete(base.Preview.PreviewItems[k].FileName);
								}
								base.Preview.PreviewItems.RemoveAt(k);
								k = base.Preview.PreviewItems.Count;
							}
						}
					}
					this.SelectedPrievewItems.Clear();
				}
			}
		}

		// Token: 0x0600174B RID: 5963 RVA: 0x0008CBA0 File Offset: 0x0008ADA0
		private void deleteAllSimulatedImages()
		{
			bool flag = Directory.Exists(StoreImageData.archiveDirectoryTestcardSimulation);
			if (flag)
			{
				string[] files = Directory.GetFiles(StoreImageData.archiveDirectoryTestcardSimulation);
				foreach (string path in files)
				{
					File.Delete(path);
				}
			}
		}

		// Token: 0x0600174C RID: 5964 RVA: 0x0008CBE8 File Offset: 0x0008ADE8
		public void SaveMarkedImages()
		{
			bool flag = this.SelectedPrievewItems != null || this.AllImagesAreMarked;
			if (flag)
			{
				this.prgSaveToUSB = new Progress(LocalizationManager.Localize("txtSavingImage") + "...", false);
				this.prgSaveToUSB.Show();
				this.bgSaveToUSB = new BackgroundWorker();
				this.bgSaveToUSB.WorkerReportsProgress = true;
				this.bgSaveToUSB.DoWork += this.bgSaveToUSB_DoWork;
				this.bgSaveToUSB.RunWorkerCompleted += this.bgSaveToUSB_RunWorkerCompleted;
				this.bgSaveToUSB.ProgressChanged += this.bgSaveToUSB_ProgressChanged;
				this.bgSaveToUSB.RunWorkerAsync();
			}
		}

		// Token: 0x0600174D RID: 5965 RVA: 0x0008CCA8 File Offset: 0x0008AEA8
		public void SimulationfromUSB(string filename)
		{
			PreviewItem previewItem = new PreviewItem(filename, this.imageArchiver);
			previewItem.XrsImage = this.imageArchiver.GetImage(filename);
			this.SelectedPrievewItems.Add(previewItem);
			this.StartSimulation(null);
		}

		// Token: 0x0600174E RID: 5966 RVA: 0x0008CCEC File Offset: 0x0008AEEC
		private void CopyImagesToUSB(string sourcepath)
		{
			bool flag = Directory.Exists(StoreImageData.archiveDirectoryThroughput) && Directory.Exists(StoreImageData.archiveDirectoryTestcardSimulation);
			if (flag)
			{
				string text = "";
				string[] array = sourcepath.Split(new char[]
				{
					'\\'
				});
				this.dbConnection.GetUsbDriveLetter(ref text);
				string str = array[array.Length - 1];
				string text2 = text + "\\RayconImages\\TestcardSimulation\\";
				bool flag2 = Directory.Exists(text);
				if (flag2)
				{
					bool flag3 = !Directory.Exists(text2);
					if (flag3)
					{
						Directory.CreateDirectory(text2);
					}
				}
				text2 += str;
				try
				{
					File.Copy(sourcepath, text2, true);
				}
				catch
				{
				}
			}
		}

		// Token: 0x0600174F RID: 5967 RVA: 0x0008CDB4 File Offset: 0x0008AFB4
		private void bgSaveToUSB_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			bool flag = this.prgSaveToUSB != null;
			if (flag)
			{
				this.prgSaveToUSB.progBar.Value = (double)e.ProgressPercentage;
			}
		}

		// Token: 0x06001750 RID: 5968 RVA: 0x0008CDE8 File Offset: 0x0008AFE8
		private void bgSaveToUSB_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			bool flag = this.prgSaveToUSB != null;
			if (flag)
			{
				this.prgSaveToUSB.Close();
			}
			Dialog.Show(LocalizationManager.Localize("txtSuccess"), LocalizationManager.Localize("txtSaveSuccessful"), DialogStyle.Ok, DialogButtons.None, true);
		}

		// Token: 0x06001751 RID: 5969 RVA: 0x0008CE2C File Offset: 0x0008B02C
		private void bgSaveToUSB_DoWork(object sender, DoWorkEventArgs e)
		{
			bool flag = this.AllImagesAreMarked;
			if (flag)
			{
				for (int i = 0; i < base.Preview.PreviewItems.Count; i++)
				{
					this.CopyImagesToUSB(base.Preview.PreviewItems[i].FileName);
					this.bgSaveToUSB.ReportProgress((i + 1) * 100 / base.Preview.PreviewItems.Count);
				}
			}
			else
			{
				for (int j = 0; j < this.SelectedPrievewItems.Count; j++)
				{
					this.CopyImagesToUSB(this.SelectedPrievewItems[j].FileName);
					this.bgSaveToUSB.ReportProgress((j + 1) * 100 / this.SelectedPrievewItems.Count);
				}
			}
			this.bgSaveToUSB.ReportProgress(100);
		}

		// Token: 0x06001752 RID: 5970 RVA: 0x0008CF10 File Offset: 0x0008B110
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

		// Token: 0x06001753 RID: 5971 RVA: 0x0008CF8C File Offset: 0x0008B18C
		private void StartSimulation(object obj)
		{
			bool flag = this.checkIfProductIsTeachedIn();
			if (flag)
			{
				Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtProductNotTeachedIn"), DialogStyle.Error, DialogButtons.OK);
			}
			else
			{
				bool flag2 = this.parHandling.ReadPar(XrayState.xrayPowerOnPath) == "1";
				if (flag2)
				{
					Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtTurnXrayOff"), DialogStyle.Error, DialogButtons.OK);
				}
				else
				{
					this.initSimProgBeforeDurationIsEstimated();
					bool flag3 = Dialog.Show(LocalizationManager.Localize("txtInfo"), LocalizationManager.Localize("txtCalcSensQuestionTestcardSim") + Environment.NewLine + this.testcardSimService.setExpectedDurationMsg(this.SelectedPrievewItems.Count * (int)this.SelectedPrievewItems[0].XrsImage.Width * (int)this.SelectedPrievewItems[0].XrsImage.Height, false), DialogStyle.Info, DialogButtons.OKCancel) == Dialog.DialogResults.Ok;
					if (flag3)
					{
						this.initSimProgAfterDurationIsEstimated();
						this.imageTransfer.FrameArrived += this.imageTransfer_SimImageArrived;
						string description = LocalizationManager.Localize("txtCalcSensTestcardSim") + "...";
						this.progress = new Progress(description, false);
						this.progress.Show();
						this.bgSimulation = new BackgroundWorker();
						this.bgSimulation.WorkerReportsProgress = true;
						this.bgSimulation.WorkerSupportsCancellation = true;
						this.bgSimulation.DoWork += this.bgSimulation_DoWork;
						this.bgSimulation.ProgressChanged += this.bgSimulation_ProgressChanged;
						this.bgSimulation.RunWorkerCompleted += this.bgSimulation_RunWorkerCompleted;
						this.bgSimulation.RunWorkerAsync();
					}
				}
			}
		}

		// Token: 0x06001754 RID: 5972 RVA: 0x0008D150 File Offset: 0x0008B350
		private void initSimProgAfterDurationIsEstimated()
		{
			this.testcardSimService.NewVABallSizeAfterSim = Convert.ToInt32(this.parHandling.ReadPar(Definition.simulationActSensVA));
			this.testcardSimService.NewGlassBallSizeAfterSim = Convert.ToInt32(this.parHandling.ReadPar(Definition.simulationActSensGlass));
			this.parHandling.WriteValue(Definition.contaminationsCounterEnablePath, "1");
			this.deleteAllSimulatedImages();
			this.parHandling.WriteValue(Definition.simulationWrongDetPix, "0");
			this.testcardSimService.CheckSensitivityDecrease = false;
			this.testcardSimService.CheckBallCounts = false;
			this.testcardSimService.ProductChangedForStatisticTable = false;
		}

		// Token: 0x06001755 RID: 5973 RVA: 0x0008D1FC File Offset: 0x0008B3FC
		private void initSimProgBeforeDurationIsEstimated()
		{
			this.simulationmode = this.parHandling.ReadPar(Definition.simulationmodelPath);
			bool flag = this.simulationmode != "0" && this.simulationmode != "2";
			if (flag)
			{
				this.simulationmode = "0";
				this.parHandling.WriteValue(Definition.simulationmodelPath, this.simulationmode);
				this.parHandling.WriteValue(Definition.simulationmaterialPath, "0");
			}
		}

		// Token: 0x06001756 RID: 5974 RVA: 0x0008D284 File Offset: 0x0008B484
		private void bgSimulation_DoWork(object sender, DoWorkEventArgs e)
		{
			this.testcardSimService.CheckVersionAndXRaySource = false;
			bool flag = !this.testcardSimService.CheckBallCountsBecauseOfNewVersion();
			if (flag)
			{
				this.simulationmode = "0";
				this.parHandling.WriteValue(Definition.simulationmodelPath, "0");
				this.parHandling.WriteValue(Definition.simulationmaterialPath, "0");
			}
			this.SimulateTestcardsOnXRIImages(this.simulationmode);
			bool flag2 = !this.testcardSimService.CheckVersionAndXRaySource;
			if (flag2)
			{
				this.WaitForImagesFromIPU(this.simulationmode);
			}
			this.CalculateAndSaveAverageSensitivities();
			this.SaveValuesForStatisticsMenu(this.simulationmode);
		}

		// Token: 0x06001757 RID: 5975 RVA: 0x0008D32C File Offset: 0x0008B52C
		private void bgSimulation_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			this.progress.FormFadeOut.Begin();
			this.bgSimulation.DoWork -= this.bgSimulation_DoWork;
			this.bgSimulation.ProgressChanged -= this.bgSimulation_ProgressChanged;
			this.bgSimulation.RunWorkerCompleted -= this.bgSimulation_RunWorkerCompleted;
			bool flag = this.simulatedImagesTotalCounts <= 3;
			if (flag)
			{
				Thread.Sleep(200);
			}
			bool checkVersionAndXRaySource = this.testcardSimService.CheckVersionAndXRaySource;
			if (checkVersionAndXRaySource)
			{
				Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtTestcardSimNotPossible"), DialogStyle.Warning);
			}
			bool flag2 = !this.testcardSimService.CheckVersionAndXRaySource && this.simulationmode == "0" && Dialog.Show(this.testcardSimService.setRetrainTestcardSimMsg(true), this.testcardSimService.setRetrainTestcardSimMsg(false), this.testcardSimService.setRetrainTestcardSimMsgLevel(), DialogButtons.OK) == Dialog.DialogResults.Ok;
			if (flag2)
			{
				this.parHandling.WriteValue(Definition.simulationActSensVA, Convert.ToString(this.testcardSimService.NewVABallSizeAfterSim));
				this.parHandling.WriteValue(Definition.simulationActSensGlass, Convert.ToString(this.testcardSimService.NewGlassBallSizeAfterSim));
				this.testcardSimService.SetProperties();
			}
			this.imageTransfer.FrameArrived -= this.imageTransfer_SimImageArrived;
			int num = Convert.ToInt32(this.parHandling.ReadPar(Definition.simulationWrongDetPix));
			bool flag3 = this.testcardSimService.CheckWrongDetectedPixel(this.simulatedImagesTotalCounts) && this.parHandling.ReadPar(Definition.anomalyDetectionInferenceEnabled) != "1";
			if (flag3)
			{
				Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtWrongPixelTestcardSim") + ": " + num.ToString(), DialogStyle.Warning, DialogButtons.OK);
			}
			bool checkSensitivityDecrease = this.testcardSimService.CheckSensitivityDecrease;
			if (checkSensitivityDecrease)
			{
				Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtDecreasesTestcardSim"), DialogStyle.Warning, DialogButtons.OK);
			}
			bool checkBallCounts = this.testcardSimService.CheckBallCounts;
			if (checkBallCounts)
			{
				Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtErrorGeneralTestcardSim"), DialogStyle.Warning, DialogButtons.OK);
			}
		}

		// Token: 0x06001758 RID: 5976 RVA: 0x0008D565 File Offset: 0x0008B765
		private void bgSimulation_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			this.progress.progBar.Value = (double)e.ProgressPercentage;
		}

		// Token: 0x06001759 RID: 5977 RVA: 0x0008D580 File Offset: 0x0008B780
		private void SaveValuesForStatisticsMenu(string mode)
		{
			this.selectedImgCountforStatistics = this.SelectedItemCount;
		}

		// Token: 0x0600175A RID: 5978 RVA: 0x0008D590 File Offset: 0x0008B790
		private void WaitForImagesFromIPU(string mode)
		{
			bool flag = mode != "1" && mode != "3";
			if (flag)
			{
				bool flag2 = this.simulatedImagesTotalCounts > 4 && this.simulatedImagesCounter < this.simulatedImagesTotalCounts;
				if (flag2)
				{
					int num = this.SimulatedImagesCounter;
					int num2 = this.SimulatedImagesCounter;
					this.progressCounter = 0;
					this.bgSimulation.ReportProgress(0);
					while (this.SimulatedImagesCounter < this.simulatedImagesTotalCounts)
					{
						bool flag3 = this.SimulatedImagesCounter > num2;
						if (flag3)
						{
							this.progressCounter++;
							this.bgSimulation.ReportProgress(100 * this.progressCounter / (this.simulatedImagesTotalCounts - num));
							num2 = this.SimulatedImagesCounter;
						}
					}
					this.bgSimulation.ReportProgress(90);
				}
			}
			else
			{
				this.bgSimulation.ReportProgress(90);
			}
		}

		// Token: 0x0600175B RID: 5979 RVA: 0x0008D684 File Offset: 0x0008B884
		private void SimulateTestcardsOnXRIImages(string simMode)
		{
			int num = (int)Convert.ToUInt16(this.parHandling.ReadPar(Definition.simulationmaterialPath));
			int num2 = (int)Convert.ToUInt16(this.parHandling.ReadPar(Definition.simulationBallSizeVA));
			int num3 = (int)Convert.ToUInt16(this.parHandling.ReadPar(Definition.simulationBallSizeGlass));
			string text = this.parHandling.ReadPar(Definition.materialdensityPath);
			int xpattern = (int)Convert.ToUInt16(this.parHandling.ReadPar(Definition.testpatternxdirectionPath));
			int ypattern = (int)Convert.ToUInt16(this.parHandling.ReadPar(Definition.testpatternydirectionPath));
			int backgroundnoise = (int)(10000 - 100 * Convert.ToUInt16(this.parHandling.ReadPar(Definition.backroundNoisePath)));
			this.SimulatedImagesCounter = 0;
			this.simulatedImagesTotalCounts = 0;
			TestcardSimViewModel.VASensitivities = new List<int>();
			TestcardSimViewModel.GlassSensitivities = new List<int>();
			TestcardSimViewModel.ImgNamesForSensitivities = new List<string>();
			bool flag = this.SelectedPrievewItems != null;
			if (flag)
			{
				for (int i = 0; i < this.SelectedPrievewItems.Count; i++)
				{
					TestcardSimViewModel.ImgNamesForSensitivities.Add(Convert.ToString(this.SelectedPrievewItems[i].FileName));
				}
				bool flag2 = simMode == "0";
				if (flag2)
				{
					this.progressCounter = 0;
					for (int j = 0; j < this.SelectedPrievewItems.Count; j++)
					{
						bool calcedgepointspattern = true;
						bool flag3 = num == 0;
						if (flag3)
						{
							this.simulatedImagesTotalCounts = this.SelectedPrievewItems.Count * Definition.sumOfAllTestcards;
							for (int k = 0; k < Definition.sumOfAllTestcards; k++)
							{
								bool flag4 = k < Definition.sumOfVATestcards;
								int simmaterial;
								int simsize;
								if (flag4)
								{
									simmaterial = 1;
									simsize = k;
								}
								else
								{
									simmaterial = 2;
									simsize = k - Definition.sumOfVATestcards;
								}
								this.testcardSimService.CheckVersionAndXRaySource = this.testcardSimService.SimulateOneTestcardOneImage(this.SelectedPrievewItems[j].XrsImage, simmaterial, simsize, xpattern, ypattern, backgroundnoise, calcedgepointspattern);
								calcedgepointspattern = false;
								this.progressCounter++;
								this.bgSimulation.ReportProgress(100 * this.progressCounter / this.simulatedImagesTotalCounts);
							}
						}
						else
						{
							bool flag5 = num > 0;
							if (flag5)
							{
								int num4 = 0;
								bool flag6 = num == 1;
								if (flag6)
								{
									num4 = Definition.sumOfVATestcards;
								}
								else
								{
									bool flag7 = num == 2;
									if (flag7)
									{
										num4 = Definition.sumOfGlassTestcards;
									}
								}
								this.simulatedImagesTotalCounts = this.SelectedPrievewItems.Count * num4;
								for (int l = 0; l < num4; l++)
								{
									this.testcardSimService.CheckVersionAndXRaySource = this.testcardSimService.SimulateOneTestcardOneImage(this.SelectedPrievewItems[j].XrsImage, num, l, xpattern, ypattern, backgroundnoise, calcedgepointspattern);
									calcedgepointspattern = false;
									this.progressCounter++;
									this.bgSimulation.ReportProgress(100 * this.progressCounter / (this.SelectedPrievewItems.Count * num4));
								}
							}
						}
					}
				}
				else
				{
					bool flag8 = simMode == "1";
					if (!flag8)
					{
						bool flag9 = simMode == "2";
						if (flag9)
						{
							int simsize2 = 0;
							bool flag10 = num == 1;
							if (flag10)
							{
								simsize2 = num2;
							}
							else
							{
								bool flag11 = num == 2;
								if (flag11)
								{
									simsize2 = num3;
								}
							}
							for (int m = 0; m < this.SelectedPrievewItems.Count; m++)
							{
								this.testcardSimService.CheckVersionAndXRaySource = this.testcardSimService.SimulateOneTestcardOneImage(this.SelectedPrievewItems[m].XrsImage, num, simsize2, xpattern, ypattern, backgroundnoise, true);
								this.bgSimulation.ReportProgress(100 * (m + 1) / this.SelectedPrievewItems.Count);
							}
							this.simulatedImagesTotalCounts = this.SelectedPrievewItems.Count;
						}
						else
						{
							bool flag12 = simMode == "3";
							if (!flag12)
							{
								bool flag13 = simMode == "4";
								if (flag13)
								{
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x0600175C RID: 5980 RVA: 0x0008DAAC File Offset: 0x0008BCAC
		public void imageTransfer_SimImageArrived(object sender, ReceiveEventArgs e)
		{
			bool flag = e.Image.Type == ImageType.TestcardSimulation;
			if (flag)
			{
				bool flag2 = this.parHandling.ReadPar(Definition.simulationsaveSimImages) == "1";
				this.calculateSensitivities((int)e.Image.TestcardSimBallCount, (int)e.Image.ContaminationCount, e.Image.TestcardSimMaterial, (int)e.Image.TestcardSimBallSize);
				string a = this.parHandling.ReadPar(Definition.simulationmodelPath);
				bool flag3 = a == "1" || a == "3";
				if (flag3)
				{
					this.SimImageArrived = true;
				}
				bool flag4 = flag2;
				if (flag4)
				{
					this.imageArchiver.ArchiveSimulatedImage(e.Image, 242, this.testcardSimService.ConvertBallSizeToString(e.Image.TestcardSimMaterial, (int)e.Image.TestcardSimBallSize, false, true), this.productManager.CurrentLoadedProduct.ProductNumber, this.productManager.CurrentLoadedProduct.ProductName);
				}
				int num = this.SimulatedImagesCounter;
				this.SimulatedImagesCounter = num + 1;
			}
		}

		// Token: 0x0600175D RID: 5981 RVA: 0x0008DBD4 File Offset: 0x0008BDD4
		private void calculateSensitivities(int totalcounts, int detectedcounts, string material, int ballsize)
		{
			string a = this.parHandling.ReadPar(Definition.simulationmodelPath);
			material = material.Trim();
			double num = 0.0;
			bool flag = totalcounts != 0;
			if (flag)
			{
				num = 100.0 * ((double)detectedcounts / (double)totalcounts);
			}
			bool flag2 = material == "VA";
			if (flag2)
			{
				TestcardSimViewModel.VASensitivities.Add(ballsize);
				TestcardSimViewModel.VASensitivities.Add((int)num);
				bool flag3 = num > 99.0 && (a == "1" || a == "3");
				if (flag3)
				{
					this.StopVASimulation = true;
				}
			}
			else
			{
				bool flag4 = material == "Glass";
				if (flag4)
				{
					TestcardSimViewModel.GlassSensitivities.Add(ballsize);
					TestcardSimViewModel.GlassSensitivities.Add((int)num);
					bool flag5 = num > 99.0 && (a == "1" || a == "3");
					if (flag5)
					{
						this.StopGlassSimulation = true;
					}
				}
			}
		}

		// Token: 0x0600175E RID: 5982 RVA: 0x0008DCE8 File Offset: 0x0008BEE8
		private void CalculateAndSaveAverageSensitivities()
		{
			bool flag = true;
			string a = this.parHandling.ReadPar(Definition.simulationmodelPath);
			string a2 = this.parHandling.ReadPar(Definition.simulationmaterialPath);
			List<string> list = new List<string>();
			bool flag2 = a2 == "0" || a2 == "1";
			if (flag2)
			{
				bool flag3 = TestcardSimViewModel.VASensitivities != null;
				if (flag3)
				{
					for (int i = 0; i < Definition.sumOfVATestcards; i++)
					{
						int num = 0;
						int num2 = 0;
						for (int j = 0; j < TestcardSimViewModel.VASensitivities.Count; j++)
						{
							bool flag4 = TestcardSimViewModel.VASensitivities[j] == i;
							if (flag4)
							{
								num += TestcardSimViewModel.VASensitivities[j + 1];
								num2++;
							}
							j++;
						}
						bool flag5 = num2 != 0;
						if (flag5)
						{
							num /= num2;
						}
						bool flag6 = flag && (a == "0" || a == "1" || a == "3");
						if (flag6)
						{
							bool flag7 = num >= 95;
							if (flag7)
							{
								this.testcardSimService.NewVABallSizeAfterSim = i;
								flag = false;
							}
						}
						list.Add(Convert.ToString(num));
					}
				}
				bool flag8 = flag;
				if (flag8)
				{
					this.testcardSimService.NewVABallSizeAfterSim = 999;
				}
			}
			bool flag9 = a2 == "0" || a2 == "2";
			if (flag9)
			{
				flag = true;
				bool flag10 = TestcardSimViewModel.GlassSensitivities != null;
				if (flag10)
				{
					for (int k = 0; k < Definition.sumOfGlassTestcards; k++)
					{
						int num = 0;
						int num2 = 0;
						for (int l = 0; l < TestcardSimViewModel.GlassSensitivities.Count; l++)
						{
							bool flag11 = TestcardSimViewModel.GlassSensitivities[l] == k;
							if (flag11)
							{
								num += TestcardSimViewModel.GlassSensitivities[l + 1];
								num2++;
							}
							l++;
						}
						bool flag12 = num2 != 0;
						if (flag12)
						{
							num /= num2;
						}
						bool flag13 = flag && (a == "0" || a == "1" || a == "3");
						if (flag13)
						{
							bool flag14 = num >= 95;
							if (flag14)
							{
								this.testcardSimService.NewGlassBallSizeAfterSim = k;
								flag = false;
							}
						}
						list.Add(Convert.ToString(num));
					}
				}
				bool flag15 = flag;
				if (flag15)
				{
					this.testcardSimService.NewGlassBallSizeAfterSim = 999;
				}
			}
			bool flag16 = a == "0" || a == "1";
			if (flag16)
			{
				bool flag17 = !this.testcardSimService.CheckBallCountsBecauseOfNewVersion();
				if (flag17)
				{
					int num3 = Convert.ToInt32(this.parHandling.ReadPar(Definition.simulationCompSensVA)) + 1;
					int num4 = Convert.ToInt32(this.parHandling.ReadPar(Definition.simulationCompSensGlass)) + 1;
					int num5 = Convert.ToInt32(this.parHandling.ReadPar(Definition.simulationInitSensVA)) + 1;
					int num6 = Convert.ToInt32(this.parHandling.ReadPar(Definition.simulationInitSensGlass)) + 1;
					int num7 = Convert.ToInt32(this.parHandling.ReadPar(Definition.simulationActSensVA)) + 1;
					int num8 = Convert.ToInt32(this.parHandling.ReadPar(Definition.simulationActSensGlass)) + 1;
					this.parHandling.WriteValue(Definition.simulationCompSensVA, Convert.ToString((num3 < 999) ? num3 : 999));
					this.parHandling.WriteValue(Definition.simulationCompSensGlass, Convert.ToString((num4 < 999) ? num4 : 999));
					this.parHandling.WriteValue(Definition.simulationInitSensVA, Convert.ToString((num5 < 999) ? num5 : 999));
					this.parHandling.WriteValue(Definition.simulationInitSensGlass, Convert.ToString((num6 < 999) ? num6 : 999));
					this.parHandling.WriteValue(Definition.simulationActSensVA, Convert.ToString((num7 < 999) ? num7 : 999));
					this.parHandling.WriteValue(Definition.simulationActSensGlass, Convert.ToString((num8 < 999) ? num8 : 999));
				}
				this.dbConnection.SetTestcardSimSensitivities(this.productManager.CurrentLoadedProduct.TableName, list);
			}
			this.bgSimulation.ReportProgress(100);
		}

		// Token: 0x0600175F RID: 5983 RVA: 0x0008E198 File Offset: 0x0008C398
		private void SimulationWithVariableStartpoint(int startpoint, XRSImage image, int material, int simxpatt, int simypatt, int backrnoise, int totalcounts, bool calcstartendpoints)
		{
			int num = Definition.sumOfVATestcards;
			bool flag = material == 2;
			if (flag)
			{
				num = Definition.sumOfGlassTestcards;
			}
			for (int i = startpoint; i < num; i++)
			{
				int num2 = 0;
				while (i > startpoint && !this.SimImageArrived && num2 < 10000)
				{
					num2++;
					Thread.Sleep(10);
				}
				this.SimImageArrived = false;
				bool flag2 = material == 1 && this.StopVASimulation;
				if (flag2)
				{
					this.StopVASimulation = false;
					break;
				}
				bool flag3 = material == 2 && this.StopGlassSimulation;
				if (flag3)
				{
					this.StopGlassSimulation = false;
					break;
				}
				this.testcardSimService.SimulateOneTestcardOneImage(image, material, i, simxpatt, simypatt, backrnoise, calcstartendpoints);
				calcstartendpoints = false;
				this.progressCounter++;
				this.bgSimulation.ReportProgress(100 * this.progressCounter / totalcounts);
			}
		}

		// Token: 0x06001760 RID: 5984 RVA: 0x0008E288 File Offset: 0x0008C488
		private bool checkIfProductIsTeachedIn()
		{
			bool result = true;
			int num = Convert.ToInt32(this.parHandling.ReadPar(Definition.filterMainPath + Definition.comparerSubPath + "1/Contrast/" + Definition.strValue));
			bool flag = num > 0 && num < 32000;
			if (flag)
			{
				result = false;
			}
			return result;
		}

		// Token: 0x04000F3F RID: 3903
		private Progress progress;

		// Token: 0x04000F40 RID: 3904
		private ParameterHandling parHandling;

		// Token: 0x04000F41 RID: 3905
		private string buttonImageHistSourcePath;

		// Token: 0x04000F42 RID: 3906
		private bool allImagesAreMarked;

		// Token: 0x04000F43 RID: 3907
		private bool historyisLoading;

		// Token: 0x04000F44 RID: 3908
		private bool showHistoryOrigSim;

		// Token: 0x04000F45 RID: 3909
		private BackgroundWorker bgSimulation;

		// Token: 0x04000F46 RID: 3910
		private StoreImageData imageArchiver;

		// Token: 0x04000F47 RID: 3911
		public Button btnStartSimulation;

		// Token: 0x04000F48 RID: 3912
		public Button btnSimulateFromUsb;

		// Token: 0x04000F49 RID: 3913
		public Button btnDelete;

		// Token: 0x04000F4A RID: 3914
		public Button btnMarkAll;

		// Token: 0x04000F4B RID: 3915
		private ImageDataTransfer imageTransfer;

		// Token: 0x04000F4C RID: 3916
		private DBConnection dbConnection;

		// Token: 0x04000F4D RID: 3917
		private string selectedItemCount;

		// Token: 0x04000F4E RID: 3918
		private bool notInitView;

		// Token: 0x04000F4F RID: 3919
		public Timer timerCheckHistoryLoading;

		// Token: 0x04000F50 RID: 3920
		private int simulatedImagesTotalCounts;

		// Token: 0x04000F51 RID: 3921
		private int simulatedImagesCounter;

		// Token: 0x04000F52 RID: 3922
		public bool checkContCountIsEnable;

		// Token: 0x04000F53 RID: 3923
		private Progress prgSaveToUSB;

		// Token: 0x04000F54 RID: 3924
		private BackgroundWorker bgSaveToUSB;

		// Token: 0x04000F55 RID: 3925
		private bool stopVASimulation = false;

		// Token: 0x04000F56 RID: 3926
		private bool stopGlassSimulation = false;

		// Token: 0x04000F57 RID: 3927
		private bool simImageArrived = false;

		// Token: 0x04000F58 RID: 3928
		private int progressCounter;

		// Token: 0x04000F59 RID: 3929
		private ProductManager productManager;

		// Token: 0x04000F5A RID: 3930
		private string selectedImgCountforStatistics;

		// Token: 0x04000F5B RID: 3931
		private string simulationmode;

		// Token: 0x04000F5C RID: 3932
		private ErrorHandling errorHandling;

		// Token: 0x04000F5D RID: 3933
		private TestcardSimService testcardSimService;

		// Token: 0x04000F5E RID: 3934
		private AccessRightsManager accessManager;

		// Token: 0x04000F5F RID: 3935
		public static List<int> VASensitivities;

		// Token: 0x04000F60 RID: 3936
		public static List<int> GlassSensitivities;

		// Token: 0x04000F61 RID: 3937
		public static List<string> ImgNamesForSensitivities;

		// Token: 0x04000F62 RID: 3938
		private XRSImage imageForComplianceMode;
	}
}
