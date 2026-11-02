using System;
using System.Collections.Generic;
using Common;
using DatabaseConnection;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x02000113 RID: 275
	public class ErrorContactViewModel
	{
		// Token: 0x170007BE RID: 1982
		// (get) Token: 0x060019F6 RID: 6646 RVA: 0x0009838D File Offset: 0x0009658D
		// (set) Token: 0x060019F7 RID: 6647 RVA: 0x00098395 File Offset: 0x00096595
		public ErrorHandling ErrorHandling { get; set; }

		// Token: 0x170007BF RID: 1983
		// (get) Token: 0x060019F8 RID: 6648 RVA: 0x0009839E File Offset: 0x0009659E
		// (set) Token: 0x060019F9 RID: 6649 RVA: 0x000983A6 File Offset: 0x000965A6
		public BoundLocalizedText StrError { get; set; }

		// Token: 0x170007C0 RID: 1984
		// (get) Token: 0x060019FA RID: 6650 RVA: 0x000983AF File Offset: 0x000965AF
		// (set) Token: 0x060019FB RID: 6651 RVA: 0x000983B7 File Offset: 0x000965B7
		public BoundLocalizedText StrInfo { get; set; }

		// Token: 0x170007C1 RID: 1985
		// (get) Token: 0x060019FC RID: 6652 RVA: 0x000983C0 File Offset: 0x000965C0
		// (set) Token: 0x060019FD RID: 6653 RVA: 0x000983C8 File Offset: 0x000965C8
		public BoundLocalizedText StrDetector { get; set; }

		// Token: 0x170007C2 RID: 1986
		// (get) Token: 0x060019FE RID: 6654 RVA: 0x000983D1 File Offset: 0x000965D1
		// (set) Token: 0x060019FF RID: 6655 RVA: 0x000983D9 File Offset: 0x000965D9
		public BoundLocalizedText StrXraySource { get; set; }

		// Token: 0x170007C3 RID: 1987
		// (get) Token: 0x06001A00 RID: 6656 RVA: 0x000983E2 File Offset: 0x000965E2
		// (set) Token: 0x06001A01 RID: 6657 RVA: 0x000983EA File Offset: 0x000965EA
		public BoundLocalizedText StrBinFull { get; set; }

		// Token: 0x170007C4 RID: 1988
		// (get) Token: 0x06001A02 RID: 6658 RVA: 0x000983F3 File Offset: 0x000965F3
		// (set) Token: 0x06001A03 RID: 6659 RVA: 0x000983FB File Offset: 0x000965FB
		public BoundLocalizedText StrRejectVerify { get; set; }

		// Token: 0x170007C5 RID: 1989
		// (get) Token: 0x06001A04 RID: 6660 RVA: 0x00098404 File Offset: 0x00096604
		// (set) Token: 0x06001A05 RID: 6661 RVA: 0x0009840C File Offset: 0x0009660C
		public BoundLocalizedText StrVoltage { get; set; }

		// Token: 0x170007C6 RID: 1990
		// (get) Token: 0x06001A06 RID: 6662 RVA: 0x00098415 File Offset: 0x00096615
		// (set) Token: 0x06001A07 RID: 6663 RVA: 0x0009841D File Offset: 0x0009661D
		public BoundLocalizedText StrTemperature { get; set; }

		// Token: 0x170007C7 RID: 1991
		// (get) Token: 0x06001A08 RID: 6664 RVA: 0x00098426 File Offset: 0x00096626
		// (set) Token: 0x06001A09 RID: 6665 RVA: 0x0009842E File Offset: 0x0009662E
		public BoundLocalizedText StrErrorMotor { get; set; }

		// Token: 0x170007C8 RID: 1992
		// (get) Token: 0x06001A0A RID: 6666 RVA: 0x00098437 File Offset: 0x00096637
		// (set) Token: 0x06001A0B RID: 6667 RVA: 0x0009843F File Offset: 0x0009663F
		public BoundLocalizedText StrLowAirPressure { get; set; }

		// Token: 0x170007C9 RID: 1993
		// (get) Token: 0x06001A0C RID: 6668 RVA: 0x00098448 File Offset: 0x00096648
		// (set) Token: 0x06001A0D RID: 6669 RVA: 0x00098450 File Offset: 0x00096650
		public BoundLocalizedText StrErrorCooling { get; set; }

		// Token: 0x170007CA RID: 1994
		// (get) Token: 0x06001A0E RID: 6670 RVA: 0x00098459 File Offset: 0x00096659
		// (set) Token: 0x06001A0F RID: 6671 RVA: 0x00098461 File Offset: 0x00096661
		public BoundLocalizedText StrSetErrorContact { get; set; }

		// Token: 0x170007CB RID: 1995
		// (get) Token: 0x06001A10 RID: 6672 RVA: 0x0009846A File Offset: 0x0009666A
		// (set) Token: 0x06001A11 RID: 6673 RVA: 0x00098472 File Offset: 0x00096672
		public BoundLocalizedText StrErrorXRayLamp { get; set; }

		// Token: 0x170007CC RID: 1996
		// (get) Token: 0x06001A12 RID: 6674 RVA: 0x0009847B File Offset: 0x0009667B
		// (set) Token: 0x06001A13 RID: 6675 RVA: 0x00098483 File Offset: 0x00096683
		public BoundLocalizedText StrBinMissing { get; set; }

		// Token: 0x170007CD RID: 1997
		// (get) Token: 0x06001A14 RID: 6676 RVA: 0x0009848C File Offset: 0x0009668C
		// (set) Token: 0x06001A15 RID: 6677 RVA: 0x00098494 File Offset: 0x00096694
		public BoundLocalizedText StrInOutFeedCheck { get; set; }

		// Token: 0x170007CE RID: 1998
		// (get) Token: 0x06001A16 RID: 6678 RVA: 0x0009849D File Offset: 0x0009669D
		// (set) Token: 0x06001A17 RID: 6679 RVA: 0x000984A5 File Offset: 0x000966A5
		public BoundLocalizedText StrStopConveyorIfError { get; set; }

		// Token: 0x170007CF RID: 1999
		// (get) Token: 0x06001A18 RID: 6680 RVA: 0x000984AE File Offset: 0x000966AE
		// (set) Token: 0x06001A19 RID: 6681 RVA: 0x000984B6 File Offset: 0x000966B6
		public BoundLocalizedText StrTooManyRejects { get; set; }

		// Token: 0x170007D0 RID: 2000
		// (get) Token: 0x06001A1A RID: 6682 RVA: 0x000984BF File Offset: 0x000966BF
		// (set) Token: 0x06001A1B RID: 6683 RVA: 0x000984C7 File Offset: 0x000966C7
		public BoundLocalizedText StrInsightNetConnection { get; set; }

		// Token: 0x170007D1 RID: 2001
		// (get) Token: 0x06001A1C RID: 6684 RVA: 0x000984D0 File Offset: 0x000966D0
		// (set) Token: 0x06001A1D RID: 6685 RVA: 0x000984D8 File Offset: 0x000966D8
		public BoundLocalizedText StrDiskSpaceLow { get; set; }

		// Token: 0x170007D2 RID: 2002
		// (get) Token: 0x06001A1E RID: 6686 RVA: 0x000984E1 File Offset: 0x000966E1
		// (set) Token: 0x06001A1F RID: 6687 RVA: 0x000984E9 File Offset: 0x000966E9
		public BoundLocalizedText StrMessageSafetySwitches { get; set; }

		// Token: 0x170007D3 RID: 2003
		// (get) Token: 0x06001A20 RID: 6688 RVA: 0x000984F2 File Offset: 0x000966F2
		// (set) Token: 0x06001A21 RID: 6689 RVA: 0x000984FA File Offset: 0x000966FA
		public bool WagoExtended { get; set; }

		// Token: 0x170007D4 RID: 2004
		// (get) Token: 0x06001A22 RID: 6690 RVA: 0x00098503 File Offset: 0x00096703
		// (set) Token: 0x06001A23 RID: 6691 RVA: 0x0009850B File Offset: 0x0009670B
		public bool BinMissingVisible { get; set; }

		// Token: 0x170007D5 RID: 2005
		// (get) Token: 0x06001A24 RID: 6692 RVA: 0x00098514 File Offset: 0x00096714
		// (set) Token: 0x06001A25 RID: 6693 RVA: 0x0009851C File Offset: 0x0009671C
		public bool HighLevelSensorsVisible { get; set; }

		// Token: 0x170007D6 RID: 2006
		// (get) Token: 0x06001A26 RID: 6694 RVA: 0x00098525 File Offset: 0x00096725
		// (set) Token: 0x06001A27 RID: 6695 RVA: 0x0009852D File Offset: 0x0009672D
		public bool StopConveyorVisible { get; set; }

		// Token: 0x170007D7 RID: 2007
		// (get) Token: 0x06001A28 RID: 6696 RVA: 0x00098536 File Offset: 0x00096736
		// (set) Token: 0x06001A29 RID: 6697 RVA: 0x0009853E File Offset: 0x0009673E
		public bool AdvancedContacts { get; set; }

		// Token: 0x170007D8 RID: 2008
		// (get) Token: 0x06001A2A RID: 6698 RVA: 0x00098547 File Offset: 0x00096747
		// (set) Token: 0x06001A2B RID: 6699 RVA: 0x0009854F File Offset: 0x0009674F
		public bool InsightNetAvailable { get; set; }

		// Token: 0x170007D9 RID: 2009
		// (get) Token: 0x06001A2C RID: 6700 RVA: 0x00098558 File Offset: 0x00096758
		// (set) Token: 0x06001A2D RID: 6701 RVA: 0x00098560 File Offset: 0x00096760
		public bool OldWAGOVersion { get; set; }

		// Token: 0x170007DA RID: 2010
		// (get) Token: 0x06001A2E RID: 6702 RVA: 0x00098569 File Offset: 0x00096769
		// (set) Token: 0x06001A2F RID: 6703 RVA: 0x00098571 File Offset: 0x00096771
		public RelayCommand CleanUpCommand { get; set; }

		// Token: 0x170007DB RID: 2011
		// (get) Token: 0x06001A30 RID: 6704 RVA: 0x0009857A File Offset: 0x0009677A
		// (set) Token: 0x06001A31 RID: 6705 RVA: 0x00098582 File Offset: 0x00096782
		public List<string> ListMessLevSafeSwitch { get; set; }

		// Token: 0x06001A32 RID: 6706 RVA: 0x0009858C File Offset: 0x0009678C
		public ErrorContactViewModel(ErrorHandling errorHandling, ParameterHandling parHandling, DBConnection dbConnection, AccessRightsManager accessManager)
		{
			this.ErrorHandling = errorHandling;
			this.dbConnection = dbConnection;
			this.accessManager = accessManager;
			this.parHandling = parHandling;
			MachineMode machineMode = MachineMode.Standard;
			MachineType machineType = MachineType.MachineTypeUndefined;
			bool highLevelSensorsVisible = false;
			this.InitializeLocalizedText();
			this.CleanUpCommand = new RelayCommand(new Action<object>(this.CleanUp));
			this.WagoExtended = (parHandling.ReadPar(Definition.ioSubversionPath) != "1");
			dbConnection.GetMachineMode(ref machineMode);
			dbConnection.GetHighLevelSensors(ref highLevelSensorsVisible);
			dbConnection.GetMachineType(ref machineType);
			this.BinMissingVisible = (machineMode == MachineMode.MAndS && accessManager.CurrentUser.Rights >= this.msChangeRight);
			this.HighLevelSensorsVisible = highLevelSensorsVisible;
			this.SetWagoDependentVisibilities(parHandling);
			this.AdvancedContacts = (machineType != MachineType.RayconEX2A && parHandling.ReadPar(Definition.ioSubversionPath) != "1");
			bool insightNetAvailable = false;
			dbConnection.GetInsightNetAvailable(ref insightNetAvailable);
			this.InsightNetAvailable = insightNetAvailable;
			this.initMessagesSafetySwitchMessages(machineType);
		}

		// Token: 0x06001A33 RID: 6707 RVA: 0x0009869C File Offset: 0x0009689C
		private void initMessagesSafetySwitchMessages(MachineType machineType)
		{
			bool flag = false;
			this.dbConnection.GetSafetySwitchMessageAsError(ref flag);
			this.ErrorHandling.MessageLevelSafetySwitch = this.StrInfo.LocalizedText;
			bool flag2 = flag;
			if (flag2)
			{
				this.ErrorHandling.MessageLevelSafetySwitch = this.StrError.LocalizedText;
			}
			string a = this.parHandling.ReadPar(Definition.beltStopOnErrorEnablePath);
			bool flag3 = this.accessManager.CurrentUser.Rights >= BuiltInPriviledges.Service;
			this.ErrorHandling.MessageLevelSafeSwiVisible = false;
			bool flag4 = a == "1";
			if (flag4)
			{
				this.ErrorHandling.StopConveyorIfError = true;
				bool flag5 = flag3 && machineType != MachineType.RayconEX2A;
				if (flag5)
				{
					this.ErrorHandling.MessageLevelSafeSwiVisible = true;
				}
			}
			else
			{
				this.ErrorHandling.StopConveyorIfError = false;
			}
			this.ListMessLevSafeSwitch = new List<string>();
			this.ListMessLevSafeSwitch.Add(this.StrInfo.LocalizedText);
			this.ListMessLevSafeSwitch.Add(this.StrError.LocalizedText);
			this.ErrorHandling.StrInfoSafetySwitch = this.StrInfo.LocalizedText;
			this.ErrorHandling.StrErrorSafetySwitch = this.StrError.LocalizedText;
			this.ErrorHandling.IsService = flag3;
		}

		// Token: 0x06001A34 RID: 6708 RVA: 0x000987F0 File Offset: 0x000969F0
		private void SetWagoDependentVisibilities(ParameterHandling parHandling)
		{
			int num = 7;
			string s = parHandling.ReadPar(Definition.ioVersionPath);
			int num2 = (parHandling.ReadPar(Definition.machineModePath) == "1") ? 1 : 0;
			bool flag = parHandling.ReadPar(Definition.machineModePath) == "4";
			int.TryParse(s, out num);
			bool flag2 = num < 7 || flag;
			if (flag2)
			{
				this.StopConveyorVisible = false;
			}
			else
			{
				this.StopConveyorVisible = true;
			}
			this.OldWAGOVersion = (num < Definition.minNewWagoConfigVersion);
		}

		// Token: 0x06001A35 RID: 6709 RVA: 0x0009887C File Offset: 0x00096A7C
		private void InitializeLocalizedText()
		{
			this.StrError = new BoundLocalizedText("txtError");
			this.StrDetector = new BoundLocalizedText("parItemDetector");
			this.StrXraySource = new BoundLocalizedText("parItemXRaySource");
			this.StrBinFull = new BoundLocalizedText("parItemBinFull");
			this.StrRejectVerify = new BoundLocalizedText("txtRejectVerify");
			this.StrVoltage = new BoundLocalizedText("txtVoltage");
			this.StrTemperature = new BoundLocalizedText("msgTempTooHigh");
			this.StrErrorMotor = new BoundLocalizedText("parItemErrorEngine");
			this.StrLowAirPressure = new BoundLocalizedText("parItemLowAirPressure");
			this.StrErrorCooling = new BoundLocalizedText("parItemErrorCooling");
			this.StrSetErrorContact = new BoundLocalizedText("txtSetErrorOutput");
			this.StrErrorXRayLamp = new BoundLocalizedText("msgLampCurrentLow");
			this.StrStopConveyorIfError = new BoundLocalizedText("txtStopConveyorIfError");
			this.StrBinMissing = new BoundLocalizedText("lbBinMissing");
			this.StrInOutFeedCheck = new BoundLocalizedText("txtInOutCheck");
			this.StrTooManyRejects = new BoundLocalizedText("txtTooManyRejects");
			this.StrInsightNetConnection = new BoundLocalizedText("txtInsightNetConnection");
			this.StrDiskSpaceLow = new BoundLocalizedText("lbDiskSpaceLow");
			this.StrMessageSafetySwitches = new BoundLocalizedText("txtMessageSafetySwitches");
			this.StrInfo = new BoundLocalizedText("txtInfo");
		}

		// Token: 0x06001A36 RID: 6710 RVA: 0x000989E0 File Offset: 0x00096BE0
		private void CleanUp(object dummy)
		{
			MainWindow.localizationManager.Deregister(this.StrError);
			MainWindow.localizationManager.Deregister(this.StrDetector);
			MainWindow.localizationManager.Deregister(this.StrXraySource);
			MainWindow.localizationManager.Deregister(this.StrBinFull);
			MainWindow.localizationManager.Deregister(this.StrRejectVerify);
			MainWindow.localizationManager.Deregister(this.StrVoltage);
			MainWindow.localizationManager.Deregister(this.StrTemperature);
			MainWindow.localizationManager.Deregister(this.StrErrorMotor);
			MainWindow.localizationManager.Deregister(this.StrLowAirPressure);
			MainWindow.localizationManager.Deregister(this.StrErrorCooling);
			MainWindow.localizationManager.Deregister(this.StrSetErrorContact);
			MainWindow.localizationManager.Deregister(this.StrErrorXRayLamp);
			MainWindow.localizationManager.Deregister(this.StrStopConveyorIfError);
			MainWindow.localizationManager.Deregister(this.StrBinMissing);
			MainWindow.localizationManager.Deregister(this.StrInOutFeedCheck);
			MainWindow.localizationManager.Deregister(this.StrTooManyRejects);
			MainWindow.localizationManager.Deregister(this.StrInsightNetConnection);
			MainWindow.localizationManager.Deregister(this.StrDiskSpaceLow);
			MainWindow.localizationManager.Deregister(this.StrMessageSafetySwitches);
			MainWindow.localizationManager.Deregister(this.StrInfo);
		}

		// Token: 0x040010FB RID: 4347
		private BuiltInPriviledges msChangeRight = BuiltInPriviledges.Service;

		// Token: 0x04001119 RID: 4377
		private DBConnection dbConnection;

		// Token: 0x0400111A RID: 4378
		private AccessRightsManager accessManager;

		// Token: 0x0400111B RID: 4379
		private ParameterHandling parHandling;
	}
}
