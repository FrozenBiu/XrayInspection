using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x02000016 RID: 22
	public class CtrlAIObjectStatus : UserControl, IComponentConnector
	{
		// Token: 0x1400000E RID: 14
		// (add) Token: 0x060001BE RID: 446 RVA: 0x0000AFC8 File Offset: 0x000091C8
		// (remove) Token: 0x060001BF RID: 447 RVA: 0x0000B000 File Offset: 0x00009200
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event EventHandler UpdateColourBtnSelectionMenu;

		// Token: 0x060001C0 RID: 448 RVA: 0x0000B035 File Offset: 0x00009235
		public CtrlAIObjectStatus(ParameterHandling parHandling, AccessRightsManager accessManager, ProductManager productManager)
		{
			this.InitializeComponent();
			this.parHandling = parHandling;
			this.viewModel = new AIObjectStatusViewModel(parHandling, accessManager, productManager);
			base.DataContext = this.viewModel;
			this.InitializeButtons();
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x0000B070 File Offset: 0x00009270
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
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x0000B110 File Offset: 0x00009310
		private void CtrlAIObjectStatus_Unloaded(object sender, RoutedEventArgs e)
		{
			bool flag = this.parHandling.ReadPar(Definition.anomalyDetectionEnabled) == "1";
			if (flag)
			{
				EventHandler updateColourBtnSelectionMenu = this.UpdateColourBtnSelectionMenu;
				if (updateColourBtnSelectionMenu != null)
				{
					updateColourBtnSelectionMenu(this, EventArgs.Empty);
				}
			}
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x0000B154 File Offset: 0x00009354
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/ai/ctrlaiobjectstatus.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x0000B18C File Offset: 0x0000938C
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
				((CtrlAIObjectStatus)target).Unloaded += this.CtrlAIObjectStatus_Unloaded;
			}
		}

		// Token: 0x04000199 RID: 409
		private AIObjectStatusViewModel viewModel;

		// Token: 0x0400019A RID: 410
		private ParameterHandling parHandling;

		// Token: 0x0400019B RID: 411
		public UIElement UiElementRightAligned;

		// Token: 0x0400019C RID: 412
		public DockPanel PanelButtons;

		// Token: 0x0400019E RID: 414
		private bool _contentLoaded;
	}
}
