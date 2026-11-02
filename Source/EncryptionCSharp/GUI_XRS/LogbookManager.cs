using System;
using DatabaseConnection;

namespace GUI_XRS
{
	// Token: 0x0200011F RID: 287
	public static class LogbookManager
	{
		// Token: 0x17000826 RID: 2086
		// (get) Token: 0x06001BA6 RID: 7078 RVA: 0x000AB446 File Offset: 0x000A9646
		// (set) Token: 0x06001BA7 RID: 7079 RVA: 0x000AB44D File Offset: 0x000A964D
		public static Logbook logbook { private get; set; }

		// Token: 0x17000827 RID: 2087
		// (get) Token: 0x06001BA8 RID: 7080 RVA: 0x000AB455 File Offset: 0x000A9655
		// (set) Token: 0x06001BA9 RID: 7081 RVA: 0x000AB45C File Offset: 0x000A965C
		public static User currentUser { private get; set; }

		// Token: 0x06001BAA RID: 7082 RVA: 0x000AB464 File Offset: 0x000A9664
		public static SSTLogbookEntry AddEntry(SSTEntryCode code)
		{
			return LogbookManager.AddEntry(code, 0);
		}

		// Token: 0x06001BAB RID: 7083 RVA: 0x000AB480 File Offset: 0x000A9680
		public static SSTLogbookEntry AddEntry(SSTEntryCode code, byte param1)
		{
			return LogbookManager.AddEntry(code, param1, "");
		}

		// Token: 0x06001BAC RID: 7084 RVA: 0x000AB4A0 File Offset: 0x000A96A0
		public static SSTLogbookEntry AddEntry(SSTEntryCode code, byte param1, string param2)
		{
			return LogbookManager.AddEntry(code, param1, param2, "");
		}

		// Token: 0x06001BAD RID: 7085 RVA: 0x000AB4C0 File Offset: 0x000A96C0
		public static SSTLogbookEntry AddEntry(SSTEntryCode code, byte param1, string param2, string param3)
		{
			return LogbookManager.AddEntry(code, param1, param2, param3, "");
		}

		// Token: 0x06001BAE RID: 7086 RVA: 0x000AB4E0 File Offset: 0x000A96E0
		public static SSTLogbookEntry AddEntry(SSTEntryCode code, byte param1, string param2, string param3, string param4)
		{
			return LogbookManager.AddEntry(code, param1, param2, param3, param4, "");
		}

		// Token: 0x06001BAF RID: 7087 RVA: 0x000AB504 File Offset: 0x000A9704
		public static SSTLogbookEntry AddEntry(SSTEntryCode code, byte param1, string param2, string param3, string param4, string param5)
		{
			return LogbookManager.AddEntry(code, param1, param2, param3, param4, param5, "");
		}

		// Token: 0x06001BB0 RID: 7088 RVA: 0x000AB528 File Offset: 0x000A9728
		public static SSTLogbookEntry AddEntry(SSTEntryCode code, byte param1, string param2, string param3, string param4, string param5, string param6)
		{
			return LogbookManager.AddEntry(DateTime.Now, code, param1, param2, param3, param4, param5, param6);
		}

		// Token: 0x06001BB1 RID: 7089 RVA: 0x000AB550 File Offset: 0x000A9750
		public static SSTLogbookEntry AddEntry(DateTime time, SSTEntryCode code, byte param1, string param2, string param3, string param4, string param5, string param6)
		{
			bool flag = LogbookManager.logbook != null && LogbookManager.currentUser != null;
			SSTLogbookEntry result;
			if (flag)
			{
				LogbookManager.newLogbookEntry = true;
				result = LogbookManager.logbook.AddEntry(time, LogbookManager.currentUser.Username, code, param1, param2, param3, param4, param5, param6);
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x04001273 RID: 4723
		public static bool newLogbookEntry;
	}
}
