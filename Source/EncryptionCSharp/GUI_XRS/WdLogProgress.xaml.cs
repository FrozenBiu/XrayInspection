using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media.Animation;

namespace GUI_XRS
{
	// Token: 0x02000164 RID: 356
	public partial class WdLogProgress : Window
	{
		// Token: 0x06002136 RID: 8502 RVA: 0x000D7FF9 File Offset: 0x000D61F9
		public WdLogProgress(ParameterHandling parHandling, MeanPixelLogger meanPixelLogger, bool offsetMode)
		{
			this.InitializeComponent();
			this.viewModel = new LogProgressViewModel(parHandling, meanPixelLogger, offsetMode);
			base.DataContext = this.viewModel;
		}

		// Token: 0x06002137 RID: 8503 RVA: 0x00075F4E File Offset: 0x0007414E
		private void FormFadeOut_Completed(object sender, EventArgs e)
		{
			base.Close();
		}

		// Token: 0x06002138 RID: 8504 RVA: 0x000D8025 File Offset: 0x000D6225
		private void btnOk_Click(object sender, RoutedEventArgs e)
		{
			this.FormFadeOut.Begin();
		}

		// Token: 0x0400157C RID: 5500
		private LogProgressViewModel viewModel;
	}
}
