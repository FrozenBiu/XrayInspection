using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Media;
using Common;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x02000252 RID: 594
	public class SubMenuItem : IBoundLocalizedText, INotifyPropertyChanged
	{
		// Token: 0x17000C2E RID: 3118
		// (get) Token: 0x06002D5A RID: 11610 RVA: 0x00129424 File Offset: 0x00127624
		// (set) Token: 0x06002D5B RID: 11611 RVA: 0x0012943C File Offset: 0x0012763C
		public string ImagePath
		{
			get
			{
				return this.imagePath;
			}
			set
			{
				bool flag = this.imagePath != value;
				if (flag)
				{
					this.imagePath = value;
					this.OnPropertyChanged("ImagePath");
				}
			}
		}

		// Token: 0x17000C2F RID: 3119
		// (get) Token: 0x06002D5C RID: 11612 RVA: 0x00129470 File Offset: 0x00127670
		// (set) Token: 0x06002D5D RID: 11613 RVA: 0x00129488 File Offset: 0x00127688
		public string ImageDisabledPath
		{
			get
			{
				return this.imageDisabledPath;
			}
			set
			{
				bool flag = this.imageDisabledPath != value;
				if (flag)
				{
					this.imageDisabledPath = value;
					this.OnPropertyChanged("ImageDisabledPath");
				}
			}
		}

		// Token: 0x17000C30 RID: 3120
		// (get) Token: 0x06002D5E RID: 11614 RVA: 0x001294BB File Offset: 0x001276BB
		// (set) Token: 0x06002D5F RID: 11615 RVA: 0x001294C3 File Offset: 0x001276C3
		public RelayCommand CommandClickHandler { get; set; }

		// Token: 0x17000C31 RID: 3121
		// (get) Token: 0x06002D60 RID: 11616 RVA: 0x001294CC File Offset: 0x001276CC
		// (set) Token: 0x06002D61 RID: 11617 RVA: 0x001294D4 File Offset: 0x001276D4
		public object CommandParameter { get; set; }

		// Token: 0x17000C32 RID: 3122
		// (get) Token: 0x06002D62 RID: 11618 RVA: 0x001294DD File Offset: 0x001276DD
		// (set) Token: 0x06002D63 RID: 11619 RVA: 0x001294E5 File Offset: 0x001276E5
		public bool Visible { get; set; }

		// Token: 0x17000C33 RID: 3123
		// (get) Token: 0x06002D64 RID: 11620 RVA: 0x001294EE File Offset: 0x001276EE
		// (set) Token: 0x06002D65 RID: 11621 RVA: 0x001294F6 File Offset: 0x001276F6
		public bool Accessible { get; set; }

		// Token: 0x14000103 RID: 259
		// (add) Token: 0x06002D66 RID: 11622 RVA: 0x00129500 File Offset: 0x00127700
		// (remove) Token: 0x06002D67 RID: 11623 RVA: 0x00129538 File Offset: 0x00127738
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x17000C34 RID: 3124
		// (get) Token: 0x06002D68 RID: 11624 RVA: 0x00129570 File Offset: 0x00127770
		// (set) Token: 0x06002D69 RID: 11625 RVA: 0x00129588 File Offset: 0x00127788
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

		// Token: 0x17000C35 RID: 3125
		// (get) Token: 0x06002D6A RID: 11626 RVA: 0x0012959E File Offset: 0x0012779E
		// (set) Token: 0x06002D6B RID: 11627 RVA: 0x001295A6 File Offset: 0x001277A6
		public string ResourceKey { get; set; }

		// Token: 0x17000C36 RID: 3126
		// (get) Token: 0x06002D6C RID: 11628 RVA: 0x001295B0 File Offset: 0x001277B0
		// (set) Token: 0x06002D6D RID: 11629 RVA: 0x001295C8 File Offset: 0x001277C8
		public double ImageHeight
		{
			get
			{
				return this.imageHeight;
			}
			set
			{
				this.imageHeight = value;
				this.OnPropertyChanged("ImageHeight");
			}
		}

		// Token: 0x17000C37 RID: 3127
		// (get) Token: 0x06002D6E RID: 11630 RVA: 0x001295E0 File Offset: 0x001277E0
		// (set) Token: 0x06002D6F RID: 11631 RVA: 0x001295F8 File Offset: 0x001277F8
		public Stretch Stretch
		{
			get
			{
				return this.stretch;
			}
			set
			{
				this.stretch = value;
				this.OnPropertyChanged("Stretch");
			}
		}

		// Token: 0x06002D70 RID: 11632 RVA: 0x00129610 File Offset: 0x00127810
		public SubMenuItem(string localizedText, string resourceKey, string imagePath, Action<object> ClickHandler, AccessRightsManager accessManager, BuiltInPriviledges visible, BuiltInPriviledges accessible)
		{
			this.Visible = (visible <= accessManager.CurrentUser.Rights);
			this.Accessible = (visible <= accessManager.CurrentUser.Rights);
			this.Initialize(localizedText, resourceKey, imagePath, ClickHandler, accessManager, this.Visible, this.Accessible, null);
		}

		// Token: 0x06002D71 RID: 11633 RVA: 0x00129674 File Offset: 0x00127874
		public SubMenuItem(string localizedText, string resourceKey, string imagePath, string imageDisabledPath, Action<object> ClickHandler, AccessRightsManager accessManager, BuiltInPriviledges visible, BuiltInPriviledges accessible)
		{
			this.Visible = (visible <= accessManager.CurrentUser.Rights);
			this.Accessible = (visible <= accessManager.CurrentUser.Rights);
			this.Initialize(localizedText, resourceKey, imagePath, ClickHandler, accessManager, this.Visible, this.Accessible, null);
			this.imageDisabledPath = imageDisabledPath;
		}

		// Token: 0x06002D72 RID: 11634 RVA: 0x001296E0 File Offset: 0x001278E0
		public SubMenuItem(string localizedText, string resourceKey, string ImagePath, Action<object> ClickHandler, AccessRightsManager accessManager, bool visible, bool accessible)
		{
			this.Initialize(localizedText, resourceKey, ImagePath, ClickHandler, accessManager, visible, accessible, null);
		}

		// Token: 0x06002D73 RID: 11635 RVA: 0x00129708 File Offset: 0x00127908
		public SubMenuItem(string localizedText, string resourceKey, string ImagePath, Action<object> ClickHandler, AccessRightsManager accessManager, BuiltInPriviledges visible, BuiltInPriviledges accessible, object commandParameter)
		{
			this.Visible = (visible <= accessManager.CurrentUser.Rights);
			this.Accessible = (visible <= accessManager.CurrentUser.Rights);
			this.Initialize(localizedText, resourceKey, ImagePath, ClickHandler, accessManager, this.Visible, this.Accessible, commandParameter);
		}

		// Token: 0x06002D74 RID: 11636 RVA: 0x00129770 File Offset: 0x00127970
		public SubMenuItem(string localizedText, string resourceKey, string ImagePath, Action<object> ClickHandler, AccessRightsManager accessManager, bool visible, bool accessible, object commandParameter)
		{
			this.Initialize(localizedText, resourceKey, ImagePath, ClickHandler, accessManager, visible, accessible, commandParameter);
		}

		// Token: 0x06002D75 RID: 11637 RVA: 0x0012979C File Offset: 0x0012799C
		private void Initialize(string defaultText, string resourceKey, string ImagePath, Action<object> ClickHandler, AccessRightsManager accessManager, bool visible, bool accessible, object commandParameter)
		{
			this.ResourceKey = resourceKey;
			this.ImagePath = ImagePath;
			this.CommandClickHandler = new RelayCommand(ClickHandler);
			this.CommandParameter = commandParameter;
			this.Visible = visible;
			this.Accessible = accessible;
			MainWindow.localizationManager.Register(this);
			bool flag = this.LocalizedText == null;
			if (flag)
			{
				this.LocalizedText = defaultText;
			}
			this.imageHeight = 55.0;
			this.stretch = Stretch.None;
			this.imageDisabledPath = "..\\..\\img\\transparent.png";
		}

		// Token: 0x06002D76 RID: 11638 RVA: 0x00129828 File Offset: 0x00127A28
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x06002D77 RID: 11639 RVA: 0x00129855 File Offset: 0x00127A55
		public void DeregisterLocalizedText()
		{
			MainWindow.localizationManager.Deregister(this);
		}

		// Token: 0x04001C5B RID: 7259
		private string localizedText;

		// Token: 0x04001C5C RID: 7260
		private string imagePath;

		// Token: 0x04001C5D RID: 7261
		private string imageDisabledPath;

		// Token: 0x04001C5E RID: 7262
		private double imageHeight;

		// Token: 0x04001C5F RID: 7263
		private Stretch stretch;
	}
}
