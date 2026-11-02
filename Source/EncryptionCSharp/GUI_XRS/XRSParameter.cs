using System;

namespace GUI_XRS
{
	// Token: 0x02000107 RID: 263
	public class XRSParameter
	{
		// Token: 0x17000752 RID: 1874
		// (get) Token: 0x060018B3 RID: 6323 RVA: 0x00093F7C File Offset: 0x0009217C
		// (set) Token: 0x060018B4 RID: 6324 RVA: 0x00093F84 File Offset: 0x00092184
		public byte Version { get; set; }

		// Token: 0x17000753 RID: 1875
		// (get) Token: 0x060018B5 RID: 6325 RVA: 0x00093F8D File Offset: 0x0009218D
		// (set) Token: 0x060018B6 RID: 6326 RVA: 0x00093F95 File Offset: 0x00092195
		public byte Command { get; set; }

		// Token: 0x17000754 RID: 1876
		// (get) Token: 0x060018B7 RID: 6327 RVA: 0x00093F9E File Offset: 0x0009219E
		// (set) Token: 0x060018B8 RID: 6328 RVA: 0x00093FA6 File Offset: 0x000921A6
		public byte Type { get; set; }

		// Token: 0x17000755 RID: 1877
		// (get) Token: 0x060018B9 RID: 6329 RVA: 0x00093FAF File Offset: 0x000921AF
		// (set) Token: 0x060018BA RID: 6330 RVA: 0x00093FB7 File Offset: 0x000921B7
		public short Length0 { get; set; }

		// Token: 0x17000756 RID: 1878
		// (get) Token: 0x060018BB RID: 6331 RVA: 0x00093FC0 File Offset: 0x000921C0
		// (set) Token: 0x060018BC RID: 6332 RVA: 0x00093FC8 File Offset: 0x000921C8
		public short Length1 { get; set; }

		// Token: 0x17000757 RID: 1879
		// (get) Token: 0x060018BD RID: 6333 RVA: 0x00093FD1 File Offset: 0x000921D1
		// (set) Token: 0x060018BE RID: 6334 RVA: 0x00093FD9 File Offset: 0x000921D9
		public byte[] Reserved { get; set; }

		// Token: 0x17000758 RID: 1880
		// (get) Token: 0x060018BF RID: 6335 RVA: 0x00093FE2 File Offset: 0x000921E2
		// (set) Token: 0x060018C0 RID: 6336 RVA: 0x00093FEA File Offset: 0x000921EA
		public byte[] Data0 { get; set; }

		// Token: 0x17000759 RID: 1881
		// (get) Token: 0x060018C1 RID: 6337 RVA: 0x00093FF3 File Offset: 0x000921F3
		// (set) Token: 0x060018C2 RID: 6338 RVA: 0x00093FFB File Offset: 0x000921FB
		public byte[] Data1 { get; set; }

		// Token: 0x060018C3 RID: 6339 RVA: 0x00094004 File Offset: 0x00092204
		public XRSParameter(byte[] bufferHeader)
		{
			this.Version = bufferHeader[0];
			this.Command = bufferHeader[1];
			this.Type = bufferHeader[2];
			this.Length0 = BitConverter.ToInt16(bufferHeader, 3);
			this.Length1 = BitConverter.ToInt16(bufferHeader, 5);
			this.Reserved = new byte[5];
			this.Data0 = new byte[(int)this.Length0];
			this.Data1 = new byte[(int)this.Length1];
		}

		// Token: 0x060018C4 RID: 6340 RVA: 0x00094084 File Offset: 0x00092284
		public XRSParameter()
		{
			this.Reserved = new byte[5];
		}

		// Token: 0x060018C5 RID: 6341 RVA: 0x0009409C File Offset: 0x0009229C
		public byte[] CreateHeader()
		{
			byte[] array = new byte[(int)XRSParameter.parameterHeaderSize];
			array[0] = this.Version;
			array[1] = this.Command;
			array[2] = this.Type;
			BitConverter.GetBytes(this.Length0).CopyTo(array, 3);
			BitConverter.GetBytes(this.Length1).CopyTo(array, 5);
			return array;
		}

		// Token: 0x0400102F RID: 4143
		public static readonly byte parameterHeaderSize = 12;
	}
}
