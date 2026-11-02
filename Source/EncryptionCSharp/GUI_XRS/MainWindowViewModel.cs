using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Common;
using DatabaseConnection;
using Definitions;
using GUI_XRS.Datalogger;
using SSTEthernet;
using WSFood;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x02000185 RID: 389
	public class MainWindowViewModel : INotifyPropertyChanged
	{
		// Token: 0x17000A06 RID: 2566
		// (get) Token: 0x06002302 RID: 8962 RVA: 0x000E1BF4 File Offset: 0x000DFDF4
		// (set) Token: 0x06002303 RID: 8963 RVA: 0x000E1BFC File Offset: 0x000DFDFC
		public ErrorSurvey errorSurvey { get; set; }

		// Token: 0x17000A07 RID: 2567
		// (get) Token: 0x06002304 RID: 8964 RVA: 0x000E1C05 File Offset: 0x000DFE05
		// (set) Token: 0x06002305 RID: 8965 RVA: 0x000E1C0D File Offset: 0x000DFE0D
		public User CurrentUser { get; set; }

		// Token: 0x17000A08 RID: 2568
		// (get) Token: 0x06002306 RID: 8966 RVA: 0x000E1C16 File Offset: 0x000DFE16
		// (set) Token: 0x06002307 RID: 8967 RVA: 0x000E1C1E File Offset: 0x000DFE1E
		public XrayState xrayState { get; set; }

		// Token: 0x17000A09 RID: 2569
		// (get) Token: 0x06002308 RID: 8968 RVA: 0x000E1C27 File Offset: 0x000DFE27
		// (set) Token: 0x06002309 RID: 8969 RVA: 0x000E1C2F File Offset: 0x000DFE2F
		public MachineState machineState { get; set; }

		// Token: 0x17000A0A RID: 2570
		// (get) Token: 0x0600230A RID: 8970 RVA: 0x000E1C38 File Offset: 0x000DFE38
		// (set) Token: 0x0600230B RID: 8971 RVA: 0x000E1C40 File Offset: 0x000DFE40
		public Logger logger { get; set; }

		// Token: 0x17000A0B RID: 2571
		// (get) Token: 0x0600230C RID: 8972 RVA: 0x000E1C4C File Offset: 0x000DFE4C
		// (set) Token: 0x0600230D RID: 8973 RVA: 0x000E1C64 File Offset: 0x000DFE64
		public Branding SelectedBranding
		{
			get
			{
				return this.selectedBranding;
			}
			set
			{
				this.selectedBranding = value;
				this.OnPropertyChanged("SelectedBranding");
			}
		}

		// Token: 0x17000A0C RID: 2572
		// (get) Token: 0x0600230E RID: 8974 RVA: 0x000E1C7C File Offset: 0x000DFE7C
		// (set) Token: 0x0600230F RID: 8975 RVA: 0x000E1C94 File Offset: 0x000DFE94
		public bool FooterMainWindowIsDeactivated
		{
			get
			{
				return this.footerMainWindowIsDeactivated;
			}
			set
			{
				this.footerMainWindowIsDeactivated = value;
				this.OnPropertyChanged("FooterMainWindowIsDeactivated");
			}
		}

		// Token: 0x17000A0D RID: 2573
		// (get) Token: 0x06002310 RID: 8976 RVA: 0x000E1CAC File Offset: 0x000DFEAC
		// (set) Token: 0x06002311 RID: 8977 RVA: 0x000E1CC4 File Offset: 0x000DFEC4
		public string Time
		{
			get
			{
				return this.time;
			}
			set
			{
				this.time = value;
				this.OnPropertyChanged("Time");
			}
		}

		// Token: 0x17000A0E RID: 2574
		// (get) Token: 0x06002312 RID: 8978 RVA: 0x000E1CDC File Offset: 0x000DFEDC
		// (set) Token: 0x06002313 RID: 8979 RVA: 0x000E1CF4 File Offset: 0x000DFEF4
		public bool FirstApplicationLoaded
		{
			get
			{
				return this.firstApplicationLoaded;
			}
			set
			{
				this.firstApplicationLoaded = value;
				this.OnPropertyChanged("FirstApplicationLoaded");
			}
		}

		// Token: 0x17000A0F RID: 2575
		// (get) Token: 0x06002314 RID: 8980 RVA: 0x000E1D0C File Offset: 0x000DFF0C
		// (set) Token: 0x06002315 RID: 8981 RVA: 0x000E1D24 File Offset: 0x000DFF24
		public bool StandardMode
		{
			get
			{
				return this.standardMode;
			}
			set
			{
				bool flag = this.standardMode != value;
				if (flag)
				{
					this.standardMode = value;
					this.OnPropertyChanged("StandardMode");
					this.OnPropertyChanged("BulkModeActive");
					bool flag2 = this.standardMode;
					if (flag2)
					{
						this.LearnOrXriImage = XrsImage.ImageConverter.ToBitmap("/img/learn.png");
						bool flag3 = this.valveActivity != null;
						if (flag3)
						{
							this.valveActivity.StopUpdateActivity();
						}
						bool flag4 = this.rejecterActivity != null;
						if (flag4)
						{
							this.rejecterActivity.StartUpdateActivity();
						}
					}
					else
					{
						this.LearnOrXriImage = XrsImage.ImageConverter.ToBitmap("/img/xriimage_big.png");
						bool flag5 = this.valveActivity != null;
						if (flag5)
						{
							this.valveActivity.StartUpdateActivity();
						}
						bool flag6 = this.rejecterActivity != null;
						if (flag6)
						{
							this.rejecterActivity.StartUpdateActivity();
						}
					}
				}
			}
		}

		// Token: 0x17000A10 RID: 2576
		// (get) Token: 0x06002316 RID: 8982 RVA: 0x000E1E04 File Offset: 0x000E0004
		public bool BulkModeActive
		{
			get
			{
				return !this.StandardMode;
			}
		}

		// Token: 0x17000A11 RID: 2577
		// (get) Token: 0x06002317 RID: 8983 RVA: 0x000E1E20 File Offset: 0x000E0020
		// (set) Token: 0x06002318 RID: 8984 RVA: 0x000E1E38 File Offset: 0x000E0038
		public bool BulkObjectModeActive
		{
			get
			{
				return this.bulkObjectModeActive;
			}
			set
			{
				this.bulkObjectModeActive = value;
				this.OnPropertyChanged("BulkObjectModeActive");
			}
		}

		// Token: 0x17000A12 RID: 2578
		// (get) Token: 0x06002319 RID: 8985 RVA: 0x000E1E50 File Offset: 0x000E0050
		// (set) Token: 0x0600231A RID: 8986 RVA: 0x000E1E68 File Offset: 0x000E0068
		public string BulkLiveModeSourcePath
		{
			get
			{
				return this.bulkLiveModeSourcePath;
			}
			set
			{
				bool flag = this.bulkLiveModeSourcePath != value;
				if (flag)
				{
					this.bulkLiveModeSourcePath = value;
					this.OnPropertyChanged("BulkLiveModeSourcePath");
				}
			}
		}

		// Token: 0x17000A13 RID: 2579
		// (get) Token: 0x0600231B RID: 8987 RVA: 0x000E1E9C File Offset: 0x000E009C
		// (set) Token: 0x0600231C RID: 8988 RVA: 0x000E1EB4 File Offset: 0x000E00B4
		public string BulkObjectModeSourcePath
		{
			get
			{
				return this.bulkObjectModeSourcePath;
			}
			set
			{
				bool flag = this.bulkObjectModeSourcePath != value;
				if (flag)
				{
					this.bulkObjectModeSourcePath = value;
					this.OnPropertyChanged("BulkObjectModeSourcePath");
				}
			}
		}

		// Token: 0x17000A14 RID: 2580
		// (get) Token: 0x0600231D RID: 8989 RVA: 0x000E1EE8 File Offset: 0x000E00E8
		// (set) Token: 0x0600231E RID: 8990 RVA: 0x000E1F00 File Offset: 0x000E0100
		public string BulkSwitchOnOffPath
		{
			get
			{
				return this.bulkSwitchOnOffPath;
			}
			set
			{
				bool flag = this.bulkSwitchOnOffPath != value;
				if (flag)
				{
					this.bulkSwitchOnOffPath = value;
					this.OnPropertyChanged("BulkSwitchOnOffPath");
				}
			}
		}

		// Token: 0x17000A15 RID: 2581
		// (get) Token: 0x0600231F RID: 8991 RVA: 0x000E1F34 File Offset: 0x000E0134
		// (set) Token: 0x06002320 RID: 8992 RVA: 0x000E1F4C File Offset: 0x000E014C
		public bool BulkOnOff
		{
			get
			{
				return this.bulkOnOff;
			}
			set
			{
				this.bulkOnOff = value;
				this.OnPropertyChanged("BulkOnOff");
			}
		}

		// Token: 0x17000A16 RID: 2582
		// (get) Token: 0x06002321 RID: 8993 RVA: 0x000E1F64 File Offset: 0x000E0164
		// (set) Token: 0x06002322 RID: 8994 RVA: 0x000E1F7C File Offset: 0x000E017C
		public bool BulkContImagesShown
		{
			get
			{
				return this.bulkContImagesShown;
			}
			set
			{
				this.bulkContImagesShown = value;
				this.OnPropertyChanged("BulkContImagesShown");
			}
		}

		// Token: 0x17000A17 RID: 2583
		// (get) Token: 0x06002323 RID: 8995 RVA: 0x000E1F92 File Offset: 0x000E0192
		// (set) Token: 0x06002324 RID: 8996 RVA: 0x000E1F9A File Offset: 0x000E019A
		public ValveActivity valveActivity { get; set; }

		// Token: 0x17000A18 RID: 2584
		// (get) Token: 0x06002325 RID: 8997 RVA: 0x000E1FA3 File Offset: 0x000E01A3
		// (set) Token: 0x06002326 RID: 8998 RVA: 0x000E1FAB File Offset: 0x000E01AB
		public RejecterActivity rejecterActivity { get; set; }

		// Token: 0x17000A19 RID: 2585
		// (get) Token: 0x06002327 RID: 8999 RVA: 0x000E1FB4 File Offset: 0x000E01B4
		// (set) Token: 0x06002328 RID: 9000 RVA: 0x000E1FBC File Offset: 0x000E01BC
		public LiveCalibration liveCalibration { get; set; }

		// Token: 0x17000A1A RID: 2586
		// (get) Token: 0x06002329 RID: 9001 RVA: 0x000E1FC5 File Offset: 0x000E01C5
		// (set) Token: 0x0600232A RID: 9002 RVA: 0x000E1FCD File Offset: 0x000E01CD
		public Vibro Vibro { get; set; }

		// Token: 0x17000A1B RID: 2587
		// (get) Token: 0x0600232B RID: 9003 RVA: 0x000E1FD6 File Offset: 0x000E01D6
		// (set) Token: 0x0600232C RID: 9004 RVA: 0x000E1FDE File Offset: 0x000E01DE
		public Sensitivity bulksensitivity { get; set; }

		// Token: 0x17000A1C RID: 2588
		// (get) Token: 0x0600232D RID: 9005 RVA: 0x000E1FE8 File Offset: 0x000E01E8
		// (set) Token: 0x0600232E RID: 9006 RVA: 0x000E2000 File Offset: 0x000E0200
		public bool SensitivityVisible
		{
			get
			{
				return this.sensitivityVisible;
			}
			set
			{
				bool flag = this.sensitivityVisible != value;
				if (flag)
				{
					this.sensitivityVisible = value;
					this.OnPropertyChanged("SensitivityVisible");
				}
			}
		}

		// Token: 0x17000A1D RID: 2589
		// (get) Token: 0x0600232F RID: 9007 RVA: 0x000E2034 File Offset: 0x000E0234
		// (set) Token: 0x06002330 RID: 9008 RVA: 0x000E204C File Offset: 0x000E024C
		public bool ContextUnlockBinVisibility
		{
			get
			{
				return this.contextUnlockBinVisibility;
			}
			set
			{
				bool flag = this.contextUnlockBinVisibility != value;
				if (flag)
				{
					this.contextUnlockBinVisibility = value;
					this.OnPropertyChanged("ContextUnlockBinVisibility");
				}
			}
		}

		// Token: 0x17000A1E RID: 2590
		// (get) Token: 0x06002331 RID: 9009 RVA: 0x000E2080 File Offset: 0x000E0280
		// (set) Token: 0x06002332 RID: 9010 RVA: 0x000E2098 File Offset: 0x000E0298
		public bool SensitivityEnabled
		{
			get
			{
				return this.sensitivityEnabled;
			}
			set
			{
				bool flag = this.sensitivityEnabled != value;
				if (flag)
				{
					this.sensitivityEnabled = value;
					this.OnPropertyChanged("SensitivityEnabled");
				}
			}
		}

		// Token: 0x17000A1F RID: 2591
		// (get) Token: 0x06002333 RID: 9011 RVA: 0x000E20CC File Offset: 0x000E02CC
		// (set) Token: 0x06002334 RID: 9012 RVA: 0x000E20E4 File Offset: 0x000E02E4
		public ImageSource XRayImage
		{
			get
			{
				return this.xRayImage;
			}
			set
			{
				bool flag = this.xRayImage != value;
				if (flag)
				{
					this.xRayImage = value;
					this.OnPropertyChanged("XRayImage");
				}
			}
		}

		// Token: 0x17000A20 RID: 2592
		// (get) Token: 0x06002335 RID: 9013 RVA: 0x000E2118 File Offset: 0x000E0318
		// (set) Token: 0x06002336 RID: 9014 RVA: 0x000E2130 File Offset: 0x000E0330
		public ImageSource LearnOrXriImage
		{
			get
			{
				return this.learnOrXriImage;
			}
			set
			{
				bool flag = this.learnOrXriImage != value;
				if (flag)
				{
					this.learnOrXriImage = value;
					this.OnPropertyChanged("LearnOrXriImage");
				}
			}
		}

		// Token: 0x17000A21 RID: 2593
		// (get) Token: 0x06002337 RID: 9015 RVA: 0x000E2164 File Offset: 0x000E0364
		// (set) Token: 0x06002338 RID: 9016 RVA: 0x000E217C File Offset: 0x000E037C
		public ImageSource XMessageSystemImage
		{
			get
			{
				return this.xMessageSystemImage;
			}
			set
			{
				bool flag = this.xMessageSystemImage != value;
				if (flag)
				{
					this.xMessageSystemImage = value;
					this.OnPropertyChanged("XMessageSystemImage");
				}
			}
		}

		// Token: 0x17000A22 RID: 2594
		// (get) Token: 0x06002339 RID: 9017 RVA: 0x000E21B0 File Offset: 0x000E03B0
		// (set) Token: 0x0600233A RID: 9018 RVA: 0x000E21C8 File Offset: 0x000E03C8
		public bool VibroStarted
		{
			get
			{
				return this.vibroStarted;
			}
			set
			{
				bool flag = this.vibroStarted != value;
				if (flag)
				{
					bool flag2 = this.machineState.Inspecting || !value;
					if (flag2)
					{
						this.vibroStarted = value;
						this.OnPropertyChanged("VibroStarted");
						this.OnPropertyChanged("VibroStopped");
					}
				}
			}
		}

		// Token: 0x17000A23 RID: 2595
		// (get) Token: 0x0600233B RID: 9019 RVA: 0x000E2224 File Offset: 0x000E0424
		public bool VibroStopped
		{
			get
			{
				return !this.vibroStarted;
			}
		}

		// Token: 0x17000A24 RID: 2596
		// (get) Token: 0x0600233C RID: 9020 RVA: 0x000E2240 File Offset: 0x000E0440
		// (set) Token: 0x0600233D RID: 9021 RVA: 0x000E2258 File Offset: 0x000E0458
		public MessageType WorstMessageType
		{
			get
			{
				return this.worstMessageType;
			}
			set
			{
				bool flag = this.worstMessageType != value;
				if (flag)
				{
					this.worstMessageType = value;
					this.OnPropertyChanged("WorstMessageType");
					this.SetXMessageSystemImage(this.worstMessageType);
				}
			}
		}

		// Token: 0x17000A25 RID: 2597
		// (get) Token: 0x0600233E RID: 9022 RVA: 0x000E2298 File Offset: 0x000E0498
		// (set) Token: 0x0600233F RID: 9023 RVA: 0x000E22B0 File Offset: 0x000E04B0
		public bool ResetMessageVisible
		{
			get
			{
				return this.resetMessagesVisible;
			}
			set
			{
				bool flag = this.resetMessagesVisible != value;
				if (flag)
				{
					bool flag2 = this.ErrorMessages.Count != 0;
					if (flag2)
					{
						this.resetMessagesVisible = true;
					}
					else
					{
						this.resetMessagesVisible = false;
					}
					this.OnPropertyChanged("ResetMessageVisible");
				}
			}
		}

		// Token: 0x17000A26 RID: 2598
		// (get) Token: 0x06002340 RID: 9024 RVA: 0x000E22FE File Offset: 0x000E04FE
		// (set) Token: 0x06002341 RID: 9025 RVA: 0x000E2306 File Offset: 0x000E0506
		public bool DebugVisible { get; set; }

		// Token: 0x17000A27 RID: 2599
		// (get) Token: 0x06002342 RID: 9026 RVA: 0x000E2310 File Offset: 0x000E0510
		// (set) Token: 0x06002343 RID: 9027 RVA: 0x000E2328 File Offset: 0x000E0528
		public bool MultiLane
		{
			get
			{
				return this.multiLane;
			}
			set
			{
				bool flag = this.multiLane != value;
				if (flag)
				{
					this.multiLane = value;
					this.OnPropertyChanged("MultiLane");
					this.OnPropertyChanged("SingleLane");
				}
			}
		}

		// Token: 0x17000A28 RID: 2600
		// (get) Token: 0x06002344 RID: 9028 RVA: 0x000E2368 File Offset: 0x000E0568
		public bool SingleLane
		{
			get
			{
				return !this.multiLane;
			}
		}

		// Token: 0x17000A29 RID: 2601
		// (get) Token: 0x06002345 RID: 9029 RVA: 0x000E2384 File Offset: 0x000E0584
		// (set) Token: 0x06002346 RID: 9030 RVA: 0x000E239C File Offset: 0x000E059C
		public LaneXri SelectedXriLane
		{
			get
			{
				return this.selectedXriLane;
			}
			set
			{
				bool flag = this.selectedXriLane != value;
				if (flag)
				{
					bool flag2 = this.selectedXriLane != null && this.productManager != null && this.productManager.CurrentLoadedProduct != null && this.productManager.CurrentLoadedProduct.IsRegionProduct && this.selectedXriLane.Image != null;
					if (flag2)
					{
						this.ViewRegionImage(value);
					}
					this.selectedXriLane = value;
					this.OnPropertyChanged("SelectedXriLane");
				}
			}
		}

		// Token: 0x17000A2A RID: 2602
		// (get) Token: 0x06002347 RID: 9031 RVA: 0x000E241D File Offset: 0x000E061D
		// (set) Token: 0x06002348 RID: 9032 RVA: 0x000E2425 File Offset: 0x000E0625
		public WindowState WState { get; set; }

		// Token: 0x17000A2B RID: 2603
		// (get) Token: 0x06002349 RID: 9033 RVA: 0x000E2430 File Offset: 0x000E0630
		// (set) Token: 0x0600234A RID: 9034 RVA: 0x0003C95F File Offset: 0x0003AB5F
		public double WindowWidth
		{
			get
			{
				return Definition.windowWidth;
			}
			set
			{
				Definition.windowWidth = value;
			}
		}

		// Token: 0x17000A2C RID: 2604
		// (get) Token: 0x0600234B RID: 9035 RVA: 0x000E2448 File Offset: 0x000E0648
		// (set) Token: 0x0600234C RID: 9036 RVA: 0x0003C97F File Offset: 0x0003AB7F
		public double WindowHeight
		{
			get
			{
				return Definition.windowHeight;
			}
			set
			{
				Definition.windowHeight = value;
			}
		}

		// Token: 0x17000A2D RID: 2605
		// (get) Token: 0x0600234D RID: 9037 RVA: 0x000E2460 File Offset: 0x000E0660
		// (set) Token: 0x0600234E RID: 9038 RVA: 0x0003C99F File Offset: 0x0003AB9F
		public double WLeft
		{
			get
			{
				return Definition.windowLeft;
			}
			set
			{
				Definition.windowLeft = value;
			}
		}

		// Token: 0x17000A2E RID: 2606
		// (get) Token: 0x0600234F RID: 9039 RVA: 0x000E2478 File Offset: 0x000E0678
		// (set) Token: 0x06002350 RID: 9040 RVA: 0x0003C9BF File Offset: 0x0003ABBF
		public double WTop
		{
			get
			{
				return Definition.windowTop;
			}
			set
			{
				Definition.windowTop = value;
			}
		}

		// Token: 0x17000A2F RID: 2607
		// (get) Token: 0x06002351 RID: 9041 RVA: 0x000E2490 File Offset: 0x000E0690
		// (set) Token: 0x06002352 RID: 9042 RVA: 0x000E24A8 File Offset: 0x000E06A8
		public Thickness TabBorderThickness
		{
			get
			{
				return this.tabBorderThickness;
			}
			set
			{
				this.tabBorderThickness = value;
				this.OnPropertyChanged("TabBorderThickness");
			}
		}

		// Token: 0x17000A30 RID: 2608
		// (get) Token: 0x06002353 RID: 9043 RVA: 0x000E24C0 File Offset: 0x000E06C0
		// (set) Token: 0x06002354 RID: 9044 RVA: 0x000E24D8 File Offset: 0x000E06D8
		public ProductManager ProductManager
		{
			get
			{
				return this.productManager;
			}
			set
			{
				this.productManager = value;
				this.OnPropertyChanged("ProductManager");
			}
		}

		// Token: 0x17000A31 RID: 2609
		// (get) Token: 0x06002355 RID: 9045 RVA: 0x000E24F0 File Offset: 0x000E06F0
		// (set) Token: 0x06002356 RID: 9046 RVA: 0x000E2508 File Offset: 0x000E0708
		public bool DoorLockAvailable
		{
			get
			{
				return this.doorLockAvailable;
			}
			set
			{
				this.doorLockAvailable = value;
				this.OnPropertyChanged("DoorLockAvailable");
			}
		}

		// Token: 0x17000A32 RID: 2610
		// (get) Token: 0x06002357 RID: 9047 RVA: 0x000E2520 File Offset: 0x000E0720
		// (set) Token: 0x06002358 RID: 9048 RVA: 0x000E2538 File Offset: 0x000E0738
		public bool StartXRayNoUser
		{
			get
			{
				return this.startXRayNoUser;
			}
			set
			{
				bool flag = value != this.startXRayNoUser;
				if (flag)
				{
					this.startXRayNoUser = value;
					this.OnPropertyChanged("StartXRayNoUser");
				}
			}
		}

		// Token: 0x17000A33 RID: 2611
		// (get) Token: 0x06002359 RID: 9049 RVA: 0x000E256C File Offset: 0x000E076C
		// (set) Token: 0x0600235A RID: 9050 RVA: 0x000E2584 File Offset: 0x000E0784
		public bool MultiLaneView
		{
			get
			{
				return this.multiLaneView;
			}
			set
			{
				this.multiLaneView = value;
				this.OnPropertyChanged("MultiLaneView");
			}
		}

		// Token: 0x17000A34 RID: 2612
		// (get) Token: 0x0600235B RID: 9051 RVA: 0x000E259C File Offset: 0x000E079C
		public WifiClient WifiClient
		{
			get
			{
				return this.wifiClient;
			}
		}

		// Token: 0x17000A35 RID: 2613
		// (get) Token: 0x0600235C RID: 9052 RVA: 0x000E25B4 File Offset: 0x000E07B4
		public Machine Machine
		{
			get
			{
				return Machine.Instance;
			}
		}

		// Token: 0x17000A36 RID: 2614
		// (get) Token: 0x0600235D RID: 9053 RVA: 0x000E25CC File Offset: 0x000E07CC
		public HomeManager HomeManager
		{
			get
			{
				return HomeManager.Instance;
			}
		}

		// Token: 0x17000A37 RID: 2615
		// (get) Token: 0x0600235E RID: 9054 RVA: 0x000E25E3 File Offset: 0x000E07E3
		// (set) Token: 0x0600235F RID: 9055 RVA: 0x000E25EB File Offset: 0x000E07EB
		public ObservableCollection<LaneXri> ShownLanes { get; set; }

		// Token: 0x17000A38 RID: 2616
		// (get) Token: 0x06002360 RID: 9056 RVA: 0x000E25F4 File Offset: 0x000E07F4
		// (set) Token: 0x06002361 RID: 9057 RVA: 0x000E25FC File Offset: 0x000E07FC
		public List<LaneXri> AllLanes { get; set; }

		// Token: 0x17000A39 RID: 2617
		// (get) Token: 0x06002362 RID: 9058 RVA: 0x000E2605 File Offset: 0x000E0805
		// (set) Token: 0x06002363 RID: 9059 RVA: 0x000E260D File Offset: 0x000E080D
		public List<LaneXri> AllSubProducts { get; set; }

		// Token: 0x17000A3A RID: 2618
		// (get) Token: 0x06002364 RID: 9060 RVA: 0x000E2616 File Offset: 0x000E0816
		// (set) Token: 0x06002365 RID: 9061 RVA: 0x000E261E File Offset: 0x000E081E
		public ObservableCollection<Message> ErrorMessages { get; set; }

		// Token: 0x17000A3B RID: 2619
		// (get) Token: 0x06002366 RID: 9062 RVA: 0x000E2627 File Offset: 0x000E0827
		// (set) Token: 0x06002367 RID: 9063 RVA: 0x000E262F File Offset: 0x000E082F
		public ObservableCollection<RejecterItem> UnlockBinItem { get; set; }

		// Token: 0x140000BE RID: 190
		// (add) Token: 0x06002368 RID: 9064 RVA: 0x000E2638 File Offset: 0x000E0838
		// (remove) Token: 0x06002369 RID: 9065 RVA: 0x000E2670 File Offset: 0x000E0870
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x17000A3C RID: 2620
		// (get) Token: 0x0600236A RID: 9066 RVA: 0x000E26A5 File Offset: 0x000E08A5
		// (set) Token: 0x0600236B RID: 9067 RVA: 0x000E26AD File Offset: 0x000E08AD
		public BoundLocalizedText StrStartVibro { get; set; }

		// Token: 0x17000A3D RID: 2621
		// (get) Token: 0x0600236C RID: 9068 RVA: 0x000E26B6 File Offset: 0x000E08B6
		// (set) Token: 0x0600236D RID: 9069 RVA: 0x000E26BE File Offset: 0x000E08BE
		public BoundLocalizedText StrStopVibro { get; set; }

		// Token: 0x17000A3E RID: 2622
		// (get) Token: 0x0600236E RID: 9070 RVA: 0x000E26C7 File Offset: 0x000E08C7
		// (set) Token: 0x0600236F RID: 9071 RVA: 0x000E26CF File Offset: 0x000E08CF
		public BoundLocalizedText StrValveActivity { get; set; }

		// Token: 0x17000A3F RID: 2623
		// (get) Token: 0x06002370 RID: 9072 RVA: 0x000E26D8 File Offset: 0x000E08D8
		// (set) Token: 0x06002371 RID: 9073 RVA: 0x000E26E0 File Offset: 0x000E08E0
		public BoundLocalizedText StrSensitivityPercent { get; set; }

		// Token: 0x17000A40 RID: 2624
		// (get) Token: 0x06002372 RID: 9074 RVA: 0x000E26E9 File Offset: 0x000E08E9
		// (set) Token: 0x06002373 RID: 9075 RVA: 0x000E26F1 File Offset: 0x000E08F1
		public BoundLocalizedText StrVibroSpeed { get; set; }

		// Token: 0x17000A41 RID: 2625
		// (get) Token: 0x06002374 RID: 9076 RVA: 0x000E26FA File Offset: 0x000E08FA
		// (set) Token: 0x06002375 RID: 9077 RVA: 0x000E2702 File Offset: 0x000E0902
		public BoundLocalizedText StrStartXray { get; set; }

		// Token: 0x17000A42 RID: 2626
		// (get) Token: 0x06002376 RID: 9078 RVA: 0x000E270B File Offset: 0x000E090B
		// (set) Token: 0x06002377 RID: 9079 RVA: 0x000E2713 File Offset: 0x000E0913
		public BoundLocalizedText StrSensTestcardSim { get; set; }

		// Token: 0x17000A43 RID: 2627
		// (get) Token: 0x06002378 RID: 9080 RVA: 0x000E271C File Offset: 0x000E091C
		// (set) Token: 0x06002379 RID: 9081 RVA: 0x000E2724 File Offset: 0x000E0924
		public BoundLocalizedText StrVATestcardSim { get; set; }

		// Token: 0x17000A44 RID: 2628
		// (get) Token: 0x0600237A RID: 9082 RVA: 0x000E272D File Offset: 0x000E092D
		// (set) Token: 0x0600237B RID: 9083 RVA: 0x000E2735 File Offset: 0x000E0935
		public BoundLocalizedText StrGlassTestcardSim { get; set; }

		// Token: 0x17000A45 RID: 2629
		// (get) Token: 0x0600237C RID: 9084 RVA: 0x000E273E File Offset: 0x000E093E
		// (set) Token: 0x0600237D RID: 9085 RVA: 0x000E2746 File Offset: 0x000E0946
		public BoundLocalizedText StrComplTestcardSim { get; set; }

		// Token: 0x17000A46 RID: 2630
		// (get) Token: 0x0600237E RID: 9086 RVA: 0x000E274F File Offset: 0x000E094F
		// (set) Token: 0x0600237F RID: 9087 RVA: 0x000E2757 File Offset: 0x000E0957
		public BoundLocalizedText StrActTestcardSim { get; set; }

		// Token: 0x17000A47 RID: 2631
		// (get) Token: 0x06002380 RID: 9088 RVA: 0x000E2760 File Offset: 0x000E0960
		// (set) Token: 0x06002381 RID: 9089 RVA: 0x000E2768 File Offset: 0x000E0968
		public BoundLocalizedText StrCompliance { get; set; }

		// Token: 0x17000A48 RID: 2632
		// (get) Token: 0x06002382 RID: 9090 RVA: 0x000E2771 File Offset: 0x000E0971
		// (set) Token: 0x06002383 RID: 9091 RVA: 0x000E2779 File Offset: 0x000E0979
		public BoundLocalizedText StrNoCompliance { get; set; }

		// Token: 0x17000A49 RID: 2633
		// (get) Token: 0x06002384 RID: 9092 RVA: 0x000E2782 File Offset: 0x000E0982
		// (set) Token: 0x06002385 RID: 9093 RVA: 0x000E278A File Offset: 0x000E098A
		public BoundLocalizedText StrRejectBulk { get; set; }

		// Token: 0x17000A4A RID: 2634
		// (get) Token: 0x06002386 RID: 9094 RVA: 0x000E2793 File Offset: 0x000E0993
		// (set) Token: 0x06002387 RID: 9095 RVA: 0x000E279B File Offset: 0x000E099B
		public RelayCommand ChangeUserCommand { get; set; }

		// Token: 0x17000A4B RID: 2635
		// (get) Token: 0x06002388 RID: 9096 RVA: 0x000E27A4 File Offset: 0x000E09A4
		// (set) Token: 0x06002389 RID: 9097 RVA: 0x000E27AC File Offset: 0x000E09AC
		public RelayCommand AutolearnCommand { get; set; }

		// Token: 0x17000A4C RID: 2636
		// (get) Token: 0x0600238A RID: 9098 RVA: 0x000E27B5 File Offset: 0x000E09B5
		// (set) Token: 0x0600238B RID: 9099 RVA: 0x000E27BD File Offset: 0x000E09BD
		public RelayCommand BulkXRIStartLiveModeCommand { get; set; }

		// Token: 0x17000A4D RID: 2637
		// (get) Token: 0x0600238C RID: 9100 RVA: 0x000E27C6 File Offset: 0x000E09C6
		// (set) Token: 0x0600238D RID: 9101 RVA: 0x000E27CE File Offset: 0x000E09CE
		public RelayCommand BulkXRIStartObjectModeCommand { get; set; }

		// Token: 0x17000A4E RID: 2638
		// (get) Token: 0x0600238E RID: 9102 RVA: 0x000E27D7 File Offset: 0x000E09D7
		// (set) Token: 0x0600238F RID: 9103 RVA: 0x000E27DF File Offset: 0x000E09DF
		public RelayCommand BulkRetrainCommand { get; set; }

		// Token: 0x17000A4F RID: 2639
		// (get) Token: 0x06002390 RID: 9104 RVA: 0x000E27E8 File Offset: 0x000E09E8
		// (set) Token: 0x06002391 RID: 9105 RVA: 0x000E27F0 File Offset: 0x000E09F0
		public RelayCommand BulkSwitchSortingOffComand { get; set; }

		// Token: 0x17000A50 RID: 2640
		// (get) Token: 0x06002392 RID: 9106 RVA: 0x000E27F9 File Offset: 0x000E09F9
		// (set) Token: 0x06002393 RID: 9107 RVA: 0x000E2801 File Offset: 0x000E0A01
		public RelayCommand ProductChangeCommand { get; set; }

		// Token: 0x17000A51 RID: 2641
		// (get) Token: 0x06002394 RID: 9108 RVA: 0x000E280A File Offset: 0x000E0A0A
		// (set) Token: 0x06002395 RID: 9109 RVA: 0x000E2812 File Offset: 0x000E0A12
		public RelayCommand ShowSubmenuCommand { get; set; }

		// Token: 0x17000A52 RID: 2642
		// (get) Token: 0x06002396 RID: 9110 RVA: 0x000E281B File Offset: 0x000E0A1B
		// (set) Token: 0x06002397 RID: 9111 RVA: 0x000E2823 File Offset: 0x000E0A23
		public RelayCommand ShowLogbookCommand { get; set; }

		// Token: 0x17000A53 RID: 2643
		// (get) Token: 0x06002398 RID: 9112 RVA: 0x000E282C File Offset: 0x000E0A2C
		// (set) Token: 0x06002399 RID: 9113 RVA: 0x000E2834 File Offset: 0x000E0A34
		public RelayCommand ShowAIStatusCommand { get; set; }

		// Token: 0x17000A54 RID: 2644
		// (get) Token: 0x0600239A RID: 9114 RVA: 0x000E283D File Offset: 0x000E0A3D
		// (set) Token: 0x0600239B RID: 9115 RVA: 0x000E2845 File Offset: 0x000E0A45
		public RelayCommand SignalUserActivityCommand { get; set; }

		// Token: 0x17000A55 RID: 2645
		// (get) Token: 0x0600239C RID: 9116 RVA: 0x000E284E File Offset: 0x000E0A4E
		// (set) Token: 0x0600239D RID: 9117 RVA: 0x000E2856 File Offset: 0x000E0A56
		public RelayCommand CloseCommand { get; set; }

		// Token: 0x17000A56 RID: 2646
		// (get) Token: 0x0600239E RID: 9118 RVA: 0x000E285F File Offset: 0x000E0A5F
		// (set) Token: 0x0600239F RID: 9119 RVA: 0x000E2867 File Offset: 0x000E0A67
		public RelayCommand ChangeTimeCommand { get; set; }

		// Token: 0x17000A57 RID: 2647
		// (get) Token: 0x060023A0 RID: 9120 RVA: 0x000E2870 File Offset: 0x000E0A70
		// (set) Token: 0x060023A1 RID: 9121 RVA: 0x000E2878 File Offset: 0x000E0A78
		public RelayCommand DecreaseSensitivityCommand { get; set; }

		// Token: 0x17000A58 RID: 2648
		// (get) Token: 0x060023A2 RID: 9122 RVA: 0x000E2881 File Offset: 0x000E0A81
		// (set) Token: 0x060023A3 RID: 9123 RVA: 0x000E2889 File Offset: 0x000E0A89
		public RelayCommand IncreaseSensitivityCommand { get; set; }

		// Token: 0x17000A59 RID: 2649
		// (get) Token: 0x060023A4 RID: 9124 RVA: 0x000E2892 File Offset: 0x000E0A92
		// (set) Token: 0x060023A5 RID: 9125 RVA: 0x000E289A File Offset: 0x000E0A9A
		public RelayCommand ChangeSensitivityCommand { get; set; }

		// Token: 0x17000A5A RID: 2650
		// (get) Token: 0x060023A6 RID: 9126 RVA: 0x000E28A3 File Offset: 0x000E0AA3
		// (set) Token: 0x060023A7 RID: 9127 RVA: 0x000E28AB File Offset: 0x000E0AAB
		public RelayCommand SliderChangedCommand { get; set; }

		// Token: 0x17000A5B RID: 2651
		// (get) Token: 0x060023A8 RID: 9128 RVA: 0x000E28B4 File Offset: 0x000E0AB4
		// (set) Token: 0x060023A9 RID: 9129 RVA: 0x000E28BC File Offset: 0x000E0ABC
		public RelayCommand ShowValveActivityCommand { get; set; }

		// Token: 0x17000A5C RID: 2652
		// (get) Token: 0x060023AA RID: 9130 RVA: 0x000E28C5 File Offset: 0x000E0AC5
		// (set) Token: 0x060023AB RID: 9131 RVA: 0x000E28CD File Offset: 0x000E0ACD
		public RelayCommand ChangeSpeedCommand { get; set; }

		// Token: 0x17000A5D RID: 2653
		// (get) Token: 0x060023AC RID: 9132 RVA: 0x000E28D6 File Offset: 0x000E0AD6
		// (set) Token: 0x060023AD RID: 9133 RVA: 0x000E28DE File Offset: 0x000E0ADE
		public RelayCommand LowerSpeedCommand { get; set; }

		// Token: 0x17000A5E RID: 2654
		// (get) Token: 0x060023AE RID: 9134 RVA: 0x000E28E7 File Offset: 0x000E0AE7
		// (set) Token: 0x060023AF RID: 9135 RVA: 0x000E28EF File Offset: 0x000E0AEF
		public RelayCommand HigherSpeedCommand { get; set; }

		// Token: 0x17000A5F RID: 2655
		// (get) Token: 0x060023B0 RID: 9136 RVA: 0x000E28F8 File Offset: 0x000E0AF8
		// (set) Token: 0x060023B1 RID: 9137 RVA: 0x000E2900 File Offset: 0x000E0B00
		public RelayCommand StartNewBatchCommand { get; set; }

		// Token: 0x17000A60 RID: 2656
		// (get) Token: 0x060023B2 RID: 9138 RVA: 0x000E2909 File Offset: 0x000E0B09
		// (set) Token: 0x060023B3 RID: 9139 RVA: 0x000E2911 File Offset: 0x000E0B11
		public RelayCommand UnlockBinCommand { get; set; }

		// Token: 0x17000A61 RID: 2657
		// (get) Token: 0x060023B4 RID: 9140 RVA: 0x000E291A File Offset: 0x000E0B1A
		// (set) Token: 0x060023B5 RID: 9141 RVA: 0x000E2922 File Offset: 0x000E0B22
		public RelayCommand DoBackupCommand { get; set; }

		// Token: 0x17000A62 RID: 2658
		// (get) Token: 0x060023B6 RID: 9142 RVA: 0x000E292B File Offset: 0x000E0B2B
		// (set) Token: 0x060023B7 RID: 9143 RVA: 0x000E2933 File Offset: 0x000E0B33
		public RelayCommand StartXrayCommand { get; set; }

		// Token: 0x17000A63 RID: 2659
		// (get) Token: 0x060023B8 RID: 9144 RVA: 0x000E293C File Offset: 0x000E0B3C
		// (set) Token: 0x060023B9 RID: 9145 RVA: 0x000E2944 File Offset: 0x000E0B44
		public RelayCommand ResetMessagesCommand { get; set; }

		// Token: 0x17000A64 RID: 2660
		// (get) Token: 0x060023BA RID: 9146 RVA: 0x000E294D File Offset: 0x000E0B4D
		// (set) Token: 0x060023BB RID: 9147 RVA: 0x000E2955 File Offset: 0x000E0B55
		public RelayCommand BulkDeactivateLiveModeCommand { get; set; }

		// Token: 0x17000A65 RID: 2661
		// (get) Token: 0x060023BC RID: 9148 RVA: 0x000E295E File Offset: 0x000E0B5E
		// (set) Token: 0x060023BD RID: 9149 RVA: 0x000E2966 File Offset: 0x000E0B66
		public RelayCommand HighSpeedImageCommand { get; set; }

		// Token: 0x17000A66 RID: 2662
		// (get) Token: 0x060023BE RID: 9150 RVA: 0x000E296F File Offset: 0x000E0B6F
		// (set) Token: 0x060023BF RID: 9151 RVA: 0x000E2977 File Offset: 0x000E0B77
		public ConveyorViewModel ConveyorViewModel { get; set; }

		// Token: 0x17000A67 RID: 2663
		// (get) Token: 0x060023C0 RID: 9152 RVA: 0x000E2980 File Offset: 0x000E0B80
		// (set) Token: 0x060023C1 RID: 9153 RVA: 0x000E2988 File Offset: 0x000E0B88
		public bool DemoMode { get; set; }

		// Token: 0x17000A68 RID: 2664
		// (get) Token: 0x060023C2 RID: 9154 RVA: 0x000E2994 File Offset: 0x000E0B94
		// (set) Token: 0x060023C3 RID: 9155 RVA: 0x000E29AC File Offset: 0x000E0BAC
		public bool TestcardSimVisible
		{
			get
			{
				return this.testcardSimVisible;
			}
			set
			{
				bool flag = this.testcardSimVisible != value;
				if (flag)
				{
					this.testcardSimVisible = value;
					this.OnPropertyChanged("TestcardSimVisible");
				}
			}
		}

		// Token: 0x17000A69 RID: 2665
		// (get) Token: 0x060023C4 RID: 9156 RVA: 0x000E29E0 File Offset: 0x000E0BE0
		// (set) Token: 0x060023C5 RID: 9157 RVA: 0x000E29F8 File Offset: 0x000E0BF8
		public string SimulatedGlassSensitivity
		{
			get
			{
				return this.simulatedGlassSensitivity;
			}
			set
			{
				bool flag = this.simulatedGlassSensitivity != value;
				if (flag)
				{
					this.simulatedGlassSensitivity = value;
					this.OnPropertyChanged("SimulatedGlassSensitivity");
				}
			}
		}

		// Token: 0x17000A6A RID: 2666
		// (get) Token: 0x060023C6 RID: 9158 RVA: 0x000E2A2C File Offset: 0x000E0C2C
		// (set) Token: 0x060023C7 RID: 9159 RVA: 0x000E2A44 File Offset: 0x000E0C44
		public string SimulatedVASensitivity
		{
			get
			{
				return this.simulatedVASensitivity;
			}
			set
			{
				bool flag = this.simulatedVASensitivity != value;
				if (flag)
				{
					this.simulatedVASensitivity = value;
					this.OnPropertyChanged("SimulatedVASensitivity");
				}
			}
		}

		// Token: 0x17000A6B RID: 2667
		// (get) Token: 0x060023C8 RID: 9160 RVA: 0x000E2A78 File Offset: 0x000E0C78
		// (set) Token: 0x060023C9 RID: 9161 RVA: 0x000E2A90 File Offset: 0x000E0C90
		public string ComplGlassSensitivity
		{
			get
			{
				return this.complGlassSensitivity;
			}
			set
			{
				bool flag = this.complGlassSensitivity != value;
				if (flag)
				{
					this.complGlassSensitivity = value;
					this.OnPropertyChanged("ComplGlassSensitivity");
				}
			}
		}

		// Token: 0x17000A6C RID: 2668
		// (get) Token: 0x060023CA RID: 9162 RVA: 0x000E2AC4 File Offset: 0x000E0CC4
		// (set) Token: 0x060023CB RID: 9163 RVA: 0x000E2ADC File Offset: 0x000E0CDC
		public string ComplVASensitivity
		{
			get
			{
				return this.complVASensitivity;
			}
			set
			{
				bool flag = this.complVASensitivity != value;
				if (flag)
				{
					this.complVASensitivity = value;
					this.OnPropertyChanged("ComplVASensitivity");
				}
			}
		}

		// Token: 0x17000A6D RID: 2669
		// (get) Token: 0x060023CC RID: 9164 RVA: 0x000E2B10 File Offset: 0x000E0D10
		// (set) Token: 0x060023CD RID: 9165 RVA: 0x000E2B28 File Offset: 0x000E0D28
		public bool ComplSensitivityVisible
		{
			get
			{
				return this.complSensitivityVisible;
			}
			set
			{
				bool flag = this.complSensitivityVisible != value;
				if (flag)
				{
					this.complSensitivityVisible = value;
					this.OnPropertyChanged("ComplSensitivityVisible");
				}
			}
		}

		// Token: 0x17000A6E RID: 2670
		// (get) Token: 0x060023CE RID: 9166 RVA: 0x000E2B5C File Offset: 0x000E0D5C
		// (set) Token: 0x060023CF RID: 9167 RVA: 0x000E2B74 File Offset: 0x000E0D74
		public string SystemIsCompliant
		{
			get
			{
				return this.systemIsCompliant;
			}
			set
			{
				bool flag = this.systemIsCompliant != value;
				if (flag)
				{
					this.systemIsCompliant = value;
					this.OnPropertyChanged("SystemIsCompliant");
				}
			}
		}

		// Token: 0x17000A6F RID: 2671
		// (get) Token: 0x060023D0 RID: 9168 RVA: 0x000E2BA8 File Offset: 0x000E0DA8
		// (set) Token: 0x060023D1 RID: 9169 RVA: 0x000E2BC0 File Offset: 0x000E0DC0
		public bool ComplTextForeground
		{
			get
			{
				return this.complTextForeground;
			}
			set
			{
				bool flag = this.complTextForeground != value;
				if (flag)
				{
					this.complTextForeground = value;
					this.OnPropertyChanged("ComplTextForeground");
				}
			}
		}

		// Token: 0x17000A70 RID: 2672
		// (get) Token: 0x060023D2 RID: 9170 RVA: 0x000E2BF4 File Offset: 0x000E0DF4
		// (set) Token: 0x060023D3 RID: 9171 RVA: 0x000E2C0C File Offset: 0x000E0E0C
		public bool ComplTextVAForeground
		{
			get
			{
				return this.complTextVAForeground;
			}
			set
			{
				bool flag = this.complTextVAForeground != value;
				if (flag)
				{
					this.complTextVAForeground = value;
					this.OnPropertyChanged("ComplTextVAForeground");
				}
			}
		}

		// Token: 0x17000A71 RID: 2673
		// (get) Token: 0x060023D4 RID: 9172 RVA: 0x000E2C40 File Offset: 0x000E0E40
		// (set) Token: 0x060023D5 RID: 9173 RVA: 0x000E2C58 File Offset: 0x000E0E58
		public bool ComplTextGlassForeground
		{
			get
			{
				return this.complTextGlassForeground;
			}
			set
			{
				bool flag = this.complTextGlassForeground != value;
				if (flag)
				{
					this.complTextGlassForeground = value;
					this.OnPropertyChanged("ComplTextGlassForeground");
				}
			}
		}

		// Token: 0x17000A72 RID: 2674
		// (get) Token: 0x060023D6 RID: 9174 RVA: 0x000E2C8C File Offset: 0x000E0E8C
		// (set) Token: 0x060023D7 RID: 9175 RVA: 0x000E2CA4 File Offset: 0x000E0EA4
		public bool SliderVisibleBulkMode
		{
			get
			{
				return this.sliderVisibleBulkMode;
			}
			set
			{
				bool flag = this.sliderVisibleBulkMode != value;
				if (flag)
				{
					this.sliderVisibleBulkMode = value;
					this.OnPropertyChanged("SliderVisibleBulkMode");
				}
			}
		}

		// Token: 0x17000A73 RID: 2675
		// (get) Token: 0x060023D8 RID: 9176 RVA: 0x000E2CD8 File Offset: 0x000E0ED8
		// (set) Token: 0x060023D9 RID: 9177 RVA: 0x000E2CF0 File Offset: 0x000E0EF0
		public bool HighSpeedMachineActive
		{
			get
			{
				return this.highSpeedMachineActive;
			}
			set
			{
				bool flag = value != this.highSpeedMachineActive;
				if (flag)
				{
					this.highSpeedMachineActive = value;
					if (value)
					{
						bool flag2 = this.Widescreen;
						if (flag2)
						{
							this.HighSpeedMachineActiveVisible1 = true;
							this.HighSpeedMachineActiveVisible2 = false;
						}
						else
						{
							this.HighSpeedMachineActiveVisible1 = false;
							this.HighSpeedMachineActiveVisible2 = true;
						}
					}
					else
					{
						this.HighSpeedMachineActiveVisible1 = false;
						this.HighSpeedMachineActiveVisible2 = false;
					}
				}
			}
		}

		// Token: 0x17000A74 RID: 2676
		// (get) Token: 0x060023DA RID: 9178 RVA: 0x000E2D64 File Offset: 0x000E0F64
		// (set) Token: 0x060023DB RID: 9179 RVA: 0x000E2D7C File Offset: 0x000E0F7C
		public bool ShowNext
		{
			get
			{
				return this.showNext;
			}
			set
			{
				this.showNext = value;
				this.OnPropertyChanged("ShowNext");
			}
		}

		// Token: 0x17000A75 RID: 2677
		// (get) Token: 0x060023DC RID: 9180 RVA: 0x000E2D94 File Offset: 0x000E0F94
		// (set) Token: 0x060023DD RID: 9181 RVA: 0x000E2DAC File Offset: 0x000E0FAC
		public bool Widescreen
		{
			get
			{
				return this.widescreen;
			}
			set
			{
				this.widescreen = value;
				this.OnPropertyChanged("Widescreen");
			}
		}

		// Token: 0x17000A76 RID: 2678
		// (get) Token: 0x060023DE RID: 9182 RVA: 0x000E2DC4 File Offset: 0x000E0FC4
		// (set) Token: 0x060023DF RID: 9183 RVA: 0x000E2DDC File Offset: 0x000E0FDC
		public bool HighSpeedMachineActiveVisible2
		{
			get
			{
				return this.highSpeedMachineActiveVisible2;
			}
			set
			{
				bool flag = this.highSpeedMachineActiveVisible2 != value;
				if (flag)
				{
					this.highSpeedMachineActiveVisible2 = value;
					this.OnPropertyChanged("HighSpeedMachineActiveVisible2");
				}
			}
		}

		// Token: 0x17000A77 RID: 2679
		// (get) Token: 0x060023E0 RID: 9184 RVA: 0x000E2E10 File Offset: 0x000E1010
		// (set) Token: 0x060023E1 RID: 9185 RVA: 0x000E2E28 File Offset: 0x000E1028
		public bool HighSpeedMachineActiveVisible1
		{
			get
			{
				return this.highSpeedMachineActiveVisible1;
			}
			set
			{
				bool flag = this.HighSpeedMachineActiveVisible1 != value;
				if (flag)
				{
					this.highSpeedMachineActiveVisible1 = value;
					this.OnPropertyChanged("HighSpeedMachineActiveVisible1");
				}
			}
		}

		// Token: 0x140000BF RID: 191
		// (add) Token: 0x060023E2 RID: 9186 RVA: 0x000E2E5C File Offset: 0x000E105C
		// (remove) Token: 0x060023E3 RID: 9187 RVA: 0x000E2E94 File Offset: 0x000E1094
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action ThroughputIncreased;

		// Token: 0x140000C0 RID: 192
		// (add) Token: 0x060023E4 RID: 9188 RVA: 0x000E2ECC File Offset: 0x000E10CC
		// (remove) Token: 0x060023E5 RID: 9189 RVA: 0x000E2F04 File Offset: 0x000E1104
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action RejectIncreased;

		// Token: 0x17000A78 RID: 2680
		// (get) Token: 0x060023E6 RID: 9190 RVA: 0x000E2F39 File Offset: 0x000E1139
		// (set) Token: 0x060023E7 RID: 9191 RVA: 0x000E2F41 File Offset: 0x000E1141
		public bool PlotShown { get; set; }

		// Token: 0x17000A79 RID: 2681
		// (get) Token: 0x060023E8 RID: 9192 RVA: 0x000E2F4C File Offset: 0x000E114C
		// (set) Token: 0x060023E9 RID: 9193 RVA: 0x000E2F64 File Offset: 0x000E1164
		public bool IsImageBoxVisible
		{
			get
			{
				return this.isImageBoxVisible;
			}
			set
			{
				this.isImageBoxVisible = value;
				this.OnPropertyChanged("IsImageBoxVisible");
			}
		}

		// Token: 0x17000A7A RID: 2682
		// (get) Token: 0x060023EA RID: 9194 RVA: 0x000E2F7C File Offset: 0x000E117C
		// (set) Token: 0x060023EB RID: 9195 RVA: 0x000E2F94 File Offset: 0x000E1194
		public bool IsImageBoxVisibleBigImage
		{
			get
			{
				return this.isImageBoxVisibleBigImage;
			}
			set
			{
				this.isImageBoxVisibleBigImage = value;
				this.OnPropertyChanged("IsImageBoxVisibleBigImage");
			}
		}

		// Token: 0x17000A7B RID: 2683
		// (get) Token: 0x060023EC RID: 9196 RVA: 0x000E2FAC File Offset: 0x000E11AC
		// (set) Token: 0x060023ED RID: 9197 RVA: 0x000E2FC4 File Offset: 0x000E11C4
		public bool AutoLearnVisibleRefProd
		{
			get
			{
				return this.autoLearnVisibleRefProd;
			}
			set
			{
				this.autoLearnVisibleRefProd = value;
				this.OnPropertyChanged("AutoLearnVisibleRefProd");
			}
		}

		// Token: 0x17000A7C RID: 2684
		// (get) Token: 0x060023EE RID: 9198 RVA: 0x000E2FDC File Offset: 0x000E11DC
		// (set) Token: 0x060023EF RID: 9199 RVA: 0x000E2FF4 File Offset: 0x000E11F4
		public bool RetrainMode
		{
			get
			{
				return this.retrainMode;
			}
			set
			{
				this.retrainMode = value;
				bool flag = !this.retrainMode;
				if (flag)
				{
					this.AutoRetrainUndo = false;
				}
				this.OnPropertyChanged("RetrainMode");
			}
		}

		// Token: 0x17000A7D RID: 2685
		// (get) Token: 0x060023F0 RID: 9200 RVA: 0x000E302C File Offset: 0x000E122C
		// (set) Token: 0x060023F1 RID: 9201 RVA: 0x000E3044 File Offset: 0x000E1244
		public bool AutoRetrainUndo
		{
			get
			{
				return this.autoRetrainUndo;
			}
			set
			{
				this.autoRetrainUndo = value;
				this.OnPropertyChanged("AutoRetrainUndo");
			}
		}

		// Token: 0x17000A7E RID: 2686
		// (get) Token: 0x060023F2 RID: 9202 RVA: 0x000E305A File Offset: 0x000E125A
		// (set) Token: 0x060023F3 RID: 9203 RVA: 0x000E3062 File Offset: 0x000E1262
		public RelayCommand RetrainModeCommand { get; set; }

		// Token: 0x17000A7F RID: 2687
		// (get) Token: 0x060023F4 RID: 9204 RVA: 0x000E306B File Offset: 0x000E126B
		// (set) Token: 0x060023F5 RID: 9205 RVA: 0x000E3073 File Offset: 0x000E1273
		public RelayCommand UndoRetrainCommand { get; set; }

		// Token: 0x17000A80 RID: 2688
		// (get) Token: 0x060023F6 RID: 9206 RVA: 0x000E307C File Offset: 0x000E127C
		// (set) Token: 0x060023F7 RID: 9207 RVA: 0x000E3094 File Offset: 0x000E1294
		public bool AutoRetrainVisible
		{
			get
			{
				return this.autoRetrainVisible;
			}
			set
			{
				bool flag = this.autoRetrainVisible != value;
				if (flag)
				{
					this.autoRetrainVisible = value;
					if (value)
					{
						this.parHandling.WriteValue(Definition.autoRetrain, "1");
					}
					else
					{
						this.parHandling.WriteValue(Definition.autoRetrain, "0");
					}
					this.OnPropertyChanged("AutoRetrainVisible");
				}
			}
		}

		// Token: 0x060023F8 RID: 9208 RVA: 0x000E30FC File Offset: 0x000E12FC
		public MainWindowViewModel(MainWindow mainWindow)
		{
			this.localizationManager = MainWindow.localizationManager;
			this.mainWindow = mainWindow;
			this.Initialize();
			this.ChangeUserCommand = new RelayCommand(new Action<object>(this.ChangeUser));
			this.ProductChangeCommand = new RelayCommand(new Action<object>(this.ProductChange));
			this.AutolearnCommand = new RelayCommand(new Action<object>(this.Autolearn));
			this.ShowSubmenuCommand = new RelayCommand(new Action<object>(this.ShowSubmenu));
			this.ShowLogbookCommand = new RelayCommand(new Action<object>(this.ShowLogbook));
			this.SignalUserActivityCommand = new RelayCommand(new Action<object>(this.SignalActivity));
			this.CloseCommand = new RelayCommand(new Action<object>(this.Close));
			this.ChangeTimeCommand = new RelayCommand(new Action<object>(this.ChangeTime));
			this.ChangeSensitivityCommand = new RelayCommand(new Action<object>(this.ChangeSensitivity));
			this.SliderChangedCommand = new RelayCommand(new Action<object>(this.SliderChanged));
			this.ShowValveActivityCommand = new RelayCommand(new Action<object>(this.ShowValveActivity));
			this.ChangeSpeedCommand = new RelayCommand(new Action<object>(this.ChangeSpeed));
			this.LowerSpeedCommand = new RelayCommand(new Action<object>(this.LowerSpeed));
			this.HigherSpeedCommand = new RelayCommand(new Action<object>(this.HigherSpeed));
			this.StartNewBatchCommand = new RelayCommand(new Action<object>(this.StartNewBatch));
			this.UnlockBinCommand = new RelayCommand(new Action<object>(this.UnlockBin));
			this.DoBackupCommand = new RelayCommand(new Action<object>(this.DoBackup));
			this.StartXrayCommand = new RelayCommand(new Action<object>(this.StartXray));
			this.ResetMessagesCommand = new RelayCommand(new Action<object>(this.ResetMessages));
			this.BulkDeactivateLiveModeCommand = new RelayCommand(new Action<object>(this.BulkDeactivateLiveMode));
			this.BulkXRIStartLiveModeCommand = new RelayCommand(new Action<object>(this.StartBulkLiveXRIImage));
			this.BulkXRIStartObjectModeCommand = new RelayCommand(new Action<object>(this.StartBulkObjectXRIImage));
			this.BulkRetrainCommand = new RelayCommand(new Action<object>(this.StartRetrainLiveXRIImage));
			this.BulkSwitchSortingOffComand = new RelayCommand(new Action<object>(this.BulkSwitchSortingOnOff));
			this.HighSpeedImageCommand = new RelayCommand(new Action<object>(this.TakeHighSpeedImage));
			this.ShowAIStatusCommand = new RelayCommand(new Action<object>(this.ShowAIStatus));
			this.RetrainModeCommand = new RelayCommand(new Action<object>(this.RetrainModeOnOff));
			this.UndoRetrainCommand = new RelayCommand(new Action<object>(this.UndoLastRetrain));
			this.Time = DateTime.Now.ToString("ddd, dd. MMM - HH:mm");
			this.timerUpdateDateTime = new DispatcherTimer();
			this.timerUpdateDateTime.Tick += this.UpdateDateTime;
			this.timerUpdateDateTime.Interval = new TimeSpan(0, 0, 1);
			this.timerUpdateDateTime.Start();
			this.timerUpdateVibroStarted = new DispatcherTimer(new TimeSpan(0, 0, 2), DispatcherPriority.Background, new EventHandler(this.UpdateVibroStarted), Application.Current.Dispatcher);
			this.testcardsimservice.SetProperties();
			this.timerAnomalyDetection = new DispatcherTimer();
			this.timerAnomalyDetection.Tick += this.CheckAnomalyDetectionParameters;
			this.timerAnomalyDetection.Interval = new TimeSpan(0, 0, 1);
			this.timerAnomalyDetection.Start();
		}

		// Token: 0x060023F9 RID: 9209 RVA: 0x000E3628 File Offset: 0x000E1828
		private void Initialize()
		{
			string a = "";
			bool flag = false;
			DetectorType detectorType = DetectorType.DTNetDE;
			Definition.guiVersionBuild = new DateTime(2000, 1, 1).Add(new TimeSpan(864000000000L * (long)Assembly.GetExecutingAssembly().GetName().Version.Build + 20000000L * (long)Assembly.GetExecutingAssembly().GetName().Version.Revision)).ToString("dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture);
			this.XRayImage = XrsImage.ImageConverter.ToBitmap("/img/xray.png");
			this.LearnOrXriImage = XrsImage.ImageConverter.ToBitmap("/img/learn.png");
			this.bgWorkerPaintImage = new BackgroundWorker();
			this.bgWorkerPaintImage.DoWork += this.bgWorkerPaintImages_DoWork;
			this.imageArchiver.XRIImageSaved += this.imageArchiver_XRIImageSaved;
			this.dbConnection = new DBConnection();
			this.dbConnection.BatchEnded += this.dbConnection_BatchEnded;
			this.dbConnection.BatchStarted += this.dbConnection_BatchStarted;
			CursorsSettings.dbConnection = this.dbConnection;
			bool flag2 = File.Exists(Path.Combine(Backup.tempBackupPath, DBConnection.database));
			if (flag2)
			{
				File.Copy(Path.Combine(Backup.tempBackupPath, DBConnection.database), DBConnection.databasePath, true);
				Directory.Delete(Backup.tempBackupPath, true);
				foreach (string path in Directory.EnumerateFiles(Backup.localBackupFolder, "*.tmp"))
				{
					File.Delete(path);
				}
			}
			bool flag3 = false;
			bool flag4 = File.Exists(DBConnection.databasePath);
			if (flag4)
			{
				this.dbConnection.Connect(DBConnection.databasePath);
				this.dbConnection.GetAutoLogin(ref a);
				flag = (a == "1");
				bool flag5 = flag;
				if (flag5)
				{
					this.CurrentUser = new User(1, "Service", BuiltInPriviledges.Service, "S8554", false);
				}
				else
				{
					this.CurrentUser = new User();
					User.GetDefaultUserStats(this.CurrentUser);
				}
				this.logbook = new Logbook(this.dbConnection);
				LogbookManager.logbook = this.logbook;
				WeightUnit weightUnit = WeightUnit.Gramm;
				this.dbConnection.GetWeightUnit(ref weightUnit);
				XRSImageDefs.grammIsWeightUnit = (weightUnit == WeightUnit.Gramm);
				this.SetWindowSize();
				this.dbConnection.GetAnomalyDetectionEnabled(ref flag3);
				bool flag6 = !flag3;
				if (flag6)
				{
					this.dbConnection.GetObjectDetectionEnabled(ref flag3);
				}
				this.ShowBtnAIStatus = flag3;
			}
			else
			{
				this.WState = WindowState.Maximized;
				this.CurrentUser = new User();
				User.GetDefaultUserStats(this.CurrentUser);
			}
			bool flag7 = false;
			this.dbDatalogs = new DBDataLogs();
			this.dbConnection.GetDBDatalogsIsActive(ref flag7);
			bool flag8 = flag7;
			if (flag8)
			{
				bool flag9 = !File.Exists(DBDataLogs.databasePath);
				if (flag9)
				{
					this.dbDatalogs.CreateDatabase(DBDataLogs.databasePath);
				}
				bool flag10 = File.Exists(DBDataLogs.databasePath);
				if (flag10)
				{
					this.dbDatalogs.Connect(DBDataLogs.databasePath);
					this.dbDatalogs.InitTableNamesWithSerialNr();
					this.dbDatalogs.CheckForLimitsTables();
					this.writeValuesforBgMonitoring();
				}
			}
			LogbookManager.currentUser = this.CurrentUser;
			this.messageSystem = new MessageSystem(this.CurrentUser, this.dbConnection);
			this.messageSystem.MessageRemoved += this.messageSystem_MessageRemoved;
			this.messageSystem.MessageAdded += this.messageSystem_MessageAdded;
			this.ErrorMessages = this.messageSystem.Messages;
			MainWindowViewModel.accessRightsManager = new AccessRightsManager(this.CurrentUser, !flag, this.dbConnection, this.setFooterMainWindow);
			this.InitializeDesign();
			this.InitializeCommunication();
			this.pixelCorrection = new PixelCorrection(this.dbConnection, this.parHandling);
			this.meanPixelLogger = new MeanPixelLogger(this.dbConnection, this.parHandling, this.imageTransfer, this.dbDatalogs);
			this.backup = new Backup(this.dbConnection, this.parHandling, this.dbDatalogs);
			bool flag11 = flag3;
			if (flag11)
			{
				this.backup.MaxManualFiles = 4;
			}
			this.logger = new Logger(this.parHandling, this.dbConnection);
			this.logger.LogPeriod = 60000;
			this.liveCalibration = new LiveCalibration(this.dbConnection, this.parHandling, this.dbDatalogs);
			int num = 0;
			this.dbConnection.GetLiveCalibrationMode(ref num);
			bool flag12 = num > 0 && this.parHandling.ReadPar(Definition.filterMainPath + DualEnergy.enableSubPath) != "1" && this.parHandling.ReadPar(Definition.machineModePath) != "1";
			if (flag12)
			{
				this.dbConnection.DeleteDtCorrection(true, true, false);
			}
			this.xrayState = new XrayState(this.parHandling, this.dbConnection, this.meanPixelLogger, this.pixelCorrection, this.dbDatalogs, this.liveCalibration);
			bool flag13 = this.parHandling.ReadPar(Definition.machineModePath) == "1" || this.parHandling.ReadPar(Definition.machineModePath) == "2" || this.parHandling.ReadPar(Definition.machineModePath) == "4" || this.parHandling.ReadPar(Definition.filterMainPath + "/acquisition/AcquisitionMode/Value") == "1" || this.parHandling.ReadPar(Definition.filterMainPath + DualEnergy.enableSubPath) == "1";
			if (flag13)
			{
				this.parHandling.WriteValue(Definition.detectorMonitoringActive, "0");
			}
			string text = "";
			this.dbConnection.GetLanguage(ref text);
			this.localizationManager.LanguageChanged += this.localizationManager_LanguageChanged;
			bool flag14 = text != "" && text != "de-DE";
			if (flag14)
			{
				this.localizationManager.ChangeLanguage(new CultureInfo(text));
			}
			this.machineState = new MachineState(this.xrayState, this.parHandling, this.dbConnection);
			this.pixelCorrection.CurrentMachineState = this.machineState;
			this.errorHandling = new ErrorHandling(this.dbConnection, this.parHandling);
			this.dbConnection.GetDetectorType(ref detectorType);
			this.errorSurvey = new ErrorSurvey(this.parHandling, this.dbConnection, this.machineState, this.messageSystem, this.errorHandling, this.sstServer, this.xrayState, detectorType, this.dbDatalogs);
			this.restore = new Restore(this.dbConnection, this.parHandling, this.errorSurvey, this.dbDatalogs, MainWindowViewModel.accessRightsManager);
			this.restore.DatabaseRestored += this.LoadCurrentApplicationAfterRestore;
			this.restore.BeforeReboot += this.restore_BeforeReboot;
			this.restore.Rebooting += this.restore_Rebooting;
			this.machineState.StateChangedToInspecting += this.machineState_StateChangedToInspecting;
			this.valveActivity = new ValveActivity(this.parHandling, this.machineState, this.dbConnection, this.dbDatalogs, this.errorSurvey);
			this.rejecterActivity = new RejecterActivity(this.parHandling, this.dbConnection, this.dbDatalogs, this.machineState, this.errorSurvey);
			MachineMode machineMode = MachineMode.Standard;
			this.dbConnection.GetMachineMode(ref machineMode);
			this.StandardMode = (machineMode != MachineMode.Bulk);
			bool flag15 = machineMode == MachineMode.BulkObject;
			if (flag15)
			{
				this.BulkObjectModeActive = true;
				this.parHandling.WriteValue(FlapsOverIOs.bulkflapsEnableValvesDOPath, "0", false);
			}
			bool flag16 = this.dbConnection.Connection != null;
			if (flag16)
			{
				this.dbConnection.GetSerialNumber(ref this.deviceSerialNbr);
				this.conveyor = new Conveyor(this.dbConnection, this.parHandling, this.xrayState);
				this.ConveyorViewModel = new ConveyorViewModel(this.conveyor, MainWindowViewModel.accessRightsManager, this.dbConnection, this.machineState, false, this.dbDatalogs, this.parHandling);
				this.auditSettings = new AuditCheckSettings(this.dbConnection, this.parHandling);
				this.errorSurvey.AuditRequired = this.auditSettings.AuditRequired;
				this.auditSettings.AuditRequired.NextCheckElapsed += this.AuditRequired_NextCheckElapsed;
				this.auditSettings.TriggerCheckIfNeeded();
				this.productManager = new ProductManager(this.dbConnection, this.parHandling, this.imageTransfer, this.machineState, this.conveyor, this.auditSettings, MainWindowViewModel.accessRightsManager, this.imageArchiver, this.testcardsimservice, this.rejecterActivity);
				this.productManager.ProductChanged += this.prodChange_ProductChanged;
				this.productManager.SubProductAdded += this.productManager_SubProductModified;
				this.productManager.SubProductRemoved += this.productManager_SubProductModified;
				this.productManager.SubProductRenamed += this.productManager_SubProductModified;
				this.ProductManager.BatchChanged += this.ProductManager_BatchChanged;
				bool flag17 = this.dbConnection.GetDeviceAddress(ref this.deviceAddress);
				if (flag17)
				{
					this.sstServer.DeviceAddress = this.deviceAddress;
				}
				this.dbConnection.GetThroughputIndexGlobal(ref this.throughputGlobal);
				this.dbConnection.GetRejectIndexGlobal(ref this.rejectGlobal);
			}
			this.dbConnection.ProductParameterChanged += this.dbConnection_ProductParameterChanged;
			this.dbConnection.ProductParametersChanged += this.dbConnection_ProductParametersChanged;
			this.dbConnection.SystemParameterChanged += this.dbConnection_SystemParameterChanged;
			this.parHandling.ParameterChanged += this.parHandling_ParameterChanged;
			this.xrayState.XRayPowerOn += this.xrayState_XRayPowerOn;
			this.xrayState.XRayPowerOff += this.xrayState_XRayPowerOff;
			this.xrayState.CalibrationDone += this.xrayState_CalibrationDone;
			bool flag18 = File.Exists(DBConnection.databasePath);
			if (flag18)
			{
				this.xrayState.Start();
			}
			this.RegisterLocalizedText();
			this.CheckIfInDemoMode();
			DateTime dateTime = default(DateTime);
			bool lastOnTime = this.dbConnection.GetLastOnTime(ref dateTime);
			if (lastOnTime)
			{
				LogbookManager.AddEntry(dateTime, SSTEntryCode.MachineTurnedOff, 0, "", "", "", "", "");
			}
			LogbookManager.AddEntry(SSTEntryCode.MachineTurnedOn);
			this.timerWriteLastOnTime = new DispatcherTimer(new TimeSpan(0, 0, 30), DispatcherPriority.Normal, new EventHandler(this.WriteLastOnTime), Application.Current.Dispatcher);
			this.Vibro = new Vibro(this.dbConnection, this.parHandling);
			this.bulksensitivity = new Sensitivity(this.parHandling, this.dbConnection, this.productManager);
			this.SensitivityEnabled = (this.CurrentUser.Rights >= this.rightSensitivity);
			MainWindowViewModel.accessRightsManager.PropertiesSet += this.accessRightsManager_PropertiesSet;
			MainWindowViewModel.accessRightsManager.ResetRetrain += this.accessRightsManager_ResetRetrain;
			this.timerAddSensitivityChangeToLb = new System.Threading.Timer(new TimerCallback(this.AddSensitivityLbEntry));
			this.DebugVisible = Definition.guiVersion.Contains('_');
			this.testcardsimservice = new TestcardSimService(this.parHandling, this.imageTransfer, this.imageArchiver, this.dbConnection, this.productManager, this.errorSurvey);
			this.testcardsimservice.PropertiesSet += this.testcardsimservice_PropertiesSet;
			this.SimulatedVASensitivity = "n.a.";
			this.SimulatedGlassSensitivity = "n.a.";
			this.ComplVASensitivity = "n.a.";
			this.ComplGlassSensitivity = "n.a.";
			this.ComplSensitivityVisible = false;
			this.aiService = new AIService(this.parHandling, this.imageTransfer, this.imageArchiver, this.dbConnection, this.productManager, this.modelTransfer, this.errorSurvey);
			this.errorSurvey.AIService = this.aiService;
			this.setFooterMainWindow = new SetFooterMainWindow();
			this.setFooterMainWindow.PropertiesSetFooterDeactivation += this.activateFooterMainWindow_PropertiesSet;
			this.setFooterMainWindow.PropertiesUnSetFooterDeactivation += this.activateFooterMainWindow_PropertiesUnSet;
			this.FooterMainWindowIsDeactivated = false;
			this.oldValfooterMainWindowIsDeactivated = false;
			this.InitializeLanes();
			this.SetImageBoxAutoZoom();
			this.TryLoginToGo();
			Batch initialBatchCounter = null;
			this.dbConnection.GetCurrentBatch(ref initialBatchCounter);
			this.SetInitialBatchCounter(initialBatchCounter);
			this.parHandling.StartConnectionTest();
			this.parHandling.Connect();
			this.imageTransfer.StartConnect();
			this.imageTransfer.StartConnectionTest();
			bool flag19 = this.modelTransfer != null;
			if (flag19)
			{
				this.modelTransfer.StartConnect();
				this.modelTransfer.StartConnectionTest();
			}
			this.errorSurvey.ImageProcessingNetworkError += this.errorSurvey_ImageProcessingNetworkError;
			this.errorSurvey.StartSurvey();
			this.errorSurvey.StartCheckingXRayOperationgHours();
			MachineType machineType = MachineType.MachineTypeUndefined;
			this.dbConnection.GetMachineType(ref machineType);
			this.Machine.MachineType = machineType;
			this.dbConnection.GetDoorLockAvailable(ref this.doorLockAvailable);
			this.dbConnection.GetStartXRayNoUser(ref this.startXRayNoUser);
			this.ConvertCorrectionTables();
			Rfid.Instance.SetDbConnection(this.dbConnection);
			Rfid.Instance.Connect();
			Rfid.Instance.RfidDetected += this.LoginRfidUser;
			this.rejecter = new Rejecter(this.parHandling, this.dbConnection);
			this.UnlockBinItem = this.rejecter.UnlockBinItems;
			this.InitializeVideojetPrinter();
			this.communicationsList = new List<object>();
			this.communicationsList.Add(this.sstServer);
			this.communicationsList.Add(this.wsFoodServer);
			this.communicationsList.Add(this.comServer);
			this.communicationsList.Add(this.videojetPrinter);
			this.InitializeImageBoxHigherContrastForDarkImg(this.parHandling);
			string text2 = "";
			this.dbConnection.GetParameterValue(Definition.bulkShowContImages, ref text2);
			bool flag20 = text2 == "1";
			if (flag20)
			{
				this.BulkContImagesShown = true;
			}
			else
			{
				this.BulkContImagesShown = false;
			}
			this.BulkLiveModeSourcePath = "..\\img\\xriimageBulk.png";
			bool flag21 = this.BulkContImagesShown;
			if (flag21)
			{
				this.BulkObjectModeSourcePath = "..\\img\\xriimageBulkCont.png";
			}
			else
			{
				this.BulkObjectModeSourcePath = "..\\img\\xriimageBulkJustCont.png";
			}
			this.BulkSwitchOnOffPath = "..\\img\\rejectOFF.png";
			bool bulkModeActive = this.BulkModeActive;
			if (bulkModeActive)
			{
				int num2 = 0;
				int.TryParse(this.parHandling.ReadPar("/filter/comparer/comparer1/Contrast/Value"), out num2);
				this.SliderVisibleBulkMode = (this.CurrentUser.Rights >= BuiltInPriviledges.Operator1 && num2 < 10000);
			}
			else
			{
				this.SliderVisibleBulkMode = false;
			}
			this.dbDatalogsManager = new DBDataLogsManager(this.dbDatalogs, this.parHandling, this.machineState, this.errorSurvey, this.dbConnection);
			bool flag22 = flag7 && File.Exists(DBDataLogs.databasePath);
			if (flag22)
			{
				this.dbDatalogsManager.StartTimer();
			}
			bool flag23 = this.rejecterActivity != null;
			if (flag23)
			{
				this.rejecterActivity.StartUpdateActivity();
			}
			bool flag24 = this.parHandling.ReadPar(Definition.throughputCountPerMin) == "1";
			if (flag24)
			{
				List<LaneXri> list = (from lane in this.ShownLanes
				where lane.Lane == Lane.All
				select lane).ToList<LaneXri>();
				bool flag25 = list.Count > 0;
				if (flag25)
				{
					list[0].Counter.StartUpdateThroughputPerMin();
				}
			}
			this.ivpTimer = new IVPSettingsViewModel(this.dbConnection, this.parHandling, this.productManager);
			this.ivpTimer.IVPTimer.NextIVPElapsed += this.IVP_NextCheckElapsed;
			this.ivpTimer.IVPTimer.StartTimer();
			this.Widescreen = (Definition.windowWidth >= 1300.0);
			bool flag26 = false;
			this.dbConnection.GetHighThroughputMachine(ref flag26);
			this.dbConnection.GetParameterValue(Definition.highThroughputEnabled, ref text2);
			bool flag27 = flag26 && text2.Equals("1");
			if (flag27)
			{
				bool flag28 = this.machineState.State == MachineStates.Inspecting || this.DemoMode;
				if (flag28)
				{
					this.ShowNext = true;
				}
				this.HighSpeedMachineActive = true;
			}
			else
			{
				this.ShowNext = false;
				this.HighSpeedMachineActive = false;
			}
			bool flag29 = this.liveCalibration != null;
			if (flag29)
			{
				this.liveCalibration.InitErrorSurvey(this.errorSurvey);
			}
			bool flag30 = this.meanPixelLogger != null;
			if (flag30)
			{
				this.meanPixelLogger.InitErrorSurvey(this.errorSurvey);
			}
			bool flag31 = this.StandardMode;
			if (flag31)
			{
				this.IsImageBoxVisible = true;
				this.machineState.StartMessage.BulkOnOff = false;
			}
			else
			{
				this.IsImageBoxVisible = false;
				this.IsImageBoxVisibleBigImage = false;
				this.dbDatalogs.AddRejectOnOffTimes();
				int num3 = 0;
				this.dbConnection.GetNumValves(ref num3);
				this.dbDatalogs.AddSVTValves(num3);
				this.dbDatalogs.AddSVTValvesOperatingCounter(num3);
				this.machineState.StartMessage.BulkOnOff = true;
			}
			this.machineState.StartMessage.BulkSortingOnOff = this.BulkOnOff;
			bool flag32 = this.productManager != null;
			if (flag32)
			{
				bool flag33 = this.productManager.referenceProduct == null;
				if (flag33)
				{
					this.CreateReferenceProduct();
				}
			}
			List<string> list2 = new List<string>();
			this.dbConnection.GetComissioningList(ref list2);
			bool flag34 = list2.Count == 0;
			if (flag34)
			{
				list2.Add(Definition.guiVersion);
				this.dbConnection.RecreateCommissionTable(list2);
			}
			else
			{
				bool flag35 = list2.Count == 12;
				if (flag35)
				{
					list2.Insert(0, Definition.guiVersion);
					list2.Add("0");
					this.dbConnection.RecreateCommissionTable(list2);
				}
				else
				{
					bool flag36 = list2.Count == 13;
					if (flag36)
					{
						list2.Insert(0, Definition.guiVersion);
						this.dbConnection.RecreateCommissionTable(list2);
					}
					else
					{
						bool flag37 = list2.Count == 14 && list2[0] != Definition.guiVersion;
						if (flag37)
						{
							list2[0] = Definition.guiVersion;
							this.dbConnection.RecreateCommissionTable(list2);
						}
					}
				}
			}
			this.dbConnection.AddIVPUser();
			this.archiveQueuePngToNetworkFolder = new BlockingCollection<ItemPngToNetworkFolder>();
			string fileName = "";
			string fileNamePath = "";
			string fileNamePathFtp = "";
			string currentArchiveDirectoryNetwork = "";
			string currentArchiveDirectoryFtp = "";
			List<string> pathpieces = new List<string>();
			bool demoModeIsActive = false;
			this.dbConnection.GetDemoMode(ref demoModeIsActive);
			Task.Run(delegate()
			{
				foreach (ItemPngToNetworkFolder itemPngToNetworkFolder in this.archiveQueuePngToNetworkFolder.GetConsumingEnumerable())
				{
					try
					{
						int num4 = this.imageArchiver.ArchivePNGImageToNetWorkFolder(itemPngToNetworkFolder.Image, itemPngToNetworkFolder.RotateDirection, itemPngToNetworkFolder.MaxFiles, ref fileName, itemPngToNetworkFolder.ProductName, itemPngToNetworkFolder.ProductNumber, itemPngToNetworkFolder.NetworkPath, itemPngToNetworkFolder.Mode, ref currentArchiveDirectoryNetwork, ref currentArchiveDirectoryFtp, ref fileNamePath, ref fileNamePathFtp, itemPngToNetworkFolder.SerialNumber, itemPngToNetworkFolder.Is16BitResolution == "1", itemPngToNetworkFolder.BinningMode, itemPngToNetworkFolder.LocationNetworkMode, demoModeIsActive);
						bool flag40 = num4 != 999;
						if (flag40)
						{
							bool flag41 = num4 == 0;
							if (flag41)
							{
								bool flag42 = itemPngToNetworkFolder.LocationNetworkMode == "1" || itemPngToNetworkFolder.LocationNetworkMode == "3";
								if (flag42)
								{
									this.fileNamePathNetWorkSaving = fileNamePath;
									MainWindowViewModel.flagSaveImgToNetwork = true;
								}
								bool flag43 = itemPngToNetworkFolder.LocationNetworkMode == "2" || itemPngToNetworkFolder.LocationNetworkMode == "3";
								if (flag43)
								{
									this.fileNamePathFtpSaving = fileNamePathFtp;
									MainWindowViewModel.flagSaveImgToFtP = true;
								}
							}
							else
							{
								MainWindowViewModel.flagNotSaveImgToNetwork = true;
								bool flag44 = num4 == 1;
								if (flag44)
								{
									this.errorSurvey.slicePathIntoArrayForLogbook(currentArchiveDirectoryNetwork, ref pathpieces);
									this.errorSurvey.PostMessage(SSTEntryCode.ImageNotSaveToExternalStorageLoc, 0, fileName, pathpieces[0], pathpieces[1], pathpieces[2]);
								}
								else
								{
									bool flag45 = num4 == 2;
									if (flag45)
									{
										this.errorSurvey.slicePathIntoArrayForLogbook(currentArchiveDirectoryFtp, ref pathpieces);
										this.errorSurvey.PostMessage(SSTEntryCode.ImageNotSaveToExternalStorageLoc, 0, fileName, pathpieces[0], pathpieces[1], pathpieces[2]);
									}
									else
									{
										bool flag46 = num4 == 3;
										if (flag46)
										{
											this.errorSurvey.slicePathIntoArrayForLogbook(currentArchiveDirectoryNetwork, ref pathpieces);
											this.errorSurvey.PostMessage(SSTEntryCode.ImageNotSaveToExternalStorageLoc, 0, fileName, pathpieces[0], pathpieces[1], pathpieces[2]);
											this.errorSurvey.slicePathIntoArrayForLogbook(currentArchiveDirectoryFtp, ref pathpieces);
											this.errorSurvey.PostMessage(SSTEntryCode.ImageNotSaveToExternalStorageLoc, 0, fileName, pathpieces[0], pathpieces[1], pathpieces[2]);
										}
										else
										{
											bool flag47 = num4 == 99;
											if (flag47)
											{
												bool flag48 = Application.Current != null;
												if (flag48)
												{
													Application.Current.Dispatcher.Invoke(delegate()
													{
														string message = LocalizationManager.Localize("menuItemImageTransfer") + ": " + LocalizationManager.Localize("txtNoSerialNumber");
														Dialog.Show(LocalizationManager.Localize("txtWarning"), message, DialogStyle.Warning, DialogButtons.OK);
													});
												}
											}
										}
									}
								}
							}
						}
					}
					catch
					{
						bool flag49 = itemPngToNetworkFolder.LocationNetworkMode == "1" || itemPngToNetworkFolder.LocationNetworkMode == "3";
						if (flag49)
						{
							this.errorSurvey.slicePathIntoArrayForLogbook(itemPngToNetworkFolder.NetworkPath, ref pathpieces);
							this.errorSurvey.PostMessage(SSTEntryCode.NoConnectionToExternalStorageLoc, 0, pathpieces[0], pathpieces[1], pathpieces[2], "");
							MainWindowViewModel.flagNotSaveImgToNetwork = true;
						}
					}
				}
			});
			bool flag38 = false;
			this.dbConnection.GetAutoRetrain(ref flag38);
			bool flag39 = flag38;
			if (flag39)
			{
				this.AutoRetrainVisible = true;
			}
			else
			{
				this.AutoRetrainVisible = false;
			}
			new Thread(new ThreadStart(this.deleteFoldersForExternalImageSaving))
			{
				Name = "Saving images externally"
			}.Start();
		}

		// Token: 0x060023FA RID: 9210 RVA: 0x000E4A84 File Offset: 0x000E2C84
		private void deleteFoldersForExternalImageSaving()
		{
			string text = "";
			while (Application.Current != null)
			{
				this.counterForExternalImageSaving++;
				Thread.Sleep(2000);
				bool flag = this.counterForExternalImageSaving >= 450;
				if (flag)
				{
					this.counterForExternalImageSaving = 0;
					string locationNetworkSavingPng = this.dbConnection.GetLocationNetworkSavingPng();
					string networkPathSavingPng = this.dbConnection.GetNetworkPathSavingPng();
					string storageModeNetworkSavingPng = this.dbConnection.GetStorageModeNetworkSavingPng();
					double num = 0.0;
					List<string> list = new List<string>();
					string text2 = "";
					this.dbConnection.GetSerialNumber(ref text2);
					bool flag2 = text2 == "";
					if (flag2)
					{
						text2 = "0000";
					}
					bool flag3 = locationNetworkSavingPng == "1" || locationNetworkSavingPng == "3";
					if (flag3)
					{
						string text3 = networkPathSavingPng + text2 + "\\";
						DriveInfo[] drives = DriveInfo.GetDrives();
						bool flag4 = text == "";
						if (flag4)
						{
							for (int i = 0; i < drives.Length; i++)
							{
								bool flag5 = drives[i].VolumeLabel != "" && text3.Contains(drives[i].VolumeLabel);
								if (flag5)
								{
									text = drives[i].RootDirectory.FullName;
									break;
								}
							}
						}
						bool flag6 = text != "";
						if (flag6)
						{
							DriveInfo driveInfo = new DriveInfo(text);
							bool flag7 = driveInfo != null;
							if (flag7)
							{
								double num2 = Convert.ToDouble(driveInfo.TotalSize);
								double num3 = Convert.ToDouble(driveInfo.AvailableFreeSpace);
								bool flag8 = num2 > 0.0;
								if (flag8)
								{
									num = Math.Round((1.0 - num3 / num2) * 100.0, 1);
								}
								bool flag9 = num > 90.0;
								if (flag9)
								{
									this.errorSurvey.slicePathIntoArrayForLogbook(networkPathSavingPng, ref list);
									this.errorSurvey.PostMessage(SSTEntryCode.Limit90PercentExternalStorageLoc, 0, list[0], list[1], list[2], "");
								}
								else
								{
									bool flag10 = num > 99.0;
									if (flag10)
									{
										this.errorSurvey.slicePathIntoArrayForLogbook(networkPathSavingPng, ref list);
										this.errorSurvey.PostMessage(SSTEntryCode.Limit100PercentExternalStorageLoc, 0, list[0], list[1], list[2], "");
									}
								}
							}
						}
						bool flag11 = (storageModeNetworkSavingPng == "1" || storageModeNetworkSavingPng == "2") && !this.delteExternalFoldersisRunning;
						if (flag11)
						{
							this.delteExternalFoldersisRunning = true;
							bool flag12 = storageModeNetworkSavingPng == "1";
							if (flag12)
							{
								this.deleteExternalFoldersIfNecessary("1", text3, "");
							}
							else
							{
								bool flag13 = storageModeNetworkSavingPng == "2" && num > 90.0;
								if (flag13)
								{
									this.deleteExternalFoldersIfNecessary("2", text3, text);
								}
							}
							this.delteExternalFoldersisRunning = false;
						}
					}
					bool flag14 = locationNetworkSavingPng == "2" || locationNetworkSavingPng == "3";
					if (flag14)
					{
						string text4 = StoreImageData.archiveRoot + "\\Image\\" + text2 + "\\";
						bool flag15 = Directory.Exists(text4);
						if (flag15)
						{
							string[] directories = Directory.GetDirectories(text4, "*", SearchOption.TopDirectoryOnly);
							bool flag16 = directories != null && directories.Count<string>() > 0;
							if (flag16)
							{
								for (int j = 0; j < directories.Count<string>(); j++)
								{
									bool flag17 = !directories[j].Contains(DateTime.Today.ToString("yyyy-MM-dd"));
									if (flag17)
									{
										bool flag18 = Directory.Exists(directories[j]);
										if (flag18)
										{
											Directory.Delete(directories[j], true);
										}
									}
								}
							}
						}
						text4 = text4 + DateTime.Today.ToString("yyyy-MM-dd") + "\\";
						bool flag19 = Directory.Exists(text4);
						if (flag19)
						{
							string[] directories2 = Directory.GetDirectories(text4, "*", SearchOption.TopDirectoryOnly);
							bool flag20 = directories2 != null && directories2.Count<string>() > 0;
							if (flag20)
							{
								for (int k = 0; k < directories2.Count<string>(); k++)
								{
									bool flag21 = !directories2[k].Contains(this.productManager.CurrentLoadedProduct.ProductNumber + " " + this.productManager.CurrentLoadedProduct.ProductName);
									if (flag21)
									{
										bool flag22 = Directory.Exists(directories2[k]);
										if (flag22)
										{
											Directory.Delete(directories2[k], true);
										}
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x060023FB RID: 9211 RVA: 0x000E4F80 File Offset: 0x000E3180
		private void deleteExternalFoldersIfNecessary(string mode, string currentdirect, string networkdrive)
		{
			bool flag = Directory.Exists(currentdirect);
			if (flag)
			{
				List<DateTime> list = new List<DateTime>();
				string[] directories = Directory.GetDirectories(currentdirect, "*", SearchOption.TopDirectoryOnly);
				int num = Convert.ToInt32(this.dbConnection.GetModeTimePeriodNetworkSavingPng());
				double num2 = 0.0;
				for (int i = 0; i < directories.Length; i++)
				{
					DateTime item;
					bool flag2 = DateTime.TryParse(Path.GetFileName(directories[i]), out item);
					if (flag2)
					{
						list.Add(item);
					}
				}
				bool flag3 = list.Count > 0;
				if (flag3)
				{
					list.Sort();
					bool flag4 = mode == "2";
					if (flag4)
					{
						for (int j = 0; j < list.Count - 1; j++)
						{
							string path = currentdirect + list[j].ToString("yyyy-MM-dd");
							bool flag5 = Directory.Exists(path);
							if (flag5)
							{
								Directory.Delete(path, true);
							}
							DriveInfo driveInfo = new DriveInfo(networkdrive);
							double num3 = Convert.ToDouble(driveInfo.TotalSize);
							bool flag6 = num3 > 0.0;
							if (flag6)
							{
								num2 = Math.Round((1.0 - Convert.ToDouble(driveInfo.AvailableFreeSpace) / Convert.ToDouble(driveInfo.TotalSize)) * 100.0, 1);
							}
							bool flag7 = num2 < 85.0;
							if (flag7)
							{
								break;
							}
						}
					}
					bool flag8 = mode == "1" && num > 0 && directories.Length > num;
					if (flag8)
					{
						int num4 = directories.Length - num;
						for (int k = 0; k < num4; k++)
						{
							string path = currentdirect + list[k].ToString("yyyy-MM-dd");
							bool flag9 = Directory.Exists(path);
							if (flag9)
							{
								Directory.Delete(path, true);
							}
						}
					}
				}
			}
		}

		// Token: 0x060023FC RID: 9212 RVA: 0x000E518C File Offset: 0x000E338C
		private void CheckUserChanged(object sender, PropertyChangedEventArgs e)
		{
			bool flag = this.productManager.CurrentLoadedProduct.ProductNumber.Equals("-1");
			if (flag)
			{
				bool flag2 = MainWindowViewModel.accessRightsManager.CurrentUser.ID < 0;
				if (flag2)
				{
					this.AutoLearnVisibleRefProd = true;
				}
				else
				{
					this.AutoLearnVisibleRefProd = false;
				}
			}
			else
			{
				this.AutoLearnVisibleRefProd = true;
			}
		}

		// Token: 0x060023FD RID: 9213 RVA: 0x000E51F0 File Offset: 0x000E33F0
		private void IVP_NextCheckElapsed()
		{
			string text = "";
			this.dbConnection.GetIVPInfoEntry(DBIntegratedValidationProcess.ValidationDate, ref text);
			bool flag = text.Length == 10;
			if (flag)
			{
				int day = Convert.ToInt32(text.Substring(0, 2));
				int month = Convert.ToInt32(text.Substring(3, 2));
				int year = Convert.ToInt32(text.Substring(6, 4));
				DateTime d = new DateTime(year, month, day);
				double difference = (d - DateTime.Today).TotalDays;
				bool flag2 = difference < 14.0 && difference >= 0.0;
				if (flag2)
				{
					LogbookManager.AddEntry(SSTEntryCode.IVPNeeded, 0, year.ToString(), month.ToString(), day.ToString());
					bool flag3 = Application.Current != null;
					if (flag3)
					{
						Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(delegate()
						{
							Dialog.Show(LocalizationManager.Localize("txtInfo"), string.Concat(new string[]
							{
								LocalizationManager.Localize("txtValidationIsDue"),
								" ",
								((int)difference).ToString(),
								" ",
								LocalizationManager.Localize("txtDays")
							}), DialogStyle.Info, DialogButtons.OK);
						}));
					}
				}
				else
				{
					LogbookManager.AddEntry(SSTEntryCode.IVPNeeded, 0, year.ToString(), month.ToString(), day.ToString());
					bool flag4 = difference < 0.0;
					if (flag4)
					{
						bool flag5 = Application.Current != null;
						if (flag5)
						{
							Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(delegate()
							{
								Dialog.Show(LocalizationManager.Localize("txtError"), string.Concat(new string[]
								{
									LocalizationManager.Localize("txtValidationWasDue"),
									" ",
									(difference * -1.0).ToString(),
									" ",
									LocalizationManager.Localize("txtDays")
								}), DialogStyle.Error, DialogButtons.OK);
							}));
						}
					}
				}
			}
		}

		// Token: 0x060023FE RID: 9214 RVA: 0x000E5366 File Offset: 0x000E3566
		private void activateFooterMainWindow_PropertiesSet()
		{
			this.FooterMainWindowIsDeactivated = true;
		}

		// Token: 0x060023FF RID: 9215 RVA: 0x000E5371 File Offset: 0x000E3571
		private void activateFooterMainWindow_PropertiesUnSet()
		{
			this.FooterMainWindowIsDeactivated = false;
		}

		// Token: 0x06002400 RID: 9216 RVA: 0x000E537C File Offset: 0x000E357C
		private void testcardsimservice_PropertiesSet()
		{
			bool flag = this.parHandling != null && this.SimulatedVASensitivity != null && this.SimulatedGlassSensitivity != null && this.ComplVASensitivity != null && this.SimulatedGlassSensitivity != null && this.testcardsimservice != null && this.parHandling.ReadPar(Definition.testcardSimulationPath) == "1" && this.parHandling.ReadPar(Definition.autoKVAdjustmentPath) == "1";
			if (flag)
			{
				string text = this.parHandling.ReadPar(Definition.simulationActSensVA);
				string text2 = this.parHandling.ReadPar(Definition.simulationActSensGlass);
				string text3 = this.parHandling.ReadPar(Definition.simulationCompSensVA);
				string text4 = this.parHandling.ReadPar(Definition.simulationCompSensGlass);
				bool flag2 = this.testcardsimservice.CheckBallCountsBecauseOfNewVersion();
				int num = flag2 ? Convert.ToInt32(text) : (Convert.ToInt32(text) + 1);
				int num2 = flag2 ? Convert.ToInt32(text2) : (Convert.ToInt32(text2) + 1);
				int num3 = flag2 ? Convert.ToInt32(text3) : (Convert.ToInt32(text3) + 1);
				int num4 = flag2 ? Convert.ToInt32(text4) : (Convert.ToInt32(text4) + 1);
				this.TestcardSimVisible = (this.parHandling.ReadPar(Definition.simulationAfterAutolearn) == "1");
				bool flag3 = text != "" && text2 != "";
				if (flag3)
				{
					this.SimulatedVASensitivity = this.testcardsimservice.ConvertBallSizeToString("VA", num, true, true);
					this.SimulatedGlassSensitivity = this.testcardsimservice.ConvertBallSizeToString("Glass", num2, true, true);
				}
				string a = this.parHandling.ReadPar(Definition.simulationAfterTimeIntervall);
				int num5 = Convert.ToInt32(this.parHandling.ReadPar(Definition.timeIntervallForTestcardSim));
				this.ComplSensitivityVisible = (a == "1");
				bool flag4 = a == "1";
				if (flag4)
				{
					bool flag5 = text3 != "" && text4 != "";
					if (flag5)
					{
						this.ComplVASensitivity = this.testcardsimservice.ConvertBallSizeToString("VA", num3, true, true);
						this.ComplGlassSensitivity = this.testcardsimservice.ConvertBallSizeToString("Glass", num4, true, true);
					}
					this.ComplTextVAForeground = (num <= num3);
					this.ComplTextGlassForeground = (num2 <= num4);
					bool flag6 = !this.ComplTextVAForeground || !this.ComplTextGlassForeground;
					if (flag6)
					{
						this.SystemIsCompliant = this.StrNoCompliance.LocalizedText;
						this.ComplTextForeground = false;
					}
					else
					{
						this.SystemIsCompliant = this.StrCompliance.LocalizedText;
						this.ComplTextForeground = true;
					}
					bool flag7 = num5 != this.timerprevieousTestcardSim;
					if (flag7)
					{
						bool flag8 = this.timerTestcardSim != null;
						if (flag8)
						{
							this.timerTestcardSim.Stop();
						}
						this.timerTestcardSim = new DispatcherTimer(new TimeSpan(0, num5, 0), DispatcherPriority.Background, new EventHandler(this.sendTestcardSimImagesWithActSens), Application.Current.Dispatcher);
						this.timerTestcardSim.Start();
						this.timerprevieousTestcardSim = num5;
					}
				}
				else
				{
					this.timerprevieousTestcardSim = 0;
					this.SystemIsCompliant = "";
					this.ComplTextForeground = true;
					this.ComplTextVAForeground = true;
					this.ComplTextGlassForeground = true;
					bool flag9 = this.timerTestcardSim != null;
					if (flag9)
					{
						this.timerTestcardSim.Stop();
					}
				}
			}
			else
			{
				this.TestcardSimVisible = false;
				this.SystemIsCompliant = "";
			}
		}

		// Token: 0x06002401 RID: 9217 RVA: 0x000E5724 File Offset: 0x000E3924
		public void LoginRfidUser(string rfid)
		{
			List<User> list = new List<User>();
			bool userList = this.dbConnection.GetUserList(list, rfid);
			if (userList)
			{
				bool flag = list.Count >= 1;
				if (flag)
				{
					bool flag2 = list.Count > 1;
					if (flag2)
					{
						bool flag3 = MainWindowViewModel.accessRightsManager.CurrentUser.Rights == BuiltInPriviledges.NoUser;
						if (flag3)
						{
							Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
							{
								this.ChangeUser(true);
							}));
						}
						else
						{
							foreach (User user in list)
							{
								bool flag4 = MainWindowViewModel.accessRightsManager.CurrentUser.ID == user.ID && this._ctrlUsr == null;
								if (flag4)
								{
									Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
									{
										MainWindowViewModel.accessRightsManager.LogoutUser();
									}));
								}
							}
						}
					}
					else
					{
						bool flag5 = MainWindowViewModel.accessRightsManager.CurrentUser.Rights == BuiltInPriviledges.NoUser;
						if (flag5)
						{
							LogbookManager.AddEntry(SSTEntryCode.UserLoggedIn, 0, list.ElementAt(0).Username);
						}
						else
						{
							bool flag6 = list.ElementAt(0).Rights == BuiltInPriviledges.NoUser;
							if (flag6)
							{
								LogbookManager.AddEntry(SSTEntryCode.UserLoggedOut, 0, MainWindowViewModel.accessRightsManager.CurrentUser.Username, "", "", "");
							}
							else
							{
								LogbookManager.AddEntry(SSTEntryCode.UserChanged, 0, MainWindowViewModel.accessRightsManager.CurrentUser.Username, list.ElementAt(0).Username, "", "");
							}
						}
						bool flag7 = MainWindowViewModel.accessRightsManager.CurrentUser.Rights == BuiltInPriviledges.NoUser;
						if (flag7)
						{
							MainWindowViewModel.accessRightsManager.CurrentUser.Deletable = list.ElementAt(0).Deletable;
							MainWindowViewModel.accessRightsManager.CurrentUser.ID = list.ElementAt(0).ID;
							MainWindowViewModel.accessRightsManager.CurrentUser.PasswordAsMD5 = list.ElementAt(0).PasswordAsMD5;
							MainWindowViewModel.accessRightsManager.CurrentUser.Rights = list.ElementAt(0).Rights;
							MainWindowViewModel.accessRightsManager.CurrentUser.Username = list.ElementAt(0).Username;
							MainWindowViewModel.accessRightsManager.CurrentUser.CommitErrors = list.ElementAt(0).CommitErrors;
							Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
							{
								MainWindowViewModel.accessRightsManager.SetProperties();
							}));
						}
						else
						{
							bool flag8 = MainWindowViewModel.accessRightsManager.CurrentUser.ID == list.ElementAt(0).ID && this._ctrlUsr == null;
							if (flag8)
							{
								Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
								{
									MainWindowViewModel.accessRightsManager.LogoutUser();
								}));
							}
						}
					}
				}
			}
		}

		// Token: 0x06002402 RID: 9218 RVA: 0x000E5A58 File Offset: 0x000E3C58
		private void ConvertCorrectionTables()
		{
			bool flag = this.dbConnection.CorrectionTablesConversionNeeded();
			if (flag)
			{
				Progress.ShowStaticProgress("Converting correction tables...", false);
				Thread thread = new Thread(delegate()
				{
					Thread.Sleep(1000);
					this.dbConnection.ConvertCorrectionTables();
					Progress.CloseStaticProgress(false);
				});
				thread.Start();
			}
		}

		// Token: 0x06002403 RID: 9219 RVA: 0x000E5A9C File Offset: 0x000E3C9C
		private void SetImageBoxAutoZoom()
		{
			bool autoZoomEnabled = false;
			this.dbConnection.GetAutoZoom(ref autoZoomEnabled);
			XRSImageBoxViewModel.autoZoomEnabled = autoZoomEnabled;
		}

		// Token: 0x06002404 RID: 9220 RVA: 0x000E5AC0 File Offset: 0x000E3CC0
		private void InitializeImageBoxHigherContrastForDarkImg(ParameterHandling parHandling)
		{
			int num = 0;
			bool shownImgGetHigherContPointX = this.dbConnection.GetShownImgGetHigherContPointX(ref num);
			if (shownImgGetHigherContPointX)
			{
				parHandling.WriteValue(Definition.shownImgGetHigherContPointXPath, Convert.ToString(num));
				XRSImageBox.ShownImgGetHigherContPointX = num;
			}
			num = 0;
			bool shownImgGetHigherContPointY = this.dbConnection.GetShownImgGetHigherContPointY(ref num);
			if (shownImgGetHigherContPointY)
			{
				parHandling.WriteValue(Definition.shownImgGetHigherContPointYPath, Convert.ToString(num));
				XRSImageBox.ShownImgGetHigherContPointY = num;
			}
			num = 0;
			bool shownImgGetHigherContAverage = this.dbConnection.GetShownImgGetHigherContAverage(ref num);
			if (shownImgGetHigherContAverage)
			{
				parHandling.WriteValue(Definition.shownImgGetHigherContAveragePath, Convert.ToString(num));
				XRSImageBox.ShownImgGetHigherContAverage = num;
			}
			num = 0;
			bool flag = int.TryParse(parHandling.ReadPar(Definition.backroundNoisePath), out num);
			if (flag)
			{
				XRSImageBox.ShownImgGetHigherContBackgroundVal = num;
			}
		}

		// Token: 0x06002405 RID: 9221 RVA: 0x000E5B75 File Offset: 0x000E3D75
		private void productManager_SubProductModified()
		{
			this.SetActiveLanes();
		}

		// Token: 0x06002406 RID: 9222 RVA: 0x000E5B80 File Offset: 0x000E3D80
		private void PreJitControls()
		{
			CtrlQAParams content = new CtrlQAParams(this.dbConnection, this.parHandling, MainWindowViewModel.accessRightsManager, false, null, null, this.machineState);
			Window window = new Window();
			window.Content = content;
			window.WindowState = WindowState.Minimized;
			window.Loaded += this.wdQAParamsPreJit_Loaded;
			window.Show();
		}

		// Token: 0x06002407 RID: 9223 RVA: 0x000E5BE0 File Offset: 0x000E3DE0
		private void wdQAParamsPreJit_Loaded(object sender, RoutedEventArgs e)
		{
			bool flag = sender is Window;
			if (flag)
			{
				(sender as Window).Close();
			}
		}

		// Token: 0x06002408 RID: 9224 RVA: 0x000E5C08 File Offset: 0x000E3E08
		private void SetWindowSize()
		{
			int num = 0;
			int num2 = 0;
			this.dbConnection.GetWindowWidth(ref num);
			this.dbConnection.GetWindowHeight(ref num2);
			bool flag = num > 0 && num < 800;
			if (flag)
			{
				num = 800;
			}
			bool flag2 = num2 > 0 && num2 < 600;
			if (flag2)
			{
				num2 = 600;
			}
			Definition.windowWidth = ((num > 0) ? ((double)num) : SystemParameters.WorkArea.Width);
			Definition.windowHeight = ((num2 > 0) ? ((double)num2) : SystemParameters.WorkArea.Height);
			Definition.windowLeft = (SystemParameters.WorkArea.Width - Definition.windowWidth) / 2.0;
			Definition.windowTop = (SystemParameters.WorkArea.Height - Definition.windowHeight) / 2.0;
			this.WState = WindowState.Normal;
		}

		// Token: 0x06002409 RID: 9225 RVA: 0x000E5CEC File Offset: 0x000E3EEC
		private void InitializeLanes()
		{
			this.ShownLanes = new ObservableCollection<LaneXri>();
			this.AllLanes = new List<LaneXri>();
			this.AllSubProducts = new List<LaneXri>();
			LaneXri laneXri = new LaneXri(Lane.All, this.imageTransfer, this.parHandling, this.dbConnection, MainWindowViewModel.accessRightsManager, this.productManager, this.meanPixelLogger, this.machineState, this.imageArchiver, false, this.testcardsimservice, this.setFooterMainWindow, this.bulksensitivity);
			this.AllLanes.Add(laneXri);
			laneXri = new LaneXri(Lane.Lane1, this.imageTransfer, this.parHandling, this.dbConnection, MainWindowViewModel.accessRightsManager, this.productManager, this.meanPixelLogger, this.machineState, this.imageArchiver, false, this.testcardsimservice, this.setFooterMainWindow, this.bulksensitivity);
			this.AllLanes.Add(laneXri);
			laneXri = new LaneXri(Lane.Lane2, this.imageTransfer, this.parHandling, this.dbConnection, MainWindowViewModel.accessRightsManager, this.productManager, this.meanPixelLogger, this.machineState, this.imageArchiver, false, this.testcardsimservice, this.setFooterMainWindow, this.bulksensitivity);
			this.AllLanes.Add(laneXri);
			laneXri = new LaneXri(Lane.Lane3, this.imageTransfer, this.parHandling, this.dbConnection, MainWindowViewModel.accessRightsManager, this.productManager, this.meanPixelLogger, this.machineState, this.imageArchiver, false, this.testcardsimservice, this.setFooterMainWindow, this.bulksensitivity);
			this.AllLanes.Add(laneXri);
			laneXri = new LaneXri(Lane.Lane4, this.imageTransfer, this.parHandling, this.dbConnection, MainWindowViewModel.accessRightsManager, this.productManager, this.meanPixelLogger, this.machineState, this.imageArchiver, false, this.testcardsimservice, this.setFooterMainWindow, this.bulksensitivity);
			this.AllLanes.Add(laneXri);
			for (int i = 0; i <= Definition.maxSubProducts; i++)
			{
				laneXri = new LaneXri(Lane.All, this.imageTransfer, this.parHandling, this.dbConnection, MainWindowViewModel.accessRightsManager, this.productManager, this.meanPixelLogger, this.machineState, this.imageArchiver, false, this.testcardsimservice, this.setFooterMainWindow, this.bulksensitivity);
				laneXri.Group = i;
				this.AllSubProducts.Add(laneXri);
			}
			this.SetActiveLanes();
		}

		// Token: 0x0600240A RID: 9226 RVA: 0x000E5F4C File Offset: 0x000E414C
		private void SetActiveLanes()
		{
			bool flag = false;
			this.dbConnection.GetMultiLaneView(ref this.multiLaneView);
			this.MultiLaneView = this.multiLaneView;
			XRSImageBox.multiLaneSupport = this.multiLaneView;
			this.dbConnection.GetMultiProductView(ref flag);
			this.ShownLanes.Clear();
			bool flag2 = !this.multiLaneView;
			if (flag2)
			{
				bool flag3 = !flag || this.productManager == null || this.productManager.CurrentLoadedProduct == null || (!this.productManager.CurrentLoadedProduct.IsMultiProduct && !this.productManager.CurrentLoadedProduct.IsRegionProduct);
				if (flag3)
				{
					this.TabBorderThickness = new Thickness(0.0);
					foreach (LaneXri laneXri in this.AllLanes)
					{
						laneXri.TabHeight = 0;
						bool flag4 = laneXri.Lane > Lane.All;
						if (flag4)
						{
							laneXri.Active = false;
						}
						else
						{
							laneXri.Active = true;
							this.ShownLanes.Add(laneXri);
							this.SelectedXriLane = laneXri;
						}
					}
				}
				else
				{
					this.SetMultiProductViews();
				}
			}
			else
			{
				this.SetMultiLaneViews();
			}
			bool flag5 = this.ShownLanes.Count > 0 && (this.selectedXriLane == null || !this.ShownLanes.Contains(this.selectedXriLane));
			if (flag5)
			{
				List<LaneXri> list = (from lane in this.ShownLanes
				where lane.Lane == Lane.All
				select lane).ToList<LaneXri>();
				bool flag6 = list.Count > 0;
				if (flag6)
				{
					this.SelectedXriLane = list[0];
				}
				else
				{
					this.SelectedXriLane = this.ShownLanes[0];
				}
			}
			XRSImageBox.MinPixelLane2 = this.parHandling.ReadPar("/filter/acquisition/lane2/MinPixel/Value");
			XRSImageBox.MinPixelLane3 = this.parHandling.ReadPar("/filter/acquisition/lane3/MinPixel/Value");
			XRSImageBox.MinPixelLane4 = this.parHandling.ReadPar("/filter/acquisition/lane4/MinPixel/Value");
		}

		// Token: 0x0600240B RID: 9227 RVA: 0x000E618C File Offset: 0x000E438C
		private void SetMultiLaneViews()
		{
			string a = "";
			int num = 0;
			this.ShownLanes.Clear();
			this.TabBorderThickness = new Thickness(1.0, 1.0, 0.0, 0.0);
			this.dbConnection.GetParameterValue(Acquisition.lane1EnablePath, ref a);
			bool flag = a == "1";
			if (flag)
			{
				List<LaneXri> list = (from lane in this.AllLanes
				where lane.Lane == Lane.Lane1
				select lane).ToList<LaneXri>();
				bool flag2 = list.Count > 0;
				if (flag2)
				{
					list[0].Active = true;
					list[0].TabHeight = 40;
					list[0].UpdateLaneName();
					this.ShownLanes.Add(list[0]);
					num++;
				}
			}
			this.dbConnection.GetParameterValue(Acquisition.lane2EnablePath, ref a);
			bool flag3 = a == "1";
			if (flag3)
			{
				List<LaneXri> list2 = (from lane in this.AllLanes
				where lane.Lane == Lane.Lane2
				select lane).ToList<LaneXri>();
				bool flag4 = list2.Count > 0;
				if (flag4)
				{
					list2[0].Active = true;
					list2[0].TabHeight = 40;
					list2[0].UpdateLaneName();
					this.ShownLanes.Add(list2[0]);
					num++;
				}
			}
			this.dbConnection.GetParameterValue(Acquisition.lane3EnablePath, ref a);
			bool flag5 = a == "1";
			if (flag5)
			{
				List<LaneXri> list3 = (from lane in this.AllLanes
				where lane.Lane == Lane.Lane3
				select lane).ToList<LaneXri>();
				bool flag6 = list3.Count > 0;
				if (flag6)
				{
					list3[0].Active = true;
					list3[0].TabHeight = 40;
					list3[0].UpdateLaneName();
					this.ShownLanes.Add(list3[0]);
					num++;
				}
			}
			this.dbConnection.GetParameterValue(Acquisition.lane4EnablePath, ref a);
			bool flag7 = a == "1";
			if (flag7)
			{
				List<LaneXri> list4 = (from lane in this.AllLanes
				where lane.Lane == Lane.Lane4
				select lane).ToList<LaneXri>();
				bool flag8 = list4.Count > 0;
				if (flag8)
				{
					list4[0].Active = true;
					list4[0].TabHeight = 40;
					list4[0].UpdateLaneName();
					this.ShownLanes.Add(list4[0]);
					num++;
				}
			}
			bool flag9 = num > 1;
			if (flag9)
			{
				List<LaneXri> list5 = (from lane in this.AllLanes
				where lane.Lane == Lane.All
				select lane).ToList<LaneXri>();
				bool flag10 = list5.Count > 0;
				if (flag10)
				{
					list5[0].Active = true;
					list5[0].TabHeight = 40;
					this.ShownLanes.Add(list5[0]);
				}
			}
		}

		// Token: 0x0600240C RID: 9228 RVA: 0x000E651C File Offset: 0x000E471C
		private void SetMultiProductViews()
		{
			List<LaneXri> list = new List<LaneXri>();
			this.TabBorderThickness = new Thickness(1.0, 1.0, 0.0, 0.0);
			int num = 15;
			this.ShownLanes.Clear();
			bool flag = this.productManager.CurrentLoadedProduct.SubProducts.Count > 5;
			if (flag)
			{
				num = 10;
			}
			bool flag2 = this.productManager.CurrentLoadedProduct.SubProducts.Count > 10;
			if (flag2)
			{
				num = 8;
			}
			bool flag3 = this.productManager.CurrentLoadedProduct.SubProducts.Count > 18;
			if (flag3)
			{
				num = 5;
			}
			bool flag4 = this.productManager.CurrentLoadedProduct.IsMultiProduct || this.productManager.CurrentLoadedProduct.SubProducts.Count > 1;
			if (flag4)
			{
				List<LaneXri> list2 = (from subProduct in this.AllSubProducts
				where subProduct.Group == 0
				select subProduct).ToList<LaneXri>();
				bool flag5 = list2.Count > 0;
				if (flag5)
				{
					list2[0].Active = true;
					list2[0].TabHeight = 40;
					this.ShownLanes.Add(list2[0]);
				}
			}
			List<ProductData> list3 = new List<ProductData>();
			list3.AddRange(this.productManager.CurrentLoadedProduct.SubProducts);
			list3.Sort(new ProductDataComparerGeneric());
			foreach (ProductData productData in list3)
			{
				int group = 0;
				string s = productData.TableName.Remove(0, productData.TableName.IndexOf(Definition.subTableIdentifier) + 1);
				bool flag6 = int.TryParse(s, out group);
				if (flag6)
				{
					List<LaneXri> list4 = (from subProdLane in this.AllSubProducts
					where subProdLane.Group == @group
					select subProdLane).ToList<LaneXri>();
					bool flag7 = list4.Count > 0;
					if (flag7)
					{
						list4[0].Active = true;
						list4[0].TabHeight = 40;
						list4[0].LaneName = ((productData.ProductName.Length <= num) ? productData.ProductName : (productData.ProductName.Substring(0, num) + ".."));
						this.ShownLanes.Add(list4[0]);
					}
				}
			}
		}

		// Token: 0x0600240D RID: 9229 RVA: 0x000E67E4 File Offset: 0x000E49E4
		private void messageSystem_MessageAdded(Message msg)
		{
			this.DetermineWorstMessageType();
			this.OnPropertyChanged("ErrorMessages");
		}

		// Token: 0x0600240E RID: 9230 RVA: 0x000E67FC File Offset: 0x000E49FC
		private void DetermineWorstMessageType()
		{
			MessageType worstType = MessageType.Info;
			foreach (Message message in this.ErrorMessages)
			{
				bool flag = message.LogbookEntry.Type == MessageType.Error;
				if (flag)
				{
					worstType = MessageType.Error;
					break;
				}
				bool flag2 = message.LogbookEntry.Type == MessageType.Warning;
				if (flag2)
				{
					worstType = MessageType.Warning;
				}
			}
			bool flag3 = Application.Current != null;
			if (flag3)
			{
				Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(delegate()
				{
					this.WorstMessageType = worstType;
				}));
			}
		}

		// Token: 0x0600240F RID: 9231 RVA: 0x000E68C4 File Offset: 0x000E4AC4
		private void SetXMessageSystemImage(MessageType messageType)
		{
			bool flag = Application.Current != null;
			if (flag)
			{
				Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(delegate()
				{
					MessageType messageType2 = messageType;
					MessageType messageType3 = messageType2;
					if (messageType3 != MessageType.Error)
					{
						if (messageType3 != MessageType.Warning)
						{
							this.XMessageSystemImage = XrsImage.ImageConverter.ToBitmap("/img/infoMessage.png");
						}
						else
						{
							this.XMessageSystemImage = XrsImage.ImageConverter.ToBitmap("/img/warningIcon.png");
						}
					}
					else
					{
						this.XMessageSystemImage = XrsImage.ImageConverter.ToBitmap("/img/errorMessage.png");
					}
				}));
			}
		}

		// Token: 0x06002410 RID: 9232 RVA: 0x000E6911 File Offset: 0x000E4B11
		private void errorSurvey_ImageProcessingNetworkError()
		{
			this.FirstApplicationLoaded = true;
		}

		// Token: 0x06002411 RID: 9233 RVA: 0x000E691C File Offset: 0x000E4B1C
		private void accessRightsManager_PropertiesSet()
		{
			bool flag = this.BulkModeActive && this.parHandling != null;
			if (flag)
			{
				int num = 0;
				int.TryParse(this.parHandling.ReadPar("/filter/comparer/comparer1/Contrast/Value"), out num);
				bool flag2 = num < 10000;
				bool flag3 = flag2;
				if (flag3)
				{
					bool flag4 = this.CurrentUser.Rights >= this.rightTrain;
					if (flag4)
					{
						this.SliderVisibleBulkMode = true;
					}
					else
					{
						this.SliderVisibleBulkMode = false;
					}
				}
				else
				{
					this.SliderVisibleBulkMode = false;
				}
			}
			XRSImageBox.ServiceIsLoggedIn = (this.CurrentUser.Rights == BuiltInPriviledges.Service);
			bool flag5 = this.productManager != null;
			if (flag5)
			{
				bool flag6 = this.productManager.CurrentLoadedProduct.ProductNumber.Equals("-1") && !this.changing;
				if (flag6)
				{
					bool flag7 = MainWindowViewModel.accessRightsManager.CurrentUser.ID >= 0;
					if (flag7)
					{
						using (IEnumerator<ProductData> enumerator = this.productManager.ProductList.GetEnumerator())
						{
							if (enumerator.MoveNext())
							{
								ProductData productData = enumerator.Current;
								bool flag8 = !productData.ProductNumber.Equals("-1");
								if (flag8)
								{
									this.changing = true;
									this.productManager.ChangeProduct(productData);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06002412 RID: 9234 RVA: 0x000E6AA0 File Offset: 0x000E4CA0
		private void messageSystem_MessageRemoved(Message msg)
		{
			this.DetermineWorstMessageType();
			bool flag = msg.LogbookEntry.EntryCode == SSTEntryCode.BinFull || msg.LogbookEntry.EntryCode == SSTEntryCode.RejectVerify || msg.LogbookEntry.EntryCode == SSTEntryCode.LightBarrier;
			if (flag)
			{
				string path = "";
				bool flag2 = msg.LogbookEntry.Param1 == 1;
				if (flag2)
				{
					path = Definition.verifyReset1Path;
				}
				else
				{
					bool flag3 = msg.LogbookEntry.Param1 == 2;
					if (flag3)
					{
						path = Definition.verifyReset2Path;
					}
					else
					{
						bool flag4 = msg.LogbookEntry.Param1 == 3;
						if (flag4)
						{
							path = Definition.verifyReset3Path;
						}
						else
						{
							bool flag5 = msg.LogbookEntry.Param1 == 4;
							if (flag5)
							{
								path = Definition.verifyReset4Path;
							}
						}
					}
				}
				bool flag6 = msg.LogbookEntry.EntryCode == SSTEntryCode.BinFull;
				string value;
				if (flag6)
				{
					value = "1";
				}
				else
				{
					bool flag7 = msg.LogbookEntry.EntryCode == SSTEntryCode.RejectVerify;
					if (flag7)
					{
						value = "2";
					}
					else
					{
						value = "3";
					}
				}
				this.parHandling.WriteValue(path, value, false);
			}
			else
			{
				bool flag8 = msg.LogbookEntry.EntryCode == SSTEntryCode.SVTError;
				if (flag8)
				{
					bool flag9 = this.errorSurvey != null;
					if (flag9)
					{
						this.errorSurvey.ResetNSVTFaults();
					}
					this.parHandling.WriteValue(SVT.svtClearFaults, "1", false);
				}
				else
				{
					bool flag10 = msg.LogbookEntry.EntryCode == SSTEntryCode.XRayMotor;
					if (flag10)
					{
						this.parHandling.WriteValue(Definition.sshResetPath, "1", false);
					}
					else
					{
						bool flag11 = msg.LogbookEntry.EntryCode == SSTEntryCode.ProductsMissing;
						if (flag11)
						{
							string path2 = (msg.LogbookEntry.Param1 == 0) ? Definition.msResetError1Path : Definition.msResetError2Path;
							this.parHandling.WriteValue(path2, "1", false);
						}
						else
						{
							bool flag12 = msg.LogbookEntry.EntryCode == SSTEntryCode.SensorBlockedPhotogating;
							if (flag12)
							{
								string path3 = (msg.LogbookEntry.Param1 == 0) ? Definition.msResetError1Path : Definition.msResetError2Path;
								this.parHandling.WriteValue(path3, "2", false);
							}
							else
							{
								bool flag13 = msg.LogbookEntry.EntryCode == SSTEntryCode.SensorBlockedRejectCheck;
								if (flag13)
								{
									string path4 = (msg.LogbookEntry.Param1 == 0) ? Definition.msResetError1Path : Definition.msResetError2Path;
									this.parHandling.WriteValue(path4, "3", false);
								}
								else
								{
									bool flag14 = msg.LogbookEntry.EntryCode == SSTEntryCode.ErrorConsistency;
									if (flag14)
									{
										string path5 = (msg.LogbookEntry.Param1 == 0) ? Definition.msResetError1Path : Definition.msResetError2Path;
										this.parHandling.WriteValue(path5, "4", false);
									}
									else
									{
										bool flag15 = msg.LogbookEntry.EntryCode == SSTEntryCode.BinMissing;
										if (flag15)
										{
											string path6 = (msg.LogbookEntry.Param1 == 0) ? Definition.msResetError1Path : Definition.msResetError2Path;
											this.parHandling.WriteValue(path6, "5", false);
										}
										else
										{
											bool flag16 = msg.LogbookEntry.EntryCode == SSTEntryCode.ProductNotInspected;
											if (flag16)
											{
												string path7 = (msg.LogbookEntry.Param1 == 0) ? Definition.msResetError1Path : Definition.msResetError2Path;
												this.parHandling.WriteValue(path7, "6", false);
											}
											else
											{
												bool flag17 = msg.LogbookEntry.EntryCode == SSTEntryCode.TooManyRejects;
												if (flag17)
												{
													this.parHandling.WriteValue(Definition.resetRejecterErrorsPath, "1", false);
												}
												else
												{
													bool flag18 = msg.LogbookEntry.EntryCode == SSTEntryCode.IOController && msg.LogbookEntry.Param1 == 1;
													if (flag18)
													{
														this.parHandling.WriteValue(Definition.ioErrorResetPath, "1", false);
													}
													else
													{
														bool flag19 = msg.LogbookEntry.EntryCode == SSTEntryCode.InOutletSensorBlocked;
														if (flag19)
														{
															this.parHandling.WriteValue(Definition.inOutletBlockedResetPath, ((int)(msg.LogbookEntry.Param1 + 1)).ToString(), false);
														}
														else
														{
															bool flag20 = msg.LogbookEntry.EntryCode == SSTEntryCode.LimitsError;
															if (flag20)
															{
																this.errorSurvey.ResetMonitoring();
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
				}
			}
			int num = 0;
			int.TryParse(this.parHandling.ReadPar(Acquisition.objimageDelayPath), out num);
			bool flag21 = msg.LogbookEntry.EntryCode == SSTEntryCode.ProcessingDelay && this.parHandling.ReadPar(Acquisition.acquisitionModePath) == "1" && num > 0;
			if (flag21)
			{
				this.parHandling.WriteValue(Acquisition.objimageDelayPath, "0", false);
			}
			bool flag22 = msg.LogbookEntry.EntryCode < SSTEntryCode.DatabaseVersion && msg.LogbookEntry.EntryCode > SSTEntryCode.Metal;
			if (flag22)
			{
				this.dbConnection.RemoveUncommittedError(msg.LogbookEntry.ID);
			}
			this.ResetIOErrorOutputIfNeeded();
			bool flag23 = msg.LogbookEntry.EntryCode == SSTEntryCode.Printer && msg.LogbookEntry.Param1 == 0;
			if (flag23)
			{
				bool enabled = this.videojetPrinter.Enabled;
				if (enabled)
				{
					this.videojetPrinter.Disconnect();
					bool flag24 = !this.videojetPrinter.Connect();
					if (flag24)
					{
						this.errorSurvey.PostMessage(SSTEntryCode.Printer, 0);
					}
				}
			}
			this.OnPropertyChanged("ErrorMessages");
		}

		// Token: 0x06002413 RID: 9235 RVA: 0x000E6FFC File Offset: 0x000E51FC
		private void ResetIOErrorOutputIfNeeded()
		{
			List<Message> list = (from message in this.messageSystem.Messages
			where (byte)message.LogbookEntry.EntryCode < 64 && (byte)message.LogbookEntry.EntryCode > 1
			select message).ToList<Message>();
			bool flag = list.Count > 0;
			if (flag)
			{
				bool flag2 = false;
				this.dbConnection.SetAuditCheckErrorReset(true);
				foreach (Message message2 in list)
				{
					SSTEntryCode entryCode = message2.LogbookEntry.EntryCode;
					bool flag3 = entryCode == SSTEntryCode.ProcessingDelay || entryCode == SSTEntryCode.MemoryUsage || entryCode == SSTEntryCode.ParamComm || entryCode == SSTEntryCode.ImageComm || entryCode == SSTEntryCode.IOComm || entryCode == SSTEntryCode.IOController || entryCode == SSTEntryCode.ImageProcPC || entryCode == SSTEntryCode.SVTComm || entryCode == SSTEntryCode.SVTError || entryCode == SSTEntryCode.XRayMotor || ((entryCode == SSTEntryCode.DetectorComm || entryCode == SSTEntryCode.CalibrationFault) && this.errorHandling.DetectorErrorEnabled) || ((entryCode == SSTEntryCode.XRayComm || entryCode == SSTEntryCode.XRayFault) && this.errorHandling.XrayErrorEnabled) || (entryCode == SSTEntryCode.LowAirPressure && this.errorHandling.LowAirPressureErrorEnabled) || (entryCode == SSTEntryCode.AuditFailedError && this.auditSettings.AuditRequired.AuditFailedError) || (entryCode == SSTEntryCode.InsightNetConnection && this.errorHandling.InsightNetConnectionErrorEnabled) || (entryCode == SSTEntryCode.DiskSpaceLow && this.errorHandling.DiskSpaceLowErrorEnabled) || (entryCode == SSTEntryCode.CoolingError && this.errorHandling.CoolingErrorEnabled) || (entryCode == SSTEntryCode.MotorError && this.errorHandling.EngineErrorEnabled) || (entryCode == SSTEntryCode.Temperature && this.errorHandling.TemperatureErrorEnabled) || (entryCode == SSTEntryCode.Voltage && this.errorHandling.VoltageErrorEnabled) || (entryCode == SSTEntryCode.XRayLamp && this.errorHandling.XrayLampErrorEnabled) || (entryCode == SSTEntryCode.BinFull && this.errorHandling.BinFullEnabled) || ((entryCode == SSTEntryCode.LightBarrier || entryCode == SSTEntryCode.RejectVerify) && this.errorHandling.MissingObjectsEnabled) || (entryCode == SSTEntryCode.BinMissing && this.errorHandling.BinMissingEnabled) || (entryCode == SSTEntryCode.TooManyRejects && this.errorHandling.TooManyRejectsEnabled) || ((entryCode == SSTEntryCode.ProductsMissing || entryCode == SSTEntryCode.SensorBlockedPhotogating || entryCode == SSTEntryCode.SensorBlockedRejectCheck || entryCode == SSTEntryCode.ErrorConsistency || entryCode == SSTEntryCode.ProductNotInspected) && this.errorHandling.HighLevelSensorsEnabled) || entryCode == SSTEntryCode.InOutletSensorBlocked;
					if (flag3)
					{
						flag2 = true;
						break;
					}
				}
				bool flag4 = !flag2;
				if (flag4)
				{
					this.errorSurvey.RequestReset = true;
				}
			}
			else
			{
				this.errorSurvey.RequestReset = true;
			}
		}

		// Token: 0x06002414 RID: 9236 RVA: 0x000E72CC File Offset: 0x000E54CC
		private void CheckIfInDemoMode()
		{
			bool flag = false;
			this.dbConnection.GetDemoMode(ref flag);
			bool flag2 = flag;
			if (flag2)
			{
				this.FirstApplicationLoaded = true;
				this.DemoMode = true;
			}
		}

		// Token: 0x06002415 RID: 9237 RVA: 0x000E7304 File Offset: 0x000E5504
		private void machineState_StateChangedToInspecting(MachineStates oldState)
		{
			this.ShowNext = true;
			bool flag = Application.Current != null;
			if (flag)
			{
				Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
				{
					bool flag4 = this.auditNeeded;
					if (flag4)
					{
						bool flag5 = oldState != MachineStates.PreAuditCheck && oldState != MachineStates.AuditCheck;
						if (flag5)
						{
							this.ExecuteAuditCheck();
						}
						this.auditNeeded = false;
					}
				}));
			}
			bool flag2 = !this.PlotShown;
			if (flag2)
			{
				bool bulkModeActive = this.BulkModeActive;
				if (bulkModeActive)
				{
					this.parHandling.WriteValue(Definition.checkArcsEnable, "0");
				}
				else
				{
					this.parHandling.WriteValue(Definition.checkArcsEnable, "1");
				}
			}
			bool flag3 = this.StandardMode;
			if (flag3)
			{
				this.IsImageBoxVisible = true;
			}
		}

		// Token: 0x06002416 RID: 9238 RVA: 0x000E73B9 File Offset: 0x000E55B9
		private void ExecuteAuditCheck()
		{
			this.auditSettings.AuditRequired.LastAuditFailedSaved = false;
			Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
			{
				try
				{
					bool contaminationEnabled = this.auditSettings.ContaminationEnabled;
					if (contaminationEnabled)
					{
						this.ShowAuditForContamination();
					}
					else
					{
						bool weightEnabled = this.auditSettings.WeightEnabled;
						if (weightEnabled)
						{
							this.ShowAuditForWeight();
						}
						else
						{
							bool countEnabled = this.auditSettings.CountEnabled;
							if (countEnabled)
							{
								this.ShowAuditForObjectCount();
							}
							else
							{
								this.FinishAuditCheck();
							}
						}
					}
				}
				catch
				{
					this.wdAuditRequired = null;
				}
			}));
		}

		// Token: 0x06002417 RID: 9239 RVA: 0x000E73EC File Offset: 0x000E55EC
		private void ShowAuditForContamination()
		{
			int num = 0;
			this.dbConnection.GetAuditCheckType(ref num);
			bool flag = num == 1;
			if (flag)
			{
				bool flag2 = this.machineState.State == MachineStates.Inspecting;
				if (flag2)
				{
					bool flag3 = false;
					this.dbConnection.GetStopConveyorIfError(ref flag3);
					bool flag4 = !this.auditSettings.AuditRequired.LastAuditFailedSaved || !this.auditSettings.AuditRequired.AuditFailedError || !flag3;
					if (flag4)
					{
						bool flag5 = this.checkTestcards();
						if (flag5)
						{
							string text = "";
							this.dbConnection.GetParameterValue("FastAuditCheck", ref text);
							bool bulkModeActive = this.BulkModeActive;
							if (bulkModeActive)
							{
								this.auditSettings.AuditRequired.UpdateRemainingTime();
								this.bulkAuditCheckProcess = new CtrlBulkAuditCheckProcess(this.imageTransfer, this.auditSettings.AuditRequired, MainWindowViewModel.accessRightsManager, this.machineState, AuditMode.Contamination, this.parHandling, this.restore, this.setFooterMainWindow, this.loginToGo, this.messageSystem, this.dbConnection, this.productManager);
								Subwindow.Show(this.bulkAuditCheckProcess, SubwindowButtons.BackAndHome);
								this.bulkAuditCheckProcess.Unloaded += this.wdAuditRequired_ContaminationUnloaded;
								this.oldValfooterMainWindowIsDeactivated = this.FooterMainWindowIsDeactivated;
								this.FooterMainWindowIsDeactivated = false;
							}
							else
							{
								bool flag6 = text.Equals("1");
								if (flag6)
								{
									this.auditSettings.AuditRequired.UpdateRemainingTime();
									this.fastAuditCheckProcess = new CtrlFastAuditCheckProcess(this.imageTransfer, this.auditSettings.AuditRequired, MainWindowViewModel.accessRightsManager, this.machineState, AuditMode.Contamination, this.parHandling, this.restore, this.setFooterMainWindow, this.loginToGo, this.messageSystem, this.dbConnection, this.productManager);
									Subwindow.Show(this.fastAuditCheckProcess, SubwindowButtons.BackAndHome);
									this.fastAuditCheckProcess.Unloaded += this.wdAuditRequired_ContaminationUnloaded;
									this.oldValfooterMainWindowIsDeactivated = this.FooterMainWindowIsDeactivated;
									this.FooterMainWindowIsDeactivated = false;
								}
								else
								{
									this.auditSettings.AuditRequired.UpdateRemainingTime();
									this.auditCheckProcess = new CtrlAuditCheckProcess(this.imageTransfer, this.auditSettings.AuditRequired, MainWindowViewModel.accessRightsManager, this.machineState, AuditMode.Contamination, this.parHandling, this.restore, this.setFooterMainWindow, this.loginToGo, this.messageSystem, this.dbConnection, this.productManager);
									Subwindow.Show(this.auditCheckProcess, SubwindowButtons.BackAndHome);
									this.auditCheckProcess.Unloaded += this.wdAuditRequired_ContaminationUnloaded;
									this.oldValfooterMainWindowIsDeactivated = this.FooterMainWindowIsDeactivated;
									this.FooterMainWindowIsDeactivated = false;
								}
							}
						}
						else
						{
							this.setAuditCheckFailedLogbookEntry();
							Dialog.Show(LocalizationManager.Localize("txtError"), string.Concat(new string[]
							{
								LocalizationManager.Localize("menuItemAuditCheck"),
								": ",
								LocalizationManager.Localize("txtTestsamplesNotSetAudit"),
								". ",
								LocalizationManager.Localize("txtTestFailed")
							}), DialogStyle.Error, DialogButtons.OK, false);
							bool weightEnabled = this.auditSettings.WeightEnabled;
							if (weightEnabled)
							{
								this.ShowAuditForWeight();
							}
							else
							{
								bool countEnabled = this.auditSettings.CountEnabled;
								if (countEnabled)
								{
									this.ShowAuditForObjectCount();
								}
								else
								{
									this.FinishAuditCheck();
								}
							}
						}
					}
					else
					{
						this.FinishAuditCheck();
					}
				}
				else
				{
					Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtCannotContinueAuditCheck") + ". " + LocalizationManager.Localize("txtTurnXrayOn") + ".", DialogStyle.Error, DialogButtons.OK);
					this.FinishAuditCheck();
				}
			}
			else
			{
				this.auditSettings.AuditRequired.UpdateRemainingTime();
				this.wdAuditRequired = new WdAuditRequired(this.imageTransfer, this.auditSettings.AuditRequired, MainWindowViewModel.accessRightsManager, this.machineState, AuditMode.Contamination, this.parHandling, this.restore, this.setFooterMainWindow, this.loginToGo);
				this.wdAuditRequired.Unloaded += this.wdAuditRequired_ContaminationUnloaded;
				this.wdAuditRequired.Owner = this.mainWindow;
				this.wdAuditRequired.Show();
				this.oldValfooterMainWindowIsDeactivated = this.FooterMainWindowIsDeactivated;
				this.FooterMainWindowIsDeactivated = false;
			}
		}

		// Token: 0x06002418 RID: 9240 RVA: 0x000E7834 File Offset: 0x000E5A34
		private void setAuditCheckFailedLogbookEntry()
		{
			bool auditFailedError = this.auditSettings.AuditRequired.AuditFailedError;
			if (auditFailedError)
			{
				this.auditSettings.AuditRequired.LastAuditFailed = true;
				this.auditSettings.AuditRequired.LastAuditFailedSaved = true;
			}
			else
			{
				LogbookManager.AddEntry(SSTEntryCode.AuditFailedInfo);
			}
			this.dbConnection.SetLastAuditCheckResult(AuditResult.NotPerformed);
			this.dbConnection.SetAuditCheckErrorReset(false);
			int num = 0;
			bool nauditChecksFailed = this.auditSettings.AuditRequired.DbConnection.GetNAuditChecksFailed(ref num);
			if (nauditChecksFailed)
			{
				this.auditSettings.AuditRequired.DbConnection.SetNAuditChecksFailed(++num);
			}
		}

		// Token: 0x06002419 RID: 9241 RVA: 0x000E78E0 File Offset: 0x000E5AE0
		private bool checkTestcards()
		{
			string path = "TestcardMaterial1";
			string path2 = "TestcardMaterial2";
			string path3 = "TestcardMaterial3";
			string path4 = "TestcardMaterial4";
			string path5 = "TestcardMaterial5";
			bool result = false;
			string text = "";
			this.dbConnection.GetParameterValue(path, ref text);
			bool flag = text != null && text != "n.a." && text != "" && text != "None";
			if (flag)
			{
				result = true;
			}
			else
			{
				this.dbConnection.GetParameterValue(path2, ref text);
				bool flag2 = text != null && text != "n.a." && text != "" && text != "None";
				if (flag2)
				{
					result = true;
				}
				else
				{
					this.dbConnection.GetParameterValue(path3, ref text);
					bool flag3 = text != null && text != "n.a." && text != "" && text != "None";
					if (flag3)
					{
						result = true;
					}
					else
					{
						this.dbConnection.GetParameterValue(path4, ref text);
						bool flag4 = text != null && text != "n.a." && text != "" && text != "None";
						if (flag4)
						{
							result = true;
						}
						else
						{
							this.dbConnection.GetParameterValue(path5, ref text);
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

		// Token: 0x0600241A RID: 9242 RVA: 0x000E7AA8 File Offset: 0x000E5CA8
		private void wdAuditRequired_ContaminationUnloaded(object sender, RoutedEventArgs e)
		{
			bool flag = this.wdAuditRequired != null;
			if (flag)
			{
				this.wdAuditRequired.Unloaded -= this.wdAuditRequired_ContaminationUnloaded;
				this.wdAuditRequired = null;
				bool weightEnabled = this.auditSettings.WeightEnabled;
				if (weightEnabled)
				{
					this.ShowAuditForWeight();
				}
				else
				{
					bool countEnabled = this.auditSettings.CountEnabled;
					if (countEnabled)
					{
						this.ShowAuditForObjectCount();
					}
					else
					{
						this.FinishAuditCheck();
					}
				}
				this.FooterMainWindowIsDeactivated = this.oldValfooterMainWindowIsDeactivated;
			}
			else
			{
				bool flag2 = this.auditCheckProcess != null;
				if (flag2)
				{
					this.auditCheckProcess.Unloaded -= this.wdAuditRequired_ContaminationUnloaded;
					this.auditCheckProcess = null;
					this.FooterMainWindowIsDeactivated = this.oldValfooterMainWindowIsDeactivated;
					bool weightEnabled2 = this.auditSettings.WeightEnabled;
					if (weightEnabled2)
					{
						this.ShowAuditForWeight();
					}
					else
					{
						bool countEnabled2 = this.auditSettings.CountEnabled;
						if (countEnabled2)
						{
							this.ShowAuditForObjectCount();
						}
						else
						{
							this.FinishAuditCheck();
						}
					}
				}
				else
				{
					bool flag3 = this.fastAuditCheckProcess != null;
					if (flag3)
					{
						this.fastAuditCheckProcess.Unloaded -= this.wdAuditRequired_ContaminationUnloaded;
						this.fastAuditCheckProcess = null;
						this.FooterMainWindowIsDeactivated = this.oldValfooterMainWindowIsDeactivated;
						bool weightEnabled3 = this.auditSettings.WeightEnabled;
						if (weightEnabled3)
						{
							this.ShowAuditForWeight();
						}
						else
						{
							bool countEnabled3 = this.auditSettings.CountEnabled;
							if (countEnabled3)
							{
								this.ShowAuditForObjectCount();
							}
							else
							{
								this.FinishAuditCheck();
							}
						}
					}
					else
					{
						bool flag4 = this.bulkAuditCheckProcess != null;
						if (flag4)
						{
							this.bulkAuditCheckProcess.Unloaded -= this.wdAuditRequired_ContaminationUnloaded;
							this.bulkAuditCheckProcess = null;
							this.FooterMainWindowIsDeactivated = this.oldValfooterMainWindowIsDeactivated;
							this.FinishAuditCheck();
						}
					}
				}
			}
		}

		// Token: 0x0600241B RID: 9243 RVA: 0x000E7C68 File Offset: 0x000E5E68
		private void ShowAuditForWeight()
		{
			int num = 0;
			this.dbConnection.GetAuditCheckType(ref num);
			bool flag = num == 1;
			if (flag)
			{
				bool flag2 = this.machineState.State == MachineStates.Inspecting;
				if (flag2)
				{
					bool flag3 = false;
					string a = "";
					string str = (this.productManager.CurrentLoadedProduct.Group > 0) ? this.productManager.CurrentLoadedProduct.Group.ToString() : "";
					bool flag4 = this.dbConnection.GetParameterValue(Definition.filterMainPath + str + Weigher.enableWeightSubPath, ref a) && a == "1";
					if (flag4)
					{
						flag3 = true;
					}
					bool flag5 = false;
					this.dbConnection.GetStopConveyorIfError(ref flag5);
					bool flag6 = !this.auditSettings.AuditRequired.LastAuditFailedSaved || !this.auditSettings.AuditRequired.AuditFailedError || !flag5;
					if (flag6)
					{
						bool flag7 = flag3;
						if (flag7)
						{
							this.auditSettings.AuditRequired.UpdateRemainingTime();
							this.auditCheckProcess = new CtrlAuditCheckProcess(this.imageTransfer, this.auditSettings.AuditRequired, MainWindowViewModel.accessRightsManager, this.machineState, AuditMode.Weight, this.parHandling, this.restore, this.setFooterMainWindow, this.loginToGo, this.messageSystem, this.dbConnection, this.productManager);
							Subwindow.Show(this.auditCheckProcess, SubwindowButtons.BackAndHome);
							this.auditCheckProcess.Unloaded += this.wdAuditRequired_WeightUnloaded;
							this.oldValfooterMainWindowIsDeactivated = this.FooterMainWindowIsDeactivated;
							this.FooterMainWindowIsDeactivated = false;
						}
						else
						{
							Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("lbAuditCheck") + ": " + LocalizationManager.Localize("lbWeigherDeactivated"), DialogStyle.Warning, DialogButtons.OK, false);
							this.setAuditCheckFailedLogbookEntry();
							bool countEnabled = this.auditSettings.CountEnabled;
							if (countEnabled)
							{
								this.ShowAuditForObjectCount();
							}
							else
							{
								this.FinishAuditCheck();
							}
						}
					}
					else
					{
						this.FinishAuditCheck();
					}
				}
				else
				{
					Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtCannotContinueAuditCheck") + ". " + LocalizationManager.Localize("txtTurnXrayOn") + ".", DialogStyle.Error, DialogButtons.OK);
					this.FinishAuditCheck();
				}
			}
			else
			{
				this.auditSettings.AuditRequired.UpdateRemainingTime();
				this.wdAuditRequired = new WdAuditRequired(this.imageTransfer, this.auditSettings.AuditRequired, MainWindowViewModel.accessRightsManager, this.machineState, AuditMode.Weight, this.parHandling, this.restore, this.setFooterMainWindow, this.loginToGo);
				this.wdAuditRequired.Unloaded += this.wdAuditRequired_WeightUnloaded;
				this.wdAuditRequired.Owner = this.mainWindow;
				this.wdAuditRequired.Show();
				this.oldValfooterMainWindowIsDeactivated = this.FooterMainWindowIsDeactivated;
				this.FooterMainWindowIsDeactivated = false;
			}
		}

		// Token: 0x0600241C RID: 9244 RVA: 0x000E7F5C File Offset: 0x000E615C
		private void wdAuditRequired_WeightUnloaded(object sender, RoutedEventArgs e)
		{
			bool flag = this.wdAuditRequired != null;
			if (flag)
			{
				this.wdAuditRequired.Unloaded -= this.wdAuditRequired_WeightUnloaded;
				this.wdAuditRequired = null;
				bool countEnabled = this.auditSettings.CountEnabled;
				if (countEnabled)
				{
					this.ShowAuditForObjectCount();
				}
				else
				{
					this.FinishAuditCheck();
				}
				this.FooterMainWindowIsDeactivated = this.oldValfooterMainWindowIsDeactivated;
			}
			else
			{
				bool flag2 = this.auditCheckProcess != null;
				if (flag2)
				{
					this.auditCheckProcess.Unloaded -= this.wdAuditRequired_ContaminationUnloaded;
					this.auditCheckProcess = null;
					this.FooterMainWindowIsDeactivated = this.oldValfooterMainWindowIsDeactivated;
					bool countEnabled2 = this.auditSettings.CountEnabled;
					if (countEnabled2)
					{
						this.ShowAuditForObjectCount();
					}
					else
					{
						this.FinishAuditCheck();
					}
				}
			}
		}

		// Token: 0x0600241D RID: 9245 RVA: 0x000E8020 File Offset: 0x000E6220
		private void ShowAuditForObjectCount()
		{
			int num = 0;
			this.dbConnection.GetAuditCheckType(ref num);
			bool flag = num == 1;
			if (flag)
			{
				bool flag2 = this.machineState.State == MachineStates.Inspecting;
				if (flag2)
				{
					bool flag3 = false;
					string a = "";
					string str = (this.productManager.CurrentLoadedProduct.Group > 0) ? this.productManager.CurrentLoadedProduct.Group.ToString() : "";
					bool flag4 = this.dbConnection.GetParameterValue(Definition.filterMainPath + str + ObjectCounter.enabledSubPath, ref a) && a == "1";
					if (flag4)
					{
						flag3 = true;
					}
					bool flag5 = false;
					this.dbConnection.GetStopConveyorIfError(ref flag5);
					bool flag6 = !this.auditSettings.AuditRequired.LastAuditFailedSaved || !this.auditSettings.AuditRequired.AuditFailedError || !flag5;
					if (flag6)
					{
						bool flag7 = flag3;
						if (flag7)
						{
							this.auditSettings.AuditRequired.UpdateRemainingTime();
							this.auditCheckProcess = new CtrlAuditCheckProcess(this.imageTransfer, this.auditSettings.AuditRequired, MainWindowViewModel.accessRightsManager, this.machineState, AuditMode.Count, this.parHandling, this.restore, this.setFooterMainWindow, this.loginToGo, this.messageSystem, this.dbConnection, this.productManager);
							this.auditCheckProcess.Unloaded += this.wdAuditRequired_ObjectCountUnloaded;
							Subwindow.Show(this.auditCheckProcess, SubwindowButtons.BackAndHome);
							this.oldValfooterMainWindowIsDeactivated = this.FooterMainWindowIsDeactivated;
							this.FooterMainWindowIsDeactivated = false;
						}
						else
						{
							Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("lbAuditCheck") + ": " + LocalizationManager.Localize("txtCountDeactivated"), DialogStyle.Warning, DialogButtons.OK, false);
							this.setAuditCheckFailedLogbookEntry();
							this.FinishAuditCheck();
							this.FooterMainWindowIsDeactivated = this.oldValfooterMainWindowIsDeactivated;
						}
					}
					else
					{
						this.FinishAuditCheck();
					}
				}
				else
				{
					Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtCannotContinueAuditCheck") + ". " + LocalizationManager.Localize("txtTurnXrayOn") + ".", DialogStyle.Error, DialogButtons.OK);
					this.FinishAuditCheck();
				}
			}
			else
			{
				this.auditSettings.AuditRequired.UpdateRemainingTime();
				this.wdAuditRequired = new WdAuditRequired(this.imageTransfer, this.auditSettings.AuditRequired, MainWindowViewModel.accessRightsManager, this.machineState, AuditMode.Count, this.parHandling, this.restore, this.setFooterMainWindow, this.loginToGo);
				this.wdAuditRequired.Unloaded += this.wdAuditRequired_ObjectCountUnloaded;
				this.wdAuditRequired.Owner = this.mainWindow;
				this.wdAuditRequired.Show();
				this.oldValfooterMainWindowIsDeactivated = this.FooterMainWindowIsDeactivated;
				this.FooterMainWindowIsDeactivated = false;
			}
		}

		// Token: 0x0600241E RID: 9246 RVA: 0x000E8308 File Offset: 0x000E6508
		private void wdAuditRequired_ObjectCountUnloaded(object sender, RoutedEventArgs e)
		{
			bool flag = this.wdAuditRequired != null;
			if (flag)
			{
				this.wdAuditRequired.Unloaded -= this.wdAuditRequired_ObjectCountUnloaded;
				this.wdAuditRequired = null;
				this.FinishAuditCheck();
				this.FooterMainWindowIsDeactivated = this.oldValfooterMainWindowIsDeactivated;
			}
			else
			{
				bool flag2 = this.auditCheckProcess != null;
				if (flag2)
				{
					this.auditCheckProcess.Unloaded -= this.wdAuditRequired_ContaminationUnloaded;
					this.auditCheckProcess = null;
					this.FooterMainWindowIsDeactivated = this.oldValfooterMainWindowIsDeactivated;
					this.FinishAuditCheck();
				}
			}
		}

		// Token: 0x0600241F RID: 9247 RVA: 0x000E839C File Offset: 0x000E659C
		private void FinishAuditCheck()
		{
			this.auditSettings.UpdateAuditInterval();
			bool bulkModeActive = this.BulkModeActive;
			if (bulkModeActive)
			{
				bool flag = this.auditSettings.ContaminationEnabled || this.auditSettings.WeightEnabled || this.auditSettings.CountEnabled;
				if (flag)
				{
					BulkMode.Activate(this.dbConnection, this.parHandling);
				}
			}
		}

		// Token: 0x06002420 RID: 9248 RVA: 0x000E8400 File Offset: 0x000E6600
		private void StartAutotrainSingleProduct(MachineType machinetype, AccessRightsManager accessManager)
		{
			bool flag = false;
			this.dbConnection.GetMultiRegionEnabled(ref flag);
			DetectorType detectorType = DetectorType.Unknown;
			this.dbConnection.GetDetectorType(ref detectorType);
			bool flag2 = false;
			bool flag3 = detectorType == DetectorType.Sesotec;
			if (flag3)
			{
				flag2 = (this.parHandling.ReadPar(Definition.detectorSesotecPixelSizePath) == "0.4");
			}
			else
			{
				bool flag4 = detectorType == DetectorType.DTNetTDI;
				if (flag4)
				{
					flag2 = (this.parHandling.ReadPar(Definition.detectorDtPixelSizePath) == "0.4");
				}
			}
			bool flag5 = true;
			this.dbConnection.GetMXFeatures04Det(ref flag5);
			string a = "";
			this.dbConnection.GetXRaySourceModelNumber(ref a);
			bool flag6 = (machinetype == MachineType.RayconDPlus || machinetype == MachineType.RayconEX2A) && (flag2 || detectorType == DetectorType.DTNetDE) && !flag && this.StandardMode && !this.productManager.CurrentLoadedProduct.ProductNumber.Equals("-1");
			MachineMode machineMode = MachineMode.Standard;
			this.dbConnection.GetMachineMode(ref machineMode);
			bool flag7 = machineMode == MachineMode.Bulk && flag2 && BulkMode.NuvoPCPerformance;
			if (flag7)
			{
				flag6 = true;
			}
			bool flag8 = flag6 && a == "IXS050SE100P214" && flag5;
			if (flag8)
			{
				flag6 = false;
			}
			bool flag9 = flag6;
			if (flag9)
			{
				this.ctrlAutotrainAdjustments = new CtrlAutotrainAdjustments(this.parHandling, this.imageTransfer, this.dbConnection, accessManager, this.machineState, this.productManager, 0, this.imageArchiver, this.testcardsimservice, this.auditSettings);
				bool flag10 = Subwindow.ShowAutotrainAdjustments(this.ctrlAutotrainAdjustments, SubwindowButtons.ConfirmAndCancel, null, true, this.parHandling, this.productManager, this.dbConnection) == Subwindow.AdjustmentResults.Ok;
				if (flag10)
				{
					this.StartAutotrainMainMenu(machinetype, accessManager, Filter.AutoKVChangedToDisabled);
				}
			}
			else
			{
				this.StartAutotrainMainMenu(machinetype, accessManager, false);
			}
			this.setFooterMainWindow.SetFooterDeactivation();
		}

		// Token: 0x06002421 RID: 9249 RVA: 0x000E85D0 File Offset: 0x000E67D0
		public void StartAutotrainMainMenu(MachineType machinetype, AccessRightsManager accessManager, bool autokVChangedToDisabled)
		{
			DetectorType detectorType = DetectorType.DTNetDE;
			bool demoMode = false;
			uint num = 1U;
			bool flag = this.dbConnection != null;
			if (flag)
			{
				this.dbConnection.GetDetectorType(ref detectorType);
				this.dbConnection.GetDemoMode(ref demoMode);
				this.DemoMode = demoMode;
				this.dbConnection.GetAutoKVAdjustment(ref num);
			}
			bool flag2 = machinetype == MachineType.RayconDPlus && detectorType == DetectorType.Sesotec && this.parHandling.ReadPar(Definition.detectorSesotecPixelSizePath) == "0.8";
			this.noAutoKVAdjustment = (machinetype == MachineType.RayconXRS || machinetype == MachineType.RayconD || machinetype == MachineType.RayconEntry || detectorType == DetectorType.DTNetDE || this.DemoMode || num == 0U || flag2 || machinetype == MachineType.RayconEX2A);
			bool flag3 = !this.noAutoKVAdjustment && (this.parHandling.ReadPar(Definition.autoKVAdjustmentPath) == "1" || this.parHandling.ReadPar(Definition.autoKVAdjustmentPath) == "2");
			if (flag3)
			{
				this.parHandling.WriteValue(Definition.autoKVInitImagePath, "1");
			}
			this.ctrlAutotrain = new AutotrainControl(this.imageTransfer, this.parHandling, this.dbConnection, AutolearnType.Contamination, this.machineState, this.productManager, 0, this.xrayState, this.noAutoKVAdjustment, autokVChangedToDisabled, accessManager, this.testcardsimservice, this.bulksensitivity);
			MainWindowViewModel.subwindowAutotrainMain = new Subwindow(this.ctrlAutotrain, SubwindowButtons.Autotrain, true, false, null, this.parHandling, this.productManager, this.dbConnection);
			MainWindowViewModel.subwindowAutotrainMain.PanelButtons = this.ctrlAutotrain.Buttons;
			HomeManager.Instance.Register(MainWindowViewModel.subwindowAutotrainMain);
			this.ctrlAutotrain.Unloaded += this.ctrlAutotrain_Unloaded;
			MainWindowViewModel.subwindowAutotrainMain.Show();
		}

		// Token: 0x06002422 RID: 9250 RVA: 0x000E8792 File Offset: 0x000E6992
		private void imageArchiver_XRIImageSaved(int time)
		{
			this.parHandling.WriteValue(MainWindowViewModel.timeSaveXriPath, time.ToString(), false);
		}

		// Token: 0x06002423 RID: 9251 RVA: 0x000E87B0 File Offset: 0x000E69B0
		private void dbConnection_ProductParametersChanged(string tableName, List<string> paths, List<string> values)
		{
			bool flag = this.machineState.State != MachineStates.ProductChange && this.parametersInitialized;
			if (flag)
			{
				this.backup.StartAutoBackupTimer();
			}
		}

		// Token: 0x06002424 RID: 9252 RVA: 0x000E87E8 File Offset: 0x000E69E8
		private void dbConnection_ProductParameterChanged(string tableName, string path, string value)
		{
			bool flag = this.machineState.State != MachineStates.ProductChange && this.parametersInitialized;
			if (flag)
			{
				this.backup.StartAutoBackupTimer();
			}
		}

		// Token: 0x06002425 RID: 9253 RVA: 0x000E8820 File Offset: 0x000E6A20
		private void dbConnection_SystemParameterChanged(string path, string value)
		{
			if (!(path == "SerialNumber"))
			{
				if (path == "DeviceAddress")
				{
					this.deviceAddress = value;
					this.sstServer.DeviceAddress = this.deviceAddress;
				}
			}
			else
			{
				this.deviceSerialNbr = value;
			}
		}

		// Token: 0x06002426 RID: 9254 RVA: 0x000E8874 File Offset: 0x000E6A74
		private void InitializeDesign()
		{
			bool flag = File.Exists(DBConnection.databasePath);
			if (flag)
			{
				Definition.GUIDesign design = this.dbConnection.GetDesign();
				this.SelectedBranding = new Branding(design);
			}
			else
			{
				this.SelectedBranding = new Branding();
			}
			this.SelectedBranding.IsActiveBranding = true;
		}

		// Token: 0x06002427 RID: 9255 RVA: 0x000E88C8 File Offset: 0x000E6AC8
		private void InitializeCommunication()
		{
			string text = "";
			string text2 = "";
			int receiveTimeout = 2000;
			int maxConnections = 1;
			this.dbConnection.GetIPElinos(ref text2);
			this.dbConnection.GetIPComServer(ref text);
			this.dbConnection.GetSSTRecvTimeout(ref receiveTimeout);
			this.dbConnection.GetSSTMaxConnections(ref maxConnections);
			bool flag = text2 != "";
			if (flag)
			{
				this.imageTransfer = new ImageDataTransfer(text2, this.imageArchiver);
			}
			else
			{
				this.imageTransfer = new ImageDataTransfer(this.imageArchiver);
			}
			this.imageTransfer.FrameArrived += this.imageTransfer_FrameArrived;
			this.imageTransfer.Connected += this.imageTransfer_Connected;
			this.imageTransfer.Disconnected += this.imageTransfer_Disconnected;
			this.imageTransfer.ImageDataError += this.imageTransfer_ImageDataError;
			this.parHandling = new ParameterHandling(this.dbConnection, this.dbDatalogs);
			this.parHandling.Connected += this.parHandling_Connected;
			this.parHandling.Disconnected += this.parHandling_Disconnected;
			bool flag2 = text2 != "";
			if (flag2)
			{
				this.modelTransfer = new ModelTransfer(text2, this.parHandling, this.dbConnection);
			}
			else
			{
				this.modelTransfer = new ModelTransfer(Definition.defaultElinosIP, this.parHandling, this.dbConnection);
			}
			bool flag3 = false;
			this.dbConnection.GetAnomalyDetectionEnabled(ref flag3);
			bool flag4 = !flag3;
			if (flag4)
			{
				this.dbConnection.GetObjectDetectionEnabled(ref flag3);
			}
			bool flag5 = flag3;
			if (flag5)
			{
				this.InitializeModelTransferConnection();
			}
			this.sstServer = new SSTEthernetServer();
			this.sstServer.MaxConnections = maxConnections;
			this.sstServer.ReceiveTimeout = receiveTimeout;
			this.sstServer.CommandFrameReceived += this.sstServer_CommandFrameReceived;
			this.sstServer.ListenErrorOccured += this.sstServer_ListenErrorOccured;
			this.sstServer.StartListening();
			bool flag6 = text != "";
			if (flag6)
			{
				this.comServer = new ComServerClient(text);
			}
			else
			{
				this.comServer = new ComServerClient();
			}
			this.comServer.Connected += this.comServer_Connected;
			this.comServer.StartConnect();
			this.wsFoodServer = new WSFoodServer();
			this.wsFoodServer.StartListening();
			this.wsFoodServer.WSFrameReceived += this.WSFoodServer_WSFrameReceived;
			this.InitializeWifi();
		}

		// Token: 0x06002428 RID: 9256 RVA: 0x000E8B64 File Offset: 0x000E6D64
		private void InitializeWifi()
		{
			try
			{
				string text = "";
				string text2 = "";
				string text3 = "";
				string text4 = "";
				bool hidden = false;
				this.wifiClient = new WifiClient(this.dbConnection);
				bool flag = File.Exists(WifiClient.ewfDllFilePath);
				if (flag)
				{
					this.wifiClient.RemoveAllProfiles();
				}
				this.dbConnection.GetWifiNetworkName(ref text);
				this.dbConnection.GetWifiKey(ref text2);
				this.dbConnection.GetWifiEncryption(ref text4);
				this.dbConnection.GetWifiAuthentication(ref text3);
				this.dbConnection.GetWifiHidden(ref hidden);
				bool flag2 = text != null && text.Length > 0 && text3 != null && text3.Length > 0 && text4 != null && text4.Length > 0 && this.wifiClient.WlanClient.Interfaces.Length != 0;
				if (flag2)
				{
					bool flag3 = text2 != null && text2.Length > 0;
					if (flag3)
					{
						try
						{
							SimpleAES simpleAES = new SimpleAES();
							text2 = simpleAES.Decrypt(text2);
						}
						catch
						{
							text2 = null;
						}
					}
					else
					{
						text2 = null;
					}
					string xmlProfile = this.wifiClient.CreateXmlProfile(text, text3, text4, text2, hidden);
					this.wifiClient.Connect(xmlProfile);
				}
			}
			catch
			{
			}
		}

		// Token: 0x06002429 RID: 9257 RVA: 0x000E8CD8 File Offset: 0x000E6ED8
		private void sstServer_ListenErrorOccured(Exception ex)
		{
			bool flag = Application.Current != null;
			if (flag)
			{
				Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(delegate()
				{
					Dialog.Show(LocalizationManager.Localize("txtError"), "SST-server couldn't be started. Error: " + ex.Message, DialogStyle.Error, DialogButtons.OK);
				}));
			}
		}

		// Token: 0x0600242A RID: 9258 RVA: 0x000E8D20 File Offset: 0x000E6F20
		private void imageTransfer_ImageDataError(string message)
		{
			bool flag = this.imageTransfer != null;
			if (flag)
			{
				this.imageTransfer.CloseConnection();
				Thread.Sleep(1500);
				this.imageTransfer.StartConnect();
			}
		}

		// Token: 0x0600242B RID: 9259 RVA: 0x000E8D60 File Offset: 0x000E6F60
		private void comServer_Connected()
		{
			SSTSocket sstsocket = new SSTSocket(this.comServer.Client);
			sstsocket.IsComServer = true;
			this.sstServer.StartClientCommunication(sstsocket);
			this.comServer.StartPingComServer();
		}

		// Token: 0x0600242C RID: 9260 RVA: 0x000E8DA0 File Offset: 0x000E6FA0
		private void sstServer_CommandFrameReceived(SSTEthernetFrame.SSTCommands command, object argument)
		{
			try
			{
				switch (command)
				{
				case SSTEthernetFrame.SSTCommands.ProductChange_PC:
				{
					this.sstServer.LastCommandSuccess = false;
					int num = 0;
					num = this.HomeManager.NumSubWindows;
					bool flag = num == 0 && this.machineState.State != MachineStates.Reseasoning && this.machineState.State != MachineStates.XrayOnCalibrationNotDone && this.machineState.State != MachineStates.Autotrain;
					if (flag)
					{
						bool flag2 = argument is ProductInfo;
						if (flag2)
						{
							ProductInfo productInfo = (ProductInfo)argument;
							foreach (ProductData productData4 in this.productManager.ProductList)
							{
								bool flag3 = productData4.ProductNumber == productInfo.ProductNumber.ToString();
								if (flag3)
								{
									bool flag4 = this.productManager.ChangeProduct(productData4);
									if (flag4)
									{
										bool flag5 = false;
										this.dbConnection.GetBatchesEnabled(ref flag5);
										this.sstServer.LastCommandSuccess = true;
										this.lastSSTBatch = productInfo.BatchNumber.ToString();
										bool flag6 = productData4 == this.productManager.CurrentLoadedProduct;
										if (flag6)
										{
											this.ShowBatchWindowOrStartNew(productData4, productData4);
										}
										bool flag7 = flag5;
										if (flag7)
										{
											this.batchHasChanged = true;
										}
									}
									break;
								}
							}
						}
					}
					bool flag8 = num == 0 && this.machineState.State != MachineStates.Reseasoning && this.machineState.State != MachineStates.XrayOnCalibrationNotDone && this.machineState.State != MachineStates.Autotrain;
					if (flag8)
					{
						bool flag9 = argument is Insight2ProductInfo;
						if (flag9)
						{
							this.sstServer.Insight2CommandSuccess = false;
							ushort productNumber;
							ushort.TryParse(this.productManager.CurrentLoadedProduct.ProductNumber, out productNumber);
							this.sstServer.Insight2Argument = new Insight2ProductInfo(productNumber);
							Insight2ProductInfo insight2ProductInfo = (Insight2ProductInfo)argument;
							foreach (ProductData productData2 in this.productManager.ProductList)
							{
								bool flag10 = productData2.ProductNumber == insight2ProductInfo.ProductNumber.ToString();
								if (flag10)
								{
									this.sstServer.Insight2CommandSuccess = this.productManager.ChangeProduct(productData2);
									this.sstServer.Insight2Argument = new Insight2ProductInfo(insight2ProductInfo.ProductNumber);
								}
							}
						}
					}
					break;
				}
				case SSTEthernetFrame.SSTCommands.DeviceName_DN:
				{
					bool flag11 = argument == null;
					if (flag11)
					{
						string text = "";
						this.dbConnection.GetDeviceName(ref text);
						bool flag12 = text.Length > 10;
						if (flag12)
						{
							text = text.Substring(0, 10);
						}
						text = text.PadRight(10, ' ');
						this.sstServer.AnswerArgument = text;
					}
					else
					{
						bool flag13 = argument is string;
						if (flag13)
						{
							this.dbConnection.SetDeviceName(((string)argument).TrimEnd(null));
						}
					}
					break;
				}
				case SSTEthernetFrame.SSTCommands.DeviceAddress_DA:
				{
					bool flag14 = argument == null;
					if (flag14)
					{
						this.sstServer.AnswerArgument = this.deviceAddress;
					}
					else
					{
						bool flag15 = argument is string;
						if (flag15)
						{
							this.sstServer.DeviceAddress = (string)argument;
							this.dbConnection.SetDeviceAddress((string)argument);
						}
					}
					break;
				}
				case SSTEthernetFrame.SSTCommands.DeviceType_DT:
					this.sstServer.AnswerArgument = "30";
					break;
				case SSTEthernetFrame.SSTCommands.NumberOfProducts_PI:
					this.sstServer.AnswerArgument = this.productManager.ProductList.Count.ToString("X4");
					break;
				case SSTEthernetFrame.SSTCommands.CurrentProductNumber_PJ:
				{
					ushort productNumber2;
					ushort.TryParse(this.productManager.CurrentLoadedProduct.ProductNumber, out productNumber2);
					this.sstServer.AnswerArgument = productNumber2.ToString("X4");
					this.sstServer.Insight2Argument = new Insight2ProductInfo(productNumber2, (ushort)this.productManager.ProductList.Count);
					this.sstServer.Insight2CommandSuccess = true;
					break;
				}
				case SSTEthernetFrame.SSTCommands.ProductName_PN:
				{
					bool flag16 = argument != null;
					if (flag16)
					{
						bool flag17 = argument is string;
						if (flag17)
						{
							int requestedProductNumber = -1;
							int.TryParse((string)argument, NumberStyles.HexNumber, null, out requestedProductNumber);
							bool flag18 = requestedProductNumber > 0;
							if (flag18)
							{
								List<ProductData> list = (from productData in this.productManager.ProductList
								where productData.ProductNumber == requestedProductNumber.ToString()
								select productData).ToList<ProductData>();
								bool flag19 = list.Count > 0;
								if (flag19)
								{
									bool flag20 = list[0].ProductName.Length <= 10;
									if (flag20)
									{
										this.sstServer.AnswerArgument = list[0].ProductName;
										while (this.sstServer.AnswerArgument.Length < 10)
										{
											SSTEthernetServer sstethernetServer = this.sstServer;
											sstethernetServer.AnswerArgument += " ";
										}
									}
									else
									{
										this.sstServer.AnswerArgument = list[0].ProductName.Substring(0, 10);
									}
								}
							}
							else
							{
								this.sstServer.AnswerArgument = "          ";
							}
						}
						else
						{
							bool flag21 = argument is ProductInfo;
							if (flag21)
							{
								ProductInfo info = (ProductInfo)argument;
								List<ProductData> list2 = (from productData in this.productManager.ProductList
								where productData.ProductNumber == info.ProductNumber.ToString()
								select productData).ToList<ProductData>();
								bool flag22 = list2.Count > 0;
								if (flag22)
								{
									this.productManager.RenameProduct((int)info.ProductNumber, info.ProductName.Trim());
								}
							}
						}
					}
					break;
				}
				case SSTEthernetFrame.SSTCommands.DeviceVersion_DV:
				{
					bool flag23 = Definition.guiVersion.Length > 4;
					if (flag23)
					{
						int num2 = 0;
						int num3 = 0;
						int num4 = 0;
						int num5 = Definition.guiVersion.IndexOf('.');
						bool flag24 = num5 > 0;
						if (flag24)
						{
							int.TryParse(Definition.guiVersion.Substring(0, num5), out num2);
						}
						int num6 = Definition.guiVersion.IndexOf('.', num5 + 1);
						int.TryParse(Definition.guiVersion.Substring(num5 + 1, num6 - num5 - 1), out num3);
						int num7 = Definition.guiVersion.IndexOf('_');
						bool flag25 = num7 < 0;
						if (flag25)
						{
							int.TryParse(Definition.guiVersion.Substring(num6 + 1, Definition.guiVersion.Length - num6 - 1), out num4);
						}
						else
						{
							int.TryParse(Definition.guiVersion.Substring(num6 + 1, num7 - num6 - 1), out num4);
						}
						this.sstServer.AnswerArgument = string.Concat(new string[]
						{
							num2.ToString(""),
							".",
							num3.ToString("D2"),
							".",
							num4.ToString()
						});
						bool flag26 = this.sstServer.AnswerArgument.Length > 6;
						if (flag26)
						{
							this.sstServer.AnswerArgument = this.sstServer.AnswerArgument.Substring(0, 6);
						}
						SSTEthernetServer sstethernetServer2 = this.sstServer;
						sstethernetServer2.AnswerArgument += " ";
					}
					else
					{
						this.sstServer.AnswerArgument = Definition.guiVersion;
						while (this.sstServer.AnswerArgument.Length < 5)
						{
							SSTEthernetServer sstethernetServer3 = this.sstServer;
							sstethernetServer3.AnswerArgument += " ";
						}
						SSTEthernetServer sstethernetServer4 = this.sstServer;
						sstethernetServer4.AnswerArgument += "  ";
					}
					break;
				}
				case SSTEthernetFrame.SSTCommands.SystemTime_TT:
				{
					bool flag27 = argument == null;
					if (flag27)
					{
						DateTime now = DateTime.Now;
						this.sstServer.AnswerArgument = (now.Year % 2000).ToString("X2");
						SSTEthernetServer sstethernetServer5 = this.sstServer;
						sstethernetServer5.AnswerArgument += now.Month.ToString("X2");
						SSTEthernetServer sstethernetServer6 = this.sstServer;
						sstethernetServer6.AnswerArgument += now.Day.ToString("X2");
						SSTEthernetServer sstethernetServer7 = this.sstServer;
						sstethernetServer7.AnswerArgument += now.Hour.ToString("X2");
						SSTEthernetServer sstethernetServer8 = this.sstServer;
						sstethernetServer8.AnswerArgument += now.Minute.ToString("X2");
						SSTEthernetServer sstethernetServer9 = this.sstServer;
						sstethernetServer9.AnswerArgument += now.Second.ToString("X2");
					}
					else
					{
						string text2 = (string)argument;
						int num8;
						int.TryParse(text2.Substring(0, 2), NumberStyles.HexNumber, null, out num8);
						num8 += 2000;
						int month;
						int.TryParse(text2.Substring(2, 2), NumberStyles.HexNumber, null, out month);
						int day;
						int.TryParse(text2.Substring(4, 2), NumberStyles.HexNumber, null, out day);
						int hour;
						int.TryParse(text2.Substring(6, 2), NumberStyles.HexNumber, null, out hour);
						int minute;
						int.TryParse(text2.Substring(8, 2), NumberStyles.HexNumber, null, out minute);
						int second;
						int.TryParse(text2.Substring(10, 2), NumberStyles.HexNumber, null, out second);
						DateTime dateTime = new DateTime(num8, month, day, hour, minute, second);
						this.imageTransfer.Disconnected -= this.imageTransfer_Disconnected;
						this.parHandling.Disconnected -= this.parHandling_Disconnected;
						System.Threading.Timer timer = new System.Threading.Timer(delegate(object dummy)
						{
							this.imageTransfer.Disconnected += this.imageTransfer_Disconnected;
							this.parHandling.Disconnected += this.parHandling_Disconnected;
						}, null, 10000, -1);
						SystemTime.SetTime(dateTime);
						LogbookManager.AddEntry(SSTEntryCode.SystemTimeChanged);
					}
					break;
				}
				case SSTEthernetFrame.SSTCommands.LineName_DL:
				{
					bool flag28 = argument == null;
					if (flag28)
					{
						string text3 = "";
						this.dbConnection.GetLineName(ref text3);
						bool flag29 = text3.Length > 10;
						if (flag29)
						{
							text3 = text3.Substring(0, 10);
						}
						text3 = text3.PadRight(10, ' ');
						this.sstServer.AnswerArgument = text3;
					}
					else
					{
						bool flag30 = argument is string;
						if (flag30)
						{
							this.dbConnection.SetLineName(((string)argument).TrimEnd(null));
						}
					}
					break;
				}
				case SSTEthernetFrame.SSTCommands.SystemCounter_SJ:
				{
					bool flag31 = argument != null && argument is string;
					if (flag31)
					{
						uint num9 = 0U;
						uint num10 = 0U;
						uint num11 = 0U;
						bool flag32 = argument.ToString() == "00";
						if (flag32)
						{
							this.dbConnection.GetThroughputIndexGlobal(ref num9);
							this.dbConnection.GetRejectIndexGlobal(ref num10);
							this.dbConnection.GetNErrors(ref num11);
							this.sstServer.AnswerArgument = num11.ToString("X4") + num10.ToString("X4") + num9.ToString("X4");
						}
						else
						{
							bool flag33 = argument.ToString() == "01";
							if (flag33)
							{
								this.dbConnection.GetCurrentProductIndex(true, ref num9);
								this.dbConnection.GetCurrentProductIndex(false, ref num10);
								this.sstServer.AnswerArgument = "0000" + num10.ToString("X4") + num9.ToString("X4");
							}
							else
							{
								bool flag34 = argument.ToString() == "02";
								if (flag34)
								{
									this.sstServer.AnswerArgument = "000000000000";
									bool flag35 = false;
									this.dbConnection.GetBatchesEnabled(ref flag35);
									bool flag36 = flag35;
									if (flag36)
									{
										Batch batch = null;
										this.dbConnection.GetCurrentBatch(ref batch);
										bool flag37 = batch != null;
										if (flag37)
										{
											this.sstServer.AnswerArgument = "0000" + batch.NRejects.ToString("X4") + batch.NProducts.ToString("X4");
										}
									}
								}
								else
								{
									this.sstServer.AnswerArgument = "000000000000";
								}
							}
						}
					}
					break;
				}
				case SSTEthernetFrame.SSTCommands.SystemStatus_SQ:
				{
					byte b = 0;
					byte b2 = 0;
					ushort num12 = 0;
					uint num13 = 0U;
					byte b3 = 0;
					ushort num14 = 0;
					byte b4 = 0;
					byte b5 = 0;
					bool flag38 = this.machineState.State == MachineStates.Inspecting;
					if (flag38)
					{
						b3 |= 1;
					}
					bool flag39 = this.machineState.Error > 0;
					if (flag39)
					{
						b3 |= 2;
					}
					bool newLogbookEntry = LogbookManager.newLogbookEntry;
					if (newLogbookEntry)
					{
						b3 |= 16;
					}
					bool flag40 = MainWindowViewModel.flagSaveImgToNetwork;
					if (flag40)
					{
						b3 |= 32;
					}
					bool flag41 = MainWindowViewModel.flagNotSaveImgToNetwork;
					if (flag41)
					{
						b3 |= 64;
					}
					bool flag42 = MainWindowViewModel.flagSaveImgToFtP;
					if (flag42)
					{
						b3 |= 128;
					}
					bool flag43 = this.machineState.State == MachineStates.PreAuditCheck;
					if (flag43)
					{
						b = 1;
						b2 = 1;
						bool flag44 = this.auditSettings != null & this.auditSettings.AuditRequired != null;
						if (flag44)
						{
							num14 = (ushort)this.auditSettings.AuditRequired.RemainingTime;
						}
					}
					else
					{
						bool flag45 = this.machineState.State == MachineStates.AuditCheck;
						if (flag45)
						{
							b = 1;
							b2 = 2;
						}
					}
					this.sstServer.AnswerArgument = string.Concat(new string[]
					{
						b.ToString("X2"),
						b2.ToString("X2"),
						num12.ToString("X4"),
						num13.ToString("X8"),
						b3.ToString("X2"),
						num14.ToString("X4"),
						b4.ToString("X2"),
						b5.ToString("X2")
					});
					break;
				}
				case SSTEthernetFrame.SSTCommands.SystemKey_SK:
				{
					bool flag46 = Application.Current != null;
					if (flag46)
					{
						Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(delegate()
						{
							bool flag108 = argument is uint && (uint)argument == 5U;
							if (flag108)
							{
								this.ResetMessages(null);
							}
						}));
					}
					break;
				}
				case SSTEthernetFrame.SSTCommands.ClearSystemStatus_SC:
					MainWindowViewModel.flagSaveImgToNetwork = false;
					MainWindowViewModel.flagNotSaveImgToNetwork = false;
					MainWindowViewModel.flagSaveImgToFtP = false;
					LogbookManager.newLogbookEntry = false;
					break;
				case SSTEthernetFrame.SSTCommands.LogbookDump_LD:
				{
					SSTEthernetFrame.LogbookType type = SSTEthernetFrame.LogbookType.Unknown;
					bool flag47 = argument is SSTEthernetFrame.LogbookType;
					if (flag47)
					{
						type = (SSTEthernetFrame.LogbookType)argument;
					}
					this.sstServer.AnswerArgument = this.logbook.GetDump(type);
					break;
				}
				case SSTEthernetFrame.SSTCommands.LogbookClear_LC:
				{
					bool flag48 = Application.Current != null;
					if (flag48)
					{
						Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(delegate()
						{
							this.logbook.DeleteLogbook(true);
						}));
					}
					break;
				}
				case SSTEthernetFrame.SSTCommands.ImageNumber_IN:
				{
					bool flag49 = argument != null && argument is string;
					if (flag49)
					{
						int num15 = 0;
						bool flag50 = (string)argument == "01";
						if (flag50)
						{
							bool flag51 = Directory.Exists(StoreImageData.archiveDirectoryReject);
							if (flag51)
							{
								num15 = Directory.GetFiles(StoreImageData.archiveDirectoryReject).Length;
							}
						}
						else
						{
							bool flag52 = (string)argument == "02";
							if (flag52)
							{
								bool flag53 = Directory.Exists(StoreImageData.archiveDirectoryThroughput);
								if (flag53)
								{
									num15 = Directory.GetFiles(StoreImageData.archiveDirectoryThroughput).Length;
								}
							}
							else
							{
								bool flag54 = Directory.Exists(StoreImageData.archiveDirectoryThroughput);
								if (flag54)
								{
									num15 = Directory.GetFiles(StoreImageData.archiveDirectoryThroughput).Length;
								}
								bool flag55 = Directory.Exists(StoreImageData.archiveDirectoryReject);
								if (flag55)
								{
									num15 += Directory.GetFiles(StoreImageData.archiveDirectoryReject).Length;
								}
							}
						}
						this.sstServer.AnswerArgument = num15.ToString("X4");
					}
					break;
				}
				case SSTEthernetFrame.SSTCommands.ImageTransfer_IT:
					this.SetSSTImageTransferAnswer(argument);
					break;
				case SSTEthernetFrame.SSTCommands.LogbookNumberOfEntries_LN:
				{
					int num16 = 0;
					this.dbConnection.GetNumLogbookEntries(ref num16);
					this.sstServer.AnswerArgument = num16.ToString("X4");
					break;
				}
				case SSTEthernetFrame.SSTCommands.LogbookGetEntry_LX:
				{
					bool flag56 = argument is uint;
					if (flag56)
					{
						this.SetSSTLogbookEntryAnswer((uint)argument);
					}
					break;
				}
				case SSTEthernetFrame.SSTCommands.DeviceSerial_DS:
				{
					string text4 = this.deviceSerialNbr;
					bool flag57 = text4.Length > 20;
					if (flag57)
					{
						text4 = text4.Substring(0, 20);
					}
					text4 = text4.PadRight(20, ' ');
					this.sstServer.AnswerArgument = text4;
					break;
				}
				case SSTEthernetFrame.SSTCommands.SystemCounterExtended_SH:
				{
					bool flag58 = argument != null && argument is string;
					if (flag58)
					{
						uint num17 = 0U;
						uint num18 = 0U;
						uint num19 = 0U;
						bool flag59 = argument.ToString() == "00";
						if (flag59)
						{
							this.dbConnection.GetThroughputIndexGlobal(ref num17);
							this.dbConnection.GetRejectIndexGlobal(ref num18);
							this.dbConnection.GetNErrors(ref num19);
							this.sstServer.AnswerArgument = num19.ToString("X8") + num18.ToString("X8") + num17.ToString("X8") + "0000";
						}
						else
						{
							bool flag60 = argument.ToString() == "01";
							if (flag60)
							{
								this.dbConnection.GetCurrentProductIndex(true, ref num17);
								this.dbConnection.GetCurrentProductIndex(false, ref num18);
								this.sstServer.AnswerArgument = "00000000" + num18.ToString("X8") + num17.ToString("X8") + "0000";
							}
							else
							{
								bool flag61 = argument.ToString() == "02";
								if (flag61)
								{
									this.sstServer.AnswerArgument = "0000000000000000000000000000";
									bool flag62 = false;
									this.dbConnection.GetBatchesEnabled(ref flag62);
									bool flag63 = flag62;
									if (flag63)
									{
										Batch batch2 = null;
										this.dbConnection.GetCurrentBatch(ref batch2);
										bool flag64 = batch2 != null;
										if (flag64)
										{
											this.sstServer.AnswerArgument = "00000000" + batch2.NRejects.ToString("X8") + batch2.NProducts.ToString("X8") + "0000";
										}
									}
								}
								else
								{
									bool flag65 = argument.ToString() == "03";
									if (flag65)
									{
										List<LaneXri> list3 = (from lane in this.ShownLanes
										where lane.Lane == Lane.All
										select lane).ToList<LaneXri>();
										uint num20 = 0U;
										uint num21 = 0U;
										bool flag66 = list3.Count > 0;
										if (flag66)
										{
											num20 = list3[0].Counter.ThroughputCounter;
											num21 = list3[0].Counter.RejectCounter;
										}
										else
										{
											bool flag67 = this.ShownLanes.Count > 0;
											if (flag67)
											{
												num20 = this.ShownLanes[0].Counter.ThroughputCounter;
												num21 = this.ShownLanes[0].Counter.RejectCounter;
											}
										}
										list3.Clear();
										this.sstServer.AnswerArgument = "00000000" + num21.ToString("X8") + num20.ToString("X8") + "0000";
									}
									else
									{
										bool flag68 = argument.ToString() == "04";
										if (flag68)
										{
											List<LaneXri> list4 = (from lane in this.ShownLanes
											where lane.Lane == Lane.Lane1
											select lane).ToList<LaneXri>();
											uint num22 = 0U;
											uint num23 = 0U;
											bool flag69 = list4.Count > 0;
											if (flag69)
											{
												num22 = list4[0].Counter.ThroughputCounter;
												num23 = list4[0].Counter.RejectCounter;
											}
											list4.Clear();
											this.sstServer.AnswerArgument = "00000000" + num23.ToString("X8") + num22.ToString("X8") + "0000";
										}
										else
										{
											bool flag70 = argument.ToString() == "05";
											if (flag70)
											{
												List<LaneXri> list5 = (from lane in this.ShownLanes
												where lane.Lane == Lane.Lane2
												select lane).ToList<LaneXri>();
												uint num24 = 0U;
												uint num25 = 0U;
												bool flag71 = list5.Count > 0;
												if (flag71)
												{
													num24 = list5[0].Counter.ThroughputCounter;
													num25 = list5[0].Counter.RejectCounter;
												}
												list5.Clear();
												this.sstServer.AnswerArgument = "00000000" + num25.ToString("X8") + num24.ToString("X8") + "0000";
											}
											else
											{
												bool flag72 = argument.ToString() == "06";
												if (flag72)
												{
													List<LaneXri> list6 = (from lane in this.ShownLanes
													where lane.Lane == Lane.Lane3
													select lane).ToList<LaneXri>();
													uint num26 = 0U;
													uint num27 = 0U;
													bool flag73 = list6.Count > 0;
													if (flag73)
													{
														num26 = list6[0].Counter.ThroughputCounter;
														num27 = list6[0].Counter.RejectCounter;
													}
													list6.Clear();
													this.sstServer.AnswerArgument = "00000000" + num27.ToString("X8") + num26.ToString("X8") + "0000";
												}
												else
												{
													bool flag74 = argument.ToString() == "07";
													if (flag74)
													{
														List<LaneXri> list7 = (from lane in this.ShownLanes
														where lane.Lane == Lane.Lane4
														select lane).ToList<LaneXri>();
														uint num28 = 0U;
														uint num29 = 0U;
														bool flag75 = list7.Count > 0;
														if (flag75)
														{
															num28 = list7[0].Counter.ThroughputCounter;
															num29 = list7[0].Counter.RejectCounter;
														}
														list7.Clear();
														this.sstServer.AnswerArgument = "00000000" + num29.ToString("X8") + num28.ToString("X8") + "0000";
													}
													else
													{
														this.sstServer.AnswerArgument = "0000000000000000000000000000";
													}
												}
											}
										}
									}
								}
							}
						}
					}
					break;
				}
				case SSTEthernetFrame.SSTCommands.ImageTransferExtended_IX:
				{
					bool flag76 = argument != null && argument is string && ((string)argument).Length == 12;
					if (flag76)
					{
						this.SetSSTImageTransferExtendedAnswer((string)argument);
					}
					break;
				}
				case SSTEthernetFrame.SSTCommands.XRayOpHrs_OH:
				{
					XRayOperatingHours xrayOperatingHours = new XRayOperatingHours(this.dbConnection, this.parHandling, this.dbDatalogs);
					string text5 = "";
					int num30 = 0;
					string text6 = "";
					int num31 = 0;
					double a = 0.0;
					string text7 = "";
					string text8 = "";
					string text9 = "00000000000000000000";
					this.dbConnection.GetXRaySerialnumber(ref text7);
					this.dbConnection.GetXRaySourceModelNumber(ref text8);
					this.dbConnection.GetXRayOnTimeInternal(ref text6);
					this.dbConnection.GetDynXRayOpHoursLimit(ref num30);
					bool flag77 = text7 != "";
					if (flag77)
					{
						text7 = text7.Substring(12);
					}
					else
					{
						text7 = text9;
					}
					text7 = text7.PadRight(20, ' ');
					text5 += text7;
					bool flag78 = text8 == "";
					if (flag78)
					{
						text8 = text9;
					}
					text8 = text8.PadRight(20, ' ');
					text5 += text8;
					bool flag79 = text6 != "";
					if (flag79)
					{
						double.TryParse(text6, NumberStyles.Float, new CultureInfo("en-US"), out a);
						num31 = Convert.ToInt32(Math.Round(a));
					}
					text5 += num31.ToString("X4");
					text5 += num30.ToString("X4");
					this.sstServer.AnswerArgument = text5;
					break;
				}
				case SSTEthernetFrame.SSTCommands.DeviceName25_DX:
				{
					bool flag80 = argument == null;
					if (flag80)
					{
						string text10 = "";
						this.dbConnection.GetDeviceName(ref text10);
						bool flag81 = text10.Length > 25;
						if (flag81)
						{
							text10 = text10.Substring(0, 25);
						}
						text10 = text10.PadRight(25, ' ');
						this.sstServer.AnswerArgument = text10;
					}
					break;
				}
				case SSTEthernetFrame.SSTCommands.LineName25_DY:
				{
					bool flag82 = argument == null;
					if (flag82)
					{
						string text11 = "";
						this.dbConnection.GetLineName(ref text11);
						bool flag83 = text11.Length > 25;
						if (flag83)
						{
							text11 = text11.Substring(0, 25);
						}
						text11 = text11.PadRight(25, ' ');
						this.sstServer.AnswerArgument = text11;
					}
					break;
				}
				case SSTEthernetFrame.SSTCommands.ProductName25_PY:
				{
					bool flag84 = argument != null;
					if (flag84)
					{
						bool flag85 = argument is string;
						if (flag85)
						{
							int requestedProductNumber = -1;
							int.TryParse((string)argument, NumberStyles.HexNumber, null, out requestedProductNumber);
							bool flag86 = requestedProductNumber > 0;
							if (flag86)
							{
								List<ProductData> list8 = (from productData in this.productManager.ProductList
								where productData.ProductNumber == requestedProductNumber.ToString()
								select productData).ToList<ProductData>();
								bool flag87 = list8.Count > 0;
								if (flag87)
								{
									bool flag88 = list8[0].ProductName.Length <= 25;
									if (flag88)
									{
										this.sstServer.AnswerArgument = list8[0].ProductName;
										this.sstServer.AnswerArgument = this.sstServer.AnswerArgument.PadRight(25);
									}
									else
									{
										this.sstServer.AnswerArgument = list8[0].ProductName.Substring(0, 25);
									}
								}
								else
								{
									this.sstServer.AnswerArgument = "not valid!";
									this.sstServer.AnswerArgument = this.sstServer.AnswerArgument.PadRight(25);
								}
							}
							else
							{
								this.sstServer.AnswerArgument = this.sstServer.AnswerArgument.PadRight(25);
							}
						}
					}
					break;
				}
				case SSTEthernetFrame.SSTCommands.SystemCounterDetailed_SR:
				{
					bool flag89 = argument != null && argument is string;
					if (flag89)
					{
						string a2 = argument.ToString().Substring(0, 2);
						string s = argument.ToString().Substring(2, 4);
						string s2 = argument.ToString().Substring(6);
						bool flag90 = false;
						string tableName = "";
						uint num32;
						bool flag91 = !uint.TryParse(s, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out num32);
						if (flag91)
						{
							this.sstServer.AnswerArgument = "01";
							SSTEthernetServer sstethernetServer10 = this.sstServer;
							sstethernetServer10.AnswerArgument += 0.ToString("X48");
						}
						else
						{
							foreach (ProductData productData3 in this.productManager.ProductList)
							{
								bool flag92 = num32.ToString() == productData3.ProductNumber;
								if (flag92)
								{
									flag90 = true;
									tableName = productData3.TableName;
									break;
								}
							}
							bool flag93 = !flag90;
							if (flag93)
							{
								this.sstServer.AnswerArgument = "01";
								SSTEthernetServer sstethernetServer11 = this.sstServer;
								sstethernetServer11.AnswerArgument += 0.ToString("X48");
							}
							else
							{
								uint num33;
								bool flag94 = !uint.TryParse(s2, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out num33);
								if (flag94)
								{
									this.sstServer.AnswerArgument = "02";
									SSTEthernetServer sstethernetServer12 = this.sstServer;
									sstethernetServer12.AnswerArgument += 0.ToString("X48");
								}
								else
								{
									bool flag95 = a2 == "01";
									if (flag95)
									{
										CounterDetailed counterDetailed = new CounterDetailed();
										bool productIndexDetailedByTableName = this.dbConnection.GetProductIndexDetailedByTableName(tableName, ref counterDetailed);
										if (productIndexDetailedByTableName)
										{
											this.sstServer.AnswerArgument = "00";
											SSTEthernetServer sstethernetServer13 = this.sstServer;
											sstethernetServer13.AnswerArgument += counterDetailed.ToSSTAnswerString();
										}
										else
										{
											this.sstServer.AnswerArgument = "01";
											SSTEthernetServer sstethernetServer14 = this.sstServer;
											sstethernetServer14.AnswerArgument += 0.ToString("X48");
										}
									}
									else
									{
										bool flag96 = a2 == "02";
										if (flag96)
										{
											bool flag97 = false;
											Batch batch3 = new Batch();
											this.dbConnection.GetBatchesEnabled(ref flag97);
											bool flag98 = flag97;
											if (flag98)
											{
												batch3.BatchID = num33.ToString();
												bool batchIndex = this.dbConnection.GetBatchIndex(tableName, ref batch3);
												if (batchIndex)
												{
													this.sstServer.AnswerArgument = "00";
													SSTEthernetServer sstethernetServer15 = this.sstServer;
													sstethernetServer15.AnswerArgument += batch3.ToSSTAnswerString();
												}
												else
												{
													this.sstServer.AnswerArgument = "02";
													SSTEthernetServer sstethernetServer16 = this.sstServer;
													sstethernetServer16.AnswerArgument += 0.ToString("X48");
												}
											}
											else
											{
												this.sstServer.AnswerArgument = "02";
												SSTEthernetServer sstethernetServer17 = this.sstServer;
												sstethernetServer17.AnswerArgument += 0.ToString("X48");
											}
										}
									}
								}
							}
						}
					}
					break;
				}
				case SSTEthernetFrame.SSTCommands.ParamRaycon_PR:
				{
					bool flag99 = argument != null && argument is string;
					if (flag99)
					{
						string text12 = argument.ToString().Substring(0, 4);
						string s3 = argument.ToString().Substring(4, 8);
						string s4 = argument.ToString().Substring(12, 8);
						this.sstServer.AnswerArgument = "";
						uint num34;
						bool flag100 = !uint.TryParse(text12, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out num34);
						if (flag100)
						{
							this.sstServer.AnswerArgument = this.sstServer.AnswerArgument.PadRight(120, '0');
						}
						else
						{
							uint num35;
							bool flag101 = !uint.TryParse(s3, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out num35);
							if (flag101)
							{
								this.sstServer.AnswerArgument = this.sstServer.AnswerArgument.PadRight(120, '0');
							}
							else
							{
								uint num36;
								bool flag102 = !uint.TryParse(s4, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out num36);
								if (flag102)
								{
									this.sstServer.AnswerArgument = this.sstServer.AnswerArgument.PadRight(120, '0');
								}
								else
								{
									bool flag103 = text12 == "0001";
									if (flag103)
									{
										bool flag104 = false;
										this.dbConnection.GetBatchesEnabled(ref flag104);
										bool flag105 = !flag104;
										if (flag105)
										{
											this.sstServer.AnswerArgument = this.sstServer.AnswerArgument.PadRight(120, ' ');
										}
										else
										{
											bool flag106 = this.productManager.CurrentLoadedProduct.CurrentBatchName != null;
											if (flag106)
											{
												this.sstServer.AnswerArgument = this.productManager.CurrentLoadedProduct.CurrentBatchName;
												this.sstServer.AnswerArgument = this.sstServer.AnswerArgument.PadRight(40, ' ');
												SSTEthernetServer sstethernetServer18 = this.sstServer;
												sstethernetServer18.AnswerArgument += 0.ToString("X40");
												SSTEthernetServer sstethernetServer19 = this.sstServer;
												sstethernetServer19.AnswerArgument += 0.ToString("X40");
											}
										}
									}
									else
									{
										this.sstServer.AnswerArgument = this.sstServer.AnswerArgument.PadRight(120, ' ');
									}
								}
							}
						}
					}
					break;
				}
				case SSTEthernetFrame.SSTCommands.ImageExternalLocation_IL:
					this.sstServer.AnswerArgument = this.fileNamePathNetWorkSaving.PadRight(120, ' ');
					break;
				case SSTEthernetFrame.SSTCommands.ImageLocationForFtp_IF:
					this.sstServer.AnswerArgument = this.fileNamePathFtpSaving.PadRight(120, ' ');
					break;
				}
			}
			catch (Exception ex)
			{
				Exception ex3;
				Exception ex2 = ex3;
				Exception ex = ex2;
				bool flag107 = Application.Current != null;
				if (flag107)
				{
					Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(delegate()
					{
						Dialog.Show(LocalizationManager.Localize("txtError"), string.Concat(new string[]
						{
							"Error CommandFrameReceived: ",
							ex.Message,
							Environment.NewLine,
							"Command: ",
							command.ToString()
						}), DialogStyle.Error, DialogButtons.OK);
					}));
				}
			}
		}

		// Token: 0x0600242D RID: 9261 RVA: 0x000EB040 File Offset: 0x000E9240
		private void SetSSTImageTransferAnswer(object argument)
		{
			bool flag = argument != null && argument is string && ((string)argument).Length == 6;
			if (flag)
			{
				string a = ((string)argument).Substring(0, 2);
				string s = ((string)argument).Substring(2, 4);
				int num = 0;
				List<string> list = new List<string>();
				int.TryParse(s, NumberStyles.HexNumber, new CultureInfo("de-DE"), out num);
				bool flag2 = a == "01";
				if (flag2)
				{
					bool flag3 = Directory.Exists(StoreImageData.archiveDirectoryReject);
					if (flag3)
					{
						list.AddRange(Directory.GetFiles(StoreImageData.archiveDirectoryReject).ToList<string>());
					}
				}
				else
				{
					bool flag4 = a == "02";
					if (flag4)
					{
						bool flag5 = Directory.Exists(StoreImageData.archiveDirectoryThroughput);
						if (flag5)
						{
							list.AddRange(Directory.GetFiles(StoreImageData.archiveDirectoryThroughput).ToList<string>());
						}
					}
					else
					{
						bool flag6 = Directory.Exists(StoreImageData.archiveDirectoryThroughput);
						if (flag6)
						{
							list.AddRange(Directory.GetFiles(StoreImageData.archiveDirectoryThroughput).ToList<string>());
						}
						bool flag7 = Directory.Exists(StoreImageData.archiveDirectoryReject);
						if (flag7)
						{
							list.AddRange(Directory.GetFiles(StoreImageData.archiveDirectoryReject).ToList<string>());
						}
					}
				}
				list.Sort(new NamesComparerInverse());
				bool flag8 = num > 0 && list.Count >= num;
				if (flag8)
				{
					XRSImage image = this.imageArchiver.GetImage(list[num - 1]);
					this.sstServer.RayconImageBytes = image.GetXriRawBytes();
				}
			}
		}

		// Token: 0x0600242E RID: 9262 RVA: 0x000EB1D0 File Offset: 0x000E93D0
		private void SetSSTImageTransferExtendedAnswer(string argument)
		{
			XRSImage xrsimage = null;
			string a = argument.Substring(0, 2);
			string s = argument.Substring(2, 8);
			string a2 = argument.Substring(10, 2);
			uint num = 0U;
			List<string> list = new List<string>();
			byte b = 0;
			uint num2 = 0U;
			uint num3 = 0U;
			uint num4 = 0U;
			uint num5 = 0U;
			int num6 = 0;
			int num7 = 0;
			string fileName = "";
			this.dbConnection.GetRejectIndexGlobal(ref num5);
			this.dbConnection.GetThroughputIndexGlobal(ref num4);
			this.dbConnection.GetNSaveImagesGood(ref num6);
			this.dbConnection.GetNSaveImagesBad(ref num7);
			uint.TryParse(s, NumberStyles.HexNumber, new CultureInfo("de-DE"), out num);
			bool flag = a == "01" || a == "04";
			if (flag)
			{
				bool flag2 = num5 >= num && (ulong)(num5 - num) <= (ulong)((long)num7);
				if (flag2)
				{
					bool flag3 = Directory.Exists(StoreImageData.archiveDirectoryReject);
					if (flag3)
					{
						list.AddRange(Directory.GetFiles(StoreImageData.archiveDirectoryReject).ToList<string>());
					}
					list.Sort(new NamesComparerInverse());
					foreach (string text in list)
					{
						this.imageArchiver.GetVersionAndIndizesFromFile(text, ref b, ref num2, ref num3);
						bool flag4 = b >= 4 && num3 == num;
						if (flag4)
						{
							xrsimage = this.imageArchiver.GetImage(text);
							fileName = text;
							break;
						}
					}
				}
			}
			else
			{
				bool flag5 = a == "02" || a == "05";
				if (flag5)
				{
					bool flag6 = num4 >= num5 + num && num4 - num5 >= num && (ulong)(num4 - num5 - num) <= (ulong)((long)num6);
					if (flag6)
					{
						bool flag7 = Directory.Exists(StoreImageData.archiveDirectoryThroughput);
						if (flag7)
						{
							list.AddRange(Directory.GetFiles(StoreImageData.archiveDirectoryThroughput).ToList<string>());
						}
						list.Sort(new NamesComparerInverse());
						foreach (string text2 in list)
						{
							this.imageArchiver.GetVersionAndIndizesFromFile(text2, ref b, ref num2, ref num3);
							bool flag8 = b >= 4 && num2 >= num3 && num2 - num3 == num;
							if (flag8)
							{
								xrsimage = this.imageArchiver.GetImage(text2);
								fileName = text2;
								break;
							}
						}
					}
				}
				else
				{
					bool flag9 = Directory.Exists(StoreImageData.archiveDirectoryThroughput);
					if (flag9)
					{
						list.AddRange(Directory.GetFiles(StoreImageData.archiveDirectoryThroughput).ToList<string>());
					}
					bool flag10 = Directory.Exists(StoreImageData.archiveDirectoryReject);
					if (flag10)
					{
						list.AddRange(Directory.GetFiles(StoreImageData.archiveDirectoryReject).ToList<string>());
					}
					list.Sort(new NamesComparerInverse());
					foreach (string text3 in list)
					{
						this.imageArchiver.GetVersionAndIndizesFromFile(text3, ref b, ref num2, ref num3);
						bool flag11 = b >= 4 && num2 == num;
						if (flag11)
						{
							xrsimage = this.imageArchiver.GetImage(text3);
							fileName = text3;
							break;
						}
					}
				}
			}
			bool flag12 = xrsimage == null || xrsimage.Width <= 0 || xrsimage.Height <= 0;
			if (flag12)
			{
				xrsimage = XRSImage.CreateTestImage();
			}
			bool appendSTXAndETX = a != "00" && a != "01" && a != "02";
			bool flag13 = a2 == "00";
			if (flag13)
			{
				this.sstServer.RayconImageBytes = this.GetSSTXRIBytesToSend(xrsimage, appendSTXAndETX);
			}
			else
			{
				bool flag14 = a2 == "01";
				if (flag14)
				{
					this.sstServer.RayconImageBytes = this.GetSSTPNGBytesToSend(xrsimage, appendSTXAndETX, fileName);
				}
			}
		}

		// Token: 0x0600242F RID: 9263 RVA: 0x000EB600 File Offset: 0x000E9800
		private void LogImageExtendedRequest(int imageIndex, string strImageType, string strXRIOrPNG)
		{
			string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\requests.txt";
			string text = "Unknown type";
			bool flag = strImageType == "00";
			if (flag)
			{
				text = "Throughput";
			}
			else
			{
				bool flag2 = strImageType == "01";
				if (flag2)
				{
					text = "Reject";
				}
				else
				{
					bool flag3 = strImageType == "02";
					if (flag3)
					{
						text = "Good";
					}
				}
			}
			string text2 = "PNG";
			bool flag4 = strXRIOrPNG == "00";
			if (flag4)
			{
				text2 = "XRI";
			}
			else
			{
				bool flag5 = strXRIOrPNG == "01";
				if (flag5)
				{
					text2 = "PNG";
				}
			}
			StreamWriter streamWriter = new StreamWriter(path, true);
			streamWriter.Write(string.Concat(new string[]
			{
				DateTime.Now.ToString("dd:MM:yyyy HH:mm:ss.fff"),
				";\tFormat: ",
				text2,
				";\tType: ",
				text,
				";\tIndex: ",
				imageIndex.ToString(),
				Environment.NewLine
			}));
			streamWriter.Close();
		}

		// Token: 0x06002430 RID: 9264 RVA: 0x000EB71C File Offset: 0x000E991C
		private byte[] GetSSTXRIBytesToSend(XRSImage imageToSend, bool appendSTXAndETX)
		{
			List<byte> list = new List<byte>();
			if (appendSTXAndETX)
			{
				list.Add(2);
			}
			list.AddRange(imageToSend.GetXriRawBytes());
			if (appendSTXAndETX)
			{
				list.Add(3);
				list = this.ReplaceSSTControlCharacters(list);
			}
			return list.ToArray();
		}

		// Token: 0x06002431 RID: 9265 RVA: 0x000EB76C File Offset: 0x000E996C
		private byte[] GetSSTPNGBytesToSend(XRSImage imageToSend, bool appendSTXAndETX, string fileName)
		{
			bool saveEdgePixelInPNG = false;
			this.dbConnection.GetSaveEdgePixelInPNG(ref saveEdgePixelInPNG);
			bool flag = !appendSTXAndETX;
			byte[] result;
			if (flag)
			{
				result = PngHelper.GetPngBytes(imageToSend, saveEdgePixelInPNG, Path.GetFileNameWithoutExtension(fileName));
			}
			else
			{
				List<byte> list = new List<byte>();
				list.Add(2);
				list.AddRange(PngHelper.GetPngBytes(imageToSend, saveEdgePixelInPNG, Path.GetFileNameWithoutExtension(fileName)));
				list.Add(3);
				list = this.ReplaceSSTControlCharacters(list);
				result = list.ToArray();
			}
			return result;
		}

		// Token: 0x06002432 RID: 9266 RVA: 0x000EB7E0 File Offset: 0x000E99E0
		private List<byte> ReplaceSSTControlCharacters(List<byte> bytes)
		{
			List<byte> list = new List<byte>();
			bool flag = bytes.Count > 0;
			if (flag)
			{
				list.Add(bytes[0]);
				for (int i = 1; i < bytes.Count - 1; i++)
				{
					bool flag2 = bytes[i] == 2 || bytes[i] == 3 || bytes[i] == 64;
					if (flag2)
					{
						list.Add(64);
						list.Add(bytes[i] / 16 + 48);
						list.Add(bytes[i] % 16 + 48);
					}
					else
					{
						list.Add(bytes[i]);
					}
				}
				list.Add(bytes[bytes.Count - 1]);
			}
			return list;
		}

		// Token: 0x06002433 RID: 9267 RVA: 0x000EB8BC File Offset: 0x000E9ABC
		private byte[] GetFakePNGImage()
		{
			WriteableBitmap writeableBitmap = new WriteableBitmap(1, 1, 96.0, 96.0, PixelFormats.Bgra32, null);
			PngBitmapEncoder pngBitmapEncoder = new PngBitmapEncoder();
			writeableBitmap.Freeze();
			pngBitmapEncoder.Frames.Add(BitmapFrame.Create(writeableBitmap));
			pngBitmapEncoder.Frames[0].Freeze();
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				pngBitmapEncoder.Save(memoryStream);
				result = memoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x06002434 RID: 9268 RVA: 0x000EB958 File Offset: 0x000E9B58
		private void SetSSTLogbookEntryAnswer(uint entryNumber)
		{
			bool flag = entryNumber == uint.MaxValue;
			if (flag)
			{
				this.dbConnection.GetLBIndex(ref entryNumber);
			}
			List<string> list = new List<string>();
			bool logbookEntry = this.dbConnection.GetLogbookEntry(entryNumber, list);
			if (logbookEntry)
			{
				SSTLogbookEntry sstlogbookEntry = new SSTLogbookEntry(list);
				this.sstServer.AnswerArgument = sstlogbookEntry.GetSSTAnswerString();
			}
		}

		// Token: 0x06002435 RID: 9269 RVA: 0x000EB9B0 File Offset: 0x000E9BB0
		private void UpdateDateTime(object sender, EventArgs e)
		{
			bool flag = !LocalizationManager.CurrentCulture.IsNeutralCulture;
			if (flag)
			{
				bool flag2 = !(LocalizationManager.CurrentCulture.ToString() == "en-US") && !(LocalizationManager.CurrentCulture.ToString() == "en-CA");
				if (flag2)
				{
					this.Time = DateTime.Now.ToString("ddd, dd. MMM - HH:mm", LocalizationManager.CurrentCulture);
				}
				else
				{
					this.Time = DateTime.Now.ToString("ddd, dd. MMM - hh:mm tt", LocalizationManager.CurrentCulture);
				}
			}
			else
			{
				this.Time = DateTime.Now.ToString("ddd, dd. MMM - HH:mm", new CultureInfo("en-GB"));
			}
		}

		// Token: 0x06002436 RID: 9270 RVA: 0x000EBA68 File Offset: 0x000E9C68
		private void UpdateVibroStarted(object dummy, EventArgs e)
		{
			bool bulkModeActive = this.BulkModeActive;
			if (bulkModeActive)
			{
				bool flag = this.parConnected;
				if (flag)
				{
					this.VibroStarted = (this.parHandling.ReadPar(Definition.vibroStartedPath) == "1");
				}
				else
				{
					this.VibroStarted = false;
				}
			}
		}

		// Token: 0x06002437 RID: 9271 RVA: 0x000EBAB8 File Offset: 0x000E9CB8
		private void sendTestcardSimImagesWithActSens(object dummy, EventArgs e)
		{
			bool flag = this.parHandling != null && this.testcardsimservice.MonitoringSensIsActive && this.testcardsimservice.MonitoringSensCheckMachineStates(this.machineState) && this.parHandling.ReadPar(Definition.autoKVAdjustmentPath) == "1";
			if (flag)
			{
				bool flag2 = this.testcardsimservice.setOrCheckImgToSimulateIsNew(false);
				if (flag2)
				{
					int ballsizeVA = Convert.ToInt32(this.parHandling.ReadPar(Definition.simulationActSensVA));
					int ballsizeGlass = Convert.ToInt32(this.parHandling.ReadPar(Definition.simulationActSensGlass));
					this.testcardsimservice.InitSimulation();
					this.testcardsimservice.TestcardSimAfterTimeIntervall(ballsizeVA, ballsizeGlass, true, true);
					this.testcardsimservice.MonitoringSensIsActive = true;
					this.testcardsimservice.MonitoringSensIsInProgress = true;
				}
			}
		}

		// Token: 0x06002438 RID: 9272 RVA: 0x000EBB88 File Offset: 0x000E9D88
		private void AuditRequired_NextCheckElapsed()
		{
			bool flag = this.machineState != null && this.machineState.State == MachineStates.Inspecting && this.xrayState.Calibrated;
			if (flag)
			{
				this.ExecuteAuditCheck();
			}
			else
			{
				this.auditNeeded = true;
			}
		}

		// Token: 0x06002439 RID: 9273 RVA: 0x000EBBD0 File Offset: 0x000E9DD0
		private void RegisterLocalizedText()
		{
			this.StrStartVibro = new BoundLocalizedText("msgStartVibro");
			this.StrStopVibro = new BoundLocalizedText("msgStopVibro");
			this.StrValveActivity = new BoundLocalizedText("msgValveActivity");
			this.StrSensitivityPercent = new BoundLocalizedText("txtSensitivityPercent");
			this.StrVibroSpeed = new BoundLocalizedText("txtVibroSpeed");
			this.StrStartXray = new BoundLocalizedText("txtXraysStart");
			this.StrActTestcardSim = new BoundLocalizedText("txtActual");
			this.StrComplTestcardSim = new BoundLocalizedText("txtCompliance");
			this.StrSensTestcardSim = new BoundLocalizedText("txtSensitivity");
			this.StrVATestcardSim = new BoundLocalizedText("txtVA");
			this.StrGlassTestcardSim = new BoundLocalizedText("txtGlass");
			this.StrCompliance = new BoundLocalizedText("txtComplianceMsg");
			this.StrNoCompliance = new BoundLocalizedText("txtNoComplianceMsg");
			this.StrRejectBulk = new BoundLocalizedText("txtRejectBulkPerMinute");
			this.SystemIsCompliant = this.StrCompliance.LocalizedText;
			this.ComplTextForeground = true;
			this.ComplTextVAForeground = true;
			this.ComplTextGlassForeground = true;
		}

		// Token: 0x0600243A RID: 9274 RVA: 0x000EBCF8 File Offset: 0x000E9EF8
		public void SetControlRights(Button btnLogbook, Button btnLearn, Button btnProduct, Button btnSettings, Button btnBulkStartLiveMode, Button btnBulkStartObjectMode, Button btnBulkRetrain, Button btnIncreaseSensitivity, Button btnDecreaseSensitivity, Button btnIncreaseVibroSpeed, Button btnDecreaseVibroSpeed, Button btnUnlockBin, Border SliderBulk)
		{
			MainWindowViewModel.accessRightsManager.RegisterControl(btnLogbook, this.rightLogbook, this.rightLogbook);
			MainWindowViewModel.accessRightsManager.RegisterControl(btnLearn, this.rightTrain, this.rightTrain);
			MainWindowViewModel.accessRightsManager.RegisterControl(btnProduct, this.rightProductChange, this.rightProductChange);
			MainWindowViewModel.accessRightsManager.RegisterControl(btnSettings, this.rightSettings, this.rightSettings);
			MainWindowViewModel.accessRightsManager.RegisterControl(btnBulkStartLiveMode, this.rightTrain, this.rightSettings);
			MainWindowViewModel.accessRightsManager.RegisterControl(btnBulkStartObjectMode, this.rightTrain, this.rightSettings);
			MainWindowViewModel.accessRightsManager.RegisterControl(btnBulkRetrain, this.rightTrain, this.rightTrain);
			MainWindowViewModel.accessRightsManager.RegisterControl(btnUnlockBin, Rejecter.doVerifyTestRight, Rejecter.doVerifyTestRight);
			MainWindowViewModel.accessRightsManager.SetProperties();
		}

		// Token: 0x0600243B RID: 9275 RVA: 0x000EBDD4 File Offset: 0x000E9FD4
		private void TestDatabase()
		{
			bool flag = false;
			bool flag2 = this.dbConnection.Connection != null;
			if (flag2)
			{
				flag = true;
			}
			bool flag3 = File.Exists(DBConnection.databasePath);
			if (flag3)
			{
				bool flag4 = true;
				bool flag5 = !flag;
				if (flag5)
				{
					flag4 = this.dbConnection.Connect(DBConnection.databasePath);
				}
				bool flag6 = this.dbConnection.TestDatabase();
				this.dbConnection.CompressDatabase();
				bool flag7 = flag4 && flag6;
				if (flag7)
				{
					this.dbConnection.CreateBackup(DBConnection.databasePath, DBConnection.backupPath);
				}
				else
				{
					bool flag8 = File.Exists(DBConnection.backupPath);
					if (flag8)
					{
						this.dbConnection.CloseConnection();
						bool flag9 = this.dbConnection.RestoreBackup(DBConnection.databasePath, DBConnection.backupPath);
						bool flag10 = this.dbConnection.Connect(DBConnection.databasePath);
						bool flag11 = this.dbConnection.TestDatabase();
						bool flag12 = !flag9 || !flag10 || !flag11;
						if (flag12)
						{
							Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
							{
								Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtRestoreFailed"), DialogStyle.Error);
							}));
						}
					}
					else
					{
						Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
						{
							Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtRestoreFailed"), DialogStyle.Error);
						}));
					}
				}
			}
			else
			{
				bool flag13 = File.Exists(DBConnection.backupPath);
				if (flag13)
				{
					this.dbConnection.CloseConnection();
					bool flag14 = this.dbConnection.RestoreBackup(DBConnection.databasePath, DBConnection.backupPath);
					bool flag15 = this.dbConnection.Connect(DBConnection.databasePath);
					bool flag16 = this.dbConnection.TestDatabase();
					bool flag17 = !flag14 || !flag15 || !flag16;
					if (flag17)
					{
						Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
						{
							Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtRestoreFailed"));
						}));
					}
				}
			}
			bool flag18 = !flag && this.dbConnection.Connection != null;
			if (flag18)
			{
				this.dbConnection.CloseConnection();
			}
		}

		// Token: 0x0600243C RID: 9276 RVA: 0x000EC000 File Offset: 0x000EA200
		private void xrayState_XRayPowerOff()
		{
			bool flag = Application.Current != null && !Application.Current.Dispatcher.CheckAccess();
			if (flag)
			{
				Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate()
				{
					foreach (LaneXri laneXri2 in this.ShownLanes)
					{
						laneXri2.Image = null;
					}
					this.SensitivityVisible = false;
					this.parHandling.WriteValue(Definition.checkArcsEnable, "0");
				}));
			}
			else
			{
				foreach (LaneXri laneXri in this.ShownLanes)
				{
					laneXri.Image = null;
				}
				this.SensitivityVisible = false;
				this.parHandling.WriteValue(Definition.checkArcsEnable, "0");
			}
			LogbookManager.AddEntry(SSTEntryCode.XrayTurnedOff);
			this.videojetPrinter.Disconnect();
			this.ShowNext = false;
			bool flag2 = !this.StandardMode;
			if (flag2)
			{
				this.IsImageBoxVisible = false;
				this.IsImageBoxVisibleBigImage = false;
			}
		}

		// Token: 0x0600243D RID: 9277 RVA: 0x000EC0F0 File Offset: 0x000EA2F0
		private void xrayState_XRayPowerOn()
		{
			bool flag = Application.Current != null && !Application.Current.Dispatcher.CheckAccess();
			if (flag)
			{
				Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate()
				{
					foreach (LaneXri laneXri3 in this.ShownLanes)
					{
						laneXri3.TempImage = ((laneXri3.Image != null) ? laneXri3.Image.DeepCopy() : null);
					}
					foreach (LaneXri laneXri4 in this.ShownLanes)
					{
						laneXri4.Image = null;
					}
					bool flag3 = !this.standardMode;
					if (flag3)
					{
						this.SensitivityVisible = false;
					}
				}));
			}
			else
			{
				foreach (LaneXri laneXri in this.ShownLanes)
				{
					laneXri.TempImage = ((laneXri.Image != null) ? laneXri.Image.DeepCopy() : null);
				}
				foreach (LaneXri laneXri2 in this.ShownLanes)
				{
					laneXri2.Image = null;
				}
				bool flag2 = !this.standardMode;
				if (flag2)
				{
					this.SensitivityVisible = false;
				}
			}
			LogbookManager.AddEntry(SSTEntryCode.XrayTurnedOn);
		}

		// Token: 0x0600243E RID: 9278 RVA: 0x000EC204 File Offset: 0x000EA404
		private void xrayState_CalibrationDone()
		{
			bool flag = Application.Current != null && !Application.Current.Dispatcher.CheckAccess();
			if (flag)
			{
				Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate()
				{
					foreach (LaneXri laneXri2 in this.ShownLanes)
					{
						bool flag5 = laneXri2.TempImage != null;
						if (flag5)
						{
							laneXri2.Image = laneXri2.TempImage.DeepCopy();
							laneXri2.TempImage = null;
						}
					}
					bool flag6 = !this.standardMode;
					if (flag6)
					{
						this.SensitivityVisible = true;
					}
				}));
			}
			else
			{
				foreach (LaneXri laneXri in this.ShownLanes)
				{
					bool flag2 = laneXri.TempImage != null;
					if (flag2)
					{
						laneXri.Image = laneXri.TempImage.DeepCopy();
						laneXri.TempImage = null;
					}
				}
				bool flag3 = !this.standardMode;
				if (flag3)
				{
					this.SensitivityVisible = true;
				}
			}
			bool enabled = this.videojetPrinter.Enabled;
			if (enabled)
			{
				bool flag4 = !this.videojetPrinter.Connect();
				if (flag4)
				{
					this.errorSurvey.PostMessage(SSTEntryCode.Printer, 0);
				}
			}
		}

		// Token: 0x0600243F RID: 9279 RVA: 0x000EC308 File Offset: 0x000EA508
		private void localizationManager_LanguageChanged()
		{
			string b = null;
			this.messageSystem.RemoveAllMessages();
			this.dbConnection.GetLanguage(ref b);
			string text = LocalizationManager.CurrentCulture.ToString();
			string a = text;
			if (!(a == "en-US"))
			{
				if (!(a == "en-CA") && !(a == "fr-CA"))
				{
					this.XRayImage = XrsImage.ImageConverter.ToBitmap("/img/xray.png");
					bool flag = LocalizationManager.CurrentCulture.ToString() != b;
					if (flag)
					{
						this.dbConnection.SetSpeedMetric(1);
						this.dbConnection.SetLengthMetric(true);
					}
				}
				else
				{
					this.XRayImage = XrsImage.ImageConverter.ToBitmap("/img/xrayCanada.png");
				}
			}
			else
			{
				this.XRayImage = XrsImage.ImageConverter.ToBitmap("/img/xrayUSA.png");
				bool flag2 = LocalizationManager.CurrentCulture.ToString() != b;
				if (flag2)
				{
					this.dbConnection.SetSpeedMetric(0);
					this.dbConnection.SetLengthMetric(false);
				}
			}
			this.dbConnection.SetLanguage(LocalizationManager.CurrentCulture.ToString());
			XRSImageBox.appCulture = LocalizationManager.CurrentCulture;
			bool flag3 = this.ShownLanes != null;
			if (flag3)
			{
				bool flag4 = false;
				bool flag5 = false;
				this.dbConnection.GetMultiLaneView(ref flag4);
				bool flag6 = flag4;
				if (flag6)
				{
					foreach (LaneXri laneXri in this.ShownLanes)
					{
						laneXri.UpdateLaneName();
					}
				}
				else
				{
					this.dbConnection.GetMultiProductView(ref flag5);
					bool flag7 = flag5;
					if (flag7)
					{
						List<LaneXri> list = (from lane in this.ShownLanes
						where lane.Group == 0
						select lane).ToList<LaneXri>();
						bool flag8 = list.Count > 0;
						if (flag8)
						{
							list[0].UpdateLaneName();
						}
					}
				}
			}
		}

		// Token: 0x06002440 RID: 9280 RVA: 0x000EC50C File Offset: 0x000EA70C
		private void parHandling_Disconnected()
		{
			bool flag = this.parConnected;
			if (flag)
			{
				this.parConnected = false;
				this.errorSurvey.ParameterPortOk = false;
				this.FirstApplicationLoaded = true;
			}
		}

		// Token: 0x06002441 RID: 9281 RVA: 0x000EC544 File Offset: 0x000EA744
		private void imageTransfer_Disconnected()
		{
			bool flag = this.imageConnected;
			if (flag)
			{
				this.imageConnected = false;
				this.errorSurvey.ImagePortOk = false;
			}
		}

		// Token: 0x06002442 RID: 9282 RVA: 0x000EC574 File Offset: 0x000EA774
		private void parHandling_Connected()
		{
			try
			{
				bool flag = false;
				bool flag2 = !this.parametersInitialized;
				if (flag2)
				{
					this.TestDatabase();
				}
				this.parametersInitialized = false;
				List<string> list = new List<string>();
				bool flag3 = !File.Exists(DBConnection.databasePath) && !File.Exists(DBConnection.backupPath);
				List<string> pathes;
				if (flag3)
				{
					pathes = this.parHandling.GetPathes(list);
				}
				else
				{
					pathes = this.parHandling.GetPathes(null);
				}
				this.parConnected = true;
				bool flag4 = this.errorSurvey != null;
				if (flag4)
				{
					this.errorSurvey.ParameterPortOk = true;
				}
				string text = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\";
				bool flag5 = !File.Exists(DBConnection.databasePath) && !File.Exists(DBConnection.backupPath);
				if (flag5)
				{
					flag = true;
					this.SetDefaultProductValues(pathes, list);
					this.dbConnection.CreateDatabase(DBConnection.databasePath, pathes, list);
					this.logbook = new Logbook(this.dbConnection);
					LogbookManager.logbook = this.logbook;
					this.dbConnection.GetDeviceAddress(ref this.deviceAddress);
					this.sstServer.DeviceAddress = this.deviceAddress;
					bool flag6 = Application.Current.Dispatcher != null;
					if (flag6)
					{
						Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
						{
							this.conveyor = new Conveyor(this.dbConnection, this.parHandling, this.xrayState);
							this.ConveyorViewModel = new ConveyorViewModel(this.conveyor, MainWindowViewModel.accessRightsManager, this.dbConnection, this.machineState, false, this.dbDatalogs, this.parHandling);
							this.auditSettings = new AuditCheckSettings(this.dbConnection, this.parHandling);
							this.errorSurvey.AuditRequired = this.auditSettings.AuditRequired;
							this.auditSettings.AuditRequired.NextCheckElapsed += this.AuditRequired_NextCheckElapsed;
							this.errorHandling.UpdateFromDatabase();
						}));
					}
					this.aiService.CreateDefaultTableAI(false);
				}
				this.CreateSubProductDefaultTable(this.parHandling);
				bool flag7 = !File.Exists(DBDataLogs.databasePath);
				if (flag7)
				{
				}
				bool flag8 = this.conveyor == null;
				if (flag8)
				{
					this.conveyor = new Conveyor(this.dbConnection, this.parHandling, this.xrayState);
					this.ConveyorViewModel = new ConveyorViewModel(this.conveyor, MainWindowViewModel.accessRightsManager, this.dbConnection, this.machineState, false, this.dbDatalogs, this.parHandling);
				}
				this.LoadGlobalValues();
				bool flag9 = !this.LoadCurrentApplication();
				if (flag9)
				{
					Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
					{
						Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtDatabaseError"), DialogStyle.Error, DialogButtons.OK);
					}));
				}
				DetectorType detectorType = DetectorType.Sesotec;
				this.dbConnection.GetDetectorType(ref detectorType);
				bool flag10 = detectorType == DetectorType.DTNetDE && !this.productManager.CurrentLoadedProduct.IsSingleProduct;
				if (flag10)
				{
					bool flag11 = this.productManager != null && this.productManager.CurrentLoadedProduct != null;
					if (flag11)
					{
						bool flag12 = this.productManager.CurrentLoadedProduct.ProductType == ProductType.SingleProduct;
						if (flag12)
						{
							this.LoadCurrentMaterialCurve(0);
						}
						else
						{
							for (int i = 0; i < this.productManager.CurrentLoadedProduct.SubProducts.Count; i++)
							{
								this.LoadCurrentMaterialCurve((int)this.productManager.CurrentLoadedProduct.SubProducts[i].Group);
							}
						}
					}
				}
				bool flag13 = flag;
				if (flag13)
				{
					this.productManager = new ProductManager(this.dbConnection, this.parHandling, this.imageTransfer, this.machineState, this.conveyor, this.auditSettings, MainWindowViewModel.accessRightsManager, this.imageArchiver, this.testcardsimservice, this.rejecterActivity);
					this.productManager.ProductChanged += this.prodChange_ProductChanged;
					this.bulksensitivity.SetProductManager(this.productManager);
				}
				this.InitializeWagoIP();
				string value = null;
				this.dbConnection.GetDatabaseVersion(ref value);
				this.parHandling.WriteValue(MainWindowViewModel.guiVersionPath, Definition.guiVersion, false);
				this.parHandling.WriteValue(MainWindowViewModel.guiVersionBuildPath, Definition.guiVersionBuild, false);
				this.parHandling.WriteValue(Definition.imageVersionWindowsPath, MainWindowViewModel.GetWindowsImageVersion(), false);
				this.parHandling.WriteValue(Definition.imageVersionWindowsHardwarePath, MainWindowViewModel.GetWindowsImageVersionHardware(), false);
				this.parHandling.WriteValue(Definition.imageVersionApplicationPath, this.GetApplicationImageVersion(), false);
				this.parHandling.WriteValue(MainWindowViewModel.databaseVersionPath, value, false);
				this.parHandling.WriteValue(Definition.guiDesignPath, this.SelectedBranding.CompanyName, false);
				this.parHandling.WriteValue(Definition.bulkOverlapImageShowPath, XRSImageBoxViewModel.BulkOverlapImageShow.ToString(), false);
				MainWindowViewModel.elinosVersion = this.parHandling.ReadPar(MainWindowViewModel.elinosVersionPath);
				MainWindowViewModel.elinosVersionBuild = this.parHandling.ReadPar(MainWindowViewModel.elinosVersionBuildPath);
				bool flag14 = this.parHandling.ReadPar("/testSuite/Active/Value") == "1";
				if (flag14)
				{
					this.xrayState.TestSuiteActive = true;
				}
				this.parametersInitialized = true;
				this.auditSettings.Initialized = true;
				this.auditSettings.AuditRequired.Initialized = true;
				this.SetIPCTimeIfNeeded();
				bool flag15 = this.testcardsimservice != null;
				if (flag15)
				{
					this.testcardsimservice.SetProperties();
				}
				bool flag16 = flag;
				if (flag16)
				{
					bool flag17 = Application.Current != null;
					if (flag17)
					{
						Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
						{
							Process.Start(Restore.restoreProcessPath, "Restart");
							Application.Current.Shutdown();
						}));
					}
				}
			}
			catch (Exception ex)
			{
				Exception ex3;
				Exception ex2 = ex3;
				Exception ex = ex2;
				bool flag18 = Application.Current != null;
				if (flag18)
				{
					Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
					{
						Dialog.Show("Error: parHandling_Connected", ex.Message, DialogStyle.Error, DialogButtons.OK);
					}));
				}
			}
			this.FirstApplicationLoaded = true;
			bool flag19 = this.parHandling.ReadPar(XrayState.xrayStarterPath) == "1";
			if (flag19)
			{
				this.InitializeRunningMachine();
			}
		}

		// Token: 0x06002443 RID: 9283 RVA: 0x000ECB50 File Offset: 0x000EAD50
		private void CreateSubProductDefaultTable(ParameterHandling parHandling)
		{
			DetectorType detectorType = DetectorType.DT;
			this.dbConnection.GetDetectorType(ref detectorType);
			string a = "";
			this.dbConnection.GetParameterValue(Definition.checkComparerDefaultSubProductPath, DBConnection.defaultSubApplication, ref a);
			MachineType machineType = MachineType.MachineTypeUndefined;
			this.dbConnection.GetMachineType(ref machineType);
			bool flag = this.dbConnection.SubProductDefaultTableExists() && (machineType == MachineType.RayconDPlus || machineType == MachineType.RayconEX2A) && detectorType == DetectorType.Sesotec && a == "";
			if (flag)
			{
				this.dbConnection.DeleteTable(DBConnection.defaultSubApplication);
			}
			bool flag2 = !this.dbConnection.SubProductDefaultTableExists();
			if (flag2)
			{
				List<string> list = new List<string>();
				parHandling.AddGroupToParameterServer(Definition.defaultSubProductGroup);
				parHandling.AddRegionToParameterServer(Definition.defaultSubProductGroup);
				List<string> pathes = parHandling.GetPathes(list);
				parHandling.RemoveGroupFromParameterServer(Definition.defaultSubProductGroup);
				parHandling.RemoveRegionFromParameterServer(Definition.defaultSubProductGroup);
				for (int i = 0; i < pathes.Count; i++)
				{
					bool flag3 = !pathes[i].StartsWith(Definition.filterMainPath + Definition.defaultSubProductGroup.ToString()) || pathes[i].StartsWith(Definition.filterMainPath + Definition.defaultSubProductGroup.ToString() + "/global");
					if (flag3)
					{
						pathes.RemoveAt(i);
						list.RemoveAt(i);
						i--;
					}
				}
				this.dbConnection.CreateDefaultSubProductTable(pathes, list);
			}
		}

		// Token: 0x06002444 RID: 9284 RVA: 0x000ECCE0 File Offset: 0x000EAEE0
		private void InitializeRunningMachine()
		{
			DetectorType detectorType = DetectorType.DT;
			this.dbConnection.GetDetectorType(ref detectorType);
			string path = (detectorType == DetectorType.DT || detectorType == DetectorType.DTNetSE || detectorType == DetectorType.DTNetDE || detectorType == DetectorType.DTNetTDI) ? Definition.dtMCUTemperaturePath : Definition.detectorSesotecTemperaturePath;
			bool flag = this.parHandling.ReadPar(Definition.isLiveCalibrationActivePath) == "1" || this.parHandling.ReadPar(Definition.isLiveCalibrationActivePath) == "2";
			int temperature;
			int.TryParse(this.parHandling.ReadPar(path), out temperature);
			this.pixelCorrection.CorrectOffset(temperature);
			bool flag2 = flag;
			if (flag2)
			{
				this.pixelCorrection.CorrectGainLiveCalib();
			}
			else
			{
				this.pixelCorrection.CorrectGain(temperature);
			}
			this.parHandling.WriteValue(XRayOnProcessing.manualOffsetCalibrationPath, "1");
			this.parHandling.WriteValue(XRayOnProcessing.manualGainCalibrationPath, "1");
		}

		// Token: 0x06002445 RID: 9285 RVA: 0x000ECDD0 File Offset: 0x000EAFD0
		public static string GetWindowsImageVersion()
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = File.Exists(Definition.imageVersionFilename);
			if (flag)
			{
				StreamReader streamReader = new StreamReader(Definition.imageVersionFilename);
				stringBuilder.Append(streamReader.ReadLine());
				bool flag2 = stringBuilder.Length > Definition.imageVersionIdentifier.Length + 2;
				if (flag2)
				{
					stringBuilder = stringBuilder.Remove(0, Definition.imageVersionIdentifier.Length + 2);
				}
				for (int i = 0; i < stringBuilder.Length; i++)
				{
					bool flag3 = stringBuilder[i] == '\n' || stringBuilder[i] == '\r';
					if (flag3)
					{
						stringBuilder = stringBuilder.Remove(i, 1);
					}
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06002446 RID: 9286 RVA: 0x000ECE90 File Offset: 0x000EB090
		public static string GetWindowsImageVersionHardware()
		{
			StringBuilder stringBuilder = new StringBuilder();
			try
			{
				bool flag = File.Exists(Definition.imageVersionFilename);
				if (flag)
				{
					StreamReader streamReader = new StreamReader(Definition.imageVersionFilename);
					streamReader.ReadLine();
					stringBuilder.Append(streamReader.ReadLine());
					bool flag2 = stringBuilder.Length > Definition.imageVersionHardwareIdentifier.Length + 2;
					if (flag2)
					{
						stringBuilder = stringBuilder.Remove(0, Definition.imageVersionHardwareIdentifier.Length + 2);
					}
					for (int i = 0; i < stringBuilder.Length; i++)
					{
						bool flag3 = stringBuilder[i] == '\n' || stringBuilder[i] == '\r';
						if (flag3)
						{
							stringBuilder = stringBuilder.Remove(i, 1);
						}
					}
					bool flag4 = stringBuilder.Length == 0;
					if (flag4)
					{
						bool flag5 = Convert.ToInt32(MainWindowViewModel.GetWindowsImageVersion()) < 30;
						if (flag5)
						{
							stringBuilder.Append("PPC2100");
						}
						else
						{
							stringBuilder.Append("PPC3100");
						}
					}
				}
			}
			catch
			{
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06002447 RID: 9287 RVA: 0x000ECFB0 File Offset: 0x000EB1B0
		private string GetApplicationImageVersion()
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = File.Exists(Definition.imageVersionApplicationFilename);
			if (flag)
			{
				StreamReader streamReader = new StreamReader(Definition.imageVersionApplicationFilename);
				stringBuilder.Append(streamReader.ReadLine());
				bool flag2 = stringBuilder.Length > Definition.imageVersionApplicationIdentifier.Length + 2;
				if (flag2)
				{
					stringBuilder = stringBuilder.Remove(0, Definition.imageVersionApplicationIdentifier.Length + 2);
				}
				for (int i = 0; i < stringBuilder.Length; i++)
				{
					bool flag3 = stringBuilder[i] == '\n' || stringBuilder[i] == '\r';
					if (flag3)
					{
						stringBuilder = stringBuilder.Remove(i, 1);
					}
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06002448 RID: 9288 RVA: 0x000ED070 File Offset: 0x000EB270
		private void InitializeWagoIP()
		{
			string value = "";
			this.dbConnection.GetIPWago(ref value);
			this.parHandling.WriteValue(MainWindowViewModel.ipWagoPath, value, false);
		}

		// Token: 0x06002449 RID: 9289 RVA: 0x000ED0A5 File Offset: 0x000EB2A5
		private void restore_Rebooting()
		{
			this.UpdateProgress();
		}

		// Token: 0x0600244A RID: 9290 RVA: 0x000ED0AF File Offset: 0x000EB2AF
		private void restore_BeforeReboot()
		{
			this.SleepMode();
		}

		// Token: 0x0600244B RID: 9291 RVA: 0x000ED0B9 File Offset: 0x000EB2B9
		private void SleepMode()
		{
			Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
			{
				MainWindowViewModel.accessRightsManager.LogoutUser();
				this.messageSystem.RemoveAllMessages();
				this.imageTransfer.Connected -= this.imageTransfer_Connected;
				this.parHandling.Connected -= this.parHandling_Connected;
				this.modelTransfer.Connected -= this.modelTransfer_Connected;
				this.imageTransfer.Disconnected -= this.imageTransfer_Disconnected;
				this.parHandling.Disconnected -= this.parHandling_Disconnected;
				this.modelTransfer.Disconnected -= this.modelTransfer_Disconnected;
				this.errorSurvey.StopSurvey();
				this.errorSurvey.StopCheckingXRayOperatingHours();
				this.xrayState.Stop();
				this.videojetPrinter.Disconnect();
				this.parHandling.Connected += this.parHandling_ConnectedAfterReboot;
				this.progressRestore = new Progress(LocalizationManager.Localize("msgRestartingImagePc"), false);
				this.progressRestore.Show();
			}));
		}

		// Token: 0x0600244C RID: 9292 RVA: 0x000ED0DC File Offset: 0x000EB2DC
		private void UpdateProgress()
		{
			int progValue = 0;
			int num = 120000;
			int num2 = 0;
			Stopwatch stopwatch = Stopwatch.StartNew();
			this.progressRestoreUpdateRunning = true;
			while (this.progressRestoreUpdateRunning && num2 < num)
			{
				bool flag = this.progressRestore != null;
				if (flag)
				{
					bool flag2 = Application.Current != null;
					if (flag2)
					{
						Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
						{
							this.progressRestore.progBar.Value = (double)progValue;
						}));
					}
					num2 = (int)stopwatch.ElapsedMilliseconds;
					progValue = num2 * 100 / num;
				}
				Thread.Sleep(100);
			}
			stopwatch.Stop();
			this.ReinitializeAfterReboot();
		}

		// Token: 0x0600244D RID: 9293 RVA: 0x000ED195 File Offset: 0x000EB395
		private void parHandling_ConnectedAfterReboot()
		{
			this.progressRestoreUpdateRunning = false;
		}

		// Token: 0x0600244E RID: 9294 RVA: 0x000ED1A0 File Offset: 0x000EB3A0
		private void ReinitializeAfterReboot()
		{
			bool flag = !this.restore.RestoreSystem;
			if (flag)
			{
				this.parHandling.Connected -= this.parHandling_ConnectedAfterReboot;
				this.imageTransfer.Connected += this.imageTransfer_Connected;
				this.parHandling.Connected += this.parHandling_Connected;
				this.modelTransfer.Connected += this.modelTransfer_Connected;
				this.imageTransfer.Disconnected += this.imageTransfer_Disconnected;
				this.parHandling.Disconnected += this.parHandling_Disconnected;
				this.modelTransfer.Disconnected += this.modelTransfer_Disconnected;
				this.xrayState.Start();
				this.parHandling_Connected();
			}
			bool flag2 = Application.Current != null;
			if (flag2)
			{
				Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
				{
					this.progressRestore.FormFadeOut.Begin();
				}));
			}
		}

		// Token: 0x0600244F RID: 9295 RVA: 0x000ED2AC File Offset: 0x000EB4AC
		private void imageTransfer_Connected()
		{
			this.imageConnected = true;
			bool flag = this.errorSurvey != null;
			if (flag)
			{
				this.errorSurvey.ImagePortOk = true;
			}
			bool flag2 = this.productManager != null && this.productManager.CurrentLoadedProduct != null;
			if (flag2)
			{
				bool flag3 = this.productManager.CurrentLoadedProduct.ProductType == ProductType.SingleProduct;
				if (flag3)
				{
					this.LoadCurrentMaterialCurve(0);
				}
				else
				{
					for (int i = 0; i < this.productManager.CurrentLoadedProduct.SubProducts.Count; i++)
					{
						this.LoadCurrentMaterialCurve((int)this.productManager.CurrentLoadedProduct.SubProducts[i].Group);
					}
				}
			}
		}

		// Token: 0x06002450 RID: 9296 RVA: 0x000ED368 File Offset: 0x000EB568
		private void imageTransfer_FrameArrived(object sender, ReceiveEventArgs e)
		{
			try
			{
				MachineMode machineMode = MachineMode.Standard;
				this.dbConnection.GetMachineMode(ref machineMode);
				bool flag = machineMode == MachineMode.Bulk;
				if (flag)
				{
					bool bulkInAutotrainMode = this.BulkInAutotrainMode;
					if (bulkInAutotrainMode)
					{
						bool flag2 = e.Image.State != 16 || (int)e.Image.Height < BulkMode.linesForAutotrain;
						if (flag2)
						{
							return;
						}
					}
				}
				bool flag3 = e.Image.Type != ImageType.TestcardSimulation && e.Image.Type != ImageType.LiveCalibration;
				if (flag3)
				{
					bool flag4 = e.Image.Type != ImageType.MaterialCurve;
					if (flag4)
					{
						DateTime now = DateTime.Now;
						long ticks = new DateTime(1970, 1, 1).Ticks;
						long num = (now.Ticks - ticks) / 10000000L;
						e.Image.Timestamp = (uint)num;
						e.Image.HundrethSecond = (byte)(now.Millisecond / 10);
						e.Image.OperatorIdName = MainWindowViewModel.accessRightsManager.CurrentUser.ID.ToString() + " - " + MainWindowViewModel.accessRightsManager.CurrentUser.Username;
						e.Image.OperatorIdName = e.Image.OperatorIdName.PadRight(50);
						bool flag5 = this.machineState.State != MachineStates.ViewingPlot && this.machineState.State != MachineStates.MovingSource;
						if (flag5)
						{
							string a = "";
							double expectedWeight = 0.0;
							bool flag6 = !this.meanPixelLogger.LogModeStarted && this.machineState.State != MachineStates.XrayOnCalibrationNotDone && this.machineState.State != MachineStates.AutoAdjustingXRay;
							if (flag6)
							{
								bool flag7 = e.Image.Type != ImageType.ConnectionTest && e.Image.Type != ImageType.Histogram && (!this.parHandling.EdgeFilterMode || e.Image.Type == ImageType.Normal);
								if (flag7)
								{
									bool flag8 = e.Image.Type == ImageType.Debug;
									if (flag8)
									{
										a = this.parHandling.ReadPar("/application/DebugImagesEnabled/Value");
									}
									bool flag9 = a != "0";
									if (flag9)
									{
										ImageType type = e.Image.Type;
										e.Image.Weight = (float)this.GetCurrentWeight((int)e.Image.Group, ref expectedWeight);
										e.Image.ProductName = this.productManager.CurrentLoadedProduct.ProductName;
										bool flag10 = e.Image.Group > 0;
										if (flag10)
										{
											ProductData[] array = (from product in this.productManager.CurrentLoadedProduct.SubProducts
											where product.TableName.EndsWith(e.Image.Group.ToString())
											select product).ToArray<ProductData>();
											bool flag11 = array.Length != 0;
											if (flag11)
											{
												e.Image.SubProductName = array[0].ProductName;
											}
										}
										bool flag12 = this.productManager.CurrentLoadedProduct.CurrentBatchName != null;
										if (flag12)
										{
											e.Image.BatchID = this.productManager.CurrentLoadedProduct.CurrentBatchName;
										}
										uint productNumber;
										uint.TryParse(this.productManager.CurrentLoadedProduct.ProductNumber, out productNumber);
										e.Image.ProductNumber = productNumber;
										bool flag13 = false;
										bool flag14 = this.ShowNext;
										if (flag14)
										{
											bool flag15 = false;
											this.dbConnection.GetMultiLaneView(ref flag15);
											bool flag16 = this.productManager.CurrentLoadedProduct.IsLaneProduct || flag15;
											if (flag16)
											{
												bool flag17 = e.Image.Lane == this.SelectedXriLane.Lane || this.SelectedXriLane.Lane == Lane.All;
												if (flag17)
												{
													flag13 = true;
												}
											}
											else
											{
												bool isMultiProduct = this.productManager.CurrentLoadedProduct.IsMultiProduct;
												if (isMultiProduct)
												{
													bool flag18 = (int)e.Image.Group == this.SelectedXriLane.Group || this.SelectedXriLane.Group == 0;
													if (flag18)
													{
														flag13 = true;
													}
												}
												else
												{
													flag13 = true;
												}
											}
										}
										int num2 = (int)(e.Image.State & 16);
										bool flag19 = num2 == 16;
										using (SQLiteTransaction sqliteTransaction = this.dbConnection.Connection.BeginTransaction())
										{
											bool flag20 = this.imageArchiver != null && this.parHandling.ReadPar(Definition.saveImagesPath) != "0" && (this.parHandling.ReadPar(Definition.saveOnlyIfContaminantPath) != "1" || e.Image.HasErrors() || flag13 || e.Image.Type == ImageType.IVP || e.Image.Type == ImageType.Audit || flag19);
											if (flag20)
											{
												bool flag21 = e.Image.Type == ImageType.PerformanceTest;
												if (flag21)
												{
													e.Image.Type = ImageType.Demo;
												}
												bool isRegionProduct = this.productManager.CurrentLoadedProduct.IsRegionProduct;
												if (isRegionProduct)
												{
													this.SetImageRegions(e.Image);
												}
												bool flag22 = this.throughputGlobal == 0U;
												if (flag22)
												{
													this.dbConnection.GetThroughputIndexGlobal(ref this.throughputGlobal);
													this.dbConnection.GetRejectIndexGlobal(ref this.rejectGlobal);
												}
												this.throughputGlobal += 1U;
												bool flag23 = e.Image.HasErrors();
												if (flag23)
												{
													this.rejectGlobal += 1U;
												}
												e.Image.ThroughputIndexGlobal = this.throughputGlobal;
												e.Image.RejectIndexGlobal = this.rejectGlobal;
												bool enabled = this.videojetPrinter.Enabled;
												if (enabled)
												{
													bool flag24 = e.Image.State == 0 && (e.Image.Type == ImageType.Normal || this.DemoMode);
													if (flag24)
													{
														bool flag25 = !this.VideojetPrintTimestamp(e.Image);
														if (flag25)
														{
															bool stopConveyorOnError = this.videojetPrinter.StopConveyorOnError;
															if (stopConveyorOnError)
															{
																this.conveyor.StopConveyor();
															}
															this.errorSurvey.PostMessage(SSTEntryCode.Printer, 0);
														}
													}
												}
												bool flag26 = this.parHandling.ReadPar(Definition.autoKVInitImagePath) == "1";
												if (flag26)
												{
													XRSImage image = e.Image;
													image.State |= 128;
													this.parHandling.WriteValue(Definition.autoKVInitImagePath, "0");
												}
												this.LogMetalAtFrameArrive(e.Image, expectedWeight);
												string fileName = this.ArchiveImageAtFrameArrive(e.Image);
												this.UpdateDBGlobalAndProductImageIndex(e.Image);
												this.UpdateStatistics(e.Image, fileName);
											}
											else
											{
												bool flag27 = this.imageArchiver != null && this.parHandling.ReadPar(Definition.saveImagesPath) != "0" && this.parHandling.ReadPar(Definition.saveOnlyIfContaminantPath) == "1" && !e.Image.HasErrors() && this.HighSpeedMachineActive;
												if (flag27)
												{
													bool flag28 = this.throughputGlobal == 0U;
													if (flag28)
													{
														this.dbConnection.GetThroughputIndexGlobal(ref this.throughputGlobal);
														this.dbConnection.GetRejectIndexGlobal(ref this.rejectGlobal);
													}
													this.throughputGlobal += 1U;
													e.Image.ThroughputIndexGlobal = this.throughputGlobal;
													this.UpdateDBGlobalAndProductImageIndex(e.Image);
												}
											}
											sqliteTransaction.Commit();
										}
										this.ShowImageAtFrameArrive(e.Image, type);
										bool flag29 = flag13;
										if (flag29)
										{
											this.ShowNext = false;
										}
										bool flag30 = e.Image.HasErrors() && this.RetrainMode && e.Image.Type != ImageType.Demo;
										if (flag30)
										{
											this.executeRetrainMode(e.Image);
										}
										bool flag31 = machineMode == MachineMode.Bulk;
										if (flag31)
										{
											this.BulkXRSImageRetrain = new XRSImage();
											this.BulkXRSImageRetrain = e.Image.DeepCopy();
											bool flag32 = this.isBulkLiveImage || this.isBulkObjectImage;
											if (flag32)
											{
												bool flag33 = this.count > 9 || (this.isBulkObjectImage && this.BulkContImagesShown) || (this.isBulkObjectImage && e.Image.HasErrors());
												if (flag33)
												{
													bool flag34 = this.isBulkObjectImage;
													if (flag34)
													{
														this.IsImageBoxVisible = true;
														this.IsImageBoxVisibleBigImage = false;
													}
													else
													{
														this.IsImageBoxVisibleBigImage = true;
														this.IsImageBoxVisible = false;
													}
													this.isBulkLiveImage = false;
													this.isBulkObjectImage = false;
													this.parHandling.WriteValue("/filter/acquisition/SendImage/Value", "0");
													this.parHandling.WriteValue("/filter/acquisition/ContinuousModeObjImg/Value", "0");
													bool flag35 = this.bulkContImagesShown;
													if (flag35)
													{
														this.parHandling.WriteValue("/filter/acquisition/SendOnlyIfContaminant/Value", "1");
													}
													else
													{
														this.parHandling.WriteValue("/filter/acquisition/SendOnlyIfContaminant/Value", "0");
													}
													this.BulkLiveModeSourcePath = "..\\img\\xriimageBulk.png";
													bool flag36 = this.BulkContImagesShown;
													if (flag36)
													{
														this.BulkObjectModeSourcePath = "..\\img\\xriimageBulkCont.png";
													}
													else
													{
														this.BulkObjectModeSourcePath = "..\\img\\xriimageBulkJustCont.png";
													}
													this.StartTimerImageBox();
													this.count = 0;
												}
												else
												{
													this.count++;
												}
											}
											else
											{
												bool flag37 = e.Image.IsContaminant();
												if (flag37)
												{
													this.StartTimerImageBox();
													this.IsImageBoxVisible = true;
													this.IsImageBoxVisibleBigImage = false;
												}
											}
										}
									}
								}
							}
						}
					}
					else
					{
						bool flag38 = e.Image.State == 1 && !XRSImageBox.IsDualEnergyMenu;
						if (flag38)
						{
							this.dbConnection.GetMaterialCurve(this.productManager.CurrentLoadedProduct.TableName, this.materialCurveOld);
							this.SaveMaterialCurve(e.Image);
						}
					}
				}
				else
				{
					bool flag39 = e.Image.Type == ImageType.TestcardSimulation;
					if (flag39)
					{
						bool flag40 = this.testcardsimservice.MonitoringSensIsActive && this.testcardsimservice.MonitoringSensIsInProgress;
						if (flag40)
						{
							bool flag41 = this.testcardsimservice.TestcardSimAfterTimeIntervallGetImg(e.Image);
							if (flag41)
							{
								this.testcardsimservice.setParameterAfterMonitoringSensitivities(false);
								this.testcardsimservice.MonitoringSensIsInProgress = false;
								bool flag42 = this.testcardsimservice.CheckChangeOfSensitivities();
								if (flag42)
								{
									bool flag43 = Application.Current != null;
									if (flag43)
									{
										Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
										{
											this.testcardsimservice.setRetrainTestcardSimMsg(false);
											this.testcardsimservice.setParameterAfterMonitoringSensitivities(true);
										}));
									}
									this.iCMSuccesfullCounter = 0U;
								}
								else
								{
									int num3 = Convert.ToInt32(this.parHandling.ReadPar(Definition.timeIntervallForTestcardSim));
									int num4 = Convert.ToInt32(this.parHandling.ReadPar(Definition.simulationActSensVA));
									int num5 = Convert.ToInt32(this.parHandling.ReadPar(Definition.simulationActSensGlass));
									int num6 = Convert.ToInt32(this.parHandling.ReadPar(Definition.simulationCompSensVA));
									int num7 = Convert.ToInt32(this.parHandling.ReadPar(Definition.simulationCompSensGlass));
									bool flag44 = !this.testcardsimservice.CheckBallCountsBecauseOfNewVersion();
									if (flag44)
									{
										num4++;
										num5++;
										num6++;
										num7++;
									}
									bool flag45 = num4 <= num6 && num5 <= num7;
									if (flag45)
									{
										bool flag46 = num3 > 0 && num3 < 60;
										if (flag46)
										{
											this.iCMSuccesfullCounter += 1U;
										}
										else
										{
											this.iCMSuccesfullCounter = 1U;
										}
										bool flag47 = (ulong)this.iCMSuccesfullCounter * (ulong)((long)num3) >= 60UL;
										if (flag47)
										{
											SSTLogbookEntry entry = LogbookManager.AddEntry(SSTEntryCode.ICMsuccesful, 0, "", "", "", "");
											this.messageSystem.PostRegisteredMessage(entry);
											this.iCMSuccesfullCounter = 0U;
										}
									}
								}
							}
						}
					}
					else
					{
						bool flag48 = e.Image.Type == ImageType.LiveCalibration;
						if (flag48)
						{
							DateTime now2 = DateTime.Now;
							long ticks2 = new DateTime(1970, 1, 1).Ticks;
							long num8 = (now2.Ticks - ticks2) / 10000000L;
							e.Image.Timestamp = (uint)num8;
							e.Image.HundrethSecond = (byte)(now2.Millisecond / 10);
							string fileName2 = this.ArchiveImageAtFrameArrive(e.Image);
							this.UpdateDBGlobalAndProductImageIndex(e.Image);
							this.UpdateStatistics(e.Image, fileName2);
						}
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x06002451 RID: 9297 RVA: 0x000EE188 File Offset: 0x000EC388
		private void UpdateStatistics(XRSImage image, string fileName)
		{
			bool flag = image.IsImageToIncreaseProductCounter(this.machineState, this.meanPixelLogger, this.parHandling);
			if (flag)
			{
				Stopwatch stopwatch = Stopwatch.StartNew();
				bool flag2 = false;
				this.dbConnection.GetStatisticsEnabled(ref flag2);
				bool flag3 = flag2;
				if (flag3)
				{
					this.UpdateStatistics(image, fileName, this.productManager.CurrentLoadedProduct);
					bool flag4 = image.Group > 0;
					if (flag4)
					{
						ProductData[] array = (from product in this.productManager.CurrentLoadedProduct.SubProducts
						where product.TableName.EndsWith(image.Group.ToString())
						select product).ToArray<ProductData>();
						bool flag5 = array.Length != 0;
						if (flag5)
						{
							this.UpdateStatistics(image, fileName, array[0]);
						}
					}
					bool isRegionProduct = this.productManager.CurrentLoadedProduct.IsRegionProduct;
					if (isRegionProduct)
					{
						for (int i = 0; i < this.productManager.CurrentLoadedProduct.SubProducts.Count; i++)
						{
							this.UpdateStatistics(image, fileName, this.productManager.CurrentLoadedProduct.SubProducts[i]);
						}
					}
				}
				stopwatch.Stop();
				this.parHandling.WriteValue(Definition.timeUpdateStatisticsPath, stopwatch.ElapsedMilliseconds.ToString(), false);
			}
		}

		// Token: 0x06002452 RID: 9298 RVA: 0x000EE2F4 File Offset: 0x000EC4F4
		private void UpdateStatistics(XRSImage image, string fileName, ProductData product)
		{
			int num = 0;
			uint num2 = 0U;
			double num3 = 0.0;
			string s = "";
			string a = null;
			string a2 = null;
			string str = (product.Group > 0) ? product.Group.ToString() : "";
			s = this.parHandling.ReadPar(Definition.filterMainPath + str + ObjectCounter.currentCountSubPath);
			Statistic statistic = new Statistic();
			statistic.GlobalRejectIndex = this.rejectGlobal;
			statistic.GlobalThroughputIndex = this.throughputGlobal;
			bool flag = this.dbConnection.GetParameterValue(Definition.filterMainPath + str + ObjectCounter.enabledSubPath, ref a) && a == "1";
			if (flag)
			{
				bool flag2 = int.TryParse(this.parHandling.ReadPar(Definition.filterMainPath + str + ObjectCounter.maxCountSubPath), out num);
				if (flag2)
				{
					statistic.MaxObjectCount = num;
				}
				bool flag3 = int.TryParse(this.parHandling.ReadPar(Definition.filterMainPath + str + ObjectCounter.minCountSubPath), out num);
				if (flag3)
				{
					statistic.MinObjectCount = num;
				}
				bool flag4 = int.TryParse(this.parHandling.ReadPar(Definition.filterMainPath + str + ObjectCounter.currentCountSubPath), out num);
				if (flag4)
				{
					statistic.ObjectCount = num;
				}
			}
			bool flag5 = this.dbConnection.GetParameterValue(Definition.filterMainPath + str + Weigher.enableWeightSubPath, ref a2) && a2 == "1";
			if (flag5)
			{
				bool flag6 = this.dbConnection.GetParameterValue("MaxWeight", product.TableName, ref s) && double.TryParse(s, NumberStyles.Float, new CultureInfo("en-US"), out num3);
				if (flag6)
				{
					statistic.MaxWeight = num3;
				}
				bool flag7 = this.dbConnection.GetParameterValue("MinWeight", product.TableName, ref s) && double.TryParse(s, NumberStyles.Float, new CultureInfo("en-US"), out num3);
				if (flag7)
				{
					statistic.MinWeight = num3;
				}
				bool flag8 = this.dbConnection.GetParameterValue(Definition.strReferenceWeight, product.TableName, ref s) && double.TryParse(s, NumberStyles.Float, new CultureInfo("en-US"), out num3);
				if (flag8)
				{
					statistic.ReferenceWeight = num3;
				}
				statistic.Weight = (double)((float)this.GetCurrentWeight((int)product.Group, ref num3));
			}
			bool flag9 = this.dbConnection.GetParameterValue("RejectIndex", product.TableName, ref s) && uint.TryParse(s, out num2);
			if (flag9)
			{
				statistic.ProductRejectIndex = num2;
			}
			bool flag10 = this.dbConnection.GetParameterValue("RejectIndexContamination", product.TableName, ref s) && uint.TryParse(s, out num2);
			if (flag10)
			{
				statistic.ProductRejectIndexContaminations = num2;
			}
			bool flag11 = this.dbConnection.GetParameterValue("RejectIndexObjectCount", product.TableName, ref s) && uint.TryParse(s, out num2);
			if (flag11)
			{
				statistic.ProductRejectIndexObjectCount = num2;
			}
			bool flag12 = this.dbConnection.GetParameterValue("RejectIndexSize", product.TableName, ref s) && uint.TryParse(s, out num2);
			if (flag12)
			{
				statistic.ProductRejectIndexSize = num2;
			}
			bool flag13 = this.dbConnection.GetParameterValue("RejectIndexWeight", product.TableName, ref s) && uint.TryParse(s, out num2);
			if (flag13)
			{
				statistic.ProductRejectIndexWeight = num2;
			}
			bool flag14 = this.dbConnection.GetParameterValue("ThroughputIndex", product.TableName, ref s) && uint.TryParse(s, out num2);
			if (flag14)
			{
				statistic.ProductThroughputIndex = num2;
			}
			try
			{
				statistic.SaveTime = DateTime.ParseExact(Path.GetFileNameWithoutExtension(fileName), StoreImageData.fileNameDateTimeFormat, CultureInfo.InvariantCulture);
			}
			catch
			{
			}
			bool isBatch = false;
			this.dbConnection.GetBatchesEnabled(ref isBatch);
			this.dbConnection.AddStatistic(product.TableName, statistic, isBatch);
		}

		// Token: 0x06002453 RID: 9299 RVA: 0x000EE700 File Offset: 0x000EC900
		private short GetCurrentObjectLength(int imageGroup)
		{
			string str = (imageGroup > 0) ? imageGroup.ToString() : "";
			short result = -1;
			bool flag = this.parHandling.ReadPar(Definition.filterMainPath + str + Definition.dimensionEnabledSubPath) == "1";
			if (flag)
			{
				string s = this.parHandling.ReadPar(Definition.filterMainPath + str + Definition.dimensionCurrentLengthSubPath);
				short.TryParse(s, out result);
			}
			return result;
		}

		// Token: 0x06002454 RID: 9300 RVA: 0x000EE77C File Offset: 0x000EC97C
		private void ShowImageAtFrameArrive(XRSImage image, ImageType imageType)
		{
			bool flag = false;
			bool flag2 = this.ShowNext;
			if (flag2)
			{
				bool flag3 = false;
				this.dbConnection.GetMultiLaneView(ref flag3);
				bool flag4 = this.productManager.CurrentLoadedProduct.IsLaneProduct || flag3;
				if (flag4)
				{
					bool flag5 = image.Lane == this.SelectedXriLane.Lane || this.SelectedXriLane.Lane == Lane.All;
					if (flag5)
					{
						flag = true;
					}
				}
				else
				{
					bool isMultiProduct = this.productManager.CurrentLoadedProduct.IsMultiProduct;
					if (isMultiProduct)
					{
						bool flag6 = (int)image.Group == this.SelectedXriLane.Group || this.SelectedXriLane.Group == 0;
						if (flag6)
						{
							flag = true;
						}
					}
					else
					{
						flag = true;
					}
				}
			}
			bool flag7 = imageType != ImageType.PerformanceTest;
			if (flag7)
			{
				bool flag8 = this.parHandling.ReadPar("/filter/acquisition/ShowImage/Value") != "0" && (this.parHandling.ReadPar("/filter/acquisition/ShowOnlyIfContaminant/Value") != "1" || image.HasErrors() || flag);
				if (flag8)
				{
					bool flag9 = !this.bgWorkerPaintImage.IsBusy;
					if (flag9)
					{
						this.bgWorkerPaintImage.RunWorkerAsync(image);
					}
					else
					{
						this.StartLastPaintedImageTimer(image);
					}
				}
			}
		}

		// Token: 0x06002455 RID: 9301 RVA: 0x000EE8C8 File Offset: 0x000ECAC8
		private string ArchiveImageAtFrameArrive(XRSImage image)
		{
			string text = "";
			int maxGoodFiles = 50;
			int maxBadFiles = 50;
			bool flag = Convert.ToInt32(this.dbConnection.GetLocationNetworkSavingPng()) > 0;
			List<string> list = new List<string>();
			bool flag2 = false;
			ConveyingDirection conveyingDirection = ConveyingDirection.LeftToRight;
			this.dbConnection.GetNSaveImagesGood(ref maxGoodFiles);
			this.dbConnection.GetNSaveImagesBad(ref maxBadFiles);
			this.dbConnection.GetConveyingDirection(ref conveyingDirection);
			this.dbConnection.GetTestAtCustomerSite(ref flag2);
			XRSImage.RotateDirection rotateDirection = (conveyingDirection == ConveyingDirection.LeftToRight) ? XRSImage.RotateDirection.ClockwiseAndInvertX : XRSImage.RotateDirection.AnticlockwiseInvertXAndLines;
			this.imageArchiver.ArchiveImage(image, rotateDirection, maxGoodFiles, maxBadFiles, ref text);
			bool flag3 = flag && image.Type != ImageType.TestcardSimulation;
			if (flag3)
			{
				string text2 = "";
				this.dbConnection.GetSerialNumber(ref text2);
				bool flag4 = text2 == "";
				if (flag4)
				{
					text2 = "0000";
				}
				ItemPngToNetworkFolder item = new ItemPngToNetworkFolder
				{
					Image = image,
					RotateDirection = rotateDirection,
					MaxFiles = Convert.ToInt32(this.dbConnection.GetMaxImagesNetworkSavingPng()),
					ProductName = this.productManager.CurrentLoadedProduct.ProductName,
					ProductNumber = this.productManager.CurrentLoadedProduct.ProductNumber,
					NetworkPath = this.dbConnection.GetNetworkPathSavingPng(),
					Mode = this.dbConnection.GetModeNetworkSavingPng(),
					SerialNumber = text2,
					Is16BitResolution = this.dbConnection.GetResolutionNetworkSavingPng(),
					BinningMode = this.dbConnection.GetNetworkSavingPngBinningMode(),
					LocationNetworkMode = this.dbConnection.GetLocationNetworkSavingPng()
				};
				this.archiveQueuePngToNetworkFolder.Add(item);
			}
			bool flag5 = this.parHandling.ReadPar(Definition.anomalyDetectionTrainEnabled) == "1";
			if (flag5)
			{
				int maxFiles = Convert.ToInt32(this.parHandling.ReadPar(Definition.anomalyDetectionTrainSizeDataset));
				string tableName = "";
				this.dbConnection.GetCurrentProductTableNameAI(ref tableName);
				this.imageArchiver.ArchiveImageTrainAnomalyDetection(image, maxFiles, tableName, text);
			}
			bool flag6 = flag2 && image.Type != ImageType.TestcardSimulation;
			if (flag6)
			{
				long diskSpaceMinFreeMB = 1000L;
				bool flag7 = image.HasErrors();
				string archiveDirectory = StoreImageData.archiveDirectoryTestAtCustomerSite + (flag7 ? "Reject\\" : "Throughput\\");
				this.imageArchiver.ArchiveImageTo(image, 250, archiveDirectory, ref text, diskSpaceMinFreeMB);
			}
			return text;
		}

		// Token: 0x06002456 RID: 9302 RVA: 0x000EEB3C File Offset: 0x000ECD3C
		private void UpdateDBGlobalAndProductImageIndex(XRSImage image)
		{
			Stopwatch stopwatch = Stopwatch.StartNew();
			bool flag = true;
			MachineMode machineMode = MachineMode.Standard;
			this.dbConnection.GetMachineMode(ref machineMode);
			this.dbConnection.GetLogImageIndex(ref flag);
			bool flag2 = flag;
			if (flag2)
			{
				this.dbConnection.SetThroughputIndexGlobal(this.throughputGlobal);
				bool flag3 = image.HasErrors();
				if (flag3)
				{
					this.dbConnection.SetRejectIndexGlobal(this.rejectGlobal);
					this.OnRejectIncreased();
				}
				else
				{
					this.OnThroughputIncreased();
				}
				bool flag4 = machineMode != MachineMode.Bulk;
				if (flag4)
				{
					this.dbDatalogs.OperatingCounterSetThroughputAndRejectCounts(this.throughputGlobal, this.rejectGlobal, image.HasErrors());
					bool flag5 = !this.dbDatalogs.OnOffEntryDoesExist(DateTime.Now.ToString("d", new CultureInfo("de-DE")));
					if (flag5)
					{
						bool flag6 = image.HasErrors();
						if (flag6)
						{
							this.dbDatalogs.SetNewOnOffTimesEntry(DateTime.Now.ToString("d", new CultureInfo("de-DE")), "0", "0", "0", "0", "0", "0", "0", "0", "1");
						}
						else
						{
							this.dbDatalogs.SetNewOnOffTimesEntry(DateTime.Now.ToString("d", new CultureInfo("de-DE")), "0", "0", "0", "0", "0", "0", "0", "1", "0");
						}
					}
					else
					{
						bool flag7 = image.HasErrors();
						if (flag7)
						{
							this.dbDatalogs.UpdateOnOffTimes(DateTime.Now.ToString("d", new CultureInfo("de-DE")), "Reject", 1);
						}
						else
						{
							this.dbDatalogs.UpdateOnOffTimes(DateTime.Now.ToString("d", new CultureInfo("de-DE")), "Throughput", 1);
						}
					}
				}
			}
			stopwatch.Stop();
			this.parHandling.WriteValue(Definition.timeUpdateGlobalIndexPath, stopwatch.ElapsedMilliseconds.ToString(), false);
			this.UpdateDBProductIndex(image);
			this.UpdateDBBatchCounters(image);
			bool flag8 = machineMode != MachineMode.Bulk;
			if (flag8)
			{
				this.CheckThroughputAndReject(image.HasErrors());
			}
		}

		// Token: 0x06002457 RID: 9303 RVA: 0x000EEDA4 File Offset: 0x000ECFA4
		private void ResetThroughputRejectMessages()
		{
			this.throughput90Message = false;
			this.throughputMessage = false;
			this.reject90Message = false;
			this.rejectMessage = false;
		}

		// Token: 0x06002458 RID: 9304 RVA: 0x000EEDC4 File Offset: 0x000ECFC4
		private void CheckThroughputAndReject(bool reject)
		{
			string text = "";
			this.dbDatalogs.GetDataLogsSettingsEntry("MonitoringMessagesOnOff", ref text);
			bool flag = text.Equals("1");
			if (flag)
			{
				bool flag2 = !reject;
				if (flag2)
				{
					string text2 = "";
					this.dbDatalogs.OnOffTimesGetEntryOfDate(DateTime.Now.ToString("d", new CultureInfo("de-DE")), ref text2, "Throughput");
					double num = 0.0;
					bool flag3 = double.TryParse(text2.Replace(',', '.'), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out num);
					if (flag3)
					{
						text2 = "";
						double num2 = 0.0;
						this.dbDatalogs.GetLimitsEntry("ThroughputPerDay", ref text2);
						bool flag4 = !text2.Equals("-") && !text2.Equals("") && !text2.Equals(" ");
						if (flag4)
						{
							bool flag5 = double.TryParse(text2.Replace(',', '.'), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out num2);
							if (flag5)
							{
								bool flag6 = num2 < num && !this.throughputMessage;
								if (flag6)
								{
									text = "";
									this.dbDatalogs.GetDataLogsSettingsEntry("MessageLevel", ref text);
									bool flag7 = text.Equals("Info");
									SSTEntryCode code;
									if (flag7)
									{
										code = SSTEntryCode.LimitsInfo;
									}
									else
									{
										bool flag8 = text.Equals("Warning");
										if (flag8)
										{
											code = SSTEntryCode.LimitsWarning;
										}
										else
										{
											code = SSTEntryCode.LimitsError;
										}
									}
									this.errorSurvey.PostMessageMonitoring(code, 0, "txtThroughput", "txtPerDay", num2.ToString(), num.ToString());
									this.throughputMessage = true;
								}
								else
								{
									bool flag9 = num2 * 0.9 <= num && num2 >= num && !this.throughput90Message;
									if (flag9)
									{
										text = "";
										this.dbDatalogs.GetDataLogsSettingsEntry("MessageLevel90Perc", ref text);
										bool flag10 = text.Equals("Info");
										SSTEntryCode code2;
										if (flag10)
										{
											code2 = SSTEntryCode.Limits90PerInfo;
										}
										else
										{
											code2 = SSTEntryCode.Limits90PerWarning;
										}
										this.errorSurvey.PostMessageMonitoring(code2, 0, "txtThroughput", "txtPerDay", num2.ToString(), num.ToString());
										this.throughput90Message = true;
									}
									else
									{
										bool flag11 = num2 >= num;
										if (flag11)
										{
											this.throughputMessage = false;
											bool flag12 = num2 * 0.9 > num;
											if (flag12)
											{
												this.throughput90Message = false;
											}
										}
									}
								}
							}
						}
					}
				}
				else if (reject)
				{
					string text3 = "";
					this.dbDatalogs.OnOffTimesGetEntryOfDate(DateTime.Now.ToString("d", new CultureInfo("de-DE")), ref text3, "Reject");
					double num3 = 0.0;
					bool flag13 = double.TryParse(text3.Replace(',', '.'), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out num3);
					if (flag13)
					{
						text3 = "";
						double num4 = 0.0;
						this.dbDatalogs.GetLimitsEntry("RejectPerDay", ref text3);
						bool flag14 = !text3.Equals("-") && !text3.Equals("") && !text3.Equals(" ");
						if (flag14)
						{
							bool flag15 = double.TryParse(text3.Replace(',', '.'), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out num4);
							if (flag15)
							{
								bool flag16 = num4 < num3 && !this.rejectMessage;
								if (flag16)
								{
									text = "";
									this.dbDatalogs.GetDataLogsSettingsEntry("MessageLevel", ref text);
									bool flag17 = text.Equals("Info");
									SSTEntryCode code3;
									if (flag17)
									{
										code3 = SSTEntryCode.LimitsInfo;
									}
									else
									{
										bool flag18 = text.Equals("Warning");
										if (flag18)
										{
											code3 = SSTEntryCode.LimitsWarning;
										}
										else
										{
											code3 = SSTEntryCode.LimitsError;
										}
									}
									this.errorSurvey.PostMessageMonitoring(code3, 0, "txtReject", "txtPerDay", num4.ToString(), num3.ToString());
									this.rejectMessage = true;
								}
								else
								{
									bool flag19 = num4 * 0.9 <= num3 && num4 >= num3 && !this.reject90Message;
									if (flag19)
									{
										text = "";
										this.dbDatalogs.GetDataLogsSettingsEntry("MessageLevel90Perc", ref text);
										bool flag20 = text.Equals("Info");
										SSTEntryCode code4;
										if (flag20)
										{
											code4 = SSTEntryCode.Limits90PerInfo;
										}
										else
										{
											code4 = SSTEntryCode.Limits90PerWarning;
										}
										this.errorSurvey.PostMessageMonitoring(code4, 0, "txtReject", "txtPerDay", num4.ToString(), num3.ToString());
										this.reject90Message = true;
									}
									else
									{
										bool flag21 = num4 >= num3;
										if (flag21)
										{
											this.rejectMessage = false;
											bool flag22 = num4 * 0.9 > num3;
											if (flag22)
											{
												this.reject90Message = false;
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

		// Token: 0x06002459 RID: 9305 RVA: 0x000EF2BC File Offset: 0x000ED4BC
		private void UpdateDBBatchCounters(XRSImage image)
		{
			bool flag = image.IsImageToIncreaseProductCounter(this.machineState, this.meanPixelLogger, this.parHandling);
			if (flag)
			{
				bool flag2 = false;
				this.dbConnection.GetBatchesEnabled(ref flag2);
				bool flag3 = flag2;
				if (flag3)
				{
					bool increaseNContaminants = image.IsContaminant();
					bool increaseFalseWeight = image.HasFalseWeight();
					bool increaseFalseObjectCount = image.HasFalseCount();
					bool increaseWrongSize = image.HasWrongSize();
					this.dbConnection.IncreaseBatchThroughputAndReject(increaseNContaminants, increaseFalseWeight, increaseFalseObjectCount, increaseWrongSize);
				}
			}
		}

		// Token: 0x0600245A RID: 9306 RVA: 0x000EF334 File Offset: 0x000ED534
		private void LogMetalAtFrameArrive(XRSImage image, double expectedWeight)
		{
			List<LaneXri> list = (from lane in this.ShownLanes
			where lane.Lane == Lane.All
			select lane).ToList<LaneXri>();
			uint throughputCount = 0U;
			uint rejectCount = 0U;
			SSTEntryCode sstentryCode = SSTEntryCode.Metal;
			int num = 0;
			int num2 = 0;
			bool flag = list.Count == 0;
			if (flag)
			{
				list = (from lane in this.ShownLanes
				where lane.Lane == Lane.Lane1
				select lane).ToList<LaneXri>();
			}
			bool flag2 = list.Count > 0;
			if (flag2)
			{
				throughputCount = ((image.Type == ImageType.Normal) ? (list[0].Counter.ThroughputCounter + 1U) : list[0].Counter.ThroughputCounter);
				rejectCount = ((image.Type == ImageType.Normal) ? (list[0].Counter.RejectCounter + 1U) : list[0].Counter.RejectCounter);
			}
			AuditImageType auditImageType = this.GetAuditImageType(image);
			bool flag3 = auditImageType > AuditImageType.None;
			if (flag3)
			{
				sstentryCode = SSTEntryCode.MetalDuringAudit;
			}
			bool flag4 = image.Type == ImageType.IVP;
			if (flag4)
			{
				auditImageType = AuditImageType.None;
			}
			Stopwatch stopwatch = Stopwatch.StartNew();
			this.LogGoodImageDuringAudit(image, expectedWeight, throughputCount, rejectCount, auditImageType);
			int num3 = 0;
			this.dbConnection.GetAuditCheckType(ref num3);
			bool flag5 = image.Type != ImageType.IVP && (image.Type != ImageType.Audit || num3 != 1);
			if (flag5)
			{
				bool flag6 = image.IsContaminant();
				if (flag6)
				{
					bool flag7 = !this.productManager.CurrentLoadedProduct.IsMultiProduct;
					if (flag7)
					{
						bool flag8 = sstentryCode != SSTEntryCode.MetalDuringAudit || auditImageType == AuditImageType.Contamination;
						if (flag8)
						{
							string text = ": P" + this.productManager.CurrentLoadedProduct.ProductNumber + " " + this.productManager.CurrentLoadedProduct.ProductName;
							SSTLogbookEntry sstlogbookEntry = LogbookManager.AddEntry(sstentryCode, 0, throughputCount.ToString(), rejectCount.ToString());
							image.LogbookID = sstlogbookEntry.ID;
						}
					}
					else
					{
						bool flag9 = image.Group <= 0;
						if (flag9)
						{
							SSTLogbookEntry sstlogbookEntry2 = LogbookManager.AddEntry(SSTEntryCode.Metal, 3, throughputCount.ToString(), rejectCount.ToString());
							bool flag10 = image.LogbookID == 0U;
							if (flag10)
							{
								image.LogbookID = sstlogbookEntry2.ID;
							}
						}
						else
						{
							bool flag11 = sstentryCode != SSTEntryCode.MetalDuringAudit || auditImageType == AuditImageType.Contamination;
							if (flag11)
							{
								ProductData[] array = (from product in this.productManager.CurrentLoadedProduct.SubProducts
								where product.TableName.EndsWith(image.Group.ToString())
								select product).ToArray<ProductData>();
								bool flag12 = array.Length != 0;
								if (flag12)
								{
									string text = string.Concat(new string[]
									{
										": P",
										this.productManager.CurrentLoadedProduct.ProductNumber,
										"_",
										image.Group.ToString(),
										" ",
										array[0].ProductName
									});
								}
								SSTLogbookEntry sstlogbookEntry3 = LogbookManager.AddEntry(sstentryCode, 0, throughputCount.ToString(), rejectCount.ToString());
								bool flag13 = image.LogbookID == 0U;
								if (flag13)
								{
									image.LogbookID = sstlogbookEntry3.ID;
								}
							}
						}
					}
				}
				bool flag14 = image.HasFalseWeight();
				if (flag14)
				{
					bool flag15 = sstentryCode != SSTEntryCode.MetalDuringAudit || auditImageType == AuditImageType.Weight;
					if (flag15)
					{
						SSTLogbookEntry sstlogbookEntry4 = LogbookManager.AddEntry(sstentryCode, 1, throughputCount.ToString(), rejectCount.ToString(), (image.Weight * 1000f).ToString(), (expectedWeight * 1000.0).ToString());
						bool flag16 = image.LogbookID == 0U;
						if (flag16)
						{
							image.LogbookID = sstlogbookEntry4.ID;
						}
					}
				}
				bool flag17 = image.HasFalseCount();
				if (flag17)
				{
					bool flag18 = sstentryCode != SSTEntryCode.MetalDuringAudit || auditImageType == AuditImageType.ObjectCount;
					if (flag18)
					{
						string str = (image.Group > 0) ? image.Group.ToString() : "";
						int.TryParse(this.parHandling.ReadPar(Definition.filterMainPath + str + ObjectCounter.minCountSubPath), out num);
						int.TryParse(this.parHandling.ReadPar(Definition.filterMainPath + str + ObjectCounter.maxCountSubPath), out num2);
						SSTLogbookEntry sstlogbookEntry5 = LogbookManager.AddEntry(sstentryCode, 2, throughputCount.ToString(), rejectCount.ToString(), image.ObjectCount.ToString(), num.ToString(), num2.ToString());
						bool flag19 = image.LogbookID == 0U;
						if (flag19)
						{
							image.LogbookID = sstlogbookEntry5.ID;
						}
					}
				}
				bool flag20 = image.HasWrongSize();
				if (flag20)
				{
					SSTLogbookEntry sstlogbookEntry6 = LogbookManager.AddEntry(sstentryCode, 4, throughputCount.ToString(), rejectCount.ToString());
					bool flag21 = image.LogbookID == 0U;
					if (flag21)
					{
						image.LogbookID = sstlogbookEntry6.ID;
					}
				}
			}
			else
			{
				bool flag22 = image.IsContaminant();
				if (flag22)
				{
				}
			}
			stopwatch.Stop();
			this.parHandling.WriteValue(Definition.timeLogMetalPath, stopwatch.ElapsedMilliseconds.ToString(), false);
		}

		// Token: 0x0600245B RID: 9307 RVA: 0x000EF914 File Offset: 0x000EDB14
		private void LogGoodImageDuringAudit(XRSImage image, double expectedWeight, uint throughputCount, uint rejectCount, AuditImageType auditImageType)
		{
			bool flag = image.Type == ImageType.Audit;
			if (flag)
			{
				SSTLogbookEntry sstlogbookEntry = null;
				int num = 0;
				this.dbConnection.GetAuditCheckType(ref num);
				bool flag2 = num != 1;
				if (flag2)
				{
					bool flag3 = auditImageType == AuditImageType.Contamination && !image.IsContaminant();
					if (flag3)
					{
						sstlogbookEntry = LogbookManager.AddEntry(SSTEntryCode.ProductDuringTest, 0, throughputCount.ToString(), rejectCount.ToString());
					}
					else
					{
						bool flag4 = auditImageType == AuditImageType.Weight && !image.HasFalseWeight();
						if (flag4)
						{
							sstlogbookEntry = LogbookManager.AddEntry(SSTEntryCode.ProductDuringTest, 1, throughputCount.ToString(), rejectCount.ToString(), (image.Weight * 1000f).ToString(), (expectedWeight * 1000.0).ToString());
						}
						else
						{
							bool flag5 = auditImageType == AuditImageType.ObjectCount && !image.HasFalseCount();
							if (flag5)
							{
								int num2 = 0;
								int num3 = 0;
								string str = (image.Group > 0) ? image.Group.ToString() : "";
								int.TryParse(this.parHandling.ReadPar(Definition.filterMainPath + str + ObjectCounter.minCountSubPath), out num2);
								int.TryParse(this.parHandling.ReadPar(Definition.filterMainPath + str + ObjectCounter.maxCountSubPath), out num3);
								sstlogbookEntry = LogbookManager.AddEntry(SSTEntryCode.ProductDuringTest, 2, throughputCount.ToString(), rejectCount.ToString(), image.ObjectCount.ToString(), num2.ToString(), num3.ToString());
							}
						}
					}
					bool flag6 = sstlogbookEntry != null;
					if (flag6)
					{
						image.LogbookID = sstlogbookEntry.ID;
					}
				}
			}
		}

		// Token: 0x0600245C RID: 9308 RVA: 0x000EFAC4 File Offset: 0x000EDCC4
		private void UpdateDBProductIndex(XRSImage image)
		{
			bool flag = image.IsImageToIncreaseProductCounter(this.machineState, this.meanPixelLogger, this.parHandling);
			if (flag)
			{
				Stopwatch stopwatch = Stopwatch.StartNew();
				bool flag2 = true;
				this.dbConnection.GetLogImageIndex(ref flag2);
				bool flag3 = flag2;
				if (flag3)
				{
					this.dbConnection.IncreaseCurrentProductThroughputIndex();
					bool flag4 = image.HasErrors();
					if (flag4)
					{
						this.dbConnection.IncreaseCurrentProductRejectIndices(image.IsContaminant(), image.HasFalseCount(), image.HasFalseWeight(), image.HasWrongSize());
					}
				}
				bool flag5 = image.Group > 0 || this.productManager.CurrentLoadedProduct.IsRegionProduct;
				if (flag5)
				{
					bool flag6 = image.Group > 0;
					IList<ProductData> list;
					if (flag6)
					{
						list = (from product in this.productManager.CurrentLoadedProduct.SubProducts
						where product.TableName.EndsWith(image.Group.ToString())
						select product).ToList<ProductData>();
					}
					else
					{
						list = this.productManager.CurrentLoadedProduct.SubProducts;
					}
					for (int i = 0; i < list.Count; i++)
					{
						bool flag7 = flag2;
						if (flag7)
						{
							this.dbConnection.IncreaseProductThroughputIndex(list[i].TableName);
							bool flag8 = image.HasErrors();
							if (flag8)
							{
								this.dbConnection.IncreaseProductRejectIndices(list[i].TableName, image.IsContaminant(), image.HasFalseCount(), image.HasFalseWeight(), image.HasWrongSize());
							}
						}
					}
				}
				stopwatch.Stop();
				this.parHandling.WriteValue(Definition.timeUpdateProductIndexPath, stopwatch.ElapsedMilliseconds.ToString(), false);
			}
		}

		// Token: 0x0600245D RID: 9309 RVA: 0x000EFCC4 File Offset: 0x000EDEC4
		private AuditImageType GetAuditImageType(XRSImage image)
		{
			AuditImageType result = AuditImageType.None;
			bool flag = image.Type == ImageType.Audit;
			if (flag)
			{
				bool flag2 = this.wdAuditRequired != null && this.wdAuditRequired.TestRunning && this.auditSettings != null && this.auditSettings.AuditRequired != null;
				if (flag2)
				{
					bool inTestContaminationMode = this.auditSettings.AuditRequired.InTestContaminationMode;
					if (inTestContaminationMode)
					{
						result = AuditImageType.Contamination;
					}
					else
					{
						bool inTestWeightMode = this.auditSettings.AuditRequired.InTestWeightMode;
						if (inTestWeightMode)
						{
							result = AuditImageType.Weight;
						}
						else
						{
							bool inTestCountMode = this.auditSettings.AuditRequired.InTestCountMode;
							if (inTestCountMode)
							{
								result = AuditImageType.ObjectCount;
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x0600245E RID: 9310 RVA: 0x000EFD6C File Offset: 0x000EDF6C
		private double GetCurrentWeight(int imageGroup, ref double expectedWeight)
		{
			double result = 0.0;
			try
			{
				string text = (imageGroup > 0) ? imageGroup.ToString() : "";
				string s = "";
				WeightUnit weightUnit = WeightUnit.Gramm;
				bool flag = this.parHandling.ReadPar(Definition.filterMainPath + text + Weigher.enableWeightSubPath) == "1";
				if (flag)
				{
					bool flag2 = imageGroup <= 0;
					if (flag2)
					{
						this.dbConnection.GetParameterValue(Definition.strReferenceWeight, ref s);
					}
					else
					{
						string str = null;
						this.dbConnection.GetCurrentProductTableName(ref str);
						this.dbConnection.GetParameterValue(Definition.strReferenceWeight, str + Definition.subTableIdentifier.ToString() + text, ref s);
					}
					this.dbConnection.GetWeightUnit(ref weightUnit);
					expectedWeight = double.Parse(s, new CultureInfo("en-US"));
					bool flag3 = expectedWeight != 0.0;
					if (flag3)
					{
						double num = double.Parse(this.parHandling.ReadPar(Definition.filterMainPath + text + Weigher.currentWeightSubPath));
						double num2 = double.Parse(this.parHandling.ReadPar(Definition.filterMainPath + text + Weigher.expectedWeightSubPath));
						bool flag4 = num2 != 0.0;
						if (flag4)
						{
							result = num / num2 * expectedWeight;
						}
						else
						{
							result = 0.0;
						}
					}
				}
			}
			catch (FormatException)
			{
			}
			return result;
		}

		// Token: 0x0600245F RID: 9311 RVA: 0x000EFF00 File Offset: 0x000EE100
		private void StartLastPaintedImageTimer(XRSImage currentImage)
		{
			this.timerLastHistoryImage = new System.Threading.Timer(delegate(object obj)
			{
				bool flag = obj is XRSImage[] && ((XRSImage[])obj).Length == 2;
				if (flag)
				{
					XRSImage argument = ((XRSImage[])obj)[0];
					XRSImage xrsimage = ((XRSImage[])obj)[1];
					bool flag2 = xrsimage == this.lastPaintedImage && !this.bgWorkerPaintImage.IsBusy;
					if (flag2)
					{
						this.bgWorkerPaintImage.RunWorkerAsync(argument);
					}
				}
			}, new XRSImage[]
			{
				currentImage,
				this.lastPaintedImage
			}, 3000, -1);
		}

		// Token: 0x06002460 RID: 9312 RVA: 0x000EFF34 File Offset: 0x000EE134
		private void bgWorkerPaintImages_DoWork(object sender, DoWorkEventArgs e)
		{
			try
			{
				bool flag = e.Argument is XRSImage;
				if (flag)
				{
					XRSImage currentImage = (XRSImage)e.Argument;
					this.lastPaintedImage = currentImage;
					Stopwatch stopwatch = Stopwatch.StartNew();
					bool flag2 = Application.Current != null;
					if (flag2)
					{
						Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
						{
							foreach (LaneXri laneXri in this.ShownLanes)
							{
								bool active = laneXri.Active;
								if (active)
								{
									bool flag4 = !this.productManager.CurrentLoadedProduct.IsRegionProduct;
									if (flag4)
									{
										bool flag5 = this.resetImages;
										if (flag5)
										{
											laneXri.resetImagesTemp();
											this.resetImages = false;
										}
										laneXri.ViewImage(currentImage, this.CurrentUser.Rights > BuiltInPriviledges.NoUser, !this.StandardMode, this.isBulkLiveImage);
									}
									else
									{
										bool flag6 = laneXri.Group == 0 || laneXri == this.selectedXriLane;
										if (flag6)
										{
											this.ViewRegionImage(laneXri);
										}
									}
								}
							}
						}));
					}
					stopwatch.Stop();
					this.parHandling.WriteValue(MainWindowViewModel.timePaintImagePath, stopwatch.ElapsedMilliseconds.ToString(), false);
				}
			}
			catch (Exception ex)
			{
				Exception ex3;
				Exception ex2 = ex3;
				Exception ex = ex2;
				Action action = delegate()
				{
					Dialog.Show("Error: bgWorkerPaintImages_DoWork", ex.Message);
				};
				bool flag3 = Application.Current != null && !Application.Current.Dispatcher.CheckAccess();
				if (flag3)
				{
					Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, action);
				}
				else
				{
					action();
				}
			}
		}

		// Token: 0x06002461 RID: 9313 RVA: 0x000F0054 File Offset: 0x000EE254
		private void ProductChange(object obj)
		{
			bool flag = this.dbConnection != null;
			if (flag)
			{
				bool multiProductsEnabled = false;
				bool multiLaneProductsEnabled = false;
				bool multiRegionEnabled = false;
				bool bulkObjectEnabled = false;
				this.dbConnection.GetMultiProductsEnabled(ref multiProductsEnabled);
				this.dbConnection.GetMultiLaneProductsEnabled(ref multiLaneProductsEnabled);
				this.dbConnection.GetMultiRegionEnabled(ref multiRegionEnabled);
				this.dbConnection.GetBulkObjectEnabled(ref bulkObjectEnabled);
				this.prodChange = new ProductChangeControl(this.dbConnection, this.parHandling, this.imageTransfer, this.productManager, MainWindowViewModel.accessRightsManager, multiProductsEnabled, multiLaneProductsEnabled, multiRegionEnabled, bulkObjectEnabled, this.imageArchiver, this.setFooterMainWindow);
				Subwindow.Show(this.prodChange, SubwindowButtons.Back, this.prodChange.Buttons);
			}
		}

		// Token: 0x06002462 RID: 9314 RVA: 0x000F0108 File Offset: 0x000EE308
		private void prodChange_ProductChanged(ProductData oldProduct, ProductData newProduct)
		{
			List<LaneXri> list = (from lane in this.ShownLanes
			where lane.Lane == Lane.All
			select lane).ToList<LaneXri>();
			uint num = 0U;
			uint num2 = 0U;
			list[0].Counter.GoodSinceRetrainCounter = 0U;
			this.AutoRetrainUndo = false;
			this.RetrainMode = false;
			MachineMode machineMode = MachineMode.Standard;
			bool flag = list.Count > 0;
			if (flag)
			{
				num = list[0].Counter.ThroughputCounter;
				num2 = list[0].Counter.RejectCounter;
			}
			else
			{
				bool flag2 = this.ShownLanes.Count > 0;
				if (flag2)
				{
					num = this.ShownLanes[0].Counter.ThroughputCounter;
					num2 = this.ShownLanes[0].Counter.RejectCounter;
				}
			}
			this.dbConnection.GetMachineMode(ref machineMode);
			string param = (machineMode != MachineMode.Bulk) ? num2.ToString() : this.valveActivity.SumCurrentActivities.ToString();
			LogbookManager.AddEntry(SSTEntryCode.ProductChanged, 0, oldProduct.ProductNumber, oldProduct.ProductName, newProduct.ProductNumber, newProduct.ProductName);
			LogbookManager.AddEntry(SSTEntryCode.ProductEnded, 0, oldProduct.ProductNumber, oldProduct.ProductName, num.ToString(), param);
			this.valveActivity.SumCurrentActivities = 0;
			this.AddWeightStatLogbookEntry(oldProduct.TableName);
			this.ResetAllCounters();
			this.SetActiveLanes();
			this.parHandling.WriteValue(Definition.resetRejecterErrorsPath, "1", false);
			this.Vibro.UpdateSpeedFromDatabase();
			this.ShowBatchWindowOrStartNew(oldProduct, newProduct);
			bool flag3 = newProduct.ProductType == ProductType.SingleProduct;
			if (flag3)
			{
				this.LoadCurrentMaterialCurve(0);
			}
			else
			{
				for (int i = 0; i < newProduct.SubProducts.Count; i++)
				{
					this.LoadCurrentMaterialCurve((int)newProduct.SubProducts[i].Group);
				}
			}
			this.auditSettings.Initialized = false;
			this.auditSettings.GetCurrentValues();
			this.auditSettings.Initialized = true;
			int num3 = 0;
			this.dbConnection.GetAuditCheckType(ref num3);
			bool flag4 = num3 == 1;
			if (flag4)
			{
				bool flag5 = false;
				this.dbConnection.GetAuditCheckProductChange(ref flag5);
				bool flag6 = flag5 && (this.auditSettings.ContaminationEnabled || this.auditSettings.WeightEnabled || this.auditSettings.CountEnabled);
				if (flag6)
				{
					new Thread(new ThreadStart(this.auditSettings.AuditRequired.TriggerAuditCheck))
					{
						Name = "TriggerAuditCheck"
					}.Start();
				}
			}
			else
			{
				bool flag7 = this.auditSettings.SelectedMode == AlarmMode.PerProductChange && (this.auditSettings.ContaminationEnabled || this.auditSettings.WeightEnabled || this.auditSettings.CountEnabled);
				if (flag7)
				{
					new Thread(new ThreadStart(this.auditSettings.AuditRequired.TriggerAuditCheck))
					{
						Name = "TriggerAuditCheck"
					}.Start();
				}
			}
			bool flag8 = this.parHandling != null && this.BulkModeActive;
			if (flag8)
			{
				string value = (this.parHandling.ReadPar(FlapsOverIOs.bulkflapsEnableValvesPath) == "1") ? "0" : "1";
				this.parHandling.WriteValue(FlapsOverIOs.bulkflapsEnableValvesDOPath, value);
				int num4 = 0;
				int.TryParse(this.parHandling.ReadPar("/filter/comparer/comparer1/Contrast/Value"), out num4);
				this.SliderVisibleBulkMode = (this.CurrentUser.Rights >= BuiltInPriviledges.Operator1 && num4 < 10000);
				this.machineState.StartMessage.BulkOnOff = true;
			}
			bool flag9 = this.parHandling != null && this.BulkObjectModeActive;
			if (flag9)
			{
				this.parHandling.WriteValue(FlapsOverIOs.bulkflapsEnableValvesDOPath, "0");
				this.machineState.StartMessage.BulkOnOff = false;
			}
			this.testcardsimservice.CheckAndSetActivationTestcardSim(newProduct);
			this.testcardsimservice.SetProperties();
			this.testcardsimservice.setOrCheckImgToSimulateIsNew(true);
			this.testcardsimservice.ProductChangedForStatisticTable = true;
			this.rejecterActivity.StartUpdateActivity();
			this.setFooterMainWindow.UnSetFooterDeactivation();
			bool flag10 = false;
			this.dbConnection.GetHighThroughputMachine(ref flag10);
			string text = "";
			this.dbConnection.GetParameterValue(Definition.highThroughputEnabled, ref text);
			bool flag11 = flag10 && text.Equals("1");
			if (flag11)
			{
				bool flag12 = this.machineState.State == MachineStates.Inspecting || this.DemoMode;
				if (flag12)
				{
					this.ShowNext = true;
				}
				else
				{
					this.ShowNext = false;
				}
				this.HighSpeedMachineActive = true;
				string text2 = this.parHandling.ReadPar("/filter/acquisition/SaveOnlyIfContaminant/Value");
				bool flag13 = text2.Equals("0");
				if (flag13)
				{
					this.setHighSpeedValues(true);
				}
			}
			else
			{
				this.HighSpeedMachineActive = false;
				this.ShowNext = false;
				bool flag14 = !flag10 && text.Equals("1");
				if (flag14)
				{
					string text3 = this.parHandling.ReadPar("/filter/acquisition/SaveOnlyIfContaminant/Value");
					bool flag15 = text3.Equals("1");
					if (flag15)
					{
						this.setHighSpeedValues(false);
					}
				}
			}
			string a = "";
			this.dbConnection.GetParameterValue(Definition.bulkShowContImages, ref a);
			bool flag16 = a == "1";
			if (flag16)
			{
				this.BulkContImagesShown = true;
				this.BulkObjectModeSourcePath = "..\\img\\xriimageBulkCont.png";
			}
			else
			{
				this.BulkContImagesShown = false;
				this.BulkObjectModeSourcePath = "..\\img\\xriimageBulkJustCont.png";
			}
			this.aiService.SetAIModelID();
			bool flag17 = this.parHandling.ReadPar(Definition.anomalyDetectionEnabled) == "1" || this.parHandling.ReadPar(Definition.objectDetectionEnabled) == "1";
			if (flag17)
			{
				this.RefreshColourBtnAIStatus();
			}
			this.changing = false;
			bool flag18 = this.parHandling.ReadPar(Definition.detectorMonitoringActive) == "1";
			if (flag18)
			{
				Thread thread = new Thread(delegate()
				{
					Thread.Sleep(2000);
					Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(delegate()
					{
						bool flag19 = this.machineState.State == MachineStates.Inspecting && this.parHandling.ReadPar(Definition.detectorMonitoringBackgroundEnable) == "0";
						if (flag19)
						{
							this.parHandling.WriteValue(Definition.detectorMonitoringBackgroundEnable, "1");
						}
					}));
				});
				thread.Start();
			}
		}

		// Token: 0x06002463 RID: 9315 RVA: 0x000F077C File Offset: 0x000EE97C
		private void ResetAllCounters()
		{
			foreach (LaneXri laneXri in this.AllLanes)
			{
				laneXri.Counter.ResetCounters();
				laneXri.Image = null;
				laneXri.TempImage = null;
			}
			foreach (LaneXri laneXri2 in this.AllSubProducts)
			{
				laneXri2.Counter.ResetCounters();
				laneXri2.Image = null;
				laneXri2.TempImage = null;
			}
		}

		// Token: 0x06002464 RID: 9316 RVA: 0x000F0844 File Offset: 0x000EEA44
		private void ShowBatchWindowOrStartNew(ProductData oldProduct, ProductData newProduct)
		{
			bool flag = false;
			this.dbConnection.GetBatchesEnabled(ref flag);
			bool flag2 = flag;
			if (flag2)
			{
				bool flag3 = this.lastSSTBatch == null;
				if (flag3)
				{
					bool flag4 = oldProduct != null;
					if (flag4)
					{
						string text = null;
						this.dbConnection.GetCurrentBatchID(ref text, oldProduct.TableName);
						bool flag5 = text != null && text.Length > 0;
						if (flag5)
						{
							this.dbConnection.EndBatch(text, oldProduct.TableName);
							this.dbConnection.SetCurrentBatchID("", oldProduct.TableName);
						}
					}
					this.productManager.ShowBatchWindow(true);
				}
				else
				{
					bool flag6 = oldProduct != null && newProduct != null;
					if (flag6)
					{
						string text2 = null;
						this.dbConnection.GetCurrentBatchID(ref text2, oldProduct.TableName);
						bool flag7 = text2 != null && text2.Length > 0;
						if (flag7)
						{
							this.dbConnection.EndBatch(text2, oldProduct.TableName);
						}
						this.dbConnection.CreateNewBatch(this.lastSSTBatch, newProduct.TableName);
						this.dbConnection.StartBatch(this.lastSSTBatch, newProduct.TableName, text2);
						newProduct.CurrentBatchName = this.lastSSTBatch;
					}
					this.lastSSTBatch = null;
				}
			}
			else
			{
				this.lastSSTBatch = null;
			}
		}

		// Token: 0x06002465 RID: 9317 RVA: 0x000F099C File Offset: 0x000EEB9C
		private void ShowSubmenu(object obj)
		{
			SubMenu subMenu = new SubMenu(this.parHandling, this.imageTransfer, this.logger, this.dbConnection, this.machineState, MainWindowViewModel.accessRightsManager, this.conveyor, this.auditSettings, this.restore, this.backup, this.SelectedBranding, this.productManager, this.logbook, this.errorHandling, this.imageArchiver, this.wifiClient, this.Vibro, this.messageSystem, this.communicationsList, this.testcardsimservice, this.setFooterMainWindow, this.dbDatalogs, this.errorSurvey, this, this.imageTransfer, this.aiService, this.xrayState);
			subMenu.ViewModelNormal.GraphPlotInitialized += this.submenu_GraphPlotInitialized;
			subMenu.ViewModelNormal.GraphPlotUnloaded += this.submenu_GraphPlotUnloaded;
			Subwindow.Show(subMenu, SubwindowButtons.Back);
			this.deactivateBulkLiveMode();
		}

		// Token: 0x06002466 RID: 9318 RVA: 0x000F0A8C File Offset: 0x000EEC8C
		private void submenu_GraphPlotUnloaded(MachineStates oldState)
		{
			Progress.ShowStaticProgress(LocalizationManager.Localize("txtProcessing"), false);
			new Thread(delegate()
			{
				MachineMode machineMode = MachineMode.Standard;
				DetectorType detectorType = DetectorType.DT;
				this.dbConnection.GetDetectorType(ref detectorType);
				Thread.Sleep(1000);
				this.parHandling.WriteValue(XRayOnProcessing.manualOffsetCalibrationPath, "1", false);
				this.parHandling.WriteValue(XRayOnProcessing.manualGainCalibrationPath, "1", false);
				this.dbConnection.GetMachineMode(ref machineMode);
				bool flag = machineMode == MachineMode.Bulk && this.machineState.State == MachineStates.ViewingPlot && oldState == MachineStates.Inspecting;
				if (flag)
				{
					Thread.Sleep(1000);
					BulkMode.Activate(this.dbConnection, this.parHandling);
				}
				Thread.Sleep(4000);
				this.imageTransfer.FrameArrived -= this.imageTransfer_FrameArrived;
				this.imageTransfer.FrameArrived += this.imageTransfer_FrameArrived;
				bool flag2 = this.machineState.State == MachineStates.ViewingPlot;
				if (flag2)
				{
					this.machineState.State = oldState;
				}
				foreach (LaneXri laneXri in this.ShownLanes)
				{
					laneXri.Counter.StartCounting();
				}
				string value = "";
				bool parameterValue = this.dbConnection.GetParameterValue(Rejecter.rejecter1MainPath + "/Enabled/Value", ref value);
				if (parameterValue)
				{
					this.parHandling.WriteValue(Rejecter.rejecter1MainPath + "/Enabled/Value", value, false);
				}
				bool parameterValue2 = this.dbConnection.GetParameterValue(Rejecter.rejecter2MainPath + "/Enabled/Value", ref value);
				if (parameterValue2)
				{
					this.parHandling.WriteValue(Rejecter.rejecter2MainPath + "/Enabled/Value", value, false);
				}
				bool parameterValue3 = this.dbConnection.GetParameterValue(Rejecter.rejecter3MainPath + "/Enabled/Value", ref value);
				if (parameterValue3)
				{
					this.parHandling.WriteValue(Rejecter.rejecter3MainPath + "/Enabled/Value", value, false);
				}
				bool parameterValue4 = this.dbConnection.GetParameterValue(Rejecter.rejecter4MainPath + "/Enabled/Value", ref value);
				if (parameterValue4)
				{
					this.parHandling.WriteValue(Rejecter.rejecter4MainPath + "/Enabled/Value", value, false);
				}
				Progress.CloseStaticProgress(false);
			})
			{
				Name = "ResetStateAndParameters"
			}.Start();
		}

		// Token: 0x06002467 RID: 9319 RVA: 0x000F0AE4 File Offset: 0x000EECE4
		private void submenu_GraphPlotInitialized()
		{
			this.imageTransfer.FrameArrived -= this.imageTransfer_FrameArrived;
			foreach (LaneXri laneXri in this.ShownLanes)
			{
				laneXri.Counter.StopCounting();
			}
		}

		// Token: 0x06002468 RID: 9320 RVA: 0x000F0B54 File Offset: 0x000EED54
		private void ShowLogbook(object obj)
		{
			bool flag = this.logbook != null;
			if (flag)
			{
				CtrlLogbook ctrlLogbook = new CtrlLogbook(this.logbook, MainWindowViewModel.accessRightsManager, this.dbConnection, this.setFooterMainWindow);
				Subwindow.Show(ctrlLogbook, SubwindowButtons.Back, ctrlLogbook.Buttons);
				this.deactivateBulkLiveMode();
			}
		}

		// Token: 0x06002469 RID: 9321 RVA: 0x000F0BA4 File Offset: 0x000EEDA4
		private void Autolearn(object obj)
		{
			MachineType machinetype = MachineType.MachineTypeUndefined;
			this.dbConnection.GetMachineType(ref machinetype);
			MachineMode machineMode = MachineMode.Standard;
			this.dbConnection.GetMachineMode(ref machineMode);
			this.AutoRetrainUndo = false;
			this.RetrainMode = false;
			bool flag = machineMode == MachineMode.Bulk;
			if (flag)
			{
				this.BulkInAutotrainMode = true;
			}
			bool flag2 = this.productManager != null;
			if (flag2)
			{
				this.setFooterMainWindow.SetFooterDeactivation();
				bool flag3 = this.productManager.CurrentLoadedProduct.IsSingleProduct || this.productManager.CurrentLoadedProduct.IsRegionProduct || this.productManager.CurrentLoadedProduct.IsBulkObjectProduct;
				if (flag3)
				{
					bool flag4 = Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtOverwriteExisting"), DialogStyle.Warning, DialogButtons.OKCancel) == Dialog.DialogResults.Ok;
					if (flag4)
					{
						this.StartAutotrainSingleProduct(machinetype, MainWindowViewModel.accessRightsManager);
					}
					else
					{
						this.setFooterMainWindow.UnSetFooterDeactivation();
					}
				}
				else
				{
					bool flag5;
					if (this.productManager.CurrentLoadedProduct.IsLaneProduct)
					{
						flag5 = ((from product in this.productManager.CurrentLoadedProduct.SubProducts
						where product.ProductAssigned
						select product).ToList<ProductData>().Count > 0);
					}
					else
					{
						flag5 = true;
					}
					bool flag6 = flag5;
					if (flag6)
					{
						this.productManager.SelectedProduct = this.productManager.CurrentLoadedProduct;
						CtrlSubProduct ctrlSubProduct = new CtrlSubProduct(this.productManager, SubProductModes.Learn, null, this.setFooterMainWindow);
						Subwindow.Show(ctrlSubProduct, SubwindowButtons.BackAndHome, ctrlSubProduct.Buttons);
					}
					else
					{
						bool flag7 = Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtNoProductAssigned"), DialogStyle.Error, DialogButtons.OK) == Dialog.DialogResults.Ok;
						if (flag7)
						{
							this.setFooterMainWindow.UnSetFooterDeactivation();
						}
					}
				}
			}
		}

		// Token: 0x0600246A RID: 9322 RVA: 0x000F0D68 File Offset: 0x000EEF68
		private void ctrlAutotrain_Unloaded(object sender, RoutedEventArgs e)
		{
			int num = 0;
			MachineMode machineMode = MachineMode.Standard;
			this.dbConnection.GetMachineMode(ref machineMode);
			bool flag = this.parHandling != null && this.BulkModeActive;
			if (flag)
			{
				int.TryParse(this.parHandling.ReadPar("/filter/comparer/comparer1/Contrast/Value"), out num);
				this.SliderVisibleBulkMode = (this.CurrentUser.Rights >= BuiltInPriviledges.Operator1 && num < 10000);
			}
			this.ctrlAutotrain.Unloaded -= this.ctrlAutotrain_Unloaded;
			this.backup.StartAutoBackupTimer();
			this.setFooterMainWindow.UnSetFooterDeactivation();
			bool flag2 = machineMode == MachineMode.Bulk;
			if (flag2)
			{
				this.BulkInAutotrainMode = false;
				this.messageSystem.RemoveAllMessages();
			}
		}

		// Token: 0x0600246B RID: 9323 RVA: 0x000F0E29 File Offset: 0x000EF029
		private void ChangeUser(object obj)
		{
			this.ChangeUser(false);
		}

		// Token: 0x0600246C RID: 9324 RVA: 0x000F0E34 File Offset: 0x000EF034
		private void ChangeUser(bool multipleRfid)
		{
			bool flag = this._ctrlUsr == null;
			if (flag)
			{
				this._ctrlUsr = new UsermanagementControl(this.dbConnection, MainWindowViewModel.accessRightsManager, this.restore, multipleRfid, this.setFooterMainWindow, this.loginToGo);
				Subwindow.Show(this._ctrlUsr, SubwindowButtons.Back, this._ctrlUsr.Buttons);
				this._ctrlUsr.Unloaded += this._ctrlUsr_Unloaded;
			}
		}

		// Token: 0x0600246D RID: 9325 RVA: 0x000F0EAA File Offset: 0x000EF0AA
		private void _ctrlUsr_Unloaded(object sender, RoutedEventArgs e)
		{
			this._ctrlUsr = null;
		}

		// Token: 0x0600246E RID: 9326 RVA: 0x000F0EB4 File Offset: 0x000EF0B4
		private bool LoadCurrentApplication()
		{
			MainWindowViewModel.<>c__DisplayClass727_0 CS$<>8__locals1 = new MainWindowViewModel.<>c__DisplayClass727_0();
			CS$<>8__locals1.<>4__this = this;
			List<string> list = new List<string>();
			List<string> list2 = new List<string>();
			List<string> list3 = new List<string>();
			string text = "";
			string tableName = "";
			CS$<>8__locals1.currentProductName = "";
			ProductType productType = ProductType.SingleProduct;
			this.dbConnection.GetMainProductTableNames(list3);
			bool flag = list3.Count == 1;
			bool flag3;
			if (flag)
			{
				string text2 = this.dbConnection.CreateNewProductTableName();
				bool flag2 = this.dbConnection.CreateNewAppl(Definition.defaultVisibleProductName, this.defaultVisibleProductNumber.ToString(), ProductType.SingleProduct, text2);
				this.dbConnection.ChangeAppl(text2, null, null);
				bool currentProductEntries = this.dbConnection.GetCurrentProductEntries(list, list2);
				flag3 = (flag2 && currentProductEntries);
			}
			else
			{
				flag3 = this.dbConnection.GetCurrentProductEntries(list, list2);
			}
			this.dbConnection.GetCurrentProductTableName(ref text);
			this.dbConnection.GetProductType(text, ref productType);
			this.dbConnection.GetCurrentAppName(ref CS$<>8__locals1.currentProductName);
			this.dbConnection.GetCurrentProductTableNameAI(ref tableName);
			bool flag4 = TableOperations.TableExists(this.dbConnection.Connection, tableName);
			if (flag4)
			{
				bool productEntries = this.dbConnection.GetProductEntries(tableName, list, list2);
				string text3 = "";
				this.dbConnection.GetCurrentProductTableNameTrainingAI(ref text3);
				bool flag5 = text3 != "" && productEntries;
				if (flag5)
				{
					text3 = text3.Insert("AppDefault".Length, "AI");
					List<string> list4 = new List<string>();
					List<string> list5 = new List<string>();
					productEntries = this.dbConnection.GetProductEntries(text3, list4, list5);
					for (int j = 0; j < list4.Count; j++)
					{
						string text4 = list4[j];
						bool flag6 = text4.StartsWith("/filter/anomalyDetection/train");
						if (flag6)
						{
							int num = list.IndexOf(text4);
							bool flag7 = num != -1;
							if (flag7)
							{
								list2[num] = list5[j];
							}
						}
					}
				}
				flag3 = (flag3 && productEntries);
			}
			this.WriteParameters(list, list2, false);
			bool flag8 = false;
			this.dbConnection.GetAnomalyDetectionEnabled(ref flag8);
			bool flag9 = !flag8;
			if (flag9)
			{
				this.dbConnection.GetObjectDetectionEnabled(ref flag8);
			}
			bool flag10 = flag8;
			if (flag10)
			{
				this.RefreshColourBtnAIStatus();
			}
			bool flag11 = productType > ProductType.SingleProduct;
			if (flag11)
			{
				MainWindowViewModel.<>c__DisplayClass727_1 CS$<>8__locals2 = new MainWindowViewModel.<>c__DisplayClass727_1();
				CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
				CS$<>8__locals2.subProductList = new List<string>();
				this.dbConnection.GetSubProductTablenames(text, CS$<>8__locals2.subProductList);
				Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
				{
					CS$<>8__locals2.CS$<>8__locals1.<>4__this.progLoadMultiProductAtStartup = new Progress(string.Concat(new string[]
					{
						LocalizationManager.Localize("txtApplication"),
						" \"",
						CS$<>8__locals2.CS$<>8__locals1.currentProductName,
						"\" ",
						LocalizationManager.Localize("msgLoadingApplication"),
						Environment.NewLine
					}), false);
					CS$<>8__locals2.CS$<>8__locals1.<>4__this.progLoadMultiProductAtStartup.Show();
					CS$<>8__locals2.CS$<>8__locals1.<>4__this.progLoadMultiProductAtStartup.progBar.Value = (double)(100 / (CS$<>8__locals2.subProductList.Count + 1));
				}));
				int i2;
				int i;
				for (i = 0; i < CS$<>8__locals2.subProductList.Count; i = i2 + 1)
				{
					int num2 = 0;
					string s = CS$<>8__locals2.subProductList[i].Remove(0, CS$<>8__locals2.subProductList[i].IndexOf(Definition.subTableIdentifier) + 1);
					int.TryParse(s, out num2);
					this.parHandling.AddGroupToParameterServer(num2);
					bool flag12 = productType == ProductType.RegionProduct;
					if (flag12)
					{
						this.parHandling.AddRegionToParameterServer(num2);
					}
					list.Clear();
					list2.Clear();
					this.dbConnection.GetCurrentSubProductEntries(num2, list, list2);
					this.WriteParameters(list, list2, true);
					Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(delegate()
					{
						CS$<>8__locals2.CS$<>8__locals1.<>4__this.progLoadMultiProductAtStartup.progBar.Value = (double)((i + 2) * 100 / (CS$<>8__locals2.subProductList.Count + 1));
					}));
					i2 = i;
				}
			}
			Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
			{
				bool flag13 = CS$<>8__locals1.<>4__this.progLoadMultiProductAtStartup != null;
				if (flag13)
				{
					CS$<>8__locals1.<>4__this.progLoadMultiProductAtStartup.Close();
					CS$<>8__locals1.<>4__this.progLoadMultiProductAtStartup = null;
				}
			}));
			return flag3;
		}

		// Token: 0x0600246F RID: 9327 RVA: 0x000F12A4 File Offset: 0x000EF4A4
		private void WriteParameters(List<string> pathes, List<string> values, bool isMulti)
		{
			MainWindowViewModel.<>c__DisplayClass728_0 CS$<>8__locals1 = new MainWindowViewModel.<>c__DisplayClass728_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.pathes = pathes;
			CS$<>8__locals1.waitText = LocalizationManager.Localize("txtWaitForCalibration");
			bool flag = false;
			int num = 0;
			int i;
			int j;
			for (i = 0; i < CS$<>8__locals1.pathes.Count; i = j + 1)
			{
				bool flag2 = this.progLoadMultiProductAtStartup != null;
				if (flag2)
				{
					while (this.machineState.State == MachineStates.XrayOnCalibrationNotDone)
					{
						flag = true;
						Dispatcher dispatcher = Application.Current.Dispatcher;
						DispatcherPriority priority = DispatcherPriority.Normal;
						Action method;
						if ((method = CS$<>8__locals1.<>9__0) == null)
						{
							method = (CS$<>8__locals1.<>9__0 = delegate()
							{
								bool flag11 = !CS$<>8__locals1.<>4__this.progLoadMultiProductAtStartup.txtHeader.Text.Contains(CS$<>8__locals1.waitText);
								if (flag11)
								{
									TextBlock txtHeader = CS$<>8__locals1.<>4__this.progLoadMultiProductAtStartup.txtHeader;
									txtHeader.Text += CS$<>8__locals1.waitText;
								}
							});
						}
						dispatcher.Invoke(priority, method);
						Thread.Sleep(1000);
						num++;
						bool flag3 = num >= 60;
						if (flag3)
						{
							num = 0;
							break;
						}
					}
				}
				bool flag4 = flag;
				if (flag4)
				{
					flag = false;
					Dispatcher dispatcher2 = Application.Current.Dispatcher;
					DispatcherPriority priority2 = DispatcherPriority.Normal;
					Action method2;
					if ((method2 = CS$<>8__locals1.<>9__1) == null)
					{
						method2 = (CS$<>8__locals1.<>9__1 = delegate()
						{
							bool flag11 = CS$<>8__locals1.<>4__this.progLoadMultiProductAtStartup != null && CS$<>8__locals1.<>4__this.progLoadMultiProductAtStartup.txtHeader.Text.Contains(CS$<>8__locals1.waitText);
							if (flag11)
							{
								CS$<>8__locals1.<>4__this.progLoadMultiProductAtStartup.txtHeader.Text = CS$<>8__locals1.<>4__this.progLoadMultiProductAtStartup.txtHeader.Text.Substring(0, CS$<>8__locals1.<>4__this.progLoadMultiProductAtStartup.txtHeader.Text.IndexOf(CS$<>8__locals1.waitText));
							}
						});
					}
					dispatcher2.Invoke(priority2, method2);
				}
				bool flag5 = CS$<>8__locals1.pathes[i].StartsWith(Definition.SVTValvesAssignmentMainPath);
				if (!flag5)
				{
					bool flag6 = CS$<>8__locals1.pathes[i] == "ConveyorSpeed" && !isMulti;
					if (flag6)
					{
						double speed;
						bool flag7 = double.TryParse(values[i], NumberStyles.Float, new CultureInfo("en-US"), out speed);
						if (flag7)
						{
							this.conveyor.Speed = speed;
							this.conveyor.AdaptScanrate(false, "");
						}
					}
					else
					{
						bool flag8 = CS$<>8__locals1.pathes[i] == "MinWeight" || CS$<>8__locals1.pathes[i] == "MaxWeight" || CS$<>8__locals1.pathes[i] == Definition.strReferenceWeight;
						if (flag8)
						{
							goto IL_2DF;
						}
						bool flag9 = CS$<>8__locals1.pathes[i] != Definition.detectorDTScanratePath;
						if (flag9)
						{
							this.parHandling.WriteValue(CS$<>8__locals1.pathes[i], values[i], false);
						}
					}
					bool flag10 = this.progressRestore != null;
					if (flag10)
					{
						Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
						{
							CS$<>8__locals1.<>4__this.progressRestore.progBar.Value = (double)(i * 100 / CS$<>8__locals1.pathes.Count);
						}));
					}
				}
				IL_2DF:
				j = i;
			}
		}

		// Token: 0x06002470 RID: 9328 RVA: 0x000F15C8 File Offset: 0x000EF7C8
		private void parHandling_ParameterChanged(object sender, ParameterChangeEventArgs e)
		{
			try
			{
				bool flag = e.Path.StartsWith(Definition.defaultProductParameterMainPath + Definition.defaultMainProductParameterSubPath);
				if (flag)
				{
					string text = e.Path.Remove(0, (Definition.defaultProductParameterMainPath + Definition.defaultMainProductParameterSubPath).Length);
					bool flag2 = e.Path.Split(new char[]
					{
						'/'
					}).Length == 5 && text.IndexOf("/Value") > 1;
					if (flag2)
					{
						text = text.Substring(1, text.IndexOf("/Value") - 1);
					}
					bool flag3 = text.StartsWith("/filter/anomalyDetection") || text.StartsWith("/filter/objectDetection");
					if (flag3)
					{
						bool flag4 = TableOperations.TableExists(this.dbConnection.Connection, DBConnection.defaultApplicationAI);
						if (flag4)
						{
							this.dbConnection.UpdateTable(DBConnection.defaultApplicationAI, text, e.NewValue.Replace(',', '.'));
						}
					}
					else
					{
						this.dbConnection.UpdateTable(DBConnection.defaultApplication, text, e.NewValue);
					}
				}
				else
				{
					bool flag5 = e.Path.StartsWith(Definition.defaultProductParameterMainPath + Definition.defaultSubProductParameterSubPath);
					if (flag5)
					{
						string text2 = e.Path.Remove(0, (Definition.defaultProductParameterMainPath + Definition.defaultSubProductParameterSubPath).Length);
						bool flag6 = e.Path.Split(new char[]
						{
							'/'
						}).Length == 5 && text2.IndexOf("/Value") > 1;
						if (flag6)
						{
							text2 = text2.Substring(1, text2.IndexOf("/Value") - 1);
						}
						this.dbConnection.UpdateTable(DBConnection.defaultSubApplication, text2, e.NewValue);
					}
					else
					{
						bool flag7 = this.dbConnection != null;
						if (flag7)
						{
							string text3 = null;
							this.dbConnection.GetCurrentProductTableName(ref text3);
							bool flag8 = e.Path.StartsWith(Definition.filterMainPath) && !e.Path.StartsWith(Definition.filterMainPath + "/");
							if (flag8)
							{
								int num;
								for (int i = 1; i <= Definition.maxSubProducts; i = num + 1)
								{
									bool flag9 = e.Path.StartsWith(Definition.filterMainPath + i.ToString());
									if (flag9)
									{
										text3 = text3 + Definition.subTableIdentifier.ToString() + i.ToString();
										break;
									}
									num = i;
								}
							}
							this.dbConnection.UpdateTable(text3, e.Path, e.NewValue.Replace(',', '.'));
							this.UpdateRegions(text3, e.Path, e.NewValue);
							bool flag10 = e.Path.StartsWith("/filter/anomalyDetection") || e.Path.StartsWith("/filter/objectDetection");
							if (flag10)
							{
								string tableName = null;
								bool flag11 = e.Path.StartsWith("/filter/anomalyDetection/inference") || e.Path.StartsWith("/filter/objectDetection/inference");
								if (flag11)
								{
									this.dbConnection.GetCurrentProductTableNameAI(ref tableName);
								}
								else
								{
									bool flag12 = e.Path.StartsWith("/filter/anomalyDetection/train");
									if (flag12)
									{
										string text4 = null;
										string text5 = this.parHandling.ReadPar(Definition.anomalyDetectionTrainModelID);
										bool flag13 = text5 == "-1";
										if (flag13)
										{
											text5 = this.productManager.CurrentLoadedProduct.ProductNumber;
										}
										this.dbConnection.GetAppTableNameWithNumber(text5, ref text4);
										bool flag14 = text4 != null;
										if (flag14)
										{
											tableName = text4.Insert("AppDefault".Length, "AI");
										}
									}
								}
								bool flag15 = TableOperations.TableExists(this.dbConnection.Connection, tableName);
								if (flag15)
								{
									this.dbConnection.UpdateTable(tableName, e.Path, e.NewValue.Replace(',', '.'));
								}
							}
							bool flag16 = e.Path == Conveyor.conveyingDirectionPath;
							if (flag16)
							{
								bool flag17 = e.NewValue == "0";
								if (flag17)
								{
									this.dbConnection.SetConveyingDirection(ConveyingDirection.LeftToRight);
								}
								else
								{
									bool flag18 = e.NewValue == "1";
									if (flag18)
									{
										this.dbConnection.SetConveyingDirection(ConveyingDirection.RightToLeft);
									}
								}
							}
							bool flag19 = e.Path == MachineState.lastXRayOnPath;
							if (flag19)
							{
								DateTime dateTime = default(DateTime);
								bool flag20 = DateTime.TryParse(e.NewValue.Replace('-', '.').Replace('_', '.'), new CultureInfo("de-DE"), DateTimeStyles.None, out dateTime);
								if (flag20)
								{
									this.parHandling.WriteValue(e.Path, e.NewValue.Replace('-', '.').Replace('_', '.'), false);
									this.dbConnection.SetLastXrayOn(e.NewValue.Replace('-', '.').Replace('_', '.'));
								}
								else
								{
									this.parHandling.WriteValue(e.Path, "01.01.2010", false);
									this.dbConnection.SetLastXrayOn("01.01.2010");
								}
							}
							bool flag21 = e.Path == MainWindowViewModel.ipWagoPath;
							if (flag21)
							{
								this.dbConnection.SetIPWago(this.parHandling.ReadPar(MainWindowViewModel.ipWagoPath));
							}
							bool flag22 = e.Path == Conveyor.geometryFactorPath;
							if (flag22)
							{
								this.dbConnection.SetGeometryFactor(e.NewValue.Replace(',', '.'));
							}
							bool flag23 = e.Path == Conveyor.deformationFactorPath;
							if (flag23)
							{
								bool flag24 = this.conveyor != null;
								if (flag24)
								{
									this.conveyor.AdaptScanrate(false, "");
								}
							}
							bool flag25 = e.Path == ParameterHandling.ignoreLeftPixelPath;
							if (flag25)
							{
								int ignoredLeftPixel = 0;
								bool flag26 = int.TryParse(e.NewValue, out ignoredLeftPixel);
								if (flag26)
								{
									this.dbConnection.SetIgnoredLeftPixel(ignoredLeftPixel);
								}
								bool flag27 = this.parHandling.ReadPar(FlapsOverIOs.bulkflapsEnabledPath) == "1";
								if (flag27)
								{
									this.parHandling.WriteValue(FlapsOverIOs.bulkflapsCalcFlapsMappingPath, "1");
								}
								else
								{
									bool flag28 = this.parHandling.ReadPar(SVT.svtEnablePath) == "1";
									if (flag28)
									{
										this.SVTUpdateValues();
									}
								}
							}
							bool flag29 = e.Path == ParameterHandling.ignoreRightPixelPath;
							if (flag29)
							{
								int ignoredRightPixel = 0;
								bool flag30 = int.TryParse(e.NewValue, out ignoredRightPixel);
								if (flag30)
								{
									this.dbConnection.SetIgnoredRightPixel(ignoredRightPixel);
								}
								bool flag31 = this.parHandling.ReadPar(FlapsOverIOs.bulkflapsEnabledPath) == "1";
								if (flag31)
								{
									this.parHandling.WriteValue(FlapsOverIOs.bulkflapsCalcFlapsMappingPath, "1");
								}
								else
								{
									bool flag32 = this.parHandling.ReadPar(SVT.svtEnablePath) == "1";
									if (flag32)
									{
										this.SVTUpdateValues();
									}
								}
							}
							bool flag33 = e.Path.Contains("/global/LightTolerance/Value");
							if (flag33)
							{
								string text6 = e.Path.Remove(0, Definition.filterMainPath.Length);
								int num2 = text6.IndexOf('/');
								text6 = text6.Remove(num2, text6.Length - num2);
								List<string> list = new List<string>();
								int num;
								for (int j = 0; j < Definition.nObjectFinder; j = num + 1)
								{
									list.Add(string.Concat(new string[]
									{
										Definition.filterMainPath,
										text6,
										Definition.objectFindersSubPath,
										(j + 1).ToString(),
										"/",
										Definition.strLightTolerance,
										"/",
										Definition.strValue
									}));
									num = j;
								}
								this.UpdatePaths(list.ToArray(), text6);
							}
							bool flag34 = e.Path.Contains("/global/DenseTolerance/Value") || e.Path.Contains("/global/CustomTolerance/Value");
							if (flag34)
							{
								string text7 = e.Path.Remove(0, Definition.filterMainPath.Length);
								int num3 = text7.IndexOf('/');
								text7 = text7.Remove(num3, text7.Length - num3);
								List<string> list2 = new List<string>();
								int num;
								for (int k = 0; k < Definition.nObjectFinder; k = num + 1)
								{
									list2.Add(string.Concat(new string[]
									{
										Definition.filterMainPath,
										text7,
										Definition.objectFindersSubPath,
										(k + 1).ToString(),
										"/",
										Definition.strDenseTolerance,
										"/",
										Definition.strValue
									}));
									num = k;
								}
								this.UpdatePaths(list2.ToArray(), text7);
							}
							bool flag35 = e.Path.Contains("/global/Enable/Value") || e.Path.Contains(Definition.enableGlobalSmallSizeSubPath) || e.Path.Contains(Definition.enableGlobalMediumSizeSubPath) || e.Path.Contains(Definition.enableGlobalLargeSizeSubPath) || e.Path.Contains(Definition.enableGlobalCustomSizeSubPath);
							if (flag35)
							{
								string text8 = e.Path.Remove(0, Definition.filterMainPath.Length);
								int num4 = text8.IndexOf('/');
								text8 = text8.Remove(num4, text8.Length - num4);
								List<string> list3 = new List<string>();
								int num;
								for (int l = 0; l < Definition.nObjectFinder; l = num + 1)
								{
									list3.Add(string.Concat(new string[]
									{
										Definition.filterMainPath,
										text8,
										Definition.objectFindersSubPath,
										(l + 1).ToString(),
										"/",
										Definition.strEnabled,
										"/",
										Definition.strValue
									}));
									num = l;
								}
								list3.Add(Definition.filterMainPath + text8 + Definition.thresholdEnabledSubPath);
								list3.Add(Definition.filterMainPath + text8 + Definition.thresholdGlobalEnabledSubPath);
								this.UpdatePaths(list3.ToArray(), text8);
							}
							bool flag36 = e.Path.Contains("/global/CustomFilterSize/Value");
							if (flag36)
							{
								string text9 = e.Path.Remove(0, Definition.filterMainPath.Length);
								int num5 = text9.IndexOf('/');
								text9 = text9.Remove(num5, text9.Length - num5);
								List<string> list4 = new List<string>();
								int num;
								for (int m = 0; m < Definition.nObjectFinder; m = num + 1)
								{
									list4.Add(string.Concat(new string[]
									{
										Definition.filterMainPath,
										text9,
										Definition.objectFindersSubPath,
										(m + 1).ToString(),
										"/",
										Definition.strFilterSize,
										"/",
										Definition.strValue
									}));
									num = m;
								}
								this.UpdatePaths(list4.ToArray(), text9);
							}
							bool flag37 = e.Path == Definition.xRaySourceTypePath;
							if (flag37)
							{
								bool flag38 = e.NewValue == "0" || e.NewValue == "1";
								if (flag38)
								{
									this.dbConnection.SetXRaySourceType(e.NewValue);
								}
							}
							bool flag39 = e.Path == Definition.machineModePath;
							if (flag39)
							{
								string value = "0";
								bool flag40 = e.NewValue == "0" || e.NewValue == "1" || e.NewValue == "2" || e.NewValue == "3" || e.NewValue == "4";
								if (flag40)
								{
									try
									{
										MachineMode machineMode = (MachineMode)Enum.Parse(typeof(MachineMode), e.NewValue);
										bool flag41 = this.ProductManager.CurrentLoadedProduct.IsBulkObjectProduct && e.NewValue == "1";
										if (flag41)
										{
											machineMode = MachineMode.BulkObject;
											this.StandardMode = true;
										}
										else
										{
											this.StandardMode = (e.NewValue != "1");
										}
										this.dbConnection.SetMachineMode(machineMode);
										this.SliderVisibleBulkMode = false;
										bool flag42 = machineMode == MachineMode.BulkObject;
										if (flag42)
										{
											this.BulkObjectModeActive = true;
											this.machineState.StartMessage.BulkOnOff = false;
										}
										else
										{
											bool flag43 = machineMode == MachineMode.Bulk;
											if (flag43)
											{
												this.IsImageBoxVisible = false;
												int num6 = 0;
												int.TryParse(this.parHandling.ReadPar("/filter/comparer/comparer1/Contrast/Value"), out num6);
												this.SliderVisibleBulkMode = (this.CurrentUser.Rights >= BuiltInPriviledges.Operator1 && num6 < 10000);
												this.machineState.StartMessage.BulkOnOff = true;
											}
											else
											{
												this.machineState.StartMessage.BulkOnOff = false;
											}
										}
										this.selectedXriLane.UpdateThroughputString(machineMode, false);
									}
									catch
									{
									}
								}
								bool flag44 = e.NewValue == "1" || e.NewValue == "2" || e.NewValue == "4";
								if (flag44)
								{
									value = "0";
								}
								else
								{
									DetectorType detectorType = DetectorType.Unknown;
									this.dbConnection.GetDetectorType(ref detectorType);
									bool flag45 = detectorType != DetectorType.DTNetTDI;
									if (flag45)
									{
										value = "1";
									}
								}
								this.parHandling.WriteValue(Definition.isLiveCalibrationActivePath, value);
								this.dbConnection.SetLiveCalibrationMode(Convert.ToInt32(value));
							}
							bool flag46 = e.Path == Definition.recordLinesMeanValuePath;
							if (flag46)
							{
								bool flag47 = e.NewValue == "1" && e.OldValue == "0";
								if (flag47)
								{
									MeanPixelLogger.LogMode dbLogMode = (this.parHandling.ReadPar(Definition.logMeanValueToCsvPath) != "1") ? MeanPixelLogger.LogMode.Database : MeanPixelLogger.LogMode.CSV;
									bool offsetMode = this.parHandling.ReadPar(XRayOnProcessing.xrayEnablePath) != "1";
									this.imageTransfer.FrameArrived -= this.imageTransfer_FrameArrived;
									WdLogProgress wdLogProgress = new WdLogProgress(this.parHandling, this.meanPixelLogger, offsetMode);
									wdLogProgress.Show();
									this.meanPixelLogger.StartPixelLogging(dbLogMode, true, offsetMode, XRayOnProcessing.gainCalibrationValue);
								}
								bool flag48 = e.NewValue == "0" && e.OldValue == "1";
								if (flag48)
								{
									this.meanPixelLogger.StopPixelLogging(true);
									this.imageTransfer.FrameArrived += this.imageTransfer_FrameArrived;
								}
							}
							bool flag49 = e.Path == SVT.svtMinAirPressurePath;
							if (flag49)
							{
								double svtminAirPressure = 0.0;
								bool flag50 = double.TryParse(e.NewValue.Replace(',', '.'), NumberStyles.AllowDecimalPoint, new CultureInfo("en-US"), out svtminAirPressure);
								if (flag50)
								{
									this.dbConnection.SetSVTMinAirPressure(svtminAirPressure);
								}
							}
						}
						bool flag51 = e.Path == Definition.detectorTempCorrectionPath;
						if (flag51)
						{
							bool flag52 = e.NewValue == "1" && e.OldValue == "0";
							if (flag52)
							{
								this.dbConnection.SetDetectorTempCorrection(true);
							}
							bool flag53 = e.NewValue == "0" && e.OldValue == "1";
							if (flag53)
							{
								this.dbConnection.SetDetectorTempCorrection(false);
							}
						}
						bool flag54 = e.Path == Definition.resetDtCorrectionPath && e.NewValue == "1";
						if (flag54)
						{
							bool flag55 = this.dbConnection.DeleteDtCorrection(true, true, true);
							if (flag55)
							{
								Dialog.Show(LocalizationManager.Localize("txtSuccess"), LocalizationManager.Localize("txtSuccess"), DialogStyle.Ok, DialogButtons.None, true);
							}
							else
							{
								Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtFailed"), DialogStyle.Error, DialogButtons.None, true);
							}
						}
						bool flag56 = e.Path == Definition.resetDtGainCorrectionPath && e.NewValue == "1";
						if (flag56)
						{
							bool flag57 = this.dbConnection.DeleteDtCorrection(true, false, true);
							if (flag57)
							{
								Dialog.Show(LocalizationManager.Localize("txtSuccess"), LocalizationManager.Localize("txtSuccess"), DialogStyle.Ok, DialogButtons.None, true);
							}
							else
							{
								Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtFailed"), DialogStyle.Error, DialogButtons.None, true);
							}
						}
						bool flag58 = e.Path == Definition.resetDtOffsetCorrectionPath && e.NewValue == "1";
						if (flag58)
						{
							bool flag59 = this.dbConnection.DeleteDtCorrection(false, true, true);
							if (flag59)
							{
								Dialog.Show(LocalizationManager.Localize("txtSuccess"), LocalizationManager.Localize("txtSuccess"), DialogStyle.Ok, DialogButtons.None, true);
							}
							else
							{
								Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtFailed"), DialogStyle.Error, DialogButtons.None, true);
							}
						}
						bool flag60 = e.Path == Definition.useAuditCheckLampPath;
						if (flag60)
						{
							this.dbConnection.SetUseAuditCheckLamp(e.NewValue == "1");
						}
						bool flag61 = e.Path == Definition.deviceAddressPath;
						if (flag61)
						{
							int num7 = 0;
							bool flag62 = int.TryParse(e.NewValue, NumberStyles.HexNumber, new CultureInfo("de-DE"), out num7) && num7 > 0 && num7 < 256;
							if (flag62)
							{
								this.dbConnection.SetDeviceAddress(e.NewValue);
							}
							else
							{
								this.parHandling.WriteValue(e.Path, e.OldValue, false);
							}
						}
						bool flag63 = e.Path == Definition.minBackgroundValuePath;
						if (flag63)
						{
							int minBackgroundValue = 0;
							bool flag64 = int.TryParse(e.NewValue, out minBackgroundValue);
							if (flag64)
							{
								this.dbConnection.SetMinBackgroundValue(minBackgroundValue);
							}
						}
						bool flag65 = e.Path == Definition.maxBackgroundValuePath;
						if (flag65)
						{
							int maxBackgroundValue = 0;
							bool flag66 = int.TryParse(e.NewValue, out maxBackgroundValue);
							if (flag66)
							{
								this.dbConnection.SetMaxBackgroundValue(maxBackgroundValue);
							}
						}
						bool flag67 = e.Path.StartsWith(Definition.SVTValvesAssignmentMainPath) && !e.Path.EndsWith("/Trigger/Value");
						if (flag67)
						{
							int valveIndex = -1;
							int value2 = -1;
							bool flag68 = e.Path.Contains("MinPixel");
							bool flag69 = flag68;
							string text10;
							if (flag69)
							{
								text10 = e.Path.Substring(0, e.Path.IndexOf("/MinPixel"));
							}
							else
							{
								text10 = e.Path.Substring(0, e.Path.IndexOf("/MaxPixel"));
							}
							bool flag70 = text10.Length >= 2;
							if (flag70)
							{
								text10 = text10.Remove(0, text10.Length - 2);
							}
							bool flag71 = int.TryParse(text10, out valveIndex) && int.TryParse(e.NewValue, out value2);
							if (flag71)
							{
								bool flag72 = flag68;
								if (flag72)
								{
									this.dbConnection.SetSVTMinPixel(valveIndex, value2);
								}
								else
								{
									this.dbConnection.SetSVTMaxPixel(valveIndex, value2);
								}
							}
						}
						bool flag73 = e.Path == "/testSuite/Active/Value";
						if (flag73)
						{
							bool flag74 = this.xrayState != null;
							if (flag74)
							{
								bool flag75 = e.NewValue == "1";
								if (flag75)
								{
									this.xrayState.TestSuiteActive = true;
								}
								else
								{
									bool flag76 = e.NewValue == "0";
									if (flag76)
									{
										this.xrayState.TestSuiteActive = false;
									}
								}
							}
						}
						bool flag77 = e.Path == Autotrain.nAutolearnPath;
						if (flag77)
						{
							int num8 = 0;
							bool flag78 = int.TryParse(e.NewValue, out num8) && num8 > 0;
							if (flag78)
							{
								this.dbConnection.SetNAutolearn(num8);
							}
						}
						bool flag79 = e.Path == Autotrain.nAutolearnDefaultPath;
						if (flag79)
						{
							int num9 = 0;
							bool flag80 = int.TryParse(e.NewValue, out num9) && num9 > 0;
							if (flag80)
							{
								this.dbConnection.SetNAutolearnDefault(num9);
							}
						}
						bool flag81 = e.Path == Autotrain.nAutolearnDefaultBulkPath;
						if (flag81)
						{
							int num10 = 0;
							bool flag82 = int.TryParse(e.NewValue, out num10) && num10 > 0;
							if (flag82)
							{
								this.dbConnection.SetNAutolearnDefaultBulk(num10);
							}
						}
						bool flag83 = e.Path == Definition.multiProductsEnablePath;
						if (flag83)
						{
							this.dbConnection.SetMultiProductsEnabled(e.NewValue == "1");
						}
						bool flag84 = e.Path == Definition.nSaveImagesGoodPath;
						if (flag84)
						{
							int num11 = 0;
							bool flag85 = int.TryParse(e.NewValue, out num11) && num11 >= 0;
							if (flag85)
							{
								this.dbConnection.SetNSaveImagesGood(num11);
							}
						}
						bool flag86 = e.Path == Definition.nSaveImagesBadPath;
						if (flag86)
						{
							int num12 = 0;
							bool flag87 = int.TryParse(e.NewValue, out num12) && num12 >= 0;
							if (flag87)
							{
								this.dbConnection.SetNSaveImagesBad(num12);
							}
						}
						bool flag88 = e.Path == Definition.sshMotorCurrentLimitPath;
						if (flag88)
						{
							int num13 = 0;
							bool flag89 = int.TryParse(e.NewValue, out num13) && num13 >= 0;
							if (flag89)
							{
								this.dbConnection.SetSshMotorCurrentLimit(num13);
							}
						}
						bool flag90 = e.Path == Definition.bulkCleaningVibratorPath;
						if (flag90)
						{
							bool flag91 = e.NewValue == "1";
							if (flag91)
							{
								this.dbConnection.SetBulkCleaningVibrator(true);
							}
							else
							{
								bool flag92 = e.NewValue == "0";
								if (flag92)
								{
									this.dbConnection.SetBulkCleaningVibrator(false);
								}
							}
						}
						bool flag93 = e.Path == Definition.imageRecvTimeoutPath;
						if (flag93)
						{
							int imageServerTimeout = 0;
							bool flag94 = int.TryParse(e.NewValue, out imageServerTimeout);
							if (flag94)
							{
								this.dbConnection.SetImageServerTimeout(imageServerTimeout);
							}
						}
						bool flag95 = e.Path == Definition.conveyorStopDelayTimePath;
						if (flag95)
						{
							int conveyorStopDelayTime = 0;
							bool flag96 = int.TryParse(e.NewValue, out conveyorStopDelayTime);
							if (flag96)
							{
								this.dbConnection.SetConveyorStopDelayTime(conveyorStopDelayTime);
							}
						}
						bool flag97 = e.Path == Definition.nCriticalArcsPath;
						if (flag97)
						{
							int ncriticalArcs = 0;
							bool flag98 = int.TryParse(e.NewValue, out ncriticalArcs);
							if (flag98)
							{
								this.dbConnection.SetNCriticalArcs(ncriticalArcs);
							}
						}
						bool flag99 = e.Path == Definition.criticalArcsPeriodPath;
						if (flag99)
						{
							int criticalArcsPeriod = 0;
							bool flag100 = int.TryParse(e.NewValue, out criticalArcsPeriod);
							if (flag100)
							{
								this.dbConnection.SetCriticalArcsPeriod(criticalArcsPeriod);
							}
						}
						bool flag101 = e.Path == Definition.multiLaneViewPath;
						if (flag101)
						{
							bool flag102 = e.NewValue == "1";
							if (flag102)
							{
								this.dbConnection.SetMultiLaneView(true);
							}
							else
							{
								bool flag103 = e.NewValue == "0";
								if (flag103)
								{
									this.dbConnection.SetMultiLaneView(false);
								}
							}
						}
						bool flag104 = e.Path == Definition.msBinTimeout1Path;
						if (flag104)
						{
							int timeout = 0;
							bool flag105 = int.TryParse(e.NewValue, out timeout);
							if (flag105)
							{
								this.dbConnection.SetMSBinTimeout(true, timeout);
							}
						}
						bool flag106 = e.Path == Definition.msBinTimeout2Path;
						if (flag106)
						{
							int timeout2 = 0;
							bool flag107 = int.TryParse(e.NewValue, out timeout2);
							if (flag107)
							{
								this.dbConnection.SetMSBinTimeout(false, timeout2);
							}
						}
						bool flag108 = e.Path == Definition.msSafetyTimeRejectCheck1Path;
						if (flag108)
						{
							int num14 = 0;
							bool flag109 = int.TryParse(e.NewValue, out num14);
							if (flag109)
							{
								this.dbConnection.SetMSSafetyTime(true, num14);
							}
						}
						bool flag110 = e.Path == Definition.msSafetyTimeRejectCheck2Path;
						if (flag110)
						{
							int num15 = 0;
							bool flag111 = int.TryParse(e.NewValue, out num15);
							if (flag111)
							{
								this.dbConnection.SetMSSafetyTime(false, num15);
							}
						}
						bool flag112 = e.Path == Definition.multiProductViewPath;
						if (flag112)
						{
							bool flag113 = e.NewValue == "1";
							if (flag113)
							{
								this.dbConnection.SetMultiProductView(true);
							}
							else
							{
								bool flag114 = e.NewValue == "0";
								if (flag114)
								{
									this.dbConnection.SetMultiProductView(false);
								}
							}
						}
						bool flag115 = e.Path == Acquisition.lane1EnablePath || e.Path == Acquisition.lane2EnablePath || e.Path == Acquisition.lane3EnablePath || e.Path == Acquisition.lane4EnablePath || e.Path == Definition.multiLaneViewPath || e.Path == Definition.multiProductViewPath;
						if (flag115)
						{
							Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
							{
								this.SetActiveLanes();
							}));
						}
						bool flag116 = e.Path == Acquisition.lane1EnablePath;
						if (flag116)
						{
							bool flag117 = e.NewValue == "1";
							if (flag117)
							{
								this.productManager.SetLaneActiveOfLaneProducts(Lane.Lane1, true);
							}
							else
							{
								bool flag118 = e.NewValue == "0";
								if (flag118)
								{
									this.productManager.SetLaneActiveOfLaneProducts(Lane.Lane1, false);
								}
							}
						}
						bool flag119 = e.Path == Acquisition.lane2EnablePath;
						if (flag119)
						{
							bool flag120 = e.NewValue == "1";
							if (flag120)
							{
								this.productManager.SetLaneActiveOfLaneProducts(Lane.Lane2, true);
							}
							else
							{
								bool flag121 = e.NewValue == "0";
								if (flag121)
								{
									this.productManager.SetLaneActiveOfLaneProducts(Lane.Lane2, false);
								}
							}
						}
						bool flag122 = e.Path == Acquisition.lane3EnablePath;
						if (flag122)
						{
							bool flag123 = e.NewValue == "1";
							if (flag123)
							{
								this.productManager.SetLaneActiveOfLaneProducts(Lane.Lane3, true);
							}
							else
							{
								bool flag124 = e.NewValue == "0";
								if (flag124)
								{
									this.productManager.SetLaneActiveOfLaneProducts(Lane.Lane3, false);
								}
							}
						}
						bool flag125 = e.Path == Acquisition.lane4EnablePath;
						if (flag125)
						{
							bool flag126 = e.NewValue == "1";
							if (flag126)
							{
								this.productManager.SetLaneActiveOfLaneProducts(Lane.Lane4, true);
							}
							else
							{
								bool flag127 = e.NewValue == "0";
								if (flag127)
								{
									this.productManager.SetLaneActiveOfLaneProducts(Lane.Lane4, false);
								}
							}
						}
						bool flag128 = e.Path == Definition.doorLockAvailablePath;
						if (flag128)
						{
							bool flag129 = e.NewValue == "1";
							if (flag129)
							{
								this.dbConnection.SetDoorLockAvailable(true);
								this.DoorLockAvailable = true;
							}
							else
							{
								bool flag130 = e.NewValue == "0";
								if (flag130)
								{
									this.dbConnection.SetDoorLockAvailable(false);
									this.DoorLockAvailable = false;
								}
							}
						}
						bool flag131 = e.Path == Definition.highLevelSensorsPath;
						if (flag131)
						{
							bool flag132 = e.NewValue == "1";
							if (flag132)
							{
								this.dbConnection.SetHighLevelSensors(true);
								this.UpdateHighLevelSensorPaths(true);
							}
							else
							{
								bool flag133 = e.NewValue == "0";
								if (flag133)
								{
									this.dbConnection.SetHighLevelSensors(false);
									this.UpdateHighLevelSensorPaths(false);
								}
							}
						}
						bool flag134 = e.Path == Definition.multiLaneProductsPath;
						if (flag134)
						{
							bool flag135 = e.NewValue == "1";
							if (flag135)
							{
								this.dbConnection.SetMultiLaneProductsEnabled(true);
							}
							else
							{
								bool flag136 = e.NewValue == "0";
								if (flag136)
								{
									this.dbConnection.SetMultiLaneProductsEnabled(false);
								}
							}
						}
						bool flag137 = e.Path == Definition.maxUsersPath;
						if (flag137)
						{
							int num16 = 0;
							bool flag138 = int.TryParse(e.NewValue, out num16) && num16 > 0;
							if (flag138)
							{
								this.dbConnection.SetMaxUsers(num16);
							}
						}
						bool flag139 = e.Path.Contains("/global/Algorithm/Value");
						if (flag139)
						{
							string text11 = e.Path.Remove(0, Definition.filterMainPath.Length);
							int num17 = text11.IndexOf('/');
							text11 = text11.Remove(num17, text11.Length - num17);
							List<string> list5 = new List<string>();
							int num;
							for (int n = 0; n < Definition.nObjectFinder; n = num + 1)
							{
								list5.Add(string.Concat(new string[]
								{
									Definition.filterMainPath,
									text11,
									Definition.objectFindersSubPath,
									(n + 1).ToString(),
									"/",
									Definition.strAlgorithm,
									"/",
									Definition.strValue
								}));
								num = n;
							}
							this.UpdatePaths(list5.ToArray(), text11);
						}
						bool flag140 = this.productManager != null && this.productManager.CurrentLoadedProduct.IsSingleProduct;
						if (flag140)
						{
							this.productManager.UpdateAssignedSubLaneProducts(e.Path, e.NewValue);
						}
						bool flag141 = this.productManager != null && this.productManager.CurrentLoadedProduct.IsLaneProduct;
						if (flag141)
						{
							this.productManager.UpdateAssignedMainProduct(e.Path, e.NewValue);
						}
						bool flag142 = e.Path == Definition.sshPxTolerancePath;
						if (flag142)
						{
							int num18 = 0;
							bool flag143 = int.TryParse(e.NewValue, out num18) && num18 >= 0;
							if (flag143)
							{
								this.dbConnection.SetSshPixelTolerance(num18);
							}
						}
						bool flag144 = e.Path == Definition.buzzerAvailablePath;
						if (flag144)
						{
							bool flag145 = e.NewValue == "1";
							if (flag145)
							{
								this.dbConnection.SetBuzzerAvailable(true);
							}
							else
							{
								bool flag146 = e.NewValue == "0";
								if (flag146)
								{
									this.dbConnection.SetBuzzerAvailable(false);
								}
							}
						}
						bool flag147 = e.Path == Definition.detectorTypePath;
						if (flag147)
						{
							try
							{
								DetectorType detectorType2 = (DetectorType)Enum.Parse(typeof(DetectorType), e.NewValue);
								this.dbConnection.SetDetectorType(detectorType2);
								this.conveyor.AdaptScanrate(false, "");
							}
							catch
							{
							}
						}
						bool flag148 = e.Path == Definition.multiRegionPath;
						if (flag148)
						{
							bool flag149 = e.NewValue == "1";
							if (flag149)
							{
								this.dbConnection.SetMultiRegionEnabled(true);
							}
							else
							{
								bool flag150 = e.NewValue == "0";
								if (flag150)
								{
									this.dbConnection.SetMultiRegionEnabled(false);
								}
							}
						}
						bool flag151 = e.Path == Definition.bulkObjectProductsPath;
						if (flag151)
						{
							bool flag152 = e.NewValue == "1";
							if (flag152)
							{
								this.dbConnection.SetBulkObjectEnabled(true);
							}
							else
							{
								bool flag153 = e.NewValue == "0";
								if (flag153)
								{
									this.dbConnection.SetBulkObjectEnabled(false);
								}
							}
						}
						bool flag154 = e.Path == Definition.autoZoomPath;
						if (flag154)
						{
							bool flag155 = e.NewValue == "1";
							if (flag155)
							{
								this.dbConnection.SetAutoZoom(true);
								XRSImageBoxViewModel.autoZoomEnabled = true;
							}
							else
							{
								bool flag156 = e.NewValue == "0";
								if (flag156)
								{
									this.dbConnection.SetAutoZoom(false);
									XRSImageBoxViewModel.autoZoomEnabled = false;
								}
							}
						}
						bool flag157 = e.Path == Definition.noBeltStartOnErrorPath;
						if (flag157)
						{
							bool flag158 = e.NewValue == "1";
							if (flag158)
							{
								this.dbConnection.SetNoBeltStartOnError(true);
							}
							else
							{
								bool flag159 = e.NewValue == "0";
								if (flag159)
								{
									this.dbConnection.SetNoBeltStartOnError(false);
								}
							}
						}
						bool flag160 = e.Path == Definition.beltStopOnErrorEnablePath;
						if (flag160)
						{
							bool flag161 = e.NewValue == "1";
							if (flag161)
							{
								this.dbConnection.SetStopConveyorIfError(true);
							}
							else
							{
								bool flag162 = e.NewValue == "0";
								if (flag162)
								{
									this.dbConnection.SetStopConveyorIfError(false);
								}
							}
						}
						bool flag163 = e.Path == Definition.logImageIndexPath;
						if (flag163)
						{
							bool flag164 = e.NewValue == "1";
							if (flag164)
							{
								this.dbConnection.SetLogImageIndex(true);
							}
							else
							{
								bool flag165 = e.NewValue == "0";
								if (flag165)
								{
									this.dbConnection.SetLogImageIndex(false);
								}
							}
						}
						bool flag166 = e.Path == Definition.dtScanrateLimitEnablePath;
						if (flag166)
						{
							bool flag167 = e.NewValue == "1";
							if (flag167)
							{
								this.dbConnection.SetDTScanrateLimitEnable(true);
							}
							else
							{
								bool flag168 = e.NewValue == "0";
								if (flag168)
								{
									this.dbConnection.SetDTScanrateLimitEnable(false);
								}
							}
						}
						bool flag169 = e.Path == Definition.rejecterNRejectsLimitPath;
						if (flag169)
						{
							int num19 = 0;
							bool flag170 = int.TryParse(e.NewValue, out num19) && num19 >= 0;
							if (flag170)
							{
								this.dbConnection.SetNRejectsLimit(num19);
							}
						}
						bool flag171 = e.Path == Definition.rejecterRejectPeriodLimitPath;
						if (flag171)
						{
							int num20 = 0;
							bool flag172 = int.TryParse(e.NewValue, out num20) && num20 >= 0;
							if (flag172)
							{
								this.dbConnection.SetRejectPeriodLimit(num20);
							}
						}
						bool flag173 = e.Path == Definition.conveyorInManualModePath;
						if (flag173)
						{
							bool flag174 = e.NewValue == "1";
							if (flag174)
							{
								this.dbConnection.SetBeltStartIfSwitchOpen(true);
							}
							else
							{
								bool flag175 = e.NewValue == "0";
								if (flag175)
								{
									this.dbConnection.SetBeltStartIfSwitchOpen(false);
								}
							}
						}
						bool flag176 = e.Path == Definition.insightNetAvailablePath;
						if (flag176)
						{
							bool flag177 = e.NewValue == "1";
							if (flag177)
							{
								this.dbConnection.SetInsightNetAvailable(true);
							}
							else
							{
								bool flag178 = e.NewValue == "0";
								if (flag178)
								{
									this.dbConnection.SetInsightNetAvailable(false);
								}
							}
						}
						bool flag179 = e.Path == Definition.sstReceiveTimeoutPath;
						if (flag179)
						{
							int num21 = 0;
							bool flag180 = int.TryParse(e.NewValue, out num21) && num21 >= 0;
							if (flag180)
							{
								this.dbConnection.SetSSTRecvTimeout(num21);
								bool flag181 = this.sstServer != null;
								if (flag181)
								{
									this.sstServer.ReceiveTimeout = num21;
								}
							}
						}
						bool flag182 = e.Path == Definition.sstMaxConnectionsPath;
						if (flag182)
						{
							int num22 = 0;
							bool flag183 = int.TryParse(e.NewValue, out num22) && num22 > 0;
							if (flag183)
							{
								this.dbConnection.SetSSTMaxConnections(num22);
								bool flag184 = this.sstServer != null;
								if (flag184)
								{
									this.sstServer.MaxConnections = num22;
								}
							}
						}
						bool flag185 = e.Path == Definition.op2HlExtendedRightsPath;
						if (flag185)
						{
							bool flag186 = e.NewValue == "1";
							if (flag186)
							{
								this.dbConnection.SetOp2HlExtendedRights(true);
							}
							else
							{
								bool flag187 = e.NewValue == "0";
								if (flag187)
								{
									this.dbConnection.SetOp2HlExtendedRights(false);
								}
							}
						}
						bool flag188 = e.Path == Definition.saveUncommittedErrorsPath;
						if (flag188)
						{
							bool flag189 = e.NewValue == "1";
							if (flag189)
							{
								this.dbConnection.SetSaveUncommittedErrors(true);
							}
							else
							{
								bool flag190 = e.NewValue == "0";
								if (flag190)
								{
									this.dbConnection.SetSaveUncommittedErrors(false);
								}
							}
						}
						bool flag191 = e.Path == Definition.saveEdgePixelInPNGPath;
						if (flag191)
						{
							bool flag192 = e.NewValue == "1";
							if (flag192)
							{
								this.dbConnection.SetSaveEdgePixelInPNG(true);
							}
							else
							{
								bool flag193 = e.NewValue == "0";
								if (flag193)
								{
									this.dbConnection.SetSaveEdgePixelInPNG(false);
								}
							}
						}
						bool flag194 = e.Path == Definition.maxBatchesPath;
						if (flag194)
						{
							int num23 = 0;
							bool flag195 = int.TryParse(e.NewValue, out num23) && num23 > 0;
							if (flag195)
							{
								this.dbConnection.SetMaxBatches(num23);
							}
						}
						bool flag196 = e.Path == Definition.ipDetectorSesotecPath;
						if (flag196)
						{
							IPAddress ipaddress = null;
							bool flag197 = IPAddress.TryParse(e.NewValue, out ipaddress) && ipaddress != null;
							if (flag197)
							{
								this.dbConnection.SetIPDetectorSesotec(ipaddress);
							}
						}
						bool flag198 = e.Path == Definition.ipDetectorDtPath;
						if (flag198)
						{
							IPAddress ipaddress2 = null;
							bool flag199 = IPAddress.TryParse(e.NewValue, out ipaddress2) && ipaddress2 != null;
							if (flag199)
							{
								this.dbConnection.SetIPDetectorDT(ipaddress2);
							}
						}
						bool flag200 = e.Path == Definition.batchesEnabledPath;
						if (flag200)
						{
							bool flag201 = e.NewValue == "1";
							if (flag201)
							{
								this.dbConnection.SetBatchesEnabled(true);
								bool flag202 = this.productManager != null;
								if (flag202)
								{
									Thread thread = new Thread(delegate()
									{
										Thread.Sleep(100);
										Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(delegate()
										{
											this.ShowBatchWindowOrStartNew(null, this.productManager.CurrentLoadedProduct);
										}));
									});
									thread.Start();
								}
							}
							else
							{
								bool flag203 = e.NewValue == "0";
								if (flag203)
								{
									string currentBatchName = this.productManager.CurrentLoadedProduct.CurrentBatchName;
									bool flag204 = currentBatchName != null && currentBatchName.Length > 0;
									if (flag204)
									{
										this.dbConnection.EndBatch(currentBatchName, this.productManager.CurrentLoadedProduct.TableName);
										this.dbConnection.SetCurrentBatchID("", this.productManager.CurrentLoadedProduct.TableName);
									}
									this.dbConnection.SetBatchesEnabled(false);
									this.productManager.UpdateCurrentBatchNames();
								}
							}
						}
						bool flag205 = e.Path == Definition.statisticsEnabledPath;
						if (flag205)
						{
							bool flag206 = e.NewValue == "1";
							if (flag206)
							{
								this.dbConnection.SetStatisticsEnabled(true);
							}
							else
							{
								bool flag207 = e.NewValue == "0";
								if (flag207)
								{
									this.dbConnection.SetStatisticsEnabled(false);
								}
							}
						}
						bool flag208 = e.Path == Definition.maxStatisticsPath;
						if (flag208)
						{
							int num24 = 0;
							bool flag209 = int.TryParse(e.NewValue, out num24) && num24 > 0;
							if (flag209)
							{
								this.dbConnection.SetMaxStatistics(num24);
							}
						}
						bool flag210 = e.Path == Definition.sshMaxMoveTimeXRayMotorPath;
						if (flag210)
						{
							int num25 = 0;
							bool flag211 = int.TryParse(e.NewValue, out num25) && num25 > 0;
							if (flag211)
							{
								this.dbConnection.SetSshMaxMoveTimeXRayMotor(num25);
							}
						}
						bool flag212 = e.Path == Definition.ukUserRightsHLPath;
						if (flag212)
						{
							bool flag213 = e.NewValue == "1";
							if (flag213)
							{
								this.dbConnection.SetUKUserRightsHL(true);
							}
							else
							{
								bool flag214 = e.NewValue == "0";
								if (flag214)
								{
									this.dbConnection.SetUKUserRightsHL(false);
								}
							}
						}
						bool flag215 = e.Path == Definition.fcVoltageFactorPath;
						if (flag215)
						{
							double fcvoltageFactor = 0.0;
							string s = e.NewValue.Replace(',', '.');
							bool flag216 = double.TryParse(s, NumberStyles.AllowDecimalPoint, new CultureInfo("en-GB"), out fcvoltageFactor);
							if (flag216)
							{
								this.dbConnection.SetFCVoltageFactor(fcvoltageFactor);
								bool flag217 = this.conveyor != null;
								if (flag217)
								{
									this.conveyor.UpdateFCVoltage(-1.0);
								}
							}
						}
						bool flag218 = e.Path == Definition.xrayMotorThresholdPath;
						if (flag218)
						{
							int xrayMotorThreshold = 0;
							bool flag219 = int.TryParse(e.NewValue, out xrayMotorThreshold);
							if (flag219)
							{
								this.dbConnection.SetXRayMotorThreshold(xrayMotorThreshold);
							}
						}
						bool flag220 = e.Path == Definition.machineTypePath;
						if (flag220)
						{
							try
							{
								int num26 = 0;
								bool flag221 = int.TryParse(e.NewValue, out num26) && num26 >= 0 && num26 < Enum.GetNames(typeof(MachineType)).Length;
								if (flag221)
								{
									this.dbConnection.SetMachineType((MachineType)num26);
									this.Machine.MachineType = (MachineType)num26;
								}
							}
							catch
							{
							}
						}
						bool flag222 = e.Path == Definition.advancedFiltersPath;
						if (flag222)
						{
							bool flag223 = e.NewValue == "1";
							if (flag223)
							{
								this.dbConnection.SetAdvancedFilters(true);
							}
							else
							{
								bool flag224 = e.NewValue == "0";
								if (flag224)
								{
									this.dbConnection.SetAdvancedFilters(false);
								}
							}
						}
						bool flag225 = e.Path == Definition.inOutletBlockedTimePath;
						if (flag225)
						{
							int inOutBlockedTime = 0;
							bool flag226 = int.TryParse(e.NewValue, out inOutBlockedTime);
							if (flag226)
							{
								this.dbConnection.SetInOutBlockedTime(inOutBlockedTime);
							}
						}
						MachineType machineType = MachineType.RayconD;
						this.dbConnection.GetMachineType(ref machineType);
						bool isDPlus1Module = this.parHandling.ReadPar(Definition.wagoMachineTypePath) == Definition.ioModuleIsRayconDPlus1Module || this.parHandling.ReadPar(Definition.wagoMachineTypePath) == Definition.ioModuleIsRayconDPlus1ModuleHYRework;
						bool flag227 = e.Path == Definition.GetTestAirPressurePath(machineType, isDPlus1Module);
						if (flag227)
						{
							bool flag228 = e.OldValue == "1" && e.NewValue == "0" && this.errorSurvey != null;
							if (flag228)
							{
								this.errorSurvey.DelayAirPressureChecking();
							}
						}
						bool flag229 = e.Path == Definition.maxLogbookEntriesPath;
						if (flag229)
						{
							int num27 = 0;
							bool flag230 = int.TryParse(e.NewValue, out num27) && num27 > 0;
							if (flag230)
							{
								this.dbConnection.SetMaxLogbookEntries(num27);
							}
						}
						bool flag231 = e.Path == Definition.maxProductsPath;
						if (flag231)
						{
							int num28 = 0;
							bool flag232 = int.TryParse(e.NewValue, out num28) && num28 > 0 && num28 < 1000;
							if (flag232)
							{
								this.dbConnection.SetMaxProducts(num28);
							}
						}
						bool flag233 = e.Path == Definition.startXRayNoUserPath;
						if (flag233)
						{
							bool flag234 = e.NewValue == "1";
							if (flag234)
							{
								this.dbConnection.SetStartXRayNoUser(true);
								this.StartXRayNoUser = true;
							}
							else
							{
								bool flag235 = e.NewValue == "0";
								if (flag235)
								{
									this.dbConnection.SetStartXRayNoUser(false);
									this.StartXRayNoUser = false;
								}
							}
						}
						bool flag236 = e.Path == Definition.rfidLoginAvailablePath;
						if (flag236)
						{
							bool flag237 = e.NewValue == "1";
							if (flag237)
							{
								this.dbConnection.SetRFIDLoginAvailable(true);
							}
							else
							{
								bool flag238 = e.NewValue == "0";
								if (flag238)
								{
									this.dbConnection.SetRFIDLoginAvailable(false);
								}
							}
						}
						bool flag239 = e.Path == Definition.rfidLoginComPortPath;
						if (flag239)
						{
							uint num29 = 0U;
							bool flag240 = uint.TryParse(e.NewValue, out num29) && num29 > 0U && num29 < 100U;
							if (flag240)
							{
								this.dbConnection.SetRFIDLoginComPort(num29);
							}
						}
						bool flag241 = e.Path == Definition.autoKVAdjustmentPath;
						if (flag241)
						{
							uint num30 = 0U;
							bool flag242 = uint.TryParse(e.NewValue, out num30) && num30 >= 0U && num30 < 4U;
							if (flag242)
							{
								this.dbConnection.SetAutoKVAdjustment(num30);
							}
						}
						bool flag243 = e.Path == Definition.guiInactiveTimePath;
						if (flag243)
						{
							int num31 = 0;
							bool flag244 = int.TryParse(e.NewValue, out num31) && num31 > 0;
							if (flag244)
							{
								this.dbConnection.SetGUIInactiveTime(num31);
								MainWindowViewModel.accessRightsManager.SetTimeInactiveGUI(num31);
							}
						}
						bool flag245 = e.Path == Definition.rejectVerifyLowActive1Path;
						if (flag245)
						{
							bool flag246 = e.NewValue == "1";
							if (flag246)
							{
								this.dbConnection.SetRejectVerifyLowActive(1, true);
							}
							else
							{
								bool flag247 = e.NewValue == "0";
								if (flag247)
								{
									this.dbConnection.SetRejectVerifyLowActive(1, false);
								}
							}
						}
						bool flag248 = e.Path == Definition.rejectVerifyLowActive2Path;
						if (flag248)
						{
							bool flag249 = e.NewValue == "1";
							if (flag249)
							{
								this.dbConnection.SetRejectVerifyLowActive(2, true);
							}
							else
							{
								bool flag250 = e.NewValue == "0";
								if (flag250)
								{
									this.dbConnection.SetRejectVerifyLowActive(2, false);
								}
							}
						}
						bool flag251 = e.Path == Definition.rejectVerifyLowActive3Path;
						if (flag251)
						{
							bool flag252 = e.NewValue == "1";
							if (flag252)
							{
								this.dbConnection.SetRejectVerifyLowActive(3, true);
							}
							else
							{
								bool flag253 = e.NewValue == "0";
								if (flag253)
								{
									this.dbConnection.SetRejectVerifyLowActive(3, false);
								}
							}
						}
						bool flag254 = e.Path == Definition.rejectVerifyLowActive4Path;
						if (flag254)
						{
							bool flag255 = e.NewValue == "1";
							if (flag255)
							{
								this.dbConnection.SetRejectVerifyLowActive(4, true);
							}
							else
							{
								bool flag256 = e.NewValue == "0";
								if (flag256)
								{
									this.dbConnection.SetRejectVerifyLowActive(4, false);
								}
							}
						}
						bool flag257 = e.Path == Definition.limitXraySourceOperatingHoursPath;
						if (flag257)
						{
							int num32 = 0;
							bool flag258 = int.TryParse(e.NewValue, out num32) && num32 > 0;
							if (flag258)
							{
								this.dbConnection.SetXRayOpHoursLimit(num32);
							}
						}
						bool flag259 = e.Path == Definition.dynamiclimitXraySourceOperatingHoursPath;
						if (flag259)
						{
							int num33 = 0;
							bool flag260 = int.TryParse(e.NewValue, out num33) && num33 > 0;
							if (flag260)
							{
								this.dbConnection.SetDynXRayOpHoursLimit(num33);
							}
						}
						bool flag261 = e.Path == Definition.xRayOperatingHoursSesotecPath;
						if (flag261)
						{
							int num34 = 0;
							bool flag262 = int.TryParse(e.NewValue, out num34) && num34 > 0;
							if (flag262)
							{
								this.dbConnection.SetXRayOperatingHoursSesotec((double)num34);
							}
						}
						bool flag263 = e.Path == Definition.dynlimitXraySourceOperatingHoursFactPath;
						if (flag263)
						{
							double dynXRayOpHoursFactor = 0.0;
							string s2 = e.NewValue.Replace(',', '.');
							bool flag264 = double.TryParse(s2, NumberStyles.AllowDecimalPoint, new CultureInfo("en-GB"), out dynXRayOpHoursFactor);
							if (flag264)
							{
								this.dbConnection.SetDynXRayOpHoursFactor(dynXRayOpHoursFactor);
							}
						}
						bool flag265 = e.Path == Definition.conveyingSpeedMaxDCPath;
						if (flag265)
						{
							double conveyingSpeedMaxDC = 0.0;
							string s3 = e.NewValue.Replace(',', '.');
							bool flag266 = double.TryParse(s3, NumberStyles.AllowDecimalPoint, new CultureInfo("en-GB"), out conveyingSpeedMaxDC);
							if (flag266)
							{
								this.dbConnection.SetConveyingSpeedMaxDC(conveyingSpeedMaxDC);
							}
						}
						bool flag267 = e.Path == Definition.inspectionWidthPath;
						if (flag267)
						{
							ushort num35 = 0;
							bool flag268 = ushort.TryParse(e.NewValue, out num35) && num35 >= 0;
							if (flag268)
							{
								this.dbConnection.SetInspectionWidth(num35);
							}
						}
						bool flag269 = e.Path == Definition.inspectionHeightPath;
						if (flag269)
						{
							ushort num36 = 0;
							bool flag270 = ushort.TryParse(e.NewValue, out num36) && num36 >= 0;
							if (flag270)
							{
								this.dbConnection.SetInspectionHeight(num36);
							}
						}
						bool flag271 = e.Path == Definition.machineSubTypePath;
						if (flag271)
						{
							this.dbConnection.SetMachineSubType(e.NewValue);
						}
						bool flag272 = e.Path == Definition.machineNamePath;
						if (flag272)
						{
							this.dbConnection.SetDeviceName(e.NewValue);
						}
						bool flag273 = e.Path == Definition.lineNamePath;
						if (flag273)
						{
							this.dbConnection.SetLineName(e.NewValue);
						}
						bool flag274 = e.Path == Definition.machineSerialNumberPath;
						if (flag274)
						{
							this.dbConnection.SetSerialNumber(e.NewValue);
						}
						bool flag275 = e.Path == Definition.machinelocationPath;
						if (flag275)
						{
							this.dbConnection.SetLocation(e.NewValue);
						}
						bool flag276 = e.Path == Definition.testcardSimulationPath;
						if (flag276)
						{
							bool flag277 = e.NewValue == "0";
							if (flag277)
							{
								this.dbConnection.SetTestcardSimulation(false);
							}
							else
							{
								bool flag278 = e.NewValue == "1";
								if (flag278)
								{
									this.dbConnection.SetTestcardSimulation(true);
								}
							}
						}
						bool flag279 = e.Path == Definition.shownImgGetHigherContPointXPath;
						if (flag279)
						{
							int shownImgGetHigherContPointX = 0;
							bool flag280 = int.TryParse(e.NewValue, out shownImgGetHigherContPointX);
							if (flag280)
							{
								this.dbConnection.SetShownImgGetHigherContPointX(shownImgGetHigherContPointX);
								XRSImageBox.ShownImgGetHigherContPointX = shownImgGetHigherContPointX;
							}
						}
						bool flag281 = e.Path == Definition.shownImgGetHigherContPointYPath;
						if (flag281)
						{
							int shownImgGetHigherContPointY = 0;
							bool flag282 = int.TryParse(e.NewValue, out shownImgGetHigherContPointY);
							if (flag282)
							{
								this.dbConnection.SetShownImgGetHigherContPointY(shownImgGetHigherContPointY);
								XRSImageBox.ShownImgGetHigherContPointY = shownImgGetHigherContPointY;
							}
						}
						bool flag283 = e.Path == Definition.shownImgGetHigherContAveragePath;
						if (flag283)
						{
							int num37 = 0;
							bool flag284 = int.TryParse(e.NewValue, out num37);
							if (flag284)
							{
								this.dbConnection.SetShownImgGetHigherContAverage(num37);
								XRSImageBox.ShownImgGetHigherContAverage = Convert.ToInt32(num37);
							}
						}
						bool flag285 = e.Path == Definition.backroundNoisePath;
						if (flag285)
						{
							XRSImageBox.ShownImgGetHigherContBackgroundVal = Convert.ToInt32(e.NewValue);
						}
						bool flag286 = e.Path == Definition.isMXFeatures04detector;
						if (flag286)
						{
							bool flag287 = e.NewValue == "0";
							if (flag287)
							{
								this.dbConnection.SetMXFeatures04Det(false);
							}
							else
							{
								bool flag288 = e.NewValue == "1";
								if (flag288)
								{
									this.dbConnection.SetMXFeatures04Det(true);
								}
							}
						}
						bool flag289 = e.Path == Definition.isDBDatalogsActive;
						if (flag289)
						{
							bool flag290 = e.NewValue == "0";
							if (flag290)
							{
								this.dbConnection.SetDBDatalogsIsActive(false);
								bool flag291 = this.dbDatalogsManager != null;
								if (flag291)
								{
									this.dbDatalogsManager.StopTimer();
								}
							}
							else
							{
								bool flag292 = e.NewValue == "1";
								if (flag292)
								{
									this.dbConnection.SetDBDatalogsIsActive(true);
									bool flag293 = !File.Exists(DBDataLogs.databasePath) && this.dbDatalogs != null;
									if (flag293)
									{
										this.dbDatalogs.CreateDatabase(DBDataLogs.databasePath);
									}
									bool flag294 = this.dbDatalogsManager != null;
									if (flag294)
									{
										this.dbDatalogsManager.StartTimer();
									}
								}
							}
						}
						bool flag295 = e.Path == FlapsOverIOs.bulkflapsEnabledPath;
						if (flag295)
						{
							bool flag296 = e.NewValue == "1";
							if (flag296)
							{
								BulkMode.ActivateAtComissioning(this.dbConnection, this.parHandling);
							}
							else
							{
								bool flag297 = e.NewValue == "0";
								if (flag297)
								{
									BulkMode.DeactivateAtComissioning(this.dbConnection, this.parHandling);
								}
							}
						}
						bool flag298 = e.Path == FlapsOverIOs.bulkflapsDelayPath;
						if (flag298)
						{
							string tableName2 = null;
							this.dbConnection.GetCurrentProductTableName(ref tableName2);
							int num;
							for (int num38 = 1; num38 <= 4; num38 = num + 1)
							{
								string path = "/hardware/rejecter/rejecter" + num38.ToString() + "/Delay/Value";
								this.parHandling.WriteValue(path, e.NewValue, false);
								this.dbConnection.UpdateTable(tableName2, path, e.NewValue);
								num = num38;
							}
						}
						bool flag299 = e.Path == FlapsOverIOs.bulkflapsEjectTimePath;
						if (flag299)
						{
							string tableName3 = null;
							this.dbConnection.GetCurrentProductTableName(ref tableName3);
							int num;
							for (int num39 = 1; num39 <= 4; num39 = num + 1)
							{
								string path2 = "/hardware/rejecter/rejecter" + num39.ToString() + "/Length/Value";
								this.parHandling.WriteValue(path2, e.NewValue, false);
								this.dbConnection.UpdateTable(tableName3, path2, e.NewValue);
								num = num39;
							}
						}
						bool flag300 = e.Path == FlapsOverIOs.bulkflapsFlapsCountPath || e.Path == FlapsOverIOs.bulkflapsCalcFlapsMappingPath;
						if (flag300)
						{
							List<string> list6 = new List<string>();
							List<string> list7 = new List<string>();
							list6.Add(FlapsOverIOs.bulkflapsBorderPix1Path);
							list6.Add(FlapsOverIOs.bulkflapsBorderPix2Path);
							list6.Add(FlapsOverIOs.bulkflapsBorderPix3Path);
							list7.Add(this.parHandling.ReadPar(FlapsOverIOs.bulkflapsBorderPix1Path));
							list7.Add(this.parHandling.ReadPar(FlapsOverIOs.bulkflapsBorderPix2Path));
							list7.Add(this.parHandling.ReadPar(FlapsOverIOs.bulkflapsBorderPix3Path));
							this.dbConnection.UpdateCurrentProductTable(list6, list7);
						}
						bool flag301 = e.Path == FlapsOverIOs.bulkflapsEnableValvesPath;
						if (flag301)
						{
							bool flag302 = this.parHandling.ReadPar(FlapsOverIOs.bulkflapsEnabledPath) == "1";
							if (flag302)
							{
								string value3 = (e.NewValue == "1") ? "0" : "1";
								this.parHandling.WriteValue(FlapsOverIOs.bulkflapsEnableValvesDOPath, value3);
								bool flag303 = e.NewValue == "0";
								if (flag303)
								{
									this.BulkSwitchOnOffPath = "..\\img\\rejectOFF.png";
									this.machineState.StartMessage.BulkSortingOnOff = true;
									LogbookManager.AddEntry(SSTEntryCode.SortingSwitchedOff, 0, "", "", "", "");
									this.BulkOnOff = true;
								}
								else
								{
									this.BulkSwitchOnOffPath = "..\\img\\rejectON.png";
									this.machineState.StartMessage.BulkSortingOnOff = false;
									LogbookManager.AddEntry(SSTEntryCode.SortingSwitchedOn, 0, "", "", "", "");
									this.BulkOnOff = false;
								}
							}
						}
						bool flag304 = e.Path == Definition.rejectEnable1Path && this.BulkObjectModeActive;
						if (flag304)
						{
							string value4 = (e.NewValue == "1") ? "0" : "1";
							this.parHandling.WriteValue(FlapsOverIOs.bulkflapsEnableValvesDOPath, value4);
						}
						bool flag305 = e.Path == Definition.filterMainPath + DualEnergy.enableSubPath || e.Path == "/defaultProduct/mainTable/filter/dual/Enabled" || e.Path == "/defaultProduct/subTable/filter1/dual/Enabled";
						if (flag305)
						{
							this.parHandling.WriteValue(Definition.isLiveCalibrationActivePath, (e.NewValue == "1") ? "0" : "1");
							this.dbConnection.SetLiveCalibrationMode(Convert.ToInt32((e.NewValue == "1") ? "0" : "1"));
						}
						bool flag306 = e.Path == Definition.isLiveCalibrationActivePath;
						if (flag306)
						{
							int num40 = Convert.ToInt32(e.NewValue);
							this.dbConnection.SetLiveCalibrationMode(num40);
							bool flag307 = num40 > 0;
							if (flag307)
							{
								this.dbConnection.DeleteDtCorrection(true, true, false);
							}
						}
						bool flag308 = e.Path == Definition.iCMPath;
						if (flag308)
						{
							bool flag309 = e.NewValue == "0";
							if (flag309)
							{
								this.dbConnection.SetIntegratedComplianceMonitoring(false);
							}
							else
							{
								bool flag310 = e.NewValue == "1";
								if (flag310)
								{
									this.dbConnection.SetIntegratedComplianceMonitoring(true);
									this.parHandling.WriteValue(Definition.testcardSimulationPath, "1");
								}
							}
						}
						bool flag311 = e.Path == Definition.iVPPath;
						if (flag311)
						{
							bool flag312 = e.NewValue == "0";
							if (flag312)
							{
								this.dbConnection.SetIntegratedValidationProcess(false);
								this.dbConnection.SetIVPInfoEntry(DBIntegratedValidationProcess.ValidationIsActive, "0");
								this.ivpTimer.IVPTimer.IVPIsEnabled = false;
							}
							else
							{
								bool flag313 = e.NewValue == "1";
								if (flag313)
								{
									this.dbConnection.SetIntegratedValidationProcess(true);
									this.ivpTimer.IVPTimer.IVPIsEnabled = true;
									this.dbConnection.SetIVPInfoEntry(DBIntegratedValidationProcess.ValidationIsActive, "1");
									bool flag314 = this.productManager.referenceProduct == null;
									if (flag314)
									{
										this.CreateReferenceProduct();
									}
									this.IVP_NextCheckElapsed();
								}
							}
						}
						bool flag315 = e.Path == Definition.throughputCountPerMin;
						if (flag315)
						{
							MachineMode machineMode2 = MachineMode.Standard;
							machineMode2 = (MachineMode)Enum.Parse(typeof(MachineMode), e.NewValue);
							this.dbConnection.GetMachineMode(ref machineMode2);
							List<LaneXri> list8 = (from lane in this.ShownLanes
							where lane.Lane == Lane.All
							select lane).ToList<LaneXri>();
							bool flag316 = e.NewValue == "0";
							if (flag316)
							{
								this.dbConnection.SetThroughputCounterPerMin(false);
								bool flag317 = list8.Count > 0;
								if (flag317)
								{
									list8[0].Counter.StopUpdateThroughputPerMin();
								}
							}
							else
							{
								bool flag318 = e.NewValue == "1";
								if (flag318)
								{
									this.dbConnection.SetThroughputCounterPerMin(true);
									bool flag319 = list8.Count > 0;
									if (flag319)
									{
										list8[0].Counter.StartUpdateThroughputPerMin();
									}
								}
							}
							this.selectedXriLane.UpdateThroughputString(machineMode2, e.NewValue == "1");
						}
						bool flag320 = e.Path == Definition.auditTypePath;
						if (flag320)
						{
							bool flag321 = e.NewValue == "0";
							if (flag321)
							{
								this.dbConnection.SetAuditCheckType(0);
							}
							else
							{
								bool flag322 = e.NewValue == "1";
								if (flag322)
								{
									this.dbConnection.SetAuditCheckType(1);
									this.auditSettings.GetCurrentValues();
								}
							}
						}
						bool flag323 = e.Path == Definition.highThroughputMachine;
						if (flag323)
						{
							bool flag324 = e.NewValue == "0";
							if (flag324)
							{
								this.dbConnection.SetHighThroughputMachine(false);
								this.ShowNext = false;
								this.HighSpeedMachineActive = false;
								this.setHighSpeedValues(false);
							}
							else
							{
								bool flag325 = e.NewValue == "1";
								if (flag325)
								{
									this.dbConnection.SetHighThroughputMachine(true);
									string a = "";
									this.dbConnection.GetParameterValue(Definition.highThroughputEnabled, ref a);
									bool flag326 = a == "1";
									if (flag326)
									{
										this.setHighSpeedValues(true);
										this.HighSpeedMachineActive = true;
										bool flag327 = this.machineState.State == MachineStates.Inspecting || this.DemoMode;
										if (flag327)
										{
											this.ShowNext = true;
										}
									}
								}
							}
						}
						bool flag328 = e.Path == Definition.highThroughputEnabled;
						if (flag328)
						{
							bool flag329 = e.NewValue == "1";
							if (flag329)
							{
								this.dbConnection.UpdateCurrentProductTable(Definition.highThroughputEnabled, "1");
								this.setHighSpeedValues(true);
								this.HighSpeedMachineActive = true;
								bool flag330 = this.machineState.State == MachineStates.Inspecting || this.DemoMode;
								if (flag330)
								{
									this.ShowNext = true;
								}
							}
							else
							{
								bool flag331 = e.NewValue == "0";
								if (flag331)
								{
									this.dbConnection.UpdateCurrentProductTable(Definition.highThroughputEnabled, "0");
									this.setHighSpeedValues(false);
									this.ShowNext = false;
									this.HighSpeedMachineActive = false;
								}
							}
						}
						bool flag332 = e.Path == Definition.detectorMonitoringActive;
						if (flag332)
						{
							bool flag333 = e.NewValue == "1";
							if (flag333)
							{
								this.dbConnection.SetDetectorMonitoringIsActive(true);
							}
							else
							{
								bool flag334 = e.NewValue == "0";
								if (flag334)
								{
									this.dbConnection.SetDetectorMonitoringIsActive(false);
									this.parHandling.WriteValue(Definition.detectorMonitoringBackgroundEnable, "0", false);
								}
							}
						}
						bool flag335 = e.Path == Definition.inverseInspection;
						if (flag335)
						{
							bool flag336 = e.NewValue == "1";
							if (flag336)
							{
								this.dbConnection.SetInverseInspectionIsActive(true);
							}
							else
							{
								bool flag337 = e.NewValue == "0";
								if (flag337)
								{
									this.dbConnection.SetInverseInspectionIsActive(false);
								}
							}
						}
						bool flag338 = e.Path == Definition.detectorSesotecHSEnableMaxScanratePath;
						if (flag338)
						{
							bool flag339 = e.NewValue == "1";
							if (flag339)
							{
								this.dbConnection.SetDetectorSesotecHSEnableMaxScanrate(1U);
								this.conveyor.AdaptScanrate(false, "");
							}
							else
							{
								bool flag340 = e.NewValue == "0";
								if (flag340)
								{
									this.dbConnection.SetDetectorSesotecHSEnableMaxScanrate(0U);
									this.conveyor.AdaptScanrate(false, "");
								}
							}
						}
						bool flag341 = e.Path == Definition.detectorMonitoringPixelCountBorder;
						if (flag341)
						{
							this.dbDatalogs.LimitsAddEntry("DateTimeBgMonLiveErrorPixel", "BgMonLiveErrorPixel", e.NewValue, DateTime.Now.ToString(new CultureInfo("de-DE")));
						}
						bool flag342 = e.Path == Definition.detectorMonitoringImageLines;
						if (flag342)
						{
							this.dbDatalogs.LimitsAddEntry("DateTimeBgMonLiveImageLines", "BgMonLiveImageLines", e.NewValue, DateTime.Now.ToString(new CultureInfo("de-DE")));
						}
						bool flag343 = e.Path == Definition.detectorMonitoringThreshold;
						if (flag343)
						{
							this.dbDatalogs.LimitsAddEntry("DateTimeBgMonLiveThreshold", "BgMonLiveThreshold", e.NewValue, DateTime.Now.ToString(new CultureInfo("de-DE")));
						}
						bool flag344 = e.Path == Definition.detectorMonitoringDeleteTables;
						if (flag344)
						{
							bool flag345 = e.NewValue == "1";
							if (flag345)
							{
								string text12 = "";
								string text13 = "";
								this.dbDatalogs.MachineInfosGetSerialNumbers(ref text13, ref text12);
								this.dbDatalogs.DropDetectorSignalTable(text12.Replace("_", "").Replace(":", ""));
								this.parHandling.WriteValue(Definition.detectorMonitoringDeleteTables, "0");
							}
						}
						bool flag346 = e.Path == SVT.svtEnablePath;
						if (flag346)
						{
							bool flag347 = e.NewValue == "1";
							if (flag347)
							{
								BulkMode.ActivateAtComissioningSVT(this.dbConnection, this.parHandling);
								int num41 = 0;
								this.dbConnection.GetSVTCOMPort(ref num41);
								this.parHandling.WriteValue(SVT.svtCOMPort, num41.ToString());
							}
							else
							{
								bool flag348 = e.NewValue == "0";
								if (flag348)
								{
									BulkMode.DeactivateAtComissioning(this.dbConnection, this.parHandling);
								}
							}
						}
						bool flag349 = e.Path == SVT.svtTestEndValvePath;
						if (flag349)
						{
							int valveTestEndValve = 0;
							bool flag350 = int.TryParse(e.NewValue, out valveTestEndValve);
							if (flag350)
							{
								this.dbConnection.SetValveTestEndValve(valveTestEndValve);
							}
						}
						bool flag351 = e.Path == SVT.svtCOMPort;
						if (flag351)
						{
							int svtcomport = 0;
							bool flag352 = int.TryParse(e.NewValue, out svtcomport);
							if (flag352)
							{
								this.dbConnection.SetSVTCOMPort(svtcomport);
							}
						}
						bool flag353 = e.Path == SVT.svtValvesEnablePath;
						if (flag353)
						{
							bool flag354 = e.NewValue == "0";
							if (flag354)
							{
								this.BulkSwitchOnOffPath = "..\\img\\rejectOFF.png";
								this.BulkOnOff = true;
								this.machineState.StartMessage.BulkSortingOnOff = this.BulkOnOff;
								LogbookManager.AddEntry(SSTEntryCode.SortingSwitchedOff, 0, "", "", "", "");
							}
							else
							{
								this.BulkSwitchOnOffPath = "..\\img\\rejectON.png";
								this.BulkOnOff = false;
								this.machineState.StartMessage.BulkSortingOnOff = this.BulkOnOff;
								LogbookManager.AddEntry(SSTEntryCode.SortingSwitchedOn, 0, "", "", "", "");
							}
						}
						bool flag355 = e.Path == Definition.bulkShowContImages;
						if (flag355)
						{
							bool flag356 = e.NewValue == "1";
							if (flag356)
							{
								this.dbConnection.UpdateCurrentProductTable(Definition.bulkShowContImages, "1");
								this.BulkContImagesShown = true;
								bool flag357 = this.machineState.State == MachineStates.Inspecting;
								if (flag357)
								{
									this.parHandling.WriteValue("/filter/acquisition/SendOnlyIfContaminant/Value", "1");
								}
								this.BulkObjectModeSourcePath = "..\\img\\xriimageBulkCont.png";
							}
							else
							{
								this.dbConnection.UpdateCurrentProductTable(Definition.bulkShowContImages, "0");
								this.BulkContImagesShown = false;
								this.parHandling.WriteValue("/filter/acquisition/SendOnlyIfContaminant/Value", "0");
								this.BulkObjectModeSourcePath = "..\\img\\xriimageBulkJustCont.png";
							}
						}
						bool flag358 = e.Path == Definition.anomalyDetectionEnabled;
						if (flag358)
						{
							bool flag359 = e.NewValue == "1";
							if (flag359)
							{
								this.dbConnection.SetAnomalyDetectionEnabled(true);
								this.InitializeModelTransferConnection();
								this.parHandling.WriteValue("/application/NSaveImagesGood/Value", "100");
								this.parHandling.WriteValue("/application/NSaveImagesBad/Value", "100");
								this.aiService.CreateDefaultTableAIBgWorker();
								this.ShowBtnAIStatus = true;
								string text14 = this.productManager.CurrentLoadedProduct.TableName.Insert("AppDefault".Length, "AI");
								bool flag360 = !TableOperations.TableExists(this.dbConnection.Connection, text14);
								if (flag360)
								{
									this.dbConnection.CreateNewApplAI(this.productManager.CurrentLoadedProduct.ProductName, text14);
								}
							}
							else
							{
								bool flag361 = e.NewValue == "0";
								if (flag361)
								{
									this.dbConnection.SetAnomalyDetectionEnabled(false);
									bool flag362 = false;
									this.dbConnection.GetObjectDetectionEnabled(ref flag362);
									bool flag363 = !flag362;
									if (flag363)
									{
										this.CloseModelTransferConnection();
										this.ShowBtnAIStatus = false;
									}
									this.parHandling.WriteValue("/application/NSaveImagesGood/Value", "50");
									this.parHandling.WriteValue("/application/NSaveImagesBad/Value", "50");
								}
							}
							this.RefreshColourBtnAIStatus();
						}
						bool flag364 = e.Path == Definition.anomalyDetectionInferenceEnabled || e.Path == Definition.anomalyDetectionTrainEnabled;
						if (flag364)
						{
							bool flag365 = e.NewValue == "1";
							if (flag365)
							{
								string text15 = this.productManager.CurrentLoadedProduct.TableName.Insert("AppDefault".Length, "AI");
								bool flag366 = this.parHandling.ReadPar(Definition.anomalyDetectionEnabled) == "1" && !TableOperations.TableExists(this.dbConnection.Connection, text15) && e.NewValue == "1";
								if (flag366)
								{
									this.dbConnection.CreateNewApplAI(this.productManager.CurrentLoadedProduct.ProductName, text15);
								}
							}
							bool flag367 = e.Path == Definition.anomalyDetectionInferenceEnabled;
							if (flag367)
							{
								this.aiService.Comp19to30OnOff(e.NewValue == "1");
								this.RefreshColourBtnAIStatus();
							}
						}
						bool flag368 = e.Path == Definition.xraySourcePortCom;
						if (flag368)
						{
							int xraySourceComPort = 0;
							bool flag369 = int.TryParse(e.NewValue, out xraySourceComPort);
							if (flag369)
							{
								this.dbConnection.SetXraySourceComPort(xraySourceComPort);
							}
						}
						bool flag370 = e.Path == Definition.objectDetectionEnabled;
						if (flag370)
						{
							bool flag371 = e.NewValue == "1";
							if (flag371)
							{
								this.dbConnection.SetObjectDetectionEnabled(true);
								this.InitializeModelTransferConnection();
								this.aiService.CreateDefaultTableAIBgWorker();
								this.ShowBtnAIStatus = true;
								string text16 = this.productManager.CurrentLoadedProduct.TableName.Insert("AppDefault".Length, "AI");
								bool flag372 = !TableOperations.TableExists(this.dbConnection.Connection, text16);
								if (flag372)
								{
									this.dbConnection.CreateNewApplAI(this.productManager.CurrentLoadedProduct.ProductName, text16);
								}
							}
							else
							{
								bool flag373 = e.NewValue == "0";
								if (flag373)
								{
									this.dbConnection.SetObjectDetectionEnabled(false);
									bool flag374 = false;
									this.dbConnection.GetAnomalyDetectionEnabled(ref flag374);
									bool flag375 = !flag374;
									if (flag375)
									{
										this.CloseModelTransferConnection();
										this.ShowBtnAIStatus = false;
									}
								}
							}
							this.RefreshColourBtnAIStatus();
						}
						bool flag376 = e.Path == Definition.objectDetectionInferenceEnabled;
						if (flag376)
						{
							bool flag377 = e.NewValue == "1";
							if (flag377)
							{
								string text17 = this.productManager.CurrentLoadedProduct.TableName.Insert("AppDefault".Length, "AI");
								bool flag378 = this.parHandling.ReadPar(Definition.objectDetectionEnabled) == "1" && !TableOperations.TableExists(this.dbConnection.Connection, text17) && e.NewValue == "1";
								if (flag378)
								{
									this.dbConnection.CreateNewApplAI(this.productManager.CurrentLoadedProduct.ProductName, text17);
								}
							}
							this.RefreshColourBtnAIStatus();
						}
						bool flag379 = e.Path == Definition.testAtCustomerSite;
						if (flag379)
						{
							bool flag380 = e.NewValue == "1";
							if (flag380)
							{
								this.dbConnection.SetTestAtCustomerSite(true);
							}
							else
							{
								this.dbConnection.SetTestAtCustomerSite(false);
							}
						}
						bool flag381 = e.Path == Definition.bulkOverlapImageShowPath;
						if (flag381)
						{
							bool flag382 = e.NewValue == "1";
							if (flag382)
							{
								XRSImageBoxViewModel.BulkOverlapImageShow = true;
							}
							else
							{
								bool flag383 = e.NewValue == "0";
								if (flag383)
								{
									XRSImageBoxViewModel.BulkOverlapImageShow = false;
								}
							}
						}
						bool flag384 = e.Path == Definition.autoRetrain;
						if (flag384)
						{
							bool flag385 = e.NewValue == "0";
							if (flag385)
							{
								this.dbConnection.SetAutoRetrain(false);
								this.AutoRetrainVisible = false;
								this.AutoRetrainUndo = false;
								this.RetrainMode = false;
							}
							else
							{
								bool flag386 = e.NewValue == "1";
								if (flag386)
								{
									bool flag387 = !this.BulkModeActive;
									if (flag387)
									{
										this.dbConnection.SetAutoRetrain(true);
										this.AutoRetrainVisible = true;
									}
								}
							}
						}
						bool flag388 = e.Path == Definition.objectBasedFlapsEnabledPath;
						if (flag388)
						{
							bool flag389 = e.NewValue == "1";
							if (flag389)
							{
								this.dbConnection.SetObjectBasedFlapsEnabled(true);
							}
							else
							{
								bool flag390 = e.NewValue == "0";
								if (flag390)
								{
									this.dbConnection.SetObjectBasedFlapsEnabled(false);
								}
							}
						}
						bool flag391 = e.Path == Definition.objectBasedFlapsEnableValvesPath;
						if (flag391)
						{
							List<string> list9 = new List<string>();
							List<string> list10 = new List<string>();
							list9.Add(Definition.rejectEnable1Path);
							list9.Add(Definition.rejectEnable2Path);
							list9.Add(Definition.rejectEnable3Path);
							list9.Add(Definition.rejectEnable4Path);
							list10.Add(this.parHandling.ReadPar(Definition.rejectEnable1Path));
							list10.Add(this.parHandling.ReadPar(Definition.rejectEnable2Path));
							list10.Add(this.parHandling.ReadPar(Definition.rejectEnable3Path));
							list10.Add(this.parHandling.ReadPar(Definition.rejectEnable4Path));
							this.dbConnection.UpdateCurrentProductTable(list9, list10);
						}
						bool flag392 = e.Path == Definition.objectBasedFlapsCountPath;
						if (flag392)
						{
							int objectBasedFlapsCount = 0;
							bool flag393 = int.TryParse(e.NewValue, out objectBasedFlapsCount);
							if (flag393)
							{
								this.dbConnection.SetObjectBasedFlapsCount(objectBasedFlapsCount);
							}
						}
						bool flag394 = e.Path == Definition.objectBasedFlapsCountPath;
						if (flag394)
						{
							int objectBasedFlapsCount2 = 0;
							bool flag395 = int.TryParse(e.NewValue, out objectBasedFlapsCount2);
							if (flag395)
							{
								this.dbConnection.SetObjectBasedFlapsCount(objectBasedFlapsCount2);
							}
						}
						bool flag396 = e.Path == Definition.objectBasedFlapsEjectTimePath;
						if (flag396)
						{
							List<string> list11 = new List<string>();
							List<string> list12 = new List<string>();
							list11.Add(Definition.rejectLength1Path);
							list11.Add(Definition.rejectLength2Path);
							list11.Add(Definition.rejectLength3Path);
							list11.Add(Definition.rejectLength4Path);
							list12.Add(e.NewValue);
							list12.Add(e.NewValue);
							list12.Add(e.NewValue);
							list12.Add(e.NewValue);
							this.dbConnection.UpdateCurrentProductTable(list11, list12);
						}
						bool flag397 = e.Path == Definition.objectBasedFlapsDelayTimePath;
						if (flag397)
						{
							List<string> list13 = new List<string>();
							List<string> list14 = new List<string>();
							list13.Add(Definition.rejectDelay1Path);
							list13.Add(Definition.rejectDelay2Path);
							list13.Add(Definition.rejectDelay3Path);
							list13.Add(Definition.rejectDelay4Path);
							list14.Add(e.NewValue);
							list14.Add(e.NewValue);
							list14.Add(e.NewValue);
							list14.Add(e.NewValue);
							this.dbConnection.UpdateCurrentProductTable(list13, list14);
						}
					}
				}
			}
			catch (Exception ex)
			{
				Dialog.Show("Error: parHandling_ParameterChanged", ex.Message);
				bool flag398 = this.dbConnection.Connection.State != ConnectionState.Open;
				if (flag398)
				{
					this.dbConnection.Connect(DBConnection.databasePath);
				}
			}
		}

		// Token: 0x06002471 RID: 9329 RVA: 0x000F6D60 File Offset: 0x000F4F60
		private void calculateNewDelayTime()
		{
			string s = "";
			double num = 0.0;
			double num2 = 0.0;
			double num3 = 0.0;
			double num4 = 0.0;
			double num5 = 1.0;
			CultureInfo provider = new CultureInfo("en-US");
			this.dbConnection.GetGeometryFactor(ref s);
			double.TryParse(s, NumberStyles.Any, provider, out num);
			double.TryParse(this.parHandling.ReadPar(Acquisition.nMaxLinesPath), NumberStyles.Any, provider, out num2);
			double.TryParse(this.parHandling.ReadPar(Definition.conveyingSpeedPath), NumberStyles.Any, provider, out num3);
			double.TryParse(this.parHandling.ReadPar(Acquisition.binningPath), NumberStyles.Any, provider, out num4);
			bool flag = this.parHandling.ReadPar(Definition.detectorTypePath) == "1" && this.parHandling.ReadPar(Definition.detectorSesotecPixelSizePath) == "0.4";
			if (flag)
			{
				bool flag2 = num3 > 650.0;
				if (flag2)
				{
					num3 = 650.0;
				}
				num5 = 2.0;
			}
			bool flag3 = num3 > 0.0;
			if (flag3)
			{
				int num6 = (int)(num2 * num * 1000.0 / num3 * num4 / num5);
			}
		}

		// Token: 0x06002472 RID: 9330 RVA: 0x000F6EC4 File Offset: 0x000F50C4
		private void UpdateHighLevelSensorPaths(bool enable)
		{
			this.parHandling.WriteValue(Definition.msDelayedPusherEnable1Path, enable ? "1" : "0", false);
			this.parHandling.WriteValue(Definition.msDelayedPusherEnable2Path, enable ? "1" : "0", false);
			this.parHandling.WriteValue(Definition.msSensorChecksEnable1Path, enable ? "1" : "0", false);
			this.parHandling.WriteValue(Definition.msSensorChecksEnable2Path, enable ? "1" : "0", false);
			foreach (ProductData productData in this.productManager.ProductList)
			{
				this.dbConnection.UpdateTable(productData.TableName, Definition.msDelayedPusherEnable1Path, enable ? "1" : "0");
				this.dbConnection.UpdateTable(productData.TableName, Definition.msDelayedPusherEnable2Path, enable ? "1" : "0");
				this.dbConnection.UpdateTable(productData.TableName, Definition.msSensorChecksEnable1Path, enable ? "1" : "0");
				this.dbConnection.UpdateTable(productData.TableName, Definition.msSensorChecksEnable2Path, enable ? "1" : "0");
			}
			this.dbConnection.UpdateTable(DBConnection.defaultApplication, Definition.msDelayedPusherEnable1Path, enable ? "1" : "0");
			this.dbConnection.UpdateTable(DBConnection.defaultApplication, Definition.msDelayedPusherEnable2Path, enable ? "1" : "0");
			this.dbConnection.UpdateTable(DBConnection.defaultApplication, Definition.msSensorChecksEnable1Path, enable ? "1" : "0");
			this.dbConnection.UpdateTable(DBConnection.defaultApplication, Definition.msSensorChecksEnable2Path, enable ? "1" : "0");
		}

		// Token: 0x06002473 RID: 9331 RVA: 0x000F70CC File Offset: 0x000F52CC
		private void LoadGlobalValues()
		{
			bool flag = false;
			int num = 0;
			uint num2 = 0U;
			ushort num3 = 0;
			double num4 = 0.0;
			string value = "";
			ConveyingDirection conveyingDirection = ConveyingDirection.LeftToRight;
			MachineMode machineMode = MachineMode.Standard;
			DetectorType detectorType = DetectorType.DT;
			bool detectorType2 = this.dbConnection.GetDetectorType(ref detectorType);
			int num5;
			if (detectorType2)
			{
				ParameterHandling parameterHandling = this.parHandling;
				string detectorTypePath = Definition.detectorTypePath;
				num5 = (int)detectorType;
				parameterHandling.WriteValue(detectorTypePath, num5.ToString(), false);
			}
			this.dbConnection.GetLastXrayOn(ref value);
			this.parHandling.WriteValue(MachineState.lastXRayOnPath, value, false);
			this.dbConnection.GetUsbDriveLetter(ref value);
			this.parHandling.WriteValue(MachineState.usbDriveLetterPath, value, false);
			this.dbConnection.GetConveyingDirection(ref conveyingDirection);
			ParameterHandling parameterHandling2 = this.parHandling;
			string conveyingDirectionPath = Conveyor.conveyingDirectionPath;
			num5 = (int)conveyingDirection;
			parameterHandling2.WriteValue(conveyingDirectionPath, num5.ToString(), false);
			this.dbConnection.GetGeometryFactor(ref value);
			this.parHandling.WriteValue(Conveyor.geometryFactorPath, value, false);
			this.dbConnection.GetIgnoredLeftPixel(ref num);
			this.parHandling.WriteValue(ParameterHandling.ignoreLeftPixelPath, num.ToString(), false);
			this.dbConnection.GetIgnoredRightPixel(ref num);
			this.parHandling.WriteValue(ParameterHandling.ignoreRightPixelPath, num.ToString(), false);
			this.parHandling.WriteValue("/machine/IPConfiguration/Value", IPInfo.GetIPConfigString(), false);
			this.dbConnection.GetXRaySourceType(ref value);
			this.parHandling.WriteValue(Definition.xRaySourceTypePath, value, false);
			this.dbConnection.GetMachineMode(ref machineMode);
			ParameterHandling parameterHandling3 = this.parHandling;
			string machineModePath = Definition.machineModePath;
			num5 = (int)machineMode;
			parameterHandling3.WriteValue(machineModePath, num5.ToString(), false);
			this.dbConnection.GetUseAuditCheckLamp(ref flag);
			this.parHandling.WriteValue(Definition.useAuditCheckLampPath, flag ? "1" : "0", false);
			this.dbConnection.GetDeviceAddress(ref value);
			this.parHandling.WriteValue(Definition.deviceAddressPath, value, false);
			this.dbConnection.GetMinBackgroundValue(ref num);
			this.parHandling.WriteValue(Definition.minBackgroundValuePath, num.ToString(), false);
			this.dbConnection.GetMaxBackgroundValue(ref num);
			this.parHandling.WriteValue(Definition.maxBackgroundValuePath, num.ToString(), false);
			this.dbConnection.GetNAutolearn(ref num);
			this.parHandling.WriteValue(Autotrain.nAutolearnPath, num.ToString(), false);
			this.dbConnection.GetNAutolearnDefault(ref num);
			this.parHandling.WriteValue(Autotrain.nAutolearnDefaultPath, num.ToString(), false);
			this.dbConnection.GetNAutolearnDefaultBulk(ref num);
			this.parHandling.WriteValue(Autotrain.nAutolearnDefaultBulkPath, num.ToString(), false);
			this.dbConnection.GetNSaveImagesGood(ref num);
			this.parHandling.WriteValue(Definition.nSaveImagesGoodPath, num.ToString(), false);
			this.dbConnection.GetNSaveImagesBad(ref num);
			this.parHandling.WriteValue(Definition.nSaveImagesBadPath, num.ToString(), false);
			this.dbConnection.GetStopConveyorIfError(ref flag);
			this.parHandling.WriteValue(Definition.beltStopOnErrorEnablePath, flag ? "1" : "0", false);
			this.parHandling.WriteValue(Definition.beltStopOnVerifyErrorEnablePath, (flag && this.errorHandling.MissingObjectsEnabled) ? "1" : "0", false);
			this.parHandling.WriteValue(Definition.beltStopOnBinFullErrorEnablePath, (flag && this.errorHandling.BinFullEnabled) ? "1" : "0", false);
			this.dbConnection.GetSVTMinAirPressure(ref num4);
			this.parHandling.WriteValue(SVT.svtMinAirPressurePath, num4.ToString(new CultureInfo("en-US")), false);
			this.dbConnection.GetMultiProductsEnabled(ref flag);
			this.parHandling.WriteValue(Definition.multiProductsEnablePath, flag ? "1" : "0", false);
			num4 = this.conveyor.Speed * 1000.0;
			this.parHandling.WriteValue(Definition.conveyingSpeedPath, ((int)num4).ToString(), false);
			this.dbConnection.GetSshMotorCurrentLimit(ref num);
			this.parHandling.WriteValue(Definition.sshMotorCurrentLimitPath, num.ToString(), false);
			this.dbConnection.GetBulkCleaningVibrator(ref flag);
			this.parHandling.WriteValue(Definition.bulkCleaningVibratorPath, flag ? "1" : "0", false);
			bool flag2 = this.dbConnection.GetImageServerTimeout(ref num) && this.imageTransfer != null;
			if (flag2)
			{
				this.imageTransfer.SetReceiveTimeout(num * 1000);
				this.parHandling.WriteValue(Definition.imageRecvTimeoutPath, num.ToString(), false);
			}
			bool conveyorStopDelayTime = this.dbConnection.GetConveyorStopDelayTime(ref num);
			if (conveyorStopDelayTime)
			{
				this.parHandling.WriteValue(Definition.conveyorStopDelayTimePath, num.ToString(), false);
			}
			bool ncriticalArcs = this.dbConnection.GetNCriticalArcs(ref num);
			if (ncriticalArcs)
			{
				this.parHandling.WriteValue(Definition.nCriticalArcsPath, num.ToString(), false);
			}
			bool criticalArcsPeriod = this.dbConnection.GetCriticalArcsPeriod(ref num);
			if (criticalArcsPeriod)
			{
				this.parHandling.WriteValue(Definition.criticalArcsPeriodPath, num.ToString(), false);
			}
			bool flag3 = this.dbConnection.GetMultiLaneView(ref flag);
			if (flag3)
			{
				this.parHandling.WriteValue(Definition.multiLaneViewPath, flag ? "1" : "0", false);
			}
			bool msbinTimeout = this.dbConnection.GetMSBinTimeout(true, ref num);
			if (msbinTimeout)
			{
				this.parHandling.WriteValue(Definition.msBinTimeout1Path, num.ToString(), false);
			}
			bool msbinTimeout2 = this.dbConnection.GetMSBinTimeout(false, ref num);
			if (msbinTimeout2)
			{
				this.parHandling.WriteValue(Definition.msBinTimeout2Path, num.ToString(), false);
			}
			bool mssafetyTime = this.dbConnection.GetMSSafetyTime(true, ref num);
			if (mssafetyTime)
			{
				this.parHandling.WriteValue(Definition.msSafetyTimeRejectCheck1Path, num.ToString(), false);
			}
			bool mssafetyTime2 = this.dbConnection.GetMSSafetyTime(false, ref num);
			if (mssafetyTime2)
			{
				this.parHandling.WriteValue(Definition.msSafetyTimeRejectCheck2Path, num.ToString(), false);
			}
			bool multiProductView = this.dbConnection.GetMultiProductView(ref flag);
			if (multiProductView)
			{
				this.parHandling.WriteValue(Definition.multiProductViewPath, flag ? "1" : "0", false);
			}
			bool flag4 = this.dbConnection.GetDoorLockAvailable(ref flag);
			if (flag4)
			{
				this.parHandling.WriteValue(Definition.doorLockAvailablePath, flag ? "1" : "0", false);
			}
			bool highLevelSensors = this.dbConnection.GetHighLevelSensors(ref flag);
			if (highLevelSensors)
			{
				this.parHandling.WriteValue(Definition.highLevelSensorsPath, flag ? "1" : "0", false);
			}
			bool maxUsers = this.dbConnection.GetMaxUsers(ref num);
			if (maxUsers)
			{
				this.parHandling.WriteValue(Definition.maxUsersPath, num.ToString(), false);
			}
			num = 0;
			bool sshPixelTolerance = this.dbConnection.GetSshPixelTolerance(ref num);
			if (sshPixelTolerance)
			{
				this.parHandling.WriteValue(Definition.sshPxTolerancePath, num.ToString(), false);
			}
			flag = false;
			bool buzzerAvailable = this.dbConnection.GetBuzzerAvailable(ref flag);
			if (buzzerAvailable)
			{
				this.parHandling.WriteValue(Definition.buzzerAvailablePath, flag ? "1" : "0", false);
			}
			flag = false;
			bool multiRegionEnabled = this.dbConnection.GetMultiRegionEnabled(ref flag);
			if (multiRegionEnabled)
			{
				this.parHandling.WriteValue(Definition.multiRegionPath, flag ? "1" : "0", false);
			}
			flag = false;
			bool autoZoom = this.dbConnection.GetAutoZoom(ref flag);
			if (autoZoom)
			{
				this.parHandling.WriteValue(Definition.autoZoomPath, flag ? "1" : "0", false);
			}
			flag = false;
			bool noBeltStartOnError = this.dbConnection.GetNoBeltStartOnError(ref flag);
			if (noBeltStartOnError)
			{
				this.parHandling.WriteValue(Definition.noBeltStartOnErrorPath, flag ? "1" : "0", false);
			}
			flag = true;
			bool logImageIndex = this.dbConnection.GetLogImageIndex(ref flag);
			if (logImageIndex)
			{
				this.parHandling.WriteValue(Definition.logImageIndexPath, flag ? "1" : "0", false);
			}
			flag = true;
			bool dtscanrateLimitEnable = this.dbConnection.GetDTScanrateLimitEnable(ref flag);
			if (dtscanrateLimitEnable)
			{
				this.parHandling.WriteValue(Definition.dtScanrateLimitEnablePath, flag ? "1" : "0", false);
			}
			num = 0;
			bool nrejectsLimit = this.dbConnection.GetNRejectsLimit(ref num);
			if (nrejectsLimit)
			{
				this.parHandling.WriteValue(Definition.rejecterNRejectsLimitPath, num.ToString(), false);
			}
			num = 0;
			bool rejectPeriodLimit = this.dbConnection.GetRejectPeriodLimit(ref num);
			if (rejectPeriodLimit)
			{
				this.parHandling.WriteValue(Definition.rejecterRejectPeriodLimitPath, num.ToString(), false);
			}
			flag = false;
			bool beltStartIfSwitchOpen = this.dbConnection.GetBeltStartIfSwitchOpen(ref flag);
			if (beltStartIfSwitchOpen)
			{
				this.parHandling.WriteValue(Definition.conveyorInManualModePath, flag ? "1" : "0", false);
			}
			flag = false;
			bool insightNetAvailable = this.dbConnection.GetInsightNetAvailable(ref flag);
			if (insightNetAvailable)
			{
				this.parHandling.WriteValue(Definition.insightNetAvailablePath, flag ? "1" : "0", false);
			}
			num = 0;
			bool sstrecvTimeout = this.dbConnection.GetSSTRecvTimeout(ref num);
			if (sstrecvTimeout)
			{
				this.parHandling.WriteValue(Definition.sstReceiveTimeoutPath, num.ToString(), false);
			}
			num = 0;
			bool sstmaxConnections = this.dbConnection.GetSSTMaxConnections(ref num);
			if (sstmaxConnections)
			{
				this.parHandling.WriteValue(Definition.sstMaxConnectionsPath, num.ToString(), false);
			}
			flag = false;
			bool op2HlExtendedRights = this.dbConnection.GetOp2HlExtendedRights(ref flag);
			if (op2HlExtendedRights)
			{
				this.parHandling.WriteValue(Definition.op2HlExtendedRightsPath, flag ? "1" : "0", false);
			}
			flag = false;
			bool detectorTempCorrection = this.dbConnection.GetDetectorTempCorrection(ref flag);
			if (detectorTempCorrection)
			{
				this.parHandling.WriteValue(Definition.detectorTempCorrectionPath, flag ? "1" : "0", false);
			}
			flag = false;
			bool saveUncommittedErrors = this.dbConnection.GetSaveUncommittedErrors(ref flag);
			if (saveUncommittedErrors)
			{
				this.parHandling.WriteValue(Definition.saveUncommittedErrorsPath, flag ? "1" : "0", false);
			}
			flag = false;
			bool saveEdgePixelInPNG = this.dbConnection.GetSaveEdgePixelInPNG(ref flag);
			if (saveEdgePixelInPNG)
			{
				this.parHandling.WriteValue(Definition.saveEdgePixelInPNGPath, flag ? "1" : "0", false);
			}
			num = 0;
			bool maxBatches = this.dbConnection.GetMaxBatches(ref num);
			if (maxBatches)
			{
				this.parHandling.WriteValue(Definition.maxBatchesPath, num.ToString(), false);
			}
			IPAddress ipaddress = null;
			bool flag5 = this.dbConnection.GetIPDetectorSesotec(ref ipaddress) && ipaddress != null;
			if (flag5)
			{
				this.parHandling.WriteValue(Definition.ipDetectorSesotecPath, ipaddress.ToString(), false);
			}
			ipaddress = null;
			bool flag6 = this.dbConnection.GetIPDetectorDT(ref ipaddress) && ipaddress != null;
			if (flag6)
			{
				this.parHandling.WriteValue(Definition.ipDetectorDtPath, ipaddress.ToString(), false);
			}
			flag = false;
			bool batchesEnabled = this.dbConnection.GetBatchesEnabled(ref flag);
			if (batchesEnabled)
			{
				this.parHandling.WriteValue(Definition.batchesEnabledPath, flag ? "1" : "0", false);
			}
			flag = false;
			bool statisticsEnabled = this.dbConnection.GetStatisticsEnabled(ref flag);
			if (statisticsEnabled)
			{
				this.parHandling.WriteValue(Definition.statisticsEnabledPath, flag ? "1" : "0", false);
			}
			num = 0;
			bool flag7 = this.dbConnection.GetMaxStatistics(ref num) && num > 0;
			if (flag7)
			{
				this.parHandling.WriteValue(Definition.maxStatisticsPath, num.ToString(), false);
			}
			num = 0;
			bool flag8 = this.dbConnection.GetSshMaxMoveTimeXRayMotor(ref num) && num > 0;
			if (flag8)
			{
				this.parHandling.WriteValue(Definition.sshMaxMoveTimeXRayMotorPath, num.ToString(), false);
			}
			flag = false;
			bool ukuserRightsHL = this.dbConnection.GetUKUserRightsHL(ref flag);
			if (ukuserRightsHL)
			{
				this.parHandling.WriteValue(Definition.ukUserRightsHLPath, flag ? "1" : "0", false);
			}
			num4 = 0.0;
			bool flag9 = this.dbConnection.GetFCVoltageFactor(ref num4) && num4 >= 0.0;
			if (flag9)
			{
				this.parHandling.WriteValue(Definition.fcVoltageFactorPath, num4.ToString(new CultureInfo("en-GB")), false);
			}
			num = 0;
			bool flag10 = this.dbConnection.GetXRayMotorThreshold(ref num) && num > 0;
			if (flag10)
			{
				this.parHandling.WriteValue(Definition.xrayMotorThresholdPath, num.ToString(), false);
			}
			MachineType machineType = MachineType.MachineTypeUndefined;
			bool machineType2 = this.dbConnection.GetMachineType(ref machineType);
			if (machineType2)
			{
				ParameterHandling parameterHandling4 = this.parHandling;
				string machineTypePath = Definition.machineTypePath;
				num5 = (int)machineType;
				parameterHandling4.WriteValue(machineTypePath, num5.ToString(), false);
			}
			flag = false;
			bool advancedFilters = this.dbConnection.GetAdvancedFilters(ref flag);
			if (advancedFilters)
			{
				this.parHandling.WriteValue(Definition.advancedFiltersPath, flag ? "1" : "0", false);
			}
			num = 0;
			bool flag11 = this.dbConnection.GetInOutBlockedTime(ref num) && num > 0;
			if (flag11)
			{
				this.parHandling.WriteValue(Definition.inOutletBlockedTimePath, num.ToString(), false);
			}
			num = 0;
			bool flag12 = this.dbConnection.GetMaxLogbookEntries(ref num) && num > 0;
			if (flag12)
			{
				this.parHandling.WriteValue(Definition.maxLogbookEntriesPath, num.ToString(), false);
			}
			num = 0;
			bool flag13 = this.dbConnection.GetMaxProducts(ref num) && num > 0;
			if (flag13)
			{
				this.parHandling.WriteValue(Definition.maxProductsPath, num.ToString(), false);
			}
			flag = false;
			bool flag14 = this.dbConnection.GetStartXRayNoUser(ref flag);
			if (flag14)
			{
				this.parHandling.WriteValue(Definition.startXRayNoUserPath, flag ? "1" : "0", false);
			}
			this.parHandling.WriteValue(Definition.macGUIPCPath, NIC.GetFirstMAC(), false);
			flag = false;
			bool rfidloginAvailable = this.dbConnection.GetRFIDLoginAvailable(ref flag);
			if (rfidloginAvailable)
			{
				this.parHandling.WriteValue(Definition.rfidLoginAvailablePath, flag ? "1" : "0", false);
			}
			num2 = 0U;
			this.dbConnection.GetRFIDLoginComPort(ref num2);
			this.parHandling.WriteValue(Definition.rfidLoginComPortPath, num2.ToString(), false);
			num2 = 0U;
			bool autoKVAdjustment = this.dbConnection.GetAutoKVAdjustment(ref num2);
			if (autoKVAdjustment)
			{
				this.parHandling.WriteValue(Definition.autoKVAdjustmentPath, num2.ToString(), false);
			}
			num = 0;
			bool guiinactiveTime = this.dbConnection.GetGUIInactiveTime(ref num);
			if (guiinactiveTime)
			{
				this.parHandling.WriteValue(Definition.guiInactiveTimePath, num.ToString(), false);
			}
			flag = false;
			bool rejectVerifyLowActive = this.dbConnection.GetRejectVerifyLowActive(1, ref flag);
			if (rejectVerifyLowActive)
			{
				this.parHandling.WriteValue(Definition.rejectVerifyLowActive1Path, flag ? "1" : "0", false);
			}
			flag = false;
			bool rejectVerifyLowActive2 = this.dbConnection.GetRejectVerifyLowActive(2, ref flag);
			if (rejectVerifyLowActive2)
			{
				this.parHandling.WriteValue(Definition.rejectVerifyLowActive2Path, flag ? "1" : "0", false);
			}
			flag = false;
			bool rejectVerifyLowActive3 = this.dbConnection.GetRejectVerifyLowActive(3, ref flag);
			if (rejectVerifyLowActive3)
			{
				this.parHandling.WriteValue(Definition.rejectVerifyLowActive3Path, flag ? "1" : "0", false);
			}
			flag = false;
			bool rejectVerifyLowActive4 = this.dbConnection.GetRejectVerifyLowActive(4, ref flag);
			if (rejectVerifyLowActive4)
			{
				this.parHandling.WriteValue(Definition.rejectVerifyLowActive4Path, flag ? "1" : "0", false);
			}
			flag = false;
			num = 0;
			bool xrayOpHoursLimit = this.dbConnection.GetXRayOpHoursLimit(ref num);
			if (xrayOpHoursLimit)
			{
				this.parHandling.WriteValue(Definition.limitXraySourceOperatingHoursPath, num.ToString(), false);
			}
			num = 0;
			bool dynXRayOpHoursLimit = this.dbConnection.GetDynXRayOpHoursLimit(ref num);
			if (dynXRayOpHoursLimit)
			{
				this.parHandling.WriteValue(Definition.dynamiclimitXraySourceOperatingHoursPath, num.ToString(), false);
			}
			num4 = 0.0;
			bool xrayOperatingHoursSesotec = this.dbConnection.GetXRayOperatingHoursSesotec(ref num4);
			if (xrayOperatingHoursSesotec)
			{
				this.parHandling.WriteValue(Definition.xRayOperatingHoursSesotecPath, Convert.ToInt32(num4).ToString(), false);
			}
			num4 = 0.0;
			bool dynXRayOpHoursFactor = this.dbConnection.GetDynXRayOpHoursFactor(ref num4);
			if (dynXRayOpHoursFactor)
			{
				this.parHandling.WriteValue(Definition.dynlimitXraySourceOperatingHoursFactPath, num4.ToString(new CultureInfo("en-GB")), false);
			}
			num4 = 0.0;
			bool conveyingSpeedMaxDC = this.dbConnection.GetConveyingSpeedMaxDC(ref num4);
			if (conveyingSpeedMaxDC)
			{
				this.parHandling.WriteValue(Definition.conveyingSpeedMaxDCPath, num4.ToString(new CultureInfo("en-GB")), false);
			}
			num3 = 0;
			bool inspectionWidth = this.dbConnection.GetInspectionWidth(ref num3);
			if (inspectionWidth)
			{
				this.parHandling.WriteValue(Definition.inspectionWidthPath, num3.ToString());
			}
			num3 = 0;
			bool inspectionHeight = this.dbConnection.GetInspectionHeight(ref num3);
			if (inspectionHeight)
			{
				this.parHandling.WriteValue(Definition.inspectionHeightPath, num3.ToString());
			}
			value = "";
			bool machineSubType = this.dbConnection.GetMachineSubType(ref value);
			if (machineSubType)
			{
				this.parHandling.WriteValue(Definition.machineSubTypePath, value);
			}
			value = "";
			bool serialNumber = this.dbConnection.GetSerialNumber(ref value);
			if (serialNumber)
			{
				this.parHandling.WriteValue(Definition.machineSerialNumberPath, value);
			}
			value = "";
			bool deviceName = this.dbConnection.GetDeviceName(ref value);
			if (deviceName)
			{
				this.parHandling.WriteValue(Definition.machineNamePath, value);
			}
			value = "";
			bool lineName = this.dbConnection.GetLineName(ref value);
			if (lineName)
			{
				this.parHandling.WriteValue(Definition.lineNamePath, value);
			}
			value = "";
			bool location = this.dbConnection.GetLocation(ref value);
			if (location)
			{
				this.parHandling.WriteValue(Definition.machinelocationPath, value);
			}
			flag = false;
			bool testcardSimulation = this.dbConnection.GetTestcardSimulation(ref flag);
			if (testcardSimulation)
			{
				this.parHandling.WriteValue(Definition.testcardSimulationPath, flag ? "1" : "0", false);
			}
			flag = false;
			bool mxfeatures04Det = this.dbConnection.GetMXFeatures04Det(ref flag);
			if (mxfeatures04Det)
			{
				this.parHandling.WriteValue(Definition.isMXFeatures04detector, flag ? "1" : "0", false);
			}
			flag = false;
			bool dbdatalogsIsActive = this.dbConnection.GetDBDatalogsIsActive(ref flag);
			if (dbdatalogsIsActive)
			{
				this.parHandling.WriteValue(Definition.isDBDatalogsActive, flag ? "1" : "0", false);
			}
			num = 0;
			bool liveCalibrationMode = this.dbConnection.GetLiveCalibrationMode(ref num);
			if (liveCalibrationMode)
			{
				this.parHandling.WriteValue(Definition.isLiveCalibrationActivePath, Convert.ToString(num));
			}
			flag = false;
			bool bulkObjectEnabled = this.dbConnection.GetBulkObjectEnabled(ref flag);
			if (bulkObjectEnabled)
			{
				this.parHandling.WriteValue(Definition.bulkObjectProductsPath, flag ? "1" : "0", false);
			}
			flag = false;
			bool bulkBinFullSensor = this.dbConnection.GetBulkBinFullSensor(ref flag);
			if (bulkBinFullSensor)
			{
				this.parHandling.WriteValue(Definition.bulkBinFullSensorPath, flag ? "1" : "0", false);
			}
			flag = false;
			bool bulkRejectVerifySensor = this.dbConnection.GetBulkRejectVerifySensor(ref flag);
			if (bulkRejectVerifySensor)
			{
				this.parHandling.WriteValue(Definition.bulkRejectVerifySensorrPath, flag ? "1" : "0", false);
			}
			flag = false;
			bool bulkSpeedBelow = this.dbConnection.GetBulkSpeedBelow08(ref flag);
			if (bulkSpeedBelow)
			{
				this.parHandling.WriteValue(Definition.bulkSpeedBelow08Path, flag ? "1" : "0", false);
			}
			flag = false;
			bool integratedComplianceMonitoring = this.dbConnection.GetIntegratedComplianceMonitoring(ref flag);
			if (integratedComplianceMonitoring)
			{
				this.parHandling.WriteValue(Definition.iCMPath, flag ? "1" : "0", false);
			}
			flag = false;
			bool integratedValidationProcess = this.dbConnection.GetIntegratedValidationProcess(ref flag);
			if (integratedValidationProcess)
			{
				this.parHandling.WriteValue(Definition.iVPPath, flag ? "1" : "0", false);
			}
			flag = false;
			bool throughputCounterPerMin = this.dbConnection.GetThroughputCounterPerMin(ref flag);
			if (throughputCounterPerMin)
			{
				this.parHandling.WriteValue(Definition.throughputCountPerMin, flag ? "1" : "0", false);
				List<LaneXri> list = (from lane in this.ShownLanes
				where lane.Lane == Lane.All
				select lane).ToList<LaneXri>();
				bool flag15 = flag;
				if (flag15)
				{
					bool flag16 = list.Count > 0;
					if (flag16)
					{
						list[0].Counter.StartUpdateThroughputPerMin();
					}
				}
			}
			num = 0;
			bool auditCheckType = this.dbConnection.GetAuditCheckType(ref num);
			if (auditCheckType)
			{
				this.parHandling.WriteValue(Definition.auditTypePath, num.ToString(), false);
				bool flag17 = num == 1;
				if (flag17)
				{
					this.auditSettings.AuditRequired.newAuditCheck = true;
				}
				else
				{
					this.auditSettings.AuditRequired.newAuditCheck = false;
				}
			}
			flag = false;
			bool highThroughputMachine = this.dbConnection.GetHighThroughputMachine(ref flag);
			if (highThroughputMachine)
			{
				this.parHandling.WriteValue(Definition.highThroughputMachine, flag ? "1" : "0", false);
			}
			flag = false;
			bool detectorMonitoringIsActive = this.dbConnection.GetDetectorMonitoringIsActive(ref flag);
			if (detectorMonitoringIsActive)
			{
				this.parHandling.WriteValue(Definition.detectorMonitoringActive, flag ? "1" : "0", false);
			}
			flag = false;
			bool inverseInspectionIsActive = this.dbConnection.GetInverseInspectionIsActive(ref flag);
			if (inverseInspectionIsActive)
			{
				this.parHandling.WriteValue(Definition.inverseInspection, flag ? "1" : "0", false);
			}
			this.writeValuesforBgMonitoring();
			flag = false;
			bool anomalyDetectionEnabled = this.dbConnection.GetAnomalyDetectionEnabled(ref flag);
			if (anomalyDetectionEnabled)
			{
				this.parHandling.WriteValue(Definition.anomalyDetectionEnabled, flag ? "1" : "0", false);
			}
			num = 0;
			bool xraySourceComPort = this.dbConnection.GetXraySourceComPort(ref num);
			if (xraySourceComPort)
			{
				this.parHandling.WriteValue(Definition.xraySourcePortCom, num.ToString(), false);
			}
			flag = false;
			bool objectDetectionEnabled = this.dbConnection.GetObjectDetectionEnabled(ref flag);
			if (objectDetectionEnabled)
			{
				this.parHandling.WriteValue(Definition.objectDetectionEnabled, flag ? "1" : "0", false);
			}
			flag = false;
			bool testAtCustomerSite = this.dbConnection.GetTestAtCustomerSite(ref flag);
			if (testAtCustomerSite)
			{
				this.parHandling.WriteValue(Definition.testAtCustomerSite, flag ? "1" : "0", false);
			}
			bool flag18 = machineMode == MachineMode.Bulk;
			if (flag18)
			{
				int num6 = 0;
				int num7 = 0;
				int.TryParse(this.parHandling.ReadPar(Definition.imageVersionElinosPath), out num7);
				bool flag19 = num7 == 30;
				if (flag19)
				{
					BulkMode.NuvoPCPerformance = true;
				}
				else
				{
					BulkMode.NuvoPCPerformance = false;
				}
				this.dbConnection.GetSVTCOMPort(ref num6);
				this.parHandling.WriteValue(SVT.svtCOMPort, num6.ToString());
			}
			this.InitializeValvesPixel();
			this.InitializeCleaningVibratorParameters();
			this.WriteSVTTestParameters();
			flag = false;
			bool objectBasedFlapsEnabled = this.dbConnection.GetObjectBasedFlapsEnabled(ref flag);
			if (objectBasedFlapsEnabled)
			{
				this.parHandling.WriteValue(Definition.objectBasedFlapsEnabledPath, flag ? "1" : "0", false);
			}
			num = 0;
			bool objectBasedFlapsCount = this.dbConnection.GetObjectBasedFlapsCount(ref num);
			if (objectBasedFlapsCount)
			{
				this.parHandling.WriteValue(Definition.objectBasedFlapsCountPath, num.ToString(), false);
			}
		}

		// Token: 0x06002474 RID: 9332 RVA: 0x000F8840 File Offset: 0x000F6A40
		public void WriteSVTTestParameters()
		{
			int num = 0;
			this.dbConnection.GetValveTestStartValve(ref num);
			this.parHandling.WriteValue(SVT.svtTestStartValvePath, num.ToString(), false);
			this.dbConnection.GetValveTestEndValve(ref num);
			this.parHandling.WriteValue(SVT.svtTestEndValvePath, num.ToString(), false);
			this.dbConnection.GetValveTestInterval(ref num);
			this.parHandling.WriteValue(SVT.svtTestIntervalPath, num.ToString(), false);
		}

		// Token: 0x06002475 RID: 9333 RVA: 0x000F88C8 File Offset: 0x000F6AC8
		private void InitializeValvesPixel()
		{
			int num = -1;
			for (int i = 1; i <= Definition.nSVTValves; i++)
			{
				string str = Definition.SVTValvesAssignmentMainPath + i.ToString("D2");
				this.dbConnection.GetSVTMinPixel(i, ref num);
				this.parHandling.WriteValue(str + "/MinPixel/Value", num.ToString(), false);
				this.dbConnection.GetSVTMaxPixel(i, ref num);
				this.parHandling.WriteValue(str + "/MaxPixel/Value", num.ToString(), false);
			}
		}

		// Token: 0x06002476 RID: 9334 RVA: 0x000F896C File Offset: 0x000F6B6C
		private void InitializeCleaningVibratorParameters()
		{
			string text = null;
			this.dbConnection.GetParameterValue(Definition.dbCleaningVibratorModePath, ref text);
			bool flag = text == 1.ToString();
			if (flag)
			{
				this.parHandling.WriteValue(Definition.cleaningVibratorAlwaysOnPath, "1", false);
			}
			else
			{
				this.parHandling.WriteValue(Definition.cleaningVibratorAlwaysOnPath, "0", false);
			}
			bool flag2 = text != 0.ToString();
			if (flag2)
			{
				this.dbConnection.GetParameterValue(Definition.dbCleaningVibratorOnTimePath, ref text);
				this.parHandling.WriteValue(Definition.cleaningVibratorOnTimePath, text, false);
				this.dbConnection.GetParameterValue(Definition.dbCleaningVibratorOffTimePath, ref text);
				this.parHandling.WriteValue(Definition.cleaningVibratorOffTimePath, text, false);
			}
			else
			{
				this.parHandling.WriteValue(Definition.cleaningVibratorOnTimePath, "0", false);
				this.parHandling.WriteValue(Definition.cleaningVibratorOffTimePath, "0", false);
			}
		}

		// Token: 0x06002477 RID: 9335 RVA: 0x000F8A68 File Offset: 0x000F6C68
		private void LoadCurrentApplicationAfterRestore()
		{
			this.errorHandling.UpdateFromDatabase();
			Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
			{
				this.progressRestore = new Progress("Reloading products...", false);
				this.progressRestore.Show();
			}));
			Thread thread = new Thread(new ThreadStart(this.LoadCurrentApplicationWithProgress));
			thread.Start();
		}

		// Token: 0x06002478 RID: 9336 RVA: 0x000F8ABC File Offset: 0x000F6CBC
		private void LoadCurrentApplicationWithProgress()
		{
			this.LoadGlobalValues();
			this.LoadCurrentApplication();
			try
			{
				string s = null;
				string s2 = null;
				this.dbConnection.GetRatedConveyorSpeed(ref s);
				this.conveyor.RatedSpeed = double.Parse(s, new CultureInfo("en-US"));
				this.dbConnection.GetParameterValue("ConveyorSpeed", ref s2);
				this.conveyor.Speed = double.Parse(s2, new CultureInfo("en-US"));
			}
			catch
			{
			}
			this.InitializeWagoIP();
			this.UpdateVisibilityControls();
			this.aiService.CreateDefaultTableAI(true);
			Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
			{
				this.productManager.ReloadProductList();
				this.progressRestore.FormFadeOut.Begin();
			}));
		}

		// Token: 0x06002479 RID: 9337 RVA: 0x000F8B88 File Offset: 0x000F6D88
		public void Close(object obj)
		{
			bool flag = this.xrayState != null;
			if (flag)
			{
				this.xrayState.Stop();
			}
			bool flag2 = this.machineState != null;
			if (flag2)
			{
				this.machineState.AbortLampThread();
			}
			this.imageTransfer.CloseConnection();
			this.imageTransfer.StopConnectionTest();
			this.parHandling.CloseConnection();
			this.modelTransfer.CloseConnection();
			this.modelTransfer.StopConnectionTest();
			bool flag3 = this.errorSurvey != null;
			if (flag3)
			{
				this.errorSurvey.StopSurvey();
				this.errorSurvey.StopCheckingXRayOperatingHours();
			}
			bool flag4 = this.valveActivity != null;
			if (flag4)
			{
				this.valveActivity.StopUpdateActivity();
			}
			bool flag5 = this.rejecterActivity != null;
			if (flag5)
			{
				this.rejecterActivity.StopUpdateActivity();
			}
			bool flag6 = this.sstServer != null;
			if (flag6)
			{
				this.sstServer.StopListening();
			}
			bool flag7 = this.liveCalibration != null;
			if (flag7)
			{
				this.liveCalibration.StopLiveCalibration();
			}
			this.comServer.CloseConnection();
			this.pixelCorrection.StopCorrection();
			this.comServer.StopPingComServer();
			bool flag8 = this.wsFoodServer != null;
			if (flag8)
			{
				this.wsFoodServer.StopListening();
			}
			this.videojetPrinter.Disconnect();
		}

		// Token: 0x0600247A RID: 9338 RVA: 0x000F8CE0 File Offset: 0x000F6EE0
		private void OnPropertyChanged(string propName)
		{
			bool flag = this.PropertyChanged != null;
			if (flag)
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
			}
		}

		// Token: 0x0600247B RID: 9339 RVA: 0x000F8D10 File Offset: 0x000F6F10
		public void SignalActivity()
		{
			this.SignalActivity(null);
		}

		// Token: 0x0600247C RID: 9340 RVA: 0x000F8D1B File Offset: 0x000F6F1B
		private void SignalActivity(object obj)
		{
			MainWindowViewModel.accessRightsManager.SignalActivity();
		}

		// Token: 0x0600247D RID: 9341 RVA: 0x000F8D2C File Offset: 0x000F6F2C
		public void ShowBrandingPopupIfNeeded()
		{
			bool flag = this.selectedBranding.Design == Definition.GUIDesign.None;
			if (flag)
			{
				CtrlBranding usrControl = new CtrlBranding(this.dbConnection, this.selectedBranding, this.parHandling);
				Subwindow.Show(usrControl, SubwindowButtons.Back);
			}
		}

		// Token: 0x0600247E RID: 9342 RVA: 0x000F8D70 File Offset: 0x000F6F70
		private void ChangeTime(object obj)
		{
			bool flag = this.CurrentUser != null && this.CurrentUser.Rights >= BuiltInPriviledges.Admin;
			if (flag)
			{
				this.setFooterMainWindow.SetFooterDeactivation();
				this.imageTransfer.Disconnected -= this.imageTransfer_Disconnected;
				this.parHandling.Disconnected -= this.parHandling_Disconnected;
				SystemTimeManager systemTimeManager = new SystemTimeManager();
				systemTimeManager.ChangeTime(this.setFooterMainWindow);
				System.Threading.Timer timer = new System.Threading.Timer(delegate(object dummy)
				{
					this.imageTransfer.Disconnected += this.imageTransfer_Disconnected;
					this.parHandling.Disconnected += this.parHandling_Disconnected;
				}, null, 10000, -1);
			}
		}

		// Token: 0x0600247F RID: 9343 RVA: 0x000F8E0C File Offset: 0x000F700C
		private void UpdatePaths(string[] paths, string strGroup)
		{
			List<string> list = new List<string>();
			string text = "";
			foreach (string path in paths)
			{
				list.Add(this.parHandling.ReadPar(path));
			}
			this.dbConnection.GetCurrentProductTableName(ref text);
			bool flag = strGroup != "";
			if (flag)
			{
				text = text + Definition.subTableIdentifier.ToString() + strGroup;
			}
			this.dbConnection.UpdateTable(text, paths.ToList<string>(), list);
		}

		// Token: 0x06002480 RID: 9344 RVA: 0x000F8E9C File Offset: 0x000F709C
		private void WriteLastOnTime(object dummy, EventArgs e)
		{
			try
			{
				bool flag = this.setClockIfNeeded;
				if (flag)
				{
					this.SetClockIfNeeded();
					this.setClockIfNeeded = false;
				}
				this.counterWriteLastOnTime++;
				bool flag2 = this.dbConnection != null && this.counterWriteLastOnTime > 1;
				if (flag2)
				{
					this.dbConnection.SetLastOnTime(DateTime.Now);
					this.counterWriteLastOnTime = 0;
				}
			}
			catch (Exception ex)
			{
				Exception ex3;
				Exception ex2 = ex3;
				Exception ex = ex2;
				bool flag3 = Application.Current != null && !Application.Current.Dispatcher.CheckAccess();
				if (flag3)
				{
					Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate()
					{
						Dialog.Show("Error WriteLastOnTime", ex.Message, DialogStyle.Error, DialogButtons.OK);
					}));
				}
				else
				{
					Dialog.Show("Error WriteLastOnTime", ex.Message, DialogStyle.Error, DialogButtons.OK);
				}
			}
		}

		// Token: 0x06002481 RID: 9345 RVA: 0x000F8F88 File Offset: 0x000F7188
		private void ChangeSensitivity(object obj)
		{
			bool flag = this.CurrentUser.Rights >= this.rightSensitivity;
			if (flag)
			{
				this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtSensitivityPercent"), this.bulksensitivity.SensitivityPercent.ToString(), NumKeyboardType.TypeUInt);
				this.numKeyboard.TextInputKeyboard += this.numKeyboard_InputSensitivity;
				this.numKeyboard.ShowDialog();
				this.numKeyboard.TextInputKeyboard -= this.numKeyboard_InputSensitivity;
			}
		}

		// Token: 0x06002482 RID: 9346 RVA: 0x000F9017 File Offset: 0x000F7217
		private void SliderChanged(object obj)
		{
			this.bulksensitivity.calculateBulkSliderContrasts = true;
		}

		// Token: 0x06002483 RID: 9347 RVA: 0x000F9028 File Offset: 0x000F7228
		private void numKeyboard_InputSensitivity(object sender, KeyboardInputEventArgs e)
		{
			int num = 0;
			try
			{
				bool flag = int.TryParse(e.Text, out num);
				if (flag)
				{
					bool flag2 = num >= 1 && num <= 150;
					if (flag2)
					{
						this.bulksensitivity.calculateBulkSliderContrasts = true;
						this.bulksensitivity.SensitivityPercent = num;
						this.timerAddSensitivityChangeToLb.Change(3000, -1);
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x06002484 RID: 9348 RVA: 0x000F90A8 File Offset: 0x000F72A8
		private void AddSensitivityLbEntry(object obj)
		{
			string param = null;
			this.dbConnection.GetParameterValue(FlapsOverIOs.bulkflapsManualSensitvity, ref param);
			string text = this.parHandling.ReadPar(FlapsOverIOs.bulkflapsManualSensitvity);
			this.dbConnection.UpdateCurrentProductTable(FlapsOverIOs.bulkflapsManualSensitvity, text);
			LogbookManager.AddEntry(SSTEntryCode.SensitivityChanged, 0, param, text);
		}

		// Token: 0x06002485 RID: 9349 RVA: 0x000F9100 File Offset: 0x000F7300
		private void ShowValveActivity(object obj)
		{
			CtrlValveActivity ctrlValveActivity = new CtrlValveActivity(this.valveActivity, this.productManager, this.parHandling, this.dbConnection, this.setFooterMainWindow);
			Subwindow.Show(ctrlValveActivity, SubwindowButtons.Back, ctrlValveActivity.PanelSubButtons);
			this.deactivateBulkLiveMode();
		}

		// Token: 0x06002486 RID: 9350 RVA: 0x000F9148 File Offset: 0x000F7348
		private void SetDefaultProductValues(List<string> pathes, List<string> values)
		{
			for (int i = 0; i < pathes.Count; i++)
			{
				bool flag = pathes[i].Contains("BinFullTimeout");
				if (flag)
				{
					values[i] = Definition.defaultBinFullTimeout.ToString();
				}
				bool flag2 = pathes[i].Contains("LightBarrierBlockedTimeout");
				if (flag2)
				{
					values[i] = Definition.defaultLightBarrierBlockedTimeout.ToString();
				}
				bool flag3 = pathes[i].Contains("/detector/ScanRate");
				if (flag3)
				{
					values[i] = Definition.defaultScanrate.ToString();
				}
				bool flag4 = pathes[i] == Definition.msDelayedPusherEnable1Path || pathes[i] == Definition.msDelayedPusherEnable2Path || pathes[i] == Definition.msSensorChecksEnable1Path || pathes[i] == Definition.msSensorChecksEnable2Path;
				if (flag4)
				{
					values[i] = "0";
				}
				bool flag5 = pathes[i] == Definition.msSafetyTimeRejectCheck1Path || pathes[i] == Definition.msSafetyTimeRejectCheck2Path;
				if (flag5)
				{
					values[i] = "400";
				}
				bool flag6 = pathes[i] == Definition.msDebounceTime1Path || pathes[i] == Definition.msDebounceTime2Path;
				if (flag6)
				{
					values[i] = "200";
				}
			}
		}

		// Token: 0x06002487 RID: 9351 RVA: 0x000F92CC File Offset: 0x000F74CC
		public bool TryLoginToGo()
		{
			string text = "E:";
			bool result = false;
			User user = new User();
			bool flag = !this.loginToGo.CanCreateSesotecService;
			if (flag)
			{
				this.dbConnection.GetUsbDriveLetter(ref text);
				text += "\\";
				string[] array = new string[]
				{
					text,
					text + LoginToGo.geniusFolderName + "\\"
				};
				for (int i = 0; i < array.Length; i++)
				{
					bool userNameAndID = this.loginToGo.GetUserNameAndID(array[i], ref user);
					if (userNameAndID)
					{
						bool flag2 = this.dbConnection.UserExists(user, true);
						if (flag2)
						{
							bool flag3 = MainWindowViewModel.accessRightsManager.CurrentUser.Rights == BuiltInPriviledges.NoUser;
							if (flag3)
							{
								LogbookManager.AddEntry(SSTEntryCode.UserLoggedIn, 0, user.Username);
							}
							else
							{
								LogbookManager.AddEntry(SSTEntryCode.UserChanged, 0, MainWindowViewModel.accessRightsManager.CurrentUser.Username, user.Username, "", "");
							}
							MainWindowViewModel.accessRightsManager.CurrentUser.Deletable = user.Deletable;
							MainWindowViewModel.accessRightsManager.CurrentUser.ID = user.ID;
							MainWindowViewModel.accessRightsManager.CurrentUser.PasswordAsMD5 = user.PasswordAsMD5;
							MainWindowViewModel.accessRightsManager.CurrentUser.Rights = user.Rights;
							MainWindowViewModel.accessRightsManager.CurrentUser.Username = user.Username;
							MainWindowViewModel.accessRightsManager.CurrentUser.CommitErrors = user.CommitErrors;
							MainWindowViewModel.accessRightsManager.SetProperties();
							result = true;
							break;
						}
					}
				}
			}
			else
			{
				bool flag4 = !this.loginToGo.stickInProgress;
				if (flag4)
				{
					int num = 0;
					while (num < 10 && !Directory.Exists(text))
					{
						Thread.Sleep(100);
						num++;
					}
					bool flag5 = Directory.Exists(text);
					if (flag5)
					{
						this.loginToGo.NotifyNewStick();
					}
				}
			}
			this.loggedInWithToGoFeature = result;
			return result;
		}

		// Token: 0x06002488 RID: 9352 RVA: 0x000F94E4 File Offset: 0x000F76E4
		public void LogoutToGoUser()
		{
			bool flag = this.loggedInWithToGoFeature;
			if (flag)
			{
				MainWindowViewModel.accessRightsManager.LogoutUser();
				this.loggedInWithToGoFeature = false;
			}
		}

		// Token: 0x06002489 RID: 9353 RVA: 0x000F9510 File Offset: 0x000F7710
		private void SetImageRegions(XRSImage image)
		{
			short objectCount = 0;
			double num = 0.0;
			for (int i = 0; i < this.productManager.CurrentLoadedProduct.SubProducts.Count; i++)
			{
				int group = (int)this.productManager.CurrentLoadedProduct.SubProducts[i].Group;
				bool flag = group > 0 && group <= image.Regions.Length;
				if (flag)
				{
					image.Regions[group - 1].Vertex1 = this.productManager.CurrentLoadedProduct.SubProducts[i].Region.Vertex1;
					image.Regions[group - 1].Vertex2 = this.productManager.CurrentLoadedProduct.SubProducts[i].Region.Vertex2;
					image.Regions[group - 1].Vertex3 = this.productManager.CurrentLoadedProduct.SubProducts[i].Region.Vertex3;
					image.Regions[group - 1].Vertex4 = this.productManager.CurrentLoadedProduct.SubProducts[i].Region.Vertex4;
					image.Regions[group - 1].Vertex5 = this.productManager.CurrentLoadedProduct.SubProducts[i].Region.Vertex5;
					image.Regions[group - 1].Vertex6 = this.productManager.CurrentLoadedProduct.SubProducts[i].Region.Vertex6;
					image.Regions[group - 1].Vertex7 = this.productManager.CurrentLoadedProduct.SubProducts[i].Region.Vertex7;
					image.Regions[group - 1].Vertex8 = this.productManager.CurrentLoadedProduct.SubProducts[i].Region.Vertex8;
					image.Regions[group - 1].Vertex9 = this.productManager.CurrentLoadedProduct.SubProducts[i].Region.Vertex9;
					image.Regions[group - 1].Vertex10 = this.productManager.CurrentLoadedProduct.SubProducts[i].Region.Vertex10;
					bool flag2 = this.parHandling.ReadPar(Definition.filterMainPath + group.ToString() + ObjectCounter.enabledSubPath) == "1";
					if (flag2)
					{
						string s = this.parHandling.ReadPar(Definition.filterMainPath + group.ToString() + ObjectCounter.currentCountSubPath);
						bool flag3 = short.TryParse(s, out objectCount);
						if (flag3)
						{
							image.Regions[group - 1].ObjectCount = objectCount;
						}
					}
					bool flag4 = this.parHandling.ReadPar(Definition.filterMainPath + group.ToString() + Weigher.enableWeightSubPath) == "1";
					if (flag4)
					{
						image.Regions[group - 1].Weight = (float)this.GetCurrentWeight((int)this.productManager.CurrentLoadedProduct.SubProducts[i].Group, ref num);
					}
					image.Regions[group - 1].Name = this.productManager.CurrentLoadedProduct.SubProducts[i].ProductName;
					image.Regions[group - 1].Type = this.productManager.CurrentLoadedProduct.SubProducts[i].Region.Type;
					bool flag5 = this.parHandling.ReadPar(Definition.filterMainPath + group.ToString() + Definition.regionTriggeredSubPath) == "1";
					if (flag5)
					{
						image.SetRegionRejected(group);
					}
					bool flag6 = this.parHandling.ReadPar(Definition.filterMainPath + group.ToString() + ObjectCounter.triggeredSubPath) == "1";
					if (flag6)
					{
						image.SetRegionHadFalseCount(group);
					}
					bool flag7 = this.parHandling.ReadPar(Definition.filterMainPath + group.ToString() + Weigher.triggeredSubPath) == "1";
					if (flag7)
					{
						image.SetRegionHadFalseWeight(group);
					}
				}
			}
		}

		// Token: 0x0600248A RID: 9354 RVA: 0x000F996C File Offset: 0x000F7B6C
		private void UpdateRegions(string tableName, string path, string strValue)
		{
			bool flag = this.productManager != null && this.productManager.CurrentLoadedProduct != null && this.productManager.CurrentLoadedProduct.IsRegionProduct;
			if (flag)
			{
				bool flag2 = path.EndsWith(Definition.vertex1XSubPath) || path.EndsWith(Definition.vertex1YSubPath) || path.EndsWith(Definition.vertex2XSubPath) || path.EndsWith(Definition.vertex2YSubPath) || path.EndsWith(Definition.vertex3XSubPath) || path.EndsWith(Definition.vertex3YSubPath) || path.EndsWith(Definition.vertex4XSubPath) || path.EndsWith(Definition.vertex4YSubPath) || path.EndsWith(Definition.vertex5XSubPath) || path.EndsWith(Definition.vertex5YSubPath) || path.EndsWith(Definition.vertex6XSubPath) || path.EndsWith(Definition.vertex6YSubPath) || path.EndsWith(Definition.vertex7XSubPath) || path.EndsWith(Definition.vertex7YSubPath) || path.EndsWith(Definition.vertex8XSubPath) || path.EndsWith(Definition.vertex8YSubPath) || path.EndsWith(Definition.vertex9XSubPath) || path.EndsWith(Definition.vertex9YSubPath) || path.EndsWith(Definition.vertex10XSubPath) || path.EndsWith(Definition.vertex10YSubPath) || path.EndsWith(Definition.regionTypeSubPath);
				if (flag2)
				{
					List<ProductData> list = (from subProduct in this.productManager.CurrentLoadedProduct.SubProducts
					where subProduct.TableName == tableName
					select subProduct).ToList<ProductData>();
					bool flag3 = list.Count > 0;
					if (flag3)
					{
						int num = 0;
						bool flag4 = int.TryParse(strValue, out num);
						if (flag4)
						{
							bool flag5 = path.EndsWith(Definition.vertex1XSubPath);
							if (flag5)
							{
								list[0].Region.Vertex1 = new System.Windows.Point((double)num, list[0].Region.Vertex1.Y);
							}
							bool flag6 = path.EndsWith(Definition.vertex1YSubPath);
							if (flag6)
							{
								list[0].Region.Vertex1 = new System.Windows.Point(list[0].Region.Vertex1.X, (double)num);
							}
							bool flag7 = path.EndsWith(Definition.vertex2XSubPath);
							if (flag7)
							{
								list[0].Region.Vertex2 = new System.Windows.Point((double)num, list[0].Region.Vertex2.Y);
							}
							bool flag8 = path.EndsWith(Definition.vertex2YSubPath);
							if (flag8)
							{
								list[0].Region.Vertex2 = new System.Windows.Point(list[0].Region.Vertex2.X, (double)num);
							}
							bool flag9 = path.EndsWith(Definition.vertex3XSubPath);
							if (flag9)
							{
								list[0].Region.Vertex3 = new System.Windows.Point((double)num, list[0].Region.Vertex3.Y);
							}
							bool flag10 = path.EndsWith(Definition.vertex3YSubPath);
							if (flag10)
							{
								list[0].Region.Vertex3 = new System.Windows.Point(list[0].Region.Vertex3.X, (double)num);
							}
							bool flag11 = path.EndsWith(Definition.vertex4XSubPath);
							if (flag11)
							{
								list[0].Region.Vertex4 = new System.Windows.Point((double)num, list[0].Region.Vertex4.Y);
							}
							bool flag12 = path.EndsWith(Definition.vertex4YSubPath);
							if (flag12)
							{
								list[0].Region.Vertex4 = new System.Windows.Point(list[0].Region.Vertex4.X, (double)num);
							}
							bool flag13 = path.EndsWith(Definition.vertex5XSubPath);
							if (flag13)
							{
								list[0].Region.Vertex5 = new System.Windows.Point((double)num, list[0].Region.Vertex5.Y);
							}
							bool flag14 = path.EndsWith(Definition.vertex5YSubPath);
							if (flag14)
							{
								list[0].Region.Vertex5 = new System.Windows.Point(list[0].Region.Vertex5.X, (double)num);
							}
							bool flag15 = path.EndsWith(Definition.vertex6XSubPath);
							if (flag15)
							{
								list[0].Region.Vertex6 = new System.Windows.Point((double)num, list[0].Region.Vertex6.Y);
							}
							bool flag16 = path.EndsWith(Definition.vertex6YSubPath);
							if (flag16)
							{
								list[0].Region.Vertex6 = new System.Windows.Point(list[0].Region.Vertex6.X, (double)num);
							}
							bool flag17 = path.EndsWith(Definition.vertex7XSubPath);
							if (flag17)
							{
								list[0].Region.Vertex7 = new System.Windows.Point((double)num, list[0].Region.Vertex7.Y);
							}
							bool flag18 = path.EndsWith(Definition.vertex7YSubPath);
							if (flag18)
							{
								list[0].Region.Vertex7 = new System.Windows.Point(list[0].Region.Vertex7.X, (double)num);
							}
							bool flag19 = path.EndsWith(Definition.vertex8XSubPath);
							if (flag19)
							{
								list[0].Region.Vertex8 = new System.Windows.Point((double)num, list[0].Region.Vertex8.Y);
							}
							bool flag20 = path.EndsWith(Definition.vertex8YSubPath);
							if (flag20)
							{
								list[0].Region.Vertex8 = new System.Windows.Point(list[0].Region.Vertex8.X, (double)num);
							}
							bool flag21 = path.EndsWith(Definition.vertex9XSubPath);
							if (flag21)
							{
								list[0].Region.Vertex9 = new System.Windows.Point((double)num, list[0].Region.Vertex9.Y);
							}
							bool flag22 = path.EndsWith(Definition.vertex9YSubPath);
							if (flag22)
							{
								list[0].Region.Vertex9 = new System.Windows.Point(list[0].Region.Vertex9.X, (double)num);
							}
							bool flag23 = path.EndsWith(Definition.vertex10XSubPath);
							if (flag23)
							{
								list[0].Region.Vertex10 = new System.Windows.Point((double)num, list[0].Region.Vertex10.Y);
							}
							bool flag24 = path.EndsWith(Definition.vertex10YSubPath);
							if (flag24)
							{
								list[0].Region.Vertex10 = new System.Windows.Point(list[0].Region.Vertex10.X, (double)num);
							}
							bool flag25 = path.EndsWith(Definition.regionTypeSubPath);
							if (flag25)
							{
								list[0].Region.Type = (XRSRegionType)num;
							}
						}
					}
				}
			}
		}

		// Token: 0x0600248B RID: 9355 RVA: 0x000FA0FC File Offset: 0x000F82FC
		private void ViewRegionImage(LaneXri laneXri)
		{
			bool flag = laneXri != null && this.lastPaintedImage != null;
			if (flag)
			{
				bool flag2 = laneXri.Group == 0;
				if (flag2)
				{
					bool flag3 = this.resetImages;
					if (flag3)
					{
						laneXri.resetImagesTemp();
						this.resetImages = false;
					}
					laneXri.ViewImage(this.lastPaintedImage, this.CurrentUser.Rights > BuiltInPriviledges.NoUser, !this.StandardMode, this.isBulkLiveImage);
				}
				else
				{
					XRSImage xrsimage = this.lastPaintedImage.DeepCopy(true);
					xrsimage.Group = (short)laneXri.Group;
					bool flag4 = this.resetImages;
					if (flag4)
					{
						laneXri.resetImagesTemp();
						this.resetImages = false;
					}
					laneXri.ViewImage(xrsimage, this.CurrentUser.Rights > BuiltInPriviledges.NoUser, !this.StandardMode, this.isBulkLiveImage);
				}
			}
		}

		// Token: 0x0600248C RID: 9356 RVA: 0x000FA1D8 File Offset: 0x000F83D8
		public void ChangeSpeed(object obj)
		{
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtVibroSpeed"), this.Vibro.Speed.ToString(), NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_InputSpeed;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x0600248D RID: 9357 RVA: 0x000FA234 File Offset: 0x000F8434
		private void numKeyboard_InputSpeed(object sender, KeyboardInputEventArgs e)
		{
			int speed = 0;
			try
			{
				this.numKeyboard.TextInputKeyboard -= this.numKeyboard_InputSpeed;
				bool flag = int.TryParse(e.Text, out speed);
				if (flag)
				{
					this.Vibro.Speed = speed;
				}
			}
			catch
			{
			}
		}

		// Token: 0x0600248E RID: 9358 RVA: 0x000FA294 File Offset: 0x000F8494
		private void LowerSpeed(object obj)
		{
			this.Vibro.Speed -= Definition.vibroSpeedStep;
		}

		// Token: 0x0600248F RID: 9359 RVA: 0x000FA2AF File Offset: 0x000F84AF
		private void HigherSpeed(object obj)
		{
			this.Vibro.Speed += Definition.vibroSpeedStep;
		}

		// Token: 0x06002490 RID: 9360 RVA: 0x000FA2CC File Offset: 0x000F84CC
		private void WSFoodServer_WSFrameReceived(WSTag tag, object argument, bool sValue)
		{
			WSTag wstag = tag;
			WSTag wstag2 = wstag;
			if (wstag2 <= WSTag.WS_S88_STATE)
			{
				if (wstag2 <= WSTag.SS_CURRENT_PROD_NBR)
				{
					if (wstag2 - WSTag.WS_SET_BATCH_ID <= 3)
					{
						return;
					}
					if (wstag2 == WSTag.WS_SYS_TIME_LOC)
					{
						uint timestamp = (uint)argument;
						SystemTime.SetTime(UnixTime.ToDateTime(timestamp));
						return;
					}
					if (wstag2 == WSTag.SS_CURRENT_PROD_NBR)
					{
						bool flag = this.productManager != null && this.productManager.CurrentLoadedProduct != null;
						if (flag)
						{
							uint num = 0U;
							uint.TryParse(this.productManager.CurrentLoadedProduct.ProductNumber, out num);
							this.wsFoodServer.AnswerSValue = (long)((ulong)num);
						}
						return;
					}
				}
				else
				{
					if (wstag2 == WSTag.SS_CURRENT_PROD_NAME)
					{
						bool flag2 = this.productManager != null && this.productManager.CurrentLoadedProduct != null;
						if (flag2)
						{
							this.wsFoodServer.AnswerSValue = (long)((ulong)((this.productManager.CurrentLoadedProduct.ProductName.Length > 0) ? this.productManager.CurrentLoadedProduct.ProductName[0] : '\0'));
							this.wsFoodServer.AnswerString = this.productManager.CurrentLoadedProduct.ProductName;
						}
						return;
					}
					if (wstag2 == WSTag.WS_CUR_MODE)
					{
						return;
					}
					if (wstag2 == WSTag.WS_S88_STATE)
					{
						bool flag3 = this.machineState != null;
						if (flag3)
						{
							bool flag4 = this.machineState.State == MachineStates.ConveyorOff;
							if (flag4)
							{
								this.wsFoodServer.AnswerSValue = 1L;
							}
							else
							{
								bool flag5 = this.machineState.State == MachineStates.Inspecting;
								if (flag5)
								{
									this.wsFoodServer.AnswerSValue = 128L;
								}
								else
								{
									this.wsFoodServer.AnswerSValue = 4L;
								}
							}
						}
						return;
					}
				}
			}
			else if (wstag2 <= WSTag.SS_METAL_SIGNAL)
			{
				if (wstag2 == WSTag.WS_MACH_DESIGN_SPD)
				{
					bool flag6 = this.conveyor != null;
					if (flag6)
					{
						this.wsFoodServer.AnswerSValue = (long)BitConverter.ToInt32(BitConverter.GetBytes((float)this.conveyor.Speed), 0);
					}
					return;
				}
				if (wstag2 == WSTag.WS_L_TEST_RESULT)
				{
					AuditResult auditResult = AuditResult.Passed;
					this.dbConnection.GetLastAuditCheckResult(ref auditResult);
					this.wsFoodServer.AnswerSValue = (long)auditResult;
					return;
				}
				if (wstag2 == WSTag.SS_METAL_SIGNAL)
				{
					return;
				}
			}
			else if (wstag2 <= WSTag.WS_TOT_EJECTION_DUR)
			{
				switch (wstag2)
				{
				case WSTag.WS_TOT_DET_F_BODIES:
					this.wsFoodServer.AnswerSValue = (long)((ulong)this.rejectGlobal);
					return;
				case (WSTag)50245:
					break;
				case WSTag.WS_TOT_FUNC_TESTS:
				{
					int num2 = 0;
					int num3 = 0;
					this.dbConnection.GetNAuditChecksPassed(ref num2);
					this.dbConnection.GetNAuditChecksFailed(ref num3);
					this.wsFoodServer.AnswerSValue = (long)(num2 + num3);
					return;
				}
				case WSTag.WS_TOT_FUNC_TESTS_OK:
				{
					int num2 = 0;
					this.dbConnection.GetNAuditChecksPassed(ref num2);
					this.wsFoodServer.AnswerSValue = (long)num2;
					return;
				}
				case WSTag.WS_TOT_FUNC_TESTS_F:
				{
					int num3 = 0;
					this.dbConnection.GetNAuditChecksFailed(ref num3);
					this.wsFoodServer.AnswerSValue = (long)num3;
					return;
				}
				default:
					if (wstag2 == WSTag.WS_TOT_EJECTION_DUR)
					{
						string s = null;
						int num4 = 0;
						this.dbConnection.GetParameterValue("/hardware/rejecter/rejecter1/Length/Value", ref s);
						int.TryParse(s, out num4);
						this.wsFoodServer.AnswerSValue = (long)num4;
						return;
					}
					break;
				}
			}
			else
			{
				if (wstag2 == WSTag.WS_TOT_MACH_FAIL)
				{
					uint num5 = 0U;
					this.dbConnection.GetNErrors(ref num5);
					this.wsFoodServer.AnswerSValue = (long)((ulong)num5);
					return;
				}
				if (wstag2 == WSTag.SS_PRODUCT_COUNTER)
				{
					this.wsFoodServer.AnswerSValue = (long)((ulong)this.throughputGlobal);
					return;
				}
			}
			this.wsFoodServer.AnswerSValue = (long)((ulong)tag);
			tag = WSTag.WS_ERROR;
		}

		// Token: 0x06002491 RID: 9361 RVA: 0x000FA69C File Offset: 0x000F889C
		private void dbConnection_BatchEnded(Batch batch, string oldProductTableName)
		{
			string param = "";
			string param2 = "";
			this.dbConnection.GetProductName(oldProductTableName, ref param);
			this.dbConnection.GetProductNumber(oldProductTableName, ref param2);
			LogbookManager.AddEntry(SSTEntryCode.BatchEnded, 0, param2, param, batch.NProducts.ToString(), batch.NRejects.ToString(), batch.BatchID);
			this.AddWeightStatLogbookEntry(oldProductTableName, batch);
			this.ResetAllCounters();
		}

		// Token: 0x06002492 RID: 9362 RVA: 0x000FA714 File Offset: 0x000F8914
		private void AddWeightStatLogbookEntry(string oldProductTableName)
		{
			this.AddWeightStatLogbookEntry(oldProductTableName, null);
		}

		// Token: 0x06002493 RID: 9363 RVA: 0x000FA720 File Offset: 0x000F8920
		private void AddWeightStatLogbookEntry(string oldProductTableName, Batch batch)
		{
			string a = "";
			string str = (this.productManager.CurrentLoadedProduct.Group > 0) ? this.productManager.CurrentLoadedProduct.Group.ToString() : "";
			this.dbConnection.GetParameterValue(Definition.filterMainPath + str + Weigher.enableWeightSubPath, oldProductTableName, ref a);
			bool flag = a == "1";
			if (flag)
			{
				bool flag2 = false;
				this.dbConnection.GetStatisticsEnabled(ref flag2);
				bool flag3 = flag2;
				if (flag3)
				{
					List<Statistic> list = new List<Statistic>();
					double num = 0.0;
					double num2 = 0.0;
					double num3 = 0.0;
					double num4 = 0.0;
					this.dbConnection.GetStatistics(oldProductTableName, list, batch);
					for (int i = 0; i < list.Count; i++)
					{
						num = list[i].ReferenceWeight;
						num2 += list[i].Weight;
					}
					bool flag4 = num2 > 0.0 && list.Count > 0;
					if (flag4)
					{
						num3 = num2 / (double)list.Count;
					}
					for (int j = 0; j < list.Count; j++)
					{
						num4 = Math.Pow(list[j].Weight - num3, 2.0);
					}
					bool flag5 = list.Count > 0;
					if (flag5)
					{
						num4 /= (double)list.Count;
					}
					num4 = Math.Sqrt(num4);
					LogbookManager.AddEntry(SSTEntryCode.WeightStat, (batch == null) ? 0 : 1, (num * 1000.0).ToString(), (num2 * 1000.0).ToString(), (num3 * 1000.0).ToString(), (num4 * 1000.0).ToString());
				}
			}
		}

		// Token: 0x06002494 RID: 9364 RVA: 0x000FA934 File Offset: 0x000F8B34
		private void dbConnection_BatchStarted(Batch batch, string oldBatchID)
		{
			LogbookManager.AddEntry(SSTEntryCode.BatchChanged, 0, oldBatchID, batch.BatchID);
			this.SetInitialBatchCounter(batch);
		}

		// Token: 0x06002495 RID: 9365 RVA: 0x000FA954 File Offset: 0x000F8B54
		private void SetInitialBatchCounter(Batch batch)
		{
			bool flag = false;
			this.dbConnection.GetBatchesEnabled(ref flag);
			bool flag2 = batch != null && flag && this.AllLanes != null;
			if (flag2)
			{
				List<LaneXri> list = (from lane in this.AllLanes
				where lane.Lane == Lane.All
				select lane).ToList<LaneXri>();
				bool flag3 = list.Count > 0;
				if (flag3)
				{
					list[0].Counter.ThroughputCounter = (uint)batch.NProducts;
					list[0].Counter.RejectCounter = (uint)batch.NRejects;
				}
			}
		}

		// Token: 0x06002496 RID: 9366 RVA: 0x000FA9FC File Offset: 0x000F8BFC
		private void StartNewBatch(object obj)
		{
			bool flag = false;
			this.dbConnection.GetBatchesEnabled(ref flag);
			bool flag2 = flag;
			if (flag2)
			{
				this.keyboard = new Keyboard(LocalizationManager.Localize("txtNewBatch"), "", KeyboardType.Application);
				this.keyboard.TextInputKeyboard += this.keyboard_CreateBatch;
				this.keyboard.ShowDialog();
				this.keyboard.TextInputKeyboard -= this.keyboard_CreateBatch;
			}
		}

		// Token: 0x06002497 RID: 9367 RVA: 0x000FAA78 File Offset: 0x000F8C78
		private void keyboard_CreateBatch(object sender, KeyboardInputEventArgs e)
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
				bool flag3 = !this.dbConnection.CreateNewBatch(e.Text, this.ProductManager.SelectedProduct.TableName);
				if (flag3)
				{
					Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtBatchIDExists"), DialogStyle.Error, DialogButtons.OK);
					this.keyboard.ShouldClose = false;
				}
				else
				{
					List<Batch> list = new List<Batch>();
					this.dbConnection.GetAllBatches(list, this.productManager.CurrentLoadedProduct.TableName);
					List<Batch> list2 = (from batch in list
					where batch.BatchID == e.Text
					select batch).ToList<Batch>();
					bool flag4 = list2.Count > 0;
					if (flag4)
					{
						bool flag5 = Dialog.Show(LocalizationManager.Localize("txtWarning"), string.Concat(new string[]
						{
							"\"",
							list2[0].BatchID,
							"\" ",
							LocalizationManager.Localize("txtWillBeStarted"),
							" ",
							LocalizationManager.Localize("txtContinue")
						}), DialogStyle.Warning, DialogButtons.OKCancel) == Dialog.DialogResults.Ok;
						if (flag5)
						{
							string text = null;
							this.dbConnection.GetCurrentBatchID(ref text, this.productManager.SelectedProduct.TableName);
							bool flag6 = text != null && text.Length > 0;
							if (flag6)
							{
								this.dbConnection.EndBatch(text, this.productManager.SelectedProduct.TableName);
							}
							this.dbConnection.StartBatch(list2[0].BatchID, this.productManager.CurrentLoadedProduct.TableName, text);
							this.productManager.CurrentLoadedProduct.CurrentBatchName = list2[0].BatchID;
							this.keyboard.TextInputKeyboard -= this.keyboard_CreateBatch;
						}
						else
						{
							this.dbConnection.DeleteBatch(this.productManager.CurrentLoadedProduct.TableName, list2[0].BatchID);
							this.keyboard.ShouldClose = false;
						}
					}
				}
			}
		}

		// Token: 0x06002498 RID: 9368 RVA: 0x000FAD08 File Offset: 0x000F8F08
		private void UnlockBin(object obj)
		{
			int binTimeout = 0;
			this.dbConnection.GetMSBinTimeout(true, ref binTimeout);
			string a = this.parHandling.ReadPar(Rejecter.rejecter2MainPath + "/Enabled/Value");
			bool flag = a == "0";
			if (flag)
			{
				Rejecter.UnlockBin(Rejecter.rejecter1MainPath, this.parHandling, binTimeout);
				this.ContextUnlockBinVisibility = false;
			}
			else
			{
				this.ContextUnlockBinVisibility = true;
			}
		}

		// Token: 0x06002499 RID: 9369 RVA: 0x000FAD80 File Offset: 0x000F8F80
		private void DoBackup(object obj)
		{
			BackupOptionsViewModel backupOptionsViewModel = new BackupOptionsViewModel(this.dbConnection, this.backup, this.parHandling, this.productManager, true);
			this.backup.SaveSystem = true;
			this.backup.DoBackup(null);
		}

		// Token: 0x0600249A RID: 9370 RVA: 0x000FADCC File Offset: 0x000F8FCC
		private void SetClockIfNeeded()
		{
			DateTime t = default(DateTime);
			this.dbConnection.GetLastOnTime(ref t);
			bool flag = DateTime.Now.AddDays(1.0) < t;
			if (flag)
			{
				SystemTimeManager systemTimeManager = new SystemTimeManager();
				systemTimeManager.ChangeTime(this.setFooterMainWindow);
			}
		}

		// Token: 0x0600249B RID: 9371 RVA: 0x000FAE28 File Offset: 0x000F9028
		private void SetIPCTimeIfNeeded()
		{
			string s = this.parHandling.ReadPar(Definition.timeIPCPath);
			uint num = 0U;
			uint num2 = UnixTime.ToUnixTime(DateTime.Now);
			bool flag = uint.TryParse(s, out num);
			if (flag)
			{
				bool flag2 = num2 > 86400U + num;
				if (flag2)
				{
					this.parHandling.WriteValue(Definition.timeIPCPath, num2.ToString());
				}
			}
		}

		// Token: 0x0600249C RID: 9372 RVA: 0x000FAE8C File Offset: 0x000F908C
		private void StartXray(object obj)
		{
			string text = "";
			string text2 = "";
			this.dbDatalogs.MachineInfosGetSerialNumbers(ref text2, ref text);
			this.meanPixelLogger.SignalMonitoringSave = false;
			DetectorType detectorType = DetectorType.Unknown;
			this.dbConnection.GetDetectorType(ref detectorType);
			bool flag = detectorType == DetectorType.DTNetTDI;
			if (flag)
			{
				this.conveyor.AdaptScanrate();
				XRayOnProcessing.AdaptedIntegrationTime = this.conveyor.AdaptedIntegrationTime;
			}
			bool flag2 = this.machineState.State != MachineStates.ViewingPlot && !this.PlotShown;
			if (flag2)
			{
				bool flag3 = !this.dbDatalogs.SignalTablesExist(text.Replace("_", "").Replace(":", ""));
				if (flag3)
				{
					bool flag4 = Application.Current != null;
					if (flag4)
					{
						Application.Current.Dispatcher.Invoke(delegate()
						{
							bool flag10 = Dialog.Show(LocalizationManager.Localize("txtInfo"), LocalizationManager.Localize("txtDetectorReady"), DialogStyle.Info, DialogButtons.OKCancel) == Dialog.DialogResults.Ok;
							if (flag10)
							{
								this.meanPixelLogger.SignalMonitoringSave = true;
							}
						});
					}
				}
				else
				{
					this.meanPixelLogger.SignalMonitoringSave = true;
				}
			}
			bool flag5 = this.machineState.State == MachineStates.ConveyorOnKeyClosedXrayOff;
			if (flag5)
			{
				bool flag6 = this.parHandling.ReadPar(Definition.wagoMachineTypePath) == Definition.ioModuleIsRayconDPlus1Module || this.parHandling.ReadPar(Definition.wagoMachineTypePath) == Definition.ioModuleIsRayconDPlus1ModuleHYRework;
				this.parHandling.WriteValue(XRayOnProcessing.manualOffsetCalibrationPath, "0", false);
				this.parHandling.WriteValue(Acquisition.acquisitionModePath, "0", false);
				bool flag7 = this.Machine.MachineType == MachineType.RayconDPlus && flag6;
				if (flag7)
				{
					this.parHandling.WriteValue(XrayState.xrayStarterDPlus1ModulePath, "1", false);
				}
				else
				{
					this.parHandling.WriteValue(XrayState.xrayStarterEntryAndDPlusPath, "1", false);
				}
				bool flag8 = this.Machine.MachineType == MachineType.RayconEX2A;
				if (flag8)
				{
					Thread.Sleep(1000);
					bool flag9 = this.parHandling.ReadPar(XrayState.xrayStarterPath) == "0";
					if (flag9)
					{
						this.errorSurvey.PostMessage(SSTEntryCode.SafetyCircutEx2A);
					}
				}
			}
		}

		// Token: 0x0600249D RID: 9373 RVA: 0x000FB0B0 File Offset: 0x000F92B0
		private void SaveMaterialCurve(XRSImage image)
		{
			bool flag = image != null && image.Data != null && image.Data.Length != 0 && image.Data.Length % 3 == 0 && this.productManager != null && this.productManager.CurrentLoadedProduct != null;
			if (flag)
			{
				List<MaterialCurveElement> list = new List<MaterialCurveElement>();
				for (int i = DualEnergy.lowLimitMaterialCurveDB * 3; i < DualEnergy.highLimitMaterialCurveDB * 3; i += 3)
				{
					list.Add(new MaterialCurveElement((int)image.Data[i], (int)image.Data[i + 1], (int)image.Data[i + 2]));
				}
				bool flag2 = image.Group == 0 && this.productManager.CurrentLoadedProduct.IsSingleProduct;
				if (flag2)
				{
					this.dbConnection.SetMaterialCurve(this.productManager.CurrentLoadedProduct.TableName, list);
				}
				bool flag3 = image.Group != 0;
				if (flag3)
				{
					this.dbConnection.SetMaterialCurve(this.productManager.CurrentLoadedProduct.TableName + "_" + image.Group.ToString(), list);
				}
			}
		}

		// Token: 0x0600249E RID: 9374 RVA: 0x000FB1D0 File Offset: 0x000F93D0
		private void LoadCurrentMaterialCurve(int group)
		{
			string text = (group == 0) ? "" : group.ToString();
			string tableName = (group == 0) ? this.productManager.CurrentLoadedProduct.TableName : (this.productManager.CurrentLoadedProduct.TableName + Definition.subTableIdentifier.ToString() + text);
			string a = "";
			string str = (group == 0) ? "" : ("_" + group.ToString());
			this.dbConnection.GetParameterValue(Definition.filterMainPath + text + DualEnergy.enableSubPath, this.productManager.CurrentLoadedProduct.TableName + str, ref a);
			bool flag = a == "1" && this.dbConnection.CheckIfMaterialCurveExists(tableName);
			if (flag)
			{
				DualEnergy dualEnergy = new DualEnergy(this.imageTransfer, this.parHandling);
				List<MaterialCurveElement> list = new List<MaterialCurveElement>();
				this.dbConnection.GetMaterialCurve(tableName, list);
				dualEnergy.SendMaterialCurveImage(list, group);
			}
		}

		// Token: 0x0600249F RID: 9375 RVA: 0x000FB2D8 File Offset: 0x000F94D8
		private void messageSystem_ResetAllErrorsFromList()
		{
			foreach (Message message in this.ErrorMessages)
			{
				bool flag = message.LogbookEntry.Type == MessageType.Error;
				if (flag)
				{
					this.messageSystem_MessageRemoved(message);
				}
			}
		}

		// Token: 0x060024A0 RID: 9376 RVA: 0x000FB340 File Offset: 0x000F9540
		private void ResetMessages(object obj)
		{
			this.messageSystem_ResetAllErrorsFromList();
			this.messageSystem.RemoveAllMessages();
		}

		// Token: 0x060024A1 RID: 9377 RVA: 0x000FB356 File Offset: 0x000F9556
		private void BulkDeactivateLiveMode(object obj)
		{
			this.deactivateBulkLiveMode();
		}

		// Token: 0x060024A2 RID: 9378 RVA: 0x000FB360 File Offset: 0x000F9560
		private void deactivateBulkLiveMode()
		{
			bool flag = this.parHandling != null && this.BulkModeActive && this.machineState.State == MachineStates.Inspecting;
			if (flag)
			{
				this.parHandling.WriteValue("/filter/acquisition/SendImage/Value", "0", false);
				this.parHandling.WriteValue("/filter/acquisition/ContinuousMode/Value", "1");
				bool flag2 = this.parHandling.ReadPar(Definition.bulkShowContImages) == "1";
				if (flag2)
				{
					this.parHandling.WriteValue("/filter/acquisition/SendOnlyIfContaminant/Value", "1");
				}
				else
				{
					this.parHandling.WriteValue("/filter/acquisition/SendOnlyIfContaminant/Value", "0");
				}
				this.BulkLiveModeSourcePath = "..\\img\\xriimageBulk.png";
				bool flag3 = this.BulkContImagesShown;
				if (flag3)
				{
					this.BulkObjectModeSourcePath = "..\\img\\xriimageBulkCont.png";
				}
				else
				{
					this.BulkObjectModeSourcePath = "..\\img\\xriimageBulkJustCont.png";
				}
			}
		}

		// Token: 0x060024A3 RID: 9379 RVA: 0x000FB444 File Offset: 0x000F9644
		private void InitializeVideojetPrinter()
		{
			bool available = false;
			bool enabled = false;
			string printerIP = "";
			int port = 0;
			bool stopConveyorOnError = false;
			int timeout = 0;
			this.dbConnection.GetVideojetPrinterAvailable(ref available);
			this.dbConnection.GetVideojetPrinterEnabled(ref enabled);
			this.dbConnection.GetVideojetPrinterIP(ref printerIP);
			this.dbConnection.GetVideojetPrinterPort(ref port);
			this.dbConnection.GetVideojetPrinterStopOnError(ref stopConveyorOnError);
			this.dbConnection.GetVideojetPrinterTimeout(ref timeout);
			this.videojetPrinter = new VideojetPrinter(printerIP, port, stopConveyorOnError);
			this.videojetPrinter.Available = available;
			this.videojetPrinter.Enabled = enabled;
			this.videojetPrinter.Timeout = timeout;
		}

		// Token: 0x060024A4 RID: 9380 RVA: 0x000FB4F0 File Offset: 0x000F96F0
		private bool VideojetPrintTimestamp(XRSImage image)
		{
			string text = "";
			int num = 0;
			string text2 = image.LineName.TrimEnd(new char[]
			{
				' '
			});
			text2 = text2.PadLeft(2, '0');
			text2 = text2.Substring(text2.Length - 2, 2);
			bool flag = !int.TryParse(text2, out num);
			if (flag)
			{
				text2 = "00";
			}
			DateTime dateTime = new DateTime((long)((ulong)image.Timestamp * 10000000UL + (ulong)new DateTime(1970, 1, 1).Ticks));
			text += dateTime.Hour.ToString("D2");
			text += dateTime.Minute.ToString("D2");
			text += dateTime.Second.ToString("D2");
			text += image.HundrethSecond.ToString("D2")[0].ToString();
			text += text2;
			text += dateTime.Day.ToString("D2");
			text += dateTime.Month.ToString("D2");
			return this.videojetPrinter.Print(text);
		}

		// Token: 0x060024A5 RID: 9381 RVA: 0x000FB658 File Offset: 0x000F9858
		private void StartBulkLiveXRIImage(object obj)
		{
			this.isBulkLiveImage = true;
			this.isBulkObjectImage = false;
			this.parHandling.WriteValue("/filter/acquisition/ContinuousModeObjImg/Value", "0");
			this.parHandling.WriteValue("/filter/acquisition/SendImage/Value", "1");
			this.parHandling.WriteValue("/filter/acquisition/SendOnlyIfContaminant/Value", "0");
			this.BulkLiveModeSourcePath = "..\\img\\xriimageBulk_pause.png";
			this.IsImageBoxVisible = false;
			this.IsImageBoxVisibleBigImage = false;
			this.resetImages = true;
		}

		// Token: 0x060024A6 RID: 9382 RVA: 0x000FB6DC File Offset: 0x000F98DC
		private void StartBulkObjectXRIImage(object obj)
		{
			this.isBulkObjectImage = true;
			this.isBulkLiveImage = false;
			bool flag = this.BulkContImagesShown;
			if (flag)
			{
				this.parHandling.WriteValue("/filter/acquisition/ContinuousModeObjImg/Value", "1");
				this.BulkObjectModeSourcePath = "..\\img\\xriimageBulkCont_pause.png";
			}
			else
			{
				this.BulkObjectModeSourcePath = "..\\img\\xriimageBulkJustCont_pause.png";
				this.parHandling.WriteValue("/filter/acquisition/ContinuousModeObjImg/Value", "0");
				this.parHandling.WriteValue("/filter/acquisition/SendImage/Value", "0");
				this.parHandling.WriteValue("/filter/acquisition/SendOnlyIfContaminant/Value", "1");
			}
			this.IsImageBoxVisible = false;
			this.IsImageBoxVisibleBigImage = false;
			this.resetImages = true;
		}

		// Token: 0x060024A7 RID: 9383 RVA: 0x000FB790 File Offset: 0x000F9990
		private void BulkSwitchSortingOnOff(object obj)
		{
			this.BulkOnOff = !this.BulkOnOff;
			string value = this.BulkOnOff ? "0" : "1";
			bool flag = this.BulkOnOff;
			if (flag)
			{
				this.BulkSwitchOnOffPath = "..\\img\\rejectOFF.png";
			}
			else
			{
				this.BulkSwitchOnOffPath = "..\\img\\rejectON.png";
			}
			bool flag2 = this.parHandling.ReadPar(FlapsOverIOs.bulkflapsEnabledPath) == "1";
			if (flag2)
			{
				this.parHandling.WriteValue(FlapsOverIOs.bulkflapsEnableValvesPath, value);
			}
			else
			{
				bool flag3 = this.parHandling.ReadPar(SVT.svtEnablePath) == "1";
				if (flag3)
				{
					this.parHandling.WriteValue(SVT.svtValvesEnablePath, value);
				}
			}
			this.machineState.StartMessage.BulkSortingOnOff = this.BulkOnOff;
		}

		// Token: 0x060024A8 RID: 9384 RVA: 0x000FB868 File Offset: 0x000F9A68
		private void BulkSwitchSortingOn(object obj)
		{
			bool flag = this.BulkOnOff;
			if (flag)
			{
				LogbookManager.AddEntry(SSTEntryCode.SortingSwitchedOn, 0, "", "", "", "");
			}
			this.BulkOnOff = false;
			string value = this.BulkOnOff ? "0" : "1";
			this.BulkSwitchOnOffPath = "..\\img\\rejectON.png";
			bool flag2 = this.parHandling.ReadPar(FlapsOverIOs.bulkflapsEnabledPath) == "1";
			if (flag2)
			{
				this.parHandling.WriteValue(FlapsOverIOs.bulkflapsEnableValvesPath, value, false);
			}
			else
			{
				bool flag3 = this.parHandling.ReadPar(SVT.svtEnablePath) == "1";
				if (flag3)
				{
					this.parHandling.WriteValue(SVT.svtValvesEnablePath, value, false);
				}
			}
			this.machineState.StartMessage.BulkSortingOnOff = this.BulkOnOff;
		}

		// Token: 0x060024A9 RID: 9385 RVA: 0x000FB948 File Offset: 0x000F9B48
		private void StartRetrainLiveXRIImage(object obj)
		{
			bool flag = this.BulkXRSImageRetrain != null;
			if (flag)
			{
				Progress.ShowStaticProgress("Retraining...", false);
				bool flag2 = !this.productManager.CurrentLoadedProduct.IsRegionProduct;
				if (flag2)
				{
					this.parHandling.WriteValue("/filter/acquisition/ContinuousMode/Value", "0");
					this.parHandling.WriteValue("/filter/acquisition/ContinuousModeFiltering/Value", "0");
					Thread.Sleep(200);
					Filter.WriteToImageProcessingFiltersBulkMode(this.parHandling, "Retrain", "1");
					Filter.SaveActualLearnedParams(this.parHandling, (int)this.BulkXRSImageRetrain.Group);
				}
				this.imageTransfer.FrameArrived += this.imgTransfer_FrameArrivedRetrain;
				this.imageTransfer.SendImage(this.BulkXRSImageRetrain);
			}
		}

		// Token: 0x060024AA RID: 9386 RVA: 0x000FBA1C File Offset: 0x000F9C1C
		private void imgTransfer_FrameArrivedRetrain(object sender, ReceiveEventArgs e)
		{
			bool flag = e.Image.Type == ImageType.Demo;
			if (flag)
			{
				this.imageTransfer.FrameArrived -= this.imgTransfer_FrameArrivedRetrain;
				Filter.UpdateImageProcessingFilterPaths(this.dbConnection, this.parHandling, (int)e.Image.Group, this.productManager, false);
				LogbookManager.AddEntry(SSTEntryCode.Retrain, 0, this.productManager.CurrentLoadedProduct.ProductNumber, this.productManager.CurrentLoadedProduct.ProductName);
				this.parHandling.WriteValue("/filter/acquisition/ContinuousMode/Value", "1");
				this.parHandling.WriteValue("/filter/acquisition/ContinuousModeFiltering/Value", "1");
				Progress.CloseStaticProgress(false);
			}
		}

		// Token: 0x060024AB RID: 9387 RVA: 0x000FBADC File Offset: 0x000F9CDC
		private void setHighSpeedValues(bool val)
		{
			if (val)
			{
				this.HighSpeedMachineActive = true;
				this.parHandling.WriteValue("/filter/acquisition/SaveImage/Value", "1");
				this.parHandling.WriteValue("/filter/acquisition/SaveOnlyIfContaminant/Value", "1");
				this.parHandling.WriteValue("/filter/acquisition/SendImage/Value", "1");
				this.parHandling.WriteValue("/filter/acquisition/SendOnlyIfContaminant/Value", "0");
				this.parHandling.WriteValue("/filter/acquisition/ShowImage/Value", "1");
				this.parHandling.WriteValue("/filter/acquisition/ShowOnlyIfContaminant/Value", "1");
			}
			else
			{
				this.HighSpeedMachineActive = false;
				this.parHandling.WriteValue("/filter/acquisition/SaveImage/Value", "1");
				this.parHandling.WriteValue("/filter/acquisition/SaveOnlyIfContaminant/Value", "0");
				this.parHandling.WriteValue("/filter/acquisition/SendImage/Value", "1");
				this.parHandling.WriteValue("/filter/acquisition/SendOnlyIfContaminant/Value", "0");
				this.parHandling.WriteValue("/filter/acquisition/ShowImage/Value", "1");
				this.parHandling.WriteValue("/filter/acquisition/ShowOnlyIfContaminant/Value", "0");
			}
		}

		// Token: 0x060024AC RID: 9388 RVA: 0x000FBC14 File Offset: 0x000F9E14
		private void TakeHighSpeedImage(object ob)
		{
			bool flag = !this.ShowNext;
			if (flag)
			{
				this.ShowNext = true;
			}
			else
			{
				this.ShowNext = false;
			}
		}

		// Token: 0x060024AD RID: 9389 RVA: 0x000FBC45 File Offset: 0x000F9E45
		private void highSpeedTimer_Elapsed(object sender, ElapsedEventArgs e)
		{
			this.ShowNext = false;
		}

		// Token: 0x060024AE RID: 9390 RVA: 0x000FBC50 File Offset: 0x000F9E50
		protected virtual void OnThroughputIncreased()
		{
			Action throughputIncreased = this.ThroughputIncreased;
			bool flag = throughputIncreased != null;
			if (flag)
			{
				throughputIncreased();
			}
		}

		// Token: 0x060024AF RID: 9391 RVA: 0x000FBC74 File Offset: 0x000F9E74
		protected virtual void OnRejectIncreased()
		{
			Action rejectIncreased = this.RejectIncreased;
			bool flag = rejectIncreased != null;
			if (flag)
			{
				rejectIncreased();
			}
		}

		// Token: 0x060024B0 RID: 9392 RVA: 0x000FBC98 File Offset: 0x000F9E98
		private void SVTUpdateValues()
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			int num6 = 0;
			int num7 = 0;
			int num8 = 0;
			bool flag = false;
			List<int> list = new List<int>();
			List<int> list2 = new List<int>();
			double num9 = 0.0;
			int num10 = 0;
			string text = this.parHandling.ReadPar(Definition.detectorTypePath);
			this.dbConnection.GetIgnoredRightPixel(ref num2);
			this.dbConnection.GetIgnoredLeftPixel(ref num);
			this.dbConnection.GetFirstValve(ref num6);
			this.dbConnection.GetNumValves(ref num5);
			this.dbConnection.GetSVTEndPixelNextToLastValve(ref num3);
			this.dbConnection.GetSVTStartPixelSecondValve(ref num4);
			this.dbConnection.GetValveOverlappingLeft(ref num7);
			this.dbConnection.GetValveOverlappingRight(ref num8);
			this.dbConnection.GetValvesInverted(ref flag);
			int num11 = 0;
			bool flag2 = text.Equals("1");
			if (flag2)
			{
				int.TryParse(this.parHandling.ReadPar(Definition.detectorSesotecNPixelPath), out num11);
			}
			else
			{
				int.TryParse(this.parHandling.ReadPar(Definition.detectorDTNPixelPath), out num11);
			}
			double num12 = Math.Round((double)(num3 - num4) / ((double)num5 - 2.0), 2);
			int num13 = (int)num12 - (num4 - num);
			int num14 = num11 - num2;
			int num15 = (int)num12;
			for (int i = 1; i <= num5; i++)
			{
				bool flag3 = i < num6;
				int num16;
				if (flag3)
				{
					num16 = -1;
					num10 = -1;
				}
				else
				{
					bool flag4 = i == num6;
					if (flag4)
					{
						num16 = 0;
						num10 = num15 - num13 - 1 + num8;
					}
					else
					{
						num16 = num10 - num7 - num8;
						num10 += num15;
					}
					bool flag5 = num16 < 0;
					if (flag5)
					{
						num16 = -1;
					}
					bool flag6 = num10 < 0;
					if (flag6)
					{
						num10 = -1;
					}
					num9 += num12 - (double)num15;
					bool flag7 = num9 >= 0.5 || i == num6;
					if (flag7)
					{
						num10++;
						num9 -= 1.0;
					}
				}
				list.Add(num16);
				list2.Add(num10);
				bool flag8 = flag;
				string str;
				if (flag8)
				{
					str = Definition.SVTValvesAssignmentMainPath + (num5 - i + 1).ToString("D2");
				}
				else
				{
					str = Definition.SVTValvesAssignmentMainPath + i.ToString("D2");
				}
				this.parHandling.WriteValue(str + "/MinPixel/Value", num16.ToString(), false);
				this.parHandling.WriteValue(str + "/MaxPixel/Value", num10.ToString(), false);
			}
			for (int j = num5 + 1; j <= 80; j++)
			{
				bool flag9 = flag;
				string str;
				if (flag9)
				{
					str = Definition.SVTValvesAssignmentMainPath + (num5 - j + 1).ToString("D2");
				}
				else
				{
					str = Definition.SVTValvesAssignmentMainPath + j.ToString("D2");
				}
				this.parHandling.WriteValue(str + "/MinPixel/Value", "-1", false);
				this.parHandling.WriteValue(str + "/MaxPixel/Value", "-1", false);
				list.Add(-1);
				list2.Add(-1);
			}
			this.dbConnection.SetSVTMinPixels(0, list.ToArray(), flag);
			this.dbConnection.SetSVTMaxPixels(0, list2.ToArray(), flag);
		}

		// Token: 0x060024B1 RID: 9393 RVA: 0x000FC02C File Offset: 0x000FA22C
		private void StartTimerImageBox()
		{
			bool flag = this.timerImageBoxVisible != null;
			if (flag)
			{
				this.timerImageBoxVisible.Stop();
			}
			this.timerImageBoxVisible = new DispatcherTimer(new TimeSpan(0, 0, 15), DispatcherPriority.Background, new EventHandler(this.stopImageBoxTimer), Application.Current.Dispatcher);
			this.timerImageBoxVisible.Start();
		}

		// Token: 0x060024B2 RID: 9394 RVA: 0x000FC08C File Offset: 0x000FA28C
		private void stopImageBoxTimer(object dummy, EventArgs e)
		{
			bool flag = this.timerImageBoxVisible != null;
			if (flag)
			{
				this.timerImageBoxVisible.Stop();
			}
			this.IsImageBoxVisible = false;
			this.IsImageBoxVisibleBigImage = false;
		}

		// Token: 0x060024B3 RID: 9395 RVA: 0x000FC0C4 File Offset: 0x000FA2C4
		private void setCheckArcs(object dummy, EventArgs e)
		{
			this.parHandling.WriteValue(Definition.checkArcsEnable, "1");
			bool flag = this.timerBulk != null;
			if (flag)
			{
				this.timerBulk.Stop();
			}
		}

		// Token: 0x060024B4 RID: 9396 RVA: 0x000FC104 File Offset: 0x000FA304
		private void CreateReferenceProduct()
		{
			bool flag = this.productManager.referenceProduct != null;
			if (!flag)
			{
				int num = -1;
				string text = this.dbConnection.CreateNewProductTableName();
				bool flag2 = this.dbConnection.CreateNewAppl("Reference Product", num.ToString(), ProductType.SingleProduct, text);
				bool flag3 = flag2;
				if (flag3)
				{
					ProductData productData = new ProductData(text, "Reference Product", num.ToString());
					this.productManager.referenceProduct = productData;
					LogbookManager.AddEntry(SSTEntryCode.ProductCreated, 0, productData.ProductNumber, productData.ProductName);
				}
			}
		}

		// Token: 0x060024B5 RID: 9397 RVA: 0x000FC194 File Offset: 0x000FA394
		private void CheckAnomalyDetectionParameters(object dummy, EventArgs e)
		{
			bool flag = this.parHandling.ReadPar(Definition.anomalyDetectionEnabled) == "1";
			if (flag)
			{
				bool flag2 = this.parHandling.ReadPar(Definition.anomalyDetectionTrainFinished) == "1";
				if (flag2)
				{
					this.aiService.UpdateAIParametersAfterTrain();
					this.RefreshColourBtnAIStatus();
				}
				bool flag3 = this.parHandling.ReadPar(Definition.anomalyDetectionTrainingInProgress) == "1";
				if (flag3)
				{
					string text = "";
					this.dbConnection.GetCurrentProductTableNameTrainingAI(ref text);
					bool flag4 = text == "";
					if (flag4)
					{
						string number = this.parHandling.ReadPar(Definition.anomalyDetectionTrainModelID);
						this.dbConnection.GetAppTableNameWithNumber(number, ref text);
						this.dbConnection.SetProductTableNameTrainingAI(text);
					}
				}
				this.aiService.SendAnomalyDetectionTrainImages();
			}
		}

		// Token: 0x060024B6 RID: 9398 RVA: 0x000FC27C File Offset: 0x000FA47C
		private void InitializeModelTransferConnection()
		{
			bool flag = !this.aiModelInitialized;
			if (flag)
			{
				this.modelTransfer.Connected += this.modelTransfer_Connected;
				this.modelTransfer.Disconnected += this.modelTransfer_Disconnected;
				this.modelTransfer.ModelDataError += this.modelTransfer_ModelDataError;
				this.aiModelInitialized = true;
			}
		}

		// Token: 0x060024B7 RID: 9399 RVA: 0x000FC2E8 File Offset: 0x000FA4E8
		private void CloseModelTransferConnection()
		{
			this.modelTransfer.Connected -= this.modelTransfer_Connected;
			this.modelTransfer.Disconnected -= this.modelTransfer_Disconnected;
			this.modelTransfer.ModelDataError -= this.modelTransfer_ModelDataError;
			this.aiModelInitialized = false;
		}

		// Token: 0x060024B8 RID: 9400 RVA: 0x000FC348 File Offset: 0x000FA548
		private void modelTransfer_Connected()
		{
			this.aiModelConnected = true;
			bool flag = this.errorSurvey != null;
			if (flag)
			{
				this.errorSurvey.AIModelPortOk = true;
			}
		}

		// Token: 0x060024B9 RID: 9401 RVA: 0x000FC378 File Offset: 0x000FA578
		private void modelTransfer_Disconnected()
		{
			bool flag = this.aiModelConnected;
			if (flag)
			{
				this.aiModelConnected = false;
				this.errorSurvey.AIModelPortOk = false;
			}
		}

		// Token: 0x060024BA RID: 9402 RVA: 0x000FC3A8 File Offset: 0x000FA5A8
		private void modelTransfer_ModelDataError(string message)
		{
			bool flag = this.modelTransfer != null;
			if (flag)
			{
				this.modelTransfer.CloseConnection();
				Thread.Sleep(1500);
				this.modelTransfer.StartConnect();
			}
			bool flag2 = this.aiService != null;
			if (flag2)
			{
				this.aiService.SetAIModelID();
			}
		}

		// Token: 0x060024BB RID: 9403 RVA: 0x000FC404 File Offset: 0x000FA604
		private void ShowAIStatus(object obj)
		{
			bool flag = false;
			bool flag2 = false;
			this.dbConnection.GetAnomalyDetectionEnabled(ref flag);
			this.dbConnection.GetObjectDetectionEnabled(ref flag2);
			bool flag3 = flag && flag2;
			if (flag3)
			{
				CtrlAIMenuSelection ctrlAIMenuSelection = new CtrlAIMenuSelection(this.parHandling, this.productManager, this.dbConnection, MainWindowViewModel.accessRightsManager, this.imageTransfer, this.imageArchiver, this.errorHandling, this.testcardsimservice, this.aiService, CtrlAIMenuSelection.SelectionMenuContext.StatusMenu);
				Subwindow.Show(ctrlAIMenuSelection, SubwindowButtons.Back, ctrlAIMenuSelection.PanelButtons, ctrlAIMenuSelection.UiElementRightAligned);
			}
			else
			{
				bool flag4 = flag;
				if (flag4)
				{
					CtrlAIStatus ctrlAIStatus = new CtrlAIStatus(this.parHandling, MainWindowViewModel.accessRightsManager, this.dbConnection, this.productManager, this.aiService, this.testcardsimservice, this.imageTransfer, this.imageArchiver);
					Subwindow.Show(ctrlAIStatus, SubwindowButtons.Back, ctrlAIStatus.PanelButtons, ctrlAIStatus.UiElementRightAligned);
				}
				else
				{
					bool flag5 = flag2;
					if (flag5)
					{
						CtrlAIObjectStatus ctrlAIObjectStatus = new CtrlAIObjectStatus(this.parHandling, MainWindowViewModel.accessRightsManager, this.productManager);
						Subwindow.Show(ctrlAIObjectStatus, SubwindowButtons.Back, ctrlAIObjectStatus.PanelButtons, ctrlAIObjectStatus.UiElementRightAligned);
					}
				}
			}
		}

		// Token: 0x17000A81 RID: 2689
		// (get) Token: 0x060024BC RID: 9404 RVA: 0x000FC520 File Offset: 0x000FA720
		// (set) Token: 0x060024BD RID: 9405 RVA: 0x000FC538 File Offset: 0x000FA738
		public string ImageSourceAIStatus
		{
			get
			{
				return this.imageSourceAIStatus;
			}
			set
			{
				bool flag = this.imageSourceAIStatus != value;
				if (flag)
				{
					this.imageSourceAIStatus = value;
					this.OnPropertyChanged("ImageSourceAIStatus");
				}
			}
		}

		// Token: 0x17000A82 RID: 2690
		// (get) Token: 0x060024BE RID: 9406 RVA: 0x000FC56C File Offset: 0x000FA76C
		// (set) Token: 0x060024BF RID: 9407 RVA: 0x000FC584 File Offset: 0x000FA784
		public bool ShowBtnAIStatus
		{
			get
			{
				return this.showBtnAIStatus;
			}
			set
			{
				bool flag = this.showBtnAIStatus != value;
				if (flag)
				{
					this.showBtnAIStatus = value;
					this.OnPropertyChanged("ShowBtnAIStatus");
				}
			}
		}

		// Token: 0x060024C0 RID: 9408 RVA: 0x000FC5B8 File Offset: 0x000FA7B8
		public void RefreshColourBtnAIStatus()
		{
			this.aiService.UpdateAvailabilityModelFiles();
			bool flag = this.parHandling.ReadPar(Definition.anomalyDetectionEnabled) == "1" && this.parHandling.ReadPar(Definition.objectDetectionEnabled) == "0";
			if (flag)
			{
				bool flag2 = this.parHandling.ReadPar(Definition.anomalyDetectionTensorRTEngineAvailable) == "1";
				bool flag3 = this.parHandling.ReadPar(Definition.anomalyDetectionOnnxAvailable) == "1";
				bool flag4 = this.parHandling.ReadPar(Definition.anomalyDetectionInferenceEnabled) == "1";
				bool flag5 = (flag2 || flag3) && flag4;
				if (flag5)
				{
					this.ImageSourceAIStatus = "../img/ai_brain_green.png";
				}
				else
				{
					bool flag6 = (flag2 || flag3) && !flag4;
					if (flag6)
					{
						this.ImageSourceAIStatus = "../img/ai_brain_blue.png";
					}
					else
					{
						this.ImageSourceAIStatus = "../img/ai_brain.png";
					}
				}
			}
			else
			{
				bool flag7 = this.parHandling.ReadPar(Definition.anomalyDetectionEnabled) == "0" && this.parHandling.ReadPar(Definition.objectDetectionEnabled) == "1";
				if (flag7)
				{
					bool flag8 = this.parHandling.ReadPar(Definition.objectDetectionTensorRTEngineAvailable) == "1";
					bool flag9 = this.parHandling.ReadPar(Definition.objectDetectionOnnxAvailable) == "1";
					bool flag10 = this.parHandling.ReadPar(Definition.objectDetectionInferenceEnabled) == "1";
					bool flag11 = (flag8 || flag9) && flag10;
					if (flag11)
					{
						this.ImageSourceAIStatus = "../img/ai_brain_green.png";
					}
					else
					{
						bool flag12 = (flag8 || flag9) && !flag10;
						if (flag12)
						{
							this.ImageSourceAIStatus = "../img/ai_brain_blue.png";
						}
						else
						{
							this.ImageSourceAIStatus = "../img/ai_brain.png";
						}
					}
				}
				else
				{
					bool flag13 = this.parHandling.ReadPar(Definition.anomalyDetectionEnabled) == "1" && this.parHandling.ReadPar(Definition.objectDetectionEnabled) == "1";
					if (flag13)
					{
						this.ImageSourceAIStatus = "../img/ai_brain_thinkX.png";
					}
				}
			}
		}

		// Token: 0x060024C1 RID: 9409 RVA: 0x000FC7E1 File Offset: 0x000FA9E1
		private void ProductManager_BatchChanged()
		{
			this.batchHasChanged = true;
		}

		// Token: 0x060024C2 RID: 9410 RVA: 0x000FC7EC File Offset: 0x000FA9EC
		private void writeValuesforBgMonitoring()
		{
			string value = "";
			bool flag = this.parHandling != null;
			if (flag)
			{
				bool limitsEntry = this.dbDatalogs.GetLimitsEntry("BgMonLiveErrorPixel", ref value);
				if (limitsEntry)
				{
					this.parHandling.WriteValue(Definition.detectorMonitoringPixelCountBorder, value, false);
				}
				value = "";
				bool limitsEntry2 = this.dbDatalogs.GetLimitsEntry("BgMonLiveImageLines", ref value);
				if (limitsEntry2)
				{
					this.parHandling.WriteValue(Definition.detectorMonitoringImageLines, value, false);
				}
				value = "";
				bool limitsEntry3 = this.dbDatalogs.GetLimitsEntry("BgMonLiveThreshold", ref value);
				if (limitsEntry3)
				{
					this.parHandling.WriteValue(Definition.detectorMonitoringThreshold, value, false);
				}
			}
		}

		// Token: 0x060024C3 RID: 9411 RVA: 0x000FC89C File Offset: 0x000FAA9C
		private void UpdateVisibilityControls()
		{
			this.ShowBtnAIStatus = (this.parHandling.ReadPar(Definition.anomalyDetectionEnabled) == "1" || this.parHandling.ReadPar(Definition.objectDetectionEnabled) == "1");
			this.DoorLockAvailable = (this.parHandling.ReadPar(Definition.doorLockAvailablePath) == "1");
			this.HighSpeedMachineActive = (this.parHandling.ReadPar(Definition.highThroughputEnabled) == "1");
			this.StandardMode = (this.parHandling.ReadPar(Definition.machineModePath) != Convert.ToString(MachineMode.Bulk));
			this.SliderVisibleBulkMode = this.BulkModeActive;
		}

		// Token: 0x060024C4 RID: 9412 RVA: 0x000FC960 File Offset: 0x000FAB60
		public bool takeScreenshot()
		{
			bool flag = this.CurrentUser.Rights >= BuiltInPriviledges.Operator1;
			if (flag)
			{
				double virtualScreenLeft = SystemParameters.VirtualScreenLeft;
				double num = SystemParameters.VirtualScreenTop - 50.0;
				double virtualScreenHeight = SystemParameters.VirtualScreenHeight;
				double virtualScreenWidth = SystemParameters.VirtualScreenWidth;
				Bitmap bitmap = new Bitmap((int)virtualScreenWidth, (int)virtualScreenHeight - 50);
				Graphics graphics = Graphics.FromImage(bitmap);
				graphics.CopyFromScreen((int)virtualScreenLeft, 50, 0, 0, bitmap.Size);
				string text = "Screenshot_" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_ff") + ".png";
				string text2 = "";
				this.dbConnection.GetUsbDriveLetter(ref text2);
				string text3 = "Screenshots";
				bool flag2 = Directory.Exists(text2);
				if (!flag2)
				{
					return false;
				}
				bool flag3 = !Directory.Exists(text2 + text3);
				if (flag3)
				{
					Directory.CreateDirectory(text2 + "\\" + text3);
				}
				bool flag4 = true;
				bool flag5 = flag4;
				if (flag5)
				{
					long totalFreeSpace = Disk.GetTotalFreeSpace(text2 + "\\");
					bool flag6 = totalFreeSpace > 5000000L;
					if (flag6)
					{
						bitmap.Save(string.Concat(new string[]
						{
							text2,
							"\\",
							text3,
							"\\",
							text
						}));
						return true;
					}
					bool flag7 = Application.Current != null;
					if (flag7)
					{
						Application.Current.Dispatcher.Invoke(delegate()
						{
							Dialog.Show(LocalizationManager.Localize("txtInfo"), "Speicherplatz voll", DialogStyle.Info, DialogButtons.OKCancel);
						});
					}
					return false;
				}
			}
			return false;
		}

		// Token: 0x060024C5 RID: 9413 RVA: 0x000FCB1C File Offset: 0x000FAD1C
		private void executeRetrainMode(XRSImage image)
		{
			bool flag = !this.productManager.CurrentLoadedProduct.IsRegionProduct;
			if (flag)
			{
				Filter.WriteToImageProcessingFilters(this.parHandling, "Retrain", "1", (int)image.Group);
				Filter.SaveActualLearnedParams(this.parHandling, (int)image.Group);
			}
			this.imageTransfer.FrameArrived += this.imgTransfer_FrameArrivedRetrainSchwarz;
			this.imageTransfer.SendImage(image);
		}

		// Token: 0x060024C6 RID: 9414 RVA: 0x000FCB98 File Offset: 0x000FAD98
		private void imgTransfer_FrameArrivedRetrainSchwarz(object sender, ReceiveEventArgs e)
		{
			bool flag = e.Image.Type == ImageType.Demo;
			if (flag)
			{
				this.lastRetrainImage = e.Image;
				this.imageTransfer.FrameArrived -= this.imgTransfer_FrameArrivedRetrainSchwarz;
				Filter.UpdateImageProcessingFilterPaths(this.dbConnection, this.parHandling, (int)e.Image.Group, this.productManager, false);
				bool flag2 = this.parHandling.ReadPar(Definition.anomalyDetectionEnabled) == "1" && this.parHandling.ReadPar(Definition.anomalyDetectionInferenceEnabled) == "1";
				if (flag2)
				{
					Filter.UpdateAnomalyDetectionInferenceThresholds(this.dbConnection, this.parHandling);
				}
				bool flag3 = this.parHandling.ReadPar(Definition.objectDetectionEnabled) == "1" && this.parHandling.ReadPar(Definition.objectDetectionInferenceEnabled) == "1";
				if (flag3)
				{
					Filter.UpdateObjectDetectionInferenceThresholds(this.dbConnection, this.parHandling);
				}
				LogbookManager.AddEntry(SSTEntryCode.Retrain, 0, this.productManager.CurrentLoadedProduct.ProductNumber, this.productManager.CurrentLoadedProduct.ProductName);
				this.AutoRetrainUndo = true;
			}
		}

		// Token: 0x060024C7 RID: 9415 RVA: 0x000FCCD8 File Offset: 0x000FAED8
		private void UndoLastRetrain(object obj)
		{
			bool flag = Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtUndoRetrain"), DialogStyle.Warning, DialogButtons.OKCancel) == Dialog.DialogResults.Ok;
			if (flag)
			{
				Progress.ShowStaticProgress("Undo Retraining..", false);
				bool flag2 = !this.productManager.CurrentLoadedProduct.IsRegionProduct;
				if (flag2)
				{
					Filter.SetPreviousLearnedParams(this.parHandling);
				}
			}
			this.AutoRetrainUndo = false;
			bool flag3 = this.lastRetrainImage != null;
			if (flag3)
			{
				this.imageTransfer.SendImage(this.lastRetrainImage);
				this.imageTransfer.FrameArrived += this.imgTransfer_FrameArrivedUndoRetrain;
			}
			else
			{
				Progress.CloseStaticProgress(false);
			}
		}

		// Token: 0x060024C8 RID: 9416 RVA: 0x000FCD88 File Offset: 0x000FAF88
		private void imgTransfer_FrameArrivedUndoRetrain(object sender, ReceiveEventArgs e)
		{
			bool flag = e.Image.Type == ImageType.Demo;
			if (flag)
			{
				bool flag2 = Application.Current != null;
				if (flag2)
				{
					Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
					{
						Dialog.Show(LocalizationManager.Localize("txtInfo"), LocalizationManager.Localize("txtSuccess"), DialogStyle.Info, DialogButtons.None, true);
					}));
				}
				Progress.CloseStaticProgress(false);
				LogbookManager.AddEntry(SSTEntryCode.UndoRetrain);
				this.imageTransfer.FrameArrived -= this.imgTransfer_FrameArrivedUndoRetrain;
			}
		}

		// Token: 0x060024C9 RID: 9417 RVA: 0x000FCE13 File Offset: 0x000FB013
		private void RetrainModeOnOff(object obj)
		{
			this.RetrainMode = !this.RetrainMode;
		}

		// Token: 0x060024CA RID: 9418 RVA: 0x000FCE26 File Offset: 0x000FB026
		private void accessRightsManager_ResetRetrain()
		{
			this.AutoRetrainUndo = false;
		}

		// Token: 0x040016B1 RID: 5809
		public static string elinosVersion = "";

		// Token: 0x040016B2 RID: 5810
		public static string elinosVersionBuild = "";

		// Token: 0x040016B3 RID: 5811
		private const string filterAlgorithmSubPath = "/global/Algorithm/Value";

		// Token: 0x040016B4 RID: 5812
		private const string filterGlobalLightToleranceSubPath = "/global/LightTolerance/Value";

		// Token: 0x040016B5 RID: 5813
		private const string filterGlobalDenseToleranceSubPath = "/global/DenseTolerance/Value";

		// Token: 0x040016B6 RID: 5814
		private const string filterGlobalCustomToleranceSubPath = "/global/CustomTolerance/Value";

		// Token: 0x040016B7 RID: 5815
		private const string filterGlobalEnableSubPath = "/global/Enable/Value";

		// Token: 0x040016B8 RID: 5816
		private const string filterGlobalCustomFilterSizeSubPath = "/global/CustomFilterSize/Value";

		// Token: 0x040016B9 RID: 5817
		private const string testSuiteActivePath = "/testSuite/Active/Value";

		// Token: 0x040016BA RID: 5818
		public static string guiVersionPath = "/application/GUIVersion/Value";

		// Token: 0x040016BB RID: 5819
		public static string guiVersionBuildPath = "/application/GUIVersionBuild/Value";

		// Token: 0x040016BC RID: 5820
		public static string databaseVersionPath = "/application/DatabaseVersion/Value";

		// Token: 0x040016BD RID: 5821
		public static string elinosVersionPath = "/application/ElinosVersion/Value";

		// Token: 0x040016BE RID: 5822
		public static string elinosVersionBuildPath = "/application/ElinosVersionBuild/Value";

		// Token: 0x040016BF RID: 5823
		public static string ipWagoPath = "/hardware/ios/IPAddress/Value";

		// Token: 0x040016C0 RID: 5824
		public static string filterIndicator = "/filter";

		// Token: 0x040016C1 RID: 5825
		private int defaultVisibleProductNumber = 1;

		// Token: 0x040016C2 RID: 5826
		public static readonly int defaultUser = 0;

		// Token: 0x040016C3 RID: 5827
		private const string rayconDeviceType = "30";

		// Token: 0x040016C4 RID: 5828
		public static string timePaintImagePath = "/application/TimePaintImage/Value";

		// Token: 0x040016C5 RID: 5829
		public static string timeSaveXriPath = "/application/TimeSaveXri/Value";

		// Token: 0x040016C6 RID: 5830
		private const int laneTabHeight = 40;

		// Token: 0x040016C7 RID: 5831
		private static Subwindow subwindowAutotrainMain;

		// Token: 0x040016C8 RID: 5832
		private BuiltInPriviledges rightLogbook = BuiltInPriviledges.NoUser;

		// Token: 0x040016C9 RID: 5833
		private BuiltInPriviledges rightTrain = BuiltInPriviledges.Operator1;

		// Token: 0x040016CA RID: 5834
		private BuiltInPriviledges rightProductChange = BuiltInPriviledges.Operator2;

		// Token: 0x040016CB RID: 5835
		private BuiltInPriviledges rightSettings = BuiltInPriviledges.Operator3;

		// Token: 0x040016CC RID: 5836
		private BuiltInPriviledges rightSensitivity = BuiltInPriviledges.Operator1;

		// Token: 0x040016CD RID: 5837
		private bool noAutoKVAdjustment = false;

		// Token: 0x040016CE RID: 5838
		private LocalizationManager localizationManager;

		// Token: 0x040016CF RID: 5839
		private bool imageConnected = true;

		// Token: 0x040016D0 RID: 5840
		private bool parConnected = true;

		// Token: 0x040016D1 RID: 5841
		private bool aiModelConnected = false;

		// Token: 0x040016D2 RID: 5842
		private bool aiModelInitialized = false;

		// Token: 0x040016D3 RID: 5843
		private ProductChangeControl prodChange;

		// Token: 0x040016D4 RID: 5844
		private AutotrainControl ctrlAutotrain;

		// Token: 0x040016D5 RID: 5845
		private CtrlAutotrainAdjustments ctrlAutotrainAdjustments;

		// Token: 0x040016D6 RID: 5846
		private string deviceAddress;

		// Token: 0x040016D7 RID: 5847
		private string deviceSerialNbr;

		// Token: 0x040016D8 RID: 5848
		private bool auditNeeded;

		// Token: 0x040016D9 RID: 5849
		private DBConnection dbConnection;

		// Token: 0x040016DA RID: 5850
		private DBDataLogs dbDatalogs;

		// Token: 0x040016DB RID: 5851
		private DBDataLogsManager dbDatalogsManager;

		// Token: 0x040016DC RID: 5852
		private StoreImageData imageArchiver = new StoreImageData();

		// Token: 0x040016DD RID: 5853
		private ImageDataTransfer imageTransfer;

		// Token: 0x040016DE RID: 5854
		private ParameterHandling parHandling;

		// Token: 0x040016DF RID: 5855
		private Conveyor conveyor;

		// Token: 0x040016E0 RID: 5856
		private AuditCheckSettings auditSettings;

		// Token: 0x040016E2 RID: 5858
		private Restore restore;

		// Token: 0x040016E3 RID: 5859
		private Backup backup;

		// Token: 0x040016E4 RID: 5860
		private bool parametersInitialized;

		// Token: 0x040016E5 RID: 5861
		private Progress progressRestore;

		// Token: 0x040016E6 RID: 5862
		private bool progressRestoreUpdateRunning;

		// Token: 0x040016E7 RID: 5863
		private string time;

		// Token: 0x040016E8 RID: 5864
		private DispatcherTimer timerUpdateDateTime;

		// Token: 0x040016E9 RID: 5865
		private DispatcherTimer timerUpdateVibroStarted;

		// Token: 0x040016EA RID: 5866
		private DispatcherTimer timerTestcardSim;

		// Token: 0x040016EB RID: 5867
		private DispatcherTimer timerImageBoxVisible;

		// Token: 0x040016EC RID: 5868
		private int timerprevieousTestcardSim = 0;

		// Token: 0x040016ED RID: 5869
		private Branding selectedBranding;

		// Token: 0x040016EE RID: 5870
		private ProductManager productManager;

		// Token: 0x040016EF RID: 5871
		private SSTEthernetServer sstServer;

		// Token: 0x040016F0 RID: 5872
		private WSFoodServer wsFoodServer;

		// Token: 0x040016F1 RID: 5873
		private ComServerClient comServer;

		// Token: 0x040016F2 RID: 5874
		private MessageSystem messageSystem;

		// Token: 0x040016F3 RID: 5875
		private BackgroundWorker bgWorkerPaintImage;

		// Token: 0x040016F4 RID: 5876
		private XRSImage lastPaintedImage;

		// Token: 0x040016F5 RID: 5877
		private XRSImage BulkXRSImageRetrain;

		// Token: 0x040016F6 RID: 5878
		private System.Threading.Timer timerLastHistoryImage;

		// Token: 0x040016F7 RID: 5879
		private Logbook logbook;

		// Token: 0x040016F8 RID: 5880
		private bool firstApplicationLoaded;

		// Token: 0x040016F9 RID: 5881
		private MeanPixelLogger meanPixelLogger;

		// Token: 0x040016FA RID: 5882
		private PixelCorrection pixelCorrection;

		// Token: 0x040016FB RID: 5883
		private DispatcherTimer timerWriteLastOnTime;

		// Token: 0x040016FC RID: 5884
		private int counterWriteLastOnTime = 0;

		// Token: 0x040016FD RID: 5885
		private bool setClockIfNeeded = true;

		// Token: 0x040016FE RID: 5886
		private ErrorHandling errorHandling;

		// Token: 0x040016FF RID: 5887
		private bool standardMode = true;

		// Token: 0x04001700 RID: 5888
		private bool sensitivityVisible;

		// Token: 0x04001701 RID: 5889
		private bool sensitivityEnabled;

		// Token: 0x04001702 RID: 5890
		private NumKeyboard numKeyboard;

		// Token: 0x04001703 RID: 5891
		private Keyboard keyboard;

		// Token: 0x04001704 RID: 5892
		private System.Threading.Timer timerAddSensitivityChangeToLb;

		// Token: 0x04001705 RID: 5893
		private ImageSource xRayImage;

		// Token: 0x04001706 RID: 5894
		private ImageSource learnOrXriImage;

		// Token: 0x04001707 RID: 5895
		private bool vibroStarted;

		// Token: 0x04001708 RID: 5896
		private MessageType worstMessageType;

		// Token: 0x04001709 RID: 5897
		private ImageSource xMessageSystemImage;

		// Token: 0x0400170A RID: 5898
		private Progress progLoadMultiProductAtStartup;

		// Token: 0x0400170B RID: 5899
		private bool multiLane;

		// Token: 0x0400170C RID: 5900
		private bool multiLaneView = false;

		// Token: 0x0400170D RID: 5901
		private LaneXri selectedXriLane;

		// Token: 0x0400170E RID: 5902
		private Thickness tabBorderThickness;

		// Token: 0x0400170F RID: 5903
		private LoginToGo loginToGo = new LoginToGo();

		// Token: 0x04001710 RID: 5904
		private bool loggedInWithToGoFeature = false;

		// Token: 0x04001711 RID: 5905
		private uint throughputGlobal = 0U;

		// Token: 0x04001712 RID: 5906
		private uint rejectGlobal = 0U;

		// Token: 0x04001713 RID: 5907
		private WdAuditRequired wdAuditRequired;

		// Token: 0x04001714 RID: 5908
		private WifiClient wifiClient;

		// Token: 0x04001715 RID: 5909
		private string lastSSTBatch = null;

		// Token: 0x04001716 RID: 5910
		private bool doorLockAvailable;

		// Token: 0x04001717 RID: 5911
		private bool resetMessagesVisible = true;

		// Token: 0x04001718 RID: 5912
		private bool startXRayNoUser = true;

		// Token: 0x04001719 RID: 5913
		private VideojetPrinter videojetPrinter;

		// Token: 0x0400171A RID: 5914
		private SetFooterMainWindow setFooterMainWindow;

		// Token: 0x0400171B RID: 5915
		private List<object> communicationsList;

		// Token: 0x0400171C RID: 5916
		private ModelTransfer modelTransfer;

		// Token: 0x0400171D RID: 5917
		private DispatcherTimer timerAnomalyDetection;

		// Token: 0x04001722 RID: 5922
		private bool isBulkLiveImage = false;

		// Token: 0x04001723 RID: 5923
		private bool isBulkObjectImage = false;

		// Token: 0x04001724 RID: 5924
		private bool BulkInAutotrainMode = false;

		// Token: 0x04001725 RID: 5925
		private BlockingCollection<ItemPngToNetworkFolder> archiveQueuePngToNetworkFolder;

		// Token: 0x04001726 RID: 5926
		private bool footerMainWindowIsDeactivated;

		// Token: 0x04001727 RID: 5927
		private bool oldValfooterMainWindowIsDeactivated;

		// Token: 0x04001728 RID: 5928
		public static AccessRightsManager accessRightsManager;

		// Token: 0x04001729 RID: 5929
		private bool bulkObjectModeActive;

		// Token: 0x0400172A RID: 5930
		private string bulkLiveModeSourcePath;

		// Token: 0x0400172B RID: 5931
		private string bulkObjectModeSourcePath;

		// Token: 0x0400172C RID: 5932
		private string bulkSwitchOnOffPath;

		// Token: 0x0400172D RID: 5933
		private bool bulkOnOff = false;

		// Token: 0x0400172E RID: 5934
		private bool bulkContImagesShown = false;

		// Token: 0x04001734 RID: 5940
		private bool contextUnlockBinVisibility = false;

		// Token: 0x04001769 RID: 5993
		private MainWindow mainWindow;

		// Token: 0x0400176A RID: 5994
		private Rejecter rejecter;

		// Token: 0x0400176B RID: 5995
		private TestcardSimService testcardsimservice;

		// Token: 0x0400176C RID: 5996
		private AIService aiService;

		// Token: 0x0400176D RID: 5997
		private bool testcardSimVisible;

		// Token: 0x0400176E RID: 5998
		private string imageSourceAIStatus = "../img/ai_brain.png";

		// Token: 0x0400176F RID: 5999
		private bool showBtnAIStatus;

		// Token: 0x04001770 RID: 6000
		private string simulatedGlassSensitivity;

		// Token: 0x04001771 RID: 6001
		private string simulatedVASensitivity;

		// Token: 0x04001772 RID: 6002
		private string complGlassSensitivity;

		// Token: 0x04001773 RID: 6003
		private string complVASensitivity;

		// Token: 0x04001774 RID: 6004
		private bool complSensitivityVisible;

		// Token: 0x04001775 RID: 6005
		private string systemIsCompliant;

		// Token: 0x04001776 RID: 6006
		private bool complTextForeground;

		// Token: 0x04001777 RID: 6007
		private bool complTextVAForeground;

		// Token: 0x04001778 RID: 6008
		private bool complTextGlassForeground;

		// Token: 0x04001779 RID: 6009
		private bool sliderVisibleBulkMode;

		// Token: 0x0400177A RID: 6010
		private IVPSettingsViewModel ivpTimer;

		// Token: 0x0400177B RID: 6011
		private CtrlAuditCheckProcess auditCheckProcess;

		// Token: 0x0400177C RID: 6012
		private CtrlFastAuditCheckProcess fastAuditCheckProcess;

		// Token: 0x0400177D RID: 6013
		private CtrlBulkAuditCheckProcess bulkAuditCheckProcess;

		// Token: 0x0400177E RID: 6014
		private bool highSpeedMachineActive;

		// Token: 0x0400177F RID: 6015
		private bool showNext;

		// Token: 0x04001780 RID: 6016
		private bool widescreen;

		// Token: 0x04001781 RID: 6017
		private bool highSpeedMachineActiveVisible2;

		// Token: 0x04001782 RID: 6018
		private bool highSpeedMachineActiveVisible1;

		// Token: 0x04001785 RID: 6021
		private bool throughputMessage = false;

		// Token: 0x04001786 RID: 6022
		private bool throughput90Message = false;

		// Token: 0x04001787 RID: 6023
		private bool rejectMessage = false;

		// Token: 0x04001788 RID: 6024
		private bool reject90Message = false;

		// Token: 0x04001789 RID: 6025
		private uint iCMSuccesfullCounter = 0U;

		// Token: 0x0400178B RID: 6027
		private bool isImageBoxVisible = false;

		// Token: 0x0400178C RID: 6028
		private bool isImageBoxVisibleBigImage = false;

		// Token: 0x0400178D RID: 6029
		private int count = 0;

		// Token: 0x0400178E RID: 6030
		private DispatcherTimer timerBulk;

		// Token: 0x0400178F RID: 6031
		private bool autoLearnVisibleRefProd = false;

		// Token: 0x04001790 RID: 6032
		private bool changing = false;

		// Token: 0x04001791 RID: 6033
		private bool batchHasChanged = false;

		// Token: 0x04001792 RID: 6034
		private string fileNamePathNetWorkSaving = "";

		// Token: 0x04001793 RID: 6035
		private string fileNamePathFtpSaving = "";

		// Token: 0x04001794 RID: 6036
		public static bool flagSaveImgToNetwork = false;

		// Token: 0x04001795 RID: 6037
		public static bool flagNotSaveImgToNetwork = false;

		// Token: 0x04001796 RID: 6038
		public static bool flagSaveImgToFtP = false;

		// Token: 0x04001797 RID: 6039
		private bool delteExternalFoldersisRunning = false;

		// Token: 0x04001798 RID: 6040
		private bool retrainMode = false;

		// Token: 0x04001799 RID: 6041
		private bool autoRetrainUndo = false;

		// Token: 0x0400179A RID: 6042
		private XRSImage lastRetrainImage;

		// Token: 0x0400179B RID: 6043
		private List<MaterialCurveElement> materialCurveOld = new List<MaterialCurveElement>();

		// Token: 0x0400179E RID: 6046
		private bool autoRetrainVisible;

		// Token: 0x0400179F RID: 6047
		private int counterForExternalImageSaving = 0;

		// Token: 0x040017A0 RID: 6048
		private UsermanagementControl _ctrlUsr;

		// Token: 0x040017A1 RID: 6049
		private bool resetImages = false;
	}
}
