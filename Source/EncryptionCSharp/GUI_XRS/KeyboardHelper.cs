using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Input;

namespace GUI_XRS
{
	// Token: 0x020000BD RID: 189
	public class KeyboardHelper
	{
		// Token: 0x0600127F RID: 4735
		[DllImport("user32.dll")]
		public static extern int ToUnicode(uint wVirtKey, uint wScanCode, byte[] lpKeyState, [MarshalAs(UnmanagedType.LPWStr)] [Out] StringBuilder pwszBuff, int cchBuff, uint wFlags);

		// Token: 0x06001280 RID: 4736
		[DllImport("user32.dll")]
		public static extern bool GetKeyboardState(byte[] lpKeyState);

		// Token: 0x06001281 RID: 4737
		[DllImport("user32.dll")]
		public static extern uint MapVirtualKey(uint uCode, KeyboardHelper.MapType uMapType);

		// Token: 0x06001282 RID: 4738 RVA: 0x0006F744 File Offset: 0x0006D944
		public static char GetCharFromKey(Key key)
		{
			char result = ' ';
			int num = KeyInterop.VirtualKeyFromKey(key);
			byte[] lpKeyState = new byte[256];
			KeyboardHelper.GetKeyboardState(lpKeyState);
			uint wScanCode = KeyboardHelper.MapVirtualKey((uint)num, KeyboardHelper.MapType.MAPVK_VK_TO_VSC);
			StringBuilder stringBuilder = new StringBuilder(2);
			switch (KeyboardHelper.ToUnicode((uint)num, wScanCode, lpKeyState, stringBuilder, stringBuilder.Capacity, 0U))
			{
			case -1:
				break;
			case 0:
				break;
			case 1:
				result = stringBuilder[0];
				break;
			default:
				result = stringBuilder[0];
				break;
			}
			return result;
		}

		// Token: 0x020002D1 RID: 721
		public enum MapType : uint
		{
			// Token: 0x04001FF7 RID: 8183
			MAPVK_VK_TO_VSC,
			// Token: 0x04001FF8 RID: 8184
			MAPVK_VSC_TO_VK,
			// Token: 0x04001FF9 RID: 8185
			MAPVK_VK_TO_CHAR,
			// Token: 0x04001FFA RID: 8186
			MAPVK_VSC_TO_VK_EX
		}
	}
}
