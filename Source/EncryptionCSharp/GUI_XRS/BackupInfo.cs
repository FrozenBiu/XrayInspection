using System;
using System.ComponentModel;
using System.Diagnostics;

namespace GUI_XRS
{
	// Token: 0x02000174 RID: 372
	public class BackupInfo : INotifyPropertyChanged
	{
		// Token: 0x140000B5 RID: 181
		// (add) Token: 0x0600222E RID: 8750 RVA: 0x000DD614 File Offset: 0x000DB814
		// (remove) Token: 0x0600222F RID: 8751 RVA: 0x000DD64C File Offset: 0x000DB84C
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x170009DD RID: 2525
		// (get) Token: 0x06002230 RID: 8752 RVA: 0x000DD684 File Offset: 0x000DB884
		// (set) Token: 0x06002231 RID: 8753 RVA: 0x000DD69C File Offset: 0x000DB89C
		public string InfoText
		{
			get
			{
				return this.infoText;
			}
			set
			{
				this.infoText = value;
				this.OnPropertyChanged("InfoText");
			}
		}

		// Token: 0x170009DE RID: 2526
		// (get) Token: 0x06002232 RID: 8754 RVA: 0x000DD6B4 File Offset: 0x000DB8B4
		// (set) Token: 0x06002233 RID: 8755 RVA: 0x000DD6CC File Offset: 0x000DB8CC
		public string InfoValue
		{
			get
			{
				return this.infoValue;
			}
			set
			{
				this.infoValue = value;
				this.OnPropertyChanged("InfoValue");
			}
		}

		// Token: 0x06002234 RID: 8756 RVA: 0x000DD6E2 File Offset: 0x000DB8E2
		public BackupInfo(string infoText, string infoValue)
		{
			this.InfoText = infoText;
			this.InfoValue = infoValue;
		}

		// Token: 0x06002235 RID: 8757 RVA: 0x000DD6FC File Offset: 0x000DB8FC
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x0400163D RID: 5693
		private string infoValue;

		// Token: 0x0400163E RID: 5694
		private string infoText;
	}
}
