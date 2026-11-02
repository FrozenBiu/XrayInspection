using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Threading;
using Common;
using DatabaseConnection;
using Definitions;
using GUI_XRS.AuditCheckNew;
using GUI_XRS.Properties;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x02000025 RID: 37
	public class AuditCheckProcessViewModel : INotifyPropertyChanged
	{
		// Token: 0x1400001C RID: 28
		// (add) Token: 0x06000420 RID: 1056 RVA: 0x0001AF14 File Offset: 0x00019114
		// (remove) Token: 0x06000421 RID: 1057 RVA: 0x0001AF4C File Offset: 0x0001914C
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x06000422 RID: 1058 RVA: 0x0001AF81 File Offset: 0x00019181
		// (set) Token: 0x06000423 RID: 1059 RVA: 0x0001AF89 File Offset: 0x00019189
		public BoundLocalizedText StrData { get; set; }

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x06000424 RID: 1060 RVA: 0x0001AF92 File Offset: 0x00019192
		// (set) Token: 0x06000425 RID: 1061 RVA: 0x0001AF9A File Offset: 0x0001919A
		public BoundLocalizedText StrRequest { get; set; }

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x06000426 RID: 1062 RVA: 0x0001AFA3 File Offset: 0x000191A3
		// (set) Token: 0x06000427 RID: 1063 RVA: 0x0001AFAB File Offset: 0x000191AB
		public BoundLocalizedText StrResult { get; set; }

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x06000428 RID: 1064 RVA: 0x0001AFB4 File Offset: 0x000191B4
		// (set) Token: 0x06000429 RID: 1065 RVA: 0x0001AFBC File Offset: 0x000191BC
		public BoundLocalizedText StrAuditCheck { get; set; }

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x0600042A RID: 1066 RVA: 0x0001AFC5 File Offset: 0x000191C5
		// (set) Token: 0x0600042B RID: 1067 RVA: 0x0001AFCD File Offset: 0x000191CD
		public BoundLocalizedText StrProduct { get; set; }

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x0600042C RID: 1068 RVA: 0x0001AFD6 File Offset: 0x000191D6
		// (set) Token: 0x0600042D RID: 1069 RVA: 0x0001AFDE File Offset: 0x000191DE
		public BoundLocalizedText StrSampleOk { get; set; }

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x0600042E RID: 1070 RVA: 0x0001AFE7 File Offset: 0x000191E7
		// (set) Token: 0x0600042F RID: 1071 RVA: 0x0001AFEF File Offset: 0x000191EF
		public BoundLocalizedText StrSampleFail { get; set; }

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x06000430 RID: 1072 RVA: 0x0001AFF8 File Offset: 0x000191F8
		// (set) Token: 0x06000431 RID: 1073 RVA: 0x0001B000 File Offset: 0x00019200
		public BoundLocalizedText StrContinue { get; set; }

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x06000432 RID: 1074 RVA: 0x0001B009 File Offset: 0x00019209
		// (set) Token: 0x06000433 RID: 1075 RVA: 0x0001B011 File Offset: 0x00019211
		public BoundLocalizedText StrCheckImages { get; set; }

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x06000434 RID: 1076 RVA: 0x0001B01A File Offset: 0x0001921A
		// (set) Token: 0x06000435 RID: 1077 RVA: 0x0001B022 File Offset: 0x00019222
		public BoundLocalizedText StrConvey { get; set; }

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x06000436 RID: 1078 RVA: 0x0001B02B File Offset: 0x0001922B
		// (set) Token: 0x06000437 RID: 1079 RVA: 0x0001B033 File Offset: 0x00019233
		public BoundLocalizedText StrStartAudit { get; set; }

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x06000438 RID: 1080 RVA: 0x0001B03C File Offset: 0x0001923C
		// (set) Token: 0x06000439 RID: 1081 RVA: 0x0001B044 File Offset: 0x00019244
		public BoundLocalizedText StrPassed { get; set; }

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x0600043A RID: 1082 RVA: 0x0001B04D File Offset: 0x0001924D
		// (set) Token: 0x0600043B RID: 1083 RVA: 0x0001B055 File Offset: 0x00019255
		public BoundLocalizedText StrFailed { get; set; }

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x0600043C RID: 1084 RVA: 0x0001B05E File Offset: 0x0001925E
		// (set) Token: 0x0600043D RID: 1085 RVA: 0x0001B066 File Offset: 0x00019266
		public BoundLocalizedText StrTestballs { get; set; }

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x0600043E RID: 1086 RVA: 0x0001B06F File Offset: 0x0001926F
		// (set) Token: 0x0600043F RID: 1087 RVA: 0x0001B077 File Offset: 0x00019277
		public BoundLocalizedText StrProducts { get; set; }

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x06000440 RID: 1088 RVA: 0x0001B080 File Offset: 0x00019280
		// (set) Token: 0x06000441 RID: 1089 RVA: 0x0001B088 File Offset: 0x00019288
		public BoundLocalizedText StrNumberContaminations { get; set; }

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x06000442 RID: 1090 RVA: 0x0001B091 File Offset: 0x00019291
		// (set) Token: 0x06000443 RID: 1091 RVA: 0x0001B099 File Offset: 0x00019299
		public BoundLocalizedText StrStartTime { get; set; }

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x06000444 RID: 1092 RVA: 0x0001B0A2 File Offset: 0x000192A2
		// (set) Token: 0x06000445 RID: 1093 RVA: 0x0001B0AA File Offset: 0x000192AA
		public BoundLocalizedText StrEndTime { get; set; }

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x06000446 RID: 1094 RVA: 0x0001B0B3 File Offset: 0x000192B3
		// (set) Token: 0x06000447 RID: 1095 RVA: 0x0001B0BB File Offset: 0x000192BB
		public BoundLocalizedText StrTestcard { get; set; }

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x06000448 RID: 1096 RVA: 0x0001B0C4 File Offset: 0x000192C4
		// (set) Token: 0x06000449 RID: 1097 RVA: 0x0001B0CC File Offset: 0x000192CC
		public BoundLocalizedText StrWeight { get; set; }

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x0600044A RID: 1098 RVA: 0x0001B0D5 File Offset: 0x000192D5
		// (set) Token: 0x0600044B RID: 1099 RVA: 0x0001B0DD File Offset: 0x000192DD
		public BoundLocalizedText StrNumber { get; set; }

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x0600044C RID: 1100 RVA: 0x0001B0E6 File Offset: 0x000192E6
		// (set) Token: 0x0600044D RID: 1101 RVA: 0x0001B0EE File Offset: 0x000192EE
		public BoundLocalizedText StrEndResult { get; set; }

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x0600044E RID: 1102 RVA: 0x0001B0F7 File Offset: 0x000192F7
		// (set) Token: 0x0600044F RID: 1103 RVA: 0x0001B0FF File Offset: 0x000192FF
		public BoundLocalizedText StrDocumenation { get; set; }

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x06000450 RID: 1104 RVA: 0x0001B108 File Offset: 0x00019308
		// (set) Token: 0x06000451 RID: 1105 RVA: 0x0001B110 File Offset: 0x00019310
		public BoundLocalizedText StrUser { get; set; }

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x06000452 RID: 1106 RVA: 0x0001B119 File Offset: 0x00019319
		// (set) Token: 0x06000453 RID: 1107 RVA: 0x0001B121 File Offset: 0x00019321
		public BoundLocalizedText StrSign { get; set; }

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x06000454 RID: 1108 RVA: 0x0001B12A File Offset: 0x0001932A
		// (set) Token: 0x06000455 RID: 1109 RVA: 0x0001B132 File Offset: 0x00019332
		public BoundLocalizedText StrProductNumber { get; set; }

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x06000456 RID: 1110 RVA: 0x0001B13B File Offset: 0x0001933B
		// (set) Token: 0x06000457 RID: 1111 RVA: 0x0001B143 File Offset: 0x00019343
		public BoundLocalizedText StrDeviceData { get; set; }

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x06000458 RID: 1112 RVA: 0x0001B14C File Offset: 0x0001934C
		// (set) Token: 0x06000459 RID: 1113 RVA: 0x0001B154 File Offset: 0x00019354
		public BoundLocalizedText StrRemainingTime { get; set; }

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x0600045A RID: 1114 RVA: 0x0001B160 File Offset: 0x00019360
		// (set) Token: 0x0600045B RID: 1115 RVA: 0x0001B178 File Offset: 0x00019378
		public XRSImage CurrentImage
		{
			get
			{
				return this.currentImage;
			}
			set
			{
				bool flag = this.currentImage != value;
				if (flag)
				{
					this.currentImage = value;
					this.OnPropertyChanged("CurrentImage");
				}
			}
		}

		// Token: 0x1400001D RID: 29
		// (add) Token: 0x0600045C RID: 1116 RVA: 0x0001B1AC File Offset: 0x000193AC
		// (remove) Token: 0x0600045D RID: 1117 RVA: 0x0001B1E4 File Offset: 0x000193E4
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action Close;

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x0600045E RID: 1118 RVA: 0x0001B219 File Offset: 0x00019419
		// (set) Token: 0x0600045F RID: 1119 RVA: 0x0001B221 File Offset: 0x00019421
		public AccessRightsManager AccessManager { get; set; }

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x06000460 RID: 1120 RVA: 0x0001B22C File Offset: 0x0001942C
		// (set) Token: 0x06000461 RID: 1121 RVA: 0x0001B244 File Offset: 0x00019444
		public string StringNumberContaminations
		{
			get
			{
				return this.stringNumberContaminations;
			}
			set
			{
				bool flag = value != this.stringNumberContaminations;
				if (flag)
				{
					this.stringNumberContaminations = value;
					this.OnPropertyChanged("StringNumbercontaminations");
				}
			}
		}

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x06000462 RID: 1122 RVA: 0x0001B278 File Offset: 0x00019478
		// (set) Token: 0x06000463 RID: 1123 RVA: 0x0001B290 File Offset: 0x00019490
		public int RemainingProducts
		{
			get
			{
				return this.remainingProducts;
			}
			set
			{
				bool flag = this.remainingProducts != value;
				if (flag)
				{
					this.remainingProducts = value;
					this.OnPropertyChanged("RemainingProducts");
				}
			}
		}

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x06000464 RID: 1124 RVA: 0x0001B2C4 File Offset: 0x000194C4
		// (set) Token: 0x06000465 RID: 1125 RVA: 0x0001B2DC File Offset: 0x000194DC
		public int RemainingTestBalls
		{
			get
			{
				return this.remainingTestBalls;
			}
			set
			{
				bool flag = this.remainingTestBalls != value;
				if (flag)
				{
					this.remainingTestBalls = value;
					this.OnPropertyChanged("RemainingTestBalls");
				}
			}
		}

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x06000466 RID: 1126 RVA: 0x0001B310 File Offset: 0x00019510
		// (set) Token: 0x06000467 RID: 1127 RVA: 0x0001B328 File Offset: 0x00019528
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

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x06000468 RID: 1128 RVA: 0x0001B35B File Offset: 0x0001955B
		// (set) Token: 0x06000469 RID: 1129 RVA: 0x0001B363 File Offset: 0x00019563
		public ObservableCollection<RejecterItem> UnlockBinItem { get; set; }

		// Token: 0x17000159 RID: 345
		// (get) Token: 0x0600046A RID: 1130 RVA: 0x0001B36C File Offset: 0x0001956C
		// (set) Token: 0x0600046B RID: 1131 RVA: 0x0001B374 File Offset: 0x00019574
		public RelayCommand UnlockBinCommand { get; set; }

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x0600046C RID: 1132 RVA: 0x0001B380 File Offset: 0x00019580
		// (set) Token: 0x0600046D RID: 1133 RVA: 0x0001B398 File Offset: 0x00019598
		public bool UserLogAvailable
		{
			get
			{
				return this.userLogAvailable;
			}
			set
			{
				this.userLogAvailable = value;
				this.OnPropertyChanged("UserLogAvailable");
			}
		}

		// Token: 0x1700015B RID: 347
		// (get) Token: 0x0600046E RID: 1134 RVA: 0x0001B3B0 File Offset: 0x000195B0
		// (set) Token: 0x0600046F RID: 1135 RVA: 0x0001B3C8 File Offset: 0x000195C8
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

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x06000470 RID: 1136 RVA: 0x0001B3E0 File Offset: 0x000195E0
		// (set) Token: 0x06000471 RID: 1137 RVA: 0x0001B3F8 File Offset: 0x000195F8
		public bool IsCont
		{
			get
			{
				return this.isCont;
			}
			set
			{
				this.isCont = value;
				this.IsNotCont = !value;
				this.OnPropertyChanged("IsCont");
			}
		}

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x06000472 RID: 1138 RVA: 0x0001B41C File Offset: 0x0001961C
		// (set) Token: 0x06000473 RID: 1139 RVA: 0x0001B434 File Offset: 0x00019634
		public bool IsNotCont
		{
			get
			{
				return this.isNotCont;
			}
			set
			{
				this.isNotCont = value;
				this.OnPropertyChanged("IsNotCont");
			}
		}

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x06000474 RID: 1140 RVA: 0x0001B44C File Offset: 0x0001964C
		// (set) Token: 0x06000475 RID: 1141 RVA: 0x0001B464 File Offset: 0x00019664
		public string TextStep1
		{
			get
			{
				return this.textStep1;
			}
			set
			{
				this.textStep1 = value;
				this.OnPropertyChanged("TextStep1");
			}
		}

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x06000476 RID: 1142 RVA: 0x0001B47A File Offset: 0x0001967A
		// (set) Token: 0x06000477 RID: 1143 RVA: 0x0001B482 File Offset: 0x00019682
		public string Data { get; set; }

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x06000478 RID: 1144 RVA: 0x0001B48C File Offset: 0x0001968C
		// (set) Token: 0x06000479 RID: 1145 RVA: 0x0001B4A4 File Offset: 0x000196A4
		public bool button1Enabled
		{
			get
			{
				return this.enabled1;
			}
			set
			{
				bool flag = value != this.enabled1;
				if (flag)
				{
					this.enabled1 = value;
					this.OnPropertyChanged("button1Enabled");
					bool flag2 = this.enabled1;
					if (flag2)
					{
						bool flag3 = this.AuditRequired.TimeToStartAudit > 0;
						if (flag3)
						{
							this.RemainingTimeVisible = true;
						}
						else
						{
							this.RemainingTimeVisible = false;
						}
						bool flag4 = this.Mode == AuditMode.Contamination;
						if (flag4)
						{
							this.TestsamplesList = this.initTestsampleList();
						}
						else
						{
							bool flag5 = this.Mode == AuditMode.Weight;
							if (flag5)
							{
								string text = "";
								string text2 = "";
								string text3 = "";
								WeightUnit weightUnit = WeightUnit.Gramm;
								string tableName = "";
								this.dbConnection.GetCurrentProductTableName(ref tableName);
								bool flag6 = this.dbConnection.Connection != null;
								if (flag6)
								{
									this.dbConnection.GetParameterValue(Definition.strReferenceWeight, tableName, ref text);
									this.dbConnection.GetParameterValue("MinWeight", tableName, ref text2);
									this.dbConnection.GetParameterValue("MaxWeight", tableName, ref text3);
									this.dbConnection.GetWeightUnit(ref weightUnit);
								}
								this.TextStep1 = string.Concat(new string[]
								{
									this.StrWeight.LocalizedText,
									": ",
									text2,
									" ",
									weightUnit.ToString(),
									" - ",
									text3,
									" ",
									weightUnit.ToString()
								});
							}
							else
							{
								bool flag7 = this.Mode == AuditMode.Count;
								if (flag7)
								{
									string tableName2 = "";
									this.dbConnection.GetCurrentProductTableName(ref tableName2);
									string str = "/objectCounter/";
									string str2 = str + "MinObjects/Value";
									string str3 = str + "MaxObjects/Value";
									string text4 = "";
									this.dbConnection.GetParameterValue(Definition.filterMainPath + str2, tableName2, ref text4);
									string text5 = "";
									this.dbConnection.GetParameterValue(Definition.filterMainPath + str3, tableName2, ref text5);
									this.TextStep1 = string.Concat(new string[]
									{
										this.StrNumber.LocalizedText,
										": ",
										text4,
										" - ",
										text5
									});
								}
							}
						}
						bool flag8 = !this.cancelled;
						if (flag8)
						{
							LogbookManager.AddEntry(SSTEntryCode.AuditStart);
						}
					}
				}
			}
		}

		// Token: 0x0600047A RID: 1146 RVA: 0x0001B72C File Offset: 0x0001992C
		private ObservableCollection<InfosTestsamples> initTestsampleList()
		{
			ObservableCollection<InfosTestsamples> observableCollection = new ObservableCollection<InfosTestsamples>();
			List<string> list = new List<string>();
			string text = "";
			this.dbConnection.GetParameterValue(this.TestcardMaterialPath1, ref text);
			bool flag = text != null && text != "n.a." && text != "" && text != "None";
			if (flag)
			{
				list.Add(text);
				this.dbConnection.GetParameterValue(this.TestcardType1, ref text);
				list.Add(text);
				this.dbConnection.GetParameterValue(this.TestcardSize1, ref text);
				list.Add(text);
				this.dbConnection.GetParameterValue(this.TestcardNumberCont1, ref text);
				list.Add(text);
				this.dbConnection.GetParameterValue(this.TestcardNumber1, ref text);
				list.Add(text);
				observableCollection.Add(new InfosTestsamples
				{
					Description = this.convertStringToTestcardMaterial(list[0]),
					Value = list[2],
					TestcardSize = this.convertStringToTestcardType(list[1]),
					NumberFoundTestballs = 0,
					NumberTestballsToBeFound = Convert.ToInt32(list[3]),
					INNUmber = list[4],
					NumberProductsRejected = 0
				});
				list = new List<string>();
			}
			this.dbConnection.GetParameterValue(this.TestcardMaterialPath2, ref text);
			bool flag2 = text != null && text != "n.a." && text != "" && text != "None";
			if (flag2)
			{
				list.Add(text);
				this.dbConnection.GetParameterValue(this.TestcardType2, ref text);
				list.Add(text);
				this.dbConnection.GetParameterValue(this.TestcardSize2, ref text);
				list.Add(text);
				this.dbConnection.GetParameterValue(this.TestcardNumberCont2, ref text);
				list.Add(text);
				this.dbConnection.GetParameterValue(this.TestcardNumber2, ref text);
				list.Add(text);
				observableCollection.Add(new InfosTestsamples
				{
					Description = this.convertStringToTestcardMaterial(list[0]),
					Value = list[2],
					TestcardSize = this.convertStringToTestcardType(list[1]),
					NumberFoundTestballs = 0,
					NumberTestballsToBeFound = Convert.ToInt32(list[3]),
					INNUmber = list[4],
					NumberProductsRejected = 0
				});
				list = new List<string>();
			}
			this.dbConnection.GetParameterValue(this.TestcardMaterialPath3, ref text);
			bool flag3 = text != null && text != "n.a." && text != "" && text != "None";
			if (flag3)
			{
				list.Add(text);
				this.dbConnection.GetParameterValue(this.TestcardType3, ref text);
				list.Add(text);
				this.dbConnection.GetParameterValue(this.TestcardSize3, ref text);
				list.Add(text);
				this.dbConnection.GetParameterValue(this.TestcardNumberCont3, ref text);
				list.Add(text);
				this.dbConnection.GetParameterValue(this.TestcardNumber3, ref text);
				list.Add(text);
				observableCollection.Add(new InfosTestsamples
				{
					Description = this.convertStringToTestcardMaterial(list[0]),
					Value = list[2],
					TestcardSize = this.convertStringToTestcardType(list[1]),
					NumberFoundTestballs = 0,
					NumberTestballsToBeFound = Convert.ToInt32(list[3]),
					INNUmber = list[4],
					NumberProductsRejected = 0
				});
				list = new List<string>();
			}
			this.dbConnection.GetParameterValue(this.TestcardMaterialPath4, ref text);
			bool flag4 = text != null && text != "n.a." && text != "" && text != "None";
			if (flag4)
			{
				list.Add(text);
				this.dbConnection.GetParameterValue(this.TestcardType4, ref text);
				list.Add(text);
				this.dbConnection.GetParameterValue(this.TestcardSize4, ref text);
				list.Add(text);
				this.dbConnection.GetParameterValue(this.TestcardNumberCont4, ref text);
				list.Add(text);
				this.dbConnection.GetParameterValue(this.TestcardNumber4, ref text);
				list.Add(text);
				observableCollection.Add(new InfosTestsamples
				{
					Description = this.convertStringToTestcardMaterial(list[0]),
					Value = list[2],
					TestcardSize = this.convertStringToTestcardType(list[1]),
					NumberFoundTestballs = 0,
					NumberTestballsToBeFound = Convert.ToInt32(list[3]),
					INNUmber = list[4],
					NumberProductsRejected = 0
				});
				list = new List<string>();
			}
			this.dbConnection.GetParameterValue(this.TestcardMaterialPath5, ref text);
			bool flag5 = text != null && text != "n.a." && text != "" && text != "None";
			if (flag5)
			{
				list.Add(text);
				this.dbConnection.GetParameterValue(this.TestcardType5, ref text);
				list.Add(text);
				this.dbConnection.GetParameterValue(this.TestcardSize5, ref text);
				list.Add(text);
				this.dbConnection.GetParameterValue(this.TestcardNumberCont5, ref text);
				list.Add(text);
				this.dbConnection.GetParameterValue(this.TestcardNumber5, ref text);
				list.Add(text);
				observableCollection.Add(new InfosTestsamples
				{
					Description = this.convertStringToTestcardMaterial(list[0]),
					Value = list[2],
					TestcardSize = this.convertStringToTestcardType(list[1]),
					NumberFoundTestballs = 0,
					NumberTestballsToBeFound = Convert.ToInt32(list[3]),
					INNUmber = list[4],
					NumberProductsRejected = 0
				});
				list = new List<string>();
			}
			return observableCollection;
		}

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x0600047B RID: 1147 RVA: 0x0001BD78 File Offset: 0x00019F78
		// (set) Token: 0x0600047C RID: 1148 RVA: 0x0001BD90 File Offset: 0x00019F90
		public bool button2Enabled
		{
			get
			{
				return this.enabled2;
			}
			set
			{
				this.enabled2 = value;
				this.OnPropertyChanged("button2Enabled");
				bool flag = this.enabled2;
				if (flag)
				{
					bool flag2 = this.parHandling.ReadPar(Definition.contaminationsCounterEnablePath) == "1";
					if (flag2)
					{
						this.countCont = true;
					}
					else
					{
						this.countCont = false;
						this.parHandling.WriteValue(Definition.contaminationsCounterEnablePath, "1");
					}
					bool flag3 = this.Mode == AuditMode.Contamination;
					if (flag3)
					{
						this.numberTestsamples = this.TestsamplesList.Count<InfosTestsamples>();
					}
					else
					{
						this.numberTestsamples = 1;
					}
					this.RemainingProducts = this.numberProducts;
					this.RemainingTestBalls = this.numberTestsamples;
					this.ButtonNextEnabled = false;
					bool flag4 = false;
					bool flag5 = this.rejectAll;
					if (flag5)
					{
						bool flag6 = this.parHandling.ReadPar(Definition.rejectEnable1Path).Equals("1");
						if (flag6)
						{
							this.parHandling.WriteValue(Definition.rejectAll1Path, "1");
							flag4 = true;
						}
						bool flag7 = this.parHandling.ReadPar(Definition.rejectEnable2Path).Equals("1");
						if (flag7)
						{
							this.parHandling.WriteValue(Definition.rejectAll2Path, "1");
							flag4 = true;
						}
						bool flag8 = this.parHandling.ReadPar(Definition.rejectEnable3Path).Equals("1");
						if (flag8)
						{
							this.parHandling.WriteValue(Definition.rejectAll3Path, "1");
							flag4 = true;
						}
						bool flag9 = this.parHandling.ReadPar(Definition.rejectEnable4Path).Equals("1");
						if (flag9)
						{
							this.parHandling.WriteValue(Definition.rejectAll4Path, "1");
							flag4 = true;
						}
					}
					bool flag10 = !flag4 && this.rejectAll;
					if (flag10)
					{
						bool flag11 = System.Windows.Application.Current != null;
						if (flag11)
						{
							System.Windows.Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
							{
								bool flag12 = Dialog.Show(LocalizationManager.Localize("txtInfo"), string.Concat(new string[]
								{
									LocalizationManager.Localize("txtRejecterIsDisabled"),
									". ",
									LocalizationManager.Localize("txtRejectAll"),
									" ",
									LocalizationManager.Localize("txtNotPossible")
								}), DialogStyle.Info, DialogButtons.OK) == Dialog.DialogResults.Ok;
								if (flag12)
								{
									this.imageTransfer.FrameArrived += this.imageTransfer_FrameArrived;
									this.StartTest();
								}
							}));
						}
					}
					else
					{
						this.imageTransfer.FrameArrived += this.imageTransfer_FrameArrived;
						this.StartTest();
					}
				}
			}
		}

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x0600047D RID: 1149 RVA: 0x0001BFB4 File Offset: 0x0001A1B4
		// (set) Token: 0x0600047E RID: 1150 RVA: 0x0001BFCC File Offset: 0x0001A1CC
		public bool button3Enabled
		{
			get
			{
				return this.enabled3;
			}
			set
			{
				this.enabled3 = value;
				bool flag = this.enabled3;
				if (flag)
				{
					this.AuditRequired.StopRemainingTimeUpdate();
					this.AuditRequired.TimeToStartAuditElapsed -= this.AuditRequired_TimeToStartAuditElapsed;
					bool result = this.getResult();
					bool flag2 = result;
					if (flag2)
					{
						bool flag3 = System.Windows.Application.Current != null;
						if (flag3)
						{
							System.Windows.Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
							{
								Dialog.Show(LocalizationManager.Localize("txtSuccess"), LocalizationManager.Localize("txtTestOk"), DialogStyle.Ok, DialogButtons.OK);
							}));
						}
						bool flag4 = !this.cancelled;
						if (flag4)
						{
							string[] array = this.drawCertificateSign(true, false);
							bool flag5 = array == null;
							if (flag5)
							{
								bool flag6 = !this.endProcess && !this.cancelled;
								if (flag6)
								{
									this.backToStart();
								}
							}
							else
							{
								bool flag7 = !array[0].Equals("Cancelled");
								if (flag7)
								{
									this.reset();
									this.ButtonNextEnabled = true;
									bool documentationEnabled = this.documentationEnabled;
									if (documentationEnabled)
									{
										this.certificateReady(array[1]);
										this.PopUpOpen = true;
										this.resultForEnd = true;
										this.setResultError = true;
									}
									else
									{
										this.EndText = this.StrPassed.LocalizedText;
									}
								}
							}
						}
					}
					else
					{
						bool flag8 = System.Windows.Application.Current != null;
						if (flag8)
						{
							System.Windows.Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
							{
								Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtTestFailed"), DialogStyle.Warning, DialogButtons.OK);
							}));
						}
						bool flag9 = !this.cancelled;
						if (flag9)
						{
							string[] array = this.drawCertificateSign(false, false);
							bool flag10 = array != null;
							if (flag10)
							{
								bool flag11 = !array[0].Equals("Cancelled");
								if (flag11)
								{
									this.reset();
									this.ButtonNextEnabled = true;
									bool documentationEnabled2 = this.documentationEnabled;
									if (documentationEnabled2)
									{
										this.certificateReady(array[1]);
										this.PopUpOpen = true;
										this.resultForEnd = false;
										this.setResultError = true;
									}
									else
									{
										this.EndText = this.StrFailed.LocalizedText;
									}
								}
							}
							else
							{
								bool flag12 = !this.endProcess && !this.cancelled;
								if (flag12)
								{
									this.backToStart();
								}
							}
						}
					}
				}
				this.OnPropertyChanged("button3Enabled");
			}
		}

		// Token: 0x1400001E RID: 30
		// (add) Token: 0x0600047F RID: 1151 RVA: 0x0001C238 File Offset: 0x0001A438
		// (remove) Token: 0x06000480 RID: 1152 RVA: 0x0001C270 File Offset: 0x0001A470
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<string> certificateReady;

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x06000481 RID: 1153 RVA: 0x0001C2A5 File Offset: 0x0001A4A5
		// (set) Token: 0x06000482 RID: 1154 RVA: 0x0001C2AD File Offset: 0x0001A4AD
		public string success { get; set; }

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x06000483 RID: 1155 RVA: 0x0001C2B8 File Offset: 0x0001A4B8
		// (set) Token: 0x06000484 RID: 1156 RVA: 0x0001C2D0 File Offset: 0x0001A4D0
		public bool Check
		{
			get
			{
				return this.check;
			}
			set
			{
				this.check = value;
				this.OnPropertyChanged("Check");
			}
		}

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x06000485 RID: 1157 RVA: 0x0001C2E8 File Offset: 0x0001A4E8
		// (set) Token: 0x06000486 RID: 1158 RVA: 0x0001C300 File Offset: 0x0001A500
		public bool ButtonNextEnabled
		{
			get
			{
				return this.enabledNext;
			}
			set
			{
				this.enabledNext = value;
				this.OnPropertyChanged("buttonNextEnabled");
			}
		}

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x06000487 RID: 1159 RVA: 0x0001C318 File Offset: 0x0001A518
		// (set) Token: 0x06000488 RID: 1160 RVA: 0x0001C330 File Offset: 0x0001A530
		public string TestsampleToConvey
		{
			get
			{
				return this.testsampleToConvey;
			}
			set
			{
				bool flag = value != null;
				if (flag)
				{
					this.testsampleToConvey = value;
					this.OnPropertyChanged("TestsampleToConvey");
				}
			}
		}

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x06000489 RID: 1161 RVA: 0x0001C35C File Offset: 0x0001A55C
		// (set) Token: 0x0600048A RID: 1162 RVA: 0x0001C374 File Offset: 0x0001A574
		public string ProductsConveyed
		{
			get
			{
				return this.productsConveyed;
			}
			set
			{
				bool flag = value != null;
				if (flag)
				{
					this.productsConveyed = value;
					this.OnPropertyChanged("ProductsConveyed");
				}
			}
		}

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x0600048B RID: 1163 RVA: 0x0001C3A0 File Offset: 0x0001A5A0
		// (set) Token: 0x0600048C RID: 1164 RVA: 0x0001C3B8 File Offset: 0x0001A5B8
		public string TestsamplesConveyed
		{
			get
			{
				return this.testsamplesConveyed;
			}
			set
			{
				bool flag = value != null;
				if (flag)
				{
					this.testsamplesConveyed = value;
					this.OnPropertyChanged("TestsamplesConveyed");
				}
			}
		}

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x0600048D RID: 1165 RVA: 0x0001C3E4 File Offset: 0x0001A5E4
		// (set) Token: 0x0600048E RID: 1166 RVA: 0x0001C3FC File Offset: 0x0001A5FC
		public string EndText
		{
			get
			{
				return this.endText;
			}
			set
			{
				this.endText = value;
				this.OnPropertyChanged("EndText");
			}
		}

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x0600048F RID: 1167 RVA: 0x0001C414 File Offset: 0x0001A614
		// (set) Token: 0x06000490 RID: 1168 RVA: 0x0001C42C File Offset: 0x0001A62C
		public ObservableCollection<InfosTestsamples> TestsamplesList
		{
			get
			{
				return this.testsamplesList;
			}
			set
			{
				this.testsamplesList = value;
				this.OnPropertyChanged("TestsamplesList");
			}
		}

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x06000491 RID: 1169 RVA: 0x0001C442 File Offset: 0x0001A642
		// (set) Token: 0x06000492 RID: 1170 RVA: 0x0001C44A File Offset: 0x0001A64A
		public RelayCommand CleanUpCommand { get; set; }

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x06000493 RID: 1171 RVA: 0x0001C453 File Offset: 0x0001A653
		// (set) Token: 0x06000494 RID: 1172 RVA: 0x0001C45B File Offset: 0x0001A65B
		public ParameterHandling parHandling { get; set; }

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x06000495 RID: 1173 RVA: 0x0001C464 File Offset: 0x0001A664
		// (set) Token: 0x06000496 RID: 1174 RVA: 0x0001C46C File Offset: 0x0001A66C
		public MachineState machineState { get; set; }

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x06000497 RID: 1175 RVA: 0x0001C475 File Offset: 0x0001A675
		// (set) Token: 0x06000498 RID: 1176 RVA: 0x0001C47D File Offset: 0x0001A67D
		public Backup backup { get; set; }

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x06000499 RID: 1177 RVA: 0x0001C488 File Offset: 0x0001A688
		// (set) Token: 0x0600049A RID: 1178 RVA: 0x0001C4A0 File Offset: 0x0001A6A0
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

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x0600049B RID: 1179 RVA: 0x0001C4B6 File Offset: 0x0001A6B6
		// (set) Token: 0x0600049C RID: 1180 RVA: 0x0001C4BE File Offset: 0x0001A6BE
		public AuditRequired AuditRequired { get; set; }

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x0600049D RID: 1181 RVA: 0x0001C4C7 File Offset: 0x0001A6C7
		// (set) Token: 0x0600049E RID: 1182 RVA: 0x0001C4CF File Offset: 0x0001A6CF
		public Restore Restore { get; private set; }

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x0600049F RID: 1183 RVA: 0x0001C4D8 File Offset: 0x0001A6D8
		// (set) Token: 0x060004A0 RID: 1184 RVA: 0x0001C4F0 File Offset: 0x0001A6F0
		public bool RemainingTimeVisible
		{
			get
			{
				return this.remainingTimeVisible;
			}
			set
			{
				this.remainingTimeVisible = value;
				this.OnPropertyChanged("RemainingTimeVisible");
			}
		}

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x060004A1 RID: 1185 RVA: 0x0001C508 File Offset: 0x0001A708
		// (set) Token: 0x060004A2 RID: 1186 RVA: 0x0001C520 File Offset: 0x0001A720
		public bool PopUpOpen
		{
			get
			{
				return this.popUpOpen;
			}
			set
			{
				this.popUpOpen = value;
				this.OnPropertyChanged("PopUpOpen");
			}
		}

		// Token: 0x17000174 RID: 372
		// (get) Token: 0x060004A3 RID: 1187 RVA: 0x0001C536 File Offset: 0x0001A736
		// (set) Token: 0x060004A4 RID: 1188 RVA: 0x0001C53E File Offset: 0x0001A73E
		public bool documentationEnabled { get; set; }

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x060004A6 RID: 1190 RVA: 0x0001C560 File Offset: 0x0001A760
		// (set) Token: 0x060004A5 RID: 1189 RVA: 0x0001C547 File Offset: 0x0001A747
		public bool ButtonLeftEnabled
		{
			get
			{
				return this.buttonLeftEnabled;
			}
			set
			{
				this.buttonLeftEnabled = value;
				this.OnPropertyChanged("ButtonLeftEnabled");
			}
		}

		// Token: 0x060004A7 RID: 1191 RVA: 0x0001C578 File Offset: 0x0001A778
		public AuditCheckProcessViewModel(XRSImageBox imageBox, AccessRightsManager accessManager, MachineState machineState, ParameterHandling parHandl, ImageDataTransfer imageTransfer, DBConnection dbCon, MessageSystem messageSystem, AuditRequired auditRequired, Restore restore, AuditMode Mode, ProductManager productManager)
		{
			this.PopUpOpen = false;
			this.Widescreen = (Definition.windowWidth >= 1300.0);
			this.AuditRequired = auditRequired;
			this.dbConnection = dbCon;
			this.parHandling = parHandl;
			this.machineState = machineState;
			this.imageTransfer = imageTransfer;
			this.Mode = Mode;
			this.productManager = productManager;
			this.imageBox = imageBox;
			this.Restore = restore;
			this.AccessManager = accessManager;
			this.dbConnection.GetDoorLockAvailable(ref this.doorLockAvailable);
			this.rejecter = new Rejecter(this.parHandling, this.dbConnection);
			this.UnlockBinItem = this.rejecter.UnlockBinItems;
			this.documentationEnabled = false;
			bool documentationEnabled = false;
			this.dbConnection.GetAuditCheckDocumentation(ref documentationEnabled);
			this.documentationEnabled = documentationEnabled;
			this.AuditRequired.TimeToStartAuditElapsed += this.AuditRequired_TimeToStartAuditElapsed;
			this.AuditRequired.DbConnection.UpdateCurrentProductTable("LastAuditCheckDay", DateTime.Now.DayOfWeek.ToString());
			this.RegisterLocalizedText();
			this.InitValues();
			bool flag = Mode == AuditMode.Contamination;
			if (flag)
			{
				this.IsCont = true;
			}
			else
			{
				this.IsCont = false;
			}
			this.startTime = DateTime.Now;
			this.UnlockBinCommand = new RelayCommand(new Action<object>(this.UnlockBin));
			this.CleanUpCommand = new RelayCommand(new Action<object>(this.CleanUp));
			bool flag2 = machineState.State == MachineStates.Inspecting;
			if (flag2)
			{
				this.timerCheck = new DispatcherTimer(new TimeSpan(0, 0, 2), DispatcherPriority.Normal, new EventHandler(this.checkMachineState), System.Windows.Application.Current.Dispatcher);
				machineState.State = MachineStates.AuditCheck;
				this.timerCheck.Start();
			}
			else
			{
				Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtTurnXrayOn") + ".", DialogStyle.Error, DialogButtons.OK);
				this.OnClose();
			}
			this.button1Enabled = true;
			this.messageSystem = messageSystem;
			this.dbConnection.GetRejectAllAuditCheck(ref this.rejectAll);
			bool flag3 = false;
			this.dbConnection.GetStopConveyorIfError(ref flag3);
			string text = "";
			this.dbConnection.GetAuditFailedError(ref text);
			bool flag4 = flag3 && text.Equals("1");
			if (flag4)
			{
				this.endProcess = true;
				this.ButtonLeftEnabled = false;
			}
			else
			{
				this.endProcess = false;
				this.ButtonLeftEnabled = true;
			}
			bool flag5 = this.parHandling.ReadPar(Definition.detectorMonitoringActive) == "1";
			if (flag5)
			{
				this.parHandling.WriteValue(Definition.detectorMonitoringBackgroundEnable, "0");
			}
		}

		// Token: 0x060004A8 RID: 1192 RVA: 0x0001CA08 File Offset: 0x0001AC08
		private void AuditRequired_TimeToStartAuditElapsed()
		{
			this.AuditRequired.TimeToStartAuditElapsed -= this.AuditRequired_TimeToStartAuditElapsed;
			bool flag = !this.cancelled;
			if (flag)
			{
				this.cancelled = true;
				this.timeout = true;
				bool auditFailedError = this.AuditRequired.AuditFailedError;
				if (auditFailedError)
				{
					this.AuditRequired.LastAuditFailed = true;
					this.AuditRequired.LastAuditFailedSaved = true;
				}
				else
				{
					LogbookManager.AddEntry(SSTEntryCode.AuditFailedInfo);
				}
				this.AuditRequired.DbConnection.SetLastAuditCheckResult(AuditResult.Timeout);
				this.AuditRequired.DbConnection.SetAuditCheckErrorReset(false);
				this.IncreaseNAuditChecks(false);
				bool flag2 = System.Windows.Application.Current != null;
				if (flag2)
				{
					System.Windows.Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
					{
						bool flag3 = Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtTestFailed"), DialogStyle.Error, DialogButtons.OK) == Dialog.DialogResults.Ok;
						if (flag3)
						{
							this.drawCertificateSign(false, false);
							this.OnClose();
						}
					}));
				}
			}
		}

		// Token: 0x060004A9 RID: 1193 RVA: 0x0001CAE0 File Offset: 0x0001ACE0
		private void checkMachineState(object sender, EventArgs e)
		{
			bool flag = this.machineState.State != MachineStates.AuditCheck;
			if (flag)
			{
				bool flag2 = !this.cancelled;
				if (flag2)
				{
					Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtTurnXrayOn") + ".", DialogStyle.Error, DialogButtons.OK);
					this.cancelled = true;
					bool auditFailedError = this.AuditRequired.AuditFailedError;
					if (auditFailedError)
					{
						this.AuditRequired.LastAuditFailed = true;
						this.AuditRequired.LastAuditFailedSaved = true;
					}
					else
					{
						LogbookManager.AddEntry(SSTEntryCode.AuditFailedInfo);
					}
					this.AuditRequired.DbConnection.SetLastAuditCheckResult(AuditResult.NotPerformed);
					this.AuditRequired.DbConnection.SetAuditCheckErrorReset(false);
					this.IncreaseNAuditChecks(false);
					this.drawCertificateSign(false, false);
					this.OnClose();
				}
			}
		}

		// Token: 0x060004AA RID: 1194 RVA: 0x0001CBC0 File Offset: 0x0001ADC0
		private void InitValues()
		{
			string text = "";
			this.dbConnection.GetParameterValue(AuditRequired.nProductsPath, ref text);
			this.numberProducts = Convert.ToInt32(text);
			this.dbConnection.GetAuditCheckData(ref text);
			this.Data = text;
			this.numberContaminations = 1;
			this.button1Enabled = true;
			this.button2Enabled = false;
			this.button3Enabled = false;
			this.ButtonNextEnabled = true;
		}

		// Token: 0x060004AB RID: 1195 RVA: 0x0001CC30 File Offset: 0x0001AE30
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x060004AC RID: 1196 RVA: 0x0001CC60 File Offset: 0x0001AE60
		public void setResult(bool result)
		{
			this.Check = false;
			bool flag = !this.cancelled;
			if (flag)
			{
				bool flag2 = !result;
				if (flag2)
				{
					bool flag3 = this.endProcess;
					if (flag3)
					{
						bool auditFailedError = this.AuditRequired.AuditFailedError;
						if (auditFailedError)
						{
							this.AuditRequired.LastAuditFailed = true;
							this.AuditRequired.LastAuditFailedSaved = true;
						}
						else
						{
							LogbookManager.AddEntry(SSTEntryCode.AuditFailedInfo);
						}
						this.cancelled = true;
						this.AuditRequired.DbConnection.SetLastAuditCheckResult(AuditResult.NotPerformed);
						this.AuditRequired.DbConnection.SetAuditCheckErrorReset(false);
						this.IncreaseNAuditChecks(false);
						bool flag4 = System.Windows.Application.Current != null;
						if (flag4)
						{
							System.Windows.Application.Current.Dispatcher.Invoke(delegate()
							{
								bool flag8 = Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtTimeOver") + ".", DialogStyle.Error, DialogButtons.OK) == Dialog.DialogResults.Ok;
								if (flag8)
								{
									this.drawCertificateSign(false, false);
									this.OnClose();
								}
							});
						}
					}
					else
					{
						bool flag5 = System.Windows.Application.Current != null;
						if (flag5)
						{
							System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(delegate()
							{
								bool flag8 = Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtContinue"), DialogStyle.Warning, DialogButtons.OKCancel) == Dialog.DialogResults.Cancel;
								if (flag8)
								{
									this.AbortTest();
								}
								else
								{
									bool flag9 = this.RemainingProducts == 0;
									if (flag9)
									{
										this.Update(this.CurrentImage);
									}
									else
									{
										this.imageQueue.Remove(this.CurrentImage);
										bool flag10 = this.imageQueue.Count >= 1;
										if (flag10)
										{
											this.workWithImage();
										}
									}
								}
							}), new object[0]);
						}
					}
				}
				else
				{
					this.countGoodSamples++;
					bool flag6 = this.RemainingProducts == 0;
					if (flag6)
					{
						this.Update(this.CurrentImage);
					}
					else
					{
						this.imageQueue.Remove(this.CurrentImage);
						bool flag7 = this.imageQueue.Count >= 1;
						if (flag7)
						{
							this.workWithImage();
						}
					}
				}
			}
		}

		// Token: 0x060004AD RID: 1197 RVA: 0x0001CDD8 File Offset: 0x0001AFD8
		private void RegisterLocalizedText()
		{
			this.StrAuditCheck = new BoundLocalizedText("lbAuditCheck");
			this.StrData = new BoundLocalizedText("txtData");
			this.StrRequest = new BoundLocalizedText("txtRequest");
			this.StrResult = new BoundLocalizedText("txtResult");
			this.StrCheckImages = new BoundLocalizedText("txtAuditCheckImages");
			this.StrContinue = new BoundLocalizedText("txtAuditFailedContinue");
			switch (this.Mode)
			{
			case AuditMode.Contamination:
				this.StrConvey = new BoundLocalizedText("txtConveyContaminantProduct");
				this.StrSampleOk = new BoundLocalizedText("txtAuditNumberContaminationsRight");
				this.StrSampleFail = new BoundLocalizedText("txtAuditNumberContaminationsLess");
				this.AuditRequired.InTestContaminationMode = true;
				break;
			case AuditMode.Weight:
				this.StrConvey = new BoundLocalizedText("txtConveyWeightProduct");
				this.StrSampleOk = new BoundLocalizedText("txtFalseWeightDetected");
				this.StrSampleFail = new BoundLocalizedText("txtWeightRight");
				this.AuditRequired.InTestWeightMode = true;
				break;
			case AuditMode.Count:
				this.StrConvey = new BoundLocalizedText("txtConveyCountProduct");
				this.StrSampleOk = new BoundLocalizedText("txtFalseCountDetected");
				this.StrSampleFail = new BoundLocalizedText("txtCountRight");
				this.AuditRequired.InTestCountMode = true;
				break;
			}
			this.StrStartAudit = new BoundLocalizedText("lbAuditStarted");
			this.StrPassed = new BoundLocalizedText("txtPassed");
			this.StrFailed = new BoundLocalizedText("txtFailed");
			this.StrTestballs = new BoundLocalizedText("txtTestballs");
			this.StrProducts = new BoundLocalizedText("txtApplication");
			this.StrNumberContaminations = new BoundLocalizedText("txtNumberContaminations");
			this.StrTestcard = new BoundLocalizedText("txtTestcard");
			this.StrNumber = new BoundLocalizedText("txtCount");
			this.StrWeight = new BoundLocalizedText("txtWeightAuditCheck");
			this.StrEndTime = new BoundLocalizedText("txtEndTime");
			this.StrStartTime = new BoundLocalizedText("txtStartTime");
			this.StrEndResult = new BoundLocalizedText("txtEndResult");
			this.StrDocumenation = new BoundLocalizedText("txtDocumentation");
			this.StrProduct = new BoundLocalizedText("txtApplication");
			this.StrUser = new BoundLocalizedText("txtUser");
			this.StrSign = new BoundLocalizedText("txtSigned");
			this.StrProductNumber = new BoundLocalizedText("txtProductNumber");
			this.StrDeviceData = new BoundLocalizedText("txtDeviceData");
			this.StrRemainingTime = new BoundLocalizedText("txtRemainingTime");
		}

		// Token: 0x060004AE RID: 1198 RVA: 0x0001D078 File Offset: 0x0001B278
		public void DeregisterLocalizedText()
		{
			MainWindow.localizationManager.Deregister(this.StrAuditCheck);
			MainWindow.localizationManager.Deregister(this.StrData);
			MainWindow.localizationManager.Deregister(this.StrRequest);
			MainWindow.localizationManager.Deregister(this.StrResult);
			MainWindow.localizationManager.Deregister(this.StrProduct);
			MainWindow.localizationManager.Deregister(this.StrConvey);
			MainWindow.localizationManager.Deregister(this.StrStartAudit);
			MainWindow.localizationManager.Deregister(this.StrPassed);
			MainWindow.localizationManager.Deregister(this.StrFailed);
			MainWindow.localizationManager.Deregister(this.StrProducts);
			MainWindow.localizationManager.Deregister(this.StrTestballs);
			MainWindow.localizationManager.Deregister(this.StrNumberContaminations);
			MainWindow.localizationManager.Deregister(this.StrTestcard);
			MainWindow.localizationManager.Deregister(this.StrNumber);
			MainWindow.localizationManager.Deregister(this.StrWeight);
			MainWindow.localizationManager.Deregister(this.StrCheckImages);
			MainWindow.localizationManager.Deregister(this.StrEndTime);
			MainWindow.localizationManager.Deregister(this.StrStartTime);
			MainWindow.localizationManager.Deregister(this.StrEndResult);
			MainWindow.localizationManager.Deregister(this.StrDocumenation);
			MainWindow.localizationManager.Deregister(this.StrUser);
			MainWindow.localizationManager.Deregister(this.StrSign);
			MainWindow.localizationManager.Deregister(this.StrDeviceData);
			MainWindow.localizationManager.Deregister(this.StrProductNumber);
			MainWindow.localizationManager.Deregister(this.StrRemainingTime);
			MainWindow.localizationManager.Deregister(this.StrContinue);
			MainWindow.localizationManager.Deregister(this.StrSampleOk);
			MainWindow.localizationManager.Deregister(this.StrSampleFail);
		}

		// Token: 0x060004AF RID: 1199 RVA: 0x0001D264 File Offset: 0x0001B464
		public virtual void OnClose()
		{
			Action close = this.Close;
			bool flag = close != null;
			if (flag)
			{
				close();
			}
		}

		// Token: 0x060004B0 RID: 1200 RVA: 0x0001D28C File Offset: 0x0001B48C
		public void backToStart()
		{
			this.reset();
			this.AccessManager.StopTimerAutoLogout();
			this.AuditRequired.TimeToStartAuditElapsed += this.AuditRequired_TimeToStartAuditElapsed;
			this.startTime = DateTime.Now;
			this.AuditRequired.UpdateRemainingTime();
			bool flag = this.TestsamplesList != null;
			if (flag)
			{
				foreach (InfosTestsamples infosTestsamples in this.TestsamplesList)
				{
					infosTestsamples.NumberFoundTestballs = 0;
					infosTestsamples.NumberProductsRejected = 0;
				}
			}
			this.button1Enabled = true;
			this.button2Enabled = false;
			this.button3Enabled = false;
			this.ButtonNextEnabled = true;
			this.imageBox.Reset();
			this.Check = false;
		}

		// Token: 0x060004B1 RID: 1201 RVA: 0x0001D36C File Offset: 0x0001B56C
		private void AbortTest()
		{
			this.AuditRequired.StopRemainingTimeUpdate();
			this.AuditRequired.TimeToStartAuditElapsed -= this.AuditRequired_TimeToStartAuditElapsed;
			bool auditFailedError = this.AuditRequired.AuditFailedError;
			if (auditFailedError)
			{
				this.AuditRequired.LastAuditFailed = true;
				this.AuditRequired.LastAuditFailedSaved = true;
			}
			else
			{
				LogbookManager.AddEntry(SSTEntryCode.AuditFailedInfo);
			}
			this.cancelled = true;
			this.AuditRequired.DbConnection.SetLastAuditCheckResult(AuditResult.NotPerformed);
			this.AuditRequired.DbConnection.SetAuditCheckErrorReset(false);
			this.IncreaseNAuditChecks(false);
			bool flag = System.Windows.Application.Current != null;
			if (flag)
			{
				System.Windows.Application.Current.Dispatcher.Invoke(delegate()
				{
					bool flag2 = Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtTestFailed"), DialogStyle.Error, DialogButtons.OK) == Dialog.DialogResults.Ok;
					if (flag2)
					{
						this.drawCertificateSign(false, false);
						this.OnClose();
					}
				});
			}
		}

		// Token: 0x060004B2 RID: 1202 RVA: 0x0001D438 File Offset: 0x0001B638
		public bool ShowImage(XRSImage image)
		{
			bool result = false;
			bool flag = image.Type == ImageType.Audit;
			if (flag)
			{
				bool flag2 = this.isLane || this.isMultiProduct;
				if (flag2)
				{
					bool flag3 = image.SubProductName == this.nameSubproduct;
					if (flag3)
					{
						result = true;
					}
				}
				else
				{
					result = true;
				}
			}
			else
			{
				bool flag4 = image.Type != ImageType.Audit;
				if (flag4)
				{
					Dialog.Show(LocalizationManager.Localize("txtWarning"), "NOT AUDIT IMAGE", DialogStyle.Warning, DialogButtons.OK);
				}
			}
			return result;
		}

		// Token: 0x060004B3 RID: 1203 RVA: 0x0001D4C4 File Offset: 0x0001B6C4
		private void StartTest()
		{
			bool flag = this.machineState.State == MachineStates.AuditCheck;
			if (flag)
			{
				bool flag2 = this.AccessManager.CurrentUser.ID != 0;
				if (flag2)
				{
					this.AccessManager.StopTimerAutoLogout();
					this.parHandling.WriteValue(Definition.inAuditModePath, "1", false);
					this.numberOfImages = 0;
					this.ProductsConveyed = string.Concat(new string[]
					{
						this.StrProducts.LocalizedText,
						": ",
						(this.numberProducts - this.RemainingProducts).ToString(),
						"/",
						this.numberProducts.ToString()
					});
					bool flag3 = this.Mode == AuditMode.Contamination;
					if (flag3)
					{
						this.TestsampleToConvey = string.Concat(new string[]
						{
							this.ConvertMaterial(this.TestsamplesList[this.numberTestsamples - this.RemainingTestBalls].Description),
							", ",
							this.TestsamplesList[this.numberTestsamples - this.RemainingTestBalls].Value,
							" mm, ",
							this.ConvertTestcard(this.TestsamplesList[this.numberTestsamples - this.RemainingTestBalls].TestcardSize),
							", ",
							this.TestsamplesList[this.numberTestsamples - this.RemainingTestBalls].INNUmber,
							" (",
							this.StrTestcard.LocalizedText,
							" ",
							(1 + this.testsamplesList.Count<InfosTestsamples>() - this.remainingTestBalls).ToString(),
							"/",
							this.testsamplesList.Count<InfosTestsamples>().ToString(),
							")"
						});
						this.parHandling.WriteValue(Definition.contaminationsCounterEnablePath, "1");
						this.numberContaminations = this.TestsamplesList[this.numberTestsamples - this.RemainingTestBalls].NumberTestballsToBeFound;
						this.StringNumberContaminations = this.StrNumberContaminations.LocalizedText + ": " + this.numberContaminations.ToString();
					}
					else
					{
						bool flag4 = this.Mode == AuditMode.Weight;
						if (flag4)
						{
							string text = "";
							string text2 = "";
							string text3 = "";
							WeightUnit weightUnit = WeightUnit.Gramm;
							string tableName = "";
							this.dbConnection.GetCurrentProductTableName(ref tableName);
							bool flag5 = this.dbConnection.Connection != null;
							if (flag5)
							{
								this.dbConnection.GetParameterValue(Definition.strReferenceWeight, tableName, ref text);
								this.dbConnection.GetParameterValue("MinWeight", tableName, ref text2);
								this.dbConnection.GetParameterValue("MaxWeight", tableName, ref text3);
								this.dbConnection.GetWeightUnit(ref weightUnit);
							}
							this.TestsampleToConvey = string.Concat(new string[]
							{
								this.StrWeight.LocalizedText,
								": ",
								text2,
								" ",
								weightUnit.ToString(),
								" - ",
								text3,
								" ",
								weightUnit.ToString()
							});
							this.StringNumberContaminations = "";
						}
						else
						{
							bool flag6 = this.Mode == AuditMode.Count;
							if (flag6)
							{
								string tableName2 = "";
								this.dbConnection.GetCurrentProductTableName(ref tableName2);
								string str = "/objectCounter/";
								string str2 = str + "MinObjects/Value";
								string str3 = str + "MaxObjects/Value";
								string text4 = "";
								this.dbConnection.GetParameterValue(Definition.filterMainPath + str2, tableName2, ref text4);
								string text5 = "";
								this.dbConnection.GetParameterValue(Definition.filterMainPath + str3, tableName2, ref text5);
								this.TestsampleToConvey = string.Concat(new string[]
								{
									this.StrNumber.LocalizedText,
									": ",
									text4,
									" - ",
									text5
								});
								this.StringNumberContaminations = "";
							}
						}
					}
					this.UserLogAvailable = true;
					this.countGoodSamples = 0;
				}
				else
				{
					Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("msgLogin"), DialogStyle.Warning, DialogButtons.OK);
					this.backToStart();
				}
			}
			else
			{
				Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtTurnXrayOn") + ".", DialogStyle.Error, DialogButtons.OK);
				this.AbortTest();
			}
		}

		// Token: 0x060004B4 RID: 1204 RVA: 0x0001D97C File Offset: 0x0001BB7C
		private void imageTransfer_FrameArrived(object sender, ReceiveEventArgs e)
		{
			bool flag = e.Image.Type != ImageType.Histogram && e.Image.Type != ImageType.MaterialCurve && e.Image.Type != ImageType.TestcardSimulation;
			if (flag)
			{
				bool flag2 = this.ShowImage(e.Image);
				if (flag2)
				{
					this.numberOfImages++;
					bool flag3 = this.numberOfImages >= this.numberTestsamples * this.numberProducts;
					if (flag3)
					{
						this.parHandling.WriteValue(Definition.inAuditModePath, "0", false);
						this.imageTransfer.FrameArrived -= this.imageTransfer_FrameArrived;
					}
					this.imageQueue.Add(e.Image);
					bool flag4 = this.imageQueue.Count == 1;
					if (flag4)
					{
						this.workWithImage();
					}
				}
			}
		}

		// Token: 0x060004B5 RID: 1205 RVA: 0x0001DA60 File Offset: 0x0001BC60
		public void workWithImage()
		{
			bool flag = this.imageQueue != null && this.imageQueue.Count<XRSImage>() > 0;
			if (flag)
			{
				XRSImage image = this.imageQueue.First<XRSImage>();
				this.CurrentImage = image;
				this.imageBox.ViewXRSImage(image);
				int number = this.ImageConformsToTest(image);
				bool flag2 = number == 0;
				if (flag2)
				{
					bool flag3 = System.Windows.Application.Current != null;
					if (flag3)
					{
						System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(delegate()
						{
							Dialog.Show(LocalizationManager.Localize("txtInfo"), this.StrSampleOk.LocalizedText, DialogStyle.Info, DialogButtons.OK);
							this.countGoodSamples++;
							this.setResultAfterDialog(number, image);
						}), new object[0]);
					}
				}
				else
				{
					bool flag4 = number == 1;
					if (flag4)
					{
						bool flag5 = System.Windows.Application.Current != null;
						if (flag5)
						{
							System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(delegate()
							{
								bool flag8 = this.IsCont;
								if (flag8)
								{
									Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtNumberContaminationsMore"), DialogStyle.Warning, DialogButtons.OK);
								}
								else
								{
									Dialog.Show(LocalizationManager.Localize("txtWarning"), this.StrSampleOk.LocalizedText.Replace(".", "") + ". " + this.StrCheckImages.LocalizedText, DialogStyle.Warning, DialogButtons.OK);
								}
								this.Check = true;
								int num = this.RemainingProducts;
								this.RemainingProducts = num - 1;
								this.ProductsConveyed = string.Concat(new string[]
								{
									this.StrProducts.LocalizedText,
									": ",
									(this.numberProducts - this.RemainingProducts).ToString(),
									"/",
									this.numberProducts.ToString()
								});
							}), new object[0]);
						}
					}
					else
					{
						bool flag6 = this.endProcess;
						if (flag6)
						{
							this.AuditRequired.StopRemainingTimeUpdate();
							this.AuditRequired.TimeToStartAuditElapsed -= this.AuditRequired_TimeToStartAuditElapsed;
							bool auditFailedError = this.AuditRequired.AuditFailedError;
							if (auditFailedError)
							{
								this.AuditRequired.LastAuditFailed = true;
								this.AuditRequired.LastAuditFailedSaved = true;
							}
							else
							{
								LogbookManager.AddEntry(SSTEntryCode.AuditFailedInfo);
							}
							this.cancelled = true;
							this.AuditRequired.DbConnection.SetLastAuditCheckResult(AuditResult.NotPerformed);
							this.AuditRequired.DbConnection.SetAuditCheckErrorReset(false);
							this.IncreaseNAuditChecks(false);
							bool flag7 = System.Windows.Application.Current != null;
							if (flag7)
							{
								System.Windows.Application.Current.Dispatcher.Invoke(delegate()
								{
									bool flag8 = Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtTestFailed") + ". " + this.StrSampleFail.LocalizedText, DialogStyle.Error, DialogButtons.OK) == Dialog.DialogResults.Ok;
									if (flag8)
									{
										this.drawCertificateSign(false, false);
										this.OnClose();
									}
								});
							}
						}
						else
						{
							System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(delegate()
							{
								bool flag8 = Dialog.Show(LocalizationManager.Localize("txtError"), this.StrSampleFail.LocalizedText + " " + this.StrContinue.LocalizedText, DialogStyle.Error, DialogButtons.OKCancel) == Dialog.DialogResults.Cancel;
								if (flag8)
								{
									this.AbortTest();
								}
								else
								{
									this.setResultAfterDialog(number, image);
								}
							}), new object[0]);
						}
					}
				}
			}
		}

		// Token: 0x060004B6 RID: 1206 RVA: 0x0001DC68 File Offset: 0x0001BE68
		public void setResultAfterDialog(int number, XRSImage image)
		{
			bool flag = !this.cancelled;
			if (flag)
			{
				int num = this.RemainingProducts;
				this.RemainingProducts = num - 1;
				this.ProductsConveyed = string.Concat(new string[]
				{
					this.StrProducts.LocalizedText,
					": ",
					(this.numberProducts - this.RemainingProducts).ToString(),
					"/",
					this.numberProducts.ToString()
				});
				bool flag2 = number != 1;
				if (flag2)
				{
					bool flag3 = this.RemainingProducts == 0;
					if (flag3)
					{
						this.Update(image);
					}
					else
					{
						this.imageQueue.Remove(image);
						bool flag4 = this.imageQueue.Count >= 1;
						if (flag4)
						{
							this.workWithImage();
						}
					}
				}
			}
		}

		// Token: 0x060004B7 RID: 1207 RVA: 0x0001DD44 File Offset: 0x0001BF44
		public void Update(XRSImage image)
		{
			bool flag = this.IsCont;
			if (flag)
			{
				this.TestsamplesList[this.numberTestsamples - this.RemainingTestBalls].NumberFoundTestballs = this.countGoodSamples;
				LogbookManager.AddEntry(SSTEntryCode.AuditTestballFinished, 0, this.ConvertMaterial(this.TestsamplesList[this.numberTestsamples - this.RemainingTestBalls].Description), this.TestsamplesList[this.numberTestsamples - this.RemainingTestBalls].Value, this.countGoodSamples.ToString(), this.numberProducts.ToString());
				this.countGoodSamples = 0;
			}
			else
			{
				bool flag2 = this.Mode == AuditMode.Count;
				if (flag2)
				{
					LogbookManager.AddEntry(SSTEntryCode.AuditTestballFinished, 0, "Audit Check", this.TestsampleToConvey, this.countGoodSamples.ToString(), this.numberProducts.ToString());
				}
				else
				{
					LogbookManager.AddEntry(SSTEntryCode.AuditTestballFinished, 0, "Audit Check", this.TestsampleToConvey, this.countGoodSamples.ToString(), this.numberProducts.ToString());
				}
			}
			int num = this.RemainingTestBalls;
			this.RemainingTestBalls = num - 1;
			bool flag3 = this.RemainingTestBalls == 0;
			if (flag3)
			{
				this.button2Enabled = false;
				this.button3Enabled = true;
			}
			else
			{
				this.RemainingProducts = this.numberProducts;
				this.ProductsConveyed = string.Concat(new string[]
				{
					this.StrProducts.LocalizedText,
					": ",
					(this.numberProducts - this.RemainingProducts).ToString(),
					"/",
					this.numberProducts.ToString()
				});
				this.TestsampleToConvey = string.Concat(new string[]
				{
					this.ConvertMaterial(this.TestsamplesList[this.numberTestsamples - this.RemainingTestBalls].Description),
					", ",
					this.TestsamplesList[this.numberTestsamples - this.RemainingTestBalls].Value,
					" mm, ",
					this.ConvertTestcard(this.TestsamplesList[this.numberTestsamples - this.RemainingTestBalls].TestcardSize),
					", ",
					this.TestsamplesList[this.numberTestsamples - this.RemainingTestBalls].INNUmber,
					" (",
					this.StrTestcard.LocalizedText,
					" ",
					(1 + this.testsamplesList.Count<InfosTestsamples>() - this.remainingTestBalls).ToString(),
					"/",
					this.testsamplesList.Count<InfosTestsamples>().ToString(),
					")"
				});
				this.numberContaminations = this.TestsamplesList[this.numberTestsamples - this.RemainingTestBalls].NumberTestballsToBeFound;
				this.StringNumberContaminations = this.StrNumberContaminations.LocalizedText + ": " + this.numberContaminations.ToString();
				bool flag4 = System.Windows.Application.Current != null;
				if (flag4)
				{
					System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(delegate()
					{
						bool flag5 = Dialog.Show(LocalizationManager.Localize("txtInfo"), LocalizationManager.Localize("txtNextTestsample") + ": " + this.TestsampleToConvey, DialogStyle.Info, DialogButtons.OK) == Dialog.DialogResults.Ok;
						if (flag5)
						{
							this.imageQueue.Remove(image);
							bool flag6 = this.imageQueue.Count >= 1;
							if (flag6)
							{
								this.workWithImage();
							}
						}
					}), new object[0]);
				}
			}
		}

		// Token: 0x060004B8 RID: 1208 RVA: 0x0001E0AC File Offset: 0x0001C2AC
		private int ImageConformsToTest(XRSImage image)
		{
			bool flag = this.Mode == AuditMode.Contamination;
			int result;
			if (flag)
			{
				bool flag2 = (int)image.ContaminationCount == this.numberContaminations;
				if (flag2)
				{
					result = 0;
				}
				else
				{
					bool flag3 = (int)image.ContaminationCount >= this.numberContaminations;
					if (flag3)
					{
						result = 1;
					}
					else
					{
						result = -1;
					}
				}
			}
			else
			{
				bool flag4 = this.Mode == AuditMode.Count;
				if (flag4)
				{
					bool flag5 = image.HasFalseCount();
					if (flag5)
					{
						result = 1;
					}
					else
					{
						result = -1;
					}
				}
				else
				{
					bool flag6 = this.Mode == AuditMode.Weight;
					if (flag6)
					{
						bool flag7 = image.HasFalseWeight();
						if (flag7)
						{
							result = 1;
						}
						else
						{
							result = -1;
						}
					}
					else
					{
						result = -1;
					}
				}
			}
			return result;
		}

		// Token: 0x060004B9 RID: 1209 RVA: 0x0001E14C File Offset: 0x0001C34C
		public void CheckUserRightforUnlockBin(System.Windows.Controls.Button btnUnlockBin)
		{
			this.AccessManager.RegisterControl(btnUnlockBin, Rejecter.doVerifyTestRight, Rejecter.doVerifyTestRight);
		}

		// Token: 0x060004BA RID: 1210 RVA: 0x0001E166 File Offset: 0x0001C366
		public void DeregisterUnlockBinButton(System.Windows.Controls.Button btnUnlockBin)
		{
			this.AccessManager.DeregisterControl(btnUnlockBin);
		}

		// Token: 0x060004BB RID: 1211 RVA: 0x0001E178 File Offset: 0x0001C378
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

		// Token: 0x060004BC RID: 1212 RVA: 0x0001E1F0 File Offset: 0x0001C3F0
		public void reset()
		{
			this.imageTransfer.FrameArrived -= this.imageTransfer_FrameArrived;
			bool flag = !this.timeout;
			if (flag)
			{
				this.AuditRequired.StopRemainingTimeUpdate();
				this.AuditRequired.TimeToStartAuditElapsed -= this.AuditRequired_TimeToStartAuditElapsed;
			}
			this.parHandling.WriteValue(Definition.inAuditModePath, "0", false);
			bool flag2 = !this.countCont;
			if (flag2)
			{
				this.parHandling.WriteValue(Definition.contaminationsCounterEnablePath, "0");
			}
			this.parHandling.WriteValue(Definition.rejectAll1Path, "0");
			this.parHandling.WriteValue(Definition.rejectAll2Path, "0");
			this.parHandling.WriteValue(Definition.rejectAll3Path, "0");
			this.parHandling.WriteValue(Definition.rejectAll4Path, "0");
			this.CurrentImage = null;
			bool flag3 = (this.button2Enabled || this.button1Enabled) && !this.timeout && !this.cancelled;
			if (flag3)
			{
				bool auditFailedError = this.AuditRequired.AuditFailedError;
				if (auditFailedError)
				{
					this.AuditRequired.LastAuditFailed = true;
					this.AuditRequired.LastAuditFailedSaved = true;
				}
				else
				{
					LogbookManager.AddEntry(SSTEntryCode.AuditFailedInfo);
				}
				this.AuditRequired.DbConnection.SetLastAuditCheckResult(AuditResult.NotPerformed);
				this.AuditRequired.DbConnection.SetAuditCheckErrorReset(false);
				this.IncreaseNAuditChecks(false);
			}
			this.RemainingProducts = this.numberProducts;
			this.RemainingTestBalls = this.numberTestsamples;
			this.countGoodSamples = 0;
			this.AccessManager.StartTimerAutoLogout();
			bool flag4 = this.imageQueue != null;
			if (flag4)
			{
				this.imageQueue.Clear();
			}
			this.PopUpOpen = false;
		}

		// Token: 0x060004BD RID: 1213 RVA: 0x0001E3C8 File Offset: 0x0001C5C8
		private void IncreaseNAuditChecks(bool passed)
		{
			int num = 0;
			if (passed)
			{
				bool nauditChecksPassed = this.AuditRequired.DbConnection.GetNAuditChecksPassed(ref num);
				if (nauditChecksPassed)
				{
					this.AuditRequired.DbConnection.SetNAuditChecksPassed(++num);
				}
			}
			else
			{
				bool nauditChecksFailed = this.AuditRequired.DbConnection.GetNAuditChecksFailed(ref num);
				if (nauditChecksFailed)
				{
					this.AuditRequired.DbConnection.SetNAuditChecksFailed(++num);
				}
			}
		}

		// Token: 0x060004BE RID: 1214 RVA: 0x0001E43C File Offset: 0x0001C63C
		public bool getResult()
		{
			bool flag = true;
			bool flag2 = this.IsCont;
			bool result;
			if (flag2)
			{
				foreach (InfosTestsamples infosTestsamples in this.TestsamplesList)
				{
					bool flag3 = this.numberProducts != infosTestsamples.NumberFoundTestballs;
					if (flag3)
					{
						flag = false;
					}
				}
				result = flag;
			}
			else
			{
				bool flag4 = this.countGoodSamples == this.numberProducts;
				result = flag4;
			}
			return result;
		}

		// Token: 0x060004BF RID: 1215 RVA: 0x0001E4D8 File Offset: 0x0001C6D8
		private string ConvertMaterial(Material value)
		{
			string result = LocalizationManager.Localize("txtNA");
			switch (value)
			{
			case Material.VA:
				result = LocalizationManager.Localize("txtVA");
				break;
			case Material.Glass:
				result = LocalizationManager.Localize("txtGlass");
				break;
			case Material.VAWire:
				result = LocalizationManager.Localize("txtVAWire");
				break;
			case Material.Ceramics:
				result = LocalizationManager.Localize("txtKeramik");
				break;
			case Material.EPDM:
				result = LocalizationManager.Localize("txtEPDM");
				break;
			case Material.PTFE:
				result = LocalizationManager.Localize("txtPTFE");
				break;
			case Material.Aluminium:
				result = LocalizationManager.Localize("txtAluminium");
				break;
			case Material.Bone:
				result = LocalizationManager.Localize("txtBone");
				break;
			case Material.None:
				result = LocalizationManager.Localize("txtNA");
				break;
			case Material.Other:
				result = LocalizationManager.Localize("txtOther");
				break;
			case Material.FE:
				result = LocalizationManager.Localize("txtFE");
				break;
			case Material.NonFE:
				result = LocalizationManager.Localize("txtNonFE");
				break;
			}
			return result;
		}

		// Token: 0x060004C0 RID: 1216 RVA: 0x0001E5D8 File Offset: 0x0001C7D8
		private string ConvertTestcard(TestCardSize value)
		{
			string result = "";
			switch (value)
			{
			case TestCardSize.SingleBall:
				result = LocalizationManager.Localize("txtSingleBall");
				break;
			case TestCardSize.Testcard5:
				result = LocalizationManager.Localize("txtTestCard5");
				break;
			case TestCardSize.Testcard100:
				result = LocalizationManager.Localize("txtTestCard100");
				break;
			case TestCardSize.Other:
				result = LocalizationManager.Localize("txtOther");
				break;
			}
			return result;
		}

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x060004C1 RID: 1217 RVA: 0x0001E644 File Offset: 0x0001C844
		// (set) Token: 0x060004C2 RID: 1218 RVA: 0x0001E65C File Offset: 0x0001C85C
		public ObservableCollection<InfosTestsamples> ResultList
		{
			get
			{
				return this.resultList;
			}
			set
			{
				this.resultList = value;
				this.OnPropertyChanged("ResultList");
			}
		}

		// Token: 0x060004C3 RID: 1219 RVA: 0x0001E674 File Offset: 0x0001C874
		public void UpdateDateDocumentation()
		{
			this.dbConnection.SetLastDateDocumentation(DateTime.Now.ToString("dd.MM.yyyy"));
			bool flag = false;
			this.dbConnection.GetDocIsManually(ref flag);
			bool flag2 = flag;
			if (flag2)
			{
				int num = 0;
				this.dbConnection.GetDocIntervallDays(ref num);
				this.dbConnection.SetNextDateDocumentation(DateTime.Now.AddDays((double)num).ToString("dd.MM.yyyy"));
			}
			else
			{
				this.dbConnection.GetDocIsDaily(ref flag);
				bool flag3 = flag;
				if (flag3)
				{
					this.dbConnection.SetNextDateDocumentation(DateTime.Now.AddDays(1.0).ToString("dd.MM.yyyy"));
				}
				else
				{
					this.dbConnection.SetNextDateDocumentation(DateTime.Now.AddDays(7.0).ToString("dd.MM.yyyy"));
				}
			}
		}

		// Token: 0x060004C4 RID: 1220 RVA: 0x0001E76C File Offset: 0x0001C96C
		public string[] drawCertificate(bool passed)
		{
			string text = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/AuditcheckDocumentation/CurrentDoc/";
			string str = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/AuditcheckDocumentation/OtherDocs/";
			string text2 = "";
			this.dbConnection.GetNextDateDocumentation(ref text2);
			bool flag = text2.Length > 0;
			if (flag)
			{
				int day = Convert.ToInt32(text2.Substring(0, 2));
				int month = Convert.ToInt32(text2.Substring(3, 2));
				int year = Convert.ToInt32(text2.Substring(6, 4));
				DateTime t = new DateTime(year, month, day);
				bool flag2 = t <= DateTime.Now;
				if (flag2)
				{
					bool flag3 = File.Exists(text + "AuditCheck.pdf");
					if (flag3)
					{
						File.Copy(text + "AuditCheck.pdf", str + "AuditCheck" + DateTime.Now.ToShortDateString().Replace(".", "_").Replace("/", "_").Replace(":", "_") + ".pdf");
						File.Delete(text + "AuditCheck.pdf");
						this.UpdateDateDocumentation();
					}
				}
			}
			PdfDocument pdfDocument = new PdfDocument();
			PdfPage pdfPage = new PdfPage();
			bool flag4 = File.Exists(text + "AuditCheck.pdf");
			if (flag4)
			{
				PdfDocument pdfDocument2 = PdfReader.Open(text + "/AuditCheck.pdf", PdfDocumentOpenMode.Import);
				for (int i = 0; i < pdfDocument2.PageCount; i++)
				{
					pdfPage = pdfDocument.AddPage(pdfDocument2.Pages[i]);
				}
				pdfDocument2.Close();
				pdfDocument2.Dispose();
			}
			else
			{
				pdfDocument = new PdfDocument();
				this.dbConnection.SetYPositionAudit(0);
				pdfPage = pdfDocument.AddPage();
				try
				{
					bool flag5 = !Directory.Exists(text);
					if (flag5)
					{
						Directory.CreateDirectory(text);
					}
				}
				catch (Exception)
				{
				}
			}
			XSize xsize = PageSizeConverter.ToSize(PageSize.A4);
			pdfPage.Height = xsize.Height;
			pdfPage.Width = xsize.Width;
			pdfPage.Orientation = PageOrientation.Landscape;
			int num = 0;
			int num2 = 50;
			int num3 = 800;
			int num4 = 83;
			this.dbConnection.GetYPositionAudit(ref num);
			bool flag6 = this.IsCont;
			int num5;
			if (flag6)
			{
				num5 = 15 * this.testsamplesList.Count + 10;
				bool flag7 = num5 < 45;
				if (flag7)
				{
					num5 = 45;
				}
			}
			else
			{
				num5 = 45;
			}
			bool flag8 = num + num5 > 550;
			if (flag8)
			{
				num = 50;
				pdfPage = pdfDocument.AddPage();
				pdfPage.Height = xsize.Height;
				pdfPage.Width = xsize.Width;
				pdfPage.Orientation = PageOrientation.Landscape;
			}
			XGraphics xgraphics = XGraphics.FromPdfPage(pdfPage);
			XFont font = new XFont("Calibri", 9.0);
			XFont font2 = new XFont("Calibri", 40.0);
			XPen pen = new XPen(XColors.Black);
			XTextFormatter xtextFormatter = new XTextFormatter(xgraphics);
			bool flag9 = num <= 50;
			if (flag9)
			{
				bool flag10 = num == 0;
				if (flag10)
				{
					xgraphics.DrawString(this.StrAuditCheck.LocalizedText + " " + this.StrDocumenation.LocalizedText, font2, XBrushes.Black, 50.0, 50.0);
					xtextFormatter.DrawString(this.Data, font, XBrushes.Black, new XRect((double)num2, 100.0, 750.0, 50.0));
					num = 200;
				}
				int num6 = 30;
				xgraphics.DrawLine(pen, (double)num2, (double)num, (double)num3, (double)num);
				xgraphics.DrawLine(pen, (double)num2, (double)num, (double)num2, (double)(num + num6));
				xgraphics.DrawLine(pen, (double)num3, (double)num, (double)num3, (double)(num + num6));
				xgraphics.DrawLine(pen, (double)num2, (double)(num + num6), (double)num3, (double)(num + num6));
				int num7 = num2 + num4;
				for (int j = 0; j < 8; j++)
				{
					bool flag11 = j != 5;
					if (flag11)
					{
						xgraphics.DrawLine(pen, (double)num7, (double)num, (double)num7, (double)(num + num6));
					}
					num7 += num4;
				}
				xtextFormatter.DrawString(this.StrStartTime.LocalizedText, font, XBrushes.Black, new XRect((double)(num2 + 5), (double)(num + 5), (double)(num2 + num4 - 5), (double)(num + num6 - 5)));
				xtextFormatter.DrawString(this.StrEndTime.LocalizedText, font, XBrushes.Black, new XRect((double)(num2 + num4 + 5), (double)(num + 5), (double)(num2 + 2 * num4 - 5), (double)(num + num6 - 5)));
				xtextFormatter.DrawString(this.StrUser.LocalizedText, font, XBrushes.Black, new XRect((double)(num2 + 2 * num4 + 5), (double)(num + 5), (double)(num2 + 3 * num4 - 5), (double)(num + num6 - 5)));
				xtextFormatter.DrawString(this.StrProduct.LocalizedText, font, XBrushes.Black, new XRect((double)(num2 + 3 * num4 + 5), (double)(num + 5), (double)(num4 - 10), (double)(num6 - 10)));
				xtextFormatter.DrawString(this.StrNumber.LocalizedText, font, XBrushes.Black, new XRect((double)(num2 + 4 * num4 + 5), (double)(num + 5), (double)(num4 - 10), (double)(num6 - 10)));
				xtextFormatter.DrawString(this.StrTestballs.LocalizedText, font, XBrushes.Black, new XRect((double)(num2 + 5 * num4 + 5), (double)(num + 5), (double)(num4 - 10), (double)(num6 - 10)));
				xtextFormatter.DrawString(this.StrResult.LocalizedText, font, XBrushes.Black, new XRect((double)(num2 + 7 * num4 + 5), (double)(num + 5), (double)(num4 - 10), (double)(num6 - 10)));
				xtextFormatter.DrawString(this.StrEndResult.LocalizedText, font, XBrushes.Black, new XRect((double)(num2 + 8 * num4 + 5), (double)(num + 5), (double)(num4 - 10), (double)(num6 - 10)));
				num += num6;
			}
			string text3 = "";
			List<string[]> list = new List<string[]>();
			xgraphics.DrawLine(pen, (double)num2, (double)num, (double)num2, (double)(num + num5));
			xgraphics.DrawLine(pen, (double)num3, (double)num, (double)num3, (double)(num + num5));
			xgraphics.DrawLine(pen, (double)num2, (double)(num + num5), (double)num3, (double)(num + num5));
			int num8 = num2 + num4;
			for (int k = 0; k < 8; k++)
			{
				bool flag12 = k != 5;
				if (flag12)
				{
					xgraphics.DrawLine(pen, (double)num8, (double)num, (double)num8, (double)(num + num5));
				}
				num8 += num4;
			}
			xtextFormatter.DrawString(this.startTime.ToString(), font, XBrushes.Black, new XRect((double)(num2 + 5), (double)(num + 5), (double)(num4 - 10), (double)(num5 - 10)));
			text3 = DateTime.Now.ToString();
			xtextFormatter.DrawString(text3, font, XBrushes.Black, new XRect((double)(num2 + num4 + 5), (double)(num + 5), (double)(num4 - 10), (double)(num5 - 10)));
			xtextFormatter.DrawString(this.AccessManager.CurrentUser.Username + "\n" + this.AccessManager.CurrentUser.ID.ToString(), font, XBrushes.Black, new XRect((double)(num2 + 2 * num4 + 5), (double)(num + 5), (double)(num4 - 10), (double)(num5 - 10)));
			xtextFormatter.DrawString(string.Concat(new string[]
			{
				this.productManager.CurrentLoadedProduct.ProductName,
				"\nP: ",
				this.productManager.CurrentLoadedProduct.ProductNumber,
				"\n",
				this.productManager.CurrentLoadedProduct.CurrentBatchName
			}), font, XBrushes.Black, new XRect((double)(num2 + 3 * num4 + 5), (double)(num + 5), (double)(num4 - 10), (double)(num5 - 10)));
			xtextFormatter.DrawString(this.numberProducts.ToString(), font, XBrushes.Black, new XRect((double)(num2 + 4 * num4 + 5), (double)(num + 5), (double)(num4 - 10), (double)(num5 - 10)));
			bool flag13 = true;
			bool flag14 = this.isCont;
			if (flag14)
			{
				int num9 = (num5 - 10) / this.TestsamplesList.Count<InfosTestsamples>();
				int num10 = num;
				foreach (InfosTestsamples infosTestsamples in this.TestsamplesList)
				{
					xtextFormatter.DrawString(string.Concat(new string[]
					{
						this.ConvertMaterial(infosTestsamples.Description),
						",",
						infosTestsamples.Value,
						" mm,",
						this.ConvertTestcard(infosTestsamples.TestcardSize),
						",",
						infosTestsamples.INNUmber
					}), font, XBrushes.Black, new XRect((double)(num2 + 5 * num4 + 5), (double)(num10 + 5), (double)(num4 * 2 - 10), (double)num9));
					bool flag15 = infosTestsamples.NumberFoundTestballs != this.numberProducts;
					if (flag15)
					{
						flag13 = false;
					}
					xtextFormatter.DrawString(infosTestsamples.NumberFoundTestballs.ToString() + "/" + this.numberProducts.ToString(), font, XBrushes.Black, new XRect((double)(num2 + 7 * num4 + 5), (double)(num10 + 5), (double)(num4 - 10), (double)num9));
					list.Add(new string[]
					{
						string.Concat(new string[]
						{
							this.ConvertMaterial(infosTestsamples.Description),
							", ",
							infosTestsamples.Value,
							" mm, ",
							this.ConvertTestcard(infosTestsamples.TestcardSize),
							", ",
							infosTestsamples.INNUmber
						}),
						infosTestsamples.NumberFoundTestballs.ToString() + "/" + this.numberProducts.ToString()
					});
					num10 += num9;
				}
			}
			else
			{
				string[] array = new string[2];
				bool flag16 = this.countGoodSamples != this.numberProducts;
				if (flag16)
				{
					flag13 = false;
				}
				xtextFormatter.DrawString(this.countGoodSamples.ToString() + "/" + this.numberProducts.ToString(), font, XBrushes.Black, new XRect((double)(num2 + 7 * num4 + 5), (double)(num + 5), (double)(num4 - 10), (double)(num5 - 10)));
				array[1] = this.countGoodSamples.ToString() + "/" + this.numberProducts.ToString();
				bool flag17 = this.Mode == AuditMode.Count;
				if (flag17)
				{
					xtextFormatter.DrawString(this.StrNumber.LocalizedText + " " + this.StrAuditCheck.LocalizedText, font, XBrushes.Black, new XRect((double)(num2 + 5 * num4 + 5), (double)(num + 5), (double)(num4 * 2 - 10), (double)(num5 - 10)));
					array[0] = this.StrNumber.LocalizedText + " " + this.StrAuditCheck.LocalizedText;
				}
				else
				{
					xtextFormatter.DrawString(this.StrWeight.LocalizedText + " " + this.StrAuditCheck.LocalizedText, font, XBrushes.Black, new XRect((double)(num2 + 5 * num4 + 5), (double)(num + 5), (double)(num4 * 2 - 10), (double)(num5 - 10)));
					array[0] = this.StrWeight.LocalizedText + " " + this.StrAuditCheck.LocalizedText;
				}
				list.Add(array);
			}
			while (list.Count < 5)
			{
				list.Add(new string[]
				{
					"",
					""
				});
			}
			bool flag18 = !flag13;
			if (flag18)
			{
				xgraphics.DrawRectangle(XBrushes.Red, (double)(num2 + 8 * num4 + 1), (double)(num + 1), (double)(num4 + 1), (double)(num5 - 2));
				xtextFormatter.DrawString(this.StrFailed.LocalizedText, font, XBrushes.Black, new XRect((double)(num2 + 8 * num4 + 5), (double)(num + 5), (double)(num4 - 10), (double)(num5 - 10)));
			}
			else
			{
				xgraphics.DrawRectangle(XBrushes.LightGreen, (double)(num2 + 8 * num4 + 1), (double)(num + 1), (double)(num4 + 1), (double)(num5 - 2));
				xtextFormatter.DrawString(this.StrPassed.LocalizedText, font, XBrushes.Black, new XRect((double)(num2 + 8 * num4 + 5), (double)(num + 5), (double)(num4 - 10), (double)(num5 - 10)));
			}
			this.dbConnection.SetAuditEntry(this.startTime.ToString(), text3, this.AccessManager.CurrentUser.Username, this.AccessManager.CurrentUser.ID.ToString(), this.productManager.CurrentLoadedProduct.ProductName, this.productManager.CurrentLoadedProduct.ProductNumber, this.productManager.CurrentLoadedProduct.CurrentBatchName, this.numberProducts.ToString(), list[0][0], list[0][1], list[1][0], list[1][1], list[2][0], list[2][1], list[3][0], list[3][1], list[4][0], list[4][1], flag13 ? "1" : "0");
			this.dbConnection.SetYPositionAudit(num + num5);
			string text4 = "AuditCheck";
			string str2 = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\AuditcheckDocumentation\\CurrentDoc\\";
			pdfDocument.Save(str2 + text4 + ".pdf");
			pdfDocument.Close();
			pdfDocument.Dispose();
			if (passed)
			{
				LogbookManager.AddEntry(SSTEntryCode.AuditOK);
				this.AuditRequired.DbConnection.SetLastAuditCheckResult(AuditResult.Passed);
				this.AuditRequired.DbConnection.SetAuditCheckErrorReset(false);
				this.IncreaseNAuditChecks(true);
			}
			else
			{
				bool auditFailedError = this.AuditRequired.AuditFailedError;
				if (auditFailedError)
				{
					this.AuditRequired.LastAuditFailed = true;
					this.AuditRequired.LastAuditFailedSaved = true;
				}
				else
				{
					LogbookManager.AddEntry(SSTEntryCode.AuditFailedInfo);
				}
				this.AuditRequired.DbConnection.SetLastAuditCheckResult(AuditResult.Failed);
				this.AuditRequired.DbConnection.SetAuditCheckErrorReset(false);
				this.IncreaseNAuditChecks(false);
			}
			return new string[]
			{
				text4,
				str2 + text4 + ".pdf"
			};
		}

		// Token: 0x060004C5 RID: 1221 RVA: 0x0001F6FC File Offset: 0x0001D8FC
		public string[] drawCertificateSign(bool passed, bool writeLogbook)
		{
			List<string[]> list = new List<string[]>();
			ObservableCollection<InfosTestsamples> observableCollection = new ObservableCollection<InfosTestsamples>();
			string text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
			bool flag = this.isCont;
			if (flag)
			{
				foreach (InfosTestsamples infosTestsamples in this.TestsamplesList)
				{
					bool flag2 = infosTestsamples.TestcardSize == TestCardSize.Testcard5;
					if (flag2)
					{
						list.Add(new string[]
						{
							string.Concat(new string[]
							{
								this.ConvertMaterial(infosTestsamples.Description),
								", ",
								infosTestsamples.Value,
								" mm, 5x, ",
								infosTestsamples.INNUmber
							}),
							infosTestsamples.NumberFoundTestballs.ToString() + "/" + this.numberProducts.ToString()
						});
					}
					else
					{
						list.Add(new string[]
						{
							string.Concat(new string[]
							{
								this.ConvertMaterial(infosTestsamples.Description),
								", ",
								infosTestsamples.Value,
								" mm, ",
								this.ConvertTestcard(infosTestsamples.TestcardSize),
								", ",
								infosTestsamples.INNUmber
							}),
							infosTestsamples.NumberFoundTestballs.ToString() + "/" + this.numberProducts.ToString()
						});
					}
					observableCollection.Add(new InfosTestsamples
					{
						Value = infosTestsamples.NumberFoundTestballs.ToString() + "/" + this.numberProducts.ToString(),
						Density = string.Concat(new string[]
						{
							this.ConvertMaterial(infosTestsamples.Description),
							",",
							infosTestsamples.Value,
							" mm,",
							infosTestsamples.INNUmber
						}),
						INNUmber = this.ConvertTestcard(infosTestsamples.TestcardSize)
					});
				}
			}
			else
			{
				string[] array = new string[]
				{
					null,
					this.countGoodSamples.ToString() + "/" + this.numberProducts.ToString()
				};
				bool flag3 = this.Mode == AuditMode.Count;
				if (flag3)
				{
					array[0] = this.StrAuditCheck.LocalizedText + " " + this.TestsampleToConvey;
				}
				else
				{
					array[0] = this.StrAuditCheck.LocalizedText + " " + this.TestsampleToConvey;
				}
				list.Add(array);
				observableCollection.Add(new InfosTestsamples
				{
					Value = array[1],
					Density = array[0],
					INNUmber = ""
				});
			}
			while (list.Count < 5)
			{
				list.Add(new string[]
				{
					"",
					""
				});
			}
			bool documentationEnabled = this.documentationEnabled;
			string[] result;
			if (documentationEnabled)
			{
				SignatureWindow signatureWindow = new SignatureWindow();
				signatureWindow.Show();
				MemoryStream memoryStream = new MemoryStream();
				Signature signature = new Signature();
				signature.StartPosition = FormStartPosition.CenterScreen;
				DialogResult dialogResult = signature.ShowDialog();
				signatureWindow.Close();
				bool flag4 = dialogResult == DialogResult.OK;
				if (flag4)
				{
					XImage ximage = null;
					memoryStream = signature.SignatureEnd;
					bool flag5 = memoryStream != null;
					if (flag5)
					{
						ximage = XImage.FromStream(memoryStream);
					}
					signatureWindow.Close();
					memoryStream.Dispose();
					signature.SignatureEnd.Dispose();
					signature.Dispose();
					this.dbConnection.SetAuditEntry(this.startTime.ToString("yyyy-MM-dd HH:mm:ss"), text, this.AccessManager.CurrentUser.Username, this.AccessManager.CurrentUser.ID.ToString(), this.productManager.CurrentLoadedProduct.ProductName, this.productManager.CurrentLoadedProduct.ProductNumber, this.productManager.CurrentLoadedProduct.CurrentBatchName, this.numberProducts.ToString(), list[0][0], list[0][1], list[1][0], list[1][1], list[2][0], list[2][1], list[3][0], list[3][1], list[4][0], list[4][1], passed ? "1" : "0");
					string text2 = "";
					this.dbConnection.GetSerialNumber(ref text2);
					int num = 0;
					this.dbConnection.GetNumAuditCheckEntries(ref num);
					bool flag6 = num > 1500;
					if (flag6)
					{
						this.dbConnection.DeleteOldestAuditCheckEntries(num - 1500);
					}
					string text3 = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/AuditcheckDocumentation/CurrentDoc/";
					string text4 = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/AuditcheckDocumentation/OtherDocs/";
					string text5 = "";
					int num2 = 0;
					this.dbConnection.GetNextDateDocumentation(ref text5);
					bool flag7 = text5.Length > 0;
					if (flag7)
					{
						bool flag8 = text5.Length >= 10;
						int day;
						int month;
						int num3;
						if (flag8)
						{
							day = Convert.ToInt32(text5.Substring(0, 2));
							month = Convert.ToInt32(text5.Substring(3, 2));
							num3 = Convert.ToInt32(text5.Substring(6, 4));
						}
						else
						{
							day = Convert.ToInt32(text5.Substring(0, 1));
							month = Convert.ToInt32(text5.Substring(2, 2));
							num3 = Convert.ToInt32(text5.Substring(5, 4));
						}
						DateTime t = new DateTime(num3, month, day);
						bool flag9 = t <= DateTime.Now;
						if (flag9)
						{
							bool flag10 = File.Exists(text3 + "AuditCheckCurrent.pdf");
							if (flag10)
							{
								bool flag11 = !Directory.Exists(text4);
								if (flag11)
								{
									Directory.CreateDirectory(text4);
								}
								File.Copy(text3 + "AuditCheckCurrent.pdf", string.Concat(new string[]
								{
									text4,
									"AuditCheck_",
									DateTime.Now.ToString("yyyyMMdd_HHmmss").Replace(".", "_").Replace("/", "_").Replace(":", "_"),
									"_",
									text2,
									".pdf"
								}));
								File.Delete(text3 + "AuditCheckCurrent.pdf");
								DirectoryInfo directoryInfo = new DirectoryInfo(text4);
								FileInfo[] files = directoryInfo.GetFiles();
								long num4 = files.Sum((FileInfo f) => f.Length) / 1024L;
								Array.Sort<FileInfo>(files, (FileInfo f1, FileInfo f2) => f1.Name.CompareTo(f2.Name));
								while (num4 > 15000L)
								{
									try
									{
										files.First<FileInfo>().Delete();
										files = directoryInfo.GetFiles();
										num4 = files.Sum((FileInfo f) => f.Length) / 1024L;
									}
									catch
									{
										break;
									}
								}
							}
							this.UpdateDateDocumentation();
							this.dbConnection.SetYPositionAudit(0);
						}
						else
						{
							bool flag12 = num3 == 2999;
							if (flag12)
							{
								this.dbConnection.SetLastDateDocumentation(DateTime.Now.ToString("dd.MM.yyyy"));
							}
						}
					}
					PdfDocument pdfDocument = new PdfDocument();
					PdfPage pdfPage = new PdfPage();
					bool flag13 = File.Exists(text3 + "AuditCheckCurrent.pdf");
					if (flag13)
					{
						pdfDocument = PdfReader.Open(text3 + "/AuditCheckCurrent.pdf", PdfDocumentOpenMode.Modify);
						num2 = pdfDocument.Pages.Count;
						pdfPage = pdfDocument.Pages[pdfDocument.Pages.Count - 1];
					}
					else
					{
						pdfDocument = new PdfDocument();
						this.dbConnection.SetYPositionAudit(0);
						pdfPage = pdfDocument.AddPage();
						try
						{
							bool flag14 = !Directory.Exists(text3);
							if (flag14)
							{
								Directory.CreateDirectory(text3);
							}
						}
						catch (Exception)
						{
						}
					}
					XSize xsize = PageSizeConverter.ToSize(PageSize.A4);
					pdfPage.Height = xsize.Height;
					pdfPage.Width = xsize.Width;
					pdfPage.Orientation = PageOrientation.Landscape;
					int num5 = 0;
					int num6 = 50;
					int num7 = 800;
					int num8 = 65;
					this.dbConnection.GetYPositionAudit(ref num5);
					bool flag15 = this.IsCont;
					int num9;
					if (flag15)
					{
						num9 = 15 * this.testsamplesList.Count + 10;
						bool flag16 = num9 < 45;
						if (flag16)
						{
							num9 = 45;
						}
					}
					else
					{
						num9 = 45;
					}
					bool flag17 = num5 + num9 > 550;
					if (flag17)
					{
						num5 = 80;
						pdfPage = pdfDocument.AddPage();
						pdfPage.Height = xsize.Height;
						pdfPage.Width = xsize.Width;
						pdfPage.Orientation = PageOrientation.Landscape;
					}
					XGraphics xgraphics = XGraphics.FromPdfPage(pdfPage);
					XFont font = new XFont("Calibri", 8.5);
					XFont font2 = new XFont("Calibri", 40.0);
					XPen pen = new XPen(XColors.Black);
					XTextFormatter xtextFormatter = new XTextFormatter(xgraphics);
					bool flag18 = num5 <= 80;
					if (flag18)
					{
						string text6 = "";
						string text7 = "";
						string text8 = "";
						ushort num10 = 0;
						ushort num11 = 0;
						this.dbConnection.GetDeviceName(ref text6);
						this.dbConnection.GetLineName(ref text7);
						MachineType machineType = MachineType.MachineTypeUndefined;
						this.dbConnection.GetMachineType(ref machineType);
						bool flag19 = this.dbConnection.GetDesign() == Definition.GUIDesign.Bizerba;
						if (flag19)
						{
							bool flag20 = machineType == MachineType.RayconXRS;
							if (flag20)
							{
								text8 = "XRE";
							}
							else
							{
								bool flag21 = machineType == MachineType.RayconD;
								if (flag21)
								{
									text8 = "XRE-D";
								}
								else
								{
									bool flag22 = machineType == MachineType.RayconEntry;
									if (flag22)
									{
										text8 = "XRE-D";
									}
									else
									{
										bool flag23 = machineType == MachineType.RayconDPlus;
										if (flag23)
										{
											text8 = "XRE-D pro";
										}
										else
										{
											bool flag24 = machineType == MachineType.RayconEX2A;
											if (flag24)
											{
												text8 = "Raycon EX2A";
											}
										}
									}
								}
							}
						}
						else
						{
							bool flag25 = machineType == MachineType.RayconXRS;
							if (flag25)
							{
								text8 = "Raycon XRS";
							}
							else
							{
								bool flag26 = machineType == MachineType.RayconD;
								if (flag26)
								{
									text8 = "Raycon D";
								}
								else
								{
									bool flag27 = machineType == MachineType.RayconEntry;
									if (flag27)
									{
										text8 = "Raycon EX1";
									}
									else
									{
										bool flag28 = machineType == MachineType.RayconDPlus;
										if (flag28)
										{
											text8 = "Raycon D+";
										}
										else
										{
											bool flag29 = machineType == MachineType.RayconEX2A;
											if (flag29)
											{
												text8 = "Raycon EX2A";
											}
										}
									}
								}
							}
						}
						this.dbConnection.GetInspectionWidth(ref num11);
						this.dbConnection.GetInspectionHeight(ref num10);
						bool flag30 = num5 == 0;
						if (flag30)
						{
							Definition.GUIDesign design = this.dbConnection.GetDesign();
							bool flag31 = design == Definition.GUIDesign.Bizerba;
							if (flag31)
							{
								Bitmap bizerba_powered_by_sesotec = Resources.bizerba_powered_by_sesotec;
								MemoryStream stream = new MemoryStream();
								bizerba_powered_by_sesotec.Save(stream, ImageFormat.Png);
								xgraphics.DrawImage(XImage.FromStream(stream), 50.0, 68.5, 200.0, 50.0);
							}
							else
							{
								Bitmap suS_logo = Resources.SuS_logo;
								MemoryStream stream2 = new MemoryStream();
								suS_logo.Save(stream2, ImageFormat.Png);
								xgraphics.DrawImage(XImage.FromStream(stream2), 50.0, 60.0, 200.0, 48.0);
							}
							xgraphics.DrawString(this.StrAuditCheck.LocalizedText, font2, XBrushes.Black, 275.0, 95.0);
							string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/AuditcheckDocumentation/Icon/";
							bool flag32 = Directory.Exists(path);
							if (flag32)
							{
								string[] files2 = Directory.GetFiles(path);
								foreach (string text9 in files2)
								{
									bool flag33 = text9.EndsWith(Definition.pngFileEnding);
									if (flag33)
									{
										XImage ximage2 = XImage.FromFile(text9);
										float num12 = (float)ximage2.PixelHeight;
										float num13 = (float)ximage2.PixelWidth;
										bool flag34 = num13 / num12 > 4f;
										if (flag34)
										{
											float num14 = num13 / 200f;
											num13 = 200f;
											num12 /= num14;
										}
										else
										{
											float num14 = num12 / 50f;
											num12 = 50f;
											num13 /= num14;
										}
										xgraphics.DrawImage(ximage2, (double)(800f - num13), 60.0, (double)num13, (double)num12);
										break;
									}
								}
							}
							string text10 = string.Concat(new string[]
							{
								this.StrDeviceData.LocalizedText,
								":\n",
								text8,
								" ",
								num11.ToString(),
								"/",
								num10.ToString(),
								"\n",
								text6,
								"\n",
								text7,
								"\n",
								text2
							});
							xtextFormatter.DrawString(this.Data, font, XBrushes.Black, new XRect((double)(num6 + 270), 125.0, 470.0, 80.0));
							xgraphics.DrawRectangle(XPens.Black, XBrushes.Transparent, new XRect((double)(num6 + 260), 120.0, 490.0, 80.0));
							xtextFormatter.DrawString(text10, font, XBrushes.Black, new XRect((double)(num6 + 10), 125.0, 230.0, 80.0));
							xgraphics.DrawRectangle(XPens.Black, XBrushes.Transparent, new XRect((double)num6, 120.0, 250.0, 80.0));
							num5 = 220;
						}
						else
						{
							XFont font3 = new XFont("Calibri", 12.0);
							Definition.GUIDesign design2 = this.dbConnection.GetDesign();
							bool flag35 = design2 == Definition.GUIDesign.Bizerba;
							if (flag35)
							{
								Bitmap bizerba_powered_by_sesotec2 = Resources.bizerba_powered_by_sesotec;
								MemoryStream stream3 = new MemoryStream();
								bizerba_powered_by_sesotec2.Save(stream3, ImageFormat.Png);
								xgraphics.DrawImage(XImage.FromStream(stream3), 650.0, 42.0, 62.5, 15.6);
							}
							else
							{
								Bitmap suS_logo2 = Resources.SuS_logo;
								MemoryStream stream4 = new MemoryStream();
								suS_logo2.Save(stream4, ImageFormat.Png);
								xgraphics.DrawImage(XImage.FromStream(stream4), 650.0, 39.0, 62.5, 15.0);
							}
							xgraphics.DrawString(text6 + "; " + text2, font3, XBrushes.Black, 60.0, 50.0);
							xgraphics.DrawString(this.StrAuditCheck.LocalizedText, font3, XBrushes.Black, 715.0, 50.0);
							xgraphics.DrawLine(new XPen(XColor.FromArgb(184, 228, 9), 2.0), (double)num6, 60.0, (double)num7, 60.0);
						}
						int num15 = 30;
						xgraphics.DrawLine(pen, (double)num6, (double)num5, (double)num7, (double)num5);
						xgraphics.DrawLine(pen, (double)num6, (double)num5, (double)num6, (double)(num5 + num15));
						xgraphics.DrawLine(pen, (double)num7, (double)num5, (double)num7, (double)(num5 + num15));
						xgraphics.DrawLine(pen, (double)num6, (double)(num5 + num15), (double)num7, (double)(num5 + num15));
						double num16 = (double)(num6 + num8);
						for (int j = 0; j < 9; j++)
						{
							bool flag36 = j != 5;
							if (flag36)
							{
								xgraphics.DrawLine(pen, num16, (double)num5, num16, (double)(num5 + num15));
								num16 += (double)num8;
							}
							else
							{
								num16 += (double)num8 * 2.5;
							}
						}
						xtextFormatter.DrawString((num2 + 1).ToString(), font, XBrushes.Black, new XRect(780.0, 570.0, 30.0, 30.0));
						xtextFormatter.DrawString(this.StrStartTime.LocalizedText, font, XBrushes.Black, new XRect((double)(num6 + 3), (double)(num5 + 5), (double)(num6 + num8 - 5), (double)(num5 + num15 - 5)));
						xtextFormatter.DrawString(this.StrEndTime.LocalizedText, font, XBrushes.Black, new XRect((double)(num6 + num8 + 3), (double)(num5 + 5), (double)(num6 + 2 * num8 - 5), (double)(num5 + num15 - 5)));
						xtextFormatter.DrawString(this.StrUser.LocalizedText, font, XBrushes.Black, new XRect((double)(num6 + 2 * num8 + 3), (double)(num5 + 5), (double)(num6 + 3 * num8 - 5), (double)(num5 + num15 - 5)));
						xtextFormatter.DrawString(this.StrProduct.LocalizedText, font, XBrushes.Black, new XRect((double)(num6 + 3 * num8 + 3), (double)(num5 + 5), (double)(num8 - 10), (double)(num15 - 10)));
						xtextFormatter.DrawString(this.StrNumber.LocalizedText, font, XBrushes.Black, new XRect((double)(num6 + 4 * num8 + 3), (double)(num5 + 5), (double)(num8 - 10), (double)(num15 - 10)));
						xtextFormatter.DrawString(this.StrTestballs.LocalizedText, font, XBrushes.Black, new XRect((double)(num6 + 5 * num8 + 3), (double)(num5 + 5), (double)(num8 - 10), (double)(num15 - 10)));
						xtextFormatter.DrawString(this.StrResult.LocalizedText, font, XBrushes.Black, new XRect((double)num6 + 8.5 * (double)num8 + 3.0, (double)(num5 + 5), (double)(num8 - 10), (double)(num15 - 10)));
						xtextFormatter.DrawString(this.StrEndResult.LocalizedText, font, XBrushes.Black, new XRect((double)num6 + 9.5 * (double)num8 + 3.0, (double)(num5 + 5), (double)(num8 - 10), (double)(num15 - 10)));
						xtextFormatter.DrawString(this.StrSign.LocalizedText, font, XBrushes.Black, new XRect((double)num6 + 10.5 * (double)num8 + 3.0, (double)(num5 + 5), (double)(num8 - 10), (double)(num15 - 10)));
						num5 += num15;
					}
					xgraphics.DrawLine(pen, (double)num6, (double)num5, (double)num6, (double)(num5 + num9));
					xgraphics.DrawLine(pen, (double)num7, (double)num5, (double)num7, (double)(num5 + num9));
					xgraphics.DrawLine(pen, (double)num6, (double)(num5 + num9), (double)num7, (double)(num5 + num9));
					double num17 = (double)(num6 + num8);
					for (int k = 0; k < 9; k++)
					{
						bool flag37 = k != 5;
						if (flag37)
						{
							xgraphics.DrawLine(pen, num17, (double)num5, num17, (double)(num5 + num9));
							num17 += (double)num8;
						}
						else
						{
							num17 += (double)num8 * 2.5;
						}
					}
					xtextFormatter.DrawString(this.startTime.ToString("yyyy-MM-dd HH:mm:ss"), font, XBrushes.Black, new XRect((double)(num6 + 3), (double)(num5 + 5), (double)(num8 - 10), (double)(num9 - 10)));
					xtextFormatter.DrawString(text, font, XBrushes.Black, new XRect((double)(num6 + num8 + 3), (double)(num5 + 5), (double)(num8 - 10), (double)(num9 - 10)));
					string text11 = this.AccessManager.CurrentUser.Username;
					double num18 = xgraphics.MeasureString(text11, font).Width / (double)(num8 - 10);
					bool flag38 = num18 > 1.0;
					if (flag38)
					{
						int num19 = (int)((double)text11.Length / num18);
						int length = num19 - 1;
						text11 = text11.Substring(0, length);
					}
					xtextFormatter.DrawString(text11 + "\nID: " + this.AccessManager.CurrentUser.ID.ToString(), font, XBrushes.Black, new XRect((double)(num6 + 2 * num8 + 3), (double)(num5 + 5), (double)(num8 - 10), (double)(num9 - 10)));
					string text12 = this.productManager.CurrentLoadedProduct.ProductName;
					num18 = xgraphics.MeasureString(text12, font).Width / (double)(num8 - 10);
					bool flag39 = num18 > 1.0;
					if (flag39)
					{
						int num20 = (int)((double)text12.Length / num18);
						int length2 = num20;
						text12 = text12.Substring(0, length2);
					}
					string text13 = this.productManager.CurrentLoadedProduct.CurrentBatchName;
					bool flag40 = text13 != null && text13.Length > 0;
					if (flag40)
					{
						num18 = xgraphics.MeasureString(text13, font).Width / (double)(num8 - 10);
						bool flag41 = num18 > 1.0;
						if (flag41)
						{
							int num21 = (int)((double)text13.Length / num18);
							int length3 = num21;
							text13 = text13.Substring(0, length3);
						}
					}
					else
					{
						text13 = "";
					}
					xtextFormatter.DrawString(string.Concat(new string[]
					{
						text12,
						"\n",
						this.StrProductNumber.LocalizedText,
						": ",
						this.productManager.CurrentLoadedProduct.ProductNumber,
						"\n",
						text13
					}), font, XBrushes.Black, new XRect((double)(num6 + 3 * num8 + 3), (double)(num5 + 5), (double)(num8 - 10), (double)(num9 - 10)));
					xtextFormatter.DrawString(this.numberProducts.ToString(), font, XBrushes.Black, new XRect((double)(num6 + 4 * num8 + 3), (double)(num5 + 5), (double)(num8 - 10), (double)(num9 - 10)));
					bool flag42 = this.isCont;
					if (flag42)
					{
						int num22 = (num9 - 10) / this.TestsamplesList.Count<InfosTestsamples>();
						int num23 = num5;
						foreach (string[] array3 in list)
						{
							bool flag43 = !array3[0].Equals("");
							if (flag43)
							{
								string text14 = array3[0];
								double num24 = xgraphics.MeasureString(text14, font).Width / ((double)num8 * 3.5 - 10.0);
								bool flag44 = num24 > 1.0;
								if (flag44)
								{
									int num25 = (int)((double)text14.Length / num24);
									int length4 = num25 - 1;
									text14 = text14.Substring(0, length4);
								}
								xtextFormatter.DrawString(text14, font, XBrushes.Black, new XRect((double)(num6 + 5 * num8 + 5), (double)(num23 + 5), (double)num8 * 3.5 - 10.0, (double)num22));
								xtextFormatter.DrawString(array3[1], font, XBrushes.Black, new XRect((double)num6 + 8.5 * (double)num8 + 3.0, (double)(num23 + 5), (double)(num8 - 10), (double)num22));
								num23 += num22;
							}
						}
					}
					else
					{
						xtextFormatter.DrawString(list.First<string[]>()[1], font, XBrushes.Black, new XRect((double)num6 + 8.5 * (double)num8 + 3.0, (double)(num5 + 5), (double)(num8 - 10), (double)(num9 - 10)));
						xtextFormatter.DrawString(list.First<string[]>()[0], font, XBrushes.Black, new XRect((double)(num6 + 5 * num8 + 3), (double)(num5 + 5), (double)num8 * 3.5 - 10.0, (double)(num9 - 10)));
					}
					bool flag45 = !passed;
					if (flag45)
					{
						xgraphics.DrawRectangle(XBrushes.Red, (double)num6 + 9.5 * (double)num8 + 1.0, (double)(num5 + 1), (double)(num8 - 2), (double)(num9 - 2));
						xtextFormatter.DrawString(this.StrFailed.LocalizedText, font, XBrushes.Black, new XRect((double)num6 + 9.5 * (double)num8 + 3.0, (double)(num5 + 5), (double)(num8 - 10), (double)(num9 - 10)));
					}
					else
					{
						xgraphics.DrawRectangle(XPens.Transparent, new XSolidBrush(XColor.FromArgb(184, 228, 9)), (double)num6 + 9.5 * (double)num8 + 1.0, (double)(num5 + 1), (double)(num8 - 2), (double)(num9 - 2));
						xtextFormatter.DrawString(this.StrPassed.LocalizedText, font, XBrushes.Black, new XRect((double)num6 + 9.5 * (double)num8 + 3.0, (double)(num5 + 5), (double)(num8 - 10), (double)(num9 - 10)));
					}
					bool flag46 = ximage != null;
					if (flag46)
					{
						xgraphics.DrawImage(ximage, (double)num6 + 10.5 * (double)num8, (double)num5, (double)num8, (double)num9);
					}
					this.dbConnection.SetYPositionAudit(num5 + num9);
					string text15 = "AuditCheckCurrent";
					string str = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\AuditcheckDocumentation\\CurrentDoc\\";
					pdfDocument.Save(str + text15 + ".pdf");
					pdfDocument.Close();
					pdfDocument.Dispose();
					if (writeLogbook)
					{
						bool flag47 = passed;
						if (flag47)
						{
							LogbookManager.AddEntry(SSTEntryCode.AuditOK);
							this.AuditRequired.DbConnection.SetLastAuditCheckResult(AuditResult.Passed);
							this.AuditRequired.DbConnection.SetAuditCheckErrorReset(false);
							this.IncreaseNAuditChecks(true);
						}
						else
						{
							bool auditFailedError = this.AuditRequired.AuditFailedError;
							if (auditFailedError)
							{
								this.AuditRequired.LastAuditFailed = true;
								this.AuditRequired.LastAuditFailedSaved = true;
							}
							else
							{
								LogbookManager.AddEntry(SSTEntryCode.AuditFailedInfo);
							}
							this.AuditRequired.DbConnection.SetLastAuditCheckResult(AuditResult.Failed);
							this.AuditRequired.DbConnection.SetAuditCheckErrorReset(false);
							this.IncreaseNAuditChecks(false);
						}
					}
					result = new string[]
					{
						text15,
						str + text15 + ".pdf"
					};
				}
				else
				{
					passed = false;
					this.dbConnection.SetAuditEntry(this.startTime.ToString("yyyy-MM-dd HH:mm:ss"), text, this.AccessManager.CurrentUser.Username, this.AccessManager.CurrentUser.ID.ToString(), this.productManager.CurrentLoadedProduct.ProductName, this.productManager.CurrentLoadedProduct.ProductNumber, this.productManager.CurrentLoadedProduct.CurrentBatchName, this.numberProducts.ToString(), list[0][0], list[0][1], list[1][0], list[1][1], list[2][0], list[2][1], list[3][0], list[3][1], list[4][0], list[4][1], passed ? "1" : "0");
					int num26 = 0;
					this.dbConnection.GetNumAuditCheckEntries(ref num26);
					bool flag48 = num26 > 1500;
					if (flag48)
					{
						this.dbConnection.DeleteOldestAuditCheckEntries(num26 - 1500);
					}
					if (writeLogbook)
					{
						bool auditFailedError2 = this.AuditRequired.AuditFailedError;
						if (auditFailedError2)
						{
							this.AuditRequired.LastAuditFailed = true;
							this.AuditRequired.LastAuditFailedSaved = true;
						}
						else
						{
							LogbookManager.AddEntry(SSTEntryCode.AuditFailedInfo);
						}
						this.AuditRequired.DbConnection.SetLastAuditCheckResult(AuditResult.Failed);
						this.AuditRequired.DbConnection.SetAuditCheckErrorReset(false);
						this.IncreaseNAuditChecks(false);
					}
					bool flag49 = System.Windows.Application.Current != null;
					if (flag49)
					{
						System.Windows.Application.Current.Dispatcher.Invoke(delegate()
						{
							Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtTestFailed"), DialogStyle.Error, DialogButtons.OK);
						});
					}
					this.cancelled = true;
					this.OnClose();
					result = null;
				}
			}
			else
			{
				this.dbConnection.SetAuditEntry(this.startTime.ToString("yyyy-MM-dd HH:mm:ss"), text, this.AccessManager.CurrentUser.Username, this.AccessManager.CurrentUser.ID.ToString(), this.productManager.CurrentLoadedProduct.ProductName, this.productManager.CurrentLoadedProduct.ProductNumber, this.productManager.CurrentLoadedProduct.CurrentBatchName, this.numberProducts.ToString(), list[0][0], list[0][1], list[1][0], list[1][1], list[2][0], list[2][1], list[3][0], list[3][1], list[4][0], list[4][1], passed ? "1" : "0");
				int num27 = 0;
				this.dbConnection.GetNumAuditCheckEntries(ref num27);
				bool flag50 = num27 > 1500;
				if (flag50)
				{
					this.dbConnection.DeleteOldestAuditCheckEntries(num27 - 1500);
				}
				if (writeLogbook)
				{
					bool flag51 = passed;
					if (flag51)
					{
						LogbookManager.AddEntry(SSTEntryCode.AuditOK, 0, "");
						this.AuditRequired.DbConnection.SetLastAuditCheckResult(AuditResult.Passed);
						this.AuditRequired.DbConnection.SetAuditCheckErrorReset(false);
						this.IncreaseNAuditChecks(true);
					}
					else
					{
						bool auditFailedError3 = this.AuditRequired.AuditFailedError;
						if (auditFailedError3)
						{
							this.AuditRequired.LastAuditFailed = true;
							this.AuditRequired.LastAuditFailedSaved = true;
						}
						else
						{
							LogbookManager.AddEntry(SSTEntryCode.AuditFailedInfo);
						}
						this.AuditRequired.DbConnection.SetLastAuditCheckResult(AuditResult.Failed);
						this.AuditRequired.DbConnection.SetAuditCheckErrorReset(false);
						this.IncreaseNAuditChecks(false);
					}
				}
				this.ResultList = observableCollection;
				result = new string[]
				{
					"",
					""
				};
			}
			return result;
		}

		// Token: 0x060004C6 RID: 1222 RVA: 0x00021684 File Offset: 0x0001F884
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

		// Token: 0x060004C7 RID: 1223 RVA: 0x000217D4 File Offset: 0x0001F9D4
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

		// Token: 0x060004C8 RID: 1224 RVA: 0x00021834 File Offset: 0x0001FA34
		public void CleanUp(object obj)
		{
			bool flag = this.parHandling.ReadPar(Definition.detectorMonitoringActive) == "1";
			if (flag)
			{
				this.parHandling.WriteValue(Definition.detectorMonitoringBackgroundEnable, "1");
			}
			this.parHandling.WriteValue(Definition.inAuditModePath, "0", false);
			bool flag2 = this.timerCheck != null;
			if (flag2)
			{
				this.timerCheck.Stop();
			}
			this.DeregisterLocalizedText();
			this.reset();
			bool flag3 = this.TestsamplesList != null;
			if (flag3)
			{
				this.TestsamplesList.Clear();
				this.TestsamplesList = null;
			}
			bool flag4 = this.ResultList != null;
			if (flag4)
			{
				this.ResultList.Clear();
				this.ResultList = null;
			}
			bool flag5 = this.imageQueue != null;
			if (flag5)
			{
				this.imageQueue.Clear();
				this.imageQueue = null;
			}
			bool flag6 = this.UnlockBinItem != null;
			if (flag6)
			{
				this.UnlockBinItem.Clear();
				this.UnlockBinItem = null;
			}
			bool flag7 = this.machineState.State == MachineStates.AuditCheck;
			if (flag7)
			{
				this.machineState.State = MachineStates.Inspecting;
			}
			bool flag8 = this.setResultError;
			if (flag8)
			{
				bool flag9 = this.resultForEnd;
				if (flag9)
				{
					LogbookManager.AddEntry(SSTEntryCode.AuditOK);
					this.AuditRequired.DbConnection.SetLastAuditCheckResult(AuditResult.Passed);
					this.AuditRequired.DbConnection.SetAuditCheckErrorReset(false);
					this.IncreaseNAuditChecks(true);
				}
				else
				{
					bool auditFailedError = this.AuditRequired.AuditFailedError;
					if (auditFailedError)
					{
						this.AuditRequired.LastAuditFailed = true;
						this.AuditRequired.LastAuditFailedSaved = true;
					}
					else
					{
						LogbookManager.AddEntry(SSTEntryCode.AuditFailedInfo);
					}
					this.AuditRequired.DbConnection.SetLastAuditCheckResult(AuditResult.Failed);
					this.AuditRequired.DbConnection.SetAuditCheckErrorReset(false);
					this.IncreaseNAuditChecks(false);
				}
			}
		}

		// Token: 0x060004C9 RID: 1225 RVA: 0x00021A28 File Offset: 0x0001FC28
		public void EndResultAuditCheck()
		{
			bool flag = !this.enabled3;
			if (flag)
			{
				bool auditFailedError = this.AuditRequired.AuditFailedError;
				if (auditFailedError)
				{
					this.AuditRequired.LastAuditFailed = true;
					this.AuditRequired.LastAuditFailedSaved = true;
				}
				else
				{
					LogbookManager.AddEntry(SSTEntryCode.AuditFailedInfo);
				}
				this.AuditRequired.DbConnection.SetLastAuditCheckResult(AuditResult.NotPerformed);
				this.AuditRequired.DbConnection.SetAuditCheckErrorReset(false);
				this.IncreaseNAuditChecks(false);
				this.drawCertificateSign(false, false);
				this.cancelled = true;
			}
		}

		// Token: 0x04000325 RID: 805
		public static readonly string minWeightSubPath = "/weigher/MinWeight/Value";

		// Token: 0x04000326 RID: 806
		public static readonly string maxWeightSubPath = "/weigher/MaxWeight/Value";

		// Token: 0x04000327 RID: 807
		private bool cancelled = false;

		// Token: 0x04000328 RID: 808
		private readonly string TestcardMaterialPath1 = "TestcardMaterial1";

		// Token: 0x04000329 RID: 809
		private readonly string TestcardType1 = "TestcardType1";

		// Token: 0x0400032A RID: 810
		private readonly string TestcardSize1 = "TestcardSize1";

		// Token: 0x0400032B RID: 811
		private readonly string TestcardNumberCont1 = "TestcardNumberCont1";

		// Token: 0x0400032C RID: 812
		private readonly string TestcardNumber1 = "TestcardNumber1";

		// Token: 0x0400032D RID: 813
		private readonly string TestcardMaterialPath2 = "TestcardMaterial2";

		// Token: 0x0400032E RID: 814
		private readonly string TestcardType2 = "TestcardType2";

		// Token: 0x0400032F RID: 815
		private readonly string TestcardSize2 = "TestcardSize2";

		// Token: 0x04000330 RID: 816
		private readonly string TestcardNumberCont2 = "TestcardNumberCont2";

		// Token: 0x04000331 RID: 817
		private readonly string TestcardNumber2 = "TestcardNumber2";

		// Token: 0x04000332 RID: 818
		private readonly string TestcardMaterialPath3 = "TestcardMaterial3";

		// Token: 0x04000333 RID: 819
		private readonly string TestcardType3 = "TestcardType3";

		// Token: 0x04000334 RID: 820
		private readonly string TestcardSize3 = "TestcardSize3";

		// Token: 0x04000335 RID: 821
		private readonly string TestcardNumberCont3 = "TestcardNumberCont3";

		// Token: 0x04000336 RID: 822
		private readonly string TestcardNumber3 = "TestcardNumber3";

		// Token: 0x04000337 RID: 823
		private readonly string TestcardMaterialPath4 = "TestcardMaterial4";

		// Token: 0x04000338 RID: 824
		private readonly string TestcardType4 = "TestcardType4";

		// Token: 0x04000339 RID: 825
		private readonly string TestcardSize4 = "TestcardSize4";

		// Token: 0x0400033A RID: 826
		private readonly string TestcardNumberCont4 = "TestcardNumberCont4";

		// Token: 0x0400033B RID: 827
		private readonly string TestcardNumber4 = "TestcardNumber4";

		// Token: 0x0400033C RID: 828
		private readonly string TestcardMaterialPath5 = "TestcardMaterial5";

		// Token: 0x0400033D RID: 829
		private readonly string TestcardType5 = "TestcardType5";

		// Token: 0x0400033E RID: 830
		private readonly string TestcardSize5 = "TestcardSize5";

		// Token: 0x0400033F RID: 831
		private readonly string TestcardNumberCont5 = "TestcardNumberCont5";

		// Token: 0x04000340 RID: 832
		private readonly string TestcardNumber5 = "TestcardNumber5";

		// Token: 0x0400035E RID: 862
		public Keyboard keyboard;

		// Token: 0x0400035F RID: 863
		private XRSImage currentImage;

		// Token: 0x04000360 RID: 864
		private DateTime startTime;

		// Token: 0x04000361 RID: 865
		private DBConnection dbConnection;

		// Token: 0x04000362 RID: 866
		private bool countCont;

		// Token: 0x04000365 RID: 869
		public AuditMode Mode = AuditMode.Contamination;

		// Token: 0x04000366 RID: 870
		private ImageDataTransfer imageTransfer;

		// Token: 0x04000367 RID: 871
		private int remainingProducts;

		// Token: 0x04000368 RID: 872
		private int numberContaminations;

		// Token: 0x04000369 RID: 873
		private string stringNumberContaminations;

		// Token: 0x0400036A RID: 874
		private int numberProducts;

		// Token: 0x0400036B RID: 875
		private int numberTestsamples;

		// Token: 0x0400036C RID: 876
		private bool userLogAvailable;

		// Token: 0x0400036D RID: 877
		public int remainingTestBalls;

		// Token: 0x0400036E RID: 878
		private bool contextUnlockBinVisibility = false;

		// Token: 0x04000370 RID: 880
		private Rejecter rejecter;

		// Token: 0x04000372 RID: 882
		private int countGoodSamples = 0;

		// Token: 0x04000373 RID: 883
		private bool widescreen;

		// Token: 0x04000374 RID: 884
		private List<XRSImage> imageQueue = new List<XRSImage>();

		// Token: 0x04000375 RID: 885
		private bool isCont = false;

		// Token: 0x04000376 RID: 886
		private bool isNotCont = false;

		// Token: 0x04000377 RID: 887
		private string textStep1 = "";

		// Token: 0x04000379 RID: 889
		private bool enabled1;

		// Token: 0x0400037A RID: 890
		private bool enabled2;

		// Token: 0x0400037B RID: 891
		private bool enabled3;

		// Token: 0x0400037E RID: 894
		private bool check;

		// Token: 0x0400037F RID: 895
		private bool enabledNext;

		// Token: 0x04000380 RID: 896
		private string testsampleToConvey;

		// Token: 0x04000381 RID: 897
		private string productsConveyed;

		// Token: 0x04000382 RID: 898
		private string testsamplesConveyed;

		// Token: 0x04000383 RID: 899
		private string endText = "";

		// Token: 0x04000384 RID: 900
		public ObservableCollection<InfosTestsamples> testsamplesList;

		// Token: 0x04000388 RID: 904
		private DispatcherTimer timerCheck;

		// Token: 0x0400038A RID: 906
		private string nameSubproduct = "";

		// Token: 0x0400038B RID: 907
		private bool isLane = false;

		// Token: 0x0400038C RID: 908
		private bool isMultiProduct = false;

		// Token: 0x0400038D RID: 909
		private int numberOfImages = 0;

		// Token: 0x0400038E RID: 910
		private MessageSystem messageSystem;

		// Token: 0x0400038F RID: 911
		private bool doorLockAvailable;

		// Token: 0x04000392 RID: 914
		private ProductManager productManager;

		// Token: 0x04000393 RID: 915
		private bool timeout = false;

		// Token: 0x04000394 RID: 916
		private bool remainingTimeVisible;

		// Token: 0x04000395 RID: 917
		private bool popUpOpen = false;

		// Token: 0x04000397 RID: 919
		private bool rejectAll = false;

		// Token: 0x04000398 RID: 920
		private XRSImageBox imageBox;

		// Token: 0x04000399 RID: 921
		private bool endProcess = false;

		// Token: 0x0400039A RID: 922
		private bool buttonLeftEnabled = true;

		// Token: 0x0400039B RID: 923
		private bool setResultError = false;

		// Token: 0x0400039C RID: 924
		private bool resultForEnd = false;

		// Token: 0x0400039D RID: 925
		private ObservableCollection<InfosTestsamples> resultList = new ObservableCollection<InfosTestsamples>();
	}
}
