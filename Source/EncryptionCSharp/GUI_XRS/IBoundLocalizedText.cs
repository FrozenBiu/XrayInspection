using System;
using System.ComponentModel;

namespace GUI_XRS
{
	// Token: 0x02000221 RID: 545
	public interface IBoundLocalizedText : INotifyPropertyChanged
	{
		// Token: 0x17000B2B RID: 2859
		// (get) Token: 0x060028F6 RID: 10486
		// (set) Token: 0x060028F7 RID: 10487
		string LocalizedText { get; set; }

		// Token: 0x17000B2C RID: 2860
		// (get) Token: 0x060028F8 RID: 10488
		// (set) Token: 0x060028F9 RID: 10489
		string ResourceKey { get; set; }
	}
}
