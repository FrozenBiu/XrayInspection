using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using Common;
using DatabaseConnection;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x0200013C RID: 316
	public class ProductsToLaneViewModel : INotifyPropertyChanged
	{
		// Token: 0x170008C3 RID: 2243
		// (get) Token: 0x06001E70 RID: 7792 RVA: 0x000C812E File Offset: 0x000C632E
		// (set) Token: 0x06001E71 RID: 7793 RVA: 0x000C8136 File Offset: 0x000C6336
		public ProductManager ProductManager { get; set; }

		// Token: 0x170008C4 RID: 2244
		// (get) Token: 0x06001E72 RID: 7794 RVA: 0x000C8140 File Offset: 0x000C6340
		// (set) Token: 0x06001E73 RID: 7795 RVA: 0x000C8158 File Offset: 0x000C6358
		public ProductData SelectedSubLane
		{
			get
			{
				return this.selectedSubLane;
			}
			set
			{
				this.selectedSubLane = value;
				this.OnPropertyChanged("SelectedSubLane");
			}
		}

		// Token: 0x170008C5 RID: 2245
		// (get) Token: 0x06001E74 RID: 7796 RVA: 0x000C8170 File Offset: 0x000C6370
		// (set) Token: 0x06001E75 RID: 7797 RVA: 0x000C8188 File Offset: 0x000C6388
		public ProductData LaneProduct
		{
			get
			{
				return this.laneProduct;
			}
			set
			{
				this.laneProduct = value;
				this.OnPropertyChanged("LaneProduct");
			}
		}

		// Token: 0x170008C6 RID: 2246
		// (get) Token: 0x06001E76 RID: 7798 RVA: 0x000C819E File Offset: 0x000C639E
		// (set) Token: 0x06001E77 RID: 7799 RVA: 0x000C81A6 File Offset: 0x000C63A6
		public RelayCommand AssignCommand { get; set; }

		// Token: 0x170008C7 RID: 2247
		// (get) Token: 0x06001E78 RID: 7800 RVA: 0x000C81AF File Offset: 0x000C63AF
		// (set) Token: 0x06001E79 RID: 7801 RVA: 0x000C81B7 File Offset: 0x000C63B7
		public RelayCommand DeleteSubLaneProductCommand { get; set; }

		// Token: 0x140000A1 RID: 161
		// (add) Token: 0x06001E7A RID: 7802 RVA: 0x000C81C0 File Offset: 0x000C63C0
		// (remove) Token: 0x06001E7B RID: 7803 RVA: 0x000C81F8 File Offset: 0x000C63F8
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x06001E7C RID: 7804 RVA: 0x000C8230 File Offset: 0x000C6430
		public ProductsToLaneViewModel(ProductManager productManager, DBConnection dbConnection, ParameterHandling parHandling, ImageDataTransfer imagetransfer)
		{
			this.ProductManager = productManager;
			this.dbConnection = dbConnection;
			this.parHandling = parHandling;
			this.imagetransfer = imagetransfer;
			dbConnection.GetDetectorType(ref this.detectortype);
			this.AssignCommand = new RelayCommand(new Action<object>(this.Assign));
			this.DeleteSubLaneProductCommand = new RelayCommand(new Action<object>(this.DeleteSubLaneProduct));
			this.laneProduct = productManager.SelectedProduct;
			bool flag = productManager.ProductList.Count > 0;
			if (flag)
			{
				productManager.SelectedProduct = productManager.ProductList[0];
			}
			bool flag2 = this.laneProduct.SubProducts.Count > 0;
			if (flag2)
			{
				this.SelectedSubLane = this.laneProduct.SubProducts[0];
			}
		}

		// Token: 0x06001E7D RID: 7805 RVA: 0x000C8308 File Offset: 0x000C6508
		private void Assign(object obj)
		{
			bool flag = this.ProductManager.SelectedProduct != null && this.selectedSubLane != null;
			if (flag)
			{
				Dialog.DialogResults dialogResults = Dialog.DialogResults.Ok;
				bool flag2 = this.selectedSubLane.ProductName != "";
				if (flag2)
				{
					dialogResults = Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtOverwriteProductAssignment") + "?", DialogStyle.Warning, DialogButtons.OKCancel);
				}
				bool flag3 = dialogResults == Dialog.DialogResults.Ok;
				if (flag3)
				{
					List<string> list = new List<string>();
					List<string> list2 = new List<string>();
					List<string> list3 = new List<string>();
					this.dbConnection.GetProductEntries(this.SelectedSubLane.TableName, list, list3);
					list3.Clear();
					this.dbConnection.GetProductEntries(this.ProductManager.SelectedProduct.TableName, list2, list3);
					for (int i = 0; i < list2.Count; i++)
					{
						list2[i] = this.ProductManager.SetMainPathToSubPath(list2[i], (int)this.SelectedSubLane.Lane);
						bool flag4 = !list.Contains(list2[i]);
						if (flag4)
						{
							list2.RemoveAt(i);
							list3.RemoveAt(i);
							i--;
						}
					}
					this.dbConnection.UpdateTable(this.SelectedSubLane.TableName, list2, list3);
					this.dbConnection.RenameAppl(this.SelectedSubLane.TableName, this.ProductManager.SelectedProduct.ProductName);
					this.dbConnection.SetProductNumber(this.SelectedSubLane.TableName, this.ProductManager.SelectedProduct.ProductNumber);
					this.SetParameterValues((int)this.selectedSubLane.Lane, list2, list3);
					this.SelectedSubLane.ProductName = this.ProductManager.SelectedProduct.ProductName;
					this.SelectedSubLane.ProductNumber = this.ProductManager.SelectedProduct.ProductNumber;
					this.dbConnection.UpdateTable(this.SelectedSubLane.TableName, Definition.refTableNamePath, this.ProductManager.SelectedProduct.TableName);
					bool flag5 = this.detectortype == DetectorType.DTNetDE;
					if (flag5)
					{
						List<MaterialCurveElement> list4 = new List<MaterialCurveElement>();
						bool flag6 = this.dbConnection.CheckIfMaterialCurveExists(this.ProductManager.SelectedProduct.TableName);
						if (flag6)
						{
							this.dbConnection.GetMaterialCurve(this.ProductManager.SelectedProduct.TableName, list4);
							this.dbConnection.SetMaterialCurve(this.SelectedSubLane.TableName, list4);
							DualEnergy dualEnergy = new DualEnergy(this.imagetransfer, this.parHandling);
							dualEnergy.SendMaterialCurveImage(list4, Convert.ToInt32(this.SelectedSubLane.Group));
						}
					}
				}
			}
		}

		// Token: 0x06001E7E RID: 7806 RVA: 0x000C85D4 File Offset: 0x000C67D4
		private void SetParameterValues(int group, List<string> paths, List<string> strValues)
		{
			for (int i = 0; i < paths.Count; i++)
			{
				this.parHandling.WriteValue(paths[i], strValues[i], false);
			}
		}

		// Token: 0x06001E7F RID: 7807 RVA: 0x000C8614 File Offset: 0x000C6814
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x06001E80 RID: 7808 RVA: 0x000C8644 File Offset: 0x000C6844
		private void DeleteSubLaneProduct(object obj)
		{
			bool flag = this.SelectedSubLane != null;
			if (flag)
			{
				Dialog.DialogResults dialogResults = Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtProductAssignment") + " " + LocalizationManager.Localize("msgGettingErased"), DialogStyle.Warning, DialogButtons.OKCancel);
				bool flag2 = dialogResults == Dialog.DialogResults.Ok;
				if (flag2)
				{
					List<string> list = new List<string>();
					List<string> list2 = new List<string>();
					this.dbConnection.GetProductEntries(DBConnection.defaultSubApplication, list, list2);
					for (int i = 0; i < list.Count; i++)
					{
						bool flag3 = list[i] == Definition.filterMainPath + Definition.defaultSubProductGroup.ToString() + Definition.propertiesSubPath + "/Lane/Value";
						if (flag3)
						{
							list.RemoveAt(i);
							list2.RemoveAt(i);
							i--;
						}
						else
						{
							list[i] = this.ProductManager.SetMainPathToSubPath(list[i], (int)this.selectedSubLane.Lane);
						}
					}
					this.dbConnection.UpdateTable(this.selectedSubLane.TableName, list, list2);
					this.SetParameterValues((int)this.selectedSubLane.Lane, list, list2);
					this.dbConnection.RenameAppl(this.SelectedSubLane.TableName, "");
					this.dbConnection.SetProductNumber(this.SelectedSubLane.TableName, "");
					this.dbConnection.UpdateTable(this.SelectedSubLane.TableName, Definition.refTableNamePath, "");
					this.SelectedSubLane.ProductName = "";
					this.SelectedSubLane.ProductNumber = "";
					bool flag4 = this.detectortype == DetectorType.DTNetDE;
					if (flag4)
					{
						bool flag5 = this.dbConnection.CheckIfMaterialCurveExists(this.SelectedSubLane.TableName);
						if (flag5)
						{
							this.dbConnection.DeleteMaterialCurve(this.SelectedSubLane.TableName);
							DualEnergy dualEnergy = new DualEnergy(this.imagetransfer, this.parHandling);
							dualEnergy.SendMaterialCurveImage(null, Convert.ToInt32(this.SelectedSubLane.Group));
						}
					}
					bool flag6 = this.dbConnection.CheckIfTestcardSimSensitivitiesExists(this.SelectedSubLane.TableName);
					if (flag6)
					{
						this.dbConnection.DeleteTestcardSimSensitivities(this.SelectedSubLane.TableName);
					}
				}
			}
		}

		// Token: 0x040013D7 RID: 5079
		private DBConnection dbConnection;

		// Token: 0x040013D8 RID: 5080
		private ParameterHandling parHandling;

		// Token: 0x040013D9 RID: 5081
		private ProductData selectedSubLane;

		// Token: 0x040013DA RID: 5082
		private ProductData laneProduct;

		// Token: 0x040013DB RID: 5083
		private ImageDataTransfer imagetransfer;

		// Token: 0x040013DC RID: 5084
		private DetectorType detectortype = DetectorType.Sesotec;
	}
}
