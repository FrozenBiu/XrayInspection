using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Timers;
using System.Windows;
using System.Windows.Threading;
using DatabaseConnection;
using Definitions;
using ICSharpCode.SharpZipLib.Zip;

namespace GUI_XRS
{
	// Token: 0x02000122 RID: 290
	public class LoginToGo
	{
		// Token: 0x1700083C RID: 2108
		// (get) Token: 0x06001BFC RID: 7164 RVA: 0x000B4624 File Offset: 0x000B2824
		// (set) Token: 0x06001BFD RID: 7165 RVA: 0x000B463C File Offset: 0x000B283C
		public bool CanCreateSesotecService
		{
			get
			{
				return this.canCreateSesotecService;
			}
			set
			{
				this.canCreateSesotecService = value;
			}
		}

		// Token: 0x06001BFE RID: 7166 RVA: 0x000B4648 File Offset: 0x000B2848
		public void Seal(string usbPath)
		{
			bool flag = File.Exists(usbPath + "ID.tag");
			if (flag)
			{
				File.Delete(usbPath + "ID.tag");
			}
			using (StreamWriter streamWriter = new StreamWriter(usbPath + "ID.tag"))
			{
				string value = new FileInfo(usbPath + "ID.tag")
				{
					Attributes = (FileAttributes.Hidden | FileAttributes.System)
				}.CreationTime.Ticks.ToString();
				streamWriter.WriteLine(value);
			}
		}

		// Token: 0x06001BFF RID: 7167 RVA: 0x000B46E8 File Offset: 0x000B28E8
		public string GetSealedFileTicks(string usbPath)
		{
			string result;
			using (StreamReader streamReader = new StreamReader(usbPath + "ID.tag"))
			{
				result = streamReader.ReadToEnd().TrimEnd(new char[0]);
			}
			return result;
		}

		// Token: 0x06001C00 RID: 7168 RVA: 0x000B4738 File Offset: 0x000B2938
		public string GetSealedFileCreationTimeTicks(string usbPath)
		{
			return File.GetCreationTime(usbPath + "ID.tag").Ticks.ToString();
		}

		// Token: 0x06001C01 RID: 7169 RVA: 0x000B476C File Offset: 0x000B296C
		public bool ZipAndCorrectSealedFileExist(string usbPath)
		{
			bool result = false;
			bool flag = File.Exists(usbPath + "LTG" + Definition.binFileEnding) && File.Exists(usbPath + "ID.tag");
			if (flag)
			{
				result = (this.GetSealedFileTicks(usbPath) == this.GetSealedFileCreationTimeTicks(usbPath));
			}
			return result;
		}

		// Token: 0x06001C02 RID: 7170 RVA: 0x000B47C4 File Offset: 0x000B29C4
		public bool GetUserNameAndID(string usbPath, ref User user)
		{
			bool result = false;
			bool flag = this.ExtractZip(usbPath);
			if (flag)
			{
				SQLiteConnection sqliteConnection = new SQLiteConnection("Data Source=" + Path.GetTempPath() + "LTG" + Definition.binFileEnding);
				sqliteConnection.Open();
				bool flag2 = sqliteConnection.State == ConnectionState.Open;
				if (flag2)
				{
					bool flag3 = TableOperations.TableExists(sqliteConnection, "User");
					if (flag3)
					{
						this.GetUserData(sqliteConnection, ref user);
						result = true;
					}
					sqliteConnection.Close();
				}
			}
			return result;
		}

		// Token: 0x06001C03 RID: 7171 RVA: 0x000B4844 File Offset: 0x000B2A44
		private void GetUserData(SQLiteConnection connection, ref User user)
		{
			try
			{
				SQLiteDataReader sqliteDataReader = new SQLiteCommand(connection)
				{
					CommandText = "SELECT * FROM User",
					Connection = connection
				}.ExecuteReader();
				sqliteDataReader.Read();
				try
				{
					int id = 0;
					bool flag = int.TryParse(sqliteDataReader["ID"].ToString(), out id);
					if (flag)
					{
						user.ID = id;
					}
				}
				catch
				{
				}
				try
				{
					user.Username = sqliteDataReader["Name"].ToString();
				}
				catch
				{
				}
				try
				{
					user.Rights = (BuiltInPriviledges)Enum.Parse(typeof(BuiltInPriviledges), sqliteDataReader["Priviledge"].ToString());
				}
				catch
				{
				}
				try
				{
					user.PasswordAsMD5 = sqliteDataReader["Password"].ToString();
				}
				catch
				{
				}
				try
				{
					user.Language = (Languages)Enum.Parse(typeof(Languages), sqliteDataReader["Language"].ToString());
				}
				catch
				{
				}
				try
				{
					user.BuiltIn = (sqliteDataReader["BuiltIn"].ToString() == "1");
				}
				catch
				{
				}
				try
				{
					user.Deletable = (sqliteDataReader["Deleteable"].ToString() == "1");
				}
				catch
				{
				}
				try
				{
					user.LeftHanded = (sqliteDataReader["LeftHanded"].ToString() == "1");
				}
				catch
				{
				}
				try
				{
					user.VelocityUnit = (UnitVelocity)Enum.Parse(typeof(UnitVelocity), sqliteDataReader["UnitVelocity"].ToString());
				}
				catch
				{
				}
				try
				{
					user.DistanceUnit = (UnitDistance)Enum.Parse(typeof(UnitDistance), sqliteDataReader["UnitDistance"].ToString());
				}
				catch
				{
				}
			}
			catch
			{
			}
		}

		// Token: 0x06001C04 RID: 7172 RVA: 0x000B4B60 File Offset: 0x000B2D60
		private bool ExtractZip(string usbPath)
		{
			bool result = false;
			bool flag = this.ZipAndCorrectSealedFileExist(usbPath);
			if (flag)
			{
				try
				{
					new FastZip
					{
						Password = "SeSoTec94513"
					}.ExtractZip(usbPath + "LTG" + Definition.binFileEnding, Path.GetTempPath(), FastZip.Overwrite.Always, null, null, null, false);
					result = true;
				}
				catch
				{
				}
			}
			return result;
		}

		// Token: 0x06001C05 RID: 7173 RVA: 0x000B4BD0 File Offset: 0x000B2DD0
		public bool CreateLTGUser(User user, string usbPath)
		{
			bool result = false;
			try
			{
				string tempPath = Path.GetTempPath();
				bool flag = File.Exists(tempPath + "LTG" + Definition.binFileEnding);
				if (flag)
				{
					File.Delete(tempPath + "LTG" + Definition.binFileEnding);
				}
				SQLiteConnection sqliteConnection = new SQLiteConnection("Data Source=" + tempPath + "LTG" + Definition.binFileEnding);
				sqliteConnection.Open();
				bool flag2 = sqliteConnection.State == ConnectionState.Open;
				if (flag2)
				{
					TableOperations.ExecuteCommand(sqliteConnection, "CREATE TABLE IF NOT EXISTS User (ID INTEGER NOT NULL PRIMARY KEY, Name TEXT, Priviledge INTEGER, Password TEXT, Language INTEGER, BuiltIn INTEGER, Deleteable INTEGER, LeftHanded INTEGER, UnitVelocity INTEGER, UnitDistance INTEGER, UnitWeight INTEGER);");
					using (SQLiteCommand sqliteCommand = new SQLiteCommand(sqliteConnection))
					{
						sqliteCommand.CommandText = string.Concat(new string[]
						{
							"INSERT INTO User (ID, Name, Priviledge, Password, Language, BuiltIn, Deleteable, LeftHanded, UnitVelocity, UnitDistance, UnitWeight) VALUES ('",
							user.ID.ToString(),
							"', '",
							user.Username,
							"', '",
							((int)user.Rights).ToString(),
							"', '",
							user.PasswordAsMD5,
							"', '",
							((int)user.Language).ToString(),
							"', '",
							user.BuiltIn ? "1" : "0",
							"', '",
							user.Deletable ? "1" : "0",
							"', '",
							user.LeftHanded ? "1" : "0",
							"', '",
							((int)user.VelocityUnit).ToString(),
							"', '",
							((int)user.DistanceUnit).ToString(),
							"', '",
							((int)user.WeightUnit).ToString(),
							"');"
						});
						sqliteCommand.ExecuteNonQuery();
					}
					sqliteConnection.Close();
					new FastZip
					{
						Password = "SeSoTec94513"
					}.CreateZip(tempPath + "LTG" + Definition.zipFileEnding, tempPath, false, (tempPath + "LTG" + Definition.binFileEnding).Replace("\\", "\\\\"));
					bool flag3 = File.Exists(usbPath + "LTG" + Definition.binFileEnding);
					if (flag3)
					{
						File.Delete(usbPath + "LTG" + Definition.binFileEnding);
					}
					File.Delete(tempPath + "LTG" + Definition.binFileEnding);
					bool flag4 = Directory.Exists(usbPath);
					if (flag4)
					{
						File.Move(tempPath + "LTG" + Definition.zipFileEnding, usbPath + "LTG" + Definition.binFileEnding);
						this.Seal(usbPath);
						result = true;
					}
					else
					{
						File.Delete(tempPath + "LTG" + Definition.zipFileEnding);
					}
				}
			}
			catch
			{
			}
			return result;
		}

		// Token: 0x06001C06 RID: 7174 RVA: 0x000B4EF4 File Offset: 0x000B30F4
		public bool LoginToGoUserExists(string usbPath)
		{
			return File.Exists(usbPath + "LTG" + Definition.binFileEnding);
		}

		// Token: 0x06001C07 RID: 7175 RVA: 0x000B4F1C File Offset: 0x000B311C
		public bool NotifyNewStick()
		{
			bool result = false;
			this.stickInProgress = true;
			bool flag = this.CanCreateSesotecService;
			if (flag)
			{
				string text = "";
				this.dbConnection.GetUsbDriveLetter(ref text);
				text += "\\";
				List<User> list = new List<User>();
				this.dbConnection.GetUserList(list);
				int id = -1;
				bool flag2 = Dialog.Show(LocalizationManager.Localize("txtInfo"), LocalizationManager.Localize("txtMakeSesotecService"), DialogStyle.Info, DialogButtons.OKCancel, false) == Dialog.DialogResults.Cancel;
				if (flag2)
				{
					bool flag3 = Dialog.Show(LocalizationManager.Localize("txtInfo"), LocalizationManager.Localize("txtMakeValidator"), DialogStyle.Info, DialogButtons.OKCancel, false) == Dialog.DialogResults.Ok;
					if (flag3)
					{
						id = -2;
					}
					else
					{
						bool flag4 = Dialog.Show(LocalizationManager.Localize("txtInfo"), LocalizationManager.Localize("txtMakeEndAssembly"), DialogStyle.Info, DialogButtons.OKCancel, false) == Dialog.DialogResults.Ok;
						if (flag4)
						{
							id = -3;
						}
						else
						{
							id = -100;
						}
					}
				}
				bool flag5 = id >= -3;
				if (flag5)
				{
					User user = list.Find((User x) => x.ID == id);
					bool flag6 = true;
					bool flag7 = this.LoginToGoUserExists(text);
					if (flag7)
					{
						flag6 = (Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtOverwriteLoginToGoUser"), DialogStyle.Warning, DialogButtons.OKCancel) == Dialog.DialogResults.Ok);
					}
					bool flag8 = flag6;
					if (flag8)
					{
						bool flag9 = this.CreateLTGUser(user, text);
						if (flag9)
						{
							result = true;
							this.CanCreateSesotecService = false;
							this.loginIVPTimer.Stop();
							this.loginIVPTimer.Dispose();
							Dialog.Show(LocalizationManager.Localize("txtSuccess"), LocalizationManager.Localize("txtSuccess"), DialogStyle.Ok, DialogButtons.None, true);
						}
						else
						{
							result = false;
							Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtError"), DialogStyle.Error, DialogButtons.None, true);
						}
					}
				}
			}
			this.stickInProgress = false;
			return result;
		}

		// Token: 0x06001C08 RID: 7176 RVA: 0x000B50FC File Offset: 0x000B32FC
		public void startTimer(DBConnection dbCon)
		{
			this.CanCreateSesotecService = true;
			this.dbConnection = dbCon;
			this.loginIVPTimer = new Timer();
			this.loginIVPTimer.Elapsed += this.loginIVPTimer_Elapsed;
			this.loginIVPTimer.Interval = 60000.0;
			this.loginIVPTimer.Start();
		}

		// Token: 0x06001C09 RID: 7177 RVA: 0x000B5160 File Offset: 0x000B3360
		private void loginIVPTimer_Elapsed(object sender, ElapsedEventArgs e)
		{
			this.loginIVPTimer.Stop();
			this.loginIVPTimer.Dispose();
			this.CanCreateSesotecService = false;
			bool flag = Application.Current != null;
			if (flag)
			{
				Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
				{
					Dialog.Show(LocalizationManager.Localize("txtInfo"), LocalizationManager.Localize("txtTimeElapsed"), DialogStyle.Info, DialogButtons.OK);
				}));
			}
		}

		// Token: 0x04001296 RID: 4758
		private const string sealedFileName = "ID.tag";

		// Token: 0x04001297 RID: 4759
		private const string zipAndDBFileName = "LTG";

		// Token: 0x04001298 RID: 4760
		private const string zipPassword = "SeSoTec94513";

		// Token: 0x04001299 RID: 4761
		private const string userTableName = "User";

		// Token: 0x0400129A RID: 4762
		public static readonly string geniusFolderName = "GTUIv3";

		// Token: 0x0400129B RID: 4763
		public bool stickInProgress = false;

		// Token: 0x0400129C RID: 4764
		private bool canCreateSesotecService = false;

		// Token: 0x0400129D RID: 4765
		private DBConnection dbConnection;

		// Token: 0x0400129E RID: 4766
		private Timer loginIVPTimer = new Timer();
	}
}
