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
	// Token: 0x020000F0 RID: 240
	public class CtrlVideojetPrinter : UserControl, IComponentConnector
	{
		// Token: 0x06001785 RID: 6021 RVA: 0x0008EEEF File Offset: 0x0008D0EF
		public CtrlVideojetPrinter(VideojetPrinter printer, DBConnection dbConnection, AccessRightsManager accessManager)
		{
			this.InitializeComponent();
			base.Loaded += this.CtrlVideojetPrinter_Loaded;
			this.viewModel = new VideojetPrinterViewModel(printer, dbConnection, accessManager);
			base.DataContext = this.viewModel;
		}

		// Token: 0x06001786 RID: 6022 RVA: 0x0008EF30 File Offset: 0x0008D130
		private void CtrlVideojetPrinter_Loaded(object sender, RoutedEventArgs e)
		{
			Window window = Window.GetWindow(this);
			window.Closing += this.Window_Closing;
		}

		// Token: 0x06001787 RID: 6023 RVA: 0x0008EF58 File Offset: 0x0008D158
		private void Window_Closing(object sender, CancelEventArgs e)
		{
			this.viewModel.Cleanup();
		}

		// Token: 0x06001788 RID: 6024 RVA: 0x0008EF68 File Offset: 0x0008D168
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/videojetprinter/ctrlvideojetprinter.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x06001789 RID: 6025 RVA: 0x0008EFA0 File Offset: 0x0008D1A0
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		internal Delegate _CreateDelegate(Type delegateType, string handler)
		{
			return Delegate.CreateDelegate(delegateType, this, handler);
		}

		// Token: 0x0600178A RID: 6026 RVA: 0x0008EFBA File Offset: 0x0008D1BA
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			this._contentLoaded = true;
		}

		// Token: 0x04000F7B RID: 3963
		private VideojetPrinterViewModel viewModel;

		// Token: 0x04000F7C RID: 3964
		private bool _contentLoaded;
	}
}
