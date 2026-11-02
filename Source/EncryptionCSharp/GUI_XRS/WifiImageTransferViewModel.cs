using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using Common;
using DatabaseConnection;

namespace GUI_XRS
{
	// Token: 0x0200016D RID: 365
	public class WifiImageTransferViewModel : INotifyPropertyChanged
	{
		// Token: 0x170009AB RID: 2475
		// (get) Token: 0x0600217F RID: 8575 RVA: 0x000D9351 File Offset: 0x000D7551
		// (set) Token: 0x06002180 RID: 8576 RVA: 0x000D9359 File Offset: 0x000D7559
		public RelayCommand ChangeMaxImagePerFolderCommand { get; set; }

		// Token: 0x170009AC RID: 2476
		// (get) Token: 0x06002181 RID: 8577 RVA: 0x000D9362 File Offset: 0x000D7562
		// (set) Token: 0x06002182 RID: 8578 RVA: 0x000D936A File Offset: 0x000D756A
		public RelayCommand ChangeStoragePathCommand { get; set; }

		// Token: 0x170009AD RID: 2477
		// (get) Token: 0x06002183 RID: 8579 RVA: 0x000D9373 File Offset: 0x000D7573
		// (set) Token: 0x06002184 RID: 8580 RVA: 0x000D937B File Offset: 0x000D757B
		public BoundLocalizedText StrEnableImageTransfer { get; set; }

		// Token: 0x170009AE RID: 2478
		// (get) Token: 0x06002185 RID: 8581 RVA: 0x000D9384 File Offset: 0x000D7584
		// (set) Token: 0x06002186 RID: 8582 RVA: 0x000D938C File Offset: 0x000D758C
		public BoundLocalizedText StrNetworkPath { get; set; }

		// Token: 0x170009AF RID: 2479
		// (get) Token: 0x06002187 RID: 8583 RVA: 0x000D9395 File Offset: 0x000D7595
		// (set) Token: 0x06002188 RID: 8584 RVA: 0x000D939D File Offset: 0x000D759D
		public BoundLocalizedText StrMaximumImageNumber { get; set; }

		// Token: 0x170009B0 RID: 2480
		// (get) Token: 0x06002189 RID: 8585 RVA: 0x000D93A6 File Offset: 0x000D75A6
		// (set) Token: 0x0600218A RID: 8586 RVA: 0x000D93AE File Offset: 0x000D75AE
		public BoundLocalizedText StrConnectionStatus { get; set; }

		// Token: 0x170009B1 RID: 2481
		// (get) Token: 0x0600218B RID: 8587 RVA: 0x000D93B7 File Offset: 0x000D75B7
		// (set) Token: 0x0600218C RID: 8588 RVA: 0x000D93BF File Offset: 0x000D75BF
		public BoundLocalizedText StrImageMode { get; set; }

		// Token: 0x170009B2 RID: 2482
		// (get) Token: 0x0600218D RID: 8589 RVA: 0x000D93C8 File Offset: 0x000D75C8
		// (set) Token: 0x0600218E RID: 8590 RVA: 0x000D93D0 File Offset: 0x000D75D0
		public BoundLocalizedText StrSavingMode { get; set; }

		// Token: 0x170009B3 RID: 2483
		// (get) Token: 0x0600218F RID: 8591 RVA: 0x000D93D9 File Offset: 0x000D75D9
		// (set) Token: 0x06002190 RID: 8592 RVA: 0x000D93E1 File Offset: 0x000D75E1
		public BoundLocalizedText StrCount { get; set; }

		// Token: 0x170009B4 RID: 2484
		// (get) Token: 0x06002191 RID: 8593 RVA: 0x000D93EA File Offset: 0x000D75EA
		// (set) Token: 0x06002192 RID: 8594 RVA: 0x000D93F2 File Offset: 0x000D75F2
		public string ConnectionStatusText { get; set; }

		// Token: 0x170009B5 RID: 2485
		// (get) Token: 0x06002193 RID: 8595 RVA: 0x000D93FB File Offset: 0x000D75FB
		// (set) Token: 0x06002194 RID: 8596 RVA: 0x000D9403 File Offset: 0x000D7603
		public RelayCommand ChangeOperatingDaysCommand { get; set; }

		// Token: 0x140000B0 RID: 176
		// (add) Token: 0x06002195 RID: 8597 RVA: 0x000D940C File Offset: 0x000D760C
		// (remove) Token: 0x06002196 RID: 8598 RVA: 0x000D9444 File Offset: 0x000D7644
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x170009B6 RID: 2486
		// (get) Token: 0x06002197 RID: 8599 RVA: 0x000D9479 File Offset: 0x000D7679
		// (set) Token: 0x06002198 RID: 8600 RVA: 0x000D9481 File Offset: 0x000D7681
		public ObservableCollection<WifiNetwork> AvailableNetworks { get; set; }

		// Token: 0x170009B7 RID: 2487
		// (get) Token: 0x06002199 RID: 8601 RVA: 0x000D948C File Offset: 0x000D768C
		public int NAvailableNetworks
		{
			get
			{
				return this.AvailableNetworks.Count;
			}
		}

		// Token: 0x170009B8 RID: 2488
		// (get) Token: 0x0600219A RID: 8602 RVA: 0x000D94AC File Offset: 0x000D76AC
		// (set) Token: 0x0600219B RID: 8603 RVA: 0x000D94C4 File Offset: 0x000D76C4
		public string LocationNetworkSavingPng
		{
			get
			{
				return this.locationNetworkSavingPng;
			}
			set
			{
				bool flag = value != this.locationNetworkSavingPng;
				if (flag)
				{
					this.locationNetworkSavingPng = value;
					this.dbConnection.SetLocationNetworkSavingPng(this.locationNetworkSavingPng);
					this.OnPropertyChanged("LocationNetworkSavingPng");
				}
			}
		}

		// Token: 0x170009B9 RID: 2489
		// (get) Token: 0x0600219C RID: 8604 RVA: 0x000D950C File Offset: 0x000D770C
		// (set) Token: 0x0600219D RID: 8605 RVA: 0x000D9524 File Offset: 0x000D7724
		public int MaxNumberImages
		{
			get
			{
				return this.maxNumberImages;
			}
			set
			{
				bool flag = value != this.maxNumberImages;
				if (flag)
				{
					this.maxNumberImages = value;
					this.dbConnection.SetMaxImagesNetworkSavingPng(Convert.ToString(this.maxNumberImages));
					this.OnPropertyChanged("MaxNumberImages");
				}
			}
		}

		// Token: 0x170009BA RID: 2490
		// (get) Token: 0x0600219E RID: 8606 RVA: 0x000D9570 File Offset: 0x000D7770
		// (set) Token: 0x0600219F RID: 8607 RVA: 0x000D9588 File Offset: 0x000D7788
		public string StoragePath
		{
			get
			{
				return this.storagePath;
			}
			set
			{
				bool flag = value != this.storagePath;
				if (flag)
				{
					this.storagePath = value;
					this.dbConnection.SetNetworkPathSavingPng(this.storagePath);
					this.OnPropertyChanged("StoragePath");
				}
			}
		}

		// Token: 0x170009BB RID: 2491
		// (get) Token: 0x060021A0 RID: 8608 RVA: 0x000D95D0 File Offset: 0x000D77D0
		// (set) Token: 0x060021A1 RID: 8609 RVA: 0x000D95E8 File Offset: 0x000D77E8
		public string OperatingDays
		{
			get
			{
				return this.operatingDays;
			}
			set
			{
				bool flag = value != this.operatingDays;
				if (flag)
				{
					this.operatingDays = value;
					this.dbConnection.SetModeTimePeriodNetworkSavingPng(this.operatingDays);
					this.OnPropertyChanged("OperatingDays");
				}
			}
		}

		// Token: 0x170009BC RID: 2492
		// (get) Token: 0x060021A2 RID: 8610 RVA: 0x000D9630 File Offset: 0x000D7830
		// (set) Token: 0x060021A3 RID: 8611 RVA: 0x000D9648 File Offset: 0x000D7848
		public SavingPngMode SelectedMode
		{
			get
			{
				return this.selectedMode;
			}
			set
			{
				bool flag = this.selectedMode != value;
				if (flag)
				{
					this.selectedMode = value;
					this.dbConnection.SetModeNetworkSavingPng(this.convertSelectedModetostring(this.selectedMode));
					this.OnPropertyChanged("SelectedMode");
				}
			}
		}

		// Token: 0x170009BD RID: 2493
		// (get) Token: 0x060021A4 RID: 8612 RVA: 0x000D9694 File Offset: 0x000D7894
		// (set) Token: 0x060021A5 RID: 8613 RVA: 0x000D96AC File Offset: 0x000D78AC
		public SavingPngStorageMode SelectedStorageMode
		{
			get
			{
				return this.selectedStorageMode;
			}
			set
			{
				bool flag = this.selectedStorageMode != value;
				if (flag)
				{
					this.selectedStorageMode = value;
					this.dbConnection.SetStorageModeNetworkSavingPng(this.convertSelectedStorageModetostring(this.selectedStorageMode));
					this.OnPropertyChanged("SelectedStorageMode");
				}
			}
		}

		// Token: 0x170009BE RID: 2494
		// (get) Token: 0x060021A6 RID: 8614 RVA: 0x000D96F8 File Offset: 0x000D78F8
		// (set) Token: 0x060021A7 RID: 8615 RVA: 0x000D9710 File Offset: 0x000D7910
		public SavingPngLocation SelectedLocation
		{
			get
			{
				return this.selectedLocation;
			}
			set
			{
				bool flag = this.selectedLocation != value;
				if (flag)
				{
					this.selectedLocation = value;
					this.dbConnection.SetLocationNetworkSavingPng(this.convertSelectedLocationtostring(this.selectedLocation));
					this.OnPropertyChanged("SelectedLocation");
				}
			}
		}

		// Token: 0x170009BF RID: 2495
		// (get) Token: 0x060021A8 RID: 8616 RVA: 0x000D975C File Offset: 0x000D795C
		// (set) Token: 0x060021A9 RID: 8617 RVA: 0x000D9774 File Offset: 0x000D7974
		public bool EditAllowed
		{
			get
			{
				return this.editAllowed;
			}
			set
			{
				this.editAllowed = value;
				this.OnPropertyChanged("EditAllowed");
			}
		}

		// Token: 0x170009C0 RID: 2496
		// (get) Token: 0x060021AA RID: 8618 RVA: 0x000D978C File Offset: 0x000D798C
		// (set) Token: 0x060021AB RID: 8619 RVA: 0x000D97A4 File Offset: 0x000D79A4
		public bool IsNetworkDriveVisible
		{
			get
			{
				return this.isNetworkDriveVisible;
			}
			set
			{
				this.isNetworkDriveVisible = value;
				if (value)
				{
					this.btnShowDirectories.Visibility = Visibility.Visible;
				}
				else
				{
					this.btnShowDirectories.Visibility = Visibility.Collapsed;
				}
				this.OnPropertyChanged("IsNetworkDriveVisible");
			}
		}

		// Token: 0x170009C1 RID: 2497
		// (get) Token: 0x060021AC RID: 8620 RVA: 0x000D97E8 File Offset: 0x000D79E8
		// (set) Token: 0x060021AD RID: 8621 RVA: 0x000D9800 File Offset: 0x000D7A00
		public bool IsOperatingDaysVisible
		{
			get
			{
				return this.isOperatingDaysVisible;
			}
			set
			{
				this.isOperatingDaysVisible = value;
				this.OnPropertyChanged("IsOperatingDaysVisible");
			}
		}

		// Token: 0x170009C2 RID: 2498
		// (get) Token: 0x060021AE RID: 8622 RVA: 0x000D9818 File Offset: 0x000D7A18
		// (set) Token: 0x060021AF RID: 8623 RVA: 0x000D9830 File Offset: 0x000D7A30
		public bool FileListVisible
		{
			get
			{
				return this.fileListVisible;
			}
			set
			{
				this.fileListVisible = value;
				this.OnPropertyChanged("FileListVisible");
			}
		}

		// Token: 0x060021B0 RID: 8624 RVA: 0x000D9846 File Offset: 0x000D7A46
		public WifiImageTransferViewModel(DBConnection dbConnection)
		{
			this.dbConnection = dbConnection;
			this.Initialize();
		}

		// Token: 0x060021B1 RID: 8625 RVA: 0x000D9868 File Offset: 0x000D7A68
		private void RegisterLocalizedText()
		{
			this.StrEnableImageTransfer = new BoundLocalizedText("menuItemImageTransfer");
			this.StrNetworkPath = new BoundLocalizedText("txtNetworkPath");
			this.StrMaximumImageNumber = new BoundLocalizedText("txtMaximumNumberPerFolder");
			this.StrConnectionStatus = new BoundLocalizedText("txtConnectionStatus");
			this.StrImageMode = new BoundLocalizedText("txtImageMode");
			this.StrSavingMode = new BoundLocalizedText("txtSavingMode");
			this.StrCount = new BoundLocalizedText("txtCount");
		}

		// Token: 0x060021B2 RID: 8626 RVA: 0x000D98F0 File Offset: 0x000D7AF0
		private void DeregisterLocalizedText()
		{
			MainWindow.localizationManager.Deregister(this.StrEnableImageTransfer);
			MainWindow.localizationManager.Deregister(this.StrNetworkPath);
			MainWindow.localizationManager.Deregister(this.StrMaximumImageNumber);
			MainWindow.localizationManager.Deregister(this.StrConnectionStatus);
			MainWindow.localizationManager.Deregister(this.StrImageMode);
			MainWindow.localizationManager.Deregister(this.StrSavingMode);
			MainWindow.localizationManager.Deregister(this.StrCount);
		}

		// Token: 0x060021B3 RID: 8627 RVA: 0x000D9978 File Offset: 0x000D7B78
		private void Initialize()
		{
			this.RegisterLocalizedText();
			this.btnShowDirectories = new Button();
			this.ChangeMaxImagePerFolderCommand = new RelayCommand(new Action<object>(this.ChangeMaxImagePerFolder));
			this.ChangeStoragePathCommand = new RelayCommand(new Action<object>(this.ChangeStoragePath));
			this.ChangeOperatingDaysCommand = new RelayCommand(new Action<object>(this.ChangeOperatingDays));
			this.locationNetworkSavingPng = this.dbConnection.GetLocationNetworkSavingPng();
			this.maxNumberImages = Convert.ToInt32(this.dbConnection.GetMaxImagesNetworkSavingPng());
			this.selectedMode = this.convertStringToSelectedMode();
			this.selectedLocation = this.convertStringToSelectedLocation();
			this.selectedStorageMode = this.convertStringToSelectedStorageMode();
			this.operatingDays = this.dbConnection.GetModeTimePeriodNetworkSavingPng();
			this.isNetworkDriveVisible = (this.selectedLocation == SavingPngLocation.NetworkFolder || this.selectedLocation == SavingPngLocation.NetworkAndLocalFolderFTP);
			this.isOperatingDaysVisible = (this.selectedStorageMode == SavingPngStorageMode.OperatingDays);
			string networkPathSavingPng = this.dbConnection.GetNetworkPathSavingPng();
			bool flag = networkPathSavingPng != null && networkPathSavingPng != "";
			if (flag)
			{
				this.storagePath = networkPathSavingPng;
			}
			else
			{
				this.storagePath = "null";
			}
			this.ConnectionStatusText = "TO DO";
			this.editAllowed = true;
		}

		// Token: 0x060021B4 RID: 8628 RVA: 0x000D9AB0 File Offset: 0x000D7CB0
		private SavingPngMode convertStringToSelectedMode()
		{
			string modeNetworkSavingPng = this.dbConnection.GetModeNetworkSavingPng();
			bool flag = modeNetworkSavingPng == "0";
			SavingPngMode result;
			if (flag)
			{
				result = SavingPngMode.BadImages;
			}
			else
			{
				bool flag2 = modeNetworkSavingPng == "1";
				if (flag2)
				{
					result = SavingPngMode.GoodImages;
				}
				else
				{
					bool flag3 = modeNetworkSavingPng == "2";
					if (flag3)
					{
						result = SavingPngMode.GoodandBadImages;
					}
					else
					{
						result = SavingPngMode.BadImages;
					}
				}
			}
			return result;
		}

		// Token: 0x060021B5 RID: 8629 RVA: 0x000D9B0C File Offset: 0x000D7D0C
		private string convertSelectedModetostring(SavingPngMode mode)
		{
			bool flag = mode == SavingPngMode.BadImages;
			string result;
			if (flag)
			{
				result = "0";
			}
			else
			{
				bool flag2 = mode == SavingPngMode.GoodImages;
				if (flag2)
				{
					result = "1";
				}
				else
				{
					bool flag3 = mode == SavingPngMode.GoodandBadImages;
					if (flag3)
					{
						result = "2";
					}
					else
					{
						result = "0";
					}
				}
			}
			return result;
		}

		// Token: 0x060021B6 RID: 8630 RVA: 0x000D9B54 File Offset: 0x000D7D54
		private SavingPngLocation convertStringToSelectedLocation()
		{
			string a = this.dbConnection.GetLocationNetworkSavingPng();
			bool flag = a == "0";
			SavingPngLocation result;
			if (flag)
			{
				result = SavingPngLocation.Disabled;
			}
			else
			{
				bool flag2 = a == "1";
				if (flag2)
				{
					result = SavingPngLocation.NetworkFolder;
				}
				else
				{
					bool flag3 = a == "2";
					if (flag3)
					{
						result = SavingPngLocation.LocalFolderFTP;
					}
					else
					{
						bool flag4 = a == "3";
						if (flag4)
						{
							result = SavingPngLocation.NetworkAndLocalFolderFTP;
						}
						else
						{
							result = SavingPngLocation.Disabled;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x060021B7 RID: 8631 RVA: 0x000D9BC4 File Offset: 0x000D7DC4
		private string convertSelectedLocationtostring(SavingPngLocation mode)
		{
			bool flag = mode == SavingPngLocation.Disabled;
			string result;
			if (flag)
			{
				this.IsNetworkDriveVisible = false;
				this.IsOperatingDaysVisible = false;
				result = "0";
			}
			else
			{
				bool flag2 = mode == SavingPngLocation.NetworkFolder;
				if (flag2)
				{
					this.IsNetworkDriveVisible = true;
					this.IsOperatingDaysVisible = (this.selectedStorageMode == SavingPngStorageMode.OperatingDays);
					result = "1";
				}
				else
				{
					bool flag3 = mode == SavingPngLocation.LocalFolderFTP;
					if (flag3)
					{
						this.IsNetworkDriveVisible = false;
						this.IsOperatingDaysVisible = false;
						result = "2";
					}
					else
					{
						bool flag4 = mode == SavingPngLocation.NetworkAndLocalFolderFTP;
						if (flag4)
						{
							this.IsNetworkDriveVisible = true;
							this.IsOperatingDaysVisible = (this.selectedStorageMode == SavingPngStorageMode.OperatingDays);
							result = "3";
						}
						else
						{
							result = "0";
						}
					}
				}
			}
			return result;
		}

		// Token: 0x060021B8 RID: 8632 RVA: 0x000D9C7C File Offset: 0x000D7E7C
		private SavingPngStorageMode convertStringToSelectedStorageMode()
		{
			string storageModeNetworkSavingPng = this.dbConnection.GetStorageModeNetworkSavingPng();
			bool flag = storageModeNetworkSavingPng == "0";
			SavingPngStorageMode result;
			if (flag)
			{
				result = SavingPngStorageMode.Disabled;
			}
			else
			{
				bool flag2 = storageModeNetworkSavingPng == "1";
				if (flag2)
				{
					result = SavingPngStorageMode.OperatingDays;
				}
				else
				{
					bool flag3 = storageModeNetworkSavingPng == "2";
					if (flag3)
					{
						result = SavingPngStorageMode.MaxCapacity;
					}
					else
					{
						result = SavingPngStorageMode.Disabled;
					}
				}
			}
			return result;
		}

		// Token: 0x060021B9 RID: 8633 RVA: 0x000D9CD8 File Offset: 0x000D7ED8
		private string convertSelectedStorageModetostring(SavingPngStorageMode mode)
		{
			bool flag = mode == SavingPngStorageMode.Disabled;
			string result;
			if (flag)
			{
				this.IsOperatingDaysVisible = false;
				result = "0";
			}
			else
			{
				bool flag2 = mode == SavingPngStorageMode.OperatingDays;
				if (flag2)
				{
					this.IsOperatingDaysVisible = true;
					result = "1";
				}
				else
				{
					bool flag3 = mode == SavingPngStorageMode.MaxCapacity;
					if (flag3)
					{
						this.IsOperatingDaysVisible = false;
						result = "2";
					}
					else
					{
						result = "0";
					}
				}
			}
			return result;
		}

		// Token: 0x060021BA RID: 8634 RVA: 0x000D9D3C File Offset: 0x000D7F3C
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x060021BB RID: 8635 RVA: 0x000D9D6C File Offset: 0x000D7F6C
		public void CleanUp()
		{
			try
			{
				this.DeregisterLocalizedText();
			}
			catch
			{
			}
		}

		// Token: 0x060021BC RID: 8636 RVA: 0x000D9D9C File Offset: 0x000D7F9C
		public void ChangeMaxImagePerFolder(object obj)
		{
			this.numKeyboard = new NumKeyboard("", this.MaxNumberImages.ToString(), NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_InputMaxImages;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x060021BD RID: 8637 RVA: 0x000D9DF0 File Offset: 0x000D7FF0
		private void numKeyboard_InputMaxImages(object sender, KeyboardInputEventArgs e)
		{
			try
			{
				int num = int.Parse(e.Text);
				bool flag = num != 0;
				if (flag)
				{
					this.MaxNumberImages = num;
				}
			}
			catch
			{
			}
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_InputMaxImages;
		}

		// Token: 0x060021BE RID: 8638 RVA: 0x000D9E4C File Offset: 0x000D804C
		public void ChangeStoragePath(object obj)
		{
			this.keyboard = new Keyboard("", this.StoragePath.ToString(), KeyboardType.Network);
			this.keyboard.TextInputKeyboard += this.keyboard_ChangeStoragePath;
			this.keyboard.ShowDialog();
		}

		// Token: 0x060021BF RID: 8639 RVA: 0x000D9E9C File Offset: 0x000D809C
		private void keyboard_ChangeStoragePath(object sender, KeyboardInputEventArgs e)
		{
			try
			{
				this.StoragePath = e.Text;
			}
			catch
			{
			}
			this.keyboard.TextInputKeyboard -= this.keyboard_ChangeStoragePath;
		}

		// Token: 0x060021C0 RID: 8640 RVA: 0x000D9EE8 File Offset: 0x000D80E8
		public void ChangeOperatingDays(object obj)
		{
			this.keyboard = new Keyboard("", this.OperatingDays.ToString(), KeyboardType.Default);
			this.keyboard.TextInputKeyboard += this.keyboard_ChangeOperatingDays;
			this.keyboard.ShowDialog();
		}

		// Token: 0x060021C1 RID: 8641 RVA: 0x000D9F38 File Offset: 0x000D8138
		private void keyboard_ChangeOperatingDays(object sender, KeyboardInputEventArgs e)
		{
			try
			{
				this.OperatingDays = e.Text;
			}
			catch
			{
			}
			this.keyboard.TextInputKeyboard -= this.keyboard_ChangeOperatingDays;
		}

		// Token: 0x040015CC RID: 5580
		private DBConnection dbConnection;

		// Token: 0x040015CD RID: 5581
		private NumKeyboard numKeyboard;

		// Token: 0x040015CE RID: 5582
		private Keyboard keyboard;

		// Token: 0x040015DC RID: 5596
		public Button btnShowDirectories;

		// Token: 0x040015DD RID: 5597
		private string locationNetworkSavingPng;

		// Token: 0x040015DE RID: 5598
		private int maxNumberImages;

		// Token: 0x040015DF RID: 5599
		private string storagePath;

		// Token: 0x040015E0 RID: 5600
		private string operatingDays;

		// Token: 0x040015E1 RID: 5601
		private SavingPngMode selectedMode;

		// Token: 0x040015E2 RID: 5602
		private SavingPngStorageMode selectedStorageMode;

		// Token: 0x040015E3 RID: 5603
		private SavingPngLocation selectedLocation;

		// Token: 0x040015E4 RID: 5604
		private bool editAllowed;

		// Token: 0x040015E5 RID: 5605
		private bool isNetworkDriveVisible;

		// Token: 0x040015E6 RID: 5606
		private bool isOperatingDaysVisible;

		// Token: 0x040015E7 RID: 5607
		private bool fileListVisible = false;
	}
}
