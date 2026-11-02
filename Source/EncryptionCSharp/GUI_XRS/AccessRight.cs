using System;
using System.Windows;
using DatabaseConnection;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x02000189 RID: 393
	public class AccessRight
	{
		// Token: 0x17000A87 RID: 2695
		// (get) Token: 0x06002526 RID: 9510 RVA: 0x000FFF28 File Offset: 0x000FE128
		// (set) Token: 0x06002527 RID: 9511 RVA: 0x0001092C File Offset: 0x0000EB2C
		public UIElement Element
		{
			get
			{
				return this.element;
			}
			private set
			{
			}
		}

		// Token: 0x06002528 RID: 9512 RVA: 0x000FFF40 File Offset: 0x000FE140
		public AccessRight(UIElement element) : this(element, BuiltInPriviledges.Service, BuiltInPriviledges.Service)
		{
		}

		// Token: 0x06002529 RID: 9513 RVA: 0x000FFF55 File Offset: 0x000FE155
		public AccessRight(UIElement element, BuiltInPriviledges accessible, BuiltInPriviledges visible)
		{
			this.element = element;
			this.accessible = accessible;
			this.visible = visible;
		}

		// Token: 0x0600252A RID: 9514 RVA: 0x000FFF74 File Offset: 0x000FE174
		public void SetProperties(User currentUser)
		{
			bool flag = currentUser.Rights >= this.visible;
			if (flag)
			{
				this.element.Visibility = Visibility.Visible;
			}
			else
			{
				this.element.Visibility = Visibility.Collapsed;
			}
		}

		// Token: 0x040017E5 RID: 6117
		private UIElement element;

		// Token: 0x040017E6 RID: 6118
		private BuiltInPriviledges accessible;

		// Token: 0x040017E7 RID: 6119
		private BuiltInPriviledges visible;
	}
}
