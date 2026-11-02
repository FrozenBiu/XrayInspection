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
	// Token: 0x02000013 RID: 19
	public class CtrlAIMenuSelection : UserControl, IComponentConnector
	{
		// Token: 0x0600019B RID: 411 RVA: 0x00009C18 File Offset: 0x00007E18
		public CtrlAIMenuSelection(ParameterHandling parHandling, ProductManager productManager, DBConnection dbConnection, AccessRightsManager accessManager, ImageDataTransfer imageTransfer, StoreImageData imageArchiver, ErrorHandling errorHandling, TestcardSimService testcardSimService, AIService aiService, CtrlAIMenuSelection.SelectionMenuContext context)
		{
			this.InitializeComponent();
			this.viewModel = new AIMenuSelectionViewModel(parHandling, productManager, dbConnection, accessManager, imageTransfer, imageArchiver, errorHandling, testcardSimService, aiService, context);
			base.DataContext = this.viewModel;
			this.InitializeButtons(context);
		}

		// Token: 0x0600019C RID: 412 RVA: 0x00009C68 File Offset: 0x00007E68
		private void InitializeButtons(CtrlAIMenuSelection.SelectionMenuContext context)
		{
			this.PanelButtons = new DockPanel();
			this.PanelButtons.HorizontalAlignment = HorizontalAlignment.Stretch;
			this.PanelButtons.VerticalAlignment = VerticalAlignment.Stretch;
			Button button = new Button();
			button.Content = "";
			button.Style = (base.FindResource("defaultButtonStyle") as Style);
			button.Visibility = Visibility.Hidden;
			button.Width = 40.0;
			this.PanelButtons.Children.Add(button);
			bool flag = context == CtrlAIMenuSelection.SelectionMenuContext.MainMenu;
			if (flag)
			{
				Image image = ImageConverter.ToImage("/img/restore.png");
				image.Height = 40.0;
				image.Width = 40.0;
				image.Stretch = Stretch.Uniform;
				this.viewModel.btnRestoreModel.Content = image;
				this.viewModel.btnRestoreModel.Style = (base.FindResource("defaultButtonStyle") as Style);
				this.viewModel.btnRestoreModel.Command = this.viewModel.RestoreModelCommand;
				this.viewModel.btnRestoreModel.Margin = new Thickness(0.0, 2.0, 10.0, 2.0);
				this.viewModel.btnRestoreModel.IsEnabled = true;
				this.PanelButtons.Children.Add(this.viewModel.btnRestoreModel);
				DockPanel.SetDock(this.viewModel.btnRestoreModel, Dock.Left);
			}
		}

		// Token: 0x0600019D RID: 413 RVA: 0x00009DF7 File Offset: 0x00007FF7
		private void UserControl_Unloaded(object sender, RoutedEventArgs e)
		{
			this.viewModel.CleanUp();
			base.DataContext = null;
		}

		// Token: 0x0600019E RID: 414 RVA: 0x00009E10 File Offset: 0x00008010
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/ai/ctrlaimenuselection.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x0600019F RID: 415 RVA: 0x00009E48 File Offset: 0x00008048
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
				((CtrlAIMenuSelection)target).Unloaded += this.UserControl_Unloaded;
			}
		}

		// Token: 0x04000187 RID: 391
		private AIMenuSelectionViewModel viewModel;

		// Token: 0x04000188 RID: 392
		public DockPanel PanelButtons;

		// Token: 0x04000189 RID: 393
		public UIElement UiElementRightAligned;

		// Token: 0x0400018A RID: 394
		private bool _contentLoaded;

		// Token: 0x0200028D RID: 653
		public enum SelectionMenuContext
		{
			// Token: 0x04001F22 RID: 7970
			MainMenu,
			// Token: 0x04001F23 RID: 7971
			StatusMenu
		}
	}
}
