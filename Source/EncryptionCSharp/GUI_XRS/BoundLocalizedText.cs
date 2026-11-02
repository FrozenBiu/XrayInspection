using System;
using System.ComponentModel;
using System.Diagnostics;

namespace GUI_XRS
{
	// Token: 0x0200021F RID: 543
	public class BoundLocalizedText : IBoundLocalizedText, INotifyPropertyChanged
	{
		// Token: 0x140000D6 RID: 214
		// (add) Token: 0x060028EA RID: 10474 RVA: 0x00110B40 File Offset: 0x0010ED40
		// (remove) Token: 0x060028EB RID: 10475 RVA: 0x00110B78 File Offset: 0x0010ED78
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x17000B29 RID: 2857
		// (get) Token: 0x060028EC RID: 10476 RVA: 0x00110BB0 File Offset: 0x0010EDB0
		// (set) Token: 0x060028ED RID: 10477 RVA: 0x00110BC8 File Offset: 0x0010EDC8
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

		// Token: 0x17000B2A RID: 2858
		// (get) Token: 0x060028EE RID: 10478 RVA: 0x00110BE0 File Offset: 0x0010EDE0
		// (set) Token: 0x060028EF RID: 10479 RVA: 0x00110BF8 File Offset: 0x0010EDF8
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

		// Token: 0x060028F0 RID: 10480 RVA: 0x00110C0E File Offset: 0x0010EE0E
		public BoundLocalizedText() : this("")
		{
		}

		// Token: 0x060028F1 RID: 10481 RVA: 0x00110C1D File Offset: 0x0010EE1D
		public BoundLocalizedText(string resourceKey)
		{
			this.ResourceKey = resourceKey;
			MainWindow.localizationManager.Register(this);
		}

		// Token: 0x060028F2 RID: 10482 RVA: 0x00110C3C File Offset: 0x0010EE3C
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x04001980 RID: 6528
		private string localizedText;

		// Token: 0x04001981 RID: 6529
		private string resourceKey;
	}
}
