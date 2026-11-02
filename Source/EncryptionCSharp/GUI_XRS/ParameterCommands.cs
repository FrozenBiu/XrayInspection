using System;

namespace GUI_XRS
{
	// Token: 0x02000103 RID: 259
	internal enum ParameterCommands
	{
		// Token: 0x0400101A RID: 4122
		GetNumTotalEntries,
		// Token: 0x0400101B RID: 4123
		GetEntryAt,
		// Token: 0x0400101C RID: 4124
		GetNumSubEntriesFrom,
		// Token: 0x0400101D RID: 4125
		GetSubEntryFromAt,
		// Token: 0x0400101E RID: 4126
		ReadEntry = 10,
		// Token: 0x0400101F RID: 4127
		WriteEntry,
		// Token: 0x04001020 RID: 4128
		DisableWriteProtection = 20,
		// Token: 0x04001021 RID: 4129
		EnableWriteProtection,
		// Token: 0x04001022 RID: 4130
		Reboot
	}
}
