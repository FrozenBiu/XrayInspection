using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using Common;
using DatabaseConnection;

namespace GUI_XRS
{
	// Token: 0x02000073 RID: 115
	public class IVPCertificatesContentViewModel : INotifyPropertyChanged
	{
		// Token: 0x1400003B RID: 59
		// (add) Token: 0x06000A9B RID: 2715 RVA: 0x0003F888 File Offset: 0x0003DA88
		// (remove) Token: 0x06000A9C RID: 2716 RVA: 0x0003F8C0 File Offset: 0x0003DAC0
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x17000315 RID: 789
		// (get) Token: 0x06000A9D RID: 2717 RVA: 0x0003F8F5 File Offset: 0x0003DAF5
		// (set) Token: 0x06000A9E RID: 2718 RVA: 0x0003F8FD File Offset: 0x0003DAFD
		public RelayCommand CleanUpCommand { get; set; }

		// Token: 0x17000316 RID: 790
		// (get) Token: 0x06000A9F RID: 2719 RVA: 0x0003F906 File Offset: 0x0003DB06
		// (set) Token: 0x06000AA0 RID: 2720 RVA: 0x0003F90E File Offset: 0x0003DB0E
		public BoundLocalizedText StrValidation { get; set; }

		// Token: 0x17000317 RID: 791
		// (get) Token: 0x06000AA1 RID: 2721 RVA: 0x0003F917 File Offset: 0x0003DB17
		// (set) Token: 0x06000AA2 RID: 2722 RVA: 0x0003F91F File Offset: 0x0003DB1F
		public BoundLocalizedText StrProduct { get; set; }

		// Token: 0x17000318 RID: 792
		// (get) Token: 0x06000AA3 RID: 2723 RVA: 0x0003F928 File Offset: 0x0003DB28
		// (set) Token: 0x06000AA4 RID: 2724 RVA: 0x0003F930 File Offset: 0x0003DB30
		public BoundLocalizedText StrTemperature { get; set; }

		// Token: 0x17000319 RID: 793
		// (get) Token: 0x06000AA5 RID: 2725 RVA: 0x0003F939 File Offset: 0x0003DB39
		// (set) Token: 0x06000AA6 RID: 2726 RVA: 0x0003F941 File Offset: 0x0003DB41
		public BoundLocalizedText StrLength { get; set; }

		// Token: 0x1700031A RID: 794
		// (get) Token: 0x06000AA7 RID: 2727 RVA: 0x0003F94A File Offset: 0x0003DB4A
		// (set) Token: 0x06000AA8 RID: 2728 RVA: 0x0003F952 File Offset: 0x0003DB52
		public BoundLocalizedText StrWidth { get; set; }

		// Token: 0x1700031B RID: 795
		// (get) Token: 0x06000AA9 RID: 2729 RVA: 0x0003F95B File Offset: 0x0003DB5B
		// (set) Token: 0x06000AAA RID: 2730 RVA: 0x0003F963 File Offset: 0x0003DB63
		public BoundLocalizedText StrHeight { get; set; }

		// Token: 0x1700031C RID: 796
		// (get) Token: 0x06000AAB RID: 2731 RVA: 0x0003F96C File Offset: 0x0003DB6C
		// (set) Token: 0x06000AAC RID: 2732 RVA: 0x0003F974 File Offset: 0x0003DB74
		public BoundLocalizedText StrState { get; set; }

		// Token: 0x1700031D RID: 797
		// (get) Token: 0x06000AAD RID: 2733 RVA: 0x0003F97D File Offset: 0x0003DB7D
		// (set) Token: 0x06000AAE RID: 2734 RVA: 0x0003F985 File Offset: 0x0003DB85
		public BoundLocalizedText StrRate { get; set; }

		// Token: 0x1700031E RID: 798
		// (get) Token: 0x06000AAF RID: 2735 RVA: 0x0003F98E File Offset: 0x0003DB8E
		// (set) Token: 0x06000AB0 RID: 2736 RVA: 0x0003F996 File Offset: 0x0003DB96
		public BoundLocalizedText StrName { get; set; }

		// Token: 0x1700031F RID: 799
		// (get) Token: 0x06000AB1 RID: 2737 RVA: 0x0003F99F File Offset: 0x0003DB9F
		// (set) Token: 0x06000AB2 RID: 2738 RVA: 0x0003F9A7 File Offset: 0x0003DBA7
		public BoundLocalizedText StrStreet { get; set; }

		// Token: 0x17000320 RID: 800
		// (get) Token: 0x06000AB3 RID: 2739 RVA: 0x0003F9B0 File Offset: 0x0003DBB0
		// (set) Token: 0x06000AB4 RID: 2740 RVA: 0x0003F9B8 File Offset: 0x0003DBB8
		public BoundLocalizedText StrCity { get; set; }

		// Token: 0x17000321 RID: 801
		// (get) Token: 0x06000AB5 RID: 2741 RVA: 0x0003F9C1 File Offset: 0x0003DBC1
		// (set) Token: 0x06000AB6 RID: 2742 RVA: 0x0003F9C9 File Offset: 0x0003DBC9
		public BoundLocalizedText StrPLZ { get; set; }

		// Token: 0x17000322 RID: 802
		// (get) Token: 0x06000AB7 RID: 2743 RVA: 0x0003F9D2 File Offset: 0x0003DBD2
		// (set) Token: 0x06000AB8 RID: 2744 RVA: 0x0003F9DA File Offset: 0x0003DBDA
		public BoundLocalizedText StrAdditionalAddress { get; set; }

		// Token: 0x17000323 RID: 803
		// (get) Token: 0x06000AB9 RID: 2745 RVA: 0x0003F9E3 File Offset: 0x0003DBE3
		// (set) Token: 0x06000ABA RID: 2746 RVA: 0x0003F9EB File Offset: 0x0003DBEB
		public BoundLocalizedText StrContactPerson { get; set; }

		// Token: 0x17000324 RID: 804
		// (get) Token: 0x06000ABB RID: 2747 RVA: 0x0003F9F4 File Offset: 0x0003DBF4
		// (set) Token: 0x06000ABC RID: 2748 RVA: 0x0003F9FC File Offset: 0x0003DBFC
		public BoundLocalizedText StrEmail { get; set; }

		// Token: 0x17000325 RID: 805
		// (get) Token: 0x06000ABD RID: 2749 RVA: 0x0003FA05 File Offset: 0x0003DC05
		// (set) Token: 0x06000ABE RID: 2750 RVA: 0x0003FA0D File Offset: 0x0003DC0D
		public BoundLocalizedText StrTelephone { get; set; }

		// Token: 0x17000326 RID: 806
		// (get) Token: 0x06000ABF RID: 2751 RVA: 0x0003FA16 File Offset: 0x0003DC16
		// (set) Token: 0x06000AC0 RID: 2752 RVA: 0x0003FA1E File Offset: 0x0003DC1E
		public BoundLocalizedText StrOffice { get; set; }

		// Token: 0x17000327 RID: 807
		// (get) Token: 0x06000AC1 RID: 2753 RVA: 0x0003FA27 File Offset: 0x0003DC27
		// (set) Token: 0x06000AC2 RID: 2754 RVA: 0x0003FA2F File Offset: 0x0003DC2F
		public BoundLocalizedText StrServiceTechnician { get; set; }

		// Token: 0x17000328 RID: 808
		// (get) Token: 0x06000AC3 RID: 2755 RVA: 0x0003FA38 File Offset: 0x0003DC38
		// (set) Token: 0x06000AC4 RID: 2756 RVA: 0x0003FA40 File Offset: 0x0003DC40
		public BoundLocalizedText StrCustomerInformation { get; set; }

		// Token: 0x17000329 RID: 809
		// (get) Token: 0x06000AC5 RID: 2757 RVA: 0x0003FA49 File Offset: 0x0003DC49
		// (set) Token: 0x06000AC6 RID: 2758 RVA: 0x0003FA51 File Offset: 0x0003DC51
		public BoundLocalizedText StrManufacturerInformation { get; set; }

		// Token: 0x1700032A RID: 810
		// (get) Token: 0x06000AC7 RID: 2759 RVA: 0x0003FA5A File Offset: 0x0003DC5A
		// (set) Token: 0x06000AC8 RID: 2760 RVA: 0x0003FA62 File Offset: 0x0003DC62
		public BoundLocalizedText StrProductInformation { get; set; }

		// Token: 0x1700032B RID: 811
		// (get) Token: 0x06000AC9 RID: 2761 RVA: 0x0003FA6B File Offset: 0x0003DC6B
		// (set) Token: 0x06000ACA RID: 2762 RVA: 0x0003FA73 File Offset: 0x0003DC73
		public BoundLocalizedText StrContent { get; set; }

		// Token: 0x1700032C RID: 812
		// (get) Token: 0x06000ACB RID: 2763 RVA: 0x0003FA7C File Offset: 0x0003DC7C
		// (set) Token: 0x06000ACC RID: 2764 RVA: 0x0003FA94 File Offset: 0x0003DC94
		public ObservableCollection<Infos> ProductItems
		{
			get
			{
				return this.productItems;
			}
			set
			{
				this.productItems = value;
				this.OnPropertyChanged("ProductItems");
			}
		}

		// Token: 0x1700032D RID: 813
		// (get) Token: 0x06000ACD RID: 2765 RVA: 0x0003FAAC File Offset: 0x0003DCAC
		// (set) Token: 0x06000ACE RID: 2766 RVA: 0x0003FAC4 File Offset: 0x0003DCC4
		public ObservableCollection<Infos> CustomerItems
		{
			get
			{
				return this.customerItems;
			}
			set
			{
				this.customerItems = value;
				this.OnPropertyChanged("CustomerItems");
			}
		}

		// Token: 0x1700032E RID: 814
		// (get) Token: 0x06000ACF RID: 2767 RVA: 0x0003FADC File Offset: 0x0003DCDC
		// (set) Token: 0x06000AD0 RID: 2768 RVA: 0x0003FAF4 File Offset: 0x0003DCF4
		public bool ProductDataVisible
		{
			get
			{
				return this.productDataVisible;
			}
			set
			{
				this.productDataVisible = value;
				this.OnPropertyChanged("ProductDataVisible");
			}
		}

		// Token: 0x06000AD1 RID: 2769 RVA: 0x0003FB0C File Offset: 0x0003DD0C
		public IVPCertificatesContentViewModel(DBConnection dBCon, AccessRightsManager accessMan, ProductManager productManager)
		{
			this.dbConnection = dBCon;
			this.accessManager = accessMan;
			bool flag = productManager.CurrentLoadedProduct.ProductNumber.Equals("-1");
			if (flag)
			{
				this.ProductDataVisible = false;
			}
			else
			{
				this.ProductDataVisible = true;
			}
			this.RegisterLocalizedText();
			this.InitLists();
		}

		// Token: 0x06000AD2 RID: 2770 RVA: 0x0003FB8C File Offset: 0x0003DD8C
		private void InitLists()
		{
			string value = "";
			ObservableCollection<Infos> observableCollection = new ObservableCollection<Infos>();
			this.dbConnection.GetIVPInfoEntry(DBIntegratedValidationProcess.ProductLength, ref value);
			observableCollection.Add(new Infos(this.dbConnection)
			{
				Description = this.StrLength.LocalizedText + ":",
				Value = value,
				DBPath = DBIntegratedValidationProcess.ProductLength
			});
			this.dbConnection.GetIVPInfoEntry(DBIntegratedValidationProcess.ProductWidth, ref value);
			observableCollection.Add(new Infos(this.dbConnection)
			{
				Description = this.StrWidth.LocalizedText + ":",
				Value = value,
				DBPath = DBIntegratedValidationProcess.ProductWidth
			});
			this.dbConnection.GetIVPInfoEntry(DBIntegratedValidationProcess.ProductHeight, ref value);
			observableCollection.Add(new Infos(this.dbConnection)
			{
				Description = this.StrHeight.LocalizedText + ":",
				Value = value,
				DBPath = DBIntegratedValidationProcess.ProductHeight
			});
			this.dbConnection.GetIVPInfoEntry(DBIntegratedValidationProcess.ProductTemperature, ref value);
			observableCollection.Add(new Infos(this.dbConnection)
			{
				Description = this.StrTemperature.LocalizedText + ":",
				Value = value,
				DBPath = DBIntegratedValidationProcess.ProductTemperature
			});
			this.dbConnection.GetIVPInfoEntry(DBIntegratedValidationProcess.ProductPackRate, ref value);
			observableCollection.Add(new Infos(this.dbConnection)
			{
				Description = this.StrRate.LocalizedText + ":",
				Value = value,
				DBPath = DBIntegratedValidationProcess.ProductPackRate
			});
			this.ProductItems = observableCollection;
			observableCollection = new ObservableCollection<Infos>();
			this.dbConnection.GetIVPInfoEntry(DBIntegratedValidationProcess.CustomerName, ref value);
			observableCollection.Add(new Infos(this.dbConnection)
			{
				Description = this.StrName.LocalizedText + ":",
				Value = value,
				DBPath = DBIntegratedValidationProcess.CustomerName
			});
			this.dbConnection.GetIVPInfoEntry(DBIntegratedValidationProcess.CustomerStreet, ref value);
			observableCollection.Add(new Infos(this.dbConnection)
			{
				Description = this.StrStreet.LocalizedText + ":",
				Value = value,
				DBPath = DBIntegratedValidationProcess.CustomerStreet
			});
			this.dbConnection.GetIVPInfoEntry(DBIntegratedValidationProcess.CustomerLocation, ref value);
			observableCollection.Add(new Infos(this.dbConnection)
			{
				Description = this.StrCity.LocalizedText + ":",
				Value = value,
				DBPath = DBIntegratedValidationProcess.CustomerLocation
			});
			this.dbConnection.GetIVPInfoEntry(DBIntegratedValidationProcess.CustomerPostalCode, ref value);
			observableCollection.Add(new Infos(this.dbConnection)
			{
				Description = this.StrPLZ.LocalizedText + ":",
				Value = value,
				DBPath = DBIntegratedValidationProcess.CustomerPostalCode
			});
			this.dbConnection.GetIVPInfoEntry(DBIntegratedValidationProcess.CustomerState, ref value);
			observableCollection.Add(new Infos(this.dbConnection)
			{
				Description = this.StrState.LocalizedText + ":",
				Value = value,
				DBPath = DBIntegratedValidationProcess.CustomerState
			});
			this.dbConnection.GetIVPInfoEntry(DBIntegratedValidationProcess.CustomerAdditionalAdress, ref value);
			observableCollection.Add(new Infos(this.dbConnection)
			{
				Description = this.StrAdditionalAddress.LocalizedText + ":",
				Value = value,
				DBPath = DBIntegratedValidationProcess.CustomerAdditionalAdress
			});
			this.dbConnection.GetIVPInfoEntry(DBIntegratedValidationProcess.CustomerContactPerson, ref value);
			observableCollection.Add(new Infos(this.dbConnection)
			{
				Description = this.StrContactPerson.LocalizedText + ":",
				Value = value,
				DBPath = DBIntegratedValidationProcess.CustomerContactPerson
			});
			this.dbConnection.GetIVPInfoEntry(DBIntegratedValidationProcess.CustomerEMail, ref value);
			observableCollection.Add(new Infos(this.dbConnection)
			{
				Description = this.StrEmail.LocalizedText + ":",
				Value = value,
				DBPath = DBIntegratedValidationProcess.CustomerEMail
			});
			this.dbConnection.GetIVPInfoEntry(DBIntegratedValidationProcess.CustomerPhone, ref value);
			observableCollection.Add(new Infos(this.dbConnection)
			{
				Description = this.StrTelephone.LocalizedText + ":",
				Value = value,
				DBPath = DBIntegratedValidationProcess.CustomerPhone
			});
			this.CustomerItems = observableCollection;
		}

		// Token: 0x06000AD3 RID: 2771 RVA: 0x00040064 File Offset: 0x0003E264
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x06000AD4 RID: 2772 RVA: 0x00040094 File Offset: 0x0003E294
		private void RegisterLocalizedText()
		{
			this.StrState = new BoundLocalizedText("txtState");
			this.StrValidation = new BoundLocalizedText("txtValidation");
			this.StrProduct = new BoundLocalizedText("txtApplication");
			this.StrTemperature = new BoundLocalizedText("txtTemperature");
			this.StrLength = new BoundLocalizedText("txtLengthProduct");
			this.StrWidth = new BoundLocalizedText("txtWidth");
			this.StrHeight = new BoundLocalizedText("txtHeight");
			this.StrRate = new BoundLocalizedText("txtRate");
			this.StrName = new BoundLocalizedText("txtName");
			this.StrStreet = new BoundLocalizedText("txtStreet");
			this.StrCity = new BoundLocalizedText("txtLocation");
			this.StrPLZ = new BoundLocalizedText("txtPLZ");
			this.StrAdditionalAddress = new BoundLocalizedText("txtAdditionalAddress");
			this.StrContactPerson = new BoundLocalizedText("txtContactPerson");
			this.StrEmail = new BoundLocalizedText("txtEmail");
			this.StrTelephone = new BoundLocalizedText("txtTelephone");
			this.StrServiceTechnician = new BoundLocalizedText("txtServiceTechnician");
			this.StrOffice = new BoundLocalizedText("txtOffice");
			this.StrManufacturerInformation = new BoundLocalizedText("txtInformationManufacturer");
			this.StrCustomerInformation = new BoundLocalizedText("txtInformationCustomer");
			this.StrProductInformation = new BoundLocalizedText("txtInformationProduct");
			this.StrContent = new BoundLocalizedText("menuItemIVPCertificatsContent");
		}

		// Token: 0x06000AD5 RID: 2773 RVA: 0x00040218 File Offset: 0x0003E418
		public void DeregisterLocalizedText()
		{
			MainWindow.localizationManager.Deregister(this.StrValidation);
			MainWindow.localizationManager.Deregister(this.StrProduct);
			MainWindow.localizationManager.Deregister(this.StrTemperature);
			MainWindow.localizationManager.Deregister(this.StrLength);
			MainWindow.localizationManager.Deregister(this.StrWidth);
			MainWindow.localizationManager.Deregister(this.StrHeight);
			MainWindow.localizationManager.Deregister(this.StrRate);
			MainWindow.localizationManager.Deregister(this.StrName);
			MainWindow.localizationManager.Deregister(this.StrStreet);
			MainWindow.localizationManager.Deregister(this.StrCity);
			MainWindow.localizationManager.Deregister(this.StrPLZ);
			MainWindow.localizationManager.Deregister(this.StrAdditionalAddress);
			MainWindow.localizationManager.Deregister(this.StrContactPerson);
			MainWindow.localizationManager.Deregister(this.StrEmail);
			MainWindow.localizationManager.Deregister(this.StrTelephone);
			MainWindow.localizationManager.Deregister(this.StrServiceTechnician);
			MainWindow.localizationManager.Deregister(this.StrOffice);
			MainWindow.localizationManager.Deregister(this.StrCustomerInformation);
			MainWindow.localizationManager.Deregister(this.StrManufacturerInformation);
			MainWindow.localizationManager.Deregister(this.StrProductInformation);
			MainWindow.localizationManager.Deregister(this.StrState);
			MainWindow.localizationManager.Deregister(this.StrContent);
		}

		// Token: 0x06000AD6 RID: 2774 RVA: 0x0004039C File Offset: 0x0003E59C
		public void CleanUp(object obj)
		{
			bool flag = this.ProductItems != null;
			if (flag)
			{
				this.ProductItems.Clear();
				this.ProductItems = null;
			}
			bool flag2 = this.CustomerItems != null;
			if (flag2)
			{
				this.CustomerItems.Clear();
				this.CustomerItems = null;
			}
			this.DeregisterLocalizedText();
		}

		// Token: 0x06000AD7 RID: 2775 RVA: 0x000403F8 File Offset: 0x0003E5F8
		public void ShowEditKeyboard(Infos item)
		{
			this.info = item;
			this.keyboard = new Keyboard(item.Description, item.Value, KeyboardType.Default);
			this.keyboard.TextInputKeyboard += this.keyboard_TextInputKeyboard;
			this.keyboard.Show();
		}

		// Token: 0x06000AD8 RID: 2776 RVA: 0x0004044C File Offset: 0x0003E64C
		public void ShowEditKeyboardEmail(Infos item)
		{
			this.info = item;
			this.keyboard = new Keyboard(item.Description, item.Value, KeyboardType.Email);
			this.keyboard.TextInputKeyboard += this.keyboard_TextInputKeyboard;
			this.keyboard.Show();
		}

		// Token: 0x06000AD9 RID: 2777 RVA: 0x000404A0 File Offset: 0x0003E6A0
		private void keyboard_TextInputKeyboard(object sender, KeyboardInputEventArgs e)
		{
			bool flag = this.info.Value != e.Text;
			if (flag)
			{
				this.info.Value = e.Text;
			}
			this.keyboard.TextInputKeyboard -= this.keyboard_TextInputKeyboard;
		}

		// Token: 0x04000727 RID: 1831
		private Infos info;

		// Token: 0x04000729 RID: 1833
		private DBConnection dbConnection;

		// Token: 0x0400072A RID: 1834
		private AccessRightsManager accessManager;

		// Token: 0x0400072B RID: 1835
		private Keyboard keyboard;

		// Token: 0x04000742 RID: 1858
		private ObservableCollection<Infos> productItems = new ObservableCollection<Infos>();

		// Token: 0x04000743 RID: 1859
		private ObservableCollection<Infos> customerItems = new ObservableCollection<Infos>();

		// Token: 0x04000744 RID: 1860
		private bool productDataVisible = true;
	}
}
