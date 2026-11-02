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
	// Token: 0x02000021 RID: 33
	public class CtrlFastAuditCheckProcess : UserControl, IComponentConnector
	{
		// Token: 0x14000019 RID: 25
		// (add) Token: 0x060003ED RID: 1005 RVA: 0x00019BB4 File Offset: 0x00017DB4
		// (remove) Token: 0x060003EE RID: 1006 RVA: 0x00019BEC File Offset: 0x00017DEC
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action AuditCompleted;

		// Token: 0x060003EF RID: 1007 RVA: 0x00019C24 File Offset: 0x00017E24
		public CtrlFastAuditCheckProcess(ImageDataTransfer imageTransfer, AuditRequired auditRequired, AccessRightsManager accessManager, MachineState state, AuditMode mode, ParameterHandling handling, Restore restore, SetFooterMainWindow setFooterMainWindow, LoginToGo loginToGo, MessageSystem messageSystem, DBConnection connection, ProductManager productManager)
		{
			this.loginToGo = loginToGo;
			this.setFooterMainWindow = setFooterMainWindow;
			this.AccessManager = accessManager;
			this.InitializeComponent();
			this.viewModel = new FastAuditCheckProcessViewModel(this.imagebox, accessManager, state, handling, imageTransfer, connection, messageSystem, auditRequired, restore, mode, productManager);
			base.DataContext = this.viewModel;
			this.viewModel.Close += this.ViewModel_Close;
			this.viewModel.certificateReady += this.setImage;
			this.webbrowser.Navigate(new Uri("about:blank"));
			this.viewModel.CheckUserRightforUnlockBin(this.btnUnlockBin);
			this.viewModel.AuditRequired.ResetProperties();
		}

		// Token: 0x060003F0 RID: 1008 RVA: 0x00019CF0 File Offset: 0x00017EF0
		private void btnChangeUser_Click(object sender, RoutedEventArgs e)
		{
			this.ctrlUser = new UsermanagementControl(this.viewModel.AuditRequired.DbConnection, this.viewModel.AccessManager, this.viewModel.Restore, false, this.setFooterMainWindow, this.loginToGo);
			this.ctrlUser.Unloaded += this.ctrlUser_Unloaded;
			Subwindow.Show(this.ctrlUser, SubwindowButtons.Back, this.ctrlUser.Buttons, false);
			this.viewModel.UserLogAvailable = true;
		}

		// Token: 0x060003F1 RID: 1009 RVA: 0x00019D7C File Offset: 0x00017F7C
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

		// Token: 0x060003F2 RID: 1010 RVA: 0x00019DC4 File Offset: 0x00017FC4
		protected virtual void OnAuditCompleted()
		{
			Action auditCompleted = this.AuditCompleted;
			bool flag = auditCompleted != null;
			if (flag)
			{
				auditCompleted();
			}
		}

		// Token: 0x060003F3 RID: 1011 RVA: 0x00019DE8 File Offset: 0x00017FE8
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

		// Token: 0x060003F4 RID: 1012 RVA: 0x00019E08 File Offset: 0x00018008
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

		// Token: 0x060003F5 RID: 1013 RVA: 0x00019EAC File Offset: 0x000180AC
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

		// Token: 0x060003F6 RID: 1014 RVA: 0x00019F10 File Offset: 0x00018110
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

		// Token: 0x060003F7 RID: 1015 RVA: 0x00019FAC File Offset: 0x000181AC
		private void AuditCheck_Unloaded(object sender, RoutedEventArgs e)
		{
			this.viewModel.certificateReady -= this.setImage;
			this.viewModel.CleanUp(0);
			this.wdSub.BackButtonClicked -= this.wdSub_BackButtonClicked;
			this.webbrowser.Dispose();
			this.viewModel.Close -= this.ViewModel_Close;
			BindingOperations.ClearAllBindings(this);
			base.DataContext = null;
			GC.Collect();
		}

		// Token: 0x060003F8 RID: 1016 RVA: 0x0001A035 File Offset: 0x00018235
		private void BadButton_Click(object sender, RoutedEventArgs e)
		{
			this.viewModel.setResult(false);
		}

		// Token: 0x060003F9 RID: 1017 RVA: 0x0001A048 File Offset: 0x00018248
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

		// Token: 0x060003FA RID: 1018 RVA: 0x0001A09C File Offset: 0x0001829C
		private void AuditCheckProcess_Loaded(object sender, RoutedEventArgs e)
		{
			this.wdSub = (Window.GetWindow(this) as Subwindow);
			bool flag = this.wdSub != null;
			if (flag)
			{
				this.wdSub.BackButtonClicked += this.wdSub_BackButtonClicked;
			}
		}

		// Token: 0x060003FB RID: 1019 RVA: 0x0001A0E4 File Offset: 0x000182E4
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/auditchecknew/ctrlfastauditcheckprocess.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x060003FC RID: 1020 RVA: 0x0001A11C File Offset: 0x0001831C
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				((CtrlFastAuditCheckProcess)target).Unloaded += this.AuditCheck_Unloaded;
				((CtrlFastAuditCheckProcess)target).Loaded += this.AuditCheckProcess_Loaded;
				break;
			case 2:
				this.mainGrid = (Grid)target;
				break;
			case 3:
				this.btnUnlockBin = (Button)target;
				break;
			case 4:
				((Button)target).Click += this.BadButton_Click;
				break;
			case 5:
				this.imagebox = (XRSImageBox)target;
				break;
			case 6:
				this.listTestsamples = (ListView)target;
				break;
			case 7:
				this.pdfViewGrid = (Grid)target;
				break;
			case 8:
				this.popUp = (Popup)target;
				break;
			case 9:
				this.webbrowser = (WebBrowser)target;
				break;
			case 10:
				this.arrowLeft = (Button)target;
				this.arrowLeft.Click += this.arrowLeft_Click;
				break;
			case 11:
				((Button)target).Click += this.arrowRight_Click;
				break;
			default:
				this._contentLoaded = true;
				break;
			}
		}

		// Token: 0x040002FA RID: 762
		private FastAuditCheckProcessViewModel viewModel;

		// Token: 0x040002FB RID: 763
		private UsermanagementControl ctrlUser;

		// Token: 0x040002FC RID: 764
		private SetFooterMainWindow setFooterMainWindow;

		// Token: 0x040002FD RID: 765
		private LoginToGo loginToGo;

		// Token: 0x040002FF RID: 767
		public AccessRightsManager AccessManager;

		// Token: 0x04000300 RID: 768
		private Subwindow wdSub;

		// Token: 0x04000301 RID: 769
		internal Grid mainGrid;

		// Token: 0x04000302 RID: 770
		internal Button btnUnlockBin;

		// Token: 0x04000303 RID: 771
		internal XRSImageBox imagebox;

		// Token: 0x04000304 RID: 772
		internal ListView listTestsamples;

		// Token: 0x04000305 RID: 773
		internal Grid pdfViewGrid;

		// Token: 0x04000306 RID: 774
		internal Popup popUp;

		// Token: 0x04000307 RID: 775
		internal WebBrowser webbrowser;

		// Token: 0x04000308 RID: 776
		internal Button arrowLeft;

		// Token: 0x04000309 RID: 777
		private bool _contentLoaded;
	}
}
