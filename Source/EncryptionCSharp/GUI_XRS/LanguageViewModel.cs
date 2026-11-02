using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Data;
using Common;
using DatabaseConnection;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x02000223 RID: 547
	public class LanguageViewModel : INotifyPropertyChanged
	{
		// Token: 0x140000D7 RID: 215
		// (add) Token: 0x06002901 RID: 10497 RVA: 0x00110D38 File Offset: 0x0010EF38
		// (remove) Token: 0x06002902 RID: 10498 RVA: 0x00110D70 File Offset: 0x0010EF70
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x17000B30 RID: 2864
		// (get) Token: 0x06002903 RID: 10499 RVA: 0x00110DA5 File Offset: 0x0010EFA5
		// (set) Token: 0x06002904 RID: 10500 RVA: 0x00110DAD File Offset: 0x0010EFAD
		public List<Language> Languages { get; set; }

		// Token: 0x17000B31 RID: 2865
		// (get) Token: 0x06002905 RID: 10501 RVA: 0x00110DB8 File Offset: 0x0010EFB8
		// (set) Token: 0x06002906 RID: 10502 RVA: 0x00110DD0 File Offset: 0x0010EFD0
		public Language SelectedLanguage
		{
			get
			{
				return this.selectedLanguage;
			}
			set
			{
				bool flag = this.selectedLanguage != value;
				if (flag)
				{
					this.selectedLanguage = value;
					this.OnPropertyChanged("SelectedLanguage");
				}
			}
		}

		// Token: 0x17000B32 RID: 2866
		// (get) Token: 0x06002907 RID: 10503 RVA: 0x00110E03 File Offset: 0x0010F003
		// (set) Token: 0x06002908 RID: 10504 RVA: 0x00110E0B File Offset: 0x0010F00B
		public BoundLocalizedText StrCurrentLanguage { get; set; }

		// Token: 0x17000B33 RID: 2867
		// (get) Token: 0x06002909 RID: 10505 RVA: 0x00110E14 File Offset: 0x0010F014
		// (set) Token: 0x0600290A RID: 10506 RVA: 0x00110E1C File Offset: 0x0010F01C
		public RelayCommand DeregisterLocalizedTextCommand { get; set; }

		// Token: 0x0600290B RID: 10507 RVA: 0x00110E28 File Offset: 0x0010F028
		public LanguageViewModel(DBConnection dbConnection)
		{
			this.dbConnection = dbConnection;
			this.StrCurrentLanguage = new BoundLocalizedText("txtCurrentLanguage");
			this.DeregisterLocalizedTextCommand = new RelayCommand(new Action<object>(this.DeregisterLocalizedText));
			this.AddLanguages();
			this.view = (ListCollectionView)CollectionViewSource.GetDefaultView(this.Languages);
			this.view.CurrentChanged += delegate(object <p0>, EventArgs <p1>)
			{
				this.SelectedLanguage = (Language)this.view.CurrentItem;
				MainWindow.localizationManager.ChangeLanguage(new CultureInfo(this.SelectedLanguage.CountryCode));
			};
			this.SelectCurrentLanguage();
		}

		// Token: 0x0600290C RID: 10508 RVA: 0x00110EAC File Offset: 0x0010F0AC
		private void AddLanguages()
		{
			this.Languages = new List<Language>();
			this.Languages.Add(new Language("/img/germany.png", "de-DE", Definitions.Languages.German));
			this.Languages.Add(new Language("/img/greatbritain.png", "en-GB", Definitions.Languages.English));
			this.Languages.Add(new Language("/img/usa.png", "en-US", Definitions.Languages.American));
			this.Languages.Add(new Language("/img/canadaEN.png", "en-CA", Definitions.Languages.Canadian));
			this.Languages.Add(new Language("/img/canadaFR.png", "fr-CA", Definitions.Languages.Canadian));
			this.Languages.Add(new Language("/img/french.png", "fr-FR", Definitions.Languages.French));
			this.Languages.Add(new Language("/img/czech.png", "cs-CZ", Definitions.Languages.Czech));
			this.Languages.Add(new Language("/img/poland.png", "pl-PL", Definitions.Languages.Polish));
			this.Languages.Add(new Language("/img/russland.png", "ru-RU", Definitions.Languages.Russian));
			this.Languages.Add(new Language("/img/portugal.png", "pt-PT", Definitions.Languages.Portugese));
			this.Languages.Add(new Language("/img/sweden.png", "se-SE", Definitions.Languages.Swedish));
			this.Languages.Add(new Language("/img/italy.png", "it-IT", Definitions.Languages.Italian));
			this.Languages.Add(new Language("/img/netherlands.png", "nl-NL", Definitions.Languages.Dutch));
			this.Languages.Add(new Language("/img/spain.png", "es-ES", Definitions.Languages.Spanish));
			this.Languages.Add(new Language("/img/chineseSimplified.png", "zh-CN", Definitions.Languages.Chinese));
			this.Languages.Add(new Language("/img/chineseTraditional.png", "zh-Tw", Definitions.Languages.Taiwanese));
			this.Languages.Add(new Language("/img/finland.png", "fi-FI", Definitions.Languages.Finland));
			this.Languages.Add(new Language("/img/thailand.png", "th-TH", Definitions.Languages.Thai));
			this.Languages.Add(new Language("/img/brazil.png", "pt-BR", Definitions.Languages.Brazil));
			this.Languages.Add(new Language("/img/hungary.png", "hu-HU", Definitions.Languages.Hungary));
			this.Languages.Add(new Language("/img/turkey.png", "tr-TR", Definitions.Languages.Turkish));
			this.Languages.Add(new Language("/img/japan.png", "ja-JP", Definitions.Languages.Japanese));
			this.Languages.Add(new Language("/img/danish.png", "da-DK", Definitions.Languages.Danish));
			this.Languages.Add(new Language("/img/slovak.png", "sk-SK", Definitions.Languages.Slovak));
		}

		// Token: 0x0600290D RID: 10509 RVA: 0x00111178 File Offset: 0x0010F378
		private void DeregisterLocalizedText(object obj)
		{
			MainWindow.localizationManager.Deregister(this.StrCurrentLanguage);
			for (int i = 0; i < this.Languages.Count; i++)
			{
				this.Languages[i].Flag.Freeze();
			}
		}

		// Token: 0x0600290E RID: 10510 RVA: 0x001111CC File Offset: 0x0010F3CC
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x0600290F RID: 10511 RVA: 0x001111FC File Offset: 0x0010F3FC
		private void SelectCurrentLanguage()
		{
			string text = "";
			this.dbConnection.GetLanguage(ref text);
			foreach (Language language in this.Languages)
			{
				bool flag = language.CountryCode.ToUpper() == text.ToUpper();
				if (flag)
				{
					this.view.MoveCurrentTo(language);
					break;
				}
			}
		}

		// Token: 0x04001989 RID: 6537
		private ListCollectionView view;

		// Token: 0x0400198A RID: 6538
		private Language selectedLanguage;

		// Token: 0x0400198B RID: 6539
		private DBConnection dbConnection;
	}
}
