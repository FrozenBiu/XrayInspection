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
	// Token: 0x02000074 RID: 116
	public class IVPCertificatesOverviewViewModel : INotifyPropertyChanged
	{
		// Token: 0x1400003C RID: 60
		// (add) Token: 0x06000ADA RID: 2778 RVA: 0x000404F4 File Offset: 0x0003E6F4
		// (remove) Token: 0x06000ADB RID: 2779 RVA: 0x0004052C File Offset: 0x0003E72C
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x1700032F RID: 815
		// (get) Token: 0x06000ADC RID: 2780 RVA: 0x00040561 File Offset: 0x0003E761
		// (set) Token: 0x06000ADD RID: 2781 RVA: 0x00040569 File Offset: 0x0003E769
		private List<Document> documents { get; set; }

		// Token: 0x17000330 RID: 816
		// (get) Token: 0x06000ADE RID: 2782 RVA: 0x00040574 File Offset: 0x0003E774
		// (set) Token: 0x06000ADF RID: 2783 RVA: 0x0004058C File Offset: 0x0003E78C
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

		// Token: 0x17000331 RID: 817
		// (get) Token: 0x06000AE0 RID: 2784 RVA: 0x000405A4 File Offset: 0x0003E7A4
		// (set) Token: 0x06000AE1 RID: 2785 RVA: 0x000405BC File Offset: 0x0003E7BC
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

		// Token: 0x17000332 RID: 818
		// (get) Token: 0x06000AE2 RID: 2786 RVA: 0x000405D2 File Offset: 0x0003E7D2
		// (set) Token: 0x06000AE3 RID: 2787 RVA: 0x000405DA File Offset: 0x0003E7DA
		public List<Document> SelectedDocuments { get; set; }

		// Token: 0x17000333 RID: 819
		// (get) Token: 0x06000AE4 RID: 2788 RVA: 0x000405E3 File Offset: 0x0003E7E3
		// (set) Token: 0x06000AE5 RID: 2789 RVA: 0x000405EB File Offset: 0x0003E7EB
		public RelayCommand CleanUpCommand { get; set; }

		// Token: 0x17000334 RID: 820
		// (get) Token: 0x06000AE6 RID: 2790 RVA: 0x000405F4 File Offset: 0x0003E7F4
		// (set) Token: 0x06000AE7 RID: 2791 RVA: 0x0004060C File Offset: 0x0003E80C
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

		// Token: 0x17000335 RID: 821
		// (get) Token: 0x06000AE8 RID: 2792 RVA: 0x00040624 File Offset: 0x0003E824
		// (set) Token: 0x06000AE9 RID: 2793 RVA: 0x0004063F File Offset: 0x0003E83F
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

		// Token: 0x17000336 RID: 822
		// (get) Token: 0x06000AEA RID: 2794 RVA: 0x0004064E File Offset: 0x0003E84E
		// (set) Token: 0x06000AEB RID: 2795 RVA: 0x00040656 File Offset: 0x0003E856
		public bool DeleteAllowed { get; set; }

		// Token: 0x17000337 RID: 823
		// (get) Token: 0x06000AEC RID: 2796 RVA: 0x0004065F File Offset: 0x0003E85F
		// (set) Token: 0x06000AED RID: 2797 RVA: 0x00040667 File Offset: 0x0003E867
		public BoundLocalizedText StrDocuments { get; set; }

		// Token: 0x06000AEE RID: 2798 RVA: 0x00040670 File Offset: 0x0003E870
		public IVPCertificatesOverviewViewModel(string documentsPath, string usbPath, AccessRightsManager accessManager)
		{
			this.documentsPath = documentsPath;
			this.usbPath = usbPath;
			this.CleanUpCommand = new RelayCommand(new Action<object>(this.CleanUp));
			this.Documents = new List<Document>();
			this.accessManager = accessManager;
			this.DeleteAllowed = (accessManager.CurrentUser.Rights >= this.rightDeleteFiles);
			this.GetDocuments();
			this.usbDirExists = Directory.Exists(usbPath);
			this.RegisterLocalizedText();
		}

		// Token: 0x06000AEF RID: 2799 RVA: 0x00040700 File Offset: 0x0003E900
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x06000AF0 RID: 2800 RVA: 0x00040730 File Offset: 0x0003E930
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
							bool flag4 = this.accessManager.CurrentUser.ID < 0 || (!document.DisplayedName.Contains("Cert_") && !document.DisplayedName.Contains("EA_Cert"));
							if (flag4)
							{
								try
								{
									File.Delete(document.FilePath);
									Directory.Delete(Path.GetDirectoryName(document.FilePath), true);
									num++;
								}
								catch
								{
									Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtDeletingNotPossible") + ": " + document.DisplayedName, DialogStyle.Warning, DialogButtons.OK);
								}
							}
							else
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

		// Token: 0x06000AF1 RID: 2801 RVA: 0x000408E0 File Offset: 0x0003EAE0
		public void AddFileToUSB(object obj)
		{
			bool flag = this.SelectedDocuments != null;
			if (flag)
			{
				bool flag2 = this.SelectedDocuments.Count<Document>() > 0 && Directory.Exists(this.usbPath);
				if (flag2)
				{
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
							}
							catch (Exception)
							{
								Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtError"), DialogStyle.Warning, DialogButtons.OK);
							}
						}
					}
					this.GetDocuments();
					Dialog.Show(LocalizationManager.Localize("txtInfo"), LocalizationManager.Localize("txtSaveSuccessful"), DialogStyle.Info, DialogButtons.OK);
				}
			}
		}

		// Token: 0x06000AF2 RID: 2802 RVA: 0x00040A34 File Offset: 0x0003EC34
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

		// Token: 0x06000AF3 RID: 2803 RVA: 0x00040B60 File Offset: 0x0003ED60
		private void CleanUp(object obj)
		{
			this.DeregisterLocalizedText();
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

		// Token: 0x06000AF4 RID: 2804 RVA: 0x00040BBB File Offset: 0x0003EDBB
		private void RegisterLocalizedText()
		{
			this.StrDocuments = new BoundLocalizedText("menuItemIVPCertificats");
		}

		// Token: 0x06000AF5 RID: 2805 RVA: 0x00040BCF File Offset: 0x0003EDCF
		private void DeregisterLocalizedText()
		{
			MainWindow.localizationManager.Deregister(this.StrDocuments);
		}

		// Token: 0x04000746 RID: 1862
		private Document selectedDocument;

		// Token: 0x0400074A RID: 1866
		private bool usbDirExists;

		// Token: 0x0400074C RID: 1868
		private string documentsPath;

		// Token: 0x0400074D RID: 1869
		private string usbPath;

		// Token: 0x0400074E RID: 1870
		private BuiltInPriviledges rightDeleteFiles = BuiltInPriviledges.Admin;

		// Token: 0x04000750 RID: 1872
		private AccessRightsManager accessManager;
	}
}
