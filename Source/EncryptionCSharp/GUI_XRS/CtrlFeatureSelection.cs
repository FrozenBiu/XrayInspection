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
	// Token: 0x020000A3 RID: 163
	public class CtrlFeatureSelection : UserControl, IComponentConnector
	{
		// Token: 0x06001040 RID: 4160 RVA: 0x000656E7 File Offset: 0x000638E7
		public CtrlFeatureSelection(DBConnection dbConnection, ParameterHandling parHandling, TestcardSimService testcardSimService)
		{
			this.InitializeComponent();
			this.viewModel = new FeatureSelectionViewModel(dbConnection, parHandling, testcardSimService);
			base.DataContext = this.viewModel;
		}

		// Token: 0x06001041 RID: 4161 RVA: 0x00065714 File Offset: 0x00063914
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/featureselection/ctrlfeatureselection.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x06001042 RID: 4162 RVA: 0x0006574A File Offset: 0x0006394A
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			this._contentLoaded = true;
		}

		// Token: 0x04000AE5 RID: 2789
		private FeatureSelectionViewModel viewModel;

		// Token: 0x04000AE6 RID: 2790
		private bool _contentLoaded;
	}
}
