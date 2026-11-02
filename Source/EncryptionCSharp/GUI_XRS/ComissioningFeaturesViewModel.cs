using System;
using System.ComponentModel;
using System.Diagnostics;
using Common;
using DatabaseConnection;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x0200005A RID: 90
	public class ComissioningFeaturesViewModel : INotifyPropertyChanged
	{
		// Token: 0x14000034 RID: 52
		// (add) Token: 0x060008EF RID: 2287 RVA: 0x00038058 File Offset: 0x00036258
		// (remove) Token: 0x060008F0 RID: 2288 RVA: 0x00038090 File Offset: 0x00036290
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x1700029A RID: 666
		// (get) Token: 0x060008F1 RID: 2289 RVA: 0x000380C5 File Offset: 0x000362C5
		// (set) Token: 0x060008F2 RID: 2290 RVA: 0x000380CD File Offset: 0x000362CD
		public RelayCommand CleanUpCommand { get; set; }

		// Token: 0x1700029B RID: 667
		// (get) Token: 0x060008F3 RID: 2291 RVA: 0x000380D8 File Offset: 0x000362D8
		// (set) Token: 0x060008F4 RID: 2292 RVA: 0x000380F0 File Offset: 0x000362F0
		public bool IVP
		{
			get
			{
				return this.ivp;
			}
			set
			{
				bool flag = this.ivp != value;
				if (flag)
				{
					this.ivp = value;
					string value2 = this.ivp ? "1" : "0";
					this.parHandling.WriteValue(Definition.iVPPath, value2);
					this.OnPropertyChanged("IVP");
				}
			}
		}

		// Token: 0x1700029C RID: 668
		// (get) Token: 0x060008F5 RID: 2293 RVA: 0x0003814C File Offset: 0x0003634C
		// (set) Token: 0x060008F6 RID: 2294 RVA: 0x00038164 File Offset: 0x00036364
		public bool ICM
		{
			get
			{
				return this.icm;
			}
			set
			{
				bool flag = this.icm != value;
				if (flag)
				{
					this.icm = value;
					string text = this.icm ? "1" : "0";
					this.parHandling.WriteValue(Definition.iCMPath, text);
					this.writeToActProdAndDefault(Definition.simulationAfterTimeIntervall, text);
					bool flag2 = this.icm;
					if (flag2)
					{
						this.parHandling.WriteValue(Definition.testcardSimulationPath, "1");
						this.writeToActProdAndDefault(Definition.simulationAfterAutolearn, "1");
						this.parHandling.WriteValue(Autotrain.nAutolearnPath, "10");
					}
					else
					{
						this.parHandling.WriteValue(Autotrain.nAutolearnPath, "5");
					}
					this.OnPropertyChanged("ICM");
				}
			}
		}

		// Token: 0x1700029D RID: 669
		// (get) Token: 0x060008F7 RID: 2295 RVA: 0x00038231 File Offset: 0x00036431
		// (set) Token: 0x060008F8 RID: 2296 RVA: 0x00038239 File Offset: 0x00036439
		public bool ICMIsVisible { get; set; }

		// Token: 0x1700029E RID: 670
		// (get) Token: 0x060008F9 RID: 2297 RVA: 0x00038242 File Offset: 0x00036442
		// (set) Token: 0x060008FA RID: 2298 RVA: 0x0003824A File Offset: 0x0003644A
		public bool IVPIsVisible { get; set; }

		// Token: 0x060008FB RID: 2299 RVA: 0x00038254 File Offset: 0x00036454
		public ComissioningFeaturesViewModel(DBConnection dbConnection, ParameterHandling parHandling)
		{
			this.parHandling = parHandling;
			this.dbConnection = dbConnection;
			bool flag = false;
			bool flag2 = false;
			this.ICMIsVisible = (parHandling.ReadPar(Definition.testcardSimulationPath) == "1");
			MachineMode machineMode = MachineMode.Standard;
			dbConnection.GetMachineMode(ref machineMode);
			bool flag3 = machineMode == MachineMode.Bulk;
			if (flag3)
			{
				this.IVPIsVisible = false;
			}
			else
			{
				this.IVPIsVisible = true;
			}
			dbConnection.GetIntegratedComplianceMonitoring(ref flag);
			dbConnection.GetIntegratedValidationProcess(ref flag2);
			this.IVP = flag2;
			this.ICM = flag;
			this.CleanUpCommand = new RelayCommand(new Action<object>(this.CleanUp));
		}

		// Token: 0x060008FC RID: 2300 RVA: 0x000382FC File Offset: 0x000364FC
		private void writeToActProdAndDefault(string path, string val)
		{
			this.parHandling.WriteValue(path, val);
			this.parHandling.WriteValue("/defaultProduct/mainTable" + path, val);
		}

		// Token: 0x060008FD RID: 2301 RVA: 0x00038328 File Offset: 0x00036528
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x060008FE RID: 2302 RVA: 0x0001092C File Offset: 0x0000EB2C
		private void CleanUp(object obj)
		{
		}

		// Token: 0x04000612 RID: 1554
		private ParameterHandling parHandling;

		// Token: 0x04000613 RID: 1555
		private DBConnection dbConnection;

		// Token: 0x04000616 RID: 1558
		private bool ivp;

		// Token: 0x04000617 RID: 1559
		private bool icm;
	}
}
