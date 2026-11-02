using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media.Animation;

namespace GUI_XRS
{
	// Token: 0x020000C5 RID: 197
	public class MsgBox : Window, IComponentConnector
	{
		// Token: 0x17000615 RID: 1557
		// (get) Token: 0x060013DA RID: 5082 RVA: 0x00075EE5 File Offset: 0x000740E5
		// (set) Token: 0x060013DB RID: 5083 RVA: 0x00075EED File Offset: 0x000740ED
		public Dialog.DialogResults result { get; private set; }

		// Token: 0x060013DC RID: 5084 RVA: 0x00075EF8 File Offset: 0x000740F8
		public MsgBox(string title, string message)
		{
			this.InitializeComponent();
			this.viewModel = new MsgBoxViewModel(title, message, base.FindResource("okButtonStyle") as Style, base.FindResource("cancelButtonStyle") as Style);
			base.DataContext = this.viewModel;
		}

		// Token: 0x060013DD RID: 5085 RVA: 0x00075F4E File Offset: 0x0007414E
		private void FormFadeOut_Completed(object sender, EventArgs e)
		{
			base.Close();
		}

		// Token: 0x060013DE RID: 5086 RVA: 0x00075F58 File Offset: 0x00074158
		private void btnCancel_Click(object sender, RoutedEventArgs e)
		{
			this.result = Dialog.DialogResults.Cancel;
			this.FormFadeOut.Begin();
		}

		// Token: 0x060013DF RID: 5087 RVA: 0x00075F70 File Offset: 0x00074170
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/msgbox/msgbox.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x060013E0 RID: 5088 RVA: 0x00075FA8 File Offset: 0x000741A8
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				this.overlay = (Grid)target;
				break;
			case 2:
				this.mainGrid = (Grid)target;
				break;
			case 3:
				this.FormFadeIn = (Storyboard)target;
				break;
			case 4:
				this.MaximizeWidthAnimation = (DoubleAnimation)target;
				break;
			case 5:
				this.MaximizeHeightAnimation = (DoubleAnimation)target;
				break;
			case 6:
				this.FadeInAnimation = (DoubleAnimation)target;
				break;
			case 7:
				this.FadeInContentAnimation = (DoubleAnimation)target;
				break;
			case 8:
				this.FadeInBackgroundAnimation = (DoubleAnimation)target;
				break;
			case 9:
				this.FormFadeOut = (Storyboard)target;
				this.FormFadeOut.Completed += this.FormFadeOut_Completed;
				break;
			case 10:
				this.MinimizeWidthAnimation = (DoubleAnimation)target;
				break;
			case 11:
				this.MinimizeAnimation = (DoubleAnimation)target;
				break;
			case 12:
				this.FadeOutAnimation = (DoubleAnimation)target;
				break;
			case 13:
				this.FadeOutContentAnimation = (DoubleAnimation)target;
				break;
			case 14:
				this.FadeOutBackgroundAnimation = (DoubleAnimation)target;
				break;
			case 15:
				this.mainContent = (DockPanel)target;
				break;
			case 16:
				this.Header = (Border)target;
				break;
			case 17:
				this.Footer = (Border)target;
				break;
			case 18:
				this.btnCancel = (Button)target;
				this.btnCancel.Click += this.btnCancel_Click;
				break;
			default:
				this._contentLoaded = true;
				break;
			}
		}

		// Token: 0x04000CFF RID: 3327
		private MsgBoxViewModel viewModel;

		// Token: 0x04000D01 RID: 3329
		internal Grid overlay;

		// Token: 0x04000D02 RID: 3330
		internal Grid mainGrid;

		// Token: 0x04000D03 RID: 3331
		internal Storyboard FormFadeIn;

		// Token: 0x04000D04 RID: 3332
		internal DoubleAnimation MaximizeWidthAnimation;

		// Token: 0x04000D05 RID: 3333
		internal DoubleAnimation MaximizeHeightAnimation;

		// Token: 0x04000D06 RID: 3334
		internal DoubleAnimation FadeInAnimation;

		// Token: 0x04000D07 RID: 3335
		internal DoubleAnimation FadeInContentAnimation;

		// Token: 0x04000D08 RID: 3336
		internal DoubleAnimation FadeInBackgroundAnimation;

		// Token: 0x04000D09 RID: 3337
		internal Storyboard FormFadeOut;

		// Token: 0x04000D0A RID: 3338
		internal DoubleAnimation MinimizeWidthAnimation;

		// Token: 0x04000D0B RID: 3339
		internal DoubleAnimation MinimizeAnimation;

		// Token: 0x04000D0C RID: 3340
		internal DoubleAnimation FadeOutAnimation;

		// Token: 0x04000D0D RID: 3341
		internal DoubleAnimation FadeOutContentAnimation;

		// Token: 0x04000D0E RID: 3342
		internal DoubleAnimation FadeOutBackgroundAnimation;

		// Token: 0x04000D0F RID: 3343
		internal DockPanel mainContent;

		// Token: 0x04000D10 RID: 3344
		internal Border Header;

		// Token: 0x04000D11 RID: 3345
		internal Border Footer;

		// Token: 0x04000D12 RID: 3346
		internal Button btnCancel;

		// Token: 0x04000D13 RID: 3347
		private bool _contentLoaded;
	}
}
