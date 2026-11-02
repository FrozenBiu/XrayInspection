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
	// Token: 0x02000124 RID: 292
	public class CtrlMachineParameter : UserControl, IComponentConnector
	{
		// Token: 0x06001C25 RID: 7205 RVA: 0x000B5639 File Offset: 0x000B3839
		public CtrlMachineParameter(DBConnection dbConnection, AccessRightsManager accessManager, ParameterHandling parHandling)
		{
			this.InitializeComponent();
			this.viewModel = new MachineParameterViewModel(dbConnection, accessManager, parHandling);
			base.DataContext = this.viewModel;
		}

		// Token: 0x06001C26 RID: 7206 RVA: 0x000B5668 File Offset: 0x000B3868
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/machineparameter/ctrlmachineparameter.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x06001C27 RID: 7207 RVA: 0x000B569E File Offset: 0x000B389E
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			this._contentLoaded = true;
		}

		// Token: 0x040012A8 RID: 4776
		private MachineParameterViewModel viewModel;

		// Token: 0x040012A9 RID: 4777
		private bool _contentLoaded;
	}
}
