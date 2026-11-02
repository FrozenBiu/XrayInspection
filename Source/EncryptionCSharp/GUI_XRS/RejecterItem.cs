using System;
using System.ComponentModel;
using System.Diagnostics;
using Common;

namespace GUI_XRS
{
	// Token: 0x020000CC RID: 204
	public class RejecterItem : IBoundLocalizedText, INotifyPropertyChanged
	{
		// Token: 0x14000066 RID: 102
		// (add) Token: 0x06001439 RID: 5177 RVA: 0x00077A9C File Offset: 0x00075C9C
		// (remove) Token: 0x0600143A RID: 5178 RVA: 0x00077AD4 File Offset: 0x00075CD4
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x1700062D RID: 1581
		// (get) Token: 0x0600143B RID: 5179 RVA: 0x00077B0C File Offset: 0x00075D0C
		// (set) Token: 0x0600143C RID: 5180 RVA: 0x00077B24 File Offset: 0x00075D24
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

		// Token: 0x1700062E RID: 1582
		// (get) Token: 0x0600143D RID: 5181 RVA: 0x00077B3C File Offset: 0x00075D3C
		// (set) Token: 0x0600143E RID: 5182 RVA: 0x00077B54 File Offset: 0x00075D54
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

		// Token: 0x1700062F RID: 1583
		// (get) Token: 0x0600143F RID: 5183 RVA: 0x00077B6A File Offset: 0x00075D6A
		// (set) Token: 0x06001440 RID: 5184 RVA: 0x00077B72 File Offset: 0x00075D72
		public RelayCommand ClickCmd { get; set; }

		// Token: 0x06001441 RID: 5185 RVA: 0x00077B7B File Offset: 0x00075D7B
		public RejecterItem() : this("", null, null)
		{
		}

		// Token: 0x06001442 RID: 5186 RVA: 0x00077B8C File Offset: 0x00075D8C
		public RejecterItem(string resourceKey, Action<object> clickHandler, ParameterHandling parHandling)
		{
			this.parHandling = parHandling;
			this.ResourceKey = resourceKey;
			this.ClickCmd = new RelayCommand(clickHandler);
		}

		// Token: 0x06001443 RID: 5187 RVA: 0x00077BB4 File Offset: 0x00075DB4
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x04000D69 RID: 3433
		private string localizedText;

		// Token: 0x04000D6A RID: 3434
		private string resourceKey;

		// Token: 0x04000D6B RID: 3435
		private ParameterHandling parHandling;
	}
}
