using System;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x020000A2 RID: 162
	public static class ExtensionMethods
	{
		// Token: 0x0600103F RID: 4159 RVA: 0x0006566C File Offset: 0x0006386C
		public static bool IsImageToIncreaseProductCounter(this XRSImage image, MachineState machineState, MeanPixelLogger meanPixelLogger, ParameterHandling parHandling)
		{
			return machineState.State != MachineStates.XrayOnCalibrationNotDone && machineState.State != MachineStates.AutoAdjustingXRay && !meanPixelLogger.LogModeStarted && image.Type != ImageType.ConnectionTest && image.Type != ImageType.Debug && image.Type != ImageType.Demo && image.Type != ImageType.Audit && image.Type != ImageType.MaterialCurve && image.Type != ImageType.IVP && (!parHandling.EdgeFilterMode || image.Type == ImageType.Normal);
		}
	}
}
