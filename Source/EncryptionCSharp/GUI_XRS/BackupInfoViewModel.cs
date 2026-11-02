using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Common;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x02000175 RID: 373
	public class BackupInfoViewModel
	{
		// Token: 0x170009DF RID: 2527
		// (get) Token: 0x06002236 RID: 8758 RVA: 0x000DD729 File Offset: 0x000DB929
		// (set) Token: 0x06002237 RID: 8759 RVA: 0x000DD731 File Offset: 0x000DB931
		public ObservableCollection<BackupInfo> Infos { get; set; }

		// Token: 0x170009E0 RID: 2528
		// (get) Token: 0x06002238 RID: 8760 RVA: 0x000DD73A File Offset: 0x000DB93A
		// (set) Token: 0x06002239 RID: 8761 RVA: 0x000DD742 File Offset: 0x000DB942
		public RelayCommand GetInfosCommand { get; set; }

		// Token: 0x0600223A RID: 8762 RVA: 0x000DD74B File Offset: 0x000DB94B
		public BackupInfoViewModel(string infoFileName)
		{
			this.infoFileName = infoFileName;
			this.Infos = new ObservableCollection<BackupInfo>();
			this.GetInfosCommand = new RelayCommand(new Action<object>(this.GetInfos));
		}

		// Token: 0x0600223B RID: 8763 RVA: 0x000DD780 File Offset: 0x000DB980
		private void GetInfos(object obj)
		{
			bool serviceMode = MainWindowViewModel.accessRightsManager.CurrentUser.Rights == BuiltInPriviledges.Service;
			this.GetBackupInfos(this.infoFileName, serviceMode);
		}

		// Token: 0x0600223C RID: 8764 RVA: 0x000DD7B4 File Offset: 0x000DB9B4
		private void GetBackupInfos(string infoFileName, bool serviceMode)
		{
			BackupInfoFileParser backupInfoFileParser = new BackupInfoFileParser(infoFileName);
			List<BackupInfo> list = backupInfoFileParser.ParseInfoFile(serviceMode);
			for (int i = 0; i < list.Count; i++)
			{
				this.Infos.Add(list[i]);
			}
		}

		// Token: 0x04001640 RID: 5696
		private string infoFileName;
	}
}
