using System;
using System.Collections.Generic;

namespace GUI_XRS
{
	// Token: 0x02000065 RID: 101
	public class DTNetTdiDefectPixelList
	{
		// Token: 0x170002C8 RID: 712
		// (get) Token: 0x0600097A RID: 2426 RVA: 0x0003A5D0 File Offset: 0x000387D0
		public List<DTNetNoTdiDefectPixel> NoTdiDefectPixelList
		{
			get
			{
				return this.noTdiDefectPixelList;
			}
		}

		// Token: 0x170002C9 RID: 713
		// (get) Token: 0x0600097B RID: 2427 RVA: 0x0003A5E8 File Offset: 0x000387E8
		public List<DTNetTdiDefectPixel> TdiDefectPixelList
		{
			get
			{
				return this.tdiDefectPixelList;
			}
		}

		// Token: 0x0600097D RID: 2429 RVA: 0x0003A600 File Offset: 0x00038800
		public void AddNoTdiDefectPixelList(List<DTNetNoTdiDefectPixel> noTdiDefectPixelList)
		{
			this.noTdiDefectPixelList = noTdiDefectPixelList;
			this.tdiDefectPixelList = new List<DTNetTdiDefectPixel>();
			DTNetTdiDefectPixel dtnetTdiDefectPixel = new DTNetTdiDefectPixel();
			bool flag = noTdiDefectPixelList.Count == 0;
			if (!flag)
			{
				dtnetTdiDefectPixel.AddNoTdiPixelIfSameTdiPixel(noTdiDefectPixelList[0]);
				this.tdiDefectPixelList.Add(dtnetTdiDefectPixel);
				for (int i = 1; i < noTdiDefectPixelList.Count; i++)
				{
					bool flag2 = false;
					for (int j = 0; j < this.tdiDefectPixelList.Count; j++)
					{
						bool flag3 = noTdiDefectPixelList[i].TdiPixel == this.tdiDefectPixelList[j].TdiPixel;
						if (flag3)
						{
							this.tdiDefectPixelList[j].AddNoTdiPixelIfSameTdiPixel(noTdiDefectPixelList[i]);
							flag2 = true;
							break;
						}
					}
					bool flag4 = !flag2;
					if (flag4)
					{
						dtnetTdiDefectPixel = new DTNetTdiDefectPixel();
						dtnetTdiDefectPixel.AddNoTdiPixelIfSameTdiPixel(noTdiDefectPixelList[i]);
						this.tdiDefectPixelList.Add(dtnetTdiDefectPixel);
					}
				}
			}
		}

		// Token: 0x0600097E RID: 2430 RVA: 0x0003A70C File Offset: 0x0003890C
		public void Clear()
		{
			bool flag = this.noTdiDefectPixelList != null;
			if (flag)
			{
				this.noTdiDefectPixelList.Clear();
			}
			bool flag2 = this.tdiDefectPixelList != null;
			if (flag2)
			{
				this.tdiDefectPixelList.Clear();
			}
		}

		// Token: 0x0600097F RID: 2431 RVA: 0x0003A74C File Offset: 0x0003894C
		public override string ToString()
		{
			string text = "";
			bool flag = this.tdiDefectPixelList != null;
			if (flag)
			{
				foreach (DTNetTdiDefectPixel dtnetTdiDefectPixel in this.tdiDefectPixelList)
				{
					text = text + dtnetTdiDefectPixel.ToStringAsLine() + "\n";
				}
			}
			return text;
		}

		// Token: 0x0400067E RID: 1662
		private List<DTNetNoTdiDefectPixel> noTdiDefectPixelList;

		// Token: 0x0400067F RID: 1663
		private List<DTNetTdiDefectPixel> tdiDefectPixelList;
	}
}
