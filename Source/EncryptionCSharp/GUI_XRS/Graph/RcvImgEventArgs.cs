using System;
using XrsImage;

namespace GUI_XRS.Graph
{
	// Token: 0x02000276 RID: 630
	public class RcvImgEventArgs : EventArgs
	{
		// Token: 0x060034A8 RID: 13480 RVA: 0x0014F2B8 File Offset: 0x0014D4B8
		public RcvImgEventArgs(IXRSImage image)
		{
			this.image = image;
		}

		// Token: 0x17001118 RID: 4376
		// (get) Token: 0x060034A9 RID: 13481 RVA: 0x0014F2CC File Offset: 0x0014D4CC
		public IXRSImage Image
		{
			get
			{
				return this.image;
			}
		}

		// Token: 0x04001EED RID: 7917
		private readonly IXRSImage image;
	}
}
