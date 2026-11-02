using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
using DatabaseConnection;

namespace GUI_XRS
{
	// Token: 0x02000052 RID: 82
	public class ValveActivity : INotifyPropertyChanged
	{
		// Token: 0x14000030 RID: 48
		// (add) Token: 0x06000870 RID: 2160 RVA: 0x00035B38 File Offset: 0x00033D38
		// (remove) Token: 0x06000871 RID: 2161 RVA: 0x00035B70 File Offset: 0x00033D70
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x17000270 RID: 624
		// (get) Token: 0x06000872 RID: 2162 RVA: 0x00035BA8 File Offset: 0x00033DA8
		// (set) Token: 0x06000873 RID: 2163 RVA: 0x00035BC0 File Offset: 0x00033DC0
		public string Activity
		{
			get
			{
				return this.activity;
			}
			set
			{
				this.activity = value;
				this.OnPropertyChanged("Activity");
			}
		}

		// Token: 0x17000271 RID: 625
		// (get) Token: 0x06000874 RID: 2164 RVA: 0x00035BD8 File Offset: 0x00033DD8
		public short[] Activities
		{
			get
			{
				return this.activities;
			}
		}

		// Token: 0x17000272 RID: 626
		// (get) Token: 0x06000875 RID: 2165 RVA: 0x00035BF0 File Offset: 0x00033DF0
		public int[] SumActivities
		{
			get
			{
				return this.sumActivities;
			}
		}

		// Token: 0x17000273 RID: 627
		// (get) Token: 0x06000876 RID: 2166 RVA: 0x00035C08 File Offset: 0x00033E08
		public int NumActivitesValves
		{
			get
			{
				return this.numActivitiesValves;
			}
		}

		// Token: 0x17000274 RID: 628
		// (get) Token: 0x06000877 RID: 2167 RVA: 0x00035C20 File Offset: 0x00033E20
		public int[] ActivitiesForRejectCounter
		{
			get
			{
				return this.activitiesForRejectCounter;
			}
		}

		// Token: 0x17000275 RID: 629
		// (get) Token: 0x06000878 RID: 2168 RVA: 0x00035C38 File Offset: 0x00033E38
		public int[] ThroughputForRejectCounter
		{
			get
			{
				return this.throughputForRejectCounter;
			}
		}

		// Token: 0x17000276 RID: 630
		// (get) Token: 0x06000879 RID: 2169 RVA: 0x00035C50 File Offset: 0x00033E50
		public DBConnection DbConnection
		{
			get
			{
				return this.dbConnection;
			}
		}

		// Token: 0x17000277 RID: 631
		// (get) Token: 0x0600087A RID: 2170 RVA: 0x00035C68 File Offset: 0x00033E68
		// (set) Token: 0x0600087B RID: 2171 RVA: 0x00035C70 File Offset: 0x00033E70
		public int SumCurrentActivities { get; set; }

		// Token: 0x0600087C RID: 2172 RVA: 0x00035C7C File Offset: 0x00033E7C
		public ValveActivity(ParameterHandling parHandling, MachineState machineState, DBConnection dbConnection, DBDataLogs dbDataLogs, ErrorSurvey errorSurvey)
		{
			this.parHandling = parHandling;
			this.machineState = machineState;
			this.dbConnection = dbConnection;
			this.dbDataLogs = dbDataLogs;
			this.errorSurvey = errorSurvey;
			int num = 0;
			dbConnection.GetNumValves(ref num);
			bool flag = parHandling.ReadPar(SVT.svtEnablePath) == "1";
			if (flag)
			{
				this.rejectMessageFirst = new bool[num];
				this.rejectMessage90First = new bool[num];
			}
			this.activities = new short[60];
			this.sumActivities = new int[60];
			this.activitiesForRejectCounter = new int[60];
			this.throughputForRejectCounter = new int[60];
		}

		// Token: 0x0600087D RID: 2173 RVA: 0x00035D28 File Offset: 0x00033F28
		public void StartUpdateActivity()
		{
			bool flag = !this.updateThreadRunning;
			if (flag)
			{
				this.updateThreadRunning = true;
				new Thread(new ThreadStart(this.UpdateActivity))
				{
					Name = "UpdateValveActivity"
				}.Start();
			}
		}

		// Token: 0x0600087E RID: 2174 RVA: 0x00035D71 File Offset: 0x00033F71
		public void StopUpdateActivity()
		{
			this.updateThreadRunning = false;
		}

		// Token: 0x0600087F RID: 2175 RVA: 0x00035D7C File Offset: 0x00033F7C
		private void UpdateActivity()
		{
			int num = 0;
			int num2 = 0;
			bool flag = false;
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			bool flag2 = false;
			int num6 = 0;
			this.dbConnection.GetNumValves(ref num6);
			int[] array = new int[num6];
			int num7 = DateTime.Now.Second;
			while (this.updateThreadRunning)
			{
				bool flag3 = this.machineState.State == MachineStates.Inspecting;
				if (flag3)
				{
					bool flag4 = this.parHandling.ReadPar(FlapsOverIOs.bulkflapsEnabledPath) == "1" && this.parHandling.ReadPar(FlapsOverIOs.bulkflapsValveTestIsRunningPath) == "0";
					if (flag4)
					{
						int num8 = Convert.ToInt32(this.parHandling.ReadPar(FlapsOverIOs.bulkflapsValveActivityFlap1));
						int num9 = Convert.ToInt32(this.parHandling.ReadPar(FlapsOverIOs.bulkflapsValveActivityFlap2));
						int num10 = Convert.ToInt32(this.parHandling.ReadPar(FlapsOverIOs.bulkflapsValveActivityFlap3));
						int num11 = Convert.ToInt32(this.parHandling.ReadPar(FlapsOverIOs.bulkflapsValveActivityFlap4));
						int num12 = Convert.ToInt32(this.parHandling.ReadPar(FlapsOverIOs.bulkflapsValveActivityThroughput));
						int num13 = num8 + num9 + num10 + num11;
						bool flag5 = num7 >= 60;
						if (flag5)
						{
							DateTime now = DateTime.Now;
							num7 = now.Second;
							DateTime date = (now.Second > 30) ? now.AddSeconds((double)(60 - now.Second)) : now.AddSeconds((double)(-1 * now.Second));
							this.dbConnection.ValveActivityDateEntryExists(ref flag, date);
							bool flag6 = flag;
							if (flag6)
							{
								date = date.AddMinutes(1.0);
								this.dbConnection.ValveActivityDateEntryExists(ref flag, date);
							}
							bool flag7 = !flag;
							if (flag7)
							{
								this.dbConnection.AddValveActivity(date, num8, num9, num10, num11, num12);
							}
							this.parHandling.WriteValue(FlapsOverIOs.bulkflapsValveActivityFlap1, "0");
							this.parHandling.WriteValue(FlapsOverIOs.bulkflapsValveActivityFlap2, "0");
							this.parHandling.WriteValue(FlapsOverIOs.bulkflapsValveActivityFlap3, "0");
							this.parHandling.WriteValue(FlapsOverIOs.bulkflapsValveActivityFlap4, "0");
							this.parHandling.WriteValue(FlapsOverIOs.bulkflapsValveActivityThroughput, "0");
							flag2 = true;
						}
						else
						{
							num7++;
						}
						bool flag8 = num3 < 60 && num13 >= num5 && num12 >= num4;
						if (flag8)
						{
							this.activitiesForRejectCounter[num3] = num13 - num5;
							this.throughputForRejectCounter[num3] = num12 - num4;
							this.Activity = this.activitiesForRejectCounter.Sum((int value) => value).ToString();
						}
						num5 = num13;
						num4 = num12;
						num3++;
						bool flag9 = flag2;
						if (flag9)
						{
							flag2 = false;
							num3 = 0;
							num5 = 0;
							num4 = 0;
						}
					}
					else
					{
						bool flag10 = this.parHandling.ReadPar(SVT.svtEnablePath) == "1";
						if (flag10)
						{
							bool flag11 = num7 >= 60;
							if (flag11)
							{
								DateTime now = DateTime.Now;
								num7 = now.Second;
								bool flag12 = now.Second > 30;
								DateTime date2;
								if (flag12)
								{
									date2 = now.AddSeconds((double)(60 - now.Second));
								}
								else
								{
									date2 = now.AddSeconds((double)(-1 * now.Second));
								}
								this.dbConnection.ValveActivityDateEntryExists(ref flag, date2);
								bool flag13 = flag;
								if (flag13)
								{
									date2 = date2.AddMinutes(1.0);
									this.dbConnection.ValveActivityDateEntryExists(ref flag, date2);
								}
								bool flag14 = !flag;
								if (flag14)
								{
									this.dbConnection.AddValveActivity(date2, this.numActivitiesValves, 0, 0, 0, 0);
								}
								Array.Clear(this.sumActivities, 0, this.sumActivities.Length);
								this.numActivitiesValves = 0;
							}
							else
							{
								num7++;
							}
							string s = this.parHandling.ReadPar(ValveActivity.svtNRejectsPath);
							string text = "";
							int num14 = 0;
							uint num15 = 0U;
							this.dbDataLogs.GetLimitsEntry("Rejecter1PerDay", ref text);
							bool flag15 = !text.Equals("-") && !text.Equals("") && !text.Equals(" ");
							if (flag15)
							{
								int.TryParse(text, out num14);
							}
							string text2 = "";
							this.dbDataLogs.GetDataLogsSettingsEntry("MessageLevel", ref text2);
							bool flag16 = text2.Equals("Info");
							SSTEntryCode code;
							if (flag16)
							{
								code = SSTEntryCode.LimitsInfo;
							}
							else
							{
								bool flag17 = text2.Equals("Warning");
								if (flag17)
								{
									code = SSTEntryCode.LimitsWarning;
								}
								else
								{
									code = SSTEntryCode.LimitsError;
								}
							}
							text2 = "";
							this.dbDataLogs.GetDataLogsSettingsEntry("MessageLevel90Perc", ref text2);
							bool flag18 = text2.Equals("Info");
							SSTEntryCode code2;
							if (flag18)
							{
								code2 = SSTEntryCode.Limits90PerInfo;
							}
							else
							{
								code2 = SSTEntryCode.Limits90PerWarning;
							}
							bool flag19 = this.rejectMessageFirst == null;
							if (flag19)
							{
								this.rejectMessageFirst = new bool[num6];
								this.rejectMessage90First = new bool[num6];
							}
							else
							{
								bool flag20 = this.rejectMessageFirst.Length < num6;
								if (flag20)
								{
									this.rejectMessageFirst = new bool[num6];
									this.rejectMessage90First = new bool[num6];
								}
							}
							int num16 = 0;
							for (int i = 1; i <= num6; i++)
							{
								string text3 = "/hardware/svt/valve";
								bool flag21 = i < 10;
								if (flag21)
								{
									text3 = text3 + "0" + i.ToString() + "/Rejects/Value";
								}
								else
								{
									text3 = text3 + i.ToString() + "/Rejects/Value";
								}
								s = this.parHandling.ReadPar(text3);
								bool flag22 = int.TryParse(s, out num);
								if (flag22)
								{
									int num17 = num - array[i - 1];
									bool flag23 = num17 > 0;
									if (flag23)
									{
										num16 += num17;
										this.dbDataLogs.UpdateOnOffTimes(DateTime.Now.ToString("d", new CultureInfo("de-DE")), "SVTValve" + i.ToString(), num17);
										this.dbDataLogs.OperatingCounterAddValvesValues("CounterRejecter" + i.ToString(), num17);
										array[i - 1] = num;
										string a = "";
										this.dbDataLogs.GetDataLogsSettingsEntry("MonitoringMessagesOnOff", ref a);
										bool flag24 = a == "1";
										if (flag24)
										{
											this.dbDataLogs.OnOffTimesGetEntryOfDate(DateTime.Now.ToString("d", new CultureInfo("de-DE")), ref text, "SVTValve" + i.ToString());
											bool flag25 = uint.TryParse(text, out num15) && num14 > 0;
											if (flag25)
											{
												bool flag26 = (long)num14 < (long)((ulong)num15) && !this.rejectMessageFirst[i - 1];
												if (flag26)
												{
													this.errorSurvey.PostMessageMonitoring(code, (byte)i, "txtValve", "txtPerDay", num14.ToString(), num15.ToString());
													this.rejectMessageFirst[i - 1] = true;
												}
												else
												{
													bool flag27 = (double)num14 * 0.9 <= num15 && (long)num14 >= (long)((ulong)num15) && !this.rejectMessage90First[i - 1];
													if (flag27)
													{
														this.errorSurvey.PostMessageMonitoring(code2, (byte)i, "txtValve", "txtPerDay", num14.ToString(), num15.ToString());
														this.rejectMessage90First[i - 1] = true;
													}
													else
													{
														bool flag28 = (long)num14 >= (long)((ulong)num15);
														if (flag28)
														{
															this.rejectMessageFirst[i - 1] = false;
															bool flag29 = (double)num14 * 0.9 > num15;
															if (flag29)
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
							bool flag30 = num16 > 0;
							if (flag30)
							{
								bool flag31 = false;
								this.dbConnection.GetBatchesEnabled(ref flag31);
								bool flag32 = flag31;
								if (flag32)
								{
									this.dbConnection.IncreaseCurrentBatchRejects(num16);
								}
							}
							this.activities[num2] = (short)num16;
							this.Activity = this.activities.Sum((short value) => (int)value).ToString();
							this.sumActivities[num2] = Convert.ToInt32(this.Activity);
							this.numActivitiesValves += num16;
							num2++;
							bool flag33 = num2 == this.activities.Length;
							if (flag33)
							{
								num2 = 0;
							}
						}
					}
				}
				else
				{
					this.Activity = "0";
					Array.Clear(this.activitiesForRejectCounter, 0, this.activitiesForRejectCounter.Length);
					Array.Clear(this.throughputForRejectCounter, 0, this.throughputForRejectCounter.Length);
					Array.Clear(this.activities, 0, this.activities.Length);
					num4 = 0;
					num5 = 0;
					this.parHandling.WriteValue(FlapsOverIOs.bulkflapsValveActivityFlap1, "0");
					this.parHandling.WriteValue(FlapsOverIOs.bulkflapsValveActivityFlap2, "0");
					this.parHandling.WriteValue(FlapsOverIOs.bulkflapsValveActivityFlap3, "0");
					this.parHandling.WriteValue(FlapsOverIOs.bulkflapsValveActivityFlap4, "0");
					this.numActivitiesValves = 0;
				}
				Thread.Sleep(1000);
			}
		}

		// Token: 0x06000880 RID: 2176 RVA: 0x00036724 File Offset: 0x00034924
		private string calculateSlidingAverage()
		{
			double value = 0.0;
			double num = 0.0;
			double.TryParse(this.parHandling.ReadPar(FlapsOverIOs.bulkflapsFlapsCountPath), out num);
			bool flag = this.throughputForRejectCounter.Sum() != 0 && num != 0.0;
			if (flag)
			{
				value = 100.0 * (Convert.ToDouble(this.activitiesForRejectCounter.Sum()) / (Convert.ToDouble(this.throughputForRejectCounter.Sum()) * num));
			}
			return Math.Round(value, 3).ToString();
		}

		// Token: 0x06000881 RID: 2177 RVA: 0x000367C4 File Offset: 0x000349C4
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x040005BB RID: 1467
		public static readonly string svtNRejectsPath = "/hardware/svt/NRejects/Value";

		// Token: 0x040005BC RID: 1468
		private const int updateInterval = 1000;

		// Token: 0x040005BD RID: 1469
		private const int activitiesSize = 60;

		// Token: 0x040005BE RID: 1470
		private bool updateThreadRunning;

		// Token: 0x040005BF RID: 1471
		private ParameterHandling parHandling;

		// Token: 0x040005C0 RID: 1472
		private MachineState machineState;

		// Token: 0x040005C1 RID: 1473
		private DBConnection dbConnection;

		// Token: 0x040005C2 RID: 1474
		private string activity;

		// Token: 0x040005C3 RID: 1475
		private short[] activities;

		// Token: 0x040005C4 RID: 1476
		private int[] sumActivities;

		// Token: 0x040005C5 RID: 1477
		private int numActivitiesValves;

		// Token: 0x040005C7 RID: 1479
		private int[] activitiesForRejectCounter;

		// Token: 0x040005C8 RID: 1480
		private int[] throughputForRejectCounter;

		// Token: 0x040005CA RID: 1482
		private DBDataLogs dbDataLogs;

		// Token: 0x040005CB RID: 1483
		private ErrorSurvey errorSurvey;

		// Token: 0x040005CC RID: 1484
		private bool[] rejectMessageFirst;

		// Token: 0x040005CD RID: 1485
		private bool[] rejectMessage90First;
	}
}
