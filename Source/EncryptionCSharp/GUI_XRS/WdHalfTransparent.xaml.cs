using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Threading;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x02000163 RID: 355
	public partial class WdHalfTransparent : Window
	{
		// Token: 0x17000992 RID: 2450
		// (get) Token: 0x06002126 RID: 8486 RVA: 0x000D7E3E File Offset: 0x000D603E
		// (set) Token: 0x06002127 RID: 8487 RVA: 0x000D7E46 File Offset: 0x000D6046
		public WindowState WState { get; set; }

		// Token: 0x17000993 RID: 2451
		// (get) Token: 0x06002128 RID: 8488 RVA: 0x000D7E50 File Offset: 0x000D6050
		// (set) Token: 0x06002129 RID: 8489 RVA: 0x0003C95F File Offset: 0x0003AB5F
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

		// Token: 0x17000994 RID: 2452
		// (get) Token: 0x0600212A RID: 8490 RVA: 0x000D7E68 File Offset: 0x000D6068
		// (set) Token: 0x0600212B RID: 8491 RVA: 0x0003C97F File Offset: 0x0003AB7F
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

		// Token: 0x17000995 RID: 2453
		// (get) Token: 0x0600212C RID: 8492 RVA: 0x000D7E80 File Offset: 0x000D6080
		// (set) Token: 0x0600212D RID: 8493 RVA: 0x0003C99F File Offset: 0x0003AB9F
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

		// Token: 0x17000996 RID: 2454
		// (get) Token: 0x0600212E RID: 8494 RVA: 0x000D7E98 File Offset: 0x000D6098
		// (set) Token: 0x0600212F RID: 8495 RVA: 0x0003C9BF File Offset: 0x0003ABBF
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

		// Token: 0x06002130 RID: 8496 RVA: 0x000D7EAF File Offset: 0x000D60AF
		public WdHalfTransparent() : this(new TimeSpan(0L))
		{
		}

		// Token: 0x06002131 RID: 8497 RVA: 0x000D7EC0 File Offset: 0x000D60C0
		public WdHalfTransparent(TimeSpan maxOpenTime)
		{
			this.InitializeComponent();
			this.WState = ((Definition.windowHeight > 0.0 && Definition.windowWidth > 0.0) ? WindowState.Normal : WindowState.Maximized);
			base.DataContext = this;
			bool flag = maxOpenTime.Ticks > 0L;
			if (flag)
			{
				this.timerClose = new DispatcherTimer(maxOpenTime, DispatcherPriority.Background, new EventHandler(this.Close), base.Dispatcher);
				this.timerClose.Start();
			}
		}

		// Token: 0x06002132 RID: 8498 RVA: 0x00075F4E File Offset: 0x0007414E
		private void Close(object sender, EventArgs e)
		{
			base.Close();
		}

		// Token: 0x06002133 RID: 8499 RVA: 0x000D7F4C File Offset: 0x000D614C
		private void WdHalfTransparent_Closing(object sender, CancelEventArgs e)
		{
			bool flag = this.timerClose != null;
			if (flag)
			{
				this.timerClose.Stop();
			}
		}

		// Token: 0x04001578 RID: 5496
		private DispatcherTimer timerClose;
	}
}
