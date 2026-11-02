using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using Common;
using DatabaseConnection;

namespace GUI_XRS.Datalogger
{
	// Token: 0x02000268 RID: 616
	public class DatalogsPredMainResetsViewModel : INotifyPropertyChanged
	{
		// Token: 0x1400010D RID: 269
		// (add) Token: 0x06002F38 RID: 12088 RVA: 0x00133D30 File Offset: 0x00131F30
		// (remove) Token: 0x06002F39 RID: 12089 RVA: 0x00133D68 File Offset: 0x00131F68
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x17000CA3 RID: 3235
		// (get) Token: 0x06002F3A RID: 12090 RVA: 0x00133D9D File Offset: 0x00131F9D
		// (set) Token: 0x06002F3B RID: 12091 RVA: 0x00133DA5 File Offset: 0x00131FA5
		public RelayCommand ResetSelectedCounterCommand { get; set; }

		// Token: 0x17000CA4 RID: 3236
		// (get) Token: 0x06002F3C RID: 12092 RVA: 0x00133DAE File Offset: 0x00131FAE
		// (set) Token: 0x06002F3D RID: 12093 RVA: 0x00133DB6 File Offset: 0x00131FB6
		public BoundLocalizedText StrResetCounts { get; set; }

		// Token: 0x17000CA5 RID: 3237
		// (get) Token: 0x06002F3E RID: 12094 RVA: 0x00133DBF File Offset: 0x00131FBF
		// (set) Token: 0x06002F3F RID: 12095 RVA: 0x00133DC7 File Offset: 0x00131FC7
		public BoundLocalizedText StrConveyor { get; set; }

		// Token: 0x17000CA6 RID: 3238
		// (get) Token: 0x06002F40 RID: 12096 RVA: 0x00133DD0 File Offset: 0x00131FD0
		// (set) Token: 0x06002F41 RID: 12097 RVA: 0x00133DD8 File Offset: 0x00131FD8
		public BoundLocalizedText StrProducts { get; set; }

		// Token: 0x17000CA7 RID: 3239
		// (get) Token: 0x06002F42 RID: 12098 RVA: 0x00133DE1 File Offset: 0x00131FE1
		// (set) Token: 0x06002F43 RID: 12099 RVA: 0x00133DE9 File Offset: 0x00131FE9
		public BoundLocalizedText StrValves { get; set; }

		// Token: 0x17000CA8 RID: 3240
		// (get) Token: 0x06002F44 RID: 12100 RVA: 0x00133DF4 File Offset: 0x00131FF4
		// (set) Token: 0x06002F45 RID: 12101 RVA: 0x00133E0C File Offset: 0x0013200C
		public List<bool> SelectedTables
		{
			get
			{
				return this.selectedTables;
			}
			set
			{
				bool flag = this.selectedTables != value;
				if (flag)
				{
					this.selectedTables = value;
					this.OnPropertyChanged("SelectedTables");
				}
			}
		}

		// Token: 0x17000CA9 RID: 3241
		// (get) Token: 0x06002F46 RID: 12102 RVA: 0x00133E40 File Offset: 0x00132040
		// (set) Token: 0x06002F47 RID: 12103 RVA: 0x00133E58 File Offset: 0x00132058
		public List<bool> RejecterIsActive
		{
			get
			{
				return this.rejecterIsActive;
			}
			set
			{
				bool flag = this.rejecterIsActive != value;
				if (flag)
				{
					this.rejecterIsActive = value;
					this.OnPropertyChanged("RejecterIsActive");
				}
			}
		}

		// Token: 0x17000CAA RID: 3242
		// (get) Token: 0x06002F48 RID: 12104 RVA: 0x00133E8C File Offset: 0x0013208C
		// (set) Token: 0x06002F49 RID: 12105 RVA: 0x00133EA4 File Offset: 0x001320A4
		public bool SVTIsActive
		{
			get
			{
				return this.svtIsActive;
			}
			set
			{
				bool flag = this.svtIsActive != value;
				if (flag)
				{
					this.svtIsActive = value;
					this.OnPropertyChanged("SVTIsActive");
				}
			}
		}

		// Token: 0x17000CAB RID: 3243
		// (get) Token: 0x06002F4A RID: 12106 RVA: 0x00133ED7 File Offset: 0x001320D7
		// (set) Token: 0x06002F4B RID: 12107 RVA: 0x00133EDF File Offset: 0x001320DF
		public string NumValves { get; set; }

		// Token: 0x06002F4C RID: 12108 RVA: 0x00133EE8 File Offset: 0x001320E8
		public DatalogsPredMainResetsViewModel(DBDataLogs dBDataLogs, DBConnection dBConnection, ParameterHandling parHandling)
		{
			this.dBDataLogs = dBDataLogs;
			this.dBConnection = dBConnection;
			this.parHandling = parHandling;
			this.RegisterLocalizedText();
			this.Init();
		}

		// Token: 0x06002F4D RID: 12109 RVA: 0x00133F18 File Offset: 0x00132118
		private void Init()
		{
			this.SelectedTables = new List<bool>
			{
				false,
				false,
				false,
				false,
				false,
				false,
				false
			};
			bool flag = this.parHandling != null && this.dBConnection != null;
			if (flag)
			{
				this.RejecterIsActive = new List<bool>();
				bool flag2 = this.parHandling.ReadPar(FlapsOverIOs.bulkflapsEnabledPath).Equals("1");
				if (flag2)
				{
					int num = 0;
					string tableName = "";
					this.dBConnection.GetCurrentProductTableName(ref tableName);
					this.dBConnection.GetFlapsCount(tableName, ref num);
					bool flag3 = num == 1;
					if (flag3)
					{
						this.RejecterIsActive.Add(true);
						this.RejecterIsActive.Add(false);
						this.RejecterIsActive.Add(false);
						this.RejecterIsActive.Add(false);
					}
					else
					{
						this.RejecterIsActive.Add(true);
						this.RejecterIsActive.Add(true);
						this.RejecterIsActive.Add(true);
						this.RejecterIsActive.Add(true);
					}
				}
				else
				{
					bool flag4 = this.parHandling.ReadPar(SVT.svtEnablePath).Equals("1");
					if (flag4)
					{
						this.RejecterIsActive.Add(false);
						this.RejecterIsActive.Add(false);
						this.RejecterIsActive.Add(false);
						this.RejecterIsActive.Add(false);
						int num2 = 0;
						this.dBConnection.GetNumValves(ref num2);
						this.NumValves = num2.ToString();
						this.SVTIsActive = true;
					}
					else
					{
						this.SVTIsActive = false;
						this.RejecterIsActive = new List<bool>();
						bool item = this.parHandling.ReadPar("/hardware/rejecter/rejecter1/Enabled/Value") == "1";
						this.RejecterIsActive.Add(item);
						item = (this.parHandling.ReadPar("/hardware/rejecter/rejecter2/Enabled/Value") == "1");
						this.RejecterIsActive.Add(item);
						item = (this.parHandling.ReadPar("/hardware/rejecter/rejecter3/Enabled/Value") == "1");
						this.RejecterIsActive.Add(item);
						item = (this.parHandling.ReadPar("/hardware/rejecter/rejecter4/Enabled/Value") == "1");
						this.RejecterIsActive.Add(item);
					}
				}
			}
			this.ResetSelectedCounterCommand = new RelayCommand(new Action<object>(this.resetSelectedCounter));
		}

		// Token: 0x06002F4E RID: 12110 RVA: 0x001341CC File Offset: 0x001323CC
		private void resetSelectedCounter(object obj)
		{
			bool flag = false;
			for (int i = 0; i < this.SelectedTables.Count; i++)
			{
				bool flag2 = this.SelectedTables[i];
				if (flag2)
				{
					flag = true;
					break;
				}
			}
			bool flag3 = flag && Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtResetSelectedCounter"), DialogStyle.Warning, DialogButtons.OKCancel) == Dialog.DialogResults.Ok;
			if (flag3)
			{
				bool flag4 = this.SelectedTables[0];
				if (flag4)
				{
					this.dBDataLogs.ResetLogsConveyorData();
				}
				bool flag5 = this.SelectedTables[1];
				if (flag5)
				{
					this.dBDataLogs.ResetLogsProductsData();
				}
				bool flag6 = this.SelectedTables[2];
				if (flag6)
				{
					this.dBDataLogs.ResetLogsRejecterData("1");
				}
				bool flag7 = this.SelectedTables[3];
				if (flag7)
				{
					this.dBDataLogs.ResetLogsRejecterData("2");
				}
				bool flag8 = this.SelectedTables[4];
				if (flag8)
				{
					this.dBDataLogs.ResetLogsRejecterData("3");
				}
				bool flag9 = this.SelectedTables[5];
				if (flag9)
				{
					this.dBDataLogs.ResetLogsRejecterData("4");
				}
				bool flag10 = this.SelectedTables[6];
				if (flag10)
				{
					int number = 0;
					this.dBConnection.GetNumValves(ref number);
					this.dBDataLogs.ResetLogsAllValvesData(number);
				}
			}
			bool flag11 = !flag;
			if (flag11)
			{
				Dialog.Show(LocalizationManager.Localize("txtInfo"), LocalizationManager.Localize("txtNoCounterSelection"), DialogStyle.Info, DialogButtons.OK);
			}
		}

		// Token: 0x06002F4F RID: 12111 RVA: 0x00134360 File Offset: 0x00132560
		private void RegisterLocalizedText()
		{
			this.StrResetCounts = new BoundLocalizedText("txtResetSelectedCounter");
			this.StrConveyor = new BoundLocalizedText("txtConveyor");
			this.StrProducts = new BoundLocalizedText("txtProductCounter");
			this.StrValves = new BoundLocalizedText("txtValve");
		}

		// Token: 0x06002F50 RID: 12112 RVA: 0x001343B4 File Offset: 0x001325B4
		private void DeregisterLocalizedText()
		{
			MainWindow.localizationManager.Deregister(this.StrResetCounts);
			MainWindow.localizationManager.Deregister(this.StrConveyor);
			MainWindow.localizationManager.Deregister(this.StrProducts);
			MainWindow.localizationManager.Deregister(this.StrValves);
		}

		// Token: 0x06002F51 RID: 12113 RVA: 0x00134408 File Offset: 0x00132608
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x06002F52 RID: 12114 RVA: 0x00134435 File Offset: 0x00132635
		private void CleanUp(object obj)
		{
			this.DeregisterLocalizedText();
		}

		// Token: 0x04001D94 RID: 7572
		private DBConnection dBConnection;

		// Token: 0x04001D95 RID: 7573
		private DBDataLogs dBDataLogs;

		// Token: 0x04001D96 RID: 7574
		private ParameterHandling parHandling;

		// Token: 0x04001D9D RID: 7581
		private List<bool> selectedTables;

		// Token: 0x04001D9E RID: 7582
		private List<bool> rejecterIsActive;

		// Token: 0x04001D9F RID: 7583
		private bool svtIsActive;
	}
}
