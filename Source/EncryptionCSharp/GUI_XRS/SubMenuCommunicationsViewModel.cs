using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media;
using Common;
using DatabaseConnection;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x020000CF RID: 207
	public class SubMenuCommunicationsViewModel
	{
		// Token: 0x17000634 RID: 1588
		// (get) Token: 0x0600145F RID: 5215 RVA: 0x0007822F File Offset: 0x0007642F
		// (set) Token: 0x06001460 RID: 5216 RVA: 0x00078237 File Offset: 0x00076437
		public ObservableCollection<SubMenuItem> SubMenuItems { get; set; }

		// Token: 0x17000635 RID: 1589
		// (get) Token: 0x06001461 RID: 5217 RVA: 0x00078240 File Offset: 0x00076440
		// (set) Token: 0x06001462 RID: 5218 RVA: 0x00078248 File Offset: 0x00076448
		public RelayCommand CloseCommand { get; set; }

		// Token: 0x17000636 RID: 1590
		// (get) Token: 0x06001463 RID: 5219 RVA: 0x00078251 File Offset: 0x00076451
		// (set) Token: 0x06001464 RID: 5220 RVA: 0x00078259 File Offset: 0x00076459
		public MachineState MachineState { get; set; }

		// Token: 0x17000637 RID: 1591
		// (get) Token: 0x06001465 RID: 5221 RVA: 0x00078262 File Offset: 0x00076462
		// (set) Token: 0x06001466 RID: 5222 RVA: 0x0007826A File Offset: 0x0007646A
		public bool DemoMode { get; set; }

		// Token: 0x06001467 RID: 5223 RVA: 0x00078274 File Offset: 0x00076474
		public SubMenuCommunicationsViewModel(DBConnection dbConnection, AccessRightsManager accessManager, List<object> communicationsList, MachineState machineState)
		{
			this.dbConnection = dbConnection;
			this.accessManager = accessManager;
			this.communicationsList = communicationsList;
			this.MachineState = machineState;
			this.CloseCommand = new RelayCommand(new Action<object>(this.CloseCommunications));
			this.InitializeCommunicationItems();
		}

		// Token: 0x06001468 RID: 5224 RVA: 0x000782D4 File Offset: 0x000764D4
		private void InitializeCommunicationItems()
		{
			bool flag = this.SubMenuItems == null;
			if (flag)
			{
				this.SubMenuItems = new ObservableCollection<SubMenuItem>();
			}
			else
			{
				this.SubMenuItems.Clear();
			}
			VideojetPrinter videojetPrinter = null;
			foreach (object obj in this.communicationsList)
			{
				bool flag2 = obj.GetType().FullName == "GUI_XRS.VideojetPrinter";
				if (flag2)
				{
					videojetPrinter = (VideojetPrinter)obj;
				}
			}
			bool flag3 = videojetPrinter != null;
			if (flag3)
			{
				this.videojetPrinter = videojetPrinter;
				bool available = this.videojetPrinter.Available;
				if (available)
				{
					SubMenuItem subMenuItem = new SubMenuItem("Videojet Drucker", "menuItemVideojetPrinter", "..\\..\\img\\videojet-logo.png", new Action<object>(this.btnVideojetPrinter_Click), this.accessManager, this.rightVideojetPrinter, this.rightVideojetPrinter);
					subMenuItem.Stretch = Stretch.Uniform;
					this.SubMenuItems.Add(subMenuItem);
				}
			}
		}

		// Token: 0x06001469 RID: 5225 RVA: 0x000783E4 File Offset: 0x000765E4
		private void btnVideojetPrinter_Click(object sender)
		{
			CtrlVideojetPrinter usrControl = new CtrlVideojetPrinter(this.videojetPrinter, this.dbConnection, this.accessManager);
			Subwindow.Show(usrControl);
		}

		// Token: 0x0600146A RID: 5226 RVA: 0x00078414 File Offset: 0x00076614
		private void CloseCommunications(object obj)
		{
			foreach (SubMenuItem subMenuItem in this.SubMenuItems)
			{
				subMenuItem.DeregisterLocalizedText();
			}
		}

		// Token: 0x04000D77 RID: 3447
		private BuiltInPriviledges rightVideojetPrinter = BuiltInPriviledges.Operator3;

		// Token: 0x04000D78 RID: 3448
		private DBConnection dbConnection;

		// Token: 0x04000D79 RID: 3449
		private AccessRightsManager accessManager;

		// Token: 0x04000D7A RID: 3450
		private List<object> communicationsList;

		// Token: 0x04000D7B RID: 3451
		private VideojetPrinter videojetPrinter;
	}
}
