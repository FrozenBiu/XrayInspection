using System;
using System.Windows;
using Definitions;

namespace GUI_XRS.AuditCheck
{
	// Token: 0x02000277 RID: 631
	internal class SignatureWindowViewModel
	{
		// Token: 0x17001119 RID: 4377
		// (get) Token: 0x060034AA RID: 13482 RVA: 0x0014F2E4 File Offset: 0x0014D4E4
		// (set) Token: 0x060034AB RID: 13483 RVA: 0x0014F2EC File Offset: 0x0014D4EC
		public WindowState WState { get; set; }

		// Token: 0x1700111A RID: 4378
		// (get) Token: 0x060034AC RID: 13484 RVA: 0x0014F2F8 File Offset: 0x0014D4F8
		// (set) Token: 0x060034AD RID: 13485 RVA: 0x0003C95F File Offset: 0x0003AB5F
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

		// Token: 0x1700111B RID: 4379
		// (get) Token: 0x060034AE RID: 13486 RVA: 0x0014F310 File Offset: 0x0014D510
		// (set) Token: 0x060034AF RID: 13487 RVA: 0x0003C97F File Offset: 0x0003AB7F
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

		// Token: 0x1700111C RID: 4380
		// (get) Token: 0x060034B0 RID: 13488 RVA: 0x0014F328 File Offset: 0x0014D528
		// (set) Token: 0x060034B1 RID: 13489 RVA: 0x0003C99F File Offset: 0x0003AB9F
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

		// Token: 0x1700111D RID: 4381
		// (get) Token: 0x060034B2 RID: 13490 RVA: 0x0014F340 File Offset: 0x0014D540
		// (set) Token: 0x060034B3 RID: 13491 RVA: 0x0003C9BF File Offset: 0x0003ABBF
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

		// Token: 0x060034B4 RID: 13492 RVA: 0x0014F357 File Offset: 0x0014D557
		public SignatureWindowViewModel()
		{
			this.WState = ((Definition.windowHeight > 0.0 && Definition.windowWidth > 0.0) ? WindowState.Normal : WindowState.Maximized);
		}
	}
}
