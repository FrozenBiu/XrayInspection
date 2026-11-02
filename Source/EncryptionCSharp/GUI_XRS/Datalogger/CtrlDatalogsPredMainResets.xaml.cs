using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using DatabaseConnection;

namespace GUI_XRS.Datalogger
{
	// Token: 0x02000263 RID: 611
	public partial class CtrlDatalogsPredMainResets : UserControl
	{
		// Token: 0x06002EE3 RID: 12003 RVA: 0x00133626 File Offset: 0x00131826
		public CtrlDatalogsPredMainResets(DBDataLogs dBDataLogs, DBConnection dBConnection, ParameterHandling parHandling)
		{
			this.InitializeComponent();
			this.viewModel = new DatalogsPredMainResetsViewModel(dBDataLogs, dBConnection, parHandling);
			base.DataContext = this.viewModel;
		}

		// Token: 0x04001D64 RID: 7524
		private DatalogsPredMainResetsViewModel viewModel;
	}
}
