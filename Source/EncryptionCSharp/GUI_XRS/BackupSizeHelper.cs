using System;
using System.Collections.Generic;
using System.IO;

namespace GUI_XRS
{
	// Token: 0x02000033 RID: 51
	internal class BackupSizeHelper
	{
		// Token: 0x06000616 RID: 1558 RVA: 0x00029780 File Offset: 0x00027980
		public void CalculateFileFilterSize(string directoryPath, string fileFilter, ref long totalSize, ref int fileNumber, ref List<string> filesToBackup)
		{
			string[] array = fileFilter.Split(new char[]
			{
				';'
			});
			this.positiveFilters = Array.FindAll<string>(array, (string f) => f.StartsWith("+"));
			this.negativeFilters = Array.FindAll<string>(array, (string f) => f.StartsWith("-"));
			this.positiveFilters = this.RemovePrefix(this.positiveFilters, "+");
			this.positiveFilters = this.RemovePrefix(this.positiveFilters, ".*");
			this.positiveFilters = this.RemovePostfix(this.positiveFilters, "*");
			this.negativeFilters = this.RemovePrefix(this.negativeFilters, "-");
			this.negativeFilters = this.RemovePrefix(this.negativeFilters, ".*");
			this.negativeFilters = this.RemovePostfix(this.negativeFilters, "*");
			this.negativeFilters = this.RemovePostfix(this.negativeFilters, "$");
			totalSize = 0L;
			fileNumber = 0;
			foreach (string text in Directory.GetFiles(directoryPath, "*", SearchOption.AllDirectories))
			{
				string filePath = text.Replace("\\", "\\\\");
				bool flag = this.MatchesFilter(filePath);
				if (flag)
				{
					FileInfo fileInfo = new FileInfo(text);
					totalSize += fileInfo.Length;
					fileNumber++;
					filesToBackup.Add(text);
				}
			}
		}

		// Token: 0x06000617 RID: 1559 RVA: 0x0002990C File Offset: 0x00027B0C
		private string[] RemovePrefix(string[] filters, string prefix)
		{
			for (int i = 0; i < filters.Length; i++)
			{
				bool flag = filters[i].StartsWith(prefix);
				if (flag)
				{
					filters[i] = filters[i].Substring(prefix.Length);
				}
			}
			return filters;
		}

		// Token: 0x06000618 RID: 1560 RVA: 0x00029954 File Offset: 0x00027B54
		private string[] RemovePostfix(string[] filters, string postfix)
		{
			for (int i = 0; i < filters.Length; i++)
			{
				bool flag = filters[i].EndsWith(postfix);
				if (flag)
				{
					filters[i] = filters[i].Substring(0, filters[i].Length - postfix.Length);
				}
			}
			return filters;
		}

		// Token: 0x06000619 RID: 1561 RVA: 0x000299A8 File Offset: 0x00027BA8
		private bool MatchesFilter(string filePath)
		{
			foreach (string value in this.negativeFilters)
			{
				bool flag = filePath.Contains(value);
				if (flag)
				{
					return false;
				}
			}
			foreach (string value2 in this.positiveFilters)
			{
				bool flag2 = filePath.Contains(value2);
				if (flag2)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600061A RID: 1562 RVA: 0x00029A24 File Offset: 0x00027C24
		public static long GetDirectorySize(string folderPath)
		{
			long num = 0L;
			try
			{
				foreach (string fileName in Directory.GetFiles(folderPath, "*", SearchOption.AllDirectories))
				{
					FileInfo fileInfo = new FileInfo(fileName);
					num += fileInfo.Length;
				}
			}
			catch (Exception ex)
			{
			}
			return num;
		}

		// Token: 0x0400046D RID: 1133
		private string[] positiveFilters;

		// Token: 0x0400046E RID: 1134
		private string[] negativeFilters;
	}
}
