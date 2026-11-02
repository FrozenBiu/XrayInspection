using System;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Text;
using System.Threading;

namespace GUI_XRS.RS232
{
	// Token: 0x02000262 RID: 610
	internal class RS232 : IDisposable
	{
		// Token: 0x17000C80 RID: 3200
		// (get) Token: 0x06002ED1 RID: 11985 RVA: 0x001330E3 File Offset: 0x001312E3
		// (set) Token: 0x06002ED2 RID: 11986 RVA: 0x001330EB File Offset: 0x001312EB
		public Config Configuration { get; private set; }

		// Token: 0x17000C81 RID: 3201
		// (get) Token: 0x06002ED3 RID: 11987 RVA: 0x001330F4 File Offset: 0x001312F4
		// (set) Token: 0x06002ED4 RID: 11988 RVA: 0x001330FC File Offset: 0x001312FC
		public SerialPort Connection { get; private set; }

		// Token: 0x17000C82 RID: 3202
		// (get) Token: 0x06002ED5 RID: 11989 RVA: 0x00133105 File Offset: 0x00131305
		// (set) Token: 0x06002ED6 RID: 11990 RVA: 0x0013310D File Offset: 0x0013130D
		public bool IsConnected { get; set; }

		// Token: 0x1400010B RID: 267
		// (add) Token: 0x06002ED7 RID: 11991 RVA: 0x00133118 File Offset: 0x00131318
		// (remove) Token: 0x06002ED8 RID: 11992 RVA: 0x00133150 File Offset: 0x00131350
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event MessageDeleage MessageReceived;

		// Token: 0x06002ED9 RID: 11993 RVA: 0x00133188 File Offset: 0x00131388
		public void OnMessageReceived(string query)
		{
			bool flag = this.MessageReceived != null;
			if (flag)
			{
				this.MessageReceived(query);
			}
		}

		// Token: 0x06002EDA RID: 11994 RVA: 0x001331B4 File Offset: 0x001313B4
		public bool Connect(Config configuration, int readTimeout = 250, int writeTimeout = 250)
		{
			this.Configuration = configuration;
			bool flag = this.Connection != null;
			if (flag)
			{
				this.Connection.Close();
			}
			try
			{
				this.Connection = new SerialPort();
				this.Connection.PortName = "COM" + this.Configuration.Port.ToString();
				this.Connection.BaudRate = this.Configuration.Baudrate;
				this.Connection.Parity = this.Configuration.Parity;
				this.Connection.DataBits = this.Configuration.Databits;
				this.Connection.StopBits = this.Configuration.StopBits;
				this.Connection.ReadTimeout = readTimeout;
				this.Connection.WriteTimeout = writeTimeout;
				this.Connection.DataReceived += this._ConnectionDataReceived;
				try
				{
					this.Connection.Open();
					this.IsConnected = this.Connection.IsOpen;
					return this.Connection.IsOpen;
				}
				catch (InvalidOperationException)
				{
				}
				catch (IOException)
				{
				}
				catch (UnauthorizedAccessException)
				{
				}
			}
			catch (ArgumentNullException)
			{
			}
			catch (ArgumentOutOfRangeException)
			{
			}
			catch
			{
			}
			return false;
		}

		// Token: 0x06002EDB RID: 11995 RVA: 0x00133394 File Offset: 0x00131594
		~RS232()
		{
			this.Dispose();
		}

		// Token: 0x06002EDC RID: 11996 RVA: 0x001333C4 File Offset: 0x001315C4
		public void Dispose()
		{
			bool flag = this.Connection != null;
			if (flag)
			{
				bool isOpen = this.Connection.IsOpen;
				if (isOpen)
				{
					this.Connection.Close();
				}
				this.Connection.Dispose();
			}
		}

		// Token: 0x06002EDD RID: 11997 RVA: 0x00133408 File Offset: 0x00131608
		void IDisposable.Dispose()
		{
			this.Dispose();
			GC.SuppressFinalize(this);
		}

		// Token: 0x06002EDE RID: 11998 RVA: 0x0013341C File Offset: 0x0013161C
		public void Disconnect()
		{
			try
			{
				bool flag = this.Connection != null && this.Connection.IsOpen;
				if (flag)
				{
					this.Connection.Close();
				}
			}
			catch
			{
			}
		}

		// Token: 0x06002EDF RID: 11999 RVA: 0x0013346C File Offset: 0x0013166C
		public void Send(string data)
		{
			bool flag = this.Connection != null;
			if (flag)
			{
				try
				{
					this.Connection.DiscardOutBuffer();
					this.Connection.DiscardInBuffer();
					this.Connection.Write(data);
				}
				catch (TimeoutException)
				{
					this.IsConnected = false;
				}
				catch (InvalidOperationException)
				{
					this.IsConnected = false;
				}
				catch (Exception value)
				{
					Debug.WriteLine(value);
					this.IsConnected = false;
				}
			}
		}

		// Token: 0x06002EE0 RID: 12000 RVA: 0x00133508 File Offset: 0x00131708
		private string _ReceiveData()
		{
			byte[] array = new byte[this.Connection.ReadBufferSize];
			string empty = string.Empty;
			int num = 0;
			try
			{
				do
				{
					int value = this.Connection.ReadByte();
					array[num] = Convert.ToByte(value);
					num++;
				}
				while (this.Connection.BytesToRead != 0);
			}
			catch (TimeoutException ex)
			{
				Debug.WriteLine(ex.ToString());
				this.IsConnected = false;
			}
			catch (IOException)
			{
				this.IsConnected = false;
			}
			catch (ThreadAbortException)
			{
				this.IsConnected = false;
			}
			catch
			{
				this.IsConnected = false;
			}
			return Encoding.ASCII.GetString(array, 0, num);
		}

		// Token: 0x06002EE1 RID: 12001 RVA: 0x001335EC File Offset: 0x001317EC
		private void _ConnectionDataReceived(object sender, SerialDataReceivedEventArgs e)
		{
			string text = string.Empty;
			text = this._ReceiveData();
			bool flag = !string.IsNullOrEmpty(text);
			if (flag)
			{
				this.IsConnected = true;
				this.OnMessageReceived(text);
			}
		}
	}
}
