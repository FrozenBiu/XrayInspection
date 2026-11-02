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
	// Token: 0x020000A4 RID: 164
	public class CtrlHardwareFeatures : UserControl, IComponentConnector
	{
		// Token: 0x06001043 RID: 4163 RVA: 0x00065754 File Offset: 0x00063954
		public CtrlHardwareFeatures(DBConnection dbConnection, ParameterHandling parHandling, Conveyor conveyor)
		{
			this.InitializeComponent();
			this.viewModel = new HardwareFeaturesViewModel(dbConnection, parHandling, conveyor);
			base.DataContext = this.viewModel;
		}

		// Token: 0x06001044 RID: 4164 RVA: 0x00065780 File Offset: 0x00063980
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/featureselection/ctrlhardwarefeatures.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x06001045 RID: 4165 RVA: 0x000657B6 File Offset: 0x000639B6
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			this._contentLoaded = true;
		}

		// Token: 0x04000AE7 RID: 2791
		private HardwareFeaturesViewModel viewModel;

		// Token: 0x04000AE8 RID: 2792
		private bool _contentLoaded;
	}
}
