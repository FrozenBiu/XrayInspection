using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using Microsoft.Windows.Controls;

namespace GUI_XRS
{
	// Token: 0x020000DD RID: 221
	public class CtrlTestcardSimStatisticsTable : UserControl, IComponentConnector
	{
		// Token: 0x060015DC RID: 5596 RVA: 0x0008396C File Offset: 0x00081B6C
		public CtrlTestcardSimStatisticsTable()
		{
			this.InitializeComponent();
			this.viewModel = new TestcardSimStatisticsTableViewModel();
			this.InitializeButtons();
			this.vasensDataGrid.ItemsSource = TestcardSimStatisticsViewModel.SensitivityVAPerImgList;
			this.glasssensDataGrid.ItemsSource = TestcardSimStatisticsViewModel.SensitivityGlassPerImgList;
			base.DataContext = this.viewModel;
		}

		// Token: 0x060015DD RID: 5597 RVA: 0x000839CC File Offset: 0x00081BCC
		private void InitializeButtons()
		{
			this.UiElementRightAligned = new Label
			{
				Content = this.viewModel.StrStatisticTable.LocalizedText,
				FontSize = 32.0,
				FontWeight = FontWeights.Bold,
				FontFamily = new FontFamily("Verdana"),
				Foreground = Brushes.DarkGray,
				VerticalAlignment = VerticalAlignment.Center,
				HorizontalAlignment = HorizontalAlignment.Right,
				Margin = new Thickness(0.0, 0.0, 20.0, 0.0)
			};
		}

		// Token: 0x060015DE RID: 5598 RVA: 0x00083A78 File Offset: 0x00081C78
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/testcardsimulation/ctrltestcardsimstatisticstable.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x060015DF RID: 5599 RVA: 0x00083AB0 File Offset: 0x00081CB0
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
					this.glasssensDataGrid = (Microsoft.Windows.Controls.DataGrid)target;
				}
			}
			else
			{
				this.vasensDataGrid = (Microsoft.Windows.Controls.DataGrid)target;
			}
		}

		// Token: 0x04000E6E RID: 3694
		private TestcardSimStatisticsTableViewModel viewModel;

		// Token: 0x04000E6F RID: 3695
		public DockPanel PanelButtons;

		// Token: 0x04000E70 RID: 3696
		public UIElement UiElementRightAligned;

		// Token: 0x04000E71 RID: 3697
		internal Microsoft.Windows.Controls.DataGrid vasensDataGrid;

		// Token: 0x04000E72 RID: 3698
		internal Microsoft.Windows.Controls.DataGrid glasssensDataGrid;

		// Token: 0x04000E73 RID: 3699
		private bool _contentLoaded;
	}
}
