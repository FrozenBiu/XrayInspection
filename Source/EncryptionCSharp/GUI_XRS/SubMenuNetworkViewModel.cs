using System;
using System.Collections.ObjectModel;
using Common;
using DatabaseConnection;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x0200015D RID: 349
	public class SubMenuNetworkViewModel
	{
		// Token: 0x1700098B RID: 2443
		// (get) Token: 0x060020F9 RID: 8441 RVA: 0x000D7060 File Offset: 0x000D5260
		// (set) Token: 0x060020FA RID: 8442 RVA: 0x000D7068 File Offset: 0x000D5268
		public ObservableCollection<SubMenuItem> SubMenuItems { get; set; }

		// Token: 0x1700098C RID: 2444
		// (get) Token: 0x060020FB RID: 8443 RVA: 0x000D7071 File Offset: 0x000D5271
		// (set) Token: 0x060020FC RID: 8444 RVA: 0x000D7079 File Offset: 0x000D5279
		public RelayCommand CloseCommand { get; set; }

		// Token: 0x1700098D RID: 2445
		// (get) Token: 0x060020FD RID: 8445 RVA: 0x000D7082 File Offset: 0x000D5282
		// (set) Token: 0x060020FE RID: 8446 RVA: 0x000D708A File Offset: 0x000D528A
		public MachineState MachineState { get; set; }

		// Token: 0x1700098E RID: 2446
		// (get) Token: 0x060020FF RID: 8447 RVA: 0x000D7093 File Offset: 0x000D5293
		// (set) Token: 0x06002100 RID: 8448 RVA: 0x000D709B File Offset: 0x000D529B
		public bool DemoMode { get; set; }

		// Token: 0x06002101 RID: 8449 RVA: 0x000D70A4 File Offset: 0x000D52A4
		public SubMenuNetworkViewModel(AccessRightsManager accessManager, WifiClient wifiClient, DBConnection dbConnection, MachineState machineState)
		{
			this.accessManager = accessManager;
			this.wifiClient = wifiClient;
			this.dbConnection = dbConnection;
			this.MachineState = machineState;
			this.CloseCommand = new RelayCommand(new Action<object>(this.CloseSubMenu));
			bool demoMode = false;
			dbConnection.GetDemoMode(ref demoMode);
			this.DemoMode = demoMode;
			this.FillMenuItems();
		}

		// Token: 0x06002102 RID: 8450 RVA: 0x000D710C File Offset: 0x000D530C
		private void FillMenuItems()
		{
			this.SubMenuItems = new ObservableCollection<SubMenuItem>();
			SubMenuItem item = new SubMenuItem("Adaptereinstellung (kabelgebunden)", "menuItemWired", "..\\..\\img\\networkSettings.png", new Action<object>(this.btnEthernet_Click), this.accessManager, true, true);
			this.SubMenuItems.Add(item);
			bool flag = this.wifiClient.WlanClient != null && this.wifiClient.WlanClient.Interfaces.Length != 0 && IPConfigRegistry.AnyNICAvailable(Definition.guidsWireless);
			if (flag)
			{
				SubMenuItem item2 = new SubMenuItem("Adaptereinstellung (drahtlos)", "menuItemWireless", "..\\..\\img\\wifiSettings.png", new Action<object>(this.btnWifi_Click), this.accessManager, true, true);
				this.SubMenuItems.Add(item2);
			}
			bool flag2 = this.wifiClient.WlanClient != null && this.wifiClient.WlanClient.Interfaces.Length != 0;
			if (flag2)
			{
				SubMenuItem item3 = new SubMenuItem("Drahtlosnetzwerk verwalten", "menuItemManageWireless", "..\\..\\img\\wifiManage.png", new Action<object>(this.btnManageWifi_Click), this.accessManager, true, true);
				this.SubMenuItems.Add(item3);
			}
			SubMenuItem item4 = new SubMenuItem("Bildtransfer", "menuItemImageTransfer", "..\\..\\img\\xriimagelist.png", new Action<object>(this.btnImageTransfer_Click), this.accessManager, true, true);
			this.SubMenuItems.Add(item4);
		}

		// Token: 0x06002103 RID: 8451 RVA: 0x000D7260 File Offset: 0x000D5460
		private void btnEthernet_Click(object sender)
		{
			bool flag = IPConfigRegistry.AnyNICAvailable(Definition.guidsExternalNIC);
			if (flag)
			{
				CtrlIPConfiguration ctrlIPConfiguration = new CtrlIPConfiguration(true, this.MachineState);
				Subwindow.Show(ctrlIPConfiguration, SubwindowButtons.BackAndHome, ctrlIPConfiguration.Buttons);
			}
			else
			{
				Dialog.Show("Error", "No NIC available.", DialogStyle.Error);
			}
		}

		// Token: 0x06002104 RID: 8452 RVA: 0x000D72B0 File Offset: 0x000D54B0
		private void btnWifi_Click(object sender)
		{
			bool flag = IPConfigRegistry.AnyNICAvailable(Definition.guidsWireless);
			if (flag)
			{
				CtrlIPConfiguration ctrlIPConfiguration = new CtrlIPConfiguration(false, this.MachineState);
				Subwindow.Show(ctrlIPConfiguration, SubwindowButtons.BackAndHome, ctrlIPConfiguration.Buttons);
			}
			else
			{
				Dialog.Show("Error", "No NIC for wireless communication available.", DialogStyle.Error);
			}
		}

		// Token: 0x06002105 RID: 8453 RVA: 0x000D7300 File Offset: 0x000D5500
		private void btnManageWifi_Click(object sender)
		{
			bool flag = this.wifiClient.WlanClient != null && this.wifiClient.WlanClient.Interfaces.Length != 0;
			if (flag)
			{
				CtrlWifiManage ctrlWifiManage = new CtrlWifiManage(this.wifiClient, this.dbConnection);
				Subwindow.Show(ctrlWifiManage, SubwindowButtons.BackAndHome, ctrlWifiManage.Buttons);
			}
			else
			{
				Dialog.Show("Error", "Wifi service not running. Wrong image version?", DialogStyle.Error);
			}
		}

		// Token: 0x06002106 RID: 8454 RVA: 0x000D7370 File Offset: 0x000D5570
		private void btnImageTransfer_Click(object sender)
		{
			CtrlWifiImageTransfer ctrlWifiImageTransfer = new CtrlWifiImageTransfer(this.dbConnection);
			Subwindow.Show(ctrlWifiImageTransfer, SubwindowButtons.BackAndHome, ctrlWifiImageTransfer.Dockpanel);
		}

		// Token: 0x06002107 RID: 8455 RVA: 0x000D7398 File Offset: 0x000D5598
		private void CloseSubMenu(object obj)
		{
			foreach (SubMenuItem subMenuItem in this.SubMenuItems)
			{
				subMenuItem.DeregisterLocalizedText();
				bool flag = subMenuItem is SubMenuItemWifi;
				if (flag)
				{
					((SubMenuItemWifi)subMenuItem).StopImageTimer();
				}
			}
		}

		// Token: 0x0400155E RID: 5470
		private AccessRightsManager accessManager;

		// Token: 0x0400155F RID: 5471
		private WifiClient wifiClient;

		// Token: 0x04001560 RID: 5472
		private DBConnection dbConnection;
	}
}
