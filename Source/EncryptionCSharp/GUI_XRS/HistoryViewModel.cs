using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using DatabaseConnection;
using Definitions;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x0200011B RID: 283
	public class HistoryViewModel : PreviewViewModel
	{
		// Token: 0x1700081B RID: 2075
		// (get) Token: 0x06001B43 RID: 6979 RVA: 0x000A46B9 File Offset: 0x000A28B9
		// (set) Token: 0x06001B44 RID: 6980 RVA: 0x000A46C1 File Offset: 0x000A28C1
		public List<PreviewItem> SelectedImages { get; set; }

		// Token: 0x1700081C RID: 2076
		// (get) Token: 0x06001B45 RID: 6981 RVA: 0x000A46CC File Offset: 0x000A28CC
		// (set) Token: 0x06001B46 RID: 6982 RVA: 0x000A46E4 File Offset: 0x000A28E4
		public bool RetrainMax
		{
			get
			{
				return this.retrainMax;
			}
			set
			{
				this.retrainMax = value;
				this.OnPropertyChanged("RetrainMax");
			}
		}

		// Token: 0x1700081D RID: 2077
		// (get) Token: 0x06001B47 RID: 6983 RVA: 0x000A46FC File Offset: 0x000A28FC
		// (set) Token: 0x06001B48 RID: 6984 RVA: 0x000A4714 File Offset: 0x000A2914
		public PreviewItem FirstSelected
		{
			get
			{
				return this.firstSelected;
			}
			set
			{
				this.firstSelected = value;
				this.OnPropertyChanged("FirstSelected");
			}
		}

		// Token: 0x1700081E RID: 2078
		// (get) Token: 0x06001B49 RID: 6985 RVA: 0x000A472C File Offset: 0x000A292C
		// (set) Token: 0x06001B4A RID: 6986 RVA: 0x000A4744 File Offset: 0x000A2944
		public bool Widescreen
		{
			get
			{
				return this.widescreen;
			}
			set
			{
				this.widescreen = value;
				this.OnPropertyChanged("Widescreen");
			}
		}

		// Token: 0x1700081F RID: 2079
		// (get) Token: 0x06001B4B RID: 6987 RVA: 0x000A475C File Offset: 0x000A295C
		// (set) Token: 0x06001B4C RID: 6988 RVA: 0x000A4774 File Offset: 0x000A2974
		public bool FileListVisible
		{
			get
			{
				return this.fileListVisible;
			}
			set
			{
				this.fileListVisible = value;
				this.OnPropertyChanged("FileListVisible");
			}
		}

		// Token: 0x17000820 RID: 2080
		// (get) Token: 0x06001B4D RID: 6989 RVA: 0x000A478A File Offset: 0x000A298A
		// (set) Token: 0x06001B4E RID: 6990 RVA: 0x000A4791 File Offset: 0x000A2991
		public static bool cancelSaving { get; set; }

		// Token: 0x17000821 RID: 2081
		// (get) Token: 0x06001B4F RID: 6991 RVA: 0x000A479C File Offset: 0x000A299C
		// (set) Token: 0x06001B50 RID: 6992 RVA: 0x000A47B4 File Offset: 0x000A29B4
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

		// Token: 0x17000822 RID: 2082
		// (get) Token: 0x06001B51 RID: 6993 RVA: 0x000A47E8 File Offset: 0x000A29E8
		public ProductData CurrentLoadedProduct
		{
			get
			{
				bool flag = this.productManager != null;
				ProductData result;
				if (flag)
				{
					result = this.productManager.CurrentLoadedProduct;
				}
				else
				{
					result = null;
				}
				return result;
			}
		}

		// Token: 0x17000823 RID: 2083
		// (get) Token: 0x06001B52 RID: 6994 RVA: 0x000A4818 File Offset: 0x000A2A18
		// (set) Token: 0x06001B53 RID: 6995 RVA: 0x000A4830 File Offset: 0x000A2A30
		public string Number
		{
			get
			{
				return this.number;
			}
			set
			{
				bool flag = value != null;
				if (flag)
				{
					this.number = value;
				}
				else
				{
					this.number = "0";
				}
				this.OnPropertyChanged("Number");
			}
		}

		// Token: 0x1400008A RID: 138
		// (add) Token: 0x06001B54 RID: 6996 RVA: 0x000A486C File Offset: 0x000A2A6C
		// (remove) Token: 0x06001B55 RID: 6997 RVA: 0x000A48A4 File Offset: 0x000A2AA4
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action NotRetrained;

		// Token: 0x17000824 RID: 2084
		// (get) Token: 0x06001B56 RID: 6998 RVA: 0x000A48D9 File Offset: 0x000A2AD9
		// (set) Token: 0x06001B57 RID: 6999 RVA: 0x000A48E1 File Offset: 0x000A2AE1
		public BoundLocalizedText StrRetrainMax { get; set; }

		// Token: 0x06001B58 RID: 7000 RVA: 0x000A48EC File Offset: 0x000A2AEC
		public HistoryViewModel(XRSImageBox imagebox, string[] imageFolders, int waitTime, ParameterHandling parHandling, ImageDataTransfer imgTransfer, DBConnection dbConnection, ProductManager productManager, Lane lane, int imageGroup, StoreImageData imageArchiver, User user, TestcardSimService testcardSimService, Sensitivity bulksensitivity, int number) : base(imagebox, imageFolders, waitTime, lane, imageGroup, imageArchiver, false, false, "", number)
		{
			this.parHandling = parHandling;
			this.imgTransfer = imgTransfer;
			this.dbConnection = dbConnection;
			this.productManager = productManager;
			this.user = user;
			this.testcardSimService = testcardSimService;
			this.bulksensitivity = bulksensitivity;
			this.numberToShow = number;
			this.Widescreen = (Definition.windowWidth >= 800.0);
			this.SelectedImages = new List<PreviewItem>();
			base.Preview.FirstItemCreated += this.Preview_FirstItemCreated;
			this.InitializeLocalizedText();
		}

		// Token: 0x06001B59 RID: 7001 RVA: 0x000A4A02 File Offset: 0x000A2C02
		private void InitializeLocalizedText()
		{
			this.StrRetrainMax = new BoundLocalizedText("txtRetrainMax");
		}

		// Token: 0x06001B5A RID: 7002 RVA: 0x000A4A16 File Offset: 0x000A2C16
		public void DeregisterLocalizedText()
		{
			MainWindow.localizationManager.Deregister(this.StrRetrainMax);
		}

		// Token: 0x06001B5B RID: 7003 RVA: 0x000A4A2C File Offset: 0x000A2C2C
		public void Preview_FirstItemCreated()
		{
			bool flag = base.Preview.PreviewItems.Count > 0;
			if (flag)
			{
				this.FirstSelected = base.Preview.PreviewItems[0];
				bool flag2 = this.FirstSelected.XrsImage.Height > 2000;
				if (flag2)
				{
					this.maximumNumber = 1;
				}
				else
				{
					bool flag3 = this.FirstSelected.XrsImage.Height > 1000;
					if (flag3)
					{
						this.maximumNumber = 5;
					}
					else
					{
						this.maximumNumber = 10;
					}
				}
			}
		}

		// Token: 0x06001B5C RID: 7004 RVA: 0x000A4AC0 File Offset: 0x000A2CC0
		public bool Retrain()
		{
			bool flag = base.SelectedImage != null;
			if (flag)
			{
				bool flag2 = Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtOverwriteExisting"), DialogStyle.Warning, DialogButtons.OKCancel) == Dialog.DialogResults.Ok;
				if (flag2)
				{
					this.numberItem = 0;
					base.SelectedImage = this.SelectedImages.First<PreviewItem>();
					this.retrain = true;
					this.allRetrained = true;
					this.prgRetrain = new Progress(LocalizationManager.Localize("txtRetrain") + "...", false);
					this.prgRetrain.Show();
					this.bgRetrain = new BackgroundWorker();
					this.bgRetrain.WorkerReportsProgress = true;
					this.bgRetrain.DoWork += this.bgRetrain_DoWork;
					this.bgRetrain.WorkerSupportsCancellation = true;
					this.bgRetrain.RunWorkerCompleted += this.bgRetrain_RunWorkerCompleted;
					this.bgRetrain.ProgressChanged += this.bgRetrain_ProgressChanged;
					this.bgRetrain.RunWorkerAsync();
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001B5D RID: 7005 RVA: 0x000A4BDC File Offset: 0x000A2DDC
		public bool doRetrain()
		{
			bool result = false;
			bool flag = base.SelectedImage != null;
			if (flag)
			{
				bool flag2 = base.SelectedImage.XrsImage != null && base.SelectedImage.XrsImage.Data != null;
				if (flag2)
				{
					this.isTestcardSimActive = (this.parHandling.ReadPar(Definition.testcardSimulationPath) == "1" && this.parHandling.ReadPar(Definition.simulationAfterAutolearn) == "1");
					this.SetContinuousModeBulk(false, false);
					this.bgRetrain.ReportProgress(this.numberItem * 100 / this.SelectedImages.Count<PreviewItem>());
					bool flag3 = !this.productManager.CurrentLoadedProduct.IsRegionProduct;
					if (flag3)
					{
						Filter.WriteToImageProcessingFilters(this.parHandling, "Retrain", "1", (int)base.SelectedImage.XrsImage.Group);
						bool flag4 = this.first;
						if (flag4)
						{
							Filter.SaveActualLearnedParams(this.parHandling, (int)base.SelectedImage.XrsImage.Group);
							this.first = false;
						}
					}
					else
					{
						for (int i = 0; i < this.productManager.CurrentLoadedProduct.SubProducts.Count; i++)
						{
							Filter.WriteToImageProcessingFilters(this.parHandling, "Retrain", "1", (int)this.productManager.CurrentLoadedProduct.SubProducts[i].Group);
						}
					}
					this.imgTransfer.FrameArrived += this.imgTransfer_FrameArrived;
					bool flag5 = base.SelectedImage.XrsImage.IsBulkOverlapImage && !XRSImageBoxViewModel.BulkOverlapImageShow;
					if (flag5)
					{
						StoreImageData storeImageData = new StoreImageData();
						XRSImage image = storeImageData.GetImage(base.SelectedImage.XrsImage.FilePath);
						this.imgTransfer.SendImage(image);
					}
					else
					{
						this.imgTransfer.SendImage(base.SelectedImage.XrsImage);
					}
				}
				result = true;
			}
			else
			{
				Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
				{
					Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtImagesDontExist"), DialogStyle.Warning, DialogButtons.None, true);
				}));
			}
			return result;
		}

		// Token: 0x06001B5E RID: 7006 RVA: 0x000A4E28 File Offset: 0x000A3028
		public void UndoRetrain()
		{
			bool flag = base.SelectedImage != null;
			if (flag)
			{
				bool flag2 = Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtUndoRetrain"), DialogStyle.Warning, DialogButtons.OKCancel) == Dialog.DialogResults.Ok;
				if (flag2)
				{
					bool flag3 = base.SelectedImage.XrsImage != null;
					if (flag3)
					{
						this.isTestcardSimActive = (this.parHandling.ReadPar(Definition.testcardSimulationPath) == "1" && this.parHandling.ReadPar(Definition.simulationAfterAutolearn) == "1");
						this.SetContinuousModeBulk(false, false);
						Progress.ShowStaticProgress("Undo Retraining..", this.isTestcardSimActive);
						bool flag4 = !this.productManager.CurrentLoadedProduct.IsRegionProduct;
						if (flag4)
						{
							Filter.SetPreviousLearnedParams(this.parHandling);
						}
						this.allRetrained = true;
						this.numberItem = 0;
						this.imgTransfer.FrameArrived += this.imgTransfer_FrameArrivedUndoRetrain;
						foreach (PreviewItem selectedImage in this.SelectedImages)
						{
							base.SelectedImage = selectedImage;
							bool flag5 = base.SelectedImage.XrsImage.Data == null;
							if (flag5)
							{
								this.allRetrained = false;
							}
							else
							{
								this.imgTransfer.SendImage(base.SelectedImage.XrsImage);
							}
						}
						this.first = false;
					}
				}
			}
		}

		// Token: 0x06001B5F RID: 7007 RVA: 0x000A4FC0 File Offset: 0x000A31C0
		private void imgTransfer_FrameArrived(object sender, ReceiveEventArgs e)
		{
			bool flag = e.Image.Type == ImageType.Demo || e.Image.Type == ImageType.TrainAI;
			if (flag)
			{
				this.imgTransfer.FrameArrived -= this.imgTransfer_FrameArrived;
				this.done = true;
				this.SetContinuousModeBulk(true, true);
				bool flag2 = !this.productManager.CurrentLoadedProduct.IsRegionProduct;
				if (flag2)
				{
					Filter.UpdateImageProcessingFilterPaths(this.dbConnection, this.parHandling, (int)e.Image.Group, this.productManager, false);
					bool flag3 = this.parHandling.ReadPar(Definition.anomalyDetectionEnabled) == "1" && this.parHandling.ReadPar(Definition.anomalyDetectionInferenceEnabled) == "1";
					if (flag3)
					{
						Filter.UpdateAnomalyDetectionInferenceThresholds(this.dbConnection, this.parHandling);
					}
					bool flag4 = this.parHandling.ReadPar(Definition.objectDetectionEnabled) == "1" && this.parHandling.ReadPar(Definition.objectDetectionInferenceEnabled) == "1";
					if (flag4)
					{
						Filter.UpdateObjectDetectionInferenceThresholds(this.dbConnection, this.parHandling);
					}
				}
				else
				{
					int[] array = new int[this.productManager.CurrentLoadedProduct.SubProducts.Count];
					for (int i = 0; i < this.productManager.CurrentLoadedProduct.SubProducts.Count; i++)
					{
						array[i] = (int)this.productManager.CurrentLoadedProduct.SubProducts[i].Group;
					}
					Filter.UpdateImageProcessingFilterPathsForRegionProd(this.dbConnection, this.parHandling, array, this.productManager);
				}
				bool flag5 = !this.retrain;
				if (flag5)
				{
					Progress.CloseStaticProgress(false);
				}
				LogbookManager.AddEntry(SSTEntryCode.Retrain, 0, this.productManager.CurrentLoadedProduct.ProductNumber, this.productManager.CurrentLoadedProduct.ProductName);
				bool flag6 = this.numberItem < this.SelectedImages.Count<PreviewItem>() && this.retrain;
				if (!flag6)
				{
					this.first = true;
					this.retrain = false;
					bool flag7 = !this.allRetrained;
					if (flag7)
					{
						bool flag8 = Application.Current != null;
						if (flag8)
						{
							Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
							{
								Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtNotAllRetrained"), DialogStyle.Warning, DialogButtons.OK, false);
							}));
						}
					}
					else
					{
						bool flag9 = this.canceled;
						if (flag9)
						{
							this.canceled = false;
							bool flag10 = Application.Current != null;
							if (flag10)
							{
								Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
								{
									Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtTooManyImages"), DialogStyle.Warning, DialogButtons.None, true);
								}));
							}
						}
						else
						{
							MachineMode machineMode = MachineMode.Standard;
							this.dbConnection.GetMachineMode(ref machineMode);
							bool flag11 = machineMode == MachineMode.Bulk;
							if (flag11)
							{
								this.setValueTo100ForBulkSlider();
							}
							bool flag12 = Application.Current != null;
							if (flag12)
							{
								Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
								{
									Dialog.Show(LocalizationManager.Localize("txtInfo"), LocalizationManager.Localize("txtSuccess"), DialogStyle.Info, DialogButtons.None, true);
								}));
							}
						}
					}
					this.allRetrained = true;
					bool flag13 = this.isTestcardSimActive && !this.productManager.CurrentLoadedProduct.IsRegionProduct && e.Image.Type != ImageType.TestcardSimulation && this.parHandling.ReadPar(Definition.autoKVAdjustmentPath) == "1";
					if (flag13)
					{
						this.testcardSimulation(e.Image);
					}
				}
			}
		}

		// Token: 0x06001B60 RID: 7008 RVA: 0x000A5380 File Offset: 0x000A3580
		private void setValueTo100ForBulkSlider()
		{
			bool flag = Application.Current != null;
			if (flag)
			{
				Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
				{
					bool flag2 = this.bulksensitivity != null;
					if (flag2)
					{
						bool flag3 = this.bulksensitivity.SensitivityPercent != 100;
						if (flag3)
						{
							this.bulksensitivity.calculateBulkSliderContrasts = false;
							this.bulksensitivity.SensitivityPercent = 100;
							Dialog.Show(LocalizationManager.Localize("txtInfo"), LocalizationManager.Localize("txtBulkRetrain"), DialogStyle.Info, DialogButtons.OK);
						}
					}
				}));
			}
		}

		// Token: 0x06001B61 RID: 7009 RVA: 0x000A53BC File Offset: 0x000A35BC
		private void imgTransfer_FrameArrivedUndoRetrain(object sender, ReceiveEventArgs e)
		{
			bool flag = e.Image.Type == ImageType.Demo;
			if (flag)
			{
				this.numberItem++;
				bool flag2 = this.numberItem == this.SelectedImages.Count;
				if (flag2)
				{
					bool flag3 = Application.Current != null;
					if (flag3)
					{
						Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
						{
							Dialog.Show(LocalizationManager.Localize("txtInfo"), LocalizationManager.Localize("txtSuccess"), DialogStyle.Info, DialogButtons.None, true);
						}));
					}
					Progress.CloseStaticProgress(false);
					LogbookManager.AddEntry(SSTEntryCode.UndoRetrain);
					this.imgTransfer.FrameArrived -= this.imgTransfer_FrameArrivedUndoRetrain;
					bool flag4 = this.isTestcardSimActive && !this.productManager.CurrentLoadedProduct.IsRegionProduct && e.Image.Type != ImageType.TestcardSimulation && this.parHandling.ReadPar(Definition.autoKVAdjustmentPath) == "1";
					if (flag4)
					{
						bool flag5 = Application.Current != null;
						if (flag5)
						{
							Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
							{
								bool flag6 = false;
								foreach (PreviewItem selectedImage in this.Preview.PreviewItems)
								{
									this.SelectedImage = selectedImage;
									bool flag7 = this.SelectedImage.XrsImage.ProductNumber == 0U && this.SelectedImage.XrsImage.ProductName.Equals("Reference Product");
									if (flag7)
									{
										bool flag8 = this.CurrentLoadedProduct.ProductNumber.Equals("-1") && this.SelectedImage.XrsImage.Data != null;
										if (flag8)
										{
											flag6 = true;
											break;
										}
									}
									else
									{
										bool flag9 = this.SelectedImage.XrsImage.ProductNumber.ToString().Equals(this.CurrentLoadedProduct.ProductNumber) && this.SelectedImage.XrsImage.Data != null;
										if (flag9)
										{
											flag6 = true;
											break;
										}
									}
								}
								bool flag10 = flag6;
								if (flag10)
								{
									bool flag11 = this.SelectedImage.XrsImage.State >= 128;
									if (flag11)
									{
										Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtTestcardSimNotPossible"), DialogStyle.Warning, DialogButtons.OK);
									}
									else
									{
										DialogButtons buttonOption = (this.parHandling.ReadPar(Definition.simulationAfterTimeIntervall) == "1") ? DialogButtons.OK : DialogButtons.OKCancel;
										bool flag12 = Dialog.Show(LocalizationManager.Localize("txtInfo"), LocalizationManager.Localize("txtCalcSensQuestionTestcardSim") + Environment.NewLine + this.testcardSimService.setExpectedDurationMsg((int)(e.Image.Height * e.Image.Width), true), DialogStyle.Info, buttonOption) == Dialog.DialogResults.Ok;
										if (flag12)
										{
											Progress.ShowStaticProgress(LocalizationManager.Localize("txtCalcSensTestcardSim"), this.isTestcardSimActive);
											this.imgTransfer.FrameArrived += this.imageTestcardSimTransfer_FrameArrived;
											this.sendTestcardSimImagesWithActSens(e.Image);
										}
									}
								}
								else
								{
									Dialog.Show(LocalizationManager.Localize("txtInfo"), LocalizationManager.Localize("txtSensitivityNotPossible"), DialogStyle.Info, DialogButtons.OK);
								}
							}));
						}
					}
				}
			}
		}

		// Token: 0x06001B62 RID: 7010 RVA: 0x000A5510 File Offset: 0x000A3710
		private void sendTestcardSimImagesWithActSens(XRSImage xrsimage)
		{
			this.testcardSimService.TestcardSimAfterTrainingSendImg(false, xrsimage, false);
			bool flag = Application.Current != null;
			if (flag)
			{
				Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
				{
					bool checkVersionAndXRaySource = this.testcardSimService.CheckVersionAndXRaySource;
					if (checkVersionAndXRaySource)
					{
						Progress.CloseStaticProgress(false);
						this.imgTransfer.FrameArrived -= this.imageTestcardSimTransfer_FrameArrived;
						Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtTestcardSimNotPossible"), DialogStyle.Warning, DialogButtons.OK);
					}
				}));
			}
		}

		// Token: 0x06001B63 RID: 7011 RVA: 0x000A555C File Offset: 0x000A375C
		private void imageTestcardSimTransfer_FrameArrived(object sender, ReceiveEventArgs e)
		{
			bool flag = e.Image.Type == ImageType.TestcardSimulation;
			if (flag)
			{
				bool flag2 = this.testcardSimService.TestcardSimAfterTrainingGetImg(e.Image, false);
				if (flag2)
				{
					Progress.CloseStaticProgress(false);
					bool flag3 = Application.Current != null;
					if (flag3)
					{
						Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
						{
							bool flag4 = Dialog.Show(this.testcardSimService.setRetrainTestcardSimMsg(true), this.testcardSimService.setRetrainTestcardSimMsg(false), this.testcardSimService.setRetrainTestcardSimMsgLevel(), DialogButtons.OK) == Dialog.DialogResults.Ok;
							if (flag4)
							{
								this.testcardSimService.setParameterAfterTestcardSim(false);
								this.imgTransfer.FrameArrived -= this.imageTestcardSimTransfer_FrameArrived;
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
								bool checkBallCounts = this.testcardSimService.CheckBallCounts;
								if (checkBallCounts)
								{
									Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtErrorGeneralTestcardSim"), DialogStyle.Warning, DialogButtons.OK);
								}
							}
						}));
					}
				}
			}
		}

		// Token: 0x06001B64 RID: 7012 RVA: 0x000A55CC File Offset: 0x000A37CC
		public void CommentXri()
		{
			bool flag = base.SelectedImage != null;
			if (flag)
			{
				bool flag2 = base.SelectedImage.XrsImage.Version < 7;
				if (flag2)
				{
					Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("msgCommentNotPossible"), DialogStyle.Error, DialogButtons.OK);
				}
				else
				{
					string[] array = base.SelectedImage.XrsImage.Comment.TrimEnd(new char[]
					{
						' '
					}).Split(new char[]
					{
						'\n'
					});
					string text = "";
					for (int i = 0; i < array.Length - 1; i++)
					{
						text += array[i];
					}
					this.keyboard = new Keyboard(LocalizationManager.Localize("txtComment"), text, KeyboardType.Application);
					this.keyboard.HasEnterButton = true;
					this.keyboard.MaxTextLength = 80;
					this.keyboard.TextInputKeyboard += this.keyboard_InputComment;
					this.keyboard.ShowDialog();
				}
			}
		}

		// Token: 0x06001B65 RID: 7013 RVA: 0x000A56D8 File Offset: 0x000A38D8
		private void keyboard_InputComment(object sender, KeyboardInputEventArgs e)
		{
			base.SelectedImage.XrsImage.Comment = e.Text;
			XRSImage xrsImage = base.SelectedImage.XrsImage;
			xrsImage.Comment = xrsImage.Comment + "\n" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
			base.SelectedImage.XrsImage.Comment = base.SelectedImage.XrsImage.Comment.PadRight(100);
			base.SelectedImage.XrsImage.EditorComment = (this.user.ID.ToString() + " - " + this.user.Username).PadRight(50);
			base.SelectedImage.XrsImage.SaveCommentToFile();
			this.keyboard.TextInputKeyboard -= this.keyboard_InputComment;
		}

		// Token: 0x06001B66 RID: 7014 RVA: 0x000A57C4 File Offset: 0x000A39C4
		public void SaveToUSB()
		{
			bool flag = base.SelectedImage != null;
			if (flag)
			{
				USBSave usbsave = new USBSave();
				string usbdriveLetter = "";
				bool saveEdgePixelInPNG = false;
				this.dbConnection.GetUsbDriveLetter(ref usbdriveLetter);
				this.dbConnection.GetSaveEdgePixelInPNG(ref saveEdgePixelInPNG);
				usbsave.SaveToUSB(base.SelectedImage.XrsImage, usbdriveLetter, saveEdgePixelInPNG, base.SelectedImage.Timestamp);
			}
		}

		// Token: 0x06001B67 RID: 7015 RVA: 0x000A582C File Offset: 0x000A3A2C
		public void SaveToUSBWithName(string name)
		{
			bool flag = base.SelectedImage != null;
			if (flag)
			{
				USBSave usbsave = new USBSave();
				string usbdriveLetter = "";
				bool saveEdgePixelInPNG = false;
				this.dbConnection.GetUsbDriveLetter(ref usbdriveLetter);
				this.dbConnection.GetSaveEdgePixelInPNG(ref saveEdgePixelInPNG);
				usbsave.SaveToUSBWithName(base.SelectedImage.XrsImage, usbdriveLetter, saveEdgePixelInPNG, base.SelectedImage.Timestamp, name);
			}
		}

		// Token: 0x06001B68 RID: 7016 RVA: 0x000A5894 File Offset: 0x000A3A94
		public void SaveAllToUSB()
		{
			string str = "";
			try
			{
				bool flag = base.SelectedImage != null;
				if (flag)
				{
					this.dbConnection.GetUsbDriveLetter(ref str);
					bool flag2 = Directory.Exists(str + "\\");
					if (flag2)
					{
						bool flag3 = Dialog.Show(LocalizationManager.Localize("txtInfo"), LocalizationManager.Localize("txtSaveImages") + "?", DialogStyle.Info, DialogButtons.OKCancel) == Dialog.DialogResults.Ok;
						if (flag3)
						{
							this.prgSaveAllImages = new Progress(LocalizationManager.Localize("txtSavingImage") + "...", false);
							this.prgSaveAllImages.Show();
							this.bgSaveImages = new BackgroundWorker();
							this.bgSaveImages.WorkerReportsProgress = true;
							this.bgSaveImages.DoWork += this.bgSaveImages_DoWork;
							this.bgSaveImages.RunWorkerCompleted += this.bgSaveImages_RunWorkerCompleted;
							this.bgSaveImages.ProgressChanged += this.bgSaveImages_ProgressChanged;
							this.bgSaveImages.RunWorkerAsync();
						}
					}
					else
					{
						Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtNoUSB"), DialogStyle.Error);
					}
				}
			}
			catch
			{
				Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtError"), DialogStyle.Error, DialogButtons.None);
			}
		}

		// Token: 0x06001B69 RID: 7017 RVA: 0x000A5A08 File Offset: 0x000A3C08
		private void bgSaveImages_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			bool flag = this.prgSaveAllImages != null;
			if (flag)
			{
				this.prgSaveAllImages.progBar.Value = (double)e.ProgressPercentage;
			}
		}

		// Token: 0x06001B6A RID: 7018 RVA: 0x000A5A3C File Offset: 0x000A3C3C
		private void bgDelete_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			bool flag = this.prgDelete != null;
			if (flag)
			{
				this.prgDelete.progBar.Value = (double)e.ProgressPercentage;
			}
		}

		// Token: 0x06001B6B RID: 7019 RVA: 0x000A5A70 File Offset: 0x000A3C70
		private void bgRetrain_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			bool flag = this.prgRetrain != null;
			if (flag)
			{
				this.prgRetrain.progBar.Value = (double)e.ProgressPercentage;
			}
		}

		// Token: 0x06001B6C RID: 7020 RVA: 0x000A5AA4 File Offset: 0x000A3CA4
		private void waitForImage()
		{
			int millisecondsDelay = 30000;
			bool flag = this.isTestcardSimActive;
			if (flag)
			{
				millisecondsDelay = 90000;
			}
			CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(millisecondsDelay);
			while (!this.done && !cancellationTokenSource.IsCancellationRequested)
			{
			}
			bool isCancellationRequested = cancellationTokenSource.IsCancellationRequested;
			if (isCancellationRequested)
			{
				this.canceled = true;
				this.bgRetrain.CancelAsync();
			}
			this.done = false;
		}

		// Token: 0x06001B6D RID: 7021 RVA: 0x000A5B18 File Offset: 0x000A3D18
		private void bgDelete_DoWork(object sender, DoWorkEventArgs e)
		{
			List<PreviewItem> selectedImages = this.SelectedImages;
			for (int i = 0; i < selectedImages.Count; i++)
			{
				int percentProgress = 100 * i / selectedImages.Count<PreviewItem>();
				this.bgDelete.ReportProgress(percentProgress);
				int k;
				int j;
				for (j = 0; j < base.Preview.PreviewItems.Count; j = k + 1)
				{
					bool flag = selectedImages[i].Timestamp == base.Preview.PreviewItems[j].Timestamp;
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
			this.SelectedImages.Clear();
			bool flag4 = base.Preview.PreviewItems.Count > 0;
			if (flag4)
			{
				base.SelectedImage = base.Preview.PreviewItems[0];
				this.SelectedImages.Add(base.SelectedImage);
			}
			else
			{
				base.SelectedImage = null;
			}
		}

		// Token: 0x06001B6E RID: 7022 RVA: 0x000A5CBC File Offset: 0x000A3EBC
		private void bgSaveImages_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			bool flag = this.prgSaveAllImages != null;
			if (flag)
			{
				this.prgSaveAllImages.Close();
			}
			bool flag2 = !this.notAllSaved;
			if (flag2)
			{
				Dialog.Show(LocalizationManager.Localize("txtSuccess"), LocalizationManager.Localize("txtSaveSuccessful"), DialogStyle.Ok, DialogButtons.None, true);
			}
			else
			{
				Dialog.Show(LocalizationManager.Localize("txtInfo"), LocalizationManager.Localize("txtImagesNotAllSaved"), DialogStyle.Info, DialogButtons.OK);
			}
		}

		// Token: 0x06001B6F RID: 7023 RVA: 0x000A5D2F File Offset: 0x000A3F2F
		private void bgRetrain_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			this.prgRetrain.Close();
		}

		// Token: 0x06001B70 RID: 7024 RVA: 0x000A5D3E File Offset: 0x000A3F3E
		private void bgDelete_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			this.prgDelete.Close();
		}

		// Token: 0x06001B71 RID: 7025 RVA: 0x000A5D50 File Offset: 0x000A3F50
		private void bgRetrain_DoWork(object sender, DoWorkEventArgs e)
		{
			int num = 0;
			bool flag = Filter.CountAndUnmaskForContEnabled(this.parHandling);
			foreach (PreviewItem previewItem in this.SelectedImages)
			{
				base.SelectedImage = previewItem;
				bool flag2 = previewItem.XrsImage.Data != null;
				if (flag2)
				{
					bool flag3 = !previewItem.XrsImage.ProductNumber.ToString().Equals(this.CurrentLoadedProduct.ProductNumber) && (!this.CurrentLoadedProduct.ProductNumber.Equals("-1") || !previewItem.XrsImage.ProductName.Equals("Reference Product"));
					if (flag3)
					{
						Dialog.DialogResults result = Dialog.DialogResults.Cancel;
						Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
						{
							result = Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtWrongImages"), DialogStyle.Warning, DialogButtons.OKCancel);
						}));
						bool flag4 = result == Dialog.DialogResults.Cancel;
						if (flag4)
						{
							bool flag5 = num == 0;
							if (flag5)
							{
								this.OnNotRetrained();
							}
							break;
						}
					}
					bool flag6 = flag && previewItem.XrsImage.HasFalseCount();
					if (flag6)
					{
						this.bgRetrain.ReportProgress(this.numberItem * 100 / this.SelectedImages.Count<PreviewItem>());
						Thread.Sleep(5000);
						num++;
						bool flag7 = this.numberItem >= this.SelectedImages.Count<PreviewItem>() - 1;
						if (flag7)
						{
							bool flag8 = Application.Current != null;
							if (flag8)
							{
								Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
								{
									Dialog.Show(LocalizationManager.Localize("txtInfo"), LocalizationManager.Localize("txtSuccess"), DialogStyle.Info, DialogButtons.None, true);
								}));
								this.isTestcardSimActive = (this.parHandling.ReadPar(Definition.testcardSimulationPath) == "1" && this.parHandling.ReadPar(Definition.simulationAfterAutolearn) == "1");
								bool flag9 = this.isTestcardSimActive && !this.productManager.CurrentLoadedProduct.IsRegionProduct && this.parHandling.ReadPar(Definition.autoKVAdjustmentPath) == "1";
								if (flag9)
								{
									this.testcardSimulation(previewItem.XrsImage);
								}
							}
							this.first = true;
							this.retrain = false;
							break;
						}
						this.numberItem++;
						continue;
					}
					else
					{
						bool flag10 = this.doRetrain();
						if (flag10)
						{
							num++;
						}
						this.waitForImage();
						bool cancellationPending = this.bgRetrain.CancellationPending;
						if (cancellationPending)
						{
							break;
						}
					}
				}
				else
				{
					this.allRetrained = false;
				}
				this.numberItem++;
			}
		}

		// Token: 0x06001B72 RID: 7026 RVA: 0x000A6044 File Offset: 0x000A4244
		private void bgSaveImages_DoWork(object sender, DoWorkEventArgs e)
		{
			this.SaveAllImages();
		}

		// Token: 0x06001B73 RID: 7027 RVA: 0x000A6050 File Offset: 0x000A4250
		public void SaveSelectedToUSB()
		{
			bool flag = this.SelectedImages != null && this.SelectedImages.Count > 0;
			if (flag)
			{
				this.list.Clear();
				foreach (PreviewItem item in this.SelectedImages)
				{
					this.list.Add(item);
				}
				USBSave usbsave = new USBSave();
				string text = "";
				this.dbConnection.GetUsbDriveLetter(ref text);
				bool flag2 = this.SelectedImages.Count<PreviewItem>() == 1;
				if (flag2)
				{
					bool flag3 = base.SelectedImage != null;
					if (flag3)
					{
						bool flag4 = base.SelectedImage.XrsImage.Data != null;
						if (flag4)
						{
							bool saveEdgePixelInPNG = false;
							this.dbConnection.GetUsbDriveLetter(ref text);
							this.dbConnection.GetSaveEdgePixelInPNG(ref saveEdgePixelInPNG);
							usbsave.SaveToUSB(base.SelectedImage.XrsImage, text, saveEdgePixelInPNG, base.SelectedImage.Timestamp);
						}
						else
						{
							Dialog.Show(LocalizationManager.Localize("txtInfo"), LocalizationManager.Localize("txtImageNotSaved"), DialogStyle.Info, DialogButtons.OK);
						}
					}
				}
				else
				{
					bool flag5 = Directory.Exists(text + "\\");
					if (flag5)
					{
						Dialog.DialogResults dialogResults = Dialog.Show(LocalizationManager.Localize("txtInfo"), LocalizationManager.Localize("txtSaveImages") + "?", DialogStyle.Info, DialogButtons.OkRenameCancel);
						bool flag6 = dialogResults == Dialog.DialogResults.Rename;
						if (flag6)
						{
							int num = 1;
							foreach (PreviewItem selectedImage in this.SelectedImages)
							{
								bool cancelSaving = HistoryViewModel.cancelSaving;
								if (cancelSaving)
								{
									break;
								}
								base.SelectedImage = selectedImage;
								bool flag7 = base.SelectedImage.XrsImage.Data != null;
								if (flag7)
								{
									this.SaveToUSBWithName(LocalizationManager.Localize("txtNumOfChoosenImg") + " " + num.ToString());
								}
								else
								{
									Dialog.Show(LocalizationManager.Localize("txtInfo"), string.Concat(new string[]
									{
										LocalizationManager.Localize("txtImage"),
										" ",
										num.ToString(),
										": ",
										LocalizationManager.Localize("txtImageNotSaved")
									}), DialogStyle.Info, DialogButtons.OK);
								}
								num++;
							}
						}
						else
						{
							bool flag8 = dialogResults == Dialog.DialogResults.Ok;
							if (flag8)
							{
								this.prgSaveAllImages = new Progress(LocalizationManager.Localize("txtSaveImages") + "...", false);
								this.prgSaveAllImages.Show();
								this.bgSaveImages = new BackgroundWorker();
								this.bgSaveImages.WorkerReportsProgress = true;
								this.bgSaveImages.DoWork += this.bgSaveSelectedImages_DoWork;
								this.bgSaveImages.RunWorkerCompleted += this.bgSaveImages_RunWorkerCompleted;
								this.bgSaveImages.ProgressChanged += this.bgSaveImages_ProgressChanged;
								this.bgSaveImages.RunWorkerAsync();
							}
						}
					}
					else
					{
						Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtNoUSB"), DialogStyle.Error);
					}
				}
			}
		}

		// Token: 0x06001B74 RID: 7028 RVA: 0x000A63C0 File Offset: 0x000A45C0
		private void bgSaveSelectedImages_DoWork(object sender, DoWorkEventArgs e)
		{
			this.saveSelectedImages();
		}

		// Token: 0x06001B75 RID: 7029 RVA: 0x000A63CC File Offset: 0x000A45CC
		private void saveSelectedImages()
		{
			string usbdriveLetter = "";
			int num = 0;
			int num2 = 3;
			this.dbConnection.GetUsbDriveLetter(ref usbdriveLetter);
			PreviewItem selectedImage = this.list[0];
			this.notAllSaved = false;
			for (int i = 0; i < this.list.Count; i++)
			{
				base.SelectedImage = this.list[i];
				bool flag = base.SelectedImage.XrsImage.Data != null;
				if (flag)
				{
					bool flag2 = this.SaveSelectedImage(usbdriveLetter);
					while (!flag2)
					{
						num++;
						bool flag3 = num >= num2;
						if (flag3)
						{
							break;
						}
						Thread.Sleep(2000);
						flag2 = this.SaveSelectedImage(usbdriveLetter);
					}
					num = 0;
					this.bgSaveImages.ReportProgress((i + 1) * 100 / this.list.Count);
				}
				else
				{
					this.notAllSaved = true;
				}
			}
			base.SelectedImage = selectedImage;
		}

		// Token: 0x06001B76 RID: 7030 RVA: 0x000A64D4 File Offset: 0x000A46D4
		private void SaveAllImages()
		{
			string usbdriveLetter = "";
			int num = 0;
			int num2 = 3;
			this.dbConnection.GetUsbDriveLetter(ref usbdriveLetter);
			PreviewItem selectedImage = base.SelectedImage;
			for (int i = 0; i < base.Preview.PreviewItems.Count; i++)
			{
				base.SelectedImage = base.Preview.PreviewItems[i];
				bool flag = base.SelectedImage.XrsImage.Data != null;
				if (flag)
				{
					bool flag2 = this.SaveSelectedImage(usbdriveLetter);
					while (!flag2)
					{
						num++;
						bool flag3 = num >= num2;
						if (flag3)
						{
							break;
						}
						Thread.Sleep(2000);
						flag2 = this.SaveSelectedImage(usbdriveLetter);
					}
					num = 0;
				}
				this.bgSaveImages.ReportProgress((i + 1) * 100 / base.Preview.PreviewItems.Count);
			}
			base.SelectedImage = selectedImage;
		}

		// Token: 0x06001B77 RID: 7031 RVA: 0x000A65D0 File Offset: 0x000A47D0
		private bool SaveSelectedImage(string usbdriveLetter)
		{
			bool flag = base.SelectedImage != null && base.SelectedImage.XrsImage != null && base.SelectedImage.XrsImage.Width > 0 && base.SelectedImage.XrsImage.Height > 0 && base.SelectedImage.BitmapThumbnail != null;
			bool result;
			if (flag)
			{
				bool flag2 = base.SelectedImage.XrsImage.ProductName != null;
				string text;
				string text2;
				if (flag2)
				{
					bool flag3 = base.SelectedImage.XrsImage.ProductNumber == 0U && base.SelectedImage.XrsImage.ProductName.Equals("Reference Product");
					if (flag3)
					{
						text = string.Concat(new string[]
						{
							"\\RayconImages\\P-1_",
							base.SelectedImage.XrsImage.ProductName,
							"\\",
							base.SelectedImage.Timestamp,
							".xri"
						});
						text2 = string.Concat(new string[]
						{
							"\\RayconImages\\P-1_",
							base.SelectedImage.XrsImage.ProductName,
							"\\",
							base.SelectedImage.Timestamp,
							".png"
						});
					}
					else
					{
						text = string.Concat(new string[]
						{
							"\\RayconImages\\P",
							base.SelectedImage.XrsImage.ProductNumber.ToString(),
							"_",
							base.SelectedImage.XrsImage.ProductName,
							"\\",
							base.SelectedImage.Timestamp,
							".xri"
						});
						text2 = string.Concat(new string[]
						{
							"\\RayconImages\\P",
							base.SelectedImage.XrsImage.ProductNumber.ToString(),
							"_",
							base.SelectedImage.XrsImage.ProductName,
							"\\",
							base.SelectedImage.Timestamp,
							".png"
						});
					}
				}
				else
				{
					text = "\\RayconImages\\" + base.SelectedImage.Timestamp + ".xri";
					text2 = "\\RayconImages\\" + base.SelectedImage.Timestamp + ".png";
				}
				text = text.Replace(':', '-');
				text2 = text2.Replace(':', '-');
				this.dbConnection.GetUsbDriveLetter(ref usbdriveLetter);
				text = usbdriveLetter + text;
				text2 = usbdriveLetter + text2;
				bool flag4 = base.SelectedImage.XrsImage.ProductNumber == 0U && base.SelectedImage.XrsImage.ProductName.Equals("Reference Product");
				if (flag4)
				{
					bool flag5 = !Directory.Exists(usbdriveLetter + "\\RayconImages\\P-1_" + base.SelectedImage.XrsImage.ProductName);
					if (flag5)
					{
						Directory.CreateDirectory(usbdriveLetter + "\\RayconImages\\P-1_" + base.SelectedImage.XrsImage.ProductName);
					}
				}
				else
				{
					bool flag6 = !Directory.Exists(string.Concat(new string[]
					{
						usbdriveLetter,
						"\\RayconImages\\P",
						base.SelectedImage.XrsImage.ProductNumber.ToString(),
						"_",
						base.SelectedImage.XrsImage.ProductName
					}));
					if (flag6)
					{
						Directory.CreateDirectory(string.Concat(new string[]
						{
							usbdriveLetter,
							"\\RayconImages\\P",
							base.SelectedImage.XrsImage.ProductNumber.ToString(),
							"_",
							base.SelectedImage.XrsImage.ProductName
						}));
					}
				}
				bool flag7 = !File.Exists(text) && base.SelectedImage != null && base.SelectedImage.XrsImage != null;
				if (flag7)
				{
					base.SelectedImage.XrsImage.SaveToFile(text);
				}
				bool flag8 = !File.Exists(text2) && base.SelectedImage != null && base.SelectedImage.XrsImage != null;
				if (flag8)
				{
					bool saveEdgeFilteredPixel = false;
					this.dbConnection.GetSaveEdgePixelInPNG(ref saveEdgeFilteredPixel);
					FileStream fileStream = new FileStream(text2, FileMode.Create);
					new PngBitmapEncoder
					{
						Frames = 
						{
							BitmapFrame.Create(base.SelectedImage.XrsImage.GetCombinedImage(saveEdgeFilteredPixel))
						}
					}.Save(fileStream);
					fileStream.Close();
				}
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06001B78 RID: 7032 RVA: 0x000A6A58 File Offset: 0x000A4C58
		public void PrintReport()
		{
			bool flag = base.SelectedImage != null;
			if (flag)
			{
				string printerName = null;
				this.dbConnection.GetPrinterName(ref printerName);
				bool saveEdgeFilteredPixel = false;
				this.dbConnection.GetSaveEdgePixelInPNG(ref saveEdgeFilteredPixel);
				CtrlPrintReport element = new CtrlPrintReport(base.SelectedImage.XrsImage.ProductName, base.SelectedImage.XrsImage.GetCombinedImage(saveEdgeFilteredPixel));
				Dispatcher.CurrentDispatcher.Invoke(new Action(delegate()
				{
				}), DispatcherPriority.ApplicationIdle, null);
				Printing.Print(element, "GUI_XRS: " + base.SelectedImage.XrsImage.ProductName, printerName);
			}
		}

		// Token: 0x06001B79 RID: 7033 RVA: 0x000A6B10 File Offset: 0x000A4D10
		public void ShowRetrainRegion()
		{
			bool flag = base.SelectedImage != null && base.SelectedImage.XrsImage != null;
			if (flag)
			{
				CtrlRetrainRegion ctrlRetrainRegion = new CtrlRetrainRegion(this.dbConnection, this.parHandling, this.imgTransfer, base.SelectedImage.XrsImage.DeepCopy(), this.productManager, this.testcardSimService);
				Subwindow.Show(ctrlRetrainRegion, SubwindowButtons.Back, ctrlRetrainRegion.PanelSubButtons);
			}
		}

		// Token: 0x06001B7A RID: 7034 RVA: 0x000A6B80 File Offset: 0x000A4D80
		public void DeletMarkedImages()
		{
			bool flag = this.SelectedImages.Count > 0 && this.SelectedImages != null;
			if (flag)
			{
				bool flag2 = base.Preview.PreviewItems.Count > 0 && Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtDeleteSelected") + "?", DialogStyle.Warning, DialogButtons.OKCancel) == Dialog.DialogResults.Ok;
				if (flag2)
				{
					bool flag3 = this.AllImagesAreMarked || this.SelectedImages.Count == base.Preview.PreviewItems.Count;
					if (flag3)
					{
						for (int i = 0; i < base.Preview.PreviewItems.Count; i++)
						{
							bool flag4 = File.Exists(base.Preview.PreviewItems[i].FileName);
							if (flag4)
							{
								File.Delete(base.Preview.PreviewItems[i].FileName);
							}
						}
						base.Preview.PreviewItems.Clear();
						this.AllImagesAreMarked = false;
						this.Number = "0";
						base.SelectedImage = null;
						base.Preview.AbortCreation();
						this.imagebox.Reset();
					}
					else
					{
						bool flag5 = base.Preview.PreviewItems != null && this.SelectedImages != null;
						if (flag5)
						{
							this.prgDelete = new Progress(LocalizationManager.Localize("txtDeleteSelected"), false);
							this.prgDelete.Show();
							this.bgDelete = new BackgroundWorker();
							this.bgDelete.WorkerReportsProgress = true;
							this.bgDelete.DoWork += this.bgDelete_DoWork;
							this.bgDelete.WorkerSupportsCancellation = true;
							this.bgDelete.RunWorkerCompleted += this.bgDelete_RunWorkerCompleted;
							this.bgDelete.ProgressChanged += this.bgDelete_ProgressChanged;
							this.bgDelete.RunWorkerAsync();
						}
					}
				}
			}
		}

		// Token: 0x06001B7B RID: 7035 RVA: 0x000A6D94 File Offset: 0x000A4F94
		public void RetrainGroup()
		{
			bool flag = base.SelectedImage != null;
			if (flag)
			{
				this.productManager.SelectedProduct = this.productManager.CurrentLoadedProduct;
				CtrlSubProduct ctrlSubProduct = new CtrlSubProduct(this.productManager, SubProductModes.RetrainGroup, base.SelectedImage.XrsImage, null);
				Subwindow.Show(ctrlSubProduct, SubwindowButtons.BackAndHome, ctrlSubProduct.Buttons);
			}
		}

		// Token: 0x06001B7C RID: 7036 RVA: 0x000A6DF0 File Offset: 0x000A4FF0
		public void SetContinuousModeBulk(bool activate, bool checkXrayOn)
		{
			string value = activate ? "1" : "0";
			MachineMode machineMode = MachineMode.Standard;
			this.dbConnection.GetMachineMode(ref machineMode);
			bool flag = true;
			bool flag2 = machineMode == MachineMode.Bulk;
			if (flag2)
			{
				if (checkXrayOn)
				{
					flag = (this.parHandling.ReadPar(XrayState.sourceXrayOnPath) == "1");
				}
				bool flag3 = flag;
				if (flag3)
				{
					this.parHandling.WriteValue("/filter/acquisition/ContinuousMode/Value", value);
					this.parHandling.WriteValue("/filter/acquisition/ContinuousModeFiltering/Value", value);
					bool flag4 = !activate;
					if (flag4)
					{
						Thread.Sleep(200);
					}
				}
			}
		}

		// Token: 0x06001B7D RID: 7037 RVA: 0x000A6E94 File Offset: 0x000A5094
		public virtual void OnNotRetrained()
		{
			Action notRetrained = this.NotRetrained;
			bool flag = notRetrained != null;
			if (flag)
			{
				notRetrained();
			}
		}

		// Token: 0x06001B7E RID: 7038 RVA: 0x000A6EBC File Offset: 0x000A50BC
		public void SaveWithDirectoryname()
		{
			bool flag = this.SelectedImages != null && this.SelectedImages.Count > 0;
			if (flag)
			{
				this.list.Clear();
				foreach (PreviewItem item in this.SelectedImages)
				{
					this.list.Add(item);
				}
				USBSave usbsave = new USBSave();
				string str = "";
				this.dbConnection.GetUsbDriveLetter(ref str);
				bool flag2 = Directory.Exists(str + "\\");
				if (flag2)
				{
					Dialog.DialogResults dialogResults = Dialog.Show(LocalizationManager.Localize("txtInfo"), LocalizationManager.Localize("txtSaveImages") + "?", DialogStyle.Info, DialogButtons.OKCancel);
					bool flag3 = dialogResults == Dialog.DialogResults.Ok;
					if (flag3)
					{
						this.keyboard = new Keyboard("Directoryname", "", KeyboardType.Default);
						this.keyboard.TextInputKeyboard += this.keyboard_TextInputKeyboard;
						this.keyboard.ShowDialog();
					}
				}
				else
				{
					Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtNoUSB"), DialogStyle.Error);
				}
			}
		}

		// Token: 0x06001B7F RID: 7039 RVA: 0x000A7008 File Offset: 0x000A5208
		private void keyboard_TextInputKeyboard(object sender, KeyboardInputEventArgs e)
		{
			this.keyboard.TextInputKeyboard -= this.keyboard_TextInputKeyboard;
			bool flag = e.Text != "";
			if (flag)
			{
				string text = e.Text;
				bool flag2 = text.Length > 0;
				if (flag2)
				{
					text.Replace(":", "").Replace(".", "").Replace("/", "").Replace("\\", "");
					string text2 = "";
					this.dbConnection.GetUsbDriveLetter(ref text2);
					bool flag3 = base.SelectedImage.XrsImage == null;
					if (flag3)
					{
						base.SelectedImage.LoadXRSImage();
					}
					bool flag4 = base.SelectedImage.XrsImage.ProductNumber == 0U && base.SelectedImage.XrsImage.ProductName.Equals("Reference Product");
					if (flag4)
					{
						bool flag5 = !Directory.Exists(string.Concat(new string[]
						{
							text2,
							"\\RayconImages\\P-1_",
							base.SelectedImage.XrsImage.ProductName,
							"\\",
							text
						}));
						if (flag5)
						{
							this.prgSaveImagesDirectoryname = new Progress(LocalizationManager.Localize("txtSaveImages") + "...", false);
							this.prgSaveImagesDirectoryname.Show();
							this.bgSaveImagesDirectoryname = new BackgroundWorker();
							this.bgSaveImagesDirectoryname.WorkerReportsProgress = true;
							this.bgSaveImagesDirectoryname.DoWork += this.bgSaveSelectedImagesDirectoryname_DoWork;
							this.bgSaveImagesDirectoryname.RunWorkerCompleted += this.bgSaveImagesDirectoryname_RunWorkerCompleted;
							this.bgSaveImagesDirectoryname.ProgressChanged += this.bgSaveImagesDirectoryname_ProgressChanged;
							object obj = text;
							object[] argument = new object[]
							{
								obj
							};
							this.bgSaveImagesDirectoryname.RunWorkerAsync(argument);
						}
						else
						{
							Dialog.Show(LocalizationManager.Localize("txtInfo"), "Name exists already. Saving not possible", DialogStyle.Info, DialogButtons.OK);
						}
					}
					else
					{
						bool flag6 = !Directory.Exists(string.Concat(new string[]
						{
							text2,
							"\\RayconImages\\P",
							base.SelectedImage.XrsImage.ProductNumber.ToString(),
							"_",
							base.SelectedImage.XrsImage.ProductName,
							"\\",
							text
						}));
						if (flag6)
						{
							this.prgSaveImagesDirectoryname = new Progress(LocalizationManager.Localize("txtSaveImages") + "...", false);
							this.prgSaveImagesDirectoryname.Show();
							this.bgSaveImagesDirectoryname = new BackgroundWorker();
							this.bgSaveImagesDirectoryname.WorkerReportsProgress = true;
							this.bgSaveImagesDirectoryname.DoWork += this.bgSaveSelectedImagesDirectoryname_DoWork;
							this.bgSaveImagesDirectoryname.RunWorkerCompleted += this.bgSaveImagesDirectoryname_RunWorkerCompleted;
							this.bgSaveImagesDirectoryname.ProgressChanged += this.bgSaveImagesDirectoryname_ProgressChanged;
							object obj2 = text;
							object[] argument2 = new object[]
							{
								obj2
							};
							this.bgSaveImagesDirectoryname.RunWorkerAsync(argument2);
						}
						else
						{
							Dialog.Show(LocalizationManager.Localize("txtInfo"), "Name exists already. Saving not possible", DialogStyle.Info, DialogButtons.OK);
						}
					}
				}
				else
				{
					Dialog.Show(LocalizationManager.Localize("txtInfo"), "No name entered", DialogStyle.Info, DialogButtons.OK);
				}
			}
			else
			{
				Dialog.Show(LocalizationManager.Localize("txtInfo"), "No name entered", DialogStyle.Info, DialogButtons.OK);
			}
		}

		// Token: 0x06001B80 RID: 7040 RVA: 0x000A738C File Offset: 0x000A558C
		private void bgSaveSelectedImagesDirectoryname_DoWork(object sender, DoWorkEventArgs e)
		{
			object[] array = e.Argument as object[];
			this.saveSelectedImagesDirectoryname(array[0].ToString());
		}

		// Token: 0x06001B81 RID: 7041 RVA: 0x000A73B8 File Offset: 0x000A55B8
		private void saveSelectedImagesDirectoryname(string filename)
		{
			string usbdriveLetter = "";
			int num = 0;
			int num2 = 3;
			this.dbConnection.GetUsbDriveLetter(ref usbdriveLetter);
			PreviewItem selectedImage = this.list[0];
			this.notAllSaved = false;
			for (int i = 0; i < this.list.Count; i++)
			{
				base.SelectedImage = this.list[i];
				bool flag = base.SelectedImage.XrsImage.Data != null;
				if (flag)
				{
					bool flag2 = this.SaveSelectedImageDirectoryname(usbdriveLetter, filename);
					while (!flag2)
					{
						num++;
						bool flag3 = num >= num2;
						if (flag3)
						{
							break;
						}
						Thread.Sleep(2000);
						flag2 = this.SaveSelectedImageDirectoryname(usbdriveLetter, filename);
					}
					num = 0;
					this.bgSaveImagesDirectoryname.ReportProgress((i + 1) * 100 / this.list.Count);
				}
				else
				{
					this.notAllSaved = true;
				}
			}
			base.SelectedImage = selectedImage;
		}

		// Token: 0x06001B82 RID: 7042 RVA: 0x000A74C0 File Offset: 0x000A56C0
		private bool SaveSelectedImageDirectoryname(string usbdriveLetter, string filename)
		{
			bool flag = base.SelectedImage != null && base.SelectedImage.XrsImage != null && base.SelectedImage.XrsImage.Width > 0 && base.SelectedImage.XrsImage.Height > 0 && base.SelectedImage.BitmapThumbnail != null;
			bool result;
			if (flag)
			{
				bool flag2 = base.SelectedImage.XrsImage.ProductName != null;
				string text;
				string text2;
				if (flag2)
				{
					bool flag3 = base.SelectedImage.XrsImage.ProductNumber == 0U && base.SelectedImage.XrsImage.ProductName.Equals("Reference Product");
					if (flag3)
					{
						text = string.Concat(new string[]
						{
							"\\RayconImages\\P-1_",
							base.SelectedImage.XrsImage.ProductName,
							"\\",
							filename,
							"\\",
							base.SelectedImage.Timestamp,
							".xri"
						});
						text2 = string.Concat(new string[]
						{
							"\\RayconImages\\P-1_",
							base.SelectedImage.XrsImage.ProductName,
							"\\",
							filename,
							"\\",
							base.SelectedImage.Timestamp,
							".png"
						});
					}
					else
					{
						text = string.Concat(new string[]
						{
							"\\RayconImages\\P",
							base.SelectedImage.XrsImage.ProductNumber.ToString(),
							"_",
							base.SelectedImage.XrsImage.ProductName,
							"\\",
							filename,
							"\\",
							base.SelectedImage.Timestamp,
							".xri"
						});
						text2 = string.Concat(new string[]
						{
							"\\RayconImages\\P",
							base.SelectedImage.XrsImage.ProductNumber.ToString(),
							"_",
							base.SelectedImage.XrsImage.ProductName,
							"\\",
							filename,
							"\\",
							base.SelectedImage.Timestamp,
							".png"
						});
					}
				}
				else
				{
					text = string.Concat(new string[]
					{
						"\\RayconImages\\",
						filename,
						"\\",
						base.SelectedImage.Timestamp,
						".xri"
					});
					text2 = string.Concat(new string[]
					{
						"\\RayconImages\\",
						filename,
						"\\",
						base.SelectedImage.Timestamp,
						".png"
					});
				}
				text = text.Replace(':', '-');
				text2 = text2.Replace(':', '-');
				this.dbConnection.GetUsbDriveLetter(ref usbdriveLetter);
				text = usbdriveLetter + text;
				text2 = usbdriveLetter + text2;
				bool flag4 = base.SelectedImage.XrsImage.ProductNumber == 0U && base.SelectedImage.XrsImage.ProductName.Equals("Reference Product");
				if (flag4)
				{
					bool flag5 = !Directory.Exists(usbdriveLetter + "\\RayconImages\\P-1_" + base.SelectedImage.XrsImage.ProductName);
					if (flag5)
					{
						Directory.CreateDirectory(usbdriveLetter + "\\RayconImages\\P-1_" + base.SelectedImage.XrsImage.ProductName);
					}
				}
				else
				{
					bool flag6 = !Directory.Exists(string.Concat(new string[]
					{
						usbdriveLetter,
						"\\RayconImages\\P",
						base.SelectedImage.XrsImage.ProductNumber.ToString(),
						"_",
						base.SelectedImage.XrsImage.ProductName
					}));
					if (flag6)
					{
						Directory.CreateDirectory(string.Concat(new string[]
						{
							usbdriveLetter,
							"\\RayconImages\\P",
							base.SelectedImage.XrsImage.ProductNumber.ToString(),
							"_",
							base.SelectedImage.XrsImage.ProductName
						}));
					}
				}
				bool flag7 = !File.Exists(text) && base.SelectedImage != null && base.SelectedImage.XrsImage != null;
				if (flag7)
				{
					base.SelectedImage.XrsImage.SaveToFile(text);
				}
				bool flag8 = !File.Exists(text2) && base.SelectedImage != null && base.SelectedImage.XrsImage != null;
				if (flag8)
				{
					bool saveEdgeFilteredPixel = false;
					this.dbConnection.GetSaveEdgePixelInPNG(ref saveEdgeFilteredPixel);
					FileStream fileStream = new FileStream(text2, FileMode.Create);
					new PngBitmapEncoder
					{
						Frames = 
						{
							BitmapFrame.Create(base.SelectedImage.XrsImage.GetCombinedImage(saveEdgeFilteredPixel))
						}
					}.Save(fileStream);
					fileStream.Close();
				}
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06001B83 RID: 7043 RVA: 0x000A79B0 File Offset: 0x000A5BB0
		private void bgSaveImagesDirectoryname_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			bool flag = this.prgSaveImagesDirectoryname != null;
			if (flag)
			{
				this.prgSaveImagesDirectoryname.Close();
			}
			bool flag2 = !this.notAllSaved;
			if (flag2)
			{
				Dialog.Show(LocalizationManager.Localize("txtSuccess"), LocalizationManager.Localize("txtSaveSuccessful"), DialogStyle.Ok, DialogButtons.None, true);
			}
			else
			{
				Dialog.Show(LocalizationManager.Localize("txtInfo"), LocalizationManager.Localize("txtImagesNotAllSaved"), DialogStyle.Info, DialogButtons.OK);
			}
		}

		// Token: 0x06001B84 RID: 7044 RVA: 0x000A7A24 File Offset: 0x000A5C24
		private void bgSaveImagesDirectoryname_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			bool flag = this.prgSaveImagesDirectoryname != null;
			if (flag)
			{
				this.prgSaveImagesDirectoryname.progBar.Value = (double)e.ProgressPercentage;
			}
		}

		// Token: 0x06001B85 RID: 7045 RVA: 0x000A7A58 File Offset: 0x000A5C58
		private void testcardSimulation(XRSImage image)
		{
			bool flag = Application.Current != null;
			if (flag)
			{
				Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
				{
					bool flag2 = false;
					foreach (PreviewItem selectedImage in this.Preview.PreviewItems)
					{
						this.SelectedImage = selectedImage;
						bool flag3 = this.SelectedImage.XrsImage.ProductNumber == 0U && this.SelectedImage.XrsImage.ProductName.Equals("Reference Product");
						if (flag3)
						{
							bool flag4 = this.CurrentLoadedProduct.ProductNumber.Equals("-1") && this.SelectedImage.XrsImage.Data != null;
							if (flag4)
							{
								flag2 = true;
								break;
							}
						}
						else
						{
							bool flag5 = this.SelectedImage.XrsImage.ProductNumber.ToString().Equals(this.CurrentLoadedProduct.ProductNumber) && this.SelectedImage.XrsImage.Data != null;
							if (flag5)
							{
								flag2 = true;
								break;
							}
						}
					}
					bool flag6 = flag2;
					if (flag6)
					{
						bool flag7 = this.SelectedImage.XrsImage.State >= 128;
						if (flag7)
						{
							Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtTestcardSimNotPossible"), DialogStyle.Warning, DialogButtons.OK);
						}
						else
						{
							DialogButtons buttonOption = (this.parHandling.ReadPar(Definition.simulationAfterTimeIntervall) == "1") ? DialogButtons.OK : DialogButtons.OKCancel;
							bool flag8 = Dialog.Show(LocalizationManager.Localize("txtInfo"), LocalizationManager.Localize("txtCalcSensQuestionTestcardSim") + Environment.NewLine + this.testcardSimService.setExpectedDurationMsg((int)(image.Height * image.Width), true), DialogStyle.Info, buttonOption) == Dialog.DialogResults.Ok;
							if (flag8)
							{
								Progress.ShowStaticProgress(LocalizationManager.Localize("txtCalcSensTestcardSim"), this.isTestcardSimActive);
								this.imgTransfer.FrameArrived += this.imageTestcardSimTransfer_FrameArrived;
								this.sendTestcardSimImagesWithActSens(image);
							}
						}
					}
					else
					{
						Dialog.Show(LocalizationManager.Localize("txtInfo"), LocalizationManager.Localize("txtSensitivityNotPossible"), DialogStyle.Info, DialogButtons.OK);
					}
				}));
			}
		}

		// Token: 0x04001227 RID: 4647
		private const string strRetrain = "Retrain";

		// Token: 0x04001228 RID: 4648
		private bool allImagesAreMarked = false;

		// Token: 0x04001229 RID: 4649
		private ParameterHandling parHandling;

		// Token: 0x0400122A RID: 4650
		private ImageDataTransfer imgTransfer;

		// Token: 0x0400122B RID: 4651
		private DBConnection dbConnection;

		// Token: 0x0400122C RID: 4652
		private ProductManager productManager;

		// Token: 0x0400122D RID: 4653
		private Progress prgSaveAllImages;

		// Token: 0x0400122E RID: 4654
		private Progress prgRetrain;

		// Token: 0x0400122F RID: 4655
		private Progress prgDelete;

		// Token: 0x04001230 RID: 4656
		private BackgroundWorker bgSaveImages;

		// Token: 0x04001231 RID: 4657
		private BackgroundWorker bgDelete;

		// Token: 0x04001232 RID: 4658
		private BackgroundWorker bgRetrain;

		// Token: 0x04001233 RID: 4659
		private Keyboard keyboard;

		// Token: 0x04001234 RID: 4660
		private User user;

		// Token: 0x04001235 RID: 4661
		private TestcardSimService testcardSimService;

		// Token: 0x04001236 RID: 4662
		private const int timestampLength = 20;

		// Token: 0x04001237 RID: 4663
		private bool isTestcardSimActive = false;

		// Token: 0x04001238 RID: 4664
		private List<PreviewItem> list = new List<PreviewItem>();

		// Token: 0x0400123A RID: 4666
		private bool retrainMax = false;

		// Token: 0x0400123B RID: 4667
		private PreviewItem firstSelected;

		// Token: 0x0400123C RID: 4668
		public int numberItem = 0;

		// Token: 0x0400123D RID: 4669
		public bool first = true;

		// Token: 0x0400123E RID: 4670
		public bool done = false;

		// Token: 0x0400123F RID: 4671
		public bool canceled = false;

		// Token: 0x04001240 RID: 4672
		private bool widescreen;

		// Token: 0x04001241 RID: 4673
		private bool fileListVisible = false;

		// Token: 0x04001242 RID: 4674
		private bool allRetrained = false;

		// Token: 0x04001244 RID: 4676
		private bool notAllSaved;

		// Token: 0x04001245 RID: 4677
		public int maximumNumber = 1;

		// Token: 0x04001246 RID: 4678
		private string number = "0";

		// Token: 0x04001247 RID: 4679
		private bool retrain = false;

		// Token: 0x0400124A RID: 4682
		private Progress prgSaveImagesDirectoryname;

		// Token: 0x0400124B RID: 4683
		private BackgroundWorker bgSaveImagesDirectoryname;

		// Token: 0x0400124C RID: 4684
		private Sensitivity bulksensitivity;

		// Token: 0x0400124D RID: 4685
		private int numberToShow = 0;
	}
}
