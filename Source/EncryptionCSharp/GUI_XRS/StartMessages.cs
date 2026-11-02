using System;
using System.ComponentModel;
using System.Diagnostics;

namespace GUI_XRS
{
	// Token: 0x02000250 RID: 592
	public class StartMessages : INotifyPropertyChanged
	{
		// Token: 0x17000C26 RID: 3110
		// (get) Token: 0x06002D3B RID: 11579 RVA: 0x00128BE8 File Offset: 0x00126DE8
		// (set) Token: 0x06002D3C RID: 11580 RVA: 0x00128BEF File Offset: 0x00126DEF
		public static bool NoConnectionMessage { get; set; }

		// Token: 0x14000102 RID: 258
		// (add) Token: 0x06002D3D RID: 11581 RVA: 0x00128BF8 File Offset: 0x00126DF8
		// (remove) Token: 0x06002D3E RID: 11582 RVA: 0x00128C30 File Offset: 0x00126E30
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x17000C27 RID: 3111
		// (get) Token: 0x06002D3F RID: 11583 RVA: 0x00128C68 File Offset: 0x00126E68
		// (set) Token: 0x06002D40 RID: 11584 RVA: 0x00128C80 File Offset: 0x00126E80
		public bool BulkSortingOnOff
		{
			get
			{
				return this.bulkSortingOnOff;
			}
			set
			{
				this.bulkSortingOnOff = value;
				bool flag = this.state == MachineStates.Inspecting && this.BulkSortingOnOff && this.BulkOnOff;
				if (flag)
				{
					this.Message.ResourceKey = "txtSortingOff";
				}
				else
				{
					bool flag2 = this.state == MachineStates.Inspecting && !this.BulkSortingOnOff && this.BulkOnOff;
					if (flag2)
					{
						this.Message.ResourceKey = "txtInspectionIsRunning";
					}
					else
					{
						bool flag3 = !this.BulkOnOff && this.state == MachineStates.Inspecting;
						if (flag3)
						{
							this.Message.ResourceKey = "";
						}
					}
				}
			}
		}

		// Token: 0x17000C28 RID: 3112
		// (get) Token: 0x06002D41 RID: 11585 RVA: 0x00128D28 File Offset: 0x00126F28
		// (set) Token: 0x06002D42 RID: 11586 RVA: 0x00128D40 File Offset: 0x00126F40
		public bool BulkOnOff
		{
			get
			{
				return this.bulkOnOff;
			}
			set
			{
				this.bulkOnOff = value;
			}
		}

		// Token: 0x17000C29 RID: 3113
		// (get) Token: 0x06002D43 RID: 11587 RVA: 0x00128D4C File Offset: 0x00126F4C
		// (set) Token: 0x06002D44 RID: 11588 RVA: 0x00128D64 File Offset: 0x00126F64
		public MachineStates State
		{
			get
			{
				return this.state;
			}
			set
			{
				this.state = value;
				bool flag = this.state == MachineStates.NoConnectToSourceOrDetector;
				if (flag)
				{
					bool noConnectionMessage = StartMessages.NoConnectionMessage;
					string resourceKey;
					if (noConnectionMessage)
					{
						resourceKey = "lbXRayError";
					}
					else
					{
						resourceKey = "lbDetectorCommError";
					}
					this.Message.ResourceKey = resourceKey;
				}
				else
				{
					bool flag2 = this.state == MachineStates.Inspecting;
					if (flag2)
					{
						bool flag3 = this.BulkSortingOnOff && this.BulkOnOff;
						if (flag3)
						{
							this.Message.ResourceKey = "txtSortingOff";
						}
						else
						{
							bool flag4 = !this.BulkSortingOnOff && this.BulkOnOff;
							if (flag4)
							{
								this.Message.ResourceKey = "txtInspectionIsRunning";
							}
							else
							{
								bool flag5 = !this.BulkOnOff;
								if (flag5)
								{
									this.Message.ResourceKey = "";
								}
							}
						}
					}
					else
					{
						this.Message.ResourceKey = ((this.state < (MachineStates)this.messages.Length) ? this.messages[(int)this.state] : "");
						bool flag6 = this.state == MachineStates.Reseasoning;
						if (flag6)
						{
							int num = this.RemainingReseasonTime / 60;
							int num2 = this.RemainingReseasonTime % 60;
							bool flag7 = this.RemainingReseasonTime > 0;
							if (flag7)
							{
								BoundLocalizedText message = this.Message;
								message.LocalizedText = string.Concat(new string[]
								{
									message.LocalizedText,
									Environment.NewLine,
									LocalizationManager.Localize("txtRemainingTime"),
									" ",
									num.ToString("D2"),
									":",
									num2.ToString("D2")
								});
							}
						}
					}
				}
			}
		}

		// Token: 0x17000C2A RID: 3114
		// (get) Token: 0x06002D45 RID: 11589 RVA: 0x00128F13 File Offset: 0x00127113
		// (set) Token: 0x06002D46 RID: 11590 RVA: 0x00128F1B File Offset: 0x0012711B
		public BoundLocalizedText Message { get; set; }

		// Token: 0x17000C2B RID: 3115
		// (get) Token: 0x06002D47 RID: 11591 RVA: 0x00128F24 File Offset: 0x00127124
		// (set) Token: 0x06002D48 RID: 11592 RVA: 0x00128F2C File Offset: 0x0012712C
		public XrayState XRayState { get; set; }

		// Token: 0x17000C2C RID: 3116
		// (get) Token: 0x06002D49 RID: 11593 RVA: 0x00128F35 File Offset: 0x00127135
		// (set) Token: 0x06002D4A RID: 11594 RVA: 0x00128F3D File Offset: 0x0012713D
		public int RemainingReseasonTime { get; set; }

		// Token: 0x06002D4B RID: 11595 RVA: 0x00128F48 File Offset: 0x00127148
		public StartMessages()
		{
			this.Message = new BoundLocalizedText(this.messages[0]);
		}

		// Token: 0x06002D4C RID: 11596 RVA: 0x0012901C File Offset: 0x0012721C
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x04001C47 RID: 7239
		private string[] messages = new string[]
		{
			"txtStartConveyor",
			"txtTurnXrayOn",
			"txtStartXray",
			"txtReseasoning",
			"txtCalibrating",
			"",
			"",
			"txtInspectionIsRunning",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			"txtNoBeltStartOnError"
		};

		// Token: 0x04001C48 RID: 7240
		private MachineStates state;

		// Token: 0x04001C4B RID: 7243
		private bool bulkSortingOnOff = true;

		// Token: 0x04001C4C RID: 7244
		private bool bulkOnOff = true;
	}
}
