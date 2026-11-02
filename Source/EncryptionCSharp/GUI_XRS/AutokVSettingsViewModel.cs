using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
using Common;
using DatabaseConnection;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x020000B2 RID: 178
	public class AutokVSettingsViewModel : INotifyPropertyChanged
	{
		// Token: 0x14000058 RID: 88
		// (add) Token: 0x060011D2 RID: 4562 RVA: 0x0006B8B0 File Offset: 0x00069AB0
		// (remove) Token: 0x060011D3 RID: 4563 RVA: 0x0006B8E8 File Offset: 0x00069AE8
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x17000567 RID: 1383
		// (get) Token: 0x060011D4 RID: 4564 RVA: 0x0006B91D File Offset: 0x00069B1D
		// (set) Token: 0x060011D5 RID: 4565 RVA: 0x0006B925 File Offset: 0x00069B25
		public BoundLocalizedText StrVoltage { get; set; }

		// Token: 0x17000568 RID: 1384
		// (get) Token: 0x060011D6 RID: 4566 RVA: 0x0006B92E File Offset: 0x00069B2E
		// (set) Token: 0x060011D7 RID: 4567 RVA: 0x0006B936 File Offset: 0x00069B36
		public BoundLocalizedText StrCurrent { get; set; }

		// Token: 0x17000569 RID: 1385
		// (get) Token: 0x060011D8 RID: 4568 RVA: 0x0006B93F File Offset: 0x00069B3F
		// (set) Token: 0x060011D9 RID: 4569 RVA: 0x0006B947 File Offset: 0x00069B47
		public bool AutoKVChangedToDisabled { get; set; }

		// Token: 0x1700056A RID: 1386
		// (get) Token: 0x060011DA RID: 4570 RVA: 0x0006B950 File Offset: 0x00069B50
		// (set) Token: 0x060011DB RID: 4571 RVA: 0x0006B968 File Offset: 0x00069B68
		public bool ManualkVDeactivation
		{
			get
			{
				return this.manualkVDeactivation;
			}
			set
			{
				bool flag = this.manualkVDeactivation != value;
				if (flag)
				{
					this.manualkVDeactivation = value;
					this.OnPropertyChanged("ManualkVDeactivation");
				}
			}
		}

		// Token: 0x1700056B RID: 1387
		// (get) Token: 0x060011DC RID: 4572 RVA: 0x0006B99C File Offset: 0x00069B9C
		// (set) Token: 0x060011DD RID: 4573 RVA: 0x0006B9B4 File Offset: 0x00069BB4
		public bool IsManualkVVisible
		{
			get
			{
				return this.bIsManualkVVisible;
			}
			set
			{
				this.bIsManualkVVisible = value;
				this.OnPropertyChanged("IsManualkVVisible");
			}
		}

		// Token: 0x1700056C RID: 1388
		// (get) Token: 0x060011DE RID: 4574 RVA: 0x0006B9CC File Offset: 0x00069BCC
		// (set) Token: 0x060011DF RID: 4575 RVA: 0x0006B9E4 File Offset: 0x00069BE4
		public bool IsAutokVVisible
		{
			get
			{
				return this.bIsAutoKVVisible;
			}
			set
			{
				this.bIsAutoKVVisible = value;
				this.OnPropertyChanged("IsAutokVVisible");
			}
		}

		// Token: 0x1700056D RID: 1389
		// (get) Token: 0x060011E0 RID: 4576 RVA: 0x0006B9FC File Offset: 0x00069BFC
		// (set) Token: 0x060011E1 RID: 4577 RVA: 0x0006BA14 File Offset: 0x00069C14
		public double VoltageSetpoint
		{
			get
			{
				return this.voltageSetpoint;
			}
			set
			{
				bool flag = this.voltageSetpoint != value;
				if (flag)
				{
					double num = Convert.ToDouble(this.currentSetpoint);
					string a = this.parHandling.ReadPar("/hardware/xraysource/ModelNumber/Value");
					bool flag2 = value < 40.0 && value > 0.0;
					if (flag2)
					{
						value = 40.0;
					}
					else
					{
						bool flag3 = value > 100.0;
						if (flag3)
						{
							value = 100.0;
						}
					}
					bool flag4 = a == "IXS100BP300P381";
					double num2;
					if (flag4)
					{
						num2 = Math.Round(300.0 / value, 2);
						bool flag5 = num2 > 6.0;
						if (flag5)
						{
							num2 = 6.0;
						}
					}
					else
					{
						bool flag6 = a == "IXS050SE100P214";
						if (flag6)
						{
							bool flag7 = value > 50.0;
							if (flag7)
							{
								value = 50.0;
							}
							num2 = Math.Round(100.0 / value, 2);
							bool flag8 = num2 > 2.5;
							if (flag8)
							{
								num2 = 2.5;
							}
						}
						else
						{
							num2 = Math.Round(200.0 / value, 2);
							bool flag9 = num2 > 5.0;
							if (flag9)
							{
								num2 = 5.0;
							}
						}
					}
					bool flag10 = num2 < 1.0;
					if (flag10)
					{
						num2 = 1.0;
					}
					bool flag11 = value < this.voltageSetpoint;
					if (flag11)
					{
						this.voltageSetpoint = value;
						this.parHandling.WriteValue("/hardware/xraysource/VoltageSetpoint/Value", Convert.ToString(this.voltageSetpoint));
						this.CurrentSetpoint = num2;
					}
					else
					{
						this.CurrentSetpoint = num2;
						this.voltageSetpoint = value;
						this.parHandling.WriteValue("/hardware/xraysource/VoltageSetpoint/Value", Convert.ToString(this.voltageSetpoint));
					}
					this.OnPropertyChanged("VoltageSetpoint");
				}
			}
		}

		// Token: 0x1700056E RID: 1390
		// (get) Token: 0x060011E2 RID: 4578 RVA: 0x0006BC18 File Offset: 0x00069E18
		// (set) Token: 0x060011E3 RID: 4579 RVA: 0x0006BC30 File Offset: 0x00069E30
		public double VoltageSetpointDisplay
		{
			get
			{
				return this.voltageSetpointDisplay;
			}
			set
			{
				this.voltageSetpointDisplay = value;
				this.OnPropertyChanged("VoltageSetpointDisplay");
			}
		}

		// Token: 0x1700056F RID: 1391
		// (get) Token: 0x060011E4 RID: 4580 RVA: 0x0006BC48 File Offset: 0x00069E48
		// (set) Token: 0x060011E5 RID: 4581 RVA: 0x0006BC60 File Offset: 0x00069E60
		public double CurrentSetpointDisplay
		{
			get
			{
				return this.currentSetpointDisplay;
			}
			set
			{
				this.currentSetpointDisplay = value;
				this.OnPropertyChanged("CurrentSetpointDisplay");
			}
		}

		// Token: 0x17000570 RID: 1392
		// (get) Token: 0x060011E6 RID: 4582 RVA: 0x0006BC78 File Offset: 0x00069E78
		// (set) Token: 0x060011E7 RID: 4583 RVA: 0x0006BC90 File Offset: 0x00069E90
		public double CurrentSetpoint
		{
			get
			{
				return this.currentSetpoint;
			}
			set
			{
				bool flag = this.currentSetpoint != value;
				if (flag)
				{
					double num = 5.0;
					double num2 = 5.0;
					double num3 = Convert.ToDouble(this.voltageSetpoint);
					string a = this.parHandling.ReadPar("/hardware/xraysource/ModelNumber/Value");
					bool flag2 = num3 > 0.0;
					if (flag2)
					{
						bool flag3 = a == "IXS100BP300P381" && num3 > 0.0;
						if (flag3)
						{
							num = Math.Round(300.0 / num3, 2);
							num2 = 6.0;
						}
						else
						{
							num = Math.Round(200.0 / num3, 2);
							num2 = 5.0;
						}
					}
					bool flag4 = num > num2;
					if (flag4)
					{
						num = num2;
					}
					bool flag5 = value < 1.0 && value > 0.0;
					if (flag5)
					{
						this.currentSetpoint = 1.0;
					}
					else
					{
						bool flag6 = value > num;
						if (flag6)
						{
							this.currentSetpoint = num;
						}
						else
						{
							this.currentSetpoint = value;
						}
					}
					this.OnPropertyChanged("CurrentSetpoint");
					this.parHandling.WriteValue("/hardware/xraysource/CurrentSetpoint/Value", Convert.ToString(this.currentSetpoint));
				}
			}
		}

		// Token: 0x17000571 RID: 1393
		// (get) Token: 0x060011E8 RID: 4584 RVA: 0x0006BDE1 File Offset: 0x00069FE1
		// (set) Token: 0x060011E9 RID: 4585 RVA: 0x0006BDE9 File Offset: 0x00069FE9
		public RelayCommand ChangeVoltageSetpointCommand { get; set; }

		// Token: 0x17000572 RID: 1394
		// (get) Token: 0x060011EA RID: 4586 RVA: 0x0006BDF2 File Offset: 0x00069FF2
		// (set) Token: 0x060011EB RID: 4587 RVA: 0x0006BDFA File Offset: 0x00069FFA
		public RelayCommand ChangeCurrentSetpointCommand { get; set; }

		// Token: 0x17000573 RID: 1395
		// (get) Token: 0x060011EC RID: 4588 RVA: 0x0006BE04 File Offset: 0x0006A004
		// (set) Token: 0x060011ED RID: 4589 RVA: 0x0006BE1C File Offset: 0x0006A01C
		public int AutokVMode
		{
			get
			{
				return this.autokVMode;
			}
			set
			{
				bool flag = value != this.autokVMode;
				if (flag)
				{
					this.autokVMode = value;
					bool flag2 = this.autokVMode == 0;
					if (flag2)
					{
						this.dbConnection.SetAutoKVAdjustment(0U);
						this.parHandling.WriteValue(Definition.autoKVAdjustmentPath, "0");
					}
					else
					{
						bool flag3 = this.autokVMode == 2;
						if (flag3)
						{
							this.dbConnection.SetAutoKVAdjustment(2U);
							this.parHandling.WriteValue(Definition.autoKVAdjustmentPath, "2");
						}
						else
						{
							this.dbConnection.SetAutoKVAdjustment(1U);
							this.parHandling.WriteValue(Definition.autoKVAdjustmentPath, "1");
						}
					}
					this.OnPropertyChanged("AutokVMode");
				}
			}
		}

		// Token: 0x17000574 RID: 1396
		// (get) Token: 0x060011EE RID: 4590 RVA: 0x0006BEE0 File Offset: 0x0006A0E0
		// (set) Token: 0x060011EF RID: 4591 RVA: 0x0006BEF8 File Offset: 0x0006A0F8
		public bool FullPowerModeEnabled
		{
			get
			{
				return this.fullPowerModeEnabled;
			}
			set
			{
				bool flag = value != this.fullPowerModeEnabled;
				if (flag)
				{
					this.fullPowerModeEnabled = value;
					bool flag2 = this.fullPowerModeEnabled;
					if (flag2)
					{
						this.AutokVMode = 1;
						this.checkTestcardSimIfAutokVIsDeactiated();
					}
					this.OnPropertyChanged("FullPowerModeEnabled");
				}
			}
		}

		// Token: 0x17000575 RID: 1397
		// (get) Token: 0x060011F0 RID: 4592 RVA: 0x0006BF48 File Offset: 0x0006A148
		// (set) Token: 0x060011F1 RID: 4593 RVA: 0x0006BF60 File Offset: 0x0006A160
		public bool EcoModeEnabled
		{
			get
			{
				return this.ecoModeEnabled;
			}
			set
			{
				bool flag = value != this.ecoModeEnabled;
				if (flag)
				{
					this.ecoModeEnabled = value;
					bool flag2 = this.ecoModeEnabled;
					if (flag2)
					{
						this.AutokVMode = 2;
						this.checkTestcardSimIfAutokVIsDeactiated();
					}
					this.OnPropertyChanged("EcoModeEnabled");
				}
			}
		}

		// Token: 0x17000576 RID: 1398
		// (get) Token: 0x060011F2 RID: 4594 RVA: 0x0006BFB0 File Offset: 0x0006A1B0
		// (set) Token: 0x060011F3 RID: 4595 RVA: 0x0006BFC8 File Offset: 0x0006A1C8
		public bool ManualModeEnabled
		{
			get
			{
				return this.manualModeEnabled;
			}
			set
			{
				bool flag = value != this.manualModeEnabled;
				if (flag)
				{
					bool flag2 = !this.manualModeEnabled && value;
					if (flag2)
					{
						Filter.AutoKVChangedToDisabled = true;
					}
					else
					{
						Filter.AutoKVChangedToDisabled = false;
					}
					this.manualModeEnabled = value;
					bool flag3 = this.manualModeEnabled;
					if (flag3)
					{
						this.AutokVMode = 0;
						this.checkTestcardSimIfAutokVIsDeactiated();
					}
					this.OnPropertyChanged("ManualModeEnabled");
				}
			}
		}

		// Token: 0x17000577 RID: 1399
		// (get) Token: 0x060011F4 RID: 4596 RVA: 0x0006C036 File Offset: 0x0006A236
		// (set) Token: 0x060011F5 RID: 4597 RVA: 0x0006C03E File Offset: 0x0006A23E
		public RelayCommand CleanUpCommand { get; set; }

		// Token: 0x060011F6 RID: 4598 RVA: 0x0006C048 File Offset: 0x0006A248
		public AutokVSettingsViewModel(DBConnection dbConnection, ParameterHandling parHandling, int imageGroup, bool showDialogWindows, ProductManager productManager, AccessRightsManager accessManager, MachineState machineState, bool isOnlymanualkVActive, TestcardSimService testcardSimService)
		{
			this.dbConnection = dbConnection;
			this.parHandling = parHandling;
			this.imageGroup = imageGroup;
			this.showDialogWindows = showDialogWindows;
			this.machineState = machineState;
			this.productManager = productManager;
			this.accessManager = accessManager;
			this.testcardSimService = testcardSimService;
			this.ChangeVoltageSetpointCommand = new RelayCommand(new Action<object>(this.ChangeVoltageSetpoint));
			this.voltageSetpoint = Convert.ToDouble(parHandling.ReadPar("/hardware/xraysource/VoltageSetpoint/Value"));
			this.VoltageSetpointDisplay = this.voltageSetpoint;
			this.ChangeCurrentSetpointCommand = new RelayCommand(new Action<object>(this.ChangeCurrentSetpoint));
			this.currentSetpoint = Convert.ToDouble(parHandling.ReadPar("/hardware/xraysource/CurrentSetpoint/Value"));
			this.CurrentSetpointDisplay = this.currentSetpoint;
			this.showTestcardSimMsg = false;
			uint num = 0U;
			dbConnection.GetAutoKVAdjustment(ref num);
			bool flag = num == 0U;
			if (flag)
			{
				this.ManualModeEnabled = true;
			}
			else
			{
				bool flag2 = num == 2U;
				if (flag2)
				{
					this.EcoModeEnabled = true;
				}
				else
				{
					this.FullPowerModeEnabled = true;
				}
			}
			this.IsManualkVVisible = (accessManager.CurrentUser.Rights >= BuiltInPriviledges.Admin);
			MachineMode machineMode = MachineMode.Standard;
			dbConnection.GetMachineMode(ref machineMode);
			bool flag3 = machineMode == MachineMode.Bulk;
			if (flag3)
			{
				isOnlymanualkVActive = true;
			}
			this.IsAutokVVisible = !isOnlymanualkVActive;
			bool flag4 = isOnlymanualkVActive;
			if (flag4)
			{
				this.AutokVMode = 0;
				this.IsManualkVVisible = false;
			}
			this.ManualkVDeactivation = true;
			this.timerforManualkV = new Timer(new TimerCallback(this.CheckStateforManualkV), null, 0, 1000);
			this.RegisterLocalizedText();
			this.initialized = true;
			this.CleanUpCommand = new RelayCommand(new Action<object>(this.CleanUp));
		}

		// Token: 0x060011F7 RID: 4599 RVA: 0x0006C200 File Offset: 0x0006A400
		private void CheckStateforManualkV(object obj)
		{
			bool flag = this.machineState.State == MachineStates.Inspecting && this.accessManager.CurrentUser.Rights >= BuiltInPriviledges.Admin && this.ManualModeEnabled;
			if (flag)
			{
				this.ManualkVDeactivation = false;
			}
			else
			{
				this.ManualkVDeactivation = true;
			}
			this.VoltageSetpointDisplay = Convert.ToDouble(this.parHandling.ReadPar("/hardware/xraysource/VoltageSetpoint/Value"));
			this.CurrentSetpointDisplay = Convert.ToDouble(this.parHandling.ReadPar("/hardware/xraysource/CurrentSetpoint/Value"));
		}

		// Token: 0x060011F8 RID: 4600 RVA: 0x0006C28C File Offset: 0x0006A48C
		private void numKeyboard_InputVoltageSetpoint(object sender, KeyboardInputEventArgs e)
		{
			try
			{
				string s = e.Text.Replace('.', ',');
				double num = double.Parse(s, new CultureInfo("de-DE"));
				this.VoltageSetpoint = num;
				this.VoltageSetpointDisplay = num;
				string value = this.parHandling.ReadPar(XRayOnProcessing.xrayModelNumberPath);
				bool flag = XRayOnProcessing.modelNr60KV200WP760.Contains(value);
				if (flag)
				{
					bool flag2 = num < 50.0;
					if (flag2)
					{
						this.CurrentSetpoint = 2.0;
						this.CurrentSetpointDisplay = 2.0;
					}
				}
				this.numKeyboard.TextInputKeyboard -= this.numKeyboard_InputVoltageSetpoint;
			}
			catch
			{
			}
		}

		// Token: 0x060011F9 RID: 4601 RVA: 0x0006C354 File Offset: 0x0006A554
		public void ChangeVoltageSetpoint(object onj)
		{
			string oldValue = this.parHandling.ReadPar("/hardware/xraysource/VoltageSetpoint/Value");
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtXRayVoltage"), oldValue, NumKeyboardType.TypeDouble);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_InputVoltageSetpoint;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x060011FA RID: 4602 RVA: 0x0006C3B4 File Offset: 0x0006A5B4
		private void numKeyboard_InputCurrentSetpoint(object sender, KeyboardInputEventArgs e)
		{
			try
			{
				string s = e.Text.Replace('.', ',');
				double num = double.Parse(s, new CultureInfo("de-DE"));
				string value = this.parHandling.ReadPar(XRayOnProcessing.xrayModelNumberPath);
				bool flag = XRayOnProcessing.modelNr60KV200WP760.Contains(value);
				if (flag)
				{
					bool flag2 = this.VoltageSetpoint < 50.0;
					if (flag2)
					{
						this.CurrentSetpoint = 2.0;
						this.CurrentSetpointDisplay = 2.0;
					}
					else
					{
						this.CurrentSetpoint = num;
						this.CurrentSetpointDisplay = num;
					}
				}
				else
				{
					this.CurrentSetpoint = num;
					this.CurrentSetpointDisplay = num;
				}
				this.numKeyboard.TextInputKeyboard -= this.numKeyboard_InputCurrentSetpoint;
			}
			catch
			{
			}
		}

		// Token: 0x060011FB RID: 4603 RVA: 0x0006C498 File Offset: 0x0006A698
		public void ChangeCurrentSetpoint(object onj)
		{
			string oldValue = this.parHandling.ReadPar("/hardware/xraysource/CurrentSetpoint/Value");
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtXRayCurrent"), oldValue, NumKeyboardType.TypeDouble);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_InputCurrentSetpoint;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x060011FC RID: 4604 RVA: 0x0006C4F8 File Offset: 0x0006A6F8
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x060011FD RID: 4605 RVA: 0x0006C525 File Offset: 0x0006A725
		private void RegisterLocalizedText()
		{
			this.StrVoltage = new BoundLocalizedText("txtXRayVoltage");
			this.StrCurrent = new BoundLocalizedText("txtXRayCurrent");
		}

		// Token: 0x060011FE RID: 4606 RVA: 0x0006C54A File Offset: 0x0006A74A
		public void DeregisterLocalizedText()
		{
			MainWindow.localizationManager.Deregister(this.StrVoltage);
			MainWindow.localizationManager.Deregister(this.StrCurrent);
		}

		// Token: 0x060011FF RID: 4607 RVA: 0x0006C570 File Offset: 0x0006A770
		private void checkTestcardSimIfAutokVIsDeactiated()
		{
			bool flag = this.parHandling.ReadPar(Definition.testcardSimulationPath) == "1" && this.showTestcardSimMsg;
			if (flag)
			{
				bool flag2 = (this.ecoModeEnabled || this.manualModeEnabled) && (this.parHandling.ReadPar(Definition.simulationAfterTimeIntervall) == "1" || this.parHandling.ReadPar(Definition.simulationAfterAutolearn) == "1");
				if (flag2)
				{
					bool flag3 = Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtDeactivateTestcardSim"), DialogStyle.Warning, DialogButtons.OK) == Dialog.DialogResults.Ok;
					if (flag3)
					{
						this.parHandling.WriteValue(Definition.simulationAfterAutolearn, "0");
						this.parHandling.WriteValue(Definition.simulationAfterTimeIntervall, "0");
						this.parHandling.WriteValue(Autotrain.nAutolearnPath, "5");
						this.testcardSimService.SetProperties();
					}
				}
				else
				{
					bool flag4 = this.fullPowerModeEnabled && (this.parHandling.ReadPar(Definition.simulationAfterTimeIntervall) == "0" || this.parHandling.ReadPar(Definition.simulationAfterAutolearn) == "0");
					if (flag4)
					{
						bool flag5 = Dialog.Show("", LocalizationManager.Localize("txtActivateTestcardSim"), DialogStyle.Info, DialogButtons.OKCancel) == Dialog.DialogResults.Ok;
						if (flag5)
						{
							this.parHandling.WriteValue(Definition.simulationAfterAutolearn, "1");
							this.parHandling.WriteValue(Definition.simulationAfterTimeIntervall, "1");
							this.parHandling.WriteValue(Autotrain.nAutolearnPath, "10");
							this.testcardSimService.SetProperties();
						}
					}
				}
			}
			this.showTestcardSimMsg = true;
		}

		// Token: 0x06001200 RID: 4608 RVA: 0x0006C738 File Offset: 0x0006A938
		private void CleanUp(object obj)
		{
			bool flag = this.timerforManualkV != null;
			if (flag)
			{
				this.timerforManualkV.Dispose();
			}
			this.DeregisterLocalizedText();
		}

		// Token: 0x04000BCF RID: 3023
		private DBConnection dbConnection;

		// Token: 0x04000BD0 RID: 3024
		private ParameterHandling parHandling;

		// Token: 0x04000BD1 RID: 3025
		private Timer timerforManualkV;

		// Token: 0x04000BD2 RID: 3026
		private int imageGroup;

		// Token: 0x04000BD3 RID: 3027
		private bool showDialogWindows;

		// Token: 0x04000BD4 RID: 3028
		private ProductManager productManager;

		// Token: 0x04000BD5 RID: 3029
		private AccessRightsManager accessManager;

		// Token: 0x04000BD6 RID: 3030
		private MachineState machineState;

		// Token: 0x04000BD7 RID: 3031
		private NumKeyboard numKeyboard;

		// Token: 0x04000BDB RID: 3035
		private TestcardSimService testcardSimService;

		// Token: 0x04000BDC RID: 3036
		private bool showTestcardSimMsg;

		// Token: 0x04000BDE RID: 3038
		private bool manualkVDeactivation;

		// Token: 0x04000BDF RID: 3039
		private bool bIsManualkVVisible;

		// Token: 0x04000BE0 RID: 3040
		private bool bIsAutoKVVisible;

		// Token: 0x04000BE1 RID: 3041
		private double voltageSetpoint;

		// Token: 0x04000BE2 RID: 3042
		private double voltageSetpointDisplay;

		// Token: 0x04000BE3 RID: 3043
		private double currentSetpointDisplay;

		// Token: 0x04000BE4 RID: 3044
		private double currentSetpoint;

		// Token: 0x04000BE7 RID: 3047
		private int autokVMode;

		// Token: 0x04000BE8 RID: 3048
		private bool fullPowerModeEnabled;

		// Token: 0x04000BE9 RID: 3049
		private bool ecoModeEnabled;

		// Token: 0x04000BEA RID: 3050
		private bool manualModeEnabled;

		// Token: 0x04000BEB RID: 3051
		private bool initialized = false;
	}
}
