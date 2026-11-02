using System;
using System.Windows.Controls;
using System.Windows.Media;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x0200016F RID: 367
	public class WifiConnectIcons
	{
		// Token: 0x060021E5 RID: 8677 RVA: 0x000DAB42 File Offset: 0x000D8D42
		public WifiConnectIcons(Button btnConnectOrDisconnect)
		{
			this.Initialize(btnConnectOrDisconnect);
		}

		// Token: 0x060021E6 RID: 8678 RVA: 0x000DAB54 File Offset: 0x000D8D54
		private void Initialize(Button btnConnectOrDisconnect)
		{
			this.btnConnectOrDisconnect = btnConnectOrDisconnect;
			this.connectImage = ImageConverter.ToImage("/img/wifiConnect.png");
			this.connectImage.Width = 38.0;
			this.connectImage.Stretch = Stretch.Uniform;
			this.disconnectImage = ImageConverter.ToImage("/img/wifiDisconnect.png");
			this.disconnectImage.Width = 38.0;
			this.disconnectImage.Stretch = Stretch.Uniform;
			this.noWifiImage = ImageConverter.ToImage("/img/wifiQualityNone.png");
			this.noWifiImage.Width = 32.0;
			this.noWifiImage.Stretch = Stretch.Uniform;
		}

		// Token: 0x060021E7 RID: 8679 RVA: 0x000DAC00 File Offset: 0x000D8E00
		public void SetConnectedIcon(int nAvailableNetworks, WifiNetwork connectedNetwork)
		{
			bool flag = nAvailableNetworks == 0;
			if (flag)
			{
				bool flag2 = this.currentConnectIconPath != "/img/wifiQualityNone.png";
				if (flag2)
				{
					this.currentConnectIconPath = "/img/wifiQualityNone.png";
					this.btnConnectOrDisconnect.Content = this.noWifiImage;
				}
			}
			else
			{
				bool flag3 = connectedNetwork != null && this.currentConnectIconPath != "/img/wifiDisconnect.png";
				if (flag3)
				{
					this.currentConnectIconPath = "/img/wifiDisconnect.png";
					this.btnConnectOrDisconnect.Content = this.disconnectImage;
				}
				else
				{
					bool flag4 = connectedNetwork == null && this.currentConnectIconPath != "/img/wifiConnect.png";
					if (flag4)
					{
						this.currentConnectIconPath = "/img/wifiConnect.png";
						this.btnConnectOrDisconnect.Content = this.connectImage;
					}
				}
			}
		}

		// Token: 0x040015F6 RID: 5622
		private string currentConnectIconPath;

		// Token: 0x040015F7 RID: 5623
		private const string pathConnectIcon = "/img/wifiConnect.png";

		// Token: 0x040015F8 RID: 5624
		private const string pathDisconnectIcon = "/img/wifiDisconnect.png";

		// Token: 0x040015F9 RID: 5625
		private const string pathNoWifiIcon = "/img/wifiQualityNone.png";

		// Token: 0x040015FA RID: 5626
		private Image connectImage;

		// Token: 0x040015FB RID: 5627
		private Image disconnectImage;

		// Token: 0x040015FC RID: 5628
		private Image noWifiImage;

		// Token: 0x040015FD RID: 5629
		private Button btnConnectOrDisconnect;
	}
}
