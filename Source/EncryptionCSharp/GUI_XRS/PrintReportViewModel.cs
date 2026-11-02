using System;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Media.Imaging;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x02000137 RID: 311
	public class PrintReportViewModel
	{
		// Token: 0x170008B7 RID: 2231
		// (get) Token: 0x06001DE2 RID: 7650 RVA: 0x000C29D6 File Offset: 0x000C0BD6
		// (set) Token: 0x06001DE3 RID: 7651 RVA: 0x000C29DE File Offset: 0x000C0BDE
		public string ProductName { get; set; }

		// Token: 0x170008B8 RID: 2232
		// (get) Token: 0x06001DE4 RID: 7652 RVA: 0x000C29E7 File Offset: 0x000C0BE7
		// (set) Token: 0x06001DE5 RID: 7653 RVA: 0x000C29EF File Offset: 0x000C0BEF
		public BitmapSource ProductImage { get; set; }

		// Token: 0x170008B9 RID: 2233
		// (get) Token: 0x06001DE6 RID: 7654 RVA: 0x000C29F8 File Offset: 0x000C0BF8
		// (set) Token: 0x06001DE7 RID: 7655 RVA: 0x000C2A00 File Offset: 0x000C0C00
		public BitmapSource Logo { get; set; }

		// Token: 0x170008BA RID: 2234
		// (get) Token: 0x06001DE8 RID: 7656 RVA: 0x000C2A09 File Offset: 0x000C0C09
		// (set) Token: 0x06001DE9 RID: 7657 RVA: 0x000C2A11 File Offset: 0x000C0C11
		public bool LogoAvailable { get; set; }

		// Token: 0x170008BB RID: 2235
		// (get) Token: 0x06001DEA RID: 7658 RVA: 0x000C2A1A File Offset: 0x000C0C1A
		// (set) Token: 0x06001DEB RID: 7659 RVA: 0x000C2A22 File Offset: 0x000C0C22
		public Thickness BorderThickness { get; set; }

		// Token: 0x06001DEC RID: 7660 RVA: 0x000C2A2C File Offset: 0x000C0C2C
		public PrintReportViewModel(string currentProductName, BitmapSource productImage)
		{
			this.ProductName = currentProductName;
			this.ProductImage = productImage;
			this.LogoAvailable = File.Exists(this.logoFile);
			bool logoAvailable = this.LogoAvailable;
			if (logoAvailable)
			{
				this.Logo = ImageConverter.ToBitmap(this.logoFile, false);
				this.BorderThickness = new Thickness(1.0);
			}
			else
			{
				this.BorderThickness = new Thickness(0.0);
			}
		}

		// Token: 0x040013A5 RID: 5029
		private readonly string logoFile = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Logo.png";
	}
}
