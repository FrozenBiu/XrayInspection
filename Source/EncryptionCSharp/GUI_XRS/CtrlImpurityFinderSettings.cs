using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using DatabaseConnection;

namespace GUI_XRS
{
	// Token: 0x020000B0 RID: 176
	public class CtrlImpurityFinderSettings : UserControl, IComponentConnector
	{
		// Token: 0x060011C2 RID: 4546 RVA: 0x0006B321 File Offset: 0x00069521
		public CtrlImpurityFinderSettings(DBConnection dbConnection, ParameterHandling parHandling, int imageGroup, bool showDialogWindows, ProductManager productManager, AccessRightsManager accessManager)
		{
			this.InitializeComponent();
			this.viewModel = new ImpurityFinderSettingsViewModel(dbConnection, parHandling, imageGroup, showDialogWindows, productManager, accessManager);
			base.DataContext = this.viewModel;
		}

		// Token: 0x060011C3 RID: 4547 RVA: 0x0006B354 File Offset: 0x00069554
		private void CancelLowDensityEnableIfNeeded(object sender, EventArgs e)
		{
			bool flag = sender is CheckBox;
			if (flag)
			{
				CheckBox checkBox = (CheckBox)sender;
				bool flag2 = !this.viewModel.LowDensityEnabled;
				if (flag2)
				{
					checkBox.IsChecked = new bool?(false);
				}
			}
		}

		// Token: 0x060011C4 RID: 4548 RVA: 0x0006B39C File Offset: 0x0006959C
		private void CancelAlgorithmChangeIfNeeded(object sender, SelectionChangedEventArgs e)
		{
			bool flag = sender is ComboBox;
			if (flag)
			{
				ComboBox comboBox = (ComboBox)sender;
				ImpurityFinderTypeStringConverter impurityFinderTypeStringConverter = new ImpurityFinderTypeStringConverter();
				ImpurityFinderType impurityFinderType = (ImpurityFinderType)impurityFinderTypeStringConverter.ConvertBack(comboBox.SelectedItem.ToString(), typeof(ImpurityFinderType), null, CultureInfo.InvariantCulture);
				bool flag2 = this.viewModel.ImpurityFinderType != impurityFinderType && e.RemovedItems.Count > 0;
				if (flag2)
				{
					comboBox.SelectedItem = e.RemovedItems[0];
				}
			}
		}

		// Token: 0x060011C5 RID: 4549 RVA: 0x0001092C File Offset: 0x0000EB2C
		private void CancelLowDensityEnableIfNeeded(object sender, RoutedEventArgs e)
		{
		}

		// Token: 0x060011C6 RID: 4550 RVA: 0x0006B42C File Offset: 0x0006962C
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/filter/ctrlimpurityfindersettings.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x060011C7 RID: 4551 RVA: 0x0006B464 File Offset: 0x00069664
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				((ComboBox)target).SelectionChanged += this.CancelAlgorithmChangeIfNeeded;
				break;
			case 2:
				((CheckBox)target).Checked += this.CancelLowDensityEnableIfNeeded;
				break;
			case 3:
				((CheckBox)target).Checked += this.CancelLowDensityEnableIfNeeded;
				break;
			case 4:
				((CheckBox)target).Checked += this.CancelLowDensityEnableIfNeeded;
				break;
			case 5:
				((CheckBox)target).Checked += this.CancelLowDensityEnableIfNeeded;
				break;
			case 6:
				((CheckBox)target).Checked += this.CancelLowDensityEnableIfNeeded;
				break;
			default:
				this._contentLoaded = true;
				break;
			}
		}

		// Token: 0x04000BC5 RID: 3013
		private ImpurityFinderSettingsViewModel viewModel;

		// Token: 0x04000BC6 RID: 3014
		private bool _contentLoaded;
	}
}
