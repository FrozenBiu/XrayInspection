using System;
using Common;

namespace GUI_XRS
{
	// Token: 0x020000BC RID: 188
	public class KeyboardChar : ViewModelBase
	{
		// Token: 0x17000591 RID: 1425
		// (get) Token: 0x06001275 RID: 4725 RVA: 0x0006F5E4 File Offset: 0x0006D7E4
		// (set) Token: 0x06001276 RID: 4726 RVA: 0x0006F5FC File Offset: 0x0006D7FC
		public string CurrentChar
		{
			get
			{
				return this.currentChar;
			}
			set
			{
				this.SetProperty<string>(ref this.currentChar, value, "CurrentChar");
			}
		}

		// Token: 0x17000592 RID: 1426
		// (get) Token: 0x06001277 RID: 4727 RVA: 0x0006F614 File Offset: 0x0006D814
		// (set) Token: 0x06001278 RID: 4728 RVA: 0x0006F62C File Offset: 0x0006D82C
		public bool IsUpperCase
		{
			get
			{
				return this.isUpperCase;
			}
			set
			{
				bool flag = this.SetProperty<bool>(ref this.isUpperCase, value, "IsUpperCase");
				if (flag)
				{
					this.SetCurrentChar();
				}
			}
		}

		// Token: 0x17000593 RID: 1427
		// (get) Token: 0x06001279 RID: 4729 RVA: 0x0006F658 File Offset: 0x0006D858
		// (set) Token: 0x0600127A RID: 4730 RVA: 0x0006F670 File Offset: 0x0006D870
		public bool SymbolMode
		{
			get
			{
				return this.symbolMode;
			}
			set
			{
				bool flag = this.SetProperty<bool>(ref this.symbolMode, value, "SymbolMode");
				if (flag)
				{
					this.SetCurrentChar();
				}
			}
		}

		// Token: 0x0600127B RID: 4731 RVA: 0x0006F69B File Offset: 0x0006D89B
		public KeyboardChar() : this("", "", "")
		{
		}

		// Token: 0x0600127C RID: 4732 RVA: 0x0006F6B4 File Offset: 0x0006D8B4
		public KeyboardChar(char lowerCase, char upperCase) : this(lowerCase.ToString(), upperCase.ToString(), "")
		{
		}

		// Token: 0x0600127D RID: 4733 RVA: 0x0006F6D1 File Offset: 0x0006D8D1
		public KeyboardChar(string lowerCase, string upperCase, string symbol)
		{
			this.lowerCase = lowerCase;
			this.upperCase = upperCase;
			this.symbol = symbol;
			this.isUpperCase = true;
			this.currentChar = upperCase;
		}

		// Token: 0x0600127E RID: 4734 RVA: 0x0006F700 File Offset: 0x0006D900
		private void SetCurrentChar()
		{
			bool flag = this.symbolMode;
			if (flag)
			{
				this.CurrentChar = this.symbol;
			}
			else
			{
				this.CurrentChar = (this.isUpperCase ? this.upperCase : this.lowerCase);
			}
		}

		// Token: 0x04000C3B RID: 3131
		private bool isUpperCase;

		// Token: 0x04000C3C RID: 3132
		private bool symbolMode;

		// Token: 0x04000C3D RID: 3133
		private string lowerCase;

		// Token: 0x04000C3E RID: 3134
		private string upperCase;

		// Token: 0x04000C3F RID: 3135
		private string symbol;

		// Token: 0x04000C40 RID: 3136
		private string currentChar;
	}
}
