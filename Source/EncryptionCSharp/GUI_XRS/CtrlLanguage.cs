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
	// Token: 0x02000220 RID: 544
	public class CtrlLanguage : UserControl, IComponentConnector
	{
		// Token: 0x060028F3 RID: 10483 RVA: 0x00110C69 File Offset: 0x0010EE69
		public CtrlLanguage(DBConnection dbConnection)
		{
			this.InitializeComponent();
			this.languageViewModel = new LanguageViewModel(dbConnection);
			base.DataContext = this.languageViewModel;
		}

		// Token: 0x060028F4 RID: 10484 RVA: 0x00110C94 File Offset: 0x0010EE94
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/language/ctrllanguage.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x060028F5 RID: 10485 RVA: 0x00110CCA File Offset: 0x0010EECA
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			this._contentLoaded = true;
		}

		// Token: 0x04001983 RID: 6531
		private LanguageViewModel languageViewModel;

		// Token: 0x04001984 RID: 6532
		private bool _contentLoaded;
	}
}
