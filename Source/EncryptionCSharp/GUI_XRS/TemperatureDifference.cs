using System;

namespace GUI_XRS
{
	// Token: 0x02000161 RID: 353
	public class TemperatureDifference
	{
		// Token: 0x1700098F RID: 2447
		// (get) Token: 0x0600211A RID: 8474 RVA: 0x000D7A66 File Offset: 0x000D5C66
		// (set) Token: 0x0600211B RID: 8475 RVA: 0x000D7A6E File Offset: 0x000D5C6E
		public int Temperature { get; set; }

		// Token: 0x17000990 RID: 2448
		// (get) Token: 0x0600211C RID: 8476 RVA: 0x000D7A77 File Offset: 0x000D5C77
		// (set) Token: 0x0600211D RID: 8477 RVA: 0x000D7A7F File Offset: 0x000D5C7F
		public int TemperatureDiff { get; set; }

		// Token: 0x0600211E RID: 8478 RVA: 0x000D7A88 File Offset: 0x000D5C88
		public TemperatureDifference(int temperature, int temperatureDifference)
		{
			this.Temperature = temperature;
			this.TemperatureDiff = temperatureDifference;
		}
	}
}
