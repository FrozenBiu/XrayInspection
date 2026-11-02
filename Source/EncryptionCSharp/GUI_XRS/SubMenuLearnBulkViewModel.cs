using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Common;
using DatabaseConnection;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x0200015C RID: 348
	public class SubMenuLearnBulkViewModel
	{
		// Token: 0x17000987 RID: 2439
		// (get) Token: 0x060020EB RID: 8427 RVA: 0x000D6C79 File Offset: 0x000D4E79
		// (set) Token: 0x060020EC RID: 8428 RVA: 0x000D6C81 File Offset: 0x000D4E81
		public MachineState MachineState { get; set; }

		// Token: 0x17000988 RID: 2440
		// (get) Token: 0x060020ED RID: 8429 RVA: 0x000D6C8A File Offset: 0x000D4E8A
		// (set) Token: 0x060020EE RID: 8430 RVA: 0x000D6C92 File Offset: 0x000D4E92
		public bool DemoMode { get; set; }

		// Token: 0x17000989 RID: 2441
		// (get) Token: 0x060020EF RID: 8431 RVA: 0x000D6C9B File Offset: 0x000D4E9B
		// (set) Token: 0x060020F0 RID: 8432 RVA: 0x000D6CA3 File Offset: 0x000D4EA3
		public ObservableCollection<SubMenuItem> SubMenuItems { get; set; }

		// Token: 0x1700098A RID: 2442
		// (get) Token: 0x060020F1 RID: 8433 RVA: 0x000D6CAC File Offset: 0x000D4EAC
		// (set) Token: 0x060020F2 RID: 8434 RVA: 0x000D6CB4 File Offset: 0x000D4EB4
		public RelayCommand CloseCommand { get; set; }

		// Token: 0x060020F3 RID: 8435 RVA: 0x000D6CC0 File Offset: 0x000D4EC0
		public SubMenuLearnBulkViewModel(ParameterHandling parHandling, ImageDataTransfer imageTransfer, DBConnection dbConnection, AccessRightsManager accessManager, MachineState machineState, ProductManager productManager, Vibro vibro, StoreImageData imageArchiver)
		{
			this.parHandling = parHandling;
			this.imageTransfer = imageTransfer;
			this.dbConnection = dbConnection;
			this.accessManager = accessManager;
			this.MachineState = machineState;
			this.productManager = productManager;
			this.vibro = vibro;
			this.imageArchiver = imageArchiver;
			this.CloseCommand = new RelayCommand(new Action<object>(this.CloseSubMenu));
			this.FillMenuItems();
		}

		// Token: 0x060020F4 RID: 8436 RVA: 0x000D6D34 File Offset: 0x000D4F34
		private void FillMenuItems()
		{
			bool demoMode = false;
			this.dbConnection.GetDemoMode(ref demoMode);
			this.DemoMode = demoMode;
			this.SubMenuItems = new ObservableCollection<SubMenuItem>();
			SubMenuItem item = new SubMenuItem("Browse", "menuItemBrowseImages", "..\\..\\img\\xriimagelist.png", new Action<object>(this.btnBrowse_Click), this.accessManager, true, true);
			this.SubMenuItems.Add(item);
			SubMenuItem item2 = new SubMenuItem("Record", "menuItemAcquisition", "..\\..\\img\\objectxriimage.png", new Action<object>(this.btnRecord_Click), this.accessManager, true, true);
			this.SubMenuItems.Add(item2);
			SubMenuItem item3 = new SubMenuItem("LiveImage", "menuItemLiveImage", "..\\..\\img\\livexriimage.png", new Action<object>(this.btnLiveImage_Click), this.accessManager, true, true);
			this.SubMenuItems.Add(item3);
		}

		// Token: 0x060020F5 RID: 8437 RVA: 0x000D6E08 File Offset: 0x000D5008
		private void CloseSubMenu(object obj)
		{
			foreach (SubMenuItem subMenuItem in this.SubMenuItems)
			{
				subMenuItem.DeregisterLocalizedText();
			}
		}

		// Token: 0x060020F6 RID: 8438 RVA: 0x000D6E58 File Offset: 0x000D5058
		private void btnBrowse_Click(object sender)
		{
			CtrlBrowseImages ctrlBrowseImages = new CtrlBrowseImages(this.dbConnection, this.parHandling, this.imageTransfer, this.MachineState, this.productManager, this.imageArchiver);
			Subwindow.Show(ctrlBrowseImages, SubwindowButtons.BackAndHome, ctrlBrowseImages.Buttons);
		}

		// Token: 0x060020F7 RID: 8439 RVA: 0x000D6EA0 File Offset: 0x000D50A0
		private void btnRecord_Click(object sender)
		{
			bool flag = this.MachineState.State != MachineStates.Inspecting;
			if (flag)
			{
				Dialog.Show(LocalizationManager.Localize("txtInfo"), LocalizationManager.Localize("txtTurnXrayOn") + ".", DialogStyle.Info);
			}
			else
			{
				string str = null;
				this.dbConnection.GetCurrentProductTableName(ref str);
				string path = StoreImageData.archiveRoot + "\\" + str;
				bool flag2;
				if (Directory.Exists(path))
				{
					flag2 = ((from name in Directory.GetFiles(path)
					where !name.EndsWith(LiveModeViewModel.liveImageName)
					select name).Count<string>() < RecordImagesViewModel.maxProductImages);
				}
				else
				{
					flag2 = true;
				}
				bool flag3 = flag2;
				if (flag3)
				{
					bool flag4 = Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtTurningOffProductInspection"), DialogStyle.Warning, DialogButtons.OKCancel) == Dialog.DialogResults.Ok;
					if (flag4)
					{
						CtrlRecordImages ctrlRecordImages = new CtrlRecordImages(this.dbConnection, this.parHandling, this.imageTransfer, this.MachineState, this.productManager, this.vibro, this.accessManager);
						Subwindow.Show(ctrlRecordImages, SubwindowButtons.None, ctrlRecordImages.Buttons);
					}
				}
				else
				{
					Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtMaxImagesAcquired"), DialogStyle.Error, DialogButtons.OK);
				}
			}
		}

		// Token: 0x060020F8 RID: 8440 RVA: 0x000D6FE0 File Offset: 0x000D51E0
		private void btnLiveImage_Click(object obj)
		{
			bool flag = this.MachineState.State != MachineStates.Inspecting;
			if (flag)
			{
				Dialog.Show(LocalizationManager.Localize("txtInfo"), LocalizationManager.Localize("txtTurnXrayOn") + ".", DialogStyle.Info);
			}
			else
			{
				CtrlLiveMode ctrlLiveMode = new CtrlLiveMode(this.dbConnection, this.parHandling, this.imageTransfer, this.productManager, this.imageArchiver);
				Subwindow.Show(ctrlLiveMode, SubwindowButtons.BackAndHome, ctrlLiveMode.Buttons);
			}
		}

		// Token: 0x04001553 RID: 5459
		private ParameterHandling parHandling;

		// Token: 0x04001554 RID: 5460
		private ImageDataTransfer imageTransfer;

		// Token: 0x04001555 RID: 5461
		private DBConnection dbConnection;

		// Token: 0x04001556 RID: 5462
		private AccessRightsManager accessManager;

		// Token: 0x04001558 RID: 5464
		private ProductManager productManager;

		// Token: 0x04001559 RID: 5465
		private Vibro vibro;

		// Token: 0x0400155A RID: 5466
		private StoreImageData imageArchiver;
	}
}
