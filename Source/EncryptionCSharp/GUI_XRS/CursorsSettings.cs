using System;
using System.Windows.Controls;
using System.Windows.Input;
using DatabaseConnection;

namespace GUI_XRS
{
	// Token: 0x0200023C RID: 572
	public class CursorsSettings
	{
		// Token: 0x17000BDE RID: 3038
		// (get) Token: 0x06002BB5 RID: 11189 RVA: 0x001213A5 File Offset: 0x0011F5A5
		// (set) Token: 0x06002BB6 RID: 11190 RVA: 0x001213AC File Offset: 0x0011F5AC
		public static DBConnection dbConnection { private get; set; }

		// Token: 0x06002BB7 RID: 11191 RVA: 0x001213B4 File Offset: 0x0011F5B4
		public static void SetCursor(Control o)
		{
			bool flag = true;
			bool flag2 = CursorsSettings.dbConnection != null && CursorsSettings.dbConnection.Connection != null;
			if (flag2)
			{
				bool flag3 = CursorsSettings.haveCursor == "";
				if (flag3)
				{
					CursorsSettings.dbConnection.GetHaveCursor(ref CursorsSettings.haveCursor);
				}
				bool flag4 = CursorsSettings.haveCursor == "1";
				if (flag4)
				{
					flag = false;
				}
			}
			bool flag5 = flag;
			if (flag5)
			{
				o.Cursor = Cursors.None;
				Mouse.OverrideCursor = Cursors.None;
			}
		}

		// Token: 0x04001B53 RID: 6995
		private static string haveCursor = "";
	}
}
