using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using SesoPlot;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x020000DC RID: 220
	public class CtrlTestcardSimHistogram : UserControl, IComponentConnector
	{
		// Token: 0x060015D8 RID: 5592 RVA: 0x000837D8 File Offset: 0x000819D8
		public CtrlTestcardSimHistogram(List<PreviewItem> previewitems, ParameterHandling parHandling)
		{
			this.parHandling = parHandling;
			this.InitializeComponent();
			this.viewModel = new TestcardSimHistogramViewModel(previewitems, parHandling);
			base.DataContext = this.viewModel;
			this.InitializeButtons();
			this.plot.Graphs[0].DataContext = this.viewModel;
			this.plot.Graphs[1].DataContext = this.viewModel;
		}

		// Token: 0x060015D9 RID: 5593 RVA: 0x00083858 File Offset: 0x00081A58
		private void InitializeButtons()
		{
			this.UiElementRightAligned = new Label
			{
				Content = this.viewModel.StrHistogram.LocalizedText,
				FontSize = 32.0,
				FontWeight = FontWeights.Bold,
				FontFamily = new FontFamily("Verdana"),
				Foreground = Brushes.DarkGray,
				VerticalAlignment = VerticalAlignment.Center,
				HorizontalAlignment = HorizontalAlignment.Right,
				Margin = new Thickness(0.0, 0.0, 20.0, 0.0)
			};
		}

		// Token: 0x060015DA RID: 5594 RVA: 0x00083904 File Offset: 0x00081B04
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/testcardsimulation/ctrltestcardsimhistogram.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x060015DB RID: 5595 RVA: 0x0008393C File Offset: 0x00081B3C
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
				this.plot = (SesoPlot)target;
			}
		}

		// Token: 0x04000E68 RID: 3688
		public DockPanel PanelButtons;

		// Token: 0x04000E69 RID: 3689
		public UIElement UiElementRightAligned;

		// Token: 0x04000E6A RID: 3690
		private TestcardSimHistogramViewModel viewModel;

		// Token: 0x04000E6B RID: 3691
		private ParameterHandling parHandling;

		// Token: 0x04000E6C RID: 3692
		internal SesoPlot plot;

		// Token: 0x04000E6D RID: 3693
		private bool _contentLoaded;
	}
}
