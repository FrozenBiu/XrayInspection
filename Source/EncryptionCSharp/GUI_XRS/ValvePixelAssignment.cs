using System;

namespace GUI_XRS
{
	// Token: 0x02000055 RID: 85
	public class ValvePixelAssignment
	{
		// Token: 0x17000287 RID: 647
		// (get) Token: 0x060008AF RID: 2223 RVA: 0x0003761C File Offset: 0x0003581C
		// (set) Token: 0x060008B0 RID: 2224 RVA: 0x00037634 File Offset: 0x00035834
		public int ValveIndex
		{
			get
			{
				return this.valveIndex;
			}
			set
			{
				this.valveIndex = value;
			}
		}

		// Token: 0x17000288 RID: 648
		// (get) Token: 0x060008B1 RID: 2225 RVA: 0x00037640 File Offset: 0x00035840
		// (set) Token: 0x060008B2 RID: 2226 RVA: 0x00037658 File Offset: 0x00035858
		public int StartPixel
		{
			get
			{
				return this.startPixel;
			}
			set
			{
				this.startPixel = value;
			}
		}

		// Token: 0x17000289 RID: 649
		// (get) Token: 0x060008B3 RID: 2227 RVA: 0x00037664 File Offset: 0x00035864
		// (set) Token: 0x060008B4 RID: 2228 RVA: 0x0003767C File Offset: 0x0003587C
		public int EndPixel
		{
			get
			{
				return this.endPixel;
			}
			set
			{
				this.endPixel = value;
			}
		}

		// Token: 0x060008B5 RID: 2229 RVA: 0x00037686 File Offset: 0x00035886
		public ValvePixelAssignment() : this(-1, -1, -1)
		{
		}

		// Token: 0x060008B6 RID: 2230 RVA: 0x00037693 File Offset: 0x00035893
		public ValvePixelAssignment(int valveIndex, int startPixel, int endPixel)
		{
			this.valveIndex = valveIndex;
			this.startPixel = startPixel;
			this.endPixel = endPixel;
		}

		// Token: 0x040005EA RID: 1514
		private int valveIndex;

		// Token: 0x040005EB RID: 1515
		private int startPixel;

		// Token: 0x040005EC RID: 1516
		private int endPixel;
	}
}
