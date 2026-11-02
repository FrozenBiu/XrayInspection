using System;
using System.Globalization;

namespace GUI_XRS
{
	// Token: 0x0200015F RID: 351
	public class SystemTimeManager
	{
		// Token: 0x0600210B RID: 8459 RVA: 0x000D74AC File Offset: 0x000D56AC
		public void ChangeTime(SetFooterMainWindow setFooterMainWindow)
		{
			this.setFooterMainWindow = setFooterMainWindow;
			this.UpdateDateParameters();
			this.ChangeDateDay();
			this.UpdateDateParameters();
			this.ChangeDateMonth();
			this.UpdateDateParameters();
			this.ChangeDateYear();
			this.UpdateDateParameters();
			this.ChangeTimeHour();
			this.UpdateDateParameters();
			this.ChangeTimeMinute();
			LogbookManager.AddEntry(SSTEntryCode.SystemTimeChanged);
		}

		// Token: 0x0600210C RID: 8460 RVA: 0x000D7514 File Offset: 0x000D5714
		private void UpdateDateParameters()
		{
			DateTime now = DateTime.Now;
			this.dateDay = now.Day;
			this.dateMonth = now.Month;
			this.dateYear = now.Year;
			this.timeHour = now.Hour;
			this.timeMinute = now.Minute;
			this.timeSecond = now.Second;
		}

		// Token: 0x0600210D RID: 8461 RVA: 0x000D7578 File Offset: 0x000D5778
		private void ChangeDateDay()
		{
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtDay"), this.dateDay.ToString(), NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_InputDateDay;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x0600210E RID: 8462 RVA: 0x000D75CC File Offset: 0x000D57CC
		private void numKeyboard_InputDateDay(object sender, KeyboardInputEventArgs e)
		{
			try
			{
				this.dateDay = int.Parse(e.Text, new CultureInfo("de-DE"));
				DateTime time = new DateTime(this.dateYear, this.dateMonth, int.Parse(e.Text, new CultureInfo("de-DE")), this.timeHour, this.timeMinute, this.timeSecond);
				SystemTime.SetTime(time);
			}
			catch
			{
			}
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_InputDateDay;
		}

		// Token: 0x0600210F RID: 8463 RVA: 0x000D7668 File Offset: 0x000D5868
		private void ChangeDateMonth()
		{
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtMonth"), this.dateMonth.ToString(), NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_InputDateMonth;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x06002110 RID: 8464 RVA: 0x000D76BC File Offset: 0x000D58BC
		private void numKeyboard_InputDateMonth(object sender, KeyboardInputEventArgs e)
		{
			try
			{
				this.dateMonth = int.Parse(e.Text, new CultureInfo("de-DE"));
				DateTime time = new DateTime(this.dateYear, int.Parse(e.Text, new CultureInfo("de-DE")), this.dateDay, this.timeHour, this.timeMinute, this.timeSecond);
				SystemTime.SetTime(time);
			}
			catch
			{
			}
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_InputDateMonth;
		}

		// Token: 0x06002111 RID: 8465 RVA: 0x000D7758 File Offset: 0x000D5958
		private void ChangeDateYear()
		{
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtYear"), this.dateYear.ToString(), NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_InputDateYear;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x06002112 RID: 8466 RVA: 0x000D77AC File Offset: 0x000D59AC
		private void numKeyboard_InputDateYear(object sender, KeyboardInputEventArgs e)
		{
			try
			{
				this.dateYear = int.Parse(e.Text, new CultureInfo("de-DE"));
				DateTime time = new DateTime(int.Parse(e.Text, new CultureInfo("de-DE")), this.dateMonth, this.dateDay, this.timeHour, this.timeMinute, this.timeSecond);
				SystemTime.SetTime(time);
			}
			catch
			{
			}
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_InputDateYear;
		}

		// Token: 0x06002113 RID: 8467 RVA: 0x000D7848 File Offset: 0x000D5A48
		private void ChangeTimeHour()
		{
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtHour"), this.timeHour.ToString(), NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_InputTimeHour;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x06002114 RID: 8468 RVA: 0x000D789C File Offset: 0x000D5A9C
		private void numKeyboard_InputTimeHour(object sender, KeyboardInputEventArgs e)
		{
			try
			{
				this.timeHour = int.Parse(e.Text, new CultureInfo("de-DE"));
				DateTime time = new DateTime(this.dateYear, this.dateMonth, this.dateDay, int.Parse(e.Text, new CultureInfo("de-DE")), this.timeMinute, this.timeSecond);
				SystemTime.SetTime(time);
			}
			catch
			{
			}
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_InputTimeHour;
		}

		// Token: 0x06002115 RID: 8469 RVA: 0x000D7938 File Offset: 0x000D5B38
		private void ChangeTimeMinute()
		{
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtMinute"), this.timeMinute.ToString(), NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_InputTimeMinute;
			this.numKeyboard.ShowDialog();
			bool flag = this.setFooterMainWindow != null;
			if (flag)
			{
				this.setFooterMainWindow.UnSetFooterDeactivation();
			}
		}

		// Token: 0x06002116 RID: 8470 RVA: 0x000D79A4 File Offset: 0x000D5BA4
		private void numKeyboard_InputTimeMinute(object sender, KeyboardInputEventArgs e)
		{
			try
			{
				this.timeMinute = int.Parse(e.Text, new CultureInfo("de-DE"));
				DateTime time = new DateTime(this.dateYear, this.dateMonth, this.dateDay, this.timeHour, int.Parse(e.Text, new CultureInfo("de-DE")), this.timeSecond);
				SystemTime.SetTime(time);
			}
			catch
			{
			}
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_InputTimeMinute;
		}

		// Token: 0x04001565 RID: 5477
		private NumKeyboard numKeyboard;

		// Token: 0x04001566 RID: 5478
		private int dateDay;

		// Token: 0x04001567 RID: 5479
		private int dateMonth;

		// Token: 0x04001568 RID: 5480
		private int dateYear;

		// Token: 0x04001569 RID: 5481
		private int timeHour;

		// Token: 0x0400156A RID: 5482
		private int timeMinute;

		// Token: 0x0400156B RID: 5483
		private int timeSecond;

		// Token: 0x0400156C RID: 5484
		private SetFooterMainWindow setFooterMainWindow;
	}
}
