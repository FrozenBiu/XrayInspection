using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using Common;
using DatabaseConnection;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x020000F2 RID: 242
	public class VideojetPrinterViewModel : INotifyPropertyChanged
	{
		// Token: 0x1700071A RID: 1818
		// (get) Token: 0x060017A1 RID: 6049 RVA: 0x0008F279 File Offset: 0x0008D479
		// (set) Token: 0x060017A2 RID: 6050 RVA: 0x0008F281 File Offset: 0x0008D481
		public RelayCommand SetIPAddressCmd { get; set; }

		// Token: 0x1700071B RID: 1819
		// (get) Token: 0x060017A3 RID: 6051 RVA: 0x0008F28A File Offset: 0x0008D48A
		// (set) Token: 0x060017A4 RID: 6052 RVA: 0x0008F292 File Offset: 0x0008D492
		public RelayCommand SetPortCmd { get; set; }

		// Token: 0x1700071C RID: 1820
		// (get) Token: 0x060017A5 RID: 6053 RVA: 0x0008F29B File Offset: 0x0008D49B
		// (set) Token: 0x060017A6 RID: 6054 RVA: 0x0008F2A3 File Offset: 0x0008D4A3
		public RelayCommand SetTimeoutCmd { get; set; }

		// Token: 0x14000076 RID: 118
		// (add) Token: 0x060017A7 RID: 6055 RVA: 0x0008F2AC File Offset: 0x0008D4AC
		// (remove) Token: 0x060017A8 RID: 6056 RVA: 0x0008F2E4 File Offset: 0x0008D4E4
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x1700071D RID: 1821
		// (get) Token: 0x060017A9 RID: 6057 RVA: 0x0008F31C File Offset: 0x0008D51C
		// (set) Token: 0x060017AA RID: 6058 RVA: 0x0008F334 File Offset: 0x0008D534
		public bool AdminMode
		{
			get
			{
				return this.adminMode;
			}
			set
			{
				this.adminMode = value;
				this.OnPropertyChanged("AdminMode");
			}
		}

		// Token: 0x1700071E RID: 1822
		// (get) Token: 0x060017AB RID: 6059 RVA: 0x0008F34C File Offset: 0x0008D54C
		// (set) Token: 0x060017AC RID: 6060 RVA: 0x0008F364 File Offset: 0x0008D564
		public bool OP1Mode
		{
			get
			{
				return this.op1Mode;
			}
			set
			{
				this.op1Mode = value;
				this.OnPropertyChanged("OP1Mode");
			}
		}

		// Token: 0x1700071F RID: 1823
		// (get) Token: 0x060017AD RID: 6061 RVA: 0x0008F37C File Offset: 0x0008D57C
		// (set) Token: 0x060017AE RID: 6062 RVA: 0x0008F399 File Offset: 0x0008D599
		public bool Enabled
		{
			get
			{
				return this.Printer.Enabled;
			}
			set
			{
				this.Printer.Enabled = value;
				this.dbConnection.SetVideojetPrinterEnabled(value);
				this.OnPropertyChanged("Enabled");
			}
		}

		// Token: 0x17000720 RID: 1824
		// (get) Token: 0x060017AF RID: 6063 RVA: 0x0008F3C4 File Offset: 0x0008D5C4
		// (set) Token: 0x060017B0 RID: 6064 RVA: 0x0008F400 File Offset: 0x0008D600
		public IPAddress IPAddress
		{
			get
			{
				IPAddress ipaddress = null;
				IPAddress.TryParse(this.Printer.PrinterIP, out ipaddress);
				bool flag = ipaddress == null;
				if (flag)
				{
					ipaddress = new IPAddress(new byte[4]);
				}
				return ipaddress;
			}
			set
			{
				this.Printer.PrinterIP = value.ToString();
				this.dbConnection.SetVideojetPrinterIP(this.Printer.PrinterIP);
				this.OnPropertyChanged("IPAddress");
			}
		}

		// Token: 0x17000721 RID: 1825
		// (get) Token: 0x060017B1 RID: 6065 RVA: 0x0008F438 File Offset: 0x0008D638
		// (set) Token: 0x060017B2 RID: 6066 RVA: 0x0008F455 File Offset: 0x0008D655
		public int Port
		{
			get
			{
				return this.Printer.Port;
			}
			set
			{
				this.Printer.Port = value;
				this.dbConnection.SetVideojetPrinterPort(value);
				this.OnPropertyChanged("Port");
			}
		}

		// Token: 0x17000722 RID: 1826
		// (get) Token: 0x060017B3 RID: 6067 RVA: 0x0008F480 File Offset: 0x0008D680
		// (set) Token: 0x060017B4 RID: 6068 RVA: 0x0008F49D File Offset: 0x0008D69D
		public int Timeout
		{
			get
			{
				return this.Printer.Timeout;
			}
			set
			{
				this.Printer.Timeout = value;
				this.dbConnection.SetVideojetPrinterTimeout(value);
				this.OnPropertyChanged("Timeout");
			}
		}

		// Token: 0x17000723 RID: 1827
		// (get) Token: 0x060017B5 RID: 6069 RVA: 0x0008F4C8 File Offset: 0x0008D6C8
		// (set) Token: 0x060017B6 RID: 6070 RVA: 0x0008F4E5 File Offset: 0x0008D6E5
		public bool StopConveyorOnError
		{
			get
			{
				return this.Printer.StopConveyorOnError;
			}
			set
			{
				this.Printer.StopConveyorOnError = value;
				this.dbConnection.SetVideojetPrinterStopOnError(value);
				this.OnPropertyChanged("StopConveyorOnError");
			}
		}

		// Token: 0x17000724 RID: 1828
		// (get) Token: 0x060017B7 RID: 6071 RVA: 0x0008F510 File Offset: 0x0008D710
		public string SentTimestamps
		{
			get
			{
				string text = "";
				for (int i = this.Printer.SentTimestamps.Count - 1; i >= 0; i--)
				{
					text = text + this.Printer.SentTimestamps[i] + "\n";
				}
				return text;
			}
		}

		// Token: 0x060017B8 RID: 6072 RVA: 0x0008F56C File Offset: 0x0008D76C
		public VideojetPrinterViewModel(VideojetPrinter printer, DBConnection dbConnection, AccessRightsManager accessManager)
		{
			this.Printer = printer;
			printer.TimestampIsSent += this.Printer_TimestampIsSent;
			this.dbConnection = dbConnection;
			this.AdminMode = (accessManager.CurrentUser.Rights >= BuiltInPriviledges.Admin);
			this.OP1Mode = (accessManager.CurrentUser.Rights >= BuiltInPriviledges.Operator1);
			this.SetIPAddressCmd = new RelayCommand(new Action<object>(this.SetIPAddress));
			this.SetPortCmd = new RelayCommand(new Action<object>(this.SetPort));
			this.SetTimeoutCmd = new RelayCommand(new Action<object>(this.SetTimeout));
		}

		// Token: 0x060017B9 RID: 6073 RVA: 0x0008F624 File Offset: 0x0008D824
		public void SetIPAddress(object obj)
		{
			this.numKeyboard = new NumKeyboard("IP Adresse", this.IPAddress.ToString(), NumKeyboardType.TypeIPAddress);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_SetIPAddress;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x060017BA RID: 6074 RVA: 0x0008F674 File Offset: 0x0008D874
		public void SetPort(object obj)
		{
			this.numKeyboard = new NumKeyboard("Port", this.Port.ToString(), NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_SetPort;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x060017BB RID: 6075 RVA: 0x0008F6C8 File Offset: 0x0008D8C8
		public void SetTimeout(object obj)
		{
			this.numKeyboard = new NumKeyboard("Timeout", this.Timeout.ToString(), NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_SetTimeout;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x060017BC RID: 6076 RVA: 0x0008F719 File Offset: 0x0008D919
		public void Cleanup()
		{
			this.Printer.TimestampIsSent -= this.Printer_TimestampIsSent;
		}

		// Token: 0x060017BD RID: 6077 RVA: 0x0008F734 File Offset: 0x0008D934
		private void numKeyboard_SetIPAddress(object sender, KeyboardInputEventArgs e)
		{
			IPAddress ipaddress;
			bool flag = IPAddress.TryParse(e.Text, out ipaddress);
			if (flag)
			{
				this.IPAddress = ipaddress;
			}
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_SetIPAddress;
		}

		// Token: 0x060017BE RID: 6078 RVA: 0x0008F774 File Offset: 0x0008D974
		private void numKeyboard_SetPort(object sender, KeyboardInputEventArgs e)
		{
			int port = 0;
			bool flag = int.TryParse(e.Text, out port);
			if (flag)
			{
				this.Port = port;
			}
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_SetIPAddress;
		}

		// Token: 0x060017BF RID: 6079 RVA: 0x0008F7B8 File Offset: 0x0008D9B8
		private void numKeyboard_SetTimeout(object sender, KeyboardInputEventArgs e)
		{
			int timeout = 0;
			bool flag = int.TryParse(e.Text, out timeout);
			if (flag)
			{
				this.Timeout = timeout;
			}
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_SetTimeout;
		}

		// Token: 0x060017C0 RID: 6080 RVA: 0x0008F7FC File Offset: 0x0008D9FC
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x060017C1 RID: 6081 RVA: 0x0008F829 File Offset: 0x0008DA29
		private void Printer_TimestampIsSent(object sender, EventArgs e)
		{
			this.OnPropertyChanged("SentTimestamps");
		}

		// Token: 0x04000F84 RID: 3972
		private DBConnection dbConnection;

		// Token: 0x04000F85 RID: 3973
		private NumKeyboard numKeyboard;

		// Token: 0x04000F86 RID: 3974
		private bool adminMode;

		// Token: 0x04000F87 RID: 3975
		private bool op1Mode;

		// Token: 0x04000F88 RID: 3976
		public VideojetPrinter Printer;
	}
}
