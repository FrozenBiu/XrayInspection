using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using DatabaseConnection;
using Definitions;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x02000258 RID: 600
	public class UsermanagementControl : UserControl, IComponentConnector, IStyleConnector
	{
		// Token: 0x17000C56 RID: 3158
		// (get) Token: 0x06002E08 RID: 11784 RVA: 0x0012CE1E File Offset: 0x0012B01E
		// (set) Token: 0x06002E09 RID: 11785 RVA: 0x0012CE26 File Offset: 0x0012B026
		public List<Button> Buttons { get; set; }

		// Token: 0x06002E0A RID: 11786 RVA: 0x0012CE30 File Offset: 0x0012B030
		public UsermanagementControl(DBConnection dbConnection, AccessRightsManager accessManager, Restore restore, bool multipleRfid, SetFooterMainWindow setFooterMainWindow, LoginToGo loginToGo)
		{
			this.dbConnection = dbConnection;
			this.accessManager = accessManager;
			this.setFooterMainWindow = setFooterMainWindow;
			this.InitializeButtons();
			this.SetControlRights();
			this.usermanagementViewModel = new UsermanagementViewModel(dbConnection, accessManager, restore, loginToGo, multipleRfid);
			this.usermanagementViewModel.UserChanged += this.usermanagementViewModel_UserChanged;
			base.DataContext = this.usermanagementViewModel;
			this.InitializeComponent();
			setFooterMainWindow.SetFooterDeactivation();
		}

		// Token: 0x06002E0B RID: 11787 RVA: 0x0012CF30 File Offset: 0x0012B130
		private void usermanagementViewModel_UserChanged()
		{
			bool flag = this.accessManager.CurrentUser.Rights < BuiltInPriviledges.Admin;
			if (flag)
			{
				(Window.GetWindow(this) as Subwindow).Close();
			}
		}

		// Token: 0x06002E0C RID: 11788 RVA: 0x0012CF6C File Offset: 0x0012B16C
		public void SetControlRights()
		{
			this.accessManager.RegisterControl(this.btnChange, this.rightChange, this.rightChange);
			this.accessManager.RegisterControl(this.btnNew, this.rightNew, this.rightNew);
			this.accessManager.RegisterControl(this.btnDelete, this.rightDelete, this.rightDelete);
			this.accessManager.RegisterControl(this.btnRename, this.rightRename, this.rightRename);
			this.accessManager.RegisterControl(this.btnImportUsers, this.rightImportUsers, this.rightImportUsers);
			this.accessManager.RegisterControl(this.btnLoginToGo, this.rightLoginToGo, this.rightLoginToGo);
			this.accessManager.RegisterControl(this.btnLoginRfIdAdd, this.rightLearnRfId, this.rightLearnRfId);
			this.accessManager.RegisterControl(this.btnLoginRfIdDel, this.rightDeleteRfId, this.rightDeleteRfId);
			this.accessManager.RegisterControl(this.btnLoginRfIdInf, this.rightInfoRfId, this.rightInfoRfId);
		}

		// Token: 0x06002E0D RID: 11789 RVA: 0x0012D088 File Offset: 0x0012B288
		private void InitializeButtons()
		{
			this.Buttons = new List<Button>();
			string usbLetter = "";
			bool flag = false;
			this.dbConnection.GetUsbDriveLetter(ref usbLetter);
			this.dbConnection.GetRFIDLoginAvailable(ref flag);
			this.btnChange = new Button();
			this.btnChange.Content = ImageConverter.ToImage("/img/changeuser.png");
			this.btnChange.Click += this.btnChange_Click;
			this.btnChange.Style = (base.FindResource("defaultButtonStyle") as Style);
			this.Buttons.Add(this.btnChange);
			this.btnNew = new Button();
			this.btnNew.Content = ImageConverter.ToImage("/img/adduser.png");
			this.btnNew.Click += this.btnNew_Click;
			this.btnNew.Style = (base.FindResource("defaultButtonStyle") as Style);
			this.Buttons.Add(this.btnNew);
			this.btnDelete = new Button();
			this.btnDelete.Content = ImageConverter.ToImage("/img/deleteuser.png");
			this.btnDelete.Click += this.btnDelete_Click;
			this.btnDelete.Style = (base.FindResource("defaultButtonStyle") as Style);
			this.Buttons.Add(this.btnDelete);
			this.btnRename = new Button();
			this.btnRename.Content = ImageConverter.ToImage("/img/edituser.png");
			this.btnRename.Click += this.btnRename_Click;
			this.btnRename.Style = (base.FindResource("defaultButtonStyle") as Style);
			this.Buttons.Add(this.btnRename);
			this.btnImportUsers = new Button();
			Image image = ImageConverter.ToImage("/img/importusers.png");
			image.Height = 45.0;
			image.Stretch = Stretch.Uniform;
			this.btnImportUsers.Content = image;
			this.btnImportUsers.Click += this.btnImport_Click;
			this.btnImportUsers.Style = (base.FindResource("defaultButtonStyle") as Style);
			this.Buttons.Add(this.btnImportUsers);
			this.btnLoginToGo = new ButtonUSB(usbLetter, "/img/usbuser.png", "/img/usbuser_nousb.png", new RoutedEventHandler(this.btnLoginToGo_Click));
			this.Buttons.Add(this.btnLoginToGo);
			bool flag2 = flag;
			if (flag2)
			{
				bool isConnected = Rfid.Instance.IsConnected;
				if (isConnected)
				{
					this.btnLoginRfIdAdd.Content = ImageConverter.ToImage("/img/rfid_add.png");
				}
				else
				{
					this.btnLoginRfIdAdd.Content = ImageConverter.ToImage("/img/rfid_add_disabled.png");
				}
				this.btnLoginRfIdAdd.Click += this.btnCreateRfid_Click;
				this.btnLoginRfIdAdd.Style = (base.FindResource("defaultButtonStyle") as Style);
				this.btnLoginRfIdAdd.IsEnabled = Rfid.Instance.IsConnected;
				this.Buttons.Add(this.btnLoginRfIdAdd);
				bool isConnected2 = Rfid.Instance.IsConnected;
				if (isConnected2)
				{
					this.btnLoginRfIdDel.Content = ImageConverter.ToImage("/img/rfid_remove.png");
				}
				else
				{
					this.btnLoginRfIdDel.Content = ImageConverter.ToImage("/img/rfid_remove_disabled.png");
				}
				this.btnLoginRfIdDel.Click += this.btnRemoveRfid_Click;
				this.btnLoginRfIdDel.Style = (base.FindResource("defaultButtonStyle") as Style);
				this.btnLoginRfIdDel.IsEnabled = Rfid.Instance.IsConnected;
				this.Buttons.Add(this.btnLoginRfIdDel);
				bool isConnected3 = Rfid.Instance.IsConnected;
				if (isConnected3)
				{
					this.btnLoginRfIdInf.Content = ImageConverter.ToImage("/img/rfid_info.png");
				}
				else
				{
					this.btnLoginRfIdInf.Content = ImageConverter.ToImage("/img/rfid_info_disabled.png");
				}
				this.btnLoginRfIdInf.Click += this.btnInfoRfid_Click;
				this.btnLoginRfIdInf.Style = (base.FindResource("defaultButtonStyle") as Style);
				this.btnLoginRfIdInf.IsEnabled = Rfid.Instance.IsConnected;
				this.Buttons.Add(this.btnLoginRfIdInf);
			}
			Rfid.Instance.PropertyChanged += this.rfIdConnectionChanged;
		}

		// Token: 0x06002E0E RID: 11790 RVA: 0x0012D508 File Offset: 0x0012B708
		private void rfIdConnectionChanged(object sender, PropertyChangedEventArgs e)
		{
			bool flag = e.PropertyName == "IsConnected";
			if (flag)
			{
				bool isConnected = Rfid.Instance.IsConnected;
				if (isConnected)
				{
					this.btnLoginRfIdAdd.Content = ImageConverter.ToImage("/img/rfid_add.png");
					this.btnLoginRfIdDel.Content = ImageConverter.ToImage("/img/rfid_remove.png");
					this.btnLoginRfIdInf.Content = ImageConverter.ToImage("/img/rfid_info.png");
				}
				else
				{
					this.btnLoginRfIdAdd.Content = ImageConverter.ToImage("/img/rfid_add_disabled.png");
					this.btnLoginRfIdDel.Content = ImageConverter.ToImage("/img/rfid_remove_disabled.png");
					this.btnLoginRfIdInf.Content = ImageConverter.ToImage("/img/rfid_info_disabled.png");
				}
			}
		}

		// Token: 0x06002E0F RID: 11791 RVA: 0x0012D5C7 File Offset: 0x0012B7C7
		private void btnChange_Click(object sender, RoutedEventArgs e)
		{
			this.usermanagementViewModel.ChangeUser();
		}

		// Token: 0x06002E10 RID: 11792 RVA: 0x0012D5D8 File Offset: 0x0012B7D8
		private void listBoxItem_DoubleClick(object sender, RoutedEventArgs e)
		{
			bool flag = sender is ListBoxItem && ((ListBoxItem)sender).Content is User;
			if (flag)
			{
				User user = (User)((ListBoxItem)sender).Content;
				bool flag2 = user == this.usermanagementViewModel.SelectedUser;
				if (flag2)
				{
					this.usermanagementViewModel.ChangeUser();
				}
			}
		}

		// Token: 0x06002E11 RID: 11793 RVA: 0x0012D63C File Offset: 0x0012B83C
		private void btnNew_Click(object sender, RoutedEventArgs e)
		{
			int num = 0;
			this.dbConnection.GetMaxUsers(ref num);
			bool flag = num <= 0;
			if (flag)
			{
				num = Definition.maxDefaultNumUsers;
			}
			bool flag2 = this.usermanagementViewModel.UserList.Count < num;
			if (flag2)
			{
				ChangeUserData changeUserData = new ChangeUserData(this.dbConnection, ChangeUserData.ChangeUserDataType.newUser, this.usermanagementViewModel);
				Subwindow.Show(changeUserData, SubwindowButtons.BackAndHome, changeUserData.Buttons);
			}
			else
			{
				Dialog.Show("Error", "Maximum number of users: " + num.ToString(), DialogStyle.Error, DialogButtons.OK);
			}
		}

		// Token: 0x06002E12 RID: 11794 RVA: 0x0012D6C7 File Offset: 0x0012B8C7
		private void btnDelete_Click(object sender, RoutedEventArgs e)
		{
			this.usermanagementViewModel.DeleteUser();
		}

		// Token: 0x06002E13 RID: 11795 RVA: 0x0012D6D8 File Offset: 0x0012B8D8
		private void btnRename_Click(object sender, RoutedEventArgs e)
		{
			bool flag = this.usermanagementViewModel.SelectedUser != null;
			if (flag)
			{
				bool deletable = this.usermanagementViewModel.SelectedUser.Deletable;
				if (deletable)
				{
					ChangeUserData changeUserData = new ChangeUserData(this.dbConnection, ChangeUserData.ChangeUserDataType.changeUserdata, this.usermanagementViewModel);
					Subwindow.Show(changeUserData, SubwindowButtons.BackAndHome, changeUserData.Buttons);
				}
				else
				{
					Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("msgUserChangeForbidden"), DialogStyle.Warning);
				}
			}
		}

		// Token: 0x06002E14 RID: 11796 RVA: 0x0012D74E File Offset: 0x0012B94E
		private void btnLoginToGo_Click(object sender, RoutedEventArgs e)
		{
			this.usermanagementViewModel.CreateLTGUser();
		}

		// Token: 0x06002E15 RID: 11797 RVA: 0x0012D760 File Offset: 0x0012B960
		private void UsermanagementControl_Unloaded(object sender, RoutedEventArgs e)
		{
			Rfid.Instance.PropertyChanged -= this.rfIdConnectionChanged;
			this.usermanagementViewModel.UserChanged -= this.usermanagementViewModel_UserChanged;
			this.accessManager.DeregisterControl(this.btnChange);
			this.accessManager.DeregisterControl(this.btnNew);
			this.accessManager.DeregisterControl(this.btnDelete);
			this.accessManager.DeregisterControl(this.btnRename);
			this.accessManager.DeregisterControl(this.btnImportUsers);
			this.accessManager.DeregisterControl(this.btnLoginToGo);
			this.accessManager.DeregisterControl(this.btnLoginRfIdAdd);
			this.accessManager.DeregisterControl(this.btnLoginRfIdDel);
			this.accessManager.DeregisterControl(this.btnLoginRfIdInf);
			this.setFooterMainWindow.UnSetFooterDeactivation();
			this.usermanagementViewModel.CleanUp();
		}

		// Token: 0x06002E16 RID: 11798 RVA: 0x0012D857 File Offset: 0x0012BA57
		private void btnImport_Click(object sender, RoutedEventArgs e)
		{
			this.usermanagementViewModel.ShowImportUsersWindow();
		}

		// Token: 0x06002E17 RID: 11799 RVA: 0x0012D868 File Offset: 0x0012BA68
		private void btnCreateRfid_Click(object sender, RoutedEventArgs e)
		{
			bool isConnected = Rfid.Instance.IsConnected;
			if (isConnected)
			{
				this.usermanagementViewModel.CreateRfidUser();
			}
		}

		// Token: 0x06002E18 RID: 11800 RVA: 0x0012D890 File Offset: 0x0012BA90
		private void btnRemoveRfid_Click(object sender, RoutedEventArgs e)
		{
			bool isConnected = Rfid.Instance.IsConnected;
			if (isConnected)
			{
				this.usermanagementViewModel.RemoveRfidUser();
			}
		}

		// Token: 0x06002E19 RID: 11801 RVA: 0x0012D8B8 File Offset: 0x0012BAB8
		private void btnInfoRfid_Click(object sender, RoutedEventArgs e)
		{
			bool isConnected = Rfid.Instance.IsConnected;
			if (isConnected)
			{
				this.usermanagementViewModel.CheckRfidUser();
			}
		}

		// Token: 0x06002E1A RID: 11802 RVA: 0x0012D8E0 File Offset: 0x0012BAE0
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/usermanagement/usermanagementcontrol.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x06002E1B RID: 11803 RVA: 0x0012D918 File Offset: 0x0012BB18
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				((UsermanagementControl)target).Unloaded += this.UsermanagementControl_Unloaded;
				return;
			case 2:
				this.tbCurrentUser = (TextBlock)target;
				return;
			case 3:
				this.lstUsers = (ListBox)target;
				return;
			case 5:
				this.Filter = (Button)target;
				return;
			}
			this._contentLoaded = true;
		}

		// Token: 0x06002E1C RID: 11804 RVA: 0x0012D994 File Offset: 0x0012BB94
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IStyleConnector.Connect(int connectionId, object target)
		{
			if (connectionId == 4)
			{
				EventSetter eventSetter = new EventSetter();
				eventSetter.Event = Control.MouseDoubleClickEvent;
				eventSetter.Handler = new MouseButtonEventHandler(this.listBoxItem_DoubleClick);
				((Style)target).Setters.Add(eventSetter);
			}
		}

		// Token: 0x04001CC8 RID: 7368
		private BuiltInPriviledges rightChange = BuiltInPriviledges.NoUser;

		// Token: 0x04001CC9 RID: 7369
		private BuiltInPriviledges rightNew = BuiltInPriviledges.Admin;

		// Token: 0x04001CCA RID: 7370
		private BuiltInPriviledges rightDelete = BuiltInPriviledges.Admin;

		// Token: 0x04001CCB RID: 7371
		private BuiltInPriviledges rightRename = BuiltInPriviledges.Admin;

		// Token: 0x04001CCC RID: 7372
		private BuiltInPriviledges rightLoginToGo = BuiltInPriviledges.Operator3;

		// Token: 0x04001CCD RID: 7373
		private BuiltInPriviledges rightImportUsers = BuiltInPriviledges.Admin;

		// Token: 0x04001CCE RID: 7374
		private BuiltInPriviledges rightLearnRfId = BuiltInPriviledges.Admin;

		// Token: 0x04001CCF RID: 7375
		private BuiltInPriviledges rightDeleteRfId = BuiltInPriviledges.Admin;

		// Token: 0x04001CD0 RID: 7376
		private BuiltInPriviledges rightInfoRfId = BuiltInPriviledges.Admin;

		// Token: 0x04001CD1 RID: 7377
		private DBConnection dbConnection;

		// Token: 0x04001CD2 RID: 7378
		private UsermanagementViewModel usermanagementViewModel;

		// Token: 0x04001CD3 RID: 7379
		private Button btnRename;

		// Token: 0x04001CD4 RID: 7380
		private Button btnChange;

		// Token: 0x04001CD5 RID: 7381
		private Button btnNew;

		// Token: 0x04001CD6 RID: 7382
		private Button btnDelete;

		// Token: 0x04001CD7 RID: 7383
		private ButtonUSB btnLoginToGo;

		// Token: 0x04001CD8 RID: 7384
		private Button btnImportUsers;

		// Token: 0x04001CD9 RID: 7385
		private Button btnLoginRfIdAdd = new Button();

		// Token: 0x04001CDA RID: 7386
		private Button btnLoginRfIdDel = new Button();

		// Token: 0x04001CDB RID: 7387
		private Button btnLoginRfIdInf = new Button();

		// Token: 0x04001CDC RID: 7388
		private AccessRightsManager accessManager;

		// Token: 0x04001CDD RID: 7389
		private SetFooterMainWindow setFooterMainWindow;

		// Token: 0x04001CDF RID: 7391
		internal TextBlock tbCurrentUser;

		// Token: 0x04001CE0 RID: 7392
		internal ListBox lstUsers;

		// Token: 0x04001CE1 RID: 7393
		internal Button Filter;

		// Token: 0x04001CE2 RID: 7394
		private bool _contentLoaded;
	}
}
