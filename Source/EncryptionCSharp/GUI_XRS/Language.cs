using System;
using System.Windows.Media.Imaging;
using Definitions;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x02000222 RID: 546
	public class Language
	{
		// Token: 0x17000B2D RID: 2861
		// (get) Token: 0x060028FA RID: 10490 RVA: 0x00110CD4 File Offset: 0x0010EED4
		// (set) Token: 0x060028FB RID: 10491 RVA: 0x00110CDC File Offset: 0x0010EEDC
		public BitmapSource Flag { get; set; }

		// Token: 0x17000B2E RID: 2862
		// (get) Token: 0x060028FC RID: 10492 RVA: 0x00110CE5 File Offset: 0x0010EEE5
		// (set) Token: 0x060028FD RID: 10493 RVA: 0x00110CED File Offset: 0x0010EEED
		public string CountryCode { get; set; }

		// Token: 0x17000B2F RID: 2863
		// (get) Token: 0x060028FE RID: 10494 RVA: 0x00110CF6 File Offset: 0x0010EEF6
		// (set) Token: 0x060028FF RID: 10495 RVA: 0x00110CFE File Offset: 0x0010EEFE
		public Languages ChosenLanguage { get; set; }

		// Token: 0x06002900 RID: 10496 RVA: 0x00110D07 File Offset: 0x0010EF07
		public Language(string flagPath, string countryCode, Languages language)
		{
			this.flagPath = flagPath;
			this.CountryCode = countryCode;
			this.ChosenLanguage = language;
			this.Flag = ImageConverter.ToBitmap(flagPath);
		}

		// Token: 0x04001985 RID: 6533
		private string flagPath;
	}
}
