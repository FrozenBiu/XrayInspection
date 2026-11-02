using System;
using System.Text.RegularExpressions;

namespace GUI_XRS.Ftp
{
	// Token: 0x0200025F RID: 607
	public class FTPfileInfo
	{
		// Token: 0x17000C72 RID: 3186
		// (get) Token: 0x06002EB3 RID: 11955 RVA: 0x00132BB8 File Offset: 0x00130DB8
		public string FullName
		{
			get
			{
				return this.Path + this.Filename;
			}
		}

		// Token: 0x17000C73 RID: 3187
		// (get) Token: 0x06002EB4 RID: 11956 RVA: 0x00132BDC File Offset: 0x00130DDC
		public string Filename
		{
			get
			{
				return this._filename;
			}
		}

		// Token: 0x17000C74 RID: 3188
		// (get) Token: 0x06002EB5 RID: 11957 RVA: 0x00132BF4 File Offset: 0x00130DF4
		public string Path
		{
			get
			{
				return this._path;
			}
		}

		// Token: 0x17000C75 RID: 3189
		// (get) Token: 0x06002EB6 RID: 11958 RVA: 0x00132C0C File Offset: 0x00130E0C
		public FTPfileInfo.DirectoryEntryTypes FileType
		{
			get
			{
				return this._fileType;
			}
		}

		// Token: 0x17000C76 RID: 3190
		// (get) Token: 0x06002EB7 RID: 11959 RVA: 0x00132C24 File Offset: 0x00130E24
		public long Size
		{
			get
			{
				return this._size;
			}
		}

		// Token: 0x17000C77 RID: 3191
		// (get) Token: 0x06002EB8 RID: 11960 RVA: 0x00132C3C File Offset: 0x00130E3C
		public DateTime FileDateTime
		{
			get
			{
				return this._fileDateTime;
			}
		}

		// Token: 0x17000C78 RID: 3192
		// (get) Token: 0x06002EB9 RID: 11961 RVA: 0x00132C54 File Offset: 0x00130E54
		public string Permission
		{
			get
			{
				return this._permission;
			}
		}

		// Token: 0x17000C79 RID: 3193
		// (get) Token: 0x06002EBA RID: 11962 RVA: 0x00132C6C File Offset: 0x00130E6C
		public string Extension
		{
			get
			{
				int num = this.Filename.LastIndexOf(".");
				bool flag = num >= 0 && num < this.Filename.Length - 1;
				string result;
				if (flag)
				{
					result = this.Filename.Substring(num + 1);
				}
				else
				{
					result = "";
				}
				return result;
			}
		}

		// Token: 0x17000C7A RID: 3194
		// (get) Token: 0x06002EBB RID: 11963 RVA: 0x00132CC4 File Offset: 0x00130EC4
		public string NameOnly
		{
			get
			{
				int num = this.Filename.LastIndexOf(".");
				bool flag = num > 0;
				string result;
				if (flag)
				{
					result = this.Filename.Substring(0, num);
				}
				else
				{
					result = this.Filename;
				}
				return result;
			}
		}

		// Token: 0x06002EBC RID: 11964 RVA: 0x00132D08 File Offset: 0x00130F08
		public FTPfileInfo(string line, string path)
		{
			Match matchingRegex = this.GetMatchingRegex(line);
			bool flag = matchingRegex == null;
			if (flag)
			{
				throw new ApplicationException("Unable to parse line: " + line);
			}
			this._filename = matchingRegex.Groups["name"].Value;
			this._path = path;
			long.TryParse(matchingRegex.Groups["size"].Value, out this._size);
			this._permission = matchingRegex.Groups["permission"].Value;
			string value = matchingRegex.Groups["dir"].Value;
			bool flag2 = value != "" && value != "-";
			if (flag2)
			{
				this._fileType = FTPfileInfo.DirectoryEntryTypes.Directory;
			}
			else
			{
				this._fileType = FTPfileInfo.DirectoryEntryTypes.File;
			}
			try
			{
				this._fileDateTime = DateTime.Parse(matchingRegex.Groups["timestamp"].Value);
			}
			catch (Exception)
			{
				this._fileDateTime = Convert.ToDateTime(null);
			}
		}

		// Token: 0x06002EBD RID: 11965 RVA: 0x00132E30 File Offset: 0x00131030
		private Match GetMatchingRegex(string line)
		{
			for (int i = 0; i <= FTPfileInfo._ParseFormats.Length - 1; i++)
			{
				Regex regex = new Regex(FTPfileInfo._ParseFormats[i]);
				Match match = regex.Match(line);
				bool success = match.Success;
				if (success)
				{
					return match;
				}
			}
			return null;
		}

		// Token: 0x04001D53 RID: 7507
		private string _filename;

		// Token: 0x04001D54 RID: 7508
		private string _path;

		// Token: 0x04001D55 RID: 7509
		private FTPfileInfo.DirectoryEntryTypes _fileType;

		// Token: 0x04001D56 RID: 7510
		private long _size;

		// Token: 0x04001D57 RID: 7511
		private DateTime _fileDateTime;

		// Token: 0x04001D58 RID: 7512
		private string _permission;

		// Token: 0x04001D59 RID: 7513
		private static string[] _ParseFormats = new string[]
		{
			"(?<dir>[\\-d])(?<permission>([\\-r][\\-w][\\-xs]){3})\\s+\\d+\\s+\\w+\\s+\\w+\\s+(?<size>\\d+)\\s+(?<timestamp>\\w+\\s+\\d+\\s+\\d{4})\\s+(?<name>.+)",
			"(?<dir>[\\-d])(?<permission>([\\-r][\\-w][\\-xs]){3})\\s+\\d+\\s+\\d+\\s+(?<size>\\d+)\\s+(?<timestamp>\\w+\\s+\\d+\\s+\\d{4})\\s+(?<name>.+)",
			"(?<dir>[\\-d])(?<permission>([\\-r][\\-w][\\-xs]){3})\\s+\\d+\\s+\\d+\\s+(?<size>\\d+)\\s+(?<timestamp>\\w+\\s+\\d+\\s+\\d{1,2}:\\d{2})\\s+(?<name>.+)",
			"(?<dir>[\\-d])(?<permission>([\\-r][\\-w][\\-xs]){3})\\s+\\d+\\s+\\w+\\s+\\w+\\s+(?<size>\\d+)\\s+(?<timestamp>\\w+\\s+\\d+\\s+\\d{1,2}:\\d{2})\\s+(?<name>.+)",
			"(?<dir>[\\-d])(?<permission>([\\-r][\\-w][\\-xs]){3})(\\s+)(?<size>(\\d+))(\\s+)(?<ctbit>(\\w+\\s\\w+))(\\s+)(?<size2>(\\d+))\\s+(?<timestamp>\\w+\\s+\\d+\\s+\\d{2}:\\d{2})\\s+(?<name>.+)",
			"(?<timestamp>\\d{2}\\-\\d{2}\\-\\d{2}\\s+\\d{2}:\\d{2}[Aa|Pp][mM])\\s+(?<dir>\\<\\w+\\>){0,1}(?<size>\\d+){0,1}\\s+(?<name>.+)"
		};

		// Token: 0x02000395 RID: 917
		public enum DirectoryEntryTypes
		{
			// Token: 0x0400222F RID: 8751
			File,
			// Token: 0x04002230 RID: 8752
			Directory
		}
	}
}
