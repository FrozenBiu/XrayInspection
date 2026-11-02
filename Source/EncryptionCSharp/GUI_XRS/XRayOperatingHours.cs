using System;
using DatabaseConnection;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x02000239 RID: 569
	public class XRayOperatingHours
	{
		// Token: 0x06002B5D RID: 11101 RVA: 0x0011EED0 File Offset: 0x0011D0D0
		public XRayOperatingHours(DBConnection dbConnection, ParameterHandling parHandling, DBDataLogs dbDatalogs)
		{
			this.dbConnection = dbConnection;
			this.dbDatalogs = dbDatalogs;
			this.parHandling = parHandling;
			this.xrayOnTimePerkVmAActual = Math.Round(this.timerforDbEntry / 3600.0, 3);
		}

		// Token: 0x06002B5E RID: 11102 RVA: 0x0011EF2C File Offset: 0x0011D12C
		private void GetAssignedValues()
		{
			bool flag = this.parHandling != null;
			if (flag)
			{
				double num = 0.0;
				double num2 = 0.0;
				double.TryParse(this.parHandling.ReadPar("/hardware/xraysource/Voltage/Value"), out num);
				double.TryParse(this.parHandling.ReadPar("/hardware/xraysource/Current/Value"), out num2);
				bool flag2 = num < 30.0 || num > 100.0;
				if (flag2)
				{
					this.xrayVoltageActual = 0;
				}
				else
				{
					this.xrayVoltageActual = Convert.ToInt32(Math.Round(num));
				}
				bool flag3 = num2 < 0.2 || num2 > 6.0;
				if (flag3)
				{
					this.xrayCurrentActual = 0.0;
				}
				else
				{
					this.xrayCurrentActual = Math.Round(num2, 1);
				}
			}
		}

		// Token: 0x06002B5F RID: 11103 RVA: 0x0011F008 File Offset: 0x0011D208
		public void SetXrayOnTimeForkVmA(bool xrayon)
		{
			bool flag = false;
			int num = 0;
			bool flag2 = false;
			double num2 = 0.0;
			double num3 = 0.0;
			if (xrayon)
			{
				this.secondcounter++;
				bool flag3 = (double)this.secondcounter >= this.timerforDbEntry;
				if (flag3)
				{
					this.secondcounter = 0;
					this.SetNameXrayOnTimeTable();
					this.GetAssignedValues();
					this.dbConnection.GetXRayOnTimeskVmALength(ref this.dBLength);
					this.dbDatalogs.XRayOnTimesGetTableLength(ref num);
					this.dbConnection.GetDBDatalogsIsActive(ref flag2);
					this.CopyDatalogsToXRayOnTimesFromDBIfNeccessary(num, flag2);
					this.CopyXRayOnTimesFromDBToDatalogsIfNeccessary(num, flag2, true);
					bool flag4 = this.dBLength == 0;
					if (flag4)
					{
						bool flag5 = this.xrayCurrentActual != 0.0 && this.xrayVoltageActual != 0;
						if (flag5)
						{
							this.dbConnection.AddXRayOnTimeskVmAEntry(this.xrayVoltageActual, this.xrayCurrentActual, this.xrayOnTimePerkVmAActual);
							bool flag6 = flag2;
							if (flag6)
							{
								bool flag7 = num != 0;
								if (flag7)
								{
									this.ResetDatalogsIfNecessary(num, flag2);
								}
								else
								{
									this.dbDatalogs.XRayOnTimesAddEntry(this.xrayVoltageActual, this.xrayCurrentActual, this.xrayOnTimePerkVmAActual, "");
								}
							}
						}
					}
					else
					{
						for (int i = 1; i <= this.dBLength; i++)
						{
							this.dbConnection.GetXRayOnTimeskVmAEntry(i, ref this.xraydBVoltage, ref this.xraydBCurrent, ref this.xraydBOnTimePerkVmA, ref this.xraydBdatetime);
							this.dbDatalogs.GetXRayOnkVmAEntrywithID(i, ref num2, ref num3, ref this.xraydBOnTimePerkVmA, ref this.xraydBdatetime, false);
							bool flag8 = (double)this.xrayVoltageActual == this.xraydBVoltage && this.xrayCurrentActual == this.xraydBCurrent && !flag;
							if (flag8)
							{
								double operatinghours = this.xraydBOnTimePerkVmA + this.xrayOnTimePerkVmAActual;
								this.dbConnection.SummEntryXRayOnTimeskVmA(i, operatinghours);
								bool flag9 = flag2;
								if (flag9)
								{
									bool flag10 = num2 != this.xraydBVoltage || num3 != this.xraydBCurrent;
									if (flag10)
									{
										this.ResetDatalogsIfNecessary(num, flag2);
									}
									else
									{
										this.dbDatalogs.XRayOnTimesSummEntry(i, operatinghours);
									}
								}
								flag = true;
							}
							bool flag11 = i == this.dBLength && !flag && this.xrayCurrentActual != 0.0 && this.xrayVoltageActual != 0;
							if (flag11)
							{
								this.dbConnection.AddXRayOnTimeskVmAEntry(this.xrayVoltageActual, this.xrayCurrentActual, this.xrayOnTimePerkVmAActual);
								bool flag12 = flag2;
								if (flag12)
								{
									bool flag13 = num2 != this.xraydBVoltage || num3 != this.xraydBCurrent;
									if (flag13)
									{
										this.ResetDatalogsIfNecessary(num, flag2);
									}
									else
									{
										this.dbDatalogs.XRayOnTimesAddEntry(this.xrayVoltageActual, this.xrayCurrentActual, this.xrayOnTimePerkVmAActual, "");
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06002B60 RID: 11104 RVA: 0x0011F308 File Offset: 0x0011D508
		public void GetXrayOnTimeForkVmAOverview(string[] date, string[] voltage, string[] current, string[] hours, ref string totalhours)
		{
			double value = 0.0;
			double value2 = 0.0;
			double num = 0.0;
			double num2 = 0.0;
			string text = "";
			bool flag = this.dbConnection != null;
			if (flag)
			{
				this.dbConnection.GetXRayOnTimeskVmALength(ref this.dBLength);
				bool flag2 = this.dBLength != 0;
				if (flag2)
				{
					for (int i = 1; i <= this.dBLength; i++)
					{
						this.dbConnection.GetXRayOnTimeskVmAEntry(i, ref value2, ref value, ref num, ref text);
						voltage[i - 1] = Convert.ToString(value2);
						current[i - 1] = Convert.ToString(value);
						hours[i - 1] = Convert.ToString(num);
						date[i - 1] = text;
						num2 += num;
					}
					totalhours = Convert.ToString(num2);
				}
			}
		}

		// Token: 0x06002B61 RID: 11105 RVA: 0x0011F3EC File Offset: 0x0011D5EC
		private void SetNameXrayOnTimeTable()
		{
			bool flag = false;
			this.dbConnection.GetDBDatalogsIsActive(ref flag);
			string text = this.parHandling.ReadPar("/hardware/xraysource/SerialNumber/Value");
			string a = this.parHandling.ReadPar("/hardware/detector/Type/Value");
			bool flag2 = a == "2" || a == "4";
			string text2;
			if (flag2)
			{
				text2 = this.parHandling.ReadPar("/hardware/detector/dt/Serial/Value");
			}
			else
			{
				text2 = this.parHandling.ReadPar("/hardware/detector/sesotec/MacAddress/Value");
			}
			bool flag3 = text2 != "";
			if (flag3)
			{
				text2 = text2.Replace(":", "");
				text2 = text2.Replace("-", "");
			}
			bool flag4 = text != "";
			if (flag4)
			{
				text = text.Replace("-", "");
				bool flag5 = flag;
				if (flag5)
				{
					this.dbDatalogs.XRayOnTimeskVmARenameTables(text, text2);
				}
				text = "XRayOnTimes_" + text;
				this.dbConnection.SetXRaySerialnumber(text);
				this.dbConnection.XRayOnTimeskVmARenameTable();
			}
		}

		// Token: 0x06002B62 RID: 11106 RVA: 0x0011F50C File Offset: 0x0011D70C
		public void SetXrayOperatingHoursDB()
		{
			double num = 0.0;
			double num2 = 0.0;
			double num3 = 0.0;
			double num4 = 0.0;
			string text = "";
			bool flag = this.dbConnection != null;
			if (flag)
			{
				this.dbConnection.GetXRayOnTimeskVmALength(ref this.dBLength);
				bool flag2 = this.dBLength != 0;
				if (flag2)
				{
					for (int i = 1; i <= this.dBLength; i++)
					{
						this.dbConnection.GetXRayOnTimeskVmAEntry(i, ref num2, ref num, ref num3, ref text);
						num4 += num3;
					}
					Math.Round(num4, 2);
					this.dbConnection.SetXRayOperatingHoursSesotec(num4);
					this.parHandling.WriteValue(Definition.xRayOperatingHoursSesotecPath, Convert.ToString(Convert.ToInt32(num4)));
				}
			}
		}

		// Token: 0x06002B63 RID: 11107 RVA: 0x0011F5EC File Offset: 0x0011D7EC
		public int GetAdjustedLimitForXrayOperatingHours(double limit)
		{
			double num = 0.0;
			double num2 = 0.0;
			double num3 = 0.0;
			string text = "";
			this.dbConnection.GetXRayOnTimeskVmALength(ref this.dBLength);
			bool flag = this.dBLength != 0;
			if (flag)
			{
				int maxWatt = XRayOnProcessing.GetMaxWatt(this.parHandling);
				double num4 = Math.Round(Convert.ToDouble(this.parHandling.ReadPar(Definition.dynlimitXraySourceOperatingHoursFactPath)), 2);
				for (int i = 1; i <= this.dBLength; i++)
				{
					this.dbConnection.GetXRayOnTimeskVmAEntry(i, ref num2, ref num, ref num3, ref text);
					bool flag2 = maxWatt > 0;
					if (flag2)
					{
						double num5 = 2.0 * num4 * (1.0 - num2 * num / (double)maxWatt) * num3;
						bool flag3 = num5 > 0.0;
						if (flag3)
						{
							limit += num5;
						}
					}
				}
			}
			return Convert.ToInt32(limit);
		}

		// Token: 0x06002B64 RID: 11108 RVA: 0x0011F710 File Offset: 0x0011D910
		private void CopyXRayOnTimesFromDBToDatalogsIfNeccessary(int datalogslength, bool datalogsIsActive, bool withLengthCheck)
		{
			bool flag = !withLengthCheck || (datalogslength == 0 && this.dBLength != 0);
			if (flag)
			{
				for (int i = 1; i <= this.dBLength; i++)
				{
					this.dbConnection.GetXRayOnTimeskVmAEntry(i, ref this.xraydBVoltage, ref this.xraydBCurrent, ref this.xraydBOnTimePerkVmA, ref this.xraydBdatetime);
					if (datalogsIsActive)
					{
						this.dbDatalogs.XRayOnTimesAddEntry(Convert.ToInt32(this.xraydBVoltage), this.xraydBCurrent, this.xraydBOnTimePerkVmA, this.xraydBdatetime);
					}
				}
			}
		}

		// Token: 0x06002B65 RID: 11109 RVA: 0x0011F7A8 File Offset: 0x0011D9A8
		private void CopyDatalogsToXRayOnTimesFromDBIfNeccessary(int datalogslength, bool datalogsIsActive)
		{
			double value = 0.0;
			double current = 0.0;
			double operatinghours = 0.0;
			string timestamp = "";
			bool flag = this.dBLength == 0 && datalogslength != 0;
			if (flag)
			{
				for (int i = 1; i <= datalogslength; i++)
				{
					this.dbDatalogs.GetXRayOnkVmAEntrywithID(i, ref value, ref current, ref operatinghours, ref timestamp, true);
					this.dbConnection.AddXRayOnTimeskVmAEntryWithTimeStamp(Convert.ToInt32(value), current, operatinghours, timestamp);
				}
			}
		}

		// Token: 0x06002B66 RID: 11110 RVA: 0x0011F838 File Offset: 0x0011DA38
		private void ResetDatalogsIfNecessary(int datalogslength, bool datalogsIsActive)
		{
			this.dbDatalogs.DropXRayOnTable();
			this.dbDatalogs.CreateNewXRayOnTable();
			this.CopyXRayOnTimesFromDBToDatalogsIfNeccessary(datalogslength, datalogsIsActive, false);
		}

		// Token: 0x04001B14 RID: 6932
		private DBConnection dbConnection;

		// Token: 0x04001B15 RID: 6933
		private DBDataLogs dbDatalogs;

		// Token: 0x04001B16 RID: 6934
		private ParameterHandling parHandling;

		// Token: 0x04001B17 RID: 6935
		private double xraydBVoltage;

		// Token: 0x04001B18 RID: 6936
		private double xraydBCurrent;

		// Token: 0x04001B19 RID: 6937
		private double xraydBOnTimePerkVmA;

		// Token: 0x04001B1A RID: 6938
		private string xraydBdatetime;

		// Token: 0x04001B1B RID: 6939
		private int dBLength;

		// Token: 0x04001B1C RID: 6940
		private int xrayVoltageActual;

		// Token: 0x04001B1D RID: 6941
		private double xrayCurrentActual;

		// Token: 0x04001B1E RID: 6942
		private double xrayOnTimePerkVmAActual;

		// Token: 0x04001B1F RID: 6943
		private double timerforDbEntry = 60.0;

		// Token: 0x04001B20 RID: 6944
		private int secondcounter = 0;
	}
}
