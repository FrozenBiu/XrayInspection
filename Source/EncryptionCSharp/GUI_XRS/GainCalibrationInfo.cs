using System;

namespace GUI_XRS
{
	// Token: 0x02000135 RID: 309
	public class GainCalibrationInfo
	{
		// Token: 0x170008B1 RID: 2225
		// (get) Token: 0x06001DC6 RID: 7622 RVA: 0x000C1E70 File Offset: 0x000C0070
		// (set) Token: 0x06001DC7 RID: 7623 RVA: 0x000C1E78 File Offset: 0x000C0078
		public int Temperature { get; set; }

		// Token: 0x170008B2 RID: 2226
		// (get) Token: 0x06001DC8 RID: 7624 RVA: 0x000C1E81 File Offset: 0x000C0081
		// (set) Token: 0x06001DC9 RID: 7625 RVA: 0x000C1E89 File Offset: 0x000C0089
		public int Scanrate { get; set; }

		// Token: 0x170008B3 RID: 2227
		// (get) Token: 0x06001DCA RID: 7626 RVA: 0x000C1E92 File Offset: 0x000C0092
		// (set) Token: 0x06001DCB RID: 7627 RVA: 0x000C1E9A File Offset: 0x000C009A
		public double VoltageSetpoint { get; set; }

		// Token: 0x170008B4 RID: 2228
		// (get) Token: 0x06001DCC RID: 7628 RVA: 0x000C1EA3 File Offset: 0x000C00A3
		// (set) Token: 0x06001DCD RID: 7629 RVA: 0x000C1EAB File Offset: 0x000C00AB
		public double CurrentSetpoint { get; set; }

		// Token: 0x170008B5 RID: 2229
		// (get) Token: 0x06001DCE RID: 7630 RVA: 0x000C1EB4 File Offset: 0x000C00B4
		// (set) Token: 0x06001DCF RID: 7631 RVA: 0x000C1EBC File Offset: 0x000C00BC
		public bool CalibrationError { get; set; }

		// Token: 0x06001DD0 RID: 7632 RVA: 0x000C1EC5 File Offset: 0x000C00C5
		public GainCalibrationInfo(int temperature, int scanrate, double voltageSetpoiint, double currentSetpoint, bool calibrationError)
		{
			this.Temperature = temperature;
			this.Scanrate = scanrate;
			this.VoltageSetpoint = voltageSetpoiint;
			this.CurrentSetpoint = currentSetpoint;
			this.CalibrationError = calibrationError;
		}
	}
}
