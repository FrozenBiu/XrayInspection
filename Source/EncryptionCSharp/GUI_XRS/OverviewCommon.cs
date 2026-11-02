using System;
using System.Collections.Generic;
using System.Linq;
using Definitions;
using GUI_XRS.Datalogger;

namespace GUI_XRS
{
	// Token: 0x0200012B RID: 299
	public class OverviewCommon
	{
		// Token: 0x17000879 RID: 2169
		// (get) Token: 0x06001CE8 RID: 7400 RVA: 0x000BABB4 File Offset: 0x000B8DB4
		// (set) Token: 0x06001CE9 RID: 7401 RVA: 0x000BABBB File Offset: 0x000B8DBB
		public static bool OperatingHourVisibility { get; set; }

		// Token: 0x06001CEA RID: 7402 RVA: 0x000BABC3 File Offset: 0x000B8DC3
		public OverviewCommon(ParameterHandling parHandling, Logger logger)
		{
			this.parHandling = parHandling;
			this.logger = logger;
		}

		// Token: 0x06001CEB RID: 7403 RVA: 0x000BABE4 File Offset: 0x000B8DE4
		public void ShowVoltageWindow()
		{
			string[] currentPaths = new string[]
			{
				ErrorSurvey.ioRoot + ErrorSurvey.ioVoltage12V.IOPath + "/",
				ErrorSurvey.ioRoot + ErrorSurvey.ioVoltage24V.IOPath + "/"
			};
			OverviewCommon.OperatingHourVisibility = false;
			this.ctrlParIOs = new CtrlParameter(this.parHandling, currentPaths, true, false, this.logger, false, null, null, null);
			this.ctrlParIOs.ItemsCreated += this.ctrlParVoltages_ItemsCreated;
			this.ctrlParIOs.StartCreation();
			Subwindow.Show(this.ctrlParIOs);
		}

		// Token: 0x06001CEC RID: 7404 RVA: 0x000BAC88 File Offset: 0x000B8E88
		private void ctrlParVoltages_ItemsCreated()
		{
			bool flag = this.ctrlParIOs != null;
			if (flag)
			{
				this.ctrlParIOs.ItemsCreated -= this.ctrlParVoltages_ItemsCreated;
				List<InOut> list = new List<InOut>();
				list.Add(ErrorSurvey.ioVoltage12V);
				list.Add(ErrorSurvey.ioVoltage24V);
				this.ctrlParIOs.SetLocalizedPseudoPath(list);
			}
		}

		// Token: 0x06001CED RID: 7405 RVA: 0x000BACE8 File Offset: 0x000B8EE8
		public void ShowTemperatureWindow(int ioVersion)
		{
			List<string> list = new List<string>();
			this.ioVersion = ioVersion;
			bool flag = ioVersion < Definition.minNewWagoConfigVersion;
			if (flag)
			{
				list.Add(ErrorSurvey.ioRoot + ErrorSurvey.ioTempSensor0Old.IOPath + "/");
				list.Add(ErrorSurvey.ioRoot + ErrorSurvey.ioTempSensor1Old.IOPath + "/");
			}
			else
			{
				bool flag2 = ioVersion < Definition.minSesotecIOProgramVersion;
				if (flag2)
				{
					list.Add(ErrorSurvey.ioRoot + ErrorSurvey.ioTempSensor0New.IOPath + "/");
					list.Add(ErrorSurvey.ioRoot + ErrorSurvey.ioTempSensor1New.IOPath + "/");
				}
				else
				{
					list.Add(ErrorSurvey.ioRoot + ErrorSurvey.ioTempSensorSesotecMain.IOPath + "/");
				}
			}
			OverviewCommon.OperatingHourVisibility = false;
			this.ctrlParIOs = new CtrlParameter(this.parHandling, list.ToArray(), true, false, this.logger, false, null, null, null);
			this.ctrlParIOs.ItemsCreated += this.ctrlParTemperatures_ItemsCreated;
			this.ctrlParIOs.StartCreation();
			Subwindow.Show(this.ctrlParIOs);
		}

		// Token: 0x06001CEE RID: 7406 RVA: 0x000BAE20 File Offset: 0x000B9020
		private void ctrlParTemperatures_ItemsCreated()
		{
			this.ctrlParIOs.ItemsCreated -= this.ctrlParTemperatures_ItemsCreated;
			bool flag = this.ctrlParIOs != null;
			if (flag)
			{
				List<InOut> list = new List<InOut>();
				bool flag2 = this.ioVersion < Definition.minNewWagoConfigVersion;
				if (flag2)
				{
					list.Add(ErrorSurvey.ioTempSensor0Old);
					list.Add(ErrorSurvey.ioTempSensor1Old);
				}
				else
				{
					bool flag3 = this.ioVersion < Definition.minSesotecIOProgramVersion;
					if (flag3)
					{
						list.Add(ErrorSurvey.ioTempSensor0New);
						list.Add(ErrorSurvey.ioTempSensor1New);
					}
					else
					{
						list.Add(ErrorSurvey.ioTempSensorSesotecMain);
					}
				}
				this.ctrlParIOs.SetLocalizedPseudoPath(list);
			}
		}

		// Token: 0x06001CEF RID: 7407 RVA: 0x000BAED0 File Offset: 0x000B90D0
		public void ShowSafetySwitchWindow(int ioVersion, bool newBulkConfig)
		{
			List<string> list = new List<string>();
			MachineType machineType = this.GetMachineType();
			bool flag = this.parHandling.ReadPar(Definition.machineModePath) == "1";
			this.newBulkConfig = newBulkConfig;
			this.ioVersion = ioVersion;
			bool flag2 = ioVersion < Definition.minNewWagoConfigVersion;
			if (flag2)
			{
				list = (from item in ErrorSurvey.IOsSafetySwitchesOld
				select item.IOPath).ToList<string>();
			}
			else
			{
				bool flag3 = ioVersion < Definition.minSesotecIOProgramVersion;
				if (flag3)
				{
					bool flag4 = !newBulkConfig;
					if (flag4)
					{
						list = (from item in ErrorSurvey.IOsSafetySwitchesNew
						select item.IOPath).ToList<string>();
					}
					else
					{
						list = (from item in ErrorSurvey.IOsSafetySwitchesNewBulk
						select item.IOPath).ToList<string>();
					}
				}
				else
				{
					bool flag5 = machineType == MachineType.RayconEntry;
					if (flag5)
					{
						list = (from item in ErrorSurvey.IOsSafetySwitchesEntry
						select item.IOPath).ToList<string>();
					}
					bool flag6 = machineType == MachineType.RayconDPlus;
					if (flag6)
					{
						bool flag7 = this.parHandling.ReadPar(Definition.wagoMachineTypePath) == Definition.ioModuleIsRayconDPlus1ModuleHYRework;
						if (flag7)
						{
							list = (from item in ErrorSurvey.IOsSafetySwitchesDPlus1ModuleHYRework
							select item.IOPath).ToList<string>();
						}
						else
						{
							bool flag8 = flag;
							if (flag8)
							{
								bool flag9 = this.parHandling.ReadPar(Definition.wagoMachineTypePath) == Definition.ioModuleIsRayconDPlus1Module;
								if (flag9)
								{
									list = (from item in ErrorSurvey.IOsSafetySwitchesDPlus1ModuleBulk
									select item.IOPath).ToList<string>();
								}
								else
								{
									list = (from item in ErrorSurvey.IOsSafetySwitchesDPlusBulk
									select item.IOPath).ToList<string>();
								}
							}
							else
							{
								bool flag10 = this.parHandling.ReadPar(Definition.wagoMachineTypePath) == Definition.ioModuleIsRayconDPlus1Module;
								if (flag10)
								{
									list = (from item in ErrorSurvey.IOsSafetySwitchesDPlus1Module
									select item.IOPath).ToList<string>();
								}
								else
								{
									list = (from item in ErrorSurvey.IOsSafetySwitchesDPlus
									select item.IOPath).ToList<string>();
								}
							}
						}
					}
					bool flag11 = machineType == MachineType.RayconEX2A;
					if (flag11)
					{
					}
				}
			}
			list.Add("AI_00");
			for (int i = 0; i < list.Count; i++)
			{
				list[i] = ErrorSurvey.ioRoot + list[i] + "/";
			}
			OverviewCommon.OperatingHourVisibility = false;
			this.ctrlParIOs = new CtrlParameter(this.parHandling, list.ToArray(), true, false, this.logger, false, null, null, null);
			this.ctrlParIOs.ItemsCreated += this.ctrlParSafetySwitches_ItemsCreated;
			this.ctrlParIOs.StartCreation();
			Subwindow.Show(this.ctrlParIOs);
		}

		// Token: 0x06001CF0 RID: 7408 RVA: 0x000BB240 File Offset: 0x000B9440
		private void ctrlParSafetySwitches_ItemsCreated()
		{
			List<InOut> list = new List<InOut>();
			MachineType machineType = this.GetMachineType();
			bool flag = this.parHandling.ReadPar(Definition.machineModePath) == "1";
			this.ctrlParIOs.ItemsCreated -= this.ctrlParSafetySwitches_ItemsCreated;
			bool flag2 = this.ctrlParIOs != null;
			if (flag2)
			{
				bool flag3 = this.ioVersion < Definition.minNewWagoConfigVersion;
				if (flag3)
				{
					list = ErrorSurvey.IOsSafetySwitchesOld.ToList<InOut>();
				}
				else
				{
					bool flag4 = this.ioVersion < Definition.minSesotecIOProgramVersion;
					if (flag4)
					{
						bool flag5 = !this.newBulkConfig;
						if (flag5)
						{
							list = ErrorSurvey.IOsSafetySwitchesNew.ToList<InOut>();
						}
						else
						{
							list = ErrorSurvey.IOsSafetySwitchesNewBulk.ToList<InOut>();
						}
					}
					else
					{
						bool flag6 = machineType == MachineType.RayconEntry;
						if (flag6)
						{
							list = ErrorSurvey.IOsSafetySwitchesEntry.ToList<InOut>();
						}
						bool flag7 = machineType == MachineType.RayconDPlus;
						if (flag7)
						{
							bool flag8 = flag;
							if (flag8)
							{
								bool flag9 = this.parHandling.ReadPar(Definition.wagoMachineTypePath) == Definition.ioModuleIsRayconDPlus1Module;
								if (flag9)
								{
									list = ErrorSurvey.IOsSafetySwitchesDPlus1ModuleBulk.ToList<InOut>();
								}
								else
								{
									list = ErrorSurvey.IOsSafetySwitchesDPlusBulk.ToList<InOut>();
								}
							}
							else
							{
								bool flag10 = this.parHandling.ReadPar(Definition.wagoMachineTypePath) == Definition.ioModuleIsRayconDPlus1Module;
								if (flag10)
								{
									list = ErrorSurvey.IOsSafetySwitchesDPlus1Module.ToList<InOut>();
								}
								else
								{
									bool flag11 = this.parHandling.ReadPar(Definition.wagoMachineTypePath) == Definition.ioModuleIsRayconDPlus1ModuleHYRework;
									if (flag11)
									{
										list = ErrorSurvey.IOsSafetySwitchesDPlus1ModuleHYRework.ToList<InOut>();
									}
									else
									{
										list = ErrorSurvey.IOsSafetySwitchesDPlus.ToList<InOut>();
									}
								}
							}
						}
						bool flag12 = machineType == MachineType.RayconEX2A;
						if (flag12)
						{
						}
					}
				}
				list.Add(new InOut("AI_00", "parItemLampMonitor"));
				this.ctrlParIOs.SetLocalizedPseudoPath(list);
			}
		}

		// Token: 0x06001CF1 RID: 7409 RVA: 0x000BB40C File Offset: 0x000B960C
		public void ShowXRaySourceWindow()
		{
			int num = 0;
			List<string> list = new List<string>();
			list.AddRange(new string[]
			{
				"/hardware/xraysource/Voltage/",
				"/hardware/xraysource/Current/",
				"/hardware/xraysource/Faults/",
				"/hardware/xraysource/TankTemperature/",
				"/hardware/xraysource/FirmwareVersion/",
				"/hardware/xraysource/HardwareVersion/",
				"/hardware/xraysource/SoftwareVersion/",
				"/hardware/xraysource/ModelNumber/"
			});
			string s = this.parHandling.ReadPar(Definition.ioVersionPath);
			int.TryParse(s, out num);
			bool flag = num < Definition.minSesotecIOProgramVersion;
			if (flag)
			{
				list.AddRange(new string[]
				{
					"/hardware/xraysource/XRayOnTimeInternal/",
					"/hardware/xraysource/LastXRayOnTime/",
					"/hardware/xraysource/XRayOnCycles/"
				});
			}
			else
			{
				list.Add("/hardware/xraysource/XRayOnTimeInternal/");
			}
			OverviewCommon.OperatingHourVisibility = true;
			CtrlParameter ctrlParameter = new CtrlParameter(this.parHandling, list.ToArray(), true, false, this.logger, false, null, null, null);
			ctrlParameter.StartCreation();
			Subwindow.Show(ctrlParameter);
		}

		// Token: 0x06001CF2 RID: 7410 RVA: 0x000BB504 File Offset: 0x000B9704
		public void ShowDetectorWindow(DetectorType detectorType)
		{
			bool flag = detectorType == DetectorType.DT;
			string[] currentPaths;
			if (flag)
			{
				currentPaths = new string[]
				{
					"/hardware/detector/dt/ScanRate/",
					"/hardware/detector/dt/MCUTemperature/",
					"/hardware/detector/dt/FPGAInfo/",
					"/hardware/detector/dt/Info/"
				};
			}
			else
			{
				bool flag2 = detectorType == DetectorType.Sesotec;
				if (flag2)
				{
					currentPaths = new string[]
					{
						"/hardware/detector/sesotec/ScanRate/",
						"/hardware/detector/sesotec/HardwareVersion/",
						"/hardware/detector/sesotec/SoftwareVersion/",
						"/hardware/detector/sesotec/MacAddress/",
						"/hardware/detector/sesotec/NumPixel/",
						"/hardware/detector/sesotec/NumModules/",
						"/hardware/detector/sesotec/Gain/",
						"/hardware/detector/sesotec/Status/",
						"/hardware/detector/sesotec/Temperature/",
						"/hardware/detector/sesotec/PixelSize/",
						"/hardware/detector/sesotec/SubType/"
					};
				}
				else
				{
					bool flag3 = detectorType == DetectorType.DTNetDE;
					if (flag3)
					{
						currentPaths = new string[]
						{
							"/hardware/detector/dt/ScanRate/",
							"/hardware/detector/dt/Info/",
							"/hardware/detector/dt/Serial/",
							"/hardware/detector/dt/NumPixel/",
							"/hardware/detector/dt/NumModulesTotal/",
							"/hardware/detector/dt/NumModulesPerChannel/",
							"/hardware/detector/dt/LowGain/",
							"/hardware/detector/dt/HighGain/",
							"/hardware/detector/dt/MCUTemperature/",
							"/hardware/detector/dt/PixelSize/",
							"/hardware/detector/dt/GCUVoltages/"
						};
					}
					else
					{
						bool flag4 = detectorType == DetectorType.DTNetTDI;
						if (flag4)
						{
							currentPaths = new string[]
							{
								"/hardware/detector/dt/ScanRate/",
								"/hardware/detector/dt/Info/",
								"/hardware/detector/dt/Serial/",
								"/hardware/detector/dt/NumPixel/",
								"/hardware/detector/dt/NumModulesTotal/",
								"/hardware/detector/dt/NumModulesPerChannel/",
								"/hardware/detector/dt/LowGain/",
								"/hardware/detector/dt/HighGain/",
								"/hardware/detector/dt/MCUTemperature/",
								"/hardware/detector/dt/PixelSize/",
								"/hardware/detector/dt/GCUVoltages/",
								"/hardware/detector/dt/GCUHumidity/"
							};
						}
						else
						{
							currentPaths = new string[]
							{
								"/hardware/detector/dt/ScanRate/",
								"/hardware/detector/dt/MCUTemperature/",
								"/hardware/detector/dt/Serial/",
								"/hardware/detector/dt/NumPixel/",
								"/hardware/detector/dt/PixelSize/"
							};
						}
					}
				}
			}
			OverviewCommon.OperatingHourVisibility = false;
			CtrlParameter ctrlParameter = new CtrlParameter(this.parHandling, currentPaths, true, false, this.logger, false, null, null, null);
			ctrlParameter.StartCreation();
			Subwindow.Show(ctrlParameter);
		}

		// Token: 0x06001CF3 RID: 7411 RVA: 0x000BB708 File Offset: 0x000B9908
		public MachineType GetMachineType()
		{
			MachineType result = MachineType.RayconEntry;
			string value = this.parHandling.ReadPar(Definition.machineTypePath);
			try
			{
				result = (MachineType)Enum.Parse(typeof(MachineType), value);
			}
			catch
			{
			}
			return result;
		}

		// Token: 0x0400131F RID: 4895
		private CtrlParameter ctrlParIOs;

		// Token: 0x04001320 RID: 4896
		private ParameterHandling parHandling;

		// Token: 0x04001321 RID: 4897
		private Logger logger;

		// Token: 0x04001322 RID: 4898
		private int ioVersion = 0;

		// Token: 0x04001323 RID: 4899
		private bool newBulkConfig;
	}
}
