using System;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;

namespace GUI_XRS
{
	// Token: 0x02000101 RID: 257
	public class Network
	{
		// Token: 0x1700074E RID: 1870
		// (get) Token: 0x0600188D RID: 6285 RVA: 0x000934A5 File Offset: 0x000916A5
		// (set) Token: 0x0600188E RID: 6286 RVA: 0x000934AD File Offset: 0x000916AD
		public bool ConnectionEstablished { get; set; }

		// Token: 0x1700074F RID: 1871
		// (get) Token: 0x0600188F RID: 6287 RVA: 0x000934B8 File Offset: 0x000916B8
		// (set) Token: 0x06001890 RID: 6288 RVA: 0x000934D0 File Offset: 0x000916D0
		public int ReconnectInterval
		{
			get
			{
				return this.reconnectInterval;
			}
			set
			{
				this.reconnectInterval = value;
			}
		}

		// Token: 0x17000750 RID: 1872
		// (get) Token: 0x06001891 RID: 6289 RVA: 0x000934DC File Offset: 0x000916DC
		public Socket Client
		{
			get
			{
				bool flag = this.tcpClient != null;
				Socket result;
				if (flag)
				{
					result = this.tcpClient.Client;
				}
				else
				{
					result = null;
				}
				return result;
			}
		}

		// Token: 0x1400007C RID: 124
		// (add) Token: 0x06001892 RID: 6290 RVA: 0x0009350C File Offset: 0x0009170C
		// (remove) Token: 0x06001893 RID: 6291 RVA: 0x00093544 File Offset: 0x00091744
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action Connected;

		// Token: 0x1400007D RID: 125
		// (add) Token: 0x06001894 RID: 6292 RVA: 0x0009357C File Offset: 0x0009177C
		// (remove) Token: 0x06001895 RID: 6293 RVA: 0x000935B4 File Offset: 0x000917B4
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action Disconnected;

		// Token: 0x06001896 RID: 6294 RVA: 0x000935EC File Offset: 0x000917EC
		public Network(string address, int port, string subClassName)
		{
			this.serverIP = address;
			this.port = port;
			this.subClassName = subClassName;
		}

		// Token: 0x06001897 RID: 6295 RVA: 0x00093644 File Offset: 0x00091844
		public void StartConnect()
		{
			bool flag = !this.connectThreadStarted;
			if (flag)
			{
				this.stopTryConnecting = false;
				new Thread(new ThreadStart(this.Connect))
				{
					Name = "ConnectThread " + this.subClassName
				}.Start();
			}
		}

		// Token: 0x06001898 RID: 6296 RVA: 0x00093698 File Offset: 0x00091898
		private void Connect()
		{
			this.syncConnectThread.WaitOne();
			this.connectThreadStarted = true;
			while (!this.ConnectionEstablished && !this.stopTryConnecting)
			{
				try
				{
					using (Ping ping = new Ping())
					{
						PingReply pingReply = ping.Send(this.serverIP, 1000);
						bool flag = pingReply.Status == IPStatus.Success;
						if (flag)
						{
							this.tcpClient = new TcpClient(this.serverIP, this.port);
							this.tcpClient.ReceiveTimeout = this.recvTimeout;
							this.tcpClient.SendBufferSize = int.MaxValue;
							this.tcpClient.NoDelay = true;
							this.ConnectionEstablished = true;
							this.OnConnected();
						}
					}
				}
				catch
				{
					this.ConnectionEstablished = false;
					Thread.Sleep(this.reconnectInterval);
					bool flag2 = this.firstConnect;
					if (flag2)
					{
						this.firstConnect = false;
						bool flag3 = !this.stopTryConnecting;
						if (flag3)
						{
							this.OnDisconnected();
						}
					}
				}
			}
			this.connectThreadStarted = false;
			this.syncConnectThread.ReleaseMutex();
		}

		// Token: 0x06001899 RID: 6297 RVA: 0x000937E0 File Offset: 0x000919E0
		public void SetReceiveTimeout(int milliseconds)
		{
			this.recvTimeout = milliseconds;
		}

		// Token: 0x0600189A RID: 6298 RVA: 0x000937EC File Offset: 0x000919EC
		public byte[] ReceiveBytes(int offset, int size)
		{
			byte[] array = null;
			bool connectionEstablished = this.ConnectionEstablished;
			if (connectionEstablished)
			{
				try
				{
					bool flag = size > 0;
					if (flag)
					{
						array = new byte[size + offset];
						this.ReceiveBytes(offset, size, array);
					}
					return array;
				}
				catch (SocketException ex)
				{
					Debug.WriteLine(ex.Message);
					this.ConnectionEstablished = false;
					bool flag2 = !this.stopTryConnecting;
					if (flag2)
					{
						this.OnDisconnected();
						this.StartConnect();
					}
					return null;
				}
				catch (ObjectDisposedException)
				{
					return null;
				}
			}
			return null;
		}

		// Token: 0x0600189B RID: 6299 RVA: 0x0009388C File Offset: 0x00091A8C
		private void ReceiveBytes(int offset, int size, byte[] buffer)
		{
			bool connectionEstablished = this.ConnectionEstablished;
			if (connectionEstablished)
			{
				int num = this.tcpClient.Client.Receive(buffer, offset, size, SocketFlags.None);
				bool flag = num != 0;
				if (flag)
				{
					bool flag2 = num != size;
					if (flag2)
					{
						this.ReceiveBytes(num + offset, size - num, buffer);
					}
				}
			}
		}

		// Token: 0x0600189C RID: 6300 RVA: 0x000938E0 File Offset: 0x00091AE0
		public void CloseConnection()
		{
			this.stopTryConnecting = true;
			this.ConnectionEstablished = false;
			bool flag = this.tcpClient != null && this.tcpClient.Client != null;
			if (flag)
			{
				this.tcpClient.Client.Close();
			}
		}

		// Token: 0x0600189D RID: 6301 RVA: 0x0009392C File Offset: 0x00091B2C
		protected void SendBytes(byte[] buffer, SocketFlags flags)
		{
			bool connectionEstablished = this.ConnectionEstablished;
			if (connectionEstablished)
			{
				try
				{
					this.tcpClient.Client.Send(buffer, flags);
				}
				catch (SocketException)
				{
					this.ConnectionEstablished = false;
					bool flag = !this.stopTryConnecting;
					if (flag)
					{
						this.OnDisconnected();
						this.StartConnect();
					}
				}
			}
		}

		// Token: 0x0600189E RID: 6302 RVA: 0x00093998 File Offset: 0x00091B98
		protected void SendBytes(byte[] buffer, int length, SocketFlags flags)
		{
			bool connectionEstablished = this.ConnectionEstablished;
			if (connectionEstablished)
			{
				try
				{
					this.tcpClient.Client.Send(buffer, length, flags);
				}
				catch (SocketException)
				{
					this.ConnectionEstablished = false;
					bool flag = !this.stopTryConnecting;
					if (flag)
					{
						this.OnDisconnected();
						this.StartConnect();
					}
				}
			}
		}

		// Token: 0x0600189F RID: 6303 RVA: 0x00093A04 File Offset: 0x00091C04
		protected virtual void OnConnected()
		{
			Action connected = this.Connected;
			bool flag = connected != null;
			if (flag)
			{
				connected();
			}
		}

		// Token: 0x060018A0 RID: 6304 RVA: 0x00093A28 File Offset: 0x00091C28
		protected virtual void OnDisconnected()
		{
			Action disconnected = this.Disconnected;
			bool flag = disconnected != null;
			if (flag)
			{
				disconnected();
			}
		}

		// Token: 0x04001009 RID: 4105
		private Mutex syncConnectThread = new Mutex();

		// Token: 0x0400100A RID: 4106
		private int recvTimeout = 0;

		// Token: 0x0400100B RID: 4107
		private bool connectThreadStarted = false;

		// Token: 0x0400100C RID: 4108
		private bool firstConnect = true;

		// Token: 0x0400100D RID: 4109
		private bool stopTryConnecting;

		// Token: 0x0400100E RID: 4110
		private int reconnectInterval = 1000;

		// Token: 0x0400100F RID: 4111
		private string subClassName;

		// Token: 0x04001010 RID: 4112
		protected TcpClient tcpClient;

		// Token: 0x04001011 RID: 4113
		protected string serverIP;

		// Token: 0x04001012 RID: 4114
		protected int port;
	}
}
