using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Threading;

namespace GUI_XRS
{
	// Token: 0x02000194 RID: 404
	public class WdAuditRequired : Window, IComponentConnector
	{
		// Token: 0x140000CD RID: 205
		// (add) Token: 0x0600266C RID: 9836 RVA: 0x00104970 File Offset: 0x00102B70
		// (remove) Token: 0x0600266D RID: 9837 RVA: 0x001049A8 File Offset: 0x00102BA8
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action AuditCompleted;

		// Token: 0x17000AF6 RID: 2806
		// (get) Token: 0x0600266E RID: 9838 RVA: 0x001049E0 File Offset: 0x00102BE0
		public bool TestRunning
		{
			get
			{
				bool flag = this.auditRequiredViewModel != null;
				return flag && this.auditRequiredViewModel.StartTestVisible == Visibility.Collapsed && this.auditRequiredViewModel.AuditImages.Count < this.auditRequiredViewModel.AuditRequired.NProducts;
			}
		}

		// Token: 0x0600266F RID: 9839 RVA: 0x00104A38 File Offset: 0x00102C38
		public WdAuditRequired(ImageDataTransfer imageTransfer, AuditRequired auditRequired, AccessRightsManager accessManager, MachineState machineState, AuditMode mode, ParameterHandling parHandling, Restore restore, SetFooterMainWindow setFooterMainWindow, LoginToGo loginToGo)
		{
			this.setFooterMainWindow = setFooterMainWindow;
			this.loginToGo = loginToGo;
			this.InitializeComponent();
			this.auditRequiredViewModel = new AuditRequiredViewModel(auditRequired, accessManager, machineState, mode, imageTransfer, parHandling, restore);
			this.auditRequiredViewModel.AuditRequired.ResetProperties();
			this.auditRequiredViewModel.Close += this.auditRequiredViewModel_Close;
			this.auditRequiredViewModel.CheckUserRightforUnlockBin(this.btnUnlockBin);
			base.DataContext = this.auditRequiredViewModel;
		}

		// Token: 0x06002670 RID: 9840 RVA: 0x00104AC4 File Offset: 0x00102CC4
		protected virtual void OnAuditCompleted()
		{
			Action auditCompleted = this.AuditCompleted;
			bool flag = auditCompleted != null;
			if (flag)
			{
				auditCompleted();
			}
		}

		// Token: 0x06002671 RID: 9841 RVA: 0x00104AE8 File Offset: 0x00102CE8
		private void auditRequiredViewModel_Close()
		{
			base.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
			{
				this.auditRequiredViewModel.DeregisterUnlockBinButton(this.btnUnlockBin);
				this.auditRequiredViewModel.Close -= this.auditRequiredViewModel_Close;
				this.auditRequiredViewModel.CleanUp();
				this.auditRequiredViewModel.AuditRequired.StopRemainingTimeUpdate();
				this.auditRequiredViewModel.AuditRequired.DisableContaminationsCounter();
				this.auditRequiredViewModel.CurrentImage = null;
				this.auditRequiredViewModel = null;
				base.Close();
				BindingOperations.ClearAllBindings(this);
				base.DataContext = null;
			}));
		}

		// Token: 0x06002672 RID: 9842 RVA: 0x00104B08 File Offset: 0x00102D08
		private void btnChangeUser_Click(object sender, RoutedEventArgs e)
		{
			this.ctrlUser = new UsermanagementControl(this.auditRequiredViewModel.AuditRequired.DbConnection, this.auditRequiredViewModel.AccessManager, this.auditRequiredViewModel.Restore, false, this.setFooterMainWindow, this.loginToGo);
			this.ctrlUser.Unloaded += this.ctrlUser_Unloaded;
			Subwindow.Show(this.ctrlUser, SubwindowButtons.Back, this.ctrlUser.Buttons, false);
			this.auditRequiredViewModel.UserLogAvailable = true;
		}

		// Token: 0x06002673 RID: 9843 RVA: 0x00104B94 File Offset: 0x00102D94
		private void ctrlUser_Unloaded(object sender, RoutedEventArgs e)
		{
			bool flag = this.auditRequiredViewModel != null;
			if (flag)
			{
				this.auditRequiredViewModel.UserLogAvailable = false;
			}
			this.ctrlUser.Unloaded -= this.ctrlUser_Unloaded;
			base.Focus();
		}

		// Token: 0x06002674 RID: 9844 RVA: 0x0003CA4C File Offset: 0x0003AC4C
		private void Window_Closing(object sender, CancelEventArgs e)
		{
			WorkingArea.ClearResources(this);
		}

		// Token: 0x06002675 RID: 9845 RVA: 0x00104BDC File Offset: 0x00102DDC
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/auditcheck/wdauditrequired.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x06002676 RID: 9846 RVA: 0x00104C14 File Offset: 0x00102E14
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				((WdAuditRequired)target).Closing += this.Window_Closing;
				break;
			case 2:
				this.overlay = (Grid)target;
				break;
			case 3:
				this.subWindowContent = (Grid)target;
				break;
			case 4:
				this.btnChangeUser = (Button)target;
				this.btnChangeUser.Click += this.btnChangeUser_Click;
				break;
			case 5:
				this.tbUser = (TextBlock)target;
				break;
			case 6:
				this.btnUnlockBin = (Button)target;
				break;
			default:
				this._contentLoaded = true;
				break;
			}
		}

		// Token: 0x0400188E RID: 6286
		private AuditRequiredViewModel auditRequiredViewModel;

		// Token: 0x0400188F RID: 6287
		private UsermanagementControl ctrlUser;

		// Token: 0x04001890 RID: 6288
		private SetFooterMainWindow setFooterMainWindow;

		// Token: 0x04001892 RID: 6290
		private LoginToGo loginToGo;

		// Token: 0x04001893 RID: 6291
		internal Grid overlay;

		// Token: 0x04001894 RID: 6292
		internal Grid subWindowContent;

		// Token: 0x04001895 RID: 6293
		internal Button btnChangeUser;

		// Token: 0x04001896 RID: 6294
		internal TextBlock tbUser;

		// Token: 0x04001897 RID: 6295
		internal Button btnUnlockBin;

		// Token: 0x04001898 RID: 6296
		private bool _contentLoaded;
	}
}
