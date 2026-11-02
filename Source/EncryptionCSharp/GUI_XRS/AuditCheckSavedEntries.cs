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
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using Common;
using DatabaseConnection;
using Definitions;
using GUI_XRS.Properties;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using PdfSharp.Pdf;

namespace GUI_XRS
{
	// Token: 0x0200001F RID: 31
	public class AuditCheckSavedEntries : INotifyPropertyChanged
	{
		// Token: 0x14000018 RID: 24
		// (add) Token: 0x0600035B RID: 859 RVA: 0x00016A04 File Offset: 0x00014C04
		// (remove) Token: 0x0600035C RID: 860 RVA: 0x00016A3C File Offset: 0x00014C3C
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x0600035D RID: 861 RVA: 0x00016A71 File Offset: 0x00014C71
		// (set) Token: 0x0600035E RID: 862 RVA: 0x00016A79 File Offset: 0x00014C79
		public RelayCommand CleanUpCommand { get; set; }

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x0600035F RID: 863 RVA: 0x00016A82 File Offset: 0x00014C82
		// (set) Token: 0x06000360 RID: 864 RVA: 0x00016A8A File Offset: 0x00014C8A
		public BoundLocalizedText StrData { get; set; }

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x06000361 RID: 865 RVA: 0x00016A93 File Offset: 0x00014C93
		// (set) Token: 0x06000362 RID: 866 RVA: 0x00016A9B File Offset: 0x00014C9B
		public BoundLocalizedText StrRequest { get; set; }

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x06000363 RID: 867 RVA: 0x00016AA4 File Offset: 0x00014CA4
		// (set) Token: 0x06000364 RID: 868 RVA: 0x00016AAC File Offset: 0x00014CAC
		public BoundLocalizedText StrResult { get; set; }

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x06000365 RID: 869 RVA: 0x00016AB5 File Offset: 0x00014CB5
		// (set) Token: 0x06000366 RID: 870 RVA: 0x00016ABD File Offset: 0x00014CBD
		public BoundLocalizedText StrAuditCheck { get; set; }

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x06000367 RID: 871 RVA: 0x00016AC6 File Offset: 0x00014CC6
		// (set) Token: 0x06000368 RID: 872 RVA: 0x00016ACE File Offset: 0x00014CCE
		public BoundLocalizedText StrProduct { get; set; }

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x06000369 RID: 873 RVA: 0x00016AD7 File Offset: 0x00014CD7
		// (set) Token: 0x0600036A RID: 874 RVA: 0x00016ADF File Offset: 0x00014CDF
		public BoundLocalizedText StrSampleOk { get; set; }

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x0600036B RID: 875 RVA: 0x00016AE8 File Offset: 0x00014CE8
		// (set) Token: 0x0600036C RID: 876 RVA: 0x00016AF0 File Offset: 0x00014CF0
		public BoundLocalizedText StrSampleFail { get; set; }

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x0600036D RID: 877 RVA: 0x00016AF9 File Offset: 0x00014CF9
		// (set) Token: 0x0600036E RID: 878 RVA: 0x00016B01 File Offset: 0x00014D01
		public BoundLocalizedText StrCheckImages { get; set; }

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x0600036F RID: 879 RVA: 0x00016B0A File Offset: 0x00014D0A
		// (set) Token: 0x06000370 RID: 880 RVA: 0x00016B12 File Offset: 0x00014D12
		public BoundLocalizedText StrConvey { get; set; }

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x06000371 RID: 881 RVA: 0x00016B1B File Offset: 0x00014D1B
		// (set) Token: 0x06000372 RID: 882 RVA: 0x00016B23 File Offset: 0x00014D23
		public BoundLocalizedText StrStartAudit { get; set; }

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x06000373 RID: 883 RVA: 0x00016B2C File Offset: 0x00014D2C
		// (set) Token: 0x06000374 RID: 884 RVA: 0x00016B34 File Offset: 0x00014D34
		public BoundLocalizedText StrPassed { get; set; }

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x06000375 RID: 885 RVA: 0x00016B3D File Offset: 0x00014D3D
		// (set) Token: 0x06000376 RID: 886 RVA: 0x00016B45 File Offset: 0x00014D45
		public BoundLocalizedText StrFailed { get; set; }

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x06000377 RID: 887 RVA: 0x00016B4E File Offset: 0x00014D4E
		// (set) Token: 0x06000378 RID: 888 RVA: 0x00016B56 File Offset: 0x00014D56
		public BoundLocalizedText StrTestballs { get; set; }

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x06000379 RID: 889 RVA: 0x00016B5F File Offset: 0x00014D5F
		// (set) Token: 0x0600037A RID: 890 RVA: 0x00016B67 File Offset: 0x00014D67
		public BoundLocalizedText StrProducts { get; set; }

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x0600037B RID: 891 RVA: 0x00016B70 File Offset: 0x00014D70
		// (set) Token: 0x0600037C RID: 892 RVA: 0x00016B78 File Offset: 0x00014D78
		public BoundLocalizedText StrNumberContaminations { get; set; }

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x0600037D RID: 893 RVA: 0x00016B81 File Offset: 0x00014D81
		// (set) Token: 0x0600037E RID: 894 RVA: 0x00016B89 File Offset: 0x00014D89
		public BoundLocalizedText StrStartTime { get; set; }

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x0600037F RID: 895 RVA: 0x00016B92 File Offset: 0x00014D92
		// (set) Token: 0x06000380 RID: 896 RVA: 0x00016B9A File Offset: 0x00014D9A
		public BoundLocalizedText StrTimeIntervall { get; set; }

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x06000381 RID: 897 RVA: 0x00016BA3 File Offset: 0x00014DA3
		// (set) Token: 0x06000382 RID: 898 RVA: 0x00016BAB File Offset: 0x00014DAB
		public BoundLocalizedText StrEndTime { get; set; }

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x06000383 RID: 899 RVA: 0x00016BB4 File Offset: 0x00014DB4
		// (set) Token: 0x06000384 RID: 900 RVA: 0x00016BBC File Offset: 0x00014DBC
		public BoundLocalizedText StrTestcard { get; set; }

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x06000385 RID: 901 RVA: 0x00016BC5 File Offset: 0x00014DC5
		// (set) Token: 0x06000386 RID: 902 RVA: 0x00016BCD File Offset: 0x00014DCD
		public BoundLocalizedText StrWeight { get; set; }

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x06000387 RID: 903 RVA: 0x00016BD6 File Offset: 0x00014DD6
		// (set) Token: 0x06000388 RID: 904 RVA: 0x00016BDE File Offset: 0x00014DDE
		public BoundLocalizedText StrNumber { get; set; }

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x06000389 RID: 905 RVA: 0x00016BE7 File Offset: 0x00014DE7
		// (set) Token: 0x0600038A RID: 906 RVA: 0x00016BEF File Offset: 0x00014DEF
		public BoundLocalizedText StrEndResult { get; set; }

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x0600038B RID: 907 RVA: 0x00016BF8 File Offset: 0x00014DF8
		// (set) Token: 0x0600038C RID: 908 RVA: 0x00016C00 File Offset: 0x00014E00
		public BoundLocalizedText StrProductNumber { get; set; }

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x0600038D RID: 909 RVA: 0x00016C09 File Offset: 0x00014E09
		// (set) Token: 0x0600038E RID: 910 RVA: 0x00016C11 File Offset: 0x00014E11
		public BoundLocalizedText StrDocumenation { get; set; }

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x0600038F RID: 911 RVA: 0x00016C1A File Offset: 0x00014E1A
		// (set) Token: 0x06000390 RID: 912 RVA: 0x00016C22 File Offset: 0x00014E22
		public BoundLocalizedText StrUser { get; set; }

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x06000391 RID: 913 RVA: 0x00016C2B File Offset: 0x00014E2B
		// (set) Token: 0x06000392 RID: 914 RVA: 0x00016C33 File Offset: 0x00014E33
		public RelayCommand FirstItemCommand { get; set; }

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x06000393 RID: 915 RVA: 0x00016C3C File Offset: 0x00014E3C
		// (set) Token: 0x06000394 RID: 916 RVA: 0x00016C44 File Offset: 0x00014E44
		public RelayCommand LastItemCommand { get; set; }

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x06000395 RID: 917 RVA: 0x00016C4D File Offset: 0x00014E4D
		// (set) Token: 0x06000396 RID: 918 RVA: 0x00016C55 File Offset: 0x00014E55
		public RelayCommand PreviousItemCommand { get; set; }

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x06000397 RID: 919 RVA: 0x00016C5E File Offset: 0x00014E5E
		// (set) Token: 0x06000398 RID: 920 RVA: 0x00016C66 File Offset: 0x00014E66
		public RelayCommand NextItemCommand { get; set; }

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x06000399 RID: 921 RVA: 0x00016C70 File Offset: 0x00014E70
		// (set) Token: 0x0600039A RID: 922 RVA: 0x00016C88 File Offset: 0x00014E88
		public bool BrowserVisible
		{
			get
			{
				return this.browserVisible;
			}
			set
			{
				this.browserVisible = value;
				this.BrowserNotVisible = !value;
				this.OnPropertyChanged("BrowserVisible");
			}
		}

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x0600039B RID: 923 RVA: 0x00016CAC File Offset: 0x00014EAC
		// (set) Token: 0x0600039C RID: 924 RVA: 0x00016CC4 File Offset: 0x00014EC4
		public bool BrowserNotVisible
		{
			get
			{
				return this.browserNotVisible;
			}
			set
			{
				this.browserNotVisible = value;
				this.OnPropertyChanged("BrowserNotVisible");
			}
		}

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x0600039D RID: 925 RVA: 0x00016CDC File Offset: 0x00014EDC
		// (set) Token: 0x0600039E RID: 926 RVA: 0x00016CF4 File Offset: 0x00014EF4
		public List<string> Modes
		{
			get
			{
				return this.modes;
			}
			set
			{
				this.modes = value;
				this.OnPropertyChanged("Modes");
			}
		}

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x0600039F RID: 927 RVA: 0x00016D0C File Offset: 0x00014F0C
		// (set) Token: 0x060003A0 RID: 928 RVA: 0x00016D24 File Offset: 0x00014F24
		public List<string> DataModes
		{
			get
			{
				return this.dataModes;
			}
			set
			{
				bool flag = value != null;
				if (flag)
				{
					this.dataModes = value;
					bool flag2 = this.DateVisible;
					if (flag2)
					{
						bool flag3 = this.dataModes.Count<string>() > 0;
						if (flag3)
						{
							string text = this.dataModes.Last<string>();
							this.SelectedDataMode2 = text;
						}
					}
					this.SelectedDataMode = value.Last<string>();
					this.OnPropertyChanged("DataModes");
				}
			}
		}

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x060003A1 RID: 929 RVA: 0x00016D90 File Offset: 0x00014F90
		// (set) Token: 0x060003A2 RID: 930 RVA: 0x00016DA8 File Offset: 0x00014FA8
		public ObservableCollection<AuditCheckEntries> Entries
		{
			get
			{
				return this.entries;
			}
			set
			{
				this.entries = value;
				this.OnPropertyChanged("Entries");
				bool flag = this.Entries != null;
				if (flag)
				{
					bool flag2 = this.Entries.Count > 0;
					if (flag2)
					{
						this.SelectedEntry = this.Entries.First<AuditCheckEntries>();
					}
				}
			}
		}

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x060003A3 RID: 931 RVA: 0x00016DFC File Offset: 0x00014FFC
		// (set) Token: 0x060003A4 RID: 932 RVA: 0x00016E14 File Offset: 0x00015014
		public string SelectedMode
		{
			get
			{
				return this.selectedMode;
			}
			set
			{
				bool flag = value != null;
				if (flag)
				{
					this.selectedMode = value;
					List<string> list = new List<string>();
					bool flag2 = value.Equals("None");
					if (flag2)
					{
						this.DateVisible = false;
						list.Add("None");
					}
					else
					{
						bool flag3 = value.Equals(this.StrUser.LocalizedText);
						if (flag3)
						{
							this.DateVisible = false;
							this.dbConnection.GetAuditUserIDs(ref list);
							list.Add("-");
						}
						else
						{
							bool flag4 = value.Equals(this.StrProduct.LocalizedText);
							if (flag4)
							{
								this.DateVisible = false;
								this.dbConnection.GetAuditProductNumbers(ref list);
								list.Add("-");
							}
							else
							{
								bool flag5 = value.Equals(this.StrTimeIntervall.LocalizedText);
								if (flag5)
								{
									this.DateVisible = true;
									this.dbConnection.GetAuditDates(ref list);
									list.Add("-");
								}
								else
								{
									bool flag6 = value.Equals(this.StrEndResult.LocalizedText);
									if (flag6)
									{
										this.DateVisible = false;
										list.Add(this.StrFailed.LocalizedText);
										list.Add(this.StrPassed.LocalizedText);
										list.Add("-");
									}
									else
									{
										this.DateVisible = false;
										list.Add("None");
									}
								}
							}
						}
					}
					this.first = true;
					this.DataModes = list;
					this.OnPropertyChanged("SelectedMode");
				}
			}
		}

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x060003A5 RID: 933 RVA: 0x00016FA8 File Offset: 0x000151A8
		// (set) Token: 0x060003A6 RID: 934 RVA: 0x00016FC0 File Offset: 0x000151C0
		public bool DateVisible
		{
			get
			{
				return this.dateVisible;
			}
			set
			{
				this.dateVisible = value;
				this.OnPropertyChanged("DateVisible");
			}
		}

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x060003A7 RID: 935 RVA: 0x00016FD8 File Offset: 0x000151D8
		// (set) Token: 0x060003A8 RID: 936 RVA: 0x00016FF0 File Offset: 0x000151F0
		public string SelectedDataMode
		{
			get
			{
				return this.selectedDataMode;
			}
			set
			{
				this.selectedDataMode = value;
				bool flag = !this.first;
				if (flag)
				{
					bool flag2 = !value.Equals("-") || this.SelectedMode.Equals(this.StrTimeIntervall.LocalizedText);
					if (flag2)
					{
						bool flag3 = this.SelectedMode.Equals("None");
						if (flag3)
						{
							this.InitializeList();
							this.stopCreationAll = false;
						}
						else
						{
							bool flag4 = this.SelectedMode.Equals(this.StrUser.LocalizedText);
							if (flag4)
							{
								this.InitializeUserList(value);
								this.stopCreationAll = true;
							}
							else
							{
								bool flag5 = this.SelectedMode.Equals(this.StrProduct.LocalizedText);
								if (flag5)
								{
									this.InitializeProductList(value);
									this.stopCreationAll = true;
								}
								else
								{
									bool flag6 = this.SelectedMode.Equals(this.StrTimeIntervall.LocalizedText);
									if (flag6)
									{
										bool flag7 = (value.CompareTo(this.SelectedDataMode2) <= 0 || value.Equals("-") || this.SelectedDataMode2.Equals("-")) && this.SelectedDataMode2 != null && !this.first;
										if (flag7)
										{
											this.InitializeDateList();
											this.stopCreationAll = true;
										}
									}
									else
									{
										bool flag8 = this.SelectedMode.Equals(this.StrEndResult.LocalizedText);
										if (flag8)
										{
											this.InitializeEndresultList(value);
											this.stopCreationAll = true;
										}
									}
								}
							}
						}
					}
					else
					{
						this.InitializeList();
						this.stopCreationAll = false;
					}
				}
				else
				{
					bool flag9 = this.SelectedDataMode.Equals("None");
					if (flag9)
					{
						this.InitializeList();
					}
					this.stopCreationAll = false;
				}
				this.first = false;
				this.OnPropertyChanged("SelectedDataMode");
			}
		}

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x060003A9 RID: 937 RVA: 0x000171C4 File Offset: 0x000153C4
		// (set) Token: 0x060003AA RID: 938 RVA: 0x000171DC File Offset: 0x000153DC
		public string SelectedDataMode2
		{
			get
			{
				return this.selectedDataMode2;
			}
			set
			{
				bool flag = this.DateVisible;
				if (flag)
				{
					this.selectedDataMode2 = value;
					this.OnPropertyChanged("SelectedDataMode2");
					bool flag2 = (value.CompareTo(this.SelectedDataMode) >= 0 || value.Equals("-") || this.SelectedDataMode.Equals("-")) && !this.first;
					if (flag2)
					{
						this.InitializeDateList();
						this.stopCreationAll = true;
					}
				}
			}
		}

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x060003AB RID: 939 RVA: 0x00017258 File Offset: 0x00015458
		// (set) Token: 0x060003AC RID: 940 RVA: 0x00017270 File Offset: 0x00015470
		public AuditCheckEntries SelectedEntry
		{
			get
			{
				return this.selectedEntry;
			}
			set
			{
				this.selectedEntry = value;
				this.OnPropertyChanged("SelectedEntry");
			}
		}

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x060003AD RID: 941 RVA: 0x00017288 File Offset: 0x00015488
		// (set) Token: 0x060003AE RID: 942 RVA: 0x000172A0 File Offset: 0x000154A0
		public bool SaveEnabled
		{
			get
			{
				return this.saveEnabled;
			}
			set
			{
				this.saveEnabled = value;
				this.OnPropertyChanged("SaveEnabled");
			}
		}

		// Token: 0x060003AF RID: 943 RVA: 0x000172B8 File Offset: 0x000154B8
		public AuditCheckSavedEntries(DBConnection dBCon)
		{
			this.dbConnection = dBCon;
			this.RegisterLocalizedText();
			this.InitData();
			this.FirstItemCommand = new RelayCommand(new Action<object>(this.SelectFirstItem));
			this.LastItemCommand = new RelayCommand(new Action<object>(this.SelectLastItem));
			this.PreviousItemCommand = new RelayCommand(new Action<object>(this.SelectPreviousItem));
			this.NextItemCommand = new RelayCommand(new Action<object>(this.SelectNextItem));
		}

		// Token: 0x060003B0 RID: 944 RVA: 0x00017380 File Offset: 0x00015580
		public void SelectFirstItem(object obj)
		{
			bool flag = this.Entries.Count > 0;
			if (flag)
			{
				this.SelectedEntry = this.Entries.First<AuditCheckEntries>();
			}
		}

		// Token: 0x060003B1 RID: 945 RVA: 0x000173B4 File Offset: 0x000155B4
		public void SelectLastItem(object obj)
		{
			bool flag = this.Entries.Count > 0;
			if (flag)
			{
				this.SelectedEntry = this.Entries.Last<AuditCheckEntries>();
			}
		}

		// Token: 0x060003B2 RID: 946 RVA: 0x000173E8 File Offset: 0x000155E8
		public void SelectPreviousItem(object obj)
		{
			bool flag = this.Entries.Count > 0;
			if (flag)
			{
				int num = this.Entries.IndexOf(this.SelectedEntry);
				bool flag2 = num == 0;
				if (flag2)
				{
					this.SelectLastItem("x");
				}
				else
				{
					this.SelectedEntry = this.Entries[num - 1];
				}
			}
		}

		// Token: 0x060003B3 RID: 947 RVA: 0x00017448 File Offset: 0x00015648
		public void SelectNextItem(object obj)
		{
			bool flag = this.Entries.Count > 0;
			if (flag)
			{
				int num = this.Entries.IndexOf(this.SelectedEntry);
				bool flag2 = num != this.Entries.Count - 1;
				if (flag2)
				{
					this.SelectedEntry = this.Entries[num + 1];
				}
				else
				{
					this.SelectFirstItem("x");
				}
			}
		}

		// Token: 0x060003B4 RID: 948 RVA: 0x000174B8 File Offset: 0x000156B8
		private void InitializeEndresultList(string value)
		{
			Thread thread = new Thread(new ThreadStart(this.LoadEntriesEndresult));
			thread.Start();
		}

		// Token: 0x060003B5 RID: 949 RVA: 0x000174E0 File Offset: 0x000156E0
		private void LoadEntriesEndresult()
		{
			Progress prog = null;
			bool flag = Application.Current != null;
			if (flag)
			{
				Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate()
				{
					prog = new Progress("Filtering entries...", false);
					prog.Show();
				}));
			}
			int i = 0;
			this.Entries = new ObservableCollection<AuditCheckEntries>();
			List<string[]> entries = new List<string[]>();
			entries.Clear();
			this.Entries.Clear();
			bool flag2 = this.SelectedDataMode.Equals(this.StrFailed.LocalizedText);
			if (flag2)
			{
				this.dbConnection.GetAllAuditEntriesForEndresult(ref entries, "0");
			}
			else
			{
				this.dbConnection.GetAllAuditEntriesForEndresult(ref entries, "1");
			}
			Action <>9__3;
			for (int j = entries.Count - 1; j >= 0; j--)
			{
				string[] item = entries[j];
				bool flag3 = this.stopCreation;
				if (flag3)
				{
					break;
				}
				string endr = "";
				bool flag4 = item[19].Equals("1");
				if (flag4)
				{
					endr = this.StrPassed.LocalizedText;
				}
				else
				{
					endr = this.StrFailed.LocalizedText;
				}
				bool flag5 = Application.Current != null;
				if (flag5)
				{
					Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate()
					{
						AuditCheckEntries item = new AuditCheckEntries
						{
							id = item[0],
							startDate = item[1].Replace(" ", "\n"),
							endDate = item[2].Replace(" ", "\n"),
							userName = item[3],
							userId = item[4],
							productName = item[5],
							productNumber = item[6],
							batchNumber = item[7],
							numberConveyed = item[8],
							testcard1 = item[9],
							testcardResult1 = item[10],
							testcard2 = item[11],
							testcardResult2 = item[12],
							testcard3 = item[13],
							testcardResult3 = item[14],
							testcard4 = item[15],
							testcardResult4 = item[16],
							testcard5 = item[17],
							testcardResult5 = item[18],
							endresult = endr
						};
						this.Entries.Add(item);
					}));
				}
				bool flag6 = Application.Current != null;
				if (flag6)
				{
					Dispatcher dispatcher = Application.Current.Dispatcher;
					DispatcherPriority priority = DispatcherPriority.Background;
					Action method;
					if ((method = <>9__3) == null)
					{
						method = (<>9__3 = delegate()
						{
							bool flag8 = prog != null;
							if (flag8)
							{
								prog.progBar.Value = (double)(i * 100 / entries.Count);
							}
						});
					}
					dispatcher.Invoke(priority, method);
				}
				int i2 = i;
				i = i2 + 1;
			}
			bool flag7 = Application.Current != null;
			if (flag7)
			{
				Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate()
				{
					bool flag8 = prog != null;
					if (flag8)
					{
						prog.Close();
					}
				}));
			}
		}

		// Token: 0x060003B6 RID: 950 RVA: 0x00017730 File Offset: 0x00015930
		private void InitializeDateList()
		{
			Thread thread = new Thread(new ThreadStart(this.LoadEntriesDate));
			thread.Start();
		}

		// Token: 0x060003B7 RID: 951 RVA: 0x00017758 File Offset: 0x00015958
		private void LoadEntriesDate()
		{
			Progress prog = null;
			bool flag = Application.Current != null;
			if (flag)
			{
				Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate()
				{
					prog = new Progress("Filtering entries...", false);
					prog.Show();
				}));
			}
			int i = 0;
			this.Entries = new ObservableCollection<AuditCheckEntries>();
			List<string[]> entries = new List<string[]>();
			entries.Clear();
			this.Entries.Clear();
			string dateFirst = this.SelectedDataMode;
			string dateSecond = this.SelectedDataMode2;
			bool flag2 = this.SelectedDataMode.Equals("-") && this.DataModes.Count > 1;
			if (flag2)
			{
				dateFirst = this.DataModes.First<string>();
			}
			bool flag3 = this.SelectedDataMode2.Equals("-") && this.DataModes.Count > 1;
			if (flag3)
			{
				dateSecond = this.DataModes[this.DataModes.Count - 2];
			}
			this.dbConnection.GetAllAuditEntriesFromDate(ref entries, dateFirst, dateSecond);
			Action <>9__3;
			for (int j = entries.Count - 1; j >= 0; j--)
			{
				string[] item = entries[j];
				bool flag4 = this.stopCreation;
				if (flag4)
				{
					break;
				}
				string endr = "";
				bool flag5 = item[19].Equals("1");
				if (flag5)
				{
					endr = this.StrPassed.LocalizedText;
				}
				else
				{
					endr = this.StrFailed.LocalizedText;
				}
				bool flag6 = Application.Current != null;
				if (flag6)
				{
					Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate()
					{
						AuditCheckEntries item = new AuditCheckEntries
						{
							id = item[0],
							startDate = item[1].Replace(" ", "\n"),
							endDate = item[2].Replace(" ", "\n"),
							userName = item[3],
							userId = item[4],
							productName = item[5],
							productNumber = item[6],
							batchNumber = item[7],
							numberConveyed = item[8],
							testcard1 = item[9],
							testcardResult1 = item[10],
							testcard2 = item[11],
							testcardResult2 = item[12],
							testcard3 = item[13],
							testcardResult3 = item[14],
							testcard4 = item[15],
							testcardResult4 = item[16],
							testcard5 = item[17],
							testcardResult5 = item[18],
							endresult = endr
						};
						this.Entries.Add(item);
					}));
				}
				bool flag7 = Application.Current != null;
				if (flag7)
				{
					Dispatcher dispatcher = Application.Current.Dispatcher;
					DispatcherPriority priority = DispatcherPriority.Background;
					Action method;
					if ((method = <>9__3) == null)
					{
						method = (<>9__3 = delegate()
						{
							bool flag9 = prog != null;
							if (flag9)
							{
								prog.progBar.Value = (double)(i * 100 / entries.Count);
							}
						});
					}
					dispatcher.Invoke(priority, method);
				}
				int i2 = i;
				i = i2 + 1;
			}
			bool flag8 = Application.Current != null;
			if (flag8)
			{
				Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate()
				{
					bool flag9 = prog != null;
					if (flag9)
					{
						prog.Close();
					}
				}));
			}
		}

		// Token: 0x060003B8 RID: 952 RVA: 0x000179F8 File Offset: 0x00015BF8
		private void InitializeProductList(string value)
		{
			bool flag = !this.stopCreation;
			if (flag)
			{
				Thread thread = new Thread(new ThreadStart(this.LoadEntriesProduct));
				thread.Start();
			}
		}

		// Token: 0x060003B9 RID: 953 RVA: 0x00017A30 File Offset: 0x00015C30
		private void LoadEntriesProduct()
		{
			Progress prog = null;
			bool flag = Application.Current != null;
			if (flag)
			{
				Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate()
				{
					prog = new Progress("Filtering entries...", false);
					prog.Show();
				}));
			}
			int i = 0;
			this.Entries = new ObservableCollection<AuditCheckEntries>();
			List<string[]> entries = new List<string[]>();
			entries.Clear();
			this.Entries.Clear();
			this.dbConnection.GetAllAuditEntriesForProductNumber(ref entries, this.SelectedDataMode);
			Action <>9__3;
			for (int j = entries.Count - 1; j >= 0; j--)
			{
				string[] item = entries[j];
				bool flag2 = this.stopCreation;
				if (flag2)
				{
					break;
				}
				string endr = "";
				bool flag3 = item[19].Equals("1");
				if (flag3)
				{
					endr = this.StrPassed.LocalizedText;
				}
				else
				{
					endr = this.StrFailed.LocalizedText;
				}
				bool flag4 = Application.Current != null;
				if (flag4)
				{
					Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate()
					{
						AuditCheckEntries item = new AuditCheckEntries
						{
							id = item[0],
							startDate = item[1].Replace(" ", "\n"),
							endDate = item[2].Replace(" ", "\n"),
							userName = item[3],
							userId = item[4],
							productName = item[5],
							productNumber = item[6],
							batchNumber = item[7],
							numberConveyed = item[8],
							testcard1 = item[9],
							testcardResult1 = item[10],
							testcard2 = item[11],
							testcardResult2 = item[12],
							testcard3 = item[13],
							testcardResult3 = item[14],
							testcard4 = item[15],
							testcardResult4 = item[16],
							testcard5 = item[17],
							testcardResult5 = item[18],
							endresult = endr
						};
						this.Entries.Add(item);
					}));
				}
				bool flag5 = Application.Current != null;
				if (flag5)
				{
					Dispatcher dispatcher = Application.Current.Dispatcher;
					DispatcherPriority priority = DispatcherPriority.Background;
					Action method;
					if ((method = <>9__3) == null)
					{
						method = (<>9__3 = delegate()
						{
							bool flag7 = prog != null;
							if (flag7)
							{
								prog.progBar.Value = (double)(i * 100 / entries.Count);
							}
						});
					}
					dispatcher.Invoke(priority, method);
				}
				int i2 = i;
				i = i2 + 1;
			}
			bool flag6 = Application.Current != null;
			if (flag6)
			{
				Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate()
				{
					bool flag7 = prog != null;
					if (flag7)
					{
						prog.Close();
					}
				}));
			}
		}

		// Token: 0x060003BA RID: 954 RVA: 0x00017C3C File Offset: 0x00015E3C
		private void InitializeUserList(string value)
		{
			Thread thread = new Thread(new ThreadStart(this.LoadEntriesUser));
			thread.Start();
		}

		// Token: 0x060003BB RID: 955 RVA: 0x00017C64 File Offset: 0x00015E64
		private void InitializeList()
		{
			bool flag = !this.stopCreation;
			if (flag)
			{
				new Thread(new ThreadStart(this.LoadAllEntries))
				{
					Name = "LoadLogbook"
				}.Start();
			}
		}

		// Token: 0x060003BC RID: 956 RVA: 0x00017CA8 File Offset: 0x00015EA8
		private void LoadAllEntries()
		{
			this.SaveEnabled = false;
			this.Entries = new ObservableCollection<AuditCheckEntries>();
			List<string[]> list = new List<string[]>();
			list.Clear();
			this.dbConnection.GetAllAuditEntries(ref list);
			for (int i = list.Count - 1; i >= 0; i--)
			{
				string[] item = list[i];
				bool flag = this.stopCreation || this.stopCreationAll;
				if (flag)
				{
					break;
				}
				string endr = "";
				bool flag2 = item[19].Equals("1");
				if (flag2)
				{
					endr = this.StrPassed.LocalizedText;
				}
				else
				{
					endr = this.StrFailed.LocalizedText;
				}
				bool flag3 = Application.Current != null;
				if (flag3)
				{
					Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate()
					{
						bool flag5 = this.Entries != null;
						if (flag5)
						{
							this.Entries.Add(new AuditCheckEntries
							{
								id = item[0],
								startDate = item[1].Replace(" ", "\n"),
								endDate = item[2].Replace(" ", "\n"),
								userName = item[3],
								userId = item[4],
								productName = item[5],
								productNumber = item[6],
								batchNumber = item[7],
								numberConveyed = item[8],
								testcard1 = item[9],
								testcardResult1 = item[10],
								testcard2 = item[11],
								testcardResult2 = item[12],
								testcard3 = item[13],
								testcardResult3 = item[14],
								testcard4 = item[15],
								testcardResult4 = item[16],
								testcard5 = item[17],
								testcardResult5 = item[18],
								endresult = endr
							});
						}
					}));
				}
			}
			bool flag4 = this.Entries != null && this.Entries.Count > 0;
			if (flag4)
			{
				this.SelectedEntry = this.Entries.First<AuditCheckEntries>();
			}
			this.SaveEnabled = true;
		}

		// Token: 0x060003BD RID: 957 RVA: 0x00017DEC File Offset: 0x00015FEC
		private void LoadEntriesUser()
		{
			Progress prog = null;
			bool flag = Application.Current != null;
			if (flag)
			{
				Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate()
				{
					prog = new Progress("Filtering entries...", false);
					prog.Show();
				}));
			}
			int i = 0;
			this.Entries = new ObservableCollection<AuditCheckEntries>();
			List<string[]> entries = new List<string[]>();
			entries.Clear();
			this.Entries.Clear();
			this.dbConnection.GetAllAuditEntriesWithUserID(ref entries, this.SelectedDataMode);
			Action <>9__3;
			for (int j = entries.Count - 1; j >= 0; j--)
			{
				string[] item = entries[j];
				bool flag2 = this.stopCreation;
				if (flag2)
				{
					break;
				}
				string endr = "";
				bool flag3 = item[19].Equals("1");
				if (flag3)
				{
					endr = this.StrPassed.LocalizedText;
				}
				else
				{
					endr = this.StrFailed.LocalizedText;
				}
				bool flag4 = Application.Current != null;
				if (flag4)
				{
					Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate()
					{
						AuditCheckEntries item = new AuditCheckEntries
						{
							id = item[0],
							startDate = item[1].Replace(" ", "\n"),
							endDate = item[2].Replace(" ", "\n"),
							userName = item[3],
							userId = item[4],
							productName = item[5],
							productNumber = item[6],
							batchNumber = item[7],
							numberConveyed = item[8],
							testcard1 = item[9],
							testcardResult1 = item[10],
							testcard2 = item[11],
							testcardResult2 = item[12],
							testcard3 = item[13],
							testcardResult3 = item[14],
							testcard4 = item[15],
							testcardResult4 = item[16],
							testcard5 = item[17],
							testcardResult5 = item[18],
							endresult = endr
						};
						this.Entries.Add(item);
					}));
				}
				bool flag5 = Application.Current != null;
				if (flag5)
				{
					Dispatcher dispatcher = Application.Current.Dispatcher;
					DispatcherPriority priority = DispatcherPriority.Background;
					Action method;
					if ((method = <>9__3) == null)
					{
						method = (<>9__3 = delegate()
						{
							bool flag7 = prog != null;
							if (flag7)
							{
								prog.progBar.Value = (double)(i * 100 / entries.Count);
							}
						});
					}
					dispatcher.Invoke(priority, method);
				}
				int i2 = i;
				i = i2 + 1;
			}
			bool flag6 = Application.Current != null;
			if (flag6)
			{
				Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate()
				{
					bool flag7 = prog != null;
					if (flag7)
					{
						prog.Close();
					}
				}));
			}
		}

		// Token: 0x060003BE RID: 958 RVA: 0x00017FF8 File Offset: 0x000161F8
		private void InitData()
		{
			this.Modes.Add("None");
			this.Modes.Add(this.StrUser.LocalizedText);
			this.Modes.Add(this.StrProduct.LocalizedText);
			this.Modes.Add(this.StrTimeIntervall.LocalizedText);
			this.Modes.Add(this.StrEndResult.LocalizedText);
			this.SelectedMode = this.Modes.First<string>();
		}

		// Token: 0x060003BF RID: 959 RVA: 0x00018088 File Offset: 0x00016288
		public string drawList(ObservableCollection<AuditCheckEntries> entries, string usbPath)
		{
			PdfDocument pdfDocument = new PdfDocument();
			PdfPage pdfPage = new PdfPage();
			pdfPage = pdfDocument.AddPage();
			XSize xsize = PageSizeConverter.ToSize(PageSize.A4);
			pdfPage.Height = xsize.Height;
			pdfPage.Width = xsize.Width;
			pdfPage.Orientation = PageOrientation.Landscape;
			int num = 50;
			int num2 = 800;
			int num3 = 75;
			int num4 = 1;
			XGraphics xgraphics = XGraphics.FromPdfPage(pdfPage);
			XFont font = new XFont("Calibri", 9.0);
			XFont font2 = new XFont("Calibri", 40.0);
			XPen pen = new XPen(XColors.Black);
			XTextFormatter xtextFormatter = new XTextFormatter(xgraphics);
			string text = "";
			string text2 = "";
			string text3 = "";
			string text4 = "";
			ushort num5 = 0;
			ushort num6 = 0;
			this.dbConnection.GetDeviceName(ref text);
			this.dbConnection.GetLineName(ref text2);
			this.dbConnection.GetSerialNumber(ref text3);
			MachineType machineType = MachineType.MachineTypeUndefined;
			this.dbConnection.GetMachineType(ref machineType);
			bool flag = this.dbConnection.GetDesign() == Definition.GUIDesign.Bizerba;
			if (flag)
			{
				bool flag2 = machineType == MachineType.RayconXRS;
				if (flag2)
				{
					text4 = "XRE";
				}
				else
				{
					bool flag3 = machineType == MachineType.RayconD;
					if (flag3)
					{
						text4 = "XRE-D";
					}
					else
					{
						bool flag4 = machineType == MachineType.RayconEntry;
						if (flag4)
						{
							text4 = "XRE-D";
						}
						else
						{
							bool flag5 = machineType == MachineType.RayconDPlus;
							if (flag5)
							{
								text4 = "XRE-D pro";
							}
							else
							{
								bool flag6 = machineType == MachineType.RayconEX2A;
								if (flag6)
								{
									text4 = "Raycon EX2A";
								}
							}
						}
					}
				}
			}
			else
			{
				bool flag7 = machineType == MachineType.RayconXRS;
				if (flag7)
				{
					text4 = "Raycon XRS";
				}
				else
				{
					bool flag8 = machineType == MachineType.RayconD;
					if (flag8)
					{
						text4 = "Raycon D";
					}
					else
					{
						bool flag9 = machineType == MachineType.RayconEntry;
						if (flag9)
						{
							text4 = "Raycon EX1";
						}
						else
						{
							bool flag10 = machineType == MachineType.RayconDPlus;
							if (flag10)
							{
								text4 = "Raycon D+";
							}
							else
							{
								bool flag11 = machineType == MachineType.RayconEX2A;
								if (flag11)
								{
									text4 = "Raycon EX2A";
								}
							}
						}
					}
				}
			}
			this.dbConnection.GetInspectionWidth(ref num6);
			this.dbConnection.GetInspectionHeight(ref num5);
			Definition.GUIDesign design = this.dbConnection.GetDesign();
			bool flag12 = design == Definition.GUIDesign.Bizerba;
			if (flag12)
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
			bool flag13 = Directory.Exists(path);
			if (flag13)
			{
				string[] files = Directory.GetFiles(path);
				foreach (string text5 in files)
				{
					bool flag14 = text5.EndsWith(Definition.pngFileEnding);
					if (flag14)
					{
						XImage ximage = XImage.FromFile(text5);
						float num7 = (float)ximage.PixelHeight;
						float num8 = (float)ximage.PixelWidth;
						bool flag15 = num8 / num7 > 4f;
						if (flag15)
						{
							float num9 = num8 / 200f;
							num8 = 200f;
							num7 /= num9;
						}
						else
						{
							float num9 = num7 / 50f;
							num7 = 50f;
							num8 /= num9;
						}
						xgraphics.DrawImage(ximage, (double)(800f - num8), 60.0, (double)num8, (double)num7);
						break;
					}
				}
			}
			xtextFormatter.DrawString(num4.ToString(), font, XBrushes.Black, new XRect(780.0, 570.0, 30.0, 30.0));
			string text6 = string.Concat(new string[]
			{
				text4,
				" ",
				num6.ToString(),
				"/",
				num5.ToString(),
				"\n",
				text,
				"\n",
				text2,
				"\n",
				text3
			});
			bool flag16 = this.SelectedMode.Equals(this.StrTimeIntervall.LocalizedText);
			if (flag16)
			{
				xtextFormatter.DrawString(string.Concat(new string[]
				{
					"Filter: ",
					this.StrStartTime.LocalizedText,
					" = ",
					this.SelectedDataMode,
					", ",
					this.StrEndTime.LocalizedText,
					" = ",
					this.SelectedDataMode2
				}), font, XBrushes.Black, new XRect((double)(num + 270), 125.0, 470.0, 80.0));
			}
			else
			{
				xtextFormatter.DrawString("Filter: " + this.SelectedMode + " = " + this.SelectedDataMode, font, XBrushes.Black, new XRect((double)(num + 270), 125.0, 470.0, 80.0));
			}
			xgraphics.DrawRectangle(XPens.Black, XBrushes.Transparent, new XRect((double)(num + 260), 120.0, 490.0, 80.0));
			xtextFormatter.DrawString(text6, font, XBrushes.Black, new XRect((double)(num + 10), 125.0, 230.0, 80.0));
			xgraphics.DrawRectangle(XPens.Black, XBrushes.Transparent, new XRect((double)num, 120.0, 250.0, 80.0));
			int num10 = 220;
			int num11 = 30;
			xgraphics.DrawLine(pen, (double)num, (double)num10, (double)num2, (double)num10);
			xgraphics.DrawLine(pen, (double)num, (double)num10, (double)num, (double)(num10 + num11));
			xgraphics.DrawLine(pen, (double)num2, (double)num10, (double)num2, (double)(num10 + num11));
			xgraphics.DrawLine(pen, (double)num, (double)(num10 + num11), (double)num2, (double)(num10 + num11));
			int num12 = num + num3;
			for (int j = 0; j < 8; j++)
			{
				bool flag17 = j != 5;
				if (flag17)
				{
					xgraphics.DrawLine(pen, (double)num12, (double)num10, (double)num12, (double)(num10 + num11));
					num12 += num3;
				}
				else
				{
					num12 += num3 * 2;
				}
			}
			xtextFormatter.DrawString(this.StrStartTime.LocalizedText, font, XBrushes.Black, new XRect((double)(num + 5), (double)(num10 + 5), (double)(num + num3 - 5), (double)(num10 + num11 - 5)));
			xtextFormatter.DrawString(this.StrEndTime.LocalizedText, font, XBrushes.Black, new XRect((double)(num + num3 + 5), (double)(num10 + 5), (double)(num + 2 * num3 - 5), (double)(num10 + num11 - 5)));
			xtextFormatter.DrawString(this.StrUser.LocalizedText, font, XBrushes.Black, new XRect((double)(num + 2 * num3 + 5), (double)(num10 + 5), (double)(num + 3 * num3 - 5), (double)(num10 + num11 - 5)));
			xtextFormatter.DrawString(this.StrProduct.LocalizedText, font, XBrushes.Black, new XRect((double)(num + 3 * num3 + 5), (double)(num10 + 5), (double)(num3 - 10), (double)(num11 - 10)));
			xtextFormatter.DrawString(this.StrNumberContaminations.LocalizedText, font, XBrushes.Black, new XRect((double)(num + 4 * num3 + 5), (double)(num10 + 5), (double)(num3 - 10), (double)(num11 - 10)));
			xtextFormatter.DrawString(this.StrTestballs.LocalizedText, font, XBrushes.Black, new XRect((double)(num + 5 * num3 + 5), (double)(num10 + 5), (double)(num3 - 10), (double)(num11 - 10)));
			xtextFormatter.DrawString(this.StrResult.LocalizedText, font, XBrushes.Black, new XRect((double)(num + 8 * num3 + 5), (double)(num10 + 5), (double)(num3 - 10), (double)(num11 - 10)));
			xtextFormatter.DrawString(this.StrEndResult.LocalizedText, font, XBrushes.Black, new XRect((double)(num + 9 * num3 + 5), (double)(num10 + 5), (double)(num3 - 10), (double)(num11 - 10)));
			num10 += num11;
			for (int k = entries.Count - 1; k >= 0; k--)
			{
				AuditCheckEntries auditCheckEntries = entries[k];
				bool flag18 = !auditCheckEntries.testcard4.Equals(" ") && !auditCheckEntries.testcard4.Equals("");
				int num13;
				if (flag18)
				{
					num13 = 60;
				}
				else
				{
					num13 = 45;
				}
				bool flag19 = num10 + num13 > 550;
				if (flag19)
				{
					num10 = 80;
					num4++;
					pdfPage = pdfDocument.AddPage();
					pdfPage.Height = xsize.Height;
					pdfPage.Width = xsize.Width;
					pdfPage.Orientation = PageOrientation.Landscape;
					xgraphics = XGraphics.FromPdfPage(pdfPage);
					xtextFormatter = new XTextFormatter(xgraphics);
					XFont font3 = new XFont("Calibri", 12.0);
					bool flag20 = design == Definition.GUIDesign.Bizerba;
					if (flag20)
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
					xgraphics.DrawString(text + "; " + text3, font3, XBrushes.Black, 60.0, 50.0);
					xgraphics.DrawString(this.StrAuditCheck.LocalizedText, font3, XBrushes.Black, 720.0, 50.0);
					xgraphics.DrawLine(new XPen(XColor.FromArgb(184, 228, 9), 2.0), (double)num, 60.0, (double)num2, 60.0);
					xtextFormatter.DrawString(num4.ToString(), font, XBrushes.Black, new XRect(780.0, 570.0, 30.0, 30.0));
				}
				bool flag21 = num10 <= 80;
				if (flag21)
				{
					num11 = 30;
					xgraphics.DrawLine(pen, (double)num, (double)num10, (double)num2, (double)num10);
					xgraphics.DrawLine(pen, (double)num, (double)num10, (double)num, (double)(num10 + num11));
					xgraphics.DrawLine(pen, (double)num2, (double)num10, (double)num2, (double)(num10 + num11));
					xgraphics.DrawLine(pen, (double)num, (double)(num10 + num11), (double)num2, (double)(num10 + num11));
					num12 = num + num3;
					for (int l = 0; l < 8; l++)
					{
						bool flag22 = l != 5;
						if (flag22)
						{
							xgraphics.DrawLine(pen, (double)num12, (double)num10, (double)num12, (double)(num10 + num11));
							num12 += num3;
						}
						else
						{
							num12 += num3 * 2;
						}
					}
					xtextFormatter.DrawString(this.StrStartTime.LocalizedText, font, XBrushes.Black, new XRect((double)(num + 5), (double)(num10 + 5), (double)(num + num3 - 5), (double)(num10 + num11 - 5)));
					xtextFormatter.DrawString(this.StrEndTime.LocalizedText, font, XBrushes.Black, new XRect((double)(num + num3 + 5), (double)(num10 + 5), (double)(num + 2 * num3 - 5), (double)(num10 + num11 - 5)));
					xtextFormatter.DrawString(this.StrUser.LocalizedText, font, XBrushes.Black, new XRect((double)(num + 2 * num3 + 5), (double)(num10 + 5), (double)(num + 3 * num3 - 5), (double)(num10 + num11 - 5)));
					xtextFormatter.DrawString(this.StrProduct.LocalizedText, font, XBrushes.Black, new XRect((double)(num + 3 * num3 + 5), (double)(num10 + 5), (double)(num3 - 10), (double)(num11 - 10)));
					xtextFormatter.DrawString(this.StrNumberContaminations.LocalizedText, font, XBrushes.Black, new XRect((double)(num + 4 * num3 + 5), (double)(num10 + 5), (double)(num3 - 10), (double)(num11 - 10)));
					xtextFormatter.DrawString(this.StrTestballs.LocalizedText, font, XBrushes.Black, new XRect((double)(num + 5 * num3 + 5), (double)(num10 + 5), (double)(num3 - 10), (double)(num11 - 10)));
					xtextFormatter.DrawString(this.StrResult.LocalizedText, font, XBrushes.Black, new XRect((double)(num + 8 * num3 + 5), (double)(num10 + 5), (double)(num3 - 10), (double)(num11 - 10)));
					xtextFormatter.DrawString(this.StrEndResult.LocalizedText, font, XBrushes.Black, new XRect((double)(num + 9 * num3 + 5), (double)(num10 + 5), (double)(num3 - 10), (double)(num11 - 10)));
					num10 += num11;
				}
				List<string[]> list = new List<string[]>();
				xgraphics.DrawLine(pen, (double)num, (double)num10, (double)num, (double)(num10 + num13));
				xgraphics.DrawLine(pen, (double)num2, (double)num10, (double)num2, (double)(num10 + num13));
				xgraphics.DrawLine(pen, (double)num, (double)(num10 + num13), (double)num2, (double)(num10 + num13));
				int num14 = num + num3;
				for (int m = 0; m < 8; m++)
				{
					bool flag23 = m != 5;
					if (flag23)
					{
						xgraphics.DrawLine(pen, (double)num14, (double)num10, (double)num14, (double)(num10 + num13));
						num14 += num3;
					}
					else
					{
						num14 += num3 * 2;
					}
				}
				xtextFormatter.DrawString(auditCheckEntries.startDate, font, XBrushes.Black, new XRect((double)(num + 5), (double)(num10 + 5), (double)(num3 - 10), (double)(num13 - 10)));
				string text7 = DateTime.Now.ToString();
				xtextFormatter.DrawString(auditCheckEntries.endDate, font, XBrushes.Black, new XRect((double)(num + num3 + 5), (double)(num10 + 5), (double)(num3 - 10), (double)(num13 - 10)));
				string text8 = auditCheckEntries.userName;
				double num15 = xgraphics.MeasureString(text8, font).Width / (double)(num3 - 10);
				bool flag24 = num15 > 1.0;
				if (flag24)
				{
					int num16 = (int)((double)text8.Length / num15);
					int length = num16 - 1;
					text8 = text8.Substring(0, length);
				}
				string text9 = auditCheckEntries.productName;
				num15 = xgraphics.MeasureString(text9, font).Width / (double)(num3 - 10);
				bool flag25 = num15 > 1.0;
				if (flag25)
				{
					int num17 = (int)((double)text9.Length / num15);
					int length2 = num17;
					text9 = text9.Substring(0, length2);
				}
				string text10 = auditCheckEntries.batchNumber;
				num15 = xgraphics.MeasureString(text10, font).Width / (double)(num3 - 10);
				bool flag26 = num15 > 1.0;
				if (flag26)
				{
					int num18 = (int)((double)text10.Length / num15);
					int length3 = num18;
					text10 = text10.Substring(0, length3);
				}
				xtextFormatter.DrawString(text8 + "\nID: " + auditCheckEntries.userId, font, XBrushes.Black, new XRect((double)(num + 2 * num3 + 5), (double)(num10 + 5), (double)(num3 - 10), (double)(num13 - 10)));
				xtextFormatter.DrawString(string.Concat(new string[]
				{
					text9,
					"\n",
					this.StrProductNumber.LocalizedText,
					" ",
					auditCheckEntries.productNumber,
					"\n",
					text10
				}), font, XBrushes.Black, new XRect((double)(num + 3 * num3 + 5), (double)(num10 + 5), (double)(num3 - 10), (double)(num13 - 10)));
				xtextFormatter.DrawString(auditCheckEntries.numberConveyed.ToString(), font, XBrushes.Black, new XRect((double)(num + 4 * num3 + 5), (double)(num10 + 5), (double)(num3 - 10), (double)(num13 - 10)));
				int num19 = num10;
				int num20 = 10;
				bool flag27 = !auditCheckEntries.testcard1.Equals("") && !auditCheckEntries.testcard1.Equals(" ");
				if (flag27)
				{
					xtextFormatter.DrawString(auditCheckEntries.testcard1, font, XBrushes.Black, new XRect((double)(num + 5 * num3 + 5), (double)(num19 + 5), (double)(num3 * 3 - 6), (double)num20));
					xtextFormatter.DrawString(auditCheckEntries.testcardResult1, font, XBrushes.Black, new XRect((double)(num + 8 * num3 + 5), (double)(num19 + 5), (double)(num3 - 10), (double)num20));
					num19 += num20;
				}
				bool flag28 = !auditCheckEntries.testcard2.Equals("") && !auditCheckEntries.testcard2.Equals(" ");
				if (flag28)
				{
					xtextFormatter.DrawString(auditCheckEntries.testcard2, font, XBrushes.Black, new XRect((double)(num + 5 * num3 + 5), (double)(num19 + 5), (double)(num3 * 3 - 10), (double)num20));
					xtextFormatter.DrawString(auditCheckEntries.testcardResult2, font, XBrushes.Black, new XRect((double)(num + 8 * num3 + 5), (double)(num19 + 5), (double)(num3 - 10), (double)num20));
					num19 += num20;
				}
				bool flag29 = !auditCheckEntries.testcard3.Equals("") && !auditCheckEntries.testcard3.Equals(" ");
				if (flag29)
				{
					xtextFormatter.DrawString(auditCheckEntries.testcard3, font, XBrushes.Black, new XRect((double)(num + 5 * num3 + 5), (double)(num19 + 5), (double)(num3 * 3 - 10), (double)num20));
					xtextFormatter.DrawString(auditCheckEntries.testcardResult3, font, XBrushes.Black, new XRect((double)(num + 8 * num3 + 5), (double)(num19 + 5), (double)(num3 - 10), (double)num20));
					num19 += num20;
				}
				bool flag30 = !auditCheckEntries.testcard4.Equals("") && !auditCheckEntries.testcard4.Equals(" ");
				if (flag30)
				{
					xtextFormatter.DrawString(auditCheckEntries.testcard4, font, XBrushes.Black, new XRect((double)(num + 5 * num3 + 5), (double)(num19 + 5), (double)(num3 * 3 - 10), (double)num20));
					xtextFormatter.DrawString(auditCheckEntries.testcardResult4, font, XBrushes.Black, new XRect((double)(num + 8 * num3 + 5), (double)(num19 + 5), (double)(num3 - 10), (double)num20));
					num19 += num20;
				}
				bool flag31 = !auditCheckEntries.testcard5.Equals("") && !auditCheckEntries.testcard5.Equals(" ");
				if (flag31)
				{
					xtextFormatter.DrawString(auditCheckEntries.testcard5, font, XBrushes.Black, new XRect((double)(num + 5 * num3 + 5), (double)(num19 + 5), (double)(num3 * 3 - 10), (double)num20));
					xtextFormatter.DrawString(auditCheckEntries.testcardResult5, font, XBrushes.Black, new XRect((double)(num + 8 * num3 + 5), (double)(num19 + 5), (double)(num3 - 10), (double)num20));
					num19 += num20;
				}
				bool flag32 = auditCheckEntries.endresult.Equals(this.StrFailed.LocalizedText);
				if (flag32)
				{
					xgraphics.DrawRectangle(XBrushes.Red, (double)(num + 9 * num3 + 1), (double)(num10 + 1), (double)(num3 - 2), (double)(num13 - 2));
					xtextFormatter.DrawString(auditCheckEntries.endresult, font, XBrushes.Black, new XRect((double)(num + 9 * num3 + 5), (double)(num10 + 5), (double)(num3 - 10), (double)(num13 - 10)));
				}
				else
				{
					xgraphics.DrawRectangle(XPens.Transparent, new XSolidBrush(XColor.FromArgb(184, 228, 9)), (double)(num + 9 * num3 + 1), (double)(num10 + 1), (double)(num3 - 2), (double)(num13 - 2));
					xtextFormatter.DrawString(auditCheckEntries.endresult, font, XBrushes.Black, new XRect((double)(num + 9 * num3 + 5), (double)(num10 + 5), (double)(num3 - 10), (double)(num13 - 10)));
				}
				num10 += num13;
			}
			string text11 = "AuditCheck_Filter_" + DateTime.Now.ToString().Replace(" ", "_").Replace(".", "_").Replace("/", "_").Replace(":", "_");
			string text12 = usbPath + "\\AuditCheckFilter\\";
			bool flag33 = !Directory.Exists(text12);
			if (flag33)
			{
				Directory.CreateDirectory(text12);
			}
			pdfDocument.Save(text12 + "\\" + text11 + ".pdf");
			pdfDocument.Close();
			pdfDocument.Dispose();
			return text12 + text11 + ".pdf";
		}

		// Token: 0x060003C0 RID: 960 RVA: 0x00019670 File Offset: 0x00017870
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x060003C1 RID: 961 RVA: 0x000196A0 File Offset: 0x000178A0
		private void RegisterLocalizedText()
		{
			this.StrAuditCheck = new BoundLocalizedText("lbAuditCheck");
			this.StrData = new BoundLocalizedText("txtData");
			this.StrRequest = new BoundLocalizedText("txtRequest");
			this.StrResult = new BoundLocalizedText("txtResult");
			this.StrCheckImages = new BoundLocalizedText("txtAuditCheckImages");
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
			this.StrConvey = new BoundLocalizedText("txtNumberOfBalls");
			this.StrProductNumber = new BoundLocalizedText("txtProductNumber");
			this.StrTimeIntervall = new BoundLocalizedText("txtTimeIntervall");
		}

		// Token: 0x060003C2 RID: 962 RVA: 0x00019838 File Offset: 0x00017A38
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
			MainWindow.localizationManager.Deregister(this.StrProductNumber);
			MainWindow.localizationManager.Deregister(this.StrTimeIntervall);
		}

		// Token: 0x060003C3 RID: 963 RVA: 0x000199D0 File Offset: 0x00017BD0
		public void CleanUp(object obj)
		{
			this.stopCreation = true;
			bool flag = this.DataModes != null;
			if (flag)
			{
				this.DataModes.Clear();
				this.DataModes = null;
			}
			bool flag2 = this.Modes != null;
			if (flag2)
			{
				this.Modes.Clear();
				this.Modes = null;
			}
			bool flag3 = this.Entries != null;
			if (flag3)
			{
				this.Entries.Clear();
				this.Entries = null;
			}
			this.DeregisterLocalizedText();
		}

		// Token: 0x040002BA RID: 698
		private DBConnection dbConnection;

		// Token: 0x040002D8 RID: 728
		private bool browserVisible = false;

		// Token: 0x040002D9 RID: 729
		private bool browserNotVisible = true;

		// Token: 0x040002DA RID: 730
		private bool stopCreation = false;

		// Token: 0x040002DB RID: 731
		private List<string> modes = new List<string>();

		// Token: 0x040002DC RID: 732
		private List<string> dataModes;

		// Token: 0x040002DD RID: 733
		private ObservableCollection<AuditCheckEntries> entries;

		// Token: 0x040002DE RID: 734
		private string selectedMode;

		// Token: 0x040002DF RID: 735
		private bool dateVisible = false;

		// Token: 0x040002E0 RID: 736
		private string selectedDataMode;

		// Token: 0x040002E1 RID: 737
		private string selectedDataMode2;

		// Token: 0x040002E2 RID: 738
		private AuditCheckEntries selectedEntry;

		// Token: 0x040002E3 RID: 739
		private bool first = false;

		// Token: 0x040002E4 RID: 740
		private bool stopCreationAll = false;

		// Token: 0x040002E5 RID: 741
		private bool saveEnabled = false;
	}
}
