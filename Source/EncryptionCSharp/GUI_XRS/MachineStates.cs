using System;

namespace GUI_XRS
{
	// Token: 0x02000230 RID: 560
	public enum MachineStates
	{
		// Token: 0x04001A95 RID: 6805
		ConveyorOff,
		// Token: 0x04001A96 RID: 6806
		ConveyorOnKeyOpened,
		// Token: 0x04001A97 RID: 6807
		ConveyorOnKeyClosedXrayOff,
		// Token: 0x04001A98 RID: 6808
		Reseasoning,
		// Token: 0x04001A99 RID: 6809
		XrayOnCalibrationNotDone,
		// Token: 0x04001A9A RID: 6810
		Autotrain,
		// Token: 0x04001A9B RID: 6811
		ProductChange,
		// Token: 0x04001A9C RID: 6812
		Inspecting,
		// Token: 0x04001A9D RID: 6813
		PreAuditCheck,
		// Token: 0x04001A9E RID: 6814
		AuditCheck,
		// Token: 0x04001A9F RID: 6815
		TestSuiteActive,
		// Token: 0x04001AA0 RID: 6816
		MovingSource,
		// Token: 0x04001AA1 RID: 6817
		ViewingPlot,
		// Token: 0x04001AA2 RID: 6818
		ImageAcquiring,
		// Token: 0x04001AA3 RID: 6819
		AutoAdjustingXRay,
		// Token: 0x04001AA4 RID: 6820
		NoConnectToSourceOrDetector,
		// Token: 0x04001AA5 RID: 6821
		StateForNoBeltStartOnError
	}
}
