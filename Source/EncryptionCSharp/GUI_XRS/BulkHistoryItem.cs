using System;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x02000044 RID: 68
	public class BulkHistoryItem : PreviewItem
	{
		// Token: 0x17000204 RID: 516
		// (get) Token: 0x060006E5 RID: 1765 RVA: 0x0002D408 File Offset: 0x0002B608
		// (set) Token: 0x060006E6 RID: 1766 RVA: 0x0002D420 File Offset: 0x0002B620
		public bool Selected
		{
			get
			{
				return this.selected;
			}
			set
			{
				this.selected = value;
				this.OnPropertyChanged("Selected");
			}
		}

		// Token: 0x060006E7 RID: 1767 RVA: 0x0002D436 File Offset: 0x0002B636
		public BulkHistoryItem(string fileName, StoreImageData imageArchiver) : base(fileName, imageArchiver)
		{
		}

		// Token: 0x040004CA RID: 1226
		private bool selected;
	}
}
