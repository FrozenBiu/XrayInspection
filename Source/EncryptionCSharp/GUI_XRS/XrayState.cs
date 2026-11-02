using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Timers;
using System.Windows;
using System.Windows.Threading;
using DatabaseConnection;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x0200023A RID: 570
	public class XrayState
	{
		// Token: 0x140000E3 RID: 227
		// (add) Token: 0x06002B67 RID: 11111 RVA: 0x0011F860 File Offset: 0x0011DA60
		// (remove) Token: 0x06002B68 RID: 11112 RVA: 0x0011F898 File Offset: 0x0011DA98
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action XRayPowerOn;

		// Token: 0x140000E4 RID: 228
		// (add) Token: 0x06002B69 RID: 11113 RVA: 0x0011F8D0 File Offset: 0x0011DAD0
		// (remove) Token: 0x06002B6A RID: 11114 RVA: 0x0011F908 File Offset: 0x0011DB08
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action XRayPowerOff;

		// Token: 0x140000E5 RID: 229
		// (add) Token: 0x06002B6B RID: 11115 RVA: 0x0011F940 File Offset: 0x0011DB40
		// (remove) Token: 0x06002B6C RID: 11116 RVA: 0x0011F978 File Offset: 0x0011DB78
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action ConveyorRunningOn;

		// Token: 0x140000E6 RID: 230
		// (add) Token: 0x06002B6D RID: 11117 RVA: 0x0011F9B0 File Offset: 0x0011DBB0
		// (remove) Token: 0x06002B6E RID: 11118 RVA: 0x0011F9E8 File Offset: 0x0011DBE8
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action ConveyorRunningOff;

		// Token: 0x140000E7 RID: 231
		// (add) Token: 0x06002B6F RID: 11119 RVA: 0x0011FA20 File Offset: 0x0011DC20
		// (remove) Token: 0x06002B70 RID: 11120 RVA: 0x0011FA58 File Offset: 0x0011DC58
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action CalibrationDone;

		// Token: 0x140000E8 RID: 232
		// (add) Token: 0x06002B71 RID: 11121 RVA: 0x0011FA90 File Offset: 0x0011DC90
		// (remove) Token: 0x06002B72 RID: 11122 RVA: 0x0011FAC8 File Offset: 0x0011DCC8
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action KeyClosed;

		// Token: 0x140000E9 RID: 233
		// (add) Token: 0x06002B73 RID: 11123 RVA: 0x0011FB00 File Offset: 0x0011DD00
		// (remove) Token: 0x06002B74 RID: 11124 RVA: 0x0011FB38 File Offset: 0x0011DD38
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action KeyOpened;

		// Token: 0x140000EA RID: 234
		// (add) Token: 0x06002B75 RID: 11125 RVA: 0x0011FB70 File Offset: 0x0011DD70
		// (remove) Token: 0x06002B76 RID: 11126 RVA: 0x0011FBA8 File Offset: 0x0011DDA8
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action Reseasoning;

		// Token: 0x140000EB RID: 235
		// (add) Token: 0x06002B77 RID: 11127 RVA: 0x0011FBE0 File Offset: 0x0011DDE0
		// (remove) Token: 0x06002B78 RID: 11128 RVA: 0x0011FC18 File Offset: 0x0011DE18
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<bool> SourceXRayOnChanged;

		// Token: 0x140000EC RID: 236
		// (add) Token: 0x06002B79 RID: 11129 RVA: 0x0011FC50 File Offset: 0x0011DE50
		// (remove) Token: 0x06002B7A RID: 11130 RVA: 0x0011FC88 File Offset: 0x0011DE88
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<int> RemainingReseasonTimeChanged;

		// Token: 0x140000ED RID: 237
		// (add) Token: 0x06002B7B RID: 11131 RVA: 0x0011FCC0 File Offset: 0x0011DEC0
		// (remove) Token: 0x06002B7C RID: 11132 RVA: 0x0011FCF8 File Offset: 0x0011DEF8
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<bool> TestSuiteActiveChanged;

		// Token: 0x140000EE RID: 238
		// (add) Token: 0x06002B7D RID: 11133 RVA: 0x0011FD30 File Offset: 0x0011DF30
		// (remove) Token: 0x06002B7E RID: 11134 RVA: 0x0011FD68 File Offset: 0x0011DF68
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<string> UnknownModelNumberDetected;

		// Token: 0x140000EF RID: 239
		// (add) Token: 0x06002B7F RID: 11135 RVA: 0x0011FDA0 File Offset: 0x0011DFA0
		// (remove) Token: 0x06002B80 RID: 11136 RVA: 0x0011FDD8 File Offset: 0x0011DFD8
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<bool> NoConnectionToSourceOrDetector;

		// Token: 0x140000F0 RID: 240
		// (add) Token: 0x06002B81 RID: 11137 RVA: 0x0011FE10 File Offset: 0x0011E010
		// (remove) Token: 0x06002B82 RID: 11138 RVA: 0x0011FE48 File Offset: 0x0011E048
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action StateForNoBeltStartOnError;

		// Token: 0x140000F1 RID: 241
		// (add) Token: 0x06002B83 RID: 11139 RVA: 0x0011FE80 File Offset: 0x0011E080
		// (remove) Token: 0x06002B84 RID: 11140 RVA: 0x0011FEB8 File Offset: 0x0011E0B8
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action SetOldStateForNoConnection;

		// Token: 0x140000F2 RID: 242
		// (add) Token: 0x06002B85 RID: 11141 RVA: 0x0011FEF0 File Offset: 0x0011E0F0
		// (remove) Token: 0x06002B86 RID: 11142 RVA: 0x0011FF28 File Offset: 0x0011E128
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action SetActualStateForNoConnection;

		// Token: 0x17000BDB RID: 3035
		// (get) Token: 0x06002B87 RID: 11143 RVA: 0x0011FF60 File Offset: 0x0011E160
		// (set) Token: 0x06002B88 RID: 11144 RVA: 0x0011FF78 File Offset: 0x0011E178
		public bool TestSuiteActive
		{
			get
			{
				return this.testSuiteActive;
			}
			set
			{
				bool flag = this.testSuiteActive != value;
				if (flag)
				{
					this.testSuiteActive = value;
					this.OnTestSuiteActiveChanged(value);
				}
			}
		}

		// Token: 0x17000BDC RID: 3036
		// (get) Token: 0x06002B89 RID: 11145 RVA: 0x0011FFA7 File Offset: 0x0011E1A7
		// (set) Token: 0x06002B8A RID: 11146 RVA: 0x0011FFAF File Offset: 0x0011E1AF
		public bool Calibrated { get; set; }

		// Token: 0x06002B8B RID: 11147 RVA: 0x0011FFB8 File Offset: 0x0011E1B8
		public XrayState(ParameterHandling parHandling, DBConnection dbConnection, MeanPixelLogger meanPixelLogger, PixelCorrection pixelCorrection, DBDataLogs dbDatalogs, LiveCalibration liveCalibration)
		{
			this.parHandling = parHandling;
			this.dbConnection = dbConnection;
			this.meanPixelLogger = meanPixelLogger;
			this.pixelCorrection = pixelCorrection;
			this.liveCalibration = liveCalibration;
			this.Calibrated = false;
			this.xrayOperatingHours = new XRayOperatingHours(dbConnection, parHandling, dbDatalogs);
			parHandling.ParameterChanged += this.parHandling_ParameterChanged;
			this.timerLastXrayOn.Interval = 1800000.0;
			this.timerLastXrayOn.Elapsed += this.timerLastXrayOn_Elapsed;
		}

		// Token: 0x06002B8C RID: 11148 RVA: 0x00120080 File Offset: 0x0011E280
		protected virtual void OnXRayPowerOn()
		{
			Action xrayPowerOn = this.XRayPowerOn;
			bool flag = xrayPowerOn != null;
			if (flag)
			{
				xrayPowerOn();
			}
		}

		// Token: 0x06002B8D RID: 11149 RVA: 0x001200A4 File Offset: 0x0011E2A4
		protected virtual void OnXRayPowerOff()
		{
			Action xrayPowerOff = this.XRayPowerOff;
			bool flag = xrayPowerOff != null;
			if (flag)
			{
				xrayPowerOff();
			}
		}

		// Token: 0x06002B8E RID: 11150 RVA: 0x001200C8 File Offset: 0x0011E2C8
		protected virtual void OnConveyorRunningOn()
		{
			Action conveyorRunningOn = this.ConveyorRunningOn;
			bool flag = conveyorRunningOn != null;
			if (flag)
			{
				conveyorRunningOn();
			}
		}

		// Token: 0x06002B8F RID: 11151 RVA: 0x001200EC File Offset: 0x0011E2EC
		protected virtual void OnConveyorRunningOff()
		{
			Action conveyorRunningOff = this.ConveyorRunningOff;
			bool flag = conveyorRunningOff != null;
			if (flag)
			{
				conveyorRunningOff();
			}
		}

		// Token: 0x06002B90 RID: 11152 RVA: 0x00120110 File Offset: 0x0011E310
		protected virtual void OnCalibrationDone()
		{
			Action calibrationDone = this.CalibrationDone;
			bool flag = calibrationDone != null;
			if (flag)
			{
				calibrationDone();
			}
		}

		// Token: 0x06002B91 RID: 11153 RVA: 0x00120134 File Offset: 0x0011E334
		protected virtual void OnReseasoning()
		{
			Action action = this.Reseasoning;
			bool flag = action != null;
			if (flag)
			{
				action();
			}
		}

		// Token: 0x06002B92 RID: 11154 RVA: 0x00120158 File Offset: 0x0011E358
		protected virtual void OnKeyClosed()
		{
			Action action = this.KeyClosed;
			bool flag = action != null;
			if (flag)
			{
				action();
			}
		}

		// Token: 0x06002B93 RID: 11155 RVA: 0x0012017C File Offset: 0x0011E37C
		protected virtual void OnKeyOpened()
		{
			Action keyOpened = this.KeyOpened;
			bool flag = keyOpened != null;
			if (flag)
			{
				keyOpened();
			}
		}

		// Token: 0x06002B94 RID: 11156 RVA: 0x001201A0 File Offset: 0x0011E3A0
		protected virtual void OnTestSuiteActiveChanged(bool active)
		{
			Action<bool> testSuiteActiveChanged = this.TestSuiteActiveChanged;
			bool flag = testSuiteActiveChanged != null;
			if (flag)
			{
				testSuiteActiveChanged(active);
			}
		}

		// Token: 0x06002B95 RID: 11157 RVA: 0x001201C8 File Offset: 0x0011E3C8
		protected virtual void OnNoConnectionToSourceOrDetector(bool message)
		{
			Action<bool> noConnectionToSourceOrDetector = this.NoConnectionToSourceOrDetector;
			bool flag = noConnectionToSourceOrDetector != null;
			if (flag)
			{
				noConnectionToSourceOrDetector(message);
			}
		}

		// Token: 0x06002B96 RID: 11158 RVA: 0x001201F0 File Offset: 0x0011E3F0
		protected virtual void OnStateForNoBeltStartOnError()
		{
			Action stateForNoBeltStartOnError = this.StateForNoBeltStartOnError;
			bool flag = stateForNoBeltStartOnError != null;
			if (flag)
			{
				stateForNoBeltStartOnError();
			}
		}

		// Token: 0x06002B97 RID: 11159 RVA: 0x00120214 File Offset: 0x0011E414
		protected virtual void OnRemainingReseasonTimeChanged(int remainingTime)
		{
			Action<int> remainingReseasonTimeChanged = this.RemainingReseasonTimeChanged;
			bool flag = remainingReseasonTimeChanged != null;
			if (flag)
			{
				remainingReseasonTimeChanged(remainingTime);
			}
		}

		// Token: 0x06002B98 RID: 11160 RVA: 0x0012023C File Offset: 0x0011E43C
		protected virtual void OnSetOldState()
		{
			Action setOldStateForNoConnection = this.SetOldStateForNoConnection;
			bool flag = setOldStateForNoConnection != null;
			if (flag)
			{
				setOldStateForNoConnection();
			}
		}

		// Token: 0x06002B99 RID: 11161 RVA: 0x00120260 File Offset: 0x0011E460
		protected virtual void OnSetActualState()
		{
			Action setActualStateForNoConnection = this.SetActualStateForNoConnection;
			bool flag = setActualStateForNoConnection != null;
			if (flag)
			{
				setActualStateForNoConnection();
			}
		}

		// Token: 0x06002B9A RID: 11162 RVA: 0x00120284 File Offset: 0x0011E484
		private void timerLastXrayOn_Elapsed(object sender, ElapsedEventArgs e)
		{
			this.dbConnection.SetLastXrayOn(DateTime.Now.ToString("dd.MM.yyyy"));
		}

		// Token: 0x06002B9B RID: 11163 RVA: 0x001202B0 File Offset: 0x0011E4B0
		private void parHandling_ParameterChanged(object sender, ParameterChangeEventArgs e)
		{
			bool flag = !this.reseasoning && (e.Path == Definition.detectorDTScanratePath || e.Path == Definition.detectorSesotecScanratePath || e.Path == "/hardware/xraysource/VoltageSetpoint/Value" || e.Path == "/hardware/xraysource/CurrentSetpoint/Value" || e.Path == "SShXRayPosition");
			if (flag)
			{
				XRayOnProcessing.DisableXray(this.parHandling, this.dbConnection, true);
				this.calibrated = false;
				this.Calibrated = false;
			}
		}

		// Token: 0x06002B9C RID: 11164 RVA: 0x0012034C File Offset: 0x0011E54C
		public void Start()
		{
			this.checkThreadRunning = true;
			new Thread(new ThreadStart(this.CheckXRay))
			{
				Name = "XRayState",
				Priority = ThreadPriority.AboveNormal
			}.Start();
			new Thread(new ThreadStart(this.CheckSourceXRayOn))
			{
				Name = "SourceXRayOn"
			}.Start();
		}

		// Token: 0x06002B9D RID: 11165 RVA: 0x001203B3 File Offset: 0x0011E5B3
		public void Stop()
		{
			this.checkThreadRunning = false;
		}

		// Token: 0x06002B9E RID: 11166 RVA: 0x001203C0 File Offset: 0x0011E5C0
		private void CheckXRay()
		{
			XRayOnProcessing xrayOnProcessing = new XRayOnProcessing(this.dbConnection, this.parHandling, this.meanPixelLogger, this.pixelCorrection, this.liveCalibration);
			xrayOnProcessing.RemainingReseasonTimeChanged += this.xrayOnProcessing_RemainingReseasonTimeChanged;
			xrayOnProcessing.UnknownModelNumberDetected += this.xrayOnProcessing_UnknownModelNumberDetected;
			while (this.checkThreadRunning)
			{
				bool flag = false;
				bool flag2 = false;
				bool flag3 = false;
				this.dbConnection.GetNoBeltStartOnError(ref flag2);
				this.dbConnection.GetHasAnyError(ref flag3);
				try
				{
					bool flag4 = this.TestSuiteActive;
					bool flag5;
					if (flag4)
					{
						flag5 = false;
					}
					else
					{
						bool flag6 = flag2;
						flag5 = (flag6 || (!ErrorSurvey.CommDetector && !ErrorSurvey.CommXRaySource));
						bool flag7 = !flag5;
						if (flag7)
						{
							bool flag8 = !this.setOldState;
							if (flag8)
							{
								this.OnSetOldState();
								this.setOldState = true;
							}
							bool commDetector = ErrorSurvey.CommDetector;
							if (commDetector)
							{
								this.OnNoConnectionToSourceOrDetector(false);
							}
							else
							{
								bool commXRaySource = ErrorSurvey.CommXRaySource;
								if (commXRaySource)
								{
									this.OnNoConnectionToSourceOrDetector(true);
								}
							}
							this.setErrorFlagForConnectFault = true;
						}
					}
					bool flag9 = flag5 && !this.skipNextStatusCheck;
					if (flag9)
					{
						string a = this.parHandling.ReadPar(Definition.conveyorIsRunning);
						bool flag10 = a == "1" && !this.conveyorRunning;
						if (flag10)
						{
							this.SignalConveyorRunning();
							flag = true;
						}
						bool flag11 = a == "0" && this.conveyorRunning;
						if (flag11)
						{
							bool flag12 = flag2 && flag3;
							if (flag12)
							{
								this.OnStateForNoBeltStartOnError();
							}
							else
							{
								this.SignalConveyorOff();
							}
							flag = true;
						}
						string a2 = this.parHandling.ReadPar("/hardware/ios/DI_03/Value");
						bool flag13 = a2 == "1" && this.conveyorRunning && !this.keyClosed;
						if (flag13)
						{
							this.keyClosed = true;
							this.OnKeyClosed();
							flag = true;
						}
						bool flag14 = a2 == "0" && this.conveyorRunning && this.keyClosed;
						if (flag14)
						{
							this.keyClosed = false;
							this.OnKeyOpened();
							flag = true;
						}
						bool flag15 = !this.xrayPower;
						if (flag15)
						{
							XRayOnProcessing.CalibrationError = false;
							bool flag16 = this.parHandling.ReadPar(Definition.isLiveCalibrationActivePath) == "1" || this.parHandling.ReadPar(Definition.isLiveCalibrationActivePath) == "2";
							if (flag16)
							{
								this.parHandling.WriteValue(Definition.LiveCalibrationError, "0");
							}
							this.switchBackgroundMonitoring(false);
						}
						string a3 = this.parHandling.ReadPar(XrayState.xrayStarterPath);
						bool flag17 = (a3 == "1" && !this.xrayPower) || (this.xrayPower && !this.calibrated);
						if (flag17)
						{
							this.switchBackgroundMonitoring(false);
							flag = true;
							bool flag18 = true;
							this.xrayPower = true;
							this.parHandling.WriteValue(Definition.vibroManualModeByGUIPath, "0", false);
							bool flag19 = xrayOnProcessing.CheckForReseasoning();
							if (flag19)
							{
								this.OnReseasoning();
								this.reseasoning = true;
								bool flag20 = this.WaitForXRayPowerOn();
								flag18 = (flag20 && xrayOnProcessing.ReseasonProcessing());
								this.reseasoning = false;
							}
							bool flag21 = flag18;
							if (flag21)
							{
								this.dbConnection.SetLastXrayOn(DateTime.Now.ToString("dd.MM.yyyy"));
								this.parHandling.WriteValue(MachineState.lastXRayOnPath, DateTime.Now.ToString("dd.MM.yyyy"), false);
								this.timerLastXrayOn.Start();
								this.OnXRayPowerOn();
								this.WaitForXRayPowerOn();
								bool flag22 = false;
								for (int i = 0; i < 10; i++)
								{
									Thread.Sleep(200);
									bool flag23 = this.parHandling.ReadPar(XrayState.xrayStarterPath) != "1";
									if (flag23)
									{
										flag22 = true;
										break;
									}
								}
								this.DoXrayPositioning();
								bool flag24 = !flag22 && xrayOnProcessing.Execute();
								if (flag24)
								{
									DetectorType detectorType = DetectorType.DT;
									this.dbConnection.GetDetectorType(ref detectorType);
									MachineMode machineMode = MachineMode.Standard;
									this.dbConnection.GetMachineMode(ref machineMode);
									MachineType machineType = MachineType.MachineTypeUndefined;
									this.dbConnection.GetMachineType(ref machineType);
									bool flag25 = detectorType == DetectorType.DTNetTDI;
									if (flag25)
									{
										this.SwitchToTDIInspecting();
									}
									bool flag26 = machineMode == MachineMode.Bulk;
									if (flag26)
									{
										BulkMode.UpdateBulkOverlappingInspection(this.parHandling);
									}
									this.OnCalibrationDone();
									this.calibrated = true;
									this.Calibrated = true;
									this.switchBackgroundMonitoring(true);
								}
								else
								{
									DetectorType detectorType2 = DetectorType.DT;
									this.dbConnection.GetDetectorType(ref detectorType2);
									bool flag27 = detectorType2 == DetectorType.DTNetTDI;
									if (flag27)
									{
										XRayOnProcessing.LogTdiCalib("Error Calibration not finished");
										ConveyingDirection conveyingDirection = ConveyingDirection.LeftToRight;
										this.dbConnection.GetConveyingDirection(ref conveyingDirection);
										bool flag28 = conveyingDirection == ConveyingDirection.LeftToRight;
										if (flag28)
										{
											this.parHandling.WriteValue("/hardware/detector/dt/TdiCorrectionMode/Value", 1.ToString(), false);
										}
										else
										{
											this.parHandling.WriteValue("/hardware/detector/dt/TdiCorrectionMode/Value", 2.ToString(), false);
										}
										bool calibrateMaxPossibleNoTdi = XRayOnProcessing.CalibrateMaxPossibleNoTdi;
										if (calibrateMaxPossibleNoTdi)
										{
											Thread.Sleep(100);
											this.parHandling.WriteValue(Definition.detectorDTIntegrationTimePath, XRayOnProcessing.CurrentIntegrationTimeTdi.ToString());
										}
									}
									this.SignalXRayPowerOff();
								}
							}
							else
							{
								this.SignalXRayPowerOff();
							}
						}
						bool flag29 = a3 == "0" && this.xrayPower;
						if (flag29)
						{
							flag = true;
							MachineMode machineMode2 = MachineMode.Standard;
							this.dbConnection.GetMachineMode(ref machineMode2);
							bool flag30 = machineMode2 == MachineMode.Bulk;
							if (flag30)
							{
								BulkMode.DeactivateForXRayOff(this.dbConnection, this.parHandling);
							}
							this.SignalXRayPowerOff();
							this.timerLastXrayOn.Stop();
						}
						bool flag31 = this.setErrorFlagForConnectFault && !flag;
						if (flag31)
						{
							this.OnSetActualState();
							this.setOldState = false;
						}
						this.setErrorFlagForConnectFault = false;
					}
					this.skipNextStatusCheck = false;
					Thread.Sleep(1000);
				}
				catch (Exception ex)
				{
					Exception ex3;
					Exception ex2 = ex3;
					Exception ex = ex2;
					bool flag32 = Application.Current != null && !Application.Current.Dispatcher.CheckAccess();
					if (flag32)
					{
						Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate()
						{
							Dialog.Show("Error XRay - State", ex.Message);
						}));
					}
				}
			}
			xrayOnProcessing.RemainingReseasonTimeChanged -= this.xrayOnProcessing_RemainingReseasonTimeChanged;
			xrayOnProcessing.UnknownModelNumberDetected -= this.xrayOnProcessing_UnknownModelNumberDetected;
		}

		// Token: 0x06002B9F RID: 11167 RVA: 0x00120A90 File Offset: 0x0011EC90
		private void xrayOnProcessing_UnknownModelNumberDetected(string modelNumber)
		{
			this.OnUnknownModelNumberDetected(modelNumber);
		}

		// Token: 0x06002BA0 RID: 11168 RVA: 0x00120A9B File Offset: 0x0011EC9B
		private void xrayOnProcessing_RemainingReseasonTimeChanged(int remainingTime)
		{
			this.OnRemainingReseasonTimeChanged(remainingTime);
		}

		// Token: 0x06002BA1 RID: 11169 RVA: 0x00120AA8 File Offset: 0x0011ECA8
		private bool WaitForXRayPowerOn()
		{
			int i = 0;
			while (i < 100)
			{
				bool flag = this.parHandling.ReadPar(XrayState.xrayPowerOnPath) == "1";
				bool result;
				if (flag)
				{
					result = true;
				}
				else
				{
					bool flag2 = this.parHandling.ReadPar(XrayState.xrayStarterPath) == "0";
					if (!flag2)
					{
						Thread.Sleep(100);
						i++;
						continue;
					}
					result = false;
				}
				return result;
			}
			return false;
		}

		// Token: 0x06002BA2 RID: 11170 RVA: 0x00120B20 File Offset: 0x0011ED20
		private void SignalXRayPowerOff()
		{
			this.xrayPower = false;
			this.OnXRayPowerOff();
			this.parHandling.WriteValue(XRayOnProcessing.dtGainEnablePath, "0", false);
			this.parHandling.WriteValue(XRayOnProcessing.dtBaselineValuePath, "0", false);
			this.pixelCorrection.StopCorrection();
		}

		// Token: 0x06002BA3 RID: 11171 RVA: 0x00120B78 File Offset: 0x0011ED78
		protected virtual void OnSourceXRayOnChanged(bool enabled)
		{
			Action<bool> sourceXRayOnChanged = this.SourceXRayOnChanged;
			bool flag = sourceXRayOnChanged != null;
			if (flag)
			{
				sourceXRayOnChanged(enabled);
			}
		}

		// Token: 0x06002BA4 RID: 11172 RVA: 0x00120BA0 File Offset: 0x0011EDA0
		private void SetXrayOnTimes(bool sourceXrayOn)
		{
			this.xrayOperatingHours.SetXrayOnTimeForkVmA(sourceXrayOn);
			bool flag = this.parHandling != null;
			if (flag)
			{
				string text = this.parHandling.ReadPar(XrayState.xRayOnTimeInternalValuePath);
				string xraySourceModelNumber = this.parHandling.ReadPar(XRayOnProcessing.xrayModelNumberPath);
				bool flag2 = text != "-1.00" && text != "";
				if (flag2)
				{
					this.dbConnection.SetXRayOnTimeInternal(text);
					this.dbConnection.SetXRaySourceModelNumber(xraySourceModelNumber);
				}
			}
		}

		// Token: 0x06002BA5 RID: 11173 RVA: 0x00120C28 File Offset: 0x0011EE28
		private void CheckSourceXRayOn()
		{
			bool flag = false;
			while (this.checkThreadRunning)
			{
				string a = this.parHandling.ReadPar(XrayState.sourceXrayOnPath);
				bool flag2 = a == "1" && !flag;
				if (flag2)
				{
					flag = true;
					this.OnSourceXRayOnChanged(true);
				}
				bool flag3 = a == "0" && flag;
				if (flag3)
				{
					flag = false;
					this.OnSourceXRayOnChanged(false);
				}
				this.SetXrayOnTimes(flag);
				Thread.Sleep(1000);
			}
		}

		// Token: 0x06002BA6 RID: 11174 RVA: 0x00120CB4 File Offset: 0x0011EEB4
		protected virtual void OnUnknownModelNumberDetected(string modelNumber)
		{
			Action<string> unknownModelNumberDetected = this.UnknownModelNumberDetected;
			bool flag = unknownModelNumberDetected != null;
			if (flag)
			{
				unknownModelNumberDetected(modelNumber);
			}
		}

		// Token: 0x06002BA7 RID: 11175 RVA: 0x00120CDC File Offset: 0x0011EEDC
		private void DoXrayPositioning()
		{
			MachineMode machineMode = MachineMode.Standard;
			bool flag = false;
			this.dbConnection.GetMachineMode(ref machineMode);
			bool flag2 = machineMode == MachineMode.Sideshooter;
			if (flag2)
			{
				this.dbConnection.GetSshSourcePosNeeded(ref flag);
				bool flag3 = flag;
				if (flag3)
				{
					this.SetSShXrayPosition();
				}
			}
		}

		// Token: 0x06002BA8 RID: 11176 RVA: 0x00120D24 File Offset: 0x0011EF24
		private void SetSShXrayPosition()
		{
			string s = "";
			int sshXrayPosition = 0;
			int num = 180;
			this.dbConnection.GetSshMaxMoveTimeXRayMotor(ref num);
			this.dbConnection.GetParameterValue("SShXRayPosition", ref s);
			bool flag = int.TryParse(s, out sshXrayPosition);
			if (flag)
			{
				bool flag2 = Application.Current != null;
				if (flag2)
				{
					Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
					{
						this.ctrlMoveSource = new CtrlMoveSource(this.dbConnection, this.parHandling, null, null, true);
						this.ctrlMoveSource.Unloaded += this.ctrlMoveSource_Unloaded;
						Thread.Sleep(1000);
						Subwindow.Show(this.ctrlMoveSource);
						this.ctrlMoveSource.TriggerSourceMove(sshXrayPosition);
					}));
				}
				this.lastXRayPositioningTimedOut = !this.semaphoreXrayPositioning.WaitOne(num * 1000);
			}
		}

		// Token: 0x06002BA9 RID: 11177 RVA: 0x00120DD0 File Offset: 0x0011EFD0
		private void ctrlMoveSource_Unloaded(object sender, RoutedEventArgs e)
		{
			bool flag = this.ctrlMoveSource != null;
			if (flag)
			{
				this.ctrlMoveSource.Unloaded -= this.ctrlMoveSource_Unloaded;
				this.ctrlMoveSource = null;
				bool flag2 = !this.lastXRayPositioningTimedOut;
				if (flag2)
				{
					this.semaphoreXrayPositioning.Release();
				}
			}
		}

		// Token: 0x06002BAA RID: 11178 RVA: 0x00120E25 File Offset: 0x0011F025
		public void SignalConveyorRunning()
		{
			this.skipNextStatusCheck = true;
			this.conveyorRunning = true;
			this.OnConveyorRunningOn();
		}

		// Token: 0x06002BAB RID: 11179 RVA: 0x00120E3D File Offset: 0x0011F03D
		public void SignalConveyorOff()
		{
			this.skipNextStatusCheck = true;
			this.conveyorRunning = false;
			this.OnConveyorRunningOff();
		}

		// Token: 0x06002BAC RID: 11180 RVA: 0x00120E58 File Offset: 0x0011F058
		private void switchBackgroundMonitoring(bool enable)
		{
			bool flag = this.parHandling.ReadPar(Definition.detectorMonitoringActive) == "1";
			if (flag)
			{
				this.parHandling.WriteValue(Definition.detectorMonitoringBackgroundEnable, enable ? "1" : "0", false);
			}
		}

		// Token: 0x06002BAD RID: 11181 RVA: 0x00120EA8 File Offset: 0x0011F0A8
		private void SwitchToTDIInspecting()
		{
			ConveyingDirection conveyingDirection = ConveyingDirection.LeftToRight;
			this.dbConnection.GetConveyingDirection(ref conveyingDirection);
			string text = "";
			DetectorType detectorType = DetectorType.DT;
			this.dbConnection.GetDetectorType(ref detectorType);
			string strVoltageSetpoint = null;
			string strCurrentSetpoint = null;
			this.dbConnection.GetParameterValue("/hardware/xraysource/VoltageSetpoint/Value", ref strVoltageSetpoint);
			this.dbConnection.GetParameterValue("/hardware/xraysource/CurrentSetpoint/Value", ref strCurrentSetpoint);
			string scanrate = this.parHandling.ReadPar(Definition.detectorDTScanratePath);
			string serial = this.parHandling.ReadPar(Definition.detectorDTSerialPath);
			int num = 0;
			List<double> list = new List<double>();
			for (int i = 0; i < 1280; i++)
			{
				bool flag = MeanPixelLogger.nTdiDefectPixels[i] > 0;
				if (flag)
				{
					double item = 8.0 / (8.0 - (double)MeanPixelLogger.nTdiDefectPixels[i]);
					list.Add(item);
				}
				else
				{
					list.Add(1.0);
				}
			}
			string s = this.parHandling.ReadPar(Definition.dtMCUTemperaturePath);
			int temperature;
			int.TryParse(s, out temperature);
			XRayOnProcessing.LogTdiCalib("18 - Enalbe DT gain correction");
			this.parHandling.WriteValue(XRayOnProcessing.dtGainEnablePath, "1", false);
			this.dbConnection.GetParameterValue("/filter/acquisition/AddedFrontLines/Value", ref text);
			this.parHandling.WriteValue("/filter/acquisition/AddedFrontLines/Value", text, false);
			this.dbConnection.GetParameterValue("/filter/acquisition/AddedBackLines/Value", ref text);
			this.parHandling.WriteValue("/filter/acquisition/AddedBackLines/Value", text, false);
			XRayOnProcessing.LogTdiCalib("19 - Writing TdiCorrectionMode to TDI");
			bool flag2 = conveyingDirection == ConveyingDirection.LeftToRight;
			if (flag2)
			{
				this.parHandling.WriteValue("/hardware/detector/dt/TdiCorrectionMode/Value", 1.ToString(), false);
			}
			else
			{
				this.parHandling.WriteValue("/hardware/detector/dt/TdiCorrectionMode/Value", 2.ToString(), false);
			}
			text = this.parHandling.ReadPar("/hardware/detector/dt/TdiCorrectionMode/Value");
			XRayOnProcessing.LogTdiCalib("20 - TdiCorrectionMode is " + text + " " + ((TdiCorrectionMode)Convert.ToInt32(text)).ToString());
			bool calibrateMaxPossibleNoTdi = XRayOnProcessing.CalibrateMaxPossibleNoTdi;
			if (calibrateMaxPossibleNoTdi)
			{
				this.parHandling.WriteValue(Definition.detectorDTIntegrationTimePath, XRayOnProcessing.CurrentIntegrationTimeTdi.ToString());
			}
			for (int j = 0; j < 1280; j++)
			{
				this.parHandling.WriteValue("/filter/acquisition/GainCalibrationIndex/Value", j.ToString(), false);
				this.parHandling.WriteValue("/filter/acquisition/GainCalibrationValue/Value", list[j].ToString(), false);
			}
			text = this.parHandling.ReadPar("/filter/acquisition/BackgroundNoise/Value");
			int.TryParse(text, out num);
			int num2 = 10000 - (int)(10000.0 * (double)((float)num) / 100.0);
			this.parHandling.WriteValue("/filter/acquisition/Threshold/Value", num2.ToString(), false);
			this.parHandling.WriteValue("/hardware/detector/dt/StartStopScanning/Value", "1", false);
			XRayOnProcessing.LogTdiCalib("21 - Check if scanning started. Scanning is: " + this.parHandling.ReadPar("/hardware/detector/dt/StartStopScanning/Value"));
			this.dbConnection.AddGainPixelsEntry(temperature, strVoltageSetpoint, strCurrentSetpoint, scanrate, serial, DateTime.Now.ToString(new CultureInfo("de-DE")), list, true, true);
			bool flag3 = this.parHandling.ReadPar("/hardware/detector/LiveCalibration/Value") == "1" || this.parHandling.ReadPar("/hardware/detector/LiveCalibration/Value") == "2";
			if (flag3)
			{
				this.liveCalibration.StartTimer();
			}
			this.parHandling.WriteValue("/hardware/detector/dt/MissingLinesReset/Value", "1", false);
			XRayOnProcessing.LogTdiCalib("Process ID:" + this.parHandling.ReadPar("/application/PidXRSMain_64/Value"));
			XRayOnProcessing.LogTdiCalib("Calibration Done");
		}

		// Token: 0x04001B21 RID: 6945
		private const int lastXrayOnInterval = 1800000;

		// Token: 0x04001B22 RID: 6946
		private const int checkPeriod = 1000;

		// Token: 0x04001B23 RID: 6947
		private const int sourceXRayOnPeriod = 1000;

		// Token: 0x04001B24 RID: 6948
		private const string keyClosedPath = "/hardware/ios/DI_03/Value";

		// Token: 0x04001B25 RID: 6949
		public static string xrayPowerOnPath = "/hardware/xraysource/XRayPower/Value";

		// Token: 0x04001B26 RID: 6950
		public static string xrayStarterPath = "/hardware/ios/DI_02/Value";

		// Token: 0x04001B27 RID: 6951
		public static string sourceXrayOnPath = "/hardware/ios/DI_07/Value";

		// Token: 0x04001B28 RID: 6952
		public static string xrayStarterEntryAndDPlusPath = "/hardware/ios/DO_06/Value";

		// Token: 0x04001B29 RID: 6953
		public static string xrayStarterDPlus1ModulePath = "/hardware/ios/DO_04/Value";

		// Token: 0x04001B2A RID: 6954
		public static string xRayOnTimeInternalValuePath = "/hardware/xraysource/XRayOnTimeInternal/Value";

		// Token: 0x04001B2B RID: 6955
		private bool checkThreadRunning;

		// Token: 0x04001B2C RID: 6956
		private bool conveyorRunning;

		// Token: 0x04001B2D RID: 6957
		private bool xrayPower;

		// Token: 0x04001B2E RID: 6958
		private bool keyClosed;

		// Token: 0x04001B2F RID: 6959
		private ParameterHandling parHandling;

		// Token: 0x04001B30 RID: 6960
		private System.Timers.Timer timerLastXrayOn = new System.Timers.Timer();

		// Token: 0x04001B31 RID: 6961
		private bool reseasoning = false;

		// Token: 0x04001B32 RID: 6962
		private DBConnection dbConnection;

		// Token: 0x04001B33 RID: 6963
		private LiveCalibration liveCalibration;

		// Token: 0x04001B34 RID: 6964
		private bool testSuiteActive;

		// Token: 0x04001B35 RID: 6965
		private bool calibrated;

		// Token: 0x04001B36 RID: 6966
		private MeanPixelLogger meanPixelLogger;

		// Token: 0x04001B37 RID: 6967
		private PixelCorrection pixelCorrection;

		// Token: 0x04001B38 RID: 6968
		private CtrlMoveSource ctrlMoveSource;

		// Token: 0x04001B39 RID: 6969
		private Semaphore semaphoreXrayPositioning = new Semaphore(0, 1);

		// Token: 0x04001B3A RID: 6970
		private bool lastXRayPositioningTimedOut = false;

		// Token: 0x04001B3B RID: 6971
		private bool skipNextStatusCheck;

		// Token: 0x04001B3C RID: 6972
		private bool setOldState = false;

		// Token: 0x04001B3D RID: 6973
		private bool setErrorFlagForConnectFault = false;

		// Token: 0x04001B3E RID: 6974
		private XRayOperatingHours xrayOperatingHours;
	}
}
