using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Data;
using System.Windows.Threading;
using DatabaseConnection;
using Definitions;
using GUI_XRS.Properties;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x0200013A RID: 314
	public class ProductManager : INotifyPropertyChanged
	{
		// Token: 0x170008BC RID: 2236
		// (get) Token: 0x06001DF3 RID: 7667 RVA: 0x000C2D6F File Offset: 0x000C0F6F
		// (set) Token: 0x06001DF4 RID: 7668 RVA: 0x000C2D77 File Offset: 0x000C0F77
		public List<ProductData> SelectedProducts { get; set; }

		// Token: 0x170008BD RID: 2237
		// (get) Token: 0x06001DF5 RID: 7669 RVA: 0x000C2D80 File Offset: 0x000C0F80
		// (set) Token: 0x06001DF6 RID: 7670 RVA: 0x000C2D98 File Offset: 0x000C0F98
		public bool ReferenceProductVisible
		{
			get
			{
				return this.referenceProductVisible;
			}
			set
			{
				this.referenceProductVisible = value;
				this.OnPropertyChanged("ReferenceProductVisible");
			}
		}

		// Token: 0x170008BE RID: 2238
		// (get) Token: 0x06001DF7 RID: 7671 RVA: 0x000C2DAE File Offset: 0x000C0FAE
		// (set) Token: 0x06001DF8 RID: 7672 RVA: 0x000C2DB6 File Offset: 0x000C0FB6
		public ObservableCollection<ProductData> ProductList { get; set; }

		// Token: 0x170008BF RID: 2239
		// (get) Token: 0x06001DF9 RID: 7673 RVA: 0x000C2DC0 File Offset: 0x000C0FC0
		// (set) Token: 0x06001DFA RID: 7674 RVA: 0x000C2DD8 File Offset: 0x000C0FD8
		public ProductData SelectedProduct
		{
			get
			{
				return this.selectedProduct;
			}
			set
			{
				bool flag = this.selectedProduct != value;
				if (flag)
				{
					this.selectedProduct = value;
					this.OnPropertyChanged("SelectedProduct");
				}
			}
		}

		// Token: 0x170008C0 RID: 2240
		// (get) Token: 0x06001DFB RID: 7675 RVA: 0x000C2E0C File Offset: 0x000C100C
		// (set) Token: 0x06001DFC RID: 7676 RVA: 0x000C2E24 File Offset: 0x000C1024
		public ProductData CurrentLoadedProduct
		{
			get
			{
				return this.currentLoadedProduct;
			}
			set
			{
				bool flag = this.currentLoadedProduct != value;
				if (flag)
				{
					this.currentLoadedProduct = value;
					this.OnPropertyChanged("CurrentLoadedProduct");
				}
			}
		}

		// Token: 0x170008C1 RID: 2241
		// (get) Token: 0x06001DFD RID: 7677 RVA: 0x000C2E58 File Offset: 0x000C1058
		// (set) Token: 0x06001DFE RID: 7678 RVA: 0x000C2E70 File Offset: 0x000C1070
		public ProductData SelectedSubProduct
		{
			get
			{
				return this.selectedSubProduct;
			}
			set
			{
				bool flag = this.selectedSubProduct != value;
				if (flag)
				{
					this.selectedSubProduct = value;
					this.OnPropertyChanged("SelectedSubProduct");
				}
			}
		}

		// Token: 0x14000099 RID: 153
		// (add) Token: 0x06001DFF RID: 7679 RVA: 0x000C2EA4 File Offset: 0x000C10A4
		// (remove) Token: 0x06001E00 RID: 7680 RVA: 0x000C2EDC File Offset: 0x000C10DC
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x1400009A RID: 154
		// (add) Token: 0x06001E01 RID: 7681 RVA: 0x000C2F14 File Offset: 0x000C1114
		// (remove) Token: 0x06001E02 RID: 7682 RVA: 0x000C2F4C File Offset: 0x000C114C
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<ProductData, ProductData> ProductChanged;

		// Token: 0x1400009B RID: 155
		// (add) Token: 0x06001E03 RID: 7683 RVA: 0x000C2F84 File Offset: 0x000C1184
		// (remove) Token: 0x06001E04 RID: 7684 RVA: 0x000C2FBC File Offset: 0x000C11BC
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<ProductData> CurrentProductRenamed;

		// Token: 0x1400009C RID: 156
		// (add) Token: 0x06001E05 RID: 7685 RVA: 0x000C2FF4 File Offset: 0x000C11F4
		// (remove) Token: 0x06001E06 RID: 7686 RVA: 0x000C302C File Offset: 0x000C122C
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action SubProductRenamed;

		// Token: 0x1400009D RID: 157
		// (add) Token: 0x06001E07 RID: 7687 RVA: 0x000C3064 File Offset: 0x000C1264
		// (remove) Token: 0x06001E08 RID: 7688 RVA: 0x000C309C File Offset: 0x000C129C
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action SubProductAdded;

		// Token: 0x1400009E RID: 158
		// (add) Token: 0x06001E09 RID: 7689 RVA: 0x000C30D4 File Offset: 0x000C12D4
		// (remove) Token: 0x06001E0A RID: 7690 RVA: 0x000C310C File Offset: 0x000C130C
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action SubProductRemoved;

		// Token: 0x1400009F RID: 159
		// (add) Token: 0x06001E0B RID: 7691 RVA: 0x000C3144 File Offset: 0x000C1344
		// (remove) Token: 0x06001E0C RID: 7692 RVA: 0x000C317C File Offset: 0x000C137C
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action RetrainGroupSucceeded;

		// Token: 0x140000A0 RID: 160
		// (add) Token: 0x06001E0D RID: 7693 RVA: 0x000C31B4 File Offset: 0x000C13B4
		// (remove) Token: 0x06001E0E RID: 7694 RVA: 0x000C31EC File Offset: 0x000C13EC
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action BatchChanged;

		// Token: 0x06001E0F RID: 7695 RVA: 0x000C3224 File Offset: 0x000C1424
		public ProductManager(DBConnection dbConnection, ParameterHandling parHandling, ImageDataTransfer imageTransfer, MachineState machineState, Conveyor conveyor, AuditCheckSettings auditSettings, AccessRightsManager accessManager, StoreImageData imageArchiver, TestcardSimService testcardSimService, RejecterActivity rejecterActivity)
		{
			this.dbConnection = dbConnection;
			this.parHandling = parHandling;
			this.machineState = machineState;
			this.conveyor = conveyor;
			this.auditSettings = auditSettings;
			this.imageTransfer = imageTransfer;
			this.accessManager = accessManager;
			this.imageArchiver = imageArchiver;
			this.rejecterActivity = rejecterActivity;
			this.ProductList = new ObservableCollection<ProductData>();
			this.dbConnection = dbConnection;
			this.testcardSimService = testcardSimService;
			this.CreateProductList();
			string currentTableName = null;
			dbConnection.GetCurrentProductTableName(ref currentTableName);
			bool flag = (from productData in this.ProductList
			where productData.TableName == currentTableName
			select productData).ToList<ProductData>().Count > 0;
			if (flag)
			{
				this.CurrentLoadedProduct = (from productData in this.ProductList
				where productData.TableName == currentTableName
				select productData).ToList<ProductData>()[0];
			}
			bool flag2 = this.CurrentLoadedProduct == null;
			if (flag2)
			{
				this.CurrentLoadedProduct = this.ProductList.First<ProductData>();
				dbConnection.ChangeAppl(this.CurrentLoadedProduct.TableName, null, null);
			}
			this.SelectedProduct = this.CurrentLoadedProduct;
			this.ViewProducts = (ListCollectionView)CollectionViewSource.GetDefaultView(this.ProductList);
			this.ViewProducts.CustomSort = new ProductDataComparer();
			this.ViewProducts.MoveCurrentTo(this.CurrentLoadedProduct);
			this.ViewProducts.CurrentChanged += delegate(object <p0>, EventArgs <p1>)
			{
				this.SelectedProduct = (ProductData)this.ViewProducts.CurrentItem;
			};
			this.backgroundWorker = new BackgroundWorker();
			this.backgroundWorker.WorkerReportsProgress = true;
			this.backgroundWorker.DoWork += this.backgroundWorker_DoWork;
			this.backgroundWorker.ProgressChanged += this.backgroundWorker_ProgressChanged;
			this.backgroundWorker.RunWorkerCompleted += this.backgroundWorker_RunWorkerCompleted;
			bool flag3 = accessManager.CurrentUser.ID < 0;
			if (flag3)
			{
				this.ReferenceProductVisible = true;
			}
			else
			{
				this.ReferenceProductVisible = false;
			}
			accessManager.CurrentUser.PropertyChanged += this.CurrentUser_PropertyChanged;
		}

		// Token: 0x06001E10 RID: 7696 RVA: 0x000C3488 File Offset: 0x000C1688
		private void CurrentUser_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			bool flag = this.accessManager.CurrentUser.ID < 0;
			if (flag)
			{
				bool flag2 = this.referenceProduct != null;
				if (flag2)
				{
					bool flag3 = !this.ProductList.Contains(this.referenceProduct);
					if (flag3)
					{
						this.ProductList.Insert(0, this.referenceProduct);
					}
					this.ReferenceProductVisible = true;
				}
			}
			else
			{
				this.ReferenceProductVisible = false;
				bool flag4 = this.referenceProduct != null;
				if (flag4)
				{
					this.ProductList.Remove(this.referenceProduct);
				}
			}
		}

		// Token: 0x06001E11 RID: 7697 RVA: 0x000C3520 File Offset: 0x000C1720
		public void ReloadProductList()
		{
			this.ProductList.Clear();
			this.CreateProductList();
			string currentTableName = null;
			this.dbConnection.GetCurrentProductTableName(ref currentTableName);
			bool flag = (from productData in this.ProductList
			where productData.TableName == currentTableName
			select productData).ToList<ProductData>().Count > 0;
			if (flag)
			{
				this.CurrentLoadedProduct = (from productData in this.ProductList
				where productData.TableName == currentTableName
				select productData).ToList<ProductData>()[0];
			}
			this.SelectedProduct = this.CurrentLoadedProduct;
		}

		// Token: 0x06001E12 RID: 7698 RVA: 0x000C35C0 File Offset: 0x000C17C0
		private bool CreateProductList()
		{
			List<string> list = new List<string>();
			bool mainProductTableNames = this.dbConnection.GetMainProductTableNames(list);
			string productName = null;
			string s = "";
			int num = 0;
			ProductType productType = ProductType.SingleProduct;
			foreach (string text in list)
			{
				bool flag = text != DBConnection.defaultApplication;
				if (flag)
				{
					this.dbConnection.GetProductName(text, ref productName);
					this.dbConnection.GetProductNumber(text, ref s);
					this.dbConnection.GetProductType(text, ref productType);
					int.TryParse(s, out num);
					bool flag2 = num > 0;
					if (flag2)
					{
						ProductData item = new ProductData(text, productName, num.ToString(), productType, this.dbConnection);
						this.ProductList.Add(item);
					}
					else
					{
						this.referenceProduct = new ProductData(text, productName, num.ToString(), productType, this.dbConnection);
					}
				}
			}
			return mainProductTableNames;
		}

		// Token: 0x06001E13 RID: 7699 RVA: 0x000C36E4 File Offset: 0x000C18E4
		public void CreateNewProduct()
		{
			this.keyboard = new Keyboard(LocalizationManager.Localize("msgCreateAppl"), "", KeyboardType.Application);
			this.keyboard.TextInputKeyboard += this.keyboard_TextInputKeyboard_New;
			this.keyboard.ShowDialog();
			this.ViewProducts.Refresh();
		}

		// Token: 0x06001E14 RID: 7700 RVA: 0x000C3740 File Offset: 0x000C1940
		private void keyboard_TextInputKeyboard_New(object sender, KeyboardInputEventArgs e)
		{
			this.keyboard.TextInputKeyboard -= this.keyboard_TextInputKeyboard_New;
			bool flag = e.Text.Length != 0;
			if (flag)
			{
				int firstFreeProductNumber = this.dbConnection.GetFirstFreeProductNumber();
				string text = this.dbConnection.CreateNewProductTableName();
				bool flag2 = this.dbConnection.CreateNewAppl(e.Text, firstFreeProductNumber.ToString(), ProductType.SingleProduct, text);
				bool flag3 = this.parHandling.ReadPar(Definition.anomalyDetectionEnabled) == "1" || this.parHandling.ReadPar(Definition.objectDetectionEnabled) == "1";
				if (flag3)
				{
					string newTableName = text.Insert("AppDefault".Length, "AI");
					bool flag4 = this.dbConnection.CreateNewApplAI(e.Text, newTableName);
					flag2 = (flag2 && flag4);
				}
				bool flag5 = flag2;
				if (flag5)
				{
					ProductData item = new ProductData(text, e.Text, firstFreeProductNumber.ToString());
					this.ProductList.Add(item);
					this.SelectedProduct = item;
					LogbookManager.AddEntry(SSTEntryCode.ProductCreated, 0, this.SelectedProduct.ProductNumber, this.SelectedProduct.ProductName);
					this.ChangeProduct(false);
				}
				else
				{
					Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("msgNewApplError"), DialogStyle.Error);
				}
			}
			else
			{
				Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("msgInsertAppName"), DialogStyle.Error);
			}
		}

		// Token: 0x06001E15 RID: 7701 RVA: 0x000C38C0 File Offset: 0x000C1AC0
		public void CopyProduct()
		{
			bool flag = this.SelectedProduct != null;
			if (flag)
			{
				this.keyboard = new Keyboard(LocalizationManager.Localize("msgCopyAppl"), this.SelectedProduct.ProductName, KeyboardType.Application);
				this.keyboard.TextInputKeyboard += this.keyboard_TextInputKeyboard_Copy;
				this.keyboard.ShowDialog();
				this.ViewProducts.Refresh();
			}
		}

		// Token: 0x06001E16 RID: 7702 RVA: 0x000C3930 File Offset: 0x000C1B30
		private void keyboard_TextInputKeyboard_Copy(object sender, KeyboardInputEventArgs e)
		{
			string text = e.Text;
			this.keyboard.TextInputKeyboard -= this.keyboard_TextInputKeyboard_Copy;
			bool flag = text.Length != 0;
			if (flag)
			{
				int firstFreeProductNumber = this.dbConnection.GetFirstFreeProductNumber();
				string text2 = this.dbConnection.CreateNewProductTableName();
				bool flag2 = this.dbConnection.CopyAppl(this.SelectedProduct.TableName, text, firstFreeProductNumber.ToString(), text2);
				bool flag3 = this.dbConnection.CopyMaterialCurve(this.SelectedProduct.TableName, text2);
				bool flag4 = this.dbConnection.CopyTestcardSimSensitivities(this.SelectedProduct.TableName, text2);
				bool flag5 = this.CopyProductAI(this.SelectedProduct.TableName, text2, text);
				bool flag6 = flag2 && flag3 && flag4 && flag5;
				if (flag6)
				{
					this.CopySubProducts(this.SelectedProduct, firstFreeProductNumber.ToString(), text2);
					ProductData item = new ProductData(text2, text, firstFreeProductNumber.ToString(), this.SelectedProduct.ProductType, this.dbConnection);
					this.ProductList.Add(item);
					LogbookManager.AddEntry(SSTEntryCode.ProductCopied, 0, this.SelectedProduct.ProductNumber, this.SelectedProduct.ProductName, firstFreeProductNumber.ToString(), text);
					this.SelectedProduct = item;
					this.ChangeProduct(false);
				}
				else
				{
					Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("msgCopyApplError"), DialogStyle.Error);
				}
			}
			else
			{
				Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("msgInsertAppName"), DialogStyle.Error);
			}
		}

		// Token: 0x06001E17 RID: 7703 RVA: 0x000C3AC0 File Offset: 0x000C1CC0
		private bool CopyProductAI(string selectedTableName, string newTableName, string copiedProductName)
		{
			bool result = true;
			string text = selectedTableName.Insert("AppDefault".Length, "AI");
			bool flag = TableOperations.TableExists(this.dbConnection.Connection, text);
			if (flag)
			{
				string text2 = newTableName.Insert("AppDefault".Length, "AI");
				result = this.dbConnection.CopyApplAI(text, copiedProductName, text2);
				string archiveDirectoryAnomalyDetection = this.imageArchiver.GetArchiveDirectoryAnomalyDetection(text);
				bool flag2 = Directory.Exists(archiveDirectoryAnomalyDetection);
				if (flag2)
				{
					string archiveDirectoryAnomalyDetection2 = this.imageArchiver.GetArchiveDirectoryAnomalyDetection(text2);
					ProductManager.CopyDirectory(archiveDirectoryAnomalyDetection, archiveDirectoryAnomalyDetection2);
				}
				string text3 = Path.Combine(new string[]
				{
					StoreImageData.archiveRoot + "\\Model\\" + text
				});
				bool flag3 = Directory.Exists(text3);
				if (flag3)
				{
					string destinationDir = Path.Combine(new string[]
					{
						StoreImageData.archiveRoot + "\\Model\\" + text2
					});
					ProductManager.CopyDirectory(text3, destinationDir);
				}
			}
			return result;
		}

		// Token: 0x06001E18 RID: 7704 RVA: 0x000C3BBC File Offset: 0x000C1DBC
		private static void CopyDirectory(string sourceDir, string destinationDir)
		{
			bool flag = !Directory.Exists(destinationDir);
			if (flag)
			{
				Directory.CreateDirectory(destinationDir);
			}
			foreach (string text in Directory.GetFiles(sourceDir))
			{
				string destFileName = Path.Combine(destinationDir, Path.GetFileName(text));
				File.Copy(text, destFileName, true);
			}
		}

		// Token: 0x06001E19 RID: 7705 RVA: 0x000C3C14 File Offset: 0x000C1E14
		private void CopySubProducts(ProductData product, string newProductNumber, string newTableName)
		{
			bool flag = !product.IsSingleProduct;
			if (flag)
			{
				foreach (ProductData productData in product.SubProducts)
				{
					string text = this.dbConnection.CreateNewSubProductTableName(newTableName, (int)productData.Group);
					string newProductNumber2 = newProductNumber + Definition.subTableIdentifier.ToString() + productData.Group.ToString();
					this.dbConnection.CopyAppl(productData.TableName, productData.ProductName, newProductNumber2, text);
					this.dbConnection.CopyMaterialCurve(productData.TableName, text);
				}
			}
		}

		// Token: 0x06001E1A RID: 7706 RVA: 0x000C3CD8 File Offset: 0x000C1ED8
		public void DeleteProduct()
		{
			bool flag = this.SelectedProducts != null;
			if (flag)
			{
				bool flag2 = this.SelectedProducts.Count > 1;
				if (flag2)
				{
					Dialog.DialogResults dialogResults = Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("msgProductsErased"), DialogStyle.Warning, DialogButtons.OKCancel);
					bool flag3 = dialogResults == Dialog.DialogResults.Ok;
					if (flag3)
					{
						foreach (ProductData productData in this.SelectedProducts)
						{
							this.SelectedProduct = productData;
							bool flag4 = this.CurrentLoadedProduct != this.SelectedProduct;
							if (flag4)
							{
								bool flag5 = this.dbConnection.DeleteTable(this.SelectedProduct.TableName);
								bool flag6 = this.DeleteProductAI(this.SelectedProduct.TableName);
								bool flag7 = flag5 && flag6;
								if (flag7)
								{
									this.SetSubLaneProductNameOrNumber(this.SelectedProduct.TableName, "", "");
									bool flag8 = !this.SelectedProduct.IsSingleProduct;
									if (flag8)
									{
										this.RemoveSubProducts(this.SelectedProduct.TableName);
									}
									LogbookManager.AddEntry(SSTEntryCode.ProductDeleted, 0, this.SelectedProduct.ProductNumber, this.SelectedProduct.ProductName);
									this.dbConnection.DeleteStatistics(this.SelectedProduct.TableName);
									this.DeleteBatchTable(this.SelectedProduct.TableName);
									this.dbConnection.DeleteMaterialCurve(this.SelectedProduct.TableName);
									this.dbConnection.DeleteTestcardSimSensitivities(this.SelectedProduct.TableName);
									this.ProductList.Remove(this.SelectedProduct);
								}
								else
								{
									Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("msgDeleteApplError"), DialogStyle.Error);
								}
							}
							else
							{
								Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("msgCurrentApplicationDeleteForbidden"), DialogStyle.Warning, DialogButtons.OK);
							}
						}
					}
				}
				else
				{
					this.SelectedProduct = this.SelectedProducts.Last<ProductData>();
					bool flag9 = this.SelectedProduct != null;
					if (flag9)
					{
						bool flag10 = this.CurrentLoadedProduct != this.SelectedProduct;
						if (flag10)
						{
							Dialog.DialogResults dialogResults = Dialog.Show(LocalizationManager.Localize("txtWarning"), string.Concat(new string[]
							{
								LocalizationManager.Localize("txtApplication"),
								" \"",
								this.SelectedProduct.ProductName,
								"\" ",
								LocalizationManager.Localize("msgGettingErased")
							}), DialogStyle.Warning, DialogButtons.OKCancel);
							bool flag11 = dialogResults == Dialog.DialogResults.Ok;
							if (flag11)
							{
								bool flag12 = this.dbConnection.DeleteTable(this.SelectedProduct.TableName);
								bool flag13 = this.DeleteProductAI(this.SelectedProduct.TableName);
								bool flag14 = flag12 && flag13;
								if (flag14)
								{
									this.SetSubLaneProductNameOrNumber(this.SelectedProduct.TableName, "", "");
									bool flag15 = !this.SelectedProduct.IsSingleProduct;
									if (flag15)
									{
										this.RemoveSubProducts(this.SelectedProduct.TableName);
									}
									LogbookManager.AddEntry(SSTEntryCode.ProductDeleted, 0, this.SelectedProduct.ProductNumber, this.SelectedProduct.ProductName);
									this.dbConnection.DeleteStatistics(this.SelectedProduct.TableName);
									this.DeleteBatchTable(this.SelectedProduct.TableName);
									this.dbConnection.DeleteMaterialCurve(this.SelectedProduct.TableName);
									this.dbConnection.DeleteTestcardSimSensitivities(this.SelectedProduct.TableName);
									this.ProductList.Remove(this.SelectedProduct);
								}
								else
								{
									Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("msgDeleteApplError"), DialogStyle.Error);
								}
							}
						}
						else
						{
							Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("msgCurrentApplicationDeleteForbidden"), DialogStyle.Warning, DialogButtons.OK);
						}
					}
				}
			}
		}

		// Token: 0x06001E1B RID: 7707 RVA: 0x000C40EC File Offset: 0x000C22EC
		private bool DeleteProductAI(string selectedProductTableName)
		{
			bool result = this.dbConnection.DeleteTableAI(selectedProductTableName);
			string text = selectedProductTableName.Insert("AppDefault".Length, "AI");
			string archiveDirectoryAnomalyDetection = this.imageArchiver.GetArchiveDirectoryAnomalyDetection(text);
			bool flag = Directory.Exists(archiveDirectoryAnomalyDetection);
			if (flag)
			{
				Directory.Delete(archiveDirectoryAnomalyDetection, true);
			}
			string path = Path.Combine(new string[]
			{
				StoreImageData.archiveRoot + "\\Model\\" + text
			});
			bool flag2 = Directory.Exists(path);
			if (flag2)
			{
				Directory.Delete(path, true);
			}
			return result;
		}

		// Token: 0x06001E1C RID: 7708 RVA: 0x000C4180 File Offset: 0x000C2380
		private void RemoveSubProducts(string mainTableName)
		{
			DetectorType detectorType = DetectorType.Sesotec;
			this.dbConnection.GetDetectorType(ref detectorType);
			List<string> list = new List<string>();
			this.dbConnection.GetSubProductTablenames(mainTableName, list);
			for (int i = 0; i < list.Count; i++)
			{
				this.dbConnection.DeleteTable(list[i]);
				bool flag = detectorType == DetectorType.DTNetDE;
				if (flag)
				{
					this.dbConnection.DeleteTable(list[i] + "_MaterialCurve");
				}
			}
			this.SelectedProduct.SubProducts.Clear();
		}

		// Token: 0x06001E1D RID: 7709 RVA: 0x000C4214 File Offset: 0x000C2414
		public void RenameProduct()
		{
			bool flag = this.SelectedProduct != null;
			if (flag)
			{
				this.keyboard = new Keyboard(LocalizationManager.Localize("msgRenameAppl"), this.SelectedProduct.ProductName, KeyboardType.Application);
				this.keyboard.TextInputKeyboard += this.keyboard_TextInputKeyboard_RenameProduct;
				this.keyboard.ShowDialog();
				this.numKeyboard = new NumKeyboard(Resources.ResourceManager.GetString("msgRenameApplNumber", LocalizationManager.CurrentCulture), this.SelectedProduct.ProductNumber.ToString(), NumKeyboardType.TypeUInt);
				this.numKeyboard.TextInputKeyboard += this.keyboard_TextInputKeyboard_RenameProductNumber;
				this.numKeyboard.ShowDialog();
			}
		}

		// Token: 0x06001E1E RID: 7710 RVA: 0x000C42D0 File Offset: 0x000C24D0
		public bool RenameProduct(int productNumber, string newProductName)
		{
			foreach (ProductData productData in this.ProductList)
			{
				bool flag = productData.ProductNumber == productNumber.ToString();
				if (flag)
				{
					this.RenameProduct(productData, newProductName);
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001E1F RID: 7711 RVA: 0x000C4344 File Offset: 0x000C2544
		private void RenameProduct(ProductData product, string newProductName)
		{
			bool flag = this.keyboard != null;
			if (flag)
			{
				this.keyboard.TextInputKeyboard -= this.keyboard_TextInputKeyboard_RenameProduct;
			}
			bool flag2 = newProductName.Length != 0;
			if (flag2)
			{
				bool flag3 = newProductName != "";
				if (flag3)
				{
					bool flag4 = this.dbConnection.RenameAppl(product.TableName, newProductName);
					bool flag5 = flag4;
					if (flag5)
					{
						LogbookManager.AddEntry(SSTEntryCode.ProductRenamed, 0, product.ProductNumber, product.ProductName, newProductName);
						product.ProductName = newProductName;
						this.SetSubLaneProductNameOrNumber(product);
						bool flag6 = product == this.CurrentLoadedProduct;
						if (flag6)
						{
							this.OnCurrentProductRenamed(product);
						}
					}
					else
					{
						Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("msgRenameApplError"), DialogStyle.Error);
					}
				}
			}
			else
			{
				Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("msgInsertAppName"), DialogStyle.Error);
			}
		}

		// Token: 0x06001E20 RID: 7712 RVA: 0x000C4430 File Offset: 0x000C2630
		private void keyboard_TextInputKeyboard_RenameProduct(object sender, KeyboardInputEventArgs e)
		{
			this.RenameProduct(this.SelectedProduct, e.Text);
		}

		// Token: 0x06001E21 RID: 7713 RVA: 0x000C4448 File Offset: 0x000C2648
		private void keyboard_TextInputKeyboard_RenameProductNumber(object sender, KeyboardInputEventArgs e)
		{
			int num = -1;
			bool flag = int.TryParse(e.Text, out num) && num <= 65535 && num > 0;
			if (flag)
			{
				this.RenameProductNumber(this.SelectedProduct, num);
			}
		}

		// Token: 0x06001E22 RID: 7714 RVA: 0x000C4488 File Offset: 0x000C2688
		private void RenameProductNumber(ProductData product, int newProductNumber)
		{
			bool flag = this.numKeyboard != null;
			if (flag)
			{
				this.numKeyboard.TextInputKeyboard -= this.keyboard_TextInputKeyboard_RenameProductNumber;
			}
			List<string> list = new List<string>();
			this.dbConnection.GetProductNumberList(list);
			bool flag2 = newProductNumber > 0 && (newProductNumber.ToString() == this.SelectedProduct.ProductNumber || !list.Contains(newProductNumber.ToString()));
			if (flag2)
			{
				bool flag3 = this.dbConnection.SetProductNumber(product.TableName, newProductNumber.ToString());
				bool flag4 = flag3;
				if (flag4)
				{
					LogbookManager.AddEntry(SSTEntryCode.ProductNumberChanged, 0, product.ProductNumber, product.ProductName, newProductNumber.ToString());
					product.ProductNumber = newProductNumber.ToString();
					this.SetSubLaneProductNameOrNumber(product);
					bool flag5 = product.IsMultiProduct || product.IsRegionProduct;
					if (flag5)
					{
						foreach (ProductData productData in product.SubProducts)
						{
							int num = productData.ProductNumber.IndexOf(Definition.subTableIdentifier);
							string str = productData.ProductNumber.Remove(0, num + 1);
							bool flag6 = num > 0 && num < productData.ProductNumber.Length - 1;
							if (flag6)
							{
								productData.ProductNumber = product.ProductNumber + Definition.subTableIdentifier.ToString() + str;
								this.dbConnection.SetProductNumber(productData.TableName, productData.ProductNumber);
							}
						}
					}
				}
				else
				{
					Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("msgRenameApplError"), DialogStyle.Error);
				}
			}
			else
			{
				Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("msgNumberExists"), DialogStyle.Error);
			}
			this.ViewProducts.Refresh();
		}

		// Token: 0x06001E23 RID: 7715 RVA: 0x000C4698 File Offset: 0x000C2898
		public bool ChangeProduct(ProductData product)
		{
			bool flag = !this.backgroundWorker.IsBusy;
			if (flag)
			{
				bool flag2 = this.ProductList.Contains(product);
				if (flag2)
				{
					this.SelectedProduct = product;
					this.ChangeProduct(true);
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001E24 RID: 7716 RVA: 0x000C46E4 File Offset: 0x000C28E4
		public bool ChangeProduct(bool noPrompt)
		{
			bool flag = (this.SelectedProduct.ProductNumber.Equals("-1") && this.accessManager.CurrentUser.ID < 0) || !this.SelectedProduct.ProductNumber.Equals("-1");
			bool result;
			if (flag)
			{
				bool flag2 = false;
				bool flag3 = !noPrompt;
				if (flag3)
				{
					this.ViewProducts.MoveCurrentTo(this.SelectedProduct);
					this.ViewProducts.Refresh();
				}
				bool flag4 = !this.backgroundWorker.IsBusy;
				if (flag4)
				{
					Dialog.DialogResults dialogResults = Dialog.DialogResults.Ok;
					bool flag5 = this.SelectedProduct != this.CurrentLoadedProduct;
					if (flag5)
					{
						bool flag6 = !noPrompt;
						if (flag6)
						{
							dialogResults = Dialog.Show(LocalizationManager.Localize("txtWarning"), string.Concat(new string[]
							{
								LocalizationManager.Localize("txtApplication"),
								" \"",
								this.SelectedProduct.ProductName,
								"\" ",
								LocalizationManager.Localize("msgGettingLoaded")
							}), DialogStyle.Warning, DialogButtons.OKCancel);
						}
						bool flag7 = dialogResults == Dialog.DialogResults.Ok;
						if (flag7)
						{
							bool flag8 = this.parHandling.ReadPar(Definition.detectorMonitoringActive) == "1";
							if (flag8)
							{
								this.parHandling.WriteValue(Definition.detectorMonitoringBackgroundEnable, "0", false);
							}
							this.oldState = this.machineState.State;
							this.machineState.State = MachineStates.ProductChange;
							bool flag9 = Application.Current.Dispatcher != null;
							if (flag9)
							{
								Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
								{
									bool flag11 = !this.CurrentLoadedProduct.IsSingleProduct;
									if (flag11)
									{
										this.AddOrRemoveSubFiltersFromParameterServer(this.CurrentLoadedProduct, false);
									}
									bool flag12 = !this.SelectedProduct.IsSingleProduct;
									if (flag12)
									{
										this.AddOrRemoveSubFiltersFromParameterServer(this.SelectedProduct, true);
									}
									this.prog = new Progress(string.Concat(new string[]
									{
										LocalizationManager.Localize("txtApplication"),
										" \"",
										this.SelectedProduct.ProductName,
										"\" ",
										LocalizationManager.Localize("msgLoadingApplication")
									}), false);
									this.pathes.Clear();
									this.values.Clear();
									this.rejecterActivity.SaveValuesBeforeProductChanged();
									this.dbConnection.ChangeAppl(this.SelectedProduct.TableName, this.pathes, this.values);
									string text = null;
									this.dbConnection.GetCurrentProductTableNameAI(ref text);
									bool flag13 = TableOperations.TableExists(this.dbConnection.Connection, text);
									bool flag14 = (this.parHandling.ReadPar(Definition.anomalyDetectionEnabled) == "1" || this.parHandling.ReadPar(Definition.objectDetectionEnabled) == "1") && !flag13;
									if (flag14)
									{
										this.dbConnection.CreateNewApplAI(this.SelectedProduct.ProductName, text);
									}
									bool flag15 = flag13 && this.parHandling.ReadPar(Definition.anomalyDetectionTrainingInProgress) == "0";
									if (flag15)
									{
										this.dbConnection.GetProductEntries(text, this.pathes, this.values);
									}
									else
									{
										bool flag16 = flag13 && this.parHandling.ReadPar(Definition.anomalyDetectionTrainingInProgress) == "1";
										if (flag16)
										{
											List<string> list = new List<string>();
											List<string> list2 = new List<string>();
											this.dbConnection.GetProductEntries(text, list2, list);
											for (int i = list2.Count - 1; i >= 0; i--)
											{
												bool flag17 = !list2[i].StartsWith("/filter/anomalyDetection/inference") && !list2[i].StartsWith("/filter/objectDetection");
												if (flag17)
												{
													list2.RemoveAt(i);
													list.RemoveAt(i);
												}
											}
											this.pathes.AddRange(list2);
											this.values.AddRange(list);
										}
									}
									bool flag18 = !this.SelectedProduct.IsSingleProduct;
									if (flag18)
									{
										this.AddMultiProductPathes();
									}
									this.prog.Show();
									this.backgroundWorker.RunWorkerAsync();
								}));
							}
						}
					}
					else
					{
						bool flag10 = !noPrompt;
						if (flag10)
						{
							flag2 = true;
						}
					}
				}
				result = flag2;
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06001E25 RID: 7717 RVA: 0x000C48BC File Offset: 0x000C2ABC
		private void AddMultiProductPathes()
		{
			List<string> list = new List<string>();
			this.dbConnection.GetSubProductTablenames(this.SelectedProduct.TableName, list);
			for (int i = 0; i < list.Count; i++)
			{
				int group = 0;
				string s = list[i].Remove(0, list[i].IndexOf(Definition.subTableIdentifier) + 1);
				int.TryParse(s, out group);
				this.dbConnection.GetCurrentSubProductEntries(group, this.pathes, this.values);
			}
		}

		// Token: 0x06001E26 RID: 7718 RVA: 0x000C4948 File Offset: 0x000C2B48
		private void AddOrRemoveSubFiltersFromParameterServer(ProductData multiProduct, bool add)
		{
			bool flag = multiProduct.SubProducts.Count > 0;
			if (flag)
			{
				this.SelectedSubProduct = multiProduct.SubProducts[0];
			}
			for (int i = 0; i < multiProduct.SubProducts.Count; i++)
			{
				int num = 0;
				string s = multiProduct.SubProducts[i].TableName.Remove(0, multiProduct.SubProducts[i].TableName.IndexOf(Definition.subTableIdentifier) + 1);
				int.TryParse(s, out num);
				if (add)
				{
					this.parHandling.AddGroupToParameterServer(num);
					bool flag2 = multiProduct.ProductType == ProductType.RegionProduct;
					if (flag2)
					{
						this.parHandling.AddRegionToParameterServer(num);
					}
				}
				else
				{
					this.parHandling.RemoveGroupFromParameterServer(num);
					bool flag3 = multiProduct.ProductType == ProductType.RegionProduct;
					if (flag3)
					{
						this.parHandling.RemoveRegionFromParameterServer(num);
					}
				}
			}
		}

		// Token: 0x06001E27 RID: 7719 RVA: 0x000C4A40 File Offset: 0x000C2C40
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x06001E28 RID: 7720 RVA: 0x000C4A70 File Offset: 0x000C2C70
		private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			ProductData productData = new ProductData(this.CurrentLoadedProduct.TableName, this.CurrentLoadedProduct.ProductName, this.CurrentLoadedProduct.ProductNumber);
			productData.ProductType = this.CurrentLoadedProduct.ProductType;
			int value = 5;
			int value2 = 30;
			this.dbConnection.GetNAutolearnDefault(ref value);
			this.dbConnection.GetNAutolearnDefaultBulk(ref value2);
			bool flag = this.currentLoadedProduct != null;
			if (flag)
			{
				this.currentLoadedProduct.CurrentBatchName = null;
			}
			this.CurrentLoadedProduct = this.SelectedProduct;
			bool flag2 = !this.CurrentLoadedProduct.IsSingleProduct;
			if (flag2)
			{
				bool flag3 = this.SelectedProduct.SubProducts.Count > 0;
				if (flag3)
				{
					this.SelectedSubProduct = this.SelectedProduct.SubProducts[0];
				}
			}
			bool flag4 = !productData.IsBulkObjectProduct && this.CurrentLoadedProduct.IsBulkObjectProduct;
			if (flag4)
			{
				this.parHandling.WriteValue(Definition.machineModePath, Convert.ToInt32(MachineMode.BulkObject).ToString(), true);
				this.parHandling.WriteValue(Autotrain.nAutolearnPath, Convert.ToInt32(value).ToString(), true);
			}
			bool flag5 = productData.IsBulkObjectProduct && !this.CurrentLoadedProduct.IsBulkObjectProduct;
			if (flag5)
			{
				this.parHandling.WriteValue(Definition.machineModePath, Convert.ToInt32(MachineMode.Bulk).ToString(), true);
				this.parHandling.WriteValue(Autotrain.nAutolearnPath, Convert.ToInt32(value2).ToString(), true);
			}
			this.prog.FormFadeOut.Begin();
			bool flag6 = this.machineState.State == MachineStates.ProductChange;
			if (flag6)
			{
				this.machineState.State = this.oldState;
			}
			this.OnProductChanged(productData, this.CurrentLoadedProduct);
		}

		// Token: 0x06001E29 RID: 7721 RVA: 0x000C4C58 File Offset: 0x000C2E58
		private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			this.prog.progBar.Value = (double)e.ProgressPercentage;
		}

		// Token: 0x06001E2A RID: 7722 RVA: 0x000C4C73 File Offset: 0x000C2E73
		private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
		{
			this.LoadProduct();
		}

		// Token: 0x06001E2B RID: 7723 RVA: 0x000C4C80 File Offset: 0x000C2E80
		private void LoadProduct()
		{
			for (int i = 0; i < this.pathes.Count; i++)
			{
				bool flag = this.pathes[i].StartsWith(Definition.SVTValvesAssignmentMainPath);
				if (!flag)
				{
					string text = this.pathes[i];
					string a = text;
					if (!(a == "/hardware/detector/ScanRate/Value"))
					{
						if (!(a == "ConveyorSpeed"))
						{
							if (!(a == "/machine/DeformationFactor/Value"))
							{
								if (!(a == "/hardware/xraysource/CurrentSetpoint/Value"))
								{
									if (!(a == "/hardware/xraysource/VoltageSetpoint/Value"))
									{
										if (!(a == "SShXRayPosition"))
										{
											this.parHandling.WriteValue(this.pathes[i], this.values[i], false);
											bool flag2 = i % 20 == 0;
											if (flag2)
											{
												this.backgroundWorker.ReportProgress(100 * (i + 1) / this.pathes.Count);
											}
										}
										else
										{
											MachineMode machineMode = MachineMode.Standard;
											this.dbConnection.GetMachineMode(ref machineMode);
											bool flag3 = machineMode == MachineMode.Sideshooter;
											if (flag3)
											{
												string a2 = this.parHandling.ReadPar(Definition.sshXrayMotorSetpointPath);
												bool flag4 = a2 != this.values[i];
												if (flag4)
												{
													this.dbConnection.SetSshSourcePosNeeded(true);
													this.parHandling.WriteValue(this.pathes[i], this.values[i]);
												}
											}
										}
									}
									else
									{
										bool flag5 = this.parHandling.ReadPar(XrayState.xrayPowerOnPath) == "1";
										if (flag5)
										{
											double num;
											bool flag6 = !double.TryParse(this.values[i], NumberStyles.Any, new CultureInfo("en-US"), out num) || num <= 0.0;
											if (flag6)
											{
												string value = "";
												XRayOnProcessing.SetDefaultVoltageSetpoint(this.dbConnection, this.parHandling);
												this.dbConnection.GetParameterValue(this.pathes[i], ref value);
												this.values[i] = value;
											}
											double num2 = 0.0;
											double num3 = 0.0;
											double.TryParse(this.parHandling.ReadPar(this.pathes[i]), NumberStyles.Float, new CultureInfo("en-US"), out num2);
											double.TryParse(this.values[i], NumberStyles.Float, new CultureInfo("en-US"), out num3);
											bool reportChange = Math.Abs(num2 - num3) > 2.0;
											this.parHandling.WriteValue(this.pathes[i], this.values[i], reportChange);
										}
									}
								}
								else
								{
									bool flag7 = this.parHandling.ReadPar(XrayState.xrayPowerOnPath) == "1";
									if (flag7)
									{
										double num4;
										bool flag8 = !double.TryParse(this.values[i], NumberStyles.Any, new CultureInfo("en-US"), out num4) || num4 <= 0.0;
										if (flag8)
										{
											string value2 = "";
											XRayOnProcessing.SetDefaultCurrentSetpoint(this.dbConnection, this.parHandling);
											this.dbConnection.GetParameterValue(this.pathes[i], ref value2);
											this.values[i] = value2;
										}
										double num5 = 0.0;
										double num6 = 0.0;
										double.TryParse(this.parHandling.ReadPar(this.pathes[i]), NumberStyles.Float, new CultureInfo("en-US"), out num5);
										double.TryParse(this.values[i], NumberStyles.Float, new CultureInfo("en-US"), out num6);
										bool reportChange2 = Math.Abs(num5 - num6) > 0.1;
										this.parHandling.WriteValue(this.pathes[i], this.values[i], reportChange2);
									}
								}
							}
							else
							{
								string text2 = "";
								this.dbConnection.GetParameterValue(Conveyor.deformationFactorPath, ref text2);
								this.parHandling.WriteValue(Conveyor.deformationFactorPath, text2);
								bool flag9 = text2 != this.deformationFactor;
								if (flag9)
								{
									Conveyor.DeformationFactorChanged = true;
									this.deformationFactor = text2;
									this.conveyor.AdaptScanrate();
								}
							}
						}
						else
						{
							double speed;
							bool flag10 = double.TryParse(this.values[i], NumberStyles.Float, new CultureInfo("en-US"), out speed);
							if (flag10)
							{
								this.conveyor.Speed = speed;
								this.conveyor.AdaptScanrate();
								XRayOnProcessing.AdaptedIntegrationTime = this.conveyor.AdaptedIntegrationTime;
							}
						}
					}
				}
			}
		}

		// Token: 0x06001E2C RID: 7724 RVA: 0x000C5174 File Offset: 0x000C3374
		public void ResetToFactorySettings()
		{
			bool flag = Application.Current.Dispatcher != null;
			if (flag)
			{
				bool status;
				Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
				{
					bool status;
					while (this.ProductList.Count != 0)
					{
						status = this.dbConnection.DeleteTable(this.ProductList[0].TableName);
						bool flag2 = this.dbConnection.CheckIfMaterialCurveExists(this.ProductList[0].TableName);
						if (flag2)
						{
							this.dbConnection.DeleteMaterialCurve(this.ProductList[0].TableName);
						}
						bool flag3 = this.dbConnection.CheckIfTestcardSimSensitivitiesExists(this.ProductList[0].TableName);
						if (flag3)
						{
							this.dbConnection.DeleteTestcardSimSensitivities(this.ProductList[0].TableName);
						}
						this.dbConnection.DeleteTableAI(this.ProductList[0].TableName);
						bool flag4 = !this.ProductList[0].IsSingleProduct;
						if (flag4)
						{
							for (int i = 0; i < this.ProductList[0].SubProducts.Count; i++)
							{
								this.dbConnection.DeleteTable(this.ProductList[0].SubProducts[i].TableName);
								bool flag5 = this.dbConnection.CheckIfMaterialCurveExists(this.ProductList[0].SubProducts[i].TableName);
								if (flag5)
								{
									this.dbConnection.DeleteMaterialCurve(this.ProductList[0].SubProducts[i].TableName);
								}
								bool flag6 = this.dbConnection.CheckIfTestcardSimSensitivitiesExists(this.ProductList[i].TableName);
								if (flag6)
								{
									this.dbConnection.DeleteTestcardSimSensitivities(this.ProductList[i].TableName);
								}
							}
							this.ProductList[0].SubProducts.Clear();
						}
						status = status;
						if (!status)
						{
							Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("msgDeleteApplError"), DialogStyle.Error);
							break;
						}
						this.ProductList.Remove(this.ProductList[0]);
					}
					this.dbConnection.DeleteAllBatches();
					this.dbConnection.DeleteAllStatistics();
					int firstFreeProductNumber = this.dbConnection.GetFirstFreeProductNumber();
					string text = this.dbConnection.CreateNewProductTableName();
					for (int j = 1; j <= Definition.maxSubProducts; j++)
					{
						this.parHandling.RemoveGroupFromParameterServer(j);
					}
					status = this.dbConnection.CreateNewAppl(Definition.defaultVisibleProductName, firstFreeProductNumber.ToString(), ProductType.SingleProduct, text);
					bool status2 = status;
					if (status2)
					{
						ProductData item = new ProductData(text, Definition.defaultVisibleProductName, firstFreeProductNumber.ToString());
						this.ProductList.Add(item);
						this.SelectedProduct = item;
						this.ChangeProduct(true);
					}
				}));
			}
		}

		// Token: 0x06001E2D RID: 7725 RVA: 0x000C51C0 File Offset: 0x000C33C0
		protected virtual void OnProductChanged(ProductData oldProduct, ProductData newProduct)
		{
			Action<ProductData, ProductData> productChanged = this.ProductChanged;
			bool flag = productChanged != null;
			if (flag)
			{
				productChanged(oldProduct, newProduct);
			}
		}

		// Token: 0x06001E2E RID: 7726 RVA: 0x000C51E8 File Offset: 0x000C33E8
		protected virtual void OnCurrentProductRenamed(ProductData product)
		{
			Action<ProductData> currentProductRenamed = this.CurrentProductRenamed;
			bool flag = currentProductRenamed != null;
			if (flag)
			{
				currentProductRenamed(product);
			}
		}

		// Token: 0x06001E2F RID: 7727 RVA: 0x000C5210 File Offset: 0x000C3410
		public void CreateNewMultiProduct()
		{
			this.keyboard = new Keyboard(LocalizationManager.Localize("msgCreateAppl"), "", KeyboardType.Application);
			this.keyboard.TextInputKeyboard += this.keyboard_TextInputKeyboard_NewMulti;
			this.keyboard.ShowDialog();
			this.ViewProducts.Refresh();
		}

		// Token: 0x06001E30 RID: 7728 RVA: 0x000C526C File Offset: 0x000C346C
		private void keyboard_TextInputKeyboard_NewMulti(object sender, KeyboardInputEventArgs e)
		{
			this.keyboard.TextInputKeyboard -= this.keyboard_TextInputKeyboard_NewMulti;
			Action <>9__5;
			new Thread(delegate()
			{
				bool flag = e.Text.Length != 0;
				if (flag)
				{
					bool flag2 = Application.Current.Dispatcher != null;
					if (flag2)
					{
						Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
						{
							Progress.ShowStaticProgress(LocalizationManager.Localize("txtProcessing"), false);
						}));
					}
					int firstFreeProductNumber = this.dbConnection.GetFirstFreeProductNumber();
					string text = this.dbConnection.CreateNewProductTableName();
					bool flag3 = this.dbConnection.CreateNewAppl(e.Text, firstFreeProductNumber.ToString(), ProductType.MultiProduct, text);
					bool flag4 = flag3;
					if (flag4)
					{
						ProductData productToChangeTo = new ProductData(text, e.Text, firstFreeProductNumber.ToString());
						productToChangeTo.ProductType = ProductType.MultiProduct;
						bool flag5 = Application.Current.Dispatcher != null;
						if (flag5)
						{
							Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
							{
								this.ProductList.Add(productToChangeTo);
								this.SelectedProduct = productToChangeTo;
							}));
						}
						this.AddSubProduct("Multi 1", false);
						LogbookManager.AddEntry(SSTEntryCode.ProductCreated, 0, this.SelectedProduct.ProductNumber, this.SelectedProduct.ProductName);
						Progress.CloseStaticProgress(false);
						bool flag6 = Application.Current.Dispatcher != null;
						if (flag6)
						{
							Dispatcher dispatcher = Application.Current.Dispatcher;
							DispatcherPriority priority = DispatcherPriority.Normal;
							Action method;
							if ((method = <>9__5) == null)
							{
								method = (<>9__5 = delegate()
								{
									this.ChangeProduct(false);
								});
							}
							dispatcher.Invoke(priority, method);
						}
					}
					else
					{
						bool flag7 = Application.Current.Dispatcher != null;
						if (flag7)
						{
							Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
							{
								Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("msgNewApplError"), DialogStyle.Error);
							}));
						}
					}
				}
				else
				{
					bool flag8 = Application.Current.Dispatcher != null;
					if (flag8)
					{
						Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
						{
							Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("msgInsertAppName"), DialogStyle.Error);
						}));
					}
				}
			})
			{
				Name = "CreateNewMulti"
			}.Start();
		}

		// Token: 0x06001E31 RID: 7729 RVA: 0x000C52CC File Offset: 0x000C34CC
		public void AddSubProduct()
		{
			List<string> list = new List<string>();
			this.dbConnection.GetSubProductTablenames(this.SelectedProduct.TableName, list);
			bool flag = list.Count < Definition.maxSubProducts;
			if (flag)
			{
				this.keyboard = new Keyboard(LocalizationManager.Localize("msgCreateAppl"), "", KeyboardType.Application);
				this.keyboard.TextInputKeyboard += this.keyboard_TextInputKeyboard_AddSubProduct;
				this.keyboard.ShowDialog();
				ListCollectionView listCollectionView = (ListCollectionView)CollectionViewSource.GetDefaultView(this.SelectedProduct.SubProducts);
				bool flag2 = listCollectionView != null;
				if (flag2)
				{
					listCollectionView.Refresh();
				}
			}
			else
			{
				Dialog.Show(LocalizationManager.Localize("txtError"), "Max. sub products: " + Definition.maxSubProducts.ToString(), DialogStyle.Error);
			}
		}

		// Token: 0x06001E32 RID: 7730 RVA: 0x000C539C File Offset: 0x000C359C
		private void keyboard_TextInputKeyboard_AddSubProduct(object sender, KeyboardInputEventArgs e)
		{
			this.keyboard.TextInputKeyboard -= this.keyboard_TextInputKeyboard_AddSubProduct;
			bool flag = e.Text != null && e.Text.Length > 0;
			if (flag)
			{
				new Thread(new ParameterizedThreadStart(this.AddSubProduct))
				{
					Name = "AddSubProduct"
				}.Start(e.Text);
			}
			else
			{
				Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("msgInsertAppName"), DialogStyle.Error);
			}
		}

		// Token: 0x06001E33 RID: 7731 RVA: 0x000C542C File Offset: 0x000C362C
		private void AddSubProduct(string subProductName, bool showProgress)
		{
			if (showProgress)
			{
				bool flag = Application.Current.Dispatcher != null;
				if (flag)
				{
					Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
					{
						Progress.ShowStaticProgress(LocalizationManager.Localize("txtProcessing"), false);
					}));
				}
			}
			string text = this.dbConnection.CreateNewSubProductTableName(this.SelectedProduct.TableName);
			string firstFreeSubProductNumber = this.dbConnection.GetFirstFreeSubProductNumber(this.SelectedProduct.ProductNumber);
			ProductData subProduct = new ProductData(text, subProductName, firstFreeSubProductNumber);
			bool flag2 = this.dbConnection.CreateNewSubAppl(subProductName, firstFreeSubProductNumber, text, subProduct.Group.ToString());
			bool flag3 = flag2;
			if (flag3)
			{
				this.parHandling.AddGroupToParameterServer((int)subProduct.Group);
				DetectorType detectorType = DetectorType.DTNetSE;
				this.dbConnection.GetDetectorType(ref detectorType);
				bool flag4 = detectorType == DetectorType.DTNetDE;
				if (flag4)
				{
					this.parHandling.WriteValue(string.Concat(new string[]
					{
						Definition.filterMainPath,
						subProduct.Group.ToString(),
						DualEnergy.dualSubPath,
						"/Enabled/",
						Definition.strValue
					}), "1", false);
				}
				bool flag5 = Application.Current.Dispatcher != null;
				if (flag5)
				{
					Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
					{
						this.SelectedProduct.SubProducts.Add(subProduct);
						this.SelectedSubProduct = subProduct;
						this.OnSubProductAdded();
					}));
				}
				bool flag6 = !this.CurrentLoadedProduct.IsMultiProduct;
				if (flag6)
				{
					this.parHandling.RemoveGroupFromParameterServer((int)subProduct.Group);
				}
				LogbookManager.AddEntry(SSTEntryCode.ProductCreated, 0, firstFreeSubProductNumber, subProductName);
			}
			else
			{
				bool flag7 = Application.Current.Dispatcher != null;
				if (flag7)
				{
					Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(delegate()
					{
						Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("msgNewApplError"), DialogStyle.Error);
					}));
				}
			}
			if (showProgress)
			{
				Progress.CloseStaticProgress(false);
			}
		}

		// Token: 0x06001E34 RID: 7732 RVA: 0x000C5650 File Offset: 0x000C3850
		private void AddSubProduct(object objSubProductName)
		{
			bool flag = objSubProductName is string && this.SelectedProduct != null;
			if (flag)
			{
				bool flag2 = this.SelectedProduct.SubProducts.Count < Definition.maxSubProducts;
				if (flag2)
				{
					string subProductName = (string)objSubProductName;
					this.AddSubProduct(subProductName, true);
				}
				else
				{
					bool flag3 = Application.Current.Dispatcher != null;
					if (flag3)
					{
						Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(delegate()
						{
							Dialog.Show(LocalizationManager.Localize("txtError"), "Max. sub products: " + Definition.maxSubProducts.ToString(), DialogStyle.Error, DialogButtons.OK);
						}));
					}
				}
			}
		}

		// Token: 0x06001E35 RID: 7733 RVA: 0x000C56F0 File Offset: 0x000C38F0
		public void CopySubProduct()
		{
			bool flag = this.SelectedSubProduct != null;
			if (flag)
			{
				List<string> list = new List<string>();
				this.dbConnection.GetSubProductTablenames(this.SelectedProduct.TableName, list);
				bool flag2 = list.Count < Definition.maxSubProducts;
				if (flag2)
				{
					this.keyboard = new Keyboard(LocalizationManager.Localize("msgCopyAppl"), this.SelectedSubProduct.ProductName, KeyboardType.Application);
					this.keyboard.TextInputKeyboard += this.keyboard_TextInputKeyboard_CopySubProduct;
					this.keyboard.ShowDialog();
					ListCollectionView listCollectionView = (ListCollectionView)CollectionViewSource.GetDefaultView(this.SelectedProduct.SubProducts);
					bool flag3 = listCollectionView != null;
					if (flag3)
					{
						listCollectionView.Refresh();
					}
				}
				else
				{
					Dialog.Show(LocalizationManager.Localize("txtError"), "Max. sub products: " + Definition.maxSubProducts.ToString(), DialogStyle.Error);
				}
			}
		}

		// Token: 0x06001E36 RID: 7734 RVA: 0x000C57D8 File Offset: 0x000C39D8
		private void CopySubProduct(object objSubProductName)
		{
			bool flag = objSubProductName is string && this.SelectedProduct != null;
			if (flag)
			{
				bool flag2 = this.SelectedProduct.SubProducts.Count < Definition.maxSubProducts;
				if (flag2)
				{
					string subProductName = (string)objSubProductName;
					this.CopySubProduct(subProductName, true);
				}
				else
				{
					bool flag3 = Application.Current.Dispatcher != null;
					if (flag3)
					{
						Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(delegate()
						{
							Dialog.Show(LocalizationManager.Localize("txtError"), "Max. sub products: " + Definition.maxSubProducts.ToString(), DialogStyle.Error, DialogButtons.OK);
						}));
					}
				}
			}
		}

		// Token: 0x06001E37 RID: 7735 RVA: 0x000C5878 File Offset: 0x000C3A78
		private void CopySubProduct(string subProductName, bool showProgress)
		{
			if (showProgress)
			{
				bool flag = Application.Current.Dispatcher != null;
				if (flag)
				{
					Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
					{
						Progress.ShowStaticProgress(LocalizationManager.Localize("txtProcessing"), false);
					}));
				}
			}
			int group = 0;
			bool flag2 = subProductName.Length != 0;
			if (flag2)
			{
				List<string> list = new List<string>();
				List<string> list2 = new List<string>();
				string text = this.dbConnection.CreateNewSubProductTableName(this.SelectedProduct.TableName);
				string firstFreeSubProductNumber = this.dbConnection.GetFirstFreeSubProductNumber(this.SelectedProduct.ProductNumber);
				string text2 = text.Remove(0, text.IndexOf(Definition.subTableIdentifier) + 1);
				int.TryParse(text2, out group);
				this.parHandling.AddGroupToParameterServer(group);
				string strOldGroup = this.SelectedSubProduct.TableName.Remove(0, text.IndexOf(Definition.subTableIdentifier) + 1);
				bool flag3 = this.dbConnection.CopySubAppl(subProductName, firstFreeSubProductNumber, this.SelectedSubProduct.TableName, text, strOldGroup, text2, false);
				this.dbConnection.GetCurrentSubProductEntries(group, list, list2);
				for (int i = 0; i < list.Count; i++)
				{
					this.parHandling.WriteValue(list[i], list2[i], false);
				}
				bool flag4 = this.dbConnection.CheckIfMaterialCurveExists(this.SelectedSubProduct.TableName);
				if (flag4)
				{
					this.dbConnection.CopyMaterialCurve(this.SelectedSubProduct.TableName, text);
				}
				bool flag5 = this.dbConnection.CheckIfTestcardSimSensitivitiesExists(this.SelectedSubProduct.TableName);
				if (flag5)
				{
					this.dbConnection.CopyTestcardSimSensitivities(this.SelectedSubProduct.TableName, text);
				}
				bool flag6 = flag3;
				if (flag6)
				{
					ProductData subProduct = new ProductData(text, subProductName, firstFreeSubProductNumber);
					bool flag7 = Application.Current.Dispatcher != null;
					if (flag7)
					{
						Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
						{
							this.SelectedProduct.SubProducts.Add(subProduct);
							this.SelectedSubProduct = subProduct;
							this.OnSubProductAdded();
						}));
					}
					bool flag8 = !this.CurrentLoadedProduct.IsMultiProduct;
					if (flag8)
					{
						this.parHandling.RemoveGroupFromParameterServer(group);
					}
					LogbookManager.AddEntry(SSTEntryCode.ProductCopied, 0, firstFreeSubProductNumber, subProductName);
				}
				else
				{
					bool flag9 = Application.Current.Dispatcher != null;
					if (flag9)
					{
						Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(delegate()
						{
							Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("msgNewApplError"), DialogStyle.Error);
						}));
					}
				}
			}
			else
			{
				bool flag10 = Application.Current.Dispatcher != null;
				if (flag10)
				{
					Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(delegate()
					{
						Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("msgInsertAppName"), DialogStyle.Error);
					}));
				}
			}
			if (showProgress)
			{
				Progress.CloseStaticProgress(false);
			}
		}

		// Token: 0x06001E38 RID: 7736 RVA: 0x000C5B84 File Offset: 0x000C3D84
		private void keyboard_TextInputKeyboard_CopySubProduct(object sender, KeyboardInputEventArgs e)
		{
			this.keyboard.TextInputKeyboard -= this.keyboard_TextInputKeyboard_CopySubProduct;
			bool flag = e.Text != null && e.Text.Length > 0;
			if (flag)
			{
				new Thread(new ParameterizedThreadStart(this.CopySubProduct))
				{
					Name = "CopySubProduct"
				}.Start(e.Text);
			}
			else
			{
				Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("msgInsertAppName"), DialogStyle.Error);
			}
		}

		// Token: 0x06001E39 RID: 7737 RVA: 0x000C5C14 File Offset: 0x000C3E14
		public bool RemoveSelectedSubProduct(bool noPrompt)
		{
			Dialog.DialogResults dialogResults = Dialog.DialogResults.Ok;
			bool flag = this.SelectedSubProduct != null;
			if (flag)
			{
				bool flag2 = this.SelectedProduct.SubProducts.Count > 1;
				if (flag2)
				{
					bool flag3 = !noPrompt;
					if (flag3)
					{
						dialogResults = Dialog.Show(LocalizationManager.Localize("txtWarning"), string.Concat(new string[]
						{
							LocalizationManager.Localize("txtApplication"),
							" \"",
							this.SelectedSubProduct.ProductName,
							"\" ",
							LocalizationManager.Localize("msgGettingErased")
						}), DialogStyle.Warning, DialogButtons.OKCancel);
					}
					bool flag4 = dialogResults == Dialog.DialogResults.Ok;
					if (flag4)
					{
						this.parHandling.RemoveGroupFromParameterServer((int)this.SelectedSubProduct.Group);
						this.parHandling.RemoveRegionFromParameterServer((int)this.SelectedSubProduct.Group);
						bool flag5 = this.dbConnection.DeleteTable(this.SelectedSubProduct.TableName);
						this.dbConnection.DeleteSubStatistics(this.SelectedSubProduct.TableName);
						bool flag6 = this.dbConnection.CheckIfMaterialCurveExists(this.SelectedSubProduct.TableName);
						if (flag6)
						{
							this.dbConnection.DeleteMaterialCurve(this.SelectedSubProduct.TableName);
						}
						bool flag7 = this.dbConnection.CheckIfTestcardSimSensitivitiesExists(this.SelectedSubProduct.TableName);
						if (flag7)
						{
							this.dbConnection.DeleteTestcardSimSensitivities(this.SelectedSubProduct.TableName);
						}
						bool flag8 = flag5;
						if (flag8)
						{
							LogbookManager.AddEntry(SSTEntryCode.ProductDeleted, 0, this.SelectedSubProduct.ProductNumber, this.SelectedSubProduct.ProductName);
							this.SelectedProduct.SubProducts.Remove(this.SelectedSubProduct);
							this.OnSubProductRemoved();
							return true;
						}
						Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("msgDeleteApplError"), DialogStyle.Error);
					}
				}
				else
				{
					Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("msgDeleteApplError"), DialogStyle.Error);
				}
			}
			return false;
		}

		// Token: 0x06001E3A RID: 7738 RVA: 0x000C5E10 File Offset: 0x000C4010
		public bool LearnSubProduct()
		{
			MachineType machineType = MachineType.MachineTypeUndefined;
			this.dbConnection.GetMachineType(ref machineType);
			DetectorType detectorType = DetectorType.Unknown;
			this.dbConnection.GetDetectorType(ref detectorType);
			bool result = false;
			bool flag = Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtOverwriteExisting"), DialogStyle.Warning, DialogButtons.OKCancel) == Dialog.DialogResults.Ok;
			if (flag)
			{
				result = true;
				int num = 0;
				this.dbConnection.GetNAutolearn(ref num);
				this.parHandling.WriteValue(Definition.recognitionLearnedIndexPath, this.SelectedSubProduct.Group.ToString(), false);
				this.parHandling.WriteValue(Definition.recognitionNLearnPath, num.ToString(), false);
				bool flag2 = false;
				this.dbConnection.GetMultiRegionEnabled(ref flag2);
				bool flag3 = false;
				bool flag4 = detectorType == DetectorType.Sesotec;
				if (flag4)
				{
					flag3 = (this.parHandling.ReadPar(Definition.detectorSesotecPixelSizePath) == "0.4");
				}
				else
				{
					bool flag5 = detectorType == DetectorType.DTNetTDI;
					if (flag5)
					{
						flag3 = (this.parHandling.ReadPar(Definition.detectorDtPixelSizePath) == "0.4");
					}
				}
				bool flag6 = true;
				this.dbConnection.GetMXFeatures04Det(ref flag6);
				string a = "";
				this.dbConnection.GetXRaySourceModelNumber(ref a);
				bool flag7 = a == "IXS050SE100P214" && flag3;
				bool flag8 = machineType == MachineType.RayconDPlus && (flag3 || (flag7 && !flag6)) && !flag2;
				if (flag8)
				{
					this.ctrlAutotrainAdjustments = new CtrlAutotrainAdjustments(this.parHandling, this.imageTransfer, this.dbConnection, this.accessManager, this.machineState, this, (int)this.SelectedSubProduct.Group, this.imageArchiver, this.testcardSimService, this.auditSettings);
					bool flag9 = Subwindow.ShowAutotrainAdjustments(this.ctrlAutotrainAdjustments, SubwindowButtons.ConfirmAndCancel, null, true, this.parHandling, this, this.dbConnection) == Subwindow.AdjustmentResults.Ok;
					if (flag9)
					{
						this.StartAutotrainMainMenuSubProduct();
					}
				}
				else
				{
					this.StartAutotrainMainMenuSubProduct();
				}
			}
			return result;
		}

		// Token: 0x06001E3B RID: 7739 RVA: 0x000C6008 File Offset: 0x000C4208
		private void StartAutotrainMainMenuSubProduct()
		{
			this.ctrlAutotrain = new AutotrainControl(this.imageTransfer, this.parHandling, this.dbConnection, AutolearnType.Contamination, this.machineState, this, (int)this.SelectedSubProduct.Group, null, true, false, this.accessManager, null, null);
			ProductManager.subwindowAutotrainMain = new Subwindow(this.ctrlAutotrain, SubwindowButtons.Autotrain, true, false, null, this.parHandling, this, this.dbConnection);
			ProductManager.subwindowAutotrainMain.PanelButtons = this.ctrlAutotrain.Buttons;
			HomeManager.Instance.Register(ProductManager.subwindowAutotrainMain);
			ProductManager.subwindowAutotrainMain.Show();
		}

		// Token: 0x06001E3C RID: 7740 RVA: 0x000C60A4 File Offset: 0x000C42A4
		public void OpenFilter()
		{
			FilterControl usrControl = new FilterControl(this.parHandling, this.imageTransfer, this.dbConnection, this.accessManager, this.machineState, this, (int)this.SelectedSubProduct.Group, this.imageArchiver, this.testcardSimService, this.auditSettings);
			Subwindow.Show(usrControl, SubwindowButtons.BackAndHome);
		}

		// Token: 0x06001E3D RID: 7741 RVA: 0x000C60FC File Offset: 0x000C42FC
		public void RenameSelectedSubProduct()
		{
			this.RenameSelectedSubProduct("msgRenameAppl");
		}

		// Token: 0x06001E3E RID: 7742 RVA: 0x000C610C File Offset: 0x000C430C
		public void RenameSelectedSubProduct(string localizationKey)
		{
			bool flag = this.SelectedSubProduct != null;
			if (flag)
			{
				this.keyboard = new Keyboard(LocalizationManager.Localize(localizationKey), this.SelectedSubProduct.ProductName, KeyboardType.Application);
				this.keyboard.TextInputKeyboard += this.keyboard_TextInputKeyboard_RenameSubProduct;
				this.keyboard.ShowDialog();
			}
		}

		// Token: 0x06001E3F RID: 7743 RVA: 0x000C616C File Offset: 0x000C436C
		private void keyboard_TextInputKeyboard_RenameSubProduct(object sender, KeyboardInputEventArgs e)
		{
			string text = e.Text;
			bool flag = this.keyboard != null;
			if (flag)
			{
				this.keyboard.TextInputKeyboard -= this.keyboard_TextInputKeyboard_RenameProduct;
			}
			bool flag2 = text.Length != 0;
			if (flag2)
			{
				bool flag3 = text != "";
				if (flag3)
				{
					bool flag4 = this.dbConnection.RenameAppl(this.selectedSubProduct.TableName, text);
					bool flag5 = flag4;
					if (flag5)
					{
						LogbookManager.AddEntry(SSTEntryCode.ProductRenamed, 0, this.selectedSubProduct.ProductNumber, this.selectedSubProduct.ProductName, text);
						this.selectedSubProduct.ProductName = text;
						this.OnSubProductRenamed();
					}
					else
					{
						Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("msgRenameApplError"), DialogStyle.Error);
					}
				}
			}
			else
			{
				Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("msgInsertAppName"), DialogStyle.Error);
			}
		}

		// Token: 0x06001E40 RID: 7744 RVA: 0x000C6260 File Offset: 0x000C4460
		public void RelearnGroup(XRSImage image)
		{
			bool flag = Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtOverwriteExisting"), DialogStyle.Warning, DialogButtons.OKCancel) == Dialog.DialogResults.Ok;
			if (flag)
			{
				int num = 0;
				string text = this.SelectedSubProduct.TableName.Remove(0, this.SelectedSubProduct.TableName.IndexOf(Definition.subTableIdentifier) + 1);
				int.TryParse(text, out num);
				Progress.ShowStaticProgress("Retraining...", false);
				this.parHandling.WriteValue(Definition.filterMainPath + text + Definition.relearnGroupSubPath, "1", false);
				this.imageTransfer.FrameArrived += this.imageTransfer_FrameArrived;
				this.imageTransfer.SendImage(image);
			}
		}

		// Token: 0x06001E41 RID: 7745 RVA: 0x000C631C File Offset: 0x000C451C
		private void imageTransfer_FrameArrived(object sender, ReceiveEventArgs e)
		{
			bool flag = e.Image.Type == ImageType.Demo;
			if (flag)
			{
				this.imageTransfer.FrameArrived -= this.imageTransfer_FrameArrived;
				string a = this.SelectedSubProduct.TableName.Remove(0, this.SelectedSubProduct.TableName.IndexOf(Definition.subTableIdentifier) + 1);
				Filter.UpdateImageProcessingFilterPaths(this.dbConnection, this.parHandling, (int)e.Image.Group, this, true);
				bool flag2 = a == e.Image.Group.ToString();
				if (flag2)
				{
					LogbookManager.AddEntry(SSTEntryCode.RetrainGroup, 0, this.CurrentLoadedProduct.ProductNumber, this.CurrentLoadedProduct.ProductName);
					Progress.CloseStaticProgress(true);
					this.OnRetrainGroupSucceeded();
				}
				else
				{
					LogbookManager.AddEntry(SSTEntryCode.RetrainGroupFailed, 0, this.CurrentLoadedProduct.ProductNumber, this.CurrentLoadedProduct.ProductName);
					Progress.CloseStaticProgress(false);
					bool flag3 = Application.Current.Dispatcher != null;
					if (flag3)
					{
						Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(delegate()
						{
							Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtError"), DialogStyle.Error, DialogButtons.None, true);
						}));
					}
				}
			}
		}

		// Token: 0x06001E42 RID: 7746 RVA: 0x000C6464 File Offset: 0x000C4664
		protected virtual void OnRetrainGroupSucceeded()
		{
			Action retrainGroupSucceeded = this.RetrainGroupSucceeded;
			bool flag = retrainGroupSucceeded != null;
			if (flag)
			{
				retrainGroupSucceeded();
			}
		}

		// Token: 0x06001E43 RID: 7747 RVA: 0x000C6488 File Offset: 0x000C4688
		protected virtual void OnSubProductRenamed()
		{
			Action subProductRenamed = this.SubProductRenamed;
			bool flag = subProductRenamed != null;
			if (flag)
			{
				subProductRenamed();
			}
		}

		// Token: 0x06001E44 RID: 7748 RVA: 0x000C64AC File Offset: 0x000C46AC
		protected virtual void OnSubProductAdded()
		{
			Action subProductAdded = this.SubProductAdded;
			bool flag = subProductAdded != null;
			if (flag)
			{
				subProductAdded();
			}
		}

		// Token: 0x06001E45 RID: 7749 RVA: 0x000C64D0 File Offset: 0x000C46D0
		protected virtual void OnSubProductRemoved()
		{
			Action subProductRemoved = this.SubProductRemoved;
			bool flag = subProductRemoved != null;
			if (flag)
			{
				subProductRemoved();
			}
		}

		// Token: 0x06001E46 RID: 7750 RVA: 0x000C64F4 File Offset: 0x000C46F4
		protected virtual void OnBatchChanged()
		{
			Action batchChanged = this.BatchChanged;
			bool flag = batchChanged != null;
			if (flag)
			{
				batchChanged();
			}
		}

		// Token: 0x06001E47 RID: 7751 RVA: 0x000C6518 File Offset: 0x000C4718
		public void CreateNewLaneProduct()
		{
			this.keyboard = new Keyboard(LocalizationManager.Localize("msgCreateAppl"), "", KeyboardType.Application);
			this.keyboard.TextInputKeyboard += this.keyboard_TextInputKeyboard_NewLaneProduct;
			this.keyboard.ShowDialog();
			this.ViewProducts.Refresh();
		}

		// Token: 0x06001E48 RID: 7752 RVA: 0x000C6574 File Offset: 0x000C4774
		private void keyboard_TextInputKeyboard_NewLaneProduct(object sender, KeyboardInputEventArgs e)
		{
			this.keyboard.TextInputKeyboard -= this.keyboard_TextInputKeyboard_NewLaneProduct;
			Action <>9__5;
			new Thread(delegate()
			{
				bool flag = e.Text.Length != 0;
				if (flag)
				{
					bool flag2 = Application.Current.Dispatcher != null;
					if (flag2)
					{
						Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
						{
							Progress.ShowStaticProgress(LocalizationManager.Localize("txtProcessing"), false);
						}));
					}
					int firstFreeProductNumber = this.dbConnection.GetFirstFreeProductNumber();
					string text = this.dbConnection.CreateNewProductTableName();
					bool flag3 = this.dbConnection.CreateNewAppl(e.Text, firstFreeProductNumber.ToString(), ProductType.LaneProduct, text);
					bool flag4 = flag3;
					if (flag4)
					{
						this.EnableOnlyLaneRecognition(text);
						ProductData productToChangeTo = new ProductData(text, e.Text, firstFreeProductNumber.ToString());
						productToChangeTo.ProductType = ProductType.LaneProduct;
						bool flag5 = Application.Current.Dispatcher != null;
						if (flag5)
						{
							Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
							{
								this.ProductList.Add(productToChangeTo);
								this.SelectedProduct = productToChangeTo;
							}));
						}
						this.AddSubLaneProducts(productToChangeTo.TableName);
						LogbookManager.AddEntry(SSTEntryCode.ProductCreated, 0, this.SelectedProduct.ProductNumber, this.SelectedProduct.ProductName);
						Progress.CloseStaticProgress(false);
						bool flag6 = Application.Current.Dispatcher != null;
						if (flag6)
						{
							Dispatcher dispatcher = Application.Current.Dispatcher;
							DispatcherPriority priority = DispatcherPriority.Normal;
							Action method;
							if ((method = <>9__5) == null)
							{
								method = (<>9__5 = delegate()
								{
									this.ChangeProduct(false);
								});
							}
							dispatcher.Invoke(priority, method);
						}
					}
					else
					{
						bool flag7 = Application.Current.Dispatcher != null;
						if (flag7)
						{
							Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
							{
								Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("msgNewApplError"), DialogStyle.Error);
							}));
						}
					}
				}
				else
				{
					bool flag8 = Application.Current.Dispatcher != null;
					if (flag8)
					{
						Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
						{
							Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("msgInsertAppName"), DialogStyle.Error);
						}));
					}
				}
			})
			{
				Name = "CreateNewLane"
			}.Start();
		}

		// Token: 0x06001E49 RID: 7753 RVA: 0x000C65D4 File Offset: 0x000C47D4
		private void EnableOnlyLaneRecognition(string tableName)
		{
			List<string> list = new List<string>();
			for (int i = 0; i < Definition.recognitionEnablePaths.Length; i++)
			{
				list.Add("0");
			}
			this.dbConnection.UpdateTable(tableName, Definition.recognitionEnablePaths.ToList<string>(), list);
			this.dbConnection.UpdateTable(tableName, Definition.recognitionEnableLanePath, "1");
		}

		// Token: 0x06001E4A RID: 7754 RVA: 0x000C663C File Offset: 0x000C483C
		private void AddSubLaneProducts(string mainTableName)
		{
			for (int i = 1; i < Enum.GetNames(typeof(Lane)).Length; i++)
			{
				this.AddSubProduct("", false);
				this.SelectedSubProduct.Lane = (Lane)i;
				this.SelectedSubProduct.ProductNumber = "";
				this.SelectedSubProduct.SetLaneActiveFromDB(mainTableName, this.dbConnection);
				this.dbConnection.SetProductNumber(this.SelectedSubProduct.TableName, "");
				this.dbConnection.UpdateTable(this.SelectedSubProduct.TableName, Definition.filterMainPath + i.ToString() + Definition.propertiesSubPath + "/Lane/Value", i.ToString());
			}
		}

		// Token: 0x06001E4B RID: 7755 RVA: 0x000C6705 File Offset: 0x000C4905
		private void SetSubLaneProductNameOrNumber(ProductData renamedProduct)
		{
			this.SetSubLaneProductNameOrNumber(renamedProduct.TableName, renamedProduct.ProductName, renamedProduct.ProductNumber);
		}

		// Token: 0x06001E4C RID: 7756 RVA: 0x000C6724 File Offset: 0x000C4924
		private void SetSubLaneProductNameOrNumber(string tableName, string productName, string productNumber)
		{
			string a = null;
			foreach (ProductData productData in this.ProductList)
			{
				bool isLaneProduct = productData.IsLaneProduct;
				if (isLaneProduct)
				{
					foreach (ProductData productData2 in productData.SubProducts)
					{
						this.dbConnection.GetParameterValue(Definition.refTableNamePath, productData2.TableName, ref a);
						bool flag = a == tableName;
						if (flag)
						{
							this.dbConnection.RenameAppl(productData2.TableName, productName);
							this.dbConnection.SetProductNumber(productData2.TableName, productNumber);
							productData2.ProductName = productName;
							productData2.ProductNumber = productNumber;
						}
					}
				}
			}
		}

		// Token: 0x06001E4D RID: 7757 RVA: 0x000C6828 File Offset: 0x000C4A28
		public void SetLaneActiveOfLaneProducts(Lane lane, bool active)
		{
			bool isLaneProduct = this.CurrentLoadedProduct.IsLaneProduct;
			if (isLaneProduct)
			{
				foreach (ProductData productData in this.CurrentLoadedProduct.SubProducts)
				{
					bool flag = productData.Lane == lane;
					if (flag)
					{
						productData.LaneActive = active;
					}
				}
			}
		}

		// Token: 0x06001E4E RID: 7758 RVA: 0x000C68A0 File Offset: 0x000C4AA0
		public string SetMainPathToSubPath(string mainPath, int group)
		{
			return this.SetToSubOrMainPath(mainPath, group.ToString());
		}

		// Token: 0x06001E4F RID: 7759 RVA: 0x000C68C0 File Offset: 0x000C4AC0
		public string SetSubPathToMainPath(string subPath)
		{
			return this.SetToSubOrMainPath(subPath, "");
		}

		// Token: 0x06001E50 RID: 7760 RVA: 0x000C68E0 File Offset: 0x000C4AE0
		private string SetToSubOrMainPath(string subOrMainPath, string strGroup)
		{
			for (int i = 0; i < 2; i++)
			{
				int num = subOrMainPath.IndexOf('/');
				bool flag = num >= 0;
				if (flag)
				{
					subOrMainPath = subOrMainPath.Remove(0, num + 1);
				}
			}
			bool flag2 = !this.subLanePathsWithoutSlashes.Contains(subOrMainPath);
			if (flag2)
			{
				subOrMainPath = Definition.filterMainPath + strGroup + "/" + subOrMainPath;
			}
			return subOrMainPath;
		}

		// Token: 0x06001E51 RID: 7761 RVA: 0x000C6950 File Offset: 0x000C4B50
		private int GetGroupFromPath(string path)
		{
			int result = 0;
			int num = path.IndexOf('/');
			bool flag = num >= 0;
			if (flag)
			{
				path = path.Remove(0, num + 1);
			}
			num = path.IndexOf('/');
			bool flag2 = num >= 2;
			if (flag2)
			{
				path = path.Remove(0, num - 1);
			}
			int.TryParse(path[0].ToString(), out result);
			return result;
		}

		// Token: 0x06001E52 RID: 7762 RVA: 0x000C69C4 File Offset: 0x000C4BC4
		public void UpdateAssignedSubLaneProducts(string mainPath, string newValue)
		{
			string b = "";
			bool flag = (from product in this.ProductList
			where product.IsLaneProduct
			select product).ToList<ProductData>().Count > 0;
			if (flag)
			{
				foreach (ProductData productData in this.ProductList)
				{
					bool isLaneProduct = productData.IsLaneProduct;
					if (isLaneProduct)
					{
						foreach (ProductData productData2 in productData.SubProducts)
						{
							this.dbConnection.GetParameterValue(Definition.refTableNamePath, productData2.TableName, ref b);
							bool flag2 = this.CurrentLoadedProduct.TableName == b;
							if (flag2)
							{
								bool flag3 = mainPath.StartsWith(Definition.filterMainPath);
								if (flag3)
								{
									mainPath = this.SetMainPathToSubPath(mainPath, (int)productData2.Group);
								}
								this.dbConnection.UpdateTable(productData2.TableName, mainPath, newValue);
							}
						}
					}
				}
			}
		}

		// Token: 0x06001E53 RID: 7763 RVA: 0x000C6B18 File Offset: 0x000C4D18
		public void UpdateAssignedMainProduct(string subPath, string newValue)
		{
			int groupFromPath = this.GetGroupFromPath(subPath);
			this.UpdateAssignedMainProduct(subPath, newValue, groupFromPath);
		}

		// Token: 0x06001E54 RID: 7764 RVA: 0x000C6B3C File Offset: 0x000C4D3C
		public void UpdateAssignedMainProduct(string subPath, string newValue, int group)
		{
			string a = "";
			bool flag = group > 0;
			if (flag)
			{
				this.dbConnection.GetParameterValue(Definition.refTableNamePath, this.currentLoadedProduct.TableName + Definition.subTableIdentifier.ToString() + group.ToString(), ref a);
				foreach (ProductData productData in this.ProductList)
				{
					bool isSingleProduct = productData.IsSingleProduct;
					if (isSingleProduct)
					{
						bool flag2 = a == productData.TableName;
						if (flag2)
						{
							bool flag3 = subPath.StartsWith(Definition.filterMainPath);
							if (flag3)
							{
								subPath = this.SetSubPathToMainPath(subPath);
							}
							this.dbConnection.UpdateTable(productData.TableName, subPath, newValue);
						}
					}
				}
			}
		}

		// Token: 0x06001E55 RID: 7765 RVA: 0x000C6C28 File Offset: 0x000C4E28
		public void UpdateAssignedProductsFilterPaths(List<string> changedPaths, List<string> changedValues, int group)
		{
			string text = "";
			bool isSingleProduct = this.currentLoadedProduct.IsSingleProduct;
			if (isSingleProduct)
			{
				bool flag = (from product in this.ProductList
				where product.IsLaneProduct
				select product).ToList<ProductData>().Count > 0;
				if (flag)
				{
					foreach (ProductData productData in this.ProductList)
					{
						bool isLaneProduct = productData.IsLaneProduct;
						if (isLaneProduct)
						{
							foreach (ProductData productData2 in productData.SubProducts)
							{
								this.dbConnection.GetParameterValue(Definition.refTableNamePath, productData2.TableName, ref text);
								bool flag2 = this.CurrentLoadedProduct.TableName == text;
								if (flag2)
								{
									for (int i = 0; i < changedPaths.Count; i++)
									{
										bool flag3 = changedPaths[i].StartsWith(Definition.filterMainPath);
										if (flag3)
										{
											changedPaths[i] = this.SetMainPathToSubPath(changedPaths[i], (int)productData2.Group);
										}
									}
									this.dbConnection.UpdateTable(productData2.TableName, changedPaths, changedValues);
								}
							}
						}
					}
				}
			}
			else
			{
				bool isLaneProduct2 = this.currentLoadedProduct.IsLaneProduct;
				if (isLaneProduct2)
				{
					bool flag4 = group > 0;
					if (flag4)
					{
						this.dbConnection.GetParameterValue(Definition.refTableNamePath, this.currentLoadedProduct.TableName + Definition.subTableIdentifier.ToString() + group.ToString(), ref text);
						foreach (ProductData productData3 in this.ProductList)
						{
							bool isSingleProduct2 = productData3.IsSingleProduct;
							if (isSingleProduct2)
							{
								bool flag5 = text == productData3.TableName;
								if (flag5)
								{
									for (int j = 0; j < changedPaths.Count; j++)
									{
										bool flag6 = changedPaths[j].StartsWith(Definition.filterMainPath);
										if (flag6)
										{
											changedPaths[j] = this.SetSubPathToMainPath(changedPaths[j]);
										}
									}
									this.dbConnection.UpdateTable(productData3.TableName, changedPaths, changedValues);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06001E56 RID: 7766 RVA: 0x000C6F18 File Offset: 0x000C5118
		public void CreateNewRegionProduct()
		{
			this.keyboard = new Keyboard(LocalizationManager.Localize("msgCreateAppl"), "", KeyboardType.Application);
			this.keyboard.TextInputKeyboard += this.keyboard_TextInputKeyboard_NewRegion;
			this.keyboard.ShowDialog();
			this.ViewProducts.Refresh();
		}

		// Token: 0x06001E57 RID: 7767 RVA: 0x000C6F74 File Offset: 0x000C5174
		private void keyboard_TextInputKeyboard_NewRegion(object sender, KeyboardInputEventArgs e)
		{
			this.keyboard.TextInputKeyboard -= this.keyboard_TextInputKeyboard_NewRegion;
			Action <>9__5;
			new Thread(delegate()
			{
				bool flag = e.Text.Length != 0;
				if (flag)
				{
					bool flag2 = Application.Current.Dispatcher != null;
					if (flag2)
					{
						Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
						{
							Progress.ShowStaticProgress(LocalizationManager.Localize("txtProcessing"), false);
						}));
					}
					int firstFreeProductNumber = this.dbConnection.GetFirstFreeProductNumber();
					string text = this.dbConnection.CreateNewProductTableName();
					bool flag3 = this.dbConnection.CreateNewAppl(e.Text, firstFreeProductNumber.ToString(), ProductType.RegionProduct, text);
					bool flag4 = flag3;
					if (flag4)
					{
						ProductData productToChangeTo = new ProductData(text, e.Text, firstFreeProductNumber.ToString());
						productToChangeTo.ProductType = ProductType.RegionProduct;
						bool flag5 = Application.Current.Dispatcher != null;
						if (flag5)
						{
							Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
							{
								this.ProductList.Add(productToChangeTo);
								this.SelectedProduct = productToChangeTo;
							}));
						}
						this.AddRegionProduct("Region 1", false, XRSRegionType.Rectangle);
						LogbookManager.AddEntry(SSTEntryCode.ProductCreated, 0, this.SelectedProduct.ProductNumber, this.SelectedProduct.ProductName);
						Progress.CloseStaticProgress(false);
						bool flag6 = Application.Current.Dispatcher != null;
						if (flag6)
						{
							Dispatcher dispatcher = Application.Current.Dispatcher;
							DispatcherPriority priority = DispatcherPriority.Normal;
							Action method;
							if ((method = <>9__5) == null)
							{
								method = (<>9__5 = delegate()
								{
									this.ChangeProduct(false);
								});
							}
							dispatcher.Invoke(priority, method);
						}
					}
					else
					{
						bool flag7 = Application.Current.Dispatcher != null;
						if (flag7)
						{
							Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
							{
								Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("msgNewApplError"), DialogStyle.Error);
							}));
						}
					}
				}
				else
				{
					bool flag8 = Application.Current.Dispatcher != null;
					if (flag8)
					{
						Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
						{
							Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("msgInsertAppName"), DialogStyle.Error);
						}));
					}
				}
			})
			{
				Name = "CreateNewRegion"
			}.Start();
		}

		// Token: 0x06001E58 RID: 7768 RVA: 0x000C6FD4 File Offset: 0x000C51D4
		public void CreateNewBulkObjectProduct()
		{
			this.keyboard = new Keyboard(LocalizationManager.Localize("msgCreateAppl"), "", KeyboardType.Application);
			this.keyboard.TextInputKeyboard += this.keyboard_TextInputKeyboard_NewBulkObject;
			this.keyboard.ShowDialog();
			this.ViewProducts.Refresh();
		}

		// Token: 0x06001E59 RID: 7769 RVA: 0x000C7030 File Offset: 0x000C5230
		private void keyboard_TextInputKeyboard_NewBulkObject(object sender, KeyboardInputEventArgs e)
		{
			this.keyboard.TextInputKeyboard -= this.keyboard_TextInputKeyboard_NewBulkObject;
			bool flag = e.Text.Length != 0;
			if (flag)
			{
				int firstFreeProductNumber = this.dbConnection.GetFirstFreeProductNumber();
				string text = this.dbConnection.CreateNewProductTableName();
				bool flag2 = this.dbConnection.CreateNewAppl(e.Text, firstFreeProductNumber.ToString(), ProductType.BulkObjectProduct, text);
				bool flag3 = flag2;
				if (flag3)
				{
					ProductData productData = new ProductData(text, e.Text, firstFreeProductNumber.ToString());
					productData.ProductType = ProductType.BulkObjectProduct;
					BulkMode.InitializeBulkObjectProduct(this.dbConnection, this.parHandling, text);
					this.ProductList.Add(productData);
					this.SelectedProduct = productData;
					LogbookManager.AddEntry(SSTEntryCode.ProductCreated, 0, this.SelectedProduct.ProductNumber, this.SelectedProduct.ProductName);
					this.ChangeProduct(false);
				}
				else
				{
					Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("msgNewApplError"), DialogStyle.Error);
				}
			}
			else
			{
				Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("msgInsertAppName"), DialogStyle.Error);
			}
		}

		// Token: 0x06001E5A RID: 7770 RVA: 0x000C7158 File Offset: 0x000C5358
		public byte AddRegionProduct(string subProductName, bool showProgress, XRSRegionType regionType)
		{
			byte group = byte.MaxValue;
			if (showProgress)
			{
				bool flag = Application.Current.Dispatcher != null;
				if (flag)
				{
					Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
					{
						Progress.ShowStaticProgress(LocalizationManager.Localize("txtProcessing"), false);
					}));
				}
			}
			string text = this.dbConnection.CreateNewSubProductTableName(this.SelectedProduct.TableName);
			string firstFreeSubProductNumber = this.dbConnection.GetFirstFreeSubProductNumber(this.SelectedProduct.ProductNumber);
			ProductData subProduct = new ProductData(text, subProductName, firstFreeSubProductNumber);
			this.parHandling.AddGroupToParameterServer((int)subProduct.Group);
			this.parHandling.AddRegionToParameterServer((int)subProduct.Group);
			bool flag2 = this.dbConnection.CreateNewSubAppl(subProductName, firstFreeSubProductNumber, text, subProduct.Group.ToString());
			this.SetRegionDefaultValues(subProduct, regionType);
			subProduct.GetRegion(this.dbConnection);
			bool flag3 = flag2;
			if (flag3)
			{
				bool flag4 = Application.Current.Dispatcher != null;
				if (flag4)
				{
					Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
					{
						group = subProduct.Group;
						this.SelectedProduct.SubProducts.Add(subProduct);
						this.SelectedSubProduct = subProduct;
						this.OnSubProductAdded();
					}));
				}
				bool flag5 = this.CurrentLoadedProduct.IsSingleProduct || (from product in this.CurrentLoadedProduct.SubProducts
				where product.Group != subProduct.Group
				select product).Count<ProductData>() == 0;
				if (flag5)
				{
					this.parHandling.RemoveGroupFromParameterServer((int)subProduct.Group);
					this.parHandling.RemoveRegionFromParameterServer((int)subProduct.Group);
				}
				LogbookManager.AddEntry(SSTEntryCode.ProductCreated, 0, firstFreeSubProductNumber, subProductName);
			}
			else
			{
				bool flag6 = Application.Current.Dispatcher != null;
				if (flag6)
				{
					Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(delegate()
					{
						Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("msgNewApplError"), DialogStyle.Error);
					}));
				}
			}
			if (showProgress)
			{
				Progress.CloseStaticProgress(false);
			}
			return group;
		}

		// Token: 0x06001E5B RID: 7771 RVA: 0x000C7398 File Offset: 0x000C5598
		public void PrepareRegionImage(XRSImage copiedImage, int group)
		{
			bool flag = this.CurrentLoadedProduct != null && this.CurrentLoadedProduct.IsRegionProduct && group > 0;
			if (flag)
			{
				copiedImage.Group = (short)group;
				copiedImage.ObjectCount = copiedImage.Regions[group - 1].ObjectCount;
				copiedImage.Weight = copiedImage.Regions[group - 1].Weight;
			}
		}

		// Token: 0x06001E5C RID: 7772 RVA: 0x000C7400 File Offset: 0x000C5600
		private void SetRegionDefaultValues(ProductData subProduct, XRSRegionType regionType)
		{
			DBConnection dbconnection = this.dbConnection;
			string tableName = subProduct.TableName;
			string path = Definition.filterMainPath + subProduct.Group.ToString() + Definition.regionTypeSubPath;
			int num = (int)regionType;
			dbconnection.UpdateTable(tableName, path, num.ToString());
			bool flag = regionType == XRSRegionType.Rectangle || regionType == XRSRegionType.Ellipse;
			if (flag)
			{
				bool flag2 = this.selectedProduct == this.currentLoadedProduct;
				if (flag2)
				{
					ParameterHandling parameterHandling = this.parHandling;
					string path2 = Definition.filterMainPath + subProduct.Group.ToString() + Definition.regionTypeSubPath;
					num = (int)regionType;
					parameterHandling.WriteValue(path2, num.ToString(), false);
					this.parHandling.WriteValue(Definition.filterMainPath + subProduct.Group.ToString() + Definition.vertex1XSubPath, "0", false);
					this.parHandling.WriteValue(Definition.filterMainPath + subProduct.Group.ToString() + Definition.vertex1YSubPath, "0", false);
					this.parHandling.WriteValue(Definition.filterMainPath + subProduct.Group.ToString() + Definition.vertex2XSubPath, 10240.ToString(), false);
					this.parHandling.WriteValue(Definition.filterMainPath + subProduct.Group.ToString() + Definition.vertex2YSubPath, 10240.ToString(), false);
				}
				this.dbConnection.UpdateTable(subProduct.TableName, Definition.filterMainPath + subProduct.Group.ToString() + Definition.vertex1XSubPath, "0");
				this.dbConnection.UpdateTable(subProduct.TableName, Definition.filterMainPath + subProduct.Group.ToString() + Definition.vertex1YSubPath, "0");
				this.dbConnection.UpdateTable(subProduct.TableName, Definition.filterMainPath + subProduct.Group.ToString() + Definition.vertex2XSubPath, 10240.ToString());
				this.dbConnection.UpdateTable(subProduct.TableName, Definition.filterMainPath + subProduct.Group.ToString() + Definition.vertex2YSubPath, 10240.ToString());
			}
			else
			{
				bool flag3 = regionType == XRSRegionType.Polygon;
				if (flag3)
				{
					bool flag4 = this.selectedProduct == this.currentLoadedProduct;
					if (flag4)
					{
						ParameterHandling parameterHandling2 = this.parHandling;
						string path3 = Definition.filterMainPath + subProduct.Group.ToString() + Definition.regionTypeSubPath;
						num = (int)regionType;
						parameterHandling2.WriteValue(path3, num.ToString());
						this.parHandling.WriteValue(Definition.filterMainPath + subProduct.Group.ToString() + Definition.vertex1XSubPath, "0", false);
						this.parHandling.WriteValue(Definition.filterMainPath + subProduct.Group.ToString() + Definition.vertex1YSubPath, "0", false);
						this.parHandling.WriteValue(Definition.filterMainPath + subProduct.Group.ToString() + Definition.vertex2XSubPath, "0", false);
						this.parHandling.WriteValue(Definition.filterMainPath + subProduct.Group.ToString() + Definition.vertex2YSubPath, 6144.ToString(), false);
						this.parHandling.WriteValue(Definition.filterMainPath + subProduct.Group.ToString() + Definition.vertex3XSubPath, 10240.ToString(), false);
						this.parHandling.WriteValue(Definition.filterMainPath + subProduct.Group.ToString() + Definition.vertex3YSubPath, 6144.ToString(), false);
						this.parHandling.WriteValue(Definition.filterMainPath + subProduct.Group.ToString() + Definition.vertex4XSubPath, 10240.ToString(), false);
						this.parHandling.WriteValue(Definition.filterMainPath + subProduct.Group.ToString() + Definition.vertex4YSubPath, "0", false);
					}
					this.dbConnection.UpdateTable(subProduct.TableName, Definition.filterMainPath + subProduct.Group.ToString() + Definition.vertex1XSubPath, "0");
					this.dbConnection.UpdateTable(subProduct.TableName, Definition.filterMainPath + subProduct.Group.ToString() + Definition.vertex1YSubPath, "0");
					this.dbConnection.UpdateTable(subProduct.TableName, Definition.filterMainPath + subProduct.Group.ToString() + Definition.vertex2XSubPath, "0");
					this.dbConnection.UpdateTable(subProduct.TableName, Definition.filterMainPath + subProduct.Group.ToString() + Definition.vertex2YSubPath, 6144.ToString());
					this.dbConnection.UpdateTable(subProduct.TableName, Definition.filterMainPath + subProduct.Group.ToString() + Definition.vertex3XSubPath, 10240.ToString());
					this.dbConnection.UpdateTable(subProduct.TableName, Definition.filterMainPath + subProduct.Group.ToString() + Definition.vertex3YSubPath, 6144.ToString());
					this.dbConnection.UpdateTable(subProduct.TableName, Definition.filterMainPath + subProduct.Group.ToString() + Definition.vertex4XSubPath, 10240.ToString());
					this.dbConnection.UpdateTable(subProduct.TableName, Definition.filterMainPath + subProduct.Group.ToString() + Definition.vertex4YSubPath, "0");
				}
			}
		}

		// Token: 0x06001E5D RID: 7773 RVA: 0x000C7A08 File Offset: 0x000C5C08
		public void EditSelectedRegion(XRSRegionType newRegionType)
		{
			bool flag = this.selectedSubProduct != null;
			if (flag)
			{
				bool flag2 = this.selectedProduct == this.currentLoadedProduct;
				int num;
				if (flag2)
				{
					ParameterHandling parameterHandling = this.parHandling;
					string path = Definition.filterMainPath + this.selectedSubProduct.Group.ToString() + Definition.regionTypeSubPath;
					num = (int)newRegionType;
					parameterHandling.WriteValue(path, num.ToString(), false);
				}
				DBConnection dbconnection = this.dbConnection;
				string tableName = this.selectedSubProduct.TableName;
				string path2 = Definition.filterMainPath + this.selectedSubProduct.Group.ToString() + Definition.regionTypeSubPath;
				num = (int)newRegionType;
				dbconnection.UpdateTable(tableName, path2, num.ToString());
				this.selectedSubProduct.Region.Type = newRegionType;
				this.selectedSubProduct.Region = this.selectedSubProduct.Region;
			}
		}

		// Token: 0x06001E5E RID: 7774 RVA: 0x000C7AE0 File Offset: 0x000C5CE0
		public void UpdateCurrentBatchNames()
		{
			foreach (ProductData productData in this.ProductList)
			{
				productData.SetCurrentBatchName(this.dbConnection);
			}
		}

		// Token: 0x06001E5F RID: 7775 RVA: 0x000C7B38 File Offset: 0x000C5D38
		private void DeleteBatchTable(string productTableName)
		{
			this.dbConnection.DeleteBatchTable(productTableName);
		}

		// Token: 0x06001E60 RID: 7776 RVA: 0x000C7B48 File Offset: 0x000C5D48
		public void ShowBatchWindow(bool showNewBatchKeyboard)
		{
			CtrlBatches ctrlBatches = new CtrlBatches(this.dbConnection, this, this.imageTransfer, showNewBatchKeyboard);
			Subwindow.Show(ctrlBatches, SubwindowButtons.BackAndHome, ctrlBatches.Buttons);
		}

		// Token: 0x06001E61 RID: 7777 RVA: 0x000C7B78 File Offset: 0x000C5D78
		public void BatchHasChanged()
		{
			this.OnBatchChanged();
		}

		// Token: 0x06001E62 RID: 7778 RVA: 0x000C7B84 File Offset: 0x000C5D84
		public void SetAsDefaultSubProduct()
		{
			bool flag = this.selectedProduct != null && this.selectedProduct.IsMultiProduct && this.selectedSubProduct != null;
			if (flag)
			{
				bool isDefaultSubProduct = this.selectedSubProduct.IsDefaultSubProduct;
				if (isDefaultSubProduct)
				{
					bool flag2 = Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtRemoveDefaultProductAssignment"), DialogStyle.Warning, DialogButtons.OKCancel) == Dialog.DialogResults.Ok;
					if (flag2)
					{
						this.selectedSubProduct.IsDefaultSubProduct = false;
						this.parHandling.WriteValue(Definition.recognitionDefaultGroupPath, "-1", false);
						this.OnPropertyChanged("SelectedSubProduct");
					}
				}
				else
				{
					bool flag3 = Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtAssignNewDefaultProduct"), DialogStyle.Warning, DialogButtons.OKCancel) == Dialog.DialogResults.Ok;
					if (flag3)
					{
						for (int i = 0; i < this.selectedProduct.SubProducts.Count; i++)
						{
							this.selectedProduct.SubProducts[i].IsDefaultSubProduct = (this.selectedProduct.SubProducts[i] == this.selectedSubProduct);
						}
						this.dbConnection.UpdateTable(this.selectedProduct.TableName, Definition.recognitionDefaultGroupPath, this.selectedSubProduct.Group.ToString());
						bool flag4 = this.selectedProduct == this.currentLoadedProduct;
						if (flag4)
						{
							this.parHandling.WriteValue(Definition.recognitionDefaultGroupPath, this.selectedSubProduct.Group.ToString(), false);
						}
						this.OnPropertyChanged("SelectedSubProduct");
					}
				}
			}
		}

		// Token: 0x040013AB RID: 5035
		private const double voltageTolerance = 2.0;

		// Token: 0x040013AC RID: 5036
		private const double currentTolerance = 0.1;

		// Token: 0x040013AD RID: 5037
		private string[] subLanePathsWithoutSlashes = new string[]
		{
			"ReferenceWeight",
			"MinWeight",
			"MaxWeight"
		};

		// Token: 0x040013AE RID: 5038
		private string deformationFactor;

		// Token: 0x040013AF RID: 5039
		private ProductData selectedProduct;

		// Token: 0x040013B1 RID: 5041
		private bool referenceProductVisible = false;

		// Token: 0x040013B2 RID: 5042
		private ProductData currentLoadedProduct;

		// Token: 0x040013B3 RID: 5043
		private Keyboard keyboard;

		// Token: 0x040013B4 RID: 5044
		private NumKeyboard numKeyboard;

		// Token: 0x040013B5 RID: 5045
		private DBConnection dbConnection;

		// Token: 0x040013B6 RID: 5046
		private ParameterHandling parHandling;

		// Token: 0x040013B7 RID: 5047
		private List<string> pathes = new List<string>();

		// Token: 0x040013B8 RID: 5048
		private List<string> values = new List<string>();

		// Token: 0x040013B9 RID: 5049
		private BackgroundWorker backgroundWorker;

		// Token: 0x040013BA RID: 5050
		private Progress prog;

		// Token: 0x040013BB RID: 5051
		private MachineState machineState;

		// Token: 0x040013BC RID: 5052
		private ImageDataTransfer imageTransfer;

		// Token: 0x040013BD RID: 5053
		private MachineStates oldState;

		// Token: 0x040013BE RID: 5054
		private Conveyor conveyor;

		// Token: 0x040013BF RID: 5055
		private AuditCheckSettings auditSettings;

		// Token: 0x040013C0 RID: 5056
		private object lockObjectProductChange = new object();

		// Token: 0x040013C1 RID: 5057
		private ProductData selectedSubProduct;

		// Token: 0x040013C2 RID: 5058
		private AccessRightsManager accessManager;

		// Token: 0x040013C3 RID: 5059
		private StoreImageData imageArchiver;

		// Token: 0x040013C4 RID: 5060
		private CtrlAutotrainAdjustments ctrlAutotrainAdjustments;

		// Token: 0x040013C5 RID: 5061
		public ListCollectionView ViewProducts;

		// Token: 0x040013C7 RID: 5063
		private AutotrainControl ctrlAutotrain;

		// Token: 0x040013C8 RID: 5064
		private static Subwindow subwindowAutotrainMain;

		// Token: 0x040013D1 RID: 5073
		private TestcardSimService testcardSimService;

		// Token: 0x040013D2 RID: 5074
		private RejecterActivity rejecterActivity;

		// Token: 0x040013D3 RID: 5075
		public ProductData referenceProduct;
	}
}
