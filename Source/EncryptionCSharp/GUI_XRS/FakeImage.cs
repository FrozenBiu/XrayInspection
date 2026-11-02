using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GUI_XRS
{
	// Token: 0x02000183 RID: 387
	public class FakeImage : Canvas
	{
		// Token: 0x060022F9 RID: 8953 RVA: 0x000E1578 File Offset: 0x000DF778
		public FakeImage(string url)
		{
			this.m_bmp = new BitmapImage();
			this.m_brush = new ImageBrush();
			base.Width = 256.0;
			base.Height = 256.0;
			this.Reset(url);
		}

		// Token: 0x060022FA RID: 8954 RVA: 0x000E15DC File Offset: 0x000DF7DC
		public void Reset(string url)
		{
			this.m_bmp.BeginInit();
			this.m_bmp.CacheOption = BitmapCacheOption.OnLoad;
			this.m_bmp.UriSource = new Uri(url, UriKind.Absolute);
			this.m_bmp.EndInit();
			this.m_brush.ImageSource = this.m_bmp;
			base.Background = this.m_brush;
		}

		// Token: 0x040016AF RID: 5807
		private ImageBrush m_brush = null;

		// Token: 0x040016B0 RID: 5808
		private BitmapImage m_bmp = null;
	}
}
