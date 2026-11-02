using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Threading;
using Microsoft.Shell;

namespace GUI_XRS
{
	// Token: 0x0200001B RID: 27
	public partial class App : Application, ISingleInstanceApp
	{
		// Token: 0x060001E4 RID: 484 RVA: 0x0000B898 File Offset: 0x00009A98
		public bool SignalExternalCommandLineArgs(IList<string> args)
		{
			return true;
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x0000B8AB File Offset: 0x00009AAB
		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x0000B8B8 File Offset: 0x00009AB8
		private void APP_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
		{
			bool flag = !this.showingExceptionDialog;
			if (flag)
			{
				this.showingExceptionDialog = true;
				Exception ex = e.Exception;
				string text = string.Empty;
				while (ex != null)
				{
					text = string.Concat(new string[]
					{
						text,
						ex.GetType().Name,
						": ",
						ex.Message,
						Environment.NewLine
					});
					ex = ex.InnerException;
				}
				Dialog.Show("Error", text, DialogStyle.Error, DialogButtons.OK);
				this.showingExceptionDialog = false;
			}
			e.Handled = true;
		}

		// Token: 0x040001B2 RID: 434
		private const string Unique = "GUI_XRS";

		// Token: 0x040001B3 RID: 435
		private bool showingExceptionDialog;
	}
}
