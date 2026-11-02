using System;
using System.ComponentModel;
using System.Diagnostics;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x020000C0 RID: 192
	public class Machine : INotifyPropertyChanged
	{
		// Token: 0x170005EF RID: 1519
		// (get) Token: 0x0600134E RID: 4942 RVA: 0x00071220 File Offset: 0x0006F420
		public static Machine Instance
		{
			get
			{
				return Machine.instance;
			}
		}

		// Token: 0x14000060 RID: 96
		// (add) Token: 0x0600134F RID: 4943 RVA: 0x00071238 File Offset: 0x0006F438
		// (remove) Token: 0x06001350 RID: 4944 RVA: 0x00071270 File Offset: 0x0006F470
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x170005F0 RID: 1520
		// (get) Token: 0x06001351 RID: 4945 RVA: 0x000712A8 File Offset: 0x0006F4A8
		// (set) Token: 0x06001352 RID: 4946 RVA: 0x000712C0 File Offset: 0x0006F4C0
		public MachineType MachineType
		{
			get
			{
				return this.machineType;
			}
			set
			{
				this.machineType = value;
				this.OnPropertyChanged("MachineType");
			}
		}

		// Token: 0x06001354 RID: 4948 RVA: 0x00029776 File Offset: 0x00027976
		private Machine()
		{
		}

		// Token: 0x06001355 RID: 4949 RVA: 0x000712E4 File Offset: 0x0006F4E4
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x04000C99 RID: 3225
		private static readonly Machine instance = new Machine();

		// Token: 0x04000C9A RID: 3226
		private MachineType machineType;
	}
}
