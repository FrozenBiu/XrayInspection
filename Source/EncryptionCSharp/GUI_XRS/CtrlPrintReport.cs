using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media.Imaging;

namespace GUI_XRS
{
	// Token: 0x02000187 RID: 391
	public class CtrlPrintReport : UserControl, IComponentConnector
	{
		// Token: 0x060024E1 RID: 9441 RVA: 0x000FD604 File Offset: 0x000FB804
		public CtrlPrintReport(string currentProductName, BitmapSource productImage)
		{
			this.InitializeComponent();
			this.viewModel = new PrintReportViewModel(currentProductName, productImage);
			base.DataContext = this.viewModel;
		}

		// Token: 0x060024E2 RID: 9442 RVA: 0x000FD630 File Offset: 0x000FB830
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/printing/ctrlprintreport.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x060024E3 RID: 9443 RVA: 0x000FD666 File Offset: 0x000FB866
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			this._contentLoaded = true;
		}

		// Token: 0x040017A2 RID: 6050
		private PrintReportViewModel viewModel;

		// Token: 0x040017A3 RID: 6051
		private bool _contentLoaded;
	}
}
