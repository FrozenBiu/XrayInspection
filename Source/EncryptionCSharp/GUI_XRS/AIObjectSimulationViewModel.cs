using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using Common;
using DatabaseConnection;
using Definitions;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x0200000B RID: 11
	internal class AIObjectSimulationViewModel
	{
		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000065 RID: 101 RVA: 0x00003A0E File Offset: 0x00001C0E
		// (set) Token: 0x06000066 RID: 102 RVA: 0x00003A16 File Offset: 0x00001C16
		public RelayCommand RetrainRegionCommand { get; set; }

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000067 RID: 103 RVA: 0x00003A1F File Offset: 0x00001C1F
		// (set) Token: 0x06000068 RID: 104 RVA: 0x00003A27 File Offset: 0x00001C27
		public RelayCommand TrainToTriggerCommand { get; set; }

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000069 RID: 105 RVA: 0x00003A30 File Offset: 0x00001C30
		// (set) Token: 0x0600006A RID: 106 RVA: 0x00003A38 File Offset: 0x00001C38
		public RelayCommand UndoCommand { get; set; }

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x0600006B RID: 107 RVA: 0x00003A41 File Offset: 0x00001C41
		// (set) Token: 0x0600006C RID: 108 RVA: 0x00003A49 File Offset: 0x00001C49
		public RelayCommand MoveOrSelectCommand { get; set; }

		// Token: 0x0600006D RID: 109 RVA: 0x00003A54 File Offset: 0x00001C54
		public AIObjectSimulationViewModel(StoreImageData imageArchiver, ImageDataTransfer imageTransfer, ParameterHandling parHandling, XRSImageBox imagebox, DBConnection dbConnection, ProductManager productManager)
		{
			this.imageArchiver = imageArchiver;
			this.imageTransfer = imageTransfer;
			this.imageTransfer.FrameArrived += this.imageTransfer_FrameArrived;
			this.parHandling = parHandling;
			this.imagebox = imagebox;
			this.dbConnection = dbConnection;
			this.productManager = productManager;
			this.timer = new Timer(new TimerCallback(this.LoadImages));
			this.timer.Change(100, -1);
			this.RetrainRegionCommand = new RelayCommand(new Action<object>(this.RetrainRegion));
			this.TrainToTriggerCommand = new RelayCommand(new Action<object>(this.TrainToTrigger));
			this.UndoCommand = new RelayCommand(new Action<object>(this.Undo));
			this.MoveOrSelectCommand = new RelayCommand(new Action<object>(this.MoveOrSelect));
			imagebox.RegionSelectStarted += this.imagebox_RegionSelectStarted;
			imagebox.RegionSelectEnded += this.imagebox_RegionSelectEnded;
			this.btnRetrainRegion = new Button();
			this.btnTrainToTrigger = new Button();
			this.btnUndo = new Button();
			this.btnMoveOrSelect = new Button();
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00003B90 File Offset: 0x00001D90
		private void LoadImages(object obj)
		{
			this.currentImage = HistoryHelper.GetLatestImage(this.imageArchiver);
			bool flag = this.currentImage != null;
			if (flag)
			{
				this.parHandling.WriteValue(AIObjectSimulationViewModel.objectDetectionDebug, "1", false);
				this.imageTransfer.SendImage(this.currentImage);
			}
			else
			{
				Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
				{
					Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtNoValidImage"), DialogStyle.Error);
				}));
			}
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00003C1C File Offset: 0x00001E1C
		private void imageTransfer_FrameArrived(object sender, ReceiveEventArgs e)
		{
			bool flag = e.Image.Type == ImageType.Debug;
			if (flag)
			{
				bool flag2 = this.inTrainMode;
				if (flag2)
				{
					this.inTrainMode = false;
					Filter.UpdateObjectDetectionInferenceThresholds(this.dbConnection, this.parHandling);
					Progress.CloseStaticProgress(false);
					this.imagebox.RemoveRetrainRectangle();
				}
				this.parHandling.WriteValue(AIObjectSimulationViewModel.objectDetectionDebug, "0", false);
				bool flag3 = this.ShowImageAction != null;
				if (flag3)
				{
					XRSImage obj = e.Image.DeepCopy();
					this.ShowImageAction(obj);
				}
			}
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00003CB8 File Offset: 0x00001EB8
		public void CleanUp()
		{
			bool flag = this.timer != null;
			if (flag)
			{
				this.timer.Dispose();
			}
			this.imagebox.RegionSelectStarted -= this.imagebox_RegionSelectStarted;
			this.imagebox.RegionSelectEnded -= this.imagebox_RegionSelectEnded;
			this.imageTransfer.FrameArrived -= this.imageTransfer_FrameArrived;
			this.parHandling.WriteValue(AIObjectSimulationViewModel.objectDetectionDebug, "0", false);
			this.currentImage = null;
			this.btnRetrainRegion = null;
			this.RetrainRegionCommand = null;
			this.btnTrainToTrigger = null;
			this.TrainToTriggerCommand = null;
			this.btnUndo = null;
			this.UndoCommand = null;
			this.btnMoveOrSelect = null;
			this.MoveOrSelectCommand = null;
			List<string> list = this.listForActualLearnedParameters;
			if (list != null)
			{
				list.Clear();
			}
			this.listForActualLearnedParameters = null;
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00003D9A File Offset: 0x00001F9A
		public void RetrainRegion(object obj)
		{
			this.SaveActualLearnedParams();
			this.TrainMarkedArea("RetrainRegion");
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00003DB0 File Offset: 0x00001FB0
		public void TrainToTrigger(object obj)
		{
			this.SaveActualLearnedParams();
			this.TrainMarkedArea("TrainToTrigger");
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00003DC8 File Offset: 0x00001FC8
		public void Undo(object obj)
		{
			bool flag = Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtUndoRetrain"), DialogStyle.Warning, DialogButtons.OKCancel) == Dialog.DialogResults.Ok;
			if (flag)
			{
				this.SetPreviousLearnedParams();
				this.SetImageUndoButton(false);
				Progress.ShowStaticProgress("Undo Retraining..", false);
				this.inTrainMode = true;
				this.parHandling.WriteValue(AIObjectSimulationViewModel.objectDetectionDebug, "1", false);
				this.imageTransfer.SendImage(this.currentImage);
			}
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00003E45 File Offset: 0x00002045
		public void MoveOrSelect(object obj)
		{
			this.imagebox.ToggleBoxSelectionModeAndButtonImage(this.btnMoveOrSelect);
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00003E5C File Offset: 0x0000205C
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

		// Token: 0x06000076 RID: 118 RVA: 0x00003F1E File Offset: 0x0000211E
		private void imagebox_RegionSelectStarted(Point startPoint)
		{
			this.startPoint = startPoint;
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00003F28 File Offset: 0x00002128
		private void TrainMarkedArea(string command)
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
						bool flag3 = command == "TrainToTrigger";
						if (flag3)
						{
							LogbookManager.AddEntry(SSTEntryCode.ObjectDetectionParameterChanged, 0, this.productManager.CurrentLoadedProduct.ProductNumber, this.productManager.CurrentLoadedProduct.ProductName);
						}
						else
						{
							bool flag4 = command == "RetrainRegion";
							if (flag4)
							{
								LogbookManager.AddEntry(SSTEntryCode.ObjectDetectionParameterChanged, 9, this.productManager.CurrentLoadedProduct.ProductNumber, this.productManager.CurrentLoadedProduct.ProductName);
							}
						}
						for (int i = 0; i < Definition.nAIObjectClasses; i++)
						{
							bool flag5 = this.parHandling.ReadPar(Definition.objectDetectionClassesSubPath + (i + 1).ToString() + "/DetectionType/" + Definition.strValue) == "1";
							bool flag6 = !flag5;
							if (flag6)
							{
								this.parHandling.WriteValue(string.Concat(new string[]
								{
									Definition.objectDetectionClassesSubPath,
									(i + 1).ToString(),
									"/",
									command,
									"/",
									Definition.strValue
								}), "1", false);
								this.parHandling.WriteValue(Definition.objectDetectionClassesSubPath + (i + 1).ToString() + "/MinX/" + Definition.strValue, this.minPointX.ToString(), false);
								this.parHandling.WriteValue(Definition.objectDetectionClassesSubPath + (i + 1).ToString() + "/MinY/" + Definition.strValue, this.minPointY.ToString(), false);
								this.parHandling.WriteValue(Definition.objectDetectionClassesSubPath + (i + 1).ToString() + "/MaxX/" + Definition.strValue, this.maxPointX.ToString(), false);
								this.parHandling.WriteValue(Definition.objectDetectionClassesSubPath + (i + 1).ToString() + "/MaxY/" + Definition.strValue, this.maxPointY.ToString(), false);
							}
						}
						this.inTrainMode = true;
						this.SetImageUndoButton(true);
						this.parHandling.WriteValue(AIObjectSimulationViewModel.objectDetectionDebug, "1", false);
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

		// Token: 0x06000078 RID: 120 RVA: 0x0000421C File Offset: 0x0000241C
		public void SetImageUndoButton(bool enable)
		{
			Image image = ImageConverter.ToImage("/img/retrainUndo_disabled.png");
			if (enable)
			{
				image = ImageConverter.ToImage("/img/retrainUndo.png");
			}
			image.Height = 40.0;
			image.Width = 40.0;
			image.Stretch = Stretch.Uniform;
			bool flag = this.btnUndo != null;
			if (flag)
			{
				this.btnUndo.Content = image;
				this.btnUndo.IsEnabled = enable;
			}
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00004298 File Offset: 0x00002498
		private void SaveActualLearnedParams()
		{
			this.listForActualLearnedParameters = new List<string>();
			bool flag = this.parHandling.ReadPar(Definition.objectDetectionInferenceEnabled) == "1";
			if (flag)
			{
				for (int i = 1; i <= Definition.nAIObjectClasses; i++)
				{
					this.listForActualLearnedParameters.Add(this.parHandling.ReadPar(Definition.objectDetectionClassesSubPath + i.ToString() + Definition.objectDetectionConfThreshold));
					this.listForActualLearnedParameters.Add(this.parHandling.ReadPar(Definition.objectDetectionClassesSubPath + i.ToString() + Definition.objectDetectionConfThresholdTTA1));
					this.listForActualLearnedParameters.Add(this.parHandling.ReadPar(Definition.objectDetectionClassesSubPath + i.ToString() + Definition.objectDetectionConfThresholdTTA2));
					this.listForActualLearnedParameters.Add(this.parHandling.ReadPar(Definition.objectDetectionClassesSubPath + i.ToString() + Definition.objectDetectionConfThresholdTTA3));
				}
			}
		}

		// Token: 0x0600007A RID: 122 RVA: 0x000043A8 File Offset: 0x000025A8
		private void SetPreviousLearnedParams()
		{
			bool flag = this.parHandling.ReadPar(Definition.objectDetectionInferenceEnabled) == "1";
			if (flag)
			{
				int num = 0;
				for (int i = 0; i < Definition.nAIObjectClasses; i++)
				{
					this.parHandling.WriteValue(Definition.objectDetectionClassesSubPath + (i + 1).ToString() + Definition.objectDetectionConfThreshold, this.listForActualLearnedParameters[num], false);
					this.parHandling.WriteValue(Definition.objectDetectionClassesSubPath + (i + 1).ToString() + Definition.objectDetectionConfThresholdTTA1, this.listForActualLearnedParameters[num + 1], false);
					this.parHandling.WriteValue(Definition.objectDetectionClassesSubPath + (i + 1).ToString() + Definition.objectDetectionConfThresholdTTA2, this.listForActualLearnedParameters[num + 2], false);
					this.parHandling.WriteValue(Definition.objectDetectionClassesSubPath + (i + 1).ToString() + Definition.objectDetectionConfThresholdTTA3, this.listForActualLearnedParameters[num + 3], false);
					num += 4;
				}
			}
		}

		// Token: 0x040000CC RID: 204
		private static readonly string objectDetectionDebug = "/filter/objectDetection/inference/Debug/Value";

		// Token: 0x040000CD RID: 205
		private ParameterHandling parHandling;

		// Token: 0x040000CE RID: 206
		private StoreImageData imageArchiver;

		// Token: 0x040000CF RID: 207
		private ImageDataTransfer imageTransfer;

		// Token: 0x040000D0 RID: 208
		private DBConnection dbConnection;

		// Token: 0x040000D1 RID: 209
		private XRSImage currentImage;

		// Token: 0x040000D2 RID: 210
		private XRSImageBox imagebox;

		// Token: 0x040000D3 RID: 211
		public Button btnRetrainRegion;

		// Token: 0x040000D4 RID: 212
		public Button btnTrainToTrigger;

		// Token: 0x040000D5 RID: 213
		public Button btnUndo;

		// Token: 0x040000D6 RID: 214
		public Button btnMoveOrSelect;

		// Token: 0x040000DB RID: 219
		private Timer timer;

		// Token: 0x040000DC RID: 220
		public Action<XRSImage> ShowImageAction;

		// Token: 0x040000DD RID: 221
		private Point startPoint;

		// Token: 0x040000DE RID: 222
		private double maxPointY;

		// Token: 0x040000DF RID: 223
		private double minPointY;

		// Token: 0x040000E0 RID: 224
		private double maxPointX;

		// Token: 0x040000E1 RID: 225
		private double minPointX;

		// Token: 0x040000E2 RID: 226
		private bool inTrainMode = false;

		// Token: 0x040000E3 RID: 227
		private ProductManager productManager;

		// Token: 0x040000E4 RID: 228
		private List<string> listForActualLearnedParameters;
	}
}
