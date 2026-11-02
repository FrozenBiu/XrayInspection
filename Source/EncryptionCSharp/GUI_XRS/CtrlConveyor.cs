using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using DatabaseConnection;

namespace GUI_XRS
{
	// Token: 0x02000182 RID: 386
	public class CtrlConveyor : UserControl, IComponentConnector
	{
		// Token: 0x060022F4 RID: 8948 RVA: 0x000E1461 File Offset: 0x000DF661
		public CtrlConveyor(Conveyor conveyor, AccessRightsManager accessManager, DBConnection dbConnection, MachineState machineState, DBDataLogs dbDatalogs, ParameterHandling parHandling)
		{
			this.InitializeComponent();
			this.viewModel = new ConveyorViewModel(conveyor, accessManager, dbConnection, machineState, true, dbDatalogs, parHandling);
			base.DataContext = this.viewModel;
		}

		// Token: 0x060022F5 RID: 8949 RVA: 0x000E1494 File Offset: 0x000DF694
		public CtrlConveyor(Conveyor conveyor, AccessRightsManager accessManager, DBConnection dbConnection, MachineState machineState, DBDataLogs dbDatalogs)
		{
			this.InitializeComponent();
			this.viewModel = new ConveyorViewModel(conveyor, accessManager, dbConnection, machineState, true, dbDatalogs, null);
			base.DataContext = this.viewModel;
		}

		// Token: 0x060022F6 RID: 8950 RVA: 0x000E14C8 File Offset: 0x000DF6C8
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/conveyor/ctrlconveyor.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x060022F7 RID: 8951 RVA: 0x000E1500 File Offset: 0x000DF700
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		internal Delegate _CreateDelegate(Type delegateType, string handler)
		{
			return Delegate.CreateDelegate(delegateType, this, handler);
		}

		// Token: 0x060022F8 RID: 8952 RVA: 0x000E151C File Offset: 0x000DF71C
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				this.btnLowerSpeed = (Button)target;
				break;
			case 2:
				this.txtSpeed = (TextBox)target;
				break;
			case 3:
				this.btnHigherSpeed = (Button)target;
				break;
			default:
				this._contentLoaded = true;
				break;
			}
		}

		// Token: 0x040016AA RID: 5802
		private ConveyorViewModel viewModel;

		// Token: 0x040016AB RID: 5803
		internal Button btnLowerSpeed;

		// Token: 0x040016AC RID: 5804
		internal TextBox txtSpeed;

		// Token: 0x040016AD RID: 5805
		internal Button btnHigherSpeed;

		// Token: 0x040016AE RID: 5806
		private bool _contentLoaded;
	}
}
