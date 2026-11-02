using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using DatabaseConnection;
using Definitions;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x0200001A RID: 26
	public class CtrlAIStatus : UserControl, IComponentConnector
	{
		// Token: 0x14000010 RID: 16
		// (add) Token: 0x060001DA RID: 474 RVA: 0x0000B5E4 File Offset: 0x000097E4
		// (remove) Token: 0x060001DB RID: 475 RVA: 0x0000B61C File Offset: 0x0000981C
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event EventHandler UpdateColourBtnSelectionMenu;

		// Token: 0x060001DC RID: 476 RVA: 0x0000B654 File Offset: 0x00009854
		public CtrlAIStatus(ParameterHandling parHandling, AccessRightsManager accessManager, DBConnection dbConnection, ProductManager productManager, AIService aiService, TestcardSimService testcardSimService, ImageDataTransfer imageTransfer, StoreImageData imageArchiver)
		{
			this.InitializeComponent();
			this.testcardSimService = testcardSimService;
			this.parHandling = parHandling;
			this.viewModel = new AIStatusViewModel(parHandling, accessManager, dbConnection, productManager, aiService, testcardSimService, imageTransfer, imageArchiver);
			this.viewModel.SendTestcardSimImages += this.afterAIAcitvation_SendTestcardSimImg;
			base.DataContext = this.viewModel;
			this.InitializeButtons();
		}

		// Token: 0x060001DD RID: 477 RVA: 0x0000B6C4 File Offset: 0x000098C4
		private void afterAIAcitvation_SendTestcardSimImg()
		{
			Thread thread = new Thread(delegate()
			{
				this.viewModel.initprogBarSendingTestcardSim();
				this.testcardSimService.TestcardSimAfterTrainingSendImg(false, null, true);
			});
			thread.Start();
		}

		// Token: 0x060001DE RID: 478 RVA: 0x0000B6EC File Offset: 0x000098EC
		private void InitializeButtons()
		{
			this.UiElementRightAligned = new Label
			{
				Content = "THiNK XA",
				FontSize = 30.0,
				FontWeight = FontWeights.Bold,
				FontFamily = new FontFamily("Verdana"),
				Foreground = Brushes.DarkGray,
				VerticalAlignment = VerticalAlignment.Center,
				HorizontalAlignment = HorizontalAlignment.Right,
				Margin = new Thickness(0.0, 2.0, 20.0, 2.0)
			};
		}

		// Token: 0x060001DF RID: 479 RVA: 0x0000B78C File Offset: 0x0000998C
		private void CtrlAIStatus_Unloaded(object sender, RoutedEventArgs e)
		{
			bool flag = this.parHandling.ReadPar(Definition.objectDetectionEnabled) == "1";
			if (flag)
			{
				EventHandler updateColourBtnSelectionMenu = this.UpdateColourBtnSelectionMenu;
				if (updateColourBtnSelectionMenu != null)
				{
					updateColourBtnSelectionMenu(this, EventArgs.Empty);
				}
			}
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x0000B7D0 File Offset: 0x000099D0
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/ai/ctrlaistatus.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x0000B808 File Offset: 0x00009A08
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
				((CtrlAIStatus)target).Unloaded += this.CtrlAIStatus_Unloaded;
			}
		}

		// Token: 0x040001AB RID: 427
		private AIStatusViewModel viewModel;

		// Token: 0x040001AC RID: 428
		private TestcardSimService testcardSimService;

		// Token: 0x040001AD RID: 429
		public UIElement UiElementRightAligned;

		// Token: 0x040001AE RID: 430
		public DockPanel PanelButtons;

		// Token: 0x040001B0 RID: 432
		private ParameterHandling parHandling;

		// Token: 0x040001B1 RID: 433
		private bool _contentLoaded;
	}
}
