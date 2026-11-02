using System;
using Common;
using DatabaseConnection;

namespace GUI_XRS
{
	// Token: 0x02000041 RID: 65
	public class ActivatorViewModel
	{
		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x060006AA RID: 1706 RVA: 0x0002C2E1 File Offset: 0x0002A4E1
		// (set) Token: 0x060006AB RID: 1707 RVA: 0x0002C2E9 File Offset: 0x0002A4E9
		public Activator Activator { get; set; }

		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x060006AC RID: 1708 RVA: 0x0002C2F2 File Offset: 0x0002A4F2
		// (set) Token: 0x060006AD RID: 1709 RVA: 0x0002C2FA File Offset: 0x0002A4FA
		public RelayCommand SetOnTimeMinutesCommand { get; set; }

		// Token: 0x170001FA RID: 506
		// (get) Token: 0x060006AE RID: 1710 RVA: 0x0002C303 File Offset: 0x0002A503
		// (set) Token: 0x060006AF RID: 1711 RVA: 0x0002C30B File Offset: 0x0002A50B
		public RelayCommand SetOnTimeSecondsCommand { get; set; }

		// Token: 0x170001FB RID: 507
		// (get) Token: 0x060006B0 RID: 1712 RVA: 0x0002C314 File Offset: 0x0002A514
		// (set) Token: 0x060006B1 RID: 1713 RVA: 0x0002C31C File Offset: 0x0002A51C
		public RelayCommand SetOffTimeMinutesCommand { get; set; }

		// Token: 0x170001FC RID: 508
		// (get) Token: 0x060006B2 RID: 1714 RVA: 0x0002C325 File Offset: 0x0002A525
		// (set) Token: 0x060006B3 RID: 1715 RVA: 0x0002C32D File Offset: 0x0002A52D
		public RelayCommand SetOffTimeSecondsCommand { get; set; }

		// Token: 0x060006B4 RID: 1716 RVA: 0x0002C338 File Offset: 0x0002A538
		public ActivatorViewModel(DBConnection dbConnection, ParameterHandling parHandling)
		{
			this.Activator = new Activator(dbConnection, parHandling);
			this.SetOnTimeMinutesCommand = new RelayCommand(new Action<object>(this.SetOnTimeMinutes));
			this.SetOnTimeSecondsCommand = new RelayCommand(new Action<object>(this.SetOnTimeSeconds));
			this.SetOffTimeMinutesCommand = new RelayCommand(new Action<object>(this.SetOffTimeMinutes));
			this.SetOffTimeSecondsCommand = new RelayCommand(new Action<object>(this.SetOffTimeSeconds));
		}

		// Token: 0x060006B5 RID: 1717 RVA: 0x0002C3BC File Offset: 0x0002A5BC
		private void SetOnTimeMinutes(object obj)
		{
			this.numKeyboard = new NumKeyboard("min", this.Activator.OnTimeMinutes.ToString(), NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_OnTimeMinutes;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x060006B6 RID: 1718 RVA: 0x0002C414 File Offset: 0x0002A614
		private void numKeyboard_OnTimeMinutes(object sender, KeyboardInputEventArgs e)
		{
			int onTimeMinutes = 0;
			try
			{
				this.numKeyboard.TextInputKeyboard -= this.numKeyboard_OnTimeMinutes;
				bool flag = int.TryParse(e.Text, out onTimeMinutes);
				if (flag)
				{
					this.Activator.OnTimeMinutes = onTimeMinutes;
				}
			}
			catch
			{
			}
		}

		// Token: 0x060006B7 RID: 1719 RVA: 0x0002C474 File Offset: 0x0002A674
		private void SetOnTimeSeconds(object obj)
		{
			this.numKeyboard = new NumKeyboard("s", this.Activator.OnTimeSeconds.ToString(), NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_OnTimeSeconds;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x060006B8 RID: 1720 RVA: 0x0002C4CC File Offset: 0x0002A6CC
		private void numKeyboard_OnTimeSeconds(object sender, KeyboardInputEventArgs e)
		{
			int onTimeSeconds = 0;
			try
			{
				this.numKeyboard.TextInputKeyboard -= this.numKeyboard_OnTimeSeconds;
				bool flag = int.TryParse(e.Text, out onTimeSeconds);
				if (flag)
				{
					this.Activator.OnTimeSeconds = onTimeSeconds;
				}
			}
			catch
			{
			}
		}

		// Token: 0x060006B9 RID: 1721 RVA: 0x0002C52C File Offset: 0x0002A72C
		private void SetOffTimeMinutes(object obj)
		{
			this.numKeyboard = new NumKeyboard("min", this.Activator.OffTimeMinutes.ToString(), NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_OffTimeMinutes;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x060006BA RID: 1722 RVA: 0x0002C584 File Offset: 0x0002A784
		private void numKeyboard_OffTimeMinutes(object sender, KeyboardInputEventArgs e)
		{
			int offTimeMinutes = 0;
			try
			{
				this.numKeyboard.TextInputKeyboard -= this.numKeyboard_OffTimeMinutes;
				bool flag = int.TryParse(e.Text, out offTimeMinutes);
				if (flag)
				{
					this.Activator.OffTimeMinutes = offTimeMinutes;
				}
			}
			catch
			{
			}
		}

		// Token: 0x060006BB RID: 1723 RVA: 0x0002C5E4 File Offset: 0x0002A7E4
		private void SetOffTimeSeconds(object obj)
		{
			this.numKeyboard = new NumKeyboard("s", this.Activator.OffTimeSeconds.ToString(), NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_OffTimeSeconds;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x060006BC RID: 1724 RVA: 0x0002C63C File Offset: 0x0002A83C
		private void numKeyboard_OffTimeSeconds(object sender, KeyboardInputEventArgs e)
		{
			int offTimeSeconds = 0;
			try
			{
				this.numKeyboard.TextInputKeyboard -= this.numKeyboard_OffTimeSeconds;
				bool flag = int.TryParse(e.Text, out offTimeSeconds);
				if (flag)
				{
					this.Activator.OffTimeSeconds = offTimeSeconds;
				}
			}
			catch
			{
			}
		}

		// Token: 0x040004B0 RID: 1200
		private NumKeyboard numKeyboard;
	}
}
