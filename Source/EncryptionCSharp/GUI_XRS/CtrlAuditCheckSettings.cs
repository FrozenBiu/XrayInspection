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
	// Token: 0x02000023 RID: 35
	public class CtrlAuditCheckSettings : UserControl, IComponentConnector
	{
		// Token: 0x1400001A RID: 26
		// (add) Token: 0x06000407 RID: 1031 RVA: 0x0001A628 File Offset: 0x00018828
		// (remove) Token: 0x06000408 RID: 1032 RVA: 0x0001A660 File Offset: 0x00018860
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x06000409 RID: 1033 RVA: 0x0001A695 File Offset: 0x00018895
		public CtrlAuditCheckSettings(AuditCheckSettings auditSettings, DBConnection dbConnection, AccessRightsManager accessManager, ParameterHandling parHandling, int imageGroup, ProductManager productManager)
		{
			this.InitializeComponent();
			this.auditCheckSettingsViewModel = new AuditCheckSettingsViewModel(auditSettings, dbConnection, accessManager, productManager, parHandling);
			base.DataContext = this.auditCheckSettingsViewModel;
		}

		// Token: 0x0600040A RID: 1034 RVA: 0x0001A6C5 File Offset: 0x000188C5
		private void UserControl_Unloaded(object sender, RoutedEventArgs e)
		{
			this.auditCheckSettingsViewModel = null;
			BindingOperations.ClearAllBindings(this);
			base.DataContext = null;
		}

		// Token: 0x0600040B RID: 1035 RVA: 0x0001A6E0 File Offset: 0x000188E0
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x0600040C RID: 1036 RVA: 0x0001A710 File Offset: 0x00018910
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/auditchecknew/ctrlauditchecksettings.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x0600040D RID: 1037 RVA: 0x0001A748 File Offset: 0x00018948
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
				((CtrlAuditCheckSettings)target).Unloaded += this.UserControl_Unloaded;
			}
		}

		// Token: 0x04000313 RID: 787
		private AuditCheckSettingsViewModel auditCheckSettingsViewModel;

		// Token: 0x04000314 RID: 788
		private bool _contentLoaded;
	}
}
