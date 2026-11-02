using System;
using Common;
using DatabaseConnection;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x02000154 RID: 340
	public class SettingModeViewModel
	{
		// Token: 0x17000958 RID: 2392
		// (get) Token: 0x0600205B RID: 8283 RVA: 0x000D43D8 File Offset: 0x000D25D8
		// (set) Token: 0x0600205C RID: 8284 RVA: 0x000D43E0 File Offset: 0x000D25E0
		public bool BulkMode { get; set; }

		// Token: 0x17000959 RID: 2393
		// (get) Token: 0x0600205D RID: 8285 RVA: 0x000D43E9 File Offset: 0x000D25E9
		// (set) Token: 0x0600205E RID: 8286 RVA: 0x000D43F1 File Offset: 0x000D25F1
		public bool EntryTypeMachine { get; set; }

		// Token: 0x1700095A RID: 2394
		// (get) Token: 0x0600205F RID: 8287 RVA: 0x000D43FA File Offset: 0x000D25FA
		// (set) Token: 0x06002060 RID: 8288 RVA: 0x000D4402 File Offset: 0x000D2602
		public ConveyorViewModel ConveyorViewModel { get; set; }

		// Token: 0x1700095B RID: 2395
		// (get) Token: 0x06002061 RID: 8289 RVA: 0x000D440B File Offset: 0x000D260B
		// (set) Token: 0x06002062 RID: 8290 RVA: 0x000D4413 File Offset: 0x000D2613
		public VibroViewModel VibroViewModel { get; set; }

		// Token: 0x1700095C RID: 2396
		// (get) Token: 0x06002063 RID: 8291 RVA: 0x000D441C File Offset: 0x000D261C
		// (set) Token: 0x06002064 RID: 8292 RVA: 0x000D4424 File Offset: 0x000D2624
		public SVTViewModel SVTViewModel { get; set; }

		// Token: 0x1700095D RID: 2397
		// (get) Token: 0x06002065 RID: 8293 RVA: 0x000D442D File Offset: 0x000D262D
		// (set) Token: 0x06002066 RID: 8294 RVA: 0x000D4435 File Offset: 0x000D2635
		public FlapsOverIOsViewModel FlapsOverIOsViewModel { get; set; }

		// Token: 0x1700095E RID: 2398
		// (get) Token: 0x06002067 RID: 8295 RVA: 0x000D443E File Offset: 0x000D263E
		// (set) Token: 0x06002068 RID: 8296 RVA: 0x000D4446 File Offset: 0x000D2646
		public RelayCommand CleanUpCommand { get; set; }

		// Token: 0x1700095F RID: 2399
		// (get) Token: 0x06002069 RID: 8297 RVA: 0x000D444F File Offset: 0x000D264F
		// (set) Token: 0x0600206A RID: 8298 RVA: 0x000D4457 File Offset: 0x000D2657
		public BoundLocalizedText StrVibro { get; set; }

		// Token: 0x17000960 RID: 2400
		// (get) Token: 0x0600206B RID: 8299 RVA: 0x000D4460 File Offset: 0x000D2660
		// (set) Token: 0x0600206C RID: 8300 RVA: 0x000D4468 File Offset: 0x000D2668
		public BoundLocalizedText StrConveyor { get; set; }

		// Token: 0x17000961 RID: 2401
		// (get) Token: 0x0600206D RID: 8301 RVA: 0x000D4471 File Offset: 0x000D2671
		// (set) Token: 0x0600206E RID: 8302 RVA: 0x000D4479 File Offset: 0x000D2679
		public BoundLocalizedText StrRejecter { get; set; }

		// Token: 0x17000962 RID: 2402
		// (get) Token: 0x0600206F RID: 8303 RVA: 0x000D4482 File Offset: 0x000D2682
		// (set) Token: 0x06002070 RID: 8304 RVA: 0x000D448A File Offset: 0x000D268A
		public MachineState machineState { get; set; }

		// Token: 0x17000963 RID: 2403
		// (get) Token: 0x06002071 RID: 8305 RVA: 0x000D4493 File Offset: 0x000D2693
		// (set) Token: 0x06002072 RID: 8306 RVA: 0x000D449B File Offset: 0x000D269B
		public bool FlapsOverIOActive { get; set; }

		// Token: 0x17000964 RID: 2404
		// (get) Token: 0x06002073 RID: 8307 RVA: 0x000D44A4 File Offset: 0x000D26A4
		// (set) Token: 0x06002074 RID: 8308 RVA: 0x000D44AC File Offset: 0x000D26AC
		public bool SVTActive { get; set; }

		// Token: 0x06002075 RID: 8309 RVA: 0x000D44B8 File Offset: 0x000D26B8
		public SettingModeViewModel(Conveyor conveyor, Vibro vibro, AccessRightsManager accessManager, DBConnection dbConnection, ParameterHandling parHandling, MachineState machineState, DBDataLogs dbDatalogs, MessageSystem messageSystem, ImageDataTransfer imageDataTransfer)
		{
			this.parHandling = parHandling;
			this.machineState = machineState;
			this.ConveyorViewModel = new ConveyorViewModel(conveyor, accessManager, dbConnection, machineState, true, dbDatalogs, parHandling);
			this.VibroViewModel = new VibroViewModel(vibro, accessManager, parHandling);
			this.FlapsOverIOActive = (parHandling.ReadPar(FlapsOverIOs.bulkflapsEnabledPath) == "1");
			this.SVTActive = !this.FlapsOverIOActive;
			MachineMode machineMode = MachineMode.Standard;
			dbConnection.GetMachineMode(ref machineMode);
			this.BulkMode = (machineMode == MachineMode.Bulk);
			MachineType machineType = MachineType.MachineTypeUndefined;
			dbConnection.GetMachineType(ref machineType);
			this.EntryTypeMachine = (machineType == MachineType.RayconEntry);
			this.RegisterLocalizedText();
			this.CleanUpCommand = new RelayCommand(new Action<object>(this.CleanUp));
		}

		// Token: 0x06002076 RID: 8310 RVA: 0x000D4585 File Offset: 0x000D2785
		private void RegisterLocalizedText()
		{
			this.StrConveyor = new BoundLocalizedText("txtConveyor");
			this.StrVibro = new BoundLocalizedText("txtVibro");
			this.StrRejecter = new BoundLocalizedText("txtRejecter");
		}

		// Token: 0x06002077 RID: 8311 RVA: 0x000D45BC File Offset: 0x000D27BC
		private void CleanUp(object obj)
		{
			this.ConveyorViewModel.Conveyor.InManualMode = false;
			this.parHandling.WriteValue(SVT.valveTestEnablePath, "0", false);
			this.parHandling.WriteValue(SVT.svtPeriodicModePath, "0", false);
			MainWindow.localizationManager.Deregister(this.StrConveyor);
			MainWindow.localizationManager.Deregister(this.StrVibro);
			MainWindow.localizationManager.Deregister(this.StrRejecter);
		}

		// Token: 0x0400150E RID: 5390
		private ParameterHandling parHandling;
	}
}
