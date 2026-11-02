using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using Definitions;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x020000C6 RID: 198
	public class MsgBoxViewModel : INotifyPropertyChanged
	{
		// Token: 0x17000616 RID: 1558
		// (get) Token: 0x060013E1 RID: 5089 RVA: 0x00076164 File Offset: 0x00074364
		// (set) Token: 0x060013E2 RID: 5090 RVA: 0x0007617C File Offset: 0x0007437C
		public string Title
		{
			get
			{
				return this.title;
			}
			set
			{
				this.title = value;
				this.OnPropertyChanged("Title");
			}
		}

		// Token: 0x17000617 RID: 1559
		// (get) Token: 0x060013E3 RID: 5091 RVA: 0x00076194 File Offset: 0x00074394
		// (set) Token: 0x060013E4 RID: 5092 RVA: 0x000761AC File Offset: 0x000743AC
		public string Message
		{
			get
			{
				return this.message;
			}
			set
			{
				this.message = value;
				this.OnPropertyChanged("Message");
			}
		}

		// Token: 0x17000618 RID: 1560
		// (get) Token: 0x060013E5 RID: 5093 RVA: 0x000761C4 File Offset: 0x000743C4
		// (set) Token: 0x060013E6 RID: 5094 RVA: 0x000761DC File Offset: 0x000743DC
		public ImageSource ButtonImage
		{
			get
			{
				return this.buttonImage;
			}
			set
			{
				this.buttonImage = value;
				this.OnPropertyChanged("ButtonImage");
			}
		}

		// Token: 0x17000619 RID: 1561
		// (get) Token: 0x060013E7 RID: 5095 RVA: 0x000761F4 File Offset: 0x000743F4
		// (set) Token: 0x060013E8 RID: 5096 RVA: 0x0007620C File Offset: 0x0007440C
		public Style ButtonStyle
		{
			get
			{
				return this.buttonStyle;
			}
			set
			{
				this.buttonStyle = value;
				this.OnPropertyChanged("ButtonStyle");
			}
		}

		// Token: 0x14000064 RID: 100
		// (add) Token: 0x060013E9 RID: 5097 RVA: 0x00076224 File Offset: 0x00074424
		// (remove) Token: 0x060013EA RID: 5098 RVA: 0x0007625C File Offset: 0x0007445C
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x1700061A RID: 1562
		// (get) Token: 0x060013EB RID: 5099 RVA: 0x00076291 File Offset: 0x00074491
		// (set) Token: 0x060013EC RID: 5100 RVA: 0x00076299 File Offset: 0x00074499
		public WindowState WState { get; set; }

		// Token: 0x1700061B RID: 1563
		// (get) Token: 0x060013ED RID: 5101 RVA: 0x000762A4 File Offset: 0x000744A4
		// (set) Token: 0x060013EE RID: 5102 RVA: 0x0003C95F File Offset: 0x0003AB5F
		public double WindowWidth
		{
			get
			{
				return Definition.windowWidth;
			}
			set
			{
				Definition.windowWidth = value;
			}
		}

		// Token: 0x1700061C RID: 1564
		// (get) Token: 0x060013EF RID: 5103 RVA: 0x000762BC File Offset: 0x000744BC
		// (set) Token: 0x060013F0 RID: 5104 RVA: 0x0003C97F File Offset: 0x0003AB7F
		public double WindowHeight
		{
			get
			{
				return Definition.windowHeight;
			}
			set
			{
				Definition.windowHeight = value;
			}
		}

		// Token: 0x1700061D RID: 1565
		// (get) Token: 0x060013F1 RID: 5105 RVA: 0x000762D4 File Offset: 0x000744D4
		// (set) Token: 0x060013F2 RID: 5106 RVA: 0x0003C99F File Offset: 0x0003AB9F
		public double WLeft
		{
			get
			{
				return Definition.windowLeft;
			}
			set
			{
				Definition.windowLeft = value;
			}
		}

		// Token: 0x1700061E RID: 1566
		// (get) Token: 0x060013F3 RID: 5107 RVA: 0x000762EC File Offset: 0x000744EC
		// (set) Token: 0x060013F4 RID: 5108 RVA: 0x0003C9BF File Offset: 0x0003ABBF
		public double WTop
		{
			get
			{
				return Definition.windowTop;
			}
			set
			{
				Definition.windowTop = value;
			}
		}

		// Token: 0x060013F5 RID: 5109 RVA: 0x00076304 File Offset: 0x00074504
		public MsgBoxViewModel(string title, string message, Style okButtonStyle, Style cancelButtonStyle)
		{
			this.okButtonStyle = okButtonStyle;
			this.cancelButtonStyle = cancelButtonStyle;
			this.WState = ((Definition.windowHeight > 0.0 && Definition.windowWidth > 0.0) ? WindowState.Normal : WindowState.Maximized);
			this.Title = title;
			this.Message = message;
			this.buttonImage = ImageConverter.ToBitmap("/img/cancel.png");
			this.buttonStyle = cancelButtonStyle;
		}

		// Token: 0x060013F6 RID: 5110 RVA: 0x0007637C File Offset: 0x0007457C
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x04000D14 RID: 3348
		private string title;

		// Token: 0x04000D15 RID: 3349
		private string message;

		// Token: 0x04000D16 RID: 3350
		private ImageSource buttonImage;

		// Token: 0x04000D17 RID: 3351
		private Style okButtonStyle;

		// Token: 0x04000D18 RID: 3352
		private Style cancelButtonStyle;

		// Token: 0x04000D19 RID: 3353
		private Style buttonStyle;
	}
}
