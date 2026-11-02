using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Common;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x020000BE RID: 190
	public class KeyboardViewModel : ViewModelBase
	{
		// Token: 0x17000594 RID: 1428
		// (get) Token: 0x06001284 RID: 4740 RVA: 0x0006F7D4 File Offset: 0x0006D9D4
		// (set) Token: 0x06001285 RID: 4741 RVA: 0x0006F7DC File Offset: 0x0006D9DC
		public WindowState WState { get; set; }

		// Token: 0x17000595 RID: 1429
		// (get) Token: 0x06001286 RID: 4742 RVA: 0x0006F7E8 File Offset: 0x0006D9E8
		// (set) Token: 0x06001287 RID: 4743 RVA: 0x0003C95F File Offset: 0x0003AB5F
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

		// Token: 0x17000596 RID: 1430
		// (get) Token: 0x06001288 RID: 4744 RVA: 0x0006F800 File Offset: 0x0006DA00
		// (set) Token: 0x06001289 RID: 4745 RVA: 0x0003C97F File Offset: 0x0003AB7F
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

		// Token: 0x17000597 RID: 1431
		// (get) Token: 0x0600128A RID: 4746 RVA: 0x0006F818 File Offset: 0x0006DA18
		// (set) Token: 0x0600128B RID: 4747 RVA: 0x0003C99F File Offset: 0x0003AB9F
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

		// Token: 0x17000598 RID: 1432
		// (get) Token: 0x0600128C RID: 4748 RVA: 0x0006F830 File Offset: 0x0006DA30
		// (set) Token: 0x0600128D RID: 4749 RVA: 0x0003C9BF File Offset: 0x0003ABBF
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

		// Token: 0x17000599 RID: 1433
		// (get) Token: 0x0600128E RID: 4750 RVA: 0x0006F848 File Offset: 0x0006DA48
		// (set) Token: 0x0600128F RID: 4751 RVA: 0x0006F860 File Offset: 0x0006DA60
		public bool CancelVisibility
		{
			get
			{
				return this.cancelVisibility;
			}
			set
			{
				this.cancelVisibility = value;
				this.OnPropertyChanged("CancelVisibility");
			}
		}

		// Token: 0x1700059A RID: 1434
		// (get) Token: 0x06001290 RID: 4752 RVA: 0x0006F878 File Offset: 0x0006DA78
		// (set) Token: 0x06001291 RID: 4753 RVA: 0x0006F890 File Offset: 0x0006DA90
		public bool EmailVisibility
		{
			get
			{
				return this.emailVisibility;
			}
			set
			{
				this.emailVisibility = value;
				this.OnPropertyChanged("EmailVisibility");
			}
		}

		// Token: 0x1700059B RID: 1435
		// (get) Token: 0x06001292 RID: 4754 RVA: 0x0006F8A8 File Offset: 0x0006DAA8
		// (set) Token: 0x06001293 RID: 4755 RVA: 0x0006F8C0 File Offset: 0x0006DAC0
		public bool NetworkVisibility
		{
			get
			{
				return this.networkVisibility;
			}
			set
			{
				this.networkVisibility = value;
				this.OnPropertyChanged("NetworkVisibility");
			}
		}

		// Token: 0x1700059C RID: 1436
		// (get) Token: 0x06001294 RID: 4756 RVA: 0x0006F8D6 File Offset: 0x0006DAD6
		// (set) Token: 0x06001295 RID: 4757 RVA: 0x0006F8DE File Offset: 0x0006DADE
		public KeyboardChar Key1 { get; set; }

		// Token: 0x1700059D RID: 1437
		// (get) Token: 0x06001296 RID: 4758 RVA: 0x0006F8E7 File Offset: 0x0006DAE7
		// (set) Token: 0x06001297 RID: 4759 RVA: 0x0006F8EF File Offset: 0x0006DAEF
		public KeyboardChar Key2 { get; set; }

		// Token: 0x1700059E RID: 1438
		// (get) Token: 0x06001298 RID: 4760 RVA: 0x0006F8F8 File Offset: 0x0006DAF8
		// (set) Token: 0x06001299 RID: 4761 RVA: 0x0006F900 File Offset: 0x0006DB00
		public KeyboardChar Key3 { get; set; }

		// Token: 0x1700059F RID: 1439
		// (get) Token: 0x0600129A RID: 4762 RVA: 0x0006F909 File Offset: 0x0006DB09
		// (set) Token: 0x0600129B RID: 4763 RVA: 0x0006F911 File Offset: 0x0006DB11
		public KeyboardChar Key4 { get; set; }

		// Token: 0x170005A0 RID: 1440
		// (get) Token: 0x0600129C RID: 4764 RVA: 0x0006F91A File Offset: 0x0006DB1A
		// (set) Token: 0x0600129D RID: 4765 RVA: 0x0006F922 File Offset: 0x0006DB22
		public KeyboardChar Key5 { get; set; }

		// Token: 0x170005A1 RID: 1441
		// (get) Token: 0x0600129E RID: 4766 RVA: 0x0006F92B File Offset: 0x0006DB2B
		// (set) Token: 0x0600129F RID: 4767 RVA: 0x0006F933 File Offset: 0x0006DB33
		public KeyboardChar Key6 { get; set; }

		// Token: 0x170005A2 RID: 1442
		// (get) Token: 0x060012A0 RID: 4768 RVA: 0x0006F93C File Offset: 0x0006DB3C
		// (set) Token: 0x060012A1 RID: 4769 RVA: 0x0006F944 File Offset: 0x0006DB44
		public KeyboardChar Key7 { get; set; }

		// Token: 0x170005A3 RID: 1443
		// (get) Token: 0x060012A2 RID: 4770 RVA: 0x0006F94D File Offset: 0x0006DB4D
		// (set) Token: 0x060012A3 RID: 4771 RVA: 0x0006F955 File Offset: 0x0006DB55
		public KeyboardChar Key8 { get; set; }

		// Token: 0x170005A4 RID: 1444
		// (get) Token: 0x060012A4 RID: 4772 RVA: 0x0006F95E File Offset: 0x0006DB5E
		// (set) Token: 0x060012A5 RID: 4773 RVA: 0x0006F966 File Offset: 0x0006DB66
		public KeyboardChar Key9 { get; set; }

		// Token: 0x170005A5 RID: 1445
		// (get) Token: 0x060012A6 RID: 4774 RVA: 0x0006F96F File Offset: 0x0006DB6F
		// (set) Token: 0x060012A7 RID: 4775 RVA: 0x0006F977 File Offset: 0x0006DB77
		public KeyboardChar Key0 { get; set; }

		// Token: 0x170005A6 RID: 1446
		// (get) Token: 0x060012A8 RID: 4776 RVA: 0x0006F980 File Offset: 0x0006DB80
		// (set) Token: 0x060012A9 RID: 4777 RVA: 0x0006F988 File Offset: 0x0006DB88
		public KeyboardChar KeyQ { get; set; }

		// Token: 0x170005A7 RID: 1447
		// (get) Token: 0x060012AA RID: 4778 RVA: 0x0006F991 File Offset: 0x0006DB91
		// (set) Token: 0x060012AB RID: 4779 RVA: 0x0006F999 File Offset: 0x0006DB99
		public KeyboardChar KeyW { get; set; }

		// Token: 0x170005A8 RID: 1448
		// (get) Token: 0x060012AC RID: 4780 RVA: 0x0006F9A2 File Offset: 0x0006DBA2
		// (set) Token: 0x060012AD RID: 4781 RVA: 0x0006F9AA File Offset: 0x0006DBAA
		public KeyboardChar KeyE { get; set; }

		// Token: 0x170005A9 RID: 1449
		// (get) Token: 0x060012AE RID: 4782 RVA: 0x0006F9B3 File Offset: 0x0006DBB3
		// (set) Token: 0x060012AF RID: 4783 RVA: 0x0006F9BB File Offset: 0x0006DBBB
		public KeyboardChar KeyR { get; set; }

		// Token: 0x170005AA RID: 1450
		// (get) Token: 0x060012B0 RID: 4784 RVA: 0x0006F9C4 File Offset: 0x0006DBC4
		// (set) Token: 0x060012B1 RID: 4785 RVA: 0x0006F9CC File Offset: 0x0006DBCC
		public KeyboardChar KeyT { get; set; }

		// Token: 0x170005AB RID: 1451
		// (get) Token: 0x060012B2 RID: 4786 RVA: 0x0006F9D5 File Offset: 0x0006DBD5
		// (set) Token: 0x060012B3 RID: 4787 RVA: 0x0006F9DD File Offset: 0x0006DBDD
		public KeyboardChar KeyZ { get; set; }

		// Token: 0x170005AC RID: 1452
		// (get) Token: 0x060012B4 RID: 4788 RVA: 0x0006F9E6 File Offset: 0x0006DBE6
		// (set) Token: 0x060012B5 RID: 4789 RVA: 0x0006F9EE File Offset: 0x0006DBEE
		public KeyboardChar KeyU { get; set; }

		// Token: 0x170005AD RID: 1453
		// (get) Token: 0x060012B6 RID: 4790 RVA: 0x0006F9F7 File Offset: 0x0006DBF7
		// (set) Token: 0x060012B7 RID: 4791 RVA: 0x0006F9FF File Offset: 0x0006DBFF
		public KeyboardChar KeyI { get; set; }

		// Token: 0x170005AE RID: 1454
		// (get) Token: 0x060012B8 RID: 4792 RVA: 0x0006FA08 File Offset: 0x0006DC08
		// (set) Token: 0x060012B9 RID: 4793 RVA: 0x0006FA10 File Offset: 0x0006DC10
		public KeyboardChar KeyO { get; set; }

		// Token: 0x170005AF RID: 1455
		// (get) Token: 0x060012BA RID: 4794 RVA: 0x0006FA19 File Offset: 0x0006DC19
		// (set) Token: 0x060012BB RID: 4795 RVA: 0x0006FA21 File Offset: 0x0006DC21
		public KeyboardChar KeyP { get; set; }

		// Token: 0x170005B0 RID: 1456
		// (get) Token: 0x060012BC RID: 4796 RVA: 0x0006FA2A File Offset: 0x0006DC2A
		// (set) Token: 0x060012BD RID: 4797 RVA: 0x0006FA32 File Offset: 0x0006DC32
		public KeyboardChar KeyA { get; set; }

		// Token: 0x170005B1 RID: 1457
		// (get) Token: 0x060012BE RID: 4798 RVA: 0x0006FA3B File Offset: 0x0006DC3B
		// (set) Token: 0x060012BF RID: 4799 RVA: 0x0006FA43 File Offset: 0x0006DC43
		public KeyboardChar KeyS { get; set; }

		// Token: 0x170005B2 RID: 1458
		// (get) Token: 0x060012C0 RID: 4800 RVA: 0x0006FA4C File Offset: 0x0006DC4C
		// (set) Token: 0x060012C1 RID: 4801 RVA: 0x0006FA54 File Offset: 0x0006DC54
		public KeyboardChar KeyD { get; set; }

		// Token: 0x170005B3 RID: 1459
		// (get) Token: 0x060012C2 RID: 4802 RVA: 0x0006FA5D File Offset: 0x0006DC5D
		// (set) Token: 0x060012C3 RID: 4803 RVA: 0x0006FA65 File Offset: 0x0006DC65
		public KeyboardChar KeyF { get; set; }

		// Token: 0x170005B4 RID: 1460
		// (get) Token: 0x060012C4 RID: 4804 RVA: 0x0006FA6E File Offset: 0x0006DC6E
		// (set) Token: 0x060012C5 RID: 4805 RVA: 0x0006FA76 File Offset: 0x0006DC76
		public KeyboardChar KeyG { get; set; }

		// Token: 0x170005B5 RID: 1461
		// (get) Token: 0x060012C6 RID: 4806 RVA: 0x0006FA7F File Offset: 0x0006DC7F
		// (set) Token: 0x060012C7 RID: 4807 RVA: 0x0006FA87 File Offset: 0x0006DC87
		public KeyboardChar KeyH { get; set; }

		// Token: 0x170005B6 RID: 1462
		// (get) Token: 0x060012C8 RID: 4808 RVA: 0x0006FA90 File Offset: 0x0006DC90
		// (set) Token: 0x060012C9 RID: 4809 RVA: 0x0006FA98 File Offset: 0x0006DC98
		public KeyboardChar KeyJ { get; set; }

		// Token: 0x170005B7 RID: 1463
		// (get) Token: 0x060012CA RID: 4810 RVA: 0x0006FAA1 File Offset: 0x0006DCA1
		// (set) Token: 0x060012CB RID: 4811 RVA: 0x0006FAA9 File Offset: 0x0006DCA9
		public KeyboardChar KeyK { get; set; }

		// Token: 0x170005B8 RID: 1464
		// (get) Token: 0x060012CC RID: 4812 RVA: 0x0006FAB2 File Offset: 0x0006DCB2
		// (set) Token: 0x060012CD RID: 4813 RVA: 0x0006FABA File Offset: 0x0006DCBA
		public KeyboardChar KeyL { get; set; }

		// Token: 0x170005B9 RID: 1465
		// (get) Token: 0x060012CE RID: 4814 RVA: 0x0006FAC3 File Offset: 0x0006DCC3
		// (set) Token: 0x060012CF RID: 4815 RVA: 0x0006FACB File Offset: 0x0006DCCB
		public KeyboardChar KeyY { get; set; }

		// Token: 0x170005BA RID: 1466
		// (get) Token: 0x060012D0 RID: 4816 RVA: 0x0006FAD4 File Offset: 0x0006DCD4
		// (set) Token: 0x060012D1 RID: 4817 RVA: 0x0006FADC File Offset: 0x0006DCDC
		public KeyboardChar KeyX { get; set; }

		// Token: 0x170005BB RID: 1467
		// (get) Token: 0x060012D2 RID: 4818 RVA: 0x0006FAE5 File Offset: 0x0006DCE5
		// (set) Token: 0x060012D3 RID: 4819 RVA: 0x0006FAED File Offset: 0x0006DCED
		public KeyboardChar KeyC { get; set; }

		// Token: 0x170005BC RID: 1468
		// (get) Token: 0x060012D4 RID: 4820 RVA: 0x0006FAF6 File Offset: 0x0006DCF6
		// (set) Token: 0x060012D5 RID: 4821 RVA: 0x0006FAFE File Offset: 0x0006DCFE
		public KeyboardChar KeyV { get; set; }

		// Token: 0x170005BD RID: 1469
		// (get) Token: 0x060012D6 RID: 4822 RVA: 0x0006FB07 File Offset: 0x0006DD07
		// (set) Token: 0x060012D7 RID: 4823 RVA: 0x0006FB0F File Offset: 0x0006DD0F
		public KeyboardChar KeyB { get; set; }

		// Token: 0x170005BE RID: 1470
		// (get) Token: 0x060012D8 RID: 4824 RVA: 0x0006FB18 File Offset: 0x0006DD18
		// (set) Token: 0x060012D9 RID: 4825 RVA: 0x0006FB20 File Offset: 0x0006DD20
		public KeyboardChar KeyN { get; set; }

		// Token: 0x170005BF RID: 1471
		// (get) Token: 0x060012DA RID: 4826 RVA: 0x0006FB29 File Offset: 0x0006DD29
		// (set) Token: 0x060012DB RID: 4827 RVA: 0x0006FB31 File Offset: 0x0006DD31
		public KeyboardChar KeyM { get; set; }

		// Token: 0x170005C0 RID: 1472
		// (get) Token: 0x060012DC RID: 4828 RVA: 0x0006FB3A File Offset: 0x0006DD3A
		// (set) Token: 0x060012DD RID: 4829 RVA: 0x0006FB42 File Offset: 0x0006DD42
		public KeyboardChar KeySpace { get; set; }

		// Token: 0x170005C1 RID: 1473
		// (get) Token: 0x060012DE RID: 4830 RVA: 0x0006FB4B File Offset: 0x0006DD4B
		// (set) Token: 0x060012DF RID: 4831 RVA: 0x0006FB53 File Offset: 0x0006DD53
		public KeyboardChar KeyPlus { get; set; }

		// Token: 0x170005C2 RID: 1474
		// (get) Token: 0x060012E0 RID: 4832 RVA: 0x0006FB5C File Offset: 0x0006DD5C
		// (set) Token: 0x060012E1 RID: 4833 RVA: 0x0006FB64 File Offset: 0x0006DD64
		public KeyboardChar KeyMinus { get; set; }

		// Token: 0x170005C3 RID: 1475
		// (get) Token: 0x060012E2 RID: 4834 RVA: 0x0006FB6D File Offset: 0x0006DD6D
		// (set) Token: 0x060012E3 RID: 4835 RVA: 0x0006FB75 File Offset: 0x0006DD75
		public KeyboardChar KeyUnderscore { get; set; }

		// Token: 0x170005C4 RID: 1476
		// (get) Token: 0x060012E4 RID: 4836 RVA: 0x0006FB7E File Offset: 0x0006DD7E
		// (set) Token: 0x060012E5 RID: 4837 RVA: 0x0006FB86 File Offset: 0x0006DD86
		public KeyboardChar KeySemicolon { get; set; }

		// Token: 0x170005C5 RID: 1477
		// (get) Token: 0x060012E6 RID: 4838 RVA: 0x0006FB8F File Offset: 0x0006DD8F
		// (set) Token: 0x060012E7 RID: 4839 RVA: 0x0006FB97 File Offset: 0x0006DD97
		public KeyboardChar KeyComma { get; set; }

		// Token: 0x170005C6 RID: 1478
		// (get) Token: 0x060012E8 RID: 4840 RVA: 0x0006FBA0 File Offset: 0x0006DDA0
		// (set) Token: 0x060012E9 RID: 4841 RVA: 0x0006FBA8 File Offset: 0x0006DDA8
		public KeyboardChar KeyPoint { get; set; }

		// Token: 0x170005C7 RID: 1479
		// (get) Token: 0x060012EA RID: 4842 RVA: 0x0006FBB1 File Offset: 0x0006DDB1
		// (set) Token: 0x060012EB RID: 4843 RVA: 0x0006FBB9 File Offset: 0x0006DDB9
		public KeyboardChar KeyBracketOpen { get; set; }

		// Token: 0x170005C8 RID: 1480
		// (get) Token: 0x060012EC RID: 4844 RVA: 0x0006FBC2 File Offset: 0x0006DDC2
		// (set) Token: 0x060012ED RID: 4845 RVA: 0x0006FBCA File Offset: 0x0006DDCA
		public KeyboardChar KeyBracketClose { get; set; }

		// Token: 0x170005C9 RID: 1481
		// (get) Token: 0x060012EE RID: 4846 RVA: 0x0006FBD3 File Offset: 0x0006DDD3
		// (set) Token: 0x060012EF RID: 4847 RVA: 0x0006FBDB File Offset: 0x0006DDDB
		public KeyboardChar KeyMinusAndUnderscore { get; set; }

		// Token: 0x170005CA RID: 1482
		// (get) Token: 0x060012F0 RID: 4848 RVA: 0x0006FBE4 File Offset: 0x0006DDE4
		// (set) Token: 0x060012F1 RID: 4849 RVA: 0x0006FBEC File Offset: 0x0006DDEC
		public KeyboardChar KeyAt { get; set; }

		// Token: 0x170005CB RID: 1483
		// (get) Token: 0x060012F2 RID: 4850 RVA: 0x0006FBF5 File Offset: 0x0006DDF5
		// (set) Token: 0x060012F3 RID: 4851 RVA: 0x0006FBFD File Offset: 0x0006DDFD
		public KeyboardChar KeySlash { get; set; }

		// Token: 0x170005CC RID: 1484
		// (get) Token: 0x060012F4 RID: 4852 RVA: 0x0006FC06 File Offset: 0x0006DE06
		// (set) Token: 0x060012F5 RID: 4853 RVA: 0x0006FC0E File Offset: 0x0006DE0E
		public KeyboardChar KeyBackSlash { get; set; }

		// Token: 0x170005CD RID: 1485
		// (get) Token: 0x060012F6 RID: 4854 RVA: 0x0006FC17 File Offset: 0x0006DE17
		// (set) Token: 0x060012F7 RID: 4855 RVA: 0x0006FC1F File Offset: 0x0006DE1F
		public KeyboardChar KeyDollar { get; set; }

		// Token: 0x170005CE RID: 1486
		// (get) Token: 0x060012F8 RID: 4856 RVA: 0x0006FC28 File Offset: 0x0006DE28
		// (set) Token: 0x060012F9 RID: 4857 RVA: 0x0006FC30 File Offset: 0x0006DE30
		public List<KeyboardChar> KeyboardChars { get; set; }

		// Token: 0x170005CF RID: 1487
		// (get) Token: 0x060012FA RID: 4858 RVA: 0x0006FC3C File Offset: 0x0006DE3C
		// (set) Token: 0x060012FB RID: 4859 RVA: 0x0006FC54 File Offset: 0x0006DE54
		public string InputText
		{
			get
			{
				return this.inputText;
			}
			set
			{
				this.SetProperty<string>(ref this.inputText, value, "InputText");
			}
		}

		// Token: 0x170005D0 RID: 1488
		// (get) Token: 0x060012FC RID: 4860 RVA: 0x0006FC6C File Offset: 0x0006DE6C
		// (set) Token: 0x060012FD RID: 4861 RVA: 0x0006FC84 File Offset: 0x0006DE84
		public int InputBoxSelectionStart
		{
			get
			{
				return this.inputBoxSelectionStart;
			}
			set
			{
				this.SetProperty<int>(ref this.inputBoxSelectionStart, value, "InputBoxSelectionStart");
			}
		}

		// Token: 0x170005D1 RID: 1489
		// (get) Token: 0x060012FE RID: 4862 RVA: 0x0006FC9C File Offset: 0x0006DE9C
		// (set) Token: 0x060012FF RID: 4863 RVA: 0x0006FCB4 File Offset: 0x0006DEB4
		public int InputBoxSelectionLength
		{
			get
			{
				return this.inputBoxSelectionLength;
			}
			set
			{
				this.SetProperty<int>(ref this.inputBoxSelectionLength, value, "InputBoxSelectionLength");
			}
		}

		// Token: 0x170005D2 RID: 1490
		// (get) Token: 0x06001300 RID: 4864 RVA: 0x0006FCCA File Offset: 0x0006DECA
		// (set) Token: 0x06001301 RID: 4865 RVA: 0x0006FCD2 File Offset: 0x0006DED2
		public KeyboardType Type { get; set; }

		// Token: 0x170005D3 RID: 1491
		// (get) Token: 0x06001302 RID: 4866 RVA: 0x0006FCDC File Offset: 0x0006DEDC
		public bool IsPasswordKeyboard
		{
			get
			{
				return this.Type == KeyboardType.Password;
			}
		}

		// Token: 0x170005D4 RID: 1492
		// (get) Token: 0x06001303 RID: 4867 RVA: 0x0006FCF8 File Offset: 0x0006DEF8
		public bool IsNotPasswordKeyboard
		{
			get
			{
				return this.Type != KeyboardType.Password;
			}
		}

		// Token: 0x170005D5 RID: 1493
		// (get) Token: 0x06001304 RID: 4868 RVA: 0x0006FD16 File Offset: 0x0006DF16
		// (set) Token: 0x06001305 RID: 4869 RVA: 0x0006FD1E File Offset: 0x0006DF1E
		public PasswordBox PasswordBox { get; set; }

		// Token: 0x170005D6 RID: 1494
		// (get) Token: 0x06001306 RID: 4870 RVA: 0x0006FD27 File Offset: 0x0006DF27
		// (set) Token: 0x06001307 RID: 4871 RVA: 0x0006FD2F File Offset: 0x0006DF2F
		public string[] Passwords { get; set; }

		// Token: 0x170005D7 RID: 1495
		// (get) Token: 0x06001308 RID: 4872 RVA: 0x0006FD38 File Offset: 0x0006DF38
		// (set) Token: 0x06001309 RID: 4873 RVA: 0x0006FD40 File Offset: 0x0006DF40
		public bool TestPassword { get; set; }

		// Token: 0x170005D8 RID: 1496
		// (get) Token: 0x0600130A RID: 4874 RVA: 0x0006FD49 File Offset: 0x0006DF49
		// (set) Token: 0x0600130B RID: 4875 RVA: 0x0006FD51 File Offset: 0x0006DF51
		public string Description { get; set; }

		// Token: 0x170005D9 RID: 1497
		// (get) Token: 0x0600130C RID: 4876 RVA: 0x0006FD5A File Offset: 0x0006DF5A
		// (set) Token: 0x0600130D RID: 4877 RVA: 0x0006FD62 File Offset: 0x0006DF62
		public string OldValue { get; set; }

		// Token: 0x170005DA RID: 1498
		// (get) Token: 0x0600130E RID: 4878 RVA: 0x0006FD6B File Offset: 0x0006DF6B
		// (set) Token: 0x0600130F RID: 4879 RVA: 0x0006FD73 File Offset: 0x0006DF73
		public bool ShouldClose { get; set; }

		// Token: 0x170005DB RID: 1499
		// (get) Token: 0x06001310 RID: 4880 RVA: 0x0006FD7C File Offset: 0x0006DF7C
		// (set) Token: 0x06001311 RID: 4881 RVA: 0x0006FD94 File Offset: 0x0006DF94
		public bool WrongPasswordEntered
		{
			get
			{
				return this.wrongPasswordEntered;
			}
			set
			{
				this.SetProperty<bool>(ref this.wrongPasswordEntered, value, "WrongPasswordEntered");
			}
		}

		// Token: 0x170005DC RID: 1500
		// (get) Token: 0x06001312 RID: 4882 RVA: 0x0006FDAC File Offset: 0x0006DFAC
		// (set) Token: 0x06001313 RID: 4883 RVA: 0x0006FDC4 File Offset: 0x0006DFC4
		public bool IsUpperCase
		{
			get
			{
				return this.isUpperCase;
			}
			set
			{
				this.SetProperty<bool>(ref this.isUpperCase, value, "IsUpperCase");
			}
		}

		// Token: 0x170005DD RID: 1501
		// (get) Token: 0x06001314 RID: 4884 RVA: 0x0006FDDC File Offset: 0x0006DFDC
		// (set) Token: 0x06001315 RID: 4885 RVA: 0x0006FDF4 File Offset: 0x0006DFF4
		public bool Widescreen
		{
			get
			{
				return this.widescreen;
			}
			set
			{
				this.SetProperty<bool>(ref this.widescreen, value, "Widescreen");
			}
		}

		// Token: 0x170005DE RID: 1502
		// (get) Token: 0x06001316 RID: 4886 RVA: 0x0006FE0C File Offset: 0x0006E00C
		public bool NoWidescreen
		{
			get
			{
				return !this.widescreen;
			}
		}

		// Token: 0x170005DF RID: 1503
		// (get) Token: 0x06001317 RID: 4887 RVA: 0x0006FE27 File Offset: 0x0006E027
		// (set) Token: 0x06001318 RID: 4888 RVA: 0x0006FE2F File Offset: 0x0006E02F
		public bool HasEnterButton { get; set; }

		// Token: 0x170005E0 RID: 1504
		// (get) Token: 0x06001319 RID: 4889 RVA: 0x0006FE38 File Offset: 0x0006E038
		public bool HasNotEnterButton
		{
			get
			{
				return !this.HasEnterButton;
			}
		}

		// Token: 0x170005E1 RID: 1505
		// (get) Token: 0x0600131A RID: 4890 RVA: 0x0006FE53 File Offset: 0x0006E053
		// (set) Token: 0x0600131B RID: 4891 RVA: 0x0006FE5B File Offset: 0x0006E05B
		public int MaxTextLength { get; set; }

		// Token: 0x170005E2 RID: 1506
		// (get) Token: 0x0600131C RID: 4892 RVA: 0x0006FE64 File Offset: 0x0006E064
		// (set) Token: 0x0600131D RID: 4893 RVA: 0x0006FE6C File Offset: 0x0006E06C
		public RelayCommand ToggleCaseCommand { get; set; }

		// Token: 0x170005E3 RID: 1507
		// (get) Token: 0x0600131E RID: 4894 RVA: 0x0006FE75 File Offset: 0x0006E075
		// (set) Token: 0x0600131F RID: 4895 RVA: 0x0006FE7D File Offset: 0x0006E07D
		public RelayCommand AddToDisplayCommand { get; set; }

		// Token: 0x170005E4 RID: 1508
		// (get) Token: 0x06001320 RID: 4896 RVA: 0x0006FE86 File Offset: 0x0006E086
		// (set) Token: 0x06001321 RID: 4897 RVA: 0x0006FE8E File Offset: 0x0006E08E
		public RelayCommand BackspaceCommand { get; set; }

		// Token: 0x170005E5 RID: 1509
		// (get) Token: 0x06001322 RID: 4898 RVA: 0x0006FE97 File Offset: 0x0006E097
		// (set) Token: 0x06001323 RID: 4899 RVA: 0x0006FE9F File Offset: 0x0006E09F
		public RelayCommand EnterCommand { get; set; }

		// Token: 0x170005E6 RID: 1510
		// (get) Token: 0x06001324 RID: 4900 RVA: 0x0006FEA8 File Offset: 0x0006E0A8
		// (set) Token: 0x06001325 RID: 4901 RVA: 0x0006FEB0 File Offset: 0x0006E0B0
		public RelayCommand AddCharToPasswordCommand { get; set; }

		// Token: 0x170005E7 RID: 1511
		// (get) Token: 0x06001326 RID: 4902 RVA: 0x0006FEB9 File Offset: 0x0006E0B9
		// (set) Token: 0x06001327 RID: 4903 RVA: 0x0006FEC1 File Offset: 0x0006E0C1
		public RelayCommand ToggleSymbolsCommand { get; set; }

		// Token: 0x170005E8 RID: 1512
		// (get) Token: 0x06001328 RID: 4904 RVA: 0x0006FECA File Offset: 0x0006E0CA
		// (set) Token: 0x06001329 RID: 4905 RVA: 0x0006FED2 File Offset: 0x0006E0D2
		public RelayCommand CommitCommand { get; set; }

		// Token: 0x170005E9 RID: 1513
		// (get) Token: 0x0600132A RID: 4906 RVA: 0x0006FEDB File Offset: 0x0006E0DB
		// (set) Token: 0x0600132B RID: 4907 RVA: 0x0006FEE3 File Offset: 0x0006E0E3
		public RelayCommand CancelCommand { get; set; }

		// Token: 0x170005EA RID: 1514
		// (get) Token: 0x0600132C RID: 4908 RVA: 0x0006FEEC File Offset: 0x0006E0EC
		// (set) Token: 0x0600132D RID: 4909 RVA: 0x0006FEF4 File Offset: 0x0006E0F4
		public RelayCommand CleanUpCommand { get; set; }

		// Token: 0x1400005F RID: 95
		// (add) Token: 0x0600132E RID: 4910 RVA: 0x0006FF00 File Offset: 0x0006E100
		// (remove) Token: 0x0600132F RID: 4911 RVA: 0x0006FF38 File Offset: 0x0006E138
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<string> Committed;

		// Token: 0x170005EB RID: 1515
		// (get) Token: 0x06001330 RID: 4912 RVA: 0x0006FF6D File Offset: 0x0006E16D
		// (set) Token: 0x06001331 RID: 4913 RVA: 0x0006FF75 File Offset: 0x0006E175
		public BoundLocalizedText StrWrongPasswordEntered { get; set; }

		// Token: 0x170005EC RID: 1516
		// (get) Token: 0x06001332 RID: 4914 RVA: 0x0006FF7E File Offset: 0x0006E17E
		// (set) Token: 0x06001333 RID: 4915 RVA: 0x0006FF86 File Offset: 0x0006E186
		public RelayCommand CancelAllCommand { get; set; }

		// Token: 0x06001334 RID: 4916 RVA: 0x0006FF90 File Offset: 0x0006E190
		public KeyboardViewModel(Action CloseAction)
		{
			this.WState = ((Definition.windowHeight > 0.0 && Definition.windowWidth > 0.0) ? WindowState.Normal : WindowState.Maximized);
			this.ShouldClose = true;
			this.Widescreen = (Definition.windowWidth >= 1300.0);
			this.MaxTextLength = KeyboardViewModel.maxTextLength;
			this.closeAction = CloseAction;
			this.ToggleCaseCommand = new RelayCommand(new Action<object>(this.ToggleCase));
			this.AddToDisplayCommand = new RelayCommand(new Action<object>(this.AddToDisplay));
			this.BackspaceCommand = new RelayCommand(new Action<object>(this.DoBackspace));
			this.EnterCommand = new RelayCommand(new Action<object>(this.DoEnter));
			this.AddCharToPasswordCommand = new RelayCommand(new Action<object>(this.AddCharToPassword));
			this.ToggleSymbolsCommand = new RelayCommand(new Action<object>(this.ToggleSymbols));
			this.CommitCommand = new RelayCommand(new Action<object>(this.Commit));
			this.CancelCommand = new RelayCommand(new Action<object>(this.Cancel));
			this.CleanUpCommand = new RelayCommand(new Action<object>(this.CleanUp));
			this.StrWrongPasswordEntered = new BoundLocalizedText("msgWrongPassword");
			this.CancelAllCommand = new RelayCommand(new Action<object>(this.CancelAll));
			this.InitChars();
			this.AddCharsToList();
			MainWindowViewModel.accessRightsManager.UserAutoLogout += this.accessRightsManager_UserAutoLogout;
		}

		// Token: 0x06001335 RID: 4917 RVA: 0x00070148 File Offset: 0x0006E348
		private void InitChars()
		{
			this.Key1 = new KeyboardChar("1", "1", "1");
			this.Key2 = new KeyboardChar("2", "2", "2");
			this.Key3 = new KeyboardChar("3", "3", "3");
			this.Key4 = new KeyboardChar("4", "4", "4");
			this.Key5 = new KeyboardChar("5", "5", "5");
			this.Key6 = new KeyboardChar("6", "6", "6");
			this.Key7 = new KeyboardChar("7", "7", "7");
			this.Key8 = new KeyboardChar("8", "8", "8");
			this.Key9 = new KeyboardChar("9", "9", "9");
			this.Key0 = new KeyboardChar("0", "0", "0");
			this.KeyQ = new KeyboardChar("q", "Q", "");
			this.KeyW = new KeyboardChar("w", "W", "");
			this.KeyE = new KeyboardChar("e", "E", "");
			this.KeyR = new KeyboardChar("r", "R", "");
			this.KeyT = new KeyboardChar("t", "T", "");
			this.KeyQ = new KeyboardChar("q", "Q", "");
			this.KeyU = new KeyboardChar("u", "U", "");
			this.KeyI = new KeyboardChar("i", "I", "");
			this.KeyO = new KeyboardChar("o", "O", "");
			this.KeyP = new KeyboardChar("p", "P", "");
			this.KeyA = new KeyboardChar("a", "A", "");
			this.KeyS = new KeyboardChar("s", "S", "+");
			this.KeyD = new KeyboardChar("d", "D", "-");
			this.KeyF = new KeyboardChar("f", "F", "_");
			this.KeyG = new KeyboardChar("g", "G", ";");
			this.KeyH = new KeyboardChar("h", "H", ",");
			this.KeyJ = new KeyboardChar("j", "J", ".");
			this.KeyK = new KeyboardChar("k", "K", "(");
			this.KeyL = new KeyboardChar("l", "L", ")");
			this.KeyX = new KeyboardChar("x", "X", "");
			this.KeyC = new KeyboardChar("c", "C", "");
			this.KeyV = new KeyboardChar("v", "V", "");
			this.KeyB = new KeyboardChar("b", "B", "");
			this.KeyN = new KeyboardChar("n", "N", "");
			this.KeyM = new KeyboardChar("m", "M", "");
			bool flag = !LocalizationManager.CurrentCulture.TwoLetterISOLanguageName.StartsWith("en");
			if (flag)
			{
				this.KeyY = new KeyboardChar("y", "Y", "");
				this.KeyZ = new KeyboardChar("z", "Z", "");
			}
			else
			{
				this.KeyY = new KeyboardChar("z", "Z", "");
				this.KeyZ = new KeyboardChar("y", "Y", "");
			}
			this.KeySpace = new KeyboardChar(" ", " ", " ");
			this.KeyPlus = new KeyboardChar("+", "+", "+");
			this.KeyMinus = new KeyboardChar("-", "-", "-");
			this.KeyUnderscore = new KeyboardChar("_", "_", "_");
			this.KeyMinusAndUnderscore = new KeyboardChar("-", "_", "-");
			this.KeySemicolon = new KeyboardChar(";", ";", ";");
			this.KeyComma = new KeyboardChar(",", ",", ",");
			this.KeyPoint = new KeyboardChar(".", ".", ".");
			this.KeyBracketOpen = new KeyboardChar("(", "(", "(");
			this.KeyBracketClose = new KeyboardChar(")", ")", ")");
			this.KeyAt = new KeyboardChar("@", "@", "@");
			this.KeyBackSlash = new KeyboardChar("\\", "\\", "\\");
			this.KeySlash = new KeyboardChar("/", "/", "/");
			this.KeyDollar = new KeyboardChar("$", "$", "$");
		}

		// Token: 0x06001336 RID: 4918 RVA: 0x00070710 File Offset: 0x0006E910
		private void AddCharsToList()
		{
			this.KeyboardChars = new List<KeyboardChar>();
			this.KeyboardChars.Add(this.Key1);
			this.KeyboardChars.Add(this.Key2);
			this.KeyboardChars.Add(this.Key3);
			this.KeyboardChars.Add(this.Key4);
			this.KeyboardChars.Add(this.Key5);
			this.KeyboardChars.Add(this.Key6);
			this.KeyboardChars.Add(this.Key7);
			this.KeyboardChars.Add(this.Key8);
			this.KeyboardChars.Add(this.Key9);
			this.KeyboardChars.Add(this.Key0);
			this.KeyboardChars.Add(this.KeyQ);
			this.KeyboardChars.Add(this.KeyW);
			this.KeyboardChars.Add(this.KeyE);
			this.KeyboardChars.Add(this.KeyR);
			this.KeyboardChars.Add(this.KeyT);
			this.KeyboardChars.Add(this.KeyZ);
			this.KeyboardChars.Add(this.KeyU);
			this.KeyboardChars.Add(this.KeyI);
			this.KeyboardChars.Add(this.KeyO);
			this.KeyboardChars.Add(this.KeyP);
			this.KeyboardChars.Add(this.KeyA);
			this.KeyboardChars.Add(this.KeyS);
			this.KeyboardChars.Add(this.KeyD);
			this.KeyboardChars.Add(this.KeyF);
			this.KeyboardChars.Add(this.KeyG);
			this.KeyboardChars.Add(this.KeyH);
			this.KeyboardChars.Add(this.KeyJ);
			this.KeyboardChars.Add(this.KeyK);
			this.KeyboardChars.Add(this.KeyL);
			this.KeyboardChars.Add(this.KeyY);
			this.KeyboardChars.Add(this.KeyX);
			this.KeyboardChars.Add(this.KeyC);
			this.KeyboardChars.Add(this.KeyV);
			this.KeyboardChars.Add(this.KeyB);
			this.KeyboardChars.Add(this.KeyN);
			this.KeyboardChars.Add(this.KeyM);
			this.KeyboardChars.Add(this.KeyPlus);
			this.KeyboardChars.Add(this.KeyMinus);
			this.KeyboardChars.Add(this.KeyUnderscore);
			this.KeyboardChars.Add(this.KeySemicolon);
			this.KeyboardChars.Add(this.KeyComma);
			this.KeyboardChars.Add(this.KeyPoint);
			this.KeyboardChars.Add(this.KeyBracketOpen);
			this.KeyboardChars.Add(this.KeyBracketClose);
			this.KeyboardChars.Add(this.KeySpace);
			this.KeyboardChars.Add(this.KeyMinusAndUnderscore);
			this.KeyboardChars.Add(this.KeyAt);
			this.KeyboardChars.Add(this.KeyBackSlash);
			this.KeyboardChars.Add(this.KeyDollar);
			this.KeyboardChars.Add(this.KeySlash);
		}

		// Token: 0x06001337 RID: 4919 RVA: 0x00070AB0 File Offset: 0x0006ECB0
		public void ToggleCase(object obj)
		{
			for (int i = 0; i < this.KeyboardChars.Count; i++)
			{
				this.KeyboardChars[i].IsUpperCase = !this.KeyboardChars[i].IsUpperCase;
			}
		}

		// Token: 0x06001338 RID: 4920 RVA: 0x00070B00 File Offset: 0x0006ED00
		private void AddToDisplay(object objString)
		{
			bool flag = objString is string;
			if (flag)
			{
				string text = (string)objString;
				bool flag2 = (this.Type == KeyboardType.Application && this.InputText.Length < this.MaxTextLength) || this.Type == KeyboardType.Default || (this.Type == KeyboardType.Cancelable && this.InputText.Length < this.MaxTextLength) || (this.Type == KeyboardType.Email && this.InputText.Length < this.MaxTextLength) || (this.Type == KeyboardType.Network && this.InputText.Length < this.MaxTextLength);
				if (flag2)
				{
					int num = this.inputBoxSelectionStart;
					this.DeleteSelectedChars();
					this.InputText = this.InputText.Insert(num, text);
					this.InputBoxSelectionStart = num + 1;
					bool flag3 = this.IsUpperCase;
					if (flag3)
					{
						this.ToggleCase(null);
						this.IsUpperCase = false;
					}
				}
				else
				{
					bool flag4 = this.Type == KeyboardType.Password;
					if (flag4)
					{
						bool flag5 = this.PasswordBox.Password.Length < KeyboardViewModel.maxPasswordLength;
						if (flag5)
						{
							PasswordBox passwordBox = this.PasswordBox;
							passwordBox.Password += text;
						}
					}
				}
			}
		}

		// Token: 0x06001339 RID: 4921 RVA: 0x00070C44 File Offset: 0x0006EE44
		private void DeleteSelectedChars()
		{
			bool flag = this.Type != KeyboardType.Password;
			if (flag)
			{
				bool flag2 = this.inputBoxSelectionLength != 0 && this.inputBoxSelectionStart + this.inputBoxSelectionLength <= this.inputText.Length;
				if (flag2)
				{
					string str = this.inputText.Substring(0, this.inputBoxSelectionStart);
					str += this.inputText.Remove(0, this.inputBoxSelectionStart + this.inputBoxSelectionLength);
					this.InputText = str;
				}
			}
		}

		// Token: 0x0600133A RID: 4922 RVA: 0x00070CD0 File Offset: 0x0006EED0
		private void DoBackspace(object obj)
		{
			bool flag = this.Type != KeyboardType.Password;
			if (flag)
			{
				EditingCommands.Backspace.Execute(0, obj as IInputElement);
			}
			else
			{
				bool flag2 = this.PasswordBox.Password.Length > 0;
				if (flag2)
				{
					this.PasswordBox.Password = this.PasswordBox.Password.Substring(0, this.PasswordBox.Password.Length - 1);
				}
			}
		}

		// Token: 0x0600133B RID: 4923 RVA: 0x00070D50 File Offset: 0x0006EF50
		private void DoEnter(object obj)
		{
			bool flag = this.Type != KeyboardType.Password && this.HasEnterButton;
			if (flag)
			{
				bool flag2 = this.inputText.CountChar('\n') < 4;
				if (flag2)
				{
					EditingCommands.EnterLineBreak.Execute(0, obj as IInputElement);
				}
			}
		}

		// Token: 0x0600133C RID: 4924 RVA: 0x00070DA4 File Offset: 0x0006EFA4
		private void AddCharToPassword(object obj)
		{
			string text = obj as string;
			bool flag = this.Type == KeyboardType.Password && text != null;
			if (flag)
			{
				bool flag2 = text[0] != '\b';
				if (flag2)
				{
					this.AddToDisplay(text);
				}
				else
				{
					this.DoBackspace(null);
				}
			}
		}

		// Token: 0x0600133D RID: 4925 RVA: 0x00070DF4 File Offset: 0x0006EFF4
		public void ToggleSymbols(object obj)
		{
			for (int i = 0; i < this.KeyboardChars.Count; i++)
			{
				this.KeyboardChars[i].SymbolMode = !this.KeyboardChars[i].SymbolMode;
			}
		}

		// Token: 0x0600133E RID: 4926 RVA: 0x00070E44 File Offset: 0x0006F044
		public void Commit(object obj)
		{
			bool flag = false;
			bool flag2 = this.Type == KeyboardType.Password;
			if (flag2)
			{
				bool flag3 = this.Passwords != null;
				if (flag3)
				{
					for (int i = 0; i < this.Passwords.Length; i++)
					{
						bool flag4 = this.PasswordBox.Password.ToMD5() == this.Passwords[i];
						if (flag4)
						{
							flag = true;
							break;
						}
					}
				}
				bool flag5 = !flag && this.TestPassword;
				if (flag5)
				{
					this.WrongPasswordEntered = true;
					this.PasswordBox.Password = "";
				}
				else
				{
					this.OnCommitted(this.PasswordBox.Password);
					bool flag6 = this.ShouldClose && this.closeAction != null;
					if (flag6)
					{
						this.closeAction();
					}
					else
					{
						this.ShouldClose = true;
					}
				}
			}
			else
			{
				this.OnCommitted(this.InputText);
				bool flag7 = this.ShouldClose && this.closeAction != null;
				if (flag7)
				{
					this.closeAction();
				}
				else
				{
					this.ShouldClose = true;
				}
			}
		}

		// Token: 0x0600133F RID: 4927 RVA: 0x00070F6C File Offset: 0x0006F16C
		public void Cancel(object obj)
		{
			bool flag = this.closeAction != null;
			if (flag)
			{
				this.closeAction();
			}
		}

		// Token: 0x06001340 RID: 4928 RVA: 0x00070F94 File Offset: 0x0006F194
		public void CancelAll(object obj)
		{
			bool flag = this.closeAction != null;
			if (flag)
			{
				this.closeAction();
				HistoryViewModel.cancelSaving = true;
			}
		}

		// Token: 0x06001341 RID: 4929 RVA: 0x00070FC4 File Offset: 0x0006F1C4
		protected virtual void OnCommitted(string text)
		{
			Action<string> committed = this.Committed;
			bool flag = committed != null;
			if (flag)
			{
				committed(text);
			}
		}

		// Token: 0x06001342 RID: 4930 RVA: 0x00070FE9 File Offset: 0x0006F1E9
		private void accessRightsManager_UserAutoLogout()
		{
			this.Cancel(null);
		}

		// Token: 0x06001343 RID: 4931 RVA: 0x00070FF4 File Offset: 0x0006F1F4
		public void CleanUp(object obj)
		{
			MainWindowViewModel.accessRightsManager.UserAutoLogout -= this.accessRightsManager_UserAutoLogout;
			MainWindow.localizationManager.Deregister(this.StrWrongPasswordEntered);
		}

		// Token: 0x04000C41 RID: 3137
		public static readonly int maxTextLength = 40;

		// Token: 0x04000C42 RID: 3138
		public static readonly int maxPasswordLength = 40;

		// Token: 0x04000C43 RID: 3139
		private string inputText;

		// Token: 0x04000C44 RID: 3140
		private int inputBoxSelectionStart;

		// Token: 0x04000C45 RID: 3141
		private int inputBoxSelectionLength;

		// Token: 0x04000C46 RID: 3142
		private Action closeAction;

		// Token: 0x04000C47 RID: 3143
		private bool wrongPasswordEntered;

		// Token: 0x04000C48 RID: 3144
		private bool isUpperCase = true;

		// Token: 0x04000C49 RID: 3145
		private bool widescreen;

		// Token: 0x04000C4B RID: 3147
		private bool cancelVisibility = false;

		// Token: 0x04000C4C RID: 3148
		private bool emailVisibility = false;

		// Token: 0x04000C4D RID: 3149
		private bool networkVisibility = false;
	}
}
