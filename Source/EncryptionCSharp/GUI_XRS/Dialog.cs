using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace GUI_XRS
{
	// Token: 0x0200023E RID: 574
	public class Dialog : Window, IComponentConnector
	{
		// Token: 0x06002BBA RID: 11194 RVA: 0x00121447 File Offset: 0x0011F647
		private Dialog()
		{
			this.InitializeComponent();
			this.timer = new DispatcherTimer();
			this.timer.Tick += this.Close;
		}

		// Token: 0x06002BBB RID: 11195 RVA: 0x0012147C File Offset: 0x0011F67C
		public static Dialog.DialogResults Show(string title, string message)
		{
			return Dialog.Show(title, message, DialogStyle.Info, DialogButtons.OK);
		}

		// Token: 0x06002BBC RID: 11196 RVA: 0x00121498 File Offset: 0x0011F698
		public static Dialog.DialogResults Show(string title, string message, DialogStyle styleOption)
		{
			return Dialog.Show(title, message, styleOption, DialogButtons.OK);
		}

		// Token: 0x06002BBD RID: 11197 RVA: 0x001214B4 File Offset: 0x0011F6B4
		public static Dialog.DialogResults Show(string title, string message, DialogStyle styleOption, DialogButtons buttonOption)
		{
			return Dialog.Show(title, message, styleOption, buttonOption, false);
		}

		// Token: 0x06002BBE RID: 11198 RVA: 0x001214D0 File Offset: 0x0011F6D0
		public static Dialog.DialogResults Show(string title, string message, DialogStyle styleOption, DialogButtons buttonOption, bool closeSelf)
		{
			Dialog.___Dialog = new Dialog();
			DialogViewModel dataContext = new DialogViewModel(Dialog.___Dialog, title, message, styleOption, buttonOption);
			Dialog.___Dialog.DataContext = dataContext;
			CursorsSettings.SetCursor(Dialog.___Dialog);
			bool flag = !closeSelf;
			if (flag)
			{
				Dialog.___Dialog.ShowDialog();
			}
			else
			{
				Dialog.___Dialog.timer.Interval = new TimeSpan(0, 0, 1);
				Dialog.___Dialog.timer.Start();
				Dialog.___Dialog.ShowDialog();
			}
			return Dialog.___Dialog.result;
		}

		// Token: 0x06002BBF RID: 11199 RVA: 0x00121567 File Offset: 0x0011F767
		private void btnOk_Click(object sender, RoutedEventArgs e)
		{
			this.result = Dialog.DialogResults.Ok;
			this.FormFadeOut.Begin();
		}

		// Token: 0x06002BC0 RID: 11200 RVA: 0x0012157D File Offset: 0x0011F77D
		private void btnCancel_Click(object sender, RoutedEventArgs e)
		{
			this.result = Dialog.DialogResults.Cancel;
			this.FormFadeOut.Begin();
		}

		// Token: 0x06002BC1 RID: 11201 RVA: 0x00075F4E File Offset: 0x0007414E
		private void FormFadeOut_Completed(object sender, EventArgs e)
		{
			base.Close();
		}

		// Token: 0x06002BC2 RID: 11202 RVA: 0x00121594 File Offset: 0x0011F794
		private void messageBox_Loaded(object sender, RoutedEventArgs e)
		{
			HwndSource hwndSource = PresentationSource.FromVisual(this) as HwndSource;
			HwndTarget compositionTarget = hwndSource.CompositionTarget;
			compositionTarget.RenderMode = RenderMode.SoftwareOnly;
		}

		// Token: 0x06002BC3 RID: 11203 RVA: 0x001215BD File Offset: 0x0011F7BD
		private void Close(object sender, EventArgs e)
		{
			this.timer.Stop();
			this.result = Dialog.DialogResults.Ok;
			this.FormFadeOut.Begin();
		}

		// Token: 0x06002BC4 RID: 11204 RVA: 0x001215E0 File Offset: 0x0011F7E0
		public static Dialog.DialogResults ShowInvoked(string title, string message, DialogStyle styleOption, DialogButtons buttonOption)
		{
			Dialog.DialogResults result = Dialog.DialogResults.Cancel;
			bool flag = Application.Current != null;
			if (flag)
			{
				Application.Current.Dispatcher.Invoke(DispatcherPriority.Render, new Action(delegate()
				{
					result = Dialog.Show(title, message, styleOption, buttonOption, false);
				}));
			}
			return result;
		}

		// Token: 0x06002BC5 RID: 11205 RVA: 0x0012164C File Offset: 0x0011F84C
		private void messageBox_Unloaded(object sender, RoutedEventArgs e)
		{
			Dialog.___Dialog.DataContext = null;
		}

		// Token: 0x06002BC6 RID: 11206 RVA: 0x0012165B File Offset: 0x0011F85B
		private void btnRename_Click(object sender, RoutedEventArgs e)
		{
			this.result = Dialog.DialogResults.Rename;
			this.FormFadeOut.Begin();
		}

		// Token: 0x06002BC7 RID: 11207 RVA: 0x00121674 File Offset: 0x0011F874
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/dialog/dialog.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x06002BC8 RID: 11208 RVA: 0x001216AC File Offset: 0x0011F8AC
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				this.messageBox = (Dialog)target;
				this.messageBox.Unloaded += this.messageBox_Unloaded;
				this.messageBox.Loaded += this.messageBox_Loaded;
				break;
			case 2:
				this.overlay = (Grid)target;
				break;
			case 3:
				this.mainGrid = (Grid)target;
				break;
			case 4:
				this.FormFadeIn = (Storyboard)target;
				break;
			case 5:
				this.MaximizeWidthAnimation = (DoubleAnimation)target;
				break;
			case 6:
				this.MaximizeHeightAnimation = (DoubleAnimation)target;
				break;
			case 7:
				this.FadeInAnimation = (DoubleAnimation)target;
				break;
			case 8:
				this.FadeInContentAnimation = (DoubleAnimation)target;
				break;
			case 9:
				this.FadeInBackgroundAnimation = (DoubleAnimation)target;
				break;
			case 10:
				this.FormFadeOut = (Storyboard)target;
				this.FormFadeOut.Completed += this.FormFadeOut_Completed;
				break;
			case 11:
				this.MinimizeWidthAnimation = (DoubleAnimation)target;
				break;
			case 12:
				this.MinimizeAnimation = (DoubleAnimation)target;
				break;
			case 13:
				this.FadeOutAnimation = (DoubleAnimation)target;
				break;
			case 14:
				this.FadeOutContentAnimation = (DoubleAnimation)target;
				break;
			case 15:
				this.FadeOutBackgroundAnimation = (DoubleAnimation)target;
				break;
			case 16:
				this.mainContent = (DockPanel)target;
				break;
			case 17:
				this.Header = (Border)target;
				break;
			case 18:
				this.Footer = (Border)target;
				break;
			case 19:
				this.btnOK = (Button)target;
				this.btnOK.Click += this.btnOk_Click;
				break;
			case 20:
				this.btnRename = (Button)target;
				this.btnRename.Click += this.btnRename_Click;
				break;
			case 21:
				this.btnOK2 = (Button)target;
				this.btnOK2.Click += this.btnOk_Click;
				break;
			case 22:
				this.btnCancel = (Button)target;
				this.btnCancel.Click += this.btnCancel_Click;
				break;
			case 23:
				this.message = (TextBlock)target;
				break;
			default:
				this._contentLoaded = true;
				break;
			}
		}

		// Token: 0x04001B59 RID: 7001
		private DispatcherTimer timer;

		// Token: 0x04001B5A RID: 7002
		private Dialog.DialogResults result;

		// Token: 0x04001B5B RID: 7003
		[ThreadStatic]
		private static Dialog ___Dialog;

		// Token: 0x04001B5C RID: 7004
		internal Dialog messageBox;

		// Token: 0x04001B5D RID: 7005
		internal Grid overlay;

		// Token: 0x04001B5E RID: 7006
		internal Grid mainGrid;

		// Token: 0x04001B5F RID: 7007
		internal Storyboard FormFadeIn;

		// Token: 0x04001B60 RID: 7008
		internal DoubleAnimation MaximizeWidthAnimation;

		// Token: 0x04001B61 RID: 7009
		internal DoubleAnimation MaximizeHeightAnimation;

		// Token: 0x04001B62 RID: 7010
		internal DoubleAnimation FadeInAnimation;

		// Token: 0x04001B63 RID: 7011
		internal DoubleAnimation FadeInContentAnimation;

		// Token: 0x04001B64 RID: 7012
		internal DoubleAnimation FadeInBackgroundAnimation;

		// Token: 0x04001B65 RID: 7013
		internal Storyboard FormFadeOut;

		// Token: 0x04001B66 RID: 7014
		internal DoubleAnimation MinimizeWidthAnimation;

		// Token: 0x04001B67 RID: 7015
		internal DoubleAnimation MinimizeAnimation;

		// Token: 0x04001B68 RID: 7016
		internal DoubleAnimation FadeOutAnimation;

		// Token: 0x04001B69 RID: 7017
		internal DoubleAnimation FadeOutContentAnimation;

		// Token: 0x04001B6A RID: 7018
		internal DoubleAnimation FadeOutBackgroundAnimation;

		// Token: 0x04001B6B RID: 7019
		internal DockPanel mainContent;

		// Token: 0x04001B6C RID: 7020
		internal Border Header;

		// Token: 0x04001B6D RID: 7021
		internal Border Footer;

		// Token: 0x04001B6E RID: 7022
		internal Button btnOK;

		// Token: 0x04001B6F RID: 7023
		internal Button btnRename;

		// Token: 0x04001B70 RID: 7024
		internal Button btnOK2;

		// Token: 0x04001B71 RID: 7025
		internal Button btnCancel;

		// Token: 0x04001B72 RID: 7026
		internal TextBlock message;

		// Token: 0x04001B73 RID: 7027
		private bool _contentLoaded;

		// Token: 0x0200037F RID: 895
		public enum DialogResults
		{
			// Token: 0x040021F7 RID: 8695
			Ok,
			// Token: 0x040021F8 RID: 8696
			Cancel,
			// Token: 0x040021F9 RID: 8697
			Rename
		}
	}
}
