using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Common;
using DatabaseConnection;
using Definitions;
using XrsImage;

namespace GUI_XRS.Datalogger
{
	// Token: 0x02000269 RID: 617
	public class DatalogsPredMainViewModel : INotifyPropertyChanged
	{
		// Token: 0x1400010E RID: 270
		// (add) Token: 0x06002F53 RID: 12115 RVA: 0x00134440 File Offset: 0x00132640
		// (remove) Token: 0x06002F54 RID: 12116 RVA: 0x00134478 File Offset: 0x00132678
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x17000CAC RID: 3244
		// (get) Token: 0x06002F55 RID: 12117 RVA: 0x001344AD File Offset: 0x001326AD
		// (set) Token: 0x06002F56 RID: 12118 RVA: 0x001344B5 File Offset: 0x001326B5
		public RelayCommand ActualizeSelectedTableCommand { get; set; }

		// Token: 0x17000CAD RID: 3245
		// (get) Token: 0x06002F57 RID: 12119 RVA: 0x001344BE File Offset: 0x001326BE
		// (set) Token: 0x06002F58 RID: 12120 RVA: 0x001344C6 File Offset: 0x001326C6
		public RelayCommand SetInitTimesCommand { get; set; }

		// Token: 0x17000CAE RID: 3246
		// (get) Token: 0x06002F59 RID: 12121 RVA: 0x001344CF File Offset: 0x001326CF
		// (set) Token: 0x06002F5A RID: 12122 RVA: 0x001344D7 File Offset: 0x001326D7
		public RelayCommand ChangeStartTimeCmd { get; set; }

		// Token: 0x17000CAF RID: 3247
		// (get) Token: 0x06002F5B RID: 12123 RVA: 0x001344E0 File Offset: 0x001326E0
		// (set) Token: 0x06002F5C RID: 12124 RVA: 0x001344E8 File Offset: 0x001326E8
		public RelayCommand ChangeEndTimeCmd { get; set; }

		// Token: 0x17000CB0 RID: 3248
		// (get) Token: 0x06002F5D RID: 12125 RVA: 0x001344F1 File Offset: 0x001326F1
		// (set) Token: 0x06002F5E RID: 12126 RVA: 0x001344F9 File Offset: 0x001326F9
		public RelayCommand CleanUpCommand { get; set; }

		// Token: 0x17000CB1 RID: 3249
		// (get) Token: 0x06002F5F RID: 12127 RVA: 0x00134504 File Offset: 0x00132704
		// (set) Token: 0x06002F60 RID: 12128 RVA: 0x0013451C File Offset: 0x0013271C
		public ObservableCollection<DataLogsListColumns> DataLogsList
		{
			get
			{
				return this.dataLogsList;
			}
			set
			{
				bool flag = this.dataLogsList != value;
				if (flag)
				{
					this.dataLogsList = value;
					this.OnPropertyChanged("DataLogsList");
				}
			}
		}

		// Token: 0x17000CB2 RID: 3250
		// (get) Token: 0x06002F61 RID: 12129 RVA: 0x0013454F File Offset: 0x0013274F
		// (set) Token: 0x06002F62 RID: 12130 RVA: 0x00134557 File Offset: 0x00132757
		public List<string> DataLogsTableNameList { get; set; }

		// Token: 0x17000CB3 RID: 3251
		// (get) Token: 0x06002F63 RID: 12131 RVA: 0x00134560 File Offset: 0x00132760
		// (set) Token: 0x06002F64 RID: 12132 RVA: 0x00134578 File Offset: 0x00132778
		public string SelectedDataLogsTable
		{
			get
			{
				return this.selectedDataLogsTable;
			}
			set
			{
				bool flag = this.selectedDataLogsTable != value;
				if (flag)
				{
					bool flag2 = !value.Contains("OLD TABLES");
					if (flag2)
					{
						this.selectedDataLogsTable = value;
						this.StartTimeSelection = this.dateStartDBDataLogs;
						this.EndTimeSelection = this.dateToday;
						this.SetColumnHeaders(this.selectedDataLogsTable);
						this.GetDatafromDB(this.selectedDataLogsTable);
						this.OnPropertyChanged("SelectedDataLogsTable");
					}
				}
			}
		}

		// Token: 0x17000CB4 RID: 3252
		// (get) Token: 0x06002F65 RID: 12133 RVA: 0x001345F4 File Offset: 0x001327F4
		// (set) Token: 0x06002F66 RID: 12134 RVA: 0x0013460C File Offset: 0x0013280C
		public List<string> Header
		{
			get
			{
				return this.header;
			}
			set
			{
				bool flag = this.header != value;
				if (flag)
				{
					this.header = value;
					this.OnPropertyChanged("Header");
				}
			}
		}

		// Token: 0x17000CB5 RID: 3253
		// (get) Token: 0x06002F67 RID: 12135 RVA: 0x00134640 File Offset: 0x00132840
		// (set) Token: 0x06002F68 RID: 12136 RVA: 0x00134658 File Offset: 0x00132858
		public string StartTimeSelection
		{
			get
			{
				return this.startTimeSelection;
			}
			set
			{
				bool flag = this.startTimeSelection != value;
				if (flag)
				{
					this.startTimeSelection = value;
					this.OnPropertyChanged("StartTimeSelection");
				}
			}
		}

		// Token: 0x17000CB6 RID: 3254
		// (get) Token: 0x06002F69 RID: 12137 RVA: 0x0013468C File Offset: 0x0013288C
		// (set) Token: 0x06002F6A RID: 12138 RVA: 0x001346A4 File Offset: 0x001328A4
		public string EndTimeSelection
		{
			get
			{
				return this.endTimeSelection;
			}
			set
			{
				bool flag = this.endTimeSelection != value;
				if (flag)
				{
					this.endTimeSelection = value;
					this.OnPropertyChanged("EndTimeSelection");
				}
			}
		}

		// Token: 0x17000CB7 RID: 3255
		// (get) Token: 0x06002F6B RID: 12139 RVA: 0x001346D8 File Offset: 0x001328D8
		// (set) Token: 0x06002F6C RID: 12140 RVA: 0x001346F0 File Offset: 0x001328F0
		public int RowCount
		{
			get
			{
				return this.rowCount;
			}
			set
			{
				bool flag = this.rowCount != value;
				if (flag)
				{
					this.rowCount = value;
					this.OnPropertyChanged("RowCount");
				}
			}
		}

		// Token: 0x17000CB8 RID: 3256
		// (get) Token: 0x06002F6D RID: 12141 RVA: 0x00134724 File Offset: 0x00132924
		// (set) Token: 0x06002F6E RID: 12142 RVA: 0x0013473C File Offset: 0x0013293C
		public int ColumnCount
		{
			get
			{
				return this.columnCount;
			}
			set
			{
				bool flag = this.columnCount != value;
				if (flag)
				{
					this.columnCount = value;
					this.OnPropertyChanged("ColumnCount");
				}
			}
		}

		// Token: 0x17000CB9 RID: 3257
		// (get) Token: 0x06002F6F RID: 12143 RVA: 0x00134770 File Offset: 0x00132970
		// (set) Token: 0x06002F70 RID: 12144 RVA: 0x00134788 File Offset: 0x00132988
		public bool TimeSelectionIsPossible
		{
			get
			{
				return this.timeSelectionIsPossible;
			}
			set
			{
				bool flag = this.timeSelectionIsPossible != value;
				if (flag)
				{
					this.timeSelectionIsPossible = value;
					this.OnPropertyChanged("TimeSelectionIsPossible");
				}
			}
		}

		// Token: 0x17000CBA RID: 3258
		// (get) Token: 0x06002F71 RID: 12145 RVA: 0x001347BB File Offset: 0x001329BB
		// (set) Token: 0x06002F72 RID: 12146 RVA: 0x001347C3 File Offset: 0x001329C3
		public bool IsNotRayconDPlus { get; set; }

		// Token: 0x17000CBB RID: 3259
		// (get) Token: 0x06002F73 RID: 12147 RVA: 0x001347CC File Offset: 0x001329CC
		// (set) Token: 0x06002F74 RID: 12148 RVA: 0x001347D4 File Offset: 0x001329D4
		public bool IsRayconDPlus { get; set; }

		// Token: 0x17000CBC RID: 3260
		// (get) Token: 0x06002F75 RID: 12149 RVA: 0x001347E0 File Offset: 0x001329E0
		// (set) Token: 0x06002F76 RID: 12150 RVA: 0x001347F8 File Offset: 0x001329F8
		public bool NotWidescreen
		{
			get
			{
				return this.notWidescreen;
			}
			set
			{
				this.notWidescreen = value;
				this.OnPropertyChanged("NotWidescreen");
			}
		}

		// Token: 0x17000CBD RID: 3261
		// (get) Token: 0x06002F77 RID: 12151 RVA: 0x00134810 File Offset: 0x00132A10
		// (set) Token: 0x06002F78 RID: 12152 RVA: 0x00134828 File Offset: 0x00132A28
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

		// Token: 0x06002F79 RID: 12153 RVA: 0x00134840 File Offset: 0x00132A40
		public DatalogsPredMainViewModel(DBDataLogs dBDataLogs, DBConnection dBConnection, ParameterHandling parameterHandling)
		{
			this.dBDataLogs = dBDataLogs;
			this.dBConnection = dBConnection;
			this.parameterHandling = parameterHandling;
			MachineType machineType = MachineType.MachineTypeUndefined;
			dBConnection.GetMachineType(ref machineType);
			this.IsNotRayconDPlus = (machineType != MachineType.RayconDPlus);
			this.IsRayconDPlus = !this.IsNotRayconDPlus;
			this.NotWidescreen = (Definition.windowWidth < 1300.0);
			this.Widescreen = !this.NotWidescreen;
			MachineMode machineMode = MachineMode.Standard;
			dBConnection.GetMachineMode(ref machineMode);
			bool flag = machineMode == MachineMode.Bulk;
			if (flag)
			{
				bool flag2 = parameterHandling.ReadPar(SVT.svtEnablePath) == "1";
				if (flag2)
				{
					this.BulkSVT = true;
				}
				else
				{
					this.BulkFlaps = true;
				}
			}
			int num = 0;
			dBConnection.GetNumValves(ref num);
			for (int i = 1; i <= num; i++)
			{
				this.OnOffTimesColNamesBulkSVT.Add("Valve" + i.ToString() + "  ");
			}
			for (int j = num; j <= 20; j++)
			{
				this.OnOffTimesColNamesBulkSVT.Add("");
			}
			this.Initialize();
		}

		// Token: 0x06002F7A RID: 12154 RVA: 0x001362EC File Offset: 0x001344EC
		private void Initialize()
		{
			this.DataLogsList = new ObservableCollection<DataLogsListColumns>();
			this.currentColumnValues = new DataLogsListColumns();
			this.btnExportCsvSelection = new Button();
			this.btnExportCsvAll = new Button();
			this.btnExportPng = new Button();
			this.btnResets = new Button();
			this.dBDataLogs.MachineInfosGetStartTimeDBDatalogs(ref this.dateStartDBDataLogs);
			bool flag = this.dateStartDBDataLogs != null && this.dateStartDBDataLogs != "";
			if (flag)
			{
				this.dateStartDBDataLogs = this.dateStartDBDataLogs.Substring(0, 10);
				this.startTimeSelection = this.dateStartDBDataLogs;
			}
			this.dateToday = DateTime.Today.ToString(new CultureInfo("de-DE"));
			this.dateToday = this.dateToday.Substring(0, 10);
			this.endTimeSelection = this.dateToday;
			this.ChangeStartTimeCmd = new RelayCommand(new Action<object>(this.ChangeStartTimeSelection));
			this.ChangeEndTimeCmd = new RelayCommand(new Action<object>(this.ChangeEndTimeSelection));
			this.CleanUpCommand = new RelayCommand(new Action<object>(this.CleanUp));
			this.ActualizeSelectedTableCommand = new RelayCommand(new Action<object>(this.actualizeSelectedTable));
			this.SetInitTimesCommand = new RelayCommand(new Action<object>(this.setInitTimes));
			this.DataLogsList.Add(this.currentColumnValues);
			this.InitializeDataLogsTableNamesList();
		}

		// Token: 0x06002F7B RID: 12155 RVA: 0x00136460 File Offset: 0x00134660
		private void InitializeDataLogsTableNamesList()
		{
			this.DataLogsTableNameList = new List<string>();
			List<string> list = new List<string>();
			List<string> list2 = new List<string>();
			string[] tableNames = this.dBDataLogs.GetTableNames();
			string text = "";
			string text2 = "";
			this.dBDataLogs.MachineInfosGetSerialNumbers(ref text2, ref text);
			text2 = text2.Replace("-", "");
			text = text.Replace(":", "");
			bool flag = tableNames != null && tableNames.Length != 0;
			if (flag)
			{
				for (int i = 0; i < tableNames.Length; i++)
				{
					bool flag2 = !tableNames[i].Equals("DefaultLimits") && !tableNames[i].Equals("DataLogsSettings") && !tableNames[i].Contains("DetectorSignal") && !tableNames[i].Contains("SignalPredMaint");
					if (flag2)
					{
						bool flag3 = !tableNames[i].Contains("_");
						if (flag3)
						{
							bool flag4 = tableNames[i].Contains(DBDataLogs.Limits);
							if (flag4)
							{
								tableNames[i] += " (Current and Default)";
							}
							this.DataLogsTableNameList.Add(tableNames[i]);
						}
						else
						{
							bool flag5 = tableNames[i].Contains(text2) || (tableNames[i].Contains(text) && !text.Equals(""));
							if (flag5)
							{
								list.Add(tableNames[i]);
							}
							else
							{
								list2.Add(tableNames[i]);
							}
						}
					}
				}
				list2.Sort();
				this.DataLogsTableNameList.AddRange(list);
				this.DataLogsTableNameList.Sort();
				bool flag6 = list2.Count > 0;
				if (flag6)
				{
					this.DataLogsTableNameList.Add("OLD TABLES:");
					this.DataLogsTableNameList.AddRange(list2);
				}
				this.SelectedDataLogsTable = this.DataLogsTableNameList[0];
				this.SetColumnHeaders(this.DataLogsTableNameList[0]);
			}
		}

		// Token: 0x06002F7C RID: 12156 RVA: 0x0013666C File Offset: 0x0013486C
		public void ChangeStartTimeSelection(object obj)
		{
			string oldValue = this.StartTimeSelection;
			this.numKeyboard = new Keyboard("Start Time", oldValue, KeyboardType.Default);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_ChangeStartTime;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x06002F7D RID: 12157 RVA: 0x001366B8 File Offset: 0x001348B8
		private void numKeyboard_ChangeStartTime(object sender, KeyboardInputEventArgs e)
		{
			try
			{
				string text = e.Text.ToString().Trim(new char[]
				{
					' '
				});
				DateTime t;
				DateTime t2;
				DateTime t3;
				bool flag = text.Length == 10 && DateTime.TryParse(text, new CultureInfo("de-DE"), DateTimeStyles.None, out t) && DateTime.TryParse(this.endTimeSelection, new CultureInfo("de-DE"), DateTimeStyles.None, out t2) && DateTime.TryParse(this.dateStartDBDataLogs, new CultureInfo("de-DE"), DateTimeStyles.None, out t3);
				if (flag)
				{
					bool flag2 = t >= t3 && t <= t2;
					if (flag2)
					{
						this.StartTimeSelection = text;
						this.actualizeSelectedTable(1);
					}
				}
				this.numKeyboard.TextInputKeyboard -= this.numKeyboard_ChangeStartTime;
			}
			catch
			{
			}
		}

		// Token: 0x06002F7E RID: 12158 RVA: 0x001367A0 File Offset: 0x001349A0
		public void ChangeEndTimeSelection(object obj)
		{
			string oldValue = this.EndTimeSelection;
			this.numKeyboard = new Keyboard("End Time", oldValue, KeyboardType.Default);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_ChangeEndTime;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x06002F7F RID: 12159 RVA: 0x001367EC File Offset: 0x001349EC
		private void numKeyboard_ChangeEndTime(object sender, KeyboardInputEventArgs e)
		{
			try
			{
				string text = e.Text.ToString().Trim(new char[]
				{
					' '
				});
				DateTime t;
				DateTime t2;
				DateTime t3;
				bool flag = text.Length == 10 && DateTime.TryParse(text, new CultureInfo("de-DE"), DateTimeStyles.None, out t) && DateTime.TryParse(this.startTimeSelection, new CultureInfo("de-DE"), DateTimeStyles.None, out t2) && DateTime.TryParse(this.dateToday, new CultureInfo("de-DE"), DateTimeStyles.None, out t3);
				if (flag)
				{
					bool flag2 = t >= t2 && t <= t3;
					if (flag2)
					{
						this.EndTimeSelection = text.Replace("-", ".");
						this.actualizeSelectedTable(1);
					}
				}
				this.numKeyboard.TextInputKeyboard -= this.numKeyboard_ChangeEndTime;
			}
			catch
			{
			}
		}

		// Token: 0x06002F80 RID: 12160 RVA: 0x001368E4 File Offset: 0x00134AE4
		private void actualizeSelectedTable(object obj)
		{
			this.SetColumnHeaders(this.selectedDataLogsTable);
			this.GetDatafromDB(this.selectedDataLogsTable);
		}

		// Token: 0x06002F81 RID: 12161 RVA: 0x00136901 File Offset: 0x00134B01
		private void setInitTimes(object obj)
		{
			this.StartTimeSelection = this.dateStartDBDataLogs;
			this.EndTimeSelection = this.dateToday;
			this.SetColumnHeaders(this.selectedDataLogsTable);
			this.GetDatafromDB(this.selectedDataLogsTable);
		}

		// Token: 0x06002F82 RID: 12162 RVA: 0x00136938 File Offset: 0x00134B38
		private void GetDatafromDB(string tablename)
		{
			bool flag = !tablename.Equals("OLD TABLES:");
			if (flag)
			{
				this.DataLogsList = new ObservableCollection<DataLogsListColumns>();
				List<string> list = new List<string>();
				List<string> list2 = new List<string>();
				List<string> list3 = new List<string>();
				List<string> list4 = new List<string>();
				List<string> list5 = new List<string>();
				List<string> list6 = new List<string>();
				List<string> list7 = new List<string>();
				List<string> list8 = new List<string>();
				List<string> list9 = new List<string>();
				List<string> list10 = new List<string>();
				List<string> list11 = new List<string>();
				int num = 0;
				int num2 = 0;
				int num3 = 0;
				this.dBDataLogs.GUIGetTableLength(tablename, ref num3);
				this.dBDataLogs.GetStartAndEndIndexForTimeInterval(tablename, num3, this.startTimeSelection, this.endTimeSelection, ref num, ref num2);
				bool flag2 = tablename.Contains(DBDataLogs.MachineInfos);
				if (flag2)
				{
					this.dBDataLogs.MachineInfosAndOperatingCounterGetEntries(tablename, ref list, ref list2);
					for (int i = 0; i < num3; i++)
					{
						this.currentColumnValues = new DataLogsListColumns();
						this.currentColumnValues.Column0 = Convert.ToString(i + 1) + " ";
						this.currentColumnValues.Column1_Path = list[i] + " ";
						this.currentColumnValues.Column2 = list2[i];
						this.DataLogsList.Add(this.currentColumnValues);
					}
					this.TimeSelectionIsPossible = false;
				}
				else
				{
					bool flag3 = tablename.Contains(DBDataLogs.OperatingCounter);
					if (flag3)
					{
						this.dBDataLogs.MachineInfosAndOperatingCounterGetEntries(tablename, ref list, ref list2);
						for (int j = 0; j < num3; j++)
						{
							this.currentColumnValues = new DataLogsListColumns();
							this.currentColumnValues.Column0 = Convert.ToString(j + 1) + " ";
							this.currentColumnValues.Column1_Path = list[j] + " ";
							this.currentColumnValues.Column2 = list2[j];
							this.DataLogsList.Add(this.currentColumnValues);
						}
						this.TimeSelectionIsPossible = false;
					}
					else
					{
						bool flag4 = tablename.Contains(DBDataLogs.Temperatures);
						if (flag4)
						{
							this.dBDataLogs.TemperaturesGetEntries(tablename, ref list, ref list2, ref list3, ref list4);
							bool flag5 = num > 0 && num2 > 0;
							if (flag5)
							{
								for (int k = num - 1; k <= num2 - 1; k++)
								{
									this.currentColumnValues = new DataLogsListColumns();
									this.currentColumnValues.Column0 = Convert.ToString(k + 1) + " ";
									this.currentColumnValues.Column1_DateTime = list[k] + " ";
									this.currentColumnValues.Column2 = list2[k];
									this.currentColumnValues.Column3 = list3[k];
									this.currentColumnValues.Column4 = list4[k];
									this.DataLogsList.Add(this.currentColumnValues);
								}
							}
							this.TimeSelectionIsPossible = true;
						}
						else
						{
							bool flag6 = tablename.Contains(DBDataLogs.XRayOnTimes);
							if (flag6)
							{
								this.dBDataLogs.XRayOnTimesGetEntries(tablename, ref list, ref list2, ref list3, ref list4);
								for (int l = 0; l < num3; l++)
								{
									this.currentColumnValues = new DataLogsListColumns();
									this.currentColumnValues.Column0 = Convert.ToString(l + 1) + " ";
									this.currentColumnValues.Column1_DateTime = list[l] + " ";
									this.currentColumnValues.Column2 = list2[l];
									this.currentColumnValues.Column3 = list3[l];
									this.currentColumnValues.Column4 = list4[l];
									this.DataLogsList.Add(this.currentColumnValues);
								}
								this.TimeSelectionIsPossible = false;
							}
							else
							{
								bool flag7 = tablename.Contains(DBDataLogs.FilamentCurrent);
								if (flag7)
								{
									this.dBDataLogs.FilamentCurrentGetEntries(tablename, ref list, ref list2, ref list3, ref list4, ref list5, ref list6, ref list7);
									bool flag8 = num > 0 && num2 > 0;
									if (flag8)
									{
										for (int m = num - 1; m <= num2 - 1; m++)
										{
											this.currentColumnValues = new DataLogsListColumns();
											this.currentColumnValues.Column0 = Convert.ToString(m + 1) + " ";
											this.currentColumnValues.Column1_DateTime = list[m] + " ";
											this.currentColumnValues.Column2 = list2[m];
											this.currentColumnValues.Column3 = list3[m];
											this.currentColumnValues.Column4 = list4[m];
											this.currentColumnValues.Column5 = list5[m];
											this.currentColumnValues.Column6 = list6[m];
											this.currentColumnValues.Column7 = list7[m];
											this.DataLogsList.Add(this.currentColumnValues);
										}
									}
									this.TimeSelectionIsPossible = true;
								}
								else
								{
									bool flag9 = tablename.Contains(DBDataLogs.Arcs);
									if (flag9)
									{
										this.dBDataLogs.ArcsGetEntriesType0(tablename, ref list, ref list2, ref list3);
										bool flag10 = num > 0 && num2 > 0;
										if (flag10)
										{
											for (int n = num - 1; n <= num2 - 1; n++)
											{
												this.currentColumnValues = new DataLogsListColumns();
												this.currentColumnValues.Column0 = Convert.ToString(n + 1) + " ";
												this.currentColumnValues.Column1_DateTime = list[n] + " ";
												this.currentColumnValues.Column2 = list2[n];
												this.DataLogsList.Add(this.currentColumnValues);
											}
										}
										this.TimeSelectionIsPossible = true;
									}
									else
									{
										bool flag11 = tablename.Contains(DBDataLogs.SwitchOnCycles);
										if (flag11)
										{
											this.dBDataLogs.XRaySwitchOnGetEntries(tablename, ref list);
											bool flag12 = num > 0 && num2 > 0;
											if (flag12)
											{
												for (int num4 = num - 1; num4 <= num2 - 1; num4++)
												{
													this.currentColumnValues = new DataLogsListColumns();
													this.currentColumnValues.Column0 = Convert.ToString(num4 + 1) + " ";
													this.currentColumnValues.Column1_DateTime = list[num4] + " ";
													this.DataLogsList.Add(this.currentColumnValues);
												}
											}
											this.TimeSelectionIsPossible = true;
										}
										else
										{
											bool flag13 = tablename.Contains(DBDataLogs.DetectorOnTimes);
											if (flag13)
											{
												this.dBDataLogs.DetectorOnTimesGetEntries(tablename, ref list, ref list2, ref list3, ref list4, false);
												for (int num5 = 0; num5 < num3; num5++)
												{
													this.currentColumnValues = new DataLogsListColumns();
													this.currentColumnValues.Column0 = Convert.ToString(num5 + 1) + " ";
													this.currentColumnValues.Column1_DateTime = list[num5] + " ";
													this.currentColumnValues.Column2 = list2[num5];
													this.currentColumnValues.Column3 = list3[num5];
													this.currentColumnValues.Column4 = list4[num5];
													this.DataLogsList.Add(this.currentColumnValues);
												}
												this.TimeSelectionIsPossible = false;
											}
											else
											{
												bool flag14 = tablename.Contains(DBDataLogs.BackgroundIndex);
												if (flag14)
												{
													this.dBDataLogs.BackgroundIndexGetEntries(tablename, ref list, ref list2, ref list3, ref list4, ref list5, ref list6, ref list7, ref list8, ref list9);
													bool flag15 = num > 0 && num2 > 0;
													if (flag15)
													{
														for (int num6 = num - 1; num6 <= num2 - 1; num6++)
														{
															this.currentColumnValues = new DataLogsListColumns();
															this.currentColumnValues.Column0 = Convert.ToString(num6 + 1) + " ";
															this.currentColumnValues.Column1_DateTime = list[num6] + " ";
															this.currentColumnValues.Column2 = list2[num6];
															this.currentColumnValues.Column3 = list3[num6];
															this.currentColumnValues.Column4 = list4[num6];
															this.currentColumnValues.Column5 = list5[num6];
															this.currentColumnValues.Column6 = list6[num6];
															this.currentColumnValues.Column7 = list7[num6];
															this.currentColumnValues.Column8 = list8[num6];
															this.currentColumnValues.Column9 = list9[num6];
															this.DataLogsList.Add(this.currentColumnValues);
														}
													}
													this.TimeSelectionIsPossible = true;
												}
												else
												{
													bool flag16 = tablename.Contains(DBDataLogs.ResetLogs);
													if (flag16)
													{
														this.dBDataLogs.ResetLogsGetEntries(tablename, ref list, ref list2, ref list3, ref list4);
														bool flag17 = num > 0 && num2 > 0;
														if (flag17)
														{
															for (int num7 = num - 1; num7 <= num2 - 1; num7++)
															{
																this.currentColumnValues = new DataLogsListColumns();
																this.currentColumnValues.Column0 = Convert.ToString(num7 + 1) + " ";
																this.currentColumnValues.Column1_DateTime = list[num7] + " ";
																this.currentColumnValues.Column2 = list2[num7];
																this.currentColumnValues.Column3 = list3[num7];
																this.currentColumnValues.Column4 = list4[num7];
																this.DataLogsList.Add(this.currentColumnValues);
															}
														}
														this.TimeSelectionIsPossible = true;
													}
													else
													{
														bool flag18 = tablename.Contains("Limits");
														if (flag18)
														{
															List<string[]> defaultLimits = this.dBDataLogs.getDefaultLimits();
															int num8 = 1;
															int num9 = 1;
															MachineType machineType = MachineType.MachineTypeUndefined;
															this.dBConnection.GetMachineType(ref machineType);
															MachineMode machineMode = MachineMode.Standard;
															this.dBConnection.GetMachineMode(ref machineMode);
															int num10 = 0;
															string tableName = "";
															this.dBConnection.GetCurrentProductTableName(ref tableName);
															this.dBConnection.GetFlapsCount(tableName, ref num10);
															bool flag19 = this.parameterHandling.ReadPar(SVT.svtEnablePath) == "1";
															bool flag20 = machineMode != MachineMode.BulkObject && (machineMode != MachineMode.Bulk || num10 != 1) && !flag19;
															if (flag20)
															{
																num9 = 2;
															}
															bool flag21 = machineType != MachineType.RayconEntry && machineMode != MachineMode.BulkObject && (machineMode != MachineMode.Bulk || num10 != 1) && !flag19;
															if (flag21)
															{
																string a = this.parameterHandling.ReadPar(Definition.ioSubversionPath);
																bool flag22 = a != "1" && a != "3" && a != "4";
																if (flag22)
																{
																	num9 = 4;
																}
																bool flag23 = machineMode == MachineMode.Bulk && this.parameterHandling.ReadPar(SVT.svtEnablePath).Equals("1");
																if (flag23)
																{
																	num9 = 1;
																}
															}
															foreach (string[] array in defaultLimits)
															{
																bool flag24 = array[0].Equals("Temperature");
																if (flag24)
																{
																	this.currentColumnValues = new DataLogsListColumns();
																	this.currentColumnValues.Column0 = num8.ToString();
																	num8++;
																	this.currentColumnValues.Column1_DateTime = "Operating Parameters";
																	this.DataLogsList.Add(this.currentColumnValues);
																}
																else
																{
																	bool flag25 = array[0].Equals("TemperatureSource");
																	if (flag25)
																	{
																		this.currentColumnValues = new DataLogsListColumns();
																		this.currentColumnValues.Column0 = num8.ToString();
																		num8++;
																		this.currentColumnValues.Column1_DateTime = "Source";
																		this.DataLogsList.Add(this.currentColumnValues);
																	}
																	else
																	{
																		bool flag26 = array[0].Equals("TemperatureDetector");
																		if (flag26)
																		{
																			this.currentColumnValues = new DataLogsListColumns();
																			this.currentColumnValues.Column0 = num8.ToString();
																			num8++;
																			this.currentColumnValues.Column1_DateTime = "Detector ";
																			this.DataLogsList.Add(this.currentColumnValues);
																		}
																		else
																		{
																			bool flag27 = array[0].Equals("OperatingHoursConveyor");
																			if (flag27)
																			{
																				this.currentColumnValues = new DataLogsListColumns();
																				this.currentColumnValues.Column0 = num8.ToString();
																				num8++;
																				this.currentColumnValues.Column1_DateTime = "Conveyor ";
																				this.DataLogsList.Add(this.currentColumnValues);
																			}
																			else
																			{
																				bool flag28 = array[0].Equals("Rejecter1");
																				if (flag28)
																				{
																					this.currentColumnValues = new DataLogsListColumns();
																					this.currentColumnValues.Column0 = num8.ToString();
																					num8++;
																					this.currentColumnValues.Column1_DateTime = "Rejecter";
																					this.DataLogsList.Add(this.currentColumnValues);
																				}
																				else
																				{
																					bool flag29 = array[0].Equals("Rejecter1PerDay");
																					if (flag29)
																					{
																						this.currentColumnValues = new DataLogsListColumns();
																						this.currentColumnValues.Column0 = num8.ToString();
																						num8++;
																						this.currentColumnValues.Column1_DateTime = "Values per Day";
																						this.DataLogsList.Add(this.currentColumnValues);
																					}
																				}
																			}
																		}
																	}
																}
																bool flag30 = (!array[0].Contains("Rejecter2") || num9 >= 2) && ((!array[0].Contains("Rejecter3") && !array[0].Contains("Rejecter4")) || num9 >= 4);
																if (flag30)
																{
																	this.currentColumnValues = new DataLogsListColumns();
																	this.currentColumnValues.Column0 = num8.ToString();
																	num8++;
																	this.currentColumnValues.Column1_DateTime = array[0];
																	this.currentColumnValues.Column5 = array[1];
																	string column = "";
																	this.dBDataLogs.GetLimitsEntry(array[0], ref column);
																	this.currentColumnValues.Column4 = column;
																	this.DataLogsList.Add(this.currentColumnValues);
																}
															}
															this.TimeSelectionIsPossible = false;
														}
														else
														{
															bool flag31 = tablename.Contains(DBDataLogs.DetectorQuality);
															if (flag31)
															{
																this.dBDataLogs.DetectorQualityGetEntries(tablename, ref list, ref list2, ref list3, ref list4, ref list5, ref list6, ref list7);
																bool flag32 = num > 0 && num2 > 0;
																if (flag32)
																{
																	for (int num11 = num - 1; num11 <= num2 - 1; num11++)
																	{
																		this.currentColumnValues = new DataLogsListColumns();
																		this.currentColumnValues.Column0 = Convert.ToString(num11 + 1) + " ";
																		this.currentColumnValues.Column1_DateTime = list[num11] + " ";
																		this.currentColumnValues.Column2 = list2[num11];
																		this.currentColumnValues.Column3 = list3[num11];
																		this.currentColumnValues.Column4 = list4[num11];
																		this.currentColumnValues.Column5 = list5[num11];
																		this.currentColumnValues.Column6 = list6[num11];
																		this.currentColumnValues.Column7 = list7[num11];
																		this.DataLogsList.Add(this.currentColumnValues);
																	}
																}
																this.TimeSelectionIsPossible = true;
															}
															else
															{
																bool flag33 = tablename.Contains(DBDataLogs.OnOffTimes);
																if (flag33)
																{
																	bool bulkSVT = this.BulkSVT;
																	if (bulkSVT)
																	{
																		List<List<string>> list12 = new List<List<string>>();
																		this.dBDataLogs.OnOffTimesGetEntriesWithSVT(ref list, ref list2, ref list3, ref list4, ref list5, ref list6, ref list7, ref list8, ref list9, ref list10, ref list11, ref list12);
																		bool flag34 = num > 0 && num2 > 0;
																		if (flag34)
																		{
																			for (int num12 = num - 1; num12 <= num2 - 1; num12++)
																			{
																				this.currentColumnValues = new DataLogsListColumns();
																				this.currentColumnValues.Column0 = Convert.ToString(num12 + 1) + " ";
																				this.currentColumnValues.Column1_DateTime = list[num12] + " ";
																				this.currentColumnValues.Column2 = list2[num12];
																				this.currentColumnValues.Column3 = list3[num12];
																				this.currentColumnValues.Column4 = list4[num12];
																				this.currentColumnValues.Column5 = list11[num12];
																				bool flag35 = list12[num12].Count > 0;
																				if (flag35)
																				{
																					this.currentColumnValues.Column7 = list12[num12][0];
																					bool flag36 = list12[num12].Count > 1;
																					if (flag36)
																					{
																						this.currentColumnValues.Column8 = list12[num12][1];
																						bool flag37 = list12[num12].Count > 2;
																						if (flag37)
																						{
																							this.currentColumnValues.Column9 = list12[num12][2];
																							bool flag38 = list12[num12].Count > 3;
																							if (flag38)
																							{
																								this.currentColumnValues.Column10 = list12[num12][3];
																								bool flag39 = list12[num12].Count > 4;
																								if (flag39)
																								{
																									this.currentColumnValues.Column11 = list12[num12][4];
																									bool flag40 = list12[num12].Count > 5;
																									if (flag40)
																									{
																										this.currentColumnValues.Column12 = list12[num12][5];
																										bool flag41 = list12[num12].Count > 6;
																										if (flag41)
																										{
																											this.currentColumnValues.Column13 = list12[num12][6];
																											bool flag42 = list12[num12].Count > 7;
																											if (flag42)
																											{
																												this.currentColumnValues.Column14 = list12[num12][7];
																												bool flag43 = list12[num12].Count > 8;
																												if (flag43)
																												{
																													this.currentColumnValues.Column15 = list12[num12][8];
																													bool flag44 = list12[num12].Count > 9;
																													if (flag44)
																													{
																														this.currentColumnValues.Column16 = list12[num12][9];
																														bool flag45 = list12[num12].Count > 10;
																														if (flag45)
																														{
																															this.currentColumnValues.Column17 = list12[num12][10];
																															bool flag46 = list12[num12].Count > 11;
																															if (flag46)
																															{
																																this.currentColumnValues.Column18 = list12[num12][11];
																																bool flag47 = list12[num12].Count > 12;
																																if (flag47)
																																{
																																	this.currentColumnValues.Column19 = list12[num12][12];
																																	bool flag48 = list12[num12].Count > 13;
																																	if (flag48)
																																	{
																																		this.currentColumnValues.Column20 = list12[num12][13];
																																		bool flag49 = list12[num12].Count > 0;
																																		if (flag49)
																																		{
																																			this.currentColumnValues.Column21 = list12[num12][14];
																																			bool flag50 = list12[num12].Count > 14;
																																			if (flag50)
																																			{
																																				this.currentColumnValues.Column22 = list12[num12][15];
																																				bool flag51 = list12[num12].Count > 15;
																																				if (flag51)
																																				{
																																					this.currentColumnValues.Column23 = list12[num12][16];
																																					bool flag52 = list12[num12].Count > 16;
																																					if (flag52)
																																					{
																																						this.currentColumnValues.Column24 = list12[num12][17];
																																						bool flag53 = list12[num12].Count > 17;
																																						if (flag53)
																																						{
																																							this.currentColumnValues.Column25 = list12[num12][18];
																																							bool flag54 = list12[num12].Count > 18;
																																							if (flag54)
																																							{
																																								this.currentColumnValues.Column26 = list12[num12][19];
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
																							}
																						}
																					}
																				}
																				this.DataLogsList.Add(this.currentColumnValues);
																			}
																		}
																	}
																	else
																	{
																		bool bulkFlaps = this.BulkFlaps;
																		if (bulkFlaps)
																		{
																			this.dBDataLogs.OnOffTimesGetEntries(ref list, ref list2, ref list3, ref list4, ref list5, ref list6, ref list7, ref list8, ref list9, ref list10, ref list11);
																			bool flag55 = num > 0 && num2 > 0;
																			if (flag55)
																			{
																				for (int num13 = num - 1; num13 <= num2 - 1; num13++)
																				{
																					this.currentColumnValues = new DataLogsListColumns();
																					this.currentColumnValues.Column0 = Convert.ToString(num13 + 1) + " ";
																					this.currentColumnValues.Column1_DateTime = list[num13] + " ";
																					this.currentColumnValues.Column2 = list2[num13];
																					this.currentColumnValues.Column3 = list3[num13];
																					this.currentColumnValues.Column4 = list4[num13];
																					this.currentColumnValues.Column5 = list11[num13];
																					this.currentColumnValues.Column6 = list5[num13];
																					this.currentColumnValues.Column7 = list6[num13];
																					this.currentColumnValues.Column8 = list7[num13];
																					this.currentColumnValues.Column9 = list8[num13];
																					this.DataLogsList.Add(this.currentColumnValues);
																				}
																			}
																		}
																		else
																		{
																			this.dBDataLogs.OnOffTimesGetEntries(ref list, ref list2, ref list3, ref list4, ref list5, ref list6, ref list7, ref list8, ref list9, ref list10, ref list11);
																			bool flag56 = num > 0 && num2 > 0;
																			if (flag56)
																			{
																				for (int num14 = num - 1; num14 <= num2 - 1; num14++)
																				{
																					this.currentColumnValues = new DataLogsListColumns();
																					this.currentColumnValues.Column0 = Convert.ToString(num14 + 1) + " ";
																					this.currentColumnValues.Column1_DateTime = list[num14] + " ";
																					this.currentColumnValues.Column2 = list2[num14];
																					this.currentColumnValues.Column3 = list3[num14];
																					this.currentColumnValues.Column4 = list4[num14];
																					this.currentColumnValues.Column5 = list5[num14];
																					this.currentColumnValues.Column6 = list6[num14];
																					this.currentColumnValues.Column7 = list7[num14];
																					this.currentColumnValues.Column8 = list8[num14];
																					this.currentColumnValues.Column9 = list9[num14];
																					this.currentColumnValues.Column10 = list10[num14];
																					this.DataLogsList.Add(this.currentColumnValues);
																				}
																			}
																			this.TimeSelectionIsPossible = true;
																		}
																	}
																}
																else
																{
																	bool flag57 = tablename.Contains(DBDataLogs.BackUps);
																	if (flag57)
																	{
																		this.dBDataLogs.BackUpsGetEntries(ref list, ref list2, ref list3, ref list4, ref list5, ref list6, ref list7, ref list8);
																		bool flag58 = num > 0 && num2 > 0;
																		if (flag58)
																		{
																			for (int num15 = num - 1; num15 <= num2 - 1; num15++)
																			{
																				this.currentColumnValues = new DataLogsListColumns();
																				this.currentColumnValues.Column0 = Convert.ToString(num15 + 1) + " ";
																				this.currentColumnValues.Column1_DateTime = list[num15] + " ";
																				this.currentColumnValues.Column2 = list2[num15];
																				this.currentColumnValues.Column3 = list3[num15];
																				this.currentColumnValues.Column4 = list4[num15];
																				this.currentColumnValues.Column5 = list5[num15];
																				this.currentColumnValues.Column6 = list6[num15];
																				this.currentColumnValues.Column7 = list7[num15];
																				this.currentColumnValues.Column8 = list8[num15];
																				this.DataLogsList.Add(this.currentColumnValues);
																			}
																			this.TimeSelectionIsPossible = true;
																		}
																	}
																	else
																	{
																		bool flag59 = tablename.Contains(DBDataLogs.DetectorFluctuation);
																		if (flag59)
																		{
																			this.dBDataLogs.DetectorFluctuationGetEntries(tablename, ref list, ref list2, ref list3, ref list4, ref list5, ref list6, ref list7, ref list8);
																			bool flag60 = num > 0 && num2 > 0;
																			if (flag60)
																			{
																				for (int num16 = num - 1; num16 <= num2 - 1; num16++)
																				{
																					this.currentColumnValues = new DataLogsListColumns();
																					this.currentColumnValues.Column0 = Convert.ToString(num16 + 1) + " ";
																					this.currentColumnValues.Column1_DateTime = list[num16] + " ";
																					this.currentColumnValues.Column2 = list2[num16];
																					this.currentColumnValues.Column3 = list3[num16];
																					this.currentColumnValues.Column4 = list4[num16];
																					this.currentColumnValues.Column5 = list5[num16];
																					this.currentColumnValues.Column6 = list6[num16];
																					this.currentColumnValues.Column7 = list7[num16];
																					this.currentColumnValues.Column8 = list8[num16];
																					this.DataLogsList.Add(this.currentColumnValues);
																				}
																			}
																			this.TimeSelectionIsPossible = true;
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
				this.SetRowAndColumsCount(tablename, num3, num, num2);
			}
		}

		// Token: 0x06002F83 RID: 12163 RVA: 0x0013855C File Offset: 0x0013675C
		private void SetRowAndColumsCount(string tablename, int tableLength, int startindex, int endindex)
		{
			bool flag = tablename.Contains(DBDataLogs.OperatingCounter) || tablename.Contains(DBDataLogs.MachineInfos) || tablename.Contains(DBDataLogs.XRayOnTimes) || tablename.Contains(DBDataLogs.Limits);
			if (flag)
			{
				this.RowCount = tableLength;
			}
			else
			{
				this.RowCount = endindex - startindex + 1;
			}
			bool flag2 = this.RowCount < 0;
			if (flag2)
			{
				this.RowCount = 0;
			}
			bool flag3 = this.Header != null;
			if (flag3)
			{
				int num = 0;
				for (int i = 0; i < this.Header.Count; i++)
				{
					bool flag4 = this.Header[i] != "";
					if (flag4)
					{
						num++;
					}
				}
				this.ColumnCount = num;
			}
		}

		// Token: 0x06002F84 RID: 12164 RVA: 0x0013862C File Offset: 0x0013682C
		private void SetColumnHeaders(string selectedTable)
		{
			this.Header = new List<string>();
			bool flag = selectedTable.Contains(DBDataLogs.MachineInfos);
			if (flag)
			{
				this.Header = this.MachineInfosColumnNames;
			}
			else
			{
				bool flag2 = selectedTable.Contains(DBDataLogs.OperatingCounter);
				if (flag2)
				{
					this.Header = this.OperatingCounterColNames;
				}
				else
				{
					bool flag3 = selectedTable.Contains(DBDataLogs.Temperatures);
					if (flag3)
					{
						this.Header = this.TemperaturesColNames;
					}
					else
					{
						bool flag4 = selectedTable.Contains(DBDataLogs.XRayOnTimes);
						if (flag4)
						{
							this.Header = this.XRayOnTimesColNames;
						}
						else
						{
							bool flag5 = selectedTable.Contains(DBDataLogs.FilamentCurrent);
							if (flag5)
							{
								this.Header = this.FilamentCurrentColNames;
							}
							else
							{
								bool flag6 = selectedTable.Contains(DBDataLogs.Arcs);
								if (flag6)
								{
									this.Header = this.ArcsColNames;
								}
								else
								{
									bool flag7 = selectedTable.Contains(DBDataLogs.SwitchOnCycles);
									if (flag7)
									{
										this.Header = this.SwitchOnCyclesColNames;
									}
									else
									{
										bool flag8 = selectedTable.Contains(DBDataLogs.DetectorOnTimes);
										if (flag8)
										{
											this.Header = this.DetectorOnTimesColNames;
										}
										else
										{
											bool flag9 = selectedTable.Contains(DBDataLogs.BackgroundIndex);
											if (flag9)
											{
												this.Header = this.BackgroundIndexColNames;
											}
											else
											{
												bool flag10 = selectedTable.Contains(DBDataLogs.ResetLogs);
												if (flag10)
												{
													this.Header = this.ResetLogsColumnNames;
												}
												else
												{
													bool flag11 = selectedTable.Contains(DBDataLogs.Limits);
													if (flag11)
													{
														this.Header = this.LimitsColNames;
													}
													else
													{
														bool flag12 = selectedTable.Contains(DBDataLogs.DetectorQuality);
														if (flag12)
														{
															this.Header = this.DetectorQualityColNames;
														}
														else
														{
															bool flag13 = selectedTable.Contains(DBDataLogs.OnOffTimes);
															if (flag13)
															{
																MachineMode machineMode = MachineMode.Standard;
																this.dBConnection.GetMachineMode(ref machineMode);
																bool bulkSVT = this.BulkSVT;
																if (bulkSVT)
																{
																	this.Header = this.OnOffTimesColNamesBulkSVT;
																}
																else
																{
																	bool bulkFlaps = this.BulkFlaps;
																	if (bulkFlaps)
																	{
																		this.Header = this.OnOffTimesColNamesBulkFlaps;
																	}
																	else
																	{
																		this.Header = this.OnOffTimesColNames;
																	}
																}
															}
															else
															{
																bool flag14 = selectedTable.Contains(DBDataLogs.BackUps);
																if (flag14)
																{
																	this.Header = this.BackUpsColNames;
																}
																else
																{
																	bool flag15 = selectedTable.Contains(DBDataLogs.DetectorFluctuation);
																	if (flag15)
																	{
																		this.Header = this.DetectorFluctuationColNames;
																	}
																	else
																	{
																		bool flag16 = selectedTable.Contains("OLD TABLES:");
																		if (flag16)
																		{
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
		}

		// Token: 0x06002F85 RID: 12165 RVA: 0x001388C4 File Offset: 0x00136AC4
		public void CreateCsv(bool createAllTables)
		{
			string text = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_ff");
			string text2 = "";
			string text3 = "DataLogs";
			List<string> list = new List<string>();
			string[] tableNames = this.dBDataLogs.GetTableNames();
			int tablelength = 0;
			bool flag = false;
			this.dBConnection.GetUsbDriveLetter(ref text2);
			string text4 = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\DataLogs\\";
			bool flag2 = !Directory.Exists(text4);
			if (flag2)
			{
				Directory.CreateDirectory(text4);
			}
			bool flag3 = Directory.Exists(text2);
			if (flag3)
			{
				bool flag4 = !Directory.Exists(text2 + text3);
				if (flag4)
				{
					Directory.CreateDirectory(text2 + "\\" + text3);
				}
				flag = true;
			}
			try
			{
				bool flag5 = tableNames != null && tableNames.Length != 0;
				if (flag5)
				{
					if (createAllTables)
					{
						Progress.ShowStaticProgress("Export all tables...", false);
						for (int i = 0; i < tableNames.Length; i++)
						{
							bool flag6 = !tableNames[i].Contains("DetectorSignal") && !tableNames[i].Contains("DefaultLimits") && !tableNames[i].Contains("DataLogsSettings") && !tableNames[i].Contains("OLD TABLES:");
							if (flag6)
							{
								string text5 = this.setExportDataName(tableNames[i]);
								string text6 = text4 + "\\" + text5 + ".csv";
								this.csvwriter = File.CreateText(text6);
								this.dBDataLogs.GUIGetTableLength(tableNames[i], ref tablelength);
								this.createOneCSVFilePerTable(tableNames[i], tablelength);
								this.csvwriter.Close();
								this.csvwriter.Dispose();
								bool flag7 = flag;
								if (flag7)
								{
									File.Copy(text6, string.Concat(new string[]
									{
										text2,
										text3,
										"\\",
										text5,
										".csv"
									}));
								}
							}
						}
					}
					else
					{
						Progress.ShowStaticProgress("Export selected table...", false);
						string text5 = this.setExportDataName(this.SelectedDataLogsTable);
						string text6 = text4 + "\\" + text5 + ".csv";
						this.csvwriter = File.CreateText(text6);
						this.dBDataLogs.GUIGetTableLength(this.SelectedDataLogsTable, ref tablelength);
						this.createOneCSVFilePerTable(this.SelectedDataLogsTable, tablelength);
						this.csvwriter.Close();
						this.csvwriter.Dispose();
						bool flag8 = flag;
						if (flag8)
						{
							File.Copy(text6, string.Concat(new string[]
							{
								text2,
								text3,
								"\\",
								text5,
								".csv"
							}));
						}
					}
					Progress.CloseStaticProgress(true);
				}
			}
			catch (UnauthorizedAccessException)
			{
				Progress.CloseStaticProgress(false);
				Dialog.Show(LocalizationManager.Localize("txtError"), string.Concat(new string[]
				{
					LocalizationManager.Localize("msgPathAccess"),
					" \"",
					Path.GetDirectoryName(text2 + "\\DataLogs"),
					"\" ",
					LocalizationManager.Localize("msgForbidden")
				}), DialogStyle.Error);
			}
			catch
			{
				Progress.CloseStaticProgress(false);
				Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtError"), DialogStyle.Error);
			}
		}

		// Token: 0x06002F86 RID: 12166 RVA: 0x00138C5C File Offset: 0x00136E5C
		private void createOneCSVFilePerTable(string tablename, int tablelength)
		{
			List<string> list = new List<string>();
			List<string> list2 = new List<string>();
			List<string> list3 = new List<string>();
			List<string> list4 = new List<string>();
			List<string> list5 = new List<string>();
			List<string> list6 = new List<string>();
			List<string> list7 = new List<string>();
			List<string> list8 = new List<string>();
			List<string> list9 = new List<string>();
			List<string> list10 = new List<string>();
			List<string> list11 = new List<string>();
			string text = "";
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			this.dBDataLogs.GUIGetTableLength(tablename, ref num3);
			this.dBDataLogs.GetStartAndEndIndexForTimeInterval(tablename, tablelength, this.startTimeSelection, this.endTimeSelection, ref num, ref num2);
			bool flag = tablename.Contains(DBDataLogs.MachineInfos);
			if (flag)
			{
				for (int i = 0; i < this.MachineInfosColumnNames.Count; i++)
				{
					bool flag2 = this.MachineInfosColumnNames[i] != "";
					if (flag2)
					{
						text = text + this.MachineInfosColumnNames[i] + ";";
					}
				}
				this.csvwriter.WriteLine(text);
				this.dBDataLogs.MachineInfosAndOperatingCounterGetEntries(tablename, ref list, ref list2);
				for (int j = 0; j < num3; j++)
				{
					string text2 = string.Concat(new string[]
					{
						Convert.ToString(j + 1),
						";",
						list[j],
						";",
						list2[j],
						";"
					});
					this.csvwriter.WriteLine(text2);
				}
			}
			else
			{
				bool flag3 = tablename.Contains(DBDataLogs.OperatingCounter);
				if (flag3)
				{
					for (int k = 0; k < this.OperatingCounterColNames.Count; k++)
					{
						bool flag4 = this.OperatingCounterColNames[k] != "";
						if (flag4)
						{
							text = text + this.OperatingCounterColNames[k] + ";";
						}
					}
					this.csvwriter.WriteLine(text);
					this.dBDataLogs.MachineInfosAndOperatingCounterGetEntries(tablename, ref list, ref list2);
					for (int l = 0; l < num3; l++)
					{
						string text2 = string.Concat(new string[]
						{
							Convert.ToString(l + 1),
							";",
							list[l],
							";",
							list2[l],
							";"
						});
						this.csvwriter.WriteLine(text2);
					}
				}
				else
				{
					bool flag5 = tablename.Contains(DBDataLogs.Temperatures);
					if (flag5)
					{
						for (int m = 0; m < this.TemperaturesColNames.Count; m++)
						{
							bool flag6 = this.TemperaturesColNames[m] != "";
							if (flag6)
							{
								text = text + this.TemperaturesColNames[m] + ";";
							}
						}
						this.csvwriter.WriteLine(text);
						this.dBDataLogs.TemperaturesGetEntries(tablename, ref list, ref list2, ref list3, ref list4);
						bool flag7 = num > 0 && num2 > 0;
						if (flag7)
						{
							for (int n = num - 1; n <= num2 - 1; n++)
							{
								string text2 = string.Concat(new string[]
								{
									Convert.ToString(n + 1),
									";",
									list[n],
									";",
									list2[n],
									";",
									list3[n],
									";",
									list4[n],
									";"
								});
								this.csvwriter.WriteLine(text2);
							}
						}
					}
					else
					{
						bool flag8 = tablename.Contains(DBDataLogs.XRayOnTimes);
						if (flag8)
						{
							for (int num4 = 0; num4 < this.XRayOnTimesColNames.Count; num4++)
							{
								bool flag9 = this.XRayOnTimesColNames[num4] != "";
								if (flag9)
								{
									text = text + this.XRayOnTimesColNames[num4] + ";";
								}
							}
							this.csvwriter.WriteLine(text);
							this.dBDataLogs.XRayOnTimesGetEntries(tablename, ref list, ref list2, ref list3, ref list4);
							for (int num5 = 0; num5 < num3; num5++)
							{
								string text2 = string.Concat(new string[]
								{
									Convert.ToString(num5 + 1),
									";",
									list[num5],
									";",
									list2[num5],
									";",
									list3[num5],
									";",
									list4[num5],
									";"
								});
								this.csvwriter.WriteLine(text2);
							}
						}
						else
						{
							bool flag10 = tablename.Contains(DBDataLogs.FilamentCurrent);
							if (flag10)
							{
								for (int num6 = 0; num6 < this.FilamentCurrentColNames.Count; num6++)
								{
									bool flag11 = this.FilamentCurrentColNames[num6] != "";
									if (flag11)
									{
										text = text + this.FilamentCurrentColNames[num6] + ";";
									}
								}
								this.csvwriter.WriteLine(text);
								this.dBDataLogs.FilamentCurrentGetEntries(tablename, ref list, ref list2, ref list3, ref list4, ref list5, ref list6, ref list7);
								bool flag12 = num > 0 && num2 > 0;
								if (flag12)
								{
									for (int num7 = num - 1; num7 <= num2 - 1; num7++)
									{
										string text2 = string.Concat(new string[]
										{
											Convert.ToString(num7 + 1),
											";",
											list[num7],
											";",
											list2[num7],
											";",
											list3[num7],
											";",
											list4[num7],
											";",
											list5[num7],
											";",
											list6[num7],
											";",
											list7[num7],
											";"
										});
										this.csvwriter.WriteLine(text2);
									}
								}
							}
							else
							{
								bool flag13 = tablename.Contains(DBDataLogs.Arcs);
								if (flag13)
								{
									for (int num8 = 0; num8 < this.ArcsColNames.Count; num8++)
									{
										bool flag14 = this.ArcsColNames[num8] != "";
										if (flag14)
										{
											text = text + this.ArcsColNames[num8] + ";";
										}
									}
									this.csvwriter.WriteLine(text);
									this.dBDataLogs.ArcsGetEntriesType0(tablename, ref list, ref list2, ref list3);
									bool flag15 = num > 0 && num2 > 0;
									if (flag15)
									{
										for (int num9 = num - 1; num9 <= num2 - 1; num9++)
										{
											string text2 = string.Concat(new string[]
											{
												Convert.ToString(num9 + 1),
												";",
												list[num9],
												";",
												list2[num9],
												";"
											});
											this.csvwriter.WriteLine(text2);
										}
									}
								}
								else
								{
									bool flag16 = tablename.Contains(DBDataLogs.SwitchOnCycles);
									if (flag16)
									{
										for (int num10 = 0; num10 < this.SwitchOnCyclesColNames.Count; num10++)
										{
											bool flag17 = this.SwitchOnCyclesColNames[num10] != "";
											if (flag17)
											{
												text = text + this.SwitchOnCyclesColNames[num10] + ";";
											}
										}
										this.csvwriter.WriteLine(text);
										this.dBDataLogs.XRaySwitchOnGetEntries(tablename, ref list);
										bool flag18 = num > 0 && num2 > 0;
										if (flag18)
										{
											for (int num11 = num - 1; num11 <= num2 - 1; num11++)
											{
												string text2 = Convert.ToString(num11 + 1) + ";" + list[num11] + ";";
												this.csvwriter.WriteLine(text2);
											}
										}
									}
									else
									{
										bool flag19 = tablename.Contains(DBDataLogs.DetectorOnTimes);
										if (flag19)
										{
											for (int num12 = 0; num12 < this.DetectorOnTimesColNames.Count; num12++)
											{
												bool flag20 = this.DetectorOnTimesColNames[num12] != "";
												if (flag20)
												{
													text = text + this.DetectorOnTimesColNames[num12] + ";";
												}
											}
											this.csvwriter.WriteLine(text);
											this.dBDataLogs.DetectorOnTimesGetEntries(tablename, ref list, ref list2, ref list3, ref list4, false);
											for (int num13 = 0; num13 < num3; num13++)
											{
												string text2 = string.Concat(new string[]
												{
													Convert.ToString(num13 + 1),
													";",
													list[num13],
													";",
													list2[num13],
													";",
													list3[num13],
													";",
													list4[num13],
													";"
												});
												this.csvwriter.WriteLine(text2);
											}
										}
										else
										{
											bool flag21 = tablename.Contains(DBDataLogs.BackgroundIndex);
											if (flag21)
											{
												for (int num14 = 0; num14 < this.BackgroundIndexColNames.Count; num14++)
												{
													bool flag22 = this.BackgroundIndexColNames[num14] != "";
													if (flag22)
													{
														text = text + this.BackgroundIndexColNames[num14] + ";";
													}
												}
												this.csvwriter.WriteLine(text);
												this.dBDataLogs.BackgroundIndexGetEntries(tablename, ref list, ref list2, ref list3, ref list4, ref list5, ref list6, ref list7, ref list8, ref list9);
												bool flag23 = num > 0 && num2 > 0;
												if (flag23)
												{
													for (int num15 = num - 1; num15 <= num2 - 1; num15++)
													{
														string text2 = string.Concat(new string[]
														{
															Convert.ToString(num15 + 1),
															";",
															list[num15],
															";",
															list2[num15],
															";",
															list3[num15],
															";",
															list4[num15],
															";",
															list5[num15],
															";",
															list6[num15],
															";",
															list7[num15],
															";",
															list8[num15],
															";",
															list9[num15],
															";"
														});
														this.csvwriter.WriteLine(text2);
													}
												}
											}
											else
											{
												bool flag24 = tablename.Contains(DBDataLogs.ResetLogs);
												if (flag24)
												{
													for (int num16 = 0; num16 < this.ResetLogsColumnNames.Count; num16++)
													{
														bool flag25 = this.ResetLogsColumnNames[num16] != "";
														if (flag25)
														{
															text = text + this.ResetLogsColumnNames[num16] + ";";
														}
													}
													this.csvwriter.WriteLine(text);
													this.dBDataLogs.ResetLogsGetEntries(tablename, ref list, ref list2, ref list3, ref list4);
													bool flag26 = num > 0 && num2 > 0;
													if (flag26)
													{
														for (int num17 = num - 1; num17 <= num2 - 1; num17++)
														{
															string text2 = string.Concat(new string[]
															{
																Convert.ToString(num17 + 1),
																";",
																list[num17],
																";",
																list2[num17],
																";",
																list3[num17],
																";",
																list4[num17],
																";"
															});
															this.csvwriter.WriteLine(text2);
														}
													}
												}
												else
												{
													bool flag27 = tablename.Contains(DBDataLogs.Limits);
													if (flag27)
													{
														text = "ID; Name; Current Limit; Default Limit";
														this.csvwriter.WriteLine(text);
														int num18 = 1;
														MachineType machineType = MachineType.MachineTypeUndefined;
														this.dBConnection.GetMachineType(ref machineType);
														MachineMode machineMode = MachineMode.Standard;
														this.dBConnection.GetMachineMode(ref machineMode);
														int num19 = 0;
														string tableName = "";
														this.dBConnection.GetCurrentProductTableName(ref tableName);
														this.dBConnection.GetFlapsCount(tableName, ref num19);
														bool flag28 = this.parameterHandling.ReadPar(SVT.svtEnablePath) == "1";
														bool flag29 = machineMode != MachineMode.BulkObject && (machineMode != MachineMode.Bulk || num19 != 1) && !flag28;
														if (flag29)
														{
															num18 = 2;
														}
														bool flag30 = machineType != MachineType.RayconEntry && machineMode != MachineMode.BulkObject && (machineMode != MachineMode.Bulk || num19 != 1) && !flag28;
														if (flag30)
														{
															string a = this.parameterHandling.ReadPar(Definition.ioSubversionPath);
															bool flag31 = a != "1" && a != "3" && a != "4";
															if (flag31)
															{
																num18 = 4;
															}
														}
														List<string[]> defaultLimits = this.dBDataLogs.getDefaultLimits();
														int num20 = 0;
														foreach (string[] array in defaultLimits)
														{
															bool flag32 = array[0].Equals("Temperature");
															if (flag32)
															{
																text = Convert.ToString(num20 + 1) + ";Operating Parameters";
																num20++;
																this.csvwriter.WriteLine(text);
															}
															else
															{
																bool flag33 = array[0].Equals("TemperatureSource");
																if (flag33)
																{
																	text = Convert.ToString(num20 + 1) + ";Source";
																	num20++;
																	this.csvwriter.WriteLine(text);
																}
																else
																{
																	bool flag34 = array[0].Equals("TemperatureDetector");
																	if (flag34)
																	{
																		text = Convert.ToString(num20 + 1) + ";Detector ";
																		num20++;
																		this.csvwriter.WriteLine(text);
																	}
																	else
																	{
																		bool flag35 = array[0].Equals("OperatingHoursConveyor");
																		if (flag35)
																		{
																			text = Convert.ToString(num20 + 1) + ";Conveyor";
																			num20++;
																			this.csvwriter.WriteLine(text);
																		}
																		else
																		{
																			bool flag36 = array[0].Equals("Rejecter1");
																			if (flag36)
																			{
																				text = Convert.ToString(num20 + 1) + ";Rejecter";
																				num20++;
																				this.csvwriter.WriteLine(text);
																			}
																			else
																			{
																				bool flag37 = array[0].Equals("Rejecter1PerDay");
																				if (flag37)
																				{
																					text = Convert.ToString(num20 + 1) + ";Values per Day";
																					num20++;
																					this.csvwriter.WriteLine(text);
																				}
																			}
																		}
																	}
																}
															}
															bool flag38 = (!array[0].Contains("Rejecter2") || num18 >= 2) && ((!array[0].Contains("Rejecter3") && !array[0].Contains("Rejecter4")) || num18 >= 4);
															if (flag38)
															{
																string text3 = "";
																this.dBDataLogs.GetLimitsEntry(array[0], ref text3);
																string text2 = string.Concat(new string[]
																{
																	Convert.ToString(num20 + 1),
																	";",
																	array[0],
																	";",
																	text3,
																	";",
																	array[1]
																});
																this.csvwriter.WriteLine(text2);
																num20++;
															}
														}
													}
													else
													{
														bool flag39 = tablename.Contains(DBDataLogs.DetectorQuality);
														if (flag39)
														{
															for (int num21 = 0; num21 < this.DetectorQualityColNames.Count; num21++)
															{
																bool flag40 = this.DetectorQualityColNames[num21] != "";
																if (flag40)
																{
																	text = text + this.DetectorQualityColNames[num21] + ";";
																}
															}
															this.csvwriter.WriteLine(text);
															this.dBDataLogs.DetectorQualityGetEntries(tablename, ref list, ref list2, ref list3, ref list4, ref list5, ref list6, ref list7);
															bool flag41 = num > 0 && num2 > 0;
															if (flag41)
															{
																for (int num22 = num - 1; num22 <= num2 - 1; num22++)
																{
																	string text2 = string.Concat(new string[]
																	{
																		Convert.ToString(num22 + 1),
																		";",
																		list[num22],
																		";",
																		list2[num22],
																		";",
																		list3[num22],
																		";",
																		list4[num22],
																		";",
																		list5[num22],
																		";",
																		list6[num22],
																		";",
																		list7[num22],
																		";"
																	});
																	this.csvwriter.WriteLine(text2);
																}
															}
														}
														else
														{
															bool flag42 = tablename.Contains(DBDataLogs.OnOffTimes);
															if (flag42)
															{
																bool bulkFlaps = this.BulkFlaps;
																if (bulkFlaps)
																{
																	for (int num23 = 0; num23 < this.OnOffTimesColNamesBulkFlaps.Count; num23++)
																	{
																		bool flag43 = this.OnOffTimesColNamesBulkFlaps[num23] != "";
																		if (flag43)
																		{
																			text = text + this.OnOffTimesColNamesBulkFlaps[num23] + ";";
																		}
																	}
																	this.csvwriter.WriteLine(text);
																	this.dBDataLogs.OnOffTimesGetEntries(ref list, ref list2, ref list3, ref list4, ref list5, ref list6, ref list7, ref list8, ref list9, ref list10, ref list11);
																	for (int num24 = 0; num24 < list.Count<string>(); num24++)
																	{
																		string text2 = string.Concat(new string[]
																		{
																			Convert.ToString(num24 + 1),
																			";",
																			list[num24],
																			";",
																			list2[num24],
																			";",
																			list3[num24],
																			";",
																			list4[num24],
																			";",
																			list11[num24],
																			";",
																			list5[num24],
																			";",
																			list6[num24],
																			";",
																			list7[num24],
																			";",
																			list8[num24],
																			";"
																		});
																		this.csvwriter.WriteLine(text2);
																	}
																}
																else
																{
																	bool bulkSVT = this.BulkSVT;
																	if (bulkSVT)
																	{
																		for (int num25 = 0; num25 < this.OnOffTimesColNamesBulkSVT.Count; num25++)
																		{
																			bool flag44 = this.OnOffTimesColNamesBulkSVT[num25] != "";
																			if (flag44)
																			{
																				text = text + this.OnOffTimesColNamesBulkSVT[num25] + ";";
																			}
																		}
																		this.csvwriter.WriteLine(text);
																		List<List<string>> list12 = new List<List<string>>();
																		this.dBDataLogs.OnOffTimesGetEntriesWithSVT(ref list, ref list2, ref list3, ref list4, ref list5, ref list6, ref list7, ref list8, ref list9, ref list10, ref list11, ref list12);
																		for (int num26 = 0; num26 < list.Count<string>(); num26++)
																		{
																			string text2 = string.Concat(new string[]
																			{
																				Convert.ToString(num26 + 1),
																				";",
																				list[num26],
																				";",
																				list2[num26],
																				";",
																				list3[num26],
																				";",
																				list4[num26],
																				";",
																				list11[num26],
																				";"
																			});
																			for (int num27 = 0; num27 < list12[num26].Count; num27++)
																			{
																				text2 = text2 + list12[num26][num27] + ";";
																			}
																			this.csvwriter.WriteLine(text2);
																		}
																	}
																	else
																	{
																		for (int num28 = 0; num28 < this.OnOffTimesColNames.Count; num28++)
																		{
																			bool flag45 = this.OnOffTimesColNames[num28] != "";
																			if (flag45)
																			{
																				text = text + this.OnOffTimesColNames[num28] + ";";
																			}
																		}
																		this.csvwriter.WriteLine(text);
																		this.dBDataLogs.OnOffTimesGetEntries(ref list, ref list2, ref list3, ref list4, ref list5, ref list6, ref list7, ref list8, ref list9, ref list10, ref list11);
																		for (int num29 = 0; num29 < list.Count<string>(); num29++)
																		{
																			string text2 = string.Concat(new string[]
																			{
																				Convert.ToString(num29 + 1),
																				";",
																				list[num29],
																				";",
																				list2[num29],
																				";",
																				list3[num29],
																				";",
																				list4[num29],
																				";",
																				list5[num29],
																				";",
																				list6[num29],
																				";",
																				list7[num29],
																				";",
																				list8[num29],
																				";",
																				list9[num29],
																				";",
																				list10[num29],
																				";"
																			});
																			this.csvwriter.WriteLine(text2);
																		}
																	}
																}
															}
															else
															{
																bool flag46 = tablename.Contains(DBDataLogs.BackUps);
																if (flag46)
																{
																	for (int num30 = 0; num30 < this.BackUpsColNames.Count; num30++)
																	{
																		bool flag47 = this.BackUpsColNames[num30] != "";
																		if (flag47)
																		{
																			text = text + this.BackUpsColNames[num30] + ";";
																		}
																	}
																	this.csvwriter.WriteLine(text);
																	this.dBDataLogs.BackUpsGetEntries(ref list, ref list2, ref list3, ref list4, ref list5, ref list6, ref list7, ref list8);
																	for (int num31 = 0; num31 < list.Count<string>(); num31++)
																	{
																		string text2 = string.Concat(new string[]
																		{
																			Convert.ToString(num31 + 1),
																			";",
																			list[num31],
																			";",
																			list2[num31],
																			";",
																			list3[num31],
																			";",
																			list4[num31],
																			";",
																			list5[num31],
																			";",
																			list6[num31],
																			";",
																			list7[num31],
																			";",
																			list8[num31],
																			";"
																		});
																		this.csvwriter.WriteLine(text2);
																	}
																}
																else
																{
																	bool flag48 = tablename.Contains(DBDataLogs.DetectorFluctuation);
																	if (flag48)
																	{
																		for (int num32 = 0; num32 < this.DetectorFluctuationColNames.Count; num32++)
																		{
																			bool flag49 = this.DetectorFluctuationColNames[num32] != "";
																			if (flag49)
																			{
																				text = text + this.DetectorFluctuationColNames[num32] + ";";
																			}
																		}
																		this.csvwriter.WriteLine(text);
																		this.dBDataLogs.DetectorFluctuationGetEntries(tablename, ref list, ref list2, ref list3, ref list4, ref list5, ref list6, ref list7, ref list8);
																		bool flag50 = num > 0 && num2 > 0;
																		if (flag50)
																		{
																			for (int num33 = num - 1; num33 <= num2 - 1; num33++)
																			{
																				string text2 = string.Concat(new string[]
																				{
																					Convert.ToString(num33 + 1),
																					";",
																					list[num33],
																					";",
																					list2[num33],
																					";",
																					list3[num33],
																					";",
																					list4[num33],
																					";",
																					list5[num33],
																					";",
																					list6[num33],
																					";",
																					list7[num33],
																					";",
																					list8[num33],
																					";"
																				});
																				this.csvwriter.WriteLine(text2);
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
			}
		}

		// Token: 0x06002F87 RID: 12167 RVA: 0x0013A74C File Offset: 0x0013894C
		public void CreatePng(Grid chartGrid, Rect bounds)
		{
			string text = "";
			string text2 = "";
			string text3 = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\DataLogs\\";
			string text4 = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_ff");
			bool flag = !Directory.Exists(text3);
			if (flag)
			{
				Directory.CreateDirectory(text3);
			}
			this.dBConnection.GetUsbDriveLetter(ref text);
			try
			{
				List<string> list = Directory.GetFiles(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\DataLogs").ToList<string>();
				list.Sort(new NamesComparer());
				int count = list.Count;
				string str = this.setExportDataName(this.SelectedDataLogsTable) + ".png";
				text2 = text + "\\DataLogs";
				bool flag2 = Directory.Exists(text);
				if (flag2)
				{
					bool flag3 = !Directory.Exists(text2);
					if (flag3)
					{
						Directory.CreateDirectory(text2);
					}
				}
				text3 = text3 + "\\" + str;
				text2 = text + "\\DataLogs\\" + str;
				FileInfo fileInfo = new FileInfo(text2);
				RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap((int)bounds.Width, (int)bounds.Height, 96.0, 96.0, PixelFormats.Pbgra32);
				DrawingVisual drawingVisual = new DrawingVisual();
				using (DrawingContext drawingContext = drawingVisual.RenderOpen())
				{
					VisualBrush brush = new VisualBrush(chartGrid);
					Pen pen = new Pen();
					drawingContext.DrawRectangle(brush, pen, new Rect(default(Point), bounds.Size));
				}
				renderTargetBitmap.Render(drawingVisual);
				FileStream fileStream = new FileStream(text3, FileMode.Create);
				new PngBitmapEncoder
				{
					Frames = 
					{
						BitmapFrame.Create(renderTargetBitmap)
					}
				}.Save(fileStream);
				fileStream.Close();
				bool flag4 = Directory.Exists(text + "\\");
				if (flag4)
				{
					fileStream = new FileStream(text2, FileMode.Create);
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
					Path.GetDirectoryName(text2),
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
					Path.GetDirectoryName(text2),
					"\" ",
					LocalizationManager.Localize("msgForbidden")
				}), DialogStyle.Error);
			}
		}

		// Token: 0x06002F88 RID: 12168 RVA: 0x0013AA90 File Offset: 0x00138C90
		private string setExportDataName(string tablename)
		{
			string str = "DataLogs_";
			string str2 = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_ff");
			string text = "";
			MachineType machineType = MachineType.MachineTypeUndefined;
			this.dBConnection.GetMachineType(ref machineType);
			this.dBConnection.GetSerialNumber(ref text);
			bool flag = machineType == MachineType.RayconXRS;
			if (flag)
			{
				str += "RayconXRS_";
			}
			else
			{
				bool flag2 = machineType == MachineType.RayconD;
				if (flag2)
				{
					str += "RayconD_";
				}
				else
				{
					bool flag3 = machineType == MachineType.RayconEntry;
					if (flag3)
					{
						str += "RayconEX1_";
					}
					else
					{
						bool flag4 = machineType == MachineType.RayconDPlus;
						if (flag4)
						{
							str += "RayconDPlus_";
						}
						else
						{
							bool flag5 = machineType == MachineType.RayconEX2A;
							if (flag5)
							{
								str += "RayconEX2A_";
							}
							else
							{
								str += "UnknownType_";
							}
						}
					}
				}
			}
			bool flag6 = text != "";
			if (flag6)
			{
				str = str + text + "_";
			}
			else
			{
				str += "UnkownSerialnr_";
			}
			return str + tablename + "_" + str2;
		}

		// Token: 0x06002F89 RID: 12169 RVA: 0x0013ABA4 File Offset: 0x00138DA4
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x06002F8A RID: 12170 RVA: 0x0013ABD4 File Offset: 0x00138DD4
		private void CleanUp(object obj)
		{
			bool flag = this.DataLogsList != null;
			if (flag)
			{
				this.DataLogsList.Clear();
				this.DataLogsList = null;
			}
		}

		// Token: 0x04001DA1 RID: 7585
		private const string TypeRaycon_EX1 = "Raycon EX1";

		// Token: 0x04001DA2 RID: 7586
		private const string TypeRaycon_DPlus_MX = "Raycon D+ MX";

		// Token: 0x04001DA3 RID: 7587
		private const string TypeRaycon_DPlus_HX = "Raycon D+ HX";

		// Token: 0x04001DA4 RID: 7588
		private const string SubTypeStandard = "Standard";

		// Token: 0x04001DA5 RID: 7589
		private const string SubTypeLW = "LW";

		// Token: 0x04001DA6 RID: 7590
		private const string SubTypeBulk = "Bulk";

		// Token: 0x04001DA7 RID: 7591
		private const string SubTypeLWHy = "LW Hy";

		// Token: 0x04001DA8 RID: 7592
		private const string SubTypeBulkHy = "Bulk Hy";

		// Token: 0x04001DA9 RID: 7593
		private const string SensorsStandard = "Standard";

		// Token: 0x04001DAA RID: 7594
		private const string SensorsHighlevel = "HighLevel";

		// Token: 0x04001DAB RID: 7595
		private const string SensorsAdvanced = "Advanced";

		// Token: 0x04001DAC RID: 7596
		private DBDataLogs dBDataLogs;

		// Token: 0x04001DAD RID: 7597
		private DBConnection dBConnection;

		// Token: 0x04001DAF RID: 7599
		public Button btnExportCsvSelection;

		// Token: 0x04001DB0 RID: 7600
		public Button btnExportCsvAll;

		// Token: 0x04001DB1 RID: 7601
		public Button btnExportPng;

		// Token: 0x04001DB2 RID: 7602
		public Button btnResets;

		// Token: 0x04001DB5 RID: 7605
		private StreamWriter csvwriter;

		// Token: 0x04001DB9 RID: 7609
		private Keyboard numKeyboard;

		// Token: 0x04001DBA RID: 7610
		private string dateStartDBDataLogs;

		// Token: 0x04001DBB RID: 7611
		private string dateToday;

		// Token: 0x04001DBC RID: 7612
		private List<string> ArcsColNames = new List<string>
		{
			"ID  ",
			"",
			"DateTime  ",
			"Type  ",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			""
		};

		// Token: 0x04001DBD RID: 7613
		private List<string> BackgroundIndexColNames = new List<string>
		{
			"ID  ",
			"",
			"DateTime  ",
			"VoltageKV  ",
			"CurrentMA  ",
			"Scanrate  ",
			"BgIndex  ",
			"PixDeltaMax  ",
			"DeltaMax  ",
			"ErrorZeros  ",
			"ErrorLength  ",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			""
		};

		// Token: 0x04001DBE RID: 7614
		private List<string> FilamentCurrentColNames = new List<string>
		{
			"ID  ",
			"",
			"DateTime  ",
			"VoltageKV  ",
			"CurrentMA  ",
			"Filament  ",
			"FilamentMIN  ",
			"FilamentMAX  ",
			"FilamentAVG  ",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			""
		};

		// Token: 0x04001DBF RID: 7615
		private List<string> MachineInfosColumnNames = new List<string>
		{
			"ID  ",
			"Path  ",
			"",
			"Value ",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			""
		};

		// Token: 0x04001DC0 RID: 7616
		private List<string> OperatingCounterColNames = new List<string>
		{
			"ID  ",
			"Path  ",
			"",
			"Value ",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			""
		};

		// Token: 0x04001DC1 RID: 7617
		private List<string> ResetLogsColumnNames = new List<string>
		{
			"ID  ",
			"",
			"DateTime  ",
			"Path ",
			"Value  ",
			"Serialnr  ",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			""
		};

		// Token: 0x04001DC2 RID: 7618
		private List<string> SwitchOnCyclesColNames = new List<string>
		{
			"ID  ",
			"",
			"SwitchOnCycles  ",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			""
		};

		// Token: 0x04001DC3 RID: 7619
		private List<string> TemperaturesColNames = new List<string>
		{
			"ID  ",
			"",
			"DateTime  ",
			"Machine  ",
			"SourceTank  ",
			"Detector  ",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			""
		};

		// Token: 0x04001DC4 RID: 7620
		private List<string> XRayOnTimesColNames = new List<string>
		{
			"ID  ",
			"",
			"DateTime  ",
			"VoltageKV  ",
			"CurrentMA  ",
			"XRayOnTimeH  ",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			""
		};

		// Token: 0x04001DC5 RID: 7621
		private List<string> DetectorOnTimesColNames = new List<string>
		{
			"ID  ",
			"",
			"DateTime  ",
			"VoltageKV  ",
			"CurrentMA  ",
			"XRayOnTimeH  ",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			""
		};

		// Token: 0x04001DC6 RID: 7622
		private List<string> LimitsColNames = new List<string>
		{
			"ID  ",
			"",
			"Name   ",
			"",
			"",
			"Current limit  ",
			"Default limit  ",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			""
		};

		// Token: 0x04001DC7 RID: 7623
		private List<string> DetectorQualityColNames = new List<string>
		{
			"ID  ",
			"",
			"DateTime   ",
			"VoltageKV",
			"CurrentMA",
			"Scanrate  ",
			"PercentOfMeanValue  ",
			"MaximumModuleName",
			"MaxMeanModule",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			""
		};

		// Token: 0x04001DC8 RID: 7624
		private List<string> OnOffTimesColNames = new List<string>
		{
			"ID  ",
			"",
			"DateTime   ",
			"DeviceOnTimes  ",
			"ConveyorOnTimes  ",
			"XRayOnTimes  ",
			"Rejecter1  ",
			"Rejecter2  ",
			"Rejecter3  ",
			"Rejecter4  ",
			"Throughput  ",
			"Reject  ",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			""
		};

		// Token: 0x04001DC9 RID: 7625
		private List<string> BackUpsColNames = new List<string>
		{
			"ID  ",
			"",
			"DateTime   ",
			"BackupName  ",
			"UserID  ",
			"UserName  ",
			"UserLevel  ",
			"ProductsRestored  ",
			"DatabaseRestored ",
			"SystemRestored ",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			""
		};

		// Token: 0x04001DCA RID: 7626
		private List<string> DetectorFluctuationColNames = new List<string>
		{
			"ID  ",
			"",
			"DateTime   ",
			"VoltageKV",
			"CurrentMA",
			"Scanrate  ",
			"MaximumDetector",
			"MaxValue  ",
			"MaximumModuleName",
			"MaxValueModule",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			""
		};

		// Token: 0x04001DCB RID: 7627
		private List<string> OnOffTimesColNamesBulkSVT = new List<string>
		{
			"ID  ",
			"",
			"DateTime   ",
			"DeviceOnTimes  ",
			"ConveyorOnTimes  ",
			"XRayOnTimes  ",
			"SortingSwitchedOnTimes",
			""
		};

		// Token: 0x04001DCC RID: 7628
		private List<string> OnOffTimesColNamesBulkFlaps = new List<string>
		{
			"ID  ",
			"",
			"DateTime   ",
			"DeviceOnTimes  ",
			"ConveyorOnTimes  ",
			"XRayOnTimes  ",
			"SortingSwitchedOnTimes",
			"Rejecter1  ",
			"Rejecter2  ",
			"Rejecter3  ",
			"Rejecter4  ",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			""
		};

		// Token: 0x04001DCD RID: 7629
		private DataLogsListColumns currentColumnValues;

		// Token: 0x04001DCE RID: 7630
		private ObservableCollection<DataLogsListColumns> dataLogsList;

		// Token: 0x04001DD0 RID: 7632
		private string selectedDataLogsTable;

		// Token: 0x04001DD1 RID: 7633
		private List<string> header;

		// Token: 0x04001DD2 RID: 7634
		private string startTimeSelection;

		// Token: 0x04001DD3 RID: 7635
		private string endTimeSelection;

		// Token: 0x04001DD4 RID: 7636
		public int rowCount;

		// Token: 0x04001DD5 RID: 7637
		public int columnCount;

		// Token: 0x04001DD6 RID: 7638
		public bool timeSelectionIsPossible;

		// Token: 0x04001DD9 RID: 7641
		private ParameterHandling parameterHandling;

		// Token: 0x04001DDA RID: 7642
		private bool notWidescreen;

		// Token: 0x04001DDB RID: 7643
		private bool widescreen;

		// Token: 0x04001DDC RID: 7644
		private bool BulkSVT = false;

		// Token: 0x04001DDD RID: 7645
		private bool BulkFlaps = false;
	}
}
