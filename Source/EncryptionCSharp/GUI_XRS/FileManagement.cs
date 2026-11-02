using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace GUI_XRS
{
	// Token: 0x020000A8 RID: 168
	public class FileManagement
	{
		// Token: 0x17000506 RID: 1286
		// (get) Token: 0x060010A0 RID: 4256 RVA: 0x00066D39 File Offset: 0x00064F39
		// (set) Token: 0x060010A1 RID: 4257 RVA: 0x00066D41 File Offset: 0x00064F41
		public ObservableCollection<string> ValidSourceFiles { get; set; }

		// Token: 0x17000507 RID: 1287
		// (get) Token: 0x060010A2 RID: 4258 RVA: 0x00066D4A File Offset: 0x00064F4A
		// (set) Token: 0x060010A3 RID: 4259 RVA: 0x00066D52 File Offset: 0x00064F52
		public ObservableCollection<string> ValidDestinationFiles { get; set; }

		// Token: 0x060010A4 RID: 4260 RVA: 0x00066D5B File Offset: 0x00064F5B
		public FileManagement() : this("", "", null)
		{
		}

		// Token: 0x060010A5 RID: 4261 RVA: 0x00066D70 File Offset: 0x00064F70
		public FileManagement(string sourceFolder, string destinationFolder, string[] fileEndings)
		{
			this.ValidSourceFiles = new ObservableCollection<string>();
			this.ValidDestinationFiles = new ObservableCollection<string>();
			this.sourceFolder = sourceFolder;
			this.destinationFolder = destinationFolder;
			this.fileEndings = fileEndings;
		}

		// Token: 0x060010A6 RID: 4262 RVA: 0x00066DA8 File Offset: 0x00064FA8
		private void GetValidFiles(bool source)
		{
			string path = source ? this.sourceFolder : this.destinationFolder;
			ObservableCollection<string> observableCollection = source ? this.ValidSourceFiles : this.ValidDestinationFiles;
			observableCollection.Clear();
			bool flag = Directory.Exists(path);
			if (flag)
			{
				string[] files = Directory.GetFiles(path);
				int j;
				int i;
				for (i = 0; i < files.Length; i = j + 1)
				{
					bool flag2 = this.fileEndings == null || (from fileEnding in this.fileEndings
					where files[i].EndsWith(fileEnding)
					select fileEnding).Count<string>() > 0;
					if (flag2)
					{
						observableCollection.Add(files[i]);
					}
					j = i;
				}
			}
		}

		// Token: 0x04000B17 RID: 2839
		private string sourceFolder;

		// Token: 0x04000B18 RID: 2840
		private string destinationFolder;

		// Token: 0x04000B19 RID: 2841
		private string[] fileEndings;
	}
}
