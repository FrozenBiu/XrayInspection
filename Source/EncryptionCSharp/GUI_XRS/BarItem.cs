using System;
using System.ComponentModel;
using System.Diagnostics;

namespace GUI_XRS
{
	// Token: 0x02000155 RID: 341
	public class BarItem : INotifyPropertyChanged
	{
		// Token: 0x140000AA RID: 170
		// (add) Token: 0x06002078 RID: 8312 RVA: 0x000D4640 File Offset: 0x000D2840
		// (remove) Token: 0x06002079 RID: 8313 RVA: 0x000D4678 File Offset: 0x000D2878
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x17000965 RID: 2405
		// (get) Token: 0x0600207A RID: 8314 RVA: 0x000D46B0 File Offset: 0x000D28B0
		// (set) Token: 0x0600207B RID: 8315 RVA: 0x000D46C8 File Offset: 0x000D28C8
		public double ItemHeight
		{
			get
			{
				return this.itemHeight;
			}
			set
			{
				this.itemHeight = value;
				this.OnPropertyChanged("ItemHeight");
			}
		}

		// Token: 0x17000966 RID: 2406
		// (get) Token: 0x0600207C RID: 8316 RVA: 0x000D46E0 File Offset: 0x000D28E0
		// (set) Token: 0x0600207D RID: 8317 RVA: 0x000D46F8 File Offset: 0x000D28F8
		public double Value
		{
			get
			{
				return this._value;
			}
			set
			{
				this._value = value;
				this.OnPropertyChanged("Value");
			}
		}

		// Token: 0x17000967 RID: 2407
		// (get) Token: 0x0600207E RID: 8318 RVA: 0x000D4710 File Offset: 0x000D2910
		// (set) Token: 0x0600207F RID: 8319 RVA: 0x000D4728 File Offset: 0x000D2928
		public bool IsFirstItem
		{
			get
			{
				return this.isFirstItem;
			}
			set
			{
				this.isFirstItem = value;
				this.OnPropertyChanged("IsFirstItem");
			}
		}

		// Token: 0x17000968 RID: 2408
		// (get) Token: 0x06002080 RID: 8320 RVA: 0x000D4740 File Offset: 0x000D2940
		// (set) Token: 0x06002081 RID: 8321 RVA: 0x000D4758 File Offset: 0x000D2958
		public bool IsLastItem
		{
			get
			{
				return this.isLastItem;
			}
			set
			{
				this.isLastItem = value;
				this.OnPropertyChanged("IsLastItem");
			}
		}

		// Token: 0x17000969 RID: 2409
		// (get) Token: 0x06002082 RID: 8322 RVA: 0x000D4770 File Offset: 0x000D2970
		// (set) Token: 0x06002083 RID: 8323 RVA: 0x000D4788 File Offset: 0x000D2988
		public bool IsMarker
		{
			get
			{
				return this.isMarker;
			}
			set
			{
				this.isMarker = value;
				this.OnPropertyChanged("IsMarker");
			}
		}

		// Token: 0x1700096A RID: 2410
		// (get) Token: 0x06002084 RID: 8324 RVA: 0x000D47A0 File Offset: 0x000D29A0
		// (set) Token: 0x06002085 RID: 8325 RVA: 0x000D47B8 File Offset: 0x000D29B8
		public bool IsShortMarker
		{
			get
			{
				return this.isShortMarker;
			}
			set
			{
				this.isShortMarker = value;
				this.OnPropertyChanged("IsShortMarker");
			}
		}

		// Token: 0x1700096B RID: 2411
		// (get) Token: 0x06002086 RID: 8326 RVA: 0x000D47D0 File Offset: 0x000D29D0
		// (set) Token: 0x06002087 RID: 8327 RVA: 0x000D47E8 File Offset: 0x000D29E8
		public bool Active
		{
			get
			{
				return this.active;
			}
			set
			{
				this.active = value;
				this.OnPropertyChanged("Active");
			}
		}

		// Token: 0x06002088 RID: 8328 RVA: 0x000D47FE File Offset: 0x000D29FE
		public BarItem(double value)
		{
			this._value = value;
		}

		// Token: 0x06002089 RID: 8329 RVA: 0x000D4810 File Offset: 0x000D2A10
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x0400151C RID: 5404
		private double itemHeight;

		// Token: 0x0400151D RID: 5405
		private double _value;

		// Token: 0x0400151E RID: 5406
		private bool isFirstItem;

		// Token: 0x0400151F RID: 5407
		private bool isLastItem;

		// Token: 0x04001520 RID: 5408
		private bool isMarker;

		// Token: 0x04001521 RID: 5409
		private bool isShortMarker;

		// Token: 0x04001522 RID: 5410
		private bool active;
	}
}
