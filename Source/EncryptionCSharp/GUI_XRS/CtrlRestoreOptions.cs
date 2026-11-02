using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using DatabaseConnection;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x0200017E RID: 382
	public class CtrlRestoreOptions : UserControl, IComponentConnector
	{
		// Token: 0x170009FF RID: 2559
		// (get) Token: 0x060022BB RID: 8891 RVA: 0x000DFEF2 File Offset: 0x000DE0F2
		// (set) Token: 0x060022BC RID: 8892 RVA: 0x000DFEFA File Offset: 0x000DE0FA
		public List<Button> Buttons { get; set; }

		// Token: 0x060022BD RID: 8893 RVA: 0x000DFF03 File Offset: 0x000DE103
		public CtrlRestoreOptions(DBConnection dbConnection, ParameterHandling parHandling, Restore restore, AccessRightsManager accessManager, TestcardSimService testcardSimService)
		{
			this.InitializeComponent();
			this.viewModel = new RestoreOptionsViewModel(dbConnection, parHandling, restore, accessManager, testcardSimService);
			base.DataContext = this.viewModel;
			this.InitializeButtons();
		}

		// Token: 0x060022BE RID: 8894 RVA: 0x000DFF3C File Offset: 0x000DE13C
		private void InitializeButtons()
		{
			this.Buttons = new List<Button>();
			Button button = new Button();
			button.Content = ImageConverter.ToImage("/img/usb.png");
			button.Style = (base.FindResource("btnStyleNoBorders") as Style);
			button.Visibility = Visibility.Hidden;
			this.Buttons.Add(button);
			this.viewModel.StartUsbSurvey(button);
		}

		// Token: 0x060022BF RID: 8895 RVA: 0x000DFFA8 File Offset: 0x000DE1A8
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/backup/ctrlrestoreoptions.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x060022C0 RID: 8896 RVA: 0x000DFFDE File Offset: 0x000DE1DE
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			this._contentLoaded = true;
		}

		// Token: 0x0400168B RID: 5771
		private RestoreOptionsViewModel viewModel;

		// Token: 0x0400168D RID: 5773
		private bool _contentLoaded;
	}
}
