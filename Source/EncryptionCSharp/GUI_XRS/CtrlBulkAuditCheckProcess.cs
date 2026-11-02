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
	// Token: 0x0200001D RID: 29
	public class CtrlBulkAuditCheckProcess : UserControl, IComponentConnector
	{
		// Token: 0x14000014 RID: 20
		// (add) Token: 0x0600029A RID: 666 RVA: 0x00010D0C File Offset: 0x0000EF0C
		// (remove) Token: 0x0600029B RID: 667 RVA: 0x00010D44 File Offset: 0x0000EF44
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action AuditCompleted;

		// Token: 0x0600029C RID: 668 RVA: 0x00010D7C File Offset: 0x0000EF7C
		public CtrlBulkAuditCheckProcess(ImageDataTransfer imageTransfer, AuditRequired auditRequired, AccessRightsManager accessManager, MachineState state, AuditMode mode, ParameterHandling handling, Restore restore, SetFooterMainWindow setFooterMainWindow, LoginToGo loginToGo, MessageSystem messageSystem, DBConnection connection, ProductManager productManager)
		{
			this.loginToGo = loginToGo;
			this.setFooterMainWindow = setFooterMainWindow;
			this.AccessManager = accessManager;
			this.InitializeComponent();
			this.viewModel = new BulkAuditCheckProcessViewModel(this.imagebox, accessManager, state, handling, imageTransfer, connection, messageSystem, auditRequired, restore, mode, productManager);
			base.DataContext = this.viewModel;
			this.viewModel.Close += this.ViewModel_Close;
			this.viewModel.certificateReady += this.setImage;
			this.webbrowser.Navigate(new Uri("about:blank"));
			this.viewModel.CheckUserRightforUnlockBin(this.btnUnlockBin);
			this.viewModel.AuditRequired.ResetProperties();
		}

		// Token: 0x0600029D RID: 669 RVA: 0x00010E48 File Offset: 0x0000F048
		private void btnChangeUser_Click(object sender, RoutedEventArgs e)
		{
			this.ctrlUser = new UsermanagementControl(this.viewModel.AuditRequired.DbConnection, this.viewModel.AccessManager, this.viewModel.Restore, false, this.setFooterMainWindow, this.loginToGo);
			this.ctrlUser.Unloaded += this.ctrlUser_Unloaded;
			Subwindow.Show(this.ctrlUser, SubwindowButtons.Back, this.ctrlUser.Buttons, false);
			this.viewModel.UserLogAvailable = true;
		}

		// Token: 0x0600029E RID: 670 RVA: 0x00010ED4 File Offset: 0x0000F0D4
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

		// Token: 0x0600029F RID: 671 RVA: 0x00010F1C File Offset: 0x0000F11C
		protected virtual void OnAuditCompleted()
		{
			Action auditCompleted = this.AuditCompleted;
			bool flag = auditCompleted != null;
			if (flag)
			{
				auditCompleted();
			}
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x00010F40 File Offset: 0x0000F140
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

		// Token: 0x060002A1 RID: 673 RVA: 0x00010F60 File Offset: 0x0000F160
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

		// Token: 0x060002A2 RID: 674 RVA: 0x00011004 File Offset: 0x0000F204
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

		// Token: 0x060002A3 RID: 675 RVA: 0x00011068 File Offset: 0x0000F268
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

		// Token: 0x060002A4 RID: 676 RVA: 0x00011104 File Offset: 0x0000F304
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

		// Token: 0x060002A5 RID: 677 RVA: 0x00011175 File Offset: 0x0000F375
		private void BadButton_Click(object sender, RoutedEventArgs e)
		{
			this.viewModel.setResult(false);
		}

		// Token: 0x060002A6 RID: 678 RVA: 0x00011188 File Offset: 0x0000F388
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

		// Token: 0x060002A7 RID: 679 RVA: 0x000111DC File Offset: 0x0000F3DC
		private void AuditCheckProcess_Loaded(object sender, RoutedEventArgs e)
		{
			this.wdSub = (Window.GetWindow(this) as Subwindow);
			bool flag = this.wdSub != null;
			if (flag)
			{
				this.wdSub.BackButtonClicked += this.wdSub_BackButtonClicked;
			}
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x00011224 File Offset: 0x0000F424
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/auditchecknew/ctrlbulkauditcheckprocess.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x0001125C File Offset: 0x0000F45C
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				((CtrlBulkAuditCheckProcess)target).Unloaded += this.AuditCheck_Unloaded;
				((CtrlBulkAuditCheckProcess)target).Loaded += this.AuditCheckProcess_Loaded;
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

		// Token: 0x0400022F RID: 559
		private BulkAuditCheckProcessViewModel viewModel;

		// Token: 0x04000230 RID: 560
		private UsermanagementControl ctrlUser;

		// Token: 0x04000231 RID: 561
		private SetFooterMainWindow setFooterMainWindow;

		// Token: 0x04000232 RID: 562
		private LoginToGo loginToGo;

		// Token: 0x04000234 RID: 564
		public AccessRightsManager AccessManager;

		// Token: 0x04000235 RID: 565
		private Subwindow wdSub;

		// Token: 0x04000236 RID: 566
		internal Grid mainGrid;

		// Token: 0x04000237 RID: 567
		internal Button btnUnlockBin;

		// Token: 0x04000238 RID: 568
		internal XRSImageBox imagebox;

		// Token: 0x04000239 RID: 569
		internal ListView listTestsamples;

		// Token: 0x0400023A RID: 570
		internal Grid pdfViewGrid;

		// Token: 0x0400023B RID: 571
		internal Popup popUp;

		// Token: 0x0400023C RID: 572
		internal WebBrowser webbrowser;

		// Token: 0x0400023D RID: 573
		internal Button arrowLeft;

		// Token: 0x0400023E RID: 574
		private bool _contentLoaded;
	}
}
