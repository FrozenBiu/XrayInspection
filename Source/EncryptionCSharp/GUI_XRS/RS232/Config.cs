using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO.Ports;

namespace GUI_XRS.RS232
{
	// Token: 0x02000260 RID: 608
	internal class Config : INotifyPropertyChanged
	{
		// Token: 0x1400010A RID: 266
		// (add) Token: 0x06002EBF RID: 11967 RVA: 0x00132EC8 File Offset: 0x001310C8
		// (remove) Token: 0x06002EC0 RID: 11968 RVA: 0x00132F00 File Offset: 0x00131100
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x06002EC1 RID: 11969 RVA: 0x00132F38 File Offset: 0x00131138
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x17000C7B RID: 3195
		// (get) Token: 0x06002EC2 RID: 11970 RVA: 0x00132F68 File Offset: 0x00131168
		// (set) Token: 0x06002EC3 RID: 11971 RVA: 0x00132F80 File Offset: 0x00131180
		public uint Port
		{
			get
			{
				return this.port;
			}
			set
			{
				bool flag = this.port != value;
				if (flag)
				{
					this.port = value;
					this.OnPropertyChanged("Port");
				}
			}
		}

		// Token: 0x17000C7C RID: 3196
		// (get) Token: 0x06002EC4 RID: 11972 RVA: 0x00132FB4 File Offset: 0x001311B4
		// (set) Token: 0x06002EC5 RID: 11973 RVA: 0x00132FCC File Offset: 0x001311CC
		public int Baudrate
		{
			get
			{
				return this.baudrate;
			}
			set
			{
				bool flag = this.baudrate != value;
				if (flag)
				{
					this.baudrate = value;
					this.OnPropertyChanged("Baudrate");
				}
			}
		}

		// Token: 0x17000C7D RID: 3197
		// (get) Token: 0x06002EC6 RID: 11974 RVA: 0x00133000 File Offset: 0x00131200
		// (set) Token: 0x06002EC7 RID: 11975 RVA: 0x00133018 File Offset: 0x00131218
		public int Databits
		{
			get
			{
				return this.databits;
			}
			set
			{
				bool flag = this.databits != value;
				if (flag)
				{
					this.databits = value;
					this.OnPropertyChanged("Databits");
				}
			}
		}

		// Token: 0x17000C7E RID: 3198
		// (get) Token: 0x06002EC8 RID: 11976 RVA: 0x0013304C File Offset: 0x0013124C
		// (set) Token: 0x06002EC9 RID: 11977 RVA: 0x00133064 File Offset: 0x00131264
		public Parity Parity
		{
			get
			{
				return this.parity;
			}
			set
			{
				bool flag = this.parity != value;
				if (flag)
				{
					this.parity = value;
					this.OnPropertyChanged("Parity");
				}
			}
		}

		// Token: 0x17000C7F RID: 3199
		// (get) Token: 0x06002ECA RID: 11978 RVA: 0x00133098 File Offset: 0x00131298
		// (set) Token: 0x06002ECB RID: 11979 RVA: 0x001330B0 File Offset: 0x001312B0
		public StopBits StopBits
		{
			get
			{
				return this.stopBits;
			}
			set
			{
				bool flag = this.stopBits != value;
				if (flag)
				{
					this.stopBits = value;
					this.OnPropertyChanged("StopBits");
				}
			}
		}

		// Token: 0x04001D5B RID: 7515
		private uint port;

		// Token: 0x04001D5C RID: 7516
		private int baudrate;

		// Token: 0x04001D5D RID: 7517
		private int databits;

		// Token: 0x04001D5E RID: 7518
		private Parity parity;

		// Token: 0x04001D5F RID: 7519
		private StopBits stopBits;
	}
}
