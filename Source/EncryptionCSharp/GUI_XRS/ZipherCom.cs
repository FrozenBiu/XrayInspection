using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace GUI_XRS
{
	// Token: 0x020000F3 RID: 243
	public class ZipherCom
	{
		// Token: 0x17000725 RID: 1829
		// (get) Token: 0x060017C2 RID: 6082 RVA: 0x0008F838 File Offset: 0x0008DA38
		// (set) Token: 0x060017C3 RID: 6083 RVA: 0x0008F840 File Offset: 0x0008DA40
		public string ServerIP { get; set; }

		// Token: 0x17000726 RID: 1830
		// (get) Token: 0x060017C4 RID: 6084 RVA: 0x0008F849 File Offset: 0x0008DA49
		// (set) Token: 0x060017C5 RID: 6085 RVA: 0x0008F851 File Offset: 0x0008DA51
		public int Port { get; set; }

		// Token: 0x17000727 RID: 1831
		// (get) Token: 0x060017C6 RID: 6086 RVA: 0x0008F85A File Offset: 0x0008DA5A
		// (set) Token: 0x060017C7 RID: 6087 RVA: 0x0008F862 File Offset: 0x0008DA62
		public int Timeout { get; set; }

		// Token: 0x17000728 RID: 1832
		// (get) Token: 0x060017C8 RID: 6088 RVA: 0x0008F86C File Offset: 0x0008DA6C
		public bool Connected
		{
			get
			{
				bool flag = this.client == null;
				bool result;
				if (flag)
				{
					result = false;
				}
				else
				{
					bool flag2 = !this.client.Connected;
					if (flag2)
					{
						result = false;
					}
					else
					{
						bool flag3 = this.stream == null;
						result = !flag3;
					}
				}
				return result;
			}
		}

		// Token: 0x060017C9 RID: 6089 RVA: 0x0008F8B7 File Offset: 0x0008DAB7
		public ZipherCom(string serverIP, int port)
		{
			this.ServerIP = serverIP;
			this.Port = port;
			this.Timeout = 200;
			this.sendData = new List<byte>();
		}

		// Token: 0x060017CA RID: 6090 RVA: 0x0008F8F0 File Offset: 0x0008DAF0
		public bool Connect()
		{
			bool flag = this.client != null;
			if (flag)
			{
				bool connected = this.client.Connected;
				if (connected)
				{
					this.Disconnect();
				}
			}
			try
			{
				this.client = new TcpClient(this.ServerIP, this.Port);
				this.stream = this.client.GetStream();
				this.stream.ReadTimeout = this.Timeout;
				this.stream.WriteTimeout = this.Timeout;
				this.ClearQueue();
			}
			catch
			{
				this.Disconnect();
				return false;
			}
			return true;
		}

		// Token: 0x060017CB RID: 6091 RVA: 0x0008F9A0 File Offset: 0x0008DBA0
		public bool Disconnect()
		{
			bool flag = this.stream != null;
			if (flag)
			{
				this.stream.Close();
				this.stream = null;
			}
			bool flag2 = this.client != null;
			if (flag2)
			{
				this.client.Close();
				this.client = null;
			}
			return true;
		}

		// Token: 0x060017CC RID: 6092 RVA: 0x0008F9F8 File Offset: 0x0008DBF8
		public bool SendTimestamp(string timestamp)
		{
			byte[] array = new byte[256];
			bool flag = !this.Connected;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				this.sendData.Clear();
				this.sendData.AddRange(Encoding.ASCII.GetBytes("JDI|1|V1="));
				this.sendData.AddRange(Encoding.ASCII.GetBytes(timestamp));
				this.sendData.AddRange(Encoding.ASCII.GetBytes("|"));
				this.sendData.Add(13);
				try
				{
					this.stream.Write(this.sendData.ToArray(), 0, this.sendData.Count);
					int num = this.stream.Read(array, 0, array.Length);
					bool flag2 = array[num - 1] != 13;
					if (flag2)
					{
						return false;
					}
					string @string = Encoding.ASCII.GetString(array, 0, num - 1);
					bool flag3 = @string == "ERR";
					if (flag3)
					{
						return false;
					}
				}
				catch
				{
					return false;
				}
				result = true;
			}
			return result;
		}

		// Token: 0x060017CD RID: 6093 RVA: 0x0008FB28 File Offset: 0x0008DD28
		public bool ClearQueue()
		{
			byte[] array = new byte[256];
			bool flag = !this.Connected;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				this.sendData.Clear();
				this.sendData.AddRange(Encoding.ASCII.GetBytes("CQI"));
				this.sendData.Add(13);
				try
				{
					this.stream.Write(this.sendData.ToArray(), 0, this.sendData.Count);
					int num = this.stream.Read(array, 0, array.Length);
					bool flag2 = array[num - 1] != 13;
					if (flag2)
					{
						return false;
					}
					string @string = Encoding.ASCII.GetString(array, 0, num - 1);
					bool flag3 = @string == "ERR";
					if (flag3)
					{
						return false;
					}
				}
				catch
				{
					return false;
				}
				result = true;
			}
			return result;
		}

		// Token: 0x04000F8F RID: 3983
		public const int DefaultPort = 3007;

		// Token: 0x04000F91 RID: 3985
		private const byte commandTerminator = 13;

		// Token: 0x04000F92 RID: 3986
		private const string returnError = "ERR";

		// Token: 0x04000F93 RID: 3987
		private const string returnAck = "ACK";

		// Token: 0x04000F94 RID: 3988
		private TcpClient client = null;

		// Token: 0x04000F95 RID: 3989
		private NetworkStream stream;

		// Token: 0x04000F96 RID: 3990
		private List<byte> sendData;
	}
}
