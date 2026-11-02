using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Threading;
using Common;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x02000120 RID: 288
	public class LogbookViewModel : INotifyPropertyChanged
	{
		// Token: 0x1400008B RID: 139
		// (add) Token: 0x06001BB2 RID: 7090 RVA: 0x000AB5A4 File Offset: 0x000A97A4
		// (remove) Token: 0x06001BB3 RID: 7091 RVA: 0x000AB5DC File Offset: 0x000A97DC
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x17000828 RID: 2088
		// (get) Token: 0x06001BB4 RID: 7092 RVA: 0x000AB611 File Offset: 0x000A9811
		// (set) Token: 0x06001BB5 RID: 7093 RVA: 0x000AB619 File Offset: 0x000A9819
		public Logbook Logbook { get; set; }

		// Token: 0x17000829 RID: 2089
		// (get) Token: 0x06001BB6 RID: 7094 RVA: 0x000AB624 File Offset: 0x000A9824
		// (set) Token: 0x06001BB7 RID: 7095 RVA: 0x000AB63C File Offset: 0x000A983C
		public SSTLogbookEntry SelectedEntry
		{
			get
			{
				return this.selectedEntry;
			}
			set
			{
				this.selectedEntry = value;
				this.OnPropertyChanged("SelectedEntry");
			}
		}

		// Token: 0x1700082A RID: 2090
		// (get) Token: 0x06001BB8 RID: 7096 RVA: 0x000AB652 File Offset: 0x000A9852
		// (set) Token: 0x06001BB9 RID: 7097 RVA: 0x000AB65A File Offset: 0x000A985A
		public RelayCommand FirstItemCommand { get; set; }

		// Token: 0x1700082B RID: 2091
		// (get) Token: 0x06001BBA RID: 7098 RVA: 0x000AB663 File Offset: 0x000A9863
		// (set) Token: 0x06001BBB RID: 7099 RVA: 0x000AB66B File Offset: 0x000A986B
		public RelayCommand LastItemCommand { get; set; }

		// Token: 0x1700082C RID: 2092
		// (get) Token: 0x06001BBC RID: 7100 RVA: 0x000AB674 File Offset: 0x000A9874
		// (set) Token: 0x06001BBD RID: 7101 RVA: 0x000AB67C File Offset: 0x000A987C
		public RelayCommand PreviousItemCommand { get; set; }

		// Token: 0x1700082D RID: 2093
		// (get) Token: 0x06001BBE RID: 7102 RVA: 0x000AB685 File Offset: 0x000A9885
		// (set) Token: 0x06001BBF RID: 7103 RVA: 0x000AB68D File Offset: 0x000A988D
		public RelayCommand NextItemCommand { get; set; }

		// Token: 0x1700082E RID: 2094
		// (get) Token: 0x06001BC0 RID: 7104 RVA: 0x000AB696 File Offset: 0x000A9896
		// (set) Token: 0x06001BC1 RID: 7105 RVA: 0x000AB69E File Offset: 0x000A989E
		public RelayCommand CleanUpCommand { get; set; }

		// Token: 0x06001BC2 RID: 7106 RVA: 0x000AB6A8 File Offset: 0x000A98A8
		public LogbookViewModel(Logbook logbook, AccessRightsManager accessManager, SetFooterMainWindow setFooterMainWindow)
		{
			this.Logbook = logbook;
			this.accessManager = accessManager;
			this.setFooterMainWindow = setFooterMainWindow;
			this.Logbook.ClearEntries();
			this.view = (ListCollectionView)CollectionViewSource.GetDefaultView(this.Logbook.ShownLogbookEntries);
			this.view.SortDescriptions.Add(new SortDescription("ID", ListSortDirection.Descending));
			this.view.CurrentChanged += delegate(object <p0>, EventArgs <p1>)
			{
				this.SelectedEntry = (SSTLogbookEntry)this.view.CurrentItem;
			};
			this.FirstItemCommand = new RelayCommand(new Action<object>(this.SelectFirstItem));
			this.LastItemCommand = new RelayCommand(new Action<object>(this.SelectLastItem));
			this.PreviousItemCommand = new RelayCommand(new Action<object>(this.SelectPreviousItem));
			this.NextItemCommand = new RelayCommand(new Action<object>(this.SelectNextItem));
			this.CleanUpCommand = new RelayCommand(new Action<object>(this.CleanUp));
			new Thread(new ThreadStart(this.LoadLogbook))
			{
				Name = "LoadLogbook"
			}.Start();
		}

		// Token: 0x06001BC3 RID: 7107 RVA: 0x000AB7CC File Offset: 0x000A99CC
		private void LoadLogbook()
		{
			try
			{
				this.Logbook.CreateLogbook();
			}
			catch (Exception ex)
			{
				Exception ex3;
				Exception ex2 = ex3;
				Exception ex = ex2;
				bool flag = Application.Current != null;
				if (flag)
				{
					Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate()
					{
						Dialog.Show("Error", ex.Message, DialogStyle.Error, DialogButtons.OK);
					}));
				}
			}
		}

		// Token: 0x06001BC4 RID: 7108 RVA: 0x000AB838 File Offset: 0x000A9A38
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x06001BC5 RID: 7109 RVA: 0x000AB868 File Offset: 0x000A9A68
		public void SelectFirstItem(object obj)
		{
			bool flag = this.Logbook.ShownLogbookEntries.Count > 0;
			if (flag)
			{
				this.view.MoveCurrentToFirst();
			}
		}

		// Token: 0x06001BC6 RID: 7110 RVA: 0x000AB89C File Offset: 0x000A9A9C
		public void SelectLastItem(object obj)
		{
			bool flag = this.Logbook.ShownLogbookEntries.Count > 0;
			if (flag)
			{
				this.view.MoveCurrentToLast();
			}
		}

		// Token: 0x06001BC7 RID: 7111 RVA: 0x000AB8D0 File Offset: 0x000A9AD0
		public void SelectPreviousItem(object obj)
		{
			bool flag = this.Logbook.ShownLogbookEntries.Count > 0;
			if (flag)
			{
				bool flag2 = this.view.CurrentPosition < 0;
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

		// Token: 0x06001BC8 RID: 7112 RVA: 0x000AB944 File Offset: 0x000A9B44
		public void SelectNextItem(object obj)
		{
			bool flag = this.Logbook.ShownLogbookEntries.Count > 0;
			if (flag)
			{
				bool flag2 = this.view.CurrentPosition != this.Logbook.ShownLogbookEntries.Count - 1;
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

		// Token: 0x06001BC9 RID: 7113 RVA: 0x000AB9AB File Offset: 0x000A9BAB
		public void DeleteLogbook(object sender, RoutedEventArgs e)
		{
			this.Logbook.DeleteLogbook();
		}

		// Token: 0x06001BCA RID: 7114 RVA: 0x000AB9BC File Offset: 0x000A9BBC
		private void CleanUp(object obj)
		{
			this.Logbook.StopLogbookCreation();
			this.accessManager.DeregisterControl(this.btnDelete);
			this.accessManager.DeregisterControl(this.btnSave);
			this.btnDelete = null;
			this.btnSave = null;
			this.btnFilterContaminations = null;
			this.btnFilterErrors = null;
			this.btnFilterWarnings = null;
			this.btnFilterInfos = null;
			this.setFooterMainWindow.UnSetFooterDeactivation();
		}

		// Token: 0x06001BCB RID: 7115 RVA: 0x000ABA30 File Offset: 0x000A9C30
		public void SetButtons(Button btnDelete, Button btnSave, Button btnFilterContaminations, Button btnFilterErrors, Button btnFilterWarnings, Button btnFilterInfos)
		{
			this.btnDelete = btnDelete;
			this.btnSave = btnSave;
			this.btnFilterContaminations = btnFilterContaminations;
			this.btnFilterErrors = btnFilterErrors;
			this.btnFilterWarnings = btnFilterWarnings;
			this.btnFilterInfos = btnFilterInfos;
		}

		// Token: 0x06001BCC RID: 7116 RVA: 0x000ABA60 File Offset: 0x000A9C60
		public void SaveLogbook(object sender, EventArgs e)
		{
			bool flag = this.Logbook.SaveLogbook();
			if (flag)
			{
				Dialog.Show(LocalizationManager.Localize("txtSuccess"), LocalizationManager.Localize("lbLogbookSaved"), DialogStyle.Ok, DialogButtons.None, true);
			}
			else
			{
				Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtNoUSB"), DialogStyle.Error);
			}
		}

		// Token: 0x06001BCD RID: 7117 RVA: 0x000ABAB8 File Offset: 0x000A9CB8
		public void FilterContaminations(object sender, RoutedEventArgs e)
		{
			bool flag = this.Logbook.FilterContaminations();
			if (flag)
			{
				Image image = ImageConverter.ToImage("/img/reject_orange.png");
				image.Height = 40.0;
				image.Width = 40.0;
				image.Stretch = Stretch.Uniform;
				this.btnFilterContaminations.Content = image;
			}
			else
			{
				Image image2 = ImageConverter.ToImage("/img/delete.png");
				image2.Height = 40.0;
				image2.Width = 40.0;
				image2.Stretch = Stretch.Uniform;
				this.btnFilterContaminations.Content = image2;
			}
		}

		// Token: 0x06001BCE RID: 7118 RVA: 0x000ABB5C File Offset: 0x000A9D5C
		public void FilterErrors(object sender, RoutedEventArgs e)
		{
			bool flag = this.Logbook.FilterErrors();
			if (flag)
			{
				Image image = ImageConverter.ToImage("/img/errorLog.png");
				image.Height = 40.0;
				image.Width = 40.0;
				image.Stretch = Stretch.Uniform;
				this.btnFilterErrors.Content = image;
			}
			else
			{
				Image image2 = ImageConverter.ToImage("/img/errorLog_Disabled.png");
				image2.Height = 40.0;
				image2.Width = 40.0;
				image2.Stretch = Stretch.Uniform;
				this.btnFilterErrors.Content = image2;
			}
		}

		// Token: 0x06001BCF RID: 7119 RVA: 0x000ABC00 File Offset: 0x000A9E00
		public void FilterWarnings(object sender, RoutedEventArgs e)
		{
			bool flag = this.Logbook.FilterWarnings();
			if (flag)
			{
				Image image = ImageConverter.ToImage("/img/warning_yellow.png");
				image.Height = 40.0;
				image.Width = 40.0;
				image.Stretch = Stretch.Uniform;
				this.btnFilterWarnings.Content = image;
			}
			else
			{
				Image image2 = ImageConverter.ToImage("/img/warning_disabled.png");
				image2.Height = 40.0;
				image2.Width = 40.0;
				image2.Stretch = Stretch.Uniform;
				this.btnFilterWarnings.Content = image2;
			}
		}

		// Token: 0x06001BD0 RID: 7120 RVA: 0x000ABCA4 File Offset: 0x000A9EA4
		public void FilterInfos(object sender, RoutedEventArgs e)
		{
			bool flag = this.Logbook.FilterInfos();
			if (flag)
			{
				Image image = ImageConverter.ToImage("/img/infoLog.png");
				image.Height = 40.0;
				image.Width = 40.0;
				image.Stretch = Stretch.Uniform;
				this.btnFilterInfos.Content = image;
			}
			else
			{
				Image image2 = ImageConverter.ToImage("/img/info.png");
				image2.Height = 40.0;
				image2.Width = 40.0;
				image2.Stretch = Stretch.Uniform;
				this.btnFilterInfos.Content = image2;
			}
		}

		// Token: 0x04001274 RID: 4724
		private SSTLogbookEntry selectedEntry;

		// Token: 0x04001275 RID: 4725
		private ListCollectionView view;

		// Token: 0x04001276 RID: 4726
		private AccessRightsManager accessManager;

		// Token: 0x04001277 RID: 4727
		private Button btnDelete;

		// Token: 0x04001278 RID: 4728
		private Button btnSave;

		// Token: 0x04001279 RID: 4729
		private Button btnFilterContaminations;

		// Token: 0x0400127A RID: 4730
		private Button btnFilterErrors;

		// Token: 0x0400127B RID: 4731
		private Button btnFilterWarnings;

		// Token: 0x0400127C RID: 4732
		private Button btnFilterInfos;

		// Token: 0x04001284 RID: 4740
		private SetFooterMainWindow setFooterMainWindow;
	}
}
