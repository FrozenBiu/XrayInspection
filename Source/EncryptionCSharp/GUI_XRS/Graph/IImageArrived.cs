using System;

namespace GUI_XRS.Graph
{
	// Token: 0x02000274 RID: 628
	public interface IImageArrived
	{
		// Token: 0x14000114 RID: 276
		// (add) Token: 0x060034A4 RID: 13476
		// (remove) Token: 0x060034A5 RID: 13477
		event EventHandler<RcvImgEventArgs> ImageArrived;
	}
}
