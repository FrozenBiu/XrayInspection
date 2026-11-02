using System;

namespace Microsoft.Research.DynamicDataDisplay.ViewportRestrictions
{
	// Token: 0x02000007 RID: 7
	public class ViewportAxesRangeRestriction : ViewportRestriction
	{
		// Token: 0x06000028 RID: 40 RVA: 0x000023BC File Offset: 0x000005BC
		public override DataRect Apply(DataRect oldVisible, DataRect newVisible, Viewport2D viewport)
		{
			bool flag = this.XRange != null;
			if (flag)
			{
				newVisible.XMin = this.XRange.Start;
				newVisible.Width = this.XRange.End - this.XRange.Start;
			}
			bool flag2 = this.YRange != null;
			if (flag2)
			{
				newVisible.YMin = this.YRange.Start;
				newVisible.Height = this.YRange.End - this.YRange.Start;
			}
			return newVisible;
		}

		// Token: 0x04000099 RID: 153
		public DisplayRange XRange = null;

		// Token: 0x0400009A RID: 154
		public DisplayRange YRange = null;
	}
}
