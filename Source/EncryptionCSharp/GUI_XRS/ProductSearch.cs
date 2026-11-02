using System;
using System.Windows.Data;

namespace GUI_XRS
{
	// Token: 0x020000C9 RID: 201
	public class ProductSearch : TextSearch
	{
		// Token: 0x0600142D RID: 5165 RVA: 0x00077171 File Offset: 0x00075371
		public ProductSearch(ListCollectionView viewCollection) : base(viewCollection)
		{
		}

		// Token: 0x0600142E RID: 5166 RVA: 0x0007717C File Offset: 0x0007537C
		public override void FilterList(string searchString)
		{
			this.viewCollection.Filter = ((object product) => ((ProductData)product).ProductName.StartsWith(searchString, StringComparison.OrdinalIgnoreCase) || ((ProductData)product).ProductNumber.ToString().StartsWith(searchString, StringComparison.OrdinalIgnoreCase));
		}
	}
}
