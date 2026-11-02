using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace GUI_XRS
{
	// Token: 0x02000237 RID: 567
	public static class WorkingArea
	{
		// Token: 0x06002B43 RID: 11075 RVA: 0x0011EA3D File Offset: 0x0011CC3D
		public static void SetWdToWorkingArea(Window window)
		{
			window.SourceInitialized += WorkingArea.win_SourceInitialized;
		}

		// Token: 0x06002B44 RID: 11076 RVA: 0x0011EA54 File Offset: 0x0011CC54
		public static void ClearResources(Window window)
		{
			window.SourceInitialized -= WorkingArea.win_SourceInitialized;
			IntPtr handle = new WindowInteropHelper(window).Handle;
			HwndSource.FromHwnd(handle).RemoveHook(new HwndSourceHook(WorkingArea.WindowProc));
		}

		// Token: 0x06002B45 RID: 11077 RVA: 0x0011EA9C File Offset: 0x0011CC9C
		private static void win_SourceInitialized(object sender, EventArgs e)
		{
			IntPtr handle = new WindowInteropHelper((Window)sender).Handle;
			HwndSource.FromHwnd(handle).AddHook(new HwndSourceHook(WorkingArea.WindowProc));
		}

		// Token: 0x06002B46 RID: 11078 RVA: 0x0011EAD4 File Offset: 0x0011CCD4
		private static IntPtr WindowProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			if (msg == 36)
			{
				WorkingArea.WmGetMinMaxInfo(hwnd, lParam);
				handled = true;
			}
			return (IntPtr)0;
		}

		// Token: 0x06002B47 RID: 11079 RVA: 0x0011EB08 File Offset: 0x0011CD08
		private static void WmGetMinMaxInfo(IntPtr hwnd, IntPtr lParam)
		{
			APIStructDefinitions.MINMAXINFO structure = (APIStructDefinitions.MINMAXINFO)Marshal.PtrToStructure(lParam, typeof(APIStructDefinitions.MINMAXINFO));
			int flags = 2;
			IntPtr intPtr = APIStructDefinitions.MonitorFromWindow(hwnd, flags);
			bool flag = intPtr != IntPtr.Zero;
			if (flag)
			{
				APIStructDefinitions.MONITORINFO monitorinfo = new APIStructDefinitions.MONITORINFO();
				APIStructDefinitions.GetMonitorInfo(intPtr, monitorinfo);
				APIStructDefinitions.RECT rcWork = monitorinfo.rcWork;
				APIStructDefinitions.RECT rcMonitor = monitorinfo.rcMonitor;
				structure.ptMaxPosition.x = Math.Abs(rcWork.left - rcMonitor.left);
				structure.ptMaxPosition.y = Math.Abs(rcWork.top - rcMonitor.top);
				structure.ptMaxSize.x = Math.Abs(rcWork.right - rcWork.left);
				structure.ptMaxSize.y = Math.Abs(rcWork.bottom - rcWork.top);
			}
			Marshal.StructureToPtr<APIStructDefinitions.MINMAXINFO>(structure, lParam, true);
		}
	}
}
