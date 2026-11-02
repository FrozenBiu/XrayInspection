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
	// Token: 0x02000111 RID: 273
	public class CtrlErrorContact : UserControl, IComponentConnector
	{
		// Token: 0x060019C6 RID: 6598 RVA: 0x00097992 File Offset: 0x00095B92
		public CtrlErrorContact(ErrorHandling errorHandling, ParameterHandling parHandling, DBConnection dbConnection, AccessRightsManager accessManager)
		{
			this.parHandling = parHandling;
			this.InitializeComponent();
			this.viewModel = new ErrorContactViewModel(errorHandling, parHandling, dbConnection, accessManager);
			base.DataContext = this.viewModel;
		}

		// Token: 0x060019C7 RID: 6599 RVA: 0x000979C8 File Offset: 0x00095BC8
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/errorhandling/ctrlerrorcontact.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x060019C8 RID: 6600 RVA: 0x000979FE File Offset: 0x00095BFE
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			this._contentLoaded = true;
		}

		// Token: 0x040010DA RID: 4314
		private ErrorContactViewModel viewModel;

		// Token: 0x040010DB RID: 4315
		private ParameterHandling parHandling;

		// Token: 0x040010DC RID: 4316
		private bool _contentLoaded;
	}
}
