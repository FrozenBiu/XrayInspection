using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Common;
using DatabaseConnection;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x0200008B RID: 139
	public class DataPlotsViewModel : INotifyPropertyChanged
	{
		// Token: 0x1700041B RID: 1051
		// (get) Token: 0x06000DA4 RID: 3492 RVA: 0x00054119 File Offset: 0x00052319
		// (set) Token: 0x06000DA5 RID: 3493 RVA: 0x00054121 File Offset: 0x00052321
		public int ColumnsForRejecter { get; set; }

		// Token: 0x1700041C RID: 1052
		// (get) Token: 0x06000DA6 RID: 3494 RVA: 0x0005412A File Offset: 0x0005232A
		// (set) Token: 0x06000DA7 RID: 3495 RVA: 0x00054132 File Offset: 0x00052332
		public RelayCommand CleanUpCommand { get; set; }

		// Token: 0x1700041D RID: 1053
		// (get) Token: 0x06000DA8 RID: 3496 RVA: 0x0005413B File Offset: 0x0005233B
		// (set) Token: 0x06000DA9 RID: 3497 RVA: 0x00054143 File Offset: 0x00052343
		public RelayCommand ZoomInYCommand { get; set; }

		// Token: 0x1700041E RID: 1054
		// (get) Token: 0x06000DAA RID: 3498 RVA: 0x0005414C File Offset: 0x0005234C
		// (set) Token: 0x06000DAB RID: 3499 RVA: 0x00054154 File Offset: 0x00052354
		public RelayCommand ZoomOutYCommand { get; set; }

		// Token: 0x1700041F RID: 1055
		// (get) Token: 0x06000DAC RID: 3500 RVA: 0x0005415D File Offset: 0x0005235D
		// (set) Token: 0x06000DAD RID: 3501 RVA: 0x00054165 File Offset: 0x00052365
		public BoundLocalizedText StrValveActivity { get; set; }

		// Token: 0x14000049 RID: 73
		// (add) Token: 0x06000DAE RID: 3502 RVA: 0x00054170 File Offset: 0x00052370
		// (remove) Token: 0x06000DAF RID: 3503 RVA: 0x000541A8 File Offset: 0x000523A8
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x17000420 RID: 1056
		// (get) Token: 0x06000DB0 RID: 3504 RVA: 0x000541DD File Offset: 0x000523DD
		// (set) Token: 0x06000DB1 RID: 3505 RVA: 0x000541E5 File Offset: 0x000523E5
		public BoundLocalizedText StrLimit { get; set; }

		// Token: 0x17000421 RID: 1057
		// (get) Token: 0x06000DB2 RID: 3506 RVA: 0x000541EE File Offset: 0x000523EE
		// (set) Token: 0x06000DB3 RID: 3507 RVA: 0x000541F6 File Offset: 0x000523F6
		public BoundLocalizedText StrTemperature { get; set; }

		// Token: 0x17000422 RID: 1058
		// (get) Token: 0x06000DB4 RID: 3508 RVA: 0x000541FF File Offset: 0x000523FF
		// (set) Token: 0x06000DB5 RID: 3509 RVA: 0x00054207 File Offset: 0x00052407
		public BoundLocalizedText StrOperatingHours { get; set; }

		// Token: 0x17000423 RID: 1059
		// (get) Token: 0x06000DB6 RID: 3510 RVA: 0x00054210 File Offset: 0x00052410
		// (set) Token: 0x06000DB7 RID: 3511 RVA: 0x00054218 File Offset: 0x00052418
		public BoundLocalizedText StrArcs { get; set; }

		// Token: 0x17000424 RID: 1060
		// (get) Token: 0x06000DB8 RID: 3512 RVA: 0x00054221 File Offset: 0x00052421
		// (set) Token: 0x06000DB9 RID: 3513 RVA: 0x00054229 File Offset: 0x00052429
		public BoundLocalizedText StrFilamentCurrent { get; set; }

		// Token: 0x17000425 RID: 1061
		// (get) Token: 0x06000DBA RID: 3514 RVA: 0x00054232 File Offset: 0x00052432
		// (set) Token: 0x06000DBB RID: 3515 RVA: 0x0005423A File Offset: 0x0005243A
		public BoundLocalizedText StrRejecter { get; set; }

		// Token: 0x17000426 RID: 1062
		// (get) Token: 0x06000DBC RID: 3516 RVA: 0x00054243 File Offset: 0x00052443
		// (set) Token: 0x06000DBD RID: 3517 RVA: 0x0005424B File Offset: 0x0005244B
		public BoundLocalizedText StrProductCounterGood { get; set; }

		// Token: 0x17000427 RID: 1063
		// (get) Token: 0x06000DBE RID: 3518 RVA: 0x00054254 File Offset: 0x00052454
		// (set) Token: 0x06000DBF RID: 3519 RVA: 0x0005425C File Offset: 0x0005245C
		public BoundLocalizedText StrProductCounterBad { get; set; }

		// Token: 0x17000428 RID: 1064
		// (get) Token: 0x06000DC0 RID: 3520 RVA: 0x00054265 File Offset: 0x00052465
		// (set) Token: 0x06000DC1 RID: 3521 RVA: 0x0005426D File Offset: 0x0005246D
		public BoundLocalizedText StrBadPixel { get; set; }

		// Token: 0x17000429 RID: 1065
		// (get) Token: 0x06000DC2 RID: 3522 RVA: 0x00054276 File Offset: 0x00052476
		// (set) Token: 0x06000DC3 RID: 3523 RVA: 0x0005427E File Offset: 0x0005247E
		public BoundLocalizedText StrSignalMonitoring { get; set; }

		// Token: 0x1700042A RID: 1066
		// (get) Token: 0x06000DC4 RID: 3524 RVA: 0x00054287 File Offset: 0x00052487
		// (set) Token: 0x06000DC5 RID: 3525 RVA: 0x0005428F File Offset: 0x0005248F
		public BoundLocalizedText StrSwitchOnCycles { get; set; }

		// Token: 0x1700042B RID: 1067
		// (get) Token: 0x06000DC6 RID: 3526 RVA: 0x00054298 File Offset: 0x00052498
		// (set) Token: 0x06000DC7 RID: 3527 RVA: 0x000542A0 File Offset: 0x000524A0
		public BoundLocalizedText StrPixelDeltaMax { get; set; }

		// Token: 0x1700042C RID: 1068
		// (get) Token: 0x06000DC8 RID: 3528 RVA: 0x000542A9 File Offset: 0x000524A9
		// (set) Token: 0x06000DC9 RID: 3529 RVA: 0x000542B1 File Offset: 0x000524B1
		public BoundLocalizedText StrTitleConveyor { get; set; }

		// Token: 0x1700042D RID: 1069
		// (get) Token: 0x06000DCA RID: 3530 RVA: 0x000542BA File Offset: 0x000524BA
		// (set) Token: 0x06000DCB RID: 3531 RVA: 0x000542C2 File Offset: 0x000524C2
		public BoundLocalizedText StrTitleSource { get; set; }

		// Token: 0x1700042E RID: 1070
		// (get) Token: 0x06000DCC RID: 3532 RVA: 0x000542CB File Offset: 0x000524CB
		// (set) Token: 0x06000DCD RID: 3533 RVA: 0x000542D3 File Offset: 0x000524D3
		public BoundLocalizedText StrTitleOperational { get; set; }

		// Token: 0x1700042F RID: 1071
		// (get) Token: 0x06000DCE RID: 3534 RVA: 0x000542DC File Offset: 0x000524DC
		// (set) Token: 0x06000DCF RID: 3535 RVA: 0x000542E4 File Offset: 0x000524E4
		public BoundLocalizedText StrTitleDetector { get; set; }

		// Token: 0x17000430 RID: 1072
		// (get) Token: 0x06000DD0 RID: 3536 RVA: 0x000542ED File Offset: 0x000524ED
		// (set) Token: 0x06000DD1 RID: 3537 RVA: 0x000542F5 File Offset: 0x000524F5
		public BoundLocalizedText StrTitleRejecter { get; set; }

		// Token: 0x17000431 RID: 1073
		// (get) Token: 0x06000DD2 RID: 3538 RVA: 0x000542FE File Offset: 0x000524FE
		// (set) Token: 0x06000DD3 RID: 3539 RVA: 0x00054306 File Offset: 0x00052506
		public BoundLocalizedText StrConveyorDistance { get; set; }

		// Token: 0x17000432 RID: 1074
		// (get) Token: 0x06000DD4 RID: 3540 RVA: 0x0005430F File Offset: 0x0005250F
		// (set) Token: 0x06000DD5 RID: 3541 RVA: 0x00054317 File Offset: 0x00052517
		public BoundLocalizedText StrBackgroundIndex { get; set; }

		// Token: 0x17000433 RID: 1075
		// (get) Token: 0x06000DD6 RID: 3542 RVA: 0x00054320 File Offset: 0x00052520
		// (set) Token: 0x06000DD7 RID: 3543 RVA: 0x00054328 File Offset: 0x00052528
		public BoundLocalizedText StrTime { get; set; }

		// Token: 0x17000434 RID: 1076
		// (get) Token: 0x06000DD8 RID: 3544 RVA: 0x00054331 File Offset: 0x00052531
		// (set) Token: 0x06000DD9 RID: 3545 RVA: 0x00054339 File Offset: 0x00052539
		public BoundLocalizedText StrThroughput { get; set; }

		// Token: 0x17000435 RID: 1077
		// (get) Token: 0x06000DDA RID: 3546 RVA: 0x00054342 File Offset: 0x00052542
		// (set) Token: 0x06000DDB RID: 3547 RVA: 0x0005434A File Offset: 0x0005254A
		public BoundLocalizedText StrReject { get; set; }

		// Token: 0x17000436 RID: 1078
		// (get) Token: 0x06000DDC RID: 3548 RVA: 0x00054353 File Offset: 0x00052553
		// (set) Token: 0x06000DDD RID: 3549 RVA: 0x0005435B File Offset: 0x0005255B
		public BoundLocalizedText StrMachine { get; set; }

		// Token: 0x17000437 RID: 1079
		// (get) Token: 0x06000DDE RID: 3550 RVA: 0x00054364 File Offset: 0x00052564
		// (set) Token: 0x06000DDF RID: 3551 RVA: 0x0005436C File Offset: 0x0005256C
		public BoundLocalizedText StrPerDay { get; set; }

		// Token: 0x17000438 RID: 1080
		// (get) Token: 0x06000DE0 RID: 3552 RVA: 0x00054375 File Offset: 0x00052575
		// (set) Token: 0x06000DE1 RID: 3553 RVA: 0x0005437D File Offset: 0x0005257D
		public BoundLocalizedText StrCounter { get; set; }

		// Token: 0x17000439 RID: 1081
		// (get) Token: 0x06000DE2 RID: 3554 RVA: 0x00054386 File Offset: 0x00052586
		// (set) Token: 0x06000DE3 RID: 3555 RVA: 0x0005438E File Offset: 0x0005258E
		public BoundLocalizedText StrOverview { get; set; }

		// Token: 0x1700043A RID: 1082
		// (get) Token: 0x06000DE4 RID: 3556 RVA: 0x00054397 File Offset: 0x00052597
		// (set) Token: 0x06000DE5 RID: 3557 RVA: 0x0005439F File Offset: 0x0005259F
		public BoundLocalizedText StrValve { get; set; }

		// Token: 0x1700043B RID: 1083
		// (get) Token: 0x06000DE6 RID: 3558 RVA: 0x000543A8 File Offset: 0x000525A8
		// (set) Token: 0x06000DE7 RID: 3559 RVA: 0x000543B0 File Offset: 0x000525B0
		public BoundLocalizedText StrSortingSwitchedOn { get; set; }

		// Token: 0x1700043C RID: 1084
		// (get) Token: 0x06000DE8 RID: 3560 RVA: 0x000543BC File Offset: 0x000525BC
		// (set) Token: 0x06000DE9 RID: 3561 RVA: 0x000543D4 File Offset: 0x000525D4
		public List<Point> Data1
		{
			get
			{
				return this.data1;
			}
			set
			{
				this.data1 = value;
				this.OnPropertyChanged("Data1");
			}
		}

		// Token: 0x1700043D RID: 1085
		// (get) Token: 0x06000DEA RID: 3562 RVA: 0x000543EC File Offset: 0x000525EC
		// (set) Token: 0x06000DEB RID: 3563 RVA: 0x00054404 File Offset: 0x00052604
		public List<Point> Data2
		{
			get
			{
				return this.data2;
			}
			set
			{
				this.data2 = value;
				this.OnPropertyChanged("Data2");
			}
		}

		// Token: 0x1700043E RID: 1086
		// (get) Token: 0x06000DEC RID: 3564 RVA: 0x0005441C File Offset: 0x0005261C
		// (set) Token: 0x06000DED RID: 3565 RVA: 0x00054434 File Offset: 0x00052634
		public List<Point> Data3
		{
			get
			{
				return this.data3;
			}
			set
			{
				this.data3 = value;
				this.OnPropertyChanged("Data3");
			}
		}

		// Token: 0x1700043F RID: 1087
		// (get) Token: 0x06000DEE RID: 3566 RVA: 0x0005444C File Offset: 0x0005264C
		// (set) Token: 0x06000DEF RID: 3567 RVA: 0x00054464 File Offset: 0x00052664
		public List<Point> Data4
		{
			get
			{
				return this.data4;
			}
			set
			{
				this.data4 = value;
				this.OnPropertyChanged("Data4");
			}
		}

		// Token: 0x17000440 RID: 1088
		// (get) Token: 0x06000DF0 RID: 3568 RVA: 0x0005447C File Offset: 0x0005267C
		// (set) Token: 0x06000DF1 RID: 3569 RVA: 0x00054494 File Offset: 0x00052694
		public List<Point> Data5
		{
			get
			{
				return this.data5;
			}
			set
			{
				this.data5 = value;
				this.OnPropertyChanged("Data5");
			}
		}

		// Token: 0x17000441 RID: 1089
		// (get) Token: 0x06000DF2 RID: 3570 RVA: 0x000544AC File Offset: 0x000526AC
		// (set) Token: 0x06000DF3 RID: 3571 RVA: 0x000544C4 File Offset: 0x000526C4
		public int MaxY
		{
			get
			{
				return this.maxY;
			}
			set
			{
				this.maxY = value;
				this.OnPropertyChanged("MaxY");
			}
		}

		// Token: 0x17000442 RID: 1090
		// (get) Token: 0x06000DF4 RID: 3572 RVA: 0x000544DC File Offset: 0x000526DC
		// (set) Token: 0x06000DF5 RID: 3573 RVA: 0x000544F4 File Offset: 0x000526F4
		public double ZoomFactorX
		{
			get
			{
				return this.zoomFactorX;
			}
			set
			{
				this.zoomFactorX = value;
				this.OnPropertyChanged("ZoomFactorX");
			}
		}

		// Token: 0x17000443 RID: 1091
		// (get) Token: 0x06000DF6 RID: 3574 RVA: 0x0005450C File Offset: 0x0005270C
		// (set) Token: 0x06000DF7 RID: 3575 RVA: 0x00054524 File Offset: 0x00052724
		public double ZoomFactorY
		{
			get
			{
				return this.zoomFactorY;
			}
			set
			{
				this.zoomFactorY = value;
				this.OnPropertyChanged("ZoomFactorY");
			}
		}

		// Token: 0x17000444 RID: 1092
		// (get) Token: 0x06000DF8 RID: 3576 RVA: 0x0005453C File Offset: 0x0005273C
		// (set) Token: 0x06000DF9 RID: 3577 RVA: 0x00054554 File Offset: 0x00052754
		public int TicksY
		{
			get
			{
				return this.ticksY;
			}
			set
			{
				this.ticksY = value;
				this.OnPropertyChanged("TicksY");
			}
		}

		// Token: 0x17000445 RID: 1093
		// (get) Token: 0x06000DFA RID: 3578 RVA: 0x0005456A File Offset: 0x0005276A
		// (set) Token: 0x06000DFB RID: 3579 RVA: 0x00054572 File Offset: 0x00052772
		public DateTime LastDate { get; set; }

		// Token: 0x17000446 RID: 1094
		// (get) Token: 0x06000DFC RID: 3580 RVA: 0x0005457B File Offset: 0x0005277B
		// (set) Token: 0x06000DFD RID: 3581 RVA: 0x00054583 File Offset: 0x00052783
		public DateTime FirstDate { get; set; }

		// Token: 0x17000447 RID: 1095
		// (get) Token: 0x06000DFE RID: 3582 RVA: 0x0005458C File Offset: 0x0005278C
		// (set) Token: 0x06000DFF RID: 3583 RVA: 0x00054594 File Offset: 0x00052794
		public DateTime CurrentDate1 { get; set; }

		// Token: 0x17000448 RID: 1096
		// (get) Token: 0x06000E00 RID: 3584 RVA: 0x0005459D File Offset: 0x0005279D
		// (set) Token: 0x06000E01 RID: 3585 RVA: 0x000545A5 File Offset: 0x000527A5
		public DateTime CurrentDate2 { get; set; }

		// Token: 0x17000449 RID: 1097
		// (get) Token: 0x06000E02 RID: 3586 RVA: 0x000545B0 File Offset: 0x000527B0
		// (set) Token: 0x06000E03 RID: 3587 RVA: 0x000545C8 File Offset: 0x000527C8
		public bool ZoomPanelVisible
		{
			get
			{
				return this.zoomPanelVisible;
			}
			set
			{
				this.zoomPanelVisible = value;
				this.OnPropertyChanged("ZoomPanelVisible");
			}
		}

		// Token: 0x1700044A RID: 1098
		// (get) Token: 0x06000E04 RID: 3588 RVA: 0x000545DE File Offset: 0x000527DE
		// (set) Token: 0x06000E05 RID: 3589 RVA: 0x000545E6 File Offset: 0x000527E6
		public int MaxYInit { get; set; }

		// Token: 0x1700044B RID: 1099
		// (get) Token: 0x06000E06 RID: 3590 RVA: 0x000545EF File Offset: 0x000527EF
		// (set) Token: 0x06000E07 RID: 3591 RVA: 0x000545F7 File Offset: 0x000527F7
		public int TicksYInit { get; set; }

		// Token: 0x1700044C RID: 1100
		// (get) Token: 0x06000E08 RID: 3592 RVA: 0x00054600 File Offset: 0x00052800
		// (set) Token: 0x06000E09 RID: 3593 RVA: 0x00054618 File Offset: 0x00052818
		public double TicksX
		{
			get
			{
				return this.ticksX;
			}
			set
			{
				this.ticksX = value;
				this.OnPropertyChanged("TicksX");
			}
		}

		// Token: 0x1700044D RID: 1101
		// (get) Token: 0x06000E0A RID: 3594 RVA: 0x00054630 File Offset: 0x00052830
		// (set) Token: 0x06000E0B RID: 3595 RVA: 0x00054648 File Offset: 0x00052848
		public double MaxX
		{
			get
			{
				return this.maxX;
			}
			set
			{
				this.maxX = value;
				this.OnPropertyChanged("MaxX");
			}
		}

		// Token: 0x1700044E RID: 1102
		// (get) Token: 0x06000E0C RID: 3596 RVA: 0x00054660 File Offset: 0x00052860
		// (set) Token: 0x06000E0D RID: 3597 RVA: 0x00054678 File Offset: 0x00052878
		public double TicksXMinor
		{
			get
			{
				return this.ticksXMinor;
			}
			set
			{
				this.ticksXMinor = value;
				this.OnPropertyChanged("TicksXMinor");
			}
		}

		// Token: 0x1700044F RID: 1103
		// (get) Token: 0x06000E0E RID: 3598 RVA: 0x00054690 File Offset: 0x00052890
		// (set) Token: 0x06000E0F RID: 3599 RVA: 0x000546A8 File Offset: 0x000528A8
		public string Unit
		{
			get
			{
				return this.unit;
			}
			set
			{
				this.unit = value;
				this.OnPropertyChanged("Unit");
			}
		}

		// Token: 0x17000450 RID: 1104
		// (get) Token: 0x06000E10 RID: 3600 RVA: 0x000546C0 File Offset: 0x000528C0
		// (set) Token: 0x06000E11 RID: 3601 RVA: 0x000546D8 File Offset: 0x000528D8
		public string NameData
		{
			get
			{
				return this.nameData;
			}
			set
			{
				this.nameData = value;
				this.OnPropertyChanged("NameData");
			}
		}

		// Token: 0x17000451 RID: 1105
		// (get) Token: 0x06000E12 RID: 3602 RVA: 0x000546F0 File Offset: 0x000528F0
		// (set) Token: 0x06000E13 RID: 3603 RVA: 0x00054708 File Offset: 0x00052908
		public string NameData1
		{
			get
			{
				return this.nameData1;
			}
			set
			{
				this.nameData1 = value;
				this.OnPropertyChanged("NameData1");
			}
		}

		// Token: 0x17000452 RID: 1106
		// (get) Token: 0x06000E14 RID: 3604 RVA: 0x00054720 File Offset: 0x00052920
		// (set) Token: 0x06000E15 RID: 3605 RVA: 0x00054738 File Offset: 0x00052938
		public string NameData2
		{
			get
			{
				return this.nameData2;
			}
			set
			{
				this.nameData2 = value;
				this.OnPropertyChanged("NameData2");
			}
		}

		// Token: 0x17000453 RID: 1107
		// (get) Token: 0x06000E16 RID: 3606 RVA: 0x00054750 File Offset: 0x00052950
		// (set) Token: 0x06000E17 RID: 3607 RVA: 0x00054768 File Offset: 0x00052968
		public string NameData3
		{
			get
			{
				return this.nameData3;
			}
			set
			{
				this.nameData3 = value;
				this.OnPropertyChanged("NameData3");
			}
		}

		// Token: 0x17000454 RID: 1108
		// (get) Token: 0x06000E18 RID: 3608 RVA: 0x00054780 File Offset: 0x00052980
		// (set) Token: 0x06000E19 RID: 3609 RVA: 0x00054798 File Offset: 0x00052998
		public string NameData4
		{
			get
			{
				return this.nameData4;
			}
			set
			{
				this.nameData4 = value;
				this.OnPropertyChanged("NameData4");
			}
		}

		// Token: 0x17000455 RID: 1109
		// (get) Token: 0x06000E1A RID: 3610 RVA: 0x000547B0 File Offset: 0x000529B0
		// (set) Token: 0x06000E1B RID: 3611 RVA: 0x000547C8 File Offset: 0x000529C8
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
				this.OnPropertyChanged("Name");
			}
		}

		// Token: 0x17000456 RID: 1110
		// (get) Token: 0x06000E1C RID: 3612 RVA: 0x000547E0 File Offset: 0x000529E0
		// (set) Token: 0x06000E1D RID: 3613 RVA: 0x000547F8 File Offset: 0x000529F8
		public string SelectionComboBox
		{
			get
			{
				return this.selectionComboBox;
			}
			set
			{
				this.selectionComboBox = value;
				this.NameData = this.selectionComboBox;
				this.OnPropertyChanged("SelectionComboBox");
			}
		}

		// Token: 0x17000457 RID: 1111
		// (get) Token: 0x06000E1E RID: 3614 RVA: 0x0005481C File Offset: 0x00052A1C
		// (set) Token: 0x06000E1F RID: 3615 RVA: 0x00054834 File Offset: 0x00052A34
		public List<string> DataComboBox
		{
			get
			{
				return this.dataComboBox;
			}
			set
			{
				this.dataComboBox = value;
				this.OnPropertyChanged("DataComboBox");
			}
		}

		// Token: 0x1400004A RID: 74
		// (add) Token: 0x06000E20 RID: 3616 RVA: 0x0005484C File Offset: 0x00052A4C
		// (remove) Token: 0x06000E21 RID: 3617 RVA: 0x00054884 File Offset: 0x00052A84
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action UpdateDataPlots;

		// Token: 0x17000458 RID: 1112
		// (get) Token: 0x06000E22 RID: 3618 RVA: 0x000548B9 File Offset: 0x00052AB9
		// (set) Token: 0x06000E23 RID: 3619 RVA: 0x000548C1 File Offset: 0x00052AC1
		public DateTime Date1 { get; set; }

		// Token: 0x17000459 RID: 1113
		// (get) Token: 0x06000E24 RID: 3620 RVA: 0x000548CA File Offset: 0x00052ACA
		// (set) Token: 0x06000E25 RID: 3621 RVA: 0x000548D2 File Offset: 0x00052AD2
		public DateTime Date2 { get; set; }

		// Token: 0x1700045A RID: 1114
		// (get) Token: 0x06000E26 RID: 3622 RVA: 0x000548DC File Offset: 0x00052ADC
		// (set) Token: 0x06000E27 RID: 3623 RVA: 0x000548F4 File Offset: 0x00052AF4
		public string TextDate1
		{
			get
			{
				return this.textDate1;
			}
			set
			{
				this.textDate1 = value;
				this.OnPropertyChanged("TextDate1");
			}
		}

		// Token: 0x1700045B RID: 1115
		// (get) Token: 0x06000E28 RID: 3624 RVA: 0x0005490C File Offset: 0x00052B0C
		// (set) Token: 0x06000E29 RID: 3625 RVA: 0x00054924 File Offset: 0x00052B24
		public string TextDate2
		{
			get
			{
				return this.textDate2;
			}
			set
			{
				this.textDate2 = value;
				this.OnPropertyChanged("TextDate2");
			}
		}

		// Token: 0x1700045C RID: 1116
		// (get) Token: 0x06000E2A RID: 3626 RVA: 0x0005493C File Offset: 0x00052B3C
		// (set) Token: 0x06000E2B RID: 3627 RVA: 0x00054954 File Offset: 0x00052B54
		public bool Col2
		{
			get
			{
				return this.col2;
			}
			set
			{
				this.col2 = value;
				this.OnPropertyChanged("Col2");
			}
		}

		// Token: 0x1700045D RID: 1117
		// (get) Token: 0x06000E2C RID: 3628 RVA: 0x0005496C File Offset: 0x00052B6C
		// (set) Token: 0x06000E2D RID: 3629 RVA: 0x00054984 File Offset: 0x00052B84
		public bool Col3
		{
			get
			{
				return this.col3;
			}
			set
			{
				this.col3 = value;
				this.OnPropertyChanged("Col3");
			}
		}

		// Token: 0x1700045E RID: 1118
		// (get) Token: 0x06000E2E RID: 3630 RVA: 0x0005499C File Offset: 0x00052B9C
		// (set) Token: 0x06000E2F RID: 3631 RVA: 0x000549B4 File Offset: 0x00052BB4
		public bool Col4
		{
			get
			{
				return this.col4;
			}
			set
			{
				this.col4 = value;
				this.OnPropertyChanged("Col4");
			}
		}

		// Token: 0x1700045F RID: 1119
		// (get) Token: 0x06000E30 RID: 3632 RVA: 0x000549CC File Offset: 0x00052BCC
		// (set) Token: 0x06000E31 RID: 3633 RVA: 0x000549E4 File Offset: 0x00052BE4
		public bool LimitVisible
		{
			get
			{
				return this.limitVisible;
			}
			set
			{
				this.limitVisible = value;
				this.OnPropertyChanged("LimitVisible");
			}
		}

		// Token: 0x17000460 RID: 1120
		// (get) Token: 0x06000E32 RID: 3634 RVA: 0x000549FC File Offset: 0x00052BFC
		// (set) Token: 0x06000E33 RID: 3635 RVA: 0x00054A14 File Offset: 0x00052C14
		public bool BarDiagramm
		{
			get
			{
				return this.barDiagramm;
			}
			set
			{
				this.barDiagramm = value;
				this.OnPropertyChanged("BarDiagramm");
			}
		}

		// Token: 0x06000E34 RID: 3636 RVA: 0x00054A2C File Offset: 0x00052C2C
		public DataPlotsViewModel(DBConnection dbConnection, ParameterHandling parameterHandling)
		{
			this.CleanUpCommand = new RelayCommand(new Action<object>(this.CleanUp));
			this.ZoomInYCommand = new RelayCommand(new Action<object>(this.ZoomInY));
			this.ZoomOutYCommand = new RelayCommand(new Action<object>(this.ZoomOutY));
			this.dbConnection = dbConnection;
			this.parameterHandling = parameterHandling;
			this.actualModeToSavePNG = "Sum";
			this.RegisterLocalizedText();
			this.ColumnsForRejecter = 1;
		}

		// Token: 0x06000E35 RID: 3637 RVA: 0x00054C14 File Offset: 0x00052E14
		public void setTicksY()
		{
			int num = 1;
			double num2 = (double)this.MaxY;
			bool flag = this.MaxY < 100;
			if (flag)
			{
				bool flag2 = this.MaxY <= 20;
				if (flag2)
				{
					this.TicksY = 1;
				}
				else
				{
					bool flag3 = this.MaxY <= 40;
					if (flag3)
					{
						this.TicksY = 2;
					}
					else
					{
						bool flag4 = this.MaxY <= 80;
						if (flag4)
						{
							this.TicksY = 4;
						}
						else
						{
							this.TicksY = 8;
						}
					}
				}
			}
			else
			{
				while (num2 / 10.0 > 10.0)
				{
					num2 /= 10.0;
					num++;
				}
				this.TicksY = (int)Math.Pow(10.0, (double)num);
				while (this.MaxY / this.TicksY > 10)
				{
					this.TicksY *= 2;
				}
			}
			bool flag5 = this.maxY == 24;
			if (flag5)
			{
				this.MaxY = 24;
			}
			else
			{
				this.MaxY += this.TicksY;
			}
			bool flag6 = this.setMaxYInit;
			if (flag6)
			{
				this.MaxYInit = this.MaxY;
				this.TicksYInit = this.TicksY;
				this.setMaxYInit = false;
			}
		}

		// Token: 0x06000E36 RID: 3638 RVA: 0x00054D6D File Offset: 0x00052F6D
		public void ZoomIn()
		{
			this.ZoomFactorX *= 2.0;
		}

		// Token: 0x06000E37 RID: 3639 RVA: 0x00054D87 File Offset: 0x00052F87
		public void ZoomOut()
		{
			this.ZoomFactorX /= 2.0;
		}

		// Token: 0x06000E38 RID: 3640 RVA: 0x00054DA1 File Offset: 0x00052FA1
		public void ZoomInY(object obj)
		{
			this.ZoomFactorY *= 2.0;
		}

		// Token: 0x06000E39 RID: 3641 RVA: 0x00054DBB File Offset: 0x00052FBB
		public void ZoomOutY(object obj)
		{
			this.ZoomFactorY /= 2.0;
		}

		// Token: 0x06000E3A RID: 3642 RVA: 0x00054DD8 File Offset: 0x00052FD8
		public void SaveActualGraphPNG(Grid chartGrid, Rect bounds)
		{
			this.ZoomPanelVisible = false;
			string text = "";
			string text2 = "";
			string text3 = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\DataPlotsMonitoring\\";
			string text4 = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_ff");
			bool flag = !Directory.Exists(text3);
			if (flag)
			{
				Directory.CreateDirectory(text3);
			}
			this.dbConnection.GetUsbDriveLetter(ref text);
			try
			{
				string str = string.Concat(new string[]
				{
					"DataPlot_",
					this.NameData,
					"_",
					text4,
					".png"
				});
				text2 = text + "\\DataPlotsMonitoring";
				bool flag2 = Directory.Exists(text);
				if (flag2)
				{
					bool flag3 = !Directory.Exists(text2);
					if (flag3)
					{
						Directory.CreateDirectory(text2);
					}
				}
				text3 = text3 + "\\" + str;
				text2 = text + "\\DataPlotsMonitoring\\" + str;
				FileInfo fileInfo = new FileInfo(text2);
				RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap((int)bounds.Width, (int)bounds.Height, 96.0, 96.0, PixelFormats.Pbgra32);
				DrawingVisual drawingVisual = new DrawingVisual();
				using (DrawingContext drawingContext = drawingVisual.RenderOpen())
				{
					VisualBrush brush = new VisualBrush(chartGrid);
					Pen pen = new Pen();
					drawingContext.DrawRectangle(brush, pen, new Rect(default(Point), bounds.Size));
				}
				renderTargetBitmap.Render(drawingVisual);
				FileStream fileStream = new FileStream(text3, FileMode.Create);
				new PngBitmapEncoder
				{
					Frames = 
					{
						BitmapFrame.Create(renderTargetBitmap)
					}
				}.Save(fileStream);
				fileStream.Close();
				bool flag4 = Directory.Exists(text + "\\");
				if (flag4)
				{
					fileStream = new FileStream(text2, FileMode.Create);
					new PngBitmapEncoder
					{
						Frames = 
						{
							BitmapFrame.Create(renderTargetBitmap)
						}
					}.Save(fileStream);
					fileStream.Close();
				}
				Dialog.Show(LocalizationManager.Localize("txtSuccess"), LocalizationManager.Localize("txtSaveSuccessful"), DialogStyle.Ok, DialogButtons.None, true);
			}
			catch (UnauthorizedAccessException)
			{
				Dialog.Show(LocalizationManager.Localize("txtError"), string.Concat(new string[]
				{
					LocalizationManager.Localize("msgPathAccess"),
					" \"",
					Path.GetDirectoryName(text2),
					"\" ",
					LocalizationManager.Localize("msgForbidden")
				}), DialogStyle.Error);
			}
			catch
			{
				Dialog.Show(LocalizationManager.Localize("txtError"), string.Concat(new string[]
				{
					LocalizationManager.Localize("msgPathAccess"),
					" \"",
					Path.GetDirectoryName(text2),
					"\" ",
					LocalizationManager.Localize("msgForbidden")
				}), DialogStyle.Error);
			}
			this.ZoomPanelVisible = true;
		}

		// Token: 0x06000E3B RID: 3643 RVA: 0x00055108 File Offset: 0x00053308
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x06000E3C RID: 3644 RVA: 0x00055138 File Offset: 0x00053338
		private void RegisterLocalizedText()
		{
			this.StrValveActivity = new BoundLocalizedText("msgValveActivity");
			this.StrTemperature = new BoundLocalizedText("txtTemperature");
			this.StrLimit = new BoundLocalizedText("txtLimitMonitoring");
			this.StrTitleSource = new BoundLocalizedText("parItemXRaySource");
			this.StrOperatingHours = new BoundLocalizedText("txtOperatingHours");
			this.StrArcs = new BoundLocalizedText("txtArcs");
			this.StrFilamentCurrent = new BoundLocalizedText("txtFilamentCurrent");
			this.StrSwitchOnCycles = new BoundLocalizedText("txtSwitchOnCycles");
			this.StrTitleRejecter = new BoundLocalizedText("txtRejecter");
			this.StrTitleOperational = new BoundLocalizedText("txtDeviceData");
			this.StrProductCounterGood = new BoundLocalizedText("txtProductCounterGood");
			this.StrProductCounterBad = new BoundLocalizedText("txtProductCounterBad");
			this.StrTitleDetector = new BoundLocalizedText("parItemDetector");
			this.StrBadPixel = new BoundLocalizedText("txtBadPixel");
			this.StrSignalMonitoring = new BoundLocalizedText("txtSignalMonitoring");
			this.StrPixelDeltaMax = new BoundLocalizedText("txtPixelDeltaMax");
			this.StrTitleConveyor = new BoundLocalizedText("txtConveyor");
			this.StrConveyorDistance = new BoundLocalizedText("txtConveyorDistance");
			this.StrBackgroundIndex = new BoundLocalizedText("txtBackgroundIndex");
			this.StrTime = new BoundLocalizedText("txtOnTime");
			this.StrThroughput = new BoundLocalizedText("txtProductCounterGood");
			this.StrReject = new BoundLocalizedText("txtProductCounterBad");
			this.StrMachine = new BoundLocalizedText("txtMachine");
			this.StrPerDay = new BoundLocalizedText("txtPerDay");
			this.StrCounter = new BoundLocalizedText("txtProductCounter");
			this.StrValve = new BoundLocalizedText("txtValve");
			this.StrOverview = new BoundLocalizedText("menuItemOverview");
			this.StrSortingSwitchedOn = new BoundLocalizedText("txtSortingSwitchedOn");
		}

		// Token: 0x06000E3D RID: 3645 RVA: 0x00055324 File Offset: 0x00053524
		public void DeregisterLocalizedText()
		{
			MainWindow.localizationManager.Deregister(this.StrValveActivity);
			MainWindow.localizationManager.Deregister(this.StrTemperature);
			MainWindow.localizationManager.Deregister(this.StrLimit);
			MainWindow.localizationManager.Deregister(this.StrTitleSource);
			MainWindow.localizationManager.Deregister(this.StrOperatingHours);
			MainWindow.localizationManager.Deregister(this.StrArcs);
			MainWindow.localizationManager.Deregister(this.StrFilamentCurrent);
			MainWindow.localizationManager.Deregister(this.StrSwitchOnCycles);
			MainWindow.localizationManager.Deregister(this.StrTitleRejecter);
			MainWindow.localizationManager.Deregister(this.StrTitleOperational);
			MainWindow.localizationManager.Deregister(this.StrProductCounterGood);
			MainWindow.localizationManager.Deregister(this.StrProductCounterBad);
			MainWindow.localizationManager.Deregister(this.StrTitleDetector);
			MainWindow.localizationManager.Deregister(this.StrBadPixel);
			MainWindow.localizationManager.Deregister(this.StrSignalMonitoring);
			MainWindow.localizationManager.Deregister(this.StrPixelDeltaMax);
			MainWindow.localizationManager.Deregister(this.StrTitleConveyor);
			MainWindow.localizationManager.Deregister(this.StrConveyorDistance);
			MainWindow.localizationManager.Deregister(this.StrBackgroundIndex);
			MainWindow.localizationManager.Deregister(this.StrTime);
			MainWindow.localizationManager.Deregister(this.StrThroughput);
			MainWindow.localizationManager.Deregister(this.StrReject);
			MainWindow.localizationManager.Deregister(this.StrMachine);
			MainWindow.localizationManager.Deregister(this.StrPerDay);
			MainWindow.localizationManager.Deregister(this.StrCounter);
			MainWindow.localizationManager.Deregister(this.StrValve);
			MainWindow.localizationManager.Deregister(this.StrOverview);
			MainWindow.localizationManager.Deregister(this.StrSortingSwitchedOn);
		}

		// Token: 0x06000E3E RID: 3646 RVA: 0x00055510 File Offset: 0x00053710
		private void CleanUp(object obj)
		{
			this.Data1.Clear();
			this.Data2.Clear();
			this.Data1 = null;
			this.Data2 = null;
			this.Data3.Clear();
			this.Data4.Clear();
			this.Data3 = null;
			this.Data4 = null;
			this.Data5.Clear();
			this.Data5 = null;
			this.DeregisterLocalizedText();
		}

		// Token: 0x06000E3F RID: 3647 RVA: 0x0005558C File Offset: 0x0005378C
		internal void AddOperational()
		{
			MachineType machineType = MachineType.RayconEX2A;
			this.dbConnection.GetMachineType(ref machineType);
			this.Name = this.StrMachine.LocalizedText;
			bool flag = machineType != MachineType.RayconEX2A;
			if (flag)
			{
				this.DataComboBox.Add(this.StrTemperature.LocalizedText);
			}
			this.DataComboBox.Add(this.StrTime.LocalizedText);
			MachineMode machineMode = MachineMode.Standard;
			this.dbConnection.GetMachineMode(ref machineMode);
			bool flag2 = machineMode != MachineMode.Bulk;
			if (flag2)
			{
				this.DataComboBox.Add(this.StrCounter.LocalizedText);
				this.DataComboBox.Add(this.StrThroughput.LocalizedText);
				this.DataComboBox.Add(this.StrReject.LocalizedText);
			}
		}

		// Token: 0x06000E40 RID: 3648 RVA: 0x00055658 File Offset: 0x00053858
		internal void AddXRay()
		{
			this.Name = this.StrTitleSource.LocalizedText;
			this.DataComboBox.Add(this.StrTemperature.LocalizedText);
			MachineMode machineMode = MachineMode.Standard;
			this.dbConnection.GetMachineMode(ref machineMode);
			bool flag = machineMode != MachineMode.Bulk && machineMode != MachineMode.BulkObject;
			if (flag)
			{
				this.DataComboBox.Add(this.StrArcs.LocalizedText);
			}
			this.DataComboBox.Add(this.StrFilamentCurrent.LocalizedText);
			this.DataComboBox.Add(this.StrSwitchOnCycles.LocalizedText);
			this.DataComboBox.Add(this.StrTime.LocalizedText);
		}

		// Token: 0x06000E41 RID: 3649 RVA: 0x0005570F File Offset: 0x0005390F
		internal void AddDetector()
		{
			this.Name = this.StrTitleDetector.LocalizedText;
			this.DataComboBox.Add(this.StrTemperature.LocalizedText);
		}

		// Token: 0x06000E42 RID: 3650 RVA: 0x0005573C File Offset: 0x0005393C
		internal void AddRejecter()
		{
			this.Name = this.StrTitleRejecter.LocalizedText;
			bool flag = this.parameterHandling.ReadPar(SVT.svtEnablePath) == "1";
			if (flag)
			{
				this.DataComboBox.Add(this.StrOverview.LocalizedText);
				this.DataComboBox.Add("SVT " + this.StrValveActivity.LocalizedText);
				int num = 0;
				this.dbConnection.GetNumValves(ref num);
				for (int i = 1; i <= num; i++)
				{
					this.DataComboBox.Add(this.StrValve.LocalizedText + " " + i.ToString());
				}
			}
			else
			{
				this.DataComboBox.Add(this.StrTitleRejecter.LocalizedText);
				this.DataComboBox.Add(this.StrTitleRejecter.LocalizedText + " 1");
				this.ColumnsForRejecter = 1;
				MachineType machineType = MachineType.MachineTypeUndefined;
				this.dbConnection.GetMachineType(ref machineType);
				MachineMode machineMode = MachineMode.Standard;
				this.dbConnection.GetMachineMode(ref machineMode);
				int num2 = 0;
				string tableName = "";
				this.dbConnection.GetCurrentProductTableName(ref tableName);
				this.dbConnection.GetFlapsCount(tableName, ref num2);
				bool flag2 = machineMode != MachineMode.BulkObject && (machineMode != MachineMode.Bulk || num2 != 1);
				if (flag2)
				{
					this.DataComboBox.Add(this.StrTitleRejecter.LocalizedText + " 2");
					this.ColumnsForRejecter = 2;
				}
				bool flag3 = machineType != MachineType.RayconEntry && machineMode != MachineMode.BulkObject && (machineMode != MachineMode.Bulk || num2 != 1);
				if (flag3)
				{
					string a = this.parameterHandling.ReadPar(Definition.ioSubversionPath);
					bool flag4 = a != "1" && a != "3" && a != "4";
					if (flag4)
					{
						this.ColumnsForRejecter = 4;
						this.DataComboBox.Add(this.StrTitleRejecter.LocalizedText + " 3");
						this.DataComboBox.Add(this.StrTitleRejecter.LocalizedText + " 4");
						bool flag5 = machineMode == MachineMode.Bulk;
						if (flag5)
						{
							this.DataComboBox.Add(this.StrValveActivity.LocalizedText);
							this.DataComboBox.Add(this.StrValveActivity.LocalizedText + " " + this.StrTitleRejecter.LocalizedText + " 1");
							this.DataComboBox.Add(this.StrValveActivity.LocalizedText + " " + this.StrTitleRejecter.LocalizedText + " 2");
							this.DataComboBox.Add(this.StrValveActivity.LocalizedText + " " + this.StrTitleRejecter.LocalizedText + " 3");
							this.DataComboBox.Add(this.StrValveActivity.LocalizedText + " " + this.StrTitleRejecter.LocalizedText + " 4");
						}
					}
				}
			}
		}

		// Token: 0x06000E43 RID: 3651 RVA: 0x00055A86 File Offset: 0x00053C86
		internal void AddConveyor()
		{
			this.Name = this.StrTitleConveyor.LocalizedText;
			this.DataComboBox.Add(this.StrTime.LocalizedText);
			this.SelectionComboBox = this.DataComboBox.First<string>();
		}

		// Token: 0x06000E44 RID: 3652 RVA: 0x00055AC4 File Offset: 0x00053CC4
		internal void DateClicked(int date)
		{
			bool flag = date == 1;
			if (flag)
			{
				this.date1 = true;
				this.dateDay = this.Date1.Day;
				this.dateMonth = this.Date1.Month;
				this.dateYear = this.Date1.Year;
			}
			else
			{
				this.date1 = false;
				this.dateDay = this.Date2.AddDays(-1.0).Day;
				this.dateMonth = this.Date2.Month;
				this.dateYear = this.Date2.Year;
			}
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtDay"), this.dateDay.ToString(), NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_InputDateDay;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x06000E45 RID: 3653 RVA: 0x00055BBC File Offset: 0x00053DBC
		private void numKeyboard_InputDateDay(object sender, KeyboardInputEventArgs e)
		{
			this.dateDay = int.Parse(e.Text);
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_InputDateDay;
			this.ChangeDateMonth();
		}

		// Token: 0x06000E46 RID: 3654 RVA: 0x00055BF0 File Offset: 0x00053DF0
		private void ChangeDateMonth()
		{
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtMonth"), this.dateMonth.ToString(), NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_InputDateMonth;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x06000E47 RID: 3655 RVA: 0x00055C43 File Offset: 0x00053E43
		private void numKeyboard_InputDateMonth(object sender, KeyboardInputEventArgs e)
		{
			this.dateMonth = int.Parse(e.Text);
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_InputDateMonth;
			this.ChangeDateYear();
		}

		// Token: 0x06000E48 RID: 3656 RVA: 0x00055C78 File Offset: 0x00053E78
		private void ChangeDateYear()
		{
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtYear"), this.dateYear.ToString(), NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_InputDateYear;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x06000E49 RID: 3657 RVA: 0x00055CCC File Offset: 0x00053ECC
		private void numKeyboard_InputDateYear(object sender, KeyboardInputEventArgs e)
		{
			this.dateYear = int.Parse(e.Text);
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_InputDateYear;
			int num = 180;
			int num2 = 6;
			bool flag = this.BarDiagramm;
			if (flag)
			{
				num = 30;
				num2 = 1;
			}
			bool flag2 = this.dateYear > 0 && this.dateMonth > 0 && this.dateDay > 0;
			if (flag2)
			{
				try
				{
					DateTime dateTime = new DateTime(this.dateYear, this.dateMonth, this.dateDay);
					bool flag3 = this.SelectionComboBox.Equals(this.StrOverview.LocalizedText);
					if (flag3)
					{
						this.Date1 = dateTime;
						this.Date2 = dateTime;
						bool flag4 = this.Date1 < this.FirstDate.Date;
						if (flag4)
						{
							this.Date1 = this.FirstDate.Date;
							this.Date2 = this.Date1;
						}
						else
						{
							bool flag5 = this.Date2 > this.LastDate.Date;
							if (flag5)
							{
								this.Date2 = this.LastDate.Date;
								this.Date1 = this.Date2;
							}
						}
					}
					else
					{
						bool flag6 = this.date1;
						if (flag6)
						{
							this.TextDate1 = dateTime.ToString("yyyy-MM-dd");
							bool flag7 = dateTime < this.FirstDate;
							if (flag7)
							{
								dateTime = this.FirstDate;
							}
							bool flag8 = dateTime > this.LastDate;
							if (flag8)
							{
								dateTime = this.LastDate;
							}
							this.Date1 = dateTime;
							bool flag9 = (this.Date2 - dateTime).TotalDays > (double)num || this.Date2 < dateTime;
							if (flag9)
							{
								this.Date2 = this.Date1.AddMonths(num2);
								bool flag10 = this.Date2 > this.LastDate;
								if (flag10)
								{
									this.Date2 = this.LastDate;
								}
							}
						}
						else
						{
							this.TextDate2 = dateTime.ToString("yyyy-MM-dd");
							bool flag11 = dateTime > this.LastDate;
							if (flag11)
							{
								dateTime = this.LastDate;
							}
							bool flag12 = dateTime < this.FirstDate;
							if (flag12)
							{
								dateTime = this.FirstDate;
							}
							this.Date2 = dateTime;
							bool flag13 = (dateTime - this.Date1).TotalDays > (double)num || this.Date1 > dateTime;
							if (flag13)
							{
								bool flag14 = dateTime > this.FirstDate;
								if (flag14)
								{
									this.Date1 = this.Date2.AddMonths(-num2);
									bool flag15 = this.Date1 < this.FirstDate;
									if (flag15)
									{
										this.Date1 = this.FirstDate;
									}
								}
							}
						}
					}
					this.OnUpdateDataPlots();
				}
				catch
				{
				}
			}
		}

		// Token: 0x06000E4A RID: 3658 RVA: 0x00055FFC File Offset: 0x000541FC
		protected virtual void OnUpdateDataPlots()
		{
			Action updateDataPlots = this.UpdateDataPlots;
			bool flag = updateDataPlots != null;
			if (flag)
			{
				updateDataPlots();
			}
		}

		// Token: 0x04000903 RID: 2307
		private const string TypeRaycon_EX1 = "Raycon EX1";

		// Token: 0x04000904 RID: 2308
		private const string TypeRaycon_DPlus_MX = "Raycon D+ MX";

		// Token: 0x04000905 RID: 2309
		private const string TypeRaycon_DPlus_HX = "Raycon D+ HX";

		// Token: 0x04000906 RID: 2310
		private const string SubTypeStandard = "Standard";

		// Token: 0x04000907 RID: 2311
		private const string SubTypeLW = "LW";

		// Token: 0x04000908 RID: 2312
		private const string SubTypeBulk = "Bulk";

		// Token: 0x04000909 RID: 2313
		private const string SubTypeLWHy = "LW Hy";

		// Token: 0x0400090A RID: 2314
		private const string SubTypeBulkHy = "Bulk Hy";

		// Token: 0x0400090B RID: 2315
		private const string SensorsStandard = "Standard";

		// Token: 0x0400090C RID: 2316
		private const string SensorsHighlevel = "HighLevel";

		// Token: 0x0400090D RID: 2317
		private const string SensorsAdvanced = "Advanced";

		// Token: 0x04000930 RID: 2352
		private List<Point> data1 = new List<Point>();

		// Token: 0x04000931 RID: 2353
		private List<Point> data2 = new List<Point>();

		// Token: 0x04000932 RID: 2354
		private List<Point> data3 = new List<Point>();

		// Token: 0x04000933 RID: 2355
		private List<Point> data4 = new List<Point>();

		// Token: 0x04000934 RID: 2356
		private List<Point> data5 = new List<Point>();

		// Token: 0x04000935 RID: 2357
		private int maxY = 10;

		// Token: 0x04000936 RID: 2358
		private double zoomFactorX = 1.0;

		// Token: 0x04000937 RID: 2359
		private double zoomFactorY = 1.0;

		// Token: 0x04000938 RID: 2360
		private int ticksY = 10;

		// Token: 0x0400093D RID: 2365
		private bool zoomPanelVisible = true;

		// Token: 0x0400093E RID: 2366
		public string actualModeToSavePNG = "";

		// Token: 0x0400093F RID: 2367
		public bool setMaxYInit = true;

		// Token: 0x04000942 RID: 2370
		private double ticksX = 10.0;

		// Token: 0x04000943 RID: 2371
		private double maxX = 10.0;

		// Token: 0x04000944 RID: 2372
		private double ticksXMinor = 10.0;

		// Token: 0x04000945 RID: 2373
		private string unit = "";

		// Token: 0x04000946 RID: 2374
		private string nameData = "";

		// Token: 0x04000947 RID: 2375
		private string nameData1 = "";

		// Token: 0x04000948 RID: 2376
		private string nameData2 = "";

		// Token: 0x04000949 RID: 2377
		private string nameData3 = "";

		// Token: 0x0400094A RID: 2378
		private string nameData4 = "";

		// Token: 0x0400094B RID: 2379
		private string name = "";

		// Token: 0x0400094C RID: 2380
		private string selectionComboBox = "";

		// Token: 0x0400094D RID: 2381
		private List<string> dataComboBox = new List<string>();

		// Token: 0x0400094E RID: 2382
		private int dateDay = 0;

		// Token: 0x0400094F RID: 2383
		private int dateYear = 0;

		// Token: 0x04000950 RID: 2384
		private int dateMonth = 0;

		// Token: 0x04000951 RID: 2385
		private bool date1 = false;

		// Token: 0x04000955 RID: 2389
		private string textDate1 = "";

		// Token: 0x04000956 RID: 2390
		private string textDate2 = "";

		// Token: 0x04000957 RID: 2391
		private NumKeyboard numKeyboard;

		// Token: 0x04000958 RID: 2392
		private DBConnection dbConnection;

		// Token: 0x04000959 RID: 2393
		private bool col2 = false;

		// Token: 0x0400095A RID: 2394
		private bool col3 = false;

		// Token: 0x0400095B RID: 2395
		private bool col4 = false;

		// Token: 0x0400095C RID: 2396
		private bool limitVisible = false;

		// Token: 0x0400095D RID: 2397
		private bool barDiagramm = false;

		// Token: 0x0400095E RID: 2398
		private ParameterHandling parameterHandling;
	}
}
