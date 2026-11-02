using System;

namespace GUI_XRS
{
	// Token: 0x02000087 RID: 135
	internal abstract class MonitoringViewModel
	{
		// Token: 0x06000D4D RID: 3405 RVA: 0x00029776 File Offset: 0x00027976
		public MonitoringViewModel()
		{
		}

		// Token: 0x06000D4E RID: 3406
		public abstract void getNumKeyboardNumber(MonitoringItem pItem, int v);

		// Token: 0x06000D4F RID: 3407
		public abstract void ShowDiagramm();
	}
}
