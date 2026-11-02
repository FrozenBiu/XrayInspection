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
	// Token: 0x0200006D RID: 109
	public class CtrlIVPCertificatesOverview : UserControl, IComponentConnector, IStyleConnector
	{
		// Token: 0x060009CD RID: 2509 RVA: 0x0003C260 File Offset: 0x0003A460
		public CtrlIVPCertificatesOverview(AccessRightsManager accessMan, DBConnection dbCon)
		{
			this.accessManager = accessMan;
			this.dbConnection = dbCon;
			this.InitializeComponent();
			string text = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\IVPCertificates";
			bool flag = !Directory.Exists(text);
			if (flag)
			{
				Directory.CreateDirectory(text);
			}
			this.dbConnection.GetUsbDriveLetter(ref this.usbPath);
			this.viewModel = new IVPCertificatesOverviewViewModel(text, this.usbPath, this.accessManager);
			base.DataContext = this.viewModel;
			this.InitializeLabel();
			this.InitializeButtons();
			this.SetControlRights();
		}

		// Token: 0x060009CE RID: 2510 RVA: 0x0003C31C File Offset: 0x0003A51C
		private void InitializeLabel()
		{
			this.UiElementRightAligned = new Label
			{
				Content = this.viewModel.StrDocuments.LocalizedText,
				FontSize = 30.0,
				FontWeight = FontWeights.Bold,
				FontFamily = new FontFamily("Verdana"),
				Foreground = Brushes.DarkGray,
				VerticalAlignment = VerticalAlignment.Center,
				HorizontalAlignment = HorizontalAlignment.Right,
				Margin = new Thickness(0.0, 0.0, 20.0, 0.0)
			};
		}

		// Token: 0x060009CF RID: 2511 RVA: 0x0003C3C8 File Offset: 0x0003A5C8
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

		// Token: 0x060009D0 RID: 2512 RVA: 0x0003C538 File Offset: 0x0003A738
		private void DeleteCertificate(object sender, RoutedEventArgs e)
		{
			this.viewModel.DeleteFile(sender);
		}

		// Token: 0x060009D1 RID: 2513 RVA: 0x0003C548 File Offset: 0x0003A748
		private void SaveCertificate(object sender, RoutedEventArgs e)
		{
			this.viewModel.AddFileToUSB(sender);
		}

		// Token: 0x060009D2 RID: 2514 RVA: 0x0003C558 File Offset: 0x0003A758
		private void SetControlRights()
		{
			this.accessManager.RegisterControl(this.btnSaveCertificate, this.rightCertificates, this.rightCertificates);
			this.accessManager.RegisterControl(this.btnDeleteCertificate, this.rightCertificates, this.rightCertificates);
			this.accessManager.SetProperties();
		}

		// Token: 0x060009D3 RID: 2515 RVA: 0x0003C5B0 File Offset: 0x0003A7B0
		private void IVPControl_Loaded(object sender, RoutedEventArgs e)
		{
			Subwindow subwindow = Window.GetWindow(this) as Subwindow;
			subwindow.buttonGrid.Children.Remove(subwindow.buttonPanel);
			subwindow.buttonGrid.Children.Add(this.dockpanel);
		}

		// Token: 0x060009D4 RID: 2516 RVA: 0x0003C5F8 File Offset: 0x0003A7F8
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

		// Token: 0x060009D5 RID: 2517 RVA: 0x0003C66C File Offset: 0x0003A86C
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

		// Token: 0x060009D6 RID: 2518 RVA: 0x0003C6EC File Offset: 0x0003A8EC
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

		// Token: 0x060009D7 RID: 2519 RVA: 0x0003C778 File Offset: 0x0003A978
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/integrated%20validation%20process/ctrlivpcertificatesoverview.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x060009D8 RID: 2520 RVA: 0x0003C7B0 File Offset: 0x0003A9B0
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
				((CtrlIVPCertificatesOverview)target).Unloaded += this.IVPControl_Unloaded;
				((CtrlIVPCertificatesOverview)target).Loaded += this.IVPControl_Loaded;
			}
		}

		// Token: 0x060009D9 RID: 2521 RVA: 0x0003C830 File Offset: 0x0003AA30
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

		// Token: 0x040006B4 RID: 1716
		private IVPCertificatesOverviewViewModel viewModel;

		// Token: 0x040006B5 RID: 1717
		private DockPanel dockpanel;

		// Token: 0x040006B6 RID: 1718
		private AccessRightsManager accessManager;

		// Token: 0x040006B7 RID: 1719
		private Button btnSaveCertificate;

		// Token: 0x040006B8 RID: 1720
		private Button btnDeleteCertificate;

		// Token: 0x040006B9 RID: 1721
		private BuiltInPriviledges rightCertificates = BuiltInPriviledges.Admin;

		// Token: 0x040006BA RID: 1722
		private string usbPath = "";

		// Token: 0x040006BB RID: 1723
		private DBConnection dbConnection;

		// Token: 0x040006BC RID: 1724
		private WdManual wdManual;

		// Token: 0x040006BD RID: 1725
		public UIElement UiElementRightAligned;

		// Token: 0x040006BE RID: 1726
		internal ListBox ListBoxDocuments;

		// Token: 0x040006BF RID: 1727
		private bool _contentLoaded;
	}
}
