using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using DatabaseConnection;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x02000099 RID: 153
	public class LiveCalibration
	{
		// Token: 0x170004BB RID: 1211
		// (get) Token: 0x06000F90 RID: 3984 RVA: 0x0005CC70 File Offset: 0x0005AE70
		// (set) Token: 0x06000F91 RID: 3985 RVA: 0x0005CC78 File Offset: 0x0005AE78
		private List<double> GainList { get; set; }

		// Token: 0x06000F92 RID: 3986 RVA: 0x0005CC84 File Offset: 0x0005AE84
		public LiveCalibration(DBConnection dbConnection, ParameterHandling parHandling, DBDataLogs dataLogs)
		{
			this.dbConnection = dbConnection;
			this.parHandling = parHandling;
			this.dataLogs = dataLogs;
			this.GainList = new List<double>();
			this.checkTempDetector();
			this.timerForCheckingCalibration = new DispatcherTimer(new TimeSpan(0, 2, 0), DispatcherPriority.Normal, new EventHandler(this.CheckIfCalibIsNecessary), Application.Current.Dispatcher);
		}

		// Token: 0x06000F93 RID: 3987 RVA: 0x0005CCEC File Offset: 0x0005AEEC
		public void StartTimer()
		{
			bool flag = this.timerForCheckingCalibration != null;
			if (flag)
			{
				this.parHandling.WriteValue("/filter/acquisition/LiveCalibrationResetImage/Value", "1", false);
				this.timerForCheckingCalibration.Start();
			}
		}

		// Token: 0x06000F94 RID: 3988 RVA: 0x0005CD2C File Offset: 0x0005AF2C
		public void StopTimer()
		{
			bool flag = this.timerForCheckingCalibration != null;
			if (flag)
			{
				this.timerForCheckingCalibration.Stop();
			}
		}

		// Token: 0x06000F95 RID: 3989 RVA: 0x0005CD58 File Offset: 0x0005AF58
		private void CheckIfCalibIsNecessary(object dummy, EventArgs e)
		{
			bool flag = this.parHandling != null;
			if (flag)
			{
				bool flag2 = this.checkTempDetector();
				string a = this.parHandling.ReadPar(Definition.isLiveCalibrationActivePath);
				bool flag3 = a == "1" || a == "2";
				bool flag4 = flag3 && this.parHandling.ReadPar(Definition.machineStatePath) == "Inspecting";
				if (flag4)
				{
					this.parHandling.WriteValue("/filter/acquisition/LiveCalibrationCalcGains/Value", "1", false);
					this.StartLiveCalibration();
				}
			}
		}

		// Token: 0x06000F96 RID: 3990 RVA: 0x0005CDF0 File Offset: 0x0005AFF0
		public void StartLiveCalibration()
		{
			bool flag = !this.updateThreadRunning;
			if (flag)
			{
				this.updateThreadRunning = true;
				this.thLiveCalibration = new Thread(new ThreadStart(this.ExecuteCalibration));
				this.thLiveCalibration.Name = "LiveCalibration";
				this.thLiveCalibration.Start();
			}
		}

		// Token: 0x06000F97 RID: 3991 RVA: 0x0005CE48 File Offset: 0x0005B048
		public void StopLiveCalibration()
		{
			this.updateThreadRunning = false;
		}

		// Token: 0x06000F98 RID: 3992 RVA: 0x0005CE54 File Offset: 0x0005B054
		public void ExecuteCalibration()
		{
			while (this.updateThreadRunning)
			{
				bool flag = this.parHandling.ReadPar(Definition.LiveCalibrationCalcGains) == "1";
				bool saveGainsInDB = this.parHandling.ReadPar(Definition.LiveCalibrationSendImg) == "1";
				bool flag2 = !flag;
				if (flag2)
				{
					this.GainList.Clear();
					double item = 0.0;
					string strVoltageSetpoint = null;
					string strCurrentSetpoint = null;
					DetectorType detectorType = DetectorType.DT;
					this.dbConnection.GetDetectorType(ref detectorType);
					string path = (detectorType == DetectorType.DT || detectorType == DetectorType.DTNetSE || detectorType == DetectorType.DTNetDE || detectorType == DetectorType.DTNetTDI) ? Definition.detectorDTSerialPath : Definition.detectorSesotecMACPath;
					string path2 = (detectorType == DetectorType.DT || detectorType == DetectorType.DTNetSE || detectorType == DetectorType.DTNetDE || detectorType == DetectorType.DTNetTDI) ? Definition.detectorDTScanratePath : Definition.detectorSesotecScanratePath;
					string scanrate = this.parHandling.ReadPar(path2);
					string serial = this.parHandling.ReadPar(path);
					this.dbConnection.GetParameterValue("/hardware/xraysource/VoltageSetpoint/Value", ref strVoltageSetpoint);
					this.dbConnection.GetParameterValue("/hardware/xraysource/CurrentSetpoint/Value", ref strCurrentSetpoint);
					string path3 = (detectorType == DetectorType.DT || detectorType == DetectorType.DTNetSE || detectorType == DetectorType.DTNetDE || detectorType == DetectorType.DTNetTDI) ? Definition.detectorDTNPixelPath : Definition.detectorSesotecNPixelPath;
					int num = Convert.ToInt32(this.parHandling.ReadPar(path3));
					bool flag3 = detectorType == DetectorType.DTNetTDI && num != 1280;
					if (flag3)
					{
						this.updateThreadRunning = false;
					}
					else
					{
						for (int i = 0; i < num; i++)
						{
							this.parHandling.WriteValue("/filter/acquisition/GainCalibrationIndex/Value", i.ToString());
							string s = this.parHandling.ReadPar("/filter/acquisition/GainCalibrationValue/Value");
							double.TryParse(s, NumberStyles.Float, new CultureInfo("en-US"), out item);
							this.GainList.Add(item);
						}
						this.dbConnection.AddGainPixelsEntry(this.detectorTempActual, strVoltageSetpoint, strCurrentSetpoint, scanrate, serial, DateTime.Now.ToString(new CultureInfo("de-DE")), this.GainList, true, saveGainsInDB);
						this.monitoringOfCalibrationParams();
						this.parHandling.WriteValue(Definition.temperatureInUsePath, this.detectorTempActual.ToString());
						this.updateThreadRunning = false;
					}
				}
			}
		}

		// Token: 0x06000F99 RID: 3993 RVA: 0x0005D0C0 File Offset: 0x0005B2C0
		private bool checkTempDetector()
		{
			DetectorType detectorType = DetectorType.DT;
			this.dbConnection.GetDetectorType(ref detectorType);
			string path = (detectorType == DetectorType.DT || detectorType == DetectorType.DTNetSE || detectorType == DetectorType.DTNetDE || detectorType == DetectorType.DTNetTDI) ? Definition.dtMCUTemperaturePath : Definition.detectorSesotecTemperaturePath;
			string s = this.parHandling.ReadPar(path);
			bool flag = int.TryParse(s, out this.detectorTempActual);
			if (flag)
			{
				bool flag2 = this.detectorTempActual != this.detectorTempOld;
				if (flag2)
				{
					this.detectorTempOld = this.detectorTempActual;
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000F9A RID: 3994 RVA: 0x0005D150 File Offset: 0x0005B350
		private void monitoringOfCalibrationParams()
		{
			bool flag = this.parHandling.ReadPar(Definition.detectorMonitoringActive) == "1";
			if (flag)
			{
				string text = "";
				string text2 = "";
				int num = 0;
				int num2 = 0;
				int num3 = 0;
				int.TryParse(this.parHandling.ReadPar(Definition.detectorMonitoringBackgroundIndex), out num3);
				int num4 = 0;
				int.TryParse(this.parHandling.ReadPar(Definition.detectorMonitoringPixelDeltaMax), out num4);
				int num5 = 0;
				int.TryParse(this.parHandling.ReadPar(Definition.detectorMonitoringPixelDeltaMaxPosition), out num5);
				bool flag2 = this.parHandling.ReadPar(Definition.detectorMonitoringBadPixel) == "1";
				bool flag3 = this.parHandling.ReadPar(Definition.detectorMonitoringWrongLength) == "1";
				this.dataLogs.GetLimitsEntry("BackgroundIndex", ref text);
				int.TryParse(text, out num);
				this.dataLogs.GetLimitsEntry("PixelDeltaMax", ref text2);
				int.TryParse(text2, out num2);
				string text3 = "";
				this.dataLogs.GetDataLogsSettingsEntry("MessageLevel", ref text3);
				bool flag4 = text3.Equals("Info");
				SSTEntryCode code;
				if (flag4)
				{
					code = SSTEntryCode.BackgroundMonitoringInfo;
				}
				else
				{
					bool flag5 = text3.Equals("Warning");
					if (flag5)
					{
						code = SSTEntryCode.BackgroundMonitoringWarning;
					}
					else
					{
						code = SSTEntryCode.BackgroundMonitoringError;
					}
				}
				bool flag6 = this.errorSurvey != null;
				if (flag6)
				{
					bool flag7 = text != "" && text != "-" && num3 > num;
					if (flag7)
					{
						this.errorSurvey.PostMessage(code, 0, "txtBackgroundindex", "", num.ToString(), num3.ToString());
					}
					bool flag8 = text2 != "" && text2 != "-" && num4 > num2;
					if (flag8)
					{
						this.errorSurvey.PostMessage(code, 0, "txtPixelDeltaMax", "", num2.ToString(), num4.ToString());
					}
					bool flag9 = flag2;
					if (flag9)
					{
						this.errorSurvey.PostMessage(code, 1, "txtBadPixel", "", "", "");
					}
					bool flag10 = flag3;
					if (flag10)
					{
						this.errorSurvey.PostMessage(code, 1, "txtDefectiveModule", "", "", "");
					}
				}
			}
		}

		// Token: 0x06000F9B RID: 3995 RVA: 0x0005D3C1 File Offset: 0x0005B5C1
		public void InitErrorSurvey(ErrorSurvey errorSurvey)
		{
			this.errorSurvey = errorSurvey;
		}

		// Token: 0x04000A64 RID: 2660
		private DBConnection dbConnection;

		// Token: 0x04000A65 RID: 2661
		private ParameterHandling parHandling;

		// Token: 0x04000A66 RID: 2662
		private DBDataLogs dataLogs;

		// Token: 0x04000A68 RID: 2664
		private ErrorSurvey errorSurvey;

		// Token: 0x04000A69 RID: 2665
		private int detectorTempActual;

		// Token: 0x04000A6A RID: 2666
		private int detectorTempOld;

		// Token: 0x04000A6B RID: 2667
		private DispatcherTimer timerForCheckingCalibration;

		// Token: 0x04000A6C RID: 2668
		private Thread thLiveCalibration;

		// Token: 0x04000A6D RID: 2669
		private bool updateThreadRunning;

		// Token: 0x04000A6E RID: 2670
		private const string offsetValuePath = "/filter/acquisition/OffsetCalibrationValue/Value";

		// Token: 0x04000A6F RID: 2671
		private const string offsetIndexPath = "/filter/acquisition/OffsetCalibrationIndex/Value";
	}
}
