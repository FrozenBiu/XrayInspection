using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x020000B8 RID: 184
	public class HistoryHelper
	{
		// Token: 0x06001255 RID: 4693 RVA: 0x0006E26C File Offset: 0x0006C46C
		public static XRSImage GetLatestImage(StoreImageData imageArchiver)
		{
			List<string> list = new List<string>();
			XRSImage result = null;
			bool flag = Directory.Exists(StoreImageData.archiveDirectoryThroughput);
			if (flag)
			{
				list.AddRange(Directory.GetFiles(StoreImageData.archiveDirectoryThroughput).ToList<string>());
			}
			bool flag2 = Directory.Exists(StoreImageData.archiveDirectoryReject);
			if (flag2)
			{
				list.AddRange(Directory.GetFiles(StoreImageData.archiveDirectoryReject).ToList<string>());
			}
			list.Sort(new NamesComparerInverse());
			foreach (string path in list)
			{
				XRSImage image = imageArchiver.GetImage(path);
				bool flag3 = image.Type == ImageType.Normal || image.Type == ImageType.Demo || image.Type == ImageType.Audit || image.Type == ImageType.IVP;
				if (flag3)
				{
					result = image;
					break;
				}
			}
			return result;
		}

		// Token: 0x06001256 RID: 4694 RVA: 0x0006E360 File Offset: 0x0006C560
		public static XRSImage GetLatestImage(int imageGroup, StoreImageData imageArchiver)
		{
			List<string> list = new List<string>();
			XRSImage result = null;
			bool flag = Directory.Exists(StoreImageData.archiveDirectoryThroughput);
			if (flag)
			{
				list.AddRange(Directory.GetFiles(StoreImageData.archiveDirectoryThroughput).ToList<string>());
			}
			bool flag2 = Directory.Exists(StoreImageData.archiveDirectoryReject);
			if (flag2)
			{
				list.AddRange(Directory.GetFiles(StoreImageData.archiveDirectoryReject).ToList<string>());
			}
			list.Sort(new NamesComparerInverse());
			foreach (string path in list)
			{
				XRSImage image = imageArchiver.GetImage(path);
				bool flag3 = (int)image.Group == imageGroup && (image.Type == ImageType.Normal || image.Type == ImageType.Demo || image.Type == ImageType.Audit || image.Type == ImageType.IVP);
				if (flag3)
				{
					result = image;
					break;
				}
			}
			return result;
		}

		// Token: 0x06001257 RID: 4695 RVA: 0x0006E460 File Offset: 0x0006C660
		public static string GetLatestImageName(ref bool isThroughput)
		{
			List<string> list = new List<string>();
			string result = "";
			string[] separator = new string[]
			{
				"\\"
			};
			bool flag = Directory.Exists(StoreImageData.archiveDirectoryThroughput);
			if (flag)
			{
				list.AddRange(Directory.GetFiles(StoreImageData.archiveDirectoryThroughput).ToList<string>());
			}
			bool flag2 = Directory.Exists(StoreImageData.archiveDirectoryReject);
			if (flag2)
			{
				list.AddRange(Directory.GetFiles(StoreImageData.archiveDirectoryReject).ToList<string>());
			}
			bool flag3 = list.Count > 0;
			if (flag3)
			{
				list.Sort(new NamesComparerInverse());
				string[] array = list[0].Split(separator, 100, StringSplitOptions.None);
				result = array[array.Length - 1];
				bool flag4 = list[0].Contains("Throughput");
				if (flag4)
				{
					isThroughput = true;
				}
				else
				{
					isThroughput = false;
				}
			}
			return result;
		}

		// Token: 0x06001258 RID: 4696 RVA: 0x0006E534 File Offset: 0x0006C734
		public static bool CheckIfImageIsAvailable(int imageGroup, StoreImageData imageArchiver)
		{
			List<string> list = new List<string>();
			bool result = false;
			bool flag = Directory.Exists(StoreImageData.archiveDirectoryThroughput);
			if (flag)
			{
				list.AddRange(Directory.GetFiles(StoreImageData.archiveDirectoryThroughput).ToList<string>());
			}
			bool flag2 = Directory.Exists(StoreImageData.archiveDirectoryReject);
			if (flag2)
			{
				list.AddRange(Directory.GetFiles(StoreImageData.archiveDirectoryReject).ToList<string>());
			}
			list.Sort(new NamesComparerInverse());
			foreach (string path in list)
			{
				XRSImage image = imageArchiver.GetImage(path);
				bool flag3 = (int)image.Group == imageGroup && (image.Type == ImageType.Normal || image.Type == ImageType.Demo || image.Type == ImageType.Audit || image.Type == ImageType.IVP);
				if (flag3)
				{
					result = true;
					break;
				}
			}
			return result;
		}
	}
}
