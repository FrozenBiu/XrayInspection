using System;
using System.Collections.Generic;

namespace GUI_XRS.Ftp
{
	// Token: 0x0200025D RID: 605
	public class FTPdirectory : List<FTPfileInfo>
	{
		// Token: 0x06002EA9 RID: 11945 RVA: 0x00132961 File Offset: 0x00130B61
		public FTPdirectory()
		{
		}

		// Token: 0x06002EAA RID: 11946 RVA: 0x0013296C File Offset: 0x00130B6C
		public FTPdirectory(string dir, string path)
		{
			foreach (string text in dir.Replace("\n", "").Split(new char[]
			{
				Convert.ToChar('\r')
			}))
			{
				bool flag = text != "";
				if (flag)
				{
					base.Add(new FTPfileInfo(text, path));
				}
			}
		}

		// Token: 0x06002EAB RID: 11947 RVA: 0x001329DC File Offset: 0x00130BDC
		public FTPdirectory GetFiles(string ext)
		{
			return this.GetFileOrDir(FTPfileInfo.DirectoryEntryTypes.File, ext);
		}

		// Token: 0x06002EAC RID: 11948 RVA: 0x001329F8 File Offset: 0x00130BF8
		public FTPdirectory GetDirectories()
		{
			return this.GetFileOrDir(FTPfileInfo.DirectoryEntryTypes.Directory, "");
		}

		// Token: 0x06002EAD RID: 11949 RVA: 0x00132A18 File Offset: 0x00130C18
		private FTPdirectory GetFileOrDir(FTPfileInfo.DirectoryEntryTypes type, string ext)
		{
			FTPdirectory ftpdirectory = new FTPdirectory();
			foreach (FTPfileInfo ftpfileInfo in this)
			{
				bool flag = ftpfileInfo.FileType == type;
				if (flag)
				{
					bool flag2 = ext == "";
					if (flag2)
					{
						ftpdirectory.Add(ftpfileInfo);
					}
					else
					{
						bool flag3 = ext == ftpfileInfo.Extension;
						if (flag3)
						{
							ftpdirectory.Add(ftpfileInfo);
						}
					}
				}
			}
			return ftpdirectory;
		}

		// Token: 0x06002EAE RID: 11950 RVA: 0x00132ABC File Offset: 0x00130CBC
		public bool FileExists(string filename)
		{
			foreach (FTPfileInfo ftpfileInfo in this)
			{
				bool flag = ftpfileInfo.Filename == filename;
				if (flag)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002EAF RID: 11951 RVA: 0x00132B24 File Offset: 0x00130D24
		public static string GetParentDirectory(string dir)
		{
			string text = dir.TrimEnd(new char[]
			{
				'/'
			});
			int num = text.LastIndexOf('/');
			bool flag = num > 0;
			if (flag)
			{
				return text.Substring(0, num - 1);
			}
			throw new ApplicationException("No parent for root");
		}

		// Token: 0x04001D50 RID: 7504
		private const char slash = '/';
	}
}
