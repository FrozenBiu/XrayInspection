using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using DatabaseConnection;
using Definitions;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x0200002F RID: 47
	public class CtrlRetrainRegion : UserControl, IComponentConnector
	{
		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x060005E6 RID: 1510 RVA: 0x00027F63 File Offset: 0x00026163
		// (set) Token: 0x060005E7 RID: 1511 RVA: 0x00027F6B File Offset: 0x0002616B
		public Grid PanelSubButtons { get; set; }

		// Token: 0x060005E8 RID: 1512 RVA: 0x00027F74 File Offset: 0x00026174
		public CtrlRetrainRegion(DBConnection dbConnection, ParameterHandling parHandling, ImageDataTransfer imageTransfer, XRSImage image, ProductManager productManager, TestcardSimService testcardSimService)
		{
			this.InitializeComponent();
			this.dbConnection = dbConnection;
			this.productManager = productManager;
			this.viewModel = new RetrainRegionViewModel(dbConnection, parHandling, imageTransfer, this.imagebox, image, productManager, testcardSimService);
			base.DataContext = this.viewModel;
			this.InitializeButtonsPanel();
		}

		// Token: 0x060005E9 RID: 1513 RVA: 0x00027FD0 File Offset: 0x000261D0
		private void InitializeButtonsPanel()
		{
			this.PanelSubButtons = new Grid();
			this.PanelSubButtons.ColumnDefinitions.Add(new ColumnDefinition());
			this.PanelSubButtons.ColumnDefinitions.Add(new ColumnDefinition());
			this.PanelSubButtons.ColumnDefinitions.Add(new ColumnDefinition());
			this.PanelSubButtons.ColumnDefinitions.Add(new ColumnDefinition());
			this.btnRetrainRegion = new Button();
			Image image = ImageConverter.ToImage("/img/retrainregionMinus.png");
			image.Height = 40.0;
			image.Width = 40.0;
			image.Stretch = Stretch.Uniform;
			this.btnRetrainRegion.Content = image;
			this.btnRetrainRegion.Click += this.RetrainRegion;
			this.btnRetrainRegion.Style = (base.FindResource("defaultButtonStyle") as Style);
			this.btnRetrainRegion.Margin = new Thickness(0.0, 0.0, 10.0, 0.0);
			Grid.SetColumn(this.btnRetrainRegion, 0);
			this.PanelSubButtons.Children.Add(this.btnRetrainRegion);
			Button button = new Button();
			Image image2 = ImageConverter.ToImage("/img/retrainregionPlus.png");
			image2.Height = 40.0;
			image2.Width = 40.0;
			image2.Stretch = Stretch.Uniform;
			button.Content = image2;
			button.Click += this.TrainToTrigger;
			button.Style = (base.FindResource("defaultButtonStyle") as Style);
			button.Margin = new Thickness(0.0, 0.0, 10.0, 0.0);
			Grid.SetColumn(button, 1);
			this.PanelSubButtons.Children.Add(button);
			this.detectortype = DetectorType.Sesotec;
			this.dbConnection.GetDetectorType(ref this.detectortype);
			bool flag = this.detectortype != DetectorType.DTNetDE && !this.productManager.CurrentLoadedProduct.IsRegionProduct;
			if (flag)
			{
				this.btnUndoRetrainRegion = new Button();
				this.setRetrainRegionUndoButton(false);
				this.btnUndoRetrainRegion.Click += this.UndoRetrainRegion;
				this.btnUndoRetrainRegion.Style = (base.FindResource("defaultButtonStyle") as Style);
				this.btnUndoRetrainRegion.Margin = new Thickness(0.0, 0.0, 25.0, 0.0);
				Grid.SetColumn(this.btnUndoRetrainRegion, 2);
				this.PanelSubButtons.Children.Add(this.btnUndoRetrainRegion);
			}
			this.btnMoveOrSelect = new Button();
			this.btnMoveOrSelect.Click += this.ChangeToMoveOrSelect;
			this.btnMoveOrSelect.Style = (base.FindResource("defaultButtonStyle") as Style);
			Grid.SetColumn(this.btnMoveOrSelect, 3);
			this.PanelSubButtons.Children.Add(this.btnMoveOrSelect);
			this.imagebox.ToggleBoxSelectionModeAndButtonImage(this.btnMoveOrSelect);
		}

		// Token: 0x060005EA RID: 1514 RVA: 0x00028328 File Offset: 0x00026528
		public void RetrainRegion(object sender, RoutedEventArgs e)
		{
			bool flag = this.viewModel.RetrainRegion(false);
			if (flag)
			{
				this.setRetrainRegionUndoButton(true);
			}
		}

		// Token: 0x060005EB RID: 1515 RVA: 0x00028350 File Offset: 0x00026550
		private void TrainToTrigger(object sender, RoutedEventArgs e)
		{
			bool flag = this.viewModel.RetrainRegion(true);
			if (flag)
			{
				this.setRetrainRegionUndoButton(true);
			}
		}

		// Token: 0x060005EC RID: 1516 RVA: 0x00028376 File Offset: 0x00026576
		private void UndoRetrainRegion(object sender, RoutedEventArgs e)
		{
			this.setRetrainRegionUndoButton(false);
			this.viewModel.UndoRetrainRegion();
		}

		// Token: 0x060005ED RID: 1517 RVA: 0x0002838D File Offset: 0x0002658D
		private void ChangeToMoveOrSelect(object sender, RoutedEventArgs e)
		{
			this.imagebox.ToggleBoxSelectionModeAndButtonImage(this.btnMoveOrSelect);
		}

		// Token: 0x060005EE RID: 1518 RVA: 0x000283A2 File Offset: 0x000265A2
		private void UserControl_Unloaded(object sender, RoutedEventArgs e)
		{
			this.viewModel.xrsImage = null;
			this.viewModel.SetContinuousModeBulk(true, true);
			BindingOperations.ClearAllBindings(this);
			base.DataContext = null;
		}

		// Token: 0x060005EF RID: 1519 RVA: 0x000283D0 File Offset: 0x000265D0
		private void setRetrainRegionUndoButton(bool enable)
		{
			Image image = ImageConverter.ToImage("/img/retrainUndo_disabled.png");
			if (enable)
			{
				image = ImageConverter.ToImage("/img/retrainUndo.png");
			}
			image.Height = 40.0;
			image.Width = 40.0;
			image.Stretch = Stretch.Uniform;
			bool flag = this.btnUndoRetrainRegion != null;
			if (flag)
			{
				this.btnUndoRetrainRegion.Content = image;
				this.btnUndoRetrainRegion.IsEnabled = enable;
			}
		}

		// Token: 0x060005F0 RID: 1520 RVA: 0x0002844C File Offset: 0x0002664C
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/autotrain/retrainregion.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x060005F1 RID: 1521 RVA: 0x00028484 File Offset: 0x00026684
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
				((CtrlRetrainRegion)target).Unloaded += this.UserControl_Unloaded;
			}
		}

		// Token: 0x04000444 RID: 1092
		private RetrainRegionViewModel viewModel;

		// Token: 0x04000445 RID: 1093
		private Button btnRetrainRegion;

		// Token: 0x04000446 RID: 1094
		private Button btnMoveOrSelect;

		// Token: 0x04000447 RID: 1095
		private Button btnUndoRetrainRegion;

		// Token: 0x04000449 RID: 1097
		private DBConnection dbConnection;

		// Token: 0x0400044A RID: 1098
		private DetectorType detectortype;

		// Token: 0x0400044B RID: 1099
		private ProductManager productManager;

		// Token: 0x0400044C RID: 1100
		internal XRSImageBox imagebox;

		// Token: 0x0400044D RID: 1101
		private bool _contentLoaded;
	}
}
