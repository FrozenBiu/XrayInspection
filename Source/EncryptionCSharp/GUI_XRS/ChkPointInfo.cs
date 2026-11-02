using System;
using System.ComponentModel;
using System.Diagnostics;

namespace GUI_XRS
{
	// Token: 0x0200013E RID: 318
	public class ChkPointInfo : INotifyPropertyChanged
	{
		// Token: 0x170008C8 RID: 2248
		// (get) Token: 0x06001E83 RID: 7811 RVA: 0x000C88EC File Offset: 0x000C6AEC
		// (set) Token: 0x06001E84 RID: 7812 RVA: 0x000C88F4 File Offset: 0x000C6AF4
		public int ChkIndex { get; set; }

		// Token: 0x170008C9 RID: 2249
		// (get) Token: 0x06001E85 RID: 7813 RVA: 0x000C88FD File Offset: 0x000C6AFD
		// (set) Token: 0x06001E86 RID: 7814 RVA: 0x000C8905 File Offset: 0x000C6B05
		public int LstIndex { get; set; }

		// Token: 0x170008CA RID: 2250
		// (get) Token: 0x06001E87 RID: 7815 RVA: 0x000C8910 File Offset: 0x000C6B10
		// (set) Token: 0x06001E88 RID: 7816 RVA: 0x000C8928 File Offset: 0x000C6B28
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
				this.OnPropertyChanged("Name");
			}
		}

		// Token: 0x170008CB RID: 2251
		// (get) Token: 0x06001E89 RID: 7817 RVA: 0x000C8940 File Offset: 0x000C6B40
		// (set) Token: 0x06001E8A RID: 7818 RVA: 0x000C8958 File Offset: 0x000C6B58
		public DateTime Date
		{
			get
			{
				return this.date;
			}
			set
			{
				this.date = value;
				this.OnPropertyChanged("Date");
			}
		}

		// Token: 0x170008CC RID: 2252
		// (get) Token: 0x06001E8B RID: 7819 RVA: 0x000C8970 File Offset: 0x000C6B70
		// (set) Token: 0x06001E8C RID: 7820 RVA: 0x000C8988 File Offset: 0x000C6B88
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

		// Token: 0x170008CD RID: 2253
		// (get) Token: 0x06001E8D RID: 7821 RVA: 0x000C89A0 File Offset: 0x000C6BA0
		// (set) Token: 0x06001E8E RID: 7822 RVA: 0x000C89B8 File Offset: 0x000C6BB8
		public bool Available
		{
			get
			{
				return this.available;
			}
			set
			{
				this.available = value;
				this.OnPropertyChanged("Available");
			}
		}

		// Token: 0x140000A2 RID: 162
		// (add) Token: 0x06001E8F RID: 7823 RVA: 0x000C89D0 File Offset: 0x000C6BD0
		// (remove) Token: 0x06001E90 RID: 7824 RVA: 0x000C8A08 File Offset: 0x000C6C08
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x170008CE RID: 2254
		// (get) Token: 0x06001E91 RID: 7825 RVA: 0x000C8A40 File Offset: 0x000C6C40
		// (set) Token: 0x06001E92 RID: 7826 RVA: 0x000C8A58 File Offset: 0x000C6C58
		public bool IsChanged
		{
			get
			{
				return this.isChanged;
			}
			set
			{
				this.isChanged = value;
				this.OnPropertyChanged("IsChanged");
			}
		}

		// Token: 0x170008CF RID: 2255
		// (get) Token: 0x06001E93 RID: 7827 RVA: 0x000C8A70 File Offset: 0x000C6C70
		// (set) Token: 0x06001E94 RID: 7828 RVA: 0x000C8A88 File Offset: 0x000C6C88
		public bool IsSelected
		{
			get
			{
				return this.isSelected;
			}
			set
			{
				this.isSelected = value;
				this.OnPropertyChanged("IsSelected");
			}
		}

		// Token: 0x170008D0 RID: 2256
		// (get) Token: 0x06001E95 RID: 7829 RVA: 0x000C8AA0 File Offset: 0x000C6CA0
		// (set) Token: 0x06001E96 RID: 7830 RVA: 0x000C8AB8 File Offset: 0x000C6CB8
		public int NumChanged
		{
			get
			{
				return this.numChanged;
			}
			set
			{
				this.numChanged = value;
				this.OnPropertyChanged("NumChanged");
			}
		}

		// Token: 0x06001E97 RID: 7831 RVA: 0x000C8ACE File Offset: 0x000C6CCE
		public ChkPointInfo(int chkIndex, DateTime date, string username)
		{
			this.ChkIndex = chkIndex;
			this.Date = date;
			this.Username = username;
		}

		// Token: 0x06001E98 RID: 7832 RVA: 0x000C8AF0 File Offset: 0x000C6CF0
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x040013E1 RID: 5089
		private bool available;

		// Token: 0x040013E2 RID: 5090
		private DateTime date;

		// Token: 0x040013E3 RID: 5091
		private string username;

		// Token: 0x040013E4 RID: 5092
		private string name;

		// Token: 0x040013E5 RID: 5093
		private bool isChanged;

		// Token: 0x040013E6 RID: 5094
		private bool isSelected;

		// Token: 0x040013E7 RID: 5095
		private int numChanged;
	}
}
