using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using DatabaseConnection;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x02000232 RID: 562
	public class MachineState : INotifyPropertyChanged
	{
		// Token: 0x140000E0 RID: 224
		// (add) Token: 0x06002AAD RID: 10925 RVA: 0x0011C000 File Offset: 0x0011A200
		// (remove) Token: 0x06002AAE RID: 10926 RVA: 0x0011C038 File Offset: 0x0011A238
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x17000BAA RID: 2986
		// (get) Token: 0x06002AAF RID: 10927 RVA: 0x0011C06D File Offset: 0x0011A26D
		// (set) Token: 0x06002AB0 RID: 10928 RVA: 0x0011C075 File Offset: 0x0011A275
		public StartMessages StartMessage { get; set; }

		// Token: 0x17000BAB RID: 2987
		// (get) Token: 0x06002AB1 RID: 10929 RVA: 0x0011C080 File Offset: 0x0011A280
		// (set) Token: 0x06002AB2 RID: 10930 RVA: 0x0011C098 File Offset: 0x0011A298
		public MachineStates State
		{
			get
			{
				return this.state;
			}
			set
			{
				MachineStates machineStates = this.state;
				this.state = value;
				this.OnPropertyChanged("State");
				this.parHandling.WriteValue(Definition.machineStatePath, this.state.ToString());
				bool flag = machineStates != MachineStates.Inspecting && value == MachineStates.Inspecting;
				if (flag)
				{
					bool flag2 = this.Error == 0;
					if (flag2)
					{
						this.Inspecting = true;
					}
					this.OnStateChangedToInspecting(machineStates);
				}
				else
				{
					this.Inspecting = false;
				}
			}
		}

		// Token: 0x17000BAC RID: 2988
		// (get) Token: 0x06002AB3 RID: 10931 RVA: 0x0011C11A File Offset: 0x0011A31A
		// (set) Token: 0x06002AB4 RID: 10932 RVA: 0x0011C122 File Offset: 0x0011A322
		public MachineStates OldState { get; set; }

		// Token: 0x17000BAD RID: 2989
		// (get) Token: 0x06002AB5 RID: 10933 RVA: 0x0011C12C File Offset: 0x0011A32C
		// (set) Token: 0x06002AB6 RID: 10934 RVA: 0x0011C144 File Offset: 0x0011A344
		public bool KeyClosed
		{
			get
			{
				return this.keyClosed;
			}
			set
			{
				this.keyClosed = value;
				this.OnPropertyChanged("KeyClosed");
			}
		}

		// Token: 0x17000BAE RID: 2990
		// (get) Token: 0x06002AB7 RID: 10935 RVA: 0x0011C15C File Offset: 0x0011A35C
		// (set) Token: 0x06002AB8 RID: 10936 RVA: 0x0011C174 File Offset: 0x0011A374
		public bool XrayPower
		{
			get
			{
				return this.xrayPower;
			}
			set
			{
				this.xrayPower = value;
				this.OnPropertyChanged("XrayPower");
				if (value)
				{
					int num = 0;
					string s = this.parHandling.ReadPar(Definition.ioVersionPath);
					int.TryParse(s, out num);
					bool flag = num < Definition.wagoNewWiringStartVersion;
					if (flag)
					{
						this.XRayOnIndicator = true;
					}
				}
				else
				{
					this.XRayOnIndicator = false;
				}
			}
		}

		// Token: 0x17000BAF RID: 2991
		// (get) Token: 0x06002AB9 RID: 10937 RVA: 0x0011C1D8 File Offset: 0x0011A3D8
		// (set) Token: 0x06002ABA RID: 10938 RVA: 0x0011C1F0 File Offset: 0x0011A3F0
		public bool Inspecting
		{
			get
			{
				return this.inspecting;
			}
			set
			{
				this.inspecting = value;
				this.OnPropertyChanged("Inspecting");
			}
		}

		// Token: 0x17000BB0 RID: 2992
		// (get) Token: 0x06002ABB RID: 10939 RVA: 0x0011C208 File Offset: 0x0011A408
		// (set) Token: 0x06002ABC RID: 10940 RVA: 0x0011C220 File Offset: 0x0011A420
		public int Error
		{
			get
			{
				return this.error;
			}
			set
			{
				bool flag = this.error != value;
				if (flag)
				{
					this.error = value;
					this.OnPropertyChanged("Error");
					bool flag2 = value == 0 && this.State == MachineStates.Inspecting;
					if (flag2)
					{
						this.Inspecting = true;
					}
					else
					{
						this.Inspecting = false;
					}
				}
			}
		}

		// Token: 0x17000BB1 RID: 2993
		// (get) Token: 0x06002ABD RID: 10941 RVA: 0x0011C278 File Offset: 0x0011A478
		// (set) Token: 0x06002ABE RID: 10942 RVA: 0x0011C290 File Offset: 0x0011A490
		public bool ErrorsBeltStopForSignalLamp
		{
			get
			{
				return this.errorsBeltStopForSignalLamp;
			}
			set
			{
				bool flag = this.errorsBeltStopForSignalLamp != value;
				if (flag)
				{
					this.errorsBeltStopForSignalLamp = value;
					this.OnPropertyChanged("ErrorsBeltStopForSignalLamp");
				}
			}
		}

		// Token: 0x17000BB2 RID: 2994
		// (get) Token: 0x06002ABF RID: 10943 RVA: 0x0011C2C4 File Offset: 0x0011A4C4
		// (set) Token: 0x06002AC0 RID: 10944 RVA: 0x0011C2DC File Offset: 0x0011A4DC
		public bool SourceXrayOn
		{
			get
			{
				return this.sourceXrayOn;
			}
			set
			{
				bool flag = this.sourceXrayOn != value;
				if (flag)
				{
					int num = 0;
					string s = this.parHandling.ReadPar(Definition.ioVersionPath);
					int.TryParse(s, out num);
					this.sourceXrayOn = value;
					this.OnPropertyChanged("SourceXrayOn");
					bool flag2 = num >= Definition.wagoNewWiringStartVersion;
					if (flag2)
					{
						this.XRayOnIndicator = value;
					}
				}
			}
		}

		// Token: 0x17000BB3 RID: 2995
		// (get) Token: 0x06002AC1 RID: 10945 RVA: 0x0011C344 File Offset: 0x0011A544
		// (set) Token: 0x06002AC2 RID: 10946 RVA: 0x0011C35C File Offset: 0x0011A55C
		public bool XRayOnIndicator
		{
			get
			{
				return this.xrayOnIndicator;
			}
			set
			{
				bool flag = this.xrayOnIndicator != value;
				if (flag)
				{
					this.xrayOnIndicator = value;
					this.OnPropertyChanged("XRayOnIndicator");
				}
			}
		}

		// Token: 0x140000E1 RID: 225
		// (add) Token: 0x06002AC3 RID: 10947 RVA: 0x0011C390 File Offset: 0x0011A590
		// (remove) Token: 0x06002AC4 RID: 10948 RVA: 0x0011C3C8 File Offset: 0x0011A5C8
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<MachineStates> StateChangedToInspecting;

		// Token: 0x06002AC5 RID: 10949 RVA: 0x0011C400 File Offset: 0x0011A600
		public MachineState(XrayState xrayState, ParameterHandling parHandling, DBConnection dbConnection)
		{
			this.Error = 0;
			this.ErrorsBeltStopForSignalLamp = false;
			this.parHandling = parHandling;
			this.dbConnection = dbConnection;
			this.xrayState = xrayState;
			this.StartMessage = new StartMessages();
			bool flag = xrayState != null;
			if (flag)
			{
				xrayState.ConveyorRunningOff += this.Xraystate_ConveyorRunningOff;
				xrayState.ConveyorRunningOn += this.Xraystate_ConveyorRunningOn;
				xrayState.XRayPowerOn += this.Xraystate_XRayPowerOn;
				xrayState.XRayPowerOff += this.Xraystate_XRayPowerOff;
				xrayState.KeyClosed += this.xrayState_KeyClosed;
				xrayState.KeyOpened += this.xrayState_KeyOpened;
				xrayState.Reseasoning += this.Xraystate_Reseasoning;
				xrayState.SetOldStateForNoConnection += this.xraystate_SetOldStateForNoConnection;
				xrayState.SetActualStateForNoConnection += this.xraystate_SetActualStateForNoConnection;
				xrayState.CalibrationDone += this.Xraystate_CalibrationDone;
				xrayState.RemainingReseasonTimeChanged += this.Xraystate_RemainingReseasonTimeChanged;
				xrayState.TestSuiteActiveChanged += this.xrayState_DemoModeChanged;
				xrayState.SourceXRayOnChanged += this.xrayState_SourceXRayOnChanged;
				xrayState.NoConnectionToSourceOrDetector += this.xrayState_NoConnectionToSourceOrDetector;
				xrayState.StateForNoBeltStartOnError += this.xrayState_StateForNoBeltStartOnError;
			}
			Thread thread = new Thread(new ThreadStart(this.WriteLampParameter));
			thread.Name = "LampParameter";
			this.lampThreadRunning = true;
			thread.Start();
		}

		// Token: 0x06002AC6 RID: 10950 RVA: 0x0011C5A7 File Offset: 0x0011A7A7
		private void xrayState_KeyOpened()
		{
			this.KeyClosed = false;
			this.State = MachineStates.ConveyorOnKeyOpened;
			this.StartMessage.State = this.State;
		}

		// Token: 0x06002AC7 RID: 10951 RVA: 0x0011C5CC File Offset: 0x0011A7CC
		private void xrayState_KeyClosed()
		{
			this.KeyClosed = true;
			this.State = MachineStates.ConveyorOnKeyClosedXrayOff;
			this.StartMessage.State = this.State;
		}

		// Token: 0x06002AC8 RID: 10952 RVA: 0x0011C5F4 File Offset: 0x0011A7F4
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x06002AC9 RID: 10953 RVA: 0x0011C621 File Offset: 0x0011A821
		private void Xraystate_RemainingReseasonTimeChanged(int remainingTime)
		{
			this.State = MachineStates.Reseasoning;
			this.StartMessage.RemainingReseasonTime = remainingTime;
			this.StartMessage.State = MachineStates.Reseasoning;
		}

		// Token: 0x06002ACA RID: 10954 RVA: 0x0011C646 File Offset: 0x0011A846
		private void Xraystate_Reseasoning()
		{
			this.XrayPower = true;
			this.StartMessage.RemainingReseasonTime = 0;
			this.State = MachineStates.Reseasoning;
			this.StartMessage.State = this.State;
		}

		// Token: 0x06002ACB RID: 10955 RVA: 0x0011C678 File Offset: 0x0011A878
		private void Xraystate_XRayPowerOff()
		{
			this.XrayPower = false;
			this.Inspecting = false;
			bool flag = this.State > MachineStates.ConveyorOff;
			if (flag)
			{
				this.State = MachineStates.ConveyorOnKeyOpened;
				this.StartMessage.State = this.State;
			}
		}

		// Token: 0x06002ACC RID: 10956 RVA: 0x0011C6C0 File Offset: 0x0011A8C0
		private void Xraystate_CalibrationDone()
		{
			bool flag = this.State != MachineStates.AutoAdjustingXRay;
			if (flag)
			{
				this.State = MachineStates.Inspecting;
			}
			this.StartMessage.State = this.State;
		}

		// Token: 0x06002ACD RID: 10957 RVA: 0x0011C6FC File Offset: 0x0011A8FC
		private void Xraystate_XRayPowerOn()
		{
			this.XrayPower = true;
			this.Inspecting = false;
			bool flag = this.State != MachineStates.AutoAdjustingXRay;
			if (flag)
			{
				this.State = MachineStates.XrayOnCalibrationNotDone;
			}
			this.StartMessage.State = this.State;
		}

		// Token: 0x06002ACE RID: 10958 RVA: 0x0011C748 File Offset: 0x0011A948
		private void Xraystate_ConveyorRunningOn()
		{
			bool flag = !this.keyClosed;
			if (flag)
			{
				this.State = MachineStates.ConveyorOnKeyOpened;
				this.StartMessage.State = this.State;
			}
			else
			{
				this.State = MachineStates.ConveyorOnKeyClosedXrayOff;
				this.StartMessage.State = this.State;
			}
		}

		// Token: 0x06002ACF RID: 10959 RVA: 0x0011C79D File Offset: 0x0011A99D
		private void Xraystate_ConveyorRunningOff()
		{
			this.State = MachineStates.ConveyorOff;
			this.StartMessage.State = this.State;
		}

		// Token: 0x06002AD0 RID: 10960 RVA: 0x0011C7BC File Offset: 0x0011A9BC
		private void xrayState_DemoModeChanged(bool activated)
		{
			if (activated)
			{
				this.State = MachineStates.TestSuiteActive;
			}
			else
			{
				this.State = MachineStates.ConveyorOff;
			}
			this.StartMessage.State = this.State;
		}

		// Token: 0x06002AD1 RID: 10961 RVA: 0x0011C7F4 File Offset: 0x0011A9F4
		private void xraystate_SetOldStateForNoConnection()
		{
			this.OldState = this.State;
		}

		// Token: 0x06002AD2 RID: 10962 RVA: 0x0011C804 File Offset: 0x0011AA04
		private void xraystate_SetActualStateForNoConnection()
		{
			bool flag = this.OldState == MachineStates.XrayOnCalibrationNotDone || this.OldState == MachineStates.Inspecting;
			if (flag)
			{
				this.State = MachineStates.ConveyorOnKeyClosedXrayOff;
			}
			else
			{
				this.State = this.OldState;
			}
			this.StartMessage.State = this.State;
		}

		// Token: 0x06002AD3 RID: 10963 RVA: 0x0011C858 File Offset: 0x0011AA58
		private void WriteLampParameter()
		{
			bool flag = false;
			while (this.lampThreadRunning)
			{
				bool flag2 = this.State == MachineStates.Inspecting && this.Error == 0 && this.parHandling.ReadPar(XRayOnProcessing.xrayEnablePath) == "1" && !this.ErrorsBeltStopForSignalLamp;
				if (flag2)
				{
					this.parHandling.WriteValue("/hardware/ios/DO_01/Value", "0", false);
					this.parHandling.WriteValue("/hardware/ios/DO_00/Value", "1", false);
				}
				else
				{
					bool flag3 = this.State == MachineStates.PreAuditCheck || this.State == MachineStates.AuditCheck;
					if (flag3)
					{
						bool flag4 = this.Error == 0;
						if (flag4)
						{
							this.parHandling.WriteValue("/hardware/ios/DO_01/Value", "0", false);
						}
						else
						{
							this.parHandling.WriteValue("/hardware/ios/DO_01/Value", "1", false);
						}
						this.dbConnection.GetUseAuditCheckLamp(ref flag);
						bool flag5 = !flag;
						if (flag5)
						{
							while (this.State == MachineStates.PreAuditCheck || this.State == MachineStates.AuditCheck)
							{
								this.parHandling.WriteValue("/hardware/ios/DO_00/Value", "1", false);
								Thread.Sleep(this.blinkInterval);
								this.parHandling.WriteValue("/hardware/ios/DO_00/Value", "0", false);
								Thread.Sleep(this.blinkInterval);
								bool flag6 = !this.lampThreadRunning;
								if (flag6)
								{
									break;
								}
							}
						}
						else
						{
							while (this.State == MachineStates.PreAuditCheck)
							{
								this.parHandling.WriteValue("/hardware/ios/DO_02/Value", "1", false);
								Thread.Sleep(1000);
							}
							while (this.State == MachineStates.AuditCheck)
							{
								this.parHandling.WriteValue("/hardware/ios/DO_02/Value", "1", false);
								Thread.Sleep(this.blinkInterval);
								this.parHandling.WriteValue("/hardware/ios/DO_02/Value", "0", false);
								Thread.Sleep(this.blinkInterval);
							}
						}
					}
					else
					{
						this.parHandling.WriteValue("/hardware/ios/DO_01/Value", "1", false);
						this.parHandling.WriteValue("/hardware/ios/DO_00/Value", "0", false);
					}
				}
				this.parHandling.WriteValue(Definition.machineErrorPath, this.Error.ToString(), false);
				Thread.Sleep(1000);
			}
		}

		// Token: 0x06002AD4 RID: 10964 RVA: 0x0011CAD1 File Offset: 0x0011ACD1
		public void AbortLampThread()
		{
			this.lampThreadRunning = false;
		}

		// Token: 0x06002AD5 RID: 10965 RVA: 0x0011CADC File Offset: 0x0011ACDC
		protected virtual void OnStateChangedToInspecting(MachineStates oldState)
		{
			Action<MachineStates> stateChangedToInspecting = this.StateChangedToInspecting;
			bool flag = stateChangedToInspecting != null;
			if (flag)
			{
				stateChangedToInspecting(oldState);
			}
		}

		// Token: 0x06002AD6 RID: 10966 RVA: 0x0011CB04 File Offset: 0x0011AD04
		private void xrayState_SourceXRayOnChanged(bool enabled)
		{
			bool flag = enabled && !this.SourceXrayOn;
			if (flag)
			{
				this.SourceXrayOn = true;
			}
			bool flag2 = !enabled && this.SourceXrayOn;
			if (flag2)
			{
				this.SourceXrayOn = false;
			}
		}

		// Token: 0x06002AD7 RID: 10967 RVA: 0x0011CB45 File Offset: 0x0011AD45
		private void xrayState_NoConnectionToSourceOrDetector(bool message)
		{
			this.XrayPower = false;
			this.State = MachineStates.NoConnectToSourceOrDetector;
			this.StartMessage.State = this.State;
			StartMessages.NoConnectionMessage = message;
		}

		// Token: 0x06002AD8 RID: 10968 RVA: 0x0011CB72 File Offset: 0x0011AD72
		private void xrayState_StateForNoBeltStartOnError()
		{
			this.State = MachineStates.StateForNoBeltStartOnError;
			this.StartMessage.State = this.State;
		}

		// Token: 0x04001AA9 RID: 6825
		private const string auditCheckLampPath = "/hardware/ios/DO_02/Value";

		// Token: 0x04001AAA RID: 6826
		private const string orangeLampPath = "/hardware/ios/DO_01/Value";

		// Token: 0x04001AAB RID: 6827
		private const string greenLampPath = "/hardware/ios/DO_00/Value";

		// Token: 0x04001AAC RID: 6828
		private const string customerErrorPath = "/hardware/ios/DO_17/Value";

		// Token: 0x04001AAD RID: 6829
		private const string ioAddressPath = "/hardware/ios/Address/Value";

		// Token: 0x04001AAE RID: 6830
		public static readonly string usbDriveLetterPath = "/machine/USBDriveLetter/Value";

		// Token: 0x04001AAF RID: 6831
		public static readonly string lastXRayOnPath = "/machine/LastXrayOn/Value";

		// Token: 0x04001AB0 RID: 6832
		private int blinkInterval = 1000;

		// Token: 0x04001AB1 RID: 6833
		private const int machineErrorAddress = 802;

		// Token: 0x04001AB2 RID: 6834
		private bool xrayPower;

		// Token: 0x04001AB3 RID: 6835
		private bool keyClosed;

		// Token: 0x04001AB4 RID: 6836
		private bool inspecting;

		// Token: 0x04001AB5 RID: 6837
		private ParameterHandling parHandling;

		// Token: 0x04001AB6 RID: 6838
		private DBConnection dbConnection;

		// Token: 0x04001AB7 RID: 6839
		private XrayState xrayState;

		// Token: 0x04001AB8 RID: 6840
		private MachineStates state;

		// Token: 0x04001AB9 RID: 6841
		private bool lampThreadRunning;

		// Token: 0x04001ABA RID: 6842
		private int error;

		// Token: 0x04001ABB RID: 6843
		private bool errorsBeltStopForSignalLamp;

		// Token: 0x04001ABC RID: 6844
		private bool sourceXrayOn;

		// Token: 0x04001ABD RID: 6845
		private bool xrayOnIndicator;
	}
}
