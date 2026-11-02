using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using Common;
using DatabaseConnection;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x0200021C RID: 540
	public class ConveyorViewModel : INotifyPropertyChanged
	{
		// Token: 0x140000D4 RID: 212
		// (add) Token: 0x0600288B RID: 10379 RVA: 0x0010E6DC File Offset: 0x0010C8DC
		// (remove) Token: 0x0600288C RID: 10380 RVA: 0x0010E714 File Offset: 0x0010C914
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x17000B0E RID: 2830
		// (get) Token: 0x0600288D RID: 10381 RVA: 0x0010E749 File Offset: 0x0010C949
		// (set) Token: 0x0600288E RID: 10382 RVA: 0x0010E751 File Offset: 0x0010C951
		public Conveyor Conveyor { get; set; }

		// Token: 0x17000B0F RID: 2831
		// (get) Token: 0x0600288F RID: 10383 RVA: 0x0010E75A File Offset: 0x0010C95A
		// (set) Token: 0x06002890 RID: 10384 RVA: 0x0010E762 File Offset: 0x0010C962
		public RelayCommand ChangeSpeedCommand { get; set; }

		// Token: 0x17000B10 RID: 2832
		// (get) Token: 0x06002891 RID: 10385 RVA: 0x0010E76B File Offset: 0x0010C96B
		// (set) Token: 0x06002892 RID: 10386 RVA: 0x0010E773 File Offset: 0x0010C973
		public RelayCommand ChangeMaxSpeedCommand { get; set; }

		// Token: 0x17000B11 RID: 2833
		// (get) Token: 0x06002893 RID: 10387 RVA: 0x0010E77C File Offset: 0x0010C97C
		// (set) Token: 0x06002894 RID: 10388 RVA: 0x0010E784 File Offset: 0x0010C984
		public RelayCommand LowerSpeedCommand { get; set; }

		// Token: 0x17000B12 RID: 2834
		// (get) Token: 0x06002895 RID: 10389 RVA: 0x0010E78D File Offset: 0x0010C98D
		// (set) Token: 0x06002896 RID: 10390 RVA: 0x0010E795 File Offset: 0x0010C995
		public RelayCommand HigherSpeedCommand { get; set; }

		// Token: 0x17000B13 RID: 2835
		// (get) Token: 0x06002897 RID: 10391 RVA: 0x0010E79E File Offset: 0x0010C99E
		// (set) Token: 0x06002898 RID: 10392 RVA: 0x0010E7A6 File Offset: 0x0010C9A6
		public RelayCommand StartConveyorCommand { get; set; }

		// Token: 0x17000B14 RID: 2836
		// (get) Token: 0x06002899 RID: 10393 RVA: 0x0010E7AF File Offset: 0x0010C9AF
		// (set) Token: 0x0600289A RID: 10394 RVA: 0x0010E7B7 File Offset: 0x0010C9B7
		public RelayCommand StopConveyorCommand { get; set; }

		// Token: 0x17000B15 RID: 2837
		// (get) Token: 0x0600289B RID: 10395 RVA: 0x0010E7C0 File Offset: 0x0010C9C0
		// (set) Token: 0x0600289C RID: 10396 RVA: 0x0010E7C8 File Offset: 0x0010C9C8
		public RelayCommand ResetConveyorDistanceCommand { get; set; }

		// Token: 0x17000B16 RID: 2838
		// (get) Token: 0x0600289D RID: 10397 RVA: 0x0010E7D1 File Offset: 0x0010C9D1
		// (set) Token: 0x0600289E RID: 10398 RVA: 0x0010E7D9 File Offset: 0x0010C9D9
		public RelayCommand CleanUpCommand { get; set; }

		// Token: 0x17000B17 RID: 2839
		// (get) Token: 0x0600289F RID: 10399 RVA: 0x0010E7E4 File Offset: 0x0010C9E4
		// (set) Token: 0x060028A0 RID: 10400 RVA: 0x0010E7FC File Offset: 0x0010C9FC
		public bool RatedSpeedEnabled
		{
			get
			{
				return this.ratedSpeedEnabled;
			}
			set
			{
				this.ratedSpeedEnabled = value;
				this.OnPropertyChanged("RatedSpeedEnabled");
			}
		}

		// Token: 0x17000B18 RID: 2840
		// (get) Token: 0x060028A1 RID: 10401 RVA: 0x0010E814 File Offset: 0x0010CA14
		// (set) Token: 0x060028A2 RID: 10402 RVA: 0x0010E82C File Offset: 0x0010CA2C
		public bool SpeedEnabled
		{
			get
			{
				return this.speedEnabled;
			}
			set
			{
				this.speedEnabled = value;
				this.OnPropertyChanged("SpeedEnabled");
			}
		}

		// Token: 0x17000B19 RID: 2841
		// (get) Token: 0x060028A3 RID: 10403 RVA: 0x0010E844 File Offset: 0x0010CA44
		// (set) Token: 0x060028A4 RID: 10404 RVA: 0x0010E85C File Offset: 0x0010CA5C
		public int SpeedMetric
		{
			get
			{
				return this.speedMetric;
			}
			set
			{
				this.speedMetric = value;
				this.OnPropertyChanged("SpeedMetric");
				uint value2 = 0U;
				bool flag = this.dbDatalogs != null;
				if (flag)
				{
					this.dbDatalogs.OperatingCounterGetIntEntry("ConveyorDistanceMeter", ref value2);
				}
				bool flag2 = this.speedMetric == 0;
				if (flag2)
				{
					this.dbConnection.SetSpeedMetric(0);
					this.SpeedAbbr = "ft / min";
					this.DistanceAbbr = "ft";
					this.ConveyorDistance = Math.Round(Convert.ToDouble(value2) * Definition.meterToFeetFactor, 0).ToString();
				}
				else
				{
					bool flag3 = this.speedMetric == 1;
					if (flag3)
					{
						this.dbConnection.SetSpeedMetric(1);
						this.SpeedAbbr = "m / s";
						this.DistanceAbbr = "m";
						this.ConveyorDistance = Convert.ToString(value2);
					}
					else
					{
						this.dbConnection.SetSpeedMetric(2);
						this.SpeedAbbr = "m / min";
						this.DistanceAbbr = "m";
						this.ConveyorDistance = Convert.ToString(value2);
					}
				}
			}
		}

		// Token: 0x17000B1A RID: 2842
		// (get) Token: 0x060028A5 RID: 10405 RVA: 0x0010E970 File Offset: 0x0010CB70
		// (set) Token: 0x060028A6 RID: 10406 RVA: 0x0010E994 File Offset: 0x0010CB94
		public bool ShowSpeedMetricMin
		{
			get
			{
				return this.SpeedMetric == 2;
			}
			set
			{
				this.SpeedMetric = 2;
			}
		}

		// Token: 0x17000B1B RID: 2843
		// (get) Token: 0x060028A7 RID: 10407 RVA: 0x0010E9A0 File Offset: 0x0010CBA0
		// (set) Token: 0x060028A8 RID: 10408 RVA: 0x0010E9C4 File Offset: 0x0010CBC4
		public bool ShowSpeedMetric
		{
			get
			{
				return this.SpeedMetric == 1;
			}
			set
			{
				this.SpeedMetric = 1;
			}
		}

		// Token: 0x17000B1C RID: 2844
		// (get) Token: 0x060028A9 RID: 10409 RVA: 0x0010E9D0 File Offset: 0x0010CBD0
		// (set) Token: 0x060028AA RID: 10410 RVA: 0x0010E9F4 File Offset: 0x0010CBF4
		public bool ShowSpeedAmerican
		{
			get
			{
				return this.SpeedMetric == 0;
			}
			set
			{
				this.SpeedMetric = 0;
			}
		}

		// Token: 0x17000B1D RID: 2845
		// (get) Token: 0x060028AB RID: 10411 RVA: 0x0010EA00 File Offset: 0x0010CC00
		// (set) Token: 0x060028AC RID: 10412 RVA: 0x0010EA18 File Offset: 0x0010CC18
		public string SpeedAbbr
		{
			get
			{
				return this.speedAbbr;
			}
			set
			{
				this.speedAbbr = value;
				this.OnPropertyChanged("SpeedAbbr");
			}
		}

		// Token: 0x17000B1E RID: 2846
		// (get) Token: 0x060028AD RID: 10413 RVA: 0x0010EA30 File Offset: 0x0010CC30
		// (set) Token: 0x060028AE RID: 10414 RVA: 0x0010EA48 File Offset: 0x0010CC48
		public string DistanceAbbr
		{
			get
			{
				return this.distanceAbbr;
			}
			set
			{
				this.distanceAbbr = value;
				this.OnPropertyChanged("DistanceAbbr");
			}
		}

		// Token: 0x17000B1F RID: 2847
		// (get) Token: 0x060028AF RID: 10415 RVA: 0x0010EA60 File Offset: 0x0010CC60
		// (set) Token: 0x060028B0 RID: 10416 RVA: 0x0010EA78 File Offset: 0x0010CC78
		public bool SpeedUnitEnabled
		{
			get
			{
				return this.speedUnitEnabled;
			}
			set
			{
				this.speedUnitEnabled = value;
				this.OnPropertyChanged("SpeedUnitEnabled");
			}
		}

		// Token: 0x17000B20 RID: 2848
		// (get) Token: 0x060028B1 RID: 10417 RVA: 0x0010EA90 File Offset: 0x0010CC90
		// (set) Token: 0x060028B2 RID: 10418 RVA: 0x0010EAA8 File Offset: 0x0010CCA8
		public bool MachineCalibrating
		{
			get
			{
				return this.machineCalibrating;
			}
			set
			{
				bool flag = this.machineCalibrating != value;
				if (flag)
				{
					this.machineCalibrating = value;
					this.OnPropertyChanged("MachineCalibrating");
				}
			}
		}

		// Token: 0x17000B21 RID: 2849
		// (get) Token: 0x060028B3 RID: 10419 RVA: 0x0010EADC File Offset: 0x0010CCDC
		// (set) Token: 0x060028B4 RID: 10420 RVA: 0x0010EAF4 File Offset: 0x0010CCF4
		public string ConveyorDistance
		{
			get
			{
				return this.conveyorDistance;
			}
			set
			{
				bool flag = this.conveyorDistance != value;
				if (flag)
				{
					this.conveyorDistance = value;
					this.OnPropertyChanged("ConveyorDistance");
				}
			}
		}

		// Token: 0x17000B22 RID: 2850
		// (get) Token: 0x060028B5 RID: 10421 RVA: 0x0010EB27 File Offset: 0x0010CD27
		// (set) Token: 0x060028B6 RID: 10422 RVA: 0x0010EB2F File Offset: 0x0010CD2F
		public BoundLocalizedText StrConveyorSpeed { get; set; }

		// Token: 0x17000B23 RID: 2851
		// (get) Token: 0x060028B7 RID: 10423 RVA: 0x0010EB38 File Offset: 0x0010CD38
		// (set) Token: 0x060028B8 RID: 10424 RVA: 0x0010EB40 File Offset: 0x0010CD40
		public BoundLocalizedText StrRatedConveyorSpeed { get; set; }

		// Token: 0x17000B24 RID: 2852
		// (get) Token: 0x060028B9 RID: 10425 RVA: 0x0010EB49 File Offset: 0x0010CD49
		// (set) Token: 0x060028BA RID: 10426 RVA: 0x0010EB51 File Offset: 0x0010CD51
		public BoundLocalizedText StrSpeedUnit { get; set; }

		// Token: 0x17000B25 RID: 2853
		// (get) Token: 0x060028BB RID: 10427 RVA: 0x0010EB5A File Offset: 0x0010CD5A
		// (set) Token: 0x060028BC RID: 10428 RVA: 0x0010EB62 File Offset: 0x0010CD62
		public BoundLocalizedText StrConveyorDistance { get; set; }

		// Token: 0x17000B26 RID: 2854
		// (get) Token: 0x060028BD RID: 10429 RVA: 0x0010EB6B File Offset: 0x0010CD6B
		// (set) Token: 0x060028BE RID: 10430 RVA: 0x0010EB73 File Offset: 0x0010CD73
		public BoundLocalizedText StrResetConveyorDistance { get; set; }

		// Token: 0x060028BF RID: 10431 RVA: 0x0010EB7C File Offset: 0x0010CD7C
		public ConveyorViewModel(Conveyor conveyor, AccessRightsManager accessManager, DBConnection dbConnection, MachineState machineState, bool checkIfCalibrating, DBDataLogs dBDatalogs, ParameterHandling parHandling)
		{
			this.Conveyor = conveyor;
			this.accessManager = accessManager;
			this.dbConnection = dbConnection;
			this.dbDatalogs = dBDatalogs;
			this.machineState = machineState;
			this.parHandling = parHandling;
			this.Initialize(checkIfCalibrating);
		}

		// Token: 0x060028C0 RID: 10432 RVA: 0x0010EBF4 File Offset: 0x0010CDF4
		private void Initialize(bool checkIfCalibrating)
		{
			MachineMode machineMode = MachineMode.Standard;
			this.cleanedup = false;
			this.RegisterLocalizedText();
			this.ChangeSpeedCommand = new RelayCommand(new Action<object>(this.ChangeSpeed));
			this.ChangeMaxSpeedCommand = new RelayCommand(new Action<object>(this.ChangeMaxSpeed));
			this.LowerSpeedCommand = new RelayCommand(new Action<object>(this.LowerSpeed));
			this.HigherSpeedCommand = new RelayCommand(new Action<object>(this.HigherSpeed));
			this.CleanUpCommand = new RelayCommand(new Action<object>(this.CleanUp));
			this.StartConveyorCommand = new RelayCommand(new Action<object>(this.StartConveyor));
			this.StopConveyorCommand = new RelayCommand(new Action<object>(this.StopConveyor));
			this.ResetConveyorDistanceCommand = new RelayCommand(new Action<object>(this.ResetConveyorDistance));
			this.GetSpeedUnit();
			this.RatedSpeedEnabled = (this.accessManager.CurrentUser.Rights >= this.rightRatedSpeedEnabled);
			this.dbConnection.GetMachineMode(ref machineMode);
			this.SpeedEnabled = (this.accessManager.CurrentUser.Rights >= this.rightSpeedEnabled);
			this.SpeedUnitEnabled = (this.accessManager.CurrentUser.Rights >= this.rightChangeSpeedUnit);
			this.oldSpeed = this.Conveyor.Speed;
			this.oldRatedSpeed = this.Conveyor.RatedSpeed;
			uint value = 0U;
			bool flag = this.dbDatalogs != null;
			if (flag)
			{
				this.dbDatalogs.OperatingCounterGetIntEntry("ConveyorDistanceMeter", ref value);
			}
			this.ConveyorDistance = Convert.ToString(value);
			if (checkIfCalibrating)
			{
				this.timerIsMachineCalibrating = new Timer(new TimerCallback(this.CheckMachineState), null, 0, 500);
			}
		}

		// Token: 0x060028C1 RID: 10433 RVA: 0x0010EDC1 File Offset: 0x0010CFC1
		private void CheckMachineState(object obj)
		{
			this.MachineCalibrating = (this.machineState.State == MachineStates.XrayOnCalibrationNotDone || this.machineState.State == MachineStates.Reseasoning);
		}

		// Token: 0x060028C2 RID: 10434 RVA: 0x0010EDEC File Offset: 0x0010CFEC
		private void RegisterLocalizedText()
		{
			this.StrConveyorSpeed = new BoundLocalizedText("txtConveyorSpeed");
			this.StrRatedConveyorSpeed = new BoundLocalizedText("txtRatedConveyorSpeed");
			this.StrSpeedUnit = new BoundLocalizedText("txtSpeedUnit");
			this.StrConveyorDistance = new BoundLocalizedText("txtConveyorDistance");
			this.StrResetConveyorDistance = new BoundLocalizedText("txtResetConveyorDistance");
		}

		// Token: 0x060028C3 RID: 10435 RVA: 0x0010EE50 File Offset: 0x0010D050
		public void CleanUp(object obj)
		{
			bool flag = !this.cleanedup;
			if (flag)
			{
				MainWindow.localizationManager.Deregister(this.StrConveyorSpeed);
				MainWindow.localizationManager.Deregister(this.StrRatedConveyorSpeed);
				MainWindow.localizationManager.Deregister(this.StrSpeedUnit);
				MainWindow.localizationManager.Deregister(this.StrConveyorDistance);
				MainWindow.localizationManager.Deregister(this.StrResetConveyorDistance);
				bool flag2 = this.Conveyor.Speed != this.oldSpeed;
				if (flag2)
				{
					string text = string.Format("{0:0.00}", this.oldSpeed);
					string text2 = string.Format("{0:0.00}", this.Conveyor.Speed);
					bool flag3 = this.speedMetric == 0;
					if (flag3)
					{
						text = string.Format("{0:0.0}", this.oldSpeed * Definition.meterToFeetFactor * 60.0);
						text2 = string.Format("{0:0.0}", this.Conveyor.Speed * Definition.meterToFeetFactor * 60.0);
					}
					bool flag4 = this.speedMetric == 2;
					if (flag4)
					{
						text = string.Format("{0:0.0}", this.oldSpeed * 60.0);
						text2 = string.Format("{0:0.0}", this.Conveyor.Speed * 60.0);
					}
					LogbookManager.AddEntry(SSTEntryCode.ConveyorSpeed, 0, ((int)(this.oldSpeed * 100.0)).ToString(), ((int)(this.Conveyor.Speed * 100.0)).ToString());
					bool flag5 = this.parHandling != null;
					if (flag5)
					{
						string a = this.parHandling.ReadPar("/machine/MachineSubType/Value");
						bool flag6 = a == "HX Bulk" || a == "HX Bulk Hy";
						if (flag6)
						{
							double num = this.Conveyor.Speed / this.oldSpeed;
							bool flag7 = this.parHandling.ReadPar(SVT.svtEnablePath) == "1";
							string s;
							if (flag7)
							{
								s = this.parHandling.ReadPar(SVT.svtDelayPath);
							}
							else
							{
								s = this.parHandling.ReadPar(FlapsOverIOs.bulkflapsDelayPath);
							}
							double num2;
							bool flag8 = double.TryParse(s, out num2);
							if (flag8)
							{
								num2 /= num;
								bool flag9 = num2 > 5000.0;
								if (flag9)
								{
									num2 = 5000.0;
								}
								int num3 = (int)num2;
								bool flag10 = this.parHandling.ReadPar(SVT.svtEnablePath) == "1";
								if (flag10)
								{
									this.parHandling.WriteValue(SVT.svtDelayPath, num3.ToString());
								}
								else
								{
									this.parHandling.WriteValue(FlapsOverIOs.bulkflapsDelayPath, num3.ToString());
								}
							}
						}
					}
				}
				bool flag11 = this.Conveyor.RatedSpeed != this.oldRatedSpeed;
				if (flag11)
				{
					string text3 = string.Format("{0:0.00}", this.oldRatedSpeed);
					string text4 = string.Format("{0:0.00}", this.Conveyor.RatedSpeed);
					bool flag12 = this.speedMetric == 0;
					if (flag12)
					{
						text3 = string.Format("{0:0.0}", this.oldRatedSpeed * Definition.meterToFeetFactor * 60.0);
						text4 = string.Format("{0:0.0}", this.Conveyor.RatedSpeed * Definition.meterToFeetFactor * 60.0);
					}
					bool flag13 = this.speedMetric == 2;
					if (flag13)
					{
						text3 = string.Format("{0:0.0}", this.oldSpeed * 60.0);
						text4 = string.Format("{0:0.0}", this.Conveyor.Speed * 60.0);
					}
					LogbookManager.AddEntry(SSTEntryCode.RatedConveyorSpeed, 0, ((int)(this.oldRatedSpeed * 100.0)).ToString(), ((int)(this.Conveyor.RatedSpeed * 100.0)).ToString());
				}
				bool flag14 = this.timerIsMachineCalibrating != null;
				if (flag14)
				{
					this.timerIsMachineCalibrating.Change(-1, -1);
					this.timerIsMachineCalibrating.Dispose();
				}
				bool flag15 = this.AdaptScanrate();
				if (flag15)
				{
					int num4 = 0;
					XRayOnProcessing.AdaptedIntegrationTime = this.Conveyor.AdaptedIntegrationTime;
					while (this.machineState.State == MachineStates.Inspecting && num4 < 20)
					{
						Thread.Sleep(200);
						num4++;
					}
				}
				this.cleanedup = true;
			}
		}

		// Token: 0x060028C4 RID: 10436 RVA: 0x0010F334 File Offset: 0x0010D534
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x060028C5 RID: 10437 RVA: 0x0010F364 File Offset: 0x0010D564
		public void ChangeSpeed(object onj)
		{
			bool flag = this.speedMetric == 0;
			string oldValue;
			if (flag)
			{
				oldValue = Math.Round(this.Conveyor.Speed * Definition.meterToFeetFactor * 60.0, 2).ToString();
			}
			else
			{
				bool flag2 = this.speedMetric == 1;
				if (flag2)
				{
					oldValue = Math.Round(this.Conveyor.Speed, 2).ToString();
				}
				else
				{
					oldValue = Math.Round(this.Conveyor.Speed * 60.0, 2).ToString();
				}
			}
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtConveyorSpeed"), oldValue, NumKeyboardType.TypeDouble);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_InputSpeed;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x060028C6 RID: 10438 RVA: 0x0010F440 File Offset: 0x0010D640
		public void ChangeMaxSpeed(object onj)
		{
			bool flag = this.speedMetric == 0;
			string oldValue;
			if (flag)
			{
				oldValue = Math.Round(this.Conveyor.RatedSpeed * Definition.meterToFeetFactor * 60.0, 2).ToString();
			}
			else
			{
				bool flag2 = this.speedMetric == 1;
				if (flag2)
				{
					oldValue = Math.Round(this.Conveyor.RatedSpeed, 2).ToString();
				}
				else
				{
					oldValue = Math.Round(this.Conveyor.RatedSpeed * 60.0, 2).ToString();
				}
			}
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtRatedConveyorSpeed"), oldValue, NumKeyboardType.TypeDouble);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_InputMaxSpeed;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x060028C7 RID: 10439 RVA: 0x0010F51C File Offset: 0x0010D71C
		private void numKeyboard_InputSpeed(object sender, KeyboardInputEventArgs e)
		{
			try
			{
				bool flag = this.speedMetric == 0;
				double speed;
				if (flag)
				{
					speed = double.Parse(e.Text, new CultureInfo("de-DE")) / (Definition.meterToFeetFactor * 60.0);
				}
				else
				{
					bool flag2 = this.speedMetric == 1;
					if (flag2)
					{
						speed = double.Parse(e.Text, new CultureInfo("de-DE"));
					}
					else
					{
						speed = double.Parse(e.Text, new CultureInfo("de-DE")) / 60.0;
					}
				}
				this.Conveyor.Speed = speed;
				this.numKeyboard.TextInputKeyboard -= this.numKeyboard_InputSpeed;
			}
			catch
			{
			}
		}

		// Token: 0x060028C8 RID: 10440 RVA: 0x0010F5E4 File Offset: 0x0010D7E4
		private void numKeyboard_InputMaxSpeed(object sender, KeyboardInputEventArgs e)
		{
			try
			{
				bool flag = this.speedMetric == 0;
				double ratedSpeed;
				if (flag)
				{
					ratedSpeed = double.Parse(e.Text, new CultureInfo("de-DE")) / (Definition.meterToFeetFactor * 60.0);
				}
				else
				{
					bool flag2 = this.speedMetric == 1;
					if (flag2)
					{
						ratedSpeed = double.Parse(e.Text, new CultureInfo("de-DE"));
					}
					else
					{
						ratedSpeed = double.Parse(e.Text, new CultureInfo("de-DE")) / 60.0;
					}
				}
				this.Conveyor.RatedSpeed = ratedSpeed;
				this.numKeyboard.TextInputKeyboard -= this.numKeyboard_InputMaxSpeed;
			}
			catch
			{
			}
		}

		// Token: 0x060028C9 RID: 10441 RVA: 0x0010F6AC File Offset: 0x0010D8AC
		private void LowerSpeed(object obj)
		{
			this.Conveyor.Speed = this.Conveyor.Speed - 0.02;
		}

		// Token: 0x060028CA RID: 10442 RVA: 0x0010F6D0 File Offset: 0x0010D8D0
		private void HigherSpeed(object obj)
		{
			this.Conveyor.Speed = this.Conveyor.Speed + 0.02;
		}

		// Token: 0x060028CB RID: 10443 RVA: 0x0010F6F4 File Offset: 0x0010D8F4
		private bool AdaptScanrate()
		{
			return this.Conveyor.AdaptScanrate();
		}

		// Token: 0x060028CC RID: 10444 RVA: 0x0010F714 File Offset: 0x0010D914
		private void GetSpeedUnit()
		{
			int num = 1;
			this.dbConnection.GetSpeedMetric(ref num);
			this.SpeedMetric = num;
		}

		// Token: 0x060028CD RID: 10445 RVA: 0x0010F73A File Offset: 0x0010D93A
		private void StartConveyor(object obj)
		{
			this.Conveyor.StartConveyor();
		}

		// Token: 0x060028CE RID: 10446 RVA: 0x0010F749 File Offset: 0x0010D949
		private void StopConveyor(object obj)
		{
			this.Conveyor.StopConveyor();
		}

		// Token: 0x060028CF RID: 10447 RVA: 0x0010F758 File Offset: 0x0010D958
		private void ResetConveyorDistance(object obj)
		{
			string message = this.StrResetConveyorDistance.LocalizedText + "?";
			bool flag = Dialog.Show(LocalizationManager.Localize("txtWarning"), message, DialogStyle.Warning, DialogButtons.OKCancel) == Dialog.DialogResults.Ok;
			if (flag)
			{
				this.dbDatalogs.ResetLogsConveyorData();
				uint value = 0U;
				this.dbDatalogs.OperatingCounterGetIntEntry("ConveyorDistanceMeter", ref value);
				this.ConveyorDistance = Convert.ToString(value);
			}
		}

		// Token: 0x04001920 RID: 6432
		private BuiltInPriviledges rightRatedSpeedEnabled = BuiltInPriviledges.Service;

		// Token: 0x04001921 RID: 6433
		private BuiltInPriviledges rightSpeedEnabled = BuiltInPriviledges.Operator1;

		// Token: 0x04001922 RID: 6434
		private BuiltInPriviledges rightChangeSpeedBulk = BuiltInPriviledges.Service;

		// Token: 0x04001923 RID: 6435
		private BuiltInPriviledges rightChangeSpeedUnit = BuiltInPriviledges.Admin;

		// Token: 0x04001924 RID: 6436
		private const double speedStep = 0.02;

		// Token: 0x04001925 RID: 6437
		private const string SubTypeBulk = "HX Bulk";

		// Token: 0x04001926 RID: 6438
		private const string SubTypeLWHy = "LW Hy";

		// Token: 0x04001927 RID: 6439
		private const string SubTypeBulkHy = "HX Bulk Hy";

		// Token: 0x04001928 RID: 6440
		private double oldSpeed;

		// Token: 0x04001929 RID: 6441
		private double oldRatedSpeed;

		// Token: 0x0400192A RID: 6442
		private NumKeyboard numKeyboard;

		// Token: 0x0400192B RID: 6443
		private bool ratedSpeedEnabled;

		// Token: 0x0400192C RID: 6444
		private bool speedEnabled;

		// Token: 0x0400192D RID: 6445
		private bool speedUnitEnabled;

		// Token: 0x0400192E RID: 6446
		private AccessRightsManager accessManager;

		// Token: 0x0400192F RID: 6447
		private DBConnection dbConnection;

		// Token: 0x04001930 RID: 6448
		private DBDataLogs dbDatalogs;

		// Token: 0x04001931 RID: 6449
		private int speedMetric;

		// Token: 0x04001932 RID: 6450
		private string speedAbbr;

		// Token: 0x04001933 RID: 6451
		private string distanceAbbr;

		// Token: 0x04001934 RID: 6452
		private bool machineCalibrating;

		// Token: 0x04001935 RID: 6453
		private Timer timerIsMachineCalibrating;

		// Token: 0x04001936 RID: 6454
		private MachineState machineState;

		// Token: 0x04001937 RID: 6455
		private bool cleanedup;

		// Token: 0x04001938 RID: 6456
		private string conveyorDistance;

		// Token: 0x04001948 RID: 6472
		private ParameterHandling parHandling;
	}
}
