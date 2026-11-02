using System;
using System.Collections.Generic;
using Common;
using DatabaseConnection;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x02000147 RID: 327
	public class BuzzerViewModel
	{
		// Token: 0x170008E9 RID: 2281
		// (get) Token: 0x06001F15 RID: 7957 RVA: 0x000CEE70 File Offset: 0x000CD070
		// (set) Token: 0x06001F16 RID: 7958 RVA: 0x000CEE78 File Offset: 0x000CD078
		public List<Buzzer> Buzzers { get; set; }

		// Token: 0x170008EA RID: 2282
		// (get) Token: 0x06001F17 RID: 7959 RVA: 0x000CEE81 File Offset: 0x000CD081
		// (set) Token: 0x06001F18 RID: 7960 RVA: 0x000CEE89 File Offset: 0x000CD089
		public RelayCommand CleanUpCommand { get; set; }

		// Token: 0x06001F19 RID: 7961 RVA: 0x000CEE94 File Offset: 0x000CD094
		public BuzzerViewModel(ParameterHandling parHandling, DBConnection dbConnection)
		{
			this.parHandling = parHandling;
			this.dbConnection = dbConnection;
			this.Buzzers = new List<Buzzer>();
			this.Buzzers.Add(new Buzzer(parHandling, dbConnection, Definition.buzzerMainPath));
			this.CleanUpCommand = new RelayCommand(new Action<object>(this.CleanUp));
		}

		// Token: 0x06001F1A RID: 7962 RVA: 0x000CEEF4 File Offset: 0x000CD0F4
		private void CleanUp(object obj)
		{
			foreach (Buzzer buzzer in this.Buzzers)
			{
				buzzer.DeregisterLocalizedText();
			}
		}

		// Token: 0x04001447 RID: 5191
		private ParameterHandling parHandling;

		// Token: 0x04001448 RID: 5192
		private DBConnection dbConnection;
	}
}
