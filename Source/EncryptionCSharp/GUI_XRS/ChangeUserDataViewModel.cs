using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using Common;
using DatabaseConnection;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x02000227 RID: 551
	public class ChangeUserDataViewModel : INotifyPropertyChanged
	{
		// Token: 0x140000D9 RID: 217
		// (add) Token: 0x06002927 RID: 10535 RVA: 0x0011186C File Offset: 0x0010FA6C
		// (remove) Token: 0x06002928 RID: 10536 RVA: 0x001118A4 File Offset: 0x0010FAA4
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<bool> PasswordEntered;

		// Token: 0x17000B36 RID: 2870
		// (get) Token: 0x06002929 RID: 10537 RVA: 0x001118D9 File Offset: 0x0010FAD9
		// (set) Token: 0x0600292A RID: 10538 RVA: 0x001118E1 File Offset: 0x0010FAE1
		public UsermanagementViewModel usermanagementViewModel { get; set; }

		// Token: 0x140000DA RID: 218
		// (add) Token: 0x0600292B RID: 10539 RVA: 0x001118EC File Offset: 0x0010FAEC
		// (remove) Token: 0x0600292C RID: 10540 RVA: 0x00111924 File Offset: 0x0010FB24
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x17000B37 RID: 2871
		// (get) Token: 0x0600292D RID: 10541 RVA: 0x0011195C File Offset: 0x0010FB5C
		// (set) Token: 0x0600292E RID: 10542 RVA: 0x00111974 File Offset: 0x0010FB74
		public string ID
		{
			get
			{
				return this.iD;
			}
			set
			{
				this.iD = value;
				this.OnPropertyChanged("ID");
			}
		}

		// Token: 0x17000B38 RID: 2872
		// (get) Token: 0x0600292F RID: 10543 RVA: 0x0011198C File Offset: 0x0010FB8C
		// (set) Token: 0x06002930 RID: 10544 RVA: 0x001119A4 File Offset: 0x0010FBA4
		public string Username
		{
			get
			{
				return this.username;
			}
			set
			{
				this.username = value;
				this.OnPropertyChanged("Username");
			}
		}

		// Token: 0x17000B39 RID: 2873
		// (get) Token: 0x06002931 RID: 10545 RVA: 0x001119BC File Offset: 0x0010FBBC
		// (set) Token: 0x06002932 RID: 10546 RVA: 0x001119D4 File Offset: 0x0010FBD4
		public string Password
		{
			get
			{
				return this.password;
			}
			set
			{
				this.password = value;
				this.OnPropertyChanged("Password");
			}
		}

		// Token: 0x17000B3A RID: 2874
		// (get) Token: 0x06002933 RID: 10547 RVA: 0x001119EA File Offset: 0x0010FBEA
		// (set) Token: 0x06002934 RID: 10548 RVA: 0x001119F2 File Offset: 0x0010FBF2
		public string EnteredPassword { get; private set; }

		// Token: 0x17000B3B RID: 2875
		// (get) Token: 0x06002935 RID: 10549 RVA: 0x001119FC File Offset: 0x0010FBFC
		// (set) Token: 0x06002936 RID: 10550 RVA: 0x00111A14 File Offset: 0x0010FC14
		public string PasswordRepeate
		{
			get
			{
				return this.passwordRepeate;
			}
			set
			{
				this.passwordRepeate = value;
				this.OnPropertyChanged("PasswordRepeate");
			}
		}

		// Token: 0x17000B3C RID: 2876
		// (get) Token: 0x06002937 RID: 10551 RVA: 0x00111A2A File Offset: 0x0010FC2A
		// (set) Token: 0x06002938 RID: 10552 RVA: 0x00111A32 File Offset: 0x0010FC32
		public string EnteredPasswordRepeat { get; private set; }

		// Token: 0x17000B3D RID: 2877
		// (get) Token: 0x06002939 RID: 10553 RVA: 0x00111A3C File Offset: 0x0010FC3C
		// (set) Token: 0x0600293A RID: 10554 RVA: 0x00111A54 File Offset: 0x0010FC54
		public BuiltInPriviledges Right
		{
			get
			{
				return this.right;
			}
			set
			{
				this.right = value;
				this.OnPropertyChanged("Right");
				this.OnPropertyChanged("CommitErrorsVisible");
			}
		}

		// Token: 0x17000B3E RID: 2878
		// (get) Token: 0x0600293B RID: 10555 RVA: 0x00111A78 File Offset: 0x0010FC78
		// (set) Token: 0x0600293C RID: 10556 RVA: 0x00111A90 File Offset: 0x0010FC90
		public bool PwRepeatEnable
		{
			get
			{
				return this.pwRepeatEnable;
			}
			set
			{
				this.pwRepeatEnable = value;
				this.OnPropertyChanged("PwRepeatEnable");
			}
		}

		// Token: 0x17000B3F RID: 2879
		// (get) Token: 0x0600293D RID: 10557 RVA: 0x00111AA8 File Offset: 0x0010FCA8
		// (set) Token: 0x0600293E RID: 10558 RVA: 0x00111AC0 File Offset: 0x0010FCC0
		public bool CommitErrors
		{
			get
			{
				return this.commitErrors;
			}
			set
			{
				this.commitErrors = value;
				this.OnPropertyChanged("CommitErrors");
			}
		}

		// Token: 0x17000B40 RID: 2880
		// (get) Token: 0x0600293F RID: 10559 RVA: 0x00111AD8 File Offset: 0x0010FCD8
		public bool CommitErrorsVisible
		{
			get
			{
				return this.right < BuiltInPriviledges.Operator1;
			}
		}

		// Token: 0x17000B41 RID: 2881
		// (get) Token: 0x06002940 RID: 10560 RVA: 0x00111AF7 File Offset: 0x0010FCF7
		// (set) Token: 0x06002941 RID: 10561 RVA: 0x00111AFF File Offset: 0x0010FCFF
		public List<BuiltInPriviledges> SelectableUserRights { get; set; }

		// Token: 0x17000B42 RID: 2882
		// (get) Token: 0x06002942 RID: 10562 RVA: 0x00111B08 File Offset: 0x0010FD08
		// (set) Token: 0x06002943 RID: 10563 RVA: 0x00111B10 File Offset: 0x0010FD10
		public BoundLocalizedText StrPassword { get; set; }

		// Token: 0x17000B43 RID: 2883
		// (get) Token: 0x06002944 RID: 10564 RVA: 0x00111B19 File Offset: 0x0010FD19
		// (set) Token: 0x06002945 RID: 10565 RVA: 0x00111B21 File Offset: 0x0010FD21
		public BoundLocalizedText StrRepeatPassword { get; set; }

		// Token: 0x17000B44 RID: 2884
		// (get) Token: 0x06002946 RID: 10566 RVA: 0x00111B2A File Offset: 0x0010FD2A
		// (set) Token: 0x06002947 RID: 10567 RVA: 0x00111B32 File Offset: 0x0010FD32
		public RelayCommand ChangeIDCommand { get; set; }

		// Token: 0x17000B45 RID: 2885
		// (get) Token: 0x06002948 RID: 10568 RVA: 0x00111B3B File Offset: 0x0010FD3B
		// (set) Token: 0x06002949 RID: 10569 RVA: 0x00111B43 File Offset: 0x0010FD43
		public RelayCommand ChangeUsernameCommand { get; set; }

		// Token: 0x17000B46 RID: 2886
		// (get) Token: 0x0600294A RID: 10570 RVA: 0x00111B4C File Offset: 0x0010FD4C
		// (set) Token: 0x0600294B RID: 10571 RVA: 0x00111B54 File Offset: 0x0010FD54
		public RelayCommand InputPasswordCommand { get; set; }

		// Token: 0x17000B47 RID: 2887
		// (get) Token: 0x0600294C RID: 10572 RVA: 0x00111B5D File Offset: 0x0010FD5D
		// (set) Token: 0x0600294D RID: 10573 RVA: 0x00111B65 File Offset: 0x0010FD65
		public RelayCommand InputPasswordRepeatCommand { get; set; }

		// Token: 0x17000B48 RID: 2888
		// (get) Token: 0x0600294E RID: 10574 RVA: 0x00111B6E File Offset: 0x0010FD6E
		// (set) Token: 0x0600294F RID: 10575 RVA: 0x00111B76 File Offset: 0x0010FD76
		public RelayCommand DeregisterLocalizedTextCommand { get; set; }

		// Token: 0x06002950 RID: 10576 RVA: 0x00111B80 File Offset: 0x0010FD80
		public ChangeUserDataViewModel(DBConnection dbConnection, ChangeUserData.ChangeUserDataType type, UsermanagementViewModel usermanagementViewModel)
		{
			this.dbConnection = dbConnection;
			this.usermanagementViewModel = usermanagementViewModel;
			this.type = type;
			this.PwRepeatEnable = false;
			this.StrPassword = new BoundLocalizedText("txtPassword");
			this.StrRepeatPassword = new BoundLocalizedText("txtRepeatPassword");
			this.ChangeIDCommand = new RelayCommand(new Action<object>(this.ChangeID));
			this.ChangeUsernameCommand = new RelayCommand(new Action<object>(this.ChangeUsername));
			this.InputPasswordCommand = new RelayCommand(new Action<object>(this.InputPassword));
			this.InputPasswordRepeatCommand = new RelayCommand(new Action<object>(this.InputPasswordRepeat));
			this.DeregisterLocalizedTextCommand = new RelayCommand(new Action<object>(this.DeregisterLocalizedText));
			this.InitializeSelectableUserRights();
			bool flag = type == ChangeUserData.ChangeUserDataType.changeUserdata;
			if (flag)
			{
				this.ID = usermanagementViewModel.SelectedUser.ID.ToString();
				this.Password = usermanagementViewModel.SelectedUser.PasswordAsMD5;
				this.PasswordRepeate = usermanagementViewModel.SelectedUser.PasswordAsMD5;
				this.Right = usermanagementViewModel.SelectedUser.Rights;
				this.Username = usermanagementViewModel.SelectedUser.Username;
				this.CommitErrors = usermanagementViewModel.SelectedUser.CommitErrors;
			}
			else
			{
				this.ID = dbConnection.GetFirstFreeID().ToString();
				this.Right = BuiltInPriviledges.Operator3;
			}
		}

		// Token: 0x06002951 RID: 10577 RVA: 0x00111CF8 File Offset: 0x0010FEF8
		private void InitializeSelectableUserRights()
		{
			this.SelectableUserRights = new List<BuiltInPriviledges>();
			this.SelectableUserRights.Add(BuiltInPriviledges.Admin);
			this.SelectableUserRights.Add(BuiltInPriviledges.Operator1);
			this.SelectableUserRights.Add(BuiltInPriviledges.Operator2);
			this.SelectableUserRights.Add(BuiltInPriviledges.Operator3);
		}

		// Token: 0x06002952 RID: 10578 RVA: 0x00111D56 File Offset: 0x0010FF56
		private void DeregisterLocalizedText(object obj)
		{
			MainWindow.localizationManager.Deregister(this.StrPassword);
			MainWindow.localizationManager.Deregister(this.StrRepeatPassword);
		}

		// Token: 0x06002953 RID: 10579 RVA: 0x00111D7C File Offset: 0x0010FF7C
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x06002954 RID: 10580 RVA: 0x00111DA9 File Offset: 0x0010FFA9
		private void ChangeID(object dummy)
		{
			this.numKeyboard = new NumKeyboard("User ID", this.ID, NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_ChangeID;
			this.numKeyboard.Show();
		}

		// Token: 0x06002955 RID: 10581 RVA: 0x00111DE8 File Offset: 0x0010FFE8
		private void numKeyboard_ChangeID(object sender, KeyboardInputEventArgs e)
		{
			bool flag = true;
			this.dbConnection.CheckUserID(Convert.ToInt32(e.Text), ref flag);
			bool flag2 = flag;
			if (flag2)
			{
				this.ID = e.Text.ToString();
				this.numKeyboard.TextInputKeyboard -= this.numKeyboard_ChangeID;
			}
			else
			{
				Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("msgIDAlreadyExistent"), DialogStyle.Error);
				this.numKeyboard.ShouldClose = false;
			}
		}

		// Token: 0x06002956 RID: 10582 RVA: 0x00111E6E File Offset: 0x0011006E
		private void ChangeUsername(object dummy)
		{
			this.keyboard = new Keyboard("Username", this.Username, KeyboardType.Application);
			this.keyboard.TextInputKeyboard += this.keyboard_ChangeUsername;
			this.keyboard.Show();
		}

		// Token: 0x06002957 RID: 10583 RVA: 0x00111EAC File Offset: 0x001100AC
		private void keyboard_ChangeUsername(object sender, KeyboardInputEventArgs e)
		{
			this.Username = e.Text;
			this.keyboard.TextInputKeyboard -= this.keyboard_ChangeUsername;
		}

		// Token: 0x06002958 RID: 10584 RVA: 0x00111ED4 File Offset: 0x001100D4
		private void InputPassword(object dummy)
		{
			this.keyboard = new Keyboard(LocalizationManager.Localize("msgEnterPassword"), new string[]
			{
				""
			}, false);
			this.keyboard.TextInputKeyboard += this.keyboard_InputPassword;
			this.keyboard.Show();
		}

		// Token: 0x06002959 RID: 10585 RVA: 0x00111F2C File Offset: 0x0011012C
		private void keyboard_InputPassword(object sender, KeyboardInputEventArgs e)
		{
			this.Password = e.Text.ToMD5();
			this.EnteredPassword = e.Text;
			this.PasswordRepeate = "";
			this.keyboard.TextInputKeyboard -= this.keyboard_InputPassword;
			bool flag = this.Password.Length > 0;
			if (flag)
			{
				this.PwRepeatEnable = true;
			}
			else
			{
				this.PwRepeatEnable = false;
			}
			this.OnPasswordEntered(false);
		}

		// Token: 0x0600295A RID: 10586 RVA: 0x00111FAC File Offset: 0x001101AC
		private void InputPasswordRepeat(object dummy)
		{
			bool flag = this.PwRepeatEnable;
			if (flag)
			{
				this.keyboard = new Keyboard(LocalizationManager.Localize("msgRepeatPassword"), new string[]
				{
					this.Password
				}, true);
				this.keyboard.TextInputKeyboard += this.keyboard_InputPasswordRepeat;
				this.keyboard.Show();
			}
			else
			{
				Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("msgInsertPasswordFirst"), DialogStyle.Error);
			}
		}

		// Token: 0x0600295B RID: 10587 RVA: 0x0011202C File Offset: 0x0011022C
		private void keyboard_InputPasswordRepeat(object sender, KeyboardInputEventArgs e)
		{
			this.PasswordRepeate = e.Text.ToMD5();
			this.EnteredPasswordRepeat = e.Text;
			this.keyboard.TextInputKeyboard -= this.keyboard_InputPasswordRepeat;
			this.OnPasswordEntered(true);
		}

		// Token: 0x0600295C RID: 10588 RVA: 0x0011207C File Offset: 0x0011027C
		public bool SubmitChanges()
		{
			bool result = false;
			try
			{
				bool flag = !string.IsNullOrEmpty(this.Username) && !string.IsNullOrEmpty(this.ID) && !string.IsNullOrEmpty(this.Password);
				if (flag)
				{
					bool flag2 = this.Password == this.PasswordRepeate;
					if (flag2)
					{
						User user = new User();
						user.ID = int.Parse(this.ID);
						user.PasswordAsMD5 = this.Password;
						user.Rights = this.Right;
						user.Username = this.Username;
						user.CommitErrors = this.CommitErrors;
						bool flag3 = this.type == ChangeUserData.ChangeUserDataType.newUser;
						if (flag3)
						{
							user.Deletable = true;
							this.dbConnection.CreateNewUser(user);
							this.usermanagementViewModel.UserList.Add(user);
							LogbookManager.AddEntry(SSTEntryCode.UserCreated, 0, user.ID.ToString(), user.Username);
						}
						else
						{
							bool flag4 = this.type == ChangeUserData.ChangeUserDataType.changeUserdata;
							if (flag4)
							{
								user.Deletable = this.usermanagementViewModel.SelectedUser.Deletable;
								int id = this.usermanagementViewModel.SelectedUser.ID;
								this.usermanagementViewModel.SelectedUser.ID = user.ID;
								this.usermanagementViewModel.SelectedUser.PasswordAsMD5 = user.PasswordAsMD5;
								this.usermanagementViewModel.SelectedUser.Rights = user.Rights;
								this.usermanagementViewModel.SelectedUser.Username = user.Username;
								this.usermanagementViewModel.SelectedUser.CommitErrors = user.CommitErrors;
								bool flag5 = this.ID == this.usermanagementViewModel.CurrentUser.ID.ToString();
								if (flag5)
								{
									this.usermanagementViewModel.SetCurrentUserPropertiesToSelectedUserProperties(false);
								}
								this.dbConnection.UpdateUser(user, id);
								LogbookManager.AddEntry(SSTEntryCode.UserParameterChanged, 0, user.ID.ToString(), user.Username);
							}
						}
						result = true;
					}
					else
					{
						Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("msgPasswordsDontMatch"), DialogStyle.Warning);
					}
				}
				else
				{
					Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("msgUserFieldEntriesMissing"), DialogStyle.Warning);
				}
			}
			catch (Exception ex)
			{
				Dialog.Show(LocalizationManager.Localize("txtError"), ex.Message, DialogStyle.Error);
			}
			return result;
		}

		// Token: 0x0600295D RID: 10589 RVA: 0x00112334 File Offset: 0x00110534
		protected virtual void OnPasswordEntered(bool repeatedPassword)
		{
			Action<bool> passwordEntered = this.PasswordEntered;
			bool flag = passwordEntered != null;
			if (flag)
			{
				passwordEntered(repeatedPassword);
			}
		}

		// Token: 0x0400199F RID: 6559
		private string passwordRepeate;

		// Token: 0x040019A0 RID: 6560
		private string password;

		// Token: 0x040019A1 RID: 6561
		private string username;

		// Token: 0x040019A2 RID: 6562
		private string iD;

		// Token: 0x040019A3 RID: 6563
		private BuiltInPriviledges right;

		// Token: 0x040019A4 RID: 6564
		private bool pwRepeatEnable;

		// Token: 0x040019A5 RID: 6565
		private Keyboard keyboard;

		// Token: 0x040019A6 RID: 6566
		private NumKeyboard numKeyboard;

		// Token: 0x040019A7 RID: 6567
		private DBConnection dbConnection;

		// Token: 0x040019A8 RID: 6568
		private ChangeUserData.ChangeUserDataType type;

		// Token: 0x040019A9 RID: 6569
		private bool commitErrors;
	}
}
