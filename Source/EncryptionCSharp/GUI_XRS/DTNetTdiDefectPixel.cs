using System;
using System.Collections.Generic;

namespace GUI_XRS
{
	// Token: 0x02000063 RID: 99
	public class DTNetTdiDefectPixel
	{
		// Token: 0x170002C4 RID: 708
		// (get) Token: 0x06000972 RID: 2418 RVA: 0x0003A3F8 File Offset: 0x000385F8
		public int TdiPixel
		{
			get
			{
				return this.tdiPixel;
			}
		}

		// Token: 0x170002C5 RID: 709
		// (get) Token: 0x06000973 RID: 2419 RVA: 0x0003A410 File Offset: 0x00038610
		public int NumModule
		{
			get
			{
				return this.numModule;
			}
		}

		// Token: 0x170002C6 RID: 710
		// (get) Token: 0x06000974 RID: 2420 RVA: 0x0003A428 File Offset: 0x00038628
		public int TdiPixelOnModule
		{
			get
			{
				return this.tdiPixelOnModule;
			}
		}

		// Token: 0x170002C7 RID: 711
		// (get) Token: 0x06000975 RID: 2421 RVA: 0x0003A440 File Offset: 0x00038640
		public int NumDefectTdiStages
		{
			get
			{
				return this.numDefectTdiStages;
			}
		}

		// Token: 0x06000976 RID: 2422 RVA: 0x0003A458 File Offset: 0x00038658
		public DTNetTdiDefectPixel()
		{
			this.noTdiDefectPixelList = new List<DTNetNoTdiDefectPixel>();
			this.numDefectTdiStages = 0;
		}

		// Token: 0x06000977 RID: 2423 RVA: 0x0003A474 File Offset: 0x00038674
		public void AddNoTdiPixelIfSameTdiPixel(DTNetNoTdiDefectPixel noTdiDefectPixel)
		{
			bool flag = this.noTdiDefectPixelList.Count == 0;
			if (flag)
			{
				this.noTdiDefectPixelList.Add(noTdiDefectPixel);
				this.tdiPixel = noTdiDefectPixel.TdiPixel;
				this.numModule = noTdiDefectPixel.NumModule;
				this.tdiPixelOnModule = noTdiDefectPixel.TdiPixelOnModule;
				this.numDefectTdiStages = 1;
			}
			else
			{
				bool flag2 = noTdiDefectPixel.TdiPixel == this.noTdiDefectPixelList[0].TdiPixel;
				if (flag2)
				{
					this.noTdiDefectPixelList.Add(noTdiDefectPixel);
					this.numDefectTdiStages++;
				}
			}
		}

		// Token: 0x06000978 RID: 2424 RVA: 0x0003A50C File Offset: 0x0003870C
		public void Clear()
		{
			this.noTdiDefectPixelList.Clear();
			this.tdiPixel = 0;
			this.numModule = 0;
			this.tdiPixelOnModule = 0;
			this.numDefectTdiStages = 0;
		}

		// Token: 0x06000979 RID: 2425 RVA: 0x0003A538 File Offset: 0x00038738
		public string ToStringAsLine()
		{
			string str = "";
			str = str + "TdiPixel: " + this.tdiPixel.ToString("D4") + ", ";
			str = str + "NumModule: " + this.numModule.ToString("D2") + ", ";
			str = str + "TdiPixelOnModule: " + this.tdiPixelOnModule.ToString("D3") + ", ";
			return str + "DefectStage(s): " + this.numDefectTdiStages.ToString() + " / 8";
		}

		// Token: 0x04000674 RID: 1652
		private List<DTNetNoTdiDefectPixel> noTdiDefectPixelList;

		// Token: 0x04000675 RID: 1653
		private int tdiPixel;

		// Token: 0x04000676 RID: 1654
		private int numModule;

		// Token: 0x04000677 RID: 1655
		private int tdiPixelOnModule;

		// Token: 0x04000678 RID: 1656
		private int numDefectTdiStages;
	}
}
