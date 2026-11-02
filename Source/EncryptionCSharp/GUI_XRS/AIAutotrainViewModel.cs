using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
	// Token: 0x02000008 RID: 8
	public class AIAutotrainViewModel : PreviewViewModel
	{
		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600002A RID: 42 RVA: 0x00002468 File Offset: 0x00000668
		// (set) Token: 0x0600002B RID: 43 RVA: 0x00002470 File Offset: 0x00000670
		public List<PreviewItem> SelectedPrievewItems { get; set; }

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600002C RID: 44 RVA: 0x00002479 File Offset: 0x00000679
		// (set) Token: 0x0600002D RID: 45 RVA: 0x00002481 File Offset: 0x00000681
		public PreviewItem FirstSelected { get; set; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600002E RID: 46 RVA: 0x0000248A File Offset: 0x0000068A
		// (set) Token: 0x0600002F RID: 47 RVA: 0x00002492 File Offset: 0x00000692
		public RelayCommand StartAutotrainCommand { get; set; }

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000030 RID: 48 RVA: 0x0000249C File Offset: 0x0000069C
		// (set) Token: 0x06000031 RID: 49 RVA: 0x000024B4 File Offset: 0x000006B4
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

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000032 RID: 50 RVA: 0x000024E8 File Offset: 0x000006E8
		// (set) Token: 0x06000033 RID: 51 RVA: 0x00002500 File Offset: 0x00000700
		public bool ShowSelectImagesMessage
		{
			get
			{
				return this.showSelectImagesMessage;
			}
			set
			{
				this.showSelectImagesMessage = value;
				this.OnPropertyChanged("ShowSelectImagesMessage");
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000034 RID: 52 RVA: 0x00002516 File Offset: 0x00000716
		// (set) Token: 0x06000035 RID: 53 RVA: 0x0000251E File Offset: 0x0000071E
		public BoundLocalizedText StrSelectImagesMessage { get; set; }

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000036 RID: 54 RVA: 0x00002528 File Offset: 0x00000728
		// (remove) Token: 0x06000037 RID: 55 RVA: 0x00002560 File Offset: 0x00000760
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event EventHandler AutotrainCompleted;

		// Token: 0x06000038 RID: 56 RVA: 0x00002598 File Offset: 0x00000798
		public AIAutotrainViewModel(XRSImageBox imagebox, string[] imageFolders, StoreImageData imageArchiver, DBConnection dbConnection, ProductManager productManager, AIService aiService, ParameterHandling parHandling, ImageDataTransfer imageTransfer) : base(imagebox, imageFolders, 300, Lane.All, 0, imageArchiver, false, true, productManager.CurrentLoadedProduct.ProductName, -1)
		{
			this.imageArchiver = imageArchiver;
			this.dbConnection = dbConnection;
			this.productManager = productManager;
			this.aiService = aiService;
			this.parHandling = parHandling;
			this.imageTransfer = imageTransfer;
			this.StartAutotrainCommand = new RelayCommand(new Action<object>(this.StartAutotrain));
			this.btnStartAutotrain = new Button();
			bool flag = this.SelectedPrievewItems != null;
			if (flag)
			{
				this.SelectedItemCount = Convert.ToString(this.SelectedPrievewItems.Count);
			}
			else
			{
				this.SelectedItemCount = "0";
			}
			base.Preview.IsDatasetViewAI = true;
			base.Preview.PreviewItems.Clear();
			bool flag2 = this.notInitView;
			if (flag2)
			{
				base.Preview.CreatePreview();
			}
			this.notInitView = true;
			this.SelectedPrievewItems = new List<PreviewItem>();
			base.Preview.FirstItemCreated += this.Preview_FirstItemCreated;
			this.RegisterLocalizedText();
		}

		// Token: 0x06000039 RID: 57 RVA: 0x000026DC File Offset: 0x000008DC
		private void RegisterLocalizedText()
		{
			this.StrSelectImagesMessage = new BoundLocalizedText("txtRetrainMin");
			string localizedText = Convert.ToString(10) + " " + this.StrSelectImagesMessage.LocalizedText;
			this.StrSelectImagesMessage.LocalizedText = localizedText;
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00002725 File Offset: 0x00000925
		public void DeregisterLocalizedText()
		{
			MainWindow.localizationManager.Deregister(this.StrSelectImagesMessage);
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00002739 File Offset: 0x00000939
		public void CleanUp()
		{
			this.imageTransfer.FrameArrived -= this.imageTransfer_FrameArrived;
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00002754 File Offset: 0x00000954
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

		// Token: 0x0600003D RID: 61 RVA: 0x000027BC File Offset: 0x000009BC
		public void UpdateTextAndButtons()
		{
			bool flag = this.StrSelectImagesMessage != null;
			if (flag)
			{
				bool flag2 = this.SelectedPrievewItems.Count < 10;
				if (flag2)
				{
					this.StrSelectImagesMessage.ResourceKey = "txtRetrainMin";
					string localizedText = Convert.ToString(10) + " " + this.StrSelectImagesMessage.LocalizedText;
					this.StrSelectImagesMessage.LocalizedText = localizedText;
					this.ShowSelectImagesMessage = true;
					this.btnStartAutotrain.Content = ImageConverter.ToImage("/img/retrain_disabled.png");
					this.btnStartAutotrain.IsEnabled = false;
				}
				else
				{
					bool flag3 = this.SelectedPrievewItems.Count == 10;
					if (flag3)
					{
						this.ShowSelectImagesMessage = false;
						this.btnStartAutotrain.Content = ImageConverter.ToImage("/img/retrain.png");
						this.btnStartAutotrain.IsEnabled = true;
					}
					else
					{
						this.StrSelectImagesMessage.ResourceKey = "txtRetrainMax";
						this.ShowSelectImagesMessage = true;
						this.btnStartAutotrain.Content = ImageConverter.ToImage("/img/retrain_disabled.png");
						this.btnStartAutotrain.IsEnabled = false;
					}
				}
			}
		}

		// Token: 0x0600003E RID: 62 RVA: 0x000028DC File Offset: 0x00000ADC
		private void StartAutotrain(object obj)
		{
			bool flag = base.SelectedImage != null;
			if (flag)
			{
				this.numberItem = 0;
				base.SelectedImage = this.SelectedPrievewItems.First<PreviewItem>();
				this.retrain = true;
				this.allRetrained = true;
				this.prgAutotrain = new Progress(LocalizationManager.Localize("txtRetrain") + "...", false);
				this.prgAutotrain.Show();
				this.bgAutotrain = new BackgroundWorker();
				this.bgAutotrain.WorkerReportsProgress = true;
				this.bgAutotrain.DoWork += this.bgAutotrain_DoWork;
				this.bgAutotrain.WorkerSupportsCancellation = true;
				this.bgAutotrain.RunWorkerCompleted += this.bgAutotrain_RunWorkerCompleted;
				this.bgAutotrain.ProgressChanged += this.bgAutotrain_ProgressChanged;
				this.bgAutotrain.RunWorkerAsync();
			}
		}

		// Token: 0x0600003F RID: 63 RVA: 0x000029C8 File Offset: 0x00000BC8
		private void bgAutotrain_DoWork(object sender, DoWorkEventArgs e)
		{
			foreach (PreviewItem previewItem in this.SelectedPrievewItems)
			{
				base.SelectedImage = previewItem;
				bool flag = previewItem.XrsImage.Data != null;
				if (flag)
				{
					this.DoAutotrain();
					this.WaitForImage();
					bool cancellationPending = this.bgAutotrain.CancellationPending;
					if (cancellationPending)
					{
						Filter.WriteToImageProcessingFiltersAI(this.parHandling, "Autolearn", "0", this.dbConnection);
						break;
					}
				}
				else
				{
					this.allRetrained = false;
				}
				this.numberItem++;
			}
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00002A90 File Offset: 0x00000C90
		private void bgAutotrain_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			bool flag = this.prgAutotrain != null;
			if (flag)
			{
				this.prgAutotrain.progBar.Value = (double)e.ProgressPercentage;
			}
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00002AC3 File Offset: 0x00000CC3
		private void bgAutotrain_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			this.prgAutotrain.Close();
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00002AD4 File Offset: 0x00000CD4
		private void WaitForImage()
		{
			int millisecondsDelay = 90000;
			CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(millisecondsDelay);
			while (!this.done && !cancellationTokenSource.IsCancellationRequested)
			{
			}
			bool isCancellationRequested = cancellationTokenSource.IsCancellationRequested;
			if (isCancellationRequested)
			{
				this.canceled = true;
				this.bgAutotrain.CancelAsync();
			}
			this.done = false;
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00002B34 File Offset: 0x00000D34
		private void DoAutotrain()
		{
			bool flag = base.SelectedImage != null;
			if (flag)
			{
				bool flag2 = base.SelectedImage.XrsImage != null && base.SelectedImage.XrsImage.Data != null;
				if (flag2)
				{
					this.bgAutotrain.ReportProgress(this.numberItem * 100 / this.SelectedPrievewItems.Count<PreviewItem>());
					string text = Convert.ToString(this.SelectedPrievewItems.Count<PreviewItem>() - this.numberItem);
					bool flag3 = text == Convert.ToString(10);
					if (flag3)
					{
						Filter.WriteToImageProcessingFiltersAI(this.parHandling, "Autolearn", text, this.dbConnection);
					}
					this.imageTransfer.FrameArrived += this.imageTransfer_FrameArrived;
					base.SelectedImage.XrsImage.Type = ImageType.Demo;
					this.imageTransfer.SendImage(base.SelectedImage.XrsImage);
				}
			}
			else
			{
				Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
				{
					Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtImagesDontExist"), DialogStyle.Warning, DialogButtons.None, true);
				}));
			}
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00002C58 File Offset: 0x00000E58
		private void imageTransfer_FrameArrived(object sender, ReceiveEventArgs e)
		{
			bool flag = e.Image.Type == ImageType.Demo || e.Image.Type == ImageType.TrainAI;
			if (flag)
			{
				this.imageTransfer.FrameArrived -= this.imageTransfer_FrameArrived;
				this.done = true;
				Filter.UpdateImageProcessingFilterPaths(this.dbConnection, this.parHandling, (int)e.Image.Group, this.productManager, false);
				Filter.UpdateAnomalyDetectionInferenceThresholds(this.dbConnection, this.parHandling);
				bool flag2 = !this.retrain;
				if (flag2)
				{
					Progress.CloseStaticProgress(false);
				}
				LogbookManager.AddEntry(SSTEntryCode.Retrain, 0, this.productManager.CurrentLoadedProduct.ProductNumber, this.productManager.CurrentLoadedProduct.ProductName);
				bool flag3 = this.numberItem < this.SelectedPrievewItems.Count<PreviewItem>() && this.retrain;
				if (!flag3)
				{
					this.retrain = false;
					bool flag4 = !this.allRetrained;
					if (flag4)
					{
						bool flag5 = Application.Current != null;
						if (flag5)
						{
							Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
							{
								Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtNotAllRetrained"), DialogStyle.Warning, DialogButtons.OK, false);
							}));
						}
					}
					else
					{
						bool flag6 = this.canceled;
						if (flag6)
						{
							this.canceled = false;
							bool flag7 = Application.Current != null;
							if (flag7)
							{
								Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
								{
									Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtTooManyImages"), DialogStyle.Warning, DialogButtons.None, true);
								}));
							}
						}
						else
						{
							bool flag8 = Application.Current != null;
							if (flag8)
							{
								Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
								{
									Dialog.Show(LocalizationManager.Localize("txtInfo"), LocalizationManager.Localize("txtSuccess"), DialogStyle.Info, DialogButtons.None, true);
								}));
							}
						}
					}
					this.allRetrained = true;
					EventHandler autotrainCompleted = this.AutotrainCompleted;
					if (autotrainCompleted != null)
					{
						autotrainCompleted(this, EventArgs.Empty);
					}
				}
			}
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00002E68 File Offset: 0x00001068
		public void CancelAutolearn()
		{
			this.parHandling.WriteValue(Definition.anomalyDetectionInferenceEnabled, Convert.ToString("0"));
			LogbookManager.AddEntry(SSTEntryCode.AIDetectionDeactivated, 0, this.productManager.CurrentLoadedProduct.ProductNumber, this.productManager.CurrentLoadedProduct.ProductName);
			this.aiService.Comp19to30OnOff(false);
		}

		// Token: 0x0400009B RID: 155
		private const string strAutotrain = "Autolearn";

		// Token: 0x0400009C RID: 156
		public const int imagesAutotrain = 10;

		// Token: 0x0400009D RID: 157
		private StoreImageData imageArchiver;

		// Token: 0x0400009E RID: 158
		private DBConnection dbConnection;

		// Token: 0x0400009F RID: 159
		private ProductManager productManager;

		// Token: 0x040000A0 RID: 160
		private AIService aiService;

		// Token: 0x040000A1 RID: 161
		private ParameterHandling parHandling;

		// Token: 0x040000A2 RID: 162
		private ImageDataTransfer imageTransfer;

		// Token: 0x040000A3 RID: 163
		private string selectedItemCount;

		// Token: 0x040000A6 RID: 166
		public Button btnStartAutotrain;

		// Token: 0x040000A7 RID: 167
		public int numberItem = 0;

		// Token: 0x040000A8 RID: 168
		public bool done = false;

		// Token: 0x040000A9 RID: 169
		public bool canceled = false;

		// Token: 0x040000AA RID: 170
		private bool allRetrained = false;

		// Token: 0x040000AB RID: 171
		private bool retrain = false;

		// Token: 0x040000AC RID: 172
		private bool notInitView;

		// Token: 0x040000AE RID: 174
		private bool showSelectImagesMessage = true;

		// Token: 0x040000B0 RID: 176
		private Progress prgAutotrain;

		// Token: 0x040000B1 RID: 177
		private BackgroundWorker bgAutotrain;
	}
}
