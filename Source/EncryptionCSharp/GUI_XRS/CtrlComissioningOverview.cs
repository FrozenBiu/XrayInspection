using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using DatabaseConnection;
using Microsoft.Windows.Controls;

namespace GUI_XRS
{
	// Token: 0x02000062 RID: 98
	public class CtrlComissioningOverview : UserControl, IComponentConnector
	{
		// Token: 0x0600096F RID: 2415 RVA: 0x0003A354 File Offset: 0x00038554
		public CtrlComissioningOverview(DBConnection dbConnection, ParameterHandling parHandling)
		{
			this.InitializeComponent();
			this.viewModel = new ComissioningOverviewViewModel(dbConnection, parHandling);
			base.DataContext = this.viewModel;
			this.OveviewComissionParamsDataGrid.ItemsSource = ComissioningOverviewViewModel.ComissioningOverviewList;
		}

		// Token: 0x06000970 RID: 2416 RVA: 0x0003A390 File Offset: 0x00038590
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/comissioning/ctrlcomissioningoverview.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x06000971 RID: 2417 RVA: 0x0003A3C8 File Offset: 0x000385C8
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
				this.OveviewComissionParamsDataGrid = (Microsoft.Windows.Controls.DataGrid)target;
			}
		}

		// Token: 0x04000670 RID: 1648
		private ComissioningOverviewViewModel viewModel;

		// Token: 0x04000671 RID: 1649
		public DockPanel PanelSubButtons;

		// Token: 0x04000672 RID: 1650
		internal Microsoft.Windows.Controls.DataGrid OveviewComissionParamsDataGrid;

		// Token: 0x04000673 RID: 1651
		private bool _contentLoaded;
	}
}
