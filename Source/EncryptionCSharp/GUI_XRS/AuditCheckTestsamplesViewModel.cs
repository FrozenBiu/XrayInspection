using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using Common;
using DatabaseConnection;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x0200002B RID: 43
	public class AuditCheckTestsamplesViewModel : INotifyPropertyChanged
	{
		// Token: 0x14000021 RID: 33
		// (add) Token: 0x06000542 RID: 1346 RVA: 0x00023D24 File Offset: 0x00021F24
		// (remove) Token: 0x06000543 RID: 1347 RVA: 0x00023D5C File Offset: 0x00021F5C
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x06000544 RID: 1348 RVA: 0x00023D91 File Offset: 0x00021F91
		// (set) Token: 0x06000545 RID: 1349 RVA: 0x00023D99 File Offset: 0x00021F99
		public RelayCommand CleanUpCommand { get; set; }

		// Token: 0x17000197 RID: 407
		// (get) Token: 0x06000546 RID: 1350 RVA: 0x00023DA2 File Offset: 0x00021FA2
		// (set) Token: 0x06000547 RID: 1351 RVA: 0x00023DAA File Offset: 0x00021FAA
		public BoundLocalizedText StrMaterial { get; set; }

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x06000548 RID: 1352 RVA: 0x00023DB3 File Offset: 0x00021FB3
		// (set) Token: 0x06000549 RID: 1353 RVA: 0x00023DBB File Offset: 0x00021FBB
		public BoundLocalizedText StrBallSize { get; set; }

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x0600054A RID: 1354 RVA: 0x00023DC4 File Offset: 0x00021FC4
		// (set) Token: 0x0600054B RID: 1355 RVA: 0x00023DCC File Offset: 0x00021FCC
		public BoundLocalizedText StrTestcardSize { get; set; }

		// Token: 0x1700019A RID: 410
		// (get) Token: 0x0600054C RID: 1356 RVA: 0x00023DD5 File Offset: 0x00021FD5
		// (set) Token: 0x0600054D RID: 1357 RVA: 0x00023DDD File Offset: 0x00021FDD
		public BoundLocalizedText StrNumberOfBalls { get; set; }

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x0600054E RID: 1358 RVA: 0x00023DE6 File Offset: 0x00021FE6
		// (set) Token: 0x0600054F RID: 1359 RVA: 0x00023DEE File Offset: 0x00021FEE
		public BoundLocalizedText StrSettings { get; set; }

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x06000550 RID: 1360 RVA: 0x00023DF7 File Offset: 0x00021FF7
		// (set) Token: 0x06000551 RID: 1361 RVA: 0x00023DFF File Offset: 0x00021FFF
		public BoundLocalizedText StrINNumber { get; set; }

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x06000552 RID: 1362 RVA: 0x00023E08 File Offset: 0x00022008
		// (set) Token: 0x06000553 RID: 1363 RVA: 0x00023E10 File Offset: 0x00022010
		public int chosenTextbox { get; set; }

		// Token: 0x1700019E RID: 414
		// (get) Token: 0x06000554 RID: 1364 RVA: 0x00023E19 File Offset: 0x00022019
		// (set) Token: 0x06000555 RID: 1365 RVA: 0x00023E21 File Offset: 0x00022021
		public List<Material> MaterialList { get; set; }

		// Token: 0x1700019F RID: 415
		// (get) Token: 0x06000556 RID: 1366 RVA: 0x00023E2C File Offset: 0x0002202C
		// (set) Token: 0x06000557 RID: 1367 RVA: 0x00023E44 File Offset: 0x00022044
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
					this.dbConnection.UpdateCurrentProductTable(this.TestcardMaterialPath1, Convert.ToString(this.selectedMaterial1));
					bool flag2 = this.selectedMaterial1 != Material.None;
					if (flag2)
					{
						this.TestCardSize1 = this.InitTestCardSizeList(this.selectedMaterial1);
						this.SelectedTestCardSize1 = this.TestCardSize1.First<TestCardSize>();
						this.TestBallSize1 = this.InitializeTestBalls(this.selectedTestCardSize1, this.selectedMaterial1);
						this.SelectedTestBallSize1 = this.TestBallSize1.First<string>();
					}
					else
					{
						this.TestCardSize1 = new List<TestCardSize>();
						this.TestBallSize1 = new List<string>();
						this.TextBoxVisible1 = false;
						this.NumberTestBalls1 = 0;
						this.INNumber1 = "n.a.";
					}
					this.OnPropertyChanged("SelectedMaterial1");
				}
			}
		}

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x06000558 RID: 1368 RVA: 0x00023F38 File Offset: 0x00022138
		// (set) Token: 0x06000559 RID: 1369 RVA: 0x00023F50 File Offset: 0x00022150
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
					this.dbConnection.UpdateCurrentProductTable(this.TestcardMaterialPath2, Convert.ToString(this.selectedMaterial2));
					bool flag2 = this.selectedMaterial2 != Material.None;
					if (flag2)
					{
						this.TestCardSize2 = this.InitTestCardSizeList(this.selectedMaterial2);
						this.SelectedTestCardSize2 = this.TestCardSize2.First<TestCardSize>();
						this.TestBallSize2 = this.InitializeTestBalls(this.selectedTestCardSize2, this.selectedMaterial2);
						this.SelectedTestBallSize2 = this.TestBallSize2.First<string>();
					}
					else
					{
						this.TestCardSize2 = new List<TestCardSize>();
						this.TestBallSize2 = new List<string>();
						this.TextBoxVisible2 = false;
						this.NumberTestBalls2 = 0;
						this.INNumber2 = "n.a.";
					}
					this.OnPropertyChanged("SelectedMaterial2");
				}
			}
		}

		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x0600055A RID: 1370 RVA: 0x00024044 File Offset: 0x00022244
		// (set) Token: 0x0600055B RID: 1371 RVA: 0x0002405C File Offset: 0x0002225C
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
					this.dbConnection.UpdateCurrentProductTable(this.TestcardMaterialPath3, Convert.ToString(this.selectedMaterial3));
					bool flag2 = this.selectedMaterial3 != Material.None;
					if (flag2)
					{
						this.TestCardSize3 = this.InitTestCardSizeList(this.selectedMaterial3);
						this.SelectedTestCardSize3 = this.TestCardSize3.First<TestCardSize>();
						this.TestBallSize3 = this.InitializeTestBalls(this.selectedTestCardSize3, this.selectedMaterial3);
						this.SelectedTestBallSize3 = this.TestBallSize3.First<string>();
					}
					else
					{
						this.TestCardSize3 = new List<TestCardSize>();
						this.TestBallSize3 = new List<string>();
						this.TextBoxVisible3 = false;
						this.NumberTestBalls3 = 0;
						this.INNumber3 = "n.a.";
					}
					this.OnPropertyChanged("SelectedMaterial3");
				}
			}
		}

		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x0600055C RID: 1372 RVA: 0x00024150 File Offset: 0x00022350
		// (set) Token: 0x0600055D RID: 1373 RVA: 0x00024168 File Offset: 0x00022368
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
					this.dbConnection.UpdateCurrentProductTable(this.TestcardMaterialPath4, Convert.ToString(this.selectedMaterial4));
					bool flag2 = this.selectedMaterial4 != Material.None;
					if (flag2)
					{
						this.TestCardSize4 = this.InitTestCardSizeList(this.selectedMaterial4);
						this.SelectedTestCardSize4 = this.TestCardSize4.First<TestCardSize>();
						this.TestBallSize4 = this.InitializeTestBalls(this.selectedTestCardSize4, this.selectedMaterial4);
						this.SelectedTestBallSize4 = this.TestBallSize4.First<string>();
					}
					else
					{
						this.TestCardSize4 = new List<TestCardSize>();
						this.TestBallSize4 = new List<string>();
						this.TextBoxVisible4 = false;
						this.NumberTestBalls4 = 0;
						this.INNumber4 = "n.a.";
					}
					this.OnPropertyChanged("SelectedMaterial4");
				}
			}
		}

		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x0600055E RID: 1374 RVA: 0x0002425C File Offset: 0x0002245C
		// (set) Token: 0x0600055F RID: 1375 RVA: 0x00024274 File Offset: 0x00022474
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
					this.dbConnection.UpdateCurrentProductTable(this.TestcardMaterialPath5, Convert.ToString(this.selectedMaterial5));
					bool flag2 = this.selectedMaterial5 != Material.None;
					if (flag2)
					{
						this.TestCardSize5 = this.InitTestCardSizeList(this.selectedMaterial5);
						this.SelectedTestCardSize5 = this.TestCardSize5.First<TestCardSize>();
						this.TestBallSize5 = this.InitializeTestBalls(this.selectedTestCardSize5, this.selectedMaterial5);
						this.SelectedTestBallSize5 = this.TestBallSize5.First<string>();
					}
					else
					{
						this.TestCardSize5 = new List<TestCardSize>();
						this.TestBallSize5 = new List<string>();
						this.TextBoxVisible5 = false;
						this.NumberTestBalls5 = 0;
						this.INNumber5 = "n.a.";
					}
					this.OnPropertyChanged("SelectedMaterial5");
				}
			}
		}

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x06000560 RID: 1376 RVA: 0x00024368 File Offset: 0x00022568
		// (set) Token: 0x06000561 RID: 1377 RVA: 0x00024380 File Offset: 0x00022580
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

		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x06000562 RID: 1378 RVA: 0x000243B4 File Offset: 0x000225B4
		// (set) Token: 0x06000563 RID: 1379 RVA: 0x000243CC File Offset: 0x000225CC
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

		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x06000564 RID: 1380 RVA: 0x00024400 File Offset: 0x00022600
		// (set) Token: 0x06000565 RID: 1381 RVA: 0x00024418 File Offset: 0x00022618
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

		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x06000566 RID: 1382 RVA: 0x0002444C File Offset: 0x0002264C
		// (set) Token: 0x06000567 RID: 1383 RVA: 0x00024464 File Offset: 0x00022664
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

		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x06000568 RID: 1384 RVA: 0x00024498 File Offset: 0x00022698
		// (set) Token: 0x06000569 RID: 1385 RVA: 0x000244B0 File Offset: 0x000226B0
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

		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x0600056A RID: 1386 RVA: 0x000244E4 File Offset: 0x000226E4
		// (set) Token: 0x0600056B RID: 1387 RVA: 0x000244FC File Offset: 0x000226FC
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
					this.dbConnection.UpdateCurrentProductTable(this.TestcardType1, Convert.ToString(this.selectedTestCardSize1));
					this.TestBallSize1 = this.InitializeTestBalls(this.selectedTestCardSize1, this.selectedMaterial1);
					this.SelectedTestBallSize1 = this.TestBallSize1.First<string>();
					bool flag2 = this.IsNotBulk;
					if (flag2)
					{
						bool flag3 = this.selectedTestCardSize1 != TestCardSize.Other && this.selectedMaterial1 != Material.Other;
						if (flag3)
						{
							this.TextBoxVisible1 = false;
							this.NumberTestBalls1 = this.calculateNumberTestBalls(this.selectedMaterial1, this.selectedTestCardSize1, this.selectedTestBallSize1, this.testBallSize1);
						}
						else
						{
							bool flag4 = this.selectedMaterial1 == Material.None;
							if (flag4)
							{
								this.TextBoxVisible1 = false;
								this.NumberTestBalls1 = 0;
							}
							else
							{
								bool flag5 = this.selectedMaterial1 == Material.Other && this.selectedTestCardSize1 != TestCardSize.Other;
								if (flag5)
								{
									bool flag6 = this.selectedTestCardSize1 == TestCardSize.Testcard100;
									if (flag6)
									{
										this.NumberTestBalls1 = 100;
									}
									else
									{
										bool flag7 = this.selectedTestCardSize1 == TestCardSize.SingleBall;
										if (flag7)
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
					}
					else
					{
						this.TextBoxVisible1 = false;
						this.NumberTestBalls1 = 1;
					}
					this.OnPropertyChanged("SelectedTestCardSize1");
				}
			}
		}

		// Token: 0x170001AA RID: 426
		// (get) Token: 0x0600056C RID: 1388 RVA: 0x00024688 File Offset: 0x00022888
		// (set) Token: 0x0600056D RID: 1389 RVA: 0x000246A0 File Offset: 0x000228A0
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
					this.dbConnection.UpdateCurrentProductTable(this.TestcardType2, Convert.ToString(this.selectedTestCardSize2));
					this.TestBallSize2 = this.InitializeTestBalls(this.selectedTestCardSize2, this.selectedMaterial2);
					this.SelectedTestBallSize2 = this.TestBallSize2.First<string>();
					bool flag2 = this.IsNotBulk;
					if (flag2)
					{
						bool flag3 = this.selectedTestCardSize2 != TestCardSize.Other && this.selectedMaterial2 != Material.Other;
						if (flag3)
						{
							this.TextBoxVisible2 = false;
							this.NumberTestBalls2 = this.calculateNumberTestBalls(this.selectedMaterial2, this.selectedTestCardSize2, this.selectedTestBallSize2, this.testBallSize2);
						}
						else
						{
							bool flag4 = this.selectedMaterial2 == Material.None;
							if (flag4)
							{
								this.TextBoxVisible2 = false;
								this.NumberTestBalls2 = 0;
							}
							else
							{
								bool flag5 = this.selectedMaterial2 == Material.Other && this.selectedTestCardSize2 != TestCardSize.Other;
								if (flag5)
								{
									bool flag6 = this.selectedTestCardSize2 == TestCardSize.Testcard100;
									if (flag6)
									{
										this.NumberTestBalls2 = 100;
									}
									else
									{
										bool flag7 = this.selectedTestCardSize2 == TestCardSize.SingleBall;
										if (flag7)
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
					}
					else
					{
						this.TextBoxVisible2 = false;
						this.NumberTestBalls2 = 1;
					}
					this.OnPropertyChanged("SelectedTestCardSize2");
				}
			}
		}

		// Token: 0x170001AB RID: 427
		// (get) Token: 0x0600056E RID: 1390 RVA: 0x0002482C File Offset: 0x00022A2C
		// (set) Token: 0x0600056F RID: 1391 RVA: 0x00024844 File Offset: 0x00022A44
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
					this.dbConnection.UpdateCurrentProductTable(this.TestcardType3, Convert.ToString(this.selectedTestCardSize3));
					this.TestBallSize3 = this.InitializeTestBalls(this.selectedTestCardSize3, this.selectedMaterial3);
					this.SelectedTestBallSize3 = this.TestBallSize3.First<string>();
					bool flag2 = this.IsNotBulk;
					if (flag2)
					{
						bool flag3 = this.selectedTestCardSize3 != TestCardSize.Other && this.selectedMaterial3 != Material.Other;
						if (flag3)
						{
							this.TextBoxVisible3 = false;
							this.NumberTestBalls3 = this.calculateNumberTestBalls(this.selectedMaterial3, this.selectedTestCardSize3, this.selectedTestBallSize3, this.testBallSize3);
						}
						else
						{
							bool flag4 = this.selectedMaterial3 == Material.None;
							if (flag4)
							{
								this.TextBoxVisible3 = false;
								this.NumberTestBalls3 = 0;
							}
							else
							{
								bool flag5 = this.selectedMaterial3 == Material.Other && this.selectedTestCardSize3 != TestCardSize.Other;
								if (flag5)
								{
									bool flag6 = this.selectedTestCardSize3 == TestCardSize.Testcard100;
									if (flag6)
									{
										this.NumberTestBalls3 = 100;
									}
									else
									{
										bool flag7 = this.selectedTestCardSize3 == TestCardSize.SingleBall;
										if (flag7)
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
					}
					else
					{
						this.TextBoxVisible3 = false;
						this.NumberTestBalls3 = 1;
					}
					this.OnPropertyChanged("SelectedTestCardSize3");
				}
			}
		}

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x06000570 RID: 1392 RVA: 0x000249D0 File Offset: 0x00022BD0
		// (set) Token: 0x06000571 RID: 1393 RVA: 0x000249E8 File Offset: 0x00022BE8
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
					this.dbConnection.UpdateCurrentProductTable(this.TestcardType4, Convert.ToString(this.selectedTestCardSize4));
					this.TestBallSize4 = this.InitializeTestBalls(this.selectedTestCardSize4, this.selectedMaterial4);
					this.SelectedTestBallSize4 = this.TestBallSize4.First<string>();
					bool flag2 = this.IsNotBulk;
					if (flag2)
					{
						bool flag3 = this.selectedTestCardSize4 != TestCardSize.Other && this.selectedMaterial4 != Material.Other;
						if (flag3)
						{
							this.TextBoxVisible4 = false;
							this.NumberTestBalls4 = this.calculateNumberTestBalls(this.selectedMaterial4, this.selectedTestCardSize4, this.selectedTestBallSize4, this.testBallSize4);
						}
						else
						{
							bool flag4 = this.selectedMaterial4 == Material.None;
							if (flag4)
							{
								this.TextBoxVisible4 = false;
								this.NumberTestBalls4 = 0;
							}
							else
							{
								bool flag5 = this.selectedMaterial4 == Material.Other && this.selectedTestCardSize4 != TestCardSize.Other;
								if (flag5)
								{
									bool flag6 = this.selectedTestCardSize4 == TestCardSize.Testcard100;
									if (flag6)
									{
										this.NumberTestBalls4 = 100;
									}
									else
									{
										bool flag7 = this.selectedTestCardSize4 == TestCardSize.SingleBall;
										if (flag7)
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
					}
					else
					{
						this.TextBoxVisible4 = false;
						this.NumberTestBalls4 = 1;
					}
					this.OnPropertyChanged("SelectedTestCardSize4");
				}
			}
		}

		// Token: 0x170001AD RID: 429
		// (get) Token: 0x06000572 RID: 1394 RVA: 0x00024B74 File Offset: 0x00022D74
		// (set) Token: 0x06000573 RID: 1395 RVA: 0x00024B8C File Offset: 0x00022D8C
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
					this.dbConnection.UpdateCurrentProductTable(this.TestcardType5, Convert.ToString(this.selectedTestCardSize5));
					this.TestBallSize5 = this.InitializeTestBalls(this.selectedTestCardSize5, this.selectedMaterial5);
					this.SelectedTestBallSize5 = this.TestBallSize5.First<string>();
					bool flag2 = this.IsNotBulk;
					if (flag2)
					{
						bool flag3 = this.selectedTestCardSize5 != TestCardSize.Other && this.selectedMaterial5 != Material.Other;
						if (flag3)
						{
							this.TextBoxVisible5 = false;
							this.NumberTestBalls5 = this.calculateNumberTestBalls(this.selectedMaterial5, this.selectedTestCardSize5, this.selectedTestBallSize5, this.testBallSize5);
						}
						else
						{
							bool flag4 = this.selectedMaterial5 == Material.None;
							if (flag4)
							{
								this.TextBoxVisible5 = false;
								this.NumberTestBalls5 = 0;
							}
							else
							{
								bool flag5 = this.selectedMaterial5 == Material.Other && this.selectedTestCardSize5 != TestCardSize.Other;
								if (flag5)
								{
									bool flag6 = this.selectedTestCardSize5 == TestCardSize.Testcard100;
									if (flag6)
									{
										this.NumberTestBalls5 = 100;
									}
									else
									{
										bool flag7 = this.selectedTestCardSize5 == TestCardSize.SingleBall;
										if (flag7)
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
					}
					else
					{
						this.TextBoxVisible5 = false;
						this.NumberTestBalls5 = 1;
					}
					this.OnPropertyChanged("SelectedTestCardSize5");
				}
			}
		}

		// Token: 0x170001AE RID: 430
		// (get) Token: 0x06000574 RID: 1396 RVA: 0x00024D18 File Offset: 0x00022F18
		// (set) Token: 0x06000575 RID: 1397 RVA: 0x00024D30 File Offset: 0x00022F30
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

		// Token: 0x170001AF RID: 431
		// (get) Token: 0x06000576 RID: 1398 RVA: 0x00024D64 File Offset: 0x00022F64
		// (set) Token: 0x06000577 RID: 1399 RVA: 0x00024D7C File Offset: 0x00022F7C
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

		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x06000578 RID: 1400 RVA: 0x00024DB0 File Offset: 0x00022FB0
		// (set) Token: 0x06000579 RID: 1401 RVA: 0x00024DC8 File Offset: 0x00022FC8
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

		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x0600057A RID: 1402 RVA: 0x00024DFC File Offset: 0x00022FFC
		// (set) Token: 0x0600057B RID: 1403 RVA: 0x00024E14 File Offset: 0x00023014
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

		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x0600057C RID: 1404 RVA: 0x00024E48 File Offset: 0x00023048
		// (set) Token: 0x0600057D RID: 1405 RVA: 0x00024E60 File Offset: 0x00023060
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

		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x0600057E RID: 1406 RVA: 0x00024E94 File Offset: 0x00023094
		// (set) Token: 0x0600057F RID: 1407 RVA: 0x00024EAC File Offset: 0x000230AC
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
					this.dbConnection.UpdateCurrentProductTable(this.TestcardSize1, Convert.ToString(this.selectedTestBallSize1));
					this.NumberTestBalls1 = this.calculateNumberTestBalls(this.selectedMaterial1, this.selectedTestCardSize1, this.selectedTestBallSize1, this.testBallSize1);
					this.OnPropertyChanged("SelectedTestBallSize1");
				}
			}
		}

		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x06000580 RID: 1408 RVA: 0x00024F24 File Offset: 0x00023124
		// (set) Token: 0x06000581 RID: 1409 RVA: 0x00024F3C File Offset: 0x0002313C
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
					this.dbConnection.UpdateCurrentProductTable(this.TestcardSize2, Convert.ToString(this.selectedTestBallSize2));
					this.NumberTestBalls2 = this.calculateNumberTestBalls(this.selectedMaterial2, this.selectedTestCardSize2, this.selectedTestBallSize2, this.testBallSize2);
					this.OnPropertyChanged("SelectedTestBallSize2");
				}
			}
		}

		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x06000582 RID: 1410 RVA: 0x00024FB4 File Offset: 0x000231B4
		// (set) Token: 0x06000583 RID: 1411 RVA: 0x00024FCC File Offset: 0x000231CC
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
					this.dbConnection.UpdateCurrentProductTable(this.TestcardSize3, Convert.ToString(this.selectedTestBallSize3));
					this.NumberTestBalls3 = this.calculateNumberTestBalls(this.selectedMaterial3, this.selectedTestCardSize3, this.selectedTestBallSize3, this.testBallSize3);
					this.OnPropertyChanged("SelectedTestBallSize3");
				}
			}
		}

		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x06000584 RID: 1412 RVA: 0x00025044 File Offset: 0x00023244
		// (set) Token: 0x06000585 RID: 1413 RVA: 0x0002505C File Offset: 0x0002325C
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
					this.dbConnection.UpdateCurrentProductTable(this.TestcardSize4, Convert.ToString(this.selectedTestBallSize4));
					this.NumberTestBalls4 = this.calculateNumberTestBalls(this.selectedMaterial4, this.selectedTestCardSize4, this.selectedTestBallSize4, this.testBallSize4);
					this.OnPropertyChanged("SelectedTestBallSize4");
				}
			}
		}

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x06000586 RID: 1414 RVA: 0x000250D4 File Offset: 0x000232D4
		// (set) Token: 0x06000587 RID: 1415 RVA: 0x000250EC File Offset: 0x000232EC
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
					this.dbConnection.UpdateCurrentProductTable(this.TestcardSize5, Convert.ToString(this.selectedTestBallSize5));
					this.NumberTestBalls5 = this.calculateNumberTestBalls(this.selectedMaterial5, this.selectedTestCardSize5, this.selectedTestBallSize5, this.testBallSize5);
					this.OnPropertyChanged("SelectedTestBallSize5");
				}
			}
		}

		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x06000588 RID: 1416 RVA: 0x00025164 File Offset: 0x00023364
		// (set) Token: 0x06000589 RID: 1417 RVA: 0x0002517C File Offset: 0x0002337C
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
					this.dbConnection.UpdateCurrentProductTable(this.TestcardNumberCont1, Convert.ToString(this.numberTestBalls1));
					this.OnPropertyChanged("NumberTestBalls1");
				}
			}
		}

		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x0600058A RID: 1418 RVA: 0x000251CC File Offset: 0x000233CC
		// (set) Token: 0x0600058B RID: 1419 RVA: 0x000251E4 File Offset: 0x000233E4
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
					this.dbConnection.UpdateCurrentProductTable(this.TestcardNumberCont2, Convert.ToString(this.numberTestBalls2));
					this.OnPropertyChanged("NumberTestBalls2");
				}
			}
		}

		// Token: 0x170001BA RID: 442
		// (get) Token: 0x0600058C RID: 1420 RVA: 0x00025234 File Offset: 0x00023434
		// (set) Token: 0x0600058D RID: 1421 RVA: 0x0002524C File Offset: 0x0002344C
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
					this.dbConnection.UpdateCurrentProductTable(this.TestcardNumberCont3, Convert.ToString(this.numberTestBalls3));
					this.OnPropertyChanged("NumberTestBalls3");
				}
			}
		}

		// Token: 0x170001BB RID: 443
		// (get) Token: 0x0600058E RID: 1422 RVA: 0x0002529C File Offset: 0x0002349C
		// (set) Token: 0x0600058F RID: 1423 RVA: 0x000252B4 File Offset: 0x000234B4
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
					this.dbConnection.UpdateCurrentProductTable(this.TestcardNumberCont4, Convert.ToString(this.numberTestBalls4));
					this.OnPropertyChanged("NumberTestBalls4");
				}
			}
		}

		// Token: 0x170001BC RID: 444
		// (get) Token: 0x06000590 RID: 1424 RVA: 0x00025304 File Offset: 0x00023504
		// (set) Token: 0x06000591 RID: 1425 RVA: 0x0002531C File Offset: 0x0002351C
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
					this.dbConnection.UpdateCurrentProductTable(this.TestcardNumberCont5, Convert.ToString(this.numberTestBalls5));
					this.OnPropertyChanged("NumberTestBalls5");
				}
			}
		}

		// Token: 0x170001BD RID: 445
		// (get) Token: 0x06000592 RID: 1426 RVA: 0x0002536C File Offset: 0x0002356C
		// (set) Token: 0x06000593 RID: 1427 RVA: 0x00025384 File Offset: 0x00023584
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

		// Token: 0x170001BE RID: 446
		// (get) Token: 0x06000594 RID: 1428 RVA: 0x000253B8 File Offset: 0x000235B8
		// (set) Token: 0x06000595 RID: 1429 RVA: 0x000253D0 File Offset: 0x000235D0
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

		// Token: 0x170001BF RID: 447
		// (get) Token: 0x06000596 RID: 1430 RVA: 0x00025404 File Offset: 0x00023604
		// (set) Token: 0x06000597 RID: 1431 RVA: 0x0002541C File Offset: 0x0002361C
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

		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x06000598 RID: 1432 RVA: 0x00025450 File Offset: 0x00023650
		// (set) Token: 0x06000599 RID: 1433 RVA: 0x00025468 File Offset: 0x00023668
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

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x0600059A RID: 1434 RVA: 0x0002549C File Offset: 0x0002369C
		// (set) Token: 0x0600059B RID: 1435 RVA: 0x000254B4 File Offset: 0x000236B4
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

		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x0600059C RID: 1436 RVA: 0x000254E8 File Offset: 0x000236E8
		// (set) Token: 0x0600059D RID: 1437 RVA: 0x00025500 File Offset: 0x00023700
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
					this.dbConnection.UpdateCurrentProductTable(this.TestcardNumber1, Convert.ToString(this.iNNumber1));
					this.OnPropertyChanged("INNumber1");
				}
			}
		}

		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x0600059E RID: 1438 RVA: 0x00025550 File Offset: 0x00023750
		// (set) Token: 0x0600059F RID: 1439 RVA: 0x00025568 File Offset: 0x00023768
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
					this.dbConnection.UpdateCurrentProductTable(this.TestcardNumber2, Convert.ToString(this.iNNumber2));
					this.OnPropertyChanged("INNumber2");
				}
			}
		}

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x060005A0 RID: 1440 RVA: 0x000255B8 File Offset: 0x000237B8
		// (set) Token: 0x060005A1 RID: 1441 RVA: 0x000255D0 File Offset: 0x000237D0
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
					this.dbConnection.UpdateCurrentProductTable(this.TestcardNumber3, Convert.ToString(this.iNNumber3));
					this.OnPropertyChanged("INNumber3");
				}
			}
		}

		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x060005A2 RID: 1442 RVA: 0x00025620 File Offset: 0x00023820
		// (set) Token: 0x060005A3 RID: 1443 RVA: 0x00025638 File Offset: 0x00023838
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
					this.dbConnection.UpdateCurrentProductTable(this.TestcardNumber4, Convert.ToString(this.iNNumber4));
					this.OnPropertyChanged("INNumber4");
				}
			}
		}

		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x060005A4 RID: 1444 RVA: 0x00025688 File Offset: 0x00023888
		// (set) Token: 0x060005A5 RID: 1445 RVA: 0x000256A0 File Offset: 0x000238A0
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
					this.dbConnection.UpdateCurrentProductTable(this.TestcardNumber5, Convert.ToString(this.iNNumber5));
					this.OnPropertyChanged("INNumber5");
				}
			}
		}

		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x060005A6 RID: 1446 RVA: 0x000256F0 File Offset: 0x000238F0
		// (set) Token: 0x060005A7 RID: 1447 RVA: 0x00025708 File Offset: 0x00023908
		public bool IsNotBulk
		{
			get
			{
				return this.isNotBulk;
			}
			set
			{
				this.isNotBulk = value;
				this.OnPropertyChanged("IsNotBulk");
			}
		}

		// Token: 0x060005A8 RID: 1448 RVA: 0x00025720 File Offset: 0x00023920
		public AuditCheckTestsamplesViewModel(AccessRightsManager accessManager, DBConnection dbConnection)
		{
			this.accessManager = accessManager;
			this.dbConnection = dbConnection;
			MachineMode machineMode = MachineMode.Standard;
			dbConnection.GetMachineMode(ref machineMode);
			bool flag = machineMode != MachineMode.Bulk;
			if (flag)
			{
				this.IsNotBulk = true;
			}
			else
			{
				this.IsNotBulk = false;
			}
			this.Initialize();
			this.RegisterLocalizedText();
			this.CleanUpCommand = new RelayCommand(new Action<object>(this.CleanUp));
		}

		// Token: 0x060005A9 RID: 1449 RVA: 0x000258CE File Offset: 0x00023ACE
		public void Initialize()
		{
			this.InitializeMaterialList();
			this.InitializeSelectedParams();
		}

		// Token: 0x060005AA RID: 1450 RVA: 0x000258E0 File Offset: 0x00023AE0
		public void InitializeSelectedParams()
		{
			string text = "";
			this.dbConnection.GetParameterValue(this.TestcardMaterialPath1, ref text);
			bool flag = this.convertStringToTestcardMaterial(text) != Material.None;
			if (flag)
			{
				string[] array = new string[5];
				array[0] = text;
				this.dbConnection.GetParameterValue(this.TestcardType1, ref text);
				array[1] = text;
				this.dbConnection.GetParameterValue(this.TestcardSize1, ref text);
				array[2] = text;
				this.dbConnection.GetParameterValue(this.TestcardNumberCont1, ref text);
				array[3] = text;
				this.dbConnection.GetParameterValue(this.TestcardNumber1, ref text);
				array[4] = text;
				this.SelectedMaterial1 = this.convertStringToTestcardMaterial(array[0]);
				this.SelectedTestCardSize1 = this.convertStringToTestcardType(array[1]);
				this.SelectedTestBallSize1 = array[2];
				this.NumberTestBalls1 = Convert.ToInt32(array[3]);
				this.INNumber1 = array[4];
			}
			else
			{
				this.SelectedMaterial1 = this.convertStringToTestcardMaterial(text);
			}
			this.dbConnection.GetParameterValue(this.TestcardMaterialPath2, ref text);
			bool flag2 = this.convertStringToTestcardMaterial(text) != Material.None;
			if (flag2)
			{
				string[] array2 = new string[5];
				array2[0] = text;
				this.dbConnection.GetParameterValue(this.TestcardType2, ref text);
				array2[1] = text;
				this.dbConnection.GetParameterValue(this.TestcardSize2, ref text);
				array2[2] = text;
				this.dbConnection.GetParameterValue(this.TestcardNumberCont2, ref text);
				array2[3] = text;
				this.dbConnection.GetParameterValue(this.TestcardNumber2, ref text);
				array2[4] = text;
				this.SelectedMaterial2 = this.convertStringToTestcardMaterial(array2[0]);
				this.SelectedTestCardSize2 = this.convertStringToTestcardType(array2[1]);
				this.SelectedTestBallSize2 = array2[2];
				this.NumberTestBalls2 = Convert.ToInt32(array2[3]);
				this.INNumber2 = array2[4];
			}
			else
			{
				this.SelectedMaterial2 = this.convertStringToTestcardMaterial(text);
			}
			this.dbConnection.GetParameterValue(this.TestcardMaterialPath3, ref text);
			bool flag3 = this.convertStringToTestcardMaterial(text) != Material.None;
			if (flag3)
			{
				string[] array3 = new string[5];
				array3[0] = text;
				this.dbConnection.GetParameterValue(this.TestcardType3, ref text);
				array3[1] = text;
				this.dbConnection.GetParameterValue(this.TestcardSize3, ref text);
				array3[2] = text;
				this.dbConnection.GetParameterValue(this.TestcardNumberCont3, ref text);
				array3[3] = text;
				this.dbConnection.GetParameterValue(this.TestcardNumber3, ref text);
				array3[4] = text;
				this.SelectedMaterial3 = this.convertStringToTestcardMaterial(array3[0]);
				this.SelectedTestCardSize3 = this.convertStringToTestcardType(array3[1]);
				this.SelectedTestBallSize3 = array3[2];
				this.NumberTestBalls3 = Convert.ToInt32(array3[3]);
				this.INNumber3 = array3[4];
			}
			else
			{
				this.SelectedMaterial3 = this.convertStringToTestcardMaterial(text);
			}
			this.dbConnection.GetParameterValue(this.TestcardMaterialPath4, ref text);
			bool flag4 = this.convertStringToTestcardMaterial(text) != Material.None;
			if (flag4)
			{
				string[] array4 = new string[5];
				array4[0] = text;
				this.dbConnection.GetParameterValue(this.TestcardType4, ref text);
				array4[1] = text;
				this.dbConnection.GetParameterValue(this.TestcardSize4, ref text);
				array4[2] = text;
				this.dbConnection.GetParameterValue(this.TestcardNumberCont4, ref text);
				array4[3] = text;
				this.dbConnection.GetParameterValue(this.TestcardNumber4, ref text);
				array4[4] = text;
				this.SelectedMaterial4 = this.convertStringToTestcardMaterial(array4[0]);
				this.SelectedTestCardSize4 = this.convertStringToTestcardType(array4[1]);
				this.SelectedTestBallSize4 = array4[2];
				this.NumberTestBalls4 = Convert.ToInt32(array4[3]);
				this.INNumber4 = array4[4];
			}
			else
			{
				this.SelectedMaterial4 = this.convertStringToTestcardMaterial(text);
			}
			this.dbConnection.GetParameterValue(this.TestcardMaterialPath5, ref text);
			bool flag5 = this.convertStringToTestcardMaterial(text) != Material.None;
			if (flag5)
			{
				string[] array5 = new string[5];
				array5[0] = text;
				this.dbConnection.GetParameterValue(this.TestcardType5, ref text);
				array5[1] = text;
				this.dbConnection.GetParameterValue(this.TestcardSize5, ref text);
				array5[2] = text;
				this.dbConnection.GetParameterValue(this.TestcardNumberCont5, ref text);
				array5[3] = text;
				this.dbConnection.GetParameterValue(this.TestcardNumber5, ref text);
				array5[4] = text;
				this.SelectedMaterial5 = this.convertStringToTestcardMaterial(array5[0]);
				this.SelectedTestCardSize5 = this.convertStringToTestcardType(array5[1]);
				this.SelectedTestBallSize5 = array5[2];
				this.NumberTestBalls5 = Convert.ToInt32(array5[3]);
				this.INNumber5 = array5[4];
			}
			else
			{
				this.SelectedMaterial5 = this.convertStringToTestcardMaterial(text);
			}
		}

		// Token: 0x060005AB RID: 1451 RVA: 0x00025DB8 File Offset: 0x00023FB8
		public void ImportIVPSettings()
		{
			bool flag = Dialog.Show(LocalizationManager.Localize("txtInfo"), LocalizationManager.Localize("txtImportFromIVP"), DialogStyle.Info, DialogButtons.OKCancel) == Dialog.DialogResults.Ok;
			if (flag)
			{
				List<string> list = new List<string>();
				this.dbConnection.GetIVPTestcardRow(0, ref list);
				bool flag2 = list.Count >= 5;
				if (flag2)
				{
					this.SelectedMaterial1 = this.convertStringToTestcardMaterial(list[0]);
					this.SelectedTestCardSize1 = this.convertStringToTestcardType(list[1]);
					this.SelectedTestBallSize1 = list[2];
					this.NumberTestBalls1 = Convert.ToInt32(list[3]);
					this.INNumber1 = list[4];
				}
				list = new List<string>();
				this.dbConnection.GetIVPTestcardRow(1, ref list);
				bool flag3 = list.Count >= 5;
				if (flag3)
				{
					this.SelectedMaterial2 = this.convertStringToTestcardMaterial(list[0]);
					this.SelectedTestCardSize2 = this.convertStringToTestcardType(list[1]);
					this.SelectedTestBallSize2 = list[2];
					this.NumberTestBalls2 = Convert.ToInt32(list[3]);
					this.INNumber2 = list[4];
				}
				list = new List<string>();
				this.dbConnection.GetIVPTestcardRow(2, ref list);
				bool flag4 = list.Count >= 5;
				if (flag4)
				{
					this.SelectedMaterial3 = this.convertStringToTestcardMaterial(list[0]);
					this.SelectedTestCardSize3 = this.convertStringToTestcardType(list[1]);
					this.SelectedTestBallSize3 = list[2];
					this.NumberTestBalls3 = Convert.ToInt32(list[3]);
					this.INNumber3 = list[4];
				}
				list = new List<string>();
				this.dbConnection.GetIVPTestcardRow(3, ref list);
				bool flag5 = list.Count >= 5;
				if (flag5)
				{
					this.SelectedMaterial4 = this.convertStringToTestcardMaterial(list[0]);
					this.SelectedTestCardSize4 = this.convertStringToTestcardType(list[1]);
					this.SelectedTestBallSize4 = list[2];
					this.NumberTestBalls4 = Convert.ToInt32(list[3]);
					this.INNumber4 = list[4];
				}
				list = new List<string>();
				this.dbConnection.GetIVPTestcardRow(4, ref list);
				bool flag6 = list.Count >= 5;
				if (flag6)
				{
					this.SelectedMaterial5 = this.convertStringToTestcardMaterial(list[0]);
					this.SelectedTestCardSize5 = this.convertStringToTestcardType(list[1]);
					this.SelectedTestBallSize5 = list[2];
					this.NumberTestBalls5 = Convert.ToInt32(list[3]);
					this.INNumber5 = list[4];
				}
			}
		}

		// Token: 0x060005AC RID: 1452 RVA: 0x0002606C File Offset: 0x0002426C
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

		// Token: 0x060005AD RID: 1453 RVA: 0x000261BC File Offset: 0x000243BC
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

		// Token: 0x060005AE RID: 1454 RVA: 0x0002621C File Offset: 0x0002441C
		public List<TestCardSize> InitTestCardSizeList(Material material)
		{
			List<TestCardSize> list = new List<TestCardSize>();
			list.Clear();
			bool flag = this.IsNotBulk;
			if (flag)
			{
				bool flag2 = material == Material.VA || material == Material.Glass;
				if (flag2)
				{
					list.Add(TestCardSize.SingleBall);
					list.Add(TestCardSize.Testcard5);
					list.Add(TestCardSize.Testcard100);
				}
				else
				{
					bool flag3 = material == Material.Aluminium || material == Material.PTFE || material == Material.EPDM || material == Material.Ceramics || material == Material.VAWire || material == Material.FE || material == Material.NonFE;
					if (flag3)
					{
						list.Add(TestCardSize.Testcard5);
					}
					else
					{
						bool flag4 = material == Material.Bone;
						if (flag4)
						{
							list.Add(TestCardSize.SingleBall);
							list.Add(TestCardSize.Testcard5);
						}
						else
						{
							bool flag5 = material == Material.Other;
							if (flag5)
							{
								list.Add(TestCardSize.SingleBall);
								list.Add(TestCardSize.Testcard100);
							}
						}
					}
				}
			}
			else
			{
				list.Add(TestCardSize.SingleBall);
			}
			list.Add(TestCardSize.Other);
			return list;
		}

		// Token: 0x060005AF RID: 1455 RVA: 0x000262F8 File Offset: 0x000244F8
		public List<string> InitializeTestBalls(TestCardSize testCard, Material material)
		{
			List<string> list = new List<string>();
			bool flag = material == Material.VA;
			if (flag)
			{
				bool flag2 = this.IsNotBulk;
				if (flag2)
				{
					bool flag3 = testCard == TestCardSize.Testcard100;
					if (flag3)
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
						bool flag4 = testCard == TestCardSize.SingleBall || testCard == TestCardSize.Other;
						if (flag4)
						{
							list.Add("0.2");
						}
						list.Add("0.3");
						list.Add("0.4");
						list.Add("0.5");
						list.Add("0.6");
						bool flag5 = testCard == TestCardSize.Testcard5;
						if (flag5)
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
						bool flag6 = testCard == TestCardSize.Testcard5;
						if (flag6)
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
						bool flag7 = testCard > TestCardSize.SingleBall;
						if (flag7)
						{
							list.Add("3.5");
						}
					}
				}
				else
				{
					list.Add("0.4");
					list.Add("0.5");
					list.Add("0.6");
					list.Add("0.8");
					list.Add("1.0");
					list.Add("1.2");
					list.Add("1.5");
					list.Add("1.8");
					list.Add("2.0");
					list.Add("2.5");
				}
			}
			else
			{
				bool flag8 = material == Material.Glass;
				if (flag8)
				{
					bool flag9 = this.IsNotBulk;
					if (flag9)
					{
						bool flag10 = testCard == TestCardSize.SingleBall || testCard == TestCardSize.Other;
						if (flag10)
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
							bool flag11 = testCard == TestCardSize.Testcard5;
							if (flag11)
							{
								list.Add("0.8");
								list.Add("1.0");
								list.Add("1.2");
								list.Add("1.5");
								list.Add("2.0 (0.8-2)");
								list.Add("2.0 (2-4)");
								list.Add("2.5");
								list.Add("3.0");
								list.Add("3.5");
								list.Add("4.0 (2-4)");
								list.Add("4.0 (4-8)");
								list.Add("5.0");
								list.Add("6.0");
								list.Add("7.0");
								list.Add("8.0");
							}
							else
							{
								bool flag12 = testCard == TestCardSize.Testcard100;
								if (flag12)
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
						list.Add("1.5");
						list.Add("2.0");
						list.Add("2.5");
						list.Add("3.0");
						list.Add("3.5");
						list.Add("4.0");
					}
				}
				else
				{
					bool flag13 = material == Material.EPDM;
					if (flag13)
					{
						list.Add("6.0");
						list.Add("7.0");
						list.Add("8.0");
						list.Add("9.0");
						list.Add("10.0");
					}
					else
					{
						bool flag14 = material == Material.PTFE;
						if (flag14)
						{
							list.Add("3.0");
							list.Add("4.0");
							list.Add("5.0");
							list.Add("6.0");
							list.Add("7.0");
						}
						else
						{
							bool flag15 = material == Material.Ceramics || material == Material.Aluminium;
							if (flag15)
							{
								list.Add("2.0");
								list.Add("3.0");
								list.Add("4.0");
								list.Add("5.0");
								list.Add("6.0");
							}
							else
							{
								bool flag16 = material == Material.VAWire;
								if (flag16)
								{
									list.Add("0.15x2.0");
									list.Add("0.2x2.0");
									bool flag17 = testCard == TestCardSize.Testcard5;
									if (flag17)
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
									bool flag18 = material == Material.Bone;
									if (flag18)
									{
										bool flag19 = testCard == TestCardSize.SingleBall;
										if (flag19)
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
											bool flag20 = testCard == TestCardSize.Testcard5;
											if (flag20)
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
										bool flag21 = material == Material.Other;
										if (flag21)
										{
											list.Add("-");
										}
										else
										{
											bool flag22 = material == Material.FE || material == Material.NonFE;
											if (flag22)
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

		// Token: 0x060005B0 RID: 1456 RVA: 0x00026B30 File Offset: 0x00024D30
		private void InitializeMaterialList()
		{
			this.MaterialList = new List<Material>();
			this.MaterialList.Add(Material.Glass);
			this.MaterialList.Add(Material.VA);
			bool flag = this.IsNotBulk;
			if (flag)
			{
				this.MaterialList.Add(Material.VAWire);
				this.MaterialList.Add(Material.EPDM);
				this.MaterialList.Add(Material.Bone);
				this.MaterialList.Add(Material.PTFE);
				this.MaterialList.Add(Material.Aluminium);
				this.MaterialList.Add(Material.Ceramics);
				this.MaterialList.Add(Material.FE);
				this.MaterialList.Add(Material.NonFE);
			}
			this.MaterialList.Add(Material.Other);
			this.MaterialList.Add(Material.None);
		}

		// Token: 0x060005B1 RID: 1457 RVA: 0x00026BF8 File Offset: 0x00024DF8
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x060005B2 RID: 1458 RVA: 0x00026C28 File Offset: 0x00024E28
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

		// Token: 0x060005B3 RID: 1459 RVA: 0x00026D00 File Offset: 0x00024F00
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

		// Token: 0x060005B4 RID: 1460 RVA: 0x00026E08 File Offset: 0x00025008
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

		// Token: 0x060005B5 RID: 1461 RVA: 0x00026E78 File Offset: 0x00025078
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

		// Token: 0x060005B6 RID: 1462 RVA: 0x00026F28 File Offset: 0x00025128
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

		// Token: 0x060005B7 RID: 1463 RVA: 0x00026FD8 File Offset: 0x000251D8
		private void RegisterLocalizedText()
		{
			this.StrMaterial = new BoundLocalizedText("txtMaterial");
			this.StrBallSize = new BoundLocalizedText("txtSize");
			this.StrSettings = new BoundLocalizedText("txtSettings");
			this.StrNumberOfBalls = new BoundLocalizedText("txtNumberOfBalls");
			this.StrTestcardSize = new BoundLocalizedText("txtTestcardSize");
			this.StrINNumber = new BoundLocalizedText("txtINNumber");
		}

		// Token: 0x060005B8 RID: 1464 RVA: 0x0002704C File Offset: 0x0002524C
		public void DeregisterLocalizedText()
		{
			MainWindow.localizationManager.Deregister(this.StrMaterial);
			MainWindow.localizationManager.Deregister(this.StrBallSize);
			MainWindow.localizationManager.Deregister(this.StrSettings);
			MainWindow.localizationManager.Deregister(this.StrNumberOfBalls);
			MainWindow.localizationManager.Deregister(this.StrTestcardSize);
			MainWindow.localizationManager.Deregister(this.StrINNumber);
		}

		// Token: 0x060005B9 RID: 1465 RVA: 0x000270C0 File Offset: 0x000252C0
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

		// Token: 0x040003DF RID: 991
		private DBConnection dbConnection;

		// Token: 0x040003E0 RID: 992
		private NumKeyboard numKeyboard;

		// Token: 0x040003E1 RID: 993
		private Keyboard keyboard;

		// Token: 0x040003E8 RID: 1000
		private AccessRightsManager accessManager;

		// Token: 0x040003EA RID: 1002
		private readonly string TestcardMaterialPath1 = "TestcardMaterial1";

		// Token: 0x040003EB RID: 1003
		private readonly string TestcardType1 = "TestcardType1";

		// Token: 0x040003EC RID: 1004
		private readonly string TestcardSize1 = "TestcardSize1";

		// Token: 0x040003ED RID: 1005
		private readonly string TestcardNumberCont1 = "TestcardNumberCont1";

		// Token: 0x040003EE RID: 1006
		private readonly string TestcardNumber1 = "TestcardNumber1";

		// Token: 0x040003EF RID: 1007
		private readonly string TestcardMaterialPath2 = "TestcardMaterial2";

		// Token: 0x040003F0 RID: 1008
		private readonly string TestcardType2 = "TestcardType2";

		// Token: 0x040003F1 RID: 1009
		private readonly string TestcardSize2 = "TestcardSize2";

		// Token: 0x040003F2 RID: 1010
		private readonly string TestcardNumberCont2 = "TestcardNumberCont2";

		// Token: 0x040003F3 RID: 1011
		private readonly string TestcardNumber2 = "TestcardNumber2";

		// Token: 0x040003F4 RID: 1012
		private readonly string TestcardMaterialPath3 = "TestcardMaterial3";

		// Token: 0x040003F5 RID: 1013
		private readonly string TestcardType3 = "TestcardType3";

		// Token: 0x040003F6 RID: 1014
		private readonly string TestcardSize3 = "TestcardSize3";

		// Token: 0x040003F7 RID: 1015
		private readonly string TestcardNumberCont3 = "TestcardNumberCont3";

		// Token: 0x040003F8 RID: 1016
		private readonly string TestcardNumber3 = "TestcardNumber3";

		// Token: 0x040003F9 RID: 1017
		private readonly string TestcardMaterialPath4 = "TestcardMaterial4";

		// Token: 0x040003FA RID: 1018
		private readonly string TestcardType4 = "TestcardType4";

		// Token: 0x040003FB RID: 1019
		private readonly string TestcardSize4 = "TestcardSize4";

		// Token: 0x040003FC RID: 1020
		private readonly string TestcardNumberCont4 = "TestcardNumberCont4";

		// Token: 0x040003FD RID: 1021
		private readonly string TestcardNumber4 = "TestcardNumber4";

		// Token: 0x040003FE RID: 1022
		private readonly string TestcardMaterialPath5 = "TestcardMaterial5";

		// Token: 0x040003FF RID: 1023
		private readonly string TestcardType5 = "TestcardType5";

		// Token: 0x04000400 RID: 1024
		private readonly string TestcardSize5 = "TestcardSize5";

		// Token: 0x04000401 RID: 1025
		private readonly string TestcardNumberCont5 = "TestcardNumberCont5";

		// Token: 0x04000402 RID: 1026
		private readonly string TestcardNumber5 = "TestcardNumber5";

		// Token: 0x04000404 RID: 1028
		private Material selectedMaterial1 = Material.None;

		// Token: 0x04000405 RID: 1029
		private Material selectedMaterial2 = Material.None;

		// Token: 0x04000406 RID: 1030
		private Material selectedMaterial3 = Material.None;

		// Token: 0x04000407 RID: 1031
		private Material selectedMaterial4 = Material.None;

		// Token: 0x04000408 RID: 1032
		private Material selectedMaterial5 = Material.None;

		// Token: 0x04000409 RID: 1033
		public List<TestCardSize> testCardSize1;

		// Token: 0x0400040A RID: 1034
		public List<TestCardSize> testCardSize2;

		// Token: 0x0400040B RID: 1035
		public List<TestCardSize> testCardSize3;

		// Token: 0x0400040C RID: 1036
		public List<TestCardSize> testCardSize4;

		// Token: 0x0400040D RID: 1037
		public List<TestCardSize> testCardSize5;

		// Token: 0x0400040E RID: 1038
		private TestCardSize selectedTestCardSize1;

		// Token: 0x0400040F RID: 1039
		private TestCardSize selectedTestCardSize2;

		// Token: 0x04000410 RID: 1040
		private TestCardSize selectedTestCardSize3;

		// Token: 0x04000411 RID: 1041
		private TestCardSize selectedTestCardSize4;

		// Token: 0x04000412 RID: 1042
		private TestCardSize selectedTestCardSize5;

		// Token: 0x04000413 RID: 1043
		private List<string> testBallSize1;

		// Token: 0x04000414 RID: 1044
		private List<string> testBallSize2;

		// Token: 0x04000415 RID: 1045
		private List<string> testBallSize3;

		// Token: 0x04000416 RID: 1046
		private List<string> testBallSize4;

		// Token: 0x04000417 RID: 1047
		private List<string> testBallSize5;

		// Token: 0x04000418 RID: 1048
		private string selectedTestBallSize1;

		// Token: 0x04000419 RID: 1049
		private string selectedTestBallSize2;

		// Token: 0x0400041A RID: 1050
		private string selectedTestBallSize3;

		// Token: 0x0400041B RID: 1051
		private string selectedTestBallSize4;

		// Token: 0x0400041C RID: 1052
		private string selectedTestBallSize5;

		// Token: 0x0400041D RID: 1053
		private int numberTestBalls1;

		// Token: 0x0400041E RID: 1054
		private int numberTestBalls2;

		// Token: 0x0400041F RID: 1055
		private int numberTestBalls3;

		// Token: 0x04000420 RID: 1056
		private int numberTestBalls4;

		// Token: 0x04000421 RID: 1057
		private int numberTestBalls5;

		// Token: 0x04000422 RID: 1058
		private bool textboxVisible1;

		// Token: 0x04000423 RID: 1059
		private bool textboxVisible2;

		// Token: 0x04000424 RID: 1060
		private bool textboxVisible3;

		// Token: 0x04000425 RID: 1061
		private bool textboxVisible4;

		// Token: 0x04000426 RID: 1062
		private bool textboxVisible5;

		// Token: 0x04000427 RID: 1063
		private string iNNumber1;

		// Token: 0x04000428 RID: 1064
		private string iNNumber2;

		// Token: 0x04000429 RID: 1065
		private string iNNumber3;

		// Token: 0x0400042A RID: 1066
		private string iNNumber4;

		// Token: 0x0400042B RID: 1067
		private string iNNumber5;

		// Token: 0x0400042C RID: 1068
		private bool isNotBulk = false;
	}
}
