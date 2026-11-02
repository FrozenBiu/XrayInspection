using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using Common;
using DatabaseConnection;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x020000E7 RID: 231
	public class TestcardSimSettingsViewModel : INotifyPropertyChanged
	{
		// Token: 0x170006B5 RID: 1717
		// (get) Token: 0x06001656 RID: 5718 RVA: 0x00087CD1 File Offset: 0x00085ED1
		// (set) Token: 0x06001657 RID: 5719 RVA: 0x00087CD9 File Offset: 0x00085ED9
		public BoundLocalizedText StrSimulationMode { get; set; }

		// Token: 0x170006B6 RID: 1718
		// (get) Token: 0x06001658 RID: 5720 RVA: 0x00087CE2 File Offset: 0x00085EE2
		// (set) Token: 0x06001659 RID: 5721 RVA: 0x00087CEA File Offset: 0x00085EEA
		public BoundLocalizedText StrSimulationMaterial { get; set; }

		// Token: 0x170006B7 RID: 1719
		// (get) Token: 0x0600165A RID: 5722 RVA: 0x00087CF3 File Offset: 0x00085EF3
		// (set) Token: 0x0600165B RID: 5723 RVA: 0x00087CFB File Offset: 0x00085EFB
		public BoundLocalizedText StrBallSize { get; set; }

		// Token: 0x170006B8 RID: 1720
		// (get) Token: 0x0600165C RID: 5724 RVA: 0x00087D04 File Offset: 0x00085F04
		// (set) Token: 0x0600165D RID: 5725 RVA: 0x00087D0C File Offset: 0x00085F0C
		public BoundLocalizedText StrSimulationPattern { get; set; }

		// Token: 0x170006B9 RID: 1721
		// (get) Token: 0x0600165E RID: 5726 RVA: 0x00087D15 File Offset: 0x00085F15
		// (set) Token: 0x0600165F RID: 5727 RVA: 0x00087D1D File Offset: 0x00085F1D
		public BoundLocalizedText StrMaterialDensity { get; set; }

		// Token: 0x170006BA RID: 1722
		// (get) Token: 0x06001660 RID: 5728 RVA: 0x00087D26 File Offset: 0x00085F26
		// (set) Token: 0x06001661 RID: 5729 RVA: 0x00087D2E File Offset: 0x00085F2E
		public BoundLocalizedText StrSettings { get; set; }

		// Token: 0x170006BB RID: 1723
		// (get) Token: 0x06001662 RID: 5730 RVA: 0x00087D37 File Offset: 0x00085F37
		// (set) Token: 0x06001663 RID: 5731 RVA: 0x00087D3F File Offset: 0x00085F3F
		public BoundLocalizedText StrAdjustmentTestcardPat { get; set; }

		// Token: 0x170006BC RID: 1724
		// (get) Token: 0x06001664 RID: 5732 RVA: 0x00087D48 File Offset: 0x00085F48
		// (set) Token: 0x06001665 RID: 5733 RVA: 0x00087D50 File Offset: 0x00085F50
		public BoundLocalizedText StrSaveSimulatedImg { get; set; }

		// Token: 0x170006BD RID: 1725
		// (get) Token: 0x06001666 RID: 5734 RVA: 0x00087D59 File Offset: 0x00085F59
		// (set) Token: 0x06001667 RID: 5735 RVA: 0x00087D61 File Offset: 0x00085F61
		public BoundLocalizedText StrSimuAfterLearn { get; set; }

		// Token: 0x170006BE RID: 1726
		// (get) Token: 0x06001668 RID: 5736 RVA: 0x00087D6A File Offset: 0x00085F6A
		// (set) Token: 0x06001669 RID: 5737 RVA: 0x00087D72 File Offset: 0x00085F72
		public BoundLocalizedText StrMsgDeactivateSimulationAfterTrain { get; set; }

		// Token: 0x1400006F RID: 111
		// (add) Token: 0x0600166A RID: 5738 RVA: 0x00087D7C File Offset: 0x00085F7C
		// (remove) Token: 0x0600166B RID: 5739 RVA: 0x00087DB4 File Offset: 0x00085FB4
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x170006BF RID: 1727
		// (get) Token: 0x0600166C RID: 5740 RVA: 0x00087DE9 File Offset: 0x00085FE9
		// (set) Token: 0x0600166D RID: 5741 RVA: 0x00087DF1 File Offset: 0x00085FF1
		public RelayCommand CleanUpCommand { get; set; }

		// Token: 0x170006C0 RID: 1728
		// (get) Token: 0x0600166E RID: 5742 RVA: 0x00087DFA File Offset: 0x00085FFA
		// (set) Token: 0x0600166F RID: 5743 RVA: 0x00087E02 File Offset: 0x00086002
		public RelayCommand ChangePatternYDirectionCmd { get; set; }

		// Token: 0x170006C1 RID: 1729
		// (get) Token: 0x06001670 RID: 5744 RVA: 0x00087E0B File Offset: 0x0008600B
		// (set) Token: 0x06001671 RID: 5745 RVA: 0x00087E13 File Offset: 0x00086013
		public RelayCommand ChangePatternXDirectionCmd { get; set; }

		// Token: 0x170006C2 RID: 1730
		// (get) Token: 0x06001672 RID: 5746 RVA: 0x00087E1C File Offset: 0x0008601C
		// (set) Token: 0x06001673 RID: 5747 RVA: 0x00087E24 File Offset: 0x00086024
		public RelayCommand ChangeMaterialDensityCmd { get; set; }

		// Token: 0x170006C3 RID: 1731
		// (get) Token: 0x06001674 RID: 5748 RVA: 0x00087E30 File Offset: 0x00086030
		// (set) Token: 0x06001675 RID: 5749 RVA: 0x00087E48 File Offset: 0x00086048
		public bool IsBallSizeVisible
		{
			get
			{
				return this.bIsBallSizeVisible;
			}
			set
			{
				this.bIsBallSizeVisible = value;
				this.OnPropertyChanged("IsBallSizeVisible");
			}
		}

		// Token: 0x170006C4 RID: 1732
		// (get) Token: 0x06001676 RID: 5750 RVA: 0x00087E60 File Offset: 0x00086060
		// (set) Token: 0x06001677 RID: 5751 RVA: 0x00087E78 File Offset: 0x00086078
		public bool IsMaterialVisible
		{
			get
			{
				return this.bIsMaterialVisible;
			}
			set
			{
				this.bIsMaterialVisible = value;
				this.OnPropertyChanged("IsMaterialVisible");
			}
		}

		// Token: 0x170006C5 RID: 1733
		// (get) Token: 0x06001678 RID: 5752 RVA: 0x00087E90 File Offset: 0x00086090
		// (set) Token: 0x06001679 RID: 5753 RVA: 0x00087EA8 File Offset: 0x000860A8
		public bool IsTestPatternVisible
		{
			get
			{
				return this.bIsTestPatternVisible;
			}
			set
			{
				this.bIsTestPatternVisible = value;
				this.OnPropertyChanged("IsTestPatternVisible");
			}
		}

		// Token: 0x170006C6 RID: 1734
		// (get) Token: 0x0600167A RID: 5754 RVA: 0x00087EC0 File Offset: 0x000860C0
		// (set) Token: 0x0600167B RID: 5755 RVA: 0x00087ED8 File Offset: 0x000860D8
		public bool IsMaterialDensityVisible
		{
			get
			{
				return this.bIsMaterialDensityVisible;
			}
			set
			{
				this.bIsMaterialDensityVisible = value;
				this.OnPropertyChanged("IsMaterialDensityVisible");
			}
		}

		// Token: 0x170006C7 RID: 1735
		// (get) Token: 0x0600167C RID: 5756 RVA: 0x00087EF0 File Offset: 0x000860F0
		// (set) Token: 0x0600167D RID: 5757 RVA: 0x00087F08 File Offset: 0x00086108
		public double MaterialDensity
		{
			get
			{
				return this.materialDensity;
			}
			set
			{
				bool flag = this.materialDensity != value;
				if (flag)
				{
					this.materialDensity = value;
					this.OnPropertyChanged("MaterialDensity");
					this.parHandling.WriteValue(Definition.materialdensityPath, this.materialDensity.ToString());
				}
			}
		}

		// Token: 0x170006C8 RID: 1736
		// (get) Token: 0x0600167E RID: 5758 RVA: 0x00087F58 File Offset: 0x00086158
		// (set) Token: 0x0600167F RID: 5759 RVA: 0x00087F70 File Offset: 0x00086170
		public int PatternYDirection
		{
			get
			{
				return this.patternYDirection;
			}
			set
			{
				bool flag = this.patternYDirection != value;
				if (flag)
				{
					this.patternYDirection = value;
					this.OnPropertyChanged("PatternYDirection");
					this.parHandling.WriteValue(Definition.testpatternydirectionPath, this.patternYDirection.ToString());
					this.CalculateTotalCountsforPattern();
				}
			}
		}

		// Token: 0x170006C9 RID: 1737
		// (get) Token: 0x06001680 RID: 5760 RVA: 0x00087FC8 File Offset: 0x000861C8
		// (set) Token: 0x06001681 RID: 5761 RVA: 0x00087FE0 File Offset: 0x000861E0
		public int PatternXDirection
		{
			get
			{
				return this.patternXDirection;
			}
			set
			{
				bool flag = this.patternXDirection != value;
				if (flag)
				{
					this.patternXDirection = value;
					this.OnPropertyChanged("PatternXDirection");
					this.parHandling.WriteValue(Definition.testpatternxdirectionPath, this.patternXDirection.ToString());
					this.CalculateTotalCountsforPattern();
				}
			}
		}

		// Token: 0x170006CA RID: 1738
		// (get) Token: 0x06001682 RID: 5762 RVA: 0x00088038 File Offset: 0x00086238
		// (set) Token: 0x06001683 RID: 5763 RVA: 0x00088050 File Offset: 0x00086250
		public int PatternTotalCounts
		{
			get
			{
				return this.patternTotalCounts;
			}
			set
			{
				bool flag = this.patternTotalCounts != value;
				if (flag)
				{
					this.patternTotalCounts = value;
					this.OnPropertyChanged("PatternTotalCounts");
				}
			}
		}

		// Token: 0x170006CB RID: 1739
		// (get) Token: 0x06001684 RID: 5764 RVA: 0x00088083 File Offset: 0x00086283
		// (set) Token: 0x06001685 RID: 5765 RVA: 0x0008808B File Offset: 0x0008628B
		public List<string> BallDiameterList { get; set; }

		// Token: 0x170006CC RID: 1740
		// (get) Token: 0x06001686 RID: 5766 RVA: 0x00088094 File Offset: 0x00086294
		// (set) Token: 0x06001687 RID: 5767 RVA: 0x0008809C File Offset: 0x0008629C
		public List<SimMaterial> SimMaterialList { get; set; }

		// Token: 0x170006CD RID: 1741
		// (get) Token: 0x06001688 RID: 5768 RVA: 0x000880A8 File Offset: 0x000862A8
		// (set) Token: 0x06001689 RID: 5769 RVA: 0x000880C0 File Offset: 0x000862C0
		public SimMode SelectedSimMode
		{
			get
			{
				return this.selectedSimMode;
			}
			set
			{
				bool flag = this.selectedSimMode != value;
				if (flag)
				{
					this.selectedSimMode = value;
					this.OnPropertyChanged("SelectedSimMode");
					string value2 = "";
					bool flag2 = value == SimMode.AllCards;
					if (flag2)
					{
						this.IsBallSizeVisible = false;
						this.IsMaterialVisible = true;
						this.InitializeMaterialList();
						value2 = "0";
					}
					bool flag3 = value == SimMode.SingleCard;
					if (flag3)
					{
						this.IsBallSizeVisible = true;
						this.SelectedBallDiameter = this.ConvertParamsinBallDiameter();
						this.IsMaterialVisible = true;
						this.InitializeMaterialList();
						this.InitializeBallDiameterList();
						value2 = "2";
					}
					this.parHandling.WriteValue(Definition.simulationmodelPath, value2);
				}
			}
		}

		// Token: 0x170006CE RID: 1742
		// (get) Token: 0x0600168A RID: 5770 RVA: 0x00088170 File Offset: 0x00086370
		// (set) Token: 0x0600168B RID: 5771 RVA: 0x00088188 File Offset: 0x00086388
		public SimMaterial SelectedSimMaterial
		{
			get
			{
				return this.selectedSimMaterial;
			}
			set
			{
				bool flag = this.selectedSimMaterial != value;
				if (flag)
				{
					this.selectedSimMaterial = value;
					this.OnPropertyChanged("SelectedSimMaterial");
					string value2 = "";
					bool flag2 = value == SimMaterial.VAandGlass;
					if (flag2)
					{
						value2 = "0";
						this.IsMaterialDensityVisible = false;
					}
					bool flag3 = value == SimMaterial.VA;
					if (flag3)
					{
						value2 = "1";
						this.IsMaterialDensityVisible = false;
					}
					bool flag4 = value == SimMaterial.Glass;
					if (flag4)
					{
						value2 = "2";
						this.IsMaterialDensityVisible = false;
					}
					this.parHandling.WriteValue(Definition.simulationmaterialPath, value2);
					bool flag5 = this.SelectedSimMode == SimMode.SingleCard;
					if (flag5)
					{
						this.InitializeBallDiameterList();
						this.SelectedBallDiameter = this.ConvertParamsinBallDiameter();
					}
				}
			}
		}

		// Token: 0x170006CF RID: 1743
		// (get) Token: 0x0600168C RID: 5772 RVA: 0x00088248 File Offset: 0x00086448
		// (set) Token: 0x0600168D RID: 5773 RVA: 0x00088260 File Offset: 0x00086460
		public string SelectedBallDiameter
		{
			get
			{
				return this.selectedBallDiameter;
			}
			set
			{
				bool flag = value != null && this.selectedBallDiameter != value;
				if (flag)
				{
					this.selectedBallDiameter = value;
					string value2 = "";
					bool flag2 = this.SelectedSimMaterial == SimMaterial.VA || this.SelectedSimMaterial == SimMaterial.VAandGlass;
					if (flag2)
					{
						bool flag3 = value == "0.3";
						if (flag3)
						{
							value2 = "0";
						}
						else
						{
							bool flag4 = value == "0.4";
							if (flag4)
							{
								value2 = "1";
							}
							else
							{
								bool flag5 = value == "0.5";
								if (flag5)
								{
									value2 = "2";
								}
								else
								{
									bool flag6 = value == "0.6";
									if (flag6)
									{
										value2 = "3";
									}
									else
									{
										bool flag7 = value == "0.8";
										if (flag7)
										{
											value2 = "4";
										}
										else
										{
											bool flag8 = value == "1.0";
											if (flag8)
											{
												value2 = "5";
											}
											else
											{
												bool flag9 = value == "1.2";
												if (flag9)
												{
													value2 = "6";
												}
												else
												{
													bool flag10 = value == "1.6";
													if (flag10)
													{
														value2 = "7";
													}
													else
													{
														bool flag11 = value == "1.8";
														if (flag11)
														{
															value2 = "8";
														}
													}
												}
											}
										}
									}
								}
							}
						}
						this.parHandling.WriteValue(Definition.simulationBallSizeVA, value2);
					}
					else
					{
						bool flag12 = value == "0.6";
						if (flag12)
						{
							value2 = "0";
						}
						else
						{
							bool flag13 = value == "0.8";
							if (flag13)
							{
								value2 = "1";
							}
							else
							{
								bool flag14 = value == "1.0";
								if (flag14)
								{
									value2 = "2";
								}
								else
								{
									bool flag15 = value == "1.2";
									if (flag15)
									{
										value2 = "3";
									}
									else
									{
										bool flag16 = value == "1.5";
										if (flag16)
										{
											value2 = "4";
										}
										else
										{
											bool flag17 = value == "2.0";
											if (flag17)
											{
												value2 = "5";
											}
											else
											{
												bool flag18 = value == "2.5";
												if (flag18)
												{
													value2 = "6";
												}
												else
												{
													bool flag19 = value == "3.0";
													if (flag19)
													{
														value2 = "7";
													}
													else
													{
														bool flag20 = value == "3.5";
														if (flag20)
														{
															value2 = "8";
														}
														else
														{
															bool flag21 = value == "4.0";
															if (flag21)
															{
																value2 = "9";
															}
															else
															{
																bool flag22 = value == "6.0";
																if (flag22)
																{
																	value2 = "10";
																}
																else
																{
																	bool flag23 = value == "7.0";
																	if (flag23)
																	{
																		value2 = "11";
																	}
																	else
																	{
																		bool flag24 = value == "8.0";
																		if (flag24)
																		{
																			value2 = "12";
																		}
																		else
																		{
																			bool flag25 = value == "9.0";
																			if (flag25)
																			{
																				value2 = "13";
																			}
																			else
																			{
																				bool flag26 = value == "10.0";
																				if (flag26)
																				{
																					value2 = "14";
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
						this.parHandling.WriteValue(Definition.simulationBallSizeGlass, value2);
					}
					this.OnPropertyChanged("SelectedBallDiameter");
				}
			}
		}

		// Token: 0x170006D0 RID: 1744
		// (get) Token: 0x0600168E RID: 5774 RVA: 0x00088564 File Offset: 0x00086764
		// (set) Token: 0x0600168F RID: 5775 RVA: 0x0008857C File Offset: 0x0008677C
		public bool SaveSimulatedImages
		{
			get
			{
				return this.saveSimulatedImages;
			}
			set
			{
				bool flag = this.saveSimulatedImages != value;
				if (flag)
				{
					this.saveSimulatedImages = value;
					string value2 = this.saveSimulatedImages ? "1" : "0";
					this.parHandling.WriteValue(Definition.simulationsaveSimImages, value2);
				}
			}
		}

		// Token: 0x170006D1 RID: 1745
		// (get) Token: 0x06001690 RID: 5776 RVA: 0x000885CC File Offset: 0x000867CC
		// (set) Token: 0x06001691 RID: 5777 RVA: 0x000885E4 File Offset: 0x000867E4
		public bool SimulationAfterAutolearn
		{
			get
			{
				return this.simulationAfterAutolearn;
			}
			set
			{
				bool flag = this.simulationAfterAutolearn != value;
				if (flag)
				{
					this.simulationAfterAutolearn = value;
					bool flag2 = !this.simulationAfterAutolearn;
					if (flag2)
					{
						bool flag3 = this.parHandling.ReadPar(Definition.simulationAfterTimeIntervall) == "1";
						if (flag3)
						{
							bool flag4 = Dialog.Show(LocalizationManager.Localize("txtWarning"), this.StrMsgDeactivateSimulationAfterTrain.LocalizedText, DialogStyle.Warning, DialogButtons.OK) == Dialog.DialogResults.Ok;
							if (flag4)
							{
								this.parHandling.WriteValue(Definition.simulationAfterTimeIntervall, "0");
								this.parHandling.WriteValue(Autotrain.nAutolearnPath, "5");
								this.parHandling.WriteValue(Definition.simulationAfterAutolearn, "0");
							}
						}
						else
						{
							this.parHandling.WriteValue(Autotrain.nAutolearnPath, "5");
							this.parHandling.WriteValue(Definition.simulationAfterAutolearn, "0");
						}
					}
					else
					{
						this.parHandling.WriteValue(Autotrain.nAutolearnPath, "10");
						this.parHandling.WriteValue(Definition.simulationAfterAutolearn, "1");
					}
					this.testcardSimService.SetProperties();
					this.OnPropertyChanged("SimulationAfterAutolearn");
				}
			}
		}

		// Token: 0x170006D2 RID: 1746
		// (get) Token: 0x06001692 RID: 5778 RVA: 0x00088720 File Offset: 0x00086920
		// (set) Token: 0x06001693 RID: 5779 RVA: 0x00088738 File Offset: 0x00086938
		public bool DrawAllBallsOfPattern
		{
			get
			{
				return this.drawAllBallsOfPattern;
			}
			set
			{
				bool flag = this.drawAllBallsOfPattern != value;
				if (flag)
				{
					this.drawAllBallsOfPattern = value;
					string value2 = this.drawAllBallsOfPattern ? "0" : "1";
					this.parHandling.WriteValue(Definition.simulationDrawAllBalls, value2);
				}
			}
		}

		// Token: 0x170006D3 RID: 1747
		// (get) Token: 0x06001694 RID: 5780 RVA: 0x00088786 File Offset: 0x00086986
		// (set) Token: 0x06001695 RID: 5781 RVA: 0x0008878E File Offset: 0x0008698E
		public bool DeactivateSimAfterLearnAndICM { get; set; }

		// Token: 0x06001696 RID: 5782 RVA: 0x00088798 File Offset: 0x00086998
		public TestcardSimSettingsViewModel(ParameterHandling parHandling, TestcardSimService testcardSimService, DBConnection dBConnection, ProductManager productManager, AccessRightsManager accessManager, ErrorHandling errorHandling, ImageDataTransfer imageTransfer)
		{
			this.parHandling = parHandling;
			this.testcardSimService = testcardSimService;
			this.dBConnection = dBConnection;
			this.productManager = productManager;
			this.accessManager = accessManager;
			this.errorHandling = errorHandling;
			this.imageTransfer = imageTransfer;
			this.Initialize();
			this.RegisterLocalizedText();
			this.CleanUpCommand = new RelayCommand(new Action<object>(this.CleanUp));
		}

		// Token: 0x06001697 RID: 5783 RVA: 0x00088808 File Offset: 0x00086A08
		private void Initialize()
		{
			this.SetSimMaterial();
			this.SetSimModeAndVisibilty();
			this.InitializeMaterialList();
			this.InitializeBallDiameterList();
			this.SelectedBallDiameter = this.ConvertParamsinBallDiameter();
			this.MaterialDensity = Convert.ToDouble(this.parHandling.ReadPar(Definition.materialdensityPath)) / 100.0;
			this.PatternYDirection = Convert.ToInt32(this.parHandling.ReadPar(Definition.testpatternydirectionPath));
			this.PatternXDirection = Convert.ToInt32(this.parHandling.ReadPar(Definition.testpatternxdirectionPath));
			this.DrawAllBallsOfPattern = !(this.parHandling.ReadPar(Definition.simulationDrawAllBalls) == "1");
			this.SaveSimulatedImages = (this.parHandling.ReadPar(Definition.simulationsaveSimImages) == "1");
			this.SimulationAfterAutolearn = (this.parHandling.ReadPar(Definition.simulationAfterAutolearn) == "1");
			this.CalculateTotalCountsforPattern();
			this.ChangePatternYDirectionCmd = new RelayCommand(new Action<object>(this.ChangePatternYDirection));
			this.ChangePatternXDirectionCmd = new RelayCommand(new Action<object>(this.ChangePatternXDirection));
			this.ChangeMaterialDensityCmd = new RelayCommand(new Action<object>(this.ChangeDensity));
			bool flag = this.parHandling.ReadPar(Acquisition.binningPath) == "2";
			uint num = 0U;
			this.dBConnection.GetAutoKVAdjustment(ref num);
			this.DeactivateSimAfterLearnAndICM = false;
			bool flag2 = flag || num != 1U;
			if (flag2)
			{
				this.DeactivateSimAfterLearnAndICM = true;
			}
		}

		// Token: 0x06001698 RID: 5784 RVA: 0x000889B4 File Offset: 0x00086BB4
		private string ConvertParamsinBallDiameter()
		{
			bool flag = this.SelectedSimMaterial == SimMaterial.VA || this.SelectedSimMaterial == SimMaterial.VAandGlass;
			string text;
			if (flag)
			{
				text = this.parHandling.ReadPar(Definition.simulationBallSizeVA);
				bool flag2 = text == "0";
				if (flag2)
				{
					text = "0.3";
				}
				bool flag3 = text == "1";
				if (flag3)
				{
					text = "0.4";
				}
				else
				{
					bool flag4 = text == "2";
					if (flag4)
					{
						text = "0.5";
					}
					else
					{
						bool flag5 = text == "3";
						if (flag5)
						{
							text = "0.6";
						}
						else
						{
							bool flag6 = text == "4";
							if (flag6)
							{
								text = "0.8";
							}
							else
							{
								bool flag7 = text == "5";
								if (flag7)
								{
									text = "1.0";
								}
								else
								{
									bool flag8 = text == "6";
									if (flag8)
									{
										text = "1.2";
									}
									else
									{
										bool flag9 = text == "7";
										if (flag9)
										{
											text = "1.6";
										}
										else
										{
											bool flag10 = text == "8";
											if (flag10)
											{
												text = "1.8";
											}
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
				text = this.parHandling.ReadPar(Definition.simulationBallSizeGlass);
				bool flag11 = text == "0";
				if (flag11)
				{
					text = "0.6";
				}
				else
				{
					bool flag12 = text == "1";
					if (flag12)
					{
						text = "0.8";
					}
					else
					{
						bool flag13 = text == "2";
						if (flag13)
						{
							text = "1.0";
						}
						else
						{
							bool flag14 = text == "3";
							if (flag14)
							{
								text = "1.2";
							}
							else
							{
								bool flag15 = text == "4";
								if (flag15)
								{
									text = "1.5";
								}
								else
								{
									bool flag16 = text == "5";
									if (flag16)
									{
										text = "2.0";
									}
									else
									{
										bool flag17 = text == "6";
										if (flag17)
										{
											text = "2.5";
										}
										else
										{
											bool flag18 = text == "7";
											if (flag18)
											{
												text = "3.0";
											}
											else
											{
												bool flag19 = text == "8";
												if (flag19)
												{
													text = "3.5";
												}
												else
												{
													bool flag20 = text == "9";
													if (flag20)
													{
														text = "4.0";
													}
													else
													{
														bool flag21 = text == "10";
														if (flag21)
														{
															text = "6.0";
														}
														else
														{
															bool flag22 = text == "11";
															if (flag22)
															{
																text = "7.0";
															}
															else
															{
																bool flag23 = text == "12";
																if (flag23)
																{
																	text = "8.0";
																}
																else
																{
																	bool flag24 = text == "13";
																	if (flag24)
																	{
																		text = "9.0";
																	}
																	else
																	{
																		bool flag25 = text == "14";
																		if (flag25)
																		{
																			text = "10.0";
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
			return text;
		}

		// Token: 0x06001699 RID: 5785 RVA: 0x00088C88 File Offset: 0x00086E88
		private void InitializeMaterialList()
		{
			this.SimMaterialList = new List<SimMaterial>();
			bool flag = this.SelectedSimMode == SimMode.AllCards;
			if (flag)
			{
				this.SimMaterialList.Add(SimMaterial.VAandGlass);
			}
			this.SimMaterialList.Add(SimMaterial.VA);
			this.SimMaterialList.Add(SimMaterial.Glass);
			bool flag2 = this.SelectedSimMode != SimMode.AllCards && this.SelectedSimMaterial == SimMaterial.VAandGlass;
			if (flag2)
			{
				this.SelectedSimMaterial = SimMaterial.VA;
			}
			this.OnPropertyChanged("SimMaterialList");
		}

		// Token: 0x0600169A RID: 5786 RVA: 0x00088D04 File Offset: 0x00086F04
		private void InitializeBallDiameterList()
		{
			this.BallDiameterList = new List<string>();
			bool flag = this.SelectedSimMode == SimMode.SingleCard;
			if (flag)
			{
				bool flag2 = this.SelectedSimMaterial == SimMaterial.VA || this.SelectedSimMaterial == SimMaterial.VAandGlass;
				if (flag2)
				{
					this.BallDiameterList.Add("0.3");
					this.BallDiameterList.Add("0.4");
					this.BallDiameterList.Add("0.5");
					this.BallDiameterList.Add("0.6");
					this.BallDiameterList.Add("0.8");
					this.BallDiameterList.Add("1.0");
					this.BallDiameterList.Add("1.2");
					this.BallDiameterList.Add("1.6");
					this.BallDiameterList.Add("1.8");
				}
				else
				{
					this.BallDiameterList.Add("0.6");
					this.BallDiameterList.Add("0.8");
					this.BallDiameterList.Add("1.0");
					this.BallDiameterList.Add("1.2");
					this.BallDiameterList.Add("1.5");
					this.BallDiameterList.Add("2.0");
					this.BallDiameterList.Add("2.5");
					this.BallDiameterList.Add("3.0");
					this.BallDiameterList.Add("3.5");
					this.BallDiameterList.Add("4.0");
					this.BallDiameterList.Add("6.0");
					this.BallDiameterList.Add("7.0");
					this.BallDiameterList.Add("8.0");
					this.BallDiameterList.Add("9.0");
					this.BallDiameterList.Add("10.0");
				}
				this.OnPropertyChanged("BallDiameterList");
			}
		}

		// Token: 0x0600169B RID: 5787 RVA: 0x00088EF9 File Offset: 0x000870F9
		private void CalculateTotalCountsforPattern()
		{
			this.PatternTotalCounts = this.PatternXDirection * this.PatternYDirection;
		}

		// Token: 0x0600169C RID: 5788 RVA: 0x00088F10 File Offset: 0x00087110
		public void ChangeDensity(object obj)
		{
			string oldValue = "";
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtMaterialDensity"), oldValue, NumKeyboardType.TypeDouble);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_ChangeDensity;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x0600169D RID: 5789 RVA: 0x00088F60 File Offset: 0x00087160
		public void ChangePatternYDirection(object obj)
		{
			string oldValue = "";
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtTestcardPattern"), oldValue, NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_ChangePatternYDirection;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x0600169E RID: 5790 RVA: 0x00088FB0 File Offset: 0x000871B0
		public void ChangePatternXDirection(object obj)
		{
			string oldValue = "";
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtTestcardPattern"), oldValue, NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_ChangePatternXDirection;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x0600169F RID: 5791 RVA: 0x00089000 File Offset: 0x00087200
		private void numKeyboard_ChangeDensity(object sender, KeyboardInputEventArgs e)
		{
			try
			{
				double num = double.Parse(e.Text, new CultureInfo("de-DE"));
				bool flag = num > 0.0 && num <= 10.0;
				if (flag)
				{
					this.MaterialDensity = num;
				}
				this.numKeyboard.TextInputKeyboard -= this.numKeyboard_ChangeDensity;
			}
			catch
			{
			}
		}

		// Token: 0x060016A0 RID: 5792 RVA: 0x00089080 File Offset: 0x00087280
		private void numKeyboard_ChangePatternYDirection(object sender, KeyboardInputEventArgs e)
		{
			try
			{
				int num = int.Parse(e.Text, new CultureInfo("de-DE"));
				bool flag = num > 0 && num <= 100;
				if (flag)
				{
					this.PatternYDirection = num;
				}
				this.numKeyboard.TextInputKeyboard -= this.numKeyboard_ChangePatternYDirection;
			}
			catch
			{
			}
		}

		// Token: 0x060016A1 RID: 5793 RVA: 0x000890F0 File Offset: 0x000872F0
		private void numKeyboard_ChangePatternXDirection(object sender, KeyboardInputEventArgs e)
		{
			try
			{
				int num = int.Parse(e.Text, new CultureInfo("de-DE"));
				bool flag = num > 0 && num <= 100;
				if (flag)
				{
					this.PatternXDirection = num;
				}
				this.numKeyboard.TextInputKeyboard -= this.numKeyboard_ChangePatternXDirection;
			}
			catch
			{
			}
		}

		// Token: 0x060016A2 RID: 5794 RVA: 0x00089160 File Offset: 0x00087360
		private void SetSimModeAndVisibilty()
		{
			string a = this.parHandling.ReadPar(Definition.simulationmodelPath);
			this.IsTestPatternVisible = (this.accessManager.CurrentUser.Rights == BuiltInPriviledges.Service);
			bool flag = a == "0";
			if (flag)
			{
				this.SelectedSimMode = SimMode.AllCards;
				this.IsBallSizeVisible = false;
				this.IsMaterialVisible = true;
			}
			else
			{
				bool flag2 = a == "2";
				if (flag2)
				{
					this.SelectedSimMode = SimMode.SingleCard;
					this.IsBallSizeVisible = true;
					this.IsMaterialVisible = true;
				}
				else
				{
					this.SelectedSimMode = SimMode.AllCards;
					this.IsBallSizeVisible = false;
					this.IsMaterialVisible = true;
				}
			}
		}

		// Token: 0x060016A3 RID: 5795 RVA: 0x00089210 File Offset: 0x00087410
		private void SetSimMaterial()
		{
			string a = this.parHandling.ReadPar(Definition.simulationmaterialPath);
			bool flag = a == "0";
			if (flag)
			{
				this.SelectedSimMaterial = SimMaterial.VAandGlass;
				this.IsMaterialDensityVisible = false;
			}
			else
			{
				bool flag2 = a == "1";
				if (flag2)
				{
					this.SelectedSimMaterial = SimMaterial.VA;
					this.IsMaterialDensityVisible = false;
				}
				else
				{
					bool flag3 = a == "2";
					if (flag3)
					{
						this.SelectedSimMaterial = SimMaterial.Glass;
						this.IsMaterialDensityVisible = false;
					}
				}
			}
		}

		// Token: 0x060016A4 RID: 5796 RVA: 0x00089298 File Offset: 0x00087498
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x060016A5 RID: 5797 RVA: 0x000892C8 File Offset: 0x000874C8
		private void RegisterLocalizedText()
		{
			this.StrSimulationMode = new BoundLocalizedText("txtMode");
			this.StrSimulationMaterial = new BoundLocalizedText("txtMaterial");
			this.StrBallSize = new BoundLocalizedText("txtBallDiameter");
			this.StrSimulationPattern = new BoundLocalizedText("txtTestcardPattern");
			this.StrMaterialDensity = new BoundLocalizedText("txtMaterialDensity");
			this.StrSettings = new BoundLocalizedText("txtSettings");
			this.StrAdjustmentTestcardPat = new BoundLocalizedText("txtAdjustmentTestcardPat");
			this.StrSaveSimulatedImg = new BoundLocalizedText("txtSaveSimulatedImg");
			this.StrSimuAfterLearn = new BoundLocalizedText("txtSimAfterLearn");
			this.StrMsgDeactivateSimulationAfterTrain = new BoundLocalizedText("txtDeactivateTestcardSim");
		}

		// Token: 0x060016A6 RID: 5798 RVA: 0x00089380 File Offset: 0x00087580
		public void DeregisterLocalizedText()
		{
			MainWindow.localizationManager.Deregister(this.StrSimulationMode);
			MainWindow.localizationManager.Deregister(this.StrSimulationMaterial);
			MainWindow.localizationManager.Deregister(this.StrBallSize);
			MainWindow.localizationManager.Deregister(this.StrSimulationPattern);
			MainWindow.localizationManager.Deregister(this.StrMaterialDensity);
			MainWindow.localizationManager.Deregister(this.StrSettings);
			MainWindow.localizationManager.Deregister(this.StrAdjustmentTestcardPat);
			MainWindow.localizationManager.Deregister(this.StrSaveSimulatedImg);
			MainWindow.localizationManager.Deregister(this.StrSimuAfterLearn);
			MainWindow.localizationManager.Deregister(this.StrMsgDeactivateSimulationAfterTrain);
		}

		// Token: 0x060016A7 RID: 5799 RVA: 0x00089438 File Offset: 0x00087638
		private void CleanUp(object obj)
		{
			this.DeregisterLocalizedText();
			this.SimMaterialList.Clear();
			this.SimMaterialList = null;
			this.BallDiameterList.Clear();
			this.BallDiameterList = null;
		}

		// Token: 0x04000ED7 RID: 3799
		private ParameterHandling parHandling;

		// Token: 0x04000ED8 RID: 3800
		private TestcardSimService testcardSimService;

		// Token: 0x04000ED9 RID: 3801
		private DBConnection dBConnection;

		// Token: 0x04000EDA RID: 3802
		private ProductManager productManager;

		// Token: 0x04000EDB RID: 3803
		private NumKeyboard numKeyboard;

		// Token: 0x04000EEB RID: 3819
		private SimMode selectedSimMode;

		// Token: 0x04000EEC RID: 3820
		private SimMaterial selectedSimMaterial;

		// Token: 0x04000EED RID: 3821
		private bool bIsBallSizeVisible;

		// Token: 0x04000EEE RID: 3822
		private bool bIsMaterialVisible;

		// Token: 0x04000EEF RID: 3823
		private bool bIsTestPatternVisible;

		// Token: 0x04000EF0 RID: 3824
		private bool bIsMaterialDensityVisible;

		// Token: 0x04000EF1 RID: 3825
		private double materialDensity;

		// Token: 0x04000EF2 RID: 3826
		private int patternYDirection;

		// Token: 0x04000EF3 RID: 3827
		private int patternXDirection;

		// Token: 0x04000EF4 RID: 3828
		private int patternTotalCounts;

		// Token: 0x04000EF5 RID: 3829
		private string selectedBallDiameter;

		// Token: 0x04000EF6 RID: 3830
		private bool saveSimulatedImages;

		// Token: 0x04000EF7 RID: 3831
		private bool simulationAfterAutolearn;

		// Token: 0x04000EF8 RID: 3832
		private bool drawAllBallsOfPattern;

		// Token: 0x04000EF9 RID: 3833
		private AccessRightsManager accessManager;

		// Token: 0x04000EFA RID: 3834
		private ErrorHandling errorHandling;

		// Token: 0x04000EFB RID: 3835
		private ImageDataTransfer imageTransfer;
	}
}
