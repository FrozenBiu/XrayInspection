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
	// Token: 0x0200017F RID: 383
	public class CtrlBackup : UserControl, IComponentConnector
	{
		// Token: 0x060022C1 RID: 8897 RVA: 0x000DFFE8 File Offset: 0x000DE1E8
		public CtrlBackup(DBConnection dbConnection, ParameterHandling parHandling, Restore restore, Backup backup, AccessRightsManager accessManager, TestcardSimService testcardSimService, ProductManager productManager)
		{
			this.InitializeComponent();
			this.viewModel = new BackupViewModel(dbConnection, parHandling, restore, backup, accessManager, testcardSimService, productManager);
			base.DataContext = this.viewModel;
		}

		// Token: 0x060022C2 RID: 8898 RVA: 0x000E001C File Offset: 0x000DE21C
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/backup/ctrlbackup.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x060022C3 RID: 8899 RVA: 0x000E0054 File Offset: 0x000DE254
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
				this.testIt = (Border)target;
			}
		}

		// Token: 0x0400168E RID: 5774
		private BackupViewModel viewModel;

		// Token: 0x0400168F RID: 5775
		internal Border testIt;

		// Token: 0x04001690 RID: 5776
		private bool _contentLoaded;
	}
}
