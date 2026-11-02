using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using DatabaseConnection;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x0200009C RID: 156
	public class DBDataLogsManager
	{
		// Token: 0x06000FA4 RID: 4004 RVA: 0x0005D580 File Offset: 0x0005B780
		public DBDataLogsManager(DBDataLogs dbDatalogs, ParameterHandling parHandling, MachineState machineState, ErrorSurvey errorSurvey, DBConnection dbConnection)
		{
			this.dbDatalogs = dbDatalogs;
			this.parHandling = parHandling;
			this.machineState = machineState;
			this.errorSurvey = errorSurvey;
			this.dbConnection = dbConnection;
			this.timerDataLogsSeconds = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, new EventHandler(this.WriteValuesToDBDataLogsSecond), Application.Current.Dispatcher);
			this.timerDataLogsMinutes = new DispatcherTimer(new TimeSpan(0, 1, 0), DispatcherPriority.Normal, new EventHandler(this.WriteValuesToDBDataLogsMinute), Application.Current.Dispatcher);
			this.timerDataLogsHours = new DispatcherTimer(new TimeSpan(1, 0, 0), DispatcherPriority.Normal, new EventHandler(this.WriteValuesToDBDataLogsHour), Application.Current.Dispatcher);
		}

		// Token: 0x06000FA5 RID: 4005 RVA: 0x0005D694 File Offset: 0x0005B894
		public void StartTimer()
		{
			string text = this.parHandling.ReadPar("/hardware/detector/Type/Value");
			string machineType = this.parHandling.ReadPar(Definition.machineTypePath);
			string machineSubType = this.parHandling.ReadPar(Definition.machineSubTypePath);
			string detectorResolution = this.parHandling.ReadPar(Definition.detectorSesotecPixelSizePath);
			bool flag = this.timerDataLogsSeconds != null && this.timerDataLogsMinutes != null && this.timerDataLogsHours != null;
			if (flag)
			{
				this.timerDataLogsSeconds.Start();
				this.timerDataLogsMinutes.Start();
				this.timerDataLogsHours.Start();
				bool flag2 = text == "4";
				if (flag2)
				{
					bool flag3 = !this.dbDatalogs.MachineInfosContainsDtDetectorEntries();
					if (flag3)
					{
						this.dbDatalogs.ClearMachineInfos();
						this.SetDBTableMachineInfos();
					}
					bool flag4 = !this.dbDatalogs.DefaultLimitsContainsTXEntries();
					if (flag4)
					{
						this.dbDatalogs.ClearDefaultLimits();
						this.dbDatalogs.CreateDefaultEntries(DBDataLogs.DefaultLimits);
						this.dbDatalogs.setCommissioningDefaultLimits("Raycon D+ TX", machineSubType, "DTNetTDI");
					}
				}
				bool flag5 = !this.initialized;
				if (flag5)
				{
					int num = -1;
					this.dbDatalogs.GetNumberLimits(ref num);
					bool flag6 = num == 0;
					if (flag6)
					{
						this.SetDBTableMachineInfos();
						this.dbDatalogs.setDefaultLimits();
					}
					else
					{
						this.dbDatalogs.setMaxLimitsTemperatures(text, machineType, detectorResolution);
					}
					this.initialized = true;
				}
				bool flag7 = !this.dbDatalogs.OnOffEntryDoesExist(DateTime.Now.ToString("d", new CultureInfo("de-DE")));
				if (flag7)
				{
					this.dbDatalogs.SetNewOnOffTimesEntry(DateTime.Now.ToString("d", new CultureInfo("de-DE")), "0", "0", "0", "0", "0", "0", "0", "0", "0");
				}
				string text2 = "";
				this.dbDatalogs.GetDataLogsSettingsEntry("MonitoringMessagesOnOff", ref text2);
				bool flag8 = text2.Equals("1");
				if (flag8)
				{
					this.CheckValues();
					this.CheckValuesHourly();
				}
			}
		}

		// Token: 0x06000FA6 RID: 4006 RVA: 0x0005D8D8 File Offset: 0x0005BAD8
		public void StopTimer()
		{
			bool flag = this.timerDataLogsSeconds != null && this.timerDataLogsMinutes != null && this.timerDataLogsHours != null;
			if (flag)
			{
				this.timerDataLogsSeconds.Stop();
				this.timerDataLogsMinutes.Stop();
				this.timerDataLogsHours.Stop();
			}
		}

		// Token: 0x06000FA7 RID: 4007 RVA: 0x0005D92C File Offset: 0x0005BB2C
		private void WriteValuesToDBDataLogsSecond(object dummy, EventArgs e)
		{
			bool flag = this.parHandling != null;
			if (flag)
			{
				this.SetValuesForSwitchOnCycles();
				bool flag2 = !this.isHour;
				if (flag2)
				{
					this.addListEntryFilamentCurrent();
				}
			}
		}

		// Token: 0x06000FA8 RID: 4008 RVA: 0x0005D964 File Offset: 0x0005BB64
		private void WriteValuesToDBDataLogsMinute(object dummy, EventArgs e)
		{
			string a = this.parHandling.ReadPar("/hardware/detector/Type/Value");
			string machineSubType = this.parHandling.ReadPar(Definition.machineSubTypePath);
			bool flag = a == "4";
			if (flag)
			{
				bool flag2 = !this.dbDatalogs.MachineInfosContainsDtDetectorEntries();
				if (flag2)
				{
					this.dbDatalogs.ClearMachineInfos();
					this.SetDBTableMachineInfos();
				}
				bool flag3 = !this.dbDatalogs.DefaultLimitsContainsTXEntries();
				if (flag3)
				{
					this.dbDatalogs.ClearDefaultLimits();
					this.dbDatalogs.CreateDefaultEntries(DBDataLogs.DefaultLimits);
					this.dbDatalogs.setCommissioningDefaultLimits("Raycon D+ TX", machineSubType, "DTNetTDI");
				}
			}
			this.SetDBTableMachineInfos();
			this.SetDBTableOperatingCounter();
			bool flag4 = !this.versionInitalized;
			if (flag4)
			{
				List<string> list = new List<string>();
				List<string> list2 = new List<string>();
				string str = "/Value";
				string text = "/application/ElinosVersion";
				list.Add(text);
				list2.Add(this.parHandling.ReadPar(text + str));
				text = "/application/GUIVersion";
				list.Add(text);
				list2.Add(this.parHandling.ReadPar(text + str));
				text = "/application/DatabaseVersion";
				list.Add(text);
				list2.Add(this.parHandling.ReadPar(text + str));
				this.dbDatalogs.MachineInfosAddEntries(list, list2);
				this.versionInitalized = true;
			}
		}

		// Token: 0x06000FA9 RID: 4009 RVA: 0x0005DAF4 File Offset: 0x0005BCF4
		private void WriteValuesToDBDataLogsHour(object dummy, EventArgs e)
		{
			this.isHour = true;
			this.SetDBTableTemperatures();
			this.SetDBTableFilamentCurrent();
			int num = 0;
			string text = "";
			this.dbDatalogs.GetDataLogsSettingsEntry("MonitoringMessagesOnOff", ref text);
			bool flag = text.Equals("1");
			if (flag)
			{
				this.CheckValuesHourly();
				text = "";
				this.dbDatalogs.GetDataLogsSettingsEntry("TimeSpanRepetitionMessage", ref text);
				bool flag2 = int.TryParse(text, out num);
				if (flag2)
				{
					bool flag3 = this.countRepeatMessage >= num - 1;
					if (flag3)
					{
						this.CheckValues();
						this.countRepeatMessage = 0;
					}
					else
					{
						this.countRepeatMessage++;
					}
				}
			}
			this.isHour = false;
		}

		// Token: 0x06000FAA RID: 4010 RVA: 0x0005DBB0 File Offset: 0x0005BDB0
		private void SetDBTableTemperatures()
		{
			string text = this.parHandling.ReadPar("/hardware/ios/Temp_00/Value");
			string text2 = this.parHandling.ReadPar("/hardware/xraysource/TankTemperature/Value");
			string a = this.parHandling.ReadPar("/hardware/detector/Type/Value");
			bool flag = a == "2" || a == "3" || a == "4";
			string s;
			if (flag)
			{
				s = this.parHandling.ReadPar("/hardware/detector/dt/MCUTemperature/Value");
			}
			else
			{
				s = this.parHandling.ReadPar("/hardware/detector/sesotec/Temperature/Value");
			}
			double num = 0.0;
			double.TryParse(s, out num);
			num = Math.Round(num / 10.0, 1);
			this.dbDatalogs.TemperaturesAddEntry(text, text2, num.ToString(new CultureInfo("en-US")));
			double num2 = 0.0;
			double num3 = 0.0;
			string text3 = "";
			string text4 = "";
			this.dbDatalogs.GetDataLogsSettingsEntry("MonitoringMessagesOnOff", ref text4);
			bool flag2 = text4.Equals("1");
			if (flag2)
			{
				this.dbDatalogs.GetDataLogsSettingsEntry("MessageLevel", ref text4);
				bool flag3 = text4.Equals("Info");
				SSTEntryCode code;
				if (flag3)
				{
					code = SSTEntryCode.LimitsInfo;
				}
				else
				{
					bool flag4 = text4.Equals("Warning");
					if (flag4)
					{
						code = SSTEntryCode.LimitsWarning;
					}
					else
					{
						code = SSTEntryCode.LimitsError;
					}
				}
				text4 = "";
				this.dbDatalogs.GetDataLogsSettingsEntry("MessageLevel90Perc", ref text4);
				bool flag5 = text4.Equals("Info");
				SSTEntryCode code2;
				if (flag5)
				{
					code2 = SSTEntryCode.Limits90PerInfo;
				}
				else
				{
					code2 = SSTEntryCode.Limits90PerWarning;
				}
				this.dbDatalogs.GetLimitsEntry("Temperature", ref text3);
				bool flag6 = !text3.Equals("-") && !text3.Equals("") && !text3.Equals(" ");
				if (flag6)
				{
					bool flag7 = double.TryParse(text3.Replace(',', '.'), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out num2);
					if (flag7)
					{
						bool flag8 = double.TryParse(text.Replace(',', '.'), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out num3);
						if (flag8)
						{
							bool flag9 = num2 < num3;
							if (flag9)
							{
								this.errorSurvey.PostMessageMonitoring(code, 0, "txtTemperature", "txtMachine", num2.ToString(), num3.ToString());
							}
							else
							{
								bool flag10 = num2 * 0.9 <= num3;
								if (flag10)
								{
									this.errorSurvey.PostMessageMonitoring(code2, 0, "txtTemperature", "txtMachine", num2.ToString(), num3.ToString());
								}
							}
						}
					}
				}
				num2 = 0.0;
				num3 = 0.0;
				text3 = "";
				this.dbDatalogs.GetLimitsEntry("TemperatureSource", ref text3);
				bool flag11 = !text3.Equals("-") && !text3.Equals("") && !text3.Equals(" ");
				if (flag11)
				{
					bool flag12 = double.TryParse(text3.Replace(',', '.'), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out num2);
					if (flag12)
					{
						bool flag13 = double.TryParse(text2.Replace(',', '.'), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out num3);
						if (flag13)
						{
							bool flag14 = num2 < num3;
							if (flag14)
							{
								this.errorSurvey.PostMessageMonitoring(code, 0, "txtTemperature", "parItemXRaySource", num2.ToString(), num3.ToString());
							}
							else
							{
								bool flag15 = num2 * 0.9 <= num3;
								if (flag15)
								{
									this.errorSurvey.PostMessageMonitoring(code2, 0, "txtTemperature", "parItemXRaySource", num2.ToString(), num3.ToString());
								}
							}
						}
					}
				}
				num2 = 0.0;
				num3 = 0.0;
				text3 = "";
				this.dbDatalogs.GetLimitsEntry("TemperatureDetector", ref text3);
				bool flag16 = !text3.Equals("-") && !text3.Equals("") && !text3.Equals(" ");
				if (flag16)
				{
					bool flag17 = double.TryParse(text3.Replace(',', '.'), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out num2);
					if (flag17)
					{
						bool flag18 = num2 < num;
						if (flag18)
						{
							this.errorSurvey.PostMessageMonitoring(code, 0, "txtTemperature", "parItemDetector", num2.ToString(), num.ToString());
						}
						else
						{
							bool flag19 = num2 * 0.9 <= num;
							if (flag19)
							{
								this.errorSurvey.PostMessageMonitoring(code2, 0, "txtTemperature", "parItemDetector", num2.ToString(), num.ToString());
							}
						}
					}
				}
			}
		}

		// Token: 0x06000FAB RID: 4011 RVA: 0x0005E0A0 File Offset: 0x0005C2A0
		private void SetDBTableFilamentCurrent()
		{
			bool flag = this.listFilamentCurrent != null && this.listFilamentCurrent.Count > 0;
			if (flag)
			{
				string sourcevolt = this.parHandling.ReadPar("/hardware/xraysource/Voltage/Value");
				string sourcecurrent = this.parHandling.ReadPar("/hardware/xraysource/Current/Value");
				string filament = this.parHandling.ReadPar("/hardware/xraysource/FilamentCurrent/Value");
				string filamentMin = Convert.ToString(this.listFilamentCurrent.Min());
				string text = Convert.ToString(this.listFilamentCurrent.Max());
				string filamentAvg = Convert.ToString(Math.Round(this.listFilamentCurrent.Average(), 2));
				this.dbDatalogs.FilamentCurrentAddEntry(sourcevolt, sourcecurrent, filament, filamentMin, text, filamentAvg);
				string text2 = "";
				this.dbDatalogs.GetDataLogsSettingsEntry("MonitoringMessagesOnOff", ref text2);
				bool flag2 = text2.Equals("1");
				if (flag2)
				{
					this.dbDatalogs.GetDataLogsSettingsEntry("MessageLevel", ref text2);
					bool flag3 = text2.Equals("Info");
					SSTEntryCode code;
					if (flag3)
					{
						code = SSTEntryCode.LimitsInfo;
					}
					else
					{
						bool flag4 = text2.Equals("Warning");
						if (flag4)
						{
							code = SSTEntryCode.LimitsWarning;
						}
						else
						{
							code = SSTEntryCode.LimitsError;
						}
					}
					string text3 = "";
					double num = 0.0;
					this.dbDatalogs.GetLimitsEntry("FilamentCurrent", ref text3);
					bool flag5 = !text3.Equals("-") && !text3.Equals("") && !text3.Equals(" ");
					if (flag5)
					{
						bool flag6 = double.TryParse(text3.Replace(',', '.'), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out num);
						if (flag6)
						{
							bool flag7 = num < this.listFilamentCurrent.Max();
							if (flag7)
							{
								this.errorSurvey.PostMessageMonitoring(code, 0, "txtFilamentCurrent", "", num.ToString(), text);
							}
							else
							{
								bool flag8 = num * 0.9 <= this.listFilamentCurrent.Max();
								if (flag8)
								{
									this.dbDatalogs.GetDataLogsSettingsEntry("MessageLevel90Perc", ref text2);
									bool flag9 = text2.Equals("Info");
									if (flag9)
									{
										code = SSTEntryCode.Limits90PerInfo;
									}
									else
									{
										code = SSTEntryCode.Limits90PerWarning;
									}
									this.errorSurvey.PostMessageMonitoring(code, 0, "txtFilamentCurrent", "", num.ToString(), text);
								}
								else
								{
									this.filamentMessageFirst = true;
									this.filamentMessage90First = true;
								}
							}
						}
					}
				}
				this.listFilamentCurrent.Clear();
				this.listFilamentCurrent = new List<double>();
			}
			else
			{
				this.filamentMessageFirst = true;
			}
		}

		// Token: 0x06000FAC RID: 4012 RVA: 0x0005E340 File Offset: 0x0005C540
		private void addListEntryFilamentCurrent()
		{
			string s = this.parHandling.ReadPar("/hardware/xraysource/FilamentCurrent/Value").ToString(CultureInfo.InvariantCulture);
			double num = 0.0;
			double.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out num);
			num = Math.Round(num, 2);
			bool flag = num > 0.0;
			if (flag)
			{
				this.listFilamentCurrent.Add(num);
				string text = "";
				this.dbDatalogs.GetDataLogsSettingsEntry("MonitoringMessagesOnOff", ref text);
				bool flag2 = text.Equals("1");
				if (flag2)
				{
					string text2 = "";
					double num2 = 0.0;
					this.dbDatalogs.GetLimitsEntry("FilamentCurrent", ref text2);
					bool flag3 = !text2.Equals("-") && !text2.Equals("") && !text2.Equals(" ");
					if (flag3)
					{
						bool flag4 = double.TryParse(text2.Replace(',', '.'), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out num2);
						if (flag4)
						{
							bool flag5 = num2 < num && this.filamentMessageFirst;
							if (flag5)
							{
								text = "";
								this.dbDatalogs.GetDataLogsSettingsEntry("MessageLevel", ref text);
								bool flag6 = text.Equals("Info");
								SSTEntryCode code;
								if (flag6)
								{
									code = SSTEntryCode.LimitsInfo;
								}
								else
								{
									bool flag7 = text.Equals("Warning");
									if (flag7)
									{
										code = SSTEntryCode.LimitsWarning;
									}
									else
									{
										code = SSTEntryCode.LimitsError;
									}
								}
								this.errorSurvey.PostMessageMonitoring(code, 0, "txtFilamentCurrent", "", num2.ToString(), num.ToString());
								this.filamentMessageFirst = false;
							}
							else
							{
								bool flag8 = num2 * 0.9 <= num && this.filamentMessage90First && num2 >= num;
								if (flag8)
								{
									text = "";
									this.dbDatalogs.GetDataLogsSettingsEntry("MessageLevel90Perc", ref text);
									bool flag9 = text.Equals("Info");
									SSTEntryCode code2;
									if (flag9)
									{
										code2 = SSTEntryCode.Limits90PerInfo;
									}
									else
									{
										code2 = SSTEntryCode.Limits90PerWarning;
									}
									this.errorSurvey.PostMessageMonitoring(code2, 0, "txtFilamentCurrent", "", num2.ToString(), num.ToString());
									this.filamentMessageFirst = false;
								}
								else
								{
									bool flag10 = num2 > num;
									if (flag10)
									{
										this.filamentMessageFirst = true;
										bool flag11 = num2 * 0.9 > num;
										if (flag11)
										{
											this.filamentMessage90First = true;
										}
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06000FAD RID: 4013 RVA: 0x0005E5C4 File Offset: 0x0005C7C4
		private void SetDBTableMachineInfos()
		{
			List<string> list = new List<string>();
			List<string> list2 = new List<string>();
			string str = "/Value";
			bool flag = this.dbDatalogs.MachineInfosCheckInitValues();
			bool flag2 = flag || this.machineState.State == MachineStates.Inspecting;
			if (flag2)
			{
				string text = "StartDBDataLogs";
				list.Add(text);
				list2.Add(this.setDatalogsStartTime());
				text = "Machine";
				list.Add(text);
				list2.Add("");
				text = "/machine/MachineType";
				list.Add(text);
				list2.Add(this.valuesToStringMachineInfos("MachineType", this.parHandling.ReadPar(text + str)));
				text = "/machine/InspectionHeight";
				list.Add(text);
				list2.Add(this.parHandling.ReadPar(text + str));
				text = "/machine/InspectionWidth";
				list.Add(text);
				list2.Add(this.parHandling.ReadPar(text + str));
				text = "/machine/MachineSubType";
				list.Add(text);
				list2.Add(this.parHandling.ReadPar(text + str));
				text = "/machine/MachineName";
				list.Add(text);
				list2.Add(this.parHandling.ReadPar(text + str));
				text = "/machine/MachineSerialNumber";
				list.Add(text);
				list2.Add(this.parHandling.ReadPar(text + str));
				text = "/machine/LineName";
				list.Add(text);
				list2.Add(this.parHandling.ReadPar(text + str));
				text = "/machine/Location";
				list.Add(text);
				list2.Add(this.parHandling.ReadPar(text + str));
				text = "Application";
				list.Add(text);
				list2.Add("");
				text = "/application/ElinosVersion";
				list.Add(text);
				list2.Add(this.parHandling.ReadPar(text + str));
				text = "/application/GUIVersion";
				list.Add(text);
				list2.Add(this.parHandling.ReadPar(text + str));
				text = "/application/DatabaseVersion";
				list.Add(text);
				list2.Add(this.parHandling.ReadPar(text + str));
				text = "/application/Bitness";
				list.Add(text);
				list2.Add(this.parHandling.ReadPar(text + str));
				text = "/machine/ImageVersionApplication";
				list.Add(text);
				list2.Add(this.parHandling.ReadPar(text + str));
				text = "/machine/ImageVersionElinos";
				list.Add(text);
				list2.Add(this.parHandling.ReadPar(text + str));
				text = "/machine/ImageVersionWindows";
				list.Add(text);
				list2.Add(this.parHandling.ReadPar(text + str));
				text = "Source";
				list.Add(text);
				list2.Add("");
				text = "/hardware/xraysource/SourceType";
				list.Add(text);
				list2.Add(this.valuesToStringMachineInfos("SourceType", this.parHandling.ReadPar(text + str)));
				text = "/hardware/xraysource/ModelNumber";
				list.Add(text);
				list2.Add(this.parHandling.ReadPar(text + str));
				text = "/hardware/xraysource/SerialNumber";
				list.Add(text);
				list2.Add(this.parHandling.ReadPar(text + str));
				this.checkSourceOrDetectorChange(this.parHandling.ReadPar(text + str), true);
				text = "/hardware/xraysource/HardwareVersion";
				list.Add(text);
				list2.Add(this.parHandling.ReadPar(text + str));
				text = "/hardware/xraysource/SoftwareVersion";
				list.Add(text);
				list2.Add(this.parHandling.ReadPar(text + str));
				text = "/hardware/xraysource/FirmwareVersion";
				list.Add(text);
				list2.Add(this.parHandling.ReadPar(text + str));
				bool flag3 = flag;
				if (flag3)
				{
					text = "Detector";
					list.Add(text);
					list2.Add("");
					text = "/hardware/detector/Type";
					list.Add(text);
					list2.Add(this.valuesToStringMachineInfos("DetType", this.parHandling.ReadPar(text + str)));
					text = "/hardware/detector/sesotec/SubType";
					list.Add(text);
					list2.Add(this.valuesToStringMachineInfos("DetSubType", this.parHandling.ReadPar(text + str)));
					text = "/hardware/detector/sesotec/HardwareVersion";
					list.Add(text);
					list2.Add(this.parHandling.ReadPar(text + str));
					text = "/hardware/detector/sesotec/SoftwareVersion";
					list.Add(text);
					list2.Add(this.parHandling.ReadPar(text + str));
					text = "/hardware/detector/sesotec/MacAddress";
					list.Add(text);
					list2.Add(this.parHandling.ReadPar(text + str));
					this.checkSourceOrDetectorChange(this.parHandling.ReadPar(text + str), false);
					text = "/hardware/detector/sesotec/NumPixel";
					list.Add(text);
					list2.Add(this.parHandling.ReadPar(text + str));
					text = "/hardware/detector/sesotec/PixelSize";
					list.Add(text);
					list2.Add(this.parHandling.ReadPar(text + str));
					text = "/hardware/detector/sesotec/NumModules";
					list.Add(text);
					list2.Add(this.parHandling.ReadPar(text + str));
					text = "/hardware/detector/dt/Info";
					list.Add(text);
					list2.Add(this.parHandling.ReadPar(text + str));
					text = "/hardware/detector/dt/Serial";
					list.Add(text);
					list2.Add(this.parHandling.ReadPar(text + str));
					this.checkSourceOrDetectorChange(this.parHandling.ReadPar(text + str), false);
					text = "/hardware/detector/dt/NumPixel";
					list.Add(text);
					list2.Add(this.parHandling.ReadPar(text + str));
					text = "/hardware/detector/dt/PixelSize";
					list.Add(text);
					list2.Add(this.parHandling.ReadPar(text + str));
					text = "/hardware/detector/dt/NumModulesTotal";
					list.Add(text);
					list2.Add(this.parHandling.ReadPar(text + str));
				}
				text = "IOs";
				list.Add(text);
				list2.Add("");
				text = "/hardware/ios/MachineType";
				list.Add(text);
				list2.Add(this.valuesToStringMachineInfos("IOMachineType", this.parHandling.ReadPar(text + str)));
				text = "/hardware/ios/UserProgramVersion";
				list.Add(text);
				list2.Add(this.parHandling.ReadPar(text + str));
				text = "/hardware/ios/UserProgramSubVersion";
				list.Add(text);
				list2.Add(this.valuesToStringMachineInfos("IOUserProgramSubVersion", this.parHandling.ReadPar(text + str)));
				text = "/hardware/ios/UserProgramType";
				list.Add(text);
				list2.Add(this.valuesToStringMachineInfos("IOUserProgramType", this.parHandling.ReadPar(text + str)));
				text = "/hardware/ios/HWCode";
				list.Add(text);
				list2.Add(this.parHandling.ReadPar(text + str));
				text = "/hardware/ios/HWCode_SM";
				list.Add(text);
				list2.Add(this.parHandling.ReadPar(text + str));
				text = "/hardware/ios/SoftwareVersionIO";
				list.Add(text);
				list2.Add(this.parHandling.ReadPar(text + str));
				text = "/hardware/ios/SoftwareVersionIO_SM";
				list.Add(text);
				list2.Add(this.parHandling.ReadPar(text + str));
				text = "/hardware/ios/MacAddress";
				list.Add(text);
				list2.Add(this.parHandling.ReadPar(text + str));
				text = "/hardware/ios/MacAddress_SM";
				list.Add(text);
				list2.Add(this.parHandling.ReadPar(text + str));
				this.dbDatalogs.MachineInfosAddEntries(list, list2);
			}
		}

		// Token: 0x06000FAE RID: 4014 RVA: 0x0005EE20 File Offset: 0x0005D020
		private void writeDetectorMachineInfosWithoutXRay()
		{
			List<string> list = new List<string>();
			List<string> list2 = new List<string>();
			string str = "/Value";
			string text = "Detector";
			list.Add(text);
			list2.Add("");
			text = "/hardware/detector/Type";
			list.Add(text);
			list2.Add(this.valuesToStringMachineInfos("DetType", this.parHandling.ReadPar(text + str)));
			bool flag = this.parHandling.ReadPar(text + str) == "2";
			if (flag)
			{
				text = "/hardware/detector/sesotec/SubType";
				list.Add(text);
				list2.Add("n.a.");
				text = "/hardware/detector/sesotec/HardwareVersion";
				list.Add(text);
				list2.Add("n.a.");
				text = "/hardware/detector/sesotec/SoftwareVersion";
				list.Add(text);
				list2.Add("n.a.");
				text = "/hardware/detector/dt/Serial";
				list.Add(text);
				list2.Add(this.parHandling.ReadPar(text + str));
				this.checkSourceOrDetectorChange(this.parHandling.ReadPar(text + str), false);
				text = "/hardware/detector/dt/NumPixel";
				list.Add(text);
				list2.Add(this.parHandling.ReadPar(text + str));
				text = "/hardware/detector/dt/PixelSize";
				list.Add(text);
				list2.Add(this.parHandling.ReadPar(text + str));
				text = "/hardware/detector/sesotec/NumModules";
				list.Add(text);
				list2.Add("n.a.");
			}
			else
			{
				bool flag2 = this.parHandling.ReadPar(text + str) == "4";
				if (flag2)
				{
					text = "/hardware/detector/sesotec/SubType";
					list.Add(text);
					list2.Add("n.a.");
					text = "/hardware/detector/sesotec/HardwareVersion";
					list.Add(text);
					list2.Add("n.a.");
					text = "/hardware/detector/dt/Info";
					list.Add(text);
					list2.Add(this.parHandling.ReadPar(text + str));
					text = "/hardware/detector/dt/Serial";
					list.Add(text);
					list2.Add(this.parHandling.ReadPar(text + str));
					this.checkSourceOrDetectorChange(this.parHandling.ReadPar(text + str), false);
					text = "/hardware/detector/dt/NumPixel";
					list.Add(text);
					list2.Add(this.parHandling.ReadPar(text + str));
					text = "/hardware/detector/dt/PixelSize";
					list.Add(text);
					list2.Add(this.parHandling.ReadPar(text + str));
					text = "/hardware/detector/dt/NumModulesTotal";
					list.Add(text);
					list2.Add(this.parHandling.ReadPar(text + str));
				}
				else
				{
					text = "/hardware/detector/sesotec/SubType";
					list.Add(text);
					list2.Add(this.valuesToStringMachineInfos("DetSubType", this.parHandling.ReadPar(text + str)));
					text = "/hardware/detector/sesotec/HardwareVersion";
					list.Add(text);
					list2.Add(this.parHandling.ReadPar(text + str));
					text = "/hardware/detector/sesotec/SoftwareVersion";
					list.Add(text);
					list2.Add(this.parHandling.ReadPar(text + str));
					text = "/hardware/detector/sesotec/MacAddress";
					list.Add(text);
					list2.Add(this.parHandling.ReadPar(text + str));
					this.checkSourceOrDetectorChange(this.parHandling.ReadPar(text + str), false);
					text = "/hardware/detector/sesotec/NumPixel";
					list.Add(text);
					list2.Add(this.parHandling.ReadPar(text + str));
					text = "/hardware/detector/sesotec/PixelSize";
					list.Add(text);
					list2.Add(this.parHandling.ReadPar(text + str));
					text = "/hardware/detector/sesotec/NumModules";
					list.Add(text);
					list2.Add(this.parHandling.ReadPar(text + str));
				}
			}
			this.dbDatalogs.MachineInfosAddEntries(list, list2);
		}

		// Token: 0x06000FAF RID: 4015 RVA: 0x0005F238 File Offset: 0x0005D438
		private void checkSourceOrDetectorChange(string newvalue, bool isSource)
		{
			bool flag = newvalue != "" && newvalue != "0";
			if (flag)
			{
				string text = "";
				string text2 = "";
				this.dbDatalogs.MachineInfosGetSerialNumbers(ref text, ref text2);
				if (isSource)
				{
					bool flag2 = newvalue != text && text != "";
					if (flag2)
					{
						this.dbDatalogs.ResetLogsXRaySourceData(text);
					}
					text = newvalue;
				}
				else
				{
					bool flag3 = newvalue != text2 && text2 != "";
					if (flag3)
					{
						this.dbDatalogs.ResetLogsDetectorData(text2);
					}
					text2 = newvalue;
				}
			}
		}

		// Token: 0x06000FB0 RID: 4016 RVA: 0x0005F2F0 File Offset: 0x0005D4F0
		private string valuesToStringMachineInfos(string parameter, string value)
		{
			string result = "";
			bool flag = parameter == "MachineType";
			if (flag)
			{
				bool flag2 = value == "0";
				if (flag2)
				{
					result = "Raycon XRS";
				}
				else
				{
					bool flag3 = value == "1";
					if (flag3)
					{
						result = "Raycon D";
					}
					else
					{
						bool flag4 = value == "2";
						if (flag4)
						{
							result = "Raycon Entry";
						}
						else
						{
							bool flag5 = value == "3";
							if (flag5)
							{
								result = "Raycon D+";
							}
							else
							{
								result = value;
							}
						}
					}
				}
			}
			else
			{
				bool flag6 = parameter == "SourceType";
				if (flag6)
				{
					bool flag7 = value == "0";
					if (flag7)
					{
						result = "Spellman";
					}
					else
					{
						bool flag8 = value == "1";
						if (flag8)
						{
							result = "VJ";
						}
						else
						{
							result = value;
						}
					}
				}
				else
				{
					bool flag9 = parameter == "DetType";
					if (flag9)
					{
						bool flag10 = value == "0";
						if (flag10)
						{
							result = "DT";
						}
						else
						{
							bool flag11 = value == "1";
							if (flag11)
							{
								result = "Sesotec";
							}
							else
							{
								bool flag12 = value == "2";
								if (flag12)
								{
									result = "DT Dual Energy";
								}
								else
								{
									bool flag13 = value == "3";
									if (flag13)
									{
										result = "DT";
									}
									else
									{
										bool flag14 = value == "4";
										if (flag14)
										{
											result = "DTNetTDI";
										}
										else
										{
											result = value;
										}
									}
								}
							}
						}
					}
					else
					{
						bool flag15 = parameter == "DetSubType";
						if (flag15)
						{
							bool flag16 = value == "0";
							if (flag16)
							{
								result = "Standard";
							}
							else
							{
								bool flag17 = value == "1";
								if (flag17)
								{
									result = "HS";
								}
								else
								{
									result = value;
								}
							}
						}
						else
						{
							bool flag18 = parameter == "IOMachineType";
							if (flag18)
							{
								bool flag19 = value == "0";
								if (flag19)
								{
									result = "Development";
								}
								else
								{
									bool flag20 = value == "1";
									if (flag20)
									{
										result = "XRS,Raycon D";
									}
									else
									{
										bool flag21 = value == "2";
										if (flag21)
										{
											result = "Sideshooter";
										}
										else
										{
											bool flag22 = value == "3";
											if (flag22)
											{
												result = "Bulk XRS";
											}
											else
											{
												bool flag23 = value == "4";
												if (flag23)
												{
													result = "Entry";
												}
												else
												{
													bool flag24 = value == "5";
													if (flag24)
													{
														result = "Raycon D+";
													}
													else
													{
														result = value;
													}
												}
											}
										}
									}
								}
							}
							else
							{
								bool flag25 = parameter == "IOUserProgramSubVersion";
								if (flag25)
								{
									bool flag26 = value == "0";
									if (flag26)
									{
										result = "Development";
									}
									else
									{
										bool flag27 = value == "1";
										if (flag27)
										{
											result = "Default";
										}
										else
										{
											bool flag28 = value == "2";
											if (flag28)
											{
												result = "Advanced";
											}
											else
											{
												bool flag29 = value == "3";
												if (flag29)
												{
													result = "High level";
												}
												else
												{
													result = value;
												}
											}
										}
									}
								}
								else
								{
									bool flag30 = parameter == "IOUserProgramType";
									if (flag30)
									{
										bool flag31 = value == "0";
										if (flag31)
										{
											result = "Not defined";
										}
										else
										{
											bool flag32 = value == "1";
											if (flag32)
											{
												result = "Standard";
											}
											else
											{
												bool flag33 = value == "2";
												if (flag33)
												{
													result = "Custom";
												}
												else
												{
													result = value;
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

		// Token: 0x06000FB1 RID: 4017 RVA: 0x0005F644 File Offset: 0x0005D844
		private void SetDBTableOperatingCounter()
		{
			List<string> list = new List<string>();
			List<string> list2 = new List<string>();
			uint num = 0U;
			double num2 = 0.0;
			double num3 = 0.0;
			list.Add("MachineOperatingMinutes");
			this.dbDatalogs.OperatingCounterGetIntEntry("MachineOperatingMinutes", ref num);
			num += 1U;
			list2.Add(Convert.ToString(num));
			list.Add("MachineOperatingHours");
			num2 = Convert.ToDouble(num) / 60.0;
			bool flag = !this.dbDatalogs.OnOffEntryDoesExist(DateTime.Now.ToString("d", new CultureInfo("de-DE")));
			if (flag)
			{
				this.dbDatalogs.SetNewOnOffTimesEntry(DateTime.Now.ToString("d", new CultureInfo("de-DE")), "1", "0", "0", "0", "0", "0", "0", "0", "0");
			}
			else
			{
				this.dbDatalogs.UpdateOnOffTimes(DateTime.Now.ToString("d", new CultureInfo("de-DE")), "DeviceOnTimes", 1);
			}
			list2.Add(Convert.ToString(Math.Round(num2, 3)));
			num = 0U;
			bool flag2 = !ErrorSurvey.CommDetector;
			if (flag2)
			{
				string value = "";
				string value2 = "";
				string value3 = "";
				string value4 = "";
				this.writeDetectorMachineInfosWithoutXRay();
				bool flag3 = this.checkInitRenameDetectorTable;
				if (flag3)
				{
					string text = "";
					string text2 = "";
					this.dbDatalogs.MachineInfosGetSerialNumbers(ref text, ref text2);
					bool flag4 = text != "" && text2 != "";
					if (flag4)
					{
						text = text.Replace("-", "");
						text2 = text2.Replace(":", "");
						text2 = text2.Replace("-", "");
						this.dbDatalogs.RenameTables(text, text2);
						this.checkInitRenameDetectorTable = false;
					}
				}
				this.setDetectorOnTimes();
				this.dbDatalogs.DetectorOnTimesGetTimes(ref value, ref value2, ref value3, ref value4);
				list.Add("DetectorOperatingMinutesWithXRay");
				list2.Add(Convert.ToString(value2));
				list.Add("DetectorOperatingHoursWithXRay");
				list2.Add(Convert.ToString(value));
				list.Add("DetectorOperatingMinutesWithoutXRay");
				list2.Add(Convert.ToString(value4));
				list.Add("DetectorOperatingHoursWithoutXRay");
				list2.Add(Convert.ToString(value3));
			}
			bool flag5 = this.newValConveyorIsRunning;
			if (flag5)
			{
				list.Add("ConveyorOperatingMinutes");
				this.dbDatalogs.OperatingCounterGetIntEntry("ConveyorOperatingMinutes", ref num);
				num += 1U;
				list2.Add(Convert.ToString(num));
				list.Add("ConveyorOperatingHours");
				num2 = Convert.ToDouble(num) / 60.0;
				list2.Add(Convert.ToString(Math.Round(num2, 3)));
				num = 0U;
				bool flag6 = !this.dbDatalogs.OnOffEntryDoesExist(DateTime.Now.ToString("d", new CultureInfo("de-DE")));
				if (flag6)
				{
					this.dbDatalogs.SetNewOnOffTimesEntry(DateTime.Now.ToString("d", new CultureInfo("de-DE")), "0", "1", "0", "0", "0", "0", "0", "0", "0");
				}
				else
				{
					this.dbDatalogs.UpdateOnOffTimes(DateTime.Now.ToString("d", new CultureInfo("de-DE")), "ConveyorOnTimes", 1);
				}
			}
			bool flag7 = this.newValConveyorIsRunning;
			if (flag7)
			{
				list.Add("ConveyorDistanceMeter");
				double.TryParse(this.parHandling.ReadPar(Definition.conveyingSpeedPath), out num3);
				this.dbDatalogs.OperatingCounterGetDoubleEntry("ConveyorDistanceMeter", ref num2);
				num3 /= 1000.0;
				num3 *= 60.0;
				num3 += num2;
				num3 = Math.Round(num3, 3);
				list2.Add(Convert.ToString(num3));
			}
			bool flag8 = this.machineState.State == MachineStates.Inspecting;
			if (flag8)
			{
				list.Add("SourceOperatingHoursInternal");
				list2.Add(this.parHandling.ReadPar("/hardware/xraysource/XRayOnTimeInternal/Value"));
				list.Add("SourceOperatingHoursSesotec");
				list2.Add(this.parHandling.ReadPar("/machine/XRayOpHoursSesotec/Value"));
				List<string> list3 = new List<string>();
				list.Add("SourceOperatingOnCycles");
				string text3 = "SwitchOnCycles_" + this.parHandling.ReadPar("/hardware/xraysource/SerialNumber/Value");
				text3 = text3.Replace("-", "");
				MachineMode machineMode = MachineMode.Standard;
				this.dbConnection.GetMachineMode(ref machineMode);
				this.dbDatalogs.XRaySwitchOnGetEntries(text3, ref list3);
				list2.Add(Convert.ToString(list3.Count));
				bool flag9 = !this.dbDatalogs.OnOffEntryDoesExist(DateTime.Now.ToString("d", new CultureInfo("de-DE")));
				if (flag9)
				{
					this.dbDatalogs.SetNewOnOffTimesEntry(DateTime.Now.ToString("d", new CultureInfo("de-DE")), "0", "0", "1", "0", "0", "0", "0", "0", "0");
				}
				else
				{
					this.dbDatalogs.UpdateOnOffTimes(DateTime.Now.ToString("d", new CultureInfo("de-DE")), "XRayOnTimes", 1);
				}
				bool flag10 = this.parHandling.ReadPar(FlapsOverIOs.bulkflapsEnabledPath) == "1";
				if (flag10)
				{
					bool flag11 = this.parHandling.ReadPar(FlapsOverIOs.bulkflapsEnableValvesPath).Equals("1");
					if (flag11)
					{
						this.dbDatalogs.UpdateOnOffTimes(DateTime.Now.ToString("d", new CultureInfo("de-DE")), "SortingSwitchedOnTime", 1);
					}
				}
				else
				{
					bool flag12 = this.parHandling.ReadPar(SVT.svtEnablePath) == "1";
					if (flag12)
					{
						bool flag13 = this.parHandling.ReadPar(SVT.svtValvesEnablePath).Equals("1");
						if (flag13)
						{
							this.dbDatalogs.UpdateOnOffTimes(DateTime.Now.ToString("d", new CultureInfo("de-DE")), "SortingSwitchedOnTime", 1);
						}
					}
					else
					{
						bool flag14 = machineMode == MachineMode.BulkObject;
						if (flag14)
						{
							this.dbDatalogs.UpdateOnOffTimes(DateTime.Now.ToString("d", new CultureInfo("de-DE")), "SortingSwitchedOnTime", 1);
						}
					}
				}
			}
			this.dbDatalogs.OperatingCounterAddEntries(list, list2);
		}

		// Token: 0x06000FB2 RID: 4018 RVA: 0x0005FD64 File Offset: 0x0005DF64
		private void setDetectorOnTimes()
		{
			double a = 0.0;
			double value = 0.0;
			bool flag = false;
			List<string> list = new List<string>();
			List<string> list2 = new List<string>();
			List<string> list3 = new List<string>();
			List<string> list4 = new List<string>();
			double num = Math.Round(0.016666666666666666, 3);
			double.TryParse(this.parHandling.ReadPar("/hardware/xraysource/Voltage/Value"), out a);
			double.TryParse(this.parHandling.ReadPar("/hardware/xraysource/Current/Value"), out value);
			int num2 = Convert.ToInt32(Math.Round(a));
			double num3 = Math.Round(value, 1);
			this.dbDatalogs.DetectorOnTimesGetEntries("", ref list4, ref list, ref list2, ref list3, true);
			bool flag2 = list4.Count == 0;
			if (flag2)
			{
				this.dbDatalogs.DetectorOnTimesAddEntry(num2, num3, num, Convert.ToString(DateTime.Now));
			}
			else
			{
				for (int i = 0; i < list4.Count; i++)
				{
					int num4 = 0;
					int.TryParse(list[i], out num4);
					double num5 = 0.0;
					double.TryParse(list2[i], out num5);
					double num6 = 0.0;
					double.TryParse(list3[i], out num6);
					bool flag3 = num2 == num4 && num3 == num5 && !flag;
					if (flag3)
					{
						this.dbDatalogs.DetectorOnTimesSummEntry(i + 1, num6 + num);
						flag = true;
					}
					bool flag4 = i == list4.Count - 1 && !flag && num3 != 0.0 && num2 != 0;
					if (flag4)
					{
						this.dbDatalogs.DetectorOnTimesAddEntry(num2, num3, num, "");
					}
				}
			}
		}

		// Token: 0x06000FB3 RID: 4019 RVA: 0x0005FF38 File Offset: 0x0005E138
		private void SetValuesForSwitchOnCycles()
		{
			List<string> list = new List<string>();
			List<string> list2 = new List<string>();
			uint num = 0U;
			this.oldValConveyorIsRunning = this.newValConveyorIsRunning;
			this.newValConveyorIsRunning = (this.parHandling.ReadPar(Definition.conveyorIsRunning) == "1");
			this.oldValSourceIsRunning = this.newValSourceIsRunning;
			this.newValSourceIsRunning = (this.parHandling.ReadPar(XrayState.xrayPowerOnPath) == "1");
			bool flag = this.newValMachineOnCycle;
			if (flag)
			{
				list.Add("MachineOperatingOnCycles");
				this.dbDatalogs.OperatingCounterGetIntEntry("MachineOperatingOnCycles", ref num);
				num += 1U;
				list2.Add(Convert.ToString(num));
				num = 0U;
				this.newValMachineOnCycle = false;
			}
			bool flag2 = this.newValConveyorIsRunning && !this.oldValConveyorIsRunning;
			if (flag2)
			{
				list.Add("ConveyorOperatingOnCycles");
				this.dbDatalogs.OperatingCounterGetIntEntry("ConveyorOperatingOnCycles", ref num);
				num += 1U;
				list2.Add(Convert.ToString(num));
				num = 0U;
			}
			bool flag3 = this.newValSourceIsRunning && !this.oldValSourceIsRunning;
			if (flag3)
			{
				this.dbDatalogs.OperatingCounterGetIntEntry("SourceOperatingOnCycles", ref num);
				num += 1U;
				this.dbDatalogs.XRaySwitchOnCyclesAddEntry();
				num = 0U;
			}
			this.dbDatalogs.OperatingCounterAddEntries(list, list2);
		}

		// Token: 0x06000FB4 RID: 4020 RVA: 0x0006009C File Offset: 0x0005E29C
		private string setDatalogsStartTime()
		{
			string text = "";
			bool flag = this.dbDatalogs.MachineInfosGetStartTimeDBDatalogs(ref text) && text == "";
			string result;
			if (flag)
			{
				result = DateTime.Now.ToString(new CultureInfo("de-DE"));
			}
			else
			{
				result = text;
			}
			return result;
		}

		// Token: 0x06000FB5 RID: 4021 RVA: 0x000600F0 File Offset: 0x0005E2F0
		private void CheckValues()
		{
			uint num = 0U;
			int num2 = 0;
			double num3 = 0.0;
			double num4 = 0.0;
			List<string> source = new List<string>();
			List<string> list = new List<string>();
			List<string> list2 = new List<string>();
			List<string> list3 = new List<string>();
			string text = "";
			string text2 = "";
			this.dbDatalogs.GetDataLogsSettingsEntry("MessageLevel", ref text2);
			bool flag = text2.Equals("Info");
			SSTEntryCode code;
			if (flag)
			{
				code = SSTEntryCode.LimitsInfo;
			}
			else
			{
				bool flag2 = text2.Equals("Warning");
				if (flag2)
				{
					code = SSTEntryCode.LimitsWarning;
				}
				else
				{
					code = SSTEntryCode.LimitsError;
				}
			}
			text2 = "";
			this.dbDatalogs.GetDataLogsSettingsEntry("MessageLevel90Perc", ref text2);
			bool flag3 = text2.Equals("Info");
			SSTEntryCode code2;
			if (flag3)
			{
				code2 = SSTEntryCode.Limits90PerInfo;
			}
			else
			{
				code2 = SSTEntryCode.Limits90PerWarning;
			}
			this.dbDatalogs.OperatingCounterGetDoubleEntry("MachineOperatingHours", ref num4);
			this.dbDatalogs.GetLimitsEntry("OperatingHours", ref text);
			bool flag4 = !text.Equals("-") && !text.Equals("") && !text.Equals(" ");
			if (flag4)
			{
				bool flag5 = double.TryParse(text.Replace(',', '.'), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out num3);
				if (flag5)
				{
					bool flag6 = num3 < num4;
					if (flag6)
					{
						this.errorSurvey.PostMessageMonitoring(code, 0, "txtOperatingHours", "txtMachine", num3.ToString(), num4.ToString());
					}
					else
					{
						bool flag7 = num3 * 0.9 <= num4;
						if (flag7)
						{
							this.errorSurvey.PostMessageMonitoring(code2, 0, "txtOperatingHours", "txtMachine", num3.ToString(), num4.ToString());
						}
					}
				}
			}
			num4 = 0.0;
			text = "";
			num3 = 0.0;
			this.dbDatalogs.GetLimitsEntry("OperatingHoursSource", ref text);
			this.dbDatalogs.OperatingCounterGetDoubleEntry("SourceOperatingHoursSesotec", ref num4);
			bool flag8 = !text.Equals("-") && !text.Equals("") && !text.Equals(" ");
			if (flag8)
			{
				bool flag9 = double.TryParse(text.Replace(',', '.'), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out num3);
				if (flag9)
				{
					bool flag10 = num3 < num4;
					if (flag10)
					{
						this.errorSurvey.PostMessageMonitoring(code, 0, "txtOperatingHours", "parItemXRaySource", num3.ToString(), num4.ToString());
					}
					else
					{
						bool flag11 = num3 * 0.9 <= num4;
						if (flag11)
						{
							this.errorSurvey.PostMessageMonitoring(code2, 0, "txtOperatingHours", "parItemXRaySource", num3.ToString(), num4.ToString());
						}
					}
				}
			}
			text = "";
			num3 = 0.0;
			num4 = 0.0;
			this.dbDatalogs.GetLimitsEntry("OperatingHoursDetector", ref text);
			this.dbDatalogs.OperatingCounterGetDoubleEntry("DetectorOperatingHoursWithXRay", ref num4);
			bool flag12 = !text.Equals("-") && !text.Equals("") && !text.Equals(" ");
			if (flag12)
			{
				bool flag13 = double.TryParse(text.Replace(',', '.'), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out num3);
				if (flag13)
				{
					bool flag14 = num3 < num4;
					if (flag14)
					{
						this.errorSurvey.PostMessageMonitoring(code, 0, "txtOperatingHours", "parItemDetector", num3.ToString(), num4.ToString());
					}
					else
					{
						bool flag15 = num3 * 0.9 <= num4;
						if (flag15)
						{
							this.errorSurvey.PostMessageMonitoring(code2, 0, "txtOperatingHours", "parItemDetector", num3.ToString(), num4.ToString());
						}
					}
				}
			}
			text = "";
			num3 = 0.0;
			num4 = 0.0;
			this.dbDatalogs.GetLimitsEntry("OperatingHoursConveyor", ref text);
			this.dbDatalogs.OperatingCounterGetDoubleEntry("ConveyorOperatingHours", ref num4);
			bool flag16 = !text.Equals("-") && !text.Equals("") && !text.Equals(" ");
			if (flag16)
			{
				bool flag17 = double.TryParse(text.Replace(',', '.'), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out num3);
				if (flag17)
				{
					bool flag18 = num3 < num4;
					if (flag18)
					{
						this.errorSurvey.PostMessageMonitoring(code, 0, "txtOperatingHours", "txtConveyor", num3.ToString(), num4.ToString());
					}
					else
					{
						bool flag19 = num3 * 0.9 <= num4;
						if (flag19)
						{
							this.errorSurvey.PostMessageMonitoring(code2, 0, "txtOperatingHours", "", num3.ToString(), num4.ToString());
						}
					}
				}
			}
			text = "";
			num3 = 0.0;
			num4 = 0.0;
			this.dbDatalogs.GetLimitsEntry("OperatingDistance", ref text);
			this.dbDatalogs.OperatingCounterGetDoubleEntry("ConveyorDistanceMeter", ref num4);
			bool flag20 = !text.Equals("-") && !text.Equals("") && !text.Equals(" ");
			if (flag20)
			{
				bool flag21 = double.TryParse(text.Replace(',', '.'), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out num3);
				if (flag21)
				{
					bool flag22 = num3 < num4 / 1000.0;
					if (flag22)
					{
						this.errorSurvey.PostMessageMonitoring(code, 0, "txtConveyorDistance", "", num3.ToString(), Math.Round(num4 / 1000.0, 2).ToString());
					}
					else
					{
						bool flag23 = num3 * 0.9 <= num4 / 1000.0;
						if (flag23)
						{
							this.errorSurvey.PostMessageMonitoring(code2, 0, "txtConveyorDistance", "", num3.ToString(), Math.Round(num4 / 1000.0, 2).ToString());
						}
					}
				}
			}
			text = "";
			num3 = 0.0;
			num4 = 0.0;
			this.dbConnection.GetParameterValue(SVT.svtEnablePath, ref text);
			bool flag24 = !text.Equals("1");
			if (flag24)
			{
				this.dbDatalogs.OperatingCounterGetIntEntry("RejectIndex", ref num);
				this.dbDatalogs.GetLimitsEntry("Reject", ref text);
				bool flag25 = !text.Equals("-") && !text.Equals("") && !text.Equals(" ");
				if (flag25)
				{
					bool flag26 = int.TryParse(text, out num2);
					if (flag26)
					{
						bool flag27 = (long)num2 < (long)((ulong)num);
						if (flag27)
						{
							this.errorSurvey.PostMessageMonitoring(code, 0, "txtProductCounterBad", "", num2.ToString(), num.ToString());
						}
						else
						{
							bool flag28 = (double)num2 * 0.9 <= num;
							if (flag28)
							{
								this.errorSurvey.PostMessageMonitoring(code2, 0, "txtProductCounterBad", "", num2.ToString(), num.ToString());
							}
						}
					}
				}
				uint num5 = num;
				num = 0U;
				this.dbDatalogs.OperatingCounterGetIntEntry("ThroughputIndex", ref num);
				this.dbDatalogs.GetLimitsEntry("Throughput", ref text);
				bool flag29 = !text.Equals("-") && !text.Equals("") && !text.Equals(" ");
				if (flag29)
				{
					bool flag30 = int.TryParse(text, out num2);
					if (flag30)
					{
						bool flag31 = (long)num2 < (long)((ulong)(num - num5));
						if (flag31)
						{
							this.errorSurvey.PostMessageMonitoring(code, 0, "txtProductCounterGood", "", num2.ToString(), (num - num5).ToString());
						}
						else
						{
							bool flag32 = (double)num2 * 0.9 <= num - num5;
							if (flag32)
							{
								this.errorSurvey.PostMessageMonitoring(code2, 0, "txtProductCounterGood", "", num2.ToString(), (num - num5).ToString());
							}
						}
					}
				}
				num = 0U;
				this.dbDatalogs.GetLimitsEntry("Rejecter4", ref text);
				this.dbDatalogs.OperatingCounterGetIntEntry("CounterRejecter4", ref num);
				bool flag33 = !text.Equals("-") && !text.Equals("") && !text.Equals(" ");
				if (flag33)
				{
					bool flag34 = int.TryParse(text, out num2);
					if (flag34)
					{
						bool flag35 = (long)num2 < (long)((ulong)num);
						if (flag35)
						{
							this.errorSurvey.PostMessageMonitoring(code, 4, "txtRejecter", "", num2.ToString(), num.ToString());
						}
						else
						{
							bool flag36 = (double)num2 * 0.9 <= num;
							if (flag36)
							{
								this.errorSurvey.PostMessageMonitoring(code2, 4, "txtRejecter", "", num2.ToString(), num.ToString());
							}
						}
					}
				}
				num = 0U;
				this.dbDatalogs.GetLimitsEntry("Rejecter1", ref text);
				this.dbDatalogs.OperatingCounterGetIntEntry("CounterRejecter1", ref num);
				bool flag37 = !text.Equals("-") && !text.Equals("") && !text.Equals(" ");
				if (flag37)
				{
					bool flag38 = int.TryParse(text, out num2);
					if (flag38)
					{
						bool flag39 = (long)num2 < (long)((ulong)num);
						if (flag39)
						{
							this.errorSurvey.PostMessageMonitoring(code, 1, "txtRejecter", "", num2.ToString(), num.ToString());
						}
						else
						{
							bool flag40 = (double)num2 * 0.9 <= num;
							if (flag40)
							{
								this.errorSurvey.PostMessageMonitoring(code2, 1, "txtRejecter", "", num2.ToString(), num.ToString());
							}
						}
					}
				}
				num = 0U;
				this.dbDatalogs.GetLimitsEntry("Rejecter2", ref text);
				this.dbDatalogs.OperatingCounterGetIntEntry("CounterRejecter2", ref num);
				bool flag41 = !text.Equals("-") && !text.Equals("") && !text.Equals(" ");
				if (flag41)
				{
					bool flag42 = int.TryParse(text, out num2);
					if (flag42)
					{
						bool flag43 = (long)num2 < (long)((ulong)num);
						if (flag43)
						{
							this.errorSurvey.PostMessageMonitoring(code, 2, "txtRejecter", "", num2.ToString(), num.ToString());
						}
						else
						{
							bool flag44 = (double)num2 * 0.9 <= num;
							if (flag44)
							{
								this.errorSurvey.PostMessageMonitoring(code2, 2, "txtRejecter", "", num2.ToString(), num.ToString());
							}
						}
					}
				}
				num = 0U;
				this.dbDatalogs.GetLimitsEntry("Rejecter3", ref text);
				this.dbDatalogs.OperatingCounterGetIntEntry("CounterRejecter3", ref num);
				bool flag45 = !text.Equals("-") && !text.Equals("") && !text.Equals(" ");
				if (flag45)
				{
					bool flag46 = int.TryParse(text, out num2);
					if (flag46)
					{
						bool flag47 = (long)num2 < (long)((ulong)num);
						if (flag47)
						{
							this.errorSurvey.PostMessageMonitoring(code, 3, "txtRejecter", "", num2.ToString(), num.ToString());
						}
						else
						{
							bool flag48 = (double)num2 * 0.9 <= num;
							if (flag48)
							{
								this.errorSurvey.PostMessageMonitoring(code2, 3, "txtRejecter", "", num2.ToString(), num.ToString());
							}
						}
					}
				}
			}
			else
			{
				text = "";
				num2 = 0;
				int num6 = 0;
				string s = "";
				this.dbConnection.GetNumValves(ref num6);
				uint num7 = 0U;
				bool flag49 = false;
				this.dbDatalogs.GetLimitsEntry("Rejecter1", ref text);
				bool flag50 = !text.Equals("-") && !text.Equals("") && !text.Equals(" ");
				if (flag50)
				{
					bool flag51 = int.TryParse(text, out num2);
					if (flag51)
					{
						for (int i = 1; i <= num6; i++)
						{
							this.dbDatalogs.OperatingCounterGetIntEntry("CounterRejecter" + i.ToString(), ref num);
							num7 += num;
							flag49 = true;
							bool flag52 = (long)num2 < (long)((ulong)num);
							if (flag52)
							{
								this.errorSurvey.PostMessageMonitoring(code, (byte)i, "txtValve", "", num2.ToString(), num.ToString());
							}
							else
							{
								bool flag53 = (double)num2 * 0.9 <= num;
								if (flag53)
								{
									this.errorSurvey.PostMessageMonitoring(code2, (byte)i, "txtValve", "", num2.ToString(), num.ToString());
								}
							}
						}
					}
				}
				this.dbDatalogs.GetLimitsEntry("Rejecter2", ref text);
				bool flag54 = !text.Equals("-") && !text.Equals("") && !text.Equals(" ");
				if (flag54)
				{
					bool flag55 = int.TryParse(text, out num2);
					if (flag55)
					{
						bool flag56 = !flag49;
						if (flag56)
						{
							for (int j = 1; j <= num6; j++)
							{
								this.dbDatalogs.OnOffTimesGetEntryOfDate(DateTime.Now.ToString("d", new CultureInfo("de-DE")), ref s, "SVTValve" + j.ToString());
								bool flag57 = uint.TryParse(s, out num);
								if (flag57)
								{
									num7 += num;
								}
							}
						}
						bool flag58 = (long)num2 < (long)((ulong)num7);
						if (flag58)
						{
							this.errorSurvey.PostMessageMonitoring(code, 0, "txtSumValves", "", num2.ToString(), num7.ToString());
						}
						else
						{
							bool flag59 = (double)num2 * 0.9 <= num7;
							if (flag59)
							{
								this.errorSurvey.PostMessageMonitoring(code2, 0, "txtSumValves", "", num2.ToString(), num7.ToString());
							}
						}
					}
				}
			}
			num = 0U;
			this.dbDatalogs.GetLimitsEntry("SwitchOnCycles", ref text2);
			this.dbDatalogs.OperatingCounterGetIntEntry("SourceOperatingOnCycles", ref num);
			bool flag60 = int.TryParse(text2.Replace(',', '.'), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out num2);
			if (flag60)
			{
				bool flag61 = (long)num2 < (long)((ulong)num);
				if (flag61)
				{
					this.errorSurvey.PostMessageMonitoring(code, 0, "txtSwitchOnCycles", "", num2.ToString(), num.ToString());
				}
				else
				{
					bool flag62 = (double)num2 * 0.9 <= num;
					if (flag62)
					{
						this.errorSurvey.PostMessageMonitoring(code2, 0, "txtSwitchOnCycles", "", num2.ToString(), num.ToString());
					}
				}
			}
			string text3 = "";
			string text4 = "";
			this.dbDatalogs.MachineInfosGetSerialNumbers(ref text3, ref text4);
			string text5 = "Arcs";
			num2 = 0;
			list2.Clear();
			list.Clear();
			int num8 = 0;
			bool flag63 = text3.Length > 0;
			if (flag63)
			{
				text5 = text5 + "_" + text3.Replace("-", "");
				this.dbDatalogs.GetLimitsEntry("ArcsDaily", ref text2);
				this.dbDatalogs.ArcsGetNumberToday(text5, ref num8);
				bool flag64 = int.TryParse(text2.Replace(',', '.'), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out num2);
				if (flag64)
				{
					bool flag65 = num2 < num8;
					if (flag65)
					{
						this.errorSurvey.PostMessageMonitoring(code, 0, "txtArcs", "txtDaily", num2.ToString(), num8.ToString());
					}
					else
					{
						bool flag66 = (double)num2 * 0.9 <= (double)num8;
						if (flag66)
						{
							this.errorSurvey.PostMessageMonitoring(code2, 0, "txtArcs", "txtDaily", num2.ToString(), num8.ToString());
						}
					}
				}
				this.dbDatalogs.GetLimitsEntry("Arcs", ref text2);
				this.dbDatalogs.ArcsGetEntriesType0(text5, ref source, ref list, ref list2);
				bool flag67 = int.TryParse(text2.Replace(',', '.'), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out num2);
				if (flag67)
				{
					bool flag68 = num2 < source.Count<string>();
					if (flag68)
					{
						this.errorSurvey.PostMessageMonitoring(code, 0, "txtArcs", "", num2.ToString(), source.Count<string>().ToString());
					}
					else
					{
						bool flag69 = (double)num2 * 0.9 < (double)source.Count<string>();
						if (flag69)
						{
							this.errorSurvey.PostMessageMonitoring(code2, 0, "txtArcs", "", num2.ToString(), source.Count<string>().ToString());
						}
					}
				}
			}
		}

		// Token: 0x06000FB6 RID: 4022 RVA: 0x000612B4 File Offset: 0x0005F4B4
		public void CheckValuesHourly()
		{
			uint num = 0U;
			int num2 = 0;
			List<string> list = new List<string>();
			List<string> list2 = new List<string>();
			List<string> list3 = new List<string>();
			List<string> list4 = new List<string>();
			string text = "";
			string s = "";
			string text2 = "";
			this.dbDatalogs.GetDataLogsSettingsEntry("MessageLevel", ref text2);
			bool flag = text2.Equals("Info");
			SSTEntryCode code;
			if (flag)
			{
				code = SSTEntryCode.LimitsInfo;
			}
			else
			{
				bool flag2 = text2.Equals("Warning");
				if (flag2)
				{
					code = SSTEntryCode.LimitsWarning;
				}
				else
				{
					code = SSTEntryCode.LimitsError;
				}
			}
			text2 = "";
			this.dbDatalogs.GetDataLogsSettingsEntry("MessageLevel90Perc", ref text2);
			bool flag3 = text2.Equals("Info");
			SSTEntryCode code2;
			if (flag3)
			{
				code2 = SSTEntryCode.Limits90PerInfo;
			}
			else
			{
				code2 = SSTEntryCode.Limits90PerWarning;
			}
			this.dbConnection.GetParameterValue(SVT.svtEnablePath, ref text);
			bool flag4 = text.Equals("1");
			if (flag4)
			{
				int num3 = 0;
				this.dbConnection.GetNumValves(ref num3);
				uint num4 = 0U;
				bool flag5 = false;
				this.dbDatalogs.GetLimitsEntry("Rejecter1PerDay", ref text);
				bool flag6 = !text.Equals("-") && !text.Equals("") && !text.Equals(" ");
				if (flag6)
				{
					bool flag7 = int.TryParse(text, out num2);
					if (flag7)
					{
						for (int i = 1; i <= num3; i++)
						{
							this.dbDatalogs.OnOffTimesGetEntryOfDate(DateTime.Now.ToString("d", new CultureInfo("de-DE")), ref s, "SVTValve" + i.ToString());
							bool flag8 = uint.TryParse(s, out num);
							if (flag8)
							{
								num4 += num;
								flag5 = true;
								bool flag9 = (long)num2 < (long)((ulong)num);
								if (flag9)
								{
									this.errorSurvey.PostMessageMonitoring(code, (byte)i, "txtValve", "txtPerDay", num2.ToString(), num.ToString());
								}
								else
								{
									bool flag10 = (double)num2 * 0.9 <= num;
									if (flag10)
									{
										this.errorSurvey.PostMessageMonitoring(code2, (byte)i, "txtValve", "txtPerDay", num2.ToString(), num.ToString());
									}
								}
							}
						}
					}
				}
				this.dbDatalogs.GetLimitsEntry("Rejecter2PerDay", ref text);
				bool flag11 = !text.Equals("-") && !text.Equals("") && !text.Equals(" ");
				if (flag11)
				{
					bool flag12 = int.TryParse(text, out num2);
					if (flag12)
					{
						bool flag13 = !flag5;
						if (flag13)
						{
							for (int j = 1; j <= num3; j++)
							{
								this.dbDatalogs.OnOffTimesGetEntryOfDate(DateTime.Now.ToString("d", new CultureInfo("de-DE")), ref s, "SVTValve" + j.ToString());
								bool flag14 = uint.TryParse(s, out num);
								if (flag14)
								{
									num4 += num;
								}
							}
						}
						bool flag15 = (long)num2 < (long)((ulong)num4);
						if (flag15)
						{
							this.errorSurvey.PostMessageMonitoring(code, 0, "txtSumValves", "txtPerDay", num2.ToString(), num4.ToString());
						}
						else
						{
							bool flag16 = (double)num2 * 0.9 <= num4;
							if (flag16)
							{
								this.errorSurvey.PostMessageMonitoring(code2, 0, "txtSumValves", "txtPerDay", num2.ToString(), num4.ToString());
							}
						}
					}
				}
			}
			else
			{
				this.dbDatalogs.GetLimitsEntry("Rejecter1PerDay", ref text);
				this.dbDatalogs.OnOffTimesGetEntryOfDate(DateTime.Now.ToString("d", new CultureInfo("de-DE")), ref s, "CounterRejecter1");
				bool flag17 = uint.TryParse(s, out num);
				if (flag17)
				{
					bool flag18 = !text.Equals("-") && !text.Equals("") && !text.Equals(" ");
					if (flag18)
					{
						bool flag19 = int.TryParse(text, out num2);
						if (flag19)
						{
							bool flag20 = (long)num2 < (long)((ulong)num);
							if (flag20)
							{
								this.errorSurvey.PostMessageMonitoring(code, 1, "txtRejecter", "txtPerDay", num2.ToString(), num.ToString());
							}
							else
							{
								bool flag21 = (double)num2 * 0.9 <= num;
								if (flag21)
								{
									this.errorSurvey.PostMessageMonitoring(code2, 1, "txtRejecter", "txtPerDay", num2.ToString(), num.ToString());
								}
							}
						}
					}
				}
				s = "";
				this.dbDatalogs.GetLimitsEntry("Rejecter2PerDay", ref text);
				this.dbDatalogs.OnOffTimesGetEntryOfDate(DateTime.Now.ToString("d", new CultureInfo("de-DE")), ref s, "CounterRejecter2");
				bool flag22 = uint.TryParse(s, out num);
				if (flag22)
				{
					bool flag23 = !text.Equals("-") && !text.Equals("") && !text.Equals(" ");
					if (flag23)
					{
						bool flag24 = int.TryParse(text, out num2);
						if (flag24)
						{
							bool flag25 = (long)num2 < (long)((ulong)num);
							if (flag25)
							{
								this.errorSurvey.PostMessageMonitoring(code, 2, "txtRejecter", "txtPerDay", num2.ToString(), num.ToString());
							}
							else
							{
								bool flag26 = (double)num2 * 0.9 <= num;
								if (flag26)
								{
									this.errorSurvey.PostMessageMonitoring(code2, 2, "txtRejecter", "txtPerDay", num2.ToString(), num.ToString());
								}
							}
						}
					}
				}
				s = "";
				this.dbDatalogs.GetLimitsEntry("Rejecter3PerDay", ref text);
				this.dbDatalogs.OnOffTimesGetEntryOfDate(DateTime.Now.ToString("d", new CultureInfo("de-DE")), ref s, "CounterRejecter3");
				bool flag27 = uint.TryParse(s, out num);
				if (flag27)
				{
					bool flag28 = !text.Equals("-") && !text.Equals("") && !text.Equals(" ");
					if (flag28)
					{
						bool flag29 = int.TryParse(text, out num2);
						if (flag29)
						{
							bool flag30 = (long)num2 < (long)((ulong)num);
							if (flag30)
							{
								this.errorSurvey.PostMessageMonitoring(code, 3, "txtRejecter", "txtPerDay", num2.ToString(), num.ToString());
							}
							else
							{
								bool flag31 = (double)num2 * 0.9 <= num;
								if (flag31)
								{
									this.errorSurvey.PostMessageMonitoring(code2, 3, "txtRejecter", "txtPerDay", num2.ToString(), num.ToString());
								}
							}
						}
					}
				}
				s = "";
				this.dbDatalogs.GetLimitsEntry("Rejecter4PerDay", ref text);
				this.dbDatalogs.OnOffTimesGetEntryOfDate(DateTime.Now.ToString("d", new CultureInfo("de-DE")), ref s, "CounterRejecter4");
				bool flag32 = uint.TryParse(s, out num);
				if (flag32)
				{
					bool flag33 = !text.Equals("-") && !text.Equals("") && !text.Equals(" ");
					if (flag33)
					{
						bool flag34 = int.TryParse(text, out num2);
						if (flag34)
						{
							bool flag35 = (long)num2 < (long)((ulong)num);
							if (flag35)
							{
								this.errorSurvey.PostMessageMonitoring(code, 4, "txtRejecter", "txtPerDay", num2.ToString(), num.ToString());
							}
							else
							{
								bool flag36 = (double)num2 * 0.9 <= num;
								if (flag36)
								{
									this.errorSurvey.PostMessageMonitoring(code2, 4, "txtRejecter", "txtPerDay", num2.ToString(), num.ToString());
								}
							}
						}
					}
				}
			}
			s = "";
			this.dbDatalogs.GetLimitsEntry("RejectPerDay", ref text);
			this.dbDatalogs.OnOffTimesGetEntryOfDate(DateTime.Now.ToString("d", new CultureInfo("de-DE")), ref s, "Reject");
			bool flag37 = uint.TryParse(s, out num);
			if (flag37)
			{
				bool flag38 = !text.Equals("-") && !text.Equals("") && !text.Equals(" ");
				if (flag38)
				{
					bool flag39 = int.TryParse(text, out num2);
					if (flag39)
					{
						bool flag40 = (long)num2 < (long)((ulong)num);
						if (flag40)
						{
							this.errorSurvey.PostMessageMonitoring(code, 0, "txtProductCounterBad", "txtPerDay", num2.ToString(), num.ToString());
						}
						else
						{
							bool flag41 = (double)num2 * 0.9 <= num;
							if (flag41)
							{
								this.errorSurvey.PostMessageMonitoring(code2, 0, "txtProductCounterBad", "txtPerDay", num2.ToString(), num.ToString());
							}
						}
					}
				}
			}
			s = "";
			this.dbDatalogs.GetLimitsEntry("ThroughputPerDay", ref text);
			this.dbDatalogs.OnOffTimesGetEntryOfDate(DateTime.Now.ToString("d", new CultureInfo("de-DE")), ref s, "Throughput");
			bool flag42 = uint.TryParse(s, out num);
			if (flag42)
			{
				bool flag43 = !text.Equals("-") && !text.Equals("") && !text.Equals(" ");
				if (flag43)
				{
					bool flag44 = int.TryParse(text, out num2);
					if (flag44)
					{
						bool flag45 = (long)num2 < (long)((ulong)num);
						if (flag45)
						{
							this.errorSurvey.PostMessageMonitoring(code, 0, "txtProductCounterGood", "txtPerDay", num2.ToString(), num.ToString());
						}
						else
						{
							bool flag46 = (double)num2 * 0.9 <= num;
							if (flag46)
							{
								this.errorSurvey.PostMessageMonitoring(code2, 0, "txtProductCounterGood", "txtPerDay", num2.ToString(), num.ToString());
							}
						}
					}
				}
			}
			s = "";
		}

		// Token: 0x04000A76 RID: 2678
		private DBDataLogs dbDatalogs;

		// Token: 0x04000A77 RID: 2679
		private ParameterHandling parHandling;

		// Token: 0x04000A78 RID: 2680
		private List<double> listFilamentCurrent = new List<double>();

		// Token: 0x04000A79 RID: 2681
		private MachineState machineState;

		// Token: 0x04000A7A RID: 2682
		private bool newValMachineOnCycle = true;

		// Token: 0x04000A7B RID: 2683
		private bool oldValConveyorIsRunning = false;

		// Token: 0x04000A7C RID: 2684
		private bool newValConveyorIsRunning = false;

		// Token: 0x04000A7D RID: 2685
		private bool oldValSourceIsRunning = false;

		// Token: 0x04000A7E RID: 2686
		private bool newValSourceIsRunning = false;

		// Token: 0x04000A7F RID: 2687
		private DispatcherTimer timerDataLogsSeconds;

		// Token: 0x04000A80 RID: 2688
		private DispatcherTimer timerDataLogsMinutes;

		// Token: 0x04000A81 RID: 2689
		private DispatcherTimer timerDataLogsHours;

		// Token: 0x04000A82 RID: 2690
		private bool isHour;

		// Token: 0x04000A83 RID: 2691
		private bool checkInitRenameDetectorTable = true;

		// Token: 0x04000A84 RID: 2692
		private ErrorSurvey errorSurvey;

		// Token: 0x04000A85 RID: 2693
		private bool filamentMessageFirst = true;

		// Token: 0x04000A86 RID: 2694
		private bool filamentMessage90First = true;

		// Token: 0x04000A87 RID: 2695
		private int countRepeatMessage = 0;

		// Token: 0x04000A88 RID: 2696
		private bool initialized = false;

		// Token: 0x04000A89 RID: 2697
		private DBConnection dbConnection;

		// Token: 0x04000A8A RID: 2698
		private bool versionInitalized = false;
	}
}
