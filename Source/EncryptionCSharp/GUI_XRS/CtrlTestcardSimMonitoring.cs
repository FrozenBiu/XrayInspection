using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using DatabaseConnection;
using Definitions;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x020000D6 RID: 214
	public class CtrlTestcardSimMonitoring : UserControl, IComponentConnector
	{
		// Token: 0x060014F0 RID: 5360 RVA: 0x0007D638 File Offset: 0x0007B838
		public CtrlTestcardSimMonitoring(ParameterHandling parHandling, TestcardSimService testcardSimService, DBConnection dbConnection, ProductManager productManager, ErrorHandling errorHandling, AccessRightsManager accessManager, XRSImage imageForComplianceMode, ImageDataTransfer imageTransfer, StoreImageData imageArchiver)
		{
			this.dbConnection = dbConnection;
			this.InitializeComponent();
			this.viewModel = new TestcardSimMonitoringViewModel(parHandling, testcardSimService, dbConnection, productManager, errorHandling, accessManager, imageForComplianceMode, imageTransfer, imageArchiver);
			base.DataContext = this.viewModel;
			this.InitializeButtons();
		}

		// Token: 0x060014F1 RID: 5361 RVA: 0x0007D68C File Offset: 0x0007B88C
		private void InitializeButtons()
		{
			bool flag = false;
			this.dbConnection.GetIntegratedComplianceMonitoring(ref flag);
			this.PanelButtons = new DockPanel();
			this.PanelButtons.HorizontalAlignment = HorizontalAlignment.Stretch;
			this.PanelButtons.VerticalAlignment = VerticalAlignment.Stretch;
			bool flag2 = flag;
			if (flag2)
			{
				this.btnOverview = new Button();
				this.btnOverview.Content = ImageConverter.ToImage("/img/tablesettings.png");
				this.btnOverview.Style = (base.FindResource("defaultButtonStyle") as Style);
				this.btnOverview.Command = this.viewModel.ShowMonitoringOverviewCmd;
				this.PanelButtons.Children.Add(this.btnOverview);
				DockPanel.SetDock(this.btnOverview, Dock.Left);
			}
			Label label = new Label();
			Definition.GUIDesign design = this.dbConnection.GetDesign();
			bool flag3 = design == Definition.GUIDesign.Bizerba;
			if (flag3)
			{
				label.Content = this.viewModel.StrComplianceMode.LocalizedText + " by Sesotec";
			}
			else
			{
				label.Content = this.viewModel.StrComplianceMode.LocalizedText;
			}
			label.FontSize = 30.0;
			label.FontWeight = FontWeights.Bold;
			label.FontFamily = new FontFamily("Verdana");
			label.Foreground = Brushes.DarkGray;
			label.VerticalAlignment = VerticalAlignment.Center;
			label.HorizontalAlignment = HorizontalAlignment.Right;
			label.Margin = new Thickness(0.0, 0.0, 20.0, 0.0);
			this.UiElementRightAligned = label;
		}

		// Token: 0x060014F2 RID: 5362 RVA: 0x0007D828 File Offset: 0x0007BA28
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/testcardsimulation/ctrltestcardsimmonitoring.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x060014F3 RID: 5363 RVA: 0x0007D860 File Offset: 0x0007BA60
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
				this.btnStartCMVA = (Button)target;
			}
		}

		// Token: 0x04000DEC RID: 3564
		private TestcardSimMonitoringViewModel viewModel;

		// Token: 0x04000DED RID: 3565
		public DockPanel PanelButtons;

		// Token: 0x04000DEE RID: 3566
		public UIElement UiElementRightAligned;

		// Token: 0x04000DEF RID: 3567
		public Button btnOverview;

		// Token: 0x04000DF0 RID: 3568
		private DBConnection dbConnection;

		// Token: 0x04000DF1 RID: 3569
		internal Button btnStartCMVA;

		// Token: 0x04000DF2 RID: 3570
		private bool _contentLoaded;
	}
}
