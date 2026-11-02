using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace GUI_XRS
{
	// Token: 0x02000031 RID: 49
	public class BackupInfoFileParser
	{
		// Token: 0x06000604 RID: 1540 RVA: 0x000291BB File Offset: 0x000273BB
		public BackupInfoFileParser(string fileName)
		{
			this.fileName = fileName;
		}

		// Token: 0x06000605 RID: 1541 RVA: 0x000291CC File Offset: 0x000273CC
		public List<BackupInfo> ParseInfoFile(bool serviceMode)
		{
			List<BackupInfo> list = new List<BackupInfo>();
			bool flag = File.Exists(this.fileName);
			if (flag)
			{
				StreamReader streamReader = new StreamReader(this.fileName);
				while (!streamReader.EndOfStream)
				{
					string text = streamReader.ReadLine();
					string[] array = text.Split(new char[]
					{
						'='
					});
					if (serviceMode)
					{
						bool flag2 = array.Length == 2;
						if (flag2)
						{
							list.Add(new BackupInfo(array[0].Trim(), array[1].Trim()));
						}
						bool flag3 = array.Length == 1;
						if (flag3)
						{
							list.Add(new BackupInfo("", array[0].Trim()));
						}
					}
					else
					{
						bool flag4 = array.Length == 2;
						if (flag4)
						{
							bool flag5 = array[0].Trim() + "Value" == MainWindowViewModel.guiVersionPath || array[0].Trim() + "Value" == MainWindowViewModel.guiVersionBuildPath || array[0].Trim() + "Value" == MainWindowViewModel.elinosVersionPath || array[0].Trim() == "SerialNumber" || array[0].Trim() == "LineName" || array[0].Trim() == "DeviceName";
							if (flag5)
							{
								list.Add(new BackupInfo(array[0].Trim(), array[1].Trim()));
							}
						}
					}
				}
				streamReader.Close();
				File.Delete(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\" + Backup.infoFileName);
			}
			return list;
		}

		// Token: 0x06000606 RID: 1542 RVA: 0x00029394 File Offset: 0x00027594
		public BackupInfo GetGUIVersion()
		{
			BackupInfo result = null;
			bool flag = File.Exists(this.fileName);
			if (flag)
			{
				StreamReader streamReader = new StreamReader(this.fileName);
				while (!streamReader.EndOfStream)
				{
					string text = streamReader.ReadLine();
					string[] array = text.Split(new char[]
					{
						'='
					});
					bool flag2 = array.Length == 2;
					if (flag2)
					{
						bool flag3 = array[0].Trim() + "Value" == MainWindowViewModel.guiVersionPath;
						if (flag3)
						{
							result = new BackupInfo(array[0].Trim(), array[1].Trim());
						}
					}
				}
				streamReader.Close();
				File.Delete(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\" + Backup.infoFileName);
			}
			return result;
		}

		// Token: 0x06000607 RID: 1543 RVA: 0x0002946C File Offset: 0x0002766C
		public List<BackupInfo> GetValuesFromInfoTxt(List<string> paths)
		{
			List<BackupInfo> list = new List<BackupInfo>(new BackupInfo[paths.Count]);
			List<BackupInfo> result;
			try
			{
				bool flag = File.Exists(this.fileName);
				if (flag)
				{
					using (StreamReader streamReader = new StreamReader(this.fileName))
					{
						Dictionary<string, string> dictionary = new Dictionary<string, string>();
						while (!streamReader.EndOfStream)
						{
							string text = streamReader.ReadLine();
							string[] array = text.Split(new char[]
							{
								'='
							}, 2);
							bool flag2 = array.Length == 2;
							if (flag2)
							{
								dictionary[array[0].Trim()] = array[1].Trim();
							}
						}
						for (int i = 0; i < paths.Count; i++)
						{
							string text2 = paths[i].Replace("Value", "").Trim();
							string infoValue;
							bool flag3 = dictionary.TryGetValue(text2, out infoValue);
							if (flag3)
							{
								list[i] = new BackupInfo(text2, infoValue);
							}
							else
							{
								list[i] = new BackupInfo(text2, null);
							}
						}
					}
					File.Delete(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\" + Backup.infoFileName);
				}
				result = list;
			}
			catch (FormatException)
			{
				result = list;
			}
			return result;
		}

		// Token: 0x04000467 RID: 1127
		private string fileName;
	}
}
