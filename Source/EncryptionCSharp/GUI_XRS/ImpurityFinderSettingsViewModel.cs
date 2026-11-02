using System;
using System.ComponentModel;
using System.Diagnostics;
using Common;
using DatabaseConnection;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x020000B3 RID: 179
	public class ImpurityFinderSettingsViewModel : INotifyPropertyChanged
	{
		// Token: 0x14000059 RID: 89
		// (add) Token: 0x06001201 RID: 4609 RVA: 0x0006C768 File Offset: 0x0006A968
		// (remove) Token: 0x06001202 RID: 4610 RVA: 0x0006C7A0 File Offset: 0x0006A9A0
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x17000578 RID: 1400
		// (get) Token: 0x06001203 RID: 4611 RVA: 0x0006C7D8 File Offset: 0x0006A9D8
		// (set) Token: 0x06001204 RID: 4612 RVA: 0x0006C7F0 File Offset: 0x0006A9F0
		public ImpurityFinderType ImpurityFinderType
		{
			get
			{
				return this.impurityFinderType;
			}
			set
			{
				bool flag = this.impurityFinderType != value;
				if (flag)
				{
					bool flag2 = !this.showDialogWindows || Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtNewAutotrainRequired"), DialogStyle.Warning, DialogButtons.OKCancel) == Dialog.DialogResults.Ok;
					if (flag2)
					{
						this.SetNewAlgorithmParameter(value);
						this.impurityFinderType = value;
						this.OnPropertyChanged("ImpurityFinderType");
					}
				}
			}
		}

		// Token: 0x17000579 RID: 1401
		// (get) Token: 0x06001205 RID: 4613 RVA: 0x0006C85C File Offset: 0x0006AA5C
		// (set) Token: 0x06001206 RID: 4614 RVA: 0x0006C874 File Offset: 0x0006AA74
		public bool LowDensityEnabled
		{
			get
			{
				return this.lowDensityEnabled;
			}
			set
			{
				bool flag = this.lowDensityEnabled != value;
				if (flag)
				{
					bool flag2 = !this.showDialogWindows || !value || Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtNewAutotrainRequired"), DialogStyle.Warning, DialogButtons.OKCancel) == Dialog.DialogResults.Ok;
					if (flag2)
					{
						this.parHandling.WriteValue(Definition.filterMainPath + ((this.imageGroup > 0) ? this.imageGroup.ToString() : "") + "/lowDensity/Enabled/Value", value ? "1" : "0");
						this.lowDensityEnabled = value;
						this.OnPropertyChanged("LowDensityEnabled");
					}
				}
			}
		}

		// Token: 0x1700057A RID: 1402
		// (get) Token: 0x06001207 RID: 4615 RVA: 0x0006C924 File Offset: 0x0006AB24
		// (set) Token: 0x06001208 RID: 4616 RVA: 0x0006C93C File Offset: 0x0006AB3C
		public bool WireComparerEnabled
		{
			get
			{
				return this.wireComparerEnabled;
			}
			set
			{
				bool flag = this.wireComparerEnabled != value;
				if (flag)
				{
					bool flag2 = false;
					bool flag3 = !this.wireComparerEnabled;
					if (flag3)
					{
						int num = Convert.ToInt32(this.parHandling.ReadPar(string.Concat(new string[]
						{
							Definition.filterMainPath,
							(this.imageGroup > 0) ? this.imageGroup.ToString() : "",
							Definition.comparerSubPath,
							"9/Contrast/",
							Definition.strValue
						})));
						bool flag4 = num > 0 && num < 32000;
						if (flag4)
						{
							flag2 = true;
						}
						else
						{
							bool flag5 = Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtNewAutotrainRequired"), DialogStyle.Warning, DialogButtons.OKCancel) == Dialog.DialogResults.Ok;
							if (flag5)
							{
								flag2 = true;
							}
						}
					}
					else
					{
						flag2 = true;
					}
					bool flag6 = flag2;
					if (flag6)
					{
						this.parHandling.WriteValue(string.Concat(new string[]
						{
							Definition.filterMainPath,
							(this.imageGroup > 0) ? this.imageGroup.ToString() : "",
							Definition.comparerSubPath,
							"9/Enabled/",
							Definition.strValue
						}), value ? "1" : "0");
						this.parHandling.WriteValue(string.Concat(new string[]
						{
							Definition.filterMainPath,
							(this.imageGroup > 0) ? this.imageGroup.ToString() : "",
							Definition.comparerSubPath,
							"10/Enabled/",
							Definition.strValue
						}), value ? "1" : "0");
						this.wireComparerEnabled = value;
						this.OnPropertyChanged("WireComparerEnabled");
					}
					bool flag7 = this.machineMode == MachineMode.Bulk;
					if (flag7)
					{
						BulkMode.UpdateBulkOverlappingInspection(this.parHandling);
					}
				}
			}
		}

		// Token: 0x1700057B RID: 1403
		// (get) Token: 0x06001209 RID: 4617 RVA: 0x0006CB14 File Offset: 0x0006AD14
		// (set) Token: 0x0600120A RID: 4618 RVA: 0x0006CB2C File Offset: 0x0006AD2C
		public bool GlassComparerEnabled
		{
			get
			{
				return this.glassComparerEnabled;
			}
			set
			{
				bool flag = this.glassComparerEnabled != value;
				if (flag)
				{
					bool flag2 = false;
					bool flag3 = !this.glassComparerEnabled;
					if (flag3)
					{
						int num = Convert.ToInt32(this.parHandling.ReadPar(string.Concat(new string[]
						{
							Definition.filterMainPath,
							(this.imageGroup > 0) ? this.imageGroup.ToString() : "",
							Definition.comparerSubPath,
							"11/Contrast/",
							Definition.strValue
						})));
						bool flag4 = num > 0 && num < 32000;
						if (flag4)
						{
							flag2 = true;
						}
						else
						{
							bool flag5 = Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtNewAutotrainRequired"), DialogStyle.Warning, DialogButtons.OKCancel) == Dialog.DialogResults.Ok;
							if (flag5)
							{
								flag2 = true;
							}
						}
					}
					else
					{
						flag2 = true;
					}
					bool flag6 = flag2;
					if (flag6)
					{
						for (int i = 0; i < 4; i++)
						{
							this.parHandling.WriteValue(string.Concat(new string[]
							{
								Definition.filterMainPath,
								(this.imageGroup > 0) ? this.imageGroup.ToString() : "",
								Definition.comparerSubPath,
								(11 + i).ToString(),
								"/Enabled/",
								Definition.strValue
							}), value ? "1" : "0");
						}
						this.glassComparerEnabled = value;
						this.OnPropertyChanged("GlassComparerEnabled");
					}
					bool flag7 = this.machineMode == MachineMode.Bulk;
					if (flag7)
					{
						BulkMode.UpdateBulkOverlappingInspection(this.parHandling);
					}
				}
			}
		}

		// Token: 0x1700057C RID: 1404
		// (get) Token: 0x0600120B RID: 4619 RVA: 0x0006CCCC File Offset: 0x0006AECC
		// (set) Token: 0x0600120C RID: 4620 RVA: 0x0006CCE4 File Offset: 0x0006AEE4
		public bool BonesComparerEnabled
		{
			get
			{
				return this.bonesComparerEnabled;
			}
			set
			{
				bool flag = this.bonesComparerEnabled != value;
				if (flag)
				{
					bool flag2 = false;
					bool flag3 = !this.bonesComparerEnabled;
					if (flag3)
					{
						int num = Convert.ToInt32(this.parHandling.ReadPar(string.Concat(new string[]
						{
							Definition.filterMainPath,
							(this.imageGroup > 0) ? this.imageGroup.ToString() : "",
							Definition.comparerSubPath,
							"15/Contrast/",
							Definition.strValue
						})));
						bool flag4 = num > 0 && num < 32000;
						if (flag4)
						{
							flag2 = true;
						}
						else
						{
							bool flag5 = Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtNewAutotrainRequired"), DialogStyle.Warning, DialogButtons.OKCancel) == Dialog.DialogResults.Ok;
							if (flag5)
							{
								flag2 = true;
							}
						}
					}
					else
					{
						flag2 = true;
					}
					bool flag6 = flag2;
					if (flag6)
					{
						for (int i = 0; i < 4; i++)
						{
							this.parHandling.WriteValue(string.Concat(new string[]
							{
								Definition.filterMainPath,
								(this.imageGroup > 0) ? this.imageGroup.ToString() : "",
								Definition.comparerSubPath,
								(15 + i).ToString(),
								"/Enabled/",
								Definition.strValue
							}), value ? "1" : "0");
						}
						this.bonesComparerEnabled = value;
						this.OnPropertyChanged("BonesComparerEnabled");
					}
					bool flag7 = this.machineMode == MachineMode.Bulk;
					if (flag7)
					{
						BulkMode.UpdateBulkOverlappingInspection(this.parHandling);
					}
				}
			}
		}

		// Token: 0x1700057D RID: 1405
		// (get) Token: 0x0600120D RID: 4621 RVA: 0x0006CE84 File Offset: 0x0006B084
		// (set) Token: 0x0600120E RID: 4622 RVA: 0x0006CE9C File Offset: 0x0006B09C
		public bool BlackMetalCheckerEnabled
		{
			get
			{
				return this.blackMetalCheckerEnabled;
			}
			set
			{
				bool flag = this.blackMetalCheckerEnabled != value;
				if (flag)
				{
					bool flag2 = false;
					bool flag3 = !this.blackMetalCheckerEnabled;
					if (flag3)
					{
						int num = Convert.ToInt32(this.parHandling.ReadPar(string.Concat(new string[]
						{
							Definition.filterMainPath,
							(this.imageGroup > 0) ? this.imageGroup.ToString() : "",
							Definition.comparerSubPath,
							"19/Contrast/",
							Definition.strValue
						})));
						bool flag4 = num > 0 && num < 32000;
						if (flag4)
						{
							flag2 = true;
						}
						else
						{
							bool flag5 = Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtNewAutotrainRequired"), DialogStyle.Warning, DialogButtons.OKCancel) == Dialog.DialogResults.Ok;
							if (flag5)
							{
								flag2 = true;
							}
						}
					}
					else
					{
						flag2 = true;
					}
					bool flag6 = flag2;
					if (flag6)
					{
						for (int i = 0; i < 12; i++)
						{
							this.parHandling.WriteValue(string.Concat(new string[]
							{
								Definition.filterMainPath,
								(this.imageGroup > 0) ? this.imageGroup.ToString() : "",
								Definition.comparerSubPath,
								(19 + i).ToString(),
								"/Enabled/",
								Definition.strValue
							}), value ? "1" : "0");
						}
						this.blackMetalCheckerEnabled = value;
						this.OnPropertyChanged("BlackMetalCheckerEnabled");
					}
				}
			}
		}

		// Token: 0x1700057E RID: 1406
		// (get) Token: 0x0600120F RID: 4623 RVA: 0x0006D022 File Offset: 0x0006B222
		// (set) Token: 0x06001210 RID: 4624 RVA: 0x0006D02A File Offset: 0x0006B22A
		public BoundLocalizedText StrEnhancedLowDensity { get; set; }

		// Token: 0x1700057F RID: 1407
		// (get) Token: 0x06001211 RID: 4625 RVA: 0x0006D033 File Offset: 0x0006B233
		// (set) Token: 0x06001212 RID: 4626 RVA: 0x0006D03B File Offset: 0x0006B23B
		public BoundLocalizedText StrSpecializedToDetect { get; set; }

		// Token: 0x17000580 RID: 1408
		// (get) Token: 0x06001213 RID: 4627 RVA: 0x0006D044 File Offset: 0x0006B244
		// (set) Token: 0x06001214 RID: 4628 RVA: 0x0006D04C File Offset: 0x0006B24C
		public RelayCommand CleanUpCommand { get; set; }

		// Token: 0x17000581 RID: 1409
		// (get) Token: 0x06001215 RID: 4629 RVA: 0x0006D055 File Offset: 0x0006B255
		// (set) Token: 0x06001216 RID: 4630 RVA: 0x0006D05D File Offset: 0x0006B25D
		public bool IsAlgorithmVisible { get; set; }

		// Token: 0x17000582 RID: 1410
		// (get) Token: 0x06001217 RID: 4631 RVA: 0x0006D066 File Offset: 0x0006B266
		// (set) Token: 0x06001218 RID: 4632 RVA: 0x0006D06E File Offset: 0x0006B26E
		public bool IsLDVisible { get; set; }

		// Token: 0x17000583 RID: 1411
		// (get) Token: 0x06001219 RID: 4633 RVA: 0x0006D077 File Offset: 0x0006B277
		// (set) Token: 0x0600121A RID: 4634 RVA: 0x0006D07F File Offset: 0x0006B27F
		public bool IsComparerVisible { get; set; }

		// Token: 0x17000584 RID: 1412
		// (get) Token: 0x0600121B RID: 4635 RVA: 0x0006D088 File Offset: 0x0006B288
		// (set) Token: 0x0600121C RID: 4636 RVA: 0x0006D090 File Offset: 0x0006B290
		public bool IsHighPrecisionVisible { get; set; }

		// Token: 0x17000585 RID: 1413
		// (get) Token: 0x0600121D RID: 4637 RVA: 0x0006D09C File Offset: 0x0006B29C
		// (set) Token: 0x0600121E RID: 4638 RVA: 0x0006D0B4 File Offset: 0x0006B2B4
		public string HighPrecisionDescription
		{
			get
			{
				return this.highPrecisionDescription;
			}
			set
			{
				this.highPrecisionDescription = value;
				this.OnPropertyChanged("HighPrecisionDesription");
			}
		}

		// Token: 0x0600121F RID: 4639 RVA: 0x0006D0CC File Offset: 0x0006B2CC
		public ImpurityFinderSettingsViewModel(DBConnection dbConnection, ParameterHandling parHandling, int imageGroup, bool showDialogWindows, ProductManager productManager, AccessRightsManager accessManager)
		{
			this.dbConnection = dbConnection;
			this.parHandling = parHandling;
			this.imageGroup = imageGroup;
			this.showDialogWindows = showDialogWindows;
			this.productManager = productManager;
			this.accessManager = accessManager;
			this.IsLDVisible = (accessManager.CurrentUser.Rights >= BuiltInPriviledges.Service);
			this.machineMode = MachineMode.Standard;
			dbConnection.GetMachineMode(ref this.machineMode);
			bool flag = this.machineMode == MachineMode.Bulk;
			if (flag)
			{
				this.IsLDVisible = false;
				this.IsHighPrecisionVisible = false;
			}
			this.HighPrecisionDescription = LocalizationManager.Localize("txtVA") + " 0.3 - 0.6 mm   " + LocalizationManager.Localize("txtGlass") + " 0.6 - 1.5 mm";
			this.CleanUpCommand = new RelayCommand(new Action<object>(this.CleanUp));
			this.setAdditionalComparer();
			bool isComparerVisible = this.IsComparerVisible;
			if (isComparerVisible)
			{
				this.wireComparerEnabled = (parHandling.ReadPar(string.Concat(new string[]
				{
					Definition.filterMainPath,
					(imageGroup > 0) ? imageGroup.ToString() : "",
					Definition.comparerSubPath,
					"9/Enabled/",
					Definition.strValue
				})) == "1");
				this.glassComparerEnabled = (parHandling.ReadPar(string.Concat(new string[]
				{
					Definition.filterMainPath,
					(imageGroup > 0) ? imageGroup.ToString() : "",
					Definition.comparerSubPath,
					"11/Enabled/",
					Definition.strValue
				})) == "1");
				this.bonesComparerEnabled = (parHandling.ReadPar(string.Concat(new string[]
				{
					Definition.filterMainPath,
					(imageGroup > 0) ? imageGroup.ToString() : "",
					Definition.comparerSubPath,
					"15/Enabled/",
					Definition.strValue
				})) == "1");
				this.blackMetalCheckerEnabled = (parHandling.ReadPar(string.Concat(new string[]
				{
					Definition.filterMainPath,
					(imageGroup > 0) ? imageGroup.ToString() : "",
					Definition.comparerSubPath,
					"19/Enabled/",
					Definition.strValue
				})) == "1");
			}
			this.GetValuesFromDatabase();
			this.RegisterLocalizedText();
		}

		// Token: 0x06001220 RID: 4640 RVA: 0x0006D334 File Offset: 0x0006B534
		private void setAdditionalComparer()
		{
			DetectorType detectorType = DetectorType.DT;
			this.dbConnection.GetDetectorType(ref detectorType);
			string a = "";
			bool flag = detectorType == DetectorType.Sesotec;
			if (flag)
			{
				a = this.parHandling.ReadPar(Definition.detectorSesotecPixelSizePath);
			}
			bool flag2 = detectorType == DetectorType.DTNetTDI;
			if (flag2)
			{
				a = this.parHandling.ReadPar(Definition.detectorDtPixelSizePath);
			}
			bool flag3 = false;
			this.dbConnection.GetDemoMode(ref flag3);
			bool flag4 = true;
			this.dbConnection.GetMXFeatures04Det(ref flag4);
			string a2 = "";
			this.dbConnection.GetXRaySourceModelNumber(ref a2);
			bool flag5 = a2 == "IXS050SE100P214" && a == "0.4";
			this.IsAlgorithmVisible = false;
			this.IsComparerVisible = false;
			this.IsHighPrecisionVisible = false;
			bool flag6 = Machine.Instance.MachineType == MachineType.RayconDPlus || Machine.Instance.MachineType == MachineType.RayconEX2A || (flag3 && a == "0.4");
			if (flag6)
			{
				bool flag7 = (detectorType == DetectorType.Sesotec || detectorType == DetectorType.DTNetTDI) && a == "0.4" && this.parHandling.ReadPar(CtrlAcquisitionSettingsViewModel.binning2EnabledPath) == "1";
				if (flag7)
				{
					this.IsComparerVisible = true;
					bool flag8 = detectorType == DetectorType.DTNetTDI || (flag3 && this.machineMode != MachineMode.Bulk);
					if (flag8)
					{
						this.IsHighPrecisionVisible = true;
					}
					bool flag9 = detectorType == DetectorType.DTNetTDI;
					if (flag9)
					{
						this.IsLDVisible = false;
					}
					bool flag10 = (flag5 && flag4) || Machine.Instance.MachineType == MachineType.RayconEX2A;
					if (flag10)
					{
						this.IsComparerVisible = false;
						this.IsHighPrecisionVisible = false;
					}
				}
			}
			else
			{
				this.IsAlgorithmVisible = true;
			}
		}

		// Token: 0x06001221 RID: 4641 RVA: 0x0006D4E8 File Offset: 0x0006B6E8
		private bool checkSingleProduct()
		{
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			this.dbConnection.GetMultiRegionEnabled(ref flag);
			this.dbConnection.GetMultiLaneProductsEnabled(ref flag2);
			this.dbConnection.GetMultiProductsEnabled(ref flag3);
			return this.productManager.CurrentLoadedProduct.IsSingleProduct && (!flag2 && !flag3 && !flag);
		}

		// Token: 0x06001222 RID: 4642 RVA: 0x0006D553 File Offset: 0x0006B753
		private void RegisterLocalizedText()
		{
			this.StrSpecializedToDetect = new BoundLocalizedText("txtSpecializedToDetect");
			this.StrEnhancedLowDensity = new BoundLocalizedText("txtEnhancedLowDensity");
		}

		// Token: 0x06001223 RID: 4643 RVA: 0x0006D578 File Offset: 0x0006B778
		public void DeregisterLocalizedText()
		{
			MainWindow.localizationManager.Deregister(this.StrSpecializedToDetect);
			MainWindow.localizationManager.Deregister(this.StrEnhancedLowDensity);
		}

		// Token: 0x06001224 RID: 4644 RVA: 0x0006D5A0 File Offset: 0x0006B7A0
		private void GetValuesFromDatabase()
		{
			string str = (this.imageGroup > 0) ? this.imageGroup.ToString() : "";
			int num = 0;
			string a = this.parHandling.ReadPar(Definition.filterMainPath + str + "/lowDensity/Enabled/Value");
			this.lowDensityEnabled = (a == "1");
			string s = this.parHandling.ReadPar(Definition.filterMainPath + str + "/global/Algorithm/Value");
			int.TryParse(s, out num);
			bool flag = num == 0 || num == 3;
			if (flag)
			{
				this.impurityFinderType = (ImpurityFinderType)num;
			}
		}

		// Token: 0x06001225 RID: 4645 RVA: 0x0006D644 File Offset: 0x0006B844
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x06001226 RID: 4646 RVA: 0x0006D674 File Offset: 0x0006B874
		private void SetNewAlgorithmParameter(ImpurityFinderType newType)
		{
			string str = (this.imageGroup > 0) ? this.imageGroup.ToString() : "";
			ParameterHandling parameterHandling = this.parHandling;
			string path = Definition.filterMainPath + str + "/global/Algorithm/Value";
			int num = (int)newType;
			parameterHandling.WriteValue(path, num.ToString());
			bool flag = newType == ImpurityFinderType.Glass;
			if (flag)
			{
				this.parHandling.WriteValue(Definition.filterMainPath + str + "/global/CustomTolerance/Value", "0.20");
				this.parHandling.WriteValue(Definition.filterMainPath + str + "/global/CustomFilterSize/Value", "21");
				this.parHandling.WriteValue(Definition.filterMainPath + str + Definition.enableGlobalCustomSizeSubPath, "1");
			}
			else
			{
				this.parHandling.WriteValue(Definition.filterMainPath + str + Definition.enableGlobalCustomSizeSubPath, "0");
			}
		}

		// Token: 0x06001227 RID: 4647 RVA: 0x0006D753 File Offset: 0x0006B953
		private void CleanUp(object obj)
		{
			this.DeregisterLocalizedText();
		}

		// Token: 0x04000BED RID: 3053
		private const string filterAlgorithmSubPath = "/global/Algorithm/Value";

		// Token: 0x04000BEE RID: 3054
		private const string customToleranceSubPath = "/global/CustomTolerance/Value";

		// Token: 0x04000BEF RID: 3055
		private const string customFilterSizeSubPath = "/global/CustomFilterSize/Value";

		// Token: 0x04000BF0 RID: 3056
		private const string lowDensityEnabledSubPath = "/lowDensity/Enabled/Value";

		// Token: 0x04000BF1 RID: 3057
		private DBConnection dbConnection;

		// Token: 0x04000BF2 RID: 3058
		private ParameterHandling parHandling;

		// Token: 0x04000BF3 RID: 3059
		private ImpurityFinderType impurityFinderType;

		// Token: 0x04000BF4 RID: 3060
		private bool lowDensityEnabled;

		// Token: 0x04000BF5 RID: 3061
		private bool wireComparerEnabled;

		// Token: 0x04000BF6 RID: 3062
		private bool glassComparerEnabled;

		// Token: 0x04000BF7 RID: 3063
		private bool bonesComparerEnabled;

		// Token: 0x04000BF8 RID: 3064
		private bool blackMetalCheckerEnabled;

		// Token: 0x04000BF9 RID: 3065
		private int imageGroup;

		// Token: 0x04000BFA RID: 3066
		private bool showDialogWindows;

		// Token: 0x04000BFB RID: 3067
		private ProductManager productManager;

		// Token: 0x04000BFC RID: 3068
		private AccessRightsManager accessManager;

		// Token: 0x04000BFD RID: 3069
		private MachineMode machineMode;

		// Token: 0x04000C06 RID: 3078
		private string highPrecisionDescription;
	}
}
