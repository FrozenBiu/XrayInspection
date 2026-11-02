using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using Common;
using DatabaseConnection;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x02000089 RID: 137
	internal class RejectParametersViewModel : MonitoringViewModel, INotifyPropertyChanged
	{
		// Token: 0x14000047 RID: 71
		// (add) Token: 0x06000D50 RID: 3408 RVA: 0x00051BE8 File Offset: 0x0004FDE8
		// (remove) Token: 0x06000D51 RID: 3409 RVA: 0x00051C20 File Offset: 0x0004FE20
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x170003FE RID: 1022
		// (get) Token: 0x06000D52 RID: 3410 RVA: 0x00051C55 File Offset: 0x0004FE55
		// (set) Token: 0x06000D53 RID: 3411 RVA: 0x00051C5D File Offset: 0x0004FE5D
		public RelayCommand CleanUpCommand { get; set; }

		// Token: 0x170003FF RID: 1023
		// (get) Token: 0x06000D54 RID: 3412 RVA: 0x00051C68 File Offset: 0x0004FE68
		// (set) Token: 0x06000D55 RID: 3413 RVA: 0x00051C80 File Offset: 0x0004FE80
		public List<MonitoringItem> Items
		{
			get
			{
				return this.items;
			}
			set
			{
				this.items = value;
				this.OnPropertyChanged("Items");
			}
		}

		// Token: 0x17000400 RID: 1024
		// (get) Token: 0x06000D56 RID: 3414 RVA: 0x00051C96 File Offset: 0x0004FE96
		// (set) Token: 0x06000D57 RID: 3415 RVA: 0x00051C9E File Offset: 0x0004FE9E
		public BoundLocalizedText StrTitle { get; set; }

		// Token: 0x17000401 RID: 1025
		// (get) Token: 0x06000D58 RID: 3416 RVA: 0x00051CA7 File Offset: 0x0004FEA7
		// (set) Token: 0x06000D59 RID: 3417 RVA: 0x00051CAF File Offset: 0x0004FEAF
		public BoundLocalizedText StrCurrentVal { get; set; }

		// Token: 0x17000402 RID: 1026
		// (get) Token: 0x06000D5A RID: 3418 RVA: 0x00051CB8 File Offset: 0x0004FEB8
		// (set) Token: 0x06000D5B RID: 3419 RVA: 0x00051CC0 File Offset: 0x0004FEC0
		public BoundLocalizedText StrLimits { get; set; }

		// Token: 0x17000403 RID: 1027
		// (get) Token: 0x06000D5C RID: 3420 RVA: 0x00051CC9 File Offset: 0x0004FEC9
		// (set) Token: 0x06000D5D RID: 3421 RVA: 0x00051CD1 File Offset: 0x0004FED1
		public BoundLocalizedText StrPerDay { get; set; }

		// Token: 0x17000404 RID: 1028
		// (get) Token: 0x06000D5E RID: 3422 RVA: 0x00051CDA File Offset: 0x0004FEDA
		// (set) Token: 0x06000D5F RID: 3423 RVA: 0x00051CE2 File Offset: 0x0004FEE2
		public BoundLocalizedText StrValve { get; set; }

		// Token: 0x17000405 RID: 1029
		// (get) Token: 0x06000D60 RID: 3424 RVA: 0x00051CEB File Offset: 0x0004FEEB
		// (set) Token: 0x06000D61 RID: 3425 RVA: 0x00051CF3 File Offset: 0x0004FEF3
		public BoundLocalizedText StrValveSum { get; set; }

		// Token: 0x17000406 RID: 1030
		// (get) Token: 0x06000D62 RID: 3426 RVA: 0x00051CFC File Offset: 0x0004FEFC
		// (set) Token: 0x06000D63 RID: 3427 RVA: 0x00051D14 File Offset: 0x0004FF14
		public bool EnableLimits
		{
			get
			{
				return this.enableLimits;
			}
			set
			{
				this.enableLimits = value;
				this.OnPropertyChanged("EnableLimits");
			}
		}

		// Token: 0x06000D64 RID: 3428 RVA: 0x00051D2C File Offset: 0x0004FF2C
		public RejectParametersViewModel(DBDataLogs dBDataLogs, DBConnection dBConnection, ParameterHandling parameterHandling, bool EnableLimits, ErrorSurvey errorSurvey)
		{
			this.RegisterLocalizedText();
			this.dBDataLogs = dBDataLogs;
			this.dbConnection = dBConnection;
			this.parameterHandling = parameterHandling;
			this.EnableLimits = EnableLimits;
			this.errorSurvey = errorSurvey;
			this.Initialize();
			this.CleanUpCommand = new RelayCommand(new Action<object>(this.CleanUp));
		}

		// Token: 0x06000D65 RID: 3429 RVA: 0x00051DFC File Offset: 0x0004FFFC
		private void UpdateValues(object sender, EventArgs e)
		{
			uint num = 0U;
			uint.TryParse(this.parameterHandling.ReadPar("/hardware/rejecter/rejecter1/TotalCounts/Value"), out num);
			this.items[0].CurrentValue = (this.start1 + num - this.startCount1).ToString();
			this.items[1].CurrentValue = (this.start1PerDay + num - this.startCount1).ToString();
			bool flag = this.items.Count > 2;
			if (flag)
			{
				uint.TryParse(this.parameterHandling.ReadPar("/hardware/rejecter/rejecter2/TotalCounts/Value"), out num);
				this.items[2].CurrentValue = (this.start2 + num - this.startCount2).ToString();
				this.items[3].CurrentValue = (this.start2PerDay + num - this.startCount2).ToString();
				bool flag2 = this.items.Count > 4;
				if (flag2)
				{
					uint.TryParse(this.parameterHandling.ReadPar("/hardware/rejecter/rejecter3/TotalCounts/Value"), out num);
					this.items[4].CurrentValue = (this.start3 + num - this.startCount3).ToString();
					this.items[5].CurrentValue = (this.start3PerDay + num - this.startCount3).ToString();
					uint.TryParse(this.parameterHandling.ReadPar("/hardware/rejecter/rejecter4/TotalCounts/Value"), out num);
					this.items[6].CurrentValue = (this.start4 + num - this.startCount4).ToString();
					this.items[7].CurrentValue = (this.start4PerDay + num - this.startCount4).ToString();
				}
			}
		}

		// Token: 0x06000D66 RID: 3430 RVA: 0x00051FE4 File Offset: 0x000501E4
		private void Initialize()
		{
			bool flag = this.parameterHandling.ReadPar(SVT.svtEnablePath).Equals("1");
			if (flag)
			{
				string limit = "";
				string limit2 = "";
				string text = "";
				int num = 0;
				uint num2 = 0U;
				int num3 = 0;
				int num4 = 0;
				this.dbConnection.GetNumValves(ref num);
				uint num5 = 0U;
				this.dBDataLogs.GetLimitsEntry("Rejecter1", ref limit);
				this.dBDataLogs.OperatingCounterGetIntEntry("CounterRejecter1", ref num2);
				num5 += num2;
				this.items.Add(new MonitoringItem(this.StrValve.LocalizedText + " 1 ", num2.ToString(), limit, "", true, "txtValve", ""));
				this.dBDataLogs.GetLimitsEntry("Rejecter1PerDay", ref limit2);
				this.dBDataLogs.OnOffTimesGetEntryOfDate(DateTime.Now.ToString("d", new CultureInfo("de-DE")), ref text, "SVTValve1");
				bool flag2 = text.Equals("");
				if (flag2)
				{
					text = "0";
				}
				this.items.Add(new MonitoringItem(this.StrValve.LocalizedText + " 1 " + this.StrPerDay.LocalizedText, text, limit2, "", true, "txtValve", "txtPerDay"));
				bool flag3 = int.TryParse(text, out num3);
				if (flag3)
				{
					num4 += num3;
				}
				for (int i = 2; i <= num; i++)
				{
					this.dBDataLogs.OperatingCounterGetIntEntry("CounterRejecter" + i.ToString(), ref num2);
					this.items.Add(new MonitoringItem(this.StrValve.LocalizedText + " " + i.ToString(), num2.ToString(), limit, "", false, "txtValve", ""));
					num5 += num2;
					num2 = 0U;
					this.dBDataLogs.OnOffTimesGetEntryOfDate(DateTime.Now.ToString("d", new CultureInfo("de-DE")), ref text, "SVTValve" + i.ToString());
					bool flag4 = text.Equals("");
					if (flag4)
					{
						text = "0";
					}
					this.items.Add(new MonitoringItem(string.Concat(new string[]
					{
						this.StrValve.LocalizedText,
						" ",
						i.ToString(),
						" ",
						this.StrPerDay.LocalizedText
					}), text, limit2, "", false, "txtValve", "txtPerDay"));
					bool flag5 = int.TryParse(text, out num3);
					if (flag5)
					{
						num4 += num3;
					}
					num3 = 0;
				}
				this.dBDataLogs.GetLimitsEntry("Rejecter2PerDay", ref limit);
				this.items.Insert(0, new MonitoringItem(this.StrValveSum.LocalizedText + " " + this.StrPerDay.LocalizedText, num4.ToString(), limit, "", true, "txtSumValves", "txtPerDay"));
				this.dBDataLogs.GetLimitsEntry("Rejecter2", ref limit);
				this.items.Insert(0, new MonitoringItem(this.StrValveSum.LocalizedText, num5.ToString(), limit, "", true, "txtSumValves", ""));
			}
			else
			{
				MachineType machineType = MachineType.MachineTypeUndefined;
				this.dbConnection.GetMachineType(ref machineType);
				MachineMode machineMode = MachineMode.Standard;
				this.dbConnection.GetMachineMode(ref machineMode);
				int num6 = 0;
				string tableName = "";
				this.dbConnection.GetCurrentProductTableName(ref tableName);
				this.dbConnection.GetFlapsCount(tableName, ref num6);
				uint num7 = 0U;
				string text2 = "";
				uint num8 = 0U;
				this.dBDataLogs.GetDataLogsSettingsEntry("MonitoringMessagesOnOff", ref text2);
				bool flag6 = text2.Equals("1");
				if (flag6)
				{
					this.showMessages = true;
				}
				text2 = "";
				this.dBDataLogs.GetLimitsEntry("Rejecter1", ref text2);
				this.dBDataLogs.OperatingCounterGetIntEntry("CounterRejecter1", ref num8);
				this.items.Add(new MonitoringItem(this.StrTitle.LocalizedText + " 1 ", num8.ToString(), text2, "", this.EnableLimits, "txtRejecter", ""));
				this.start1 = num8;
				this.dBDataLogs.OnOffTimesGetEntryOfDate(DateTime.Now.ToString("d", new CultureInfo("de-DE")), ref text2, "CounterRejecter1");
				uint.TryParse(text2, out num7);
				this.start1PerDay = num7;
				text2 = "";
				this.dBDataLogs.GetLimitsEntry("Rejecter1PerDay", ref text2);
				this.items.Add(new MonitoringItem(this.StrTitle.LocalizedText + " 1 " + this.StrPerDay.LocalizedText, this.start1PerDay.ToString(), text2, "", this.EnableLimits, "txtRejecter", "txtPerDay"));
				this.dBDataLogs.GetLimitsEntry("Rejecter2", ref text2);
				bool flag7 = machineMode != MachineMode.BulkObject && (machineMode != MachineMode.Bulk || num6 != 1);
				if (flag7)
				{
					this.dBDataLogs.OperatingCounterGetIntEntry("CounterRejecter2", ref num8);
					this.items.Add(new MonitoringItem(this.StrTitle.LocalizedText + " 2 ", num8.ToString(), text2, "", this.EnableLimits, "txtRejecter", ""));
					this.start2 = num8;
					this.dBDataLogs.OnOffTimesGetEntryOfDate(DateTime.Now.ToString("d", new CultureInfo("de-DE")), ref text2, "CounterRejecter2");
					uint.TryParse(text2, out num7);
					this.start2PerDay = num7;
					text2 = "";
					this.dBDataLogs.GetLimitsEntry("Rejecter2PerDay", ref text2);
					this.items.Add(new MonitoringItem(this.StrTitle.LocalizedText + " 2 " + this.StrPerDay.LocalizedText, this.start2PerDay.ToString(), text2, "", this.EnableLimits, "txtRejecter", "txtPerDay"));
				}
				bool flag8 = machineType != MachineType.RayconEntry && machineMode != MachineMode.BulkObject && (machineMode != MachineMode.Bulk || num6 != 1);
				if (flag8)
				{
					string a = this.parameterHandling.ReadPar(Definition.ioSubversionPath);
					bool flag9 = a != "1" && a != "3" && a != "4";
					if (flag9)
					{
						this.dBDataLogs.GetLimitsEntry("Rejecter3", ref text2);
						this.dBDataLogs.OperatingCounterGetIntEntry("CounterRejecter3", ref num8);
						this.items.Add(new MonitoringItem(this.StrTitle.LocalizedText + " 3 ", num8.ToString(), text2, "", this.EnableLimits, "txtRejecter", ""));
						this.start3 = num8;
						this.dBDataLogs.OnOffTimesGetEntryOfDate(DateTime.Now.ToString("d", new CultureInfo("de-DE")), ref text2, "CounterRejecter3");
						uint.TryParse(text2, out num7);
						this.start3PerDay = num7;
						text2 = "";
						this.dBDataLogs.GetLimitsEntry("Rejecter3PerDay", ref text2);
						this.items.Add(new MonitoringItem(this.StrTitle.LocalizedText + " 3 " + this.StrPerDay.LocalizedText, this.start3PerDay.ToString(), text2, "", this.EnableLimits, "txtRejecter", "txtPerDay"));
						this.dBDataLogs.GetLimitsEntry("Rejecter4", ref text2);
						this.dBDataLogs.OperatingCounterGetIntEntry("CounterRejecter4", ref num8);
						this.items.Add(new MonitoringItem(this.StrTitle.LocalizedText + " 4 ", num8.ToString(), text2, "", this.EnableLimits, "txtRejecter", ""));
						this.start4 = num8;
						this.dBDataLogs.OnOffTimesGetEntryOfDate(DateTime.Now.ToString("d", new CultureInfo("de-DE")), ref text2, "CounterRejecter4");
						uint.TryParse(text2, out num7);
						this.start4PerDay = num7;
						text2 = "";
						this.dBDataLogs.GetLimitsEntry("Rejecter4PerDay", ref text2);
						this.items.Add(new MonitoringItem(this.StrTitle.LocalizedText + " 4 " + this.StrPerDay.LocalizedText, this.start4PerDay.ToString(), text2, "", this.EnableLimits, "txtRejecter", "txtPerDay"));
					}
				}
				uint.TryParse(this.parameterHandling.ReadPar("/hardware/rejecter/rejecter1/TotalCounts/Value"), out this.startCount1);
				bool flag10 = this.items.Count > 2;
				if (flag10)
				{
					uint.TryParse(this.parameterHandling.ReadPar("/hardware/rejecter/rejecter2/TotalCounts/Value"), out this.startCount2);
					bool flag11 = this.items.Count > 4;
					if (flag11)
					{
						uint.TryParse(this.parameterHandling.ReadPar("/hardware/rejecter/rejecter3/TotalCounts/Value"), out this.startCount3);
						uint.TryParse(this.parameterHandling.ReadPar("/hardware/rejecter/rejecter4/TotalCounts/Value"), out this.startCount4);
					}
				}
				this.timerDataLogsSeconds = new DispatcherTimer(new TimeSpan(0, 0, 2), DispatcherPriority.Normal, new EventHandler(this.UpdateValues), Application.Current.Dispatcher);
				this.timerDataLogsSeconds.Start();
			}
		}

		// Token: 0x06000D67 RID: 3431 RVA: 0x000529E4 File Offset: 0x00050BE4
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x06000D68 RID: 3432 RVA: 0x00052A14 File Offset: 0x00050C14
		private void RegisterLocalizedText()
		{
			this.StrLimits = new BoundLocalizedText("txtLimits");
			this.StrCurrentVal = new BoundLocalizedText("txtCurrentValue");
			this.StrTitle = new BoundLocalizedText("txtRejecter");
			this.StrPerDay = new BoundLocalizedText("txtPerDay");
			this.StrValve = new BoundLocalizedText("txtValve");
			this.StrValveSum = new BoundLocalizedText("txtSumValves");
		}

		// Token: 0x06000D69 RID: 3433 RVA: 0x00052A88 File Offset: 0x00050C88
		private void DeregisterLocalizedText()
		{
			MainWindow.localizationManager.Deregister(this.StrLimits);
			MainWindow.localizationManager.Deregister(this.StrCurrentVal);
			MainWindow.localizationManager.Deregister(this.StrTitle);
			MainWindow.localizationManager.Deregister(this.StrPerDay);
			MainWindow.localizationManager.Deregister(this.StrValve);
			MainWindow.localizationManager.Deregister(this.StrValveSum);
		}

		// Token: 0x06000D6A RID: 3434 RVA: 0x00052AFC File Offset: 0x00050CFC
		public override void getNumKeyboardNumber(MonitoringItem mItem, int limitNumb)
		{
			this.limitNumber = limitNumb;
			this.selectedItem = mItem;
			bool flag = this.limitNumber == 1;
			string oldValue;
			if (flag)
			{
				oldValue = mItem.Limit;
			}
			else
			{
				oldValue = mItem.Limit2;
			}
			this.numKeyboard = new NumKeyboard(mItem.Name, oldValue, NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.keyboard_TextInputKeyboardNumber;
			this.numKeyboard.Show();
		}

		// Token: 0x06000D6B RID: 3435 RVA: 0x00052B78 File Offset: 0x00050D78
		private void keyboard_TextInputKeyboardNumber(object sender, KeyboardInputEventArgs e)
		{
			bool flag = e.Text != null;
			if (flag)
			{
				bool flag2 = this.limitNumber == 1;
				if (flag2)
				{
					bool flag3 = e.Text != "" && e.Text != "0";
					if (flag3)
					{
						this.selectedItem.Limit = e.Text;
					}
					else
					{
						this.selectedItem.Limit = "-";
					}
					this.SetNewLimit(this.selectedItem);
				}
				else
				{
					this.selectedItem.Limit2 = e.Text;
				}
			}
			this.numKeyboard.TextInputKeyboard -= this.keyboard_TextInputKeyboardNumber;
		}

		// Token: 0x06000D6C RID: 3436 RVA: 0x00052C30 File Offset: 0x00050E30
		public void SetNewLimit(MonitoringItem item)
		{
			byte num = 0;
			string text = "";
			this.dBDataLogs.GetDataLogsSettingsEntry("MonitoringMessagesOnOff", ref text);
			bool flag = item.Name.Equals(this.StrTitle.LocalizedText + " 1 ");
			if (flag)
			{
				this.dBDataLogs.LimitsAddEntry("DateTimeRejecter1", "Rejecter1", item.Limit, DateTime.Now.ToString(new CultureInfo("de-DE")));
				num = 1;
			}
			else
			{
				bool flag2 = item.Name.Equals(this.StrTitle.LocalizedText + " 2 ");
				if (flag2)
				{
					this.dBDataLogs.LimitsAddEntry("DateTimeRejecter2", "Rejecter2", item.Limit, DateTime.Now.ToString(new CultureInfo("de-DE")));
					num = 2;
				}
				else
				{
					bool flag3 = item.Name.Equals(this.StrTitle.LocalizedText + " 3 ");
					if (flag3)
					{
						this.dBDataLogs.LimitsAddEntry("DateTimeRejecter3", "Rejecter3", item.Limit, DateTime.Now.ToString(new CultureInfo("de-DE")));
						num = 3;
					}
					else
					{
						bool flag4 = item.Name.Equals(this.StrTitle.LocalizedText + " 4 ");
						if (flag4)
						{
							this.dBDataLogs.LimitsAddEntry("DateTimeRejecter4", "Rejecter4", item.Limit, DateTime.Now.ToString(new CultureInfo("de-DE")));
							num = 4;
						}
						else
						{
							bool flag5 = item.Name.Equals(this.StrTitle.LocalizedText + " 1 " + this.StrPerDay.LocalizedText);
							if (flag5)
							{
								this.dBDataLogs.LimitsAddEntry("DateTimeRejecter1PerDay", "Rejecter1PerDay", item.Limit, DateTime.Now.ToString(new CultureInfo("de-DE")));
								num = 1;
							}
							else
							{
								bool flag6 = item.Name.Equals(this.StrTitle.LocalizedText + " 3 " + this.StrPerDay.LocalizedText);
								if (flag6)
								{
									this.dBDataLogs.LimitsAddEntry("DateTimeRejecter3PerDay", "Rejecter3PerDay", item.Limit, DateTime.Now.ToString(new CultureInfo("de-DE")));
									num = 3;
								}
								else
								{
									bool flag7 = item.Name.Equals(this.StrTitle.LocalizedText + " 2 " + this.StrPerDay.LocalizedText);
									if (flag7)
									{
										this.dBDataLogs.LimitsAddEntry("DateTimeRejecter2PerDay", "Rejecter2PerDay", item.Limit, DateTime.Now.ToString(new CultureInfo("de-DE")));
										num = 2;
									}
									else
									{
										bool flag8 = item.Name.Equals(this.StrTitle.LocalizedText + " 4 " + this.StrPerDay.LocalizedText);
										if (flag8)
										{
											this.dBDataLogs.LimitsAddEntry("DateTimeRejecter4PerDay", "Rejecter4PerDay", item.Limit, DateTime.Now.ToString(new CultureInfo("de-DE")));
											num = 4;
										}
										else
										{
											bool flag9 = item.Name.Equals(this.StrValve.LocalizedText + " 1 " + this.StrPerDay.LocalizedText);
											if (flag9)
											{
												this.dBDataLogs.LimitsAddEntry("DateTimeRejecter1PerDay", "Rejecter1PerDay", item.Limit, DateTime.Now.ToString(new CultureInfo("de-DE")));
												this.UpdateLimitsAndValues(item.Limit, true);
											}
											else
											{
												bool flag10 = item.Name.Equals(this.StrValveSum.LocalizedText + " " + this.StrPerDay.LocalizedText);
												if (flag10)
												{
													this.dBDataLogs.LimitsAddEntry("DateTimeRejecter2PerDay", "Rejecter2PerDay", item.Limit, DateTime.Now.ToString(new CultureInfo("de-DE")));
												}
												else
												{
													bool flag11 = item.Name.Equals(this.StrValve.LocalizedText + " 1 ");
													if (flag11)
													{
														this.dBDataLogs.LimitsAddEntry("DateTimeRejecter1", "Rejecter1", item.Limit, DateTime.Now.ToString(new CultureInfo("de-DE")));
														this.UpdateLimitsAndValues(item.Limit, false);
													}
													else
													{
														bool flag12 = item.Name.Equals(this.StrValveSum.LocalizedText);
														if (flag12)
														{
															this.dBDataLogs.LimitsAddEntry("DateTimeRejecter2", "Rejecter2", item.Limit, DateTime.Now.ToString(new CultureInfo("de-DE")));
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
			bool flag13 = text.Equals("1") && !item.Name.Equals(this.StrValve.LocalizedText + " 1 " + this.StrPerDay.LocalizedText) && !item.Name.Equals(this.StrValve.LocalizedText + " 1 ");
			if (flag13)
			{
				this.CheckLimit(item, num);
			}
		}

		// Token: 0x06000D6D RID: 3437 RVA: 0x000531A4 File Offset: 0x000513A4
		public void CheckLimit(MonitoringItem item, byte num)
		{
			bool flag = !item.Limit.Equals("") && !item.Limit.Equals("-");
			if (flag)
			{
				string text = "";
				this.dBDataLogs.GetDataLogsSettingsEntry("MessageLevel", ref text);
				bool flag2 = text.Equals("Info");
				SSTEntryCode code;
				if (flag2)
				{
					code = SSTEntryCode.LimitsInfo;
				}
				else
				{
					bool flag3 = text.Equals("Warning");
					if (flag3)
					{
						code = SSTEntryCode.LimitsWarning;
					}
					else
					{
						code = SSTEntryCode.LimitsError;
					}
				}
				text = "";
				this.dBDataLogs.GetDataLogsSettingsEntry("MessageLevel90Perc", ref text);
				bool flag4 = text.Equals("Info");
				SSTEntryCode code2;
				if (flag4)
				{
					code2 = SSTEntryCode.Limits90PerInfo;
				}
				else
				{
					code2 = SSTEntryCode.Limits90PerWarning;
				}
				double num2;
				double.TryParse(item.Limit, out num2);
				double num3;
				double.TryParse(item.CurrentValue, out num3);
				bool flag5 = num3 > num2;
				if (flag5)
				{
					this.errorSurvey.PostMessageMonitoring(code, num, item.ressourceText, item.ressourceText2, num2.ToString(), num3.ToString());
				}
				else
				{
					bool flag6 = num3 >= 0.9 * num2;
					if (flag6)
					{
						this.errorSurvey.PostMessageMonitoring(code2, num, item.ressourceText, item.ressourceText2, num2.ToString(), num3.ToString());
					}
				}
			}
		}

		// Token: 0x06000D6E RID: 3438 RVA: 0x00053300 File Offset: 0x00051500
		private void UpdateLimitsAndValues(string limit, bool perDay)
		{
			bool flag = !limit.Equals("") && !limit.Equals("-");
			if (flag)
			{
				string text = "";
				this.dBDataLogs.GetDataLogsSettingsEntry("MessageLevel", ref text);
				bool flag2 = text.Equals("Info");
				SSTEntryCode code;
				if (flag2)
				{
					code = SSTEntryCode.LimitsInfo;
				}
				else
				{
					bool flag3 = text.Equals("Warning");
					if (flag3)
					{
						code = SSTEntryCode.LimitsWarning;
					}
					else
					{
						code = SSTEntryCode.LimitsError;
					}
				}
				text = "";
				this.dBDataLogs.GetDataLogsSettingsEntry("MessageLevel90Perc", ref text);
				bool flag4 = text.Equals("Info");
				SSTEntryCode code2;
				if (flag4)
				{
					code2 = SSTEntryCode.Limits90PerInfo;
				}
				else
				{
					code2 = SSTEntryCode.Limits90PerWarning;
				}
				double num;
				double.TryParse(limit, out num);
				for (int i = 0; i < this.Items.Count<MonitoringItem>(); i++)
				{
					bool flag5 = !this.Items[i].Name.Contains(this.StrValveSum.LocalizedText) && ((!this.Items[i].Name.Contains(this.StrPerDay.LocalizedText) && !perDay) || (perDay && this.Items[i].Name.Contains(this.StrPerDay.LocalizedText)));
					if (flag5)
					{
						string text2 = "";
						this.dBDataLogs.GetDataLogsSettingsEntry("MonitoringMessagesOnOff", ref text2);
						bool flag6 = text2.Equals("1");
						if (flag6)
						{
							string[] array = this.Items[i].Name.Split(new char[]
							{
								' '
							});
							int num2 = 0;
							bool flag7 = int.TryParse(array[1], out num2);
							if (flag7)
							{
								double num3;
								double.TryParse(this.Items[i].CurrentValue, out num3);
								bool flag8 = num3 > num;
								if (flag8)
								{
									this.errorSurvey.PostMessageMonitoring(code, (byte)num2, this.Items[i].ressourceText, this.Items[i].ressourceText2, limit.ToString(), num3.ToString());
								}
								else
								{
									bool flag9 = num3 >= 0.9 * num;
									if (flag9)
									{
										this.errorSurvey.PostMessageMonitoring(code2, (byte)num2, this.Items[i].ressourceText, this.Items[i].ressourceText2, limit.ToString(), num3.ToString());
									}
								}
							}
						}
					}
				}
			}
			for (int j = 0; j < this.Items.Count<MonitoringItem>(); j++)
			{
				bool flag10 = !this.Items[j].Name.Contains(this.StrValveSum.LocalizedText) && ((!this.Items[j].Name.Contains(this.StrPerDay.LocalizedText) && !perDay) || (perDay && this.Items[j].Name.Contains(this.StrPerDay.LocalizedText)));
				if (flag10)
				{
					this.Items[j].Limit = limit;
				}
			}
		}

		// Token: 0x06000D6F RID: 3439 RVA: 0x00053660 File Offset: 0x00051860
		public override void ShowDiagramm()
		{
			CtrlDataPlots usrControl = new CtrlDataPlots(this.parameterHandling, this.dbConnection, this.dBDataLogs, DataModesMonitoring.rejecter);
			Subwindow.Show(usrControl, SubwindowButtons.BackAndHome);
		}

		// Token: 0x06000D70 RID: 3440 RVA: 0x00053690 File Offset: 0x00051890
		private void CleanUp(object obj)
		{
			bool flag = this.timerDataLogsSeconds != null;
			if (flag)
			{
				this.timerDataLogsSeconds.Stop();
			}
			this.DeregisterLocalizedText();
			this.Items.Clear();
			this.Items = null;
		}

		// Token: 0x040008C0 RID: 2240
		private const string TypeRaycon_EX1 = "Raycon EX1";

		// Token: 0x040008C1 RID: 2241
		private const string TypeRaycon_DPlus_MX = "Raycon D+ MX";

		// Token: 0x040008C2 RID: 2242
		private const string TypeRaycon_DPlus_HX = "Raycon D+ HX";

		// Token: 0x040008C3 RID: 2243
		private const string SubTypeStandard = "Standard";

		// Token: 0x040008C4 RID: 2244
		private const string SubTypeLW = "LW";

		// Token: 0x040008C5 RID: 2245
		private const string SubTypeBulk = "Bulk";

		// Token: 0x040008C6 RID: 2246
		private const string SubTypeLWHy = "LW Hy";

		// Token: 0x040008C7 RID: 2247
		private const string SubTypeBulkHy = "Bulk Hy";

		// Token: 0x040008C8 RID: 2248
		private const string SensorsStandard = "Standard";

		// Token: 0x040008C9 RID: 2249
		private const string SensorsHighlevel = "HighLevel";

		// Token: 0x040008CA RID: 2250
		private const string SensorsAdvanced = "Advanced";

		// Token: 0x040008CD RID: 2253
		private NumKeyboard numKeyboard;

		// Token: 0x040008CE RID: 2254
		private MonitoringItem selectedItem;

		// Token: 0x040008CF RID: 2255
		private int limitNumber;

		// Token: 0x040008D0 RID: 2256
		private List<MonitoringItem> items = new List<MonitoringItem>();

		// Token: 0x040008D7 RID: 2263
		private DBDataLogs dBDataLogs;

		// Token: 0x040008D8 RID: 2264
		private DBConnection dbConnection;

		// Token: 0x040008D9 RID: 2265
		private ParameterHandling parameterHandling;

		// Token: 0x040008DA RID: 2266
		private bool enableLimits = false;

		// Token: 0x040008DB RID: 2267
		private ErrorSurvey errorSurvey;

		// Token: 0x040008DC RID: 2268
		private DispatcherTimer timerDataLogsSeconds;

		// Token: 0x040008DD RID: 2269
		private uint startCount1 = 0U;

		// Token: 0x040008DE RID: 2270
		private uint startCount2 = 0U;

		// Token: 0x040008DF RID: 2271
		private uint startCount3 = 0U;

		// Token: 0x040008E0 RID: 2272
		private uint startCount4 = 0U;

		// Token: 0x040008E1 RID: 2273
		private uint start1 = 0U;

		// Token: 0x040008E2 RID: 2274
		private uint start2 = 0U;

		// Token: 0x040008E3 RID: 2275
		private uint start3 = 0U;

		// Token: 0x040008E4 RID: 2276
		private uint start4 = 0U;

		// Token: 0x040008E5 RID: 2277
		private uint start1PerDay = 0U;

		// Token: 0x040008E6 RID: 2278
		private uint start2PerDay = 0U;

		// Token: 0x040008E7 RID: 2279
		private uint start3PerDay = 0U;

		// Token: 0x040008E8 RID: 2280
		private uint start4PerDay = 0U;

		// Token: 0x040008E9 RID: 2281
		private bool showMessages = false;
	}
}
