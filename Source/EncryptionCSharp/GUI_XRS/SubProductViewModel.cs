using System;
using System.Windows.Data;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x0200013B RID: 315
	public class SubProductViewModel
	{
		// Token: 0x170008C2 RID: 2242
		// (get) Token: 0x06001E65 RID: 7781 RVA: 0x000C7FEC File Offset: 0x000C61EC
		// (set) Token: 0x06001E66 RID: 7782 RVA: 0x000C8004 File Offset: 0x000C6204
		public ProductManager ProductManager
		{
			get
			{
				return this.productManager;
			}
			set
			{
				this.productManager = value;
			}
		}

		// Token: 0x06001E67 RID: 7783 RVA: 0x000C8010 File Offset: 0x000C6210
		public SubProductViewModel(ProductManager productManager, XRSImage image)
		{
			this.productManager = productManager;
			this.image = image;
			this.viewProducts = (ListCollectionView)CollectionViewSource.GetDefaultView(this.ProductManager.SelectedProduct.SubProducts);
			this.viewProducts.CustomSort = new ProductDataComparer();
			bool flag = productManager.SelectedSubProduct == null && productManager.SelectedProduct.SubProducts.Count > 0;
			if (flag)
			{
				productManager.SelectedSubProduct = productManager.SelectedProduct.SubProducts[0];
			}
		}

		// Token: 0x06001E68 RID: 7784 RVA: 0x000C809E File Offset: 0x000C629E
		public void AddSubProduct()
		{
			this.ProductManager.AddSubProduct();
		}

		// Token: 0x06001E69 RID: 7785 RVA: 0x000C80AD File Offset: 0x000C62AD
		public void RemoveSubProduct()
		{
			this.ProductManager.RemoveSelectedSubProduct(false);
		}

		// Token: 0x06001E6A RID: 7786 RVA: 0x000C80C0 File Offset: 0x000C62C0
		public bool LearnSubProduct()
		{
			return this.ProductManager.LearnSubProduct();
		}

		// Token: 0x06001E6B RID: 7787 RVA: 0x000C80DD File Offset: 0x000C62DD
		public void OpenFilter()
		{
			this.ProductManager.OpenFilter();
		}

		// Token: 0x06001E6C RID: 7788 RVA: 0x000C80EC File Offset: 0x000C62EC
		public void RenameSubProduct()
		{
			this.productManager.RenameSelectedSubProduct();
		}

		// Token: 0x06001E6D RID: 7789 RVA: 0x000C80FB File Offset: 0x000C62FB
		public void RetrainGroup()
		{
			this.productManager.RelearnGroup(this.image);
		}

		// Token: 0x06001E6E RID: 7790 RVA: 0x000C8110 File Offset: 0x000C6310
		public void CopySubProduct()
		{
			this.productManager.CopySubProduct();
		}

		// Token: 0x06001E6F RID: 7791 RVA: 0x000C811F File Offset: 0x000C631F
		public void SetAsDefaultSubProduct()
		{
			this.productManager.SetAsDefaultSubProduct();
		}

		// Token: 0x040013D4 RID: 5076
		private ProductManager productManager;

		// Token: 0x040013D5 RID: 5077
		private XRSImage image;

		// Token: 0x040013D6 RID: 5078
		private ListCollectionView viewProducts;
	}
}
