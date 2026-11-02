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
	// Token: 0x020000AF RID: 175
	public class CtrlAutokVSettings : UserControl, IComponentConnector
	{
		// Token: 0x060011BE RID: 4542 RVA: 0x0006B230 File Offset: 0x00069430
		public CtrlAutokVSettings(DBConnection dbConnection, ParameterHandling parHandling, int imageGroup, bool showDialogWindows, ProductManager productManager, AccessRightsManager accessManager, MachineState machineState, bool ismanualkVActive, TestcardSimService testcardSimService)
		{
			this.InitializeComponent();
			this.viewModel = new AutokVSettingsViewModel(dbConnection, parHandling, imageGroup, showDialogWindows, productManager, accessManager, machineState, ismanualkVActive, testcardSimService);
			DetectorType detectorType = DetectorType.DT;
			dbConnection.GetDetectorType(ref detectorType);
			bool flag = detectorType == DetectorType.DTNetTDI;
			if (flag)
			{
				this.EcoModeRowVisible.Height = new GridLength(0.0);
			}
			base.DataContext = this.viewModel;
		}

		// Token: 0x060011BF RID: 4543 RVA: 0x0006B2A0 File Offset: 0x000694A0
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/filter/ctrlautokvsettings.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x060011C0 RID: 4544 RVA: 0x0006B2D8 File Offset: 0x000694D8
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		internal Delegate _CreateDelegate(Type delegateType, string handler)
		{
			return Delegate.CreateDelegate(delegateType, this, handler);
		}

		// Token: 0x060011C1 RID: 4545 RVA: 0x0006B2F4 File Offset: 0x000694F4
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			if (connectionId != 1)
			{
				this._contentLoaded = true;
			}
			else
			{
				this.EcoModeRowVisible = (RowDefinition)target;
			}
		}

		// Token: 0x04000BC2 RID: 3010
		private AutokVSettingsViewModel viewModel;

		// Token: 0x04000BC3 RID: 3011
		internal RowDefinition EcoModeRowVisible;

		// Token: 0x04000BC4 RID: 3012
		private bool _contentLoaded;
	}
}
