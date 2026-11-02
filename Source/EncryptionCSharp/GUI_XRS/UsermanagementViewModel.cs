using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Threading;
using Common;
using DatabaseConnection;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x02000229 RID: 553
	public class UsermanagementViewModel : INotifyPropertyChanged
	{
		// Token: 0x140000DB RID: 219
		// (add) Token: 0x06002968 RID: 10600 RVA: 0x001125A4 File Offset: 0x001107A4
		// (remove) Token: 0x06002969 RID: 10601 RVA: 0x001125DC File Offset: 0x001107DC
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action UserChanged;

		// Token: 0x17000B49 RID: 2889
		// (get) Token: 0x0600296A RID: 10602 RVA: 0x00112611 File Offset: 0x00110811
		// (set) Token: 0x0600296B RID: 10603 RVA: 0x00112619 File Offset: 0x00110819
		public BoundLocalizedText UserID { get; set; }

		// Token: 0x17000B4A RID: 2890
		// (get) Token: 0x0600296C RID: 10604 RVA: 0x00112622 File Offset: 0x00110822
		// (set) Token: 0x0600296D RID: 10605 RVA: 0x0011262A File Offset: 0x0011082A
		public BoundLocalizedText UserName { get; set; }

		// Token: 0x17000B4B RID: 2891
		// (get) Token: 0x0600296E RID: 10606 RVA: 0x00112633 File Offset: 0x00110833
		// (set) Token: 0x0600296F RID: 10607 RVA: 0x0011263B File Offset: 0x0011083B
		public BoundLocalizedText UserGroup { get; set; }

		// Token: 0x17000B4C RID: 2892
		// (get) Token: 0x06002970 RID: 10608 RVA: 0x00112644 File Offset: 0x00110844
		// (set) Token: 0x06002971 RID: 10609 RVA: 0x0011264C File Offset: 0x0011084C
		public BoundLocalizedText CommitErrors { get; set; }

		// Token: 0x140000DC RID: 220
		// (add) Token: 0x06002972 RID: 10610 RVA: 0x00112658 File Offset: 0x00110858
		// (remove) Token: 0x06002973 RID: 10611 RVA: 0x00112690 File Offset: 0x00110890
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x17000B4D RID: 2893
		// (get) Token: 0x06002974 RID: 10612 RVA: 0x001126C8 File Offset: 0x001108C8
		// (set) Token: 0x06002975 RID: 10613 RVA: 0x001126E0 File Offset: 0x001108E0
		public ObservableCollection<User> UserList
		{
			get
			{
				return this._userList;
			}
			set
			{
				bool flag = this._userList != value;
				if (flag)
				{
					this._userList = value;
					this.OnPropertyChanged("UserList");
				}
			}
		}

		// Token: 0x17000B4E RID: 2894
		// (get) Token: 0x06002976 RID: 10614 RVA: 0x00112714 File Offset: 0x00110914
		// (set) Token: 0x06002977 RID: 10615 RVA: 0x0011272C File Offset: 0x0011092C
		public User SelectedUser
		{
			get
			{
				return this.selectedUser;
			}
			set
			{
				this.selectedUser = value;
				this.OnPropertyChanged("SelectedUser");
			}
		}

		// Token: 0x17000B4F RID: 2895
		// (get) Token: 0x06002978 RID: 10616 RVA: 0x00112744 File Offset: 0x00110944
		// (set) Token: 0x06002979 RID: 10617 RVA: 0x0001092C File Offset: 0x0000EB2C
		public User CurrentUser
		{
			get
			{
				return (this.accessManager != null) ? this.accessManager.CurrentUser : null;
			}
			private set
			{
			}
		}

		// Token: 0x17000B50 RID: 2896
		// (get) Token: 0x0600297A RID: 10618 RVA: 0x0011276C File Offset: 0x0011096C
		// (set) Token: 0x0600297B RID: 10619 RVA: 0x00112774 File Offset: 0x00110974
		public TextSearch UserSearch { get; set; }

		// Token: 0x17000B51 RID: 2897
		// (get) Token: 0x0600297C RID: 10620 RVA: 0x00112780 File Offset: 0x00110980
		// (set) Token: 0x0600297D RID: 10621 RVA: 0x00112798 File Offset: 0x00110998
		public bool IsFilterActive
		{
			get
			{
				return this._isFilterActive;
			}
			set
			{
				bool flag = this._isFilterActive != value;
				if (flag)
				{
					this._isFilterActive = value;
					this.OnPropertyChanged("IsFilterActive");
				}
			}
		}

		// Token: 0x17000B52 RID: 2898
		// (get) Token: 0x0600297E RID: 10622 RVA: 0x001127CB File Offset: 0x001109CB
		// (set) Token: 0x0600297F RID: 10623 RVA: 0x001127D3 File Offset: 0x001109D3
		public RelayCommand ResetFilterCommand { get; set; }

		// Token: 0x06002980 RID: 10624 RVA: 0x001127DC File Offset: 0x001109DC
		public UsermanagementViewModel(DBConnection dbConnection, AccessRightsManager accessManager, Restore restore, LoginToGo loginToGo, bool multipleRfid = false)
		{
			this.accessManager = accessManager;
			this.restore = restore;
			this.IsFilterActive = false;
			this.loginToGo = loginToGo;
			accessManager.SetProperties();
			this.UserID = new BoundLocalizedText("txtUserID");
			this.UserName = new BoundLocalizedText("txtUserName");
			this.UserGroup = new BoundLocalizedText("txtUserGroup");
			this.CommitErrors = new BoundLocalizedText("txtCommitErrors");
			this.UserList = new ObservableCollection<User>();
			this.view = (ListCollectionView)CollectionViewSource.GetDefaultView(this.UserList);
			this.dbConnection = dbConnection;
			this.UserSearch = new UserSearch(this.view);
			Rfid.Instance.RfidDetected += this.LoginRfidUser;
			this.UserSearch.PropertyChanged += this.userSearchPropertyChanged;
			this.UserList.CollectionChanged += this.userListCollectionChanged;
			this.ResetFilterCommand = new RelayCommand(new Action<object>(this.ResetFilter));
			bool flag = !multipleRfid;
			if (flag)
			{
				Rfid.Instance.CleanUid();
			}
			this.CreateUserList(null);
			this.SelectCurrentUser();
		}

		// Token: 0x06002981 RID: 10625 RVA: 0x00112918 File Offset: 0x00110B18
		public void ResetFilter(object obj)
		{
			Rfid.Instance.CleanUid();
			this.UserSearch.SearchText = "";
			List<User> userList = new List<User>();
			this.dbConnection.GetUserList(userList);
			this.UserList.Clear();
			this.CreateUserList(null);
		}

		// Token: 0x06002982 RID: 10626 RVA: 0x0011296C File Offset: 0x00110B6C
		private void userSearchPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			bool flag = e.PropertyName == "SearchText";
			if (flag)
			{
				this.IsFilterActive = (!string.IsNullOrEmpty(this.UserSearch.SearchText) && this.UserSearch.SearchText != this.UserSearch.searchTextDefault);
				bool flag2 = Rfid.Instance.HasRfid && this.IsFilterActive;
				if (flag2)
				{
					this.UserSearch.PropertyChanged -= this.userSearchPropertyChanged;
					Rfid.Instance.CleanUid();
					string searchText = this.UserSearch.SearchText;
					this.ResetFilterCommand.Execute(null);
					this.IsFilterActive = true;
					this.UserSearch.SearchText = searchText;
					this.UserSearch.PropertyChanged += this.userSearchPropertyChanged;
				}
			}
		}

		// Token: 0x06002983 RID: 10627 RVA: 0x00112A50 File Offset: 0x00110C50
		private void userListCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			List<User> list = new List<User>();
			this.dbConnection.GetUserList(list);
			this.IsFilterActive = (this.UserList.Count != list.Count);
			this.OnPropertyChanged("UserList");
		}

		// Token: 0x06002984 RID: 10628 RVA: 0x00112A9C File Offset: 0x00110C9C
		public UsermanagementViewModel(Restore restore, string backupPath)
		{
			this.restore = restore;
			this.UserID = new BoundLocalizedText("txtUserID");
			this.UserName = new BoundLocalizedText("txtUserName");
			this.UserGroup = new BoundLocalizedText("txtUserGroup");
			this.CommitErrors = new BoundLocalizedText("txtCommitErrors");
			this.UserList = new ObservableCollection<User>();
			this.view = (ListCollectionView)CollectionViewSource.GetDefaultView(this.UserList);
			restore.UserInfoExtracted += this.restore_UserInfoExtracted;
			this.CreateUserList(backupPath);
		}

		// Token: 0x06002985 RID: 10629 RVA: 0x00112B3C File Offset: 0x00110D3C
		public void CleanUp()
		{
			MainWindow.localizationManager.Deregister(this.UserID);
			MainWindow.localizationManager.Deregister(this.UserName);
			MainWindow.localizationManager.Deregister(this.UserGroup);
			bool flag = this.UserSearch != null;
			if (flag)
			{
				this.UserSearch.PropertyChanged -= this.userSearchPropertyChanged;
			}
			this.UserList.CollectionChanged -= this.userListCollectionChanged;
			this.restore.UserInfoExtracted -= this.restore_UserInfoExtracted;
			Rfid.Instance.RfidDetected -= this.LoginRfidUser;
			this.UserList.Clear();
		}

		// Token: 0x06002986 RID: 10630 RVA: 0x00112BF8 File Offset: 0x00110DF8
		public void DeleteUser()
		{
			bool flag = this.SelectedUser != null;
			if (flag)
			{
				bool flag2 = this.accessManager.CurrentUser.ID != this.SelectedUser.ID;
				if (flag2)
				{
					bool deletable = this.SelectedUser.Deletable;
					if (deletable)
					{
						Dialog.DialogResults dialogResults = Dialog.Show(LocalizationManager.Localize("txtWarning"), string.Concat(new string[]
						{
							LocalizationManager.Localize("msgUser"),
							" \"",
							this.SelectedUser.Username,
							"\" ",
							LocalizationManager.Localize("msgGettingErased")
						}), DialogStyle.Warning, DialogButtons.OKCancel);
						bool flag3 = dialogResults == Dialog.DialogResults.Ok;
						if (flag3)
						{
							bool flag4 = this.dbConnection.DeleteUser(this.SelectedUser);
							bool flag5 = flag4;
							if (flag5)
							{
								LogbookManager.AddEntry(SSTEntryCode.UserDeleted, 0, this.SelectedUser.ID.ToString(), this.SelectedUser.Username);
								this.UserList.Remove(this.SelectedUser);
							}
							else
							{
								Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("msgDeleteUserError"), DialogStyle.Error);
							}
						}
					}
					else
					{
						Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("msgUserDeleteForbidden"), DialogStyle.Warning, DialogButtons.OK);
					}
				}
				else
				{
					Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("msgCurrentUserDeleteForbidden"), DialogStyle.Warning, DialogButtons.OK);
				}
			}
		}

		// Token: 0x06002987 RID: 10631 RVA: 0x00112D6C File Offset: 0x00110F6C
		public void ChangeUser()
		{
			bool flag = this.SelectedUser != null;
			if (flag)
			{
				bool flag2 = this.SelectedUser.ID != this.accessManager.CurrentUser.ID;
				if (flag2)
				{
					bool flag3 = this.SelectedUser.Rights > BuiltInPriviledges.NoUser;
					if (flag3)
					{
						List<string> list = new List<string>();
						list.Add(this.SelectedUser.PasswordAsMD5);
						bool flag4 = this.SelectedUser.Rights == BuiltInPriviledges.Service;
						if (flag4)
						{
							list.Add(this.GetServiceDayPassword().ToMD5());
						}
						this.keyboard = new Keyboard(LocalizationManager.Localize("msgEnterPassword"), list.ToArray(), true);
						this.keyboard.TextInputKeyboard += this.keyboard_TextInputKeyboard;
						this.keyboard.Show();
					}
					else
					{
						bool flag5 = Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("msgSignOut"), DialogStyle.Warning, DialogButtons.OKCancel) == Dialog.DialogResults.Ok;
						if (flag5)
						{
							this.keyboard_TextInputKeyboard(null, null);
						}
					}
				}
				else
				{
					Dialog.Show(LocalizationManager.Localize("txtWarning"), string.Concat(new string[]
					{
						LocalizationManager.Localize("msgUser"),
						" \"",
						this.SelectedUser.Username,
						"\" ",
						LocalizationManager.Localize("msgAlreadyLoggedIn")
					}), DialogStyle.Warning);
				}
			}
		}

		// Token: 0x06002988 RID: 10632 RVA: 0x00112ED8 File Offset: 0x001110D8
		private void keyboard_TextInputKeyboard(object sender, KeyboardInputEventArgs e)
		{
			bool flag = this.keyboard != null;
			if (flag)
			{
				this.keyboard.TextInputKeyboard -= this.keyboard_TextInputKeyboard;
			}
			this.SetCurrentUserPropertiesToSelectedUserProperties(true);
			this.accessManager.SetProperties();
			this.OnUserChanged();
		}

		// Token: 0x06002989 RID: 10633 RVA: 0x00112F28 File Offset: 0x00111128
		public void SetCurrentUserPropertiesToSelectedUserProperties(bool userChange)
		{
			string username = this.accessManager.CurrentUser.Username;
			if (userChange)
			{
				bool flag = this.accessManager.CurrentUser.Rights == BuiltInPriviledges.NoUser;
				if (flag)
				{
					LogbookManager.AddEntry(SSTEntryCode.UserLoggedIn, 0, this.selectedUser.Username);
				}
				else
				{
					bool flag2 = this.selectedUser.Rights == BuiltInPriviledges.NoUser;
					if (flag2)
					{
						LogbookManager.AddEntry(SSTEntryCode.UserLoggedOut, 0, this.accessManager.CurrentUser.Username, "", "", "");
					}
					else
					{
						LogbookManager.AddEntry(SSTEntryCode.UserChanged, 0, this.accessManager.CurrentUser.Username, this.selectedUser.Username, "", "");
					}
				}
			}
			this.accessManager.CurrentUser.Deletable = this.selectedUser.Deletable;
			this.accessManager.CurrentUser.ID = this.selectedUser.ID;
			this.accessManager.CurrentUser.PasswordAsMD5 = this.selectedUser.PasswordAsMD5;
			this.accessManager.CurrentUser.Rights = this.selectedUser.Rights;
			this.accessManager.CurrentUser.Username = this.selectedUser.Username;
			this.accessManager.CurrentUser.CommitErrors = this.selectedUser.CommitErrors;
		}

		// Token: 0x0600298A RID: 10634 RVA: 0x0011309C File Offset: 0x0011129C
		private void CreateUserList(string backupPath)
		{
			List<User> list = new List<User>();
			bool flag = backupPath == null;
			if (flag)
			{
				bool flag2 = this.dbConnection.Connection != null;
				if (flag2)
				{
					bool flag3 = false;
					this.dbConnection.GetRFIDLoginAvailable(ref flag3);
					bool flag4 = !flag3 || string.IsNullOrEmpty(Rfid.Instance.Uid);
					if (flag4)
					{
						this.dbConnection.GetUserList(list);
					}
					else
					{
						this.dbConnection.GetUserList(list, Rfid.Instance.Uid);
					}
					foreach (User user in list)
					{
						bool flag5 = !flag3;
						if (flag5)
						{
							user.Rfid = "";
						}
						else
						{
							bool flag6 = !string.IsNullOrEmpty(user.Rfid);
							if (flag6)
							{
								bool flag7 = !Rfid.Instance.IsConnected;
								if (flag7)
								{
									user.Rfid = "Disabled";
								}
							}
						}
						bool flag8 = user.ID != -1 && user.ID != -2 && user.ID != -3;
						if (flag8)
						{
							this.UserList.Add(user);
						}
					}
				}
			}
			else
			{
				Progress.ShowStaticProgress(LocalizationManager.Localize("txtProcessing"), false);
				this.restore.GetUsersFromBackup(backupPath, this.dbConnection);
			}
		}

		// Token: 0x0600298B RID: 10635 RVA: 0x00113228 File Offset: 0x00111428
		private void SelectCurrentUser()
		{
			bool flag = this.UserList != null;
			if (flag)
			{
				foreach (User user in this.UserList)
				{
					bool flag2 = user.ID == this.accessManager.CurrentUser.ID;
					if (flag2)
					{
						this.SelectedUser = user;
						break;
					}
				}
			}
		}

		// Token: 0x0600298C RID: 10636 RVA: 0x001132AC File Offset: 0x001114AC
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x0600298D RID: 10637 RVA: 0x001132DC File Offset: 0x001114DC
		protected virtual void OnUserChanged()
		{
			Action userChanged = this.UserChanged;
			bool flag = userChanged != null;
			if (flag)
			{
				userChanged();
			}
		}

		// Token: 0x0600298E RID: 10638 RVA: 0x00113300 File Offset: 0x00111500
		private string GetServiceDayPassword()
		{
			DateTime now = DateTime.Now;
			StringBuilder stringBuilder = new StringBuilder();
			string text = now.ToString("d", new CultureInfo("de-DE"));
			bool flag = text.Length > 8;
			if (flag)
			{
				text = text.Remove(6, 2);
			}
			int num = 2 * int.Parse(text.Substring(0, 2)) + int.Parse(text.Substring(4, 1)) * 10 + int.Parse(text.Substring(6, 1)) + 8 * int.Parse(text.Substring(7, 1));
			bool flag2 = num < 16;
			if (flag2)
			{
				stringBuilder.Append("0");
			}
			stringBuilder.Append(string.Format("{0:X}", num));
			num = int.Parse(text.Substring(0, 2)) + 2 * (int.Parse(text.Substring(4, 1)) * 10 + int.Parse(text.Substring(6, 1))) + 2 * int.Parse(text.Substring(7, 1));
			bool flag3 = num < 16;
			if (flag3)
			{
				stringBuilder.Append("0");
			}
			stringBuilder.Append(string.Format("{0:X}", num));
			num = 2 * (int.Parse(text.Substring(1, 1)) * 10 + int.Parse(text.Substring(3, 1))) + int.Parse(text.Substring(0, 1)) * 10 + int.Parse(text.Substring(4, 1));
			bool flag4 = num < 16;
			if (flag4)
			{
				stringBuilder.Append("0");
			}
			stringBuilder.Append(string.Format("{0:X}", num));
			return stringBuilder.ToString();
		}

		// Token: 0x0600298F RID: 10639 RVA: 0x001134A0 File Offset: 0x001116A0
		public void CreateLTGUser()
		{
			bool flag = this.accessManager.CurrentUser.ID == -1;
			if (flag)
			{
				bool flag2 = Dialog.Show(LocalizationManager.Localize("txtInfo"), LocalizationManager.Localize("txtNewValidationUser"), DialogStyle.Info, DialogButtons.OKCancel, false) == Dialog.DialogResults.Ok;
				if (flag2)
				{
					bool flag3 = Dialog.Show(LocalizationManager.Localize("txtInfo"), LocalizationManager.Localize("txtNewStick"), DialogStyle.Info, DialogButtons.OK, false) == Dialog.DialogResults.Ok;
					if (flag3)
					{
						this.loginToGo.startTimer(this.dbConnection);
					}
				}
			}
			else
			{
				string text = "";
				bool flag4 = true;
				this.dbConnection.GetUsbDriveLetter(ref text);
				text += "\\";
				bool flag5 = this.loginToGo.LoginToGoUserExists(text);
				if (flag5)
				{
					flag4 = (Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtOverwriteLoginToGoUser"), DialogStyle.Warning, DialogButtons.OKCancel) == Dialog.DialogResults.Ok);
				}
				bool flag6 = flag4;
				if (flag6)
				{
					bool flag7 = this.loginToGo.CreateLTGUser(this.accessManager.CurrentUser, text);
					if (flag7)
					{
						Dialog.Show(LocalizationManager.Localize("txtSuccess"), LocalizationManager.Localize("txtSuccess"), DialogStyle.Ok, DialogButtons.None, true);
					}
					else
					{
						Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtError"), DialogStyle.Error);
					}
				}
			}
		}

		// Token: 0x06002990 RID: 10640 RVA: 0x001135E4 File Offset: 0x001117E4
		public void ShowImportUsersWindow()
		{
			CtrlRestoreList ctrlRestoreList = new CtrlRestoreList(this.dbConnection, this.restore, this.accessManager);
			Subwindow.Show(ctrlRestoreList, SubwindowButtons.BackAndHome, ctrlRestoreList.Buttons);
		}

		// Token: 0x06002991 RID: 10641 RVA: 0x00113618 File Offset: 0x00111818
		private void restore_UserInfoExtracted(List<User> users)
		{
			Progress.CloseStaticProgress(false);
			bool flag = users != null;
			if (flag)
			{
				Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(delegate()
				{
					foreach (User user in users)
					{
						bool flag2 = user.ID != -1 && user.ID != -2;
						if (flag2)
						{
							this.UserList.Add(user);
						}
					}
				}));
			}
		}

		// Token: 0x06002992 RID: 10642 RVA: 0x00113670 File Offset: 0x00111870
		public void Wait4RfId(MsgBox box, CancellationToken token)
		{
			do
			{
				bool isCancellationRequested = token.IsCancellationRequested;
				if (isCancellationRequested)
				{
					break;
				}
			}
			while (!Rfid.Instance.HasRfid);
			bool flag = !token.IsCancellationRequested;
			if (flag)
			{
				Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
				{
					box.Close();
				}));
			}
		}

		// Token: 0x06002993 RID: 10643 RVA: 0x001136D8 File Offset: 0x001118D8
		public void CreateRfidUser()
		{
			Rfid.Instance.CleanUid();
			bool flag = this.SelectedUser != null;
			if (flag)
			{
				bool flag2 = this.SelectedUser.Rights == BuiltInPriviledges.Service || this.SelectedUser.Rights == BuiltInPriviledges.NoUser;
				if (flag2)
				{
					Dialog.DialogResults dialogResults = Dialog.Show(LocalizationManager.Localize("create_rfid_login"), LocalizationManager.Localize("user_not_allowed_for_rfid_login"), DialogStyle.Error, DialogButtons.OK);
				}
				else
				{
					bool flag3 = string.IsNullOrEmpty(this.SelectedUser.Rfid);
					Dialog.DialogResults dialogResults;
					if (flag3)
					{
						dialogResults = Dialog.Show(LocalizationManager.Localize("create_rfid_login"), LocalizationManager.Localize("create_rfid_login_for_selected_user"), DialogStyle.Info, DialogButtons.OKCancel);
					}
					else
					{
						dialogResults = Dialog.Show(LocalizationManager.Localize("create_rfid_login"), LocalizationManager.Localize("overwrite_rfid_login_for_selected_user"), DialogStyle.Info, DialogButtons.OKCancel);
					}
					bool flag4 = dialogResults == Dialog.DialogResults.Ok;
					if (flag4)
					{
						MsgBox box;
						do
						{
							CancellationTokenSource tokenSource = new CancellationTokenSource();
							box = new MsgBox(LocalizationManager.Localize("create_rfid_login"), LocalizationManager.Localize("Hold_RFID_token_to_reader"));
							Task task = Task.Run(delegate()
							{
								this.Wait4RfId(box, tokenSource.Token);
							}, tokenSource.Token);
							box.ShowDialog();
							tokenSource.Cancel();
							bool hasRfid = Rfid.Instance.HasRfid;
							if (hasRfid)
							{
								List<User> list = new List<User>();
								this.dbConnection.GetUserList(list, Rfid.Instance.Uid);
								bool flag5 = list.Count != 0;
								if (flag5)
								{
									Rfid.Instance.StopTimer();
									dialogResults = Dialog.Show(LocalizationManager.Localize("create_rfid_login"), LocalizationManager.Localize("rfid_already_mapped_to_a_user_map_to_a_second_user"), DialogStyle.Warning, DialogButtons.OKCancel);
									bool flag6 = dialogResults == Dialog.DialogResults.Ok && Rfid.Instance.HasRfid;
									if (flag6)
									{
										break;
									}
									Rfid.Instance.CleanUid();
								}
							}
							bool flag7 = box.result == Dialog.DialogResults.Cancel;
							if (flag7)
							{
								break;
							}
						}
						while (!Rfid.Instance.HasRfid);
						bool flag8 = box.result != Dialog.DialogResults.Cancel && dialogResults == Dialog.DialogResults.Ok && Rfid.Instance.HasRfid;
						if (flag8)
						{
							this.SelectedUser.Rfid = Rfid.Instance.Uid;
							bool flag9 = !this.dbConnection.UpdateUser(this.SelectedUser, this.SelectedUser.ID);
							if (flag9)
							{
								Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("msgDeleteUserError"), DialogStyle.Error);
							}
						}
					}
				}
			}
			else
			{
				Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("Please_select_a_user_for_rfid_authentication"), DialogStyle.Error);
			}
		}

		// Token: 0x06002994 RID: 10644 RVA: 0x001139A0 File Offset: 0x00111BA0
		public void RemoveRfidUser()
		{
			bool flag = this.SelectedUser != null;
			if (flag)
			{
				Dialog.DialogResults dialogResults = Dialog.Show(LocalizationManager.Localize("delete_rfid_login"), LocalizationManager.Localize("delete_rfid_login_for_the_selected_user"), DialogStyle.Info, DialogButtons.OKCancel);
				bool flag2 = dialogResults == Dialog.DialogResults.Ok;
				if (flag2)
				{
					this.SelectedUser.Rfid = "";
					bool flag3 = !this.dbConnection.UpdateUser(this.SelectedUser, this.SelectedUser.ID);
					if (flag3)
					{
						Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("msgDeleteUserError"), DialogStyle.Error);
					}
				}
			}
			else
			{
				Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("Please_select_a_user_for_rfid_authentication"), DialogStyle.Error);
			}
		}

		// Token: 0x06002995 RID: 10645 RVA: 0x00113A54 File Offset: 0x00111C54
		public void LoginRfidUser(string rfid)
		{
			List<User> list = new List<User>();
			bool userList = this.dbConnection.GetUserList(list, rfid);
			if (userList)
			{
				bool flag = list.Count >= 1;
				if (flag)
				{
					bool flag2 = this.SelectedUser != null;
					if (flag2)
					{
						foreach (User user in list)
						{
							bool flag3 = user.Rfid == this.SelectedUser.Rfid && this.accessManager.CurrentUser.ID != this.SelectedUser.ID;
							if (flag3)
							{
								Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
								{
									this.keyboard_TextInputKeyboard(null, null);
								}));
							}
						}
					}
					else
					{
						Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
						{
							Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("Please_select_a_user_for_rfid_authentication"), DialogStyle.Error);
						}));
					}
				}
			}
		}

		// Token: 0x06002996 RID: 10646 RVA: 0x00113B78 File Offset: 0x00111D78
		public void CheckRfidUser()
		{
			Rfid.Instance.RfidDetected -= this.LoginRfidUser;
			Rfid.Instance.CleanUid();
			this.ResetFilterCommand.Execute(null);
			MsgBox box;
			do
			{
				CancellationTokenSource tokenSource = new CancellationTokenSource();
				box = new MsgBox(LocalizationManager.Localize("Check_RFID_Login"), LocalizationManager.Localize("Hold_RFID_token_to_reader"));
				Task task = Task.Run(delegate()
				{
					this.Wait4RfId(box, tokenSource.Token);
				}, tokenSource.Token);
				box.ShowDialog();
				tokenSource.Cancel();
				bool hasRfid = Rfid.Instance.HasRfid;
				if (hasRfid)
				{
					List<User> list = new List<User>();
					this.dbConnection.GetUserList(list, Rfid.Instance.Uid);
					bool flag = list.Count == 0;
					if (flag)
					{
						Rfid.Instance.StopTimer();
						Dialog.DialogResults dialogResults = Dialog.Show(LocalizationManager.Localize("Check_RFID_Login"), LocalizationManager.Localize("RFID_not_mapped_with_a_user_check_one_more"), DialogStyle.Warning, DialogButtons.OKCancel);
						bool flag2 = dialogResults > Dialog.DialogResults.Ok;
						if (flag2)
						{
							break;
						}
						Rfid.Instance.CleanUid();
					}
				}
				bool flag3 = box.result == Dialog.DialogResults.Cancel;
				if (flag3)
				{
					break;
				}
			}
			while (!Rfid.Instance.HasRfid);
			string uid = Rfid.Instance.Uid;
			bool flag4 = box.result != Dialog.DialogResults.Cancel && Rfid.Instance.HasRfid;
			if (flag4)
			{
				this.UserList.Clear();
				List<User> list2 = new List<User>();
				this.dbConnection.GetUserList(list2, uid);
				bool flag5 = list2.Count == 0;
				if (flag5)
				{
					this.dbConnection.GetUserList(list2);
				}
				foreach (User item in list2)
				{
					this.UserList.Add(item);
				}
			}
			Rfid.Instance.RfidDetected += this.LoginRfidUser;
		}

		// Token: 0x040019BA RID: 6586
		private User selectedUser;

		// Token: 0x040019BB RID: 6587
		private DBConnection dbConnection;

		// Token: 0x040019BC RID: 6588
		private ListCollectionView view;

		// Token: 0x040019BD RID: 6589
		private Keyboard keyboard;

		// Token: 0x040019BE RID: 6590
		private AccessRightsManager accessManager;

		// Token: 0x040019BF RID: 6591
		private Restore restore;

		// Token: 0x040019C6 RID: 6598
		private ObservableCollection<User> _userList;

		// Token: 0x040019C8 RID: 6600
		private bool _isFilterActive;

		// Token: 0x040019CA RID: 6602
		private LoginToGo loginToGo;
	}
}
