using System;
using System.ComponentModel;
using System.Diagnostics;
using DatabaseConnection;

namespace GUI_XRS
{
	// Token: 0x02000076 RID: 118
	public class Infos : INotifyPropertyChanged
	{
		// Token: 0x1400003D RID: 61
		// (add) Token: 0x06000AF8 RID: 2808 RVA: 0x00040C84 File Offset: 0x0003EE84
		// (remove) Token: 0x06000AF9 RID: 2809 RVA: 0x00040CBC File Offset: 0x0003EEBC
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x06000AFA RID: 2810 RVA: 0x00040CF1 File Offset: 0x0003EEF1
		public Infos(DBConnection dbConnection)
		{
			this.dbConnection = dbConnection;
			this.DBPath = "";
		}

		// Token: 0x17000338 RID: 824
		// (get) Token: 0x06000AFB RID: 2811 RVA: 0x00040D0E File Offset: 0x0003EF0E
		// (set) Token: 0x06000AFC RID: 2812 RVA: 0x00040D16 File Offset: 0x0003EF16
		public string DBPath { get; set; }

		// Token: 0x17000339 RID: 825
		// (get) Token: 0x06000AFD RID: 2813 RVA: 0x00040D1F File Offset: 0x0003EF1F
		// (set) Token: 0x06000AFE RID: 2814 RVA: 0x00040D27 File Offset: 0x0003EF27
		public string Description { get; set; }

		// Token: 0x1700033A RID: 826
		// (get) Token: 0x06000AFF RID: 2815 RVA: 0x00040D30 File Offset: 0x0003EF30
		// (set) Token: 0x06000B00 RID: 2816 RVA: 0x00040D48 File Offset: 0x0003EF48
		public string Value
		{
			get
			{
				return this._value;
			}
			set
			{
				bool flag = value != this._value;
				if (flag)
				{
					this._value = value;
					bool flag2 = this.DBPath != "";
					if (flag2)
					{
						this.dbConnection.SetIVPInfoEntry(this.DBPath, this._value);
					}
					this.OnPropertyChanged("Value");
				}
			}
		}

		// Token: 0x06000B01 RID: 2817 RVA: 0x00040DA8 File Offset: 0x0003EFA8
		protected void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x04000752 RID: 1874
		private DBConnection dbConnection;

		// Token: 0x04000755 RID: 1877
		private string _value;
	}
}
