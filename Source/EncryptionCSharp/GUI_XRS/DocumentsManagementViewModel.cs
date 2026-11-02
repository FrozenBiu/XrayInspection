using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Threading;
using Common;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x020000A9 RID: 169
	public class DocumentsManagementViewModel : INotifyPropertyChanged
	{
		// Token: 0x14000054 RID: 84
		// (add) Token: 0x060010A7 RID: 4263 RVA: 0x00066E9C File Offset: 0x0006509C
		// (remove) Token: 0x060010A8 RID: 4264 RVA: 0x00066ED4 File Offset: 0x000650D4
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x17000508 RID: 1288
		// (get) Token: 0x060010A9 RID: 4265 RVA: 0x00066F09 File Offset: 0x00065109
		// (set) Token: 0x060010AA RID: 4266 RVA: 0x00066F11 File Offset: 0x00065111
		public ObservableCollection<Document> DocumentsLocal { get; set; }

		// Token: 0x17000509 RID: 1289
		// (get) Token: 0x060010AB RID: 4267 RVA: 0x00066F1A File Offset: 0x0006511A
		// (set) Token: 0x060010AC RID: 4268 RVA: 0x00066F22 File Offset: 0x00065122
		public ObservableCollection<Document> DocumentsUSB { get; set; }

		// Token: 0x1700050A RID: 1290
		// (get) Token: 0x060010AD RID: 4269 RVA: 0x00066F2C File Offset: 0x0006512C
		// (set) Token: 0x060010AE RID: 4270 RVA: 0x00066F44 File Offset: 0x00065144
		public Document SelectedLocalDocument
		{
			get
			{
				return this.selectedLocalDocument;
			}
			set
			{
				this.selectedLocalDocument = value;
				this.OnPropertyChanged("SelectedLocalDocument");
			}
		}

		// Token: 0x1700050B RID: 1291
		// (get) Token: 0x060010AF RID: 4271 RVA: 0x00066F5C File Offset: 0x0006515C
		// (set) Token: 0x060010B0 RID: 4272 RVA: 0x00066F74 File Offset: 0x00065174
		public Document SelectedUSBDocument
		{
			get
			{
				return this.selectedUSBDocument;
			}
			set
			{
				this.selectedUSBDocument = value;
				this.OnPropertyChanged("SelectedUSBDocument");
			}
		}

		// Token: 0x1700050C RID: 1292
		// (get) Token: 0x060010B1 RID: 4273 RVA: 0x00066F8C File Offset: 0x0006518C
		// (set) Token: 0x060010B2 RID: 4274 RVA: 0x00066FA4 File Offset: 0x000651A4
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

		// Token: 0x1700050D RID: 1293
		// (get) Token: 0x060010B3 RID: 4275 RVA: 0x00066FBC File Offset: 0x000651BC
		// (set) Token: 0x060010B4 RID: 4276 RVA: 0x00066FD7 File Offset: 0x000651D7
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

		// Token: 0x1700050E RID: 1294
		// (get) Token: 0x060010B5 RID: 4277 RVA: 0x00066FE6 File Offset: 0x000651E6
		// (set) Token: 0x060010B6 RID: 4278 RVA: 0x00066FEE File Offset: 0x000651EE
		public bool DeleteAllowed { get; set; }

		// Token: 0x1700050F RID: 1295
		// (get) Token: 0x060010B7 RID: 4279 RVA: 0x00066FF7 File Offset: 0x000651F7
		// (set) Token: 0x060010B8 RID: 4280 RVA: 0x00066FFF File Offset: 0x000651FF
		public RelayCommand AddFileToLocalCommand { get; set; }

		// Token: 0x17000510 RID: 1296
		// (get) Token: 0x060010B9 RID: 4281 RVA: 0x00067008 File Offset: 0x00065208
		// (set) Token: 0x060010BA RID: 4282 RVA: 0x00067010 File Offset: 0x00065210
		public RelayCommand AddFileToUSBCommand { get; set; }

		// Token: 0x17000511 RID: 1297
		// (get) Token: 0x060010BB RID: 4283 RVA: 0x00067019 File Offset: 0x00065219
		// (set) Token: 0x060010BC RID: 4284 RVA: 0x00067021 File Offset: 0x00065221
		public RelayCommand DeleteLocalFileCommand { get; set; }

		// Token: 0x17000512 RID: 1298
		// (get) Token: 0x060010BD RID: 4285 RVA: 0x0006702A File Offset: 0x0006522A
		// (set) Token: 0x060010BE RID: 4286 RVA: 0x00067032 File Offset: 0x00065232
		public RelayCommand RenameLocalFileCommand { get; set; }

		// Token: 0x17000513 RID: 1299
		// (get) Token: 0x060010BF RID: 4287 RVA: 0x0006703B File Offset: 0x0006523B
		// (set) Token: 0x060010C0 RID: 4288 RVA: 0x00067043 File Offset: 0x00065243
		public RelayCommand CleanUpCommand { get; set; }

		// Token: 0x060010C1 RID: 4289 RVA: 0x0006704C File Offset: 0x0006524C
		public DocumentsManagementViewModel(string documentsPath, string usbPath, AccessRightsManager accessManager)
		{
			this.documentsPath = documentsPath;
			this.usbPath = usbPath;
			this.AddFileToLocalCommand = new RelayCommand(new Action<object>(this.AddFileToLocal));
			this.AddFileToUSBCommand = new RelayCommand(new Action<object>(this.AddFileToUSB));
			this.DeleteLocalFileCommand = new RelayCommand(new Action<object>(this.DeleteLocalFile));
			this.RenameLocalFileCommand = new RelayCommand(new Action<object>(this.RenameLocalFile));
			this.CleanUpCommand = new RelayCommand(new Action<object>(this.CleanUp));
			this.DocumentsLocal = new ObservableCollection<Document>();
			this.DocumentsUSB = new ObservableCollection<Document>();
			this.DeleteAllowed = (accessManager.CurrentUser.Rights >= this.rightDeleteFiles);
			this.GetDocuments();
			this.usbDirExists = Directory.Exists(usbPath);
			this.timerUSB = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Background, new EventHandler(this.SurveyUSB), Application.Current.Dispatcher);
		}

		// Token: 0x060010C2 RID: 4290 RVA: 0x00067164 File Offset: 0x00065364
		private void GetDocuments()
		{
			int num = 0;
			this.DocumentsLocal.Clear();
			this.DocumentsUSB.Clear();
			string[] files = Directory.GetFiles(this.documentsPath);
			for (int i = 0; i < files.Length; i++)
			{
				bool flag = files[i].EndsWith(Definition.pdfFileEnding);
				if (flag)
				{
					num++;
					Document item = new Document(files[i], num);
					this.DocumentsLocal.Add(item);
				}
			}
			bool flag2 = this.DocumentsLocal.Count > 0 && this.SelectedLocalDocument == null;
			if (flag2)
			{
				this.SelectedLocalDocument = this.DocumentsLocal[0];
			}
			bool flag3 = Directory.Exists(this.usbPath);
			if (flag3)
			{
				num = 0;
				files = Directory.GetFiles(this.usbPath);
				for (int j = 0; j < files.Length; j++)
				{
					bool flag4 = files[j].EndsWith(Definition.pdfFileEnding);
					if (flag4)
					{
						num++;
						Document item2 = new Document(files[j], num);
						this.DocumentsUSB.Add(item2);
					}
				}
				bool flag5 = this.DocumentsUSB.Count > 0 && this.SelectedUSBDocument == null;
				if (flag5)
				{
					this.SelectedUSBDocument = this.DocumentsUSB[0];
				}
			}
			else
			{
				this.SelectedUSBDocument = null;
			}
		}

		// Token: 0x060010C3 RID: 4291 RVA: 0x000672C0 File Offset: 0x000654C0
		private void AddFileToLocal(object obj)
		{
			bool flag = this.SelectedUSBDocument != null && Directory.Exists(this.usbPath);
			if (flag)
			{
				string fileName = Path.GetFileName(this.SelectedUSBDocument.FilePath);
				string text = this.documentsPath + "\\" + fileName;
				bool flag2 = !File.Exists(text) || Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("msgFileAlreadyExistent"), DialogStyle.Warning, DialogButtons.OKCancel) == Dialog.DialogResults.Ok;
				if (flag2)
				{
					File.Copy(this.SelectedUSBDocument.FilePath, text, true);
					this.GetDocuments();
				}
			}
		}

		// Token: 0x060010C4 RID: 4292 RVA: 0x00067358 File Offset: 0x00065558
		private void AddFileToUSB(object obj)
		{
			bool flag = this.SelectedLocalDocument != null && Directory.Exists(this.usbPath);
			if (flag)
			{
				string fileName = Path.GetFileName(this.SelectedLocalDocument.FilePath);
				string text = this.usbPath + "\\" + fileName;
				bool flag2 = !File.Exists(text) || Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("msgFileAlreadyExistent"), DialogStyle.Warning, DialogButtons.OKCancel) == Dialog.DialogResults.Ok;
				if (flag2)
				{
					File.Copy(this.SelectedLocalDocument.FilePath, text, true);
					this.GetDocuments();
				}
			}
		}

		// Token: 0x060010C5 RID: 4293 RVA: 0x000673F0 File Offset: 0x000655F0
		private void DeleteLocalFile(object obj)
		{
			Document document = null;
			bool flag = obj is string;
			if (flag)
			{
				bool flag2 = (string)obj == "local";
				if (flag2)
				{
					document = this.SelectedLocalDocument;
				}
				else
				{
					bool flag3 = (string)obj == "usb";
					if (flag3)
					{
						document = this.SelectedUSBDocument;
					}
				}
			}
			bool flag4 = document != null;
			if (flag4)
			{
				bool flag5 = Dialog.Show(LocalizationManager.Localize("txtWarning"), "Delete document?", DialogStyle.Warning, DialogButtons.OKCancel) == Dialog.DialogResults.Ok;
				if (flag5)
				{
					File.Delete(document.FilePath);
					this.GetDocuments();
				}
			}
		}

		// Token: 0x060010C6 RID: 4294 RVA: 0x00067488 File Offset: 0x00065688
		private void RenameLocalFile(object obj)
		{
			Document document = null;
			bool flag = obj is string;
			if (flag)
			{
				bool flag2 = (string)obj == "local";
				if (flag2)
				{
					document = this.SelectedLocalDocument;
				}
				else
				{
					bool flag3 = (string)obj == "usb";
					if (flag3)
					{
						document = this.SelectedUSBDocument;
					}
				}
			}
			bool flag4 = document != null;
			if (flag4)
			{
				Keyboard keyboard = new Keyboard(LocalizationManager.Localize("msgRenameAppl"), document.DisplayedName, KeyboardType.Application);
				bool flag5 = (string)obj == "local";
				if (flag5)
				{
					keyboard.TextInputKeyboard += this.keyboard_TextInputKeyboard_RenameLocalFile;
				}
				else
				{
					keyboard.TextInputKeyboard += this.keyboard_TextInputKeyboard_RenameUSBFile;
				}
				keyboard.ShowDialog();
			}
		}

		// Token: 0x060010C7 RID: 4295 RVA: 0x0006754C File Offset: 0x0006574C
		private void keyboard_TextInputKeyboard_RenameLocalFile(object sender, KeyboardInputEventArgs e)
		{
			Keyboard keyboard = sender as Keyboard;
			bool flag = keyboard != null;
			if (flag)
			{
				keyboard.TextInputKeyboard -= this.keyboard_TextInputKeyboard_RenameLocalFile;
				this.RenameFile(this.SelectedLocalDocument, this.documentsPath, e.Text);
			}
		}

		// Token: 0x060010C8 RID: 4296 RVA: 0x00067598 File Offset: 0x00065798
		private void keyboard_TextInputKeyboard_RenameUSBFile(object sender, KeyboardInputEventArgs e)
		{
			Keyboard keyboard = sender as Keyboard;
			bool flag = keyboard != null;
			if (flag)
			{
				keyboard.TextInputKeyboard -= this.keyboard_TextInputKeyboard_RenameUSBFile;
				this.RenameFile(this.SelectedUSBDocument, this.usbPath, e.Text);
			}
		}

		// Token: 0x060010C9 RID: 4297 RVA: 0x000675E4 File Offset: 0x000657E4
		private void RenameFile(Document doc, string path, string newName)
		{
			bool flag = newName.Length != 0 && doc != null;
			if (flag)
			{
				string text = path + "\\" + newName + Definition.pdfFileEnding;
				bool flag2 = !File.Exists(text);
				if (flag2)
				{
					File.Move(doc.FilePath, text);
					this.GetDocuments();
				}
				else
				{
					Dialog.Show(LocalizationManager.Localize("txtError"), "File already exists.", DialogStyle.Error, DialogButtons.OK);
				}
			}
		}

		// Token: 0x060010CA RID: 4298 RVA: 0x00067658 File Offset: 0x00065858
		private void SurveyUSB(object sender, EventArgs e)
		{
			bool flag = !this.usbDirExists && Directory.Exists(this.usbPath);
			if (flag)
			{
				this.ShowUSBConnected = true;
				this.ShowUSBNotConnected = false;
				this.GetDocuments();
			}
			else
			{
				bool flag2 = this.usbDirExists && !Directory.Exists(this.usbPath);
				if (flag2)
				{
					this.ShowUSBConnected = false;
					this.ShowUSBNotConnected = true;
					this.GetDocuments();
				}
			}
		}

		// Token: 0x060010CB RID: 4299 RVA: 0x000676D4 File Offset: 0x000658D4
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x060010CC RID: 4300 RVA: 0x00067701 File Offset: 0x00065901
		private void CleanUp(object obj)
		{
			this.timerUSB.Stop();
		}

		// Token: 0x04000B1C RID: 2844
		private BuiltInPriviledges rightDeleteFiles = BuiltInPriviledges.Admin;

		// Token: 0x04000B1D RID: 2845
		private Document selectedUSBDocument;

		// Token: 0x04000B1E RID: 2846
		private Document selectedLocalDocument;

		// Token: 0x04000B1F RID: 2847
		private string documentsPath;

		// Token: 0x04000B20 RID: 2848
		private string usbPath;

		// Token: 0x04000B21 RID: 2849
		private DispatcherTimer timerUSB;

		// Token: 0x04000B22 RID: 2850
		private bool usbDirExists;
	}
}
