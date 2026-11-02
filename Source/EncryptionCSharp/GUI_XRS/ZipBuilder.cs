using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Ionic.Zip;
using Ionic.Zlib;

namespace GUI_XRS
{
	// Token: 0x02000038 RID: 56
	internal class ZipBuilder
	{
		// Token: 0x14000023 RID: 35
		// (add) Token: 0x06000637 RID: 1591 RVA: 0x0002A218 File Offset: 0x00028418
		// (remove) Token: 0x06000638 RID: 1592 RVA: 0x0002A24C File Offset: 0x0002844C
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event EventHandler<ZipWriteProgressEventArgs> FileWritten;

		// Token: 0x06000639 RID: 1593 RVA: 0x0002A280 File Offset: 0x00028480
		public static void CreateZipFromList(string zipFilePath, string baseDirectory, IEnumerable<string> filePaths, string password = null)
		{
			ZipBuilder.zipCompletedSource = new TaskCompletionSource<bool>();
			using (ZipFile zipFile = new ZipFile())
			{
				zipFile.UseZip64WhenSaving = Zip64Option.AsNecessary;
				zipFile.ParallelDeflateThreshold = 262144L;
				bool flag = !string.IsNullOrEmpty(password);
				if (flag)
				{
					zipFile.Password = password;
				}
				zipFile.SaveProgress += ZipBuilder.Zip_SaveProgress;
				foreach (string text in filePaths)
				{
					bool flag2 = !File.Exists(text);
					if (!flag2)
					{
						string fileName = ZipBuilder.GetRelativePath(baseDirectory, text).Replace("\\", "/");
						string a = Path.GetExtension(text).ToLowerInvariant();
						bool flag3 = a == ".engine" || a == ".weights" || a == ".onnx";
						ZipEntry zipEntry = zipFile.AddFile(text);
						zipEntry.FileName = fileName;
						zipEntry.CompressionLevel = (flag3 ? CompressionLevel.None : CompressionLevel.BestSpeed);
					}
				}
				zipFile.Save(zipFilePath);
			}
		}

		// Token: 0x0600063A RID: 1594 RVA: 0x0002A3C4 File Offset: 0x000285C4
		public static string GetRelativePath(string baseDir, string fullPath)
		{
			Uri uri = new Uri(ZipBuilder.AppendDirectorySeparatorChar(baseDir));
			Uri uri2 = new Uri(fullPath);
			Uri uri3 = uri.MakeRelativeUri(uri2);
			string text = Uri.UnescapeDataString(uri3.ToString());
			return text.Replace('/', Path.DirectorySeparatorChar);
		}

		// Token: 0x0600063B RID: 1595 RVA: 0x0002A40C File Offset: 0x0002860C
		private static string AppendDirectorySeparatorChar(string path)
		{
			bool flag = !path.EndsWith(Path.DirectorySeparatorChar.ToString());
			string result;
			if (flag)
			{
				result = path + Path.DirectorySeparatorChar.ToString();
			}
			else
			{
				result = path;
			}
			return result;
		}

		// Token: 0x0600063C RID: 1596 RVA: 0x0002A450 File Offset: 0x00028650
		private static void Zip_SaveProgress(object sender, SaveProgressEventArgs e)
		{
			bool flag = e.EventType == ZipProgressEventType.Saving_AfterWriteEntry;
			if (flag)
			{
				EventHandler<ZipWriteProgressEventArgs> fileWritten = ZipBuilder.FileWritten;
				if (fileWritten != null)
				{
					fileWritten(null, new ZipWriteProgressEventArgs
					{
						FileName = e.CurrentEntry.FileName,
						BytesWritten = e.CurrentEntry.UncompressedSize
					});
				}
			}
			else
			{
				bool flag2 = e.EventType == ZipProgressEventType.Saving_Completed;
				if (flag2)
				{
					bool flag3 = ZipBuilder.zipCompletedSource != null;
					if (flag3)
					{
						ZipBuilder.zipCompletedSource.SetResult(true);
					}
				}
			}
		}

		// Token: 0x0600063D RID: 1597 RVA: 0x0002A4D4 File Offset: 0x000286D4
		[DebuggerStepThrough]
		public static Task<bool> WaitForZipCompletedAsync(int timeoutMilliseconds)
		{
			ZipBuilder.<WaitForZipCompletedAsync>d__8 <WaitForZipCompletedAsync>d__ = new ZipBuilder.<WaitForZipCompletedAsync>d__8();
			<WaitForZipCompletedAsync>d__.<>t__builder = AsyncTaskMethodBuilder<bool>.Create();
			<WaitForZipCompletedAsync>d__.timeoutMilliseconds = timeoutMilliseconds;
			<WaitForZipCompletedAsync>d__.<>1__state = -1;
			<WaitForZipCompletedAsync>d__.<>t__builder.Start<ZipBuilder.<WaitForZipCompletedAsync>d__8>(ref <WaitForZipCompletedAsync>d__);
			return <WaitForZipCompletedAsync>d__.<>t__builder.Task;
		}

		// Token: 0x0400047E RID: 1150
		private static TaskCompletionSource<bool> zipCompletedSource;
	}
}
