using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace GUI_XRS
{
	// Token: 0x02000035 RID: 53
	public class CtrlUserBackupInfo : UserControl, IComponentConnector
	{
		// Token: 0x0600062B RID: 1579 RVA: 0x0002A0C1 File Offset: 0x000282C1
		public CtrlUserBackupInfo(Restore restore, string backupPath)
		{
			this.InitializeComponent();
			this.viewModel = new UsermanagementViewModel(restore, backupPath);
			base.DataContext = this.viewModel;
		}

		// Token: 0x0600062C RID: 1580 RVA: 0x0002A0EC File Offset: 0x000282EC
		private void UserControl_Unloaded(object sender, RoutedEventArgs e)
		{
			this.viewModel.CleanUp();
		}

		// Token: 0x0600062D RID: 1581 RVA: 0x0002A0FC File Offset: 0x000282FC
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/backup/ctrluserbackupinfo.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x0600062E RID: 1582 RVA: 0x0002A134 File Offset: 0x00028334
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				((CtrlUserBackupInfo)target).Unloaded += this.UserControl_Unloaded;
				break;
			case 2:
				this.tbCurrentUser = (TextBlock)target;
				break;
			case 3:
				this.lstUsers = (ItemsControl)target;
				break;
			default:
				this._contentLoaded = true;
				break;
			}
		}

		// Token: 0x04000477 RID: 1143
		private UsermanagementViewModel viewModel;

		// Token: 0x04000478 RID: 1144
		internal TextBlock tbCurrentUser;

		// Token: 0x04000479 RID: 1145
		internal ItemsControl lstUsers;

		// Token: 0x0400047A RID: 1146
		private bool _contentLoaded;
	}
}
