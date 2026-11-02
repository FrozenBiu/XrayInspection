using System;
using System.Linq;
using Microsoft.Research.DynamicDataDisplay.Charts;

namespace GUI_XRS
{
	// Token: 0x02000118 RID: 280
	public class ChartCustomTicksProvider : DateTimeTicksProvider
	{
		// Token: 0x06001B28 RID: 6952 RVA: 0x000A30C0 File Offset: 0x000A12C0
		protected override DateTime[] ModifyTicks(DateTime[] ticks, object info)
		{
			bool flag = ticks[0].Ticks > 0L;
			if (flag)
			{
				bool flag2 = info.Equals(DifferenceIn.Minute);
				if (flag2)
				{
					return ChartCustomTicksProvider.SetMinutesTicks(ticks);
				}
				bool flag3 = info.Equals(DifferenceIn.Hour);
				if (flag3)
				{
					return ChartCustomTicksProvider.SetHoursTicks(ticks);
				}
				bool flag4 = info.Equals(DifferenceIn.Day);
				if (flag4)
				{
					return ChartCustomTicksProvider.SetDaysTicks(ticks);
				}
				bool flag5 = info.Equals(DifferenceIn.Month);
				if (flag5)
				{
					return ChartCustomTicksProvider.SetMonthTicks(ticks);
				}
			}
			return base.ModifyTicks(ticks, info);
		}

		// Token: 0x06001B29 RID: 6953 RVA: 0x000A316C File Offset: 0x000A136C
		private static DateTime[] SetMonthTicks(DateTime[] ticks)
		{
			TimeSpan timeSpan = ticks[ticks.ToList<DateTime>().Count - 1].Subtract(ticks[0]);
			int num = 0;
			int num2 = 1;
			bool flag = timeSpan.TotalDays > 30.0 && timeSpan.TotalDays <= 60.0;
			if (flag)
			{
				num2 = 5;
				num = timeSpan.Days / num2;
			}
			else
			{
				bool flag2 = timeSpan.TotalDays > 60.0 && timeSpan.TotalDays <= 120.0;
				if (flag2)
				{
					num2 = 15;
					num = timeSpan.Days / num2;
				}
				else
				{
					bool flag3 = timeSpan.TotalDays > 120.0 && timeSpan.TotalDays <= 240.0;
					if (flag3)
					{
						num2 = 30;
						num = timeSpan.Days / num2;
					}
					else
					{
						bool flag4 = timeSpan.TotalDays > 240.0 && timeSpan.TotalDays <= 360.0;
						if (flag4)
						{
							num2 = 60;
							num = timeSpan.Days / num2;
						}
						else
						{
							bool flag5 = timeSpan.TotalDays > 360.0;
							if (flag5)
							{
								num2 = 120;
								num = timeSpan.Days / num2;
							}
						}
					}
				}
			}
			DateTime[] array = new DateTime[num + 2];
			array[0] = ticks[0];
			for (int i = 1; i < array.ToList<DateTime>().Count; i++)
			{
				array[i] = array[i - 1].AddDays((double)num2);
			}
			return array;
		}

		// Token: 0x06001B2A RID: 6954 RVA: 0x000A3328 File Offset: 0x000A1528
		private static DateTime[] SetDaysTicks(DateTime[] ticks)
		{
			TimeSpan timeSpan = ticks[ticks.ToList<DateTime>().Count - 1].Subtract(ticks[0]);
			int num = 0;
			int num2 = 1;
			bool flag = false;
			bool flag2 = false;
			bool flag3 = timeSpan.TotalMinutes > 1440.0 && timeSpan.TotalMinutes <= 2880.0;
			if (flag3)
			{
				num2 = 240;
				num = (int)timeSpan.TotalMinutes / num2;
				flag = true;
				flag2 = false;
			}
			else
			{
				bool flag4 = timeSpan.TotalMinutes > 2880.0 && timeSpan.TotalMinutes <= 7200.0;
				if (flag4)
				{
					num2 = 720;
					num = (int)timeSpan.TotalMinutes / num2;
					flag = true;
					flag2 = false;
				}
				else
				{
					bool flag5 = timeSpan.TotalDays > 5.0 && timeSpan.TotalDays <= 14.0;
					if (flag5)
					{
						num2 = 1;
						num = (int)timeSpan.TotalDays / num2;
						flag = false;
						flag2 = true;
					}
					else
					{
						bool flag6 = timeSpan.TotalDays > 14.0 && timeSpan.TotalDays <= 30.0;
						if (flag6)
						{
							num2 = 2;
							num = (int)timeSpan.TotalDays / num2;
							flag = false;
							flag2 = true;
						}
						else
						{
							bool flag7 = timeSpan.TotalDays > 30.0;
							if (flag7)
							{
								num2 = 2;
								num = (int)timeSpan.TotalDays / num2;
								flag = false;
								flag2 = true;
							}
						}
					}
				}
			}
			DateTime[] array = new DateTime[num + 2];
			array[0] = ticks[0];
			bool flag8 = flag && !flag2;
			if (flag8)
			{
				for (int i = 1; i < array.ToList<DateTime>().Count; i++)
				{
					array[i] = array[i - 1].AddMinutes((double)num2);
				}
			}
			else
			{
				bool flag9 = !flag && flag2;
				if (flag9)
				{
					for (int j = 1; j < array.ToList<DateTime>().Count; j++)
					{
						array[j] = array[j - 1].AddDays((double)num2);
					}
				}
			}
			return array;
		}

		// Token: 0x06001B2B RID: 6955 RVA: 0x000A3574 File Offset: 0x000A1774
		private static DateTime[] SetHoursTicks(DateTime[] ticks)
		{
			TimeSpan timeSpan = ticks[ticks.ToList<DateTime>().Count - 1].Subtract(ticks[0]);
			int num = 0;
			int num2 = 1;
			bool flag = timeSpan.TotalMinutes > 60.0 && timeSpan.TotalMinutes <= 90.0;
			if (flag)
			{
				num2 = 10;
				num = (int)timeSpan.TotalMinutes / num2;
			}
			else
			{
				bool flag2 = timeSpan.TotalMinutes > 90.0 && timeSpan.TotalMinutes <= 120.0;
				if (flag2)
				{
					num2 = 15;
					num = (int)timeSpan.TotalMinutes / num2;
				}
				else
				{
					bool flag3 = timeSpan.TotalMinutes > 120.0 && timeSpan.TotalMinutes <= 200.0;
					if (flag3)
					{
						num2 = 20;
						num = (int)timeSpan.TotalMinutes / num2;
					}
					else
					{
						bool flag4 = timeSpan.TotalMinutes > 200.0 && timeSpan.TotalMinutes <= 300.0;
						if (flag4)
						{
							num2 = 30;
							num = (int)timeSpan.TotalMinutes / num2;
						}
						else
						{
							bool flag5 = timeSpan.TotalMinutes > 300.0 && timeSpan.TotalMinutes <= 540.0;
							if (flag5)
							{
								num2 = 60;
								num = (int)timeSpan.TotalMinutes / num2;
							}
							else
							{
								bool flag6 = timeSpan.TotalMinutes > 540.0 && timeSpan.TotalMinutes <= 720.0;
								if (flag6)
								{
									num2 = 90;
									num = (int)timeSpan.TotalMinutes / num2;
								}
								else
								{
									bool flag7 = timeSpan.TotalMinutes > 720.0 && timeSpan.TotalMinutes <= 960.0;
									if (flag7)
									{
										num2 = 120;
										num = (int)timeSpan.TotalMinutes / num2;
									}
									else
									{
										bool flag8 = timeSpan.TotalMinutes > 960.0 && timeSpan.TotalMinutes <= 1440.0;
										if (flag8)
										{
											num2 = 180;
											num = (int)timeSpan.TotalMinutes / num2;
										}
										else
										{
											bool flag9 = timeSpan.TotalMinutes > 1440.0;
											if (flag9)
											{
												num2 = 240;
												num = (int)timeSpan.TotalMinutes / num2;
											}
										}
									}
								}
							}
						}
					}
				}
			}
			DateTime[] array = new DateTime[num + 2];
			array[0] = ticks[0];
			for (int i = 1; i < array.ToList<DateTime>().Count; i++)
			{
				array[i] = array[i - 1].AddMinutes((double)num2);
			}
			return array;
		}

		// Token: 0x06001B2C RID: 6956 RVA: 0x000A3850 File Offset: 0x000A1A50
		private static DateTime[] SetMinutesTicks(DateTime[] ticks)
		{
			TimeSpan timeSpan = ticks[ticks.ToList<DateTime>().Count - 1].Subtract(ticks[0]);
			int num = 0;
			int num2 = 1;
			bool flag = false;
			bool flag2 = timeSpan.TotalMinutes <= 3.0;
			if (flag2)
			{
				flag = true;
				num2 = 30;
				num = (int)timeSpan.TotalSeconds / num2;
			}
			else
			{
				bool flag3 = timeSpan.TotalMinutes > 3.0 && timeSpan.TotalMinutes <= 6.0;
				if (flag3)
				{
					num2 = 1;
					num = (int)timeSpan.TotalMinutes / num2;
					flag = false;
				}
				else
				{
					bool flag4 = timeSpan.TotalMinutes > 6.0 && timeSpan.TotalMinutes <= 12.0;
					if (flag4)
					{
						num2 = 2;
						num = (int)timeSpan.TotalMinutes / num2;
						flag = false;
					}
					else
					{
						bool flag5 = timeSpan.TotalMinutes > 12.0 && timeSpan.TotalMinutes <= 24.0;
						if (flag5)
						{
							num2 = 3;
							num = (int)timeSpan.TotalMinutes / num2;
							flag = false;
						}
						else
						{
							bool flag6 = timeSpan.TotalMinutes > 24.0 && timeSpan.TotalMinutes <= 45.0;
							if (flag6)
							{
								num2 = 5;
								num = (int)timeSpan.TotalMinutes / num2;
								flag = false;
							}
							else
							{
								bool flag7 = timeSpan.TotalMinutes > 45.0 && timeSpan.TotalMinutes < 60.0;
								if (flag7)
								{
									num2 = 6;
									num = (int)timeSpan.TotalMinutes / num2;
									flag = false;
								}
								else
								{
									bool flag8 = timeSpan.TotalMinutes >= 60.0;
									if (flag8)
									{
										num2 = 10;
										num = (int)timeSpan.TotalMinutes / num2;
										flag = false;
									}
								}
							}
						}
					}
				}
			}
			DateTime[] array = new DateTime[num + 2];
			array[0] = ticks[0];
			bool flag9 = flag;
			if (flag9)
			{
				for (int i = 1; i < array.ToList<DateTime>().Count; i++)
				{
					array[i] = array[i - 1].AddSeconds((double)num2);
				}
			}
			else
			{
				for (int j = 1; j < array.ToList<DateTime>().Count; j++)
				{
					array[j] = array[j - 1].AddMinutes((double)num2);
				}
			}
			return array;
		}
	}
}
