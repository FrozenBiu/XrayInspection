using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using DatabaseConnection;

namespace GUI_XRS
{
	// Token: 0x0200003E RID: 62
	public class CtrlBranding : UserControl, IComponentConnector
	{
		// Token: 0x06000692 RID: 1682 RVA: 0x0002BC0C File Offset: 0x00029E0C
		public CtrlBranding(DBConnection dbConnection, Branding selectedBranding, ParameterHandling parHandling)
		{
			this.InitializeComponent();
			this.brandingViewModel = new BrandingViewModel(dbConnection, selectedBranding, parHandling);
			base.DataContext = this.brandingViewModel;
		}

		// Token: 0x06000693 RID: 1683 RVA: 0x0002BC38 File Offset: 0x00029E38
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/branding/ctrlbranding.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x06000694 RID: 1684 RVA: 0x0002BC6E File Offset: 0x00029E6E
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			this._contentLoaded = true;
		}

		// Token: 0x040004A2 RID: 1186
		private BrandingViewModel brandingViewModel;

		// Token: 0x040004A3 RID: 1187
		private bool _contentLoaded;
	}
}
