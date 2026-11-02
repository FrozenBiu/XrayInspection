using System;
using System.ComponentModel;
using System.Diagnostics;
using Common;
using DatabaseConnection;

namespace GUI_XRS.Datalogger
{
	// Token: 0x02000266 RID: 614
	public class DataloggerSelectionViewModel : INotifyPropertyChanged
	{
		// Token: 0x17000C83 RID: 3203
		// (get) Token: 0x06002EF0 RID: 12016 RVA: 0x0013398C File Offset: 0x00131B8C
		// (set) Token: 0x06002EF1 RID: 12017 RVA: 0x00133994 File Offset: 0x00131B94
		private Logger datalogger { get; set; }

		// Token: 0x1400010C RID: 268
		// (add) Token: 0x06002EF2 RID: 12018 RVA: 0x001339A0 File Offset: 0x00131BA0
		// (remove) Token: 0x06002EF3 RID: 12019 RVA: 0x001339D8 File Offset: 0x00131BD8
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x17000C84 RID: 3204
		// (get) Token: 0x06002EF4 RID: 12020 RVA: 0x00133A0D File Offset: 0x00131C0D
		// (set) Token: 0x06002EF5 RID: 12021 RVA: 0x00133A15 File Offset: 0x00131C15
		public RelayCommand CleanUpCommand { get; set; }

		// Token: 0x17000C85 RID: 3205
		// (get) Token: 0x06002EF6 RID: 12022 RVA: 0x00133A1E File Offset: 0x00131C1E
		// (set) Token: 0x06002EF7 RID: 12023 RVA: 0x00133A26 File Offset: 0x00131C26
		public RelayCommand DataloggerManualCommand { get; set; }

		// Token: 0x17000C86 RID: 3206
		// (get) Token: 0x06002EF8 RID: 12024 RVA: 0x00133A2F File Offset: 0x00131C2F
		// (set) Token: 0x06002EF9 RID: 12025 RVA: 0x00133A37 File Offset: 0x00131C37
		public RelayCommand DataloggerAutomaticCommand { get; set; }

		// Token: 0x06002EFA RID: 12026 RVA: 0x00133A40 File Offset: 0x00131C40
		public DataloggerSelectionViewModel(Logger logger, DBConnection dbConnection, ParameterHandling parHandling, DBDataLogs dbDatalogs)
		{
			this.dbConnection = dbConnection;
			this.parHandling = parHandling;
			this.datalogger = logger;
			this.dbDatalogs = dbDatalogs;
			this.CleanUpCommand = new RelayCommand(new Action<object>(this.CleanUp));
			this.DataloggerManualCommand = new RelayCommand(new Action<object>(this.cmdDataloggerManual_Click));
			this.DataloggerAutomaticCommand = new RelayCommand(new Action<object>(this.cmdDataloggerAutomatic_Click));
		}

		// Token: 0x06002EFB RID: 12027 RVA: 0x00133ABC File Offset: 0x00131CBC
		private void cmdDataloggerManual_Click(object obj)
		{
			CtrlDatalogger ctrlDatalogger = new CtrlDatalogger(this.parHandling, this.datalogger, this.dbConnection);
			Subwindow.Show(ctrlDatalogger, SubwindowButtons.BackAndHome, ctrlDatalogger.Buttons);
		}

		// Token: 0x06002EFC RID: 12028 RVA: 0x00133AF0 File Offset: 0x00131CF0
		private void cmdDataloggerAutomatic_Click(object obj)
		{
			CtrlDatalogsPredMain ctrlDatalogsPredMain = new CtrlDatalogsPredMain(this.dbDatalogs, this.dbConnection, this.parHandling);
			Subwindow.Show(ctrlDatalogsPredMain, SubwindowButtons.BackAndHome, ctrlDatalogsPredMain.PanelButtons);
		}

		// Token: 0x06002EFD RID: 12029 RVA: 0x00133B24 File Offset: 0x00131D24
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x06002EFE RID: 12030 RVA: 0x0001092C File Offset: 0x0000EB2C
		private void CleanUp(object obj)
		{
		}

		// Token: 0x04001D70 RID: 7536
		private ParameterHandling parHandling;

		// Token: 0x04001D72 RID: 7538
		private DBConnection dbConnection;

		// Token: 0x04001D73 RID: 7539
		private DBDataLogs dbDatalogs;
	}
}
