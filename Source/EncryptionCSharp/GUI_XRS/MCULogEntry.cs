using System;

namespace GUI_XRS
{
	// Token: 0x02000128 RID: 296
	public class MCULogEntry
	{
		// Token: 0x1700086A RID: 2154
		// (get) Token: 0x06001C9B RID: 7323 RVA: 0x000B6E6C File Offset: 0x000B506C
		// (set) Token: 0x06001C9C RID: 7324 RVA: 0x000B6E74 File Offset: 0x000B5074
		public DateTime Time { get; set; }

		// Token: 0x1700086B RID: 2155
		// (get) Token: 0x06001C9D RID: 7325 RVA: 0x000B6E7D File Offset: 0x000B507D
		// (set) Token: 0x06001C9E RID: 7326 RVA: 0x000B6E85 File Offset: 0x000B5085
		public int Temperature { get; set; }

		// Token: 0x1700086C RID: 2156
		// (get) Token: 0x06001C9F RID: 7327 RVA: 0x000B6E8E File Offset: 0x000B508E
		// (set) Token: 0x06001CA0 RID: 7328 RVA: 0x000B6E96 File Offset: 0x000B5096
		public int Scanrate { get; set; }

		// Token: 0x1700086D RID: 2157
		// (get) Token: 0x06001CA1 RID: 7329 RVA: 0x000B6E9F File Offset: 0x000B509F
		// (set) Token: 0x06001CA2 RID: 7330 RVA: 0x000B6EA7 File Offset: 0x000B50A7
		public double VoltageSetpoint { get; set; }

		// Token: 0x1700086E RID: 2158
		// (get) Token: 0x06001CA3 RID: 7331 RVA: 0x000B6EB0 File Offset: 0x000B50B0
		// (set) Token: 0x06001CA4 RID: 7332 RVA: 0x000B6EB8 File Offset: 0x000B50B8
		public double CurrentSetpoint { get; set; }

		// Token: 0x1700086F RID: 2159
		// (get) Token: 0x06001CA5 RID: 7333 RVA: 0x000B6EC1 File Offset: 0x000B50C1
		// (set) Token: 0x06001CA6 RID: 7334 RVA: 0x000B6EC9 File Offset: 0x000B50C9
		public bool GainMode { get; set; }

		// Token: 0x06001CA7 RID: 7335 RVA: 0x000B6ED2 File Offset: 0x000B50D2
		public MCULogEntry(int temperature) : this(temperature, 0, 0.0, 0.0, false)
		{
		}

		// Token: 0x06001CA8 RID: 7336 RVA: 0x000B6EF1 File Offset: 0x000B50F1
		public MCULogEntry(int temperature, int scanrate, double voltageSetpoint, double currentSetpoint, bool gainMode)
		{
			this.Time = DateTime.Now;
			this.Temperature = temperature;
			this.Scanrate = scanrate;
			this.VoltageSetpoint = voltageSetpoint;
			this.CurrentSetpoint = currentSetpoint;
			this.GainMode = gainMode;
		}
	}
}
