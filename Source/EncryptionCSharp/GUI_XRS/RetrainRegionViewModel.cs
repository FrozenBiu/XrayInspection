using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using Common;
using DatabaseConnection;
using Definitions;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x02000030 RID: 48
	public class RetrainRegionViewModel : INotifyPropertyChanged
	{
		// Token: 0x14000022 RID: 34
		// (add) Token: 0x060005F2 RID: 1522 RVA: 0x000284D4 File Offset: 0x000266D4
		// (remove) Token: 0x060005F3 RID: 1523 RVA: 0x0002850C File Offset: 0x0002670C
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x060005F4 RID: 1524 RVA: 0x00028541 File Offset: 0x00026741
		// (set) Token: 0x060005F5 RID: 1525 RVA: 0x00028549 File Offset: 0x00026749
		public RelayCommand ReleaseResourcesCommand { get; set; }

		// Token: 0x060005F6 RID: 1526 RVA: 0x00028554 File Offset: 0x00026754
		public RetrainRegionViewModel(DBConnection dbConnection, ParameterHandling parHandling, ImageDataTransfer imageTransfer, XRSImageBox imageBox, XRSImage image, ProductManager productManager, TestcardSimService testcardSimService)
		{
			this.dbConnection = dbConnection;
			this.imageTransfer = imageTransfer;
			this.imageBox = imageBox;
			this.parHandling = parHandling;
			this.xrsImage = image;
			this.productManager = productManager;
			this.testcardSimService = testcardSimService;
			this.ReleaseResourcesCommand = new RelayCommand(new Action<object>(this.ReleaseResources));
			this.imageStateOrig = this.xrsImage.State;
			imageBox.ViewXRSImage(this.xrsImage);
			imageBox.RegionSelectStarted += this.imageBox_RegionSelectStarted;
			imageBox.RegionSelectEnded += this.imageBox_RegionSelectEnded;
		}

		// Token: 0x060005F7 RID: 1527 RVA: 0x0002860B File Offset: 0x0002680B
		private void ReleaseResources(object obj)
		{
			this.imageBox.RegionSelectStarted -= this.imageBox_RegionSelectStarted;
			this.imageBox.RegionSelectEnded -= this.imageBox_RegionSelectEnded;
		}

		// Token: 0x060005F8 RID: 1528 RVA: 0x00028640 File Offset: 0x00026840
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x060005F9 RID: 1529 RVA: 0x00028670 File Offset: 0x00026870
		private void imageBox_RegionSelectEnded(Point endPoint)
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
			bool flag3 = this.xrsImage.IsBulkOverlapImage && !XRSImageBoxViewModel.BulkOverlapImageShow;
			if (flag3)
			{
				this.minPointY += (double)this.xrsImage.BulkOverlapImgEdgeLines;
				this.maxPointY += (double)this.xrsImage.BulkOverlapImgEdgeLines;
			}
		}

		// Token: 0x060005FA RID: 1530 RVA: 0x00028782 File Offset: 0x00026982
		private void imageBox_RegionSelectStarted(Point startPoint)
		{
			this.startPoint = startPoint;
		}

		// Token: 0x060005FB RID: 1531 RVA: 0x0002878C File Offset: 0x0002698C
		public bool RetrainRegion(bool trainToTrigger)
		{
			bool result = false;
			bool flag = this.imageBox.RectanglePainted();
			if (flag)
			{
				bool flag2 = Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtOverwriteExisting"), DialogStyle.Warning, DialogButtons.OKCancel) == Dialog.DialogResults.Ok;
				if (flag2)
				{
					this.isTestcardSimActive = (this.parHandling.ReadPar(Definition.testcardSimulationPath) == "1" && this.parHandling.ReadPar(Definition.simulationAfterAutolearn) == "1");
					this.SetContinuousModeBulk(false, false);
					Progress.ShowStaticProgress("Retraining...", this.isTestcardSimActive);
					new Thread(delegate()
					{
						bool flag3 = !trainToTrigger;
						if (flag3)
						{
							bool flag4 = !this.productManager.CurrentLoadedProduct.IsRegionProduct;
							if (flag4)
							{
								Filter.WriteToImageProcessingFilters(this.parHandling, "RetrainRegion", "1", (int)this.xrsImage.Group);
								Filter.SaveActualLearnedParams(this.parHandling, (int)this.xrsImage.Group);
							}
							else
							{
								for (int i = 0; i < this.productManager.CurrentLoadedProduct.SubProducts.Count; i++)
								{
									Filter.WriteToImageProcessingFilters(this.parHandling, "RetrainRegion", "1", (int)this.productManager.CurrentLoadedProduct.SubProducts[i].Group);
								}
							}
						}
						else
						{
							bool flag5 = !this.productManager.CurrentLoadedProduct.IsRegionProduct;
							if (flag5)
							{
								Filter.WriteToImageProcessingFilters(this.parHandling, "TrainToTrigger", "1", (int)this.xrsImage.Group);
								Filter.SaveActualLearnedParams(this.parHandling, (int)this.xrsImage.Group);
							}
							else
							{
								for (int j = 0; j < this.productManager.CurrentLoadedProduct.SubProducts.Count; j++)
								{
									Filter.WriteToImageProcessingFilters(this.parHandling, "TrainToTrigger", "1", (int)this.productManager.CurrentLoadedProduct.SubProducts[j].Group);
								}
							}
						}
						this.WriteRegionCoordinates();
						this.inRetrainRegionMode = true;
						this.imageTransfer.FrameArrived += this.imageTransfer_FrameArrived;
						bool flag6 = this.xrsImage.IsBulkOverlapImage && !XRSImageBoxViewModel.BulkOverlapImageShow;
						if (flag6)
						{
							StoreImageData storeImageData = new StoreImageData();
							XRSImage image = new XRSImage();
							bool flag7 = this.xrsImage.FilePath != null;
							if (flag7)
							{
								image = storeImageData.GetImage(this.xrsImage.FilePath);
							}
							else
							{
								image = this.bulkImageWithOverlapping;
							}
							this.imageTransfer.SendImage(image);
						}
						else
						{
							this.imageTransfer.SendImage(this.xrsImage);
						}
					})
					{
						Name = "TrainRegion"
					}.Start();
					result = true;
				}
			}
			else
			{
				Dialog.Show(LocalizationManager.Localize("txtError"), "No region selected.", DialogStyle.Error, DialogButtons.OK);
			}
			return result;
		}

		// Token: 0x060005FC RID: 1532 RVA: 0x00028894 File Offset: 0x00026A94
		public void UndoRetrainRegion()
		{
			bool flag = this.xrsImage != null;
			if (flag)
			{
				bool flag2 = Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtUndoRetrain"), DialogStyle.Warning, DialogButtons.OKCancel) == Dialog.DialogResults.Ok;
				if (flag2)
				{
					this.isTestcardSimActive = (this.parHandling.ReadPar(Definition.testcardSimulationPath) == "1" && this.parHandling.ReadPar(Definition.simulationAfterAutolearn) == "1");
					this.SetContinuousModeBulk(false, false);
					Progress.ShowStaticProgress("Undo Retraining..", this.isTestcardSimActive);
					bool flag3 = !this.productManager.CurrentLoadedProduct.IsRegionProduct;
					if (flag3)
					{
						Filter.SetPreviousLearnedParams(this.parHandling);
					}
					this.inUndoRetrainRegionMode = true;
					this.imageTransfer.FrameArrived += this.imageTransfer_FrameArrived;
					bool flag4 = this.xrsImage.IsBulkOverlapImage && !XRSImageBoxViewModel.BulkOverlapImageShow;
					if (flag4)
					{
						StoreImageData storeImageData = new StoreImageData();
						XRSImage image = new XRSImage();
						bool flag5 = this.xrsImage.FilePath != null;
						if (flag5)
						{
							image = storeImageData.GetImage(this.xrsImage.FilePath);
						}
						else
						{
							image = this.bulkImageWithOverlapping;
						}
						this.imageTransfer.SendImage(image);
					}
					else
					{
						this.imageTransfer.SendImage(this.xrsImage);
					}
				}
			}
		}

		// Token: 0x060005FD RID: 1533 RVA: 0x000289FC File Offset: 0x00026BFC
		private void WriteRegionCoordinates()
		{
			bool flag = !this.productManager.CurrentLoadedProduct.IsRegionProduct;
			if (flag)
			{
				Filter.WriteToImageProcessingFilters(this.parHandling, "MinX", this.minPointX.ToString(), (int)this.xrsImage.Group);
				Filter.WriteToImageProcessingFilters(this.parHandling, "MinY", this.minPointY.ToString(), (int)this.xrsImage.Group);
				Filter.WriteToImageProcessingFilters(this.parHandling, "MaxX", this.maxPointX.ToString(), (int)this.xrsImage.Group);
				Filter.WriteToImageProcessingFilters(this.parHandling, "MaxY", this.maxPointY.ToString(), (int)this.xrsImage.Group);
			}
			else
			{
				for (int i = 0; i < this.productManager.CurrentLoadedProduct.SubProducts.Count; i++)
				{
					Filter.WriteToImageProcessingFilters(this.parHandling, "MinX", this.minPointX.ToString(), (int)this.productManager.CurrentLoadedProduct.SubProducts[i].Group);
					Filter.WriteToImageProcessingFilters(this.parHandling, "MinY", this.minPointY.ToString(), (int)this.productManager.CurrentLoadedProduct.SubProducts[i].Group);
					Filter.WriteToImageProcessingFilters(this.parHandling, "MaxX", this.maxPointX.ToString(), (int)this.productManager.CurrentLoadedProduct.SubProducts[i].Group);
					Filter.WriteToImageProcessingFilters(this.parHandling, "MaxY", this.maxPointY.ToString(), (int)this.productManager.CurrentLoadedProduct.SubProducts[i].Group);
				}
			}
		}

		// Token: 0x060005FE RID: 1534 RVA: 0x00028BD4 File Offset: 0x00026DD4
		private void imageTransfer_FrameArrived(object sender, ReceiveEventArgs e)
		{
			bool flag = e.Image.Type == ImageType.Demo || e.Image.Type == ImageType.TrainAI;
			if (flag)
			{
				this.imageTransfer.FrameArrived -= this.imageTransfer_FrameArrived;
				this.bulkImageWithOverlapping = e.Image.DeepCopy(true);
				this.SetContinuousModeBulk(true, true);
				bool flag2 = this.inRetrainRegionMode || this.inUndoRetrainRegionMode;
				if (flag2)
				{
					bool flag3 = !this.productManager.CurrentLoadedProduct.IsRegionProduct;
					if (flag3)
					{
						Filter.UpdateImageProcessingFilterPaths(this.dbConnection, this.parHandling, (int)e.Image.Group, this.productManager, false);
						bool flag4 = this.parHandling.ReadPar(Definition.anomalyDetectionEnabled) == "1" && this.parHandling.ReadPar(Definition.anomalyDetectionInferenceEnabled) == "1";
						if (flag4)
						{
							Filter.UpdateAnomalyDetectionInferenceThresholds(this.dbConnection, this.parHandling);
						}
						bool flag5 = this.parHandling.ReadPar(Definition.objectDetectionEnabled) == "1" && this.parHandling.ReadPar(Definition.objectDetectionInferenceEnabled) == "1";
						if (flag5)
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
					this.inRetrainRegionMode = false;
					this.inUndoRetrainRegionMode = false;
				}
				this.imageBox.RemoveRetrainRectangle();
				this.xrsImage = e.Image;
				this.imageBox.ViewXRSImage(this.xrsImage);
				LogbookManager.AddEntry(SSTEntryCode.Retrain, 1, this.productManager.CurrentLoadedProduct.ProductNumber, this.productManager.CurrentLoadedProduct.ProductName);
				Progress.CloseStaticProgress(false);
				bool flag6 = this.isTestcardSimActive && !this.productManager.CurrentLoadedProduct.IsRegionProduct && e.Image.Type != ImageType.TestcardSimulation && this.parHandling.ReadPar(Definition.autoKVAdjustmentPath) == "1";
				if (flag6)
				{
					bool flag7 = Application.Current != null;
					if (flag7)
					{
						Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
						{
							bool flag8 = this.imageStateOrig >= 128;
							if (flag8)
							{
								Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtTestcardSimNotPossible"), DialogStyle.Warning, DialogButtons.OK);
							}
							else
							{
								DialogButtons buttonOption = (this.parHandling.ReadPar(Definition.simulationAfterTimeIntervall) == "1") ? DialogButtons.OK : DialogButtons.OKCancel;
								bool flag9 = Dialog.Show(LocalizationManager.Localize("txtInfo"), LocalizationManager.Localize("txtCalcSensQuestionTestcardSim") + Environment.NewLine + this.testcardSimService.setExpectedDurationMsg((int)(e.Image.Width * e.Image.Height), true), DialogStyle.Info, buttonOption) == Dialog.DialogResults.Ok;
								if (flag9)
								{
									Progress.ShowStaticProgress(LocalizationManager.Localize("txtCalcSensTestcardSim"), this.isTestcardSimActive);
									this.imageTransfer.FrameArrived += this.imageTestcardSimTransfer_FrameArrived;
									this.sendTestcardSimImagesWithActSens(this.xrsImage);
								}
							}
						}));
					}
				}
			}
		}

		// Token: 0x060005FF RID: 1535 RVA: 0x00028EDC File Offset: 0x000270DC
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
						this.imageTransfer.FrameArrived -= this.imageTestcardSimTransfer_FrameArrived;
						Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtTestcardSimNotPossible"), DialogStyle.Warning, DialogButtons.OK);
					}
				}));
			}
		}

		// Token: 0x06000600 RID: 1536 RVA: 0x00028F28 File Offset: 0x00027128
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
								this.imageTransfer.FrameArrived -= this.imageTestcardSimTransfer_FrameArrived;
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

		// Token: 0x06000601 RID: 1537 RVA: 0x00028F98 File Offset: 0x00027198
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

		// Token: 0x0400044E RID: 1102
		private const string strRetrainRegion = "RetrainRegion";

		// Token: 0x0400044F RID: 1103
		private const string strMinX = "MinX";

		// Token: 0x04000450 RID: 1104
		private const string strMaxX = "MaxX";

		// Token: 0x04000451 RID: 1105
		private const string strMinY = "MinY";

		// Token: 0x04000452 RID: 1106
		private const string strMaxY = "MaxY";

		// Token: 0x04000453 RID: 1107
		private const string strTrainToTrigger = "TrainToTrigger";

		// Token: 0x04000454 RID: 1108
		private ImageDataTransfer imageTransfer;

		// Token: 0x04000455 RID: 1109
		private ParameterHandling parHandling;

		// Token: 0x04000456 RID: 1110
		public XRSImage xrsImage;

		// Token: 0x04000457 RID: 1111
		public XRSImage bulkImageWithOverlapping;

		// Token: 0x04000458 RID: 1112
		public XRSImageBox imageBox;

		// Token: 0x04000459 RID: 1113
		private Point startPoint;

		// Token: 0x0400045A RID: 1114
		private double maxPointY;

		// Token: 0x0400045B RID: 1115
		private double minPointY;

		// Token: 0x0400045C RID: 1116
		private double maxPointX;

		// Token: 0x0400045D RID: 1117
		private double minPointX;

		// Token: 0x0400045E RID: 1118
		private bool inRetrainRegionMode;

		// Token: 0x0400045F RID: 1119
		private bool inUndoRetrainRegionMode;

		// Token: 0x04000460 RID: 1120
		private DBConnection dbConnection;

		// Token: 0x04000461 RID: 1121
		private ProductManager productManager;

		// Token: 0x04000462 RID: 1122
		private TestcardSimService testcardSimService;

		// Token: 0x04000465 RID: 1125
		private bool isTestcardSimActive = false;

		// Token: 0x04000466 RID: 1126
		private byte imageStateOrig = 0;
	}
}
