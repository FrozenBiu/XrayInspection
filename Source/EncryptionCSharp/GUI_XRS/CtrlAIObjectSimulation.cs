using System;
using System.CodeDom.Compiler;
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
	// Token: 0x02000015 RID: 21
	public class CtrlAIObjectSimulation : UserControl, IComponentConnector
	{
		// Token: 0x1400000D RID: 13
		// (add) Token: 0x060001B6 RID: 438 RVA: 0x0000A924 File Offset: 0x00008B24
		// (remove) Token: 0x060001B7 RID: 439 RVA: 0x0000A95C File Offset: 0x00008B5C
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event EventHandler UpdateConfThreshold;

		// Token: 0x060001B8 RID: 440 RVA: 0x0000A994 File Offset: 0x00008B94
		public CtrlAIObjectSimulation(StoreImageData imageArchiver, ImageDataTransfer imageTransfer, ParameterHandling parHandling, DBConnection dbConnection, ProductManager productManager)
		{
			this.InitializeComponent();
			this.viewModel = new AIObjectSimulationViewModel(imageArchiver, imageTransfer, parHandling, this.imagebox, dbConnection, productManager);
			base.DataContext = this.viewModel;
			this.viewModel.ShowImageAction = delegate(XRSImage image)
			{
				Application.Current.Dispatcher.Invoke(delegate()
				{
					this.imagebox.ViewXRSImage(image);
				});
			};
			this.InitializeButtons();
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x0000A9F3 File Offset: 0x00008BF3
		private void UserControl_Unloaded(object sender, RoutedEventArgs e)
		{
			this.viewModel.CleanUp();
			base.DataContext = null;
			EventHandler updateConfThreshold = this.UpdateConfThreshold;
			if (updateConfThreshold != null)
			{
				updateConfThreshold(this, EventArgs.Empty);
			}
		}

		// Token: 0x060001BA RID: 442 RVA: 0x0000AA24 File Offset: 0x00008C24
		private void InitializeButtons()
		{
			this.UiElementRightAligned = new Label
			{
				Content = "THiNK XO",
				FontSize = 30.0,
				FontWeight = FontWeights.Bold,
				FontFamily = new FontFamily("Verdana"),
				Foreground = Brushes.DarkGray,
				VerticalAlignment = VerticalAlignment.Center,
				HorizontalAlignment = HorizontalAlignment.Right,
				Margin = new Thickness(0.0, 2.0, 20.0, 2.0)
			};
			this.PanelButtons = new DockPanel();
			this.PanelButtons.HorizontalAlignment = HorizontalAlignment.Stretch;
			this.PanelButtons.VerticalAlignment = VerticalAlignment.Stretch;
			Button button = new Button();
			button.Content = "";
			button.Style = (base.FindResource("defaultButtonStyle") as Style);
			button.Visibility = Visibility.Hidden;
			button.Width = 40.0;
			this.PanelButtons.Children.Add(button);
			Image image = ImageConverter.ToImage("/img/retrainregionMinus.png");
			image.Height = 40.0;
			image.Width = 40.0;
			image.Stretch = Stretch.Uniform;
			this.viewModel.btnRetrainRegion.Content = image;
			this.viewModel.btnRetrainRegion.Style = (base.FindResource("defaultButtonStyle") as Style);
			this.viewModel.btnRetrainRegion.Command = this.viewModel.RetrainRegionCommand;
			this.viewModel.btnRetrainRegion.Margin = new Thickness(0.0, 2.0, 10.0, 2.0);
			this.viewModel.btnRetrainRegion.IsEnabled = true;
			this.PanelButtons.Children.Add(this.viewModel.btnRetrainRegion);
			DockPanel.SetDock(this.viewModel.btnRetrainRegion, Dock.Left);
			Image image2 = ImageConverter.ToImage("/img/retrainregionPlus.png");
			image2.Height = 40.0;
			image2.Width = 40.0;
			image2.Stretch = Stretch.Uniform;
			this.viewModel.btnTrainToTrigger.Content = image2;
			this.viewModel.btnTrainToTrigger.Style = (base.FindResource("defaultButtonStyle") as Style);
			this.viewModel.btnTrainToTrigger.Command = this.viewModel.TrainToTriggerCommand;
			this.viewModel.btnTrainToTrigger.Margin = new Thickness(0.0, 2.0, 10.0, 2.0);
			this.viewModel.btnTrainToTrigger.IsEnabled = true;
			this.PanelButtons.Children.Add(this.viewModel.btnTrainToTrigger);
			DockPanel.SetDock(this.viewModel.btnTrainToTrigger, Dock.Left);
			this.viewModel.SetImageUndoButton(false);
			this.viewModel.btnUndo.Style = (base.FindResource("defaultButtonStyle") as Style);
			this.viewModel.btnUndo.Command = this.viewModel.UndoCommand;
			this.viewModel.btnUndo.Margin = new Thickness(0.0, 2.0, 25.0, 2.0);
			this.PanelButtons.Children.Add(this.viewModel.btnUndo);
			DockPanel.SetDock(this.viewModel.btnUndo, Dock.Left);
			Image image3 = ImageConverter.ToImage("/img/moveImage.png");
			image3.Height = 40.0;
			image3.Width = 40.0;
			image3.Stretch = Stretch.Uniform;
			this.viewModel.btnMoveOrSelect.Content = image3;
			this.viewModel.btnMoveOrSelect.Style = (base.FindResource("defaultButtonStyle") as Style);
			this.viewModel.btnMoveOrSelect.Command = this.viewModel.MoveOrSelectCommand;
			this.viewModel.btnMoveOrSelect.Margin = new Thickness(0.0, 2.0, 10.0, 2.0);
			this.viewModel.btnMoveOrSelect.IsEnabled = true;
			this.PanelButtons.Children.Add(this.viewModel.btnMoveOrSelect);
			DockPanel.SetDock(this.viewModel.btnMoveOrSelect, Dock.Left);
			this.imagebox.ToggleBoxSelectionModeAndButtonImage(this.viewModel.btnMoveOrSelect);
		}

		// Token: 0x060001BB RID: 443 RVA: 0x0000AF00 File Offset: 0x00009100
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/ai/ctrlaiobjectsimulation.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x060001BC RID: 444 RVA: 0x0000AF38 File Offset: 0x00009138
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
				((CtrlAIObjectSimulation)target).Unloaded += this.UserControl_Unloaded;
			}
		}

		// Token: 0x04000193 RID: 403
		private AIObjectSimulationViewModel viewModel;

		// Token: 0x04000194 RID: 404
		public DockPanel PanelButtons;

		// Token: 0x04000195 RID: 405
		public UIElement UiElementRightAligned;

		// Token: 0x04000197 RID: 407
		internal XRSImageBox imagebox;

		// Token: 0x04000198 RID: 408
		private bool _contentLoaded;
	}
}
