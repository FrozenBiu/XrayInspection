using System;
using System.Collections.ObjectModel;
using System.Threading;
using Common;
using DatabaseConnection;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x02000097 RID: 151
	public class SubMenuAuditcheckViewModel
	{
		// Token: 0x170004AE RID: 1198
		// (get) Token: 0x06000F5D RID: 3933 RVA: 0x0005B8DC File Offset: 0x00059ADC
		// (set) Token: 0x06000F5E RID: 3934 RVA: 0x0005B8E4 File Offset: 0x00059AE4
		public ObservableCollection<SubMenuItem> SubMenuItems { get; set; }

		// Token: 0x170004AF RID: 1199
		// (get) Token: 0x06000F5F RID: 3935 RVA: 0x0005B8ED File Offset: 0x00059AED
		// (set) Token: 0x06000F60 RID: 3936 RVA: 0x0005B8F5 File Offset: 0x00059AF5
		public RelayCommand CloseCommand { get; set; }

		// Token: 0x170004B0 RID: 1200
		// (get) Token: 0x06000F61 RID: 3937 RVA: 0x0005B8FE File Offset: 0x00059AFE
		// (set) Token: 0x06000F62 RID: 3938 RVA: 0x0005B906 File Offset: 0x00059B06
		public MachineState MachineState { get; set; }

		// Token: 0x170004B1 RID: 1201
		// (get) Token: 0x06000F63 RID: 3939 RVA: 0x0005B90F File Offset: 0x00059B0F
		// (set) Token: 0x06000F64 RID: 3940 RVA: 0x0005B917 File Offset: 0x00059B17
		public ParameterHandling parameterHandling { get; set; }

		// Token: 0x170004B2 RID: 1202
		// (get) Token: 0x06000F65 RID: 3941 RVA: 0x0005B920 File Offset: 0x00059B20
		// (set) Token: 0x06000F66 RID: 3942 RVA: 0x0005B928 File Offset: 0x00059B28
		public Backup backup { get; set; }

		// Token: 0x170004B3 RID: 1203
		// (get) Token: 0x06000F67 RID: 3943 RVA: 0x0005B931 File Offset: 0x00059B31
		// (set) Token: 0x06000F68 RID: 3944 RVA: 0x0005B939 File Offset: 0x00059B39
		public bool DemoMode { get; set; }

		// Token: 0x06000F69 RID: 3945 RVA: 0x0005B944 File Offset: 0x00059B44
		public SubMenuAuditcheckViewModel(ParameterHandling parHandling, DBConnection dbConnection, AccessRightsManager accessManager, MachineState machineState, ImageDataTransfer imageTransfer, Backup backup, ProductManager productManager, MessageSystem messageSystem, AuditCheckSettings auditCheckSettings, Restore restore, int imageGroup)
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
			this.restore = restore;
			this.auditCheckSettings = auditCheckSettings;
			this.imageGroup = imageGroup;
			this.CloseCommand = new RelayCommand(new Action<object>(this.CloseSubMenu));
			this.RegisterLocalizedText();
			this.InitSubMenus();
		}

		// Token: 0x06000F6A RID: 3946 RVA: 0x0005BA18 File Offset: 0x00059C18
		private void InitSubMenus()
		{
			this.SubMenuItems = new ObservableCollection<SubMenuItem>();
			bool flag = false;
			this.dBConnection.GetAuditCheckDocumentation(ref flag);
			bool flag2 = flag;
			if (flag2)
			{
				SubMenuItem item = new SubMenuItem("Dokumente", "menuItemDocuments", "..\\..\\img\\Auditcheck_new.png", "..\\..\\img\\Auditcheck_new.png", new Action<object>(this.btnAuditCheckCertificates_Click), this.accessManager, BuiltInPriviledges.Operator3, BuiltInPriviledges.Operator3);
				this.SubMenuItems.Add(item);
				SubMenuItem item2 = new SubMenuItem("Inhalt Dokumente", "menuItemDocumentsContent", "..\\..\\img\\Auditcheck_content.png", "..\\..\\img\\Auditcheck_content.png", new Action<object>(this.btnAuditCheckDocContent_Click), this.accessManager, BuiltInPriviledges.Operator1, BuiltInPriviledges.Operator1);
				this.SubMenuItems.Add(item2);
			}
			SubMenuItem item3 = new SubMenuItem("Filter", "menuItemFilter", "..\\..\\img\\filter_aktiv.png", "..\\..\\img\\filter_aktiv.png", new Action<object>(this.btnAuditCheckFilter_Click), this.accessManager, BuiltInPriviledges.Operator3, BuiltInPriviledges.Operator3);
			this.SubMenuItems.Add(item3);
			SubMenuItem item4 = new SubMenuItem("Testkörper", "menuItemIVPTestsamples", "..\\..\\img\\testballs.png", "..\\..\\img\\testballs_disabled.png", new Action<object>(this.btnAuditCheckTestsamples_Click), this.accessManager, BuiltInPriviledges.Operator1, BuiltInPriviledges.Operator1);
			this.SubMenuItems.Add(item4);
			SubMenuItem item5 = new SubMenuItem("Einstellungen", "txtSettings", "..\\..\\img\\settings.png", "..\\..\\img\\settings.png", new Action<object>(this.btnAuditCheckSettings_Click), this.accessManager, BuiltInPriviledges.Operator1, BuiltInPriviledges.Operator1);
			this.SubMenuItems.Add(item5);
			SubMenuItem item6 = new SubMenuItem("Audit Check durchführen", "menuItemAuditCheckStart", "..\\..\\img\\play_big.png", "..\\..\\img\\play_big_disabled.png", new Action<object>(this.btnAuditCheckStart_Click), this.accessManager, BuiltInPriviledges.Operator3, BuiltInPriviledges.Operator3);
			this.SubMenuItems.Add(item6);
		}

		// Token: 0x06000F6B RID: 3947 RVA: 0x0005BBE8 File Offset: 0x00059DE8
		private void btnAuditCheckCertificates_Click(object sender)
		{
			CtrlAuditCheckDocOverview usrControl = new CtrlAuditCheckDocOverview(this.accessManager, this.dBConnection);
			Subwindow.Show(usrControl, SubwindowButtons.BackAndHome);
		}

		// Token: 0x06000F6C RID: 3948 RVA: 0x0005BC10 File Offset: 0x00059E10
		private void btnAuditCheckDocContent_Click(object sender)
		{
			CtrlAuditCheckDocContent usrControl = new CtrlAuditCheckDocContent(this.dBConnection, this.accessManager);
			Subwindow.Show(usrControl, SubwindowButtons.BackAndHome);
		}

		// Token: 0x06000F6D RID: 3949 RVA: 0x0005BC38 File Offset: 0x00059E38
		private void btnAuditCheckTestsamples_Click(object sender)
		{
			CtrlAuditCheckTestsamples usrControl = new CtrlAuditCheckTestsamples(this.accessManager, this.dBConnection);
			Subwindow.Show(usrControl, SubwindowButtons.BackAndHome);
		}

		// Token: 0x06000F6E RID: 3950 RVA: 0x0005BC60 File Offset: 0x00059E60
		private void btnAuditCheckSettings_Click(object sender)
		{
			CtrlAuditCheckSettings usrControl = new CtrlAuditCheckSettings(this.auditCheckSettings, this.dBConnection, this.accessManager, this.parameterHandling, this.imageGroup, this.productManager);
			Subwindow.Show(usrControl, SubwindowButtons.BackAndHome);
		}

		// Token: 0x06000F6F RID: 3951 RVA: 0x0005BCA0 File Offset: 0x00059EA0
		private void btnAuditCheckFilter_Click(object sender)
		{
			CtrlAuditCheckSavedEntries usrControl = new CtrlAuditCheckSavedEntries(this.accessManager, this.dBConnection);
			Subwindow.Show(usrControl, SubwindowButtons.BackAndHome);
		}

		// Token: 0x06000F70 RID: 3952 RVA: 0x0005BCC8 File Offset: 0x00059EC8
		private void btnAuditCheckStart_Click(object sender)
		{
			ProductType productType = ProductType.SingleProduct;
			string tableName = "";
			this.dBConnection.GetCurrentProductTableName(ref tableName);
			this.dBConnection.GetProductType(tableName, ref productType);
			bool flag = productType == ProductType.RegionProduct;
			bool isRegion = flag;
			bool flag2 = productType == ProductType.MultiProduct || productType == ProductType.LaneProduct;
			bool isMulti = flag2;
			bool flag3 = this.checkIfProductIsTeachedIn(isRegion, isMulti);
			if (flag3)
			{
				Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtProductNotTeachedIn"), DialogStyle.Warning, DialogButtons.OK);
			}
			else
			{
				bool flag4 = !this.checkTestcards() && this.auditCheckSettings.ContaminationEnabled;
				if (flag4)
				{
					Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtTestsamplesNotSetAudit"), DialogStyle.Warning, DialogButtons.OK);
				}
				else
				{
					bool flag5 = this.MachineState.State != MachineStates.Inspecting;
					if (flag5)
					{
						Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtTurnXrayOn") + ".", DialogStyle.Warning, DialogButtons.OK);
					}
					else
					{
						bool flag6 = !this.auditCheckSettings.ContaminationEnabled && !this.auditCheckSettings.CountEnabled && !this.auditCheckSettings.WeightEnabled;
						if (flag6)
						{
							Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtNoAuditActive"), DialogStyle.Error, DialogButtons.OK);
						}
						else
						{
							Thread thread = new Thread(new ThreadStart(this.auditCheckSettings.AuditRequired.TriggerAuditCheck));
							thread.Start();
						}
					}
				}
			}
		}

		// Token: 0x06000F71 RID: 3953 RVA: 0x0005BE50 File Offset: 0x0005A050
		private bool checkIfProductIsTeachedIn(bool isRegion, bool isMulti)
		{
			bool result = true;
			if (isMulti)
			{
				ObservableCollection<ProductData> subProducts = this.productManager.CurrentLoadedProduct.SubProducts;
				string text = "";
				int num = 1;
				foreach (ProductData productData in subProducts)
				{
					bool flag = !productData.ProductName.Equals("");
					if (flag)
					{
						string tableName = productData.TableName;
						this.dBConnection.GetParameterValue(string.Concat(new string[]
						{
							Definition.filterMainPath,
							num.ToString(),
							Definition.comparerSubPath,
							"1/Contrast/",
							Definition.strValue
						}), tableName, ref text);
						bool flag2 = text.Length > 0;
						int num2;
						if (flag2)
						{
							num2 = Convert.ToInt32(text);
						}
						else
						{
							num2 = 0;
						}
						this.dBConnection.GetParameterValue(string.Concat(new string[]
						{
							Definition.filterMainPath,
							num.ToString(),
							Definition.objectFindersSubPath,
							"1/Contrast/",
							Definition.strValue
						}), tableName, ref text);
						bool flag3 = text.Length > 0;
						int num3;
						if (flag3)
						{
							num3 = Convert.ToInt32(text);
						}
						else
						{
							num3 = 0;
						}
						num++;
						bool flag4 = (num2 > 0 && num2 < 32000) || (num3 > 0 && num3 < 32000);
						if (flag4)
						{
							result = false;
						}
					}
				}
			}
			else
			{
				int num2;
				int num3;
				if (isRegion)
				{
					string text2 = this.parameterHandling.ReadPar(string.Concat(new string[]
					{
						Definition.filterMainPath,
						"1",
						Definition.comparerSubPath,
						"1/Contrast/",
						Definition.strValue
					}));
					bool flag5 = text2.Length > 0;
					if (flag5)
					{
						num2 = Convert.ToInt32(text2);
						num3 = Convert.ToInt32(this.parameterHandling.ReadPar(string.Concat(new string[]
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
						num2 = 0;
						num3 = 0;
					}
				}
				else
				{
					string text3 = this.parameterHandling.ReadPar(Definition.filterMainPath + Definition.comparerSubPath + "1/Contrast/" + Definition.strValue);
					bool flag6 = text3.Length > 0;
					if (flag6)
					{
						num2 = Convert.ToInt32(text3);
						num3 = Convert.ToInt32(this.parameterHandling.ReadPar(Definition.filterMainPath + Definition.objectFindersSubPath + "1/Contrast/" + Definition.strValue));
					}
					else
					{
						num2 = 0;
						num3 = 0;
					}
				}
				bool flag7 = (num2 > 0 && num2 < 32000) || (num3 > 0 && num3 < 32000);
				if (flag7)
				{
					result = false;
				}
			}
			return result;
		}

		// Token: 0x06000F72 RID: 3954 RVA: 0x0005C14C File Offset: 0x0005A34C
		private bool checkTestcards()
		{
			bool result = false;
			string text = "";
			this.dBConnection.GetParameterValue(this.TestcardMaterialPath1, ref text);
			bool flag = text != null && text != "n.a." && text != "" && text != "None";
			if (flag)
			{
				result = true;
			}
			else
			{
				this.dBConnection.GetParameterValue(this.TestcardMaterialPath2, ref text);
				bool flag2 = text != null && text != "n.a." && text != "" && text != "None";
				if (flag2)
				{
					result = true;
				}
				else
				{
					this.dBConnection.GetParameterValue(this.TestcardMaterialPath3, ref text);
					bool flag3 = text != null && text != "n.a." && text != "" && text != "None";
					if (flag3)
					{
						result = true;
					}
					else
					{
						this.dBConnection.GetParameterValue(this.TestcardMaterialPath4, ref text);
						bool flag4 = text != null && text != "n.a." && text != "" && text != "None";
						if (flag4)
						{
							result = true;
						}
						else
						{
							this.dBConnection.GetParameterValue(this.TestcardMaterialPath5, ref text);
							bool flag5 = text != null && text != "n.a." && text != "" && text != "None";
							if (flag5)
							{
								result = true;
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06000F73 RID: 3955 RVA: 0x0001092C File Offset: 0x0000EB2C
		private void RegisterLocalizedText()
		{
		}

		// Token: 0x06000F74 RID: 3956 RVA: 0x0005C2EC File Offset: 0x0005A4EC
		private void CloseSubMenu(object obj)
		{
			foreach (SubMenuItem subMenuItem in this.SubMenuItems)
			{
				subMenuItem.DeregisterLocalizedText();
			}
		}

		// Token: 0x04000A46 RID: 2630
		private AccessRightsManager accessManager;

		// Token: 0x04000A4B RID: 2635
		public ImageDataTransfer imageTransfer;

		// Token: 0x04000A4D RID: 2637
		private MessageSystem messageSystem;

		// Token: 0x04000A4E RID: 2638
		public ProductManager productManager;

		// Token: 0x04000A4F RID: 2639
		private DBConnection dBConnection;

		// Token: 0x04000A50 RID: 2640
		private AuditCheckSettings auditCheckSettings;

		// Token: 0x04000A51 RID: 2641
		private int imageGroup;

		// Token: 0x04000A52 RID: 2642
		private readonly string TestcardMaterialPath1 = "TestcardMaterial1";

		// Token: 0x04000A53 RID: 2643
		private readonly string TestcardMaterialPath2 = "TestcardMaterial2";

		// Token: 0x04000A54 RID: 2644
		private readonly string TestcardMaterialPath3 = "TestcardMaterial3";

		// Token: 0x04000A55 RID: 2645
		private readonly string TestcardMaterialPath4 = "TestcardMaterial4";

		// Token: 0x04000A56 RID: 2646
		private readonly string TestcardMaterialPath5 = "TestcardMaterial5";

		// Token: 0x04000A57 RID: 2647
		private Restore restore;
	}
}
