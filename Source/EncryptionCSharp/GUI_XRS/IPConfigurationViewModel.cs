using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using Common;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x020000FB RID: 251
	public class IPConfigurationViewModel : INotifyPropertyChanged
	{
		// Token: 0x1700072F RID: 1839
		// (get) Token: 0x0600180E RID: 6158 RVA: 0x000913D6 File Offset: 0x0008F5D6
		// (set) Token: 0x0600180F RID: 6159 RVA: 0x000913DE File Offset: 0x0008F5DE
		public NIC NIC { get; set; }

		// Token: 0x17000730 RID: 1840
		// (get) Token: 0x06001810 RID: 6160 RVA: 0x000913E7 File Offset: 0x0008F5E7
		// (set) Token: 0x06001811 RID: 6161 RVA: 0x000913EF File Offset: 0x0008F5EF
		public RelayCommand SetIPAddressCommand { get; set; }

		// Token: 0x17000731 RID: 1841
		// (get) Token: 0x06001812 RID: 6162 RVA: 0x000913F8 File Offset: 0x0008F5F8
		// (set) Token: 0x06001813 RID: 6163 RVA: 0x00091400 File Offset: 0x0008F600
		public RelayCommand SetSubnetMaskCommand { get; set; }

		// Token: 0x17000732 RID: 1842
		// (get) Token: 0x06001814 RID: 6164 RVA: 0x00091409 File Offset: 0x0008F609
		// (set) Token: 0x06001815 RID: 6165 RVA: 0x00091411 File Offset: 0x0008F611
		public RelayCommand SetGatewayCommand { get; set; }

		// Token: 0x17000733 RID: 1843
		// (get) Token: 0x06001816 RID: 6166 RVA: 0x0009141A File Offset: 0x0008F61A
		// (set) Token: 0x06001817 RID: 6167 RVA: 0x00091422 File Offset: 0x0008F622
		public RelayCommand SetPreferredDNSCommand { get; set; }

		// Token: 0x17000734 RID: 1844
		// (get) Token: 0x06001818 RID: 6168 RVA: 0x0009142B File Offset: 0x0008F62B
		// (set) Token: 0x06001819 RID: 6169 RVA: 0x00091433 File Offset: 0x0008F633
		public RelayCommand SetAlternateDNSCommand { get; set; }

		// Token: 0x17000735 RID: 1845
		// (get) Token: 0x0600181A RID: 6170 RVA: 0x0009143C File Offset: 0x0008F63C
		// (set) Token: 0x0600181B RID: 6171 RVA: 0x00091444 File Offset: 0x0008F644
		public RelayCommand CleanUpCommand { get; set; }

		// Token: 0x14000077 RID: 119
		// (add) Token: 0x0600181C RID: 6172 RVA: 0x00091450 File Offset: 0x0008F650
		// (remove) Token: 0x0600181D RID: 6173 RVA: 0x00091488 File Offset: 0x0008F688
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x17000736 RID: 1846
		// (get) Token: 0x0600181E RID: 6174 RVA: 0x000914BD File Offset: 0x0008F6BD
		// (set) Token: 0x0600181F RID: 6175 RVA: 0x000914C5 File Offset: 0x0008F6C5
		public BoundLocalizedText StrIPAddress { get; set; }

		// Token: 0x17000737 RID: 1847
		// (get) Token: 0x06001820 RID: 6176 RVA: 0x000914CE File Offset: 0x0008F6CE
		// (set) Token: 0x06001821 RID: 6177 RVA: 0x000914D6 File Offset: 0x0008F6D6
		public BoundLocalizedText StrSubnetMask { get; set; }

		// Token: 0x17000738 RID: 1848
		// (get) Token: 0x06001822 RID: 6178 RVA: 0x000914DF File Offset: 0x0008F6DF
		// (set) Token: 0x06001823 RID: 6179 RVA: 0x000914E7 File Offset: 0x0008F6E7
		public BoundLocalizedText StrDefaultGateway { get; set; }

		// Token: 0x17000739 RID: 1849
		// (get) Token: 0x06001824 RID: 6180 RVA: 0x000914F0 File Offset: 0x0008F6F0
		// (set) Token: 0x06001825 RID: 6181 RVA: 0x000914F8 File Offset: 0x0008F6F8
		public BoundLocalizedText StrPreferredDNSServer { get; set; }

		// Token: 0x1700073A RID: 1850
		// (get) Token: 0x06001826 RID: 6182 RVA: 0x00091501 File Offset: 0x0008F701
		// (set) Token: 0x06001827 RID: 6183 RVA: 0x00091509 File Offset: 0x0008F709
		public BoundLocalizedText StrAlternateDNSServer { get; set; }

		// Token: 0x1700073B RID: 1851
		// (get) Token: 0x06001828 RID: 6184 RVA: 0x00091512 File Offset: 0x0008F712
		// (set) Token: 0x06001829 RID: 6185 RVA: 0x0009151A File Offset: 0x0008F71A
		public BoundLocalizedText StrObtainIPAutomatically { get; set; }

		// Token: 0x1700073C RID: 1852
		// (get) Token: 0x0600182A RID: 6186 RVA: 0x00091523 File Offset: 0x0008F723
		// (set) Token: 0x0600182B RID: 6187 RVA: 0x0009152B File Offset: 0x0008F72B
		public BoundLocalizedText StrUseFollowingIP { get; set; }

		// Token: 0x1700073D RID: 1853
		// (get) Token: 0x0600182C RID: 6188 RVA: 0x00091534 File Offset: 0x0008F734
		// (set) Token: 0x0600182D RID: 6189 RVA: 0x0009153C File Offset: 0x0008F73C
		public BoundLocalizedText StrObtainDNSAutomatically { get; set; }

		// Token: 0x1700073E RID: 1854
		// (get) Token: 0x0600182E RID: 6190 RVA: 0x00091545 File Offset: 0x0008F745
		// (set) Token: 0x0600182F RID: 6191 RVA: 0x0009154D File Offset: 0x0008F74D
		public BoundLocalizedText StrUseFollowingDNS { get; set; }

		// Token: 0x14000078 RID: 120
		// (add) Token: 0x06001830 RID: 6192 RVA: 0x00091558 File Offset: 0x0008F758
		// (remove) Token: 0x06001831 RID: 6193 RVA: 0x00091590 File Offset: 0x0008F790
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<bool> DNSCleared;

		// Token: 0x06001832 RID: 6194 RVA: 0x000915C8 File Offset: 0x0008F7C8
		public IPConfigurationViewModel(bool wired, MachineState machineState)
		{
			this.machineState = machineState;
			this.SetIPAddressCommand = new RelayCommand(new Action<object>(this.SetIPAddress));
			this.SetSubnetMaskCommand = new RelayCommand(new Action<object>(this.SetSubnetMask));
			this.SetGatewayCommand = new RelayCommand(new Action<object>(this.SetGateway));
			this.SetPreferredDNSCommand = new RelayCommand(new Action<object>(this.SetPreferredDNS));
			this.SetAlternateDNSCommand = new RelayCommand(new Action<object>(this.SetAlternateDNS));
			this.CleanUpCommand = new RelayCommand(new Action<object>(this.CleanUp));
			this.RegisterLocalizedText();
			if (wired)
			{
				this.NIC = IPConfigRegistry.GetNICFromRegistry(Definition.guidsExternalNIC, Definition.captionsExternalNIC);
			}
			else
			{
				this.NIC = IPConfigRegistry.GetNICFromRegistry(Definition.guidsWireless, Definition.captionsWireless);
			}
		}

		// Token: 0x06001833 RID: 6195 RVA: 0x000916B0 File Offset: 0x0008F8B0
		private void RegisterLocalizedText()
		{
			this.StrIPAddress = new BoundLocalizedText("txtIPAddress");
			this.StrSubnetMask = new BoundLocalizedText("txtSubnetMask");
			this.StrDefaultGateway = new BoundLocalizedText("txtDefaultGateway");
			this.StrPreferredDNSServer = new BoundLocalizedText("txtPreferredDNSServer");
			this.StrAlternateDNSServer = new BoundLocalizedText("txtAlternateDNSServer");
			this.StrObtainIPAutomatically = new BoundLocalizedText("txtObtainIPAutomatically");
			this.StrUseFollowingIP = new BoundLocalizedText("txtUseFollowingIP");
			this.StrObtainDNSAutomatically = new BoundLocalizedText("txtObtainDNSAutomatically");
			this.StrUseFollowingDNS = new BoundLocalizedText("txtUseFollowingDNS");
		}

		// Token: 0x06001834 RID: 6196 RVA: 0x00091758 File Offset: 0x0008F958
		private void DeregisterLocalizedText()
		{
			MainWindow.localizationManager.Deregister(this.StrIPAddress);
			MainWindow.localizationManager.Deregister(this.StrSubnetMask);
			MainWindow.localizationManager.Deregister(this.StrDefaultGateway);
			MainWindow.localizationManager.Deregister(this.StrPreferredDNSServer);
			MainWindow.localizationManager.Deregister(this.StrAlternateDNSServer);
			MainWindow.localizationManager.Deregister(this.StrObtainIPAutomatically);
			MainWindow.localizationManager.Deregister(this.StrUseFollowingIP);
			MainWindow.localizationManager.Deregister(this.StrObtainDNSAutomatically);
			MainWindow.localizationManager.Deregister(this.StrUseFollowingDNS);
		}

		// Token: 0x06001835 RID: 6197 RVA: 0x000917FF File Offset: 0x0008F9FF
		private void CleanUp(object obj)
		{
			this.DeregisterLocalizedText();
		}

		// Token: 0x06001836 RID: 6198 RVA: 0x0009180C File Offset: 0x0008FA0C
		private void SetIPAddress(object obj)
		{
			string oldValue = (this.NIC.IPAddress != null) ? this.NIC.IPAddress.ToString() : null;
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtIPAddress"), oldValue, NumKeyboardType.TypeIPAddress);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_InputIP;
			this.numKeyboard.Show();
		}

		// Token: 0x06001837 RID: 6199 RVA: 0x00091878 File Offset: 0x0008FA78
		private void numKeyboard_InputIP(object sender, KeyboardInputEventArgs e)
		{
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_InputIP;
			IPAddress ipaddress;
			bool flag = IPAddress.TryParse(e.Text, out ipaddress);
			if (flag)
			{
				bool flag2 = !ipaddress.ToString().StartsWith("192.168.0.");
				if (flag2)
				{
					this.NIC.ConfigurationChanged = true;
					this.NIC.IPAddress = ipaddress;
				}
				else
				{
					Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtSubnetReserved") + " (\"192.168.0\"). " + LocalizationManager.Localize("txtChooseAnotherSubnet"), DialogStyle.Error, DialogButtons.OK);
				}
			}
		}

		// Token: 0x06001838 RID: 6200 RVA: 0x00091918 File Offset: 0x0008FB18
		private void SetSubnetMask(object obj)
		{
			string oldValue = (this.NIC.SubnetMask != null) ? this.NIC.SubnetMask.ToString() : "255.255.255.0";
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtSubnetMask"), oldValue, NumKeyboardType.TypeSubnetMask);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_InputSubnetMask;
			this.numKeyboard.Show();
		}

		// Token: 0x06001839 RID: 6201 RVA: 0x00091988 File Offset: 0x0008FB88
		private void numKeyboard_InputSubnetMask(object sender, KeyboardInputEventArgs e)
		{
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_InputIP;
			IPAddress subnetMask;
			bool flag = IPAddress.TryParse(e.Text, out subnetMask);
			if (flag)
			{
				this.NIC.ConfigurationChanged = true;
				this.NIC.SubnetMask = subnetMask;
			}
		}

		// Token: 0x0600183A RID: 6202 RVA: 0x000919DC File Offset: 0x0008FBDC
		private void SetGateway(object obj)
		{
			string oldValue = (this.NIC.DefaultGateway != null) ? this.NIC.DefaultGateway.ToString() : null;
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtDefaultGateway"), oldValue, NumKeyboardType.TypeIPAddress);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_InputDefaultGateway;
			this.numKeyboard.Show();
		}

		// Token: 0x0600183B RID: 6203 RVA: 0x00091A48 File Offset: 0x0008FC48
		private void numKeyboard_InputDefaultGateway(object sender, KeyboardInputEventArgs e)
		{
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_InputDefaultGateway;
			IPAddress defaultGateway;
			bool flag = IPAddress.TryParse(e.Text, out defaultGateway);
			if (flag)
			{
				this.NIC.ConfigurationChanged = true;
				this.NIC.DefaultGateway = defaultGateway;
			}
		}

		// Token: 0x0600183C RID: 6204 RVA: 0x00091A9C File Offset: 0x0008FC9C
		private void SetPreferredDNS(object obj)
		{
			string oldValue = (this.NIC.PreferredDNS != null) ? this.NIC.PreferredDNS.ToString() : null;
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtPreferredDNSServer"), oldValue, NumKeyboardType.TypeDNSServer);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_InputPreferredDNS;
			this.numKeyboard.Show();
		}

		// Token: 0x0600183D RID: 6205 RVA: 0x00091B08 File Offset: 0x0008FD08
		private void numKeyboard_InputPreferredDNS(object sender, KeyboardInputEventArgs e)
		{
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_InputPreferredDNS;
			IPAddress preferredDNS;
			bool flag = IPAddress.TryParse(e.Text, out preferredDNS);
			if (flag)
			{
				this.NIC.PreferredDNS = preferredDNS;
			}
			else
			{
				this.NIC.PreferredDNS = null;
			}
			this.NIC.ConfigurationChanged = true;
		}

		// Token: 0x0600183E RID: 6206 RVA: 0x00091B68 File Offset: 0x0008FD68
		private void SetAlternateDNS(object obj)
		{
			string oldValue = (this.NIC.AlternateDNS != null) ? this.NIC.AlternateDNS.ToString() : null;
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtAlternateDNSServer"), oldValue, NumKeyboardType.TypeDNSServer);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_InputAlternateDNS;
			this.numKeyboard.Show();
		}

		// Token: 0x0600183F RID: 6207 RVA: 0x00091BD4 File Offset: 0x0008FDD4
		private void numKeyboard_InputAlternateDNS(object sender, KeyboardInputEventArgs e)
		{
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_InputAlternateDNS;
			IPAddress alternateDNS;
			bool flag = IPAddress.TryParse(e.Text, out alternateDNS);
			if (flag)
			{
				this.NIC.AlternateDNS = alternateDNS;
			}
			else
			{
				this.NIC.AlternateDNS = null;
			}
			this.NIC.ConfigurationChanged = true;
		}

		// Token: 0x06001840 RID: 6208 RVA: 0x00091C34 File Offset: 0x0008FE34
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x06001841 RID: 6209 RVA: 0x00091C64 File Offset: 0x0008FE64
		public bool CommitChanges()
		{
			bool result = false;
			bool flag = this.NIC != null;
			if (flag)
			{
				bool flag2 = this.NIC.SettingsValid();
				bool flag3 = flag2;
				if (flag3)
				{
					bool flag4 = this.machineState.State == MachineStates.Inspecting || this.machineState.State == MachineStates.XrayOnCalibrationNotDone;
					if (flag4)
					{
						Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtTurnXrayOff"), DialogStyle.Error, DialogButtons.OK);
					}
					else
					{
						bool flag5 = Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtRebootRequired"), DialogStyle.Warning, DialogButtons.OKCancel) == Dialog.DialogResults.Ok;
						if (flag5)
						{
							IPConfigRegistry.SetNICInRegistry(this.NIC);
							this.NIC.ConfigurationChanged = false;
							bool flag6 = File.Exists("C:\\CommitAndRestart.bat");
							if (flag6)
							{
								Process.Start("C:\\CommitAndRestart.bat");
							}
							else
							{
								Dialog.Show(LocalizationManager.Localize("txtError"), "File \"C:\\CommitAndRestart.bat\" not found", DialogStyle.Error, DialogButtons.OK);
							}
							result = true;
						}
					}
				}
				else
				{
					Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtSettingsInvalid") + ".", DialogStyle.Error, DialogButtons.OK);
				}
			}
			return result;
		}

		// Token: 0x06001842 RID: 6210 RVA: 0x00091D8C File Offset: 0x0008FF8C
		protected virtual void OnDNSCleared(bool preferredDNS)
		{
			Action<bool> dnscleared = this.DNSCleared;
			bool flag = dnscleared != null;
			if (flag)
			{
				dnscleared(preferredDNS);
			}
		}

		// Token: 0x04000FB4 RID: 4020
		private const string commitAndRestartFilePath = "C:\\CommitAndRestart.bat";

		// Token: 0x04000FB5 RID: 4021
		private NumKeyboard numKeyboard;

		// Token: 0x04000FB6 RID: 4022
		private MachineState machineState;
	}
}
