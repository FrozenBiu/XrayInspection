using System;
using System.Net.NetworkInformation;
using System.Threading;

namespace GUI_XRS
{
	// Token: 0x0200009A RID: 154
	public class ComServerClient : Network
	{
		// Token: 0x06000F9C RID: 3996 RVA: 0x0005D3CB File Offset: 0x0005B5CB
		public ComServerClient() : this("192.168.0.90")
		{
		}

		// Token: 0x06000F9D RID: 3997 RVA: 0x0005D3DA File Offset: 0x0005B5DA
		public ComServerClient(string ipAddress) : base(ipAddress, 8000, "ComServerClient")
		{
			base.SetReceiveTimeout(0);
			base.ReconnectInterval = 1500;
		}

		// Token: 0x06000F9E RID: 3998 RVA: 0x0005D404 File Offset: 0x0005B604
		private void PingComServer()
		{
			while (this.pingThreadRunning)
			{
				try
				{
					using (Ping ping = new Ping())
					{
						PingReply pingReply = ping.Send("192.168.0.90", 1000);
						bool flag = pingReply.Status > IPStatus.Success;
						if (flag)
						{
							bool flag2 = this.tcpClient != null;
							if (flag2)
							{
								this.tcpClient.Close();
							}
							this.pingThreadRunning = false;
							base.ConnectionEstablished = false;
							base.StartConnect();
						}
						else
						{
							Thread.Sleep(3000);
						}
					}
				}
				catch
				{
					Thread.Sleep(3000);
				}
			}
		}

		// Token: 0x06000F9F RID: 3999 RVA: 0x0005D4CC File Offset: 0x0005B6CC
		public void StartPingComServer()
		{
			bool flag = !this.pingThreadRunning;
			if (flag)
			{
				this.pingThreadRunning = true;
				new Thread(new ThreadStart(this.PingComServer))
				{
					Name = "Ping Com Server"
				}.Start();
			}
		}

		// Token: 0x06000FA0 RID: 4000 RVA: 0x0005D515 File Offset: 0x0005B715
		public void StopPingComServer()
		{
			this.pingThreadRunning = false;
		}

		// Token: 0x04000A70 RID: 2672
		private const int comServerPort = 8000;

		// Token: 0x04000A71 RID: 2673
		private const string defaultComServerIP = "192.168.0.90";

		// Token: 0x04000A72 RID: 2674
		private const int pingInterval = 3000;

		// Token: 0x04000A73 RID: 2675
		private bool pingThreadRunning;
	}
}
