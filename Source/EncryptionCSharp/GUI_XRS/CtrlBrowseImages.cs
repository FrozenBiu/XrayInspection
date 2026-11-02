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
	// Token: 0x02000045 RID: 69
	public class CtrlBrowseImages : UserControl, IComponentConnector
	{
		// Token: 0x060006E8 RID: 1768 RVA: 0x0002D442 File Offset: 0x0002B642
		public CtrlBrowseImages(DBConnection dbConnection, ParameterHandling parHandling, ImageDataTransfer imageTransfer, MachineState machineState, ProductManager productManager, StoreImageData imageArchiver)
		{
			this.InitializeComponent();
			this.viewModel = new BrowseImagesViewModel(dbConnection, parHandling, imageTransfer, machineState, productManager, imageArchiver);
			this.InitializeButtons(dbConnection);
			base.DataContext = this.viewModel;
		}

		// Token: 0x060006E9 RID: 1769 RVA: 0x0002D47C File Offset: 0x0002B67C
		private void InitializeButtons(DBConnection dbConnection)
		{
			string usbLetter = "";
			dbConnection.GetUsbDriveLetter(ref usbLetter);
			this.Buttons = new List<Button>();
			Button button = new Button();
			button.Content = ImageConverter.ToImage("/img/viewimage_small.png");
			button.Style = (base.FindResource("defaultButtonStyle") as Style);
			button.Click += this.btnShow_Click;
			this.Buttons.Add(button);
			Button button2 = new Button();
			Image image = ImageConverter.ToImage("/img/deleteimage_small.png");
			image.Height = 40.0;
			image.Width = 40.0;
			image.Stretch = Stretch.Uniform;
			button2.Content = image;
			button2.Style = (base.FindResource("defaultButtonStyle") as Style);
			button2.Click += this.btnDelete_Click;
			this.Buttons.Add(button2);
			Button button3 = new Button();
			Image image2 = ImageConverter.ToImage("/img/deletexriimagelist_small.png");
			image2.Height = 40.0;
			image2.Width = 40.0;
			image2.Stretch = Stretch.Uniform;
			button3.Content = image2;
			button3.Style = (base.FindResource("defaultButtonStyle") as Style);
			button3.Click += this.DeleteAllImages;
			this.Buttons.Add(button3);
			ButtonUSB item = new ButtonUSB(usbLetter, "/img/saveonusb_small.png", "/img/saveonusbsmall_nousb.png", new RoutedEventHandler(this.SaveToUSB));
			this.Buttons.Add(item);
		}

		// Token: 0x060006EA RID: 1770 RVA: 0x0002D618 File Offset: 0x0002B818
		private void btnTrain_Click(object sender, RoutedEventArgs e)
		{
			this.viewModel.Train();
		}

		// Token: 0x060006EB RID: 1771 RVA: 0x0002D627 File Offset: 0x0002B827
		private void btnDelete_Click(object sender, RoutedEventArgs e)
		{
			this.viewModel.DeleteSelectedItem();
		}

		// Token: 0x060006EC RID: 1772 RVA: 0x0002D636 File Offset: 0x0002B836
		private void btnShow_Click(object sender, RoutedEventArgs e)
		{
			this.viewModel.ShowSelectedImage();
		}

		// Token: 0x060006ED RID: 1773 RVA: 0x0002D645 File Offset: 0x0002B845
		private void DeleteAllImages(object sender, RoutedEventArgs e)
		{
			this.viewModel.DeleteAllImages();
		}

		// Token: 0x060006EE RID: 1774 RVA: 0x0002D654 File Offset: 0x0002B854
		public void SaveToUSB(object sender, RoutedEventArgs e)
		{
			this.viewModel.SaveToUSB();
		}

		// Token: 0x060006EF RID: 1775 RVA: 0x0002D664 File Offset: 0x0002B864
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/bulk/browse/ctrlbrowseimages.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x060006F0 RID: 1776 RVA: 0x0002D69C File Offset: 0x0002B89C
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
				this.lstImages = (ListBox)target;
			}
		}

		// Token: 0x040004CB RID: 1227
		private BrowseImagesViewModel viewModel;

		// Token: 0x040004CC RID: 1228
		public List<Button> Buttons;

		// Token: 0x040004CD RID: 1229
		internal ListBox lstImages;

		// Token: 0x040004CE RID: 1230
		private bool _contentLoaded;
	}
}
