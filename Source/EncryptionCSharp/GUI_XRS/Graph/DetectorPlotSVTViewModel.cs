using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Common;
using DatabaseConnection;
using Definitions;
using SesoPlot;
using XrsImage;

namespace GUI_XRS.Graph
{
	// Token: 0x02000272 RID: 626
	public class DetectorPlotSVTViewModel : INotifyPropertyChanged
	{
		// Token: 0x14000112 RID: 274
		// (add) Token: 0x06003410 RID: 13328 RVA: 0x00146F70 File Offset: 0x00145170
		// (remove) Token: 0x06003411 RID: 13329 RVA: 0x00146FA8 File Offset: 0x001451A8
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x170010E9 RID: 4329
		// (get) Token: 0x06003412 RID: 13330 RVA: 0x00146FE0 File Offset: 0x001451E0
		// (set) Token: 0x06003413 RID: 13331 RVA: 0x00146FF8 File Offset: 0x001451F8
		public double WidthValve
		{
			get
			{
				return this.widthValve;
			}
			set
			{
				this.widthValve = value;
				this.OnPropertyChanged("WidthValve");
				this.dBConnection.SetValveWidth(value);
			}
		}

		// Token: 0x170010EA RID: 4330
		// (get) Token: 0x06003414 RID: 13332 RVA: 0x0014701C File Offset: 0x0014521C
		// (set) Token: 0x06003415 RID: 13333 RVA: 0x00147034 File Offset: 0x00145234
		public int StartPixelSecondFlap
		{
			get
			{
				return this.startPixelSecondFlap;
			}
			set
			{
				this.startPixelSecondFlap = value;
				this.OnPropertyChanged("StartPixelSecondFlap");
				this.dBConnection.SetSVTStartPixelSecondValve(value);
			}
		}

		// Token: 0x170010EB RID: 4331
		// (get) Token: 0x06003416 RID: 13334 RVA: 0x00147058 File Offset: 0x00145258
		// (set) Token: 0x06003417 RID: 13335 RVA: 0x00147070 File Offset: 0x00145270
		public int EndPixelNextToLastFlap
		{
			get
			{
				return this.endPixelNextToLastFlap;
			}
			set
			{
				this.endPixelNextToLastFlap = value;
				this.OnPropertyChanged("EndPixelNextToLastFlap");
				this.dBConnection.SetSVTEndPixelNextToLastValve(value);
			}
		}

		// Token: 0x170010EC RID: 4332
		// (get) Token: 0x06003418 RID: 13336 RVA: 0x00147094 File Offset: 0x00145294
		// (set) Token: 0x06003419 RID: 13337 RVA: 0x001470AC File Offset: 0x001452AC
		public int NumberValves
		{
			get
			{
				return this.numberValves;
			}
			set
			{
				bool flag = value > 20;
				if (flag)
				{
					value = 20;
				}
				bool flag2 = value < 1;
				if (flag2)
				{
					value = 1;
				}
				this.numberValves = value;
				this.OnPropertyChanged("NumberValves");
				this.dBConnection.SetNumValves(value);
				this.dbDataLogs.AddRejectOnOffTimes();
				this.dbDataLogs.AddSVTValves(value);
				this.dbDataLogs.AddSVTValvesOperatingCounter(value);
			}
		}

		// Token: 0x170010ED RID: 4333
		// (get) Token: 0x0600341A RID: 13338 RVA: 0x00147118 File Offset: 0x00145318
		// (set) Token: 0x0600341B RID: 13339 RVA: 0x00147130 File Offset: 0x00145330
		public int StartPixel
		{
			get
			{
				return this.startPixel;
			}
			set
			{
				this.startPixel = value;
				this.OnPropertyChanged("StartPixel");
				this.dBConnection.SetValveStartPixel(value);
			}
		}

		// Token: 0x170010EE RID: 4334
		// (get) Token: 0x0600341C RID: 13340 RVA: 0x00147154 File Offset: 0x00145354
		// (set) Token: 0x0600341D RID: 13341 RVA: 0x0014716C File Offset: 0x0014536C
		public int IgnoredLeftPixel
		{
			get
			{
				return this.ignoredLeftPixel;
			}
			set
			{
				this.ignoredLeftPixel = value;
				this.OnPropertyChanged("IgnoredLeftPixel");
			}
		}

		// Token: 0x170010EF RID: 4335
		// (get) Token: 0x0600341E RID: 13342 RVA: 0x00147184 File Offset: 0x00145384
		// (set) Token: 0x0600341F RID: 13343 RVA: 0x0014719C File Offset: 0x0014539C
		public int IgnoredRightPixel
		{
			get
			{
				return this.ignoredRightPixel;
			}
			set
			{
				this.ignoredRightPixel = value;
				this.OnPropertyChanged("IgnoredRightPixel");
			}
		}

		// Token: 0x170010F0 RID: 4336
		// (get) Token: 0x06003420 RID: 13344 RVA: 0x001471B4 File Offset: 0x001453B4
		// (set) Token: 0x06003421 RID: 13345 RVA: 0x001471CC File Offset: 0x001453CC
		public int IgnoredLeftPixelDB
		{
			get
			{
				return this.ignoredLeftPixelDB;
			}
			set
			{
				this.ignoredLeftPixelDB = value;
				this.OnPropertyChanged("IgnoredLeftPixelDB");
			}
		}

		// Token: 0x170010F1 RID: 4337
		// (get) Token: 0x06003422 RID: 13346 RVA: 0x001471E4 File Offset: 0x001453E4
		// (set) Token: 0x06003423 RID: 13347 RVA: 0x001471FC File Offset: 0x001453FC
		public int IgnoredRightPixelDB
		{
			get
			{
				return this.ignoredRightPixelDB;
			}
			set
			{
				this.ignoredRightPixelDB = value;
				this.OnPropertyChanged("IgnoredRightPixelDB");
			}
		}

		// Token: 0x170010F2 RID: 4338
		// (get) Token: 0x06003424 RID: 13348 RVA: 0x00147214 File Offset: 0x00145414
		// (set) Token: 0x06003425 RID: 13349 RVA: 0x0014722C File Offset: 0x0014542C
		public int OverlappingLeft
		{
			get
			{
				return this.overlappingLeft;
			}
			set
			{
				bool flag = value > 100;
				if (flag)
				{
					value = 100;
				}
				this.overlappingLeft = value;
				this.OnPropertyChanged("OverlappingLeft");
				this.dBConnection.SetValveOverlappingLeft(value);
			}
		}

		// Token: 0x170010F3 RID: 4339
		// (get) Token: 0x06003426 RID: 13350 RVA: 0x00147268 File Offset: 0x00145468
		// (set) Token: 0x06003427 RID: 13351 RVA: 0x00147280 File Offset: 0x00145480
		public int OverlappingRight
		{
			get
			{
				return this.overlappingRight;
			}
			set
			{
				bool flag = value > 100;
				if (flag)
				{
					value = 100;
				}
				this.overlappingRight = value;
				this.OnPropertyChanged("OverlappingRight");
				this.dBConnection.SetValveOverlappingRight(value);
			}
		}

		// Token: 0x170010F4 RID: 4340
		// (get) Token: 0x06003428 RID: 13352 RVA: 0x001472BC File Offset: 0x001454BC
		// (set) Token: 0x06003429 RID: 13353 RVA: 0x001472D4 File Offset: 0x001454D4
		public double Voltage
		{
			get
			{
				return this.voltage;
			}
			set
			{
				this.voltage = value;
				this.OnPropertyChanged("Voltage");
			}
		}

		// Token: 0x170010F5 RID: 4341
		// (get) Token: 0x0600342A RID: 13354 RVA: 0x001472EC File Offset: 0x001454EC
		// (set) Token: 0x0600342B RID: 13355 RVA: 0x00147304 File Offset: 0x00145504
		public double Current
		{
			get
			{
				return this.current;
			}
			set
			{
				this.current = value;
				this.OnPropertyChanged("Current");
			}
		}

		// Token: 0x170010F6 RID: 4342
		// (get) Token: 0x0600342C RID: 13356 RVA: 0x0014731C File Offset: 0x0014551C
		// (set) Token: 0x0600342D RID: 13357 RVA: 0x00147334 File Offset: 0x00145534
		public double DetectorTemperature
		{
			get
			{
				return this.detectorTemperature;
			}
			set
			{
				this.detectorTemperature = value;
				this.OnPropertyChanged("DetectorTemperature");
			}
		}

		// Token: 0x170010F7 RID: 4343
		// (get) Token: 0x0600342E RID: 13358 RVA: 0x0014734C File Offset: 0x0014554C
		// (set) Token: 0x0600342F RID: 13359 RVA: 0x00147364 File Offset: 0x00145564
		public bool BaselineEnable
		{
			get
			{
				return this.baselineEnable;
			}
			set
			{
				this.baselineEnable = value;
				this.OnPropertyChanged("BaselineEnable");
			}
		}

		// Token: 0x170010F8 RID: 4344
		// (get) Token: 0x06003430 RID: 13360 RVA: 0x0014737C File Offset: 0x0014557C
		// (set) Token: 0x06003431 RID: 13361 RVA: 0x00147394 File Offset: 0x00145594
		public bool DualEnergyAvailable
		{
			get
			{
				return this.dualEnergyAvailable;
			}
			set
			{
				this.dualEnergyAvailable = value;
				this.OnPropertyChanged("DualEnergyAvailable");
			}
		}

		// Token: 0x170010F9 RID: 4345
		// (get) Token: 0x06003432 RID: 13362 RVA: 0x001473AC File Offset: 0x001455AC
		// (set) Token: 0x06003433 RID: 13363 RVA: 0x001473C4 File Offset: 0x001455C4
		public bool BaselineVisible
		{
			get
			{
				return this.baselineVisible;
			}
			set
			{
				this.baselineVisible = value;
				this.OnPropertyChanged("BaselineVisible");
			}
		}

		// Token: 0x170010FA RID: 4346
		// (get) Token: 0x06003434 RID: 13364 RVA: 0x001473DC File Offset: 0x001455DC
		// (set) Token: 0x06003435 RID: 13365 RVA: 0x001473F4 File Offset: 0x001455F4
		public bool OffsetEnable
		{
			get
			{
				return this.offsetEnable;
			}
			set
			{
				this.offsetEnable = value;
				this.OnPropertyChanged("OffsetEnable");
			}
		}

		// Token: 0x170010FB RID: 4347
		// (get) Token: 0x06003436 RID: 13366 RVA: 0x0014740C File Offset: 0x0014560C
		// (set) Token: 0x06003437 RID: 13367 RVA: 0x00147424 File Offset: 0x00145624
		public bool GainEnable
		{
			get
			{
				return this.gainEnable;
			}
			set
			{
				this.gainEnable = value;
				this.OnPropertyChanged("GainEnable");
			}
		}

		// Token: 0x170010FC RID: 4348
		// (get) Token: 0x06003438 RID: 13368 RVA: 0x0014743C File Offset: 0x0014563C
		// (set) Token: 0x06003439 RID: 13369 RVA: 0x00147454 File Offset: 0x00145654
		public bool XRayEnable
		{
			get
			{
				return this.xRayEnable;
			}
			set
			{
				this.xRayEnable = value;
				this.OnPropertyChanged("XRayEnable");
			}
		}

		// Token: 0x170010FD RID: 4349
		// (get) Token: 0x0600343A RID: 13370 RVA: 0x0014746C File Offset: 0x0014566C
		// (set) Token: 0x0600343B RID: 13371 RVA: 0x00147484 File Offset: 0x00145684
		public bool SerialNumberXRayVisible
		{
			get
			{
				return this.serialNumberXRayVisible;
			}
			set
			{
				this.serialNumberXRayVisible = value;
				this.OnPropertyChanged("SerialNumberXRayVisible");
			}
		}

		// Token: 0x170010FE RID: 4350
		// (get) Token: 0x0600343C RID: 13372 RVA: 0x0014749C File Offset: 0x0014569C
		// (set) Token: 0x0600343D RID: 13373 RVA: 0x001474B4 File Offset: 0x001456B4
		public int Scanrate
		{
			get
			{
				return this.scanrate;
			}
			set
			{
				this.scanrate = value;
				this.OnPropertyChanged("Scanrate");
			}
		}

		// Token: 0x170010FF RID: 4351
		// (get) Token: 0x0600343E RID: 13374 RVA: 0x001474CA File Offset: 0x001456CA
		// (set) Token: 0x0600343F RID: 13375 RVA: 0x001474D2 File Offset: 0x001456D2
		public bool ParameterAccessible { get; set; }

		// Token: 0x17001100 RID: 4352
		// (get) Token: 0x06003440 RID: 13376 RVA: 0x001474DC File Offset: 0x001456DC
		// (set) Token: 0x06003441 RID: 13377 RVA: 0x001474F4 File Offset: 0x001456F4
		public int NPixel
		{
			get
			{
				return this.nPixel;
			}
			set
			{
				this.nPixel = value;
				this.OnPropertyChanged("NPixel");
			}
		}

		// Token: 0x17001101 RID: 4353
		// (get) Token: 0x06003442 RID: 13378 RVA: 0x0014750C File Offset: 0x0014570C
		// (set) Token: 0x06003443 RID: 13379 RVA: 0x00147524 File Offset: 0x00145724
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

		// Token: 0x17001102 RID: 4354
		// (get) Token: 0x06003444 RID: 13380 RVA: 0x0014753C File Offset: 0x0014573C
		// (set) Token: 0x06003445 RID: 13381 RVA: 0x00147554 File Offset: 0x00145754
		public int MinY
		{
			get
			{
				return this.minY;
			}
			set
			{
				this.minY = value;
				this.OnPropertyChanged("MinY");
			}
		}

		// Token: 0x17001103 RID: 4355
		// (get) Token: 0x06003446 RID: 13382 RVA: 0x0014756C File Offset: 0x0014576C
		// (set) Token: 0x06003447 RID: 13383 RVA: 0x00147584 File Offset: 0x00145784
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

		// Token: 0x17001104 RID: 4356
		// (get) Token: 0x06003448 RID: 13384 RVA: 0x0014759C File Offset: 0x0014579C
		// (set) Token: 0x06003449 RID: 13385 RVA: 0x001475B4 File Offset: 0x001457B4
		public int TicksX
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

		// Token: 0x17001105 RID: 4357
		// (get) Token: 0x0600344A RID: 13386 RVA: 0x001475CC File Offset: 0x001457CC
		// (set) Token: 0x0600344B RID: 13387 RVA: 0x001475E4 File Offset: 0x001457E4
		public List<Point> PlotPoints
		{
			get
			{
				return this.plotPoints;
			}
			set
			{
				this.plotPoints = value;
				this.OnPropertyChanged("PlotPoints");
			}
		}

		// Token: 0x17001106 RID: 4358
		// (get) Token: 0x0600344C RID: 13388 RVA: 0x001475FC File Offset: 0x001457FC
		// (set) Token: 0x0600344D RID: 13389 RVA: 0x00147614 File Offset: 0x00145814
		public List<Point> IgnoreLeftPoints
		{
			get
			{
				return this.ignoreLeftPoints;
			}
			set
			{
				this.ignoreLeftPoints = value;
				this.OnPropertyChanged("IgnoreLeftPoints");
			}
		}

		// Token: 0x17001107 RID: 4359
		// (get) Token: 0x0600344E RID: 13390 RVA: 0x0014762C File Offset: 0x0014582C
		// (set) Token: 0x0600344F RID: 13391 RVA: 0x00147644 File Offset: 0x00145844
		public List<Point> IgnoreRightPoints
		{
			get
			{
				return this.ignoreRightPoints;
			}
			set
			{
				this.ignoreRightPoints = value;
				this.OnPropertyChanged("IgnoreRightPoints");
			}
		}

		// Token: 0x17001108 RID: 4360
		// (get) Token: 0x06003450 RID: 13392 RVA: 0x0014765C File Offset: 0x0014585C
		// (set) Token: 0x06003451 RID: 13393 RVA: 0x00147674 File Offset: 0x00145874
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

		// Token: 0x17001109 RID: 4361
		// (get) Token: 0x06003452 RID: 13394 RVA: 0x0014768C File Offset: 0x0014588C
		// (set) Token: 0x06003453 RID: 13395 RVA: 0x001476A4 File Offset: 0x001458A4
		public List<Point> ValveLines1
		{
			get
			{
				return this.valveLines1;
			}
			set
			{
				this.valveLines1 = value;
				this.OnPropertyChanged("ValveLines1");
			}
		}

		// Token: 0x1700110A RID: 4362
		// (get) Token: 0x06003454 RID: 13396 RVA: 0x001476BC File Offset: 0x001458BC
		// (set) Token: 0x06003455 RID: 13397 RVA: 0x001476D4 File Offset: 0x001458D4
		public List<Point> ValveLines2
		{
			get
			{
				return this.valveLines2;
			}
			set
			{
				this.valveLines2 = value;
				this.OnPropertyChanged("ValveLines2");
			}
		}

		// Token: 0x1700110B RID: 4363
		// (get) Token: 0x06003456 RID: 13398 RVA: 0x001476EC File Offset: 0x001458EC
		// (set) Token: 0x06003457 RID: 13399 RVA: 0x00147704 File Offset: 0x00145904
		public bool IsDualEnergyDetector
		{
			get
			{
				return this.isDualEnergyDetector;
			}
			set
			{
				this.isDualEnergyDetector = value;
				this.OnPropertyChanged("IsDualEnergyDetector");
			}
		}

		// Token: 0x1700110C RID: 4364
		// (get) Token: 0x06003458 RID: 13400 RVA: 0x0014771C File Offset: 0x0014591C
		// (set) Token: 0x06003459 RID: 13401 RVA: 0x00147734 File Offset: 0x00145934
		public EnergyLevelMode EnergyMode
		{
			get
			{
				return this.energyMode;
			}
			set
			{
				this.energyMode = value;
				this.OnPropertyChanged("EnergyMode");
			}
		}

		// Token: 0x1700110D RID: 4365
		// (get) Token: 0x0600345A RID: 13402 RVA: 0x0014774A File Offset: 0x0014594A
		// (set) Token: 0x0600345B RID: 13403 RVA: 0x00147752 File Offset: 0x00145952
		public RelayCommand RescaleCommand { get; set; }

		// Token: 0x1700110E RID: 4366
		// (get) Token: 0x0600345C RID: 13404 RVA: 0x0014775B File Offset: 0x0014595B
		// (set) Token: 0x0600345D RID: 13405 RVA: 0x00147763 File Offset: 0x00145963
		public RelayCommand SaveReferencePlotCommand { get; set; }

		// Token: 0x1700110F RID: 4367
		// (get) Token: 0x0600345E RID: 13406 RVA: 0x0014776C File Offset: 0x0014596C
		// (set) Token: 0x0600345F RID: 13407 RVA: 0x00147774 File Offset: 0x00145974
		public RelayCommand ZoomInXCommand { get; set; }

		// Token: 0x17001110 RID: 4368
		// (get) Token: 0x06003460 RID: 13408 RVA: 0x0014777D File Offset: 0x0014597D
		// (set) Token: 0x06003461 RID: 13409 RVA: 0x00147785 File Offset: 0x00145985
		public RelayCommand ZoomOutXCommand { get; set; }

		// Token: 0x17001111 RID: 4369
		// (get) Token: 0x06003462 RID: 13410 RVA: 0x0014778E File Offset: 0x0014598E
		// (set) Token: 0x06003463 RID: 13411 RVA: 0x00147796 File Offset: 0x00145996
		public RelayCommand CleanUpCommand { get; set; }

		// Token: 0x17001112 RID: 4370
		// (get) Token: 0x06003464 RID: 13412 RVA: 0x0014779F File Offset: 0x0014599F
		// (set) Token: 0x06003465 RID: 13413 RVA: 0x001477A7 File Offset: 0x001459A7
		public RelayCommand ShowSnrCommand { get; set; }

		// Token: 0x14000113 RID: 275
		// (add) Token: 0x06003466 RID: 13414 RVA: 0x001477B0 File Offset: 0x001459B0
		// (remove) Token: 0x06003467 RID: 13415 RVA: 0x001477E8 File Offset: 0x001459E8
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event EventHandler<RcvImgEventArgs> ImageArrived;

		// Token: 0x17001113 RID: 4371
		// (get) Token: 0x06003468 RID: 13416 RVA: 0x00147820 File Offset: 0x00145A20
		// (set) Token: 0x06003469 RID: 13417 RVA: 0x00147838 File Offset: 0x00145A38
		public double MachineParametersFontSize
		{
			get
			{
				return this.machineParametersFontSize;
			}
			set
			{
				this.machineParametersFontSize = value;
			}
		}

		// Token: 0x17001114 RID: 4372
		// (get) Token: 0x0600346A RID: 13418 RVA: 0x00147844 File Offset: 0x00145A44
		// (set) Token: 0x0600346B RID: 13419 RVA: 0x0014785C File Offset: 0x00145A5C
		public ZoomMode ZoomMode
		{
			get
			{
				return this.zoomMode;
			}
			set
			{
				this.zoomMode = value;
				this.OnPropertyChanged("ZoomMode");
			}
		}

		// Token: 0x0600346C RID: 13420 RVA: 0x00147874 File Offset: 0x00145A74
		public DetectorPlotSVTViewModel(ParameterHandling parHandling, AccessRightsManager accessManager, DBConnection dBConnection, MachineState machineState, ImageDataTransfer imageTransfer, DBDataLogs dbDataLogs)
		{
			this.parHandling = parHandling;
			this.dBConnection = dBConnection;
			this.machineState = machineState;
			this.imageTransfer = imageTransfer;
			this.dbDataLogs = dbDataLogs;
			this.SaveReferencePlotCommand = new RelayCommand(new Action<object>(this.SaveReferencePlot));
			this.ZoomInXCommand = new RelayCommand(new Action<object>(this.ZoomInX));
			this.ZoomOutXCommand = new RelayCommand(new Action<object>(this.ZoomOutX));
			this.CleanUpCommand = new RelayCommand(new Action<object>(this.CleanUp));
			this.Initialize();
			string text = "";
			Progress.ShowStaticProgress(LocalizationManager.Localize("txtProcessing"), false);
			this.InitializePlotParameter();
			machineState.StateChangedToInspecting += this.machineState_StateChangedToInspecting;
			this.timerUpdateParams = new Timer(new TimerCallback(this.timer_UpdateParameters), null, 0, 2000);
			this.ParameterAccessible = (accessManager.CurrentUser.Rights == BuiltInPriviledges.Service);
			dBConnection.GetSerialNumber(ref text);
			MachineType machineType = MachineType.MachineTypeUndefined;
			dBConnection.GetMachineType(ref machineType);
			this.machineParametersFontSize = ((machineType != MachineType.RayconEntry) ? 12.0 : 10.0);
			Progress.CloseStaticProgress(false);
			ErrorSurvey.CheckArcsIsActive = false;
			imageTransfer.FrameArrived += this.imageTransfer_FrameArrivedSVT;
		}

		// Token: 0x0600346D RID: 13421 RVA: 0x00147A50 File Offset: 0x00145C50
		public void Initialize()
		{
			int num = 0;
			this.dBConnection.GetNumValves(ref num);
			this.NumberValves = num;
			int num2 = 0;
			this.dBConnection.GetIgnoredLeftPixel(ref num2);
			this.IgnoredLeftPixelDB = num2;
			num2 = 0;
			this.dBConnection.GetIgnoredRightPixel(ref num2);
			this.IgnoredRightPixelDB = num2;
			num2 = 0;
			bool flag = int.TryParse(this.parHandling.ReadPar(ParameterHandling.ignoreLeftPixelPath), out num2);
			if (flag)
			{
				this.IgnoredLeftPixel = num2;
			}
			num2 = 0;
			bool flag2 = int.TryParse(this.parHandling.ReadPar(ParameterHandling.ignoreRightPixelPath), out num2);
			if (flag2)
			{
				this.IgnoredRightPixel = num2;
			}
			int num3 = 0;
			this.dBConnection.GetValveOverlappingLeft(ref num3);
			this.OverlappingLeft = num3;
			int num4 = 0;
			this.dBConnection.GetValveOverlappingRight(ref num4);
			this.OverlappingRight = num4;
			double num5 = 0.0;
			this.dBConnection.GetValveWidth(ref num5);
			this.WidthValve = num5;
			int num6 = 0;
			this.dBConnection.GetValveStartPixel(ref num6);
			this.StartPixel = num6;
			num6 = 0;
			this.dBConnection.GetSVTStartPixelSecondValve(ref num6);
			this.StartPixelSecondFlap = num6;
			num6 = 0;
			this.dBConnection.GetSVTEndPixelNextToLastValve(ref num6);
			this.EndPixelNextToLastFlap = num6;
		}

		// Token: 0x0600346E RID: 13422 RVA: 0x00147B94 File Offset: 0x00145D94
		public void InitializeValveLines()
		{
			int num = 0;
			this.dBConnection.GetNumValves(ref num);
			List<Point> list = new List<Point>();
			List<Point> list2 = new List<Point>();
			for (int i = 2; i <= num; i++)
			{
				string str = Definition.SVTValvesAssignmentMainPath + i.ToString("D2");
				string s = this.parHandling.ReadPar(str + "/MinPixel/Value");
				int num2 = 0;
				bool flag = int.TryParse(s, out num2);
				if (flag)
				{
					num2 = num2 + this.IgnoredLeftPixelDB + this.OverlappingLeft;
					list.Add(new Point
					{
						X = (double)num2,
						Y = 10000.0
					});
					list2.Add(new Point
					{
						X = (double)(num2 + this.overlappingRight),
						Y = 10000.0
					});
					list2.Add(new Point
					{
						X = (double)(num2 - this.overlappingLeft),
						Y = 10000.0
					});
				}
			}
			this.ValveLines1 = list;
			this.ValveLines2 = list2;
		}

		// Token: 0x0600346F RID: 13423 RVA: 0x00147CE5 File Offset: 0x00145EE5
		private void machineState_StateChangedToInspecting(MachineStates obj)
		{
			this.xrayTurnedOnInPlotMenu = true;
			this.InitializePlotParameter();
		}

		// Token: 0x06003470 RID: 13424 RVA: 0x00147CF8 File Offset: 0x00145EF8
		private void timer_UpdateParameters(object dummy)
		{
			int num = 0;
			this.dBConnection.GetIgnoredLeftPixel(ref num);
			this.IgnoredLeftPixelDB = num;
			num = 0;
			this.dBConnection.GetIgnoredRightPixel(ref num);
			this.IgnoredRightPixelDB = num;
			num = 0;
			bool flag = int.TryParse(this.parHandling.ReadPar(ParameterHandling.ignoreLeftPixelPath), out num);
			if (flag)
			{
				this.IgnoredLeftPixel = num;
			}
			num = 0;
			bool flag2 = int.TryParse(this.parHandling.ReadPar(ParameterHandling.ignoreRightPixelPath), out num);
			if (flag2)
			{
				this.IgnoredRightPixel = num;
			}
			double num2 = 0.0;
			this.dBConnection.GetValveWidth(ref num2);
			this.WidthValve = num2;
			int num3 = 0;
			this.dBConnection.GetValveStartPixel(ref num3);
			this.StartPixel = num3;
		}

		// Token: 0x06003471 RID: 13425 RVA: 0x00147DBC File Offset: 0x00145FBC
		public void CleanUp(object obj)
		{
			bool flag = this.ValveLines1 != null;
			if (flag)
			{
				this.ValveLines1.Clear();
				this.ValveLines1 = null;
			}
			bool flag2 = this.ValveLines2 != null;
			if (flag2)
			{
				this.ValveLines2.Clear();
				this.ValveLines2 = null;
			}
			bool flag3 = this.PlotPoints != null;
			if (flag3)
			{
				this.PlotPoints.Clear();
				this.PlotPoints = null;
			}
			bool flag4 = this.IgnoreLeftPoints != null;
			if (flag4)
			{
				this.IgnoreLeftPoints.Clear();
				this.IgnoreLeftPoints = null;
			}
			bool flag5 = this.IgnoreRightPoints != null;
			if (flag5)
			{
				this.IgnoreRightPoints.Clear();
				this.IgnoreRightPoints = null;
			}
			this.machineState.StateChangedToInspecting -= this.machineState_StateChangedToInspecting;
			this.imageTransfer.FrameArrived -= this.imageTransfer_FrameArrivedSVT;
			string a = this.parHandling.ReadPar(Definition.machineModePath);
			bool flag6 = a == 1.ToString();
			if (flag6)
			{
				this.parHandling.WriteValue("/filter/xrayoffimage/Enable/Value", "1");
			}
			this.timerUpdateParams.Change(-1, -1);
			this.DeinitializePlotParameter();
		}

		// Token: 0x06003472 RID: 13426 RVA: 0x00147F00 File Offset: 0x00146100
		public void SavePlot(Grid chartGrid, Rect bounds)
		{
			string str = "";
			string text = DetectorPlotSVTViewModel.plotFolderName;
			string text2 = null;
			bool flag = this.dBConnection == null;
			if (flag)
			{
				Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtNoDatabase"), DialogStyle.Error);
			}
			else
			{
				bool flag2 = !Directory.Exists(text);
				if (flag2)
				{
					Directory.CreateDirectory(text);
				}
				this.dBConnection.GetUsbDriveLetter(ref str);
				this.dBConnection.GetSerialNumber(ref text2);
				string str2 = "Plot_SVT_" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_ff") + ((text2 != null && text2.Length > 0) ? ("_" + text2) : "") + ".png";
				string text3 = str + "\\" + str2;
				text = text + "\\" + str2;
				FileInfo fileInfo = new FileInfo(text3);
				try
				{
					RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap((int)bounds.Width, (int)bounds.Height, 96.0, 96.0, PixelFormats.Pbgra32);
					DrawingVisual drawingVisual = new DrawingVisual();
					using (DrawingContext drawingContext = drawingVisual.RenderOpen())
					{
						VisualBrush brush = new VisualBrush(chartGrid);
						drawingContext.DrawRectangle(brush, null, new Rect(default(Point), bounds.Size));
					}
					renderTargetBitmap.Render(drawingVisual);
					List<string> list = Directory.GetFiles(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Plot").ToList<string>();
					list.Sort(new NamesComparer());
					for (int i = list.Count; i >= 10; i--)
					{
						File.Delete(list[0]);
					}
					FileStream fileStream = new FileStream(text, FileMode.Create);
					new PngBitmapEncoder
					{
						Frames = 
						{
							BitmapFrame.Create(renderTargetBitmap)
						}
					}.Save(fileStream);
					fileStream.Close();
					bool flag3 = Directory.Exists(str + "\\");
					if (flag3)
					{
						fileStream = new FileStream(fileInfo.FullName, FileMode.Create);
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
						Path.GetDirectoryName(text3),
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
						Path.GetDirectoryName(text3),
						"\" ",
						LocalizationManager.Localize("msgForbidden")
					}), DialogStyle.Error);
				}
			}
		}

		// Token: 0x06003473 RID: 13427 RVA: 0x00148270 File Offset: 0x00146470
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x06003474 RID: 13428 RVA: 0x001482A0 File Offset: 0x001464A0
		private void imageTransfer_FrameArrivedSVT(object sender, ReceiveEventArgs e)
		{
			bool flag = e.Image.Type != ImageType.Histogram && e.Image.Type != ImageType.MaterialCurve && e.Image.Type != ImageType.TestcardSimulation;
			if (flag)
			{
				List<Point> pointsTemp = new List<Point>();
				List<Point> list = new List<Point>();
				List<Point> list2 = new List<Point>();
				int num = 0;
				string s = this.parHandling.ReadPar(ParameterHandling.ignoreLeftPixelPath);
				int.TryParse(s, out num);
				bool flag2 = e.Image.Width > 0 && e.Image.Height > 0 && Application.Current != null;
				if (flag2)
				{
					bool flag3 = e.Image.EnergyLevels == 1;
					if (flag3)
					{
						for (int i = 0; i < (int)e.Image.Width; i++)
						{
							pointsTemp.Add(new Point((double)(i + this.ignoredLeftPixel), (double)e.Image.Data[i]));
						}
					}
					else
					{
						for (int j = 0; j < (int)(e.Image.Width / 2); j++)
						{
							pointsTemp.Add(new Point((double)(j + this.ignoredLeftPixel), (double)e.Image.Data[j]));
							list.Add(new Point((double)(j + this.ignoredLeftPixel), (double)e.Image.Data[j + (int)(e.Image.Width / 2)]));
						}
					}
				}
				List<int> list3 = new List<int>();
				List<int> list4 = list3;
				int item;
				if (this.plotPoints == null || this.plotPoints.Count <= 0 || this.energyMode == EnergyLevelMode.HighEnergy)
				{
					item = 0;
				}
				else
				{
					item = (int)this.plotPoints.Max((Point point) => point.Y);
				}
				list4.Add(item);
				this.CalculateNewMaxY(list3.Max(), false);
				Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate()
				{
					this.PlotPoints = ((this.energyMode != EnergyLevelMode.HighEnergy) ? pointsTemp : new List<Point>());
					this.IgnoreLeftPoints = this.GetIgnorePoints(true);
					this.IgnoreRightPoints = this.GetIgnorePoints(false);
					bool flag5 = this.saveReferencePlot;
					if (flag5)
					{
						this.SaveReferencePlot(e.Image);
						this.saveReferencePlot = false;
					}
				}));
				bool flag4 = this.ImageArrived != null;
				if (flag4)
				{
					this.ImageArrived(sender, new RcvImgEventArgs(e.Image));
				}
			}
		}

		// Token: 0x06003475 RID: 13429 RVA: 0x00148588 File Offset: 0x00146788
		private void CalculateNewMaxY(int currentMax, bool respectCurrentMax = true)
		{
			bool flag = currentMax > this.maxY || !respectCurrentMax;
			if (flag)
			{
				int num = 1;
				int newMax = (int)((double)currentMax * 1.3);
				int newTicksY = 1;
				bool flag2 = newMax >= 10000;
				if (flag2)
				{
					num = 1000;
				}
				else
				{
					bool flag3 = newMax >= 1000;
					if (flag3)
					{
						num = 100;
					}
					else
					{
						bool flag4 = newMax >= 100;
						if (flag4)
						{
							num = 10;
						}
					}
				}
				newMax = newMax / num * num;
				newTicksY = newMax / 10;
				bool flag5 = newTicksY > 100;
				if (flag5)
				{
					newTicksY = newTicksY / 100 * 100;
				}
				else
				{
					bool flag6 = newTicksY > 10;
					if (flag6)
					{
						newTicksY = newTicksY / 10 * 10;
					}
				}
				Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate()
				{
					this.MinY = 0;
					this.MaxY = newMax;
					this.TicksY = newTicksY;
				}));
			}
		}

		// Token: 0x06003476 RID: 13430 RVA: 0x001486A8 File Offset: 0x001468A8
		public void Rescale()
		{
			bool flag = this.plotPoints != null;
			if (flag)
			{
				List<int> list = new List<int>();
				List<int> list2 = list;
				int item;
				if (this.plotPoints.Count <= 0 || this.energyMode == EnergyLevelMode.HighEnergy)
				{
					item = 0;
				}
				else
				{
					item = (int)this.plotPoints.Max((Point point) => point.Y);
				}
				list2.Add(item);
				this.CalculateNewMaxY(list.Max(), false);
			}
			this.TicksX = ((this.NPixel <= 512) ? 4 : 8);
			this.IgnoreLeftPoints = this.GetIgnorePoints(true);
			this.IgnoreRightPoints = this.GetIgnorePoints(false);
		}

		// Token: 0x06003477 RID: 13431 RVA: 0x0014875C File Offset: 0x0014695C
		private List<Point> GetIgnorePoints(bool left)
		{
			List<Point> list = new List<Point>();
			int num = 0;
			try
			{
				if (left)
				{
					this.dBConnection.GetIgnoredLeftPixel(ref num);
				}
				else
				{
					this.dBConnection.GetIgnoredRightPixel(ref num);
				}
				int num2 = this.maxY / 20;
				for (int i = 0; i < this.maxY - this.minY; i += num2)
				{
					list.Add(new Point((double)(left ? num : (this.NPixel - num)), (double)i));
				}
			}
			catch
			{
			}
			return list;
		}

		// Token: 0x06003478 RID: 13432 RVA: 0x00148804 File Offset: 0x00146A04
		private void SaveReferencePlot(object obj)
		{
			bool flag = Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtOverwriteExisting"), DialogStyle.Warning, DialogButtons.OKCancel) == Dialog.DialogResults.Ok;
			if (flag)
			{
				this.saveReferencePlot = true;
			}
		}

		// Token: 0x06003479 RID: 13433 RVA: 0x0014883D File Offset: 0x00146A3D
		private void ZoomInX(object obj)
		{
			this.ZoomMode = ZoomMode.ToMinimum;
			this.ZoomFactorX *= 2.0;
		}

		// Token: 0x0600347A RID: 13434 RVA: 0x0014885F File Offset: 0x00146A5F
		private void ZoomOutX(object obj)
		{
			this.ZoomMode = ZoomMode.ToCenter;
			this.ZoomFactorX /= 2.0;
		}

		// Token: 0x0600347B RID: 13435 RVA: 0x00148884 File Offset: 0x00146A84
		public void EnergyModeChanged(object sender, SelectionChangedEventArgs e)
		{
			bool flag = e.AddedItems.Count > 0 && e.AddedItems[0] is EnergyLevelMode;
			if (flag)
			{
				EnergyLevelMode energyLevelMode = (EnergyLevelMode)e.AddedItems[0];
				this.EnergyMode = energyLevelMode;
			}
		}

		// Token: 0x0600347C RID: 13436 RVA: 0x001488D8 File Offset: 0x00146AD8
		public void getNumKeyboardNumber(string name)
		{
			string oldValue = "";
			if (!(name == "Overlapping Left"))
			{
				if (!(name == "Overlapping Right"))
				{
					if (!(name == "Number Valves"))
					{
						if (!(name == "Start Pixel Second Valve"))
						{
							if (name == "End Pixel First Valve")
							{
								oldValue = this.EndPixelNextToLastFlap.ToString();
							}
						}
						else
						{
							oldValue = this.StartPixelSecondFlap.ToString();
						}
					}
					else
					{
						oldValue = this.NumberValves.ToString();
					}
				}
				else
				{
					oldValue = this.OverlappingRight.ToString();
				}
			}
			else
			{
				oldValue = this.OverlappingLeft.ToString();
			}
			this.NameToChange = name;
			this.numKeyboard = new NumKeyboard(name, oldValue, NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.keyboard_TextInputKeyboardNumber;
			this.numKeyboard.Show();
		}

		// Token: 0x0600347D RID: 13437 RVA: 0x001489C4 File Offset: 0x00146BC4
		private void keyboard_TextInputKeyboardNumber(object sender, KeyboardInputEventArgs e)
		{
			bool flag = e.Text != null;
			if (flag)
			{
				int num = 0;
				bool flag2 = int.TryParse(e.Text, out num);
				if (flag2)
				{
					string nameToChange = this.NameToChange;
					string a = nameToChange;
					if (!(a == "Overlapping Left"))
					{
						if (!(a == "Overlapping Right"))
						{
							if (!(a == "Number Valves"))
							{
								if (!(a == "Start Pixel Second Valve"))
								{
									if (a == "End Pixel First Valve")
									{
										this.dBConnection.SetSVTEndPixelNextToLastValve(num);
										this.EndPixelNextToLastFlap = num;
									}
								}
								else
								{
									this.dBConnection.SetSVTStartPixelSecondValve(num);
									this.StartPixelSecondFlap = num;
								}
							}
							else
							{
								this.dBConnection.SetNumValves(num);
								this.dbDataLogs.AddRejectOnOffTimes();
								this.NumberValves = num;
								this.dbDataLogs.AddSVTValves(num);
								this.dbDataLogs.AddSVTValvesOperatingCounter(num);
							}
						}
						else
						{
							this.dBConnection.SetValveOverlappingRight(num);
							this.OverlappingRight = num;
						}
					}
					else
					{
						this.dBConnection.SetValveOverlappingLeft(num);
						this.OverlappingLeft = num;
					}
					this.CalculateNewValvePositions();
				}
			}
			this.numKeyboard.TextInputKeyboard -= this.keyboard_TextInputKeyboardNumber;
		}

		// Token: 0x0600347E RID: 13438 RVA: 0x00148B10 File Offset: 0x00146D10
		private void CalculateNewValvePositions()
		{
			int num = 0;
			int num2 = 0;
			this.dBConnection.GetIgnoredRightPixel(ref num2);
			this.dBConnection.GetIgnoredLeftPixel(ref num);
			int num3 = 0;
			string text = this.parHandling.ReadPar(Definition.detectorTypePath);
			bool flag = text.Equals("1");
			if (flag)
			{
				int.TryParse(this.parHandling.ReadPar(Definition.detectorSesotecNPixelPath), out num3);
			}
			else
			{
				int.TryParse(this.parHandling.ReadPar(Definition.detectorDTNPixelPath), out num3);
			}
			double num4 = Math.Round((double)(this.EndPixelNextToLastFlap - this.StartPixelSecondFlap) / ((double)this.NumberValves - 2.0), 3);
			int num5 = (int)num4;
			bool flag2 = (double)(num5 - num5) >= 0.5;
			int num6;
			if (flag2)
			{
				num6 = (int)num4 - (this.StartPixelSecondFlap - num) + 1;
			}
			else
			{
				num6 = (int)num4 - (this.StartPixelSecondFlap - num) + 1;
			}
			int num7 = num3 - num2;
			int num8 = num6;
			int num9 = 0;
			this.dBConnection.GetFirstValve(ref num9);
			bool flag3 = false;
			this.dBConnection.GetValvesInverted(ref flag3);
			List<int> list = new List<int>();
			List<int> list2 = new List<int>();
			double num10 = 0.0;
			int num11 = 0;
			for (int i = 1; i <= this.NumberValves; i++)
			{
				bool flag4 = i < num9;
				int num12;
				if (flag4)
				{
					num12 = -1;
					num11 = -1;
				}
				else
				{
					bool flag5 = i == num9;
					if (flag5)
					{
						num12 = 0;
						num11 = num5 - num8 + this.overlappingRight;
					}
					else
					{
						num12 = num11 - this.overlappingLeft - this.overlappingRight;
						num11 += num5;
					}
					bool flag6 = num12 < 0;
					if (flag6)
					{
						num12 = -1;
					}
					bool flag7 = num11 < 0;
					if (flag7)
					{
						num11 = -1;
					}
					num10 += num4 - (double)num5;
					bool flag8 = num10 >= 0.5 || i == num9;
					if (flag8)
					{
						num11++;
						num10 -= 1.0;
					}
				}
				list.Add(num12);
				list2.Add(num11);
				bool flag9 = flag3;
				string str;
				if (flag9)
				{
					str = Definition.SVTValvesAssignmentMainPath + (this.NumberValves - i + 1).ToString("D2");
				}
				else
				{
					str = Definition.SVTValvesAssignmentMainPath + i.ToString("D2");
				}
				this.parHandling.WriteValue(str + "/MinPixel/Value", num12.ToString(), false);
				this.parHandling.WriteValue(str + "/MaxPixel/Value", num11.ToString(), false);
			}
			for (int j = this.NumberValves + 1; j <= 80; j++)
			{
				bool flag10 = flag3;
				string str;
				if (flag10)
				{
					str = Definition.SVTValvesAssignmentMainPath + (this.NumberValves - j + 1).ToString("D2");
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
			this.dBConnection.SetSVTMinPixels(0, list.ToArray(), flag3);
			this.dBConnection.SetSVTMaxPixels(0, list2.ToArray(), flag3);
			this.WidthValve = num4;
			this.StartPixel = num6;
			this.InitializeValveLines();
		}

		// Token: 0x0600347F RID: 13439 RVA: 0x00148EC8 File Offset: 0x001470C8
		private void InitializePlotParameter()
		{
			this.machineState.State = MachineStates.ViewingPlot;
			DetectorSubType detectorSubType = DetectorSubType.None;
			string value = "0";
			this.dBConnection.GetDetectorType(ref this.detectorType);
			this.scanratePath = ((this.detectorType == DetectorType.DT || this.detectorType == DetectorType.DTNetSE || this.detectorType == DetectorType.DTNetDE || this.detectorType == DetectorType.DTNetTDI) ? Definition.detectorDTScanratePath : Definition.detectorSesotecScanratePath);
			bool flag = this.oldScanrate == 0;
			if (flag)
			{
				int.TryParse(this.parHandling.ReadPar(this.scanratePath), out this.oldScanrate);
			}
			this.oldContinuousMode = this.parHandling.ReadPar(ParameterHandling.continuousModePath);
			this.oldContinuousModeFiltering = this.parHandling.ReadPar(Acquisition.continuousModeFilteringPath);
			this.oldSendOnlyIfContaminant = this.parHandling.ReadPar(Acquisition.sendOnlyIfContaminantPath);
			this.oldSendImage = this.parHandling.ReadPar(Acquisition.sendImagePath);
			this.oldBulkFlapsValvesActive = this.parHandling.ReadPar(FlapsOverIOs.bulkflapsEnableValvesPath);
			this.oldSVTValvesActive = this.parHandling.ReadPar(SVT.svtValvesEnablePath);
			bool flag2 = this.parHandling.ReadPar(FlapsOverIOs.bulkflapsEnabledPath) == "1";
			if (flag2)
			{
				this.parHandling.WriteValue(FlapsOverIOs.bulkflapsEnableValvesPath, "0");
			}
			else
			{
				this.parHandling.WriteValue(SVT.svtValvesEnablePath, "0");
			}
			this.parHandling.WriteValue(Acquisition.binningPath, "1", false);
			this.parHandling.WriteValue(Acquisition.binPixelPath, "1", false);
			this.parHandling.WriteValue(Acquisition.lane1EnablePath, "1", false);
			this.parHandling.WriteValue(Acquisition.lane2EnablePath, "0", false);
			this.parHandling.WriteValue(Acquisition.lane3EnablePath, "0", false);
			this.parHandling.WriteValue(Acquisition.lane4EnablePath, "0", false);
			this.parHandling.WriteValue(Acquisition.lane1MinPixelPath, "0", false);
			this.parHandling.WriteValue(Acquisition.lane1MaxPixelPath, 10239.ToString(), false);
			this.parHandling.WriteValue(Acquisition.continuousModeFilteringPath, "0", false);
			this.parHandling.WriteValue(XRayOnProcessing.dtGainEnablePath, "0", false);
			this.parHandling.WriteValue(XRayOnProcessing.manualGainCalibrationPath, "0", false);
			this.GainEnable = false;
			this.parHandling.WriteValue(XRayOnProcessing.dtOffsetEnablePath, "0", false);
			string a = this.parHandling.ReadPar("/hardware/detector/sesotec/SubType/Value");
			bool flag3 = a == "1";
			if (flag3)
			{
				detectorSubType = DetectorSubType.SesotecHS;
			}
			bool flag4 = this.detectorType == DetectorType.DTNetDE || (this.detectorType == DetectorType.Sesotec && detectorSubType == DetectorSubType.SesotecHS);
			if (flag4)
			{
				value = "1";
			}
			this.parHandling.WriteValue(XRayOnProcessing.manualOffsetCalibrationPath, value, false);
			this.OffsetEnable = false;
			this.Scanrate = ((this.detectorType != DetectorType.DTNetDE) ? 375 : 500);
			this.parHandling.WriteValue(this.scanratePath, this.Scanrate.ToString(), false);
			this.IgnoredLeftPixel = 0;
			this.parHandling.WriteValue(ParameterHandling.ignoreLeftPixelPath, this.IgnoredLeftPixel.ToString(), false);
			this.IgnoredRightPixel = 0;
			this.parHandling.WriteValue(ParameterHandling.ignoreRightPixelPath, this.IgnoredRightPixel.ToString(), false);
			this.parHandling.WriteValue(XRayOnProcessing.dtBaselineEnablePath, "0", false);
			this.BaselineEnable = false;
			this.BaselineVisible = (this.detectorType == DetectorType.DT || this.detectorType == DetectorType.DTNetSE || this.detectorType == DetectorType.DTNetDE || this.detectorType == DetectorType.DTNetTDI);
			this.DualEnergyAvailable = (this.detectorType == DetectorType.DTNetDE);
			this.Current = ((this.detectorType != DetectorType.DTNetDE) ? 1.0 : 0.5);
			ParameterHandling parameterHandling = this.parHandling;
			string path = "/hardware/xraysource/CurrentSetpoint/Value";
			double num = this.Current;
			parameterHandling.WriteValue(path, num.ToString(new CultureInfo("en-US")), false);
			this.Voltage = (double)((this.detectorType != DetectorType.DTNetDE) ? 40 : 70);
			this.parHandling.WriteValue("/hardware/xraysource/VoltageSetpoint/Value", ((int)this.Voltage).ToString(), false);
			this.parHandling.WriteValue(Acquisition.sendOnlyIfContaminantPath, "0", false);
			this.parHandling.WriteValue(Acquisition.sendImagePath, "1", false);
			this.parHandling.WriteValue("/filter/acquisition/MaxLines/Value", "200", false);
			this.parHandling.WriteValue(ParameterHandling.continuousModePath, "1", false);
			bool flag5 = this.parHandling.ReadPar(XRayOnProcessing.xrayEnablePath) == "1";
			if (flag5)
			{
				this.XRayEnable = true;
			}
			else
			{
				this.XRayEnable = false;
			}
			this.SerialNumberXRayVisible = (this.parHandling.ReadPar(Definition.xRaySourceTypePath) == "1");
			this.parHandling.WriteValue(Acquisition.acquisitionModePath, "0", false);
			int.TryParse(this.parHandling.ReadPar((this.detectorType == DetectorType.DT || this.detectorType == DetectorType.DTNetSE || this.detectorType == DetectorType.DTNetDE || this.detectorType == DetectorType.DTNetTDI) ? Definition.detectorDTNPixelPath : Definition.detectorSesotecNPixelPath), out this.nPixel);
			this.isDualEnergyDetector = (this.detectorType == DetectorType.DTNetDE);
			bool flag6 = this.isDualEnergyDetector;
			if (flag6)
			{
				this.nPixel /= 2;
			}
			this.maxY = 50;
			this.ticksY = 2;
			this.ticksX = ((this.nPixel <= 512) ? 4 : 8);
			string a2 = this.parHandling.ReadPar(Definition.machineModePath);
			bool flag7 = a2 == 1.ToString();
			if (flag7)
			{
				this.parHandling.WriteValue("/filter/xrayoffimage/Enable/Value", "0");
			}
			this.parHandling.WriteValue(Rejecter.rejecter1MainPath + "/Enabled/Value", "0", false);
			this.parHandling.WriteValue(Rejecter.rejecter2MainPath + "/Enabled/Value", "0", false);
			this.parHandling.WriteValue(Rejecter.rejecter3MainPath + "/Enabled/Value", "0", false);
			this.parHandling.WriteValue(Rejecter.rejecter4MainPath + "/Enabled/Value", "0", false);
		}

		// Token: 0x06003480 RID: 13440 RVA: 0x00149558 File Offset: 0x00147758
		private void DeinitializePlotParameter()
		{
			string s = null;
			string s2 = null;
			string value = null;
			this.dBConnection.GetParameterValue(Acquisition.binningPath, ref value);
			this.parHandling.WriteValue(Acquisition.binningPath, value, false);
			this.dBConnection.GetParameterValue(Acquisition.binPixelPath, ref value);
			this.parHandling.WriteValue(Acquisition.binPixelPath, value, false);
			this.dBConnection.GetParameterValue(Acquisition.lane1EnablePath, ref value);
			this.parHandling.WriteValue(Acquisition.lane1EnablePath, value, false);
			this.dBConnection.GetParameterValue(Acquisition.lane2EnablePath, ref value);
			this.parHandling.WriteValue(Acquisition.lane2EnablePath, value, false);
			this.dBConnection.GetParameterValue(Acquisition.lane3EnablePath, ref value);
			this.parHandling.WriteValue(Acquisition.lane3EnablePath, value, false);
			this.dBConnection.GetParameterValue(Acquisition.lane4EnablePath, ref value);
			this.parHandling.WriteValue(Acquisition.lane4EnablePath, value, false);
			this.dBConnection.GetParameterValue(Acquisition.lane1MinPixelPath, ref value);
			this.parHandling.WriteValue(Acquisition.lane1MinPixelPath, value, false);
			this.dBConnection.GetParameterValue(Acquisition.lane1MaxPixelPath, ref value);
			this.parHandling.WriteValue(Acquisition.lane1MaxPixelPath, value, false);
			this.parHandling.WriteValue(Acquisition.sendOnlyIfContaminantPath, this.oldSendOnlyIfContaminant, false);
			this.parHandling.WriteValue(Acquisition.sendImagePath, this.oldSendImage, false);
			this.parHandling.WriteValue(ParameterHandling.continuousModePath, this.oldContinuousMode, false);
			this.parHandling.WriteValue(Acquisition.continuousModeFilteringPath, this.oldContinuousModeFiltering, false);
			bool flag = this.oldScanrate != 0;
			if (flag)
			{
				this.parHandling.WriteValue(this.scanratePath, this.oldScanrate.ToString(), this.xrayTurnedOnInPlotMenu);
			}
			bool flag2 = this.parHandling.ReadPar(ParameterHandling.ignoreLeftPixelPath) == "0";
			if (flag2)
			{
				int num = 0;
				this.dBConnection.GetIgnoredLeftPixel(ref num);
				this.parHandling.WriteValue(ParameterHandling.ignoreLeftPixelPath, num.ToString(), false);
			}
			bool flag3 = this.parHandling.ReadPar(ParameterHandling.ignoreRightPixelPath) == "0";
			if (flag3)
			{
				int num2 = 0;
				this.dBConnection.GetIgnoredRightPixel(ref num2);
				this.parHandling.WriteValue(ParameterHandling.ignoreRightPixelPath, num2.ToString(), false);
			}
			bool flag4 = this.parHandling.ReadPar(FlapsOverIOs.bulkflapsEnabledPath) == "1";
			if (flag4)
			{
				this.parHandling.WriteValue(FlapsOverIOs.bulkflapsEnableValvesPath, this.oldBulkFlapsValvesActive, true);
			}
			else
			{
				bool flag5 = this.parHandling.ReadPar(SVT.svtEnablePath) == "1";
				if (flag5)
				{
					this.parHandling.WriteValue(SVT.svtValvesEnablePath, this.oldSVTValvesActive, true);
				}
			}
			string value2 = null;
			this.dBConnection.GetParameterValue("/filter/acquisition/MaxLines/Value", ref value2);
			this.parHandling.WriteValue("/filter/acquisition/MaxLines/Value", value2, false);
			this.dBConnection.GetParameterValue("/hardware/xraysource/CurrentSetpoint/Value", ref s);
			this.dBConnection.GetParameterValue("/hardware/xraysource/VoltageSetpoint/Value", ref s2);
			double num3;
			bool flag6 = double.TryParse(s, NumberStyles.Any, new CultureInfo("en-US"), out num3) && num3 > 0.0;
			if (flag6)
			{
				this.parHandling.WriteValue("/hardware/xraysource/CurrentSetpoint/Value", num3.ToString().Replace(',', '.'), false);
			}
			double num4;
			bool flag7 = double.TryParse(s2, NumberStyles.Any, new CultureInfo("en-US"), out num4) && num4 > 0.0;
			if (flag7)
			{
				this.parHandling.WriteValue("/hardware/xraysource/VoltageSetpoint/Value", num4.ToString().Replace(',', '.'), false);
			}
			string value3 = null;
			this.dBConnection.GetParameterValue(Acquisition.acquisitionModePath, ref value3);
			this.parHandling.WriteValue(Acquisition.acquisitionModePath, value3, false);
		}

		// Token: 0x04001E1E RID: 7710
		private int ignoredRightPixel;

		// Token: 0x04001E1F RID: 7711
		private int ignoredLeftPixel;

		// Token: 0x04001E20 RID: 7712
		private int ignoredRightPixelDB;

		// Token: 0x04001E21 RID: 7713
		private int ignoredLeftPixelDB;

		// Token: 0x04001E22 RID: 7714
		private const string maxLinesPath = "/filter/acquisition/MaxLines/Value";

		// Token: 0x04001E23 RID: 7715
		private const int maxPlotFiles = 10;

		// Token: 0x04001E24 RID: 7716
		public static readonly string plotFolderName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\PlotBulk";

		// Token: 0x04001E25 RID: 7717
		private DBConnection dBConnection;

		// Token: 0x04001E26 RID: 7718
		private ParameterHandling parHandling;

		// Token: 0x04001E27 RID: 7719
		private int oldScanrate;

		// Token: 0x04001E28 RID: 7720
		private string oldContinuousModeFiltering;

		// Token: 0x04001E29 RID: 7721
		private string oldContinuousMode;

		// Token: 0x04001E2A RID: 7722
		private string oldSendOnlyIfContaminant;

		// Token: 0x04001E2B RID: 7723
		private string oldSendImage;

		// Token: 0x04001E2C RID: 7724
		private string oldBulkFlapsValvesActive;

		// Token: 0x04001E2D RID: 7725
		private string oldSVTValvesActive;

		// Token: 0x04001E2E RID: 7726
		private Timer timerUpdateParams;

		// Token: 0x04001E2F RID: 7727
		private MachineState machineState;

		// Token: 0x04001E30 RID: 7728
		private string scanratePath = "";

		// Token: 0x04001E31 RID: 7729
		private DetectorType detectorType = DetectorType.DT;

		// Token: 0x04001E32 RID: 7730
		private ImageDataTransfer imageTransfer;

		// Token: 0x04001E33 RID: 7731
		private List<Point> plotPoints;

		// Token: 0x04001E34 RID: 7732
		private List<Point> ignoreLeftPoints;

		// Token: 0x04001E35 RID: 7733
		private List<Point> ignoreRightPoints;

		// Token: 0x04001E36 RID: 7734
		private int nPixel;

		// Token: 0x04001E37 RID: 7735
		private int maxY;

		// Token: 0x04001E38 RID: 7736
		private int minY;

		// Token: 0x04001E39 RID: 7737
		private int ticksY;

		// Token: 0x04001E3A RID: 7738
		private int ticksX;

		// Token: 0x04001E3B RID: 7739
		private double zoomFactorX = 1.0;

		// Token: 0x04001E3C RID: 7740
		private bool isDualEnergyDetector;

		// Token: 0x04001E3D RID: 7741
		private EnergyLevelMode energyMode = EnergyLevelMode.AllEnergies;

		// Token: 0x04001E3E RID: 7742
		private bool saveReferencePlot;

		// Token: 0x04001E40 RID: 7744
		private double widthValve = 0.0;

		// Token: 0x04001E41 RID: 7745
		private int startPixelSecondFlap = 0;

		// Token: 0x04001E42 RID: 7746
		private int endPixelNextToLastFlap = 0;

		// Token: 0x04001E43 RID: 7747
		private int numberValves = 0;

		// Token: 0x04001E44 RID: 7748
		private int startPixel = 0;

		// Token: 0x04001E45 RID: 7749
		private int overlappingLeft = 0;

		// Token: 0x04001E46 RID: 7750
		private int overlappingRight = 0;

		// Token: 0x04001E47 RID: 7751
		private bool gainEnable;

		// Token: 0x04001E48 RID: 7752
		private bool offsetEnable;

		// Token: 0x04001E49 RID: 7753
		private bool baselineEnable;

		// Token: 0x04001E4A RID: 7754
		private double current;

		// Token: 0x04001E4B RID: 7755
		private double voltage;

		// Token: 0x04001E4C RID: 7756
		private double detectorTemperature;

		// Token: 0x04001E4D RID: 7757
		private int scanrate;

		// Token: 0x04001E4E RID: 7758
		private bool xRayEnable;

		// Token: 0x04001E4F RID: 7759
		private bool baselineVisible;

		// Token: 0x04001E50 RID: 7760
		private bool dualEnergyAvailable;

		// Token: 0x04001E51 RID: 7761
		private bool serialNumberXRayVisible;

		// Token: 0x04001E52 RID: 7762
		private string NameToChange = "";

		// Token: 0x04001E54 RID: 7764
		private List<Point> valveLines1;

		// Token: 0x04001E55 RID: 7765
		private List<Point> valveLines2;

		// Token: 0x04001E5D RID: 7773
		private bool xrayTurnedOnInPlotMenu = false;

		// Token: 0x04001E5E RID: 7774
		private double machineParametersFontSize;

		// Token: 0x04001E5F RID: 7775
		private NumKeyboard numKeyboard;

		// Token: 0x04001E60 RID: 7776
		private ZoomMode zoomMode = ZoomMode.ToMinimum;

		// Token: 0x04001E61 RID: 7777
		private DBDataLogs dbDataLogs;
	}
}
