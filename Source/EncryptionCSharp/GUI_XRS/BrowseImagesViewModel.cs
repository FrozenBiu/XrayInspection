using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Data;
using System.Windows.Threading;
using Common;
using DatabaseConnection;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x02000043 RID: 67
	public class BrowseImagesViewModel : INotifyPropertyChanged
	{
		// Token: 0x14000028 RID: 40
		// (add) Token: 0x060006C1 RID: 1729 RVA: 0x0002C724 File Offset: 0x0002A924
		// (remove) Token: 0x060006C2 RID: 1730 RVA: 0x0002C75C File Offset: 0x0002A95C
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x170001FD RID: 509
		// (get) Token: 0x060006C3 RID: 1731 RVA: 0x0002C791 File Offset: 0x0002A991
		// (set) Token: 0x060006C4 RID: 1732 RVA: 0x0002C799 File Offset: 0x0002A999
		public ObservableCollection<BulkHistoryItem> HistoryItems { get; set; }

		// Token: 0x170001FE RID: 510
		// (get) Token: 0x060006C5 RID: 1733 RVA: 0x0002C7A4 File Offset: 0x0002A9A4
		// (set) Token: 0x060006C6 RID: 1734 RVA: 0x0002C7BC File Offset: 0x0002A9BC
		public BulkHistoryItem SelectedItem
		{
			get
			{
				return this.selectedItem;
			}
			set
			{
				bool flag = this.selectedItem != value;
				if (flag)
				{
					this.selectedItem = value;
					this.OnPropertyChanged("SelectedItem");
				}
			}
		}

		// Token: 0x170001FF RID: 511
		// (get) Token: 0x060006C7 RID: 1735 RVA: 0x0002C7EF File Offset: 0x0002A9EF
		// (set) Token: 0x060006C8 RID: 1736 RVA: 0x0002C7F7 File Offset: 0x0002A9F7
		public RelayCommand NextItemCommand { get; set; }

		// Token: 0x17000200 RID: 512
		// (get) Token: 0x060006C9 RID: 1737 RVA: 0x0002C800 File Offset: 0x0002AA00
		// (set) Token: 0x060006CA RID: 1738 RVA: 0x0002C808 File Offset: 0x0002AA08
		public RelayCommand PreviousItemCommand { get; set; }

		// Token: 0x17000201 RID: 513
		// (get) Token: 0x060006CB RID: 1739 RVA: 0x0002C811 File Offset: 0x0002AA11
		// (set) Token: 0x060006CC RID: 1740 RVA: 0x0002C819 File Offset: 0x0002AA19
		public RelayCommand FirstItemCommand { get; set; }

		// Token: 0x17000202 RID: 514
		// (get) Token: 0x060006CD RID: 1741 RVA: 0x0002C822 File Offset: 0x0002AA22
		// (set) Token: 0x060006CE RID: 1742 RVA: 0x0002C82A File Offset: 0x0002AA2A
		public RelayCommand LastItemCommand { get; set; }

		// Token: 0x17000203 RID: 515
		// (get) Token: 0x060006CF RID: 1743 RVA: 0x0002C833 File Offset: 0x0002AA33
		// (set) Token: 0x060006D0 RID: 1744 RVA: 0x0002C83B File Offset: 0x0002AA3B
		public RelayCommand CloseCommand { get; set; }

		// Token: 0x060006D1 RID: 1745 RVA: 0x0002C844 File Offset: 0x0002AA44
		public BrowseImagesViewModel(DBConnection dbConnection, ParameterHandling parHandling, ImageDataTransfer imageTransfer, MachineState machineState, ProductManager productManager, StoreImageData imageArchiver)
		{
			this.parHandling = parHandling;
			this.dbConnection = dbConnection;
			this.imageTransfer = imageTransfer;
			this.machineState = machineState;
			this.productManager = productManager;
			this.imageArchiver = imageArchiver;
			this.HistoryItems = new ObservableCollection<BulkHistoryItem>();
			this.StartCreateItems();
			this.view = (ListCollectionView)CollectionViewSource.GetDefaultView(this.HistoryItems);
			this.view.CurrentChanged += delegate(object <p0>, EventArgs <p1>)
			{
				this.SelectedItem = (BulkHistoryItem)this.view.CurrentItem;
			};
			this.NextItemCommand = new RelayCommand(new Action<object>(this.SelectNextItem));
			this.PreviousItemCommand = new RelayCommand(new Action<object>(this.SelectPreviousItem));
			this.FirstItemCommand = new RelayCommand(new Action<object>(this.SelectFirstItem));
			this.LastItemCommand = new RelayCommand(new Action<object>(this.SelectLastItem));
			this.CloseCommand = new RelayCommand(new Action<object>(this.CleanUp));
		}

		// Token: 0x060006D2 RID: 1746 RVA: 0x0002C954 File Offset: 0x0002AB54
		private void StartCreateItems()
		{
			this.backgroundWorker = new BackgroundWorker();
			this.backgroundWorker.DoWork += this.backgroundWorker_DoWork;
			this.backgroundWorker.RunWorkerCompleted += this.backgroundWorker_RunWorkerCompleted;
			this.backgroundWorker.WorkerReportsProgress = true;
			this.backgroundWorker.RunWorkerAsync();
		}

		// Token: 0x060006D3 RID: 1747 RVA: 0x0002C9B6 File Offset: 0x0002ABB6
		private void imageTransfer_FrameArrived(object sender, ReceiveEventArgs e)
		{
			this.semaphore.Release();
		}

		// Token: 0x060006D4 RID: 1748 RVA: 0x0002C9C8 File Offset: 0x0002ABC8
		public void Train()
		{
			bool flag = (from item in this.HistoryItems
			where item.Selected
			select item).Count<BulkHistoryItem>() > 0;
			if (flag)
			{
				new Thread(new ThreadStart(this.TrainItems))
				{
					Name = "thTrainItems"
				}.Start();
			}
			else
			{
				Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtNoLearnImagesChosen") + "!", DialogStyle.Error, DialogButtons.OK);
			}
		}

		// Token: 0x060006D5 RID: 1749 RVA: 0x0002CA60 File Offset: 0x0002AC60
		private void TrainItems()
		{
			BrowseImagesViewModel.<>c__DisplayClass45_0 CS$<>8__locals1 = new BrowseImagesViewModel.<>c__DisplayClass45_0();
			CS$<>8__locals1.prog = null;
			bool flag = Application.Current != null;
			if (flag)
			{
				Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate()
				{
					CS$<>8__locals1.prog = new Progress(LocalizationManager.Localize("lbAutolearnStarted") + ".", false);
					CS$<>8__locals1.prog.Show();
				}));
			}
			CS$<>8__locals1.numSelected = (from item in this.HistoryItems
			where item.Selected
			select item).Count<BulkHistoryItem>();
			Thread.Sleep(500);
			Filter.WriteToImageProcessingFilters(this.parHandling, Autotrain.strAutolearn, CS$<>8__locals1.numSelected.ToString(), 0);
			this.imageTransfer.FrameArrived += this.imageTransfer_FrameArrived;
			int i;
			int j;
			for (i = 0; i < this.HistoryItems.Count; i = j + 1)
			{
				bool selected = this.HistoryItems[i].Selected;
				if (selected)
				{
					this.imageTransfer.SendImage(this.HistoryItems[i].XrsImage);
					bool flag2 = this.semaphore.WaitOne(5000);
					if (!flag2)
					{
						bool flag3 = Application.Current != null;
						if (flag3)
						{
							Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate()
							{
								Dialog.Show(LocalizationManager.Localize("txtError"), "Comm-Error", DialogStyle.Error, DialogButtons.OK);
							}));
						}
						break;
					}
					bool flag4 = Application.Current != null;
					if (flag4)
					{
						Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate()
						{
							bool flag6 = CS$<>8__locals1.prog != null;
							if (flag6)
							{
								CS$<>8__locals1.prog.progBar.Value = (double)((i + 1) * 100 / CS$<>8__locals1.numSelected);
							}
						}));
					}
					Thread.Sleep(200);
				}
				j = i;
			}
			this.imageTransfer.FrameArrived -= this.imageTransfer_FrameArrived;
			Filter.UpdateImageProcessingFilterPaths(this.dbConnection, this.parHandling, 0, this.productManager, true);
			bool flag5 = Application.Current != null;
			if (flag5)
			{
				Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate()
				{
					CS$<>8__locals1.prog.Close();
					Dialog.Show(LocalizationManager.Localize("txtSuccess"), LocalizationManager.Localize("txtAfterAutotrain"), DialogStyle.Ok, DialogButtons.None, true);
				}));
			}
		}

		// Token: 0x060006D6 RID: 1750 RVA: 0x0002CC9C File Offset: 0x0002AE9C
		public void SelectNextItem(object obj)
		{
			bool flag = this.HistoryItems.Count > 0;
			if (flag)
			{
				bool flag2 = this.view.CurrentPosition != this.HistoryItems.Count - 1 && this.view.CurrentPosition != -1;
				if (flag2)
				{
					this.view.MoveCurrentToNext();
				}
				else
				{
					this.view.MoveCurrentToFirst();
				}
			}
		}

		// Token: 0x060006D7 RID: 1751 RVA: 0x0002CD0C File Offset: 0x0002AF0C
		public void SelectPreviousItem(object obj)
		{
			bool flag = this.HistoryItems.Count > 0;
			if (flag)
			{
				bool flag2 = this.view.CurrentPosition == -1;
				if (flag2)
				{
					this.view.MoveCurrentToFirst();
				}
				else
				{
					bool flag3 = this.view.CurrentPosition != 0;
					if (flag3)
					{
						this.view.MoveCurrentToPrevious();
					}
					else
					{
						this.view.MoveCurrentToLast();
					}
				}
			}
		}

		// Token: 0x060006D8 RID: 1752 RVA: 0x0002CD7C File Offset: 0x0002AF7C
		public void SelectFirstItem(object obj)
		{
			bool flag = this.HistoryItems.Count > 0;
			if (flag)
			{
				this.view.MoveCurrentToFirst();
			}
		}

		// Token: 0x060006D9 RID: 1753 RVA: 0x0002CDA8 File Offset: 0x0002AFA8
		public void SelectLastItem(object obj)
		{
			bool flag = this.HistoryItems.Count > 0;
			if (flag)
			{
				this.view.MoveCurrentToLast();
			}
		}

		// Token: 0x060006DA RID: 1754 RVA: 0x0002CDD4 File Offset: 0x0002AFD4
		private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
		{
			string str = "";
			try
			{
				Thread.Sleep(100);
				this.dbConnection.GetCurrentProductTableName(ref str);
				string path = StoreImageData.archiveRoot + "\\" + str;
				bool flag = Directory.Exists(path);
				if (flag)
				{
					List<string> historyFiles = new List<string>();
					historyFiles.AddRange(Directory.GetFiles(path));
					historyFiles.Remove((from name in historyFiles
					where name.EndsWith(LiveModeViewModel.liveImageName)
					select name).FirstOrDefault<string>());
					historyFiles.Sort(new NamesComparerInverse());
					bool flag2 = historyFiles.Count > StoreImageData.defaultMaxHistoryFiles;
					if (flag2)
					{
						historyFiles.RemoveRange(StoreImageData.defaultMaxHistoryFiles, historyFiles.Count - StoreImageData.defaultMaxHistoryFiles);
					}
					int j;
					int i;
					for (i = 0; i < historyFiles.Count; i = j + 1)
					{
						bool flag3 = this.shouldClose;
						if (flag3)
						{
							break;
						}
						bool flag4 = historyFiles[i].EndsWith(XRSImage.fileEnding);
						if (flag4)
						{
							bool flag5 = File.Exists(historyFiles[i]);
							if (flag5)
							{
								Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate()
								{
									XRSImage image = this.imageArchiver.GetImage(historyFiles[i]);
									BulkHistoryItem bulkHistoryItem = new BulkHistoryItem(historyFiles[i], this.imageArchiver);
									bulkHistoryItem.BitmapThumbnail = image.GetCombinedImage(false);
									bulkHistoryItem.Timestamp = this.CreateTimestamp(Path.GetFileNameWithoutExtension(historyFiles[i]));
									bulkHistoryItem.XrsImage = image;
									this.HistoryItems.Add(bulkHistoryItem);
								}));
							}
						}
						bool flag6 = i < 5;
						if (flag6)
						{
							Thread.Sleep(100);
						}
						else
						{
							Thread.Sleep(250);
						}
						j = i;
					}
				}
			}
			catch (Exception ex)
			{
				Exception ex3;
				Exception ex2 = ex3;
				Exception ex = ex2;
				bool flag7 = Application.Current != null;
				if (flag7)
				{
					Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate()
					{
						Dialog.Show("Error", ex.Message);
					}));
				}
			}
		}

		// Token: 0x060006DB RID: 1755 RVA: 0x0002D030 File Offset: 0x0002B230
		private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			this.backgroundWorker.DoWork -= this.backgroundWorker_DoWork;
			this.backgroundWorker.RunWorkerCompleted -= this.backgroundWorker_RunWorkerCompleted;
		}

		// Token: 0x060006DC RID: 1756 RVA: 0x0002D064 File Offset: 0x0002B264
		public void AbortCreation()
		{
			this.shouldClose = true;
			this.backgroundWorker.DoWork -= this.backgroundWorker_DoWork;
			this.backgroundWorker.RunWorkerCompleted -= this.backgroundWorker_RunWorkerCompleted;
			this.semaphore.Close();
		}

		// Token: 0x060006DD RID: 1757 RVA: 0x0002D0B8 File Offset: 0x0002B2B8
		private string CreateTimestamp(string fileTimestamp)
		{
			StringBuilder stringBuilder = new StringBuilder(fileTimestamp);
			stringBuilder.Remove(0, 2);
			stringBuilder = stringBuilder.Replace('_', '-');
			bool flag = stringBuilder.Length > 17;
			if (flag)
			{
				stringBuilder[8] = '_';
				stringBuilder[11] = ':';
				stringBuilder[14] = ':';
				stringBuilder[17] = ',';
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060006DE RID: 1758 RVA: 0x0002D128 File Offset: 0x0002B328
		private void CleanUp(object obj)
		{
			this.AbortCreation();
			foreach (BulkHistoryItem bulkHistoryItem in this.HistoryItems)
			{
				bulkHistoryItem.XrsImage = new XRSImage();
				bulkHistoryItem.BitmapThumbnail = null;
			}
			this.HistoryItems.Clear();
		}

		// Token: 0x060006DF RID: 1759 RVA: 0x0002D19C File Offset: 0x0002B39C
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x060006E0 RID: 1760 RVA: 0x0002D1CC File Offset: 0x0002B3CC
		public void DeleteSelectedItem()
		{
			bool flag = this.SelectedItem != null;
			if (flag)
			{
				bool flag2 = Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtDeleteImage") + "?", DialogStyle.Warning, DialogButtons.OKCancel) == Dialog.DialogResults.Ok;
				if (flag2)
				{
					bool flag3 = this.SelectedItem.FileName != null;
					if (flag3)
					{
						File.Delete(this.SelectedItem.FileName);
					}
					this.HistoryItems.Remove(this.SelectedItem);
				}
			}
			else
			{
				Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtNoImageSelected") + "!", DialogStyle.Error, DialogButtons.OK);
			}
		}

		// Token: 0x060006E1 RID: 1761 RVA: 0x0002D278 File Offset: 0x0002B478
		public void ShowSelectedImage()
		{
			bool flag = this.SelectedItem != null;
			if (flag)
			{
				CtrlShowImage ctrlShowImage = new CtrlShowImage(this.SelectedItem.XrsImage, this.imageArchiver, this.SelectedItem.Timestamp, this.dbConnection);
				Subwindow.Show(ctrlShowImage, SubwindowButtons.Back, ctrlShowImage.Buttons);
			}
			else
			{
				Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtNoImageSelected") + "!", DialogStyle.Error, DialogButtons.OK);
			}
		}

		// Token: 0x060006E2 RID: 1762 RVA: 0x0002D2F8 File Offset: 0x0002B4F8
		public void DeleteAllImages()
		{
			string str = "";
			bool flag = this.HistoryItems.Count > 0 && Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtDeleteImages"), DialogStyle.Warning, DialogButtons.OKCancel) == Dialog.DialogResults.Ok;
			if (flag)
			{
				this.dbConnection.GetCurrentProductTableName(ref str);
				string path = StoreImageData.archiveRoot + "\\" + str;
				bool flag2 = Directory.Exists(path);
				if (flag2)
				{
					Directory.Delete(path, true);
					this.HistoryItems.Clear();
				}
			}
		}

		// Token: 0x060006E3 RID: 1763 RVA: 0x0002D388 File Offset: 0x0002B588
		public void SaveToUSB()
		{
			bool flag = this.SelectedItem != null;
			if (flag)
			{
				USBSave usbsave = new USBSave();
				string usbdriveLetter = "";
				bool saveEdgePixelInPNG = false;
				this.dbConnection.GetUsbDriveLetter(ref usbdriveLetter);
				this.dbConnection.GetSaveEdgePixelInPNG(ref saveEdgePixelInPNG);
				usbsave.SaveToUSB(this.SelectedItem.XrsImage, usbdriveLetter, saveEdgePixelInPNG, this.SelectedItem.Timestamp);
			}
		}

		// Token: 0x040004B8 RID: 1208
		private ListCollectionView view;

		// Token: 0x040004B9 RID: 1209
		private BulkHistoryItem selectedItem;

		// Token: 0x040004BA RID: 1210
		private BackgroundWorker backgroundWorker;

		// Token: 0x040004BB RID: 1211
		private ParameterHandling parHandling;

		// Token: 0x040004BC RID: 1212
		private ImageDataTransfer imageTransfer;

		// Token: 0x040004BD RID: 1213
		private DBConnection dbConnection;

		// Token: 0x040004BE RID: 1214
		private MachineState machineState;

		// Token: 0x040004BF RID: 1215
		private ProductManager productManager;

		// Token: 0x040004C0 RID: 1216
		private bool shouldClose = false;

		// Token: 0x040004C1 RID: 1217
		private Semaphore semaphore = new Semaphore(0, 1);

		// Token: 0x040004C2 RID: 1218
		private StoreImageData imageArchiver;
	}
}
