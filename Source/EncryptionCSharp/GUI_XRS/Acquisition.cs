using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using DatabaseConnection;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x0200021D RID: 541
	public class Acquisition : INotifyPropertyChanged
	{
		// Token: 0x140000D5 RID: 213
		// (add) Token: 0x060028D0 RID: 10448 RVA: 0x0010F7C8 File Offset: 0x0010D9C8
		// (remove) Token: 0x060028D1 RID: 10449 RVA: 0x0010F800 File Offset: 0x0010DA00
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x17000B27 RID: 2855
		// (get) Token: 0x060028D2 RID: 10450 RVA: 0x0010F838 File Offset: 0x0010DA38
		// (set) Token: 0x060028D3 RID: 10451 RVA: 0x0010F850 File Offset: 0x0010DA50
		public bool IgnoreClip
		{
			get
			{
				return this.ignoreClip;
			}
			set
			{
				bool flag = this.initialized;
				if (flag)
				{
					bool flag2 = !this.showDialogWindows || !value || Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtNewAutotrainRequired"), DialogStyle.Warning, DialogButtons.OKCancel) == Dialog.DialogResults.Ok;
					if (flag2)
					{
						bool flag3 = this.ignoreClip != value;
						if (flag3)
						{
							if (value)
							{
								LogbookManager.AddEntry(SSTEntryCode.FilterActivated, 3);
								this.parHandling.WriteValue(Definition.filterMainPath + this.strGroup + Definition.clipFindingEnableSubPath, "1");
								this.parHandling.WriteValue(Acquisition.nConnectedLinesPath, 100.ToString());
								DetectorType detectorType = DetectorType.Sesotec;
								this.dbConnection.GetDetectorType(ref detectorType);
								bool flag4 = detectorType == DetectorType.Sesotec && this.parHandling.ReadPar(Definition.detectorSesotecPixelSizePath) == "0.4" && this.parHandling.ReadPar(Acquisition.binningPath) == "1";
								string value2;
								string value3;
								string value4;
								if (flag4)
								{
									value2 = "8000";
									value3 = "1000";
									value4 = "22";
								}
								else
								{
									value2 = "2000";
									value3 = "500";
									value4 = "11";
								}
								this.parHandling.WriteValue(Definition.filterMainPath + this.strGroup + Definition.minClipSizeSubPath, value3);
								this.parHandling.WriteValue(Definition.filterMainPath + this.strGroup + Definition.maxClipSizeSubPath, value2);
								this.parHandling.WriteValue(Definition.filterMainPath + this.strGroup + Definition.clipFilterSizeSubPath, value4);
							}
							else
							{
								LogbookManager.AddEntry(SSTEntryCode.FilterDeactivated, 3);
								this.parHandling.WriteValue(Definition.filterMainPath + this.strGroup + Definition.clipFindingEnableSubPath, "0");
								this.parHandling.WriteValue(Acquisition.nConnectedLinesPath, 40.ToString());
							}
							this.ignoreClip = value;
							this.OnPropertyChanged("IgnoreClip");
						}
					}
				}
			}
		}

		// Token: 0x060028D4 RID: 10452 RVA: 0x0010FA7C File Offset: 0x0010DC7C
		public Acquisition(ParameterHandling parHandling, int group, ProductManager productManager, bool showDialogWindows, DBConnection dbConnection)
		{
			this.showDialogWindows = showDialogWindows;
			this.dbConnection = dbConnection;
			bool flag = group <= 0;
			if (flag)
			{
				this.additionalLogbookText = ": P" + productManager.CurrentLoadedProduct.ProductNumber + " " + productManager.CurrentLoadedProduct.ProductName;
			}
			else
			{
				ProductData[] array = (from product in productManager.CurrentLoadedProduct.SubProducts
				where product.TableName.EndsWith(@group.ToString())
				select product).ToArray<ProductData>();
				bool flag2 = array.Length != 0;
				if (flag2)
				{
					this.additionalLogbookText = string.Concat(new string[]
					{
						": P",
						productManager.CurrentLoadedProduct.ProductNumber,
						"_",
						group.ToString(),
						" ",
						array[0].ProductName
					});
				}
			}
			this.strGroup = ((group <= 0) ? "" : group.ToString());
			this.parHandling = parHandling;
			this.currentLengthUnit = ((LocalizationManager.CurrentCulture.Name == "en-US") ? "inch" : "cm");
			this.maxPossibleProductLength = ((this.currentLengthUnit == "inch") ? (this.maxProductLengthInCm08Detector * 0.393700787) : this.maxProductLengthInCm08Detector);
			this.CalculateMaxProductLength();
			this.ignoreClip = (parHandling.ReadPar(Definition.filterMainPath + this.strGroup + Definition.clipFindingEnableSubPath) == "1");
			this.initialized = true;
		}

		// Token: 0x060028D5 RID: 10453 RVA: 0x0010FC3C File Offset: 0x0010DE3C
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x060028D6 RID: 10454 RVA: 0x0010FC6C File Offset: 0x0010DE6C
		private void CalculateMaxProductLength()
		{
			int num = 0;
			bool flag = int.TryParse(this.parHandling.ReadPar(Acquisition.nMaxLinesPath), out num);
			if (flag)
			{
				this.maxProductLength = (double)num * 0.056;
				bool flag2 = this.parHandling.ReadPar(Conveyor.detectorDTPixelSizePath) == Conveyor.detectorDT04Identifier;
				if (flag2)
				{
					this.maxProductLength /= 2.0;
				}
				bool flag3 = this.currentLengthUnit == "inch";
				if (flag3)
				{
					this.maxProductLength *= 0.393700787;
				}
				this.maxProductLength = Math.Round(this.maxProductLength, 1);
			}
		}

		// Token: 0x060028D7 RID: 10455 RVA: 0x0010FD20 File Offset: 0x0010DF20
		private void SetNMaxLines()
		{
			bool flag = this.currentLengthUnit == "inch";
			int num;
			if (flag)
			{
				num = (int)(this.maxProductLength / 0.022047244072);
			}
			else
			{
				num = (int)(this.maxProductLength / 0.056);
			}
			bool flag2 = this.parHandling.ReadPar(Conveyor.detectorDTPixelSizePath) == Conveyor.detectorDT04Identifier;
			if (flag2)
			{
				num *= 2;
			}
			this.parHandling.WriteValue(Acquisition.nMaxLinesPath, num.ToString());
		}

		// Token: 0x04001949 RID: 6473
		private const double cmToInchFactor = 0.393700787;

		// Token: 0x0400194A RID: 6474
		private const int nConnectedLinesWithClip = 100;

		// Token: 0x0400194B RID: 6475
		private const int nConnectedLinesWithoutClip = 40;

		// Token: 0x0400194C RID: 6476
		private const string lengthUnitMetric = "cm";

		// Token: 0x0400194D RID: 6477
		private const string lengthUnitUS = "inch";

		// Token: 0x0400194E RID: 6478
		public static readonly string nMaxLinesPath = "/filter/acquisition/MaxLines/Value";

		// Token: 0x0400194F RID: 6479
		public static readonly string sendOnlyIfContaminantPath = "/filter/acquisition/SendOnlyIfContaminant/Value";

		// Token: 0x04001950 RID: 6480
		public static readonly string sendImagePath = "/filter/acquisition/SendImage/Value";

		// Token: 0x04001951 RID: 6481
		public static readonly string continuousModeFilteringPath = "/filter/acquisition/ContinuousModeFiltering/Value";

		// Token: 0x04001952 RID: 6482
		public static readonly string imageDelayPath = "/filter/acquisition/ImageDelay/Value";

		// Token: 0x04001953 RID: 6483
		public static readonly string objimageDelayPath = "/filter/acquisition/MissedBlobObjectImg/Value";

		// Token: 0x04001954 RID: 6484
		private const string connectImagesPath = "/filter/acquisition/ConnectImages/Value";

		// Token: 0x04001955 RID: 6485
		public static readonly string nConnectedLinesPath = "/filter/acquisition/NConnectedLines/Value";

		// Token: 0x04001956 RID: 6486
		public static readonly string binningPath = "/filter/acquisition/Binning/Value";

		// Token: 0x04001957 RID: 6487
		public static readonly string binPixelPath = "/filter/acquisition/BinPixel/Value";

		// Token: 0x04001958 RID: 6488
		public static readonly string lane1EnablePath = "/filter/acquisition/lane1/Enable/Value";

		// Token: 0x04001959 RID: 6489
		public static readonly string lane2EnablePath = "/filter/acquisition/lane2/Enable/Value";

		// Token: 0x0400195A RID: 6490
		public static readonly string lane3EnablePath = "/filter/acquisition/lane3/Enable/Value";

		// Token: 0x0400195B RID: 6491
		public static readonly string lane4EnablePath = "/filter/acquisition/lane4/Enable/Value";

		// Token: 0x0400195C RID: 6492
		public static readonly string lane1MinPixelPath = "/filter/acquisition/lane1/MinPixel/Value";

		// Token: 0x0400195D RID: 6493
		public static readonly string lane1MaxPixelPath = "/filter/acquisition/lane1/MaxPixel/Value";

		// Token: 0x0400195E RID: 6494
		public static readonly string resetCalibrationPath = "/filter/acquisition/ResetCalibration/Value";

		// Token: 0x0400195F RID: 6495
		public static readonly string acquisitionModePath = "/filter/acquisition/AcquisitionMode/Value";

		// Token: 0x04001960 RID: 6496
		private double maxProductLengthInCm08Detector = 112.1;

		// Token: 0x04001961 RID: 6497
		private DBConnection dbConnection;

		// Token: 0x04001962 RID: 6498
		private bool initialized = false;

		// Token: 0x04001963 RID: 6499
		private double maxPossibleProductLength;

		// Token: 0x04001964 RID: 6500
		private string currentLengthUnit;

		// Token: 0x04001965 RID: 6501
		private ParameterHandling parHandling;

		// Token: 0x04001966 RID: 6502
		private bool ignoreClip;

		// Token: 0x04001967 RID: 6503
		private double maxProductLength;

		// Token: 0x04001968 RID: 6504
		private string strGroup;

		// Token: 0x04001969 RID: 6505
		private string additionalLogbookText;

		// Token: 0x0400196A RID: 6506
		private bool showDialogWindows;
	}
}
