using System;
using System.Diagnostics;
using System.Timers;
using DatabaseConnection;

namespace GUI_XRS
{
	// Token: 0x0200007D RID: 125
	public class IVPTimer
	{
		// Token: 0x14000042 RID: 66
		// (add) Token: 0x06000C63 RID: 3171 RVA: 0x0004A1A0 File Offset: 0x000483A0
		// (remove) Token: 0x06000C64 RID: 3172 RVA: 0x0004A1D8 File Offset: 0x000483D8
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action NextIVPElapsed;

		// Token: 0x170003C6 RID: 966
		// (get) Token: 0x06000C65 RID: 3173 RVA: 0x0004A210 File Offset: 0x00048410
		// (set) Token: 0x06000C66 RID: 3174 RVA: 0x0004A228 File Offset: 0x00048428
		public bool IVPIsEnabled
		{
			get
			{
				return this.ivpIsEnabled;
			}
			set
			{
				this.ivpIsEnabled = value;
				bool flag = this.ivpIsEnabled;
				if (flag)
				{
					bool flag2 = !this.isStarted;
					if (flag2)
					{
						this.Disable();
						this.StartTimer();
					}
				}
				else
				{
					this.Disable();
				}
			}
		}

		// Token: 0x06000C67 RID: 3175 RVA: 0x0004A274 File Offset: 0x00048474
		public IVPTimer(DBConnection dbConnection)
		{
			this.dbConnection = dbConnection;
			this.timerNextCheck = new Timer();
			this.timerNextCheck.Elapsed += this.timerNextCheck_Elapsed;
			this.timerNextCheck.Interval = 86400000.0;
		}

		// Token: 0x06000C68 RID: 3176 RVA: 0x0004A2DC File Offset: 0x000484DC
		protected virtual void OnNextCheckElapsed()
		{
			Action nextIVPElapsed = this.NextIVPElapsed;
			bool flag = nextIVPElapsed != null;
			if (flag)
			{
				nextIVPElapsed();
			}
		}

		// Token: 0x06000C69 RID: 3177 RVA: 0x0004A300 File Offset: 0x00048500
		private void timerNextCheck_Elapsed(object sender, ElapsedEventArgs e)
		{
			string text = "";
			this.dbConnection.GetIVPInfoEntry(DBIntegratedValidationProcess.ValidationIsActive, ref text);
			bool flag = false;
			this.dbConnection.GetIntegratedValidationProcess(ref flag);
			bool flag2 = text.Equals("1") && flag;
			if (flag2)
			{
				this.OnNextCheckElapsed();
			}
		}

		// Token: 0x06000C6A RID: 3178 RVA: 0x0004A350 File Offset: 0x00048550
		public void CheckTimer()
		{
			string text = "";
			this.dbConnection.GetIVPInfoEntry(DBIntegratedValidationProcess.ValidationIsActive, ref text);
			bool flag = false;
			this.dbConnection.GetIntegratedValidationProcess(ref flag);
			bool flag2 = text.Equals("1") && flag;
			if (flag2)
			{
				this.OnNextCheckElapsed();
			}
		}

		// Token: 0x06000C6B RID: 3179 RVA: 0x0004A3A0 File Offset: 0x000485A0
		public void Disable()
		{
			bool flag = this.timerNextCheck != null;
			if (flag)
			{
				this.timerNextCheck.Stop();
				this.isStarted = false;
			}
		}

		// Token: 0x06000C6C RID: 3180 RVA: 0x0004A3D0 File Offset: 0x000485D0
		public void StartTimer()
		{
			string text = "";
			this.dbConnection.GetIVPInfoEntry(DBIntegratedValidationProcess.ValidationIsActive, ref text);
			bool flag = false;
			this.dbConnection.GetIntegratedValidationProcess(ref flag);
			bool flag2 = text.Equals("1") && !this.isStarted && flag;
			if (flag2)
			{
				this.timerNextCheck.Start();
				this.isStarted = true;
			}
		}

		// Token: 0x0400082C RID: 2092
		private bool ivpIsEnabled;

		// Token: 0x0400082D RID: 2093
		private bool isStarted = false;

		// Token: 0x0400082E RID: 2094
		private DBConnection dbConnection;

		// Token: 0x0400082F RID: 2095
		private Timer timerNextCheck = new Timer();
	}
}
