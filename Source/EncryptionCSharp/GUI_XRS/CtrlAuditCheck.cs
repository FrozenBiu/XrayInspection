using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using DatabaseConnection;

namespace GUI_XRS
{
	// Token: 0x02000192 RID: 402
	public class CtrlAuditCheck : UserControl, IComponentConnector
	{
		// Token: 0x06002667 RID: 9831 RVA: 0x001048AD File Offset: 0x00102AAD
		public CtrlAuditCheck(AuditCheckSettings auditSettings, DBConnection dbConnection, AccessRightsManager accessManager)
		{
			this.InitializeComponent();
			this.auditCheckSettingsViewModel = new AuditCheckSettingsViewModel(auditSettings, dbConnection, accessManager, null, null);
			base.DataContext = this.auditCheckSettingsViewModel;
		}

		// Token: 0x06002668 RID: 9832 RVA: 0x001048DB File Offset: 0x00102ADB
		private void UserControl_Unloaded(object sender, RoutedEventArgs e)
		{
			this.auditCheckSettingsViewModel = null;
			BindingOperations.ClearAllBindings(this);
			base.DataContext = null;
		}

		// Token: 0x06002669 RID: 9833 RVA: 0x001048F4 File Offset: 0x00102AF4
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/auditcheck/ctrlauditcheck.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x0600266A RID: 9834 RVA: 0x0010492C File Offset: 0x00102B2C
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
				((CtrlAuditCheck)target).Unloaded += this.UserControl_Unloaded;
			}
		}

		// Token: 0x0400188C RID: 6284
		private AuditCheckSettingsViewModel auditCheckSettingsViewModel;

		// Token: 0x0400188D RID: 6285
		private bool _contentLoaded;
	}
}
