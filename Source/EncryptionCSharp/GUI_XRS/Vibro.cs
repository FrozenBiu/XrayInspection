using System;
using System.ComponentModel;
using System.Diagnostics;
using Common;
using DatabaseConnection;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x02000058 RID: 88
	public class Vibro : INotifyPropertyChanged
	{
		// Token: 0x14000032 RID: 50
		// (add) Token: 0x060008C0 RID: 2240 RVA: 0x00037870 File Offset: 0x00035A70
		// (remove) Token: 0x060008C1 RID: 2241 RVA: 0x000378A8 File Offset: 0x00035AA8
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x1700028C RID: 652
		// (get) Token: 0x060008C2 RID: 2242 RVA: 0x000378E0 File Offset: 0x00035AE0
		// (set) Token: 0x060008C3 RID: 2243 RVA: 0x000378F8 File Offset: 0x00035AF8
		public int Speed
		{
			get
			{
				return this.speed;
			}
			set
			{
				bool flag = value > 100;
				if (flag)
				{
					this.speed = 100;
				}
				else
				{
					bool flag2 = value < 0;
					if (flag2)
					{
						this.speed = 0;
					}
					else
					{
						this.speed = value;
					}
				}
				int num = this.ComputeSpeedInBits();
				this.parHandling.WriteValue(Definition.vibroSpeedPath, num.ToString(), this.initialized);
				this.dbConnection.UpdateCurrentProductTable(Definition.vibroSpeedPath, num.ToString());
				this.OnPropertyChanged("Speed");
			}
		}

		// Token: 0x1700028D RID: 653
		// (get) Token: 0x060008C4 RID: 2244 RVA: 0x0003797C File Offset: 0x00035B7C
		// (set) Token: 0x060008C5 RID: 2245 RVA: 0x00037994 File Offset: 0x00035B94
		public bool VibroManualModeByGUI
		{
			get
			{
				return this.vibroManualModeByGUI;
			}
			set
			{
				bool flag = this.initialized;
				if (flag)
				{
					this.parHandling.WriteValue(Definition.vibroManualModeByGUIPath, value ? "1" : "0", false);
				}
				this.vibroManualModeByGUI = value;
				this.OnPropertyChanged("VibroManualModeByGUI");
				this.OnPropertyChanged("VibroManualModeByGUIInverted");
			}
		}

		// Token: 0x1700028E RID: 654
		// (get) Token: 0x060008C6 RID: 2246 RVA: 0x000379EC File Offset: 0x00035BEC
		public bool VibroManualModeByGUIInverted
		{
			get
			{
				return !this.vibroManualModeByGUI;
			}
		}

		// Token: 0x1700028F RID: 655
		// (get) Token: 0x060008C7 RID: 2247 RVA: 0x00037A07 File Offset: 0x00035C07
		// (set) Token: 0x060008C8 RID: 2248 RVA: 0x00037A0F File Offset: 0x00035C0F
		public RelayCommand StartVibroCommand { get; set; }

		// Token: 0x17000290 RID: 656
		// (get) Token: 0x060008C9 RID: 2249 RVA: 0x00037A18 File Offset: 0x00035C18
		// (set) Token: 0x060008CA RID: 2250 RVA: 0x00037A20 File Offset: 0x00035C20
		public RelayCommand StopVibroCommand { get; set; }

		// Token: 0x17000291 RID: 657
		// (get) Token: 0x060008CB RID: 2251 RVA: 0x00037A29 File Offset: 0x00035C29
		// (set) Token: 0x060008CC RID: 2252 RVA: 0x00037A31 File Offset: 0x00035C31
		public RelayCommand ChangeSpeedCommand { get; set; }

		// Token: 0x060008CD RID: 2253 RVA: 0x00037A3C File Offset: 0x00035C3C
		public Vibro(DBConnection dbConnection, ParameterHandling parHandling)
		{
			this.dbConnection = dbConnection;
			this.parHandling = parHandling;
			this.StartVibroCommand = new RelayCommand(new Action<object>(this.StartVibro));
			this.StopVibroCommand = new RelayCommand(new Action<object>(this.StopVibro));
			bool flag = dbConnection != null;
			if (flag)
			{
				try
				{
					this.UpdateSpeedFromDatabase();
				}
				catch
				{
				}
			}
			this.vibroManualModeByGUI = (parHandling.ReadPar(Definition.vibroManualModeByGUIPath) == "1");
			this.initialized = true;
		}

		// Token: 0x060008CE RID: 2254 RVA: 0x00037AE4 File Offset: 0x00035CE4
		private void StartVibro(object obj)
		{
			this.parHandling.WriteValue(Definition.vibroStartByGUIPath, "1", false);
		}

		// Token: 0x060008CF RID: 2255 RVA: 0x00037AFE File Offset: 0x00035CFE
		private void StopVibro(object obj)
		{
			this.parHandling.WriteValue(Definition.vibroStopByGUIPath, "1", false);
		}

		// Token: 0x060008D0 RID: 2256 RVA: 0x00037B18 File Offset: 0x00035D18
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x060008D1 RID: 2257 RVA: 0x00037B48 File Offset: 0x00035D48
		private int ComputeSpeedInBits()
		{
			return 32767 * this.speed / 100;
		}

		// Token: 0x060008D2 RID: 2258 RVA: 0x00037B70 File Offset: 0x00035D70
		public void UpdateSpeedFromDatabase()
		{
			string text = "";
			this.dbConnection.GetParameterValue(Definition.vibroSpeedPath, ref text);
			int num = 0;
			bool flag = int.TryParse(text, out num) || text == "";
			if (flag)
			{
				this.Speed = (int)Math.Round((double)(num * 100) / 32767.0);
			}
		}

		// Token: 0x040005FA RID: 1530
		private int speed;

		// Token: 0x040005FB RID: 1531
		private DBConnection dbConnection;

		// Token: 0x040005FC RID: 1532
		private ParameterHandling parHandling;

		// Token: 0x040005FD RID: 1533
		private bool vibroManualModeByGUI;

		// Token: 0x040005FE RID: 1534
		private bool initialized = false;
	}
}
