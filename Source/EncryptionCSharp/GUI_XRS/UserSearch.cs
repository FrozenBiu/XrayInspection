using System;
using System.Windows.Data;
using DatabaseConnection;

namespace GUI_XRS
{
	// Token: 0x020000EF RID: 239
	public class UserSearch : TextSearch
	{
		// Token: 0x06001783 RID: 6019 RVA: 0x00077171 File Offset: 0x00075371
		public UserSearch(ListCollectionView viewCollection) : base(viewCollection)
		{
		}

		// Token: 0x06001784 RID: 6020 RVA: 0x0008EEBC File Offset: 0x0008D0BC
		public override void FilterList(string searchString)
		{
			this.viewCollection.Filter = ((object user) => ((User)user).Username.StartsWith(searchString, StringComparison.OrdinalIgnoreCase) || ((User)user).ID.ToString().StartsWith(searchString, StringComparison.OrdinalIgnoreCase) || ((User)user).Userlevel.StartsWith(searchString, StringComparison.OrdinalIgnoreCase));
		}
	}
}
