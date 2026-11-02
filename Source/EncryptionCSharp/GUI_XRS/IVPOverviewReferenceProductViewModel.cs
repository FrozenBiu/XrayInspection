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
	// Token: 0x02000071 RID: 113
	internal class IVPOverviewReferenceProductViewModel : INotifyPropertyChanged
	{
		// Token: 0x14000039 RID: 57
		// (add) Token: 0x060009FF RID: 2559 RVA: 0x0003CF88 File Offset: 0x0003B188
		// (remove) Token: 0x06000A00 RID: 2560 RVA: 0x0003CFC0 File Offset: 0x0003B1C0
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x170002D8 RID: 728
		// (get) Token: 0x06000A01 RID: 2561 RVA: 0x0003CFF5 File Offset: 0x0003B1F5
		// (set) Token: 0x06000A02 RID: 2562 RVA: 0x0003CFFD File Offset: 0x0003B1FD
		public RelayCommand CleanUpCommand { get; set; }

		// Token: 0x170002D9 RID: 729
		// (get) Token: 0x06000A03 RID: 2563 RVA: 0x0003D006 File Offset: 0x0003B206
		// (set) Token: 0x06000A04 RID: 2564 RVA: 0x0003D00E File Offset: 0x0003B20E
		public BoundLocalizedText StrValidation { get; set; }

		// Token: 0x170002DA RID: 730
		// (get) Token: 0x06000A05 RID: 2565 RVA: 0x0003D017 File Offset: 0x0003B217
		// (set) Token: 0x06000A06 RID: 2566 RVA: 0x0003D01F File Offset: 0x0003B21F
		public BoundLocalizedText StrProduct { get; set; }

		// Token: 0x170002DB RID: 731
		// (get) Token: 0x06000A07 RID: 2567 RVA: 0x0003D028 File Offset: 0x0003B228
		// (set) Token: 0x06000A08 RID: 2568 RVA: 0x0003D030 File Offset: 0x0003B230
		public BoundLocalizedText StrTemperature { get; set; }

		// Token: 0x170002DC RID: 732
		// (get) Token: 0x06000A09 RID: 2569 RVA: 0x0003D039 File Offset: 0x0003B239
		// (set) Token: 0x06000A0A RID: 2570 RVA: 0x0003D041 File Offset: 0x0003B241
		public BoundLocalizedText StrLength { get; set; }

		// Token: 0x170002DD RID: 733
		// (get) Token: 0x06000A0B RID: 2571 RVA: 0x0003D04A File Offset: 0x0003B24A
		// (set) Token: 0x06000A0C RID: 2572 RVA: 0x0003D052 File Offset: 0x0003B252
		public BoundLocalizedText StrWidth { get; set; }

		// Token: 0x170002DE RID: 734
		// (get) Token: 0x06000A0D RID: 2573 RVA: 0x0003D05B File Offset: 0x0003B25B
		// (set) Token: 0x06000A0E RID: 2574 RVA: 0x0003D063 File Offset: 0x0003B263
		public BoundLocalizedText StrHeight { get; set; }

		// Token: 0x170002DF RID: 735
		// (get) Token: 0x06000A0F RID: 2575 RVA: 0x0003D06C File Offset: 0x0003B26C
		// (set) Token: 0x06000A10 RID: 2576 RVA: 0x0003D074 File Offset: 0x0003B274
		public BoundLocalizedText StrState { get; set; }

		// Token: 0x170002E0 RID: 736
		// (get) Token: 0x06000A11 RID: 2577 RVA: 0x0003D07D File Offset: 0x0003B27D
		// (set) Token: 0x06000A12 RID: 2578 RVA: 0x0003D085 File Offset: 0x0003B285
		public BoundLocalizedText StrRate { get; set; }

		// Token: 0x170002E1 RID: 737
		// (get) Token: 0x06000A13 RID: 2579 RVA: 0x0003D08E File Offset: 0x0003B28E
		// (set) Token: 0x06000A14 RID: 2580 RVA: 0x0003D096 File Offset: 0x0003B296
		public BoundLocalizedText StrName { get; set; }

		// Token: 0x170002E2 RID: 738
		// (get) Token: 0x06000A15 RID: 2581 RVA: 0x0003D09F File Offset: 0x0003B29F
		// (set) Token: 0x06000A16 RID: 2582 RVA: 0x0003D0A7 File Offset: 0x0003B2A7
		public BoundLocalizedText StrStreet { get; set; }

		// Token: 0x170002E3 RID: 739
		// (get) Token: 0x06000A17 RID: 2583 RVA: 0x0003D0B0 File Offset: 0x0003B2B0
		// (set) Token: 0x06000A18 RID: 2584 RVA: 0x0003D0B8 File Offset: 0x0003B2B8
		public BoundLocalizedText StrCity { get; set; }

		// Token: 0x170002E4 RID: 740
		// (get) Token: 0x06000A19 RID: 2585 RVA: 0x0003D0C1 File Offset: 0x0003B2C1
		// (set) Token: 0x06000A1A RID: 2586 RVA: 0x0003D0C9 File Offset: 0x0003B2C9
		public BoundLocalizedText StrPLZ { get; set; }

		// Token: 0x170002E5 RID: 741
		// (get) Token: 0x06000A1B RID: 2587 RVA: 0x0003D0D2 File Offset: 0x0003B2D2
		// (set) Token: 0x06000A1C RID: 2588 RVA: 0x0003D0DA File Offset: 0x0003B2DA
		public BoundLocalizedText StrAdditionalAddress { get; set; }

		// Token: 0x170002E6 RID: 742
		// (get) Token: 0x06000A1D RID: 2589 RVA: 0x0003D0E3 File Offset: 0x0003B2E3
		// (set) Token: 0x06000A1E RID: 2590 RVA: 0x0003D0EB File Offset: 0x0003B2EB
		public BoundLocalizedText StrContactPerson { get; set; }

		// Token: 0x170002E7 RID: 743
		// (get) Token: 0x06000A1F RID: 2591 RVA: 0x0003D0F4 File Offset: 0x0003B2F4
		// (set) Token: 0x06000A20 RID: 2592 RVA: 0x0003D0FC File Offset: 0x0003B2FC
		public BoundLocalizedText StrEmail { get; set; }

		// Token: 0x170002E8 RID: 744
		// (get) Token: 0x06000A21 RID: 2593 RVA: 0x0003D105 File Offset: 0x0003B305
		// (set) Token: 0x06000A22 RID: 2594 RVA: 0x0003D10D File Offset: 0x0003B30D
		public BoundLocalizedText StrTelephone { get; set; }

		// Token: 0x170002E9 RID: 745
		// (get) Token: 0x06000A23 RID: 2595 RVA: 0x0003D116 File Offset: 0x0003B316
		// (set) Token: 0x06000A24 RID: 2596 RVA: 0x0003D11E File Offset: 0x0003B31E
		public BoundLocalizedText StrOffice { get; set; }

		// Token: 0x170002EA RID: 746
		// (get) Token: 0x06000A25 RID: 2597 RVA: 0x0003D127 File Offset: 0x0003B327
		// (set) Token: 0x06000A26 RID: 2598 RVA: 0x0003D12F File Offset: 0x0003B32F
		public BoundLocalizedText StrServiceTechnician { get; set; }

		// Token: 0x170002EB RID: 747
		// (get) Token: 0x06000A27 RID: 2599 RVA: 0x0003D138 File Offset: 0x0003B338
		// (set) Token: 0x06000A28 RID: 2600 RVA: 0x0003D140 File Offset: 0x0003B340
		public BoundLocalizedText StrCustomerInformation { get; set; }

		// Token: 0x170002EC RID: 748
		// (get) Token: 0x06000A29 RID: 2601 RVA: 0x0003D149 File Offset: 0x0003B349
		// (set) Token: 0x06000A2A RID: 2602 RVA: 0x0003D151 File Offset: 0x0003B351
		public BoundLocalizedText StrManufacturerInformation { get; set; }

		// Token: 0x170002ED RID: 749
		// (get) Token: 0x06000A2B RID: 2603 RVA: 0x0003D15A File Offset: 0x0003B35A
		// (set) Token: 0x06000A2C RID: 2604 RVA: 0x0003D162 File Offset: 0x0003B362
		public BoundLocalizedText StrProductInformation { get; set; }

		// Token: 0x170002EE RID: 750
		// (get) Token: 0x06000A2D RID: 2605 RVA: 0x0003D16B File Offset: 0x0003B36B
		// (set) Token: 0x06000A2E RID: 2606 RVA: 0x0003D173 File Offset: 0x0003B373
		public BoundLocalizedText StrContent { get; set; }

		// Token: 0x170002EF RID: 751
		// (get) Token: 0x06000A2F RID: 2607 RVA: 0x0003D17C File Offset: 0x0003B37C
		// (set) Token: 0x06000A30 RID: 2608 RVA: 0x0003D184 File Offset: 0x0003B384
		public BoundLocalizedText StrNumberTestsamples { get; set; }

		// Token: 0x170002F0 RID: 752
		// (get) Token: 0x06000A31 RID: 2609 RVA: 0x0003D18D File Offset: 0x0003B38D
		// (set) Token: 0x06000A32 RID: 2610 RVA: 0x0003D195 File Offset: 0x0003B395
		public BoundLocalizedText StrMaterial { get; set; }

		// Token: 0x170002F1 RID: 753
		// (get) Token: 0x06000A33 RID: 2611 RVA: 0x0003D19E File Offset: 0x0003B39E
		// (set) Token: 0x06000A34 RID: 2612 RVA: 0x0003D1A6 File Offset: 0x0003B3A6
		public BoundLocalizedText StrBallSize { get; set; }

		// Token: 0x170002F2 RID: 754
		// (get) Token: 0x06000A35 RID: 2613 RVA: 0x0003D1AF File Offset: 0x0003B3AF
		// (set) Token: 0x06000A36 RID: 2614 RVA: 0x0003D1B7 File Offset: 0x0003B3B7
		public BoundLocalizedText StrTestcardSize { get; set; }

		// Token: 0x170002F3 RID: 755
		// (get) Token: 0x06000A37 RID: 2615 RVA: 0x0003D1C0 File Offset: 0x0003B3C0
		// (set) Token: 0x06000A38 RID: 2616 RVA: 0x0003D1C8 File Offset: 0x0003B3C8
		public BoundLocalizedText StrNumberOfBalls { get; set; }

		// Token: 0x170002F4 RID: 756
		// (get) Token: 0x06000A39 RID: 2617 RVA: 0x0003D1D1 File Offset: 0x0003B3D1
		// (set) Token: 0x06000A3A RID: 2618 RVA: 0x0003D1D9 File Offset: 0x0003B3D9
		public BoundLocalizedText StrSettings { get; set; }

		// Token: 0x170002F5 RID: 757
		// (get) Token: 0x06000A3B RID: 2619 RVA: 0x0003D1E2 File Offset: 0x0003B3E2
		// (set) Token: 0x06000A3C RID: 2620 RVA: 0x0003D1EA File Offset: 0x0003B3EA
		public BoundLocalizedText StrINNumber { get; set; }

		// Token: 0x170002F6 RID: 758
		// (get) Token: 0x06000A3D RID: 2621 RVA: 0x0003D1F3 File Offset: 0x0003B3F3
		// (set) Token: 0x06000A3E RID: 2622 RVA: 0x0003D1FB File Offset: 0x0003B3FB
		public BoundLocalizedText StrDensity { get; set; }

		// Token: 0x170002F7 RID: 759
		// (get) Token: 0x06000A3F RID: 2623 RVA: 0x0003D204 File Offset: 0x0003B404
		// (set) Token: 0x06000A40 RID: 2624 RVA: 0x0003D20C File Offset: 0x0003B40C
		public BoundLocalizedText StrTestContaminations { get; set; }

		// Token: 0x170002F8 RID: 760
		// (get) Token: 0x06000A41 RID: 2625 RVA: 0x0003D218 File Offset: 0x0003B418
		// (set) Token: 0x06000A42 RID: 2626 RVA: 0x0003D230 File Offset: 0x0003B430
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

		// Token: 0x170002F9 RID: 761
		// (get) Token: 0x06000A43 RID: 2627 RVA: 0x0003D248 File Offset: 0x0003B448
		// (set) Token: 0x06000A44 RID: 2628 RVA: 0x0003D260 File Offset: 0x0003B460
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

		// Token: 0x170002FA RID: 762
		// (get) Token: 0x06000A45 RID: 2629 RVA: 0x0003D278 File Offset: 0x0003B478
		// (set) Token: 0x06000A46 RID: 2630 RVA: 0x0003D290 File Offset: 0x0003B490
		public ObservableCollection<Infos> VAItems
		{
			get
			{
				return this.vaItems;
			}
			set
			{
				this.vaItems = value;
				this.OnPropertyChanged("VAItems");
			}
		}

		// Token: 0x170002FB RID: 763
		// (get) Token: 0x06000A47 RID: 2631 RVA: 0x0003D2A8 File Offset: 0x0003B4A8
		// (set) Token: 0x06000A48 RID: 2632 RVA: 0x0003D2C0 File Offset: 0x0003B4C0
		public ObservableCollection<Infos> GlassItems
		{
			get
			{
				return this.glassItems;
			}
			set
			{
				this.glassItems = value;
				this.OnPropertyChanged("GlassItems");
			}
		}

		// Token: 0x170002FC RID: 764
		// (get) Token: 0x06000A49 RID: 2633 RVA: 0x0003D2D8 File Offset: 0x0003B4D8
		// (set) Token: 0x06000A4A RID: 2634 RVA: 0x0003D2F0 File Offset: 0x0003B4F0
		public bool ManufacturerVisible
		{
			get
			{
				return this.manufacturerVisible;
			}
			set
			{
				this.manufacturerVisible = value;
				this.OnPropertyChanged("ManufacturerVisible");
			}
		}

		// Token: 0x06000A4B RID: 2635 RVA: 0x0003D308 File Offset: 0x0003B508
		public IVPOverviewReferenceProductViewModel(DBConnection dBCon, AccessRightsManager accessManager)
		{
			this.dbConnection = dBCon;
			this.accessManager = accessManager;
			this.RegisterLocalizedText();
			this.InitListsReferenceProduct();
			this.InitListTestCards();
			bool flag = accessManager.CurrentUser.ID == -3;
			if (flag)
			{
				this.InitListsManufacturer();
			}
			else
			{
				this.ManufacturerVisible = false;
			}
		}

		// Token: 0x06000A4C RID: 2636 RVA: 0x0003D39C File Offset: 0x0003B59C
		private void InitListsReferenceProduct()
		{
			string value = "";
			ObservableCollection<Infos> observableCollection = new ObservableCollection<Infos>();
			this.dbConnection.GetIVPInfoEntryReferenceProduct(DBIntegratedValidationProcessReferenceProduct.NumberOfProductsToConvey, ref value);
			observableCollection.Add(new Infos(this.dbConnection)
			{
				Description = this.StrNumberTestsamples.LocalizedText + ":",
				Value = value,
				DBPath = DBIntegratedValidationProcessReferenceProduct.ProductLength
			});
			this.dbConnection.GetIVPInfoEntryReferenceProduct(DBIntegratedValidationProcessReferenceProduct.ProductLength, ref value);
			observableCollection.Add(new Infos(this.dbConnection)
			{
				Description = this.StrLength.LocalizedText + ":",
				Value = value,
				DBPath = DBIntegratedValidationProcessReferenceProduct.ProductLength
			});
			this.dbConnection.GetIVPInfoEntryReferenceProduct(DBIntegratedValidationProcessReferenceProduct.ProductWidth, ref value);
			observableCollection.Add(new Infos(this.dbConnection)
			{
				Description = this.StrWidth.LocalizedText + ":",
				Value = value,
				DBPath = DBIntegratedValidationProcessReferenceProduct.ProductWidth
			});
			this.dbConnection.GetIVPInfoEntryReferenceProduct(DBIntegratedValidationProcessReferenceProduct.ProductHeight, ref value);
			observableCollection.Add(new Infos(this.dbConnection)
			{
				Description = this.StrHeight.LocalizedText + ":",
				Value = value,
				DBPath = DBIntegratedValidationProcessReferenceProduct.ProductHeight
			});
			this.dbConnection.GetIVPInfoEntryReferenceProduct(DBIntegratedValidationProcessReferenceProduct.ProductTemperature, ref value);
			observableCollection.Add(new Infos(this.dbConnection)
			{
				Description = this.StrTemperature.LocalizedText + ":",
				Value = value,
				DBPath = DBIntegratedValidationProcessReferenceProduct.ProductTemperature
			});
			this.dbConnection.GetIVPInfoEntryReferenceProduct(DBIntegratedValidationProcessReferenceProduct.ProductPackRate, ref value);
			observableCollection.Add(new Infos(this.dbConnection)
			{
				Description = this.StrRate.LocalizedText + ":",
				Value = value,
				DBPath = DBIntegratedValidationProcessReferenceProduct.ProductPackRate
			});
			this.ProductItems = observableCollection;
		}

		// Token: 0x06000A4D RID: 2637 RVA: 0x0003D5BC File Offset: 0x0003B7BC
		private void InitListTestCards()
		{
			ObservableCollection<Infos> observableCollection = new ObservableCollection<Infos>();
			MachineType machineType = MachineType.MachineTypeUndefined;
			this.dbConnection.GetMachineType(ref machineType);
			string text = "";
			this.dbConnection.GetMachineSubType(ref text);
			this.dbConnection.GetMachineType(ref machineType);
			List<string> list = new List<string>();
			bool flag = machineType == MachineType.RayconEntry || (text.Contains("MX") && machineType == MachineType.RayconDPlus);
			if (flag)
			{
				this.dbConnection.GetIVPTestcardRowReferenceProduct(1, ref list);
			}
			else
			{
				this.dbConnection.GetIVPTestcardRowReferenceProduct(0, ref list);
			}
			observableCollection.Add(new Infos(this.dbConnection)
			{
				Description = this.StrMaterial.LocalizedText + ":",
				Value = this.ConvertMaterial(this.convertStringToTestcardMaterial(list[0])),
				DBPath = DBIntegratedValidationProcessReferenceProduct.TestcardMaterial
			});
			observableCollection.Add(new Infos(this.dbConnection)
			{
				Description = this.StrTestcardSize.LocalizedText + ":",
				Value = this.ConvertTestcard(this.convertStringToTestcardType(list[1])),
				DBPath = DBIntegratedValidationProcessReferenceProduct.TestcardType
			});
			observableCollection.Add(new Infos(this.dbConnection)
			{
				Description = this.StrBallSize.LocalizedText + ":",
				Value = list[2],
				DBPath = DBIntegratedValidationProcessReferenceProduct.TestcardBallSize
			});
			observableCollection.Add(new Infos(this.dbConnection)
			{
				Description = this.StrNumberOfBalls.LocalizedText + ":",
				Value = list[3],
				DBPath = DBIntegratedValidationProcessReferenceProduct.TestcardCount
			});
			observableCollection.Add(new Infos(this.dbConnection)
			{
				Description = this.StrINNumber.LocalizedText + ":",
				Value = list[4],
				DBPath = DBIntegratedValidationProcessReferenceProduct.TestcardINNumber
			});
			observableCollection.Add(new Infos(this.dbConnection)
			{
				Description = this.StrDensity.LocalizedText + ":",
				Value = list[5],
				DBPath = DBIntegratedValidationProcessReferenceProduct.TestcardDensity
			});
			this.VAItems = observableCollection;
			list = new List<string>();
			observableCollection = new ObservableCollection<Infos>();
			bool flag2 = machineType != MachineType.RayconEX2A;
			if (flag2)
			{
				this.dbConnection.GetIVPTestcardRowReferenceProduct(2, ref list);
				observableCollection.Add(new Infos(this.dbConnection)
				{
					Description = this.StrMaterial.LocalizedText + ":",
					Value = this.ConvertMaterial(this.convertStringToTestcardMaterial(list[0])),
					DBPath = DBIntegratedValidationProcessReferenceProduct.TestcardMaterial
				});
				observableCollection.Add(new Infos(this.dbConnection)
				{
					Description = this.StrTestcardSize.LocalizedText + ":",
					Value = this.ConvertTestcard(this.convertStringToTestcardType(list[1])),
					DBPath = DBIntegratedValidationProcessReferenceProduct.TestcardType
				});
				observableCollection.Add(new Infos(this.dbConnection)
				{
					Description = this.StrBallSize.LocalizedText + ":",
					Value = list[2],
					DBPath = DBIntegratedValidationProcessReferenceProduct.TestcardBallSize
				});
				observableCollection.Add(new Infos(this.dbConnection)
				{
					Description = this.StrNumberOfBalls.LocalizedText + ":",
					Value = list[3],
					DBPath = DBIntegratedValidationProcessReferenceProduct.TestcardCount
				});
				observableCollection.Add(new Infos(this.dbConnection)
				{
					Description = this.StrINNumber.LocalizedText + ":",
					Value = list[4],
					DBPath = DBIntegratedValidationProcessReferenceProduct.TestcardINNumber
				});
				observableCollection.Add(new Infos(this.dbConnection)
				{
					Description = this.StrDensity.LocalizedText + ":",
					Value = list[5],
					DBPath = DBIntegratedValidationProcessReferenceProduct.TestcardDensity
				});
				this.GlassItems = observableCollection;
			}
		}

		// Token: 0x06000A4E RID: 2638 RVA: 0x0003DA1C File Offset: 0x0003BC1C
		private void InitListsManufacturer()
		{
			string value = "";
			ObservableCollection<Infos> observableCollection = new ObservableCollection<Infos>();
			observableCollection = new ObservableCollection<Infos>();
			this.dbConnection.GetIVPInfoEntryReferenceProduct(DBIntegratedValidationProcessReferenceProduct.ManufacturerServiceTechnician, ref value);
			observableCollection.Add(new Infos(this.dbConnection)
			{
				Description = this.StrName.LocalizedText + ":",
				Value = value,
				DBPath = DBIntegratedValidationProcessReferenceProduct.ManufacturerServiceTechnician
			});
			this.dbConnection.GetIVPInfoEntryReferenceProduct(DBIntegratedValidationProcessReferenceProduct.ManufacturerStreet, ref value);
			observableCollection.Add(new Infos(this.dbConnection)
			{
				Description = this.StrStreet.LocalizedText + ":",
				Value = value,
				DBPath = DBIntegratedValidationProcessReferenceProduct.ManufacturerStreet
			});
			this.dbConnection.GetIVPInfoEntryReferenceProduct(DBIntegratedValidationProcessReferenceProduct.ManufacturerLocation, ref value);
			observableCollection.Add(new Infos(this.dbConnection)
			{
				Description = this.StrCity.LocalizedText + ":",
				Value = value,
				DBPath = DBIntegratedValidationProcessReferenceProduct.ManufacturerLocation
			});
			this.dbConnection.GetIVPInfoEntryReferenceProduct(DBIntegratedValidationProcessReferenceProduct.ManufacturerPostalCode, ref value);
			observableCollection.Add(new Infos(this.dbConnection)
			{
				Description = this.StrPLZ.LocalizedText + ":",
				Value = value,
				DBPath = DBIntegratedValidationProcessReferenceProduct.ManufacturerPostalCode
			});
			this.dbConnection.GetIVPInfoEntryReferenceProduct(DBIntegratedValidationProcessReferenceProduct.ManufacturerState, ref value);
			observableCollection.Add(new Infos(this.dbConnection)
			{
				Description = this.StrState.LocalizedText + ":",
				Value = value,
				DBPath = DBIntegratedValidationProcessReferenceProduct.ManufacturerState
			});
			this.dbConnection.GetIVPInfoEntryReferenceProduct(DBIntegratedValidationProcessReferenceProduct.ManufacturerAdditionalAdress, ref value);
			observableCollection.Add(new Infos(this.dbConnection)
			{
				Description = this.StrAdditionalAddress.LocalizedText + ":",
				Value = value,
				DBPath = DBIntegratedValidationProcessReferenceProduct.ManufacturerAdditionalAdress
			});
			this.dbConnection.GetIVPInfoEntryReferenceProduct(DBIntegratedValidationProcessReferenceProduct.ManufacturerEMail, ref value);
			observableCollection.Add(new Infos(this.dbConnection)
			{
				Description = this.StrEmail.LocalizedText + ":",
				Value = value,
				DBPath = DBIntegratedValidationProcessReferenceProduct.ManufacturerEMail
			});
			this.dbConnection.GetIVPInfoEntryReferenceProduct(DBIntegratedValidationProcessReferenceProduct.ManufacturerPhone, ref value);
			observableCollection.Add(new Infos(this.dbConnection)
			{
				Description = this.StrTelephone.LocalizedText + ":",
				Value = value,
				DBPath = DBIntegratedValidationProcessReferenceProduct.ManufacturerPhone
			});
			this.ManufacturerItems = observableCollection;
		}

		// Token: 0x06000A4F RID: 2639 RVA: 0x0003DCEC File Offset: 0x0003BEEC
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x06000A50 RID: 2640 RVA: 0x0003DD1C File Offset: 0x0003BF1C
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
			this.StrNumberTestsamples = new BoundLocalizedText("txtNumImagesValidation");
			this.StrMaterial = new BoundLocalizedText("txtMaterial");
			this.StrBallSize = new BoundLocalizedText("txtSize");
			this.StrSettings = new BoundLocalizedText("txtSettings");
			this.StrNumberOfBalls = new BoundLocalizedText("txtNumberOfBalls");
			this.StrTestcardSize = new BoundLocalizedText("txtTestcardSize");
			this.StrINNumber = new BoundLocalizedText("txtINNumber");
			this.StrDensity = new BoundLocalizedText("txtDensity");
			this.StrTestContaminations = new BoundLocalizedText("menuItemIVPTestsamples");
		}

		// Token: 0x06000A51 RID: 2641 RVA: 0x0003DF3C File Offset: 0x0003C13C
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
			MainWindow.localizationManager.Deregister(this.StrNumberTestsamples);
			MainWindow.localizationManager.Deregister(this.StrMaterial);
			MainWindow.localizationManager.Deregister(this.StrBallSize);
			MainWindow.localizationManager.Deregister(this.StrSettings);
			MainWindow.localizationManager.Deregister(this.StrNumberOfBalls);
			MainWindow.localizationManager.Deregister(this.StrTestcardSize);
			MainWindow.localizationManager.Deregister(this.StrINNumber);
			MainWindow.localizationManager.Deregister(this.StrDensity);
			MainWindow.localizationManager.Deregister(this.StrTestContaminations);
		}

		// Token: 0x06000A52 RID: 2642 RVA: 0x0003E15C File Offset: 0x0003C35C
		public void CleanUp(object obj)
		{
			bool flag = this.ProductItems != null;
			if (flag)
			{
				this.ProductItems.Clear();
				this.ProductItems = null;
			}
			bool flag2 = this.ManufacturerItems != null;
			if (flag2)
			{
				this.ManufacturerItems.Clear();
				this.ManufacturerItems = null;
			}
			bool flag3 = this.VAItems != null;
			if (flag3)
			{
				this.VAItems.Clear();
				this.VAItems = null;
			}
			bool flag4 = this.GlassItems != null;
			if (flag4)
			{
				this.GlassItems.Clear();
				this.GlassItems = null;
			}
			this.DeregisterLocalizedText();
		}

		// Token: 0x06000A53 RID: 2643 RVA: 0x0003E200 File Offset: 0x0003C400
		private string ConvertTestcard(TestCardSize value)
		{
			string result = "";
			switch (value)
			{
			case TestCardSize.SingleBall:
				result = LocalizationManager.Localize("txtSingleBall");
				break;
			case TestCardSize.Testcard5:
				result = LocalizationManager.Localize("txtTestCard5");
				break;
			case TestCardSize.Testcard100:
				result = LocalizationManager.Localize("txtTestCard100");
				break;
			case TestCardSize.Other:
				result = LocalizationManager.Localize("txtOther");
				break;
			case TestCardSize.Testcard5Same:
				result = LocalizationManager.Localize("txtTestCard5Same");
				break;
			}
			return result;
		}

		// Token: 0x06000A54 RID: 2644 RVA: 0x0003E27C File Offset: 0x0003C47C
		private string ConvertMaterial(Material value)
		{
			string result = LocalizationManager.Localize("txtNA");
			switch (value)
			{
			case Material.VA:
				result = LocalizationManager.Localize("txtVA");
				break;
			case Material.Glass:
				result = LocalizationManager.Localize("txtGlass");
				break;
			case Material.VAWire:
				result = LocalizationManager.Localize("txtVAWire");
				break;
			case Material.Ceramics:
				result = LocalizationManager.Localize("txtKeramik");
				break;
			case Material.EPDM:
				result = LocalizationManager.Localize("txtEPDM");
				break;
			case Material.PTFE:
				result = LocalizationManager.Localize("txtPTFE");
				break;
			case Material.Aluminium:
				result = LocalizationManager.Localize("txtAluminium");
				break;
			case Material.Bone:
				result = LocalizationManager.Localize("txtBone");
				break;
			case Material.None:
				result = LocalizationManager.Localize("txtNA");
				break;
			case Material.Other:
				result = LocalizationManager.Localize("txtOther");
				break;
			case Material.FE:
				result = LocalizationManager.Localize("txtFE");
				break;
			case Material.NonFE:
				result = LocalizationManager.Localize("txtNonFE");
				break;
			}
			return result;
		}

		// Token: 0x06000A55 RID: 2645 RVA: 0x0003E37C File Offset: 0x0003C57C
		private TestCardSize convertStringToTestcardType(string input)
		{
			TestCardSize result = TestCardSize.Other;
			bool flag = input == Convert.ToString(TestCardSize.SingleBall);
			if (flag)
			{
				result = TestCardSize.SingleBall;
			}
			else
			{
				bool flag2 = input == Convert.ToString(TestCardSize.Testcard5);
				if (flag2)
				{
					result = TestCardSize.Testcard5;
				}
				else
				{
					bool flag3 = input == Convert.ToString(TestCardSize.Testcard100);
					if (flag3)
					{
						result = TestCardSize.Testcard100;
					}
					else
					{
						bool flag4 = input == Convert.ToString(TestCardSize.Testcard5Same);
						if (flag4)
						{
							result = TestCardSize.Testcard5Same;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06000A56 RID: 2646 RVA: 0x0003E3F8 File Offset: 0x0003C5F8
		private Material convertStringToTestcardMaterial(string input)
		{
			Material result = Material.None;
			bool flag = input == Convert.ToString(Material.VA);
			if (flag)
			{
				result = Material.VA;
			}
			else
			{
				bool flag2 = input == Convert.ToString(Material.Glass);
				if (flag2)
				{
					result = Material.Glass;
				}
				else
				{
					bool flag3 = input == Convert.ToString(Material.VAWire);
					if (flag3)
					{
						result = Material.VAWire;
					}
					else
					{
						bool flag4 = input == Convert.ToString(Material.Ceramics);
						if (flag4)
						{
							result = Material.Ceramics;
						}
						else
						{
							bool flag5 = input == Convert.ToString(Material.EPDM);
							if (flag5)
							{
								result = Material.EPDM;
							}
							else
							{
								bool flag6 = input == Convert.ToString(Material.PTFE);
								if (flag6)
								{
									result = Material.PTFE;
								}
								else
								{
									bool flag7 = input == Convert.ToString(Material.Aluminium);
									if (flag7)
									{
										result = Material.Aluminium;
									}
									else
									{
										bool flag8 = input == Convert.ToString(Material.Bone);
										if (flag8)
										{
											result = Material.Bone;
										}
										else
										{
											bool flag9 = input == Convert.ToString(Material.Other);
											if (flag9)
											{
												result = Material.Other;
											}
											else
											{
												bool flag10 = input == Convert.ToString(Material.FE);
												if (flag10)
												{
													result = Material.FE;
												}
												else
												{
													bool flag11 = input == Convert.ToString(Material.NonFE);
													if (flag11)
													{
														result = Material.NonFE;
													}
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x040006DC RID: 1756
		private DBConnection dbConnection;

		// Token: 0x040006FC RID: 1788
		private ObservableCollection<Infos> productItems = new ObservableCollection<Infos>();

		// Token: 0x040006FD RID: 1789
		private ObservableCollection<Infos> manufacturerItems = new ObservableCollection<Infos>();

		// Token: 0x040006FE RID: 1790
		private ObservableCollection<Infos> vaItems = new ObservableCollection<Infos>();

		// Token: 0x040006FF RID: 1791
		private ObservableCollection<Infos> glassItems = new ObservableCollection<Infos>();

		// Token: 0x04000700 RID: 1792
		private bool manufacturerVisible = true;

		// Token: 0x04000701 RID: 1793
		private AccessRightsManager accessManager;
	}
}
