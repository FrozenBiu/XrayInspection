using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using Common;
using DatabaseConnection;

namespace GUI_XRS
{
	// Token: 0x0200007C RID: 124
	public class IVPTestsamplesViewModel : INotifyPropertyChanged
	{
		// Token: 0x14000041 RID: 65
		// (add) Token: 0x06000BDC RID: 3036 RVA: 0x00046A10 File Offset: 0x00044C10
		// (remove) Token: 0x06000BDD RID: 3037 RVA: 0x00046A48 File Offset: 0x00044C48
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x1700038E RID: 910
		// (get) Token: 0x06000BDE RID: 3038 RVA: 0x00046A7D File Offset: 0x00044C7D
		// (set) Token: 0x06000BDF RID: 3039 RVA: 0x00046A85 File Offset: 0x00044C85
		public RelayCommand CleanUpCommand { get; set; }

		// Token: 0x1700038F RID: 911
		// (get) Token: 0x06000BE0 RID: 3040 RVA: 0x00046A8E File Offset: 0x00044C8E
		// (set) Token: 0x06000BE1 RID: 3041 RVA: 0x00046A96 File Offset: 0x00044C96
		public BoundLocalizedText StrMaterial { get; set; }

		// Token: 0x17000390 RID: 912
		// (get) Token: 0x06000BE2 RID: 3042 RVA: 0x00046A9F File Offset: 0x00044C9F
		// (set) Token: 0x06000BE3 RID: 3043 RVA: 0x00046AA7 File Offset: 0x00044CA7
		public BoundLocalizedText StrBallSize { get; set; }

		// Token: 0x17000391 RID: 913
		// (get) Token: 0x06000BE4 RID: 3044 RVA: 0x00046AB0 File Offset: 0x00044CB0
		// (set) Token: 0x06000BE5 RID: 3045 RVA: 0x00046AB8 File Offset: 0x00044CB8
		public BoundLocalizedText StrTestcardSize { get; set; }

		// Token: 0x17000392 RID: 914
		// (get) Token: 0x06000BE6 RID: 3046 RVA: 0x00046AC1 File Offset: 0x00044CC1
		// (set) Token: 0x06000BE7 RID: 3047 RVA: 0x00046AC9 File Offset: 0x00044CC9
		public BoundLocalizedText StrNumberOfBalls { get; set; }

		// Token: 0x17000393 RID: 915
		// (get) Token: 0x06000BE8 RID: 3048 RVA: 0x00046AD2 File Offset: 0x00044CD2
		// (set) Token: 0x06000BE9 RID: 3049 RVA: 0x00046ADA File Offset: 0x00044CDA
		public BoundLocalizedText StrSettings { get; set; }

		// Token: 0x17000394 RID: 916
		// (get) Token: 0x06000BEA RID: 3050 RVA: 0x00046AE3 File Offset: 0x00044CE3
		// (set) Token: 0x06000BEB RID: 3051 RVA: 0x00046AEB File Offset: 0x00044CEB
		public BoundLocalizedText StrINNumber { get; set; }

		// Token: 0x17000395 RID: 917
		// (get) Token: 0x06000BEC RID: 3052 RVA: 0x00046AF4 File Offset: 0x00044CF4
		// (set) Token: 0x06000BED RID: 3053 RVA: 0x00046AFC File Offset: 0x00044CFC
		public BoundLocalizedText StrDensity { get; set; }

		// Token: 0x17000396 RID: 918
		// (get) Token: 0x06000BEE RID: 3054 RVA: 0x00046B05 File Offset: 0x00044D05
		// (set) Token: 0x06000BEF RID: 3055 RVA: 0x00046B0D File Offset: 0x00044D0D
		public BoundLocalizedText StrTestContaminations { get; set; }

		// Token: 0x17000397 RID: 919
		// (get) Token: 0x06000BF0 RID: 3056 RVA: 0x00046B16 File Offset: 0x00044D16
		// (set) Token: 0x06000BF1 RID: 3057 RVA: 0x00046B1E File Offset: 0x00044D1E
		public int chosenTextbox { get; set; }

		// Token: 0x17000398 RID: 920
		// (get) Token: 0x06000BF2 RID: 3058 RVA: 0x00046B27 File Offset: 0x00044D27
		// (set) Token: 0x06000BF3 RID: 3059 RVA: 0x00046B2F File Offset: 0x00044D2F
		public List<Material> MaterialList { get; set; }

		// Token: 0x17000399 RID: 921
		// (get) Token: 0x06000BF4 RID: 3060 RVA: 0x00046B38 File Offset: 0x00044D38
		// (set) Token: 0x06000BF5 RID: 3061 RVA: 0x00046B50 File Offset: 0x00044D50
		public Material SelectedMaterial1
		{
			get
			{
				return this.selectedMaterial1;
			}
			set
			{
				bool flag = this.selectedMaterial1 != value;
				if (flag)
				{
					this.selectedMaterial1 = value;
					this.dbConnection.SetIVPTestcardEntry(1, DBIntegratedValidationProcess.TestcardMaterial, Convert.ToString(this.selectedMaterial1));
					bool flag2 = this.selectedMaterial1 != Material.None;
					if (flag2)
					{
						this.TestCardSize1 = this.InitTestCardSizeList(this.selectedMaterial1);
						this.SelectedTestCardSize1 = this.TestCardSize1.First<TestCardSize>();
						this.TestBallSize1 = this.InitializeTestBalls(this.selectedTestCardSize1, this.selectedMaterial1);
						this.SelectedTestBallSize1 = this.TestBallSize1.First<string>();
						this.Density1 = this.getDensity(this.selectedMaterial1);
					}
					else
					{
						this.TestCardSize1 = new List<TestCardSize>();
						this.TestBallSize1 = new List<string>();
						this.TextBoxVisible1 = false;
						this.NumberTestBalls1 = 0;
						this.INNumber1 = "n.a.";
						this.Density1 = this.getDensity(this.selectedMaterial1);
					}
					this.OnPropertyChanged("SelectedMaterial1");
				}
			}
		}

		// Token: 0x1700039A RID: 922
		// (get) Token: 0x06000BF6 RID: 3062 RVA: 0x00046C68 File Offset: 0x00044E68
		// (set) Token: 0x06000BF7 RID: 3063 RVA: 0x00046C80 File Offset: 0x00044E80
		public Material SelectedMaterial2
		{
			get
			{
				return this.selectedMaterial2;
			}
			set
			{
				bool flag = this.selectedMaterial2 != value;
				if (flag)
				{
					this.selectedMaterial2 = value;
					this.dbConnection.SetIVPTestcardEntry(2, DBIntegratedValidationProcess.TestcardMaterial, Convert.ToString(this.selectedMaterial2));
					bool flag2 = this.selectedMaterial2 != Material.None;
					if (flag2)
					{
						this.TestCardSize2 = this.InitTestCardSizeList(this.selectedMaterial2);
						this.SelectedTestCardSize2 = this.TestCardSize2.First<TestCardSize>();
						this.TestBallSize2 = this.InitializeTestBalls(this.selectedTestCardSize2, this.selectedMaterial2);
						this.SelectedTestBallSize2 = this.TestBallSize2.First<string>();
						this.Density2 = this.getDensity(this.selectedMaterial2);
					}
					else
					{
						this.TestCardSize2 = new List<TestCardSize>();
						this.TestBallSize2 = new List<string>();
						this.TextBoxVisible2 = false;
						this.NumberTestBalls2 = 0;
						this.INNumber2 = "n.a.";
						this.Density2 = this.getDensity(this.selectedMaterial2);
					}
					this.OnPropertyChanged("SelectedMaterial2");
				}
			}
		}

		// Token: 0x1700039B RID: 923
		// (get) Token: 0x06000BF8 RID: 3064 RVA: 0x00046D98 File Offset: 0x00044F98
		// (set) Token: 0x06000BF9 RID: 3065 RVA: 0x00046DB0 File Offset: 0x00044FB0
		public Material SelectedMaterial3
		{
			get
			{
				return this.selectedMaterial3;
			}
			set
			{
				bool flag = this.selectedMaterial3 != value;
				if (flag)
				{
					this.selectedMaterial3 = value;
					this.dbConnection.SetIVPTestcardEntry(3, DBIntegratedValidationProcess.TestcardMaterial, Convert.ToString(this.selectedMaterial3));
					bool flag2 = this.selectedMaterial3 != Material.None;
					if (flag2)
					{
						this.TestCardSize3 = this.InitTestCardSizeList(this.selectedMaterial3);
						this.SelectedTestCardSize3 = this.TestCardSize3.First<TestCardSize>();
						this.TestBallSize3 = this.InitializeTestBalls(this.selectedTestCardSize3, this.selectedMaterial3);
						this.SelectedTestBallSize3 = this.TestBallSize3.First<string>();
						this.Density3 = this.getDensity(this.selectedMaterial3);
					}
					else
					{
						this.TestCardSize3 = new List<TestCardSize>();
						this.TestBallSize3 = new List<string>();
						this.TextBoxVisible3 = false;
						this.NumberTestBalls3 = 0;
						this.INNumber3 = "n.a.";
						this.Density3 = this.getDensity(this.selectedMaterial3);
					}
					this.OnPropertyChanged("SelectedMaterial3");
				}
			}
		}

		// Token: 0x1700039C RID: 924
		// (get) Token: 0x06000BFA RID: 3066 RVA: 0x00046EC8 File Offset: 0x000450C8
		// (set) Token: 0x06000BFB RID: 3067 RVA: 0x00046EE0 File Offset: 0x000450E0
		public Material SelectedMaterial4
		{
			get
			{
				return this.selectedMaterial4;
			}
			set
			{
				bool flag = this.selectedMaterial4 != value;
				if (flag)
				{
					this.selectedMaterial4 = value;
					this.dbConnection.SetIVPTestcardEntry(4, DBIntegratedValidationProcess.TestcardMaterial, Convert.ToString(this.selectedMaterial4));
					bool flag2 = this.selectedMaterial4 != Material.None;
					if (flag2)
					{
						this.TestCardSize4 = this.InitTestCardSizeList(this.selectedMaterial4);
						this.SelectedTestCardSize4 = this.TestCardSize4.First<TestCardSize>();
						this.TestBallSize4 = this.InitializeTestBalls(this.selectedTestCardSize4, this.selectedMaterial4);
						this.SelectedTestBallSize4 = this.TestBallSize4.First<string>();
						this.Density4 = this.getDensity(this.selectedMaterial4);
					}
					else
					{
						this.TestCardSize4 = new List<TestCardSize>();
						this.TestBallSize4 = new List<string>();
						this.TextBoxVisible4 = false;
						this.NumberTestBalls4 = 0;
						this.INNumber4 = "n.a.";
						this.Density4 = this.getDensity(this.selectedMaterial4);
					}
					this.OnPropertyChanged("SelectedMaterial4");
				}
			}
		}

		// Token: 0x1700039D RID: 925
		// (get) Token: 0x06000BFC RID: 3068 RVA: 0x00046FF8 File Offset: 0x000451F8
		// (set) Token: 0x06000BFD RID: 3069 RVA: 0x00047010 File Offset: 0x00045210
		public Material SelectedMaterial5
		{
			get
			{
				return this.selectedMaterial5;
			}
			set
			{
				bool flag = this.selectedMaterial5 != value;
				if (flag)
				{
					this.selectedMaterial5 = value;
					this.dbConnection.SetIVPTestcardEntry(5, DBIntegratedValidationProcess.TestcardMaterial, Convert.ToString(this.selectedMaterial5));
					bool flag2 = this.selectedMaterial5 != Material.None;
					if (flag2)
					{
						this.TestCardSize5 = this.InitTestCardSizeList(this.selectedMaterial5);
						this.SelectedTestCardSize5 = this.TestCardSize5.First<TestCardSize>();
						this.TestBallSize5 = this.InitializeTestBalls(this.selectedTestCardSize5, this.selectedMaterial5);
						this.SelectedTestBallSize5 = this.TestBallSize5.First<string>();
						this.Density5 = this.getDensity(this.selectedMaterial5);
					}
					else
					{
						this.TestCardSize5 = new List<TestCardSize>();
						this.TestBallSize5 = new List<string>();
						this.TextBoxVisible5 = false;
						this.NumberTestBalls5 = 0;
						this.INNumber5 = "n.a.";
						this.Density5 = this.getDensity(this.selectedMaterial5);
					}
					this.OnPropertyChanged("SelectedMaterial5");
				}
			}
		}

		// Token: 0x1700039E RID: 926
		// (get) Token: 0x06000BFE RID: 3070 RVA: 0x00047128 File Offset: 0x00045328
		// (set) Token: 0x06000BFF RID: 3071 RVA: 0x00047140 File Offset: 0x00045340
		public List<TestCardSize> TestCardSize1
		{
			get
			{
				return this.testCardSize1;
			}
			set
			{
				bool flag = value != this.testCardSize1;
				if (flag)
				{
					this.testCardSize1 = value;
					this.OnPropertyChanged("TestCardSize1");
				}
			}
		}

		// Token: 0x1700039F RID: 927
		// (get) Token: 0x06000C00 RID: 3072 RVA: 0x00047174 File Offset: 0x00045374
		// (set) Token: 0x06000C01 RID: 3073 RVA: 0x0004718C File Offset: 0x0004538C
		public List<TestCardSize> TestCardSize2
		{
			get
			{
				return this.testCardSize2;
			}
			set
			{
				bool flag = value != this.testCardSize2;
				if (flag)
				{
					this.testCardSize2 = value;
					this.OnPropertyChanged("TestCardSize2");
				}
			}
		}

		// Token: 0x170003A0 RID: 928
		// (get) Token: 0x06000C02 RID: 3074 RVA: 0x000471C0 File Offset: 0x000453C0
		// (set) Token: 0x06000C03 RID: 3075 RVA: 0x000471D8 File Offset: 0x000453D8
		public List<TestCardSize> TestCardSize3
		{
			get
			{
				return this.testCardSize3;
			}
			set
			{
				bool flag = value != this.testCardSize3;
				if (flag)
				{
					this.testCardSize3 = value;
					this.OnPropertyChanged("TestCardSize3");
				}
			}
		}

		// Token: 0x170003A1 RID: 929
		// (get) Token: 0x06000C04 RID: 3076 RVA: 0x0004720C File Offset: 0x0004540C
		// (set) Token: 0x06000C05 RID: 3077 RVA: 0x00047224 File Offset: 0x00045424
		public List<TestCardSize> TestCardSize4
		{
			get
			{
				return this.testCardSize4;
			}
			set
			{
				bool flag = value != this.testCardSize4;
				if (flag)
				{
					this.testCardSize4 = value;
					this.OnPropertyChanged("TestCardSize4");
				}
			}
		}

		// Token: 0x170003A2 RID: 930
		// (get) Token: 0x06000C06 RID: 3078 RVA: 0x00047258 File Offset: 0x00045458
		// (set) Token: 0x06000C07 RID: 3079 RVA: 0x00047270 File Offset: 0x00045470
		public List<TestCardSize> TestCardSize5
		{
			get
			{
				return this.testCardSize5;
			}
			set
			{
				bool flag = value != this.testCardSize5;
				if (flag)
				{
					this.testCardSize5 = value;
					this.OnPropertyChanged("TestCardSize5");
				}
			}
		}

		// Token: 0x170003A3 RID: 931
		// (get) Token: 0x06000C08 RID: 3080 RVA: 0x000472A4 File Offset: 0x000454A4
		// (set) Token: 0x06000C09 RID: 3081 RVA: 0x000472BC File Offset: 0x000454BC
		public TestCardSize SelectedTestCardSize1
		{
			get
			{
				return this.selectedTestCardSize1;
			}
			set
			{
				bool flag = value != this.selectedTestCardSize1;
				if (flag)
				{
					this.selectedTestCardSize1 = value;
					this.dbConnection.SetIVPTestcardEntry(1, DBIntegratedValidationProcess.TestcardType, Convert.ToString(this.selectedTestCardSize1));
					this.TestBallSize1 = this.InitializeTestBalls(this.selectedTestCardSize1, this.selectedMaterial1);
					this.SelectedTestBallSize1 = this.TestBallSize1.First<string>();
					bool flag2 = this.selectedTestCardSize1 != TestCardSize.Other && this.selectedMaterial1 != Material.Other;
					if (flag2)
					{
						this.TextBoxVisible1 = false;
						this.NumberTestBalls1 = this.calculateNumberTestBalls(this.selectedMaterial1, this.selectedTestCardSize1, this.selectedTestBallSize1, this.testBallSize1);
					}
					else
					{
						bool flag3 = this.selectedMaterial1 == Material.None;
						if (flag3)
						{
							this.TextBoxVisible1 = false;
							this.NumberTestBalls1 = 0;
						}
						else
						{
							bool flag4 = this.selectedMaterial1 == Material.Other && this.selectedTestCardSize1 != TestCardSize.Other;
							if (flag4)
							{
								bool flag5 = this.selectedTestCardSize1 == TestCardSize.Testcard100;
								if (flag5)
								{
									this.NumberTestBalls1 = 100;
								}
								else
								{
									bool flag6 = this.selectedTestCardSize1 == TestCardSize.SingleBall;
									if (flag6)
									{
										this.NumberTestBalls1 = 1;
									}
								}
								this.TextBoxVisible1 = false;
							}
							else
							{
								this.TextBoxVisible1 = true;
								this.NumberTestBalls1 = 1;
							}
						}
					}
					this.OnPropertyChanged("SelectedTestCardSize1");
				}
			}
		}

		// Token: 0x170003A4 RID: 932
		// (get) Token: 0x06000C0A RID: 3082 RVA: 0x00047424 File Offset: 0x00045624
		// (set) Token: 0x06000C0B RID: 3083 RVA: 0x0004743C File Offset: 0x0004563C
		public TestCardSize SelectedTestCardSize2
		{
			get
			{
				return this.selectedTestCardSize2;
			}
			set
			{
				bool flag = value != this.selectedTestCardSize2;
				if (flag)
				{
					this.selectedTestCardSize2 = value;
					this.dbConnection.SetIVPTestcardEntry(2, DBIntegratedValidationProcess.TestcardType, Convert.ToString(this.selectedTestCardSize2));
					this.TestBallSize2 = this.InitializeTestBalls(this.selectedTestCardSize2, this.selectedMaterial2);
					this.SelectedTestBallSize2 = this.TestBallSize2.First<string>();
					bool flag2 = this.selectedTestCardSize2 != TestCardSize.Other && this.selectedMaterial2 != Material.Other;
					if (flag2)
					{
						this.TextBoxVisible2 = false;
						this.NumberTestBalls2 = this.calculateNumberTestBalls(this.selectedMaterial2, this.selectedTestCardSize2, this.selectedTestBallSize2, this.testBallSize2);
					}
					else
					{
						bool flag3 = this.selectedMaterial2 == Material.None;
						if (flag3)
						{
							this.TextBoxVisible2 = false;
							this.NumberTestBalls2 = 0;
						}
						else
						{
							bool flag4 = this.selectedMaterial2 == Material.Other && this.selectedTestCardSize2 != TestCardSize.Other;
							if (flag4)
							{
								bool flag5 = this.selectedTestCardSize2 == TestCardSize.Testcard100;
								if (flag5)
								{
									this.NumberTestBalls2 = 100;
								}
								else
								{
									bool flag6 = this.selectedTestCardSize2 == TestCardSize.SingleBall;
									if (flag6)
									{
										this.NumberTestBalls2 = 1;
									}
								}
								this.TextBoxVisible2 = false;
							}
							else
							{
								this.TextBoxVisible2 = true;
								this.NumberTestBalls2 = 1;
							}
						}
					}
					this.OnPropertyChanged("SelectedTestCardSize2");
				}
			}
		}

		// Token: 0x170003A5 RID: 933
		// (get) Token: 0x06000C0C RID: 3084 RVA: 0x000475A4 File Offset: 0x000457A4
		// (set) Token: 0x06000C0D RID: 3085 RVA: 0x000475BC File Offset: 0x000457BC
		public TestCardSize SelectedTestCardSize3
		{
			get
			{
				return this.selectedTestCardSize3;
			}
			set
			{
				bool flag = value != this.selectedTestCardSize3;
				if (flag)
				{
					this.selectedTestCardSize3 = value;
					this.dbConnection.SetIVPTestcardEntry(3, DBIntegratedValidationProcess.TestcardType, Convert.ToString(this.selectedTestCardSize3));
					this.TestBallSize3 = this.InitializeTestBalls(this.selectedTestCardSize3, this.selectedMaterial3);
					this.SelectedTestBallSize3 = this.TestBallSize3.First<string>();
					bool flag2 = this.selectedTestCardSize3 != TestCardSize.Other && this.selectedMaterial3 != Material.Other;
					if (flag2)
					{
						this.TextBoxVisible3 = false;
						this.NumberTestBalls3 = this.calculateNumberTestBalls(this.selectedMaterial3, this.selectedTestCardSize3, this.selectedTestBallSize3, this.testBallSize3);
					}
					else
					{
						bool flag3 = this.selectedMaterial3 == Material.None;
						if (flag3)
						{
							this.TextBoxVisible3 = false;
							this.NumberTestBalls3 = 0;
						}
						else
						{
							bool flag4 = this.selectedMaterial3 == Material.Other && this.selectedTestCardSize3 != TestCardSize.Other;
							if (flag4)
							{
								bool flag5 = this.selectedTestCardSize3 == TestCardSize.Testcard100;
								if (flag5)
								{
									this.NumberTestBalls3 = 100;
								}
								else
								{
									bool flag6 = this.selectedTestCardSize3 == TestCardSize.SingleBall;
									if (flag6)
									{
										this.NumberTestBalls3 = 1;
									}
								}
								this.TextBoxVisible3 = false;
							}
							else
							{
								this.TextBoxVisible3 = true;
								this.NumberTestBalls3 = 1;
							}
						}
					}
					this.OnPropertyChanged("SelectedTestCardSize3");
				}
			}
		}

		// Token: 0x170003A6 RID: 934
		// (get) Token: 0x06000C0E RID: 3086 RVA: 0x00047724 File Offset: 0x00045924
		// (set) Token: 0x06000C0F RID: 3087 RVA: 0x0004773C File Offset: 0x0004593C
		public TestCardSize SelectedTestCardSize4
		{
			get
			{
				return this.selectedTestCardSize4;
			}
			set
			{
				bool flag = value != this.selectedTestCardSize4;
				if (flag)
				{
					this.selectedTestCardSize4 = value;
					this.dbConnection.SetIVPTestcardEntry(4, DBIntegratedValidationProcess.TestcardType, Convert.ToString(this.selectedTestCardSize4));
					this.TestBallSize4 = this.InitializeTestBalls(this.selectedTestCardSize4, this.selectedMaterial4);
					this.SelectedTestBallSize4 = this.TestBallSize4.First<string>();
					bool flag2 = this.selectedTestCardSize4 != TestCardSize.Other && this.selectedMaterial4 != Material.Other;
					if (flag2)
					{
						this.TextBoxVisible4 = false;
						this.NumberTestBalls4 = this.calculateNumberTestBalls(this.selectedMaterial4, this.selectedTestCardSize4, this.selectedTestBallSize4, this.testBallSize4);
					}
					else
					{
						bool flag3 = this.selectedMaterial4 == Material.None;
						if (flag3)
						{
							this.TextBoxVisible4 = false;
							this.NumberTestBalls4 = 0;
						}
						else
						{
							bool flag4 = this.selectedMaterial4 == Material.Other && this.selectedTestCardSize4 != TestCardSize.Other;
							if (flag4)
							{
								bool flag5 = this.selectedTestCardSize4 == TestCardSize.Testcard100;
								if (flag5)
								{
									this.NumberTestBalls4 = 100;
								}
								else
								{
									bool flag6 = this.selectedTestCardSize4 == TestCardSize.SingleBall;
									if (flag6)
									{
										this.NumberTestBalls4 = 1;
									}
								}
								this.TextBoxVisible4 = false;
							}
							else
							{
								this.TextBoxVisible4 = true;
								this.NumberTestBalls4 = 1;
							}
						}
					}
					this.OnPropertyChanged("SelectedTestCardSize4");
				}
			}
		}

		// Token: 0x170003A7 RID: 935
		// (get) Token: 0x06000C10 RID: 3088 RVA: 0x000478A4 File Offset: 0x00045AA4
		// (set) Token: 0x06000C11 RID: 3089 RVA: 0x000478BC File Offset: 0x00045ABC
		public TestCardSize SelectedTestCardSize5
		{
			get
			{
				return this.selectedTestCardSize5;
			}
			set
			{
				bool flag = value != this.selectedTestCardSize5;
				if (flag)
				{
					this.selectedTestCardSize5 = value;
					this.dbConnection.SetIVPTestcardEntry(5, DBIntegratedValidationProcess.TestcardType, Convert.ToString(this.selectedTestCardSize5));
					this.TestBallSize5 = this.InitializeTestBalls(this.selectedTestCardSize5, this.selectedMaterial5);
					this.SelectedTestBallSize5 = this.TestBallSize5.First<string>();
					bool flag2 = this.selectedTestCardSize5 != TestCardSize.Other && this.selectedMaterial5 != Material.Other;
					if (flag2)
					{
						this.TextBoxVisible5 = false;
						this.NumberTestBalls5 = this.calculateNumberTestBalls(this.selectedMaterial5, this.selectedTestCardSize5, this.selectedTestBallSize5, this.testBallSize5);
					}
					else
					{
						bool flag3 = this.selectedMaterial5 == Material.None;
						if (flag3)
						{
							this.TextBoxVisible5 = false;
							this.NumberTestBalls5 = 0;
						}
						else
						{
							bool flag4 = this.selectedMaterial5 == Material.Other && this.selectedTestCardSize5 != TestCardSize.Other;
							if (flag4)
							{
								bool flag5 = this.selectedTestCardSize5 == TestCardSize.Testcard100;
								if (flag5)
								{
									this.NumberTestBalls5 = 100;
								}
								else
								{
									bool flag6 = this.selectedTestCardSize5 == TestCardSize.SingleBall;
									if (flag6)
									{
										this.NumberTestBalls5 = 1;
									}
								}
								this.TextBoxVisible5 = false;
							}
							else
							{
								this.TextBoxVisible5 = true;
								this.NumberTestBalls5 = 1;
							}
						}
					}
					this.OnPropertyChanged("SelectedTestCardSize5");
				}
			}
		}

		// Token: 0x170003A8 RID: 936
		// (get) Token: 0x06000C12 RID: 3090 RVA: 0x00047A24 File Offset: 0x00045C24
		// (set) Token: 0x06000C13 RID: 3091 RVA: 0x00047A3C File Offset: 0x00045C3C
		public List<string> TestBallSize1
		{
			get
			{
				return this.testBallSize1;
			}
			set
			{
				bool flag = value != this.testBallSize1;
				if (flag)
				{
					this.testBallSize1 = value;
					this.OnPropertyChanged("TestBallSize1");
				}
			}
		}

		// Token: 0x170003A9 RID: 937
		// (get) Token: 0x06000C14 RID: 3092 RVA: 0x00047A70 File Offset: 0x00045C70
		// (set) Token: 0x06000C15 RID: 3093 RVA: 0x00047A88 File Offset: 0x00045C88
		public List<string> TestBallSize2
		{
			get
			{
				return this.testBallSize2;
			}
			set
			{
				bool flag = value != this.testBallSize2;
				if (flag)
				{
					this.testBallSize2 = value;
					this.OnPropertyChanged("TestBallSize2");
				}
			}
		}

		// Token: 0x170003AA RID: 938
		// (get) Token: 0x06000C16 RID: 3094 RVA: 0x00047ABC File Offset: 0x00045CBC
		// (set) Token: 0x06000C17 RID: 3095 RVA: 0x00047AD4 File Offset: 0x00045CD4
		public List<string> TestBallSize3
		{
			get
			{
				return this.testBallSize3;
			}
			set
			{
				bool flag = value != this.testBallSize3;
				if (flag)
				{
					this.testBallSize3 = value;
					this.OnPropertyChanged("TestBallSize3");
				}
			}
		}

		// Token: 0x170003AB RID: 939
		// (get) Token: 0x06000C18 RID: 3096 RVA: 0x00047B08 File Offset: 0x00045D08
		// (set) Token: 0x06000C19 RID: 3097 RVA: 0x00047B20 File Offset: 0x00045D20
		public List<string> TestBallSize4
		{
			get
			{
				return this.testBallSize4;
			}
			set
			{
				bool flag = value != this.testBallSize4;
				if (flag)
				{
					this.testBallSize4 = value;
					this.OnPropertyChanged("TestBallSize4");
				}
			}
		}

		// Token: 0x170003AC RID: 940
		// (get) Token: 0x06000C1A RID: 3098 RVA: 0x00047B54 File Offset: 0x00045D54
		// (set) Token: 0x06000C1B RID: 3099 RVA: 0x00047B6C File Offset: 0x00045D6C
		public List<string> TestBallSize5
		{
			get
			{
				return this.testBallSize5;
			}
			set
			{
				bool flag = value != this.testBallSize5;
				if (flag)
				{
					this.testBallSize5 = value;
					this.OnPropertyChanged("TestBallSize5");
				}
			}
		}

		// Token: 0x170003AD RID: 941
		// (get) Token: 0x06000C1C RID: 3100 RVA: 0x00047BA0 File Offset: 0x00045DA0
		// (set) Token: 0x06000C1D RID: 3101 RVA: 0x00047BB8 File Offset: 0x00045DB8
		public string SelectedTestBallSize1
		{
			get
			{
				return this.selectedTestBallSize1;
			}
			set
			{
				bool flag = this.selectedTestBallSize1 != value;
				if (flag)
				{
					this.selectedTestBallSize1 = value;
					this.dbConnection.SetIVPTestcardEntry(1, DBIntegratedValidationProcess.TestcardBallSize, Convert.ToString(this.selectedTestBallSize1));
					this.NumberTestBalls1 = this.calculateNumberTestBalls(this.selectedMaterial1, this.selectedTestCardSize1, this.selectedTestBallSize1, this.testBallSize1);
					this.OnPropertyChanged("SelectedTestBallSize1");
				}
			}
		}

		// Token: 0x170003AE RID: 942
		// (get) Token: 0x06000C1E RID: 3102 RVA: 0x00047C30 File Offset: 0x00045E30
		// (set) Token: 0x06000C1F RID: 3103 RVA: 0x00047C48 File Offset: 0x00045E48
		public string SelectedTestBallSize2
		{
			get
			{
				return this.selectedTestBallSize2;
			}
			set
			{
				bool flag = this.selectedTestBallSize2 != value;
				if (flag)
				{
					this.selectedTestBallSize2 = value;
					this.dbConnection.SetIVPTestcardEntry(2, DBIntegratedValidationProcess.TestcardBallSize, Convert.ToString(this.selectedTestBallSize2));
					this.NumberTestBalls2 = this.calculateNumberTestBalls(this.selectedMaterial2, this.selectedTestCardSize2, this.selectedTestBallSize2, this.testBallSize2);
					this.OnPropertyChanged("SelectedTestBallSize2");
				}
			}
		}

		// Token: 0x170003AF RID: 943
		// (get) Token: 0x06000C20 RID: 3104 RVA: 0x00047CC0 File Offset: 0x00045EC0
		// (set) Token: 0x06000C21 RID: 3105 RVA: 0x00047CD8 File Offset: 0x00045ED8
		public string SelectedTestBallSize3
		{
			get
			{
				return this.selectedTestBallSize3;
			}
			set
			{
				bool flag = this.selectedTestBallSize3 != value;
				if (flag)
				{
					this.selectedTestBallSize3 = value;
					this.dbConnection.SetIVPTestcardEntry(3, DBIntegratedValidationProcess.TestcardBallSize, Convert.ToString(this.selectedTestBallSize3));
					this.NumberTestBalls3 = this.calculateNumberTestBalls(this.selectedMaterial3, this.selectedTestCardSize3, this.selectedTestBallSize3, this.testBallSize3);
					this.OnPropertyChanged("SelectedTestBallSize3");
				}
			}
		}

		// Token: 0x170003B0 RID: 944
		// (get) Token: 0x06000C22 RID: 3106 RVA: 0x00047D50 File Offset: 0x00045F50
		// (set) Token: 0x06000C23 RID: 3107 RVA: 0x00047D68 File Offset: 0x00045F68
		public string SelectedTestBallSize4
		{
			get
			{
				return this.selectedTestBallSize4;
			}
			set
			{
				bool flag = this.selectedTestBallSize4 != value;
				if (flag)
				{
					this.selectedTestBallSize4 = value;
					this.dbConnection.SetIVPTestcardEntry(4, DBIntegratedValidationProcess.TestcardBallSize, Convert.ToString(this.selectedTestBallSize4));
					this.NumberTestBalls4 = this.calculateNumberTestBalls(this.selectedMaterial4, this.selectedTestCardSize4, this.selectedTestBallSize4, this.testBallSize4);
					this.OnPropertyChanged("SelectedTestBallSize4");
				}
			}
		}

		// Token: 0x170003B1 RID: 945
		// (get) Token: 0x06000C24 RID: 3108 RVA: 0x00047DE0 File Offset: 0x00045FE0
		// (set) Token: 0x06000C25 RID: 3109 RVA: 0x00047DF8 File Offset: 0x00045FF8
		public string SelectedTestBallSize5
		{
			get
			{
				return this.selectedTestBallSize5;
			}
			set
			{
				bool flag = this.selectedTestBallSize5 != value;
				if (flag)
				{
					this.selectedTestBallSize5 = value;
					this.dbConnection.SetIVPTestcardEntry(5, DBIntegratedValidationProcess.TestcardBallSize, Convert.ToString(this.selectedTestBallSize5));
					this.NumberTestBalls5 = this.calculateNumberTestBalls(this.selectedMaterial5, this.selectedTestCardSize5, this.selectedTestBallSize5, this.testBallSize5);
					this.OnPropertyChanged("SelectedTestBallSize5");
				}
			}
		}

		// Token: 0x170003B2 RID: 946
		// (get) Token: 0x06000C26 RID: 3110 RVA: 0x00047E70 File Offset: 0x00046070
		// (set) Token: 0x06000C27 RID: 3111 RVA: 0x00047E88 File Offset: 0x00046088
		public int NumberTestBalls1
		{
			get
			{
				return this.numberTestBalls1;
			}
			set
			{
				bool flag = value != this.numberTestBalls1;
				if (flag)
				{
					this.numberTestBalls1 = value;
					this.dbConnection.SetIVPTestcardEntry(1, "TestcardCount", Convert.ToString(this.numberTestBalls1));
					this.OnPropertyChanged("NumberTestBalls1");
				}
			}
		}

		// Token: 0x170003B3 RID: 947
		// (get) Token: 0x06000C28 RID: 3112 RVA: 0x00047ED8 File Offset: 0x000460D8
		// (set) Token: 0x06000C29 RID: 3113 RVA: 0x00047EF0 File Offset: 0x000460F0
		public int NumberTestBalls2
		{
			get
			{
				return this.numberTestBalls2;
			}
			set
			{
				bool flag = value != this.numberTestBalls2;
				if (flag)
				{
					this.numberTestBalls2 = value;
					this.dbConnection.SetIVPTestcardEntry(2, "TestcardCount", Convert.ToString(this.numberTestBalls2));
					this.OnPropertyChanged("NumberTestBalls2");
				}
			}
		}

		// Token: 0x170003B4 RID: 948
		// (get) Token: 0x06000C2A RID: 3114 RVA: 0x00047F40 File Offset: 0x00046140
		// (set) Token: 0x06000C2B RID: 3115 RVA: 0x00047F58 File Offset: 0x00046158
		public int NumberTestBalls3
		{
			get
			{
				return this.numberTestBalls3;
			}
			set
			{
				bool flag = value != this.numberTestBalls3;
				if (flag)
				{
					this.numberTestBalls3 = value;
					this.dbConnection.SetIVPTestcardEntry(3, "TestcardCount", Convert.ToString(this.numberTestBalls3));
					this.OnPropertyChanged("NumberTestBalls3");
				}
			}
		}

		// Token: 0x170003B5 RID: 949
		// (get) Token: 0x06000C2C RID: 3116 RVA: 0x00047FA8 File Offset: 0x000461A8
		// (set) Token: 0x06000C2D RID: 3117 RVA: 0x00047FC0 File Offset: 0x000461C0
		public int NumberTestBalls4
		{
			get
			{
				return this.numberTestBalls4;
			}
			set
			{
				bool flag = value != this.numberTestBalls4;
				if (flag)
				{
					this.numberTestBalls4 = value;
					this.dbConnection.SetIVPTestcardEntry(4, "TestcardCount", Convert.ToString(this.numberTestBalls4));
					this.OnPropertyChanged("NumberTestBalls4");
				}
			}
		}

		// Token: 0x170003B6 RID: 950
		// (get) Token: 0x06000C2E RID: 3118 RVA: 0x00048010 File Offset: 0x00046210
		// (set) Token: 0x06000C2F RID: 3119 RVA: 0x00048028 File Offset: 0x00046228
		public int NumberTestBalls5
		{
			get
			{
				return this.numberTestBalls5;
			}
			set
			{
				bool flag = value != this.numberTestBalls5;
				if (flag)
				{
					this.numberTestBalls5 = value;
					this.dbConnection.SetIVPTestcardEntry(5, "TestcardCount", Convert.ToString(this.numberTestBalls5));
					this.OnPropertyChanged("NumberTestBalls5");
				}
			}
		}

		// Token: 0x170003B7 RID: 951
		// (get) Token: 0x06000C30 RID: 3120 RVA: 0x00048078 File Offset: 0x00046278
		// (set) Token: 0x06000C31 RID: 3121 RVA: 0x00048090 File Offset: 0x00046290
		public bool TextBoxVisible1
		{
			get
			{
				return this.textboxVisible1;
			}
			set
			{
				bool flag = value != this.textboxVisible1;
				if (flag)
				{
					this.textboxVisible1 = value;
					this.OnPropertyChanged("TextBoxVisible1");
				}
			}
		}

		// Token: 0x170003B8 RID: 952
		// (get) Token: 0x06000C32 RID: 3122 RVA: 0x000480C4 File Offset: 0x000462C4
		// (set) Token: 0x06000C33 RID: 3123 RVA: 0x000480DC File Offset: 0x000462DC
		public bool TextBoxVisible2
		{
			get
			{
				return this.textboxVisible2;
			}
			set
			{
				bool flag = value != this.textboxVisible2;
				if (flag)
				{
					this.textboxVisible2 = value;
					this.OnPropertyChanged("TextBoxVisible2");
				}
			}
		}

		// Token: 0x170003B9 RID: 953
		// (get) Token: 0x06000C34 RID: 3124 RVA: 0x00048110 File Offset: 0x00046310
		// (set) Token: 0x06000C35 RID: 3125 RVA: 0x00048128 File Offset: 0x00046328
		public bool TextBoxVisible3
		{
			get
			{
				return this.textboxVisible3;
			}
			set
			{
				bool flag = value != this.textboxVisible3;
				if (flag)
				{
					this.textboxVisible3 = value;
					this.OnPropertyChanged("TextBoxVisible3");
				}
			}
		}

		// Token: 0x170003BA RID: 954
		// (get) Token: 0x06000C36 RID: 3126 RVA: 0x0004815C File Offset: 0x0004635C
		// (set) Token: 0x06000C37 RID: 3127 RVA: 0x00048174 File Offset: 0x00046374
		public bool TextBoxVisible4
		{
			get
			{
				return this.textboxVisible4;
			}
			set
			{
				bool flag = value != this.textboxVisible4;
				if (flag)
				{
					this.textboxVisible4 = value;
					this.OnPropertyChanged("TextBoxVisible4");
				}
			}
		}

		// Token: 0x170003BB RID: 955
		// (get) Token: 0x06000C38 RID: 3128 RVA: 0x000481A8 File Offset: 0x000463A8
		// (set) Token: 0x06000C39 RID: 3129 RVA: 0x000481C0 File Offset: 0x000463C0
		public bool TextBoxVisible5
		{
			get
			{
				return this.textboxVisible5;
			}
			set
			{
				bool flag = value != this.textboxVisible5;
				if (flag)
				{
					this.textboxVisible5 = value;
					this.OnPropertyChanged("TextBoxVisible5");
				}
			}
		}

		// Token: 0x170003BC RID: 956
		// (get) Token: 0x06000C3A RID: 3130 RVA: 0x000481F4 File Offset: 0x000463F4
		// (set) Token: 0x06000C3B RID: 3131 RVA: 0x0004820C File Offset: 0x0004640C
		public string INNumber1
		{
			get
			{
				return this.iNNumber1;
			}
			set
			{
				bool flag = value != this.iNNumber1;
				if (flag)
				{
					this.iNNumber1 = value;
					this.dbConnection.SetIVPTestcardEntry(1, "TestcardINNumber", Convert.ToString(this.iNNumber1));
					this.OnPropertyChanged("INNumber1");
				}
			}
		}

		// Token: 0x170003BD RID: 957
		// (get) Token: 0x06000C3C RID: 3132 RVA: 0x0004825C File Offset: 0x0004645C
		// (set) Token: 0x06000C3D RID: 3133 RVA: 0x00048274 File Offset: 0x00046474
		public string INNumber2
		{
			get
			{
				return this.iNNumber2;
			}
			set
			{
				bool flag = value != this.iNNumber2;
				if (flag)
				{
					this.iNNumber2 = value;
					this.dbConnection.SetIVPTestcardEntry(2, "TestcardINNumber", Convert.ToString(this.iNNumber2));
					this.OnPropertyChanged("INNumber2");
				}
			}
		}

		// Token: 0x170003BE RID: 958
		// (get) Token: 0x06000C3E RID: 3134 RVA: 0x000482C4 File Offset: 0x000464C4
		// (set) Token: 0x06000C3F RID: 3135 RVA: 0x000482DC File Offset: 0x000464DC
		public string INNumber3
		{
			get
			{
				return this.iNNumber3;
			}
			set
			{
				bool flag = value != this.iNNumber3;
				if (flag)
				{
					this.iNNumber3 = value;
					this.dbConnection.SetIVPTestcardEntry(3, "TestcardINNumber", Convert.ToString(this.iNNumber3));
					this.OnPropertyChanged("INNumber3");
				}
			}
		}

		// Token: 0x170003BF RID: 959
		// (get) Token: 0x06000C40 RID: 3136 RVA: 0x0004832C File Offset: 0x0004652C
		// (set) Token: 0x06000C41 RID: 3137 RVA: 0x00048344 File Offset: 0x00046544
		public string INNumber4
		{
			get
			{
				return this.iNNumber4;
			}
			set
			{
				bool flag = value != this.iNNumber4;
				if (flag)
				{
					this.iNNumber4 = value;
					this.dbConnection.SetIVPTestcardEntry(4, "TestcardINNumber", Convert.ToString(this.iNNumber4));
					this.OnPropertyChanged("INNumber4");
				}
			}
		}

		// Token: 0x170003C0 RID: 960
		// (get) Token: 0x06000C42 RID: 3138 RVA: 0x00048394 File Offset: 0x00046594
		// (set) Token: 0x06000C43 RID: 3139 RVA: 0x000483AC File Offset: 0x000465AC
		public string INNumber5
		{
			get
			{
				return this.iNNumber5;
			}
			set
			{
				bool flag = value != this.iNNumber5;
				if (flag)
				{
					this.iNNumber5 = value;
					this.dbConnection.SetIVPTestcardEntry(5, "TestcardINNumber", Convert.ToString(this.iNNumber5));
					this.OnPropertyChanged("INNumber5");
				}
			}
		}

		// Token: 0x170003C1 RID: 961
		// (get) Token: 0x06000C44 RID: 3140 RVA: 0x000483FC File Offset: 0x000465FC
		// (set) Token: 0x06000C45 RID: 3141 RVA: 0x00048414 File Offset: 0x00046614
		public string Density1
		{
			get
			{
				return this.density1;
			}
			set
			{
				bool flag = value != this.density1;
				if (flag)
				{
					this.density1 = value;
					this.dbConnection.SetIVPTestcardEntry(1, "TestcardDensity", Convert.ToString(this.density1));
					this.OnPropertyChanged("Density1");
				}
			}
		}

		// Token: 0x170003C2 RID: 962
		// (get) Token: 0x06000C46 RID: 3142 RVA: 0x00048464 File Offset: 0x00046664
		// (set) Token: 0x06000C47 RID: 3143 RVA: 0x0004847C File Offset: 0x0004667C
		public string Density2
		{
			get
			{
				return this.density2;
			}
			set
			{
				bool flag = value != this.density2;
				if (flag)
				{
					this.density2 = value;
					this.dbConnection.SetIVPTestcardEntry(2, "TestcardDensity", Convert.ToString(this.density2));
					this.OnPropertyChanged("Density2");
				}
			}
		}

		// Token: 0x170003C3 RID: 963
		// (get) Token: 0x06000C48 RID: 3144 RVA: 0x000484CC File Offset: 0x000466CC
		// (set) Token: 0x06000C49 RID: 3145 RVA: 0x000484E4 File Offset: 0x000466E4
		public string Density3
		{
			get
			{
				return this.density3;
			}
			set
			{
				bool flag = value != this.density3;
				if (flag)
				{
					this.density3 = value;
					this.dbConnection.SetIVPTestcardEntry(3, "TestcardDensity", Convert.ToString(this.density3));
					this.OnPropertyChanged("Density3");
				}
			}
		}

		// Token: 0x170003C4 RID: 964
		// (get) Token: 0x06000C4A RID: 3146 RVA: 0x00048534 File Offset: 0x00046734
		// (set) Token: 0x06000C4B RID: 3147 RVA: 0x0004854C File Offset: 0x0004674C
		public string Density4
		{
			get
			{
				return this.density4;
			}
			set
			{
				bool flag = value != this.density4;
				if (flag)
				{
					this.density4 = value;
					this.dbConnection.SetIVPTestcardEntry(4, "TestcardDensity", Convert.ToString(this.density4));
					this.OnPropertyChanged("Density4");
				}
			}
		}

		// Token: 0x170003C5 RID: 965
		// (get) Token: 0x06000C4C RID: 3148 RVA: 0x0004859C File Offset: 0x0004679C
		// (set) Token: 0x06000C4D RID: 3149 RVA: 0x000485B4 File Offset: 0x000467B4
		public string Density5
		{
			get
			{
				return this.density5;
			}
			set
			{
				bool flag = value != this.density5;
				if (flag)
				{
					this.density5 = value;
					this.dbConnection.SetIVPTestcardEntry(5, "TestcardDensity", Convert.ToString(this.density5));
					this.OnPropertyChanged("Density5");
				}
			}
		}

		// Token: 0x06000C4E RID: 3150 RVA: 0x00048604 File Offset: 0x00046804
		public IVPTestsamplesViewModel(AccessRightsManager accessManager, DBConnection dbConnection)
		{
			this.accessManager = accessManager;
			this.dbConnection = dbConnection;
			this.Initialize();
			this.RegisterLocalizedText();
			this.CleanUpCommand = new RelayCommand(new Action<object>(this.CleanUp));
		}

		// Token: 0x06000C4F RID: 3151 RVA: 0x00048783 File Offset: 0x00046983
		public void Initialize()
		{
			this.InitializeMaterialList();
			this.InitializeSelectedParams();
		}

		// Token: 0x06000C50 RID: 3152 RVA: 0x00048794 File Offset: 0x00046994
		public void InitializeSelectedParams()
		{
			List<string> list = new List<string>();
			this.dbConnection.GetIVPTestcardRow(0, ref list);
			bool flag = list.Count == 6;
			if (flag)
			{
				this.SelectedMaterial1 = this.convertStringToTestcardMaterial(list[0]);
				this.SelectedTestCardSize1 = this.convertStringToTestcardType(list[1]);
				this.SelectedTestBallSize1 = list[2];
				this.NumberTestBalls1 = Convert.ToInt32(list[3]);
				this.INNumber1 = list[4];
				this.Density1 = list[5];
			}
			list = new List<string>();
			this.dbConnection.GetIVPTestcardRow(1, ref list);
			bool flag2 = list.Count == 6;
			if (flag2)
			{
				this.SelectedMaterial2 = this.convertStringToTestcardMaterial(list[0]);
				this.SelectedTestCardSize2 = this.convertStringToTestcardType(list[1]);
				this.SelectedTestBallSize2 = list[2];
				this.NumberTestBalls2 = Convert.ToInt32(list[3]);
				this.INNumber2 = list[4];
				this.Density2 = list[5];
			}
			list = new List<string>();
			this.dbConnection.GetIVPTestcardRow(2, ref list);
			bool flag3 = list.Count == 6;
			if (flag3)
			{
				this.SelectedMaterial3 = this.convertStringToTestcardMaterial(list[0]);
				this.SelectedTestCardSize3 = this.convertStringToTestcardType(list[1]);
				this.SelectedTestBallSize3 = list[2];
				this.NumberTestBalls3 = Convert.ToInt32(list[3]);
				this.INNumber3 = list[4];
				this.Density3 = list[5];
			}
			list = new List<string>();
			this.dbConnection.GetIVPTestcardRow(3, ref list);
			bool flag4 = list.Count == 6;
			if (flag4)
			{
				this.SelectedMaterial4 = this.convertStringToTestcardMaterial(list[0]);
				this.SelectedTestCardSize4 = this.convertStringToTestcardType(list[1]);
				this.SelectedTestBallSize4 = list[2];
				this.NumberTestBalls4 = Convert.ToInt32(list[3]);
				this.INNumber4 = list[4];
				this.Density4 = list[5];
			}
			list = new List<string>();
			this.dbConnection.GetIVPTestcardRow(4, ref list);
			bool flag5 = list.Count == 6;
			if (flag5)
			{
				this.SelectedMaterial5 = this.convertStringToTestcardMaterial(list[0]);
				this.SelectedTestCardSize5 = this.convertStringToTestcardType(list[1]);
				this.SelectedTestBallSize5 = list[2];
				this.NumberTestBalls5 = Convert.ToInt32(list[3]);
				this.INNumber5 = list[4];
				this.Density5 = list[5];
			}
		}

		// Token: 0x06000C51 RID: 3153 RVA: 0x00048A54 File Offset: 0x00046C54
		private Material convertStringToTestcardMaterial(string input)
		{
			Material result = Material.None;
			bool flag = input == Convert.ToString(Material.VA);
			if (flag)
			{
				result = Material.VA;
			}
			else
			{
				bool flag2 = input == Convert.ToString(Material.Glass);
				if (flag2)
				{
					result = Material.Glass;
				}
				else
				{
					bool flag3 = input == Convert.ToString(Material.VAWire);
					if (flag3)
					{
						result = Material.VAWire;
					}
					else
					{
						bool flag4 = input == Convert.ToString(Material.Ceramics);
						if (flag4)
						{
							result = Material.Ceramics;
						}
						else
						{
							bool flag5 = input == Convert.ToString(Material.EPDM);
							if (flag5)
							{
								result = Material.EPDM;
							}
							else
							{
								bool flag6 = input == Convert.ToString(Material.PTFE);
								if (flag6)
								{
									result = Material.PTFE;
								}
								else
								{
									bool flag7 = input == Convert.ToString(Material.Aluminium);
									if (flag7)
									{
										result = Material.Aluminium;
									}
									else
									{
										bool flag8 = input == Convert.ToString(Material.Bone);
										if (flag8)
										{
											result = Material.Bone;
										}
										else
										{
											bool flag9 = input == Convert.ToString(Material.Other);
											if (flag9)
											{
												result = Material.Other;
											}
											else
											{
												bool flag10 = input == Convert.ToString(Material.FE);
												if (flag10)
												{
													result = Material.FE;
												}
												else
												{
													bool flag11 = input == Convert.ToString(Material.NonFE);
													if (flag11)
													{
														result = Material.NonFE;
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
			return result;
		}

		// Token: 0x06000C52 RID: 3154 RVA: 0x00048BA4 File Offset: 0x00046DA4
		private TestCardSize convertStringToTestcardType(string input)
		{
			TestCardSize result = TestCardSize.Other;
			bool flag = input == Convert.ToString(TestCardSize.SingleBall);
			if (flag)
			{
				result = TestCardSize.SingleBall;
			}
			else
			{
				bool flag2 = input == Convert.ToString(TestCardSize.Testcard5);
				if (flag2)
				{
					result = TestCardSize.Testcard5;
				}
				else
				{
					bool flag3 = input == Convert.ToString(TestCardSize.Testcard100);
					if (flag3)
					{
						result = TestCardSize.Testcard100;
					}
				}
			}
			return result;
		}

		// Token: 0x06000C53 RID: 3155 RVA: 0x00048C04 File Offset: 0x00046E04
		public List<TestCardSize> InitTestCardSizeList(Material material)
		{
			List<TestCardSize> list = new List<TestCardSize>();
			list.Clear();
			bool flag = material == Material.VA || material == Material.Glass;
			if (flag)
			{
				list.Add(TestCardSize.SingleBall);
				list.Add(TestCardSize.Testcard5);
				list.Add(TestCardSize.Testcard100);
			}
			else
			{
				bool flag2 = material == Material.Aluminium || material == Material.PTFE || material == Material.EPDM || material == Material.Ceramics || material == Material.VAWire || material == Material.FE || material == Material.NonFE;
				if (flag2)
				{
					list.Add(TestCardSize.Testcard5);
				}
				else
				{
					bool flag3 = material == Material.Bone;
					if (flag3)
					{
						list.Add(TestCardSize.SingleBall);
						list.Add(TestCardSize.Testcard5);
					}
					else
					{
						bool flag4 = material == Material.Other;
						if (flag4)
						{
							list.Add(TestCardSize.SingleBall);
							list.Add(TestCardSize.Testcard100);
						}
					}
				}
			}
			list.Add(TestCardSize.Other);
			return list;
		}

		// Token: 0x06000C54 RID: 3156 RVA: 0x00048CC0 File Offset: 0x00046EC0
		public List<string> InitializeTestBalls(TestCardSize testCard, Material material)
		{
			List<string> list = new List<string>();
			bool flag = material == Material.VA;
			if (flag)
			{
				bool flag2 = testCard == TestCardSize.Testcard100;
				if (flag2)
				{
					list.Add("0.6");
					list.Add("0.8");
					list.Add("1.0");
					list.Add("1.2");
					list.Add("1.5");
					list.Add("1.8");
				}
				else
				{
					bool flag3 = testCard == TestCardSize.SingleBall || testCard == TestCardSize.Other;
					if (flag3)
					{
						list.Add("0.2");
					}
					list.Add("0.3");
					list.Add("0.4");
					list.Add("0.5");
					list.Add("0.6");
					bool flag4 = testCard == TestCardSize.Testcard5;
					if (flag4)
					{
						list.Add("0.8 (0.3-0.8)");
						list.Add("0.8 (0.8-1.8)");
					}
					else
					{
						list.Add("0.8");
					}
					list.Add("1.0");
					list.Add("1.2");
					list.Add("1.5");
					bool flag5 = testCard == TestCardSize.Testcard5;
					if (flag5)
					{
						list.Add("1.8 (0.8-1.8)");
						list.Add("1.8 (1.8-3.5)");
					}
					else
					{
						list.Add("1.8");
					}
					list.Add("2.0");
					list.Add("2.5");
					list.Add("3.0");
					bool flag6 = testCard > TestCardSize.SingleBall;
					if (flag6)
					{
						list.Add("3.5");
					}
				}
			}
			else
			{
				bool flag7 = material == Material.Glass;
				if (flag7)
				{
					bool flag8 = testCard == TestCardSize.SingleBall || testCard == TestCardSize.Other;
					if (flag8)
					{
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
						list.Add("5.0");
						list.Add("6.0");
						list.Add("7.0");
						list.Add("8.0");
						list.Add("9.0");
						list.Add("10.0");
					}
					else
					{
						bool flag9 = testCard == TestCardSize.Testcard5;
						if (flag9)
						{
							list.Add("0.8");
							list.Add("1.0");
							list.Add("1.2");
							list.Add("1.5");
							list.Add("2.0 (0.8-2 mm)");
							list.Add("2.0 (2-4 mm)");
							list.Add("2.5");
							list.Add("3.0");
							list.Add("3.5");
							list.Add("4.0 (2-4 mm)");
							list.Add("4.0 (4-8 mm)");
							list.Add("5.0");
							list.Add("6.0");
							list.Add("7.0");
							list.Add("8.0");
						}
						else
						{
							bool flag10 = testCard == TestCardSize.Testcard100;
							if (flag10)
							{
								list.Add("1.5");
								list.Add("2.0");
								list.Add("2.5");
								list.Add("3.0");
								list.Add("3.5");
								list.Add("4.0");
							}
						}
					}
				}
				else
				{
					bool flag11 = material == Material.EPDM;
					if (flag11)
					{
						list.Add("6.0");
						list.Add("7.0");
						list.Add("8.0");
						list.Add("9.0");
						list.Add("10.0");
					}
					else
					{
						bool flag12 = material == Material.PTFE;
						if (flag12)
						{
							list.Add("3.0");
							list.Add("4.0");
							list.Add("5.0");
							list.Add("6.0");
							list.Add("7.0");
						}
						else
						{
							bool flag13 = material == Material.Ceramics || material == Material.Aluminium;
							if (flag13)
							{
								list.Add("2.0");
								list.Add("3.0");
								list.Add("4.0");
								list.Add("5.0");
								list.Add("6.0");
							}
							else
							{
								bool flag14 = material == Material.VAWire;
								if (flag14)
								{
									list.Add("0.15x2.0");
									list.Add("0.2x2.0");
									bool flag15 = testCard == TestCardSize.Testcard5;
									if (flag15)
									{
										list.Add("0.3 (0.15-0.5 x 2.0)");
										list.Add("0.4 (0.15-0.5 x 2.0)");
										list.Add("0.5 (0.15-0.5 x 2.0)");
										list.Add("0.3 (0.3-0.8 x 2.0)");
										list.Add("0.4 (0.3-0.8 x 2.0)");
										list.Add("0.5 (0.3-0.8 x 2.0)");
									}
									else
									{
										list.Add("0.3x2.0");
										list.Add("0.4x2.0");
										list.Add("0.5x2.0");
									}
									list.Add("0.6x2.0");
									list.Add("0.8x2.0");
									list.Add("0.3x5.0");
									list.Add("0.4x5.0");
									list.Add("0.5x5.0");
									list.Add("0.6x5.0");
									list.Add("0.8x5.0");
								}
								else
								{
									bool flag16 = material == Material.Bone;
									if (flag16)
									{
										bool flag17 = testCard == TestCardSize.SingleBall;
										if (flag17)
										{
											list.Add("2x2x2");
											list.Add("3x3x3");
											list.Add("4x4x4");
											list.Add("6x6x6");
											list.Add("8x8x8");
											list.Add("10x10x10");
										}
										else
										{
											list.Add("2x2x2");
											list.Add("3x3x3");
											list.Add("4x4x4");
											list.Add("5x5x5");
											bool flag18 = testCard == TestCardSize.Testcard5;
											if (flag18)
											{
												list.Add("6x6x6 (2-6)");
												list.Add("6x6x6 (6-10)");
											}
											else
											{
												list.Add("6x6x6");
											}
											list.Add("7x7x7");
											list.Add("8x8x8");
											list.Add("9x9x9");
											list.Add("10x10x10");
										}
									}
									else
									{
										bool flag19 = material == Material.Other;
										if (flag19)
										{
											list.Add("-");
										}
										else
										{
											bool flag20 = material == Material.FE || material == Material.NonFE;
											if (flag20)
											{
												list.Add("0.8");
												list.Add("1.0");
												list.Add("1.2");
												list.Add("1.5");
												list.Add("1.8 (0.8-1.8)");
												list.Add("1.8 (1.8-3.5)");
												list.Add("2.0");
												list.Add("2.5");
												list.Add("3.0");
												list.Add("3.5");
											}
											else
											{
												list.Add("n.a.");
											}
										}
									}
								}
							}
						}
					}
				}
			}
			return list;
		}

		// Token: 0x06000C55 RID: 3157 RVA: 0x0004940C File Offset: 0x0004760C
		private void InitializeMaterialList()
		{
			this.MaterialList = new List<Material>();
			this.MaterialList.Add(Material.Glass);
			this.MaterialList.Add(Material.VA);
			this.MaterialList.Add(Material.VAWire);
			this.MaterialList.Add(Material.EPDM);
			this.MaterialList.Add(Material.Bone);
			this.MaterialList.Add(Material.PTFE);
			this.MaterialList.Add(Material.Aluminium);
			this.MaterialList.Add(Material.Ceramics);
			this.MaterialList.Add(Material.FE);
			this.MaterialList.Add(Material.NonFE);
			this.MaterialList.Add(Material.Other);
			this.MaterialList.Add(Material.None);
		}

		// Token: 0x06000C56 RID: 3158 RVA: 0x000494C8 File Offset: 0x000476C8
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x06000C57 RID: 3159 RVA: 0x000494F8 File Offset: 0x000476F8
		public void ShowEditKeyboard(int item)
		{
			this.chosenTextbox = item;
			string oldValue;
			switch (this.chosenTextbox)
			{
			case 1:
				oldValue = this.NumberTestBalls1.ToString();
				break;
			case 2:
				oldValue = this.NumberTestBalls2.ToString();
				break;
			case 3:
				oldValue = this.NumberTestBalls3.ToString();
				break;
			case 4:
				oldValue = this.NumberTestBalls4.ToString();
				break;
			case 5:
				oldValue = this.NumberTestBalls5.ToString();
				break;
			default:
				oldValue = "";
				break;
			}
			this.numKeyboard = new NumKeyboard("", oldValue, NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.keyboard_TextInputKeyboard;
			this.numKeyboard.Show();
		}

		// Token: 0x06000C58 RID: 3160 RVA: 0x000495D0 File Offset: 0x000477D0
		private void keyboard_TextInputKeyboard(object sender, KeyboardInputEventArgs e)
		{
			bool flag = e.Text != null && !e.Text.Equals("");
			if (flag)
			{
				int num = int.Parse(e.Text);
				bool flag2 = num > 100;
				if (flag2)
				{
					num = 100;
					Dialog.Show(LocalizationManager.Localize("txtInfo"), LocalizationManager.Localize("txtWrongInput"), DialogStyle.Info, DialogButtons.OK);
				}
				bool flag3 = num < 1;
				if (flag3)
				{
					num = 1;
					Dialog.Show(LocalizationManager.Localize("txtInfo"), LocalizationManager.Localize("txtWrongInput"), DialogStyle.Info, DialogButtons.OK);
				}
				switch (this.chosenTextbox)
				{
				case 1:
					this.NumberTestBalls1 = num;
					break;
				case 2:
					this.NumberTestBalls2 = num;
					break;
				case 3:
					this.NumberTestBalls3 = num;
					break;
				case 4:
					this.NumberTestBalls4 = num;
					break;
				case 5:
					this.NumberTestBalls5 = num;
					break;
				}
			}
			this.numKeyboard.TextInputKeyboard -= this.keyboard_TextInputKeyboard;
		}

		// Token: 0x06000C59 RID: 3161 RVA: 0x000496D8 File Offset: 0x000478D8
		public int calculateNumberTestBalls(Material material, TestCardSize testCardSize, string value, List<string> list)
		{
			bool flag = testCardSize == TestCardSize.Testcard100;
			int result;
			if (flag)
			{
				result = 100;
			}
			else
			{
				bool flag2 = testCardSize == TestCardSize.SingleBall;
				if (flag2)
				{
					result = 1;
				}
				else
				{
					bool flag3 = testCardSize == TestCardSize.Testcard5;
					if (flag3)
					{
						int i;
						for (i = list.Count<string>() - list.IndexOf(value); i > 5; i -= 5)
						{
						}
						result = i;
					}
					else
					{
						bool flag4 = testCardSize == TestCardSize.Other;
						if (flag4)
						{
							result = 1;
						}
						else
						{
							result = 0;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06000C5A RID: 3162 RVA: 0x00049748 File Offset: 0x00047948
		public void ShowEditKeyboardINNumber(int item)
		{
			this.chosenTextbox = item;
			string oldValue;
			switch (this.chosenTextbox)
			{
			case 1:
				oldValue = this.INNumber1;
				break;
			case 2:
				oldValue = this.INNumber2;
				break;
			case 3:
				oldValue = this.INNumber3;
				break;
			case 4:
				oldValue = this.INNumber4;
				break;
			case 5:
				oldValue = this.INNumber5;
				break;
			default:
				oldValue = "";
				break;
			}
			this.keyboard = new Keyboard("", oldValue, KeyboardType.Default);
			this.keyboard.TextInputKeyboard += this.keyboard_TextInputKeyboardINNumber;
			this.keyboard.Show();
		}

		// Token: 0x06000C5B RID: 3163 RVA: 0x000497F8 File Offset: 0x000479F8
		private void keyboard_TextInputKeyboardINNumber(object sender, KeyboardInputEventArgs e)
		{
			bool flag = e.Text != null;
			if (flag)
			{
				string text = e.Text;
				bool flag2 = text.Length > 12;
				if (flag2)
				{
					text = text.Substring(0, 12);
				}
				switch (this.chosenTextbox)
				{
				case 1:
					this.INNumber1 = text;
					break;
				case 2:
					this.INNumber2 = text;
					break;
				case 3:
					this.INNumber3 = text;
					break;
				case 4:
					this.INNumber4 = text;
					break;
				case 5:
					this.INNumber5 = text;
					break;
				}
			}
			this.keyboard.TextInputKeyboard -= this.keyboard_TextInputKeyboardINNumber;
		}

		// Token: 0x06000C5C RID: 3164 RVA: 0x000498A8 File Offset: 0x00047AA8
		public void ShowEditKeyboardDensity(int item)
		{
			this.chosenTextbox = item;
			string oldValue;
			switch (this.chosenTextbox)
			{
			case 1:
				oldValue = this.Density1;
				break;
			case 2:
				oldValue = this.Density2;
				break;
			case 3:
				oldValue = this.Density3;
				break;
			case 4:
				oldValue = this.Density4;
				break;
			case 5:
				oldValue = this.Density5;
				break;
			default:
				oldValue = "";
				break;
			}
			this.keyboard = new Keyboard("", oldValue, KeyboardType.Default);
			this.keyboard.TextInputKeyboard += this.keyboard_TextInputKeyboardDensity;
			this.keyboard.Show();
		}

		// Token: 0x06000C5D RID: 3165 RVA: 0x00049958 File Offset: 0x00047B58
		private void keyboard_TextInputKeyboardDensity(object sender, KeyboardInputEventArgs e)
		{
			bool flag = e.Text != null;
			if (flag)
			{
				string text = e.Text;
				bool flag2 = text.Length > 12;
				if (flag2)
				{
					text = text.Substring(0, 12);
				}
				switch (this.chosenTextbox)
				{
				case 1:
					this.Density1 = text;
					break;
				case 2:
					this.Density2 = text;
					break;
				case 3:
					this.Density3 = text;
					break;
				case 4:
					this.Density4 = text;
					break;
				case 5:
					this.Density5 = text;
					break;
				}
			}
			this.keyboard.TextInputKeyboard -= this.keyboard_TextInputKeyboardDensity;
		}

		// Token: 0x06000C5E RID: 3166 RVA: 0x00049A08 File Offset: 0x00047C08
		private void RegisterLocalizedText()
		{
			this.StrMaterial = new BoundLocalizedText("txtMaterial");
			this.StrBallSize = new BoundLocalizedText("txtSize");
			this.StrSettings = new BoundLocalizedText("txtSettings");
			this.StrNumberOfBalls = new BoundLocalizedText("txtNumberOfBalls");
			this.StrTestcardSize = new BoundLocalizedText("txtTestcardSize");
			this.StrINNumber = new BoundLocalizedText("txtINNumber");
			this.StrDensity = new BoundLocalizedText("txtDensity");
			this.StrTestContaminations = new BoundLocalizedText("menuItemIVPTestsamples");
		}

		// Token: 0x06000C5F RID: 3167 RVA: 0x00049AA0 File Offset: 0x00047CA0
		public void DeregisterLocalizedText()
		{
			MainWindow.localizationManager.Deregister(this.StrMaterial);
			MainWindow.localizationManager.Deregister(this.StrBallSize);
			MainWindow.localizationManager.Deregister(this.StrSettings);
			MainWindow.localizationManager.Deregister(this.StrNumberOfBalls);
			MainWindow.localizationManager.Deregister(this.StrTestcardSize);
			MainWindow.localizationManager.Deregister(this.StrINNumber);
			MainWindow.localizationManager.Deregister(this.StrDensity);
			MainWindow.localizationManager.Deregister(this.StrTestContaminations);
		}

		// Token: 0x06000C60 RID: 3168 RVA: 0x00049B38 File Offset: 0x00047D38
		public string getDensity(Material material)
		{
			switch (material)
			{
			case Material.VA:
				return "8,0 g/cm^3";
			case Material.Glass:
				return "2,5 g/cm^3";
			case Material.VAWire:
				return "8,0 g/cm^3";
			case Material.Ceramics:
				return "- g/cm^3";
			case Material.EPDM:
				return "1,2 g/cm^3";
			case Material.PTFE:
				return "2,1 g/cm^3";
			case Material.Aluminium:
				return "2,7 g/cm^3";
			case Material.Bone:
				return "1,6 g/cm^3";
			case Material.None:
				return "- g/cm^3";
			case Material.FE:
				return "7,8g/cm^3";
			case Material.NonFE:
				return "8,4g/cm^3";
			}
			return "- g/cm^3";
		}

		// Token: 0x06000C61 RID: 3169 RVA: 0x00049BF0 File Offset: 0x00047DF0
		public void ImportAuditCheckSettings()
		{
			bool flag = Dialog.Show(LocalizationManager.Localize("txtInfo"), LocalizationManager.Localize("txtImportFromAuditCheck"), DialogStyle.Info, DialogButtons.OKCancel) == Dialog.DialogResults.Ok;
			if (flag)
			{
				string text = "";
				this.dbConnection.GetParameterValue(this.TestcardMaterialPath1, ref text);
				this.SelectedMaterial1 = this.convertStringToTestcardMaterial(text);
				bool flag2 = this.SelectedMaterial1 != Material.None;
				if (flag2)
				{
					this.dbConnection.GetParameterValue(this.TestcardType1, ref text);
					this.SelectedTestCardSize1 = this.convertStringToTestcardType(text);
					this.dbConnection.GetParameterValue(this.TestcardSize1, ref text);
					this.SelectedTestBallSize1 = text;
					this.dbConnection.GetParameterValue(this.TestcardNumberCont1, ref text);
					this.NumberTestBalls1 = Convert.ToInt32(text);
					this.dbConnection.GetParameterValue(this.TestcardNumber1, ref text);
					this.INNumber1 = text;
				}
				this.dbConnection.GetParameterValue(this.TestcardMaterialPath2, ref text);
				this.SelectedMaterial2 = this.convertStringToTestcardMaterial(text);
				bool flag3 = this.SelectedMaterial2 != Material.None;
				if (flag3)
				{
					this.dbConnection.GetParameterValue(this.TestcardType2, ref text);
					this.SelectedTestCardSize2 = this.convertStringToTestcardType(text);
					this.dbConnection.GetParameterValue(this.TestcardSize2, ref text);
					this.SelectedTestBallSize2 = text;
					this.dbConnection.GetParameterValue(this.TestcardNumberCont2, ref text);
					this.NumberTestBalls2 = Convert.ToInt32(text);
					this.dbConnection.GetParameterValue(this.TestcardNumber2, ref text);
					this.INNumber2 = text;
				}
				this.dbConnection.GetParameterValue(this.TestcardMaterialPath3, ref text);
				this.SelectedMaterial3 = this.convertStringToTestcardMaterial(text);
				bool flag4 = this.SelectedMaterial3 != Material.None;
				if (flag4)
				{
					this.dbConnection.GetParameterValue(this.TestcardType3, ref text);
					this.SelectedTestCardSize3 = this.convertStringToTestcardType(text);
					this.dbConnection.GetParameterValue(this.TestcardSize3, ref text);
					this.SelectedTestBallSize3 = text;
					this.dbConnection.GetParameterValue(this.TestcardNumberCont3, ref text);
					this.NumberTestBalls3 = Convert.ToInt32(text);
					this.dbConnection.GetParameterValue(this.TestcardNumber3, ref text);
					this.INNumber3 = text;
				}
				this.dbConnection.GetParameterValue(this.TestcardMaterialPath4, ref text);
				this.SelectedMaterial4 = this.convertStringToTestcardMaterial(text);
				bool flag5 = this.SelectedMaterial4 != Material.None;
				if (flag5)
				{
					this.dbConnection.GetParameterValue(this.TestcardType4, ref text);
					this.SelectedTestCardSize4 = this.convertStringToTestcardType(text);
					this.dbConnection.GetParameterValue(this.TestcardSize4, ref text);
					this.SelectedTestBallSize4 = text;
					this.dbConnection.GetParameterValue(this.TestcardNumberCont4, ref text);
					this.NumberTestBalls4 = Convert.ToInt32(text);
					this.dbConnection.GetParameterValue(this.TestcardNumber4, ref text);
					this.INNumber4 = text;
				}
				this.dbConnection.GetParameterValue(this.TestcardMaterialPath5, ref text);
				this.SelectedMaterial5 = this.convertStringToTestcardMaterial(text);
				bool flag6 = this.SelectedMaterial5 != Material.None;
				if (flag6)
				{
					this.dbConnection.GetParameterValue(this.TestcardType5, ref text);
					this.SelectedTestCardSize5 = this.convertStringToTestcardType(text);
					this.dbConnection.GetParameterValue(this.TestcardSize5, ref text);
					this.SelectedTestBallSize5 = text;
					this.dbConnection.GetParameterValue(this.TestcardNumberCont5, ref text);
					this.NumberTestBalls5 = Convert.ToInt32(text);
					this.dbConnection.GetParameterValue(this.TestcardNumber5, ref text);
					this.INNumber5 = text;
				}
				this.Density1 = this.getDensity(this.SelectedMaterial1);
				this.Density2 = this.getDensity(this.SelectedMaterial2);
				this.Density3 = this.getDensity(this.SelectedMaterial3);
				this.Density4 = this.getDensity(this.SelectedMaterial4);
				this.Density5 = this.getDensity(this.SelectedMaterial5);
			}
		}

		// Token: 0x06000C62 RID: 3170 RVA: 0x00049FFC File Offset: 0x000481FC
		private void CleanUp(object obj)
		{
			this.DeregisterLocalizedText();
			bool flag = this.MaterialList != null;
			if (flag)
			{
				this.MaterialList.Clear();
				this.MaterialList = null;
			}
			bool flag2 = this.TestCardSize1 != null;
			if (flag2)
			{
				this.TestCardSize1.Clear();
				this.TestCardSize1 = null;
			}
			bool flag3 = this.TestCardSize2 != null;
			if (flag3)
			{
				this.TestCardSize2.Clear();
				this.TestCardSize2 = null;
			}
			bool flag4 = this.TestCardSize3 != null;
			if (flag4)
			{
				this.TestCardSize3.Clear();
				this.TestCardSize3 = null;
			}
			bool flag5 = this.TestCardSize4 != null;
			if (flag5)
			{
				this.TestCardSize4.Clear();
				this.TestCardSize4 = null;
			}
			bool flag6 = this.TestCardSize5 != null;
			if (flag6)
			{
				this.TestCardSize5.Clear();
				this.TestCardSize5 = null;
			}
			bool flag7 = this.TestBallSize1 != null;
			if (flag7)
			{
				this.TestBallSize1.Clear();
				this.TestBallSize1 = null;
			}
			bool flag8 = this.TestBallSize2 != null;
			if (flag8)
			{
				this.TestBallSize2.Clear();
				this.TestBallSize2 = null;
			}
			bool flag9 = this.TestBallSize3 != null;
			if (flag9)
			{
				this.TestBallSize3.Clear();
				this.TestBallSize3 = null;
			}
			bool flag10 = this.TestBallSize4 != null;
			if (flag10)
			{
				this.TestBallSize4.Clear();
				this.TestBallSize4 = null;
			}
			bool flag11 = this.TestBallSize5 != null;
			if (flag11)
			{
				this.TestBallSize5.Clear();
				this.TestBallSize5 = null;
			}
		}

		// Token: 0x040007D5 RID: 2005
		private readonly string TestcardMaterialPath1 = "TestcardMaterial1";

		// Token: 0x040007D6 RID: 2006
		private readonly string TestcardType1 = "TestcardType1";

		// Token: 0x040007D7 RID: 2007
		private readonly string TestcardSize1 = "TestcardSize1";

		// Token: 0x040007D8 RID: 2008
		private readonly string TestcardNumberCont1 = "TestcardNumberCont1";

		// Token: 0x040007D9 RID: 2009
		private readonly string TestcardNumber1 = "TestcardNumber1";

		// Token: 0x040007DA RID: 2010
		private readonly string TestcardMaterialPath2 = "TestcardMaterial2";

		// Token: 0x040007DB RID: 2011
		private readonly string TestcardType2 = "TestcardType2";

		// Token: 0x040007DC RID: 2012
		private readonly string TestcardSize2 = "TestcardSize2";

		// Token: 0x040007DD RID: 2013
		private readonly string TestcardNumberCont2 = "TestcardNumberCont2";

		// Token: 0x040007DE RID: 2014
		private readonly string TestcardNumber2 = "TestcardNumber2";

		// Token: 0x040007DF RID: 2015
		private readonly string TestcardMaterialPath3 = "TestcardMaterial3";

		// Token: 0x040007E0 RID: 2016
		private readonly string TestcardType3 = "TestcardType3";

		// Token: 0x040007E1 RID: 2017
		private readonly string TestcardSize3 = "TestcardSize3";

		// Token: 0x040007E2 RID: 2018
		private readonly string TestcardNumberCont3 = "TestcardNumberCont3";

		// Token: 0x040007E3 RID: 2019
		private readonly string TestcardNumber3 = "TestcardNumber3";

		// Token: 0x040007E4 RID: 2020
		private readonly string TestcardMaterialPath4 = "TestcardMaterial4";

		// Token: 0x040007E5 RID: 2021
		private readonly string TestcardType4 = "TestcardType4";

		// Token: 0x040007E6 RID: 2022
		private readonly string TestcardSize4 = "TestcardSize4";

		// Token: 0x040007E7 RID: 2023
		private readonly string TestcardNumberCont4 = "TestcardNumberCont4";

		// Token: 0x040007E8 RID: 2024
		private readonly string TestcardNumber4 = "TestcardNumber4";

		// Token: 0x040007E9 RID: 2025
		private readonly string TestcardMaterialPath5 = "TestcardMaterial5";

		// Token: 0x040007EA RID: 2026
		private readonly string TestcardType5 = "TestcardType5";

		// Token: 0x040007EB RID: 2027
		private readonly string TestcardSize5 = "TestcardSize5";

		// Token: 0x040007EC RID: 2028
		private readonly string TestcardNumberCont5 = "TestcardNumberCont5";

		// Token: 0x040007ED RID: 2029
		private readonly string TestcardNumber5 = "TestcardNumber5";

		// Token: 0x040007F0 RID: 2032
		private DBConnection dbConnection;

		// Token: 0x040007F1 RID: 2033
		private NumKeyboard numKeyboard;

		// Token: 0x040007F2 RID: 2034
		private Keyboard keyboard;

		// Token: 0x040007FB RID: 2043
		private AccessRightsManager accessManager;

		// Token: 0x040007FE RID: 2046
		private Material selectedMaterial1 = Material.None;

		// Token: 0x040007FF RID: 2047
		private Material selectedMaterial2 = Material.None;

		// Token: 0x04000800 RID: 2048
		private Material selectedMaterial3 = Material.None;

		// Token: 0x04000801 RID: 2049
		private Material selectedMaterial4 = Material.None;

		// Token: 0x04000802 RID: 2050
		private Material selectedMaterial5 = Material.None;

		// Token: 0x04000803 RID: 2051
		public List<TestCardSize> testCardSize1;

		// Token: 0x04000804 RID: 2052
		public List<TestCardSize> testCardSize2;

		// Token: 0x04000805 RID: 2053
		public List<TestCardSize> testCardSize3;

		// Token: 0x04000806 RID: 2054
		public List<TestCardSize> testCardSize4;

		// Token: 0x04000807 RID: 2055
		public List<TestCardSize> testCardSize5;

		// Token: 0x04000808 RID: 2056
		private TestCardSize selectedTestCardSize1;

		// Token: 0x04000809 RID: 2057
		private TestCardSize selectedTestCardSize2;

		// Token: 0x0400080A RID: 2058
		private TestCardSize selectedTestCardSize3;

		// Token: 0x0400080B RID: 2059
		private TestCardSize selectedTestCardSize4;

		// Token: 0x0400080C RID: 2060
		private TestCardSize selectedTestCardSize5;

		// Token: 0x0400080D RID: 2061
		private List<string> testBallSize1;

		// Token: 0x0400080E RID: 2062
		private List<string> testBallSize2;

		// Token: 0x0400080F RID: 2063
		private List<string> testBallSize3;

		// Token: 0x04000810 RID: 2064
		private List<string> testBallSize4;

		// Token: 0x04000811 RID: 2065
		private List<string> testBallSize5;

		// Token: 0x04000812 RID: 2066
		private string selectedTestBallSize1;

		// Token: 0x04000813 RID: 2067
		private string selectedTestBallSize2;

		// Token: 0x04000814 RID: 2068
		private string selectedTestBallSize3;

		// Token: 0x04000815 RID: 2069
		private string selectedTestBallSize4;

		// Token: 0x04000816 RID: 2070
		private string selectedTestBallSize5;

		// Token: 0x04000817 RID: 2071
		private int numberTestBalls1;

		// Token: 0x04000818 RID: 2072
		private int numberTestBalls2;

		// Token: 0x04000819 RID: 2073
		private int numberTestBalls3;

		// Token: 0x0400081A RID: 2074
		private int numberTestBalls4;

		// Token: 0x0400081B RID: 2075
		private int numberTestBalls5;

		// Token: 0x0400081C RID: 2076
		private bool textboxVisible1;

		// Token: 0x0400081D RID: 2077
		private bool textboxVisible2;

		// Token: 0x0400081E RID: 2078
		private bool textboxVisible3;

		// Token: 0x0400081F RID: 2079
		private bool textboxVisible4;

		// Token: 0x04000820 RID: 2080
		private bool textboxVisible5;

		// Token: 0x04000821 RID: 2081
		private string iNNumber1;

		// Token: 0x04000822 RID: 2082
		private string iNNumber2;

		// Token: 0x04000823 RID: 2083
		private string iNNumber3;

		// Token: 0x04000824 RID: 2084
		private string iNNumber4;

		// Token: 0x04000825 RID: 2085
		private string iNNumber5;

		// Token: 0x04000826 RID: 2086
		private string density1;

		// Token: 0x04000827 RID: 2087
		private string density2;

		// Token: 0x04000828 RID: 2088
		private string density3;

		// Token: 0x04000829 RID: 2089
		private string density4;

		// Token: 0x0400082A RID: 2090
		private string density5;
	}
}
