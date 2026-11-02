using System;
using System.CodeDom.Compiler;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace GUI_XRS.Properties
{
	// Token: 0x0200026F RID: 623
	[CompilerGenerated]
	[GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "17.14.0.0")]
	internal sealed partial class Settings : ApplicationSettingsBase
	{
		// Token: 0x170010D8 RID: 4312
		// (get) Token: 0x060033D6 RID: 13270 RVA: 0x00145ED8 File Offset: 0x001440D8
		public static Settings Default
		{
			get
			{
				return Settings.defaultInstance;
			}
		}

		// Token: 0x170010D9 RID: 4313
		// (get) Token: 0x060033D7 RID: 13271 RVA: 0x00145EF0 File Offset: 0x001440F0
		[ApplicationScopedSetting]
		[DebuggerNonUserCode]
		[SpecialSetting(SpecialSetting.ConnectionString)]
		[DefaultSettingValue("Data Source=.\\SQLEXPRESS;AttachDbFilename=|DataDirectory|\\XRS_Database.mdf;Integrated Security=True;User Instance=True")]
		public string XRS_DatabaseConnectionString
		{
			get
			{
				return (string)this["XRS_DatabaseConnectionString"];
			}
		}

		// Token: 0x170010DA RID: 4314
		// (get) Token: 0x060033D8 RID: 13272 RVA: 0x00145F14 File Offset: 0x00144114
		[ApplicationScopedSetting]
		[DebuggerNonUserCode]
		[SpecialSetting(SpecialSetting.ConnectionString)]
		[DefaultSettingValue("Data Source=LENOVO-91A3CB78\\SQLEXPRESS;AttachDbFilename=C:\\Projekte\\S+S\\SqlTestLINQ\\Test\\XRS_Database.mdf;Integrated Security=True")]
		public string XRS_DatabaseConnectionString1
		{
			get
			{
				return (string)this["XRS_DatabaseConnectionString1"];
			}
		}

		// Token: 0x04001E03 RID: 7683
		private static Settings defaultInstance = (Settings)SettingsBase.Synchronized(new Settings());
	}
}
