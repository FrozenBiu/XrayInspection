using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using DatabaseConnection;
using Definitions;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x020000D3 RID: 211
	public class AIService
	{
		// Token: 0x1400006A RID: 106
		// (add) Token: 0x06001489 RID: 5257 RVA: 0x00079764 File Offset: 0x00077964
		// (remove) Token: 0x0600148A RID: 5258 RVA: 0x0007979C File Offset: 0x0007799C
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action PropertiesSet;

		// Token: 0x0600148B RID: 5259 RVA: 0x000797D1 File Offset: 0x000779D1
		public AIService(ParameterHandling parHandling, ImageDataTransfer imageTransfer, StoreImageData imageArchiver, DBConnection dbConnection, ProductManager productManager, ModelTransfer modelTransfer, ErrorSurvey errorSurvey)
		{
			this.parHandling = parHandling;
			this.imageTransfer = imageTransfer;
			this.imageArchiver = imageArchiver;
			this.dbConnection = dbConnection;
			this.productManager = productManager;
			this.modelTransfer = modelTransfer;
			this.errorSurvey = errorSurvey;
		}

		// Token: 0x0600148C RID: 5260 RVA: 0x00079810 File Offset: 0x00077A10
		public void CreateDefaultTableAI(bool setDefaultValues)
		{
			bool flag = this.parHandling.ReadPar(Definition.imgprocarchitecture) == "1";
			bool flag2 = !TableOperations.TableExists(this.dbConnection.Connection, DBConnection.defaultApplicationAI) && flag;
			if (flag2)
			{
				if (setDefaultValues)
				{
					this.parHandling.WriteValue(Definition.anomalyDetectionTrainEnabled, "0");
					this.parHandling.WriteValue(Definition.anomalyDetectionTrainingInProgress, "0");
					this.parHandling.WriteValue(Definition.anomalyDetectionTrainCurrentEpoch, "0");
					this.parHandling.WriteValue(Definition.anomalyDetectionInferenceEnabled, "0");
					this.dbConnection.SetProductTableNameTrainingAI("");
					for (int i = 0; i < Definition.nAIPostproFilters; i++)
					{
						string str = string.Format("{0}{1}{2}{3}{4}", new object[]
						{
							Definition.filterMainPath,
							Definition.anomalyDetectionMain,
							Definition.anomalyDetectionInference,
							Definition.aipostproFilterSubPath,
							i + 1
						});
						this.parHandling.WriteValue(str + Definition.anomalyDetectionInferenceThresholdGroupContour + "/" + Definition.strValue, "0");
						this.parHandling.WriteValue(str + Definition.anomalyDetectionInferenceThresholdGroupInside + "/" + Definition.strValue, "0");
						this.parHandling.WriteValue(str + Definition.anomalyDetectionInferenceThresholdReconErrorContour + "/" + Definition.strValue, "0");
						this.parHandling.WriteValue(str + Definition.anomalyDetectionInferenceThresholdReconErrorInside + "/" + Definition.strValue, "0");
					}
				}
				List<string> list = new List<string>();
				List<string> parValues = new List<string>();
				string machineSubType = this.parHandling.ReadPar(Definition.machineSubTypePath);
				list = this.parHandling.GetPathesAI();
				parValues = this.parHandling.GetValuesAI(list);
				this.dbConnection.CreateAppDefaultAI(list, parValues, machineSubType, setDefaultValues);
			}
		}

		// Token: 0x0600148D RID: 5261 RVA: 0x00079A18 File Offset: 0x00077C18
		public void CreateDefaultTableAIBgWorker()
		{
			bool flag = this.parHandling.ReadPar(Definition.imgprocarchitecture) == "1";
			bool flag2 = !TableOperations.TableExists(this.dbConnection.Connection, DBConnection.defaultApplicationAI) && flag;
			if (flag2)
			{
				string description = LocalizationManager.Localize("txtAICreateDefaultTable") + "...";
				this.progressBgWorkerAIDefaultTable = new Progress(description, false);
				this.progressBgWorkerAIDefaultTable.Show();
				this.bgWorkerAIDefaultTable = new BackgroundWorker();
				this.bgWorkerAIDefaultTable.WorkerReportsProgress = true;
				this.bgWorkerAIDefaultTable.DoWork += this.bgWorkerAIDefaultTable_DoWork;
				this.bgWorkerAIDefaultTable.ProgressChanged += this.bgWorkerAIDefaultTable_ProgressChanged;
				this.bgWorkerAIDefaultTable.RunWorkerCompleted += this.bgWorkerAIDefaultTable_RunWorkerCompleted;
				this.bgWorkerAIDefaultTable.RunWorkerAsync();
			}
		}

		// Token: 0x0600148E RID: 5262 RVA: 0x00079B00 File Offset: 0x00077D00
		private void bgWorkerAIDefaultTable_DoWork(object sender, DoWorkEventArgs e)
		{
			Application.Current.Dispatcher.Invoke(delegate()
			{
			}, DispatcherPriority.Render);
			List<string> list = new List<string>();
			List<string> parValues = new List<string>();
			string machineSubType = this.parHandling.ReadPar(Definition.machineSubTypePath);
			list = this.parHandling.GetPathesAI();
			parValues = this.parHandling.GetValuesAI(list);
			this.bgWorkerAIDefaultTable.ReportProgress(50);
			this.dbConnection.CreateAppDefaultAI(list, parValues, machineSubType, true);
		}

		// Token: 0x0600148F RID: 5263 RVA: 0x00079B92 File Offset: 0x00077D92
		private void bgWorkerAIDefaultTable_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			this.progressBgWorkerAIDefaultTable.progBar.Value = (double)e.ProgressPercentage;
		}

		// Token: 0x06001490 RID: 5264 RVA: 0x00079BB0 File Offset: 0x00077DB0
		private void bgWorkerAIDefaultTable_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			this.progressBgWorkerAIDefaultTable.FormFadeOut.Begin();
			this.bgWorkerAIDefaultTable.DoWork -= this.bgWorkerAIDefaultTable_DoWork;
			this.bgWorkerAIDefaultTable.ProgressChanged -= this.bgWorkerAIDefaultTable_ProgressChanged;
			this.bgWorkerAIDefaultTable.RunWorkerCompleted -= this.bgWorkerAIDefaultTable_RunWorkerCompleted;
		}

		// Token: 0x06001491 RID: 5265 RVA: 0x00079C18 File Offset: 0x00077E18
		public void SendAnomalyDetectionTrainImages()
		{
			bool flag = this.parHandling.ReadPar(Definition.anomalyDetectionTrainEnabled) == "1" && this.parHandling.ReadPar(Definition.anomalyDetectionTrainSendImages) == "1";
			if (flag)
			{
				this.bgWorkerAISendImages = new BackgroundWorker();
				this.bgWorkerAISendImages.WorkerReportsProgress = false;
				this.bgWorkerAISendImages.WorkerSupportsCancellation = true;
				this.bgWorkerAISendImages.DoWork += this.bgWorkerAISendImages_DoWork;
				this.bgWorkerAISendImages.RunWorkerCompleted += this.bgWorkerAISendImages_RunWorkerCompleted;
				this.bgWorkerAISendImages.RunWorkerAsync();
			}
		}

		// Token: 0x06001492 RID: 5266 RVA: 0x00079CC8 File Offset: 0x00077EC8
		private void bgWorkerAISendImages_DoWork(object sender, DoWorkEventArgs e)
		{
			this.parHandling.WriteValue(Definition.anomalyDetectionTrainSendImages, "0");
			string text = "";
			string tableName = "";
			bool flag = this.parHandling.ReadPar(Definition.anomalyDetectionTrainModelID) == this.parHandling.ReadPar(Definition.anomalyDetectionInferenceModelID);
			bool flag2 = flag;
			string archiveDirectoryAnomalyDetection;
			if (flag2)
			{
				this.dbConnection.GetCurrentProductTableNameAI(ref tableName);
				archiveDirectoryAnomalyDetection = this.imageArchiver.GetArchiveDirectoryAnomalyDetection(tableName);
			}
			else
			{
				this.dbConnection.GetAppTableNameWithNumber(this.parHandling.ReadPar(Definition.anomalyDetectionTrainModelID), ref text);
				tableName = text.Insert("AppDefault".Length, "AI");
				archiveDirectoryAnomalyDetection = this.imageArchiver.GetArchiveDirectoryAnomalyDetection(tableName);
			}
			List<string> list = new List<string>();
			bool flag3 = true;
			bool flag4 = Directory.Exists(archiveDirectoryAnomalyDetection);
			if (flag4)
			{
				list.AddRange(Directory.GetFiles(archiveDirectoryAnomalyDetection).ToList<string>());
				foreach (string path in list)
				{
					bool flag5 = this.parHandling.ReadPar(XrayState.xrayPowerOnPath) == "1" || this.parHandling.ReadPar(XrayState.xrayStarterPath) == "1";
					if (flag5)
					{
						bool flag6 = Application.Current != null;
						if (flag6)
						{
							Application.Current.Dispatcher.Invoke(delegate()
							{
								Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("lbAnomalyDetectionStopXray"), DialogStyle.Error, DialogButtons.OK);
							});
						}
						this.parHandling.WriteValue(Definition.anomalyDetectionTrainEnabled, "0");
						string a = "";
						this.dbConnection.GetCurrentProductTableNameTrainingAI(ref a);
						bool flag7 = a != "";
						if (flag7)
						{
							this.dbConnection.SetProductTableNameTrainingAI("");
							this.parHandling.WriteValue(Definition.anomalyDetectionTrainFinished, "1");
						}
						flag3 = false;
						break;
					}
					XRSImage image = this.imageArchiver.GetImage(path);
					image.Type = ImageType.TrainAI;
					this.imageTransfer.SendImage(image);
				}
			}
			bool flag8 = flag3 && (list.Count > 0 || this.parHandling.ReadPar(Definition.anomalyDetectionNumberImagesFineTuning) != "0");
			if (flag8)
			{
				bool flag9 = !flag;
				if (flag9)
				{
					string param = "";
					this.dbConnection.GetProductName(text, ref param);
					this.errorSurvey.PostMessage(SSTEntryCode.AIDetectionActivated, 2, this.parHandling.ReadPar(Definition.anomalyDetectionTrainModelID), param, "", "");
				}
				else
				{
					this.errorSurvey.PostMessage(SSTEntryCode.AIDetectionActivated, 2, this.productManager.CurrentLoadedProduct.ProductNumber, this.productManager.CurrentLoadedProduct.ProductName, "", "");
				}
			}
		}

		// Token: 0x06001493 RID: 5267 RVA: 0x00079FEC File Offset: 0x000781EC
		private void bgWorkerAISendImages_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			this.bgWorkerAISendImages.DoWork -= this.bgWorkerAISendImages_DoWork;
			this.bgWorkerAISendImages.RunWorkerCompleted -= this.bgWorkerAISendImages_RunWorkerCompleted;
		}

		// Token: 0x06001494 RID: 5268 RVA: 0x0007A020 File Offset: 0x00078220
		public void SetAIModelID()
		{
			this.parHandling.WriteValue(Definition.anomalyDetectionInferenceModelID, this.productManager.CurrentLoadedProduct.ProductNumber);
			this.parHandling.WriteValue(Definition.objectDetectionInferenceModelID, this.productManager.CurrentLoadedProduct.ProductNumber);
			string text = "";
			this.dbConnection.GetCurrentProductTableNameTrainingAI(ref text);
			bool flag = this.parHandling.ReadPar(Definition.anomalyDetectionTrainingInProgress) == "0" && text == "";
			if (flag)
			{
				this.parHandling.WriteValue(Definition.anomalyDetectionTrainModelID, this.productManager.CurrentLoadedProduct.ProductNumber);
			}
			else
			{
				bool flag2 = text != "";
				if (flag2)
				{
					string text2 = "";
					this.dbConnection.GetProductNumber(text, ref text2);
					bool flag3 = text2 != "";
					if (flag3)
					{
						this.parHandling.WriteValue(Definition.anomalyDetectionTrainModelID, text2);
					}
				}
			}
		}

		// Token: 0x06001495 RID: 5269 RVA: 0x0007A120 File Offset: 0x00078320
		public void UpdateAIParametersAfterTrain()
		{
			bool flag = this.parHandling.ReadPar(Definition.anomalyDetectionTrainFinished) == "1";
			if (flag)
			{
				this.bgWorkerAIUpdateParam = new BackgroundWorker();
				this.bgWorkerAIUpdateParam.WorkerReportsProgress = false;
				this.bgWorkerAIUpdateParam.WorkerSupportsCancellation = true;
				this.bgWorkerAIUpdateParam.DoWork += this.bgWorkerAIUpdateParam_DoWork;
				this.bgWorkerAIUpdateParam.RunWorkerCompleted += this.bgWorkerAIUpdateParam_RunWorkerCompleted;
				this.bgWorkerAIUpdateParam.RunWorkerAsync();
			}
		}

		// Token: 0x06001496 RID: 5270 RVA: 0x0007A1B0 File Offset: 0x000783B0
		private void bgWorkerAIUpdateParam_DoWork(object sender, DoWorkEventArgs e)
		{
			this.parHandling.WriteValue(Definition.anomalyDetectionTrainFinished, "0");
			string a = this.parHandling.ReadPar(Definition.anomalyDetectionTrainCurrentEpoch);
			string text = "";
			this.dbConnection.GetCurrentProductTableNameTrainingAI(ref text);
			bool flag = a == "0" && text != "";
			if (flag)
			{
				Filter.UpdateAnomalyDetectionPathesAfterTraining(this.dbConnection, this.parHandling);
				string param = "";
				this.dbConnection.GetProductName(text, ref param);
				this.errorSurvey.PostMessage(SSTEntryCode.AIDetectionDeactivated, 2, this.parHandling.ReadPar(Definition.anomalyDetectionTrainModelID), param, "", "");
			}
			this.dbConnection.SetProductTableNameTrainingAI("");
			bool flag2 = this.parHandling.ReadPar(Definition.anomalyDetectionTrainModelID) != this.productManager.CurrentLoadedProduct.ProductNumber;
			if (flag2)
			{
				string tableName = "";
				this.dbConnection.GetCurrentProductTableNameAI(ref tableName);
				bool flag3 = TableOperations.TableExists(this.dbConnection.Connection, tableName);
				if (flag3)
				{
					List<string> list = new List<string>();
					List<string> list2 = new List<string>();
					this.dbConnection.GetProductEntries(tableName, list, list2);
					for (int i = 0; i < list.Count; i++)
					{
						bool flag4 = list[i].StartsWith("/filter/anomalyDetection/train");
						if (flag4)
						{
							this.parHandling.WriteValue(list[i], list2[i], false);
						}
					}
				}
				this.SetAIModelID();
			}
		}

		// Token: 0x06001497 RID: 5271 RVA: 0x0007A35D File Offset: 0x0007855D
		private void bgWorkerAIUpdateParam_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			this.bgWorkerAIUpdateParam.DoWork -= this.bgWorkerAIUpdateParam_DoWork;
			this.bgWorkerAIUpdateParam.RunWorkerCompleted -= this.bgWorkerAIUpdateParam_RunWorkerCompleted;
		}

		// Token: 0x06001498 RID: 5272 RVA: 0x0007A390 File Offset: 0x00078590
		public void UpdateAvailabilityModelFiles()
		{
			bool flag = this.productManager.CurrentLoadedProduct != null && this.modelTransfer != null;
			if (flag)
			{
				bool flag2 = this.parHandling.ReadPar(Definition.anomalyDetectionInferenceModelID) == "-1" || this.parHandling.ReadPar(Definition.objectDetectionInferenceModelID) == "-1";
				if (flag2)
				{
					this.SetAIModelID();
				}
				string tableName = "";
				this.dbConnection.GetCurrentProductTableNameAI(ref tableName);
				string archiveDirectory = ModelTransfer.GetArchiveDirectory(tableName);
				bool flag3 = !Directory.Exists(ModelTransfer.archiveRoot);
				if (flag3)
				{
					Directory.CreateDirectory(ModelTransfer.archiveRoot);
				}
				string path = Path.Combine(archiveDirectory, "model.engine");
				string path2 = Path.Combine(archiveDirectory, "model.onnx");
				this.parHandling.WriteValue(Definition.anomalyDetectionTensorRTEngineAvailable, File.Exists(path) ? "1" : "0", false);
				this.parHandling.WriteValue(Definition.anomalyDetectionOnnxAvailable, File.Exists(path2) ? "1" : "0", false);
				string path3 = "";
				string path4 = "";
				string text = this.parHandling.ReadPar(Definition.machineSubTypePath);
				bool flag4 = text.StartsWith("HX");
				if (flag4)
				{
					path3 = Path.Combine(archiveDirectory, "modelHX.weights");
					path4 = Path.Combine(archiveDirectory, "featureHX.weights");
				}
				else
				{
					bool flag5 = text.StartsWith("TX");
					if (flag5)
					{
						path3 = Path.Combine(archiveDirectory, "modelTX.weights");
						path4 = Path.Combine(archiveDirectory, "featureTX.weights");
					}
				}
				this.parHandling.WriteValue(Definition.anomalyDetectionWeightsInferenceAvailable, File.Exists(path3) ? "1" : "0", false);
				this.parHandling.WriteValue(Definition.anomalyDetectionResnetInferenceAvailable, File.Exists(path4) ? "1" : "0", false);
				string path5 = Path.Combine(archiveDirectory, "modelXO.engine");
				string path6 = Path.Combine(archiveDirectory, "modelXO.onnx");
				this.parHandling.WriteValue(Definition.objectDetectionTensorRTEngineAvailable, File.Exists(path5) ? "1" : "0", false);
				this.parHandling.WriteValue(Definition.objectDetectionOnnxAvailable, File.Exists(path6) ? "1" : "0", false);
				string text2 = "";
				this.dbConnection.GetCurrentProductTableNameTrainingAI(ref text2);
				bool flag6 = this.parHandling.ReadPar(Definition.anomalyDetectionTrainModelID) != this.parHandling.ReadPar(Definition.anomalyDetectionInferenceModelID) && text2 != "";
				if (flag6)
				{
					text2 = text2.Insert("AppDefault".Length, "AI");
					archiveDirectory = ModelTransfer.GetArchiveDirectory(text2);
					bool flag7 = text.StartsWith("HX");
					if (flag7)
					{
						path3 = Path.Combine(archiveDirectory, "modelHX.weights");
						path4 = Path.Combine(archiveDirectory, "featureHX.weights");
					}
					else
					{
						bool flag8 = text.StartsWith("TX");
						if (flag8)
						{
							path3 = Path.Combine(archiveDirectory, "modelTX.weights");
							path4 = Path.Combine(archiveDirectory, "featureTX.weights");
						}
					}
					this.parHandling.WriteValue(Definition.anomalyDetectionWeightsTrainAvailable, File.Exists(path3) ? "1" : "0", false);
					this.parHandling.WriteValue(Definition.anomalyDetectionResnetTrainAvailable, File.Exists(path4) ? "1" : "0", false);
				}
				else
				{
					this.parHandling.WriteValue(Definition.anomalyDetectionWeightsTrainAvailable, File.Exists(path3) ? "1" : "0", false);
					this.parHandling.WriteValue(Definition.anomalyDetectionResnetTrainAvailable, File.Exists(path4) ? "1" : "0", false);
				}
			}
		}

		// Token: 0x06001499 RID: 5273 RVA: 0x0007A754 File Offset: 0x00078954
		public void Comp19to30OnOff(bool value)
		{
			DetectorType detectorType = DetectorType.DT;
			this.dbConnection.GetDetectorType(ref detectorType);
			MachineType machineType = MachineType.MachineTypeUndefined;
			this.dbConnection.GetMachineType(ref machineType);
			string text = "";
			this.dbConnection.GetMachineSubType(ref text);
			bool flag = machineType == MachineType.RayconDPlus && text.Contains("HX") && detectorType != DetectorType.DTNetTDI;
			if (flag)
			{
				for (int i = 0; i < 12; i++)
				{
					this.parHandling.WriteValue(string.Concat(new string[]
					{
						Definition.filterMainPath,
						(this.productManager.SelectedProduct.Group > 0) ? this.productManager.SelectedProduct.Group.ToString() : "",
						Definition.comparerSubPath,
						(19 + i).ToString(),
						"/Enabled/",
						Definition.strValue
					}), value ? "1" : "0");
				}
			}
		}

		// Token: 0x0600149A RID: 5274 RVA: 0x0007A868 File Offset: 0x00078A68
		public bool CheckForFreeMemory()
		{
			string text = this.parHandling.ReadPar(Definition.machineSubTypePath);
			bool flag = text.StartsWith("TX");
			long num;
			if (flag)
			{
				num = 533725184L + (long)Definition.diskSpaceMinFreeMB;
			}
			else
			{
				num = 217055232L + (long)Definition.diskSpaceMinFreeMB;
			}
			string pathRoot = Path.GetPathRoot(Environment.CurrentDirectory);
			long totalFreeSpace = Disk.GetTotalFreeSpace(pathRoot);
			bool flag2 = totalFreeSpace < num + (long)Definition.diskSpaceMinFreeMB;
			bool result;
			if (flag2)
			{
				string message = LocalizationManager.Localize("lbMemoryError");
				Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
				{
					Dialog.Show(LocalizationManager.Localize("txtError"), message, DialogStyle.Error, DialogButtons.OK);
				}));
				result = false;
			}
			else
			{
				result = true;
			}
			return result;
		}

		// Token: 0x04000D96 RID: 3478
		private ParameterHandling parHandling;

		// Token: 0x04000D97 RID: 3479
		private DBConnection dbConnection;

		// Token: 0x04000D98 RID: 3480
		private ImageDataTransfer imageTransfer;

		// Token: 0x04000D99 RID: 3481
		private StoreImageData imageArchiver;

		// Token: 0x04000D9A RID: 3482
		private ProductManager productManager;

		// Token: 0x04000D9C RID: 3484
		private ModelTransfer modelTransfer;

		// Token: 0x04000D9D RID: 3485
		public ErrorSurvey errorSurvey;

		// Token: 0x04000D9E RID: 3486
		private BackgroundWorker bgWorkerAISendImages;

		// Token: 0x04000D9F RID: 3487
		private BackgroundWorker bgWorkerAIUpdateParam;

		// Token: 0x04000DA0 RID: 3488
		private BackgroundWorker bgWorkerAIDefaultTable;

		// Token: 0x04000DA1 RID: 3489
		private Progress progressBgWorkerAIDefaultTable;
	}
}
