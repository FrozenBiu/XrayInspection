using System;
using System.ComponentModel;
using System.Diagnostics;
using Common;

namespace GUI_XRS
{
	// Token: 0x0200019A RID: 410
	public class CounterItem : IBoundLocalizedText, INotifyPropertyChanged
	{
		// Token: 0x140000D2 RID: 210
		// (add) Token: 0x060026D8 RID: 9944 RVA: 0x00108348 File Offset: 0x00106548
		// (remove) Token: 0x060026D9 RID: 9945 RVA: 0x00108380 File Offset: 0x00106580
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x17000B03 RID: 2819
		// (get) Token: 0x060026DA RID: 9946 RVA: 0x001083B8 File Offset: 0x001065B8
		// (set) Token: 0x060026DB RID: 9947 RVA: 0x001083D0 File Offset: 0x001065D0
		public string LocalizedText
		{
			get
			{
				return this.localizedText;
			}
			set
			{
				this.localizedText = value;
				this.OnPropertyChanged("LocalizedText");
			}
		}

		// Token: 0x17000B04 RID: 2820
		// (get) Token: 0x060026DC RID: 9948 RVA: 0x001083E8 File Offset: 0x001065E8
		// (set) Token: 0x060026DD RID: 9949 RVA: 0x00108400 File Offset: 0x00106600
		public string ResourceKey
		{
			get
			{
				return this.resourceKey;
			}
			set
			{
				this.resourceKey = value;
				MainWindow.localizationManager.SetCurrentLanguage(this);
			}
		}

		// Token: 0x17000B05 RID: 2821
		// (get) Token: 0x060026DE RID: 9950 RVA: 0x00108416 File Offset: 0x00106616
		// (set) Token: 0x060026DF RID: 9951 RVA: 0x0010841E File Offset: 0x0010661E
		public RelayCommand ClickCommand { get; set; }

		// Token: 0x060026E0 RID: 9952 RVA: 0x00108427 File Offset: 0x00106627
		public CounterItem() : this("", null)
		{
		}

		// Token: 0x060026E1 RID: 9953 RVA: 0x00108437 File Offset: 0x00106637
		public CounterItem(string resourceKey, Action<object> clickHandler)
		{
			this.ResourceKey = resourceKey;
			this.ClickCommand = new RelayCommand(clickHandler);
		}

		// Token: 0x060026E2 RID: 9954 RVA: 0x00108458 File Offset: 0x00106658
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x040018F8 RID: 6392
		private string localizedText;

		// Token: 0x040018F9 RID: 6393
		private string resourceKey;
	}
}
