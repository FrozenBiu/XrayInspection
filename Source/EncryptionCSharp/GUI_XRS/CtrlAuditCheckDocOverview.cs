using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using DatabaseConnection;
using Definitions;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x02000027 RID: 39
	public class CtrlAuditCheckDocOverview : UserControl, IComponentConnector, IStyleConnector
	{
		// Token: 0x060004EA RID: 1258 RVA: 0x00022420 File Offset: 0x00020620
		public CtrlAuditCheckDocOverview(AccessRightsManager accessMan, DBConnection dbCon)
		{
			this.accessManager = accessMan;
			this.dbConnection = dbCon;
			this.InitializeComponent();
			string text = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\AuditcheckDocumentation";
			bool flag = !Directory.Exists(text);
			if (flag)
			{
				Directory.CreateDirectory(text);
			}
			this.dbConnection.GetUsbDriveLetter(ref this.usbPath);
			this.viewModel = new AuditCheckDocOverviewViewModel(text, this.usbPath, this.accessManager);
			base.DataContext = this.viewModel;
			this.InitializeButtons();
			this.SetControlRights();
		}

		// Token: 0x060004EB RID: 1259 RVA: 0x000224E0 File Offset: 0x000206E0
		public void InitializeButtons()
		{
			this.dockpanel = new DockPanel();
			this.dockpanel.HorizontalAlignment = HorizontalAlignment.Stretch;
			this.dockpanel.VerticalAlignment = VerticalAlignment.Stretch;
			Grid.SetColumn(this.dockpanel, 4);
			this.btnDeleteCertificate = new Button();
			Image image = ImageConverter.ToImage("/img/deleteDocument_small.png");
			image.Height = 40.0;
			image.Width = 40.0;
			image.Stretch = Stretch.Uniform;
			this.btnDeleteCertificate.Content = image;
			this.btnDeleteCertificate.Click += this.DeleteCertificate;
			this.btnDeleteCertificate.Style = (base.FindResource("defaultButtonStyle") as Style);
			this.btnDeleteCertificate.Margin = new Thickness(0.0, 0.0, 10.0, 0.0);
			this.btnDeleteCertificate.Visibility = Visibility.Visible;
			this.dockpanel.Children.Add(this.btnDeleteCertificate);
			DockPanel.SetDock(this.btnDeleteCertificate, Dock.Left);
			this.btnSaveCertificate = new ButtonUSB(this.usbPath, "/img/saveonusb_small.png", "/img/saveonusbsmall_nousb.png", new RoutedEventHandler(this.SaveCertificate));
			this.dockpanel.Children.Add(this.btnSaveCertificate);
			DockPanel.SetDock(this.btnSaveCertificate, Dock.Left);
		}

		// Token: 0x060004EC RID: 1260 RVA: 0x00022650 File Offset: 0x00020850
		private void showEntries(object sender, RoutedEventArgs e)
		{
			CtrlAuditCheckSavedEntries usrControl = new CtrlAuditCheckSavedEntries(this.accessManager, this.dbConnection);
			Subwindow.Show(usrControl, SubwindowButtons.BackAndHome);
		}

		// Token: 0x060004ED RID: 1261 RVA: 0x00022678 File Offset: 0x00020878
		private void DeleteCertificate(object sender, RoutedEventArgs e)
		{
			this.viewModel.DeleteFile(sender);
		}

		// Token: 0x060004EE RID: 1262 RVA: 0x00022688 File Offset: 0x00020888
		private void SaveCertificate(object sender, RoutedEventArgs e)
		{
			this.viewModel.AddFileToUSB(sender);
		}

		// Token: 0x060004EF RID: 1263 RVA: 0x00022698 File Offset: 0x00020898
		private void SetControlRights()
		{
			this.accessManager.RegisterControl(this.btnSaveCertificate, this.rightSaveDoc, this.rightSaveDoc);
			this.accessManager.RegisterControl(this.btnDeleteCertificate, this.rightDeleteDoc, this.rightDeleteDoc);
			this.accessManager.SetProperties();
		}

		// Token: 0x060004F0 RID: 1264 RVA: 0x000226F0 File Offset: 0x000208F0
		private void IVPControl_Loaded(object sender, RoutedEventArgs e)
		{
			Subwindow subwindow = Window.GetWindow(this) as Subwindow;
			subwindow.buttonGrid.Children.Remove(subwindow.buttonPanel);
			subwindow.buttonGrid.Children.Add(this.dockpanel);
		}

		// Token: 0x060004F1 RID: 1265 RVA: 0x00022738 File Offset: 0x00020938
		private void IVPControl_Unloaded(object sender, RoutedEventArgs e)
		{
			bool flag = this.wdManual != null;
			if (flag)
			{
				this.wdManual.Close();
			}
			this.accessManager.DeregisterControl(this.btnSaveCertificate);
			this.accessManager.DeregisterControl(this.btnDeleteCertificate);
			BindingOperations.ClearAllBindings(this);
			base.DataContext = null;
			this.dockpanel.Children.Clear();
			GC.Collect();
		}

		// Token: 0x060004F2 RID: 1266 RVA: 0x000227AC File Offset: 0x000209AC
		private void listBoxItem_DoubleClick(object sender, RoutedEventArgs e)
		{
			bool flag = sender is ListBoxItem;
			if (flag)
			{
				object dataContext = ((ListBoxItem)e.Source).DataContext;
				Document document = (Document)dataContext;
				bool flag2 = File.Exists(document.FilePath);
				if (flag2)
				{
					bool flag3 = this.wdManual != null;
					if (flag3)
					{
						this.wdManual.Close();
					}
					this.wdManual = new WdManual(document.FilePath);
					this.wdManual.Show();
				}
			}
		}

		// Token: 0x060004F3 RID: 1267 RVA: 0x0002282C File Offset: 0x00020A2C
		public void SelectedItems(object sender, SelectionChangedEventArgs eventArgs)
		{
			IList selectedItems = ((ListBox)sender).SelectedItems;
			this.viewModel.SelectedDocuments = selectedItems.Cast<Document>().ToList<Document>();
			bool flag = this.viewModel.SelectedDocuments != null;
			if (flag)
			{
				bool flag2 = this.viewModel.SelectedDocuments.Count > 0;
				if (flag2)
				{
					this.viewModel.SelectedDocument = this.viewModel.SelectedDocuments.Last<Document>();
				}
			}
			else
			{
				this.viewModel.SelectedDocument = null;
			}
		}

		// Token: 0x060004F4 RID: 1268 RVA: 0x000228B8 File Offset: 0x00020AB8
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/auditchecknew/ctrlauditcheckdocoverview.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x060004F5 RID: 1269 RVA: 0x000228F0 File Offset: 0x00020AF0
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			if (connectionId != 1)
			{
				if (connectionId != 2)
				{
					this._contentLoaded = true;
				}
				else
				{
					this.ListBoxDocuments = (ListBox)target;
					this.ListBoxDocuments.SelectionChanged += this.SelectedItems;
				}
			}
			else
			{
				((CtrlAuditCheckDocOverview)target).Unloaded += this.IVPControl_Unloaded;
				((CtrlAuditCheckDocOverview)target).Loaded += this.IVPControl_Loaded;
			}
		}

		// Token: 0x060004F6 RID: 1270 RVA: 0x00022970 File Offset: 0x00020B70
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IStyleConnector.Connect(int connectionId, object target)
		{
			if (connectionId == 3)
			{
				EventSetter eventSetter = new EventSetter();
				eventSetter.Event = Control.MouseDoubleClickEvent;
				eventSetter.Handler = new MouseButtonEventHandler(this.listBoxItem_DoubleClick);
				((Style)target).Setters.Add(eventSetter);
			}
		}

		// Token: 0x040003A8 RID: 936
		private AuditCheckDocOverviewViewModel viewModel;

		// Token: 0x040003A9 RID: 937
		private DockPanel dockpanel;

		// Token: 0x040003AA RID: 938
		private AccessRightsManager accessManager;

		// Token: 0x040003AB RID: 939
		private Button btnSaveCertificate;

		// Token: 0x040003AC RID: 940
		private Button btnDeleteCertificate;

		// Token: 0x040003AD RID: 941
		private BuiltInPriviledges rightDeleteDoc = BuiltInPriviledges.Operator1;

		// Token: 0x040003AE RID: 942
		private BuiltInPriviledges rightSaveDoc = BuiltInPriviledges.Operator3;

		// Token: 0x040003AF RID: 943
		private string usbPath = "";

		// Token: 0x040003B0 RID: 944
		private DBConnection dbConnection;

		// Token: 0x040003B1 RID: 945
		private WdManual wdManual;

		// Token: 0x040003B2 RID: 946
		internal ListBox ListBoxDocuments;

		// Token: 0x040003B3 RID: 947
		private bool _contentLoaded;
	}
}
