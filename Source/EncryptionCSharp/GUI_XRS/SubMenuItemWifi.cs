using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using NativeWifi;

namespace GUI_XRS
{
	// Token: 0x0200015B RID: 347
	public class SubMenuItemWifi : SubMenuItem
	{
		// Token: 0x060020E7 RID: 8423 RVA: 0x000D6A7C File Offset: 0x000D4C7C
		public SubMenuItemWifi(string localizedText, string resourceKey, Action<object> clickHandler, AccessRightsManager accessManager) : base(localizedText, resourceKey, "..\\..\\img\\wifiQualityNone.png", clickHandler, accessManager, true, true)
		{
			this.SetQualityImage(null, new EventArgs());
			this.timerSetQualityImage = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Background, new EventHandler(this.SetQualityImage), Application.Current.Dispatcher);
		}

		// Token: 0x060020E8 RID: 8424 RVA: 0x000D6AE0 File Offset: 0x000D4CE0
		private void SetQualityImage(object sender, EventArgs e)
		{
			bool flag = this.client.Interfaces.Length != 0 && this.client.Interfaces[0].InterfaceState == Wlan.WlanInterfaceState.Connected;
			if (flag)
			{
				Wlan.Dot11Ssid wlanSSID;
				wlanSSID.SSID = new byte[0];
				try
				{
					wlanSSID = this.client.Interfaces[0].CurrentConnection.wlanAssociationAttributes.dot11Ssid;
				}
				catch (Win32Exception)
				{
					this.SetQualityImagePath(0U);
				}
				Wlan.WlanAvailableNetwork[] availableNetworkList = this.client.Interfaces[0].GetAvailableNetworkList((Wlan.WlanGetAvailableNetworkFlags)0);
				List<Wlan.WlanAvailableNetwork> list = (from network in availableNetworkList
				where network.dot11Ssid.SSID.SequenceEqual(wlanSSID.SSID)
				select network).ToList<Wlan.WlanAvailableNetwork>();
				bool flag2 = list.Count > 0;
				if (flag2)
				{
					this.SetQualityImagePath(list[0].wlanSignalQuality);
				}
				else
				{
					this.SetQualityImagePath(0U);
				}
			}
			else
			{
				this.SetQualityImagePath(0U);
			}
		}

		// Token: 0x060020E9 RID: 8425 RVA: 0x000D6BDC File Offset: 0x000D4DDC
		private void SetQualityImagePath(uint signalQuality)
		{
			bool flag = signalQuality == 0U;
			if (flag)
			{
				base.ImagePath = "..\\..\\img\\wifiQualityNone.png";
			}
			else
			{
				bool flag2 = signalQuality < 20U;
				if (flag2)
				{
					base.ImagePath = "..\\..\\img\\wifiQualityLow.png";
				}
				else
				{
					bool flag3 = signalQuality < 40U;
					if (flag3)
					{
						base.ImagePath = "..\\..\\img\\wifiQualityMediumLow.png";
					}
					else
					{
						bool flag4 = signalQuality < 60U;
						if (flag4)
						{
							base.ImagePath = "..\\..\\img\\wifiQualityMedium.png";
						}
						else
						{
							bool flag5 = signalQuality < 80U;
							if (flag5)
							{
								base.ImagePath = "..\\..\\img\\wifiQualityMediumHigh.png";
							}
							else
							{
								base.ImagePath = "..\\..\\img\\wifiQualityHigh.png";
							}
						}
					}
				}
			}
		}

		// Token: 0x060020EA RID: 8426 RVA: 0x000D6C6A File Offset: 0x000D4E6A
		public void StopImageTimer()
		{
			this.timerSetQualityImage.Stop();
		}

		// Token: 0x04001551 RID: 5457
		private DispatcherTimer timerSetQualityImage;

		// Token: 0x04001552 RID: 5458
		private WlanClient client = new WlanClient();
	}
}
