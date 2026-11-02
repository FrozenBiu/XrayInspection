using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace GUI_XRS
{
	// Token: 0x020000EE RID: 238
	internal static class UsbNotification
	{
		// Token: 0x06001778 RID: 6008 RVA: 0x0008ED10 File Offset: 0x0008CF10
		public static void RegisterUsbDeviceNotification(IntPtr windowHandle)
		{
			UsbNotification.DevBroadcastDeviceinterface devBroadcastDeviceinterface = new UsbNotification.DevBroadcastDeviceinterface
			{
				DeviceType = 5,
				Reserved = 0,
				ClassGuid = UsbNotification.GuidDevinterfaceUSBDevice,
				Name = 0
			};
			devBroadcastDeviceinterface.Size = Marshal.SizeOf<UsbNotification.DevBroadcastDeviceinterface>(devBroadcastDeviceinterface);
			IntPtr intPtr = Marshal.AllocHGlobal(devBroadcastDeviceinterface.Size);
			Marshal.StructureToPtr<UsbNotification.DevBroadcastDeviceinterface>(devBroadcastDeviceinterface, intPtr, true);
			UsbNotification.notificationHandle = UsbNotification.RegisterDeviceNotification(windowHandle, intPtr, 0);
		}

		// Token: 0x06001779 RID: 6009 RVA: 0x0008ED7B File Offset: 0x0008CF7B
		public static void UnregisterUsbDeviceNotification()
		{
			UsbNotification.UnregisterDeviceNotification(UsbNotification.notificationHandle);
		}

		// Token: 0x0600177A RID: 6010
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern IntPtr RegisterDeviceNotification(IntPtr recipient, IntPtr notificationFilter, int flags);

		// Token: 0x0600177B RID: 6011
		[DllImport("user32.dll")]
		private static extern bool UnregisterDeviceNotification(IntPtr handle);

		// Token: 0x14000073 RID: 115
		// (add) Token: 0x0600177C RID: 6012 RVA: 0x0008ED8C File Offset: 0x0008CF8C
		// (remove) Token: 0x0600177D RID: 6013 RVA: 0x0008EDC0 File Offset: 0x0008CFC0
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event UsbNotification.UsbDeviceChanged UsbDeviceAttached;

		// Token: 0x14000074 RID: 116
		// (add) Token: 0x0600177E RID: 6014 RVA: 0x0008EDF4 File Offset: 0x0008CFF4
		// (remove) Token: 0x0600177F RID: 6015 RVA: 0x0008EE28 File Offset: 0x0008D028
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event UsbNotification.UsbDeviceChanged UsbDeviceDetached;

		// Token: 0x06001780 RID: 6016 RVA: 0x0008EE5C File Offset: 0x0008D05C
		public static void OnUsbDeviceAttached()
		{
			bool flag = UsbNotification.UsbDeviceAttached != null;
			if (flag)
			{
				UsbNotification.UsbDeviceAttached();
			}
		}

		// Token: 0x06001781 RID: 6017 RVA: 0x0008EE84 File Offset: 0x0008D084
		public static void OnUsbDeviceDetached()
		{
			bool flag = UsbNotification.UsbDeviceDetached != null;
			if (flag)
			{
				UsbNotification.UsbDeviceDetached();
			}
		}

		// Token: 0x04000F73 RID: 3955
		public const int DbtDevicearrival = 32768;

		// Token: 0x04000F74 RID: 3956
		public const int DbtDeviceremovecomplete = 32772;

		// Token: 0x04000F75 RID: 3957
		public const int WmDevicechange = 537;

		// Token: 0x04000F76 RID: 3958
		private const int DbtDevtypDeviceinterface = 5;

		// Token: 0x04000F77 RID: 3959
		private static readonly Guid GuidDevinterfaceUSBDevice = new Guid("A5DCBF10-6530-11D2-901F-00C04FB951ED");

		// Token: 0x04000F78 RID: 3960
		private static IntPtr notificationHandle;

		// Token: 0x020002DE RID: 734
		private struct DevBroadcastDeviceinterface
		{
			// Token: 0x04002013 RID: 8211
			internal int Size;

			// Token: 0x04002014 RID: 8212
			internal int DeviceType;

			// Token: 0x04002015 RID: 8213
			internal int Reserved;

			// Token: 0x04002016 RID: 8214
			internal Guid ClassGuid;

			// Token: 0x04002017 RID: 8215
			internal short Name;
		}

		// Token: 0x020002DF RID: 735
		// (Invoke) Token: 0x060035C6 RID: 13766
		public delegate void UsbDeviceChanged();
	}
}
