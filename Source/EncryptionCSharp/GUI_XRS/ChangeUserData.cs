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
	// Token: 0x02000226 RID: 550
	public class ChangeUserData : UserControl, IComponentConnector
	{
		// Token: 0x17000B35 RID: 2869
		// (get) Token: 0x0600291F RID: 10527 RVA: 0x0011160F File Offset: 0x0010F80F
		// (set) Token: 0x06002920 RID: 10528 RVA: 0x00111617 File Offset: 0x0010F817
		public List<Button> Buttons { get; set; }

		// Token: 0x06002921 RID: 10529 RVA: 0x00111620 File Offset: 0x0010F820
		public ChangeUserData(DBConnection dbConnection, ChangeUserData.ChangeUserDataType type, UsermanagementViewModel usermanagementViewModel)
		{
			this.InitializeComponent();
			this.InitializeObjects();
			this.dbConnection = dbConnection;
			this.type = type;
			this.usermanagementViewModel = usermanagementViewModel;
			this.changeUserDataViewModel = new ChangeUserDataViewModel(dbConnection, type, usermanagementViewModel);
			base.DataContext = this.changeUserDataViewModel;
			this.pbPassword.Password = ((type == ChangeUserData.ChangeUserDataType.newUser) ? "" : "Dummy");
			this.pbPasswordRepeate.Password = ((type == ChangeUserData.ChangeUserDataType.newUser) ? "" : "Dummy");
			this.changeUserDataViewModel.PasswordEntered += this.changeUserDataViewModel_PasswordEntered;
		}

		// Token: 0x06002922 RID: 10530 RVA: 0x001116C4 File Offset: 0x0010F8C4
		private void InitializeObjects()
		{
			this.Buttons = new List<Button>();
			Button button = new Button();
			button.Content = ImageConverter.ToImage("/img/ok.png");
			button.Click += this.btnOk_Click;
			button.Style = (base.FindResource("okButtonStyle") as Style);
			this.Buttons.Add(button);
		}

		// Token: 0x06002923 RID: 10531 RVA: 0x0011172C File Offset: 0x0010F92C
		private void btnOk_Click(object sender, RoutedEventArgs e)
		{
			bool flag = this.changeUserDataViewModel.SubmitChanges();
			if (flag)
			{
				Window window = Window.GetWindow(this);
				window.Close();
			}
		}

		// Token: 0x06002924 RID: 10532 RVA: 0x0011175C File Offset: 0x0010F95C
		private void changeUserDataViewModel_PasswordEntered(bool repeatBox)
		{
			bool flag = !repeatBox;
			if (flag)
			{
				this.pbPassword.Password = this.changeUserDataViewModel.EnteredPassword;
			}
			else
			{
				this.pbPasswordRepeate.Password = this.changeUserDataViewModel.EnteredPasswordRepeat;
			}
		}

		// Token: 0x06002925 RID: 10533 RVA: 0x001117A4 File Offset: 0x0010F9A4
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/usermanagement/changeuserdatacontrol.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x06002926 RID: 10534 RVA: 0x001117DC File Offset: 0x0010F9DC
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				this.tbUserName = (TextBlock)target;
				break;
			case 2:
				this.tbUserGroup = (TextBlock)target;
				break;
			case 3:
				this.txtID = (TextBox)target;
				break;
			case 4:
				this.txtUsername = (TextBox)target;
				break;
			case 5:
				this.pbPassword = (PasswordBox)target;
				break;
			case 6:
				this.pbPasswordRepeate = (PasswordBox)target;
				break;
			default:
				this._contentLoaded = true;
				break;
			}
		}

		// Token: 0x04001993 RID: 6547
		private ChangeUserDataViewModel changeUserDataViewModel;

		// Token: 0x04001994 RID: 6548
		private DBConnection dbConnection;

		// Token: 0x04001995 RID: 6549
		private ChangeUserData.ChangeUserDataType type;

		// Token: 0x04001996 RID: 6550
		private UsermanagementViewModel usermanagementViewModel;

		// Token: 0x04001998 RID: 6552
		internal TextBlock tbUserName;

		// Token: 0x04001999 RID: 6553
		internal TextBlock tbUserGroup;

		// Token: 0x0400199A RID: 6554
		internal TextBox txtID;

		// Token: 0x0400199B RID: 6555
		internal TextBox txtUsername;

		// Token: 0x0400199C RID: 6556
		internal PasswordBox pbPassword;

		// Token: 0x0400199D RID: 6557
		internal PasswordBox pbPasswordRepeate;

		// Token: 0x0400199E RID: 6558
		private bool _contentLoaded;

		// Token: 0x0200036A RID: 874
		public enum ChangeUserDataType
		{
			// Token: 0x040021C6 RID: 8646
			newUser,
			// Token: 0x040021C7 RID: 8647
			changeUserdata
		}
	}
}
