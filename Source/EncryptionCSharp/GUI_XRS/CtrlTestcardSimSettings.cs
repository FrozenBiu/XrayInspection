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
	// Token: 0x020000DB RID: 219
	public class CtrlTestcardSimSettings : UserControl, IComponentConnector
	{
		// Token: 0x060015D4 RID: 5588 RVA: 0x000836B0 File Offset: 0x000818B0
		public CtrlTestcardSimSettings(ParameterHandling parHandling, TestcardSimService testcardSimService, DBConnection dBConnection, ProductManager productManager, AccessRightsManager accessManager, ErrorHandling errorHandling, XRSImage imageForComplianceMode, ImageDataTransfer imageTransfer)
		{
			this.InitializeComponent();
			this.viewModel = new TestcardSimSettingsViewModel(parHandling, testcardSimService, dBConnection, productManager, accessManager, errorHandling, imageTransfer);
			base.DataContext = this.viewModel;
			this.InitializeButtons();
		}

		// Token: 0x060015D5 RID: 5589 RVA: 0x000836EC File Offset: 0x000818EC
		private void InitializeButtons()
		{
			this.UiElementRightAligned = new Label
			{
				Content = this.viewModel.StrSettings.LocalizedText,
				FontSize = 30.0,
				FontWeight = FontWeights.Bold,
				FontFamily = new FontFamily("Verdana"),
				Foreground = Brushes.DarkGray,
				VerticalAlignment = VerticalAlignment.Center,
				HorizontalAlignment = HorizontalAlignment.Right,
				Margin = new Thickness(0.0, 0.0, 20.0, 0.0)
			};
		}

		// Token: 0x060015D6 RID: 5590 RVA: 0x00083798 File Offset: 0x00081998
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/testcardsimulation/ctrltestcardsimsettings.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x060015D7 RID: 5591 RVA: 0x000837CE File Offset: 0x000819CE
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			this._contentLoaded = true;
		}

		// Token: 0x04000E64 RID: 3684
		private TestcardSimSettingsViewModel viewModel;

		// Token: 0x04000E65 RID: 3685
		public DockPanel PanelButtons;

		// Token: 0x04000E66 RID: 3686
		public UIElement UiElementRightAligned;

		// Token: 0x04000E67 RID: 3687
		private bool _contentLoaded;
	}
}
