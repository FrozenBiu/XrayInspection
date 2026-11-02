using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

namespace GUI_XRS
{
	// Token: 0x02000240 RID: 576
	public class HomeManager : INotifyPropertyChanged
	{
		// Token: 0x17000BDF RID: 3039
		// (get) Token: 0x06002BC9 RID: 11209 RVA: 0x00121954 File Offset: 0x0011FB54
		public static HomeManager Instance
		{
			get
			{
				return HomeManager.instance;
			}
		}

		// Token: 0x17000BE0 RID: 3040
		// (get) Token: 0x06002BCA RID: 11210 RVA: 0x0012196C File Offset: 0x0011FB6C
		public int NumSubWindows
		{
			get
			{
				return this.windows.Count;
			}
		}

		// Token: 0x140000F4 RID: 244
		// (add) Token: 0x06002BCB RID: 11211 RVA: 0x0012198C File Offset: 0x0011FB8C
		// (remove) Token: 0x06002BCC RID: 11212 RVA: 0x001219C4 File Offset: 0x0011FBC4
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x06002BCE RID: 11214 RVA: 0x00121A06 File Offset: 0x0011FC06
		private HomeManager()
		{
		}

		// Token: 0x06002BCF RID: 11215 RVA: 0x00121A1B File Offset: 0x0011FC1B
		public void Register(Subwindow window)
		{
			this.windows.Add(window);
			this.OnPropertyChanged("NumSubWindows");
		}

		// Token: 0x06002BD0 RID: 11216 RVA: 0x00121A37 File Offset: 0x0011FC37
		public void Deregister(Subwindow window)
		{
			this.windows.Remove(window);
			this.OnPropertyChanged("NumSubWindows");
		}

		// Token: 0x06002BD1 RID: 11217 RVA: 0x00121A54 File Offset: 0x0011FC54
		public void CloseAll()
		{
			for (int i = this.windows.Count - 1; i >= 0; i--)
			{
				this.windows[i].Close();
			}
		}

		// Token: 0x06002BD2 RID: 11218 RVA: 0x00121A98 File Offset: 0x0011FC98
		public void BringWindowsToFront()
		{
			for (int i = 0; i < this.windows.Count; i++)
			{
				this.windows[i].Activate();
			}
		}

		// Token: 0x06002BD3 RID: 11219 RVA: 0x00121AD4 File Offset: 0x0011FCD4
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x04001B79 RID: 7033
		private static readonly HomeManager instance = new HomeManager();

		// Token: 0x04001B7A RID: 7034
		private List<Subwindow> windows = new List<Subwindow>();
	}
}
