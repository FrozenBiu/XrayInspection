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
	// Token: 0x020000AD RID: 173
	public class CtrlCanFilter : UserControl, IComponentConnector
	{
		// Token: 0x060011B4 RID: 4532 RVA: 0x0006AE54 File Offset: 0x00069054
		public CtrlCanFilter(ParameterHandling parHandling, ImageDataTransfer imageTransfer, DBConnection dbConnection, ProductManager productManager, MachineState machineState, int imageGroup, StoreImageData imageArchiver)
		{
			this.InitializeComponent();
			this.InitializeButtons();
			this.imagebox.IncreaseInitialBrightness(5);
			this.viewModel = new CanFilterViewModel(parHandling, imageTransfer, dbConnection, productManager, machineState, imageGroup, imageArchiver);
			base.DataContext = this.viewModel;
		}

		// Token: 0x060011B5 RID: 4533 RVA: 0x0006AEA8 File Offset: 0x000690A8
		private void InitializeButtons()
		{
			this.Buttons = new List<Button>();
			Button button = new Button();
			Image image = ImageConverter.ToImage("/img/learn.png");
			image.Width = 40.0;
			image.Height = 40.0;
			image.Stretch = Stretch.Uniform;
			button.Content = image;
			button.Style = (base.FindResource("defaultButtonStyle") as Style);
			button.Click += this.btnLearn_Click;
			this.Buttons.Add(button);
		}

		// Token: 0x060011B6 RID: 4534 RVA: 0x0006AF39 File Offset: 0x00069139
		private void btnLearn_Click(object sender, RoutedEventArgs e)
		{
			this.viewModel.ShowLearnWindow();
		}

		// Token: 0x060011B7 RID: 4535 RVA: 0x0006AF48 File Offset: 0x00069148
		private void Slider_PreviewMouseUp(object sender, MouseButtonEventArgs e)
		{
			this.viewModel.SendImage();
		}

		// Token: 0x060011B8 RID: 4536 RVA: 0x0006AF58 File Offset: 0x00069158
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/filter/ctrlcanfilter.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x060011B9 RID: 4537 RVA: 0x0006AF90 File Offset: 0x00069190
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				((Slider)target).PreviewMouseUp += this.Slider_PreviewMouseUp;
				break;
			case 2:
				((Slider)target).PreviewMouseUp += this.Slider_PreviewMouseUp;
				break;
			case 3:
				((Slider)target).PreviewMouseUp += this.Slider_PreviewMouseUp;
				break;
			case 4:
				((Slider)target).PreviewMouseUp += this.Slider_PreviewMouseUp;
				break;
			case 5:
				((Slider)target).PreviewMouseUp += this.Slider_PreviewMouseUp;
				break;
			case 6:
				this.imagebox = (XRSImageBox)target;
				break;
			default:
				this._contentLoaded = true;
				break;
			}
		}

		// Token: 0x04000BB5 RID: 2997
		private CanFilterViewModel viewModel;

		// Token: 0x04000BB6 RID: 2998
		public List<Button> Buttons;

		// Token: 0x04000BB7 RID: 2999
		internal XRSImageBox imagebox;

		// Token: 0x04000BB8 RID: 3000
		private bool _contentLoaded;
	}
}
