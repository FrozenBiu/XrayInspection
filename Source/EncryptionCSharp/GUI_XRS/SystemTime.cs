using System;
using System.Runtime.InteropServices;

namespace GUI_XRS
{
	// Token: 0x0200015E RID: 350
	public static class SystemTime
	{
		// Token: 0x06002108 RID: 8456
		[DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
		internal static extern void GetLocalTime(SystemTime.NativeSystemTime systime);

		// Token: 0x06002109 RID: 8457
		[DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern int SetLocalTime(SystemTime.NativeSystemTime systime);

		// Token: 0x0600210A RID: 8458 RVA: 0x000D7408 File Offset: 0x000D5608
		public static void SetTime(DateTime dtTime)
		{
			SystemTime.NativeSystemTime nativeSystemTime = new SystemTime.NativeSystemTime();
			SystemTime.GetLocalTime(nativeSystemTime);
			checked
			{
				nativeSystemTime.wYear = (short)dtTime.Year;
				nativeSystemTime.wMonth = (short)dtTime.Month;
				nativeSystemTime.wDay = (short)dtTime.Day;
				nativeSystemTime.wHour = (short)dtTime.Hour;
				nativeSystemTime.wMinute = (short)dtTime.Minute;
				nativeSystemTime.wSecond = (short)dtTime.Second;
				nativeSystemTime.wMilliseconds = (short)dtTime.Millisecond;
				bool flag = SystemTime.SetLocalTime(nativeSystemTime) != 0;
				if (!flag)
				{
					int lastWin32Error = Marshal.GetLastWin32Error();
					bool flag2 = lastWin32Error == 87;
					if (flag2)
					{
					}
				}
			}
		}

		// Token: 0x0200032A RID: 810
		[StructLayout(LayoutKind.Sequential)]
		internal sealed class NativeSystemTime
		{
			// Token: 0x0400210E RID: 8462
			public short wYear;

			// Token: 0x0400210F RID: 8463
			public short wMonth;

			// Token: 0x04002110 RID: 8464
			public short wDayOfWeek;

			// Token: 0x04002111 RID: 8465
			public short wDay;

			// Token: 0x04002112 RID: 8466
			public short wHour;

			// Token: 0x04002113 RID: 8467
			public short wMinute;

			// Token: 0x04002114 RID: 8468
			public short wSecond;

			// Token: 0x04002115 RID: 8469
			public short wMilliseconds;
		}
	}
}
