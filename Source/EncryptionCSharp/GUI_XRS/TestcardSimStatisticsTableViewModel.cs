using System;
using Common;

namespace GUI_XRS
{
	// Token: 0x020000E9 RID: 233
	public class TestcardSimStatisticsTableViewModel
	{
		// Token: 0x170006EA RID: 1770
		// (get) Token: 0x060016E1 RID: 5857 RVA: 0x00089F9D File Offset: 0x0008819D
		// (set) Token: 0x060016E2 RID: 5858 RVA: 0x00089FA5 File Offset: 0x000881A5
		public RelayCommand CleanUpCommand { get; set; }

		// Token: 0x170006EB RID: 1771
		// (get) Token: 0x060016E3 RID: 5859 RVA: 0x00089FAE File Offset: 0x000881AE
		// (set) Token: 0x060016E4 RID: 5860 RVA: 0x00089FB6 File Offset: 0x000881B6
		public BoundLocalizedText StrStatisticTable { get; set; }

		// Token: 0x170006EC RID: 1772
		// (get) Token: 0x060016E5 RID: 5861 RVA: 0x00089FBF File Offset: 0x000881BF
		// (set) Token: 0x060016E6 RID: 5862 RVA: 0x00089FC7 File Offset: 0x000881C7
		public BoundLocalizedText StrVA { get; set; }

		// Token: 0x170006ED RID: 1773
		// (get) Token: 0x060016E7 RID: 5863 RVA: 0x00089FD0 File Offset: 0x000881D0
		// (set) Token: 0x060016E8 RID: 5864 RVA: 0x00089FD8 File Offset: 0x000881D8
		public BoundLocalizedText StrGlass { get; set; }

		// Token: 0x060016E9 RID: 5865 RVA: 0x00089FE1 File Offset: 0x000881E1
		public TestcardSimStatisticsTableViewModel()
		{
			this.RegisterLocalizedText();
			this.CleanUpCommand = new RelayCommand(new Action<object>(this.CleanUp));
		}

		// Token: 0x060016EA RID: 5866 RVA: 0x0008A00A File Offset: 0x0008820A
		private void RegisterLocalizedText()
		{
			this.StrStatisticTable = new BoundLocalizedText("txtStatisticTable");
			this.StrVA = new BoundLocalizedText("txtVA");
			this.StrGlass = new BoundLocalizedText("txtGlass");
		}

		// Token: 0x060016EB RID: 5867 RVA: 0x0008A040 File Offset: 0x00088240
		public void DeregisterLocalizedText()
		{
			MainWindow.localizationManager.Deregister(this.StrStatisticTable);
			MainWindow.localizationManager.Deregister(this.StrVA);
			MainWindow.localizationManager.Deregister(this.StrGlass);
		}

		// Token: 0x060016EC RID: 5868 RVA: 0x0008A076 File Offset: 0x00088276
		private void CleanUp(object obj)
		{
			this.DeregisterLocalizedText();
		}
	}
}
