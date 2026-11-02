using System;
using DatabaseConnection;

namespace GUI_XRS
{
	// Token: 0x02000256 RID: 598
	public class UserChangeEventArgs : EventArgs
	{
		// Token: 0x17000C54 RID: 3156
		// (get) Token: 0x06002DDF RID: 11743 RVA: 0x0012AEF4 File Offset: 0x001290F4
		public User User
		{
			get
			{
				return this.user;
			}
		}

		// Token: 0x06002DE0 RID: 11744 RVA: 0x0012AF0C File Offset: 0x0012910C
		public UserChangeEventArgs(User user)
		{
			this.user = user;
		}

		// Token: 0x04001CA4 RID: 7332
		private readonly User user;
	}
}
