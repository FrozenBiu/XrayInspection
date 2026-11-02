using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;
using DatabaseConnection;

namespace GUI_XRS
{
	// Token: 0x02000153 RID: 339
	public class CtrlSettingMode : UserControl, IComponentConnector
	{
		// Token: 0x06002057 RID: 8279 RVA: 0x000D433C File Offset: 0x000D253C
		public CtrlSettingMode(Conveyor conveyor, Vibro vibro, AccessRightsManager accessManager, DBConnection dbConnection, ParameterHandling parHandling, MachineState machineState, DBDataLogs dbDatalogs, MessageSystem messageSystem, ImageDataTransfer imageDataTransfer)
		{
			this.InitializeComponent();
			this.viewModel = new SettingModeViewModel(conveyor, vibro, accessManager, dbConnection, parHandling, machineState, dbDatalogs, messageSystem, imageDataTransfer);
			base.DataContext = this.viewModel;
		}

		// Token: 0x06002058 RID: 8280 RVA: 0x000D437F File Offset: 0x000D257F
		private void CheckBox_Checked(object sender, RoutedEventArgs e)
		{
			((CheckBox)sender).GetBindingExpression(ToggleButton.IsCheckedProperty).UpdateTarget();
		}

		// Token: 0x06002059 RID: 8281 RVA: 0x000D4398 File Offset: 0x000D2598
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/settingmode/ctrlsettingmode.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x0600205A RID: 8282 RVA: 0x000D43CE File Offset: 0x000D25CE
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			this._contentLoaded = true;
		}

		// Token: 0x0400150C RID: 5388
		private SettingModeViewModel viewModel;

		// Token: 0x0400150D RID: 5389
		private bool _contentLoaded;
	}
}
