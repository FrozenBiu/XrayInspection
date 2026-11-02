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
	// Token: 0x02000180 RID: 384
	public class CtrlBackupOptions : UserControl, IComponentConnector
	{
		// Token: 0x17000A00 RID: 2560
		// (get) Token: 0x060022C4 RID: 8900 RVA: 0x000E0081 File Offset: 0x000DE281
		// (set) Token: 0x060022C5 RID: 8901 RVA: 0x000E0089 File Offset: 0x000DE289
		public List<Button> Buttons { get; set; }

		// Token: 0x060022C6 RID: 8902 RVA: 0x000E0092 File Offset: 0x000DE292
		public CtrlBackupOptions(DBConnection dbConnection, Backup backup, ParameterHandling parHandling, ProductManager productManager)
		{
			this.InitializeComponent();
			this.backupOptionsViewModel = new BackupOptionsViewModel(dbConnection, backup, parHandling, productManager);
			base.DataContext = this.backupOptionsViewModel;
			this.InitializeButtons();
		}

		// Token: 0x060022C7 RID: 8903 RVA: 0x000E00C8 File Offset: 0x000DE2C8
		private void InitializeButtons()
		{
			this.Buttons = new List<Button>();
			Button button = new Button();
			button.Content = ImageConverter.ToImage("/img/usb.png");
			button.Style = (base.FindResource("btnStyleNoBorders") as Style);
			button.Visibility = Visibility.Hidden;
			this.Buttons.Add(button);
			this.backupOptionsViewModel.StartUsbSurvey(button);
		}

		// Token: 0x060022C8 RID: 8904 RVA: 0x000E0134 File Offset: 0x000DE334
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/backup/ctrlbackupoptions.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x060022C9 RID: 8905 RVA: 0x000E016A File Offset: 0x000DE36A
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			this._contentLoaded = true;
		}

		// Token: 0x04001691 RID: 5777
		private BackupOptionsViewModel backupOptionsViewModel;

		// Token: 0x04001693 RID: 5779
		private bool _contentLoaded;
	}
}
