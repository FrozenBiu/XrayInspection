using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using DatabaseConnection;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x0200025B RID: 603
	public class XRayOnProcessing
	{
		// Token: 0x17000C69 RID: 3177
		// (get) Token: 0x06002E5C RID: 11868 RVA: 0x0012EEED File Offset: 0x0012D0ED
		// (set) Token: 0x06002E5D RID: 11869 RVA: 0x0012EEF4 File Offset: 0x0012D0F4
		public static bool CalibrationError { get; set; }

		// Token: 0x14000107 RID: 263
		// (add) Token: 0x06002E5E RID: 11870 RVA: 0x0012EEFC File Offset: 0x0012D0FC
		// (remove) Token: 0x06002E5F RID: 11871 RVA: 0x0012EF34 File Offset: 0x0012D134
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<int> RemainingReseasonTimeChanged;

		// Token: 0x14000108 RID: 264
		// (add) Token: 0x06002E60 RID: 11872 RVA: 0x0012EF6C File Offset: 0x0012D16C
		// (remove) Token: 0x06002E61 RID: 11873 RVA: 0x0012EFA4 File Offset: 0x0012D1A4
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<string> UnknownModelNumberDetected;

		// Token: 0x17000C6A RID: 3178
		// (get) Token: 0x06002E62 RID: 11874 RVA: 0x0012EFD9 File Offset: 0x0012D1D9
		// (set) Token: 0x06002E63 RID: 11875 RVA: 0x0012EFE0 File Offset: 0x0012D1E0
		public static int CurrentIntegrationTimeTdi { get; set; }

		// Token: 0x17000C6B RID: 3179
		// (get) Token: 0x06002E64 RID: 11876 RVA: 0x0012EFE8 File Offset: 0x0012D1E8
		// (set) Token: 0x06002E65 RID: 11877 RVA: 0x0012EFEF File Offset: 0x0012D1EF
		public static double TdiGainFactor { get; set; } = 0.0;

		// Token: 0x06002E66 RID: 11878 RVA: 0x0012EFF8 File Offset: 0x0012D1F8
		public XRayOnProcessing(DBConnection dbConnection, ParameterHandling parHandling, MeanPixelLogger meanPixelLogger, PixelCorrection pixelCorrection, LiveCalibration liveCalibration)
		{
			string[,] array = new string[5, 2];
			array[0, 0] = "30";
			array[0, 1] = "0.5";
			array[1, 0] = "35";
			array[1, 1] = "1.0";
			array[2, 0] = "40";
			array[2, 1] = "1.5";
			array[3, 0] = "45";
			array[3, 1] = "2.0";
			array[4, 0] = "50";
			array[4, 1] = "2.0";
			this.reseasonKvMaSpellman50kV100W = array;
			string[,] array2 = new string[3, 2];
			array2[0, 0] = "40";
			array2[0, 1] = "1.0";
			array2[1, 0] = "45";
			array2[1, 1] = "1.5";
			array2[2, 0] = "50";
			array2[2, 1] = "2.0";
			this.reseasonKvMaVJ50kV100W = array2;
			string[,] array3 = new string[10, 2];
			array3[0, 0] = "40";
			array3[0, 1] = "0.20";
			array3[1, 0] = "42";
			array3[1, 1] = "0.54";
			array3[2, 0] = "44";
			array3[2, 1] = "0.89";
			array3[3, 0] = "47";
			array3[3, 1] = "1.24";
			array3[4, 0] = "49";
			array3[4, 1] = "1.59";
			array3[5, 0] = "51";
			array3[5, 1] = "1.94";
			array3[6, 0] = "53";
			array3[6, 1] = "2.28";
			array3[7, 0] = "56";
			array3[7, 1] = "2.63";
			array3[8, 0] = "58";
			array3[8, 1] = "2.98";
			array3[9, 0] = "60";
			array3[9, 1] = "3.33";
			this.reseasonKvMa60kV200W = array3;
			string[,] array4 = new string[10, 2];
			array4[0, 0] = "40";
			array4[0, 1] = "2.00";
			array4[1, 0] = "42";
			array4[1, 1] = "2.15";
			array4[2, 0] = "44";
			array4[2, 1] = "2.30";
			array4[3, 0] = "47";
			array4[3, 1] = "2.44";
			array4[4, 0] = "49";
			array4[4, 1] = "2.59";
			array4[5, 0] = "51";
			array4[5, 1] = "2.74";
			array4[6, 0] = "53";
			array4[6, 1] = "2.89";
			array4[7, 0] = "56";
			array4[7, 1] = "3.04";
			array4[8, 0] = "58";
			array4[8, 1] = "3.19";
			array4[9, 0] = "60";
			array4[9, 1] = "3.33";
			this.reseasonKvMa60kV200WP760 = array4;
			string[,] array5 = new string[9, 2];
			array5[0, 0] = "40";
			array5[0, 1] = "0.25";
			array5[1, 0] = "45";
			array5[1, 1] = "0.4";
			array5[2, 0] = "50";
			array5[2, 1] = "0.55";
			array5[3, 0] = "55";
			array5[3, 1] = "0.7";
			array5[4, 0] = "60";
			array5[4, 1] = "0.85";
			array5[5, 0] = "65";
			array5[5, 1] = "1.0";
			array5[6, 0] = "70";
			array5[6, 1] = "1.15";
			array5[7, 0] = "75";
			array5[7, 1] = "1.25";
			array5[8, 0] = "80";
			array5[8, 1] = "1.25";
			this.reseasonKvMa80kV100W = array5;
			string[,] array6 = new string[10, 2];
			array6[0, 0] = "40";
			array6[0, 1] = "0.15";
			array6[1, 0] = "47";
			array6[1, 1] = "0.30";
			array6[2, 0] = "54";
			array6[2, 1] = "0.45";
			array6[3, 0] = "61";
			array6[3, 1] = "0.60";
			array6[4, 0] = "68";
			array6[4, 1] = "0.75";
			array6[5, 0] = "75";
			array6[5, 1] = "1.90";
			array6[6, 0] = "82";
			array6[6, 1] = "1.05";
			array6[7, 0] = "89";
			array6[7, 1] = "1.20";
			array6[8, 0] = "96";
			array6[8, 1] = "1.35";
			array6[9, 0] = "100";
			array6[9, 1] = "1.50";
			this.reseasonKvMa100kV150W = array6;
			string[,] array7 = new string[10, 2];
			array7[0, 0] = "40";
			array7[0, 1] = "0.2";
			array7[1, 0] = "47";
			array7[1, 1] = "0.4";
			array7[2, 0] = "54";
			array7[2, 1] = "0.6";
			array7[3, 0] = "61";
			array7[3, 1] = "0.8";
			array7[4, 0] = "68";
			array7[4, 1] = "1.0";
			array7[5, 0] = "75";
			array7[5, 1] = "1.2";
			array7[6, 0] = "82";
			array7[6, 1] = "1.4";
			array7[7, 0] = "89";
			array7[7, 1] = "1.6";
			array7[8, 0] = "96";
			array7[8, 1] = "1.8";
			array7[9, 0] = "100";
			array7[9, 1] = "2.0";
			this.reseasonKvMa100kV200W = array7;
			string[,] array8 = new string[3, 2];
			array8[0, 0] = "40";
			array8[0, 1] = "0.4";
			array8[1, 0] = "45";
			array8[1, 1] = "0.8";
			array8[2, 0] = "50";
			array8[2, 1] = "1.2";
			this.reseasonKvMa50kV60W = array8;
			string[,] array9 = new string[10, 2];
			array9[0, 0] = "40";
			array9[0, 1] = "0.5";
			array9[1, 0] = "47";
			array9[1, 1] = "0.78";
			array9[2, 0] = "54";
			array9[2, 1] = "1.06";
			array9[3, 0] = "61";
			array9[3, 1] = "1.33";
			array9[4, 0] = "68";
			array9[4, 1] = "1.61";
			array9[5, 0] = "75";
			array9[5, 1] = "1.89";
			array9[6, 0] = "82";
			array9[6, 1] = "2.17";
			array9[7, 0] = "89";
			array9[7, 1] = "2.45";
			array9[8, 0] = "96";
			array9[8, 1] = "2.72";
			array9[9, 0] = "100";
			array9[9, 1] = "3.00";
			this.reseasonKvMa100kV300W = array9;
			base..ctor();
			this.dbConnection = dbConnection;
			this.parHandling = parHandling;
			this.meanPixelLogger = meanPixelLogger;
			this.pixelCorrection = pixelCorrection;
			this.liveCalibration = liveCalibration;
			this.offsetLoggingSemaphore = new Semaphore(0, 1);
		}

		// Token: 0x06002E67 RID: 11879 RVA: 0x0012F7E0 File Offset: 0x0012D9E0
		public bool Execute()
		{
			int temperature = 0;
			MachineMode machineMode = MachineMode.Standard;
			DetectorType detectorType = DetectorType.DT;
			int nValves = 1;
			bool flag = false;
			this.dbConnection.GetMachineMode(ref machineMode);
			this.dbConnection.GetDetectorType(ref detectorType);
			ErrorSurvey.CheckArcsIsActive = true;
			this.dbConnection.GetDemoMode(ref flag);
			bool flag2 = this.parHandling.ReadPar(XRayOnProcessing.xrayEnablePath) == "1";
			if (flag2)
			{
				XRayOnProcessing.DisableXray(this.parHandling, this.dbConnection, true);
				Thread.Sleep(2000);
			}
			this.parHandling.WriteValue("/filter/xrayoffimage/Enable/Value", "0");
			bool flag3 = machineMode == MachineMode.Bulk && this.parHandling.ReadPar(SVT.svtEnablePath) == "1";
			if (flag3)
			{
				this.dbConnection.GetNumValves(ref nValves);
				this.EnableValvesConsecutively(nValves);
			}
			bool calibrateInPlot = XRayOnProcessing.CalibrateInPlot;
			if (calibrateInPlot)
			{
				string path = (detectorType == DetectorType.DT || detectorType == DetectorType.DTNetSE || detectorType == DetectorType.DTNetDE || detectorType == DetectorType.DTNetTDI) ? Definition.detectorDTScanratePath : Definition.detectorSesotecScanratePath;
				this.parHandling.WriteValue(path, XRayOnProcessing.ConveyingScanrate.ToString(), false);
			}
			bool flag4 = detectorType == DetectorType.DT || detectorType == DetectorType.DTNetSE || detectorType == DetectorType.DTNetDE || detectorType == DetectorType.DTNetTDI;
			if (flag4)
			{
				this.parHandling.WriteValue("/hardware/detector/dt/GetBufferedValues/Value", "1", false);
				int.TryParse(this.parHandling.ReadPar(Definition.dtMCUTemperaturePath), out temperature);
				bool flag5 = detectorType == DetectorType.DTNetTDI;
				if (flag5)
				{
					DirectoryInfo directoryInfo = new DirectoryInfo(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\" + Definition.tdiCalibFolder);
					bool exists = directoryInfo.Exists;
					if (exists)
					{
						foreach (FileInfo fileInfo in directoryInfo.GetFiles())
						{
							fileInfo.Delete();
						}
					}
					else
					{
						directoryInfo.Create();
					}
					XRayOnProcessing.LogTdiCalib("Process ID:" + this.parHandling.ReadPar("/application/PidXRSMain_64/Value"));
					XRayOnProcessing.LogTdiCalib("00 - Stop scanning");
					for (int j = 0; j < 3; j++)
					{
						this.parHandling.WriteValue("/hardware/detector/dt/StartStopScanning/Value", "0", false);
						string text = this.parHandling.ReadPar("/hardware/detector/dt/StartStopScanning/Value");
						XRayOnProcessing.LogTdiCalib("01 - Check if scanning stopped. Scanning is: " + text);
						bool flag6 = text == "0";
						if (flag6)
						{
							break;
						}
					}
				}
				bool flag7 = detectorType == DetectorType.DTNetTDI;
				if (flag7)
				{
					XRayOnProcessing.LogTdiCalib("02 - Disable Offset correction");
				}
				this.parHandling.WriteValue(XRayOnProcessing.dtOffsetEnablePath, "0", false);
				bool flag8 = detectorType == DetectorType.DTNetTDI;
				if (flag8)
				{
					XRayOnProcessing.LogTdiCalib("03 - Disable Gain correction");
				}
				this.parHandling.WriteValue(XRayOnProcessing.dtGainEnablePath, "0", false);
				bool flag9 = detectorType == DetectorType.DTNetTDI;
				if (flag9)
				{
					XRayOnProcessing.LogTdiCalib("04 - Disable Baseline correction");
				}
				this.parHandling.WriteValue(XRayOnProcessing.dtBaselineEnablePath, "0", false);
			}
			else
			{
				int.TryParse(this.parHandling.ReadPar(Definition.detectorSesotecTemperaturePath), out temperature);
			}
			this.pixelCorrection.NullOffsetAndGain();
			this.parHandling.WriteValue(XRayOnProcessing.manualOffsetCalibrationPath, "0", false);
			this.parHandling.WriteValue(XRayOnProcessing.manualGainCalibrationPath, "0", false);
			this.PrepareLanes(true);
			bool flag10 = detectorType == DetectorType.DTNetTDI;
			if (flag10)
			{
				this.PrepareTdiDetector();
			}
			int num = 0;
			if (num < 30)
			{
				Thread.Sleep(100);
				string a = this.parHandling.ReadPar(XrayState.xrayPowerOnPath);
				bool flag11 = a == "1";
				if (!flag11)
				{
					bool flag12 = machineMode == MachineMode.Bulk && this.parHandling.ReadPar(SVT.svtEnablePath) == "1";
					if (flag12)
					{
						this.RestoreValvesParameter();
					}
					return false;
				}
			}
			bool flag13 = detectorType == DetectorType.DTNetTDI;
			if (flag13)
			{
				this.parHandling.WriteValue("/hardware/detector/dt/StartStopScanning/Value", "1", false);
				XRayOnProcessing.LogTdiCalib("08 - Check if scanning started. Scanning is: " + this.parHandling.ReadPar("/hardware/detector/dt/StartStopScanning/Value"));
				XRayOnProcessing.LogTdiCalib("09 - Waiting for Offset Image");
			}
			bool flag14 = machineMode == MachineMode.Bulk;
			if (flag14)
			{
				this.parHandling.WriteValue("/machine/BulkOverlapImgActivate/Value", "0");
			}
			bool flag15 = machineMode == MachineMode.BulkObject;
			if (flag15)
			{
				this.parHandling.WriteValue("/machine/BulkOverlapImgActivate/Value", "0");
			}
			bool flag16 = !flag;
			if (flag16)
			{
				this.meanPixelLogger.OffsetValuesSaved += this.meanPixelLogger_OffsetValuesSaved;
				this.parHandling.WriteValue(Definition.machineStateSubPath, MachineStatesSub.WaitForOffsetImage.ToString(), false);
				this.meanPixelLogger.StartPixelLogging(MeanPixelLogger.LogMode.Database, true);
			}
			bool flag17 = flag || this.offsetLoggingSemaphore.WaitOne(20000);
			bool result;
			if (flag17)
			{
				bool flag18 = detectorType != DetectorType.DTNetTDI;
				if (flag18)
				{
					this.pixelCorrection.CorrectOffset(temperature);
				}
				bool flag19 = this.parHandling.ReadPar("/hardware/detector/LiveCalibration/Value") == "1" || this.parHandling.ReadPar("/hardware/detector/LiveCalibration/Value") == "2";
				if (flag19)
				{
					this.liveCalibration.StopTimer();
				}
				this.parHandling.WriteValue(XRayOnProcessing.manualOffsetCalibrationPath, "1", false);
				result = this.SetXrayAndGainCalibration(XRayOnProcessing.gainCalibrationValue, temperature);
			}
			else
			{
				bool flag20 = detectorType == DetectorType.DTNetTDI;
				if (flag20)
				{
					XRayOnProcessing.LogTdiCalib("Error No Offset image arrived");
				}
				this.meanPixelLogger.OffsetValuesSaved -= this.meanPixelLogger_OffsetValuesSaved;
				this.meanPixelLogger.StopPixelLogging(true);
				this.parHandling.WriteValue(Definition.machineStateSubPath, "", false);
				XRayOnProcessing.CalibrationError = true;
				bool flag21 = this.parHandling.ReadPar(Definition.isLiveCalibrationActivePath) == "1" || this.parHandling.ReadPar(Definition.isLiveCalibrationActivePath) == "2";
				if (flag21)
				{
					this.parHandling.WriteValue(Definition.LiveCalibrationError, "1");
				}
				bool flag22 = machineMode == MachineMode.Bulk && this.parHandling.ReadPar(SVT.svtEnablePath) == "1";
				if (flag22)
				{
					this.RestoreValvesParameter();
				}
				result = false;
			}
			return result;
		}

		// Token: 0x06002E68 RID: 11880 RVA: 0x0012FE48 File Offset: 0x0012E048
		private void PrepareLanes(bool preCalibration)
		{
			MachineMode machineMode = MachineMode.Standard;
			this.dbConnection.GetMachineMode(ref machineMode);
			if (preCalibration)
			{
				this.parHandling.WriteValue(Acquisition.lane1EnablePath, "1", false);
				this.parHandling.WriteValue(Acquisition.lane2EnablePath, "0", false);
				this.parHandling.WriteValue(Acquisition.lane3EnablePath, "0", false);
				this.parHandling.WriteValue(Acquisition.lane4EnablePath, "0", false);
				this.parHandling.WriteValue(Acquisition.lane1MinPixelPath, "0", false);
				this.parHandling.WriteValue(Acquisition.lane1MaxPixelPath, 10239.ToString(), false);
				this.parHandling.WriteValue(Acquisition.sendOnlyIfContaminantPath, "0", false);
				this.parHandling.WriteValue(Acquisition.sendImagePath, "1", false);
				this.parHandling.WriteValue(Acquisition.continuousModeFilteringPath, "0", false);
				this.parHandling.WriteValue(Acquisition.binningPath, "1", false);
				this.parHandling.WriteValue(Acquisition.binPixelPath, "1", false);
				this.parHandling.WriteValue(SVT.svtValvesEnablePath, "0", false);
				this.parHandling.WriteValue(Acquisition.acquisitionModePath, "0", false);
			}
			else
			{
				string text = null;
				this.dbConnection.GetParameterValue(Acquisition.lane1EnablePath, ref text);
				this.parHandling.WriteValue(Acquisition.lane1EnablePath, text, false);
				this.dbConnection.GetParameterValue(Acquisition.lane2EnablePath, ref text);
				this.parHandling.WriteValue(Acquisition.lane2EnablePath, text, false);
				this.dbConnection.GetParameterValue(Acquisition.lane3EnablePath, ref text);
				this.parHandling.WriteValue(Acquisition.lane3EnablePath, text, false);
				this.dbConnection.GetParameterValue(Acquisition.lane4EnablePath, ref text);
				this.parHandling.WriteValue(Acquisition.lane4EnablePath, text, false);
				this.dbConnection.GetParameterValue(Acquisition.lane1MinPixelPath, ref text);
				this.parHandling.WriteValue(Acquisition.lane1MinPixelPath, text, false);
				this.dbConnection.GetParameterValue(Acquisition.lane1MaxPixelPath, ref text);
				this.parHandling.WriteValue(Acquisition.lane1MaxPixelPath, text, false);
				this.dbConnection.GetParameterValue(Acquisition.sendOnlyIfContaminantPath, ref text);
				this.parHandling.WriteValue(Acquisition.sendOnlyIfContaminantPath, text, false);
				this.dbConnection.GetParameterValue(Acquisition.sendImagePath, ref text);
				this.parHandling.WriteValue(Acquisition.sendImagePath, text, false);
				this.dbConnection.GetParameterValue(Acquisition.continuousModeFilteringPath, ref text);
				this.parHandling.WriteValue(Acquisition.continuousModeFilteringPath, text, false);
				this.dbConnection.GetParameterValue(Acquisition.binningPath, ref text);
				this.parHandling.WriteValue(Acquisition.binningPath, text, false);
				this.dbConnection.GetParameterValue(Acquisition.binPixelPath, ref text);
				this.parHandling.WriteValue(Acquisition.binPixelPath, text, false);
				this.dbConnection.GetParameterValue(Acquisition.acquisitionModePath, ref text);
				this.parHandling.WriteValue(Acquisition.acquisitionModePath, text, false);
				this.dbConnection.GetParameterValue(FlapsOverIOs.bulkflapsEnabledPath, ref text);
				bool flag = text == "1";
				if (flag)
				{
					this.dbConnection.GetParameterValue(FlapsOverIOs.bulkflapsEnableValvesPath, ref text);
					this.parHandling.WriteValue(FlapsOverIOs.bulkflapsEnableValvesPath, text);
				}
				this.dbConnection.GetParameterValue(SVT.svtEnablePath, ref text);
				bool flag2 = text == "1";
				if (flag2)
				{
					this.dbConnection.GetParameterValue(SVT.svtValvesEnablePath, ref text);
					this.parHandling.WriteValue(SVT.svtValvesEnablePath, text);
				}
				bool flag3 = machineMode == MachineMode.BulkObject;
				if (flag3)
				{
					this.parHandling.WriteValue(FlapsOverIOs.bulkflapsEnableValvesDOPath, "0", false);
				}
			}
		}

		// Token: 0x06002E69 RID: 11881 RVA: 0x0013022C File Offset: 0x0012E42C
		private void meanPixelLogger_OffsetValuesSaved(int temperature)
		{
			this.meanPixelLogger.OffsetValuesSaved -= this.meanPixelLogger_OffsetValuesSaved;
			this.meanPixelLogger.StopPixelLogging(false);
			this.parHandling.WriteValue(Definition.machineStateSubPath, "", false);
			this.offsetLoggingSemaphore.Release();
		}

		// Token: 0x06002E6A RID: 11882 RVA: 0x00130284 File Offset: 0x0012E484
		private void meanPixelLogger_GainValuesSaved(GainCalibrationInfo info)
		{
			this.meanPixelLogger.GainValuesSaved -= this.meanPixelLogger_GainValuesSaved;
			this.meanPixelLogger.StopPixelLogging(true);
			this.parHandling.WriteValue(Definition.machineStateSubPath, "", false);
			bool calibrationError = info.CalibrationError;
			if (calibrationError)
			{
				XRayOnProcessing.CalibrationError = true;
				bool flag = this.parHandling.ReadPar(Definition.isLiveCalibrationActivePath) == "1" || this.parHandling.ReadPar(Definition.isLiveCalibrationActivePath) == "2";
				if (flag)
				{
					this.parHandling.WriteValue(Definition.LiveCalibrationError, "1");
				}
			}
			this.offsetLoggingSemaphore.Release();
		}

		// Token: 0x06002E6B RID: 11883 RVA: 0x00130340 File Offset: 0x0012E540
		private bool SetXrayAndGainCalibration(int gainCalibrationValue, int temperature)
		{
			string text = "";
			string text2 = "";
			MachineMode machineMode = MachineMode.Standard;
			double num = 0.0;
			double num2 = 0.0;
			bool flag = true;
			bool flag2 = false;
			DetectorType detectorType = DetectorType.DT;
			this.dbConnection.GetDetectorType(ref detectorType);
			this.dbConnection.GetDemoMode(ref flag2);
			bool flag3 = this.dbConnection != null;
			if (flag3)
			{
				this.dbConnection.GetParameterValue("/hardware/xraysource/VoltageSetpoint/Value", ref text);
				this.dbConnection.GetParameterValue("/hardware/xraysource/CurrentSetpoint/Value", ref text2);
			}
			bool flag4 = !double.TryParse(text, NumberStyles.Any, new CultureInfo("en-US"), out num) || num <= 0.0;
			if (flag4)
			{
				XRayOnProcessing.SetDefaultVoltageSetpoint(this.dbConnection, this.parHandling);
			}
			bool flag5 = !double.TryParse(text2, NumberStyles.Any, new CultureInfo("en-US"), out num2) || num2 <= 0.0;
			if (flag5)
			{
				XRayOnProcessing.SetDefaultCurrentSetpoint(this.dbConnection, this.parHandling);
			}
			this.dbConnection.GetParameterValue("/hardware/xraysource/VoltageSetpoint/Value", ref text);
			this.dbConnection.GetParameterValue("/hardware/xraysource/CurrentSetpoint/Value", ref text2);
			this.parHandling.WriteValue("/hardware/xraysource/VoltageSetpoint/Value", text, false);
			this.parHandling.WriteValue("/hardware/xraysource/CurrentSetpoint/Value", text2, false);
			this.dbConnection.GetMachineMode(ref machineMode);
			bool flag6 = machineMode == MachineMode.Bulk;
			bool flag7 = detectorType == DetectorType.DTNetTDI;
			if (flag7)
			{
				XRayOnProcessing.LogTdiCalib("13 - Enable X-Ray");
			}
			XRayOnProcessing.EnableXray(this.parHandling);
			Thread.Sleep(2000);
			int num3 = 0;
			if (num3 < 30)
			{
				Thread.Sleep(100);
				string a = this.parHandling.ReadPar(XrayState.xrayPowerOnPath);
				bool flag8 = a == "1";
				if (!flag8)
				{
					bool flag9 = machineMode == MachineMode.Bulk && this.parHandling.ReadPar(SVT.svtEnablePath) == "1";
					if (flag9)
					{
						this.RestoreValvesParameter();
					}
					return false;
				}
			}
			this.dbConnection.GetSaveGainCalibration(ref flag);
			bool flag10 = detectorType == DetectorType.DTNetTDI;
			if (flag10)
			{
				string text3 = this.parHandling.ReadPar("/hardware/detector/dt/TdiCorrectionMode/Value");
				XRayOnProcessing.LogTdiCalib("   - TdiCorrectionMode is " + text3 + " " + ((TdiCorrectionMode)Convert.ToInt32(text3)).ToString());
				this.parHandling.WriteValue("/hardware/detector/dt/StartStopScanning/Value", "1", false);
				XRayOnProcessing.LogTdiCalib("14 - Check if scanning started. Scanning is: " + this.parHandling.ReadPar("/hardware/detector/dt/StartStopScanning/Value"));
				XRayOnProcessing.LogTdiCalib("15 - Waiting for Gain Image");
			}
			bool flag11 = !flag2;
			if (flag11)
			{
				this.meanPixelLogger.GainValuesSaved += this.meanPixelLogger_GainValuesSaved;
				this.parHandling.WriteValue(Definition.machineStateSubPath, MachineStatesSub.WaitForGainImage.ToString(), false);
				this.meanPixelLogger.StartPixelLogging(MeanPixelLogger.LogMode.Database, flag && machineMode != MachineMode.Sideshooter, false, gainCalibrationValue);
			}
			bool flag12 = flag2 || this.offsetLoggingSemaphore.WaitOne(20000);
			bool flag13 = flag12;
			if (flag13)
			{
				bool flag14 = detectorType != DetectorType.DTNetTDI;
				if (flag14)
				{
					this.pixelCorrection.CorrectGain(this.meanPixelLogger.PixelGainsLine);
					bool flag15 = this.parHandling.ReadPar("/hardware/detector/LiveCalibration/Value") == "1" || this.parHandling.ReadPar("/hardware/detector/LiveCalibration/Value") == "2";
					if (flag15)
					{
						this.liveCalibration.StartTimer();
					}
				}
			}
			else
			{
				bool flag16 = detectorType == DetectorType.DTNetTDI;
				if (flag16)
				{
					XRayOnProcessing.LogTdiCalib("Error No Gain image arrived");
				}
				this.meanPixelLogger.GainValuesSaved -= this.meanPixelLogger_GainValuesSaved;
				this.meanPixelLogger.StopPixelLogging(true);
				this.parHandling.WriteValue(Definition.machineStateSubPath, "", false);
				XRayOnProcessing.CalibrationError = true;
				bool flag17 = this.parHandling.ReadPar(Definition.isLiveCalibrationActivePath) == "1" || this.parHandling.ReadPar(Definition.isLiveCalibrationActivePath) == "2";
				if (flag17)
				{
					this.parHandling.WriteValue(Definition.LiveCalibrationError, "1");
				}
			}
			this.PrepareLanes(false);
			this.parHandling.WriteValue(XRayOnProcessing.manualGainCalibrationPath, "1", false);
			bool flag18 = true;
			this.dbConnection.GetDetectorTempCorrection(ref flag18);
			bool flag19 = flag18 && detectorType != DetectorType.DTNetTDI;
			if (flag19)
			{
				this.pixelCorrection.StartCorrection();
			}
			bool flag20 = machineMode == MachineMode.BulkObject;
			if (flag20)
			{
				this.parHandling.WriteValue(Definition.isLiveCalibrationActivePath, "2");
			}
			Thread.Sleep(1000);
			bool flag21 = flag6;
			if (flag21)
			{
				BulkMode.Activate(this.dbConnection, this.parHandling);
				bool flag22 = this.parHandling.ReadPar(SVT.svtEnablePath) == "1";
				if (flag22)
				{
					this.RestoreValvesParameter();
				}
				else
				{
					this.EnableFlapsConsecutively();
				}
			}
			this.parHandling.WriteValue(Definition.temperatureInUsePath, temperature.ToString(), false);
			return flag12;
		}

		// Token: 0x06002E6C RID: 11884 RVA: 0x001308A4 File Offset: 0x0012EAA4
		public bool CheckForReseasoning()
		{
			bool result = false;
			string value = "";
			bool lastXrayOn = this.dbConnection.GetLastXrayOn(ref value);
			if (lastXrayOn)
			{
				CultureInfo provider = new CultureInfo("de-DE");
				this.timePassedSinceXrayOn = DateTime.Now.Ticks - Convert.ToDateTime(value, provider).Ticks;
				result = (this.timePassedSinceXrayOn > 2592000000000L);
			}
			return result;
		}

		// Token: 0x06002E6D RID: 11885 RVA: 0x00130918 File Offset: 0x0012EB18
		public bool ReseasonProcessing()
		{
			this.parHandling.WriteValue(XRayOnProcessing.dtBaselineValuePath, "0", false);
			this.parHandling.WriteValue(XRayOnProcessing.dtGainEnablePath, "0", false);
			this.parHandling.WriteValue(XRayOnProcessing.manualGainCalibrationPath, "0");
			bool flag = this.timePassedSinceXrayOn > 2592000000000L && this.timePassedSinceXrayOn < 25920000000000L;
			bool result;
			if (flag)
			{
				result = this.Reseason(30);
			}
			else
			{
				bool flag2 = this.timePassedSinceXrayOn > 25920000000000L && this.timePassedSinceXrayOn < 77760000000000L;
				if (flag2)
				{
					result = this.Reseason(60);
				}
				else
				{
					result = this.Reseason(300);
				}
			}
			return result;
		}

		// Token: 0x06002E6E RID: 11886 RVA: 0x001309E4 File Offset: 0x0012EBE4
		private bool Reseason(int stepTime)
		{
			bool flag = false;
			for (int i = 0; i < 10; i++)
			{
				Thread.Sleep(200);
				bool flag2 = this.parHandling.ReadPar(XrayState.xrayStarterPath) != "1";
				if (flag2)
				{
					flag = true;
					break;
				}
			}
			bool flag3 = !flag;
			bool result;
			if (flag3)
			{
				string value = this.parHandling.ReadPar(XRayOnProcessing.xrayModelNumberPath);
				bool flag4 = XRayOnProcessing.modelNr50KV_Spellman.Contains(value);
				if (flag4)
				{
					result = this.Reseason(stepTime, this.reseasonKvMaSpellman50kV100W);
				}
				else
				{
					bool flag5 = XRayOnProcessing.modelNr80KV.Contains(value);
					if (flag5)
					{
						result = this.Reseason(stepTime, this.reseasonKvMa80kV100W);
					}
					else
					{
						bool flag6 = XRayOnProcessing.modelNr50KV_VJ.Contains(value);
						if (flag6)
						{
							result = this.Reseason(stepTime, this.reseasonKvMaVJ50kV100W);
						}
						else
						{
							bool flag7 = XRayOnProcessing.modelNr100KV150W.Contains(value);
							if (flag7)
							{
								result = this.Reseason(stepTime, this.reseasonKvMa100kV150W);
							}
							else
							{
								bool flag8 = XRayOnProcessing.modelNr60KV200W.Contains(value);
								if (flag8)
								{
									result = this.Reseason(stepTime, this.reseasonKvMa60kV200W);
								}
								else
								{
									bool flag9 = XRayOnProcessing.modelNr60KV200WP760.Contains(value);
									if (flag9)
									{
										result = this.Reseason(stepTime, this.reseasonKvMa60kV200WP760);
									}
									else
									{
										bool flag10 = XRayOnProcessing.modelNr100KV200W.Contains(value) || XRayOnProcessing.modelNr100KV200WLonglife.Contains(value);
										if (flag10)
										{
											result = this.Reseason(stepTime, this.reseasonKvMa100kV200W);
										}
										else
										{
											bool flag11 = XRayOnProcessing.modelNr50KV60W.Contains(value);
											if (flag11)
											{
												result = this.Reseason(stepTime, this.reseasonKvMa50kV60W);
											}
											else
											{
												bool flag12 = XRayOnProcessing.modelNr100KV300W.Contains(value);
												if (flag12)
												{
													result = this.Reseason(stepTime, this.reseasonKvMa100kV200W);
												}
												else
												{
													result = this.Reseason(stepTime, this.reseasonKvMaVJ50kV100W);
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06002E6F RID: 11887 RVA: 0x00130BD8 File Offset: 0x0012EDD8
		private bool Reseason(int stepTime, string[,] reseasonKvMa)
		{
			int num = reseasonKvMa.Length / 2 * stepTime;
			this.parHandling.WriteValue("/hardware/xraysource/VoltageSetpoint/Value", reseasonKvMa[0, 0], false);
			this.parHandling.WriteValue("/hardware/xraysource/CurrentSetpoint/Value", reseasonKvMa[0, 1], false);
			XRayOnProcessing.EnableXray(this.parHandling);
			bool flag = !this.UpdateRemainingReseasoningTime(stepTime, ref num);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				for (int i = 1; i < reseasonKvMa.Length / 2; i++)
				{
					this.parHandling.WriteValue("/hardware/xraysource/VoltageSetpoint/Value", reseasonKvMa[i, 0], false);
					this.parHandling.WriteValue("/hardware/xraysource/CurrentSetpoint/Value", reseasonKvMa[i, 1], false);
					bool flag2 = !this.UpdateRemainingReseasoningTime(stepTime, ref num);
					if (flag2)
					{
						return false;
					}
				}
				XRayOnProcessing.DisableXray(this.parHandling, this.dbConnection, true);
				result = true;
			}
			return result;
		}

		// Token: 0x06002E70 RID: 11888 RVA: 0x00130CC4 File Offset: 0x0012EEC4
		protected virtual void OnRemainingReseasonTimeChanged(int remainingTime)
		{
			Action<int> remainingReseasonTimeChanged = this.RemainingReseasonTimeChanged;
			bool flag = remainingReseasonTimeChanged != null;
			if (flag)
			{
				remainingReseasonTimeChanged(remainingTime);
			}
		}

		// Token: 0x06002E71 RID: 11889 RVA: 0x00130CEC File Offset: 0x0012EEEC
		public static void SetDefaultVoltageSetpoint(DBConnection dbConnection, ParameterHandling parHandling)
		{
			string value = parHandling.ReadPar(XRayOnProcessing.xrayModelNumberPath);
			bool flag = XRayOnProcessing.modelNr50KV_Spellman.Contains(value);
			if (flag)
			{
				dbConnection.UpdateCurrentProductTable("/hardware/xraysource/VoltageSetpoint/Value", "50");
			}
			else
			{
				bool flag2 = XRayOnProcessing.modelNr50KV_VJ.Contains(value);
				if (flag2)
				{
					dbConnection.UpdateCurrentProductTable("/hardware/xraysource/VoltageSetpoint/Value", "50");
				}
				else
				{
					bool flag3 = XRayOnProcessing.modelNr80KV.Contains(value);
					if (flag3)
					{
						dbConnection.UpdateCurrentProductTable("/hardware/xraysource/VoltageSetpoint/Value", "80");
					}
					else
					{
						bool flag4 = XRayOnProcessing.modelNr100KV150W.Contains(value);
						if (flag4)
						{
							dbConnection.UpdateCurrentProductTable("/hardware/xraysource/VoltageSetpoint/Value", "60");
						}
						else
						{
							bool flag5 = XRayOnProcessing.modelNr100KV200W.Contains(value);
							if (flag5)
							{
								dbConnection.UpdateCurrentProductTable("/hardware/xraysource/VoltageSetpoint/Value", "50");
							}
							else
							{
								bool flag6 = XRayOnProcessing.modelNr60KV200W.Contains(value);
								if (flag6)
								{
									dbConnection.UpdateCurrentProductTable("/hardware/xraysource/VoltageSetpoint/Value", "50");
								}
								else
								{
									bool flag7 = XRayOnProcessing.modelNr60KV200WP760.Contains(value);
									if (flag7)
									{
										dbConnection.UpdateCurrentProductTable("/hardware/xraysource/VoltageSetpoint/Value", "50");
									}
									else
									{
										bool flag8 = XRayOnProcessing.modelNr100KV200WLonglife.Contains(value);
										if (flag8)
										{
											dbConnection.UpdateCurrentProductTable("/hardware/xraysource/VoltageSetpoint/Value", "50");
										}
										else
										{
											bool flag9 = XRayOnProcessing.modelNr100KV300W.Contains(value);
											if (flag9)
											{
												dbConnection.UpdateCurrentProductTable("/hardware/xraysource/VoltageSetpoint/Value", "50");
											}
											else
											{
												dbConnection.UpdateCurrentProductTable("/hardware/xraysource/VoltageSetpoint/Value", "50");
											}
										}
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06002E72 RID: 11890 RVA: 0x00130E7C File Offset: 0x0012F07C
		public static void SetDefaultCurrentSetpoint(DBConnection dbConnection, ParameterHandling parHandling)
		{
			string value = parHandling.ReadPar(XRayOnProcessing.xrayModelNumberPath);
			bool flag = XRayOnProcessing.modelNr50KV_Spellman.Contains(value);
			if (flag)
			{
				dbConnection.UpdateCurrentProductTable("/hardware/xraysource/CurrentSetpoint/Value", "1.5");
			}
			else
			{
				bool flag2 = XRayOnProcessing.modelNr50KV_VJ.Contains(value);
				if (flag2)
				{
					dbConnection.UpdateCurrentProductTable("/hardware/xraysource/CurrentSetpoint/Value", "2.0");
				}
				else
				{
					bool flag3 = XRayOnProcessing.modelNr80KV.Contains(value);
					if (flag3)
					{
						dbConnection.UpdateCurrentProductTable("/hardware/xraysource/CurrentSetpoint/Value", "1.25");
					}
					else
					{
						bool flag4 = XRayOnProcessing.modelNr50KV60W.Contains(value);
						if (flag4)
						{
							dbConnection.UpdateCurrentProductTable("/hardware/xraysource/CurrentSetpoint/Value", "1.2");
						}
						else
						{
							bool flag5 = XRayOnProcessing.modelNr100KV150W.Contains(value);
							if (flag5)
							{
								dbConnection.UpdateCurrentProductTable("/hardware/xraysource/CurrentSetpoint/Value", "2.5");
							}
							else
							{
								bool flag6 = XRayOnProcessing.modelNr60KV200W.Contains(value);
								if (flag6)
								{
									dbConnection.UpdateCurrentProductTable("/hardware/xraysource/CurrentSetpoint/Value", "4.0");
								}
								else
								{
									bool flag7 = XRayOnProcessing.modelNr60KV200WP760.Contains(value);
									if (flag7)
									{
										dbConnection.UpdateCurrentProductTable("/hardware/xraysource/CurrentSetpoint/Value", "4.0");
									}
									else
									{
										bool flag8 = XRayOnProcessing.modelNr100KV200W.Contains(value);
										if (flag8)
										{
											dbConnection.UpdateCurrentProductTable("/hardware/xraysource/CurrentSetpoint/Value", "4.0");
										}
										else
										{
											bool flag9 = XRayOnProcessing.modelNr100KV200WLonglife.Contains(value);
											if (flag9)
											{
												dbConnection.UpdateCurrentProductTable("/hardware/xraysource/CurrentSetpoint/Value", "3.0");
											}
											else
											{
												bool flag10 = XRayOnProcessing.modelNr100KV300W.Contains(value);
												if (flag10)
												{
													dbConnection.UpdateCurrentProductTable("/hardware/xraysource/CurrentSetpoint/Value", "4.0");
												}
												else
												{
													dbConnection.UpdateCurrentProductTable("/hardware/xraysource/CurrentSetpoint/Value", "1.5");
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06002E73 RID: 11891 RVA: 0x00131034 File Offset: 0x0012F234
		protected virtual void OnUnknownModelNumberDetected(string modelNumber)
		{
			Action<string> unknownModelNumberDetected = this.UnknownModelNumberDetected;
			bool flag = unknownModelNumberDetected != null;
			if (flag)
			{
				unknownModelNumberDetected(modelNumber);
			}
		}

		// Token: 0x06002E74 RID: 11892 RVA: 0x00131059 File Offset: 0x0012F259
		public static void EnableXray(ParameterHandling parHandling)
		{
			parHandling.WriteValue(XRayOnProcessing.xrayEnablePath, "1", false);
		}

		// Token: 0x06002E75 RID: 11893 RVA: 0x00131070 File Offset: 0x0012F270
		public static void DisableXray(ParameterHandling parHandling, DBConnection dBConnection, bool switchBulkParams)
		{
			int num = 0;
			MachineMode machineMode = MachineMode.Standard;
			bool flag = dBConnection != null;
			if (flag)
			{
				dBConnection.GetMachineMode(ref machineMode);
			}
			bool flag2 = parHandling.ReadPar(XRayOnProcessing.xrayEnablePath) == "1";
			if (flag2)
			{
				bool flag3 = machineMode == MachineMode.Bulk && switchBulkParams;
				if (flag3)
				{
					parHandling.WriteValue("/filter/acquisition/ContinuousMode/Value", "0", false);
					parHandling.WriteValue("/filter/acquisition/SendImage/Value", "0", false);
					parHandling.WriteValue("/filter/acquisition/SendOnlyIfContaminant/Value", "0", false);
					bool flag4 = parHandling.ReadPar(FlapsOverIOs.bulkflapsEnabledPath) == "1";
					if (flag4)
					{
						parHandling.WriteValue(FlapsOverIOs.bulkflapsEnableValvesPath, "0", false);
					}
					else
					{
						parHandling.WriteValue(SVT.svtValvesEnablePath, "0", false);
					}
					Thread.Sleep(1000);
				}
				string s = parHandling.ReadPar(Definition.ioVersionPath);
				int.TryParse(s, out num);
				bool flag5 = num >= Definition.wagoNewWiringStartVersion;
				if (flag5)
				{
					parHandling.WriteValue(XRayOnProcessing.xrayEnableLampInterlockBypass, "0", false);
					parHandling.WriteValue(XRayOnProcessing.xrayEnableLampInterlockBypass, "1", false);
				}
				parHandling.WriteValue(XRayOnProcessing.xrayEnablePath, "0", false);
			}
		}

		// Token: 0x06002E76 RID: 11894 RVA: 0x001311A8 File Offset: 0x0012F3A8
		private void EnableValvesConsecutively(int nValves)
		{
			int num = 1000;
			bool flag = nValves > 0;
			if (flag)
			{
				num = 5000 / nValves;
			}
			this.parHandling.WriteValue(SVT.svtEjectTimePath, "127", false);
			this.parHandling.WriteValue(SVT.svtTestStartValvePath, "1", false);
			this.parHandling.WriteValue(SVT.svtTestEndValvePath, nValves.ToString(), false);
			this.parHandling.WriteValue(SVT.svtTestIntervalPath, num.ToString(), false);
			this.parHandling.WriteValue(SVT.svtPeriodicModePath, SVT.svtPeriodicModeOneShotIndicator.ToString(), false);
		}

		// Token: 0x06002E77 RID: 11895 RVA: 0x0013124C File Offset: 0x0012F44C
		private void EnableFlapsConsecutively()
		{
			string value = this.parHandling.ReadPar(FlapsOverIOs.bulkflapsValveTestIntervalPath);
			this.parHandling.WriteValue(FlapsOverIOs.bulkflapsValveTestIntervalPath, "500");
			this.parHandling.WriteValue(FlapsOverIOs.bulkflapsValveTestStartPath, "1");
			Thread.Sleep(2000);
			this.parHandling.WriteValue(FlapsOverIOs.bulkflapsValveTestIntervalPath, value);
			this.parHandling.WriteValue(FlapsOverIOs.bulkflapsValveTestStartPath, "0");
			this.parHandling.WriteValue(FlapsOverIOs.bulkflapsValveTestIsRunningPath, "0");
		}

		// Token: 0x06002E78 RID: 11896 RVA: 0x001312E0 File Offset: 0x0012F4E0
		private void RestoreValvesParameter()
		{
			string value = null;
			int num = 0;
			this.dbConnection.GetParameterValue(SVT.svtEjectTimePath, ref value);
			this.parHandling.WriteValue(SVT.svtEjectTimePath, value, false);
			this.dbConnection.GetValveTestStartValve(ref num);
			this.parHandling.WriteValue(SVT.svtTestStartValvePath, num.ToString(), false);
			this.dbConnection.GetValveTestEndValve(ref num);
			this.parHandling.WriteValue(SVT.svtTestEndValvePath, num.ToString(), false);
			this.dbConnection.GetValveTestInterval(ref num);
			this.parHandling.WriteValue(SVT.svtTestIntervalPath, num.ToString(), false);
		}

		// Token: 0x06002E79 RID: 11897 RVA: 0x00131390 File Offset: 0x0012F590
		public static int GetMaxKV(ParameterHandling parHandling)
		{
			int result = 50;
			string value = parHandling.ReadPar(XRayOnProcessing.xrayModelNumberPath);
			bool flag = XRayOnProcessing.modelNr60KV200W.Contains(value) || XRayOnProcessing.modelNr60KV200WP760.Contains(value);
			if (flag)
			{
				result = 60;
			}
			else
			{
				bool flag2 = XRayOnProcessing.modelNr80KV.Contains(value);
				if (flag2)
				{
					result = 80;
				}
				else
				{
					bool flag3 = XRayOnProcessing.modelNr100KV150W.Contains(value) || XRayOnProcessing.modelNr100KV200W.Contains(value) || XRayOnProcessing.modelNr100KV200WLonglife.Contains(value) || XRayOnProcessing.modelNr100KV300W.Contains(value);
					if (flag3)
					{
						result = 100;
					}
				}
			}
			return result;
		}

		// Token: 0x06002E7A RID: 11898 RVA: 0x00131430 File Offset: 0x0012F630
		public static double GetMaxMA(ParameterHandling parHandling)
		{
			double result = 1.0;
			string value = parHandling.ReadPar(XRayOnProcessing.xrayModelNumberPath);
			bool flag = XRayOnProcessing.modelNr50KV60W.Contains(value);
			if (flag)
			{
				result = 1.5;
			}
			else
			{
				bool flag2 = XRayOnProcessing.modelNr100KV150W.Contains(value) || XRayOnProcessing.modelNr100KV200WLonglife.Contains(value);
				if (flag2)
				{
					result = 3.0;
				}
				else
				{
					bool flag3 = XRayOnProcessing.modelNr60KV200W.Contains(value) || XRayOnProcessing.modelNr100KV200W.Contains(value);
					if (flag3)
					{
						result = 5.0;
					}
					else
					{
						bool flag4 = XRayOnProcessing.modelNr60KV200WP760.Contains(value);
						if (flag4)
						{
							result = 4.0;
						}
						else
						{
							bool flag5 = XRayOnProcessing.modelNr100KV300W.Contains(value);
							if (flag5)
							{
								result = 5.0;
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06002E7B RID: 11899 RVA: 0x0013150C File Offset: 0x0012F70C
		public static int GetMaxWatt(ParameterHandling parHandling)
		{
			int result = 100;
			string value = parHandling.ReadPar(XRayOnProcessing.xrayModelNumberPath);
			bool flag = XRayOnProcessing.modelNr100KV150W.Contains(value);
			if (flag)
			{
				result = 150;
			}
			else
			{
				bool flag2 = XRayOnProcessing.modelNr60KV200W.Contains(value) || XRayOnProcessing.modelNr60KV200WP760.Contains(value) || XRayOnProcessing.modelNr100KV200W.Contains(value) || XRayOnProcessing.modelNr100KV200WLonglife.Contains(value);
				if (flag2)
				{
					result = 200;
				}
				else
				{
					bool flag3 = XRayOnProcessing.modelNr100KV300W.Contains(value);
					if (flag3)
					{
						result = 200;
					}
					else
					{
						bool flag4 = XRayOnProcessing.modelNr50KV60W.Contains(value);
						if (flag4)
						{
							result = 60;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06002E7C RID: 11900 RVA: 0x001315C0 File Offset: 0x0012F7C0
		private bool UpdateRemainingReseasoningTime(int stepTime, ref int remainingTime)
		{
			for (int i = 0; i < stepTime; i++)
			{
				Thread.Sleep(1000);
				remainingTime--;
				this.OnRemainingReseasonTimeChanged(remainingTime);
				string a = this.parHandling.ReadPar(XrayState.xrayPowerOnPath);
				bool flag = a != "1";
				if (flag)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06002E7D RID: 11901 RVA: 0x0013162C File Offset: 0x0012F82C
		public static void SetRealLimitOpHoursXRaySource(DBConnection dbConnection, ParameterHandling parHandling, int reallimit)
		{
			bool flag = reallimit == 0;
			if (flag)
			{
				reallimit = 99999;
				dbConnection.SetXRayOpHoursLimit(reallimit);
				parHandling.WriteValue(Definition.limitXraySourceOperatingHoursPath, Convert.ToString(reallimit));
			}
		}

		// Token: 0x06002E7E RID: 11902 RVA: 0x00131668 File Offset: 0x0012F868
		private void PrepareTdiDetector()
		{
			ConveyingDirection conveyingDirection = ConveyingDirection.LeftToRight;
			this.dbConnection.GetConveyingDirection(ref conveyingDirection);
			this.meanPixelLogger.TdiOffsetSaved = false;
			this.meanPixelLogger.TdiGainSaved = false;
			XRayOnProcessing.CalibrateMaxPossibleNoTdi = false;
			this.parHandling.WriteValue("/filter/acquisition/AddedFrontLines/Value", "0", false);
			this.parHandling.WriteValue("/filter/acquisition/AddedBackLines/Value", "0", false);
			XRayOnProcessing.LogTdiCalib("05 - Writing TdiCorrectionMode to NoTDI");
			bool flag = conveyingDirection == ConveyingDirection.LeftToRight;
			if (flag)
			{
				this.parHandling.WriteValue("/hardware/detector/dt/TdiCorrectionMode/Value", 0.ToString(), false);
			}
			else
			{
				this.parHandling.WriteValue("/hardware/detector/dt/TdiCorrectionMode/Value", 16.ToString(), false);
			}
			string text = this.parHandling.ReadPar("/hardware/detector/dt/TdiCorrectionMode/Value");
			XRayOnProcessing.LogTdiCalib("06 - TdiCorrectionMode is " + text + " " + ((TdiCorrectionMode)Convert.ToInt32(text)).ToString());
			int adaptedIntegrationTime = XRayOnProcessing.AdaptedIntegrationTime;
			bool flag2 = adaptedIntegrationTime < 631 && adaptedIntegrationTime != 0;
			if (flag2)
			{
				XRayOnProcessing.CalibrateMaxPossibleNoTdi = true;
				XRayOnProcessing.CurrentIntegrationTimeTdi = adaptedIntegrationTime;
				XRayOnProcessing.TdiGainFactor = 631.0 / (double)adaptedIntegrationTime;
				this.parHandling.WriteValue(Definition.detectorDTIntegrationTimePath, 631.ToString());
			}
			this.parHandling.WriteValue("/hardware/detector/dt/AdOutputValidBitRange/Value", "3", false);
			this.parHandling.WriteValue("/filter/acquisition/Threshold/Value", "60000", false);
			this.parHandling.WriteValue("/hardware/detector/dt/LowGain/Value", "4", false);
			this.parHandling.WriteValue("/hardware/detector/dt/HighGain/Value", "4", false);
			XRayOnProcessing.LogTdiCalib("07 - Doing ADOffsetCorrection");
			this.parHandling.WriteValue("/hardware/detector/dt/DoAdOffsetCorrection/Value", "1", false);
			Thread.Sleep(2500);
		}

		// Token: 0x06002E7F RID: 11903 RVA: 0x0013184C File Offset: 0x0012FA4C
		public static void LogTdiCalib(string calibMessage)
		{
			string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\" + Definition.tdiCalibFolder + "\\TdiCalibLog.txt";
			StreamWriter streamWriter = new StreamWriter(path, true);
			streamWriter.Write(DateTime.Now.ToString("dd:MM:yyyy HH:mm:ss.fff") + "\t" + calibMessage + Environment.NewLine);
			streamWriter.Close();
		}

		// Token: 0x04001D0A RID: 7434
		private string[,] reseasonKvMaSpellman50kV100W;

		// Token: 0x04001D0B RID: 7435
		private string[,] reseasonKvMaVJ50kV100W;

		// Token: 0x04001D0C RID: 7436
		private string[,] reseasonKvMa60kV200W;

		// Token: 0x04001D0D RID: 7437
		private string[,] reseasonKvMa60kV200WP760;

		// Token: 0x04001D0E RID: 7438
		private string[,] reseasonKvMa80kV100W;

		// Token: 0x04001D0F RID: 7439
		private string[,] reseasonKvMa100kV150W;

		// Token: 0x04001D10 RID: 7440
		private string[,] reseasonKvMa100kV200W;

		// Token: 0x04001D11 RID: 7441
		private string[,] reseasonKvMa50kV60W;

		// Token: 0x04001D12 RID: 7442
		private string[,] reseasonKvMa100kV300W;

		// Token: 0x04001D13 RID: 7443
		private const int maxCalibrationWaitTime = 20;

		// Token: 0x04001D14 RID: 7444
		private static string[] modelNr50KV_VJ = new string[]
		{
			"IXS050SE100P214"
		};

		// Token: 0x04001D15 RID: 7445
		private static string[] modelNr50KV_Spellman = new string[]
		{
			"XRB50N100",
			"X4209",
			"X4771"
		};

		// Token: 0x04001D16 RID: 7446
		private static string[] modelNr80KV = new string[]
		{
			"XRB80N100",
			"X4128",
			"X4391",
			"IXS080SE100P213"
		};

		// Token: 0x04001D17 RID: 7447
		private static string[] modelNr100KV150W = new string[]
		{
			"IXS100SE150P274",
			"IXS100SE150P290"
		};

		// Token: 0x04001D18 RID: 7448
		public static string X5885Model = "XRB100PN210HRX5885";

		// Token: 0x04001D19 RID: 7449
		private static string[] modelNr60KV200W = new string[]
		{
			"IXS060BP200P630"
		};

		// Token: 0x04001D1A RID: 7450
		public static string[] modelNr60KV200WP760 = new string[]
		{
			"IXS060BP200P760"
		};

		// Token: 0x04001D1B RID: 7451
		private static string[] modelNr100KV200W = new string[]
		{
			"IXS100BP200P367",
			"IXS100BP200P395",
			XRayOnProcessing.X5885Model
		};

		// Token: 0x04001D1C RID: 7452
		private static string[] modelNr100KV200WLonglife = new string[]
		{
			"IXS100BP200P380"
		};

		// Token: 0x04001D1D RID: 7453
		public static string X5884Model = "X5884";

		// Token: 0x04001D1E RID: 7454
		private static string[] modelNr50KV60W = new string[]
		{
			"IXS050SE060P359",
			XRayOnProcessing.X5884Model
		};

		// Token: 0x04001D1F RID: 7455
		private static string[] modelNr100KV300W = new string[]
		{
			"IXS100BP300P381"
		};

		// Token: 0x04001D20 RID: 7456
		public static readonly string xrayModelNumberPath = "/hardware/xraysource/ModelNumber/Value";

		// Token: 0x04001D21 RID: 7457
		public static readonly string xrayEnablePath = "/hardware/xraysource/XRayEnable/Value";

		// Token: 0x04001D22 RID: 7458
		public static readonly string xrayEnableLampInterlockBypass = "/hardware/ios/LampInterlockBypassOn/Value";

		// Token: 0x04001D23 RID: 7459
		private const string dtOffsetCalibrationPath = "/hardware/detector/dt/OffsetCalibration/Value";

		// Token: 0x04001D24 RID: 7460
		private const string dtGainCalibrationPath = "/hardware/detector/dt/GainCalibration/Value";

		// Token: 0x04001D25 RID: 7461
		public static readonly string dtBaselineEnablePath = "/hardware/detector/dt/BaselineEnable/Value";

		// Token: 0x04001D26 RID: 7462
		public static readonly string dtOffsetEnablePath = "/hardware/detector/dt/OffsetEnable/Value";

		// Token: 0x04001D27 RID: 7463
		public static readonly string dtGainEnablePath = "/hardware/detector/dt/GainEnable/Value";

		// Token: 0x04001D28 RID: 7464
		public static readonly string dtBaselineValuePath = "/hardware/detector/dt/BaselineValue/Value";

		// Token: 0x04001D29 RID: 7465
		public static readonly string xrayVoltagePath = "/hardware/xraysource/Voltage/Value";

		// Token: 0x04001D2A RID: 7466
		public const string xrayVoltageSetpointPath = "/hardware/xraysource/VoltageSetpoint/Value";

		// Token: 0x04001D2B RID: 7467
		public static readonly string xrayCurrentPath = "/hardware/xraysource/Current/Value";

		// Token: 0x04001D2C RID: 7468
		public static readonly string manualOffsetCalibrationPath = "/hardware/detector/ManualOffsetCalibration/Value";

		// Token: 0x04001D2D RID: 7469
		public static readonly string manualGainCalibrationPath = "/hardware/detector/ManualGainCalibration/Value";

		// Token: 0x04001D2E RID: 7470
		public const string xrayCurrentSetpointPath = "/hardware/xraysource/CurrentSetpoint/Value";

		// Token: 0x04001D2F RID: 7471
		public static readonly int gainCalibrationValue = 10000;

		// Token: 0x04001D30 RID: 7472
		public static readonly int baselineValueInAutoMode = 500;

		// Token: 0x04001D31 RID: 7473
		private DBConnection dbConnection;

		// Token: 0x04001D32 RID: 7474
		private ParameterHandling parHandling;

		// Token: 0x04001D33 RID: 7475
		private long timePassedSinceXrayOn;

		// Token: 0x04001D34 RID: 7476
		private MeanPixelLogger meanPixelLogger;

		// Token: 0x04001D35 RID: 7477
		private Semaphore offsetLoggingSemaphore;

		// Token: 0x04001D36 RID: 7478
		private PixelCorrection pixelCorrection;

		// Token: 0x04001D3A RID: 7482
		private LiveCalibration liveCalibration;

		// Token: 0x04001D3B RID: 7483
		public const int minIntegrationTimeNoTdi512 = 631;

		// Token: 0x04001D3C RID: 7484
		public const int minIntegrationTimeNoTdi820 = 841;

		// Token: 0x04001D3E RID: 7486
		public static bool CalibrateMaxPossibleNoTdi = false;

		// Token: 0x04001D40 RID: 7488
		public static bool CalibrateInPlot = false;

		// Token: 0x04001D41 RID: 7489
		public static int ConveyingScanrate;

		// Token: 0x04001D42 RID: 7490
		public static int AdaptedIntegrationTime;
	}
}
