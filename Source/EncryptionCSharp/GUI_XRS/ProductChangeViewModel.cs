using System;
using System.Collections.Generic;
using DatabaseConnection;

namespace GUI_XRS
{
	// Token: 0x0200024C RID: 588
	public class ProductChangeViewModel
	{
		// Token: 0x17000C03 RID: 3075
		// (get) Token: 0x06002C9F RID: 11423 RVA: 0x00125D41 File Offset: 0x00123F41
		// (set) Token: 0x06002CA0 RID: 11424 RVA: 0x00125D49 File Offset: 0x00123F49
		public ProductManager ProductManager { get; set; }

		// Token: 0x17000C04 RID: 3076
		// (get) Token: 0x06002CA1 RID: 11425 RVA: 0x00125D52 File Offset: 0x00123F52
		// (set) Token: 0x06002CA2 RID: 11426 RVA: 0x00125D5A File Offset: 0x00123F5A
		public TextSearch ProductSearch { get; set; }

		// Token: 0x17000C05 RID: 3077
		// (get) Token: 0x06002CA3 RID: 11427 RVA: 0x00125D63 File Offset: 0x00123F63
		// (set) Token: 0x06002CA4 RID: 11428 RVA: 0x00125D6B File Offset: 0x00123F6B
		public List<ProductData> SelectedProducts { get; set; }

		// Token: 0x06002CA5 RID: 11429 RVA: 0x00125D74 File Offset: 0x00123F74
		public ProductChangeViewModel(ProductManager productManager, DBConnection dbConnection)
		{
			this.ProductManager = productManager;
			this.dbConnection = dbConnection;
			productManager.SelectedProduct = productManager.CurrentLoadedProduct;
			productManager.ViewProducts.MoveCurrentTo(productManager.CurrentLoadedProduct);
			this.ProductSearch = new ProductSearch(productManager.ViewProducts);
		}

		// Token: 0x06002CA6 RID: 11430 RVA: 0x00125DCC File Offset: 0x00123FCC
		public void CreateNewProduct()
		{
			int maxProducts = 0;
			bool flag = !this.MaxProductsReached(ref maxProducts);
			if (flag)
			{
				this.ProductManager.CreateNewProduct();
			}
			else
			{
				this.ShowMaxProductsMessage(maxProducts);
			}
		}

		// Token: 0x06002CA7 RID: 11431 RVA: 0x00125E04 File Offset: 0x00124004
		public void CreateNewMultiProduct()
		{
			int maxProducts = 0;
			bool flag = !this.MaxProductsReached(ref maxProducts);
			if (flag)
			{
				this.ProductManager.CreateNewMultiProduct();
			}
			else
			{
				this.ShowMaxProductsMessage(maxProducts);
			}
		}

		// Token: 0x06002CA8 RID: 11432 RVA: 0x00125E3C File Offset: 0x0012403C
		public void CopyProduct()
		{
			int maxProducts = 0;
			bool flag = !this.MaxProductsReached(ref maxProducts);
			if (flag)
			{
				this.ProductManager.CopyProduct();
			}
			else
			{
				this.ShowMaxProductsMessage(maxProducts);
			}
		}

		// Token: 0x06002CA9 RID: 11433 RVA: 0x00125E71 File Offset: 0x00124071
		public void DeleteProduct()
		{
			this.ProductManager.DeleteProduct();
		}

		// Token: 0x06002CAA RID: 11434 RVA: 0x00125E80 File Offset: 0x00124080
		public void RenameProduct()
		{
			this.ProductManager.RenameProduct();
		}

		// Token: 0x06002CAB RID: 11435 RVA: 0x00125E90 File Offset: 0x00124090
		public bool ChangeProduct()
		{
			return this.ProductManager.ChangeProduct(false);
		}

		// Token: 0x06002CAC RID: 11436 RVA: 0x00125EB0 File Offset: 0x001240B0
		public void CreateNewLaneProduct()
		{
			int maxProducts = 0;
			bool flag = !this.MaxProductsReached(ref maxProducts);
			if (flag)
			{
				this.ProductManager.CreateNewLaneProduct();
			}
			else
			{
				this.ShowMaxProductsMessage(maxProducts);
			}
		}

		// Token: 0x06002CAD RID: 11437 RVA: 0x00125EE8 File Offset: 0x001240E8
		public void CreateNewRegionProduct()
		{
			int maxProducts = 0;
			bool flag = !this.MaxProductsReached(ref maxProducts);
			if (flag)
			{
				this.ProductManager.CreateNewRegionProduct();
			}
			else
			{
				this.ShowMaxProductsMessage(maxProducts);
			}
		}

		// Token: 0x06002CAE RID: 11438 RVA: 0x00125F20 File Offset: 0x00124120
		public void CreateNewBulkObjectProduct()
		{
			int maxProducts = 0;
			bool flag = !this.MaxProductsReached(ref maxProducts);
			if (flag)
			{
				this.ProductManager.CreateNewBulkObjectProduct();
			}
			else
			{
				this.ShowMaxProductsMessage(maxProducts);
			}
		}

		// Token: 0x06002CAF RID: 11439 RVA: 0x00125F55 File Offset: 0x00124155
		public void ResetProductSearch()
		{
			this.ProductSearch.FilterList("");
		}

		// Token: 0x06002CB0 RID: 11440 RVA: 0x00125F6C File Offset: 0x0012416C
		private bool MaxProductsReached(ref int maxProducts)
		{
			this.dbConnection.GetMaxProducts(ref maxProducts);
			return this.ProductManager.ProductList.Count >= maxProducts;
		}

		// Token: 0x06002CB1 RID: 11441 RVA: 0x00125FA4 File Offset: 0x001241A4
		private void ShowMaxProductsMessage(int maxProducts)
		{
			string message = string.Concat(new string[]
			{
				LocalizationManager.Localize("txtMaxProductsReached"),
				Environment.NewLine,
				LocalizationManager.Localize("txtMaxProducts"),
				": ",
				maxProducts.ToString()
			});
			Dialog.Show(LocalizationManager.Localize("txtError"), message, DialogStyle.Error, DialogButtons.OK);
		}

		// Token: 0x04001BF5 RID: 7157
		private DBConnection dbConnection;
	}
}
