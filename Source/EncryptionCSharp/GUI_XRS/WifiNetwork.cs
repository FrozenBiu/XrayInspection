using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using NativeWifi;

namespace GUI_XRS
{
	// Token: 0x02000170 RID: 368
	public class WifiNetwork : INotifyPropertyChanged
	{
		// Token: 0x170009CC RID: 2508
		// (get) Token: 0x060021E8 RID: 8680 RVA: 0x000DACC4 File Offset: 0x000D8EC4
		public string Name
		{
			get
			{
				return Encoding.ASCII.GetString(this.networkNative.dot11Ssid.SSID, 0, (int)this.networkNative.dot11Ssid.SSIDLength);
			}
		}

		// Token: 0x170009CD RID: 2509
		// (get) Token: 0x060021E9 RID: 8681 RVA: 0x000DAD04 File Offset: 0x000D8F04
		public string Type
		{
			get
			{
				return this.networkNative.dot11BssType.ToString();
			}
		}

		// Token: 0x170009CE RID: 2510
		// (get) Token: 0x060021EA RID: 8682 RVA: 0x000DAD2C File Offset: 0x000D8F2C
		public string StrAuthAlgorithm
		{
			get
			{
				bool flag = this.networkNative.dot11DefaultAuthAlgorithm == Wlan.Dot11AuthAlgorithm.RSNA;
				string result;
				if (flag)
				{
					result = "WPA2";
				}
				else
				{
					bool flag2 = this.networkNative.dot11DefaultAuthAlgorithm == Wlan.Dot11AuthAlgorithm.WPA_PSK;
					if (flag2)
					{
						result = "WPA-PSK";
					}
					else
					{
						bool flag3 = this.networkNative.dot11DefaultAuthAlgorithm == Wlan.Dot11AuthAlgorithm.RSNA_PSK;
						if (flag3)
						{
							result = "WPA2-PSK";
						}
						else
						{
							bool flag4 = this.networkNative.dot11DefaultAuthAlgorithm == Wlan.Dot11AuthAlgorithm.IEEE80211_Open;
							if (flag4)
							{
								bool flag5 = this.networkNative.dot11DefaultCipherAlgorithm == Wlan.Dot11CipherAlgorithm.None;
								if (flag5)
								{
									result = "open";
								}
								else
								{
									result = this.networkNative.dot11DefaultCipherAlgorithm.ToString();
								}
							}
							else
							{
								result = this.networkNative.dot11DefaultAuthAlgorithm.ToString();
							}
						}
					}
				}
				return result;
			}
		}

		// Token: 0x170009CF RID: 2511
		// (get) Token: 0x060021EB RID: 8683 RVA: 0x000DADF0 File Offset: 0x000D8FF0
		public string StrCipherAlgorithm
		{
			get
			{
				bool flag = this.networkNative.dot11DefaultCipherAlgorithm == Wlan.Dot11CipherAlgorithm.CCMP;
				string result;
				if (flag)
				{
					result = "AES";
				}
				else
				{
					bool flag2 = this.networkNative.dot11DefaultCipherAlgorithm == Wlan.Dot11CipherAlgorithm.None;
					if (flag2)
					{
						result = "none";
					}
					else
					{
						result = this.networkNative.dot11DefaultCipherAlgorithm.ToString();
					}
				}
				return result;
			}
		}

		// Token: 0x170009D0 RID: 2512
		// (get) Token: 0x060021EC RID: 8684 RVA: 0x000DAE4C File Offset: 0x000D904C
		public int SignalQuality
		{
			get
			{
				return (int)this.networkNative.wlanSignalQuality;
			}
		}

		// Token: 0x170009D1 RID: 2513
		// (get) Token: 0x060021ED RID: 8685 RVA: 0x000DAE6C File Offset: 0x000D906C
		// (set) Token: 0x060021EE RID: 8686 RVA: 0x000DAE84 File Offset: 0x000D9084
		public Wlan.WlanAvailableNetwork NetworkNative
		{
			get
			{
				return this.networkNative;
			}
			set
			{
				this.networkNative = value;
				this.OnPropertyChanged("NetworkNative");
				this.OnPropertyChanged("Name");
				this.OnPropertyChanged("Type");
				this.OnPropertyChanged("StrAuthAlgorithm");
				this.OnPropertyChanged("StrCipherAlgorithm");
				this.OnPropertyChanged("SignalQuality");
			}
		}

		// Token: 0x170009D2 RID: 2514
		// (get) Token: 0x060021EF RID: 8687 RVA: 0x000DAEE4 File Offset: 0x000D90E4
		// (set) Token: 0x060021F0 RID: 8688 RVA: 0x000DAEFC File Offset: 0x000D90FC
		public bool IsConnected
		{
			get
			{
				return this.isConnected;
			}
			set
			{
				bool flag = this.isConnected != value;
				if (flag)
				{
					this.isConnected = value;
					this.OnPropertyChanged("IsConnected");
				}
			}
		}

		// Token: 0x140000B2 RID: 178
		// (add) Token: 0x060021F1 RID: 8689 RVA: 0x000DAF30 File Offset: 0x000D9130
		// (remove) Token: 0x060021F2 RID: 8690 RVA: 0x000DAF68 File Offset: 0x000D9168
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x060021F3 RID: 8691 RVA: 0x000DAF9D File Offset: 0x000D919D
		public WifiNetwork(Wlan.WlanAvailableNetwork networkNative)
		{
			this.networkNative = networkNative;
		}

		// Token: 0x060021F4 RID: 8692 RVA: 0x000DAFB0 File Offset: 0x000D91B0
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x060021F5 RID: 8693 RVA: 0x000DAFE0 File Offset: 0x000D91E0
		public string GetStrAuthAlgorithmForProfile()
		{
			bool flag = this.networkNative.dot11DefaultAuthAlgorithm == Wlan.Dot11AuthAlgorithm.RSNA;
			string result;
			if (flag)
			{
				result = "WPA2";
			}
			else
			{
				bool flag2 = this.networkNative.dot11DefaultAuthAlgorithm == Wlan.Dot11AuthAlgorithm.RSNA_PSK;
				if (flag2)
				{
					result = "WPA2PSK";
				}
				else
				{
					bool flag3 = this.networkNative.dot11DefaultAuthAlgorithm == Wlan.Dot11AuthAlgorithm.IEEE80211_Open || this.networkNative.dot11DefaultAuthAlgorithm == Wlan.Dot11AuthAlgorithm.IEEE80211_SharedKey;
					if (flag3)
					{
						result = "open";
					}
					else
					{
						result = this.networkNative.dot11DefaultAuthAlgorithm.ToString();
					}
				}
			}
			return result;
		}

		// Token: 0x060021F6 RID: 8694 RVA: 0x000DB068 File Offset: 0x000D9268
		public static string GetStrAuthAlgorithmForProfile(WifiAuthentication authentication)
		{
			string result = "";
			bool flag = authentication == WifiAuthentication.Open || authentication == WifiAuthentication.WEP;
			if (flag)
			{
				result = "open";
			}
			else
			{
				bool flag2 = authentication == WifiAuthentication.WPA2_PSK;
				if (flag2)
				{
					result = "WPA2PSK";
				}
				else
				{
					bool flag3 = authentication == WifiAuthentication.WPA_PSK;
					if (flag3)
					{
						result = "WPAPSK";
					}
				}
			}
			return result;
		}

		// Token: 0x060021F7 RID: 8695 RVA: 0x000DB0B8 File Offset: 0x000D92B8
		public static string GetStrEncryptionForProfile(WifiEncryption encryption)
		{
			string result = "";
			bool flag = encryption == WifiEncryption.None;
			if (flag)
			{
				result = "none";
			}
			else
			{
				bool flag2 = encryption == WifiEncryption.AES;
				if (flag2)
				{
					result = "AES";
				}
				else
				{
					bool flag3 = encryption == WifiEncryption.TKIP;
					if (flag3)
					{
						result = "TKIP";
					}
					else
					{
						bool flag4 = encryption == WifiEncryption.WEP;
						if (flag4)
						{
							result = "WEP";
						}
					}
				}
			}
			return result;
		}

		// Token: 0x040015FE RID: 5630
		private Wlan.WlanAvailableNetwork networkNative;

		// Token: 0x040015FF RID: 5631
		private bool isConnected;
	}
}
