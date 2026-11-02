using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Common;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x02000026 RID: 38
	public class AuditCheckDocOverviewViewModel : INotifyPropertyChanged
	{
		// Token: 0x1400001F RID: 31
		// (add) Token: 0x060004D2 RID: 1234 RVA: 0x00021DFC File Offset: 0x0001FFFC
		// (remove) Token: 0x060004D3 RID: 1235 RVA: 0x00021E34 File Offset: 0x00020034
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x060004D4 RID: 1236 RVA: 0x00021E69 File Offset: 0x00020069
		// (set) Token: 0x060004D5 RID: 1237 RVA: 0x00021E71 File Offset: 0x00020071
		private List<Document> documents { get; set; }

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x060004D6 RID: 1238 RVA: 0x00021E7C File Offset: 0x0002007C
		// (set) Token: 0x060004D7 RID: 1239 RVA: 0x00021E94 File Offset: 0x00020094
		public List<Document> Documents
		{
			get
			{
				return this.documents;
			}
			set
			{
				this.documents = value;
				this.OnPropertyChanged("Documents");
			}
		}

		// Token: 0x17000179 RID: 377
		// (get) Token: 0x060004D8 RID: 1240 RVA: 0x00021EAC File Offset: 0x000200AC
		// (set) Token: 0x060004D9 RID: 1241 RVA: 0x00021EC4 File Offset: 0x000200C4
		public Document SelectedDocument
		{
			get
			{
				return this.selectedDocument;
			}
			set
			{
				this.selectedDocument = value;
				this.OnPropertyChanged("SelectedDocument");
			}
		}

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x060004DA RID: 1242 RVA: 0x00021EDA File Offset: 0x000200DA
		// (set) Token: 0x060004DB RID: 1243 RVA: 0x00021EE2 File Offset: 0x000200E2
		public List<Document> SelectedDocuments { get; set; }

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x060004DC RID: 1244 RVA: 0x00021EEB File Offset: 0x000200EB
		// (set) Token: 0x060004DD RID: 1245 RVA: 0x00021EF3 File Offset: 0x000200F3
		public RelayCommand CleanUpCommand { get; set; }

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x060004DE RID: 1246 RVA: 0x00021EFC File Offset: 0x000200FC
		// (set) Token: 0x060004DF RID: 1247 RVA: 0x00021F14 File Offset: 0x00020114
		public bool ShowUSBConnected
		{
			get
			{
				return this.usbDirExists;
			}
			set
			{
				this.usbDirExists = value;
				this.OnPropertyChanged("ShowUSBConnected");
			}
		}

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x060004E0 RID: 1248 RVA: 0x00021F2C File Offset: 0x0002012C
		// (set) Token: 0x060004E1 RID: 1249 RVA: 0x00021F47 File Offset: 0x00020147
		public bool ShowUSBNotConnected
		{
			get
			{
				return !this.usbDirExists;
			}
			set
			{
				this.OnPropertyChanged("ShowUSBNotConnected");
			}
		}

		// Token: 0x1700017E RID: 382
		// (get) Token: 0x060004E2 RID: 1250 RVA: 0x00021F56 File Offset: 0x00020156
		// (set) Token: 0x060004E3 RID: 1251 RVA: 0x00021F5E File Offset: 0x0002015E
		public bool DeleteAllowed { get; set; }

		// Token: 0x060004E4 RID: 1252 RVA: 0x00021F68 File Offset: 0x00020168
		public AuditCheckDocOverviewViewModel(string documentsPath, string usbPath, AccessRightsManager accessManager)
		{
			this.documentsPath = documentsPath;
			this.usbPath = usbPath;
			this.CleanUpCommand = new RelayCommand(new Action<object>(this.CleanUp));
			this.Documents = new List<Document>();
			this.DeleteAllowed = (accessManager.CurrentUser.Rights >= this.rightDeleteFiles);
			this.GetDocuments();
			this.usbDirExists = Directory.Exists(usbPath);
		}

		// Token: 0x060004E5 RID: 1253 RVA: 0x00021FEC File Offset: 0x000201EC
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x060004E6 RID: 1254 RVA: 0x0002201C File Offset: 0x0002021C
		public void DeleteFile(object obj)
		{
			bool flag = this.SelectedDocuments != null;
			if (flag)
			{
				bool flag2 = this.SelectedDocuments.Count<Document>() > 0;
				if (flag2)
				{
					bool flag3 = Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtDeleteDocuments"), DialogStyle.Warning, DialogButtons.OKCancel) == Dialog.DialogResults.Ok;
					if (flag3)
					{
						int num = 0;
						foreach (Document document in this.SelectedDocuments)
						{
							try
							{
								File.Delete(document.FilePath);
								num++;
							}
							catch
							{
								Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtDeletingNotPossible") + ": " + document.DisplayedName, DialogStyle.Warning, DialogButtons.OK);
							}
						}
						Dialog.Show(LocalizationManager.Localize("txtInfo"), LocalizationManager.Localize("txtDeleteDone"), DialogStyle.Info, DialogButtons.OK);
						this.GetDocuments();
					}
				}
			}
		}

		// Token: 0x060004E7 RID: 1255 RVA: 0x00022138 File Offset: 0x00020338
		public void AddFileToUSB(object obj)
		{
			bool flag = this.SelectedDocuments != null;
			if (flag)
			{
				bool flag2 = this.SelectedDocuments.Count<Document>() > 0 && Directory.Exists(this.usbPath);
				if (flag2)
				{
					int num = 0;
					foreach (Document document in this.SelectedDocuments)
					{
						string fileName = Path.GetFileName(document.FilePath);
						string text = this.usbPath + "\\" + fileName;
						bool flag3 = !File.Exists(text) || Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("msgFileAlreadyExistent") + " " + document.DisplayedName, DialogStyle.Warning, DialogButtons.OKCancel) == Dialog.DialogResults.Ok;
						if (flag3)
						{
							try
							{
								File.Copy(document.FilePath, text, true);
								num++;
							}
							catch (Exception)
							{
								Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtError"), DialogStyle.Warning, DialogButtons.OK);
							}
						}
					}
					this.GetDocuments();
					bool flag4 = num > 0;
					if (flag4)
					{
						Dialog.Show(LocalizationManager.Localize("txtInfo"), LocalizationManager.Localize("txtSaveSuccessful"), DialogStyle.Info, DialogButtons.OK);
					}
				}
			}
		}

		// Token: 0x060004E8 RID: 1256 RVA: 0x000222A0 File Offset: 0x000204A0
		private void GetDocuments()
		{
			int num = 0;
			List<Document> list = new List<Document>();
			string[] directories = Directory.GetDirectories(this.documentsPath);
			for (int i = 0; i < directories.Length; i++)
			{
				string[] files = Directory.GetFiles(directories[i]);
				for (int j = 0; j < files.Length; j++)
				{
					bool flag = files[j].EndsWith(Definition.pdfFileEnding);
					if (flag)
					{
						num++;
						Document item = new Document(files[j], num);
						list.Add(item);
					}
				}
			}
			list.Sort(new DocumentsComparer());
			int num2 = 1;
			foreach (Document document in list)
			{
				document.Index = num2;
				num2++;
			}
			this.Documents = list;
			bool flag2 = this.Documents.Count > 0 && this.SelectedDocument == null;
			if (flag2)
			{
				this.SelectedDocument = this.Documents[0];
			}
		}

		// Token: 0x060004E9 RID: 1257 RVA: 0x000223CC File Offset: 0x000205CC
		private void CleanUp(object obj)
		{
			bool flag = this.Documents != null;
			if (flag)
			{
				this.Documents.Clear();
				this.Documents = null;
			}
			bool flag2 = this.SelectedDocuments != null;
			if (flag2)
			{
				this.SelectedDocuments.Clear();
				this.SelectedDocuments = null;
			}
		}

		// Token: 0x0400039F RID: 927
		private Document selectedDocument;

		// Token: 0x040003A3 RID: 931
		private bool usbDirExists;

		// Token: 0x040003A5 RID: 933
		private string documentsPath;

		// Token: 0x040003A6 RID: 934
		private string usbPath;

		// Token: 0x040003A7 RID: 935
		private BuiltInPriviledges rightDeleteFiles = BuiltInPriviledges.Admin;
	}
}
