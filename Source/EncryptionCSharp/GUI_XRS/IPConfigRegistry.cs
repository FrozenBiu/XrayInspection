using System;
using System.Net;
using Microsoft.Win32;

namespace GUI_XRS
{
	// Token: 0x020000FC RID: 252
	public static class IPConfigRegistry
	{
		// Token: 0x06001843 RID: 6211 RVA: 0x00091DB4 File Offset: 0x0008FFB4
		public static NIC GetNICFromRegistry(string[] guids, string[] captions)
		{
			NIC nic = null;
			bool flag = guids != null && captions != null && guids.Length == captions.Length;
			if (flag)
			{
				for (int i = 0; i < guids.Length; i++)
				{
					string keyName = IPConfigRegistry.regPathInterfaceMain + guids[i];
					int? num = Registry.GetValue(keyName, IPConfigRegistry.regKeyEnableDHCP, "") as int?;
					bool flag2 = num != null;
					if (flag2)
					{
						nic = new NIC(guids[i], captions[i]);
						NIC nic2 = nic;
						int? num2 = num;
						int num3 = 1;
						nic2.DHCPEnabled = (num2.GetValueOrDefault() == num3 & num2 != null);
						bool flag3 = !nic.DHCPEnabled;
						if (flag3)
						{
							string[] array = Registry.GetValue(keyName, IPConfigRegistry.regKeyIPAddress, "") as string[];
							string[] array2 = Registry.GetValue(keyName, IPConfigRegistry.regKeySubnetMask, "") as string[];
							string[] array3 = Registry.GetValue(keyName, IPConfigRegistry.regKeyDefaultGateway, "") as string[];
							bool flag4 = array != null && array.Length != 0;
							if (flag4)
							{
								nic.IPAddress = IPAddress.Parse(array[0]);
							}
							bool flag5 = array2 != null && array2.Length != 0;
							if (flag5)
							{
								nic.SubnetMask = IPAddress.Parse(array2[0]);
							}
							bool flag6 = array3 != null && array3.Length != 0;
							if (flag6)
							{
								nic.DefaultGateway = IPAddress.Parse(array3[0]);
							}
						}
						string text = (string)Registry.GetValue(keyName, IPConfigRegistry.regKeyNameServer, "");
						nic.DNSAutomatic = (text == null || text.Length <= 0);
						bool flag7 = text != null && text.Length > 0;
						if (flag7)
						{
							string[] array4 = text.Split(new char[]
							{
								','
							});
							bool flag8 = array4.Length != 0;
							if (flag8)
							{
								nic.PreferredDNS = IPAddress.Parse(array4[0]);
							}
							bool flag9 = array4.Length > 1;
							if (flag9)
							{
								nic.AlternateDNS = IPAddress.Parse(array4[1]);
							}
						}
						break;
					}
				}
			}
			return nic;
		}

		// Token: 0x06001844 RID: 6212 RVA: 0x00091FC8 File Offset: 0x000901C8
		public static void SetNICInRegistry(NIC nic)
		{
			bool flag = nic != null && nic.Guid != null;
			if (flag)
			{
				string keyName = IPConfigRegistry.regPathInterfaceMain + nic.Guid;
				Registry.SetValue(keyName, IPConfigRegistry.regKeyEnableDHCP, nic.DHCPEnabled ? 1 : 0);
				bool flag2 = nic.IPAddress != null;
				if (flag2)
				{
					bool dhcpenabled = nic.DHCPEnabled;
					if (dhcpenabled)
					{
						Registry.SetValue(keyName, IPConfigRegistry.regKeyIPAddress, new string[]
						{
							""
						});
					}
					else
					{
						Registry.SetValue(keyName, IPConfigRegistry.regKeyIPAddress, new string[]
						{
							nic.IPAddress.ToString()
						});
					}
				}
				Registry.SetValue(keyName, IPConfigRegistry.regKeySubnetMask, new string[]
				{
					nic.SubnetMask.ToString()
				});
				bool flag3 = nic.DefaultGateway != null;
				if (flag3)
				{
					Registry.SetValue(keyName, IPConfigRegistry.regKeyDefaultGateway, new string[]
					{
						nic.DefaultGateway.ToString()
					});
				}
				string text = "";
				bool flag4 = !nic.DNSAutomaticReal && nic.PreferredDNS != null;
				if (flag4)
				{
					text = nic.PreferredDNS.ToString();
					bool flag5 = nic.AlternateDNS != null;
					if (flag5)
					{
						text = text + "," + nic.AlternateDNS.ToString();
					}
				}
				Registry.SetValue(keyName, IPConfigRegistry.regKeyNameServer, text);
			}
		}

		// Token: 0x06001845 RID: 6213 RVA: 0x00092124 File Offset: 0x00090324
		public static bool AnyNICAvailable(string[] guids)
		{
			bool result = false;
			bool flag = guids != null;
			if (flag)
			{
				for (int i = 0; i < guids.Length; i++)
				{
					string keyName = IPConfigRegistry.regPathInterfaceMain + guids[i];
					bool flag2 = Registry.GetValue(keyName, IPConfigRegistry.regKeyEnableDHCP, "") is int?;
					if (flag2)
					{
						result = true;
						break;
					}
				}
			}
			return result;
		}

		// Token: 0x04000FC9 RID: 4041
		private static readonly string regPathInterfaceMain = "HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\services\\Tcpip\\Parameters\\Interfaces\\";

		// Token: 0x04000FCA RID: 4042
		private static readonly string regKeyEnableDHCP = "EnableDHCP";

		// Token: 0x04000FCB RID: 4043
		private static readonly string regKeyIPAddress = "IPAddress";

		// Token: 0x04000FCC RID: 4044
		private static readonly string regKeySubnetMask = "SubnetMask";

		// Token: 0x04000FCD RID: 4045
		private static readonly string regKeyDefaultGateway = "DefaultGateway";

		// Token: 0x04000FCE RID: 4046
		private static readonly string regKeyNameServer = "NameServer";
	}
}
