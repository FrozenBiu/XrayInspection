using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
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
	// Token: 0x02000067 RID: 103
	public class CtrlIVPCertificatesContent : UserControl, IComponentConnector, IStyleConnector
	{
		// Token: 0x06000988 RID: 2440 RVA: 0x0003A970 File Offset: 0x00038B70
		public CtrlIVPCertificatesContent(DBConnection dBCon, AccessRightsManager manager, ProductManager productManager)
		{
			this.InitializeComponent();
			this.viewModel = new IVPCertificatesContentViewModel(dBCon, manager, productManager);
			base.DataContext = this.viewModel;
			this.accessManager = manager;
			this.dBConncection = dBCon;
			this.InitializeLabel();
			bool flag = this.accessManager.CurrentUser.ID < 0;
			if (flag)
			{
				this.InitializeButtons();
				this.SetControlRights();
			}
		}

		// Token: 0x06000989 RID: 2441 RVA: 0x0003A9F0 File Offset: 0x00038BF0
		private void InitializeLabel()
		{
			this.UiElementRightAligned = new Label
			{
				Content = this.viewModel.StrContent.LocalizedText,
				FontSize = 30.0,
				FontWeight = FontWeights.Bold,
				FontFamily = new FontFamily("Verdana"),
				Foreground = Brushes.DarkGray,
				VerticalAlignment = VerticalAlignment.Center,
				HorizontalAlignment = HorizontalAlignment.Right,
				Margin = new Thickness(0.0, 0.0, 20.0, 0.0)
			};
		}

		// Token: 0x0600098A RID: 2442 RVA: 0x0003AA9C File Offset: 0x00038C9C
		public void InitializeButtons()
		{
			this.dockpanel = new DockPanel();
			this.dockpanel.HorizontalAlignment = HorizontalAlignment.Stretch;
			this.dockpanel.VerticalAlignment = VerticalAlignment.Stretch;
			Grid.SetColumn(this.dockpanel, 4);
			this.btnManufacturerData = new Button();
			this.btnManufacturerData.Content = ImageConverter.ToImage("/img/producer_settings.png");
			this.btnManufacturerData.Click += this.OpenManufacturerData;
			this.btnManufacturerData.Style = (base.FindResource("defaultButtonStyle") as Style);
			this.btnManufacturerData.Margin = new Thickness(0.0, 0.0, 10.0, 0.0);
			this.btnManufacturerData.Visibility = Visibility.Visible;
			this.dockpanel.Children.Add(this.btnManufacturerData);
			DockPanel.SetDock(this.btnManufacturerData, Dock.Left);
		}

		// Token: 0x0600098B RID: 2443 RVA: 0x0003AB97 File Offset: 0x00038D97
		private void SetControlRights()
		{
			this.accessManager.RegisterControl(this.btnManufacturerData, this.rightManufacturerData, this.rightManufacturerData);
			this.accessManager.SetProperties();
		}

		// Token: 0x0600098C RID: 2444 RVA: 0x0003ABC4 File Offset: 0x00038DC4
		private void OpenManufacturerData(object sender, RoutedEventArgs e)
		{
			CtrlIVPCertificatesManufacturer ctrlIVPCertificatesManufacturer = new CtrlIVPCertificatesManufacturer(this.dBConncection);
			Subwindow.Show(ctrlIVPCertificatesManufacturer, SubwindowButtons.BackAndHome, ctrlIVPCertificatesManufacturer.UiElementRightAligned);
		}

		// Token: 0x0600098D RID: 2445 RVA: 0x0003ABEC File Offset: 0x00038DEC
		private void IVPControl_Loaded(object sender, RoutedEventArgs e)
		{
			bool flag = this.accessManager.CurrentUser.ID < 0;
			if (flag)
			{
				Subwindow subwindow = Window.GetWindow(this) as Subwindow;
				subwindow.buttonGrid.Children.Remove(subwindow.buttonPanel);
				subwindow.buttonGrid.Children.Add(this.dockpanel);
			}
		}

		// Token: 0x0600098E RID: 2446 RVA: 0x0003AC50 File Offset: 0x00038E50
		private void IVPControl_Unloaded(object sender, RoutedEventArgs e)
		{
			this.viewModel.DeregisterLocalizedText();
			this.viewModel.ProductItems.Clear();
			this.viewModel.ProductItems = null;
			this.viewModel.CustomerItems.Clear();
			this.viewModel.CustomerItems = null;
			BindingOperations.ClearAllBindings(this);
			base.DataContext = null;
			bool flag = this.accessManager.CurrentUser.ID < 0;
			if (flag)
			{
				this.accessManager.DeregisterControl(this.btnManufacturerData);
				this.dockpanel.Children.Clear();
			}
			GC.Collect();
		}

		// Token: 0x0600098F RID: 2447 RVA: 0x0003ACF8 File Offset: 0x00038EF8
		private void TextBox_MouseUp(object sender, MouseButtonEventArgs e)
		{
			bool flag = e.Source is TextBox;
			if (flag)
			{
				Infos infos = (Infos)((TextBox)e.Source).DataContext;
				bool flag2 = infos.Description.Equals(this.viewModel.StrEmail.LocalizedText + ":");
				if (flag2)
				{
					this.viewModel.ShowEditKeyboardEmail(infos);
				}
				else
				{
					this.viewModel.ShowEditKeyboard(infos);
				}
			}
		}

		// Token: 0x06000990 RID: 2448 RVA: 0x0003AD7C File Offset: 0x00038F7C
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/integrated%20validation%20process/ctrlivpcertificatescontent.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x06000991 RID: 2449 RVA: 0x0003ADB4 File Offset: 0x00038FB4
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
					this.image = (Image)target;
				}
			}
			else
			{
				((CtrlIVPCertificatesContent)target).Unloaded += this.IVPControl_Unloaded;
				((CtrlIVPCertificatesContent)target).Loaded += this.IVPControl_Loaded;
			}
		}

		// Token: 0x06000992 RID: 2450 RVA: 0x0003AE1C File Offset: 0x0003901C
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IStyleConnector.Connect(int connectionId, object target)
		{
			if (connectionId != 3)
			{
				if (connectionId == 4)
				{
					((TextBox)target).MouseUp += this.TextBox_MouseUp;
				}
			}
			else
			{
				((TextBox)target).MouseUp += this.TextBox_MouseUp;
			}
		}

		// Token: 0x04000688 RID: 1672
		private IVPCertificatesContentViewModel viewModel;

		// Token: 0x04000689 RID: 1673
		private BuiltInPriviledges rightManufacturerData = BuiltInPriviledges.Service;

		// Token: 0x0400068A RID: 1674
		private DockPanel dockpanel;

		// Token: 0x0400068B RID: 1675
		private AccessRightsManager accessManager;

		// Token: 0x0400068C RID: 1676
		private Button btnManufacturerData;

		// Token: 0x0400068D RID: 1677
		private DBConnection dBConncection;

		// Token: 0x0400068E RID: 1678
		public UIElement UiElementRightAligned;

		// Token: 0x0400068F RID: 1679
		internal Image image;

		// Token: 0x04000690 RID: 1680
		private bool _contentLoaded;
	}
}
