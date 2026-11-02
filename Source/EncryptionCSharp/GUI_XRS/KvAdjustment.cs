using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Definitions;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x0200002E RID: 46
	public class KvAdjustment
	{
		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x060005D2 RID: 1490 RVA: 0x00027991 File Offset: 0x00025B91
		// (set) Token: 0x060005D3 RID: 1491 RVA: 0x00027999 File Offset: 0x00025B99
		public int LowThreshold { get; set; }

		// Token: 0x170001CA RID: 458
		// (get) Token: 0x060005D4 RID: 1492 RVA: 0x000279A2 File Offset: 0x00025BA2
		// (set) Token: 0x060005D5 RID: 1493 RVA: 0x000279AA File Offset: 0x00025BAA
		public int HighThreshold { get; set; }

		// Token: 0x170001CB RID: 459
		// (get) Token: 0x060005D6 RID: 1494 RVA: 0x000279B3 File Offset: 0x00025BB3
		// (set) Token: 0x060005D7 RID: 1495 RVA: 0x000279BB File Offset: 0x00025BBB
		public double MinKV { get; set; }

		// Token: 0x170001CC RID: 460
		// (get) Token: 0x060005D8 RID: 1496 RVA: 0x000279C4 File Offset: 0x00025BC4
		// (set) Token: 0x060005D9 RID: 1497 RVA: 0x000279CC File Offset: 0x00025BCC
		public double MaxKV { get; set; }

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x060005DA RID: 1498 RVA: 0x000279D8 File Offset: 0x00025BD8
		public double Watt
		{
			get
			{
				return this.watt;
			}
		}

		// Token: 0x170001CE RID: 462
		// (get) Token: 0x060005DB RID: 1499 RVA: 0x000279F0 File Offset: 0x00025BF0
		public double CurrentKV
		{
			get
			{
				return this.currentKV;
			}
		}

		// Token: 0x170001CF RID: 463
		// (get) Token: 0x060005DC RID: 1500 RVA: 0x00027A08 File Offset: 0x00025C08
		public double CurrentMA
		{
			get
			{
				return this.currentMA;
			}
		}

		// Token: 0x060005DD RID: 1501 RVA: 0x00027A20 File Offset: 0x00025C20
		public KvAdjustment(ParameterHandling parHandling)
		{
			this.parHandling = parHandling;
			this.LowThreshold = 500;
			this.HighThreshold = 2000;
			this.MinKV = 40.0;
			string value = parHandling.ReadPar(XRayOnProcessing.xrayModelNumberPath);
			bool flag = XRayOnProcessing.modelNr60KV200WP760.Contains(value);
			if (flag)
			{
				this.MinKV = 50.0;
			}
			this.MaxKV = (double)XRayOnProcessing.GetMaxKV(parHandling);
			this.watt = (double)XRayOnProcessing.GetMaxWatt(parHandling);
		}

		// Token: 0x060005DE RID: 1502 RVA: 0x00027AAC File Offset: 0x00025CAC
		public bool AdjustSettings(XRSImage image)
		{
			int num = this.MeanValueforAutokV(image);
			double newKV = this.GetNewKV(num);
			return this.AdjustSettings(newKV, num, false);
		}

		// Token: 0x060005DF RID: 1503 RVA: 0x00027AD8 File Offset: 0x00025CD8
		public bool AdjustmentNeeded(double kV)
		{
			double num = this.watt / kV;
			double maxMA = XRayOnProcessing.GetMaxMA(this.parHandling);
			bool flag = num > maxMA;
			if (flag)
			{
				num = maxMA;
			}
			return this.NewKVOrMANeeded(kV, num);
		}

		// Token: 0x060005E0 RID: 1504 RVA: 0x00027B14 File Offset: 0x00025D14
		public bool AdjustSettings(double newKV, int meanval, bool defaultkV)
		{
			double num = this.watt / newKV;
			string a = this.parHandling.ReadPar(Definition.autoKVAdjustmentPath);
			bool flag = a == "2" && !defaultkV;
			if (flag)
			{
				bool flag2 = newKV == 40.0;
				if (flag2)
				{
					newKV = 50.0;
					num = 2.0;
				}
				else
				{
					bool flag3 = newKV == 50.0;
					if (flag3)
					{
						num = 3.0;
					}
					else
					{
						bool flag4 = newKV > 50.0 && newKV <= 100.0;
						if (flag4)
						{
							num = (this.watt - 50.0) / newKV;
						}
					}
				}
			}
			double maxMA = XRayOnProcessing.GetMaxMA(this.parHandling);
			bool flag5 = num > maxMA;
			if (flag5)
			{
				num = maxMA;
			}
			bool flag6 = this.NewKVOrMANeeded(newKV, num);
			bool result;
			if (flag6)
			{
				this.parHandling.WriteValue("/hardware/xraysource/VoltageSetpoint/Value", newKV.ToString(new CultureInfo("en-US")));
				this.parHandling.WriteValue("/hardware/xraysource/CurrentSetpoint/Value", num.ToString(new CultureInfo("en-US")));
				this.currentKV = newKV;
				this.currentMA = num;
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x060005E1 RID: 1505 RVA: 0x00027C64 File Offset: 0x00025E64
		private bool NewKVOrMANeeded(double newKV, double newMA)
		{
			string s = this.parHandling.ReadPar("/hardware/xraysource/CurrentSetpoint/Value");
			double.TryParse(s, NumberStyles.AllowDecimalPoint, new CultureInfo("en-US"), out this.currentMA);
			string s2 = this.parHandling.ReadPar("/hardware/xraysource/VoltageSetpoint/Value");
			double.TryParse(s2, NumberStyles.AllowDecimalPoint, new CultureInfo("en-US"), out this.currentKV);
			bool flag = Math.Abs(this.currentMA - newMA) >= 0.1;
			bool flag2 = Math.Abs(this.currentKV - newKV) >= 0.1;
			return flag2 || flag;
		}

		// Token: 0x060005E2 RID: 1506 RVA: 0x00027D18 File Offset: 0x00025F18
		public bool CheckAdjustment(XRSImage image)
		{
			bool result = false;
			int num = this.MeanValueforAutokV(image);
			string a = this.parHandling.ReadPar(Definition.autoKVAdjustmentPath);
			bool flag = a == "1";
			if (flag)
			{
				bool flag2 = (this.currentKV == this.MinKV && num > this.HighThreshold) || (this.currentKV == this.MaxKV && num < this.LowThreshold) || (num >= this.LowThreshold && num <= this.HighThreshold);
				if (flag2)
				{
					result = true;
				}
			}
			return result;
		}

		// Token: 0x060005E3 RID: 1507 RVA: 0x00027DB0 File Offset: 0x00025FB0
		private double GetNewKV(int min)
		{
			double num = this.currentKV;
			bool flag = min < this.LowThreshold;
			if (flag)
			{
				bool flag2 = min < 50;
				if (flag2)
				{
					num = this.MaxKV;
				}
				else
				{
					num += (double)((this.LowThreshold - min) / 10);
				}
			}
			else
			{
				bool flag3 = min > this.HighThreshold;
				if (flag3)
				{
					num = this.MinKV;
				}
			}
			bool flag4 = num > this.MaxKV;
			if (flag4)
			{
				num = this.MaxKV;
			}
			else
			{
				bool flag5 = num < this.MinKV;
				if (flag5)
				{
					num = this.MinKV;
				}
			}
			num = this.RoundUpToNearestMultipleOfTen(num);
			bool flag6 = num == 70.0;
			if (flag6)
			{
				num = 60.0;
			}
			else
			{
				bool flag7 = num == 80.0 || num == 90.0;
				if (flag7)
				{
					num = 100.0;
				}
			}
			return num;
		}

		// Token: 0x060005E4 RID: 1508 RVA: 0x00027E94 File Offset: 0x00026094
		private double RoundUpToNearestMultipleOfTen(double valueToRoundUp)
		{
			return Math.Ceiling(valueToRoundUp / 10.0) * 10.0;
		}

		// Token: 0x060005E5 RID: 1509 RVA: 0x00027EC4 File Offset: 0x000260C4
		private int MeanValueforAutokV(XRSImage image)
		{
			int num = 0;
			List<short> list = new List<short>(image.Data.Length);
			list.AddRange(image.Data);
			list.Sort();
			int num2 = Convert.ToInt32(Convert.ToDouble(list.Count) * 0.005);
			bool flag = num2 != 0;
			if (flag)
			{
				for (int i = 0; i < num2; i++)
				{
					num += (int)list[i];
				}
				num /= num2;
			}
			this.parHandling.WriteValue("/application/AutoKVMinGreyscale/Value", num.ToString(), true);
			return num;
		}

		// Token: 0x0400043C RID: 1084
		private ParameterHandling parHandling;

		// Token: 0x0400043D RID: 1085
		private double watt;

		// Token: 0x0400043E RID: 1086
		private double currentKV;

		// Token: 0x0400043F RID: 1087
		private double currentMA;
	}
}
