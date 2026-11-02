using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using DatabaseConnection;
using Definitions;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x02000195 RID: 405
	public class Autotrain : INotifyPropertyChanged
	{
		// Token: 0x17000AF7 RID: 2807
		// (get) Token: 0x06002678 RID: 9848 RVA: 0x00104D58 File Offset: 0x00102F58
		// (set) Token: 0x06002679 RID: 9849 RVA: 0x00104D70 File Offset: 0x00102F70
		public int NAutotrain
		{
			get
			{
				return this.nAutotrain;
			}
			set
			{
				this.nAutotrain = value;
				bool flag = this.nAutotrain > 0;
				if (flag)
				{
					this.AutotrainText = string.Concat(new string[]
					{
						this.preAutotrainText,
						" ",
						this.NAutotrain.ToString(),
						" ",
						this.postAutotrainText
					});
				}
				else
				{
					this.AutotrainText = LocalizationManager.Localize("txtAfterAutotrain");
					LogbookManager.AddEntry(SSTEntryCode.AutotrainEnded);
					bool flag2 = this.machineState.State == MachineStates.Autotrain;
					if (flag2)
					{
						this.machineState.State = this.oldState;
					}
				}
			}
		}

		// Token: 0x17000AF8 RID: 2808
		// (get) Token: 0x0600267A RID: 9850 RVA: 0x00104E20 File Offset: 0x00103020
		// (set) Token: 0x0600267B RID: 9851 RVA: 0x00104E38 File Offset: 0x00103038
		public string AutotrainText
		{
			get
			{
				return this.autotrainText;
			}
			set
			{
				this.autotrainText = value;
				this.OnPropertyChanged("AutotrainText");
			}
		}

		// Token: 0x17000AF9 RID: 2809
		// (get) Token: 0x0600267C RID: 9852 RVA: 0x00104E50 File Offset: 0x00103050
		// (set) Token: 0x0600267D RID: 9853 RVA: 0x00104E68 File Offset: 0x00103068
		public string ProductText
		{
			get
			{
				return this.productText;
			}
			set
			{
				this.productText = value;
				this.OnPropertyChanged("ProductText");
			}
		}

		// Token: 0x17000AFA RID: 2810
		// (get) Token: 0x0600267E RID: 9854 RVA: 0x00104E80 File Offset: 0x00103080
		// (set) Token: 0x0600267F RID: 9855 RVA: 0x00104E98 File Offset: 0x00103098
		public ProductManager ProductManager
		{
			get
			{
				return this.productManager;
			}
			set
			{
				this.productManager = value;
				this.OnPropertyChanged("ProductManager");
			}
		}

		// Token: 0x17000AFB RID: 2811
		// (get) Token: 0x06002680 RID: 9856 RVA: 0x00104EB0 File Offset: 0x001030B0
		// (set) Token: 0x06002681 RID: 9857 RVA: 0x00104EC8 File Offset: 0x001030C8
		public XRSImage Image
		{
			get
			{
				return this.image;
			}
			set
			{
				this.image = value;
				this.OnPropertyChanged("Image");
			}
		}

		// Token: 0x17000AFC RID: 2812
		// (get) Token: 0x06002682 RID: 9858 RVA: 0x00104EE0 File Offset: 0x001030E0
		// (set) Token: 0x06002683 RID: 9859 RVA: 0x00104EF8 File Offset: 0x001030F8
		public bool AdjustingKV
		{
			get
			{
				return this.adjustingKV;
			}
			set
			{
				bool flag = this.adjustingKV != value;
				if (flag)
				{
					this.adjustingKV = value;
					this.OnPropertyChanged("AdjustingKV");
				}
			}
		}

		// Token: 0x140000CE RID: 206
		// (add) Token: 0x06002684 RID: 9860 RVA: 0x00104F2C File Offset: 0x0010312C
		// (remove) Token: 0x06002685 RID: 9861 RVA: 0x00104F64 File Offset: 0x00103164
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x140000CF RID: 207
		// (add) Token: 0x06002686 RID: 9862 RVA: 0x00104F9C File Offset: 0x0010319C
		// (remove) Token: 0x06002687 RID: 9863 RVA: 0x00104FD4 File Offset: 0x001031D4
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action AutotrainFinished;

		// Token: 0x140000D0 RID: 208
		// (add) Token: 0x06002688 RID: 9864 RVA: 0x0010500C File Offset: 0x0010320C
		// (remove) Token: 0x06002689 RID: 9865 RVA: 0x00105044 File Offset: 0x00103244
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action SendTestcardSimImages;

		// Token: 0x0600268A RID: 9866 RVA: 0x0010507C File Offset: 0x0010327C
		public Autotrain(DBConnection dbConnection, ParameterHandling parHandling, ImageDataTransfer imageTransfer, int nAutotrain, AutolearnType type, MachineState machineState, ProductManager productManager, int learnedGroup, XrayState xrayState, bool noAutoKVAdjustment, bool autokVChangedToDisabled, TestcardSimService testcardSimService, XRSImageBox imagebox, Sensitivity bulksensitivity)
		{
			this.nAutotrainStart = nAutotrain;
			this.NAutotrain = nAutotrain;
			this.parHandling = parHandling;
			this.dbConnection = dbConnection;
			this.imageTransfer = imageTransfer;
			this.type = type;
			this.machineState = machineState;
			this.productManager = productManager;
			this.learnedGroup = learnedGroup;
			this.xrayState = xrayState;
			this.noAutoKVAdjustment = noAutoKVAdjustment;
			this.testcardSimService = testcardSimService;
			this.imagebox = imagebox;
			this.bulksensitivity = bulksensitivity;
			this.oldState = machineState.State;
			this.numAdjustedKv = 0;
			this.progBarWaitForImage = true;
			machineState.State = MachineStates.Autotrain;
			this.preAutotrainText = LocalizationManager.Localize("txtPreAutotrain");
			this.postAutotrainText = LocalizationManager.Localize("txtPostAutotrain");
			this.AutotrainText = string.Concat(new string[]
			{
				this.preAutotrainText,
				" ",
				this.NAutotrain.ToString(),
				" ",
				this.postAutotrainText
			});
			bool isMultiProduct = productManager.CurrentLoadedProduct.IsMultiProduct;
			if (isMultiProduct)
			{
				this.ProductText = "P " + productManager.SelectedSubProduct.ProductNumber + ": " + productManager.SelectedSubProduct.ProductName;
			}
			else
			{
				this.ProductText = "P " + productManager.CurrentLoadedProduct.ProductNumber + ": " + productManager.CurrentLoadedProduct.ProductName;
			}
			dbConnection.GetMachineMode(ref this.machineMode);
			bool flag = this.machineMode == MachineMode.Bulk;
			if (flag)
			{
				this.ActivateAutolearnForBulkMode();
				this.SetHighThresholdsForComparerAndThresholds();
				Thread.Sleep(1000);
			}
			imageTransfer.FrameArrived += this.imageTransfer_FrameArrived;
			imageTransfer.FrameArrived += this.imageTestcardSimTransfer_FrameArrived;
			this.CreateWeigherPaths();
			MachineType machinetype = MachineType.RayconXRS;
			dbConnection.GetMachineType(ref machinetype);
			this.EnableComparerIfNecessary(machinetype);
			this.SwitchLDParamsIfNecessary();
			this.unmaskForContEnabled = Filter.CountAndUnmaskForContEnabled(parHandling);
			this.StartTrain();
			this.DisableHLSC();
			bool flag2 = type == AutolearnType.Contamination;
			if (flag2)
			{
				LogbookManager.AddEntry(SSTEntryCode.AutotrainStarted, 0);
			}
			else
			{
				bool flag3 = type == AutolearnType.Weight;
				if (flag3)
				{
					LogbookManager.AddEntry(SSTEntryCode.AutotrainStarted, 1);
				}
			}
			bool flag4 = type == AutolearnType.Contamination && !this.noAutoKVAdjustment && !autokVChangedToDisabled && (productManager.CurrentLoadedProduct.IsSingleProduct || productManager.CurrentLoadedProduct.IsRegionProduct);
			if (flag4)
			{
				double num = 50.0;
				this.kvAdjustment = new KvAdjustment(parHandling);
				xrayState.CalibrationDone += this.xrayState_CalibrationDone;
				this.adjustingKV = this.kvAdjustment.AdjustmentNeeded(num);
				bool flag5 = this.adjustingKV;
				if (flag5)
				{
					this.progBarWaitForImage = false;
					machineState.State = MachineStates.AutoAdjustingXRay;
					parHandling.WriteValue(XRayOnProcessing.manualGainCalibrationPath, "0", false);
					this.kvAdjustment.AdjustSettings(num, 0, true);
					this.AutotrainText = this.GetKvAdjustmentText();
					this.ProductText = "";
				}
				machineState.PropertyChanged += this.machineState_PropertyChanged;
			}
			this.dualEnergy = new DualEnergy(imageTransfer, parHandling);
			this.dualEnergy.PropertyChanged += this.dualEnergy_PropertyChanged;
			this.dualEnergy.StartBusyCheck();
			this.demomode = false;
			dbConnection.GetDemoMode(ref this.demomode);
			bool flag6 = type == AutolearnType.Contamination;
			if (flag6)
			{
				this.StartCheckImageInProcessForProgBar();
			}
			this.timerCheckErrorTestcardSim = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, new EventHandler(this.checkErrorsTestcardSim), Application.Current.Dispatcher);
			this.timerCheckErrorTestcardSim.Stop();
			this.timerCounterTestcardSim = 0;
			bool flag7 = testcardSimService != null;
			if (flag7)
			{
				testcardSimService.MonitoringSensIsActive = false;
			}
		}

		// Token: 0x0600268B RID: 9867 RVA: 0x00105474 File Offset: 0x00103674
		private void DisableHLSC()
		{
			this.parHandling.WriteValue(Definition.msSensorChecksEnable1Path, "0", false);
			this.parHandling.WriteValue(Definition.msSensorChecksEnable2Path, "0", false);
		}

		// Token: 0x0600268C RID: 9868 RVA: 0x001054A8 File Offset: 0x001036A8
		private void ResetHLSCEnable()
		{
			string value = "";
			string value2 = "";
			this.dbConnection.GetParameterValue(Definition.msSensorChecksEnable1Path, ref value);
			this.dbConnection.GetParameterValue(Definition.msSensorChecksEnable2Path, ref value2);
			this.parHandling.WriteValue(Definition.msSensorChecksEnable1Path, value, false);
			this.parHandling.WriteValue(Definition.msSensorChecksEnable2Path, value2, false);
		}

		// Token: 0x0600268D RID: 9869 RVA: 0x00105510 File Offset: 0x00103710
		public void EnableComparerIfNecessary(MachineType machinetype)
		{
			bool flag = this.type == AutolearnType.Contamination;
			if (flag)
			{
				bool flag2 = false;
				this.dbConnection.GetDemoMode(ref flag2);
				DetectorType detectorType = DetectorType.Unknown;
				this.dbConnection.GetDetectorType(ref detectorType);
				string a = "";
				bool flag3 = detectorType == DetectorType.Sesotec;
				if (flag3)
				{
					a = this.parHandling.ReadPar(Definition.detectorSesotecPixelSizePath);
				}
				else
				{
					bool flag4 = detectorType == DetectorType.DTNetTDI;
					if (flag4)
					{
						a = this.parHandling.ReadPar(Definition.detectorDtPixelSizePath);
					}
				}
				bool flag5 = machinetype == MachineType.RayconDPlus || machinetype == MachineType.RayconEX2A;
				bool flag6 = flag2 && a == "0.4";
				if (flag6)
				{
					flag5 = true;
				}
				bool flag7 = flag5;
				if (flag7)
				{
					bool flag8 = !this.productManager.CurrentLoadedProduct.IsRegionProduct;
					if (flag8)
					{
						Filter.EnableComparerFilter(this.parHandling, true, this.learnedGroup);
					}
					else
					{
						for (int i = 0; i < this.productManager.CurrentLoadedProduct.SubProducts.Count; i++)
						{
							Filter.EnableComparerFilter(this.parHandling, true, (int)this.productManager.CurrentLoadedProduct.SubProducts[i].Group);
						}
					}
					MachineMode machineMode = MachineMode.Standard;
					this.dbConnection.GetMachineMode(ref machineMode);
					bool flag9 = machinetype == MachineType.RayconDPlus && machineMode == MachineMode.Bulk;
					if (flag9)
					{
						for (int j = 0; j < Definition.nComparer; j++)
						{
							this.parHandling.WriteValue(Definition.filterMainPath + "/comparer/comparer" + (j + 1).ToString() + "/ContrastMin/Value", "50");
						}
					}
				}
				else
				{
					bool flag10 = !this.productManager.CurrentLoadedProduct.IsRegionProduct;
					if (flag10)
					{
						Filter.EnableComparerFilter(this.parHandling, false, this.learnedGroup);
					}
					else
					{
						for (int k = 0; k < this.productManager.CurrentLoadedProduct.SubProducts.Count; k++)
						{
							Filter.EnableComparerFilter(this.parHandling, false, (int)this.productManager.CurrentLoadedProduct.SubProducts[k].Group);
						}
					}
				}
			}
		}

		// Token: 0x0600268E RID: 9870 RVA: 0x00105748 File Offset: 0x00103948
		public void SwitchLDParamsIfNecessary()
		{
			bool flag = this.type == AutolearnType.Contamination;
			if (flag)
			{
				DetectorType detectorType = DetectorType.Unknown;
				this.dbConnection.GetDetectorType(ref detectorType);
				string a = "";
				bool flag2 = detectorType == DetectorType.Sesotec;
				if (flag2)
				{
					a = this.parHandling.ReadPar(Definition.detectorSesotecPixelSizePath);
				}
				else
				{
					bool flag3 = detectorType == DetectorType.DTNetTDI;
					if (flag3)
					{
						a = this.parHandling.ReadPar(Definition.detectorDtPixelSizePath);
					}
				}
				bool flag4 = (detectorType == DetectorType.Sesotec || detectorType == DetectorType.DTNetTDI) && a == "0.4" && this.parHandling.ReadPar(Acquisition.binningPath) == "1";
				if (flag4)
				{
					bool flag5 = !this.productManager.CurrentLoadedProduct.IsRegionProduct;
					if (flag5)
					{
						Filter.SwitchLowDensityParameter(this.parHandling, true, this.learnedGroup);
					}
					else
					{
						for (int i = 0; i < this.productManager.CurrentLoadedProduct.SubProducts.Count; i++)
						{
							Filter.SwitchLowDensityParameter(this.parHandling, true, (int)this.productManager.CurrentLoadedProduct.SubProducts[i].Group);
						}
					}
				}
				else
				{
					bool flag6 = !this.productManager.CurrentLoadedProduct.IsRegionProduct;
					if (flag6)
					{
						Filter.SwitchLowDensityParameter(this.parHandling, false, this.learnedGroup);
					}
					else
					{
						for (int j = 0; j < this.productManager.CurrentLoadedProduct.SubProducts.Count; j++)
						{
							Filter.SwitchLowDensityParameter(this.parHandling, false, (int)this.productManager.CurrentLoadedProduct.SubProducts[j].Group);
						}
					}
				}
			}
		}

		// Token: 0x0600268F RID: 9871 RVA: 0x00105904 File Offset: 0x00103B04
		private void SetWireComparerSize(ParameterHandling parHandling, int group)
		{
			string str = (group > 0) ? group.ToString() : "";
			DetectorType detectorType = DetectorType.Unknown;
			this.dbConnection.GetDetectorType(ref detectorType);
			string a = "";
			bool flag = detectorType == DetectorType.Sesotec;
			if (flag)
			{
				a = parHandling.ReadPar(Definition.detectorSesotecPixelSizePath);
			}
			else
			{
				bool flag2 = detectorType == DetectorType.DTNetTDI;
				if (flag2)
				{
					a = parHandling.ReadPar(Definition.detectorDtPixelSizePath);
				}
			}
			bool flag3 = (detectorType == DetectorType.Sesotec || detectorType == DetectorType.DTNetTDI) && a == "0.4";
			bool flag4 = parHandling.ReadPar(Definition.detectorTypePath) == "2";
			if (flag4)
			{
				parHandling.WriteValue(Definition.filterMainPath + str + "/comparer/comparer9/Matches/Value", "5");
				parHandling.WriteValue(Definition.filterMainPath + str + "/comparer/comparer10/Matches/Value", "5");
				parHandling.WriteValue(Definition.filterMainPath + str + "/comparer/comparer9/Radius/Value", "1");
				parHandling.WriteValue(Definition.filterMainPath + str + "/comparer/comparer9/MinSize/Value", "2");
				parHandling.WriteValue(Definition.filterMainPath + str + "/comparer/comparer10/Radius/Value", "3");
				parHandling.WriteValue(Definition.filterMainPath + str + "/comparer/comparer10/MinSize/Value", "4");
			}
			bool flag5 = flag3;
			if (flag5)
			{
				parHandling.WriteValue(Definition.filterMainPath + str + "/comparer/comparer9/Matches/Value", "5");
				parHandling.WriteValue(Definition.filterMainPath + str + "/comparer/comparer10/Matches/Value", "5");
				parHandling.WriteValue(Definition.filterMainPath + str + "/comparer/comparer9/Radius/Value", "2");
				parHandling.WriteValue(Definition.filterMainPath + str + "/comparer/comparer9/MinSize/Value", "3");
				parHandling.WriteValue(Definition.filterMainPath + str + "/comparer/comparer10/Radius/Value", "4");
				parHandling.WriteValue(Definition.filterMainPath + str + "/comparer/comparer10/MinSize/Value", "8");
			}
		}

		// Token: 0x06002690 RID: 9872 RVA: 0x00105B04 File Offset: 0x00103D04
		public void StartTrain()
		{
			AutolearnType autolearnType = this.type;
			AutolearnType autolearnType2 = autolearnType;
			if (autolearnType2 != AutolearnType.Contamination)
			{
				if (autolearnType2 == AutolearnType.Weight)
				{
					foreach (string path in this.autolearnWeightPaths)
					{
						this.parHandling.WriteValue(path, this.NAutotrain.ToString(), false);
					}
				}
			}
			else
			{
				bool flag = !this.productManager.CurrentLoadedProduct.IsRegionProduct;
				if (flag)
				{
					Filter.WriteToImageProcessingFilters(this.parHandling, Autotrain.strAutolearn, this.NAutotrain.ToString(), this.learnedGroup);
					bool flag2 = this.unmaskForContEnabled;
					if (flag2)
					{
						Filter.SaveDefaultParamsForAutolearn(this.parHandling, this.learnedGroup, this.dbConnection);
					}
				}
				else
				{
					for (int j = 0; j < this.productManager.CurrentLoadedProduct.SubProducts.Count; j++)
					{
						Filter.WriteToImageProcessingFilters(this.parHandling, Autotrain.strAutolearn, this.NAutotrain.ToString(), (int)this.productManager.CurrentLoadedProduct.SubProducts[j].Group);
					}
				}
			}
		}

		// Token: 0x06002691 RID: 9873 RVA: 0x00105C3C File Offset: 0x00103E3C
		public void AbortTrain(bool aborted)
		{
			AutolearnType autolearnType = this.type;
			AutolearnType autolearnType2 = autolearnType;
			if (autolearnType2 != AutolearnType.Contamination)
			{
				if (autolearnType2 == AutolearnType.Weight)
				{
					foreach (string path in this.autolearnWeightPaths)
					{
						this.parHandling.WriteValue(path, "0", false);
					}
				}
			}
			else
			{
				bool flag = !this.productManager.CurrentLoadedProduct.IsRegionProduct;
				if (flag)
				{
					Filter.WriteToImageProcessingFilters(this.parHandling, Autotrain.strAutolearn, "0", this.learnedGroup);
				}
				else
				{
					for (int j = 0; j < this.productManager.CurrentLoadedProduct.SubProducts.Count; j++)
					{
						Filter.WriteToImageProcessingFilters(this.parHandling, Autotrain.strAutolearn, "0", (int)this.productManager.CurrentLoadedProduct.SubProducts[j].Group);
					}
				}
			}
			bool flag2 = this.productManager.CurrentLoadedProduct.IsRegionProduct && this.productManager.CurrentLoadedProduct.SubProducts.Count > 2;
			if (flag2)
			{
				Progress.ShowStaticProgress(LocalizationManager.Localize("txtProcessing"), false);
				new Thread(delegate()
				{
					this.WriteParamsToDatabase(aborted);
					Progress.CloseStaticProgress(false);
				})
				{
					Name = "WriteParamsToDB"
				}.Start();
			}
			else
			{
				this.WriteParamsToDatabase(aborted);
			}
			this.parHandling.WriteValue(Definition.recognitionNLearnPath, "0", false);
			bool aborted2 = aborted;
			if (aborted2)
			{
				this.AutotrainText = LocalizationManager.Localize("txtAbortAutotrain");
				LogbookManager.AddEntry(SSTEntryCode.AutotrainAborted);
			}
			this.dualEnergy.StopBusyCheck();
			bool flag3 = this.machineState.State == MachineStates.Autotrain;
			if (flag3)
			{
				this.machineState.State = this.oldState;
			}
			bool flag4 = this.progresstestcardSim != null && this.parHandling.ReadPar(Definition.testcardSimulationPath) == "1" && this.parHandling.ReadPar(Definition.simulationAfterAutolearn) == "1";
			if (flag4)
			{
				this.progresstestcardSim.Close();
				this.timerCheckErrorTestcardSim.Stop();
			}
			MachineMode machineMode = MachineMode.Standard;
			this.dbConnection.GetMachineMode(ref machineMode);
			bool flag5 = machineMode == MachineMode.Bulk;
			if (flag5)
			{
				this.ReactivateBulkModeAfterAutotrain();
			}
		}

		// Token: 0x06002692 RID: 9874 RVA: 0x00105EBC File Offset: 0x001040BC
		private void WriteParamsToDatabase(bool aborted)
		{
			List<string> list = new List<string>();
			AutolearnType autolearnType = this.type;
			AutolearnType autolearnType2 = autolearnType;
			if (autolearnType2 != AutolearnType.Contamination)
			{
				if (autolearnType2 == AutolearnType.Weight)
				{
					foreach (string path in this.weigherPaths)
					{
						list.Add(this.parHandling.ReadPar(path));
					}
					bool flag = this.learnedGroup <= 0;
					if (flag)
					{
						this.dbConnection.UpdateCurrentProductTable(this.weigherPaths.ToList<string>(), list);
					}
					else
					{
						string str = null;
						this.dbConnection.GetCurrentProductTableName(ref str);
						this.dbConnection.UpdateTable(str + Definition.subTableIdentifier.ToString() + this.learnedGroup.ToString(), this.weigherPaths.ToList<string>(), list);
					}
					this.productManager.UpdateAssignedProductsFilterPaths(this.weigherPaths.ToList<string>(), list, this.learnedGroup);
				}
			}
			else
			{
				bool flag2 = !this.productManager.CurrentLoadedProduct.IsRegionProduct;
				if (flag2)
				{
					Filter.UpdateImageProcessingFilterPaths(this.dbConnection, this.parHandling, this.learnedGroup, this.productManager, !aborted);
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
					int[] array2 = new int[this.productManager.CurrentLoadedProduct.SubProducts.Count];
					for (int j = 0; j < this.productManager.CurrentLoadedProduct.SubProducts.Count; j++)
					{
						array2[j] = (int)this.productManager.CurrentLoadedProduct.SubProducts[j].Group;
					}
					Filter.UpdateImageProcessingFilterPathsForRegionProd(this.dbConnection, this.parHandling, array2, this.productManager);
				}
			}
		}

		// Token: 0x06002693 RID: 9875 RVA: 0x0010612C File Offset: 0x0010432C
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x06002694 RID: 9876 RVA: 0x0010615C File Offset: 0x0010435C
		public void CleanUp()
		{
			this.imageTransfer.FrameArrived -= this.imageTransfer_FrameArrived;
			this.imageTransfer.FrameArrived -= this.imageTestcardSimTransfer_FrameArrived;
			this.timerCheckErrorTestcardSim.Stop();
			bool flag = !this.noAutoKVAdjustment;
			if (flag)
			{
				this.xrayState.CalibrationDone -= this.xrayState_CalibrationDone;
			}
			this.ResetHLSCEnable();
			this.machineState.PropertyChanged -= this.machineState_PropertyChanged;
			this.StopCheckImageInProcessForProgBar();
		}

		// Token: 0x06002695 RID: 9877 RVA: 0x001061F4 File Offset: 0x001043F4
		private void CreateWeigherPaths()
		{
			string str = (this.learnedGroup > 0) ? this.learnedGroup.ToString() : "";
			this.autolearnWeightPaths = new string[]
			{
				Definition.filterMainPath + str + "/weigher/Autolearn/Value"
			};
			this.weigherPaths = new string[]
			{
				Definition.filterMainPath + str + "/weigher/MinWeight/Value",
				Definition.filterMainPath + str + "/weigher/MaxWeight/Value",
				Definition.filterMainPath + str + "/weigher/ExpectedWeight/Value"
			};
		}

		// Token: 0x06002696 RID: 9878 RVA: 0x00106284 File Offset: 0x00104484
		public void imageTransfer_FrameArrived(object sender, ReceiveEventArgs e)
		{
			bool flag = this.progBarWaitForImage && this.type == AutolearnType.Contamination && (this.machineState.State == MachineStates.AutoAdjustingXRay || this.machineState.State == MachineStates.Autotrain);
			if (flag)
			{
				this.StopCheckImageInProcessForProgBar();
			}
			bool flag2 = this.machineMode == MachineMode.Bulk;
			if (flag2)
			{
				bool flag3 = e.Image.State != 16 || (int)e.Image.Height < BulkMode.linesForAutotrain;
				if (flag3)
				{
					return;
				}
			}
			bool flag4 = (this.learnedGroup == 0 || this.learnedGroup == (int)e.Image.Group || this.productManager.CurrentLoadedProduct.IsRegionProduct) && e.Image.Type != ImageType.TestcardSimulation;
			if (flag4)
			{
				bool flag5 = !this.adjustingKV && e.Image.Type != ImageType.Histogram && e.Image.Type != ImageType.MaterialCurve;
				if (flag5)
				{
					Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
					{
						bool flag20 = this.type == AutolearnType.Weight;
						if (flag20)
						{
							XRSImage copiedImage = e.Image.DeepCopy();
							this.productManager.PrepareRegionImage(copiedImage, this.learnedGroup);
							this.Image = copiedImage;
						}
						else
						{
							this.Image = e.Image;
						}
					}));
					bool flag6 = this.type == AutolearnType.Contamination;
					if (flag6)
					{
						bool flag7 = this.learnedGroup == 0 || this.learnedGroup == (int)e.Image.Group;
						if (flag7)
						{
							bool flag8 = this.noAutoKVAdjustment || this.nAutotrain <= 2 || this.kvAdjustment.CheckAdjustment(e.Image);
							if (flag8)
							{
								int nautotrain = this.NAutotrain;
								this.NAutotrain = nautotrain - 1;
								this.UnmaskForContUndoAutolearn(e.Image);
								bool flag9 = this.NAutotrain >= 1;
								if (flag9)
								{
									this.StartCheckImageInProcessForProgBar();
								}
							}
							else
							{
								bool flag10 = this.numAdjustedKv < 1 && this.NAutotrain == this.nAutotrainStart;
								if (flag10)
								{
									this.progBarWaitForImage = false;
									Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
									{
										this.AdjustingKV = true;
									}));
									this.parHandling.WriteValue(XRayOnProcessing.manualGainCalibrationPath, "0", false);
									this.kvAdjustment.AdjustSettings(this.image);
									this.progBarWaitForImage = true;
									this.NAutotrain = this.nAutotrainStart;
									this.StartTrain();
									Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
									{
										this.AutotrainText = this.GetKvAdjustmentText();
										this.ProductText = "";
									}));
									this.numAdjustedKv++;
								}
								else
								{
									int nautotrain = this.NAutotrain;
									this.NAutotrain = nautotrain - 1;
									this.UnmaskForContUndoAutolearn(e.Image);
									bool flag11 = this.NAutotrain >= 1;
									if (flag11)
									{
										this.StartCheckImageInProcessForProgBar();
									}
								}
							}
						}
					}
					else
					{
						bool flag12 = this.type == AutolearnType.Weight;
						if (flag12)
						{
							bool flag13 = !this.productManager.CurrentLoadedProduct.IsRegionProduct;
							if (flag13)
							{
								bool flag14 = this.learnedGroup == 0 || this.learnedGroup == (int)e.Image.Group;
								if (flag14)
								{
									int nautotrain = this.NAutotrain;
									this.NAutotrain = nautotrain - 1;
								}
							}
							else
							{
								bool flag15 = e.Image.Group == 0;
								if (flag15)
								{
									int nautotrain = this.NAutotrain;
									this.NAutotrain = nautotrain - 1;
								}
							}
						}
					}
					bool flag16 = this.nAutotrain <= 0;
					if (flag16)
					{
						bool flag17 = this.parHandling.ReadPar(Definition.testcardSimulationPath) == "1" && this.parHandling.ReadPar(Definition.simulationAfterAutolearn) == "1" && this.type == AutolearnType.Contamination && this.parHandling.ReadPar(Definition.autoKVAdjustmentPath) == "1";
						if (flag17)
						{
							this.OnSendSimTesctardImages();
							this.timerCheckErrorTestcardSim.Start();
							this.imageTransfer.FrameArrived -= this.imageTransfer_FrameArrived;
						}
						else
						{
							bool flag18 = this.machineMode == MachineMode.Bulk;
							if (flag18)
							{
								bool flag19 = this.bulksensitivity != null;
								if (flag19)
								{
									this.bulksensitivity.calculateBulkSliderContrasts = false;
									this.bulksensitivity.SensitivityPercent = 100;
								}
								this.ReactivateBulkModeAfterAutotrain();
							}
							this.OnAutotrainFinished();
						}
					}
				}
			}
		}

		// Token: 0x06002697 RID: 9879 RVA: 0x00106728 File Offset: 0x00104928
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
			bool flag3 = this.timerCounterTestcardSim > 180;
			if (flag3)
			{
				this.progresstestcardSim.Close();
				this.timerCheckErrorTestcardSim.Stop();
				Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtAbortTestcardSim"), DialogStyle.Error, DialogButtons.OK);
			}
			this.timerCounterTestcardSim++;
		}

		// Token: 0x06002698 RID: 9880 RVA: 0x0010682C File Offset: 0x00104A2C
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

		// Token: 0x06002699 RID: 9881 RVA: 0x00106868 File Offset: 0x00104A68
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
							this.testcardSimService.setParameterAfterTestcardSim(true);
							bool flag4 = Dialog.Show(LocalizationManager.Localize("txtInfo"), this.testcardSimService.setAutotrainTestcardSimMsg(), DialogStyle.Info, DialogButtons.OK) == Dialog.DialogResults.Ok;
							if (flag4)
							{
								bool checkSensitivityDecrease = this.testcardSimService.CheckSensitivityDecrease;
								if (checkSensitivityDecrease)
								{
									Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtDecreasesTestcardSim"), DialogStyle.Warning, DialogButtons.OK);
								}
								this.OnAutotrainFinished();
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

		// Token: 0x0600269A RID: 9882 RVA: 0x001068D0 File Offset: 0x00104AD0
		protected virtual void OnAutotrainFinished()
		{
			Action autotrainFinished = this.AutotrainFinished;
			bool flag = autotrainFinished != null;
			if (flag)
			{
				autotrainFinished();
			}
		}

		// Token: 0x0600269B RID: 9883 RVA: 0x001068F8 File Offset: 0x00104AF8
		protected virtual void OnSendSimTesctardImages()
		{
			Action sendTestcardSimImages = this.SendTestcardSimImages;
			bool flag = sendTestcardSimImages != null;
			if (flag)
			{
				sendTestcardSimImages();
			}
		}

		// Token: 0x0600269C RID: 9884 RVA: 0x0010691F File Offset: 0x00104B1F
		private void xrayState_CalibrationDone()
		{
			this.machineState.State = MachineStates.Autotrain;
			this.progBarWaitForImage = true;
			Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(delegate()
			{
				this.AdjustingKV = false;
				this.AutotrainText = string.Concat(new string[]
				{
					this.preAutotrainText,
					" ",
					this.NAutotrain.ToString(),
					" ",
					this.postAutotrainText
				});
				this.ProductText = "P " + this.productManager.CurrentLoadedProduct.ProductNumber + ": " + this.productManager.CurrentLoadedProduct.ProductName;
			}));
			this.StartCheckImageInProcessForProgBar();
		}

		// Token: 0x0600269D RID: 9885 RVA: 0x0010695C File Offset: 0x00104B5C
		private string GetKvAdjustmentText()
		{
			return string.Concat(new string[]
			{
				this.txtAdjustingKV,
				Environment.NewLine,
				"(",
				this.kvAdjustment.CurrentKV.ToString(),
				" kV, ",
				Math.Round(this.kvAdjustment.CurrentMA, 2).ToString(),
				" mA)"
			});
		}

		// Token: 0x0600269E RID: 9886 RVA: 0x001069D4 File Offset: 0x00104BD4
		private void dualEnergy_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			bool isBusy = this.dualEnergy.IsBusy;
			if (isBusy)
			{
				bool flag = this.nAutotrain == 1;
				if (flag)
				{
					Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
					{
						Progress.ShowStaticProgress(LocalizationManager.Localize("txtProcessing"), false);
					}));
				}
			}
			else
			{
				Progress.CloseStaticProgress(false);
			}
		}

		// Token: 0x0600269F RID: 9887 RVA: 0x00106A3C File Offset: 0x00104C3C
		private void machineState_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			bool flag = this.machineState.State == MachineStates.ConveyorOnKeyClosedXrayOff || this.machineState.State == MachineStates.ConveyorOnKeyOpened || this.machineState.State == MachineStates.ConveyorOff;
			if (flag)
			{
				this.machineState.PropertyChanged -= this.machineState_PropertyChanged;
				Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
				{
					Dialog.Show(LocalizationManager.Localize("txtInfo"), LocalizationManager.Localize("txtTurnOnXray"), DialogStyle.Info);
					this.OnAutotrainFinished();
				}));
			}
		}

		// Token: 0x060026A0 RID: 9888 RVA: 0x00106AB4 File Offset: 0x00104CB4
		public void StartCheckImageInProcessForProgBar()
		{
			this.StopCheckImageInProcessForProgBar();
			bool flag = !this.checkImageInProcessIsRunning;
			if (flag)
			{
				this.checkImageInProcessIsRunning = true;
				this.thUpdateProg = new Thread(new ThreadStart(this.CheckImageInProcess));
				this.thUpdateProg.Name = "Update ProgImageInProcess";
				this.thUpdateProg.Start();
			}
		}

		// Token: 0x060026A1 RID: 9889 RVA: 0x00106B14 File Offset: 0x00104D14
		public void StopCheckImageInProcessForProgBar()
		{
			this.checkImageInProcessIsRunning = false;
			bool flag = Application.Current != null && this.progCheckImageInProcess != null;
			if (flag)
			{
				Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate()
				{
					this.progCheckImageInProcess.Close();
				}));
			}
			bool flag2 = this.thUpdateProg != null;
			if (flag2)
			{
				this.thUpdateProg.Join();
			}
		}

		// Token: 0x060026A2 RID: 9890 RVA: 0x00106B78 File Offset: 0x00104D78
		private void CheckImageInProcess()
		{
			bool checkprogress = true;
			int progBarAutotrain = 0;
			int countProgress = 0;
			int sleepTimeout = 200;
			int cancelTimeout = 60000;
			Action <>9__0;
			while (this.checkImageInProcessIsRunning)
			{
				bool flag = false;
				int num = 0;
				int.TryParse(this.parHandling.ReadPar(Definition.imageInProcessPath), out num);
				bool flag2 = num == 1;
				if (flag2)
				{
					flag = true;
				}
				bool flag3 = flag;
				if (flag3)
				{
					bool flag4 = Application.Current != null;
					if (flag4)
					{
						Dispatcher dispatcher = Application.Current.Dispatcher;
						DispatcherPriority priority = DispatcherPriority.Background;
						Action method;
						if ((method = <>9__0) == null)
						{
							method = (<>9__0 = delegate()
							{
								bool checkprogress = checkprogress;
								if (checkprogress)
								{
									checkprogress = false;
									this.progCheckImageInProcess = new Progress(LocalizationManager.Localize("txtProcessing"), false);
									this.progCheckImageInProcess.Show();
								}
								progBarAutotrain += 5;
								bool flag6 = progBarAutotrain > 100;
								if (flag6)
								{
									progBarAutotrain = 0;
								}
								this.progCheckImageInProcess.progBar.Value = (double)progBarAutotrain;
								bool flag7 = countProgress * sleepTimeout > cancelTimeout;
								if (flag7)
								{
									this.checkImageInProcessIsRunning = false;
									this.progCheckImageInProcess.Close();
								}
								bool flag8 = !this.demomode && this.machineState.State != MachineStates.Autotrain && this.machineState.State != MachineStates.AutoAdjustingXRay;
								if (flag8)
								{
									this.checkImageInProcessIsRunning = false;
									this.progCheckImageInProcess.Close();
								}
							});
						}
						dispatcher.Invoke(priority, method);
					}
				}
				Thread.Sleep(sleepTimeout);
				int countProgress2 = countProgress;
				countProgress = countProgress2 + 1;
				bool flag5 = !this.checkImageInProcessIsRunning;
				if (flag5)
				{
					break;
				}
			}
		}

		// Token: 0x060026A3 RID: 9891 RVA: 0x00106C74 File Offset: 0x00104E74
		private void ActivateAutolearnForBulkMode()
		{
			BulkMode.ActivateObjectModeAutotrain(this.dbConnection, this.parHandling);
			this.parHandling.WriteValue("/filter/acquisition/SendImage/Value", "1");
			this.parHandling.WriteValue("/filter/acquisition/SendOnlyIfContaminant/Value", "0");
		}

		// Token: 0x060026A4 RID: 9892 RVA: 0x00106CC0 File Offset: 0x00104EC0
		private void ReactivateBulkModeAfterAutotrain()
		{
			BulkMode.DeactivateObjectModeAutotrain(this.dbConnection, this.parHandling);
		}

		// Token: 0x060026A5 RID: 9893 RVA: 0x00106CD8 File Offset: 0x00104ED8
		private void SetHighThresholdsForComparerAndThresholds()
		{
			int num = Convert.ToInt32(this.parHandling.ReadPar(Definition.backroundNoisePath));
			int value = Definition.Background10000 - num * 100;
			for (int i = 0; i < Definition.nComparerHXBulkNuvo; i++)
			{
				this.parHandling.WriteValue(Definition.filterMainPath + "/comparer/comparer" + (i + 1).ToString() + "/HighThreshold/Value", Convert.ToString(value));
			}
			this.parHandling.WriteValue(Definition.filterMainPath + Definition.thresholdSubPath + "/HighThreshold/Value", Convert.ToString(value));
			this.parHandling.WriteValue(Definition.filterMainPath + Definition.thresholdGlobalSubPath + "/HighThreshold/Value", Convert.ToString(value));
		}

		// Token: 0x060026A6 RID: 9894 RVA: 0x00106D9C File Offset: 0x00104F9C
		private void UnmaskForContUndoAutolearn(XRSImage image)
		{
			bool flag = !this.unmaskForContEnabled;
			if (!flag)
			{
				bool flag2 = image.HasFalseCount();
				if (flag2)
				{
					Filter.SetPreviousLearnedParams(this.parHandling);
				}
				else
				{
					bool flag3 = !image.HasFalseCount();
					if (flag3)
					{
						Filter.SaveActualLearnedParams(this.parHandling, this.learnedGroup);
					}
				}
			}
		}

		// Token: 0x04001899 RID: 6297
		public static readonly string strAutolearn = "Autolearn";

		// Token: 0x0400189A RID: 6298
		public static readonly string nAutolearnPath = "/machine/NAutolearn/Value";

		// Token: 0x0400189B RID: 6299
		public static readonly string nAutolearnDefaultPath = "/machine/NAutolearnDefault/Value";

		// Token: 0x0400189C RID: 6300
		public static readonly string nAutolearnDefaultBulkPath = "/machine/NAutolearnDefaultBulk/Value";

		// Token: 0x0400189D RID: 6301
		private string[] autolearnWeightPaths;

		// Token: 0x0400189E RID: 6302
		private string[] weigherPaths;

		// Token: 0x0400189F RID: 6303
		private string preAutotrainText;

		// Token: 0x040018A0 RID: 6304
		private string postAutotrainText;

		// Token: 0x040018A1 RID: 6305
		private ParameterHandling parHandling;

		// Token: 0x040018A2 RID: 6306
		private DBConnection dbConnection;

		// Token: 0x040018A3 RID: 6307
		private ImageDataTransfer imageTransfer;

		// Token: 0x040018A4 RID: 6308
		private string autotrainText;

		// Token: 0x040018A5 RID: 6309
		private string productText;

		// Token: 0x040018A6 RID: 6310
		private AutolearnType type;

		// Token: 0x040018A7 RID: 6311
		private MachineState machineState;

		// Token: 0x040018A8 RID: 6312
		private MachineStates oldState;

		// Token: 0x040018A9 RID: 6313
		private int nAutotrain;

		// Token: 0x040018AA RID: 6314
		private int nAutotrainStart;

		// Token: 0x040018AB RID: 6315
		private ProductManager productManager;

		// Token: 0x040018AC RID: 6316
		private int learnedGroup;

		// Token: 0x040018AD RID: 6317
		private XRSImage image;

		// Token: 0x040018AE RID: 6318
		private XrayState xrayState;

		// Token: 0x040018AF RID: 6319
		private KvAdjustment kvAdjustment;

		// Token: 0x040018B0 RID: 6320
		private bool noAutoKVAdjustment;

		// Token: 0x040018B1 RID: 6321
		private TestcardSimService testcardSimService;

		// Token: 0x040018B2 RID: 6322
		private bool adjustingKV;

		// Token: 0x040018B3 RID: 6323
		private int numAdjustedKv;

		// Token: 0x040018B4 RID: 6324
		private const int numAdjustedKvMax = 1;

		// Token: 0x040018B5 RID: 6325
		private string txtAdjustingKV = LocalizationManager.Localize("txtAdjustingXRayVoltage") + "...";

		// Token: 0x040018B6 RID: 6326
		private DualEnergy dualEnergy;

		// Token: 0x040018B7 RID: 6327
		private bool checkImageInProcessIsRunning;

		// Token: 0x040018B8 RID: 6328
		private Progress progCheckImageInProcess;

		// Token: 0x040018B9 RID: 6329
		private bool progBarWaitForImage;

		// Token: 0x040018BA RID: 6330
		private Thread thUpdateProg;

		// Token: 0x040018BB RID: 6331
		private bool demomode;

		// Token: 0x040018BC RID: 6332
		private Progress progresstestcardSim;

		// Token: 0x040018BD RID: 6333
		private int testcardSimProgressCounter;

		// Token: 0x040018BE RID: 6334
		private DispatcherTimer timerCheckErrorTestcardSim;

		// Token: 0x040018BF RID: 6335
		private int timerCounterTestcardSim;

		// Token: 0x040018C0 RID: 6336
		private Sensitivity bulksensitivity;

		// Token: 0x040018C1 RID: 6337
		private MachineMode machineMode = MachineMode.Standard;

		// Token: 0x040018C2 RID: 6338
		private bool unmaskForContEnabled = false;

		// Token: 0x040018C3 RID: 6339
		private XRSImageBox imagebox;
	}
}
