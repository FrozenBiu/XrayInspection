using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace GUI_XRS
{
	// Token: 0x020000F1 RID: 241
	public class VideojetPrinter
	{
		// Token: 0x17000713 RID: 1811
		// (get) Token: 0x0600178B RID: 6027 RVA: 0x0008EFC4 File Offset: 0x0008D1C4
		// (set) Token: 0x0600178C RID: 6028 RVA: 0x0008EFCC File Offset: 0x0008D1CC
		public bool Available { get; set; }

		// Token: 0x17000714 RID: 1812
		// (get) Token: 0x0600178D RID: 6029 RVA: 0x0008EFD5 File Offset: 0x0008D1D5
		// (set) Token: 0x0600178E RID: 6030 RVA: 0x0008EFDD File Offset: 0x0008D1DD
		public bool Enabled { get; set; }

		// Token: 0x17000715 RID: 1813
		// (get) Token: 0x0600178F RID: 6031 RVA: 0x0008EFE8 File Offset: 0x0008D1E8
		// (set) Token: 0x06001790 RID: 6032 RVA: 0x0008F005 File Offset: 0x0008D205
		public string PrinterIP
		{
			get
			{
				return this.zipherCom.ServerIP;
			}
			set
			{
				this.zipherCom.ServerIP = value;
			}
		}

		// Token: 0x17000716 RID: 1814
		// (get) Token: 0x06001791 RID: 6033 RVA: 0x0008F018 File Offset: 0x0008D218
		// (set) Token: 0x06001792 RID: 6034 RVA: 0x0008F035 File Offset: 0x0008D235
		public int Port
		{
			get
			{
				return this.zipherCom.Port;
			}
			set
			{
				this.zipherCom.Port = value;
			}
		}

		// Token: 0x17000717 RID: 1815
		// (get) Token: 0x06001793 RID: 6035 RVA: 0x0008F048 File Offset: 0x0008D248
		// (set) Token: 0x06001794 RID: 6036 RVA: 0x0008F065 File Offset: 0x0008D265
		public int Timeout
		{
			get
			{
				return this.zipherCom.Timeout;
			}
			set
			{
				this.zipherCom.Timeout = value;
			}
		}

		// Token: 0x17000718 RID: 1816
		// (get) Token: 0x06001795 RID: 6037 RVA: 0x0008F075 File Offset: 0x0008D275
		// (set) Token: 0x06001796 RID: 6038 RVA: 0x0008F07D File Offset: 0x0008D27D
		public bool StopConveyorOnError { get; set; }

		// Token: 0x17000719 RID: 1817
		// (get) Token: 0x06001797 RID: 6039 RVA: 0x0008F088 File Offset: 0x0008D288
		public List<string> SentTimestamps
		{
			get
			{
				return this.sentTimestamps;
			}
		}

		// Token: 0x14000075 RID: 117
		// (add) Token: 0x06001798 RID: 6040 RVA: 0x0008F0A0 File Offset: 0x0008D2A0
		// (remove) Token: 0x06001799 RID: 6041 RVA: 0x0008F0D8 File Offset: 0x0008D2D8
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event EventHandler TimestampIsSent;

		// Token: 0x0600179A RID: 6042 RVA: 0x0008F10D File Offset: 0x0008D30D
		public VideojetPrinter(string printerIP, int port, bool stopConveyorOnError)
		{
			this.StopConveyorOnError = stopConveyorOnError;
			this.sentTimestamps = new List<string>();
			this.zipherCom = new ZipherCom(printerIP, port);
			this.PrinterIP = printerIP;
			this.Port = port;
		}

		// Token: 0x0600179B RID: 6043 RVA: 0x0008F148 File Offset: 0x0008D348
		public bool Connect()
		{
			return this.zipherCom.Connect();
		}

		// Token: 0x0600179C RID: 6044 RVA: 0x0008F168 File Offset: 0x0008D368
		public bool Disconnect()
		{
			return this.zipherCom.Disconnect();
		}

		// Token: 0x0600179D RID: 6045 RVA: 0x0008F188 File Offset: 0x0008D388
		public bool Print(string timestamp)
		{
			bool flag = !this.Enabled;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = this.zipherCom.SendTimestamp(timestamp);
				if (flag2)
				{
					this.AddTimestampToList(timestamp + "         Sent at: " + DateTime.Now.ToString("yyyy MM dd HH:mm:ss.fff"));
					this.OnTimestampIsSent();
					result = true;
				}
				else
				{
					result = false;
				}
			}
			return result;
		}

		// Token: 0x0600179E RID: 6046 RVA: 0x0008F1F0 File Offset: 0x0008D3F0
		public bool ClearQueue()
		{
			return this.zipherCom.ClearQueue();
		}

		// Token: 0x0600179F RID: 6047 RVA: 0x0008F210 File Offset: 0x0008D410
		private void AddTimestampToList(string timestamp)
		{
			bool flag = this.sentTimestamps.Count == 20;
			if (flag)
			{
				this.sentTimestamps.RemoveAt(0);
			}
			this.sentTimestamps.Add(timestamp);
		}

		// Token: 0x060017A0 RID: 6048 RVA: 0x0008F24C File Offset: 0x0008D44C
		protected virtual void OnTimestampIsSent()
		{
			bool flag = this.TimestampIsSent != null;
			if (flag)
			{
				this.TimestampIsSent(this, new EventArgs());
			}
		}

		// Token: 0x04000F80 RID: 3968
		private const int maxSentTimestamps = 20;

		// Token: 0x04000F81 RID: 3969
		private List<string> sentTimestamps;

		// Token: 0x04000F82 RID: 3970
		private ZipherCom zipherCom;
	}
}
