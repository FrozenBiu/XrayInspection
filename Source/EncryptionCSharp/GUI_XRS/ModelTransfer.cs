using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;
using DatabaseConnection;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x0200008F RID: 143
	public class ModelTransfer : Network
	{
		// Token: 0x1400004D RID: 77
		// (add) Token: 0x06000E9E RID: 3742 RVA: 0x00058658 File Offset: 0x00056858
		// (remove) Token: 0x06000E9F RID: 3743 RVA: 0x00058690 File Offset: 0x00056890
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<string> ModelDataError;

		// Token: 0x06000EA0 RID: 3744 RVA: 0x000586C8 File Offset: 0x000568C8
		public ModelTransfer(string ipAddress, ParameterHandling parameterHandling, DBConnection databaseConnection) : base(ipAddress, ModelTransfer.modelPort, "ModelTransfer")
		{
			this.parHandling = parameterHandling;
			this.dbConnection = databaseConnection;
			base.Connected += this.StartGetModelData;
			base.SetReceiveTimeout(30000);
		}

		// Token: 0x06000EA1 RID: 3745 RVA: 0x00058728 File Offset: 0x00056928
		public void StartConnectionTest()
		{
			new Thread(new ThreadStart(this.TestConnection))
			{
				Name = "TestModelConnection"
			}.Start();
		}

		// Token: 0x06000EA2 RID: 3746 RVA: 0x0005875C File Offset: 0x0005695C
		private void TestConnection()
		{
			while (this.testThreadRunning)
			{
				this.SendConnectionTest();
				Thread.Sleep(this.testPeriod);
			}
		}

		// Token: 0x06000EA3 RID: 3747 RVA: 0x0005878C File Offset: 0x0005698C
		private void SendConnectionTest()
		{
			lock (this)
			{
				base.SendBytes(this.CreateHeader(5, 1), SocketFlags.None);
			}
		}

		// Token: 0x06000EA4 RID: 3748 RVA: 0x000587D8 File Offset: 0x000569D8
		public void StopConnectionTest()
		{
			this.testThreadRunning = false;
		}

		// Token: 0x06000EA5 RID: 3749 RVA: 0x000587E4 File Offset: 0x000569E4
		public void StartGetModelData()
		{
			new Thread(new ThreadStart(this.GetModelData))
			{
				Name = "GetModelData"
			}.Start();
		}

		// Token: 0x06000EA6 RID: 3750 RVA: 0x00058818 File Offset: 0x00056A18
		public void GetModelData()
		{
			while (base.ConnectionEstablished)
			{
				try
				{
					byte[] array = base.ReceiveBytes(0, 1);
					this.bufferRawModelData = null;
					bool flag = array != null && array.Length >= 1;
					if (flag)
					{
						this.bufferHeader = base.ReceiveBytes(1, ModelTransfer.headerSize - 1);
						bool flag2 = this.bufferHeader != null && this.bufferHeader.Length >= 1;
						if (flag2)
						{
							bool flag3 = this.bufferHeader[1] == 1;
							short num = BitConverter.ToInt16(this.bufferHeader, 2);
							bool flag4 = num != 0 && num != 5 && !flag3;
							if (flag4)
							{
								int num2 = BitConverter.ToInt32(this.bufferHeader, 4);
								this.bufferRawModelData = new byte[ModelTransfer.chunkBufferSize];
								long num3 = 0L;
								string path = "";
								this.GetPathModelFile((int)num, ref path);
								using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write))
								{
									while (num3 < (long)num2)
									{
										int size = (int)Math.Min((long)ModelTransfer.chunkBufferSize, (long)num2 - num3);
										this.bufferRawModelData = base.ReceiveBytes(0, size);
										bool flag5 = this.bufferRawModelData == null || this.bufferRawModelData.Length == 0;
										if (flag5)
										{
											break;
										}
										fileStream.Write(this.bufferRawModelData, 0, this.bufferRawModelData.Length);
										num3 += (long)this.bufferRawModelData.Length;
									}
								}
							}
							else
							{
								bool flag6 = num > 0 && flag3;
								if (flag6)
								{
									this.bgWorkerSendModel = new BackgroundWorker();
									this.bgWorkerSendModel.WorkerReportsProgress = false;
									this.bgWorkerSendModel.WorkerSupportsCancellation = true;
									this.bgWorkerSendModel.DoWork += this.bgWorkerSendModel_DoWork;
									this.bgWorkerSendModel.RunWorkerCompleted += this.bgWorkerSendModel_RunWorkerCompleted;
									this.bgWorkerSendModel.RunWorkerAsync(num);
								}
							}
						}
					}
				}
				catch (Exception ex)
				{
					this.OnModelDataError(ex.Message);
				}
			}
		}

		// Token: 0x06000EA7 RID: 3751 RVA: 0x00058A5C File Offset: 0x00056C5C
		private void bgWorkerSendModel_DoWork(object sender, DoWorkEventArgs e)
		{
			try
			{
				short num = (short)e.Argument;
				lock (this)
				{
					string text = "";
					bool flag2 = num == 7 || num == 6;
					string number;
					if (flag2)
					{
						number = this.parHandling.ReadPar(Definition.objectDetectionInferenceModelID);
					}
					else
					{
						bool flag3 = num == 2 || num == 3;
						if (flag3)
						{
							number = this.parHandling.ReadPar(Definition.anomalyDetectionInferenceModelID);
						}
						else
						{
							number = this.parHandling.ReadPar(Definition.anomalyDetectionTrainModelID);
						}
					}
					this.dbConnection.GetAppTableNameWithNumber(number, ref text);
					string tableName = text.Insert("AppDefault".Length, "AI");
					string archiveDirectory = ModelTransfer.GetArchiveDirectory(tableName);
					string path = "";
					switch (num)
					{
					case 1:
					{
						string text2 = this.parHandling.ReadPar(Definition.machineSubTypePath);
						bool flag4 = text2.StartsWith("HX");
						if (flag4)
						{
							path = Path.Combine(archiveDirectory, "modelHX.weights");
						}
						else
						{
							bool flag5 = text2.StartsWith("TX");
							if (flag5)
							{
								path = Path.Combine(archiveDirectory, "modelTX.weights");
							}
						}
						break;
					}
					case 2:
						path = Path.Combine(archiveDirectory, "model.engine");
						break;
					case 3:
						path = Path.Combine(archiveDirectory, "model.onnx");
						break;
					case 4:
					{
						string text2 = this.parHandling.ReadPar(Definition.machineSubTypePath);
						bool flag6 = text2.StartsWith("HX");
						if (flag6)
						{
							path = Path.Combine(archiveDirectory, "featureHX.weights");
						}
						else
						{
							bool flag7 = text2.StartsWith("TX");
							if (flag7)
							{
								path = Path.Combine(archiveDirectory, "featureTX.weights");
							}
						}
						break;
					}
					case 6:
						path = Path.Combine(archiveDirectory, "modelXO.onnx");
						break;
					case 7:
						path = Path.Combine(archiveDirectory, "modelXO.engine");
						break;
					}
					bool flag8 = File.Exists(path);
					if (flag8)
					{
						byte[] array = new byte[ModelTransfer.chunkBufferSize];
						using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
						{
							int modelSize = (int)fileStream.Length;
							base.SendBytes(this.CreateHeader(num, modelSize), SocketFlags.None);
							int length;
							while ((length = fileStream.Read(array, 0, array.Length)) > 0)
							{
								base.SendBytes(array, length, SocketFlags.None);
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				this.OnModelDataError(ex.Message);
			}
		}

		// Token: 0x06000EA8 RID: 3752 RVA: 0x00058D48 File Offset: 0x00056F48
		private void bgWorkerSendModel_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			this.bgWorkerSendModel.DoWork -= this.bgWorkerSendModel_DoWork;
			this.bgWorkerSendModel.RunWorkerCompleted -= this.bgWorkerSendModel_RunWorkerCompleted;
		}

		// Token: 0x06000EA9 RID: 3753 RVA: 0x00058D7C File Offset: 0x00056F7C
		private void GetPathModelFile(int modelType, ref string modelPath)
		{
			string text = "";
			bool flag = modelType == 7 || modelType == 6;
			string number;
			if (flag)
			{
				number = this.parHandling.ReadPar(Definition.objectDetectionInferenceModelID);
			}
			else
			{
				number = this.parHandling.ReadPar(Definition.anomalyDetectionTrainModelID);
			}
			this.dbConnection.GetAppTableNameWithNumber(number, ref text);
			string tableName = text.Insert("AppDefault".Length, "AI");
			string archiveDirectory = ModelTransfer.GetArchiveDirectory(tableName);
			bool flag2 = !Directory.Exists(archiveDirectory);
			if (flag2)
			{
				Directory.CreateDirectory(archiveDirectory);
			}
			bool flag3 = modelType == 1;
			if (flag3)
			{
				this.dbConnection.UpdateTable(tableName, Definition.anomalyDetectionTrainCurrentEpoch, this.parHandling.ReadPar(Definition.anomalyDetectionTrainCurrentEpoch));
				string text2 = this.parHandling.ReadPar(Definition.machineSubTypePath);
				bool flag4 = text2.StartsWith("HX");
				if (flag4)
				{
					modelPath = Path.Combine(new string[]
					{
						archiveDirectory + "/modelHX.weights"
					});
				}
				else
				{
					bool flag5 = text2.StartsWith("TX");
					if (flag5)
					{
						modelPath = Path.Combine(new string[]
						{
							archiveDirectory + "/modelTX.weights"
						});
					}
				}
			}
			else
			{
				bool flag6 = modelType == 4;
				if (flag6)
				{
					string text3 = this.parHandling.ReadPar(Definition.machineSubTypePath);
					bool flag7 = text3.StartsWith("HX");
					if (flag7)
					{
						modelPath = Path.Combine(new string[]
						{
							archiveDirectory + "/featureHX.weights"
						});
					}
					else
					{
						bool flag8 = text3.StartsWith("TX");
						if (flag8)
						{
							modelPath = Path.Combine(new string[]
							{
								archiveDirectory + "/featureTX.weights"
							});
						}
					}
				}
				else
				{
					bool flag9 = modelType == 2;
					if (flag9)
					{
						modelPath = Path.Combine(new string[]
						{
							archiveDirectory + "/model.engine"
						});
					}
					else
					{
						bool flag10 = modelType == 7;
						if (flag10)
						{
							modelPath = Path.Combine(new string[]
							{
								archiveDirectory + "/modelXO.engine"
							});
						}
					}
				}
			}
		}

		// Token: 0x06000EAA RID: 3754 RVA: 0x00058F84 File Offset: 0x00057184
		private byte[] CreateHeader(short modelType, int modelSize)
		{
			byte[] array = new byte[ModelTransfer.headerSize];
			array[0] = 1;
			array[1] = 0;
			BitConverter.GetBytes(modelType).CopyTo(array, 2);
			BitConverter.GetBytes(modelSize).CopyTo(array, 4);
			return array;
		}

		// Token: 0x06000EAB RID: 3755 RVA: 0x00058FC8 File Offset: 0x000571C8
		public static string GetArchiveDirectory(string tableName)
		{
			return Path.Combine(new string[]
			{
				ModelTransfer.archiveRoot + tableName
			});
		}

		// Token: 0x06000EAC RID: 3756 RVA: 0x00058FF8 File Offset: 0x000571F8
		protected virtual void OnModelDataError(string message)
		{
			Action<string> modelDataError = this.ModelDataError;
			bool flag = modelDataError != null;
			if (flag)
			{
				modelDataError(message);
			}
		}

		// Token: 0x040009A1 RID: 2465
		private readonly int testPeriod = 10000;

		// Token: 0x040009A2 RID: 2466
		public static readonly int modelPort = 1348;

		// Token: 0x040009A3 RID: 2467
		private static readonly int headerSize = 8;

		// Token: 0x040009A4 RID: 2468
		private static readonly int chunkBufferSize = 52428800;

		// Token: 0x040009A5 RID: 2469
		private byte[] bufferHeader;

		// Token: 0x040009A6 RID: 2470
		private byte[] bufferRawModelData;

		// Token: 0x040009A7 RID: 2471
		private bool testThreadRunning = true;

		// Token: 0x040009A8 RID: 2472
		private ParameterHandling parHandling;

		// Token: 0x040009A9 RID: 2473
		private DBConnection dbConnection;

		// Token: 0x040009AA RID: 2474
		public static readonly string archiveRoot = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Archive\\Model\\";

		// Token: 0x040009AC RID: 2476
		public const string nameWeightsFileHX = "modelHX.weights";

		// Token: 0x040009AD RID: 2477
		public const string nameWeightsFileTX = "modelTX.weights";

		// Token: 0x040009AE RID: 2478
		public const string nameResnetFileHX = "featureHX.weights";

		// Token: 0x040009AF RID: 2479
		public const string nameResnetFileTX = "featureTX.weights";

		// Token: 0x040009B0 RID: 2480
		public const string nameEngineFileXA = "model.engine";

		// Token: 0x040009B1 RID: 2481
		public const string nameOnnxFileXA = "model.onnx";

		// Token: 0x040009B2 RID: 2482
		public const string nameEngineFileXO = "modelXO.engine";

		// Token: 0x040009B3 RID: 2483
		public const string nameOnnxFileXO = "modelXO.onnx";

		// Token: 0x040009B4 RID: 2484
		private BackgroundWorker bgWorkerSendModel;

		// Token: 0x020002C6 RID: 710
		public enum ModelType
		{
			// Token: 0x04001FD2 RID: 8146
			None,
			// Token: 0x04001FD3 RID: 8147
			WeightsXA,
			// Token: 0x04001FD4 RID: 8148
			TensorRTEngineXA,
			// Token: 0x04001FD5 RID: 8149
			OnnxXA,
			// Token: 0x04001FD6 RID: 8150
			ResnetXA,
			// Token: 0x04001FD7 RID: 8151
			ConnectionTest,
			// Token: 0x04001FD8 RID: 8152
			OnnxXO,
			// Token: 0x04001FD9 RID: 8153
			TensorRTEngineXO
		}
	}
}
