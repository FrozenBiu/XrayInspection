using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Threading;
using Common;
using DatabaseConnection;

namespace GUI_XRS
{
	// Token: 0x0200003A RID: 58
	public class BatchesViewModel : INotifyPropertyChanged
	{
		// Token: 0x14000024 RID: 36
		// (add) Token: 0x06000643 RID: 1603 RVA: 0x0002A744 File Offset: 0x00028944
		// (remove) Token: 0x06000644 RID: 1604 RVA: 0x0002A77C File Offset: 0x0002897C
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x170001DB RID: 475
		// (get) Token: 0x06000645 RID: 1605 RVA: 0x0002A7B1 File Offset: 0x000289B1
		// (set) Token: 0x06000646 RID: 1606 RVA: 0x0002A7B9 File Offset: 0x000289B9
		public string CurrentProductName { get; set; }

		// Token: 0x170001DC RID: 476
		// (get) Token: 0x06000647 RID: 1607 RVA: 0x0002A7C2 File Offset: 0x000289C2
		// (set) Token: 0x06000648 RID: 1608 RVA: 0x0002A7CA File Offset: 0x000289CA
		public ObservableCollection<Batch> UIBatches { get; private set; }

		// Token: 0x170001DD RID: 477
		// (get) Token: 0x06000649 RID: 1609 RVA: 0x0002A7D3 File Offset: 0x000289D3
		// (set) Token: 0x0600064A RID: 1610 RVA: 0x0002A7DB File Offset: 0x000289DB
		public RelayCommand NewBatchCommand { get; set; }

		// Token: 0x170001DE RID: 478
		// (get) Token: 0x0600064B RID: 1611 RVA: 0x0002A7E4 File Offset: 0x000289E4
		// (set) Token: 0x0600064C RID: 1612 RVA: 0x0002A7EC File Offset: 0x000289EC
		public RelayCommand StartBatchCommand { get; set; }

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x0600064D RID: 1613 RVA: 0x0002A7F5 File Offset: 0x000289F5
		// (set) Token: 0x0600064E RID: 1614 RVA: 0x0002A7FD File Offset: 0x000289FD
		public RelayCommand RenameBatchCommand { get; set; }

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x0600064F RID: 1615 RVA: 0x0002A806 File Offset: 0x00028A06
		// (set) Token: 0x06000650 RID: 1616 RVA: 0x0002A80E File Offset: 0x00028A0E
		public RelayCommand DeleteBatchCommand { get; set; }

		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x06000651 RID: 1617 RVA: 0x0002A817 File Offset: 0x00028A17
		// (set) Token: 0x06000652 RID: 1618 RVA: 0x0002A81F File Offset: 0x00028A1F
		public RelayCommand CleanUpCommand { get; set; }

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x06000653 RID: 1619 RVA: 0x0002A828 File Offset: 0x00028A28
		// (set) Token: 0x06000654 RID: 1620 RVA: 0x0002A830 File Offset: 0x00028A30
		public RelayCommand InitializeCommand { get; set; }

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x06000655 RID: 1621 RVA: 0x0002A83C File Offset: 0x00028A3C
		// (set) Token: 0x06000656 RID: 1622 RVA: 0x0002A854 File Offset: 0x00028A54
		public Batch SelectedBatch
		{
			get
			{
				return this.selectedBatch;
			}
			set
			{
				this.selectedBatch = value;
				this.OnPropertyChanged("SelectedBatch");
			}
		}

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x06000657 RID: 1623 RVA: 0x0002A86A File Offset: 0x00028A6A
		// (set) Token: 0x06000658 RID: 1624 RVA: 0x0002A872 File Offset: 0x00028A72
		public ProductManager ProductManager { get; set; }

		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x06000659 RID: 1625 RVA: 0x0002A87B File Offset: 0x00028A7B
		// (set) Token: 0x0600065A RID: 1626 RVA: 0x0002A883 File Offset: 0x00028A83
		public BoundLocalizedText StrThroughput { get; set; }

		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x0600065B RID: 1627 RVA: 0x0002A88C File Offset: 0x00028A8C
		// (set) Token: 0x0600065C RID: 1628 RVA: 0x0002A894 File Offset: 0x00028A94
		public BoundLocalizedText StrReject { get; set; }

		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x0600065D RID: 1629 RVA: 0x0002A89D File Offset: 0x00028A9D
		// (set) Token: 0x0600065E RID: 1630 RVA: 0x0002A8A5 File Offset: 0x00028AA5
		public BoundLocalizedText StrStartTime { get; set; }

		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x0600065F RID: 1631 RVA: 0x0002A8AE File Offset: 0x00028AAE
		// (set) Token: 0x06000660 RID: 1632 RVA: 0x0002A8B6 File Offset: 0x00028AB6
		public BoundLocalizedText StrEndTime { get; set; }

		// Token: 0x06000661 RID: 1633 RVA: 0x0002A8C0 File Offset: 0x00028AC0
		public BatchesViewModel(DBConnection dbConnection, ProductManager productManager, ImageDataTransfer imageTransfer, bool showNewBatchKeyboard)
		{
			this.dbConnection = dbConnection;
			this.ProductManager = productManager;
			this.imageTransfer = imageTransfer;
			this.UIBatches = new ObservableCollection<Batch>();
			this.NewBatchCommand = new RelayCommand(new Action<object>(this.NewBatch));
			this.StartBatchCommand = new RelayCommand(new Action<object>(this.StartBatch));
			this.RenameBatchCommand = new RelayCommand(new Action<object>(this.RenameBatch));
			this.DeleteBatchCommand = new RelayCommand(new Action<object>(this.DeleteBatch));
			this.CleanUpCommand = new RelayCommand(new Action<object>(this.CleanUp));
			this.InitializeCommand = new RelayCommand(new Action<object>(this.Initialize));
			imageTransfer.FrameArrived += this.imageTransfer_FrameArrived;
			this.BatchesGuiUpdate();
			if (showNewBatchKeyboard)
			{
				this.NewBatch(null);
			}
			this.RegisterLocalizedText();
		}

		// Token: 0x06000662 RID: 1634 RVA: 0x0002A9C4 File Offset: 0x00028BC4
		private void RegisterLocalizedText()
		{
			this.StrThroughput = new BoundLocalizedText("txtThroughput");
			this.StrReject = new BoundLocalizedText("txtReject");
			this.StrStartTime = new BoundLocalizedText("txtStartTime");
			this.StrEndTime = new BoundLocalizedText("txtEndTime");
		}

		// Token: 0x06000663 RID: 1635 RVA: 0x0002AA18 File Offset: 0x00028C18
		private void DeregisterLocalizedText()
		{
			MainWindow.localizationManager.Deregister(this.StrThroughput);
			MainWindow.localizationManager.Deregister(this.StrReject);
			MainWindow.localizationManager.Deregister(this.StrStartTime);
			MainWindow.localizationManager.Deregister(this.StrEndTime);
		}

		// Token: 0x06000664 RID: 1636 RVA: 0x0002AA6C File Offset: 0x00028C6C
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x06000665 RID: 1637 RVA: 0x0002AA98 File Offset: 0x00028C98
		private void DeleteBatch(object obj)
		{
			bool flag = this.selectedBatch != null;
			if (flag)
			{
				bool flag2 = this.ProductManager.SelectedProduct.CurrentBatchName != this.selectedBatch.BatchID;
				if (flag2)
				{
					bool flag3 = Dialog.Show(LocalizationManager.Localize("txtWarning"), "\"" + this.selectedBatch.BatchID + "\" " + LocalizationManager.Localize("msgGettingErased"), DialogStyle.Warning, DialogButtons.OKCancel) == Dialog.DialogResults.Ok;
					if (flag3)
					{
						this.dbConnection.DeleteBatch(this.ProductManager.SelectedProduct.TableName, this.selectedBatch.BatchID);
						this.dbConnection.DeleteStatistics(this.ProductManager.SelectedProduct.TableName, this.selectedBatch);
						this.BatchesGuiUpdate();
					}
				}
				else
				{
					Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtCurrentBatchDeleteForbidden"), DialogStyle.Error, DialogButtons.OK);
				}
			}
			else
			{
				Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtNoBatchSelected"), DialogStyle.Error, DialogButtons.OK);
			}
		}

		// Token: 0x06000666 RID: 1638 RVA: 0x0002ABAC File Offset: 0x00028DAC
		private void RenameBatch(object obj)
		{
			bool flag = this.selectedBatch != null;
			if (flag)
			{
				this.keyboard = new Keyboard(LocalizationManager.Localize("txtRenameBatch"), this.selectedBatch.BatchID, KeyboardType.Application);
				this.keyboard.TextInputKeyboard += this.keyboard_Rename;
				this.keyboard.ShowDialog();
				this.BatchesGuiUpdate();
			}
			else
			{
				Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtNoBatchSelected"), DialogStyle.Error, DialogButtons.OK);
			}
		}

		// Token: 0x06000667 RID: 1639 RVA: 0x0002AC34 File Offset: 0x00028E34
		private void Rename(string newBatchID)
		{
			bool flag = !this.dbConnection.RenameBatch(this.ProductManager.SelectedProduct.TableName, this.selectedBatch.BatchID, newBatchID);
			if (flag)
			{
				Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtBatchIDExists"), DialogStyle.Error, DialogButtons.OK);
			}
			else
			{
				string b = null;
				this.dbConnection.GetCurrentBatchID(ref b, this.ProductManager.SelectedProduct.TableName);
				bool flag2 = this.ProductManager.SelectedProduct == this.ProductManager.CurrentLoadedProduct && this.selectedBatch.BatchID == b;
				if (flag2)
				{
					this.dbConnection.SetCurrentBatchID(newBatchID, this.ProductManager.SelectedProduct.TableName);
					this.ProductManager.SelectedProduct.CurrentBatchName = newBatchID;
				}
			}
		}

		// Token: 0x06000668 RID: 1640 RVA: 0x0002AD18 File Offset: 0x00028F18
		private void NewBatch(object obj)
		{
			string oldValue = "";
			this.keyboard = new Keyboard(LocalizationManager.Localize("txtNewBatch"), oldValue, KeyboardType.Application);
			this.keyboard.TextInputKeyboard += this.keyboard_TextInputKeyboard2;
			this.keyboard.ShowDialog();
		}

		// Token: 0x06000669 RID: 1641 RVA: 0x0002AD68 File Offset: 0x00028F68
		private void StartBatch(object obj)
		{
			this.ProductManager.BatchHasChanged();
			bool flag = this.selectedBatch != null;
			if (flag)
			{
				bool flag2 = this.selectedBatch.StartTimeD == DateTime.MinValue;
				if (flag2)
				{
					bool flag3 = Dialog.Show(LocalizationManager.Localize("txtWarning"), string.Concat(new string[]
					{
						"\"",
						this.selectedBatch.BatchID,
						"\" ",
						LocalizationManager.Localize("txtWillBeStarted"),
						" ",
						LocalizationManager.Localize("txtContinue")
					}), DialogStyle.Warning, DialogButtons.OKCancel) == Dialog.DialogResults.Ok;
					if (flag3)
					{
						this.StartSelectedBatch();
					}
				}
				else
				{
					bool flag4 = this.selectedBatch.EndTimeD == DateTime.MinValue;
					if (flag4)
					{
						Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtBatchAlreadyStarted"), DialogStyle.Error, DialogButtons.OK);
					}
					else
					{
						bool flag5 = Dialog.Show(LocalizationManager.Localize("txtWarning"), string.Concat(new string[]
						{
							"\"",
							this.selectedBatch.BatchID,
							"\" ",
							LocalizationManager.Localize("txtBatchAlreadyEnded"),
							" ",
							LocalizationManager.Localize("txtResume")
						}), DialogStyle.Warning, DialogButtons.OKCancel) == Dialog.DialogResults.Ok;
						if (flag5)
						{
							this.StartSelectedBatch();
						}
					}
				}
			}
			else
			{
				Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtNoBatchSelected"), DialogStyle.Error, DialogButtons.OK);
			}
		}

		// Token: 0x0600066A RID: 1642 RVA: 0x0002AEE8 File Offset: 0x000290E8
		private void StartSelectedBatch()
		{
			string text = null;
			this.dbConnection.GetCurrentBatchID(ref text, this.ProductManager.SelectedProduct.TableName);
			bool flag = text != null && text.Length > 0;
			if (flag)
			{
				this.dbConnection.EndBatch(text, this.ProductManager.SelectedProduct.TableName);
			}
			this.dbConnection.StartBatch(this.selectedBatch.BatchID, this.ProductManager.SelectedProduct.TableName, text);
			this.ProductManager.SelectedProduct.CurrentBatchName = this.selectedBatch.BatchID;
			this.BatchesGuiUpdate();
		}

		// Token: 0x0600066B RID: 1643 RVA: 0x0002AF94 File Offset: 0x00029194
		public bool CreateBatch(string newBatchID)
		{
			bool result = false;
			bool flag = !this.dbConnection.CreateNewBatch(newBatchID, this.ProductManager.SelectedProduct.TableName);
			if (flag)
			{
				Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtBatchIDExists"), DialogStyle.Error, DialogButtons.OK);
				bool flag2 = this.keyboard != null;
				if (flag2)
				{
					this.keyboard.ShouldClose = false;
				}
			}
			else
			{
				result = true;
				this.BatchesGuiUpdate();
				bool flag3 = string.IsNullOrEmpty(this.ProductManager.CurrentLoadedProduct.CurrentBatchName) && this.UIBatches.Count > 0;
				if (flag3)
				{
					this.SelectedBatch = this.UIBatches[0];
				}
				else
				{
					bool flag4 = this.UIBatches.Count > 1;
					if (flag4)
					{
						this.SelectedBatch = this.UIBatches[1];
					}
				}
				bool flag5 = this.ProductManager.CurrentLoadedProduct == this.ProductManager.SelectedProduct;
				if (flag5)
				{
					this.StartBatch(null);
				}
			}
			return result;
		}

		// Token: 0x0600066C RID: 1644 RVA: 0x0002B0A8 File Offset: 0x000292A8
		public void BatchesGuiUpdate()
		{
			this.batches.Clear();
			this.dbConnection.GetAllBatches(this.batches, this.ProductManager.SelectedProduct.TableName);
			this.UIBatches.Clear();
			this.selectedBatch = null;
			List<Batch> list = new List<Batch>();
			for (int i = 0; i < this.batches.Count; i++)
			{
				list.Add(this.batches[i]);
			}
			list.Sort(new BatchesComparer());
			for (int j = 0; j < this.batches.Count; j++)
			{
				this.UIBatches.Add(list[j]);
			}
			bool flag = this.UIBatches.Count > 0;
			if (flag)
			{
				this.SelectedBatch = this.UIBatches[0];
			}
		}

		// Token: 0x0600066D RID: 1645 RVA: 0x0002B190 File Offset: 0x00029390
		private void keyboard_TextInputKeyboard2(object sender, KeyboardInputEventArgs e)
		{
			bool flag = e.Text == null || e.Text.Length == 0;
			if (flag)
			{
				Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtInsertID"), DialogStyle.Error);
				bool flag2 = this.keyboard != null;
				if (flag2)
				{
					this.keyboard.ShouldClose = false;
				}
			}
			else
			{
				bool flag3 = this.CreateBatch(e.Text);
				if (flag3)
				{
					this.keyboard.TextInputKeyboard -= this.keyboard_TextInputKeyboard2;
				}
			}
		}

		// Token: 0x0600066E RID: 1646 RVA: 0x0002B21E File Offset: 0x0002941E
		private void keyboard_Rename(object sender, KeyboardInputEventArgs e)
		{
			this.keyboard.TextInputKeyboard -= this.keyboard_Rename;
			this.Rename(e.Text);
		}

		// Token: 0x0600066F RID: 1647 RVA: 0x0002B248 File Offset: 0x00029448
		private void Initialize(object obj)
		{
			this.wdSub = (Window.GetWindow(obj as DependencyObject) as Subwindow);
			bool flag = this.wdSub != null;
			if (flag)
			{
				this.wdSub.BackButtonClicked += this.wdSub_BackButtonClicked;
			}
		}

		// Token: 0x06000670 RID: 1648 RVA: 0x0002B294 File Offset: 0x00029494
		private void wdSub_BackButtonClicked()
		{
			bool flag = this.wdSub != null;
			if (flag)
			{
				bool flag2 = this.ProductManager.CurrentLoadedProduct == this.ProductManager.SelectedProduct && (this.ProductManager.SelectedProduct.CurrentBatchName == null || this.ProductManager.SelectedProduct.CurrentBatchName.Length == 0);
				if (flag2)
				{
					bool flag3 = Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtNoBatchSelected") + " " + LocalizationManager.Localize("txtContinue"), DialogStyle.Warning, DialogButtons.OKCancel) == Dialog.DialogResults.Cancel;
					if (flag3)
					{
						this.wdSub.IgnoreNextBack = true;
					}
					else
					{
						this.wdSub.BackButtonClicked -= this.wdSub_BackButtonClicked;
					}
				}
				else
				{
					this.wdSub.BackButtonClicked -= this.wdSub_BackButtonClicked;
				}
			}
		}

		// Token: 0x06000671 RID: 1649 RVA: 0x0002B37B File Offset: 0x0002957B
		private void CleanUp(object obj)
		{
			this.imageTransfer.FrameArrived -= this.imageTransfer_FrameArrived;
			this.DeregisterLocalizedText();
		}

		// Token: 0x06000672 RID: 1650 RVA: 0x0002B39D File Offset: 0x0002959D
		private void imageTransfer_FrameArrived(object sender, ReceiveEventArgs e)
		{
			Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(delegate()
			{
				Batch batch = new Batch();
				this.dbConnection.GetCurrentBatch(ref batch);
				bool flag = batch != null;
				if (flag)
				{
					for (int i = 0; i < this.UIBatches.Count; i++)
					{
						bool flag2 = this.UIBatches[i].BatchID == batch.BatchID;
						if (flag2)
						{
							this.UIBatches[i].SetMembers(batch);
							break;
						}
					}
				}
			}));
		}

		// Token: 0x0400047F RID: 1151
		private Keyboard keyboard;

		// Token: 0x04000480 RID: 1152
		private DBConnection dbConnection;

		// Token: 0x04000481 RID: 1153
		private Batch selectedBatch;

		// Token: 0x04000482 RID: 1154
		private List<Batch> batches = new List<Batch>();

		// Token: 0x04000483 RID: 1155
		private ImageDataTransfer imageTransfer;

		// Token: 0x04000484 RID: 1156
		private Subwindow wdSub;
	}
}
