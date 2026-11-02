using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using DatabaseConnection;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x0200004A RID: 74
	public class CtrlLiveMode : UserControl, IComponentConnector
	{
		// Token: 0x17000230 RID: 560
		// (get) Token: 0x0600077A RID: 1914 RVA: 0x00030F50 File Offset: 0x0002F150
		// (set) Token: 0x0600077B RID: 1915 RVA: 0x00030F58 File Offset: 0x0002F158
		public List<Button> Buttons { get; set; }

		// Token: 0x0600077C RID: 1916 RVA: 0x00030F61 File Offset: 0x0002F161
		public CtrlLiveMode(DBConnection dbConnection, ParameterHandling parHandling, ImageDataTransfer imgTransfer, ProductManager productManager, StoreImageData imageArchiver)
		{
			this.InitializeComponent();
			this.InitializeButtons(dbConnection);
			this.viewModel = new LiveModeViewModel(dbConnection, parHandling, imgTransfer, this.imagebox, productManager, imageArchiver);
			base.DataContext = this.viewModel;
		}

		// Token: 0x0600077D RID: 1917 RVA: 0x00030FA0 File Offset: 0x0002F1A0
		private void InitializeButtons(DBConnection dbConnection)
		{
			string usbLetter = "";
			dbConnection.GetUsbDriveLetter(ref usbLetter);
			this.Buttons = new List<Button>();
			Button button = new Button();
			Image image = ImageConverter.ToImage("/img/livexriimage.png");
			image.Height = 40.0;
			image.Width = 40.0;
			image.Stretch = Stretch.Uniform;
			button.Content = image;
			button.Click += this.Record;
			button.Style = (base.FindResource("defaultButtonStyle") as Style);
			this.Buttons.Add(button);
			ButtonUSB item = new ButtonUSB(usbLetter, "/img/saveonusb_small.png", "/img/saveonusbsmall_nousb.png", new RoutedEventHandler(this.SaveToUSB));
			this.Buttons.Add(item);
		}

		// Token: 0x0600077E RID: 1918 RVA: 0x0003106B File Offset: 0x0002F26B
		public void SaveToUSB(object sender, RoutedEventArgs e)
		{
			this.viewModel.SaveToUSB();
		}

		// Token: 0x0600077F RID: 1919 RVA: 0x0003107A File Offset: 0x0002F27A
		private void Record(object sender, RoutedEventArgs e)
		{
			this.viewModel.Record();
		}

		// Token: 0x06000780 RID: 1920 RVA: 0x00031089 File Offset: 0x0002F289
		private void CtrlLiveMode_Unloaded(object sender, RoutedEventArgs e)
		{
			this.viewModel.CleanUp();
		}

		// Token: 0x06000781 RID: 1921 RVA: 0x00031098 File Offset: 0x0002F298
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/bulk/livemode/ctrllivemode.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x06000782 RID: 1922 RVA: 0x000310D0 File Offset: 0x0002F2D0
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
					this.imagebox = (XRSImageBox)target;
				}
			}
			else
			{
				((CtrlLiveMode)target).Unloaded += this.CtrlLiveMode_Unloaded;
			}
		}

		// Token: 0x04000525 RID: 1317
		private LiveModeViewModel viewModel;

		// Token: 0x04000527 RID: 1319
		internal XRSImageBox imagebox;

		// Token: 0x04000528 RID: 1320
		private bool _contentLoaded;
	}
}
