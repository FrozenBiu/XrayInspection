using System;
using DatabaseConnection;

namespace GUI_XRS
{
	// Token: 0x0200010A RID: 266
	public class MachineErrors
	{
		// Token: 0x17000762 RID: 1890
		// (get) Token: 0x060018E7 RID: 6375 RVA: 0x00094820 File Offset: 0x00092A20
		// (set) Token: 0x060018E8 RID: 6376 RVA: 0x00094838 File Offset: 0x00092A38
		public bool ImagePortCommError
		{
			get
			{
				return this.imagePortCommError;
			}
			set
			{
				bool flag = this.imagePortCommError != value;
				if (flag)
				{
					this.imagePortCommError = value;
					if (value)
					{
						this.IncreaseErrors();
					}
				}
			}
		}

		// Token: 0x17000763 RID: 1891
		// (get) Token: 0x060018E9 RID: 6377 RVA: 0x0009486C File Offset: 0x00092A6C
		// (set) Token: 0x060018EA RID: 6378 RVA: 0x00094884 File Offset: 0x00092A84
		public bool ParamPortCommError
		{
			get
			{
				return this.paramPortCommError;
			}
			set
			{
				bool flag = this.paramPortCommError != value;
				if (flag)
				{
					this.paramPortCommError = value;
					if (value)
					{
						this.IncreaseErrors();
					}
				}
			}
		}

		// Token: 0x17000764 RID: 1892
		// (get) Token: 0x060018EB RID: 6379 RVA: 0x000948B8 File Offset: 0x00092AB8
		// (set) Token: 0x060018EC RID: 6380 RVA: 0x000948D0 File Offset: 0x00092AD0
		public bool ImageProcPcOffline
		{
			get
			{
				return this.imageProcPcOffline;
			}
			set
			{
				bool flag = this.imageProcPcOffline != value;
				if (flag)
				{
					this.imageProcPcOffline = value;
					if (value)
					{
						this.IncreaseErrors();
					}
				}
			}
		}

		// Token: 0x17000765 RID: 1893
		// (get) Token: 0x060018ED RID: 6381 RVA: 0x00094904 File Offset: 0x00092B04
		// (set) Token: 0x060018EE RID: 6382 RVA: 0x0009491C File Offset: 0x00092B1C
		public bool Detector
		{
			get
			{
				return this.detector;
			}
			set
			{
				bool flag = this.detector != value;
				if (flag)
				{
					this.detector = value;
					if (value)
					{
						this.IncreaseErrors();
					}
				}
			}
		}

		// Token: 0x17000766 RID: 1894
		// (get) Token: 0x060018EF RID: 6383 RVA: 0x00094950 File Offset: 0x00092B50
		// (set) Token: 0x060018F0 RID: 6384 RVA: 0x00094968 File Offset: 0x00092B68
		public bool XRay
		{
			get
			{
				return this.xRay;
			}
			set
			{
				bool flag = this.xRay != value;
				if (flag)
				{
					this.xRay = value;
					if (value)
					{
						this.IncreaseErrors();
					}
				}
			}
		}

		// Token: 0x17000767 RID: 1895
		// (get) Token: 0x060018F1 RID: 6385 RVA: 0x0009499C File Offset: 0x00092B9C
		// (set) Token: 0x060018F2 RID: 6386 RVA: 0x000949B4 File Offset: 0x00092BB4
		public bool Calibration
		{
			get
			{
				return this.calibration;
			}
			set
			{
				bool flag = this.calibration != value;
				if (flag)
				{
					this.calibration = value;
					if (value)
					{
						this.IncreaseErrors();
					}
				}
			}
		}

		// Token: 0x17000768 RID: 1896
		// (get) Token: 0x060018F3 RID: 6387 RVA: 0x000949E8 File Offset: 0x00092BE8
		// (set) Token: 0x060018F4 RID: 6388 RVA: 0x00094A00 File Offset: 0x00092C00
		public bool HELEDifferenceforDual
		{
			get
			{
				return this.heleDifferenceforDual;
			}
			set
			{
				bool flag = this.heleDifferenceforDual != value;
				if (flag)
				{
					this.heleDifferenceforDual = value;
					if (value)
					{
						this.IncreaseErrors();
					}
				}
			}
		}

		// Token: 0x17000769 RID: 1897
		// (get) Token: 0x060018F5 RID: 6389 RVA: 0x00094A34 File Offset: 0x00092C34
		// (set) Token: 0x060018F6 RID: 6390 RVA: 0x00094A4C File Offset: 0x00092C4C
		public bool SVTComm
		{
			get
			{
				return this.sVTComm;
			}
			set
			{
				bool flag = this.sVTComm != value;
				if (flag)
				{
					this.sVTComm = value;
					if (value)
					{
						this.IncreaseErrors();
					}
				}
			}
		}

		// Token: 0x1700076A RID: 1898
		// (get) Token: 0x060018F7 RID: 6391 RVA: 0x00094A80 File Offset: 0x00092C80
		// (set) Token: 0x060018F8 RID: 6392 RVA: 0x00094A98 File Offset: 0x00092C98
		public bool SVTStatus
		{
			get
			{
				return this.sVTStatus;
			}
			set
			{
				bool flag = this.sVTStatus != value;
				if (flag)
				{
					this.sVTStatus = value;
					if (value)
					{
						this.IncreaseErrors();
					}
				}
			}
		}

		// Token: 0x1700076B RID: 1899
		// (get) Token: 0x060018F9 RID: 6393 RVA: 0x00094ACC File Offset: 0x00092CCC
		// (set) Token: 0x060018FA RID: 6394 RVA: 0x00094AE4 File Offset: 0x00092CE4
		public bool ImageDelay
		{
			get
			{
				return this.imageDelay;
			}
			set
			{
				bool flag = this.imageDelay != value;
				if (flag)
				{
					this.imageDelay = value;
					if (value)
					{
						this.IncreaseErrors();
					}
				}
			}
		}

		// Token: 0x1700076C RID: 1900
		// (get) Token: 0x060018FB RID: 6395 RVA: 0x00094B18 File Offset: 0x00092D18
		// (set) Token: 0x060018FC RID: 6396 RVA: 0x00094B30 File Offset: 0x00092D30
		public bool MemoryUsage
		{
			get
			{
				return this.memoryUsage;
			}
			set
			{
				bool flag = this.memoryUsage != value;
				if (flag)
				{
					this.memoryUsage = value;
					if (value)
					{
						this.IncreaseErrors();
					}
				}
			}
		}

		// Token: 0x1700076D RID: 1901
		// (get) Token: 0x060018FD RID: 6397 RVA: 0x00094B64 File Offset: 0x00092D64
		// (set) Token: 0x060018FE RID: 6398 RVA: 0x00094B7C File Offset: 0x00092D7C
		public bool IOComm
		{
			get
			{
				return this.ioComm;
			}
			set
			{
				bool flag = this.ioComm != value;
				if (flag)
				{
					this.ioComm = value;
					if (value)
					{
						this.IncreaseErrors();
					}
				}
			}
		}

		// Token: 0x1700076E RID: 1902
		// (get) Token: 0x060018FF RID: 6399 RVA: 0x00094BB0 File Offset: 0x00092DB0
		// (set) Token: 0x06001900 RID: 6400 RVA: 0x00094BC8 File Offset: 0x00092DC8
		public bool WAGOCode
		{
			get
			{
				return this.wAGOCode;
			}
			set
			{
				bool flag = this.wAGOCode != value;
				if (flag)
				{
					this.wAGOCode = value;
					if (value)
					{
						this.IncreaseErrors();
					}
				}
			}
		}

		// Token: 0x1700076F RID: 1903
		// (get) Token: 0x06001901 RID: 6401 RVA: 0x00094BFC File Offset: 0x00092DFC
		// (set) Token: 0x06001902 RID: 6402 RVA: 0x00094C14 File Offset: 0x00092E14
		public bool BinFull1
		{
			get
			{
				return this.binFull1;
			}
			set
			{
				bool flag = this.binFull1 != value;
				if (flag)
				{
					this.binFull1 = value;
					if (value)
					{
						this.IncreaseErrors();
					}
				}
			}
		}

		// Token: 0x17000770 RID: 1904
		// (get) Token: 0x06001903 RID: 6403 RVA: 0x00094C48 File Offset: 0x00092E48
		// (set) Token: 0x06001904 RID: 6404 RVA: 0x00094C60 File Offset: 0x00092E60
		public bool BinFull2
		{
			get
			{
				return this.binFull2;
			}
			set
			{
				bool flag = this.binFull2 != value;
				if (flag)
				{
					this.binFull2 = value;
					if (value)
					{
						this.IncreaseErrors();
					}
				}
			}
		}

		// Token: 0x17000771 RID: 1905
		// (get) Token: 0x06001905 RID: 6405 RVA: 0x00094C94 File Offset: 0x00092E94
		// (set) Token: 0x06001906 RID: 6406 RVA: 0x00094CAC File Offset: 0x00092EAC
		public bool BinFull3
		{
			get
			{
				return this.binFull3;
			}
			set
			{
				bool flag = this.binFull3 != value;
				if (flag)
				{
					this.binFull3 = value;
					if (value)
					{
						this.IncreaseErrors();
					}
				}
			}
		}

		// Token: 0x17000772 RID: 1906
		// (get) Token: 0x06001907 RID: 6407 RVA: 0x00094CE0 File Offset: 0x00092EE0
		// (set) Token: 0x06001908 RID: 6408 RVA: 0x00094CF8 File Offset: 0x00092EF8
		public bool BinFull4
		{
			get
			{
				return this.binFull4;
			}
			set
			{
				bool flag = this.binFull4 != value;
				if (flag)
				{
					this.binFull4 = value;
					if (value)
					{
						this.IncreaseErrors();
					}
				}
			}
		}

		// Token: 0x17000773 RID: 1907
		// (get) Token: 0x06001909 RID: 6409 RVA: 0x00094D2C File Offset: 0x00092F2C
		// (set) Token: 0x0600190A RID: 6410 RVA: 0x00094D44 File Offset: 0x00092F44
		public bool MissingObjects1
		{
			get
			{
				return this.missingObjects1;
			}
			set
			{
				bool flag = this.missingObjects1 != value;
				if (flag)
				{
					this.missingObjects1 = value;
					if (value)
					{
						this.IncreaseErrors();
					}
				}
			}
		}

		// Token: 0x17000774 RID: 1908
		// (get) Token: 0x0600190B RID: 6411 RVA: 0x00094D78 File Offset: 0x00092F78
		// (set) Token: 0x0600190C RID: 6412 RVA: 0x00094D90 File Offset: 0x00092F90
		public bool MissingObjects2
		{
			get
			{
				return this.missingObjects2;
			}
			set
			{
				bool flag = this.missingObjects2 != value;
				if (flag)
				{
					this.missingObjects2 = value;
					if (value)
					{
						this.IncreaseErrors();
					}
				}
			}
		}

		// Token: 0x17000775 RID: 1909
		// (get) Token: 0x0600190D RID: 6413 RVA: 0x00094DC4 File Offset: 0x00092FC4
		// (set) Token: 0x0600190E RID: 6414 RVA: 0x00094DDC File Offset: 0x00092FDC
		public bool MissingObjects3
		{
			get
			{
				return this.missingObjects3;
			}
			set
			{
				bool flag = this.missingObjects3 != value;
				if (flag)
				{
					this.missingObjects3 = value;
					if (value)
					{
						this.IncreaseErrors();
					}
				}
			}
		}

		// Token: 0x17000776 RID: 1910
		// (get) Token: 0x0600190F RID: 6415 RVA: 0x00094E10 File Offset: 0x00093010
		// (set) Token: 0x06001910 RID: 6416 RVA: 0x00094E28 File Offset: 0x00093028
		public bool MissingObjects4
		{
			get
			{
				return this.missingObjects4;
			}
			set
			{
				bool flag = this.missingObjects4 != value;
				if (flag)
				{
					this.missingObjects4 = value;
					if (value)
					{
						this.IncreaseErrors();
					}
				}
			}
		}

		// Token: 0x17000777 RID: 1911
		// (get) Token: 0x06001911 RID: 6417 RVA: 0x00094E5C File Offset: 0x0009305C
		// (set) Token: 0x06001912 RID: 6418 RVA: 0x00094E74 File Offset: 0x00093074
		public bool XRayOnLamp
		{
			get
			{
				return this.xRayOnLamp;
			}
			set
			{
				bool flag = this.xRayOnLamp != value;
				if (flag)
				{
					this.xRayOnLamp = value;
					if (value)
					{
						this.IncreaseErrors();
					}
				}
			}
		}

		// Token: 0x17000778 RID: 1912
		// (get) Token: 0x06001913 RID: 6419 RVA: 0x00094EA8 File Offset: 0x000930A8
		// (set) Token: 0x06001914 RID: 6420 RVA: 0x00094EC0 File Offset: 0x000930C0
		public bool Cooling
		{
			get
			{
				return this.cooling;
			}
			set
			{
				bool flag = this.cooling != value;
				if (flag)
				{
					this.cooling = value;
					if (value)
					{
						this.IncreaseErrors();
					}
				}
			}
		}

		// Token: 0x17000779 RID: 1913
		// (get) Token: 0x06001915 RID: 6421 RVA: 0x00094EF4 File Offset: 0x000930F4
		// (set) Token: 0x06001916 RID: 6422 RVA: 0x00094F0C File Offset: 0x0009310C
		public bool LowAirPressure
		{
			get
			{
				return this.lowAirPressure;
			}
			set
			{
				bool flag = this.lowAirPressure != value;
				if (flag)
				{
					this.lowAirPressure = value;
					if (value)
					{
						this.IncreaseErrors();
					}
				}
			}
		}

		// Token: 0x1700077A RID: 1914
		// (get) Token: 0x06001917 RID: 6423 RVA: 0x00094F40 File Offset: 0x00093140
		// (set) Token: 0x06001918 RID: 6424 RVA: 0x00094F58 File Offset: 0x00093158
		public bool Motor
		{
			get
			{
				return this.motor;
			}
			set
			{
				bool flag = this.motor != value;
				if (flag)
				{
					this.motor = value;
					if (value)
					{
						this.IncreaseErrors();
					}
				}
			}
		}

		// Token: 0x1700077B RID: 1915
		// (get) Token: 0x06001919 RID: 6425 RVA: 0x00094F8C File Offset: 0x0009318C
		// (set) Token: 0x0600191A RID: 6426 RVA: 0x00094FA4 File Offset: 0x000931A4
		public bool Temperature0
		{
			get
			{
				return this.temperature0;
			}
			set
			{
				bool flag = this.temperature0 != value;
				if (flag)
				{
					this.temperature0 = value;
					if (value)
					{
						this.IncreaseErrors();
					}
				}
			}
		}

		// Token: 0x1700077C RID: 1916
		// (get) Token: 0x0600191B RID: 6427 RVA: 0x00094FD8 File Offset: 0x000931D8
		// (set) Token: 0x0600191C RID: 6428 RVA: 0x00094FF0 File Offset: 0x000931F0
		public bool Temperature1
		{
			get
			{
				return this.temperature1;
			}
			set
			{
				bool flag = this.temperature1 != value;
				if (flag)
				{
					this.temperature1 = value;
					if (value)
					{
						this.IncreaseErrors();
					}
				}
			}
		}

		// Token: 0x1700077D RID: 1917
		// (get) Token: 0x0600191D RID: 6429 RVA: 0x00095024 File Offset: 0x00093224
		// (set) Token: 0x0600191E RID: 6430 RVA: 0x0009503C File Offset: 0x0009323C
		public bool Voltage12V
		{
			get
			{
				return this.voltage12V;
			}
			set
			{
				bool flag = this.voltage12V != value;
				if (flag)
				{
					this.voltage12V = value;
					if (value)
					{
						this.IncreaseErrors();
					}
				}
			}
		}

		// Token: 0x1700077E RID: 1918
		// (get) Token: 0x0600191F RID: 6431 RVA: 0x00095070 File Offset: 0x00093270
		// (set) Token: 0x06001920 RID: 6432 RVA: 0x00095088 File Offset: 0x00093288
		public bool Voltage24V
		{
			get
			{
				return this.voltage24V;
			}
			set
			{
				bool flag = this.voltage24V != value;
				if (flag)
				{
					this.voltage24V = value;
					if (value)
					{
						this.IncreaseErrors();
					}
				}
			}
		}

		// Token: 0x1700077F RID: 1919
		// (get) Token: 0x06001921 RID: 6433 RVA: 0x000950BC File Offset: 0x000932BC
		// (set) Token: 0x06001922 RID: 6434 RVA: 0x000950D4 File Offset: 0x000932D4
		public bool LightBarrier1
		{
			get
			{
				return this.lightBarrier1;
			}
			set
			{
				bool flag = this.lightBarrier1 != value;
				if (flag)
				{
					this.lightBarrier1 = value;
					if (value)
					{
						this.IncreaseErrors();
					}
				}
			}
		}

		// Token: 0x17000780 RID: 1920
		// (get) Token: 0x06001923 RID: 6435 RVA: 0x00095108 File Offset: 0x00093308
		// (set) Token: 0x06001924 RID: 6436 RVA: 0x00095120 File Offset: 0x00093320
		public bool LightBarrier2
		{
			get
			{
				return this.lightBarrier2;
			}
			set
			{
				bool flag = this.lightBarrier2 != value;
				if (flag)
				{
					this.lightBarrier2 = value;
					if (value)
					{
						this.IncreaseErrors();
					}
				}
			}
		}

		// Token: 0x17000781 RID: 1921
		// (get) Token: 0x06001925 RID: 6437 RVA: 0x00095154 File Offset: 0x00093354
		// (set) Token: 0x06001926 RID: 6438 RVA: 0x0009516C File Offset: 0x0009336C
		public bool LightBarrier3
		{
			get
			{
				return this.lightBarrier3;
			}
			set
			{
				bool flag = this.lightBarrier3 != value;
				if (flag)
				{
					this.lightBarrier3 = value;
					if (value)
					{
						this.IncreaseErrors();
					}
				}
			}
		}

		// Token: 0x17000782 RID: 1922
		// (get) Token: 0x06001927 RID: 6439 RVA: 0x000951A0 File Offset: 0x000933A0
		// (set) Token: 0x06001928 RID: 6440 RVA: 0x000951B8 File Offset: 0x000933B8
		public bool LightBarrier4
		{
			get
			{
				return this.lightBarrier4;
			}
			set
			{
				bool flag = this.lightBarrier4 != value;
				if (flag)
				{
					this.lightBarrier4 = value;
					if (value)
					{
						this.IncreaseErrors();
					}
				}
			}
		}

		// Token: 0x17000783 RID: 1923
		// (get) Token: 0x06001929 RID: 6441 RVA: 0x000951EC File Offset: 0x000933EC
		// (set) Token: 0x0600192A RID: 6442 RVA: 0x00095204 File Offset: 0x00093404
		public bool AuditFailed
		{
			get
			{
				return this.auditFailed;
			}
			set
			{
				bool flag = this.auditFailed != value;
				if (flag)
				{
					this.auditFailed = value;
					if (value)
					{
						this.IncreaseErrors();
					}
				}
			}
		}

		// Token: 0x17000784 RID: 1924
		// (get) Token: 0x0600192B RID: 6443 RVA: 0x00095238 File Offset: 0x00093438
		// (set) Token: 0x0600192C RID: 6444 RVA: 0x00095250 File Offset: 0x00093450
		public bool XrayMotorError
		{
			get
			{
				return this.xrayMotorError;
			}
			set
			{
				bool flag = this.xrayMotorError != value;
				if (flag)
				{
					this.xrayMotorError = value;
					if (value)
					{
						this.IncreaseErrors();
					}
				}
			}
		}

		// Token: 0x17000785 RID: 1925
		// (get) Token: 0x0600192D RID: 6445 RVA: 0x00095284 File Offset: 0x00093484
		// (set) Token: 0x0600192E RID: 6446 RVA: 0x0009529C File Offset: 0x0009349C
		public bool HighLevelSensorsError
		{
			get
			{
				return this.highLevelSensorsError;
			}
			set
			{
				bool flag = this.highLevelSensorsError != value;
				if (flag)
				{
					this.highLevelSensorsError = value;
					if (value)
					{
						this.IncreaseErrors();
					}
				}
			}
		}

		// Token: 0x17000786 RID: 1926
		// (get) Token: 0x0600192F RID: 6447 RVA: 0x000952D0 File Offset: 0x000934D0
		// (set) Token: 0x06001930 RID: 6448 RVA: 0x000952E8 File Offset: 0x000934E8
		public bool BinMissingError
		{
			get
			{
				return this.binMissingError;
			}
			set
			{
				bool flag = this.binMissingError != value;
				if (flag)
				{
					this.binMissingError = value;
					if (value)
					{
						this.IncreaseErrors();
					}
				}
			}
		}

		// Token: 0x17000787 RID: 1927
		// (get) Token: 0x06001931 RID: 6449 RVA: 0x0009531C File Offset: 0x0009351C
		// (set) Token: 0x06001932 RID: 6450 RVA: 0x00095334 File Offset: 0x00093534
		public bool TooManyRejectsError
		{
			get
			{
				return this.tooManyRejectsError;
			}
			set
			{
				bool flag = this.tooManyRejectsError != value;
				if (flag)
				{
					this.tooManyRejectsError = value;
					if (value)
					{
						this.IncreaseErrors();
					}
				}
			}
		}

		// Token: 0x17000788 RID: 1928
		// (get) Token: 0x06001933 RID: 6451 RVA: 0x00095368 File Offset: 0x00093568
		// (set) Token: 0x06001934 RID: 6452 RVA: 0x00095380 File Offset: 0x00093580
		public bool InsightNetConnectionError
		{
			get
			{
				return this.insightNetConnectionError;
			}
			set
			{
				bool flag = this.insightNetConnectionError != value;
				if (flag)
				{
					this.insightNetConnectionError = value;
					if (value)
					{
						this.IncreaseErrors();
					}
				}
			}
		}

		// Token: 0x17000789 RID: 1929
		// (get) Token: 0x06001935 RID: 6453 RVA: 0x000953B4 File Offset: 0x000935B4
		// (set) Token: 0x06001936 RID: 6454 RVA: 0x000953CC File Offset: 0x000935CC
		public bool DiskSpaceLowError
		{
			get
			{
				return this.diskSpaceLowError;
			}
			set
			{
				bool flag = this.diskSpaceLowError != value;
				if (flag)
				{
					this.diskSpaceLowError = value;
					if (value)
					{
						this.IncreaseErrors();
					}
				}
			}
		}

		// Token: 0x1700078A RID: 1930
		// (get) Token: 0x06001937 RID: 6455 RVA: 0x00095400 File Offset: 0x00093600
		// (set) Token: 0x06001938 RID: 6456 RVA: 0x00095418 File Offset: 0x00093618
		public bool InOutletError
		{
			get
			{
				return this.inOutletError;
			}
			set
			{
				bool flag = this.inOutletError != value;
				if (flag)
				{
					this.inOutletError = value;
					if (value)
					{
						this.IncreaseErrors();
					}
				}
			}
		}

		// Token: 0x1700078B RID: 1931
		// (get) Token: 0x06001939 RID: 6457 RVA: 0x0009544C File Offset: 0x0009364C
		// (set) Token: 0x0600193A RID: 6458 RVA: 0x00095464 File Offset: 0x00093664
		public bool ErrorXRayOperatingHours
		{
			get
			{
				return this.errorXRayOperatingHours;
			}
			set
			{
				bool flag = this.errorXRayOperatingHours != value;
				if (flag)
				{
					this.errorXRayOperatingHours = value;
					if (value)
					{
						this.IncreaseErrors();
					}
				}
			}
		}

		// Token: 0x1700078C RID: 1932
		// (get) Token: 0x0600193B RID: 6459 RVA: 0x00095498 File Offset: 0x00093698
		// (set) Token: 0x0600193C RID: 6460 RVA: 0x000954B0 File Offset: 0x000936B0
		public bool SafetySwitchesAsError
		{
			get
			{
				return this.safetySwitchesAsError;
			}
			set
			{
				bool flag = this.safetySwitchesAsError != value;
				if (flag)
				{
					this.safetySwitchesAsError = value;
					if (value)
					{
						this.IncreaseErrors();
					}
				}
			}
		}

		// Token: 0x1700078D RID: 1933
		// (get) Token: 0x0600193D RID: 6461 RVA: 0x000954E4 File Offset: 0x000936E4
		// (set) Token: 0x0600193E RID: 6462 RVA: 0x000954FC File Offset: 0x000936FC
		public bool MonitoringError
		{
			get
			{
				return this.monitoringError;
			}
			set
			{
				bool flag = this.monitoringError != value;
				if (flag)
				{
					this.monitoringError = value;
					if (value)
					{
						this.IncreaseErrors();
					}
				}
			}
		}

		// Token: 0x1700078E RID: 1934
		// (get) Token: 0x0600193F RID: 6463 RVA: 0x00095530 File Offset: 0x00093730
		// (set) Token: 0x06001940 RID: 6464 RVA: 0x00095548 File Offset: 0x00093748
		public bool AIDeploymentError
		{
			get
			{
				return this.aiDeploymentError;
			}
			set
			{
				bool flag = this.aiDeploymentError != value;
				if (flag)
				{
					this.aiDeploymentError = value;
					if (value)
					{
						this.IncreaseErrors();
					}
				}
			}
		}

		// Token: 0x1700078F RID: 1935
		// (get) Token: 0x06001941 RID: 6465 RVA: 0x0009557C File Offset: 0x0009377C
		// (set) Token: 0x06001942 RID: 6466 RVA: 0x00095594 File Offset: 0x00093794
		public bool AIInferenceError
		{
			get
			{
				return this.aiInferenceError;
			}
			set
			{
				bool flag = this.aiInferenceError != value;
				if (flag)
				{
					this.aiInferenceError = value;
					if (value)
					{
						this.IncreaseErrors();
					}
				}
			}
		}

		// Token: 0x17000790 RID: 1936
		// (get) Token: 0x06001943 RID: 6467 RVA: 0x000955C8 File Offset: 0x000937C8
		// (set) Token: 0x06001944 RID: 6468 RVA: 0x000955E0 File Offset: 0x000937E0
		public bool ExternalNetworkForSavingPngError
		{
			get
			{
				return this.externalNetworkForSavingPngError;
			}
			set
			{
				bool flag = this.externalNetworkForSavingPngError != value;
				if (flag)
				{
					this.externalNetworkForSavingPngError = value;
					if (value)
					{
						this.IncreaseErrors();
					}
				}
			}
		}

		// Token: 0x06001945 RID: 6469 RVA: 0x00095613 File Offset: 0x00093813
		public MachineErrors(MachineState machineState, DBConnection dbConnection)
		{
			this.machineState = machineState;
			this.dbConnection = dbConnection;
		}

		// Token: 0x06001946 RID: 6470 RVA: 0x0009562C File Offset: 0x0009382C
		public bool HasAnyError()
		{
			return this.Detector || this.XRay || this.Calibration || this.SVTComm || this.SVTStatus || this.ImageDelay || this.MemoryUsage || this.IOComm || this.WAGOCode || this.BinFull1 || this.MissingObjects1 || this.BinFull2 || this.MissingObjects2 || this.BinFull3 || this.MissingObjects3 || this.BinFull4 || this.MissingObjects4 || this.XRayOnLamp || this.Cooling || this.LowAirPressure || this.Motor || this.Temperature0 || this.Temperature1 || this.Voltage12V || this.Voltage24V || this.LightBarrier1 || this.LightBarrier2 || this.LightBarrier3 || this.LightBarrier4 || this.AuditFailed || this.xrayMotorError || this.binMissingError || this.highLevelSensorsError || this.tooManyRejectsError || this.insightNetConnectionError || this.diskSpaceLowError || this.imageProcPcOffline || this.imagePortCommError || this.paramPortCommError || this.inOutletError || this.HELEDifferenceforDual || this.ErrorXRayOperatingHours || this.SafetySwitchesAsError || this.MonitoringError || this.AIDeploymentError || this.AIInferenceError;
		}

		// Token: 0x06001947 RID: 6471 RVA: 0x0009580C File Offset: 0x00093A0C
		private void IncreaseErrors()
		{
			MachineState machineState = this.machineState;
			int error = machineState.Error;
			machineState.Error = error + 1;
			this.dbConnection.IncreaseNErrors();
			this.dbConnection.SetHasAnyError(true);
		}

		// Token: 0x0400105A RID: 4186
		private DBConnection dbConnection;

		// Token: 0x0400105B RID: 4187
		private bool imagePortCommError;

		// Token: 0x0400105C RID: 4188
		private bool imageProcPcOffline;

		// Token: 0x0400105D RID: 4189
		private bool paramPortCommError;

		// Token: 0x0400105E RID: 4190
		private bool motor;

		// Token: 0x0400105F RID: 4191
		private bool lowAirPressure;

		// Token: 0x04001060 RID: 4192
		private bool cooling;

		// Token: 0x04001061 RID: 4193
		private bool xRayOnLamp;

		// Token: 0x04001062 RID: 4194
		private bool missingObjects1;

		// Token: 0x04001063 RID: 4195
		private bool missingObjects2;

		// Token: 0x04001064 RID: 4196
		private bool missingObjects3;

		// Token: 0x04001065 RID: 4197
		private bool missingObjects4;

		// Token: 0x04001066 RID: 4198
		private bool lightBarrier1;

		// Token: 0x04001067 RID: 4199
		private bool lightBarrier2;

		// Token: 0x04001068 RID: 4200
		private bool lightBarrier3;

		// Token: 0x04001069 RID: 4201
		private bool lightBarrier4;

		// Token: 0x0400106A RID: 4202
		private bool binFull1;

		// Token: 0x0400106B RID: 4203
		private bool binFull2;

		// Token: 0x0400106C RID: 4204
		private bool binFull3;

		// Token: 0x0400106D RID: 4205
		private bool binFull4;

		// Token: 0x0400106E RID: 4206
		private bool wAGOCode;

		// Token: 0x0400106F RID: 4207
		private bool ioComm;

		// Token: 0x04001070 RID: 4208
		private bool memoryUsage;

		// Token: 0x04001071 RID: 4209
		private bool imageDelay;

		// Token: 0x04001072 RID: 4210
		private bool sVTStatus;

		// Token: 0x04001073 RID: 4211
		private bool sVTComm;

		// Token: 0x04001074 RID: 4212
		private bool calibration;

		// Token: 0x04001075 RID: 4213
		private bool heleDifferenceforDual;

		// Token: 0x04001076 RID: 4214
		private bool xRay;

		// Token: 0x04001077 RID: 4215
		private bool detector;

		// Token: 0x04001078 RID: 4216
		private bool temperature0;

		// Token: 0x04001079 RID: 4217
		private bool temperature1;

		// Token: 0x0400107A RID: 4218
		private bool voltage12V;

		// Token: 0x0400107B RID: 4219
		private bool voltage24V;

		// Token: 0x0400107C RID: 4220
		private bool auditFailed;

		// Token: 0x0400107D RID: 4221
		private bool xrayMotorError;

		// Token: 0x0400107E RID: 4222
		private bool binMissingError;

		// Token: 0x0400107F RID: 4223
		private bool highLevelSensorsError;

		// Token: 0x04001080 RID: 4224
		private bool tooManyRejectsError;

		// Token: 0x04001081 RID: 4225
		private bool insightNetConnectionError;

		// Token: 0x04001082 RID: 4226
		private bool diskSpaceLowError;

		// Token: 0x04001083 RID: 4227
		private bool inOutletError;

		// Token: 0x04001084 RID: 4228
		private bool errorXRayOperatingHours;

		// Token: 0x04001085 RID: 4229
		private bool safetySwitchesAsError;

		// Token: 0x04001086 RID: 4230
		private MachineState machineState;

		// Token: 0x04001087 RID: 4231
		private bool monitoringError;

		// Token: 0x04001088 RID: 4232
		private bool aiDeploymentError;

		// Token: 0x04001089 RID: 4233
		private bool aiInferenceError;

		// Token: 0x0400108A RID: 4234
		private bool externalNetworkForSavingPngError;
	}
}
