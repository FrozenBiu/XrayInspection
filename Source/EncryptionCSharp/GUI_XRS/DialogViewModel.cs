using System;
using System.Windows;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x02000242 RID: 578
	internal class DialogViewModel
	{
		// Token: 0x17000BE5 RID: 3045
		// (get) Token: 0x06002BE5 RID: 11237 RVA: 0x00121DD4 File Offset: 0x0011FFD4
		// (set) Token: 0x06002BE6 RID: 11238 RVA: 0x00121DEC File Offset: 0x0011FFEC
		public string Title
		{
			get
			{
				return this.___Title;
			}
			private set
			{
				bool flag = this.___Title != value;
				if (flag)
				{
					this.___Title = value;
				}
			}
		}

		// Token: 0x17000BE6 RID: 3046
		// (get) Token: 0x06002BE7 RID: 11239 RVA: 0x00121E14 File Offset: 0x00120014
		// (set) Token: 0x06002BE8 RID: 11240 RVA: 0x00121E2C File Offset: 0x0012002C
		public string Message
		{
			get
			{
				return this.___Message;
			}
			private set
			{
				bool flag = this.___Message != value;
				if (flag)
				{
					this.___Message = value;
				}
			}
		}

		// Token: 0x17000BE7 RID: 3047
		// (get) Token: 0x06002BE9 RID: 11241 RVA: 0x00121E54 File Offset: 0x00120054
		// (set) Token: 0x06002BEA RID: 11242 RVA: 0x00121E6C File Offset: 0x0012006C
		public string StyleResourceKey
		{
			get
			{
				return this.___StyleResourceKey;
			}
			private set
			{
				bool flag = this.___StyleResourceKey != value;
				if (flag)
				{
					this.___StyleResourceKey = value;
				}
			}
		}

		// Token: 0x17000BE8 RID: 3048
		// (get) Token: 0x06002BEB RID: 11243 RVA: 0x00121E94 File Offset: 0x00120094
		// (set) Token: 0x06002BEC RID: 11244 RVA: 0x00121EAC File Offset: 0x001200AC
		public Visibility OkVisibility
		{
			get
			{
				return this.___OkVisibility;
			}
			private set
			{
				bool flag = this.___OkVisibility != value;
				if (flag)
				{
					this.___OkVisibility = value;
				}
			}
		}

		// Token: 0x17000BE9 RID: 3049
		// (get) Token: 0x06002BED RID: 11245 RVA: 0x00121ED4 File Offset: 0x001200D4
		// (set) Token: 0x06002BEE RID: 11246 RVA: 0x00121EEC File Offset: 0x001200EC
		public Visibility OkCancelVisibility
		{
			get
			{
				return this.___OkCancelVisibility;
			}
			private set
			{
				bool flag = this.___OkCancelVisibility != value;
				if (flag)
				{
					this.___OkCancelVisibility = value;
				}
			}
		}

		// Token: 0x17000BEA RID: 3050
		// (get) Token: 0x06002BEF RID: 11247 RVA: 0x00121F14 File Offset: 0x00120114
		// (set) Token: 0x06002BF0 RID: 11248 RVA: 0x00121F2C File Offset: 0x0012012C
		public Visibility RenameVisibility
		{
			get
			{
				return this.___RenameVisibility;
			}
			private set
			{
				bool flag = this.___RenameVisibility != value;
				if (flag)
				{
					this.___RenameVisibility = value;
				}
			}
		}

		// Token: 0x17000BEB RID: 3051
		// (get) Token: 0x06002BF1 RID: 11249 RVA: 0x00121F53 File Offset: 0x00120153
		// (set) Token: 0x06002BF2 RID: 11250 RVA: 0x00121F5B File Offset: 0x0012015B
		public WindowState WState { get; set; }

		// Token: 0x17000BEC RID: 3052
		// (get) Token: 0x06002BF3 RID: 11251 RVA: 0x00121F64 File Offset: 0x00120164
		// (set) Token: 0x06002BF4 RID: 11252 RVA: 0x0003C95F File Offset: 0x0003AB5F
		public double WindowWidth
		{
			get
			{
				return Definition.windowWidth;
			}
			set
			{
				Definition.windowWidth = value;
			}
		}

		// Token: 0x17000BED RID: 3053
		// (get) Token: 0x06002BF5 RID: 11253 RVA: 0x00121F7C File Offset: 0x0012017C
		// (set) Token: 0x06002BF6 RID: 11254 RVA: 0x0003C97F File Offset: 0x0003AB7F
		public double WindowHeight
		{
			get
			{
				return Definition.windowHeight;
			}
			set
			{
				Definition.windowHeight = value;
			}
		}

		// Token: 0x17000BEE RID: 3054
		// (get) Token: 0x06002BF7 RID: 11255 RVA: 0x00121F94 File Offset: 0x00120194
		// (set) Token: 0x06002BF8 RID: 11256 RVA: 0x0003C99F File Offset: 0x0003AB9F
		public double WLeft
		{
			get
			{
				return Definition.windowLeft;
			}
			set
			{
				Definition.windowLeft = value;
			}
		}

		// Token: 0x17000BEF RID: 3055
		// (get) Token: 0x06002BF9 RID: 11257 RVA: 0x00121FAC File Offset: 0x001201AC
		// (set) Token: 0x06002BFA RID: 11258 RVA: 0x0003C9BF File Offset: 0x0003ABBF
		public double WTop
		{
			get
			{
				return Definition.windowTop;
			}
			set
			{
				Definition.windowTop = value;
			}
		}

		// Token: 0x06002BFB RID: 11259 RVA: 0x00121FC4 File Offset: 0x001201C4
		public DialogViewModel(Dialog view, string title, string message, DialogStyle styleOption, DialogButtons buttonOption)
		{
			this.WState = ((Definition.windowHeight > 0.0 && Definition.windowWidth > 0.0) ? WindowState.Normal : WindowState.Maximized);
			this.Title = title;
			this.Message = message;
			this.SetStyle(styleOption);
			this.SetButtonVisibility(buttonOption);
			this.___View = view;
		}

		// Token: 0x06002BFC RID: 11260 RVA: 0x00122030 File Offset: 0x00120230
		private void SetStyle(DialogStyle styleOption)
		{
			switch (styleOption)
			{
			case DialogStyle.Ok:
				this.StyleResourceKey = "OkStyle";
				break;
			case DialogStyle.Error:
				this.StyleResourceKey = "ErrorStyle";
				break;
			case DialogStyle.Warning:
				this.StyleResourceKey = "WarningStyle";
				break;
			case DialogStyle.Info:
				this.StyleResourceKey = "DefaultStyle";
				break;
			}
		}

		// Token: 0x06002BFD RID: 11261 RVA: 0x00122094 File Offset: 0x00120294
		private void SetButtonVisibility(DialogButtons buttonOption)
		{
			switch (buttonOption)
			{
			case DialogButtons.OKCancel:
				this.OkVisibility = Visibility.Collapsed;
				this.RenameVisibility = Visibility.Collapsed;
				break;
			case DialogButtons.OK:
				this.OkCancelVisibility = Visibility.Collapsed;
				this.RenameVisibility = Visibility.Collapsed;
				break;
			case DialogButtons.None:
				this.OkVisibility = Visibility.Collapsed;
				this.OkCancelVisibility = Visibility.Collapsed;
				this.RenameVisibility = Visibility.Collapsed;
				break;
			case DialogButtons.OkRenameCancel:
				this.OkVisibility = Visibility.Collapsed;
				break;
			}
		}

		// Token: 0x04001B83 RID: 7043
		private string ___Title;

		// Token: 0x04001B84 RID: 7044
		private string ___Message;

		// Token: 0x04001B85 RID: 7045
		private string ___StyleResourceKey;

		// Token: 0x04001B86 RID: 7046
		private Visibility ___OkVisibility;

		// Token: 0x04001B87 RID: 7047
		private Visibility ___OkCancelVisibility;

		// Token: 0x04001B88 RID: 7048
		private Visibility ___RenameVisibility;

		// Token: 0x04001B89 RID: 7049
		private Dialog ___View;
	}
}
