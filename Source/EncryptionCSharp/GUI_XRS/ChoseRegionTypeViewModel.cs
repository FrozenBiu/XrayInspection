using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using Common;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x0200012C RID: 300
	public class ChoseRegionTypeViewModel : INotifyPropertyChanged
	{
		// Token: 0x1700087A RID: 2170
		// (get) Token: 0x06001CF4 RID: 7412 RVA: 0x000BB764 File Offset: 0x000B9964
		// (set) Token: 0x06001CF5 RID: 7413 RVA: 0x000BB76C File Offset: 0x000B996C
		public ProductManager ProductManager { get; set; }

		// Token: 0x1700087B RID: 2171
		// (get) Token: 0x06001CF6 RID: 7414 RVA: 0x000BB778 File Offset: 0x000B9978
		// (set) Token: 0x06001CF7 RID: 7415 RVA: 0x000BB790 File Offset: 0x000B9990
		public XRSRegionType SelectedRegionType
		{
			get
			{
				return this.selectedRegionType;
			}
			set
			{
				this.selectedRegionType = value;
				this.OnPropertyChanged("SelectedRegionType");
			}
		}

		// Token: 0x1700087C RID: 2172
		// (get) Token: 0x06001CF8 RID: 7416 RVA: 0x000BB7A6 File Offset: 0x000B99A6
		// (set) Token: 0x06001CF9 RID: 7417 RVA: 0x000BB7AE File Offset: 0x000B99AE
		public bool RegionAdded { get; private set; }

		// Token: 0x1700087D RID: 2173
		// (get) Token: 0x06001CFA RID: 7418 RVA: 0x000BB7B7 File Offset: 0x000B99B7
		// (set) Token: 0x06001CFB RID: 7419 RVA: 0x000BB7BF File Offset: 0x000B99BF
		public bool RegionEdited { get; private set; }

		// Token: 0x14000094 RID: 148
		// (add) Token: 0x06001CFC RID: 7420 RVA: 0x000BB7C8 File Offset: 0x000B99C8
		// (remove) Token: 0x06001CFD RID: 7421 RVA: 0x000BB800 File Offset: 0x000B9A00
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x14000095 RID: 149
		// (add) Token: 0x06001CFE RID: 7422 RVA: 0x000BB838 File Offset: 0x000B9A38
		// (remove) Token: 0x06001CFF RID: 7423 RVA: 0x000BB870 File Offset: 0x000B9A70
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action RegionAddedOrEdited;

		// Token: 0x1700087E RID: 2174
		// (get) Token: 0x06001D00 RID: 7424 RVA: 0x000BB8A5 File Offset: 0x000B9AA5
		// (set) Token: 0x06001D01 RID: 7425 RVA: 0x000BB8AD File Offset: 0x000B9AAD
		public RelayCommand AddOrEditRegionCommand { get; set; }

		// Token: 0x06001D02 RID: 7426 RVA: 0x000BB8B8 File Offset: 0x000B9AB8
		public ChoseRegionTypeViewModel(ProductManager productManager, ObservableCollection<ProductData> regionProduct, ProductData dummyProduct, bool addMode)
		{
			this.ProductManager = productManager;
			this.regionProduct = regionProduct;
			this.dummyProduct = dummyProduct;
			this.addMode = addMode;
			this.AddOrEditRegionCommand = new RelayCommand(new Action<object>(this.AddOrEditRegion));
			if (addMode)
			{
				this.SelectedRegionType = XRSRegionType.Rectangle;
			}
			else
			{
				bool flag = productManager.SelectedSubProduct != null;
				if (flag)
				{
					this.SelectedRegionType = productManager.SelectedSubProduct.Region.Type;
				}
			}
		}

		// Token: 0x06001D03 RID: 7427 RVA: 0x000BB93C File Offset: 0x000B9B3C
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x06001D04 RID: 7428 RVA: 0x000BB96C File Offset: 0x000B9B6C
		private void AddOrEditRegion(object obj)
		{
			bool flag = this.addMode;
			if (flag)
			{
				this.AddRegion();
			}
			else
			{
				this.EditRegion();
			}
		}

		// Token: 0x06001D05 RID: 7429 RVA: 0x000BB994 File Offset: 0x000B9B94
		private void AddRegion()
		{
			this.keyboard = new Keyboard(LocalizationManager.Localize("msgCreateRegion"), "", KeyboardType.Application);
			this.keyboard.TextInputKeyboard -= this.keyboard_TextInputKeyboard_AddRegion;
			this.keyboard.TextInputKeyboard += this.keyboard_TextInputKeyboard_AddRegion;
			this.keyboard.ShowDialog();
		}

		// Token: 0x06001D06 RID: 7430 RVA: 0x000BB9FC File Offset: 0x000B9BFC
		private void keyboard_TextInputKeyboard_AddRegion(object sender, KeyboardInputEventArgs e)
		{
			this.keyboard.TextInputKeyboard -= this.keyboard_TextInputKeyboard_AddRegion;
			bool flag = e.Text != null && e.Text.Length > 0;
			if (flag)
			{
				Action <>9__1;
				new Thread(delegate()
				{
					this.ProductManager.AddRegionProduct(e.Text, true, this.selectedRegionType);
					Dispatcher dispatcher = Application.Current.Dispatcher;
					DispatcherPriority priority = DispatcherPriority.Normal;
					Action method;
					if ((method = <>9__1) == null)
					{
						method = (<>9__1 = delegate()
						{
							bool flag2 = this.ProductManager.SelectedProduct.SubProducts.Count > 0 && this.regionProduct.Count > 0;
							if (flag2)
							{
								this.regionProduct.Remove(this.dummyProduct);
								this.regionProduct.Add(this.ProductManager.SelectedProduct.SubProducts[this.ProductManager.SelectedProduct.SubProducts.Count - 1]);
								this.regionProduct.Add(this.dummyProduct);
							}
							this.RegionAdded = true;
							this.OnRegionAddedOrEdited();
						});
					}
					dispatcher.Invoke(priority, method);
				})
				{
					Name = "AddRegionProduct"
				}.Start();
			}
			else
			{
				Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("msgInsertAppName"), DialogStyle.Error);
			}
		}

		// Token: 0x06001D07 RID: 7431 RVA: 0x000BBAA4 File Offset: 0x000B9CA4
		protected virtual void OnRegionAddedOrEdited()
		{
			Action regionAddedOrEdited = this.RegionAddedOrEdited;
			bool flag = regionAddedOrEdited != null;
			if (flag)
			{
				regionAddedOrEdited();
			}
		}

		// Token: 0x06001D08 RID: 7432 RVA: 0x000BBACC File Offset: 0x000B9CCC
		private void EditRegion()
		{
			bool flag = this.ProductManager.SelectedSubProduct != null;
			if (flag)
			{
				this.ProductManager.EditSelectedRegion(this.selectedRegionType);
				this.RegionEdited = true;
				this.OnRegionAddedOrEdited();
			}
		}

		// Token: 0x04001325 RID: 4901
		private Keyboard keyboard;

		// Token: 0x04001326 RID: 4902
		private ObservableCollection<ProductData> regionProduct;

		// Token: 0x04001327 RID: 4903
		private ProductData dummyProduct;

		// Token: 0x04001328 RID: 4904
		private XRSRegionType selectedRegionType;

		// Token: 0x04001329 RID: 4905
		private bool addMode;
	}
}
