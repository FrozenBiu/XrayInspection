using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using Common;

namespace GUI_XRS
{
	// Token: 0x020000FE RID: 254
	public class NIC : INotifyPropertyChanged
	{
		// Token: 0x1700073F RID: 1855
		// (get) Token: 0x0600184A RID: 6218 RVA: 0x00092250 File Offset: 0x00090450
		// (set) Token: 0x0600184B RID: 6219 RVA: 0x00092268 File Offset: 0x00090468
		public bool ConfigurationChanged
		{
			get
			{
				return this.configurationChanged;
			}
			set
			{
				this.configurationChanged = value;
				this.OnPropertyChanged("ConfigurationChanged");
			}
		}

		// Token: 0x17000740 RID: 1856
		// (get) Token: 0x0600184C RID: 6220 RVA: 0x0009227E File Offset: 0x0009047E
		// (set) Token: 0x0600184D RID: 6221 RVA: 0x00092286 File Offset: 0x00090486
		public string Guid { get; set; }

		// Token: 0x17000741 RID: 1857
		// (get) Token: 0x0600184E RID: 6222 RVA: 0x0009228F File Offset: 0x0009048F
		// (set) Token: 0x0600184F RID: 6223 RVA: 0x00092297 File Offset: 0x00090497
		public string Caption { get; set; }

		// Token: 0x17000742 RID: 1858
		// (get) Token: 0x06001850 RID: 6224 RVA: 0x000922A0 File Offset: 0x000904A0
		// (set) Token: 0x06001851 RID: 6225 RVA: 0x000922B8 File Offset: 0x000904B8
		public IPAddress IPAddress
		{
			get
			{
				return this.ipAddress;
			}
			set
			{
				this.ipAddress = value;
				this.OnPropertyChanged("IPAddress");
			}
		}

		// Token: 0x17000743 RID: 1859
		// (get) Token: 0x06001852 RID: 6226 RVA: 0x000922D0 File Offset: 0x000904D0
		// (set) Token: 0x06001853 RID: 6227 RVA: 0x000922E8 File Offset: 0x000904E8
		public IPAddress SubnetMask
		{
			get
			{
				return this.subnetMask;
			}
			set
			{
				this.subnetMask = value;
				this.OnPropertyChanged("SubnetMask");
			}
		}

		// Token: 0x17000744 RID: 1860
		// (get) Token: 0x06001854 RID: 6228 RVA: 0x00092300 File Offset: 0x00090500
		// (set) Token: 0x06001855 RID: 6229 RVA: 0x00092318 File Offset: 0x00090518
		public IPAddress DefaultGateway
		{
			get
			{
				return this.defaultGateway;
			}
			set
			{
				this.defaultGateway = value;
				this.OnPropertyChanged("DefaultGateway");
			}
		}

		// Token: 0x17000745 RID: 1861
		// (get) Token: 0x06001856 RID: 6230 RVA: 0x00092330 File Offset: 0x00090530
		// (set) Token: 0x06001857 RID: 6231 RVA: 0x00092348 File Offset: 0x00090548
		public IPAddress PreferredDNS
		{
			get
			{
				return this.preferredDNS;
			}
			set
			{
				this.preferredDNS = value;
				this.OnPropertyChanged("PreferredDNS");
			}
		}

		// Token: 0x17000746 RID: 1862
		// (get) Token: 0x06001858 RID: 6232 RVA: 0x00092360 File Offset: 0x00090560
		// (set) Token: 0x06001859 RID: 6233 RVA: 0x00092378 File Offset: 0x00090578
		public IPAddress AlternateDNS
		{
			get
			{
				return this.alternateDNS;
			}
			set
			{
				this.alternateDNS = value;
				this.OnPropertyChanged("AlternateDNS");
			}
		}

		// Token: 0x17000747 RID: 1863
		// (get) Token: 0x0600185A RID: 6234 RVA: 0x00092390 File Offset: 0x00090590
		public bool IPStatic
		{
			get
			{
				return !this.DHCPEnabled;
			}
		}

		// Token: 0x17000748 RID: 1864
		// (get) Token: 0x0600185B RID: 6235 RVA: 0x000923AC File Offset: 0x000905AC
		// (set) Token: 0x0600185C RID: 6236 RVA: 0x000923C4 File Offset: 0x000905C4
		public bool DHCPEnabled
		{
			get
			{
				return this.dHCPEnabled;
			}
			set
			{
				this.dHCPEnabled = value;
				this.OnPropertyChanged("DHCPEnabled");
				this.OnPropertyChanged("IPStatic");
				this.OnPropertyChanged("DNSAutomatic");
				this.OnPropertyChanged("DNSAutomaticReal");
				this.OnPropertyChanged("DNSStatic");
			}
		}

		// Token: 0x17000749 RID: 1865
		// (get) Token: 0x0600185D RID: 6237 RVA: 0x00092418 File Offset: 0x00090618
		// (set) Token: 0x0600185E RID: 6238 RVA: 0x00092430 File Offset: 0x00090630
		public bool DNSAutomatic
		{
			get
			{
				return this.dNSAutomatic;
			}
			set
			{
				this.dNSAutomatic = value;
				this.OnPropertyChanged("DNSAutomatic");
				this.OnPropertyChanged("DNSAutomaticReal");
				this.OnPropertyChanged("DNSStatic");
			}
		}

		// Token: 0x1700074A RID: 1866
		// (get) Token: 0x0600185F RID: 6239 RVA: 0x00092460 File Offset: 0x00090660
		public bool DNSAutomaticReal
		{
			get
			{
				return this.DNSAutomatic && this.DHCPEnabled;
			}
		}

		// Token: 0x1700074B RID: 1867
		// (get) Token: 0x06001860 RID: 6240 RVA: 0x00092484 File Offset: 0x00090684
		public bool DNSStatic
		{
			get
			{
				return !this.DNSAutomaticReal;
			}
		}

		// Token: 0x14000079 RID: 121
		// (add) Token: 0x06001861 RID: 6241 RVA: 0x000924A0 File Offset: 0x000906A0
		// (remove) Token: 0x06001862 RID: 6242 RVA: 0x000924D8 File Offset: 0x000906D8
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x1700074C RID: 1868
		// (get) Token: 0x06001863 RID: 6243 RVA: 0x0009250D File Offset: 0x0009070D
		// (set) Token: 0x06001864 RID: 6244 RVA: 0x00092515 File Offset: 0x00090715
		public RelayCommand SetDHCPConfigCommand { get; set; }

		// Token: 0x1700074D RID: 1869
		// (get) Token: 0x06001865 RID: 6245 RVA: 0x0009251E File Offset: 0x0009071E
		// (set) Token: 0x06001866 RID: 6246 RVA: 0x00092526 File Offset: 0x00090726
		public RelayCommand SetDNSConfigCommand { get; set; }

		// Token: 0x06001867 RID: 6247 RVA: 0x0009252F File Offset: 0x0009072F
		public NIC() : this(null, null)
		{
		}

		// Token: 0x06001868 RID: 6248 RVA: 0x0009253C File Offset: 0x0009073C
		public NIC(string guid, string caption)
		{
			this.Guid = guid;
			this.Caption = caption;
			this.SetDHCPConfigCommand = new RelayCommand(new Action<object>(this.SetDHCPConfig));
			this.SetDNSConfigCommand = new RelayCommand(new Action<object>(this.SetDNSConfig));
		}

		// Token: 0x06001869 RID: 6249 RVA: 0x00092594 File Offset: 0x00090794
		private void SetDHCPConfig(object obj)
		{
			bool flag2;
			bool flag = bool.TryParse((string)obj, out flag2);
			if (flag)
			{
				bool flag3 = this.DHCPEnabled != flag2;
				if (flag3)
				{
					this.DHCPEnabled = flag2;
					this.ConfigurationChanged = true;
				}
			}
		}

		// Token: 0x0600186A RID: 6250 RVA: 0x000925D8 File Offset: 0x000907D8
		private void SetDNSConfig(object obj)
		{
			bool flag2;
			bool flag = bool.TryParse((string)obj, out flag2);
			if (flag)
			{
				bool flag3 = this.DNSAutomatic != flag2;
				if (flag3)
				{
					bool dnsautomaticReal = this.DNSAutomaticReal;
					this.DNSAutomatic = flag2;
					bool flag4 = dnsautomaticReal != this.DNSAutomaticReal;
					if (flag4)
					{
						this.OnPropertyChanged("DNSAutomaticReal");
						this.ConfigurationChanged = true;
					}
				}
			}
		}

		// Token: 0x0600186B RID: 6251 RVA: 0x00092644 File Offset: 0x00090844
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x0600186C RID: 6252 RVA: 0x00092674 File Offset: 0x00090874
		public bool SettingsValid()
		{
			return this.dHCPEnabled || (this.ipAddress != null && this.subnetMask != null);
		}

		// Token: 0x0600186D RID: 6253 RVA: 0x000926A8 File Offset: 0x000908A8
		public static string GetFirstMAC()
		{
			string text = (from nic in NetworkInterface.GetAllNetworkInterfaces()
			where nic.OperationalStatus == OperationalStatus.Up && nic.NetworkInterfaceType != NetworkInterfaceType.Loopback
			select nic.GetPhysicalAddress().ToString()).FirstOrDefault<string>();
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = text != null;
			if (flag)
			{
				for (int i = 0; i < text.Length; i++)
				{
					bool flag2 = i != 0 && i % 2 == 0;
					if (flag2)
					{
						stringBuilder.Append(':');
					}
					stringBuilder.Append(text[i]);
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04000FCF RID: 4047
		private bool dHCPEnabled;

		// Token: 0x04000FD0 RID: 4048
		private bool dNSAutomatic;

		// Token: 0x04000FD1 RID: 4049
		private IPAddress ipAddress;

		// Token: 0x04000FD2 RID: 4050
		private IPAddress subnetMask;

		// Token: 0x04000FD3 RID: 4051
		private IPAddress defaultGateway;

		// Token: 0x04000FD4 RID: 4052
		private IPAddress preferredDNS;

		// Token: 0x04000FD5 RID: 4053
		private IPAddress alternateDNS;

		// Token: 0x04000FD6 RID: 4054
		private bool configurationChanged;
	}
}
