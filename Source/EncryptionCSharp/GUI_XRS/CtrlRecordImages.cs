using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Threading;
using DatabaseConnection;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x0200004C RID: 76
	public class CtrlRecordImages : UserControl, IComponentConnector
	{
		// Token: 0x17000231 RID: 561
		// (get) Token: 0x0600078B RID: 1931 RVA: 0x00031430 File Offset: 0x0002F630
		// (set) Token: 0x0600078C RID: 1932 RVA: 0x00031438 File Offset: 0x0002F638
		public List<Button> Buttons { get; set; }

		// Token: 0x0600078D RID: 1933 RVA: 0x00031444 File Offset: 0x0002F644
		public CtrlRecordImages(DBConnection dbConnection, ParameterHandling parHandling, ImageDataTransfer imageTransfer, MachineState machineState, ProductManager productManager, Vibro vibro, AccessRightsManager accessManager)
		{
			this.InitializeComponent();
			this.InitializeButtons();
			this.viewModel = new RecordImagesViewModel(this.imagebox, dbConnection, parHandling, imageTransfer, machineState, productManager, vibro, accessManager);
			this.viewModel.MaxImagesSaved += this.viewModel_MaxImagesSaved;
			base.DataContext = this.viewModel;
		}

		// Token: 0x0600078E RID: 1934 RVA: 0x000314A8 File Offset: 0x0002F6A8
		private void InitializeButtons()
		{
			this.Buttons = new List<Button>();
			Button button = new Button();
			button.Content = ImageConverter.ToImage("/img/cancel.png");
			button.Click += this.StopRecording;
			button.Style = (base.FindResource("cancelButtonStyle") as Style);
			this.Buttons.Add(button);
		}

		// Token: 0x0600078F RID: 1935 RVA: 0x00031510 File Offset: 0x0002F710
		private void StopRecording(object sender, RoutedEventArgs e)
		{
			this.CloseWindowDelayed();
		}

		// Token: 0x06000790 RID: 1936 RVA: 0x0003151A File Offset: 0x0002F71A
		public void CloseSubWindow()
		{
			Thread.Sleep(1500);
			base.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
			{
				(Window.GetWindow(this) as Subwindow).Close();
			}));
		}

		// Token: 0x06000791 RID: 1937 RVA: 0x00031542 File Offset: 0x0002F742
		private void CtrlRecordImages_Unloaded(object sender, RoutedEventArgs e)
		{
			this.viewModel.CleanUp();
		}

		// Token: 0x06000792 RID: 1938 RVA: 0x00031551 File Offset: 0x0002F751
		private void viewModel_MaxImagesSaved()
		{
			base.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(delegate()
			{
				this.viewModel.RecordText = LocalizationManager.Localize("txtRecordImagesStopped");
			}));
			this.CloseWindowDelayed();
		}

		// Token: 0x06000793 RID: 1939 RVA: 0x00031578 File Offset: 0x0002F778
		private void CloseWindowDelayed()
		{
			Thread thread = new Thread(new ThreadStart(this.CloseSubWindow));
			thread.Start();
		}

		// Token: 0x06000794 RID: 1940 RVA: 0x000315A0 File Offset: 0x0002F7A0
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/bulk/record/ctrlrecordimages.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x06000795 RID: 1941 RVA: 0x000315D8 File Offset: 0x0002F7D8
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		internal Delegate _CreateDelegate(Type delegateType, string handler)
		{
			return Delegate.CreateDelegate(delegateType, this, handler);
		}

		// Token: 0x06000796 RID: 1942 RVA: 0x000315F4 File Offset: 0x0002F7F4
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
				((CtrlRecordImages)target).Unloaded += this.CtrlRecordImages_Unloaded;
			}
		}

		// Token: 0x04000532 RID: 1330
		private RecordImagesViewModel viewModel;

		// Token: 0x04000534 RID: 1332
		internal XRSImageBox imagebox;

		// Token: 0x04000535 RID: 1333
		private bool _contentLoaded;
	}
}
