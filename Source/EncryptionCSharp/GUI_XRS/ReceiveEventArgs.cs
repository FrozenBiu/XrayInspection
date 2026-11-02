using System;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x02000105 RID: 261
	public class ReceiveEventArgs : EventArgs
	{
		// Token: 0x060018B1 RID: 6321 RVA: 0x00093F53 File Offset: 0x00092153
		public ReceiveEventArgs(XRSImage image)
		{
			this.image = image;
		}

		// Token: 0x17000751 RID: 1873
		// (get) Token: 0x060018B2 RID: 6322 RVA: 0x00093F64 File Offset: 0x00092164
		public XRSImage Image
		{
			get
			{
				return this.image;
			}
		}

		// Token: 0x04001027 RID: 4135
		private readonly XRSImage image;
	}
}
