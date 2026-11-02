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
	// Token: 0x0200001E RID: 30
	public class FastAuditCheckProcessViewModel : INotifyPropertyChanged
	{
		// Token: 0x14000015 RID: 21
		// (add) Token: 0x060002AB RID: 683 RVA: 0x00011468 File Offset: 0x0000F668
		// (remove) Token: 0x060002AC RID: 684 RVA: 0x000114A0 File Offset: 0x0000F6A0
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x060002AD RID: 685 RVA: 0x000114D5 File Offset: 0x0000F6D5
		// (set) Token: 0x060002AE RID: 686 RVA: 0x000114DD File Offset: 0x0000F6DD
		public BoundLocalizedText StrData { get; set; }

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x060002AF RID: 687 RVA: 0x000114E6 File Offset: 0x0000F6E6
		// (set) Token: 0x060002B0 RID: 688 RVA: 0x000114EE File Offset: 0x0000F6EE
		public BoundLocalizedText StrRequest { get; set; }

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x060002B1 RID: 689 RVA: 0x000114F7 File Offset: 0x0000F6F7
		// (set) Token: 0x060002B2 RID: 690 RVA: 0x000114FF File Offset: 0x0000F6FF
		public BoundLocalizedText StrResult { get; set; }

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x060002B3 RID: 691 RVA: 0x00011508 File Offset: 0x0000F708
		// (set) Token: 0x060002B4 RID: 692 RVA: 0x00011510 File Offset: 0x0000F710
		public BoundLocalizedText StrAuditCheck { get; set; }

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x060002B5 RID: 693 RVA: 0x00011519 File Offset: 0x0000F719
		// (set) Token: 0x060002B6 RID: 694 RVA: 0x00011521 File Offset: 0x0000F721
		public BoundLocalizedText StrProduct { get; set; }

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x060002B7 RID: 695 RVA: 0x0001152A File Offset: 0x0000F72A
		// (set) Token: 0x060002B8 RID: 696 RVA: 0x00011532 File Offset: 0x0000F732
		public BoundLocalizedText StrSampleOk { get; set; }

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x060002B9 RID: 697 RVA: 0x0001153B File Offset: 0x0000F73B
		// (set) Token: 0x060002BA RID: 698 RVA: 0x00011543 File Offset: 0x0000F743
		public BoundLocalizedText StrSampleFail { get; set; }

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x060002BB RID: 699 RVA: 0x0001154C File Offset: 0x0000F74C
		// (set) Token: 0x060002BC RID: 700 RVA: 0x00011554 File Offset: 0x0000F754
		public BoundLocalizedText StrContinue { get; set; }

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x060002BD RID: 701 RVA: 0x0001155D File Offset: 0x0000F75D
		// (set) Token: 0x060002BE RID: 702 RVA: 0x00011565 File Offset: 0x0000F765
		public BoundLocalizedText StrCheckImages { get; set; }

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x060002BF RID: 703 RVA: 0x0001156E File Offset: 0x0000F76E
		// (set) Token: 0x060002C0 RID: 704 RVA: 0x00011576 File Offset: 0x0000F776
		public BoundLocalizedText StrConvey { get; set; }

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x060002C1 RID: 705 RVA: 0x0001157F File Offset: 0x0000F77F
		// (set) Token: 0x060002C2 RID: 706 RVA: 0x00011587 File Offset: 0x0000F787
		public BoundLocalizedText StrStartAudit { get; set; }

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x060002C3 RID: 707 RVA: 0x00011590 File Offset: 0x0000F790
		// (set) Token: 0x060002C4 RID: 708 RVA: 0x00011598 File Offset: 0x0000F798
		public BoundLocalizedText StrPassed { get; set; }

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x060002C5 RID: 709 RVA: 0x000115A1 File Offset: 0x0000F7A1
		// (set) Token: 0x060002C6 RID: 710 RVA: 0x000115A9 File Offset: 0x0000F7A9
		public BoundLocalizedText StrFailed { get; set; }

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x060002C7 RID: 711 RVA: 0x000115B2 File Offset: 0x0000F7B2
		// (set) Token: 0x060002C8 RID: 712 RVA: 0x000115BA File Offset: 0x0000F7BA
		public BoundLocalizedText StrTestballs { get; set; }

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x060002C9 RID: 713 RVA: 0x000115C3 File Offset: 0x0000F7C3
		// (set) Token: 0x060002CA RID: 714 RVA: 0x000115CB File Offset: 0x0000F7CB
		public BoundLocalizedText StrProducts { get; set; }

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x060002CB RID: 715 RVA: 0x000115D4 File Offset: 0x0000F7D4
		// (set) Token: 0x060002CC RID: 716 RVA: 0x000115DC File Offset: 0x0000F7DC
		public BoundLocalizedText StrNumberContaminations { get; set; }

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x060002CD RID: 717 RVA: 0x000115E5 File Offset: 0x0000F7E5
		// (set) Token: 0x060002CE RID: 718 RVA: 0x000115ED File Offset: 0x0000F7ED
		public BoundLocalizedText StrStartTime { get; set; }

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x060002CF RID: 719 RVA: 0x000115F6 File Offset: 0x0000F7F6
		// (set) Token: 0x060002D0 RID: 720 RVA: 0x000115FE File Offset: 0x0000F7FE
		public BoundLocalizedText StrEndTime { get; set; }

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x060002D1 RID: 721 RVA: 0x00011607 File Offset: 0x0000F807
		// (set) Token: 0x060002D2 RID: 722 RVA: 0x0001160F File Offset: 0x0000F80F
		public BoundLocalizedText StrTestcard { get; set; }

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x060002D3 RID: 723 RVA: 0x00011618 File Offset: 0x0000F818
		// (set) Token: 0x060002D4 RID: 724 RVA: 0x00011620 File Offset: 0x0000F820
		public BoundLocalizedText StrWeight { get; set; }

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x060002D5 RID: 725 RVA: 0x00011629 File Offset: 0x0000F829
		// (set) Token: 0x060002D6 RID: 726 RVA: 0x00011631 File Offset: 0x0000F831
		public BoundLocalizedText StrNumber { get; set; }

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x060002D7 RID: 727 RVA: 0x0001163A File Offset: 0x0000F83A
		// (set) Token: 0x060002D8 RID: 728 RVA: 0x00011642 File Offset: 0x0000F842
		public BoundLocalizedText StrEndResult { get; set; }

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x060002D9 RID: 729 RVA: 0x0001164B File Offset: 0x0000F84B
		// (set) Token: 0x060002DA RID: 730 RVA: 0x00011653 File Offset: 0x0000F853
		public BoundLocalizedText StrDocumenation { get; set; }

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x060002DB RID: 731 RVA: 0x0001165C File Offset: 0x0000F85C
		// (set) Token: 0x060002DC RID: 732 RVA: 0x00011664 File Offset: 0x0000F864
		public BoundLocalizedText StrUser { get; set; }

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x060002DD RID: 733 RVA: 0x0001166D File Offset: 0x0000F86D
		// (set) Token: 0x060002DE RID: 734 RVA: 0x00011675 File Offset: 0x0000F875
		public BoundLocalizedText StrSign { get; set; }

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x060002DF RID: 735 RVA: 0x0001167E File Offset: 0x0000F87E
		// (set) Token: 0x060002E0 RID: 736 RVA: 0x00011686 File Offset: 0x0000F886
		public BoundLocalizedText StrProductNumber { get; set; }

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x060002E1 RID: 737 RVA: 0x0001168F File Offset: 0x0000F88F
		// (set) Token: 0x060002E2 RID: 738 RVA: 0x00011697 File Offset: 0x0000F897
		public BoundLocalizedText StrDeviceData { get; set; }

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x060002E3 RID: 739 RVA: 0x000116A0 File Offset: 0x0000F8A0
		// (set) Token: 0x060002E4 RID: 740 RVA: 0x000116A8 File Offset: 0x0000F8A8
		public BoundLocalizedText StrRemainingTime { get; set; }

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x060002E5 RID: 741 RVA: 0x000116B4 File Offset: 0x0000F8B4
		// (set) Token: 0x060002E6 RID: 742 RVA: 0x000116CC File Offset: 0x0000F8CC
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

		// Token: 0x14000016 RID: 22
		// (add) Token: 0x060002E7 RID: 743 RVA: 0x00011700 File Offset: 0x0000F900
		// (remove) Token: 0x060002E8 RID: 744 RVA: 0x00011738 File Offset: 0x0000F938
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action Close;

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x060002E9 RID: 745 RVA: 0x0001176D File Offset: 0x0000F96D
		// (set) Token: 0x060002EA RID: 746 RVA: 0x00011775 File Offset: 0x0000F975
		public AccessRightsManager AccessManager { get; set; }

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x060002EB RID: 747 RVA: 0x00011780 File Offset: 0x0000F980
		// (set) Token: 0x060002EC RID: 748 RVA: 0x00011798 File Offset: 0x0000F998
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

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x060002ED RID: 749 RVA: 0x000117CC File Offset: 0x0000F9CC
		// (set) Token: 0x060002EE RID: 750 RVA: 0x000117E4 File Offset: 0x0000F9E4
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

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x060002EF RID: 751 RVA: 0x00011818 File Offset: 0x0000FA18
		// (set) Token: 0x060002F0 RID: 752 RVA: 0x00011830 File Offset: 0x0000FA30
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

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x060002F1 RID: 753 RVA: 0x00011864 File Offset: 0x0000FA64
		// (set) Token: 0x060002F2 RID: 754 RVA: 0x0001187C File Offset: 0x0000FA7C
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

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x060002F3 RID: 755 RVA: 0x000118AF File Offset: 0x0000FAAF
		// (set) Token: 0x060002F4 RID: 756 RVA: 0x000118B7 File Offset: 0x0000FAB7
		public ObservableCollection<RejecterItem> UnlockBinItem { get; set; }

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x060002F5 RID: 757 RVA: 0x000118C0 File Offset: 0x0000FAC0
		// (set) Token: 0x060002F6 RID: 758 RVA: 0x000118C8 File Offset: 0x0000FAC8
		public RelayCommand UnlockBinCommand { get; set; }

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x060002F7 RID: 759 RVA: 0x000118D4 File Offset: 0x0000FAD4
		// (set) Token: 0x060002F8 RID: 760 RVA: 0x000118EC File Offset: 0x0000FAEC
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

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x060002F9 RID: 761 RVA: 0x00011904 File Offset: 0x0000FB04
		// (set) Token: 0x060002FA RID: 762 RVA: 0x0001191C File Offset: 0x0000FB1C
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

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x060002FB RID: 763 RVA: 0x00011934 File Offset: 0x0000FB34
		// (set) Token: 0x060002FC RID: 764 RVA: 0x0001194C File Offset: 0x0000FB4C
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

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x060002FD RID: 765 RVA: 0x00011970 File Offset: 0x0000FB70
		// (set) Token: 0x060002FE RID: 766 RVA: 0x00011988 File Offset: 0x0000FB88
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

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x060002FF RID: 767 RVA: 0x000119A0 File Offset: 0x0000FBA0
		// (set) Token: 0x06000300 RID: 768 RVA: 0x000119B8 File Offset: 0x0000FBB8
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

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x06000301 RID: 769 RVA: 0x000119CE File Offset: 0x0000FBCE
		// (set) Token: 0x06000302 RID: 770 RVA: 0x000119D6 File Offset: 0x0000FBD6
		public string Data { get; set; }

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x06000303 RID: 771 RVA: 0x000119E0 File Offset: 0x0000FBE0
		// (set) Token: 0x06000304 RID: 772 RVA: 0x000119F8 File Offset: 0x0000FBF8
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

		// Token: 0x06000305 RID: 773 RVA: 0x00011A84 File Offset: 0x0000FC84
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

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x06000306 RID: 774 RVA: 0x000120D0 File Offset: 0x000102D0
		// (set) Token: 0x06000307 RID: 775 RVA: 0x000120E8 File Offset: 0x000102E8
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

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x06000308 RID: 776 RVA: 0x0001230C File Offset: 0x0001050C
		// (set) Token: 0x06000309 RID: 777 RVA: 0x00012324 File Offset: 0x00010524
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
										name = this.drawCertificateSign(true, false);
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
							string[] name = this.drawCertificateSign(false, false);
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

		// Token: 0x14000017 RID: 23
		// (add) Token: 0x0600030A RID: 778 RVA: 0x000124DC File Offset: 0x000106DC
		// (remove) Token: 0x0600030B RID: 779 RVA: 0x00012514 File Offset: 0x00010714
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<string> certificateReady;

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x0600030C RID: 780 RVA: 0x00012549 File Offset: 0x00010749
		// (set) Token: 0x0600030D RID: 781 RVA: 0x00012551 File Offset: 0x00010751
		public string success { get; set; }

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x0600030E RID: 782 RVA: 0x0001255C File Offset: 0x0001075C
		// (set) Token: 0x0600030F RID: 783 RVA: 0x00012574 File Offset: 0x00010774
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

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x06000310 RID: 784 RVA: 0x0001258C File Offset: 0x0001078C
		// (set) Token: 0x06000311 RID: 785 RVA: 0x000125A4 File Offset: 0x000107A4
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

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x06000312 RID: 786 RVA: 0x000125BC File Offset: 0x000107BC
		// (set) Token: 0x06000313 RID: 787 RVA: 0x000125D4 File Offset: 0x000107D4
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

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x06000314 RID: 788 RVA: 0x00012600 File Offset: 0x00010800
		// (set) Token: 0x06000315 RID: 789 RVA: 0x00012618 File Offset: 0x00010818
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

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x06000316 RID: 790 RVA: 0x00012644 File Offset: 0x00010844
		// (set) Token: 0x06000317 RID: 791 RVA: 0x0001265C File Offset: 0x0001085C
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

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x06000318 RID: 792 RVA: 0x00012688 File Offset: 0x00010888
		// (set) Token: 0x06000319 RID: 793 RVA: 0x000126A0 File Offset: 0x000108A0
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

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x0600031A RID: 794 RVA: 0x000126B8 File Offset: 0x000108B8
		// (set) Token: 0x0600031B RID: 795 RVA: 0x000126D0 File Offset: 0x000108D0
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

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x0600031C RID: 796 RVA: 0x000126E6 File Offset: 0x000108E6
		// (set) Token: 0x0600031D RID: 797 RVA: 0x000126EE File Offset: 0x000108EE
		public RelayCommand CleanUpCommand { get; set; }

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x0600031E RID: 798 RVA: 0x000126F7 File Offset: 0x000108F7
		// (set) Token: 0x0600031F RID: 799 RVA: 0x000126FF File Offset: 0x000108FF
		public ParameterHandling parHandling { get; set; }

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x06000320 RID: 800 RVA: 0x00012708 File Offset: 0x00010908
		// (set) Token: 0x06000321 RID: 801 RVA: 0x00012710 File Offset: 0x00010910
		public MachineState machineState { get; set; }

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x06000322 RID: 802 RVA: 0x00012719 File Offset: 0x00010919
		// (set) Token: 0x06000323 RID: 803 RVA: 0x00012721 File Offset: 0x00010921
		public Backup backup { get; set; }

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x06000324 RID: 804 RVA: 0x0001272C File Offset: 0x0001092C
		// (set) Token: 0x06000325 RID: 805 RVA: 0x00012744 File Offset: 0x00010944
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

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x06000326 RID: 806 RVA: 0x0001275A File Offset: 0x0001095A
		// (set) Token: 0x06000327 RID: 807 RVA: 0x00012762 File Offset: 0x00010962
		public AuditRequired AuditRequired { get; set; }

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x06000328 RID: 808 RVA: 0x0001276B File Offset: 0x0001096B
		// (set) Token: 0x06000329 RID: 809 RVA: 0x00012773 File Offset: 0x00010973
		public Restore Restore { get; private set; }

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x0600032A RID: 810 RVA: 0x0001277C File Offset: 0x0001097C
		// (set) Token: 0x0600032B RID: 811 RVA: 0x00012794 File Offset: 0x00010994
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

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x0600032C RID: 812 RVA: 0x000127AC File Offset: 0x000109AC
		// (set) Token: 0x0600032D RID: 813 RVA: 0x000127C4 File Offset: 0x000109C4
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

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x0600032E RID: 814 RVA: 0x000127DA File Offset: 0x000109DA
		// (set) Token: 0x0600032F RID: 815 RVA: 0x000127E2 File Offset: 0x000109E2
		public bool documentationEnabled { get; set; }

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x06000331 RID: 817 RVA: 0x00012804 File Offset: 0x00010A04
		// (set) Token: 0x06000330 RID: 816 RVA: 0x000127EB File Offset: 0x000109EB
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

		// Token: 0x06000332 RID: 818 RVA: 0x0001281C File Offset: 0x00010A1C
		public FastAuditCheckProcessViewModel(XRSImageBox imageBox, AccessRightsManager accessManager, MachineState machineState, ParameterHandling parHandl, ImageDataTransfer imageTransfer, DBConnection dbCon, MessageSystem messageSystem, AuditRequired auditRequired, Restore restore, AuditMode Mode, ProductManager productManager)
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
			bool flag4 = this.parHandling.ReadPar(Definition.detectorMonitoringActive) == "1";
			if (flag4)
			{
				this.parHandling.WriteValue(Definition.detectorMonitoringBackgroundEnable, "0");
			}
		}

		// Token: 0x06000333 RID: 819 RVA: 0x00012C80 File Offset: 0x00010E80
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

		// Token: 0x06000334 RID: 820 RVA: 0x00012D58 File Offset: 0x00010F58
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

		// Token: 0x06000335 RID: 821 RVA: 0x00012E38 File Offset: 0x00011038
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

		// Token: 0x06000336 RID: 822 RVA: 0x00012EA8 File Offset: 0x000110A8
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x06000337 RID: 823 RVA: 0x00012ED8 File Offset: 0x000110D8
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

		// Token: 0x06000338 RID: 824 RVA: 0x00012F8C File Offset: 0x0001118C
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

		// Token: 0x06000339 RID: 825 RVA: 0x00013184 File Offset: 0x00011384
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

		// Token: 0x0600033A RID: 826 RVA: 0x0001333C File Offset: 0x0001153C
		public virtual void OnClose()
		{
			Action close = this.Close;
			bool flag = close != null;
			if (flag)
			{
				close();
			}
		}

		// Token: 0x0600033B RID: 827 RVA: 0x00013364 File Offset: 0x00011564
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

		// Token: 0x0600033C RID: 828 RVA: 0x00013444 File Offset: 0x00011644
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

		// Token: 0x0600033D RID: 829 RVA: 0x00013510 File Offset: 0x00011710
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

		// Token: 0x0600033E RID: 830 RVA: 0x0001359C File Offset: 0x0001179C
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

		// Token: 0x0600033F RID: 831 RVA: 0x00013858 File Offset: 0x00011A58
		private void imageTransfer_FrameArrived(object sender, ReceiveEventArgs e)
		{
			bool flag = e.Image.Type != ImageType.Histogram && e.Image.Type != ImageType.MaterialCurve && e.Image.Type != ImageType.TestcardSimulation;
			if (flag)
			{
				bool flag2 = e.Image.IsContaminant();
				if (flag2)
				{
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
						this.imageQueue.Add(e.Image);
						bool flag5 = this.imageQueue.Count == 1;
						if (flag5)
						{
							this.workWithImage();
						}
					}
				}
			}
		}

		// Token: 0x06000340 RID: 832 RVA: 0x00013954 File Offset: 0x00011B54
		public void workWithImage()
		{
			bool flag = this.imageQueue != null && this.imageQueue.Count<XRSImage>() > 0;
			if (flag)
			{
				XRSImage image = this.imageQueue.First<XRSImage>();
				this.CurrentImage = image;
				this.imageBox.ViewXRSImage(image);
				int num = this.ImageConformsToTest(image);
				bool flag2 = num == 0 || num == 1;
				if (flag2)
				{
					this.countGoodSamples++;
					this.TestsamplesList[this.numberTestsamples - this.RemainingTestBalls].NumberFoundTestballs = this.countGoodSamples;
					this.setResultAfterDialog(num, image);
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
					bool flag3 = System.Windows.Application.Current != null;
					if (flag3)
					{
						System.Windows.Application.Current.Dispatcher.Invoke(delegate()
						{
							bool flag4 = Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtTestFailed") + ". " + this.StrSampleFail.LocalizedText, DialogStyle.Error, DialogButtons.OK) == Dialog.DialogResults.Ok;
							if (flag4)
							{
								this.drawCertificateSign(false, false);
								this.OnClose();
							}
						});
					}
				}
			}
		}

		// Token: 0x06000341 RID: 833 RVA: 0x00013A9C File Offset: 0x00011C9C
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
				bool flag2 = this.RemainingProducts == 0;
				if (flag2)
				{
					this.Update(image);
				}
				else
				{
					this.imageQueue.Remove(image);
					bool flag3 = this.imageQueue.Count >= 1;
					if (flag3)
					{
						this.workWithImage();
					}
				}
			}
		}

		// Token: 0x06000342 RID: 834 RVA: 0x00013B6C File Offset: 0x00011D6C
		public void Update(XRSImage image)
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
							this.imageQueue.Remove(image);
							bool flag5 = this.imageQueue.Count >= 1;
							if (flag5)
							{
								this.workWithImage();
							}
						}
					}), new object[0]);
				}
			}
		}

		// Token: 0x06000343 RID: 835 RVA: 0x00013E60 File Offset: 0x00012060
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

		// Token: 0x06000344 RID: 836 RVA: 0x00013EB8 File Offset: 0x000120B8
		public void CheckUserRightforUnlockBin(System.Windows.Controls.Button btnUnlockBin)
		{
			this.AccessManager.RegisterControl(btnUnlockBin, Rejecter.doVerifyTestRight, Rejecter.doVerifyTestRight);
		}

		// Token: 0x06000345 RID: 837 RVA: 0x00013ED2 File Offset: 0x000120D2
		public void DeregisterUnlockBinButton(System.Windows.Controls.Button btnUnlockBin)
		{
			this.AccessManager.DeregisterControl(btnUnlockBin);
		}

		// Token: 0x06000346 RID: 838 RVA: 0x00013EE4 File Offset: 0x000120E4
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

		// Token: 0x06000347 RID: 839 RVA: 0x00013F5C File Offset: 0x0001215C
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
				this.EndResultAuditCheck();
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

		// Token: 0x06000348 RID: 840 RVA: 0x000140D4 File Offset: 0x000122D4
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

		// Token: 0x06000349 RID: 841 RVA: 0x00014148 File Offset: 0x00012348
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

		// Token: 0x0600034A RID: 842 RVA: 0x000141E4 File Offset: 0x000123E4
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

		// Token: 0x0600034B RID: 843 RVA: 0x000142E4 File Offset: 0x000124E4
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

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x0600034C RID: 844 RVA: 0x00014350 File Offset: 0x00012550
		// (set) Token: 0x0600034D RID: 845 RVA: 0x00014368 File Offset: 0x00012568
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

		// Token: 0x0600034E RID: 846 RVA: 0x00014380 File Offset: 0x00012580
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

		// Token: 0x0600034F RID: 847 RVA: 0x00014478 File Offset: 0x00012678
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

		// Token: 0x06000350 RID: 848 RVA: 0x00016400 File Offset: 0x00014600
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

		// Token: 0x06000351 RID: 849 RVA: 0x00016550 File Offset: 0x00014750
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

		// Token: 0x06000352 RID: 850 RVA: 0x0001092C File Offset: 0x0000EB2C
		public void entryForCertificateFailed()
		{
		}

		// Token: 0x06000353 RID: 851 RVA: 0x000165B0 File Offset: 0x000147B0
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

		// Token: 0x06000354 RID: 852 RVA: 0x000167A4 File Offset: 0x000149A4
		public void EndResultAuditCheck()
		{
			bool flag = !this.button3Enabled;
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

		// Token: 0x0400023F RID: 575
		public static readonly string minWeightSubPath = "/weigher/MinWeight/Value";

		// Token: 0x04000240 RID: 576
		public static readonly string maxWeightSubPath = "/weigher/MaxWeight/Value";

		// Token: 0x04000241 RID: 577
		private bool cancelled = false;

		// Token: 0x04000242 RID: 578
		private readonly string TestcardMaterialPath1 = "TestcardMaterial1";

		// Token: 0x04000243 RID: 579
		private readonly string TestcardType1 = "TestcardType1";

		// Token: 0x04000244 RID: 580
		private readonly string TestcardSize1 = "TestcardSize1";

		// Token: 0x04000245 RID: 581
		private readonly string TestcardNumberCont1 = "TestcardNumberCont1";

		// Token: 0x04000246 RID: 582
		private readonly string TestcardNumber1 = "TestcardNumber1";

		// Token: 0x04000247 RID: 583
		private readonly string TestcardMaterialPath2 = "TestcardMaterial2";

		// Token: 0x04000248 RID: 584
		private readonly string TestcardType2 = "TestcardType2";

		// Token: 0x04000249 RID: 585
		private readonly string TestcardSize2 = "TestcardSize2";

		// Token: 0x0400024A RID: 586
		private readonly string TestcardNumberCont2 = "TestcardNumberCont2";

		// Token: 0x0400024B RID: 587
		private readonly string TestcardNumber2 = "TestcardNumber2";

		// Token: 0x0400024C RID: 588
		private readonly string TestcardMaterialPath3 = "TestcardMaterial3";

		// Token: 0x0400024D RID: 589
		private readonly string TestcardType3 = "TestcardType3";

		// Token: 0x0400024E RID: 590
		private readonly string TestcardSize3 = "TestcardSize3";

		// Token: 0x0400024F RID: 591
		private readonly string TestcardNumberCont3 = "TestcardNumberCont3";

		// Token: 0x04000250 RID: 592
		private readonly string TestcardNumber3 = "TestcardNumber3";

		// Token: 0x04000251 RID: 593
		private readonly string TestcardMaterialPath4 = "TestcardMaterial4";

		// Token: 0x04000252 RID: 594
		private readonly string TestcardType4 = "TestcardType4";

		// Token: 0x04000253 RID: 595
		private readonly string TestcardSize4 = "TestcardSize4";

		// Token: 0x04000254 RID: 596
		private readonly string TestcardNumberCont4 = "TestcardNumberCont4";

		// Token: 0x04000255 RID: 597
		private readonly string TestcardNumber4 = "TestcardNumber4";

		// Token: 0x04000256 RID: 598
		private readonly string TestcardMaterialPath5 = "TestcardMaterial5";

		// Token: 0x04000257 RID: 599
		private readonly string TestcardType5 = "TestcardType5";

		// Token: 0x04000258 RID: 600
		private readonly string TestcardSize5 = "TestcardSize5";

		// Token: 0x04000259 RID: 601
		private readonly string TestcardNumberCont5 = "TestcardNumberCont5";

		// Token: 0x0400025A RID: 602
		private readonly string TestcardNumber5 = "TestcardNumber5";

		// Token: 0x04000278 RID: 632
		public Keyboard keyboard;

		// Token: 0x04000279 RID: 633
		private XRSImage currentImage;

		// Token: 0x0400027A RID: 634
		private DateTime startTime;

		// Token: 0x0400027B RID: 635
		private DBConnection dbConnection;

		// Token: 0x0400027C RID: 636
		private bool countCont;

		// Token: 0x0400027F RID: 639
		public AuditMode Mode = AuditMode.Contamination;

		// Token: 0x04000280 RID: 640
		private ImageDataTransfer imageTransfer;

		// Token: 0x04000281 RID: 641
		private int remainingProducts;

		// Token: 0x04000282 RID: 642
		private int numberContaminations;

		// Token: 0x04000283 RID: 643
		private string stringNumberContaminations;

		// Token: 0x04000284 RID: 644
		private int numberProducts;

		// Token: 0x04000285 RID: 645
		private int numberTestsamples;

		// Token: 0x04000286 RID: 646
		private bool userLogAvailable;

		// Token: 0x04000287 RID: 647
		public int remainingTestBalls;

		// Token: 0x04000288 RID: 648
		private bool contextUnlockBinVisibility = false;

		// Token: 0x0400028A RID: 650
		private Rejecter rejecter;

		// Token: 0x0400028C RID: 652
		private int countGoodSamples = 0;

		// Token: 0x0400028D RID: 653
		private bool widescreen;

		// Token: 0x0400028E RID: 654
		private List<XRSImage> imageQueue = new List<XRSImage>();

		// Token: 0x0400028F RID: 655
		private bool isCont = false;

		// Token: 0x04000290 RID: 656
		private bool isNotCont = false;

		// Token: 0x04000291 RID: 657
		private string textStep1 = "";

		// Token: 0x04000293 RID: 659
		private bool enabled1;

		// Token: 0x04000294 RID: 660
		private bool enabled2;

		// Token: 0x04000295 RID: 661
		private bool enabled3;

		// Token: 0x04000298 RID: 664
		private bool check;

		// Token: 0x04000299 RID: 665
		private bool enabledNext;

		// Token: 0x0400029A RID: 666
		private string testsampleToConvey;

		// Token: 0x0400029B RID: 667
		private string productsConveyed;

		// Token: 0x0400029C RID: 668
		private string testsamplesConveyed;

		// Token: 0x0400029D RID: 669
		private string endText = "";

		// Token: 0x0400029E RID: 670
		public ObservableCollection<InfosTestsamples> testsamplesList;

		// Token: 0x040002A2 RID: 674
		private DispatcherTimer timerCheck;

		// Token: 0x040002A4 RID: 676
		private string nameSubproduct = "";

		// Token: 0x040002A5 RID: 677
		private bool isLane = false;

		// Token: 0x040002A6 RID: 678
		private bool isMultiProduct = false;

		// Token: 0x040002A7 RID: 679
		private int numberOfImages = 0;

		// Token: 0x040002A8 RID: 680
		private MessageSystem messageSystem;

		// Token: 0x040002A9 RID: 681
		private bool doorLockAvailable;

		// Token: 0x040002AC RID: 684
		private ProductManager productManager;

		// Token: 0x040002AD RID: 685
		private bool timeout = false;

		// Token: 0x040002AE RID: 686
		private bool remainingTimeVisible;

		// Token: 0x040002AF RID: 687
		private bool popUpOpen = false;

		// Token: 0x040002B1 RID: 689
		private bool rejectAll = false;

		// Token: 0x040002B2 RID: 690
		private XRSImageBox imageBox;

		// Token: 0x040002B3 RID: 691
		private bool endProcess = false;

		// Token: 0x040002B4 RID: 692
		private bool buttonLeftEnabled = true;

		// Token: 0x040002B5 RID: 693
		private bool setResultError = false;

		// Token: 0x040002B6 RID: 694
		private bool resultForEnd = false;

		// Token: 0x040002B7 RID: 695
		private ObservableCollection<InfosTestsamples> resultList = new ObservableCollection<InfosTestsamples>();
	}
}
