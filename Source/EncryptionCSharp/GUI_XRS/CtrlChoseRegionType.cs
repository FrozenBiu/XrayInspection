using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x0200012D RID: 301
	public class CtrlChoseRegionType : UserControl, IComponentConnector
	{
		// Token: 0x1700087F RID: 2175
		// (get) Token: 0x06001D09 RID: 7433 RVA: 0x000BBB0F File Offset: 0x000B9D0F
		// (set) Token: 0x06001D0A RID: 7434 RVA: 0x000BBB17 File Offset: 0x000B9D17
		public List<Button> ButtonsAddRegion { get; set; }

		// Token: 0x17000880 RID: 2176
		// (get) Token: 0x06001D0B RID: 7435 RVA: 0x000BBB20 File Offset: 0x000B9D20
		// (set) Token: 0x06001D0C RID: 7436 RVA: 0x000BBB28 File Offset: 0x000B9D28
		public DockPanel SubPanelEdit { get; set; }

		// Token: 0x17000881 RID: 2177
		// (get) Token: 0x06001D0D RID: 7437 RVA: 0x000BBB34 File Offset: 0x000B9D34
		public bool RegionAdded
		{
			get
			{
				return this.viewModel.RegionAdded;
			}
		}

		// Token: 0x17000882 RID: 2178
		// (get) Token: 0x06001D0E RID: 7438 RVA: 0x000BBB54 File Offset: 0x000B9D54
		public bool RegionEdited
		{
			get
			{
				return this.viewModel.RegionEdited;
			}
		}

		// Token: 0x06001D0F RID: 7439 RVA: 0x000BBB74 File Offset: 0x000B9D74
		public CtrlChoseRegionType(ProductManager productManager, ObservableCollection<ProductData> regionProduct, ProductData dummyProduct, bool addRegion)
		{
			this.InitializeComponent();
			this.viewModel = new ChoseRegionTypeViewModel(productManager, regionProduct, dummyProduct, addRegion);
			this.viewModel.RegionAddedOrEdited += this.viewModel_RegionAddedOrEdited;
			this.InitializeButtons(addRegion);
			base.DataContext = this.viewModel;
		}

		// Token: 0x06001D10 RID: 7440 RVA: 0x000BBBD0 File Offset: 0x000B9DD0
		private void InitializeButtons(bool addRegion)
		{
			if (addRegion)
			{
				this.ButtonsAddRegion = new List<Button>();
				Button button = new Button();
				button.Content = ImageConverter.ToImage("/img/addproduct.png");
				button.Command = this.viewModel.AddOrEditRegionCommand;
				button.Style = (base.FindResource("defaultButtonStyle") as Style);
				this.ButtonsAddRegion.Add(button);
			}
			else
			{
				this.SubPanelEdit = new DockPanel();
				this.SubPanelEdit.HorizontalAlignment = HorizontalAlignment.Stretch;
				this.SubPanelEdit.VerticalAlignment = VerticalAlignment.Stretch;
				Grid.SetColumn(this.SubPanelEdit, 4);
				Button button2 = new Button();
				button2.Content = ImageConverter.ToImage("/img/ok.png");
				button2.Command = this.viewModel.AddOrEditRegionCommand;
				button2.Style = (base.FindResource("okButtonStyle") as Style);
				button2.Margin = new Thickness(-60.0, 0.0, 0.0, 0.0);
				this.SubPanelEdit.Children.Add(button2);
				DockPanel.SetDock(button2, Dock.Left);
				Button button3 = new Button();
				button3.Content = ImageConverter.ToImage("/img/cancel.png");
				button3.Click += delegate(object sender, RoutedEventArgs args)
				{
					(Window.GetWindow(this) as Subwindow).Close();
				};
				button3.Style = (base.FindResource("cancelButtonStyle") as Style);
				button3.HorizontalAlignment = HorizontalAlignment.Right;
				button3.Margin = new Thickness(0.0, 0.0, 10.0, 0.0);
				this.SubPanelEdit.Children.Add(button3);
				DockPanel.SetDock(button3, Dock.Right);
			}
		}

		// Token: 0x06001D11 RID: 7441 RVA: 0x000BBD95 File Offset: 0x000B9F95
		private void viewModel_RegionAddedOrEdited()
		{
			this.viewModel.RegionAddedOrEdited -= this.viewModel_RegionAddedOrEdited;
			(Window.GetWindow(this) as Subwindow).Close();
		}

		// Token: 0x06001D12 RID: 7442 RVA: 0x000BBDC4 File Offset: 0x000B9FC4
		private void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
			bool flag = this.SubPanelEdit != null;
			if (flag)
			{
				Subwindow subwindow = Window.GetWindow(this) as Subwindow;
				subwindow.buttonGrid.Children.Remove(subwindow.buttonPanel);
				subwindow.buttonGrid.Children.Add(this.SubPanelEdit);
			}
		}

		// Token: 0x06001D13 RID: 7443 RVA: 0x000BBE1C File Offset: 0x000BA01C
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/productchange/ctrlchoseregiontype.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x06001D14 RID: 7444 RVA: 0x000BBE54 File Offset: 0x000BA054
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
				((CtrlChoseRegionType)target).Loaded += this.UserControl_Loaded;
			}
		}

		// Token: 0x04001330 RID: 4912
		private ChoseRegionTypeViewModel viewModel;

		// Token: 0x04001333 RID: 4915
		private bool _contentLoaded;
	}
}
