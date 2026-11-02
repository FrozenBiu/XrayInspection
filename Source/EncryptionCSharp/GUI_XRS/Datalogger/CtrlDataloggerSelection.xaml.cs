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
	// Token: 0x02000265 RID: 613
	public partial class CtrlDataloggerSelection : UserControl
	{
		// Token: 0x06002EED RID: 12013 RVA: 0x0013391D File Offset: 0x00131B1D
		public CtrlDataloggerSelection(ParameterHandling parHandling, Logger logger, DBConnection dbConnection, DBDataLogs dbDatalogs)
		{
			this.InitializeComponent();
			this.dataloggerSelectionViewModel = new DataloggerSelectionViewModel(logger, dbConnection, parHandling, dbDatalogs);
			base.DataContext = this.dataloggerSelectionViewModel;
		}

		// Token: 0x04001D6E RID: 7534
		private DataloggerSelectionViewModel dataloggerSelectionViewModel;
	}
}
