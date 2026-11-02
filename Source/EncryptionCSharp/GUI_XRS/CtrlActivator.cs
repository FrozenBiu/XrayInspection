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
	// Token: 0x02000042 RID: 66
	public class CtrlActivator : UserControl, IComponentConnector
	{
		// Token: 0x060006BD RID: 1725 RVA: 0x0002C69C File Offset: 0x0002A89C
		public CtrlActivator(DBConnection dbConnection, ParameterHandling parHandling)
		{
			this.InitializeComponent();
			this.viewModel = new ActivatorViewModel(dbConnection, parHandling);
			base.DataContext = this.viewModel;
		}

		// Token: 0x060006BE RID: 1726 RVA: 0x0002C6C8 File Offset: 0x0002A8C8
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/bulk/activator/ctrlactivator.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x060006BF RID: 1727 RVA: 0x0002C700 File Offset: 0x0002A900
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		internal Delegate _CreateDelegate(Type delegateType, string handler)
		{
			return Delegate.CreateDelegate(delegateType, this, handler);
		}

		// Token: 0x060006C0 RID: 1728 RVA: 0x0002C71A File Offset: 0x0002A91A
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			this._contentLoaded = true;
		}

		// Token: 0x040004B6 RID: 1206
		private ActivatorViewModel viewModel;

		// Token: 0x040004B7 RID: 1207
		private bool _contentLoaded;
	}
}
