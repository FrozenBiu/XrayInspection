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
	// Token: 0x0200001C RID: 28
	public class BulkAuditCheckProcessViewModel : INotifyPropertyChanged
	{
		// Token: 0x14000011 RID: 17
		// (add) Token: 0x060001EA RID: 490 RVA: 0x0000B9C0 File Offset: 0x00009BC0
		// (remove) Token: 0x060001EB RID: 491 RVA: 0x0000B9F8 File Offset: 0x00009BF8
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x060001EC RID: 492 RVA: 0x0000BA2D File Offset: 0x00009C2D
		// (set) Token: 0x060001ED RID: 493 RVA: 0x0000BA35 File Offset: 0x00009C35
		public BoundLocalizedText StrData { get; set; }

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x060001EE RID: 494 RVA: 0x0000BA3E File Offset: 0x00009C3E
		// (set) Token: 0x060001EF RID: 495 RVA: 0x0000BA46 File Offset: 0x00009C46
		public BoundLocalizedText StrRequest { get; set; }

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x060001F0 RID: 496 RVA: 0x0000BA4F File Offset: 0x00009C4F
		// (set) Token: 0x060001F1 RID: 497 RVA: 0x0000BA57 File Offset: 0x00009C57
		public BoundLocalizedText StrResult { get; set; }

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x060001F2 RID: 498 RVA: 0x0000BA60 File Offset: 0x00009C60
		// (set) Token: 0x060001F3 RID: 499 RVA: 0x0000BA68 File Offset: 0x00009C68
		public BoundLocalizedText StrAuditCheck { get; set; }

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x060001F4 RID: 500 RVA: 0x0000BA71 File Offset: 0x00009C71
		// (set) Token: 0x060001F5 RID: 501 RVA: 0x0000BA79 File Offset: 0x00009C79
		public BoundLocalizedText StrProduct { get; set; }

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x060001F6 RID: 502 RVA: 0x0000BA82 File Offset: 0x00009C82
		// (set) Token: 0x060001F7 RID: 503 RVA: 0x0000BA8A File Offset: 0x00009C8A
		public BoundLocalizedText StrSampleOk { get; set; }

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x060001F8 RID: 504 RVA: 0x0000BA93 File Offset: 0x00009C93
		// (set) Token: 0x060001F9 RID: 505 RVA: 0x0000BA9B File Offset: 0x00009C9B
		public BoundLocalizedText StrSampleFail { get; set; }

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x060001FA RID: 506 RVA: 0x0000BAA4 File Offset: 0x00009CA4
		// (set) Token: 0x060001FB RID: 507 RVA: 0x0000BAAC File Offset: 0x00009CAC
		public BoundLocalizedText StrContinue { get; set; }

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x060001FC RID: 508 RVA: 0x0000BAB5 File Offset: 0x00009CB5
		// (set) Token: 0x060001FD RID: 509 RVA: 0x0000BABD File Offset: 0x00009CBD
		public BoundLocalizedText StrCheckImages { get; set; }

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x060001FE RID: 510 RVA: 0x0000BAC6 File Offset: 0x00009CC6
		// (set) Token: 0x060001FF RID: 511 RVA: 0x0000BACE File Offset: 0x00009CCE
		public BoundLocalizedText StrConvey { get; set; }

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x06000200 RID: 512 RVA: 0x0000BAD7 File Offset: 0x00009CD7
		// (set) Token: 0x06000201 RID: 513 RVA: 0x0000BADF File Offset: 0x00009CDF
		public BoundLocalizedText StrStartAudit { get; set; }

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x06000202 RID: 514 RVA: 0x0000BAE8 File Offset: 0x00009CE8
		// (set) Token: 0x06000203 RID: 515 RVA: 0x0000BAF0 File Offset: 0x00009CF0
		public BoundLocalizedText StrPassed { get; set; }

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x06000204 RID: 516 RVA: 0x0000BAF9 File Offset: 0x00009CF9
		// (set) Token: 0x06000205 RID: 517 RVA: 0x0000BB01 File Offset: 0x00009D01
		public BoundLocalizedText StrFailed { get; set; }

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000206 RID: 518 RVA: 0x0000BB0A File Offset: 0x00009D0A
		// (set) Token: 0x06000207 RID: 519 RVA: 0x0000BB12 File Offset: 0x00009D12
		public BoundLocalizedText StrTestballs { get; set; }

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x06000208 RID: 520 RVA: 0x0000BB1B File Offset: 0x00009D1B
		// (set) Token: 0x06000209 RID: 521 RVA: 0x0000BB23 File Offset: 0x00009D23
		public BoundLocalizedText StrProducts { get; set; }

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x0600020A RID: 522 RVA: 0x0000BB2C File Offset: 0x00009D2C
		// (set) Token: 0x0600020B RID: 523 RVA: 0x0000BB34 File Offset: 0x00009D34
		public BoundLocalizedText StrNumberContaminations { get; set; }

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x0600020C RID: 524 RVA: 0x0000BB3D File Offset: 0x00009D3D
		// (set) Token: 0x0600020D RID: 525 RVA: 0x0000BB45 File Offset: 0x00009D45
		public BoundLocalizedText StrStartTime { get; set; }

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x0600020E RID: 526 RVA: 0x0000BB4E File Offset: 0x00009D4E
		// (set) Token: 0x0600020F RID: 527 RVA: 0x0000BB56 File Offset: 0x00009D56
		public BoundLocalizedText StrEndTime { get; set; }

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x06000210 RID: 528 RVA: 0x0000BB5F File Offset: 0x00009D5F
		// (set) Token: 0x06000211 RID: 529 RVA: 0x0000BB67 File Offset: 0x00009D67
		public BoundLocalizedText StrTestcard { get; set; }

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x06000212 RID: 530 RVA: 0x0000BB70 File Offset: 0x00009D70
		// (set) Token: 0x06000213 RID: 531 RVA: 0x0000BB78 File Offset: 0x00009D78
		public BoundLocalizedText StrWeight { get; set; }

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x06000214 RID: 532 RVA: 0x0000BB81 File Offset: 0x00009D81
		// (set) Token: 0x06000215 RID: 533 RVA: 0x0000BB89 File Offset: 0x00009D89
		public BoundLocalizedText StrNumber { get; set; }

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x06000216 RID: 534 RVA: 0x0000BB92 File Offset: 0x00009D92
		// (set) Token: 0x06000217 RID: 535 RVA: 0x0000BB9A File Offset: 0x00009D9A
		public BoundLocalizedText StrEndResult { get; set; }

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x06000218 RID: 536 RVA: 0x0000BBA3 File Offset: 0x00009DA3
		// (set) Token: 0x06000219 RID: 537 RVA: 0x0000BBAB File Offset: 0x00009DAB
		public BoundLocalizedText StrDocumenation { get; set; }

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x0600021A RID: 538 RVA: 0x0000BBB4 File Offset: 0x00009DB4
		// (set) Token: 0x0600021B RID: 539 RVA: 0x0000BBBC File Offset: 0x00009DBC
		public BoundLocalizedText StrUser { get; set; }

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x0600021C RID: 540 RVA: 0x0000BBC5 File Offset: 0x00009DC5
		// (set) Token: 0x0600021D RID: 541 RVA: 0x0000BBCD File Offset: 0x00009DCD
		public BoundLocalizedText StrSign { get; set; }

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x0600021E RID: 542 RVA: 0x0000BBD6 File Offset: 0x00009DD6
		// (set) Token: 0x0600021F RID: 543 RVA: 0x0000BBDE File Offset: 0x00009DDE
		public BoundLocalizedText StrProductNumber { get; set; }

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x06000220 RID: 544 RVA: 0x0000BBE7 File Offset: 0x00009DE7
		// (set) Token: 0x06000221 RID: 545 RVA: 0x0000BBEF File Offset: 0x00009DEF
		public BoundLocalizedText StrDeviceData { get; set; }

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x06000222 RID: 546 RVA: 0x0000BBF8 File Offset: 0x00009DF8
		// (set) Token: 0x06000223 RID: 547 RVA: 0x0000BC00 File Offset: 0x00009E00
		public BoundLocalizedText StrRemainingTime { get; set; }

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x06000224 RID: 548 RVA: 0x0000BC0C File Offset: 0x00009E0C
		// (set) Token: 0x06000225 RID: 549 RVA: 0x0000BC24 File Offset: 0x00009E24
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

		// Token: 0x14000012 RID: 18
		// (add) Token: 0x06000226 RID: 550 RVA: 0x0000BC58 File Offset: 0x00009E58
		// (remove) Token: 0x06000227 RID: 551 RVA: 0x0000BC90 File Offset: 0x00009E90
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action Close;

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x06000228 RID: 552 RVA: 0x0000BCC5 File Offset: 0x00009EC5
		// (set) Token: 0x06000229 RID: 553 RVA: 0x0000BCCD File Offset: 0x00009ECD
		public AccessRightsManager AccessManager { get; set; }

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x0600022A RID: 554 RVA: 0x0000BCD8 File Offset: 0x00009ED8
		// (set) Token: 0x0600022B RID: 555 RVA: 0x0000BCF0 File Offset: 0x00009EF0
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

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x0600022C RID: 556 RVA: 0x0000BD24 File Offset: 0x00009F24
		// (set) Token: 0x0600022D RID: 557 RVA: 0x0000BD3C File Offset: 0x00009F3C
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

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x0600022E RID: 558 RVA: 0x0000BD70 File Offset: 0x00009F70
		// (set) Token: 0x0600022F RID: 559 RVA: 0x0000BD88 File Offset: 0x00009F88
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

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x06000230 RID: 560 RVA: 0x0000BDBC File Offset: 0x00009FBC
		// (set) Token: 0x06000231 RID: 561 RVA: 0x0000BDD4 File Offset: 0x00009FD4
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

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x06000232 RID: 562 RVA: 0x0000BE07 File Offset: 0x0000A007
		// (set) Token: 0x06000233 RID: 563 RVA: 0x0000BE0F File Offset: 0x0000A00F
		public ObservableCollection<RejecterItem> UnlockBinItem { get; set; }

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x06000234 RID: 564 RVA: 0x0000BE18 File Offset: 0x0000A018
		// (set) Token: 0x06000235 RID: 565 RVA: 0x0000BE20 File Offset: 0x0000A020
		public RelayCommand UnlockBinCommand { get; set; }

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x06000236 RID: 566 RVA: 0x0000BE2C File Offset: 0x0000A02C
		// (set) Token: 0x06000237 RID: 567 RVA: 0x0000BE44 File Offset: 0x0000A044
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

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x06000238 RID: 568 RVA: 0x0000BE5C File Offset: 0x0000A05C
		// (set) Token: 0x06000239 RID: 569 RVA: 0x0000BE74 File Offset: 0x0000A074
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

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x0600023A RID: 570 RVA: 0x0000BE8C File Offset: 0x0000A08C
		// (set) Token: 0x0600023B RID: 571 RVA: 0x0000BEA4 File Offset: 0x0000A0A4
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

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x0600023C RID: 572 RVA: 0x0000BEC8 File Offset: 0x0000A0C8
		// (set) Token: 0x0600023D RID: 573 RVA: 0x0000BEE0 File Offset: 0x0000A0E0
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

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x0600023E RID: 574 RVA: 0x0000BEF8 File Offset: 0x0000A0F8
		// (set) Token: 0x0600023F RID: 575 RVA: 0x0000BF10 File Offset: 0x0000A110
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

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x06000240 RID: 576 RVA: 0x0000BF26 File Offset: 0x0000A126
		// (set) Token: 0x06000241 RID: 577 RVA: 0x0000BF2E File Offset: 0x0000A12E
		public string Data { get; set; }

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x06000242 RID: 578 RVA: 0x0000BF38 File Offset: 0x0000A138
		// (set) Token: 0x06000243 RID: 579 RVA: 0x0000BF50 File Offset: 0x0000A150
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
						this.TestsamplesList = this.initTestsampleList();
						bool flag4 = !this.cancelled;
						if (flag4)
						{
							LogbookManager.AddEntry(SSTEntryCode.AuditStart);
						}
					}
				}
			}
		}

		// Token: 0x06000244 RID: 580 RVA: 0x0000BFDC File Offset: 0x0000A1DC
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

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x06000245 RID: 581 RVA: 0x0000C628 File Offset: 0x0000A828
		// (set) Token: 0x06000246 RID: 582 RVA: 0x0000C640 File Offset: 0x0000A840
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
					this.imageTransfer.FrameArrived += this.imageTransfer_FrameArrived;
					this.StartTest();
				}
			}
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x06000247 RID: 583 RVA: 0x0000C720 File Offset: 0x0000A920
		// (set) Token: 0x06000248 RID: 584 RVA: 0x0000C738 File Offset: 0x0000A938
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
							string[] name;
							System.Windows.Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
							{
								bool flag9 = Dialog.Show(LocalizationManager.Localize("txtSuccess"), LocalizationManager.Localize("txtTestOk"), DialogStyle.Ok, DialogButtons.OK) == Dialog.DialogResults.Ok;
								if (flag9)
								{
									bool flag10 = !this.cancelled;
									if (flag10)
									{
										name = this.drawCertificateSign(true, true);
										bool flag11 = name == null;
										if (flag11)
										{
											bool flag12 = !this.endProcess && !this.cancelled;
											if (flag12)
											{
												this.backToStart();
											}
										}
										else
										{
											bool flag13 = !name[0].Equals("Cancelled");
											if (flag13)
											{
												this.reset();
												this.ButtonNextEnabled = true;
												bool documentationEnabled2 = this.documentationEnabled;
												if (documentationEnabled2)
												{
													this.certificateReady(name[1]);
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
							}));
						}
					}
					else
					{
						bool flag4 = System.Windows.Application.Current != null;
						if (flag4)
						{
							System.Windows.Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
							{
								Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtTestFailed"), DialogStyle.Warning, DialogButtons.OK);
							}));
						}
						bool flag5 = !this.cancelled;
						if (flag5)
						{
							string[] name = this.drawCertificateSign(false, true);
							bool flag6 = name != null;
							if (flag6)
							{
								bool flag7 = !name[0].Equals("Cancelled");
								if (flag7)
								{
									this.reset();
									this.ButtonNextEnabled = true;
									bool documentationEnabled = this.documentationEnabled;
									if (documentationEnabled)
									{
										this.certificateReady(name[1]);
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
								bool flag8 = !this.endProcess && !this.cancelled;
								if (flag8)
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

		// Token: 0x14000013 RID: 19
		// (add) Token: 0x06000249 RID: 585 RVA: 0x0000C8F0 File Offset: 0x0000AAF0
		// (remove) Token: 0x0600024A RID: 586 RVA: 0x0000C928 File Offset: 0x0000AB28
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<string> certificateReady;

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x0600024B RID: 587 RVA: 0x0000C95D File Offset: 0x0000AB5D
		// (set) Token: 0x0600024C RID: 588 RVA: 0x0000C965 File Offset: 0x0000AB65
		public string success { get; set; }

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x0600024D RID: 589 RVA: 0x0000C970 File Offset: 0x0000AB70
		// (set) Token: 0x0600024E RID: 590 RVA: 0x0000C988 File Offset: 0x0000AB88
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

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x0600024F RID: 591 RVA: 0x0000C9A0 File Offset: 0x0000ABA0
		// (set) Token: 0x06000250 RID: 592 RVA: 0x0000C9B8 File Offset: 0x0000ABB8
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

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x06000251 RID: 593 RVA: 0x0000C9D0 File Offset: 0x0000ABD0
		// (set) Token: 0x06000252 RID: 594 RVA: 0x0000C9E8 File Offset: 0x0000ABE8
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

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x06000253 RID: 595 RVA: 0x0000CA14 File Offset: 0x0000AC14
		// (set) Token: 0x06000254 RID: 596 RVA: 0x0000CA2C File Offset: 0x0000AC2C
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

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x06000255 RID: 597 RVA: 0x0000CA58 File Offset: 0x0000AC58
		// (set) Token: 0x06000256 RID: 598 RVA: 0x0000CA70 File Offset: 0x0000AC70
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

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x06000257 RID: 599 RVA: 0x0000CA9C File Offset: 0x0000AC9C
		// (set) Token: 0x06000258 RID: 600 RVA: 0x0000CAB4 File Offset: 0x0000ACB4
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

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x06000259 RID: 601 RVA: 0x0000CACC File Offset: 0x0000ACCC
		// (set) Token: 0x0600025A RID: 602 RVA: 0x0000CAE4 File Offset: 0x0000ACE4
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

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x0600025B RID: 603 RVA: 0x0000CAFA File Offset: 0x0000ACFA
		// (set) Token: 0x0600025C RID: 604 RVA: 0x0000CB02 File Offset: 0x0000AD02
		public RelayCommand CleanUpCommand { get; set; }

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x0600025D RID: 605 RVA: 0x0000CB0B File Offset: 0x0000AD0B
		// (set) Token: 0x0600025E RID: 606 RVA: 0x0000CB13 File Offset: 0x0000AD13
		public ParameterHandling parHandling { get; set; }

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x0600025F RID: 607 RVA: 0x0000CB1C File Offset: 0x0000AD1C
		// (set) Token: 0x06000260 RID: 608 RVA: 0x0000CB24 File Offset: 0x0000AD24
		public MachineState machineState { get; set; }

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x06000261 RID: 609 RVA: 0x0000CB2D File Offset: 0x0000AD2D
		// (set) Token: 0x06000262 RID: 610 RVA: 0x0000CB35 File Offset: 0x0000AD35
		public Backup backup { get; set; }

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x06000263 RID: 611 RVA: 0x0000CB40 File Offset: 0x0000AD40
		// (set) Token: 0x06000264 RID: 612 RVA: 0x0000CB58 File Offset: 0x0000AD58
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

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x06000265 RID: 613 RVA: 0x0000CB6E File Offset: 0x0000AD6E
		// (set) Token: 0x06000266 RID: 614 RVA: 0x0000CB76 File Offset: 0x0000AD76
		public AuditRequired AuditRequired { get; set; }

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x06000267 RID: 615 RVA: 0x0000CB7F File Offset: 0x0000AD7F
		// (set) Token: 0x06000268 RID: 616 RVA: 0x0000CB87 File Offset: 0x0000AD87
		public Restore Restore { get; private set; }

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x06000269 RID: 617 RVA: 0x0000CB90 File Offset: 0x0000AD90
		// (set) Token: 0x0600026A RID: 618 RVA: 0x0000CBA8 File Offset: 0x0000ADA8
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

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x0600026B RID: 619 RVA: 0x0000CBC0 File Offset: 0x0000ADC0
		// (set) Token: 0x0600026C RID: 620 RVA: 0x0000CBD8 File Offset: 0x0000ADD8
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

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x0600026D RID: 621 RVA: 0x0000CBEE File Offset: 0x0000ADEE
		// (set) Token: 0x0600026E RID: 622 RVA: 0x0000CBF6 File Offset: 0x0000ADF6
		public bool documentationEnabled { get; set; }

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x06000270 RID: 624 RVA: 0x0000CC18 File Offset: 0x0000AE18
		// (set) Token: 0x0600026F RID: 623 RVA: 0x0000CBFF File Offset: 0x0000ADFF
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

		// Token: 0x06000271 RID: 625 RVA: 0x0000CC30 File Offset: 0x0000AE30
		public BulkAuditCheckProcessViewModel(XRSImageBox imageBox, AccessRightsManager accessManager, MachineState machineState, ParameterHandling parHandl, ImageDataTransfer imageTransfer, DBConnection dbCon, MessageSystem messageSystem, AuditRequired auditRequired, Restore restore, AuditMode Mode, ProductManager productManager)
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
			this.endProcess = true;
			this.ButtonLeftEnabled = false;
		}

		// Token: 0x06000272 RID: 626 RVA: 0x0000D060 File Offset: 0x0000B260
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
						bool flag3 = Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtTimeOver") + ".", DialogStyle.Error, DialogButtons.OK) == Dialog.DialogResults.Ok;
						if (flag3)
						{
							this.drawCertificateSign(false, false);
							this.OnClose();
						}
					}));
				}
			}
		}

		// Token: 0x06000273 RID: 627 RVA: 0x0000D138 File Offset: 0x0000B338
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

		// Token: 0x06000274 RID: 628 RVA: 0x0000D218 File Offset: 0x0000B418
		private void InitValues()
		{
			string data = "";
			this.numberProducts = 1;
			this.dbConnection.GetAuditCheckData(ref data);
			this.Data = data;
			this.numberContaminations = 1;
			this.button1Enabled = true;
			this.button2Enabled = false;
			this.button3Enabled = false;
			this.ButtonNextEnabled = true;
		}

		// Token: 0x06000275 RID: 629 RVA: 0x0000D270 File Offset: 0x0000B470
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x06000276 RID: 630 RVA: 0x0000D2A0 File Offset: 0x0000B4A0
		public void setResult(bool result)
		{
			bool flag = !result;
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
				this.cancelled = true;
				this.AuditRequired.DbConnection.SetLastAuditCheckResult(AuditResult.NotPerformed);
				this.AuditRequired.DbConnection.SetAuditCheckErrorReset(false);
				this.IncreaseNAuditChecks(false);
				bool flag2 = System.Windows.Application.Current != null;
				if (flag2)
				{
					System.Windows.Application.Current.Dispatcher.Invoke(delegate()
					{
						bool flag3 = Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtTestFailed"), DialogStyle.Error, DialogButtons.OK) == Dialog.DialogResults.Ok;
						if (flag3)
						{
							this.drawCertificateSign(false, false);
							this.OnClose();
						}
					});
				}
			}
		}

		// Token: 0x06000277 RID: 631 RVA: 0x0000D354 File Offset: 0x0000B554
		private void RegisterLocalizedText()
		{
			this.StrAuditCheck = new BoundLocalizedText("lbAuditCheck");
			this.StrData = new BoundLocalizedText("txtData");
			this.StrRequest = new BoundLocalizedText("txtRequest");
			this.StrResult = new BoundLocalizedText("txtResult");
			this.StrCheckImages = new BoundLocalizedText("txtAuditCheckImages");
			this.StrContinue = new BoundLocalizedText("txtAuditFailedContinue");
			this.StrConvey = new BoundLocalizedText("txtConveyContaminantProduct");
			this.StrSampleOk = new BoundLocalizedText("txtAuditNumberContaminationsRight");
			this.StrSampleFail = new BoundLocalizedText("txtAuditNumberContaminationsLess");
			this.AuditRequired.InTestContaminationMode = true;
			this.StrStartAudit = new BoundLocalizedText("lbAuditStarted");
			this.StrPassed = new BoundLocalizedText("txtPassed");
			this.StrFailed = new BoundLocalizedText("txtFailed");
			this.StrTestballs = new BoundLocalizedText("txtTestballs");
			this.StrProducts = new BoundLocalizedText("txtContaminatedProduct");
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

		// Token: 0x06000278 RID: 632 RVA: 0x0000D54C File Offset: 0x0000B74C
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
		}

		// Token: 0x06000279 RID: 633 RVA: 0x0000D704 File Offset: 0x0000B904
		public virtual void OnClose()
		{
			Action close = this.Close;
			bool flag = close != null;
			if (flag)
			{
				close();
			}
		}

		// Token: 0x0600027A RID: 634 RVA: 0x0000D72C File Offset: 0x0000B92C
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

		// Token: 0x0600027B RID: 635 RVA: 0x0000D80C File Offset: 0x0000BA0C
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

		// Token: 0x0600027C RID: 636 RVA: 0x0000D8D8 File Offset: 0x0000BAD8
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

		// Token: 0x0600027D RID: 637 RVA: 0x0000D964 File Offset: 0x0000BB64
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
					this.UserLogAvailable = true;
					this.countGoodSamples = 0;
					this.readyForImage = true;
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

		// Token: 0x0600027E RID: 638 RVA: 0x0000DC28 File Offset: 0x0000BE28
		private void imageTransfer_FrameArrived(object sender, ReceiveEventArgs e)
		{
			bool flag = e.Image.Type != ImageType.Histogram && e.Image.Type != ImageType.MaterialCurve && e.Image.Type != ImageType.TestcardSimulation;
			if (flag)
			{
				bool flag2 = e.Image.IsContaminant() && this.readyForImage;
				if (flag2)
				{
					this.readyForImage = false;
					bool flag3 = this.ShowImage(e.Image);
					if (flag3)
					{
						this.numberOfImages++;
						bool flag4 = this.numberOfImages >= this.numberTestsamples * this.numberProducts;
						if (flag4)
						{
							this.parHandling.WriteValue(Definition.inAuditModePath, "0", false);
							this.imageTransfer.FrameArrived -= this.imageTransfer_FrameArrived;
						}
						this.workWithImage(e.Image);
					}
				}
			}
		}

		// Token: 0x0600027F RID: 639 RVA: 0x0000DD10 File Offset: 0x0000BF10
		public void workWithImage(XRSImage image)
		{
			this.CurrentImage = image;
			this.imageBox.ViewXRSImage(image);
			int num = this.ImageConformsToTest(image);
			bool flag = num == 0 || num == 1;
			if (flag)
			{
				this.countGoodSamples++;
				this.setResultAfterDialog(num);
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
				this.cancelled = true;
				this.AuditRequired.DbConnection.SetLastAuditCheckResult(AuditResult.NotPerformed);
				this.AuditRequired.DbConnection.SetAuditCheckErrorReset(false);
				this.IncreaseNAuditChecks(false);
				bool flag2 = System.Windows.Application.Current != null;
				if (flag2)
				{
					System.Windows.Application.Current.Dispatcher.Invoke(delegate()
					{
						bool flag3 = Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtTestFailed") + ". " + this.StrSampleFail.LocalizedText, DialogStyle.Error, DialogButtons.OK) == Dialog.DialogResults.Ok;
						if (flag3)
						{
							this.drawCertificateSign(false, false);
							this.OnClose();
						}
					});
				}
			}
		}

		// Token: 0x06000280 RID: 640 RVA: 0x0000DE00 File Offset: 0x0000C000
		public void setResultAfterDialog(int number)
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
				bool flag2 = this.RemainingProducts == 0;
				if (flag2)
				{
					this.Update();
				}
			}
		}

		// Token: 0x06000281 RID: 641 RVA: 0x0000DE9C File Offset: 0x0000C09C
		public void Update()
		{
			bool flag = this.IsCont;
			if (flag)
			{
				this.TestsamplesList[this.numberTestsamples - this.RemainingTestBalls].NumberFoundTestballs = this.countGoodSamples;
				LogbookManager.AddEntry(SSTEntryCode.AuditTestballFinished, 0, this.ConvertMaterial(this.TestsamplesList[this.numberTestsamples - this.RemainingTestBalls].Description), this.TestsamplesList[this.numberTestsamples - this.RemainingTestBalls].Value, this.countGoodSamples.ToString(), this.numberProducts.ToString());
				this.countGoodSamples = 0;
			}
			int num = this.RemainingTestBalls;
			this.RemainingTestBalls = num - 1;
			bool flag2 = this.RemainingTestBalls == 0;
			if (flag2)
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
				bool flag3 = System.Windows.Application.Current != null;
				if (flag3)
				{
					System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(delegate()
					{
						bool flag4 = Dialog.Show(LocalizationManager.Localize("txtInfo"), LocalizationManager.Localize("txtNextTestsample") + ": " + this.TestsampleToConvey, DialogStyle.Info, DialogButtons.OK) == Dialog.DialogResults.Ok;
						if (flag4)
						{
							this.readyForImage = true;
						}
					}), new object[0]);
				}
			}
		}

		// Token: 0x06000282 RID: 642 RVA: 0x0000E17C File Offset: 0x0000C37C
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
				result = -1;
			}
			return result;
		}

		// Token: 0x06000283 RID: 643 RVA: 0x0000E1D4 File Offset: 0x0000C3D4
		public void CheckUserRightforUnlockBin(System.Windows.Controls.Button btnUnlockBin)
		{
			this.AccessManager.RegisterControl(btnUnlockBin, Rejecter.doVerifyTestRight, Rejecter.doVerifyTestRight);
		}

		// Token: 0x06000284 RID: 644 RVA: 0x0000E1EE File Offset: 0x0000C3EE
		public void DeregisterUnlockBinButton(System.Windows.Controls.Button btnUnlockBin)
		{
			this.AccessManager.DeregisterControl(btnUnlockBin);
		}

		// Token: 0x06000285 RID: 645 RVA: 0x0000E200 File Offset: 0x0000C400
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

		// Token: 0x06000286 RID: 646 RVA: 0x0000E278 File Offset: 0x0000C478
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

		// Token: 0x06000287 RID: 647 RVA: 0x0000E450 File Offset: 0x0000C650
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

		// Token: 0x06000288 RID: 648 RVA: 0x0000E4C4 File Offset: 0x0000C6C4
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

		// Token: 0x06000289 RID: 649 RVA: 0x0000E560 File Offset: 0x0000C760
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

		// Token: 0x0600028A RID: 650 RVA: 0x0000E660 File Offset: 0x0000C860
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

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x0600028B RID: 651 RVA: 0x0000E6CC File Offset: 0x0000C8CC
		// (set) Token: 0x0600028C RID: 652 RVA: 0x0000E6E4 File Offset: 0x0000C8E4
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

		// Token: 0x0600028D RID: 653 RVA: 0x0000E6FC File Offset: 0x0000C8FC
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

		// Token: 0x0600028E RID: 654 RVA: 0x0000E7F4 File Offset: 0x0000C9F4
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

		// Token: 0x0600028F RID: 655 RVA: 0x0001077C File Offset: 0x0000E97C
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

		// Token: 0x06000290 RID: 656 RVA: 0x000108CC File Offset: 0x0000EACC
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

		// Token: 0x06000291 RID: 657 RVA: 0x0001092C File Offset: 0x0000EB2C
		public void entryForCertificateFailed()
		{
		}

		// Token: 0x06000292 RID: 658 RVA: 0x00010930 File Offset: 0x0000EB30
		public void CleanUp(object obj)
		{
			this.parHandling.WriteValue(Definition.inAuditModePath, "0", false);
			bool flag = this.timerCheck != null;
			if (flag)
			{
				this.timerCheck.Stop();
			}
			this.DeregisterLocalizedText();
			this.reset();
			bool flag2 = this.TestsamplesList != null;
			if (flag2)
			{
				this.TestsamplesList.Clear();
				this.TestsamplesList = null;
			}
			bool flag3 = this.ResultList != null;
			if (flag3)
			{
				this.ResultList.Clear();
				this.ResultList = null;
			}
			bool flag4 = this.imageQueue != null;
			if (flag4)
			{
				this.imageQueue.Clear();
				this.imageQueue = null;
			}
			bool flag5 = this.UnlockBinItem != null;
			if (flag5)
			{
				this.UnlockBinItem.Clear();
				this.UnlockBinItem = null;
			}
			bool flag6 = this.machineState.State == MachineStates.AuditCheck;
			if (flag6)
			{
				this.machineState.State = MachineStates.Inspecting;
			}
			bool flag7 = this.setResultError;
			if (flag7)
			{
				bool flag8 = this.resultForEnd;
				if (flag8)
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

		// Token: 0x06000293 RID: 659 RVA: 0x00010AEC File Offset: 0x0000ECEC
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

		// Token: 0x040001B5 RID: 437
		public static readonly string minWeightSubPath = "/weigher/MinWeight/Value";

		// Token: 0x040001B6 RID: 438
		public static readonly string maxWeightSubPath = "/weigher/MaxWeight/Value";

		// Token: 0x040001B7 RID: 439
		private bool cancelled = false;

		// Token: 0x040001B8 RID: 440
		private readonly string TestcardMaterialPath1 = "TestcardMaterial1";

		// Token: 0x040001B9 RID: 441
		private readonly string TestcardType1 = "TestcardType1";

		// Token: 0x040001BA RID: 442
		private readonly string TestcardSize1 = "TestcardSize1";

		// Token: 0x040001BB RID: 443
		private readonly string TestcardNumberCont1 = "TestcardNumberCont1";

		// Token: 0x040001BC RID: 444
		private readonly string TestcardNumber1 = "TestcardNumber1";

		// Token: 0x040001BD RID: 445
		private readonly string TestcardMaterialPath2 = "TestcardMaterial2";

		// Token: 0x040001BE RID: 446
		private readonly string TestcardType2 = "TestcardType2";

		// Token: 0x040001BF RID: 447
		private readonly string TestcardSize2 = "TestcardSize2";

		// Token: 0x040001C0 RID: 448
		private readonly string TestcardNumberCont2 = "TestcardNumberCont2";

		// Token: 0x040001C1 RID: 449
		private readonly string TestcardNumber2 = "TestcardNumber2";

		// Token: 0x040001C2 RID: 450
		private readonly string TestcardMaterialPath3 = "TestcardMaterial3";

		// Token: 0x040001C3 RID: 451
		private readonly string TestcardType3 = "TestcardType3";

		// Token: 0x040001C4 RID: 452
		private readonly string TestcardSize3 = "TestcardSize3";

		// Token: 0x040001C5 RID: 453
		private readonly string TestcardNumberCont3 = "TestcardNumberCont3";

		// Token: 0x040001C6 RID: 454
		private readonly string TestcardNumber3 = "TestcardNumber3";

		// Token: 0x040001C7 RID: 455
		private readonly string TestcardMaterialPath4 = "TestcardMaterial4";

		// Token: 0x040001C8 RID: 456
		private readonly string TestcardType4 = "TestcardType4";

		// Token: 0x040001C9 RID: 457
		private readonly string TestcardSize4 = "TestcardSize4";

		// Token: 0x040001CA RID: 458
		private readonly string TestcardNumberCont4 = "TestcardNumberCont4";

		// Token: 0x040001CB RID: 459
		private readonly string TestcardNumber4 = "TestcardNumber4";

		// Token: 0x040001CC RID: 460
		private readonly string TestcardMaterialPath5 = "TestcardMaterial5";

		// Token: 0x040001CD RID: 461
		private readonly string TestcardType5 = "TestcardType5";

		// Token: 0x040001CE RID: 462
		private readonly string TestcardSize5 = "TestcardSize5";

		// Token: 0x040001CF RID: 463
		private readonly string TestcardNumberCont5 = "TestcardNumberCont5";

		// Token: 0x040001D0 RID: 464
		private readonly string TestcardNumber5 = "TestcardNumber5";

		// Token: 0x040001EE RID: 494
		public Keyboard keyboard;

		// Token: 0x040001EF RID: 495
		private XRSImage currentImage;

		// Token: 0x040001F0 RID: 496
		private DateTime startTime;

		// Token: 0x040001F1 RID: 497
		private DBConnection dbConnection;

		// Token: 0x040001F2 RID: 498
		private bool countCont;

		// Token: 0x040001F5 RID: 501
		public AuditMode Mode = AuditMode.Contamination;

		// Token: 0x040001F6 RID: 502
		private ImageDataTransfer imageTransfer;

		// Token: 0x040001F7 RID: 503
		private int remainingProducts;

		// Token: 0x040001F8 RID: 504
		private int numberContaminations;

		// Token: 0x040001F9 RID: 505
		private string stringNumberContaminations;

		// Token: 0x040001FA RID: 506
		private int numberProducts;

		// Token: 0x040001FB RID: 507
		private int numberTestsamples;

		// Token: 0x040001FC RID: 508
		private bool userLogAvailable;

		// Token: 0x040001FD RID: 509
		public int remainingTestBalls;

		// Token: 0x040001FE RID: 510
		private bool contextUnlockBinVisibility = false;

		// Token: 0x04000200 RID: 512
		private Rejecter rejecter;

		// Token: 0x04000202 RID: 514
		private int countGoodSamples = 0;

		// Token: 0x04000203 RID: 515
		private bool widescreen;

		// Token: 0x04000204 RID: 516
		private List<XRSImage> imageQueue = new List<XRSImage>();

		// Token: 0x04000205 RID: 517
		private bool isCont = false;

		// Token: 0x04000206 RID: 518
		private bool isNotCont = false;

		// Token: 0x04000207 RID: 519
		private string textStep1 = "";

		// Token: 0x04000209 RID: 521
		private bool enabled1;

		// Token: 0x0400020A RID: 522
		private bool enabled2;

		// Token: 0x0400020B RID: 523
		private bool enabled3;

		// Token: 0x0400020E RID: 526
		private bool check;

		// Token: 0x0400020F RID: 527
		private bool enabledNext;

		// Token: 0x04000210 RID: 528
		private string testsampleToConvey;

		// Token: 0x04000211 RID: 529
		private string productsConveyed;

		// Token: 0x04000212 RID: 530
		private string testsamplesConveyed;

		// Token: 0x04000213 RID: 531
		private string endText = "";

		// Token: 0x04000214 RID: 532
		public ObservableCollection<InfosTestsamples> testsamplesList;

		// Token: 0x04000218 RID: 536
		private DispatcherTimer timerCheck;

		// Token: 0x0400021A RID: 538
		private string nameSubproduct = "";

		// Token: 0x0400021B RID: 539
		private bool isLane = false;

		// Token: 0x0400021C RID: 540
		private bool isMultiProduct = false;

		// Token: 0x0400021D RID: 541
		private int numberOfImages = 0;

		// Token: 0x0400021E RID: 542
		private MessageSystem messageSystem;

		// Token: 0x0400021F RID: 543
		private bool doorLockAvailable;

		// Token: 0x04000222 RID: 546
		private ProductManager productManager;

		// Token: 0x04000223 RID: 547
		private bool timeout = false;

		// Token: 0x04000224 RID: 548
		private bool remainingTimeVisible;

		// Token: 0x04000225 RID: 549
		private bool popUpOpen = false;

		// Token: 0x04000227 RID: 551
		private bool rejectAll = false;

		// Token: 0x04000228 RID: 552
		private XRSImageBox imageBox;

		// Token: 0x04000229 RID: 553
		private bool endProcess = false;

		// Token: 0x0400022A RID: 554
		private bool buttonLeftEnabled = true;

		// Token: 0x0400022B RID: 555
		private bool readyForImage = false;

		// Token: 0x0400022C RID: 556
		private bool setResultError = false;

		// Token: 0x0400022D RID: 557
		private bool resultForEnd = false;

		// Token: 0x0400022E RID: 558
		private ObservableCollection<InfosTestsamples> resultList = new ObservableCollection<InfosTestsamples>();
	}
}
