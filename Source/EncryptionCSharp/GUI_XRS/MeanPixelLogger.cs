using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using DatabaseConnection;
using Definitions;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x02000129 RID: 297
	public class MeanPixelLogger
	{
		// Token: 0x14000091 RID: 145
		// (add) Token: 0x06001CA9 RID: 7337 RVA: 0x000B6F34 File Offset: 0x000B5134
		// (remove) Token: 0x06001CAA RID: 7338 RVA: 0x000B6F6C File Offset: 0x000B516C
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<int> OffsetValuesSaved;

		// Token: 0x14000092 RID: 146
		// (add) Token: 0x06001CAB RID: 7339 RVA: 0x000B6FA4 File Offset: 0x000B51A4
		// (remove) Token: 0x06001CAC RID: 7340 RVA: 0x000B6FDC File Offset: 0x000B51DC
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<GainCalibrationInfo> GainValuesSaved;

		// Token: 0x17000870 RID: 2160
		// (get) Token: 0x06001CAD RID: 7341 RVA: 0x000B7011 File Offset: 0x000B5211
		// (set) Token: 0x06001CAE RID: 7342 RVA: 0x000B7019 File Offset: 0x000B5219
		public bool LogModeStarted { get; set; }

		// Token: 0x17000871 RID: 2161
		// (get) Token: 0x06001CAF RID: 7343 RVA: 0x000B7022 File Offset: 0x000B5222
		// (set) Token: 0x06001CB0 RID: 7344 RVA: 0x000B702A File Offset: 0x000B522A
		public List<double> PixelGainsLine { get; set; }

		// Token: 0x06001CB1 RID: 7345 RVA: 0x000B7034 File Offset: 0x000B5234
		public MeanPixelLogger(DBConnection dbConnection, ParameterHandling parHandling, ImageDataTransfer imageTransfer, DBDataLogs dBDataLogs)
		{
			this.dbConnection = dbConnection;
			this.parHandling = parHandling;
			this.imageTransfer = imageTransfer;
			this.dBDataLogs = dBDataLogs;
			this.ignoreFirstImage = true;
			this.PixelGainsLine = new List<double>();
		}

		// Token: 0x06001CB2 RID: 7346 RVA: 0x000B7093 File Offset: 0x000B5293
		public void StartPixelLogging(MeanPixelLogger.LogMode dbLogMode, bool saveValue)
		{
			this.StartPixelLogging(dbLogMode, saveValue, true, 0);
		}

		// Token: 0x06001CB3 RID: 7347 RVA: 0x000B70A4 File Offset: 0x000B52A4
		public void StartPixelLogging(MeanPixelLogger.LogMode dbLogMode, bool saveValue, bool offsetMode, int calibrationValue)
		{
			this.dbLogMode = dbLogMode;
			this.offsetMode = offsetMode;
			this.calibrationValue = calibrationValue;
			this.saveValue = saveValue;
			this.ignoreFirstImage = true;
			bool flag = dbLogMode == MeanPixelLogger.LogMode.Database;
			if (flag)
			{
				this.StartDbLogging();
			}
			else
			{
				bool flag2 = dbLogMode == MeanPixelLogger.LogMode.CSV;
				if (flag2)
				{
					this.StartCsvLogging();
				}
			}
		}

		// Token: 0x06001CB4 RID: 7348 RVA: 0x000B70F8 File Offset: 0x000B52F8
		private void StartDbLogging()
		{
			this.LogModeStarted = true;
			this.SetImageProcessingParameters();
		}

		// Token: 0x06001CB5 RID: 7349 RVA: 0x000B710C File Offset: 0x000B530C
		private void StartCsvLogging()
		{
			this.LogModeStarted = true;
			StreamWriter streamWriter = null;
			DetectorType detectorType = DetectorType.DT;
			this.dbConnection.GetDetectorType(ref detectorType);
			string s = (detectorType == DetectorType.DT || detectorType == DetectorType.DTNetSE || detectorType == DetectorType.DTNetDE || detectorType == DetectorType.DTNetTDI) ? this.parHandling.ReadPar(Definition.detectorDTNPixelPath) : this.parHandling.ReadPar(Definition.detectorSesotecNPixelPath);
			int num;
			bool flag = int.TryParse(s, out num);
			if (flag)
			{
				this.currentFileName = this.CreateLogFileName();
				try
				{
					streamWriter = new StreamWriter(this.currentFileName, false);
					streamWriter.Write(Definition.csvSeparator);
					streamWriter.Write("MCUTemperature" + Definition.csvSeparator);
					streamWriter.Write("TemperatureTop" + Definition.csvSeparator);
					streamWriter.Write("TemperatureBottom" + Definition.csvSeparator);
					for (int i = 0; i < num - 1; i++)
					{
						streamWriter.Write("Pixel " + i.ToString() + Definition.csvSeparator);
					}
					streamWriter.Write("Pixel " + (num - 1).ToString() + Environment.NewLine);
					streamWriter.Close();
					this.SetImageProcessingParameters();
				}
				catch (IOException)
				{
					bool flag2 = streamWriter != null;
					if (flag2)
					{
						streamWriter.Close();
					}
				}
				finally
				{
					bool flag3 = streamWriter != null;
					if (flag3)
					{
						streamWriter.Close();
					}
				}
			}
		}

		// Token: 0x06001CB6 RID: 7350 RVA: 0x000B7294 File Offset: 0x000B5494
		private void SetImageProcessingParameters()
		{
			DetectorType detectorType = DetectorType.DT;
			DetectorSubType detectorSubType = DetectorSubType.None;
			this.lastMCUTemperature = -1;
			int num = 0;
			int num2 = 700;
			this.parHandling.WriteValue(ParameterHandling.ignoreLeftPixelPath, "0", false);
			this.parHandling.WriteValue(ParameterHandling.ignoreRightPixelPath, "0", false);
			this.dbConnection.GetDetectorType(ref detectorType);
			string a = this.parHandling.ReadPar(Definition.detectorSubtypePath);
			bool flag = a == "1";
			if (flag)
			{
				detectorSubType = DetectorSubType.SesotecHS;
			}
			string path = (detectorType == DetectorType.DT || detectorType == DetectorType.DTNetSE || detectorType == DetectorType.DTNetDE || detectorType == DetectorType.DTNetTDI) ? Definition.detectorDTScanratePath : Definition.detectorSesotecScanratePath;
			string s = this.parHandling.ReadPar(path);
			bool flag2 = detectorType == DetectorType.DT || detectorType == DetectorType.DTNetSE || detectorType == DetectorType.DTNetDE;
			if (flag2)
			{
				int num3 = 0;
				string s2 = this.parHandling.ReadPar(Conveyor.detectorDTAverageFilterSizePath);
				int.TryParse(s2, out num3);
				bool flag3 = int.TryParse(s, out num) && num > 0;
				if (flag3)
				{
					num2 = (int)((double)num / Math.Pow(2.0, (double)num3));
				}
			}
			else
			{
				bool flag4 = detectorType == DetectorType.Sesotec;
				if (flag4)
				{
					int num4 = 0;
					string s3 = this.parHandling.ReadPar(Conveyor.detectorSesotecAveragedLinesSizePath);
					int.TryParse(s3, out num4);
					bool flag5 = int.TryParse(s, out num) && num > 0;
					if (flag5)
					{
						num2 = num / num4;
					}
				}
			}
			bool flag6 = detectorType == DetectorType.Sesotec && detectorSubType == DetectorSubType.SesotecHS;
			if (flag6)
			{
				bool flag7 = num2 > 2000;
				if (flag7)
				{
					num2 = 2000;
				}
			}
			else
			{
				bool flag8 = num2 > 1000;
				if (flag8)
				{
					num2 = 1000;
				}
			}
			this.parHandling.WriteValue(Acquisition.nMaxLinesPath, num2.ToString(), false);
			this.imageTransfer.FrameArrived += this.imageTransfer_FrameArrived;
			this.parHandling.WriteValue(ParameterHandling.continuousModePath, "1", false);
			bool flag9 = detectorType == DetectorType.DTNetTDI;
			if (flag9)
			{
				XRayOnProcessing.LogTdiCalib("   - Set /filter/acquisition/ContinuousMode to 1");
			}
		}

		// Token: 0x06001CB7 RID: 7351 RVA: 0x000B74B8 File Offset: 0x000B56B8
		public void StopPixelLogging(bool resetMaxLines)
		{
			string s = "";
			this.dbConnection.GetIgnoredLeftPixel(ref this.oldIgnoredLeftPixel);
			this.dbConnection.GetIgnoredRightPixel(ref this.oldIgnoredRightPixel);
			this.dbConnection.GetParameterValue(Acquisition.nMaxLinesPath, ref s);
			int.TryParse(s, out this.oldNMaxLines);
			this.imageTransfer.FrameArrived -= this.imageTransfer_FrameArrived;
			this.LogModeStarted = false;
			this.parHandling.WriteValue(ParameterHandling.continuousModePath, "0", false);
			bool flag = this.oldIgnoredLeftPixel != 0;
			if (flag)
			{
				this.parHandling.WriteValue(ParameterHandling.ignoreLeftPixelPath, this.oldIgnoredLeftPixel.ToString(), false);
			}
			bool flag2 = this.oldIgnoredRightPixel != 0;
			if (flag2)
			{
				this.parHandling.WriteValue(ParameterHandling.ignoreRightPixelPath, this.oldIgnoredRightPixel.ToString(), false);
			}
			bool flag3 = resetMaxLines && this.oldNMaxLines > 0;
			if (flag3)
			{
				this.parHandling.WriteValue(Acquisition.nMaxLinesPath, this.oldNMaxLines.ToString(), false);
			}
		}

		// Token: 0x06001CB8 RID: 7352 RVA: 0x000B75CC File Offset: 0x000B57CC
		private void imageTransfer_FrameArrived(object sender, ReceiveEventArgs e)
		{
			DetectorType detectorType = DetectorType.DT;
			this.dbConnection.GetDetectorType(ref detectorType);
			MachineMode machineMode = MachineMode.Standard;
			this.dbConnection.GetMachineMode(ref machineMode);
			bool flag = machineMode == MachineMode.Bulk;
			if (flag)
			{
				bool flag2 = this.ignoreFirstImage;
				if (flag2)
				{
					this.ignoreFirstImage = false;
					return;
				}
			}
			bool flag3 = detectorType == DetectorType.DTNetTDI;
			if (flag3)
			{
				XRayOnProcessing.LogTdiCalib("   - MeanPixelLogger FrameArrived");
				XRayOnProcessing.LogTdiCalib("   - NumPixel detector " + this.parHandling.ReadPar("/hardware/detector/dt/NumPixel/Value"));
			}
			lock (this)
			{
				bool flag5 = this.parHandling.ReadPar(Definition.isLiveCalibrationActivePath) == "1" || this.parHandling.ReadPar(Definition.isLiveCalibrationActivePath) == "2";
				if (flag5)
				{
					bool saveNotinDBLiveCalib = this.parHandling.ReadPar(Definition.LiveCalibrationSendImg) == "0";
					bool flag6 = detectorType == DetectorType.DTNetTDI;
					if (flag6)
					{
						bool flag7 = e.Image.Width != 10240;
						if (flag7)
						{
							XRayOnProcessing.LogTdiCalib("Error Image.Width != 10240 Width=" + e.Image.Width.ToString());
							return;
						}
						XRayOnProcessing.LogTdiCalib("   - Image arrived Width: " + e.Image.Width.ToString() + " Height: " + e.Image.Height.ToString());
						this.parHandling.WriteValue("/hardware/detector/dt/StartStopScanning/Value", "0", false);
						XRayOnProcessing.LogTdiCalib("   - Check if scanning stopped. Scanning is: " + this.parHandling.ReadPar("/hardware/detector/dt/StartStopScanning/Value"));
						this.parHandling.WriteValue(Acquisition.nMaxLinesPath, "1", false);
					}
					this.WritePixelValuesToDatabase(e.Image, saveNotinDBLiveCalib);
				}
				else
				{
					bool flag8 = detectorType == DetectorType.DTNetTDI;
					if (flag8)
					{
						bool flag9 = e.Image.Width != 10240;
						if (flag9)
						{
							XRayOnProcessing.LogTdiCalib("Error Image.Width != 10240 Width=" + e.Image.Width.ToString());
						}
						else
						{
							this.MCUTemperatureChanged();
							XRayOnProcessing.LogTdiCalib("   - Image arrived Width: " + e.Image.Width.ToString() + " Height: " + e.Image.Height.ToString());
							this.parHandling.WriteValue("/hardware/detector/dt/StartStopScanning/Value", "0", false);
							XRayOnProcessing.LogTdiCalib("   - Check if scanning stopped. Scanning is: " + this.parHandling.ReadPar("/hardware/detector/dt/StartStopScanning/Value"));
							this.parHandling.WriteValue(Acquisition.nMaxLinesPath, "1", false);
							this.WritePixelValuesToDatabase(e.Image, false);
						}
					}
					else
					{
						bool flag10 = this.MCUTemperatureChanged();
						if (flag10)
						{
							bool flag11 = this.dbLogMode == MeanPixelLogger.LogMode.Database;
							if (flag11)
							{
								this.WritePixelValuesToDatabase(e.Image, false);
							}
							else
							{
								bool flag12 = this.dbLogMode == MeanPixelLogger.LogMode.CSV;
								if (flag12)
								{
									this.WritePixelValuesToCsv(e.Image);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06001CB9 RID: 7353 RVA: 0x000B7928 File Offset: 0x000B5B28
		private string CreateLogFileName()
		{
			DateTime now = DateTime.Now;
			string text = now.Year.ToString();
			text += "-";
			text += string.Format("{0:D2}", now.Month);
			text += "-";
			text += string.Format("{0:D2}", now.Day);
			text += "_";
			text += string.Format("{0:D2}", now.Hour);
			text += "-";
			text += string.Format("{0:D2}", now.Minute);
			text += "-";
			text += string.Format("{0:D2}", now.Second);
			return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\LogPixel_" + text + ".csv";
		}

		// Token: 0x06001CBA RID: 7354 RVA: 0x000B7A3C File Offset: 0x000B5C3C
		private bool MCUTemperatureChanged()
		{
			DetectorType detectorType = DetectorType.DT;
			this.dbConnection.GetDetectorType(ref detectorType);
			string path = (detectorType == DetectorType.DT || detectorType == DetectorType.DTNetSE || detectorType == DetectorType.DTNetDE || detectorType == DetectorType.DTNetTDI) ? Definition.dtMCUTemperaturePath : Definition.detectorSesotecTemperaturePath;
			string s = this.parHandling.ReadPar(path);
			int num;
			bool flag = int.TryParse(s, out num);
			if (flag)
			{
				bool flag2 = num > this.lastMCUTemperature;
				if (flag2)
				{
					this.lastMCUTemperature = num;
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001CBB RID: 7355 RVA: 0x000B7ABC File Offset: 0x000B5CBC
		private void WritePixelValuesToCsv(XRSImage image)
		{
			long num = 0L;
			bool flag = this.currentFileName != null && File.Exists(this.currentFileName);
			if (flag)
			{
				StreamWriter streamWriter = new StreamWriter(this.currentFileName, true);
				streamWriter.Write(DateTime.Now.ToString() + Definition.csvSeparator);
				streamWriter.Write(this.lastMCUTemperature.ToString() + Definition.csvSeparator);
				streamWriter.Write(this.parHandling.ReadPar("/hardware/ios/AI_07/Value").Replace('.', ',') + Definition.csvSeparator);
				streamWriter.Write(this.parHandling.ReadPar("/hardware/ios/AI_06/Value").Replace('.', ',') + Definition.csvSeparator);
				for (int i = 0; i < (int)(image.Width - 1); i++)
				{
					num = 0L;
					for (int j = 0; j < (int)image.Height; j++)
					{
						num += (long)image.Data[j * (int)image.Width + i];
					}
					num /= (long)image.Height;
					streamWriter.Write(num.ToString() + Definition.csvSeparator);
				}
				for (int k = 0; k < (int)image.Height; k++)
				{
					num += (long)image.Data[k * (int)image.Width + (int)image.Width - 1];
				}
				streamWriter.Write((num / (long)image.Height).ToString() + Environment.NewLine);
				streamWriter.Close();
				bool flag2 = this.offsetMode;
				if (flag2)
				{
					this.OnOffsetValuesSaved(this.lastMCUTemperature);
				}
				else
				{
					string s = null;
					string s2 = null;
					double voltageSetpoiint = 0.0;
					double currentSetpoint = 0.0;
					int scanrate = 0;
					DetectorType detectorType = DetectorType.DT;
					string path = "";
					this.dbConnection.GetParameterValue("/hardware/xraysource/VoltageSetpoint/Value", ref s);
					this.dbConnection.GetParameterValue("/hardware/xraysource/CurrentSetpoint/Value", ref s2);
					string path2 = (detectorType == DetectorType.DT || detectorType == DetectorType.DTNetSE || detectorType == DetectorType.DTNetDE || detectorType == DetectorType.DTNetTDI) ? Definition.detectorDTScanratePath : Definition.detectorSesotecScanratePath;
					string s3 = this.parHandling.ReadPar(path2);
					string text = this.parHandling.ReadPar(path);
					double.TryParse(s, NumberStyles.Float, new CultureInfo("en-US"), out voltageSetpoiint);
					double.TryParse(s2, NumberStyles.Float, new CultureInfo("en-US"), out currentSetpoint);
					int.TryParse(s3, out scanrate);
					this.OnGainValuesSaved(new GainCalibrationInfo(this.lastMCUTemperature, scanrate, voltageSetpoiint, currentSetpoint, false));
				}
			}
		}

		// Token: 0x06001CBC RID: 7356 RVA: 0x000B7D74 File Offset: 0x000B5F74
		public void WritePixelValuesToDatabase(XRSImage image, bool saveNotinDBLiveCalib)
		{
			List<double> list = new List<double>();
			List<int> list2 = new List<int>();
			List<int> list3 = new List<int>();
			List<double> signalDectector = new List<double>();
			string serial = null;
			DetectorType detectorType = DetectorType.DT;
			bool flag = true;
			bool flag2 = true;
			string path = Definition.detectorSesotecNPixelPath;
			string text = null;
			int num = Convert.ToInt32(this.parHandling.ReadPar(Definition.isLiveCalibrationActivePath));
			bool flag3 = num > 0;
			bool flag4 = this.parHandling.ReadPar(Definition.detectorMonitoringActive) == "1";
			this.dbConnection.GetDetectorType(ref detectorType);
			string path2 = (detectorType == DetectorType.DT || detectorType == DetectorType.DTNetSE || detectorType == DetectorType.DTNetDE || detectorType == DetectorType.DTNetTDI) ? Definition.detectorDTSerialPath : Definition.detectorSesotecMACPath;
			string path3 = (detectorType == DetectorType.DT || detectorType == DetectorType.DTNetSE || detectorType == DetectorType.DTNetDE || detectorType == DetectorType.DTNetTDI) ? Definition.detectorDTScanratePath : Definition.detectorSesotecScanratePath;
			text = this.parHandling.ReadPar(path3);
			serial = this.parHandling.ReadPar(path2);
			bool flag5 = detectorType == DetectorType.DTNetSE || detectorType == DetectorType.DTNetDE || detectorType == DetectorType.DTNetTDI;
			if (flag5)
			{
				path = Definition.detectorDTNPixelPath;
			}
			bool flag6 = (int)image.Width != Convert.ToInt32(this.parHandling.ReadPar(path));
			if (flag6)
			{
				flag2 = false;
			}
			this.PixelGainsLine.Clear();
			bool flag7 = detectorType == DetectorType.DTNetTDI && !this.offsetMode;
			if (flag7)
			{
				MeanPixelLogger.nTdiDefectPixels.Clear();
				MeanPixelLogger.nTdiDefectPixels.AddRange(Enumerable.Repeat<int>(0, 1280));
				MeanPixelLogger.noTdiDefectPixels.Clear();
				MeanPixelLogger.tdiDefectPixelList.Clear();
			}
			for (int i = 0; i < (int)image.Width; i++)
			{
				double num2 = 0.0;
				list2.Clear();
				int num3 = 0;
				for (int j = 0; j < (int)image.Height; j++)
				{
					bool flag8 = image.Data[j * (int)image.Width + i] > 0;
					if (flag8)
					{
						num2 += (double)image.Data[j * (int)image.Width + i];
						list2.Add((int)image.Data[j * (int)image.Width + i]);
						num3++;
					}
				}
				num2 /= (double)num3;
				bool flag9 = num2 == 0.0;
				if (flag9)
				{
					flag = false;
					break;
				}
				list.Add((double)((int)num2));
				list3.Add(list2.Max() - list2.Min());
				signalDectector.Add(Math.Round(list2.Average(), 2));
				bool flag10 = !this.offsetMode;
				if (flag10)
				{
					double num4 = 0.0;
					bool flag11 = num2 > 0.0;
					if (flag11)
					{
						num4 = (double)this.calibrationValue / num2;
					}
					bool flag12 = num4 < 0.0;
					if (flag12)
					{
						num4 = 0.0;
					}
					bool flag13 = detectorType == DetectorType.DTNetTDI;
					if (flag13)
					{
						bool flag14 = num4 > 100.0;
						if (flag14)
						{
							num4 = 1.0;
							DTNetNoTdiDefectPixel dtnetNoTdiDefectPixel = new DTNetNoTdiDefectPixel(i);
							MeanPixelLogger.noTdiDefectPixels.Add(dtnetNoTdiDefectPixel);
							List<int> list4 = MeanPixelLogger.nTdiDefectPixels;
							int tdiPixel = dtnetNoTdiDefectPixel.TdiPixel;
							int num5 = list4[tdiPixel];
							list4[tdiPixel] = num5 + 1;
						}
					}
					this.PixelGainsLine.Add(num4);
				}
			}
			bool flag15 = detectorType == DetectorType.DTNetTDI && !this.offsetMode;
			if (flag15)
			{
				MeanPixelLogger.tdiDefectPixelList.AddNoTdiDefectPixelList(MeanPixelLogger.noTdiDefectPixels);
			}
			bool flag16 = !this.offsetMode;
			if (flag16)
			{
				bool flag17 = false;
				this.dbConnection.GetDBDatalogsIsActive(ref flag17);
				int num6 = list3.Max();
				int num7 = list3.IndexOf(num6);
				bool flag18 = flag17;
				if (flag18)
				{
					this.saveBackgroundindex(list.Average(), text, num6, num7, flag, flag2);
				}
				bool flag19 = flag4;
				if (flag19)
				{
					this.parHandling.WriteValue(Definition.detectorMonitoringBackgroundIndex, Convert.ToString(Math.Round(list.Average(), 2)));
					this.parHandling.WriteValue(Definition.detectorMonitoringPixelDeltaMax, Convert.ToString(num6));
					this.parHandling.WriteValue(Definition.detectorMonitoringPixelDeltaMaxPosition, Convert.ToString(num7));
					this.parHandling.WriteValue(Definition.detectorMonitoringBadPixel, flag ? "0" : "1");
					this.parHandling.WriteValue(Definition.detectorMonitoringWrongLength, flag2 ? "0" : "1");
				}
			}
			bool flag20 = flag && flag2;
			if (flag20)
			{
				bool flag21 = this.offsetMode;
				if (flag21)
				{
					serial = this.parHandling.ReadPar(path2);
					bool flag22 = detectorType != DetectorType.DTNetTDI;
					if (flag22)
					{
						this.dbConnection.AddOffsetPixelsEntry(this.lastMCUTemperature, serial, DateTime.Now.ToString(new CultureInfo("de-DE")), list, flag3, true);
					}
					bool flag23 = detectorType == DetectorType.DTNetTDI && !this.TdiOffsetSaved;
					if (flag23)
					{
						XRayOnProcessing.LogTdiCalib("10 - Saving Offset file");
						image.SaveToFile(string.Concat(new string[]
						{
							Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
							"\\",
							Definition.tdiCalibFolder,
							"\\Offset",
							DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"),
							".xri"
						}));
						XRayOnProcessing.LogTdiCalib("11 - Save offset values in detector");
						this.TdiSendOffsetImage(list);
					}
					this.OnOffsetValuesSaved(this.lastMCUTemperature);
				}
				else
				{
					string text2 = null;
					string text3 = null;
					double num8 = 0.0;
					double num9 = 0.0;
					int scanrate = 0;
					bool flag24 = detectorType == DetectorType.DTNetTDI;
					if (flag24)
					{
						XRayOnProcessing.LogTdiCalib("Defect NoTDI pixel list:");
						foreach (DTNetNoTdiDefectPixel dtnetNoTdiDefectPixel2 in MeanPixelLogger.noTdiDefectPixels)
						{
							XRayOnProcessing.LogTdiCalib("Defect pixel:  " + dtnetNoTdiDefectPixel2.ToStringAsLine());
						}
						XRayOnProcessing.LogTdiCalib("Defect TDI pixel list:");
						foreach (DTNetTdiDefectPixel dtnetTdiDefectPixel in MeanPixelLogger.tdiDefectPixelList.TdiDefectPixelList)
						{
							XRayOnProcessing.LogTdiCalib("Defect pixel:  " + dtnetTdiDefectPixel.ToStringAsLine());
						}
					}
					this.dbConnection.GetParameterValue("/hardware/xraysource/VoltageSetpoint/Value", ref text2);
					this.dbConnection.GetParameterValue("/hardware/xraysource/CurrentSetpoint/Value", ref text3);
					double.TryParse(text2, NumberStyles.Float, new CultureInfo("en-US"), out num8);
					double.TryParse(text3, NumberStyles.Float, new CultureInfo("en-US"), out num9);
					int.TryParse(text, out scanrate);
					bool flag25 = this.CheckGainCalibration(image, this.PixelGainsLine, num8, num9, scanrate, false);
					bool flag26 = (flag25 && this.saveValue) || flag3;
					if (flag26)
					{
						bool flag27 = detectorType != DetectorType.DTNetTDI;
						if (flag27)
						{
							this.dbConnection.AddGainPixelsEntry(this.lastMCUTemperature, text2, text3, text, serial, DateTime.Now.ToString(new CultureInfo("de-DE")), this.PixelGainsLine, flag3, true);
						}
					}
					bool flag28 = detectorType == DetectorType.DTNetTDI && !this.TdiGainSaved;
					if (flag28)
					{
						XRayOnProcessing.LogTdiCalib("16 - Saving Gain file");
						image.SaveToFile(string.Concat(new string[]
						{
							Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
							"\\",
							Definition.tdiCalibFolder,
							"\\Gain",
							DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"),
							".xri"
						}));
						XRayOnProcessing.LogTdiCalib("17 - Save gain values in detector");
						this.TdiSendGainImage(this.PixelGainsLine);
					}
					this.OnGainValuesSaved(new GainCalibrationInfo(this.lastMCUTemperature, scanrate, num8, num9, !flag25));
					bool flag29 = this.SignalMonitoringSave && !this.offsetMode;
					if (flag29)
					{
						this.SignalMonitoringSave = false;
						Thread thread = new Thread(delegate()
						{
							this.CheckSignalsMonitoring(signalDectector);
						});
						thread.Start();
					}
				}
			}
		}

		// Token: 0x06001CBD RID: 7357 RVA: 0x000B8604 File Offset: 0x000B6804
		protected virtual void OnOffsetValuesSaved(int temperature)
		{
			Action<int> offsetValuesSaved = this.OffsetValuesSaved;
			bool flag = offsetValuesSaved != null;
			if (flag)
			{
				offsetValuesSaved(temperature);
			}
		}

		// Token: 0x06001CBE RID: 7358 RVA: 0x000B862C File Offset: 0x000B682C
		protected virtual void OnGainValuesSaved(GainCalibrationInfo info)
		{
			Action<GainCalibrationInfo> gainValuesSaved = this.GainValuesSaved;
			bool flag = gainValuesSaved != null;
			if (flag)
			{
				gainValuesSaved(info);
			}
		}

		// Token: 0x06001CBF RID: 7359 RVA: 0x000B8654 File Offset: 0x000B6854
		public bool CheckGainCalibration(XRSImage image, List<double> gainValues, double voltage, double current, int scanrate, bool isLiveCalib)
		{
			DetectorType detectorType = DetectorType.DT;
			bool result = true;
			int num = 0;
			int num2 = 0;
			int num3 = (int)((image.EnergyLevels == 1) ? image.Width : (image.Width / (short)image.EnergyLevels));
			string pixelSizePath = "";
			int num4 = 32767;
			int num5 = 0;
			List<int> list = new List<int>();
			this.dbConnection.GetDetectorType(ref detectorType);
			bool flag = detectorType == DetectorType.Sesotec;
			if (flag)
			{
				pixelSizePath = Conveyor.detectorSesotecPixelSizePath;
			}
			bool flag2 = detectorType == DetectorType.DTNetDE;
			if (flag2)
			{
				pixelSizePath = Conveyor.detectorDTPixelSizePath;
			}
			bool flag3 = detectorType == DetectorType.DTNetTDI;
			if (flag3)
			{
				pixelSizePath = Conveyor.detectorDTPixelSizePath;
				list = this.generateNoTdiIgnorePixelList((int)image.Width);
				foreach (DTNetNoTdiDefectPixel dtnetNoTdiDefectPixel in MeanPixelLogger.noTdiDefectPixels)
				{
					bool flag4 = !list.Contains(dtnetNoTdiDefectPixel.NoTdiPixel);
					if (flag4)
					{
						list.Add(dtnetNoTdiDefectPixel.NoTdiPixel);
					}
				}
			}
			this.CorrectMinAndMaxBgValue(voltage, current, scanrate, ref num, ref num2, pixelSizePath);
			for (int i = 0; i < (int)image.Height; i++)
			{
				for (int j = 0; j < num3; j++)
				{
					bool flag5 = this.IsPixelToCheckGain(j, (int)image.Width, detectorType, list);
					if (flag5)
					{
						int num6 = (int)image.Data[i * (int)image.Width + j];
						int num7 = (int)((double)num6 * gainValues[j]);
						bool flag6 = num7 < num4;
						if (flag6)
						{
							num4 = num7;
						}
						bool flag7 = num7 > num5;
						if (flag7)
						{
							num5 = num7;
						}
						bool flag8 = num7 <= num || num7 > num2;
						if (flag8)
						{
							result = false;
						}
					}
				}
			}
			this.parHandling.WriteValue(Definition.minBackgroundValueImagePath, num4.ToString(), false);
			this.parHandling.WriteValue(Definition.maxBackgroundValueImagePath, num5.ToString(), false);
			return result;
		}

		// Token: 0x06001CC0 RID: 7360 RVA: 0x000B8864 File Offset: 0x000B6A64
		private void CorrectMinAndMaxBgValue(double voltage, double current, int scanrate, ref int minValue, ref int maxValue, string pixelSizePath)
		{
			int num = 0;
			int num2 = 0;
			this.dbConnection.GetMinBackgroundValue(ref num);
			this.dbConnection.GetMaxBackgroundValue(ref num2);
			minValue = num;
			maxValue = num2;
			bool flag = this.parHandling.ReadPar(pixelSizePath) == "0.4";
			if (flag)
			{
				bool flag2 = voltage < 60.0;
				if (flag2)
				{
					minValue += (int)(voltage - 60.0) * 20;
					maxValue += (int)(60.0 - voltage) * 80;
				}
			}
			else
			{
				bool flag3 = voltage < 50.0;
				if (flag3)
				{
					minValue += (int)(voltage - 50.0) * 20;
					maxValue += (int)(50.0 - voltage) * 40;
				}
			}
			minValue += (int)((current * 1000.0 - 2000.0) * 0.25);
			maxValue += (int)((2000.0 - current * 1000.0) * 0.4);
			minValue += (int)((double)(1000 - scanrate) * 0.2);
			maxValue += (int)((double)(scanrate - 1000) * 0.2);
			bool flag4 = minValue < 0;
			if (flag4)
			{
				minValue = 0;
			}
			this.parHandling.WriteValue(Definition.minBackgroundValueRealPath, minValue.ToString(), false);
			this.parHandling.WriteValue(Definition.maxBackgroundValueRealPath, maxValue.ToString(), false);
		}

		// Token: 0x06001CC1 RID: 7361 RVA: 0x000B8A00 File Offset: 0x000B6C00
		private bool IsPixelToCheckGain(int index, int nPixel, DetectorType detectorType, List<int> noTdiIgnorePixelList)
		{
			bool result = false;
			bool flag = detectorType == DetectorType.DTNetDE;
			if (flag)
			{
				bool flag2 = index >= this.oldIgnoredLeftPixel && index < nPixel - this.oldIgnoredRightPixel;
				if (flag2)
				{
					result = (index < nPixel / 2 - this.oldIgnoredRightPixel || index >= nPixel / 2 + this.oldIgnoredLeftPixel);
				}
			}
			else
			{
				bool flag3 = detectorType == DetectorType.DTNetTDI;
				if (flag3)
				{
					bool flag4 = !noTdiIgnorePixelList.Contains(index);
					if (flag4)
					{
						result = true;
					}
				}
				else
				{
					bool flag5 = index >= this.oldIgnoredLeftPixel && index < nPixel - this.oldIgnoredRightPixel;
					if (flag5)
					{
						result = true;
					}
				}
			}
			return result;
		}

		// Token: 0x06001CC2 RID: 7362 RVA: 0x000B8AA0 File Offset: 0x000B6CA0
		private List<int> generateNoTdiIgnorePixelList(int nPixelNoTDI)
		{
			int num = 128;
			int num2 = 1024;
			int num3 = 8;
			List<int> list = new List<int>();
			for (int i = 1; i <= this.oldIgnoredLeftPixel; i++)
			{
				int num4 = (i - 1) / num;
				for (int j = 0; j < num3; j++)
				{
					list.Add(num4 * num2 + (i - 1) % num + j * num);
				}
			}
			for (int k = 1; k <= this.oldIgnoredRightPixel; k++)
			{
				int num5 = (nPixelNoTDI / num3 - k) / num;
				for (int l = 0; l < num3; l++)
				{
					list.Add(num5 * num2 + (nPixelNoTDI / num3 - k) % num + l * num);
				}
			}
			return list;
		}

		// Token: 0x06001CC3 RID: 7363 RVA: 0x000B8B78 File Offset: 0x000B6D78
		private void saveBackgroundindex(double backgroundind, string scanrate, int deltaMax, int pixeldeltaMax, bool containsnoZeros, bool nowrongLength)
		{
			string containszeros = containsnoZeros ? "0" : "1";
			string wronglength = nowrongLength ? "0" : "1";
			string sourcevolt = "";
			string sourcecurrent = "";
			string bgindex = Convert.ToString(Math.Round(backgroundind, 2));
			this.dbConnection.GetParameterValue("/hardware/xraysource/VoltageSetpoint/Value", ref sourcevolt);
			this.dbConnection.GetParameterValue("/hardware/xraysource/CurrentSetpoint/Value", ref sourcecurrent);
			this.dBDataLogs.BackgroundIndexAddEntry(sourcevolt, sourcecurrent, scanrate, bgindex, Convert.ToString(pixeldeltaMax), Convert.ToString(deltaMax), containszeros, wronglength);
		}

		// Token: 0x06001CC4 RID: 7364 RVA: 0x000B8C08 File Offset: 0x000B6E08
		private void TdiSendOffsetImage(List<double> meanValuesLine)
		{
			DetectorType detectorType = DetectorType.DT;
			this.dbConnection.GetDetectorType(ref detectorType);
			XRSImage xrsimage = new XRSImage();
			xrsimage.Version = XRSImage.imageVersion;
			xrsimage.Type = ImageType.TdiOffsetValues;
			int num = 10;
			xrsimage.Height = (short)num;
			xrsimage.Width = (short)(meanValuesLine.Count / num * 2);
			xrsimage.Data = new short[meanValuesLine.Count * 2];
			for (int i = 0; i < meanValuesLine.Count; i++)
			{
				xrsimage.Data[i * 2] = (short)meanValuesLine[i];
				xrsimage.Data[i * 2 + 1] = 0;
			}
			this.imageTransfer.SendImage(xrsimage);
			Thread.Sleep(8000);
			bool flag = detectorType == DetectorType.DTNetTDI;
			if (flag)
			{
				XRayOnProcessing.LogTdiCalib("12 - Enable DT offset correction");
			}
			this.parHandling.WriteValue(XRayOnProcessing.dtOffsetEnablePath, "1", false);
			this.TdiOffsetSaved = true;
		}

		// Token: 0x06001CC5 RID: 7365 RVA: 0x000B8CF8 File Offset: 0x000B6EF8
		private void TdiSendGainImage(List<double> PixelGainsLine)
		{
			XRSImage xrsimage = new XRSImage();
			xrsimage.Version = XRSImage.imageVersion;
			xrsimage.Type = ImageType.TdiGainValues;
			int num = 10;
			xrsimage.Height = (short)num;
			xrsimage.Width = (short)(PixelGainsLine.Count / num * 2);
			xrsimage.Data = new short[PixelGainsLine.Count * 2];
			for (int i = 0; i < PixelGainsLine.Count; i++)
			{
				bool calibrateMaxPossibleNoTdi = XRayOnProcessing.CalibrateMaxPossibleNoTdi;
				float value;
				if (calibrateMaxPossibleNoTdi)
				{
					value = (float)(PixelGainsLine[i] * XRayOnProcessing.TdiGainFactor);
				}
				else
				{
					value = (float)PixelGainsLine[i];
				}
				byte[] bytes = BitConverter.GetBytes(value);
				uint num2 = (uint)((uint)bytes[3] << 24);
				num2 += (uint)((uint)bytes[2] << 16);
				num2 += (uint)((uint)bytes[1] << 8);
				num2 += (uint)bytes[0];
				xrsimage.Data[i * 2 + 1] = (short)(num2 >> 16 & 65535U);
				xrsimage.Data[i * 2] = (short)(num2 & 65535U);
			}
			this.imageTransfer.SendImage(xrsimage);
			Thread.Sleep(5000);
			this.TdiGainSaved = true;
		}

		// Token: 0x06001CC6 RID: 7366 RVA: 0x000B8E18 File Offset: 0x000B7018
		public void CheckSignalsMonitoring(List<double> signals)
		{
			string text = null;
			string text2 = null;
			DetectorType detectorType = DetectorType.DT;
			this.dbConnection.GetDetectorType(ref detectorType);
			string path = (detectorType == DetectorType.DT || detectorType == DetectorType.DTNetSE || detectorType == DetectorType.DTNetDE || detectorType == DetectorType.DTNetTDI) ? Definition.detectorDTSerialPath : Definition.detectorSesotecMACPath;
			string path2 = (detectorType == DetectorType.DT || detectorType == DetectorType.DTNetSE || detectorType == DetectorType.DTNetDE || detectorType == DetectorType.DTNetTDI) ? Definition.detectorDTScanratePath : Definition.detectorSesotecScanratePath;
			string path3 = (detectorType == DetectorType.DT || detectorType == DetectorType.DTNetSE || detectorType == DetectorType.DTNetDE || detectorType == DetectorType.DTNetTDI) ? Definition.dtMCUTemperaturePath : Definition.detectorSesotecTemperaturePath;
			string text3 = this.parHandling.ReadPar(path2);
			string text4 = this.parHandling.ReadPar(path);
			string item = this.parHandling.ReadPar(path3);
			this.dbConnection.GetParameterValue("/hardware/xraysource/VoltageSetpoint/Value", ref text);
			this.dbConnection.GetParameterValue("/hardware/xraysource/CurrentSetpoint/Value", ref text2);
			string text5 = text2.Replace(".", "");
			bool flag = text5.Length > 3;
			if (flag)
			{
				text5 = text5.Substring(0, 3);
			}
			bool flag2 = text5.Length < 3;
			if (flag2)
			{
				text5 = text5.PadRight(3, '0');
			}
			bool flag3 = text4 != "";
			if (flag3)
			{
				string[] tableNames = this.dBDataLogs.GetTableNames();
				bool flag4 = false;
				string text6 = string.Concat(new string[]
				{
					"DetectorSignal_",
					text,
					"_",
					text5,
					"_",
					text3,
					"_",
					text4.Replace(":", "")
				});
				foreach (string text7 in tableNames)
				{
					bool flag5 = text7.Equals(text6);
					if (flag5)
					{
						flag4 = true;
						break;
					}
				}
				string text8 = "SignalPredMaint2_" + text4.Replace(":", "");
				bool flag6 = this.dBDataLogs.SignalPredMaintTablesExist(text8);
				if (flag6)
				{
					string s = "";
					this.dBDataLogs.GetLastDatePredMaint(text8, ref s);
					DateTime d;
					bool flag7 = DateTime.TryParseExact(s, "dd.MM.yyyy", new CultureInfo("de-DE"), DateTimeStyles.None, out d);
					if (flag7)
					{
						bool flag8 = (DateTime.Now - d).Days >= 7;
						if (flag8)
						{
							this.dBDataLogs.UpdateSignalTableForPredMaint2(text8, DateTime.Now.ToString("d", new CultureInfo("de-DE")), text, text2, text3, signals, signals.Count<double>());
						}
					}
				}
				else
				{
					this.dBDataLogs.CreateDefaultSignalsTablePredMain(text8, signals, new List<string>
					{
						DateTime.Now.ToString("d", new CultureInfo("de-DE")),
						text,
						text2,
						text3
					});
				}
				bool flag9 = !flag4;
				if (flag9)
				{
					List<double> statisticsValue = new List<double>();
					List<string> statisticsName = new List<string>();
					List<string> list = new List<string>();
					List<string> list2 = new List<string>();
					list.Add("Date");
					list.Add("Scanrate");
					list.Add("Current");
					list.Add("Voltage");
					list.Add("Temperature");
					list2.Add(DateTime.Now.ToString());
					list2.Add(text3);
					list2.Add(text2);
					list2.Add(text);
					list2.Add(item);
					string s2 = this.parHandling.ReadPar(ParameterHandling.ignoreLeftPixelPath);
					int removeLeft = 0;
					int removeRight = 0;
					int.TryParse(s2, out removeLeft);
					s2 = this.parHandling.ReadPar(ParameterHandling.ignoreRightPixelPath);
					int.TryParse(s2, out removeRight);
					this.dBDataLogs.GetSignalsStatistics(signals, ref statisticsValue, ref statisticsName, removeLeft, removeRight);
					this.dBDataLogs.CreateDefaultSignalsTable(text6, signals, statisticsName, statisticsValue, list, list2);
				}
				else
				{
					bool flag10 = false;
					bool flag11 = false;
					bool flag12 = false;
					bool flag13 = false;
					bool flag14 = false;
					bool flag15 = false;
					bool flag16 = false;
					bool flag17 = false;
					double num = 0.0;
					double num2 = 0.0;
					string text9 = "";
					this.dBDataLogs.GetLimitsEntry("SignalDifference", ref text9);
					double.TryParse(text9.Replace(',', '.'), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out num);
					this.dBDataLogs.GetLimitsEntry("SignalFluctuation", ref text9);
					double.TryParse(text9.Replace(',', '.'), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out num2);
					string text10 = "";
					this.dBDataLogs.GetDataLogsSettingsEntry("MessageLevel", ref text10);
					bool flag18 = text10.Equals("Info");
					SSTEntryCode code;
					if (flag18)
					{
						code = SSTEntryCode.LimitsInfo;
					}
					else
					{
						bool flag19 = text10.Equals("Warning");
						if (flag19)
						{
							code = SSTEntryCode.LimitsWarning;
						}
						else
						{
							code = SSTEntryCode.LimitsError;
						}
					}
					text10 = "";
					this.dBDataLogs.GetDataLogsSettingsEntry("MessageLevel90Perc", ref text10);
					bool flag20 = text10.Equals("Info");
					SSTEntryCode code2;
					if (flag20)
					{
						code2 = SSTEntryCode.Limits90PerInfo;
					}
					else
					{
						code2 = SSTEntryCode.Limits90PerWarning;
					}
					string tablename = text6;
					List<double> list3 = new List<double>();
					List<string> list4 = new List<string>();
					string s3 = this.parHandling.ReadPar(ParameterHandling.ignoreLeftPixelPath);
					int removeLeft2 = 0;
					int removeRight2 = 0;
					int.TryParse(s3, out removeLeft2);
					s3 = this.parHandling.ReadPar(ParameterHandling.ignoreRightPixelPath);
					int.TryParse(s3, out removeRight2);
					bool flag21 = detectorType == DetectorType.DTNetTDI;
					if (flag21)
					{
						removeLeft2 = 0;
						removeRight2 = 0;
					}
					this.dBDataLogs.GetSignalsStatistics(signals, ref list3, ref list4, removeLeft2, removeRight2);
					int num3 = 0;
					int num4 = 0;
					double num5 = -100.0;
					string moduleName = "";
					string maximumModuleName = "";
					double num6 = -100.0;
					double num7 = -100.0;
					string maximumDetectorName = "";
					double num8 = 0.0;
					double num9 = 0.0;
					double num10 = 0.0;
					double num11 = 0.0;
					int index = list4.IndexOf("MeanValue");
					double num12 = list3[index];
					this.dBDataLogs.GetReferenceSignalEntry(tablename, list4[index], ref num9);
					double num13 = (num9 - list3[index]) / num9 * 100.0;
					index = list4.IndexOf("Max");
					double num14 = list3[index];
					this.dBDataLogs.GetReferenceSignalEntry(tablename, list4[index], ref num11);
					double num15 = (num11 - list3[index]) / num11 * 100.0;
					bool flag22 = num15 > num6;
					if (flag22)
					{
						num6 = num15;
						maximumDetectorName = list4[index];
					}
					index = list4.IndexOf("Min");
					double num16 = list3[index];
					this.dBDataLogs.GetReferenceSignalEntry(tablename, list4[index], ref num10);
					num15 = (num10 - list3[index]) / num10 * 100.0;
					bool flag23 = num15 > num6;
					if (flag23)
					{
						num6 = num15;
						maximumDetectorName = list4[index];
					}
					index = list4.IndexOf("StandardDeviation");
					double num17 = list3[index];
					this.dBDataLogs.GetReferenceSignalEntry(tablename, list4[index], ref num8);
					num15 = (list3[index] - num8) / num8 * 100.0;
					bool flag24 = num15 > num6;
					if (flag24)
					{
						num6 = num15;
						maximumDetectorName = list4[index];
					}
					num15 = (num9 - num10 - (num12 - num16)) / (num9 - num10) * 100.0;
					bool flag25 = num15 > num6;
					if (flag25)
					{
						num6 = num15;
						maximumDetectorName = "Mean-Min";
					}
					num15 = (num11 - num10 - (num14 - num16)) / (num11 - num10) * 100.0;
					bool flag26 = num15 > num6;
					if (flag26)
					{
						num6 = num15;
						maximumDetectorName = "Max-Min";
					}
					num15 = (num11 - num9 - (num14 - num12)) / (num11 - num9) * 100.0;
					bool flag27 = num15 > num6;
					if (flag27)
					{
						num6 = num15;
						maximumDetectorName = "Max-Mean";
					}
					int num18 = list4.Count<string>() / 4;
					for (int j = 1; j < num18; j++)
					{
						index = list4.IndexOf("MeanValue Module " + j.ToString());
						num12 = list3[index];
						this.dBDataLogs.GetReferenceSignalEntry(tablename, list4[index], ref num9);
						double num19 = (num9 - list3[index]) / num9 * 100.0;
						bool flag28 = num19 > num5;
						if (flag28)
						{
							num5 = num19;
							moduleName = list4[index];
							num4 = j;
						}
						index = list4.IndexOf("Max Module " + j.ToString());
						num14 = list3[index];
						this.dBDataLogs.GetReferenceSignalEntry(tablename, list4[index], ref num11);
						num15 = (num11 - list3[index]) / num11 * 100.0;
						bool flag29 = num15 > num7;
						if (flag29)
						{
							num7 = num15;
							maximumModuleName = list4[index];
							num3 = j;
						}
						index = list4.IndexOf("Min Module " + j.ToString());
						num16 = list3[index];
						this.dBDataLogs.GetReferenceSignalEntry(tablename, list4[index], ref num10);
						num15 = (num10 - list3[index]) / num10 * 100.0;
						bool flag30 = num15 > num7;
						if (flag30)
						{
							num7 = num15;
							maximumModuleName = list4[index];
							num3 = j;
						}
						index = list4.IndexOf("StandardDeviation Module " + j.ToString());
						num17 = list3[index];
						this.dBDataLogs.GetReferenceSignalEntry(tablename, list4[index], ref num8);
						num15 = (list3[index] - num8) / num8 * 100.0;
						bool flag31 = num15 > num7;
						if (flag31)
						{
							num7 = num15;
							maximumModuleName = list4[index];
							num3 = j;
						}
						num15 = (num9 - num10 - (num12 - num16)) / (num9 - num10) * 100.0;
						bool flag32 = num15 > num7;
						if (flag32)
						{
							num7 = num15;
							maximumModuleName = "Mean-Min Module" + j.ToString();
							num3 = j;
						}
						num15 = (num11 - num10 - (num14 - num16)) / (num11 - num10) * 100.0;
						bool flag33 = num15 > num7;
						if (flag33)
						{
							num7 = num15;
							maximumModuleName = "Max-Min Module" + j.ToString();
							num3 = j;
						}
						num15 = (num11 - num9 - (num14 - num12)) / (num11 - num9) * 100.0;
						bool flag34 = num15 > num7;
						if (flag34)
						{
							num7 = num15;
							maximumModuleName = "Max-Mean Module" + j.ToString();
							num3 = j;
						}
					}
					bool flag35 = num > 0.0;
					if (flag35)
					{
						bool flag36 = num5 > num;
						if (flag36)
						{
							flag11 = true;
						}
						else
						{
							bool flag37 = num5 >= num * 0.9;
							if (flag37)
							{
								flag13 = true;
							}
						}
						bool flag38 = num13 > num;
						if (flag38)
						{
							flag10 = true;
						}
						else
						{
							bool flag39 = num13 >= num * 0.9;
							if (flag39)
							{
								flag12 = true;
							}
						}
					}
					bool flag40 = num2 > 0.0;
					if (flag40)
					{
						bool flag41 = num7 > num2;
						if (flag41)
						{
							flag15 = true;
						}
						else
						{
							bool flag42 = num7 >= num2 * 0.9;
							if (flag42)
							{
								flag17 = true;
							}
						}
						bool flag43 = num6 > num2;
						if (flag43)
						{
							flag14 = true;
						}
						else
						{
							bool flag44 = num6 >= num2 * 0.9;
							if (flag44)
							{
								flag16 = true;
							}
						}
					}
					text10 = "";
					this.dBDataLogs.GetDataLogsSettingsEntry("MonitoringMessagesOnOff", ref text10);
					bool flag45 = text10.Equals("1");
					if (flag45)
					{
						text9 = "";
						int num20 = 0;
						this.dBDataLogs.GetDataLogsSettingsEntry("TimeSpanRepetitionMessage", ref text9);
						int.TryParse(text9, out num20);
						bool flag46 = (DateTime.Now - this.lastWarning).TotalHours > (double)num20;
						if (flag46)
						{
							bool flag47 = flag10;
							if (flag47)
							{
								this.errorSurvey.PostMessage(code, 0, "txtSignalValue", "", num.ToString(), Math.Round(num13, 2).ToString());
								this.lastWarning = DateTime.Now;
							}
							else
							{
								bool flag48 = flag12;
								if (flag48)
								{
									this.errorSurvey.PostMessage(code2, 0, "txtSignalValue", "", num.ToString(), Math.Round(num13, 2).ToString());
									this.lastWarning = DateTime.Now;
								}
							}
							bool flag49 = flag11;
							if (flag49)
							{
								this.errorSurvey.PostMessage(code, (byte)num4, "txtSignalValueModul", "", num.ToString(), Math.Round(num5, 2).ToString());
								this.lastWarning = DateTime.Now;
							}
							else
							{
								bool flag50 = flag13;
								if (flag50)
								{
									this.errorSurvey.PostMessage(code2, (byte)num4, "txtSignalValueModul", "", num.ToString(), Math.Round(num5, 2).ToString());
									this.lastWarning = DateTime.Now;
								}
							}
							bool flag51 = flag14;
							if (flag51)
							{
								this.errorSurvey.PostMessage(code, 0, "txtSignalFluctuation", "", num2.ToString(), Math.Round(num6, 2).ToString());
								this.lastWarning = DateTime.Now;
							}
							else
							{
								bool flag52 = flag16;
								if (flag52)
								{
									this.errorSurvey.PostMessage(code2, 0, "txtSignalFluctuation", "", num2.ToString(), Math.Round(num6, 2).ToString());
									this.lastWarning = DateTime.Now;
								}
							}
							bool flag53 = flag15;
							if (flag53)
							{
								this.errorSurvey.PostMessage(code, (byte)num3, "txtSignalFluctuationModule", "", num2.ToString(), Math.Round(num7, 2).ToString());
								this.lastWarning = DateTime.Now;
							}
							else
							{
								bool flag54 = flag17;
								if (flag54)
								{
									this.errorSurvey.PostMessage(code2, (byte)num3, "txtSignalFluctuationModule", "", num2.ToString(), Math.Round(num7, 2).ToString());
									this.lastWarning = DateTime.Now;
								}
							}
						}
					}
					bool flag55 = flag10 || flag11 || flag14 || flag15;
					if (flag55)
					{
						List<string> list5 = new List<string>();
						List<string> list6 = new List<string>();
						list5.Add("Date");
						list5.Add("Scanrate");
						list5.Add("Current");
						list5.Add("Voltage");
						list5.Add("Temperature");
						list5.Add("PercentageOfMeanOriginalSignal");
						list6.Add(DateTime.Now.ToString());
						list6.Add(text3);
						list6.Add(text2);
						list6.Add(text);
						list6.Add(item);
						list6.Add(Math.Round(num13, 2).ToString());
						this.dBDataLogs.OverwriteTableDetectorSignalError("ErrorDetectorSignal_" + text4.Replace(":", ""), signals, list4, list3, list5, list6);
						this.dBDataLogs.SetLastSignalDetectorQuality("DetectorQuality_" + text4.Replace(":", ""), DateTime.Now.ToString(new CultureInfo("de-DE")), text3, text, text2, Math.Round(num13, 2), moduleName, Math.Round(num5, 2).ToString());
						this.dBDataLogs.SetLastSignalDetectorFluctuation("DetectorFluctuation_" + text4.Replace(":", ""), DateTime.Now.ToString(new CultureInfo("de-DE")), text3, text, text2, maximumDetectorName, Math.Round(num6, 2).ToString(), maximumModuleName, Math.Round(num7, 2).ToString());
					}
					else
					{
						string text11 = "";
						this.dBDataLogs.GetLastDateDetectorQuality("DetectorQuality_" + text4.Replace(":", ""), ref text11);
						bool flag56 = text11 == "";
						if (flag56)
						{
							this.dBDataLogs.SetLastSignalDetectorQuality("DetectorQuality_" + text4.Replace(":", ""), DateTime.Now.ToString(new CultureInfo("de-DE")), text3, text, text2, Math.Round(num13, 2), moduleName, Math.Round(num5, 2).ToString());
							this.dBDataLogs.SetLastSignalDetectorFluctuation("DetectorFluctuation_" + text4.Replace(":", ""), DateTime.Now.ToString(new CultureInfo("de-DE")), text3, text, text2, maximumDetectorName, Math.Round(num6, 2).ToString(), maximumModuleName, Math.Round(num7, 2).ToString());
						}
						else
						{
							DateTime d2;
							bool flag57 = DateTime.TryParseExact(text11, "dd.MM.yyyy HH:mm:ss", new CultureInfo("de-DE"), DateTimeStyles.None, out d2);
							if (flag57)
							{
								bool flag58 = (DateTime.Now - d2).Days >= 7;
								if (flag58)
								{
									this.dBDataLogs.SetLastSignalDetectorQuality("DetectorQuality_" + text4.Replace(":", ""), DateTime.Now.ToString(new CultureInfo("de-DE")), text3, text, text2, Math.Round(num13, 2), moduleName, Math.Round(num5, 2).ToString());
									this.dBDataLogs.SetLastSignalDetectorFluctuation("DetectorFluctuation_" + text4.Replace(":", ""), DateTime.Now.ToString(new CultureInfo("de-DE")), text3, text, text2, maximumDetectorName, Math.Round(num6, 2).ToString(), maximumModuleName, Math.Round(num7, 2).ToString());
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06001CC7 RID: 7367 RVA: 0x000BA12F File Offset: 0x000B832F
		public void InitErrorSurvey(ErrorSurvey errorSurvey)
		{
			this.errorSurvey = errorSurvey;
		}

		// Token: 0x040012E9 RID: 4841
		private const int nDefaultLinesForCalibration = 700;

		// Token: 0x040012EA RID: 4842
		private const int nMaxLinesForCalibration = 1000;

		// Token: 0x040012EB RID: 4843
		private const int nMaxLinesForCalibrationSesotecHS = 2000;

		// Token: 0x040012EC RID: 4844
		private int oldIgnoredLeftPixel;

		// Token: 0x040012ED RID: 4845
		private int oldIgnoredRightPixel;

		// Token: 0x040012EE RID: 4846
		private int oldNMaxLines;

		// Token: 0x040012EF RID: 4847
		private DBConnection dbConnection;

		// Token: 0x040012F0 RID: 4848
		private ParameterHandling parHandling;

		// Token: 0x040012F1 RID: 4849
		private ImageDataTransfer imageTransfer;

		// Token: 0x040012F2 RID: 4850
		private int lastMCUTemperature;

		// Token: 0x040012F3 RID: 4851
		private string currentFileName;

		// Token: 0x040012F4 RID: 4852
		private MeanPixelLogger.LogMode dbLogMode;

		// Token: 0x040012F5 RID: 4853
		private bool offsetMode;

		// Token: 0x040012F6 RID: 4854
		private int calibrationValue;

		// Token: 0x040012F7 RID: 4855
		private bool saveValue;

		// Token: 0x040012F8 RID: 4856
		private DBDataLogs dBDataLogs;

		// Token: 0x040012FD RID: 4861
		public bool TdiOffsetSaved = false;

		// Token: 0x040012FE RID: 4862
		public bool TdiGainSaved = false;

		// Token: 0x040012FF RID: 4863
		private ErrorSurvey errorSurvey;

		// Token: 0x04001300 RID: 4864
		private DateTime lastWarning = default(DateTime);

		// Token: 0x04001301 RID: 4865
		private bool ignoreFirstImage;

		// Token: 0x04001302 RID: 4866
		public bool SignalMonitoringSave;

		// Token: 0x04001303 RID: 4867
		public static List<int> nTdiDefectPixels = new List<int>();

		// Token: 0x04001304 RID: 4868
		public static List<DTNetNoTdiDefectPixel> noTdiDefectPixels = new List<DTNetNoTdiDefectPixel>();

		// Token: 0x04001305 RID: 4869
		public static DTNetTdiDefectPixelList tdiDefectPixelList = new DTNetTdiDefectPixelList();

		// Token: 0x02000305 RID: 773
		public enum LogMode
		{
			// Token: 0x040020A3 RID: 8355
			Database,
			// Token: 0x040020A4 RID: 8356
			CSV
		}
	}
}
