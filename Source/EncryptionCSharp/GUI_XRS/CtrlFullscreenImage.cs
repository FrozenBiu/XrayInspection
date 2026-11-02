using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using Definitions;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x0200011A RID: 282
	public class CtrlFullscreenImage : UserControl, IComponentConnector
	{
		// Token: 0x06001B37 RID: 6967 RVA: 0x000A3F94 File Offset: 0x000A2194
		public CtrlFullscreenImage(HistoryViewModel historyViewModel, bool retrainAvailable, bool retrainRegionAvailable, string usbPath, AccessRightsManager accessManager, TestcardSimViewModel testcardSimViewModel, AIViewModel aiViewModel, AIAutotrainViewModel aIAutotrainViewModel)
		{
			this.accessManager = accessManager;
			this.InitializeComponent();
			this.isHistorymenu = false;
			bool flag = historyViewModel != null;
			if (flag)
			{
				this.isHistorymenu = true;
			}
			this.InitializeButtons(retrainAvailable, retrainRegionAvailable, usbPath);
			this.viewModel = new FullscreenViewModel(historyViewModel, testcardSimViewModel, aiViewModel, aIAutotrainViewModel);
			base.DataContext = this.viewModel;
		}

		// Token: 0x06001B38 RID: 6968 RVA: 0x000A4004 File Offset: 0x000A2204
		private void InitializeButtons(bool retrainAvailable, bool retrainRegionAvailable, string usbPath)
		{
			bool flag = this.isHistorymenu;
			if (flag)
			{
				this.dockpanel = new DockPanel();
				this.dockpanel.HorizontalAlignment = HorizontalAlignment.Stretch;
				this.dockpanel.VerticalAlignment = VerticalAlignment.Stretch;
				Grid.SetColumn(this.dockpanel, 4);
				if (retrainAvailable)
				{
					this.btnRetrain = new Button();
					this.btnRetrain.Content = ImageConverter.ToImage("/img/retrain.png");
					this.btnRetrain.Click += this.Retrain;
					this.btnRetrain.Style = (base.FindResource("defaultButtonStyle") as Style);
					this.btnRetrain.Margin = new Thickness(0.0, 0.0, 10.0, 0.0);
					this.dockpanel.Children.Add(this.btnRetrain);
					DockPanel.SetDock(this.btnRetrain, Dock.Left);
					this.btnRetrainRegion = new Button();
					Image image = ImageConverter.ToImage("/img/retrainregion.png");
					image.Height = 40.0;
					image.Width = 40.0;
					image.Stretch = Stretch.Uniform;
					this.btnRetrainRegion.Content = image;
					this.btnRetrainRegion.Click += this.ShowRetrainRegion;
					this.btnRetrainRegion.Style = (base.FindResource("defaultButtonStyle") as Style);
					this.btnRetrainRegion.Margin = new Thickness(0.0, 0.0, 10.0, 0.0);
					this.dockpanel.Children.Add(this.btnRetrainRegion);
					DockPanel.SetDock(this.btnRetrainRegion, Dock.Left);
				}
				if (retrainRegionAvailable)
				{
					this.btnRetrainGroup = new Button();
					Image image2 = ImageConverter.ToImage("/img/retrainGroup.png");
					image2.Height = 40.0;
					image2.Width = 40.0;
					image2.Stretch = Stretch.Uniform;
					this.btnRetrainGroup.Content = image2;
					this.btnRetrainGroup.Click += this.RetrainGroup;
					this.btnRetrainGroup.Style = (base.FindResource("defaultButtonStyle") as Style);
					this.btnRetrainGroup.Margin = new Thickness(0.0, 0.0, 10.0, 0.0);
					this.dockpanel.Children.Add(this.btnRetrainGroup);
					DockPanel.SetDock(this.btnRetrainGroup, Dock.Left);
				}
				this.btnSaveToUsb = new ButtonUSB(usbPath, "/img/saveonusb_small.png", "/img/saveonusbsmall_nousb.png", new RoutedEventHandler(this.SaveToUSB));
				this.dockpanel.Children.Add(this.btnSaveToUsb);
				DockPanel.SetDock(this.btnSaveToUsb, Dock.Left);
				this.btnCommentXri = new Button();
				Image image3 = ImageConverter.ToImage("/img/commentXri_small.png");
				image3.Height = 40.0;
				image3.Width = 40.0;
				image3.Stretch = Stretch.Uniform;
				this.btnCommentXri.Content = image3;
				this.btnCommentXri.Click += this.CommentXri;
				this.btnCommentXri.Style = (base.FindResource("defaultButtonStyle") as Style);
				this.btnCommentXri.Margin = new Thickness(0.0, 0.0, 10.0, 0.0);
				this.dockpanel.Children.Add(this.btnCommentXri);
				DockPanel.SetDock(this.btnCommentXri, Dock.Left);
				TextBlock textBlock = new TextBlock();
				textBlock.Style = (base.FindResource("txtBlockStyle") as Style);
				DockPanel.SetDock(textBlock, Dock.Right);
				this.dockpanel.Children.Add(textBlock);
			}
		}

		// Token: 0x06001B39 RID: 6969 RVA: 0x000A4428 File Offset: 0x000A2628
		private void SetControlRights()
		{
			this.accessManager.RegisterControl(this.btnSaveToUsb, HistoryControl.rightSaveToUsb, HistoryControl.rightSaveToUsb);
			this.accessManager.RegisterControl(this.btnCommentXri, this.rightCommentXri, this.rightCommentXri);
			this.accessManager.SetProperties();
		}

		// Token: 0x06001B3A RID: 6970 RVA: 0x000A447C File Offset: 0x000A267C
		private void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
			HwndSource hwndSource = PresentationSource.FromVisual(this) as HwndSource;
			HwndTarget compositionTarget = hwndSource.CompositionTarget;
			compositionTarget.RenderMode = RenderMode.SoftwareOnly;
			Subwindow subwindow = Window.GetWindow(this) as Subwindow;
			subwindow.buttonGrid.Children.Remove(subwindow.buttonPanel);
			bool flag = this.isHistorymenu;
			if (flag)
			{
				subwindow.buttonGrid.Children.Add(this.dockpanel);
			}
		}

		// Token: 0x06001B3B RID: 6971 RVA: 0x000A44E9 File Offset: 0x000A26E9
		public void Retrain(object sender, RoutedEventArgs e)
		{
			this.viewModel.Retrain();
		}

		// Token: 0x06001B3C RID: 6972 RVA: 0x000A44F8 File Offset: 0x000A26F8
		public void ShowRetrainRegion(object sender, RoutedEventArgs e)
		{
			this.viewModel.RetrainRegion();
		}

		// Token: 0x06001B3D RID: 6973 RVA: 0x000A4507 File Offset: 0x000A2707
		public void RetrainGroup(object sender, RoutedEventArgs e)
		{
			this.viewModel.RetrainGroup();
		}

		// Token: 0x06001B3E RID: 6974 RVA: 0x000A4518 File Offset: 0x000A2718
		public void CommentXri(object sender, RoutedEventArgs e)
		{
			this.viewModel.CommentXri();
			TabControl tabControl = (TabControl)base.FindName("tabControl");
			((RegionImage)tabControl.SelectedItem).XrsImage = this.viewModel.Image;
		}

		// Token: 0x06001B3F RID: 6975 RVA: 0x000A455F File Offset: 0x000A275F
		public void SaveToUSB(object sender, RoutedEventArgs e)
		{
			this.viewModel.SaveToUSB();
		}

		// Token: 0x06001B40 RID: 6976 RVA: 0x000A4570 File Offset: 0x000A2770
		private void UserControl_Unloaded(object sender, RoutedEventArgs e)
		{
			this.accessManager.DeregisterControl(this.btnSaveToUsb);
			this.accessManager.DeregisterControl(this.btnCommentXri);
			bool flag = this.viewModel.Image != null;
			if (flag)
			{
				this.viewModel.Image = null;
			}
			bool flag2 = this.viewModel.RegionImages != null;
			if (flag2)
			{
				this.viewModel.RegionImages.Clear();
				this.viewModel.RegionImages = null;
			}
			BindingOperations.ClearAllBindings(this);
			base.DataContext = null;
		}

		// Token: 0x06001B41 RID: 6977 RVA: 0x000A4604 File Offset: 0x000A2804
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/history/ctrlfullscreenimage.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x06001B42 RID: 6978 RVA: 0x000A463C File Offset: 0x000A283C
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				((CtrlFullscreenImage)target).Loaded += this.UserControl_Loaded;
				((CtrlFullscreenImage)target).Unloaded += this.UserControl_Unloaded;
				break;
			case 2:
				this.fGrid = (Grid)target;
				break;
			case 3:
				this.tabControl = (TabControl)target;
				break;
			default:
				this._contentLoaded = true;
				break;
			}
		}

		// Token: 0x0400121A RID: 4634
		private DockPanel dockpanel;

		// Token: 0x0400121B RID: 4635
		private FullscreenViewModel viewModel;

		// Token: 0x0400121C RID: 4636
		private AccessRightsManager accessManager;

		// Token: 0x0400121D RID: 4637
		private Button btnRetrain;

		// Token: 0x0400121E RID: 4638
		private Button btnRetrainRegion;

		// Token: 0x0400121F RID: 4639
		private Button btnRetrainGroup;

		// Token: 0x04001220 RID: 4640
		private ButtonUSB btnSaveToUsb;

		// Token: 0x04001221 RID: 4641
		private Button btnCommentXri;

		// Token: 0x04001222 RID: 4642
		private BuiltInPriviledges rightCommentXri = BuiltInPriviledges.Operator1;

		// Token: 0x04001223 RID: 4643
		private bool isHistorymenu;

		// Token: 0x04001224 RID: 4644
		internal Grid fGrid;

		// Token: 0x04001225 RID: 4645
		internal TabControl tabControl;

		// Token: 0x04001226 RID: 4646
		private bool _contentLoaded;
	}
}
