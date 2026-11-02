using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using DatabaseConnection;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x02000022 RID: 34
	public class CtrlAuditCheckSavedEntries : UserControl, IComponentConnector
	{
		// Token: 0x060003FE RID: 1022 RVA: 0x0001A328 File Offset: 0x00018528
		public CtrlAuditCheckSavedEntries(AccessRightsManager accessManager, DBConnection dbConnection)
		{
			this.accessManager = accessManager;
			this.dbConnection = dbConnection;
			this.InitializeComponent();
			this.InitializeButtons();
			this.SetControlRights();
			this.viewModel = new AuditCheckSavedEntries(dbConnection);
			base.DataContext = this.viewModel;
		}

		// Token: 0x060003FF RID: 1023 RVA: 0x0001A390 File Offset: 0x00018590
		public void InitializeButtons()
		{
			this.dbConnection.GetUsbDriveLetter(ref this.usbPath);
			this.dockpanel = new DockPanel();
			this.dockpanel.HorizontalAlignment = HorizontalAlignment.Stretch;
			this.dockpanel.VerticalAlignment = VerticalAlignment.Stretch;
			Grid.SetColumn(this.dockpanel, 4);
			this.btnMakeDoc = new ButtonUSB(this.usbPath, "/img/usb.png", "/img/usb_disabled.png", new RoutedEventHandler(this.drawList), true);
			this.dockpanel.Children.Add(this.btnMakeDoc);
			DockPanel.SetDock(this.btnMakeDoc, Dock.Left);
		}

		// Token: 0x06000400 RID: 1024 RVA: 0x0001A430 File Offset: 0x00018630
		private void drawList(object sender, RoutedEventArgs e)
		{
			bool saveEnabled = this.viewModel.SaveEnabled;
			if (saveEnabled)
			{
				string text = this.viewModel.drawList(this.viewModel.Entries, this.usbPath);
				this.viewModel.BrowserVisible = true;
				bool flag = File.Exists(text);
				if (flag)
				{
					WdManual wdManual = new WdManual(text);
					wdManual.Show();
					Dialog.Show(LocalizationManager.Localize("txtInfo"), LocalizationManager.Localize("txtSaveSuccessful"), DialogStyle.Info, DialogButtons.OK, false);
				}
			}
			else
			{
				Dialog.Show(LocalizationManager.Localize("txtInfo"), LocalizationManager.Localize("txtNotAllEntriesLoaded"), DialogStyle.Info, DialogButtons.OK, false);
			}
		}

		// Token: 0x06000401 RID: 1025 RVA: 0x0001092C File Offset: 0x0000EB2C
		private void Sign(object sender, RoutedEventArgs e)
		{
		}

		// Token: 0x06000402 RID: 1026 RVA: 0x0001A4D1 File Offset: 0x000186D1
		private void SetControlRights()
		{
			this.accessManager.RegisterControl(this.btnMakeDoc, this.rightEntries, this.rightEntries);
			this.accessManager.SetProperties();
		}

		// Token: 0x06000403 RID: 1027 RVA: 0x0001A500 File Offset: 0x00018700
		private void Control_Loaded(object sender, RoutedEventArgs e)
		{
			Subwindow subwindow = Window.GetWindow(this) as Subwindow;
			subwindow.buttonGrid.Children.Remove(subwindow.buttonPanel);
			subwindow.buttonGrid.Children.Add(this.dockpanel);
		}

		// Token: 0x06000404 RID: 1028 RVA: 0x0001A548 File Offset: 0x00018748
		private void Control_Unloaded(object sender, RoutedEventArgs e)
		{
			this.accessManager.DeregisterControl(this.btnMakeDoc);
			this.viewModel.CleanUp(null);
			BindingOperations.ClearAllBindings(this);
			base.DataContext = null;
			this.dockpanel.Children.Clear();
			GC.Collect();
		}

		// Token: 0x06000405 RID: 1029 RVA: 0x0001A59C File Offset: 0x0001879C
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/auditchecknew/ctrlauditchecksavedentries.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x06000406 RID: 1030 RVA: 0x0001A5D4 File Offset: 0x000187D4
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
				((CtrlAuditCheckSavedEntries)target).Loaded += this.Control_Loaded;
				((CtrlAuditCheckSavedEntries)target).Unloaded += this.Control_Unloaded;
			}
		}

		// Token: 0x0400030A RID: 778
		private AuditCheckSavedEntries viewModel;

		// Token: 0x0400030B RID: 779
		private AccessRightsManager accessManager;

		// Token: 0x0400030C RID: 780
		private DockPanel dockpanel;

		// Token: 0x0400030D RID: 781
		private ButtonUSB btnMakeDoc;

		// Token: 0x0400030E RID: 782
		private BuiltInPriviledges rightEntries = BuiltInPriviledges.Operator3;

		// Token: 0x0400030F RID: 783
		private string usbPath = "";

		// Token: 0x04000310 RID: 784
		private DBConnection dbConnection;

		// Token: 0x04000311 RID: 785
		private bool _contentLoaded;
	}
}
