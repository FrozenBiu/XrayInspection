using System;
using System.Collections.Generic;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;

namespace GUI_XRS
{
	// Token: 0x02000039 RID: 57
	public class ZipHelper
	{
		// Token: 0x0600063F RID: 1599 RVA: 0x0002A518 File Offset: 0x00028718
		public static bool ZipContainsFile(string archievePath, string zippedFileName)
		{
			bool flag = false;
			ZipInputStream zipInputStream = new ZipInputStream(new FileStream(archievePath, FileMode.Open));
			zipInputStream.Password = Backup.zipPassword;
			bool result;
			try
			{
				ZipEntry nextEntry;
				while ((nextEntry = zipInputStream.GetNextEntry()) != null)
				{
					bool flag2 = nextEntry.Name == zippedFileName;
					if (flag2)
					{
						flag = true;
						break;
					}
				}
				zipInputStream.Close();
				result = flag;
			}
			catch
			{
				zipInputStream.Close();
				result = false;
			}
			return result;
		}

		// Token: 0x06000640 RID: 1600 RVA: 0x0002A598 File Offset: 0x00028798
		public static bool ZipContainsFolder(string archievePath, string folderName)
		{
			bool flag = false;
			bool flag2 = !folderName.EndsWith("/");
			if (flag2)
			{
				folderName += "/";
			}
			ZipInputStream zipInputStream = new ZipInputStream(new FileStream(archievePath, FileMode.Open));
			zipInputStream.Password = Backup.zipPassword;
			bool result;
			try
			{
				ZipEntry nextEntry;
				while ((nextEntry = zipInputStream.GetNextEntry()) != null)
				{
					bool flag3 = nextEntry.Name.StartsWith(folderName);
					if (flag3)
					{
						flag = true;
						break;
					}
				}
				zipInputStream.Close();
				result = flag;
			}
			catch
			{
				zipInputStream.Close();
				result = false;
			}
			return result;
		}

		// Token: 0x06000641 RID: 1601 RVA: 0x0002A63C File Offset: 0x0002883C
		public static bool ZipContainsFiles(string archivePath, List<string> zippedFileNames)
		{
			bool result;
			try
			{
				HashSet<string> hashSet = new HashSet<string>(zippedFileNames, StringComparer.OrdinalIgnoreCase);
				using (ZipFile zipFile = new ZipFile(File.OpenRead(archivePath)))
				{
					zipFile.Password = Backup.zipPassword;
					foreach (object obj in zipFile)
					{
						ZipEntry zipEntry = (ZipEntry)obj;
						bool flag = !zipEntry.IsDirectory && !zipEntry.Name.Contains("/") && !zipEntry.Name.Contains("\\");
						if (flag)
						{
							hashSet.Remove(zipEntry.Name);
							bool flag2 = hashSet.Count == 0;
							if (flag2)
							{
								return true;
							}
						}
					}
				}
				result = false;
			}
			catch
			{
				result = false;
			}
			return result;
		}
	}
}
