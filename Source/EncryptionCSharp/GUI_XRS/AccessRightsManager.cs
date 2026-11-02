using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Threading;
using DatabaseConnection;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x0200018A RID: 394
	public class AccessRightsManager
	{
		// Token: 0x140000C3 RID: 195
		// (add) Token: 0x0600252B RID: 9515 RVA: 0x000FFFB4 File Offset: 0x000FE1B4
		// (remove) Token: 0x0600252C RID: 9516 RVA: 0x000FFFEC File Offset: 0x000FE1EC
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action UserAutoLogout;

		// Token: 0x140000C4 RID: 196
		// (add) Token: 0x0600252D RID: 9517 RVA: 0x00100024 File Offset: 0x000FE224
		// (remove) Token: 0x0600252E RID: 9518 RVA: 0x0010005C File Offset: 0x000FE25C
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action PropertiesSet;

		// Token: 0x17000A88 RID: 2696
		// (get) Token: 0x0600252F RID: 9519 RVA: 0x00100094 File Offset: 0x000FE294
		// (set) Token: 0x06002530 RID: 9520 RVA: 0x0001092C File Offset: 0x0000EB2C
		public User CurrentUser
		{
			get
			{
				return this.currentUser;
			}
			private set
			{
			}
		}

		// Token: 0x140000C5 RID: 197
		// (add) Token: 0x06002531 RID: 9521 RVA: 0x001000AC File Offset: 0x000FE2AC
		// (remove) Token: 0x06002532 RID: 9522 RVA: 0x001000E4 File Offset: 0x000FE2E4
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action ResetRetrain;

		// Token: 0x06002533 RID: 9523 RVA: 0x0010011C File Offset: 0x000FE31C
		public AccessRightsManager(User currentUser, bool autoLogout, DBConnection dbConnection, SetFooterMainWindow setFooterMainWindow)
		{
			this.elements = new List<AccessRight>();
			this.currentUser = currentUser;
			this.autoLogout = autoLogout;
			this.setFooterMainWindow = setFooterMainWindow;
			bool flag = dbConnection != null;
			if (flag)
			{
				dbConnection.GetGUIInactiveTime(ref AccessRightsManager.timeToGetActive);
			}
			this.timerUserActivity = new DispatcherTimer();
			if (autoLogout)
			{
				this.timerUserActivity.Interval = new TimeSpan(0, AccessRightsManager.timeToGetActive, 0);
				this.timerUserActivity.Tick += this.timerUserActivity_Tick;
				this.timerUserActivity.Start();
			}
		}

		// Token: 0x06002534 RID: 9524 RVA: 0x001001B4 File Offset: 0x000FE3B4
		private void timerUserActivity_Tick(object sender, EventArgs e)
		{
			bool flag = this.CurrentUser.ID != 0;
			if (flag)
			{
				this.LogoutUser();
			}
			bool flag2 = this.setFooterMainWindow != null;
			if (flag2)
			{
				this.setFooterMainWindow.UnSetFooterDeactivation();
			}
		}

		// Token: 0x06002535 RID: 9525 RVA: 0x001001F4 File Offset: 0x000FE3F4
		public void RegisterControl(UIElement element, BuiltInPriviledges accessible, BuiltInPriviledges visible)
		{
			this.elements.Add(new AccessRight(element, accessible, visible));
		}

		// Token: 0x06002536 RID: 9526 RVA: 0x0010020C File Offset: 0x000FE40C
		public void SetProperties()
		{
			foreach (AccessRight accessRight in this.elements)
			{
				accessRight.SetProperties(this.currentUser);
			}
			this.OnPropertiesSet();
		}

		// Token: 0x06002537 RID: 9527 RVA: 0x00100274 File Offset: 0x000FE474
		public bool DeregisterControl(UIElement element)
		{
			bool result = false;
			for (int i = 0; i < this.elements.Count; i++)
			{
				bool flag = this.elements[i].Element == element;
				if (flag)
				{
					this.elements.RemoveAt(i);
					break;
				}
			}
			return result;
		}

		// Token: 0x06002538 RID: 9528 RVA: 0x001002D0 File Offset: 0x000FE4D0
		public void LogoutUser()
		{
			LogbookManager.AddEntry(SSTEntryCode.UserLoggedOut, 0, this.CurrentUser.Username, "", "", "");
			User.GetDefaultUserStats(this.CurrentUser);
			this.SetProperties();
			this.OnUserAutoLogout();
		}

		// Token: 0x06002539 RID: 9529 RVA: 0x00100320 File Offset: 0x000FE520
		public void SignalActivity()
		{
			bool flag = this.autoLogout;
			if (flag)
			{
				this.timerUserActivity.Stop();
				this.timerUserActivity.Start();
			}
		}

		// Token: 0x0600253A RID: 9530 RVA: 0x00100354 File Offset: 0x000FE554
		public void StopTimerAutoLogout()
		{
			bool flag = this.autoLogout;
			if (flag)
			{
				this.timerUserActivity.Stop();
			}
		}

		// Token: 0x0600253B RID: 9531 RVA: 0x0010037C File Offset: 0x000FE57C
		public void StartTimerAutoLogout()
		{
			bool flag = this.autoLogout;
			if (flag)
			{
				this.timerUserActivity.Start();
			}
		}

		// Token: 0x0600253C RID: 9532 RVA: 0x001003A2 File Offset: 0x000FE5A2
		public void SetTimeInactiveGUI(int timespan)
		{
			this.timerUserActivity.Stop();
			this.timerUserActivity.Interval = new TimeSpan(0, timespan, 0);
			this.timerUserActivity.Start();
		}

		// Token: 0x0600253D RID: 9533 RVA: 0x001003D4 File Offset: 0x000FE5D4
		protected virtual void OnUserAutoLogout()
		{
			Action userAutoLogout = this.UserAutoLogout;
			bool flag = userAutoLogout != null;
			if (flag)
			{
				userAutoLogout();
			}
		}

		// Token: 0x0600253E RID: 9534 RVA: 0x001003F8 File Offset: 0x000FE5F8
		protected virtual void OnPropertiesSet()
		{
			Action propertiesSet = this.PropertiesSet;
			bool flag = propertiesSet != null;
			if (flag)
			{
				propertiesSet();
			}
		}

		// Token: 0x0600253F RID: 9535 RVA: 0x0010041C File Offset: 0x000FE61C
		internal void OnResetRetrain()
		{
			Action resetRetrain = this.ResetRetrain;
			bool flag = resetRetrain != null;
			if (flag)
			{
				resetRetrain();
			}
		}

		// Token: 0x040017E8 RID: 6120
		private List<AccessRight> elements;

		// Token: 0x040017E9 RID: 6121
		private User currentUser;

		// Token: 0x040017EA RID: 6122
		private DispatcherTimer timerUserActivity;

		// Token: 0x040017EB RID: 6123
		private bool autoLogout;

		// Token: 0x040017EC RID: 6124
		private SetFooterMainWindow setFooterMainWindow;

		// Token: 0x040017EF RID: 6127
		public static int timeToGetActive;
	}
}
