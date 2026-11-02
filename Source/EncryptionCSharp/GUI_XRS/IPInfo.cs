using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;

namespace GUI_XRS
{
	// Token: 0x02000184 RID: 388
	public static class IPInfo
	{
		// Token: 0x060022FB RID: 8955 RVA: 0x000E1644 File Offset: 0x000DF844
		public static string GetIPConfigString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			NetworkInterface[] allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
			foreach (NetworkInterface adapter in allNetworkInterfaces)
			{
				IPInfo.AppendAdapterString(adapter, stringBuilder);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060022FC RID: 8956 RVA: 0x000E168C File Offset: 0x000DF88C
		private static void AppendAdapterString(NetworkInterface adapter, StringBuilder ipConfigString)
		{
			try
			{
				IPInterfaceProperties ipproperties = adapter.GetIPProperties();
				bool flag = !IPInfo.IsLoopbackAdapter(adapter);
				if (flag)
				{
					ipConfigString.AppendLine();
					ipConfigString.AppendLine(adapter.Name + ":");
					ipConfigString.AppendLine();
					IPInfo.AppendDHCPString(adapter, ipConfigString);
					IPInfo.AppendIPAndMaskString(adapter, ipConfigString);
					IPInfo.AppendGatewayString(adapter, ipConfigString);
					IPInfo.AppendServerStrings(adapter, ipConfigString);
					ipConfigString.AppendLine();
				}
			}
			catch (Exception ex)
			{
				Dialog.Show("Error", ex.Message, DialogStyle.Error, DialogButtons.OK);
			}
		}

		// Token: 0x060022FD RID: 8957 RVA: 0x000E1728 File Offset: 0x000DF928
		private static bool IsLoopbackAdapter(NetworkInterface adapter)
		{
			bool result = false;
			IPInterfaceProperties ipproperties = adapter.GetIPProperties();
			bool flag = ipproperties.UnicastAddresses.Count > 0;
			if (flag)
			{
				foreach (UnicastIPAddressInformation unicastIPAddressInformation in ipproperties.UnicastAddresses)
				{
					bool flag2 = unicastIPAddressInformation.Address.ToString() == "127.0.0.1";
					if (flag2)
					{
						result = true;
						break;
					}
				}
			}
			return result;
		}

		// Token: 0x060022FE RID: 8958 RVA: 0x000E17BC File Offset: 0x000DF9BC
		private static void AppendDHCPString(NetworkInterface adapter, StringBuilder ipConfigString)
		{
			string str = "No";
			try
			{
				bool flag = adapter.GetIPProperties().GetIPv4Properties() != null && adapter.GetIPProperties().GetIPv4Properties().IsDhcpEnabled;
				if (flag)
				{
					str = "Yes";
				}
			}
			catch (NetworkInformationException)
			{
			}
			ipConfigString.AppendLine("DHCP active: " + str);
		}

		// Token: 0x060022FF RID: 8959 RVA: 0x000E1828 File Offset: 0x000DFA28
		private static void AppendIPAndMaskString(NetworkInterface adapter, StringBuilder ipConfigString)
		{
			bool flag = adapter.GetIPProperties().UnicastAddresses.Count > 0;
			if (flag)
			{
				ipConfigString.Append("IP address: ");
				foreach (UnicastIPAddressInformation unicastIPAddressInformation in adapter.GetIPProperties().UnicastAddresses)
				{
					bool flag2 = unicastIPAddressInformation.Address != null && !unicastIPAddressInformation.Address.IsIPv6LinkLocal && !unicastIPAddressInformation.Address.IsIPv6Multicast && !unicastIPAddressInformation.Address.IsIPv6SiteLocal;
					if (flag2)
					{
						ipConfigString.AppendLine(unicastIPAddressInformation.Address.ToString());
					}
				}
				ipConfigString.Append("Subnet mask: ");
				foreach (UnicastIPAddressInformation unicastIPAddressInformation2 in adapter.GetIPProperties().UnicastAddresses)
				{
					bool flag3 = unicastIPAddressInformation2.Address != null && !unicastIPAddressInformation2.Address.IsIPv6LinkLocal && !unicastIPAddressInformation2.Address.IsIPv6Multicast && !unicastIPAddressInformation2.Address.IsIPv6SiteLocal && unicastIPAddressInformation2.IPv4Mask != null;
					if (flag3)
					{
						ipConfigString.AppendLine(unicastIPAddressInformation2.IPv4Mask.ToString());
					}
				}
			}
		}

		// Token: 0x06002300 RID: 8960 RVA: 0x000E1994 File Offset: 0x000DFB94
		private static void AppendGatewayString(NetworkInterface adapter, StringBuilder ipConfigString)
		{
			bool flag = adapter.GetIPProperties().GatewayAddresses.Count > 0;
			if (flag)
			{
				ipConfigString.Append("Gateway: ");
				foreach (GatewayIPAddressInformation gatewayIPAddressInformation in adapter.GetIPProperties().GatewayAddresses)
				{
					bool flag2 = gatewayIPAddressInformation.Address != null && !gatewayIPAddressInformation.Address.IsIPv6LinkLocal && !gatewayIPAddressInformation.Address.IsIPv6Multicast && !gatewayIPAddressInformation.Address.IsIPv6SiteLocal;
					if (flag2)
					{
						ipConfigString.AppendLine(gatewayIPAddressInformation.Address.ToString());
					}
				}
			}
		}

		// Token: 0x06002301 RID: 8961 RVA: 0x000E1A58 File Offset: 0x000DFC58
		private static void AppendServerStrings(NetworkInterface adapter, StringBuilder ipConfigString)
		{
			IPInterfaceProperties ipproperties = adapter.GetIPProperties();
			try
			{
				bool flag = ipproperties.GetIPv4Properties() != null && ipproperties.GetIPv4Properties().IsDhcpEnabled;
				if (flag)
				{
					bool flag2 = ipproperties.DhcpServerAddresses.Count > 0;
					if (flag2)
					{
						ipConfigString.Append("DHCP-Server: ");
						foreach (IPAddress ipaddress in ipproperties.DhcpServerAddresses)
						{
							bool flag3 = ipaddress != null && !ipaddress.IsIPv6LinkLocal && !ipaddress.IsIPv6Multicast && !ipaddress.IsIPv6SiteLocal;
							if (flag3)
							{
								ipConfigString.AppendLine(ipaddress.ToString());
							}
						}
					}
					bool flag4 = ipproperties.DnsAddresses.Count > 0;
					if (flag4)
					{
						ipConfigString.Append("DNS-Server: ");
						foreach (IPAddress ipaddress2 in ipproperties.DnsAddresses)
						{
							bool flag5 = ipaddress2 != null && !ipaddress2.IsIPv6LinkLocal && !ipaddress2.IsIPv6Multicast && !ipaddress2.IsIPv6SiteLocal;
							if (flag5)
							{
								ipConfigString.AppendLine(ipaddress2.ToString());
							}
						}
					}
				}
			}
			catch (NetworkInformationException)
			{
			}
		}
	}
}
