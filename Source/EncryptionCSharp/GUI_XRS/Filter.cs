using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Threading;
using Common;
using DatabaseConnection;
using Definitions;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x0200022E RID: 558
	public class Filter : INotifyPropertyChanged
	{
		// Token: 0x17000B68 RID: 2920
		// (get) Token: 0x060029F5 RID: 10741 RVA: 0x00116B17 File Offset: 0x00114D17
		// (set) Token: 0x060029F6 RID: 10742 RVA: 0x00116B1F File Offset: 0x00114D1F
		public Acquisition Acq { get; set; }

		// Token: 0x17000B69 RID: 2921
		// (get) Token: 0x060029F7 RID: 10743 RVA: 0x00116B28 File Offset: 0x00114D28
		// (set) Token: 0x060029F8 RID: 10744 RVA: 0x00116B30 File Offset: 0x00114D30
		public bool EdgeFilterAccessible { get; set; }

		// Token: 0x17000B6A RID: 2922
		// (get) Token: 0x060029F9 RID: 10745 RVA: 0x00116B39 File Offset: 0x00114D39
		// (set) Token: 0x060029FA RID: 10746 RVA: 0x00116B41 File Offset: 0x00114D41
		public bool WeigherAccessible { get; set; }

		// Token: 0x17000B6B RID: 2923
		// (get) Token: 0x060029FB RID: 10747 RVA: 0x00116B4A File Offset: 0x00114D4A
		// (set) Token: 0x060029FC RID: 10748 RVA: 0x00116B52 File Offset: 0x00114D52
		public bool CounterAccessible { get; set; }

		// Token: 0x17000B6C RID: 2924
		// (get) Token: 0x060029FD RID: 10749 RVA: 0x00116B5B File Offset: 0x00114D5B
		// (set) Token: 0x060029FE RID: 10750 RVA: 0x00116B63 File Offset: 0x00114D63
		public bool IgnoreClipAccessible { get; set; }

		// Token: 0x17000B6D RID: 2925
		// (get) Token: 0x060029FF RID: 10751 RVA: 0x00116B6C File Offset: 0x00114D6C
		// (set) Token: 0x06002A00 RID: 10752 RVA: 0x00116B74 File Offset: 0x00114D74
		public bool MaxProductLengthAccessible { get; set; }

		// Token: 0x17000B6E RID: 2926
		// (get) Token: 0x06002A01 RID: 10753 RVA: 0x00116B7D File Offset: 0x00114D7D
		// (set) Token: 0x06002A02 RID: 10754 RVA: 0x00116B85 File Offset: 0x00114D85
		public bool ContaminationCounterAccessible { get; set; }

		// Token: 0x17000B6F RID: 2927
		// (get) Token: 0x06002A03 RID: 10755 RVA: 0x00116B90 File Offset: 0x00114D90
		// (set) Token: 0x06002A04 RID: 10756 RVA: 0x00116BA8 File Offset: 0x00114DA8
		public bool IsEdgeFilterEnabled
		{
			get
			{
				return this.bEdgeFilterEnabled;
			}
			set
			{
				bool flag = !this.bshowDialogWindows || !value || Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtNewAutotrainRequired"), DialogStyle.Warning, DialogButtons.OKCancel) == Dialog.DialogResults.Ok;
				if (flag)
				{
					bool flag2 = this.bEdgeFilterEnabled != value;
					if (flag2)
					{
						this.parHandling.WriteValue(Definition.filterMainPath + this.strImageGroup + Filter.edgeEnabledSubPath, value ? "1" : "0");
						this.bEdgeFilterEnabled = value;
						this.OnPropertyChanged("IsEdgeFilterEnabled");
						bool flag3 = this.bEdgeFilterEnabled;
						if (flag3)
						{
							LogbookManager.AddEntry(SSTEntryCode.FilterActivated, 0);
						}
						else
						{
							LogbookManager.AddEntry(SSTEntryCode.FilterDeactivated, 0);
						}
					}
				}
			}
		}

		// Token: 0x17000B70 RID: 2928
		// (get) Token: 0x06002A05 RID: 10757 RVA: 0x00116C64 File Offset: 0x00114E64
		// (set) Token: 0x06002A06 RID: 10758 RVA: 0x00116C7C File Offset: 0x00114E7C
		public bool IsWeightEnabled
		{
			get
			{
				return this.bWeightEnabled;
			}
			set
			{
				this.bWeightEnabled = value;
				bool flag = this.initialized;
				if (flag)
				{
					this.OnPropertyChanged("IsWeightEnabled");
					bool flag2 = this.bWeightEnabled;
					if (flag2)
					{
						this.parHandling.WriteValue(Definition.filterMainPath + this.strImageGroup + Weigher.enableWeightSubPath, "1");
						LogbookManager.AddEntry(SSTEntryCode.FilterActivated, 1);
					}
					else
					{
						this.parHandling.WriteValue(Definition.filterMainPath + this.strImageGroup + Weigher.enableWeightSubPath, "0");
						LogbookManager.AddEntry(SSTEntryCode.FilterDeactivated, 1);
						this.auditSettings.WeightEnabled = false;
					}
				}
			}
		}

		// Token: 0x17000B71 RID: 2929
		// (get) Token: 0x06002A07 RID: 10759 RVA: 0x00116D30 File Offset: 0x00114F30
		// (set) Token: 0x06002A08 RID: 10760 RVA: 0x00116D48 File Offset: 0x00114F48
		public bool IsCountEnabled
		{
			get
			{
				return this.bCountEnabled;
			}
			set
			{
				bool flag = this.initialized;
				if (flag)
				{
					if (value)
					{
						bool flag2 = this.parHandling.ReadPar(Definition.objectDetectionInferenceEnabled) == "1";
						bool flag3 = flag2;
						if (flag3)
						{
							Dialog.Show(LocalizationManager.Localize("txtInfo"), LocalizationManager.Localize("msgThinkXoEnabled"), DialogStyle.Info, DialogButtons.OK);
							return;
						}
						this.parHandling.WriteValue(Definition.filterMainPath + this.strImageGroup + ObjectCounter.enabledSubPath, "1");
						LogbookManager.AddEntry(SSTEntryCode.FilterActivated, 2);
					}
					else
					{
						this.parHandling.WriteValue(Definition.filterMainPath + this.strImageGroup + ObjectCounter.enabledSubPath, "0");
						LogbookManager.AddEntry(SSTEntryCode.FilterDeactivated, 2);
						this.auditSettings.CountEnabled = false;
					}
					this.bCountEnabled = value;
					this.OnPropertyChanged("IsCountEnabled");
				}
			}
		}

		// Token: 0x17000B72 RID: 2930
		// (get) Token: 0x06002A09 RID: 10761 RVA: 0x00116E38 File Offset: 0x00115038
		// (set) Token: 0x06002A0A RID: 10762 RVA: 0x00116E50 File Offset: 0x00115050
		public bool IsContaminationCountEnabled
		{
			get
			{
				return this.bContaminationCountEnabled;
			}
			set
			{
				this.bContaminationCountEnabled = value;
				bool flag = this.initialized;
				if (flag)
				{
					this.OnPropertyChanged("IsContaminationCountEnabled");
					bool flag2 = this.bContaminationCountEnabled;
					if (flag2)
					{
						this.parHandling.WriteValue(Definition.contaminationsCounterEnablePath, "1");
					}
					else
					{
						this.parHandling.WriteValue(Definition.contaminationsCounterEnablePath, "0");
					}
				}
			}
		}

		// Token: 0x17000B73 RID: 2931
		// (get) Token: 0x06002A0B RID: 10763 RVA: 0x00116EB8 File Offset: 0x001150B8
		// (set) Token: 0x06002A0C RID: 10764 RVA: 0x00116ED0 File Offset: 0x001150D0
		public bool IsSizeFilterEnabled
		{
			get
			{
				return this.bSizeFilterEnabled;
			}
			set
			{
				bool flag = this.bSizeFilterEnabled != value;
				if (flag)
				{
					bool flag2 = !this.productManager.CurrentLoadedProduct.IsRegionProduct && !this.productManager.CurrentLoadedProduct.IsLaneProduct && !this.productManager.CurrentLoadedProduct.IsMultiProduct;
					if (flag2)
					{
						this.parHandling.WriteValue(Definition.filterMainPath + this.strImageGroup + Definition.sizeFilterEnableSubPath, value ? "1" : "0");
						this.bSizeFilterEnabled = value;
						this.OnPropertyChanged("IsSizeFilterEnabled");
						bool flag3 = this.bSizeFilterEnabled;
						if (flag3)
						{
							LogbookManager.AddEntry(SSTEntryCode.FilterActivated, 4);
						}
						else
						{
							LogbookManager.AddEntry(SSTEntryCode.FilterDeactivated, 4);
						}
					}
					else
					{
						bool flag4 = !this.bshowDialogWindows || !value || Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtNewAutotrainRequired"), DialogStyle.Warning, DialogButtons.OKCancel) == Dialog.DialogResults.Ok;
						if (flag4)
						{
							this.parHandling.WriteValue(Definition.filterMainPath + this.strImageGroup + Definition.sizeFilterEnableSubPath, value ? "1" : "0");
							this.bSizeFilterEnabled = value;
							this.OnPropertyChanged("IsSizeFilterEnabled");
							bool flag5 = this.bSizeFilterEnabled;
							if (flag5)
							{
								LogbookManager.AddEntry(SSTEntryCode.FilterActivated, 4);
							}
							else
							{
								LogbookManager.AddEntry(SSTEntryCode.FilterDeactivated, 4);
							}
						}
					}
				}
			}
		}

		// Token: 0x17000B74 RID: 2932
		// (get) Token: 0x06002A0D RID: 10765 RVA: 0x00117040 File Offset: 0x00115240
		// (set) Token: 0x06002A0E RID: 10766 RVA: 0x00117058 File Offset: 0x00115258
		public bool BtnSizeControlVisible
		{
			get
			{
				return this.bBtnSizeControlVisible;
			}
			set
			{
				this.bBtnSizeControlVisible = value;
				this.OnPropertyChanged("BtnSizeControlVisible");
			}
		}

		// Token: 0x17000B75 RID: 2933
		// (get) Token: 0x06002A0F RID: 10767 RVA: 0x00117070 File Offset: 0x00115270
		// (set) Token: 0x06002A10 RID: 10768 RVA: 0x00117088 File Offset: 0x00115288
		public bool SizeFilterForTrainingVisible
		{
			get
			{
				return this.bSizeFilterForTrainingVisible;
			}
			set
			{
				this.bSizeFilterForTrainingVisible = value;
				this.OnPropertyChanged("SizeFilterForTrainingVisible");
			}
		}

		// Token: 0x17000B76 RID: 2934
		// (get) Token: 0x06002A11 RID: 10769 RVA: 0x001170A0 File Offset: 0x001152A0
		// (set) Token: 0x06002A12 RID: 10770 RVA: 0x001170B8 File Offset: 0x001152B8
		public int NLeftPixel
		{
			get
			{
				return this.nLeftPixel;
			}
			set
			{
				bool flag = this.parHandling != null;
				if (flag)
				{
					this.nLeftPixel = value;
					bool flag2 = this.initialized;
					if (flag2)
					{
						bool flag3 = this.EdgeCurrentImage == null || this.EdgeCurrentImage.ConveyingDirection == ConveyingDirection.LeftToRight;
						if (flag3)
						{
							this.parHandling.WriteValue(Definition.filterMainPath + this.strImageGroup + Filter.bottomPixelSubPath, value.ToString());
						}
						else
						{
							this.parHandling.WriteValue(Definition.filterMainPath + this.strImageGroup + Filter.topPixelSubPath, value.ToString());
						}
						this.OnPropertyChanged("NLeftPixel");
					}
				}
			}
		}

		// Token: 0x17000B77 RID: 2935
		// (get) Token: 0x06002A13 RID: 10771 RVA: 0x00117168 File Offset: 0x00115368
		// (set) Token: 0x06002A14 RID: 10772 RVA: 0x00117180 File Offset: 0x00115380
		public int NRightPixel
		{
			get
			{
				return this.nRightPixel;
			}
			set
			{
				bool flag = this.parHandling != null;
				if (flag)
				{
					this.nRightPixel = value;
					bool flag2 = this.initialized;
					if (flag2)
					{
						bool flag3 = this.EdgeCurrentImage == null || this.EdgeCurrentImage.ConveyingDirection == ConveyingDirection.LeftToRight;
						if (flag3)
						{
							this.parHandling.WriteValue(Definition.filterMainPath + this.strImageGroup + Filter.topPixelSubPath, value.ToString());
						}
						else
						{
							this.parHandling.WriteValue(Definition.filterMainPath + this.strImageGroup + Filter.bottomPixelSubPath, value.ToString());
						}
						this.OnPropertyChanged("NRightPixel");
					}
				}
			}
		}

		// Token: 0x17000B78 RID: 2936
		// (get) Token: 0x06002A15 RID: 10773 RVA: 0x00117230 File Offset: 0x00115430
		// (set) Token: 0x06002A16 RID: 10774 RVA: 0x00117248 File Offset: 0x00115448
		public int NTopPixel
		{
			get
			{
				return this.nTopPixel;
			}
			set
			{
				bool flag = this.parHandling != null;
				if (flag)
				{
					this.nTopPixel = value;
					bool flag2 = this.initialized;
					if (flag2)
					{
						this.parHandling.WriteValue(Definition.filterMainPath + this.strImageGroup + Filter.rightPixelSubPath, value.ToString());
						this.OnPropertyChanged("NTopPixel");
					}
				}
			}
		}

		// Token: 0x17000B79 RID: 2937
		// (get) Token: 0x06002A17 RID: 10775 RVA: 0x001172AC File Offset: 0x001154AC
		// (set) Token: 0x06002A18 RID: 10776 RVA: 0x001172C4 File Offset: 0x001154C4
		public int NBottomPixel
		{
			get
			{
				return this.nBottomPixel;
			}
			set
			{
				bool flag = this.parHandling != null;
				if (flag)
				{
					this.nBottomPixel = value;
					bool flag2 = this.initialized;
					if (flag2)
					{
						this.parHandling.WriteValue(Definition.filterMainPath + this.strImageGroup + Filter.leftPixelSubPath, value.ToString());
						this.OnPropertyChanged("NBottomPixel");
					}
				}
			}
		}

		// Token: 0x17000B7A RID: 2938
		// (get) Token: 0x06002A19 RID: 10777 RVA: 0x00117328 File Offset: 0x00115528
		// (set) Token: 0x06002A1A RID: 10778 RVA: 0x00117340 File Offset: 0x00115540
		public bool IsCanFilterEnabled
		{
			get
			{
				return this.bCanFilterEnabled;
			}
			set
			{
				this.bCanFilterEnabled = value;
				bool flag = this.initialized;
				if (flag)
				{
					this.OnPropertyChanged("IsCanFilterEnabled");
					bool flag2 = this.bCanFilterEnabled;
					if (flag2)
					{
						this.parHandling.WriteValue(Definition.filterMainPath + this.strImageGroup + "/can/Enabled/Value", "1");
						LogbookManager.AddEntry(SSTEntryCode.FilterActivated, 5);
					}
					else
					{
						this.parHandling.WriteValue(Definition.filterMainPath + this.strImageGroup + "/can/Enabled/Value", "0");
						LogbookManager.AddEntry(SSTEntryCode.FilterDeactivated, 5);
					}
				}
			}
		}

		// Token: 0x17000B7B RID: 2939
		// (get) Token: 0x06002A1B RID: 10779 RVA: 0x001173E4 File Offset: 0x001155E4
		// (set) Token: 0x06002A1C RID: 10780 RVA: 0x001173FC File Offset: 0x001155FC
		public bool IsAutoEdgeFilterEnabled
		{
			get
			{
				return this.bAutoEdgeFilterEnabled;
			}
			set
			{
				this.bAutoEdgeFilterEnabled = value;
				bool flag = this.initialized;
				if (flag)
				{
					this.OnPropertyChanged("IsAutoEdgeFilterEnabled");
					bool flag2 = this.bAutoEdgeFilterEnabled;
					if (flag2)
					{
						this.parHandling.WriteValue(Definition.filterMainPath + this.strImageGroup + Filter.edgeAutoEdgeFilterSubPath, "1");
						this.IsEdgeFilterVisible = false;
						this.IsEdgeFilterEnabled = false;
					}
					else
					{
						this.parHandling.WriteValue(Definition.filterMainPath + this.strImageGroup + Filter.edgeAutoEdgeFilterSubPath, "0");
						this.IsEdgeFilterVisible = true;
					}
				}
			}
		}

		// Token: 0x17000B7C RID: 2940
		// (get) Token: 0x06002A1D RID: 10781 RVA: 0x001174A0 File Offset: 0x001156A0
		// (set) Token: 0x06002A1E RID: 10782 RVA: 0x001174B8 File Offset: 0x001156B8
		public bool IsEdgeFilterVisible
		{
			get
			{
				return this.bIsEdgeFilterVisble;
			}
			set
			{
				this.bIsEdgeFilterVisble = value;
				this.OnPropertyChanged("IsEdgeFilterVisible");
			}
		}

		// Token: 0x17000B7D RID: 2941
		// (get) Token: 0x06002A1F RID: 10783 RVA: 0x001174D0 File Offset: 0x001156D0
		// (set) Token: 0x06002A20 RID: 10784 RVA: 0x001174E8 File Offset: 0x001156E8
		public bool IsHighThroughputEnabled
		{
			get
			{
				return this.isHighThroughputEnabled;
			}
			set
			{
				this.isHighThroughputEnabled = value;
				this.OnPropertyChanged("IsHighThroughputEnabled");
				if (value)
				{
					this.parHandling.WriteValue(Definition.highThroughputEnabled, "1");
				}
				else
				{
					this.parHandling.WriteValue(Definition.highThroughputEnabled, "0");
				}
			}
		}

		// Token: 0x17000B7E RID: 2942
		// (get) Token: 0x06002A21 RID: 10785 RVA: 0x00117540 File Offset: 0x00115740
		// (set) Token: 0x06002A22 RID: 10786 RVA: 0x00117558 File Offset: 0x00115758
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
						this.parHandling.WriteValue(Definition.anomalyDetectionTrainEnabled, "0");
						LogbookManager.AddEntry(SSTEntryCode.AIDetectionDeactivated, 1, this.productManager.CurrentLoadedProduct.ProductNumber, this.productManager.CurrentLoadedProduct.ProductName);
						this.dbConnection.SetProductTableNameTrainingAI("");
					}
					else
					{
						bool flag5 = value && this.parHandling.ReadPar(Definition.anomalyDetectionTensorRTEngineAvailable) == "0";
						if (!flag5)
						{
							Dialog.Show(LocalizationManager.Localize("txtInfo"), LocalizationManager.Localize("txtAnomalyDetectionOnlyFineTuning"), DialogStyle.Info, DialogButtons.OK);
							return;
						}
						this.parHandling.WriteValue(Definition.anomalyDetectionTrainEnabled, "1");
						LogbookManager.AddEntry(SSTEntryCode.AIDetectionActivated, 1, this.productManager.CurrentLoadedProduct.ProductNumber, this.productManager.CurrentLoadedProduct.ProductName);
					}
					this.anomalyDetectionTrainingOnOff = value;
					this.OnPropertyChanged("AnomalyDetectionTrainingOnOff");
				}
			}
		}

		// Token: 0x17000B7F RID: 2943
		// (get) Token: 0x06002A23 RID: 10787 RVA: 0x001176B5 File Offset: 0x001158B5
		// (set) Token: 0x06002A24 RID: 10788 RVA: 0x001176BC File Offset: 0x001158BC
		public static bool AutoKVChangedToDisabled { get; set; }

		// Token: 0x17000B80 RID: 2944
		// (get) Token: 0x06002A25 RID: 10789 RVA: 0x001176C4 File Offset: 0x001158C4
		public AccessRightsManager AccessManager
		{
			get
			{
				return this.accessManager;
			}
		}

		// Token: 0x17000B81 RID: 2945
		// (get) Token: 0x06002A26 RID: 10790 RVA: 0x001176DC File Offset: 0x001158DC
		// (set) Token: 0x06002A27 RID: 10791 RVA: 0x001176E4 File Offset: 0x001158E4
		public XRSImage EdgeCurrentImage { get; set; }

		// Token: 0x17000B82 RID: 2946
		// (get) Token: 0x06002A28 RID: 10792 RVA: 0x001176ED File Offset: 0x001158ED
		// (set) Token: 0x06002A29 RID: 10793 RVA: 0x001176F5 File Offset: 0x001158F5
		public bool ContaminationCountVisible { get; set; }

		// Token: 0x17000B83 RID: 2947
		// (get) Token: 0x06002A2A RID: 10794 RVA: 0x001176FE File Offset: 0x001158FE
		// (set) Token: 0x06002A2B RID: 10795 RVA: 0x00117706 File Offset: 0x00115906
		public bool CanFilterVisible { get; set; }

		// Token: 0x17000B84 RID: 2948
		// (get) Token: 0x06002A2C RID: 10796 RVA: 0x0011770F File Offset: 0x0011590F
		// (set) Token: 0x06002A2D RID: 10797 RVA: 0x00117717 File Offset: 0x00115917
		public bool AdvancedFiltersVisible { get; set; }

		// Token: 0x17000B85 RID: 2949
		// (get) Token: 0x06002A2E RID: 10798 RVA: 0x00117720 File Offset: 0x00115920
		// (set) Token: 0x06002A2F RID: 10799 RVA: 0x00117738 File Offset: 0x00115938
		public bool IsBulkOptionVisible
		{
			get
			{
				return this.isBulkOptionVisible;
			}
			set
			{
				this.isBulkOptionVisible = value;
				this.OnPropertyChanged("IsBulkOptionVisible");
			}
		}

		// Token: 0x17000B86 RID: 2950
		// (get) Token: 0x06002A30 RID: 10800 RVA: 0x00117750 File Offset: 0x00115950
		// (set) Token: 0x06002A31 RID: 10801 RVA: 0x00117768 File Offset: 0x00115968
		public bool BulkContImagesShown
		{
			get
			{
				return this.bulkContImagesShown;
			}
			set
			{
				this.bulkContImagesShown = value;
				bool flag = this.initialized;
				if (flag)
				{
					this.parHandling.WriteValue(Definition.bulkShowContImages, value ? "1" : "0");
				}
				this.OnPropertyChanged("BulkContImagesShown");
			}
		}

		// Token: 0x17000B87 RID: 2951
		// (get) Token: 0x06002A32 RID: 10802 RVA: 0x001177B3 File Offset: 0x001159B3
		// (set) Token: 0x06002A33 RID: 10803 RVA: 0x001177BB File Offset: 0x001159BB
		public bool ImpurityFinderSettingsVisible { get; set; }

		// Token: 0x17000B88 RID: 2952
		// (get) Token: 0x06002A34 RID: 10804 RVA: 0x001177C4 File Offset: 0x001159C4
		// (set) Token: 0x06002A35 RID: 10805 RVA: 0x001177CC File Offset: 0x001159CC
		public bool IsXRayPowerAdjustmentVisible { get; set; }

		// Token: 0x17000B89 RID: 2953
		// (get) Token: 0x06002A36 RID: 10806 RVA: 0x001177D5 File Offset: 0x001159D5
		// (set) Token: 0x06002A37 RID: 10807 RVA: 0x001177DD File Offset: 0x001159DD
		public bool IsAutoEdgeFilterVisible { get; set; }

		// Token: 0x17000B8A RID: 2954
		// (get) Token: 0x06002A38 RID: 10808 RVA: 0x001177E6 File Offset: 0x001159E6
		// (set) Token: 0x06002A39 RID: 10809 RVA: 0x001177EE File Offset: 0x001159EE
		public bool HighThroughputVisible { get; set; }

		// Token: 0x17000B8B RID: 2955
		// (get) Token: 0x06002A3A RID: 10810 RVA: 0x001177F7 File Offset: 0x001159F7
		// (set) Token: 0x06002A3B RID: 10811 RVA: 0x001177FF File Offset: 0x001159FF
		public bool AnomalyDetectionTrainVisible { get; set; }

		// Token: 0x17000B8C RID: 2956
		// (get) Token: 0x06002A3C RID: 10812 RVA: 0x00117808 File Offset: 0x00115A08
		// (set) Token: 0x06002A3D RID: 10813 RVA: 0x00117810 File Offset: 0x00115A10
		public BoundLocalizedText StrAutoEdgeFilterActive { get; set; }

		// Token: 0x17000B8D RID: 2957
		// (get) Token: 0x06002A3E RID: 10814 RVA: 0x00117819 File Offset: 0x00115A19
		// (set) Token: 0x06002A3F RID: 10815 RVA: 0x00117821 File Offset: 0x00115A21
		public BoundLocalizedText StrXRayPower { get; set; }

		// Token: 0x17000B8E RID: 2958
		// (get) Token: 0x06002A40 RID: 10816 RVA: 0x0011782A File Offset: 0x00115A2A
		// (set) Token: 0x06002A41 RID: 10817 RVA: 0x00117832 File Offset: 0x00115A32
		public BoundLocalizedText StrEdgeFilterActive { get; set; }

		// Token: 0x17000B8F RID: 2959
		// (get) Token: 0x06002A42 RID: 10818 RVA: 0x0011783B File Offset: 0x00115A3B
		// (set) Token: 0x06002A43 RID: 10819 RVA: 0x00117843 File Offset: 0x00115A43
		public BoundLocalizedText StrNLeftPixel { get; set; }

		// Token: 0x17000B90 RID: 2960
		// (get) Token: 0x06002A44 RID: 10820 RVA: 0x0011784C File Offset: 0x00115A4C
		// (set) Token: 0x06002A45 RID: 10821 RVA: 0x00117854 File Offset: 0x00115A54
		public BoundLocalizedText StrNRightPixel { get; set; }

		// Token: 0x17000B91 RID: 2961
		// (get) Token: 0x06002A46 RID: 10822 RVA: 0x0011785D File Offset: 0x00115A5D
		// (set) Token: 0x06002A47 RID: 10823 RVA: 0x00117865 File Offset: 0x00115A65
		public BoundLocalizedText StrNTopPixel { get; set; }

		// Token: 0x17000B92 RID: 2962
		// (get) Token: 0x06002A48 RID: 10824 RVA: 0x0011786E File Offset: 0x00115A6E
		// (set) Token: 0x06002A49 RID: 10825 RVA: 0x00117876 File Offset: 0x00115A76
		public BoundLocalizedText StrNBottomPixel { get; set; }

		// Token: 0x17000B93 RID: 2963
		// (get) Token: 0x06002A4A RID: 10826 RVA: 0x0011787F File Offset: 0x00115A7F
		// (set) Token: 0x06002A4B RID: 10827 RVA: 0x00117887 File Offset: 0x00115A87
		public BoundLocalizedText StrWeigherActive { get; set; }

		// Token: 0x17000B94 RID: 2964
		// (get) Token: 0x06002A4C RID: 10828 RVA: 0x00117890 File Offset: 0x00115A90
		// (set) Token: 0x06002A4D RID: 10829 RVA: 0x00117898 File Offset: 0x00115A98
		public BoundLocalizedText StrObjectCounterActive { get; set; }

		// Token: 0x17000B95 RID: 2965
		// (get) Token: 0x06002A4E RID: 10830 RVA: 0x001178A1 File Offset: 0x00115AA1
		// (set) Token: 0x06002A4F RID: 10831 RVA: 0x001178A9 File Offset: 0x00115AA9
		public BoundLocalizedText StrIgnoreClip { get; set; }

		// Token: 0x17000B96 RID: 2966
		// (get) Token: 0x06002A50 RID: 10832 RVA: 0x001178B2 File Offset: 0x00115AB2
		// (set) Token: 0x06002A51 RID: 10833 RVA: 0x001178BA File Offset: 0x00115ABA
		public BoundLocalizedText StrMaxProductLength { get; set; }

		// Token: 0x17000B97 RID: 2967
		// (get) Token: 0x06002A52 RID: 10834 RVA: 0x001178C3 File Offset: 0x00115AC3
		// (set) Token: 0x06002A53 RID: 10835 RVA: 0x001178CB File Offset: 0x00115ACB
		public BoundLocalizedText StrCountContaminations { get; set; }

		// Token: 0x17000B98 RID: 2968
		// (get) Token: 0x06002A54 RID: 10836 RVA: 0x001178D4 File Offset: 0x00115AD4
		// (set) Token: 0x06002A55 RID: 10837 RVA: 0x001178DC File Offset: 0x00115ADC
		public BoundLocalizedText StrActivateImageSize { get; set; }

		// Token: 0x17000B99 RID: 2969
		// (get) Token: 0x06002A56 RID: 10838 RVA: 0x001178E5 File Offset: 0x00115AE5
		// (set) Token: 0x06002A57 RID: 10839 RVA: 0x001178ED File Offset: 0x00115AED
		public BoundLocalizedText StrImpurityFinderSettings { get; set; }

		// Token: 0x17000B9A RID: 2970
		// (get) Token: 0x06002A58 RID: 10840 RVA: 0x001178F6 File Offset: 0x00115AF6
		// (set) Token: 0x06002A59 RID: 10841 RVA: 0x001178FE File Offset: 0x00115AFE
		public BoundLocalizedText StrCanFilterActive { get; set; }

		// Token: 0x17000B9B RID: 2971
		// (get) Token: 0x06002A5A RID: 10842 RVA: 0x00117907 File Offset: 0x00115B07
		// (set) Token: 0x06002A5B RID: 10843 RVA: 0x0011790F File Offset: 0x00115B0F
		public BoundLocalizedText StrHighThroughput { get; set; }

		// Token: 0x17000B9C RID: 2972
		// (get) Token: 0x06002A5C RID: 10844 RVA: 0x00117918 File Offset: 0x00115B18
		// (set) Token: 0x06002A5D RID: 10845 RVA: 0x00117920 File Offset: 0x00115B20
		public BoundLocalizedText StrAnomalyDetectionTrain { get; set; }

		// Token: 0x17000B9D RID: 2973
		// (get) Token: 0x06002A5E RID: 10846 RVA: 0x00117929 File Offset: 0x00115B29
		// (set) Token: 0x06002A5F RID: 10847 RVA: 0x00117931 File Offset: 0x00115B31
		public BoundLocalizedText StrBulkShowContImages { get; set; }

		// Token: 0x17000B9E RID: 2974
		// (get) Token: 0x06002A60 RID: 10848 RVA: 0x0011793A File Offset: 0x00115B3A
		// (set) Token: 0x06002A61 RID: 10849 RVA: 0x00117942 File Offset: 0x00115B42
		public RelayCommand ShowNEdgePixelControlCommand { get; set; }

		// Token: 0x17000B9F RID: 2975
		// (get) Token: 0x06002A62 RID: 10850 RVA: 0x0011794B File Offset: 0x00115B4B
		// (set) Token: 0x06002A63 RID: 10851 RVA: 0x00117953 File Offset: 0x00115B53
		public RelayCommand ChangeNLeftPixelCommand { get; set; }

		// Token: 0x17000BA0 RID: 2976
		// (get) Token: 0x06002A64 RID: 10852 RVA: 0x0011795C File Offset: 0x00115B5C
		// (set) Token: 0x06002A65 RID: 10853 RVA: 0x00117964 File Offset: 0x00115B64
		public RelayCommand ChangeNRightPixelCommand { get; set; }

		// Token: 0x17000BA1 RID: 2977
		// (get) Token: 0x06002A66 RID: 10854 RVA: 0x0011796D File Offset: 0x00115B6D
		// (set) Token: 0x06002A67 RID: 10855 RVA: 0x00117975 File Offset: 0x00115B75
		public RelayCommand ChangeNTopPixelCommand { get; set; }

		// Token: 0x17000BA2 RID: 2978
		// (get) Token: 0x06002A68 RID: 10856 RVA: 0x0011797E File Offset: 0x00115B7E
		// (set) Token: 0x06002A69 RID: 10857 RVA: 0x00117986 File Offset: 0x00115B86
		public RelayCommand ChangeNBottomPixelCommand { get; set; }

		// Token: 0x17000BA3 RID: 2979
		// (get) Token: 0x06002A6A RID: 10858 RVA: 0x0011798F File Offset: 0x00115B8F
		// (set) Token: 0x06002A6B RID: 10859 RVA: 0x00117997 File Offset: 0x00115B97
		public RelayCommand ShowWeigherControlCommand { get; set; }

		// Token: 0x17000BA4 RID: 2980
		// (get) Token: 0x06002A6C RID: 10860 RVA: 0x001179A0 File Offset: 0x00115BA0
		// (set) Token: 0x06002A6D RID: 10861 RVA: 0x001179A8 File Offset: 0x00115BA8
		public RelayCommand ShowObjectCounterControlCommand { get; set; }

		// Token: 0x17000BA5 RID: 2981
		// (get) Token: 0x06002A6E RID: 10862 RVA: 0x001179B1 File Offset: 0x00115BB1
		// (set) Token: 0x06002A6F RID: 10863 RVA: 0x001179B9 File Offset: 0x00115BB9
		public RelayCommand ShowSizeControlCommand { get; set; }

		// Token: 0x17000BA6 RID: 2982
		// (get) Token: 0x06002A70 RID: 10864 RVA: 0x001179C2 File Offset: 0x00115BC2
		// (set) Token: 0x06002A71 RID: 10865 RVA: 0x001179CA File Offset: 0x00115BCA
		public RelayCommand ShowCanFilterControlCommand { get; set; }

		// Token: 0x17000BA7 RID: 2983
		// (get) Token: 0x06002A72 RID: 10866 RVA: 0x001179D3 File Offset: 0x00115BD3
		// (set) Token: 0x06002A73 RID: 10867 RVA: 0x001179DB File Offset: 0x00115BDB
		public RelayCommand ShowImpurityFinderSettingsCommand { get; set; }

		// Token: 0x17000BA8 RID: 2984
		// (get) Token: 0x06002A74 RID: 10868 RVA: 0x001179E4 File Offset: 0x00115BE4
		// (set) Token: 0x06002A75 RID: 10869 RVA: 0x001179EC File Offset: 0x00115BEC
		public RelayCommand ShowAutokVSettingsCommand { get; set; }

		// Token: 0x140000DE RID: 222
		// (add) Token: 0x06002A76 RID: 10870 RVA: 0x001179F8 File Offset: 0x00115BF8
		// (remove) Token: 0x06002A77 RID: 10871 RVA: 0x00117A30 File Offset: 0x00115C30
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x140000DF RID: 223
		// (add) Token: 0x06002A78 RID: 10872 RVA: 0x00117A68 File Offset: 0x00115C68
		// (remove) Token: 0x06002A79 RID: 10873 RVA: 0x00117AA0 File Offset: 0x00115CA0
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action ChangePixelCommandExecuted;

		// Token: 0x06002A7A RID: 10874 RVA: 0x00117AD8 File Offset: 0x00115CD8
		public Filter(ParameterHandling parHandling, ImageDataTransfer imageTransfer, DBConnection dbConnection, AccessRightsManager accessManager, MachineState machineState, ProductManager productManager, int imageGroup, StoreImageData imageArchiver, bool showDialogWindows, TestcardSimService testcardSimService, AuditCheckSettings auditSettings)
		{
			this.parHandling = parHandling;
			this.dbConnection = dbConnection;
			this.accessManager = accessManager;
			this.imageTransfer = imageTransfer;
			this.machineState = machineState;
			this.productManager = productManager;
			this.imageGroup = imageGroup;
			this.imageArchiver = imageArchiver;
			this.testcardSimService = testcardSimService;
			this.bshowDialogWindows = showDialogWindows;
			this.auditSettings = auditSettings;
			this.HighThroughputVisible = false;
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
						Definition.subTableIdentifier.ToString(),
						imageGroup.ToString(),
						" ",
						array[0].ProductName
					});
				}
			}
			this.strImageGroup = ((imageGroup > 0) ? imageGroup.ToString() : "");
			this.Acq = new Acquisition(parHandling, imageGroup, productManager, showDialogWindows, dbConnection);
			this.RegisterLocalizedText();
			this.ShowNEdgePixelControlCommand = new RelayCommand(new Action<object>(this.ShowNEdgePixelControl));
			this.ChangeNLeftPixelCommand = new RelayCommand(new Action<object>(this.ChangeNLeftPixel));
			this.ChangeNRightPixelCommand = new RelayCommand(new Action<object>(this.ChangeNRightPixel));
			this.ChangeNTopPixelCommand = new RelayCommand(new Action<object>(this.ChangeNTopPixel));
			this.ChangeNBottomPixelCommand = new RelayCommand(new Action<object>(this.ChangeNBottomPixel));
			this.ShowObjectCounterControlCommand = new RelayCommand(new Action<object>(this.ShowObjectCounterControl));
			this.ShowSizeControlCommand = new RelayCommand(new Action<object>(this.ShowSizeControl));
			this.ShowWeigherControlCommand = new RelayCommand(new Action<object>(this.ShowWeigherControl));
			this.ShowCanFilterControlCommand = new RelayCommand(new Action<object>(this.ShowCanFilterControl));
			this.ShowImpurityFinderSettingsCommand = new RelayCommand(new Action<object>(this.ShowImpurityFinderSettings));
			this.ShowAutokVSettingsCommand = new RelayCommand(new Action<object>(this.ShowAutokVSettings));
			this.EdgeFilterAccessible = (accessManager.CurrentUser.Rights >= this.rightEdgeFilterEnabled);
			this.MaxProductLengthAccessible = (accessManager.CurrentUser.Rights >= this.rightChangeMaxProductLength);
			bool flag3 = this.parHandling != null && dbConnection.Connection != null;
			if (flag3)
			{
				string a = "";
				string tableName = (imageGroup <= 0) ? productManager.CurrentLoadedProduct.TableName : (productManager.CurrentLoadedProduct.TableName + Definition.subTableIdentifier.ToString() + this.strImageGroup);
				dbConnection.GetParameterValue(Definition.filterMainPath + this.strImageGroup + Filter.edgeAutoEdgeFilterSubPath, tableName, ref a);
				this.bAutoEdgeFilterEnabled = (a == "1");
				dbConnection.GetParameterValue(Definition.filterMainPath + this.strImageGroup + Filter.edgeEnabledSubPath, tableName, ref a);
				this.bEdgeFilterEnabled = (a == "1");
				dbConnection.GetParameterValue(Definition.filterMainPath + this.strImageGroup + Weigher.enableWeightSubPath, tableName, ref a);
				this.bWeightEnabled = (a == "1");
				dbConnection.GetParameterValue(Definition.filterMainPath + this.strImageGroup + ObjectCounter.enabledSubPath, tableName, ref a);
				this.bCountEnabled = (a == "1");
				dbConnection.GetParameterValue(Definition.filterMainPath + this.strImageGroup + Definition.sizeFilterEnableSubPath, tableName, ref a);
				this.bSizeFilterEnabled = (a == "1");
				dbConnection.GetParameterValue(Definition.filterMainPath + this.strImageGroup + "/can/Enabled/Value", tableName, ref a);
				this.bCanFilterEnabled = (a == "1");
				dbConnection.GetParameterValue(Definition.contaminationsCounterEnablePath, productManager.CurrentLoadedProduct.TableName, ref a);
				this.bContaminationCountEnabled = (a == "1");
				dbConnection.GetParameterValue(Definition.highThroughputEnabled, ref a);
				bool flag4 = false;
				dbConnection.GetHighThroughputMachine(ref flag4);
				this.HighThroughputVisible = flag4;
				bool flag5 = flag4;
				if (flag5)
				{
					this.IsHighThroughputEnabled = (a == "1");
				}
			}
			this.initialized = true;
			this.ContaminationCountVisible = this.ChangeContaminationCountEnabledCanExecuteCommand(null);
			MachineMode machineMode = MachineMode.Standard;
			dbConnection.GetMachineMode(ref machineMode);
			this.CanFilterVisible = (machineMode == MachineMode.Sideshooter);
			this.multiRegionEnable = false;
			dbConnection.GetMultiRegionEnabled(ref this.multiRegionEnable);
			this.multiLaneProductEnable = false;
			dbConnection.GetMultiLaneProductsEnabled(ref this.multiLaneProductEnable);
			this.multiProductEnable = false;
			dbConnection.GetMultiProductsEnabled(ref this.multiProductEnable);
			bool flag6 = !productManager.CurrentLoadedProduct.IsRegionProduct && !productManager.CurrentLoadedProduct.IsLaneProduct && !productManager.CurrentLoadedProduct.IsMultiProduct;
			if (flag6)
			{
				this.BtnSizeControlVisible = true;
				Filter.SizeControlAvailable = true;
			}
			else
			{
				this.BtnSizeControlVisible = false;
				Filter.SizeControlAvailable = false;
			}
			this.SetAdvancedFiltersVisible();
			DetectorType detectorType = DetectorType.Unknown;
			dbConnection.GetDetectorType(ref detectorType);
			string a2 = "";
			bool flag7 = detectorType == DetectorType.Sesotec;
			if (flag7)
			{
				a2 = parHandling.ReadPar(Definition.detectorSesotecPixelSizePath);
			}
			else
			{
				bool flag8 = detectorType == DetectorType.DTNetTDI;
				if (flag8)
				{
					a2 = parHandling.ReadPar(Definition.detectorDtPixelSizePath);
				}
			}
			bool flag9 = false;
			dbConnection.GetDemoMode(ref flag9);
			bool flag10 = Machine.Instance.MachineType == MachineType.RayconDPlus || (flag9 && a2 == "0.4") || Machine.Instance.MachineType == MachineType.RayconEX2A;
			if (flag10)
			{
				this.IsXRayPowerAdjustmentVisible = true;
				string a3 = "";
				dbConnection.GetXRaySourceModelNumber(ref a3);
				bool flag11 = true;
				dbConnection.GetMXFeatures04Det(ref flag11);
				bool flag12 = a3 == "IXS050SE100P214" && a2 == "0.4";
				bool flag13 = detectorType == DetectorType.Sesotec || detectorType == DetectorType.DTNetTDI;
				if (flag13)
				{
					bool flag14 = a2 == "0.4";
					if (flag14)
					{
						bool flag15 = (flag12 && flag11) || Machine.Instance.MachineType == MachineType.RayconEX2A;
						if (flag15)
						{
							this.isOnlyManualKvActive = true;
							this.IsAutoEdgeFilterVisible = false;
							this.IsAutoEdgeFilterEnabled = false;
							this.IsEdgeFilterVisible = true;
						}
						else
						{
							bool flag16 = flag12 && !flag11;
							if (flag16)
							{
								this.isOnlyManualKvActive = true;
								this.IsAutoEdgeFilterVisible = true;
								bool isAutoEdgeFilterEnabled = this.IsAutoEdgeFilterEnabled;
								if (isAutoEdgeFilterEnabled)
								{
									this.IsEdgeFilterVisible = false;
								}
								else
								{
									this.IsEdgeFilterVisible = true;
								}
							}
							else
							{
								bool flag17 = (productManager.CurrentLoadedProduct.IsSingleProduct && (!this.multiLaneProductEnable || this.multiProductEnable)) || this.multiRegionEnable;
								if (flag17)
								{
									this.isOnlyManualKvActive = false;
								}
								else
								{
									this.isOnlyManualKvActive = true;
								}
								this.IsAutoEdgeFilterVisible = true;
								bool isAutoEdgeFilterEnabled2 = this.IsAutoEdgeFilterEnabled;
								if (isAutoEdgeFilterEnabled2)
								{
									this.IsEdgeFilterVisible = false;
								}
								else
								{
									this.IsEdgeFilterVisible = true;
								}
							}
						}
					}
					else
					{
						bool flag18 = a2 == "0.8";
						if (flag18)
						{
							this.isOnlyManualKvActive = true;
							this.IsAutoEdgeFilterVisible = false;
							this.IsEdgeFilterVisible = true;
						}
					}
				}
				else
				{
					bool flag19 = detectorType == DetectorType.DTNetDE;
					if (flag19)
					{
						this.isOnlyManualKvActive = true;
						this.IsAutoEdgeFilterVisible = true;
						this.IsEdgeFilterVisible = true;
						bool isAutoEdgeFilterEnabled3 = this.IsAutoEdgeFilterEnabled;
						if (isAutoEdgeFilterEnabled3)
						{
							this.IsEdgeFilterVisible = false;
						}
						else
						{
							this.IsEdgeFilterVisible = true;
						}
					}
				}
			}
			else
			{
				this.IsAutoEdgeFilterVisible = false;
				this.IsEdgeFilterVisible = true;
				this.IsXRayPowerAdjustmentVisible = false;
				this.isOnlyManualKvActive = false;
			}
			bool flag20 = machineMode == MachineMode.Bulk;
			if (flag20)
			{
				this.IsAutoEdgeFilterVisible = false;
				this.IsEdgeFilterVisible = false;
				this.ContaminationCountVisible = false;
				bool flag21 = !BulkMode.NuvoPCPerformance;
				if (flag21)
				{
					this.ImpurityFinderSettingsVisible = false;
				}
				else
				{
					bool flag22 = a2 == "0.8";
					if (flag22)
					{
						this.ImpurityFinderSettingsVisible = false;
					}
				}
				this.IsBulkOptionVisible = true;
				string a4 = parHandling.ReadPar(Definition.bulkShowContImages);
				bool flag23 = a4 == "1";
				if (flag23)
				{
					this.BulkContImagesShown = true;
				}
				else
				{
					this.BulkContImagesShown = false;
				}
			}
		}

		// Token: 0x06002A7B RID: 10875 RVA: 0x001183BC File Offset: 0x001165BC
		private void SetAdvancedFiltersVisible()
		{
			MachineType machineType = MachineType.MachineTypeUndefined;
			bool flag = false;
			this.dbConnection.GetMachineType(ref machineType);
			this.dbConnection.GetAdvancedFilters(ref flag);
			this.AdvancedFiltersVisible = (machineType == MachineType.RayconXRS || flag);
			bool flag2 = this.AdvancedFiltersVisible && this.BtnSizeControlVisible;
			if (flag2)
			{
				this.SizeFilterForTrainingVisible = false;
			}
			else
			{
				bool flag3 = this.AdvancedFiltersVisible && !this.BtnSizeControlVisible;
				if (flag3)
				{
					this.SizeFilterForTrainingVisible = true;
				}
				else
				{
					this.SizeFilterForTrainingVisible = false;
				}
			}
			this.ImpurityFinderSettingsVisible = (machineType > MachineType.RayconXRS);
			bool flag4 = machineType == MachineType.RayconDPlus;
			if (flag4)
			{
				string a = "";
				DetectorType detectorType = DetectorType.DT;
				this.dbConnection.GetDetectorType(ref detectorType);
				bool flag5 = detectorType == DetectorType.Sesotec;
				if (flag5)
				{
					a = this.parHandling.ReadPar(Definition.detectorSesotecPixelSizePath);
				}
				bool flag6 = a == "0.8" && this.accessManager.CurrentUser.Rights < BuiltInPriviledges.Service;
				if (flag6)
				{
					this.ImpurityFinderSettingsVisible = false;
				}
			}
			bool flag7 = this.parHandling.ReadPar(Definition.anomalyDetectionEnabled) == "1";
			if (flag7)
			{
				string tableName = "";
				this.dbConnection.GetCurrentProductTableNameAI(ref tableName);
				string archiveDirectoryAnomalyDetection = this.imageArchiver.GetArchiveDirectoryAnomalyDetection(tableName);
				bool flag8 = false;
				bool flag9 = Directory.Exists(archiveDirectoryAnomalyDetection);
				if (flag9)
				{
					flag8 = Directory.EnumerateFiles(archiveDirectoryAnomalyDetection).Any<string>();
				}
				bool flag10 = this.parHandling.ReadPar(Definition.anomalyDetectionTrainingInProgress) == "1";
				bool flag11 = this.parHandling.ReadPar(Definition.anomalyDetectionInferenceModelID) == this.parHandling.ReadPar(Definition.anomalyDetectionTrainModelID);
				this.AnomalyDetectionTrainVisible = ((!flag10 || flag11) && !flag8);
				this.anomalyDetectionTrainingOnOff = (this.parHandling.ReadPar(Definition.anomalyDetectionTrainEnabled) == "1");
			}
		}

		// Token: 0x06002A7C RID: 10876 RVA: 0x001185A8 File Offset: 0x001167A8
		public void InitializeNPixels()
		{
			try
			{
				bool flag = this.EdgeCurrentImage == null || this.EdgeCurrentImage.ConveyingDirection == ConveyingDirection.LeftToRight;
				if (flag)
				{
					int.TryParse(this.parHandling.ReadPar(Definition.filterMainPath + this.strImageGroup + Filter.bottomPixelSubPath), out this.nLeftPixel);
					int.TryParse(this.parHandling.ReadPar(Definition.filterMainPath + this.strImageGroup + Filter.topPixelSubPath), out this.nRightPixel);
				}
				else
				{
					int.TryParse(this.parHandling.ReadPar(Definition.filterMainPath + this.strImageGroup + Filter.topPixelSubPath), out this.nLeftPixel);
					int.TryParse(this.parHandling.ReadPar(Definition.filterMainPath + this.strImageGroup + Filter.bottomPixelSubPath), out this.nRightPixel);
				}
				int.TryParse(this.parHandling.ReadPar(Definition.filterMainPath + this.strImageGroup + Filter.rightPixelSubPath), out this.nTopPixel);
				int.TryParse(this.parHandling.ReadPar(Definition.filterMainPath + this.strImageGroup + Filter.leftPixelSubPath), out this.nBottomPixel);
				this.OnPropertyChanged("NLeftPixel");
				this.OnPropertyChanged("NRightPixel");
				this.OnPropertyChanged("NTopPixel");
				this.OnPropertyChanged("NBottomPixel");
			}
			catch
			{
			}
		}

		// Token: 0x06002A7D RID: 10877 RVA: 0x00118738 File Offset: 0x00116938
		private void RegisterLocalizedText()
		{
			this.StrAutoEdgeFilterActive = new BoundLocalizedText("txtAutoEdgeFilterActive");
			this.StrXRayPower = new BoundLocalizedText("txtXRayPower");
			this.StrEdgeFilterActive = new BoundLocalizedText("txtEdgeFilterActive");
			this.StrNLeftPixel = new BoundLocalizedText("txtNumLeftPixel");
			this.StrNRightPixel = new BoundLocalizedText("txtNumRightPixel");
			this.StrNTopPixel = new BoundLocalizedText("txtNumTopPixel");
			this.StrNBottomPixel = new BoundLocalizedText("txtNumBottomPixel");
			this.StrWeigherActive = new BoundLocalizedText("txtWeigherActive");
			this.StrObjectCounterActive = new BoundLocalizedText("txtObjectCounterActive");
			this.StrIgnoreClip = new BoundLocalizedText("txtIgnoreClip");
			this.StrMaxProductLength = new BoundLocalizedText("txtMaxProductLength");
			this.StrCountContaminations = new BoundLocalizedText("txtCountContaminations");
			this.StrActivateImageSize = new BoundLocalizedText("txtActivateImageSize");
			this.StrCanFilterActive = new BoundLocalizedText("txtCanFilterActive");
			this.StrImpurityFinderSettings = new BoundLocalizedText("txtImpurityFinderSettings");
			this.StrHighThroughput = new BoundLocalizedText("txtHighThroughputMode");
			this.StrAnomalyDetectionTrain = new BoundLocalizedText("txtTrainingEnabled2");
			this.StrBulkShowContImages = new BoundLocalizedText("txtBulkShowContimage");
		}

		// Token: 0x06002A7E RID: 10878 RVA: 0x00118878 File Offset: 0x00116A78
		public void DeregisterLocalizedText()
		{
			MainWindow.localizationManager.Deregister(this.StrAutoEdgeFilterActive);
			MainWindow.localizationManager.Deregister(this.StrXRayPower);
			MainWindow.localizationManager.Deregister(this.StrEdgeFilterActive);
			MainWindow.localizationManager.Deregister(this.StrNLeftPixel);
			MainWindow.localizationManager.Deregister(this.StrNRightPixel);
			MainWindow.localizationManager.Deregister(this.StrNTopPixel);
			MainWindow.localizationManager.Deregister(this.StrNBottomPixel);
			MainWindow.localizationManager.Deregister(this.StrWeigherActive);
			MainWindow.localizationManager.Deregister(this.StrObjectCounterActive);
			MainWindow.localizationManager.Deregister(this.StrIgnoreClip);
			MainWindow.localizationManager.Deregister(this.StrMaxProductLength);
			MainWindow.localizationManager.Deregister(this.StrCountContaminations);
			MainWindow.localizationManager.Deregister(this.StrActivateImageSize);
			MainWindow.localizationManager.Deregister(this.StrCanFilterActive);
			MainWindow.localizationManager.Deregister(this.StrImpurityFinderSettings);
			MainWindow.localizationManager.Deregister(this.StrHighThroughput);
			MainWindow.localizationManager.Deregister(this.StrAnomalyDetectionTrain);
			MainWindow.localizationManager.Deregister(this.StrBulkShowContImages);
		}

		// Token: 0x06002A7F RID: 10879 RVA: 0x001189B8 File Offset: 0x00116BB8
		public void ShowNEdgePixelControl(object obj)
		{
			bool flag = !this.IsEdgeFilterEnabled;
			if (flag)
			{
				bool flag2 = Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("msgEdgeFilterDisabled"), DialogStyle.Warning, DialogButtons.OKCancel) == Dialog.DialogResults.Ok;
				if (flag2)
				{
					this.IsEdgeFilterEnabled = true;
					bool flag3 = this.parHandling.ReadPar(Definition.filterMainPath + this.strImageGroup + Filter.edgeEnabledSubPath) != "1";
					if (flag3)
					{
						this.IsEdgeFilterEnabled = false;
						Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("msgEnableEdgeFilterFailed"), DialogStyle.Error, DialogButtons.OK);
					}
				}
			}
			bool isEdgeFilterEnabled = this.IsEdgeFilterEnabled;
			if (isEdgeFilterEnabled)
			{
				CtrlChangeEdgePixel ctrlChangeEdgePixel = new CtrlChangeEdgePixel(this.parHandling, this.imageTransfer, this, this.imageGroup, this.productManager, this.machineState, this.imageArchiver);
				Subwindow.Show(ctrlChangeEdgePixel, SubwindowButtons.Back, ctrlChangeEdgePixel.ControlUIElement);
			}
		}

		// Token: 0x06002A80 RID: 10880 RVA: 0x00118AA0 File Offset: 0x00116CA0
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x06002A81 RID: 10881 RVA: 0x00118AD0 File Offset: 0x00116CD0
		private void numKeyboard_InputNLeftPixel(object sender, KeyboardInputEventArgs e)
		{
			try
			{
				this.NLeftPixel = int.Parse(e.Text);
				this.OnChangePixelCommandExecuted();
			}
			catch
			{
			}
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_InputNLeftPixel;
		}

		// Token: 0x06002A82 RID: 10882 RVA: 0x00118B28 File Offset: 0x00116D28
		private void numKeyboard_InputNRightPixel(object sender, KeyboardInputEventArgs e)
		{
			try
			{
				this.NRightPixel = int.Parse(e.Text);
				this.OnChangePixelCommandExecuted();
			}
			catch
			{
			}
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_InputNRightPixel;
		}

		// Token: 0x06002A83 RID: 10883 RVA: 0x00118B80 File Offset: 0x00116D80
		private void numKeyboard_InputNTopPixel(object sender, KeyboardInputEventArgs e)
		{
			try
			{
				this.NTopPixel = int.Parse(e.Text);
				this.OnChangePixelCommandExecuted();
			}
			catch
			{
			}
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_InputNTopPixel;
		}

		// Token: 0x06002A84 RID: 10884 RVA: 0x00118BD8 File Offset: 0x00116DD8
		private void numKeyboard_InputNBottomPixel(object sender, KeyboardInputEventArgs e)
		{
			try
			{
				this.NBottomPixel = int.Parse(e.Text);
				this.OnChangePixelCommandExecuted();
			}
			catch
			{
			}
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_InputNBottomPixel;
		}

		// Token: 0x06002A85 RID: 10885 RVA: 0x00118C30 File Offset: 0x00116E30
		private bool ChangeWeightEnabledCanExecuteCommand(object dummy)
		{
			this.WeigherAccessible = (this.accessManager.CurrentUser.Rights >= this.rightWeightEnabled);
			return this.WeigherAccessible;
		}

		// Token: 0x06002A86 RID: 10886 RVA: 0x00118C6C File Offset: 0x00116E6C
		private bool ChangeCountEnabledCanExecuteCommand(object dummy)
		{
			this.CounterAccessible = (this.accessManager.CurrentUser.Rights >= this.rightCountEnabled);
			return this.CounterAccessible;
		}

		// Token: 0x06002A87 RID: 10887 RVA: 0x00118CA8 File Offset: 0x00116EA8
		private bool ChangeContaminationCountEnabledCanExecuteCommand(object dummy)
		{
			this.ContaminationCounterAccessible = (this.accessManager.CurrentUser.Rights >= Filter.rightContaminationCountEnabled);
			return this.ContaminationCounterAccessible;
		}

		// Token: 0x06002A88 RID: 10888 RVA: 0x00118CE4 File Offset: 0x00116EE4
		public void ChangeNLeftPixel(object obj)
		{
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtNumEdgePixel"), this.NLeftPixel.ToString(), NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_InputNLeftPixel;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x06002A89 RID: 10889 RVA: 0x00118D3C File Offset: 0x00116F3C
		public void ChangeNRightPixel(object obj)
		{
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtNumEdgePixel"), this.NRightPixel.ToString(), NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_InputNRightPixel;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x06002A8A RID: 10890 RVA: 0x00118D94 File Offset: 0x00116F94
		public void ChangeNTopPixel(object obj)
		{
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtNumEdgePixel"), this.NTopPixel.ToString(), NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_InputNTopPixel;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x06002A8B RID: 10891 RVA: 0x00118DEC File Offset: 0x00116FEC
		public void ChangeNBottomPixel(object obj)
		{
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtNumEdgePixel"), this.NBottomPixel.ToString(), NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_InputNBottomPixel;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x06002A8C RID: 10892 RVA: 0x00118E44 File Offset: 0x00117044
		protected virtual void OnChangePixelCommandExecuted()
		{
			Action changePixelCommandExecuted = this.ChangePixelCommandExecuted;
			bool flag = changePixelCommandExecuted != null;
			if (flag)
			{
				changePixelCommandExecuted();
			}
		}

		// Token: 0x06002A8D RID: 10893 RVA: 0x00118E68 File Offset: 0x00117068
		public void ShowObjectCounterControl(object obj)
		{
			bool flag = !this.IsCountEnabled;
			if (flag)
			{
				bool flag2 = Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("msgObjectCounterDisabled"), DialogStyle.Warning, DialogButtons.OKCancel) == Dialog.DialogResults.Ok;
				if (flag2)
				{
					this.IsCountEnabled = true;
					bool flag3 = this.parHandling.ReadPar(Definition.filterMainPath + this.strImageGroup + ObjectCounter.enabledSubPath) != "1";
					if (flag3)
					{
						this.IsCountEnabled = false;
						bool flag4 = this.parHandling.ReadPar(Definition.objectDetectionInferenceEnabled) == "1";
						bool flag5 = !flag4;
						if (flag5)
						{
							Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("msgEnableCounterFailed"), DialogStyle.Error, DialogButtons.OK);
						}
					}
				}
			}
			bool isCountEnabled = this.IsCountEnabled;
			if (isCountEnabled)
			{
				CtrlLearnCounter ctrlLearnCounter = new CtrlLearnCounter(this.dbConnection, this.imageTransfer, this.parHandling, this.imageGroup, this.productManager, this.imageArchiver, this.machineState);
				Subwindow.Show(ctrlLearnCounter, SubwindowButtons.Back, ctrlLearnCounter.PanelSubButtons);
			}
		}

		// Token: 0x06002A8E RID: 10894 RVA: 0x00118F80 File Offset: 0x00117180
		public void ShowSizeControl(object obj)
		{
			bool flag = !this.IsSizeFilterEnabled;
			if (flag)
			{
				bool flag2 = Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("msgSizeFilterDisabled"), DialogStyle.Warning, DialogButtons.OKCancel) == Dialog.DialogResults.Ok;
				if (flag2)
				{
					this.IsSizeFilterEnabled = true;
					bool flag3 = this.parHandling.ReadPar(Definition.filterMainPath + this.strImageGroup + Definition.sizeFilterEnableSubPath) != "1";
					if (flag3)
					{
						this.IsSizeFilterEnabled = false;
						Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("msgEnableSizeFilterFailed"), DialogStyle.Error, DialogButtons.OK);
					}
				}
			}
			bool isSizeFilterEnabled = this.IsSizeFilterEnabled;
			if (isSizeFilterEnabled)
			{
				CtrlSizeFilter usrControl = new CtrlSizeFilter(this.parHandling, this.imageTransfer, this.dbConnection, this, this.imageGroup, this.productManager, this.machineState, this.imageArchiver);
				Subwindow.Show(usrControl, SubwindowButtons.Back);
			}
		}

		// Token: 0x06002A8F RID: 10895 RVA: 0x00119068 File Offset: 0x00117268
		public void ShowWeigherControl(object obj)
		{
			bool flag = !this.IsWeightEnabled;
			if (flag)
			{
				bool flag2 = Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("msgWeightDisabled"), DialogStyle.Warning, DialogButtons.OKCancel) == Dialog.DialogResults.Ok;
				if (flag2)
				{
					this.IsWeightEnabled = true;
					bool flag3 = this.parHandling.ReadPar(Definition.filterMainPath + this.strImageGroup + Weigher.enableWeightSubPath) != "1";
					if (flag3)
					{
						this.IsWeightEnabled = false;
						Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("msgEnableWeightFailed"), DialogStyle.Error, DialogButtons.OK);
					}
				}
			}
			bool isWeightEnabled = this.IsWeightEnabled;
			if (isWeightEnabled)
			{
				WeigherControl weigherControl = new WeigherControl(this.parHandling, this.imageTransfer, this.dbConnection, this.machineState, this.productManager, this.imageGroup, this.accessManager);
				Subwindow.Show(weigherControl, SubwindowButtons.Back, weigherControl.Buttons);
			}
		}

		// Token: 0x06002A90 RID: 10896 RVA: 0x00119158 File Offset: 0x00117358
		private void ShowCanFilterControl(object obj)
		{
			bool flag = !this.IsCanFilterEnabled;
			if (flag)
			{
				bool flag2 = Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("msgFilterDisabled"), DialogStyle.Warning, DialogButtons.OKCancel) == Dialog.DialogResults.Ok;
				if (flag2)
				{
					this.IsCanFilterEnabled = true;
					bool flag3 = this.parHandling.ReadPar(Definition.filterMainPath + this.strImageGroup + "/can/Enabled/Value") != "1";
					if (flag3)
					{
						this.IsCanFilterEnabled = false;
						Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("msgEnableFilterFailed"), DialogStyle.Error, DialogButtons.OK);
					}
				}
			}
			bool isCanFilterEnabled = this.IsCanFilterEnabled;
			if (isCanFilterEnabled)
			{
				CtrlCanFilter ctrlCanFilter = new CtrlCanFilter(this.parHandling, this.imageTransfer, this.dbConnection, this.productManager, this.machineState, this.imageGroup, this.imageArchiver);
				Subwindow.Show(ctrlCanFilter, SubwindowButtons.Back, ctrlCanFilter.Buttons);
			}
		}

		// Token: 0x06002A91 RID: 10897 RVA: 0x00119248 File Offset: 0x00117448
		public static void WriteToImageProcessingFilters(ParameterHandling parHandling, string attachment, string value, int group)
		{
			string text = (group > 0) ? group.ToString() : "";
			for (int i = 0; i < Definition.nObjectFinder; i++)
			{
				parHandling.WriteValue(string.Concat(new string[]
				{
					Definition.filterMainPath,
					text,
					Definition.objectFindersSubPath,
					(i + 1).ToString(),
					"/",
					attachment,
					"/",
					Definition.strValue
				}), value, false);
			}
			parHandling.WriteValue(string.Concat(new string[]
			{
				Definition.filterMainPath,
				text,
				Definition.thresholdSubPath,
				"/",
				attachment,
				"/",
				Definition.strValue
			}), value, false);
			parHandling.WriteValue(string.Concat(new string[]
			{
				Definition.filterMainPath,
				text,
				Definition.thresholdGlobalSubPath,
				"/",
				attachment,
				"/",
				Definition.strValue
			}), value, false);
			bool flag = !Filter.SizeControlAvailable;
			if (flag)
			{
				parHandling.WriteValue(string.Concat(new string[]
				{
					Definition.filterMainPath,
					text,
					Definition.sizeFilterSubPath,
					"/",
					attachment,
					"/",
					Definition.strValue
				}), value, false);
			}
			parHandling.WriteValue(string.Concat(new string[]
			{
				Definition.filterMainPath,
				text,
				Definition.wireFinderSubPath,
				"/",
				attachment,
				"/",
				Definition.strValue
			}), value, false);
			parHandling.WriteValue(string.Concat(new string[]
			{
				Definition.filterMainPath,
				text,
				Definition.dimensionSubPath,
				"/",
				attachment,
				"/",
				Definition.strValue
			}), value, false);
			parHandling.WriteValue(string.Concat(new string[]
			{
				Definition.filterMainPath,
				text,
				Definition.lowDensitySubPath,
				"/",
				attachment,
				"/",
				Definition.strValue
			}), value, false);
			parHandling.WriteValue(string.Concat(new string[]
			{
				Definition.filterMainPath,
				text,
				DualEnergy.dualSubPath,
				"/",
				attachment,
				"/",
				Definition.strValue
			}), value, false);
			parHandling.WriteValue(string.Concat(new string[]
			{
				Definition.filterMainPath,
				text,
				Filter.edgeSubPath,
				"/",
				attachment,
				"/",
				Definition.strValue
			}), value, false);
			for (int j = 0; j < Definition.nComparer; j++)
			{
				parHandling.WriteValue(string.Concat(new string[]
				{
					Definition.filterMainPath,
					text,
					Definition.comparerSubPath,
					(j + 1).ToString(),
					"/",
					attachment,
					"/",
					Definition.strValue
				}), value, false);
			}
			Filter.WriteToImageProcessingFiltersAI(parHandling, attachment, value, null);
			bool flag2 = parHandling.ReadPar(Definition.objectDetectionEnabled) == "1" && parHandling.ReadPar(Definition.objectDetectionInferenceEnabled) == "1";
			if (flag2)
			{
				for (int k = 0; k < Definition.nAIObjectClasses; k++)
				{
					bool flag3 = attachment == "Autolearn" || attachment == "Retrain" || attachment == "RetrainRegion" || attachment == "TrainToTrigger" || attachment == "MaxX" || attachment == "MaxY" || attachment == "MinX" || attachment == "MinY";
					if (flag3)
					{
						parHandling.WriteValue(string.Concat(new string[]
						{
							Definition.objectDetectionClassesSubPath,
							(k + 1).ToString(),
							"/",
							attachment,
							"/",
							Definition.strValue
						}), value, false);
					}
				}
			}
		}

		// Token: 0x06002A92 RID: 10898 RVA: 0x0011968C File Offset: 0x0011788C
		public static void WriteToImageProcessingFiltersAI(ParameterHandling parHandling, string attachment, string value, DBConnection dbConnection = null)
		{
			bool flag = parHandling.ReadPar(Definition.anomalyDetectionEnabled) == "1" && parHandling.ReadPar(Definition.anomalyDetectionInferenceEnabled) == "1";
			if (flag)
			{
				for (int i = 0; i < Definition.nAIPostproFilters; i++)
				{
					bool flag2 = attachment == "Autolearn" || attachment == "Retrain" || attachment == "RetrainRegion" || attachment == "TrainToTrigger" || attachment == "MaxX" || attachment == "MaxY" || attachment == "MinX" || attachment == "MinY";
					if (flag2)
					{
						parHandling.WriteValue(string.Concat(new string[]
						{
							Definition.filterMainPath,
							Definition.anomalyDetectionMain,
							Definition.anomalyDetectionInference,
							Definition.aipostproFilterSubPath,
							(i + 1).ToString(),
							"/",
							attachment,
							"/",
							Definition.strValue
						}), value, false);
					}
				}
				bool flag3 = dbConnection != null;
				if (flag3)
				{
					DetectorType detectorType = DetectorType.DT;
					dbConnection.GetDetectorType(ref detectorType);
					MachineType machineType = MachineType.MachineTypeUndefined;
					dbConnection.GetMachineType(ref machineType);
					string text = "";
					dbConnection.GetMachineSubType(ref text);
					bool flag4 = machineType == MachineType.RayconDPlus && text.Contains("HX") && detectorType != DetectorType.DTNetTDI;
					if (flag4)
					{
						for (int j = 0; j < 12; j++)
						{
							parHandling.WriteValue(string.Concat(new string[]
							{
								Definition.filterMainPath,
								Definition.comparerSubPath,
								(j + 19).ToString(),
								"/",
								attachment,
								"/",
								Definition.strValue
							}), value, false);
						}
					}
				}
			}
		}

		// Token: 0x06002A93 RID: 10899 RVA: 0x00119884 File Offset: 0x00117A84
		public static void WriteToImageProcessingFiltersBulkMode(ParameterHandling parHandling, string attachment, string value)
		{
			parHandling.WriteValue(string.Concat(new string[]
			{
				Definition.filterMainPath,
				Definition.thresholdSubPath,
				"/",
				attachment,
				"/",
				Definition.strValue
			}), value);
			parHandling.WriteValue(string.Concat(new string[]
			{
				Definition.filterMainPath,
				Definition.thresholdGlobalSubPath,
				"/",
				attachment,
				"/",
				Definition.strValue
			}), value);
			for (int i = 0; i < 8; i++)
			{
				parHandling.WriteValue(string.Concat(new string[]
				{
					Definition.filterMainPath,
					Definition.comparerSubPath,
					(i + 1).ToString(),
					"/",
					attachment,
					"/",
					Definition.strValue
				}), value);
			}
		}

		// Token: 0x06002A94 RID: 10900 RVA: 0x00119970 File Offset: 0x00117B70
		public static void SaveActualLearnedParams(ParameterHandling parHandling, int group)
		{
			Filter.listForActualLearnedParameters = new List<string>();
			string text = (group > 0) ? group.ToString() : "";
			Filter.listForActualLearnedParameters.Add(text);
			for (int i = 0; i < Definition.nObjectFinder; i++)
			{
				Filter.listForActualLearnedParameters.Add(parHandling.ReadPar(string.Concat(new string[]
				{
					Definition.filterMainPath,
					text,
					Definition.objectFindersSubPath,
					(i + 1).ToString(),
					"/Contrast/",
					Definition.strValue
				})));
			}
			for (int j = 0; j < Definition.nComparer; j++)
			{
				Filter.listForActualLearnedParameters.Add(parHandling.ReadPar(string.Concat(new string[]
				{
					Definition.filterMainPath,
					text,
					Definition.comparerSubPath,
					(j + 1).ToString(),
					"/Contrast/",
					Definition.strValue
				})));
			}
			for (int k = 1; k <= 5; k++)
			{
				Filter.listForActualLearnedParameters.Add(parHandling.ReadPar(string.Concat(new string[]
				{
					Definition.filterMainPath,
					text,
					Definition.lowDensitySubPath,
					"/Contrast",
					k.ToString(),
					"/",
					Definition.strValue
				})));
			}
			for (int l = 1; l <= 4; l++)
			{
				Filter.listForActualLearnedParameters.Add(parHandling.ReadPar(string.Concat(new string[]
				{
					Definition.filterMainPath,
					text,
					Definition.lowDensitySubPath,
					"/CountThreshold",
					l.ToString(),
					"/",
					Definition.strValue
				})));
			}
			Filter.listForActualLearnedParameters.Add(parHandling.ReadPar(string.Concat(new string[]
			{
				Definition.filterMainPath,
				text,
				Definition.thresholdSubPath,
				"/HighThreshold/",
				Definition.strValue
			})));
			Filter.listForActualLearnedParameters.Add(parHandling.ReadPar(string.Concat(new string[]
			{
				Definition.filterMainPath,
				text,
				Definition.thresholdGlobalSubPath,
				"/HighThreshold/",
				Definition.strValue
			})));
			Filter.listForActualLearnedParameters.Add(parHandling.ReadPar(string.Concat(new string[]
			{
				Definition.filterMainPath,
				text,
				Definition.sizeFilterSubPath,
				"/MaxHeight/",
				Definition.strValue
			})));
			Filter.listForActualLearnedParameters.Add(parHandling.ReadPar(string.Concat(new string[]
			{
				Definition.filterMainPath,
				text,
				Definition.sizeFilterSubPath,
				"/MaxWidth/",
				Definition.strValue
			})));
			Filter.listForActualLearnedParameters.Add(parHandling.ReadPar(string.Concat(new string[]
			{
				Definition.filterMainPath,
				text,
				Definition.sizeFilterSubPath,
				"/MinHeight/",
				Definition.strValue
			})));
			Filter.listForActualLearnedParameters.Add(parHandling.ReadPar(string.Concat(new string[]
			{
				Definition.filterMainPath,
				text,
				Definition.sizeFilterSubPath,
				"/MinWidth/",
				Definition.strValue
			})));
			Filter.listForActualLearnedParameters.Add(parHandling.ReadPar(string.Concat(new string[]
			{
				Definition.filterMainPath,
				text,
				Definition.wireFinderSubPath,
				"/ContrastDiag1/",
				Definition.strValue
			})));
			Filter.listForActualLearnedParameters.Add(parHandling.ReadPar(string.Concat(new string[]
			{
				Definition.filterMainPath,
				text,
				Definition.wireFinderSubPath,
				"/ContrastDiag2/",
				Definition.strValue
			})));
			Filter.listForActualLearnedParameters.Add(parHandling.ReadPar(string.Concat(new string[]
			{
				Definition.filterMainPath,
				text,
				Definition.wireFinderSubPath,
				"/ContrastHorizontal/",
				Definition.strValue
			})));
			Filter.listForActualLearnedParameters.Add(parHandling.ReadPar(string.Concat(new string[]
			{
				Definition.filterMainPath,
				text,
				Definition.wireFinderSubPath,
				"/ContrastVertical/",
				Definition.strValue
			})));
			Filter.listForActualLearnedParameters.Add(parHandling.ReadPar(string.Concat(new string[]
			{
				Definition.filterMainPath,
				text,
				Definition.dimensionSubPath,
				"/MaxLength/",
				Definition.strValue
			})));
			Filter.listForActualLearnedParameters.Add(parHandling.ReadPar(string.Concat(new string[]
			{
				Definition.filterMainPath,
				text,
				Definition.dimensionSubPath,
				"/MinLength/",
				Definition.strValue
			})));
			bool flag = parHandling.ReadPar(Definition.anomalyDetectionEnabled) == "1" && parHandling.ReadPar(Definition.anomalyDetectionInferenceEnabled) == "1";
			if (flag)
			{
				for (int m = 0; m < Definition.nAIPostproFilters; m++)
				{
					string str = string.Format("{0}{1}{2}{3}{4}", new object[]
					{
						Definition.filterMainPath,
						Definition.anomalyDetectionMain,
						Definition.anomalyDetectionInference,
						Definition.aipostproFilterSubPath,
						m + 1
					});
					Filter.listForActualLearnedParameters.Add(parHandling.ReadPar(str + Definition.anomalyDetectionInferenceThresholdGroupContour + "/" + Definition.strValue));
					Filter.listForActualLearnedParameters.Add(parHandling.ReadPar(str + Definition.anomalyDetectionInferenceThresholdGroupInside + "/" + Definition.strValue));
					Filter.listForActualLearnedParameters.Add(parHandling.ReadPar(str + Definition.anomalyDetectionInferenceThresholdReconErrorInside + "/" + Definition.strValue));
					Filter.listForActualLearnedParameters.Add(parHandling.ReadPar(str + Definition.anomalyDetectionInferenceThresholdReconErrorContour + "/" + Definition.strValue));
				}
			}
			bool flag2 = parHandling.ReadPar(Definition.objectDetectionEnabled) == "1" && parHandling.ReadPar(Definition.objectDetectionInferenceEnabled) == "1";
			if (flag2)
			{
				for (int n = 1; n <= Definition.nAIObjectClasses; n++)
				{
					Filter.listForActualLearnedParameters.Add(parHandling.ReadPar(Definition.objectDetectionClassesSubPath + n.ToString() + Definition.objectDetectionConfThreshold));
					Filter.listForActualLearnedParameters.Add(parHandling.ReadPar(Definition.objectDetectionClassesSubPath + n.ToString() + Definition.objectDetectionConfThresholdTTA1));
					Filter.listForActualLearnedParameters.Add(parHandling.ReadPar(Definition.objectDetectionClassesSubPath + n.ToString() + Definition.objectDetectionConfThresholdTTA2));
					Filter.listForActualLearnedParameters.Add(parHandling.ReadPar(Definition.objectDetectionClassesSubPath + n.ToString() + Definition.objectDetectionConfThresholdTTA3));
				}
			}
		}

		// Token: 0x06002A95 RID: 10901 RVA: 0x0011A084 File Offset: 0x00118284
		public static void SetPreviousLearnedParams(ParameterHandling parHandling)
		{
			int num = 0;
			string text = Filter.listForActualLearnedParameters[num];
			num++;
			for (int i = 0; i < Definition.nObjectFinder; i++)
			{
				parHandling.WriteValue(string.Concat(new string[]
				{
					Definition.filterMainPath,
					text,
					Definition.objectFindersSubPath,
					(i + 1).ToString(),
					"/Contrast/",
					Definition.strValue
				}), Filter.listForActualLearnedParameters[num], false);
				num++;
			}
			for (int j = 0; j < Definition.nComparer; j++)
			{
				parHandling.WriteValue(string.Concat(new string[]
				{
					Definition.filterMainPath,
					text,
					Definition.comparerSubPath,
					(j + 1).ToString(),
					"/Contrast/",
					Definition.strValue
				}), Filter.listForActualLearnedParameters[num], false);
				num++;
			}
			for (int k = 1; k <= 5; k++)
			{
				parHandling.WriteValue(string.Concat(new string[]
				{
					Definition.filterMainPath,
					text,
					Definition.lowDensitySubPath,
					"/Contrast",
					k.ToString(),
					"/",
					Definition.strValue
				}), Filter.listForActualLearnedParameters[num], false);
				num++;
			}
			for (int l = 1; l <= 4; l++)
			{
				parHandling.WriteValue(string.Concat(new string[]
				{
					Definition.filterMainPath,
					text,
					Definition.lowDensitySubPath,
					"/CountThreshold",
					l.ToString(),
					"/",
					Definition.strValue
				}), Filter.listForActualLearnedParameters[num], false);
				num++;
			}
			parHandling.WriteValue(string.Concat(new string[]
			{
				Definition.filterMainPath,
				text,
				Definition.thresholdSubPath,
				"/HighThreshold/",
				Definition.strValue
			}), Filter.listForActualLearnedParameters[num], false);
			parHandling.WriteValue(string.Concat(new string[]
			{
				Definition.filterMainPath,
				text,
				Definition.thresholdGlobalSubPath,
				"/HighThreshold/",
				Definition.strValue
			}), Filter.listForActualLearnedParameters[num + 1], false);
			parHandling.WriteValue(string.Concat(new string[]
			{
				Definition.filterMainPath,
				text,
				Definition.sizeFilterSubPath,
				"/MaxHeight/",
				Definition.strValue
			}), Filter.listForActualLearnedParameters[num + 2], false);
			parHandling.WriteValue(string.Concat(new string[]
			{
				Definition.filterMainPath,
				text,
				Definition.sizeFilterSubPath,
				"/MaxWidth/",
				Definition.strValue
			}), Filter.listForActualLearnedParameters[num + 3], false);
			parHandling.WriteValue(string.Concat(new string[]
			{
				Definition.filterMainPath,
				text,
				Definition.sizeFilterSubPath,
				"/MinHeight/",
				Definition.strValue
			}), Filter.listForActualLearnedParameters[num + 4], false);
			parHandling.WriteValue(string.Concat(new string[]
			{
				Definition.filterMainPath,
				text,
				Definition.sizeFilterSubPath,
				"/MinWidth/",
				Definition.strValue
			}), Filter.listForActualLearnedParameters[num + 5], false);
			parHandling.WriteValue(string.Concat(new string[]
			{
				Definition.filterMainPath,
				text,
				Definition.wireFinderSubPath,
				"/ContrastDiag1/",
				Definition.strValue
			}), Filter.listForActualLearnedParameters[num + 6], false);
			parHandling.WriteValue(string.Concat(new string[]
			{
				Definition.filterMainPath,
				text,
				Definition.wireFinderSubPath,
				"/ContrastDiag2/",
				Definition.strValue
			}), Filter.listForActualLearnedParameters[num + 7], false);
			parHandling.WriteValue(string.Concat(new string[]
			{
				Definition.filterMainPath,
				text,
				Definition.wireFinderSubPath,
				"/ContrastHorizontal/",
				Definition.strValue
			}), Filter.listForActualLearnedParameters[num + 8], false);
			parHandling.WriteValue(string.Concat(new string[]
			{
				Definition.filterMainPath,
				text,
				Definition.wireFinderSubPath,
				"/ContrastVertical/",
				Definition.strValue
			}), Filter.listForActualLearnedParameters[num + 9], false);
			parHandling.WriteValue(string.Concat(new string[]
			{
				Definition.filterMainPath,
				text,
				Definition.dimensionSubPath,
				"/MaxLength/",
				Definition.strValue
			}), Filter.listForActualLearnedParameters[num + 10], false);
			parHandling.WriteValue(string.Concat(new string[]
			{
				Definition.filterMainPath,
				text,
				Definition.dimensionSubPath,
				"/MinLength/",
				Definition.strValue
			}), Filter.listForActualLearnedParameters[num + 11], false);
			num += 12;
			bool flag = parHandling.ReadPar(Definition.anomalyDetectionEnabled) == "1" && parHandling.ReadPar(Definition.anomalyDetectionInferenceEnabled) == "1";
			if (flag)
			{
				for (int m = 0; m < Definition.nAIPostproFilters; m++)
				{
					string str = string.Format("{0}{1}{2}{3}{4}", new object[]
					{
						Definition.filterMainPath,
						Definition.anomalyDetectionMain,
						Definition.anomalyDetectionInference,
						Definition.aipostproFilterSubPath,
						m + 1
					});
					parHandling.WriteValue(str + Definition.anomalyDetectionInferenceThresholdGroupContour + "/" + Definition.strValue, Filter.listForActualLearnedParameters[num], false);
					parHandling.WriteValue(str + Definition.anomalyDetectionInferenceThresholdGroupInside + "/" + Definition.strValue, Filter.listForActualLearnedParameters[num + 1], false);
					parHandling.WriteValue(str + Definition.anomalyDetectionInferenceThresholdReconErrorInside + "/" + Definition.strValue, Filter.listForActualLearnedParameters[num + 2], false);
					parHandling.WriteValue(str + Definition.anomalyDetectionInferenceThresholdReconErrorContour + "/" + Definition.strValue, Filter.listForActualLearnedParameters[num + 3], false);
					num += 4;
				}
			}
			bool flag2 = parHandling.ReadPar(Definition.objectDetectionEnabled) == "1" && parHandling.ReadPar(Definition.objectDetectionInferenceEnabled) == "1";
			if (flag2)
			{
				for (int n = 1; n <= Definition.nAIObjectClasses; n++)
				{
					parHandling.WriteValue(Definition.objectDetectionClassesSubPath + n.ToString() + Definition.objectDetectionConfThreshold, Filter.listForActualLearnedParameters[num], false);
					parHandling.WriteValue(Definition.objectDetectionClassesSubPath + n.ToString() + Definition.objectDetectionConfThresholdTTA1, Filter.listForActualLearnedParameters[num + 1], false);
					parHandling.WriteValue(Definition.objectDetectionClassesSubPath + n.ToString() + Definition.objectDetectionConfThresholdTTA2, Filter.listForActualLearnedParameters[num + 2], false);
					parHandling.WriteValue(Definition.objectDetectionClassesSubPath + n.ToString() + Definition.objectDetectionConfThresholdTTA3, Filter.listForActualLearnedParameters[num + 3], false);
					num += 4;
				}
			}
		}

		// Token: 0x06002A96 RID: 10902 RVA: 0x0011A7F4 File Offset: 0x001189F4
		public static void EnableComparerFilter(ParameterHandling parHandling, bool enablecomp, int group)
		{
			string text = (group > 0) ? group.ToString() : "";
			bool flag = false;
			bool flag2 = parHandling.ReadPar(Definition.detectorSesotecPixelSizePath) == "0.4";
			if (flag2)
			{
				flag = true;
			}
			for (int i = 1; i <= Definition.nObjectFinder; i++)
			{
				if (enablecomp)
				{
					parHandling.WriteValue(string.Concat(new string[]
					{
						Definition.filterMainPath,
						text,
						Definition.objectFindersSubPath,
						i.ToString(),
						"/Enabled/Value"
					}), "0");
				}
				else
				{
					bool flag3 = i <= 32;
					if (flag3)
					{
						parHandling.WriteValue(string.Concat(new string[]
						{
							Definition.filterMainPath,
							text,
							Definition.objectFindersSubPath,
							i.ToString(),
							"/Enabled/Value"
						}), "1");
					}
					else
					{
						bool flag4 = flag;
						if (flag4)
						{
							parHandling.WriteValue(string.Concat(new string[]
							{
								Definition.filterMainPath,
								text,
								Definition.objectFindersSubPath,
								i.ToString(),
								"/Enabled/Value"
							}), "1");
						}
						else
						{
							parHandling.WriteValue(string.Concat(new string[]
							{
								Definition.filterMainPath,
								text,
								Definition.objectFindersSubPath,
								i.ToString(),
								"/Enabled/Value"
							}), "0");
						}
					}
				}
			}
			int num = enablecomp ? 8 : Definition.nComparer;
			for (int j = 0; j < num; j++)
			{
				parHandling.WriteValue(string.Concat(new string[]
				{
					Definition.filterMainPath,
					text,
					"/comparer/comparer",
					(j + 1).ToString(),
					"/Enabled/Value"
				}), enablecomp ? "1" : "0");
			}
		}

		// Token: 0x06002A97 RID: 10903 RVA: 0x0011A9F8 File Offset: 0x00118BF8
		public static void SwitchLowDensityParameter(ParameterHandling parHandling, bool is04detector, int group)
		{
			string text = (group > 0) ? group.ToString() : "";
			int[] array = new int[4];
			bool flag = parHandling.ReadPar(Definition.filterMainPath + text + Definition.lowDensitySubPath + "/Enabled/Value") == "1";
			if (flag)
			{
				int value;
				int value2;
				if (is04detector)
				{
					value = 12;
					value2 = 12;
					array[0] = 3;
					array[1] = 100;
					array[2] = 500;
					array[3] = 2000;
				}
				else
				{
					value = 4;
					value2 = 5;
					array[0] = 3;
					array[1] = 20;
					array[2] = 100;
					array[3] = 500;
				}
				parHandling.WriteValue(Definition.filterMainPath + text + Definition.lowDensitySubPath + "/FilterSizeStep/Value", Convert.ToString(value));
				parHandling.WriteValue(Definition.filterMainPath + text + Definition.lowDensitySubPath + "/SizeStep/Value", Convert.ToString(value2));
				for (int i = 0; i < 4; i++)
				{
					parHandling.WriteValue(string.Concat(new string[]
					{
						Definition.filterMainPath,
						text,
						Definition.lowDensitySubPath,
						"/BorderCounter",
						(i + 1).ToString(),
						"/Value"
					}), Convert.ToString(array[i]));
				}
			}
		}

		// Token: 0x06002A98 RID: 10904 RVA: 0x0011AB48 File Offset: 0x00118D48
		public static void UpdateImageProcessingFilterPaths(DBConnection dbConnection, ParameterHandling parHandling, int group, ProductManager productManager, bool sendMaterialCurveDual)
		{
			string str = (group > 0) ? group.ToString() : "";
			bool connectionEstablished = parHandling.ConnectionEstablished;
			if (connectionEstablished)
			{
				List<string> list = new List<string>();
				List<string> list2 = new List<string>();
				bool flag = group == 0;
				if (flag)
				{
					Filter.UpdateMainTable(dbConnection, parHandling, list2, list);
				}
				else
				{
					Filter.UpdateSubTable(dbConnection, parHandling, list2, list, group);
				}
				productManager.UpdateAssignedProductsFilterPaths(list2, list, group);
				if (sendMaterialCurveDual)
				{
					parHandling.WriteValue(Definition.filterMainPath + str + DualEnergy.requestCurveSubPath, "2", false);
				}
			}
		}

		// Token: 0x06002A99 RID: 10905 RVA: 0x0011ABD8 File Offset: 0x00118DD8
		public static void UpdateImageProcessingFilterPathsForRegionProd(DBConnection dbConnection, ParameterHandling parHandling, int[] groups, ProductManager productManager)
		{
			bool flag = parHandling.ConnectionEstablished && groups.Length != 0;
			if (flag)
			{
				List<string>[] array = new List<string>[groups.Length];
				List<string>[] array2 = new List<string>[groups.Length];
				for (int i = 0; i < groups.Length; i++)
				{
					array[i] = new List<string>();
					array2[i] = new List<string>();
				}
				Filter.UpdateSubTables(dbConnection, parHandling, array, array2, groups);
				for (int j = 0; j < groups.Length; j++)
				{
					productManager.UpdateAssignedProductsFilterPaths(array[j], array2[j], groups[j]);
				}
			}
		}

		// Token: 0x06002A9A RID: 10906 RVA: 0x0011AC6C File Offset: 0x00118E6C
		private static void UpdateMainTable(DBConnection dbConnection, ParameterHandling parHandling, List<string> paths, List<string> values)
		{
			dbConnection.GetCurrentProductPaths(paths);
			for (int i = 0; i < paths.Count; i++)
			{
				bool flag = paths[i].StartsWith(Definition.filterMainPath + Definition.objectFindersSubPath) || paths[i].StartsWith(Definition.filterMainPath + Definition.thresholdSubPath) || paths[i].StartsWith(Definition.filterMainPath + Definition.thresholdGlobalSubPath) || paths[i].StartsWith(Definition.filterMainPath + Definition.sizeFilterSubPath) || paths[i].StartsWith(Definition.filterMainPath + Definition.wireFinderSubPath) || paths[i].StartsWith(Definition.filterMainPath + ObjectCounter.objectCounterSubPath) || paths[i].StartsWith(Definition.filterMainPath + Definition.dimensionSubPath) || paths[i].StartsWith(Definition.filterMainPath + Definition.lowDensitySubPath) || paths[i].StartsWith(Definition.filterMainPath + Definition.comparerSubPath) || paths[i].StartsWith(Definition.filterMainPath + DualEnergy.dualSubPath) || paths[i].StartsWith(Definition.filterMainPath + Filter.edgeSubPath);
				if (flag)
				{
					values.Add(parHandling.ReadPar(paths[i]));
				}
				else
				{
					paths.Remove(paths[i]);
					i--;
				}
			}
			bool flag2 = !dbConnection.UpdateCurrentProductTable(paths, values);
			if (flag2)
			{
				Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(delegate()
				{
					Dialog.Show("Error", "Product table couldn't be updated");
				}));
			}
		}

		// Token: 0x06002A9B RID: 10907 RVA: 0x0011AE64 File Offset: 0x00119064
		private static void UpdateSubTable(DBConnection dbConnection, ParameterHandling parHandling, List<string> paths, List<string> values, int group)
		{
			string str = null;
			dbConnection.GetCurrentProductTableName(ref str);
			dbConnection.GetCurrentSubProductEntries(group, paths, null);
			for (int i = 0; i < paths.Count; i++)
			{
				bool flag = !paths[i].StartsWith(Definition.filterMainPath);
				if (flag)
				{
					paths.RemoveAt(i);
					i--;
				}
				else
				{
					values.Add(parHandling.ReadPar(paths[i]));
				}
			}
			dbConnection.UpdateTable(str + Definition.subTableIdentifier.ToString() + group.ToString(), paths, values);
		}

		// Token: 0x06002A9C RID: 10908 RVA: 0x0011AF00 File Offset: 0x00119100
		private static void UpdateSubTables(DBConnection dbConnection, ParameterHandling parHandling, List<string>[] paths, List<string>[] values, int[] groups)
		{
			List<string> list = new List<string>();
			string str = null;
			dbConnection.GetCurrentProductTableName(ref str);
			for (int i = 0; i < groups.Length; i++)
			{
				int group = groups[i];
				List<string> list2 = paths[i];
				List<string> list3 = values[i];
				list.Add(str + Definition.subTableIdentifier.ToString() + group.ToString());
				dbConnection.GetCurrentSubProductEntries(group, list2, null);
				for (int j = 0; j < list2.Count; j++)
				{
					bool flag = !list2[j].StartsWith(Definition.filterMainPath);
					if (flag)
					{
						list2.RemoveAt(j);
						j--;
					}
					else
					{
						list3.Add(parHandling.ReadPar(list2[j]));
					}
				}
			}
			dbConnection.UpdateTables(list.ToArray(), paths, values);
		}

		// Token: 0x06002A9D RID: 10909 RVA: 0x0011AFEC File Offset: 0x001191EC
		private void ShowImpurityFinderSettings(object obj)
		{
			CtrlImpurityFinderSettings usrControl = new CtrlImpurityFinderSettings(this.dbConnection, this.parHandling, this.imageGroup, this.bshowDialogWindows, this.productManager, this.accessManager);
			Subwindow.Show(usrControl, SubwindowButtons.Back);
		}

		// Token: 0x06002A9E RID: 10910 RVA: 0x0011B02C File Offset: 0x0011922C
		private void ShowAutokVSettings(object obj)
		{
			CtrlAutokVSettings usrControl = new CtrlAutokVSettings(this.dbConnection, this.parHandling, this.imageGroup, this.bshowDialogWindows, this.productManager, this.accessManager, this.machineState, this.isOnlyManualKvActive, this.testcardSimService);
			Subwindow.Show(usrControl, SubwindowButtons.Back);
		}

		// Token: 0x06002A9F RID: 10911 RVA: 0x0011B080 File Offset: 0x00119280
		public static void UpdateAnomalyDetectionInferenceThresholds(DBConnection dbConnection, ParameterHandling parHandling)
		{
			bool connectionEstablished = parHandling.ConnectionEstablished;
			if (connectionEstablished)
			{
				List<string> list = new List<string>();
				List<string> list2 = new List<string>();
				string tableName = null;
				dbConnection.GetCurrentProductTableNameAI(ref tableName);
				bool flag = TableOperations.TableExists(dbConnection.Connection, tableName);
				if (flag)
				{
					string pattern = string.Concat(new string[]
					{
						"^",
						Definition.filterMainPath,
						Definition.anomalyDetectionMain,
						Definition.anomalyDetectionInference,
						Definition.aipostproFilterSubPath,
						"\\d+/Threshold.*$"
					});
					dbConnection.GetProductPaths(tableName, list2);
					for (int i = 0; i < list2.Count; i++)
					{
						bool flag2 = Regex.IsMatch(list2[i], pattern);
						if (flag2)
						{
							list.Add(parHandling.ReadPar(list2[i]));
						}
						else
						{
							list2.Remove(list2[i]);
							i--;
						}
					}
					bool flag3 = !dbConnection.UpdateTable(tableName, list2, list);
					if (flag3)
					{
						Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(delegate()
						{
							Dialog.Show("Error", "AI product table couldn't be updated");
						}));
					}
				}
			}
		}

		// Token: 0x06002AA0 RID: 10912 RVA: 0x0011B1B8 File Offset: 0x001193B8
		public static void UpdateAnomalyDetectionPathesAfterTraining(DBConnection dbConnection, ParameterHandling parHandling)
		{
			bool connectionEstablished = parHandling.ConnectionEstablished;
			if (connectionEstablished)
			{
				string[] source = new string[]
				{
					Definition.anomalyDetectionInferenceSize,
					"/filter/anomalyDetection/inference/BackgroundThreshold/Value",
					"/filter/anomalyDetection/inference/Gamma/Value",
					"/filter/anomalyDetection/inference/IgnoreBorderPixels/Value",
					"/filter/anomalyDetection/inference/SharpeningFactor/Value"
				};
				List<string> list = new List<string>();
				List<string> list2 = new List<string>();
				string text = null;
				string text2 = parHandling.ReadPar(Definition.anomalyDetectionTrainModelID);
				dbConnection.GetAppTableNameWithNumber(text2, ref text);
				string tableName = text.Insert("AppDefault".Length, "AI");
				bool flag = TableOperations.TableExists(dbConnection.Connection, tableName);
				if (flag)
				{
					string a = parHandling.ReadPar(Definition.anomalyDetectionTrainCurrentEpoch);
					dbConnection.GetProductPaths(tableName, list2);
					for (int i = 0; i < list2.Count; i++)
					{
						bool flag2 = list2[i].StartsWith("/filter/anomalyDetection/train");
						if (flag2)
						{
							list.Add(parHandling.ReadPar(list2[i]));
						}
						else
						{
							bool flag3 = source.Contains(list2[i]) && a == "0";
							if (flag3)
							{
								string path = list2[i].Replace("inference", "train");
								string text3 = parHandling.ReadPar(path);
								list.Add(text3);
								bool flag4 = text2 == parHandling.ReadPar(Definition.anomalyDetectionInferenceModelID);
								if (flag4)
								{
									parHandling.WriteValue(list2[i], text3);
								}
							}
							else
							{
								bool flag5 = list2[i].EndsWith("inference/Enabled/Value") && a == "0";
								if (flag5)
								{
									list.Add("0");
									bool flag6 = text2 == parHandling.ReadPar(Definition.anomalyDetectionInferenceModelID);
									if (flag6)
									{
										parHandling.WriteValue(list2[i], "0");
									}
								}
								else
								{
									list2.Remove(list2[i]);
									i--;
								}
							}
						}
					}
					bool flag7 = !dbConnection.UpdateTable(tableName, list2, list);
					if (flag7)
					{
						Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(delegate()
						{
							Dialog.Show("Error", "AI product table couldn't be updated");
						}));
					}
				}
			}
		}

		// Token: 0x06002AA1 RID: 10913 RVA: 0x0011B410 File Offset: 0x00119610
		public static void UpdateObjectDetectionInferenceThresholds(DBConnection dbConnection, ParameterHandling parHandling)
		{
			bool connectionEstablished = parHandling.ConnectionEstablished;
			if (connectionEstablished)
			{
				List<string> list = new List<string>();
				List<string> list2 = new List<string>();
				string tableName = null;
				dbConnection.GetCurrentProductTableNameAI(ref tableName);
				bool flag = TableOperations.TableExists(dbConnection.Connection, tableName);
				if (flag)
				{
					string pattern = "^" + Definition.objectDetectionClassesSubPath + "\\d+/ConfThreshold.*$";
					dbConnection.GetProductPaths(tableName, list2);
					for (int i = 0; i < list2.Count; i++)
					{
						bool flag2 = Regex.IsMatch(list2[i], pattern);
						if (flag2)
						{
							list.Add(parHandling.ReadPar(list2[i]));
						}
						else
						{
							list2.Remove(list2[i]);
							i--;
						}
					}
					bool flag3 = !dbConnection.UpdateTable(tableName, list2, list);
					if (flag3)
					{
						Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(delegate()
						{
							Dialog.Show("Error", "AI product table couldn't be updated");
						}));
					}
				}
			}
		}

		// Token: 0x06002AA2 RID: 10914 RVA: 0x0011B524 File Offset: 0x00119724
		public static bool CountAndUnmaskForContEnabled(ParameterHandling parHandling)
		{
			bool flag = parHandling.ReadPar(Definition.filterMainPath + ObjectCounter.enabledSubPath) == "1";
			bool flag2 = parHandling.ReadPar(Definition.filterMainPath + ObjectCounter.unsmaskForContPath) == "1";
			bool flag3 = flag && flag2;
			bool result;
			if (flag3)
			{
				result = true;
			}
			else
			{
				bool flag4 = parHandling.ReadPar(Definition.objectDetectionEnabled) == "1" && parHandling.ReadPar(Definition.objectDetectionInferenceEnabled) == "1";
				bool flag5 = flag4;
				if (flag5)
				{
					for (int i = 0; i < Definition.nAIObjectClasses; i++)
					{
						string path = string.Format("{0}{1}{2}", Definition.objectDetectionClassesSubPath, i + 1, Definition.objectDetectionClassEnabled);
						string path2 = string.Format("{0}{1}{2}", Definition.objectDetectionClassesSubPath, i + 1, Definition.objectDetectionUnmaskForConEnabled);
						bool flag6 = parHandling.ReadPar(path) == "1" && parHandling.ReadPar(path2) == "1";
						if (flag6)
						{
							return true;
						}
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x06002AA3 RID: 10915 RVA: 0x0011B658 File Offset: 0x00119858
		public static void SaveDefaultParamsForAutolearn(ParameterHandling parHandling, int group, DBConnection dbConnection)
		{
			string item = null;
			Filter.listForActualLearnedParameters = new List<string>();
			string text = (group > 0) ? group.ToString() : "";
			Filter.listForActualLearnedParameters.Add(text);
			for (int i = 0; i < Definition.nObjectFinder; i++)
			{
				Filter.listForActualLearnedParameters.Add("0");
			}
			for (int j = 0; j < Definition.nComparer; j++)
			{
				Filter.listForActualLearnedParameters.Add("0");
			}
			for (int k = 1; k <= 5; k++)
			{
				Filter.listForActualLearnedParameters.Add(parHandling.ReadPar(string.Concat(new string[]
				{
					Definition.filterMainPath,
					text,
					Definition.lowDensitySubPath,
					"/Contrast",
					k.ToString(),
					"/",
					Definition.strValue
				})));
			}
			for (int l = 1; l <= 4; l++)
			{
				Filter.listForActualLearnedParameters.Add(parHandling.ReadPar(string.Concat(new string[]
				{
					Definition.filterMainPath,
					text,
					Definition.lowDensitySubPath,
					"/CountThreshold",
					l.ToString(),
					"/",
					Definition.strValue
				})));
			}
			Filter.listForActualLearnedParameters.Add("32767");
			Filter.listForActualLearnedParameters.Add("32767");
			Filter.listForActualLearnedParameters.Add(parHandling.ReadPar(string.Concat(new string[]
			{
				Definition.filterMainPath,
				text,
				Definition.sizeFilterSubPath,
				"/MaxHeight/",
				Definition.strValue
			})));
			Filter.listForActualLearnedParameters.Add(parHandling.ReadPar(string.Concat(new string[]
			{
				Definition.filterMainPath,
				text,
				Definition.sizeFilterSubPath,
				"/MaxWidth/",
				Definition.strValue
			})));
			Filter.listForActualLearnedParameters.Add(parHandling.ReadPar(string.Concat(new string[]
			{
				Definition.filterMainPath,
				text,
				Definition.sizeFilterSubPath,
				"/MinHeight/",
				Definition.strValue
			})));
			Filter.listForActualLearnedParameters.Add(parHandling.ReadPar(string.Concat(new string[]
			{
				Definition.filterMainPath,
				text,
				Definition.sizeFilterSubPath,
				"/MinWidth/",
				Definition.strValue
			})));
			Filter.listForActualLearnedParameters.Add(parHandling.ReadPar(string.Concat(new string[]
			{
				Definition.filterMainPath,
				text,
				Definition.wireFinderSubPath,
				"/ContrastDiag1/",
				Definition.strValue
			})));
			Filter.listForActualLearnedParameters.Add(parHandling.ReadPar(string.Concat(new string[]
			{
				Definition.filterMainPath,
				text,
				Definition.wireFinderSubPath,
				"/ContrastDiag2/",
				Definition.strValue
			})));
			Filter.listForActualLearnedParameters.Add(parHandling.ReadPar(string.Concat(new string[]
			{
				Definition.filterMainPath,
				text,
				Definition.wireFinderSubPath,
				"/ContrastHorizontal/",
				Definition.strValue
			})));
			Filter.listForActualLearnedParameters.Add(parHandling.ReadPar(string.Concat(new string[]
			{
				Definition.filterMainPath,
				text,
				Definition.wireFinderSubPath,
				"/ContrastVertical/",
				Definition.strValue
			})));
			Filter.listForActualLearnedParameters.Add(parHandling.ReadPar(string.Concat(new string[]
			{
				Definition.filterMainPath,
				text,
				Definition.dimensionSubPath,
				"/MaxLength/",
				Definition.strValue
			})));
			Filter.listForActualLearnedParameters.Add(parHandling.ReadPar(string.Concat(new string[]
			{
				Definition.filterMainPath,
				text,
				Definition.dimensionSubPath,
				"/MinLength/",
				Definition.strValue
			})));
			bool flag = parHandling.ReadPar(Definition.anomalyDetectionEnabled) == "1" && parHandling.ReadPar(Definition.anomalyDetectionInferenceEnabled) == "1";
			if (flag)
			{
				for (int m = 0; m < Definition.nAIPostproFilters; m++)
				{
					string item2 = null;
					string item3 = null;
					string item4 = null;
					string item5 = null;
					string str = string.Format("{0}{1}{2}{3}{4}", new object[]
					{
						Definition.filterMainPath,
						Definition.anomalyDetectionMain,
						Definition.anomalyDetectionInference,
						Definition.aipostproFilterSubPath,
						m + 1
					});
					dbConnection.GetParameterValue(str + Definition.anomalyDetectionInferenceThresholdGroupContour + "/" + Definition.strValue, DBConnection.defaultApplicationAI, ref item2);
					dbConnection.GetParameterValue(str + Definition.anomalyDetectionInferenceThresholdGroupInside + "/" + Definition.strValue, DBConnection.defaultApplicationAI, ref item3);
					dbConnection.GetParameterValue(str + Definition.anomalyDetectionInferenceThresholdReconErrorInside + "/" + Definition.strValue, DBConnection.defaultApplicationAI, ref item4);
					dbConnection.GetParameterValue(str + Definition.anomalyDetectionInferenceThresholdReconErrorContour + "/" + Definition.strValue, DBConnection.defaultApplicationAI, ref item5);
					Filter.listForActualLearnedParameters.Add(item2);
					Filter.listForActualLearnedParameters.Add(item3);
					Filter.listForActualLearnedParameters.Add(item4);
					Filter.listForActualLearnedParameters.Add(item5);
				}
			}
			bool flag2 = parHandling.ReadPar(Definition.objectDetectionEnabled) == "1" && parHandling.ReadPar(Definition.objectDetectionInferenceEnabled) == "1";
			if (flag2)
			{
				for (int n = 1; n <= Definition.nAIObjectClasses; n++)
				{
					bool flag3 = parHandling.ReadPar(Definition.objectDetectionClassesSubPath + n.ToString() + "/DetectionType/" + Definition.strValue) == "0";
					if (flag3)
					{
						Filter.listForActualLearnedParameters.Add(parHandling.ReadPar(Definition.objectDetectionClassesSubPath + n.ToString() + Definition.objectDetectionConfThreshold));
						Filter.listForActualLearnedParameters.Add(parHandling.ReadPar(Definition.objectDetectionClassesSubPath + n.ToString() + Definition.objectDetectionConfThresholdTTA1));
						Filter.listForActualLearnedParameters.Add(parHandling.ReadPar(Definition.objectDetectionClassesSubPath + n.ToString() + Definition.objectDetectionConfThresholdTTA2));
						Filter.listForActualLearnedParameters.Add(parHandling.ReadPar(Definition.objectDetectionClassesSubPath + n.ToString() + Definition.objectDetectionConfThresholdTTA3));
					}
					else
					{
						item = null;
						string path = Definition.objectDetectionClassesSubPath + n.ToString() + Definition.objectDetectionConfThreshold;
						dbConnection.GetParameterValue(path, DBConnection.defaultApplicationAI, ref item);
						Filter.listForActualLearnedParameters.Add(item);
						item = null;
						path = Definition.objectDetectionClassesSubPath + n.ToString() + Definition.objectDetectionConfThresholdTTA1;
						dbConnection.GetParameterValue(path, DBConnection.defaultApplicationAI, ref item);
						Filter.listForActualLearnedParameters.Add(item);
						item = null;
						path = Definition.objectDetectionClassesSubPath + n.ToString() + Definition.objectDetectionConfThresholdTTA2;
						dbConnection.GetParameterValue(path, DBConnection.defaultApplicationAI, ref item);
						Filter.listForActualLearnedParameters.Add(item);
						item = null;
						path = Definition.objectDetectionClassesSubPath + n.ToString() + Definition.objectDetectionConfThresholdTTA3;
						dbConnection.GetParameterValue(path, DBConnection.defaultApplicationAI, ref item);
						Filter.listForActualLearnedParameters.Add(item);
					}
				}
			}
		}

		// Token: 0x04001A25 RID: 6693
		private BuiltInPriviledges rightEdgeFilterEnabled = BuiltInPriviledges.Operator1;

		// Token: 0x04001A26 RID: 6694
		private BuiltInPriviledges rightWeightEnabled = BuiltInPriviledges.Operator1;

		// Token: 0x04001A27 RID: 6695
		private BuiltInPriviledges rightCountEnabled = BuiltInPriviledges.Operator1;

		// Token: 0x04001A28 RID: 6696
		private BuiltInPriviledges rightChangeMaxProductLength = BuiltInPriviledges.Operator1;

		// Token: 0x04001A29 RID: 6697
		public static readonly BuiltInPriviledges rightContaminationCountEnabled = BuiltInPriviledges.Service;

		// Token: 0x04001A2A RID: 6698
		public static readonly string edgeSubPath = "/edge";

		// Token: 0x04001A2B RID: 6699
		public static readonly string edgeEnabledSubPath = Filter.edgeSubPath + "/Enabled/Value";

		// Token: 0x04001A2C RID: 6700
		private const string canEnabledSubPath = "/can/Enabled/Value";

		// Token: 0x04001A2D RID: 6701
		public static readonly string leftPixelSubPath = Filter.edgeSubPath + "/NLeftPixel/Value";

		// Token: 0x04001A2E RID: 6702
		public static readonly string rightPixelSubPath = Filter.edgeSubPath + "/NRightPixel/Value";

		// Token: 0x04001A2F RID: 6703
		public static readonly string topPixelSubPath = Filter.edgeSubPath + "/NTopPixel/Value";

		// Token: 0x04001A30 RID: 6704
		public static readonly string bottomPixelSubPath = Filter.edgeSubPath + "/NBottomPixel/Value";

		// Token: 0x04001A31 RID: 6705
		public static readonly string edgeDebugSubPath = Filter.edgeSubPath + "/Debug/Value";

		// Token: 0x04001A32 RID: 6706
		public static readonly string edgeThresholdSubPath = Filter.edgeSubPath + "/Threshold/Value";

		// Token: 0x04001A33 RID: 6707
		public static readonly string edgeOperationSubPath = Filter.edgeSubPath + "/FilterOperation/Value";

		// Token: 0x04001A34 RID: 6708
		public static readonly string edgeMultipleObjectsSubPath = Filter.edgeSubPath + "/MultipleObjects/Value";

		// Token: 0x04001A35 RID: 6709
		public static readonly string edgeAutoEdgeFilterSubPath = Filter.edgeSubPath + "/autoDetect/Enabled/Value";

		// Token: 0x04001A36 RID: 6710
		private bool bEdgeFilterEnabled;

		// Token: 0x04001A37 RID: 6711
		private bool bCanFilterEnabled;

		// Token: 0x04001A38 RID: 6712
		private bool bWeightEnabled;

		// Token: 0x04001A39 RID: 6713
		private bool bCountEnabled;

		// Token: 0x04001A3A RID: 6714
		private bool bContaminationCountEnabled;

		// Token: 0x04001A3B RID: 6715
		private bool bSizeFilterEnabled;

		// Token: 0x04001A3C RID: 6716
		private bool bBtnSizeControlVisible;

		// Token: 0x04001A3D RID: 6717
		private bool bSizeFilterForTrainingVisible;

		// Token: 0x04001A3E RID: 6718
		public static bool SizeControlAvailable;

		// Token: 0x04001A3F RID: 6719
		private int nLeftPixel;

		// Token: 0x04001A40 RID: 6720
		private int nRightPixel;

		// Token: 0x04001A41 RID: 6721
		private int nTopPixel;

		// Token: 0x04001A42 RID: 6722
		private int nBottomPixel;

		// Token: 0x04001A43 RID: 6723
		private AccessRightsManager accessManager;

		// Token: 0x04001A44 RID: 6724
		private ParameterHandling parHandling;

		// Token: 0x04001A45 RID: 6725
		private DBConnection dbConnection;

		// Token: 0x04001A46 RID: 6726
		private NumKeyboard numKeyboard;

		// Token: 0x04001A47 RID: 6727
		private ImageDataTransfer imageTransfer;

		// Token: 0x04001A48 RID: 6728
		private MachineState machineState;

		// Token: 0x04001A49 RID: 6729
		private bool initialized = false;

		// Token: 0x04001A4A RID: 6730
		private int imageGroup;

		// Token: 0x04001A4B RID: 6731
		private string strImageGroup;

		// Token: 0x04001A4C RID: 6732
		private ProductManager productManager;

		// Token: 0x04001A4D RID: 6733
		private string additionalLogbookText = "";

		// Token: 0x04001A4E RID: 6734
		private StoreImageData imageArchiver;

		// Token: 0x04001A4F RID: 6735
		private bool bAutoEdgeFilterEnabled;

		// Token: 0x04001A50 RID: 6736
		private bool bIsEdgeFilterVisble;

		// Token: 0x04001A51 RID: 6737
		private bool isOnlyManualKvActive;

		// Token: 0x04001A52 RID: 6738
		private bool bshowDialogWindows;

		// Token: 0x04001A53 RID: 6739
		private bool multiRegionEnable;

		// Token: 0x04001A54 RID: 6740
		private bool multiLaneProductEnable;

		// Token: 0x04001A55 RID: 6741
		private bool multiProductEnable;

		// Token: 0x04001A5D RID: 6749
		private bool isHighThroughputEnabled;

		// Token: 0x04001A5E RID: 6750
		private bool anomalyDetectionTrainingOnOff;

		// Token: 0x04001A64 RID: 6756
		private bool isBulkOptionVisible;

		// Token: 0x04001A65 RID: 6757
		private bool bulkContImagesShown;

		// Token: 0x04001A8A RID: 6794
		private static List<string> listForActualLearnedParameters;

		// Token: 0x04001A8B RID: 6795
		private TestcardSimService testcardSimService;

		// Token: 0x04001A8C RID: 6796
		private AuditCheckSettings auditSettings;
	}
}
