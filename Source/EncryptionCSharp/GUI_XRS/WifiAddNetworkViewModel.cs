using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using Common;

namespace GUI_XRS
{
	// Token: 0x02000165 RID: 357
	public class WifiAddNetworkViewModel : INotifyPropertyChanged
	{
		// Token: 0x17000997 RID: 2455
		// (get) Token: 0x0600213B RID: 8507 RVA: 0x000D8227 File Offset: 0x000D6427
		// (set) Token: 0x0600213C RID: 8508 RVA: 0x000D822F File Offset: 0x000D642F
		public ObservableCollection<WifiEncryption> AvailableEncryptions { get; set; }

		// Token: 0x140000AE RID: 174
		// (add) Token: 0x0600213D RID: 8509 RVA: 0x000D8238 File Offset: 0x000D6438
		// (remove) Token: 0x0600213E RID: 8510 RVA: 0x000D8270 File Offset: 0x000D6470
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x17000998 RID: 2456
		// (get) Token: 0x0600213F RID: 8511 RVA: 0x000D82A8 File Offset: 0x000D64A8
		// (set) Token: 0x06002140 RID: 8512 RVA: 0x000D82C0 File Offset: 0x000D64C0
		public string SSID
		{
			get
			{
				return this.sSID;
			}
			set
			{
				this.sSID = value;
				this.OnPropertyChanged("SSID");
			}
		}

		// Token: 0x17000999 RID: 2457
		// (get) Token: 0x06002141 RID: 8513 RVA: 0x000D82D8 File Offset: 0x000D64D8
		// (set) Token: 0x06002142 RID: 8514 RVA: 0x000D82F0 File Offset: 0x000D64F0
		public WifiAuthentication WifiAuthentication
		{
			get
			{
				return this.wifiAuthentication;
			}
			set
			{
				this.wifiAuthentication = value;
				this.CreateEncryptionList();
				this.OnPropertyChanged("WifiAuthentication");
			}
		}

		// Token: 0x1700099A RID: 2458
		// (get) Token: 0x06002143 RID: 8515 RVA: 0x000D8310 File Offset: 0x000D6510
		// (set) Token: 0x06002144 RID: 8516 RVA: 0x000D8328 File Offset: 0x000D6528
		public WifiEncryption WifiEncryption
		{
			get
			{
				return this.wifiEncryption;
			}
			set
			{
				this.wifiEncryption = value;
				this.OnPropertyChanged("WifiEncryption");
			}
		}

		// Token: 0x1700099B RID: 2459
		// (get) Token: 0x06002145 RID: 8517 RVA: 0x000D8340 File Offset: 0x000D6540
		// (set) Token: 0x06002146 RID: 8518 RVA: 0x000D8358 File Offset: 0x000D6558
		public string SecurityKey
		{
			get
			{
				return this.securityKey;
			}
			set
			{
				this.securityKey = value;
				this.OnPropertyChanged("SecurityKey");
			}
		}

		// Token: 0x1700099C RID: 2460
		// (get) Token: 0x06002147 RID: 8519 RVA: 0x000D8370 File Offset: 0x000D6570
		// (set) Token: 0x06002148 RID: 8520 RVA: 0x000D8388 File Offset: 0x000D6588
		public bool SecurityKeyNeeded
		{
			get
			{
				return this.securityKeyNeeded;
			}
			set
			{
				this.securityKeyNeeded = value;
				this.OnPropertyChanged("SecurityKeyNeeded");
			}
		}

		// Token: 0x1700099D RID: 2461
		// (get) Token: 0x06002149 RID: 8521 RVA: 0x000D839E File Offset: 0x000D659E
		// (set) Token: 0x0600214A RID: 8522 RVA: 0x000D83A6 File Offset: 0x000D65A6
		public BoundLocalizedText StrSSID { get; set; }

		// Token: 0x1700099E RID: 2462
		// (get) Token: 0x0600214B RID: 8523 RVA: 0x000D83AF File Offset: 0x000D65AF
		// (set) Token: 0x0600214C RID: 8524 RVA: 0x000D83B7 File Offset: 0x000D65B7
		public BoundLocalizedText StrSecurityKey { get; set; }

		// Token: 0x1700099F RID: 2463
		// (get) Token: 0x0600214D RID: 8525 RVA: 0x000D83C0 File Offset: 0x000D65C0
		// (set) Token: 0x0600214E RID: 8526 RVA: 0x000D83C8 File Offset: 0x000D65C8
		public BoundLocalizedText StrSecurityType { get; set; }

		// Token: 0x170009A0 RID: 2464
		// (get) Token: 0x0600214F RID: 8527 RVA: 0x000D83D1 File Offset: 0x000D65D1
		// (set) Token: 0x06002150 RID: 8528 RVA: 0x000D83D9 File Offset: 0x000D65D9
		public BoundLocalizedText StrEncryptionType { get; set; }

		// Token: 0x170009A1 RID: 2465
		// (get) Token: 0x06002151 RID: 8529 RVA: 0x000D83E2 File Offset: 0x000D65E2
		// (set) Token: 0x06002152 RID: 8530 RVA: 0x000D83EA File Offset: 0x000D65EA
		public RelayCommand SetSSIDCommand { get; set; }

		// Token: 0x170009A2 RID: 2466
		// (get) Token: 0x06002153 RID: 8531 RVA: 0x000D83F3 File Offset: 0x000D65F3
		// (set) Token: 0x06002154 RID: 8532 RVA: 0x000D83FB File Offset: 0x000D65FB
		public RelayCommand SetSecurityKeyCommand { get; set; }

		// Token: 0x170009A3 RID: 2467
		// (get) Token: 0x06002155 RID: 8533 RVA: 0x000D8404 File Offset: 0x000D6604
		// (set) Token: 0x06002156 RID: 8534 RVA: 0x000D840C File Offset: 0x000D660C
		public RelayCommand CleanUpCommand { get; set; }

		// Token: 0x06002157 RID: 8535 RVA: 0x000D8418 File Offset: 0x000D6618
		public WifiAddNetworkViewModel(WifiClient wifiClient)
		{
			this.wifiClient = wifiClient;
			this.AvailableEncryptions = new ObservableCollection<WifiEncryption>();
			this.WifiAuthentication = WifiAuthentication.Open;
			this.RegisterLocalizedText();
			this.SetSSIDCommand = new RelayCommand(new Action<object>(this.ShowSSIDKeyboard));
			this.SetSecurityKeyCommand = new RelayCommand(new Action<object>(this.ShowSecurityKeyKeyboard));
			this.CleanUpCommand = new RelayCommand(new Action<object>(this.CleanUp));
		}

		// Token: 0x06002158 RID: 8536 RVA: 0x000D8498 File Offset: 0x000D6698
		private void RegisterLocalizedText()
		{
			this.StrSSID = new BoundLocalizedText("txtSSID");
			this.StrSecurityKey = new BoundLocalizedText("txtSecurityKey");
			this.StrSecurityType = new BoundLocalizedText("txtSecurityType");
			this.StrEncryptionType = new BoundLocalizedText("txtEncryptionType");
		}

		// Token: 0x06002159 RID: 8537 RVA: 0x000D84EC File Offset: 0x000D66EC
		private void DeregisterLocalizedText()
		{
			MainWindow.localizationManager.Deregister(this.StrSSID);
			MainWindow.localizationManager.Deregister(this.StrSecurityKey);
			MainWindow.localizationManager.Deregister(this.StrSecurityType);
			MainWindow.localizationManager.Deregister(this.StrEncryptionType);
		}

		// Token: 0x0600215A RID: 8538 RVA: 0x000D8540 File Offset: 0x000D6740
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x0600215B RID: 8539 RVA: 0x000D8570 File Offset: 0x000D6770
		private void CreateEncryptionList()
		{
			this.AvailableEncryptions.Clear();
			bool flag = this.wifiAuthentication == WifiAuthentication.Open;
			if (flag)
			{
				this.AvailableEncryptions.Add(WifiEncryption.None);
				this.securityKey = "";
			}
			else
			{
				bool flag2 = this.wifiAuthentication == WifiAuthentication.WEP;
				if (flag2)
				{
					this.AvailableEncryptions.Add(WifiEncryption.WEP);
				}
				else
				{
					bool flag3 = this.wifiAuthentication == WifiAuthentication.WPA2_PSK || this.wifiAuthentication == WifiAuthentication.WPA_PSK;
					if (flag3)
					{
						this.AvailableEncryptions.Add(WifiEncryption.AES);
						this.AvailableEncryptions.Add(WifiEncryption.TKIP);
					}
				}
			}
			bool flag4 = this.AvailableEncryptions.Count > 0;
			if (flag4)
			{
				this.WifiEncryption = this.AvailableEncryptions[0];
			}
			this.SecurityKeyNeeded = (this.wifiAuthentication > WifiAuthentication.Open);
		}

		// Token: 0x0600215C RID: 8540 RVA: 0x000D863C File Offset: 0x000D683C
		private void ShowSSIDKeyboard(object obj)
		{
			this.keyboard = new Keyboard(this.StrSSID.LocalizedText, this.sSID, KeyboardType.Application);
			this.keyboard.TextInputKeyboard += this.keyboard_SSIDEntered;
			this.keyboard.Show();
		}

		// Token: 0x0600215D RID: 8541 RVA: 0x000D868B File Offset: 0x000D688B
		private void keyboard_SSIDEntered(object sender, KeyboardInputEventArgs e)
		{
			this.keyboard.TextInputKeyboard -= this.keyboard_SSIDEntered;
			this.SSID = e.Text;
		}

		// Token: 0x0600215E RID: 8542 RVA: 0x000D86B4 File Offset: 0x000D68B4
		private void ShowSecurityKeyKeyboard(object obj)
		{
			this.keyboard = new Keyboard(this.StrSecurityKey.LocalizedText, this.securityKey, KeyboardType.Password);
			this.keyboard.SetKeyboardUppercase(false);
			this.keyboard.TextInputKeyboard += this.keyboard_SecurityKeyEntered;
			this.keyboard.Show();
		}

		// Token: 0x0600215F RID: 8543 RVA: 0x000D8710 File Offset: 0x000D6910
		private void keyboard_SecurityKeyEntered(object sender, KeyboardInputEventArgs e)
		{
			this.keyboard.TextInputKeyboard -= this.keyboard_SecurityKeyEntered;
			this.SecurityKey = e.Text;
		}

		// Token: 0x06002160 RID: 8544 RVA: 0x000D8738 File Offset: 0x000D6938
		public void CleanUp(object obj)
		{
			this.DeregisterLocalizedText();
		}

		// Token: 0x06002161 RID: 8545 RVA: 0x000D8744 File Offset: 0x000D6944
		public void CommitNewNetwork()
		{
			string text = (this.wifiAuthentication != WifiAuthentication.Open) ? this.securityKey : null;
			string strAuthAlgorithmForProfile = WifiNetwork.GetStrAuthAlgorithmForProfile(this.wifiAuthentication);
			string strEncryptionForProfile = WifiNetwork.GetStrEncryptionForProfile(this.wifiEncryption);
			string passPhrase = "";
			string xmlProfile = this.wifiClient.CreateXmlProfile(this.sSID, strAuthAlgorithmForProfile, strEncryptionForProfile, text, true);
			try
			{
				bool flag = text != null;
				if (flag)
				{
					SimpleAES simpleAES = new SimpleAES();
					passPhrase = simpleAES.Encrypt(text);
				}
				this.wifiClient.SetDatabaseValues(this.sSID, strAuthAlgorithmForProfile, strEncryptionForProfile, passPhrase, true);
				Progress.ShowStaticProgress(LocalizationManager.Localize("txtConnecting") + "...", false);
				this.wifiClient.Connect(xmlProfile);
			}
			catch
			{
				Progress.CloseStaticProgress(false);
				Dialog.ShowInvoked(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtConnectionAttemptFailed"), DialogStyle.Error, DialogButtons.OK);
			}
		}

		// Token: 0x04001590 RID: 5520
		private string securityKey;

		// Token: 0x04001591 RID: 5521
		private WifiEncryption wifiEncryption;

		// Token: 0x04001592 RID: 5522
		private WifiAuthentication wifiAuthentication;

		// Token: 0x04001593 RID: 5523
		private string sSID;

		// Token: 0x04001594 RID: 5524
		private Keyboard keyboard;

		// Token: 0x04001595 RID: 5525
		private WifiClient wifiClient;

		// Token: 0x04001596 RID: 5526
		private bool securityKeyNeeded;
	}
}
