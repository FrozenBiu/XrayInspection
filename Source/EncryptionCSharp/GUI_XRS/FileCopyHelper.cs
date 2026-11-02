using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace GUI_XRS
{
	// Token: 0x02000036 RID: 54
	internal class FileCopyHelper
	{
		// Token: 0x0600062F RID: 1583
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern bool CopyFileEx(string lpExistingFileName, string lpNewFileName, FileCopyHelper.CopyProgressRoutine lpProgressRoutine, IntPtr lpData, ref bool pbCancel, FileCopyHelper.CopyFileFlags dwCopyFlags);

		// Token: 0x06000630 RID: 1584 RVA: 0x0002A19C File Offset: 0x0002839C
		public static void CopyWithProgress(string source, string destination, BackgroundWorker worker)
		{
			bool flag = false;
			FileCopyHelper.CopyProgressRoutine lpProgressRoutine = delegate(long total, long transferred, long _, long __, uint ___, FileCopyHelper.CopyProgressCallbackReason reason, IntPtr ____, IntPtr _____, IntPtr ______)
			{
				bool flag4 = reason == FileCopyHelper.CopyProgressCallbackReason.CALLBACK_CHUNK_FINISHED && total > 0L;
				if (flag4)
				{
					int percentProgress = (int)(transferred * 100L / total);
					worker.ReportProgress(percentProgress, 1);
				}
				return FileCopyHelper.CopyProgressResult.PROGRESS_CONTINUE;
			};
			bool flag2 = FileCopyHelper.CopyFileEx(source, destination, lpProgressRoutine, IntPtr.Zero, ref flag, FileCopyHelper.CopyFileFlags.COPY_FILE_RESTARTABLE);
			bool flag3 = !flag2;
			if (flag3)
			{
				throw new Win32Exception(Marshal.GetLastWin32Error());
			}
		}

		// Token: 0x020002A8 RID: 680
		// (Invoke) Token: 0x0600353A RID: 13626
		private delegate FileCopyHelper.CopyProgressResult CopyProgressRoutine(long totalFileSize, long totalBytesTransferred, long streamSize, long streamBytesTransferred, uint dwStreamNumber, FileCopyHelper.CopyProgressCallbackReason dwCallbackReason, IntPtr hSourceFile, IntPtr hDestinationFile, IntPtr lpData);

		// Token: 0x020002A9 RID: 681
		private enum CopyProgressResult : uint
		{
			// Token: 0x04001F7E RID: 8062
			PROGRESS_CONTINUE,
			// Token: 0x04001F7F RID: 8063
			PROGRESS_CANCEL,
			// Token: 0x04001F80 RID: 8064
			PROGRESS_STOP,
			// Token: 0x04001F81 RID: 8065
			PROGRESS_QUIET
		}

		// Token: 0x020002AA RID: 682
		private enum CopyProgressCallbackReason : uint
		{
			// Token: 0x04001F83 RID: 8067
			CALLBACK_CHUNK_FINISHED,
			// Token: 0x04001F84 RID: 8068
			CALLBACK_STREAM_SWITCH
		}

		// Token: 0x020002AB RID: 683
		[Flags]
		private enum CopyFileFlags : uint
		{
			// Token: 0x04001F86 RID: 8070
			COPY_FILE_RESTARTABLE = 2U
		}
	}
}
