using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows;
using Common;
using DatabaseConnection;
using Definitions;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x02000259 RID: 601
	public class Weigher : INotifyPropertyChanged
	{
		// Token: 0x14000106 RID: 262
		// (add) Token: 0x06002E1D RID: 11805 RVA: 0x0012D9E8 File Offset: 0x0012BBE8
		// (remove) Token: 0x06002E1E RID: 11806 RVA: 0x0012DA20 File Offset: 0x0012BC20
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x17000C57 RID: 3159
		// (get) Token: 0x06002E1F RID: 11807 RVA: 0x0012DA58 File Offset: 0x0012BC58
		// (set) Token: 0x06002E20 RID: 11808 RVA: 0x0012DA70 File Offset: 0x0012BC70
		public double ExpectedWeight
		{
			get
			{
				return this.expectedWeight;
			}
			set
			{
				bool flag = this.expectedWeight != value;
				if (flag)
				{
					double num = this.expectedWeight;
					this.expectedWeight = ((value >= 100000.0) ? 99999.9 : value);
					this.OnPropertyChanged("ExpectedWeight");
					this.dbConnection.UpdateTable(this.productTableName, Definition.strReferenceWeight, this.expectedWeight.ToString(new CultureInfo("en-US")));
					bool flag2 = this.imageGroup <= 0;
					if (flag2)
					{
						this.productManager.UpdateAssignedSubLaneProducts(Definition.strReferenceWeight, this.expectedWeight.ToString(new CultureInfo("en-US")));
					}
					else
					{
						this.productManager.UpdateAssignedMainProduct(Definition.strReferenceWeight, this.expectedWeight.ToString(new CultureInfo("en-US")), this.imageGroup);
					}
					bool flag3 = this.initialized;
					if (flag3)
					{
						LogbookManager.AddEntry(SSTEntryCode.WeigherParameterChanged, 0, ((int)(num * 1000.0)).ToString(), ((int)(this.expectedWeight * 1000.0)).ToString());
					}
				}
			}
		}

		// Token: 0x17000C58 RID: 3160
		// (get) Token: 0x06002E21 RID: 11809 RVA: 0x0012DB9C File Offset: 0x0012BD9C
		// (set) Token: 0x06002E22 RID: 11810 RVA: 0x0012DBB4 File Offset: 0x0012BDB4
		public double CurrentWeight
		{
			get
			{
				return this.currentWeight;
			}
			set
			{
				this.currentWeight = value;
				this.OnPropertyChanged("CurrentWeight");
			}
		}

		// Token: 0x17000C59 RID: 3161
		// (get) Token: 0x06002E23 RID: 11811 RVA: 0x0012DBCC File Offset: 0x0012BDCC
		// (set) Token: 0x06002E24 RID: 11812 RVA: 0x0012DBE4 File Offset: 0x0012BDE4
		public double MinWeight
		{
			get
			{
				return this.minWeight;
			}
			set
			{
				bool flag = this.minWeight != value;
				if (flag)
				{
					double num = this.minWeight;
					this.minWeight = ((value >= 100000.0) ? 99999.9 : value);
					this.OnPropertyChanged("MinWeight");
					this.UpdateMinPixelWeight();
					this.dbConnection.UpdateTable(this.productTableName, "MinWeight", this.minWeight.ToString(new CultureInfo("en-US")));
					bool flag2 = this.imageGroup <= 0;
					if (flag2)
					{
						this.productManager.UpdateAssignedSubLaneProducts("MinWeight", this.minWeight.ToString(new CultureInfo("en-US")));
					}
					else
					{
						this.productManager.UpdateAssignedMainProduct("MinWeight", this.minWeight.ToString(new CultureInfo("en-US")), this.imageGroup);
					}
					bool flag3 = this.initialized;
					if (flag3)
					{
						LogbookManager.AddEntry(SSTEntryCode.WeigherParameterChanged, 1, ((int)(num * 1000.0)).ToString(), ((int)(this.minWeight * 1000.0)).ToString());
					}
				}
			}
		}

		// Token: 0x17000C5A RID: 3162
		// (get) Token: 0x06002E25 RID: 11813 RVA: 0x0012DD18 File Offset: 0x0012BF18
		// (set) Token: 0x06002E26 RID: 11814 RVA: 0x0012DD30 File Offset: 0x0012BF30
		public double MaxWeight
		{
			get
			{
				return this.maxWeight;
			}
			set
			{
				bool flag = this.maxWeight != value;
				if (flag)
				{
					double num = this.maxWeight;
					this.maxWeight = ((value >= 100000.0) ? 99999.9 : value);
					this.OnPropertyChanged("MaxWeight");
					this.UpdateMaxPixelWeight();
					this.dbConnection.UpdateTable(this.productTableName, "MaxWeight", this.maxWeight.ToString(new CultureInfo("en-US")));
					bool flag2 = this.imageGroup <= 0;
					if (flag2)
					{
						this.productManager.UpdateAssignedSubLaneProducts("MaxWeight", this.maxWeight.ToString(new CultureInfo("en-US")));
					}
					else
					{
						this.productManager.UpdateAssignedMainProduct("MaxWeight", this.maxWeight.ToString(new CultureInfo("en-US")), this.imageGroup);
					}
					bool flag3 = this.initialized;
					if (flag3)
					{
						LogbookManager.AddEntry(SSTEntryCode.WeigherParameterChanged, 2, ((int)(num * 1000.0)).ToString(), ((int)(this.maxWeight * 1000.0)).ToString());
					}
				}
			}
		}

		// Token: 0x17000C5B RID: 3163
		// (get) Token: 0x06002E27 RID: 11815 RVA: 0x0012DE64 File Offset: 0x0012C064
		// (set) Token: 0x06002E28 RID: 11816 RVA: 0x0012DE7C File Offset: 0x0012C07C
		public bool WeightInGramm
		{
			get
			{
				return this.weightInGramm;
			}
			set
			{
				this.weightInGramm = value;
				this.OnPropertyChanged("WeightInGramm");
				bool flag = this.weightInGramm;
				if (flag)
				{
					this.dbConnection.SetWeightUnit(WeightUnit.Gramm);
					this.WeightUnitAbbr = "g";
				}
				else
				{
					this.dbConnection.SetWeightUnit(WeightUnit.Ounce);
					this.WeightUnitAbbr = "oz";
				}
				XRSImageDefs.grammIsWeightUnit = value;
			}
		}

		// Token: 0x17000C5C RID: 3164
		// (get) Token: 0x06002E29 RID: 11817 RVA: 0x0012DEE8 File Offset: 0x0012C0E8
		public bool WeightInOunce
		{
			get
			{
				return !this.weightInGramm;
			}
		}

		// Token: 0x17000C5D RID: 3165
		// (get) Token: 0x06002E2A RID: 11818 RVA: 0x0012DF04 File Offset: 0x0012C104
		// (set) Token: 0x06002E2B RID: 11819 RVA: 0x0012DF1C File Offset: 0x0012C11C
		public string WeightUnitAbbr
		{
			get
			{
				return this.weightUnitAbbr;
			}
			set
			{
				this.weightUnitAbbr = value;
				this.OnPropertyChanged("WeightUnitAbbr");
			}
		}

		// Token: 0x17000C5E RID: 3166
		// (get) Token: 0x06002E2C RID: 11820 RVA: 0x0012DF32 File Offset: 0x0012C132
		// (set) Token: 0x06002E2D RID: 11821 RVA: 0x0012DF3A File Offset: 0x0012C13A
		public MachineState MachineState { get; set; }

		// Token: 0x17000C5F RID: 3167
		// (get) Token: 0x06002E2E RID: 11822 RVA: 0x0012DF43 File Offset: 0x0012C143
		// (set) Token: 0x06002E2F RID: 11823 RVA: 0x0012DF4B File Offset: 0x0012C14B
		public BoundLocalizedText StrWeight { get; set; }

		// Token: 0x17000C60 RID: 3168
		// (get) Token: 0x06002E30 RID: 11824 RVA: 0x0012DF54 File Offset: 0x0012C154
		// (set) Token: 0x06002E31 RID: 11825 RVA: 0x0012DF5C File Offset: 0x0012C15C
		public BoundLocalizedText StrCurrentWeight { get; set; }

		// Token: 0x17000C61 RID: 3169
		// (get) Token: 0x06002E32 RID: 11826 RVA: 0x0012DF65 File Offset: 0x0012C165
		// (set) Token: 0x06002E33 RID: 11827 RVA: 0x0012DF6D File Offset: 0x0012C16D
		public BoundLocalizedText StrMinimum { get; set; }

		// Token: 0x17000C62 RID: 3170
		// (get) Token: 0x06002E34 RID: 11828 RVA: 0x0012DF76 File Offset: 0x0012C176
		// (set) Token: 0x06002E35 RID: 11829 RVA: 0x0012DF7E File Offset: 0x0012C17E
		public BoundLocalizedText StrMaximum { get; set; }

		// Token: 0x17000C63 RID: 3171
		// (get) Token: 0x06002E36 RID: 11830 RVA: 0x0012DF87 File Offset: 0x0012C187
		// (set) Token: 0x06002E37 RID: 11831 RVA: 0x0012DF8F File Offset: 0x0012C18F
		public BoundLocalizedText StrWeightUnit { get; set; }

		// Token: 0x17000C64 RID: 3172
		// (get) Token: 0x06002E38 RID: 11832 RVA: 0x0012DF98 File Offset: 0x0012C198
		// (set) Token: 0x06002E39 RID: 11833 RVA: 0x0012DFA0 File Offset: 0x0012C1A0
		public RelayCommand ChangeWeightCommand { get; set; }

		// Token: 0x17000C65 RID: 3173
		// (get) Token: 0x06002E3A RID: 11834 RVA: 0x0012DFA9 File Offset: 0x0012C1A9
		// (set) Token: 0x06002E3B RID: 11835 RVA: 0x0012DFB1 File Offset: 0x0012C1B1
		public RelayCommand ChangeMinWeightCommand { get; set; }

		// Token: 0x17000C66 RID: 3174
		// (get) Token: 0x06002E3C RID: 11836 RVA: 0x0012DFBA File Offset: 0x0012C1BA
		// (set) Token: 0x06002E3D RID: 11837 RVA: 0x0012DFC2 File Offset: 0x0012C1C2
		public RelayCommand ChangeMaxWeightCommand { get; set; }

		// Token: 0x17000C67 RID: 3175
		// (get) Token: 0x06002E3E RID: 11838 RVA: 0x0012DFCB File Offset: 0x0012C1CB
		// (set) Token: 0x06002E3F RID: 11839 RVA: 0x0012DFD3 File Offset: 0x0012C1D3
		public bool DemoMode { get; set; }

		// Token: 0x06002E40 RID: 11840 RVA: 0x0012DFDC File Offset: 0x0012C1DC
		public Weigher(DBConnection dbConnection, ParameterHandling parHandling, ImageDataTransfer imageTransfer, MachineState machineState, ProductManager productManager, int imageGroup, AccessRightsManager accessRightsManager)
		{
			this.dbConnection = dbConnection;
			this.parHandling = parHandling;
			this.imageTransfer = imageTransfer;
			this.MachineState = machineState;
			this.productManager = productManager;
			this.imageGroup = imageGroup;
			this.accessRightsManager = accessRightsManager;
			bool demoMode = false;
			dbConnection.GetDemoMode(ref demoMode);
			this.DemoMode = demoMode;
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
						"_",
						imageGroup.ToString(),
						" ",
						array[0].ProductName
					});
				}
			}
			this.strImageGroup = ((imageGroup > 0) ? imageGroup.ToString() : "");
			dbConnection.GetCurrentProductTableName(ref this.productTableName);
			bool flag3 = imageGroup > 0;
			if (flag3)
			{
				this.productTableName = this.productTableName + Definition.subTableIdentifier.ToString() + this.strImageGroup;
			}
			this.ChangeWeightCommand = new RelayCommand(new Action<object>(this.ChangeWeight));
			this.ChangeMinWeightCommand = new RelayCommand(new Action<object>(this.ChangeMinWeight));
			this.ChangeMaxWeightCommand = new RelayCommand(new Action<object>(this.ChangeMaxWeight));
			this.UpdateValuesFromDatabase();
			this.UpdateCurrentWeight();
			this.RegisterLocalizedText();
			imageTransfer.FrameArrived += this.imageTransfer_FrameArrived;
			this.initialized = true;
		}

		// Token: 0x06002E41 RID: 11841 RVA: 0x0012E1F5 File Offset: 0x0012C3F5
		private void imageTransfer_FrameArrived(object sender, ReceiveEventArgs e)
		{
			this.UpdateCurrentWeight();
		}

		// Token: 0x06002E42 RID: 11842 RVA: 0x0012E200 File Offset: 0x0012C400
		private void RegisterLocalizedText()
		{
			this.StrWeight = new BoundLocalizedText("txtWeight");
			this.StrCurrentWeight = new BoundLocalizedText("txtCurrentWeight");
			this.StrMinimum = new BoundLocalizedText("txtMinimum");
			this.StrMaximum = new BoundLocalizedText("txtMaximum");
			this.StrWeightUnit = new BoundLocalizedText("txtWeightUnit");
		}

		// Token: 0x06002E43 RID: 11843 RVA: 0x0012E264 File Offset: 0x0012C464
		public void DeregisterLocalizedText()
		{
			MainWindow.localizationManager.Deregister(this.StrWeight);
			MainWindow.localizationManager.Deregister(this.StrCurrentWeight);
			MainWindow.localizationManager.Deregister(this.StrMinimum);
			MainWindow.localizationManager.Deregister(this.StrMaximum);
			MainWindow.localizationManager.Deregister(this.StrWeightUnit);
			this.ChangeWeightCommand = null;
			this.ChangeMinWeightCommand = null;
			this.ChangeMaxWeightCommand = null;
		}

		// Token: 0x06002E44 RID: 11844 RVA: 0x0012E2E0 File Offset: 0x0012C4E0
		private void UpdateValuesFromDatabase()
		{
			string s = "";
			string s2 = "";
			string s3 = "";
			double num = 0.0;
			WeightUnit weightUnit = WeightUnit.Gramm;
			bool flag = this.dbConnection.Connection != null;
			if (flag)
			{
				this.dbConnection.GetParameterValue(Definition.strReferenceWeight, this.productTableName, ref s);
				this.dbConnection.GetParameterValue("MinWeight", this.productTableName, ref s2);
				this.dbConnection.GetParameterValue("MaxWeight", this.productTableName, ref s3);
				this.dbConnection.GetWeightUnit(ref weightUnit);
				try
				{
					bool flag2 = double.TryParse(s, NumberStyles.Float, new CultureInfo("en-US"), out num);
					if (flag2)
					{
						this.ExpectedWeight = num;
					}
					else
					{
						this.ExpectedWeight = 0.0;
					}
					bool flag3 = double.TryParse(s2, NumberStyles.Float, new CultureInfo("en-US"), out num);
					if (flag3)
					{
						this.MinWeight = num;
					}
					else
					{
						this.MinWeight = 0.0;
					}
					bool flag4 = double.TryParse(s3, NumberStyles.Float, new CultureInfo("en-US"), out num);
					if (flag4)
					{
						this.MaxWeight = num;
					}
					else
					{
						this.MaxWeight = 0.0;
					}
					bool flag5 = weightUnit == WeightUnit.Gramm;
					if (flag5)
					{
						this.WeightInGramm = true;
						this.WeightUnitAbbr = "g";
					}
					else
					{
						this.WeightInGramm = false;
						this.WeightUnitAbbr = "oz";
					}
				}
				catch (Exception)
				{
				}
			}
		}

		// Token: 0x06002E45 RID: 11845 RVA: 0x0012E478 File Offset: 0x0012C678
		private void UpdateCurrentWeight()
		{
			try
			{
				string str = (this.imageGroup > 0) ? this.imageGroup.ToString() : "";
				bool flag = this.ExpectedWeight != 0.0 && this.parHandling != null;
				if (flag)
				{
					double num = double.Parse(this.parHandling.ReadPar(Definition.filterMainPath + str + Weigher.currentWeightSubPath));
					double num2 = double.Parse(this.parHandling.ReadPar(Definition.filterMainPath + str + Weigher.expectedWeightSubPath));
					bool flag2 = num2 != 0.0;
					if (flag2)
					{
						this.CurrentWeight = num / num2 * this.ExpectedWeight;
					}
					else
					{
						this.CurrentWeight = 0.0;
					}
				}
			}
			catch (FormatException)
			{
			}
		}

		// Token: 0x06002E46 RID: 11846 RVA: 0x0012E558 File Offset: 0x0012C758
		private void UpdateMaxPixelWeight()
		{
			long num;
			long.TryParse(this.parHandling.ReadPar(Definition.filterMainPath + this.strImageGroup + Weigher.expectedWeightSubPath), out num);
			bool flag = this.expectedWeight != 0.0 && num != 0L && this.parHandling != null;
			if (flag)
			{
				double num2 = (double)num * this.maxWeight / this.expectedWeight;
				bool flag2 = this.parHandling != null;
				if (flag2)
				{
					this.parHandling.WriteValue(Definition.filterMainPath + this.strImageGroup + Weigher.maxWeightSubPath, ((long)num2).ToString());
				}
			}
		}

		// Token: 0x06002E47 RID: 11847 RVA: 0x0012E600 File Offset: 0x0012C800
		private void UpdateMinPixelWeight()
		{
			long num;
			long.TryParse(this.parHandling.ReadPar(Definition.filterMainPath + this.strImageGroup + Weigher.expectedWeightSubPath), out num);
			bool flag = this.expectedWeight != 0.0 && num != 0L && this.parHandling != null;
			if (flag)
			{
				double num2 = (double)num * this.minWeight / this.expectedWeight;
				bool flag2 = this.parHandling != null;
				if (flag2)
				{
					this.parHandling.WriteValue(Definition.filterMainPath + this.strImageGroup + Weigher.minWeightSubPath, ((long)num2).ToString());
				}
			}
		}

		// Token: 0x06002E48 RID: 11848 RVA: 0x0012E6A8 File Offset: 0x0012C8A8
		public void ChangeWeight(object obj)
		{
			string oldValue = this.weightInGramm ? Math.Round(this.ExpectedWeight, 1).ToString("F1") : Math.Round(this.ExpectedWeight * XRSImageDefs.grammToOunceFactor, 2).ToString("F2");
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtWeight"), oldValue, NumKeyboardType.TypeDouble);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_InputWeight;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x06002E49 RID: 11849 RVA: 0x0012E734 File Offset: 0x0012C934
		public void ChangeMinWeight(object obj)
		{
			string oldValue = this.weightInGramm ? Math.Round(this.MinWeight, 1).ToString("F1") : Math.Round(this.MinWeight * XRSImageDefs.grammToOunceFactor, 2).ToString("F2");
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtMinimum"), oldValue, NumKeyboardType.TypeDouble);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_InputMinWeight;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x06002E4A RID: 11850 RVA: 0x0012E7C0 File Offset: 0x0012C9C0
		public void ChangeMaxWeight(object obj)
		{
			string oldValue = this.weightInGramm ? Math.Round(this.MaxWeight, 1).ToString("F1") : Math.Round(this.MaxWeight * XRSImageDefs.grammToOunceFactor, 2).ToString("F2");
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtMaximum"), oldValue, NumKeyboardType.TypeDouble);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_InputMaxWeight;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x06002E4B RID: 11851 RVA: 0x0012E84C File Offset: 0x0012CA4C
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x06002E4C RID: 11852 RVA: 0x0012E87C File Offset: 0x0012CA7C
		private void numKeyboard_InputWeight(object sender, KeyboardInputEventArgs e)
		{
			try
			{
				bool flag = this.weightInGramm;
				if (flag)
				{
					this.ExpectedWeight = double.Parse(e.Text, new CultureInfo("de-DE"));
				}
				else
				{
					this.ExpectedWeight = double.Parse(e.Text, new CultureInfo("de-DE")) / XRSImageDefs.grammToOunceFactor;
				}
				this.numKeyboard.TextInputKeyboard -= this.numKeyboard_InputWeight;
			}
			catch
			{
			}
		}

		// Token: 0x06002E4D RID: 11853 RVA: 0x0012E908 File Offset: 0x0012CB08
		private void numKeyboard_InputMinWeight(object sender, KeyboardInputEventArgs e)
		{
			try
			{
				bool flag = this.weightInGramm;
				if (flag)
				{
					this.MinWeight = double.Parse(e.Text, new CultureInfo("de-DE"));
				}
				else
				{
					this.MinWeight = double.Parse(e.Text, new CultureInfo("de-DE")) / XRSImageDefs.grammToOunceFactor;
				}
				this.numKeyboard.TextInputKeyboard -= this.numKeyboard_InputMinWeight;
			}
			catch
			{
			}
		}

		// Token: 0x06002E4E RID: 11854 RVA: 0x0012E994 File Offset: 0x0012CB94
		private void numKeyboard_InputMaxWeight(object sender, KeyboardInputEventArgs e)
		{
			try
			{
				bool flag = this.weightInGramm;
				if (flag)
				{
					this.MaxWeight = double.Parse(e.Text, new CultureInfo("de-DE"));
				}
				else
				{
					this.MaxWeight = double.Parse(e.Text, new CultureInfo("de-DE")) / XRSImageDefs.grammToOunceFactor;
				}
				this.numKeyboard.TextInputKeyboard -= this.numKeyboard_InputMaxWeight;
			}
			catch
			{
			}
		}

		// Token: 0x06002E4F RID: 11855 RVA: 0x0012EA20 File Offset: 0x0012CC20
		public void LearnWeight()
		{
			string str = (this.imageGroup > 0) ? this.imageGroup.ToString() : "";
			bool flag = this.ExpectedWeight > 0.0;
			if (flag)
			{
				bool flag2 = this.parHandling.ReadPar(Definition.filterMainPath + str + Weigher.enableWeightSubPath) == "0";
				if (flag2)
				{
					this.parHandling.WriteValue(Definition.filterMainPath + str + Weigher.enableWeightSubPath, "1");
				}
				bool flag3 = Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtOverwriteExisting"), DialogStyle.Warning, DialogButtons.OKCancel) == Dialog.DialogResults.Ok;
				if (flag3)
				{
					this.autolearn = new AutotrainControl(this.imageTransfer, this.parHandling, this.dbConnection, AutolearnType.Weight, this.MachineState, this.productManager, this.imageGroup, null, true, false, this.accessRightsManager, null, null);
					this.autolearn.Unloaded += this.autolearn_Unloaded;
					Subwindow.Show(this.autolearn, SubwindowButtons.None, this.autolearn.Buttons);
				}
			}
			else
			{
				Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("msgWrongWeight"), DialogStyle.Error, DialogButtons.OK);
			}
		}

		// Token: 0x06002E50 RID: 11856 RVA: 0x0012EB5B File Offset: 0x0012CD5B
		private void autolearn_Unloaded(object sender, RoutedEventArgs e)
		{
			this.autolearn.Unloaded -= this.autolearn_Unloaded;
			this.UpdateMinPixelWeight();
			this.UpdateMaxPixelWeight();
			this.UpdateCurrentWeight();
		}

		// Token: 0x06002E51 RID: 11857 RVA: 0x0012EB8B File Offset: 0x0012CD8B
		public void CleanUp()
		{
			this.imageTransfer.FrameArrived -= this.imageTransfer_FrameArrived;
			this.DeregisterLocalizedText();
		}

		// Token: 0x04001CE3 RID: 7395
		public static readonly string minWeightSubPath = "/weigher/MinWeight/Value";

		// Token: 0x04001CE4 RID: 7396
		public static readonly string maxWeightSubPath = "/weigher/MaxWeight/Value";

		// Token: 0x04001CE5 RID: 7397
		public static readonly string currentWeightSubPath = "/weigher/CurrentWeight/Value";

		// Token: 0x04001CE6 RID: 7398
		public static readonly string expectedWeightSubPath = "/weigher/ExpectedWeight/Value";

		// Token: 0x04001CE7 RID: 7399
		public static readonly string enableWeightSubPath = "/weigher/Enabled/Value";

		// Token: 0x04001CE8 RID: 7400
		public static readonly string triggeredSubPath = "/weigher/Triggered/Value";

		// Token: 0x04001CE9 RID: 7401
		private double maxWeight;

		// Token: 0x04001CEA RID: 7402
		private double minWeight;

		// Token: 0x04001CEB RID: 7403
		private double expectedWeight;

		// Token: 0x04001CEC RID: 7404
		private double currentWeight;

		// Token: 0x04001CED RID: 7405
		private DBConnection dbConnection;

		// Token: 0x04001CEE RID: 7406
		private NumKeyboard numKeyboard;

		// Token: 0x04001CEF RID: 7407
		private ParameterHandling parHandling;

		// Token: 0x04001CF0 RID: 7408
		private ImageDataTransfer imageTransfer;

		// Token: 0x04001CF1 RID: 7409
		private bool weightInGramm;

		// Token: 0x04001CF2 RID: 7410
		private string weightUnitAbbr;

		// Token: 0x04001CF3 RID: 7411
		private int imageGroup;

		// Token: 0x04001CF4 RID: 7412
		private string strImageGroup;

		// Token: 0x04001CF5 RID: 7413
		private string productTableName;

		// Token: 0x04001CF6 RID: 7414
		private bool initialized;

		// Token: 0x04001CF7 RID: 7415
		private AutotrainControl autolearn;

		// Token: 0x04001CF8 RID: 7416
		private ProductManager productManager;

		// Token: 0x04001CF9 RID: 7417
		private string additionalLogbookText = "";

		// Token: 0x04001CFA RID: 7418
		private AccessRightsManager accessRightsManager;
	}
}
