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
	// Token: 0x0200006E RID: 110
	public class CtrlOverviewIVPReferenceProduct : UserControl, IComponentConnector
	{
		// Token: 0x060009DA RID: 2522 RVA: 0x0003C881 File Offset: 0x0003AA81
		public CtrlOverviewIVPReferenceProduct(DBConnection dBConnection, AccessRightsManager accessManager)
		{
			this.viewModel = new IVPOverviewReferenceProductViewModel(dBConnection, accessManager);
			this.InitializeComponent();
			base.DataContext = this.viewModel;
		}

		// Token: 0x060009DB RID: 2523 RVA: 0x0001092C File Offset: 0x0000EB2C
		private void UserControl_Unloaded(object sender, RoutedEventArgs e)
		{
		}

		// Token: 0x060009DC RID: 2524 RVA: 0x0001092C File Offset: 0x0000EB2C
		private void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
		}

		// Token: 0x060009DD RID: 2525 RVA: 0x0003C8AC File Offset: 0x0003AAAC
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/integrated%20validation%20process/ctrloverviewivpreferenceproduct.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x060009DE RID: 2526 RVA: 0x0003C8E4 File Offset: 0x0003AAE4
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
				((CtrlOverviewIVPReferenceProduct)target).Unloaded += this.UserControl_Unloaded;
				((CtrlOverviewIVPReferenceProduct)target).Loaded += this.UserControl_Loaded;
			}
		}

		// Token: 0x040006C0 RID: 1728
		private IVPOverviewReferenceProductViewModel viewModel;

		// Token: 0x040006C1 RID: 1729
		private bool _contentLoaded;
	}
}
