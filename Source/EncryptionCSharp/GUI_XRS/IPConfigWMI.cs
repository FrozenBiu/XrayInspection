using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Net;

namespace GUI_XRS
{
	// Token: 0x020000BB RID: 187
	public class IPConfigWMI
	{
		// Token: 0x0600126D RID: 4717 RVA: 0x0006EB7C File Offset: 0x0006CD7C
		public static void SetIP(string caption, IPAddress ipAddress, IPAddress subnetMask, IPAddress gateway)
		{
			using (ManagementClass managementClass = new ManagementClass(IPConfigWMI.wmiConfigPath))
			{
				using (ManagementObjectCollection instances = managementClass.GetInstances())
				{
					foreach (ManagementObject managementObject in instances.Cast<ManagementObject>())
					{
						bool flag = (string)managementObject[IPConfigWMI.wmiKeyCaption] == caption;
						if (flag)
						{
							using (ManagementBaseObject methodParameters = managementObject.GetMethodParameters(IPConfigWMI.wmiEnableStatic))
							{
								bool flag2 = (ipAddress != null && !string.IsNullOrEmpty(ipAddress.ToString())) || (subnetMask != null && !string.IsNullOrEmpty(subnetMask.ToString()));
								if (flag2)
								{
									bool flag3 = !string.IsNullOrEmpty(ipAddress.ToString());
									if (flag3)
									{
										methodParameters[IPConfigWMI.wmiKeyIPAddress] = new string[]
										{
											ipAddress.ToString()
										};
									}
									bool flag4 = !string.IsNullOrEmpty(subnetMask.ToString());
									if (flag4)
									{
										methodParameters[IPConfigWMI.wmiSetSubnetMask] = new string[]
										{
											subnetMask.ToString()
										};
									}
									managementObject.InvokeMethod(IPConfigWMI.wmiEnableStatic, methodParameters, null);
								}
								bool flag5 = gateway != null && !string.IsNullOrEmpty(gateway.ToString());
								if (flag5)
								{
									using (ManagementBaseObject methodParameters2 = managementObject.GetMethodParameters(IPConfigWMI.wmiSetGateways))
									{
										methodParameters2[IPConfigWMI.wmiDefaultGateway] = new ManagementBaseObject[]
										{
											methodParameters2
										};
										methodParameters2[IPConfigWMI.wmiGatewayCostMetric] = new int[]
										{
											1
										};
										managementObject.InvokeMethod(IPConfigWMI.wmiSetGateways, methodParameters2, null);
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x0600126E RID: 4718 RVA: 0x0006EDC4 File Offset: 0x0006CFC4
		public static void EnableDHCP(string caption)
		{
			using (ManagementClass managementClass = new ManagementClass(IPConfigWMI.wmiConfigPath))
			{
				using (ManagementObjectCollection instances = managementClass.GetInstances())
				{
					foreach (ManagementObject managementObject in instances.Cast<ManagementObject>())
					{
						bool flag = (string)managementObject[IPConfigWMI.wmiKeyCaption] == caption;
						if (flag)
						{
							managementObject.InvokeMethod(IPConfigWMI.wmiEnableDHCP, null, null);
						}
					}
				}
			}
		}

		// Token: 0x0600126F RID: 4719 RVA: 0x0006EE84 File Offset: 0x0006D084
		public static void SetDNS(string caption, IPAddress preferredDNS, IPAddress alternateDNS)
		{
			using (ManagementClass managementClass = new ManagementClass(IPConfigWMI.wmiConfigPath))
			{
				using (ManagementObjectCollection instances = managementClass.GetInstances())
				{
					foreach (ManagementObject managementObject in instances.Cast<ManagementObject>())
					{
						bool flag = (string)managementObject[IPConfigWMI.wmiKeyCaption] == caption;
						if (flag)
						{
							using (ManagementBaseObject methodParameters = managementObject.GetMethodParameters(IPConfigWMI.wmiSetDNS))
							{
								bool flag2 = (preferredDNS != null && !string.IsNullOrEmpty(preferredDNS.ToString())) || (alternateDNS != null && !string.IsNullOrEmpty(alternateDNS.ToString()));
								if (flag2)
								{
									bool flag3 = !string.IsNullOrEmpty(preferredDNS.ToString());
									if (flag3)
									{
										methodParameters[IPConfigWMI.wmiDNSServer] = new string[]
										{
											preferredDNS.ToString()
										};
										bool flag4 = !string.IsNullOrEmpty(alternateDNS.ToString());
										if (flag4)
										{
											methodParameters[IPConfigWMI.wmiDNSServer] = new string[]
											{
												preferredDNS.ToString(),
												alternateDNS.ToString()
											};
										}
									}
								}
								managementObject.InvokeMethod(IPConfigWMI.wmiSetDNS, methodParameters, null);
							}
						}
					}
				}
			}
		}

		// Token: 0x06001270 RID: 4720 RVA: 0x0006F044 File Offset: 0x0006D244
		public static List<NIC> GetNICs()
		{
			List<NIC> list = new List<NIC>();
			using (ManagementClass managementClass = new ManagementClass(IPConfigWMI.wmiConfigPath))
			{
				using (ManagementObjectCollection instances = managementClass.GetInstances())
				{
					foreach (ManagementObject managementObject in instances.Cast<ManagementObject>())
					{
						NIC nic = new NIC();
						nic.Caption = (string)managementObject[IPConfigWMI.wmiKeyCaption];
						string[] array = (string[])managementObject[IPConfigWMI.wmiKeyIPAddress];
						bool flag = array != null && array.Length != 0;
						if (flag)
						{
							nic.IPAddress = IPAddress.Parse(array[0]);
						}
						list.Add(nic);
					}
				}
			}
			return list;
		}

		// Token: 0x06001271 RID: 4721 RVA: 0x0006F148 File Offset: 0x0006D348
		public static NIC GetNIC(string caption)
		{
			NIC nic = null;
			using (ManagementClass managementClass = new ManagementClass(IPConfigWMI.wmiConfigPath))
			{
				using (ManagementObjectCollection instances = managementClass.GetInstances())
				{
					foreach (ManagementObject managementObject in instances.Cast<ManagementObject>())
					{
						bool flag = (string)managementObject[IPConfigWMI.wmiKeyCaption] == caption;
						if (flag)
						{
							nic = new NIC();
							nic.DHCPEnabled = (bool)managementObject[IPConfigWMI.wmiKeyDHCPEnabled];
							string[] array = (string[])managementObject[IPConfigWMI.wmiDNSServer];
							nic.DNSAutomatic = (array == null);
							nic.Caption = (string)managementObject[IPConfigWMI.wmiKeyCaption];
							string[] array2 = (string[])managementObject[IPConfigWMI.wmiKeyIPAddress];
							bool flag2 = array2 != null && array2.Length != 0;
							if (flag2)
							{
								nic.IPAddress = IPAddress.Parse(array2[0]);
							}
							string[] array3 = (string[])managementObject[IPConfigWMI.wmiIPSubnet];
							bool flag3 = array3 != null && array3.Length != 0;
							if (flag3)
							{
								nic.SubnetMask = IPAddress.Parse(array3[0]);
							}
							string[] array4 = (string[])managementObject[IPConfigWMI.wmiDefaultGateway];
							bool flag4 = array4 != null && array4.Length != 0;
							if (flag4)
							{
								nic.DefaultGateway = IPAddress.Parse(array4[0]);
							}
							break;
						}
					}
				}
			}
			return nic;
		}

		// Token: 0x06001272 RID: 4722 RVA: 0x0006F32C File Offset: 0x0006D52C
		public static NIC GetFirstMatchingNIC(string[] captions)
		{
			NIC nic = null;
			using (ManagementClass managementClass = new ManagementClass(IPConfigWMI.wmiConfigPath))
			{
				using (ManagementObjectCollection instances = managementClass.GetInstances())
				{
					foreach (ManagementObject managementObject in instances.Cast<ManagementObject>())
					{
						bool flag = captions.Contains((string)managementObject[IPConfigWMI.wmiKeyCaption]);
						if (flag)
						{
							nic = new NIC();
							nic.DHCPEnabled = (bool)managementObject[IPConfigWMI.wmiKeyDHCPEnabled];
							string[] array = (string[])managementObject[IPConfigWMI.wmiDNSServer];
							nic.DNSAutomatic = (array == null);
							bool flag2 = array != null;
							if (flag2)
							{
								bool flag3 = array.Length != 0;
								if (flag3)
								{
									nic.PreferredDNS = IPAddress.Parse(array[0]);
								}
								bool flag4 = array.Length > 1;
								if (flag4)
								{
									nic.AlternateDNS = IPAddress.Parse(array[1]);
								}
							}
							nic.Caption = (string)managementObject[IPConfigWMI.wmiKeyCaption];
							string[] array2 = (string[])managementObject[IPConfigWMI.wmiKeyIPAddress];
							bool flag5 = array2 != null && array2.Length != 0;
							if (flag5)
							{
								nic.IPAddress = IPAddress.Parse(array2[0]);
							}
							string[] array3 = (string[])managementObject[IPConfigWMI.wmiIPSubnet];
							bool flag6 = array3 != null && array3.Length != 0;
							if (flag6)
							{
								nic.SubnetMask = IPAddress.Parse(array3[0]);
							}
							string[] array4 = (string[])managementObject[IPConfigWMI.wmiDefaultGateway];
							bool flag7 = array4 != null && array4.Length != 0;
							if (flag7)
							{
								nic.DefaultGateway = IPAddress.Parse(array4[0]);
							}
							break;
						}
					}
				}
			}
			return nic;
		}

		// Token: 0x04000C2E RID: 3118
		private static readonly string wmiConfigPath = "Win32_NetworkAdapterConfiguration";

		// Token: 0x04000C2F RID: 3119
		private static readonly string wmiKeyCaption = "Caption";

		// Token: 0x04000C30 RID: 3120
		private static readonly string wmiKeyIPAddress = "IPAddress";

		// Token: 0x04000C31 RID: 3121
		private static readonly string wmiKeyDHCPEnabled = "DHCPEnabled";

		// Token: 0x04000C32 RID: 3122
		private static readonly string wmiIPSubnet = "IPSubnet";

		// Token: 0x04000C33 RID: 3123
		private static readonly string wmiDefaultGateway = "DefaultIPGateway";

		// Token: 0x04000C34 RID: 3124
		private static readonly string wmiDNSServer = "DNSServerSearchOrder";

		// Token: 0x04000C35 RID: 3125
		private static readonly string wmiSetSubnetMask = "SubnetMask";

		// Token: 0x04000C36 RID: 3126
		private static readonly string wmiSetGateways = "SetGateways";

		// Token: 0x04000C37 RID: 3127
		private static readonly string wmiEnableStatic = "EnableStatic";

		// Token: 0x04000C38 RID: 3128
		private static readonly string wmiEnableDHCP = "EnableDHCP";

		// Token: 0x04000C39 RID: 3129
		private static readonly string wmiSetDNS = "SetDNSServerSearchOrder";

		// Token: 0x04000C3A RID: 3130
		private static readonly string wmiGatewayCostMetric = "GatewayCostMetric";
	}
}
