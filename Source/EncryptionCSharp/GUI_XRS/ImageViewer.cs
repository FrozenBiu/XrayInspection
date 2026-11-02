using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media.Imaging;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x0200006F RID: 111
	public class ImageViewer : Window, IComponentConnector
	{
		// Token: 0x170002CF RID: 719
		// (get) Token: 0x060009DF RID: 2527 RVA: 0x0003C935 File Offset: 0x0003AB35
		// (set) Token: 0x060009E0 RID: 2528 RVA: 0x0003C93D File Offset: 0x0003AB3D
		public WindowState WState { get; set; }

		// Token: 0x170002D0 RID: 720
		// (get) Token: 0x060009E1 RID: 2529 RVA: 0x0003C948 File Offset: 0x0003AB48
		// (set) Token: 0x060009E2 RID: 2530 RVA: 0x0003C95F File Offset: 0x0003AB5F
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

		// Token: 0x170002D1 RID: 721
		// (get) Token: 0x060009E3 RID: 2531 RVA: 0x0003C968 File Offset: 0x0003AB68
		// (set) Token: 0x060009E4 RID: 2532 RVA: 0x0003C97F File Offset: 0x0003AB7F
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

		// Token: 0x170002D2 RID: 722
		// (get) Token: 0x060009E5 RID: 2533 RVA: 0x0003C988 File Offset: 0x0003AB88
		// (set) Token: 0x060009E6 RID: 2534 RVA: 0x0003C99F File Offset: 0x0003AB9F
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

		// Token: 0x170002D3 RID: 723
		// (get) Token: 0x060009E7 RID: 2535 RVA: 0x0003C9A8 File Offset: 0x0003ABA8
		// (set) Token: 0x060009E8 RID: 2536 RVA: 0x0003C9BF File Offset: 0x0003ABBF
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

		// Token: 0x060009E9 RID: 2537 RVA: 0x0003C9C8 File Offset: 0x0003ABC8
		public ImageViewer(string path)
		{
			this.InitializeComponent();
			this.WState = ((Definition.windowHeight > 0.0 && Definition.windowWidth > 0.0) ? WindowState.Normal : WindowState.Maximized);
			base.DataContext = this;
			this.image = new BitmapImage(new Uri(path));
			this.imageCert.Source = this.image;
		}

		// Token: 0x060009EA RID: 2538 RVA: 0x0003CA3A File Offset: 0x0003AC3A
		private void btnBack_Click(object sender, EventArgs e)
		{
			base.DataContext = null;
			base.Close();
		}

		// Token: 0x060009EB RID: 2539 RVA: 0x0003CA4C File Offset: 0x0003AC4C
		private void Window_Closing(object sender, CancelEventArgs e)
		{
			WorkingArea.ClearResources(this);
		}

		// Token: 0x060009EC RID: 2540 RVA: 0x0003CA58 File Offset: 0x0003AC58
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/integrated%20validation%20process/imageviewer.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x060009ED RID: 2541 RVA: 0x0003CA90 File Offset: 0x0003AC90
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				((ImageViewer)target).Closing += this.Window_Closing;
				break;
			case 2:
				this.mainGrid = (Grid)target;
				break;
			case 3:
				this.contentGrid = (Grid)target;
				break;
			case 4:
				this.imageCert = (Image)target;
				break;
			case 5:
				this.mainContent = (Border)target;
				break;
			case 6:
				this.btnBack = (Button)target;
				this.btnBack.Click += new RoutedEventHandler(this.btnBack_Click);
				break;
			default:
				this._contentLoaded = true;
				break;
			}
		}

		// Token: 0x040006C3 RID: 1731
		private BitmapImage image;

		// Token: 0x040006C4 RID: 1732
		internal Grid mainGrid;

		// Token: 0x040006C5 RID: 1733
		internal Grid contentGrid;

		// Token: 0x040006C6 RID: 1734
		internal Image imageCert;

		// Token: 0x040006C7 RID: 1735
		internal Border mainContent;

		// Token: 0x040006C8 RID: 1736
		internal Button btnBack;

		// Token: 0x040006C9 RID: 1737
		private bool _contentLoaded;
	}
}
