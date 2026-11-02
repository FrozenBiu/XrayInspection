using System;
using System.ComponentModel;
using System.Diagnostics;

namespace GUI_XRS
{
	// Token: 0x0200023B RID: 571
	public class CurrentApplicationName : INotifyPropertyChanged
	{
		// Token: 0x140000F3 RID: 243
		// (add) Token: 0x06002BAF RID: 11183 RVA: 0x001212D8 File Offset: 0x0011F4D8
		// (remove) Token: 0x06002BB0 RID: 11184 RVA: 0x00121310 File Offset: 0x0011F510
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x17000BDD RID: 3037
		// (get) Token: 0x06002BB1 RID: 11185 RVA: 0x00121348 File Offset: 0x0011F548
		// (set) Token: 0x06002BB2 RID: 11186 RVA: 0x00121360 File Offset: 0x0011F560
		public string AppName
		{
			get
			{
				return this.appName;
			}
			set
			{
				this.appName = value;
				this.OnPropertyChanged("AppName");
			}
		}

		// Token: 0x06002BB3 RID: 11187 RVA: 0x00121378 File Offset: 0x0011F578
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x04001B50 RID: 6992
		private string appName;
	}
}
