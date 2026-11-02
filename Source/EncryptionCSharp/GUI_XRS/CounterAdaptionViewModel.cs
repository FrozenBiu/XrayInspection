using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Timers;
using DatabaseConnection;

namespace GUI_XRS
{
	// Token: 0x02000090 RID: 144
	public class CounterAdaptionViewModel : INotifyPropertyChanged
	{
		// Token: 0x1700047B RID: 1147
		// (get) Token: 0x06000EAE RID: 3758 RVA: 0x00059057 File Offset: 0x00057257
		// (set) Token: 0x06000EAF RID: 3759 RVA: 0x0005905F File Offset: 0x0005725F
		public MachineState machineState { get; set; }

		// Token: 0x1700047C RID: 1148
		// (get) Token: 0x06000EB0 RID: 3760 RVA: 0x00059068 File Offset: 0x00057268
		// (set) Token: 0x06000EB1 RID: 3761 RVA: 0x00059070 File Offset: 0x00057270
		public bool DemoMode { get; set; }

		// Token: 0x1400004E RID: 78
		// (add) Token: 0x06000EB2 RID: 3762 RVA: 0x0005907C File Offset: 0x0005727C
		// (remove) Token: 0x06000EB3 RID: 3763 RVA: 0x000590B4 File Offset: 0x000572B4
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x1700047D RID: 1149
		// (get) Token: 0x06000EB4 RID: 3764 RVA: 0x000590EC File Offset: 0x000572EC
		// (set) Token: 0x06000EB5 RID: 3765 RVA: 0x00059104 File Offset: 0x00057304
		public bool ShowNext
		{
			get
			{
				return this.showNext;
			}
			set
			{
				this.showNext = value;
				this.OnPropertyChanged("ShowNext");
			}
		}

		// Token: 0x06000EB6 RID: 3766 RVA: 0x0005911C File Offset: 0x0005731C
		public CounterAdaptionViewModel(MachineState machineState, DBConnection dbConnection)
		{
			this.machineState = machineState;
			bool demoMode = false;
			dbConnection.GetDemoMode(ref demoMode);
			this.DemoMode = demoMode;
		}

		// Token: 0x06000EB7 RID: 3767 RVA: 0x00059154 File Offset: 0x00057354
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x06000EB8 RID: 3768 RVA: 0x00059181 File Offset: 0x00057381
		private void highSpeedTimer_Elapsed(object sender, ElapsedEventArgs e)
		{
			this.ShowNext = false;
		}

		// Token: 0x06000EB9 RID: 3769 RVA: 0x0001092C File Offset: 0x0000EB2C
		internal void CleanUp()
		{
		}

		// Token: 0x040009B7 RID: 2487
		private bool showNext = false;
	}
}
