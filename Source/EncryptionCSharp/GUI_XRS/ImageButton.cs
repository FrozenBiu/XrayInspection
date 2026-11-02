using System;
using System.Windows;
using System.Windows.Controls;

namespace GUI_XRS
{
	// Token: 0x02000248 RID: 584
	public class ImageButton : Button
	{
		// Token: 0x06002C5B RID: 11355 RVA: 0x001242C0 File Offset: 0x001224C0
		static ImageButton()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(ImageButton), new FrameworkPropertyMetadata(typeof(ImageButton)));
		}

		// Token: 0x17000BFC RID: 3068
		// (get) Token: 0x06002C5C RID: 11356 RVA: 0x00124338 File Offset: 0x00122538
		// (set) Token: 0x06002C5D RID: 11357 RVA: 0x0012435A File Offset: 0x0012255A
		public string ClickedImage
		{
			get
			{
				return (string)base.GetValue(ImageButton.ClickedImageProperty);
			}
			set
			{
				base.SetValue(ImageButton.ClickedImageProperty, value);
			}
		}

		// Token: 0x17000BFD RID: 3069
		// (get) Token: 0x06002C5E RID: 11358 RVA: 0x0012436C File Offset: 0x0012256C
		// (set) Token: 0x06002C5F RID: 11359 RVA: 0x0012438E File Offset: 0x0012258E
		public string NormalImage
		{
			get
			{
				return (string)base.GetValue(ImageButton.NormalImageProperty);
			}
			set
			{
				base.SetValue(ImageButton.NormalImageProperty, value);
			}
		}

		// Token: 0x04001BC7 RID: 7111
		public static readonly DependencyProperty ClickedImageProperty = DependencyProperty.Register("ClickedImage", typeof(string), typeof(ImageButton));

		// Token: 0x04001BC8 RID: 7112
		public static readonly DependencyProperty NormalImageProperty = DependencyProperty.Register("NormalImage", typeof(string), typeof(ImageButton));
	}
}
