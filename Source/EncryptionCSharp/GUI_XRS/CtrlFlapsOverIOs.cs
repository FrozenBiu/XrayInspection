using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using DatabaseConnection;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x02000047 RID: 71
	public class CtrlFlapsOverIOs : UserControl, IComponentConnector
	{
		// Token: 0x06000700 RID: 1792 RVA: 0x0002F55A File Offset: 0x0002D75A
		public CtrlFlapsOverIOs(ParameterHandling parHandling, DBConnection dbConnection, MachineState machineState, AccessRightsManager accessManager, MessageSystem messageSystem, MachineMode machineMode)
		{
			this.InitializeComponent();
			this.viewModel = new FlapsOverIOsViewModel(parHandling, dbConnection, machineState, accessManager, messageSystem, machineMode);
			base.DataContext = this.viewModel;
		}

		// Token: 0x06000701 RID: 1793 RVA: 0x0002F58C File Offset: 0x0002D78C
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/bulk/flapsoverios/ctrlflapsoverios.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x06000702 RID: 1794 RVA: 0x0002F5C4 File Offset: 0x0002D7C4
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
					this.btnVerifyTest = (Button)target;
				}
			}
			else
			{
				this.btnVerifyLearn = (Button)target;
			}
		}

		// Token: 0x040004D3 RID: 1235
		private FlapsOverIOsViewModel viewModel;

		// Token: 0x040004D4 RID: 1236
		internal Button btnVerifyLearn;

		// Token: 0x040004D5 RID: 1237
		internal Button btnVerifyTest;

		// Token: 0x040004D6 RID: 1238
		private bool _contentLoaded;
	}
}
