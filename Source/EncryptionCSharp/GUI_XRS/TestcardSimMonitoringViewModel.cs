using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using Common;
using DatabaseConnection;
using Definitions;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x020000D9 RID: 217
	public class TestcardSimMonitoringViewModel : INotifyPropertyChanged
	{
		// Token: 0x1400006D RID: 109
		// (add) Token: 0x06001532 RID: 5426 RVA: 0x0007E4E4 File Offset: 0x0007C6E4
		// (remove) Token: 0x06001533 RID: 5427 RVA: 0x0007E51C File Offset: 0x0007C71C
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x17000667 RID: 1639
		// (get) Token: 0x06001534 RID: 5428 RVA: 0x0007E551 File Offset: 0x0007C751
		// (set) Token: 0x06001535 RID: 5429 RVA: 0x0007E559 File Offset: 0x0007C759
		public BoundLocalizedText StrOverview { get; set; }

		// Token: 0x17000668 RID: 1640
		// (get) Token: 0x06001536 RID: 5430 RVA: 0x0007E562 File Offset: 0x0007C762
		// (set) Token: 0x06001537 RID: 5431 RVA: 0x0007E56A File Offset: 0x0007C76A
		public BoundLocalizedText StrComplianceMode { get; set; }

		// Token: 0x17000669 RID: 1641
		// (get) Token: 0x06001538 RID: 5432 RVA: 0x0007E573 File Offset: 0x0007C773
		// (set) Token: 0x06001539 RID: 5433 RVA: 0x0007E57B File Offset: 0x0007C77B
		public BoundLocalizedText StrInitSensitivities { get; set; }

		// Token: 0x1700066A RID: 1642
		// (get) Token: 0x0600153A RID: 5434 RVA: 0x0007E584 File Offset: 0x0007C784
		// (set) Token: 0x0600153B RID: 5435 RVA: 0x0007E58C File Offset: 0x0007C78C
		public BoundLocalizedText StrActualSensitivities { get; set; }

		// Token: 0x1700066B RID: 1643
		// (get) Token: 0x0600153C RID: 5436 RVA: 0x0007E595 File Offset: 0x0007C795
		// (set) Token: 0x0600153D RID: 5437 RVA: 0x0007E59D File Offset: 0x0007C79D
		public BoundLocalizedText StrCompliance { get; set; }

		// Token: 0x1700066C RID: 1644
		// (get) Token: 0x0600153E RID: 5438 RVA: 0x0007E5A6 File Offset: 0x0007C7A6
		// (set) Token: 0x0600153F RID: 5439 RVA: 0x0007E5AE File Offset: 0x0007C7AE
		public BoundLocalizedText StrComplianceAdjustFilters { get; set; }

		// Token: 0x1700066D RID: 1645
		// (get) Token: 0x06001540 RID: 5440 RVA: 0x0007E5B7 File Offset: 0x0007C7B7
		// (set) Token: 0x06001541 RID: 5441 RVA: 0x0007E5BF File Offset: 0x0007C7BF
		public BoundLocalizedText StrResetFiltersToInitSens { get; set; }

		// Token: 0x1700066E RID: 1646
		// (get) Token: 0x06001542 RID: 5442 RVA: 0x0007E5C8 File Offset: 0x0007C7C8
		// (set) Token: 0x06001543 RID: 5443 RVA: 0x0007E5D0 File Offset: 0x0007C7D0
		public BoundLocalizedText StrVA { get; set; }

		// Token: 0x1700066F RID: 1647
		// (get) Token: 0x06001544 RID: 5444 RVA: 0x0007E5D9 File Offset: 0x0007C7D9
		// (set) Token: 0x06001545 RID: 5445 RVA: 0x0007E5E1 File Offset: 0x0007C7E1
		public BoundLocalizedText StrGlass { get; set; }

		// Token: 0x17000670 RID: 1648
		// (get) Token: 0x06001546 RID: 5446 RVA: 0x0007E5EA File Offset: 0x0007C7EA
		// (set) Token: 0x06001547 RID: 5447 RVA: 0x0007E5F2 File Offset: 0x0007C7F2
		public BoundLocalizedText StrSensitivity { get; set; }

		// Token: 0x17000671 RID: 1649
		// (get) Token: 0x06001548 RID: 5448 RVA: 0x0007E5FB File Offset: 0x0007C7FB
		// (set) Token: 0x06001549 RID: 5449 RVA: 0x0007E603 File Offset: 0x0007C803
		public RelayCommand ShowMonitoringOverviewCmd { get; set; }

		// Token: 0x17000672 RID: 1650
		// (get) Token: 0x0600154A RID: 5450 RVA: 0x0007E60C File Offset: 0x0007C80C
		// (set) Token: 0x0600154B RID: 5451 RVA: 0x0007E614 File Offset: 0x0007C814
		public RelayCommand CleanUpCommand { get; set; }

		// Token: 0x17000673 RID: 1651
		// (get) Token: 0x0600154C RID: 5452 RVA: 0x0007E61D File Offset: 0x0007C81D
		// (set) Token: 0x0600154D RID: 5453 RVA: 0x0007E625 File Offset: 0x0007C825
		public ErrorHandling errorHandling { get; set; }

		// Token: 0x17000674 RID: 1652
		// (get) Token: 0x0600154E RID: 5454 RVA: 0x0007E62E File Offset: 0x0007C82E
		// (set) Token: 0x0600154F RID: 5455 RVA: 0x0007E636 File Offset: 0x0007C836
		public List<string> BallDiameterListVA { get; set; }

		// Token: 0x17000675 RID: 1653
		// (get) Token: 0x06001550 RID: 5456 RVA: 0x0007E63F File Offset: 0x0007C83F
		// (set) Token: 0x06001551 RID: 5457 RVA: 0x0007E647 File Offset: 0x0007C847
		public List<string> BallDiameterListGlass { get; set; }

		// Token: 0x17000676 RID: 1654
		// (get) Token: 0x06001552 RID: 5458 RVA: 0x0007E650 File Offset: 0x0007C850
		// (set) Token: 0x06001553 RID: 5459 RVA: 0x0007E668 File Offset: 0x0007C868
		public string SelectedBallDiameterComplVA
		{
			get
			{
				return this.selectedBallDiameterComplVA;
			}
			set
			{
				bool flag = value != null && this.selectedBallDiameterComplVA != value;
				if (flag)
				{
					this.selectedBallDiameterComplVA = value;
					string text = "";
					bool flag2 = value == "0.3";
					if (flag2)
					{
						text = "0";
					}
					else
					{
						bool flag3 = value == "0.4";
						if (flag3)
						{
							text = "1";
						}
						else
						{
							bool flag4 = value == "0.5";
							if (flag4)
							{
								text = "2";
							}
							else
							{
								bool flag5 = value == "0.6";
								if (flag5)
								{
									text = "3";
								}
								else
								{
									bool flag6 = value == "0.8";
									if (flag6)
									{
										text = "4";
									}
									else
									{
										bool flag7 = value == "1.0";
										if (flag7)
										{
											text = "5";
										}
										else
										{
											bool flag8 = value == "1.2";
											if (flag8)
											{
												text = "6";
											}
											else
											{
												bool flag9 = value == "1.6";
												if (flag9)
												{
													text = "7";
												}
												else
												{
													bool flag10 = value == "1.8";
													if (flag10)
													{
														text = "8";
													}
													else
													{
														bool flag11 = value == "n.a.";
														if (flag11)
														{
															text = "999";
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
					this.ActivatedCMStartVA = this.setCMActivation("VA", text);
					this.parHandling.WriteValue(Definition.simulationCompSensVA, text);
					this.testcardSimService.SetProperties();
					this.OnPropertyChanged("SelectedBallDiameterComplVA");
				}
			}
		}

		// Token: 0x17000677 RID: 1655
		// (get) Token: 0x06001554 RID: 5460 RVA: 0x0007E7DC File Offset: 0x0007C9DC
		// (set) Token: 0x06001555 RID: 5461 RVA: 0x0007E7F4 File Offset: 0x0007C9F4
		public string SelectedBallDiameterComplGlass
		{
			get
			{
				return this.selectedBallDiameterComplGlass;
			}
			set
			{
				bool flag = value != null && this.selectedBallDiameterComplGlass != value;
				if (flag)
				{
					this.selectedBallDiameterComplGlass = value;
					string text = "";
					bool flag2 = value == "0.6";
					if (flag2)
					{
						text = "0";
					}
					else
					{
						bool flag3 = value == "0.8";
						if (flag3)
						{
							text = "1";
						}
						else
						{
							bool flag4 = value == "1.0";
							if (flag4)
							{
								text = "2";
							}
							else
							{
								bool flag5 = value == "1.2";
								if (flag5)
								{
									text = "3";
								}
								else
								{
									bool flag6 = value == "1.5";
									if (flag6)
									{
										text = "4";
									}
									else
									{
										bool flag7 = value == "2.0";
										if (flag7)
										{
											text = "5";
										}
										else
										{
											bool flag8 = value == "2.5";
											if (flag8)
											{
												text = "6";
											}
											else
											{
												bool flag9 = value == "3.0";
												if (flag9)
												{
													text = "7";
												}
												else
												{
													bool flag10 = value == "3.5";
													if (flag10)
													{
														text = "8";
													}
													else
													{
														bool flag11 = value == "4.0";
														if (flag11)
														{
															text = "9";
														}
														else
														{
															bool flag12 = value == "6.0";
															if (flag12)
															{
																text = "10";
															}
															else
															{
																bool flag13 = value == "7.0";
																if (flag13)
																{
																	text = "11";
																}
																else
																{
																	bool flag14 = value == "8.0";
																	if (flag14)
																	{
																		text = "12";
																	}
																	else
																	{
																		bool flag15 = value == "9.0";
																		if (flag15)
																		{
																			text = "13";
																		}
																		else
																		{
																			bool flag16 = value == "10.0";
																			if (flag16)
																			{
																				text = "14";
																			}
																			else
																			{
																				bool flag17 = value == "n.a.";
																				if (flag17)
																				{
																					text = "999";
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
							}
						}
					}
					this.ActivatedCMStartGlass = this.setCMActivation("Glass", text);
					this.parHandling.WriteValue(Definition.simulationCompSensGlass, text);
					this.testcardSimService.SetProperties();
					this.OnPropertyChanged("SelectedBallDiameterComplGlass");
				}
			}
		}

		// Token: 0x17000678 RID: 1656
		// (get) Token: 0x06001556 RID: 5462 RVA: 0x0007EA0F File Offset: 0x0007CC0F
		// (set) Token: 0x06001557 RID: 5463 RVA: 0x0007EA17 File Offset: 0x0007CC17
		public string InitVASensitivity { get; set; }

		// Token: 0x17000679 RID: 1657
		// (get) Token: 0x06001558 RID: 5464 RVA: 0x0007EA20 File Offset: 0x0007CC20
		// (set) Token: 0x06001559 RID: 5465 RVA: 0x0007EA28 File Offset: 0x0007CC28
		public string InitGlassSensitivity { get; set; }

		// Token: 0x1700067A RID: 1658
		// (get) Token: 0x0600155A RID: 5466 RVA: 0x0007EA31 File Offset: 0x0007CC31
		// (set) Token: 0x0600155B RID: 5467 RVA: 0x0007EA39 File Offset: 0x0007CC39
		public string InitSensitivitiesWithUnit { get; set; }

		// Token: 0x1700067B RID: 1659
		// (get) Token: 0x0600155C RID: 5468 RVA: 0x0007EA44 File Offset: 0x0007CC44
		// (set) Token: 0x0600155D RID: 5469 RVA: 0x0007EA5C File Offset: 0x0007CC5C
		public string ActualVASensitivity
		{
			get
			{
				return this.actualVASensitivity;
			}
			set
			{
				bool flag = this.actualVASensitivity != value;
				if (flag)
				{
					this.actualVASensitivity = value;
					this.OnPropertyChanged("ActualVASensitivity");
				}
			}
		}

		// Token: 0x1700067C RID: 1660
		// (get) Token: 0x0600155E RID: 5470 RVA: 0x0007EA90 File Offset: 0x0007CC90
		// (set) Token: 0x0600155F RID: 5471 RVA: 0x0007EAA8 File Offset: 0x0007CCA8
		public string ActualGlassSensitivity
		{
			get
			{
				return this.actualGlassSensitivity;
			}
			set
			{
				bool flag = this.actualGlassSensitivity != value;
				if (flag)
				{
					this.actualGlassSensitivity = value;
					this.OnPropertyChanged("ActualGlassSensitivity");
				}
			}
		}

		// Token: 0x1700067D RID: 1661
		// (get) Token: 0x06001560 RID: 5472 RVA: 0x0007EADB File Offset: 0x0007CCDB
		// (set) Token: 0x06001561 RID: 5473 RVA: 0x0007EAE3 File Offset: 0x0007CCE3
		public string ActualSensitivitiesWithUnit { get; set; }

		// Token: 0x1700067E RID: 1662
		// (get) Token: 0x06001562 RID: 5474 RVA: 0x0007EAEC File Offset: 0x0007CCEC
		// (set) Token: 0x06001563 RID: 5475 RVA: 0x0007EAF4 File Offset: 0x0007CCF4
		public string CompSensitivityGlass { get; set; }

		// Token: 0x1700067F RID: 1663
		// (get) Token: 0x06001564 RID: 5476 RVA: 0x0007EAFD File Offset: 0x0007CCFD
		// (set) Token: 0x06001565 RID: 5477 RVA: 0x0007EB05 File Offset: 0x0007CD05
		public string CompSensitivityVA { get; set; }

		// Token: 0x17000680 RID: 1664
		// (get) Token: 0x06001566 RID: 5478 RVA: 0x0007EB0E File Offset: 0x0007CD0E
		// (set) Token: 0x06001567 RID: 5479 RVA: 0x0007EB16 File Offset: 0x0007CD16
		public string CompSensitivityAdjustFilters { get; set; }

		// Token: 0x17000681 RID: 1665
		// (get) Token: 0x06001568 RID: 5480 RVA: 0x0007EB1F File Offset: 0x0007CD1F
		// (set) Token: 0x06001569 RID: 5481 RVA: 0x0007EB27 File Offset: 0x0007CD27
		public string ResetSensitivityToInitSens { get; set; }

		// Token: 0x17000682 RID: 1666
		// (get) Token: 0x0600156A RID: 5482 RVA: 0x0007EB30 File Offset: 0x0007CD30
		// (set) Token: 0x0600156B RID: 5483 RVA: 0x0007EB38 File Offset: 0x0007CD38
		public List<string> ListMessLev { get; set; }

		// Token: 0x17000683 RID: 1667
		// (get) Token: 0x0600156C RID: 5484 RVA: 0x0007EB41 File Offset: 0x0007CD41
		// (set) Token: 0x0600156D RID: 5485 RVA: 0x0007EB49 File Offset: 0x0007CD49
		public bool ActivateICMParams { get; set; }

		// Token: 0x17000684 RID: 1668
		// (get) Token: 0x0600156E RID: 5486 RVA: 0x0007EB52 File Offset: 0x0007CD52
		// (set) Token: 0x0600156F RID: 5487 RVA: 0x0007EB5A File Offset: 0x0007CD5A
		public RelayCommand StartVAComplianceAdjustmentsCmd { get; set; }

		// Token: 0x17000685 RID: 1669
		// (get) Token: 0x06001570 RID: 5488 RVA: 0x0007EB63 File Offset: 0x0007CD63
		// (set) Token: 0x06001571 RID: 5489 RVA: 0x0007EB6B File Offset: 0x0007CD6B
		public RelayCommand StartGlassComplianceAdjustmentsCmd { get; set; }

		// Token: 0x17000686 RID: 1670
		// (get) Token: 0x06001572 RID: 5490 RVA: 0x0007EB74 File Offset: 0x0007CD74
		// (set) Token: 0x06001573 RID: 5491 RVA: 0x0007EB7C File Offset: 0x0007CD7C
		public RelayCommand StartFilterResetToInitSensitivitiesCmd { get; set; }

		// Token: 0x17000687 RID: 1671
		// (get) Token: 0x06001574 RID: 5492 RVA: 0x0007EB88 File Offset: 0x0007CD88
		// (set) Token: 0x06001575 RID: 5493 RVA: 0x0007EBA0 File Offset: 0x0007CDA0
		public bool ActivatedCMStartVA
		{
			get
			{
				return this.activatedCMStartVA;
			}
			set
			{
				bool flag = this.activatedCMStartVA != value;
				if (flag)
				{
					this.activatedCMStartVA = value;
					this.OnPropertyChanged("ActivatedCMStartVA");
				}
			}
		}

		// Token: 0x17000688 RID: 1672
		// (get) Token: 0x06001576 RID: 5494 RVA: 0x0007EBD4 File Offset: 0x0007CDD4
		// (set) Token: 0x06001577 RID: 5495 RVA: 0x0007EBEC File Offset: 0x0007CDEC
		public bool ActivatedCMStartGlass
		{
			get
			{
				return this.activatedCMStartGlass;
			}
			set
			{
				bool flag = this.activatedCMStartGlass != value;
				if (flag)
				{
					this.activatedCMStartGlass = value;
					this.OnPropertyChanged("ActivatedCMStartGlass");
				}
			}
		}

		// Token: 0x17000689 RID: 1673
		// (get) Token: 0x06001578 RID: 5496 RVA: 0x0007EC20 File Offset: 0x0007CE20
		// (set) Token: 0x06001579 RID: 5497 RVA: 0x0007EC38 File Offset: 0x0007CE38
		public bool ActivatedResetToInitSens
		{
			get
			{
				return this.activatedResetToInitSens;
			}
			set
			{
				bool flag = this.activatedResetToInitSens != value;
				if (flag)
				{
					this.activatedResetToInitSens = value;
					this.OnPropertyChanged("ActivatedResetToInitSens");
				}
			}
		}

		// Token: 0x1700068A RID: 1674
		// (get) Token: 0x0600157A RID: 5498 RVA: 0x0007EC6B File Offset: 0x0007CE6B
		// (set) Token: 0x0600157B RID: 5499 RVA: 0x0007EC73 File Offset: 0x0007CE73
		public bool IsAINotAvailable { get; set; }

		// Token: 0x1700068B RID: 1675
		// (get) Token: 0x0600157C RID: 5500 RVA: 0x0007EC7C File Offset: 0x0007CE7C
		// (set) Token: 0x0600157D RID: 5501 RVA: 0x0007EC84 File Offset: 0x0007CE84
		public double IsAIAvailableWidth { get; set; } = 0.0;

		// Token: 0x0600157E RID: 5502 RVA: 0x0007EC90 File Offset: 0x0007CE90
		public TestcardSimMonitoringViewModel(ParameterHandling parHandling, TestcardSimService testcardSimService, DBConnection dbConnection, ProductManager productManager, ErrorHandling errorHandling, AccessRightsManager accessManager, XRSImage imageForComplianceMode, ImageDataTransfer imageTransfer, StoreImageData imageArchiver)
		{
			this.parHandling = parHandling;
			this.testcardSimService = testcardSimService;
			this.dbConnection = dbConnection;
			this.productManager = productManager;
			this.accessManager = accessManager;
			this.imageForComplianceMode = imageForComplianceMode;
			this.imageTransfer = imageTransfer;
			this.errorHandling = errorHandling;
			this.imageArchiver = imageArchiver;
			this.RegisterLocalizedText();
			this.InitSensitivitiesWithUnit = this.StrInitSensitivities.LocalizedText + " (mm)";
			this.ActualSensitivitiesWithUnit = this.StrActualSensitivities.LocalizedText + " (mm)";
			this.CompSensitivityVA = string.Concat(new string[]
			{
				this.StrCompliance.LocalizedText,
				" ",
				this.StrVA.LocalizedText,
				" ",
				this.StrSensitivity.LocalizedText,
				" (mm)"
			});
			this.CompSensitivityGlass = string.Concat(new string[]
			{
				this.StrCompliance.LocalizedText,
				" ",
				this.StrGlass.LocalizedText,
				" ",
				this.StrSensitivity.LocalizedText,
				" (mm)"
			});
			this.CompSensitivityAdjustFilters = this.StrComplianceAdjustFilters.LocalizedText;
			this.ResetSensitivityToInitSens = this.StrResetFiltersToInitSens.LocalizedText;
			this.IsAINotAvailable = !(parHandling.ReadPar(Definition.anomalyDetectionEnabled) == "1");
			bool flag = !this.IsAINotAvailable;
			if (flag)
			{
				this.IsAIAvailableWidth = 55.0;
			}
			int num = Convert.ToInt32(parHandling.ReadPar(Definition.simulationActSensVA));
			int num2 = Convert.ToInt32(parHandling.ReadPar(Definition.simulationActSensGlass));
			int num3 = Convert.ToInt32(parHandling.ReadPar(Definition.simulationCompSensVA));
			int num4 = Convert.ToInt32(parHandling.ReadPar(Definition.simulationCompSensGlass));
			int num5 = Convert.ToInt32(parHandling.ReadPar(Definition.simulationInitSensVA));
			int num6 = Convert.ToInt32(parHandling.ReadPar(Definition.simulationInitSensGlass));
			this.ActualVASensitivity = testcardSimService.ConvertBallSizeToString("VA", num, true, false);
			this.ActualGlassSensitivity = testcardSimService.ConvertBallSizeToString("Glass", num2, true, false);
			this.SelectedBallDiameterComplVA = testcardSimService.ConvertBallSizeToString("VA", num3, true, false);
			this.SelectedBallDiameterComplGlass = testcardSimService.ConvertBallSizeToString("Glass", num4, true, false);
			this.InitVASensitivity = testcardSimService.ConvertBallSizeToString("VA", num5, true, false);
			this.InitGlassSensitivity = testcardSimService.ConvertBallSizeToString("Glass", num6, true, false);
			this.InitializeBallDiameterList(num, num2, num3, num4);
			this.ActivateICMParams = (accessManager.CurrentUser.Rights <= BuiltInPriviledges.Operator1);
			bool flag2 = !this.ActivateICMParams;
			if (flag2)
			{
				this.ActivatedResetToInitSens = (this.productIsteachedIn() && (num5 <= Definition.sumOfVATestcards || num6 <= Definition.sumOfGlassTestcards) && (num5 != num || num6 != num2));
			}
			else
			{
				this.ActivatedResetToInitSens = false;
			}
			this.ShowMonitoringOverviewCmd = new RelayCommand(new Action<object>(this.ShowMonitoringOverview));
			this.StartVAComplianceAdjustmentsCmd = new RelayCommand(new Action<object>(this.StartVAComplianceAdjustments));
			this.StartGlassComplianceAdjustmentsCmd = new RelayCommand(new Action<object>(this.StartGlassComplianceAdjustments));
			this.StartFilterResetToInitSensitivitiesCmd = new RelayCommand(new Action<object>(this.StartResetingToInitSens));
			this.CleanUpCommand = new RelayCommand(new Action<object>(this.CleanUp));
		}

		// Token: 0x0600157F RID: 5503 RVA: 0x0007F024 File Offset: 0x0007D224
		private void InitializeBallDiameterList(int vaSensAct, int glSensAct, int vaComplAct, int glComplAct)
		{
			this.BallDiameterListVA = new List<string>();
			this.BallDiameterListGlass = new List<string>();
			List<string> list = new List<string>();
			bool flag = vaSensAct > vaComplAct;
			if (flag)
			{
				this.BallDiameterListVA.Add(this.testcardSimService.ConvertBallSizeToString("VA", vaComplAct, true, false));
			}
			bool flag2 = glSensAct > glComplAct;
			if (flag2)
			{
				this.BallDiameterListGlass.Add(this.testcardSimService.ConvertBallSizeToString("Glass", glComplAct, true, false));
			}
			bool flag3 = vaSensAct > Definition.sumOfVATestcards;
			if (flag3)
			{
				this.BallDiameterListVA.Add("n.a.");
			}
			else
			{
				list.Add("0.3");
				list.Add("0.4");
				list.Add("0.5");
				list.Add("0.6");
				list.Add("0.8");
				list.Add("1.0");
				list.Add("1.2");
				list.Add("1.6");
				list.Add("1.8");
				list.Add("n.a.");
				for (int i = vaSensAct; i < Definition.sumOfVATestcards + 1; i++)
				{
					this.BallDiameterListVA.Add(list[i]);
				}
			}
			bool flag4 = glSensAct > Definition.sumOfGlassTestcards;
			if (flag4)
			{
				this.BallDiameterListGlass.Add("n.a.");
			}
			else
			{
				list = new List<string>();
				list.Add("0.6");
				list.Add("0.8");
				list.Add("1.0");
				list.Add("1.2");
				list.Add("1.5");
				list.Add("2.0");
				list.Add("2.5");
				list.Add("3.0");
				list.Add("3.5");
				list.Add("4.0");
				list.Add("6.0");
				list.Add("7.0");
				list.Add("8.0");
				list.Add("9.0");
				list.Add("10.0");
				list.Add("n.a.");
				for (int j = glSensAct; j < Definition.sumOfGlassTestcards + 1; j++)
				{
					this.BallDiameterListGlass.Add(list[j]);
				}
			}
			this.OnPropertyChanged("BallDiameterListVA");
			this.OnPropertyChanged("BallDiameterListGlass");
		}

		// Token: 0x06001580 RID: 5504 RVA: 0x0007F2A4 File Offset: 0x0007D4A4
		private bool productIsteachedIn()
		{
			bool result = false;
			int num = Convert.ToInt32(this.parHandling.ReadPar(Definition.filterMainPath + Definition.comparerSubPath + "1/Contrast/" + Definition.strValue));
			bool flag = num > 0 && num < 32000;
			if (flag)
			{
				result = true;
			}
			return result;
		}

		// Token: 0x06001581 RID: 5505 RVA: 0x0007F2F8 File Offset: 0x0007D4F8
		private void ShowMonitoringOverview(object obj)
		{
			CtrlTestcardSimMonitoringOverview ctrlTestcardSimMonitoringOverview = new CtrlTestcardSimMonitoringOverview(this.parHandling, this.testcardSimService, this.dbConnection, this.productManager, this.errorHandling, this.accessManager);
			Subwindow.Show(ctrlTestcardSimMonitoringOverview, SubwindowButtons.BackAndHome, ctrlTestcardSimMonitoringOverview.PanelButtons, ctrlTestcardSimMonitoringOverview.UiElementRightAligned);
		}

		// Token: 0x06001582 RID: 5506 RVA: 0x0007F344 File Offset: 0x0007D544
		private void StartVAComplianceAdjustments(object obj)
		{
			bool flag = this.parHandling.ReadPar(XrayState.xrayPowerOnPath) == "1";
			if (flag)
			{
				Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtTurnXrayOff"), DialogStyle.Error, DialogButtons.OK);
			}
			else
			{
				bool flag2 = this.imageForComplianceMode != null;
				if (flag2)
				{
					bool flag3 = Dialog.Show(LocalizationManager.Localize("txtInfo"), this.setMessageTextForComplianceMode(1), DialogStyle.Info, DialogButtons.OKCancel) == Dialog.DialogResults.Ok;
					if (flag3)
					{
						this.resetSimulationStatisticValues();
						Progress.ShowStaticProgress(LocalizationManager.Localize("txtProcessing"), true);
						new Thread(delegate()
						{
							this.imageTransfer.FrameArrived += this.imageTestcardSim_FrameArrived;
							bool flag4 = this.testcardSimService.TestcardSimForComplianceSensitivities(this.imageForComplianceMode, 1, true);
							if (flag4)
							{
								this.checkSensitivites(this.imageForComplianceMode);
								this.vaWasSimulated = true;
							}
							else
							{
								this.parHandling.WriteValue(Definition.simulationmodelPath, "0");
								this.imageTransfer.FrameArrived -= this.imageTestcardSim_FrameArrived;
								Progress.CloseStaticProgress(false);
								Thread.Sleep(1000);
								bool flag5 = Application.Current != null;
								if (flag5)
								{
									Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
									{
										bool checkVersionAndXRaySource = this.testcardSimService.CheckVersionAndXRaySource;
										if (checkVersionAndXRaySource)
										{
											Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtTestcardSimNotPossible"), DialogStyle.Warning, DialogButtons.OK);
										}
									}));
								}
							}
						})
						{
							Name = "SimComplianceModeVA"
						}.Start();
					}
				}
			}
		}

		// Token: 0x06001583 RID: 5507 RVA: 0x0007F400 File Offset: 0x0007D600
		private void StartGlassComplianceAdjustments(object obj)
		{
			bool flag = this.parHandling.ReadPar(XrayState.xrayPowerOnPath) == "1";
			if (flag)
			{
				Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtTurnXrayOff"), DialogStyle.Error, DialogButtons.OK);
			}
			else
			{
				bool flag2 = this.imageForComplianceMode != null;
				if (flag2)
				{
					bool flag3 = Dialog.Show(LocalizationManager.Localize("txtInfo"), this.setMessageTextForComplianceMode(2), DialogStyle.Info, DialogButtons.OKCancel) == Dialog.DialogResults.Ok;
					if (flag3)
					{
						this.resetSimulationStatisticValues();
						Progress.ShowStaticProgress(LocalizationManager.Localize("txtProcessing"), true);
						new Thread(delegate()
						{
							this.imageTransfer.FrameArrived += this.imageTestcardSim_FrameArrived;
							bool flag4 = this.testcardSimService.TestcardSimForComplianceSensitivities(this.imageForComplianceMode, 2, true);
							if (flag4)
							{
								this.checkSensitivites(this.imageForComplianceMode);
								this.glassWasSimulated = true;
							}
							else
							{
								this.parHandling.WriteValue(Definition.simulationmodelPath, "0");
								this.imageTransfer.FrameArrived -= this.imageTestcardSim_FrameArrived;
								Progress.CloseStaticProgress(false);
								Thread.Sleep(1000);
								bool flag5 = Application.Current != null;
								if (flag5)
								{
									Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
									{
										bool checkVersionAndXRaySource = this.testcardSimService.CheckVersionAndXRaySource;
										if (checkVersionAndXRaySource)
										{
											Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtTestcardSimNotPossible"), DialogStyle.Warning, DialogButtons.OK);
										}
									}));
								}
							}
						})
						{
							Name = "SimComplianceModeGlass"
						}.Start();
					}
				}
			}
		}

		// Token: 0x06001584 RID: 5508 RVA: 0x0007F4BC File Offset: 0x0007D6BC
		private string setMessageTextForComplianceMode(int material)
		{
			string text = "";
			string text2 = "";
			bool flag = material == 1;
			if (flag)
			{
				text = this.testcardSimService.ConvertBallSizeToString("VA", Convert.ToInt32(this.parHandling.ReadPar(Definition.simulationCompSensVA)), true, true);
				text2 = this.StrVA.LocalizedText;
			}
			else
			{
				bool flag2 = material == 2;
				if (flag2)
				{
					text = this.testcardSimService.ConvertBallSizeToString("Glass", Convert.ToInt32(this.parHandling.ReadPar(Definition.simulationCompSensGlass)), true, true);
					text2 = this.StrGlass.LocalizedText;
				}
			}
			return string.Concat(new string[]
			{
				this.StrComplianceAdjustFilters.LocalizedText,
				" ",
				text2,
				" ",
				text,
				" ?"
			});
		}

		// Token: 0x06001585 RID: 5509 RVA: 0x0007F594 File Offset: 0x0007D794
		private string setMessageTextForNotReachingComplianceSens(int material)
		{
			string text = "";
			string text2 = "";
			string text3 = "";
			bool flag = material == 1;
			if (flag)
			{
				text = this.testcardSimService.ConvertBallSizeToString("VA", Convert.ToInt32(this.parHandling.ReadPar(Definition.simulationCompSensVA)), true, true);
				text2 = this.testcardSimService.ConvertBallSizeToString("VA", Convert.ToInt32(this.parHandling.ReadPar(Definition.simulationActSensVA)), true, true);
				text3 = this.StrVA.LocalizedText;
			}
			else
			{
				bool flag2 = material == 2;
				if (flag2)
				{
					text = this.testcardSimService.ConvertBallSizeToString("Glass", Convert.ToInt32(this.parHandling.ReadPar(Definition.simulationCompSensGlass)), true, true);
					text2 = this.testcardSimService.ConvertBallSizeToString("Glass", Convert.ToInt32(this.parHandling.ReadPar(Definition.simulationActSensGlass)), true, true);
					text3 = this.StrGlass.LocalizedText;
				}
			}
			return string.Concat(new string[]
			{
				LocalizationManager.Localize("txtComplianceNotPossible"),
				Environment.NewLine,
				text3,
				": ",
				text,
				" --> ",
				text2,
				Environment.NewLine,
				Environment.NewLine,
				LocalizationManager.Localize("txtSetComplSensQuestion")
			});
		}

		// Token: 0x06001586 RID: 5510 RVA: 0x0007F6E8 File Offset: 0x0007D8E8
		private string setMessageTextForWrongDetectedPixel()
		{
			string str = string.Concat(new string[]
			{
				LocalizationManager.Localize("txtWrongPixelTestcardSim"),
				": ",
				this.parHandling.ReadPar(Definition.simulationWrongDetPix),
				Environment.NewLine,
				Environment.NewLine
			});
			string str2 = LocalizationManager.Localize("txtFirstImageHistoryOrNotTeachedIn");
			return str + str2;
		}

		// Token: 0x06001587 RID: 5511 RVA: 0x0007F75C File Offset: 0x0007D95C
		private void StartResetingToInitSens(object obj)
		{
			this.resetToInitSens(false);
		}

		// Token: 0x06001588 RID: 5512 RVA: 0x0007F768 File Offset: 0x0007D968
		private void resetToInitSens(bool withMessage)
		{
			bool flag = this.parHandling.ReadPar(XrayState.xrayPowerOnPath) == "1";
			if (flag)
			{
				Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtTurnXrayOff"), DialogStyle.Error, DialogButtons.OK);
			}
			else
			{
				bool flag2 = this.imageForComplianceMode != null;
				if (flag2)
				{
					bool flag3 = withMessage || Dialog.Show(LocalizationManager.Localize("txtInfo"), this.StrResetFiltersToInitSens.LocalizedText + "?", DialogStyle.Info, DialogButtons.OKCancel) == Dialog.DialogResults.Ok;
					if (flag3)
					{
						this.resetSimulationStatisticValues();
						Progress.ShowStaticProgress(LocalizationManager.Localize("txtProcessing"), true);
						new Thread(delegate()
						{
							this.imageTransfer.FrameArrived += this.imageTestcardSim_FrameArrived;
							this.testcardSimService.SetContrastsBackToInitSensitivities();
							this.checkSensitivites(this.imageForComplianceMode);
						})
						{
							Name = "ResetFiltersToInitSens"
						}.Start();
					}
				}
			}
		}

		// Token: 0x06001589 RID: 5513 RVA: 0x0007F83C File Offset: 0x0007DA3C
		private void checkSensitivites(XRSImage complianceImg)
		{
			this.testcardSimService.TestcardSimAfterTrainingSendImg(false, complianceImg, false);
			bool flag = Application.Current != null;
			if (flag)
			{
				Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
				{
					bool checkVersionAndXRaySource = this.testcardSimService.CheckVersionAndXRaySource;
					if (checkVersionAndXRaySource)
					{
						Progress.CloseStaticProgress(false);
						this.imageTransfer.FrameArrived -= this.imageTestcardSim_FrameArrived;
						Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtTestcardSimNotPossible"), DialogStyle.Warning, DialogButtons.OK);
					}
				}));
			}
		}

		// Token: 0x0600158A RID: 5514 RVA: 0x0007F888 File Offset: 0x0007DA88
		private void imageTestcardSim_FrameArrived(object sender, ReceiveEventArgs e)
		{
			bool flag = e.Image.Type == ImageType.TestcardSimulation;
			if (flag)
			{
				bool flag2 = this.parHandling.ReadPar(Definition.simulationsaveSimImages) == "1";
				if (flag2)
				{
					this.imageArchiver.ArchiveSimulatedImage(e.Image, 242, this.testcardSimService.ConvertBallSizeToString(e.Image.TestcardSimMaterial, (int)e.Image.TestcardSimBallSize, false, true), this.productManager.CurrentLoadedProduct.ProductNumber, this.productManager.CurrentLoadedProduct.ProductName);
				}
				bool flag3 = this.testcardSimService.TestcardSimAfterTrainingGetImg(e.Image, false);
				if (flag3)
				{
					Progress.CloseStaticProgress(false);
					this.imageTransfer.FrameArrived -= this.imageTestcardSim_FrameArrived;
					bool flag4 = Application.Current != null;
					if (flag4)
					{
						Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
						{
							bool flag5 = Dialog.Show(LocalizationManager.Localize("txtInfo"), this.testcardSimService.setComplianceTestcardSimMsg(), DialogStyle.Info, DialogButtons.OK) == Dialog.DialogResults.Ok;
							if (flag5)
							{
								this.testcardSimService.setParameterAfterTestcardSim(false);
								int num = Convert.ToInt32(this.parHandling.ReadPar(Definition.simulationActSensGlass));
								int num2 = Convert.ToInt32(this.parHandling.ReadPar(Definition.simulationCompSensGlass));
								int num3 = Convert.ToInt32(this.parHandling.ReadPar(Definition.simulationActSensVA));
								int num4 = Convert.ToInt32(this.parHandling.ReadPar(Definition.simulationCompSensVA));
								bool flag6 = false;
								this.ActualVASensitivity = this.testcardSimService.ConvertBallSizeToString("VA", num3, true, false);
								this.ActualGlassSensitivity = this.testcardSimService.ConvertBallSizeToString("Glass", num, true, false);
								bool flag7 = this.vaWasSimulated && num3 > num4;
								if (flag7)
								{
									bool flag8 = Dialog.Show(LocalizationManager.Localize("txtWarning"), this.setMessageTextForNotReachingComplianceSens(1), DialogStyle.Warning, DialogButtons.OKCancel) == Dialog.DialogResults.Cancel;
									if (flag8)
									{
										bool flag9 = Dialog.Show(LocalizationManager.Localize("txtInfo"), LocalizationManager.Localize("txtSetInitSens"), DialogStyle.Info, DialogButtons.OK) == Dialog.DialogResults.Ok;
										if (flag9)
										{
											this.resetToInitSens(true);
											flag6 = true;
										}
									}
								}
								bool flag10 = this.glassWasSimulated && num > num2;
								if (flag10)
								{
									bool flag11 = Dialog.Show(LocalizationManager.Localize("txtWarning"), this.setMessageTextForNotReachingComplianceSens(2), DialogStyle.Warning, DialogButtons.OKCancel) == Dialog.DialogResults.Cancel;
									if (flag11)
									{
										bool flag12 = Dialog.Show(LocalizationManager.Localize("txtInfo"), LocalizationManager.Localize("txtSetInitSens"), DialogStyle.Info, DialogButtons.OK) == Dialog.DialogResults.Ok;
										if (flag12)
										{
											this.resetToInitSens(true);
											flag6 = true;
										}
									}
								}
								bool flag13 = !flag6;
								if (flag13)
								{
									bool flag14 = this.glassWasSimulated || this.vaWasSimulated;
									if (flag14)
									{
										this.ActivatedResetToInitSens = true;
										bool flag15 = this.vaWasSimulated;
										if (flag15)
										{
											this.ActivatedCMStartVA = false;
										}
										bool flag16 = this.glassWasSimulated;
										if (flag16)
										{
											this.ActivatedCMStartGlass = false;
										}
									}
									else
									{
										this.ActivatedCMStartGlass = false;
										this.ActivatedCMStartVA = false;
										this.ActivatedResetToInitSens = false;
									}
									this.SelectedBallDiameterComplVA = this.testcardSimService.ConvertBallSizeToString("VA", num3, true, false);
									this.SelectedBallDiameterComplGlass = this.testcardSimService.ConvertBallSizeToString("Glass", num, true, false);
									num2 = Convert.ToInt32(this.parHandling.ReadPar(Definition.simulationCompSensGlass));
									num4 = Convert.ToInt32(this.parHandling.ReadPar(Definition.simulationCompSensVA));
									this.InitializeBallDiameterList(num3, num, num4, num2);
									this.parHandling.WriteValue(Definition.simulationmodelPath, "0");
									bool flag17 = this.testcardSimService.CheckWrongDetectedPixel(0) && this.parHandling.ReadPar(Definition.anomalyDetectionInferenceEnabled) != "1";
									if (flag17)
									{
										Dialog.Show(LocalizationManager.Localize("txtWarning"), this.setMessageTextForWrongDetectedPixel(), DialogStyle.Warning, DialogButtons.OK);
									}
									bool checkBallCounts = this.testcardSimService.CheckBallCounts;
									if (checkBallCounts)
									{
										Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtErrorGeneralTestcardSim"), DialogStyle.Warning, DialogButtons.OK);
									}
									bool checkSensitivityDecrease = this.testcardSimService.CheckSensitivityDecrease;
									if (checkSensitivityDecrease)
									{
										bool flag18 = Dialog.Show(LocalizationManager.Localize("txtWarning"), string.Concat(new string[]
										{
											LocalizationManager.Localize("txtDecreasesTestcardSim"),
											". ",
											Environment.NewLine,
											Environment.NewLine,
											LocalizationManager.Localize("txtSetComplSensQuestion")
										}), DialogStyle.Warning, DialogButtons.OKCancel) == Dialog.DialogResults.Cancel;
										if (flag18)
										{
											bool flag19 = Dialog.Show(LocalizationManager.Localize("txtInfo"), LocalizationManager.Localize("txtSetInitSens"), DialogStyle.Info, DialogButtons.OK) == Dialog.DialogResults.Ok;
											if (flag19)
											{
												this.resetToInitSens(true);
											}
										}
									}
									bool flag20 = this.checkActualSensInComparisonToInitSens(1, num3, num);
									if (flag20)
									{
										Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtActSensWorseInitSens"), DialogStyle.Warning, DialogButtons.OK);
									}
									else
									{
										bool flag21 = this.checkActualSensInComparisonToInitSens(0, num3, num);
										if (flag21)
										{
											Dialog.Show(LocalizationManager.Localize("txtInfo"), LocalizationManager.Localize("txtActSensBetterInitSens"), DialogStyle.Info, DialogButtons.OK);
										}
									}
								}
								this.setSimulationStatisticValues();
								this.vaWasSimulated = false;
								this.glassWasSimulated = false;
							}
						}));
					}
				}
			}
		}

		// Token: 0x0600158B RID: 5515 RVA: 0x0007F988 File Offset: 0x0007DB88
		private bool checkActualSensInComparisonToInitSens(int mode, int vaact, int glact)
		{
			int num = Convert.ToInt32(this.parHandling.ReadPar(Definition.simulationInitSensGlass));
			int num2 = Convert.ToInt32(this.parHandling.ReadPar(Definition.simulationInitSensVA));
			bool result = false;
			bool flag = !this.vaWasSimulated && !this.glassWasSimulated;
			if (flag)
			{
				bool flag2 = mode == 0 && ((num2 - 1 >= 0 && vaact == num2 - 1) || (num - 1 >= 0 && glact == num - 1));
				if (flag2)
				{
					result = true;
				}
				else
				{
					bool flag3 = mode == 1 && ((num2 + 1 < Definition.sumOfVATestcards && vaact == num2 + 1) || (num + 1 < Definition.sumOfGlassTestcards && glact == num + 1));
					if (flag3)
					{
						result = true;
					}
				}
			}
			return result;
		}

		// Token: 0x0600158C RID: 5516 RVA: 0x0007FA48 File Offset: 0x0007DC48
		private bool setCMActivation(string material, string stcomplsens)
		{
			int num = Convert.ToInt32(stcomplsens);
			bool result = true;
			bool flag = material == "VA";
			if (flag)
			{
				int num2 = Convert.ToInt32(this.parHandling.ReadPar(Definition.simulationActSensVA));
				result = (num > num2);
			}
			else
			{
				bool flag2 = material == "Glass";
				if (flag2)
				{
					int num2 = Convert.ToInt32(this.parHandling.ReadPar(Definition.simulationActSensGlass));
					result = (num > num2);
				}
			}
			return result;
		}

		// Token: 0x0600158D RID: 5517 RVA: 0x0007FAC4 File Offset: 0x0007DCC4
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x0600158E RID: 5518 RVA: 0x0007FAF4 File Offset: 0x0007DCF4
		private void RegisterLocalizedText()
		{
			this.StrComplianceMode = new BoundLocalizedText("txtComplianceMode");
			this.StrOverview = new BoundLocalizedText("menuItemOverview");
			this.StrInitSensitivities = new BoundLocalizedText("txtInitSensitivities");
			this.StrActualSensitivities = new BoundLocalizedText("txtActualSensitivities");
			this.StrCompliance = new BoundLocalizedText("txtCompliance");
			this.StrComplianceAdjustFilters = new BoundLocalizedText("txtComplianceAdjustFilters");
			this.StrResetFiltersToInitSens = new BoundLocalizedText("txtResetFiltersToInitSens");
			this.StrVA = new BoundLocalizedText("txtVA");
			this.StrGlass = new BoundLocalizedText("txtGlass");
			this.StrSensitivity = new BoundLocalizedText("txtSensitivity");
		}

		// Token: 0x0600158F RID: 5519 RVA: 0x0007FBAC File Offset: 0x0007DDAC
		public void DeregisterLocalizedText()
		{
			MainWindow.localizationManager.Deregister(this.StrOverview);
			MainWindow.localizationManager.Deregister(this.StrComplianceMode);
			MainWindow.localizationManager.Deregister(this.StrInitSensitivities);
			MainWindow.localizationManager.Deregister(this.StrActualSensitivities);
			MainWindow.localizationManager.Deregister(this.StrCompliance);
			MainWindow.localizationManager.Deregister(this.StrComplianceAdjustFilters);
			MainWindow.localizationManager.Deregister(this.StrResetFiltersToInitSens);
			MainWindow.localizationManager.Deregister(this.StrVA);
			MainWindow.localizationManager.Deregister(this.StrGlass);
			MainWindow.localizationManager.Deregister(this.StrSensitivity);
		}

		// Token: 0x06001590 RID: 5520 RVA: 0x0007FC64 File Offset: 0x0007DE64
		private void resetSimulationStatisticValues()
		{
			TestcardSimViewModel.VASensitivities = new List<int>();
			TestcardSimViewModel.GlassSensitivities = new List<int>();
			TestcardSimViewModel.ImgNamesForSensitivities = new List<string>();
		}

		// Token: 0x06001591 RID: 5521 RVA: 0x0007FC88 File Offset: 0x0007DE88
		private void setSimulationStatisticValues()
		{
			List<string> list = new List<string>();
			this.dbConnection.GetTestcardSimSensitivities(this.productManager.CurrentLoadedProduct.TableName, list);
			TestcardSimViewModel.ImgNamesForSensitivities.Add(Convert.ToString(this.imageForComplianceMode.FilePath));
			for (int i = 0; i < list.Count; i++)
			{
				bool flag = i < 8;
				if (flag)
				{
					TestcardSimViewModel.VASensitivities.Add(i);
					TestcardSimViewModel.VASensitivities.Add(Convert.ToInt32(list[i]));
				}
				else
				{
					bool flag2 = i >= 8;
					if (flag2)
					{
						TestcardSimViewModel.GlassSensitivities.Add(i - 8);
						TestcardSimViewModel.GlassSensitivities.Add(Convert.ToInt32(list[i]));
					}
				}
			}
		}

		// Token: 0x06001592 RID: 5522 RVA: 0x0007FD4E File Offset: 0x0007DF4E
		private void CleanUp(object obj)
		{
			this.DeregisterLocalizedText();
			this.imageTransfer.FrameArrived -= this.imageTestcardSim_FrameArrived;
		}

		// Token: 0x04000E15 RID: 3605
		private ParameterHandling parHandling;

		// Token: 0x04000E16 RID: 3606
		private TestcardSimService testcardSimService;

		// Token: 0x04000E17 RID: 3607
		private DBConnection dbConnection;

		// Token: 0x04000E18 RID: 3608
		private ProductManager productManager;

		// Token: 0x04000E19 RID: 3609
		private AccessRightsManager accessManager;

		// Token: 0x04000E1A RID: 3610
		private XRSImage imageForComplianceMode;

		// Token: 0x04000E29 RID: 3625
		private string selectedBallDiameterComplVA;

		// Token: 0x04000E2A RID: 3626
		private string selectedBallDiameterComplGlass;

		// Token: 0x04000E30 RID: 3632
		private string actualVASensitivity;

		// Token: 0x04000E31 RID: 3633
		private string actualGlassSensitivity;

		// Token: 0x04000E38 RID: 3640
		private ImageDataTransfer imageTransfer;

		// Token: 0x04000E3D RID: 3645
		private bool vaWasSimulated = false;

		// Token: 0x04000E3E RID: 3646
		private bool glassWasSimulated = false;

		// Token: 0x04000E3F RID: 3647
		private StoreImageData imageArchiver;

		// Token: 0x04000E40 RID: 3648
		private bool activatedCMStartVA;

		// Token: 0x04000E41 RID: 3649
		private bool activatedCMStartGlass;

		// Token: 0x04000E42 RID: 3650
		private bool activatedResetToInitSens;
	}
}
