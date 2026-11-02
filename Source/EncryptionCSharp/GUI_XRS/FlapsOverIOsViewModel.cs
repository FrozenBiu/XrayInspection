using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Threading;
using Common;
using DatabaseConnection;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x02000049 RID: 73
	public class FlapsOverIOsViewModel : INotifyPropertyChanged
	{
		// Token: 0x1400002A RID: 42
		// (add) Token: 0x0600071E RID: 1822 RVA: 0x00030038 File Offset: 0x0002E238
		// (remove) Token: 0x0600071F RID: 1823 RVA: 0x00030070 File Offset: 0x0002E270
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x1700020F RID: 527
		// (get) Token: 0x06000720 RID: 1824 RVA: 0x000300A5 File Offset: 0x0002E2A5
		// (set) Token: 0x06000721 RID: 1825 RVA: 0x000300AD File Offset: 0x0002E2AD
		public MachineMode MachineMode { get; set; }

		// Token: 0x17000210 RID: 528
		// (get) Token: 0x06000722 RID: 1826 RVA: 0x000300B6 File Offset: 0x0002E2B6
		// (set) Token: 0x06000723 RID: 1827 RVA: 0x000300BE File Offset: 0x0002E2BE
		public Rejecter Rejecter1 { get; set; }

		// Token: 0x17000211 RID: 529
		// (get) Token: 0x06000724 RID: 1828 RVA: 0x000300C7 File Offset: 0x0002E2C7
		// (set) Token: 0x06000725 RID: 1829 RVA: 0x000300CF File Offset: 0x0002E2CF
		public FlapsOverIOs flapsOverIOs { get; set; }

		// Token: 0x17000212 RID: 530
		// (get) Token: 0x06000726 RID: 1830 RVA: 0x000300D8 File Offset: 0x0002E2D8
		// (set) Token: 0x06000727 RID: 1831 RVA: 0x000300F0 File Offset: 0x0002E2F0
		public bool IsBulkFlaps
		{
			get
			{
				return this.isBulkFlaps;
			}
			set
			{
				bool flag = this.isBulkFlaps != value;
				if (flag)
				{
					this.isBulkFlaps = value;
					this.OnPropertyChanged("IsBulkFlaps");
				}
			}
		}

		// Token: 0x17000213 RID: 531
		// (get) Token: 0x06000728 RID: 1832 RVA: 0x00030124 File Offset: 0x0002E324
		// (set) Token: 0x06000729 RID: 1833 RVA: 0x0003013C File Offset: 0x0002E33C
		public bool IsBulkObjectFlaps
		{
			get
			{
				return this.isBulkObjectFlaps;
			}
			set
			{
				bool flag = this.isBulkObjectFlaps != value;
				if (flag)
				{
					this.isBulkObjectFlaps = value;
					this.OnPropertyChanged("IsBulkObjectFlaps");
				}
			}
		}

		// Token: 0x17000214 RID: 532
		// (get) Token: 0x0600072A RID: 1834 RVA: 0x00030170 File Offset: 0x0002E370
		// (set) Token: 0x0600072B RID: 1835 RVA: 0x00030188 File Offset: 0x0002E388
		public bool IsBulkBinFullVisible
		{
			get
			{
				return this.isBulkBinFullVisible;
			}
			set
			{
				bool flag = this.isBulkBinFullVisible != value;
				if (flag)
				{
					this.isBulkBinFullVisible = value;
					this.OnPropertyChanged("IsBulkBinFullVisible");
				}
			}
		}

		// Token: 0x17000215 RID: 533
		// (get) Token: 0x0600072C RID: 1836 RVA: 0x000301BC File Offset: 0x0002E3BC
		// (set) Token: 0x0600072D RID: 1837 RVA: 0x000301D4 File Offset: 0x0002E3D4
		public bool IsBulkRejectVerifyVisible
		{
			get
			{
				return this.isBulkRejectVerifyVisible;
			}
			set
			{
				bool flag = this.isBulkRejectVerifyVisible != value;
				if (flag)
				{
					this.isBulkRejectVerifyVisible = value;
					this.OnPropertyChanged("IsBulkRejectVerifyVisible");
				}
			}
		}

		// Token: 0x17000216 RID: 534
		// (get) Token: 0x0600072E RID: 1838 RVA: 0x00030207 File Offset: 0x0002E407
		// (set) Token: 0x0600072F RID: 1839 RVA: 0x0003020F File Offset: 0x0002E40F
		public bool MinOP1LoggedIn { get; set; }

		// Token: 0x17000217 RID: 535
		// (get) Token: 0x06000730 RID: 1840 RVA: 0x00030218 File Offset: 0x0002E418
		// (set) Token: 0x06000731 RID: 1841 RVA: 0x00030220 File Offset: 0x0002E420
		public bool MinOP3LoggedIn { get; set; }

		// Token: 0x17000218 RID: 536
		// (get) Token: 0x06000732 RID: 1842 RVA: 0x00030229 File Offset: 0x0002E429
		// (set) Token: 0x06000733 RID: 1843 RVA: 0x00030231 File Offset: 0x0002E431
		public bool MinAdminLoggedIn { get; set; }

		// Token: 0x17000219 RID: 537
		// (get) Token: 0x06000734 RID: 1844 RVA: 0x0003023A File Offset: 0x0002E43A
		// (set) Token: 0x06000735 RID: 1845 RVA: 0x00030242 File Offset: 0x0002E442
		public bool MinServiceLoggedIn { get; set; }

		// Token: 0x1700021A RID: 538
		// (get) Token: 0x06000736 RID: 1846 RVA: 0x0003024B File Offset: 0x0002E44B
		// (set) Token: 0x06000737 RID: 1847 RVA: 0x00030253 File Offset: 0x0002E453
		public bool RejectAllIsVisible { get; set; }

		// Token: 0x1700021B RID: 539
		// (get) Token: 0x06000738 RID: 1848 RVA: 0x0003025C File Offset: 0x0002E45C
		// (set) Token: 0x06000739 RID: 1849 RVA: 0x00030264 File Offset: 0x0002E464
		public RelayCommand SetDelayCommand { get; set; }

		// Token: 0x1700021C RID: 540
		// (get) Token: 0x0600073A RID: 1850 RVA: 0x0003026D File Offset: 0x0002E46D
		// (set) Token: 0x0600073B RID: 1851 RVA: 0x00030275 File Offset: 0x0002E475
		public RelayCommand SetEjectTimeCommand { get; set; }

		// Token: 0x1700021D RID: 541
		// (get) Token: 0x0600073C RID: 1852 RVA: 0x0003027E File Offset: 0x0002E47E
		// (set) Token: 0x0600073D RID: 1853 RVA: 0x00030286 File Offset: 0x0002E486
		public RelayCommand SetFlapsCountCommand { get; set; }

		// Token: 0x1700021E RID: 542
		// (get) Token: 0x0600073E RID: 1854 RVA: 0x0003028F File Offset: 0x0002E48F
		// (set) Token: 0x0600073F RID: 1855 RVA: 0x00030297 File Offset: 0x0002E497
		public RelayCommand SetValveTestIntervalCommand { get; set; }

		// Token: 0x1700021F RID: 543
		// (get) Token: 0x06000740 RID: 1856 RVA: 0x000302A0 File Offset: 0x0002E4A0
		// (set) Token: 0x06000741 RID: 1857 RVA: 0x000302A8 File Offset: 0x0002E4A8
		public RelayCommand SetImageSizeCommand { get; set; }

		// Token: 0x17000220 RID: 544
		// (get) Token: 0x06000742 RID: 1858 RVA: 0x000302B1 File Offset: 0x0002E4B1
		// (set) Token: 0x06000743 RID: 1859 RVA: 0x000302B9 File Offset: 0x0002E4B9
		public RelayCommand StartValveTestCommand { get; set; }

		// Token: 0x17000221 RID: 545
		// (get) Token: 0x06000744 RID: 1860 RVA: 0x000302C2 File Offset: 0x0002E4C2
		// (set) Token: 0x06000745 RID: 1861 RVA: 0x000302CA File Offset: 0x0002E4CA
		public RelayCommand SetBinFullTimeoutCommand { get; set; }

		// Token: 0x17000222 RID: 546
		// (get) Token: 0x06000746 RID: 1862 RVA: 0x000302D3 File Offset: 0x0002E4D3
		// (set) Token: 0x06000747 RID: 1863 RVA: 0x000302DB File Offset: 0x0002E4DB
		public RelayCommand SetRejecter1DelayCommand { get; set; }

		// Token: 0x17000223 RID: 547
		// (get) Token: 0x06000748 RID: 1864 RVA: 0x000302E4 File Offset: 0x0002E4E4
		// (set) Token: 0x06000749 RID: 1865 RVA: 0x000302EC File Offset: 0x0002E4EC
		public RelayCommand SetRejecter1EjectTimeCommand { get; set; }

		// Token: 0x17000224 RID: 548
		// (get) Token: 0x0600074A RID: 1866 RVA: 0x000302F5 File Offset: 0x0002E4F5
		// (set) Token: 0x0600074B RID: 1867 RVA: 0x000302FD File Offset: 0x0002E4FD
		public RelayCommand SetVerifySensorBlockedTimeoutCommand { get; set; }

		// Token: 0x17000225 RID: 549
		// (get) Token: 0x0600074C RID: 1868 RVA: 0x00030306 File Offset: 0x0002E506
		// (set) Token: 0x0600074D RID: 1869 RVA: 0x0003030E File Offset: 0x0002E50E
		public RelayCommand CleanUpCommand { get; set; }

		// Token: 0x17000226 RID: 550
		// (get) Token: 0x0600074E RID: 1870 RVA: 0x00030317 File Offset: 0x0002E517
		// (set) Token: 0x0600074F RID: 1871 RVA: 0x0003031F File Offset: 0x0002E51F
		public BoundLocalizedText StrRejecterEnabled { get; set; }

		// Token: 0x17000227 RID: 551
		// (get) Token: 0x06000750 RID: 1872 RVA: 0x00030328 File Offset: 0x0002E528
		// (set) Token: 0x06000751 RID: 1873 RVA: 0x00030330 File Offset: 0x0002E530
		public BoundLocalizedText StrEjectTimeInit { get; set; }

		// Token: 0x17000228 RID: 552
		// (get) Token: 0x06000752 RID: 1874 RVA: 0x00030339 File Offset: 0x0002E539
		// (set) Token: 0x06000753 RID: 1875 RVA: 0x00030341 File Offset: 0x0002E541
		public BoundLocalizedText StrEjectTimeAdditional { get; set; }

		// Token: 0x17000229 RID: 553
		// (get) Token: 0x06000754 RID: 1876 RVA: 0x0003034A File Offset: 0x0002E54A
		// (set) Token: 0x06000755 RID: 1877 RVA: 0x00030352 File Offset: 0x0002E552
		public BoundLocalizedText StrDelay { get; set; }

		// Token: 0x1700022A RID: 554
		// (get) Token: 0x06000756 RID: 1878 RVA: 0x0003035B File Offset: 0x0002E55B
		// (set) Token: 0x06000757 RID: 1879 RVA: 0x00030363 File Offset: 0x0002E563
		public BoundLocalizedText StrFlapsCount { get; set; }

		// Token: 0x1700022B RID: 555
		// (get) Token: 0x06000758 RID: 1880 RVA: 0x0003036C File Offset: 0x0002E56C
		// (set) Token: 0x06000759 RID: 1881 RVA: 0x00030374 File Offset: 0x0002E574
		public BoundLocalizedText StrFlapMapping { get; set; }

		// Token: 0x1700022C RID: 556
		// (get) Token: 0x0600075A RID: 1882 RVA: 0x0003037D File Offset: 0x0002E57D
		// (set) Token: 0x0600075B RID: 1883 RVA: 0x00030385 File Offset: 0x0002E585
		public BoundLocalizedText StrStartValveTest { get; set; }

		// Token: 0x1700022D RID: 557
		// (get) Token: 0x0600075C RID: 1884 RVA: 0x0003038E File Offset: 0x0002E58E
		// (set) Token: 0x0600075D RID: 1885 RVA: 0x00030396 File Offset: 0x0002E596
		public BoundLocalizedText StrInterval { get; set; }

		// Token: 0x1700022E RID: 558
		// (get) Token: 0x0600075E RID: 1886 RVA: 0x0003039F File Offset: 0x0002E59F
		// (set) Token: 0x0600075F RID: 1887 RVA: 0x000303A7 File Offset: 0x0002E5A7
		public BoundLocalizedText StrImageSize { get; set; }

		// Token: 0x1700022F RID: 559
		// (get) Token: 0x06000760 RID: 1888 RVA: 0x000303B0 File Offset: 0x0002E5B0
		// (set) Token: 0x06000761 RID: 1889 RVA: 0x000303B8 File Offset: 0x0002E5B8
		public BoundLocalizedText StrRejectAll { get; set; }

		// Token: 0x06000762 RID: 1890 RVA: 0x000303C1 File Offset: 0x0002E5C1
		public FlapsOverIOsViewModel(ParameterHandling parHandling, DBConnection dbConnection, MachineState machineState, AccessRightsManager accessManager, MessageSystem messageSystem, MachineMode machineMode) : this(parHandling, dbConnection, machineState, accessManager, true, messageSystem, machineMode)
		{
		}

		// Token: 0x06000763 RID: 1891 RVA: 0x000303D8 File Offset: 0x0002E5D8
		public FlapsOverIOsViewModel(ParameterHandling parHandling, DBConnection dbConnection, MachineState machineState, AccessRightsManager accessManager, bool showHazardMessage, MessageSystem messageSystem, MachineMode machineMode)
		{
			this.accessManager = accessManager;
			this.parHandling = parHandling;
			this.MachineMode = machineMode;
			bool flag = machineMode == MachineMode.Bulk;
			if (flag)
			{
				this.IsBulkFlaps = true;
			}
			bool flag2 = machineMode == MachineMode.BulkObject;
			if (flag2)
			{
				this.IsBulkObjectFlaps = true;
			}
			dbConnection.GetBulkBinFullSensor(ref this.isBulkBinFullVisible);
			dbConnection.GetBulkRejectVerifySensor(ref this.isBulkRejectVerifyVisible);
			this.SetDelayCommand = new RelayCommand(new Action<object>(this.SetDelay));
			this.SetEjectTimeCommand = new RelayCommand(new Action<object>(this.SetEjectTime));
			this.StartValveTestCommand = new RelayCommand(new Action<object>(this.StartValveTest));
			this.SetValveTestIntervalCommand = new RelayCommand(new Action<object>(this.SetTestInterval));
			this.SetImageSizeCommand = new RelayCommand(new Action<object>(this.SetImageSize));
			this.SetRejecter1DelayCommand = new RelayCommand(new Action<object>(this.SetRejecter1Delay));
			this.SetRejecter1EjectTimeCommand = new RelayCommand(new Action<object>(this.SetRejecter1EjectTime));
			this.SetBinFullTimeoutCommand = new RelayCommand(new Action<object>(this.SetBinFullTimeout));
			this.SetVerifySensorBlockedTimeoutCommand = new RelayCommand(new Action<object>(this.SetVerifySensorBlockedTimeout));
			this.CleanUpCommand = new RelayCommand(new Action<object>(this.CleanUp));
			this.MinOP1LoggedIn = (accessManager.CurrentUser.Rights >= BuiltInPriviledges.Operator1);
			this.MinOP3LoggedIn = (accessManager.CurrentUser.Rights >= BuiltInPriviledges.Operator3);
			this.MinAdminLoggedIn = (accessManager.CurrentUser.Rights >= BuiltInPriviledges.Admin);
			this.MinServiceLoggedIn = (accessManager.CurrentUser.Rights >= BuiltInPriviledges.Service);
			this.RejectAllIsVisible = (parHandling.ReadPar(FlapsOverIOs.bulkflapsFlapsCountPath) == "4");
			this.flapsOverIOs = new FlapsOverIOs(parHandling, dbConnection, machineState, showHazardMessage);
			this.flapsOverIOs.ValveTestIsRunning = false;
			this.RegisterLocalizedText();
			this.InitializeRejecters(parHandling, dbConnection, accessManager, machineState, messageSystem);
		}

		// Token: 0x06000764 RID: 1892 RVA: 0x000305FC File Offset: 0x0002E7FC
		private void SetDelay(object obj)
		{
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtDelay"), this.flapsOverIOs.Delay.ToString(), NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_SetDelay;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x06000765 RID: 1893 RVA: 0x00030658 File Offset: 0x0002E858
		private void numKeyboard_SetDelay(object sender, KeyboardInputEventArgs e)
		{
			try
			{
				this.flapsOverIOs.Delay = int.Parse(e.Text);
			}
			catch
			{
			}
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_SetDelay;
		}

		// Token: 0x06000766 RID: 1894 RVA: 0x000306B0 File Offset: 0x0002E8B0
		private void SetEjectTime(object obj)
		{
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtEjectTime"), this.flapsOverIOs.EjectTime.ToString(), NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_SetEjectTime;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x06000767 RID: 1895 RVA: 0x0003070C File Offset: 0x0002E90C
		private void numKeyboard_SetEjectTime(object sender, KeyboardInputEventArgs e)
		{
			try
			{
				this.flapsOverIOs.EjectTime = int.Parse(e.Text);
			}
			catch
			{
			}
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_SetEjectTime;
		}

		// Token: 0x06000768 RID: 1896 RVA: 0x00030764 File Offset: 0x0002E964
		private void SetTestInterval(object obj)
		{
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtInterval"), this.flapsOverIOs.ValveTestInterval.ToString(), NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_SetTestInterval;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x06000769 RID: 1897 RVA: 0x000307C0 File Offset: 0x0002E9C0
		private void numKeyboard_SetTestInterval(object sender, KeyboardInputEventArgs e)
		{
			try
			{
				this.flapsOverIOs.ValveTestInterval = int.Parse(e.Text);
			}
			catch
			{
			}
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_SetTestInterval;
		}

		// Token: 0x0600076A RID: 1898 RVA: 0x00030818 File Offset: 0x0002EA18
		private void SetImageSize(object obj)
		{
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtImageSize"), this.flapsOverIOs.ImageSize.ToString(), NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_SetImageSize;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x0600076B RID: 1899 RVA: 0x00030874 File Offset: 0x0002EA74
		private void numKeyboard_SetImageSize(object sender, KeyboardInputEventArgs e)
		{
			try
			{
				this.flapsOverIOs.ImageSize = int.Parse(e.Text);
			}
			catch
			{
			}
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_SetImageSize;
		}

		// Token: 0x0600076C RID: 1900 RVA: 0x000308CC File Offset: 0x0002EACC
		private void SetRejecter1Delay(object obj)
		{
			this.numKeyboard = new NumKeyboard(this.Rejecter1.StrDelay.LocalizedText, this.Rejecter1.Delay.ToString(), NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_SetRejecter1Delay;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x0600076D RID: 1901 RVA: 0x00030930 File Offset: 0x0002EB30
		private void numKeyboard_SetRejecter1Delay(object sender, KeyboardInputEventArgs e)
		{
			try
			{
				this.Rejecter1.Delay = int.Parse(e.Text);
			}
			catch
			{
			}
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_SetRejecter1EjectTime;
		}

		// Token: 0x0600076E RID: 1902 RVA: 0x00030988 File Offset: 0x0002EB88
		private void SetRejecter1EjectTime(object obj)
		{
			this.numKeyboard = new NumKeyboard(this.Rejecter1.StrEjectTime.LocalizedText, this.Rejecter1.EjectTime.ToString(), NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_SetRejecter1EjectTime;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x0600076F RID: 1903 RVA: 0x000309EC File Offset: 0x0002EBEC
		private void numKeyboard_SetRejecter1EjectTime(object sender, KeyboardInputEventArgs e)
		{
			try
			{
				this.Rejecter1.EjectTime = int.Parse(e.Text);
			}
			catch
			{
			}
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_SetRejecter1EjectTime;
		}

		// Token: 0x06000770 RID: 1904 RVA: 0x00030A44 File Offset: 0x0002EC44
		private void SetBinFullTimeout(object obj)
		{
			this.numKeyboard = new NumKeyboard(this.Rejecter1.StrBinFullTimeout.LocalizedText, this.Rejecter1.BinFullTimeout.ToString(), NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_SetBinBinFullTimeout;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x06000771 RID: 1905 RVA: 0x00030AA8 File Offset: 0x0002ECA8
		private void numKeyboard_SetBinBinFullTimeout(object sender, KeyboardInputEventArgs e)
		{
			try
			{
				this.Rejecter1.BinFullTimeout = int.Parse(e.Text);
			}
			catch
			{
			}
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_SetBinBinFullTimeout;
		}

		// Token: 0x06000772 RID: 1906 RVA: 0x00030B00 File Offset: 0x0002ED00
		private void SetVerifySensorBlockedTimeout(object obj)
		{
			this.numKeyboard = new NumKeyboard(this.Rejecter1.StrRejectVerifySensorBlockedTimeout.LocalizedText, this.Rejecter1.RejectVerifySensorBlockedTimeout.ToString(), NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.SetVerifySensorBlockedTimeout;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x06000773 RID: 1907 RVA: 0x00030B64 File Offset: 0x0002ED64
		private void SetVerifySensorBlockedTimeout(object sender, KeyboardInputEventArgs e)
		{
			try
			{
				this.Rejecter1.RejectVerifySensorBlockedTimeout = int.Parse(e.Text);
			}
			catch
			{
			}
			this.numKeyboard.TextInputKeyboard -= this.SetVerifySensorBlockedTimeout;
		}

		// Token: 0x06000774 RID: 1908 RVA: 0x00030BBC File Offset: 0x0002EDBC
		private void StartValveTest(object obj)
		{
			this.flapsOverIOs.ValveTestStart = true;
			this.flapsOverIOs.ValveTestIsRunning = true;
			double num = Convert.ToDouble(this.parHandling.ReadPar(FlapsOverIOs.bulkflapsValveTestIntervalPath));
			double num2 = Convert.ToDouble(this.parHandling.ReadPar(FlapsOverIOs.bulkflapsFlapsCountPath));
			bool flag = num != 0.0;
			if (flag)
			{
				num *= num2 + 0.5;
				this.timerValveTestIsRunning = new DispatcherTimer(new TimeSpan(0, 0, 0, 0, Convert.ToInt32(num)), DispatcherPriority.Normal, new EventHandler(this.checkValveTestIsRunning), Application.Current.Dispatcher);
				this.timerValveTestIsRunning.Start();
				this.parHandling.WriteValue(FlapsOverIOs.bulkflapsValveTestIsRunningPath, "1");
			}
		}

		// Token: 0x06000775 RID: 1909 RVA: 0x00030C87 File Offset: 0x0002EE87
		private void checkValveTestIsRunning(object dummy, EventArgs e)
		{
			this.flapsOverIOs.ValveTestStart = false;
			this.flapsOverIOs.ValveTestIsRunning = false;
			this.timerValveTestIsRunning.Stop();
		}

		// Token: 0x06000776 RID: 1910 RVA: 0x00030CB0 File Offset: 0x0002EEB0
		private void RegisterLocalizedText()
		{
			this.StrEjectTimeInit = new BoundLocalizedText("txtEjectTimeInit");
			this.StrEjectTimeAdditional = new BoundLocalizedText("txtEjectTimeAdditional");
			this.StrDelay = new BoundLocalizedText("txtDelay");
			this.StrRejecterEnabled = new BoundLocalizedText("txtRejecterEnabled");
			this.StrFlapsCount = new BoundLocalizedText("txtFlapsCount");
			this.StrFlapMapping = new BoundLocalizedText("txtFlapMapping");
			this.StrStartValveTest = new BoundLocalizedText("txtStartValveTest");
			this.StrInterval = new BoundLocalizedText("txtInterval");
			this.StrImageSize = new BoundLocalizedText("txtImageSize");
			this.StrRejectAll = new BoundLocalizedText("txtRejectWithAllFlaps");
		}

		// Token: 0x06000777 RID: 1911 RVA: 0x00030D68 File Offset: 0x0002EF68
		private void CleanUp(object obj)
		{
			MainWindow.localizationManager.Deregister(this.StrEjectTimeInit);
			MainWindow.localizationManager.Deregister(this.StrEjectTimeAdditional);
			MainWindow.localizationManager.Deregister(this.StrDelay);
			MainWindow.localizationManager.Deregister(this.StrRejecterEnabled);
			MainWindow.localizationManager.Deregister(this.StrStartValveTest);
			MainWindow.localizationManager.Deregister(this.StrInterval);
			MainWindow.localizationManager.Deregister(this.StrFlapsCount);
			MainWindow.localizationManager.Deregister(this.StrFlapMapping);
			MainWindow.localizationManager.Deregister(this.StrImageSize);
			MainWindow.localizationManager.Deregister(this.StrRejectAll);
			bool flag = this.timerValveTestIsRunning != null;
			if (flag)
			{
				this.timerValveTestIsRunning.Stop();
			}
			this.flapsOverIOs.ValveTestStart = false;
			this.flapsOverIOs.ValveTestIsRunning = false;
		}

		// Token: 0x06000778 RID: 1912 RVA: 0x00030E54 File Offset: 0x0002F054
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x06000779 RID: 1913 RVA: 0x00030E84 File Offset: 0x0002F084
		private void InitializeRejecters(ParameterHandling parHandling, DBConnection dbConnection, AccessRightsManager accessManager, MachineState machineState, MessageSystem messageSystem)
		{
			MachineType machineType = MachineType.MachineTypeUndefined;
			dbConnection.GetMachineType(ref machineType);
			MachineMode machineMode = MachineMode.Standard;
			dbConnection.GetMachineMode(ref machineMode);
			this.Rejecter1 = new Rejecter(parHandling, dbConnection, Rejecter.rejecter1MainPath, 1, accessManager, machineState, messageSystem);
			bool flag = machineMode == MachineMode.BulkObject;
			if (flag)
			{
				this.Rejecter1.BinFullTimeoutVisible = true;
				this.Rejecter1.BeltStopOnContaminantVisible = false;
				this.Rejecter1.VerifyVisible = true;
				this.Rejecter1.VerifyTestVisible = true;
				this.Rejecter1.TimeoutRejectVerifySensorBlockedVisible = true;
				this.Rejecter1.DebounceTimeVisible = false;
				this.Rejecter1.HighLevelSensors = false;
				this.Rejecter1.MSBinUnlockVisible = false;
				this.Rejecter1.MSBinUnlockTimeoutVisible = false;
				this.Rejecter1.AirPressureTestVisible = false;
			}
		}

		// Token: 0x040004FF RID: 1279
		private AccessRightsManager accessManager;

		// Token: 0x04000500 RID: 1280
		private ParameterHandling parHandling;

		// Token: 0x04000501 RID: 1281
		private NumKeyboard numKeyboard;

		// Token: 0x04000502 RID: 1282
		private bool isBulkFlaps;

		// Token: 0x04000503 RID: 1283
		private bool isBulkObjectFlaps;

		// Token: 0x04000504 RID: 1284
		private bool isBulkBinFullVisible;

		// Token: 0x04000505 RID: 1285
		private bool isBulkRejectVerifyVisible;

		// Token: 0x04000524 RID: 1316
		private DispatcherTimer timerValveTestIsRunning;
	}
}
