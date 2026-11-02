using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using DatabaseConnection;
using Definitions;
using SSTEthernet;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x02000114 RID: 276
	public class ErrorSurvey
	{
		// Token: 0x170007DC RID: 2012
		// (get) Token: 0x06001A37 RID: 6711 RVA: 0x00098B42 File Offset: 0x00096D42
		// (set) Token: 0x06001A38 RID: 6712 RVA: 0x00098B4A File Offset: 0x00096D4A
		public bool ParameterPortOk { get; set; }

		// Token: 0x170007DD RID: 2013
		// (get) Token: 0x06001A39 RID: 6713 RVA: 0x00098B53 File Offset: 0x00096D53
		// (set) Token: 0x06001A3A RID: 6714 RVA: 0x00098B5B File Offset: 0x00096D5B
		public bool ImagePortOk { get; set; }

		// Token: 0x170007DE RID: 2014
		// (get) Token: 0x06001A3B RID: 6715 RVA: 0x00098B64 File Offset: 0x00096D64
		// (set) Token: 0x06001A3C RID: 6716 RVA: 0x00098B6C File Offset: 0x00096D6C
		public bool AIModelPortOk { get; set; }

		// Token: 0x170007DF RID: 2015
		// (get) Token: 0x06001A3D RID: 6717 RVA: 0x00098B75 File Offset: 0x00096D75
		// (set) Token: 0x06001A3E RID: 6718 RVA: 0x00098B7D File Offset: 0x00096D7D
		public bool RequestReset { get; set; }

		// Token: 0x170007E0 RID: 2016
		// (get) Token: 0x06001A3F RID: 6719 RVA: 0x00098B86 File Offset: 0x00096D86
		// (set) Token: 0x06001A40 RID: 6720 RVA: 0x00098B8D File Offset: 0x00096D8D
		public static bool CommDetector { get; set; }

		// Token: 0x170007E1 RID: 2017
		// (get) Token: 0x06001A41 RID: 6721 RVA: 0x00098B95 File Offset: 0x00096D95
		// (set) Token: 0x06001A42 RID: 6722 RVA: 0x00098B9C File Offset: 0x00096D9C
		public static bool CommXRaySource { get; set; }

		// Token: 0x170007E2 RID: 2018
		// (get) Token: 0x06001A43 RID: 6723 RVA: 0x00098BA4 File Offset: 0x00096DA4
		// (set) Token: 0x06001A44 RID: 6724 RVA: 0x00098BAB File Offset: 0x00096DAB
		public static bool CheckArcsIsActive { get; set; }

		// Token: 0x170007E3 RID: 2019
		// (get) Token: 0x06001A45 RID: 6725 RVA: 0x00098BB4 File Offset: 0x00096DB4
		// (set) Token: 0x06001A46 RID: 6726 RVA: 0x00098BCC File Offset: 0x00096DCC
		public AuditRequired AuditRequired
		{
			get
			{
				return this.auditRequired;
			}
			set
			{
				this.auditRequired = value;
			}
		}

		// Token: 0x14000086 RID: 134
		// (add) Token: 0x06001A47 RID: 6727 RVA: 0x00098BD8 File Offset: 0x00096DD8
		// (remove) Token: 0x06001A48 RID: 6728 RVA: 0x00098C10 File Offset: 0x00096E10
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action ImageProcessingNetworkError;

		// Token: 0x170007E4 RID: 2020
		// (get) Token: 0x06001A49 RID: 6729 RVA: 0x00098C45 File Offset: 0x00096E45
		// (set) Token: 0x06001A4A RID: 6730 RVA: 0x00098C4D File Offset: 0x00096E4D
		public AIService AIService { get; set; }

		// Token: 0x06001A4B RID: 6731 RVA: 0x00098C58 File Offset: 0x00096E58
		public ErrorSurvey(ParameterHandling parHandling, DBConnection dbConnection, MachineState machineState, MessageSystem messageSystem, ErrorHandling errorHandling, SSTEthernetServer sstServer, XrayState xrayState, DetectorType detectorType, DBDataLogs dbDatalogs)
		{
			this.parHandling = parHandling;
			this.dbConnection = dbConnection;
			this.dbDatalogs = dbDatalogs;
			this.machineState = machineState;
			this.messageSystem = messageSystem;
			this.errorHandling = errorHandling;
			this.sstServer = sstServer;
			this.detectorType = detectorType;
			this.ParameterPortOk = true;
			this.ImagePortOk = true;
			this.AIModelPortOk = true;
			ErrorSurvey.CommDetector = false;
			ErrorSurvey.CommXRaySource = false;
			ErrorSurvey.CheckArcsIsActive = true;
			this.machineErrors = new MachineErrors(machineState, dbConnection);
			this.conveyor = new Conveyor(dbConnection, parHandling, xrayState);
			this.PostUncommittedErrors();
		}

		// Token: 0x06001A4C RID: 6732 RVA: 0x00098DD4 File Offset: 0x00096FD4
		public void StartSurvey()
		{
			bool flag = !this.surveyRunning;
			if (flag)
			{
				this.surveyRunning = true;
				new Thread(new ThreadStart(this.Survey))
				{
					Name = "Survey errors"
				}.Start();
			}
		}

		// Token: 0x06001A4D RID: 6733 RVA: 0x00098E1D File Offset: 0x0009701D
		public void StopSurvey()
		{
			this.surveyRunning = false;
		}

		// Token: 0x06001A4E RID: 6734 RVA: 0x00098E28 File Offset: 0x00097028
		private void Survey()
		{
			int num = 0;
			int ioSubVersion = 0;
			int ioType = 0;
			this.timeOfCheckStart = DateTime.Now;
			while (this.dbConnection.Connection == null)
			{
				bool flag = Application.Current == null;
				if (flag)
				{
					return;
				}
				Thread.Sleep(this.surveyPeriod);
			}
			while (this.surveyRunning && Application.Current != null)
			{
				bool flag2 = !this.CheckImageProcessingNetwork();
				if (flag2)
				{
					break;
				}
				Thread.Sleep(this.surveyPeriod);
			}
			Thread.Sleep(1000);
			this.CheckDatabaseVersion();
			while (this.surveyRunning && Application.Current != null)
			{
				try
				{
					this.dbConnection.GetMachineMode(ref this.machineMode);
					this.machineErrors.ImageProcPcOffline = this.CheckImageProcessingNetwork();
					bool flag3 = !this.machineErrors.ImageProcPcOffline;
					if (flag3)
					{
						this.machineErrors.ImagePortCommError = this.ImagePortError();
						this.machineErrors.ParamPortCommError = this.ParameterPortError();
						bool flag4 = !this.machineErrors.ParamPortCommError;
						if (flag4)
						{
							this.GetIOVersions(out num, out ioSubVersion, out ioType);
							this.machineErrors.Detector = (this.CheckForDetectorCommError() && this.errorHandling.DetectorErrorEnabled);
							bool flag5 = this.CheckIPDualDetector();
							if (flag5)
							{
								this.PostMessage(SSTEntryCode.DetectorComm);
							}
							this.SetMessageErrorXRayOperatingHours();
							this.machineErrors.XRay = (this.CheckXRayFaults() && this.errorHandling.XrayErrorEnabled);
							bool flag6 = this.machineMode != MachineMode.Bulk && this.machineMode != MachineMode.BulkObject;
							if (flag6)
							{
								this.CheckArcs();
							}
							this.machineErrors.Calibration = (this.CheckCalibrationError() && this.errorHandling.DetectorErrorEnabled);
							this.CheckBgMonContaminantOnConveyor();
							this.machineErrors.HELEDifferenceforDual = this.CheckDiffHELEforDual();
							bool flag7 = this.parHandling.ReadPar(SVT.svtEnablePath) == "1";
							if (flag7)
							{
								this.machineErrors.SVTComm = this.CheckForSVTCommError();
								this.machineErrors.SVTStatus = this.CheckSVTErrorStatus();
								this.machineErrors.LowAirPressure = (this.CheckSVTAirPressure() && this.errorHandling.LowAirPressureErrorEnabled);
							}
							else
							{
								this.machineErrors.SVTComm = false;
								this.machineErrors.SVTStatus = false;
								bool flag8 = this.machineMode == MachineMode.Bulk;
								if (flag8)
								{
									this.machineErrors.LowAirPressure = false;
								}
							}
							this.machineErrors.ImageDelay = this.CheckImageDelay();
							this.machineErrors.MemoryUsage = this.CheckMemoryUsage();
							this.machineErrors.AuditFailed = (this.CheckAuditFailedError() && this.auditRequired.AuditFailedError);
							this.machineErrors.InsightNetConnectionError = (this.CheckInsightNetConnection() && this.errorHandling.InsightNetConnectionErrorEnabled);
							this.machineErrors.DiskSpaceLowError = (this.CheckDiskSpaceLow() && this.errorHandling.DiskSpaceLowErrorEnabled);
							bool flag9 = this.CheckForIOCommError();
							if (flag9)
							{
								this.machineErrors.IOComm = true;
							}
							else
							{
								this.machineErrors.IOComm = false;
								this.CheckSafetySwitches(num);
								this.CheckOtherIOs(num, ioSubVersion);
								bool flag10 = num < Definition.minSesotecIOProgramVersion;
								if (flag10)
								{
									this.machineErrors.WAGOCode = this.CheckWAGOErrorCode();
								}
								else
								{
									this.machineErrors.WAGOCode = this.CheckSesotecIOErrors();
								}
								this.machineErrors.XRayOnLamp = (this.CheckErrorXrayOnLamp() && this.errorHandling.XrayLampErrorEnabled);
								this.CheckIOVersions(num, ioSubVersion, ioType);
								this.CheckBinFull(ioSubVersion);
								this.CheckLightBarrier(num, ioSubVersion);
								this.CheckMissingVerifyObjects(ioSubVersion);
								this.CheckVibroInManualMode();
								this.machineErrors.XrayMotorError = this.CheckXRayMotorError();
								this.machineErrors.BinMissingError = (this.CheckBinMissingErrors() && this.errorHandling.BinMissingEnabled);
								this.machineErrors.HighLevelSensorsError = (this.CheckHighLevelSensorsErrors() && this.errorHandling.HighLevelSensorsEnabled);
								this.machineErrors.TooManyRejectsError = (this.CheckForTooManyRejects() && this.errorHandling.TooManyRejectsEnabled);
								this.machineErrors.InOutletError = this.CheckInOutletErrors();
							}
							bool flag11 = this.parHandling.ReadPar(Definition.anomalyDetectionEnabled) == "1" || this.parHandling.ReadPar(Definition.objectDetectionEnabled) == "1";
							if (flag11)
							{
								this.machineErrors.AIDeploymentError = this.CheckAIDeploymentErrors();
								this.machineErrors.AIInferenceError = this.CheckAIInferenceErrors();
							}
							else
							{
								this.machineErrors.AIDeploymentError = false;
								this.machineErrors.AIInferenceError = false;
							}
							this.machineErrors.ExternalNetworkForSavingPngError = this.CheckConnectionExternalStorageLocation();
							bool flag12 = !this.machineErrors.HasAnyError();
							if (flag12)
							{
								bool flag13;
								if (!this.RequestReset)
								{
									flag13 = ((from message in this.messageSystem.Messages
									where message.LogbookEntry.Type == MessageType.Error
									select message).Count<Message>() == 0);
								}
								else
								{
									flag13 = true;
								}
								bool flag14 = flag13;
								if (flag14)
								{
									this.machineState.Error = 0;
									this.machineState.ErrorsBeltStopForSignalLamp = false;
									this.dbConnection.SetHasAnyError(false);
								}
							}
							this.RequestReset = false;
						}
					}
					Thread.Sleep(this.surveyPeriod);
				}
				catch (Exception ex)
				{
					Exception ex3;
					Exception ex2 = ex3;
					Exception ex = ex2;
					bool flag15 = Application.Current != null;
					if (flag15)
					{
						Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(delegate()
						{
							Dialog.Show("Error", ex.Message);
						}));
					}
				}
			}
		}

		// Token: 0x06001A4F RID: 6735 RVA: 0x0009943C File Offset: 0x0009763C
		private bool CheckForDetectorCommError()
		{
			bool flag = false;
			DetectorType detectorType = DetectorType.DT;
			bool flag2 = this.dbConnection.GetDetectorType(ref detectorType);
			if (flag2)
			{
				string path = (detectorType == DetectorType.DT || detectorType == DetectorType.DTNetSE || detectorType == DetectorType.DTNetDE || detectorType == DetectorType.DTNetTDI) ? "/hardware/detector/dt/CommError/Value" : "/hardware/detector/sesotec/CommError/Value";
				string a = this.parHandling.ReadPar(path);
				bool flag3 = a == "1";
				if (flag3)
				{
					this.nDetectorCommFaults++;
					bool flag4 = this.nDetectorCommFaults > this.maxDetectorCommFaults && Application.Current != null;
					if (flag4)
					{
						this.PostMessage(SSTEntryCode.DetectorComm);
						this.machineState.ErrorsBeltStopForSignalLamp = true;
						flag = true;
						this.checkSpeedCalc = true;
						bool flag5 = this.parHandling.ReadPar(XRayOnProcessing.xrayEnablePath) == "1";
						if (flag5)
						{
							XRayOnProcessing.DisableXray(this.parHandling, this.dbConnection, true);
						}
					}
				}
				else
				{
					this.nDetectorCommFaults = 0;
				}
			}
			bool flag6 = this.conveyor != null && this.checkSpeedCalc && !flag;
			if (flag6)
			{
				this.checkSpeedCalcCount++;
				bool flag7 = this.checkSpeedCalcCount <= 1;
				if (flag7)
				{
					this.dbConnection.GetParameterValue("ConveyorSpeed", ref this.strSpeedConnectFault);
					this.conveyor.AdaptScanrate(false, this.strSpeedConnectFault);
				}
				else
				{
					this.checkSpeedCalc = false;
					this.checkSpeedCalcCount = 0;
				}
			}
			ErrorSurvey.CommDetector = flag;
			return flag;
		}

		// Token: 0x06001A50 RID: 6736 RVA: 0x000995C4 File Offset: 0x000977C4
		private bool CheckCalibrationError()
		{
			bool result = false;
			bool flag = this.parHandling.ReadPar(Definition.isLiveCalibrationActivePath) == "1" || this.parHandling.ReadPar(Definition.isLiveCalibrationActivePath) == "2";
			bool flag2 = !ErrorSurvey.CommDetector && !ErrorSurvey.CommXRaySource && !this.machineErrors.XRayOnLamp;
			if (flag2)
			{
				bool flag3 = flag;
				if (flag3)
				{
					XRayOnProcessing.CalibrationError = (this.parHandling.ReadPar(Definition.LiveCalibrationError) == "1");
				}
				bool calibrationError = XRayOnProcessing.CalibrationError;
				if (calibrationError)
				{
					string param = this.parHandling.ReadPar(Definition.maxBackgroundValueRealPath);
					string text = this.parHandling.ReadPar(Definition.maxBackgroundValueImagePath);
					bool flag4 = flag;
					if (flag4)
					{
						bool flag5 = text != this.oldMaxBackgroundValueImage;
						if (flag5)
						{
							this.RemoveMessage(SSTEntryCode.CalibrationFault, 0);
						}
						this.oldMaxBackgroundValueImage = text;
					}
					this.PostMessage(SSTEntryCode.CalibrationFault, 0, param, text, "", "");
					this.machineState.ErrorsBeltStopForSignalLamp = true;
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06001A51 RID: 6737 RVA: 0x000996F0 File Offset: 0x000978F0
		private bool CheckDiffHELEforDual()
		{
			string a = this.parHandling.ReadPar(XRayOnProcessing.manualGainCalibrationPath);
			bool flag = this.detectorType == DetectorType.DTNetDE && this.machineState.State == MachineStates.Inspecting && a == "1";
			bool result;
			if (flag)
			{
				int num = 0;
				int num2 = 0;
				bool flag2 = false;
				int num3 = 1000;
				int num4 = 10;
				int num5 = 15;
				int.TryParse(this.parHandling.ReadPar(Definition.meanValuePath), out num);
				int.TryParse(this.parHandling.ReadPar(Definition.meanValueHEPath), out num2);
				int.TryParse(this.parHandling.ReadPar(Definition.meanValueCheckDualBorderPath), out num3);
				int.TryParse(this.parHandling.ReadPar(Definition.meanValueCheckDualCountsPath), out num4);
				int.TryParse(this.parHandling.ReadPar(Definition.backroundNoisePath), out num5);
				bool flag3 = num > (100 - num5) * 100 && num2 > (100 - num5) * 100;
				if (flag3)
				{
					bool flag4 = this.MovAverageHE.Count < 200;
					if (flag4)
					{
						this.MovAverageLE.Add(num);
						this.MovAverageHE.Add(num2);
					}
					else
					{
						this.MovAverageLE.RemoveAt(0);
						this.MovAverageHE.RemoveAt(0);
						this.MovAverageLE.Add(num);
						this.MovAverageHE.Add(num2);
						int num6 = Math.Abs(Convert.ToInt32(this.MovAverageLE.Average() - this.MovAverageHE.Average()));
						int num7 = Math.Abs(num2 - num);
						bool flag5 = num2 > num;
						if (flag5)
						{
							num7 = Math.Abs(num7 - num6);
						}
						else
						{
							num7 = Math.Abs(num7 + num6);
						}
						bool flag6 = num7 > num3;
						if (flag6)
						{
							this.counterForDualMeanValDiff++;
							bool flag7 = this.counterForDualMeanValDiff > num4;
							if (flag7)
							{
								flag2 = true;
								this.counterForDualMeanValDiff = 0;
								this.PostMessage(SSTEntryCode.MeanValueHELEforDual);
							}
						}
					}
				}
				result = flag2;
			}
			else
			{
				this.MovAverageLE.Clear();
				this.MovAverageHE.Clear();
				result = false;
			}
			return result;
		}

		// Token: 0x06001A52 RID: 6738 RVA: 0x0009991C File Offset: 0x00097B1C
		private bool CheckIPDualDetector()
		{
			bool flag = this.detectorType == DetectorType.DTNetDE;
			if (flag)
			{
				string a = this.parHandling.ReadPar(Definition.ipDetectorDtPath);
				bool flag2 = a != "192.168.1.100" && a != "";
				if (!flag2)
				{
					this.checkIPCounter = 0;
					return false;
				}
				bool flag3 = this.checkIPCounter > 15;
				if (flag3)
				{
					return true;
				}
				this.checkIPCounter++;
			}
			return false;
		}

		// Token: 0x06001A53 RID: 6739 RVA: 0x000999A4 File Offset: 0x00097BA4
		private bool CheckXRayFaults()
		{
			bool flag = false;
			bool flag2 = this.parHandling.ReadPar(Definition.xRaySourceTypePath) != "1";
			string text = this.parHandling.ReadPar(Definition.xrayFaultsPath);
			string[] array = flag2 ? Definition.xraySpellmanFaults : Definition.xrayVJFaults;
			string a = this.parHandling.ReadPar(XRayOnProcessing.xrayModelNumberPath);
			bool flag3 = a == XRayOnProcessing.X5884Model || a == XRayOnProcessing.X5885Model;
			if (flag3)
			{
				array = Definition.xraySpellmanHRFaults;
			}
			bool flag4 = text != null && ((text.Length == 8 && flag2) || text.Length == 9);
			if (flag4)
			{
				for (int i = 0; i < array.Length; i++)
				{
					string text2 = "\"" + array[i] + "\"";
					bool flag5 = text.Length > 7;
					if (flag5)
					{
						bool flag6 = i == 5;
						if (flag6)
						{
							bool flag7 = text[i] == '1';
							if (flag7)
							{
								bool flag8 = this.nCurrentUnderCurrentFaults < this.maxUnderCurrentFaults;
								if (flag8)
								{
									this.nCurrentUnderCurrentFaults++;
								}
							}
							else
							{
								this.nCurrentUnderCurrentFaults = 0;
							}
						}
						bool flag9 = i == 7;
						if (flag9)
						{
							bool flag10 = text[i] == '1';
							if (flag10)
							{
								bool flag11 = this.nCurrentInterlockFaults < this.nMaxInterlockFaults;
								if (flag11)
								{
									this.nCurrentInterlockFaults++;
								}
							}
							else
							{
								this.nCurrentInterlockFaults = 0;
							}
						}
					}
					byte b = flag2 ? ((byte)i) : ((byte)(i + 8));
					bool flag12 = text.Length > i && text[i] == '1';
					if (flag12)
					{
						bool flag13 = i == 5;
						if (flag13)
						{
							bool flag14 = this.nCurrentUnderCurrentFaults == this.maxUnderCurrentFaults;
							if (flag14)
							{
								flag = true;
								this.PostMessage(SSTEntryCode.XRayFault, b);
								this.machineState.ErrorsBeltStopForSignalLamp = true;
							}
						}
						else
						{
							bool flag15 = i == 7;
							if (flag15)
							{
								bool flag16 = this.nCurrentInterlockFaults == this.nMaxInterlockFaults;
								if (flag16)
								{
									flag = true;
									this.PostMessage(SSTEntryCode.XRayFault, b);
									this.machineState.ErrorsBeltStopForSignalLamp = true;
								}
							}
							else
							{
								bool flag17 = b != 0 && b != 11;
								if (flag17)
								{
									flag = true;
									this.PostMessage(SSTEntryCode.XRayFault, b);
									this.machineState.ErrorsBeltStopForSignalLamp = true;
								}
							}
						}
					}
				}
			}
			bool flag18 = text == "";
			if (flag18)
			{
				bool flag19 = this.nRS232XRayErrors > 3 && Application.Current != null;
				if (flag19)
				{
					bool flag20 = this.parHandling.ReadPar(XrayState.xrayPowerOnPath) == "1";
					if (flag20)
					{
						this.ncheckflankXrayComm = 1;
					}
					else
					{
						bool flag21 = this.parHandling.ReadPar(XrayState.xrayPowerOnPath) == "0" && this.ncheckflankXrayComm == 1;
						if (flag21)
						{
							this.ncheckflankXrayComm = 0;
							flag = true;
							this.PostMessage(SSTEntryCode.XRayComm);
							this.machineState.ErrorsBeltStopForSignalLamp = true;
						}
					}
				}
				this.nRS232XRayErrors++;
			}
			else
			{
				this.nRS232XRayErrors = 0;
				this.ncheckflankXrayComm = 0;
			}
			bool flag22 = false;
			this.dbConnection.GetDemoMode(ref flag22);
			bool flag23 = flag22;
			if (flag23)
			{
				ErrorSurvey.CommXRaySource = false;
			}
			else
			{
				ErrorSurvey.CommXRaySource = flag;
			}
			return flag;
		}

		// Token: 0x06001A54 RID: 6740 RVA: 0x00099D1C File Offset: 0x00097F1C
		private bool CheckForIOCommError()
		{
			bool result = false;
			string a = this.parHandling.ReadPar("/hardware/ios/CommError/Value");
			bool flag = a == "1";
			if (flag)
			{
				this.PostMessage(SSTEntryCode.IOComm);
				result = true;
			}
			return result;
		}

		// Token: 0x06001A55 RID: 6741 RVA: 0x00099D5E File Offset: 0x00097F5E
		public void PostMessage(SSTEntryCode code)
		{
			this.PostMessage(code, 0);
		}

		// Token: 0x06001A56 RID: 6742 RVA: 0x00099D6A File Offset: 0x00097F6A
		public void PostMessage(SSTEntryCode code, byte param1)
		{
			this.PostMessage(code, param1, "", "", "", "");
		}

		// Token: 0x06001A57 RID: 6743 RVA: 0x00099D8A File Offset: 0x00097F8A
		public void PostMessage(SSTEntryCode code, byte param1, string param2, string param3, string param4, string param5)
		{
			this.PostMessage(code, param1, param2, param3, param4, param5, true);
		}

		// Token: 0x06001A58 RID: 6744 RVA: 0x00099DA0 File Offset: 0x00097FA0
		public void PostMessage(SSTEntryCode code, byte param1, string param2, string param3, string param4, string param5, bool compareAllParams)
		{
			bool flag = Application.Current != null;
			if (flag)
			{
				Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(delegate()
				{
					bool compareAllParams2 = compareAllParams;
					if (compareAllParams2)
					{
						bool flag2 = !this.messageSystem.EntryExisiting(code, param1, param2, param3, param4, param5);
						if (flag2)
						{
							SSTLogbookEntry entry = LogbookManager.AddEntry(code, param1, param2, param3, param4, param5);
							this.messageSystem.PostRegisteredMessage(entry);
						}
					}
					else
					{
						bool flag3 = !this.messageSystem.EntryExisiting(code, param1);
						if (flag3)
						{
							SSTLogbookEntry entry2 = LogbookManager.AddEntry(code, param1, param2, param3, param4, param5);
							this.messageSystem.PostRegisteredMessage(entry2);
						}
					}
				}));
			}
		}

		// Token: 0x06001A59 RID: 6745 RVA: 0x00099E1C File Offset: 0x0009801C
		public void RemoveMessage(SSTEntryCode code, byte param1)
		{
			bool flag = Application.Current != null;
			if (flag)
			{
				Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(delegate()
				{
					this.messageSystem.RemoveRegisteredMessage(code, param1);
				}));
			}
		}

		// Token: 0x06001A5A RID: 6746 RVA: 0x00099E70 File Offset: 0x00098070
		private void ModifyMessage(SSTEntryCode code, byte param1, string newParam2, string newParam3, string newParam4, string newParam5)
		{
			bool flag = Application.Current != null;
			if (flag)
			{
				Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(delegate()
				{
					SSTLogbookEntry sstlogbookEntry = null;
					bool flag2 = !this.messageSystem.EntryExisiting(code, param1, newParam2, newParam3, newParam4, newParam5);
					if (flag2)
					{
						sstlogbookEntry = LogbookManager.AddEntry(code, param1, newParam2, newParam3, newParam4, newParam5);
						bool flag3 = sstlogbookEntry.EntryCode == SSTEntryCode.RejectVerify;
						if (flag3)
						{
							this.messageSystem.NotifyReject();
						}
					}
					bool flag4 = !this.messageSystem.ModifyMessage(code, param1, newParam2, newParam3, newParam4, newParam5);
					if (flag4)
					{
						bool flag5 = sstlogbookEntry != null;
						if (flag5)
						{
							this.messageSystem.PostRegisteredMessage(sstlogbookEntry);
						}
					}
				}));
			}
		}

		// Token: 0x06001A5B RID: 6747 RVA: 0x00099EE4 File Offset: 0x000980E4
		private bool CheckWAGOErrorCode()
		{
			bool result = false;
			string text = this.parHandling.ReadPar("/hardware/ios/LEDErrorCode/Value");
			int num = 0;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("Error code: ");
			bool flag = int.TryParse(text, out num) && num > 0 && num <= 10;
			if (flag)
			{
				this.nWAGOErrors++;
			}
			else
			{
				this.nWAGOErrors = 0;
			}
			bool flag2 = this.nWAGOErrors > 3;
			if (flag2)
			{
				stringBuilder.Append(text);
				stringBuilder.Append(" Error argument: ");
				text = this.parHandling.ReadPar("/hardware/ios/LEDErrorArgument/Value");
				stringBuilder.Append(text);
				this.PostMessage(SSTEntryCode.IOController, 0, num.ToString(), text, "", "");
				result = true;
			}
			return result;
		}

		// Token: 0x06001A5C RID: 6748 RVA: 0x00099FB4 File Offset: 0x000981B4
		private bool CheckSesotecIOErrors()
		{
			bool result = false;
			string s = this.parHandling.ReadPar("/hardware/ios/ErrorDO/Value");
			int num = 0;
			string s2 = this.parHandling.ReadPar("/hardware/ios/ErrorAI/Value");
			int num2 = 0;
			string a = this.parHandling.ReadPar("/hardware/ios/CommErrorSM/Value");
			int.TryParse(s, out num);
			int.TryParse(s2, out num2);
			bool flag = num > 0;
			if (flag)
			{
				bool flag2 = BitOperation.IsBitSet((uint)num, 0);
				if (flag2)
				{
					this.PostMessage(SSTEntryCode.IOController, 1, 4.ToString(), 0.ToString(), "", "");
				}
				bool flag3 = BitOperation.IsBitSet((uint)num, 1);
				if (flag3)
				{
					this.PostMessage(SSTEntryCode.IOController, 1, 4.ToString(), 1.ToString(), "", "");
				}
				bool flag4 = BitOperation.IsBitSet((uint)num, 2);
				if (flag4)
				{
					this.PostMessage(SSTEntryCode.IOController, 1, 4.ToString(), 2.ToString(), "", "");
				}
				bool flag5 = BitOperation.IsBitSet((uint)num, 3);
				if (flag5)
				{
					this.PostMessage(SSTEntryCode.IOController, 1, 4.ToString(), 3.ToString(), "", "");
				}
				bool flag6 = BitOperation.IsBitSet((uint)num, 4);
				if (flag6)
				{
					this.PostMessage(SSTEntryCode.IOController, 1, 4.ToString(), 4.ToString(), "", "");
				}
				bool flag7 = BitOperation.IsBitSet((uint)num, 5);
				if (flag7)
				{
					this.PostMessage(SSTEntryCode.IOController, 1, 4.ToString(), 5.ToString(), "", "");
				}
				bool flag8 = BitOperation.IsBitSet((uint)num, 6);
				if (flag8)
				{
					this.PostMessage(SSTEntryCode.IOController, 1, 4.ToString(), 6.ToString(), "", "");
				}
				bool flag9 = BitOperation.IsBitSet((uint)num, 7);
				if (flag9)
				{
					this.PostMessage(SSTEntryCode.IOController, 1, 4.ToString(), 7.ToString(), "", "");
				}
				bool flag10 = BitOperation.IsBitSet((uint)num, 8);
				if (flag10)
				{
					this.PostMessage(SSTEntryCode.IOController, 1, 4.ToString(), 8.ToString(), "", "");
				}
				bool flag11 = BitOperation.IsBitSet((uint)num, 9);
				if (flag11)
				{
					this.PostMessage(SSTEntryCode.IOController, 1, 4.ToString(), 9.ToString(), "", "");
				}
				result = true;
			}
			bool flag12 = num2 > 0;
			if (flag12)
			{
				bool flag13 = BitOperation.IsBitSet((byte)num2, 0);
				if (flag13)
				{
					this.PostMessage(SSTEntryCode.IOController, 1, 5.ToString(), 0.ToString(), "", "");
				}
				bool flag14 = BitOperation.IsBitSet((byte)num2, 1);
				if (flag14)
				{
					this.PostMessage(SSTEntryCode.IOController, 1, 5.ToString(), 1.ToString(), "", "");
				}
				bool flag15 = BitOperation.IsBitSet((byte)num2, 2);
				if (flag15)
				{
					this.PostMessage(SSTEntryCode.IOController, 1, 5.ToString(), 2.ToString(), "", "");
				}
				bool flag16 = BitOperation.IsBitSet((byte)num2, 3);
				if (flag16)
				{
					this.PostMessage(SSTEntryCode.IOController, 1, 5.ToString(), 3.ToString(), "", "");
				}
				result = true;
			}
			bool flag17 = a == "1";
			if (flag17)
			{
				this.PostMessage(SSTEntryCode.IOController, 1, 6.ToString(), "", "", "");
				result = true;
			}
			return result;
		}

		// Token: 0x06001A5D RID: 6749 RVA: 0x0009A364 File Offset: 0x00098564
		private bool CheckBinFull(int ioSubVersion)
		{
			bool result = false;
			bool flag = ioSubVersion != 1;
			if (flag)
			{
				string a = this.parHandling.ReadPar("/hardware/rejecter/rejecter1/BinFull/Value");
				string a2 = this.parHandling.ReadPar("/hardware/rejecter/rejecter2/BinFull/Value");
				string a3 = this.parHandling.ReadPar("/hardware/rejecter/rejecter3/BinFull/Value");
				string a4 = this.parHandling.ReadPar("/hardware/rejecter/rejecter4/BinFull/Value");
				bool flag2 = a == "1";
				if (flag2)
				{
					this.machineErrors.BinFull1 = this.errorHandling.BinFullEnabled;
					this.PostMessage(SSTEntryCode.BinFull, 1);
					this.machineState.ErrorsBeltStopForSignalLamp = true;
				}
				else
				{
					this.machineErrors.BinFull1 = false;
				}
				bool flag3 = a2 == "1";
				if (flag3)
				{
					this.machineErrors.BinFull2 = this.errorHandling.BinFullEnabled;
					this.PostMessage(SSTEntryCode.BinFull, 2);
					this.machineState.ErrorsBeltStopForSignalLamp = true;
				}
				else
				{
					this.machineErrors.BinFull2 = false;
				}
				bool flag4 = a3 == "1";
				if (flag4)
				{
					this.machineErrors.BinFull3 = this.errorHandling.BinFullEnabled;
					this.PostMessage(SSTEntryCode.BinFull, 3);
					this.machineState.ErrorsBeltStopForSignalLamp = true;
				}
				else
				{
					this.machineErrors.BinFull3 = false;
				}
				bool flag5 = a4 == "1";
				if (flag5)
				{
					this.machineErrors.BinFull4 = this.errorHandling.BinFullEnabled;
					this.PostMessage(SSTEntryCode.BinFull, 4);
					this.machineState.ErrorsBeltStopForSignalLamp = true;
				}
				else
				{
					this.machineErrors.BinFull4 = false;
				}
			}
			return result;
		}

		// Token: 0x06001A5E RID: 6750 RVA: 0x0009A514 File Offset: 0x00098714
		private bool CheckMissingVerifyObjects(int ioSubVersion)
		{
			bool result = false;
			bool flag = ioSubVersion != 1;
			if (flag)
			{
				string s = this.parHandling.ReadPar("/hardware/rejecter/rejecter1/VerifyMissingObjects/Value");
				string s2 = this.parHandling.ReadPar("/hardware/rejecter/rejecter2/VerifyMissingObjects/Value");
				string s3 = this.parHandling.ReadPar("/hardware/rejecter/rejecter3/VerifyMissingObjects/Value");
				string s4 = this.parHandling.ReadPar("/hardware/rejecter/rejecter4/VerifyMissingObjects/Value");
				int num;
				int.TryParse(s, out num);
				int num2;
				int.TryParse(s2, out num2);
				int num3;
				int.TryParse(s3, out num3);
				int num4;
				int.TryParse(s4, out num4);
				bool flag2 = num > 0;
				if (flag2)
				{
					this.machineErrors.MissingObjects1 = this.errorHandling.MissingObjectsEnabled;
					this.ModifyMessage(SSTEntryCode.RejectVerify, 1, num.ToString(), "", "", "");
				}
				else
				{
					this.machineErrors.MissingObjects1 = false;
				}
				bool flag3 = num2 > 0;
				if (flag3)
				{
					this.machineErrors.MissingObjects2 = this.errorHandling.MissingObjectsEnabled;
					this.ModifyMessage(SSTEntryCode.RejectVerify, 2, num2.ToString(), "", "", "");
				}
				else
				{
					this.machineErrors.MissingObjects2 = false;
				}
				bool flag4 = num3 > 0;
				if (flag4)
				{
					this.machineErrors.MissingObjects3 = this.errorHandling.MissingObjectsEnabled;
					this.ModifyMessage(SSTEntryCode.RejectVerify, 3, num3.ToString(), "", "", "");
				}
				else
				{
					this.machineErrors.MissingObjects3 = false;
				}
				bool flag5 = num4 > 0;
				if (flag5)
				{
					this.machineErrors.MissingObjects4 = this.errorHandling.MissingObjectsEnabled;
					this.ModifyMessage(SSTEntryCode.RejectVerify, 4, num4.ToString(), "", "", "");
				}
				else
				{
					this.machineErrors.MissingObjects4 = false;
				}
			}
			return result;
		}

		// Token: 0x06001A5F RID: 6751 RVA: 0x0009A6F4 File Offset: 0x000988F4
		private bool CheckSafetySwitches(int ioVersion)
		{
			bool result = false;
			InOut[] array = null;
			int num = 0;
			string s = this.parHandling.ReadPar(Definition.ioBulkVersionPath);
			int.TryParse(s, out num);
			bool flag = this.machineMode == MachineMode.Bulk;
			if (flag)
			{
				bool flag2 = this.parHandling.ReadPar(Definition.wagoMachineTypePath) == Definition.ioModuleIsRayconDPlus1Module;
				if (flag2)
				{
					array = ErrorSurvey.IOsSafetySwitchesDPlus1ModuleBulk;
				}
				else
				{
					array = ErrorSurvey.IOsSafetySwitchesDPlusBulk;
				}
			}
			else
			{
				MachineType machineType = MachineType.MachineTypeUndefined;
				bool flag3 = ioVersion > 0 && this.dbConnection.GetMachineType(ref machineType);
				if (flag3)
				{
					bool flag4 = machineType == MachineType.RayconXRS || machineType == MachineType.RayconD;
					if (flag4)
					{
						array = ((ioVersion < Definition.minNewWagoConfigVersion || ioVersion >= Definition.minSesotecIOProgramVersion) ? ErrorSurvey.IOsSafetySwitchesOld : ErrorSurvey.IOsSafetySwitchesNew);
					}
					else
					{
						bool flag5 = machineType == MachineType.RayconEntry;
						if (flag5)
						{
							array = ErrorSurvey.IOsSafetySwitchesEntry;
						}
						else
						{
							bool flag6 = machineType == MachineType.RayconDPlus;
							if (flag6)
							{
								bool flag7 = this.parHandling.ReadPar(Definition.wagoMachineTypePath) == Definition.ioModuleIsRayconDPlus1Module;
								if (flag7)
								{
									array = ErrorSurvey.IOsSafetySwitchesDPlus1Module;
								}
								else
								{
									bool flag8 = this.parHandling.ReadPar(Definition.wagoMachineTypePath) == Definition.ioModuleIsRayconDPlus1ModuleHYRework;
									if (flag8)
									{
										array = ErrorSurvey.IOsSafetySwitchesDPlus1ModuleHYRework;
									}
									else
									{
										array = ErrorSurvey.IOsSafetySwitchesDPlus;
									}
								}
							}
							else
							{
								bool flag9 = machineType == MachineType.RayconEX2A;
								if (flag9)
								{
									bool flag10 = this.parHandling.ReadPar(XrayState.xrayStarterPath) == "1" && !this.isXRayOnEX2A;
									if (flag10)
									{
										this.isXRayOnEX2A = true;
									}
									bool flag11 = this.parHandling.ReadPar(XrayState.xrayStarterPath) == "0" && this.isXRayOnEX2A;
									if (flag11)
									{
										this.isXRayOnEX2A = false;
										bool flag12 = this.parHandling.ReadPar("/hardware/ios/DI_00/Value") == "1";
										if (flag12)
										{
											this.PostMessage(SSTEntryCode.SafetyCircutEx2A);
										}
									}
								}
							}
						}
					}
				}
			}
			bool flag13 = array != null;
			if (flag13)
			{
				for (int i = 0; i < array.Length; i++)
				{
					bool flag14 = false;
					this.dbConnection.GetSafetySwitchMessageAsError(ref flag14);
					bool flag15 = array[i].IOPath != "";
					if (flag15)
					{
						bool flag16 = !flag14;
						if (flag16)
						{
							bool flag17 = this.parHandling.ReadPar(ErrorSurvey.ioRoot + array[i].IOPath + "/Value") != "0";
							if (flag17)
							{
								this.RemoveMessage(SSTEntryCode.SafetySwitchOpenInfo, (byte)array[i].SwitchID);
							}
							else
							{
								result = true;
								this.PostMessage(SSTEntryCode.SafetySwitchOpenInfo, (byte)array[i].SwitchID);
							}
							this.machineErrors.SafetySwitchesAsError = false;
						}
						else
						{
							bool flag18 = this.parHandling.ReadPar(ErrorSurvey.ioRoot + array[i].IOPath + "/Value") == "0";
							if (flag18)
							{
								result = true;
								this.PostMessage(SSTEntryCode.SafetySwitchOpenError, (byte)array[i].SwitchID);
								this.machineState.ErrorsBeltStopForSignalLamp = true;
								this.machineErrors.SafetySwitchesAsError = true;
							}
							else
							{
								this.machineErrors.SafetySwitchesAsError = false;
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06001A60 RID: 6752 RVA: 0x0009AA44 File Offset: 0x00098C44
		private void CheckOtherIOs(int ioProgramVersion, int ioSubVersion)
		{
			MachineType machineType = MachineType.MachineTypeUndefined;
			bool flag = this.dbConnection.GetMachineType(ref machineType) && ioProgramVersion > 0;
			if (flag)
			{
				bool flag2 = this.parHandling.ReadPar(ErrorSurvey.ioRoot + ErrorSurvey.ioErrorCooling.IOPath + "/Value") != "1";
				if (flag2)
				{
					this.machineErrors.Cooling = false;
				}
				else
				{
					this.machineErrors.Cooling = this.errorHandling.CoolingErrorEnabled;
					this.PostMessage(SSTEntryCode.CoolingError);
					this.machineState.ErrorsBeltStopForSignalLamp = true;
				}
				bool flag3 = machineType == MachineType.RayconXRS || this.parHandling.ReadPar(XrayState.sourceXrayOnPath) == "1";
				bool flag4 = this.parHandling.ReadPar(FlapsOverIOs.bulkflapsEnabledPath) == "1";
				if (flag4)
				{
					bool flag5 = this.machineState.State == MachineStates.XrayOnCalibrationNotDone;
					if (flag5)
					{
						flag3 = false;
					}
				}
				bool flag6 = ioProgramVersion < Definition.minNewWagoConfigVersion;
				InOut inOut;
				if (flag6)
				{
					inOut = ErrorSurvey.ioLowAirPressureOld;
				}
				else
				{
					bool flag7 = ioProgramVersion < Definition.minSesotecIOProgramVersion;
					if (flag7)
					{
						inOut = ErrorSurvey.ioLowAirPressureNew;
					}
					else
					{
						bool flag8 = machineType == MachineType.RayconEntry;
						if (flag8)
						{
							inOut = ErrorSurvey.ioLowAirPressureEntry;
						}
						else
						{
							bool flag9 = machineType == MachineType.RayconDPlus && (this.parHandling.ReadPar(Definition.wagoMachineTypePath) == Definition.ioModuleIsRayconDPlus1Module || this.parHandling.ReadPar(Definition.wagoMachineTypePath) == Definition.ioModuleIsRayconDPlus1ModuleHYRework);
							if (flag9)
							{
								inOut = ErrorSurvey.ioLowAirPressureRayconDPlus1Module;
							}
							else
							{
								bool flag10 = machineType == MachineType.RayconEX2A;
								if (flag10)
								{
									inOut = ErrorSurvey.ioLowAirPressureEntry;
								}
								else
								{
									inOut = ErrorSurvey.ioLowAirPressureSesotecIO;
								}
							}
						}
					}
				}
				string b = (machineType == MachineType.RayconXRS || machineType == MachineType.RayconD) ? "1" : "0";
				bool flag11 = !flag3 || !this.checkAirPressure || this.parHandling.ReadPar(ErrorSurvey.ioRoot + inOut.IOPath + "/Value") != b;
				if (flag11)
				{
					this.machineErrors.LowAirPressure = false;
				}
				else
				{
					this.machineErrors.LowAirPressure = this.errorHandling.LowAirPressureErrorEnabled;
					this.PostMessage(SSTEntryCode.LowAirPressure);
					this.machineState.ErrorsBeltStopForSignalLamp = true;
				}
				bool flag12 = machineType == MachineType.RayconDPlus && this.parHandling.ReadPar(Definition.wagoMachineTypePath) == Definition.ioModuleIsRayconDPlus1ModuleHYRework;
				if (flag12)
				{
					bool flag13 = this.parHandling.ReadPar(ErrorSurvey.ioRoot + ErrorSurvey.ioEmergencyStop.IOPath + "/Value") == "1";
					bool flag14 = this.parHandling.ReadPar(ErrorSurvey.ioRoot + ErrorSurvey.ioEmergencyStopExternal.IOPath + "/Value") == "1";
					bool flag15 = flag13 || flag14;
					bool flag16 = this.parHandling.ReadPar(ErrorSurvey.ioRoot + ErrorSurvey.ioErrorMotor1_2_HYRework.IOPath + "/Value") == "0" || this.parHandling.ReadPar(ErrorSurvey.ioRoot + ErrorSurvey.ioErrorMotor3_HYRework.IOPath + "/Value") == "0" || this.parHandling.ReadPar(ErrorSurvey.ioRoot + ErrorSurvey.ioErrorMotor4_HYRework.IOPath + "/Value") == "0" || this.parHandling.ReadPar(ErrorSurvey.ioRoot + ErrorSurvey.ioErrorMotor5_HYRework.IOPath + "/Value") == "0" || this.parHandling.ReadPar(ErrorSurvey.ioRoot + ErrorSurvey.ioErrorMotor6_HYRework.IOPath + "/Value") == "0" || this.parHandling.ReadPar(ErrorSurvey.ioRoot + ErrorSurvey.ioErrorMotor7_HYRework.IOPath + "/Value") == "0";
					bool flag17 = flag15 || this.emergencyStopWasPressed || !flag16;
					if (flag17)
					{
						this.machineErrors.Motor = false;
						this.counterErrorMotor = 0;
					}
					else
					{
						bool flag18 = this.counterErrorMotor >= 3;
						if (flag18)
						{
							this.machineErrors.Motor = this.errorHandling.EngineErrorEnabled;
							this.PostMessage(SSTEntryCode.MotorError, 0, this.getMotorNumberHYRework(), "", "", "");
							this.machineState.ErrorsBeltStopForSignalLamp = true;
							this.counterErrorMotor = 0;
						}
						else
						{
							this.counterErrorMotor++;
						}
					}
					this.emergencyStopWasPressed = flag15;
					bool flag19 = !flag15;
					if (flag19)
					{
						this.RemoveMessage(SSTEntryCode.EmergencyStopPressedInfo, 0);
						this.machineErrors.SafetySwitchesAsError = false;
					}
					else
					{
						string param = "";
						bool flag20 = flag13 && !flag14;
						if (flag20)
						{
							param = "1";
						}
						else
						{
							bool flag21 = !flag13 && flag14;
							if (flag21)
							{
								param = "2";
							}
							else
							{
								bool flag22 = flag13 && flag14;
								if (flag22)
								{
									param = "3";
								}
							}
						}
						bool flag23 = false;
						this.dbConnection.GetSafetySwitchMessageAsError(ref flag23);
						bool flag24 = flag23;
						if (flag24)
						{
							this.PostMessage(SSTEntryCode.EmergencyStopPressedError, 0, param, "", "", "");
							this.machineState.ErrorsBeltStopForSignalLamp = true;
							this.machineErrors.SafetySwitchesAsError = true;
						}
						else
						{
							this.PostMessage(SSTEntryCode.EmergencyStopPressedInfo, 0, param, "", "", "");
							this.machineErrors.SafetySwitchesAsError = false;
						}
					}
				}
				else
				{
					bool flag25 = machineType == MachineType.RayconEX2A;
					bool flag15;
					if (flag25)
					{
						flag15 = (this.parHandling.ReadPar(ErrorSurvey.ioRoot + ErrorSurvey.ioEmergencyStop.IOPath + "/Value") == "0");
					}
					else
					{
						flag15 = (this.parHandling.ReadPar(ErrorSurvey.ioRoot + ErrorSurvey.ioEmergencyStop.IOPath + "/Value") == "1");
					}
					string b2 = (machineType == MachineType.RayconXRS || machineType == MachineType.RayconD || machineType == MachineType.RayconEX2A) ? "1" : "0";
					bool flag26 = this.parHandling.ReadPar(ErrorSurvey.ioRoot + ErrorSurvey.ioErrorMotor.IOPath + "/Value") == b2;
					bool flag27 = flag15 || this.emergencyStopWasPressed || !flag26;
					if (flag27)
					{
						this.machineErrors.Motor = false;
						this.counterErrorMotor = 0;
					}
					else
					{
						bool flag28 = this.counterErrorMotor >= 3;
						if (flag28)
						{
							this.machineErrors.Motor = this.errorHandling.EngineErrorEnabled;
							this.PostMessage(SSTEntryCode.MotorError);
							this.machineState.ErrorsBeltStopForSignalLamp = true;
							this.counterErrorMotor = 0;
						}
						else
						{
							this.counterErrorMotor++;
						}
					}
					this.emergencyStopWasPressed = flag15;
					bool flag29 = !flag15;
					if (flag29)
					{
						this.RemoveMessage(SSTEntryCode.EmergencyStopPressedInfo, 0);
						this.machineErrors.SafetySwitchesAsError = false;
					}
					else
					{
						bool flag30 = false;
						this.dbConnection.GetSafetySwitchMessageAsError(ref flag30);
						bool flag31 = flag30;
						if (flag31)
						{
							this.PostMessage(SSTEntryCode.EmergencyStopPressedError);
							this.machineState.ErrorsBeltStopForSignalLamp = true;
							this.machineErrors.SafetySwitchesAsError = true;
						}
						else
						{
							this.PostMessage(SSTEntryCode.EmergencyStopPressedInfo);
							this.machineErrors.SafetySwitchesAsError = false;
						}
					}
				}
				this.CheckTemperature(false, ioProgramVersion);
				this.CheckTemperature(true, ioProgramVersion);
				bool flag32 = ioProgramVersion >= 25;
				if (flag32)
				{
					bool flag33 = true;
					bool flag34 = machineType == MachineType.RayconDPlus;
					if (flag34)
					{
						bool flag35 = this.parHandling.ReadPar(Definition.wagoMachineTypePath) == Definition.ioModuleIsRayconDPlus1Module || this.parHandling.ReadPar(Definition.wagoMachineTypePath) == Definition.ioModuleIsRayconDPlus1ModuleHYRework;
						if (flag35)
						{
							flag33 = (this.parHandling.ReadPar(ErrorSurvey.ioRoot + ErrorSurvey.ioMachineEnableExternEntryAndDPlus1Module.IOPath + "/Value") == "1");
						}
						else
						{
							flag33 = (this.parHandling.ReadPar(ErrorSurvey.ioRoot + ErrorSurvey.ioMachineEnableExternSesotecIO.IOPath + "/Value") == "1");
						}
					}
					else
					{
						bool flag36 = machineType == MachineType.RayconEntry;
						if (flag36)
						{
							flag33 = (this.parHandling.ReadPar(ErrorSurvey.ioRoot + ErrorSurvey.ioMachineEnableExternEntryAndDPlus1Module.IOPath + "/Value") == "1");
						}
					}
					bool flag37 = flag33;
					if (flag37)
					{
						this.RemoveMessage(SSTEntryCode.MachineEnableExtern, 0);
					}
					else
					{
						this.PostMessage(SSTEntryCode.MachineEnableExtern);
					}
				}
			}
		}

		// Token: 0x06001A61 RID: 6753 RVA: 0x0009B2E8 File Offset: 0x000994E8
		private void CheckTemperature(bool tempSensor0, int ioVersion)
		{
			MachineType machineType = MachineType.MachineTypeUndefined;
			DetectorType detectorType = DetectorType.Unknown;
			this.dbConnection.GetMachineType(ref machineType);
			this.dbConnection.GetDetectorType(ref detectorType);
			bool flag = machineType != MachineType.RayconEX2A;
			if (flag)
			{
				bool flag2 = ioVersion > 0;
				if (flag2)
				{
					if (tempSensor0)
					{
						bool flag3 = ioVersion < Definition.minSesotecIOProgramVersion;
						InOut inOut;
						if (flag3)
						{
							inOut = ((ioVersion < Definition.minNewWagoConfigVersion) ? ErrorSurvey.ioTempSensor0Old : ErrorSurvey.ioTempSensor0New);
						}
						else
						{
							inOut = ErrorSurvey.ioTempSensorSesotecSecondary;
						}
						double num;
						bool flag4 = double.TryParse(this.parHandling.ReadPar(ErrorSurvey.ioRoot + inOut.IOPath + "/Value"), NumberStyles.AllowDecimalPoint, new CultureInfo("en-US"), out num) && num > 40.0;
						if (flag4)
						{
							this.machineErrors.Temperature0 = this.errorHandling.TemperatureErrorEnabled;
							this.PostMessage(SSTEntryCode.Temperature, 0);
							this.machineState.ErrorsBeltStopForSignalLamp = true;
						}
						else
						{
							this.machineErrors.Temperature0 = false;
						}
					}
					else
					{
						bool flag5 = ioVersion < Definition.minSesotecIOProgramVersion;
						InOut inOut;
						if (flag5)
						{
							inOut = ((ioVersion < Definition.minNewWagoConfigVersion) ? ErrorSurvey.ioTempSensor1Old : ErrorSurvey.ioTempSensor1New);
						}
						else
						{
							inOut = ErrorSurvey.ioTempSensorSesotecMain;
						}
						bool flag6 = machineType == MachineType.RayconEntry;
						if (flag6)
						{
							double num;
							bool flag7 = double.TryParse(this.parHandling.ReadPar(ErrorSurvey.ioRoot + inOut.IOPath + "/Value"), NumberStyles.AllowDecimalPoint, new CultureInfo("en-US"), out num) && num > 50.0;
							if (flag7)
							{
								this.machineErrors.Temperature1 = this.errorHandling.TemperatureErrorEnabled;
								this.PostMessage(SSTEntryCode.Temperature, 1);
								this.machineState.ErrorsBeltStopForSignalLamp = true;
							}
							else
							{
								this.machineErrors.Temperature1 = false;
							}
						}
						else
						{
							bool flag8 = machineType == MachineType.RayconDPlus && detectorType == DetectorType.DTNetTDI;
							if (flag8)
							{
								double num;
								bool flag9 = double.TryParse(this.parHandling.ReadPar(ErrorSurvey.ioRoot + inOut.IOPath + "/Value"), NumberStyles.AllowDecimalPoint, new CultureInfo("en-US"), out num) && num > 48.0;
								if (flag9)
								{
									this.machineErrors.Temperature1 = this.errorHandling.TemperatureErrorEnabled;
									this.PostMessage(SSTEntryCode.Temperature, 1);
									this.machineState.ErrorsBeltStopForSignalLamp = true;
								}
								else
								{
									this.machineErrors.Temperature1 = false;
								}
							}
							else
							{
								double num;
								bool flag10 = double.TryParse(this.parHandling.ReadPar(ErrorSurvey.ioRoot + inOut.IOPath + "/Value"), NumberStyles.AllowDecimalPoint, new CultureInfo("en-US"), out num) && num > 45.0;
								if (flag10)
								{
									this.machineErrors.Temperature1 = this.errorHandling.TemperatureErrorEnabled;
									this.PostMessage(SSTEntryCode.Temperature, 1);
									this.machineState.ErrorsBeltStopForSignalLamp = true;
								}
								else
								{
									this.machineErrors.Temperature1 = false;
								}
							}
						}
					}
				}
				else
				{
					this.machineErrors.Temperature0 = false;
					this.machineErrors.Temperature1 = false;
				}
			}
		}

		// Token: 0x06001A62 RID: 6754 RVA: 0x0009B61C File Offset: 0x0009981C
		private bool CheckForSVTCommError()
		{
			bool result = false;
			bool flag = this.machineMode == MachineMode.Bulk && this.parHandling.ReadPar(SVT.svtEnablePath) == "1";
			if (flag)
			{
				string a = this.parHandling.ReadPar(SVT.svtCommErrorPath);
				bool flag2 = a == "1";
				if (flag2)
				{
					bool flag3 = this.nCurrentSVTCommFaults < this.maxSVTCommFaults;
					if (flag3)
					{
						this.nCurrentSVTCommFaults++;
					}
					else
					{
						this.PostMessage(SSTEntryCode.SVTComm);
						result = true;
					}
				}
				else
				{
					this.nCurrentSVTCommFaults = 0;
				}
			}
			return result;
		}

		// Token: 0x06001A63 RID: 6755 RVA: 0x0009B6BB File Offset: 0x000998BB
		public static void DisableImageDelayError()
		{
			ErrorSurvey.imageDelayMaxError = 8;
		}

		// Token: 0x06001A64 RID: 6756 RVA: 0x0009B6C4 File Offset: 0x000998C4
		public static void EnableImageDelayError()
		{
			ErrorSurvey.imageDelayMaxError = 5;
		}

		// Token: 0x06001A65 RID: 6757 RVA: 0x0009B6D0 File Offset: 0x000998D0
		private bool CheckImageDelay()
		{
			bool result = false;
			int num = 0;
			int num2 = 0;
			string s = this.parHandling.ReadPar(Acquisition.imageDelayPath);
			bool flag = int.TryParse(s, out num2) && num2 > ErrorSurvey.imageDelayMaxError;
			if (flag)
			{
				bool flag2 = this.nCurrentImageProcessingFaults < this.maxImageProcessingFaults;
				if (flag2)
				{
					this.nCurrentImageProcessingFaults++;
				}
			}
			else
			{
				this.nCurrentImageProcessingFaults = 0;
			}
			int.TryParse(this.parHandling.ReadPar(Acquisition.objimageDelayPath), out num);
			bool flag3 = (this.nCurrentImageProcessingFaults == this.maxImageProcessingFaults && (this.machineState.State != MachineStates.XrayOnCalibrationNotDone || !(this.parHandling.ReadPar(Acquisition.acquisitionModePath) == "1"))) || (this.parHandling.ReadPar(Acquisition.acquisitionModePath) == "1" && num > 0);
			if (flag3)
			{
				this.PostMessage(SSTEntryCode.ProcessingDelay);
				result = true;
			}
			return result;
		}

		// Token: 0x06001A66 RID: 6758 RVA: 0x0009B7D0 File Offset: 0x000999D0
		private bool CheckMemoryUsage()
		{
			bool result = false;
			double num = 0.0;
			string s = this.parHandling.ReadPar("/application/MemoryUsage/Value");
			bool flag = double.TryParse(s, NumberStyles.Float, new CultureInfo("en-US"), out num) && num > 90.0;
			if (flag)
			{
				this.PostMessage(SSTEntryCode.MemoryUsage);
				result = true;
			}
			return result;
		}

		// Token: 0x06001A67 RID: 6759 RVA: 0x0009B83C File Offset: 0x00099A3C
		private bool CheckSVTErrorStatus()
		{
			bool result = false;
			int num = 0;
			bool flag = this.machineMode == MachineMode.Bulk && this.parHandling.ReadPar(SVT.svtEnablePath) == "1";
			if (flag)
			{
				string text = this.parHandling.ReadPar(Definition.svtErrorStatusPath);
				bool flag2 = text != null && int.TryParse(text, out num) && num > 0;
				if (flag2)
				{
					bool flag3 = text != "" && text != "0";
					if (flag3)
					{
						bool flag4 = this.nSVTFaults < this.nMaxSVTFaults;
						if (flag4)
						{
							this.nSVTFaults++;
						}
						bool flag5 = this.nSVTFaults == this.nMaxSVTFaults;
						if (flag5)
						{
							result = true;
							this.PostMessage(SSTEntryCode.SVTError, (byte)num);
						}
					}
					else
					{
						this.nSVTFaults = 0;
					}
				}
				else
				{
					this.nSVTFaults = 0;
				}
			}
			return result;
		}

		// Token: 0x06001A68 RID: 6760 RVA: 0x0009B92B File Offset: 0x00099B2B
		public void ResetNSVTFaults()
		{
			this.nSVTFaults = 0;
		}

		// Token: 0x06001A69 RID: 6761 RVA: 0x0009B938 File Offset: 0x00099B38
		private void CheckDatabaseVersion()
		{
			string a = null;
			this.dbConnection.GetDatabaseVersion(ref a);
			bool flag = a != Definition.guiVersion;
			if (flag)
			{
				this.PostMessage(SSTEntryCode.DatabaseVersion);
			}
			bool flag2 = !this.parHandling.ReadPar("/application/ElinosVersion/Value").Equals(Definition.guiVersion);
			if (flag2)
			{
				this.PostMessage(SSTEntryCode.ElinosVersion);
			}
		}

		// Token: 0x06001A6A RID: 6762 RVA: 0x0009B99C File Offset: 0x00099B9C
		private bool CheckErrorXrayOnLamp()
		{
			bool result = false;
			bool flag = DateTime.Now.Subtract(this.lastErrorXRayLamp).TotalSeconds > 12.0;
			if (flag)
			{
				string a = this.parHandling.ReadPar("/hardware/ios/ErrorXrayOnLamp/Value");
				bool flag2 = a == "1";
				if (flag2)
				{
					this.lastErrorXRayLamp = DateTime.Now;
					this.PostMessage(SSTEntryCode.XRayLamp);
					this.machineState.ErrorsBeltStopForSignalLamp = true;
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06001A6B RID: 6763 RVA: 0x0009BA2C File Offset: 0x00099C2C
		private bool CheckIOVersions(int ioVersion, int ioSubVersion, int ioType)
		{
			bool result = false;
			bool flag = ioVersion > 0;
			if (flag)
			{
				bool flag2 = ioVersion < ErrorSurvey.minWagoVersion;
				if (flag2)
				{
					this.PostMessage(SSTEntryCode.WagoOutOfDate);
					result = true;
				}
				else
				{
					bool flag3 = ioVersion < Definition.minWagoVersionForUserProgramType;
					if (flag3)
					{
						bool flag4 = (this.machineMode == MachineMode.Bulk && ioSubVersion != 0 && ioSubVersion != 3) || (this.machineMode != MachineMode.Bulk && ioSubVersion == 3) || (this.machineMode != MachineMode.MAndS && ioSubVersion == 4) || (this.machineMode == MachineMode.MAndS && ioSubVersion != 0 && ioSubVersion != 4);
						if (flag4)
						{
							this.PostMessage(SSTEntryCode.WagoVersionWrong);
							result = true;
						}
					}
					else
					{
						bool flag5 = (this.machineMode == MachineMode.Bulk && ioType != 5 && ioSubVersion != 2) || (this.machineMode == MachineMode.BulkObject && ioType != 5 && ioSubVersion != 2) || (this.machineMode != MachineMode.Bulk && ioType == 3) || (this.machineMode != MachineMode.MAndS && ioSubVersion == 3 && (ioVersion < Definition.minWagoVersionRayconD || ioVersion >= Definition.minSesotecIOProgramVersion)) || (this.machineMode == MachineMode.MAndS && ioSubVersion != 0 && ioSubVersion != 3) || (this.machineMode == MachineMode.Sideshooter && ioType != 0 && ioType != 2) || (this.machineMode != MachineMode.Sideshooter && ioType == 2);
						if (flag5)
						{
							this.PostMessage(SSTEntryCode.WagoVersionWrong);
							result = true;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06001A6C RID: 6764 RVA: 0x0009BB6C File Offset: 0x00099D6C
		private bool CheckVibroInManualMode()
		{
			bool result = false;
			bool flag = this.machineMode == MachineMode.Bulk && this.parHandling.ReadPar(SVT.svtEnablePath) == "1";
			if (flag)
			{
				string a = this.parHandling.ReadPar(Definition.vibroInManualModePath);
				bool flag2 = a == "1";
				if (flag2)
				{
					this.PostMessage(SSTEntryCode.VibroInManualMode);
					result = true;
				}
				else
				{
					this.RemoveMessage(SSTEntryCode.VibroInManualMode, 0);
				}
			}
			return result;
		}

		// Token: 0x06001A6D RID: 6765 RVA: 0x0009BBEC File Offset: 0x00099DEC
		private bool CheckRejecterSafetySwitch()
		{
			bool result = false;
			bool flag = false;
			this.dbConnection.GetSafetySwitchMessageAsError(ref flag);
			bool flag2 = this.machineMode == MachineMode.Bulk && this.parHandling.ReadPar(SVT.svtEnablePath) == "1";
			if (flag2)
			{
				string a = this.parHandling.ReadPar(Definition.switchStopRejecterPath);
				bool flag3 = a == "0";
				if (flag3)
				{
					bool flag4 = !flag;
					if (flag4)
					{
						this.PostMessage(SSTEntryCode.SafetySwitchOpenInfo, 16);
						this.machineErrors.SafetySwitchesAsError = false;
					}
					else
					{
						this.PostMessage(SSTEntryCode.SafetySwitchOpenError, 16);
						this.machineState.ErrorsBeltStopForSignalLamp = true;
						this.machineErrors.SafetySwitchesAsError = true;
					}
					result = true;
				}
				else
				{
					this.RemoveMessage(SSTEntryCode.SafetySwitchOpenInfo, 16);
					this.machineErrors.SafetySwitchesAsError = false;
				}
			}
			else
			{
				this.RemoveMessage(SSTEntryCode.SafetySwitchOpenInfo, 16);
			}
			return result;
		}

		// Token: 0x06001A6E RID: 6766 RVA: 0x0009BCEC File Offset: 0x00099EEC
		private bool CheckLightBarrier(int ioVersion, int ioSubVersion)
		{
			bool result = false;
			bool flag = ioVersion >= ErrorSurvey.minLightBarrierWagoVersion && ioSubVersion != 1;
			if (flag)
			{
				string a = this.parHandling.ReadPar("/hardware/rejecter/rejecter1/LightBarrier/Value");
				string a2 = this.parHandling.ReadPar("/hardware/rejecter/rejecter2/LightBarrier/Value");
				string a3 = this.parHandling.ReadPar("/hardware/rejecter/rejecter3/LightBarrier/Value");
				string a4 = this.parHandling.ReadPar("/hardware/rejecter/rejecter4/LightBarrier/Value");
				bool flag2 = a == "1";
				if (flag2)
				{
					this.machineErrors.LightBarrier1 = this.errorHandling.MissingObjectsEnabled;
					this.PostMessage(SSTEntryCode.LightBarrier, 1);
				}
				else
				{
					this.machineErrors.LightBarrier1 = false;
				}
				bool flag3 = a2 == "1";
				if (flag3)
				{
					this.machineErrors.LightBarrier2 = this.errorHandling.MissingObjectsEnabled;
					this.PostMessage(SSTEntryCode.LightBarrier, 2);
				}
				else
				{
					this.machineErrors.LightBarrier2 = false;
				}
				bool flag4 = a3 == "1";
				if (flag4)
				{
					this.machineErrors.LightBarrier3 = this.errorHandling.MissingObjectsEnabled;
					this.PostMessage(SSTEntryCode.LightBarrier, 3);
				}
				else
				{
					this.machineErrors.LightBarrier3 = false;
				}
				bool flag5 = a4 == "1";
				if (flag5)
				{
					this.machineErrors.LightBarrier4 = this.errorHandling.MissingObjectsEnabled;
					this.PostMessage(SSTEntryCode.LightBarrier, 4);
				}
				else
				{
					this.machineErrors.LightBarrier4 = false;
				}
			}
			return result;
		}

		// Token: 0x06001A6F RID: 6767 RVA: 0x0009BE74 File Offset: 0x0009A074
		private bool GetIOVersions(out int ioVersion, out int ioSubVersion, out int ioType)
		{
			ioSubVersion = 0;
			ioVersion = 0;
			ioType = 0;
			string s = this.parHandling.ReadPar(Definition.ioVersionPath);
			bool flag = int.TryParse(s, out ioVersion);
			string s2 = this.parHandling.ReadPar(Definition.ioSubversionPath);
			flag |= int.TryParse(s2, out ioSubVersion);
			bool flag2 = ioVersion >= Definition.minWagoVersionForUserProgramType;
			if (flag2)
			{
				string s3 = this.parHandling.ReadPar(Definition.wagoMachineTypePath);
				flag |= int.TryParse(s3, out ioType);
			}
			return flag;
		}

		// Token: 0x06001A70 RID: 6768 RVA: 0x0009BF00 File Offset: 0x0009A100
		private bool CheckAuditFailedError()
		{
			bool result = false;
			bool flag = true;
			bool flag2 = this.checkAuditIfRestartMachine;
			if (flag2)
			{
				this.checkAuditIfRestartMachine = false;
				this.dbConnection.GetAuditCheckErrorReset(ref flag);
			}
			bool flag3 = this.AuditRequired != null;
			if (flag3)
			{
				bool flag4 = (this.AuditRequired.AuditFailedError && this.AuditRequired.LastAuditFailed) || !flag;
				if (flag4)
				{
					this.AuditRequired.LastAuditFailed = false;
					bool flag5 = !this.messageSystem.EntryExisiting(SSTEntryCode.AuditFailedError, 0, "", "", "", "");
					if (flag5)
					{
						this.PostMessage(SSTEntryCode.AuditFailedError);
					}
					else
					{
						LogbookManager.AddEntry(SSTEntryCode.AuditFailedError, 0, "", "", "", "");
					}
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06001A71 RID: 6769 RVA: 0x0009BFD8 File Offset: 0x0009A1D8
		private bool CheckSVTAirPressure()
		{
			bool result = false;
			double num = 0.0;
			double num2 = 0.0;
			string a = this.parHandling.ReadPar(Definition.switchStopRejecterPath);
			bool flag = a == "1";
			if (flag)
			{
				this.dbConnection.GetSVTMinAirPressure(ref num);
				string s = this.parHandling.ReadPar(SVT.svtAirPressurePath);
				double.TryParse(s, NumberStyles.Float, new CultureInfo("en-US"), out num2);
				bool flag2 = num2 < num;
				if (flag2)
				{
					bool flag3 = this.nSVTLowAirPressureErrors < this.maxSVTLowAirPressureFaults;
					if (flag3)
					{
						this.nSVTLowAirPressureErrors++;
					}
				}
				else
				{
					this.nSVTLowAirPressureErrors = 0;
				}
				bool flag4 = this.nSVTLowAirPressureErrors == this.maxSVTLowAirPressureFaults;
				if (flag4)
				{
					this.PostMessage(SSTEntryCode.LowAirPressure);
					this.machineState.ErrorsBeltStopForSignalLamp = true;
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06001A72 RID: 6770 RVA: 0x0009C0D4 File Offset: 0x0009A2D4
		private bool CheckImageProcessingNetwork()
		{
			bool flag = false;
			bool flag2 = false;
			string text = "";
			using (Ping ping = new Ping())
			{
				bool flag3 = this.dbConnection != null;
				if (flag3)
				{
					this.dbConnection.GetIPElinos(ref text);
				}
				bool flag4 = text == "";
				if (flag4)
				{
					try
					{
						PingReply pingReply = ping.Send(Definition.defaultElinosIP, 1000);
						flag = this.pingImgPC(Definition.defaultElinosIP, pingReply, ping);
					}
					catch
					{
						flag = true;
					}
					try
					{
						PingReply pingReply = ping.Send(Definition.defaultElinosIPEX2A, 1000);
						flag2 = this.pingImgPC(Definition.defaultElinosIPEX2A, pingReply, ping);
					}
					catch
					{
						flag2 = true;
					}
				}
				else
				{
					try
					{
						PingReply pingReply = ping.Send(text, 1000);
						flag = this.pingImgPC(text, pingReply, ping);
					}
					catch
					{
						flag = true;
					}
				}
			}
			bool flag5 = (text == "" && flag && flag2) || (text != "" && flag);
			if (flag5)
			{
				this.PostMessage(SSTEntryCode.ImageProcPC);
				this.OnImageProcessingNetworkError();
			}
			return flag;
		}

		// Token: 0x06001A73 RID: 6771 RVA: 0x0009C22C File Offset: 0x0009A42C
		private bool pingImgPC(string ip, PingReply pingReply, Ping pingIPC)
		{
			bool result = false;
			bool flag = pingReply.Status > IPStatus.Success;
			if (flag)
			{
				Thread.Sleep(500);
				pingReply = pingIPC.Send(ip, 1000);
				bool flag2 = pingReply.Status > IPStatus.Success;
				if (flag2)
				{
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06001A74 RID: 6772 RVA: 0x0009C278 File Offset: 0x0009A478
		private bool ParameterPortError()
		{
			bool result = false;
			bool flag = !this.ParameterPortOk;
			if (flag)
			{
				this.PostMessage(SSTEntryCode.ParamComm);
				result = true;
			}
			return result;
		}

		// Token: 0x06001A75 RID: 6773 RVA: 0x0009C2A8 File Offset: 0x0009A4A8
		private bool ImagePortError()
		{
			bool result = false;
			bool flag = !this.ImagePortOk;
			if (flag)
			{
				this.PostMessage(SSTEntryCode.ImageComm);
				result = true;
			}
			return result;
		}

		// Token: 0x06001A76 RID: 6774 RVA: 0x0009C2D8 File Offset: 0x0009A4D8
		private bool AIModelPortError()
		{
			bool result = false;
			bool flag = !this.AIModelPortOk && (this.parHandling.ReadPar(Definition.anomalyDetectionEnabled) == "1" || this.parHandling.ReadPar(Definition.objectDetectionEnabled) == "1");
			if (flag)
			{
				this.PostMessage(SSTEntryCode.AIDeploymentError, 0);
				result = true;
			}
			return result;
		}

		// Token: 0x06001A77 RID: 6775 RVA: 0x0009C344 File Offset: 0x0009A544
		protected virtual void OnImageProcessingNetworkError()
		{
			Action imageProcessingNetworkError = this.ImageProcessingNetworkError;
			bool flag = imageProcessingNetworkError != null;
			if (flag)
			{
				imageProcessingNetworkError();
			}
		}

		// Token: 0x06001A78 RID: 6776 RVA: 0x0009C368 File Offset: 0x0009A568
		private bool CheckXRayMotorError()
		{
			bool result = false;
			bool flag = this.machineMode == MachineMode.Sideshooter;
			if (flag)
			{
				string a = this.parHandling.ReadPar(Definition.sshOvercurrentPath);
				bool flag2 = a == "1";
				if (flag2)
				{
					this.PostMessage(SSTEntryCode.XRayMotor, 0);
					result = true;
				}
				string a2 = this.parHandling.ReadPar(Definition.sshErrorPath);
				bool flag3 = a2 == "1";
				if (flag3)
				{
					this.PostMessage(SSTEntryCode.XRayMotor, 1);
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06001A79 RID: 6777 RVA: 0x0009C3FC File Offset: 0x0009A5FC
		private bool CheckArcs()
		{
			bool result = false;
			int num = 0;
			int num2 = 0;
			int num3 = 4;
			int num4 = 24;
			bool flag = false;
			string s = this.parHandling.ReadPar(Definition.serialArcsPath);
			string s2 = this.parHandling.ReadPar(Definition.imageArcsPath);
			this.dbConnection.GetNCriticalArcs(ref num3);
			this.dbConnection.GetCriticalArcsPeriod(ref num4);
			this.dbConnection.GetDBDatalogsIsActive(ref flag);
			bool flag2 = !this.arcsInitialized;
			if (flag2)
			{
				this.InitializeArcs(num3);
				this.arcsInitialized = true;
			}
			else
			{
				bool flag3 = num3 > 0 && num4 > 0;
				if (flag3)
				{
					bool flag4 = int.TryParse(s, out num);
					if (flag4)
					{
						bool flag5 = num > this.oldSerialArcs;
						if (flag5)
						{
							DateTime now = DateTime.Now;
							result = true;
							for (int i = 0; i < num - this.oldSerialArcs; i++)
							{
								this.dbConnection.AddArc(new Arc(ArcType.Serial, now));
								bool flag6 = flag;
								if (flag6)
								{
									this.dbDatalogs.ArcsAddEntry(0);
								}
							}
							this.oldSerialArcs = num;
							while (this.serialArcTimes.Count >= num3)
							{
								this.serialArcTimes.RemoveAt(0);
							}
							this.serialArcTimes.Add(now);
							bool flag7 = this.serialArcTimes.Count == num3 && this.serialArcTimes[this.serialArcTimes.Count - 1] - this.serialArcTimes[0] < new TimeSpan(num4, 0, 0);
							if (flag7)
							{
								this.PostMessage(SSTEntryCode.XrayArcs);
							}
						}
					}
					bool flag8 = int.TryParse(s2, out num2);
					if (flag8)
					{
						bool flag9 = num2 > this.oldImageArcs;
						if (flag9)
						{
							bool flag10 = this.machineState.State == MachineStates.Inspecting && ErrorSurvey.CheckArcsIsActive;
							if (flag10)
							{
								for (int j = 0; j < num2 - this.oldImageArcs; j++)
								{
									bool flag11 = flag;
									if (flag11)
									{
										this.dbDatalogs.ArcsAddEntry(2);
									}
								}
								this.oldImageArcs = num2;
							}
							else
							{
								this.oldImageArcs = num2;
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06001A7A RID: 6778 RVA: 0x0009C644 File Offset: 0x0009A844
		private void InitializeArcs(int nCriticalArcs)
		{
			List<Arc> list = new List<Arc>();
			this.dbConnection.GetArcs(list);
			List<Arc> list2 = (from item in list
			where item.Type == ArcType.ImageNew
			select item).ToList<Arc>();
			List<Arc> list3 = (from item in list
			where item.Type == ArcType.Serial
			select item).ToList<Arc>();
			list2.Sort((Arc a, Arc b) => a.Time.CompareTo(b.Time));
			list3.Sort((Arc a, Arc b) => a.Time.CompareTo(b.Time));
			bool flag = list2.Count > nCriticalArcs;
			if (flag)
			{
				list2.RemoveRange(0, list2.Count - nCriticalArcs);
			}
			bool flag2 = list3.Count > nCriticalArcs;
			if (flag2)
			{
				list3.RemoveRange(0, list3.Count - nCriticalArcs);
			}
			this.imageArcTimes.AddRange(from arc in list2
			select arc.Time);
			this.serialArcTimes.AddRange(from arc in list3
			select arc.Time);
		}

		// Token: 0x06001A7B RID: 6779 RVA: 0x0009C7A8 File Offset: 0x0009A9A8
		private bool CheckBinMissingErrors()
		{
			bool result = false;
			bool flag = this.machineMode == MachineMode.MAndS;
			if (flag)
			{
				string a = this.parHandling.ReadPar(Definition.msBinMissing1Path);
				string a2 = this.parHandling.ReadPar(Definition.msBinMissing2Path);
				bool flag2 = a == "1";
				if (flag2)
				{
					this.PostMessage(SSTEntryCode.BinMissing, 0);
					this.machineState.ErrorsBeltStopForSignalLamp = true;
					result = true;
				}
				bool flag3 = a2 == "1";
				if (flag3)
				{
					this.PostMessage(SSTEntryCode.BinMissing, 1);
					this.machineState.ErrorsBeltStopForSignalLamp = true;
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06001A7C RID: 6780 RVA: 0x0009C858 File Offset: 0x0009AA58
		private bool CheckHighLevelSensorsErrors()
		{
			bool flag = false;
			bool flag2 = this.machineMode == MachineMode.MAndS;
			if (flag2)
			{
				int num = 0;
				int num2 = 0;
				string s = this.parHandling.ReadPar(Definition.msProductsMissing1Path);
				string s2 = this.parHandling.ReadPar(Definition.msProductsMissing2Path);
				int.TryParse(s, out num);
				int.TryParse(s2, out num2);
				bool flag3 = num > 0;
				if (flag3)
				{
					this.ModifyMessage(SSTEntryCode.ProductsMissing, 0, num.ToString(), "", "", "");
					flag = true;
				}
				bool flag4 = num2 > 0;
				if (flag4)
				{
					this.ModifyMessage(SSTEntryCode.ProductsMissing, 1, num2.ToString(), "", "", "");
					flag = true;
				}
				string a = this.parHandling.ReadPar(Definition.msSensorBlockedPhotogating1Path);
				string a2 = this.parHandling.ReadPar(Definition.msSensorBlockedPhotogating2Path);
				bool flag5 = a == "1";
				if (flag5)
				{
					this.PostMessage(SSTEntryCode.SensorBlockedPhotogating, 0);
					flag = true;
				}
				bool flag6 = a2 == "1";
				if (flag6)
				{
					this.PostMessage(SSTEntryCode.SensorBlockedPhotogating, 1);
					flag = true;
				}
				string a3 = this.parHandling.ReadPar(Definition.msSensorBlockedRejectCheck1Path);
				string a4 = this.parHandling.ReadPar(Definition.msSensorBlockedRejectCheck2Path);
				bool flag7 = a3 == "1";
				if (flag7)
				{
					this.PostMessage(SSTEntryCode.SensorBlockedRejectCheck, 0);
					flag = true;
				}
				bool flag8 = a4 == "1";
				if (flag8)
				{
					this.PostMessage(SSTEntryCode.SensorBlockedRejectCheck, 1);
					flag = true;
				}
				string a5 = this.parHandling.ReadPar(Definition.msConsistency1Path);
				string a6 = this.parHandling.ReadPar(Definition.msConsistency2Path);
				bool flag9 = a5 == "1";
				if (flag9)
				{
					string newParam = this.parHandling.ReadPar(Definition.msCountsPhotogating1Path);
					string newParam2 = this.parHandling.ReadPar(Definition.msCountsRejectCheck1Path);
					string newParam3 = this.parHandling.ReadPar(Definition.msCountsRejectVerify1Path);
					this.ModifyMessage(SSTEntryCode.ErrorConsistency, 0, newParam, newParam2, newParam3, "");
					flag = true;
				}
				bool flag10 = a6 == "1";
				if (flag10)
				{
					string newParam4 = this.parHandling.ReadPar(Definition.msCountsPhotogating2Path);
					string newParam5 = this.parHandling.ReadPar(Definition.msCountsRejectCheck2Path);
					string newParam6 = this.parHandling.ReadPar(Definition.msCountsRejectVerify2Path);
					this.ModifyMessage(SSTEntryCode.ErrorConsistency, 1, newParam4, newParam5, newParam6, "");
					flag = true;
				}
				string a7 = this.parHandling.ReadPar(Definition.msProductNotInspected1Path);
				string a8 = this.parHandling.ReadPar(Definition.msProductNotInspected2Path);
				bool flag11 = a7 == "1";
				if (flag11)
				{
					this.PostMessage(SSTEntryCode.ProductNotInspected, 0);
					flag = true;
				}
				bool flag12 = a8 == "1";
				if (flag12)
				{
					this.PostMessage(SSTEntryCode.ProductNotInspected, 1);
					flag = true;
				}
			}
			bool flag13 = flag;
			if (flag13)
			{
				this.machineState.ErrorsBeltStopForSignalLamp = true;
			}
			return flag;
		}

		// Token: 0x06001A7D RID: 6781 RVA: 0x0009CBB0 File Offset: 0x0009ADB0
		private bool CheckForTooManyRejects()
		{
			bool result = false;
			bool flag = this.parHandling.ReadPar(Definition.rejecterTooManyRejectsErrorPath) == "1";
			if (flag)
			{
				this.PostMessage(SSTEntryCode.TooManyRejects);
				this.machineState.ErrorsBeltStopForSignalLamp = true;
				result = true;
			}
			return result;
		}

		// Token: 0x06001A7E RID: 6782 RVA: 0x0009CC00 File Offset: 0x0009AE00
		private bool CheckInsightNetConnection()
		{
			bool result = false;
			DateTime dateTime = this.timeOfCheckStart;
			bool flag = true;
			if (flag)
			{
				bool flag2 = (int)(DateTime.Now - this.timeOfCheckStart).TotalSeconds >= this.secondsToGetInsightConnection;
				if (flag2)
				{
					bool flag3 = this.parHandling.ReadPar(Definition.insightNetAvailablePath) == "1";
					if (flag3)
					{
						bool flag4 = this.sstServer != null && this.sstServer.NConnectedClients <= 0;
						if (flag4)
						{
							bool flag5 = this.nCurrentInsightNetFaults < this.maxInsightNetFaults;
							if (flag5)
							{
								this.nCurrentInsightNetFaults++;
							}
							else
							{
								this.PostMessage(SSTEntryCode.InsightNetConnection);
								this.machineState.ErrorsBeltStopForSignalLamp = true;
								result = true;
							}
						}
						else
						{
							this.nCurrentInsightNetFaults = 0;
						}
					}
					else
					{
						this.nCurrentInsightNetFaults = 0;
					}
				}
			}
			return result;
		}

		// Token: 0x06001A7F RID: 6783 RVA: 0x0009CCEC File Offset: 0x0009AEEC
		private bool CheckDiskSpaceLow()
		{
			bool result = false;
			string text = "D:\\";
			bool flag = Directory.Exists(text) && Disk.GetTotalFreeSpace(text) < (long)(Definition.diskSpaceMinFreeMB * 1024 * 1024);
			if (flag)
			{
				this.PostMessage(SSTEntryCode.DiskSpaceLow);
				this.machineState.ErrorsBeltStopForSignalLamp = true;
				result = true;
			}
			return result;
		}

		// Token: 0x06001A80 RID: 6784 RVA: 0x0009CD4C File Offset: 0x0009AF4C
		private void PostUncommittedErrors()
		{
			List<string[]> list = new List<string[]>();
			this.dbConnection.GetUncomittedErrors(list);
			for (int i = 0; i < list.Count; i++)
			{
				SSTLogbookEntry sstlogbookEntry = new SSTLogbookEntry(list[i].ToList<string>());
				sstlogbookEntry.CreateLogbookText(this.dbConnection);
				switch (sstlogbookEntry.EntryCode)
				{
				case SSTEntryCode.DetectorComm:
					this.machineErrors.Detector = this.errorHandling.DetectorErrorEnabled;
					break;
				case SSTEntryCode.CalibrationFault:
					this.machineErrors.Calibration = this.errorHandling.DetectorErrorEnabled;
					break;
				case SSTEntryCode.XRayComm:
				case SSTEntryCode.XRayFault:
					this.machineErrors.XRay = this.errorHandling.XrayErrorEnabled;
					break;
				case SSTEntryCode.ProcessingDelay:
					this.machineErrors.ImageDelay = true;
					break;
				case SSTEntryCode.MemoryUsage:
					this.machineErrors.MemoryUsage = true;
					break;
				case SSTEntryCode.MotorError:
					this.machineErrors.Motor = this.errorHandling.EngineErrorEnabled;
					break;
				case SSTEntryCode.LowAirPressure:
					this.machineErrors.LowAirPressure = this.errorHandling.LowAirPressureErrorEnabled;
					break;
				case SSTEntryCode.CoolingError:
					this.machineErrors.Cooling = this.errorHandling.CoolingErrorEnabled;
					break;
				case SSTEntryCode.BinFull:
				{
					bool flag = sstlogbookEntry.Param1 == 1;
					if (flag)
					{
						this.machineErrors.BinFull1 = this.errorHandling.BinFullEnabled;
					}
					else
					{
						bool flag2 = sstlogbookEntry.Param1 == 2;
						if (flag2)
						{
							this.machineErrors.BinFull2 = this.errorHandling.BinFullEnabled;
						}
						else
						{
							bool flag3 = sstlogbookEntry.Param1 == 3;
							if (flag3)
							{
								this.machineErrors.BinFull3 = this.errorHandling.BinFullEnabled;
							}
							else
							{
								bool flag4 = sstlogbookEntry.Param1 == 4;
								if (flag4)
								{
									this.machineErrors.BinFull4 = this.errorHandling.BinFullEnabled;
								}
							}
						}
					}
					break;
				}
				case SSTEntryCode.ParamComm:
					this.machineErrors.ParamPortCommError = true;
					break;
				case SSTEntryCode.ImageComm:
					this.machineErrors.ImagePortCommError = true;
					break;
				case SSTEntryCode.IOComm:
					this.machineErrors.IOComm = true;
					break;
				case SSTEntryCode.IOController:
					this.machineErrors.WAGOCode = true;
					break;
				case SSTEntryCode.RejectVerify:
				{
					bool flag5 = sstlogbookEntry.Param1 == 1;
					if (flag5)
					{
						this.machineErrors.MissingObjects1 = this.errorHandling.MissingObjectsEnabled;
					}
					else
					{
						bool flag6 = sstlogbookEntry.Param1 == 2;
						if (flag6)
						{
							this.machineErrors.MissingObjects2 = this.errorHandling.MissingObjectsEnabled;
						}
						else
						{
							bool flag7 = sstlogbookEntry.Param1 == 3;
							if (flag7)
							{
								this.machineErrors.MissingObjects3 = this.errorHandling.MissingObjectsEnabled;
							}
							else
							{
								bool flag8 = sstlogbookEntry.Param1 == 4;
								if (flag8)
								{
									this.machineErrors.MissingObjects4 = this.errorHandling.MissingObjectsEnabled;
								}
							}
						}
					}
					break;
				}
				case SSTEntryCode.XRayLamp:
					this.machineErrors.XRayOnLamp = this.errorHandling.XrayLampErrorEnabled;
					break;
				case SSTEntryCode.Temperature:
				{
					bool flag9 = sstlogbookEntry.Param1 == 0;
					if (flag9)
					{
						this.machineErrors.Temperature0 = this.errorHandling.TemperatureErrorEnabled;
					}
					else
					{
						this.machineErrors.Temperature1 = this.errorHandling.TemperatureErrorEnabled;
					}
					break;
				}
				case SSTEntryCode.Voltage:
				{
					bool flag10 = sstlogbookEntry.Param1 == 0;
					if (flag10)
					{
						this.machineErrors.Voltage12V = this.errorHandling.VoltageErrorEnabled;
					}
					else
					{
						this.machineErrors.Voltage24V = this.errorHandling.VoltageErrorEnabled;
					}
					break;
				}
				case SSTEntryCode.ImageProcPC:
					this.machineErrors.ImageProcPcOffline = true;
					break;
				case SSTEntryCode.LightBarrier:
				{
					bool flag11 = sstlogbookEntry.Param1 == 1;
					if (flag11)
					{
						this.machineErrors.LightBarrier1 = this.errorHandling.MissingObjectsEnabled;
					}
					else
					{
						bool flag12 = sstlogbookEntry.Param1 == 2;
						if (flag12)
						{
							this.machineErrors.LightBarrier2 = this.errorHandling.MissingObjectsEnabled;
						}
						else
						{
							bool flag13 = sstlogbookEntry.Param1 == 3;
							if (flag13)
							{
								this.machineErrors.LightBarrier3 = this.errorHandling.MissingObjectsEnabled;
							}
							else
							{
								bool flag14 = sstlogbookEntry.Param1 == 4;
								if (flag14)
								{
									this.machineErrors.LightBarrier4 = this.errorHandling.MissingObjectsEnabled;
								}
							}
						}
					}
					break;
				}
				case SSTEntryCode.SVTComm:
					this.machineErrors.SVTComm = true;
					break;
				case SSTEntryCode.SVTError:
					this.machineErrors.SVTStatus = true;
					break;
				case SSTEntryCode.XRayMotor:
					this.machineErrors.XrayMotorError = true;
					break;
				case SSTEntryCode.AuditFailedError:
				{
					string a = null;
					this.dbConnection.GetAuditFailedError(ref a);
					this.machineErrors.AuditFailed = (a == "1");
					break;
				}
				case SSTEntryCode.ProductsMissing:
				case SSTEntryCode.SensorBlockedPhotogating:
				case SSTEntryCode.SensorBlockedRejectCheck:
				case SSTEntryCode.ErrorConsistency:
				case SSTEntryCode.ProductNotInspected:
					this.machineErrors.HighLevelSensorsError = this.errorHandling.HighLevelSensorsEnabled;
					break;
				case SSTEntryCode.BinMissing:
					this.machineErrors.BinMissingError = this.errorHandling.BinMissingEnabled;
					break;
				case SSTEntryCode.TooManyRejects:
					this.machineErrors.TooManyRejectsError = this.errorHandling.TooManyRejectsEnabled;
					break;
				case SSTEntryCode.InsightNetConnection:
					this.machineErrors.InsightNetConnectionError = this.errorHandling.InsightNetConnectionErrorEnabled;
					break;
				case SSTEntryCode.DiskSpaceLow:
					this.machineErrors.DiskSpaceLowError = this.errorHandling.DiskSpaceLowErrorEnabled;
					break;
				case SSTEntryCode.MeanValueHELEforDual:
					this.machineErrors.HELEDifferenceforDual = true;
					break;
				case SSTEntryCode.ErrorXRayOperatingHours:
					this.machineErrors.ErrorXRayOperatingHours = true;
					break;
				case SSTEntryCode.LimitsError:
					this.machineErrors.MonitoringError = true;
					break;
				}
				this.messageSystem.PostRegisteredMessage(sstlogbookEntry, false);
			}
		}

		// Token: 0x06001A81 RID: 6785 RVA: 0x0009D33C File Offset: 0x0009B53C
		public void DelayAirPressureChecking()
		{
			this.checkAirPressure = false;
			Thread thread = new Thread(delegate()
			{
				Thread.Sleep(5000);
				this.checkAirPressure = true;
			});
			thread.Start();
		}

		// Token: 0x06001A82 RID: 6786 RVA: 0x0009D36C File Offset: 0x0009B56C
		private bool CheckInOutletErrors()
		{
			bool result = false;
			MachineType machineType = MachineType.MachineTypeUndefined;
			this.dbConnection.GetMachineType(ref machineType);
			bool flag = machineType != MachineType.MachineTypeUndefined && machineType > MachineType.RayconXRS;
			if (flag)
			{
				bool flag2 = this.parHandling.ReadPar(Definition.inletBlockedPath) == "1";
				if (flag2)
				{
					this.PostMessage(SSTEntryCode.InOutletSensorBlocked, 0);
					result = true;
				}
				bool flag3 = this.parHandling.ReadPar(Definition.outletBlockedPath) == "1";
				if (flag3)
				{
					this.PostMessage(SSTEntryCode.InOutletSensorBlocked, 1);
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06001A83 RID: 6787 RVA: 0x0009D400 File Offset: 0x0009B600
		public void StartCheckingXRayOperationgHours()
		{
			bool flag = !this.checkxrayoperatinghours;
			if (flag)
			{
				this.checkxrayoperatinghours = true;
				this.thxrayoh = new Thread(new ThreadStart(this.CheckingXRayOperatingHours));
				this.thxrayoh.Name = "XRay Operating hours error";
				this.thxrayoh.Start();
			}
		}

		// Token: 0x06001A84 RID: 6788 RVA: 0x0009D458 File Offset: 0x0009B658
		public void StopCheckingXRayOperatingHours()
		{
			this.checkxrayoperatinghours = false;
			bool flag = this.thxrayoh != null;
			if (flag)
			{
				this.thxrayoh.Abort();
				this.thxrayoh = null;
			}
		}

		// Token: 0x06001A85 RID: 6789 RVA: 0x0009D490 File Offset: 0x0009B690
		private void CheckingXRayOperatingHours()
		{
			while (this.dbConnection.Connection == null)
			{
				bool flag = Application.Current == null;
				if (flag)
				{
					return;
				}
				Thread.Sleep(this.surveyPeriod);
			}
			this.xrayoperatinghours = new XRayOperatingHours(this.dbConnection, this.parHandling, this.dbDatalogs);
			int num = 0;
			string s = "";
			while (this.checkxrayoperatinghours && Application.Current != null)
			{
				this.xrayoperatinghours.SetXrayOperatingHoursDB();
				this.dbConnection.GetXRayOpHoursLimit(ref num);
				this.dbConnection.GetXRayOnTimeInternal(ref s);
				double.TryParse(s, NumberStyles.Number, new CultureInfo("en-US"), out this.actualOperatingHours);
				XRayOnProcessing.SetRealLimitOpHoursXRaySource(this.dbConnection, this.parHandling, num);
				bool flag2 = this.actualOperatingHours > 0.0 && num > 0 && num < 99999 && this.parHandling.ReadPar(XRayOnProcessing.xrayEnablePath) == "1";
				if (flag2)
				{
					bool flag3 = this.actualOperatingHours < (double)num;
					if (flag3)
					{
						this.actualBorderOperatingHours = this.xrayoperatinghours.GetAdjustedLimitForXrayOperatingHours((double)num);
						this.dbConnection.SetDynXRayOpHoursLimit(Convert.ToInt32(this.actualBorderOperatingHours));
						this.parHandling.WriteValue(Definition.dynamiclimitXraySourceOperatingHoursPath, Convert.ToString(this.actualBorderOperatingHours));
					}
					else
					{
						this.dbConnection.GetDynXRayOpHoursLimit(ref this.actualBorderOperatingHours);
					}
					bool flag4 = this.actualBorderOperatingHours > 0;
					if (flag4)
					{
						bool flag5 = this.actualOperatingHours > 0.9 * (double)this.actualBorderOperatingHours && this.actualOperatingHours < (double)this.actualBorderOperatingHours && !this.checkWarningOpHoursXRay;
						if (flag5)
						{
							this.checkWarningOpHoursXRay = true;
							this.timeStampForErrorOPHoursXRaySource = DateTime.Now;
						}
						else
						{
							bool flag6 = this.actualOperatingHours > (double)this.actualBorderOperatingHours && !this.checkErrorOpHoursXRay;
							if (flag6)
							{
								this.checkWarningOpHoursXRay = false;
								this.checkErrorOpHoursXRay = true;
								this.timeStampForErrorOPHoursXRaySource = DateTime.Now;
							}
						}
					}
				}
				Thread.Sleep(this.checkingXRaySourceOHPeriod);
			}
		}

		// Token: 0x06001A86 RID: 6790 RVA: 0x0009D6CC File Offset: 0x0009B8CC
		private void SetMessageErrorXRayOperatingHours()
		{
			int num = 0;
			this.dbConnection.GetXRayOpHoursLimit(ref num);
			DateTime now = DateTime.Now;
			bool flag = num > 0 && num < 99999 && this.parHandling.ReadPar(XRayOnProcessing.xrayEnablePath) == "0";
			if (flag)
			{
				bool flag2 = this.checkWarningOpHoursXRay && now > this.timeStampForErrorOPHoursXRaySource;
				if (flag2)
				{
					this.timeStampForErrorOPHoursXRaySource = this.timeStampForErrorOPHoursXRaySource.AddHours(24.0);
					this.PostMessage(SSTEntryCode.WarningXRayOperatingHours, 0, Convert.ToString(this.actualBorderOperatingHours), Convert.ToString(Convert.ToInt32(this.actualOperatingHours)), "", "");
				}
				bool flag3 = this.checkErrorOpHoursXRay && now > this.timeStampForErrorOPHoursXRaySource;
				if (flag3)
				{
					this.timeStampForErrorOPHoursXRaySource = this.timeStampForErrorOPHoursXRaySource.AddHours(24.0);
					this.PostMessage(SSTEntryCode.ErrorXRayOperatingHours, 0, Convert.ToString(this.actualBorderOperatingHours), Convert.ToString(Convert.ToInt32(this.actualOperatingHours)), "", "");
				}
			}
		}

		// Token: 0x06001A87 RID: 6791 RVA: 0x0009D7F0 File Offset: 0x0009B9F0
		public void SetTestcardSimLogbookMsg(string vaold, string vanew, string glold, string glnew, string vaCompl, string glCompl, bool isNotInitialSens, SSTEntryCode messagelevel, byte compliance)
		{
			if (isNotInitialSens)
			{
				string param = vaold + " --> " + vanew;
				string param2 = glold + " --> " + glnew;
				this.PostMessage(messagelevel, compliance, param, param2, vaCompl, glCompl);
			}
			else
			{
				this.PostMessage(messagelevel, 0, vanew, glnew, "", "");
			}
		}

		// Token: 0x06001A88 RID: 6792 RVA: 0x0009D84C File Offset: 0x0009BA4C
		public void PostMessageMonitoring(SSTEntryCode code, byte num, string res1, string res2, string limit, string val)
		{
			this.ModifyMessageMonitoring(code, num, res1, res2, limit, val);
			bool flag = code == SSTEntryCode.LimitsError;
			if (flag)
			{
				this.machineErrors.MonitoringError = true;
			}
		}

		// Token: 0x06001A89 RID: 6793 RVA: 0x0009D882 File Offset: 0x0009BA82
		internal void ResetMonitoring()
		{
			this.machineErrors.MonitoringError = false;
		}

		// Token: 0x06001A8A RID: 6794 RVA: 0x0009D894 File Offset: 0x0009BA94
		private void ModifyMessageMonitoring(SSTEntryCode code, byte param1, string newParam2, string newParam3, string newParam4, string newParam5)
		{
			bool flag = Application.Current != null;
			if (flag)
			{
				Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(delegate()
				{
					SSTLogbookEntry sstlogbookEntry = null;
					bool flag2 = !this.messageSystem.EntryExisiting(code, param1, newParam2, newParam3, newParam4, newParam5);
					if (flag2)
					{
						sstlogbookEntry = LogbookManager.AddEntry(code, param1, newParam2, newParam3, newParam4, newParam5);
						bool flag3 = sstlogbookEntry.EntryCode == SSTEntryCode.RejectVerify;
						if (flag3)
						{
							this.messageSystem.NotifyReject();
						}
					}
					bool flag4 = !this.messageSystem.ModifyMessageMonitoring(code, param1, newParam2, newParam3, newParam4, newParam5);
					if (flag4)
					{
						bool flag5 = sstlogbookEntry != null;
						if (flag5)
						{
							this.messageSystem.PostRegisteredMessage(sstlogbookEntry);
						}
					}
				}));
			}
		}

		// Token: 0x06001A8B RID: 6795 RVA: 0x0009D908 File Offset: 0x0009BB08
		private void CheckBgMonContaminantOnConveyor()
		{
			bool flag = this.parHandling.ReadPar(Definition.detectorMonitoringActive) == "1";
			if (flag)
			{
				bool flag2 = this.parHandling.ReadPar(Definition.detectorMonitoringContaminantDetected) == "1";
				bool flag3 = flag2;
				if (flag3)
				{
					string newParam = this.parHandling.ReadPar(Definition.detectorMonitoringPixelCount);
					string newParam2 = this.parHandling.ReadPar(Definition.detectorMonitoringPixelCountBorder);
					string text = "";
					this.dbDatalogs.GetDataLogsSettingsEntry("MessageLevel", ref text);
					bool flag4 = text.Equals("Info");
					SSTEntryCode code;
					if (flag4)
					{
						code = SSTEntryCode.BackgroundMonitoringInfo;
					}
					else
					{
						bool flag5 = text.Equals("Warning");
						if (flag5)
						{
							code = SSTEntryCode.BackgroundMonitoringWarning;
						}
						else
						{
							code = SSTEntryCode.BackgroundMonitoringError;
						}
					}
					this.ModifyMessage(code, 2, "", "", newParam2, newParam);
					this.parHandling.WriteValue(Definition.detectorMonitoringContaminantDetected, "0");
					this.parHandling.WriteValue(Definition.detectorMonitoringPixelCount, "0");
				}
			}
		}

		// Token: 0x06001A8C RID: 6796 RVA: 0x0009DA1C File Offset: 0x0009BC1C
		private bool CheckAIDeploymentErrors()
		{
			bool flag = this.AIModelPortError();
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				bool flag2 = false;
				bool flag3 = this.parHandling.ReadPar(Definition.anomalyDetectionDeploymentErrorCode) != "";
				if (flag3)
				{
					int num = Convert.ToInt32(this.parHandling.ReadPar(Definition.anomalyDetectionDeploymentErrorCode));
					bool flag4 = num >= 1 && num <= 6;
					if (flag4)
					{
						flag2 = true;
						this.parHandling.WriteValue(Definition.anomalyDetectionDeploymentErrorCode, "0");
					}
					bool flag5 = num == 1;
					if (flag5)
					{
						this.PostMessage(SSTEntryCode.AIDeploymentError, 1);
						this.parHandling.WriteValue(Definition.anomalyDetectionTrainEnabled, "0");
					}
					else
					{
						bool flag6 = num >= 2 && num <= 3;
						if (flag6)
						{
							this.PostMessage(SSTEntryCode.AIDeploymentError, 2);
							this.parHandling.WriteValue(Definition.anomalyDetectionInferenceEnabled, "0");
						}
						else
						{
							bool flag7 = num == 4;
							if (flag7)
							{
								this.PostMessage(SSTEntryCode.AIDeploymentError, 3);
								this.parHandling.WriteValue(Definition.anomalyDetectionInferenceEnabled, "0");
							}
							else
							{
								bool flag8 = num == 5;
								if (flag8)
								{
									this.PostMessage(SSTEntryCode.AIDeploymentError, 4);
									this.parHandling.WriteValue(Definition.anomalyDetectionTrainEnabled, "0");
								}
							}
						}
					}
					bool flag9 = this.AIService != null && num >= 2 && num <= 4;
					if (flag9)
					{
						this.AIService.Comp19to30OnOff(false);
					}
				}
				result = flag2;
			}
			return result;
		}

		// Token: 0x06001A8D RID: 6797 RVA: 0x0009DBA4 File Offset: 0x0009BDA4
		private bool CheckAIInferenceErrors()
		{
			bool result = false;
			bool flag = this.parHandling.ReadPar(Definition.anomalyDetectionInferenceErrorCode) != "";
			if (flag)
			{
				int num = Convert.ToInt32(this.parHandling.ReadPar(Definition.anomalyDetectionInferenceErrorCode));
				bool flag2 = num == 1;
				if (flag2)
				{
					result = true;
					this.PostMessage(SSTEntryCode.AIInferenceError, 0);
					this.parHandling.WriteValue(Definition.anomalyDetectionInferenceErrorCode, "0");
				}
			}
			bool flag3 = this.parHandling.ReadPar(Definition.objectDetectionInferenceErrorCode) != "";
			if (flag3)
			{
				int num2 = Convert.ToInt32(this.parHandling.ReadPar(Definition.objectDetectionInferenceErrorCode));
				bool flag4 = num2 == 2;
				if (flag4)
				{
					result = true;
					this.PostMessage(SSTEntryCode.AIInferenceError, 1);
					this.parHandling.WriteValue(Definition.objectDetectionInferenceErrorCode, "0");
				}
			}
			return result;
		}

		// Token: 0x06001A8E RID: 6798 RVA: 0x0009DC88 File Offset: 0x0009BE88
		private bool CheckConnectionExternalStorageLocation()
		{
			DateTime dateTime = this.timeOfCheckStart;
			bool flag = true;
			if (flag)
			{
				bool flag2 = (int)(DateTime.Now - this.timeOfCheckStart).TotalSeconds >= 60;
				if (flag2)
				{
					bool flag3 = (this.dbConnection.GetLocationNetworkSavingPng() == "1" || this.dbConnection.GetLocationNetworkSavingPng() == "3") && !Directory.Exists(this.dbConnection.GetNetworkPathSavingPng() + "\\");
					if (flag3)
					{
						string networkPathSavingPng = this.dbConnection.GetNetworkPathSavingPng();
						List<string> list = new List<string>
						{
							"",
							"",
							""
						};
						this.slicePathIntoArrayForLogbook(networkPathSavingPng, ref list);
						this.PostMessage(SSTEntryCode.NoConnectionToExternalStorageLoc, 0, list[0], list[1], list[2], "");
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06001A8F RID: 6799 RVA: 0x0009DD9C File Offset: 0x0009BF9C
		public void slicePathIntoArrayForLogbook(string path, ref List<string> pathpieces)
		{
			pathpieces = new List<string>
			{
				"",
				"",
				""
			};
			int num = 0;
			bool flag = path.Length > 0;
			if (flag)
			{
				for (int i = 0; i < 3; i++)
				{
					int num2 = Math.Min(40, path.Length - num);
					pathpieces[i] = path.Substring(num, num2);
					num += num2;
				}
			}
		}

		// Token: 0x06001A90 RID: 6800 RVA: 0x0009DE24 File Offset: 0x0009C024
		private string getMotorNumberHYRework()
		{
			string text = "";
			bool flag = this.parHandling.ReadPar(ErrorSurvey.ioRoot + ErrorSurvey.ioErrorMotor1_2_HYRework.IOPath + "/Value") == "0";
			if (flag)
			{
				text = " MA001/MA002";
			}
			bool flag2 = this.parHandling.ReadPar(ErrorSurvey.ioRoot + ErrorSurvey.ioErrorMotor3_HYRework.IOPath + "/Value") == "0";
			if (flag2)
			{
				text += " MA003";
			}
			bool flag3 = this.parHandling.ReadPar(ErrorSurvey.ioRoot + ErrorSurvey.ioErrorMotor4_HYRework.IOPath + "/Value") == "0";
			if (flag3)
			{
				text += " MA004";
			}
			bool flag4 = this.parHandling.ReadPar(ErrorSurvey.ioRoot + ErrorSurvey.ioErrorMotor5_HYRework.IOPath + "/Value") == "0";
			if (flag4)
			{
				text += " MA005";
			}
			bool flag5 = this.parHandling.ReadPar(ErrorSurvey.ioRoot + ErrorSurvey.ioErrorMotor6_HYRework.IOPath + "/Value") == "0";
			if (flag5)
			{
				text += " MA006";
			}
			bool flag6 = this.parHandling.ReadPar(ErrorSurvey.ioRoot + ErrorSurvey.ioErrorMotor7_HYRework.IOPath + "/Value") == "0";
			if (flag6)
			{
				text += " MA007";
			}
			return text;
		}

		// Token: 0x0400111D RID: 4381
		public static readonly InOut[] IOsSafetySwitchesOld = new InOut[]
		{
			new InOut("DI_16", "parItemServiceDoor", 0),
			new InOut("DI_17", "parItemDetector", 1),
			new InOut("DI_18", "parItemXRaySource", 2),
			new InOut("DI_19", "parItemPanelTop", 3),
			new InOut("DI_20", "parItemPanelLeft", 4),
			new InOut("DI_21", "parItemPanelRight", 5),
			new InOut("DI_22", "parItemCleaningDoorLeft", 6),
			new InOut("DI_23", "parItemCleaningDoorRight", 7),
			new InOut("DI_24", "parItemShieldRHTopBack", 8),
			new InOut("DI_25", "parItemShieldLHTopBack", 9),
			new InOut("DI_26", "parItemShieldRHBotBack", 10),
			new InOut("DI_27", "parItemShieldLHBotBack", 11),
			new InOut("DI_28", "parItemShieldRHTopFront", 12),
			new InOut("DI_29", "parItemShieldLHTopFront", 13),
			new InOut("DI_30", "parItemShieldRHBotFront", 14),
			new InOut("DI_31", "parItemShieldLHBotFront", 15)
		};

		// Token: 0x0400111E RID: 4382
		public static readonly InOut[] IOsSafetySwitchesNew = new InOut[]
		{
			new InOut("DI_08", "parItemServiceDoor", 0),
			new InOut("DI_09", "parItemDetector", 1),
			new InOut("DI_10", "parItemXRaySource", 2),
			new InOut("DI_11", "parItemPanelTop", 3),
			new InOut("DI_12", "parItemPanelLeft", 4),
			new InOut("DI_13", "parItemPanelRight", 5),
			new InOut("DI_14", "parItemCleaningDoorLeft", 6),
			new InOut("DI_15", "parItemCleaningDoorRight", 7),
			new InOut("DI_16", "parItemShieldRHTopBack", 8),
			new InOut("DI_17", "parItemShieldLHTopBack", 9),
			new InOut("DI_18", "parItemShieldRHBotBack", 10),
			new InOut("DI_19", "parItemShieldLHBotBack", 11),
			new InOut("DI_20", "parItemShieldRHTopFront", 12),
			new InOut("DI_21", "parItemShieldLHTopFront", 13),
			new InOut("DI_22", "parItemShieldRHBotFront", 14),
			new InOut("DI_23", "parItemShieldLHBotFront", 15)
		};

		// Token: 0x0400111F RID: 4383
		public static readonly InOut[] IOsSafetySwitchesNewBulk = new InOut[]
		{
			new InOut("DI_08", "parItemServiceDoor", 0),
			new InOut("DI_09", "parItemDetector", 1),
			new InOut("DI_10", "parItemXRaySource", 2),
			new InOut("DI_11", "parItemPanelTopRH", 17),
			new InOut("DI_12", "parItemPanelTopLH", 18),
			new InOut("DI_13", "parItemHopperWasteProduct", 19),
			new InOut("DI_14", "parItemHopperGoodProduct", 20),
			new InOut("DI_15", "parItemHopperInfeed", 21),
			new InOut("DI_16", "parItemShieldBottom", 22),
			new InOut("DI_17", "parItemCleaningDoorRH", 23),
			new InOut("DI_18", "parItemCleaningDoorBackRH", 24),
			new InOut("DI_19", "parItemCleaningDoorFrontRH", 25),
			new InOut("DI_20", "parItemCleaningDoorLH", 26),
			new InOut("DI_21", "parItemCleaningDoorBackLH", 27),
			new InOut("DI_22", "parItemCleaningDoorFrontLH", 28),
			new InOut("DI_23", "parItemCleaningDoorFront", 29)
		};

		// Token: 0x04001120 RID: 4384
		public static readonly InOut[] IOsSafetySwitchesEntry = new InOut[]
		{
			new InOut("DI_16", "parItemServiceDoor", 0),
			new InOut("DI_17", "parItemDetector", 1),
			new InOut("DI_18", "parItemXRaySource", 2),
			new InOut("DI_19", "parItemPanelTop", 3),
			new InOut("DI_20", "parItemShieldRHTopBack", 8),
			new InOut("DI_21", "parItemShieldLHTopBack", 9),
			new InOut("DI_22", "parItemShieldRHTopFront", 12),
			new InOut("DI_23", "parItemShieldLHTopFront", 13)
		};

		// Token: 0x04001121 RID: 4385
		public static readonly InOut[] IOsSafetySwitchesDPlus = new InOut[]
		{
			new InOut("DI_16", "parItemServiceDoor", 0),
			new InOut("DI_17", "parItemDetector", 1),
			new InOut("DI_18", "parItemXRaySource", 2),
			new InOut("DI_19", "parItemPanelTop", 3),
			new InOut("DI_20", "parItemPanelLeft", 4),
			new InOut("DI_21", "parItemPanelRight", 5),
			new InOut("DI_22", "parItemCleaningDoorLeft", 6),
			new InOut("DI_23", "parItemCleaningDoorRight", 7),
			new InOut("DI_24", "parItemShieldRHTopBack", 8),
			new InOut("DI_25", "parItemShieldLHTopBack", 9),
			new InOut("DI_26", "parItemShieldRHBotBack", 10),
			new InOut("DI_27", "parItemShieldLHBotBack", 11),
			new InOut("DI_28", "parItemShieldRHTopFront", 12),
			new InOut("DI_29", "parItemShieldLHTopFront", 13),
			new InOut("DI_30", "parItemShieldRHBotFront", 14),
			new InOut("DI_31", "parItemShieldLHBotFront", 15)
		};

		// Token: 0x04001122 RID: 4386
		public static readonly InOut[] IOsSafetySwitchesDPlusBulk = new InOut[]
		{
			new InOut("DI_16", "parItemServiceDoor", 0),
			new InOut("DI_17", "parItemDetector", 1),
			new InOut("DI_18", "parItemXRaySource", 2),
			new InOut("DI_19", "parItemPanelTop", 3),
			new InOut("DI_20", "parItemPanelLeft", 4),
			new InOut("DI_21", "parItemSlidingPlate", 33),
			new InOut("DI_22", "parItemHopperInfeed", 21),
			new InOut("DI_23", "parItemServiceDoorRejectionFront", 30),
			new InOut("DI_24", "parItemShieldRHTopBack", 8),
			new InOut("DI_25", "parItemShieldLHTopBack", 9),
			new InOut("DI_26", "parItemServiceDoorRejectionBack", 31),
			new InOut("DI_27", "parItemServiceDoorRejectionMiddle", 32),
			new InOut("DI_28", "parItemShieldRHTopFront", 12),
			new InOut("DI_29", "parItemShieldLHTopFront", 13),
			new InOut("DI_30", "parItemHopperGoodProduct", 20),
			new InOut("DI_31", "parItemHopperWasteProduct", 19)
		};

		// Token: 0x04001123 RID: 4387
		public static readonly InOut[] IOsSafetySwitchesDPlus1Module = new InOut[]
		{
			new InOut("DI_08", "parItemServiceDoor", 0),
			new InOut("DI_09", "parItemDetector", 1),
			new InOut("DI_10", "parItemXRaySource", 2),
			new InOut("DI_11", "parConveyorDoor", 34),
			new InOut("DI_12", "parShieldingRightBack", 35),
			new InOut("DI_13", "parShieldingLeftBack", 36),
			new InOut("DI_14", "parShieldingRightFront", 37),
			new InOut("DI_15", "parShieldingLeftFront", 38),
			new InOut("DI_27", "txtHardwareBrigeDPlus1Module", 39),
			new InOut("DI_28", "txtHardwareBrigeDPlus1Module", 40),
			new InOut("DI_29", "txtHardwareBrigeDPlus1Module", 41),
			new InOut("DI_30", "txtHardwareBrigeDPlus1Module", 42),
			new InOut("DI_31", "txtHardwareBrigeDPlus1Module", 43)
		};

		// Token: 0x04001124 RID: 4388
		public static readonly InOut[] IOsSafetySwitchesDPlus1ModuleBulk = new InOut[]
		{
			new InOut("DI_08", "parItemServiceDoor", 0),
			new InOut("DI_09", "parItemDetector", 1),
			new InOut("DI_10", "parItemXRaySource", 2),
			new InOut("DI_11", "parConveyorDoor", 34),
			new InOut("DI_12", "parShieldingRightBack", 35),
			new InOut("DI_13", "parShieldingLeftBack", 36),
			new InOut("DI_14", "parShieldingRightFront", 37),
			new InOut("DI_15", "parItemSlidingPlate", 33),
			new InOut("DI_27", "parItemHopperInfeed", 21),
			new InOut("DI_28", "parItemServiceDoorRejectionFront", 30),
			new InOut("DI_29", "parItemServiceDoorRejectionBack", 31),
			new InOut("DI_30", "parItemHopperGoodProduct", 20),
			new InOut("DI_31", "parItemHopperWasteProduct", 19)
		};

		// Token: 0x04001125 RID: 4389
		public static readonly InOut[] IOsSafetySwitchesDPlus1ModuleHYRework = new InOut[]
		{
			new InOut("DI_08", "parItemServiceDoor", 0),
			new InOut("DI_09", "parItemDetector", 1),
			new InOut("DI_10", "parItemXRaySource", 2),
			new InOut("DI_11", "parConveyorDoor", 34),
			new InOut("DI_13", "parItemPanelLeft", 4),
			new InOut("DI_12", "parItemPanelRight", 5),
			new InOut("DI_14", "parItemConveyorBackNotDocked", 44),
			new InOut("DI_15", "parItemSlidingPlate", 33),
			new InOut("DI_25", "parItemRejecterUnitNotAvailable", 45),
			new InOut("DI_26", "parItemRejecterUnitCoverTop", 46),
			new InOut("DI_27", "parItemRejecterUnitCoverFront", 47)
		};

		// Token: 0x04001126 RID: 4390
		public static readonly string ioRoot = "/hardware/ios/";

		// Token: 0x04001127 RID: 4391
		public static readonly InOut ioVoltage12V = new InOut("AI_04", "parItemVoltage12V");

		// Token: 0x04001128 RID: 4392
		public static readonly InOut ioVoltage24V = new InOut("AI_05", "parItemVoltage24V");

		// Token: 0x04001129 RID: 4393
		public static readonly InOut ioTempSensor0Old = new InOut("AI_06", "parItemTempSensor0");

		// Token: 0x0400112A RID: 4394
		public static readonly InOut ioTempSensor1Old = new InOut("AI_07", "parItemTempSensor1");

		// Token: 0x0400112B RID: 4395
		public static readonly InOut ioLowAirPressureOld = new InOut("DI_32", "parItemLowAirPressure");

		// Token: 0x0400112C RID: 4396
		public static readonly InOut ioEmergencyStop = new InOut("DI_01", "parItemEmergencyStopPressed");

		// Token: 0x0400112D RID: 4397
		public static readonly InOut ioEmergencyStopExternal = new InOut("DI_23", "parItemExternalEmergencyStopPressed");

		// Token: 0x0400112E RID: 4398
		public static readonly InOut ioErrorCooling = new InOut("DI_04", "parItemErrorCooling");

		// Token: 0x0400112F RID: 4399
		public static readonly InOut ioErrorMotor = new InOut("DI_05", "parItemErrorEngine");

		// Token: 0x04001130 RID: 4400
		public static readonly InOut ioErrorMotor1_2_HYRework = new InOut("DI_05", "parItemErrorEngine");

		// Token: 0x04001131 RID: 4401
		public static readonly InOut ioErrorMotor3_HYRework = new InOut("DI_16", "parItemErrorEngine");

		// Token: 0x04001132 RID: 4402
		public static readonly InOut ioErrorMotor4_HYRework = new InOut("DI_17", "parItemErrorEngine");

		// Token: 0x04001133 RID: 4403
		public static readonly InOut ioErrorMotor5_HYRework = new InOut("DI_18", "parItemErrorEngine");

		// Token: 0x04001134 RID: 4404
		public static readonly InOut ioErrorMotor6_HYRework = new InOut("DI_19", "parItemErrorEngine");

		// Token: 0x04001135 RID: 4405
		public static readonly InOut ioErrorMotor7_HYRework = new InOut("DI_20", "parItemErrorEngine");

		// Token: 0x04001136 RID: 4406
		public static readonly InOut ioTempSensor0New = new InOut("AI_02", "parItemTempSensor0");

		// Token: 0x04001137 RID: 4407
		public static readonly InOut ioTempSensor1New = new InOut("AI_03", "parItemTempSensor1");

		// Token: 0x04001138 RID: 4408
		public static readonly InOut ioTempSensorSesotecMain = new InOut("Temp_00", "parItemTempSensor1");

		// Token: 0x04001139 RID: 4409
		public static readonly InOut ioTempSensorSesotecSecondary = new InOut("Temp_01", "parItemTempSensor0");

		// Token: 0x0400113A RID: 4410
		public static readonly InOut ioLowAirPressureNew = new InOut("DI_24", "parItemLowAirPressure");

		// Token: 0x0400113B RID: 4411
		public static readonly InOut ioLowAirPressureSesotecIO = new InOut("DI_48", "parItemLowAirPressure");

		// Token: 0x0400113C RID: 4412
		public static readonly InOut ioLowAirPressureEntry = new InOut("DI_09", "parItemLowAirPressure");

		// Token: 0x0400113D RID: 4413
		public static readonly InOut ioLowAirPressureRayconDPlus1Module = new InOut("DI_24", "parItemLowAirPressure");

		// Token: 0x0400113E RID: 4414
		public static readonly InOut ioMachineEnableExternSesotecIO = new InOut("DI_32", "parItemMachineEnableExtern");

		// Token: 0x0400113F RID: 4415
		public static readonly InOut ioMachineEnableExternEntryAndDPlus1Module = new InOut("DI_06", "parItemMachineEnableExtern");

		// Token: 0x04001140 RID: 4416
		private int surveyPeriod = 1000;

		// Token: 0x04001141 RID: 4417
		private int checkingXRaySourceOHPeriod = 3600000;

		// Token: 0x04001142 RID: 4418
		private const string wagoErrorPath = "/hardware/ios/LEDErrorCode/Value";

		// Token: 0x04001143 RID: 4419
		private const string wagoErrorArgument = "/hardware/ios/LEDErrorArgument/Value";

		// Token: 0x04001144 RID: 4420
		private const string ioCommErrorPath = "/hardware/ios/CommError/Value";

		// Token: 0x04001145 RID: 4421
		private const string detectorDTCommErrorPath = "/hardware/detector/dt/CommError/Value";

		// Token: 0x04001146 RID: 4422
		private const string detectorSesotecCommErrorPath = "/hardware/detector/sesotec/CommError/Value";

		// Token: 0x04001147 RID: 4423
		private const string binFull1Path = "/hardware/rejecter/rejecter1/BinFull/Value";

		// Token: 0x04001148 RID: 4424
		private const string binFull2Path = "/hardware/rejecter/rejecter2/BinFull/Value";

		// Token: 0x04001149 RID: 4425
		private const string binFull3Path = "/hardware/rejecter/rejecter3/BinFull/Value";

		// Token: 0x0400114A RID: 4426
		private const string binFull4Path = "/hardware/rejecter/rejecter4/BinFull/Value";

		// Token: 0x0400114B RID: 4427
		private const string missingVerifyObjects1Path = "/hardware/rejecter/rejecter1/VerifyMissingObjects/Value";

		// Token: 0x0400114C RID: 4428
		private const string missingVerifyObjects2Path = "/hardware/rejecter/rejecter2/VerifyMissingObjects/Value";

		// Token: 0x0400114D RID: 4429
		private const string missingVerifyObjects3Path = "/hardware/rejecter/rejecter3/VerifyMissingObjects/Value";

		// Token: 0x0400114E RID: 4430
		private const string missingVerifyObjects4Path = "/hardware/rejecter/rejecter4/VerifyMissingObjects/Value";

		// Token: 0x0400114F RID: 4431
		private const string memoryUsagePath = "/application/MemoryUsage/Value";

		// Token: 0x04001150 RID: 4432
		private const string errorXrayOnLampPath = "/hardware/ios/ErrorXrayOnLamp/Value";

		// Token: 0x04001151 RID: 4433
		private const string lightBarrier1Path = "/hardware/rejecter/rejecter1/LightBarrier/Value";

		// Token: 0x04001152 RID: 4434
		private const string lightBarrier2Path = "/hardware/rejecter/rejecter2/LightBarrier/Value";

		// Token: 0x04001153 RID: 4435
		private const string lightBarrier3Path = "/hardware/rejecter/rejecter3/LightBarrier/Value";

		// Token: 0x04001154 RID: 4436
		private const string lightBarrier4Path = "/hardware/rejecter/rejecter4/LightBarrier/Value";

		// Token: 0x04001155 RID: 4437
		private const string ioErrorDOPath = "/hardware/ios/ErrorDO/Value";

		// Token: 0x04001156 RID: 4438
		private const string ioErrorAIPath = "/hardware/ios/ErrorAI/Value";

		// Token: 0x04001157 RID: 4439
		private const string ioCommErrorSMPath = "/hardware/ios/CommErrorSM/Value";

		// Token: 0x04001158 RID: 4440
		private const double tempBottomMax = 40.0;

		// Token: 0x04001159 RID: 4441
		private const double tempTopMax = 45.0;

		// Token: 0x0400115A RID: 4442
		private const double tempTopMaxEntry = 50.0;

		// Token: 0x0400115B RID: 4443
		private const double tempTopMaxDplusTX = 48.0;

		// Token: 0x0400115C RID: 4444
		private const double voltage12VMin = 11.0;

		// Token: 0x0400115D RID: 4445
		private const double voltage12VMax = 14.0;

		// Token: 0x0400115E RID: 4446
		private const double voltage24VMin = 22.0;

		// Token: 0x0400115F RID: 4447
		private const double voltage24VMax = 27.0;

		// Token: 0x04001160 RID: 4448
		private int nMaxInterlockFaults = 4;

		// Token: 0x04001161 RID: 4449
		private const int maxWAGOErrors = 3;

		// Token: 0x04001162 RID: 4450
		private const int maxRS232XRayErrors = 3;

		// Token: 0x04001163 RID: 4451
		private const int maxImageDelay = 5;

		// Token: 0x04001164 RID: 4452
		private const double maxMemoryUsage = 90.0;

		// Token: 0x04001165 RID: 4453
		private int nMaxSVTFaults = 4;

		// Token: 0x04001166 RID: 4454
		public static readonly int minWagoVersion = 4;

		// Token: 0x04001167 RID: 4455
		public static readonly int minLightBarrierWagoVersion = 7;

		// Token: 0x04001168 RID: 4456
		private const int xrayLampErrorWaitTime = 12;

		// Token: 0x04001169 RID: 4457
		private int maxUnderCurrentFaults = 2;

		// Token: 0x0400116A RID: 4458
		private int maxSVTCommFaults = 5;

		// Token: 0x0400116B RID: 4459
		private int maxSVTLowAirPressureFaults = 2;

		// Token: 0x0400116C RID: 4460
		private int maxImageProcessingFaults = 3;

		// Token: 0x0400116D RID: 4461
		private int maxDetectorCommFaults = 2;

		// Token: 0x0400116E RID: 4462
		private int maxInsightNetFaults = 3;

		// Token: 0x0400116F RID: 4463
		private int secondsToGetInsightConnection = 90;

		// Token: 0x04001170 RID: 4464
		private int counterForDualMeanValDiff = 0;

		// Token: 0x04001171 RID: 4465
		private ParameterHandling parHandling;

		// Token: 0x04001172 RID: 4466
		private DBConnection dbConnection;

		// Token: 0x04001173 RID: 4467
		private DBDataLogs dbDatalogs;

		// Token: 0x04001174 RID: 4468
		private MachineState machineState;

		// Token: 0x04001175 RID: 4469
		private MessageSystem messageSystem;

		// Token: 0x04001176 RID: 4470
		private bool surveyRunning;

		// Token: 0x04001177 RID: 4471
		private bool checkxrayoperatinghours;

		// Token: 0x04001178 RID: 4472
		private XRayOperatingHours xrayoperatinghours;

		// Token: 0x04001179 RID: 4473
		private int nCurrentInterlockFaults;

		// Token: 0x0400117A RID: 4474
		private int nCurrentUnderCurrentFaults;

		// Token: 0x0400117B RID: 4475
		private int nCurrentSVTCommFaults;

		// Token: 0x0400117C RID: 4476
		private int nCurrentImageProcessingFaults;

		// Token: 0x0400117D RID: 4477
		private int nCurrentInsightNetFaults;

		// Token: 0x0400117E RID: 4478
		private int nSVTFaults;

		// Token: 0x0400117F RID: 4479
		private int nWAGOErrors;

		// Token: 0x04001180 RID: 4480
		private int nRS232XRayErrors;

		// Token: 0x04001181 RID: 4481
		private int nSVTLowAirPressureErrors;

		// Token: 0x04001182 RID: 4482
		private bool checkSpeedCalc;

		// Token: 0x04001183 RID: 4483
		private int checkSpeedCalcCount;

		// Token: 0x04001184 RID: 4484
		private int nDetectorCommFaults;

		// Token: 0x04001185 RID: 4485
		private int ncheckflankXrayComm;

		// Token: 0x04001186 RID: 4486
		private List<int> MovAverageHE = new List<int>();

		// Token: 0x04001187 RID: 4487
		private List<int> MovAverageLE = new List<int>();

		// Token: 0x04001188 RID: 4488
		private List<DateTime> serialArcTimes = new List<DateTime>();

		// Token: 0x04001189 RID: 4489
		private List<DateTime> imageArcTimes = new List<DateTime>();

		// Token: 0x0400118A RID: 4490
		private ErrorHandling errorHandling;

		// Token: 0x0400118B RID: 4491
		private DateTime lastErrorXRayLamp;

		// Token: 0x0400118C RID: 4492
		private MachineErrors machineErrors;

		// Token: 0x0400118D RID: 4493
		private AuditRequired auditRequired;

		// Token: 0x0400118E RID: 4494
		private int oldSerialArcs;

		// Token: 0x0400118F RID: 4495
		private int oldImageArcs;

		// Token: 0x04001190 RID: 4496
		private bool arcsInitialized;

		// Token: 0x04001191 RID: 4497
		private MachineMode machineMode = MachineMode.Standard;

		// Token: 0x04001192 RID: 4498
		private SSTEthernetServer sstServer;

		// Token: 0x04001193 RID: 4499
		private DateTime timeOfCheckStart;

		// Token: 0x04001194 RID: 4500
		private string strSpeedConnectFault = "";

		// Token: 0x04001195 RID: 4501
		private bool isXRayOnEX2A = false;

		// Token: 0x04001196 RID: 4502
		private bool checkAirPressure = true;

		// Token: 0x04001197 RID: 4503
		private bool emergencyStopWasPressed = false;

		// Token: 0x040011A0 RID: 4512
		private Conveyor conveyor;

		// Token: 0x040011A1 RID: 4513
		private DetectorType detectorType;

		// Token: 0x040011A2 RID: 4514
		private int checkIPCounter;

		// Token: 0x040011A3 RID: 4515
		private bool checkErrorOpHoursXRay = false;

		// Token: 0x040011A4 RID: 4516
		private bool checkWarningOpHoursXRay = false;

		// Token: 0x040011A5 RID: 4517
		private int actualBorderOperatingHours;

		// Token: 0x040011A6 RID: 4518
		private double actualOperatingHours;

		// Token: 0x040011A7 RID: 4519
		private DateTime timeStampForErrorOPHoursXRaySource;

		// Token: 0x040011A8 RID: 4520
		private Thread thxrayoh;

		// Token: 0x040011A9 RID: 4521
		private bool checkAuditIfRestartMachine = true;

		// Token: 0x040011AA RID: 4522
		private string oldMaxBackgroundValueImage = "";

		// Token: 0x040011AB RID: 4523
		private int counterErrorMotor = 0;

		// Token: 0x040011AD RID: 4525
		private static int imageDelayMaxError = 5;

		// Token: 0x020002E7 RID: 743
		public enum IOProgramType
		{
			// Token: 0x04002034 RID: 8244
			Test,
			// Token: 0x04002035 RID: 8245
			Standard,
			// Token: 0x04002036 RID: 8246
			Sideshooter,
			// Token: 0x04002037 RID: 8247
			Bulk,
			// Token: 0x04002038 RID: 8248
			Entry,
			// Token: 0x04002039 RID: 8249
			DPlus
		}

		// Token: 0x020002E8 RID: 744
		public enum IOProgramSubversionsOld
		{
			// Token: 0x0400203B RID: 8251
			Test,
			// Token: 0x0400203C RID: 8252
			Default,
			// Token: 0x0400203D RID: 8253
			Advanced,
			// Token: 0x0400203E RID: 8254
			Bulk,
			// Token: 0x0400203F RID: 8255
			HighLevel
		}

		// Token: 0x020002E9 RID: 745
		public enum IOProgramSubversionsNew
		{
			// Token: 0x04002041 RID: 8257
			Test,
			// Token: 0x04002042 RID: 8258
			Default,
			// Token: 0x04002043 RID: 8259
			Advanced,
			// Token: 0x04002044 RID: 8260
			HighLevel
		}
	}
}
