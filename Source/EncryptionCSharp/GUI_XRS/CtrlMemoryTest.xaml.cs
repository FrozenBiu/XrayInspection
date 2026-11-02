using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace GUI_XRS
{
	// Token: 0x0200009B RID: 155
	public partial class CtrlMemoryTest : UserControl
	{
		// Token: 0x06000FA1 RID: 4001 RVA: 0x0005D51F File Offset: 0x0005B71F
		public CtrlMemoryTest()
		{
			this.InitializeComponent();
		}

		// Token: 0x04000A74 RID: 2676
		private int[] x = new int[5000000];
	}
}
