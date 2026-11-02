using System;
using System.Windows.Input;

namespace GUI_XRS
{
	// Token: 0x020000FD RID: 253
	public static class KeyChecker
	{
		// Token: 0x06001847 RID: 6215 RVA: 0x000921D8 File Offset: 0x000903D8
		public static bool IsKeyAnArrow(Key key)
		{
			return key >= Key.Left && key <= Key.Down;
		}

		// Token: 0x06001848 RID: 6216 RVA: 0x000921FC File Offset: 0x000903FC
		public static bool IsKeyAChar(Key key)
		{
			return key >= Key.A && key <= Key.Z;
		}

		// Token: 0x06001849 RID: 6217 RVA: 0x00092220 File Offset: 0x00090420
		public static bool IsKeyADigit(Key key)
		{
			return (key >= Key.D0 && key <= Key.D9) || (key >= Key.NumPad0 && key <= Key.NumPad9);
		}
	}
}
