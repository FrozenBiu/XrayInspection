using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using GUI_XRS.AuditCheck;

namespace GUI_XRS.AuditCheckNew
{
	// Token: 0x02000278 RID: 632
	public partial class SignatureWindow : Window
	{
		// Token: 0x060034B5 RID: 13493 RVA: 0x0014F38C File Offset: 0x0014D58C
		public SignatureWindow()
		{
			this.InitializeComponent();
			this.viewModel = new SignatureWindowViewModel();
			base.DataContext = this.viewModel;
		}

		// Token: 0x04001EEF RID: 7919
		private SignatureWindowViewModel viewModel;
	}
}
