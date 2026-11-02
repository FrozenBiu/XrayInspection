using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using GUI_XRS.Properties;

namespace GUI_XRS
{
	// Token: 0x02000224 RID: 548
	public class LocalizationManager
	{
		// Token: 0x140000D8 RID: 216
		// (add) Token: 0x06002911 RID: 10513 RVA: 0x001112C4 File Offset: 0x0010F4C4
		// (remove) Token: 0x06002912 RID: 10514 RVA: 0x001112FC File Offset: 0x0010F4FC
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action LanguageChanged;

		// Token: 0x17000B34 RID: 2868
		// (get) Token: 0x06002913 RID: 10515 RVA: 0x00111334 File Offset: 0x0010F534
		// (set) Token: 0x06002914 RID: 10516 RVA: 0x0011134B File Offset: 0x0010F54B
		public static CultureInfo CurrentCulture
		{
			get
			{
				return LocalizationManager.currentCulture;
			}
			set
			{
				LocalizationManager.currentCulture = value;
			}
		}

		// Token: 0x06002916 RID: 10518 RVA: 0x0011136C File Offset: 0x0010F56C
		protected virtual void OnLanguageChanged()
		{
			Action languageChanged = this.LanguageChanged;
			bool flag = languageChanged != null;
			if (flag)
			{
				languageChanged();
			}
		}

		// Token: 0x06002917 RID: 10519 RVA: 0x00111390 File Offset: 0x0010F590
		public void Register(IBoundLocalizedText element)
		{
			this.boundLocalizedText.Add(element);
			this.SetCurrentLanguage(element);
		}

		// Token: 0x06002918 RID: 10520 RVA: 0x001113A8 File Offset: 0x0010F5A8
		public bool Deregister(IBoundLocalizedText element)
		{
			return this.boundLocalizedText.Remove(element);
		}

		// Token: 0x06002919 RID: 10521 RVA: 0x001113C8 File Offset: 0x0010F5C8
		public void ChangeLanguage(CultureInfo newCulture)
		{
			foreach (IBoundLocalizedText boundLocalizedText in this.boundLocalizedText)
			{
				bool flag = boundLocalizedText != null && boundLocalizedText.ResourceKey != null;
				if (flag)
				{
					string @string = Resources.ResourceManager.GetString(boundLocalizedText.ResourceKey, newCulture);
					boundLocalizedText.LocalizedText = @string;
				}
			}
			LocalizationManager.CurrentCulture = newCulture;
			this.OnLanguageChanged();
		}

		// Token: 0x0600291A RID: 10522 RVA: 0x00111458 File Offset: 0x0010F658
		public void SetCurrentLanguage(IBoundLocalizedText element)
		{
			bool flag = element.ResourceKey != null;
			if (flag)
			{
				element.LocalizedText = Resources.ResourceManager.GetString(element.ResourceKey, LocalizationManager.CurrentCulture);
			}
		}

		// Token: 0x0600291B RID: 10523 RVA: 0x00111490 File Offset: 0x0010F690
		public static string Localize(string resourceKey)
		{
			return Resources.ResourceManager.GetString(resourceKey, LocalizationManager.CurrentCulture);
		}

		// Token: 0x04001990 RID: 6544
		private List<IBoundLocalizedText> boundLocalizedText = new List<IBoundLocalizedText>();

		// Token: 0x04001991 RID: 6545
		private static CultureInfo currentCulture = new CultureInfo("de-DE");
	}
}
