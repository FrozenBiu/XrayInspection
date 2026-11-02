using System;
using System.ComponentModel;
using System.Diagnostics;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x02000144 RID: 324
	public class QualityParameter : INotifyPropertyChanged
	{
		// Token: 0x140000A4 RID: 164
		// (add) Token: 0x06001EDD RID: 7901 RVA: 0x000CE448 File Offset: 0x000CC648
		// (remove) Token: 0x06001EDE RID: 7902 RVA: 0x000CE480 File Offset: 0x000CC680
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x170008D9 RID: 2265
		// (get) Token: 0x06001EDF RID: 7903 RVA: 0x000CE4B5 File Offset: 0x000CC6B5
		// (set) Token: 0x06001EE0 RID: 7904 RVA: 0x000CE4BD File Offset: 0x000CC6BD
		public string Path { get; set; }

		// Token: 0x170008DA RID: 2266
		// (get) Token: 0x06001EE1 RID: 7905 RVA: 0x000CE4C6 File Offset: 0x000CC6C6
		// (set) Token: 0x06001EE2 RID: 7906 RVA: 0x000CE4CE File Offset: 0x000CC6CE
		public string ParamName { get; set; }

		// Token: 0x170008DB RID: 2267
		// (get) Token: 0x06001EE3 RID: 7907 RVA: 0x000CE4D7 File Offset: 0x000CC6D7
		// (set) Token: 0x06001EE4 RID: 7908 RVA: 0x000CE4DF File Offset: 0x000CC6DF
		public string[] Values { get; set; }

		// Token: 0x170008DC RID: 2268
		// (get) Token: 0x06001EE5 RID: 7909 RVA: 0x000CE4E8 File Offset: 0x000CC6E8
		// (set) Token: 0x06001EE6 RID: 7910 RVA: 0x000CE500 File Offset: 0x000CC700
		public ParamState[] ParamStates
		{
			get
			{
				return this.paramStates;
			}
			set
			{
				this.paramStates = value;
			}
		}

		// Token: 0x170008DD RID: 2269
		// (get) Token: 0x06001EE8 RID: 7912 RVA: 0x000CE513 File Offset: 0x000CC713
		// (set) Token: 0x06001EE7 RID: 7911 RVA: 0x000CE50A File Offset: 0x000CC70A
		public string Group { get; set; }

		// Token: 0x170008DE RID: 2270
		// (get) Token: 0x06001EE9 RID: 7913 RVA: 0x000CE51B File Offset: 0x000CC71B
		// (set) Token: 0x06001EEA RID: 7914 RVA: 0x000CE523 File Offset: 0x000CC723
		public ParamState OverallParamState { get; set; }

		// Token: 0x06001EEB RID: 7915 RVA: 0x000CE52C File Offset: 0x000CC72C
		public QualityParameter(string path, string paramName)
		{
			this.Path = path;
			this.ParamName = paramName;
			this.Values = new string[Definition.nQACheckpoints + 1];
			this.ParamStates = new ParamState[Definition.nQACheckpoints + 1];
			this.Group = "";
			this.OverallParamState = ParamState.Equal;
		}

		// Token: 0x06001EEC RID: 7916 RVA: 0x000CE58C File Offset: 0x000CC78C
		public QualityParameter(string path, string paramName, string group)
		{
			this.Path = path;
			this.ParamName = paramName;
			this.Values = new string[Definition.nQACheckpoints + 1];
			this.ParamStates = new ParamState[Definition.nQACheckpoints + 1];
			this.Group = group;
			this.OverallParamState = ParamState.Equal;
		}

		// Token: 0x06001EED RID: 7917 RVA: 0x000CE5E8 File Offset: 0x000CC7E8
		public void SetParamStates(int chkIndex)
		{
			bool flag = chkIndex < 0 || chkIndex >= this.ParamStates.Length;
			if (!flag)
			{
				bool flag2 = this.Values[chkIndex + 1] != this.Values[0];
				if (flag2)
				{
					this.ParamStates[0] = ParamState.Changed;
					this.ParamStates[chkIndex + 1] = ParamState.Changed;
				}
				else
				{
					this.ParamStates[0] = ParamState.Equal;
					this.ParamStates[chkIndex + 1] = ParamState.Equal;
				}
				for (int i = 0; i < this.ParamStates.Length; i++)
				{
					bool flag3 = i != 0 && i != chkIndex + 1;
					if (flag3)
					{
						this.ParamStates[i] = ParamState.Neutral;
					}
				}
			}
		}

		// Token: 0x06001EEE RID: 7918 RVA: 0x000CE694 File Offset: 0x000CC894
		public void SetOverallParamState()
		{
			bool flag = this.Values[1] != this.Values[0] || this.Values[2] != this.Values[0] || this.Values[3] != this.Values[0];
			if (flag)
			{
				this.OverallParamState = ParamState.Changed;
			}
			else
			{
				this.OverallParamState = ParamState.Equal;
			}
			this.OnPropertyChanged("OverallParamState");
		}

		// Token: 0x06001EEF RID: 7919 RVA: 0x000CE70E File Offset: 0x000CC90E
		public void SignalValueChange()
		{
			this.OnPropertyChanged("Values");
		}

		// Token: 0x06001EF0 RID: 7920 RVA: 0x000CE720 File Offset: 0x000CC920
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x04001430 RID: 5168
		private ParamState[] paramStates;
	}
}
