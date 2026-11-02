using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;
using Common;
using DatabaseConnection;
using NativeWifi;

namespace GUI_XRS
{
	// Token: 0x0200016E RID: 366
	public class WifiViewModel : INotifyPropertyChanged
	{
		// Token: 0x140000B1 RID: 177
		// (add) Token: 0x060021C2 RID: 8642 RVA: 0x000D9F84 File Offset: 0x000D8184
		// (remove) Token: 0x060021C3 RID: 8643 RVA: 0x000D9FBC File Offset: 0x000D81BC
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x170009C3 RID: 2499
		// (get) Token: 0x060021C4 RID: 8644 RVA: 0x000D9FF1 File Offset: 0x000D81F1
		// (set) Token: 0x060021C5 RID: 8645 RVA: 0x000D9FF9 File Offset: 0x000D81F9
		public ObservableCollection<WifiNetwork> AvailableNetworks { get; set; }

		// Token: 0x170009C4 RID: 2500
		// (get) Token: 0x060021C6 RID: 8646 RVA: 0x000DA004 File Offset: 0x000D8204
		// (set) Token: 0x060021C7 RID: 8647 RVA: 0x000DA01C File Offset: 0x000D821C
		public WifiNetwork SelectedNetwork
		{
			get
			{
				return this.selectedNetwork;
			}
			set
			{
				bool flag = this.selectedNetwork != value;
				if (flag)
				{
					this.selectedNetwork = value;
					this.OnPropertyChanged("SelectedNetwork");
				}
			}
		}

		// Token: 0x170009C5 RID: 2501
		// (get) Token: 0x060021C8 RID: 8648 RVA: 0x000DA050 File Offset: 0x000D8250
		// (set) Token: 0x060021C9 RID: 8649 RVA: 0x000DA068 File Offset: 0x000D8268
		public WifiNetwork ConnectedNetwork
		{
			get
			{
				return this.connectedNetwork;
			}
			set
			{
				bool flag = this.connectedNetwork != value;
				if (flag)
				{
					this.connectedNetwork = value;
					this.OnPropertyChanged("ConnectedNetwork");
				}
			}
		}

		// Token: 0x170009C6 RID: 2502
		// (get) Token: 0x060021CA RID: 8650 RVA: 0x000DA09C File Offset: 0x000D829C
		public int NAvailableNetworks
		{
			get
			{
				return this.AvailableNetworks.Count;
			}
		}

		// Token: 0x170009C7 RID: 2503
		// (get) Token: 0x060021CB RID: 8651 RVA: 0x000DA0B9 File Offset: 0x000D82B9
		// (set) Token: 0x060021CC RID: 8652 RVA: 0x000DA0C1 File Offset: 0x000D82C1
		public ICommand ConnectCommand { get; set; }

		// Token: 0x170009C8 RID: 2504
		// (get) Token: 0x060021CD RID: 8653 RVA: 0x000DA0CA File Offset: 0x000D82CA
		// (set) Token: 0x060021CE RID: 8654 RVA: 0x000DA0D2 File Offset: 0x000D82D2
		public ICommand DisconnectCommand { get; set; }

		// Token: 0x170009C9 RID: 2505
		// (get) Token: 0x060021CF RID: 8655 RVA: 0x000DA0DB File Offset: 0x000D82DB
		// (set) Token: 0x060021D0 RID: 8656 RVA: 0x000DA0E3 File Offset: 0x000D82E3
		public ICommand ConnectOrDisconnectCommand { get; set; }

		// Token: 0x170009CA RID: 2506
		// (get) Token: 0x060021D1 RID: 8657 RVA: 0x000DA0EC File Offset: 0x000D82EC
		// (set) Token: 0x060021D2 RID: 8658 RVA: 0x000DA0F4 File Offset: 0x000D82F4
		public ICommand AddNetworkCommand { get; set; }

		// Token: 0x170009CB RID: 2507
		// (get) Token: 0x060021D3 RID: 8659 RVA: 0x000DA0FD File Offset: 0x000D82FD
		// (set) Token: 0x060021D4 RID: 8660 RVA: 0x000DA105 File Offset: 0x000D8305
		public BoundLocalizedText StrAvailableWlanConnections { get; set; }

		// Token: 0x060021D5 RID: 8661 RVA: 0x000DA110 File Offset: 0x000D8310
		public WifiViewModel(WifiClient wifiClient, DBConnection dbConnection)
		{
			this.wifiClient = wifiClient;
			this.dbConnection = dbConnection;
			this.ConnectCommand = new RelayCommand(new Action<object>(this.Connect));
			this.DisconnectCommand = new RelayCommand(new Action<object>(this.Disconnect));
			this.ConnectOrDisconnectCommand = new RelayCommand(new Action<object>(this.ConnectOrDisconnect));
			this.AddNetworkCommand = new RelayCommand(new Action<object>(this.AddNetwork));
			this.RegisterLocalizedText();
			this.Initialize();
			this.timerUpdateNetworks = new DispatcherTimer(new TimeSpan(0, 0, 0, 2, 0), DispatcherPriority.Background, new EventHandler(this.UpdateNetworks), Application.Current.Dispatcher);
			this.timerUpdateNetworks.Start();
		}

		// Token: 0x060021D6 RID: 8662 RVA: 0x000DA1D9 File Offset: 0x000D83D9
		private void RegisterLocalizedText()
		{
			this.StrAvailableWlanConnections = new BoundLocalizedText("txtAvailableWlanConnections");
		}

		// Token: 0x060021D7 RID: 8663 RVA: 0x000DA1ED File Offset: 0x000D83ED
		private void DeregisterLocalizedText()
		{
			MainWindow.localizationManager.Deregister(this.StrAvailableWlanConnections);
		}

		// Token: 0x060021D8 RID: 8664 RVA: 0x000DA204 File Offset: 0x000D8404
		private void Initialize()
		{
			this.AvailableNetworks = new ObservableCollection<WifiNetwork>();
			bool flag = this.wifiClient.WlanClient.Interfaces.Length != 0;
			if (flag)
			{
				this.wifiClient.WlanClient.Interfaces[0].WlanConnectionNotification += this.WifiViewModel_WlanConnectionNotification;
			}
			this.UpdateNetworks(null, new EventArgs());
			bool flag2 = this.AvailableNetworks.Count > 0;
			if (flag2)
			{
				this.SelectedNetwork = ((this.connectedNetwork != null) ? this.connectedNetwork : this.AvailableNetworks[0]);
			}
			this.view = (ListCollectionView)CollectionViewSource.GetDefaultView(this.AvailableNetworks);
			this.view.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
		}

		// Token: 0x060021D9 RID: 8665 RVA: 0x000DA2D0 File Offset: 0x000D84D0
		private void WifiViewModel_WlanConnectionNotification(Wlan.WlanNotificationData notifyData, Wlan.WlanConnectionNotificationData connNotifyData)
		{
			bool flag = notifyData.NotificationCode is Wlan.WlanNotificationCodeMsm && (Wlan.WlanNotificationCodeMsm)notifyData.NotificationCode == Wlan.WlanNotificationCodeMsm.Connected;
			if (flag)
			{
				Progress.CloseStaticProgress(false);
			}
			else
			{
				bool flag2 = notifyData.NotificationCode is Wlan.WlanNotificationCodeAcm && (Wlan.WlanNotificationCodeAcm)notifyData.NotificationCode == Wlan.WlanNotificationCodeAcm.ConnectionAttemptFail;
				if (flag2)
				{
					Progress.CloseStaticProgress(false);
					Dialog.ShowInvoked(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtConnectionAttemptFailed"), DialogStyle.Error, DialogButtons.OK);
				}
			}
		}

		// Token: 0x060021DA RID: 8666 RVA: 0x000DA358 File Offset: 0x000D8558
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x060021DB RID: 8667 RVA: 0x000DA388 File Offset: 0x000D8588
		private void UpdateNetworks(object sender, EventArgs e)
		{
			try
			{
				bool flag = this.wifiClient.WlanClient.Interfaces.Length != 0;
				if (flag)
				{
					WifiViewModel.<>c__DisplayClass48_0 CS$<>8__locals1 = new WifiViewModel.<>c__DisplayClass48_0();
					CS$<>8__locals1.<>4__this = this;
					WlanClient.WlanInterface wlanInterface = this.wifiClient.WlanClient.Interfaces[0];
					Wlan.WlanAvailableNetwork[] availableNetworkList = wlanInterface.GetAvailableNetworkList((Wlan.WlanGetAvailableNetworkFlags)0);
					CS$<>8__locals1.wlanNetwoksTemp = new List<WifiNetwork>();
					for (int j = 0; j < availableNetworkList.Length; j++)
					{
						bool flag2 = (availableNetworkList[j].flags & Wlan.WlanAvailableNetworkFlags.HasProfile) != Wlan.WlanAvailableNetworkFlags.HasProfile && availableNetworkList[j].dot11BssType != Wlan.Dot11BssType.Independent && availableNetworkList[j].dot11Ssid.SSID[0] > 0;
						if (flag2)
						{
							CS$<>8__locals1.wlanNetwoksTemp.Add(new WifiNetwork(availableNetworkList[j]));
						}
					}
					int i2;
					int k;
					for (k = 0; k < this.AvailableNetworks.Count; k = i2 + 1)
					{
						bool flag3 = (from wlanNetwork in CS$<>8__locals1.wlanNetwoksTemp
						where wlanNetwork.Name == CS$<>8__locals1.<>4__this.AvailableNetworks[k].Name
						select wlanNetwork).Count<WifiNetwork>() == 0;
						if (flag3)
						{
							this.AvailableNetworks.RemoveAt(k);
						}
						i2 = k;
					}
					int i;
					for (i = 0; i < CS$<>8__locals1.wlanNetwoksTemp.Count; i = i2 + 1)
					{
						List<WifiNetwork> list = (from wlanNetwork in this.AvailableNetworks
						where wlanNetwork.Name == CS$<>8__locals1.wlanNetwoksTemp[i].Name
						select wlanNetwork).ToList<WifiNetwork>();
						bool flag4 = list.Count <= 0;
						if (flag4)
						{
							this.AvailableNetworks.Add(CS$<>8__locals1.wlanNetwoksTemp[i]);
						}
						else
						{
							list[0].NetworkNative = CS$<>8__locals1.wlanNetwoksTemp[i].NetworkNative;
						}
						i2 = i;
					}
				}
				else
				{
					this.AvailableNetworks.Clear();
				}
				this.SetConnectedNetwork();
			}
			catch
			{
			}
		}

		// Token: 0x060021DC RID: 8668 RVA: 0x000DA5F8 File Offset: 0x000D87F8
		private void SetConnectedNetwork()
		{
			try
			{
				bool flag = this.wifiClient.WlanClient.Interfaces.Length != 0 && this.wifiClient.WlanClient.Interfaces[0].InterfaceState == Wlan.WlanInterfaceState.Connected;
				if (flag)
				{
					for (int i = 0; i < this.AvailableNetworks.Count; i++)
					{
						this.AvailableNetworks[i].IsConnected = (this.AvailableNetworks[i].Name == this.wifiClient.WlanClient.Interfaces[0].CurrentConnection.profileName);
						bool isConnected = this.AvailableNetworks[i].IsConnected;
						if (isConnected)
						{
							this.ConnectedNetwork = this.AvailableNetworks[i];
						}
					}
				}
				else
				{
					this.ConnectedNetwork = null;
					for (int j = 0; j < this.AvailableNetworks.Count; j++)
					{
						this.AvailableNetworks[j].IsConnected = false;
					}
				}
			}
			catch
			{
			}
			this.OnPropertyChanged("ConnectedNetwork");
		}

		// Token: 0x060021DD RID: 8669 RVA: 0x000DA740 File Offset: 0x000D8940
		public void Connect(object obj)
		{
			try
			{
				bool flag = this.SelectedNetwork != null && this.wifiClient.WlanClient.Interfaces.Length != 0;
				if (flag)
				{
					bool flag2 = this.SelectedNetwork.NetworkNative.dot11DefaultCipherAlgorithm > Wlan.Dot11CipherAlgorithm.None;
					if (flag2)
					{
						this.ShowSecurityKeyKeyboard();
					}
					else
					{
						this.Connect(this.SelectedNetwork.Name, null);
					}
				}
			}
			catch
			{
				Dialog.ShowInvoked(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtConnectionAttemptFailed"), DialogStyle.Error, DialogButtons.OK);
			}
		}

		// Token: 0x060021DE RID: 8670 RVA: 0x000DA7E0 File Offset: 0x000D89E0
		private void Connect(string ssid, string key)
		{
			try
			{
				bool flag = this.wifiClient.WlanClient.Interfaces.Length != 0;
				if (flag)
				{
					string strAuthAlgorithmForProfile = this.SelectedNetwork.GetStrAuthAlgorithmForProfile();
					string strCipherAlgorithm = this.SelectedNetwork.StrCipherAlgorithm;
					string xmlProfile = this.wifiClient.CreateXmlProfile(ssid, strAuthAlgorithmForProfile, strCipherAlgorithm, key, false);
					Progress.ShowStaticProgress(LocalizationManager.Localize("txtConnecting") + "...", false);
					bool flag2 = key != null;
					if (flag2)
					{
						SimpleAES simpleAES = new SimpleAES();
						key = simpleAES.Encrypt(key);
					}
					bool flag3 = this.connectedNetwork != null;
					if (flag3)
					{
						this.wifiClient.Disconnect();
					}
					this.wifiClient.Connect(xmlProfile);
					this.wifiClient.SetDatabaseValues(ssid, strAuthAlgorithmForProfile, strCipherAlgorithm, key, false);
				}
			}
			catch
			{
				Progress.CloseStaticProgress(false);
				Dialog.ShowInvoked(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtConnectionAttemptFailed"), DialogStyle.Error, DialogButtons.OK);
			}
		}

		// Token: 0x060021DF RID: 8671 RVA: 0x000DA8E8 File Offset: 0x000D8AE8
		public void Disconnect(object obj)
		{
			try
			{
				bool flag = this.wifiClient.WlanClient.Interfaces.Length != 0;
				if (flag)
				{
					this.wifiClient.Disconnect();
					this.wifiClient.SetDatabaseValues("", "", "", null, false);
				}
			}
			catch
			{
			}
		}

		// Token: 0x060021E0 RID: 8672 RVA: 0x000DA954 File Offset: 0x000D8B54
		private void ConnectOrDisconnect(object obj)
		{
			try
			{
				bool flag = this.wifiClient.WlanClient.Interfaces.Length != 0;
				if (flag)
				{
					bool flag2 = this.NAvailableNetworks == 0;
					if (flag2)
					{
						Dialog.Show(LocalizationManager.Localize("txtError"), "No Wifi connections available.", DialogStyle.Error, DialogButtons.OK);
					}
					else
					{
						bool flag3 = this.wifiClient.WlanClient.Interfaces[0].InterfaceState == Wlan.WlanInterfaceState.Disconnected;
						if (flag3)
						{
							this.Connect(null);
						}
						else
						{
							this.Disconnect(null);
						}
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x060021E1 RID: 8673 RVA: 0x000DA9F4 File Offset: 0x000D8BF4
		public void CleanUp()
		{
			try
			{
				this.timerUpdateNetworks.Stop();
				this.DeregisterLocalizedText();
				bool flag = this.wifiClient.WlanClient.Interfaces.Length != 0;
				if (flag)
				{
					this.wifiClient.WlanClient.Interfaces[0].WlanConnectionNotification -= this.WifiViewModel_WlanConnectionNotification;
				}
			}
			catch
			{
			}
		}

		// Token: 0x060021E2 RID: 8674 RVA: 0x000DAA6C File Offset: 0x000D8C6C
		private void AddNetwork(object obj)
		{
			CtrlWifiAddNetwork ctrlWifiAddNetwork = new CtrlWifiAddNetwork(this.wifiClient);
			Subwindow.Show(ctrlWifiAddNetwork, SubwindowButtons.BackAndHome, ctrlWifiAddNetwork.Buttons);
		}

		// Token: 0x060021E3 RID: 8675 RVA: 0x000DAA94 File Offset: 0x000D8C94
		private void ShowSecurityKeyKeyboard()
		{
			this.keyboard = new Keyboard(LocalizationManager.Localize("txtSecurityKey"), "", KeyboardType.Password);
			this.keyboard.SetKeyboardUppercase(false);
			this.keyboard.TextInputKeyboard += this.keyboard_SecurityKeyEntered;
			this.keyboard.Show();
		}

		// Token: 0x060021E4 RID: 8676 RVA: 0x000DAAF0 File Offset: 0x000D8CF0
		private void keyboard_SecurityKeyEntered(object sender, KeyboardInputEventArgs e)
		{
			this.keyboard.TextInputKeyboard -= this.keyboard_SecurityKeyEntered;
			bool flag = e.Text.Length > 0;
			if (flag)
			{
				this.Connect(this.SelectedNetwork.Name, e.Text);
			}
		}

		// Token: 0x040015E8 RID: 5608
		private WifiClient wifiClient;

		// Token: 0x040015E9 RID: 5609
		private WifiNetwork selectedNetwork;

		// Token: 0x040015EA RID: 5610
		private DispatcherTimer timerUpdateNetworks;

		// Token: 0x040015EB RID: 5611
		private WifiNetwork connectedNetwork;

		// Token: 0x040015EC RID: 5612
		private Keyboard keyboard;

		// Token: 0x040015ED RID: 5613
		private DBConnection dbConnection;

		// Token: 0x040015EE RID: 5614
		private ListCollectionView view;
	}
}
