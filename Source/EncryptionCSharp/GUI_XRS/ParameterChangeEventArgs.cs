using System;

namespace GUI_XRS
{
	// Token: 0x02000249 RID: 585
	public class ParameterChangeEventArgs : EventArgs
	{
		// Token: 0x06002C61 RID: 11361 RVA: 0x001243A7 File Offset: 0x001225A7
		public ParameterChangeEventArgs(string path, string oldValue, string newValue)
		{
			this.OldValue = oldValue;
			this.NewValue = newValue;
			this.Path = path;
		}

		// Token: 0x17000BFE RID: 3070
		// (get) Token: 0x06002C62 RID: 11362 RVA: 0x001243C9 File Offset: 0x001225C9
		// (set) Token: 0x06002C63 RID: 11363 RVA: 0x001243D1 File Offset: 0x001225D1
		public string Path { get; private set; }

		// Token: 0x17000BFF RID: 3071
		// (get) Token: 0x06002C64 RID: 11364 RVA: 0x001243DA File Offset: 0x001225DA
		// (set) Token: 0x06002C65 RID: 11365 RVA: 0x001243E2 File Offset: 0x001225E2
		public string OldValue { get; private set; }

		// Token: 0x17000C00 RID: 3072
		// (get) Token: 0x06002C66 RID: 11366 RVA: 0x001243EB File Offset: 0x001225EB
		// (set) Token: 0x06002C67 RID: 11367 RVA: 0x001243F3 File Offset: 0x001225F3
		public string NewValue { get; private set; }
	}
}
