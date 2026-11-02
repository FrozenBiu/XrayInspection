using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using Common;

namespace GUI_XRS
{
	// Token: 0x020000EC RID: 236
	public abstract class TextSearch : INotifyPropertyChanged
	{
		// Token: 0x06001761 RID: 5985
		public abstract void FilterList(string searchString);

		// Token: 0x1700070F RID: 1807
		// (get) Token: 0x06001762 RID: 5986 RVA: 0x0008E2DC File Offset: 0x0008C4DC
		// (set) Token: 0x06001763 RID: 5987 RVA: 0x0008E2F4 File Offset: 0x0008C4F4
		public string SearchText
		{
			get
			{
				return this.searchText;
			}
			set
			{
				bool flag = this.searchText != value;
				if (flag)
				{
					this.searchText = ((value != "" && value != this.searchTextDefault) ? value : this.searchTextDefault);
					this.OnPropertyChanged("SearchText");
					this.FilterList(value);
					bool flag2 = this.searchText != this.searchTextDefault;
					if (flag2)
					{
						this.SearchTextBrush = new SolidColorBrush(Color.FromRgb(94, 94, 94));
						this.SearchTextAlignment = TextAlignment.Right;
					}
					else
					{
						this.SearchTextBrush = new SolidColorBrush(Color.FromRgb(170, 170, 170));
						this.SearchTextAlignment = TextAlignment.Left;
					}
				}
			}
		}

		// Token: 0x17000710 RID: 1808
		// (get) Token: 0x06001764 RID: 5988 RVA: 0x0008E3B8 File Offset: 0x0008C5B8
		// (set) Token: 0x06001765 RID: 5989 RVA: 0x0008E3D0 File Offset: 0x0008C5D0
		public TextAlignment SearchTextAlignment
		{
			get
			{
				return this.searchTextAlignment;
			}
			set
			{
				bool flag = this.searchTextAlignment != value;
				if (flag)
				{
					this.searchTextAlignment = value;
					this.OnPropertyChanged("SearchTextAlignment");
				}
			}
		}

		// Token: 0x17000711 RID: 1809
		// (get) Token: 0x06001766 RID: 5990 RVA: 0x0008E404 File Offset: 0x0008C604
		// (set) Token: 0x06001767 RID: 5991 RVA: 0x0008E41C File Offset: 0x0008C61C
		public SolidColorBrush SearchTextBrush
		{
			get
			{
				return this.searchTextBrush;
			}
			set
			{
				bool flag = this.searchTextBrush != value;
				if (flag)
				{
					this.searchTextBrush = value;
					bool flag2 = this.searchTextBrush != null;
					if (flag2)
					{
						this.searchTextBrush.Freeze();
					}
					this.OnPropertyChanged("SearchTextBrush");
				}
			}
		}

		// Token: 0x17000712 RID: 1810
		// (get) Token: 0x06001768 RID: 5992 RVA: 0x0008E468 File Offset: 0x0008C668
		// (set) Token: 0x06001769 RID: 5993 RVA: 0x0008E470 File Offset: 0x0008C670
		public RelayCommand SearchTextInListCommand { get; set; }

		// Token: 0x14000072 RID: 114
		// (add) Token: 0x0600176A RID: 5994 RVA: 0x0008E47C File Offset: 0x0008C67C
		// (remove) Token: 0x0600176B RID: 5995 RVA: 0x0008E4B4 File Offset: 0x0008C6B4
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x0600176C RID: 5996 RVA: 0x0008E4EC File Offset: 0x0008C6EC
		public TextSearch(ListCollectionView viewCollection)
		{
			this.viewCollection = viewCollection;
			this.SearchTextInListCommand = new RelayCommand(new Action<object>(this.SearchTextInList));
			this.searchText = this.searchTextDefault;
			this.searchTextBrush = new SolidColorBrush(Color.FromRgb(170, 170, 170));
			this.searchTextBrush.Freeze();
			this.searchTextAlignment = TextAlignment.Left;
		}

		// Token: 0x0600176D RID: 5997 RVA: 0x0008E578 File Offset: 0x0008C778
		public void SearchTextInList(object obj)
		{
			string oldValue = (this.searchText != this.searchTextDefault) ? this.searchText : "";
			this.keyboard = new Keyboard(this.searchTextDefault, oldValue, KeyboardType.Application);
			this.keyboard.TextInputKeyboard += this.keyboard_TextInputSearchPattern;
			this.keyboard.ShowDialog();
		}

		// Token: 0x0600176E RID: 5998 RVA: 0x0008E5DE File Offset: 0x0008C7DE
		private void keyboard_TextInputSearchPattern(object sender, KeyboardInputEventArgs e)
		{
			this.keyboard.TextInputKeyboard -= this.keyboard_TextInputSearchPattern;
			this.SearchText = e.Text;
		}

		// Token: 0x0600176F RID: 5999 RVA: 0x0008E608 File Offset: 0x0008C808
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x04000F64 RID: 3940
		public readonly string searchTextDefault = LocalizationManager.Localize("txtSearch") + "...";

		// Token: 0x04000F65 RID: 3941
		private Keyboard keyboard;

		// Token: 0x04000F66 RID: 3942
		private string searchText;

		// Token: 0x04000F67 RID: 3943
		private SolidColorBrush searchTextBrush;

		// Token: 0x04000F68 RID: 3944
		private TextAlignment searchTextAlignment;

		// Token: 0x04000F69 RID: 3945
		protected ListCollectionView viewCollection;
	}
}
