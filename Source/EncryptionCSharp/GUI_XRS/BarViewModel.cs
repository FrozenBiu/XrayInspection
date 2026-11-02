using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

namespace GUI_XRS
{
	// Token: 0x02000156 RID: 342
	public class BarViewModel : INotifyPropertyChanged
	{
		// Token: 0x1700096C RID: 2412
		// (get) Token: 0x0600208A RID: 8330 RVA: 0x000D4840 File Offset: 0x000D2A40
		// (set) Token: 0x0600208B RID: 8331 RVA: 0x000D4858 File Offset: 0x000D2A58
		public double CurrentValue
		{
			get
			{
				return this.currentValue;
			}
			set
			{
				this.currentValue = value;
				bool flag = this.BarItems != null;
				if (flag)
				{
					foreach (BarItem barItem in this.BarItems)
					{
						barItem.Active = (barItem.Value <= this.currentValue);
					}
				}
				this.OnPropertyChanged("CurrentValue");
			}
		}

		// Token: 0x1700096D RID: 2413
		// (get) Token: 0x0600208C RID: 8332 RVA: 0x000D48E4 File Offset: 0x000D2AE4
		// (set) Token: 0x0600208D RID: 8333 RVA: 0x000D48FC File Offset: 0x000D2AFC
		public double MinValue
		{
			get
			{
				return this.minValue;
			}
			set
			{
				this.minValue = value;
				this.OnPropertyChanged("MinValue");
			}
		}

		// Token: 0x1700096E RID: 2414
		// (get) Token: 0x0600208E RID: 8334 RVA: 0x000D4914 File Offset: 0x000D2B14
		// (set) Token: 0x0600208F RID: 8335 RVA: 0x000D492C File Offset: 0x000D2B2C
		public double MaxValue
		{
			get
			{
				return this.maxValue;
			}
			set
			{
				this.maxValue = value;
				this.OnPropertyChanged("MaxValue");
			}
		}

		// Token: 0x1700096F RID: 2415
		// (get) Token: 0x06002090 RID: 8336 RVA: 0x000D4944 File Offset: 0x000D2B44
		// (set) Token: 0x06002091 RID: 8337 RVA: 0x000D495C File Offset: 0x000D2B5C
		public double FirstUsedValue
		{
			get
			{
				return this.firstUsedValue;
			}
			set
			{
				this.firstUsedValue = value;
				this.OnPropertyChanged("FirstUsedValue");
			}
		}

		// Token: 0x17000970 RID: 2416
		// (get) Token: 0x06002092 RID: 8338 RVA: 0x000D4974 File Offset: 0x000D2B74
		// (set) Token: 0x06002093 RID: 8339 RVA: 0x000D498C File Offset: 0x000D2B8C
		public double LastUsedValue
		{
			get
			{
				return this.lastUsedValue;
			}
			set
			{
				this.lastUsedValue = value;
				this.OnPropertyChanged("LastUsedValue");
			}
		}

		// Token: 0x17000971 RID: 2417
		// (get) Token: 0x06002094 RID: 8340 RVA: 0x000D49A4 File Offset: 0x000D2BA4
		// (set) Token: 0x06002095 RID: 8341 RVA: 0x000D49BC File Offset: 0x000D2BBC
		public double MinTicks
		{
			get
			{
				return this.minTicks;
			}
			set
			{
				this.minTicks = value;
				this.OnPropertyChanged("MinTicks");
			}
		}

		// Token: 0x17000972 RID: 2418
		// (get) Token: 0x06002096 RID: 8342 RVA: 0x000D49D4 File Offset: 0x000D2BD4
		// (set) Token: 0x06002097 RID: 8343 RVA: 0x000D49EC File Offset: 0x000D2BEC
		public double MaxTicks
		{
			get
			{
				return this.maxTicks;
			}
			set
			{
				this.maxTicks = value;
				this.OnPropertyChanged("MaxTicks");
			}
		}

		// Token: 0x17000973 RID: 2419
		// (get) Token: 0x06002098 RID: 8344 RVA: 0x000D4A02 File Offset: 0x000D2C02
		// (set) Token: 0x06002099 RID: 8345 RVA: 0x000D4A0A File Offset: 0x000D2C0A
		public List<BarItem> BarItems { get; set; }

		// Token: 0x140000AB RID: 171
		// (add) Token: 0x0600209A RID: 8346 RVA: 0x000D4A14 File Offset: 0x000D2C14
		// (remove) Token: 0x0600209B RID: 8347 RVA: 0x000D4A4C File Offset: 0x000D2C4C
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x0600209C RID: 8348 RVA: 0x000D4A84 File Offset: 0x000D2C84
		public BarViewModel(double minValue, double maxValue, double firstUsedValue, double lastUsedValue, double minTicks, double maxTicks, double steps)
		{
			this.minValue = minValue;
			this.maxValue = Math.Round(maxValue, 2, MidpointRounding.AwayFromZero);
			this.firstUsedValue = Math.Round(firstUsedValue, 2, MidpointRounding.AwayFromZero);
			this.lastUsedValue = Math.Round(lastUsedValue, 2, MidpointRounding.AwayFromZero);
			this.minTicks = Math.Round(minTicks, 2, MidpointRounding.AwayFromZero);
			this.maxTicks = Math.Round(maxTicks, 2, MidpointRounding.AwayFromZero);
			this.steps = Math.Round(steps, 2, MidpointRounding.AwayFromZero);
			this.Initialize();
		}

		// Token: 0x0600209D RID: 8349 RVA: 0x000D4B0C File Offset: 0x000D2D0C
		private void Initialize()
		{
			int num = -1;
			bool flag = this.BarItems == null;
			if (flag)
			{
				this.BarItems = new List<BarItem>();
			}
			else
			{
				this.BarItems.Clear();
			}
			for (double num2 = this.maxValue; num2 >= this.minValue; num2 -= this.steps)
			{
				num2 = Math.Round(num2, 2);
				BarItem barItem = new BarItem(num2);
				bool flag2 = num2 == this.lastUsedValue;
				if (flag2)
				{
					barItem.IsFirstItem = true;
				}
				bool flag3 = num2 == this.minValue;
				if (flag3)
				{
					barItem.IsLastItem = true;
				}
				bool flag4 = num2 == this.minValue || num2 >= this.firstUsedValue;
				if (flag4)
				{
					bool flag5 = (double)num % this.minTicks == 0.0;
					if (flag5)
					{
						barItem.IsMarker = true;
						bool flag6 = (double)num % this.maxTicks != 0.0;
						if (flag6)
						{
							barItem.IsShortMarker = true;
						}
					}
				}
				barItem.ItemHeight = ((this.maxValue > 0.0) ? Math.Round((double)this.barHeight * this.steps / this.maxValue, 0) : 1.0);
				this.BarItems.Add(barItem);
				num++;
			}
		}

		// Token: 0x0600209E RID: 8350 RVA: 0x000D4C64 File Offset: 0x000D2E64
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x04001524 RID: 5412
		private double currentValue;

		// Token: 0x04001525 RID: 5413
		private double minValue;

		// Token: 0x04001526 RID: 5414
		private double maxValue;

		// Token: 0x04001527 RID: 5415
		private double firstUsedValue;

		// Token: 0x04001528 RID: 5416
		private double lastUsedValue;

		// Token: 0x04001529 RID: 5417
		private double minTicks;

		// Token: 0x0400152A RID: 5418
		private double maxTicks;

		// Token: 0x0400152B RID: 5419
		private int barHeight = 240;

		// Token: 0x0400152C RID: 5420
		private double steps;
	}
}
