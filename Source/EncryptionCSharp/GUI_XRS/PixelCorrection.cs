using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using DatabaseConnection;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x02000136 RID: 310
	public class PixelCorrection
	{
		// Token: 0x170008B6 RID: 2230
		// (get) Token: 0x06001DD1 RID: 7633 RVA: 0x000C1EF9 File Offset: 0x000C00F9
		// (set) Token: 0x06001DD2 RID: 7634 RVA: 0x000C1F01 File Offset: 0x000C0101
		public MachineState CurrentMachineState { get; set; }

		// Token: 0x06001DD3 RID: 7635 RVA: 0x000C1F0A File Offset: 0x000C010A
		public PixelCorrection(DBConnection dbConnection, ParameterHandling parHandling)
		{
			this.dbConnection = dbConnection;
			this.parHandling = parHandling;
		}

		// Token: 0x06001DD4 RID: 7636 RVA: 0x000C1F30 File Offset: 0x000C0130
		public void StartCorrection()
		{
			bool flag = this.timerCorrectOffset == null;
			if (flag)
			{
				this.timerCorrectOffset = new Timer(new TimerCallback(this.CorrectOffsetAndGain), null, 0, this.period);
			}
		}

		// Token: 0x06001DD5 RID: 7637 RVA: 0x000C1F6C File Offset: 0x000C016C
		public void StopCorrection()
		{
			bool flag = this.timerCorrectOffset != null;
			if (flag)
			{
				this.timerCorrectOffset.Change(-1, -1);
				this.timerCorrectOffset.Dispose();
				this.timerCorrectOffset = null;
			}
		}

		// Token: 0x06001DD6 RID: 7638 RVA: 0x000C1FAC File Offset: 0x000C01AC
		private void CorrectOffsetAndGain(object dummy)
		{
			bool flag = true;
			bool flag2 = this.parHandling.ReadPar(Definition.isLiveCalibrationActivePath) == "1" || this.parHandling.ReadPar(Definition.isLiveCalibrationActivePath) == "2";
			int temperature;
			bool flag3 = this.CurrentMachineState != null && this.CurrentMachineState.State == MachineStates.Inspecting && !flag2 && this.MCUTemperatureChanged(out temperature);
			if (flag3)
			{
				this.CorrectOffset(temperature);
				this.dbConnection.GetSaveGainCalibration(ref flag);
				bool flag4 = flag;
				if (flag4)
				{
					this.CorrectGain(temperature);
				}
				this.parHandling.WriteValue(Definition.temperatureInUsePath, temperature.ToString(), false);
			}
		}

		// Token: 0x06001DD7 RID: 7639 RVA: 0x000C2064 File Offset: 0x000C0264
		public void CorrectOffset(int temperature)
		{
			DetectorType detectorType = DetectorType.DT;
			bool flag = this.parHandling.ReadPar(Definition.isLiveCalibrationActivePath) == "1" || this.parHandling.ReadPar(Definition.isLiveCalibrationActivePath) == "2";
			bool flag2 = detectorType == DetectorType.DTNetTDI;
			if (!flag2)
			{
				try
				{
					List<int> list = new List<int>();
					this.dbConnection.GetDetectorType(ref detectorType);
					this.lastMCUTemperature = temperature;
					string path = (detectorType == DetectorType.DT || detectorType == DetectorType.DTNetSE || detectorType == DetectorType.DTNetDE || detectorType == DetectorType.DTNetTDI) ? Definition.detectorDTSerialPath : Definition.detectorSesotecMACPath;
					string serial = this.parHandling.ReadPar(path);
					bool flag3 = flag;
					if (flag3)
					{
						this.dbConnection.GetOffsetPixelValuesLiveCalibration(list);
					}
					else
					{
						this.dbConnection.GetOffsetPixelValues(this.lastMCUTemperature, serial, list);
					}
					bool flag4 = list.Count == 0 && !flag;
					if (flag4)
					{
						List<TemperatureDifference> nearestOffsetTemperatureDifferences = this.GetNearestOffsetTemperatureDifferences(serial);
						bool flag5 = nearestOffsetTemperatureDifferences.Count > 0;
						if (flag5)
						{
							bool flag6 = nearestOffsetTemperatureDifferences.Count == 1;
							if (flag6)
							{
								this.dbConnection.GetOffsetPixelValues(nearestOffsetTemperatureDifferences[0].Temperature, serial, list);
							}
							else
							{
								List<int> list2 = new List<int>();
								List<int> list3 = new List<int>();
								this.dbConnection.GetOffsetPixelValues(nearestOffsetTemperatureDifferences[0].Temperature, serial, list2);
								this.dbConnection.GetOffsetPixelValues(nearestOffsetTemperatureDifferences[1].Temperature, serial, list3);
								int num = (list2.Count <= list3.Count) ? list2.Count : list3.Count;
								for (int i = 0; i < num; i++)
								{
									double num2 = (double)(list2[i] - list3[i]) / (double)(nearestOffsetTemperatureDifferences[0].Temperature - nearestOffsetTemperatureDifferences[1].Temperature);
									double num3 = (double)list2[i] - num2 * (double)nearestOffsetTemperatureDifferences[0].Temperature;
									list.Add((int)(num2 * (double)this.lastMCUTemperature + num3));
								}
							}
						}
					}
					this.WriteOffsetValues(list);
				}
				catch (Exception ex)
				{
					Exception ex3;
					Exception ex2 = ex3;
					Exception ex = ex2;
					bool flag7 = Application.Current != null && !Application.Current.Dispatcher.CheckAccess();
					if (flag7)
					{
						Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate()
						{
							Dialog.Show("Error Write Offset Values", ex.Message);
						}));
					}
					else
					{
						Dialog.Show("Error Write Offset Values", ex.Message);
					}
				}
			}
		}

		// Token: 0x06001DD8 RID: 7640 RVA: 0x000C2324 File Offset: 0x000C0524
		public void CorrectGain(int temperature)
		{
			string text = null;
			string text2 = null;
			DetectorType detectorType = DetectorType.DT;
			bool flag = detectorType == DetectorType.DTNetTDI;
			if (!flag)
			{
				try
				{
					List<double> list = new List<double>();
					this.dbConnection.GetParameterValue("/hardware/xraysource/VoltageSetpoint/Value", ref text);
					this.dbConnection.GetParameterValue("/hardware/xraysource/CurrentSetpoint/Value", ref text2);
					this.dbConnection.GetDetectorType(ref detectorType);
					string path = (detectorType == DetectorType.DT || detectorType == DetectorType.DTNetSE || detectorType == DetectorType.DTNetDE || detectorType == DetectorType.DTNetTDI) ? Definition.detectorDTScanratePath : Definition.detectorSesotecScanratePath;
					string scanrate = this.parHandling.ReadPar(path);
					string path2 = (detectorType == DetectorType.DT || detectorType == DetectorType.DTNetSE || detectorType == DetectorType.DTNetDE || detectorType == DetectorType.DTNetTDI) ? Definition.detectorDTSerialPath : Definition.detectorSesotecMACPath;
					string serial = this.parHandling.ReadPar(path2);
					this.lastMCUTemperature = temperature;
					this.dbConnection.GetGainPixelValues(this.lastMCUTemperature, text, text2, scanrate, serial, list);
					bool flag2 = list.Count == 0;
					if (flag2)
					{
						List<TemperatureDifference> nearestGainTemperatureDifferences = this.GetNearestGainTemperatureDifferences(text, text2, scanrate, serial);
						bool flag3 = nearestGainTemperatureDifferences.Count > 0;
						if (flag3)
						{
							bool flag4 = nearestGainTemperatureDifferences.Count == 1;
							if (flag4)
							{
								this.dbConnection.GetGainPixelValues(nearestGainTemperatureDifferences[0].Temperature, text, text2, scanrate, serial, list);
							}
							else
							{
								List<double> list2 = new List<double>();
								List<double> list3 = new List<double>();
								this.dbConnection.GetGainPixelValues(nearestGainTemperatureDifferences[0].Temperature, text, text2, scanrate, serial, list2);
								this.dbConnection.GetGainPixelValues(nearestGainTemperatureDifferences[1].Temperature, text, text2, scanrate, serial, list3);
								int num = (list2.Count <= list3.Count) ? list2.Count : list3.Count;
								for (int i = 0; i < num; i++)
								{
									double num2 = (list2[i] - list3[i]) / (double)(nearestGainTemperatureDifferences[0].Temperature - nearestGainTemperatureDifferences[1].Temperature);
									double num3 = list2[i] - num2 * (double)nearestGainTemperatureDifferences[0].Temperature;
									list.Add(num2 * (double)this.lastMCUTemperature + num3);
								}
							}
						}
					}
					this.WriteGainValues(list);
				}
				catch (Exception ex)
				{
					Exception ex3;
					Exception ex2 = ex3;
					Exception ex = ex2;
					bool flag5 = Application.Current != null && !Application.Current.Dispatcher.CheckAccess();
					if (flag5)
					{
						Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate()
						{
							Dialog.Show("Error Write Gain Values", ex.Message);
						}));
					}
					else
					{
						Dialog.Show("Error Write Gain Values", ex.Message);
					}
				}
			}
		}

		// Token: 0x06001DD9 RID: 7641 RVA: 0x000C2608 File Offset: 0x000C0808
		public void CorrectGain(List<double> gainValues)
		{
			this.WriteGainValues(gainValues);
		}

		// Token: 0x06001DDA RID: 7642 RVA: 0x000C2614 File Offset: 0x000C0814
		public void CorrectGainLiveCalib()
		{
			try
			{
				List<double> list = new List<double>();
				this.dbConnection.GetGainPixelValuesLiveCalibration(list);
				this.WriteGainValues(list);
			}
			catch (Exception ex)
			{
				Exception ex3;
				Exception ex2 = ex3;
				Exception ex = ex2;
				bool flag = Application.Current != null && !Application.Current.Dispatcher.CheckAccess();
				if (flag)
				{
					Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate()
					{
						Dialog.Show("Error Write Gain Values", ex.Message);
					}));
				}
				else
				{
					Dialog.Show("Error Write Gain Values", ex.Message);
				}
			}
		}

		// Token: 0x06001DDB RID: 7643 RVA: 0x000C26BC File Offset: 0x000C08BC
		public void NullOffsetAndGain()
		{
			this.parHandling.WriteValue(Acquisition.resetCalibrationPath, "1", false);
		}

		// Token: 0x06001DDC RID: 7644 RVA: 0x000C26D8 File Offset: 0x000C08D8
		private List<TemperatureDifference> GetNearestOffsetTemperatureDifferences(string serial)
		{
			return this.GetNearestTemperatureDifferences(true, null, null, null, serial);
		}

		// Token: 0x06001DDD RID: 7645 RVA: 0x000C26F8 File Offset: 0x000C08F8
		private List<TemperatureDifference> GetNearestGainTemperatureDifferences(string voltage, string current, string scanrate, string serial)
		{
			return this.GetNearestTemperatureDifferences(false, voltage, current, scanrate, serial);
		}

		// Token: 0x06001DDE RID: 7646 RVA: 0x000C2718 File Offset: 0x000C0918
		private List<TemperatureDifference> GetNearestTemperatureDifferences(bool offset, string voltage, string current, string scanrate, string serial)
		{
			List<int> list = new List<int>();
			List<TemperatureDifference> list2 = new List<TemperatureDifference>();
			if (offset)
			{
				this.dbConnection.GetAvailableOffsetCorrectionTemperatures(list, serial);
			}
			else
			{
				this.dbConnection.GetAvailableGainCorrectionTemperatures(list, voltage, current, scanrate, serial);
			}
			for (int i = 0; i < list.Count; i++)
			{
				list2.Add(new TemperatureDifference(list[i], Math.Abs(this.lastMCUTemperature - list[i])));
			}
			list2.Sort(new TemperatureDiffComparer());
			return list2;
		}

		// Token: 0x06001DDF RID: 7647 RVA: 0x000C27B0 File Offset: 0x000C09B0
		private bool MCUTemperatureChanged(out int currentMCUTemperature)
		{
			DetectorType detectorType = DetectorType.DT;
			this.dbConnection.GetDetectorType(ref detectorType);
			string path = (detectorType == DetectorType.DT || detectorType == DetectorType.DTNetSE || detectorType == DetectorType.DTNetDE || detectorType == DetectorType.DTNetTDI) ? Definition.dtMCUTemperaturePath : Definition.detectorSesotecTemperaturePath;
			string s = this.parHandling.ReadPar(path);
			bool flag = int.TryParse(s, out currentMCUTemperature);
			if (flag)
			{
				bool flag2 = currentMCUTemperature > this.lastMCUTemperature;
				if (flag2)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001DE0 RID: 7648 RVA: 0x000C2828 File Offset: 0x000C0A28
		private void WriteOffsetValues(List<int> offsetValues)
		{
			DetectorType detectorType = DetectorType.DT;
			this.dbConnection.GetDetectorType(ref detectorType);
			bool flag = detectorType == DetectorType.DTNetTDI;
			if (!flag)
			{
				string s = (detectorType == DetectorType.DT || detectorType == DetectorType.DTNetSE || detectorType == DetectorType.DTNetDE || detectorType == DetectorType.DTNetTDI) ? this.parHandling.ReadPar(Definition.detectorDTNPixelPath) : this.parHandling.ReadPar(Definition.detectorSesotecNPixelPath);
				int num;
				bool flag2 = int.TryParse(s, out num);
				if (flag2)
				{
					for (int i = 0; i < num; i++)
					{
						bool flag3 = offsetValues.Count <= i;
						if (flag3)
						{
							break;
						}
						this.parHandling.WriteValue("/filter/acquisition/OffsetCalibrationIndex/Value", i.ToString(), false);
						this.parHandling.WriteValue("/filter/acquisition/OffsetCalibrationValue/Value", offsetValues[i].ToString(), false);
					}
				}
			}
		}

		// Token: 0x06001DE1 RID: 7649 RVA: 0x000C2900 File Offset: 0x000C0B00
		public void WriteGainValues(List<double> gainValues)
		{
			DetectorType detectorType = DetectorType.DT;
			this.dbConnection.GetDetectorType(ref detectorType);
			bool flag = detectorType == DetectorType.DTNetTDI;
			if (!flag)
			{
				string s = (detectorType == DetectorType.DT || detectorType == DetectorType.DTNetSE || detectorType == DetectorType.DTNetDE || detectorType == DetectorType.DTNetTDI) ? this.parHandling.ReadPar(Definition.detectorDTNPixelPath) : this.parHandling.ReadPar(Definition.detectorSesotecNPixelPath);
				int num;
				bool flag2 = int.TryParse(s, out num);
				if (flag2)
				{
					for (int i = 0; i < num; i++)
					{
						bool flag3 = gainValues.Count <= i;
						if (flag3)
						{
							break;
						}
						this.parHandling.WriteValue("/filter/acquisition/GainCalibrationIndex/Value", i.ToString(), false);
						this.parHandling.WriteValue("/filter/acquisition/GainCalibrationValue/Value", gainValues[i].ToString(), false);
					}
				}
			}
		}

		// Token: 0x0400139B RID: 5019
		private const string offsetValuePath = "/filter/acquisition/OffsetCalibrationValue/Value";

		// Token: 0x0400139C RID: 5020
		private const string offsetIndexPath = "/filter/acquisition/OffsetCalibrationIndex/Value";

		// Token: 0x0400139D RID: 5021
		private const string gainValuePath = "/filter/acquisition/GainCalibrationValue/Value";

		// Token: 0x0400139E RID: 5022
		private const string gainIndexPath = "/filter/acquisition/GainCalibrationIndex/Value";

		// Token: 0x0400139F RID: 5023
		private DBConnection dbConnection;

		// Token: 0x040013A0 RID: 5024
		private ParameterHandling parHandling;

		// Token: 0x040013A1 RID: 5025
		private Timer timerCorrectOffset;

		// Token: 0x040013A2 RID: 5026
		private int lastMCUTemperature;

		// Token: 0x040013A3 RID: 5027
		private int period = 30000;
	}
}
