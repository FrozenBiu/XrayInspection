using System;
using System.ComponentModel;
using System.Diagnostics;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x020000B9 RID: 185
	public class RegionImage : INotifyPropertyChanged
	{
		// Token: 0x1700058E RID: 1422
		// (get) Token: 0x0600125A RID: 4698 RVA: 0x0006E634 File Offset: 0x0006C834
		// (set) Token: 0x0600125B RID: 4699 RVA: 0x0006E64C File Offset: 0x0006C84C
		public int TabHeight
		{
			get
			{
				return this.tabHeight;
			}
			set
			{
				this.tabHeight = value;
				this.OnPropertyChanged("TabHeight");
			}
		}

		// Token: 0x1700058F RID: 1423
		// (get) Token: 0x0600125C RID: 4700 RVA: 0x0006E664 File Offset: 0x0006C864
		// (set) Token: 0x0600125D RID: 4701 RVA: 0x0006E67C File Offset: 0x0006C87C
		public XRSImage XrsImage
		{
			get
			{
				return this.xrsImage;
			}
			set
			{
				this.xrsImage = value;
				this.OnPropertyChanged("XrsImage");
			}
		}

		// Token: 0x1400005E RID: 94
		// (add) Token: 0x0600125E RID: 4702 RVA: 0x0006E694 File Offset: 0x0006C894
		// (remove) Token: 0x0600125F RID: 4703 RVA: 0x0006E6CC File Offset: 0x0006C8CC
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x06001260 RID: 4704 RVA: 0x0006E701 File Offset: 0x0006C901
		public RegionImage(XRSImage image)
		{
			this.TabHeight = 40;
			this.xrsImage = image;
		}

		// Token: 0x06001261 RID: 4705 RVA: 0x0006E71C File Offset: 0x0006C91C
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x04000C20 RID: 3104
		private int tabHeight;

		// Token: 0x04000C21 RID: 3105
		private XRSImage xrsImage;
	}
}
