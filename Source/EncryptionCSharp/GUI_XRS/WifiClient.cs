using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Threading;
using System.Xml.Linq;
using DatabaseConnection;
using NativeWifi;

namespace GUI_XRS
{
	// Token: 0x02000167 RID: 359
	public class WifiClient : INotifyPropertyChanged
	{
		// Token: 0x140000AF RID: 175
		// (add) Token: 0x06002162 RID: 8546 RVA: 0x000D8838 File Offset: 0x000D6A38
		// (remove) Token: 0x06002163 RID: 8547 RVA: 0x000D8870 File Offset: 0x000D6A70
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x170009A4 RID: 2468
		// (get) Token: 0x06002164 RID: 8548 RVA: 0x000D88A5 File Offset: 0x000D6AA5
		// (set) Token: 0x06002165 RID: 8549 RVA: 0x000D88AD File Offset: 0x000D6AAD
		public WlanClient WlanClient { get; set; }

		// Token: 0x170009A5 RID: 2469
		// (get) Token: 0x06002166 RID: 8550 RVA: 0x000D88B8 File Offset: 0x000D6AB8
		// (set) Token: 0x06002167 RID: 8551 RVA: 0x000D88D0 File Offset: 0x000D6AD0
		public bool WifiAvailable
		{
			get
			{
				return this.wifiAvailable;
			}
			set
			{
				bool flag = this.wifiAvailable != value;
				if (flag)
				{
					this.wifiAvailable = value;
					this.OnPropertyChanged("WifiAvailable");
				}
			}
		}

		// Token: 0x170009A6 RID: 2470
		// (get) Token: 0x06002168 RID: 8552 RVA: 0x000D8904 File Offset: 0x000D6B04
		// (set) Token: 0x06002169 RID: 8553 RVA: 0x000D891C File Offset: 0x000D6B1C
		public bool WifiConnected
		{
			get
			{
				return this.wifiConnected;
			}
			set
			{
				bool flag = this.wifiConnected != value;
				if (flag)
				{
					this.wifiConnected = value;
					this.OnPropertyChanged("WifiConnected");
					this.OnPropertyChanged("WifiNotConnected");
				}
			}
		}

		// Token: 0x170009A7 RID: 2471
		// (get) Token: 0x0600216A RID: 8554 RVA: 0x000D895C File Offset: 0x000D6B5C
		public bool WifiNotConnected
		{
			get
			{
				return !this.wifiConnected;
			}
		}

		// Token: 0x170009A8 RID: 2472
		// (get) Token: 0x0600216B RID: 8555 RVA: 0x000D8978 File Offset: 0x000D6B78
		// (set) Token: 0x0600216C RID: 8556 RVA: 0x000D8990 File Offset: 0x000D6B90
		public int WifiSignalQuality
		{
			get
			{
				return this.wifiSignalQuality;
			}
			set
			{
				bool flag = this.wifiSignalQuality != value;
				if (flag)
				{
					this.wifiSignalQuality = value;
					this.OnPropertyChanged("WifiSignalQuality");
				}
			}
		}

		// Token: 0x0600216D RID: 8557 RVA: 0x000D89C4 File Offset: 0x000D6BC4
		public WifiClient(DBConnection dbConnection)
		{
			this.dbConnection = dbConnection;
			try
			{
				this.WlanClient = new WlanClient();
				this.timerWlanIndicators = new DispatcherTimer(new TimeSpan(0, 0, 0, 2, 0), DispatcherPriority.Background, new EventHandler(this.UpdateIndicators), Application.Current.Dispatcher);
			}
			catch
			{
			}
		}

		// Token: 0x0600216E RID: 8558 RVA: 0x000D8A38 File Offset: 0x000D6C38
		public string CreateXmlProfile(string profileName, string authentication, string encryption, string key, bool hidden)
		{
			XNamespace ns = "http://www.microsoft.com/networking/WLAN/profile/v1";
			string localName = "WLANProfile";
			string content = (encryption != "WEP") ? "passPhrase" : "networkKey";
			string content2 = hidden ? "true" : "false";
			bool flag = key != null;
			XElement xelement;
			if (flag)
			{
				xelement = new XElement(ns + localName, new object[]
				{
					new XElement(ns + "name", profileName),
					new XElement(ns + "SSIDConfig", new object[]
					{
						new XElement(ns + "SSID", new XElement(ns + "name", profileName)),
						new XElement(ns + "nonBroadcast", content2)
					}),
					new XElement(ns + "connectionType", "ESS"),
					new XElement(ns + "connectionMode", "auto"),
					new XElement(ns + "autoSwitch", "false"),
					new XElement(ns + "MSM", new XElement(ns + "security", new object[]
					{
						new XElement(ns + "authEncryption", new object[]
						{
							new XElement(ns + "authentication", authentication),
							new XElement(ns + "encryption", encryption),
							new XElement(ns + "useOneX", "false")
						}),
						new XElement(ns + "sharedKey", new object[]
						{
							new XElement(ns + "keyType", content),
							new XElement(ns + "protected", "false"),
							new XElement(ns + "keyMaterial", key)
						})
					}))
				});
			}
			else
			{
				xelement = new XElement(ns + localName, new object[]
				{
					new XElement(ns + "name", profileName),
					new XElement(ns + "SSIDConfig", new XElement(ns + "SSID", new XElement(ns + "name", profileName))),
					new XElement(ns + "connectionType", "ESS"),
					new XElement(ns + "connectionMode", "auto"),
					new XElement(ns + "autoSwitch", "false"),
					new XElement(ns + "MSM", new XElement(ns + "security", new XElement(ns + "authEncryption", new object[]
					{
						new XElement(ns + "authentication", authentication),
						new XElement(ns + "encryption", encryption),
						new XElement(ns + "useOneX", "false")
					})))
				});
			}
			return xelement.ToString();
		}

		// Token: 0x0600216F RID: 8559 RVA: 0x000D8D60 File Offset: 0x000D6F60
		public void RemoveAllProfiles()
		{
			bool flag = this.WlanClient.Interfaces.Length != 0;
			if (flag)
			{
				WlanClient.WlanInterface wlanInterface = this.WlanClient.Interfaces[0];
				Wlan.WlanProfileInfo[] profiles = wlanInterface.GetProfiles();
				for (int i = 0; i < profiles.Length; i++)
				{
					wlanInterface.DeleteProfile(profiles[i].profileName);
				}
			}
		}

		// Token: 0x06002170 RID: 8560 RVA: 0x000D8DC4 File Offset: 0x000D6FC4
		public void Connect(string xmlProfile)
		{
			bool flag = this.WlanClient.Interfaces.Length != 0;
			if (flag)
			{
				bool flag2 = File.Exists(WifiClient.ewfDllFilePath);
				if (flag2)
				{
					this.WlanClient.Interfaces[0].SetProfile(Wlan.WlanProfileFlags.AllUser, xmlProfile, true);
				}
				this.WlanClient.Interfaces[0].Connect(Wlan.WlanConnectionMode.TemporaryProfile, Wlan.Dot11BssType.Any, xmlProfile);
			}
		}

		// Token: 0x06002171 RID: 8561 RVA: 0x000D8E24 File Offset: 0x000D7024
		public void Disconnect()
		{
			bool flag = this.WlanClient.Interfaces.Length != 0;
			if (flag)
			{
				bool flag2 = File.Exists(WifiClient.ewfDllFilePath);
				if (flag2)
				{
					this.RemoveAllProfiles();
				}
				this.WlanClient.Interfaces[0].Disconnect();
			}
		}

		// Token: 0x06002172 RID: 8562 RVA: 0x000D8E70 File Offset: 0x000D7070
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x06002173 RID: 8563 RVA: 0x000D8EA0 File Offset: 0x000D70A0
		private void UpdateIndicators(object sender, EventArgs e)
		{
			try
			{
				bool flag = this.WlanClient != null;
				if (flag)
				{
					this.WifiAvailable = (this.WlanClient.Interfaces.Length != 0);
					bool flag2 = this.wifiAvailable;
					if (flag2)
					{
						this.WifiConnected = (this.WlanClient.Interfaces[0].InterfaceState == Wlan.WlanInterfaceState.Connected);
						bool flag3 = this.wifiConnected;
						if (flag3)
						{
							Wlan.WlanAvailableNetwork[] availableNetworkList = this.WlanClient.Interfaces[0].GetAvailableNetworkList((Wlan.WlanGetAvailableNetworkFlags)0);
							for (int i = 0; i < availableNetworkList.Length; i++)
							{
								string @string = Encoding.ASCII.GetString(availableNetworkList[i].dot11Ssid.SSID, 0, (int)availableNetworkList[i].dot11Ssid.SSIDLength);
								bool flag4 = (availableNetworkList[i].flags & Wlan.WlanAvailableNetworkFlags.HasProfile) != Wlan.WlanAvailableNetworkFlags.HasProfile && @string == this.WlanClient.Interfaces[0].CurrentConnection.profileName;
								if (flag4)
								{
									this.WifiSignalQuality = (int)availableNetworkList[i].wlanSignalQuality;
									break;
								}
							}
						}
						else
						{
							this.WifiSignalQuality = 0;
						}
						bool flag5 = this.counterForNextScan >= 5;
						if (flag5)
						{
							this.counterForNextScan = 0;
							this.WlanClient.Interfaces[0].Scan();
						}
						this.counterForNextScan++;
					}
					else
					{
						this.WifiConnected = false;
						this.WifiSignalQuality = 0;
					}
				}
				else
				{
					this.WifiAvailable = false;
					this.WifiConnected = false;
					this.WifiSignalQuality = 0;
				}
			}
			catch
			{
			}
		}

		// Token: 0x06002174 RID: 8564 RVA: 0x000D9060 File Offset: 0x000D7260
		public void SetDatabaseValues(string profileName, string authentication, string encryption, string passPhrase, bool hidden)
		{
			this.dbConnection.SetWifiNetworkName(profileName);
			this.dbConnection.SetWifiAuthentication(authentication);
			this.dbConnection.SetWifiEncryption(encryption);
			bool flag = passPhrase != null;
			if (flag)
			{
				this.dbConnection.SetWifiKey(passPhrase);
			}
			else
			{
				this.dbConnection.SetWifiKey("");
			}
			this.dbConnection.SetWifiHidden(hidden);
		}

		// Token: 0x040015A5 RID: 5541
		public static readonly string ewfDllFilePath = "C:\\Windows\\System32\\ewfapi.dll";

		// Token: 0x040015A6 RID: 5542
		private const int limitForNextScan = 5;

		// Token: 0x040015A7 RID: 5543
		private DispatcherTimer timerWlanIndicators;

		// Token: 0x040015A8 RID: 5544
		private bool wifiAvailable;

		// Token: 0x040015A9 RID: 5545
		private bool wifiConnected;

		// Token: 0x040015AA RID: 5546
		private int wifiSignalQuality;

		// Token: 0x040015AB RID: 5547
		private DBConnection dbConnection;

		// Token: 0x040015AC RID: 5548
		private int counterForNextScan = 5;
	}
}
