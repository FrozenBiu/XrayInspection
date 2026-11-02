using System;

namespace GUI_XRS
{
	// Token: 0x02000066 RID: 102
	public class DTNetNoTdiDefectPixel
	{
		// Token: 0x170002CA RID: 714
		// (get) Token: 0x06000980 RID: 2432 RVA: 0x0003A7CC File Offset: 0x000389CC
		public int NoTdiPixel
		{
			get
			{
				return this.noTdiPixel;
			}
		}

		// Token: 0x170002CB RID: 715
		// (get) Token: 0x06000981 RID: 2433 RVA: 0x0003A7E4 File Offset: 0x000389E4
		public int TdiPixel
		{
			get
			{
				return this.tdiPixel;
			}
		}

		// Token: 0x170002CC RID: 716
		// (get) Token: 0x06000982 RID: 2434 RVA: 0x0003A7FC File Offset: 0x000389FC
		public int NumModule
		{
			get
			{
				return this.numModule;
			}
		}

		// Token: 0x170002CD RID: 717
		// (get) Token: 0x06000983 RID: 2435 RVA: 0x0003A814 File Offset: 0x00038A14
		public int TdiStage
		{
			get
			{
				return this.tdiStage;
			}
		}

		// Token: 0x170002CE RID: 718
		// (get) Token: 0x06000984 RID: 2436 RVA: 0x0003A82C File Offset: 0x00038A2C
		public int TdiPixelOnModule
		{
			get
			{
				return this.tdiPixelOnModule;
			}
		}

		// Token: 0x06000985 RID: 2437 RVA: 0x0003A844 File Offset: 0x00038A44
		public DTNetNoTdiDefectPixel(int noTdiDefectPixel)
		{
			this.noTdiPixel = noTdiDefectPixel;
			this.CalculateValues(noTdiDefectPixel);
		}

		// Token: 0x06000986 RID: 2438 RVA: 0x0003A860 File Offset: 0x00038A60
		public string ToStringAsLine()
		{
			string str = "";
			str = str + "NoTdiPixel: " + this.noTdiPixel.ToString("D5") + ", ";
			str = str + "TdiPixel: " + this.tdiPixel.ToString("D4") + ", ";
			str = str + "NumModule: " + this.numModule.ToString("D2") + ", ";
			str = str + "TdiStage: " + this.tdiStage.ToString() + ", ";
			return str + "TdiPixelOnModule: " + this.tdiPixelOnModule.ToString("D3");
		}

		// Token: 0x06000987 RID: 2439 RVA: 0x0003A914 File Offset: 0x00038B14
		private void CalculateValues(int noTdiDefectPixel)
		{
			this.numModule = noTdiDefectPixel / 1024 + 1;
			this.tdiStage = noTdiDefectPixel % 1024 / 128 + 1;
			this.tdiPixelOnModule = noTdiDefectPixel % 128;
			this.tdiPixel = noTdiDefectPixel % 128 + (this.numModule - 1) * 128;
		}

		// Token: 0x04000680 RID: 1664
		public const int NumPixelPerModule = 1024;

		// Token: 0x04000681 RID: 1665
		public const int NumPixelPerTdiStage = 128;

		// Token: 0x04000682 RID: 1666
		public const int NumTdiStages = 8;

		// Token: 0x04000683 RID: 1667
		private int noTdiPixel;

		// Token: 0x04000684 RID: 1668
		private int tdiPixel;

		// Token: 0x04000685 RID: 1669
		private int numModule;

		// Token: 0x04000686 RID: 1670
		private int tdiStage;

		// Token: 0x04000687 RID: 1671
		private int tdiPixelOnModule;
	}
}
