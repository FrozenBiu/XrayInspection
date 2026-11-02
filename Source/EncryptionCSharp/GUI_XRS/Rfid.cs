using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO.Ports;
using System.Timers;
using DatabaseConnection;
using GUI_XRS.RS232;

namespace GUI_XRS
{
	// Token: 0x020000CD RID: 205
	internal class Rfid : INotifyPropertyChanged
	{
		// Token: 0x17000630 RID: 1584
		// (get) Token: 0x06001444 RID: 5188 RVA: 0x00077BE4 File Offset: 0x00075DE4
		public static Rfid Instance
		{
			get
			{
				bool flag = Rfid._instance == null;
				if (flag)
				{
					Rfid._instance = new Rfid();
				}
				return Rfid._instance;
			}
		}

		// Token: 0x06001445 RID: 5189 RVA: 0x00077C14 File Offset: 0x00075E14
		private Rfid()
		{
			this.IsConnected = false;
			this.HasRfid = false;
			this.Uid = "";
			this._timer = new Timer(30000.0);
			this._timer.Elapsed += this._resetUid;
			UsbNotification.UsbDeviceDetached += this._usbDeviceChanged;
			UsbNotification.UsbDeviceAttached += this._usbDeviceChanged;
		}

		// Token: 0x06001446 RID: 5190 RVA: 0x00077C98 File Offset: 0x00075E98
		private void _usbDeviceChanged()
		{
			bool flag = this._rs232 != null;
			if (flag)
			{
				this._rs232.Send("life");
				this.IsConnected = this._rs232.IsConnected;
				bool flag2 = !this.IsConnected;
				if (flag2)
				{
					this.Disconnect();
					this.Connect();
				}
			}
			else
			{
				this.Connect();
			}
		}

		// Token: 0x06001447 RID: 5191 RVA: 0x00077D00 File Offset: 0x00075F00
		private void _resetUid(object sender, ElapsedEventArgs e)
		{
			this._timer.Stop();
			this.Uid = "";
		}

		// Token: 0x06001448 RID: 5192 RVA: 0x00077D1C File Offset: 0x00075F1C
		private Config loadConfig()
		{
			bool flag = this._dbConnection == null;
			if (flag)
			{
				throw new Exception("No Database set.");
			}
			Config config = new Config();
			uint port = 1U;
			this._dbConnection.GetRFIDLoginComPort(ref port);
			config.Port = port;
			config.Baudrate = 115200;
			config.Databits = 8;
			config.Parity = Parity.None;
			config.StopBits = StopBits.One;
			return config;
		}

		// Token: 0x06001449 RID: 5193 RVA: 0x00077D8C File Offset: 0x00075F8C
		public void SetDbConnection(DBConnection dbConnection)
		{
			bool flag = this._dbConnection != null;
			if (flag)
			{
				throw new Exception("Database allready set.");
			}
			this._dbConnection = dbConnection;
		}

		// Token: 0x0600144A RID: 5194 RVA: 0x00077DB9 File Offset: 0x00075FB9
		public void StopTimer()
		{
			this._timer.Stop();
		}

		// Token: 0x0600144B RID: 5195 RVA: 0x00077DC8 File Offset: 0x00075FC8
		public void CleanUid()
		{
			this.Uid = "";
		}

		// Token: 0x0600144C RID: 5196 RVA: 0x00077DD8 File Offset: 0x00075FD8
		public bool Connect()
		{
			bool flag = this._dbConnection == null;
			if (flag)
			{
				throw new Exception("No Database set.");
			}
			bool flag2 = false;
			this._dbConnection.GetRFIDLoginAvailable(ref flag2);
			bool flag3 = flag2;
			bool result;
			if (flag3)
			{
				this._rs232 = new RS232();
				this.IsConnected = this._rs232.Connect(this.loadConfig(), 250, 250);
				bool isConnected = this.IsConnected;
				if (isConnected)
				{
					Debug.WriteLine("RS232 connected!");
					this._rs232.MessageReceived += this.rs232_MessageReceived;
				}
				else
				{
					Debug.WriteLine("RS232 connection failed!");
					this._rs232.Dispose();
					this._rs232 = null;
				}
				result = this.IsConnected;
			}
			else
			{
				this.IsConnected = flag2;
				result = flag2;
			}
			return result;
		}

		// Token: 0x0600144D RID: 5197 RVA: 0x00077EB0 File Offset: 0x000760B0
		public void Disconnect()
		{
			this.IsConnected = false;
			bool flag = this._rs232 != null;
			if (flag)
			{
				this._rs232.MessageReceived -= this.rs232_MessageReceived;
				this._rs232.Disconnect();
				this._rs232.Dispose();
				this._rs232 = null;
			}
		}

		// Token: 0x0600144E RID: 5198 RVA: 0x00077F0C File Offset: 0x0007610C
		private void rs232_MessageReceived(string data)
		{
			bool flag = data.Length > "PiccSelect: ".Length;
			if (flag)
			{
				string a = data.Substring(0, "PiccSelect: ".Length);
				bool flag2 = a == "PiccSelect: ";
				if (flag2)
				{
					this.Uid = data.Remove(data.Length - 2).Substring("PiccSelect: ".Length);
					Debug.WriteLine("Select: " + this.Uid);
					this.OnRfidDetected(this.Uid);
					bool flag3 = !this._timer.Enabled;
					if (flag3)
					{
						this._timer.Start();
					}
				}
				else
				{
					bool flag4 = a == "PiccRemove: ";
					if (flag4)
					{
						Debug.WriteLine("Remove: " + data.Remove(data.Length - 2).Substring("PiccRemove: ".Length));
					}
				}
			}
		}

		// Token: 0x14000067 RID: 103
		// (add) Token: 0x0600144F RID: 5199 RVA: 0x00078000 File Offset: 0x00076200
		// (remove) Token: 0x06001450 RID: 5200 RVA: 0x00078038 File Offset: 0x00076238
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event DelegateRfidDetected RfidDetected;

		// Token: 0x06001451 RID: 5201 RVA: 0x00078070 File Offset: 0x00076270
		protected void OnRfidDetected(string rfid)
		{
			bool flag = this.RfidDetected != null;
			if (flag)
			{
				this.RfidDetected(rfid);
			}
		}

		// Token: 0x14000068 RID: 104
		// (add) Token: 0x06001452 RID: 5202 RVA: 0x00078098 File Offset: 0x00076298
		// (remove) Token: 0x06001453 RID: 5203 RVA: 0x000780D0 File Offset: 0x000762D0
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x06001454 RID: 5204 RVA: 0x00078108 File Offset: 0x00076308
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x17000631 RID: 1585
		// (get) Token: 0x06001455 RID: 5205 RVA: 0x00078138 File Offset: 0x00076338
		// (set) Token: 0x06001456 RID: 5206 RVA: 0x00078150 File Offset: 0x00076350
		public bool IsConnected
		{
			get
			{
				return this._isConnected;
			}
			private set
			{
				bool flag = this._isConnected != value;
				if (flag)
				{
					this._isConnected = value;
					this.OnPropertyChanged("IsConnected");
				}
			}
		}

		// Token: 0x17000632 RID: 1586
		// (get) Token: 0x06001457 RID: 5207 RVA: 0x00078184 File Offset: 0x00076384
		// (set) Token: 0x06001458 RID: 5208 RVA: 0x0007819C File Offset: 0x0007639C
		public string Uid
		{
			get
			{
				return this._uid;
			}
			private set
			{
				bool flag = this._uid != value;
				if (flag)
				{
					this._uid = value;
					this.OnPropertyChanged("Uid");
					this.HasRfid = !string.IsNullOrEmpty(this._uid);
				}
			}
		}

		// Token: 0x17000633 RID: 1587
		// (get) Token: 0x06001459 RID: 5209 RVA: 0x000781E4 File Offset: 0x000763E4
		// (set) Token: 0x0600145A RID: 5210 RVA: 0x000781FC File Offset: 0x000763FC
		public bool HasRfid
		{
			get
			{
				return this._hasRfid;
			}
			private set
			{
				bool flag = this._hasRfid != value;
				if (flag)
				{
					this._hasRfid = value;
					this.OnPropertyChanged("HasRfid");
				}
			}
		}

		// Token: 0x04000D6E RID: 3438
		private RS232 _rs232;

		// Token: 0x04000D6F RID: 3439
		public Timer _timer;

		// Token: 0x04000D70 RID: 3440
		private bool _isConnected;

		// Token: 0x04000D71 RID: 3441
		private string _uid;

		// Token: 0x04000D72 RID: 3442
		private bool _hasRfid;

		// Token: 0x04000D73 RID: 3443
		private DBConnection _dbConnection;

		// Token: 0x04000D74 RID: 3444
		private static Rfid _instance;
	}
}
