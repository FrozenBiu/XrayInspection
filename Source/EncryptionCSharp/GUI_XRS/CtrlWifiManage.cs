using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using DatabaseConnection;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x020000F9 RID: 249
	public class CtrlWifiManage : UserControl, IComponentConnector, IStyleConnector
	{
		// Token: 0x1700072C RID: 1836
		// (get) Token: 0x060017EA RID: 6122 RVA: 0x00090578 File Offset: 0x0008E778
		// (set) Token: 0x060017EB RID: 6123 RVA: 0x00090580 File Offset: 0x0008E780
		public List<Button> Buttons { get; set; }

		// Token: 0x060017EC RID: 6124 RVA: 0x0009058C File Offset: 0x0008E78C
		public CtrlWifiManage(WifiClient wifiClient, DBConnection dbConnection)
		{
			this.InitializeComponent();
			this.viewModel = new WifiViewModel(wifiClient, dbConnection);
			this.InitializeButtons();
			this.wifiConnectIcons = new WifiConnectIcons(this.btnConnectOrDisconnect);
			this.viewModel.PropertyChanged += this.viewModel_PropertyChanged;
			this.wifiConnectIcons.SetConnectedIcon(this.viewModel.NAvailableNetworks, this.viewModel.ConnectedNetwork);
			base.DataContext = this.viewModel;
		}

		// Token: 0x060017ED RID: 6125 RVA: 0x00090614 File Offset: 0x0008E814
		private void InitializeButtons()
		{
			this.Buttons = new List<Button>();
			this.btnConnectOrDisconnect = new Button();
			this.btnConnectOrDisconnect.Command = this.viewModel.ConnectOrDisconnectCommand;
			this.btnConnectOrDisconnect.Style = (base.FindResource("defaultButtonStyle") as Style);
			this.Buttons.Add(this.btnConnectOrDisconnect);
			Button button = new Button();
			button.Command = this.viewModel.AddNetworkCommand;
			Image image = ImageConverter.ToImage("/img/wifiAdd.png");
			image.Height = 38.0;
			image.Stretch = Stretch.Uniform;
			button.Content = image;
			button.Style = (base.FindResource("defaultButtonStyle") as Style);
			this.Buttons.Add(button);
		}

		// Token: 0x060017EE RID: 6126 RVA: 0x000906E8 File Offset: 0x0008E8E8
		private void viewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			bool flag = e.PropertyName == "ConnectedNetwork";
			if (flag)
			{
				this.wifiConnectIcons.SetConnectedIcon(this.viewModel.NAvailableNetworks, this.viewModel.ConnectedNetwork);
			}
		}

		// Token: 0x060017EF RID: 6127 RVA: 0x00090730 File Offset: 0x0008E930
		private void listBoxItem_DoubleClick(object sender, RoutedEventArgs e)
		{
			bool flag = sender is ListBoxItem && ((ListBoxItem)sender).IsSelected;
			if (flag)
			{
				this.viewModel.Connect(null);
			}
		}

		// Token: 0x060017F0 RID: 6128 RVA: 0x00090767 File Offset: 0x0008E967
		private void CtrlWifiManage_Unloaded(object sender, RoutedEventArgs e)
		{
			this.viewModel.PropertyChanged -= this.viewModel_PropertyChanged;
			this.viewModel.CleanUp();
		}

		// Token: 0x060017F1 RID: 6129 RVA: 0x00090790 File Offset: 0x0008E990
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/wifi/ctrlwifimanage.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x060017F2 RID: 6130 RVA: 0x000907C8 File Offset: 0x0008E9C8
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
				((CtrlWifiManage)target).Unloaded += this.CtrlWifiManage_Unloaded;
			}
		}

		// Token: 0x060017F3 RID: 6131 RVA: 0x00090804 File Offset: 0x0008EA04
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IStyleConnector.Connect(int connectionId, object target)
		{
			if (connectionId == 2)
			{
				EventSetter eventSetter = new EventSetter();
				eventSetter.Event = Control.MouseDoubleClickEvent;
				eventSetter.Handler = new MouseButtonEventHandler(this.listBoxItem_DoubleClick);
				((Style)target).Setters.Add(eventSetter);
			}
		}

		// Token: 0x04000FA6 RID: 4006
		private WifiViewModel viewModel;

		// Token: 0x04000FA7 RID: 4007
		private Button btnConnectOrDisconnect;

		// Token: 0x04000FA8 RID: 4008
		private WifiConnectIcons wifiConnectIcons;

		// Token: 0x04000FAA RID: 4010
		private bool _contentLoaded;
	}
}
