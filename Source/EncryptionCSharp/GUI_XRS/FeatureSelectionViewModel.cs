using System;
using System.ComponentModel;
using System.Diagnostics;
using Common;
using DatabaseConnection;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x020000A6 RID: 166
	public class FeatureSelectionViewModel : INotifyPropertyChanged
	{
		// Token: 0x14000052 RID: 82
		// (add) Token: 0x0600104F RID: 4175 RVA: 0x00065958 File Offset: 0x00063B58
		// (remove) Token: 0x06001050 RID: 4176 RVA: 0x00065990 File Offset: 0x00063B90
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x170004E5 RID: 1253
		// (get) Token: 0x06001051 RID: 4177 RVA: 0x000659C5 File Offset: 0x00063BC5
		// (set) Token: 0x06001052 RID: 4178 RVA: 0x000659CD File Offset: 0x00063BCD
		public Feature AdvancedFilters { get; set; }

		// Token: 0x170004E6 RID: 1254
		// (get) Token: 0x06001053 RID: 4179 RVA: 0x000659D6 File Offset: 0x00063BD6
		// (set) Token: 0x06001054 RID: 4180 RVA: 0x000659DE File Offset: 0x00063BDE
		public Feature MultiProduct { get; set; }

		// Token: 0x170004E7 RID: 1255
		// (get) Token: 0x06001055 RID: 4181 RVA: 0x000659E7 File Offset: 0x00063BE7
		// (set) Token: 0x06001056 RID: 4182 RVA: 0x000659EF File Offset: 0x00063BEF
		public Feature MultiLaneProduct { get; set; }

		// Token: 0x170004E8 RID: 1256
		// (get) Token: 0x06001057 RID: 4183 RVA: 0x000659F8 File Offset: 0x00063BF8
		// (set) Token: 0x06001058 RID: 4184 RVA: 0x00065A00 File Offset: 0x00063C00
		public Feature MultiLaneInspection { get; set; }

		// Token: 0x170004E9 RID: 1257
		// (get) Token: 0x06001059 RID: 4185 RVA: 0x00065A09 File Offset: 0x00063C09
		// (set) Token: 0x0600105A RID: 4186 RVA: 0x00065A11 File Offset: 0x00063C11
		public Feature ZoneAnalyser { get; set; }

		// Token: 0x170004EA RID: 1258
		// (get) Token: 0x0600105B RID: 4187 RVA: 0x00065A1A File Offset: 0x00063C1A
		// (set) Token: 0x0600105C RID: 4188 RVA: 0x00065A22 File Offset: 0x00063C22
		public Feature SensPredAndComplMode { get; set; }

		// Token: 0x170004EB RID: 1259
		// (get) Token: 0x0600105D RID: 4189 RVA: 0x00065A2B File Offset: 0x00063C2B
		// (set) Token: 0x0600105E RID: 4190 RVA: 0x00065A33 File Offset: 0x00063C33
		public Feature BulkObjectProducts { get; set; }

		// Token: 0x170004EC RID: 1260
		// (get) Token: 0x0600105F RID: 4191 RVA: 0x00065A3C File Offset: 0x00063C3C
		// (set) Token: 0x06001060 RID: 4192 RVA: 0x00065A44 File Offset: 0x00063C44
		public Feature ObjectBasedFlapsFeature { get; set; }

		// Token: 0x170004ED RID: 1261
		// (get) Token: 0x06001061 RID: 4193 RVA: 0x00065A4D File Offset: 0x00063C4D
		// (set) Token: 0x06001062 RID: 4194 RVA: 0x00065A55 File Offset: 0x00063C55
		public Feature HighThroughputMode { get; set; }

		// Token: 0x170004EE RID: 1262
		// (get) Token: 0x06001063 RID: 4195 RVA: 0x00065A5E File Offset: 0x00063C5E
		// (set) Token: 0x06001064 RID: 4196 RVA: 0x00065A66 File Offset: 0x00063C66
		public Feature AnomalyDetection { get; set; }

		// Token: 0x170004EF RID: 1263
		// (get) Token: 0x06001065 RID: 4197 RVA: 0x00065A6F File Offset: 0x00063C6F
		// (set) Token: 0x06001066 RID: 4198 RVA: 0x00065A77 File Offset: 0x00063C77
		public Feature ObjectDetection { get; set; }

		// Token: 0x170004F0 RID: 1264
		// (get) Token: 0x06001067 RID: 4199 RVA: 0x00065A80 File Offset: 0x00063C80
		// (set) Token: 0x06001068 RID: 4200 RVA: 0x00065A98 File Offset: 0x00063C98
		public bool AdvancedFiltersVisible
		{
			get
			{
				return this.advancedFiltersVisible;
			}
			set
			{
				this.advancedFiltersVisible = value;
				this.OnPropertyChanged("AdvancedFiltersVisible");
			}
		}

		// Token: 0x170004F1 RID: 1265
		// (get) Token: 0x06001069 RID: 4201 RVA: 0x00065AB0 File Offset: 0x00063CB0
		// (set) Token: 0x0600106A RID: 4202 RVA: 0x00065AC8 File Offset: 0x00063CC8
		public bool MultiProductVisible
		{
			get
			{
				return this.multiProductVisible;
			}
			set
			{
				this.multiProductVisible = value;
				this.OnPropertyChanged("MultiProductVisible");
			}
		}

		// Token: 0x170004F2 RID: 1266
		// (get) Token: 0x0600106B RID: 4203 RVA: 0x00065AE0 File Offset: 0x00063CE0
		// (set) Token: 0x0600106C RID: 4204 RVA: 0x00065AF8 File Offset: 0x00063CF8
		public bool MultiLaneProductVisible
		{
			get
			{
				return this.multiLaneProductVisible;
			}
			set
			{
				this.multiLaneProductVisible = value;
				this.OnPropertyChanged("MultiLaneProductVisible");
			}
		}

		// Token: 0x170004F3 RID: 1267
		// (get) Token: 0x0600106D RID: 4205 RVA: 0x00065B10 File Offset: 0x00063D10
		// (set) Token: 0x0600106E RID: 4206 RVA: 0x00065B28 File Offset: 0x00063D28
		public bool ZoneAnalyserVisible
		{
			get
			{
				return this.zoneAnalyserVisible;
			}
			set
			{
				this.zoneAnalyserVisible = value;
				this.OnPropertyChanged("ZoneAnalyserVisible");
			}
		}

		// Token: 0x170004F4 RID: 1268
		// (get) Token: 0x0600106F RID: 4207 RVA: 0x00065B40 File Offset: 0x00063D40
		// (set) Token: 0x06001070 RID: 4208 RVA: 0x00065B58 File Offset: 0x00063D58
		public bool MultiLaneViewVisible
		{
			get
			{
				return this.multiLaneViewVisible;
			}
			set
			{
				this.multiLaneViewVisible = value;
				this.OnPropertyChanged("MultiLaneViewVisible");
			}
		}

		// Token: 0x170004F5 RID: 1269
		// (get) Token: 0x06001071 RID: 4209 RVA: 0x00065B70 File Offset: 0x00063D70
		// (set) Token: 0x06001072 RID: 4210 RVA: 0x00065B88 File Offset: 0x00063D88
		public bool SensPredAndComplModeVisible
		{
			get
			{
				return this.sensPredAndComplModeVisible;
			}
			set
			{
				this.sensPredAndComplModeVisible = value;
				this.OnPropertyChanged("SensPredAndComplModeVisible");
			}
		}

		// Token: 0x170004F6 RID: 1270
		// (get) Token: 0x06001073 RID: 4211 RVA: 0x00065BA0 File Offset: 0x00063DA0
		// (set) Token: 0x06001074 RID: 4212 RVA: 0x00065BB8 File Offset: 0x00063DB8
		public bool BulkObjectProductsVisible
		{
			get
			{
				return this.bulkObjectProductsVisible;
			}
			set
			{
				this.bulkObjectProductsVisible = value;
				this.OnPropertyChanged("BulkObjectProductsVisible");
			}
		}

		// Token: 0x170004F7 RID: 1271
		// (get) Token: 0x06001075 RID: 4213 RVA: 0x00065BD0 File Offset: 0x00063DD0
		// (set) Token: 0x06001076 RID: 4214 RVA: 0x00065BE8 File Offset: 0x00063DE8
		public bool ObjectBasedFlapsEnabled
		{
			get
			{
				return this.objectBasedFlapsEnabled;
			}
			set
			{
				bool flag = this.objectBasedFlapsEnabled != value;
				if (flag)
				{
					this.parHandling.WriteValue(Definition.objectBasedFlapsEnabledPath, value ? "1" : "0");
					this.objectBasedFlapsEnabled = value;
					bool flag2 = !this.objectBasedFlapsEnabled;
					if (flag2)
					{
						this.parHandling.WriteValue(Definition.objectBasedFlapsCountPath, "1");
					}
				}
				this.OnPropertyChanged("ObjectBasedFlapsEnabled");
			}
		}

		// Token: 0x170004F8 RID: 1272
		// (get) Token: 0x06001077 RID: 4215 RVA: 0x00065C60 File Offset: 0x00063E60
		// (set) Token: 0x06001078 RID: 4216 RVA: 0x00065C78 File Offset: 0x00063E78
		public bool HighThroughputModeVisible
		{
			get
			{
				return this.highThroughputModeVisible;
			}
			set
			{
				this.highThroughputModeVisible = value;
				this.OnPropertyChanged("HighThroughputModeVisible");
				this.UpdateOtherFeaturesVisible();
			}
		}

		// Token: 0x170004F9 RID: 1273
		// (get) Token: 0x06001079 RID: 4217 RVA: 0x00065C98 File Offset: 0x00063E98
		// (set) Token: 0x0600107A RID: 4218 RVA: 0x00065CB0 File Offset: 0x00063EB0
		public bool AnomalyDetectionVisible
		{
			get
			{
				return this.anomalyDetectionVisible;
			}
			set
			{
				this.anomalyDetectionVisible = value;
				this.OnPropertyChanged("AnomalyDetectionVisible");
				this.UpdateOtherFeaturesVisible();
			}
		}

		// Token: 0x170004FA RID: 1274
		// (get) Token: 0x0600107B RID: 4219 RVA: 0x00065CD0 File Offset: 0x00063ED0
		// (set) Token: 0x0600107C RID: 4220 RVA: 0x00065CE8 File Offset: 0x00063EE8
		public bool ObjectDetectionVisible
		{
			get
			{
				return this.objectDetectionVisible;
			}
			set
			{
				this.objectDetectionVisible = value;
				this.OnPropertyChanged("ObjectDetectionVisible");
				this.UpdateOtherFeaturesVisible();
			}
		}

		// Token: 0x170004FB RID: 1275
		// (get) Token: 0x0600107D RID: 4221 RVA: 0x00065D08 File Offset: 0x00063F08
		// (set) Token: 0x0600107E RID: 4222 RVA: 0x00065D20 File Offset: 0x00063F20
		public bool OtherFeaturesVisible
		{
			get
			{
				return this.otherFeaturesVisible;
			}
			set
			{
				this.otherFeaturesVisible = value;
				this.OnPropertyChanged("OtherFeaturesVisible");
			}
		}

		// Token: 0x0600107F RID: 4223 RVA: 0x00065D36 File Offset: 0x00063F36
		private void UpdateOtherFeaturesVisible()
		{
			this.OtherFeaturesVisible = (this.HighThroughputModeVisible || this.AnomalyDetectionVisible || this.ObjectDetectionVisible);
		}

		// Token: 0x170004FC RID: 1276
		// (get) Token: 0x06001080 RID: 4224 RVA: 0x00065D59 File Offset: 0x00063F59
		// (set) Token: 0x06001081 RID: 4225 RVA: 0x00065D61 File Offset: 0x00063F61
		public RelayCommand ShowObjectBasedFlapsCommand { get; set; }

		// Token: 0x06001082 RID: 4226 RVA: 0x00065D6C File Offset: 0x00063F6C
		public FeatureSelectionViewModel(DBConnection dbConnection, ParameterHandling parHandling, TestcardSimService testcardSimService)
		{
			this.parHandling = parHandling;
			this.dbConnection = dbConnection;
			this.testcardSimService = testcardSimService;
			MachineType machineType = MachineType.MachineTypeUndefined;
			MachineMode machineMode = MachineMode.Standard;
			bool enabled = false;
			bool enabled2 = false;
			bool enabled3 = false;
			bool enabled4 = false;
			bool enabled5 = false;
			bool enabled6 = false;
			bool flag = false;
			bool flag2 = false;
			bool enabled7 = false;
			bool enabled8 = false;
			bool enabled9 = false;
			bool enabled10 = false;
			dbConnection.GetMachineType(ref machineType);
			dbConnection.GetMachineMode(ref machineMode);
			this.advancedFiltersVisible = (machineType != MachineType.RayconXRS && machineMode != MachineMode.Bulk && machineMode != MachineMode.BulkObject);
			dbConnection.GetAdvancedFilters(ref enabled);
			dbConnection.GetMultiProductsEnabled(ref enabled2);
			dbConnection.GetMultiLaneProductsEnabled(ref enabled3);
			dbConnection.GetMultiLaneView(ref enabled4);
			dbConnection.GetMultiRegionEnabled(ref enabled5);
			dbConnection.GetTestcardSimulation(ref enabled6);
			dbConnection.GetIntegratedValidationProcess(ref flag);
			dbConnection.GetBulkObjectEnabled(ref flag2);
			bool flag3 = flag2;
			if (flag3)
			{
				this.ObjectBasedFlapsEnabled = true;
			}
			else
			{
				this.ObjectBasedFlapsEnabled = false;
			}
			dbConnection.GetHighThroughputMachine(ref enabled8);
			dbConnection.GetAnomalyDetectionEnabled(ref enabled9);
			dbConnection.GetObjectDetectionEnabled(ref enabled10);
			dbConnection.GetObjectDetectionEnabled(ref enabled10);
			string description = "Completeness check, weight analysis, clip masking, product deformation";
			this.AdvancedFilters = new Feature(parHandling, description, enabled, Definition.advancedFiltersPath);
			bool flag4 = machineType != MachineType.RayconEntry;
			if (flag4)
			{
				this.MultiProduct = new Feature(parHandling, "Multi product", enabled2, Definition.multiProductsEnablePath);
				this.MultiProduct.PropertyChanged += this.Feature_PropertyChanged;
				this.MultiLaneProduct = new Feature(parHandling, "Multi lane product", enabled3, Definition.multiLaneProductsPath);
				this.MultiLaneProduct.PropertyChanged += this.Feature_PropertyChanged;
				this.MultiLaneInspection = new Feature(parHandling, "Multi lane inspection", enabled4, Definition.multiLaneViewPath);
				this.MultiLaneInspection.PropertyChanged += this.Feature_PropertyChanged;
				this.ZoneAnalyser = new Feature(parHandling, "Zone analyzer", enabled5, Definition.multiRegionPath);
				this.ZoneAnalyser.PropertyChanged += this.Feature_PropertyChanged;
				this.SensPredAndComplMode = new Feature(parHandling, "Sensitivity Prediction, Compliance Mode", enabled6, Definition.testcardSimulationPath);
				this.SensPredAndComplMode.PropertyChanged += this.Feature_PropertyChanged;
				this.BulkObjectProducts = new Feature(parHandling, "Bulk Object products", flag2, Definition.bulkObjectProductsPath);
				this.BulkObjectProducts.PropertyChanged += this.Feature_PropertyChanged;
				this.ObjectBasedFlapsFeature = new Feature(parHandling, "Object based reject", enabled7, Definition.objectBasedFlapsEnabledPath);
				this.ObjectBasedFlapsFeature.PropertyChanged += this.Feature_PropertyChanged;
				this.AnomalyDetection = new Feature(parHandling, "THiNK XA", enabled9, Definition.anomalyDetectionEnabled);
				this.AnomalyDetection.PropertyChanged += this.Feature_PropertyChanged;
				this.ObjectDetection = new Feature(parHandling, "THiNK XO", enabled10, Definition.objectDetectionEnabled);
				this.ObjectDetection.PropertyChanged += this.Feature_PropertyChanged;
			}
			this.HighThroughputMode = new Feature(parHandling, "High Throughput Mode", enabled8, Definition.highThroughputMachine);
			this.HighThroughputMode.PropertyChanged += this.Feature_PropertyChanged;
			this.SetVisibilities(machineType, machineMode);
			this.ShowObjectBasedFlapsCommand = new RelayCommand(new Action<object>(this.ShowObjectBasedFlaps));
		}

		// Token: 0x06001083 RID: 4227 RVA: 0x000660A4 File Offset: 0x000642A4
		private void Feature_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			MachineType machineType = MachineType.MachineTypeUndefined;
			MachineMode machineMode = MachineMode.Standard;
			this.dbConnection.GetMachineType(ref machineType);
			this.dbConnection.GetMachineMode(ref machineMode);
			this.parHandling.WriteValue(Definition.multiProductViewPath, (this.MultiProduct.Enabled || this.ZoneAnalyser.Enabled) ? "1" : "0");
			this.SetVisibilities(machineType, machineMode);
			bool enabled = this.MultiLaneProduct.Enabled;
			if (enabled)
			{
				this.MultiLaneInspection.Enabled = true;
			}
			bool flag = !this.MultiLaneInspection.Enabled;
			if (flag)
			{
				this.setToOneLane();
			}
			this.parHandling.WriteValue(Definition.testcardSimulationPath, this.SensPredAndComplMode.Enabled ? "1" : "0");
			this.writeToActProdAndDefault(Definition.simulationAfterAutolearn, this.SensPredAndComplMode.Enabled ? "1" : "0");
			this.parHandling.WriteValue(Autotrain.nAutolearnPath, this.SensPredAndComplMode.Enabled ? "10" : "5");
			bool flag2 = !this.SensPredAndComplMode.Enabled;
			if (flag2)
			{
				this.parHandling.WriteValue(Definition.iCMPath, "0");
			}
			this.testcardSimService.SetProperties();
			bool enabled2 = this.BulkObjectProducts.Enabled;
			if (enabled2)
			{
				this.ObjectBasedFlapsEnabled = true;
			}
			else
			{
				this.ObjectBasedFlapsEnabled = false;
			}
		}

		// Token: 0x06001084 RID: 4228 RVA: 0x0006621C File Offset: 0x0006441C
		private void setToOneLane()
		{
			string path = Definition.detectorSesotecNPixelPath;
			DetectorType detectorType = DetectorType.Sesotec;
			this.dbConnection.GetDetectorType(ref detectorType);
			bool flag = detectorType == DetectorType.DTNetDE || detectorType == DetectorType.DTNetTDI;
			if (flag)
			{
				path = Definition.detectorDTNPixelPath;
			}
			int num = Convert.ToInt32(this.parHandling.ReadPar(path));
			bool flag2 = num > 0;
			if (flag2)
			{
				num--;
			}
			this.writeToActProdAndDefault("/filter/acquisition/lane1/Enable/Value", "1");
			this.writeToActProdAndDefault("/filter/acquisition/lane2/Enable/Value", "0");
			this.writeToActProdAndDefault("/filter/acquisition/lane3/Enable/Value", "0");
			this.writeToActProdAndDefault("/filter/acquisition/lane4/Enable/Value", "0");
			this.writeToActProdAndDefault("/filter/acquisition/lane1/MinPixel/Value", "0");
			this.writeToActProdAndDefault("/filter/acquisition/lane2/MinPixel/Value", "0");
			this.writeToActProdAndDefault("/filter/acquisition/lane3/MinPixel/Value", "0");
			this.writeToActProdAndDefault("/filter/acquisition/lane4/MinPixel/Value", "0");
			this.writeToActProdAndDefault("/filter/acquisition/lane1/MaxPixel/Value", Convert.ToString(num));
			this.writeToActProdAndDefault("/filter/acquisition/lane2/MaxPixel/Value", Convert.ToString(num));
			this.writeToActProdAndDefault("/filter/acquisition/lane3/MaxPixel/Value", Convert.ToString(num));
			this.writeToActProdAndDefault("/filter/acquisition/lane4/MaxPixel/Value", Convert.ToString(num));
		}

		// Token: 0x06001085 RID: 4229 RVA: 0x00066347 File Offset: 0x00064547
		private void writeToActProdAndDefault(string path, string val)
		{
			this.parHandling.WriteValue(path, val);
			this.parHandling.WriteValue("/defaultProduct/mainTable" + path, val);
		}

		// Token: 0x06001086 RID: 4230 RVA: 0x00066370 File Offset: 0x00064570
		private void SetVisibilities(MachineType machineType, MachineMode machineMode)
		{
			DetectorType detectorType = DetectorType.DTNetDE;
			this.dbConnection.GetDetectorType(ref detectorType);
			string a = "";
			this.dbConnection.GetXRaySourceModelNumber(ref a);
			string a2 = this.parHandling.ReadPar(Definition.detectorSesotecPixelSizePath);
			bool flag = machineType != MachineType.RayconEntry;
			if (flag)
			{
				bool flag2 = machineMode != MachineMode.Bulk && machineMode != MachineMode.BulkObject;
				if (flag2)
				{
					this.SensPredAndComplModeVisible = (machineType == MachineType.RayconDPlus && ((detectorType == DetectorType.Sesotec && a2 == "0.4") || detectorType == DetectorType.DTNetTDI) && a != "IXS050SE100P214" && !this.MultiLaneInspection.Enabled && !this.MultiLaneProduct.Enabled && !this.MultiProduct.Enabled && !this.ZoneAnalyser.Enabled && !this.HighThroughputMode.Enabled);
					string a3 = this.parHandling.ReadPar(Definition.imgprocarchitecture);
					this.AnomalyDetectionVisible = (a3 == "1" && !this.MultiProduct.Enabled && !this.MultiLaneProduct.Enabled && !this.ZoneAnalyser.Enabled && !this.HighThroughputMode.Enabled);
					this.ObjectDetectionVisible = this.AnomalyDetectionVisible;
					bool flag3 = this.SensPredAndComplModeVisible || this.AnomalyDetectionVisible;
					if (flag3)
					{
						bool flag4 = this.SensPredAndComplModeVisible;
						if (flag4)
						{
							this.MultiLaneInspection.Enabled = false;
						}
						this.MultiLaneProduct.Enabled = false;
						this.MultiProduct.Enabled = false;
						this.ZoneAnalyser.Enabled = false;
						this.HighThroughputMode.Enabled = false;
						this.setVisibilityOfSpecialProducts(!this.SensPredAndComplMode.Enabled && !this.AnomalyDetection.Enabled && !this.ObjectDetection.Enabled);
						this.MultiLaneViewVisible = !this.SensPredAndComplMode.Enabled;
					}
					else
					{
						this.MultiProductVisible = (this.MultiProduct.Enabled || (!this.MultiLaneProduct.Enabled && !this.ZoneAnalyser.Enabled));
						this.MultiLaneProductVisible = (this.MultiLaneProduct.Enabled || (!this.MultiProduct.Enabled && !this.ZoneAnalyser.Enabled));
						this.MultiLaneViewVisible = true;
						bool flag5 = this.MultiProduct.Enabled && detectorType == DetectorType.DTNetDE;
						if (flag5)
						{
							this.MultiLaneInspection.Enabled = false;
							this.MultiLaneProduct.Enabled = false;
							this.MultiLaneProductVisible = false;
							this.MultiLaneViewVisible = false;
						}
						bool flag6 = detectorType != DetectorType.DTNetDE;
						if (flag6)
						{
							this.ZoneAnalyserVisible = (this.ZoneAnalyser.Enabled || (!this.MultiProduct.Enabled && !this.MultiLaneProduct.Enabled));
						}
						this.HighThroughputModeVisible = true;
					}
				}
				else
				{
					this.BulkObjectProductsVisible = true;
				}
			}
			else
			{
				this.setVisibilityOfSpecialProducts(false);
				this.AnomalyDetectionVisible = false;
				this.SensPredAndComplModeVisible = false;
				this.HighThroughputModeVisible = true;
				this.ObjectDetectionVisible = false;
			}
		}

		// Token: 0x06001087 RID: 4231 RVA: 0x000666AD File Offset: 0x000648AD
		private void setVisibilityOfSpecialProducts(bool enable)
		{
			this.MultiLaneProductVisible = enable;
			this.MultiLaneViewVisible = enable;
			this.MultiProductVisible = enable;
			this.ZoneAnalyserVisible = enable;
			this.HighThroughputModeVisible = enable;
		}

		// Token: 0x06001088 RID: 4232 RVA: 0x000666D8 File Offset: 0x000648D8
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x06001089 RID: 4233 RVA: 0x00066708 File Offset: 0x00064908
		private void ShowObjectBasedFlaps(object obj)
		{
			CtrlComissioningObjectBasedFlaps ctrlComissioningObjectBasedFlaps = new CtrlComissioningObjectBasedFlaps(this.dbConnection, this.parHandling);
			Subwindow.Show(ctrlComissioningObjectBasedFlaps, SubwindowButtons.Back, ctrlComissioningObjectBasedFlaps.PanelSubButtons);
		}

		// Token: 0x04000AEE RID: 2798
		private bool advancedFiltersVisible;

		// Token: 0x04000AEF RID: 2799
		private ParameterHandling parHandling;

		// Token: 0x04000AF0 RID: 2800
		private DBConnection dbConnection;

		// Token: 0x04000AF1 RID: 2801
		private TestcardSimService testcardSimService;

		// Token: 0x04000AF2 RID: 2802
		private bool multiProductVisible;

		// Token: 0x04000AF3 RID: 2803
		private bool multiLaneProductVisible;

		// Token: 0x04000AF4 RID: 2804
		private bool zoneAnalyserVisible;

		// Token: 0x04000AF5 RID: 2805
		private bool multiLaneViewVisible;

		// Token: 0x04000AF6 RID: 2806
		private bool sensPredAndComplModeVisible;

		// Token: 0x04000AF7 RID: 2807
		private bool bulkObjectProductsVisible;

		// Token: 0x04000B04 RID: 2820
		private bool objectBasedFlapsEnabled;

		// Token: 0x04000B05 RID: 2821
		private bool highThroughputModeVisible;

		// Token: 0x04000B06 RID: 2822
		private bool anomalyDetectionVisible;

		// Token: 0x04000B07 RID: 2823
		private bool objectDetectionVisible;

		// Token: 0x04000B08 RID: 2824
		private bool otherFeaturesVisible;
	}
}
