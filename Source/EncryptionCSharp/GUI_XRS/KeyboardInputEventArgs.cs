using System;

namespace GUI_XRS
{
	// Token: 0x02000244 RID: 580
	public class KeyboardInputEventArgs : EventArgs
	{
		// Token: 0x06002C12 RID: 11282 RVA: 0x001226F3 File Offset: 0x001208F3
		public KeyboardInputEventArgs(string text)
		{
			this.text = text;
		}

		// Token: 0x17000BF3 RID: 3059
		// (get) Token: 0x06002C13 RID: 11283 RVA: 0x00122704 File Offset: 0x00120904
		public string Text
		{
			get
			{
				return this.text;
			}
		}

		// Token: 0x04001B95 RID: 7061
		private readonly string text;
	}
}
