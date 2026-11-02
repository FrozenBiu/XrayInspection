using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Common;
using DatabaseConnection;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x02000098 RID: 152
	public class SubMenuIVPViewModel
	{
		// Token: 0x170004B4 RID: 1204
		// (get) Token: 0x06000F75 RID: 3957 RVA: 0x0005C33C File Offset: 0x0005A53C
		// (set) Token: 0x06000F76 RID: 3958 RVA: 0x0005C344 File Offset: 0x0005A544
		public ObservableCollection<SubMenuItem> SubMenuItems { get; set; }

		// Token: 0x170004B5 RID: 1205
		// (get) Token: 0x06000F77 RID: 3959 RVA: 0x0005C34D File Offset: 0x0005A54D
		// (set) Token: 0x06000F78 RID: 3960 RVA: 0x0005C355 File Offset: 0x0005A555
		public RelayCommand CloseCommand { get; set; }

		// Token: 0x170004B6 RID: 1206
		// (get) Token: 0x06000F79 RID: 3961 RVA: 0x0005C35E File Offset: 0x0005A55E
		// (set) Token: 0x06000F7A RID: 3962 RVA: 0x0005C366 File Offset: 0x0005A566
		public MachineState MachineState { get; set; }

		// Token: 0x170004B7 RID: 1207
		// (get) Token: 0x06000F7B RID: 3963 RVA: 0x0005C36F File Offset: 0x0005A56F
		// (set) Token: 0x06000F7C RID: 3964 RVA: 0x0005C377 File Offset: 0x0005A577
		public ParameterHandling parameterHandling { get; set; }

		// Token: 0x170004B8 RID: 1208
		// (get) Token: 0x06000F7D RID: 3965 RVA: 0x0005C380 File Offset: 0x0005A580
		// (set) Token: 0x06000F7E RID: 3966 RVA: 0x0005C388 File Offset: 0x0005A588
		public Backup backup { get; set; }

		// Token: 0x170004B9 RID: 1209
		// (get) Token: 0x06000F7F RID: 3967 RVA: 0x0005C391 File Offset: 0x0005A591
		// (set) Token: 0x06000F80 RID: 3968 RVA: 0x0005C399 File Offset: 0x0005A599
		public bool DemoMode { get; set; }

		// Token: 0x170004BA RID: 1210
		// (get) Token: 0x06000F81 RID: 3969 RVA: 0x0005C3A2 File Offset: 0x0005A5A2
		// (set) Token: 0x06000F82 RID: 3970 RVA: 0x0005C3AA File Offset: 0x0005A5AA
		public BoundLocalizedText StrIVP { get; set; }

		// Token: 0x06000F83 RID: 3971 RVA: 0x0005C3B4 File Offset: 0x0005A5B4
		public SubMenuIVPViewModel(ParameterHandling parHandling, DBConnection dbConnection, AccessRightsManager accessManager, MachineState machineState, ImageDataTransfer imageTransfer, Backup backup, ProductManager productManager, MessageSystem messageSystem)
		{
			this.accessManager = accessManager;
			this.MachineState = machineState;
			this.DemoMode = false;
			this.parameterHandling = parHandling;
			this.imageTransfer = imageTransfer;
			this.dBConnection = dbConnection;
			this.backup = backup;
			this.productManager = productManager;
			this.messageSystem = messageSystem;
			this.CloseCommand = new RelayCommand(new Action<object>(this.CloseSubMenu));
			this.RegisterLocalizedText();
			this.InitSubMenus();
		}

		// Token: 0x06000F84 RID: 3972 RVA: 0x0005C438 File Offset: 0x0005A638
		private void InitSubMenus()
		{
			this.SubMenuItems = new ObservableCollection<SubMenuItem>();
			SubMenuItem item = new SubMenuItem("Zertifikate", "menuItemIVPCertificats", "..\\..\\img\\certificate_color2.png", "..\\..\\img\\certificate_color_disabled2.png", new Action<object>(this.btnIVPCertificates_Click), this.accessManager, BuiltInPriviledges.Operator1, BuiltInPriviledges.Operator1);
			this.SubMenuItems.Add(item);
			bool flag = this.accessManager.CurrentUser.ID != -3 || !this.productManager.CurrentLoadedProduct.ProductNumber.Equals("-1");
			if (flag)
			{
				SubMenuItem item2 = new SubMenuItem("Inhalt Zertifikate", "menuItemIVPCertificatsContent", "..\\..\\img\\edituser_big.png", "..\\..\\img\\edituser_big_disabled.png", new Action<object>(this.btnIVPCertificatesContent_Click), this.accessManager, BuiltInPriviledges.Operator1, BuiltInPriviledges.Operator1);
				this.SubMenuItems.Add(item2);
			}
			bool flag2 = !this.productManager.CurrentLoadedProduct.ProductNumber.Equals("-1");
			if (flag2)
			{
				SubMenuItem item3 = new SubMenuItem("Testkörper", "menuItemIVPTestsamples", "..\\..\\img\\testballs.png", "..\\..\\img\\testballs_disabled.png", new Action<object>(this.btnIVPTestsamples_Click), this.accessManager, BuiltInPriviledges.Operator1, BuiltInPriviledges.Operator1);
				this.SubMenuItems.Add(item3);
			}
			SubMenuItem item4 = new SubMenuItem("Einstellungen", "txtSettings", "..\\..\\img\\settings.png", "..\\..\\img\\settings.png", new Action<object>(this.btnIVPSettings_Click), this.accessManager, BuiltInPriviledges.Operator1, BuiltInPriviledges.Operator1);
			this.SubMenuItems.Add(item4);
			bool flag3 = (this.accessManager.CurrentUser.ID == -3 || this.accessManager.CurrentUser.ID == -2 || this.accessManager.CurrentUser.ID == -1) && this.productManager.CurrentLoadedProduct.ProductNumber.Equals("-1");
			if (flag3)
			{
				SubMenuItem item5 = new SubMenuItem("Referenzprodukt", "txtReferenceproduct", "..\\..\\img\\ivpReferenz.png", "..\\..\\img\\ivpReferenz.png", new Action<object>(this.btnIVPShowOverview_Click), this.accessManager, BuiltInPriviledges.Service, BuiltInPriviledges.Service);
				this.SubMenuItems.Add(item5);
			}
			SubMenuItem item6 = new SubMenuItem("Validierung durchführen", "menuItemIVPStart", "..\\..\\img\\play_big.png", "..\\..\\img\\play_big_disabled.png", new Action<object>(this.btnIVPStart_Click), this.accessManager, BuiltInPriviledges.Operator1, BuiltInPriviledges.Operator1);
			this.SubMenuItems.Add(item6);
		}

		// Token: 0x06000F85 RID: 3973 RVA: 0x0005C6AC File Offset: 0x0005A8AC
		private void btnIVPShowOverview_Click(object obj)
		{
			CtrlOverviewIVPReferenceProduct usrControl = new CtrlOverviewIVPReferenceProduct(this.dBConnection, this.accessManager);
			Subwindow.Show(usrControl, SubwindowButtons.BackAndHome);
		}

		// Token: 0x06000F86 RID: 3974 RVA: 0x0005C6D4 File Offset: 0x0005A8D4
		private void btnIVPCertificates_Click(object sender)
		{
			CtrlIVPCertificatesOverview ctrlIVPCertificatesOverview = new CtrlIVPCertificatesOverview(this.accessManager, this.dBConnection);
			Subwindow.Show(ctrlIVPCertificatesOverview, SubwindowButtons.BackAndHome, ctrlIVPCertificatesOverview.UiElementRightAligned, 1);
		}

		// Token: 0x06000F87 RID: 3975 RVA: 0x0005C704 File Offset: 0x0005A904
		private void btnIVPCertificatesContent_Click(object sender)
		{
			CtrlIVPCertificatesContent ctrlIVPCertificatesContent = new CtrlIVPCertificatesContent(this.dBConnection, this.accessManager, this.productManager);
			Subwindow.Show(ctrlIVPCertificatesContent, SubwindowButtons.BackAndHome, ctrlIVPCertificatesContent.UiElementRightAligned, 1);
		}

		// Token: 0x06000F88 RID: 3976 RVA: 0x0005C73C File Offset: 0x0005A93C
		private void btnIVPTestsamples_Click(object sender)
		{
			CtrlIVPTestsamples ctrlIVPTestsamples = new CtrlIVPTestsamples(this.accessManager, this.dBConnection);
			Subwindow.Show(ctrlIVPTestsamples, SubwindowButtons.BackAndHome, ctrlIVPTestsamples.UiElementRightAligned, 1);
		}

		// Token: 0x06000F89 RID: 3977 RVA: 0x0005C76C File Offset: 0x0005A96C
		private void btnIVPSettings_Click(object sender)
		{
			CtrlIVPSettings ctrlIVPSettings = new CtrlIVPSettings(this.dBConnection, this.parameterHandling, this.productManager);
			Subwindow.Show(ctrlIVPSettings, SubwindowButtons.BackAndHome, ctrlIVPSettings.UiElementRightAligned, 1);
		}

		// Token: 0x06000F8A RID: 3978 RVA: 0x0005C7A4 File Offset: 0x0005A9A4
		private void btnIVPStart_Click(object sender)
		{
			ProductType productType = ProductType.SingleProduct;
			string tableName = "";
			this.dBConnection.GetCurrentProductTableName(ref tableName);
			this.dBConnection.GetProductType(tableName, ref productType);
			string text = "";
			this.dBConnection.GetSerialNumber(ref text);
			bool flag = text.Equals("");
			if (flag)
			{
				Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtNoSerialNumber"), DialogStyle.Warning, DialogButtons.OK);
			}
			else
			{
				bool flag2 = productType == ProductType.SingleProduct || productType == ProductType.RegionProduct;
				if (flag2)
				{
					bool flag3 = productType == ProductType.RegionProduct;
					bool isRegion = flag3;
					bool flag4 = this.checkIfProductIsTeachedIn(isRegion);
					if (flag4)
					{
						Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtProductNotTeachedIn"), DialogStyle.Warning, DialogButtons.OK);
					}
					else
					{
						bool flag5 = !this.checkTestcards() && !this.productManager.CurrentLoadedProduct.ProductNumber.Equals("-1");
						if (flag5)
						{
							Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtNoTestcardsForIVP"), DialogStyle.Warning, DialogButtons.OK);
						}
						else
						{
							bool flag6 = this.MachineState.State != MachineStates.Inspecting;
							if (flag6)
							{
								Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtTurnXrayOn") + ".", DialogStyle.Warning, DialogButtons.OK);
							}
							else
							{
								CtrlIVPProcess usrControl = new CtrlIVPProcess(this.accessManager, this.MachineState, this.parameterHandling, this.imageTransfer, this.dBConnection, this.backup, 0, "", this.messageSystem, this.productManager);
								Subwindow.Show(usrControl, SubwindowButtons.BackAndHome);
							}
						}
					}
				}
				else
				{
					bool flag7 = productType == ProductType.MultiProduct || productType == ProductType.LaneProduct;
					if (flag7)
					{
						bool flag8 = !this.checkTestcards();
						if (flag8)
						{
							Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtNoTestcardsForIVP"), DialogStyle.Warning, DialogButtons.OK);
						}
						else
						{
							bool flag9 = this.MachineState.State != MachineStates.Inspecting;
							if (flag9)
							{
								Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtTurnXrayOn") + ".", DialogStyle.Warning, DialogButtons.OK);
							}
							else
							{
								CtrlIVPMultiChoice usrControl2 = new CtrlIVPMultiChoice(this.productManager, this.dBConnection, this.parameterHandling, this.accessManager, this.MachineState, this.imageTransfer, this.backup, this.messageSystem);
								Subwindow.Show(usrControl2, SubwindowButtons.BackAndHome);
							}
						}
					}
				}
			}
		}

		// Token: 0x06000F8B RID: 3979 RVA: 0x0005CA2C File Offset: 0x0005AC2C
		private bool checkIfProductIsTeachedIn(bool isRegion)
		{
			bool result = true;
			int num;
			int num2;
			if (isRegion)
			{
				num = Convert.ToInt32(this.parameterHandling.ReadPar(string.Concat(new string[]
				{
					Definition.filterMainPath,
					"1",
					Definition.comparerSubPath,
					"1/Contrast/",
					Definition.strValue
				})));
				num2 = Convert.ToInt32(this.parameterHandling.ReadPar(string.Concat(new string[]
				{
					Definition.filterMainPath,
					"1",
					Definition.objectFindersSubPath,
					"1/Contrast/",
					Definition.strValue
				})));
			}
			else
			{
				num = Convert.ToInt32(this.parameterHandling.ReadPar(Definition.filterMainPath + Definition.comparerSubPath + "1/Contrast/" + Definition.strValue));
				num2 = Convert.ToInt32(this.parameterHandling.ReadPar(Definition.filterMainPath + Definition.objectFindersSubPath + "1/Contrast/" + Definition.strValue));
			}
			bool flag = (num > 0 && num < 32000) || (num2 > 0 && num2 < 32000);
			if (flag)
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06000F8C RID: 3980 RVA: 0x0005CB54 File Offset: 0x0005AD54
		private bool checkTestcards()
		{
			bool result = false;
			ObservableCollection<InfosTestsamples> observableCollection = new ObservableCollection<InfosTestsamples>();
			List<string> list = new List<string>();
			for (int i = 0; i < 5; i++)
			{
				this.dBConnection.GetIVPTestcardRow(i, ref list);
				bool flag = list[0] != null && list[0] != "n.a." && list[0] != "" && list[0] != "None";
				if (flag)
				{
					result = true;
					break;
				}
				list = new List<string>();
			}
			return result;
		}

		// Token: 0x06000F8D RID: 3981 RVA: 0x0005CBEE File Offset: 0x0005ADEE
		private void RegisterLocalizedText()
		{
			this.StrIVP = new BoundLocalizedText("menuItemIVP");
		}

		// Token: 0x06000F8E RID: 3982 RVA: 0x0005CC02 File Offset: 0x0005AE02
		private void DeregisterLocalizedText()
		{
			MainWindow.localizationManager.Deregister(this.StrIVP);
		}

		// Token: 0x06000F8F RID: 3983 RVA: 0x0005CC18 File Offset: 0x0005AE18
		private void CloseSubMenu(object obj)
		{
			this.DeregisterLocalizedText();
			foreach (SubMenuItem subMenuItem in this.SubMenuItems)
			{
				subMenuItem.DeregisterLocalizedText();
			}
		}

		// Token: 0x04000A59 RID: 2649
		private AccessRightsManager accessManager;

		// Token: 0x04000A5E RID: 2654
		public ImageDataTransfer imageTransfer;

		// Token: 0x04000A60 RID: 2656
		private MessageSystem messageSystem;

		// Token: 0x04000A61 RID: 2657
		public ProductManager productManager;

		// Token: 0x04000A62 RID: 2658
		private DBConnection dBConnection;
	}
}
