using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x020000F7 RID: 247
	public class CtrlWifiAddNetwork : UserControl, IComponentConnector
	{
		// Token: 0x17000729 RID: 1833
		// (get) Token: 0x060017D1 RID: 6097 RVA: 0x0008FC43 File Offset: 0x0008DE43
		// (set) Token: 0x060017D2 RID: 6098 RVA: 0x0008FC4B File Offset: 0x0008DE4B
		public List<Button> Buttons { get; set; }

		// Token: 0x060017D3 RID: 6099 RVA: 0x0008FC54 File Offset: 0x0008DE54
		public CtrlWifiAddNetwork(WifiClient wifiClient)
		{
			this.InitializeComponent();
			this.viewModel = new WifiAddNetworkViewModel(wifiClient);
			this.InitializeButtons();
			base.DataContext = this.viewModel;
		}

		// Token: 0x060017D4 RID: 6100 RVA: 0x0008FC88 File Offset: 0x0008DE88
		private void InitializeButtons()
		{
			this.Buttons = new List<Button>();
			Button button = new Button();
			button.Click += this.btnAddNetwork_Click;
			Image image = ImageConverter.ToImage("/img/wifiAdd.png");
			image.Height = 38.0;
			image.Stretch = Stretch.Uniform;
			button.Content = image;
			button.Style = (base.FindResource("defaultButtonStyle") as Style);
			this.Buttons.Add(button);
		}

		// Token: 0x060017D5 RID: 6101 RVA: 0x0008FD0C File Offset: 0x0008DF0C
		private void btnAddNetwork_Click(object sender, RoutedEventArgs e)
		{
			this.viewModel.CommitNewNetwork();
			Subwindow subwindow = Window.GetWindow(this) as Subwindow;
			bool flag = subwindow != null;
			if (flag)
			{
				subwindow.Close();
			}
		}

		// Token: 0x060017D6 RID: 6102 RVA: 0x0008FD44 File Offset: 0x0008DF44
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/wifi/ctrlwifiaddnetwork.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x060017D7 RID: 6103 RVA: 0x0008FD7A File Offset: 0x0008DF7A
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			this._contentLoaded = true;
		}

		// Token: 0x04000F97 RID: 3991
		private WifiAddNetworkViewModel viewModel;

		// Token: 0x04000F99 RID: 3993
		private bool _contentLoaded;
	}
}
