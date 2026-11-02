using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x020000B1 RID: 177
	public class CtrlLearnCanFilter : UserControl, IComponentConnector
	{
		// Token: 0x060011C8 RID: 4552 RVA: 0x0006B544 File Offset: 0x00069744
		public CtrlLearnCanFilter(XRSImage currentImage, ParameterHandling parHandling, ImageDataTransfer imageTransfer, int imageGroup, CanFilter canFilter)
		{
			this.InitializeComponent();
			this.InitializeButtons();
			this.viewModel = new LearnCanFilterViewModel(currentImage, this.imagebox, parHandling, imageTransfer, imageGroup, canFilter);
			this.viewModel.LearnFinished += this.viewModel_LearnFinished;
			this.viewModel.CrossRemovedOrAdded += this.viewModel_CrossRemovedOrAdded;
			base.DataContext = this.viewModel;
		}

		// Token: 0x060011C9 RID: 4553 RVA: 0x0006B5BC File Offset: 0x000697BC
		private void InitializeButtons()
		{
			this.Buttons = new List<Button>();
			this.btnBack = new Button();
			this.btnBack.Content = ImageConverter.ToImage("/img/arrow-left.png");
			this.btnBack.Click += this.RemoveLastCross;
			this.btnBack.Style = (base.FindResource("defaultButtonStyle") as Style);
			this.btnBack.Visibility = Visibility.Collapsed;
			this.Buttons.Add(this.btnBack);
			this.moveImage = ImageConverter.ToImage("/img/moveImage.png");
			this.moveImage.Height = 40.0;
			this.moveImage.Width = 40.0;
			this.moveImage.Stretch = Stretch.Uniform;
			this.crossSelectImage = ImageConverter.ToImage("/img/crossDotted.png");
			this.crossSelectImage.Height = 40.0;
			this.crossSelectImage.Width = 40.0;
			this.crossSelectImage.Stretch = Stretch.Uniform;
			this.btnMoveOrDrawCross = new Button();
			this.btnMoveOrDrawCross.Content = this.crossSelectImage;
			this.btnMoveOrDrawCross.Click += this.ChangeToMoveOrDrawCross;
			this.btnMoveOrDrawCross.Style = (base.FindResource("defaultButtonStyle") as Style);
			this.Buttons.Add(this.btnMoveOrDrawCross);
		}

		// Token: 0x060011CA RID: 4554 RVA: 0x0006B73C File Offset: 0x0006993C
		private void ChangeToMoveOrDrawCross(object sender, RoutedEventArgs e)
		{
			bool flag = this.imagebox.BoxSelectionMode == XRSImageBox.SelectionMode.DrawCrosses;
			if (flag)
			{
				this.btnMoveOrDrawCross.Content = this.moveImage;
				this.imagebox.BoxSelectionMode = XRSImageBox.SelectionMode.PanAndZoom;
			}
			else
			{
				this.btnMoveOrDrawCross.Content = this.crossSelectImage;
				this.imagebox.BoxSelectionMode = XRSImageBox.SelectionMode.DrawCrosses;
			}
		}

		// Token: 0x060011CB RID: 4555 RVA: 0x0006B7A0 File Offset: 0x000699A0
		private void RemoveLastCross(object sender, RoutedEventArgs e)
		{
			this.viewModel.RemoveLastCross();
		}

		// Token: 0x060011CC RID: 4556 RVA: 0x0006B7B0 File Offset: 0x000699B0
		private void viewModel_LearnFinished()
		{
			this.viewModel.LearnFinished -= this.viewModel_LearnFinished;
			this.viewModel.CrossRemovedOrAdded -= this.viewModel_CrossRemovedOrAdded;
			Thread thread = new Thread(new ThreadStart(this.CloseSubWindow));
			thread.Start();
		}

		// Token: 0x060011CD RID: 4557 RVA: 0x0006B807 File Offset: 0x00069A07
		public void CloseSubWindow()
		{
			Thread.Sleep(1500);
			base.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
			{
				(Window.GetWindow(this) as Subwindow).Close();
			}));
		}

		// Token: 0x060011CE RID: 4558 RVA: 0x0006B82F File Offset: 0x00069A2F
		private void viewModel_CrossRemovedOrAdded(int numCrosses)
		{
			this.btnBack.Visibility = ((numCrosses > 0) ? Visibility.Visible : Visibility.Collapsed);
		}

		// Token: 0x060011CF RID: 4559 RVA: 0x0006B848 File Offset: 0x00069A48
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/filter/ctrllearncanfilter.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x060011D0 RID: 4560 RVA: 0x0006B880 File Offset: 0x00069A80
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
				this.imagebox = (XRSImageBox)target;
			}
		}

		// Token: 0x04000BC7 RID: 3015
		private LearnCanFilterViewModel viewModel;

		// Token: 0x04000BC8 RID: 3016
		private Image moveImage;

		// Token: 0x04000BC9 RID: 3017
		private Image crossSelectImage;

		// Token: 0x04000BCA RID: 3018
		private Button btnMoveOrDrawCross;

		// Token: 0x04000BCB RID: 3019
		private Button btnBack;

		// Token: 0x04000BCC RID: 3020
		public List<Button> Buttons;

		// Token: 0x04000BCD RID: 3021
		internal XRSImageBox imagebox;

		// Token: 0x04000BCE RID: 3022
		private bool _contentLoaded;
	}
}
