using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using DatabaseConnection;

namespace GUI_XRS
{
	// Token: 0x020000CB RID: 203
	public class RejecterActivity : INotifyPropertyChanged
	{
		// Token: 0x14000065 RID: 101
		// (add) Token: 0x0600142F RID: 5167 RVA: 0x000771B0 File Offset: 0x000753B0
		// (remove) Token: 0x06001430 RID: 5168 RVA: 0x000771E8 File Offset: 0x000753E8
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x06001431 RID: 5169 RVA: 0x00077220 File Offset: 0x00075420
		public RejecterActivity(ParameterHandling parHandling, DBConnection dbConnection, DBDataLogs dBDataLogs, MachineState machineState, ErrorSurvey errorSurvey)
		{
			this.parHandling = parHandling;
			this.dbDatalogs = dBDataLogs;
			this.dbConnection = dbConnection;
			this.machineState = machineState;
			this.errorSurvey = errorSurvey;
		}

		// Token: 0x06001432 RID: 5170 RVA: 0x00077290 File Offset: 0x00075490
		public void StartUpdateActivity()
		{
			bool flag = this.parHandling.ReadPar(SVT.svtEnablePath).Equals("1");
			if (flag)
			{
				this.updateThreadRunning = false;
			}
			else
			{
				bool flag2 = !this.updateThreadRunning;
				if (flag2)
				{
					this.updateThreadRunning = true;
					new Thread(new ThreadStart(this.UpdateActivity))
					{
						Name = "UpdateRejecterActivity"
					}.Start();
				}
			}
		}

		// Token: 0x06001433 RID: 5171 RVA: 0x00077304 File Offset: 0x00075504
		public void StopUpdateActivity()
		{
			this.updateThreadRunning = false;
		}

		// Token: 0x06001434 RID: 5172 RVA: 0x00077310 File Offset: 0x00075510
		private void UpdateActivity()
		{
			bool flag = false;
			bool flag2 = true;
			while (this.updateThreadRunning)
			{
				bool flag3 = this.machineState.State == MachineStates.Inspecting;
				if (flag3)
				{
					for (int i = 1; i <= 4; i++)
					{
						flag = (this.parHandling.ReadPar("/hardware/rejecter/rejecter" + i.ToString() + "/Enabled/Value") == "1");
						bool flag4 = flag;
						if (flag4)
						{
							break;
						}
					}
					bool flag5 = flag;
					if (flag5)
					{
						bool flag6 = flag2;
						if (flag6)
						{
							uint num = 0U;
							for (int j = 1; j <= 4; j++)
							{
								uint.TryParse(this.parHandling.ReadPar("/hardware/rejecter/rejecter" + j.ToString() + "/TotalCounts/Value"), out num);
								bool flag7 = j == 1;
								if (flag7)
								{
									this.oldValRejecter1 = num;
								}
								else
								{
									bool flag8 = j == 2;
									if (flag8)
									{
										this.oldValRejecter2 = num;
									}
									else
									{
										bool flag9 = j == 3;
										if (flag9)
										{
											this.oldValRejecter3 = num;
										}
										else
										{
											bool flag10 = j == 4;
											if (flag10)
											{
												this.oldValRejecter4 = num;
											}
										}
									}
								}
							}
							flag2 = false;
						}
						this.saveInDatabase();
						this.saveInDataLogs(true);
					}
				}
				Thread.Sleep(1000);
			}
		}

		// Token: 0x06001435 RID: 5173 RVA: 0x00077474 File Offset: 0x00075674
		private void saveInDatabase()
		{
			List<string> list = new List<string>();
			List<string> list2 = new List<string>();
			for (int i = 1; i <= 4; i++)
			{
				list.Add("/hardware/rejecter/rejecter" + i.ToString() + "/TotalCounts/Value");
				list2.Add(this.parHandling.ReadPar("/hardware/rejecter/rejecter" + i.ToString() + "/TotalCounts/Value"));
			}
			this.dbConnection.UpdateCurrentProductTable(list, list2);
		}

		// Token: 0x06001436 RID: 5174 RVA: 0x000774F8 File Offset: 0x000756F8
		private void saveInDataLogs(bool setOldValues)
		{
			bool flag = false;
			this.dbConnection.GetDBDatalogsIsActive(ref flag);
			bool flag2 = flag;
			if (flag2)
			{
				List<string> list = new List<string>();
				List<string> list2 = new List<string>();
				uint num = 0U;
				uint num2 = 0U;
				uint num3 = 0U;
				for (int i = 1; i <= 4; i++)
				{
					string text = this.parHandling.ReadPar("/hardware/rejecter/rejecter" + i.ToString() + "/TotalCounts/Value");
					bool flag3 = text != "" && uint.TryParse(text, out num) && this.dbDatalogs.OperatingCounterGetIntEntry("CounterRejecter" + i.ToString(), ref num3);
					if (flag3)
					{
						bool flag4 = i == 1;
						if (flag4)
						{
							num2 = this.oldValRejecter1;
						}
						else
						{
							bool flag5 = i == 2;
							if (flag5)
							{
								num2 = this.oldValRejecter2;
							}
							else
							{
								bool flag6 = i == 3;
								if (flag6)
								{
									num2 = this.oldValRejecter3;
								}
								else
								{
									bool flag7 = i == 4;
									if (flag7)
									{
										num2 = this.oldValRejecter4;
									}
								}
							}
						}
						bool flag8 = num > num2;
						if (flag8)
						{
							uint num4 = num - num2;
							num3 += num4;
							list.Add("CounterRejecter" + i.ToString());
							list2.Add(Convert.ToString(num3));
							if (setOldValues)
							{
								bool flag9 = i == 1;
								if (flag9)
								{
									this.oldValRejecter1 = num;
								}
								else
								{
									bool flag10 = i == 2;
									if (flag10)
									{
										this.oldValRejecter2 = num;
									}
									else
									{
										bool flag11 = i == 3;
										if (flag11)
										{
											this.oldValRejecter3 = num;
										}
										else
										{
											bool flag12 = i == 4;
											if (flag12)
											{
												this.oldValRejecter4 = num;
											}
										}
									}
								}
							}
							bool flag13 = !this.dbDatalogs.OnOffEntryDoesExist(DateTime.Now.ToString("d", new CultureInfo("de-DE")));
							if (flag13)
							{
								this.dbDatalogs.SetNewOnOffTimesEntry(DateTime.Now.ToString("d", new CultureInfo("de-DE")), "0", "0", "0", "0", "0", "0", "0", "0", "0");
							}
							this.dbDatalogs.UpdateOnOffTimes(DateTime.Now.ToString("d", new CultureInfo("de-DE")), "CounterRejecter" + i.ToString(), (int)num4);
							string text2 = "";
							this.dbDatalogs.GetDataLogsSettingsEntry("MonitoringMessagesOnOff", ref text2);
							bool flag14 = text2.Equals("1");
							if (flag14)
							{
								string text3 = "";
								this.dbDatalogs.OnOffTimesGetEntryOfDate(DateTime.Now.ToString("d", new CultureInfo("de-DE")), ref text3, "CounterRejecter" + i.ToString());
								double num5 = 0.0;
								bool flag15 = double.TryParse(text3.Replace(',', '.'), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out num5);
								if (flag15)
								{
									text3 = "";
									double num6 = 0.0;
									this.dbDatalogs.GetLimitsEntry("Rejecter" + i.ToString() + "PerDay", ref text3);
									bool flag16 = !text3.Equals("-") && !text3.Equals("") && !text3.Equals(" ");
									if (flag16)
									{
										bool flag17 = double.TryParse(text3.Replace(',', '.'), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out num6);
										if (flag17)
										{
											bool flag18 = num6 < num5 && !this.rejectMessageFirst[i - 1];
											if (flag18)
											{
												text2 = "";
												this.dbDatalogs.GetDataLogsSettingsEntry("MessageLevel", ref text2);
												bool flag19 = text2.Equals("Info");
												SSTEntryCode code;
												if (flag19)
												{
													code = SSTEntryCode.LimitsInfo;
												}
												else
												{
													bool flag20 = text2.Equals("Warning");
													if (flag20)
													{
														code = SSTEntryCode.LimitsWarning;
													}
													else
													{
														code = SSTEntryCode.LimitsError;
													}
												}
												this.errorSurvey.PostMessageMonitoring(code, (byte)i, "txtRejecter", "txtPerDay", num6.ToString(), num5.ToString());
												this.rejectMessageFirst[i - 1] = true;
											}
											else
											{
												bool flag21 = num6 * 0.9 <= num5 && num6 >= num5 && !this.rejectMessage90First[i - 1];
												if (flag21)
												{
													text2 = "";
													this.dbDatalogs.GetDataLogsSettingsEntry("MessageLevel90Perc", ref text2);
													bool flag22 = text2.Equals("Info");
													SSTEntryCode code2;
													if (flag22)
													{
														code2 = SSTEntryCode.Limits90PerInfo;
													}
													else
													{
														code2 = SSTEntryCode.Limits90PerWarning;
													}
													this.errorSurvey.PostMessageMonitoring(code2, (byte)i, "txtRejecter", "txtPerDay", num6.ToString(), num5.ToString());
													this.rejectMessage90First[i - 1] = true;
												}
												else
												{
													bool flag23 = num6 >= num5;
													if (flag23)
													{
														this.rejectMessageFirst[i - 1] = false;
														bool flag24 = num6 * 0.9 > num5;
														if (flag24)
														{
															this.rejectMessage90First[i - 1] = false;
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
				this.dbDatalogs.OperatingCounterAddEntries(list, list2);
			}
		}

		// Token: 0x06001437 RID: 5175 RVA: 0x00077A52 File Offset: 0x00075C52
		public void SaveValuesBeforeProductChanged()
		{
			this.StopUpdateActivity();
			this.saveInDatabase();
			this.saveInDataLogs(false);
		}

		// Token: 0x06001438 RID: 5176 RVA: 0x00077A6C File Offset: 0x00075C6C
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x04000D5B RID: 3419
		private const int updateInterval = 1000;

		// Token: 0x04000D5C RID: 3420
		private bool updateThreadRunning;

		// Token: 0x04000D5D RID: 3421
		private ParameterHandling parHandling;

		// Token: 0x04000D5E RID: 3422
		private DBConnection dbConnection;

		// Token: 0x04000D5F RID: 3423
		private DBDataLogs dbDatalogs;

		// Token: 0x04000D60 RID: 3424
		private MachineState machineState;

		// Token: 0x04000D61 RID: 3425
		private uint oldValRejecter1 = 0U;

		// Token: 0x04000D62 RID: 3426
		private uint oldValRejecter2 = 0U;

		// Token: 0x04000D63 RID: 3427
		private uint oldValRejecter3 = 0U;

		// Token: 0x04000D64 RID: 3428
		private uint oldValRejecter4 = 0U;

		// Token: 0x04000D66 RID: 3430
		private bool[] rejectMessageFirst = new bool[4];

		// Token: 0x04000D67 RID: 3431
		private bool[] rejectMessage90First = new bool[4];

		// Token: 0x04000D68 RID: 3432
		private ErrorSurvey errorSurvey;
	}
}
