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
	// Token: 0x02000130 RID: 304
	public class CtrlProductsToLane : UserControl, IComponentConnector
	{
		// Token: 0x06001D52 RID: 7506 RVA: 0x000BF1B8 File Offset: 0x000BD3B8
		public CtrlProductsToLane(ProductManager productManager, DBConnection dbConnection, ParameterHandling parHandling, ImageDataTransfer imagetransfer)
		{
			this.InitializeComponent();
			this.viewModel = new ProductsToLaneViewModel(productManager, dbConnection, parHandling, imagetransfer);
			base.DataContext = this.viewModel;
		}

		// Token: 0x06001D53 RID: 7507 RVA: 0x000BF1E8 File Offset: 0x000BD3E8
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/productstolane/ctrlproductstolane.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x06001D54 RID: 7508 RVA: 0x000BF220 File Offset: 0x000BD420
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
				this.tbCurrentAppl = (TextBlock)target;
			}
		}

		// Token: 0x04001359 RID: 4953
		private ProductsToLaneViewModel viewModel;

		// Token: 0x0400135A RID: 4954
		internal TextBlock tbCurrentAppl;

		// Token: 0x0400135B RID: 4955
		private bool _contentLoaded;
	}
}
