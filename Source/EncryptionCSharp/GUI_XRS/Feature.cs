using System;
using System.ComponentModel;
using System.Diagnostics;

namespace GUI_XRS
{
	// Token: 0x020000A5 RID: 165
	public class Feature : INotifyPropertyChanged
	{
		// Token: 0x14000051 RID: 81
		// (add) Token: 0x06001046 RID: 4166 RVA: 0x000657C0 File Offset: 0x000639C0
		// (remove) Token: 0x06001047 RID: 4167 RVA: 0x000657F8 File Offset: 0x000639F8
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x170004E3 RID: 1251
		// (get) Token: 0x06001048 RID: 4168 RVA: 0x00065830 File Offset: 0x00063A30
		// (set) Token: 0x06001049 RID: 4169 RVA: 0x00065848 File Offset: 0x00063A48
		public string Description
		{
			get
			{
				return this.description;
			}
			set
			{
				this.description = value;
				this.OnPropertyChanged("Description");
			}
		}

		// Token: 0x170004E4 RID: 1252
		// (get) Token: 0x0600104A RID: 4170 RVA: 0x00065860 File Offset: 0x00063A60
		// (set) Token: 0x0600104B RID: 4171 RVA: 0x00065878 File Offset: 0x00063A78
		public bool Enabled
		{
			get
			{
				return this.enabled;
			}
			set
			{
				bool flag = this.enabled != value;
				if (flag)
				{
					for (int i = 0; i < this.enablePaths.Length; i++)
					{
						this.parHandling.WriteValue(this.enablePaths[i], value ? "1" : "0");
					}
					this.enabled = value;
					this.OnPropertyChanged("Enabled");
				}
			}
		}

		// Token: 0x0600104C RID: 4172 RVA: 0x000658E7 File Offset: 0x00063AE7
		public Feature(ParameterHandling parHandling, string description, bool enabled, string enablePath) : this(parHandling, description, enabled, new string[]
		{
			enablePath
		})
		{
		}

		// Token: 0x0600104D RID: 4173 RVA: 0x000658FF File Offset: 0x00063AFF
		public Feature(ParameterHandling parHandling, string description, bool enabled, string[] enablePaths)
		{
			this.parHandling = parHandling;
			this.description = description;
			this.enabled = enabled;
			this.enablePaths = enablePaths;
		}

		// Token: 0x0600104E RID: 4174 RVA: 0x00065928 File Offset: 0x00063B28
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x04000AE9 RID: 2793
		private ParameterHandling parHandling;

		// Token: 0x04000AEA RID: 2794
		private string[] enablePaths;

		// Token: 0x04000AEB RID: 2795
		private string description;

		// Token: 0x04000AEC RID: 2796
		private bool enabled;
	}
}
