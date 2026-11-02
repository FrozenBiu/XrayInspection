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
	// Token: 0x0200005F RID: 95
	public class CtrlComissioningFeatures : UserControl, IComponentConnector
	{
		// Token: 0x06000964 RID: 2404 RVA: 0x0003A089 File Offset: 0x00038289
		public CtrlComissioningFeatures(DBConnection dbConnection, ParameterHandling parHandling)
		{
			this.InitializeComponent();
			this.viewModel = new ComissioningFeaturesViewModel(dbConnection, parHandling);
			base.DataContext = this.viewModel;
		}

		// Token: 0x06000965 RID: 2405 RVA: 0x0003A0B4 File Offset: 0x000382B4
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/comissioning/ctrlcomissioningfeatures.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x06000966 RID: 2406 RVA: 0x0003A0EA File Offset: 0x000382EA
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			this._contentLoaded = true;
		}

		// Token: 0x04000667 RID: 1639
		private ComissioningFeaturesViewModel viewModel;

		// Token: 0x04000668 RID: 1640
		public DockPanel PanelSubButtons;

		// Token: 0x04000669 RID: 1641
		private bool _contentLoaded;
	}
}
