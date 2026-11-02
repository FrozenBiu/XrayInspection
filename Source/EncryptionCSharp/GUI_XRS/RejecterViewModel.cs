using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using Common;
using DatabaseConnection;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x0200014D RID: 333
	public class RejecterViewModel : INotifyPropertyChanged
	{
		// Token: 0x140000A7 RID: 167
		// (add) Token: 0x06002004 RID: 8196 RVA: 0x000D2834 File Offset: 0x000D0A34
		// (remove) Token: 0x06002005 RID: 8197 RVA: 0x000D286C File Offset: 0x000D0A6C
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x17000940 RID: 2368
		// (get) Token: 0x06002006 RID: 8198 RVA: 0x000D28A1 File Offset: 0x000D0AA1
		// (set) Token: 0x06002007 RID: 8199 RVA: 0x000D28A9 File Offset: 0x000D0AA9
		public BoundLocalizedText StrEjectTime { get; set; }

		// Token: 0x17000941 RID: 2369
		// (get) Token: 0x06002008 RID: 8200 RVA: 0x000D28B2 File Offset: 0x000D0AB2
		// (set) Token: 0x06002009 RID: 8201 RVA: 0x000D28BA File Offset: 0x000D0ABA
		public BoundLocalizedText StrDelay { get; set; }

		// Token: 0x17000942 RID: 2370
		// (get) Token: 0x0600200A RID: 8202 RVA: 0x000D28C3 File Offset: 0x000D0AC3
		// (set) Token: 0x0600200B RID: 8203 RVA: 0x000D28CB File Offset: 0x000D0ACB
		public BoundLocalizedText StrRejecterEnabled { get; set; }

		// Token: 0x17000943 RID: 2371
		// (get) Token: 0x0600200C RID: 8204 RVA: 0x000D28D4 File Offset: 0x000D0AD4
		// (set) Token: 0x0600200D RID: 8205 RVA: 0x000D28EC File Offset: 0x000D0AEC
		public bool ObjectBasedFlapsVisible
		{
			get
			{
				return this.objectBasedFlapsVisible;
			}
			set
			{
				this.objectBasedFlapsVisible = value;
				bool flag = this.Rejecters != null;
				if (flag)
				{
					foreach (Rejecter rejecter in this.Rejecters)
					{
						rejecter.ObjectBasedFlapsVisible = this.objectBasedFlapsVisible;
					}
				}
				this.OnPropertyChanged("ObjectBasedFlapsVisible");
			}
		}

		// Token: 0x17000944 RID: 2372
		// (get) Token: 0x0600200E RID: 8206 RVA: 0x000D296C File Offset: 0x000D0B6C
		// (set) Token: 0x0600200F RID: 8207 RVA: 0x000D2984 File Offset: 0x000D0B84
		public bool ObjectBasedFlapsEnable
		{
			get
			{
				return this.objectBasedFlapsEnable;
			}
			set
			{
				bool flag = this.objectBasedFlapsEnable != value;
				if (flag)
				{
					this.objectBasedFlapsEnable = value;
					this.parHandling.WriteValue("/hardware/rejecter/objectBasedFlaps/EnableValves/Value", this.objectBasedFlapsEnable ? "1" : "0");
					this.OnPropertyChanged("ObjectBasedFlapsEnable");
				}
			}
		}

		// Token: 0x17000945 RID: 2373
		// (get) Token: 0x06002010 RID: 8208 RVA: 0x000D29DC File Offset: 0x000D0BDC
		// (set) Token: 0x06002011 RID: 8209 RVA: 0x000D29F4 File Offset: 0x000D0BF4
		public int ObjectBasedFlapsEjectTime
		{
			get
			{
				return this.objectBasedFlapsEjectTime;
			}
			set
			{
				bool flag = this.objectBasedFlapsEjectTime != value;
				if (flag)
				{
					this.objectBasedFlapsEjectTime = value;
					this.parHandling.WriteValue("/hardware/rejecter/objectBasedFlaps/EjectTime/Value", this.objectBasedFlapsEjectTime.ToString());
					this.OnPropertyChanged("ObjectBasedFlapsEjectTime");
				}
			}
		}

		// Token: 0x17000946 RID: 2374
		// (get) Token: 0x06002012 RID: 8210 RVA: 0x000D2A44 File Offset: 0x000D0C44
		// (set) Token: 0x06002013 RID: 8211 RVA: 0x000D2A5C File Offset: 0x000D0C5C
		public int ObjectBasedFlapsDelayTime
		{
			get
			{
				return this.objectBasedFlapsDelayTime;
			}
			set
			{
				bool flag = this.objectBasedFlapsDelayTime != value;
				if (flag)
				{
					this.objectBasedFlapsDelayTime = value;
					this.parHandling.WriteValue("/hardware/rejecter/objectBasedFlaps/DelayTime/Value", this.objectBasedFlapsDelayTime.ToString());
					this.OnPropertyChanged("ObjectBasedFlapsDelayTime");
				}
			}
		}

		// Token: 0x17000947 RID: 2375
		// (get) Token: 0x06002014 RID: 8212 RVA: 0x000D2AAB File Offset: 0x000D0CAB
		// (set) Token: 0x06002015 RID: 8213 RVA: 0x000D2AB3 File Offset: 0x000D0CB3
		public Thickness TabBorderThickness { get; set; }

		// Token: 0x17000948 RID: 2376
		// (get) Token: 0x06002016 RID: 8214 RVA: 0x000D2ABC File Offset: 0x000D0CBC
		// (set) Token: 0x06002017 RID: 8215 RVA: 0x000D2AC4 File Offset: 0x000D0CC4
		public List<Rejecter> Rejecters { get; set; }

		// Token: 0x17000949 RID: 2377
		// (get) Token: 0x06002018 RID: 8216 RVA: 0x000D2ACD File Offset: 0x000D0CCD
		// (set) Token: 0x06002019 RID: 8217 RVA: 0x000D2AD5 File Offset: 0x000D0CD5
		public RelayCommand CleanUpCommand { get; set; }

		// Token: 0x0600201A RID: 8218 RVA: 0x000D2AE0 File Offset: 0x000D0CE0
		public RejecterViewModel(ParameterHandling parHandling, DBConnection dbConnection, AccessRightsManager accessManager, MachineState machineState, MessageSystem messageSystem)
		{
			this.CleanUpCommand = new RelayCommand(new Action<object>(this.CleanUp));
			this.parHandling = parHandling;
			this.TabBorderThickness = new Thickness(1.0);
			string text = null;
			bool flag = false;
			dbConnection.GetObjectBasedFlapsEnabled(ref flag);
			this.objectBasedFlapsVisible = flag;
			dbConnection.GetObjectBasedFlapsCount(ref this.objectBasedFlapsCount);
			dbConnection.GetParameterValue("/hardware/rejecter/objectBasedFlaps/EnableValves/Value", ref text);
			this.objectBasedFlapsEnable = (text == "1");
			dbConnection.GetParameterValue("/hardware/rejecter/objectBasedFlaps/EjectTime/Value", ref text);
			int.TryParse(text, out this.objectBasedFlapsEjectTime);
			dbConnection.GetParameterValue("/hardware/rejecter/objectBasedFlaps/DelayTime/Value", ref text);
			int.TryParse(text, out this.objectBasedFlapsDelayTime);
			this.InitializeRejecters(parHandling, dbConnection, accessManager, machineState, messageSystem);
			this.ObjectBasedFlapsVisible = this.objectBasedFlapsVisible;
			this.RegisterLocalizedText();
		}

		// Token: 0x0600201B RID: 8219 RVA: 0x000D2BC8 File Offset: 0x000D0DC8
		private void InitializeRejecters(ParameterHandling parHandling, DBConnection dbConnection, AccessRightsManager accessManager, MachineState machineState, MessageSystem messageSystem)
		{
			MachineType machineType = MachineType.MachineTypeUndefined;
			dbConnection.GetMachineType(ref machineType);
			MachineMode machineMode = MachineMode.Standard;
			dbConnection.GetMachineMode(ref machineMode);
			this.Rejecters = new List<Rejecter>();
			this.rejecter1 = new Rejecter(parHandling, dbConnection, Rejecter.rejecter1MainPath, 1, accessManager, machineState, messageSystem);
			this.rejecter1.PropertyChanged += this.rejecter_PropertyChanged;
			this.Rejecters.Add(this.rejecter1);
			this.rejecter2 = new Rejecter(parHandling, dbConnection, Rejecter.rejecter2MainPath, 2, accessManager, machineState, messageSystem);
			bool flag = machineType == MachineType.RayconEntry;
			if (flag)
			{
				this.rejecter2.BinFullTimeoutVisible = false;
				this.rejecter2.BeltStopOnContaminantVisible = true;
				this.rejecter2.VerifyVisible = false;
				this.rejecter2.VerifyTestVisible = false;
				this.rejecter2.TimeoutRejectVerifySensorBlockedVisible = false;
				this.rejecter2.DebounceTimeVisible = false;
				this.rejecter2.HighLevelSensors = false;
				this.rejecter2.MSBinUnlockVisible = false;
				this.rejecter2.MSBinUnlockTimeoutVisible = false;
				this.rejecter2.AirPressureTestVisible = false;
			}
			this.rejecter2.PropertyChanged += this.rejecter_PropertyChanged;
			bool flag2 = !this.ObjectBasedFlapsVisible;
			if (flag2)
			{
				this.Rejecters.Add(this.rejecter2);
			}
			else
			{
				bool flag3 = this.objectBasedFlapsCount >= 2;
				if (flag3)
				{
					this.Rejecters.Add(this.rejecter2);
				}
			}
			bool flag4 = machineType != MachineType.RayconEntry;
			if (flag4)
			{
				string a = parHandling.ReadPar(Definition.ioSubversionPath);
				bool flag5 = a != "1" && a != "3" && a != "4";
				if (flag5)
				{
					this.rejecter3 = new Rejecter(parHandling, dbConnection, Rejecter.rejecter3MainPath, 3, accessManager, machineState, messageSystem);
					this.rejecter4 = new Rejecter(parHandling, dbConnection, Rejecter.rejecter4MainPath, 4, accessManager, machineState, messageSystem);
					this.rejecter3.PropertyChanged += this.rejecter_PropertyChanged;
					this.rejecter4.PropertyChanged += this.rejecter_PropertyChanged;
					bool flag6 = !this.ObjectBasedFlapsVisible;
					if (flag6)
					{
						this.Rejecters.Add(this.rejecter3);
					}
					else
					{
						bool flag7 = this.objectBasedFlapsCount >= 3;
						if (flag7)
						{
							this.Rejecters.Add(this.rejecter3);
						}
					}
					bool flag8 = !this.ObjectBasedFlapsVisible;
					if (flag8)
					{
						this.Rejecters.Add(this.rejecter4);
					}
					else
					{
						bool flag9 = this.objectBasedFlapsCount == 4;
						if (flag9)
						{
							this.Rejecters.Add(this.rejecter4);
						}
					}
				}
			}
			foreach (Rejecter rejecter in this.Rejecters)
			{
				bool flag10 = this.ObjectBasedFlapsVisible;
				if (flag10)
				{
					bool flag11 = false;
					bool flag12 = false;
					dbConnection.GetBulkBinFullSensor(ref flag11);
					dbConnection.GetBulkRejectVerifySensor(ref flag12);
					rejecter.RejectEnableAccessible = false;
					rejecter.BeltStopOnContaminantVisible = false;
					bool flag13 = flag11;
					if (flag13)
					{
						rejecter.BinFullTimeoutVisible = true;
					}
					bool flag14 = flag12;
					if (flag14)
					{
						rejecter.VerifyVisible = true;
						rejecter.VerifyTestVisible = true;
						rejecter.TimeoutRejectVerifySensorBlockedVisible = true;
					}
				}
			}
			this.SetPriorityVisibility();
			this.SetImageGroupVisibility();
			this.setRejecterVisibilitiesForRayconDPlus1ModuleHYRework();
		}

		// Token: 0x0600201C RID: 8220 RVA: 0x000D2F54 File Offset: 0x000D1154
		private void setRejecterVisibilitiesForRayconDPlus1ModuleHYRework()
		{
			bool flag = this.parHandling.ReadPar(Definition.wagoMachineTypePath) == Definition.ioModuleIsRayconDPlus1ModuleHYRework;
			if (flag)
			{
				bool flag2 = this.rejecter1 != null;
				if (flag2)
				{
					this.rejecter1.BinFullTimeoutVisible = false;
					this.rejecter1.VerifyVisible = false;
					this.rejecter1.VerifyTestVisible = false;
					this.rejecter1.TimeoutRejectVerifySensorBlockedVisible = false;
					this.rejecter1.DebounceTimeVisible = false;
					this.rejecter1.HighLevelSensors = false;
					this.rejecter1.MSBinUnlockVisible = false;
					this.rejecter1.MSBinUnlockTimeoutVisible = false;
					this.rejecter1.AirPressureTestVisible = false;
				}
				bool flag3 = this.rejecter2 != null;
				if (flag3)
				{
					this.rejecter2.BinFullTimeoutVisible = false;
					this.rejecter2.VerifyVisible = false;
					this.rejecter2.VerifyTestVisible = false;
					this.rejecter2.TimeoutRejectVerifySensorBlockedVisible = false;
					this.rejecter2.DebounceTimeVisible = false;
					this.rejecter2.HighLevelSensors = false;
					this.rejecter2.MSBinUnlockVisible = false;
					this.rejecter2.MSBinUnlockTimeoutVisible = false;
					this.rejecter2.AirPressureTestVisible = false;
				}
				bool flag4 = this.rejecter3 != null;
				if (flag4)
				{
					this.rejecter3.BinFullTimeoutVisible = false;
					this.rejecter3.VerifyVisible = false;
					this.rejecter3.VerifyTestVisible = false;
					this.rejecter3.TimeoutRejectVerifySensorBlockedVisible = false;
					this.rejecter3.DebounceTimeVisible = false;
					this.rejecter3.HighLevelSensors = false;
					this.rejecter3.MSBinUnlockVisible = false;
					this.rejecter3.MSBinUnlockTimeoutVisible = false;
					this.rejecter3.AirPressureTestVisible = false;
				}
				bool flag5 = this.rejecter4 != null;
				if (flag5)
				{
					this.rejecter4.BinFullTimeoutVisible = false;
					this.rejecter4.VerifyVisible = false;
					this.rejecter4.VerifyTestVisible = false;
					this.rejecter4.TimeoutRejectVerifySensorBlockedVisible = false;
					this.rejecter4.DebounceTimeVisible = false;
					this.rejecter4.HighLevelSensors = false;
					this.rejecter4.MSBinUnlockVisible = false;
					this.rejecter4.MSBinUnlockTimeoutVisible = false;
					this.rejecter4.AirPressureTestVisible = false;
				}
			}
		}

		// Token: 0x0600201D RID: 8221 RVA: 0x000D3198 File Offset: 0x000D1398
		private void SetPriorityVisibility()
		{
			bool flag = (from rejecter in this.Rejecters
			where rejecter.Enable
			select rejecter).Count<Rejecter>() > 1 && !this.ObjectBasedFlapsVisible;
			if (flag)
			{
				foreach (Rejecter rejecter3 in this.Rejecters)
				{
					rejecter3.PriorityVisible = rejecter3.RightsOk;
				}
			}
			else
			{
				foreach (Rejecter rejecter2 in this.Rejecters)
				{
					rejecter2.PriorityVisible = false;
				}
			}
		}

		// Token: 0x0600201E RID: 8222 RVA: 0x000D3288 File Offset: 0x000D1488
		private void rejecter_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			this.SetPriorityVisibility();
		}

		// Token: 0x0600201F RID: 8223 RVA: 0x000D3294 File Offset: 0x000D1494
		private void SetImageGroupVisibility()
		{
			bool flag = (from rejecter in this.Rejecters
			where rejecter.Enable
			select rejecter).Count<Rejecter>() >= 1 && this.ObjectBasedFlapsVisible;
			if (flag)
			{
				foreach (Rejecter rejecter2 in this.Rejecters)
				{
					rejecter2.ImageGroupVisible = false;
				}
			}
		}

		// Token: 0x06002020 RID: 8224 RVA: 0x000D3330 File Offset: 0x000D1530
		private void RegisterLocalizedText()
		{
			this.StrEjectTime = new BoundLocalizedText("txtEjectTime");
			this.StrDelay = new BoundLocalizedText("txtDelay");
			this.StrRejecterEnabled = new BoundLocalizedText("txtRejecterEnabled");
		}

		// Token: 0x06002021 RID: 8225 RVA: 0x000D3366 File Offset: 0x000D1566
		public void DeregisterLocalizedText()
		{
			MainWindow.localizationManager.Deregister(this.StrEjectTime);
			MainWindow.localizationManager.Deregister(this.StrDelay);
			MainWindow.localizationManager.Deregister(this.StrRejecterEnabled);
		}

		// Token: 0x06002022 RID: 8226 RVA: 0x000D339C File Offset: 0x000D159C
		private void CleanUp(object obj)
		{
			foreach (Rejecter rejecter in this.Rejecters)
			{
				rejecter.DeregisterLocalizedText();
			}
			this.DeregisterLocalizedText();
			this.rejecter1.PropertyChanged -= this.rejecter_PropertyChanged;
			bool flag = this.rejecter2 != null;
			if (flag)
			{
				this.rejecter2.PropertyChanged -= this.rejecter_PropertyChanged;
			}
			bool flag2 = this.rejecter3 != null;
			if (flag2)
			{
				this.rejecter3.PropertyChanged -= this.rejecter_PropertyChanged;
			}
			bool flag3 = this.rejecter4 != null;
			if (flag3)
			{
				this.rejecter4.PropertyChanged -= this.rejecter_PropertyChanged;
			}
		}

		// Token: 0x06002023 RID: 8227 RVA: 0x000D3484 File Offset: 0x000D1684
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x040014BE RID: 5310
		private Rejecter rejecter1;

		// Token: 0x040014BF RID: 5311
		private Rejecter rejecter2;

		// Token: 0x040014C0 RID: 5312
		private Rejecter rejecter3;

		// Token: 0x040014C1 RID: 5313
		private Rejecter rejecter4;

		// Token: 0x040014C2 RID: 5314
		private ParameterHandling parHandling;

		// Token: 0x040014C7 RID: 5319
		private int objectBasedFlapsCount = 1;

		// Token: 0x040014C8 RID: 5320
		private bool objectBasedFlapsVisible;

		// Token: 0x040014C9 RID: 5321
		private bool objectBasedFlapsEnable;

		// Token: 0x040014CA RID: 5322
		private int objectBasedFlapsEjectTime;

		// Token: 0x040014CB RID: 5323
		private int objectBasedFlapsDelayTime;
	}
}
