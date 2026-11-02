using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using DatabaseConnection;
using Definitions;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x0200017B RID: 379
	public class CtrlRestoreList : UserControl, IComponentConnector
	{
		// Token: 0x170009EB RID: 2539
		// (get) Token: 0x06002269 RID: 8809 RVA: 0x000DE22C File Offset: 0x000DC42C
		// (set) Token: 0x0600226A RID: 8810 RVA: 0x000DE234 File Offset: 0x000DC434
		public List<Button> Buttons { get; set; }

		// Token: 0x0600226B RID: 8811 RVA: 0x000DE23D File Offset: 0x000DC43D
		public CtrlRestoreList(DBConnection dbConnection, ParameterHandling parHandling, Restore restore)
		{
			this.Initialize(dbConnection, restore, parHandling, false, null, false, false);
		}

		// Token: 0x0600226C RID: 8812 RVA: 0x000DE255 File Offset: 0x000DC455
		public CtrlRestoreList(DBConnection dbConnection, Restore restore, AccessRightsManager accessManager)
		{
			this.accessManager = accessManager;
			this.Initialize(dbConnection, restore, null, true, accessManager, false, false);
		}

		// Token: 0x0600226D RID: 8813 RVA: 0x000DE274 File Offset: 0x000DC474
		public CtrlRestoreList(DBConnection dbConnection, ParameterHandling parHandling, Restore restore, bool isIVP, AccessRightsManager accessManager, bool checkforEX2A)
		{
			this.accessManager = accessManager;
			this.Initialize(dbConnection, restore, parHandling, false, null, isIVP, checkforEX2A);
		}

		// Token: 0x0600226E RID: 8814 RVA: 0x000DE298 File Offset: 0x000DC498
		private void Initialize(DBConnection dbConnection, Restore restore, ParameterHandling parHandling, bool restoreUsers, AccessRightsManager accessManager, bool isIVP, bool checkforEX2A)
		{
			this.InitializeComponent();
			this.viewModel = new RestoreListViewModel(dbConnection, restore, restoreUsers, parHandling, accessManager, restore.RestoreSystem, isIVP, checkforEX2A);
			this.InitializeButtons(restoreUsers, isIVP, dbConnection, restore);
			base.DataContext = this.viewModel;
		}

		// Token: 0x0600226F RID: 8815 RVA: 0x000DE2E4 File Offset: 0x000DC4E4
		private void InitializeButtons(bool restoreUsers, bool isIVP, DBConnection dbConnection, Restore restore)
		{
			this.Buttons = new List<Button>();
			Button button = new Button();
			bool flag = !restoreUsers;
			if (flag)
			{
				Button button2 = new Button();
				Image image = ImageConverter.ToImage("/img/restore.png");
				image.Height = 45.0;
				image.Stretch = Stretch.Uniform;
				button2.Content = image;
				button2.Style = (base.FindResource("defaultButtonStyle") as Style);
				button2.Command = this.viewModel.DoRestoreCommand;
				this.Buttons.Add(button2);
				bool flag2 = !isIVP && !restore.RestoreSystem;
				if (flag2)
				{
					bool flag3 = this.accessManager.CurrentUser.Rights >= BuiltInPriviledges.Admin;
					if (flag3)
					{
						bool flag4 = false;
						dbConnection.GetIntegratedValidationProcess(ref flag4);
						bool flag5 = flag4;
						if (flag5)
						{
							Button button3 = new Button();
							Image image2 = ImageConverter.ToImage("/img/certificate_color2.png");
							image2.Height = 45.0;
							image2.Stretch = Stretch.Uniform;
							button3.Content = image2;
							button3.Style = (base.FindResource("defaultButtonStyle") as Style);
							button3.Command = this.viewModel.ShowIVPCommand;
							this.Buttons.Add(button3);
						}
					}
				}
			}
			else
			{
				Button button4 = new Button();
				Image image3 = ImageConverter.ToImage("/img/importusers.png");
				image3.Height = 45.0;
				image3.Stretch = Stretch.Uniform;
				button4.Content = image3;
				button4.Style = (base.FindResource("defaultButtonStyle") as Style);
				button4.Command = this.viewModel.DoRestoreCommand;
				this.Buttons.Add(button4);
			}
			Image image4 = ImageConverter.ToImage("/img/info.png");
			image4.Height = 45.0;
			image4.Stretch = Stretch.Uniform;
			button.Content = image4;
			button.Style = (base.FindResource("defaultButtonStyle") as Style);
			button.Command = this.viewModel.ShowInfoCommand;
			this.Buttons.Add(button);
		}

		// Token: 0x06002270 RID: 8816 RVA: 0x000DE518 File Offset: 0x000DC718
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/backup/ctrlrestorelist.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x06002271 RID: 8817 RVA: 0x000DE550 File Offset: 0x000DC750
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
				this.lstBackups = (ListBox)target;
			}
		}

		// Token: 0x0400165F RID: 5727
		private RestoreListViewModel viewModel;

		// Token: 0x04001661 RID: 5729
		private AccessRightsManager accessManager;

		// Token: 0x04001662 RID: 5730
		internal ListBox lstBackups;

		// Token: 0x04001663 RID: 5731
		private bool _contentLoaded;
	}
}
