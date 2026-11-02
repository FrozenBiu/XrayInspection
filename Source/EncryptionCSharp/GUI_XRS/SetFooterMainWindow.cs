using System;
using System.Diagnostics;

namespace GUI_XRS
{
	// Token: 0x020000C3 RID: 195
	public class SetFooterMainWindow
	{
		// Token: 0x14000062 RID: 98
		// (add) Token: 0x060013CB RID: 5067 RVA: 0x00075A08 File Offset: 0x00073C08
		// (remove) Token: 0x060013CC RID: 5068 RVA: 0x00075A40 File Offset: 0x00073C40
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action PropertiesSetFooterDeactivation;

		// Token: 0x14000063 RID: 99
		// (add) Token: 0x060013CD RID: 5069 RVA: 0x00075A78 File Offset: 0x00073C78
		// (remove) Token: 0x060013CE RID: 5070 RVA: 0x00075AB0 File Offset: 0x00073CB0
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action PropertiesUnSetFooterDeactivation;

		// Token: 0x060013D0 RID: 5072 RVA: 0x00075AE5 File Offset: 0x00073CE5
		public void SetFooterDeactivation()
		{
			this.OnSetFooterDeactivation();
		}

		// Token: 0x060013D1 RID: 5073 RVA: 0x00075AF0 File Offset: 0x00073CF0
		protected virtual void OnSetFooterDeactivation()
		{
			Action propertiesSetFooterDeactivation = this.PropertiesSetFooterDeactivation;
			bool flag = propertiesSetFooterDeactivation != null;
			if (flag)
			{
				propertiesSetFooterDeactivation();
			}
		}

		// Token: 0x060013D2 RID: 5074 RVA: 0x00075B14 File Offset: 0x00073D14
		public void UnSetFooterDeactivation()
		{
			this.OnUnSetFooterDeactivation();
		}

		// Token: 0x060013D3 RID: 5075 RVA: 0x00075B20 File Offset: 0x00073D20
		protected virtual void OnUnSetFooterDeactivation()
		{
			Action propertiesUnSetFooterDeactivation = this.PropertiesUnSetFooterDeactivation;
			bool flag = propertiesUnSetFooterDeactivation != null;
			if (flag)
			{
				propertiesUnSetFooterDeactivation();
			}
		}
	}
}
