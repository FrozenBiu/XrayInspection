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
	// Token: 0x02000061 RID: 97
	public class CtrlComissioningMultilane : UserControl, IComponentConnector
	{
		// Token: 0x0600096B RID: 2411 RVA: 0x0003A224 File Offset: 0x00038424
		public CtrlComissioningMultilane(DBConnection dbConnection, ParameterHandling parHandling)
		{
			this.InitializeComponent();
			this.viewModel = new ComissioningMultilaneViewModel(dbConnection, parHandling);
			base.DataContext = this.viewModel;
			this.InitializeButtons();
		}

		// Token: 0x0600096C RID: 2412 RVA: 0x0003A258 File Offset: 0x00038458
		private void InitializeButtons()
		{
			this.PanelSubButtons = new DockPanel();
			this.PanelSubButtons.HorizontalAlignment = HorizontalAlignment.Stretch;
			this.PanelSubButtons.VerticalAlignment = VerticalAlignment.Stretch;
			this.viewModel.btnStartWritingLaneParams.Content = ImageConverter.ToImage("/img/play.png");
			this.viewModel.btnStartWritingLaneParams.Style = (base.FindResource("defaultButtonStyle") as Style);
			this.viewModel.btnStartWritingLaneParams.Command = this.viewModel.StartWritingLaneParamCommand;
			this.PanelSubButtons.Children.Add(this.viewModel.btnStartWritingLaneParams);
			DockPanel.SetDock(this.viewModel.btnStartWritingLaneParams, Dock.Left);
		}

		// Token: 0x0600096D RID: 2413 RVA: 0x0003A314 File Offset: 0x00038514
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/comissioning/ctrlcomissioningmultilane.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x0600096E RID: 2414 RVA: 0x0003A34A File Offset: 0x0003854A
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			this._contentLoaded = true;
		}

		// Token: 0x0400066D RID: 1645
		private ComissioningMultilaneViewModel viewModel;

		// Token: 0x0400066E RID: 1646
		public DockPanel PanelSubButtons;

		// Token: 0x0400066F RID: 1647
		private bool _contentLoaded;
	}
}
