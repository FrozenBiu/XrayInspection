using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace GUI_XRS
{
	// Token: 0x0200017A RID: 378
	public class CtrlBackupInfo : UserControl, IComponentConnector
	{
		// Token: 0x06002266 RID: 8806 RVA: 0x000DE1C0 File Offset: 0x000DC3C0
		public CtrlBackupInfo(string infoFileName)
		{
			this.InitializeComponent();
			this.viewModel = new BackupInfoViewModel(infoFileName);
			base.DataContext = this.viewModel;
		}

		// Token: 0x06002267 RID: 8807 RVA: 0x000DE1EC File Offset: 0x000DC3EC
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/backup/ctrlbackupinfo.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x06002268 RID: 8808 RVA: 0x000DE222 File Offset: 0x000DC422
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			this._contentLoaded = true;
		}

		// Token: 0x0400165D RID: 5725
		private BackupInfoViewModel viewModel;

		// Token: 0x0400165E RID: 5726
		private bool _contentLoaded;
	}
}
