using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using DatabaseConnection;

namespace GUI_XRS.Integrated_Validation_Process
{
	// Token: 0x02000270 RID: 624
	public class IVPCertificatesManufacturerViewModel : INotifyPropertyChanged
	{
		// Token: 0x14000111 RID: 273
		// (add) Token: 0x060033DB RID: 13275 RVA: 0x00145F58 File Offset: 0x00144158
		// (remove) Token: 0x060033DC RID: 13276 RVA: 0x00145F90 File Offset: 0x00144190
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x170010DB RID: 4315
		// (get) Token: 0x060033DD RID: 13277 RVA: 0x00145FC5 File Offset: 0x001441C5
		// (set) Token: 0x060033DE RID: 13278 RVA: 0x00145FCD File Offset: 0x001441CD
		public BoundLocalizedText StrState { get; set; }

		// Token: 0x170010DC RID: 4316
		// (get) Token: 0x060033DF RID: 13279 RVA: 0x00145FD6 File Offset: 0x001441D6
		// (set) Token: 0x060033E0 RID: 13280 RVA: 0x00145FDE File Offset: 0x001441DE
		public BoundLocalizedText StrName { get; set; }

		// Token: 0x170010DD RID: 4317
		// (get) Token: 0x060033E1 RID: 13281 RVA: 0x00145FE7 File Offset: 0x001441E7
		// (set) Token: 0x060033E2 RID: 13282 RVA: 0x00145FEF File Offset: 0x001441EF
		public BoundLocalizedText StrStreet { get; set; }

		// Token: 0x170010DE RID: 4318
		// (get) Token: 0x060033E3 RID: 13283 RVA: 0x00145FF8 File Offset: 0x001441F8
		// (set) Token: 0x060033E4 RID: 13284 RVA: 0x00146000 File Offset: 0x00144200
		public BoundLocalizedText StrCity { get; set; }

		// Token: 0x170010DF RID: 4319
		// (get) Token: 0x060033E5 RID: 13285 RVA: 0x00146009 File Offset: 0x00144209
		// (set) Token: 0x060033E6 RID: 13286 RVA: 0x00146011 File Offset: 0x00144211
		public BoundLocalizedText StrPLZ { get; set; }

		// Token: 0x170010E0 RID: 4320
		// (get) Token: 0x060033E7 RID: 13287 RVA: 0x0014601A File Offset: 0x0014421A
		// (set) Token: 0x060033E8 RID: 13288 RVA: 0x00146022 File Offset: 0x00144222
		public BoundLocalizedText StrAdditionalAddress { get; set; }

		// Token: 0x170010E1 RID: 4321
		// (get) Token: 0x060033E9 RID: 13289 RVA: 0x0014602B File Offset: 0x0014422B
		// (set) Token: 0x060033EA RID: 13290 RVA: 0x00146033 File Offset: 0x00144233
		public BoundLocalizedText StrContactPerson { get; set; }

		// Token: 0x170010E2 RID: 4322
		// (get) Token: 0x060033EB RID: 13291 RVA: 0x0014603C File Offset: 0x0014423C
		// (set) Token: 0x060033EC RID: 13292 RVA: 0x00146044 File Offset: 0x00144244
		public BoundLocalizedText StrEmail { get; set; }

		// Token: 0x170010E3 RID: 4323
		// (get) Token: 0x060033ED RID: 13293 RVA: 0x0014604D File Offset: 0x0014424D
		// (set) Token: 0x060033EE RID: 13294 RVA: 0x00146055 File Offset: 0x00144255
		public BoundLocalizedText StrTelephone { get; set; }

		// Token: 0x170010E4 RID: 4324
		// (get) Token: 0x060033EF RID: 13295 RVA: 0x0014605E File Offset: 0x0014425E
		// (set) Token: 0x060033F0 RID: 13296 RVA: 0x00146066 File Offset: 0x00144266
		public BoundLocalizedText StrOffice { get; set; }

		// Token: 0x170010E5 RID: 4325
		// (get) Token: 0x060033F1 RID: 13297 RVA: 0x0014606F File Offset: 0x0014426F
		// (set) Token: 0x060033F2 RID: 13298 RVA: 0x00146077 File Offset: 0x00144277
		public BoundLocalizedText StrServiceTechnician { get; set; }

		// Token: 0x170010E6 RID: 4326
		// (get) Token: 0x060033F3 RID: 13299 RVA: 0x00146080 File Offset: 0x00144280
		// (set) Token: 0x060033F4 RID: 13300 RVA: 0x00146088 File Offset: 0x00144288
		public BoundLocalizedText StrCustomerInformation { get; set; }

		// Token: 0x170010E7 RID: 4327
		// (get) Token: 0x060033F5 RID: 13301 RVA: 0x00146091 File Offset: 0x00144291
		// (set) Token: 0x060033F6 RID: 13302 RVA: 0x00146099 File Offset: 0x00144299
		public BoundLocalizedText StrManufacturerInformation { get; set; }

		// Token: 0x170010E8 RID: 4328
		// (get) Token: 0x060033F7 RID: 13303 RVA: 0x001460A4 File Offset: 0x001442A4
		// (set) Token: 0x060033F8 RID: 13304 RVA: 0x001460BC File Offset: 0x001442BC
		public ObservableCollection<Infos> ManufacturerItems
		{
			get
			{
				return this.manufacturerItems;
			}
			set
			{
				this.manufacturerItems = value;
				this.OnPropertyChanged("ManufacturerItems");
			}
		}

		// Token: 0x060033F9 RID: 13305 RVA: 0x001460D2 File Offset: 0x001442D2
		public IVPCertificatesManufacturerViewModel(DBConnection dBConnection)
		{
			this.dbConnection = dBConnection;
			this.RegisterLocalizedText();
			this.InitLists();
		}

		// Token: 0x060033FA RID: 13306 RVA: 0x001460FC File Offset: 0x001442FC
		private void InitLists()
		{
			ObservableCollection<Infos> observableCollection = new ObservableCollection<Infos>();
			string value = "";
			this.dbConnection.GetIVPInfoEntry(DBIntegratedValidationProcess.ManufacturerOffice, ref value);
			observableCollection.Add(new Infos(this.dbConnection)
			{
				Description = this.StrOffice.LocalizedText + ":",
				Value = value,
				DBPath = DBIntegratedValidationProcess.ManufacturerOffice
			});
			this.dbConnection.GetIVPInfoEntry(DBIntegratedValidationProcess.ManufacturerStreet, ref value);
			observableCollection.Add(new Infos(this.dbConnection)
			{
				Description = this.StrStreet.LocalizedText + ":",
				Value = value,
				DBPath = DBIntegratedValidationProcess.ManufacturerStreet
			});
			this.dbConnection.GetIVPInfoEntry(DBIntegratedValidationProcess.ManufacturerLocation, ref value);
			observableCollection.Add(new Infos(this.dbConnection)
			{
				Description = this.StrCity.LocalizedText + ":",
				Value = value,
				DBPath = DBIntegratedValidationProcess.ManufacturerLocation
			});
			this.dbConnection.GetIVPInfoEntry(DBIntegratedValidationProcess.ManufacturerState, ref value);
			observableCollection.Add(new Infos(this.dbConnection)
			{
				Description = this.StrState.LocalizedText + ":",
				Value = value,
				DBPath = DBIntegratedValidationProcess.ManufacturerState
			});
			this.dbConnection.GetIVPInfoEntry(DBIntegratedValidationProcess.ManufacturerPostalCode, ref value);
			observableCollection.Add(new Infos(this.dbConnection)
			{
				Description = this.StrPLZ.LocalizedText + ":",
				Value = value,
				DBPath = DBIntegratedValidationProcess.ManufacturerPostalCode
			});
			this.dbConnection.GetIVPInfoEntry(DBIntegratedValidationProcess.ManufacturerAdditionalAdress, ref value);
			observableCollection.Add(new Infos(this.dbConnection)
			{
				Description = this.StrAdditionalAddress.LocalizedText + ":",
				Value = value,
				DBPath = DBIntegratedValidationProcess.ManufacturerAdditionalAdress
			});
			this.dbConnection.GetIVPInfoEntry(DBIntegratedValidationProcess.ManufacturerServiceTechnician, ref value);
			observableCollection.Add(new Infos(this.dbConnection)
			{
				Description = this.StrServiceTechnician.LocalizedText + ":",
				Value = value,
				DBPath = DBIntegratedValidationProcess.ManufacturerServiceTechnician
			});
			this.dbConnection.GetIVPInfoEntry(DBIntegratedValidationProcess.ManufacturerEMail, ref value);
			observableCollection.Add(new Infos(this.dbConnection)
			{
				Description = this.StrEmail.LocalizedText + ":",
				Value = value,
				DBPath = DBIntegratedValidationProcess.ManufacturerEMail
			});
			this.dbConnection.GetIVPInfoEntry(DBIntegratedValidationProcess.ManufacturerPhone, ref value);
			observableCollection.Add(new Infos(this.dbConnection)
			{
				Description = this.StrTelephone.LocalizedText + ":",
				Value = value,
				DBPath = DBIntegratedValidationProcess.ManufacturerPhone
			});
			this.ManufacturerItems = observableCollection;
		}

		// Token: 0x060033FB RID: 13307 RVA: 0x0014641C File Offset: 0x0014461C
		private void RegisterLocalizedText()
		{
			this.StrState = new BoundLocalizedText("txtState");
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
		}

		// Token: 0x060033FC RID: 13308 RVA: 0x00146508 File Offset: 0x00144708
		public void DeregisterLocalizedText()
		{
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
			MainWindow.localizationManager.Deregister(this.StrState);
		}

		// Token: 0x060033FD RID: 13309 RVA: 0x001465F4 File Offset: 0x001447F4
		public void CleanUp(object obj)
		{
			this.DeregisterLocalizedText();
			bool flag = this.ManufacturerItems != null;
			if (flag)
			{
				this.ManufacturerItems.Clear();
				this.ManufacturerItems = null;
			}
		}

		// Token: 0x060033FE RID: 13310 RVA: 0x0014662C File Offset: 0x0014482C
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x060033FF RID: 13311 RVA: 0x0014665C File Offset: 0x0014485C
		public void ShowEditKeyboard(Infos item)
		{
			this.info = item;
			this.keyboard = new Keyboard(item.Description, item.Value, KeyboardType.Default);
			this.keyboard.TextInputKeyboard += this.keyboard_TextInputKeyboard;
			this.keyboard.Show();
		}

		// Token: 0x06003400 RID: 13312 RVA: 0x001466B0 File Offset: 0x001448B0
		public void ShowEditKeyboardEmail(Infos item)
		{
			this.info = item;
			this.keyboard = new Keyboard(item.Description, item.Value, KeyboardType.Email);
			this.keyboard.TextInputKeyboard += this.keyboard_TextInputKeyboard;
			this.keyboard.Show();
		}

		// Token: 0x06003401 RID: 13313 RVA: 0x00146704 File Offset: 0x00144904
		private void keyboard_TextInputKeyboard(object sender, KeyboardInputEventArgs e)
		{
			bool flag = this.info.Value != e.Text;
			if (flag)
			{
				this.info.Value = e.Text;
			}
			this.keyboard.TextInputKeyboard -= this.keyboard_TextInputKeyboard;
		}

		// Token: 0x04001E04 RID: 7684
		private DBConnection dbConnection;

		// Token: 0x04001E06 RID: 7686
		private Keyboard keyboard;

		// Token: 0x04001E07 RID: 7687
		private Infos info;

		// Token: 0x04001E15 RID: 7701
		private ObservableCollection<Infos> manufacturerItems = new ObservableCollection<Infos>();
	}
}
