using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using DatabaseConnection;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x02000060 RID: 96
	public class CtrlComissioningObjectBasedFlaps : UserControl, IComponentConnector
	{
		// Token: 0x06000967 RID: 2407 RVA: 0x0003A0F4 File Offset: 0x000382F4
		public CtrlComissioningObjectBasedFlaps(DBConnection dbConnection, ParameterHandling parHandling)
		{
			this.InitializeComponent();
			this.viewModel = new ComissioningObjectBasedFlapsViewModel(dbConnection, parHandling);
			base.DataContext = this.viewModel;
			this.InitializeButtons();
		}

		// Token: 0x06000968 RID: 2408 RVA: 0x0003A128 File Offset: 0x00038328
		private void InitializeButtons()
		{
			this.PanelSubButtons = new DockPanel();
			this.PanelSubButtons.HorizontalAlignment = HorizontalAlignment.Stretch;
			this.PanelSubButtons.VerticalAlignment = VerticalAlignment.Stretch;
			this.viewModel.btnStartWritingFlapParams.Content = ImageConverter.ToImage("/img/play.png");
			this.viewModel.btnStartWritingFlapParams.Style = (base.FindResource("defaultButtonStyle") as Style);
			this.viewModel.btnStartWritingFlapParams.Command = this.viewModel.StartWritingFlapParamCommand;
			this.PanelSubButtons.Children.Add(this.viewModel.btnStartWritingFlapParams);
			DockPanel.SetDock(this.viewModel.btnStartWritingFlapParams, Dock.Left);
		}

		// Token: 0x06000969 RID: 2409 RVA: 0x0003A1E4 File Offset: 0x000383E4
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/comissioning/ctrlcomissioningobjectbasedflaps.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x0600096A RID: 2410 RVA: 0x0003A21A File Offset: 0x0003841A
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			this._contentLoaded = true;
		}

		// Token: 0x0400066A RID: 1642
		private ComissioningObjectBasedFlapsViewModel viewModel;

		// Token: 0x0400066B RID: 1643
		public DockPanel PanelSubButtons;

		// Token: 0x0400066C RID: 1644
		private bool _contentLoaded;
	}
}
