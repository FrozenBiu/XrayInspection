using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace GUI_XRS
{
	// Token: 0x020000D0 RID: 208
	public class CtrlSWButtons : UserControl, IComponentConnector
	{
		// Token: 0x0600146B RID: 5227 RVA: 0x00078464 File Offset: 0x00076664
		public CtrlSWButtons()
		{
			this.InitializeComponent();
		}

		// Token: 0x0600146C RID: 5228 RVA: 0x00078478 File Offset: 0x00076678
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/subwindow/ctrlswbuttons.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x0600146D RID: 5229 RVA: 0x000784B0 File Offset: 0x000766B0
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			if (connectionId != 1)
			{
				this._contentLoaded = true;
			}
			else
			{
				this.btnStartXRay = (Button)target;
			}
		}

		// Token: 0x04000D80 RID: 3456
		internal Button btnStartXRay;

		// Token: 0x04000D81 RID: 3457
		private bool _contentLoaded;
	}
}
