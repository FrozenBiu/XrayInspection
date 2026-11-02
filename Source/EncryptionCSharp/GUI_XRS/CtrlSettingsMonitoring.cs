using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using DatabaseConnection;
using GUI_XRS.Datalogger;

namespace GUI_XRS
{
	// Token: 0x02000083 RID: 131
	public class CtrlSettingsMonitoring : UserControl, IComponentConnector
	{
		// Token: 0x06000CFE RID: 3326 RVA: 0x0004C839 File Offset: 0x0004AA39
		public CtrlSettingsMonitoring(DBDataLogs dbDataLogs, ParameterHandling parHandling, DBConnection dBConnection)
		{
			this.InitializeComponent();
			this.viewModel = new SettingsMonitoringViewModel(dbDataLogs, parHandling);
			this.dbDataLogs = dbDataLogs;
			this.parHandling = parHandling;
			this.dbConnection = dBConnection;
			base.DataContext = this.viewModel;
		}

		// Token: 0x06000CFF RID: 3327 RVA: 0x0004C87C File Offset: 0x0004AA7C
		private void ButtonResetLimits_Click(object sender, RoutedEventArgs e)
		{
			bool flag = Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtReallyResetLimits"), DialogStyle.Warning, DialogButtons.OKCancel) == Dialog.DialogResults.Ok;
			if (flag)
			{
				this.viewModel.resetLimits();
			}
		}

		// Token: 0x06000D00 RID: 3328 RVA: 0x0004C8BC File Offset: 0x0004AABC
		private void ButtonResetCounter_Click(object sender, RoutedEventArgs e)
		{
			CtrlDatalogsPredMainResets usrControl = new CtrlDatalogsPredMainResets(this.dbDataLogs, this.dbConnection, this.parHandling);
			Subwindow.Show(usrControl, SubwindowButtons.BackAndHome);
		}

		// Token: 0x06000D01 RID: 3329 RVA: 0x0004C8EA File Offset: 0x0004AAEA
		private void TextBox_MouseUp(object sender, MouseButtonEventArgs e)
		{
			this.viewModel.getNumKeyboardNumber();
		}

		// Token: 0x06000D02 RID: 3330 RVA: 0x0004C8FC File Offset: 0x0004AAFC
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/monitoring/ctrlsettingsmonitoring.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x06000D03 RID: 3331 RVA: 0x0004C934 File Offset: 0x0004AB34
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				((TextBox)target).MouseUp += this.TextBox_MouseUp;
				break;
			case 2:
				((Button)target).Click += this.ButtonResetLimits_Click;
				break;
			case 3:
				((Button)target).Click += this.ButtonResetCounter_Click;
				break;
			default:
				this._contentLoaded = true;
				break;
			}
		}

		// Token: 0x04000881 RID: 2177
		private SettingsMonitoringViewModel viewModel;

		// Token: 0x04000882 RID: 2178
		private DBDataLogs dbDataLogs;

		// Token: 0x04000883 RID: 2179
		private ParameterHandling parHandling;

		// Token: 0x04000884 RID: 2180
		private DBConnection dbConnection;

		// Token: 0x04000885 RID: 2181
		private bool _contentLoaded;
	}
}
