using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Threading;
using Common;
using DatabaseConnection;
using Definitions;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x02000010 RID: 16
	public class AIStatusViewModel : INotifyPropertyChanged
	{
		// Token: 0x14000008 RID: 8
		// (add) Token: 0x060000DD RID: 221 RVA: 0x00005D18 File Offset: 0x00003F18
		// (remove) Token: 0x060000DE RID: 222 RVA: 0x00005D50 File Offset: 0x00003F50
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060000DF RID: 223 RVA: 0x00005D85 File Offset: 0x00003F85
		// (set) Token: 0x060000E0 RID: 224 RVA: 0x00005D8D File Offset: 0x00003F8D
		public RelayCommand CleanUpCommand { get; set; }

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060000E1 RID: 225 RVA: 0x00005D96 File Offset: 0x00003F96
		// (set) Token: 0x060000E2 RID: 226 RVA: 0x00005D9E File Offset: 0x00003F9E
		public RelayCommand ChangePatternYDirectionCmd { get; set; }

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060000E3 RID: 227 RVA: 0x00005DA7 File Offset: 0x00003FA7
		// (set) Token: 0x060000E4 RID: 228 RVA: 0x00005DAF File Offset: 0x00003FAF
		public RelayCommand ChangePatternXDirectionCmd { get; set; }

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060000E5 RID: 229 RVA: 0x00005DB8 File Offset: 0x00003FB8
		// (set) Token: 0x060000E6 RID: 230 RVA: 0x00005DC0 File Offset: 0x00003FC0
		public RelayCommand ChangeMaterialDensityCmd { get; set; }

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060000E7 RID: 231 RVA: 0x00005DC9 File Offset: 0x00003FC9
		// (set) Token: 0x060000E8 RID: 232 RVA: 0x00005DD1 File Offset: 0x00003FD1
		public BoundLocalizedText StrSettings { get; set; }

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060000E9 RID: 233 RVA: 0x00005DDA File Offset: 0x00003FDA
		// (set) Token: 0x060000EA RID: 234 RVA: 0x00005DE2 File Offset: 0x00003FE2
		public BoundLocalizedText StrInferenceEnabledMessage { get; set; }

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060000EB RID: 235 RVA: 0x00005DEB File Offset: 0x00003FEB
		// (set) Token: 0x060000EC RID: 236 RVA: 0x00005DF3 File Offset: 0x00003FF3
		public BoundLocalizedText StrTrainingEnabledMessage { get; set; }

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060000ED RID: 237 RVA: 0x00005DFC File Offset: 0x00003FFC
		// (set) Token: 0x060000EE RID: 238 RVA: 0x00005E04 File Offset: 0x00004004
		public BoundLocalizedText StrTimeTrainingMessage { get; set; }

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060000EF RID: 239 RVA: 0x00005E0D File Offset: 0x0000400D
		// (set) Token: 0x060000F0 RID: 240 RVA: 0x00005E15 File Offset: 0x00004015
		public BoundLocalizedText StrTrainingProgressMessage { get; set; }

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x060000F1 RID: 241 RVA: 0x00005E20 File Offset: 0x00004020
		// (remove) Token: 0x060000F2 RID: 242 RVA: 0x00005E58 File Offset: 0x00004058
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action SendTestcardSimImages;

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060000F3 RID: 243 RVA: 0x00005E90 File Offset: 0x00004090
		// (set) Token: 0x060000F4 RID: 244 RVA: 0x00005EA8 File Offset: 0x000040A8
		public bool AnomalyDetectionInferenceOnOff
		{
			get
			{
				return this.anomalyDetectionInferenceOnOff;
			}
			set
			{
				bool flag = this.anomalyDetectionInferenceOnOff != value;
				if (flag)
				{
					bool flag2 = !value;
					if (flag2)
					{
						string message = LocalizationManager.Localize("txtAnomalyDetectionDisableInference");
						bool flag3 = Dialog.Show(LocalizationManager.Localize("txtWarning"), message, DialogStyle.Warning, DialogButtons.OKCancel) == Dialog.DialogResults.Ok;
						bool flag4 = flag3;
						if (!flag4)
						{
							return;
						}
						this.parHandling.WriteValue(Definition.anomalyDetectionInferenceEnabled, Convert.ToString("0"));
						LogbookManager.AddEntry(SSTEntryCode.AIDetectionDeactivated, 0, this.productManager.CurrentLoadedProduct.ProductNumber, this.productManager.CurrentLoadedProduct.ProductName);
						this.aiService.Comp19to30OnOff(value);
						this.startTestcardSim();
					}
					else
					{
						bool flag5 = value && (this.parHandling.ReadPar(Definition.anomalyDetectionTensorRTEngineAvailable) == "1" || this.parHandling.ReadPar(Definition.anomalyDetectionOnnxAvailable) == "1");
						if (!flag5)
						{
							Dialog.Show(LocalizationManager.Localize("txtInfo"), LocalizationManager.Localize("txtAnomalyDetectionNoModel"), DialogStyle.Info, DialogButtons.OK);
							return;
						}
						string a = this.parHandling.ReadPar(Definition.anomalyDetectionInferenceModelID);
						string text = this.parHandling.ReadPar(Definition.anomalyDetectionTrainModelID);
						bool flag6 = a != "" && text != "" && a == text && this.parHandling.ReadPar(Definition.anomalyDetectionTrainingInProgress) == "1";
						if (flag6)
						{
							Dialog.Show(LocalizationManager.Localize("txtInfo"), LocalizationManager.Localize("txtAINoInferenceStartPossible"), DialogStyle.Info, DialogButtons.OK);
							return;
						}
						this.parHandling.WriteValue(Definition.anomalyDetectionInferenceEnabled, Convert.ToString("1"));
						this.aiService.Comp19to30OnOff(value);
						bool flag7 = !this.PostProFiltersInitialized();
						if (flag7)
						{
							string tableName = "";
							this.dbConnection.GetCurrentProductTableNameAI(ref tableName);
							string archiveDirectoryAnomalyDetection = this.imageArchiver.GetArchiveDirectoryAnomalyDetection(tableName);
							int num = 0;
							bool flag8 = Directory.Exists(archiveDirectoryAnomalyDetection);
							if (flag8)
							{
								num = Directory.GetFiles(archiveDirectoryAnomalyDetection, "*.xri").Length;
							}
							bool flag9 = num >= 10;
							if (flag9)
							{
								CtrlAIAutotrain ctrlAIAutotrain = new CtrlAIAutotrain(this.imageArchiver, this.accessManager, this.dbConnection, this.productManager, this.aiService, this.parHandling, this.imageTransfer);
								ctrlAIAutotrain.AutotrainCompleted += delegate(object sender, EventArgs args)
								{
									this.startTestcardSim();
								};
								Subwindow.Show(ctrlAIAutotrain, SubwindowButtons.Home, ctrlAIAutotrain.PanelButtons, ctrlAIAutotrain.UiElementRightAligned);
							}
							else
							{
								bool flag10 = Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtNewAutotrainRequired"), DialogStyle.Warning, DialogButtons.OKCancel) == Dialog.DialogResults.Cancel;
								if (flag10)
								{
									this.parHandling.WriteValue(Definition.anomalyDetectionInferenceEnabled, Convert.ToString("0"));
									this.aiService.Comp19to30OnOff(!value);
									return;
								}
							}
						}
						else
						{
							this.startTestcardSim();
						}
						LogbookManager.AddEntry(SSTEntryCode.AIDetectionActivated, 0, this.productManager.CurrentLoadedProduct.ProductNumber, this.productManager.CurrentLoadedProduct.ProductName);
					}
					this.anomalyDetectionInferenceOnOff = value;
					this.OnPropertyChanged("AnomalyDetectionInferenceOnOff");
				}
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060000F5 RID: 245 RVA: 0x000061F8 File Offset: 0x000043F8
		// (set) Token: 0x060000F6 RID: 246 RVA: 0x00006210 File Offset: 0x00004410
		public bool AnomalyDetectionTrainingOnOff
		{
			get
			{
				return this.anomalyDetectionTrainingOnOff;
			}
			set
			{
				bool flag = this.anomalyDetectionTrainingOnOff != value;
				if (flag)
				{
					bool flag2 = !value;
					if (flag2)
					{
						string message = LocalizationManager.Localize("txtAnomalyDetectionDisableTraining");
						bool flag3 = Dialog.Show(LocalizationManager.Localize("txtWarning"), message, DialogStyle.Warning, DialogButtons.OKCancel) == Dialog.DialogResults.Ok;
						bool flag4 = flag3;
						if (!flag4)
						{
							return;
						}
						string modelID = this.parHandling.ReadPar(Definition.anomalyDetectionTrainModelID);
						this.parHandling.WriteValue(Definition.anomalyDetectionTrainEnabled, Convert.ToString("0"));
						LogbookManager.AddEntry(SSTEntryCode.AIDetectionDeactivated, 1, this.productManager.CurrentLoadedProduct.ProductNumber, this.productManager.CurrentLoadedProduct.ProductName);
						this.CurrentProductTraining = this.CurrentProductInference;
						this.StrTrainingEnabledMessage.LocalizedText = LocalizationManager.Localize("txtTrainingEnabled");
						this.dbConnection.SetProductTableNameTrainingAI("");
						this.RestoreBackupFineTuning(modelID);
					}
					else
					{
						bool flag5 = value && this.parHandling.ReadPar(Definition.anomalyDetectionTensorRTEngineAvailable) == "0";
						if (!flag5)
						{
							Dialog.Show(LocalizationManager.Localize("txtInfo"), LocalizationManager.Localize("txtAnomalyDetectionOnlyFineTuning"), DialogStyle.Info, DialogButtons.OK);
							return;
						}
						bool flag6 = !this.aiService.CheckForFreeMemory();
						if (flag6)
						{
							return;
						}
						this.parHandling.WriteValue(Definition.anomalyDetectionTrainEnabled, Convert.ToString("1"));
						LogbookManager.AddEntry(SSTEntryCode.AIDetectionActivated, 1, this.productManager.CurrentLoadedProduct.ProductNumber, this.productManager.CurrentLoadedProduct.ProductName);
					}
					this.anomalyDetectionTrainingOnOff = value;
					this.EstimatedTimeTraining = this.parHandling.ReadPar(Definition.anomalyDetectionTrainEstimatedTime);
					this.OnPropertyChanged("AnomalyDetectionTrainingOnOff");
				}
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060000F7 RID: 247 RVA: 0x000063E8 File Offset: 0x000045E8
		// (set) Token: 0x060000F8 RID: 248 RVA: 0x00006400 File Offset: 0x00004600
		public string EstimatedTimeTraining
		{
			get
			{
				return this.estimatedTimeTraining;
			}
			set
			{
				this.estimatedTimeTraining = value;
				this.OnPropertyChanged("EstimatedTimeTraining");
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060000F9 RID: 249 RVA: 0x00006418 File Offset: 0x00004618
		// (set) Token: 0x060000FA RID: 250 RVA: 0x00006430 File Offset: 0x00004630
		public bool ControlsDisabled
		{
			get
			{
				return this.controlsDisabled;
			}
			set
			{
				bool flag = this.controlsDisabled != value;
				if (flag)
				{
					this.controlsDisabled = value;
					this.OnPropertyChanged("ControlsDisabled");
				}
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060000FB RID: 251 RVA: 0x00006464 File Offset: 0x00004664
		// (set) Token: 0x060000FC RID: 252 RVA: 0x0000647C File Offset: 0x0000467C
		public string CurrentProductInference
		{
			get
			{
				return this.currentProductInference;
			}
			set
			{
				this.currentProductInference = value;
				this.OnPropertyChanged("CurrentProductInference");
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060000FD RID: 253 RVA: 0x00006494 File Offset: 0x00004694
		// (set) Token: 0x060000FE RID: 254 RVA: 0x000064AC File Offset: 0x000046AC
		public string CurrentProductTraining
		{
			get
			{
				return this.currentProductTraining;
			}
			set
			{
				this.currentProductTraining = value;
				this.OnPropertyChanged("CurrentProductTraining");
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060000FF RID: 255 RVA: 0x000064C4 File Offset: 0x000046C4
		// (set) Token: 0x06000100 RID: 256 RVA: 0x000064DC File Offset: 0x000046DC
		public string TrainingProgress
		{
			get
			{
				return this.trainingProgress;
			}
			set
			{
				this.trainingProgress = value;
				this.OnPropertyChanged("TrainingProgress");
			}
		}

		// Token: 0x06000101 RID: 257 RVA: 0x000064F4 File Offset: 0x000046F4
		public AIStatusViewModel(ParameterHandling parHandling, AccessRightsManager accessManager, DBConnection dbConnection, ProductManager productManager, AIService aiService, TestcardSimService testcardSimService, ImageDataTransfer imageTransfer, StoreImageData imageArchiver)
		{
			this.parHandling = parHandling;
			this.accessManager = accessManager;
			this.dbConnection = dbConnection;
			this.productManager = productManager;
			this.aiService = aiService;
			this.testcardSimService = testcardSimService;
			this.imageTransfer = imageTransfer;
			this.imageArchiver = imageArchiver;
			this.Initialize();
			this.RegisterLocalizedText();
			imageTransfer.FrameArrived += this.imageTestcardSimTransfer_FrameArrived;
			this.CleanUpCommand = new RelayCommand(new Action<object>(this.CleanUp));
		}

		// Token: 0x06000102 RID: 258 RVA: 0x00006588 File Offset: 0x00004788
		private void Initialize()
		{
			bool flag = this.accessManager.CurrentUser.Rights >= BuiltInPriviledges.Admin;
			if (flag)
			{
				this.ControlsDisabled = false;
			}
			else
			{
				this.ControlsDisabled = true;
			}
			this.anomalyDetectionInferenceOnOff = (this.parHandling.ReadPar(Definition.anomalyDetectionInferenceEnabled) == "1");
			this.anomalyDetectionTrainingOnOff = (this.parHandling.ReadPar(Definition.anomalyDetectionTrainEnabled) == "1");
			this.EstimatedTimeTraining = this.parHandling.ReadPar(Definition.anomalyDetectionTrainEstimatedTime);
			this.isTestcardSimActive = (this.parHandling.ReadPar(Definition.testcardSimulationPath) == "1" && this.parHandling.ReadPar(Definition.simulationAfterAutolearn) == "1");
			this.updateTextBoxes = new DispatcherTimer
			{
				Interval = TimeSpan.FromSeconds(1.0)
			};
			this.updateTextBoxes.Tick += this.UpdateTextBoxes;
			this.updateTextBoxes.Start();
			this.aiService.UpdateAvailabilityModelFiles();
			this.timerCheckErrorTestcardSim = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, new EventHandler(this.checkErrorsTestcardSim), Application.Current.Dispatcher);
			this.timerCheckErrorTestcardSim.Stop();
			this.timerCounterTestcardSim = 0;
			bool flag2 = this.testcardSimService != null;
			if (flag2)
			{
				this.testcardSimService.MonitoringSensIsActive = false;
			}
			this.SetTrainingProgress();
		}

		// Token: 0x06000103 RID: 259 RVA: 0x0000671C File Offset: 0x0000491C
		private void startTestcardSim()
		{
			bool flag = this.testcardSimService != null && this.isTestcardSimActive && !this.productManager.CurrentLoadedProduct.IsRegionProduct && this.parHandling.ReadPar(Definition.autoKVAdjustmentPath) == "1";
			if (flag)
			{
				this.OnSendSimTesctardImages();
				this.timerCheckErrorTestcardSim.Start();
			}
		}

		// Token: 0x06000104 RID: 260 RVA: 0x00006784 File Offset: 0x00004984
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

		// Token: 0x06000105 RID: 261 RVA: 0x000067EC File Offset: 0x000049EC
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

		// Token: 0x06000106 RID: 262 RVA: 0x00006828 File Offset: 0x00004A28
		protected virtual void OnSendSimTesctardImages()
		{
			Action sendTestcardSimImages = this.SendTestcardSimImages;
			bool flag = sendTestcardSimImages != null;
			if (flag)
			{
				sendTestcardSimImages();
			}
		}

		// Token: 0x06000107 RID: 263 RVA: 0x00006850 File Offset: 0x00004A50
		private void checkErrorsTestcardSim(object sender, EventArgs e)
		{
			bool flag = this.testcardSimService != null && this.testcardSimService.CheckVersionAndXRaySource;
			if (flag)
			{
				this.progresstestcardSim.Close();
				this.timerCheckErrorTestcardSim.Stop();
				Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtTestcardSimNotPossible"), DialogStyle.Error, DialogButtons.OK);
			}
			bool flag2 = this.testcardSimService != null && this.testcardSimService.CheckBallCounts;
			if (flag2)
			{
				this.progresstestcardSim.Close();
				this.timerCheckErrorTestcardSim.Stop();
				Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtErrorGeneralTestcardSim"), DialogStyle.Warning, DialogButtons.OK);
			}
			bool flag3 = this.testcardSimService != null && !this.testcardSimService.ImagesAvailable;
			if (flag3)
			{
				this.progresstestcardSim.Close();
				this.timerCheckErrorTestcardSim.Stop();
				Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtTestcardSimNoImages"), DialogStyle.Error, DialogButtons.OK);
			}
			bool flag4 = this.timerCounterTestcardSim > 180;
			if (flag4)
			{
				this.progresstestcardSim.Close();
				this.timerCheckErrorTestcardSim.Stop();
				Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtAbortTestcardSim"), DialogStyle.Error, DialogButtons.OK);
			}
			this.timerCounterTestcardSim++;
		}

		// Token: 0x06000108 RID: 264 RVA: 0x000069A8 File Offset: 0x00004BA8
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x06000109 RID: 265 RVA: 0x000069D8 File Offset: 0x00004BD8
		private void RegisterLocalizedText()
		{
			this.StrInferenceEnabledMessage = new BoundLocalizedText("txtInferenceEnabled");
			this.StrTimeTrainingMessage = new BoundLocalizedText("txtTimeTraining");
			this.StrTrainingProgressMessage = new BoundLocalizedText("txtTrainingProgress");
			bool flag = this.parHandling.ReadPar(Definition.anomalyDetectionNumberImagesFineTuning) != "0";
			bool flag2 = flag;
			if (flag2)
			{
				this.StrTrainingEnabledMessage = new BoundLocalizedText("txtFineTuningEnabled");
			}
			else
			{
				this.StrTrainingEnabledMessage = new BoundLocalizedText("txtTrainingEnabled");
			}
			this.CurrentProductInference = "P" + this.productManager.CurrentLoadedProduct.ProductNumber + ": " + this.productManager.CurrentLoadedProduct.ProductName;
			string text = "";
			string text2 = this.parHandling.ReadPar(Definition.anomalyDetectionTrainModelID);
			this.dbConnection.GetProductNameWithNumber(text2, ref text);
			bool flag3 = text != "";
			if (flag3)
			{
				this.CurrentProductTraining = "P" + text2 + ": " + text;
			}
			else
			{
				this.CurrentProductTraining = this.CurrentProductInference;
			}
		}

		// Token: 0x0600010A RID: 266 RVA: 0x00006AF4 File Offset: 0x00004CF4
		public void DeregisterLocalizedText()
		{
			MainWindow.localizationManager.Deregister(this.StrInferenceEnabledMessage);
			MainWindow.localizationManager.Deregister(this.StrTrainingEnabledMessage);
			MainWindow.localizationManager.Deregister(this.StrTimeTrainingMessage);
			MainWindow.localizationManager.Deregister(this.StrTrainingProgressMessage);
		}

		// Token: 0x0600010B RID: 267 RVA: 0x00006B48 File Offset: 0x00004D48
		private void CleanUp(object obj)
		{
			this.DeregisterLocalizedText();
			bool flag = this.updateTextBoxes != null;
			if (flag)
			{
				this.updateTextBoxes.Stop();
				this.updateTextBoxes.Tick -= this.UpdateTextBoxes;
				this.updateTextBoxes = null;
			}
			bool flag2 = this.timerCheckErrorTestcardSim != null;
			if (flag2)
			{
				this.timerCheckErrorTestcardSim.Stop();
				this.timerCheckErrorTestcardSim = null;
			}
			bool flag3 = this.testcardSimService != null;
			if (flag3)
			{
				this.testcardSimService.MonitoringSensIsActive = true;
			}
			this.imageTransfer.FrameArrived -= this.imageTestcardSimTransfer_FrameArrived;
		}

		// Token: 0x0600010C RID: 268 RVA: 0x00006BEB File Offset: 0x00004DEB
		private void UpdateTextBoxes(object sender, EventArgs e)
		{
			this.EstimatedTimeTraining = this.parHandling.ReadPar(Definition.anomalyDetectionTrainEstimatedTime);
			this.SetTrainingProgress();
		}

		// Token: 0x0600010D RID: 269 RVA: 0x00006C0C File Offset: 0x00004E0C
		private void SetTrainingProgress()
		{
			bool flag = this.parHandling.ReadPar(Definition.anomalyDetectionTrainEnabled) == "1";
			if (flag)
			{
				bool flag2 = this.parHandling.ReadPar(Definition.anomalyDetectionTrainingInProgress) != "1";
				if (flag2)
				{
					string text = "";
					bool flag3 = this.parHandling.ReadPar(Definition.anomalyDetectionTrainModelID) == this.parHandling.ReadPar(Definition.anomalyDetectionInferenceModelID);
					bool flag4 = flag3;
					string archiveDirectoryAnomalyDetection;
					if (flag4)
					{
						this.dbConnection.GetCurrentProductTableNameAI(ref text);
						archiveDirectoryAnomalyDetection = this.imageArchiver.GetArchiveDirectoryAnomalyDetection(text);
					}
					else
					{
						this.dbConnection.GetAppTableNameWithNumber(this.parHandling.ReadPar(Definition.anomalyDetectionTrainModelID), ref text);
						text = text.Insert("AppDefault".Length, "AI");
						archiveDirectoryAnomalyDetection = this.imageArchiver.GetArchiveDirectoryAnomalyDetection(text);
					}
					int value = 0;
					bool flag5 = Directory.Exists(archiveDirectoryAnomalyDetection);
					if (flag5)
					{
						value = Directory.GetFiles(archiveDirectoryAnomalyDetection).Length;
					}
					string text2 = LocalizationManager.Localize("txtAnomalyDetectionTrainingProgress1");
					string text3 = this.parHandling.ReadPar(Definition.anomalyDetectionTrainSizeDataset);
					this.TrainingProgress = string.Concat(new string[]
					{
						text2,
						"\n",
						Convert.ToString(value),
						"/",
						text3
					});
				}
				else
				{
					this.TrainingProgress = LocalizationManager.Localize("txtAnomalyDetectionTrainingProgress2");
				}
			}
			else
			{
				this.TrainingProgress = "-";
				this.anomalyDetectionTrainingOnOff = false;
				this.OnPropertyChanged("AnomalyDetectionTrainingOnOff");
			}
		}

		// Token: 0x0600010E RID: 270 RVA: 0x00006DA0 File Offset: 0x00004FA0
		private bool PostProFiltersInitialized()
		{
			for (int i = 0; i < Definition.nAIPostproFilters; i++)
			{
				string str = string.Format("{0}{1}{2}{3}{4}", new object[]
				{
					Definition.filterMainPath,
					Definition.anomalyDetectionMain,
					Definition.anomalyDetectionInference,
					Definition.aipostproFilterSubPath,
					i + 1
				});
				int num = Convert.ToInt32(this.parHandling.ReadPar(str + Definition.anomalyDetectionInferenceThresholdReconErrorContour + "/" + Definition.strValue));
				int num2 = Convert.ToInt32(this.parHandling.ReadPar(str + Definition.anomalyDetectionInferenceThresholdReconErrorInside + "/" + Definition.strValue));
				bool flag = this.parHandling.ReadPar(str + "/Enabled/" + Definition.strValue) == "1";
				bool flag2 = (num == 0 || num2 == 0) && flag;
				if (flag2)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600010F RID: 271 RVA: 0x00006E9C File Offset: 0x0000509C
		private void RestoreBackupFineTuning(string modelID)
		{
			string text = "";
			this.dbConnection.GetAppTableNameWithNumber(modelID, ref text);
			text = text.Insert("AppDefault".Length, "AI");
			string text2 = Path.Combine(new string[]
			{
				ModelTransfer.archiveRoot + text
			});
			bool flag = Directory.Exists(text2);
			if (flag)
			{
				string[] directories = Directory.GetDirectories(text2, "Backup_AI_*");
				bool flag2 = directories.Length >= 1;
				if (flag2)
				{
					string[] files = Directory.GetFiles(directories[0], "*", SearchOption.TopDirectoryOnly);
					foreach (string text3 in files)
					{
						bool flag3 = Path.GetExtension(text3).Equals(".txt", StringComparison.OrdinalIgnoreCase);
						if (!flag3)
						{
							string fileName = Path.GetFileName(text3);
							string destFileName = Path.Combine(text2, fileName);
							File.Copy(text3, destFileName, true);
						}
					}
					Directory.Delete(directories[0], true);
				}
			}
		}

		// Token: 0x0400011C RID: 284
		private ParameterHandling parHandling;

		// Token: 0x0400011D RID: 285
		private AccessRightsManager accessManager;

		// Token: 0x0400011E RID: 286
		private DBConnection dbConnection;

		// Token: 0x0400011F RID: 287
		private ProductManager productManager;

		// Token: 0x04000120 RID: 288
		private AIService aiService;

		// Token: 0x04000122 RID: 290
		private DispatcherTimer updateTextBoxes;

		// Token: 0x0400012C RID: 300
		private TestcardSimService testcardSimService;

		// Token: 0x0400012D RID: 301
		private bool isTestcardSimActive = false;

		// Token: 0x0400012F RID: 303
		private DispatcherTimer timerCheckErrorTestcardSim;

		// Token: 0x04000130 RID: 304
		private Progress progresstestcardSim;

		// Token: 0x04000131 RID: 305
		private int timerCounterTestcardSim;

		// Token: 0x04000132 RID: 306
		private int testcardSimProgressCounter;

		// Token: 0x04000133 RID: 307
		private ImageDataTransfer imageTransfer;

		// Token: 0x04000134 RID: 308
		private StoreImageData imageArchiver;

		// Token: 0x04000135 RID: 309
		private bool anomalyDetectionInferenceOnOff;

		// Token: 0x04000136 RID: 310
		private bool anomalyDetectionTrainingOnOff;

		// Token: 0x04000137 RID: 311
		private string estimatedTimeTraining;

		// Token: 0x04000138 RID: 312
		private bool controlsDisabled;

		// Token: 0x04000139 RID: 313
		private string currentProductInference;

		// Token: 0x0400013A RID: 314
		private string currentProductTraining;

		// Token: 0x0400013B RID: 315
		private string trainingProgress;
	}
}
