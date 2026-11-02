using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using Common;
using DatabaseConnection;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x02000070 RID: 112
	public class IVPMultiChoice : INotifyPropertyChanged
	{
		// Token: 0x14000038 RID: 56
		// (add) Token: 0x060009EE RID: 2542 RVA: 0x0003CB44 File Offset: 0x0003AD44
		// (remove) Token: 0x060009EF RID: 2543 RVA: 0x0003CB7C File Offset: 0x0003AD7C
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x170002D4 RID: 724
		// (get) Token: 0x060009F0 RID: 2544 RVA: 0x0003CBB4 File Offset: 0x0003ADB4
		// (set) Token: 0x060009F1 RID: 2545 RVA: 0x0003CBCC File Offset: 0x0003ADCC
		public List<string> SelectionList
		{
			get
			{
				return this.selectionList;
			}
			set
			{
				this.selectionList = value;
				this.OnPropertyChanged("SelectionList");
			}
		}

		// Token: 0x170002D5 RID: 725
		// (get) Token: 0x060009F2 RID: 2546 RVA: 0x0003CBE4 File Offset: 0x0003ADE4
		// (set) Token: 0x060009F3 RID: 2547 RVA: 0x0003CBFC File Offset: 0x0003ADFC
		public string SelectedItem
		{
			get
			{
				return this.selectedItem;
			}
			set
			{
				this.selectedItem = value;
				this.OnPropertyChanged("SelectedItem");
			}
		}

		// Token: 0x060009F4 RID: 2548 RVA: 0x0003CC14 File Offset: 0x0003AE14
		public void CheckItem()
		{
			bool flag = this.SelectedItem != null;
			if (flag)
			{
				bool flag2 = this.SelectionList.Contains(this.SelectedItem);
				if (flag2)
				{
					bool isLaneProduct = this.productManager.CurrentLoadedProduct.IsLaneProduct;
					int productType;
					if (isLaneProduct)
					{
						productType = 1;
					}
					else
					{
						productType = 2;
					}
					CtrlIVPProcess usrControl = new CtrlIVPProcess(this.accessManager, this.machineState, this.parameterHandling, this.imageTransfer, this.dbConnection, this.backup, productType, this.SelectedItem, this.messageSystem, this.productManager);
					Subwindow.Show(usrControl, SubwindowButtons.BackAndHome);
				}
			}
		}

		// Token: 0x170002D6 RID: 726
		// (get) Token: 0x060009F5 RID: 2549 RVA: 0x0003CCA9 File Offset: 0x0003AEA9
		// (set) Token: 0x060009F6 RID: 2550 RVA: 0x0003CCB1 File Offset: 0x0003AEB1
		public BoundLocalizedText StrChoose { get; set; }

		// Token: 0x170002D7 RID: 727
		// (get) Token: 0x060009F7 RID: 2551 RVA: 0x0003CCBA File Offset: 0x0003AEBA
		// (set) Token: 0x060009F8 RID: 2552 RVA: 0x0003CCC2 File Offset: 0x0003AEC2
		public BoundLocalizedText StrMenuName { get; set; }

		// Token: 0x060009F9 RID: 2553 RVA: 0x0003CCCC File Offset: 0x0003AECC
		public IVPMultiChoice(AccessRightsManager accessManager, MachineState machineState, ParameterHandling parameterHandling, ImageDataTransfer imageTransfer, Backup backup, ProductManager productManager, DBConnection dbConnection, MessageSystem messageSystem)
		{
			this.productManager = productManager;
			this.dbConnection = dbConnection;
			this.accessManager = accessManager;
			this.parameterHandling = parameterHandling;
			this.machineState = machineState;
			this.imageTransfer = imageTransfer;
			this.backup = backup;
			this.messageSystem = messageSystem;
			this.CleanUpCommand = new RelayCommand(new Action<object>(this.CleanUp));
			this.InitList();
			this.RegisterLocalizedText();
		}

		// Token: 0x060009FA RID: 2554 RVA: 0x0003CD59 File Offset: 0x0003AF59
		private void RegisterLocalizedText()
		{
			this.StrChoose = new BoundLocalizedText("txtChooseSubproduct");
			this.StrMenuName = new BoundLocalizedText("menuItemIVPStart");
		}

		// Token: 0x060009FB RID: 2555 RVA: 0x0003CD7E File Offset: 0x0003AF7E
		public void DeregisterLocalizedText()
		{
			MainWindow.localizationManager.Deregister(this.StrChoose);
			MainWindow.localizationManager.Deregister(this.StrMenuName);
		}

		// Token: 0x060009FC RID: 2556 RVA: 0x0003CDA4 File Offset: 0x0003AFA4
		public void InitList()
		{
			this.listSubproducts = this.productManager.CurrentLoadedProduct.SubProducts;
			string value = "";
			int num = 1;
			foreach (ProductData productData in this.listSubproducts)
			{
				bool flag = !productData.ProductName.Equals("");
				if (flag)
				{
					string tableName = productData.TableName;
					this.dbConnection.GetParameterValue(string.Concat(new string[]
					{
						Definition.filterMainPath,
						num.ToString(),
						Definition.comparerSubPath,
						"1/Contrast/",
						Definition.strValue
					}), tableName, ref value);
					int num2 = Convert.ToInt32(value);
					this.dbConnection.GetParameterValue(string.Concat(new string[]
					{
						Definition.filterMainPath,
						num.ToString(),
						Definition.objectFindersSubPath,
						"1/Contrast/",
						Definition.strValue
					}), tableName, ref value);
					int num3 = Convert.ToInt32(value);
					num++;
					bool flag2 = (num2 > 0 && num2 < 32000) || (num3 > 0 && num3 < 32000);
					if (flag2)
					{
						this.SelectionList.Add(productData.ProductName);
					}
				}
			}
		}

		// Token: 0x060009FD RID: 2557 RVA: 0x0003CF20 File Offset: 0x0003B120
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x060009FE RID: 2558 RVA: 0x0003CF50 File Offset: 0x0003B150
		public void CleanUp(object obj)
		{
			bool flag = this.SelectionList != null;
			if (flag)
			{
				this.SelectionList.Clear();
				this.SelectionList = null;
			}
			this.DeregisterLocalizedText();
		}

		// Token: 0x040006CB RID: 1739
		private DBConnection dbConnection;

		// Token: 0x040006CC RID: 1740
		private List<string> selectionList = new List<string>();

		// Token: 0x040006CD RID: 1741
		private string selectedItem;

		// Token: 0x040006CE RID: 1742
		private ProductManager productManager;

		// Token: 0x040006CF RID: 1743
		private ParameterHandling parameterHandling;

		// Token: 0x040006D0 RID: 1744
		private ObservableCollection<ProductData> listSubproducts;

		// Token: 0x040006D1 RID: 1745
		private List<string> numbersSubproducts = new List<string>();

		// Token: 0x040006D2 RID: 1746
		private AccessRightsManager accessManager;

		// Token: 0x040006D3 RID: 1747
		private MachineState machineState;

		// Token: 0x040006D4 RID: 1748
		private ImageDataTransfer imageTransfer;

		// Token: 0x040006D5 RID: 1749
		private Backup backup;

		// Token: 0x040006D6 RID: 1750
		public RelayCommand CleanUpCommand;

		// Token: 0x040006D9 RID: 1753
		private MessageSystem messageSystem;
	}
}
