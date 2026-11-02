using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Threading;
using DatabaseConnection;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x02000024 RID: 36
	public class CtrlAuditCheckProcess : UserControl, IComponentConnector
	{
		// Token: 0x1400001B RID: 27
		// (add) Token: 0x0600040E RID: 1038 RVA: 0x0001A784 File Offset: 0x00018984
		// (remove) Token: 0x0600040F RID: 1039 RVA: 0x0001A7BC File Offset: 0x000189BC
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action AuditCompleted;

		// Token: 0x06000410 RID: 1040 RVA: 0x0001A7F4 File Offset: 0x000189F4
		public CtrlAuditCheckProcess(ImageDataTransfer imageTransfer, AuditRequired auditRequired, AccessRightsManager accessManager, MachineState state, AuditMode mode, ParameterHandling handling, Restore restore, SetFooterMainWindow setFooterMainWindow, LoginToGo loginToGo, MessageSystem messageSystem, DBConnection connection, ProductManager productManager)
		{
			this.loginToGo = loginToGo;
			this.setFooterMainWindow = setFooterMainWindow;
			this.AccessManager = accessManager;
			this.InitializeComponent();
			this.viewModel = new AuditCheckProcessViewModel(this.imagebox, accessManager, state, handling, imageTransfer, connection, messageSystem, auditRequired, restore, mode, productManager);
			base.DataContext = this.viewModel;
			this.viewModel.Close += this.ViewModel_Close;
			this.viewModel.certificateReady += this.setImage;
			this.webbrowser.Navigate(new Uri("about:blank"));
			this.viewModel.CheckUserRightforUnlockBin(this.btnUnlockBin);
			this.viewModel.AuditRequired.ResetProperties();
		}

		// Token: 0x06000411 RID: 1041 RVA: 0x0001A8C0 File Offset: 0x00018AC0
		private void btnChangeUser_Click(object sender, RoutedEventArgs e)
		{
			this.ctrlUser = new UsermanagementControl(this.viewModel.AuditRequired.DbConnection, this.viewModel.AccessManager, this.viewModel.Restore, false, this.setFooterMainWindow, this.loginToGo);
			this.ctrlUser.Unloaded += this.ctrlUser_Unloaded;
			Subwindow.Show(this.ctrlUser, SubwindowButtons.Back, this.ctrlUser.Buttons, false);
			this.viewModel.UserLogAvailable = true;
		}

		// Token: 0x06000412 RID: 1042 RVA: 0x0001A94C File Offset: 0x00018B4C
		private void ctrlUser_Unloaded(object sender, RoutedEventArgs e)
		{
			bool flag = this.viewModel != null;
			if (flag)
			{
				this.viewModel.UserLogAvailable = false;
			}
			this.ctrlUser.Unloaded -= this.ctrlUser_Unloaded;
			base.Focus();
		}

		// Token: 0x06000413 RID: 1043 RVA: 0x0001A994 File Offset: 0x00018B94
		protected virtual void OnAuditCompleted()
		{
			Action auditCompleted = this.AuditCompleted;
			bool flag = auditCompleted != null;
			if (flag)
			{
				auditCompleted();
			}
		}

		// Token: 0x06000414 RID: 1044 RVA: 0x0001A9B8 File Offset: 0x00018BB8
		private void ViewModel_Close()
		{
			base.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
			{
				this.viewModel.DeregisterUnlockBinButton(this.btnUnlockBin);
				this.popUp.IsOpen = false;
				this.viewModel.CleanUp(null);
				this.viewModel.AuditRequired.StopRemainingTimeUpdate();
				this.viewModel.CurrentImage = null;
				this.webbrowser.Navigate("about:blank");
				this.webbrowser.Dispose();
				this.viewModel.Close -= this.ViewModel_Close;
				Window.GetWindow(this).Close();
				BindingOperations.ClearAllBindings(this);
				base.DataContext = null;
				this.viewModel.certificateReady -= this.setImage;
			}));
		}

		// Token: 0x06000415 RID: 1045 RVA: 0x0001A9D8 File Offset: 0x00018BD8
		private void arrowRight_Click(object sender, RoutedEventArgs e)
		{
			bool button1Enabled = this.viewModel.button1Enabled;
			if (button1Enabled)
			{
				bool flag = this.AccessManager.CurrentUser.ID != 0;
				if (flag)
				{
					this.viewModel.button1Enabled = false;
					this.viewModel.button2Enabled = true;
					this.setFooterMainWindow.SetFooterDeactivation();
				}
				else
				{
					Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("msgLogin"), DialogStyle.Warning, DialogButtons.OK);
				}
			}
			else
			{
				bool button3Enabled = this.viewModel.button3Enabled;
				if (button3Enabled)
				{
					this.viewModel.OnClose();
				}
			}
		}

		// Token: 0x06000416 RID: 1046 RVA: 0x0001AA7C File Offset: 0x00018C7C
		private void arrowLeft_Click(object sender, RoutedEventArgs e)
		{
			bool flag = !this.viewModel.button1Enabled;
			if (flag)
			{
				bool flag2 = Dialog.Show(LocalizationManager.Localize("txtInfo"), LocalizationManager.Localize("txtRestartAuditCheck"), DialogStyle.Info, DialogButtons.OKCancel, false) == Dialog.DialogResults.Ok;
				if (flag2)
				{
					this.webbrowser.Navigate("about:blank");
					this.viewModel.backToStart();
				}
			}
		}

		// Token: 0x06000417 RID: 1047 RVA: 0x0001AAE0 File Offset: 0x00018CE0
		public void setImage(string path)
		{
			bool flag = File.Exists(path);
			if (flag)
			{
				Stopwatch stopwatch = Stopwatch.StartNew();
				Action <>9__0;
				while (stopwatch.ElapsedMilliseconds < 1500L)
				{
					try
					{
						Dispatcher dispatcher = base.Dispatcher;
						Action callback;
						if ((callback = <>9__0) == null)
						{
							callback = (<>9__0 = delegate()
							{
								this.webbrowser.Navigate(new Uri(path));
							});
						}
						dispatcher.Invoke(callback);
						stopwatch.Stop();
						break;
					}
					catch
					{
					}
				}
			}
		}

		// Token: 0x06000418 RID: 1048 RVA: 0x0001AB7C File Offset: 0x00018D7C
		private void AuditCheck_Unloaded(object sender, RoutedEventArgs e)
		{
			this.webbrowser.Dispose();
			this.viewModel.certificateReady -= this.setImage;
			this.viewModel.CleanUp(0);
			this.viewModel.Close -= this.ViewModel_Close;
			BindingOperations.ClearAllBindings(this);
			base.DataContext = null;
			GC.Collect();
		}

		// Token: 0x06000419 RID: 1049 RVA: 0x0001ABED File Offset: 0x00018DED
		private void OkButton_Click(object sender, RoutedEventArgs e)
		{
			this.viewModel.setResult(true);
		}

		// Token: 0x0600041A RID: 1050 RVA: 0x0001ABFD File Offset: 0x00018DFD
		private void BadButton_Click_1(object sender, RoutedEventArgs e)
		{
			this.viewModel.setResult(false);
		}

		// Token: 0x0600041B RID: 1051 RVA: 0x0001AC10 File Offset: 0x00018E10
		private void wdSub_BackButtonClicked()
		{
			bool flag = this.wdSub != null;
			if (flag)
			{
				bool flag2 = this.viewModel != null;
				if (flag2)
				{
					this.viewModel.EndResultAuditCheck();
				}
				else
				{
					this.wdSub.BackButtonClicked -= this.wdSub_BackButtonClicked;
				}
			}
		}

		// Token: 0x0600041C RID: 1052 RVA: 0x0001AC64 File Offset: 0x00018E64
		private void AuditCheckProcess_Loaded(object sender, RoutedEventArgs e)
		{
			this.wdSub = (Window.GetWindow(this) as Subwindow);
			bool flag = this.wdSub != null;
			if (flag)
			{
				this.wdSub.BackButtonClicked += this.wdSub_BackButtonClicked;
			}
		}

		// Token: 0x0600041D RID: 1053 RVA: 0x0001ACAC File Offset: 0x00018EAC
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/auditchecknew/ctrlauditcheckprocess.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x0600041E RID: 1054 RVA: 0x0001ACE4 File Offset: 0x00018EE4
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				((CtrlAuditCheckProcess)target).Unloaded += this.AuditCheck_Unloaded;
				((CtrlAuditCheckProcess)target).Loaded += this.AuditCheckProcess_Loaded;
				break;
			case 2:
				this.mainGrid = (Grid)target;
				break;
			case 3:
				this.btnUnlockBin = (Button)target;
				break;
			case 4:
				this.imagebox = (XRSImageBox)target;
				break;
			case 5:
				((Button)target).Click += this.OkButton_Click;
				break;
			case 6:
				((Button)target).Click += this.BadButton_Click_1;
				break;
			case 7:
				this.listTestsamples = (ListView)target;
				break;
			case 8:
				this.pdfViewGrid = (Grid)target;
				break;
			case 9:
				this.popUp = (Popup)target;
				break;
			case 10:
				this.webbrowser = (WebBrowser)target;
				break;
			case 11:
				this.arrowLeft = (Button)target;
				this.arrowLeft.Click += this.arrowLeft_Click;
				break;
			case 12:
				((Button)target).Click += this.arrowRight_Click;
				break;
			default:
				this._contentLoaded = true;
				break;
			}
		}

		// Token: 0x04000315 RID: 789
		private AuditCheckProcessViewModel viewModel;

		// Token: 0x04000316 RID: 790
		private UsermanagementControl ctrlUser;

		// Token: 0x04000317 RID: 791
		private SetFooterMainWindow setFooterMainWindow;

		// Token: 0x04000318 RID: 792
		private LoginToGo loginToGo;

		// Token: 0x0400031A RID: 794
		public AccessRightsManager AccessManager;

		// Token: 0x0400031B RID: 795
		private Subwindow wdSub;

		// Token: 0x0400031C RID: 796
		internal Grid mainGrid;

		// Token: 0x0400031D RID: 797
		internal Button btnUnlockBin;

		// Token: 0x0400031E RID: 798
		internal XRSImageBox imagebox;

		// Token: 0x0400031F RID: 799
		internal ListView listTestsamples;

		// Token: 0x04000320 RID: 800
		internal Grid pdfViewGrid;

		// Token: 0x04000321 RID: 801
		internal Popup popUp;

		// Token: 0x04000322 RID: 802
		internal WebBrowser webbrowser;

		// Token: 0x04000323 RID: 803
		internal Button arrowLeft;

		// Token: 0x04000324 RID: 804
		private bool _contentLoaded;
	}
}
